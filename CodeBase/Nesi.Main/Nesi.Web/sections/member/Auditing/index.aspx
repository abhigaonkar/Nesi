<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_member_auditing" Title="Auditing" EnableTheming="True" Theme="NETheme01" Codebehind="index.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:content id="Content1" contentplaceholderid="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:content>
<asp:content id="Content2" contentplaceholderid="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphMasterSubMenu" runat="Server">
</asp:content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<asp:ScriptManager ID="sm" runat="server" />
	<dx:ASPxPageControl ID="pc" runat="server" ActiveTabIndex="0" Width="100%" Theme="NETheme01">
		<TabPages>
			<dx:TabPage Text="Audits">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">

						<dx:ASPxGridView ID="gv_history" runat="server" AutoGenerateColumns="False"
							ClientInstanceName="gv_history"
							KeyFieldName="audithistory_headerid"
							OnHtmlEditFormCreated="gv_history_HtmlEditFormCreated"
							OnRowDeleting="gv_history_RowDeleting" OnRowInserting="gv_history_RowInserting" oncancelrowediting="gv_history_CancelRowEditing"
							OnRowUpdating="gv_history_RowUpdating" Width="100%" Theme="NETheme01">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																	<CancelButton Image-Url="~/images/icon/icon[cancel].gif" Image-Width="16px" Text="Cancel" />
																</SettingsCommandButton>

							<Columns>
								<dx:GridViewCommandColumn ButtonType="Image" Caption=" "
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="0" Width="75px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true" ShowCancelButton="true">
									
									
									
									
									
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataDateColumn Caption="Date"
									FieldName="audithistory_header_dateofaudit" ShowInCustomizationForm="True"
									VisibleIndex="1" Width="100px">
									<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom"
										EditFormatString="yyyy-MM-dd">
									</PropertiesDateEdit>
									<EditFormSettings Visible="False" />
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="branch"
									ShowInCustomizationForm="True" VisibleIndex="2" Width="200px">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataComboBoxColumn Caption="Type" FieldName="audithistory_type"
									ShowInCustomizationForm="True" VisibleIndex="3" Width="200px">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataComboBoxColumn Caption="Auditor" FieldName="auditor"
									ShowInCustomizationForm="True" VisibleIndex="4">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataTextColumn Caption="Overall Score"
									FieldName="audithistory_header_score" ShowInCustomizationForm="True"
									VisibleIndex="5" Width="80px">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="audithistory_headerid"
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="5">
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
							<SettingsPager PageSize="50">
							</SettingsPager>
							<SettingsEditing Mode="PopupEditForm" />
							<Settings ColumnMinWidth="10" ShowFilterRow="True" ShowFooter="True"
								ShowTitlePanel="True" ShowFilterBar="Visible" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
							<SettingsPopup>
								<EditForm Height="600px" HorizontalAlign="WindowCenter" Modal="True"
									VerticalAlign="WindowCenter" Width="900px" />
							</SettingsPopup>
							<Styles>
								<Cell Wrap="False">
								</Cell>
							</Styles>
							<Templates>
								<EditForm>
									<asp:HiddenField ID="hdnNewCompanyID" runat="server" />

									<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" OnCallback="cb_Callback" Width="100%">
										<Paddings Padding="5px" />
										<PanelCollection>
											<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
												<table style="width: 100%;">
													<tr>
														<td width="150">Score:</td>
														<td>
															<dx:ASPxLabel ID="lblscore" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="16pt" Text="0.0%">
															</dx:ASPxLabel>
														</td>
													</tr>
													<tr>
														<td>Business Unit:</td>
														<td>
															<dx:ASPxComboBox ID="ddlcompany" runat="server" DataSourceID="SqlDataSource2" TextField="name" clientinstancename="ddlcompany" ValueField="id" ValueType="System.Int32">
															</dx:ASPxComboBox>
														</td>
													</tr>
													<tr>
														<td>Auditor:</td>
														<td>
															<dx:ASPxComboBox ID="ddlauditor" runat="server" DataSourceID="SqlDataSource3" AnimationType="None" IncrementalFilteringMode="StartsWith" TextField="name" ValueField="member_id" ValueType="System.Int32">
															</dx:ASPxComboBox>
														</td>
													</tr>
													<tr>
														<td>Date of Audit:</td>
														<td>
															<dx:ASPxDateEdit ID="dtedate" runat="server" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
															</dx:ASPxDateEdit>
														</td>
													</tr>
													<tr>
														<td>Type of Audit:</td>
														<td>
															<dx:ASPxComboBox ID="ddl_audittype" runat="server" AnimationType="None" IncrementalFilteringMode="StartsWith" TextField="name" ValueField="member_id" ValueType="System.Int32">
																<Items>
																	<dx:ListEditItem Text="Annual Branch Audit" Value="1" />
																	<dx:ListEditItem Text="Quarterly Branch Safety Audit" Value="2" />
																</Items>
															</dx:ASPxComboBox>
														</td>
													</tr>
												</table>
												<dx:aspxcallbackpanel ID="cbp_tree" runat="server" clientinstancename="cbp_tree" oncallback="cbp_tree_Callback">
													<panelcollection>
														<dx:panelcontent runat="server">
															<dx:ASPxTreeView ID="tree" runat="server" AllowCheckNodes="True" ClientVisible="False" ShowExpandButtons="False">
															</dx:ASPxTreeView>
														</dx:panelcontent>
													</panelcollection>
													<clientsideevents begincallback="function(s,e){please_wait('start');}" endcallback="function(s,e){please_wait('stop');}" callbackerror="function(s,e){please_wait('stop');}" />
												</dx:aspxcallbackpanel>
												<asp:HiddenField ID="lblid" runat="server"></asp:HiddenField>
												<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select member_id, member_fullname name from member where member_status = 'Active'  ORDER BY member_fullname"></asp:SqlDataSource>
												
												<dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" Text="Cancel" Wrap="False">
													<ClientSideEvents Click="function(s, e) {gv_history.CancelEdit();}" />
												</dx:ASPxButton>
															
												<dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="False" Text="Save" Wrap="False">
													<ClientSideEvents Click="function(s, e) {gv_history.UpdateEdit();}" />
												</dx:ASPxButton>
												<dx:ASPxButton ID="btn_show" runat="server" AutoPostBack="False" Text="Save" Wrap="False">
														<ClientSideEvents Click="function(s, e) {cb.PerformCallback();}" />
												</dx:ASPxButton>
											</dx:PanelContent>
										</PanelCollection>
									<clientsideevents endcallback="function(s,e){if(typeof(s.cpcloseme) !== 'undefined'){gv_history.CancelEdit();} }" />
									</dx:ASPxCallbackPanel>
								</EditForm>
								<TitlePanel>
									<dx:ASPxButton ID="btnnewaudit" runat="server" AutoPostBack="False" Text="Add">
										<ClientSideEvents Click="function(s, e) {
	gv_history.AddNewRow();
}" />
									</dx:ASPxButton>
								</TitlePanel>
							</Templates>
						</dx:ASPxGridView>

					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Types">
				<ContentCollection>
					<dx:ContentControl runat="server">
						<dx:ASPxGridView ID="gv_typeadmin" runat="server" 
						AutoGenerateColumns="False" DataSourceID="sds_typeadmin" KeyFieldName="id" Width="100%" Theme="NETheme01" ClientInstanceName="gv_typeadmin"
						 OnCommandButtonInitialize="gv_typeadmin_CommandButtonInitialize" OnCellEditorInitialize="gv_typeadmin_CellEditorInitialize" OnInitNewRow="gv_typeadmin_InitNewRow">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
							<Columns>
								<dx:GridViewCommandColumn ButtonType="Image" ShowDeleteButton="True" ShowEditButton="True" ShowInCustomizationForm="True" ShowNewButtonInHeader="True" VisibleIndex="0" Width="50px">
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" Visible="false" ShowInCustomizationForm="True" VisibleIndex="1" Width="50px">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Name" FieldName="name" ShowInCustomizationForm="True" VisibleIndex="2">
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsEditing Mode="EditForm">
							</SettingsEditing>
							
							<Templates>
								<EditForm>
									<dx:ASPxTextBox ID="tb_name" runat="server" Caption="Type Name" Text='<%# Bind("name") %>' Width="170px">
									</dx:ASPxTextBox>
									<asp:HiddenField runat="server" ID="hid_id" Value='<%# Bind("id") %>' />
									<table>
										<tr>
											<td>
									<dx:ASPxButton ID="bt_cancel" runat="server" AutoPostBack="False" Text="Cancel">
										<ClientSideEvents Click="function(s, e) {
	gv_typeadmin.CancelEdit();
}" />
										<Image Height="16px" Url="/images/icon/icon[cancel].gif" Width="16px">
										</Image>
									</dx:ASPxButton>
											</td>
											<td>
									<dx:ASPxButton ID="bt_save" runat="server" AutoPostBack="False" Text="Save">
										<ClientSideEvents Click="function(s, e) {
	gv_typeadmin.UpdateEdit();
}" />
										<Image Height="16px" Url="/images/icon/icon[save].gif" Width="16px">
										</Image>
									</dx:ASPxButton>
											</td>
										</tr>
									</table>
									<br />
									<br />
									<div id="grouplink_title" runat="server">
									<b>
									Group/Type Link</b><br />
									<br /></div>
									<dx:ASPxGridView ID="gv_grouplink" runat="server" AutoGenerateColumns="False" DataSourceID="sds_grouplink" KeyFieldName="id" Theme="NETheme01" Width="250px" ClientInstanceName="gv_grouplink" OnCustomCallback="gv_grouplink_CustomCallback" OnCommandButtonInitialize="gv_grouplink_CommandButtonInitialize">
										
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
										<Columns>
											<dx:GridViewCommandColumn ButtonType="Image" ShowDeleteButton="True" VisibleIndex="0" Width="50px" >
											</dx:GridViewCommandColumn>
											<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" VisibleIndex="1" Visible="False">
												<EditFormSettings Visible="False" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="group_name" VisibleIndex="2" Caption="Group">
											</dx:GridViewDataTextColumn>
										</Columns>
										<SettingsBehavior ConfirmDelete="True" />
										<Settings ShowTitlePanel="True" />
							
										<Templates>
											<TitlePanel>
									<dx:ASPxComboBox ID="cb_groups" runat="server" DataSourceID="sds_groups" TextField="name" ValueField="id" Width="350px" Theme="NETheme01">
										<ClientSideEvents ButtonClick="function(s, e) {
	var id = s.GetValue();
	if(id == null)
		{
		alert('Please select a group to add');
		}
	else
		{
	s.SetSelectedIndex(-1);
	gv_grouplink.PerformCallback(id);
		}
}" />
										<Buttons>
											<dx:EditButton Width="100%">
												<Image Height="16px" Url="/images/icon/icon[add].gif" Width="16px">
												</Image>
											</dx:EditButton>
										</Buttons>
									</dx:ASPxComboBox>
											</TitlePanel>
										</Templates>
									</dx:ASPxGridView>
									<br />
									<br />
									<br />
									<asp:SqlDataSource ID="sds_grouplink" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
