<%@ Page Title="Master Review Questions" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_hr_master_review_questions" Codebehind="master_review_questions.aspx.cs" %>	



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
	<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
		Width="100%">
		<TabPages>
			<dx:TabPage Text="Universal Review Items">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" BackColor="#99FF99" 
							HeaderText="Universal Review Questions" Width="100%">
							<HeaderStyle BackColor="#99FF99" Font-Bold="True" Font-Names="Arial" 
								Font-Size="16pt" />
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
									<asp:SqlDataSource ID="sqlrq" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
emp_review_items.id,
emp_review_group.group,
emp_review_items.item item,
emp_review_items.description description,
emp_review_items.`status`
FROM
emp_review_items
INNER JOIN emp_review_group ON emp_review_items.group = emp_review_group.id
ORDER BY
emp_review_group.id ASC,
emp_review_items.id ASC
"></asp:SqlDataSource>
									<dx:ASPxGridView ID="gvrq" runat="server" AutoGenerateColumns="False" 
										ClientInstanceName="gvrq" DataSourceID="sqlrq" Font-Names="Arial" 
										KeyFieldName="id" OnCustomCallback="gvrq_CustomCallback" Width="100%">
										<ClientSideEvents EndCallback="function(s, e) {
	mem_new.SetText('');
	txt_new.SetText('');
}" />
<ClientSideEvents EndCallback="function(s, e) {
	mem_new.SetText(&#39;&#39;);
	txt_new.SetText(&#39;&#39;);
}"></ClientSideEvents>
										<Columns>
											<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" 
												ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
												<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Group" FieldName="group" 
												ShowInCustomizationForm="True" VisibleIndex="1" Width="150px">
												<Settings AutoFilterCondition="Contains" />
												<DataItemTemplate>
													<dx:ASPxComboBox ID="ddl_rowgroup" runat="server" DataSourceID="SqlDataSource6" 
														oninit="ddl_rowgroup_Init" Text='<%# Eval("group") %>' TextField="group" 
														ValueField="id" ValueType="System.Int32" Width="100%">
													</dx:ASPxComboBox>
													<asp:SqlDataSource ID="SqlDataSource6" runat="server" 
														ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
														ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
														SelectCommand="Select * from emp_review_group"></asp:SqlDataSource>
												</DataItemTemplate>
												<CellStyle Wrap="False">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Item" FieldName="item" 
												ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
												<Settings AutoFilterCondition="Contains" />
												<DataItemTemplate>
													<dx:ASPxMemo ID="txt_questionrow" runat="server" Height="25px" 
														oninit="txt_questionrow_Init" Text='<%# Eval("item") %>' Width="100%">
													</dx:ASPxMemo>
												</DataItemTemplate>
												<CellStyle Wrap="False">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
												ShowInCustomizationForm="True" VisibleIndex="3" Width="350px">
												<Settings AutoFilterCondition="Contains" />
												<DataItemTemplate>
													<dx:ASPxMemo ID="mem_descrow" runat="server" Height="25px" 
														oninit="mem_descrow_Init" Text='<%# Eval("description") %>' Width="100%">
													</dx:ASPxMemo>
												</DataItemTemplate>
												<CellStyle Wrap="False">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
												ShowInCustomizationForm="True" VisibleIndex="4" Width="90px">
												<Settings AutoFilterCondition="Equals" />
												<DataItemTemplate>
													<dx:ASPxComboBox ID="ddlstatus_row" runat="server" oninit="ddlstatus_row_Init" 
														Value='<%# Eval("status") %>' Width="100%">
														<Items>
															<dx:ListEditItem Text="Active" Value="Active" />
															<dx:ListEditItem Text="InActive" Value="InActive" />
														</Items>
													</dx:ASPxComboBox>
												</DataItemTemplate>
												<CellStyle Wrap="False">
												</CellStyle>
											</dx:GridViewDataTextColumn>
										</Columns>
										<SettingsBehavior ColumnResizeMode="Control" />

<SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>

										<SettingsPager Mode="ShowAllRecords">
										</SettingsPager>
										<SettingsEditing EditFormColumnCount="7" Mode="PopupEditForm" />
										<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowTitlePanel="True" 
											ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
										<SettingsText PopupEditFormCaption="  Enter New Skill" />

<SettingsEditing Mode="PopupEditForm" EditFormColumnCount="7"></SettingsEditing>

<Settings ShowTitlePanel="True" ShowFilterRow="True" ShowFilterBar="Visible"></Settings>

<SettingsText PopupEditFormCaption="  Enter New Skill"></SettingsText>

										<SettingsPopup>
											<EditForm HorizontalAlign="WindowCenter" Modal="True" 
												VerticalAlign="WindowCenter" Width="900px" />
<EditForm Width="900px" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Modal="True"></EditForm>
										</SettingsPopup>
										<Styles>
											<Header BackColor="Silver" Font-Bold="True">
											</Header>
										</Styles>
										<StylesPopup>
											<EditForm>
												<Content BackColor="White">
													<Border BorderColor="White" BorderStyle="Solid" BorderWidth="10px" />
<Border BorderColor="White" BorderStyle="Solid" BorderWidth="10px"></Border>
												</Content>
											</EditForm>
										</StylesPopup>
										<Templates>
											<TitlePanel>
												<table style="width:100%;">
													<tr>
														<td width="100%">
															<dx:ASPxComboBox ID="ddlgroup" runat="server" ClientInstanceName="ddlgroup" 
																DataSourceID="SqlDataSource5" SelectedIndex="0" TextField="group" 
																ValueField="id" ValueType="System.Int32">
															</dx:ASPxComboBox>
															<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																SelectCommand="Select * from emp_review_group"></asp:SqlDataSource>
														</td>
														<td class="dxtcRightAlignCell" nowrap="nowrap" width="100%">
															&nbsp;</td>
														<td align="right" width="100%">
															&nbsp;</td>
													</tr>
													<tr>
														<td width="100%">
															<dx:ASPxTextBox ID="txt_new" runat="server" ClientInstanceName="txt_new" 
																Width="100%">
															</dx:ASPxTextBox>
														</td>
														<td class="dxtcRightAlignCell" nowrap="nowrap" width="100%">
															&nbsp;</td>
														<td align="right" width="100%">
															&nbsp;</td>
													</tr>
													<tr>
														<td width="100%">
															<dx:ASPxMemo ID="mem_new" runat="server" ClientInstanceName="mem_new" 
																Height="50px" Width="100%">
															</dx:ASPxMemo>
														</td>
														<td class="dxtcRightAlignCell" nowrap="nowrap" width="100%">
															&nbsp;</td>
														<td align="right" width="100%">
															<dx:ASPxButton ID="btnAdd" runat="server" AutoPostBack="False" Text="Add" 
																Wrap="False">
																<ClientSideEvents Click="function(s, e) {
	gvrq.PerformCallback();
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
			<dx:TabPage Text="Core Responsibility Items">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" BackColor="#66FF66" 
							HeaderText="Core Responsibility Review Items" Width="100%">
							<HeaderStyle BackColor="#66FF66" Font-Bold="True" Font-Names="Arial" 
								Font-Size="16pt" />
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
									<dx:ASPxGridView ID="gv_cr_review" runat="server" AutoGenerateColumns="False" 
										ClientInstanceName="gv_cr_review" DataSourceID="SqlDataSource7" 
										KeyFieldName="cr_review_id" OnCustomCallback="gv_cr_review_CustomCallback" Width="100%">
										<ClientSideEvents EndCallback="function(s, e) {
	tb_newreview.SetText('');
}" />
<ClientSideEvents EndCallback="function(s, e) {
	tb_newreview.SetText(&#39;&#39;);
}"></ClientSideEvents>
										<Columns>
											<dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" 
												VisibleIndex="0" ShowClearFilterButton="true">
												
											</dx:GridViewCommandColumn>
											<dx:GridViewDataTextColumn FieldName="cr_review_id" ReadOnly="True" 
												ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
												<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataComboBoxColumn Caption="Core Responsibility" 
												FieldName="cr_review_cr_id" ShowInCustomizationForm="True" VisibleIndex="2" 
												Width="400px">
												<PropertiesComboBox DataSourceID="SqlDataSource1" TextField="name" 
													ValueField="id" ValueType="System.Int32">
												</PropertiesComboBox>
												<Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
												<DataItemTemplate>
													<dx:ASPxComboBox ID="ddl_cr_review_crid" runat="server" 
														ClientInstanceName="ddl_cr_review_crid" DataSourceID="SqlDataSource1"  
														IncrementalFilteringMode="StartsWith" oninit="ddl_cr_review_crid_Init" 
														TextField="name" Value='<%# Eval("cr_review_cr_id") %>' ValueField="id" 
														ValueType="System.Int32" Width="400px">
													</dx:ASPxComboBox>
												</DataItemTemplate>
											</dx:GridViewDataComboBoxColumn>
											<dx:GridViewDataTextColumn Caption="Item" FieldName="cr_review_question" 
												ShowInCustomizationForm="True" VisibleIndex="3" Width="100%">
												<Settings AutoFilterCondition="Contains" />
												<DataItemTemplate>
													<dx:ASPxMemo ID="txt_cr_review_row" runat="server" Height="25px"
														ClientInstanceName="txt_cr_review_row" oninit="txt_cr_review_row_Init" Text='<%# Eval("cr_review_question") %>' 
														Width="100%">
													</dx:ASPxMemo>
												</DataItemTemplate>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
												ShowInCustomizationForm="True" VisibleIndex="4" Width="100px">
												<Settings AutoFilterCondition="Equals" />
												<DataItemTemplate>
													<dx:ASPxComboBox ID="ddl_cr_review_status" runat="server" 
														ClientInstanceName="ddl_cr_review_status" oninit="ddl_cr_review_status_Init" 
														Value='<%# Eval("status") %>' Width="100px">
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

										<SettingsPager PageSize="50">
										</SettingsPager>
										<Settings ShowTitlePanel="True" ShowFilterBar="Visible" ShowFilterRow="True" 
											ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />

