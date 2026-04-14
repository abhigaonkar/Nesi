<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_purchaseorder_NoWOWarning" Codebehind="NoWOWarning.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblWarning" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Larger"
            Text="There is no stock work order to display."></asp:Label><br />
        <asp:Label ID="lblInstruction" runat="server" Font-Names="Arial" Text="Please Close this Window and go back to the PO Page."></asp:Label>&nbsp;</div>
    </form>
</body>
</html>
