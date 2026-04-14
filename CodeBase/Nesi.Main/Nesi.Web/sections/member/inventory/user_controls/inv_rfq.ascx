<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_inventory_inv_rfq" EnableTheming="True" Codebehind="inv_rfq.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<div id="detail" runat="server"></div>
	<script type="text/javascript">
		function save_rfq(s,e)
			{
			var id			= $(".hid_id").val();
			var v			=	{
								id:			id == undefined || id == '' ? 0 : id,
								name:		t_rfq_name.GetText(),
								note:		t_rfq_notes.GetText(),
								date_open:	date_open.GetText(),
								date_close:	date_close.GetText()
								};
			if($.trim(v.name) == "")
				{
				alert("The RFQ name is invalid");
				t_rfq_name.Focus();
				return false;
				}
			if(!Date.parse(v.date_open))
				{
				alert("Your open date is invalid");
				date_open.Focus();
				return false;
				}
			if(!Date.parse(v.date_close))
				{
				alert("Your close date is invalid");
				date_close.Focus();
				return false;
				}
			if(Date.parse(v.date_close) < Date.parse(v.date_open))
				{
				alert("Your close date must be AFTER your open date");
				date_close.Focus();
				return false;
				}
			cb_save.PerformCallback(JSON.stringify(v));
			
			
			}
	</script>
	<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="up_rfq">
		<ProgressTemplate>
			<img id="Img1" alt="progressing" src="/images/loading_panel.gif" style="left: 50%; position: absolute; top: 50%" />
		</ProgressTemplate>
	</asp:UpdateProgress>
	<lc:LayoutControl runat="server" id="layout" GridviewID="gv_rfq" ShowExcelExport="True" ShowPDFExport="True" ShowToggle="True" />
