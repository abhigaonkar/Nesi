<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_dashboards_frame" Title="Target Dashboards" Codebehind="frame.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register src="~/modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>





<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	<script type="text/javascript" language="javascript">
function resizeIframe(obj) {

try{
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 5) + 'px';
   }
   catch (err){}

  }

  function CurrencyFormatted(amount) {
  	var i = parseFloat(amount);
  	if (isNaN(i)) { i = 0.00; }
  	var minus = '';
  	if (i < 0) { minus = '-'; }
  	i = Math.abs(i);
  	i = parseInt((i + .005) * 100);
  	i = i / 100;
  	s = new String(i);
  	if (s.indexOf('.') < 0) { s += '.00'; }
  	if (s.indexOf('.') == (s.length - 2)) { s += '0'; }
  	s = minus + s;
  	return s;
  }

 </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" AutoPostBack="True" 
		Text="Use Employee Agreement Bonus System">
	</dx:ASPxCheckBox>
<iframe 
		 id="frm" RunAt="server" frameborder="0" height="800px" width="100%"></iframe> </asp:Content>				

<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style5
		{
			font-size: x-small;
		}
	</style>
</asp:Content>


