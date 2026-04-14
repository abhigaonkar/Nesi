<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="master_workorders" Title="Master Work Order Grid" Codebehind="index.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"	Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>

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
		function email(id) {
			boing("/sections/reports/invoice_preview/index.aspx?id=" + id, 'invoice_preview', 850, 850);
		}

		function change_exp_hours(s, e) {
		    var o = {
		        woprog_id: s.cpwoprog_id,
		        exp_hours:s.GetText()

		}

		cb_exp_hours.PerformCallback(JSON.stringify(o));
		}

		function change_acting_ram(s, e) {
		    var o = {
		        woprog_id: s.cpwoprog_id,
		        acting_ram: s.GetValue()

		    }

		    ddl_ram_cb.PerformCallback(JSON.stringify(o));
		}

           

		function switch_expected_date(s,e)
			{
			var o		=	{
							woprog_id:		s.cpwoprog_id,
							end_date:		s.GetText()
							}
			cb_expected_date.PerformCallback(JSON.stringify(o));
			}
		function note_show(obj) {
			var exists = $("#note_window").size() > 0;
			var o = $(obj).offset();
			var x = o.left;
			var y = o.top;
			var woprog_id = $(obj).parents("div:first").attr('data-woprog_id');
			if (exists && $("#note_window")) {
				$("#note_window").remove();
			}
			var html = "<div id='note_window'>";
			html += "<b>Note:</b>\
									<br/><textarea style='width:99%;height:100px;' maxlength='512'></textarea>\
									<br><button style='font-size:11px;font-weight:bold;' onclick='master_wo.update_note.run(this);' data-woprog_id='" + woprog_id + "' type='button'><img align='absmiddle' src='/images/icon/icon[save].gif'/> Save</button>\
									<button style='font-size:11px;font-weight:bold;' onclick=\"$('#note_window').remove();\"><img align='absmiddle' src='/images/icon/icon[delete].gif'/> Close</button>";
			html += "</div>";
			$("body").append(html);
			var off = $(obj).offset();
			$("#note_window").css({ "position": "absolute",
				"top": off.top + "px",
				"left": off.left + $(obj).width() + "px",
				"width": "300px",
				"padding": "5px",
				"border": "solid 1px #cc7",
				"background-color": "#ff7"
			}).find("textarea").focus().val();
			var left = $(document).outerWidth() - $(window).width();
			$('body, html').scrollLeft(left);
		}
		var master_wo		=	{
								gv:				null,
								exp_hrs:
										{
										run:
											function(s)
												{
												master_wo.gv			= gv_workorders;
												var hrs					= $(s).val() / 1;
												var original_hrs		= $(s).attr("data-original_hrs") / 1;
												var woprog_id			= $(s).attr("data-woprog_id") / 1;
												if(hrs != original_hrs)
													{
													PageMethods.update_lbr(hrs, woprog_id, this.complete, master_wo.error, master_wo.timeout);
													}
												},
										complete:
											function()
												{
												master_wo.gv			= gv_workorders;
												master_wo.gv.Refresh();
												}
										},
								update_note:
										{
										run:
											function(obj)
												{
												master_wo.gv			= gv_workorders;
												var note				= $("#note_window").find("textarea").val();
												var woprog_id			= $(obj).attr("data-woprog_id") / 1;
												PageMethods.update_note(note, woprog_id, this.complete, master_wo.error, master_wo.timeout);
												},
										complete:
											function()
												{
												$('#note_window').remove();
												master_wo.gv			= gv_workorders;
												master_wo.gv.Refresh();
												}
										},
								timeout: 
									function(arg)
										{
										alert("Timeout occured");
										},
								error: 
									function(arg)
										{
										alert("Error has occured: " + arg._message);
										}
								}
	$(document).ready(function()
		{
		Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
		});
	function EndReqHandler()
		{
		please_wait("stop");
		}
	function BeginReqHandler()
		{
		please_wait("start");
		}
	</script>
	<asp:ScriptManager ID="sm" EnablePageMethods="true" runat="server">
	</asp:ScriptManager><lc:LayoutControl ID="layout" runat="server" ShowToggle="true" is_private="True" />
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
			<dx:ASPxCheckBox ID="chk_includeprogress" runat="server" text="Include Progress Billings" AutoPostBack="True" Checked="true" oncheckedchanged="chk_includeprogress_CheckedChanged" />
			<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
				oncallback="cb_Callback" Width="100%">
				<LoadingPanelStyle HorizontalAlign="Center" VerticalAlign="Middle">
				</LoadingPanelStyle>
				<PanelCollection>
					<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
					</dx:PanelContent>
				</PanelCollection>
			</dx:ASPxCallbackPanel>
			<dx:ASPxGridView ID="gv_workorders" runat="server" autogeneratecolumns="False" 
				clientinstancename="gv_workorders" 
				cssfilepath="~/App_Themes/BlackGlass/{0}/styles.css" csspostfix="BlackGlass" 
				Font-Names="Arial" Font-Size="9pt" keyfieldname="WOID" 
				OnCommandButtonInitialize="gv_workorders_CommandButtonInitialize" 
				oncustombuttoninitialize="gv_workorders_CustomButtonInitialize" 
				OnCustomCallback="gv_workorders_CustomCallback" 
				OnCustomJSProperties="gv_workorders_CustomJSProperties" 
				oncustomsummarycalculate="gv_workorders_CustomSummaryCalculate" 
				ondatabinding="gv_workorders_DataBinding" 
				OnHtmlDataCellPrepared="gv_workorders_HtmlDataCellPrepared" 
				onpageindexchanged="gv_workorders_PageIndexChanged" 
				onrowupdating="gv_workorders_RowUpdating" 
                OnAutoFilterCellEditorInitialize="gv_workorders_AutoFilterCellEditorInitialize"
				OnSummaryDisplayText="gv_workorders_SummaryDisplayText" width="100%" 
				onhtmlrowprepared="gv_workorders_HtmlRowPrepared" KeyboardSupport="True" 
				onhtmlfootercellprepared="gv_workorders_HtmlFooterCellPrepared" SettingsLoadingPanel-Text=" ">
                <SettingsPopup editform-horizontalalign="Center" editform-verticalalign="WindowCenter" editform-width="700px">

                </SettingsPopup>

				<ClientSideEvents CustomButtonClick="function(s, e) {
	if(e.buttonID == 'invoice'){
                    var rowVisibleIndex = e.visibleIndex;
                    var rowKeyValue;
					s.GetRowValues( e.visibleIndex,'WOID',email);
				//	alert(rowKeyValue);
				//	email(rowKeyValue);
}
}" />
				<TotalSummary>
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="Invoiced_Amount" ShowInColumn="Invoiced_Amount" SummaryType="Sum" Tag="Total_Invoiced_Amount" />
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="JobCost" ShowInColumn="Benchmark Sell" SummaryType="Sum" Tag="Total_JobCost" />
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="QuotedAmount" ShowInColumn="Quoted Amount" SummaryType="Sum" Tag="Total_Quoted_Amount" />
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="to_be_billed" ShowInColumn="To Be Billed" SummaryType="Sum" Tag="to_be_billed" />
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="gross_profit" ShowInColumn="gross_profit" SummaryType="Sum" Tag="gross_profit" />
					<dx:ASPxSummaryItem DisplayFormat="c2" FieldName="wo_total" ShowInColumn="wo_total" SummaryType="Sum" Tag="wo_total" />
					<dx:ASPxSummaryItem DisplayFormat="p2" FieldName="GrossMargin" ShowInColumn="Gross Margin" SummaryType="Custom" Tag="GrossMargin" />
					<dx:ASPxSummaryItem DisplayFormat="N1" FieldName="invoice_lag" ShowInColumn="Invoice Lag" SummaryType="Custom" Tag="invoice_lag" />
                    <dx:ASPxSummaryItem DisplayFormat="c" FieldName="mat_cost" ShowInColumn="Mat Cost" SummaryType="Sum" Tag="Total_Mat_Cost" />
                    <dx:ASPxSummaryItem DisplayFormat="c" FieldName="lab_cost" ShowInColumn="Lab Cost" SummaryType="Sum" Tag="Total_Lab_Cost" />
				    <dx:ASPxSummaryItem DisplayFormat="n" FieldName="hours_left" ShowInColumn="hours_left" SummaryType="Sum" Tag="Total_hours_left" />
				    <dx:ASPxSummaryItem DisplayFormat="n" FieldName="total_hours_worked" ShowInColumn="total_hours_worked" SummaryType="Sum" Tag="total_hours_worked" />
				    <dx:ASPxSummaryItem DisplayFormat="n" FieldName="exp_hours" ShowInColumn="exp_hours" SummaryType="Sum" Tag="total_exp_hours" />
				</TotalSummary>
				<GroupSummary>
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="QuotedAmount" SummaryType="Sum" Tag="BranchQuotedAmount" />
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="JobCost" SummaryType="Sum" Tag="Branch_Quoted_Amount" />
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="Remainder" SummaryType="Sum" Tag="Totot_Remainder" />
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="Invoiced_Amount" SummaryType="Sum" Tag="Total_PM_Invoiced" />
					<dx:ASPxSummaryItem DisplayFormat="c" FieldName="QuotedAmount" 	ShowInColumn="Quoted Amount" ShowInGroupFooterColumn="PM" SummaryType="Sum" Tag="Total_PM_Quoted" />
				    <dx:ASPxSummaryItem DisplayFormat="n" FieldName="hours_left" SummaryType="Sum" Tag="hours_left" />
				    <dx:ASPxSummaryItem DisplayFormat="n" FieldName="total_hours_worked" SummaryType="Sum" Tag="total_hours_worked" />
				<dx:ASPxSummaryItem DisplayFormat="n0" FieldName="exp_hours" SummaryType="Sum" Tag="exp_hours" />
                </GroupSummary>
                <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/EmptyNotes.JPG" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
				<Columns>
					<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" ShowEditButton="false" >
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewCommandColumn>
					<dx:GridViewDataTextColumn FieldName="business_unit" Caption="Business Unit" VisibleIndex="1" Width="40px">
						<PropertiesTextEdit Width="100px"></PropertiesTextEdit>
						<Settings HeaderFilterMode="CheckedList" />
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					
					<dx:GridViewDataTextColumn FieldName="BVWO" ReadOnly="True" VisibleIndex="3" 
						Width="25px">
						<PropertiesTextEdit Width="100px"></PropertiesTextEdit>
						<DataItemTemplate>
							<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Target="_blank"
								NavigateUrl='<%#  "/redir.aspx?url=" + HttpUtility.UrlEncode(string.Format("/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}", Eval("woprog_id"), Eval("business_unit_id"))) %>'
								Text='<%# Eval("BVWO") %>'>
							</dx:ASPxHyperLink>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Customer" ReadOnly="True" 
						VisibleIndex="4" Width="100px">
						<PropertiesTextEdit Width="200px"></PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="5" 
						Width="100%">
						<PropertiesTextEdit Height="25px" Width="600px"></PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Quote" VisibleIndex="6" Width="25px">
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Invoiced_Amount" VisibleIndex="7" 
						Width="50px">
						<PropertiesTextEdit DisplayFormatString="c"></PropertiesTextEdit>
						<EditFormSettings Visible="False" />
						<CellStyle BackColor="#E0E0E0">
						</CellStyle>
					</dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ns_job_internal_id" Caption="Netsuite Job Internal ID" VisibleIndex="8" Width="30px"></dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="QuotedAmount" VisibleIndex="9" 
						Width="25px">
						<PropertiesTextEdit DisplayFormatString="c"></PropertiesTextEdit>
						<EditFormSettings Visible="False" />
						<CellStyle BackColor="#C0FFC0">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Benchmark Sell" FieldName="JobCost" 
						VisibleIndex="10" Width="25px">
						<PropertiesTextEdit DisplayFormatString="c"></PropertiesTextEdit>
						<EditFormSettings Visible="False" />
						<CellStyle BackColor="#FFC0C0" Font-Bold="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Left on Quote" FieldName="Remainder" 
						VisibleIndex="11" Width="25px">
						<PropertiesTextEdit DisplayFormatString="c"></PropertiesTextEdit>
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="To Be Billed" FieldName="to_be_billed" 
						VisibleIndex="12" Width="25px">
						<PropertiesTextEdit DisplayFormatString="c"></PropertiesTextEdit>
						<EditFormSettings Visible="False" />
						<CellStyle BackColor="#99CCFF">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="WOID" ReadOnly="True" VisibleIndex="13" 
						Width="25px">
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Cut Date" FieldName="Cut_Date" 
						VisibleIndex="14" Width="50px">
						<EditFormSettings Visible="False" />
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="15" Width="30px">
						<Settings HeaderFilterMode="CheckedList" />
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="PM" VisibleIndex="16" Width="50px">
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Expected End Date" 
						FieldName="Expected_EndDate" VisibleIndex="17" Width="50px">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
							EditFormatString="yyyy-MM-dd" Width="100px"></PropertiesDateEdit>
						<DataItemTemplate>
							<dx:ASPxDateEdit ID="de_expected_end" runat="server" Font-Size="11px" 
								OnCustomJSProperties="de_expected_end_CustomJSProperties" 
								Value='<%# Eval("Expected_EndDate") %>' Width="95px" DisplayFormatString="yyyy-MM-dd" 
								EditFormatString="yyyy-MM-dd">
								<ClientSideEvents DateChanged="switch_expected_date" />
							</dx:ASPxDateEdit>
						</DataItemTemplate>
						<CellStyle Font-Names="Arial" Font-Size="8pt">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="Expected Sales" FieldName="Expected_Sales" 
						VisibleIndex="18" Width="25px">
						<PropertiesTextEdit DisplayFormatString="C2" NullDisplayText="0" Width="100px"><Style Font-Names="Arial" Font-Size="9pt"></Style></PropertiesTextEdit>
						<DataItemTemplate>
							<dx:ASPxLabel ID="ASPxLabel1" runat="server" 
								Text='<%# Eval("Expected_Sales", "{0:C}") %>'>
							</dx:ASPxLabel>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Progress" VisibleIndex="19" Width="25px">
						<EditFormSettings Visible="False" />
						<DataItemTemplate>
							<dx:ASPxLabel ID="ASPxLabel2" runat="server" 
								Text='<%# string.Format("{0}%", Eval("Progress")) %>'>
							</dx:ASPxLabel>
						</DataItemTemplate>
						<Settings SortMode="Value" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Invoice Date" FieldName="Invoice_Date" 
						VisibleIndex="20" Width="50px">
						<EditFormSettings Visible="False" />
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataMemoColumn FieldName="Notes" VisibleIndex="21" Width="20px">
						<PropertiesMemoEdit Height="100px" Width="600px"></PropertiesMemoEdit>
						<DataItemTemplate>
							<div class='note' data-woprog_id='<%# Eval("woprog_id") %>'>
							<dx:ASPxMemo ID="memnote" runat="server" ClientInstanceName="memnote" 
								Font-Names="Arial" Font-Size="8pt" Height="15px"  OnInit="memnote_Init" 
								Text='<%# Eval("Notes") %>' Width="100%" ReadOnly="true">
								<ClientSideEvents Init="function(s, e) {
	 var text = s.GetText().replace(/\r/g, '');
	if (text.length &gt;1 )
		{
			s.SetHeight(40);
			s.GetMainElement().style.backgroundColor = &quot;yellow&quot;
			s.GetInputElement().style.backgroundColor = &quot;yellow&quot;
		}

}" />
							</dx:ASPxMemo></div>
						</DataItemTemplate>
						<CellStyle>
							<Paddings Padding="0px" />
						</CellStyle>
					</dx:GridViewDataMemoColumn>
					<dx:GridViewDataTextColumn FieldName="Completion" VisibleIndex="22" 
						Width="25px">
						<PropertiesTextEdit DisplayFormatString="p"></PropertiesTextEdit>
						<Settings FilterMode="DisplayText" />
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Account_Manager" VisibleIndex="23" 
						Width="50px">
						<Settings HeaderFilterMode="CheckedList" />
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="N/A"
						VisibleIndex="24" Width="20px" ShowInCustomizationForm="false">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Gross Margin" FieldName="GrossMargin" 
						VisibleIndex="25" Width="20px">
						<PropertiesTextEdit DisplayFormatString="p"></PropertiesTextEdit>
						<EditFormSettings Visible="False" />
						<CellStyle Font-Bold="False" Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Progress Billed" 
						FieldName="ProgressBilling" VisibleIndex="26" Width="20px">
						<Settings AllowHeaderFilter="True" AllowSort="True" />
						<EditFormSettings Visible="False" />
						<DataItemTemplate>
							<dx:ASPxLabel ID="ASPxLabel1" runat="server" 
								Text='<%# Eval("ProgressBilling", "{0:C}") %>'>
							</dx:ASPxLabel>
						</DataItemTemplate>
						<CellStyle BackColor="#C0FFFF">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Invoice_Emailed" 
						FieldName="woprog_last_email_date" ReadOnly="True" VisibleIndex="27" 
						Width="20px">
						<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd"></PropertiesTextEdit>
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Invoice No" FieldName="WOProg_InvoiceNo" 
						ReadOnly="True" VisibleIndex="28" Width="20px">
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
				
					<dx:GridViewDataTextColumn Caption="Days Since Cut" FieldName="days_since_cut" 
						VisibleIndex="29" Width="30px">
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Days Since Inv" FieldName="days_since_inv" 
						VisibleIndex="30" Width="25px">
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataCheckColumn Caption="Service Call" FieldName="sc" 
						ReadOnly="True" VisibleIndex="31" Width="25px">
						<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" 
							ValueUnchecked="0"></PropertiesCheckEdit>
					</dx:GridViewDataCheckColumn>
					<dx:GridViewDataTextColumn Caption="PO" FieldName="PO" VisibleIndex="32" 
						Width="30px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataCheckColumn Caption="RD" FieldName="rd" VisibleIndex="33" 
						Width="30px">
						<PropertiesCheckEdit NullDisplayText="0" ValueChecked="1" ValueGrayed="0" 
							ValueType="System.Int32" ValueUnchecked="0"></PropertiesCheckEdit>
					</dx:GridViewDataCheckColumn>
					<dx:GridViewDataTextColumn Caption="Asset" FieldName="Asset" VisibleIndex="34" 
						Width="40px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="gross_profit" FieldName="gross_profit" 
						Visible="False" VisibleIndex="41">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="wo_total" FieldName="wo_total" 
						Visible="False" VisibleIndex="40">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataCheckColumn Caption="On Hold" FieldName="hold" 
						VisibleIndex="35" Width="25px">
						<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" 
							ValueUnchecked="0"></PropertiesCheckEdit>
					</dx:GridViewDataCheckColumn>
					<dx:GridViewDataTextColumn Caption="City" FieldName="city" MinWidth="10" 
						VisibleIndex="36" Width="40px">
						<Settings HeaderFilterMode="CheckedList" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="State" FieldName="province" MinWidth="10" 
						VisibleIndex="37" Width="40px">
						<Settings HeaderFilterMode="CheckedList" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Customer Requires PO" 
						FieldName="customer_req_po" Visible="False" VisibleIndex="70">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Days Till Close" Visible="false" 
						FieldName="days_till_close" MinWidth="25" VisibleIndex="42">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Days Till Start" Visible="false" 
						FieldName="days_till_start" MinWidth="25" VisibleIndex="45">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Exp Hrs Lbr" Visible="false" 
						FieldName="exp_hours" MinWidth="25" VisibleIndex="47">
						<DataItemTemplate>
						    
						    
						        <dx:ASPxTextBox ID="txt_exp_hours" runat="server" Font-Size="11px" 
						                         OnCustomJSProperties="txt_exp_hours_CustomJSProperties" 
						                         Text='<%# Eval("exp_hours") %>' Width="95px"  
						                       >
						            <ClientSideEvents TextChanged="change_exp_hours" />
						        </dx:ASPxTextBox>
						   
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Invoice Lag" FieldName="invoice_lag" 
						MinWidth="20" Visible="False" VisibleIndex="39">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Warranty" FieldName="warranty" 
						MinWidth="15" Visible="False" VisibleIndex="37" Width="40px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Service Address" FieldName="service_addr" 
						MinWidth="100" VisibleIndex="71" Width="150px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Scanned Date" FieldName="scanned_date" 
						VisibleIndex="73" Width="75px">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
							EditFormatString="yyyy-MM-dd"></PropertiesDateEdit>
						<EditFormSettings Visible="False" />
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="Days Late" FieldName="days_late" 
						VisibleIndex="75" Width="60px">
						<Settings AutoFilterCondition="GreaterOrEqual" />
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Country" FieldName="country" Visible="False" VisibleIndex="76" Width="50px">
					</dx:GridViewDataTextColumn>				
				<dx:GridViewDataTextColumn Caption="Total Hours Worked" FieldName="total_hours_worked" Visible="False" VisibleIndex="90" Width="50px">
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Job Cost WO" FieldName="WOProg_Associate_WOProg_ID" Visible="False" VisibleIndex="90">				    
				    <DataItemTemplate>
				        <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Target="_blank"
				                          NavigateUrl='<%# "/redir.aspx?url=" + HttpUtility.UrlEncode(string.Format("/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}", Eval("WOProg_Associate_WOProg_ID"), Eval("business_unit_id"))) %>'
				                          Text='<%# Eval("WOProg_Associate_WOProg_ID") %>'>
				        </dx:ASPxHyperLink>
				    </DataItemTemplate>
				</dx:GridViewDataTextColumn>
                    <dx:GridViewDataCheckColumn Caption="Inspection Rqd" FieldName="inspection_required" Visible="False" VisibleIndex="78" Width="50px">
					</dx:GridViewDataCheckColumn>
                    <dx:GridViewDataTextColumn Caption="Inspection Link" FieldName="inspection_link" Visible="False" VisibleIndex="79" Width="50px">
					</dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Days in this Bucket" FieldName="days_in_bucket" Visible="False" VisibleIndex="80" Width="50px">
					</dx:GridViewDataTextColumn>

				    <dx:GridViewDataDateColumn Caption="Last Date Worked" FieldName="last_date_worked" Visible="False" VisibleIndex="81" Width="70px">
                        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd"></PropertiesDateEdit>
                    </dx:GridViewDataDateColumn>
                     <dx:GridViewDataTextColumn Caption="Quote $ Progress" FieldName="Progress" Visible="False" VisibleIndex="82" Width="50px">
					</dx:GridViewDataTextColumn>


				    <dx:GridViewDataTextColumn Caption="BDM" FieldName="BDM" MinWidth="20" VisibleIndex="83" Width="50px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Mat cost" FieldName="mat_cost" MinWidth="20" VisibleIndex="84" Width="50px">
                    </dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn Caption="Lab cost" FieldName="lab_cost" MinWidth="20" VisibleIndex="85" Width="50px">
                    </dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Hours Left" FieldName="hours_left" MinWidth="20" VisibleIndex="86" Width="50px" Visible="False">
				</dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn Caption="Sustainability" FieldName="sustainability_project" Visible="False" VisibleIndex="87" Width="50px">
					</dx:GridViewDataTextColumn>
                     <dx:GridViewDataCheckColumn Caption="Has Open PO" FieldName="open_pos" Visible="False" VisibleIndex="88" Width="50px">
				</dx:GridViewDataCheckColumn>
					
				<dx:GridViewDataTextColumn Caption="Acting BDM on WO" FieldName="acting_bdm" ShowInCustomizationForm="True" VisibleIndex="89" Width="50px" runat="server">
				  </dx:GridViewDataTextColumn>

				<dx:GridViewDataTextColumn Caption="Quote Cost" FieldName="Quote_costs" Visible="False" VisibleIndex="91" Width="50px">
				    <PropertiesTextEdit DisplayFormatString="C2" NullDisplayText="0" Width="100px">
				        <Style Font-Names="Arial" Font-Size="9pt"></Style>
				    </PropertiesTextEdit>
				</dx:GridViewDataTextColumn>
                     <dx:GridViewDataTextColumn Caption="WO Tag" FieldName="jobtag" Visible="False" VisibleIndex="92" Width="50px">
					</dx:GridViewDataTextColumn>
                     <dx:GridViewDataDateColumn Caption="Expected Start Date" FieldName="expected_startdate" Visible="False" VisibleIndex="93" Width="70px">
                        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd"></PropertiesDateEdit>
                    </dx:GridViewDataDateColumn>
               <dx:GridViewDataTextColumn Caption="Total Quoted Hours" FieldName="quotedhours" Width="50px"	VisibleIndex="94">		
                   <PropertiesTextEdit DisplayFormatString="N1">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Controls Person" FieldName="controls_person" MinWidth="20" VisibleIndex="95" Width="50px">
                    </dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="ADS" FieldName="ADS" MinWidth="20" VisibleIndex="125" Width="50px" Visible="False"></dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="SAM" FieldName="SAM" MinWidth="20" VisibleIndex="126" Width="50px" Visible="False"></dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Created From" FieldName="CreatedFrom" MinWidth="20" VisibleIndex="127" Width="50px" Visible="False"></dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="SubTotal" FieldName="SubTotal" MinWidth="20" VisibleIndex="128" Width="50px" Visible="False"></dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Quoted Gross Margin" FieldName="QuotedGrossMargin" MinWidth="20" VisibleIndex="129" Width="50px" Visible="False">
					<Settings AllowHeaderFilter="True" AllowSort="True" FilterMode="Value" />
					<PropertiesTextEdit DisplayFormatString="P3"/>
					<EditFormSettings Visible="False" />
					<DataItemTemplate>
						<%# Eval("QuotedGrossMargin") == System.DBNull.Value ? "" : String.Format("{0:P3}", double.Parse(Eval("QuotedGrossMargin").ToString())) %>
					</DataItemTemplate>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Invoice Feedback Issues" FieldName="invoice_fb_issues" MinWidth="20" VisibleIndex="130" Width="50px" Visible="False">
                    </dx:GridViewDataTextColumn>
				</Columns>
				<SettingsBehavior AutoFilterRowInputDelay="6000" ColumnResizeMode="Control" 
					EnableRowHotTrack="True" EnableCustomizationWindow="True"/>
				<SettingsPager AlwaysShowPager="True" NumericButtonCount="50" PageSize="50" 
					Position="TopAndBottom">
					<AllButton Text="All">
					</AllButton>
				</SettingsPager>
				<SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
				<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
					ShowFooter="True" ShowGroupedColumns="True" ShowGroupFooter="VisibleIfExpanded" 
					ShowGroupPanel="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" 
					ShowFilterRowMenuLikeItem="True"
                    ColumnMinWidth="20"
                     />

				<SettingsText CommandUpdate="Save" PopupEditFormCaption="Quick Edit" />
                <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides" />


				<Images SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
					<LoadingPanelOnStatusBar Url="~/App_Themes/BlackGlass/GridView/gvLoadingOnStatusBar.gif">
					</LoadingPanelOnStatusBar>
					<LoadingPanel Url="/images/loading_panel.gif">
					</LoadingPanel>
				</Images>
				<ImagesFilterControl>
					<LoadingPanel Url="/images/loading_panel.gif">
					</LoadingPanel>
				</ImagesFilterControl>
				<Styles CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" 
					CssPostfix="BlackGlass">
					<Header Font-Bold="False" Font-Size="Small" ImageSpacing="5px" 
						SortingImageSpacing="5px" Wrap="True">
					</Header>
					<Row Font-Size="Smaller">
					</Row>
					<RowHotTrack Font-Names="Arial" Font-Size="8pt">
					</RowHotTrack>
					<Cell Font-Names="Arial" Font-Size="8pt">
					</Cell>
				</Styles>
				<StylesEditors>
					<CalendarHeader Spacing="1px">
					</CalendarHeader>
					<ProgressBar Height="25px">
					</ProgressBar>
				</StylesEditors>
				<Templates>
					<TitlePanel>
						<table>
							<tr>
								<td bgcolor="#FFFF99" width="100">&nbsp;</td>
								<td align="left" width="120"><b>Service Call</b></td>
							</tr>
							<tr>
								<td bgcolor="#FFCCCC">&nbsp;</td>
								<td align="left"><b>On Hold</b></td>
							</tr>
						</table>
					</TitlePanel>
					<EditForm>
						<dx:ASPxGridViewTemplateReplacement ID="Editors" runat="server" 
							ReplacementType="EditFormEditors" />
						<div style="margin-top: 10px; margin-bottom: 20px; padding-bottom: 20px;">
							<div style="float: left; margin-left: 5px">
								<dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" 
									ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' 
									Text="Cancel" Width="100px" />
							</div>
							<div style="float: left; margin-left: 10px">
								<dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" 
									ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' 
									CssClass="input" Text="Save" Width="100px" />
							</div>
						</div>
					</EditForm>
				</Templates>
			</dx:ASPxGridView>
           <asp:SqlDataSource ID="sds_acct_manager" runat="server" 
				                                                    ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
				                                                    ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
               SelectCommand="SELECT
		0 Member_ID,
		'No One' member_fullname