<asp:UpdatePanel ID="up_rfq" runat="server">
		<ContentTemplate>
			<dx:ASPxGridView runat="server" ID="gv_rfq" AutoGenerateColumns="False" 
				DataSourceID="ds_rfq" KeyFieldName="id" Width="100%" 
				ClientInstanceName="gv_rfq" OnCustomCallback="gv_rfq_CustomCallback" 
				OnDetailRowExpandedChanged="gv_rfq_DetailRowExpandedChanged" 
				OnHtmlEditFormCreated="gv_rfq_HtmlEditFormCreated" 
				OnStartRowEditing="gv_rfq_StartRowEditing1" 
				oncustomjsproperties="gv_rfq_CustomJSProperties" Theme="NETheme01" 
				onrowdeleting="gv_rfq_RowDeleting">
                <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
	<Columns>
		<dx:gridviewcommandcolumn ButtonType="Image" Caption="Actions" VisibleIndex="0" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true" 
			Width="45px" >
				
			
			
		</dx:gridviewcommandcolumn>
		<dx:gridviewdatatextcolumn Caption="RFQ #" FieldName="id" ReadOnly="True" VisibleIndex="1"
			Width="45px">
			<EditFormSettings Visible="False" />
		</dx:gridviewdatatextcolumn>
		<dx:gridviewdatatextcolumn Caption="Name" FieldName="name" VisibleIndex="2">
			<Settings AutoFilterCondition="Contains" />
		</dx:gridviewdatatextcolumn>
		<dx:gridviewdatatextcolumn Caption="Status" FieldName="status" VisibleIndex="3" Width="50px">
		</dx:gridviewdatatextcolumn>
		<dx:gridviewdatatextcolumn Caption="Member" FieldName="member" VisibleIndex="4">
			<Settings AutoFilterCondition="Contains" />
		</dx:gridviewdatatextcolumn>
		<dx:gridviewdatadatecolumn Caption="Date Created" FieldName="date_created" VisibleIndex="5"
			Width="50px">
		</dx:gridviewdatadatecolumn>
		<dx:gridviewdatadatecolumn Caption="Open Date" FieldName="date_open" VisibleIndex="6"
			Width="50px">
		</dx:gridviewdatadatecolumn>
		<dx:gridviewdatadatecolumn Caption="Close Date" FieldName="date_close" VisibleIndex="7"
			Width="50px">
		</dx:gridviewdatadatecolumn>
		<dx:gridviewdatatextcolumn Caption="Notes" FieldName="notes" VisibleIndex="10">
		</dx:gridviewdatatextcolumn>
		<dx:gridviewdatatextcolumn Caption="# Responded" FieldName="responded" VisibleIndex="9" Width="50px">
		</dx:gridviewdatatextcolumn>
		<dx:gridviewdatatextcolumn Caption="# Invited" FieldName="invited" VisibleIndex="8" Width="50px">
		</dx:gridviewdatatextcolumn>
		<dx:GridViewDataTextColumn Caption="vendors" FieldName="vendors" Visible="False" VisibleIndex="11" ShowInCustomizationForm="False">
		</dx:GridViewDataTextColumn>
	</Columns>
	<Settings ShowTitlePanel="True" />
				<SettingsPopup EditForm-HorizontalAlign="WindowCenter" EditForm-VerticalAlign="WindowCenter">
					<EditForm Modal="True" />
				</SettingsPopup>
	<Styles>
		<Header HorizontalAlign="Center">
		</Header>
		<Cell HorizontalAlign="Center">
		</Cell>
		<DetailCell>
			<Paddings Padding="0px" />
		</DetailCell>
		<Table>
			<Paddings Padding="0px" />
		</Table>
	</Styles>
				<StylesPopup>
					<EditForm>
						<ModalBackground Opacity="0">
						</ModalBackground>
					</EditForm>
				</StylesPopup>
	<Templates>
		<TitlePanel>
			<table style="width:100%;">
				<tr>
					<td width="120">
						<dx:ASPxButton ID="b_new" runat="server" AutoPostBack="False" Text="New RFQ" Theme="NETheme01">
							<Image Url="~/images/icon/icon[add].gif">
							</Image>
							<ClientSideEvents Click="function(s, e) {
	gv_rfq.AddNewRow();
}" />
						</dx:ASPxButton>
					</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td>
						<b style='font-size:12px;color:#000;'>Filter by Vendor:</b></td>
					<td>
						<dx:ASPxComboBox ID="combo_vendors" runat="server" DataSourceID="sds_allvendors" TextField="name" ValueField="vendor_id" Theme="NETheme01">
							<ClientSideEvents SelectedIndexChanged="function(s, e) {
	gv_rfq.PerformCallback(s.GetValue());
}" />
						</dx:ASPxComboBox>
						<asp:SqlDataSource ID="sds_allvendors" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT vendor_id, proper(vendor_name(vendor_id)) name FROM rfq_vendor_list a WHERE rfq_header_id IN (SELECT id from rfq_header WHERE business_unit_id = @business_unit_id) group by vendor_id order by name">
							<SelectParameters>
								<asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" />
							</SelectParameters>
						</asp:SqlDataSource>
					</td>
				</tr>
			</table>
		</TitlePanel>

		<EditForm>
			&nbsp;<dx:aspxpanel ID="rfq_panel" runat="server">
				<PanelCollection>
					<dx:panelcontent runat="server">
						<table cellpadding="2" cellspacing="0">
							<tr>
								<td style="width: 100px">
									RFQ Name:</td>
								<td style="width: 300px; padding-right: 10px;">
									<dx:aspxtextbox ID="t_rfq_name" runat="server" ClientInstanceName="t_rfq_name" Width="100%" Text='<%# Bind("name") %>' TabIndex="49" Theme="NETheme01">
										<ValidationSettings ValidationGroup="new_rfq">
											<RequiredField ErrorText="Please provide a Name for this RFQ" IsRequired="True" />
										</ValidationSettings>
									</dx:aspxtextbox>
								</td>
								<td rowspan="6" style="padding-left: 10px; border-left: #aaa 1px solid;" valign="top">
									<table cellpadding="5" cellspacing="0" width="550">
										<tr>
											<td align="center">
												
													<asp:Label ID="lb_vendor" runat="server" Text="Vendor" Theme="NETheme01"></asp:Label>
												</td>
											<td align="center">
												
													<asp:Label ID="lb_contact" runat="server" Text="Contact" Theme="NETheme01"></asp:Label>
												</td>
											<td align="center" style="width: 152px" valign="middle">
											</td>
										</tr>
										<tr>
											<td align="center">
												&nbsp;<input id="t_vendor" class="t_vendor" type="text" style="font-weight:bold;font-size:11px;width:150px;" onfocus="attach_ac(this, 'vendor')" data-click="refresh_contacts()" runat="server" />
											</td>
											<td align="center">
												<select id="s_contact" class="s_contact" runat="server" style="font-weight:bold;font-size:11px;width:150px;" disabled="disabled"></select>
											</td>
											<td style="width: 152px" align="center" valign="middle">
												<dx:aspxbutton ID="b_save_vendor" runat="server" AutoPostBack="False" Text="Add Vendor" Theme="NETheme01">
													<ClientSideEvents Click="save_rfq_vendor" />
												</dx:aspxbutton>
											</td>
										</tr>
									</table>
									<br />
									<dx:aspxgridview ID="gv_vendors" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_vendors"
										DataSourceID="sds_selected_vendors" KeyFieldName="id" OnRowDeleting="gv_vendors_RowDeleting"
										Width="100%" Theme="NETheme01">
                                        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
										<Columns>
											<dx:gridviewcommandcolumn ButtonType="Image" VisibleIndex="0" Width="20px" ShowDeleteButton="true">
												
											</dx:gridviewcommandcolumn>
											<dx:gridviewdatatextcolumn FieldName="vendor_id" Visible="False" VisibleIndex="2">
											</dx:gridviewdatatextcolumn>
											<dx:gridviewdatatextcolumn Caption="Vendor" FieldName="vendor_name" VisibleIndex="3">
												<DataItemTemplate>
													<dx:aspxhyperlink ID="ASPxHyperLink3" runat="server" Text='<%# Eval("vendor_name") %>' Theme="NETheme01" NavigateUrl='<%# "/redir.aspx?url=" + HttpUtility.UrlEncode(string.Format("/#/opens/11/vendors/{0}", Eval("vendor_id"))) %>' Target="_blank">
													</dx:aspxhyperlink>
												</DataItemTemplate>
											</dx:gridviewdatatextcolumn>
											<dx:gridviewdatatextcolumn Caption="Total" FieldName="total" VisibleIndex="4">
											</dx:gridviewdatatextcolumn>
											<dx:gridviewdatatextcolumn Caption="Contact" FieldName="contact_name" VisibleIndex="5">
											</dx:gridviewdatatextcolumn>
											<dx:gridviewdatatextcolumn Caption="ID" FieldName="id" Visible="False" VisibleIndex="1">
											</dx:gridviewdatatextcolumn>
											<dx:gridviewdatatextcolumn Caption="Opened" FieldName="date_opened" VisibleIndex="6">
											</dx:gridviewdatatextcolumn>
											<dx:gridviewdatatextcolumn Caption="Preview" VisibleIndex="9" Width="35px">
												<DataItemTemplate>
													<dx:aspxbutton ID="preview" runat="server" Height="20px" Width="20px" AutoPostBack="False" Theme="NETheme01" OnCustomJSProperties="preview_CustomJSProperties">
														<Image Url="~/images/icon/icon[details].gif">
														</Image>
														<Paddings Padding="0px" />
														<ClientSideEvents Click="function(s, e) {
	callback_set_vendor.PerformCallback(s.cp_vendor_id);
}" />
													</dx:aspxbutton>
												</DataItemTemplate>
												<CellStyle HorizontalAlign="Center">
											</CellStyle>
											</dx:gridviewdatatextcolumn>
											<dx:gridviewdatatextcolumn Caption="Sent" FieldName="date_sent" VisibleIndex="7">
											</dx:gridviewdatatextcolumn>
											<dx:gridviewdatatextcolumn Caption="Verified" FieldName="date_verified" VisibleIndex="8">
											</dx:gridviewdatatextcolumn>
										</Columns>
										<Styles>
											<Header Font-Bold="True" HorizontalAlign="Center">
											</Header>
										</Styles>
									</dx:aspxgridview>
									<asp:SqlDataSource ID="sds_selected_vendors" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
	a.id,
	a.vendor_id,
	URLDECODE(b.vendor_name) vendor_name,
	URLDECODE(c.contact_name) contact_name,
	a.date_opened,
	a.date_sent,
	a.date_verified,
	SUM(IFNULL(rp.qty, 0) * IFNULL(rl.vendor_price, 0)) total
