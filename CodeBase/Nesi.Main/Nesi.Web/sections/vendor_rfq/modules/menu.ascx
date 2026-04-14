<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_vendor_rfq_modules_menu" Codebehind="menu.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>


<dx:ASPxCallbackPanel ID="cbp_menu" runat="server" ClientInstanceName="cbp_menu"
	CssClass="menu" OnCallback="cbp_menu_Callback">
	<PanelCollection>
		<dx:PanelContent runat="server">
			<table width="100%">
				<tr>
					<td style="width: 165px">
			<dx:ASPxImage ID="i_logo" runat="server" ImageUrl="~/images/foundation/header/decal/decal[logo].png" CssClass="i_logo" ImageAlign="AbsMiddle" Height="79px" Width="165px">
			</dx:ASPxImage>
					</td>
					<td style="width: 250px" valign="middle">
						<dx:ASPxLabel ID="lbl_branch_info" runat="server" CssClass="lb_h" EncodeHtml="False" Width="250px">
						</dx:ASPxLabel>
					</td>
					<td style="width: 100%" colspan="2" valign="middle" align="right">
						<dx:ASPxLabel ID="lbl_rfq_info" runat="server" CssClass="lb_h" EncodeHtml="False" Width="100%">
						</dx:ASPxLabel>
					</td>
				</tr>
			</table>
			<table width="100%" class="buttons">
				<tr>
					<td align="left" style="width: 100%">
						<dx:ASPxLabel ID="lbl_rfq_number" runat="server" CssClass="lb" 
							EncodeHtml="False">
						</dx:ASPxLabel>
					</td>
					<td rowspan="1" style="width: 125px">
						<dx:ASPxButton ID="bt_saveall" runat="server" AutoPostBack="False" Height="30px" Text="Save All"
							ToolTip="Press this if you wish to save this data to submit later." UseSubmitBehavior="False" 
							Width="125px" Visible="False">
							<ClientSideEvents Click="function(s, e) {
	handle_rowsave(s, true);
}" />
							<Image Url="~/images/icon/icon[save].gif">
							</Image>
						</dx:ASPxButton>
					</td>
					<td rowspan="1" style="width: 125px">
						<dx:ASPxButton ID="bt_verify" runat="server" AutoPostBack="False" Height="30px" Text="Send Quote"
							ToolTip="Press this if all the information below is correct and you wish to sumbit your quotation." UseSubmitBehavior="False" 
							Width="125px">
							<ClientSideEvents Click="function(s, e) {
	handle_verify(s);
}" />
							<Image Url="~/images/icon/icon[approve].gif">
							</Image>
						</dx:ASPxButton>
					</td>
					<td style="width: 125px" rowspan="1">
						<dx:ASPxButton ID="bt_logout" runat="server" AutoPostBack="False" ClientInstanceName="bt_logout"
							Text="Logout" Width="125px" UseSubmitBehavior="False" Height="30px">
							<ClientSideEvents Click="function(s, e) {
	cbp_menu.PerformCallback();
}" />
							<Image Url="~/images/icon/icon[logoff].gif">
							</Image>
						</dx:ASPxButton>
					</td>
				</tr>
			</table>
		</dx:PanelContent>
	</PanelCollection>
</dx:ASPxCallbackPanel>
