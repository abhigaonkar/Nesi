<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sync_bv_data" Title="Sync BV Data" Codebehind="sync_bv_data.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">

<h3>&nbsp;&nbsp; Sync BV Data</h3> <br /><br />
    <asp:label ID="id_bv_sync_progress" runat="server" Visible="false" />
    &nbsp;&nbsp; <asp:Button runat="server" Text="Sync All Data (Taxes, Terms, Customer, Vendor and Addresses)" OnClick="Sync_All_Data" Width="350" /> <br /><br />  

        &nbsp;&nbsp; <asp:Button runat="server" Text="Sync Taxes" OnClick="Sync_Taxes" Width="100" /> <br /><br />
        &nbsp;&nbsp; <asp:Button runat="server" Text="Sync Terms" OnClick="Sync_Terms" Width="100" /> <br /><br />
        &nbsp;&nbsp; <asp:Button runat="server" Text="Sync Customers" OnClick="Sync_Customers" Width="100" /> <br /><br />
        &nbsp;&nbsp; <asp:Button runat="server" Text="Sync Vendors" OnClick="Sync_Vendors" Width="100" /> <br /><br />
        &nbsp;&nbsp; <asp:Button runat="server" Text="Sync Addresses" OnClick="Sync_Addresses" Width="100" /> <br /><br />
<br />
<br />
</asp:Content>





