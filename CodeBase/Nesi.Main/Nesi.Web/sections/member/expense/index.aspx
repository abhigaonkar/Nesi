<%@ Page Language="C#" AutoEventWireup="true" Inherits="Expense" Title="NE:Expense Reimbursement" EnableTheming ="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>








<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>NE:Expense Receipt Processing</title>
	<style type="text/css">
		.c			{
			color:					#000;
			font-weight:			bold; 
			font-size:				11px; 
			font-family:			arial;
					}
		.n			{
			color:					#f00;
			font-weight:			bold; 
			font-size:				11px; 
			font-family:			arial;
					padding-bottom:	20px;
			}
.m			{
			color:					#000;
			font-weight:			bold; 
			font-size:				12px; 
			width:					15px; 
			font-family:			arial;
			text-align:				right;
			}
.v			{
			color:					#000;
			font-weight:			bold; 
			font-size:				11px;
			font-family:			arial;
			}
.con		{
			}
	</style>
	<link type="text/css" href="/css/autocomplete.css" rel="Stylesheet" />
	<link type="text/css" href="/css/base/ui.all.css" rel="Stylesheet" />
	<meta content="True" name="HandheldFriendly" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=0" name="viewport" />
	<meta name="viewport" content="width=device-width" />
</head>
<body style="background-image: none;" id='expense_reimbursement'>
	<form id="main" runat="server">
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript" src="/js/wo_timesheet.js"></script>
	<asp:scriptmanager ID="sm" runat="server">
	</asp:scriptmanager>
	<input type="hidden" class="empname" id="hid_empname" runat="server"/>
	<input type="hidden" class="country" id="hid_country" runat="server"/>
	<asp:updatepanel ID="up" runat="server">
		<ContentTemplate>
						<dx:ASPxCallbackPanel runat="server" ClientInstanceName="cbp_new" ID="cbp_new" OnCallback="cb_new_Callback" Theme="NETheme01">
							<ClientSideEvents EndCallback="timesheet.perdiem.perdiem_processed" ></ClientSideEvents>
							<PanelCollection>
								<dx:PanelContent ID="PanelContent1" runat="server">
	<dx:ASPxPageControl ID="pc_main" ClientInstanceName="pc_main" runat="server" ActiveTabIndex="0" Font-Bold="False" EnableCallbackAnimation="True" EnableCallBacks="True" Theme="NETheme01" Width="100%">
		<ContentStyle VerticalAlign="Top" CssClass="con">
			<Paddings Padding="0px"></Paddings>
		</ContentStyle>
		<ActiveTabStyle>
			<BackgroundImage Repeat="RepeatX" VerticalPosition="top"></BackgroundImage>
		</ActiveTabStyle>
		<TabPages>
			<dx:TabPage Text="Expense" ToolTip="File a new Expense Reimbursement Request">
				<TabImage Url="~/images/icon/icon[add].gif">
				</TabImage>
				<ContentCollection>
					<dx:ContentControl runat="server">
						<iframe src="if_newexpense.aspx?type=0" style="width:100%; height:550px;" frameborder="0" ID="if_newexpense" runat="server"></iframe>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Per Diem" ToolTip="File a new Per Diem Request">
				<TabImage Url="~/images/icon/icon[calendar].gif">
				</TabImage>
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxPanel ID="pnl_perdiem" runat="server">
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<div class="expense">
				<br />
				    <div class="row">
				        <div class="title">*Business Unit:</div>
				        <div class="control">
				            <dx:ASPxComboBox ID="ddl_bu" runat="server" CallbackPageSize="25" ClientInstanceName="per_diem_ddl_bu"  DropDownStyle="DropDown" DropDownWidth="90%" EnableCallbackMode="True" TextField="ddl_name" ValueField="id" Width="90%" Theme="NETheme01" AutoPostBack="True" OnSelectedIndexChanged="ddl_bu_SelectedIndexChanged" ValueType="System.Int32">
				                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="v_expense">
				                    <RequiredField ErrorText="Field is Required" IsRequired="True" />
				                </ValidationSettings>
				            </dx:ASPxComboBox>
				        </div>
				    </div>
                    <div class="title">
                        For Employees:</div>
                    <div class="control">
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="

