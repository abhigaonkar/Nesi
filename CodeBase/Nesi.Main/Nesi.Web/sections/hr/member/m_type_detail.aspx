<%@ Page Title="" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true"  Inherits="sections_hr_member_m_type_detail" EnableTheming="True" Theme="NETheme01" Codebehind="m_type_detail.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>


<asp:Content ID="content54" ContentPlaceHolderID="header_placeholder" runat="server">
    <style type="text/css">

		.fin_wrap_it
		{
			white-space:normal !important;
		}
       
		</style>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">

	<script type="text/javascript">

	function print_mreview(mid) {
		//	boing("/sections/reports/invoice_preview/index.aspx?id=" + id, 'invoice_preview', 850, 850);
		
		boing("print_review.aspx?rid=0&locked=0&is_worksheet=1&mtid=" + mid, 'printrev', 900, 900);
	}

		$(document).ready(function () {
			page_obj.update_panel_progress.bind();
		});
	</script>
	
    
    
     <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
	<dx:ASPxPageControl ID="pc" runat="server" ActiveTabIndex="0" 
		Width="100%" EnableCallBacks="True">
		<TabPages>
			<dx:TabPage Text="Details">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						
									<table cellpadding="3px"  
										style="width: 100%;  ">
										<tr>
											<td>
												ID:</td>
											<td colspan="1">
												<dx:ASPxLabel ID="lblid" runat="server" ClientInstanceName="lblid">
												</dx:ASPxLabel>
												
											</td>
											<td width="50px">
												</td>
											<td nowrap="nowrap">
												Employees Existing Today (Across Company):</td>
											<td>
												<dx:ASPxTextBox ID="txtexisting" runat="server" Width="120px" 
													Theme="NETheme01" ClientInstanceName="txtexisting" ClientEnabled="False">
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td>
												Name:</td>
											<td colspan="1">
												<dx:ASPxTextBox ID="txtname" runat="server" Width="160px" Theme="NETheme01">
												    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
												</dx:ASPxTextBox>
											</td>
											<td width="50px">
												&nbsp;</td>
											<td colspan="1">
												By default this type will report to:</td>
											<td>
												<dx:ASPxComboBox ID="ddlreportsto" runat="server" 
													DataSourceID="sqlmember" TextField="membertype_name" ValueField="membertype_id" 
													ValueType="System.Int32" Theme="NETheme01" Width="120px">
												</dx:ASPxComboBox>
											</td>
										</tr>
										<tr>
											<td >
											</td>
											<td colspan="1">
												<dx:ASPxCheckBox ID="chkelevated" runat="server" CheckState="Unchecked" 
													ClientVisible="False" Theme="NETheme01">
												</dx:ASPxCheckBox>
											</td>
											<td width="50px" >
											</td>
											<td >
												Base Chargeout Rate (for your business unit):</td>
											<td >
												<dx:ASPxTextBox ID="txtbasechargeout" runat="server" Width="120px" 
													Theme="NETheme01">
												    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td colspan="1" nowrap="nowrap">
												Show on Rates Sheet:</td>
											<td>
												<dx:ASPxCheckBox ID="chkrates" runat="server" CheckState="Unchecked" 
													Theme="NETheme01">
												</dx:ASPxCheckBox>
											</td>
											<td width="50px">
												&nbsp;</td>
											<td >
												<asp:SqlDataSource ID="sqlmember" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													SelectCommand="Select membertype_id,membertype_name from membertype order by membertype_name">
												</asp:SqlDataSource>
											</td>
											<td >
												<dx:ASPxButton ID="btnSave0" runat="server" HorizontalAlign="Center" 
													OnClick="btnSave0_Click" Text="Update All Business Units
                                                     Not Already Set" Width="120px" 
													   Height="40px"  CssClass="fin_wrap_it" BackColor="Transparent" EnableTheming="False" Wrap="True">
													
												    <BackgroundImage ImageUrl=" " />
                                                    <Border BorderColor="#CCCCCC" />
													
												</dx:ASPxButton>
											</td>
										</tr>
										<tr>
											<td colspan="1" nowrap="nowrap">
												Benefit Package:</td>
											<td>
												<dx:ASPxComboBox ID="ddlbenefits" runat="server" 
													ClientInstanceName="ddlbenefits" Theme="NETheme01" Width="160px">
													<Items>
														<dx:ListEditItem Text="Standard benefit package" 
															Value="Standard benefit package" />
														<dx:ListEditItem Text="Senior management benefit package" 
															Value="Senior management benefit package" />
														<dx:ListEditItem Text="Executive benefit package" 
															Value="Executive benefit package" />
													</Items>
												</dx:ASPxComboBox>
											</td>
											<td width="50px">
												&nbsp;</td>
											<td nowrap="nowrap">
												Include in the Scheduler:</td>
											<td>
												<dx:ASPxCheckBox ID="chk_is_scheduled" runat="server" CheckState="Unchecked" 
													ClientInstanceName="chk_is_scheduled" Theme="NETheme01">
												</dx:ASPxCheckBox>
											</td>
										</tr>
										<tr>
											<td colspan="1" nowrap="nowrap">
												Default Severance Package (weeks):</td>
											<td>
												<dx:ASPxSpinEdit ID="spn_severance" runat="server" ClientInstanceName="spn_severance" MaxValue="10" Number="0" NumberType="Integer" Width="110px">
                                                </dx:ASPxSpinEdit>
                                            </td>
											<td width="50px">
												&nbsp;</td>
											<td nowrap="nowrap">
												Is a Team Leader:</td>
											<td>
												<dx:ASPxCheckBox ID="chk_is_teamlead" runat="server" CheckState="Unchecked" 
													ClientInstanceName="chk_is_teamlead" Theme="NETheme01">
												</dx:ASPxCheckBox>
											</td>
										</tr>
										<tr>
                                            <td colspan="1" nowrap="nowrap">Default Benefits Start (weeks):</td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="spn_benefits" runat="server" ClientInstanceName="spn_benefits" MaxValue="52" Number="0" NumberType="Integer" Width="110px">
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <td width="50px">&nbsp;</td>
                                            <td nowrap="nowrap">Considered to be Field Personnel</td>
                                            <td>
                                                <dx:ASPxCheckBox ID="chk_considered_field_staff" runat="server" CheckState="Unchecked" ClientInstanceName="chk_considered_field_staff" Theme="NETheme01">
                                                </dx:ASPxCheckBox>
                                            </td>
                                        </tr>
										<tr>
											<td colspan="1" nowrap="nowrap">
												Status:</td>
											<td>
												<dx:ASPxComboBox ID="ddlstatus" runat="server" Theme="NETheme01" 
													ValueType="System.Int32" Width="160px">
													<ClientSideEvents TextChanged="function(s, e) {
