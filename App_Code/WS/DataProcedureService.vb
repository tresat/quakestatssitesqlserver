Option Explicit On
Option Strict On

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Collections.Generic
Imports Security

Namespace WS

    <System.Web.Script.Services.ScriptService()> _
    <WebService(Namespace:="http://tomtresansky.org/QuakeStats/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Public Class DataProcedureService
        Inherits System.Web.Services.WebService

        <WebMethod(BufferResponse:=True, CacheDuration:=30)> _
        Public Function HelloWorld() As String
            Return "Hello World"
        End Function

        <WebMethod(BufferResponse:=True, CacheDuration:=30)> _
        Public Function CallChartProcedure(ByVal pstrProcedureName As String, _
                                      ByVal plstArgs As List(Of String)) As List(Of clsDataProcedureCaller.ChartData)
            Dim objDataProcCaller As clsDataProcedureCaller = CType(Application("DataProcedureCaller"), clsDataProcedureCaller)

            Return objDataProcCaller.CallChartProcedure(pstrProcedureName, plstArgs)
        End Function
    End Class
End Namespace
