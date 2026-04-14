<%@ Page Title="Member Skills" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_hr_master_skills" Codebehind="master_skills.aspx.cs" %>	



<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>


	



	
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">

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

<table width = "100%">
<tr>
<td>
<dx:ASPxGridView ID="gvskills" runat="server" ClientInstanceName="gvskills" 
		Width="100%" AutoGenerateColumns="False" DataSourceID="sqlskills" 
		Font-Names="Arial" KeyFieldName="id" 
		onrowupdating="gvtypes_RowUpdating" onrowinserting="gvtypes_RowInserting" 
		onhtmleditformcreated="gvskills_HtmlEditFormCreated" 
		onhtmldatacellprepared="gvskills_HtmlDataCellPrepared" 
		onhtmlrowprepared="gvskills_HtmlRowPrepared" 
		oncustomcallback="gvskills_CustomCallback" 
		onstartrowediting="gvskills_StartRowEditing">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
	<Columns>
		<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" 
			Width="30px">
			
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn FieldName="id" 
			ReadOnly="True" VisibleIndex="1" Width="30px" Caption="ID">
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Skill" FieldName="name" 
			VisibleIndex="2" Width="100%">
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataDateColumn Caption="Last Modified" 
			FieldName="last_modified" VisibleIndex="3" Width="100px">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
				EditFormatString="yyyy-MM-dd">
			</PropertiesDateEdit>
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataDateColumn>
		<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="6" 
			Width="50px">
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="description"
			VisibleIndex="4" Caption="Description" Visible="False">
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Required in Branch" FieldName="a" 
			VisibleIndex="7" Width="110px">
			<EditFormSettings Visible="False" />
			<CellStyle Font-Bold="True" Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataCheckColumn Caption="Can Be Trained" FieldName="can_be_trained" 
			VisibleIndex="5" Width="50px">
			<EditFormSettings Visible="False" />
			<HeaderStyle Wrap="True" />
		</dx:GridViewDataCheckColumn>
		<dx:GridViewDataTextColumn Caption="course" Visible="False" VisibleIndex="9">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataComboBoxColumn Caption="Skill Type" VisibleIndex="8" 
			Width="75px" FieldName="skilltype">
			<PropertiesComboBox>
				<Items>
					<dx:ListEditItem Text="General" Value="General" />
					<dx:ListEditItem Text="Technical" Value="Technical" />
					<dx:ListEditItem Text="Physical" Value="Physical" />
					<dx:ListEditItem Text="Clerical" Value="Clerical" />
					<dx:ListEditItem Text="Competency" Value="Competency" />
				</Items>
			</PropertiesComboBox>
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
		</dx:GridViewDataComboBoxColumn>
	</Columns>
	<SettingsBehavior ColumnResizeMode="Control" />
	<SettingsPager Mode="ShowAllRecords">
	</SettingsPager>
	<SettingsEditing EditFormColumnCount="7" Mode="PopupEditForm" />
	<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowTitlePanel="True" ShowFilterRowMenu="True" />
	<SettingsText PopupEditFormCaption="  Enter New Skill" />
	<SettingsPopup>
		<EditForm HorizontalAlign="WindowCenter" Modal="True" 
			VerticalAlign="WindowCenter" Width="900px" Height="600px" />
	</SettingsPopup>
	<Styles>
		<Header BackColor="#CCCCFF" Font-Bold="True">
		</Header>
	</Styles>
	 <StylesPopup>
		 <EditForm>
			 <Content BackColor="White">
				 <Border BorderColor="White" BorderStyle="Solid" BorderWidth="10px" />
			 </Content>
		 </EditForm>
	</StylesPopup>
	 <Templates>
                    <TitlePanel>
                      
                     <table style="width:100%;">
							<tr>
								<td>
									<dx:ASPxButton ID="btnAddType" runat="server" AutoPostBack="False" 
										Text="Add Skill" Wrap="False">
										<ClientSideEvents Click="function(s, e) {
	gvskills.AddNewRow();
}" />
									</dx:ASPxButton>
								</td>
								
								<td class="dxtcRightAlignCell" nowrap="nowrap" width="100%">
									<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										SelectCommand="Select id,name from business_unit  where active='T'">
									</asp:SqlDataSource>
								</td>
								
								<td width="100%" align="right">
									<table style="width:100%;">
										<tr>
											<td nowrap="nowrap">
												Search for Keyword -&gt;</td>
											<td>
												<dx:ASPxButtonEdit ID="ASPxButtonEdit1" runat="server" 
													ClientInstanceName="ASPxButtonEdit1">
													<ClientSideEvents ButtonClick="function(s, e) {
	gvskills.PerformCallback('x|' + s.GetText());
}" KeyDown="function(s, e) {
	      
          if (e.htmlEvent.keyCode == 13)
              s.GetButton(0).click();
    
}" />
													<Buttons>
														<dx:EditButton>
															<Image Url="~/images/icon/icon[search].gif">
															</Image>
														</dx:EditButton>
													</Buttons>
												</dx:ASPxButtonEdit>
											</td>
										</tr>
										<tr>
											<td>
												&nbsp;</td>
											<td>
												&nbsp;</td>
										</tr>
										<tr>
											<td nowrap="nowrap">
												Show skills for this branch:</td>
											<td>
												<dx:ASPxComboBox ID="ddlbranch" runat="server" ClientInstanceName="ddlbranch" 
													DataSourceID="SqlDataSource3" oninit="ddlbranch_Init" TextField="name" 
													ValueField="business_unit_id" ValueType="System.Int32">
													<ClientSideEvents SelectedIndexChanged="function(s, e) {
	gvskills.Refresh();
}" />
												</dx:ASPxComboBox>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
                    
                    </TitlePanel>
                	<EditForm>
						<table style="width:100%;">
							<tr>
								<td>
									<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
										ClientInstanceName="ASPxPageControl1" Font-Names="Arial" Width="100%">
										<TabPages>
											<dx:TabPage Text="Details">
												<ContentCollection>
													<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
														<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" BackColor="#FFFFDF" 
															ClientInstanceName="ASPxRoundPanel1" Font-Bold="True" Font-Names="Arial" 
															Font-Size="12pt" HeaderText="Skills Details" Width="100%">
															<HeaderStyle BackColor="Yellow" >
															<BorderBottom BorderStyle="None" />
															</HeaderStyle>
															
															
															
															
															
															
															<PanelCollection>
																<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																	<table style="width:100%;">
																		<tr>
																			<td class="style6">
																				<strong>ID:</strong></td>
																			<td width="100%">
																				<dx:ASPxLabel ID="lbid" runat="server" ClientInstanceName="lbid" 
																					Font-Names="Arial" Text='<%# Eval("id") %>'>
																				</dx:ASPxLabel>
																			</td>
																		</tr>
																		<tr>
																			<td class="style6" nowrap="nowrap">
																				<strong>Skill Name:</strong></td>
																			<td>
																				<dx:ASPxTextBox ID="txtskill" runat="server" BackColor="#FFFFCC" 
																					Font-Names="Arial" Height="25px" Text='<%# Eval("name") %>' Width="100%">
																				</dx:ASPxTextBox>
																			</td>
																		</tr>
																		<tr>
																			<td class="style6" valign="top">
																				<strong>Description:</strong></td>
																			<td>
																				<dx:ASPxMemo ID="mem" runat="server" ClientInstanceName="mem" 
																					Font-Names="Arial" Height="71px" Text='<%# Eval("description") %>' Width="100%">
																				</dx:ASPxMemo>
																			</td>
																		</tr>
																		<tr>
																			<td class="style6" nowrap="nowrap">
																				<strong>Status:</strong></td>
																			<td>
																				<dx:ASPxComboBox ID="ddlstatus" runat="server" Font-Names="Arial" 
																					SelectedIndex="0" Text='<%# Eval("status") %>' ClientInstanceName="ddlstatus">
																					<Items>
																						<dx:ListEditItem Selected="True" Text="Active" Value="Active" />
																						<dx:ListEditItem Text="Inactive" Value="Inactive" />
																					</Items>
																				</dx:ASPxComboBox>
																			</td>
																		</tr>
																		<tr>
																			<td class="style6" nowrap="nowrap">
																				<strong>Skill Type:</strong></td>
																			<td>
																				<dx:ASPxComboBox ID="ddltype" runat="server" OnDataBound="ddltype_DataBound" 
																					SelectedIndex="0" Text='<%# Eval("skilltype") %>' ClientInstanceName="ddltype">
																					<Items>
																						<dx:ListEditItem Selected="True" Text="General" Value="General" />
																						<dx:ListEditItem Text="Technical" Value="Technical" />
																						<dx:ListEditItem Text="Physical" Value="Physical" />
																						<dx:ListEditItem Text="Clerical" Value="Clerical" />
																						<dx:ListEditItem Text="Competency" Value="Competency" />
																					</Items>
																				</dx:ASPxComboBox>
																			</td>
																		</tr>
																		<tr>
																			<td class="style6" nowrap="nowrap">
																				<strong>Can Be Trained:</strong></td>
																			<td>
																				<dx:ASPxCheckBox ID="chkcanbetrained" runat="server" CheckState="Unchecked" 
																					ClientInstanceName="chkcanbetrained" Value='<%# Eval("can_be_trained") %>' 
																					ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td class="style6" nowrap="nowrap">
																				<strong>Last Modified:</strong></td>
																			<td>
																				<dx:ASPxLabel ID="lbl_lastmodified" runat="server" 
																					ClientInstanceName="lbl_lastmodified" Text='<%# Eval("last_modified") %>'>
																				</dx:ASPxLabel>
																			</td>
																		</tr>
																		<tr>
																			<td class="style6" nowrap="nowrap" valign="top">
																				<strong>How to Acquire:</strong></td>
																			<td>
																				<dx:ASPxMemo ID="mem2" runat="server" ClientInstanceName="mem2" 
																					Font-Names="Arial" Height="71px" Text='<%# Eval("course") %>' Width="100%">
																				</dx:ASPxMemo>
																			</td>
																		</tr>
																	</table>
																</dx:PanelContent>
															</PanelCollection>
															<Border BorderColor="#FFFFDF" BorderStyle="Solid" BorderWidth="1px" />
														</dx:ASPxRoundPanel>
													</dx:ContentControl>
												</ContentCollection>
											</dx:TabPage>
											<dx:TabPage Text="Assessment Questions">
												<ContentCollection>
													<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
														<dx:ASPxRoundPanel ID="rp_review" runat="server" BackColor="#FFECD9" 
															ClientInstanceName="rp_review" Font-Bold="True" Font-Names="Arial" 
															Font-Size="12pt" HeaderText="Assessment Questions" Width="100%">
															<HeaderStyle BackColor="#FFCC99" >
															<BorderBottom BorderStyle="None" />
															</HeaderStyle>
															
															
															
															
															
															
															<PanelCollection>
																<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																	<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
																		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