if (s.GetText()=='Not Active')
	{
	if (txtexisting.GetText()!='0')
		{
		alert('There must be no users with this membertype before you can disable this');
		s.SetText('Active');
		}
	}
}" />
													<Items>
														<dx:ListEditItem Text="Active" Value="1" />
														<dx:ListEditItem Text="Not Active" Value="0" />
													</Items>
												</dx:ASPxComboBox>
											</td>
											<td width="50px">
												&nbsp;</td>
											<td nowrap="nowrap">
												Can go on call?:</td>
											<td>
												<dx:ASPxCheckBox ID="chk_is_oncall" runat="server" CheckState="Unchecked" 
													ClientInstanceName="chk_is_teamlead" Theme="NETheme01">
												</dx:ASPxCheckBox>
											</td>
										</tr>
										<tr>
											<td colspan="1" nowrap="nowrap">
												Considered PM:</td>
											<td>
												<dx:ASPxCheckBox ID="chkConsideredPM" runat="server" CheckState="Unchecked" ClientInstanceName="chkConsideredPM" Theme="NETheme01">
												</dx:ASPxCheckBox>
											</td>
											<td width="50px">
												&nbsp;</td>
											<td nowrap="nowrap">
												Chargeout Only?</td>
											<td>
												<dx:ASPxCheckBox ID="chkChargeoutOnly" runat="server" CheckState="Unchecked" Theme="NETheme01"></dx:ASPxCheckBox> </td>
										</tr>
										<tr>
											<td colspan="1" nowrap="nowrap" valign="top">
												&nbsp;</td>
											<td colspan="4">
												&nbsp;</td>
										</tr>
										<tr>
											<td colspan="1" nowrap="nowrap" valign="top">
												Objective:</td>
											<td colspan="4">
												<dx:ASPxMemo ID="ASPxMemo1" runat="server" BackColor="#FFFFCC" Height="60px" 
													Width="100%">
												</dx:ASPxMemo>
											</td>
										</tr>
										<tr>
											<td colspan="1" nowrap="nowrap" valign="top">
												<dx:ASPxButton ID="btnSave" runat="server" ClientEnabled="False" 
													ClientInstanceName="btnSave" HorizontalAlign="Left" OnClick="btnSave_Click" 
													Text="Save" Theme="NETheme01" Width="75px">
													<Image Url="~/images/icon/icon[save].gif">
													</Image>
												</dx:ASPxButton>
											</td>
											<td colspan="4">
												<dx:ASPxLabel ID="lblerror" runat="server" style="color: #FF0000" Width="100%">
												</dx:ASPxLabel>
											</td>
										</tr>
									</table>
							
						
						<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
							Width="100%">
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
									<table cellpadding="3px" width="100%">
										<tr>
											<td nowrap="nowrap">
												<table style="width:100%;">
													<tr>
														<td width="0%">
															<table style="width:100%;">
																<tr>
																	<td nowrap="nowrap" width="300px">
																		<table style="width:100%;">
																			<tr>
																				<td>
																					<dx:ASPxComboBox ID="ASPxComboBox1" runat="server" 
																						DataSourceID="SqlDataSource2" TextField="Prov_Desc" ValueField="prov_abbv" 
																						Width="100px" ClientInstanceName="cb_prov" Theme="NETheme01">
																					</dx:ASPxComboBox>
																				</td>
																				<td width="100%">
																					<dx:ASPxButton ID="ASPxButton1" runat="server" HorizontalAlign="Left" Text="Print (for this state/province)" 
																						Width="150px" AutoPostBack="False" Theme="NETheme01">
																						<ClientSideEvents Click="function(s, e) {
	boing('mt_print_off.aspx?mtid=' + lblid.GetText()+ '&amp;prov=' + cb_prov.GetValue(),'mt',900,900);
}" />
																						<Image Url="~/images/icon/icon[print].gif">
																						</Image>
																					</dx:ASPxButton>
																					<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
																						ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																						ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																						SelectCommand="Select * from prov"></asp:SqlDataSource>
																				</td>
																			</tr>
																		</table>
																	</td>
																	<td>
																		&nbsp;</td>
																	<td width="100%">
																		&nbsp;</td>
																</tr>
																<tr>
																	<td colspan="3" nowrap="nowrap" width="300px">
																		<dx:ASPxListBox ID="ASPxListBox1" runat="server" Rows="10" Width="100%" 
																			Height="300px" Theme="NETheme01">
																			<Columns>
																				<dx:ListBoxColumn Caption="Employees of this Membertype" FieldName="_name" />
																			</Columns>
																		</dx:ASPxListBox>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</dx:PanelContent>
							</PanelCollection>
						</dx:ASPxCallbackPanel>
						<br />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Core Responsibilities">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						
									<asp:SqlDataSource ID="sql_jd" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
 membertype_responsibilities.id,
