<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_vendor_rfq_modules_login" Codebehind="login.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"	Namespace="DevExpress.Web" TagPrefix="dx" %>


<dx:ASPxCallbackPanel ID="cbp_login" runat="server" ClientInstanceName="cbp_login"
	HideContentOnCallback="True" OnCallback="cbp_login_Callback" CssClass="login">
	<PanelCollection>
		<dx:PanelContent runat="server">
	<table cellpadding="5" cellspacing="0" width="100%">
		<tr>
			<td align="center" colspan="3">
				<dx:ASPxImage ID="i_logo" runat="server">
				</dx:ASPxImage>
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<strong>RFQ Login<br />
					<dx:ASPxLabel ID="lb_error" runat="server" EncodeHtml="False" ForeColor="#C00000">
					</dx:ASPxLabel>
				</strong></td>
		</tr>
		<tr>
			<td width='120'>Key Code:</td>
			<td style="width: 5%"></td>
			<td>
				<dx:ASPxTextBox ID="t_passcode" runat="server" Width="160px">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td>
			</td>
			<td style="width: 5%"></td>
			<td>
				<dx:ASPxButton ID="bt_login" runat="server" AutoPostBack="False" ClientInstanceName="bt_login"
					Text="Login">
					<ClientSideEvents Click="function(s, e) {
	cbp_login.PerformCallback();
}" />
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
		</dx:PanelContent>
	</PanelCollection>
</dx:ASPxCallbackPanel>
