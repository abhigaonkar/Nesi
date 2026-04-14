<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_taskmanager_index" Title="Task Detail" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
<script type="text/javascript" src="/js/functions.js"></script>
<script type="text/javascript">
	function resizeIframe(obj)
 {

   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 20) + 'px';
	
	
 }
	
	function row_click(s, e, index)
		{
		pc_main.SetActiveTab(pc_main.GetTab(1));
		persist.Set("row_index", index);
		cbp_main.PerformCallback();
		}
	function current_name_check(s,e)
		{
		if(t_name.GetText() != "")
			{
			cbp_main.PerformCallback("name_check");
			}
		}
	function new_name_check(s,e)
		{
		if(t_new_name.GetText() != "")
			{
			cbp_new.PerformCallback("name_check");
			}
		}
	function current_phone_check(s, e) 
		{
		if(
		t_phone_area.GetText() != "" && 
		t_phone_prefix.GetText() != "" && 
		t_phone_suffix.GetText() != "")
				{
				cbp_main.PerformCallback("phone_check");
				}
		}
	function new_phone_check(s, e) 
		{
		if(
		t_new_phone_area.GetText() != "" && 
		t_new_phone_prefix.GetText() != "" && 
		t_new_phone_suffix.GetText() != "")
				{
				cbp_new.PerformCallback("phone_check");
				}
		}
	function add_to_branch(from, to, id)
		{
		please_wait('begin');
		$.get("./index.aspx",
				{
				add_to_branch:		true,
				frombusiness_unit_id:		from,
				tobusiness_unit_id:		to,
				vendor_id:			id
				},
				function(ret)
					{
					if(ret == "SUCCESS")
						{
						please_wait('stop');
						gv_vendor.Refresh();
						}
					else
						{
						please_wait('stop');
						alert(ret);
						}
					});
		}
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Vendor</title>
	<style type="text/css">
		body		{
					padding:			5px;
					margin:				5px;
					}
		.error		{
					overflow-y:			scroll;
					overflow-x:			hidden;
					white-space:		nowrap;
					}
		.style5
		{
			font-family: Arial;
			font-size: small;
		}
		.style6
		{
			width: 100%;
			height: 117px;
		}
		.style7
		{
			color: #FF3300;
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<dx:ASPxCallbackPanel ID="cb_main" runat="server" ClientInstanceName="cb_main" 
			oncallback="cb_main_Callback" Width="100%">
			<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<table style="width: 100%; font-family: Arial, Helvetica, sans-serif;">
		<tr>
			<td>
				<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="2" 
					Width="100%" Height="0px" Font-Names="Arial">
					<TabPages>
						<dx:TabPage Name="General" Text="General">
							<ContentCollection>
								<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
									<table class="style5" 
										style="width: 100%; font-family: Arial, Helvetica, sans-serif;">
										<tr>
											<td style="font-weight: bold">
												Task:</td>
											<td colspan="3">
												<dx:ASPxTextBox ID="txttask" runat="server" Width="100%">
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td style="font-weight: bold">
												Task ID:</td>
											<td>
												<dx:ASPxLabel ID="lblid" runat="server" ClientInstanceName="lblid" Text="0">
												</dx:ASPxLabel>
											</td>
											<td>
												<strong>
												<dx:ASPxCheckBox ID="chkprivate" runat="server" CheckState="Unchecked" 
													CssClass="style7" Font-Bold="True" Text="Private">
												</dx:ASPxCheckBox>
												</strong>
											</td>
											<td>
												&nbsp;</td>
										</tr>
										<tr>
											<td style="font-weight: bold">
												Owner:</td>
											<td>
												<dx:ASPxComboBox ID="ddlowner" runat="server" DataSourceID="SqlDataSource2" 
													AnimationType="None" TextField="membername" ValueField="member_id" 
													ValueType="System.Int32" CallbackPageSize="10" EnableCallbackMode="True"
													IncrementalFilteringMode="StartsWith">
												</dx:ASPxComboBox>
												<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													SelectCommand="Select member_id, concat(get_name(member_id),' - ',business_unit_name(business_unit_id)) membername from member where member_status = 'Active' order by business_unit_id,get_name(member_id)">
												</asp:SqlDataSource>
											</td>
											<td nowrap="nowrap">
												<strong>Percent Complete:</strong></td>
											<td>
												<dx:ASPxLabel ID="lblpercent" runat="server" Text="ASPxLabel" Width="170px">
												</dx:ASPxLabel>
											</td>
										</tr>
										<tr>
											<td style="font-weight: bold">
												Priority:</td>
											<td>
												<dx:ASPxComboBox ID="ddlpriority" runat="server" DataSourceID="SqlDataSource3" 
													AnimationType="None" TextField="ticketpriority_name" 
													ValueField="ticketpriority_id" ValueType="System.Int32">
												</dx:ASPxComboBox>
												<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													SelectCommand="SELECT ticketpriority_id, ticketpriority_name FROM ticketpriority">
												</asp:SqlDataSource>
											</td>
											<td nowrap="nowrap">
												<strong>Created By:</strong></td>
											<td>
												<dx:ASPxLabel ID="lblcreated" runat="server" Text="ASPxLabel" Width="170px">
												</dx:ASPxLabel>
											</td>
										</tr>
										<tr>
											<td style="font-weight: bold">
												Due Date:</td>
											<td>
												<dx:ASPxDateEdit ID="dteDue" runat="server" DisplayFormatString="yyyy-MM-dd" 
													EditFormat="Custom" EditFormatString="yyyy-MM-dd" AnimationType="None">
												</dx:ASPxDateEdit>
											</td>
											<td>
												<strong>Business Unit:</strong></td>
											<td>
												<dx:ASPxComboBox ID="ddldept" runat="server" DataSourceID="SqlDataSource5" 
													AnimationType="None" TextField="name" ValueField="id" 
													ValueType="System.Int32">
												</dx:ASPxComboBox>
												<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
                                                    SelectCommand="Select id, ddl_name name from business_unit  where find_in_set(id,@visible_business_unit_ids)"
                                                    >
												    <SelectParameters>
												        <asp:SessionParameter SessionField="visible_business_unit_ids" Name="@visible_business_unit_ids"  />
												    </SelectParameters>

												</asp:SqlDataSource>
											</td>
										</tr>
										<tr>
											<td style="font-weight: bold" nowrap="nowrap">
												Parent Task:</td>
											<td>
												<dx:ASPxComboBox ID="ddlparent" runat="server" DataSourceID="SqlDataSource4" 
													AnimationType="None" TextField="task_name" ValueField="id" 
													ValueType="System.Int32">
												</dx:ASPxComboBox>
												<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													SelectCommand="Select id,urldecode(name) task_name from task where id != ?id">
													<SelectParameters>
														<asp:ControlParameter ControlID="hdn_taskid" Name="id" PropertyName="Value" />
													</SelectParameters>
												</asp:SqlDataSource>
											</td>
											<td>
												<strong>Expected Hours:</strong></td>
											<td>
												<dx:ASPxTextBox ID="txtexphours" runat="server" Width="170px">
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td bgcolor="#FFFFE1" style="font-weight: bold">
												Description:</td>
											<td colspan="3">
												<dx:ASPxMemo ID="ASPxMemo1" runat="server" BackColor="#FFFFCC" Height="71px" 
													Width="100%">
												</dx:ASPxMemo>
											</td>
										</tr>
										<tr>
											<td bgcolor="#E1FFE1" style="font-weight: bold" valign="top">
												Members:</td>
											<td style="padding: 0px" colspan="3">
												<dx:ASPxCallbackPanel ID="cb_members" runat="server" 
													ClientInstanceName="cb_members" OnCallback="cb_members_Callback" Width="100%" BackColor="#CCFFCC">
													<PanelCollection>
														<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
															<table cellpadding="0" width="100%">
																<tr>
																	<td style="padding: 0px" width="10%" colspan="2">
																		<dx:ASPxComboBox ID="ddladdmember" runat="server" DataSourceID="SqlDataSource2" 
																			AnimationType="None" TextField="membername" ValueField="member_id" 
																			ValueType="System.Int32" CallbackPageSize="10" EnableCallbackMode="True">
																			<ClientSideEvents ButtonClick="function(s, e) {
	cb_members.PerformCallback(lblid.GetText());
}" />
<ClientSideEvents ButtonClick="function(s, e) {
	cb_members.PerformCallback(lblid.GetText());
}"></ClientSideEvents>
																			<Buttons>
																				<dx:EditButton Text="Add">
																				</dx:EditButton>
																			</Buttons>
																		</dx:ASPxComboBox>
																	</td>
																</tr>
																<tr>
																	<td style="padding: 0px" width="10%" colspan="1">
																		<dx:ASPxGridView ID="gv_members" runat="server" AutoGenerateColumns="False" 
																			EnableCallBacks="False" KeyFieldName="task_member_member_id" 
																			OnRowDeleting="gv_members_RowDeleting" Width="400px">
                                                                            <SettingsCommandButton>                                                                               
                                                                                <DeleteButton Text="Add New" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
                                                                            </SettingsCommandButton>
																			<Columns>
																				<dx:GridViewCommandColumn ButtonType="Image" ShowInCustomizationForm="True" ShowDeleteButton="true"
																					VisibleIndex="0" Width="30px">
																					
																				</dx:GridViewCommandColumn>
																				<dx:GridViewDataTextColumn FieldName="task_member_member_id" 
																					ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataTextColumn FieldName="member" ShowInCustomizationForm="True" 
																					VisibleIndex="2">
																				</dx:GridViewDataTextColumn>
																			</Columns>
																			<SettingsBehavior ConfirmDelete="True" />