core_responsibilities.core_responsibility core_responsibility,
core_responsibilities.`status`,
core_responsibilities.description description,
mt_cr_priority,
core_responsibilities.id cid   
FROM
core_responsibilities
INNER JOIN membertype_responsibilities ON core_responsibilities.id = membertype_responsibilities.core_responsibility_id and core_responsibilities.status = 'Active' where membertype_responsibilities.membertype_id=?id order by  mt_cr_priority,core_responsibilities.core_responsibility
">
										<SelectParameters>
											<asp:ControlParameter ControlID="hdnid" Name="id" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
									<dx:ASPxGridView ID="gv_jr" runat="server" AutoGenerateColumns="False" 
										ClientInstanceName="gv_jr" DataSourceID="sql_jd" Enabled="False" 
										KeyFieldName="id" OnCustomCallback="gv_jr_CustomCallback" 
										OnHtmlDataCellPrepared="gv_jr_HtmlDataCellPrepared" 
										OnRowDeleting="gv_jr_RowDeleting" Width="100%">
										<ClientSideEvents EndCallback="function(s, e) {
	gv_q.Refresh();
}" />
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" >
<Image Width="16px" Url="~/images/icon/icon[delete].gif"></Image>
                                                                    </DeleteButton>
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" >
<Image Width="16px" Url="~/images/icon/icon[edit].gif"></Image>
                                                                    </EditButton>
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" >
<Image Width="16px" Url="~/images/icon/icon[add].gif"></Image>
                                                                    </NewButton>
																</SettingsCommandButton>
										<Columns>
											<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
												ShowInCustomizationForm="True" VisibleIndex="0" Width="30px" ShowDeleteButton="true" ShowClearFilterButton="true">
												
												
											</dx:GridViewCommandColumn>
											<dx:GridViewDataTextColumn Caption="Responsibility" 
												FieldName="core_responsibility" ShowInCustomizationForm="True" VisibleIndex="1" 
												Width="100%">
												<PropertiesTextEdit DisplayFormatString="{0}">
												</PropertiesTextEdit>
												<Settings AutoFilterCondition="Contains" />
												<DataItemTemplate>
													<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
														NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/hr/member/cr_detail.aspx?id={0}', 'whatever1',900,1000)&quot;,Eval(&quot;cid&quot;)) %>" 
														Text='<%# Eval("core_responsibility") %>' />
												</DataItemTemplate>
												<CellStyle Wrap="True">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
												ShowInCustomizationForm="True" VisibleIndex="4" Width="60px">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="id" ShowInCustomizationForm="True" 
												Visible="False" VisibleIndex="5">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="description" 
												ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
												<Settings AutoFilterCondition="Contains" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataComboBoxColumn Caption="Priority" FieldName="mt_cr_priority" 
												ShowInCustomizationForm="True" SortIndex="0" SortOrder="Ascending" 
												VisibleIndex="3" Width="75px">
												<Settings AutoFilterCondition="BeginsWith" />
												<DataItemTemplate>
													<dx:ASPxSpinEdit ID="ASPxSpinEdit1" runat="server" 
														ClientInstanceName="ASPxSpinEdit1" Height="21px" MaxValue="10" 
														NumberType="Integer" OnInit="ASPxSpinEdit1_Init" 
														Value='<%# Eval("mt_cr_priority") %>' Width="50px" />
												</DataItemTemplate>
											</dx:GridViewDataComboBoxColumn>
										</Columns>
										<SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
										<SettingsPager Mode="ShowAllRecords" Visible="False">
										</SettingsPager>
										<Settings ShowTitlePanel="True" ShowFilterBar="Visible" ShowFilterRow="True" 
											ShowFilterRowMenu="True" />
										<Templates>
											<TitlePanel>
												<table style="width: 100%;">
													<tr>
														<td width="100%">
															<dx:ASPxComboBox ID="ddlcr" runat="server" ClientInstanceName="ddlcr" 
																DataSourceID="SqlDataSource1" TextField="core_responsibility" ValueField="id" 
																ValueType="System.Int32" Width="100%" IncrementalFilteringMode="Contains">
															</dx:ASPxComboBox>
														</td>
														<td>
															<dx:ASPxSpinEdit ID="spnpriority" runat="server" 
																ClientInstanceName="spnpriority" Height="21px" MaxValue="10" Number="9" 
																NumberType="Integer" Width="50px" ShowOutOfRangeWarning="False" >
																<ValidationSettings Display="None" ErrorDisplayMode="None">
																</ValidationSettings>
															</dx:ASPxSpinEdit>
														</td>
														<td>
															<dx:ASPxButton ID="btnadd" runat="server" AutoPostBack="False" 
																ClientInstanceName="btnadd" Text="Add">
																<ClientSideEvents Click="function(s, e) {
	gv_jr.PerformCallback();
}" />
															</dx:ASPxButton>
															<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																SelectCommand="SELECT id,core_responsibility core_responsibility FROM core_responsibilities WHERE (status = 'Active') order by core_responsibility">
															</asp:SqlDataSource>
														</td>
													</tr>
												</table>
											</TitlePanel>
										</Templates>
									</dx:ASPxGridView>
									<br />
									<br />
									<br />
								
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Skills" ClientVisible="False">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						
									<asp:SqlDataSource ID="sql_q" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT distinct
