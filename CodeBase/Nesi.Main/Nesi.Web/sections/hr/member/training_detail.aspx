<%@ Page Title="" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_hr_member_training_detail"  EnableTheming="True" Codebehind="training_detail.aspx.cs" %>	

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript">
		

 function bind_tooltips()
			{
			$(".opt1").each(function()
				{
				$(this).tip();
				});
			}
		$(document).ready(function()
			{
	//		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			bind_tooltips();
			});

function EndReqHandler()
	{
	bind_tooltips();
	
	}

   

	</script>
	<dx:ASPxCallbackPanel ID="cb" runat="server" Width="100%" 
		ClientInstanceName="cb" oncallback="cb_Callback" Theme="NETheme01">
		<ClientSideEvents EndCallback="function(s, e) {
	alert(&quot;Training has been updated&quot;);
}" />
		<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
		Font-Names="Arial" Width="100%" Theme="NETheme01">
		<TabPages>
			<dx:TabPage Text="General">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
									<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" 
										ClientInstanceName="cb1" OnCallback="cb_Callback" >
										<PanelCollection>
											<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
												<table class="style1">
													<tr>
														<td>
															<strong>ID:</strong></td>
														<td width="300px">
															<dx:ASPxLabel ID="lblid" runat="server" ClientInstanceName="lblid" Text="ID" 
																Theme="NETheme01">
															</dx:ASPxLabel>
														</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td nowrap="nowrap">
															<strong>Training Name:</strong></td>
														<td nowrap="nowrap" width="300px">
															<dx:ASPxTextBox ID="txtname" runat="server" ClientInstanceName="txtname" 
																Theme="NETheme01" Width="170px">
															</dx:ASPxTextBox>
														</td>
														<td nowrap="nowrap">
															&nbsp;</td>
													</tr>
													<tr>
														<td>
															<strong>Is Internal:</strong></td>
														<td width="300px">
															<dx:ASPxCheckBox ID="chk_isinternal" runat="server" CheckState="Unchecked" 
																ClientInstanceName="chk_isinternal" Theme="NETheme01" ValueChecked="1" 
																ValueType="System.Int32" ValueUnchecked="0">
															</dx:ASPxCheckBox>
														</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td class="style2" nowrap="nowrap">
															<strong>URL:</strong></td>
														<td width="300px">
															<dx:ASPxTextBox ID="txturl" runat="server" ClientInstanceName="txturl" 
																Theme="NETheme01" Width="170px">
															</dx:ASPxTextBox>
														</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td class="style2" nowrap="nowrap">
															<strong>Cost:</strong></td>
														<td width="300px">
															<dx:ASPxTextBox ID="txtcost" runat="server" ClientInstanceName="txtcost" 
																DisplayFormatString="c2" Theme="NETheme01" Width="170px">
															</dx:ASPxTextBox>
														</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td nowrap="nowrap">
															<strong>Phone Number:</strong></td>
														<td width="300px">
															<dx:ASPxTextBox ID="txtpn" runat="server" ClientInstanceName="txtpn" 
																Theme="NETheme01" Width="170px">
															</dx:ASPxTextBox>
														</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td nowrap="nowrap">
															<strong>Contact Info:</strong></td>
														<td width="100%">
															<dx:ASPxTextBox ID="txtci" runat="server" ClientInstanceName="txtci" 
																Theme="NETheme01" Width="350px">
															</dx:ASPxTextBox>
														</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td nowrap="nowrap">
															<strong>Quality:</strong></td>
														<td width="300px">
															<dx:ASPxSpinEdit ID="spnquality" runat="server" Height="21px" MaxValue="10" 
																Number="0" Theme="NETheme01">
															</dx:ASPxSpinEdit>
														</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td nowrap="nowrap">
															<strong>Last Modified:</strong></td>
														<td width="300px">
															
															<dx:ASPxLabel ID="lbl_last_modified" runat="server" ClientInstanceName="lbl_last_modified" 
																Theme="NETheme01">
															</dx:ASPxLabel>
														</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td valign="top">
															<strong>Notes:</strong></td>
														<td>
															<dx:ASPxMemo ID="txtnotes" runat="server" BackColor="#FFFFCC" 
																ClientInstanceName="txtnotes" Height="71px" Width="100%">
															</dx:ASPxMemo>
														</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td>
															&nbsp;</td>
														<td width="300px">
															<div ID="div_files" runat="server">
															</div>
														</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td>
															<dx:ASPxButton ID="btnsave" runat="server" AutoPostBack="False" 
																ClientInstanceName="btnsave" Text="Save" Theme="NETheme01">
																<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback();
}" />
															</dx:ASPxButton>
														</td>
														<td width="300px">
															&nbsp;</td>
														<td>
															&nbsp;</td>
													</tr>
												</table>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxCallbackPanel>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="History">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<asp:SqlDataSource ID="sqlbranch" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							SelectCommand="Select ddl_name name,id business_unit_id from business_unit  where active = 'T'">
						</asp:SqlDataSource>
						<dx:ASPxGridView ID="gv_history" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_history" Font-Names="Arial" KeyFieldName="id" 
							OnCancelRowEditing="gv_history_CancelRowEditing" 
							OnCustomCallback="gv_history_CustomCallback" 
							OnInitNewRow="gv_history_InitNewRow" OnRowDeleting="gv_history_RowDeleting" 
							OnStartRowEditing="gv_history_StartRowEditing" Theme="NETheme01" Width="100%" 
							OnHtmlEditFormCreated="gv_history_HtmlEditFormCreated">
							<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_refresh_schedule!=null &amp;&amp; s.cp_refresh_schedule=='1')
{
s.cp_refresh_schedule='0';
gv_schedule.Refresh();
}
}" />
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
							<Columns>
								<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
									ShowInCustomizationForm="True" VisibleIndex="0" Width="60px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true">
									
									
									
									<HeaderCaptionTemplate>
										<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Add">
													<ClientSideEvents Click="function(s, e) {
	gv_history.AddNewRow();
}" />
												</dx:ASPxButton>
									</HeaderCaptionTemplate>
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="12">
									
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataDateColumn Caption="Date" FieldName="date" 
									ShowInCustomizationForm="True" VisibleIndex="1" Width="125px">
									<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
									</PropertiesDateEdit>
									<EditFormSettings Visible="False" />
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataComboBoxColumn Caption="Employee" FieldName="member_id" 
									ShowInCustomizationForm="True" VisibleIndex="2" Width="125px">
									<PropertiesComboBox DataSourceID="SqlDataSource5" TextField="_name" 
										ValueField="member_id" ValueType="System.Int32">
									</PropertiesComboBox>
									<Settings SortMode="DisplayText" />
									<EditFormSettings Visible="False" />
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataComboBoxColumn Caption="Branch" FieldName="business_unit_id" 
									ShowInCustomizationForm="True" VisibleIndex="3" Width="100px">
									<PropertiesComboBox DataSourceID="sqlbranch" TextField="name" 
										ValueField="business_unit_id" ValueType="System.Int32">
									</PropertiesComboBox>
									<EditFormSettings Visible="False" />
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataTextColumn Caption="Score" FieldName="score" 
									ShowInCustomizationForm="True" VisibleIndex="4" Width="75px">
									<Settings AutoFilterCondition="LessOrEqual" />
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
									ShowInCustomizationForm="True" VisibleIndex="5" Width="100%">
									<Settings AutoFilterCondition="Contains" />
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataComboBoxColumn Caption="Membertype" FieldName="membertype_id" 
									ShowInCustomizationForm="True" VisibleIndex="6" Width="125px" Visible="False">
									<PropertiesComboBox DataSourceID="SqlDataSource6" TextField="membertype_name" 
										ValueField="membertype_id" ValueType="System.Int32">
									</PropertiesComboBox>
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataTextColumn Caption="cap_schedule_history_id" 
									FieldName="cap_training_schedule_id" ReadOnly="True" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="11" Width="50px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTimeEditColumn Caption="Start Time" FieldName="start_time" 
									ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="7" Width="70px">
									<PropertiesTimeEdit DisplayFormatString="">
									</PropertiesTimeEdit>
								</dx:GridViewDataTimeEditColumn>
								<dx:GridViewDataTextColumn Caption="End time" FieldName="end_time" 
									ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="8" Width="50px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Location" FieldName="location" 
									ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="9" Width="100px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataCheckColumn Caption="Certs Issued" FieldName="certs_issued" 
									ShowInCustomizationForm="True" VisibleIndex="10" Width="70px">
								</dx:GridViewDataCheckColumn>
							</Columns>
							<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
							<SettingsPager PageSize="25">
							</SettingsPager>
							<SettingsEditing Mode="PopupEditForm" />
							<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
								ShowHeaderFilterButton="True" ShowTitlePanel="True" />
							<SettingsText PopupEditFormCaption="Add/ Edit Training Event" 
								Title="Training Log" />
							<SettingsPopup>
								<EditForm HorizontalAlign="WindowCenter" 
									VerticalAlign="WindowCenter" />
							</SettingsPopup>
							<StylesPopup>
								<EditForm>
									<Content>
										<Paddings Padding="10px" />
									</Content>
								</EditForm>
							</StylesPopup>
							<Templates>
								<EditForm>
									<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
										oncallback="cb_Callback1" Width="100%">
										<ClientSideEvents EndCallback="function(s, e) {
