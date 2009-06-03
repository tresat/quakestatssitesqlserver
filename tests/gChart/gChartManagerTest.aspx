<%@ Page Language="VB" AutoEventWireup="false" CodeFile="gChartManagerTest.aspx.vb" Inherits="tests_gChart_gChartManagerTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Testing Construction of Chart Manager js class</title>
    
    <link type="text/css" rel="Stylesheet" media="screen" href="../../static/css/QuakeStats.css" />
    
    <script type="text/javascript" src="../../static/js/jquery/jquery.js"></script>
    <script type="text/javascript" src="../../static/js/jquery/gChart/jquery.gchart.js"></script>
    <script type="text/javascript" src="../../static/js/jsClass/class.js"></script>
    <script type="text/javascript" src="../../static/js/json/json2.js"></script>
    <script type="text/javascript" src="../../static/js/jquery/curvycorners/jquery.curvycorners.min.js"></script>
    
    <script type="text/javascript" src="../../static/js/visualization/colorManager.js"></script>
    <script type="text/javascript" src="../../static/js/visualization/gChartManagerBase.js"></script>
    <script type="text/javascript" src="../../static/js/visualization/gChartManagerPie.js"></script>
    
    <script type="text/javascript" src="../../static/js/QuakeStats.js"></script>
    <script type="text/javascript" src="gChartManagerTest.js"></script> 
</head>
<body>
    <form id="frmMain" runat="server">
        <div>
            <h1>Some Test Pie Charts</h1>
            <br />
            <div class="pieContainer" >
                <div id="killsByWeaponPie" class="pieChart" >
                    
                </div>
            </div>
        </div>
    </form>
</body>
</html>