master_skills.`name` `name`,
master_skills.description,
if(master_skills.can_be_trained,'Yes','No') can_be_trained,
max(cr_skills_priority.priority_name) priority_name,
master_skills.skilltype,
master_skills.status,
cr.status cr_status 
FROM
cr_skills_link
INNER JOIN master_skills ON cr_skills_link.skills_id = master_skills.id
INNER JOIN membertype_responsibilities ON membertype_responsibilities.core_responsibility_id = cr_skills_link.cr_id
INNER JOIN cr_skills_priority ON cr_skills_link.cr_skills_priority_id = cr_skills_priority.id
INNER JOIN core_responsibilities cr on cr_skills_link.cr_id = cr.id 
WHERE
membertype_responsibilities.membertype_id = ?id and master_skills.status='Active' and cr.status='Active' 
group by master_skills.`name` order by  skilltype,cr_skills_priority.id,  can_be_trained,master_skills.`name`
">
										<SelectParameters>
											<asp:ControlParameter ControlID="hdnid" Name="id" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
									<dx:ASPxGridView ID="gv_q" runat="server" AutoGenerateColumns="False" 
										ClientInstanceName="gv_q" DataSourceID="sql_q" Enabled="False" 
										KeyFieldName="id" Width="100%">
										
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" >
<Image Width="16px" Url="~/images/icon/icon[delete].gif"></Image>
                                                                    </DeleteButton>
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" >
<Image Width="16px" Url="~/images/icon/icon[edit].gif"></Image>
                                                                    </EditButton>
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" >
<Image Width="16px" Url="~/images/icon/icon[add].gif"></Image>
                                                                    </NewButton>
																</SettingsCommandButton>
										<Columns>
											<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
												ShowInCustomizationForm="True" VisibleIndex="0" Width="30px" Visible="False" ShowDeleteButton="true" ShowClearFilterButton="true">
												
												
											</dx:GridViewCommandColumn>
											<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" 
												ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
												<EditFormSettings Visible="False" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Skill" FieldName="name" 
												ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Priority" FieldName="priority_name" 
												ShowInCustomizationForm="True" VisibleIndex="3" Width="50px">
												<CellStyle Wrap="False">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Skill Type" FieldName="skilltype" 
												ShowInCustomizationForm="True" VisibleIndex="4" Width="75px">
												<CellStyle Wrap="False">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Can Be Trained" FieldName="can_be_trained" 
												ShowInCustomizationForm="True" VisibleIndex="5" Width="50px">
												<CellStyle Wrap="False">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
												ShowInCustomizationForm="True" VisibleIndex="6" Width="60px">
											</dx:GridViewDataTextColumn>
										</Columns>
										<SettingsBehavior ConfirmDelete="True" />
										<SettingsPager Mode="ShowAllRecords" Visible="False">
										</SettingsPager>
										<Settings ShowTitlePanel="True" ShowFilterBar="Visible" ShowFilterRow="True" 
											ShowFilterRowMenu="True" />
									</dx:ASPxGridView>
									<br />
									<br />
									<br />
						
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Review Questions">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
membertype_responsibilities.id,
core_responsibilities.core_responsibility AS core_responsibility,
core_responsibilities.`status`,
core_responsibilities.description AS description,
membertype_responsibilities.mt_cr_priority,
core_responsibilities.id AS cid,
cr_review.cr_review_question
FROM
core_responsibilities
INNER JOIN membertype_responsibilities ON core_responsibilities.id = membertype_responsibilities.core_responsibility_id
INNER JOIN cr_review ON membertype_responsibilities.core_responsibility_id = cr_review.cr_review_cr_id AND cr_review.cr_review_status = 'Active'
WHERE
          membertype_responsibilities.membertype_id = ?id and core_responsibilities.`status` = 'Active'
