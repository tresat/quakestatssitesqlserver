Option Explicit On
Option Strict On

Imports QuakeStats

Partial Class pages_stats_MapStats
    Inherits System.Web.UI.Page

    Private mlngMapID As Long
    Private mobjStats As clsStatsManager

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        'Check for the presence of a valid map id in the query string
        If Request.QueryString("mapid") IsNot Nothing Then
            'Check that the mapid is a valid long
            Try
                mlngMapID = CLng(Request.QueryString("mapid"))
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

        'Check if the mapid is a valid id
        If Not mobjStats.IsValidMapID(mlngMapID) Then
            'Send them back home
            Response.Redirect("~/pages/Home.aspx", True)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Set hidden field to correct mapid (for grid use)
        hdnMapID.Value = Request.QueryString("mapid")

        'Lookup map name
        litMap.Text = mobjStats.GetMapNameForMap(CLng(Request.QueryString("mapid")), True)
    End Sub
End Class
