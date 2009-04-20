Option Strict On
Option Explicit On

Imports QuakeStats

Partial Class site_QuakeStats
    Inherits System.Web.UI.MasterPage

    Private mobjStats As clsStatsManager

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Get stats manager
        mobjStats = CType(Application("StatsManager"), clsStatsManager)

        'Set the most recent game URL to point to the most recent game
        lnkLastGame.NavigateUrl = "~/pages/stats/GameStats.aspx?gameid=" & mobjStats.GetLastGameID()
    End Sub
End Class