if ((s.cp_alert != null) &amp;&amp; (s.cp_alert!= &quot;&quot;))
{
alert(s.cp_alert);
s.cp_alert = '';
}

if (s.cp_canceledit==&quot;1&quot;)
{
gv_history.CancelEdit();

//gv_history.UpdateEdit();
}

if (s.cp_refresh ==&quot;1&quot;)
{
//gv_history.Refresh();
}

}" />
										<PanelCollection>
											<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
												<table style="width: 100%;">
													<tr>
														<td colspan="4">
															<dx:ASPxLabel ID="lblerror" runat="server" ClientInstanceName="lblerror" 
																Font-Names="Arial" style="color: #FF0000; font-weight: 700">
															</dx:ASPxLabel>
														</td>
													</tr>
													<tr>
														<td nowrap="nowrap">
															<strong>Cap Training Schedule:</strong></td>
														<td colspan="3">
															<dx:ASPxComboBox ID="ddl_cap_schedule" runat="server" 
																ClientInstanceName="ddl_cap_schedule" DataSourceID="SqlDataSource12" 
																TextField="_name" Theme="NETheme01" 
																 ValueField="id" 
																ValueType="System.Int32">
																<ClientSideEvents SelectedIndexChanged="function(s, e) {
	cb.PerformCallback('ct_changed');
}" />
															</dx:ASPxComboBox>
															<asp:SqlDataSource ID="SqlDataSource12" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																SelectCommand="Select cap_training_schedule.id,Concat(date(cap_training_schedule.date), ' at ',cap_training_schedule.start_time) _name from cap_training_schedule where training_header_id = ?hid">
																<SelectParameters>
																	<asp:ControlParameter ControlID="hdnid" Name="hid" PropertyName="Value" />
																</SelectParameters>
															</asp:SqlDataSource>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<strong>Schedule Details:</strong></td>
														<td colspan="3">
															<dx:ASPxMemo ID="div_schedule_details" runat="server" Font-Names="Calibri" 
																Rows="5" Width="300px" ClientEnabled="False" ReadOnly="True">
																<Border BorderStyle="None" />
															</dx:ASPxMemo>
															</td>
													</tr>
													<tr>
														<td>
															&nbsp;</td>
														<td colspan="3">
															&nbsp;</td>
													</tr>
													<tr>
														<td>
															&nbsp;</td>
														<td colspan="3">
															&nbsp;</td>
													</tr>
													<tr>
														<td>
															<strong>Company:</strong></td>
														<td colspan="3">
															<dx:ASPxComboBox ID="ddlcompany" runat="server" ClientInstanceName="ddlcompany" 
																DataSourceID="SqlDataSource1" IncrementalFilteringMode="Contains" 
																TextField="name" Theme="NETheme01" 
																ValueField="business_unit_id" ValueType="System.Int32">
																<ClientSideEvents SelectedIndexChanged="function(s, e) {
	ddlmember.PerformCallback(s.GetValue());
}" />
															</dx:ASPxComboBox>
															<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																SelectCommand="Select business_unit_id,name from business_unit  where active = 'T'">
															</asp:SqlDataSource>
														</td>
													</tr>
													<tr>
														<td>
															<strong>Employee:</strong></td>
														<td colspan="3">
															<dx:ASPxComboBox ID="ddlmember" runat="server" ClientInstanceName="ddlmember" 
																DataSourceID="SqlDataSource2" IncrementalFilteringMode="Contains" 
																OnCallback="ddlmember_Callback" TextField="_name" ValueField="member_id" 
																ValueType="System.Int32" Theme="NETheme01">
															</dx:ASPxComboBox>
															<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																
																SelectCommand="Select member_id,get_name(member_id) _name from member where business_unit_id = ?cid and member_status='Active' order by get_name(member_id)">
																<SelectParameters>
																	<asp:ControlParameter ControlID="ddlcompany" Name="cid" PropertyName="Value" />
																</SelectParameters>
															</asp:SqlDataSource>
														</td>
													</tr>
													<tr>
														<td>
															<strong>Score (0-100):</strong></td>
														<td>
															<dx:ASPxTextBox ID="txtscore" runat="server" ClientInstanceName="txtscore" 
																Width="170px" Theme="NETheme01">
															</dx:ASPxTextBox>
														</td>
														<td>
															<dx:ASPxButton ID="btn_issue_cert" runat="server" AutoPostBack="False" 
																ClientInstanceName="btn_issue_cert" Text="Issue Cert(s)" Theme="NETheme01" 
																Wrap="False">
																<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback('Issue');
}" />
															</dx:ASPxButton>
														</td>
														<td width="100%">
															&nbsp;</td>
													</tr>
													<tr>
														<td>
															&nbsp;</td>
														<td>
															&nbsp;</td>
														<td>
															&nbsp;</td>
														<td width="100%">
															&nbsp;</td>
													</tr>
													<tr>
														<td>
															&nbsp;</td>
														<td colspan="3">
															&nbsp;</td>
													</tr>
													<tr>
														<td>
															<strong>Notes:</strong></td>
														<td width="100%" colspan="3">
															<dx:ASPxMemo ID="memnotes" runat="server" BackColor="#FFFFCC" 
																ClientInstanceName="memnotes" Height="71px" Width="300px"  Theme="NETheme01">
															</dx:ASPxMemo>
														</td>
													</tr>
													<tr>
														<td>
															<dx:ASPxButton ID="btn_save" runat="server" AutoPostBack="False" Text="Save" 
																UseSubmitBehavior="False" ClientInstanceName="btn_save" Theme="NETheme01">
																<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback('save');
}" />
															</dx:ASPxButton>
														</td>
														<td colspan="3">
															&nbsp;</td>
													</tr>
												</table>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxCallbackPanel>
									<br />
									<br />
								</EditForm>
								
							</Templates>
						</dx:ASPxGridView>
						<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							SelectCommand="Select member_id, get_name(member_id) _name from member">
						</asp:SqlDataSource>
						<asp:SqlDataSource ID="SqlDataSource6" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							SelectCommand="Select membertype_id,membertype_name from membertype">
						</asp:SqlDataSource>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Certificates">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<asp:SqlDataSource ID="SqlDataSource9" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							SelectCommand="SELECT