<SettingsBehavior ConfirmDelete="True"></SettingsBehavior>

																			<SettingsPager Visible="False">
																			</SettingsPager>
																			<Settings GridLines="None" ShowColumnHeaders="False" 
																				ShowHeaderFilterBlankItems="False" VerticalScrollBarMode="Visible" 
																				VerticalScrollableHeight="100" />

<Settings ShowHeaderFilterBlankItems="False" ShowColumnHeaders="False" VerticalScrollBarMode="Visible" VerticalScrollableHeight="100" GridLines="None"></Settings>
																		</dx:ASPxGridView>
																	</td>
																</tr>
															</table>
														</dx:PanelContent>
													</PanelCollection>
												</dx:ASPxCallbackPanel>
											</td>
										</tr>
										<tr>
											<td style="font-weight: bold">
												&nbsp;</td>
											<td style="padding: 0px">
												<table style="width:100%;">
													<tr>
														<td dir="ltr" nowrap="nowrap" style="white-space: nowrap">
															&nbsp;<table class="style6">
																<tr>
																	<td>
																		<dx:ASPxCheckBox ID="chkemail" runat="server" CheckState="Unchecked" 
																			Text="Email Notifications" Width="170px" Wrap="False">
																		</dx:ASPxCheckBox>
																	</td>
																	<td>
																		&nbsp;</td>
																	<td>
																		&nbsp;</td>
																</tr>
																<tr>
																	<td nowrap="nowrap">
																		<dx:ASPxCheckBox ID="chkmessageboard" runat="server" CheckState="Unchecked" 
																			Text="Use Messageboard " Wrap="False">
																		</dx:ASPxCheckBox>
																	</td>
																	<td>
																		&nbsp;</td>
																	<td>
																		&nbsp;</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxSpinEdit ID="spndayswarning" runat="server" Height="21px" Number="0" 
																			Width="100px">
																		</dx:ASPxSpinEdit>
																	</td>
																	<td colspan="2" width="100%">
																		&nbsp;- Days warning before due date if not completed</td>
																</tr>
																<tr>
																	<td colspan="3">
																		<table style="width:100%;">
																			<tr>
																				<td>
																					<dx:ASPxCheckBox ID="chkrecurring" runat="server" CheckState="Unchecked" 
																						Text="Recurring">
																						<ClientSideEvents CheckedChanged="function(s, e) {
	ddlresolution.SetVisible(s.GetChecked());
	txtrec_days.SetVisible(s.GetChecked());
	lblrecurringtext.SetVisible(s.GetChecked());
	ddlresolution.SetSelectedIndex(1);
	txtrec_days.SetText('1');
}" />
<ClientSideEvents CheckedChanged="function(s, e) {
	ddlresolution.SetVisible(s.GetChecked());
	txtrec_days.SetVisible(s.GetChecked());
	lblrecurringtext.SetVisible(s.GetChecked());
	ddlresolution.SetSelectedIndex(1);
	txtrec_days.SetText(&#39;1&#39;);
}"></ClientSideEvents>
																					</dx:ASPxCheckBox>
																				</td>
																				<td>
																					<dx:ASPxLabel ID="lblrecurringtext" runat="server" 
																						ClientInstanceName="lblrecurringtext" ClientVisible="False" Text="Every">
																					</dx:ASPxLabel>
																				</td>
																				<td>
																					<dx:ASPxTextBox ID="txtrec_days" runat="server" 
																						ClientInstanceName="txtrec_days" ClientVisible="False" Width="30px">
																					</dx:ASPxTextBox>
																				</td>
																				<td>
																					<dx:ASPxComboBox ID="ddlresolution" runat="server" 
																						ClientInstanceName="ddlresolution" ClientVisible="False">
																						<Items>
																							<dx:ListEditItem Text="Days" Value="1" />
																							<dx:ListEditItem Text="Months" Value="2" />
																							<dx:ListEditItem Text="Years" Value="3" />
																							<dx:ListEditItem Text="Quarters" Value="4" />
																						</Items>
																					</dx:ASPxComboBox>
																				</td>
																				<td width="100%">
																					&nbsp;</td>
																			</tr>
																		</table>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</td>
											<td>
												&nbsp;</td>
											<td>
												&nbsp;</td>
										</tr>
										<tr>
											<td>
												<dx:ASPxButton ID="btnsave" runat="server" AutoPostBack="False" Text="Save">
													<ClientSideEvents Click="function(s, e) {
	cb_main.PerformCallback();
}" />
<ClientSideEvents Click="function(s, e) {
	cb_main.PerformCallback();
}"></ClientSideEvents>
												</dx:ASPxButton>
											</td>
											<td style="padding: 0px">
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
						<dx:TabPage Name="history" Text="History">
							<ContentCollection>
								<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
									<table style="width: 100%;">
										<tr>
											<td>
												&nbsp;</td>
											<td>
												&nbsp;</td>
											<td>
												&nbsp;</td>
										</tr>
										<tr>
											<td colspan="3">
												<dx:ASPxGridView ID="gv_history" runat="server" ClientInstanceName="gv_history" 
													DataSourceID="SqlDataSource1" Width="100%" AutoGenerateColumns="False" Font-Names="Arial" 
													OnCustomCallback="gv_history_CustomCallback">
													<Columns>
														<dx:GridViewDataDateColumn Caption="Date" FieldName="task_history_date" 
															ShowInCustomizationForm="True" VisibleIndex="0" Width="125px">
															<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																EditFormatString="yyyy-MM-dd">
															</PropertiesDateEdit>
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataDateColumn>
														<dx:GridViewDataComboBoxColumn Caption="Member" 
															FieldName="task_history_memberid" ShowInCustomizationForm="True" 
															VisibleIndex="1" Width="125px">
															<PropertiesComboBox DataSourceID="SqlDataSource6" TextField="membername" 
																ValueField="member_id" ValueType="System.Int32">
															</PropertiesComboBox>
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataComboBoxColumn>
														<dx:GridViewDataMemoColumn Caption="Notes" FieldName="task_history_note" 
															ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
															<PropertiesMemoEdit Height="20px">
															</PropertiesMemoEdit>
														</dx:GridViewDataMemoColumn>
													</Columns>
													<SettingsPager PageSize="20">
													</SettingsPager>
													<Settings ShowTitlePanel="True" VerticalScrollBarMode="Visible"
														VerticalScrollableHeight="500" />
													<Styles>
														<Header Font-Bold="True">
														</Header>
													</Styles>
													<Templates>
														<TitlePanel>
															<table style="width:100%;">
																<tr>
																	<td width="100%">
																		<dx:ASPxMemo ID="mem_history" runat="server" BackColor="#FFFFCC" 
																			ClientInstanceName="mem_history" Height="30px" Width="100%">
																		</dx:ASPxMemo>
																	</td>
																	<td>
																		<dx:ASPxButton ID="btnaddhistory" runat="server" AutoPostBack="False" 
																			ClientInstanceName="btnaddhistory" Height="30px" Text="Add" Width="100px">
																			<ClientSideEvents Click="function(s, e) {
	gv_history.PerformCallback(&quot;add_note&quot;);
}" />
																		</dx:ASPxButton>
																	</td>
																	<td>
																		&nbsp;</td>
																</tr>
															</table>
														</TitlePanel>
													</Templates>
												</dx:ASPxGridView>
											</td>
										</tr>
										<tr>
											<td>
												<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													
													SelectCommand="SELECT
