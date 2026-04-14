<%@ Page Title="" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_hr_member_cr_detail" Codebehind="cr_detail.aspx.cs" %>	

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
	
}" />
		<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
		ClientInstanceName="ASPxPageControl1" 
		Width="100%" AutoPostBack="True" 
		OnActiveTabChanged="ASPxPageControl1_ActiveTabChanged" EnableCallBacks="True">
		<TabPages>
			<dx:TabPage Name="Details" Text="Details">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" BackColor="#CCFFCC" 
							ClientInstanceName="ASPxRoundPanel1" Font-Bold="True" Font-Names="Arial" 
							Font-Size="16pt" HeaderText="Core Responsibility Detail" Width="100%">
							<HeaderStyle BackColor="#CCFFCC" />
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
									<table style="width: 100%;">
										<tr>
											<td class="style1">
												<strong>ID:</strong></td>
											<td width="100%">
												<dx:ASPxLabel ID="lbid" runat="server" ClientInstanceName="lbid" 
													Font-Names="Arial">
												</dx:ASPxLabel>
											</td>
											<td>
												&nbsp;</td>
										</tr>
										<tr>
											<td class="style1" nowrap="nowrap">
												<strong>Group:</strong></td>
											<td>
												<dx:ASPxComboBox ID="ddlgroup" runat="server" ClientInstanceName="ddlgroup" 
													DataSourceID="SqlDataSource2" Font-Names="Arial" TextField="name" 
													ValueField="id" ValueType="System.Int32" CssClass="style1" Font-Size="9pt">
													<Items>
														<dx:ListEditItem Text="Active" Value="0" />
														<dx:ListEditItem Text="Inactive" Value="0" />
													</Items>
												</dx:ASPxComboBox>
											</td>
											<td>
												&nbsp;</td>
										</tr>
										<tr>
											<td class="style1" nowrap="nowrap">
												<strong>Core Responsibility:</strong></td>
											<td>
												<dx:ASPxTextBox ID="txtcore" runat="server" BackColor="#FFFFCC" 
													Font-Names="Arial" Height="30px" Width="100%" Font-Size="9pt">
												</dx:ASPxTextBox>
											</td>
											<td>
												&nbsp;</td>
										</tr>
										<tr>
											<td class="style1">
												<strong>Description:</strong></td>
											<td>
												<dx:ASPxMemo ID="mem" runat="server" ClientInstanceName="mem" 
													Font-Names="Arial" Height="60px" Width="100%" Font-Size="9pt">
												</dx:ASPxMemo>
											</td>
											<td>
												&nbsp;</td>
										</tr>
										<tr>
											<td class="style1" nowrap="nowrap">
												<strong>Status:</strong></td>
											<td>
												<dx:ASPxComboBox ID="ddlstatus" runat="server" Font-Names="Arial" 
													Font-Size="9pt">
													<Items>
														<dx:ListEditItem Text="Active" Value="Active" />
														<dx:ListEditItem Text="Inactive" Value="Inactive" />
													</Items>
												</dx:ASPxComboBox>
											</td>
											<td>
												&nbsp;</td>
										</tr>
										<tr>
											<td class="style1" nowrap="nowrap">
												&nbsp;</td>
											<td>
												<dx:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" 
													Width="100%">
													<TabPages>
														<dx:TabPage Text="Daily">
															<ContentCollection>
																<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																	<dx:ASPxMemo ID="txtdaily" runat="server" Font-Names="Arial" Font-Size="9pt" 
																		Height="200px" Width="100%">
																	</dx:ASPxMemo>
																</dx:ContentControl>
															</ContentCollection>
														</dx:TabPage>
														<dx:TabPage Text="Weekly">
															<ContentCollection>
																<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																	<dx:ASPxMemo ID="txtweekly" runat="server" Font-Names="Arial" Font-Size="9pt" 
																		Height="100px" Width="100%">
																	</dx:ASPxMemo>
																</dx:ContentControl>
															</ContentCollection>
														</dx:TabPage>
														<dx:TabPage Text="Monthly">
															<ContentCollection>
																<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																	<dx:ASPxMemo ID="txtmonthly" runat="server" Font-Names="Arial" Height="200px" 
																		Width="100%">
																	</dx:ASPxMemo>
																</dx:ContentControl>
															</ContentCollection>
														</dx:TabPage>
														<dx:TabPage Text="Quarterly">
															<ContentCollection>
																<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																	<dx:ASPxMemo ID="txtquarterly" runat="server" Font-Names="Arial" Height="200px" 
																		Width="100%">
																	</dx:ASPxMemo>
																</dx:ContentControl>
															</ContentCollection>
														</dx:TabPage>
														<dx:TabPage Text="Annually">
															<ContentCollection>
																<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																	<dx:ASPxMemo ID="txtannually" runat="server" Font-Names="Arial" Height="200px" 
																		Width="100%">
																	</dx:ASPxMemo>
																</dx:ContentControl>
															</ContentCollection>
														</dx:TabPage>
														<dx:TabPage Text="As Needed">
															<ContentCollection>
																<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																	<dx:ASPxMemo ID="txtas_required" runat="server" Font-Names="Arial" 
																		Height="200px" Width="100%">
																	</dx:ASPxMemo>
																</dx:ContentControl>
															</ContentCollection>
														</dx:TabPage>
													</TabPages>
												</dx:ASPxPageControl>
											</td>
											<td>
												&nbsp;</td>
										</tr>
										<tr>
											<td class="style1" nowrap="nowrap">
												&nbsp;</td>
											<td>
												<dx:ASPxCallbackPanel ID="cbmt" runat="server" ClientInstanceName="cbmt" 
													OnCallback="cbmt_Callback" Width="100%">
													<PanelCollection>
														<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
															<table class="style2">
																<tr>
																	<td valign="top">
																		&nbsp;</td>
																	<td align="center" valign="top">
																		&nbsp;</td>
																	<td valign="top">
																		&nbsp;</td>
																</tr>
																<tr>
																	<td align="right" valign="top">
																		<dx:ASPxTreeList ID="tlmt" runat="server" AutoGenerateColumns="False" 
																			DataSourceID="SqlDataSource11" Width="300px" ClientInstanceName="tlmt">
																			<Columns>
																				<dx:TreeListTextColumn Caption="Membertypes" FieldName="mt" 
																					ShowInCustomizationForm="True" VisibleIndex="0">
																				</dx:TreeListTextColumn>
																				<dx:TreeListTextColumn FieldName="id" ShowInCustomizationForm="True" 
																					Visible="False" VisibleIndex="1">
																				</dx:TreeListTextColumn>
																			</Columns>
																			<Settings ShowTreeLines="False" />
																			<SettingsSelection Enabled="True" />
																			<Border BorderColor="Gray" BorderStyle="Solid" />
																		</dx:ASPxTreeList>
																	</td>
																	<td align="center" valign="top">
																		<table style="width:100%;">
																			<tr>
																				<td>
																					<dx:ASPxButton ID="btnadd" runat="server" AutoPostBack="False" 
																						Font-Names="Arial" Font-Size="12pt" Text="&gt;&gt;" Width="30px">
																						<ClientSideEvents Click="function(s, e) {
	cbmt.PerformCallback('a');
}" />
																					</dx:ASPxButton>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					<dx:ASPxButton ID="btnremove" runat="server" AutoPostBack="False" 
																						Font-Names="Arial" Font-Size="12pt" Text="&lt;&lt;" Width="30px">
																						<ClientSideEvents Click="function(s, e) {
	cbmt.PerformCallback('r');
}" />
																					</dx:ASPxButton>
																				</td>
																			</tr>
																			<tr>
																				<td>
																					&nbsp;</td>
																			</tr>
																		</table>
																	</td>
																	<td align="left" valign="top">
																		<dx:ASPxTreeList ID="tlmt2" runat="server" AutoGenerateColumns="False" 
																			DataSourceID="SqlDataSource12" Width="300px" ClientInstanceName="tlmt2">
																			<Columns>
																				<dx:TreeListTextColumn Caption="Membertypes Who Do This" FieldName="mt" 
																					ShowInCustomizationForm="True" VisibleIndex="0">
																				</dx:TreeListTextColumn>
																				<dx:TreeListTextColumn FieldName="id" ShowInCustomizationForm="True" 
																					Visible="False" VisibleIndex="1">
																				</dx:TreeListTextColumn>
																			</Columns>
																			<Settings ShowTreeLines="False" />
																			<SettingsSelection Enabled="True" />
																			<Border BorderColor="Gray" BorderStyle="Solid" />
																		</dx:ASPxTreeList>
																	</td>
																</tr>
																<tr>
																	<td valign="top">
																		<asp:SqlDataSource ID="SqlDataSource11" runat="server" 
																			ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
