<%@ Control Language="C#" AutoEventWireup="true" Codebehind="approval.ascx.cs" Inherits="sections_hr_payroll_submit_approval" %>
<%@ Import Namespace="nesi.core" %>
<%@ register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<dx:aspxcallbackpanel id="cbp_approval" clientinstancename="cbp_approval" runat="server" width="100%" oncallback="cbp_approval_Callback">
		<clientsideevents  endcallback="approval.end_callback" />
	<panelcollection>
		<dx:panelcontent>
			<div id="div_no_employees" runat="server" style="font-size: 1.5em;padding-top:25px;text-align: center;">You have no further employees to approve.</div>
			<table cellpadding="2" cellspacing="0" width="100%">
				<tr>
					<td width="150" valign="top">
						<dx:aspxlistbox id="lb_members" runat="server" width="150px" height="600px" textfield="name" valuefield="id" theme="NETheme01" autopostback="false">
							<clientsideevents selectedindexchanged="approval.get_user" />
						</dx:aspxlistbox>
					</td>
					<td valign="top">
			<table cellpadding="5" cellspacing="0" width="100%">
				<tr>
					<td valign="middle" align="center" width="100">
						<dx:aspxbutton id="bt_prev" runat="server" encodehtml="false" theme="NETheme01" text="prev" width="100%" height="100%" autopostback="false"></dx:aspxbutton>
					</td>
					<td>
						<div>
							<dx:aspxlabel id="lb_name" runat="server" text="Name" font-size="1.6em" font-bold="true" encodehtml="false"></dx:aspxlabel>
						</div>
						<div>
							<dx:aspxlabel id="lb_handler" runat="server" text="Payroll Handler" encodehtml="false"></dx:aspxlabel>
						</div>
						<div>
							<dx:aspxlabel id="lb_paytype" runat="server" text="Pay Type" encodehtml="false"></dx:aspxlabel>
						</div>
						<div>
							<dx:aspxlabel id="lb_membertype" runat="server" text="Member Type" encodehtml="false"></dx:aspxlabel>
						</div>
						<div>
							<dx:aspxlabel id="lb_hrstatus" runat="server" text="HR Status" encodehtml="false"></dx:aspxlabel>
						</div>
						<div>
							<dx:aspxlabel id="lb_utilization" runat="server" text="Utilization" encodehtml="false"></dx:aspxlabel>
						</div>
					</td>
					<td valign="middle" align="center" width="100">
						<dx:aspxbutton id="bt_next" runat="server" encodehtml="false" theme="NETheme01" text="next" width="100%" height="100%" autopostback="false"></dx:aspxbutton>
					</td>
				</tr>
			</table>
			<br />
			<div runat="server" id="div_wage_priv" visible="false" style="color:#f60;font-size:1.35em;">* Not showing wages or totals currently because your account does not have the needed privilege.</div>
			<dx:aspxgridview id="gv_hours" runat="server" KeyFieldName="item_id" clientinstancename="gv_hours" onhtmldatacellprepared="gv_hours_HtmlDataCellPrepared" theme="NETheme01" onsummarydisplaytext="gv_hours_SummaryDisplayText" oncustomcolumngroup="gv_hours_CustomColumnGroup" onhtmlrowprepared="gv_hours_HtmlRowPrepared">
				<groupsummary>
					<dx:aspxsummaryitem fieldname="rt"		showingroupfootercolumn="rt" tag="rt"		summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="ot"		showingroupfootercolumn="ot"		summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="dt"		showingroupfootercolumn="dt"		summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="rtsp"	showingroupfootercolumn="rtsp"		summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="otsp"	showingroupfootercolumn="otsp"		summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="dtsp"	showingroupfootercolumn="dtsp"		summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="unpaid_time_off"	showingroupfootercolumn="unpaid_time_off"	summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="paid_time_off"	showingroupfootercolumn="paid_time_off"	summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="rt"		showingroupfootercolumn="wotype"	summarytype="Sum" />
					<dx:aspxsummaryitem fieldname="total"	showingroupfootercolumn="total"		summarytype="Sum" displayformat="{0:c2}" />
				</groupsummary>
				<totalsummary>
					<dx:aspxsummaryitem fieldname="rt"		showincolumn="rt" tag="rt"			summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="ot"		showincolumn="ot"			summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="dt"		showincolumn="dt"			summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="rtsp"	showincolumn="rtsp"			summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="otsp"	showincolumn="otsp"			summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="dtsp"	showincolumn="dtsp"			summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="unpaid_time_off"	showincolumn="unpaid_time_off"		summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="paid_time_off"	showincolumn="paid_time_off"		summarytype="Sum" displayformat="n" />
					<dx:aspxsummaryitem fieldname="total"	showincolumn="total"		summarytype="Sum" displayformat="{0:c2}" />
				</totalsummary>
				<columns>
					<dx:gridviewdatatextcolumn caption="Week Of" fieldname="w" name="w"></dx:gridviewdatatextcolumn>
					<dx:gridviewdatadatecolumn caption="Date" fieldname="date" name="date" width="115px" minwidth="115"></dx:gridviewdatadatecolumn>
					<dx:gridviewdatatextcolumn caption="Customer Name" fieldname="customer_name" name="customer_name">
						<cellstyle horizontalalign="Left" />
						<dataitemtemplate>
							<dx:aspxlabel id="lb_description" runat="server" data-title="Comments (if applicable)" data-tooltip='<%# Server.HtmlEncode(Toolbox.ReturnBlankIfNull_string(Eval("comments"))).Replace("\n", "<br/>") %>' cssclass="opt1" encodehtml="false" text='<%# Eval("customer_name") %>' ondatabinding="lb_description_DataBound" width="100%" />
						</dataitemtemplate>
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Type" fieldname="wotype" width="7%" minwidth="80" name="type">
						<CellStyle CssClass="row_type" />
						<groupfootercellstyle horizontalalign="Right" forecolor="Gray" />
						<footercellstyle horizontalalign="Right"/>
						<groupfootertemplate>
							Week Totals:
						</groupfootertemplate>
						<footertemplate>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_vacation_total" runat="server" text="Vacation Total:" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_unpaid_total" runat="server" text="Unpaid Time Off Total:" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_paid_total" runat="server" text="Paid Time Off Total:" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_hour_total" runat="server" text="Actual Hour Totals:" /></div>
							<div style="min-height:25px;padding-top:5px;"><dx:aspxlabel id="lb_verify_hours" runat="server" text="Verify the Hour Totals:"  height="15px" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_w" runat="server" text="Banked Pay Withdrawn:" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_d" runat="server" text="Banked Pay Deposited:" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_p" runat="server" text="Banked Pay Paid Out:" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_expenses_total" runat="server" text="Total Expenses / Per Diems:" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bonuses_total" runat="server" text="Bonuses Approved:" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_commissions_total" runat="server" text="Commissions Approved:" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_misc_total" runat="server" text="Misc. Payments:" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_finalpayout" runat="server" text="Final Payout:" /></div>
							<asp:hiddenfield id="hid_wage" runat="server" />
							<asp:hiddenfield id="hid_is_salary" runat="server" />
							<asp:hiddenfield id="hid_total_hours" runat="server" />
							<asp:hiddenfield id="hid_total_basepay" runat="server" />
							<asp:hiddenfield id="hid_total_holiday" runat="server" />
							<asp:hiddenfield id="hid_rt_original" runat="server" />
							<asp:hiddenfield id="hid_ot_original" runat="server" />
							<asp:hiddenfield id="hid_dt_original" runat="server" />
							<asp:hiddenfield id="hid_rtsp_original" runat="server" />
							<asp:hiddenfield id="hid_otsp_original" runat="server" />
							<asp:hiddenfield id="hid_dtsp_original" runat="server" />
							<asp:hiddenfield id="hid_total_vacation" runat="server" />
							<asp:HiddenField ID="hid_all_outstanding" runat="server" />
						</footertemplate>				 
					</dx:gridviewdatatextcolumn>	
					<dx:gridviewdatatextcolumn caption="RT" fieldname="rt" name="rt" width="5%" minwidth="35">
						<groupfootercellstyle horizontalalign="Center" forecolor="Black" cssclass="rt_group_summary" />
						<dataitemtemplate>
							<dx:aspxtextbox id="rt" runat="server" width="35px" horizontalalign="Center" theme="NETheme01" cssclass="rt_box" text='<%# Bind("rt") %>' ondatabound="rt_DataBound">
								<clientsideevents textchanged="function(s,e){approval.calculate_hours(s,e,false);}" />
								<disabledstyle forecolor="#333333" border-bordercolor="Transparent" backcolor="Transparent"></disabledstyle>
							</dx:aspxtextbox>
						</dataitemtemplate>
						<GroupFooterTemplate>
							
						</GroupFooterTemplate>
						<footertemplate>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_vacation_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_unpaid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_paid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_hour_total" cssclass="rt_total_summary" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxtextbox font-size="10pt" horizontalalign="Center" runat="server" id="tb_rt" clientinstancename="tb_rt" width="100%" border-bordercolor="Orange" theme="NETheme01">
								<clientsideevents textchanged="function(s,e){approval.calculate_hours(s,e,true);}"  gotfocus="approval.focus.got" lostfocus="approval.focus.lost"/>
							</dx:aspxtextbox></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_w" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_d" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_p" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_expenses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bonuses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_commissions_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_misc_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_finalpayout" runat="server" /></div>
						</footertemplate>
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="OT" fieldname="ot" name="ot" width="5%" minwidth="35" cellstyle-cssclass="ot">
						<footertemplate>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_vacation_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_unpaid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_paid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_hour_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxtextbox font-size="10pt" horizontalalign="Center" runat="server" id="tb_ot" clientinstancename="tb_ot" width="100%" border-bordercolor="Orange"  theme="NETheme01">
								<clientsideevents textchanged="function(s,e){approval.calculate_hours(s,e,true);}" gotfocus="approval.focus.got" lostfocus="approval.focus.lost" />
							</dx:aspxtextbox></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_w" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_d" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_p" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_expenses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bonuses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_commissions_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_misc_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_finalpayout" runat="server" /></div>
						</footertemplate>
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="DT" fieldname="dt" name="dt" width="5%" minwidth="35" cellstyle-cssclass="dt">
						<footertemplate>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_vacation_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_unpaid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_paid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_hour_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxtextbox font-size="10pt" horizontalalign="Center"  runat="server" id="tb_dt" clientinstancename="tb_dt" width="100%" border-bordercolor="Orange" theme="NETheme01">
								<clientsideevents textchanged="function(s,e){approval.calculate_hours(s,e,true);}"  gotfocus="approval.focus.got" lostfocus="approval.focus.lost" />
							</dx:aspxtextbox></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_w" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_d" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_p" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_expenses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bonuses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_commissions_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_misc_total" runat="server"  /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_finalpayout" runat="server" /></div>
						</footertemplate>
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="RTsp" fieldname="rtsp" name="rtsp" width="5%" minwidth="35" cellstyle-cssclass="rtsp">
						<footertemplate>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_vacation_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_unpaid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_paid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_hour_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxtextbox font-size="10pt" horizontalalign="Center" runat="server" id="tb_rtsp" clientinstancename="tb_rtsp" width="100%" border-bordercolor="Orange" theme="NETheme01">
								<clientsideevents textchanged="function(s,e){approval.calculate_hours(s,e,true);}"  gotfocus="approval.focus.got" lostfocus="approval.focus.lost"/>
							</dx:aspxtextbox></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_w" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_d" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_p" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_expenses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bonuses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_commissions_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_misc_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_finalpayout" runat="server" /></div>
						</footertemplate>
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="OTsp" fieldname="otsp" name="otsp" width="5%" minwidth="35" cellstyle-cssclass="otsp">
						<footertemplate>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_vacation_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_unpaid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_paid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_hour_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxtextbox font-size="10pt" horizontalalign="Center" runat="server" id="tb_otsp" clientinstancename="tb_otsp" width="100%" border-bordercolor="Orange" theme="NETheme01">
								<clientsideevents textchanged="function(s,e){approval.calculate_hours(s,e,true);}"  gotfocus="approval.focus.got" lostfocus="approval.focus.lost"/>
							</dx:aspxtextbox></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_w" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_d" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_p" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_expenses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bonuses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_commissions_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_misc_total" runat="server"  /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_finalpayout" runat="server" /></div>
						</footertemplate>
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="DTsp" fieldname="dtsp" name="dtsp" width="5%" minwidth="35" cellstyle-cssclass="dtsp">
						<footertemplate>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_vacation_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_unpaid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_paid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_hour_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxtextbox font-size="10pt" horizontalalign="Center" runat="server" id="tb_dtsp" clientinstancename="tb_dtsp" width="100%" border-bordercolor="Orange" theme="NETheme01">
								<clientsideevents textchanged="function(s,e){approval.calculate_hours(s,e,true);}"  gotfocus="approval.focus.got"  lostfocus="approval.focus.lost"/>
							</dx:aspxtextbox></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_w" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_d" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_p" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_expenses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bonuses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_commissions_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_misc_total" runat="server"  /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_finalpayout" runat="server" /></div>
						</footertemplate>
					</dx:gridviewdatatextcolumn>	 
					<dx:gridviewdatatextcolumn caption="Unpaid Time Off" fieldname="unpaid_time_off" name="unpaid_time_off" width="5%" minwidth="35">
						<footertemplate>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_vacation_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_unpaid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_paid_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_hour_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_verify_hours" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_w" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_d" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_p" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_expenses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_bonuses_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_commissions_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_misc_total" runat="server" /></div>
							<div style="min-height:25px;"><dx:aspxlabel id="lb_finalpayout" runat="server" /></div>
						</footertemplate>
					</dx:gridviewdatatextcolumn>	 
				<dx:gridviewdatatextcolumn caption="Paid Time Off" fieldname="paid_time_off" name="paid_time_off" cellstyle-cssclass="paid_time_off" width="5%" minwidth="35">
					<footertemplate>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_vacation_total" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_unpaid_total" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_timeoff_paid_total" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_hour_total" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_verify_hours" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_w" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_d" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_bankedpay_p" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_expenses_total" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_bonuses_total" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_commissions_total" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_misc_total" runat="server" /></div>
						<div style="min-height:25px;"><dx:aspxlabel id="lb_finalpayout" runat="server" /></div>
					</footertemplate>
				</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Total" fieldname="total" name="total" width="75px" minwidth="75" cellstyle-cssclass="line_total"  >
						<groupfootercellstyle  cssclass="linetotal_group_summary" ></groupfootercellstyle>
						<PropertiesTextEdit DisplayFormatString="{0:c2}"></PropertiesTextEdit>
						<footertemplate>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_vacation_total" runat="server" /></div>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_timeoff_unpaid_total" runat="server" Text="$0.00" /></div>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_timeoff_paid_total" runat="server" /></div>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_hour_total" runat="server" /></div>
							<div style="min-height:25px;padding-top:5px;text-align: right;"><dx:aspxlabel id="lb_verify_total" clientinstancename="lb_verify_total" runat="server" height="15px"/></div>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_bankedpay_w" runat="server" /></div>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_bankedpay_d" runat="server" /></div>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_bankedpay_p" runat="server" /></div>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_expenses_total" runat="server" /></div>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_bonuses_total" runat="server" /></div>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_commissions_total" runat="server" /></div>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_misc_total" runat="server" /></div>
							<div style="min-height:25px;text-align: right;"><dx:aspxlabel id="lb_finalpayout" clientinstancename="lb_finalpayout" runat="server" /></div>
						</footertemplate>
					</dx:gridviewdatatextcolumn>
				</columns>
				<clientsideevents endcallback="function(s,e){page_obj.bindtips();}" init="approval.gv_init" />
				<settingspager mode="ShowAllRecords">
				</settingspager>
				<settings showgroupfooter="VisibleIfExpanded" showgroupbuttons="false"  showgroupedcolumns="false" showfooter="true" />
				<settingsbehavior columnresizemode="Disabled" allowsort="false" allowdragdrop="false" />
				<styles>
					<Header HorizontalAlign="Center"></Header>
					<Cell HorizontalAlign="Center"></Cell>
					<Footer HorizontalAlign="Center"></Footer>
					<GroupFooter HorizontalAlign="Center"></GroupFooter>
				</styles>
			</dx:aspxgridview></td>
				</tr>
			</table>
			<div align="right" >
				<div id="div_status_box" align="center" style="padding-bottom:15px;"></div>
				<div align="center" width="500">
					<dx:aspxbutton runat="server" id="bt_submit"  theme="NETheme01" clientinstancename="bt_submit" text="Submit Hours" width="35%" autopostback="false" height="35px">
					</dx:aspxbutton>
				</div>
			</div>
			<br />
		</dx:panelcontent>
	</panelcollection>
</dx:aspxcallbackpanel>