master_skill_review_questions_question AS master_skill_review_questions_question,
master_skill_review_questions.master_skill_review_questions_id,
master_skill_review_questions.master_skill_review_questions_status,
master_skill_review_questions.master_skill_review_questions_skill_id
from master_skill_review_questions
where 
master_skill_review_questions.master_skill_review_questions_skill_id = ?id
">
																		<SelectParameters>
																			<asp:ControlParameter ControlID="hdnskill" Name="id" PropertyName="Value" />
																		</SelectParameters>
																	</asp:SqlDataSource>
																	<asp:HiddenField ID="hdnids" runat="server" />
																	<dx:ASPxGridView ID="gv_review" runat="server" AutoGenerateColumns="False" 
																		ClientInstanceName="gv_review" DataSourceID="SqlDataSource4" 
																		KeyFieldName="master_skill_review_questions_id" 
																		OnCustomCallback="gv_review_CustomCallback" 
																		OnRowDeleting="gv_review_RowDeleting" Width="100%">
																		<ClientSideEvents EndCallback="function(s, e) {
	txtnewreview.SetText(''); please_wait('stop');
}" />
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
																		<Columns>
																			<dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowDeleteButton="true"
																				ShowInCustomizationForm="True" VisibleIndex="0">
																				
																			</dx:GridViewCommandColumn>
																			<dx:GridViewDataTextColumn FieldName="master_skill_review_questions_id" 
																				ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
																				<EditFormSettings Visible="False" />
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="Question" 
																				FieldName="master_skill_review_questions_question" 
																				ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
																				<DataItemTemplate>
																					<dx:ASPxMemo ID="ASPxMemo1" runat="server" Font-Names="Arial" Height="31px" 
																						oninit="ASPxMemo1_Init" 
																						Text='<%# Eval("master_skill_review_questions_question") %>' Width="100%">
																					</dx:ASPxMemo>
																				</DataItemTemplate>
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="Status" 
																				FieldName="master_skill_review_questions_status" ShowInCustomizationForm="True" 
																				VisibleIndex="3" Width="75px">
																				<DataItemTemplate>
																					<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" oninit="ASPxComboBox2_Init" 
																						Text='<%# Eval("master_skill_review_questions_status") %>' Width="125px">
																						<Items>
																							<dx:ListEditItem Text="Active" Value="Active" />
																							<dx:ListEditItem Text="InActive" Value="InActive" />
																						</Items>
																					</dx:ASPxComboBox>
																				</DataItemTemplate>
																			</dx:GridViewDataTextColumn>
																		</Columns>
																		<SettingsBehavior ConfirmDelete="True" />
																		<SettingsPager Mode="ShowAllRecords">
																		</SettingsPager>
																		<Settings ShowTitlePanel="True" />
																		<Templates>
																			<TitlePanel>
																				<table style="width:100%;">
																					<tr>
																						<td width="100%">
																							<dx:ASPxMemo ID="txtnewreview" runat="server" ClientInstanceName="txtnewreview" 
																								Height="25px" Width="100%">
																							</dx:ASPxMemo>
																						</td>
																						<td>
																							&nbsp;</td>
																						<td>
																							<dx:ASPxButton ID="btnaddreview" runat="server" AutoPostBack="False" Text="Add">
																								<ClientSideEvents Click="function(s, e) {
	gv_review.PerformCallback();
}" />
																							</dx:ASPxButton>
																						</td>
																					</tr>
																				</table>
																			</TitlePanel>
																		</Templates>
																	</dx:ASPxGridView>
																</dx:PanelContent>
															</PanelCollection>
															<Border BorderColor="#8B8B8B" BorderStyle="Solid" BorderWidth="1px" />
														</dx:ASPxRoundPanel>
													</dx:ContentControl>
												</ContentCollection>
											</dx:TabPage>
											<dx:TabPage Text="Related Certifications">
												<ContentCollection>
													<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
														<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" BackColor="#D3EBCD" 
															HeaderText="Required Certifications" Width="100%">
															<HeaderStyle BackColor="#669900" Font-Bold="True" Font-Names="Arial" 
																Font-Size="12pt">
															<BorderBottom BorderStyle="None" />
															</HeaderStyle>
															
															
															
															
															
															
															<PanelCollection>
																<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																	<asp:SqlDataSource ID="SqlDataSource6" runat="server" 
																		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																		SelectCommand="Select 
