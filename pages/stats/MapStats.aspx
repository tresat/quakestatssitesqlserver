<%@ Page Title="Quake Stats - Per Map Statistics" Language="VB" MasterPageFile="~/pages/QuakeStats.master" AutoEventWireup="false" CodeFile="MapStats.aspx.vb" Inherits="pages_stats_MapStats" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" Runat="Server">
    <link rel="stylesheet" type="text/css" media="screen" href="../../static/gridThemes/basic/grid.css"/> 
    <script src="../../static/js/jquery/jquery-1.3.min.js" type="text/javascript" language="javascript"></script>
    <script src="../../static/js/pages/MapStats.js" type="text/javascript" language="javascript"></script>
</asp:Content>
<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" Runat="Server">
    <div id="main">
        <h1>Statistics for Map: <asp:Literal ID="litMap" runat="server" /></h1>
        <hr />
    </div>
    <div id="hidden">
        <input id="hdnMapID" class="mapID" type="hidden" runat="server" />
    </div>
</asp:Content>

