<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_fvr_modules_template_detail" Codebehind="template_detail.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



		<%@ Register src="template_page.ascx" tagname="template_page" tagprefix="uc1" %>

<table width="100%" cellpadding="2" cellspacing="0">
	<tr>
		<td width="100">Name:</td>
		<td><dx:ASPxTextBox ID="tb_name" runat="server" Width="250px"></dx:ASPxTextBox></td>
	</tr>
	<tr>
		<td>Type:
			<input type="hidden" id="hid_id" runat="server" /></td>
		<td><dx:ASPxComboBox ID="combo_type" runat="server" ValueType="System.Int32">
			<ClientSideEvents SelectedIndexChanged="function(s, e) {
	pc_type.SetActiveTab(pc_type.GetTab(s.GetValue()));
}" />
			<Items>
				<dx:ListEditItem Text="Please Select Type" Value="0" />
				<dx:ListEditItem Text="New" Value="1" />
				<dx:ListEditItem Text="Exit" Value="2" />
				<dx:ListEditItem Text="Renew" Value="3" />
			</Items>
			</dx:ASPxComboBox></td>
	</tr>
	<tr>
		<td>Days till Expire?:</td>
		<td><dx:ASPxSpinEdit runat="server" ID="spin_till_expire" MinValue="1" MaxValue="30" Width="50px" NumberType="Integer"></dx:ASPxSpinEdit> </td>
	</tr>
	<tr>
		<td colspan="2">
			<dx:ASPxPageControl ID="pc_type" runat="server" ActiveTabIndex="0" ClientInstanceName="pc_type" oninit="pc_type_Init">
				<TabPages>
					<dx:TabPage Name="0" Text="Blank">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True"></dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Name="NEWHIRE" Text="New">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True"></dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Name="EXIT" Text="Exit">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True"></dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Name="RENEW" Text="Renew">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True"></dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
				</TabPages>
				<ContentStyle>
					<Border BorderWidth="0px" />
<Border BorderWidth="0px"></Border>
				</ContentStyle>
				<Border BorderWidth="0px" />

<Border BorderWidth="0px"></Border>
			</dx:ASPxPageControl>
		</td>
	</tr>
	<tr>
		<td colspan="2"><dx:ASPxLabel ID="lb_error" EncodeHtml="false" runat="server"></dx:ASPxLabel></td>
	</tr>
	<tr>
		<td>
			<table>
				<tr>
					<td><dx:ASPxButton ID="bt_cancel" runat="server" AutoPostBack="False" Text="Cancel" Width="50px">
			<ClientSideEvents Click="function(s, e) {
	list_available_templates.UnselectAll();
	cbp_detail.PerformCallback(&quot;cancel|0&quot;);
}" />
			</dx:ASPxButton></td>
					<td><dx:ASPxButton ID="bt_save" runat="server" AutoPostBack="false" Text="Save" Width="50px"><ClientSideEvents Click="function(s, e) {
	cbp_detail.PerformCallback(&quot;save|0&quot;);
}" /></dx:ASPxButton></td>
				</tr>
			</table></td>
		<td></td>
	</tr>
</table>