membertype.membertype_id id,
membertype.membertype_name mt
FROM
membertype
WHERE
membertype.active = 1
ORDER BY
membertype.membertype_name ASC"></asp:SqlDataSource>
																	</td>
																	<td align="center" valign="top">
																		&nbsp;</td>
																	<td valign="top">
																		<asp:SqlDataSource ID="SqlDataSource12" runat="server" 
																			ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
membertype_responsibilities.id id,
membertype.membertype_name mt
FROM
membertype_responsibilities
INNER JOIN membertype ON membertype.membertype_id = membertype_responsibilities.membertype_id
WHERE
membertype.active = 1 AND
membertype_responsibilities.core_responsibility_id = if(?id=0,999999,?id)">
																			<SelectParameters>
																				<asp:ControlParameter ControlID="lbid" Name="id" PropertyName="Text" />
																			</SelectParameters>
																		</asp:SqlDataSource>
																	</td>
																</tr>
															</table>
														</dx:PanelContent>
													</PanelCollection>
												</dx:ASPxCallbackPanel>
											</td>
											<td>
												&nbsp;</td>
										</tr>
										<tr>
											<td class="style1" nowrap="nowrap">
												<dx:ASPxButton ID="btnsave" runat="server" AutoPostBack="False" 
													ClientInstanceName="btnsave" CssClass="input" Text="Next -&gt;" Width="100px">
													<clientsideevents click="function(s, e) {
	cb.PerformCallback();
}" />
												</dx:ASPxButton>
											</td>
											<td>
												&nbsp;</td>
											<td>
												&nbsp;</td>
										</tr>
									</table>
								</dx:PanelContent>
							</PanelCollection>
						</dx:ASPxRoundPanel>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage ClientEnabled="False" Name="Skills" Text="Skills" 
				ClientVisible="False">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" BackColor="#FFFF99" 
							Font-Bold="True" Font-Names="Arial" Font-Size="16pt" HeaderText="Skills Needed" 
							Width="100%">
							<HeaderStyle BackColor="#FFFF99" />
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
									<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										
										
										SelectCommand="SELECT cr_skills_link.id, name name, description description, if(can_be_trained=1,'Yes','No') can_be_trained,priority_name priority,master_skills.status FROM cr_skills_link,master_skills,cr_skills_priority WHERE cr_skills_link.skills_id = master_skills.id and cr_id =?ticketid and cr_skills_link.cr_skills_priority_id = cr_skills_priority.id order by master_skills.status,priority_name">
										<SelectParameters>
											<asp:ControlParameter ControlID="hdnids" Name="ticketid" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
									<asp:HiddenField ID="hdnids" runat="server" />
									<dx:ASPxGridView ID="gv_skills" runat="server" AutoGenerateColumns="False" 
										ClientInstanceName="gv_skills" DataSourceID="SqlDataSource4" KeyFieldName="id" 
										OnCustomCallback="gv_skills_CustomCallback" 
										OnHtmlDataCellPrepared="gv_skills_HtmlDataCellPrepared" 
										OnRowDeleting="gv_skills_RowDeleting" Width="100%" Font-Names="Arial">