ORDER BY
          mt_cr_priority,
          core_responsibilities.core_responsibility">
							<SelectParameters>
								<asp:ControlParameter ControlID="hdnid" Name="id" PropertyName="Value" />
							</SelectParameters>
						</asp:SqlDataSource>
						<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" 
							 Text="View Current Review Items" Width="214px">
							
							<ClientSideEvents Click="function(s, e) {
	print_mreview(lblid.GetText());
}" />
							
						</dx:ASPxButton>
						<dx:ASPxGridView ID="gv_review" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_review" DataSourceID="SqlDataSource3" KeyFieldName="id" 
							Width="100%">
							<Columns>
								<dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" 
									VisibleIndex="0" ShowClearFilterButton="true">
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Core Responsibility" 
									FieldName="core_responsibility" ShowInCustomizationForm="True" VisibleIndex="2" 
									Width="50%">
									<CellStyle Wrap="True">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
									ShowInCustomizationForm="True" VisibleIndex="4" Width="75px">
									<CellStyle Wrap="True">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Responsibility Priority" 
									FieldName="mt_cr_priority" ShowInCustomizationForm="True" VisibleIndex="5" 
									Width="70px">
									<HeaderStyle Wrap="True" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="cid" ReadOnly="True" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="6">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Review Item" FieldName="cr_review_question" 
									ShowInCustomizationForm="True" VisibleIndex="3" Width="50%">
									<CellStyle Wrap="True">
									</CellStyle>
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior ColumnResizeMode="Control" />
							<SettingsPager PageSize="100">
							</SettingsPager>
							<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
								ShowHeaderFilterButton="True" />
						</dx:ASPxGridView>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Comp Plans">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