certificates.certificate_name,
certificates.notes,
certificates.expires,
certificates.is_internal,
certificate_training_link.id
FROM
certificates
INNER JOIN certificate_training_link ON certificates.id = certificate_training_link.certificate_id
WHERE
certificate_training_link.training_header_id = ?id and certificates.status='Active'
">
							<SelectParameters>
								<asp:ControlParameter ControlID="hdnid" Name="id" 
									PropertyName="Value" />
							</SelectParameters>
						</asp:SqlDataSource>
						<asp:HiddenField ID="hdnids2" runat="server" />
						<dx:ASPxGridView ID="gv_q" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_q" DataSourceID="SqlDataSource9" Font-Names="Arial" 
							KeyFieldName="id" OnCustomCallback="gv_q_CustomCallback" 
							OnRowDeleting="gv_q_RowDeleting" Theme="NETheme01" Width="100%">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
							<Columns>
								<dx:GridViewCommandColumn ButtonType="Image" Caption=" "  ShowDeleteButton="true" ShowClearFilterButton="true"
									ShowInCustomizationForm="True" VisibleIndex="0">
									
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="1" Width="40px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Certification" FieldName="certificate_name" 
									ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
									<Settings AutoFilterCondition="Contains" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
									ShowInCustomizationForm="True" VisibleIndex="4" Width="300px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataCheckColumn Caption="Is Internal" FieldName="is_internal" 
									ShowInCustomizationForm="True" VisibleIndex="5" Width="75px">
									<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" 
										ValueUnchecked="0">
									</PropertiesCheckEdit>
								</dx:GridViewDataCheckColumn>
								<dx:GridViewDataTextColumn Caption="Expires (days)" FieldName="expires" 
									ShowInCustomizationForm="True" VisibleIndex="6" Width="100px">
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior ConfirmDelete="True" />
							<SettingsPager Mode="ShowAllRecords" Visible="False">
							</SettingsPager>
							<Settings ShowFilterRow="True" ShowTitlePanel="True" />
							<Templates>
								<TitlePanel>
									<asp:SqlDataSource ID="SqlDataSource10" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