<Settings ShowTitlePanel="True"></Settings>

										<Templates>
											<TitlePanel>
												<table style="width:100%;">
													<tr>
														<td>
															<dx:ASPxComboBox ID="ddl_new_cr_review" runat="server" 
																ClientInstanceName="ddl_new_cr_review" DataSourceID="SqlDataSource8" 
																
																TextField="name" ValueField="id" ValueType="System.Int32" Width="300px">
															</dx:ASPxComboBox>
															<asp:SqlDataSource ID="SqlDataSource8" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																SelectCommand="Select id,core_responsibility name from core_responsibilities order by core_responsibility">
															</asp:SqlDataSource>
														</td>
														<td width="100%">
															<dx:ASPxTextBox ID="tb_newreview" runat="server" 
																ClientInstanceName="tb_newreview" Width="100%">
															</dx:ASPxTextBox>
														</td>
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
										
										SelectCommand="Select cr_review_id, cr_review_cr_id,cr_review_question cr_review_question, cr_review_status status
FROM
cr_review
INNER JOIN core_responsibilities ON cr_review.cr_review_cr_id = core_responsibilities.id
order by cr_review_cr_id">
									</asp:SqlDataSource>
								</dx:PanelContent>
							</PanelCollection>
						</dx:ASPxRoundPanel>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Skills Review Items">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<asp:SqlDataSource ID="SqlDataSource9" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							SelectCommand="Select id,name name from master_skills order by name">
						</asp:SqlDataSource>
						<dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" BackColor="#33CC33" 
							HeaderText="Skills Review Items" Width="100%">
							<HeaderStyle BackColor="#33CC33" Font-Bold="True" Font-Names="Arial" 
								Font-Size="16pt" />
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
									<dx:ASPxGridView ID="gv_s_r" runat="server" AutoGenerateColumns="False" 
										ClientInstanceName="gv_s_r" DataSourceID="sqlskillreview" 
										KeyFieldName="master_skill_review_questions_id" 
										OnCustomCallback="gv_s_review_CustomCallback" Width="100%">
										<ClientSideEvents EndCallback="function(s, e) {
	tb_newreview.SetText('');
}" />
<ClientSideEvents EndCallback="function(s, e) {
	tb_newreview.SetText(&#39;&#39;);
}"></ClientSideEvents>
										<Columns>
											<dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False"  ShowClearFilterButton="true"
												VisibleIndex="0">
												
											</dx:GridViewCommandColumn>
											<dx:GridViewDataTextColumn FieldName="master_skill_review_questions_id" 
												ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
												<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataComboBoxColumn Caption="Skill" 
												FieldName="master_skill_review_questions_skill_id" 
												ShowInCustomizationForm="True" VisibleIndex="2" Width="400px">
												<PropertiesComboBox DataSourceID="SqlDataSource2" TextField="name" 
													ValueField="id" ValueType="System.Int32">
												</PropertiesComboBox>
												<Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
												<DataItemTemplate>
													<dx:ASPxComboBox ID="ddl_cr_review_crid0" runat="server" 
														ClientInstanceName="ddl_cr_review_crid0" DataSourceID="SqlDataSource9" 
														EnableCallbackMode="True" 
														IncrementalFilteringMode="StartsWith" oninit="ddl_cr_review_crid0_Init" 
														TextField="name" Value='<%# Eval("master_skill_review_questions_skill_id") %>' 
														ValueField="id" ValueType="System.Int32" Width="400px">
													</dx:ASPxComboBox>
												</DataItemTemplate>
											</dx:GridViewDataComboBoxColumn>
											<dx:GridViewDataTextColumn Caption="Item" FieldName="question" 
												ShowInCustomizationForm="True" VisibleIndex="3" Width="100%">
												<Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
												<DataItemTemplate>
													<dx:ASPxMemo ID="txt_cr_review_row0" runat="server" Height="25px"
														ClientInstanceName="txt_cr_review_row0" oninit="txt_cr_review_row0_Init" 
														Text='<%# Eval("question") %>' Width="100%">
													</dx:ASPxMemo>
												</DataItemTemplate>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Status" 
												FieldName="master_skill_review_questions_status" ShowInCustomizationForm="True" 
												VisibleIndex="4" Width="100px">
												<Settings AutoFilterCondition="Equals" />
												<DataItemTemplate>
													<dx:ASPxComboBox ID="ddl_cr_review_status0" runat="server" 
														ClientInstanceName="ddl_cr_review_status0" oninit="ddl_cr_review_status0_Init" 
														Value='<%# Eval("status") %>' Width="100px">
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

										<SettingsPager PageSize="50">
										</SettingsPager>
										<Settings ShowTitlePanel="True" ShowFilterBar="Visible" ShowFilterRow="True" 
											ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" ShowFooter="True" 
											ShowHeaderFilterButton="True" />