<SettingsBehavior ConfirmDelete="True"></SettingsBehavior>

<Settings ShowTitlePanel="True"></Settings>

										<ClientSideEvents EndCallback="function(s, e) {
	ddl_skills.SetText('');
}" />
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
										<Columns>
											<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
												ShowInCustomizationForm="True" VisibleIndex="0" Width="30px">
												
											</dx:GridViewCommandColumn>
											<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
												ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Can Be Trained" FieldName="can_be_trained" 
												ShowInCustomizationForm="True" VisibleIndex="3" Width="50px">
												<PropertiesTextEdit EnableFocusedStyle="False">
												</PropertiesTextEdit>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Skill" FieldName="name" 
												ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
												ShowInCustomizationForm="True" Visible="False" VisibleIndex="6">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataComboBoxColumn Caption="Priority" FieldName="priority" 
												ShowInCustomizationForm="True" VisibleIndex="4">
												<PropertiesComboBox DataSourceID="SqlDataSource6" TextField="priority_name" 
													ValueField="id" ValueType="System.Int32">
												</PropertiesComboBox>
												<DataItemTemplate>
													<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" 
														DataSourceID="SqlDataSource6" oninit="ASPxComboBox2_Init" 
														TextField="priority_name" Value='<%# Eval("priority") %>' ValueField="id" 
														ValueType="System.Int32">
													</dx:ASPxComboBox>
												</DataItemTemplate>
											</dx:GridViewDataComboBoxColumn>
											<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
												ShowInCustomizationForm="True" VisibleIndex="5" Width="60px">
											</dx:GridViewDataTextColumn>
										</Columns>
										<SettingsBehavior ConfirmDelete="True" />

										<SettingsPager Mode="ShowAllRecords">
										</SettingsPager>
										<Settings ShowTitlePanel="True" />

										<Templates>
											<TitlePanel>
												<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													SelectCommand="select id,master_skills.name name from master_skills where status = 'Active' order by master_skills.name">
												</asp:SqlDataSource>
												<table style="width: 100%;">
													<tr>
														<td nowrap="nowrap">
															<strong>Add Skill:</strong></td>
														<td width="100%">
															<dx:ASPxComboBox ID="ddl_skills" runat="server" ClientInstanceName="ddl_skills" 
																DataSourceID="SqlDataSource5" IncrementalFilteringMode="Contains" 
																TextField="name" ValueField="id" ValueType="System.Int32" Width="100%">
																<ClientSideEvents SelectedIndexChanged="function(s, e) {
	ddlskillpriority.SetValue(1);
}" />
															</dx:ASPxComboBox>
														</td>
														<td>
															<dx:ASPxComboBox ID="ddlskillpriority" runat="server" 
																ClientInstanceName="ddlskillpriority" DataSourceID="SqlDataSource6" 
																TextField="priority_name" ValueField="id" ValueType="System.Int32" 
																Width="150px">
															</dx:ASPxComboBox>
														</td>
														<td>
															<dx:ASPxButton ID="btnaddskill" runat="server" AutoPostBack="False" Text="Add">
																<ClientSideEvents Click="function(s, e) {
