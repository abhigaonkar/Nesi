<%@ control language="C#" autoeventwireup="true" inherits="sections_hr_payroll_submit_summary" Codebehind="summary.ascx.cs" %>
<%@ register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<dx:aspxcallbackpanel id="cbp_summary" runat="server" clientinstancename="cbp_summary" oncallback="cbp_summary_Callback">
	<panelcollection>
		<dx:panelcontent>
			<div runat="server" id="div_wage_priv" visible="false" style="color:#f60;font-size:1.35em;">* Not showing wages or totals currently because your account does not have the needed privilege.</div>
			<dx:aspxgridview id="gv_summary" clientinstancename="gv_summary" runat="server" theme="NeTheme01" width="100%" autogeneratecolumns="False" keyfieldname="id" onhtmldatacellprepared="gv_summary_HtmlDataCellPrepared">
				<totalsummary>
					<dx:aspxsummaryitem displayformat="N2" fieldname="RT" showincolumn="RT" summarytype="Sum" tag="RT" valuedisplayformat="N2" />
					<dx:aspxsummaryitem displayformat="N2" fieldname="OT" showincolumn="OT" summarytype="Sum" tag="OT" valuedisplayformat="N2" />
					<dx:aspxsummaryitem displayformat="N2" fieldname="DT" showincolumn="DT" summarytype="Sum" tag="DT" valuedisplayformat="N2" />
					<dx:aspxsummaryitem displayformat="N2" fieldname="RTsp" showincolumn="RTsp" summarytype="Sum" tag="RTsp" valuedisplayformat="N2" />
					<dx:aspxsummaryitem displayformat="N2" fieldname="OTsp" showincolumn="OTsp" summarytype="Sum" tag="OTsp" valuedisplayformat="N2" />
					<dx:aspxsummaryitem displayformat="N2" fieldname="DTsp" showincolumn="DTsp" summarytype="Sum" tag="DTsp" valuedisplayformat="N2" />
					<dx:aspxsummaryitem displayformat="N2" fieldname="unpaid" showincolumn="unpaid" summarytype="Sum" tag="Unpaid" valuedisplayformat="N2" />
					<dx:aspxsummaryitem displayformat="N2" fieldname="holiday_total" showincolumn="holiday_total" summarytype="Sum" tag="holiday_total" valuedisplayformat="N2" />
					<dx:aspxsummaryitem displayformat="N2" fieldname="vacation_amount" showincolumn="vacation_amount" summarytype="Sum" tag="vacation_amount" valuedisplayformat="N2" />
					<dx:aspxsummaryitem displayformat="N2" fieldname="total_shop" showincolumn="total_shop" summarytype="Sum" tag="total_shop" valuedisplayformat="N2" />
					<dx:aspxsummaryitem displayformat="C2" fieldname="bonus" showincolumn="bonus" summarytype="Sum" tag="bonus" valuedisplayformat="C2" />
					<dx:aspxsummaryitem displayformat="C2" fieldname="commission" showincolumn="commission" summarytype="Sum" tag="commission" valuedisplayformat="C2" />
					<dx:aspxsummaryitem displayformat="C2" fieldname="expense_total" showincolumn="expense_total" summarytype="Sum" tag="expense_total" valuedisplayformat="C2" />
					<dx:aspxsummaryitem displayformat="C2" fieldname="total" showincolumn="total" summarytype="Sum" tag="total" valuedisplayformat="C2" />
				</totalsummary>
				<columns>
					<dx:gridviewdatatextcolumn fieldname="id" readonly="True" visible="False" visibleindex="0">
						<editformsettings visible="False" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn fieldname="member_id" visible="False" visibleindex="1">
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="F. Name" fieldname="fname" visibleindex="3">
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="L. Name" fieldname="lname" visibleindex="4">
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Pay Type" fieldname="paytype" visibleindex="5">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn fieldname="RT" visibleindex="8">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn fieldname="OT" visibleindex="9">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn fieldname="DT" visibleindex="10">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn fieldname="RTsp" visibleindex="11">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn fieldname="OTsp" visibleindex="12">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn fieldname="DTsp" visibleindex="13">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Unpaid" fieldname="unpaid" visibleindex="14">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>

					<dx:gridviewdatatextcolumn caption="Expenses" fieldname="expense_total" visibleindex="15">
						<propertiestextedit displayformatstring="C2">
						</propertiestextedit>
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Holiday" fieldname="holiday_total" visibleindex="16">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Shop" fieldname="total_shop" visibleindex="17">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Wage" fieldname="wage" visibleindex="7">
						<propertiestextedit displayformatstring="C2">
						</propertiestextedit>
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn fieldname="applicable_holiday" visible="False" visibleindex="18">
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Vacation" fieldname="vacation_hours" visibleindex="19">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Commission" fieldname="commission" visibleindex="20">
						<cellstyle horizontalalign="Center" />
						<propertiestextedit displayformatstring="C2">
						</propertiestextedit>
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Bonus" fieldname="bonus" visibleindex="21">
						<cellstyle horizontalalign="Center" />
						<propertiestextedit displayformatstring="C2">
						</propertiestextedit>
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Misc" fieldname="misc" visibleindex="22">
						<cellstyle horizontalalign="Center" />
						<propertiestextedit displayformatstring="C2">
						</propertiestextedit>
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Banked D" fieldname="banked_d" visibleindex="23">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Banked W" fieldname="banked_w" visibleindex="24">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Banked P" fieldname="banked_p" visibleindex="25">
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Member Type" fieldname="membertype_name" visibleindex="2" width="150px">
						<cellstyle wrap="True">
						</cellstyle>
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatatextcolumn caption="Total" fieldname="total" visibleindex="65">
						<propertiestextedit displayformatstring="C2">
						</propertiestextedit>
						<cellstyle horizontalalign="Center" />
					</dx:gridviewdatatextcolumn>
					<dx:gridviewdatabuttoneditcolumn caption="Edit" visibleindex="66">
						<cellstyle horizontalalign="Center" />
						<dataitemtemplate>
							<dx:aspxbutton id="bt_edit" runat="server" autopostback="false" oninit="bt_edit_Init">
								<image url="/images/icon/icon[edit].gif" height="16px" width="16px" />
							</dx:aspxbutton>
						</dataitemtemplate>
					</dx:gridviewdatabuttoneditcolumn>
				</columns>
				<settingspager mode="ShowAllRecords" pagesize="100" numericbuttoncount="50">
					
				</settingspager>
				<settings showfooter="True" showgrouppanel="True" />
				<styles>
					<header horizontalalign="Center">
					</header>
					<footer horizontalalign="Center">
					</footer>
				</styles>
			</dx:aspxgridview>
			<table style="width:100%;" cellpadding="2" cellspacing="0">
				<tr>
					<td style="width:50%;" valign="top">
						
						<dx:aspxgridview id="gv_notapproved" clientinstancename="gv_notapproved" runat="server" autogeneratecolumns="False" theme="NeTheme01" width="100%">
							<columns>
								<dx:gridviewdatatextcolumn caption="Name" fieldname="name" showincustomizationform="True" visibleindex="0">
								</dx:gridviewdatatextcolumn>
							</columns>
							<settingspager mode="ShowAllRecords">
							</settingspager>
							<settings showtitlepanel="True" />
							<settingstext title="Employees in branch, not yet approved" />
						</dx:aspxgridview>
						<br />
						
					</td>
					<td style="width:50%;" valign="top">
						
						<dx:aspxgridview id="gv_withouthours" clientinstancename="gv_withouthours" runat="server" theme="NeTheme01" width="100%">
							<columns>
								<dx:gridviewdatatextcolumn caption="Name" fieldname="name" showincustomizationform="True" visibleindex="0">
								</dx:gridviewdatatextcolumn>
							</columns>
							<settingspager mode="ShowAllRecords">
							</settingspager>
							<settings showtitlepanel="True" />
							<settingstext title="Employees in branch, without hours" />
						</dx:aspxgridview>
						<br />
						
					</td>
				</tr>
			</table> 
			<div id="div_unapproved_bonuses" runat="server" visible="false" style="text-align:center;font-size: 15px;font-weight:bold;color:#f00;">
				There are unapproved bonuses, commissions or miscellaneous payments that need to be handled before payroll can be submitted.
			</div>
			<div id="div_unapproved_vacations" runat="server" visible="false" style="text-align:center;font-size: 15px;font-weight:bold;color:#f00;">
				There are unapproved vacations that need to be handled before payroll can be submitted.
				<div runat="server" id="div_unapproved_employees" style="text-align:center;font-size: 12px;font-weight:bold;color:#000;"></div>
			</div>
			<div id="div_other_approvers" runat="server" visible="false" style="text-align:center;font-size: 12px;font-weight:bold;color:#f00;">
				
			</div>
			<div style="text-align:center;padding-top: 50px;">
				<dx:aspxbutton id="bt_finalsubmit" runat="server" clientvisible="false" autopostback="false" clientinstancename="bt_finalsubmit" encodehtml="false" text="Complete Payroll<div style='font-size:11px;color:#f00 !important;margin-top:4px;'>This will send everything you have submitted to the payroll department.<br/><u>No further submissions are allowed.</u></div>" width="250px">
					<clientsideevents click="approval.finalize_payroll" />
				</dx:aspxbutton>
			</div>
		</dx:panelcontent>
	</panelcollection>
	<clientsideevents endcallback="approval.end_callback" />
</dx:aspxcallbackpanel>

