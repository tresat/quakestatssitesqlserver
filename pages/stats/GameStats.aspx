<%@ Page Title="Quake Stats - Single Game Statistics" Language="VB" MasterPageFile="~/pages/QuakeStats.master" AutoEventWireup="false" CodeFile="GameStats.aspx.vb" Inherits="site_stats_GameStats" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="cphHead" Runat="Server">
    <link rel="stylesheet" type="text/css" media="screen" href="../../static/gridThemes/basic/grid.css"/> 
    <script src="../../static/js/jquery/jquery-1.3.min.js" type="text/javascript" language="javascript"></script>
    <script src="../../static/js/jquery/grid/jquery.jqGrid.js" type="text/javascript" language="javascript"></script>
    <script src="../../static/js/pages/GameStats.js" type="text/javascript" language="javascript"></script>
</asp:Content>
<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" Runat="Server">
    <div id="main">
        <h1>Statistics for Game: <%=hdnGameID.Value%></h1>
        <hr />
        Map: <asp:HyperLink ID="lnkMap" runat="server" Text="MAPNAME" NavigateUrl="MAPURL" />
         
        <hr />
        <asp:GridView ID="grdGame" runat="server" AutoGenerateColumns="False" 
            DataSourceID="dsGame" Caption="Game Result" Width="100px">
            <Columns>
                <asp:BoundField HeaderText="Red" DataField="RedScore" />
                <asp:BoundField HeaderText="Blue" DataField="BlueScore" />
                <asp:BoundField HeaderText="Complete?" DataField="IsComplete" ReadOnly="true" />
                <asp:BoundField HeaderText="Counts Towards Lifetime Stats?" DataField="IsCounted" ReadOnly="true" />
            </Columns>
        </asp:GridView>
        <hr />
        <table id="gridGame" class="scroll"></table> 
        <div id="pagerGame" class="scroll" style="text-align:center;"></div>
        <hr />
        <asp:HyperLink ID="lnkFirstGame" runat="server" Text="<<-- First Game" />
        <asp:HyperLink ID="lnkPreviousGame" runat="server" Text="<-- Previous Game" />
        <asp:HyperLink ID="lnkNextGame" runat="server" Text="Next Game -->" />
        <asp:HyperLink ID="lnkLastGame" runat="server" Text="Last Game -->>" />
    </div>
    <div class="hidden">
        <input id="hdnGameID" class="gameID" type="hidden" runat="server" />
        <asp:SqlDataSource ID="dsGame" runat="server" 
                ConnectionString="<%$ ConnectionStrings:StatsDB %>"
                ProviderName="<%$ ConnectionStrings:StatsDB.ProviderName %>"
                SelectCommand="SELECT g.redscore, g.bluescore, 
                                    (CASE WHEN g.IsCompleteGame = 1 THEN true ELSE false END) AS IsComplete,
                                    (CASE WHEN g.IsToBeCounted = 1 THEN true ELSE false END) AS IsCounted
                                FROM games g 
                                WHERE g.GameID = @gameid" >                
            <SelectParameters>
                <asp:QueryStringParameter Name="gameid" DbType="VarNumeric" QueryStringField="gameid" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>