if (ddl_skills.GetText()!='')
{	
gv_skills.PerformCallback(ddl_skills.GetValue() + '|' + ddlskillpriority.GetValue() + '|a');
}
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
						</dx:ASPxRoundPanel>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage ClientEnabled="False" Name="Review Items" Text="Review Items">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table style="width:100%;">
							<tr>
								<td colspan="3">
									<dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
										ClientInstanceName="ASPxRoundPanel3" Font-Bold="True" Font-Names="Arial" 
										Font-Size="16pt" HeaderText="Core Responsibility Review Items" Width="100%" BackColor="#FF9966">
										<HeaderStyle BackColor="#FF6600" />
										<PanelCollection>
											<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
												<dx:ASPxGridView ID="gv_cr_review" runat="server" AutoGenerateColumns="False" 
													ClientInstanceName="gv_cr_review" DataSourceID="SqlDataSource7" 
													KeyFieldName="cr_review_id" OnCustomCallback="gv_cr_review_CustomCallback" 
													OnRowDeleting="gv_cr_review_RowDeleting" Width="100%">
													<ClientSideEvents EndCallback="function(s, e) {
	tb_newreview.SetText('');
}" />
<ClientSideEvents EndCallback="function(s, e) {
	tb_newreview.SetText(&#39;&#39;);
}"></ClientSideEvents>
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
													<Columns>
														<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
															ShowInCustomizationForm="True" VisibleIndex="0">
															
														</dx:GridViewCommandColumn>
														<dx:GridViewDataTextColumn FieldName="cr_review_id" ReadOnly="True" 
															ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
															<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Item" FieldName="cr_review_question" 
															ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
															<DataItemTemplate>
																<dx:ASPxMemo ID="ASPxTextBox1" runat="server" Height="25px" 
																	oninit="ASPxTextBox1_Init" Text='<%# Eval("cr_review_question") %>' 
																	Width="100%">
																</dx:ASPxMemo>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Status" FieldName="cr_review_status" 
															ShowInCustomizationForm="True" VisibleIndex="3" Width="125px">
															<DataItemTemplate>
																<dx:ASPxComboBox ID="ASPxComboBox3" runat="server" oninit="ASPxComboBox3_Init" 
																	Value='<%# Eval("cr_review_status") %>'>
																	<Items>
																		<dx:ListEditItem Text="Active" Value="Active" />
																		<dx:ListEditItem Text="InActive" Value="InActive" />
																	</Items>
																</dx:ASPxComboBox>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsBehavior ConfirmDelete="True" />

