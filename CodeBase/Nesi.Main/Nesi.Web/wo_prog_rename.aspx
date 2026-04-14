<%@ Page Language="C#" AutoEventWireup="true" Inherits="WoProgRename" CodeBehind="wo_prog_rename.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Untitled Page</title>
	<style type="text/css">
		#divCoName {
			font-weight: 700;
		}
		div:empty {
			display: none;
		}
	</style>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript">
		$(document).ready(
			function()
				{
				page_obj.update_panel_progress.bind();
				})
		function navigateToAngularWorld(branchId) {
			//
			// From webform to parent angular page by reloading the page.
			// http://localhost/#/home/12/workorder/buckets/112
			//
			if(window.parent.length >0)
				{
				var href = window.parent.parent.location.origin + "/" + '#/home/12/workorder/buckets/' + branchId;
				console.log(href);

				setTimeout(function navigateToAngularWorld_newPage() {
					window.parent.parent.location.href = href;
					window.parent.parent.location.reload(true);
					}, 10);
				}
			else
				{
				location.href = "/" + '#/home/12/workorder/buckets/' + branchId;
				}
			}
	</script>
</head>
<body>
	<form id="form1" runat="server">

		<asp:Panel ID="panName" runat="server" Visible="false">
			<asp:UpdatePanel runat="server" id="up">
				<ContentTemplate>
		<span id="spanMSG" runat="server"></span>
			<table border="0" cellpadding="5" cellspacing="0" style="margin: 10px; font-family: Calibri, Arial; font-size: large;">
				<tr>
					<td colspan="2" valign="top" style="padding-right: 50px;">
						<asp:ScriptManager runat="server" id="sm"></asp:ScriptManager>
						<div id="divCoName" runat="server"></div>
						<div runat="server" id="selectinfo">
							<br />
							Select the Work Order matching the scanned PDF in the right panel
						</div>
						<div>
							<asp:DropDownList ID="ddlWOrename" runat="server" AppendDataBoundItems="false" AutoPostBack="true" OnSelectedIndexChanged="OnWoSelectionChanged">
							</asp:DropDownList>
						</div>
						<div style="padding: 5px;">
							<asp:Label ID="lblWarning" runat="server" EncodeHtml="false" ForeColor="Red" Font-Names="Arial" Font-Size="Small" Visible="False">
							</asp:Label>
						</div>
						<div style="padding: 5px;">
							<asp:Button ID="btnWOrename" runat="server" Text="Submit" OnClick="btnWOrename_Click" Height="30px" Width="100px" />
							<asp:Button ID="btnDeleteScan" runat="server" Text="Delete Scan" OnClick="btnDeleteScan_Click" OnClientClick="if(!confirm('Are you sure you want to delete this scan?')){return false;}" Height="30px" Width="100px" />
							<asp:Button ID="btnReplaceScan" runat="server" Text="Replace Scan" OnClick="btnReplaceScan_Click" Height="30px" Width="100px" />
							<asp:Label ID="lblMessage" runat="server" Text="Message" ForeColor="Red" Visible="False"></asp:Label><br />
							<asp:HiddenField ID="hidCompanyID" runat="server" Value="" />
							<asp:HiddenField ID="hidScanPath" runat="server" Value="" />
							<asp:HiddenField ID="hidWOID" runat="server" Value="" />
							<div id="divButtons" runat="server"></div>
						</div>
					</td>
					<td>
						<div id="scanName" runat="server"></div>
						<iframe height="745" width="600" id="pdfDocument" src="" runat="server"></iframe>
					</td>
				</tr>
			</table>
				</ContentTemplate>
			</asp:UpdatePanel>
		</asp:Panel>

	</form>
</body>
</html>
