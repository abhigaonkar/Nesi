<%@ Page Title="" Language="C#" EnableTheming = "True" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_hr_member_certificates_detail" Codebehind="certificates_detail.aspx.cs" %>	

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
		ClientInstanceName="cb" oncallback="cb_Callback">
		<ClientSideEvents EndCallback="function(s, e) {
	alert(&quot;Certificate has been updated&quot;);
}" />
		<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
		Font-Names="Arial" Width="100%" Theme="NETheme01">
		<TabPages>
			<dx:TabPage Text="General">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
									<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%">
										<PanelCollection>
											<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
												<table class="style1">
													<tr>
														<td>
															<strong>ID:</strong></td>
														<td>
															<asp:Label ID="lblid" runat="server" Text="ID"></asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<strong>Certificate Name:</strong></td>
														<td>
															<dx:ASPxTextBox ID="txtname" runat="server" ClientInstanceName="txtname" 
																Width="170px" Theme="NETheme01">
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td>
															<strong>Is Internal:</strong></td>
														<td>
															<dx:ASPxCheckBox ID="chk_isinternal" runat="server" CheckState="Unchecked" 
																ClientInstanceName="chk_isinternal" ValueChecked="1" ValueType="System.Int32" 
																ValueUnchecked="0" Theme="NETheme01">
															</dx:ASPxCheckBox>
														</td>
													</tr>
													<tr>
														<td nowrap="nowrap">
															<strong>Expiry Time in Days:</strong></td>
														<td width="100%">
															<dx:ASPxTextBox ID="txtexpiry" runat="server" ClientInstanceName="ttxexpiry" 
																Width="170px" Theme="NETheme01">
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td nowrap="nowrap">
															<strong>Status:</strong></td>
														<td width="100%">
															<dx:ASPxComboBox ID="ddlstatus" runat="server" ClientInstanceName="ddlstatus" 
																Theme="NETheme01">
																<Items>
																	<dx:ListEditItem Text="Active" Value="Active" />
																	<dx:ListEditItem Text="InActive" Value="InActive" />
																</Items>
															</dx:ASPxComboBox>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<strong>Notes:</strong></td>
														<td>
															<dx:ASPxMemo ID="txtnotes" runat="server" ClientInstanceName="txtnotes" 
																Height="71px" Width="100%" Theme="NETheme01">
															</dx:ASPxMemo>
														</td>
													</tr>
													<tr>
														<td style="font-weight: 700" valign="top">
															How to A<strong>cquire:</strong></td>
														<td>
															<dx:ASPxMemo ID="txthow_to_acquire" runat="server" 
																ClientInstanceName="txthow_to_acquire" Height="35px" Theme="NETheme01" 
																Width="100%">
															</dx:ASPxMemo>
														</td>
													</tr>
													<tr>
														<td>
															Credential By State/Province:</td>
														<td>
															<dx:ASPxGridView ID="gv_editq" runat="server" AutoGenerateColumns="False" 
																ClientInstanceName="gv_editq" DataSourceID="SqlDataSource5" KeyFieldName="id" 
																OnCustomCallback="gv_editq_CustomCallback" OnRowDeleting="gv_editq_RowDeleting" 
																Width="100%" Theme="NETheme01">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" />
																</SettingsCommandButton>
																<Columns>
																	<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
																		ShowInCustomizationForm="True" VisibleIndex="0" Width="30px" ShowDeleteButton="true" ShowClearFilterButton="true">
																		
																		
																	</dx:GridViewCommandColumn>
																	<dx:GridViewDataTextColumn Caption="State / Province" 
																		FieldName="state_province" ShowInCustomizationForm="True" VisibleIndex="1" 
																		Width="150px">
																		<Settings AutoFilterCondition="Contains" />
																		<DataItemTemplate>
																			<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" 
																				DataSourceID="SqlDataSource4" oninit="ASPxComboBox2_Init1" 
																				Text='<%# Eval("state_province") %>' TextField="Prov_Desc" 
																				ValueField="prov_abbv" Width="100%" Theme="NETheme01">
																			</dx:ASPxComboBox>
																			<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
																				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
