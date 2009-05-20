Option Explicit On
Option Strict On

Imports System.Data.SqlClient

Namespace QuakeStats
    Public Class clsStatsManager
        Private mstrStatsConnectionString As String

        Public Sub New(ByVal pstrStatsConnectionString As String)
            mstrStatsConnectionString = pstrStatsConnectionString
        End Sub

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

        Public Function GetLastGameID() As Long
            Dim sqlcmdGet As SqlCommand
            Dim strSQL As String
            Dim lngResult As Long

            Using cxnDB As SqlConnection = GetConnection()
                strSQL = "SELECT Max(g.GameID) " & _
                        "FROM CalculatedData.Game g "

                sqlcmdGet = New SqlCommand(strSQL, cxnDB)

                lngResult = CLng(sqlcmdGet.ExecuteScalar)
            End Using

            Return lngResult
        End Function

        Public Function GetFirstGameID() As Long
            Dim sqlcmdGet As SqlCommand
            Dim strSQL As String
            Dim lngResult As Long

            Using cxnDB As SqlConnection = GetConnection()
                strSQL = "SELECT Min(g.GameID) " & _
                        "FROM CalculatedData.Game g "

                sqlcmdGet = New SqlCommand(strSQL, cxnDB)

                lngResult = CLng(sqlcmdGet.ExecuteScalar)
            End Using

            Return lngResult
        End Function

        Public Function IsValidGameID(ByVal plngGameID As Long) As Boolean
            Dim sqlcmdGet As SqlCommand
            Dim strSQL As String
            Dim blnResult As Boolean

            Using cxnDB As SqlConnection = GetConnection()
                strSQL = "SELECT 1 " & _
                        "FROM CalculatedData.Game g " & _
                        "WHERE g.GameID = @GameID "

                sqlcmdGet = New SqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("GameID", plngGameID)

                blnResult = (sqlcmdGet.ExecuteScalar IsNot Nothing)
            End Using

            Return blnResult
        End Function

        Public Function IsValidMapID(ByVal plngMapID As Long) As Boolean
            Dim sqlcmdGet As SqlCommand
            Dim strSQL As String
            Dim blnResult As Boolean

            Using cxnDB As SqlConnection = GetConnection()
                strSQL = "SELECT 1 " & _
                        "FROM CalculatedData.Map m " & _
                        "WHERE m.MapID = @MapID "

                sqlcmdGet = New SqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("MapID", plngMapID)

                blnResult = (sqlcmdGet.ExecuteScalar IsNot Nothing)
            End Using

            Return blnResult
        End Function

        Public Function GetNextGameID(ByVal plngGameID As Long) As Long
            Dim sqlcmdGet As SqlCommand
            Dim strSQL As String
            Dim lngResult As Long

            Using cxnDB As SqlConnection = GetConnection()
                strSQL = "SELECT IsNull(g.fkNextGameID, 0) " & _
                        "FROM CalculatedData.Game g " & _
                        "WHERE g.GameID = @GameID "

                sqlcmdGet = New SqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("GameID", plngGameID)

                lngResult = CLng(sqlcmdGet.ExecuteScalar)
            End Using

            Return lngResult
        End Function

        Public Function GetPreviousGameID(ByVal plngGameID As Long) As Long
            Dim sqlcmdGet As SqlCommand
            Dim strSQL As String
            Dim lngResult As Long

            Using cxnDB As SqlConnection = GetConnection()

                strSQL = "SELECT IsNull(g.fkPreviousGameID, 0) " & _
                        "FROM CalculatedData.Game g " & _
                        "WHERE g.GameID = @GameID "

                sqlcmdGet = New SqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("GameID", plngGameID)

                lngResult = CLng(sqlcmdGet.ExecuteScalar)
            End Using

            Return lngResult
        End Function

        Public Function GetMapIDForGame(ByVal plngGameID As Long) As Long
            Dim sqlcmdGet As SqlCommand
            Dim strSQL As String
            Dim lngResult As Long

            Using cxnDB As SqlConnection = GetConnection()
                strSQL = "SELECT g.fkMapID " & _
                        "FROM CalculatedData.Game g " & _
                        "WHERE g.GameID = @GameID "

                sqlcmdGet = New SqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("GameID", plngGameID)

                lngResult = CLng(sqlcmdGet.ExecuteScalar)
            End Using

            Return lngResult
        End Function

        Public Function GetMapNameForMap(ByVal plngMapID As Long, Optional ByVal pblnShowMapFileName As Boolean = False) As String
            Dim sqlcmdGet As SqlCommand
            Dim strSQL As String
            Dim strResult As String

            Using cxnDB As SqlConnection = GetConnection()
                strSQL = "SELECT Calculations.fnGetMapName(m.MapID, " & CStr(IIf(pblnShowMapFileName, 1, 0)) & ") " & _
                        "FROM CalculatedData.Map m " & _
                        "WHERE m.MapID = @MapID "

                sqlcmdGet = New SqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("MapID", plngMapID)

                strResult = CStr(sqlcmdGet.ExecuteScalar)
            End Using

            Return strResult
        End Function

        Public Function GetMapNameForGame(ByVal plngGameID As Long, Optional ByVal pblnShowMapFileName As Boolean = False) As String
            Dim sqlcmdGet As SqlCommand
            Dim strSQL As String
            Dim strResult As String

            Using cxnDB As SqlConnection = GetConnection()
                strSQL = "SELECT Calculations.fnGetMapName(m.MapID, " & CStr(IIf(pblnShowMapFileName, 1, 0)) & ") " & _
                        "FROM CalculatedData.Game g " & _
                        "   INNER JOIN CalculatedData.Map m ON g.fkMapID = m.MapID " & _
                        "WHERE g.GameID = @GameID "

                sqlcmdGet = New SqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("GameID", plngGameID)

                strResult = CStr(sqlcmdGet.ExecuteScalar)
            End Using

            Return strResult
        End Function
    End Class
End Namespace
