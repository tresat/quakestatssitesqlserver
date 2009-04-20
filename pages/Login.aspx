<%@ Page Title="Quake Stats - Login" Language="VB" MasterPageFile="~/pages/QuakeStats.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="pages_Login" %>

<asp:Content ID="cntMain" ContentPlaceHolderID="cphMain" Runat="Server">
    <asp:Login ID="lgnMain" runat="server" DisplayRememberMe="true" DestinationPageUrl="~/pages/Home.aspx" />
</asp:Content>