prov.prov_abbv,
prov.Prov_Desc
FROM
prov order by prov_abbv"></asp:SqlDataSource>
																		</DataItemTemplate>
																	</dx:GridViewDataTextColumn>
																	<dx:GridViewDataTextColumn Caption="Credential" FieldName="credential" 
																		ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
																		<Settings AutoFilterCondition="Contains" />
																		<DataItemTemplate>
																			<dx:ASPxTextBox ID="ASPxTextBox2" runat="server" oninit="ASPxTextBox2_Init" 
																				Text='<%# Eval("credential") %>' Width="100%" Theme="NETheme01">
																			</dx:ASPxTextBox>
																		</DataItemTemplate>
																	</dx:GridViewDataTextColumn>
																</Columns>
																<Settings ShowFilterRow="True" ShowTitlePanel="True" />
																<Templates>
																	<TitlePanel>
																		<table style="width:100%;">
																			<tr>
																				<td>
																					<dx:ASPxComboBox ID="ddl_prov" runat="server" ClientInstanceName="ddl_prov" 
																						DataSourceID="SqlDataSource6" IncrementalFilteringMode="Contains" 
																						TextField="prov_desc" ValueField="prov_abbv" ValueType="System.String" 
																						Width="150px" Theme="NETheme01">
																					</dx:ASPxComboBox>
																					<asp:SqlDataSource ID="SqlDataSource6" runat="server" 
																						ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																						ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																						SelectCommand="Select prov_abbv,prov_desc from prov"></asp:SqlDataSource>
																				</td>
																				<td class="style7" width="100%">
																					<dx:ASPxTextBox ID="addcred" runat="server" ClientInstanceName="addcred" 
																						Width="100%" Theme="NETheme01">
																					</dx:ASPxTextBox>
																				</td>
																				<td align="right">
																					<dx:ASPxButton ID="btnadd" runat="server" AutoPostBack="False" 
																						ClientInstanceName="btnadd" Text="Add" Width="75px" Theme="NETheme01">
																						<ClientSideEvents Click="function(s, e) {
	gv_editq.PerformCallback('a')
}" />
																					</dx:ASPxButton>
																				</td>
																			</tr>
																		</table>
																	</TitlePanel>
																</Templates>
															</dx:ASPxGridView>
															<asp:HiddenField ID="hdnqid" runat="server" />
															<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																SelectCommand="Select * from certificates_credential_link where certificate_id = ?id">
																<SelectParameters>
																	<asp:ControlParameter ControlID="hdnqid" Name="id" PropertyName="Value" />
																</SelectParameters>
															</asp:SqlDataSource>
														</td>
													</tr>
													<tr>
														<td>
															<dx:ASPxButton ID="btnsave" runat="server" AutoPostBack="False" 
																ClientInstanceName="btnsave" Text="Save" >
																<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback();
}" />
															</dx:ASPxButton>
														</td>
														<td>
															&nbsp;</td>
													</tr>
												</table>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxCallbackPanel>
									<br />
						<br />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="History">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<asp:SqlDataSource ID="sqlbranch" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							SelectCommand="Select ddl_name name, id business_unit_id from business_unit  where active = 'T'">
						</asp:SqlDataSource>
						<asp:SqlDataSource ID="SqlDataSource7" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							SelectCommand="Select member_id, member_fullname _name from member">
						</asp:SqlDataSource>
						<dx:ASPxGridView ID="gv_history" runat="server" Width="99%" 
							AutoGenerateColumns="False" ClientInstanceName="gv_history" Font-Names="Arial" 
							KeyFieldName="id" OnCancelRowEditing="gv_history_CancelRowEditing" 
							OnCustomCallback="gv_history_CustomCallback" 
							OnHtmlEditFormCreated="gv_history_HtmlEditFormCreated" 
							OnInitNewRow="gv_history_InitNewRow" OnRowDeleting="gv_history_RowDeleting" 
							OnStartRowEditing="gv_history_StartRowEditing" Theme="NETheme01">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
																
							<Columns>
								<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
									ShowInCustomizationForm="True" VisibleIndex="0" Width="60px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true">
									
									
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataDateColumn Caption="Date" FieldName="date" 
									ShowInCustomizationForm="True" VisibleIndex="2" Width="125px">
									<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
									</PropertiesDateEdit>
									<EditFormSettings Visible="False" />
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataComboBoxColumn Caption="Employee" FieldName="member_id" 
									ShowInCustomizationForm="True" VisibleIndex="3" Width="125px">
									<PropertiesComboBox DataSourceID="SqlDataSource7" TextField="_name" 
										ValueField="member_id" ValueType="System.Int32">
									</PropertiesComboBox>
									<Settings FilterMode="DisplayText" SortMode="DisplayText" />
									<EditFormSettings Visible="False" />
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="business_unit_id" 
									ShowInCustomizationForm="True" VisibleIndex="4" Width="100px">
									<PropertiesComboBox DataSourceID="sqlbranch" TextField="name" 
										ValueField="business_unit_id" ValueType="System.Int32">
									</PropertiesComboBox>
									<Settings AutoFilterCondition="BeginsWith" />
									<EditFormSettings Visible="False" />
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
									ShowInCustomizationForm="True" VisibleIndex="5" Width="100%">
									<Settings AutoFilterCondition="Contains" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Score" FieldName="score" 
									ShowInCustomizationForm="True" VisibleIndex="9" Width="60px">
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior ColumnResizeMode="Control" />
							<SettingsPager PageSize="25">
							</SettingsPager>
							<SettingsEditing Mode="PopupEditForm" />
							<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
								ShowHeaderFilterButton="True" ShowTitlePanel="True" />
							<SettingsText PopupEditFormCaption="Add/ Edit Certificate History" />
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
								<TitlePanel>
									<table style="width:100%;">
										<tr>
											<td>
												<dx:ASPxButton ID="btnadd" runat="server" AutoPostBack="False" Text="Add" 
													ClientInstanceName="btnadd" >
													<ClientSideEvents Click="function(s, e) {
	gv_history.AddNewRow();
}" />
												</dx:ASPxButton>
											</td>
											<td>
												&nbsp;</td>
											<td>
												&nbsp;</td>
										</tr>
									</table>
								</TitlePanel>
								<EditForm>
									<dx:ASPxCallbackPanel ID="cb0" runat="server" ClientInstanceName="cb0" 
										oncallback="cb_Callback1" Width="100%">
										<ClientSideEvents EndCallback="function(s, e) {
	gv_history.CancelEdit();
}" />
										<PanelCollection>
											<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
												<table style="width:100%;">
													<tr>
														<td>
															<strong>Date:</strong></td>
														<td>
															<dx:ASPxDateEdit ID="dte_date" runat="server" ClientInstanceName="dte_date" 
																DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																EditFormatString="yyyy-MM-dd" Theme="NETheme01">
															</dx:ASPxDateEdit>
														</td>
													</tr>
													<tr>
														<td>
															<strong>Business Unit:</strong></td>
														<td>
															<dx:ASPxComboBox ID="ddlcompany" runat="server" ClientInstanceName="ddlcompany" 
																DataSourceID="SqlDataSource1" IncrementalFilteringMode="Contains" 
																TextField="name" ValueField="business_unit_id" ValueType="System.Int32" Theme="NETheme01">
																<ClientSideEvents SelectedIndexChanged="function(s, e) {
	ddlmember.PerformCallback(s.GetValue());
}" />
															</dx:ASPxComboBox>
															<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																SelectCommand="Select id business_unit_id,ddl_name name from business_unit  where active = 'T'">
															</asp:SqlDataSource>
														</td>
													</tr>
													<tr>
														<td>
															<strong>Employee:</strong></td>
														<td>
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
														<td nowrap="nowrap">
															<strong>Score (0..100):</strong></td>
														<td>
															<dx:ASPxTextBox ID="txtscore" runat="server" ClientInstanceName="txtscore" 
																Width="170px" Theme="NETheme01">
																<MaskSettings Mask="&lt;0..100&gt;" />
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td>
															&nbsp;</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td>
															&nbsp;</td>
														<td>
															&nbsp;</td>
													</tr>
													<tr>
														<td>
															<strong>Notes:</strong></td>
														<td width="100%">
															<dx:ASPxMemo ID="memnotes" runat="server" BackColor="#FFFFCC" 
																ClientInstanceName="memnotes" Height="71px" Width="300px" Theme="NETheme01">
															</dx:ASPxMemo>
														</td>
													</tr>
													<tr>
														<td>
															<dx:ASPxButton ID="btn_save" runat="server" AutoPostBack="False" Text="Save" 
																Theme="NETheme01">
																<ClientSideEvents Click="function(s, e) {
	cb0.PerformCallback();
}" />
															</dx:ASPxButton>
														</td>
														<td>
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
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Related Training">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table style="width:100%;">
							<tr>
								<td>
									<asp:SqlDataSource ID="SqlDataSource9" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
