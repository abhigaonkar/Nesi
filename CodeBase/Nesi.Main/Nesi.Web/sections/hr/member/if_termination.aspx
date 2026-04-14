<%@ Page Title="" Language="C#" EnableTheming = "True" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_hr_member_if_termination" Theme="NETheme01" Codebehind="if_termination.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register src="modules/uc_responsibility.ascx" tagname="responsibility" tagprefix="uc" %>





<%@ Register src="modules/uc_member_usage.ascx" tagname="uc_member_usage" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header_placeholder" Runat="Server">
	<style type="text/css">
		.assign_lbl	
			{
			display:			block;
			font-weight:		bold;
			}
		.status_lba
			{
			display:			inline-block;
			width:				100px;
			}
		.active { color:#090; }
		.inactive { color: #900; }
		.style1
		{
			height: 29px;
			text-align: center;
		}
		.style2
		{
			font-family: Calibri, Arial, Helvetica, sans-serif;
			font-size: 14px;
		}
		.style3
		{
			font-size: small;
		}
		.style4
		{
			font-family: Calibri,Arial, Helvetica, sans-serif;
			font-size: 14px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	
	<br />
	<script type="text/javascript">
	
	$(document).ready(function()
		{
	//	Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
	//	Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
		});
	function EndReqHandler()
		{
		please_wait("stop");
		}
	function BeginReqHandler()
		{
		please_wait("start");
	}

	function ccc(obj, a, b,c,d) {
//javascript:alert(gv_chklist1.IsRowSelectedOnPage());
	var this_parent		= $(obj).parents(".dxgv:first");
alert(this_parent.attr("id"));
	if (b=="0")
	 { 
//gv_chklist1.PerformCallback(a+'|c|' + c); 
	 } 
	 else 
	 {
//gv_chklist1.PerformCallback(a+'|c|' + c + '|' + d); 
	 } 

	}

	</script>
	<div style="padding: 20px">
	<div style="font-family: Arial, Helvetica, sans-serif"><b class="status_lb">Status in NESI:</b><b runat="server" id="b_nesistatus"></b></div>
	<div class="style4"><b class="status_lb">HR Status:</b><b runat="server" id="b_hrstatus"></b></div>
	<hr />
	
				<table style="width:100%; height:auto;">
					<tr>
						<td width="375" class="style2">
							<b style="margin-left:0px;"><span class="style2">Employee Name:</span></b><span 
								class="style3"></b></span></td>
						<td>
							<dx:ASPxLabel ID="lb_employee_name" runat="server" Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style2"><b style="margin-left:0px;">
							Business Unit:</b></td>
						<td>
							<dx:ASPxLabel ID="lbl_branch" runat="server" Theme="NETheme01">
							</dx:ASPxLabel></td>
					</tr>
					<tr>
						<td class="style2">
							<b style="margin-left:0px;">Postition:</b></td>
						<td>
							<dx:ASPxLabel ID="lb_position" runat="server" Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style2">
							<b style="margin-left:0px;">Termination Date:</b></td>
						<td>
							<dx:ASPxDateEdit ID="date_termination" runat="server" EditFormat="Custom" 
								EditFormatString="yyyy-MM-dd" Theme="NETheme01">
							</dx:ASPxDateEdit>
						</td>
					</tr>
					<tr>
						<td class="style2">
							<b style="margin-left:0px;">Termination Time:</b></td>
						<td>
							<dx:ASPxTimeEdit ID="time_term" runat="server" Theme="NETheme01">
							</dx:ASPxTimeEdit>
						</td>
					</tr>
				<tr>
				    <td class="style2">
				        <b style="margin-left:0px;">Last Date Worked:</b></td>
				    <td>
				        <dx:ASPxDateEdit ID="dte_last_date_worked" runat="server" EditFormat="Custom" 
				                         EditFormatString="yyyy-MM-dd" Theme="NETheme01">
				        </dx:ASPxDateEdit>
				    </td>
				</tr>
					<tr>
						<td class="style2">
							<b style="margin-left:0px;">Termination Reason (actual):</b></td>
						<td>
							<dx:ASPxTextBox ID="tb_reason_actual" runat="server" Width="250px" 
								Theme="NETheme01">
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td class="style2">
							<b style="margin-left:0px;">Termination Reason (ROE):</b></td>
						<td>
							<dx:ASPxComboBox ID="ddl_roe" runat="server" Theme="NETheme01">
								<Items>
									<dx:ListEditItem Text="Shortage of work (layoff)" 
										Value="Shortage of work (layoff)" />
									<dx:ListEditItem Text="Return to school" Value="Return to school" />
									<dx:ListEditItem Text="Resigned or Quit" Value="Resigned or Quit" />
									<dx:ListEditItem Text="Retirement" Value="Retirement" />
									<dx:ListEditItem Text="Maternity" Value="Maternity" />
									<dx:ListEditItem Text="Work Sharing" Value="Work Sharing" />
									<dx:ListEditItem Text="Dismissal" Value="Dismissal" />
									<dx:ListEditItem Text="Apprentice Training" Value="Apprentice Training" />
									<dx:ListEditItem Text="Leave of Absence" Value="Leave of Absence" />
									<dx:ListEditItem Text="Parental" Value="Parental" />
									<dx:ListEditItem Text="Compassionate Care" Value="Compassionate Care" />
								</Items>
							</dx:ASPxComboBox>
						</td>
					</tr>
					<tr>
						<td class="style2">
							<b style="margin-left:0px;">Will Employee be Returning?</b></td>
						<td>
							<dx:ASPxRadioButtonList ID="rbl_returning" runat="server" 
								RepeatDirection="Horizontal" AutoPostBack="True" 
								onselectedindexchanged="rbl_returning_SelectedIndexChanged" 
								ValueType="System.Int32" Theme="NETheme01">
								<Paddings Padding="0px" />
								<Items>
									<dx:ListEditItem Text="Yes" Value="1" />
									<dx:ListEditItem Text="No" Value="0" />
								</Items>
								<Border BorderWidth="0px" />
							</dx:ASPxRadioButtonList>
						</td>
					</tr>
					<tr id="tr_return_date" runat="server" visible="false">
						<td class="style2">
							<b style="margin-left:10px;">Return Date?:</b></td>
						<td>
							<dx:ASPxDateEdit ID="date_return" runat="server" EditFormat="Custom" 
								EditFormatString="yyyy-MM-dd" Theme="NETheme01">
							</dx:ASPxDateEdit>
						</td>
					</tr>
					<tr id="tr_return_vacbank" runat="server" visible="false">
						<td class="style2">
							<b>Are they requesting their vacation or banked hours?:</b></td>
						<td>
							<dx:ASPxRadioButtonList ID="rbl_vac_bank" runat="server" 
								RepeatDirection="Horizontal" AutoPostBack="True" 
								onselectedindexchanged="rbl_vac_bank_SelectedIndexChanged" 
								ValueType="System.Int32" Theme="NETheme01">
								<Paddings Padding="0px" />
								<Items>
									<dx:ListEditItem Text="Yes" Value="1" />
									<dx:ListEditItem Text="No" Value="0" />
								</Items>
								<Border BorderWidth="0px" />
							</dx:ASPxRadioButtonList>
						</td>
					</tr>
					<tr id="tr_return_vacbank_details" runat="server" visible="false">
						<td valign="top" class="style2">
							<b style="margin-left:10px;">Please provide details:</b></td>
						<td>
							<dx:ASPxMemo ID="memo_vac_bank" runat="server" Height="71px" Width="100%" 
								Theme="NETheme01">
							</dx:ASPxMemo>
						</td>
					</tr>
					<tr>
						<td class="style2">
							<b style="margin-left:0px;">Has employee returned all company property?</b></td>
						<td>
							<dx:ASPxRadioButtonList ID="rbl_property" runat="server" 
								RepeatDirection="Horizontal" AutoPostBack="True" 
								onselectedindexchanged="rbl_property_SelectedIndexChanged" 
								ValueType="System.Int32" Theme="NETheme01">
								<Paddings Padding="0px" />
								<Items>
									<dx:ListEditItem Text="Yes" Value="1" />
									<dx:ListEditItem Text="No" Value="0" />
								</Items>
								<Border BorderWidth="0px" />
							</dx:ASPxRadioButtonList>
						</td>
					</tr>
					<tr id="tr_property_estimation" runat="server" visible="false">
						<td class="style2">
							<b style="margin-left:10px; ">Estimated value of any company property not returned:</b></td>
						<td>
							<dx:ASPxTextBox ID="tb_property" runat="server" Width="170px" Height="19px" 
								Theme="NETheme01">
								<MaskSettings Mask="$&lt;0..99999g&gt;.&lt;00..99&gt;" ShowHints="True" />
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
					<td colspan="2" align="left" class="style1">
							
							<dx:ASPxLabel ID="lb_status" EncodeHtml="false" runat="server" 
								style="font-weight: 700" Theme="NETheme01">
							</dx:ASPxLabel>
							
						</td>
					</tr>
					<tr>
					<td colspan="2" align="left">
							
							<dx:ASPxButton ID="button_save" runat="server" onclick="button_save_Click" 
								style="height: 25px" Text="Save" Theme="NETheme01">
								<ClientSideEvents Click="function(s, e) {
//	if($(&quot;.assign_ddl&quot;).size() != 0)
//		{
//		alert(&quot;Before you can save, you must first reassign all work orders, quotes, customers, purchase orders and subordinate employees&quot;);
//		e.processOnServer = false;
//		}
	 if($(&quot;.mandatory&quot;).size() != 0)
		{
		$(&quot;body&quot;).find(&quot;.mandatory&quot;).each(function()
			{
			if($(this).val() == &quot;&quot;)
				{
				var tb_name		= $(this).attr('data-name');
				alert(&quot;Please supply a value for '&quot;+tb_name+&quot;'&quot;);
				$(this).focus();
				e.processOnServer = false;
				return false;
				}
			});
		}

}" />
							</dx:ASPxButton>
							
						</td>
					</tr>
					<tr>
						<td align="left" colspan="2">
							
							<dx:ASPxPageControl ID="pc" runat="server" ActiveTabIndex="0" 
								AutoPostBack="True" ClientInstanceName="pc" 
								onactivetabchanged="pc_ActiveTabChanged" Theme="NETheme01" Width="99%">
								<TabPages>
									<dx:TabPage Text="Checklist">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<dx:ASPxGridView ID="gv_chklist1" runat="server" AutoGenerateColumns="False" 
													ClientInstanceName="gv_chklist1" KeyFieldName="id" 
													OnCustomCallback="gv_chklist_CustomCallback" 
													OnHtmlRowPrepared="gv_chklist1_HtmlRowCreated">
													<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_refresh==&quot;1&quot;)
	{
		alert('Good job!, you were the final person to finish their checklists! This person has now been set to PAST status');


window.opener.location.href = window.opener.location.href;


	}
}" />
													<Columns>
														<dx:GridViewCommandColumn SelectAllCheckboxMode="Page" 
															ShowInCustomizationForm="True" ShowSelectCheckbox="True" VisibleIndex="0">
															<FooterTemplate>
																<dx:ASPxButton ID="btnsave" runat="server" Text="Save" Theme="NETheme01" 
																	AutoPostBack="False" ClientInstanceName="btnsave">
																	<ClientSideEvents Click="function(s, e) {
	gv_chklist1.PerformCallback('saveall');
}" />
																</dx:ASPxButton>
															</FooterTemplate>
														</dx:GridViewCommandColumn>
														<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
															ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Item" FieldName="field" 
															ShowInCustomizationForm="True" VisibleIndex="3" Width="30%">
															<CellStyle Wrap="True">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn FieldName="resp_req" ShowInCustomizationForm="True" 
															Visible="False" VisibleIndex="2">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Comments" FieldName="comment" 
															ShowInCustomizationForm="True" VisibleIndex="6" Width="50%">
															<DataItemTemplate>
																<dx:ASPxMemo ID="mem" runat="server" Height="25px" oninit="mem_Init" 
																	Text='<%# Eval("comment") %>' Theme="NETheme01" Width="100%">
																</dx:ASPxMemo>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Type" FieldName="_type" 
															ShowInCustomizationForm="True" Visible="False" VisibleIndex="8">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Checked By" FieldName="chkby" 
															ShowInCustomizationForm="True" VisibleIndex="7" Width="125px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Re Direct" FieldName="response" 
															ShowInCustomizationForm="True" VisibleIndex="4" Width="125px">
															<DataItemTemplate>
																<dx:ASPxComboBox ID="ddl_user" runat="server" AnimationType="None" 
																	DataSourceID="sds_users" EnableCallbackMode="True" 
																	IncrementalFilteringMode="Contains" oninit="ddl_user_PreRender" 
																	Text='<%# Eval("response") %>' TextField="name" Theme="NETheme01" 
																	ValueField="id" ValueType="System.Int32">
																</dx:ASPxComboBox>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Date" FieldName="dt" 
															ShowInCustomizationForm="True" VisibleIndex="9" Width="60px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataCheckColumn Caption="Completed?" FieldName="chk" 
															ShowInCustomizationForm="True" VisibleIndex="5" Width="30px">
															<CellStyle HorizontalAlign="Center">
															</CellStyle>
														</dx:GridViewDataCheckColumn>
													</Columns>
													<SettingsPager Mode="ShowAllRecords">
													</SettingsPager>
													<Settings ShowTitlePanel="True" ShowFooter="True" />
													<Templates>
														<TitlePanel>
															<table cellpadding="5px" style="width: 100%; text-align: left;">
																<tr>
																	<td colspan="3" style="font-weight: 700; font-family: Arial;">
																		<dx:ASPxLabel ID="lbltitle" runat="server" Font-Size="16pt" Text="Checklist" 
																			Theme="NETheme01">
																		</dx:ASPxLabel>
																	</td>
																	<td style="font-weight: 700; font-family: Arial;">
																		&nbsp;</td>
																</tr>
																<tr>
																	<td nowrap="nowrap">
																		<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Cursor="pointer" 
																			Text="Direct Supervisor" Theme="NETheme01" Wrap="False">
																			<ClientSideEvents Click="function(s, e) {
		gv_chklist1.PerformCallback('Direct');
}" />
																		</dx:ASPxHyperLink>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="lbldirect" runat="server" Text="ASPxLabel" Theme="NETheme01">
																		</dx:ASPxLabel>
																	</td>
																	<td width="100%">
																		&nbsp;</td>
																	<td align="right" rowspan="4" style="text-align: right" valign="middle" 
																		width="100%">
																		&nbsp;</td>
																</tr>
																<tr>
																	<td>
																		<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" Cursor="pointer" 
																			Text="Payroll Admin" Theme="NETheme01">
																			<ClientSideEvents Click="function(s, e) {
		gv_chklist1.PerformCallback('Payroll');
}" />
																		</dx:ASPxHyperLink>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="lblpayroll" runat="server" Text="ASPxLabel" Theme="NETheme01">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		&nbsp;</td>
																</tr>
																<tr>
																	<td nowrap="nowrap">
																		<dx:ASPxHyperLink ID="ASPxHyperLink3" runat="server" Cursor="pointer" 
																			Text="Systems/IT Admin" Theme="NETheme01">
																			<ClientSideEvents Click="function(s, e) {
	gv_chklist1.PerformCallback('SysAdmin');
}" />
																		</dx:ASPxHyperLink>
																	</td>
																	<td>
																		<dx:ASPxLabel ID="lblsystems" runat="server" Text="ASPxLabel" Theme="NETheme01">
																		</dx:ASPxLabel>
																	</td>
																	<td>
																		&nbsp;</td>
																</tr>
																<tr>
																	<td colspan="3">
																		<dx:ASPxLabel ID="ASPxLabel1" runat="server" 
																			Text="Note: Even if it doesn't apply, please check every item to verify that there are no outstanding issues to be resolved prior to setting this employee's status to &quot;Past&quot;" 
																			Theme="NETheme01" Width="600px" Wrap="True">
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
									<dx:TabPage Text="Footprints">
										<ContentCollection>
											<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
												<uc1:uc_member_usage ID="uc_member_usage1" runat="server" />
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
								</TabPages>
							</dx:ASPxPageControl>
							
							<br />
						</td>
					</tr>
					<tr>
						<td colspan="2" align="left">
												
							&nbsp;</td>
					</tr>
					<tr>
						<td align="left" colspan="2">
							<dx:ASPxHyperLink ID="hl_direct" runat="server" 
								Text="Direct Supervisor Checklist" ClientVisible="False" />
							<br />
							<dx:ASPxHyperLink ID="hl_sysadmin" runat="server" 
								Text="Systems Administrator Checklist" ClientVisible="False" />
							<br />
							<dx:ASPxHyperLink ID="hl_payroll" runat="server" Text="Payroll Checklist" 
								ClientVisible="False" />
							<br />
							<asp:Panel ID="chklist" runat="server">
							</asp:Panel>
						</td>
					</tr>
					<tr>
						<td>
							
							<asp:SqlDataSource ID="sds_users" runat="server"
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
	a.member_id id,
	CONCAT(b.name, ' - ', a.member_fullname) name
FROM 
	member a
LEFT JOIN
	business_unit b ON
		a.business_unit_id = b.id
LEFT JOIN
	cellphone_number c ON 
		a.cellphone_number_id = c.id 
WHERE 
	a.member_status = 'Active'
ORDER BY
	b.name,a.member_lastname, a.member_fullname"></asp:SqlDataSource>
							
						</td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td colspan='2' align="center">
							&nbsp;</td>
					</tr>
				</table>
	</div>
</asp:Content>