Select a.member_id id, b.ddl_name, a.member_fullname, CONCAT('(',b.ddl_name,') ', a.member_fullname) name from member a 
							LEFT JOIN business_unit b ON a.business_unit_id = b.id 
							LEFT JOIN member c ON c.member_id = ?mid
							LEFT JOIN business_unit d ON c.business_unit_id = d.id
							where a.member_status='Active' and FIND_IN_SET(b.id, ?cid) and ((is_supervisor(a.member_id,?mid) or a.member_id = ?mid ) OR (d.country = b.country AND d.is_corporate = 1)) order by ddl_name, member_fullname">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddl_bu" Name="?cid" PropertyName="Value" />
                                <asp:ControlParameter ControlID="hdn_member" Name="?mid" PropertyName="Value" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:HiddenField ID="hdn_member" runat="server" />
                        <dx:ASPxCheckBoxList ID="cl_employees" runat="server" DataSourceID="SqlDataSource1" RepeatColumns="3" TextField="name" Theme="NETheme01" ValueField="id" ValueType="System.Int32" Width="100%">
                        </dx:ASPxCheckBoxList>
                    </div>
                    <br />
									<div class="row">
										<div class="title">Start Date:</div>
										<div class="control">
											<dx:ASPxDateEdit ID="perdiem_date_start" ClientInstanceName="perdiem_date_start" runat="server" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" AllowUserInput="False" Theme="NETheme01">
												<ClientSideEvents DateChanged="function(s, e) {timesheet.perdiem.perdiem_date_proc(s, 0);}" />
												<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorText="*" ValidationGroup="v_perdiem">
													<RequiredField ErrorText="Please select a start date" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxDateEdit>
										</div>
									</div>
									<div class="row">
										<div class="title">End Date:</div>
										<div class="control">
											<dx:ASPxDateEdit ID="perdiem_date_end" ClientInstanceName="perdiem_date_end" runat="server" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" AllowUserInput="False" Theme="NETheme01">
												<ClientSideEvents DateChanged="function(s, e) {timesheet.perdiem.perdiem_date_proc(s, 1);}" />
												<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorText="*" ValidationGroup="v_perdiem">
													<RequiredField ErrorText="Please select an end date" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxDateEdit>
										</div>
									</div>
									<div class="row">
										<div class="title">Rate:</div>
										<div class="control">
											<dx:ASPxTextBox ID="perdiem_t_rate" runat="server" ClientInstanceName="perdiem_t_rate" Width="170px" Theme="NETheme01">
												<ClientSideEvents TextChanged="function(s, e) {timesheet.perdiem.perdiem_date_proc(null, null);}" />
												<ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="v_perdiem">
													<RequiredField ErrorText="Field is Required" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxTextBox>
										</div>
									</div>
									<div class="row">
										<div class="title">Calculated Total:</div>
										<div class="control"><dx:ASPxLabel ID="perdiem_lb_calculated" runat="server" ClientInstanceName="perdiem_lb_calculated" Theme="NETheme01"></dx:ASPxLabel></div>
									</div>
									<div class="row">
										<div class="title">Work Order #:</div>
										<div class="control">
											<dx:ASPxComboBox ID="perdiem_c_workorder" runat="server" CallbackPageSize="25" ClientInstanceName="perdiem_c_workorder" DataSourceID="ds_workorder" DropDownStyle="DropDown" DropDownWidth="90%" EnableCallbackMode="True" TextField="woprog_bvwo" ValueField="woprog_id" ValueType="System.Int32" Width="90%" Theme="NETheme01">
												<Columns>
													<dx:ListBoxColumn Caption="WO" FieldName="woprog_bvwo" Name="WO" />
													<dx:ListBoxColumn Caption="Customer" FieldName="customer_name" Name="Customer" />
													<dx:ListBoxColumn Caption="Description" FieldName="description" Name="Description" />
												</Columns>
												<ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="v_expense">
													<RequiredField ErrorText="Field is Required" IsRequired="True" />
												</ValidationSettings>
												<ClientSideEvents SelectedIndexChanged="timesheet.perdiem.workorder_changed" />
											</dx:ASPxComboBox>
										</div>
									</div>
									<div class="row">
										<div class="title"> OR was this a shop expense?</div>
										<div class="control">
													<dx:ASPxCheckBox runat="server" CheckState="Unchecked" Text="Shop Expense" ClientInstanceName="perdiem_cb_shop" ID="perdiem_cb_shop" Theme="NETheme01">
<ClientSideEvents CheckedChanged="timesheet.perdiem.shop_check_changed"></ClientSideEvents>
</dx:ASPxCheckBox></div>
									</div>
									<div class="row">
										<div class="title">WO Line Description:</div>
										<div class="control"><dx:ASPxLabel ID="perdiem_lb_desc" runat="server" ClientInstanceName="perdiem_lb_desc"></dx:ASPxLabel></div>
									</div>
									<div class="row">
										<div class="title">&nbsp;</div>
										<div class="control">
											<dx:ASPxButton ID="perdiem_b_save" runat="server" AutoPostBack="False" ClientInstanceName="perdiem_b_save" Text="Save Request" ValidationGroup="v_perdiem" Theme="NETheme01">
												<ClientSideEvents Click="timesheet.perdiem.save" />
											<Image Url="~/images/icon/icon[save].gif">
											</Image>
										</dx:ASPxButton>
                                            <br />
                                        </div>
									</div>

							</div>
												<asp:SqlDataSource ID="ds_workorder" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT
	a.woprog_id,
	b.customer_name customer_name,
	a.woprog_bvwo,
	a.woprog_description `description`
FROM
	woprog a
LEFT JOIN
	customer b ON a.woprog_customer_id = b.customer_id
WHERE
	a.woprog_status = 'Open' AND
	a.business_unit_id = ?cid AND
	a.woprog_bvwo != 'Not Entered' 
