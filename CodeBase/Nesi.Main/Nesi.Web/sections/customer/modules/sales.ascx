<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_customer_modules_sales" Codebehind="sales.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>






<div>
	<dx:ASPxPageControl runat="server" ID="sales_tab" ActiveTabIndex="0" 
		Width="100%" Font-Names="Arial" EnableCallbacks="True">
		<TabPages>
			<dx:TabPage Text="Properties 1">
				<ContentCollection>
					<dx:ContentControl runat="server">
						<table width="100%" cellpadding="0" cellspacing="0">
							<tr>
								<td width="500">
									<table cellpadding="2" cellspacing="0">
										<tr>
											<td class="c">
												Status:
											</td>
											<td class="v">
												<dx:ASPxComboBox ID="cb_status" runat="server" TextField="status" ValueField="id" ValueType="System.Int32" Native="True">
												</dx:ASPxComboBox>
											</td>
										</tr>
										<tr>
											<td class="c">
												Call Cycle (Days):
											</td>
											<td class="s">
												<dx:ASPxSpinEdit runat="server" ID="spin_callcycle" Width="50px">
												</dx:ASPxSpinEdit>
											</td>
										</tr>
										<tr>
											<td class="c">
												Year End:
											</td>
											<td class="v">
												<dx:ASPxComboBox runat="server" ID="cb_yearend" Native="true" ValueType="System.Int32">
													<Items>
														<dx:ListEditItem Text="Unknown" Value="0" />
														<dx:ListEditItem Text="January" Value="1" />
														<dx:ListEditItem Text="February" Value="2" />
														<dx:ListEditItem Text="March" Value="3" />
														<dx:ListEditItem Text="April" Value="4" />
														<dx:ListEditItem Text="May" Value="5" />
														<dx:ListEditItem Text="June" Value="6" />
														<dx:ListEditItem Text="July" Value="7" />
														<dx:ListEditItem Text="August" Value="8" />
														<dx:ListEditItem Text="September" Value="9" />
														<dx:ListEditItem Text="October" Value="10" />
														<dx:ListEditItem Text="November" Value="11" />
														<dx:ListEditItem Text="December" Value="12" />
													</Items>
												</dx:ASPxComboBox>
											</td>
										</tr>
										<tr>
											<td class="c">
												Project Manager:
											</td>
											<td class="v">
												<dx:ASPxComboBox ID="cb_projectmanager" runat="server" DataSourceID="ds_pms" 
													Native="True" TextField="member_fullname" ValueField="member_id" 
													ValueType="System.Int32">
												</dx:ASPxComboBox>
											</td>
										</tr>
										<tr>
											<td class="c">
												Controls Manager:
											</td>
											<td class="v">
												<dx:ASPxComboBox runat="server" ID="cb_controlsmanager" Native="true" DataSourceID="ds_pms" TextField="member_fullname" ValueField="member_id" ValueType="System.Int32">
												</dx:ASPxComboBox>
											</td>
										</tr>
										<tr>
											<td class="c">
												Discount:
											</td>
											<td class="s">
												<table>
													<tr>
														<td style="max-width: 75px;">
															<dx:ASPxSpinEdit runat="server" ID="spin_discount" Width="50px">
															</dx:ASPxSpinEdit>
														</td>
														<td>
															%
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td class="c">
												Decision Maker:
											</td>
											<td class="v">
												<dx:ASPxComboBox runat="server" ID="cb_decisionmaker" DataSourceID="ds_contacts" TextField="contact_name" ValueField="contact_id" ValueType="System.Int32" Native="true">
												</dx:ASPxComboBox>
											</td>
										</tr>
										<tr>
											<td class="c">
												Account Code:
												<br />
												<strong>A-</strong> They can bring us other A,B or C type customers<br />
												<strong>B-</strong> They have mulitple facilities and would use more than 1 branch as a result<br />
												<strong>C-</strong> They have more than 50 employees in either manufacturing or logistics<br />
												<strong>D-</strong> Everyone else<br />
                                                <strong>TN-</strong> Target New<br />
                                                <strong>TE-</strong> Target Expand<br />
                                                <strong>P-</strong> Prospect<br />
											</td>
											<td class="v">
												<dx:ASPxComboBox runat="server" ID="cb_accountcode" Native="true" ValueType="System.String">
													<Items>
														<dx:ListEditItem Text="" Value="" />
														<dx:ListEditItem Text="A" Value="A" />
														<dx:ListEditItem Text="B" Value="B" />
														<dx:ListEditItem Text="C" Value="C" />
														<dx:ListEditItem Text="D" Value="D" />
                                                        <dx:ListEditItem Text="TN" Value="TN" />
                                                        <dx:ListEditItem Text="TE" Value="TE" />
                                                        <dx:ListEditItem Text="P" Value="P" />
													</Items>
												</dx:ASPxComboBox>
											</td>
										</tr>
										<tr>
											<td class="c">
												Next Followup Date:
											</td>
											<td class="v">
												<dx:ASPxDateEdit ID="date_next_followup" runat="server">
												</dx:ASPxDateEdit>
											</td>
										</tr>
										<tr>
											<td class="c" valign="top">
												Next Followup Notes:
											</td>
											<td class="s">
												<dx:ASPxMemo ID="memo_next_followup" runat="server" Height="71px" Width="170px">
												</dx:ASPxMemo>
											</td>
										</tr>
										<tr>
											<td class="c">
												Job Budget Threshold:
											</td>
											<td class="v">
												<dx:ASPxTextBox ID="tb_job_budget" runat="server" Width="170px">
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td class="c">
												PO Required:
											</td>
											<td class="v">
												<dx:ASPxCheckBox ID="chk_porequired" runat="server" CheckState="Unchecked">
												</dx:ASPxCheckBox>
											</td>
										</tr>
										<tr>
											<td class="c">
												Confirmed PO is/isn't required?:
											</td>
											<td class="v">
												<dx:ASPxRadioButtonList ID="rbl_poconfirmed" runat="server" RepeatDirection="Horizontal" SelectedIndex="1" ValueType="System.Int32">
													<Items>
														<dx:ListEditItem Text="Yes" Value="1" />
														<dx:ListEditItem Selected="True" Text="No" Value="0" />
													</Items>
												</dx:ASPxRadioButtonList>
											</td>
										</tr>
										<tr>
											<td class="c">
												Confirmed Tax Exemption:
											</td>
											<td class="v">
												<dx:ASPxRadioButtonList ID="rbl_taxexempt" runat="server" RepeatDirection="Horizontal" SelectedIndex="1" ValueType="System.Int32">
													<Items>
														<dx:ListEditItem Text="Yes" Value="1" />
														<dx:ListEditItem Selected="True" Text="No" Value="0" />
													</Items>
												</dx:ASPxRadioButtonList>
											</td>
										</tr>
									</table>
								</td>
								<td align="left" valign="top">
								</td>
								<td>
								</td>
							</tr>
						</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Properties 2">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table cellpadding="2" cellspacing="0" class="info">
							<tr>
								<td class="c">
									Industry:
								</td>
								<td class="v">
									<dx:ASPxComboBox CssClass="ddl" ID="cb_industry" Native="true" runat="server" ValueType="System.Int32">
										<Items>
											<dx:ListEditItem Text="Commercial" Value="1" />
											<dx:ListEditItem Text="Industrial" Value="2" />
											<dx:ListEditItem Text="Institutional" Value="3" />
										</Items>
									</dx:ASPxComboBox>
								</td>
							</tr>
							<tr>
								<td class="c">
									How customer found us:
								</td>
								<td class="v">
									<dx:ASPxComboBox ID="cb_found" runat="server" Native="true" ClientInstanceName="cb_found" DataSourceID="ds_origin" OnCallback="combo_lastorigin_Callback" TextField="name" ValueField="id" ValueType="System.Int32">
										<ClientSideEvents SelectedIndexChanged="function(s,e){NE_Customer.origin_control.sales.chk(s)}" />
									</dx:ASPxComboBox>
									<dx:ASPxButton ID="bt_found" runat="server" AutoPostBack="false" Text="Origins"><ClientSideEvents Click="function(s,e){pop_origins.Show();}" /></dx:ASPxButton>
								</td>
							</tr>
							<tr>
								<td class="c">
									Sector:
								</td>
								<td class="v">
									<dx:ASPxTextBox ID="tb_sector" CssClass="tb" runat="server">
									</dx:ASPxTextBox>
								</td>
							</tr>
							<tr>
								<td class="c">
									NAICS Code #:
								</td>
								<td class="v">
									<dx:ASPxTextBox ID="tb_naics" CssClass="tb" runat="server">
										<ValidationSettings ValidationGroup="vg">
											<RegularExpression ErrorText="Must be 1 to 6 digits" ValidationExpression="\d{1,6}" />
										</ValidationSettings>
									</dx:ASPxTextBox>
								</td>
							</tr>
							<tr>
								<td class="c">
									Employee Size:
								</td>
								<td class="v">
									<dx:ASPxComboBox CssClass="ddl" ID="cb_empsize" Native="true" runat="server" ValueType="System.Int32">
										<Items>
											<dx:ListEditItem Text="1 - 4" Value="1" />
											<dx:ListEditItem Text="5 - 9" Value="2" />
											<dx:ListEditItem Text="10 - 19" Value="3" />
											<dx:ListEditItem Text="20 - 49" Value="4" />
											<dx:ListEditItem Text="50 - 99" Value="5" />
											<dx:ListEditItem Text="100 - 199" Value="6" />
											<dx:ListEditItem Text="200 - 499" Value="7" />
											<dx:ListEditItem Text="500 - 999" Value="8" />
											<dx:ListEditItem Text="1000 - 9999" Value="9" />
											<dx:ListEditItem Text="10000+" Value="10" />
										</Items>
									</dx:ASPxComboBox>
								</td>
							</tr>
							<tr>
								<td class="c">
									What do they do at this location?:
								</td>
								<td class="v">
									<dx:ASPxTextBox ID="tb_do_at_location" CssClass="tb" runat="server">
									</dx:ASPxTextBox>
								</td>
							</tr>
							<tr>
								<td class="c" valign="top">
									Why did they choose us/buy from us?
								</td>
								<td class="s">
									<dx:ASPxMemo ID="memo_whychoose" runat="server" CssClass="memo" Height="100px">
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td class="c" valign="top">
									Affiliated Companies:
								</td>
								<td class="s">
									<dx:ASPxMemo ID="memo_affiliated" Height="100px" CssClass="memo" runat="server">
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td class="c" valign="top">
									Known Suppliers:
								</td>
								<td class="s">
									<dx:ASPxMemo ID="memo_suppliers" Height="100px" CssClass="memo" runat="server">
									</dx:ASPxMemo>
								</td>
							</tr>
							<tr>
								<td class="c" valign="top">
									Known Competitors:
								</td>
								<td class="s">
									<dx:ASPxMemo ID="memo_competitors" Height="100px" CssClass="memo" runat="server">
									</dx:ASPxMemo>
								</td>
							</tr>
						</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Protected" Name="protected">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table width="100%" cellpadding="2" cellspacing="0">
							<tr>
								<td class="c">
									Inside Sales Rep:
								</td>
								<td class="v">
									<dx:ASPxComboBox runat="server" DataSourceID="ds_isr" TextField="member_fullname" ValueField="member_id" Native="true" ID="cb_insidesales" ValueType="System.Int32">
									</dx:ASPxComboBox>
								</td>
							</tr>
							<tr>
								<td class="c">
									Outside Sales Rep:
								</td>
								<td class="v">
									<dx:ASPxComboBox runat="server" DataSourceID="ds_osr" TextField="member_fullname" ValueField="member_id" Native="true" ID="cb_outsidesales" ValueType="System.Int32">
									</dx:ASPxComboBox>
								</td>
							</tr>
							<tr>
								<td class="c">
									Reg. Account Manager:
								</td>
								<td class="v">
									<dx:ASPxComboBox runat="server" DataSourceID="ds_ram" TextField="member_fullname" ValueField="member_id" Native="true" ID="cb_regacctmgr" ValueType="System.Int32">
									</dx:ASPxComboBox>
								</td>
							</tr>
							<tr>
								<td class="c">
									Account Manager:</td>
								<td class="v">
									<dx:ASPxComboBox ID="cb_am" runat="server" DataSourceID="ds_am" Native="True" TextField="fn" ValueField="id" ValueType="System.Int32">
									</dx:ASPxComboBox>
								</td>
							</tr>
						</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="History" Name="history">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxGridView ID="gv_history" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_history" DataSourceID="sql_history" Font-Names="Arial" Font-Size="9pt" KeyFieldName="id" Width="100%">
							<Columns>
								<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" VisibleIndex="0" Width="1px" ShowClearFilterButton="true">
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="Date" FieldName="date" ShowInCustomizationForm="True" VisibleIndex="1" Width="100px">
									<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
									</PropertiesTextEdit>
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Action" FieldName="action" ShowInCustomizationForm="True" VisibleIndex="2" Width="150px">
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Member" FieldName="member" ShowInCustomizationForm="True" VisibleIndex="3" Width="125px">
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Origin" FieldName="origin" ShowInCustomizationForm="True" VisibleIndex="4" Width="125px">
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataMemoColumn Caption="Notes" FieldName="notes" ShowInCustomizationForm="True" VisibleIndex="5">
									<PropertiesMemoEdit Rows="5">
									</PropertiesMemoEdit>
									<DataItemTemplate>
										<div class="notecell">
											<%#Eval("notes") %></div>
									</DataItemTemplate>
								</dx:GridViewDataMemoColumn>
							</Columns>
							<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" />
							<Settings ShowFilterRow="True" ShowTitlePanel="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True"></Settings>
							<Templates>
								<TitlePanel>
									<table align="left" style="width: 100%;" cellpadding="2" cellspacing="0">
										<tr>
											<td>
												<b>Date of Action</b>
											</td>
											<td>
												<b>Action</b>
											</td>
											<td>
												<b>Member</b>
											</td>
											<td>
												<b>Origin</b>
											</td>
											<td>
												<b>Notes</b>
											</td>
											<td>
												&nbsp;
											</td>
										</tr>
										<tr>
											<td valign="top">
												<dx:ASPxDateEdit ID="date_action" runat="server" DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd" AnimationType="None" Width="100px">
												</dx:ASPxDateEdit>
											</td>
											<td valign="top">
												<dx:ASPxComboBox ID="cb_action" runat="server" DataSourceID="ds_historyaction" AnimationType="None" TextField="action" ValueField="id" ValueType="System.Int32" Width="100px">
												</dx:ASPxComboBox>
											</td>
											<td valign="top">
												<dx:ASPxComboBox ID="cb_action_member" runat="server" DataSourceID="ds_historyactionmembers" AnimationType="None" EnableCallbackMode="false" IncrementalFilteringMode="Contains" TextField="name" ValueField="id" ValueType="System.Int32" Width="125px">
												</dx:ASPxComboBox>
											</td>
											<td valign="top">
												<dx:ASPxComboBox ID="cb_origin" runat="server" DataSourceID="ds_origin" AnimationType="None" IncrementalFilteringMode="Contains" TextField="name" ValueField="id" ValueType="System.Int32" Width="125px">
												</dx:ASPxComboBox>
											</td>
											<td valign="top" width="100%">
												<dx:ASPxMemo ID="memo_note" runat="server" BackColor="#FFFFCC" Height="50px" Width="100%">
												</dx:ASPxMemo>
											</td>
											<td valign="top">
												<dx:ASPxButton ID="bt_addaction" runat="server" HorizontalAlign="Center" Text="Add" Width="75px" OnClick="bt_addaction_Click">
													<Image Url="~/images/icon/icon[add].gif">
													</Image>
												</dx:ASPxButton>
											</td>
										</tr>
										<tr>
											<td align="left">
												<dx:ASPxLabel runat="server" ID="lb_error" ForeColor="Red" EncodeHtml="false">
												</dx:ASPxLabel>
											</td>
										</tr>
									</table>
								</TitlePanel>
							</Templates>
						</dx:ASPxGridView>
						<asp:SqlDataSource ID="sql_history" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT
	c.member_fullname member,
	a.customer_history_date date,
	b.history_action_type_action action,
	a.customer_history_notes notes,
	d.name origin
