<%@ Control Language="C#" AutoEventWireup="true" Inherits="mobile_modules_dashboard" Codebehind="dashboard.ascx.cs" %>

<style type="text/css">
	
	.style1
	{
		text-align: center;
		height:30px;
		 
	}
	
	.style2
	{
		text-align: center;
	}
	
</style>
<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
<script type="text/javascript">
	$("document").ready(function ()
	{
		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
		bind_datepicker();
	});
function bind_datepicker()
	{
	var minD	= new Date().setDate(new Date().getDate() - 1);
	var maxD = new Date().setDate(new Date().getDate() + 1);
	$(".date").datepicker({
		minDate: -1,
		maxDate: 1
		});
	}
function EndReqHandler(sender, args)
	{
	if (args != undefined && args.get_error() != undefined)
		{
		var err				= args.get_error().toString().replace(/Sys.+\Exception:/g, "").trim();
		alert(err);
		}
	bind_datepicker();
	}
</script>
<asp:UpdatePanel ID="up" runat="server">
	<ContentTemplate>
		<table style="width:260px; font-family: Arial; font-size: 8pt;">
			<tr>
				<td width="5%" class="style2">
					<b>This Pay Period</b></td>
				<td>
					<table style="width:100%;" bgcolor="#CCFFCC">
						<tr>
							<td class="style1">
								% HU</td>
						</tr>
						<tr>
							<td style="text-align: center">
								<asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" 
									Font-Names="Arial" Font-Size="14pt" 
									NavigateUrl="~/mobile/index.aspx?a=timesheet" style="text-align: center">HyperLink</asp:HyperLink>
							</td>
						</tr>
					</table>
				</td>
				<td>
					<table style="width:100%;" bgcolor="#99FF99">
						<tr>
							<td class="style1">
								Total Billable</td>
						</tr>
						<tr>
							<td style="text-align: center">
								<asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="True" 
									Font-Names="Arial" Font-Size="14pt" 
									NavigateUrl="~/mobile/index.aspx?a=timesheet" style="text-align: center">HyperLink</asp:HyperLink>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr id="quote_row" runat="server">
				<td width="5%" class="style2">
					<b>Quotes</b></td>
				<td>
					<table style="width:100%;" bgcolor="#99CCFF">
						<tr>
							<td class="style1">
								To Be Quoted:</td>
						</tr>
						<tr>
							<td style="text-align: center">
								<asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" 
									Font-Size="14pt" Text="Label"></asp:Label>
							</td>
						</tr>
					</table>
				</td>
				<td>
					<table style="width:100%;" bgcolor="#6699FF">
						<tr>
							<td class="style1">
								Waiting Cust Appr</td>
						</tr>
						<tr>
							<td style="text-align: center">
								<asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" 
									Font-Size="14pt" Text="Label"></asp:Label>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr id="wo_row" runat="server">
				<td width="5%" class="style2">
					<b>Work Orders</b></td>
				<td>
					<table style="width:100%;" bgcolor="#FFFFCC">
						<tr>
							<td class="style1">
								%Margin YTD</td>
						</tr>
						<tr>
							<td class="style1">
								<asp:HyperLink ID="HyperLink3" runat="server" Font-Bold="True" 
									Font-Names="Arial" Font-Size="14pt" 
									NavigateUrl="~/mobile/index.aspx?a=timesheet">HyperLink</asp:HyperLink>
							</td>
						</tr>
					</table>
				</td>
				<td>
					<table style="width:100%;" bgcolor="#FFFF99">
						<tr>
							<td class="style1">
								Waiting My Appr</td>
						</tr>
						<tr>
							<td style="text-align: center">
								<asp:HyperLink ID="HyperLink4" runat="server" Font-Bold="True" 
									Font-Names="Arial" Font-Size="14pt" 
									NavigateUrl="~/mobile/index.aspx?a=timesheet" style="text-align: center">HyperLink</asp:HyperLink>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td width="5%" class="style2">
					<b>Tickets</b></td>
				<td>
					<table style="width:100%;" bgcolor="#FFCCFF">
						<tr>
							<td class="style1">
								Waiting For Me:</td>
						</tr>
						<tr>
							<td class="style1">
								<asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Arial" 
									Font-Size="14pt" Text="Label"></asp:Label>
							</td>
						</tr>
					</table>
				</td>
				<td>
					<table style="width:100%;" bgcolor="#FF99FF">
						<tr>
							<td class="style1">
								I&#39;m Waiting For:</td>
						</tr>
						<tr>
							<td class="style1">
								<asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="Arial" 
									Font-Size="14pt" Text="Label"></asp:Label>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td width="5%" class="style1">
					<b>Messages</b></td>
				<td width="80px" align="center">
					<table style="width:100%;" bgcolor="#FFCC99">
						<tr>
							<td class="style1">
								Unread:</td>
						</tr>
						<tr>
							<td class="style1">
								<asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Names="Arial" 
									Font-Size="14pt" Text="Label"></asp:Label>
							</td>
						</tr>
					</table>
				</td>
				<td width="80px">
					&nbsp;</td>
			</tr>
		</table>
	
		<div id="db" runat = "server"></div>
	</ContentTemplate>
</asp:UpdatePanel>
