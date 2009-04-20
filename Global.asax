<%@ Application Language="VB" %>

<script runat="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        ReadyStatsManager()
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
       
    Sub ReadyStatsManager()
        Dim strConnectionString As String
        Dim objStats As QuakeStats.clsStatsManager
        Dim objDataProcCaller As Security.clsDataProcedureCaller
        
        'Grab the connection string for the stats database
        strConnectionString = ConfigurationManager.ConnectionStrings("StatsDB").ConnectionString.ToString()
        
        'Then, create a new stats manager object and pop it into the Application object
        objStats = New QuakeStats.clsStatsManager(strConnectionString)
        Application.Add("StatsManager", objStats)
        
        'Next, create a new data procedure caller object and pop it in to Application
        objDataProcCaller = New Security.clsDataProcedureCaller(strConnectionString)
        Application.Add("DataProcedureCaller", objDataProcCaller)
    End Sub
</script>