FROM 
	rfq_vendor_list a
LEFT JOIN
	vendor b ON
		a.vendor_id = b.vendor_id
LEFT JOIN
	contact c ON
		a.contact_id = c.contact_id
LEFT JOIN
	rfq_lineitem rl ON
		a.rfq_header_id = rl.rfq_header_id AND 
		a.vendor_id = rl.vendor_id
LEFT JOIN
	rfq_part_list rp ON
		rl.rfq_header_id = rp.rfq_header_id AND 
		rl.master_id = rp.master_id
WHERE
	a.rfq_header_id = @rfq_id
GROUP BY
	a.vendor_id, a.contact_id
ORDER BY 
	b.vendor_name">
										<SelectParameters>
											<asp:SessionParameter Name="@rfq_id" SessionField="working_rfq_id" />
										</SelectParameters>
									</asp:SqlDataSource>
									<dx:aspxcallback ID="callback_set_vendor" runat="server" ClientInstanceName="callback_set_vendor"
										OnCallback="callback_set_vendor_Callback">
										<ClientSideEvents EndCallback="function(s, e) {
	pop_rfq_preview.Show();
}" />
									</dx:aspxcallback>
								</td>
							</tr>
							<tr>
								<td style="width: 100px">
									Status:</td>
								<td style="padding-right: 10px">
									<dx:aspxtextbox ID="ASPxTextBox1" Enabled="false" runat="server" Text='<%# Eval("status") %>' Theme="NETheme01"></dx:aspxtextbox>
								</td>
							</tr>
							<tr>
								<td style="width: 100px">
									Notes:</td>
								<td style="padding-right: 10px">
									<dx:aspxmemo ID="t_rfq_notes" runat="server" AutoResizeWithContainer="True" ClientInstanceName="t_rfq_notes"
										Height="71px" Width="100%" Text='<%# Bind("notes") %>' TabIndex="50" Theme="NETheme01">
									</dx:aspxmemo>
								</td>
							</tr>
							<tr>
								<td style="width: 100px">
									Open Date:</td>
								<td>
									<dx:aspxdateedit ID="date_open" runat="server" ClientInstanceName="date_open" Value='<%# bind("date_open") %>' TabIndex="51" Theme="NETheme01" DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd">
										<ValidationSettings ValidationGroup="new_rfq">
											<RequiredField ErrorText="Please Provide an Open Date" IsRequired="True" />
										</ValidationSettings>
									</dx:aspxdateedit>
								</td>
							</tr>
							<tr>
								<td style="width: 100px">
									Close Date:</td>
								<td>
									<dx:aspxdateedit ID="date_close" runat="server" ClientInstanceName="date_close" Value='<%# bind("date_close") %>' TabIndex="52" Theme="NETheme01" DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd">
										<ValidationSettings ValidationGroup="new_rfq">
											<RequiredField ErrorText="Please provide a Close Date" IsRequired="True" />
										</ValidationSettings>
									</dx:aspxdateedit>
								</td>
							</tr>
							<tr>
								<td align="center" valign="top" style="width: 100px">
									<dx:aspxbutton ID="b_cancel" runat="server" AutoPostBack="False" Text="Cancel" OnClick="b_cancel_Click" TabIndex="56" Theme="NETheme01">
										
									</dx:aspxbutton>
									&nbsp;</td>
								<td valign="top">
									<dx:aspxbutton ID="b_start" ClientInstanceName="b_start" runat="server" Text="Save" AutoPostBack="false" ValidationContainerID="vs_new_rfq"
										ValidationGroup="new_rfq" TabIndex="55" Theme="NETheme01">
										<ClientSideEvents Click="save_rfq" />
										<Image Url="~/images/icon/icon[save].gif">
										</Image>
									</dx:aspxbutton>
								</td>
							</tr>
							<tr>
								<td align="left" colspan="2" valign="top">
									<dx:aspxvalidationsummary ID="vs_new_rfq" runat="server" ClientInstanceName="vs_new_rfq"
										ValidationGroup="new_rfq">
									</dx:aspxvalidationsummary>
									<input type='hidden' ID="hid_rfq_id" class="hid_id" runat="server" value='<%# bind("id") %>' />
								</td>
							</tr>
						</table>
					</dx:panelcontent>
				</PanelCollection>
			</dx:aspxpanel>
		</EditForm>
		<DetailRow>
			<iframe runat="server" src="" width="100%" height="500" frameborder="0" id="i_rfq_detail"></iframe>
		</DetailRow>
	</Templates>
	<SettingsEditing Mode="PopupEditForm" />
	<SettingsDetail AllowOnlyOneMasterRowExpanded="True" ShowDetailRow="True" ExportMode="Expanded" />
	<ClientSideEvents ColumnSorting="function(s, e) {
	if(s.IsEditing())
		{
		e.cancel		= true;
		}
}" />
				<SettingsBehavior EnableRowHotTrack="True" />