union

	Select * from (SELECT
		member.member_id,
		concat(
			member_fullname,
			'(',
			bu.ddl_name,
			')'
		) member_fullname
	FROM
		member
	INNER JOIN business_unit bu ON bu.id = member.business_unit_id
	WHERE
		member_membertype_id IN (
			8,
			11,
			27,
			34,
			35,
			37,
			38,
			39,
			46,
			49,
			52,
			53,
			54,
			55,
			56,
			59,
			4,
			5,
			12,
			17,
			24,
			25,
			41
		)
	AND member_status = 'Active'
	ORDER BY
		bu.ddl_name asc, member_fullname)a">
               </asp:SqlDataSource>
		
            <dx:ASPxCallback ID="ddl_ram_cb" runat="server" 
		                 ClientInstanceName="ddl_ram_cb" OnCallback="ddl_ram_Callback">
		    <ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" EndCallback="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
		</dx:ASPxCallback>
			

		<dx:ASPxCallback ID="cb_exp_hours" runat="server" 
		                 ClientInstanceName="cb_exp_hours" OnCallback="cb_exp_hours_Callback">
		    <ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" EndCallback="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
		</dx:ASPxCallback>
			<dx:ASPxCallback ID="cb_expected_date" runat="server" 
				ClientInstanceName="cb_expected_date" OnCallback="cb_expected_date_Callback">
				<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" EndCallback="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
			</dx:ASPxCallback>
			<br />
		</ContentTemplate>
	</asp:UpdatePanel>
	<br />
            &nbsp;&nbsp;
			</asp:Content>

<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
    <style type="text/css">
		.style6
		{
			text-align: left;
		}
	</style>
</asp:Content>