<SettingsBehavior ConfirmDelete="True"></SettingsBehavior>

													<SettingsPager Mode="ShowAllRecords">
													</SettingsPager>
													<Settings ShowTitlePanel="True" />

<Settings ShowTitlePanel="True"></Settings>

													<Templates>
														<TitlePanel>
															<table style="width:100%;">
																<tr>
																	<td width="100%">
																		<dx:ASPxMemo ID="tb_newreview" runat="server" ClientInstanceName="tb_newreview" 
																			Height="25px" Width="100%">
																		</dx:ASPxMemo>
																	</td>
																	<td>
																		&nbsp;</td>
																	<td>
																		<dx:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="False" Text="Add">
																			<ClientSideEvents Click="function(s, e) {
	gv_cr_review.PerformCallback();

}" />
																		</dx:ASPxButton>
																	</td>
																</tr>
															</table>
														</TitlePanel>
													</Templates>
												</dx:ASPxGridView>
												<asp:SqlDataSource ID="SqlDataSource7" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													SelectCommand="Select cr_review_id, cr_review_question cr_review_question, cr_review_status from cr_review where cr_review_cr_id = ?id">
													<SelectParameters>
														<asp:ControlParameter ControlID="hdnids0" Name="id" PropertyName="Value" />
													</SelectParameters>
												</asp:SqlDataSource>
												<asp:HiddenField ID="hdnids0" runat="server" />
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxRoundPanel>
								</td>
							</tr>
							<tr>
								<td colspan="3">
									<dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" BackColor="#FFCC99" 
										ClientInstanceName="ASPxRoundPanel4" Font-Bold="True" Font-Names="Arial" 
										Font-Size="16pt" HeaderText="Active Skills Assessment Items (for reference)" Width="100%" 
										ClientVisible="False">
										<HeaderStyle BackColor="#FFCC99" />
										<PanelCollection>
											<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
												<dx:ASPxGridView ID="gv_skills_q" runat="server" AutoGenerateColumns="False" 
													ClientInstanceName="gv_skills_q" DataSourceID="SqlDataSource8" 
													KeyFieldName="master_skill_review_questions_id" 
													OnCustomCallback="gv_skills_q_CustomCallback" Width="100%">
													<Columns>
														<dx:GridViewDataTextColumn FieldName="master_skill_review_questions_id" 
															ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
															<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Type" FieldName="skilltype" 
															ShowInCustomizationForm="True" VisibleIndex="1" Width="60px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Skill" FieldName="skillsname" 
															ShowInCustomizationForm="True" VisibleIndex="2" Width="200px">
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Item" FieldName="question" 
															ShowInCustomizationForm="True" VisibleIndex="3" Width="100%">
															<DataItemTemplate>
																<dx:ASPxMemo ID="tb_skills_question" runat="server" 
																	ClientInstanceName="tb_skills_question" Height="30px" 
																	oninit="tb_skills_question_Init" Text='<%# Eval("question") %>' Width="100%">
																</dx:ASPxMemo>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Question Status" 
															FieldName="master_skill_review_questions_status" ShowInCustomizationForm="True" 
															VisibleIndex="4" Width="75px">
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsPager Mode="ShowAllRecords">
													</SettingsPager>
													<Styles>
														<Cell VerticalAlign="Middle">
														</Cell>
													</Styles>
												</dx:ASPxGridView>
												<asp:HiddenField ID="hdnids1" runat="server" />
												<asp:SqlDataSource ID="SqlDataSource8" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT DISTINCT
