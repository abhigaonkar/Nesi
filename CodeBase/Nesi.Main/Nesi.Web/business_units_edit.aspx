<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="business_units_edit"  Codebehind="business_units_edit.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/modules/business_unit_fvr_settings.ascx" TagPrefix="uc" TagName="business_unit_fvr_settings" %>




<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<asp:UpdatePanel ID="up_company" runat="server">
		<ContentTemplate>
			<script type="text/javascript">
				function PushRates(data)
				{
					editVisibleIndex = data;
					//    var collection = ASPxClientControl.GetControlCollection();
					//   collection.Get("ASPxGridView2" + data + "_
					//    var editor2 = document.getElementById('ASPxGridView2_ef' + editVisibleIndex + '_txtOT');
					//    editor2.SetValue(editor1.GetValue() * 1.5);

				}
				var check = {
					dist: function ()
					{
						var i = PageMethods.dist_exists($(".hid_business_unit_id").val(), trackbar.GetPositionStart(), trackbar.GetPositionEnd(), gv_dist.cp_editid, check.complete, check.error, check.timeout);
					},
					complete: function (arg)
					{
						lb_range_min.SetText("$" + trackbar.GetPositionStart());
						lb_range_max.SetText("$" + trackbar.GetPositionEnd());
						if (arg == 0)
						{
							lb_error.SetText("");
						}
						else
						{
							lb_error.SetText("A distribution already exists in this range.");
						}
						check.verify();
					},
					timeout: function (arg)
					{
						alert("Timeout occured");
					},
					error: function (arg)
					{
						alert("Error has occured: " + arg._message);
					},
					verify: function ()
					{
						var is_enabled = true;
						if (combo_manager.GetValue() == null)
						{
							is_enabled = false;
						}
						if (trackbar.GetPositionStart() == trackbar.GetPositionEnd())
						{
							is_enabled = false;
						}
						if (lb_error.GetText() != "")
						{
							is_enabled = false;
						}
						bt_save.SetEnabled(is_enabled);
						return is_enabled;
					}
				}
				var save = {
					dist: function ()
					{
						if (!check.verify())
						{
							alert("Can't save");
							return false;
						}
						PageMethods.dist_save($(".hid_business_unit_id").val(), trackbar.GetPositionStart(), trackbar.GetPositionEnd(), combo_manager.GetValue(), gv_dist.cp_editid, save.complete, save.error, save.timeout);
					},
					complete: function (arg)
					{
						gv_dist.CancelEdit();
						setTimeout("gv_dist.Refresh()", 500);
					},
					timeout: function (arg)
					{
						alert("Timeout occured");
					},
					error: function (arg)
					{
						alert("Error has occured: " + arg._message);
					}
				}
				$(document).ready(function ()
				{
					page_obj.update_panel_progress.bind();
				});
			</script>

			<input type="hidden" id="hid_business_unit_id" class="hid_business_unit_id" runat="server" />
			<div id="divSearchResults" runat="server">
				Business Unit Details
    <br />
			</div>

			<asp:Label runat="server" ID="c_label" ForeColor="Red"></asp:Label>
			<asp:ScriptManager ID="sm" runat="server" EnablePageMethods="True">
			</asp:ScriptManager>
			<asp:HiddenField ID="hdnid" runat="server" />
			<br />
			<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
				Width="100%" AutoPostBack="True"
				OnActiveTabChanged="ASPxPageControl1_ActiveTabChanged" Theme="NETheme01">
				<TabPages>
					<dx:TabPage Text="Business Unit Info" Name="company_info" Visible="True">
						<ContentCollection>
							<dx:ContentControl runat="server">
								<br />
								<asp:Button ID="c_button" runat="server" Text="Save" />
								<br />
								<br />
								<asp:PlaceHolder ID="c_PlaceHolder" runat="server"></asp:PlaceHolder>
								<br />
								<br />
								<br />
								<pre id="div_sql" runat="server"></pre>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="Chargeout Rates" Name="chargeout_rates">
						<ContentCollection>
							<dx:ContentControl runat="server">
								<dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
									<PanelCollection>
										<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
											<table style="width: 100%;">
												<tr>
													<td>
														<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Membertype Name"
															Visible="False" Theme="NETheme01">
														</dx:ASPxLabel>
													</td>
													<td>
														<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="150px" Visible="False"
															Theme="NETheme01">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Base Chargeout"
															Visible="False" Theme="NETheme01">
														</dx:ASPxLabel>
													</td>
													<td>
														<dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Width="50px" Visible="False"
															Theme="NETheme01">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxButton ID="btnadd" runat="server" OnClick="btnadd_Click"
															Text="Add New Membertype" ClientEnabled="False" Visible="False" Theme="NETheme01">
														</dx:ASPxButton>
													</td>
													<td>
                                                        <div style="float: right;">
                                                            <dx:ASPxButton ID="btnprint_rates" runat="server"
                                                                OnClick="btnprint_rates_Click" Text="Print Rate Sheet" Width="100px" Theme="NETheme01">
                                                                <Image Url="~/images/icon/icon[print].gif">
                                                                </Image>
                                                            </dx:ASPxButton>
                                                        </div>
													</td>
												</tr>
                                                <tr>
                                                    <table style="width: 100%;" runat="server" id="batchUpdatePanel">
                                                        <tr>
                                                            <td width="150">
                                                                <dx:ASPxButton ID="btnupdate" runat="server" OnClick="btnupdate_Click"
                                                                    Text="Reset chargeouts using this business unit -&gt;" Width="150px" Theme="NETheme01"
                                                                    ClientEnabled="True" UseSubmitBehavior="False">
                                                                    <ClientSideEvents Click="function(s,e) { if(ddl_chargeout_branch.GetValue() == null){e.processOnServer = false;alert('Please select a business unit to use as a template');} else { e.processOnServer = confirm('Are you sure you want to overwrite all chargeouts?'); }}" />

                                                                </dx:ASPxButton>
                                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                                                    ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                                                                    ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
                                                            </td>
                                                            <td align="left">
                                                                <dx:ASPxComboBox ID="ASPxComboBox3" runat="server" ClientInstanceName="ddl_chargeout_branch"
                                                                    DataSourceID="SqlDataSource1" TextField="name" ValueField="id"
                                                                    ValueType="System.Int32" Width="150px" Theme="NETheme01">
                                                                </dx:ASPxComboBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">By pressing this button, the current chargeout rates from the business unit being edited will be deleted, and then the rates from the selected business unit will be copied into this business unit.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </tr>
											</table>
										</dx:PanelContent>
									</PanelCollection>
								</dx:ASPxPanel>
								<br />
								<dx:ASPxGridView runat="server" ClientInstanceName="GV2" KeyFieldName="ID"
									AutoGenerateColumns="False" ID="ASPxGridView2"
									OnRowCommand="ASPxGridView2_RowCommand"
									OnHtmlDataCellPrepared="ASPxGridView2_HtmlDataCellPrepared"
									OnHtmlRowCreated="ASPxGridView2_HtmlRowCreated"
									Theme="NETheme01">
									<Columns>
										<dx:GridViewDataTextColumn Caption=" " VisibleIndex="7">
											<DataItemTemplate>
												<dx:ASPxButton BackColor="Transparent" CommandName="Save" ID="btnSaveRate" runat="server" Text="Save Base and Update" Wrap="False">
													<Image Url="~/images/icon/icon[save].gif">
													</Image>
													<Border BorderStyle="None"></Border>
												</dx:ASPxButton>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Member Type" FieldName="MemberType_Name" ReadOnly="True"
											VisibleIndex="0" Width="200px">
											<PropertiesTextEdit>
												<ReadOnlyStyle ForeColor="Gray">
												</ReadOnlyStyle>
											</PropertiesTextEdit>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Reg" FieldName="Reg" ReadOnly="True" ToolTip="Base Chargeout Rate"
											VisibleIndex="1" Width="45px">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="txtReg" runat="server" ClientInstanceName="txtReg" Text='<%# Eval("Reg", "{0:F}") %>'
													Width="45px" Theme="NETheme01">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="DT SP" FieldName="DTSP" ReadOnly="True" ToolTip="Double Time Shift Premium"
											VisibleIndex="6" Width="45px">
											<PropertiesTextEdit DisplayFormatString="{0:F}">
											</PropertiesTextEdit>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="OT SP" FieldName="OTSP" ToolTip="Overtime Shift Premium"
											VisibleIndex="5" Width="45px">
											<PropertiesTextEdit DisplayFormatString="{0:F}">
											</PropertiesTextEdit>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="SP" FieldName="SP" ReadOnly="True" ToolTip="Regular Shift Premium"
											VisibleIndex="4" Width="45px">
											<PropertiesTextEdit DisplayFormatString="{0:F}">
											</PropertiesTextEdit>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="DT" FieldName="DT" ReadOnly="True" ToolTip="Doubletime"
											VisibleIndex="3" Width="45px">
											<PropertiesTextEdit DisplayFormatString="{0:F}">
											</PropertiesTextEdit>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="OT" FieldName="OT" ReadOnly="True" ToolTip="Overtime"
											VisibleIndex="2" Width="45px">
											<PropertiesTextEdit DisplayFormatString="{0:F}">
												<ReadOnlyStyle ForeColor="#E0E0E0">
												</ReadOnlyStyle>
											</PropertiesTextEdit>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="ID" Visible="False" VisibleIndex="8">
										</dx:GridViewDataTextColumn>
										<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" ShowClearFilterButton="true"  
											VisibleIndex="9">
											
											<CustomButtons>
												<dx:GridViewCommandColumnCustomButton ID="chart">
												</dx:GridViewCommandColumnCustomButton>
											</CustomButtons>
										</dx:GridViewCommandColumn>
									</Columns>

									<SettingsBehavior AllowDragDrop="False" AllowSort="False" EnableRowHotTrack="True"></SettingsBehavior>

									<SettingsPager Mode="ShowAllRecords"></SettingsPager>

									<SettingsEditing Mode="Inline"></SettingsEditing>

									<Settings ShowGroupButtons="False"></Settings>

								</dx:ASPxGridView>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Name="chargeout_history" Text="Chargeout History">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
								<dx:ASPxGridView ID="gv_chargeout_history" runat="server"
									AutoGenerateColumns="False" DataSourceID="sds_chargeout_history"
									Theme="NETheme01">
									<Columns>
										<dx:GridViewDataDateColumn Caption="Date of Change" FieldName="change_date" ShowInCustomizationForm="True" VisibleIndex="0" Width="100px">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataDateColumn>
										<dx:GridViewDataTextColumn Caption="Who Changed" FieldName="name" ShowInCustomizationForm="True" VisibleIndex="1" Width="250px">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Membertype" FieldName="membertype" ShowInCustomizationForm="True" VisibleIndex="2" Width="350px">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Rate" FieldName="rate" ShowInCustomizationForm="True" VisibleIndex="3" Width="100px">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsPager PageSize="50">
									</SettingsPager>
									<Styles>
										<Header HorizontalAlign="Center">
										</Header>
									</Styles>
								</dx:ASPxGridView>
								<asp:SqlDataSource ID="sds_chargeout_history" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.membertype_chargeout_history_date change_date, c.member_fullname name, b.membertype_name membertype, a.membertype_chargeout_history_rate rate  FROM membertype_chargeout_history a LEFT JOIN membertype b ON a.membertype_chargeout_history_membertype_id = b.membertype_id LEFT JOIN member c ON a.membertype_chargeout_history_memberid = c.Member_ID  WHERE a.business_unit_id = @business_unit_id ORDER BY a.membertype_chargeout_history_date, b.membertype_name">
									<SelectParameters>
										<asp:QueryStringParameter DefaultValue="0" Name="@business_unit_id" QueryStringField="id" />
									</SelectParameters>
								</asp:SqlDataSource>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="PO Approval Settings" Name="options">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
								<dx:ASPxPanel ID="panel_poapproval" runat="server" Width="100%">
									<PanelCollection>
										<dx:PanelContent runat="server" SupportsDisabledAttribute="True">

											<dx:ASPxGridView ID="gv_dist" runat="server" AutoGenerateColumns="False"
												DataSourceID="ds_dist" EnableTheming="True" KeyFieldName="id" Theme="NETheme01"
												ClientInstanceName="gv_dist" 
												OnCustomJSProperties="gv_dist_CustomJSProperties" Width="100%" SettingsPager-Mode="ShowAllRecords">
                                                <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
												<Columns>
													<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
														<EditFormSettings Visible="False" />
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Amount from" FieldName="amount_from" Visible="False" ShowInCustomizationForm="True" VisibleIndex="2" Width="100px">
														<PropertiesTextEdit DisplayFormatString="C2">
														</PropertiesTextEdit>
														<HeaderStyle HorizontalAlign="Center" />
														<CellStyle HorizontalAlign="Center">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Amount To" FieldName="amount_to"
														ShowInCustomizationForm="True" VisibleIndex="3" Width="100px">
														<PropertiesTextEdit DisplayFormatString="C2">
														</PropertiesTextEdit>
														<HeaderStyle HorizontalAlign="Center" />
														<CellStyle HorizontalAlign="Center">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewCommandColumn ShowInCustomizationForm="True" VisibleIndex="0"
														ButtonType="Image" Width="50px"
														ShowNewButtonInHeader="True" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true" >
														
														
														
														<HeaderStyle BackColor="White" />
													</dx:GridViewCommandColumn>
													<dx:GridViewDataComboBoxColumn Caption="Can Be Approved By" FieldName="member_id"
														ShowInCustomizationForm="True" VisibleIndex="4" PropertiesComboBox-DataSourceID="sql_pm" PropertiesComboBox-ValueType="System.Int32" PropertiesComboBox-ValueField="member_id" PropertiesComboBox-TextField="member_fullname" EditFormSettings-Visible="Default">

														<PropertiesComboBox DataSourceID="sql_pm" TextField="name"
															ValueField="member_id" ValueType="System.Int32">
														</PropertiesComboBox>

														<HeaderStyle HorizontalAlign="Center" />
														<CellStyle HorizontalAlign="Left">
														</CellStyle>
													</dx:GridViewDataComboBoxColumn>
												</Columns>
												<SettingsBehavior EnableRowHotTrack="True" ConfirmDelete="True" />
												<SettingsEditing Mode="Batch">
												</SettingsEditing>
												<Settings ShowTitlePanel="True" />
												<SettingsText Title="PO Approval Amounts" />
												
                                                <SettingsCommandButton>
                                                    <NewButton>
                                                        <Image Url="~/images/icon/icon[add].gif">
														</Image>
                                                    </NewButton>
                                                </SettingsCommandButton>

												<Templates>

													<EditForm>
														<table cellpadding="5" style="width: 100%;">
															<tr>
																<td width="400" colspan="3">All purchase order approval requests between the amounts of:</td>
															</tr>
															<tr>
																<td rowspan="3">
																	<strong>
																		<dx:ASPxTrackBar ID="trackbar" runat="server" AllowRangeSelection="True" LargeTickEndValue="2000" LargeTickInterval="500" MaxValue="2000" Position="0" ScalePosition="LeftOrTop" SmallTickFrequency="250" Step="100" TextField="tb_range" Theme="Default" ValueType="System.Int16" Width="400px" ClientInstanceName="trackbar" PositionStart='0'>
																			<ClientSideEvents PositionChanged="function(s, e) {
			check.dist();
}" />
																		</dx:ASPxTrackBar>
																	</strong>
																</td>
																<td colspan="2">
																	<dx:ASPxLabel ID="lb_range_min" runat="server" ClientInstanceName="lb_range_min" Font-Bold="True">
																	</dx:ASPxLabel>
																</td>
															</tr>
															<tr>
																<td colspan="2">and</td>
															</tr>
															<tr>
																<td colspan="2">
																	<dx:ASPxLabel ID="lb_range_max" runat="server" ClientInstanceName="lb_range_max" Font-Bold="True">
																	</dx:ASPxLabel>
																</td>
															</tr>
															<tr>
																<td>Should be directed to:</td>
																<td colspan="2">
																	<dx:ASPxLabel ID="lb_error" runat="server" ClientInstanceName="lb_error" ForeColor="Red" Width="250px">
																	</dx:ASPxLabel>
																</td>
															</tr>
															<tr>
																<td>
																	<dx:ASPxComboBox ID="combo_manager" ClientInstanceName="combo_manager" runat="server" DataSourceID="ds_managers" TextField="name" TextFormatString="{1}" ValueField="id" ValueType="System.Int32" Value='<%# Bind("member_id") %>'>
																		<ClientSideEvents SelectedIndexChanged="function(s, e) {
	check.verify();
}" />
																		<Columns>
																			<dx:ListBoxColumn Caption="Branch" FieldName="branch" />
																			<dx:ListBoxColumn Caption="User" FieldName="name" />
																		</Columns>
																	</dx:ASPxComboBox>
																	<asp:SqlDataSource ID="ds_managers" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT member_id id, member_name(member_id) name, name( business_unit_id) branch FROM member WHERE member_membertype_id IN (4,5,7,9,28,29,38) AND member_status = 'Active' AND  business_unit_id != 8 ORDER BY  business_unit_id, member_lastname, member_nickname"></asp:SqlDataSource>
																</td>
																<td align="center" valign="middle">
																	<dx:ASPxButton ID="bt_save" runat="server" ClientEnabled="False" Text="Save" Theme="Default" AutoPostBack="False" ClientInstanceName="bt_save">
																		<ClientSideEvents Click="function(s, e) {
	save.dist();
}" />
																	</dx:ASPxButton>
																</td>
																<td align="center" valign="middle">
																	<dx:ASPxButton ID="bt_cancel" runat="server" AutoPostBack="False" Text="Cancel" Theme="Default">
																		<ClientSideEvents Click="function(s, e) {
	gv_dist.CancelEdit();
}" />
																	</dx:ASPxButton>
																</td>
															</tr>
														</table>
													</EditForm>
												</Templates>
											</dx:ASPxGridView>
											<asp:SqlDataSource ID="ds_dist" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT id, amount_from, amount_to, member_id from business_unit_po_dist WHERE  business_unit_id = @business_unit_id ORDER BY amount_from
