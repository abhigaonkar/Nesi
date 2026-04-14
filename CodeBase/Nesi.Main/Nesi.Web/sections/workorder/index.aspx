<%@ Page
	Language='C#'
	MasterPageFile='~/IntraDefault.master'
	AutoEventWireup='true'
	CodeBehind='index.aspx.cs'
	Inherits='sections_workorder'
	Title='Work Order'
	EnableTheming="True" %>

<%@ Import Namespace="nesi.core" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="modules/accounting_notes.ascx" TagName="accounting_notes" TagPrefix="uc" %>
<%@ Register Src="modules/wo_tasklist.ascx" TagName="wo_tasklist" TagPrefix="uc1" %>
<%@ Register Src="modules/project_schedule.ascx" TagName="project_schedule" TagPrefix="uc2" %>
<%@ Register Src="~/sections/workorder/modules/close_checklist.ascx" TagPrefix="uc" TagName="close_checklist" %>

<%@ Register Src="modules/analysis.ascx" TagName="analysis" TagPrefix="uc3" %>



<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
	</asp:ScriptManager>
	<script type="text/javascript" src="/js/functions.js?unique=<%=Toolbox.do_RandomString(10) %>"></script>
	<script type="text/javascript" src="/js/workorder.js?unique=<%=Toolbox.do_RandomString(10) %>"></script>
	<script type="text/javascript" src="/js/wo_analysis.js?unique=<%=Toolbox.do_RandomString(10) %>"></script>
	<script type="text/javascript">
        wo.id = <%= hidWOProgID.Value %>;
        wo.header.obj_margin = '<%= lbl_topmargin.ClientID %>';
		wo.header.obj_tobeinvoiced = '<%= lbl_ToBeInvoiced.ClientID %>';
		wo.header.obj_status = '<%= lblStatus.ClientID %>';
		wo.header.obj_ts = '<%= hid_ts.ClientID %>';
		wo.header.obj_business_unit_id = '<%= hidCompanyID.ClientID %>';
		wo.header.obj_margin_total = '<%=lbl_top_whole_job.ClientID %>';

        $(document).ready(function () {
            page_obj.update_panel_progress.bind();
        });
    </script>


	<asp:UpdatePanel ID="UpdatePanel1" runat="server">

		<ContentTemplate>
			<asp:HiddenField runat="server" ID="hid_ts" />
			<div id="Error"></div>
			<table width="100%" id="work_order">
				<tr>
					<td>
						<div id="header" runat="server">
						<div id="header_info">
							<div class="l_pane">
								<div id="tblbuttons" runat="server" class="action_panel_l pad left">
									<asp:ImageButton ID="btnBack" runat="server" BackColor="Transparent" BorderColor="Transparent" BorderStyle="Solid" BorderWidth="5px" CausesValidation="False" Height="25px" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_back.png" PostBackUrl="~/wo_prog_edit.aspx" ToolTip="Back to previous page" Visible="False" Width="25px" />
									<asp:ImageButton ID="ImageButtonPrintWO" runat="server" BackColor="Transparent" BorderColor="Transparent" BorderStyle="Solid" BorderWidth="5px" Height="25px"
										ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_fileprint.png" ToolTip="Print Work Order" Visible="False" Width="25px" />
									<asp:ImageButton ID="btnSendPMforQuestions" runat="server" BackColor="Transparent" BorderColor="Transparent" BorderStyle="Solid"
										BorderWidth="5px" Height="25px" ImageUrl="~/images/IconsButtons/question_mark.png" OnClientClick="return(false);"
										ToolTip="Send To Questions Column" Visible="False"
										Width="25px" />
									<asp:Image ID="btnApprovedByPM" runat="server" BackColor="Transparent"
										BorderColor="Transparent" BorderStyle="Solid" BorderWidth="5px"
										causesvalidation="False" Height="25px"
										ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_apply-ApprovedbyPM.PNG"
										ToolTip="Approved By Project Manager" Visible="False" Width="25px" />
									<asp:ImageButton ID="btnSendToBMApproval" runat="server"
										BackColor="Transparent" BorderColor="Transparent" BorderStyle="Solid"
										BorderWidth="5px" Height="25px"
										ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_apply-SendToWaitForBM.PNG"
										ToolTip="Send BM for their approval" Visible="False" Width="25px" />
									<asp:Image ID="btnSendToRework" runat="server" BackColor="Transparent"
										BorderColor="Transparent" BorderStyle="Solid" BorderWidth="5px" Height="25px"
										ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_reload.png"
										ToolTip="Send to Rework Column" Visible="False" Width="25px" />
									<asp:Image ID="btnSendToPMApproval" runat="server" BackColor="Transparent"
										BorderColor="Transparent" BorderStyle="Solid" BorderWidth="5px" Height="25px"
										ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_apply-SendToWaitForPM.PNG"
										ToolTip="Send to PM for their approval" Visible="False" Width="25px" />
									<asp:ImageButton ID="btnWorkOrderPreview" runat="server" BackColor="Transparent"
										BorderColor="Transparent" BorderStyle="Solid" BorderWidth="5px" Height="25px"
										ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_app_scanner.png"
										ToolTip="Work Order Preview" Visible="False" Width="25px" OnClientClick="" />
									<asp:ImageButton ID="btnInvoicePreview" runat="server" BackColor="Transparent"
										BorderColor="Transparent" BorderStyle="Solid" BorderWidth="5px" Height="25px"
										ImageUrl="~/images/IconsButtons/32px-Crystal_128_penguin.png"
										ToolTip="Invoice" Visible="False" Width="25px" OnClientClick="" />
									<asp:ImageButton ID="bt_delete_wo" runat="server" BorderColor="Transparent"
										BorderWidth="5px" CssClass="ttip" Height="25px"
										ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_button_cancel.png"
										OnClick="bt_delete_wo_Click" Width="25px" />
									<asp:ImageButton ID="btn_refresh" runat="server" BackColor="Transparent"
										BorderColor="Transparent" BorderStyle="Solid" BorderWidth="5px" Height="25px"
										ImageUrl="~/images/IconsButtons/refresh.png" OnClick="btn_refresh_Click"
										ToolTip="Refresh WO" Visible="True" Width="25px" />
									<asp:ImageButton ID="bt_signoff" runat="server" Height="32px"
										ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_printWOSignoff.png"
										Visible="False" Width="32px" />
								</div>
								<div class="action_panel_r right pad">
									<asp:Image ID="btnSendToWaitingToBeInvoiced" runat="server"
										BackColor="Transparent" BorderColor="Transparent" BorderStyle="Solid"
										BorderWidth="5px" CausesValidation="False"
										ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_apply.png"
										ToolTip="Final Approval, go ahead and Invoice" Visible="False" Height="25px" Width="25px" />
									<asp:Image ID="btnSendtoWaitCustPO" runat="server" BackColor="Transparent"
										BorderColor="Transparent" BorderStyle="Solid" BorderWidth="5px"
										ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_waitForCustPO.png"
										ToolTip="Send to Waiting Customer PO Column" Visible="False" Height="25px" Width="25px" />
								</div>
								<div class="clear pad">
									<asp:Label ID="lblDaysSinceScan" runat="server" Font-Bold="True"
										Font-Names="arial" ForeColor="#0000C0"></asp:Label>
								</div>
								<div class="clear pad">
									<asp:Label ID="lbCommentPopup" runat="server" Visible="False"
										Font-Names="Arial"></asp:Label>
                                    <asp:Label ID="lbCommentFixup" runat="server" Visible="False" Font-Names="Arial" Text="Fix Comments"></asp:Label>
								</div>
							</div>
							<div class="r_pane">
								<div class="pad">
									<asp:Label ID="lblWODisplay" runat="server" Text="" Font-Names="Arial"></asp:Label>
								</div>
								<div class="pad">
									<asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="#0000C0" Text=""></asp:Label>
								</div>
								<div class="pad">
									<asp:Label ID="lbl_lastmodified" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="#0000C0" Text=""></asp:Label>
									<img id="img_lastmodified" runat="server" src="/images/icon/icon[refresh].gif" onclick="wo.handle_lastupdated(this);" style="cursor: pointer;" align="absmiddle" width="16" height="16" alt="Update Last Modified" />
								</div>
								<div class="pad">
									<dx:ASPxLabel ID="lbl_ToBeInvoiced" runat="server" ClientInstanceName="lbl_ToBeInvoiced" Font-Bold="True" Font-Names="Arial" ForeColor="#0000C0" Text="$ 0.00" EnableViewState="False" ClientVisible="False"></dx:ASPxLabel>
								</div>
								<div class="pad">
									<dx:ASPxLabel ID="lbl_topmargin" runat="server"
										ClientInstanceName="lbl_topmargin" ClientVisible="False"
										EnableViewState="False" Font-Bold="True" Font-Names="Arial" ForeColor="#0000C0"
										Text="% 0.00" Wrap="False">
									</dx:ASPxLabel>
								</div>
								<div class="pad">
									<dx:ASPxLabel ID="lbl_top_whole_job" runat="server"
										ClientInstanceName="lbl_top_whole_job" ClientVisible="False"
										EnableViewState="False" Font-Bold="True" Font-Names="Arial" ForeColor="#0000C0"
										Text="% 0.00" Wrap="False">
									</dx:ASPxLabel>
								</div>
								<div class="pad">
									<button id="btnViewScan" runat="server"><img src="/images/icon/icon[pdf].gif" width="16" height="16" align="absmiddle"/> View Scan</button>
								</div>
							</div>
						</div>
					
						</div>	
						<dx:ASPxLabel ID="lblError" runat="server" Font-Bold="True" EncodeHtml="false" ForeColor="Red"
							ClientInstanceName="lblError" Font-Names="Arial">
						</dx:ASPxLabel>
					</td>
				</tr>
				<tr>
					<td>
						<dx:ASPxPageControl ID="pc_main" runat="server" ActiveTabIndex="0" ClientInstanceName="pc_main"
							BackColor="Transparent"
							OnActiveTabChanged="pc_main_ActiveTabChanged" AutoPostBack="True"
							EnableCallBacks="True" Width="100%"
							ClientVisible="False" Theme="NETheme01">
							<TabPages>
								<dx:TabPage Text="General" Name="general">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl1" runat="server">
											<table cellpadding="0" cellspacing="0" id="general">
												<tr>
													<td valign="top">
														<dx:ASPxCallbackPanel ID="cbp_left" runat="server"
															ClientInstanceName="cbp_left" OnCallback="cbp_left_Callback"
															OnCustomJSProperties="cbp_left_CustomJSProperties" Theme="NETheme01">
															<ClientSideEvents BeginCallback="wo.cbp_left.callback_begin" CallbackError="wo.cbp_left.callback_error" EndCallback="wo.cbp_left.callback_end" />
															<PanelCollection>
																<dx:PanelContent ID="PanelContent1" runat="server">
																	<table style="width: 100%; font-family: Arial;" cellpadding="2" cellspacing="0">
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="ChkServiceCall" runat="server" CheckState="Unchecked"
																					ClientInstanceName="ChkServiceCall" CssClass="ch" Text="Service Call:"
																					TextAlign="Left" Width="150px" Native="True" Font-Names="Arial">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="chk_cc" runat="server" CheckState="Unchecked"
																					ClientInstanceName="chk_cc" CssClass="ch" Text="Credit Card Payment:"
																					TextAlign="Left" Width="150px" Native="True" Font-Names="Arial">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="chkRD" runat="server" AutoPostBack="True"
																					CheckState="Unchecked" ClientInstanceName="chkRD" CssClass="ch"
																					OnCheckedChanged="chkRD_CheckedChanged" Text=" R &amp; D:" TextAlign="Left"
																					Width="150px" Native="True" Font-Names="Arial">
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="chk_warranty" runat="server" CheckState="Unchecked"
																					ClientInstanceName="chk_warranty" CssClass="ch" Native="True" Text="Warranty:"
																					TextAlign="Left" ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0"
																					Width="150px" Font-Names="Arial">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="chk_labor_only" runat="server" CheckState="Unchecked"
																					Text="Labor Only?" TextAlign="Left" Width="150px" CssClass="ch"
																					Native="True" Font-Names="Arial" ClientSideEvents-CheckedChanged="function(s,e){{chk_mat_only.SetChecked(false);chk_sub_only.SetChecked(false);}}" ClientInstanceName="chk_labor_only">
																					<ClientSideEvents CheckedChanged="function(s,e){{chk_mat_only.SetChecked(false);chk_sub_only.SetChecked(false);}}" />
																				</dx:ASPxCheckBox>
																			</td>
																			<td>
																				<dx:ASPxCheckBox ID="chk_mat_only" runat="server" CheckState="Unchecked"
																					Text="Material Only?" TextAlign="Left" Width="150px" CssClass="ch"
																					Native="True" Font-Names="Arial" ClientSideEvents-CheckedChanged="function(s,e){{chk_labor_only.SetChecked(false);chk_sub_only.SetChecked(false);}}" ClientInstanceName="chk_mat_only">
																					<ClientSideEvents CheckedChanged="function(s,e){{chk_labor_only.SetChecked(false);chk_sub_only.SetChecked(false);}}" />
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="chkInvoiceFeedbackIssues" runat="server" AutoPostBack="True" CheckState="Unchecked"
																					ClientInstanceName="ChkInvoiceFeedbackIssues" CssClass="ch" Text="Invoice Feedback Issues:" OnCheckedChanged="chkInvoiceFeedbackIssues_CheckedChanged"
																					TextAlign="Left" Width="150px" Native="True" Font-Names="Arial">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr runat="server">
																			<td>
																			</td>
																			<td>
																			</td>
																		</tr>
																		<tr runat="server">
																			<td>
																				<dx:ASPxCheckBox ID="chk_sub_only" runat="server" CheckState="Unchecked"
																					Text="Subcontract Resell Only?" TextAlign="Left" Width="150px" CssClass="ch"
																					Native="True" Font-Names="Arial" ClientSideEvents-CheckedChanged="function(s,e){{chk_mat_only.SetChecked(false);chk_lab_only.SetChecked(false);}}"
																					ClientInstanceName="chk_sub_only" ClientVisible="False">
																					<ClientSideEvents CheckedChanged="function(s,e){{chk_mat_only.SetChecked(false);chk_lab_only.SetChecked(false);}}" />
																				</dx:ASPxCheckBox>
																			</td>
																			<td></td>
																		</tr>
																	</table>
																	<table width="350" cellpadding="2" cellspacing="0" style="font-family: Arial">
																		<tr>
																			<div id="div_radiocredit" style="background-color: #FFFFCC" runat="server" visible="false">
																				<td style="width: 125px" class="c" bgcolor="#FFFFCC">
																					<dx:ASPxRadioButtonList ID="radiotype" runat="server" ValueType="System.Int32"
																						SelectedIndex="0" Style="background-color: #FFFFCC" ClientInstanceName="radiotype" AutoPostBack="true" OnSelectedIndexChanged="radiotype_SelectedIndexChanged">

																						<ClientSideEvents SelectedIndexChanged="function(s, e) {
	
	    var val=radiotype.GetValue();
        if(val == '1') {
	       dteStartDate.SetDate((new Date()));
        } else {
           dteStartDate.SetDate(null);
        }

		ddl_creditwolink.SetValue(null);
	}" />
																						<Items>
																							<dx:ListEditItem Selected="True" Text="Regular" Value="0" />
																							<dx:ListEditItem Text="Credit" Value="1" />
																							<dx:ListEditItem Text="ReBill" Value="2" />
																						</Items>
																					</dx:ASPxRadioButtonList>
																				</td>
																				<td valign="middle" bgcolor="#FFFFCC">Original Work Order
																					<dx:ASPxComboBox ID="ddl_creditwolink" runat="server" OnSelectedIndexChanged="ddl_creditwolink_SelectedIndexChanged"
																						AnimationType="None"
																						EnableCallbackMode="True" IncrementalFilteringMode="Contains" TextField="wo"
																						ValueField="id" ValueType="System.Int32" ClientInstanceName="ddl_creditwolink" EnableClientSideAPI="True"
																						ClientEnabled="true" Width="100%" CssClass="dxeEditArea" Font-Names="Arial">
																						<ClientSideEvents SelectedIndexChanged="function(s, e) {
	OnddlCreditWO(s);
}" />
																					</dx:ASPxComboBox>
																				</td>
																			</div>
																		</tr>
																		<tr>
																			<td class="c" style="width: 125px">Business Unit:</td>
																			<td valign="middle">
																				<dx:ASPxComboBox ID="ddlCompany" runat="server"
																					AnimationType="None" ClientInstanceName="ddlCompany"
																					ForeColor="Black" TextField="name" ValueField="ID" Enabled="False"
																					ValueType="System.Int32" Width="100%" CssClass="dxeEditArea" Font-Names="Arial">
																					<ClientSideEvents SelectedIndexChanged="l_cb" />

																					<ButtonStyle ForeColor="White">
																					</ButtonStyle>
																					<ValidationSettings SetFocusOnError="True">
																					</ValidationSettings>
																					<Border BorderColor="Silver" BorderStyle="Solid" />
																				</dx:ASPxComboBox>
																			</td>
																		</tr>
																		<tr>
																			<td class="c" style="width: 125px">Revenue Line (Classification):</td>
																			<td valign="middle">
																				<dx:ASPxComboBox ID="ddlRevenueLines" runat="server" ClientEnabled="false" ReadOnly="true"
																					AnimationType="None" ClientInstanceName="ddlRevenueLines"
																					ForeColor="Black" TextField="name" ValueField="ID" EnableCallbackMode="True"
																					ValueType="System.Int32" Width="100%" CssClass="dxeEditArea" Font-Names="Arial">

																					<ButtonStyle ForeColor="White">
																					</ButtonStyle>
																					<ValidationSettings SetFocusOnError="True">
																					</ValidationSettings>
																					<Border BorderColor="Silver" BorderStyle="Solid" />
																				</dx:ASPxComboBox>
																			</td>
																		</tr>
																		<tr id="row_dept" runat="server">
																			<td style="width: 125px" class="c"></td>
																			<td valign="middle"></td>
																		</tr>
																		<tr>
																			<td style="width: 125px" class="c">Customer:</td>
																			<td valign="middle">
																				<table cellpadding="0" cellspacing="0" width="100%">
																					<tr>
																						<td>
																							<dx:ASPxComboBox ID="ddlCustomer" runat="server" CallbackPageSize="250"
																								ForeColor="Black" IncrementalFilteringDelay="350"
																								IncrementalFilteringMode="Contains" TextField="Customer_Name"
																								ValueField="Customer_ID" ValueType="System.Int32" Width="100%"
																								DropDownRows="5" EnableCallbackMode="True"
																								OnDataBound="cb_DataBound" AnimationType="None" ClientInstanceName="ddlCustomer"
																								Font-Names="Arial" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">

																								<SettingsLoadingPanel Text="Loading..." />

																								<ButtonStyle ForeColor="White"></ButtonStyle>
																								<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																								<ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                        lblError.SetText(&quot;&quot;);
	                                        chkProgress.SetEnabled(true);
                                              
                                               
                                                $('#hdnJobCostWO').val('');
                                                $('#hdnJobCostTotalQuote').val('');
												if(typeof(ddlJobCostWO) !== 'undefined')
													{
													ddlJobCostWO.SetValue('');
													lbljobcost_quote_details.SetText(&quot;&quot;);
                                              
													tblProgress.SetVisible(false);
													}
                                                
                                                OnCustomerChanged(s);
}" />
																							</dx:ASPxComboBox>
																							<%--											ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select customer_id,URLDECODE(customer_name) customer_name from vw_activecustomers where business_unit  = @companyid AND customer_status NOT IN (4,5,6) AND IF(@woprog_id = 0, customer_id NOT IN (SELECT internal_companyno_intranet_custid FROM internal_companyno where business_unit_id = @companyid and internal_companyno_id != 11), true) AND customer_qc_member_id IS NOT NULL ORDER BY customer_name" DataSourceMode="DataReader"> --%>
																							<asp:SqlDataSource ID="sds_customer" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL ds_workorder_customers(@woprog_id, @business_unit_id)" DataSourceMode="DataReader">
																								<SelectParameters>
																									<asp:ControlParameter ControlID="ddlCompany" Name="@business_unit_id" PropertyName="Value" />
																									<asp:ControlParameter ControlID="hidWOProgID" DefaultValue="0" Name="@woprog_id" PropertyName="Value" />
																								</SelectParameters>
																							</asp:SqlDataSource>
																							<dx:ASPxLabel ID="lblCustomerWarning" runat="server" EncodeHtml="false" Font-Bold="True" Font-Names="Arial"
																								ForeColor="Red" Width="100%">
																							</dx:ASPxLabel>
																						</td>
																						<td>
																							<button type="button" onclick="wo.handle_customer_request(this);">
																								<img src="/images/icon/icon[edit].gif" width="16" height="16" alt="Request" /></button>
																						</td>
																					</tr>
																				</table>
																			</td>
																		</tr>
																		<tr id="td_parent_info" runat="server" style="display: none;">
																			<td class="c" style="width: 125px">Parent Work Order:</td>
																			<td valign="middle">
																				<dx:ASPxComboBox ID="ddl_parent_workorder" runat="server" ClientInstanceName="ddl_parent_workorder" Native="False" ToolTip="Note: Child Work Orders are not in this list"
																					OnCallback="ddl_parent_workorder_callback" TextField="text" ValueField="id"
																					ValueType="System.Int32" Width="100%" IncrementalFilteringMode="Contains" Cursor="pointer" Font-Underline="True" ForeColor="Blue">



																					<Border BorderColor="Silver" />
																					<ClientSideEvents Init="parent_ddl_OnInit" />
																				</dx:ASPxComboBox>
																			</td>
																		</tr>
																		<tr>
																			<td style="width: 125px;" class="c">Address:
																			</td>
																			<td valign="middle">
																				<div style="float: left">

																					<dx:ASPxComboBox ID="ddlAddress" runat="server" CallbackPageSize="7" ForeColor="Black"
																						TextField="Address" ValueField="Address_ID" ValueType="System.Int32"
																						Width="100%" ClientInstanceName="ddlAddress"
																						OnCallback="ddlAddress_Callback" DataSourceID="sds_address"
																						OnDataBound="cb_DataBound" AnimationType="None" CssClass="dxeEditArea" Font-Names="Arial">
																						<ButtonStyle ForeColor="White"></ButtonStyle>
																						<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																					</dx:ASPxComboBox>
																				</div>

																				<img src="../../images/icon/icon[help].gif" style="float: right" title="Original address was inactive, default address has been set" runat="server" id="inactive_icon" visible="False" />



																				&nbsp;</img></img></img></img></img></img></img></img></img></img></img></img></img></img></img><asp:SqlDataSource ID="sds_address" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select  0 as address_id,'Not Applicable' as address union Select address_id,address as address from vw_address where cust_id = @custid AND active = 1">
																					<SelectParameters>
																						<asp:ControlParameter ControlID="ddlCustomer" Name="@custid" PropertyName="Value" />
																					</SelectParameters>
																				</asp:SqlDataSource>
																			</td>
																		</tr>
																		<tr>
																			<td style="width: 125px" class="c">Contact:</td>
																			<td valign="middle">
																				<dx:ASPxComboBox ID="ddlContact" runat="server" ForeColor="Black" IncrementalFilteringMode="Contains" TextField="Contact_Name" ValueField="Contact_ID" ValueType="System.Int32" Width="100%" EnableCallbackMode="False" ClientInstanceName="ddlContact" EnableSynchronization="False" OnCallback="ddlContact_Callback" DropDownRows="20" OnDataBound="cb_DataBound" AnimationType="None" Font-Names="Arial">
																					<ButtonStyle ForeColor="White">
																					</ButtonStyle>
																					<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																					<ClientSideEvents ButtonClick="function(s, e) {