ORDER BY 
	a.woprog_bvwo DESC">
													<SelectParameters>
													    <asp:ControlParameter ControlID="ddl_bu" Name="?cid" PropertyName="Value" />
													</SelectParameters>
												</asp:SqlDataSource>
								    <div class="title">
                                        &nbsp;</div>
								</dx:PanelContent>
							</PanelCollection>
						</dx:ASPxPanel>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="History">
				<TabImage Url="~/images/icon/icon[report].gif">
				</TabImage>
				<ContentCollection>
					<dx:ContentControl runat="server">
					    <dx:ASPxButton ID="btn_export" runat="server" OnClick="btn_export_Click" Text="Export" Width="120px" ClientInstanceName="btn_export">
					        <Image Url="~/images/icon/icon[excel].gif">
					        </Image>
					    </dx:ASPxButton>
                        <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="gv_expense">
                        </dx:ASPxGridViewExporter>
						<dx:ASPxGridView runat="server" ClientInstanceName="gv_expense" AutoGenerateColumns="False" DataSourceID="ds_expense" Width="90%" ID="gv_expense"
                             KeyFieldName="id_expense" OnCommandButtonInitialize="gv_expense_CommandButtonInitialize" OnRowDeleting="gv_expense_RowDeleting" 
                            PreviewFieldName="gl_name" Theme="NETheme01" enablepaginggestures="False">
							<Columns>
								<dx:GridViewDataTextColumn FieldName="approved" Width="50px" Caption="Status" VisibleIndex="1">
									<DataItemTemplate>
										<dx:ASPxImage ID="image" runat="server" ImageUrl='<%# string.Format("/images/icon/icon[{0}].gif", Eval("approved")) %>' ToolTip='<%# Eval("tooltip") %>'>
										</dx:ASPxImage>
									</DataItemTemplate>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="pay_period" Width="150px" Caption="Pay Period" VisibleIndex="2">
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataDateColumn FieldName="date_requested" Width="75px" Caption="Date Requested" VisibleIndex="3">
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataTextColumn FieldName="seller" Caption="Seller" VisibleIndex="4">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<CellStyle HorizontalAlign="Left">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn FieldName="amount" Width="50px" Caption="Total" VisibleIndex="5">
									<PropertiesTextEdit DisplayFormatString="{0:c2}">
									</PropertiesTextEdit>
									<CellStyle HorizontalAlign="Center">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewCommandColumn ButtonType="Image" ShowDeleteButton="True" ShowInCustomizationForm="True" VisibleIndex="0" Caption=" ">
								</dx:GridViewCommandColumn>
							    <dx:GridViewDataTextColumn Caption="Approval Manager" FieldName="approved_by" ShowInCustomizationForm="True" VisibleIndex="6" Width="70px">
                                </dx:GridViewDataTextColumn>
							    <dx:GridViewDataTextColumn Caption="Description" FieldName="item_text" ShowInCustomizationForm="True" VisibleIndex="7" Width="100%">
                                </dx:GridViewDataTextColumn>
							</Columns>
							<SettingsCommandButton>
								<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
								<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
								<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
							</SettingsCommandButton>
							<SettingsBehavior EnableRowHotTrack="True" ></SettingsBehavior>
							<SettingsPager PageSize="50">
							</SettingsPager>
							<Settings ShowPreview="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
							
							
						</dx:ASPxGridView>
						<asp:SqlDataSource ID="ds_expense" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
a.id_expense,
	if(a.approved = -1, '', a.approved) approved,
	a.approved approved_int,
	CAST(CONCAT(DATE_FORMAT(c.startdate, '%m/%d/%Y'), ' - ', DATE_FORMAT(c.enddate, '%m/%d/%Y')) AS CHAR) pay_period,
	a.item_text,
	a.id_payperiod,
	a.date_requested,
	URLDECODE(b.name_seller) seller,
	a.amount,
                            	if (a.approved=1,'Expense has been approved','Expense is waiting to be approved') tooltip,
		m.member_fullname approved_by ,
Concat(gl_te.account_no,' - ',gl_te.gl_chart_name) gl_name
FROM 
	expense_reimbursement a 
LEFT JOIN expense_seller AS b ON a.id_seller = b.id_seller
LEFT JOIN payperiods AS c ON a.id_payperiod = c.PayperiodID
LEFT JOIN member AS m ON a.approved_by = m.Member_ID
INNER JOIN member ON a.id_member = member.Member_ID
LEFT JOIN business_unit ON member.business_unit_id = business_unit.ID
LEFT JOIN gl_te ON a.master_id = gl_te.account_no AND gl_te.tax_entity_id = business_unit.tax_entity_id
WHERE
	a.id_member = @member_id
ORDER BY a.date_requested DESC" ></asp:SqlDataSource>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>
		<TabStyle Height="25px">
			<BackgroundImage Repeat="NoRepeat" VerticalPosition="bottom"></BackgroundImage>
		</TabStyle>
	</dx:ASPxPageControl>
								</dx:PanelContent>
							</PanelCollection>
						</dx:ASPxCallbackPanel>
</ContentTemplate>
	</asp:updatepanel>

	</form>
</body>
</html>
