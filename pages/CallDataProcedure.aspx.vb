Option Explicit On
Option Strict On

Imports Security
Imports System.Collections.Generic

Partial Class pages_CallDataProcedure
    Inherits System.Web.UI.Page

    Private mobjCaller As clsDataProcedureCaller

    ''' <summary>
    ''' Handles the PreInit event of the Page control.  PreInit
    ''' is where we verify all required params are present, so 
    ''' that we quit processing ASAP if they ain't.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        'Ensure presence of required params
        If Request.Form("proc") Is Nothing OrElse _
                    Request.Form("idrow") Is Nothing OrElse _
                    Request.Form("rows") Is Nothing OrElse _
                    Request.Form("page") Is Nothing OrElse _
                    Request.Form("sidx") Is Nothing OrElse _
                    Request.Form("sord") Is Nothing Then
            Throw New Exception("Required parameter absent.")
        End If
    End Sub

    ''' <summary>
    ''' Handles the Init event of the Page control.  Here is where we'll
    ''' do the call to the procedure caller, and get the XML data, which
    ''' we'll inject into the response.  After this event fires, the response
    ''' type will be set to text/XML and the result will be written to the
    ''' response stream.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim lstArgs As New List(Of String)
        Dim intIdx As Integer = 0
        Dim blnDone As Boolean = False
        Dim strXML As String

        'Get data procedure security manager
        mobjCaller = CType(Application("DataProcedureCaller"), clsDataProcedureCaller)

        'Get all the procedure arguments into a string array for passing
        Do While Not blnDone
            If Request.Form("arg" & intIdx) IsNot Nothing Then
                lstArgs.Add(Request.Form("arg" & intIdx))
                intIdx += 1
            Else
                blnDone = True
            End If
        Loop

        'Get the data XML string by calling the procedure
        strXML = mobjCaller.CallProcedure(Request.Form("proc"), _
                lstArgs, _
                CInt(Request.Form("idrow")), _
                CLng(Request.Form("page")), _
                CLng(Request.Form("rows")), _
                CInt(Request.Form("sidx")), _
                Request.Form("sord"))

        Response.ContentType = "text/xml;charset=utf-8"

        Response.Write("<?xml version='1.0'?>")
        Response.Write(strXML)
    End Sub
End Class