"
												DeleteCommand="DELETE from business_unit_po_dist WHERE id = ?id LIMIT 1"
												UpdateCommand="Update business_unit_po_dist set [business_unit_id]=?, [member_id] = ?, [amount_to] = ? where [id] = ?"
												InsertCommand="Insert into business_unit_po_dist ([business_unit_id],[member_id],[amount_to]) values (?,?,?)">
												<DeleteParameters>
													<asp:Parameter Name="id" />
												</DeleteParameters>
												<SelectParameters>
													<asp:QueryStringParameter Name="@business_unit_id" QueryStringField="id" />
												</SelectParameters>
												<UpdateParameters>
													<asp:QueryStringParameter Name="business_unit_id" QueryStringField="id" />
													<asp:Parameter Name="member_id" Type="Int32" />
													<asp:Parameter Name="amount_to" Type="Int32" />
													<asp:Parameter Name="id" Type="Int32" />
												</UpdateParameters>
												<InsertParameters>
													<asp:QueryStringParameter Name="business_unit_id" QueryStringField="id" />
													<asp:Parameter Name="member_id" Type="Int32" />
													<asp:Parameter Name="amount_to" Type="Int32" />
													<asp:Parameter Name="id" Type="Int32" />
												</InsertParameters>
											</asp:SqlDataSource>
											<asp:SqlDataSource ID="sql_pm" runat="server"
												ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
												ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
												SelectCommand="