if (ddlCustomer.GetText()!=&quot;&quot;)
{
tbnewcontact.SetText('');
popc.Show();}
}" />
																					<Buttons>
																						<dx:EditButton>
																							<Image Url="~/images/icon/icon[add].gif">
																							</Image>
																						</dx:EditButton>
																					</Buttons>
																				</dx:ASPxComboBox>
																				<asp:SqlDataSource ID="sds_contact" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																					SelectCommand="SELECT 0 contact_id, 'Please select contact' contact_name UNION ALL (SELECT contact_id, contact_name FROM contact WHERE contact_type = 'Customer' AND contact_cust_id= @cust_id AND contact_status = 'Active' ORDER BY contact_name)">
																					<SelectParameters>
																						<asp:ControlParameter ControlID="ddlCustomer" Name="@cust_id" PropertyName="Value" />
																					</SelectParameters>
																				</asp:SqlDataSource>
																			</td>
																		</tr>
																		<tr style="display: none;">
																			<td class="c" style="width: 125px" valign="middle">Default Ext. Location:</td>
																			<td valign="middle">
																				<dx:ASPxComboBox ID="ddl_default_location" runat="server" TextField="name"
																					ValueField="id" Width="100%" Visible="false" Font-Names="Arial">
																				</dx:ASPxComboBox>
																			</td>
																		</tr>
																		<tr id="row_pm" runat="server">
																			<td class="c" style="width: 125px" valign="middle">Project Manager:</td>
																			<td valign="middle">
																				<dx:ASPxComboBox ID="ddlPM" runat="server" CallbackPageSize="50"
																					ClientInstanceName="ddlPM" DataSourceID="sds_pm" AnimationType="None"
																					EnableCallbackMode="True" EnableClientSideAPI="True" ForeColor="Black"
																					OnCallback="ddlPM_Callback" OnDataBound="cb_DataBound" TextField="Name"
																					ValueField="Member_ID" ValueType="System.Int32" Width="100%" CssClass="dxeEditArea" Font-Names="Arial">

																					<ClientSideEvents EndCallback="function(s, e) {

