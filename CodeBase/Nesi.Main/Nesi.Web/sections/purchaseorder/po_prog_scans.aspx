<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_purchaseorder_po_prog_scans2" Codebehind="po_prog_scans.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase Order Scans</title>
    <link rel="stylesheet" type="text/css" href="tabcontent.css" />

</head>
<body>
    <form id="form1" runat="server">
<script type="text/javascript" src="/js/tabcontent/tabcontent.js">
</script>
<script>
// JScript File
function DeleteScan(scanid)
{
    PageMethods.DeleteScan(scanid);
    location.href = location.href;
}
function OnFailed(error) {
   // Alert user to the error.
   alert(error.get_message());
}
</script>
    <asp:ScriptManager ID="sm" runat="server" EnablePageMethods="true"/>
    <div>
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        
        <script type="text/javascript">

          var scans=new ddtabcontent("scantabs")
          scans.setpersist(true)
          scans.setselectedClassTarget("link") //"link" or "linkparent"
          scans.init()
         </script>

    </div>
    </form>
</body>
</html>