master_skill_review_questions.master_skill_review_questions_id,
master_skills.skilltype,
master_skills.`name` AS skillsname,
master_skill_review_questions.master_skill_review_questions_question AS question,
master_skill_review_questions.master_skill_review_questions_status
FROM
cr_skills_link
INNER JOIN master_skill_review_questions ON master_skill_review_questions.master_skill_review_questions_skill_id = cr_skills_link.skills_id
INNER JOIN master_skills ON master_skill_review_questions.master_skill_review_questions_skill_id = master_skills.id
WHERE
cr_skills_link.cr_id = ?id and master_skills.status = 'Active'">
													<SelectParameters>
														<asp:ControlParameter ControlID="hdnids1" Name="id" PropertyName="Value" />
													</SelectParameters>
												</asp:SqlDataSource>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxRoundPanel>
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;</td>
								<td>
									&nbsp;</td>
								<td>
									&nbsp;</td>
							</tr>
						</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Related Certifications" ClientEnabled="False">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<asp:SqlDataSource ID="SqlDataSource9" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
certificates.certificate_name,
certificates.notes,
certificates.is_internal,
certificates.expires,
cr_certificates.id
FROM
cr_certificates
INNER JOIN certificates ON cr_certificates.certificates_id = certificates.id
WHERE
cr_certificates.cr_id = ?ticketid and certificates.status= 'Active'">
							<SelectParameters>
								<asp:ControlParameter ControlID="hdnids2" Name="ticketid" 
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
								<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
									ShowInCustomizationForm="True" VisibleIndex="0" Width="30px" ShowClearFilterButton="true" ShowDeleteButton="true">
									
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="1" Width="40px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Certification" FieldName="certificate_name" 
									ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
									<Settings AutoFilterCondition="Contains" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
									ShowInCustomizationForm="True" VisibleIndex="3" Width="200px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataCheckColumn Caption="Is Internal" FieldName="is_internal" 
									ShowInCustomizationForm="True" VisibleIndex="4" Width="75px">
									<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" 
										ValueUnchecked="0">
									</PropertiesCheckEdit>
								</dx:GridViewDataCheckColumn>
								<dx:GridViewDataTextColumn Caption="Expires (days)" FieldName="expires" 
									ShowInCustomizationForm="True" VisibleIndex="5" Width="100px">
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
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
certificates where certificates.status='Active' order by certificates.certificate_name"></asp:SqlDataSource>
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
						<dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" BackColor="#CCF5B8" 
							Font-Bold="True" Font-Names="Arial" Font-Size="16pt" 
							HeaderText="Skills Related Certifications (for reference)" Width="100%" ClientVisible="False">
							<HeaderStyle BackColor="#9CDF7B">
							<BorderBottom BorderStyle="None" />
							</HeaderStyle>
							
							
							
							
							
							
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
									<asp:SqlDataSource ID="SqlDataSource13" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT DISTINCT