bonus_membertype_link.id,
bonus_membertype_link.bonus_type_id,
bonus_type.bonus_type,
bonus_type.tooltip
FROM
bonus_type
INNER JOIN bonus_membertype_link ON bonus_type.id = bonus_membertype_link.bonus_type_id 
where bonus_membertype_link.membertype_id = ?mid">
							<SelectParameters>
								<asp:ControlParameter ControlID="hdnid" Name="mid" PropertyName="Value" />
							</SelectParameters>
						</asp:SqlDataSource>
						<dx:ASPxGridView ID="gv_comps" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_comps" OnCustomCallback="gv_comps_CustomCallback" 
							OnRowDeleting="gv_comps_RowDeleting" Width="100%" DataSourceID="SqlDataSource4" 
							KeyFieldName="id">
							
								<SettingsCommandButton>
									<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" >
<Image Width="16px" Url="~/images/icon/icon[delete].gif"></Image>
                                    </DeleteButton>
									<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" >
<Image Width="16px" Url="~/images/icon/icon[edit].gif"></Image>
                                    </EditButton>
									<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" >
<Image Width="16px" Url="~/images/icon/icon[add].gif"></Image>
                                    </NewButton>
								</SettingsCommandButton>
							<Columns>
								<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
									ShowInCustomizationForm="True" VisibleIndex="0" Width="30px">
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="Bonus Type" FieldName="bonus_type" 
									ShowInCustomizationForm="True" VisibleIndex="1">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Description" FieldName="tooltip" 
									ShowInCustomizationForm="True" VisibleIndex="2">
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior ConfirmDelete="True" />
							<SettingsPager Mode="ShowAllRecords" Visible="False">
							</SettingsPager>
							<Settings ShowTitlePanel="True" />
							<Templates>
								<TitlePanel>
									<table style="width: 100%;">
										<tr>
											<td width="100%">
												<asp:SqlDataSource ID="sql_bonus" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													SelectCommand="Select * from bonus_type where status = 1">
												</asp:SqlDataSource>
												<dx:ASPxComboBox ID="ddl_bonus" runat="server" ClientInstanceName="ddl_bonus" 
													DataSourceID="sql_bonus" TextField="bonus_type" ValueField="id" 
													ValueType="System.Int32" Width="100%">
												</dx:ASPxComboBox>
											</td>
											<td>
												&nbsp;</td>
											<td>
												<dx:ASPxButton ID="btnadd_bonus" runat="server" AutoPostBack="False" Text="Add" 
													Width="70px">
													<ClientSideEvents Click="function(s, e) {
	gv_comps.PerformCallback(ddl_bonus.GetValue());
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
		</TabPages>
	</dx:ASPxPageControl>
				<dx:ASPxCallbackPanel runat="server" 
		ClientInstanceName="cb_mt_cr_priority" Width="200px" ID="cb_mt_cr_priority" 
		OnCallback="cb_mt_cr_priority_Callback"><PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
						</dx:PanelContent>
</PanelCollection>
</dx:ASPxCallbackPanel>

			<asp:HiddenField ID="hdnid" runat="server" />
	<br />

		</ContentTemplate>
	</asp:UpdatePanel>




</asp:Content>