ddlPM.SetSelectedIndex(ddlPM.cp_selected);
txtWODescription.SetText(ddlPM.cp_description);
txtSalesValue.SetText(ddlPM.cp_salesvalue);
ddlContact.SetValue(ddlPM.cp_ContactID);
}" />
																					<ButtonStyle ForeColor="White">
																					</ButtonStyle>
																					<Border BorderColor="Silver" BorderStyle="Solid" />
																					<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																				</dx:ASPxComboBox>
																				<i style="font-family: 'Segoe UI'; font-size: 12px; font-style: normal"><b id="pm_inactive_name" runat="server"></b></i>
																				<asp:SqlDataSource ID="sds_pm" runat="server"
																					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																					SelectCommand="Select Member_id,Name from vw_activepms where business_unit_id=@business_unit_id ">
																					<SelectParameters>
																						<asp:ControlParameter ControlID="ddlCompany" Name="@business_unit_id"
																							PropertyName="Value" />
																					</SelectParameters>
																				</asp:SqlDataSource>
																			</td>
																		</tr>
																		<tr id="row_quote" runat="server">
																			<td style="width: 125px" valign="top" class="c">Quote:</td>
																			<td valign="middle">
																				<dx:ASPxComboBox ID="ddlquote" runat="server" CallbackPageSize="150"
																					ForeColor="Black"
																					TextField="Quote_Stuff" ValueField="Quote_n" ValueType="System.Int32" Width="100%"
																					ClientInstanceName="ddlquote" DataSourceID="sds_quote"
																					OnDataBound="cb_DataBound" AnimationType="None"
																					IncrementalFilteringMode="Contains" Theme="NETheme01" OnSelectedIndexChanged="ddlquoteSelectIndexChange">
																					<ButtonStyle ForeColor="White"></ButtonStyle>
																					<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																					<ClientSideEvents SelectedIndexChanged="function(s, e) {
	if(s.GetValue() != '0') // This was added per ticket 9067 - Problem when removing a quote from a WO
		{
		      cb_quoteinfo.PerformCallback(s.GetValue());
		} else {
             if(!chkProgress.GetChecked())
             {
                  document.getElementById('ctl00_cphMasterBody_pc_main_cbp_left_div_quoteterms_tr').style.display = 'none';
                 ddlRevenueLines.SetValue($('.defaultTM').val());
             }
            
         }
}" />

																				</dx:ASPxComboBox>
																				<asp:SqlDataSource ID="sds_quote" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" CacheDuration="0"
																					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select quote_n,Quote_Stuff from vw_openquotes where customer_id = @cust_id and business_unit_id = @business_unit_id AND status_id = 4">
																					<SelectParameters>
																						<asp:ControlParameter ControlID="ddlCustomer" Name="@cust_id" PropertyName="Value" />
																						<asp:ControlParameter ControlID="ddlCompany" Name="@business_unit_id" PropertyName="Value" />

																					</SelectParameters>
																				</asp:SqlDataSource>
																				<asp:Label ID="lblInstructons" runat="server" Font-Names="Arial" Text="If you need to unlink  or relink a quote remove the quote by selecting the option Select Quote if Applicable and then save the work order. " Font-Size="Smaller"></asp:Label>
																				<asp:Label ID="lblTMquote" runat="server" Font-Names="Arial" Visible="false"> </asp:Label>
																			</td>
																		</tr>
																		<tr id="div_quoteterms_tr" runat="server" style="display: none;">
																			<td class="c" valign="top">Billing Schedule:</td>
																			<td style="padding: 3px">
																				<dx:ASPxLabel ID="lblquoteterms" ClientInstanceName="lblquoteterms" runat="server"></dx:ASPxLabel>
																			</td>
																		</tr>
																		<tr>
																			<td class="c" style="width: 125px; display: none" valign="middle">
																				<div id="lbldayscredit" runat="server" style="vertical-align: middle">
																					Days Credit:
																				</div>
																			</td>
																			<td valign="middle" style="display: none">
																				<dx:ASPxSpinEdit ID="spndayscredit" runat="server"
																					ClientInstanceName="spndayscredit" Height="20px" NumberType="Integer" MaxValue="180" Number="0"
																					Width="60px">
																				</dx:ASPxSpinEdit>
																			</td>
																		</tr>
																		<tr>
																			<td class="c" style="width: 125px" valign="middle">
																				<div id="lblTerms" runat="server" style="vertical-align: middle">
																					Payment Terms:
																				</div>
																			</td>
																			<td valign="middle">
																				<asp:SqlDataSource ID="sqlterms" runat="server"
																					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																					SelectCommand="Select 0 id,'Not Applicable' Terms union Select Term_ID as id, CONCAT(Term_Code,'-',Term_Desc) as Terms from term where Active=1 order by id"></asp:SqlDataSource>
																				<dx:ASPxComboBox ID="combo_default_terms" runat="server"
																					ValueType="System.Int32" Width="100%" CssClass="dxeEditArea" Theme="NETheme01"
																					DataSourceID="sqlterms" TextField="Terms" ValueField="id" ClientInstanceName="ddl_terms">
																				</dx:ASPxComboBox>
																			</td>
																		</tr>

																		<tr id="row_invoicetype" runat="server">
																			<td class="c" style="width: 125px" valign="middle">
																				<div id="Div1" runat="server" style="vertical-align: middle">
																					Default Invoice Type:
																				</div>
																			</td>
																			<td valign="middle">
																				<asp:SqlDataSource ID="sql_invoice_types" runat="server"
																					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																					SelectCommand="Select * from customer_default_invoice_types"></asp:SqlDataSource>
																				<dx:ASPxComboBox ID="combo_default_invoicetype" runat="server"
																					SelectedIndex="0" ValueType="System.Int32" Width="100%" CssClass="dxeEditArea" Theme="NETheme01"
																					DataSourceID="sql_invoice_types" TextField="invoice_type" ValueField="id">
																					<Items>
																						<dx:ListEditItem Selected="True" Text="Default with Grouping" Value="0" />
																						<dx:ListEditItem Text="All items broken out, no material prices" Value="1" />
																						<dx:ListEditItem Text="All items broken out, material prices shown" Value="2" />
																						<dx:ListEditItem Text="Labour Total and Material Total ONLY" Value="4" />
																					</Items>
																				</dx:ASPxComboBox>
																			</td>
																		</tr>
																		<tr>
																			<td class="c" style="width: 125px" valign="middle">Currency:
																			</td>
																			<td>
																				<asp:SqlDataSource ID="sql_currency" runat="server"
																					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																					SelectCommand="Select * from currency"></asp:SqlDataSource>
																				<dx:ASPxComboBox ID="ddl_currency" runat="server"
																					ValueType="System.Int32" Width="100%" CssClass="dxeEditArea" Theme="NETheme01"
																					DataSourceID="sql_currency" TextField="currency" ValueField="id" ClientInstanceName="ddl_currency">
																				</dx:ASPxComboBox>
																			</td>
																		</tr>
																		<tr>
																			<td
																				class="c" style="width: 125px" valign="middle">Requires Inspection?
																			</td>
																			<td>
                                                                                <dx:ASPxRadioButton ID="chk_inspection_yes" runat="server" Checked="True" Text="Yes" GroupName="chk_inspection"></dx:ASPxRadioButton>
                                                                                <dx:ASPxRadioButton ID="chk_inspection_no" runat="server" Text="No" GroupName="chk_inspection"></dx:ASPxRadioButton>
																			</td>
																		</tr>
																		<tr>
																			<td
																				class="c" style="width: 125px" valign="middle">Inspection Link:
																			</td>
																			<td>
																				<dx:ASPxTextBox ID="tb_inspection_link" runat="server" Width="100%" Theme="NETheme01"></dx:ASPxTextBox>
																			</td>
																		</tr>
																		<tr id="tr_fixed_labour" runat="server">
																			<td class="c" style="width: 125px" valign="middle">Fixed Labor Rate:</td>
																			<td>
																				<table>
																					<tr>
																						<td>
																							<dx:ASPxSpinEdit ID="spn_fixed_labour" runat="server" ClientInstanceName="spn_fixed_labour" DecimalPlaces="2" Height="20px" MaxValue="500" Number="0" Width="60px" MinValue="0.0">
																								<CaptionSettings Position="Right" ShowColon="False" />
																								<CaptionCellStyle CssClass="CaptionCellStyle"></CaptionCellStyle>
																							</dx:ASPxSpinEdit>
																						</td>
																						<td>
																							<dx:ASPxCheckBox ID="chk_fixed_labour" ClientInstanceName="chk_fixed_labour" runat="server" Text="Use?">
																							</dx:ASPxCheckBox>
																						</td>
																					</tr>
																				</table>

																			</td>
																		</tr>
																		<tr id="tr_fixed_material" runat="server">
																			<td class="c" style="width: 125px" valign="middle">Fixed Material Markup:</td>
																			<td>
																				<table>
																					<tr>
																						<td>
																							<dx:ASPxSpinEdit ID="spn_material_markup" runat="server" ClientInstanceName="spn_material_markup" Height="20px" DecimalPlaces="4" Number="1" Width="60px" Increment="0.0025" MinValue="1" MaxValue="10">
																								<CaptionSettings Position="Right" ShowColon="False" />
																								<CaptionCellStyle CssClass="CaptionCellStyle"></CaptionCellStyle>
																							</dx:ASPxSpinEdit>
																						</td>
																						<td>
																							<dx:ASPxCheckBox ID="chk_fixed_markup" ClientInstanceName="chk_fixed_markup" runat="server" Text="Use?"></dx:ASPxCheckBox>
																						</td>
																					</tr>
																				</table>
																			</td>
																		</tr>
																		<tr>
																			<td class="c" style="width: 125px" valign="middle">Sustainability Project?</td>
																			<td>
																				<dx:ASPxCheckBox ID="chk_sustain" ClientInstanceName="chk_sustain" runat="server" Text=""></dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td class="c" style="width: 125px" valign="middle">Shell Work Order:</td>
																			<td>
																				<dx:ASPxCheckBox ID="chkShellWO" ClientInstanceName="chk_shell_wo" runat="server" Text=""></dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td class="c" style="width: 125px" valign="middle">Show Mobile Part Management:</td>
																			<td>
																				<dx:ASPxCheckBox ID="chkMobilePartManage" ClientInstanceName="chkMobilePartManage" runat="server" Text=""></dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr runat="server" ID="enablePrevailingWagesTr">
																			<td class="c" style="width: 125px" valign="middle">Enable Prevailing Wages:</td>
																			<td>
																				<dx:ASPxCheckBox ID="chkEnablePrevailingWages" ClientInstanceName="chkEnablePrevailingWages" Enabled="true" ClientEnabled="true" runat="server" Text=""></dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td class="c" style="width: 125px" valign="middle">Customer Reference #:</td>
																			<td>
																				<dx:ASPxTextBox ID="tbCustomerReference" runat="server" Text="" MaxLength="50"></dx:ASPxTextBox>
																			</td>
																		</tr>
																		<tr>
																			<td class="c" style="width: 125px" valign="middle">Work Order Tag:</td>
																			<td>
																				<asp:SqlDataSource ID="sql_tag" runat="server"
																					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																					SelectCommand="Select 0 id,'Not Applicable' name union  Select id,name from woprog_jobtag"></asp:SqlDataSource>
																				<dx:ASPxComboBox ID="ddl_jobtag" runat="server"
																					ValueType="System.Int32" Width="100%" CssClass="dxeEditArea" Theme="NETheme01"
																					DataSourceID="sql_tag" TextField="name" ValueField="id" ClientInstanceName="ddl_jobtag">
																				</dx:ASPxComboBox>
																			</td>
																		</tr>
																	</table>
																	<dx:ASPxCallback ID="cb_quoteinfo" runat="server" ClientInstanceName="cb_quoteinfo"
																		OnCallback="cb_quoteinfo_Callback">
																		<ClientSideEvents BeginCallback="function(s, e) {please_wait('start');}"
																			CallbackComplete="wo.quote_info"
																			CallbackError="function(s, e) {please_wait('stop');}" />
																	</dx:ASPxCallback>

																</dx:PanelContent>
															</PanelCollection>


														</dx:ASPxCallbackPanel>
													</td>
													<td style="width: 130px">&nbsp;</td>
													<td style="white-space: nowrap" valign="top" width="400px">
														<table cellpadding="2" cellspacing="0" style="font-family: Arial" width="100%">
															<tr>
																<td style="width: 150px;" class="c">
																	<asp:Label ID="lblCustPO" runat="server" Text="Customer PO:"></asp:Label>
																</td>

																<td style="width: 200px; white-space: normal;" nowrap="nowrap">
																	<table style="border-collapse: collapse">
																		<tr>
																			<td>
																				<dx:ASPxButtonEdit ID="txtCustPO" runat="server" ForeColor="Black" MaxLength="45"
																					OnButtonClick="txtCustPO_ButtonClick" Width="100%" CssClass="custpo" Height="17px"
																					ClientInstanceName="txtCustPO">
																					<ClientSideEvents KeyDown="function(s,e){if(e.htmlEvent.keyCode == 222){ASPxClientUtils.PreventEvent(e.htmlEvent);}}" TextChanged="function(s, e) {
	lblError.SetText(&quot;&quot;);
}" />
																					<Buttons>
																						<dx:EditButton>
																							<Image Url="~/images/icon/icon[save].gif"></Image>
																						</dx:EditButton>
																					</Buttons>
																					<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																				</dx:ASPxButtonEdit>
																			</td>
																			<td>
																				<asp:Image ID="txtCustPO_notice" runat="server" CssClass="custpo_notice"
																					ImageUrl="~/images/icon/icon[attention].gif" Visible="False" />
																			</td>
																		</tr>
																	</table>
																</td>
															</tr>
															<tr>
																<td style="width: 150px;" class="c">Area in Plant:</td>
																<td width="135px">
																	<dx:ASPxTextBox ID="txtAreainPlant" runat="server" Width="100%"
																		ForeColor="Black" ClientInstanceName="txtAreainPlant" Native="True">
																		<Border BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"></Border>
																	</dx:ASPxTextBox>
																</td>
															</tr>
															<tr>
																<td style="width: 150px;" class="c">Cut Date:</td>
																<td width="135px">
																	<dx:ASPxDateEdit ID="dte_cutDate" runat="server" EditFormat="Custom" EditFormatString="yyyy-MM-dd"
																		Width="100%" ForeColor="Black" AnimationType="None"
																		ClientInstanceName="dte_cutDate" Font-Names="Arial" Theme="NETheme01">
																		<Border BorderColor="Silver" BorderStyle="Solid"></Border>

																		<ValidationSettings>
																			<RequiredField ErrorText="You must set a valid Start Date" />
																			<RequiredField ErrorText="You must set a valid Start Date" />
																		</ValidationSettings>
																	</dx:ASPxDateEdit>
																</td>
															</tr>
															<tr>
																<td style="width: 150px;" class="c">Start Date:</td>
																<td width="135px">
																	<dx:ASPxDateEdit ID="dteStartDate" runat="server" EditFormat="Custom" EditFormatString="yyyy-MM-dd"
																		Width="100%" ForeColor="Black" DisplayFormatString="yyyy-MM-dd"
																		ClientInstanceName="dteStartDate" AnimationType="None" Font-Names="Arial">
																		<ClientSideEvents DateChanged="function(s, e) {
                                        
                                        var temp = dteStartDate.GetText();
                                        dteExpEndDate.SetMinDate(new Date(temp));
                                        if (!chkProgress.GetChecked())
                                        {
                                        updatereqdates(temp);
                                        }
	
}" />
																		<ValidationSettings>
																			<RequiredField ErrorText="You must set a valid Start Date" />

																		</ValidationSettings>
																		<Border BorderColor="Silver" BorderStyle="Solid" />

																		<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																	</dx:ASPxDateEdit>
																</td>
															</tr>
															<tr>
																<td style="width: 135px;" class="c">Exp End Date:</td>
																<td width="135px">
																	<dx:ASPxDateEdit ID="dteExpEndDate" runat="server"
																		EditFormat="Custom" EditFormatString="yyyy-MM-dd"
																		Width="100%" AnimationType="None" ClientInstanceName="dteExpEndDate" Font-Names="Arial">

																		<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																	</dx:ASPxDateEdit>
																</td>
															</tr>
															<tr>
																<td style="width: 135px;" class="c">Percentage Complete:</td>
																<td width="135px">
																	<dx:ASPxSpinEdit ID="spn_perc_complete" runat="server" Number="0" MaxValue="100" MinValue="0" Increment="10">
																	</dx:ASPxSpinEdit>
																</td>
															</tr>
															<tr id="row_expected_sales" runat="server">
																<td width="150" height="23">
																	<div id="lblsalesvalue" runat="server" class="c">
																		Exp Sales Value:
																	</div>
																</td>
																<td width="135px">
																	<dx:ASPxTextBox ID="txtSalesValue" runat="server" Width="100%" ClientInstanceName="txtSalesValue"
																		NullText="Expected Sales Value" CssClass="dxeEditArea" Native="True" Font-Names="Arial">
																		<NullTextStyle ForeColor="Silver" Font-Italic="True">
																		</NullTextStyle>
																		<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																	</dx:ASPxTextBox>
																</td>
															</tr>
															<tr id="row_expected_labour" runat="server">
																<td style="width: 135px;" class="c">
																	<div id="lbllabourvalue" runat="server">
																		Exp Hours Needed:
																	</div>
																</td>
																<td width="135px">
																	<dx:ASPxTextBox ID="txtlaborvalue" runat="server" Width="100%" ClientInstanceName="txtlaborvalue"
																		NullText="Expected Labor Needed" CssClass="dxeEditArea" Native="True" Font-Names="Arial">
																		<NullTextStyle Font-Italic="True" ForeColor="Silver">
																		</NullTextStyle>
																		<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																	</dx:ASPxTextBox>
																</td>
															</tr>
															<tr id="row_verbal" runat="server">
																<td class="c" style="width: 135px; white-space: nowrap;">
																	<dx:ASPxCheckBox ID="chk_verbal" runat="server" CheckState="Unchecked"
																		CssClass="ch" Font-Bold="False" Font-Italic="False" Text="Verbal Quote?:"
																		TextAlign="Left" Width="120px" Native="True" Font-Names="Arial" ClientVisible="False">
																	</dx:ASPxCheckBox>
																</td>
																<td valign="top" width="135px">&nbsp;</td>
															</tr>
															<tr id="Tr9" runat="server">
																<td class="c" style="width: 135px; white-space: nowrap;">BDM:
																</td>
																<td valign="top" width="135px">
																	<asp:SqlDataSource ID="sds_customer_ram" runat="server"
																		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																		SelectCommand="SELECT 0 member_id, 'Please Select a BDM' member_fullname UNION SELECT netsuite_employee_internal_id member_id, netsuite_employee_name member_fullname FROM netsuite_sales_rep WHERE netsuite_isinactive = 0 AND netsuite_issales_rep = 1"></asp:SqlDataSource>
																	<dx:ASPxComboBox ID="ddl_custram" runat="server" ClientInstanceName="ddl_custram"
																		SelectedIndex="0" ValueType="System.Int32" Width="100%" Theme="NETheme01"
																		DataSourceID="sds_customer_ram" TextField="member_fullname" ValueField="member_id">
																	</dx:ASPxComboBox>
																	

																</td>
															</tr>
															<tr id="Tr8" runat="server">
																<td class="c" style="width: 135px; white-space: nowrap;">Acting BDM:
																</td>
																<td valign="top" width="135px">
																	<asp:SqlDataSource ID="sds_acct_manager" runat="server"
																		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
																	<dx:ASPxComboBox ID="ddl_rams" runat="server" ClientInstanceName="ddl_rams"
																		SelectedIndex="0" ValueType="System.Int32" Width="100%" Theme="NETheme01"
																		DataSourceID="sds_acct_manager" TextField="member_fullname" ValueField="member_id" ClientEnabled="False">																		
																	</dx:ASPxComboBox>
																</td>
															</tr>
														</table>
														<table style="color: gray" cellpadding="2" cellspacing="0" width="100%">
															<tr>
																<td style="width: 150px; white-space: nowrap;" class="c" valign="top">&nbsp;</td>
															</tr>
															<tr>
																<td class="c" style="width: 150px; white-space: nowrap;" valign="top">
																	<dx:ASPxCheckBox ID="chkProgress" runat="server" AutoPostBack="True" ClientInstanceName="chkProgress" CssClass="ch" Font-Bold="False" Font-Names="Arial" Native="True" OnCheckedChanged="ProgressAssociated_CheckedChanged" Text="Progress Bill: " TextAlign="Left" Width="120px">
																	</dx:ASPxCheckBox>
																	<div id="div_jobcost_quote_link0" runat="server">
																	</div>
																</td>
															</tr>
															<tr>
																<td style="width: 100%;" valign="top">
																	<dx:ASPxPanel ID="tblProgress" runat="server" ClientInstanceName="tblProgress"
																		Width="100%">
																		<PanelCollection>
																			<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
																				<table id="tblProgressx" runat="server" cellpadding="2" cellspacing="0"
																					style="white-space: nowrap; background-color: lavender; font-family: Arial;" width="100%">
																					<tr id="Tr1" runat="server">
																						<td id="jobcostlbl" runat="server">Job Cost WO:</td>
																						<td id="Td3" runat="server">
																							<div id="div_jobcost_quote_link" runat="server">
																							</div>
																							<dx:ASPxComboBox ID="ddlJobCostWO" runat="server" AutoPostBack="True"
																								CallbackPageSize="20" ClientInstanceName="ddlJobCostWO" AnimationType="None"
																								EnableCallbackMode="True" ForeColor="DimGray"
																								OnSelectedIndexChanged="ddlJobCostWO_SelectedIndexChanged"
																								TextField="description" ValueField="woprog_id" ValueType="System.Int32"
																								Width="100%" Font-Names="Arial">
																								<ButtonStyle ForeColor="White">
																								</ButtonStyle>
																								<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																							</dx:ASPxComboBox>
																							<asp:HiddenField ID="hdnJobCostWO" runat="server" />
																							<asp:HiddenField ID="hdnJobCostTotalQuote" runat="server" />
																						</td>
																					</tr>
																					<tr id="Tr4" runat="server">
																						<td id="Td1" runat="server">Quote Details:</td>
																						<td id="Td5" runat="server">
																							<dx:ASPxLabel ID="lbljobcost_quote_details" runat="server"
																								ClientInstanceName="lbljobcost_quote_details" Text="ASPxLabel" Font-Names="Arial">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr id="Tr2" runat="server">
																						<td id="Td4" runat="server"></td>
																						<td id="Td8" runat="server">&nbsp;<dx:ASPxCheckBox ID="chkProgressBillCredit" runat="server"
																							CheckState="Unchecked" ClientInstanceName="chkProgressBillCredit" Text="Credit" Native="True">
																						</dx:ASPxCheckBox>
																							<dx:ASPxCheckBox ID="chkDownPayment" runat="server" CheckState="Unchecked"
																								ClientInstanceName="chkDownPayment" Text="Down Payment" Native="True">
																								<ClientSideEvents CheckedChanged="function(s,e){cbp_left.PerformCallback('downpayment|'+s.GetChecked());}" />
																							</dx:ASPxCheckBox>
																						</td>
																					</tr>

																					<tr id="Tr5" runat="server">
																						<td id="Td9" runat="server">Type:</td>
																						<td id="Td10" runat="server">
																							<dx:ASPxComboBox ID="cbopb_type" runat="server" ClientInstanceName="cbopb_type"
																								Font-Names="Arial" SelectedIndex="0" Width="150px">
																								<ClientSideEvents SelectedIndexChanged="function(s, e) {
