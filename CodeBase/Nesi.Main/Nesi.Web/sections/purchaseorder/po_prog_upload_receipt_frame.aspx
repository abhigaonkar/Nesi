<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_member_po_upload_receipt_frame" Title="Branch Packing Slip Upload" EnableTheming="True" Codebehind="po_prog_upload_receipt_frame.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register src="../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<ASP:CONTENT ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</ASP:CONTENT>
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <script type="text/javascript">
	 
	    function get_attachment(e, id, ext) {
	        e = e || window.event;
	        e.stopPropagation();
	        /* boing('/files/credit_card_receipts/' + id + '.' + ext, 'CC Purchase', 800, 600); */
	         boing(ext, 'PO Receipt', 800, 600); 
	    }

	    $(document).ready(function () {
	        please_wait('stop');
	    });
	    var btn_clicked = false;
    </script>
<iframe src="po_prog_upload_receipt.aspx" width="700px" frameborder="0" height="650px" 
		id="I1" name="I1"></iframe>
	
	<br />
	<br />
	<lc:LayoutControl ID="layout" runat="server" />

 </ASP:CONTENT>