certificate_skill_link.id id1,
certificates.certificate_name,
certificates2.certificate_name alt_cert,
certificates.notes,
certificates.expires,
certificates.is_internal   
from certificate_skill_link 
inner join certificates on certificate_skill_link.certificate_id=certificates.id
left JOIN certificates certificates2 ON certificate_skill_link.alternate_certificate_id = certificates2.id
where certificates.id = certificate_skill_link.certificate_id and certificate_skill_link.skill_id = ?id">
																		<SelectParameters>
																			<asp:SessionParameter Name="id" SessionField="gv_skill_id" />
																		</SelectParameters>
																	</asp:SqlDataSource>
																	<dx:ASPxGridView ID="gv_certs" runat="server" AutoGenerateColumns="False" 
																		ClientInstanceName="gv_certs" Font-Names="Arial" 
																		KeyFieldName="id1" 
																		OnCustomCallback="gv_certs_CustomCallback" OnRowDeleting="gv_certs_RowDeleting1" 
																		Width="100%" DataSourceID="SqlDataSource6">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
																		<Columns>
																			<dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowDeleteButton="true" ShowClearFilterButton="true"
																				ShowInCustomizationForm="True" VisibleIndex="0" Width="40px">
																				
																				
																			</dx:GridViewCommandColumn>
																			<dx:GridViewDataTextColumn Caption="Certification" 
																				FieldName="certificate_name" ShowInCustomizationForm="True" VisibleIndex="1" 
																				Width="100%">
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
																				ShowInCustomizationForm="True" VisibleIndex="3" Width="200px">
																				<CellStyle Wrap="False">
																				</CellStyle>
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataCheckColumn Caption="Is Internal" FieldName="is_internal" 
																				ShowInCustomizationForm="True" VisibleIndex="4" Width="70px">
																			</dx:GridViewDataCheckColumn>
																			<dx:GridViewDataTextColumn Caption="Expires (d)" FieldName="expires" 
																				ShowInCustomizationForm="True" VisibleIndex="5" Width="75px">
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="Alternate Certification" 
																				FieldName="alt_cert" ShowInCustomizationForm="True" VisibleIndex="2" 
																				Width="100%">
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn FieldName="id1" ShowInCustomizationForm="True" 
																				Visible="False" VisibleIndex="6">
																			</dx:GridViewDataTextColumn>
																		</Columns>
																		<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
																		<SettingsPager Mode="ShowAllRecords" Visible="False">
																		</SettingsPager>
																		<Settings ShowFilterRow="True" ShowTitlePanel="True" />
																		<Templates>
																			<TitlePanel>
																				<table style="width:100%;">
																					<tr>
																						<td align="left" class="style7">
																							Certification:</td>
																						<td width="100%">
																							<dx:ASPxComboBox ID="ddlcerts" runat="server" ClientInstanceName="ddlcerts" 
																								DataSourceID="SqlDataSource5" Font-Names="Arial" 
																								IncrementalFilteringMode="Contains" TextField="certificate_name" 
																								ValueField="id" ValueType="System.Int32" Width="100%">
																							</dx:ASPxComboBox>
																						</td>
																						<td>
																							&nbsp;</td>
																						<td>
																							<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
																								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																								SelectCommand="Select * from certificates"></asp:SqlDataSource>
																						</td>
																					</tr>
																					<tr>
																						<td align="left" class="style7">
																							Alternate Certification:</td>
																						<td width="100%">
																							<dx:ASPxComboBox ID="ddlcerts0" runat="server" ClientInstanceName="ddlcerts" 
																								DataSourceID="SqlDataSource5" Font-Names="Arial" 
																								IncrementalFilteringMode="Contains" TextField="certificate_name" 
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
																						</td>
																					</tr>
																				</table>
																			</TitlePanel>
																		</Templates>
																	</dx:ASPxGridView>
																</dx:PanelContent>
															</PanelCollection>
															<Border BorderColor="#8B8B8B" BorderStyle="Solid" BorderWidth="1px" />
														</dx:ASPxRoundPanel>
													</dx:ContentControl>
												</ContentCollection>
											</dx:TabPage>
										</TabPages>
									</dx:ASPxPageControl>
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;</td>
							</tr>
							<tr>
								<td>
									<dx:ASPxGridViewTemplateReplacement ID="Editors" runat="server" 
										ReplacementType="EditFormEditors" />
									<table style="width:100%;">
										<tr>
											<td width="100%">
												<div style="margin-top: 10px; margin-bottom: 20px; padding-bottom: 20px;">
													<div style="float: left; margin-left: 10px">
														<dx:ASPxButton ID="btnsave" runat="server" AutoPostBack="False" 
															ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' 
															CssClass="input" Text="Next -&gt;" Width="100px" ClientInstanceName="btnsave" />
													</div>
												</div>
											</td>
											<td>
												<dx:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="False" Text="Prev">
													<ClientSideEvents Click="function(s, e) {
	gvskills.PerformCallback('back');
}" />
												</dx:ASPxButton>
											</td>
											<td>
												<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Next">
													<ClientSideEvents Click="function(s, e) {
	gvskills.PerformCallback('next');

}" />
												</dx:ASPxButton>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
						
					</EditForm>
                </Templates>
	</dx:ASPxGridView>

				<asp:SqlDataSource runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="SELECT
master_skills.id,
master_skills.name AS name,
master_skills.last_modified,
master_skills.`status`,
master_skills.can_be_trained,
master_skills.course course,
master_skills.skilltype,
master_skills.description AS description,
0 as a
FROM
master_skills" 
		ID="sqlskills">
					<SelectParameters>
						<asp:ControlParameter ControlID="hdncompany" Name="business_unit_id" 
							PropertyName="Value" />
					</SelectParameters>
	</asp:SqlDataSource>

			<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										SelectCommand="Select * from cr_group">
	</asp:SqlDataSource>

			<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select * from cr_group"></asp:SqlDataSource>

			<asp:SqlDataSource ID="sqlmember" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select member_id,get_name(member_id) name from member">
	</asp:SqlDataSource>

	<asp:HiddenField ID="hdncompany" runat="server" />

	<asp:HiddenField ID="hdnskill" runat="server" />

</td>
</tr>
</table>



</asp:Content>
<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style6
		{
			font-size: small;
		}
		.style7
		{
			font-size: x-small;
		}
	</style>
</asp:Content>

