<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_scheduler_overviewform" Codebehind="overviewform.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<dx:ASPxCallbackPanel ID="cb_overview" runat="server" 
	BackColor="Transparent" Width="18px" Height="50px" 
	ClientInstanceName="cb_overview" style="text-align: center">
	<Paddings Padding="0px" />
	<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<table style="width: 100%; font-family: Arial, Helvetica, sans-serif;" 
		align="center">
		<tr>
			<td 
				style="overflow: hidden; white-space: nowrap; " align="center" 
				width="100%" bgcolor="Transparent">
				<dx:ASPxLabel ID="lblsched" runat="server" Font-Names="Arial" 
					Text="NA" Wrap="False" Width="18px" Font-Size="X-Small" BackColor="Transparent" 
					Cursor="pointer" ToolTip="Total Scheduled + Days Off">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td align="center" 
				style="overflow: hidden; white-space: nowrap; " 
				width="100%" bgcolor="Transparent">
				<dx:ASPxLabel ID="lblheadcount" runat="server" Font-Names="Arial" 
					Font-Size="Small" Wrap="False" Width="18px" Text="NA" BackColor="Transparent" 
					Cursor="pointer" ToolTip="Total Headcount">
				</dx:ASPxLabel> 
				</td>
			
		</tr>
		<tr>
			<td align="center" 
				style="overflow: hidden; white-space: nowrap; " 
				width="100%" bgcolor="Transparent">
				<dx:ASPxLabel ID="lblrequested" runat="server" Font-Names="Arial" 
					Font-Size="X-Small" Wrap="False" Width="18px" Text="NA" BackColor="Transparent" 
					Cursor="pointer" ToolTip="People Requested">
				</dx:ASPxLabel>
			</td>
		</tr>
	</table>
		</dx:PanelContent>
</PanelCollection>
</dx:ASPxCallbackPanel>