training_header.id header_id,
training_header.name,
training_header.url,
training_header.notes,
certificate_training_link.id 
FROM
training_header
INNER JOIN certificate_training_link ON training_header.id = certificate_training_link.training_header_id
WHERE
certificate_training_link.certificate_id = ?id
">
										<SelectParameters>
											<asp:ControlParameter ControlID="hdnid" Name="id" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
									<dx:ASPxGridView ID="gv_q" runat="server" AutoGenerateColumns="False" 
										ClientInstanceName="gv_q" DataSourceID="SqlDataSource9" Font-Names="Arial" 
										KeyFieldName="id" OnCustomCallback="gv_q_CustomCallback" 
										OnRowDeleting="gv_q_RowDeleting" Theme="NETheme01" Width="99%">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
										<Columns>
											<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
												ShowInCustomizationForm="True" VisibleIndex="0" Width="25px" ShowDeleteButton="true" ShowClearFilterButton="true">
												
												
											</dx:GridViewCommandColumn>
											<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
												ShowInCustomizationForm="True" Visible="False" VisibleIndex="1" Width="40px">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Training" FieldName="name" 
												ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
												<Settings AutoFilterCondition="Contains" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
												ShowInCustomizationForm="True" VisibleIndex="4" Width="300px">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataCheckColumn Caption="URL" FieldName="url" 
												ShowInCustomizationForm="True" VisibleIndex="5" Width="75px">
												<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" 
													ValueUnchecked="0">
												</PropertiesCheckEdit>
											</dx:GridViewDataCheckColumn>
										</Columns>
										<SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
										<SettingsPager Mode="ShowAllRecords" Visible="False">
										</SettingsPager>
										<Settings ShowFilterRow="True" ShowTitlePanel="True" ColumnMinWidth="10" />
										<Templates>
											<TitlePanel>
												<asp:SqlDataSource ID="SqlDataSource10" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