SELECT 
	a.member_id,
	CONCAT(c.ddl_name,' - ',a.member_fullname) name
FROM member a 
INNER JOIN 
	memberpageprivilege b ON a.member_id = b.memberpageprivilege_member_id AND b.memberpageprivilege_privilege_id = 86 
LEFT JOIN 
	business_unit c ON a.business_unit_id = c.id 
WHERE 
	FIND_IN_SET(c.id, ?cid)
ORDER BY 
	c.ddl_name, a.member_nickname, a.member_lastname">
												<SelectParameters>
													<asp:Parameter Name="cid" />
												</SelectParameters>
											</asp:SqlDataSource>
											<br />
										</dx:PanelContent>
									</PanelCollection>
								</dx:ASPxPanel>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="Budgets" Name="budgets" ClientVisible="False">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
								<table style="width: 100%;">
									<tr>
										<td>
											<dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Department"
												Theme="NETheme01">
											</dx:ASPxLabel>
										</td>
										<td>
											<dx:ASPxComboBox ID="ASPxComboBox5" runat="server" Theme="NETheme01">
											</dx:ASPxComboBox>
										</td>
										<td>&nbsp;</td>
									</tr>
									<tr>
										<td>
											<dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="GL Account"
												Theme="NETheme01">
											</dx:ASPxLabel>
										</td>
										<td>
											<dx:ASPxComboBox ID="ASPxComboBox4" runat="server" Theme="NETheme01">
											</dx:ASPxComboBox>
										</td>
										<td>&nbsp;</td>
									</tr>
									<tr>
										<td>&nbsp;</td>
										<td>&nbsp;</td>
										<td>&nbsp;</td>
									</tr>
									<tr>
										<td>&nbsp;</td>
										<td>&nbsp;</td>
										<td>&nbsp;</td>
									</tr>
								</table>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="Targets" Name="targets">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
								<asp:SqlDataSource ID="SqlDataSource2" runat="server"
									ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
									ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
									SelectCommand="Select *,(m1+m2+m3+m4+m5+m6+m7+m8+m9+m10+m11+m12) _total from fytarget where  business_unit_id = ?cid and fy = ?yid">
									<SelectParameters>
										<asp:ControlParameter ControlID="hdnid" Name="cid" PropertyName="Value" />
										<asp:ControlParameter ControlID="spnfy1" Name="yid" PropertyName="Number" />
									</SelectParameters>
								</asp:SqlDataSource>
								<asp:SqlDataSource ID="SqlDataSource4" runat="server"
									ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
									ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
									SelectCommand="Select *, get_name(fytarget_employees.member_id) member_name,(m1+m2+m3+m4+m5+m6+m7+m8+m9+m10+m11+m12) _total  from fytarget_employees where business_unit_id = ?cid and fy = ?yid order by get_name(fytarget_employees.member_id)">
									<SelectParameters>
										<asp:ControlParameter ControlID="hdnid" Name="cid" PropertyName="Value" />
										<asp:ControlParameter ControlID="spnfy1" Name="yid" PropertyName="Number" />
									</SelectParameters>
								</asp:SqlDataSource>
								<table style="width: 100%;">
									<tr>
										<td nowrap="nowrap"
											style="font-family: Arial; font-size: large; font-weight: 700">Fiscal Year Ending:
										</td>
										<td nowrap="nowrap">
											<dx:ASPxSpinEdit ID="spnfy1" runat="server" AutoPostBack="True"
												ClientInstanceName="spnfy1" MaxValue="2099" MinValue="2013" NumberType="Integer" OnValueChanged="spnfy1_ValueChanged"
												Width="100px" Font-Bold="True" Font-Names="Arial" Font-Size="16pt">
											</dx:ASPxSpinEdit>
										</td>
										<td align="right" nowrap="nowrap" style="white-space: nowrap" width="100%">
											<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False"
												Text="Refresh" UseSubmitBehavior="False">
												<ClientSideEvents Click="function(s, e) {
	gv_targets.PerformCallback('z');