certificates.certificate_name,
id
FROM
certificates"></asp:SqlDataSource>
									<table style="width: 100%;">
										<tr>
											<td nowrap="nowrap">
												Link Certification:</td>
											<td width="100%">
												<dx:ASPxComboBox ID="ddl_skills0" runat="server" 
													ClientInstanceName="ddl_skills" DataSourceID="SqlDataSource10" 
													IncrementalFilteringMode="Contains" TextField="certificate_name" 
													ValueField="id" ValueType="System.Int32" Width="100%">
													<ClientSideEvents SelectedIndexChanged="function(s, e) {
	
}" />
												</dx:ASPxComboBox>
											</td>
											<td>
												&nbsp;</td>
											<td>
												<dx:ASPxButton ID="btnaddskill0" runat="server" AutoPostBack="False" Text="Add">
													<ClientSideEvents Click="function(s, e) {
	gv_q.PerformCallback();
}" />
												</dx:ASPxButton>
											</td>
										</tr>
									</table>
								</TitlePanel>
							</Templates>
						</dx:ASPxGridView>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Links" ClientVisible="False" Visible="False">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table style="width:100%;">
							<tr>
								<td>
									&nbsp;</td>
							</tr>
							<tr>
								<td>
									&nbsp;</td>
							</tr>
							<tr>
								<td>
									&nbsp;</td>
							</tr>
						</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Schedule">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<asp:SqlDataSource ID="SqlDataSource11" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							
							SelectCommand="Select *,(Select count(id) from training_header_history where training_header_history.cap_training_schedule_id = cap_training_schedule.id) enrolled from cap_training_schedule where training_header_id = ?id">
							<SelectParameters>
								<asp:ControlParameter ControlID="hdnid" Name="id" PropertyName="Value" />
							</SelectParameters>
						</asp:SqlDataSource>
						<dx:ASPxGridView ID="gv_schedule" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_schedule" DataSourceID="SqlDataSource11" 
							KeyFieldName="id" OnInitNewRow="gv_schedule_InitNewRow" 
							OnRowInserting="gv_schedule_RowInserting" 
							OnStartRowEditing="gv_schedule_StartRowEditing" Theme="NETheme01" Width="100%" 
							OnRowDeleting="gv_schedule_RowDeleting" OnRowUpdating="gv_schedule_RowUpdating">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
							<Columns>
								<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True"  ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true"
									VisibleIndex="0" Width="50px">
									
									
									
									<HeaderCaptionTemplate>
										<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" 
											Text="Add New">
											<ClientSideEvents Click="function(s, e) {
	gv_schedule.AddNewRow();
}" />
										</dx:ASPxButton>
									</HeaderCaptionTemplate>
								</dx:GridViewCommandColumn>
								<dx:GridViewDataDateColumn Caption="Date" FieldName="date" 
									ShowInCustomizationForm="True" VisibleIndex="1" Width="120px">
									<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
										EditFormatString="yyyy-MM-dd">
									</PropertiesDateEdit>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataTextColumn Caption="Start Time" FieldName="start_time" 
									ShowInCustomizationForm="True" VisibleIndex="2" Width="120px">
									
									<PropertiesTextEdit>
										<MaskSettings Mask="99:99" />
									</PropertiesTextEdit>
									
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="End Time" FieldName="end_time" 
									ShowInCustomizationForm="True" VisibleIndex="3" Width="120px">
									
									<PropertiesTextEdit>
										<MaskSettings Mask="99:99" />
									</PropertiesTextEdit>
									
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="training_header_id" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="8">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Location" FieldName="location" 
									ShowInCustomizationForm="True" VisibleIndex="4" Width="100%">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="7">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Enrolled" FieldName="enrolled" 
									ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Max Fill" FieldName="max_fill" 
									ShowInCustomizationForm="True" VisibleIndex="6">
									<PropertiesTextEdit>
										<MaskSettings Mask="&lt;0..100&gt;" />
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior ConfirmDelete="True" />
							<SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
							<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" 
								ShowHeaderFilterButton="True" ShowTitlePanel="True" />
							<SettingsText Title="Training Schedule" />
							<SettingsPopup>
								<EditForm Height="300px" HorizontalAlign="WindowCenter" VerticalAlign="Above" 
									Width="300px" />
							</SettingsPopup>
						</dx:ASPxGridView>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>
	</dx:ASPxPageControl>
			</dx:PanelContent>