</dx:ASPxGridView>
 <asp:SqlDataSource runat="server" ID="ds_rfq" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id,
URLDECODE(name) name,
business_unit_id,
status,
MEMBER_NAME(member_id) member,
date_created,
date_open,
date_close,
(SELECT group_concat(vendor_id) FROM rfq_vendor_list WHERE rfq_header_id = a.id) vendors,
URLDECODE(notes) notes,
(select count(*) from rfq_vendor_list where rfq_header_id = a.id) invited,
(select count(*) from rfq_vendor_list where rfq_header_id = a.id and date_verified is not null) responded
FROM rfq_header a WHERE business_unit_id = @business_unit_id and (date_close > (curdate()-interval 1 month) or status ='Building')
ORDER BY id DESC">
	 <SelectParameters>
		 <asp:SessionParameter Name="@business_unit_id" SessionField="working_warehouse_bu_id" />
	 </SelectParameters>
 </asp:SqlDataSource>
		
			<dx:ASPxCallback ID="cb_save" runat="server" ClientInstanceName="cb_save" oncallback="cb_save_Callback">
												<ClientSideEvents BeginCallback="function(s, e) {
												please_wait('Start');
	b_start.SetEnabled(false);
}" CallbackComplete="function(s, e) {
	if(e.result == &quot;SUCCESS&quot;)
		{
		alert(&quot;Saved.&quot;);
		gv_rfq.CancelEdit();
		gv_rfq.PerformCallback();
		}
else
		{
		alert(e.result);
		}
}" CallbackError="function(s, e) {
												please_wait('Stop');
	b_start.SetEnabled(true);
}" EndCallback="function(s, e) {
												please_wait('Stop');
	b_start.SetEnabled(true);
	
}" />
			</dx:ASPxCallback>
		
		</ContentTemplate>
		</asp:UpdatePanel>
			<dx:ASPxPopupControl runat="server" PopupHorizontalAlign="WindowCenter" Theme="NETheme01" PopupVerticalAlign="TopSides" Modal="True" ScrollBars="Vertical" ClientInstanceName="pop_rfq_preview" AnimationType="None" HeaderText="Vendor Line Items" MaxHeight="768px" Width="1024px" ID="pop_rfq_preview" Height="768px">