gv_targets0.PerformCallback('z');
}" />
											</dx:ASPxButton>
										</td>
									</tr>
									<tr>
										<td>&nbsp;</td>
										<td>&nbsp;</td>
										<td>&nbsp;</td>
									</tr>
									<tr>
										<td>&nbsp;</td>
										<td>&nbsp;</td>
										<td>&nbsp;</td>
									</tr>
								</table>

								<dx:ASPxGridView ID="gv_targets" runat="server" AutoGenerateColumns="False"
									DataSourceID="SqlDataSource2" OnCustomCallback="gv_targets_CustomCallback"
									Width="100%" ClientInstanceName="gv_targets" Font-Names="Arial" KeyFieldName="id"
									OnDataBound="gv_targets_PreRender" OnPreRender="gv_targets_PreRender"
									OnRowDeleting="gv_targets_RowDeleting" OnHtmlDataCellPrepared="gv_targets_HtmlDataCellPrepared"
									OnHtmlRowPrepared="gv_targets_HtmlRowPrepared">
                                    <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
									<Columns>
										<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" ShowDeleteButton="true" ShowClearFilterButton="true"  
											VisibleIndex="0" ButtonType="Image">
											
											
										</dx:GridViewCommandColumn>
										<dx:GridViewDataTextColumn Caption="Target" FieldName="target_name"
											ShowInCustomizationForm="True" VisibleIndex="1" Width="100px">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="FY" FieldName="fy"
											ShowInCustomizationForm="True" VisibleIndex="2" Width="75px">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Budget ID" FieldName="budget_detailid"
											ShowInCustomizationForm="True" VisibleIndex="3" Visible="False">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m1" FieldName="m1"
											ShowInCustomizationForm="True" VisibleIndex="4">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox3" runat="server" OnInit="ASPxTextBox3_Init"
													Width="50px" Text='<%# Eval("m1") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m2" FieldName="m2"
											ShowInCustomizationForm="True" VisibleIndex="5">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox4" runat="server" OnInit="ASPxTextBox3_Init"
													Width="60px" Text='<%# Eval("m2") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m3" FieldName="m3"
											ShowInCustomizationForm="True" VisibleIndex="6">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox5" runat="server" OnInit="ASPxTextBox3_Init"
													Width="60px" Text='<%# Eval("m3") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m4" FieldName="m4"
											ShowInCustomizationForm="True" VisibleIndex="7">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox6" runat="server" OnInit="ASPxTextBox3_Init"
													Width="60px" Text='<%# Eval("m4") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m5" FieldName="m5"
											ShowInCustomizationForm="True" VisibleIndex="8">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox7" runat="server" OnInit="ASPxTextBox3_Init"
													Width="60px" Text='<%# Eval("m5") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m6" FieldName="m6"
											ShowInCustomizationForm="True" VisibleIndex="9">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox8" runat="server" OnInit="ASPxTextBox3_Init"
													Width="60px" Text='<%# Eval("m6") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m7" FieldName="m7"
											ShowInCustomizationForm="True" VisibleIndex="10">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox9" runat="server" OnInit="ASPxTextBox3_Init"
													Width="60px" Text='<%# Eval("m7") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m8" FieldName="m8"
											ShowInCustomizationForm="True" VisibleIndex="11">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox10" runat="server" OnInit="ASPxTextBox3_Init"
													Width="60px" Text='<%# Eval("m8") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m9" FieldName="m9"
											ShowInCustomizationForm="True" VisibleIndex="12">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox11" runat="server" OnInit="ASPxTextBox3_Init"
													Width="60px" Text='<%# Eval("m9") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m10" FieldName="m10"
											ShowInCustomizationForm="True" VisibleIndex="13">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox12" runat="server" OnInit="ASPxTextBox3_Init"
													Width="60px" Text='<%# Eval("m10") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m11" FieldName="m11"
											ShowInCustomizationForm="True" VisibleIndex="14">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox13" runat="server" OnInit="ASPxTextBox3_Init"
													Width="60px" Text='<%# Eval("m11") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m12" FieldName="m12"
											ShowInCustomizationForm="True" VisibleIndex="15">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox14" runat="server" OnInit="ASPxTextBox3_Init"
													Width="60px" Text='<%# Eval("m12") %>'>
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="id" ShowInCustomizationForm="True"
											Visible="False" VisibleIndex="17">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Total" FieldName="_total"
											ShowInCustomizationForm="True" VisibleIndex="16">
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsBehavior ConfirmDelete="True" />
									<Settings ShowFilterRow="True" ShowTitlePanel="True"
										ShowHeaderFilterButton="True" />
									<Styles>
										<TitlePanel BackColor="#0066FF">
										</TitlePanel>
									</Styles>
									<Templates>
										<TitlePanel>
											<table style="width: 100%;">
												<tr>
													<td align="left" class="style5" colspan="3">
														<strong>Branch Targets</strong></td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
												</tr>
												<tr>
													<td>Target Name</td>
													<td>FY</td>
													<td align="center">
														<dx:ASPxLabel ID="lb1" runat="server" ClientInstanceName="lb1"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb2" runat="server" ClientInstanceName="lb2"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb3" runat="server" ClientInstanceName="lb3"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb4" runat="server" ClientInstanceName="lb4"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb5" runat="server" ClientInstanceName="lb5"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb6" runat="server" ClientInstanceName="lb6"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb7" runat="server" ClientInstanceName="lb7"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb8" runat="server" ClientInstanceName="lb8"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb9" runat="server" ClientInstanceName="lb9"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb10" runat="server" ClientInstanceName="lb10"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb11" runat="server" ClientInstanceName="lb11"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb12" runat="server" ClientInstanceName="lb12"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td>&nbsp;</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxComboBox ID="ddltarget" runat="server" ClientInstanceName="ddltarget"
															SelectedIndex="0" Width="120px">
															<ClientSideEvents SelectedIndexChanged="function(s, e) {
	gv_targets.PerformCallback('z');
}" />
															<Items>
																<dx:ListEditItem Text="Revenue" Value="Revenue" Selected="True" />
																<dx:ListEditItem Text="Margin" Value="Margin" />
																<dx:ListEditItem Text="Days to Invoice" Value="Days to Invoice" />
															</Items>
														</dx:ASPxComboBox>
													</td>
													<td>
														<dx:ASPxSpinEdit ID="spnfy" runat="server" ClientInstanceName="spnfy"
															Height="21px" MinValue="2013" Width="75px" MaxValue="2099" NumberType="Integer" OnInit="spnfy_Init">
															<ClientSideEvents NumberChanged="function(s, e) {
	gv_targets.PerformCallback('z');
}" />
														</dx:ASPxSpinEdit>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb1" runat="server" ClientInstanceName="tb1" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb2" runat="server" ClientInstanceName="tb2" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb3" runat="server" ClientInstanceName="tb3" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb4" runat="server" ClientInstanceName="tb4" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb5" runat="server" ClientInstanceName="tb5" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb6" runat="server" ClientInstanceName="tb6" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb7" runat="server" ClientInstanceName="tb7" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb8" runat="server" ClientInstanceName="tb8" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb9" runat="server" ClientInstanceName="tb9" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb10" runat="server" ClientInstanceName="tb10" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb11" runat="server" ClientInstanceName="tb11" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb12" runat="server" ClientInstanceName="tb12" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxButton ID="btnadd" runat="server" Text="Add" AutoPostBack="False"
															ClientInstanceName="btnadd">
															<ClientSideEvents Click="function(s, e) {
	gv_targets.PerformCallback('a');
}" />
														</dx:ASPxButton>
													</td>
												</tr>
												<tr>
													<td colspan="2" class="style6" style="font-size: 9pt; font-family: Arial"
														bgcolor="#99CCFF">Total Current Target Shortfall</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t1" runat="server" ClientInstanceName="t1">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t2" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t3" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t4" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t5" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t6" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t7" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t8" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t9" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t10" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t11" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t12" runat="server">
														</dx:ASPxLabel>
													</td>
													<td bgcolor="#99CCFF">
														<dx:ASPxLabel ID="t0total" runat="server">
														</dx:ASPxLabel>
													</td>
												</tr>
											</table>
										</TitlePanel>
									</Templates>
								</dx:ASPxGridView>
								<br />
								<dx:ASPxGridView ID="gv_targets0" runat="server" AutoGenerateColumns="False"
									ClientInstanceName="gv_targets0" DataSourceID="SqlDataSource4"
									Font-Names="Arial" KeyFieldName="id"
									OnCustomCallback="gv_targets0_CustomCallback"
									OnDataBound="gv_targets0_PreRender" OnPreRender="gv_targets0_PreRender"
									OnRowDeleting="gv_targets0_RowDeleting" Width="100%"
									OnHtmlDataCellPrepared="gv_targets0_HtmlDataCellPrepared"
									OnHtmlRowPrepared="gv_targets0_HtmlRowPrepared">
                                    <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
									<Columns>
										<dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowDeleteButton="true" ShowClearFilterButton="true"  
											ShowInCustomizationForm="True" VisibleIndex="0">
											
											
										</dx:GridViewCommandColumn>
										<dx:GridViewDataTextColumn Caption="Target" FieldName="target_name"
											ShowInCustomizationForm="True" VisibleIndex="2" Width="100px">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="FY" FieldName="fy"
											ShowInCustomizationForm="True" VisibleIndex="3" Width="75px">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Budget ID" FieldName="budget_detailid"
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="4">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m1" FieldName="m1"
											ShowInCustomizationForm="True" VisibleIndex="5">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox15" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m1") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m2" FieldName="m2"
											ShowInCustomizationForm="True" VisibleIndex="6">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox16" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m2") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m3" FieldName="m3"
											ShowInCustomizationForm="True" VisibleIndex="7">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox17" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m3") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m4" FieldName="m4"
											ShowInCustomizationForm="True" VisibleIndex="8">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox18" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m4") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m5" FieldName="m5"
											ShowInCustomizationForm="True" VisibleIndex="9">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox19" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m5") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m6" FieldName="m6"
											ShowInCustomizationForm="True" VisibleIndex="10">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox20" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m6") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m7" FieldName="m7"
											ShowInCustomizationForm="True" VisibleIndex="11">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox21" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m7") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m8" FieldName="m8"
											ShowInCustomizationForm="True" VisibleIndex="12">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox22" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m8") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m9" FieldName="m9"
											ShowInCustomizationForm="True" VisibleIndex="13">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox23" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m9") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m10" FieldName="m10"
											ShowInCustomizationForm="True" VisibleIndex="14">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox24" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m10") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m11" FieldName="m11"
											ShowInCustomizationForm="True" VisibleIndex="15">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox25" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m11") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="m12" FieldName="m12"
											ShowInCustomizationForm="True" VisibleIndex="17">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="ASPxTextBox26" runat="server" OnInit="ASPxTextBox03_Init"
													Text='<%# Eval("m12") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="id" ShowInCustomizationForm="True"
											Visible="False" VisibleIndex="16">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Employee" FieldName="member_name"
											ShowInCustomizationForm="True" VisibleIndex="1">
											<CellStyle Wrap="False">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Total" FieldName="_total"
											ShowInCustomizationForm="True" VisibleIndex="18">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="member_id" FieldName="member_id"
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="20">
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsBehavior ConfirmDelete="True" />
									<SettingsPager Mode="ShowAllRecords">
									</SettingsPager>
									<Settings ShowFilterRow="True" ShowTitlePanel="True" ShowFilterBar="Visible"
										ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
									<Styles>
										<TitlePanel BackColor="#00CC00">
										</TitlePanel>
									</Styles>
									<Templates>
										<TitlePanel>
											<table style="width: 100%;">
												<tr>
													<td align="left" class="style5" colspan="3">
														<strong>Employee Targets</strong></td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
													<td>&nbsp;</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxComboBox ID="ddl_member" runat="server" ClientInstanceName="ddl_member"
															DataSourceID="SqlDataSource3" TextField="_name" ValueField="member_id"
															ValueType="System.Int32" Width="120px" CallbackPageSize="50" IncrementalFilteringMode="StartsWith">
															<ClientSideEvents SelectedIndexChanged="function(s, e) {
	gv_targets0.PerformCallback('z');
}" />
														</dx:ASPxComboBox>
													</td>
													<td>FY<asp:SqlDataSource ID="SqlDataSource3" runat="server"
														ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
														ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
														SelectCommand="Select get_name(member_id) _name, member_id from member where business_unit_id = ?id and member_status = 'Active'">
														<SelectParameters>
															<asp:ControlParameter ControlID="hdnid" Name="id" PropertyName="Value" />
														</SelectParameters>
													</asp:SqlDataSource>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb01" runat="server" ClientInstanceName="lb01"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb02" runat="server" ClientInstanceName="lb02"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb03" runat="server" ClientInstanceName="lb03"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb04" runat="server" ClientInstanceName="lb04"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb05" runat="server" ClientInstanceName="lb05"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb06" runat="server" ClientInstanceName="lb06"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb07" runat="server" ClientInstanceName="lb07"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb08" runat="server" ClientInstanceName="lb08"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb09" runat="server" ClientInstanceName="lb09"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb010" runat="server" ClientInstanceName="lb010"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb011" runat="server" ClientInstanceName="lb011"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="lb012" runat="server" ClientInstanceName="lb012"
															Font-Names="Arial" Font-Size="8pt" Text="m">
														</dx:ASPxLabel>
													</td>
													<td>&nbsp;</td>
												</tr>
												<tr>
													<td>
														<dx:ASPxComboBox ID="ddltarget0" runat="server" ClientInstanceName="ddltarget0"
															SelectedIndex="0" Width="120px">
															<Items>
																<dx:ListEditItem Selected="True" Text="Revenue" Value="Revenue" />
																<dx:ListEditItem Text="Margin" Value="Margin" />
																<dx:ListEditItem Text="Days to Invoice" Value="Days to Invoice" />
															</Items>
															<ClientSideEvents SelectedIndexChanged="function(s, e) {
	gv_targets0.PerformCallback('z');
}" />
														</dx:ASPxComboBox>
													</td>
													<td>
														<dx:ASPxSpinEdit ID="spnfy0" runat="server" ClientInstanceName="spnfy0"
															Height="21px" MaxValue="2099" MinValue="2013" Width="75px" NumberType="Integer" OnInit="spnfy0_Init">
															<ClientSideEvents NumberChanged="function(s, e) {
	gv_targets0.PerformCallback('z');
}" />
														</dx:ASPxSpinEdit>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb01" runat="server" ClientInstanceName="tb01" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb02" runat="server" ClientInstanceName="tb02" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb03" runat="server" ClientInstanceName="tb03" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb04" runat="server" ClientInstanceName="tb04" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb05" runat="server" ClientInstanceName="tb05" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb06" runat="server" ClientInstanceName="tb06" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb07" runat="server" ClientInstanceName="tb07" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb08" runat="server" ClientInstanceName="tb08" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb09" runat="server" ClientInstanceName="tb09" Width="60px"
															HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb010" runat="server" ClientInstanceName="tb010"
															Width="60px" HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb011" runat="server" ClientInstanceName="tb011"
															Width="60px" HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxTextBox ID="tb012" runat="server" ClientInstanceName="tb012"
															Width="60px" HorizontalAlign="Center">
														</dx:ASPxTextBox>
													</td>
													<td>
														<dx:ASPxButton ID="btnadd0" runat="server" AutoPostBack="False"
															ClientInstanceName="btnadd" Text="Add">
															<ClientSideEvents Click="function(s, e) {
	gv_targets0.PerformCallback('a');
}" />
														</dx:ASPxButton>
													</td>
												</tr>
												<tr>
													<td colspan="2" class="style7" style="font-size: 9pt; font-family: Arial"
														align="left">Total Business Unit Target</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr1" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr2" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr3" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr4" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr5" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr6" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr7" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr8" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr9" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr10" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr11" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center">
														<dx:ASPxLabel ID="tbr12" runat="server">
														</dx:ASPxLabel>
													</td>
													<td>
														<dx:ASPxLabel ID="tbrtotal" runat="server">
														</dx:ASPxLabel>
													</td>
												</tr>
												<tr>
													<td align="left" class="style7" colspan="2"
														style="font-size: 9pt; font-family: Arial" bgcolor="#8AE18A">Total Current Target Shortfall</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t01" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t02" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t03" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t04" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t05" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t06" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t07" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t08" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t09" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t010" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t011" runat="server">
														</dx:ASPxLabel>
													</td>
													<td align="center" bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t012" runat="server">
														</dx:ASPxLabel>
													</td>
													<td bgcolor="#8AE18A">
														<dx:ASPxLabel ID="t0total" runat="server">
														</dx:ASPxLabel>
													</td>
												</tr>
											</table>
										</TitlePanel>
									</Templates>
								</dx:ASPxGridView>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="Org Chart" Name="org_chart" ClientVisible="False">
						<ContentCollection>
							<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
								<div id="div_org" runat="server" align="left" style="text-align: left">
								</div>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="FVR Settings" Name="fvr_settings" Visible="True">
						<ContentCollection>
							<dx:ContentControl runat="server">
								<uc:business_unit_fvr_settings runat="server" ID="business_unit_fvr_settings" />
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
                    	<dx:TabPage Text="Pay Types" Name="pay_types" Visible="True">
						<ContentCollection>
							<dx:ContentControl runat="server">
                                <table style="width: 100%; align-content:center; display:flex">
                                    <tr>
                                        <td style="width:200px;text-align:center">
                                            <asp:Label Text="Available" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label></td>
                                       
                                        <td></td>
                                        <td style="width:200px;text-align:center">
                                            <asp:Label Text="Selected" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                           <asp:ListBox ID="LeftPaytype" runat="server" SelectionMode="multiple" Height="200px" Width="200px" /></td>
                                        <td style="align-items:center;width:150px">
                                            <asp:Button ID="Add" runat="server" OnClick="MoveRight" Text=" Add > " Width="150px"/><br />  <br />                                      
                                            <asp:Button ID="Remove" runat="server" OnClick="MoveLeft" Text=" Remove < " Width="150px" />
                                        </td>
                                        <td>
                                            <asp:ListBox ID="RightPaytype" runat="server" SelectionMode="Single" Height="200px" Width="200px"/></td>
                                    </tr>
                                   
                                </table>
                            </dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
				</TabPages>
			</dx:ASPxPageControl>
			<br />
			<dx:ASPxPopupControl ID="pop" runat="server" ClientInstanceName="pop"
				HeaderText=" " Height="600px" PopupHorizontalAlign="WindowCenter"
				PopupVerticalAlign="WindowCenter" Theme="NETheme01"
				Width="900px">
				<ContentCollection>
					<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
					</dx:PopupControlContentControl>
				</ContentCollection>
			</dx:ASPxPopupControl>
			<br />
			&nbsp;
    &nbsp;&nbsp;
    <br />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="header_placeholder">
	<style type="text/css">
		.style5 {
			font-size: medium;
			text-align: left;
		}

		.style6 {
			font-size: xx-small;
			text-align: left;
		}

		.style7 {
			font-size: xx-small;
		}
	</style>
</asp:Content>



