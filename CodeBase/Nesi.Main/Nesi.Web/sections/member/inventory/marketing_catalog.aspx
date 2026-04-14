<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_member_inventory_marketing_catalog" Theme="NETheme01" Title="Marketing Catalog" Codebehind="marketing_catalog.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" runat="Server">
    <script type="text/javascript" language="javascript">
        function resizeIframe(obj)
        {
            obj.style.height = (obj.contentWindow.document.body.scrollHeight + 10) + 'px';
	
        }
        </script>
    </br>
<div style="font-family:Arial">To obtain a logon call Bruce at FormCor at (416) 823 3216 or email him at bruce@formcor.com</div>
    </br>
    <iframe id="frame" runat="server" height="800px" width="100%" frameborder="0"  ></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" runat="Server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="header_placeholder">
	<style type="text/css">
		.style2 { text-align: left; }
	</style>
</asp:Content>
