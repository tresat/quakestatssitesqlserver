<%@ Page Language="VB" AutoEventWireup="false" CodeFile="gChartManagerTest.aspx.vb" Inherits="tests_gChart_gChartManagerTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Testing Construction of Chart Manager js class</title>
    
    <script type="text/javascript" src="../../static/js/jquery/jquery.js"></script>
    <script type="text/javascript" src="../../static/js/jquery/gChart/jquery.gchart.js"></script>
    <script type="text/javascript" src="../../static/js/jsClass/class.js"></script>
    <script type="text/javascript" src="../../static/js/json2.js"></script>
    <script type="text/javascript" src="../../static/js/visualization/colorManager.js"></script>
    <script type="text/javascript" src="../../static/js/visualization/gChartManagerBase.js"></script>
    <script type="text/javascript" src="../../static/js/visualization/gChartManagerPie.js"></script>
    <script type="text/javascript" src="gChartManagerTest.js"></script>
</head>
<body>
    <form id="frmMain" runat="server">
        <div>
            <h2>Kills by Weapon</h2>
            <div id="killsByWeaponPie" />
        </div>
    </form>
</body>
</html>