</PanelCollection>
	</dx:ASPxCallbackPanel>
	<div 
							__designer:mapid="1507">
		<table style="width:100%;" __designer:mapid="1508">
			<tr __designer:mapid="1509">
				<td width="100%" __designer:mapid="150a">
					<div class="dxtvControl_Office2003Blue" style="margin-left: 10px" 
											__designer:mapid="150b">
						<asp:HiddenField ID="hdnid" runat="server" />
					</div>
				</td>
				<td __designer:mapid="150d">
					&nbsp;</td>
				<td __designer:mapid="1510">
					&nbsp;</td>
			</tr>
		</table>

	</div>




</asp:Content>

<asp:Content ID="Content4" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style1
		{
			width: 100%;
		}
		.style2
		{
			height: 19px;
		}
	.dxgvControl,
.dxgvDisabled
{
	border: 1px Solid #9F9F9F;
	font: 12px Tahoma, Geneva, sans-serif;
	background-color: #F2F2F2;
	color: Black;
	cursor: default;
}

.dxgvTitlePanel, 
.dxgvTable caption
{
	font-size: 15px;
	font-weight: normal;
	padding: 3px 3px 5px;
	text-align: center;
	background-color: #ACACAC;
	color: White;
	border-bottom: 1px Solid #9F9F9F;
}
.dxeTrackBar, 
.dxeIRadioButton, 
.dxeButtonEdit, 
.dxeTextBox, 
.dxeRadioButtonList, 
.dxeCheckBoxList, 
.dxeMemo, 
.dxeListBox, 
.dxeCalendar, 
.dxeColorTable
{
	-webkit-tap-highlight-color: rgba(0,0,0,0);
}

