Option Explicit On
Option Strict On

Imports System.Data.SqlClient
Imports System.Collections.Generic

Namespace Security
    Public Class clsDataProcedureCaller
#Region "Inner Types"
        Protected Enum ProcedureType
            GridProcedure = 1
            ChartProcedure = 2
        End Enum

        Public Structure ChartData
            Public Label As String
            Public Data As Object

            Public Sub New(ByVal pstrLabel As String, ByVal pobjData As Object)
                Label = pstrLabel
                Data = pobjData
            End Sub
        End Structure
#End Region

#Region "Member Vars"
        Private mstrStatsConnectionString As String
#End Region

#Region "Constructors"
        Public Sub New(ByVal pstrStatsConnectionString As String)
            mstrStatsConnectionString = pstrStatsConnectionString
        End Sub
#End Region

#Region "Public Functionality"
        Public Function CallChartProcedure(ByVal pstrProcedureName As String, _
                                      ByVal plstArgs As List(Of String)) As List(Of ChartData)
            Dim sqlcmdCall As SqlCommand
            Dim lstData As New List(Of ChartData)

            Using cxnDB As SqlConnection = GetConnection()
                'Ensure current user has access to the called data procedure
                If Not VerifyUserCanAccessProcedure(pstrProcedureName, ProcedureType.ChartProcedure) Then
                    Throw New Exception("User: " & Membership.GetUser.UserName & " does not have access to procedure: " & pstrProcedureName)
                End If

                'Build sql string.  Add 1 param for each in pastringargs.  We'll 
                'call the argX, where X will count from 0.  They() 'll be populated
                'in the same order they came across the AJAX call.
                'Create the command and add each of the procedure parameters.
                sqlcmdCall = New SqlCommand(pstrProcedureName, cxnDB)
                sqlcmdCall.CommandType = Data.CommandType.StoredProcedure
                For intIdx As Integer = 0 To plstArgs.Count - 1
                    sqlcmdCall.Parameters.AddWithValue("pProcSpecificParam" + CStr(intIdx), CInt(plstArgs(intIdx)))
                Next

                'Call the procedure
                Using reader As SqlDataReader = sqlcmdCall.ExecuteReader
                    'The first resultset is the particular data of the proc
                    While reader.Read()
                        lstData.Add(New ChartData(CStr(reader("WeaponName")), reader("Kills")))
                    End While
                End Using
            End Using

            Return lstData
        End Function

        Public Function CallGridProcedure(ByVal pstrProcedureName As String, _
                                      ByVal plstArgs As List(Of String), _
                                      ByVal pintIDCol As Integer, _
                                      ByVal plngPageNo As Long, _
                                      ByVal plngRowCount As Long, _
                                      ByVal pintSortCol As Integer, _
                                      ByVal pstrSortOrder As String) As String
            Dim sqlcmdCall As SqlCommand
            Dim strSQL As String
            Dim strRowXML As String
            Dim lstRows As New List(Of String)
            Dim strResultXML As String
            Dim lngStart As Long
            Dim lngRecordCount As Long
            Dim lngTotalPages As Long
            Dim parRecordCount As SqlParameter

            Using cxnDB As SqlConnection = GetConnection()
                'Ensure current user has access to the called data procedure
                If Not VerifyUserCanAccessProcedure(pstrProcedureName, ProcedureType.GridProcedure) Then
                    Throw New Exception("User: " & Membership.GetUser.UserName & " does not have access to procedure: " & pstrProcedureName)
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
                sqlcmdCall = New SqlCommand(strSQL, cxnDB)
                sqlcmdCall.CommandType = Data.CommandType.StoredProcedure
                For intIdx As Integer = 0 To plstArgs.Count - 1
                    sqlcmdCall.Parameters.AddWithValue("pProcSpecificParam" + CStr(intIdx), CInt(plstArgs(intIdx)))
                Next
                'Names have to be the same here
                sqlcmdCall.Parameters.AddWithValue("pSortCol", pintSortCol)
                sqlcmdCall.Parameters.AddWithValue("pSortAsc", UCase$(pstrSortOrder).Equals("ASC"))
                sqlcmdCall.Parameters.AddWithValue("pStartRow", lngStart)
                sqlcmdCall.Parameters.AddWithValue("pLimitRows", plngRowCount)

                parRecordCount = sqlcmdCall.CreateParameter()
                parRecordCount.DbType = Data.DbType.Int32
                parRecordCount.Direction = Data.ParameterDirection.Output
                parRecordCount.ParameterName = "pTotalRows"

                sqlcmdCall.Parameters.Add(parRecordCount)

                'Call the procedure
                Using reader As SqlDataReader = sqlcmdCall.ExecuteReader
                    'The first resultset is the particular data of the proc
                    While reader.Read()
                        'Build each row into a Row XML string, pop that string into 
                        'the Rows list.  First add id column to xml
                        strRowXML = "<row id='" & CStr(reader(pintIDCol)) & "'>"
                        'Iterate through each column to add them to the list
                        For intIdx As Integer = 0 To reader.FieldCount - 1
                            strRowXML &= "<cell><![CDATA[" & CStr(IIf(IsDBNull(reader(intIdx)), String.Empty, reader(intIdx))) & "]]></cell>"
                        Next
                        strRowXML &= "</row>"
                        lstRows.Add(strRowXML)
                    End While
                End Using

                lngRecordCount = CLng(parRecordCount.Value)
                lngTotalPages = CLng(Math.Ceiling(lngRecordCount / plngRowCount))

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
                    strResultXML = CallGridProcedure(pstrProcedureName, plstArgs, pintIDCol, lngTotalPages, plngRowCount, pintSortCol, pstrSortOrder)
                End If
            End Using

            Return strResultXML
        End Function