task_history.task_history_id,
task_history.task_history_taskid,
task_history.task_history_date,
task_history.task_history_memberid,
urldecode(task_history.task_history_note)task_history_note from task_history where task_history_taskid = ?id order by task_history_id desc">
													<SelectParameters>
														<asp:ControlParameter ControlID="hdn_taskid" Name="id" PropertyName="Value" />
													</SelectParameters>
												</asp:SqlDataSource>
												<asp:SqlDataSource ID="SqlDataSource6" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													SelectCommand="Select member_id, concat(get_name(member_id),' - ',business_unit_name(business_unit_id)) membername from member where member_status = 'Active' order by business_unit_id,get_name(member_id)">
												</asp:SqlDataSource>
											</td>
											<td>
												&nbsp;</td>
											<td>
												&nbsp;</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Name="Files" Text="Files">
							<ContentCollection>
								<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
									<iframe ID="files_frame" runat="server" frameborder="0" height="530" 
										name="files_frame" scrolling="no" 
										style="border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none" 
										width="100%"></iframe>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Name="Subtasks" Text="Subtasks">
							<ContentCollection>
								<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
									<iframe ID="I99" runat="server" frameborder="0" height="650" name="I99" 
										scrolling="auto" 
										style="border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none" 
										width="100%" src="list.aspx"></iframe>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
					</TabPages>
				</dx:ASPxPageControl><asp:HiddenField ID="hdn_taskid" runat="server" />
			</td>
	</table>
				</dx:PanelContent>
</PanelCollection>
		</dx:ASPxCallbackPanel>
	</form>
</body>
</html>