<ClientSideEvents Closing="function(s, e) {
	gv_vendors.Refresh();
}" Shown="function(s, e) {
	gv_rfq_detail.PerformCallback(gv_rfq_detail.cp_rfq_status);
	rfq_page.SetActiveTabIndex(0);
}"></ClientSideEvents>

<ModalBackgroundStyle BackColor="White"></ModalBackgroundStyle>
<ContentCollection>
<dx:popupcontrolcontentcontrol runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxLabel ID="lbl_vendor_name" runat="server" Font-Bold="True" Text="Vendor: " Width="100px" Theme="NETheme01">
	</dx:ASPxLabel>
	<dx:ASPxLabel ID="lb_vendor_name" runat="server" ClientInstanceName="lbl_vendor">
	</dx:ASPxLabel>
	<br />
	<dx:ASPxLabel ID="lbl_contact" runat="server" Font-Bold="True" Text="Contact:" Width="100px" Theme="NETheme01">
	</dx:ASPxLabel>
	<dx:ASPxLabel ID="lb_contact" runat="server" ClientInstanceName="lbl_contact" Theme="NETheme01">
	</dx:ASPxLabel>
	<br />
	<dx:ASPxLabel ID="lbl_email" runat="server" Font-Bold="True" Text="Email:" Width="100px" Theme="NETheme01">
	</dx:ASPxLabel>
	<dx:ASPxLabel ID="lb_email" runat="server" ClientInstanceName="lbl_email" Theme="NETheme01">
	</dx:ASPxLabel>
	<dx:ASPxPageControl ID="rfq_page" runat="server" ActiveTabIndex="0" Width="100%" ClientInstanceName="rfq_page" Theme="NETheme01">
		<TabPages>
			<dx:TabPage Text="Line Items">
				<TabImage Url="~/images/icon/icon[edit].gif">
				</TabImage>
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table cellpadding="5" cellspacing="0" width="100%">
							<tbody>
								<tr>
									<td>
										<dx:ASPxLabel ID="rfq_errors" runat="server" EncodeHtml="False" Theme="NETheme01">
										</dx:ASPxLabel>
										<br />
										<script type="text/javascript">






		function merge_rfq(obj)
			{
			var r				= [];
			$("#ctl00_cphMasterBody_rfq_tab_pop_rfq_preview_up_rfq_pop").find("tr.dxgvSelectedRow").each(function()
															{
															var vendor_code		= $(this).find(".vendor_code").find("input:text").val();
															var cost			= $(this).find(".cost").find("input:text").val();
															var qty_per			= $(this).find(".qty_per").find("input:text").val();
															var master_id		= $(this).find("td:nth-child(2)").text();
															
															if(vendor_code != undefined)
																{
																//build packet
																var row			=	{
																					master_id:		master_id,
																					vendor_code:	vendor_code,
																					cost:			cost,
																					qty_per:		qty_per
																					};
																r.push(row);
																}
															});
			cb_merge.PerformCallback(JSON.stringify(r));
			}
	</script>
										<dx:ASPxCallback ID="cb_merge" runat="server" ClientInstanceName="cb_merge" OnCallback="cb_merge_Callback">
											<ClientSideEvents BeginCallback="function(s, e) {
	please_wait('start');
}" CallbackComplete="function(s, e) {
	please_wait('stop');
	if(e.result != 'SUCCESS')
		{
		alert(e.result);
		}
	else
		{
		gv_rfq_detail.UnselectFilteredRows();
		alert('Line Data Merged Successfully');
		}
}" CallbackError="function(s, e) {
	please_wait('stop');
}" />
<ClientSideEvents CallbackComplete="function(s, e) {
	please_wait(&#39;stop&#39;);
	if(e.result != &#39;SUCCESS&#39;)
		{
		alert(e.result);
		}
	else
		{
		gv_rfq_detail.UnselectFilteredRows();
		alert(&#39;Line Data Merged Successfully&#39;);
		}
}" BeginCallback="function(s, e) {
	please_wait(&#39;start&#39;);
}" CallbackError="function(s, e) {
	please_wait(&#39;stop&#39;);
}"></ClientSideEvents>
										</dx:ASPxCallback>
										<dx:ASPxGridView ID="gv_rfq_detail" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_rfq_detail" DataSourceID="sds_rfq_detail" Theme="NETheme01" KeyFieldName="master_id" OnCustomCallback="gv_rfq_detail_CustomCallback" OnCustomJSProperties="gv_rfq_detail_CustomJSProperties" PreviewFieldName="note" Width="100%">
											<ClientSideEvents EndCallback="function(s, e) {


	lbl_vendor.SetText(s.cp_vendor_name);
	lbl_contact.SetText(s.cp_contact_name);
	lbl_email.SetText(s.cp_contact_email);
	var n		= s.cp_vendor_note == undefined ? '' : s.cp_vendor_note;
	memo_email.SetText(n);

	if (s.cp_alert != null && s.cp_alert!='')
	{
	alert(s.cp_alert);
	s.cp_alert='';

	}

if(s.GetSelectedRowCount() &gt; 0)
	{
	$(&quot;#merge_rows&quot;).removeAttr(&quot;disabled&quot;);
	}
else
	{
	$(&quot;#merge_rows&quot;).attr(&quot;disabled&quot;, true);
	}
}" SelectionChanged="function(s, e) {
if(s.GetSelectedRowCount() &gt; 0)
	{
	$(&quot;#merge_rows&quot;).removeAttr(&quot;disabled&quot;);
	}
else
	{
	$(&quot;#merge_rows&quot;).attr(&quot;disabled&quot;, true);
	}
}" />
<ClientSideEvents SelectionChanged="function(s, e) {
if(s.GetSelectedRowCount() &gt; 0)
	{
	$(&quot;#merge_rows&quot;).removeAttr(&quot;disabled&quot;);
	}
else
	{
	$(&quot;#merge_rows&quot;).attr(&quot;disabled&quot;, true);
	}
}" EndCallback="function(s, e) {


	lbl_vendor.SetText(s.cp_vendor_name);
	lbl_contact.SetText(s.cp_contact_name);
	lbl_email.SetText(s.cp_contact_email);
	var n		= s.cp_vendor_note == undefined ? &#39;&#39; : s.cp_vendor_note;
	memo_email.SetText(n);

	if (s.cp_alert != null &amp;&amp; s.cp_alert!=&#39;&#39;)
	{
	alert(s.cp_alert);
	s.cp_alert=&#39;&#39;;

	}

if(s.GetSelectedRowCount() &gt; 0)
	{
	$(&quot;#merge_rows&quot;).removeAttr(&quot;disabled&quot;);
	}
else
	{
	$(&quot;#merge_rows&quot;).attr(&quot;disabled&quot;, true);
	}
}"></ClientSideEvents>
											<Columns>
												<dx:GridViewCommandColumn Caption="Merge" Name="Merge" 
													ShowInCustomizationForm="True" ShowSelectCheckbox="True" VisibleIndex="0" 
													Width="35px">
													<HeaderTemplate>
														<dx:ASPxCheckBox ID="ch_all" runat="server" CheckState="Unchecked" Text="All" Theme="NETheme01">
															<ClientSideEvents CheckedChanged="function(s, e) {
	gv_rfq_detail.SelectAllRowsOnPage(s.GetChecked())
}" />
														</dx:ASPxCheckBox>
													</HeaderTemplate>
													<FooterTemplate>
														<button ID="merge_rows" disabled onclick="gv_rfq_detail.PerformCallback('merge')" type="button">
															Merge
														</button>
													</FooterTemplate>
												</dx:GridViewCommandColumn>
												<dx:GridViewDataTextColumn Caption="Master ID" FieldName="master_id" ShowInCustomizationForm="True" VisibleIndex="1" Width="75px">
													<EditFormSettings VisibleIndex="1" />
<EditFormSettings VisibleIndex="1"></EditFormSettings>

													<CellStyle HorizontalAlign="Center">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Vendor Part #" FieldName="vendor_code" ShowInCustomizationForm="True" VisibleIndex="2" Width="100px">
													<EditFormSettings VisibleIndex="2" />
<EditFormSettings VisibleIndex="2"></EditFormSettings>
													<DataItemTemplate>
														<dx:ASPxTextBox ID="t_vendor_code" runat="server" CssClass="vendor_code" Text='<%# Bind("vendor_code") %>' Width="100%">
														</dx:ASPxTextBox>
													</DataItemTemplate>
													<CellStyle HorizontalAlign="Center">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Description" FieldName="description" ShowInCustomizationForm="True" VisibleIndex="3">
													<EditFormSettings VisibleIndex="3" />
<EditFormSettings VisibleIndex="3"></EditFormSettings>

													<CellStyle Font-Size="11px" HorizontalAlign="Left" wrap="True">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Cost" FieldName="cost" ShowInCustomizationForm="True" VisibleIndex="4" Width="50px">
													<EditFormSettings VisibleIndex="4" />
<EditFormSettings VisibleIndex="4"></EditFormSettings>
													<DataItemTemplate>
														<dx:ASPxTextBox ID="t_cost" runat="server" CssClass="cost" Text='<%# Eval("cost") %>' Width="100%" Theme="NETheme01">
														</dx:ASPxTextBox>
													</DataItemTemplate>
													<CellStyle HorizontalAlign="Center">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="QTY per" FieldName="qty_per" ShowInCustomizationForm="True" VisibleIndex="5" Width="50px">
													<EditFormSettings VisibleIndex="5" />
<EditFormSettings VisibleIndex="5"></EditFormSettings>
													<DataItemTemplate>
														<dx:ASPxTextBox ID="t_qty" runat="server" CssClass="qty_per" Text='<%# Eval("qty_per") %>' Width="100%" Theme="NETheme01">
														</dx:ASPxTextBox>
													</DataItemTemplate>
													<CellStyle HorizontalAlign="Center">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Merge Date" FieldName="merge_dt" 
													Name="Merge Date" ShowInCustomizationForm="True" VisibleIndex="6" Width="100px">
													<CellStyle Font-Size="11px">
													</CellStyle>
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsBehavior EnableRowHotTrack="True" />

<SettingsBehavior EnableRowHotTrack="True"></SettingsBehavior>

											<SettingsPager Mode="ShowAllRecords">
											</SettingsPager>
											<Settings ShowFooter="True" ShowPreview="True" />

<Settings ShowFooter="True" ShowPreview="True"></Settings>

											<Styles>
												<Header HorizontalAlign="Center">
												</Header>
											</Styles>
										</dx:ASPxGridView>
										<asp:SqlDataSource ID="sds_rfq_detail" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL RFQPreview(@id,@vendor_id)">
											<SelectParameters>
												<asp:SessionParameter Name="@id" SessionField="working_rfq_id" />
												<asp:SessionParameter Name="@vendor_id" SessionField="rfq_vendor_id" />
											</SelectParameters>
										</asp:SqlDataSource>
									</td>
								</tr>
							</tbody>
						</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Name="Email" Text="Email">
				<TabImage Url="~/images/icon/icon[email].gif">
				</TabImage>
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
<table cellspacing="0" cellpadding="5" width="100%">
	<tbody>
		<tr>
			<td>
					
					<dx:ASPxSplitter ID="rfq_split_email_preview" runat="server" AllowResize="False" Height="330px" ShowSeparatorImage="False" ClientInstanceName="rfq_split_email_preview">
						<Panes>
							<dx:SplitterPane>
								<ContentCollection>
									<dx:SplitterContentControl runat="server" SupportsDisabledAttribute="True">
										<dx:ASPxCallbackPanel ID="cbp_email_preview" runat="server" ClientInstanceName="cbp_email_preview"  OnCallback="cbp_email_preview_Callback" Width="100%">
<SettingsLoadingPanel Text="Refreshing Preview&amp;hellip;"></SettingsLoadingPanel>
											<panelcollection>
												<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
													<u><strong>Email Message Preview</strong></u><dx:ASPxPanel ID="email_preview" runat="server" ClientInstanceName="email_preview" Width="100%">
														<PanelCollection>
															<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																
															</dx:PanelContent>
														</PanelCollection>
													</dx:ASPxPanel>
												</dx:PanelContent>
											</panelcollection>
										</dx:ASPxCallbackPanel>
									</dx:SplitterContentControl>
								</ContentCollection>
							</dx:SplitterPane>
							<dx:SplitterPane>
								<ContentCollection>
									<dx:SplitterContentControl runat="server" SupportsDisabledAttribute="True">
				<div style="font-size:11px;font-family:Arial;padding:10px;">Add a personalized note to the e-mail. &nbsp;&nbsp;<dx:aspxmemo ID="memo_email" runat="server" Theme="NETheme01" Height="250px" Width="100%" ClientInstanceName="memo_email"></dx:aspxmemo>
									<br />
									<table><tr><td><dx:ASPxButton ID="b_reload" runat="server" AutoPostBack="False" Text="Update Preview" Theme="NETheme01">
						<ClientSideEvents Click="function(s, e) {
	cb_savenote.PerformCallback(memo_email.GetText());
}" />
<ClientSideEvents Click="function(s, e) {
	cb_savenote.PerformCallback(memo_email.GetText());
}"></ClientSideEvents>

						<Image Url="~/images/icon/icon[cancel].gif">
						</Image>
					</dx:ASPxButton>
										<dx:ASPxCallback ID="cb_savenote" runat="server" ClientInstanceName="cb_savenote" OnCallback="cb_savenote_Callback">
											<ClientSideEvents CallbackComplete="function(s, e) {
	if(e.result == &quot;SUCCESS&quot;)
		{
		cbp_email_preview.PerformCallback();
		}
else
		{
		alert(e.result);
		}
}" />
<ClientSideEvents CallbackComplete="function(s, e) {
	if(e.result == &quot;SUCCESS&quot;)
		{
		cbp_email_preview.PerformCallback();
		}
else
		{
		alert(e.result);
		}
}"></ClientSideEvents>
										</dx:ASPxCallback>
										</td><td>
					<dx:ASPxButton ID="bt_send_rfq" runat="server" ClientInstanceName="bt_send_rfq" Text="Send RFQ" AutoPostBack="False" Theme="NETheme01">
						<ClientSideEvents Click="function(s, e) {
	cb_sendemail.PerformCallback();
}" />
<ClientSideEvents Click="function(s, e) {
	cb_sendemail.PerformCallback();
}"></ClientSideEvents>

						<Image Url="~/images/icon/icon[send].gif">
						</Image>
					</dx:ASPxButton>
											<dx:ASPxCallback ID="cb_sendemail" runat="server" ClientInstanceName="cb_sendemail" OnCallback="cb_sendemail_Callback">
												<ClientSideEvents BeginCallback="function(s, e) {
	bt_send_rfq.SetEnabled(false);
}" CallbackComplete="function(s, e) {
	if(e.result == &quot;SUCCESS&quot;)
		{
		alert(&quot;Sent successfully.&quot;);
		}
else
		{
		alert(e.result);
		}
}" CallbackError="function(s, e) {
	bt_send_rfq.SetEnabled(true);
}" EndCallback="function(s, e) {
	bt_send_rfq.SetEnabled(true);
}" />
<ClientSideEvents CallbackComplete="function(s, e) {
	if(e.result == &quot;SUCCESS&quot;)
		{
		alert(&quot;Sent successfully.&quot;);
		}
else
		{
		alert(e.result);
		}
}" BeginCallback="function(s, e) {
	bt_send_rfq.SetEnabled(false);
}" EndCallback="function(s, e) {
	bt_send_rfq.SetEnabled(true);
}" CallbackError="function(s, e) {
	bt_send_rfq.SetEnabled(true);
}"></ClientSideEvents>
											</dx:ASPxCallback>
										</td></tr></table>
					
									</dx:SplitterContentControl>
								</ContentCollection>
							</dx:SplitterPane>
						</Panes>
						<Styles>
							<Pane>
								<Border BorderWidth="0px" />
<Border BorderWidth="0px"></Border>
							</Pane>
						</Styles>
						<Border BorderWidth="0px" />

<Border BorderWidth="0px"></Border>
					</dx:ASPxSplitter>
					<br />
				</div>
			</td>
		</tr>
		<tr>
			<td align="center" style="WIDTH: 33%">
				
				&nbsp;</td>
		</tr>
	</tbody>
</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>
		<ClientSideEvents ActiveTabChanged="function(s, e) {
	if(e.tab.index == 1)
		{
		cbp_email_preview.PerformCallback();
		}
}" />

<ClientSideEvents ActiveTabChanged="function(s, e) {
	if(e.tab.index == 1)
		{
		cbp_email_preview.PerformCallback();
		}
}"></ClientSideEvents>

		<TabStyle Font-Bold="True">
		</TabStyle>
	</dx:ASPxPageControl>
		
</ContentTemplate>
</asp:UpdatePanel>
</dx:popupcontrolcontentcontrol>
</ContentCollection>
</dx:ASPxPopupControl>

	
		

