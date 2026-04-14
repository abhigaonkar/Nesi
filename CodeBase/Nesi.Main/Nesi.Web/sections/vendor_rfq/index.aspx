<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_vendor_rfq_index" Codebehind="index.aspx.cs" %>

<%@ Register Src="modules/login.ascx" TagName="Login" TagPrefix="mod" %>
<%@ Register Src="modules/menu.ascx" TagName="Menu" TagPrefix="mod" %>
<%@ Register Src="modules/review.ascx" TagName="Review" TagPrefix="mod" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>NE: Vendor Request for Quote (RFQ)</title>
</head>
<body>
    <form id="form1" runat="server">

		<div align="center">
		<table width="800px">
		<tr><td><mod:Login ID="mod_login" runat="server" Visible="false" /></td></tr>
		<tr><td><mod:Menu ID="mod_menu" runat="server" Visible="false" /></td></tr>
		<tr><td><mod:Review ID="mod_review" runat="server" Visible="false" /></td></tr>
		</table>
		</div>
    </form>
</body>
</html>
