Option Strict On
Option Explicit On

Imports QuakeStats

Partial Class site_stats_GameStats
    Inherits System.Web.UI.Page

    Private mlngGameID As Long
    Private mobjStats As clsStatsManager

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        'Check for the presence of a valid game id in the query string
        If Request.QueryString("gameid") IsNot Nothing Then
            'Check that the gameid is a valid long
            Try
                mlngGameID = CLng(Request.QueryString("gameid"))
            Catch ex As Exception
                'Send them back home
                Response.Redirect("~/pages/Home.aspx", True)
            End Try
        Else
            'Send them back home
            Response.Redirect("~/pages/Home.aspx", True)
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Get stats manager
        mobjStats = CType(Application("StatsManager"), clsStatsManager)

        'Check if the gameid is a valid id
        If Not mobjStats.IsValidGameID(mlngGameID) Then
            'Send them back home
            Response.Redirect("~/pages/Home.aspx", True)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Set hidden field to correct gameid (for grid use)
        hdnGameID.Value = Request.QueryString("gameid")

        'Lookup map name
        lnkMap.Text = mobjStats.GetMapNameForGame(CLng(Request.QueryString("gameid")), True)
        lnkMap.NavigateUrl = "~/pages/stats/MapStats.aspx?mapid=" & mobjStats.GetMapIDForGame(CLng(Request.QueryString("gameid")))

        SetupGameLinks()
    End Sub

    Private Sub SetupGameLinks()
        Dim lngNextGameID As Long = mobjStats.GetNextGameID(mlngGameID)
        Dim lngPreviousGameID As Long = mobjStats.GetPreviousGameID(mlngGameID)
        Dim lngFirstGameID As Long = mobjStats.GetFirstGameID()
        Dim lngLastGameID As Long = mobjStats.GetLastGameID()

        If lngNextGameID <> 0 Then
            lnkNextGame.NavigateUrl = "~/pages/stats/GameStats.aspx?gameid=" & lngNextGameID
        Else
            lnkNextGame.Visible = False
        End If

        If lngPreviousGameID <> 0 Then
            lnkPreviousGame.NavigateUrl = "~/pages/stats/GameStats.aspx?gameid=" & lngPreviousGameID
        Else
            lnkPreviousGame.Visible = False
        End If

        If lngFirstGameID <> 0 Then
            lnkFirstGame.NavigateUrl = "~/pages/stats/GameStats.aspx?gameid=" & lngFirstGameID
        Else
            lnkFirstGame.Visible = False
        End If

        If lngLastGameID <> 0 Then
            lnkLastGame.NavigateUrl = "~/pages/stats/GameStats.aspx?gameid=" & lngLastGameID
        Else
            lnkLastGame.Visible = False
        End If
    End Sub
End Class
