<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="cust" Title="Customer Interface" Codebehind="frame.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server"><div id="divMenu" runat="server"></div></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server"><div id="divSide" runat="server"></div></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
<script type="text/javascript">
		$(window).resize(function()
			{
			resize_frame(0);
			});
		function resize_frame(width) {
			if (!typeof ($(".frame")[0]) === 'undefined') {
				var height = $(".frame")[0].contentWindow.document.body.scrollHeight;
				height = height > 3000 ? height - 1000 : height;
				$(".frame").width($(window).width() - 200);
				$(".frame").height(height);
			}
			}
</script>
<iframe frameborder="0" id="frame"  scrolling="no" src="index.aspx" allowtransparency="true" runat="server" style="padding: 5px; background-color:Transparent; min-height:3000px;" width="98%"></iframe>
</asp:Content>

