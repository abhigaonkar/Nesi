<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_admin_vendor_DisplayVendInfo" Codebehind="DisplayVendInfo.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
    
         function confirmation() 
       {
              return (confirm("Are you sure you wish to merge these vendor?\nPlease Be aware the merge may take a few minutes"));
       }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/sections/admin/vendor/merge-index.aspx">Back To First Page</asp:HyperLink><br />
    <span id = "Vendtable" runat="server"></span>
    <br />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/sections/admin/vendor/merge-index.aspx">Back To First Page</asp:HyperLink></div>
    </form>
</body>
</html>