SelectCommand="SELECT a.id, c.audititem_id group_id, c.AuditItem_Name AS group_name FROM audit_typegroup_link a LEFT OUTER JOIN audit_type b ON a.type_id = b.id LEFT OUTER JOIN audititem c ON a.group_id = c.AuditItem_ID WHERE a.type_id = ?id" 
DeleteCommand="DELETE FROM audit_typegroup_link WHERE id = ?id">
										<DeleteParameters>
											<asp:Parameter Name="id" />
										</DeleteParameters>
										<SelectParameters>
											<asp:ControlParameter ControlID="hid_id" Name="id" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
									<asp:SqlDataSource ID="sds_groups" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT audititem_id id, audititem_name name  FROM audititem WHERE audititem_id NOT IN (select group_id FROM audit_typegroup_link WHERE type_id = ?id)">
										<SelectParameters>
											<asp:ControlParameter ControlID="hid_id" Name="id" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
								</EditForm>
							</Templates>
						</dx:ASPxGridView>
						<asp:SqlDataSource ID="sds_typeadmin" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, name FROM audit_type" InsertCommand="INSERT INTO audit_type (name) VALUES (?name)" UpdateCommand="UPDATE audit_type SET name = ?name WHERE id = ?id" DeleteCommand="DELETE FROM audit_type WHERE id = ?id LIMIT 1">
							<DeleteParameters>
								<asp:Parameter Name="id" />
							</DeleteParameters>
							<InsertParameters>
								<asp:Parameter Name="name" />
							</InsertParameters>
						</asp:SqlDataSource>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Items">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table style="width: 100%;">
							<tr>
								<td>
									<dx:ASPxGridView ID="gv_admin" runat="server" AutoGenerateColumns="False"
										DataSourceID="Sqlauditratingdesc" KeyFieldName="audititem_ratingdesc_id"
										Width="100%" OnRowDeleting="gv_admin_RowDeleting" OnRowInserting="gv_admin_RowInserting"
										OnRowUpdating="gv_admin_RowUpdating" Theme="NETheme01" OnHtmlEditFormCreated="gv_admin_HtmlEditFormCreated">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
										<Columns>
											<dx:GridViewCommandColumn ButtonType="Image" Caption=" "
												ShowInCustomizationForm="True" VisibleIndex="0" Width="100px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true">
												
												
												
												
												<CellStyle Wrap="False">
												</CellStyle>
												<HeaderTemplate>
													<dx:ASPxButton ID="btnExport" runat="server" OnClick="btnExport_Click" UseSubmitBehavior="false"
														Text="Export">
													</dx:ASPxButton>
												</HeaderTemplate>
											</dx:GridViewCommandColumn>
											<dx:GridViewDataCheckColumn Caption="Active"
												FieldName="audititem_ratingdesc_active" ShowInCustomizationForm="True"
												VisibleIndex="1" Width="60px">
												<PropertiesCheckEdit AllowGrayedByClick="False" ValueChecked="1"
													ValueType="System.Int32" ValueUnchecked="0">
												</PropertiesCheckEdit>
											</dx:GridViewDataCheckColumn>
											<dx:GridViewDataTextColumn Caption="ID" FieldName="audititem_ratingdesc_id"
												ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2" Visible="false" Width="50px">
												<EditFormSettings Visible="False" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Type" FieldName="type_name" ShowInCustomizationForm="True" VisibleIndex="3">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Group" FieldName="group_name" ShowInCustomizationForm="True" SortIndex="0" SortOrder="Ascending" VisibleIndex="4" Width="100px">
												<PropertiesTextEdit Width="100px">
												</PropertiesTextEdit>
												<CellStyle Wrap="False">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Description"
												FieldName="item" ShowInCustomizationForm="True"
												VisibleIndex="5" Width="100%">
												<PropertiesTextEdit Width="100%">
												</PropertiesTextEdit>
											</dx:GridViewDataTextColumn>
										</Columns>
										<SettingsBehavior EnableRowHotTrack="True" />
										<SettingsPager PageSize="50">
										</SettingsPager>
										<SettingsEditing EditFormColumnCount="1" Mode="EditForm" />
										<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" />
										<Templates>
											<EditForm>
												<dx:ASPxComboBox runat="server" ID="combo_type" Caption="Type" DataSourceID="sds_typeadmin" ValueField="id" TextField="name" ValueType="System.Int32">
													<CaptionCellStyle Width="125px">
													</CaptionCellStyle>
													<ClientSideEvents SelectedIndexChanged="function(s,e){combo_group.PerformCallback(s.GetValue());}" />
												</dx:ASPxComboBox><br />
												<dx:ASPxComboBox runat="server" ID="combo_group" Caption="Group" ClientInstanceName="combo_group" ValueType="System.Int32" ValueField="group_id" TextField="group_name" OnCallback="combo_group_Callback">
													<CaptionCellStyle Width="125px">
													</CaptionCellStyle>
												</dx:ASPxComboBox><br />
												<dx:ASPxMemo runat="server" ID="memo_item" Caption="Item" Width="500px" Value='<%# Bind("item") %>' ValueType="System.Int32" Height="50px">
													<CaptionCellStyle Width="125px">
													</CaptionCellStyle></dx:ASPxMemo><br />
												<dx:ASPxCheckBox runat="server" ID="cb_active" Text="Active" CheckState="Unchecked" Value='<%# Bind("audititem_ratingdesc_active") %>'></dx:ASPxCheckBox>

												<div style="margin-top: 10px; margin-bottom: 20px; padding-bottom: 20px;">
													<div style="float: left; margin-left: 5px">
														<dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" Text="Cancel" Width="100px" UseSubmitBehavior="false"
															ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' />
													</div>
													<div style="float: left; margin-left: 10px">
														<dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" Text="Save" Width="100px"
															CssClass="input" ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' />
													</div>
												</div>
											</EditForm>
										</Templates>
									</dx:ASPxGridView>
								</td>
								<td>&nbsp;</td>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<td>
									<dx:ASPxGridViewExporter ID="export" runat="server"
										GridViewID="gv_admin">
									</dx:ASPxGridViewExporter>
								</td>
								<td>&nbsp;</td>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<td>
									<asp:SqlDataSource ID="Sqlauditratingdesc" runat="server"
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
										SelectCommand="
										SELECT 
											a.audititem_ratingdesc_id, 
											a.audititem_ratingdesc_audititem_id group_id, 
											a.audititem_ratingdesc_name item, 
											a.audititem_ratingdesc_active,
											b.name type_name,
											b.id type_id,
											c.audititem_name group_name
										FROM 
											audititem_ratingdesc a 
										LEFT JOIN 
											audit_type b ON a.AuditItem_RatingDesc_type = b.id
										LEFT JOIN
											audititem c ON a.audititem_ratingdesc_audititem_id = c.audititem_id
                                        where a.auditItem_ratingDesc_active=1
                                        "></asp:SqlDataSource>
									<asp:SqlDataSource ID="sqlAuditItem" runat="server"
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
										SelectCommand="SELECT audititem_id, audititem_name FROM audititem WHERE audititem_active = 1"></asp:SqlDataSource>
								</td>
								<td>&nbsp;</td>
								<td>&nbsp;</td>
							</tr>
						</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>
	</dx:ASPxPageControl>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>

</asp:Content>