FROM
	customer_history a
INNER JOIN 
	history_action_type b 
		ON a.customer_history_action = b.history_action_type_id 
LEFT JOIN 
	member c 
		ON a.customer_history_memberid = c.member_id 
LEFT JOIN 
	customer_origin d 
		ON a.origin = d.id
WHERE 
	a.customer_history_custid = @customer_id AND
	a.address_id = @address_id
ORDER BY 
	a.customer_history_date desc, a.customer_history_id desc">
							<SelectParameters>
								<asp:ControlParameter ControlID="hdn_customer_id" Name="@customer_id" PropertyName="Value" />
								<asp:ControlParameter ControlID="hdn_address_id" Name="@address_id" PropertyName="Value" />
							</SelectParameters>
						</asp:SqlDataSource>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Stats">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table cellpadding="2" cellspacing="0" style="font-family: Arial" width="100%">
							<tr>
								<td nowrap="nowrap" class="c">
									Days Since Last Invoice:
								</td>
								<td align="left" class="v" id="lb_last_invoice" runat="server">
								</td>
							</tr>
							<tr>
								<td class="c">
									Total FYTD $:
								</td>
								<td align="left" class="v" id="lb_current_fiscal" runat="server">
								</td>
							</tr>
							<tr>
								<td class="c">
									Total LFYTD $:
								</td>
								<td align="left" class="v" id="lb_last_fiscal" runat="server">
								</td>
							</tr>
							<tr>
								<td class="c">
									Company Wide FYTD $:
								</td>
								<td align="left" class="v" id="lb_ytd_companywide" runat="server">
								</td>
							</tr>
							<tr>
								<td class="c">
									Company Wide LFYTD $:
								</td>
								<td align="left" class="v" id="lb_lytd_companywide" runat="server">
								</td>
							</tr>
						</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>
		<Paddings Padding="0px" />
		<tabstyle height="35px"/>
		<ContentStyle>
			<Paddings Padding="2px" />
		</ContentStyle>
	</dx:ASPxPageControl>
	<table width="100%">
		<tr>
			<td width="33%" align="left">
				<dx:ASPxButton runat="server" ID="bt_savesales" Text="Save" OnClick="bt_savesales_Click" Style="height: 25px">
				</dx:ASPxButton>
				<br />
				<dx:ASPxLabel ID="lb_notify" EncodeHtml="false" runat="server" 
					Font-Names="Arial">
				</dx:ASPxLabel>
			</td>
			<td width="33%" align="left">
				<table>
					<tr>
						<td colspan="3" align="left">
							<b>Rate Sheets</b>
						</td>
					</tr>
					<tr>
						<td>
							Select Branch:
						</td>
						<td>
							<dx:ASPxComboBox runat="server" ID="cb_ratesheet_branch" ClientInstanceName="rate_sheet_branch" TextField="name" ValueField="id">
							</dx:ASPxComboBox>
						</td>
						<td>
							<dx:ASPxButton runat="server" ID="bt_ratesheet" AutoPostBack="False" Text="Show Rate Sheet">
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</td>
			<td width="33%" align="right">
				&nbsp;
			</td>
		</tr>
	</table>
	<asp:SqlDataSource ID="ds_origin" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, name FROM customer_origin ORDER BY name" UpdateCommand="UPDATE customer_origin SET name = ?name WHERE id = ?id LIMIT 1"></asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_isr" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 0 member_id,'Please Select a User' member_fullname,0 orderby UNION SELECT a.member_id, CONCAT(b.name, ' - ', member_fullname) member_fullname, 1 orderby FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE member_status = 'Active' and member_membertype_id IN (17,5,11,12,8,25,4,29,46, 59, 60,39,75) ORDER BY orderby, member_fullname"></asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_osr" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 0 member_id,'Please Select a User' member_fullname,0 orderby UNION SELECT a.member_id, CONCAT(b.name, ' - ', member_fullname) member_fullname, 1 orderby FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE member_status = 'Active' and member_membertype_id IN (27) ORDER BY orderby, member_fullname"></asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_am" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Call get_account_managers(@address_id)">
		<SelectParameters>
			<asp:ControlParameter ControlID="hdn_address_id" DefaultValue="0" Name="@address_id" PropertyName="Value" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_ram" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		
		SelectCommand="SELECT 0 member_id,'Please Select a User' member_fullname,0 orderby UNION SELECT a.member_id, CONCAT(b.name, ' - ', member_fullname) member_fullname, 1 orderby FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE member_status = 'Active' and member_membertype_id IN (27,37,41,46,54,59,60) ORDER BY orderby, member_fullname"></asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_pms" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 0 member_id,'Please Select a User' member_fullname,0 orderby UNION SELECT a.member_id, CONCAT(b.name, ' - ', member_fullname) member_fullname, 1 orderby FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE member_status = 'Active' and member_membertype_id IN (17,5,11,12,8,25,4,29,46,34,49,36,38,39,75) ORDER BY orderby, member_fullname"></asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_contacts" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT contact_id,contact_name FROM contact WHERE contact_type='Customer' AND contact_cust_id = @customer_id AND contact_status = 'Active' AND address_id = @address_id UNION select 0,'Unknown' order by contact_name">
		<SelectParameters>
			<asp:ControlParameter ControlID="hdn_customer_id" DefaultValue="0" Name="@customer_id" PropertyName="Value" />
			<asp:ControlParameter ControlID="hdn_address_id" DefaultValue="0" Name="@address_id" PropertyName="Value" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_historyaction" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT history_action_type_id id,history_action_type_action action FROM history_action_type WHERE history_action_type_action NOT LIKE '%edit' ORDER BY action"></asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_historyactionmembers" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT member_id id, member_fullname name FROM member WHERE member_status = 'Active' ORDER BY member_fullname"></asp:SqlDataSource>

				<dx:ASPxPopupControl runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" Modal="True" AllowDragging="True" ClientInstanceName="pop_origins" HeaderText="Origins" Width="250px" ID="pop_origins">