if (s.GetText()==&quot;Flat Amount&quot;)
	txtpb_amt.nullText = &quot;Enter $&quot;;
else
	txtpb_amt.nullText = &quot;Enter %&quot;;
}" />
																								<Items>
																									<dx:ListEditItem Selected="True" Text="Percentage" Value="Percentage" />
																									<dx:ListEditItem Text="Flat Amount" Value="Flat Amount" />
																								</Items>
																							</dx:ASPxComboBox>
																						</td>
																					</tr>
																					<tr id="Tr6" runat="server">
																						<td id="Td2" runat="server">Amount:</td>
																						<td id="Td12" runat="server">
																							<dx:ASPxTextBox ID="txtpb_amt" runat="server" ClientInstanceName="txtpb_amt"
																								Font-Names="Arial" NullText="Enter %" Width="150px">
																								<ClientSideEvents TextChanged="function(s, e) {
	if (cbopb_type.GetText()=='Flat Amount')
{
txtSalesValue.SetText(s.GetText());
}
}" />
																								<NullTextStyle ForeColor="Silver">
																								</NullTextStyle>
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
																					<tr id="Tr7" runat="server">
																						<td id="Td13" runat="server">% Margin:</td>
																						<td id="Td14" runat="server">
																							<dx:ASPxTextBox ID="txt_pb_margin" runat="server" ClientInstanceName="txt_pb_margin"
																								Width="150px" NullText="Enter %" Font-Names="Arial" Text="100" Enabled="True" ClientEnabled="False">
																								<NullTextStyle ForeColor="Silver">
																								</NullTextStyle>
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
																					<tr id="Tr3" runat="server">
																						<td id="Td6" runat="server">% Billed To Date:</td>
																						<td id="Td7" runat="server">
																							<dx:ASPxLabel ID="lblAlreadyBilled" runat="server"
																								ClientInstanceName="lblAlreadyBilled" Text="ASPxLabel" Font-Names="Arial">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																				</table>
																			</dx:PanelContent>
																		</PanelCollection>
																	</dx:ASPxPanel>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td colspan="3" valign="middle">
														<table cellpadding="2" cellspacing="0" width="100%" style="font-family: Arial">
															<tr id="row_description" runat="server">
																<td style="width: 125px" class="c" valign="top">Description:  <span class="chrm">
																	<br>

																	<span style="font-size: xx-small">Characters Remaining:</span>
																	<dx:ASPxLabel ID="txtWODescription_cr" runat="server"
																		EnableClientSideAPI="True" Font-Size="XX-Small">
																	</dx:ASPxLabel>
																	<br>
																</span></td>
																<td style="width: 100%">
																	<dx:ASPxMemo ID="txtWODescription" runat="server" Width="100%"
																		ClientInstanceName="txtWODescription" Height="60px"
																		NullText="Enter description of work order... keep it short" BackColor="#FFFFCC" Native="True" Font-Names="Arial">
																		<NullTextStyle ForeColor="Silver" Font-Italic="True">
																		</NullTextStyle>
																		<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																		<ClientSideEvents
																			Init="function(s, e) { InitMemoMaxLength(s, 1000); RecalculateCharsRemaining(s); }"
																			GotFocus="EnableMaxLengthMemoTimer" LostFocus="DisableMaxLengthMemoTimer"
																			KeyDown="RecalculateCharsRemaining" KeyUp="function(s, e) {
	$(s.mainElement).spellcheck();
	RecalculateCharsRemaining(s);
}" />
																	</dx:ASPxMemo>
																</td>
															</tr>
															<tr>
																<td style="width: 125px" class="c" valign="top">Special Instructions:<span class="chrm">
																	<br>
																	<span style="font-size: xx-small">Characters Remaining:</span>
																	<dx:ASPxLabel ID="txtSpecialInstruction_cr" runat="server"
																		EnableClientSideAPI="True" Font-Size="XX-Small">
																	</dx:ASPxLabel>
																	<br>
																</span></td>
																<td style="width: 100%">
																	<dx:ASPxMemo ID="txtSpecialInstruction" runat="server" Width="100%" ForeColor="Black"
																		Height="50px" ClientInstanceName="txtSpecialInstruction" Native="True" Font-Names="Arial"
																		NullText="Enter customer or contact specific stuff that needs to be remembered ...">
																		<NullTextStyle Font-Italic="True" ForeColor="Silver">
																		</NullTextStyle>
																		<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																		<ClientSideEvents
																			Init="function(s, e) { InitMemoMaxLength(s, 1000); RecalculateCharsRemaining(s); }"
																			GotFocus="EnableMaxLengthMemoTimer" LostFocus="DisableMaxLengthMemoTimer"
																			KeyDown="RecalculateCharsRemaining" KeyUp="function(s, e) {
	RecalculateCharsRemaining(s);
}" />
																	</dx:ASPxMemo>
																</td>
															</tr>
															<tr style="visibility: collapse">
																<td class="c" style="width: 125px" valign="top">
																	<div id="lbl_tax" runat="server">
																		Taxes Applicable:
																	</div>
																</td>
																<td style="width: 100%">
																	<table bgcolor="#99FF99" style="width: 100%;" cellpadding="0" cellspacing="0">
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="chkTax1" runat="server" CheckState="Unchecked"
																					ClientInstanceName="chkTax1" Native="True">
																				</dx:ASPxCheckBox>
																			</td>
																			<td rowspan="4" valign="middle" align="right">
																				<dx:ASPxButton ID="btnUpdateTaxes" runat="server" Theme="NETheme01"
																					OnClick="btnUpdateTaxes_Click" Text="Update Line Items" Width="130px" Font-Names="Arial">
																				</dx:ASPxButton>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="chkTax2" runat="server" CheckState="Unchecked"
																					ClientInstanceName="chkTax2" Native="True">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="chkTax3" runat="server" CheckState="Unchecked"
																					ClientInstanceName="chkTax3" Native="True">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<dx:ASPxCheckBox ID="chkTax4" runat="server" CheckState="Unchecked"
																					ClientInstanceName="chkTax4" Native="True">
																				</dx:ASPxCheckBox>
																			</td>
																		</tr>
																	</table>
																</td>
															</tr>
															<tr>
																<td class="c" style="width: 125px" valign="top">
																	<dx:ASPxCheckBox ID="chkOnHold" runat="server" CheckState="Unchecked"
																		ClientInstanceName="chkOnHold" Font-Bold="True" ForeColor="Red" Text="On Hold"
																		Wrap="False" Native="True">
																	</dx:ASPxCheckBox>
																</td>
																<td style="width: 100%">
																	<dx:ASPxMemo ID="txtwhyhold" runat="server" ClientInstanceName="txtwhyhold"
																		ForeColor="Black" Height="50px"
																		NullText="If this work order is on hold, enter a note about it here..."
																		ToolTip="Why is this wo on hold?" Width="100%" Native="True" Font-Names="Arial">
																		<NullTextStyle ForeColor="Silver" Font-Italic="True">
																		</NullTextStyle>
																		<Border BorderColor="Silver" BorderStyle="Solid" />
																		<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																	</dx:ASPxMemo>
																</td>
															</tr>
                                                           <tr id="remove_close_reassign" style="">
																<td class="c" style="width: 125px" valign="top">
																	<dx:ASPxButton ID="btnCloseandReassign" runat="server"
																								Theme="NETheme01"
																								Font-Names="Arial" Height="50px"
																								OnClick="btn_CloseAndReassign_Click"
																								ToolTip="Close & Reassign" AutoPostBack="False" Text="Close & Reassign">
																	</dx:ASPxButton>
																</td>
																<td style="width: 100%">
																	<dx:ASPxMemo ID="txtwhyCloseReassign" runat="server" ClientInstanceName="txtwhyCloseReassign" ReadOnly="true"
																		ForeColor="Black" Height="50px"
																		NullText="Reassign Reason"
																		ToolTip="Why is this wo to be close and reassign?" Width="100%" Native="True" Font-Names="Arial">
																		<NullTextStyle ForeColor="Silver" Font-Italic="True">
																		</NullTextStyle>
																		<Border BorderColor="Silver" BorderStyle="Solid" />
																		<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																	</dx:ASPxMemo>
																</td>
															</tr>
															<tr>
																<td class="c" colspan="2" valign="top" width="100%">
																	<table style="width: 100%;" align="center">
																		<tr>
																			<td style="text-align: center">
																				<dx:ASPxButton ID="btnSaveGeneral" runat="server"
																					OnClick="btnSaveGeneral_Click" Height="50px" HorizontalAlign="Center" ToolTip="Save / Create this Work Order"
																					VerticalAlign="Middle" Width="50px" Theme="NETheme01">
																					<Image Url="~/images/icon/icon[save].gif">
																					</Image>
																				</dx:ASPxButton>
																			</td>
																			<td align="center" style="padding-right: 10px; padding-left: 10px">
																				<table id="tbllaser" runat="server" bgcolor="whitesmoke"
																					style="width: 100%; border-collapse: collapse;">
																					<tr runat="server">
																						<td runat="server" align="right" style="padding-left: 10px">
																							<dx:ASPxButton ID="btn_printlaser" runat="server"
																								Theme="NETheme01"
																								Font-Names="Arial" Height="50px"
																								OnClick="btn_printlaser_Click"
																								ToolTip="Print a laser work order" Wrap="False" Text="Print this stuff ->">
																							</dx:ASPxButton>
																						</td>
																						<td runat="server" align="left">
																							<dx:ASPxCheckBoxList ID="chk_printwhat" runat="server" TextWrap="False"
																								ClientInstanceName="chk_printwhat" Native="True">
																								<CheckBoxStyle Wrap="False" />
																								<Items>
																									<dx:ListEditItem Text="Map" Value="1" />
																									<dx:ListEditItem Text="Safety Form" Value="2" />
																									<dx:ListEditItem Text="Sign Off" Value="3" />
																									<dx:ListEditItem Text="Quote" Value="4" />
																									<dx:ListEditItem Text="Material List" Value="5" />
																								</Items>
																								<Border BorderStyle="None" />
																								<Border BorderStyle="None"></Border>
																							</dx:ASPxCheckBoxList>
																						</td>
																					</tr>
																				</table>
																			</td>
																			<td align="center" style="padding-right: 10px; padding-left: 10px; visibility: hidden;">
																				<dx:ASPxButton ID="btn_printdot" runat="server"
																					Font-Bold="True" Font-Names="Arial" Font-Size="7pt" Height="25px"
																					OnClick="btn_printdot_Click"
																					Text="Print Dot Matrix" ToolTip="Print to the Dot Matrix Printer" Wrap="False">
																				</dx:ASPxButton>
																			</td>
																			<td align="center" style="padding-right: 10px; padding-left: 10px">
																				<dx:ASPxButton ID="btn_printquote" runat="server"
																					CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
																					Font-Bold="True" Font-Names="Arial" Height="25px"
																					OnClick="btn_printquote_Click"
																					SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Text="Print Quote"
																					ToolTip="Print a copy of the quote" Wrap="False" ClientVisible="False">
																				</dx:ASPxButton>
																			</td>
																			<td align="right" width="100%">
																				
																			</td>
																		</tr>
																	</table>
																</td>
															</tr>

															<tr>
																<td style="width: 125px; padding-right: 5px; padding-left: 5px" valign="top">&nbsp;</td>
																<td style="width: 550px; font-size: 9pt; font-family: Arial;" valign="top"
																	runat="server" id="Td11">&nbsp;</td>
															</tr>
														</table>
													</td>
												</tr>
											</table>

										</dx:ContentControl>
									</ContentCollection>
                                     
								</dx:TabPage>
								<dx:TabPage Text="Customer" ClientVisible="False" Name="customer">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl2" runat="server">
											<iframe id="cust_frame" runat="server" height="900" scrolling="auto"
												width="100%"
												style="border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none"
												frameborder="0"></iframe>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Line Items" ClientVisible="False" Name="lineItems">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl3" runat="server">
											<iframe id="picklist" runat="server" class="picklist" frameborder="0" height="1400"
												name="picklist" scrolling="auto" style="min-width: 800px"></iframe>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Quote" Name="quote" ClientVisible="False">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl4" runat="server">
											<iframe id="quote_frame" class="quote_frame" runat="server" height="auto" scrolling="no" width="100%" frameborder="0"></iframe>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="POs" ClientVisible="False" Name="pos">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl5" runat="server">
											<dx:ASPxLabel ID="lblOpenBVPOs" runat="server" Font-Names="Arial" ForeColor="Red"></dx:ASPxLabel>
											<br />
											<dx:ASPxGridView ID="grid_POs" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="po_details_id"
												PreviewFieldName="description" Theme="NETheme01" EnableCallBacks="False">
												<Columns>
													<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" ShowClearFilterButton="true"
														VisibleIndex="0">
													</dx:GridViewCommandColumn>
													<dx:GridViewDataHyperLinkColumn Caption="Purchase Order" FieldName="poprog_bvpo"
														VisibleIndex="1">
														<DataItemTemplate>
															<a href="javascript:boing('/redir.aspx?url=%2Fsections%2Fpurchaseorder%2Fpo_prog_add.aspx%3Faction%3Dshow%2526poprogid%3D<%# Eval("poprogid") %>');"><%# Eval("poprog_bvpo") %></a>
														</DataItemTemplate>
													</dx:GridViewDataHyperLinkColumn>
													<dx:GridViewDataHyperLinkColumn Caption="Vendor" FieldName="vendorname"
														VisibleIndex="2">
														<DataItemTemplate>
															<asp:LinkButton ID="LinkButton2" runat="server" Text='<%#Container.Text %>' OnClick="openvendor_click"> </asp:LinkButton>
														</DataItemTemplate>
														<CellStyle Wrap="False">
														</CellStyle>
													</dx:GridViewDataHyperLinkColumn>
													<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="3">
														<CellStyle Wrap="False">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="PO Date" FieldName="placed_date"
														VisibleIndex="4">
														<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd"></PropertiesTextEdit>
														<CellStyle Wrap="False">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Received Date" FieldName="recd_date"
														VisibleIndex="5">
														<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd"></PropertiesTextEdit>
														<CellStyle Wrap="False">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataHyperLinkColumn Caption="Part" FieldName="masterid"
														VisibleIndex="6">
														<DataItemTemplate>
															<asp:LinkButton ID="LinkButton3" runat="server" Text='<%#Container.Text %>' OnClick="openpart_click"> </asp:LinkButton>
														</DataItemTemplate>
													</dx:GridViewDataHyperLinkColumn>
													<dx:GridViewDataTextColumn Caption="Description" FieldName="description" Visible="False"
														VisibleIndex="7" Width="100%">
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Rec'd Qty" FieldName="qty_rec"
														VisibleIndex="8">
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Ord Qty" FieldName="qty_ord"
														VisibleIndex="8">
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Cost" FieldName="cost" VisibleIndex="9">
														<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Extd Cost" FieldName="Extd_Cost"
														VisibleIndex="10">
														<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Notes" VisibleIndex="18" Width="40px">
														<EditCellStyle HorizontalAlign="Center">
															<Paddings Padding="0px" />
															<Paddings Padding="0px" />
														</EditCellStyle>
														<DataItemTemplate>
															<a href="javascript:void(0);" onclick="showNotes('<%# Container.KeyValue %>',event)"><%# note_handler(Container)%></a>
														</DataItemTemplate>
														<CellStyle HorizontalAlign="Center">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Active" FieldName="active" VisibleIndex="11" Width="30px">
														<DataItemTemplate>
															<a href="javascript:void(1);"><%# noteinactive_handler(Container)%></a>
														</DataItemTemplate>
														<CellStyle HorizontalAlign="Center">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="poprogid" FieldName="poprogid" Visible="False"
														VisibleIndex="12">
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="vendorid" FieldName="vendorid" Visible="False"
														VisibleIndex="13">
													</dx:GridViewDataTextColumn>
												</Columns>
												<SettingsPager PageSize="50" Mode="ShowAllRecords">
													<AllButton Text="All">
													</AllButton>
													<NextPageButton Text="Next &gt;">
													</NextPageButton>
													<PrevPageButton Text="&lt; Prev">
													</PrevPageButton>
												</SettingsPager>
												<Settings ShowPreview="True" ShowFooter="True" ShowGroupFooter="VisibleAlways"
													ShowFilterRow="True" />

												<Settings ShowPreview="True" ShowFooter="True" ShowGroupFooter="VisibleAlways"
													ShowFilterRow="True" />
												<TotalSummary>
													<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="Extd_Cost" ShowInColumn="Extd Cost"
														Tag="Total" ShowInGroupFooterColumn="Extd Cost" SummaryType="Sum" />
												</TotalSummary>
											</dx:ASPxGridView>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Time Entries" ClientVisible="False" Name="timeEntries">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl6" runat="server">
											<table width="100%">
												<tr>
													<td style="padding-bottom: 1px; padding-top: 1px;" colspan="4">
														<textarea id="txtTSComments" runat="server" style="width: 100%; vertical-align: bottom;" cols="10" rows="10"></textarea>
														<asp:CheckBox
															ID="chkPrint" runat="server" Text="Print Timesheet Comments on Invoice (press save button to save)" ForeColor="DimGray" Font-Names="Arial" />
														<br />
														<asp:Button ID="btncomments" runat="server" Text="Save" Width="50px" Style="vertical-align: bottom"
															OnClick="btncomments_Click" Font-Names="Arial" />
														<br />
													</td>
												</tr>
												<tr>
													<td nowrap="nowrap"
														style="padding-bottom: 1px; padding-top: 1px; white-space: nowrap; font-family: Arial, Helvetica, sans-serif;"
														colspan="4">
														<dx:ASPxButtonEdit ID="btnSaveServiceDateText" runat="server" Width="400px"
															OnButtonClick="btnSaveServiceDateText_ButtonClick" Theme="NETheme01">
															<Buttons>
																<dx:EditButton ToolTip="Save service date text">
																	<Image Url="~/images/icon/icon[save].gif">
																	</Image>
																</dx:EditButton>
																<dx:EditButton Text="Refresh">
																</dx:EditButton>
															</Buttons>
														</dx:ASPxButtonEdit>
														(This appears on the invoice if work order is not quoted.)</td>
												</tr>
												<tr>
													<td colspan="4">
														<dx:ASPxGridView ID="Grid_TimeEntries" runat="server" AutoGenerateColumns="True" OnHtmlRowCreated="Grid_TimeEntries_HtmlRowCreated" Width="100%" Theme="NETheme01" DataSourceID="ds_workorder_timeentries">

															<SettingsPager PageSize="25">
															</SettingsPager>
															<Settings ShowFilterRow="True" ShowHeaderFilterButton="true" ShowFilterBar="Visible" ShowFilterRowMenu="true" UseFixedTableLayout="true"/>
															 <SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" />
															<GroupSummary>
																<dx:ASPxSummaryItem FieldName="Qty" ShowInColumn="Qty" ShowInGroupFooterColumn="Qty"
																	SummaryType="Sum" />
															</GroupSummary>
														</dx:ASPxGridView>
														
														<asp:SqlDataSource ID="ds_workorder_timeentries" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
															ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
															SelectCommand="CALL DS_WORKORDER_TIMEENTRIES(@woprog_id)">
															<SelectParameters>
																<asp:ControlParameter ControlID="hidWOProgID" Name="@woprog_id"
																	PropertyName="Value" />
															</SelectParameters>
														</asp:SqlDataSource>
													</td>
												</tr>
												<tr>
													<td style="border-style: solid; border-width: thin; text-align: left;"
														bgcolor="#FFFFCC" nowrap="nowrap">&nbsp;</td>
													<td class="current_payroll" style="width: 50%; text-align: left;">
														<strong>Indicates Entries from Other Branches</strong></td>
													<td></td>
													<td style="text-align: left;">
														<dx:ASPxButton ID="btn_export" runat="server" OnClick="btn_export_Click" Text="Export" Width="120px" ClientInstanceName="btn_export">
															<Image Url="~/images/icon/icon[excel].gif">
															</Image>
														</dx:ASPxButton>
														<dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="Grid_TimeEntries">
														</dx:ASPxGridViewExporter>
													</td>
												</tr>
											</table>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="History" ClientVisible="False" Name="history">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl7" runat="server">
											<dx:ASPxGridView ID="grid_History" runat="server" AutoGenerateColumns="False" Width="100%" DataSourceID="ds_history" Theme="NETheme01">
												<Columns>
													<dx:GridViewDataDateColumn Caption="Date" FieldName="date" VisibleIndex="0" Width="200px">
														<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom"></PropertiesDateEdit>
														<CellStyle Wrap="False">
														</CellStyle>
													</dx:GridViewDataDateColumn>
													<dx:GridViewDataTextColumn Caption="Action" FieldName="history" VisibleIndex="1" Width="250px">
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Employee" FieldName="member" VisibleIndex="2" Width="150px">
														<CellStyle Wrap="False">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Note" FieldName="note" VisibleIndex="3" Width="65%">
														<CellStyle Wrap="False">
														</CellStyle>
													</dx:GridViewDataTextColumn>
												</Columns>
												<SettingsPager PageSize="20">
													<AllButton Text="All">
													</AllButton>
													<NextPageButton Text="Next &gt;">
													</NextPageButton>
													<PrevPageButton Text="&lt; Prev">
													</PrevPageButton>
												</SettingsPager>

											</dx:ASPxGridView>
											<br />
											&nbsp;<dx:ASPxLabel ID="lbl_pct_to_invoice" runat="server" Font-Bold="True" ForeColor="Black">
											</dx:ASPxLabel>
											<asp:SqlDataSource ID="ds_history" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
												ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
												SelectCommand="Call proc_getwohistory (@woprog_id)">
												<SelectParameters>
													<asp:ControlParameter ControlID="hidWOProgID" Name="@woprog_id"
														PropertyName="Value" />
												</SelectParameters>
											</asp:SqlDataSource>
											<br />
											<br />
											<div id="div_part_history" runat="server">
												<b style='font-size: 13px;'>Distilled part history</b>
												<br />
												<dx:ASPxGridView ID="gv_distilled_history" runat="server" AutoGenerateColumns="True" DataSourceID="sds_distilled_history" Width="100%" Theme="NETheme01">
													<Columns>
													</Columns>
													<SettingsPager PageSize="50">
														
													</SettingsPager>
													<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" ShowGroupPanel="True" />
												</dx:ASPxGridView>
												<asp:SqlDataSource ID="sds_distilled_history" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																   ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																   SelectCommand="SELECT * FROM log.wo_detail_distilled WHERE woprog_id = @woprog_id ORDER BY detail_id, event_dt">
													<SelectParameters>
														<asp:ControlParameter ControlID="hidWOProgID" Name="@woprog_id"
																			  PropertyName="Value" />
													</SelectParameters>
												</asp:SqlDataSource>
												<b style='font-size: 13px;'>Complete part history</b>
												<br />
												<dx:ASPxGridView ID="gv_parthistory" runat="server" AutoGenerateColumns="False" KeyFieldName="id" OnHtmlDataCellPrepared="gv_parthistory_HtmlDataCellPrepared" OnHtmlRowPrepared="gv_parthistory_HtmlRowPrepared" OnDataBound="gv_parthistory_DataBound" OnAfterPerformCallback="gv_parthistory_DataBound" OnInit="gv_parthistory_DataBound" Width="100%" Theme="NETheme01">
													<Columns>
														<dx:GridViewDataTextColumn Caption="ID" FieldName="wo_detail_current_id" ShowInCustomizationForm="True" VisibleIndex="0">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Event" FieldName="event" ShowInCustomizationForm="True" VisibleIndex="1">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Old or New" FieldName="old_new" ShowInCustomizationForm="True" VisibleIndex="3">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Rec #" FieldName="wo_detail_current_rec_no" ShowInCustomizationForm="True" VisibleIndex="4">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Type" FieldName="wo_detail_current_type" ShowInCustomizationForm="True" VisibleIndex="5">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Description" FieldName="wo_detail_current_description" ShowInCustomizationForm="True" VisibleIndex="10">
															<CellStyle HorizontalAlign="Left" Wrap="False">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Comm Qty" FieldName="wo_detail_current_qty_committed" ShowInCustomizationForm="True" VisibleIndex="12">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Cost" FieldName="wo_detail_current_price_cost" ShowInCustomizationForm="True" VisibleIndex="13">
															<PropertiesTextEdit DisplayFormatString="{0:c2}"></PropertiesTextEdit>
															<Settings FilterMode="DisplayText" />
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Sell" FieldName="wo_detail_current_price_sell" ShowInCustomizationForm="True" VisibleIndex="14">
															<PropertiesTextEdit DisplayFormatString="{0:c2}"></PropertiesTextEdit>
															<Settings FilterMode="DisplayText" />
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Added By" FieldName="added_by" ShowInCustomizationForm="True" VisibleIndex="15">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Tax #1" FieldName="tax1" ShowInCustomizationForm="True" VisibleIndex="16">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Tax #2" FieldName="tax2" ShowInCustomizationForm="True" VisibleIndex="17">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Tax #3" FieldName="tax3" ShowInCustomizationForm="True" VisibleIndex="18">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Tax #4" FieldName="tax4" ShowInCustomizationForm="True" VisibleIndex="19">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Origin" FieldName="wo_detail_current_origin" ShowInCustomizationForm="True" VisibleIndex="20">
															<CellStyle HorizontalAlign="Left">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Billtype" FieldName="billtype" ShowInCustomizationForm="True" VisibleIndex="21">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Notes" FieldName="wo_detail_current_notes" ShowInCustomizationForm="True" VisibleIndex="22">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Req Qty" FieldName="wo_detail_current_qty_ordered" ShowInCustomizationForm="True" VisibleIndex="11">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Event DT" FieldName="ts" ShowInCustomizationForm="True" VisibleIndex="2">
															<PropertiesTextEdit DisplayFormatString="{0:yyyy-MM-dd HH:mm:ss}">
															</PropertiesTextEdit>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Date Added" FieldName="wo_detail_current_date_added" ShowInCustomizationForm="True" VisibleIndex="7">
															<PropertiesTextEdit DisplayFormatString="{0:yyyy-MM-dd HH:mm:ss}">
															</PropertiesTextEdit>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Date Modified" FieldName="wo_detail_current_date_modified" ShowInCustomizationForm="True" VisibleIndex="8">
															<PropertiesTextEdit DisplayFormatString="{0:yyyy-MM-dd HH:mm:ss}">
															</PropertiesTextEdit>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Date Required" FieldName="wo_detail_current_date_required" ShowInCustomizationForm="True" VisibleIndex="9">
															<PropertiesTextEdit DisplayFormatString="{0:yyyy-MM-dd HH:mm:ss}">
															</PropertiesTextEdit>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataHyperLinkColumn Caption="Master ID" FieldName="wo_detail_current_master_id" ShowInCustomizationForm="True" VisibleIndex="6">
															<PropertiesHyperLinkEdit NavigateUrlFormatString="/sections/member/inventory/index.aspx?a=get&amp;tab=G&amp;id={0}" Target="_blank">
															</PropertiesHyperLinkEdit>
														</dx:GridViewDataHyperLinkColumn>
													</Columns>
													<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" />
													<SettingsPager PageSize="50">
													</SettingsPager>
													<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" ShowGroupPanel="True" />
													<SettingsCookies CookiesID="gv_parthistory" />
													<Styles>
														<Cell HorizontalAlign="Center" Wrap="False">
														</Cell>
													</Styles>
												</dx:ASPxGridView>

												<u><b>Legend</b></u>
												<table style="" cellpadding="2" cellspacing="2">
													<tr>
														<td align="center" style="background-color: #cfc; border: solid 1px #777; color: #000;">Example</td>
														<td style="padding-left: 10px;">Inserted Row - This is the first version of the row data that is saved to the database.</td>
													</tr>
													<tr>
														<td align="center" style="background-color: #fff; border: solid 1px #777;">&nbsp;</td>
														<td style="padding-left: 10px;">Updated Row - When a row is updated in the work order's detail table, both the <b>Old</b> and the <b>New</b> values are retained.</td>
													</tr>
													<tr>
														<td align="center" style="background-color: #fff; color: #999; border: solid 1px #777;">Example</td>
														<td style="padding-left: 10px;">- <b>Old</b> Value</td>
													</tr>
													<tr>
														<td align="center" style="background-color: #fff; color: #090; font-weight: bold; border: solid 1px #777;">Example</td>
														<td style="padding-left: 10px;">- <b>New</b> Value</td>
													</tr>
													<tr>
														<td align="center" style="background-color: #fcc; border: solid 1px #777;">&nbsp;</td>
														<td style="padding-left: 10px;">Deleted Row - This could be a manual deletion, or when moving the rows over to the history table upon invoicing.</td>
													</tr>
												</table>
											</div>
											<br />
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Analysis" ClientVisible="False" Name="analysis">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl8" runat="server">
											<uc3:analysis ID="uc_analysis" runat="server" Visible="False" />
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="R&amp;D" ClientVisible="False" NewLine="True" name="rAndD">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl9" runat="server">
											<table width="100%">
												<tr>
													<td>
														<span style="color: dimgray; font-family: Arial">Uncertainty</span></td>
													<td style="width: 100%">
														<dx:ASPxMemo ID="mem_Uncertainty" runat="server" Height="71px" Width="100%" ClientInstanceName="mem_Uncertainty">
														</dx:ASPxMemo>
													</td>
													<td></td>
												</tr>
												<tr>
													<td>
														<span style="color: dimgray; font-family: Arial;">Technilogical Advancement</span></td>
													<td style="width: 100%">
														<dx:ASPxMemo ID="mem_Advancement" runat="server" Height="71px" Width="100%" ClientInstanceName="mem_Advancement">
														</dx:ASPxMemo>
													</td>
													<td></td>
												</tr>
												<tr>
													<td>
														<span style="color: dimgray; font-family: Arial">Related Core Competency</span></td>
													<td style="width: 100%">
														<dx:ASPxMemo ID="mem_Competancy" runat="server" Height="71px" Width="100%" ClientInstanceName="mem_Competancy">
														</dx:ASPxMemo>
													</td>
													<td></td>
												</tr>
												<tr>
													<td></td>
													<td style="width: 100%">
														<dx:ASPxButton ID="btnsaverdmemos" runat="server" ClientInstanceName="btnsaverdmemos"
															OnClick="btnsaverdmemos_Click" Text="Save">
														</dx:ASPxButton>
													</td>
													<td></td>
												</tr>
											</table>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Project Notes" ClientVisible="False" Name="projectNotes">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl10" runat="server">
											<table id="tbl_notes" style="font-size: 9pt; font-family: Arial" width="100%">
												<tr>
													<td style="width: 100%; color: gray">Project Notes:</td>
													<td></td>
													<td></td>
												</tr>
												<tr>
													<td style="width: 100%">
														<dx:ASPxMemo ID="mem_projectNotes" runat="server" Height="71px" Width="100%">
														</dx:ASPxMemo>
													</td>
													<td>
														<dx:ASPxButton ID="btn_SaveProjectNotes" runat="server" OnClick="btn_SaveProjectNotes_Click">
															<Image Url="~/images/icon/icon[save].gif">
															</Image>
														</dx:ASPxButton>
													</td>
													<td></td>
												</tr>
												<tr>
													<td style="width: 100%; color: gray">Processing Chat History:</td>
													<td></td>
													<td></td>
												</tr>
												<tr>
													<td style="width: 100%">
														<dx:ASPxMemo ID="mem_chatnewline" runat="server" Height="30px" Width="100%">
														</dx:ASPxMemo>
													</td>
													<td>
														<dx:ASPxButton ID="btn_AddnewComment" runat="server" OnClick="btn_AddnewComment_Click">
															<Image Url="~/images/icon/icon[save].gif">
															</Image>
														</dx:ASPxButton>
													</td>
													<td></td>
												</tr>
												<tr>
													<td style="width: 100%">
														<div id="divComments" runat="server" class="chatcontent" style="left: 7px; overflow: auto; width: 100%; top: 7px; height: 100%; table-layout: auto; color: gray; font-family: Arial;">
														</div>
													</td>
													<td></td>
													<td></td>
												</tr>
											</table>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Pictures" ClientVisible="False" Name="pictures">
									<TabImage Url="~/images/FullPicture.JPG">
									</TabImage>
									<ContentCollection>
										<dx:ContentControl ID="ContentControl11" runat="server">
											&nbsp;<table width="100%">
												<tr>
													<td></td>
													<td></td>
													<td></td>
												</tr>
												<tr>
													<td>
														<dx:ASPxUploadControl ID="ASPxUploadControl1" runat="server" ShowAddRemoveButtons="True"
															ShowProgressPanel="True" ShowUploadButton="True" Width="50%">
															<ValidationSettings MultiSelectionErrorText="Attention! 