#End Region

#Region "Private Helpers"
        Private Function GetConnection() As SqlConnection
            Dim cxnStatsDB As SqlConnection

            cxnStatsDB = New SqlConnection(mstrStatsConnectionString)
            If Not cxnStatsDB Is Nothing Then
                cxnStatsDB.Open()
                If Not cxnStatsDB.State = Data.ConnectionState.Open Then
                    Throw New Exception("Problem connecting to db at: " & mstrStatsConnectionString)
                End If
            End If

            Return cxnStatsDB
        End Function

        Private Function VerifyUserCanAccessProcedure(ByVal pstrProcedureName As String, ByVal penuProcedureType As ProcedureType) As Boolean
            Dim astrRoles() As String = Roles.GetRolesForUser(Membership.GetUser().UserName)
            Dim strSQL As String
            Dim sqlcmdVerify As SqlCommand
            Dim blnResult As Boolean = False

            strSQL = "SELECT Count(*) " & _
                    "FROM SiteData.DataProcedure dp " & _
                    "   INNER JOIN SiteData.DataProcedureSecurity dps ON dps.fkDataProcedureID = dp.DataProcedureID " & _
                    "WHERE dp.DataProcedureName = @Name " & _
                    "   AND dps.RoleName = @Role "
            Select Case penuProcedureType
                Case ProcedureType.GridProcedure
                    strSQL &= "AND dp.IsGridProcedure = 1 "
                Case ProcedureType.ChartProcedure
                    strSQL &= "AND dp.IsChartProcedure = 1 "
                Case Else
                    Throw New Exception("Unknown proc type: " & penuProcedureType)
            End Select

            Using cxnDB As SqlConnection = GetConnection()
                For Each strRole As String In astrRoles
                    sqlcmdVerify = New SqlCommand(strSQL, cxnDB)
                    sqlcmdVerify.Parameters.AddWithValue("Name", pstrProcedureName)
                    sqlcmdVerify.Parameters.AddWithValue("Role", strRole)

                    If CInt(sqlcmdVerify.ExecuteScalar) > 0 Then
                        blnResult = True
                        Exit For
                    End If
                Next
            End Using

            Return blnResult
        End Function
#End Region
    End Class
End Namespace