<ClientSideEvents Closing="function(s, e) {
	combo_lastorigin.PerformCallback();
	combo_found.PerformCallback();
}"></ClientSideEvents>

<ModalBackgroundStyle BackColor="Transparent"></ModalBackgroundStyle>
<ContentCollection>
<dx:popupcontrolcontentcontrol runat="server" SupportsDisabledAttribute="True">
							<dx:aspxgridview runat="server" ClientInstanceName="gv_origins" KeyFieldName="id" AutoGenerateColumns="False" DataSourceID="ds_origin" Width="100%" ID="gv_origins0">
                                <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
                                <Columns>
<dx:gridviewcommandcolumn ShowInCustomizationForm="True" Width="20px" VisibleIndex="0" ShowEditButton="true" >

</dx:gridviewcommandcolumn>
<dx:gridviewdatatextcolumn FieldName="id" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
<EditFormSettings Visible="False"></EditFormSettings>
</dx:gridviewdatatextcolumn>
<dx:gridviewdatatextcolumn FieldName="name" ShowInCustomizationForm="True" Caption="Origin" VisibleIndex="2"></dx:gridviewdatatextcolumn>
</Columns>
</dx:aspxgridview>

							<br />
							<dx:aspxtextbox runat="server" Width="170px" ClientInstanceName="tb_originname" ID="tb_originname0"></dx:aspxtextbox>

							<dx:aspxbutton runat="server" AutoPostBack="False" ClientInstanceName="bt_saveorigin" Text="Save " ID="bt_saveorigin0">
<ClientSideEvents Click="function(s, e) {
	if(tb_originname.GetText() != &quot;&quot;)
		{
		cb_saveorigin.PerformCallback(tb_originname.GetText());
		}
}"></ClientSideEvents>
</dx:aspxbutton>

							<dx:aspxcallback runat="server" ClientInstanceName="cb_saveorigin" ID="cb_saveorigin" OnCallback="cb_saveorigin_Callback">
<ClientSideEvents CallbackComplete="function(s, e) {
	if(e.result == &quot;SUCCESS&quot;)
		{
		tb_originname.SetText(&quot;&quot;);
		gv_origins.Refresh();
		tb_originname.Focus();
		}
	else
		{
		alert(&quot;Error Saving Origin\nError Returned:\n&quot;+e.result);
		tb_originname.Focus();
		}
}" BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" EndCallback="function(s, e) {
	please_wait(&quot;stop&quot;);
}"></ClientSideEvents>
</dx:aspxcallback>

						</dx:popupcontrolcontentcontrol>
</ContentCollection>
</dx:ASPxPopupControl>

</div>
<asp:HiddenField ID="hdn_address_id" runat="server" />
<asp:HiddenField ID="hdn_customer_id" runat="server" />