The following {0} files are invalid because they exceed the allowed file size ({1}) or their extensions are not allowed. These files have been removed from selection, so they will not be uploaded. 

{2}">
															</ValidationSettings>
															<AddButton ImagePosition="Right" Text="">
																<Image Url="~/images/icon/icon[add].gif">
																</Image>
															</AddButton>
															<RemoveButton Text="">
																<Image Url="~/images/icon/icon[remove].gif">
																</Image>
															</RemoveButton>
															<UploadButton Text="">
																<Image Url="~/images/icon/icon[export].gif">
																</Image>
															</UploadButton>
															<AdvancedModeSettings>
																<FileListItemStyle CssClass="pending dxucFileListItem">
																</FileListItemStyle>
															</AdvancedModeSettings>
														</dx:ASPxUploadControl>
													</td>
													<td></td>
													<td></td>
												</tr>
												<tr>
													<td>
														<dx:ASPxDataView ID="dataview_pictures" runat="server" ColumnCount="5" RowPerPage="1"
															Width="100%">
															<SettingsFlowLayout ItemsPerPage="5" />
															<SettingsTableLayout ColumnCount="5" RowsPerPage="1" />
															<PagerSettings ShowNumericButtons="False">
															</PagerSettings>
															<ItemStyle Height="50px" Width="50px" />
														</dx:ASPxDataView>
													</td>
													<td></td>
													<td></td>
												</tr>
											</table>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Name="quoteComparison" Text="Quote Comparison"
									ClientVisible="False">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl13" runat="server">
											<iframe id="comparison" runat="server" scrolling="auto" width="99%" frameborder="0" height="900"></iframe>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Project Folder" Name="projectFolder">
									<ContentCollection>
										<dx:ContentControl ID="ContentContro14" runat="server">
											<iframe id="files_frame" runat="server" height="900" scrolling="no" width="1000" style="border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none" frameborder="0"></iframe>
										</dx:ContentControl>
									</ContentCollection>

								</dx:TabPage>
								<dx:TabPage Text="ER" ClientVisible="False" Name="er">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl15" runat="server" Height="900">
											<iframe id="ER_frame" runat="server" height="900" scrolling="no" width="1000" style="border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none" frameborder="0"></iframe>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Linked WOs" Name="linkedWos">
									<ContentCollection>
										<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
											<iframe id="if_associated" runat="server" height="1500" scrolling="yes" width="1000" style="border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none" frameborder="0"></iframe>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage ClientEnabled="True" Name="scope" Text="Scope">
									<ContentCollection>
										<dx:ContentControl runat="server" Width="1000px">
											<uc1:wo_tasklist ID="wo_tasklist1" runat="server" Visible="False" />
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Checklist" ClientVisible="False" Name="checklist">
									<ContentCollection>
										<dx:ContentControl ID="ContentControl14" runat="server">
											<uc:close_checklist runat="server" ID="close_checklist" Visible="False"  />
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Integration History" ClientVisible="False" Name="integrationHistory">
									<ContentCollection>
										<dx:ContentControl ID="ContentIntErr" runat="server">
											<table>
												<tr>
													<td><button type="button" onclick="gv_integ_history.ShowCustomizationWindow();">Toggle Columns</button></td>
													<td></td>
												</tr>
											</table>
											<dx:ASPxGridView ID="gv_integ_trans_history" runat="server" ClientInstanceName="gv_integ_history" Theme="NETheme01" DataSourceID="sdsIntegrationHistory" EnableTheming="True" SettingsBehavior-EnableCustomizationWindow="True" SettingsPager-PageSize="50">
												<SettingsBehavior EnableCustomizationWindow="True" EnableRowHotTrack="True" />
												<Columns>
													<dx:GridViewDataTextColumn Caption="Ops WO Id" FieldName="NESI Work Order Id"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="NS Job Internal Id" FieldName="NetSuite Job Internal Id"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="NS WO Internal Id" FieldName="NetSuite Work Order Internal Id"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="NS PO Internal Id" FieldName="NetSuite Purchase Order Internal Id"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="NS Customer Credit Memo Internal Id" FieldName="NetSuite Customer Credit Memo Internal Id"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="NS Invoice Internal Id" FieldName="NetSuite Invoice Internal Id"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Int. Workload" FieldName="Integration Workload"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Int. Status" FieldName="Integration Status"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Start Time" FieldName="Start Time"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Completion Time" FieldName="Completion Time"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Retry" FieldName="Retry Count"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Message Type" FieldName="Message Type"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Message" FieldName="Message" CellStyle-CssClass="integMsg">
														<CellStyle Wrap="True">
														</CellStyle>
													</dx:GridViewDataTextColumn>
												</Columns>
												<SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-Width="300px" CustomizationWindow-Height="350px"></SettingsPopup>
												<SettingsResizing ColumnResizeMode="Control" Visualization="Live">
												</SettingsResizing>
												<SettingsCookies Enabled="True" CookiesID="inthistory" StoreColumnsHierarchy="True" StoreFiltering="True" StoreColumnsWidth="True" StoreGroupingAndSorting="True" StoreControlWidth="True"></SettingsCookies>
											</dx:ASPxGridView>
											<asp:SqlDataSource runat="server" ID="sdsIntegrationHistory" SelectCommand="CALL ds_integration_history(@woprog_id,1)"  ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>">
												<SelectParameters>
													<asp:QueryStringParameter Type="Int32" QueryStringField="woprog_id" Name="@woprog_id" />
												</SelectParameters>
											</asp:SqlDataSource>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Name="assets" Text="Assets">
									<ContentCollection>
										<dx:ContentControl ID="assetsCC" runat="server">
											<dx:ASPxPageControl ID="pc_assets" runat="server" ActiveTabIndex="0" ClientInstanceName="pc_assets"
												BackColor="Transparent"
												EnableCallBacks="True" Width="100%"
												ClientVisible="True" Theme="NETheme01" TabPosition="Left">
												<TabPages>
													<dx:TabPage name="assignment" Text="Assignment">
														<ContentCollection >
															<dx:ContentControl ID="ContentControl34" runat="server">
																<div>
																	<table width="500" cellpadding="2" cellspacing="0">
																		<thead>
																			<tr>
																				<th>Asset</th>
																				<th>Date Needed?</th>
																				<th>Save</th>
																			</tr>
																		</thead>
																		<tbody>
																			<tr>
																				<td>
																					<dx:ASPxComboBox ID="ddlAssetAssignment" ValueType="System.Int32" runat="server" DataSourceId="sdsAssetAssignment" OnSelectedIndexChanged="ddlAssetAssignment_SelectedIndexChanged" AutoPostBack="true" TextField="name" ValueField="id" Theme="NETheme01">
																			
																					</dx:ASPxComboBox>
																		
																					<asp:SqlDataSource ID="sdsAssetAssignment" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																								SelectCommand="SELECT assets_id id, CONCAT(assets_id,' - ', description) NAME FROM neintranet.assets WHERE business_unit_id = @businessUnitId AND active = 1 AND requires_maintenance = 0 AND IF(needs_calib = true, next_calib_date > CURDATE() , true)">
																					</asp:SqlDataSource>
																				</td>
																				<td>
																					<dx:ASPxDateEdit ID="calAssetDate" runat="server" Theme="NETheme01" OnCalendarDayCellPrepared="calAssetDate_CalendarDayCellPrepared">
																					</dx:ASPxDateEdit>
																				</td>
																				<td>
																					<dx:ASPxButton id="btSaveAsset" runat="server" Text="Save" OnClick="btSaveAsset_Click" Theme="NETheme01">

																					</dx:ASPxButton>
																				</td>
																			</tr>
																			<tr>
																				<td colspan="3"><dx:ASPxLabel ID="lblAssignmentError" runat="server" ForeColor="Red"></dx:ASPxLabel></td>
																			</tr>
																		</tbody>
																	</table>
																</div>
																<dx:ASPxGridView ID="gv_assetAssignment" AutoGenerateColumns="False" runat="server" DatasourceId="sdsAssetAssignmentGv" Theme="NETheme01">
																	<Columns>
																		<dx:GridViewDataColumn Caption="Id" FieldName="id" VisibleIndex="0">
																		</dx:GridViewDataColumn>
																		<dx:GridViewDataColumn Caption="Asset" FieldName="asset" VisibleIndex="1">
																		</dx:GridViewDataColumn>
																		<dx:GridViewDataColumn Caption="Date Needed" FieldName="date_needed" VisibleIndex="2">
																		</dx:GridViewDataColumn>
																		<dx:GridViewDataColumn Caption="Added By" FieldName="added_by" VisibleIndex="3">
																		</dx:GridViewDataColumn>
																		<dx:GridViewCommandColumn Caption="Delete">
																		</dx:GridViewCommandColumn>
																	</Columns>
																	<SettingsPager PageSize="20">
																	</SettingsPager>
																</dx:ASPxGridView>
																<asp:SqlDataSource ID="sdsAssetAssignmentGv" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																			SelectCommand="SELECT a.id, CONCAT(b.assets_id,' - ', LEFT(b.description, 100)) asset, a.date_needed, a.date_needed, c.member_fullname added_by FROM woprog_asset a LEFT JOIN assets b ON a.asset_id = b.assets_id LEFT JOIN member c ON a.added_by = c.member_id WHERE woprog_id = @woprog_Id">
																</asp:SqlDataSource>
															</dx:ContentControl>
														</ContentCollection>
													</dx:TabPage>
													<dx:TabPage Name="usage" Text="Usage">
														<ContentCollection >
															<dx:ContentControl ID="ContentControl12" runat="server">
															
																<div>
																	<table cellpadding="2" cellspacing="0">
																		<thead>
																			<tr>
																				<th align="center">Asset</th>
																				<th align="center" width="100">Unit</th>
																				<th align="center" width="100">Quantity</th>
																				<th align="center" width="100">Sell / per</th>
																				<th align="center" width="100">Sell Extd</th>
																				<th align="center" width="100">Save</th>
																			</tr>
																		</thead>
																		<tbody>
																			<tr>
																				<td width="200">
																					<dx:ASPxComboBox ID="ddlAssetUsageAsset" ValueType="System.Int32" runat="server" DataSourceId="sdsAssetUsageAsset" TextField="name" ValueField="id" Theme="NETheme01" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetUsageAsset_SelectedIndexChanged">
																					
																						<ValidationSettings CausesValidation="true" ValidateOnLeave="true" Display="Dynamic">
																							<RequiredField IsRequired="true"/>
																						</ValidationSettings>
																					</dx:ASPxComboBox>
																					<asp:SqlDataSource ID="sdsAssetUsageAsset" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																								SelectCommand="SELECT a.id, CONCAT(a.asset_id,' - ', LEFT(b.description, 100)) name FROM woprog_asset a INNER JOIN assets b ON a.asset_id = b.assets_id WHERE woprog_id = @woprog_Id AND IF(b.needs_calib = true, b.next_calib_date > CURDATE() , true) GROUP BY a.asset_id">
																					</asp:SqlDataSource>
																				</td>
																				<td width="100" align="center">
																					<dx:ASPxComboBox ID="ddlAssetUsageUnit" runat="server" ValueField="System.Int32" Theme="NETheme01" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetUsageUnit_SelectedIndexChanged">
																						<Items>
																							<dx:ListEditItem Value="1" Text="Daily"></dx:ListEditItem>
																							<dx:ListEditItem Value="2" Text="Weekly"></dx:ListEditItem>
																							<dx:ListEditItem Value="3" Text="Monthly"></dx:ListEditItem>
																						</Items>
																						<ValidationSettings CausesValidation="true" ValidateOnLeave="true" Display="Dynamic">
																							<RequiredField IsRequired="true"/>
																						</ValidationSettings>
																					</dx:ASPxComboBox>
																				</td>
																				<td width="100" align="center">
																					<dx:ASPxTextBox ID="txtAssetUsageQuantity" runat="server" Theme="NETheme01" OnTextChanged="txtAssetUsageQuantity_TextChanged" AutoPostBack="true">
																						<MaskSettings Mask="<1..99999>.<0..99>" />
																						<ValidationSettings  CausesValidation="true" ValidateOnLeave="true" Display="Dynamic">
																							<RequiredField IsRequired="true"/>
																						</ValidationSettings>
																					</dx:ASPxTextBox>
																				</td>
																				<td width="100" align="center">
																					<dx:ASPxTextBox ID="txtAssetUsageSell" runat="server" Theme="NETheme01">
																						 <MaskSettings Mask="<1..99999>.<0..99>" />
																						<ValidationSettings CausesValidation="true" ValidateOnLeave="true"  Display="Dynamic">
																							<RequiredField IsRequired="true"/>
																						</ValidationSettings>
																					</dx:ASPxTextBox>
																				</td>
																				<td width="100" align="center">
																					<dx:ASPxLabel ID="lblAssetUsageSellExtd" runat="server"></dx:ASPxLabel>
																				</td>
																				<td width="50" align="center">
																					<dx:ASPxButton id="btAssetUsageSave" runat="server" Text="Save" OnClick="btAssetUsageSave_Click" CausesValidation="true">
																						
																					</dx:ASPxButton>
																				</td>
																			</tr>
																			<tr>
																				<td colspan="5"><dx:ASPxLabel ID="txtAssetUsageError" runat="server" ForeColor="Red" EncodeHtml="False"></dx:ASPxLabel></td>
																			</tr>
																		</table>
																		<br/>
																		<br/>
																			<dx:ASPxGridView ID="gvAssetUsage" 
                                                                                AutoGenerateColumns="False" 
                                                                                runat="server" 
																				KeyFieldName="id"
                                                                                DatasourceId="sdsAssetUsage" 
                                                                                OnCancelRowEditing="gvAssetUsage_CancelRowEditing" 
                                                                                OnRowUpdating="gvAssetUsage_RowUpdating" 
                                                                                Theme="NETheme01" 
																				OnInit="gvAssetUsage_DataBound"
                                                                                OnCommandButtonInitialize="gvAssetUsage_CommandButtonInitialize"
																				>
																				<Columns>
																					<dx:GridViewDataColumn Caption="Id" FieldName="id" VisibleIndex="0">
																						<EditFormSettings Visible="False" />
																					</dx:GridViewDataColumn>
																					<dx:GridViewDataDateColumn Caption="Date" FieldName="date_added" VisibleIndex="1">
																						 <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" />
																						<EditFormSettings Visible="False" />
																					</dx:GridViewDataDateColumn>
																					<dx:GridViewDataTextColumn Caption="Quantity" FieldName="qty" VisibleIndex="2">
																						<propertiestextedit DisplayFormatString="{0:N2}">
																							<MaskSettings Mask="<1..99999>.<0..99>" />
																							<ValidationSettings CausesValidation="true" ValidateOnLeave="true" >
																								<RequiredField IsRequired="true"/>
																							</ValidationSettings>
																						</PropertiesTextEdit>
																					</dx:GridViewDataTextColumn>
                                                                                        <dx:GridViewDataComboBoxColumn Caption="Unit" FieldName="unit_id" VisibleIndex="3">
                                                                                          <PropertiesComboBox TextField="unit" ValueField="unit_id" ValueType="System.Int32" TextFormatString="{0}">  
                                                                                        </PropertiesComboBox>  
                                                                                         <CellStyle VerticalAlign="Top">  
                                                                                         </CellStyle>  
																					</dx:GridViewDataComboBoxColumn>
																					<dx:GridViewDataTextColumn Caption="Sell Per" FieldName="sell" VisibleIndex="4">
																						<propertiestextedit DisplayFormatString="{0:C2}">
																							<MaskSettings Mask="<1..99999>.<0..99>" />
																							<ValidationSettings CausesValidation="true" ValidateOnLeave="true" >
																								<RequiredField IsRequired="true"/>
																							</ValidationSettings>
																						</PropertiesTextEdit>
																					</dx:GridViewDataTextColumn>
																					<dx:GridViewDataTextColumn Caption="Description" FieldName="description" VisibleIndex="5">
																						<propertiestextedit>
																							<ValidationSettings CausesValidation="true" ValidateOnLeave="true" >
																								<RequiredField IsRequired="true"/>
																							</ValidationSettings>
																						</PropertiesTextEdit>
																					</dx:GridViewDataTextColumn>
                                                                                     <dx:GridViewDataComboBoxColumn Caption="Bill Type" FieldName="billtype_id" VisibleIndex="6">
                                                                                        <PropertiesComboBox TextField="billtype" ValueField="billtype_id" ValueType="System.Int32" TextFormatString="{0}">  
                                                                                        </PropertiesComboBox>  
                                                                                         <CellStyle VerticalAlign="Top">  
                                                                                         </CellStyle>  
																					</dx:GridViewDataComboBoxColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="origin" FieldName="origin" VisibleIndex="7" Visible="false" >
                                                                                       
																					</dx:GridViewDataTextColumn>
																					  <dx:GridViewDataTextColumn Caption="Activity Code" FieldName="activity_code" VisibleIndex="8" Visible="false" >
                                                                                      <PropertiesTextEdit MaxLength ="100">
																						
                                                                                      </PropertiesTextEdit>
																					</dx:GridViewDataTextColumn>
																					  <dx:GridViewDataTextColumn Caption="Cost Element" FieldName="cost_element" VisibleIndex="9" Visible="false" >
                                                                                            <PropertiesTextEdit MaxLength ="15">
																						
                                                                                      </PropertiesTextEdit>
																					</dx:GridViewDataTextColumn>
																					  <dx:GridViewDataTextColumn Caption="Client WO" FieldName="client_wo" VisibleIndex="10" Visible="false" >
                                                                                            <PropertiesTextEdit MaxLength ="10">
																						
                                                                                      </PropertiesTextEdit>
																					</dx:GridViewDataTextColumn>
																					  <dx:GridViewDataTextColumn Caption="Client PO" FieldName="client_po" VisibleIndex="11" Visible="false" >
                                                                                            <PropertiesTextEdit MaxLength ="10">
																						
                                                                                      </PropertiesTextEdit>
																					</dx:GridViewDataTextColumn>

																					<dx:GridViewCommandColumn ButtonType="Image" Caption="Action" Tooltip="Buttons"  Width="40px" MinWidth="75" VisibleIndex="12" ShowEditButton="true" ShowDeleteButton="true" ShowUpdateButton="true" ShowCancelButton="true" ShowClearFilterButton="true"  > </dx:GridViewCommandColumn>
																				</Columns>
																				<SettingsCommandButton>
																						<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px" >
																							<Image Height="16px" Url="~/images/icon/icon[edit].gif" Width="16px">
																							</Image>
																						</EditButton>
																						<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px">
																							<Image Height="16px" Url="~/images/icon/icon[delete].gif" Width="16px">
																							</Image>
																						</DeleteButton>
																						<UpdateButton Text="Update" Image-Url="~/images/icon/icon[save].gif" Image-Width="16px" Image-Height="16px">
																							<Image Height="16px" Url="~/images/icon/icon[save].gif" Width="16px">
																							</Image>
																						</UpdateButton>
																						<CancelButton Text="Cancel" Image-Url="~/images/icon/icon[cancel].gif" Image-Width="16px" Image-Height="16px">
																							<Image Height="16px" Url="~/images/icon/icon[cancel].gif" Width="16px">
																							</Image>
																						</CancelButton>
																				</SettingsCommandButton>
																				<SettingsPager PageSize="30">
																				</SettingsPager>
																			</dx:ASPxGridView>
																			<asp:SqlDataSource ID="sdsAssetUsage" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																						ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																						SelectCommand="" >
																			</asp:SqlDataSource>
																</div>

															</dx:ContentControl>
														</ContentCollection>
														
													</dx:TabPage>
												</TabPages>
										</dx:ASPxPageControl>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
							</TabPages>
							<SettingsLoadingPanel Text="" />
							<LoadingPanelImage Url="../../images/loading_panel.gif">
							</LoadingPanelImage>

							<ClientSideEvents Init="function(s, e) {
	s.SetVisible(true);
}" />


						</dx:ASPxPageControl>

					</td>
				</tr>
			</table>
			<div id="divButtons" runat="server"></div>

			<div id="divOpenPO" runat="server"></div>
			<asp:UpdateProgress ID="UPDATEPROGRESS1" runat="server" DisplayAfter="100"
				AssociatedUpdatePanelID="UpdatePanel1">
				<ProgressTemplate>
					<div style="position: absolute; left: 50%; bottom: 46%">
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>
			<dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="ASPxPopupControl1"
				EnableClientSideAPI="True" HeaderText="" Height="75px" Modal="True" PopupHorizontalAlign="WindowCenter"
				PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ShowHeader="False"
				Width="600px" ShowPageScrollbarWhenModal="True">
				<ClientSideEvents CloseUp="function(s, e) {                
	            if(modalResult == true)
	            {	                
	                txtRDResult.SetText('1');
	            }
}" />
				<Border BorderColor="Gray" BorderStyle="Solid" BorderWidth="3px"></Border>
				<ClientSideEvents CloseUp="function(s, e) {                
	            if(modalResult == true)
	            {	                
	                txtRDResult.SetText('1');
	            }
}" />
				<HeaderStyle BackColor="DodgerBlue" />
				<ModalBackgroundStyle Opacity="0">
				</ModalBackgroundStyle>
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
						<table width="100%">
							<tr>
								<td colspan="1"></td>
								<td colspan="3">
									<br />
									<dx:ASPxLabel ID="txtRDQuestion" runat="server" ClientInstanceName="txtRDQuestion"
										Font-Names="Arial" Font-Size="10pt" Width="100%" Wrap="True">
									</dx:ASPxLabel>
									<dx:ASPxTextBox ID="txtRDResult" runat="server" ClientInstanceName="txtRDResult"
										ClientVisible="False" Width="170px">
									</dx:ASPxTextBox>
									<dx:ASPxPanel ID="pnlRD" runat="server" ClientInstanceName="pnlRD" ClientVisible="False"
										Width="100%">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent8" runat="server">
												What is the uncertainty in the project?<br />
												<dx:ASPxMemo ID="memUncertainty" runat="server" ClientInstanceName="memUncertainty"
													Height="71px" Width="600px">
												</dx:ASPxMemo>
												<br />
												What is the Technological Advancement?<br />
												<dx:ASPxMemo ID="memAdvancement" runat="server" ClientInstanceName="memAdvancement"
													Height="71px" Width="600px">
												</dx:ASPxMemo>
												<br />
												What is the related core competency currently possessed by your branch?<br />
												<dx:ASPxMemo ID="memCoreCompetency" runat="server" ClientInstanceName="memCoreCompetency"
													Height="71px" Width="100%">
												</dx:ASPxMemo>
												<br />
												<dx:ASPxButton ID="btnSaveRD" runat="server" AutoPostBack="False" ClientInstanceName="btnSaveRD"
													Text="Save">
													<ClientSideEvents Click="function(s, e) {	closePopup1();}" />
												</dx:ASPxButton>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxPanel>
								</td>
								<td colspan="1"></td>
							</tr>
							<tr>
								<td colspan="5" style="text-align: center">
									<dx:ASPxPanel ID="pnlrdYesNo" runat="server" ClientInstanceName="pnlrdYesNo" Width="100%">
										<PanelCollection>
											<dx:PanelContent ID="PanelContent9" runat="server">
												<asp:Button ID="Button2" runat="server" BackColor="#E0E0E0" OnClientClick="closePopup(true); return false;"
													Text="Yes" Width="65px" />
												&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button ID="Button3"
													runat="server" BackColor="#E0E0E0" OnClientClick="closePopup(false); return false;"
													Text="No" Width="65px" />
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxPanel>
									&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
								</td>
							</tr>
						</table>
					</dx:PopupControlContentControl>
				</ContentCollection>
				<Border BorderColor="Gray" BorderStyle="Solid" BorderWidth="3px" />
			</dx:ASPxPopupControl>
			<br />


			<!--Display Work Order Infomation-->
			<asp:Label ID="errorlabel" runat="server" Text="" Font-Bold="True" ForeColor="#C00000"></asp:Label>
			&nbsp;<br />
			<asp:SqlDataSource ID="MySqlDataSourceBillType" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
				SelectCommand="SELECT wo_lineitem_billtypeid as billtype_id, wo_lineitem_billtype_name as billtype from wo_detail_lineitem_billtype ORDER BY wo_lineitem_billtypeid"></asp:SqlDataSource>
			<div id="divButtons2" runat="server"></div>
			&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;<br />
			<dx:ASPxPopupControl ID="ASPxpuNotes" runat="server" ClientInstanceName="NotesPopUp"
				CloseAction="CloseButton" HeaderText="Notes" Modal="True" PopupHorizontalAlign="WindowCenter"
				PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True" AppearAfter="50" DisappearAfter="50">
				<ClientSideEvents Closing="function(s, e) {
	     document.getElementById('ctl00_cphMasterBody_ASPxpuNotes_textNotes').value = '';
}" />
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
						<asp:TextBox ID="textNotes" runat="server" CssClass="textNotes" Height="100px" Rows="4"
							TextMode="MultiLine" Width="385px"></asp:TextBox>
						&nbsp;<br />
						<asp:ImageButton ID="imgbnotesupdate" runat="server" CssClass="imgbnotesupdate" Height="20px"
							ImageUrl="~/images/icon/icon[save].gif" OnClick="imgbnotesupdate_Click" ToolTip="Save"
							Width="21px" />
						&nbsp;
					</dx:PopupControlContentControl>
				</ContentCollection>
				<HeaderStyle BackColor="Gray" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
					ForeColor="White" />
				<HeaderImage Url="~/images/FullNotes.JPG">
				</HeaderImage>
			</dx:ASPxPopupControl>


			<dx:ASPxCallbackPanel ID="cb_contact" runat="server"
				ClientInstanceName="cb_contact" OnCallback="cb_contact_Callback" Width="200px">
				<ClientSideEvents EndCallback="function(s, e) {
	
            ddlContact.SetValue($('.hid_newcontact_id').val());
	ddlContact.SetText(tbnewcontact.GetText());
	$('html').css({'overflow':'auto'});
}" />
				<PanelCollection>
					<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
						<br />
						<input type="hidden" id="hid_newcontact_id" class="hid_newcontact_id" runat="server" />

						<input type="hidden" id="defaultTM" class="defaultTM" runat="server" />
						<input type="hidden" id="defaultQuoted" class="defaultQuoted" runat="server" />

						<dx:ASPxPopupControl ID="popc" runat="server" AppearAfter="0"
							ClientInstanceName="popc" CloseAction="CloseButton" HeaderText="New Contact"
							Modal="True" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter"
							PopupVerticalAlign="WindowCenter" Width="400px" Theme="NETheme01">

							<ModalBackgroundStyle Opacity="0">
							</ModalBackgroundStyle>
							<ContentCollection>
								<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
									<table style="width: 100%;">
										<tr>
											<td>Name:</td>
											<td nowrap="nowrap" style="white-space: nowrap">
												<dx:ASPxTextBox ID="tbnewcontact" runat="server"
													ClientInstanceName="tbnewcontact" NullText="Enter Name" Width="100%">
													<NullTextStyle Font-Italic="True" ForeColor="#666666">
													</NullTextStyle>
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td>Title:</td>
											<td>
												<dx:ASPxComboBox ID="ddltitle" runat="server" DataSourceID="SqlDataSource6"
													TextField="title_name" ValueField="title_id" ValueType="System.Int32" Native="true">
												</dx:ASPxComboBox>
												<asp:SqlDataSource ID="SqlDataSource6" runat="server"
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
													SelectCommand="Select * from titles"></asp:SqlDataSource>
											</td>
										</tr>
										<tr>
											<td>Email:</td>
											<td>
												<dx:ASPxTextBox ID="txtcontactemail" runat="server" Width="170px">
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td nowrap="nowrap">Cell Phone:</td>
											<td>
												<dx:ASPxTextBox ID="txtcellphone" runat="server" Width="170px">
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td>
												<dx:ASPxButton ID="btnaddcontact" runat="server" AutoPostBack="False"
													Text="Add">
													<ClientSideEvents Click="function(s, e) {
	cb_contact.PerformCallback(ddlCustomer.GetValue());
	ddlContact.PerformCallback(ddlCustomer.GetValue());
	ddlContact.SetText(tbnewcontact.GetText());
                                    ddlContact.SetValue($('.hid_newcontact_id').val());

}" />
												</dx:ASPxButton>
											</td>
											<td>&nbsp;</td>
										</tr>
									</table>
								</dx:PopupControlContentControl>
							</ContentCollection>
						</dx:ASPxPopupControl>
					</dx:PanelContent>
				</PanelCollection>
			</dx:ASPxCallbackPanel>
            <div>
          <dx:ASPxPopupControl ID="popReassignReason" ClientInstanceName="popReassignReason" runat="server" AllowDragging="True" AllowResize="True" CloseAction="CloseButton" Height="100%" Width="600px" HeaderText="Close & Reassigning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="false" AnimationType="None" Theme="NETheme01">
				
				<ContentCollection>
					<dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
						<br />
                          <dx:ASPxCallbackPanel ID="CallbackPanel" ClientInstanceName="CallbackPanel" runat="server" 
                               Height="100%" Width="600px"  OnCallback="popReassignReason_CallbackPanel_Callback" RenderMode="Table" ScrollBars="Vertical">
                                <ClientSideEvents BeginCallback="OnBeginCallback" EndCallback="OnEndCallback"></ClientSideEvents>
                         <PanelCollection>
                             <dx:PanelContent runat="server">
                        		<table style="width: 100%;">
							<tr>
                                <td colspan="2" height="30px"><asp:Label ID="lbl_Customer" runat="server" Text="Customer:" Width="150px" ></asp:Label></td>
								<td colspan="3">
                                       <dx:ASPxComboBox ID="ddl_Customer" runat="server" CallbackPageSize="50"  IncrementalFilteringMode="Contains"
													                  ForeColor="Black" TextField="Customer_Name" 
																	  ValueField="Customer_ID" ValueType="System.Int32" 
																	  DropDownRows="5" EnableCallbackMode="True" OnDataBound="cb_DataBound"
																	  AnimationType="None" ClientInstanceName="ddl_Customer"  
																	  Font-Names="Arial" Width="100%" Theme="NETheme01">  
                                                           <SettingsLoadingPanel Text="Loading..." />
                                                           <ClientSideEvents  SelectedIndexChanged="OnListBoxIndexChanged" />
                                                         </dx:ASPxComboBox> 
								</td>
							</tr>
						
							<tr>
                                <td colspan="2" height="30px"><asp:Label ID="lbl_Address" runat="server" Text="Address:" Width="150px" ></asp:Label></td>
								<td colspan="3">
                                    <dx:ASPxComboBox ID="ddl_Address" runat="server" Theme="NETheme01" ForeColor="Black" DataSourceID="sdsaddress" IncrementalFilteringMode="Contains"  TextField="address" ValueField="address_id" ValueType="System.Int32" Width="100%" EnableCallbackMode="True" ClientInstanceName="ddl_Address" EnableSynchronization="False" OnCallback="ddl_Address_Callback" DropDownRows="20" OnDataBound="cb_DataBound" AnimationType="None" Font-Names="Arial">
										<ButtonStyle ForeColor="White">
										</ButtonStyle>
                                
									</dx:ASPxComboBox>
                                       <asp:SqlDataSource ID="sdsaddress" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
                                                 SelectCommand="Select  0 as address_id,'Not Applicable' as address union Select address_id,address as address from vw_address where cust_id = @custid AND active = 1">
																					<SelectParameters>
																						<asp:ControlParameter ControlID="ddl_Customer" Name="@custid" PropertyName="Value" />
																					</SelectParameters>
																				</asp:SqlDataSource>
								</td>
							</tr>
                          
                             <tr>
							
                                <td colspan="2" height="30px"><asp:Label ID="lbl_Contact" runat="server" Text="Contact:" Width="150px" ></asp:Label></td>
								<td colspan="3">
                                   	<dx:ASPxComboBox ID="ddl_Contact" runat="server" Theme="NETheme01"  ForeColor="Black" DataSourceID="sdscontact" IncrementalFilteringMode="Contains"  TextField="Contact_Name" ValueField="Contact_ID" ValueType="System.Int32" Width="100%" EnableCallbackMode="True" ClientInstanceName="ddl_Contact" EnableSynchronization="False" OnCallback="ddl_Contact_Callback" DropDownRows="20" OnDataBound="cb_DataBound" AnimationType="None" Font-Names="Arial">
										<ButtonStyle ForeColor="White">
										</ButtonStyle>
                                       
									</dx:ASPxComboBox>
									<asp:SqlDataSource ID="sdscontact" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																					SelectCommand="SELECT 0 contact_id, 'Please select contact' contact_name UNION ALL (SELECT contact_id, contact_name FROM contact WHERE contact_type = 'Customer' AND contact_cust_id= @cust_id AND contact_status = 'Active' ORDER BY contact_name)">
																					<SelectParameters>
																						<asp:ControlParameter ControlID="ddl_Customer" Name="@cust_id" PropertyName="Value" />
																					</SelectParameters>
																				</asp:SqlDataSource>
								</td>
							</tr>
                                    <tr>
                                        <td colspan="2" height="30px"><asp:Label ID="lbl_PONum" runat="server" Text="PO:" Width="150px" ></asp:Label></td>
								        <td colspan="3">
                                               <dx:ASPxTextBox ID="txt_PONum" ClientInstanceName="txt_PONum" runat="server" Theme="NETheme01"  Width="100%" Font-Names="Arial" Font-Size="9pt" EnableViewState="False">
                                            
                                          <ClientSideEvents GotFocus="function(s, e) { s.SelectAll() }" />
                                      </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                       <tr>
                                        <td colspan="2" height="30px"><asp:Label ID="lbl_CloseReassign" runat="server" Text="Reason for reassigning:" Width="150px"  ></asp:Label></td>
								        <td colspan="3">
                                                                   <dx:ASPxMemo ID="txtCloseReassign"  Theme="NETheme01"  runat="server" ClientInstanceName="txtCloseReassign"
																		ForeColor="Black" Height="50px"
																		NullText="Reason for reassigning"
																		Width="100%" Native="True" Font-Names="Arial">
																		<NullTextStyle ForeColor="Silver" Font-Italic="True">
																		</NullTextStyle>
																		<Border BorderColor="Silver" BorderStyle="Solid" />
																		<Border BorderColor="Silver" BorderStyle="Solid"></Border>
																	</dx:ASPxMemo>
                                        </td>
                                    </tr>
                            <tr>
                                <td colspan="4"></td>
                                <td style="text-align:right">
                                 <dx:ASPxButton ID="btn_Save" runat="server" ClientInstanceName="btnCloseReassigningSave"  Text="cut workorder" Width="150px" OnClick="popCloseReassigningSave_Click" Theme="NETheme01">   
                                     <ClientSideEvents Click="OnSaveClient_Click"></ClientSideEvents>
							    </dx:ASPxButton>
                                </td>
                              
                                
                            </tr>
                           
						</table>

                       </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
						
					</dx:PopupControlContentControl>
				</ContentCollection>
				<HeaderStyle>
					<Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
				</HeaderStyle>
			</dx:ASPxPopupControl>
            </div>
			&nbsp;<br />
			<iframe id="downloader" width="1" height="1" frameborder="0"></iframe>
			<asp:HiddenField ID="hidWOProgID" runat="server" />
			<asp:HiddenField ID="hidWOBVWO" runat="server" />
			&nbsp;
            <asp:HiddenField ID="hidCompanyID" runat="server" />
			<asp:HiddenField ID="hidCompanyDSN" runat="server" />
			<asp:HiddenField ID="Commentstring" Value="" runat="server" />
			&nbsp;
            <asp:HiddenField ID="hidFromWO" Value="" runat="server" />
			&nbsp; &nbsp;&nbsp;
            <asp:HiddenField ID="hidWOStatus" Value="" runat="server" />
			<dx:ASPxHiddenField ID="hdnRD" runat="server" ClientInstanceName="hdnRD">
			</dx:ASPxHiddenField>
			<asp:HiddenField ID="hidWOAssociateNum" runat="server" />
			<br />
			<asp:HiddenField ID="hdnQuotedPrice" runat="server" />
			<asp:HiddenField ID="hidOrigin" runat="server" />
			<asp:HiddenField ID="hidNotesID" runat="server" />
			<asp:HiddenField ID="hidNotes" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>


</asp:Content>
