<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="mc" Title="Master Customers Grid" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>

<%@ Register Src="~/sections/customer/modules/sales.ascx" TagName="sales" TagPrefix="uc" %>
<%@ Register assembly="DevExpress.Xpo.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Xpo" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="header_placeholder" runat="server">
	<link rel="Stylesheet" type="text/css" href="/css/customer.css" />
	<style type="text/css">
		.options
			{
background: -moz-linear-gradient(top,  rgba(204,204,204,0.65) 0%, rgba(204,204,204,0.64) 1%, rgba(0,0,0,0) 100%); /* FF3.6+ */
background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(204,204,204,0.65)), color-stop(1%,rgba(204,204,204,0.64)), color-stop(100%,rgba(0,0,0,0))); /* Chrome,Safari4+ */
background: -webkit-linear-gradient(top,  rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* Chrome10+,Safari5.1+ */
background: -o-linear-gradient(top,  rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* Opera 11.10+ */
background: -ms-linear-gradient(top,  rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* IE10+ */
background: linear-gradient(to bottom,  rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* W3C */
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#a6cccccc', endColorstr='#00000000',GradientType=0 ); /* IE6-9 */



			}
		.fleft
			{
			float: left;
			display:block;
			}
		.fleft .table
			{
			background-color: #fff;
			border:solid 1px #999;
			margin:5px;
			border-radius:3px;
			}
		.style5
		{
			font-size: xx-small;
		}
	</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<p>
		 <div>
        <dx:ASPxGridView ID="ASPxGridView1" runat="server" OnCustomCallback="ASPxGridView1_CustomCallback" EnableViewState="false"
            ClientInstanceName="grid">
        </dx:ASPxGridView>
        <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Bind to the first database"
            AutoPostBack="false">
            <ClientSideEvents Click="function (s,e) { grid.PerformCallback('1'); }" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Bind to the second database"
            AutoPostBack="false">
            <ClientSideEvents Click="function (s,e) { grid.PerformCallback('2'); }" />
        </dx:ASPxButton>
    </div>
		
		<br />
	</p>
	<p>
		&nbsp;</p>
	<p>
	</p>
</asp:Content>
