<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_training_faq" MasterPageFile="~/IntraDefault.master" Codebehind="faq.aspx.cs" %>

<asp:Content ID="hdrcontent" ContentPlaceHolderID="header_placeholder" runat="server">
	<script type="text/javascript" src="/js/faq.js"></script>
	<style type="text/css">
		.faqs
			{
			width:				100%;
			}
		.l
			{
			float:				left;
			width:				25%;
			border-right:		solid 1px #ccc;
			padding:			10px;
			min-height:			800px;
			}
		.r
			{
			float:				left;
			width:				auto;
			padding:			10px;
			min-height:			800px;
			}
		.body
			{
			padding:			10px;
			}
		.head
			{
			font-size:			20px;
			font-weight:		bold;
			text-decoration:	underline;
			}
		.s
			{
			font-size:		14px;
			font-weight:	bold;
			margin-left:	20px;
			}
		.a
			{
			display:		none;
			margin-left:	40px;
			font-size:		12px;
			}
		.d
			{
			cursor:			pointer;
			}
	</style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">
	<asp:ScriptManager ID="sm" runat="server" EnablePageMethods="True">
	</asp:ScriptManager>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<div class="faqs">
		<div id="faq_sections" runat="server" class="l"></div>
		<div id="faq_article" class="r"><div class='head'></div><div class='body'></div></div>
	</div>
</asp:Content>
