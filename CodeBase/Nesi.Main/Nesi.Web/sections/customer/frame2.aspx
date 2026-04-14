<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="frame_2" Title="Customer Interface" Codebehind="frame2.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server"><div id="divMenu" runat="server"></div></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server"><div id="divSide" runat="server"></div></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
<script type="text/javascript">
		$(window).resize(function()
			{
			resize_frame(0);
			});
		function resize_frame(width)
			{
			$(".frame").width($(window).width()-200);
			//$(".frame").height($(window).height()+500);
			}
		$(document).ready(
			function()
				{
				resize_frame(0);
				});
</script>
<iframe frameborder="0" id="frame" class="frame" scrolling="no" src="index2.aspx" allowtransparency="true" runat="server" style="background-color:Transparent; min-height:1800px;" ></iframe>
</asp:Content>

