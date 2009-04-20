Option Explicit On
Option Strict On

Imports System.Configuration
Imports MySql.Data.MySqlClient

Partial Class pages_tests_jqGridData
    Inherits System.Web.UI.Page

    Private mintPage As Integer
    Private mintLimit As Integer
    Private mstrSidx As String
    Private mstrSord As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' to the url parameter are added 4 parameters as described in colModel
        ' we should get these parameters to construct the needed query
        ' Since we specify in the options of the grid that we will use a GET method 
        ' we should use the appropriate command to obtain the parameters. 
        ' In our case this is $_GET. If we specify that we want to use post 
        ' we should use $_POST. Maybe the better way is to use $_REQUEST, which
        ' contain both the GET and POST variables. For more information refer to php documentation.
        ' Get the requested page. By default grid sets this to 1. 
        mintPage = Integer.Parse(Request.QueryString("page"))

        ' get how many rows we want to have into the grid - rowNum parameter in the grid 
        mintLimit = Integer.Parse(Request.QueryString("rows"))

        ' get index row - i.e. user click to sort. At first time sortname parameter -
        ' after that the index from colModel 
        mstrSidx = Request.QueryString("sidx")

        ' sorting order - at first time sortorder 
        mstrSord = Request.QueryString("sord")

        ' we should set the appropriate header information
        'If Request.Headers("HTTP_ACCEPT").Contains("application/xhtml+xml") Then
        '    Response.ContentType = "application/xhtml+xml;charset=utf-8"
        'Else
        '    Response.ContentType = "text/xml;charset=utf-8"
        'End If

        Response.ContentType = "text/xml;charset=utf-8"

        Response.Write("<?xml version='1.0'?>")
        Response.Write(GetXML)
    End Sub

    Private Function GetXML() As String
        Dim intCount As Integer
        Dim intTotalPages As Integer
        Dim intStart As Integer
        Dim cxnDB As New MySqlConnection(ConfigurationManager.ConnectionStrings("TestDB").ConnectionString.ToString)
        Dim strResult As String

        cxnDB.Open()

        ' calculate the number of rows for the query. We need this for paging the result 
        Using cmdGet As New MySqlCommand("SELECT COUNT(*) AS count FROM invheader", cxnDB)
            intCount = CInt(cmdGet.ExecuteScalar)
        End Using

        ' calculate the total pages for the query 
        If intCount > 0 Then
            intTotalPages = CInt(Math.Ceiling(intCount / mintLimit))
        Else
            intTotalPages = 0
        End If

        ' if for some reasons the requested page is greater than the total 
        ' set the requested page to total page 
        If mintPage > intTotalPages Then mintPage = intTotalPages

        ' calculate the starting position of the rows 
        intStart = mintLimit * mintPage - mintLimit

        ' if for some reasons start position is negative set it to 0 
        ' typical case is that the user type 0 for the requested page 
        If intStart < 0 Then intStart = 0

        ' the actual query for the grid data 
        Using cmdGet As New MySqlCommand("SELECT invid, invdate, amount, tax, total, note FROM invheader ORDER BY " & mstrSidx & " " & mstrSord & " LIMIT " & intStart & " , " & mintLimit, cxnDB)
            Using reader As MySqlDataReader = cmdGet.ExecuteReader
                strResult = "<rows>"
                strResult &= "<page>" & mintPage & "</page>"
                strResult &= "<total>" & intTotalPages & "</total>"
                strResult &= "<records>" & intCount & "</records>"
                Do While reader.Read
                    strResult &= "<row id='" & reader("invid").ToString & "'>"
                    strResult &= "<cell>" & reader("invid").ToString & "</cell>"
                    strResult &= "<cell>" & reader("invdate").ToString & "</cell>"
                    strResult &= "<cell>" & reader("amount").ToString & "</cell>"
                    strResult &= "<cell>" & reader("tax").ToString & "</cell>"
                    strResult &= "<cell>" & reader("total").ToString & "</cell>"
                    strResult &= "<cell><![CDATA[" & reader("note").ToString & "]]></cell>"
                    strResult &= "</row>"
                Loop
                strResult &= "</rows>"
            End Using
        End Using

        cxnDB.Close()

        Return strResult
    End Function
End Class