training_header.id,
training_header.`name`
FROM
training_header
"></asp:SqlDataSource>
												<table style="width: 100%;">
													<tr>
														<td nowrap="nowrap">
															Link Training:</td>
														<td width="100%">
															<dx:ASPxComboBox ID="ddl_skills0" runat="server" 
																ClientInstanceName="ddl_skills" DataSourceID="SqlDataSource10" 
																IncrementalFilteringMode="Contains" TextField="name" ValueField="id" 
																ValueType="System.Int32" Width="100%">
																<ClientSideEvents SelectedIndexChanged="function(s, e) {
	
}" />
															</dx:ASPxComboBox>
														</td>
														<td>
															&nbsp;</td>
														<td>
															<dx:ASPxButton ID="btnaddskill0" runat="server" AutoPostBack="False" 
																ClientInstanceName="btnaddskill0" Text="Add">
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
								</td>
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
			<dx:TabPage Text="Related Core Responsibilities">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxGridView ID="gv_certs" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_certs" DataSourceID="SqlDataSource11" Font-Names="Arial" 
							KeyFieldName="id1" OnCustomCallback="gv_certs_CustomCallback" 
							OnRowDeleting="gv_certs_RowDeleting" Theme="NETheme01" Width="99%">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
							<Columns>
								<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
									ShowInCustomizationForm="True" VisibleIndex="0" Width="40px" ShowDeleteButton="true" ShowClearFilterButton="true">
									
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="Core Responsibilites" FieldName="name" 
									ShowInCustomizationForm="True" VisibleIndex="1" Width="100%">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
									ShowInCustomizationForm="True" VisibleIndex="2" Width="400px">
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="id1" ShowInCustomizationForm="True" 
									Visible="False" VisibleIndex="4">
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
							<SettingsPager Mode="ShowAllRecords" Visible="False">
							</SettingsPager>
							<Settings ShowFilterRow="True" ShowTitlePanel="True" />
							<Templates>
								<TitlePanel>
									<table style="width: 100%;">
										<tr>
											<td align="left" class="style7">
												&nbsp;</td>
											<td width="100%">
												<dx:ASPxComboBox ID="ddlcerts" runat="server" ClientInstanceName="ddlcerts" 
													DataSourceID="SqlDataSource12" Font-Names="Arial" 
													IncrementalFilteringMode="Contains" TextField="core_responsibility" 
													ValueField="id" ValueType="System.Int32" Width="100%">
												</dx:ASPxComboBox>
											</td>
											<td>
												&nbsp;</td>
											<td>
												<dx:ASPxButton ID="btnaddcert" runat="server" AutoPostBack="False" 
													ClientInstanceName="btnaddcert" Font-Names="Arial" Text="Add">
													<ClientSideEvents Click="function(s, e) {
	gv_certs.PerformCallback()
}" />
												</dx:ASPxButton>
												<asp:SqlDataSource ID="SqlDataSource12" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													SelectCommand="Select * from core_responsibilities where status = 'Active' order by core_responsibilities.core_responsibility">
												</asp:SqlDataSource>
											</td>
										</tr>
									</table>
								</TitlePanel>
							</Templates>
						</dx:ASPxGridView>
						<asp:SqlDataSource ID="SqlDataSource11" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT DISTINCT
