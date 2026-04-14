<%@ Page Language="C#" AutoEventWireup="true" Theme="" Inherits="sections_member_inventory_annual_counts_import" Codebehind="annual_counts_import.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Annual inventory importer</title>
</head>
<body>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript">
function disableF5(e) { if ((e.which || e.keyCode) == 116) e.preventDefault(); };
//$(document).bind("keydown", disableF5);
	</script>
    <form id="f" runat="server">
    <div style="font-family:Segoe UI,Calibri; font-size: .85em;">
		<b>Format of the CSV files is as follows (Each will be checked upon processing):</b>
		<div>
			<ul>
				<li>MasterID, LocationID, QTY</li>
				<li>No column headers</li>
				<li>No negative numbers</li>
				<li>No duplicate Master ID's per location</li>
				<li>Location has to be created and part of the selected branch</li>
				<li>Master ID needs to have a cost associated with it</li>
			</ul></div>
    	<b>CSV File:</b><br />
		<table cellpadding="2" cellspacing="0">
			<tr>
				<td><asp:FileUpload ID="uploader" runat="server" /></td>
				<td><asp:Button ID="bt_check" runat="server" style="background-color:#090;color:#fff;" Text="Check File" onclick="bt_check_Click" /></td>
				<td><asp:Button ID="bt_upload" runat="server" style="background-color:#f00;color:#fff;" Text="Process" onclientclick="return confirm('Are you sure? If successful, this WILL update inventory.');" onclick="bt_upload_Click" /></td>
				<td><asp:Button ID="bt_close" runat="server" Text="Close Window" onclientclick="window.close()" /></td>
			</tr>
		</table>
		<br />
		<asp:Label ID="lb_message" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