certificates.certificate_name,
certificates.notes,
certificates.expires,
certificates.is_internal,
master_skills.`name` AS skill_name
FROM
certificates
INNER JOIN certificate_skill_link ON certificate_skill_link.certificate_id = certificates.id
INNER JOIN cr_skills_link ON cr_skills_link.skills_id = certificate_skill_link.skill_id
INNER JOIN master_skills ON cr_skills_link.skills_id = master_skills.id AND master_skills.`status` = 'Active' where cr_skills_link.cr_id = ?ticketid
">
										<SelectParameters>
											<asp:ControlParameter ControlID="hdnids3" Name="ticketid" 
												PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
									<asp:HiddenField ID="hdnids3" runat="server" />
									<dx:ASPxButton ID="ASPxButton5" runat="server" OnClick="ASPxButton5_Click" 
										Text="ASPxButton">
									</dx:ASPxButton>
									<br />
									<dx:ASPxGridView ID="gv_q0" runat="server" AutoGenerateColumns="False" 
										ClientInstanceName="gv_q0" DataSourceID="SqlDataSource13" Font-Names="Arial" 
										KeyFieldName="id" Width="100%">
										<Columns>
											<dx:GridViewDataTextColumn Caption="Skill" FieldName="skill_name" 
												ShowInCustomizationForm="True" VisibleIndex="1" Width="200px">
												<CellStyle Wrap="False">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
												ShowInCustomizationForm="True" Visible="False" VisibleIndex="0" Width="40px">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Certification" FieldName="certificate_name" 
												ShowInCustomizationForm="True" VisibleIndex="2" Width="200px">
												<Settings AutoFilterCondition="Contains" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
												ShowInCustomizationForm="True" VisibleIndex="3" Width="300px">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataCheckColumn Caption="Is Internal" FieldName="is_internal" 
												ShowInCustomizationForm="True" VisibleIndex="4" Width="75px">
												<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" 
													ValueUnchecked="0">
												</PropertiesCheckEdit>
											</dx:GridViewDataCheckColumn>
											<dx:GridViewDataTextColumn Caption="Expires (days)" FieldName="expires" 
												ShowInCustomizationForm="True" VisibleIndex="5" Width="100px">
											</dx:GridViewDataTextColumn>
										</Columns>
										<SettingsBehavior ConfirmDelete="True" />
										<SettingsPager Mode="ShowAllRecords" Visible="False">
										</SettingsPager>
										<Settings ShowFilterRow="True" />
									</dx:ASPxGridView>
								</dx:PanelContent>
							</PanelCollection>
							<Border BorderColor="#9CDF7B" BorderStyle="Solid" BorderWidth="1px" />
						</dx:ASPxRoundPanel>
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
					</div>
				</td>
				<td __designer:mapid="150d">
					&nbsp;</td>
				<td __designer:mapid="1510">
					&nbsp;</td>
			</tr>
		</table>

			<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										SelectCommand="Select * from cr_group">
	</asp:SqlDataSource>

			<asp:SqlDataSource ID="SqlDataSource6" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													
										
										
		SelectCommand="SELECT id, priority_name FROM cr_skills_priority order by id">
	</asp:SqlDataSource>

	</div>




</asp:Content>

<asp:Content ID="Content4" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style1
		{
			font-size: small;
		}
		.style2
		{
			width: 100%;
		}
	</style>
</asp:Content>


