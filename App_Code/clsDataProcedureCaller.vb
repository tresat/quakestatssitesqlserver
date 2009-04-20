Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Collections.Generic

Namespace Security
    Public Class clsDataProcedureCaller
        Private mstrStatsConnectionString As String

        Public Sub New(ByVal pstrStatsConnectionString As String)
            mstrStatsConnectionString = pstrStatsConnectionString
        End Sub

        Public Function CallProcedure(ByVal pstrProcedureName As String, _
                                      ByVal plstArgs As List(Of String), _
                                      ByVal pintIDRow As Integer, _
                                      ByVal plngPageNo As Long, _
                                      ByVal plngRowCount As Long, _
                                      ByVal pintSortCol As Integer, _
                                      ByVal pstrSortOrder As String) As String
            Dim sqlcmdCall As MySqlCommand
            Dim strSQL As String
            Dim strRowXML As String
            Dim lstRows As New List(Of String)
            Dim strResultXML As String
            Dim lngStart As Long
            Dim lngRecordCount As Long
            Dim lngTotalPages As Long

            Using cxnDB As MySqlConnection = GetConnection()
                'Ensure current user has access to the called data procedure
                If Not VerifyUserCanAccessProcedure(pstrProcedureName) Then
                    Throw New Exception("User: " & Membership.GetUser().UserName & " does not have access to procedure: " & pstrProcedureName)
                End If

                'Calculate the starting position of the rows 
                lngStart = plngRowCount * (plngPageNo - 1)

                'If for some reasons start position is negative set it to 0 
                'typical case is that the user type 0 for the requested page 
                If lngStart < 0 Then lngStart = 0

                'Build sql string.  Add 1 param for each in pastringargs.  We'll 
                'call the argX, where X will count from 0.  They() 'll later be populated
                'in the same order they came across the AJAX call.
                strSQL = pstrProcedureName

                'Create the command and add each of the procedure parameters.
                sqlcmdCall = New MySqlCommand(strSQL, cxnDB)
                sqlcmdCall.CommandType = Data.CommandType.StoredProcedure
                For intIdx As Integer = 0 To plstArgs.Count - 1
                    sqlcmdCall.Parameters.AddWithValue("?pGameID", CInt(plstArgs(intIdx)))
                Next
                'Names have to be the same here
                sqlcmdCall.Parameters.AddWithValue("?pSortCol", pintSortCol)
                sqlcmdCall.Parameters.AddWithValue("?pSortAsc", UCase$(pstrSortOrder).Equals("ASC"))
                sqlcmdCall.Parameters.AddWithValue("?pStartRow", lngStart)
                sqlcmdCall.Parameters.AddWithValue("?pLimitRows", plngRowCount)

                'Call the procedure
                Using reader As MySqlDataReader = sqlcmdCall.ExecuteReader
                    'The first resultset is the particular data of the proc
                    While reader.Read()
                        'Build each row into a Row XML string, pop that string into 
                        'the Rows list.  First add id column to xml
                        strRowXML = "<row id='" & CStr(reader(pintIDRow)) & "'>"
                        'Iterate through each column to add them to the list
                        For intIdx As Integer = 0 To reader.FieldCount - 1
                            strRowXML &= "<cell><![CDATA[" & CStr(reader(intIdx)) & "]]></cell>"
                        Next
                        strRowXML &= "</row>"
                        lstRows.Add(strRowXML)
                    End While

                    'Switch to the second resultset, to retrieve the count and calc total pages
                    reader.NextResult()
                    reader.Read()
                    lngRecordCount = CLng(reader("RecordCount"))
                    lngTotalPages = CLng(Math.Ceiling(lngRecordCount / plngRowCount))
                End Using

                'Now build the resultant XML string
                strResultXML = "<rows>"
                strResultXML &= "<page>" & plngPageNo & "</page>"
                strResultXML &= "<total>" & lngTotalPages & "</total>"
                strResultXML &= "<records>" & lngRecordCount & "</records>"
                For Each strCurrRowXML As String In lstRows
                    strResultXML &= strCurrRowXML
                Next
                strResultXML &= "</rows>"

                'If for some reasons the requested page is greater than the total 
                'then call again, using last page
                If plngPageNo > lngTotalPages Then
                    strResultXML = CallProcedure(pstrProcedureName, plstArgs, pintIDRow, lngTotalPages, plngRowCount, pintSortCol, pstrSortOrder)
                End If
            End Using

            Return strResultXML
        End Function

        Private Function GetConnection() As MySqlConnection
            Dim cxnStatsDB As MySqlConnection

            cxnStatsDB = New MySql.Data.MySqlClient.MySqlConnection(mstrStatsConnectionString)
            If Not cxnStatsDB Is Nothing Then
                cxnStatsDB.Open()
                If Not cxnStatsDB.State = Data.ConnectionState.Open Then
                    Throw New Exception("Problem connecting to db at: " & mstrStatsConnectionString)
                End If
            End If

            Return cxnStatsDB
        End Function

        Private Function VerifyUserCanAccessProcedure(ByVal pstrProcedureName As String) As Boolean
            Dim astrRoles() As String = Roles.GetRolesForUser(Membership.GetUser().UserName)
            Dim strSQL As String = "SELECT Count(*) " & _
                                    "FROM dataprocedure dp " & _
                                    "   INNER JOIN dataproceduresecurity dps ON dps.fkDataProcedureID = dp.DataProcedureID " & _
                                    "WHERE dp.DataProcedureName = ?name " & _
                                    "   AND dps.Role = ?role "
            Dim sqlcmdVerify As MySqlCommand
            Dim blnResult As Boolean = False

            Using cxnDB As MySqlConnection = GetConnection()
                For Each strRole As String In astrRoles
                    sqlcmdVerify = New MySqlCommand(strSQL, cxnDB)
                    sqlcmdVerify.Parameters.AddWithValue("?name", pstrProcedureName)
                    sqlcmdVerify.Parameters.AddWithValue("?role", strRole)

                    If CInt(sqlcmdVerify.ExecuteScalar) > 0 Then
                        blnResult = True
                        Exit For
                    End If
                Next
            End Using

            Return blnResult
        End Function
    End Class
End Namespace
