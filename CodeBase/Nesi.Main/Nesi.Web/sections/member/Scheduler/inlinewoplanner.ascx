<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_scheduler_inlinewoplanner" Codebehind="inlinewoplanner.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" 
	BackColor="#FFFFCC" Width="100px" Height="45px" ClientInstanceName="cb" 
	oncallback="ASPxCallbackPanel1_Callback">
	<Paddings Padding="0px" />
	<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<table style="width: 100%; font-family: Arial, Helvetica, sans-serif;" 
		align="left">
		<tr>
			<td colspan="2" 
				style="overflow: hidden; white-space: nowrap; text-align: left; width: 100%;" 
				align="left">
				<dx:ASPxLabel ID="lblsubject" runat="server" Font-Names="Arial" 
					Text="ASPxLabel" Wrap="False" Width="90px">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td align="left" 
				style="overflow: hidden; white-space: nowrap; text-align: left; width: 0%;" 
				width="100%">
				<dx:ASPxLabel ID="lbltruck" runat="server" Font-Names="Arial" 
					Font-Size="XX-Small" Wrap="False">
				</dx:ASPxLabel> 
				&nbsp;</td>
			<td align="right" 
				style="overflow: hidden; white-space: nowrap; text-align: right; width: 50%;" 
				width="100%" height="15px">
				<dx:ASPxLabel ID="lblhours" runat="server" Font-Names="Arial" 
					Font-Size="XX-Small" Wrap="False">
				</dx:ASPxLabel>
			&nbsp;</td>
		</tr>
	</table>
		</dx:PanelContent>
</PanelCollection>
</dx:ASPxCallbackPanel>