cr_certificates.id id1,
cr_certificates.cr_id,
core_responsibilities.core_responsibility `name`,
core_responsibilities.description description,
cr_certificates.certificates_id
FROM
cr_certificates
INNER JOIN core_responsibilities ON cr_certificates.cr_id = core_responsibilities.id
WHERE
core_responsibilities.`status` = 'Active' and cr_certificates.certificates_id = ?id
">
							<SelectParameters>
								<asp:ControlParameter ControlID="hdnid" Name="id" PropertyName="Value" />
							</SelectParameters>
						</asp:SqlDataSource>
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
	.dxgvControl
{
	border: 1px Solid #9F9F9F;
	font: 12px Tahoma, Geneva, sans-serif;
	background-color: #F2F2F2;
	color: Black;
	cursor: default;
}

.dxgvTitlePanel
{
	font-size: 15px;
	font-weight: normal;
	padding: 3px 3px 5px;
	text-align: center;
	background-color: #ACACAC;
	color: White;
	border-bottom: 1px Solid #9F9F9F;
}

.dxeButtonEditSys 
{
    width: 170px;
}

.dxeButtonEdit
{
	background-color: white;
	border: 1px solid #9F9F9F;
}

.dxeButtonEdit
{
    cursor: default;
}

.dxeButtonEdit
{
	-webkit-tap-highlight-color: rgba(0,0,0,0);
}

.dxeButtonEdit .dxeEditArea
{
	background-color: white;
}

.dxeEditAreaSys 
{
    height: 14px;
    line-height: 14px;
    border: 0px!important;
	padding: 0px 1px 0px 0px; /* B146658 */
    background-position: 0 0; /* iOS Safari */
}

.dxeEditArea
{
	font: 12px Tahoma, Geneva, sans-serif;
	border: 1px solid #A0A0A0;
}
.dxeButtonEditButton
{
	vertical-align: middle;
	border: 1px solid #7f7f7f;
	cursor: pointer;
} 
.dxeButtonEditButton
{
	padding: 0px 2px 0px 3px;
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
	background-color: White;
	border-width: 0;
	border-collapse: separate!important;
	overflow: hidden;
	color: Black;
}
.dxgvTable
{
	-webkit-tap-highlight-color: rgba(0,0,0,0);
}

.dxgvHeader
{
	cursor: pointer;
	white-space: nowrap;
	padding: 4px 6px 5px;
	border: 1px Solid #9F9F9F;
	background-color: #DCDCDC;
	overflow: hidden;
	font-weight: normal;
	text-align: left;
}
.dxgvFilterRow
{
	background-color: #E7E7E7;
}
.dxgvCommandColumn
{
	padding: 2px;
}

.dxeTextBoxSys 
{
    border-collapse:separate!important;
}

.dxeTextBox
{
	background-color: white;
	border: 1px solid #9f9f9f;
}

.dxeTextBox
{
    cursor: default;
}

.dxeTextBox
{
	-webkit-tap-highlight-color: rgba(0,0,0,0);
}

.dxeTextBox .dxeEditArea
{
	background-color: white;
}
		.style7
		{
			font-size: x-small;
		}
	</style>
</asp:Content>