.dxeTextBox,
.dxeButtonEdit,
.dxeIRadioButton,
.dxeRadioButtonList,
.dxeCheckBoxList
{
    cursor: default;
}

.dxeButtonEdit
{
	background-color: white;
	border: 1px solid #9F9F9F;
}

.dxeButtonEditSys 
{
    width: 170px;
}

.dxeButtonEdit .dxeEditArea
{
	background-color: white;
}

.dxeEditArea
{
	font: 12px Tahoma, Geneva, sans-serif;
	border: 1px solid #A0A0A0;
}
.dxeEditAreaSys 
{
    height: 14px;
    line-height: 14px;
    border: 0px!important;
	padding: 0px 1px 0px 0px; /* B146658 */
    background-position: 0 0; /* iOS Safari */
}
.dxeButtonEditButton,
.dxeSpinIncButton,
.dxeSpinDecButton,
.dxeSpinLargeIncButton,
.dxeSpinLargeDecButton
{
	padding: 0px 2px 0px 3px;
}
.dxeButtonEditButton,
.dxeCalendarButton,
.dxeSpinIncButton,
.dxeSpinDecButton,
.dxeSpinLargeIncButton,
.dxeSpinLargeDecButton
{
	vertical-align: middle;
	border: 1px solid #7f7f7f;
	cursor: pointer;
} 
.dxbButton
{
	color: #000000;
	font: normal 12px Tahoma, Geneva, sans-serif;
	vertical-align: middle;
	border: 1px solid #7F7F7F;
	padding: 1px;
	cursor: pointer;
}
.dxgvTable
{
	-webkit-tap-highlight-color: rgba(0,0,0,0);
}

.dxgvTable
{
	background-color: White;
	border-width: 0;
	border-collapse: separate!important;
	overflow: hidden;
	color: Black;
}

.dxgvFilterRow
{
	background-color: #E7E7E7;
}
.dxgvCommandColumn
{
	padding: 2px;
}
.dxeTextBox,
.dxeMemo
{
	background-color: white;
	border: 1px solid #9f9f9f;
}

.dxeTextBoxSys, 
.dxeMemoSys 
{
    border-collapse:separate!important;
}

.dxeTextBox .dxeEditArea
{
	background-color: white;
}
		</style>
</asp:Content>