<Settings ShowTitlePanel="True"></Settings>

										<Templates>
											<TitlePanel>
												<table style="width:100%;">
													<tr>
														<td>
															<dx:ASPxComboBox ID="ddl_new_cr_review0" runat="server" 
																ClientInstanceName="ddl_new_cr_review0" DataSourceID="SqlDataSource10" 
																TextField="name" ValueField="id" ValueType="System.Int32" Width="300px">
															</dx:ASPxComboBox>
															<asp:SqlDataSource ID="SqlDataSource10" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																SelectCommand="Select id,name name from master_skills order by name">
															</asp:SqlDataSource>
														</td>
														<td width="100%">
															<dx:ASPxTextBox ID="tb_newreview0" runat="server" 
																ClientInstanceName="tb_newreview0" Width="100%">
															</dx:ASPxTextBox>
														</td>
														<td>
															<dx:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="False" Text="Add">
																<ClientSideEvents Click="function(s, e) {
	gv_s_r.PerformCallback();

}" />
															</dx:ASPxButton>
														</td>
													</tr>
												</table>
											</TitlePanel>
										</Templates>
									</dx:ASPxGridView>
									<asp:SqlDataSource ID="sqlskillreview" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										SelectCommand="SELECT
master_skill_review_questions.master_skill_review_questions_id,
master_skill_review_questions.master_skill_review_questions_question question,
master_skill_review_questions.master_skill_review_questions_status status,
master_skill_review_questions.master_skill_review_questions_skill_id
FROM
master_skill_review_questions order by master_skill_review_questions.master_skill_review_questions_skill_id">
									</asp:SqlDataSource>
								</dx:PanelContent>
							</PanelCollection>
						</dx:ASPxRoundPanel>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>
	</dx:ASPxPageControl>

			<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										SelectCommand="Select * from cr_group">
	</asp:SqlDataSource>

		

	<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																
		SelectCommand="Select id,core_responsibility name from core_responsibilities order by core_responsibility">
	</asp:SqlDataSource>

		

</td>
</tr>
</table>



</asp:Content>
<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	</asp:Content>

