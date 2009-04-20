Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient

Namespace QuakeStats
    Public Class clsStatsManager
        Private mstrStatsConnectionString As String

        Public Sub New(ByVal pstrStatsConnectionString As String)
            mstrStatsConnectionString = pstrStatsConnectionString
        End Sub

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

        Public Function GetLastGameID() As Long
            Dim sqlcmdGet As MySqlCommand
            Dim strSQL As String
            Dim lngResult As Long

            Using cxnDB As MySqlConnection = GetConnection()
                strSQL = "SELECT Max(GameID) FROM games"
                sqlcmdGet = New MySqlCommand(strSQL, cxnDB)

                lngResult = CLng(sqlcmdGet.ExecuteScalar)
            End Using

            Return lngResult
        End Function

        Public Function GetFirstGameID() As Long
            Dim sqlcmdGet As MySqlCommand
            Dim strSQL As String
            Dim lngResult As Long

            Using cxnDB As MySqlConnection = GetConnection()
                strSQL = "SELECT Min(GameID) FROM games"
                sqlcmdGet = New MySqlCommand(strSQL, cxnDB)

                lngResult = CLng(sqlcmdGet.ExecuteScalar)
            End Using

            Return lngResult
        End Function

        Public Function IsValidGameID(ByVal plngGameID As Long) As Boolean
            Dim sqlcmdGet As MySqlCommand
            Dim strSQL As String
            Dim blnResult As Boolean

            Using cxnDB As MySqlConnection = GetConnection()
                strSQL = "SELECT 1 FROM games WHERE GameID = ?GameID"
                sqlcmdGet = New MySqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("?GameID", plngGameID)

                blnResult = (sqlcmdGet.ExecuteScalar IsNot Nothing)
            End Using

            Return blnResult
        End Function

        Public Function IsValidMapID(ByVal plngMapID As Long) As Boolean
            Dim sqlcmdGet As MySqlCommand
            Dim strSQL As String
            Dim blnResult As Boolean

            Using cxnDB As MySqlConnection = GetConnection()
                strSQL = "SELECT 1 FROM maps WHERE MapID = ?MapID"
                sqlcmdGet = New MySqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("?MapID", plngMapID)

                blnResult = (sqlcmdGet.ExecuteScalar IsNot Nothing)
            End Using

            Return blnResult
        End Function

        Public Function GetNextGameID(ByVal plngGameID As Long) As Long
            Dim sqlcmdGet As MySqlCommand
            Dim strSQL As String
            Dim lngResult As Long

            Using cxnDB As MySqlConnection = GetConnection()
                strSQL = "SELECT IfNull(fkNextGameID, 0) FROM games WHERE GameID = ?GameID"
                sqlcmdGet = New MySqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("?GameID", plngGameID)

                lngResult = CLng(sqlcmdGet.ExecuteScalar)
            End Using

            Return lngResult
        End Function

        Public Function GetPreviousGameID(ByVal plngGameID As Long) As Long
            Dim sqlcmdGet As MySqlCommand
            Dim strSQL As String
            Dim lngResult As Long

            Using cxnDB As MySqlConnection = GetConnection()

                strSQL = "SELECT IfNull(fkPreviousGameID, 0) FROM games WHERE GameID = ?GameID"
                sqlcmdGet = New MySqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("?GameID", plngGameID)

                lngResult = CLng(sqlcmdGet.ExecuteScalar)
            End Using

            Return lngResult
        End Function

        Public Function GetMapIDForGame(ByVal plngGameID As Long) As Long
            Dim sqlcmdGet As MySqlCommand
            Dim strSQL As String
            Dim lngResult As Long

            Using cxnDB As MySqlConnection = GetConnection()
                strSQL = "SELECT g.fkMapID " & _
                        "FROM games g " & _
                        "WHERE g.GameID = ?GameID "

                sqlcmdGet = New MySqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("?GameID", plngGameID)

                lngResult = CLng(sqlcmdGet.ExecuteScalar)
            End Using

            Return lngResult
        End Function

        Public Function GetMapNameForMap(ByVal plngMapID As Long, Optional ByVal pblnShowMapFileName As Boolean = False) As String
            Dim sqlcmdGet As MySqlCommand
            Dim strSQL As String
            Dim strResult As String

            Using cxnDB As MySqlConnection = GetConnection()
                strSQL = "SELECT spGetMapName(m.MapID, " & CStr(IIf(pblnShowMapFileName, 1, 0)) & ") " & _
                        "FROM maps m " & _
                        "WHERE m.MapID = ?MapID "

                sqlcmdGet = New MySqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("?MapID", plngMapID)

                strResult = CStr(sqlcmdGet.ExecuteScalar)
            End Using

            Return strResult
        End Function

        Public Function GetMapNameForGame(ByVal plngGameID As Long, Optional ByVal pblnShowMapFileName As Boolean = False) As String
            Dim sqlcmdGet As MySqlCommand
            Dim strSQL As String
            Dim strResult As String

            Using cxnDB As MySqlConnection = GetConnection()
                strSQL = "SELECT spGetMapName(m.MapID, " & CStr(IIf(pblnShowMapFileName, 1, 0)) & ") " & _
                        "FROM games g " & _
                        "   INNER JOIN maps m ON g.fkMapID = m.MapID " & _
                        "WHERE g.GameID = ?GameID "

                sqlcmdGet = New MySqlCommand(strSQL, cxnDB)
                sqlcmdGet.Parameters.AddWithValue("?GameID", plngGameID)

                strResult = CStr(sqlcmdGet.ExecuteScalar)
            End Using

            Return strResult
        End Function
    End Class
End Namespace
