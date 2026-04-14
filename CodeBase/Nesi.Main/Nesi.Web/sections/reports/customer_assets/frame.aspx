<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="customer_assets_frame" Title="Customer Assets	" Codebehind="frame.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<iframe width="100%" frameborder="0" id="frame" height="1060" scrolling="no" src="index.aspx" allowtransparency="true" runat="server" style="background-color:Transparent" ></iframe>
<script type="text/javascript">

 function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 30) + 'px';
	
 }
 </script>
</asp:Content>

