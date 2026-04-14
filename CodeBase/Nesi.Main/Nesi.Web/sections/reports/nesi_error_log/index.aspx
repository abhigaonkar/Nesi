<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_nesi_error_log_index" Title="Nesi Error Log" EnableTheming="True" Theme="NETheme01" Codebehind="index.aspx.cs" %>

<%@ Register Src="~/modules/crash_log.ascx" TagPrefix="cl" TagName="crash_log" %>

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
		</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
    <cl:crash_log runat="server" ID="crash_log" isMessageBoard="false"/>
	
</asp:Content>

