<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_member_modules_vacation" Codebehind="vacation.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<dx:aspxpagecontrol id="pc_vacations" runat="server" activetabindex="0" theme="NETheme01" width="100%">
	<tabpages>
		<dx:tabpage text="Requests">
			<contentcollection>
				<dx:contentcontrol runat="server">
					<asp:sqldatasource id="sds_requests" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" selectcommand="SELECT a.payperiod_id, a.date_insert, a.date_start, a.date_end, DATE(a.date_return) date_return, b.status, c.method, a.payment_amount amount, a.comments, CONCAT('(', d.payperiodid, ') ', DATE(d.startdate), ' - ', DATE(d.enddate)) payperiod FROM vacation_master a LEFT JOIN vacation_status b ON a.status = b.status_id LEFT JOIN vacation_payment_method c ON a.payment_method = c.method_id LEFT JOIN payperiods d ON a.payperiod_id = d.payperiodid WHERE a.member_id = @member_id AND a.type_id = 4 ORDER BY a.vacation_id DESC">
						<selectparameters>
							<asp:parameter name="@member_id" />
						</selectparameters>
					</asp:sqldatasource>
					<dx:aspxgridview id="gv_vacations" runat="server" autogeneratecolumns="False" datasourceid="sds_requests" theme="NETheme01" width="100%" settingspager-pagesize="50">
						<columns>
							<dx:gridviewdatatextcolumn caption="Pay Period" fieldname="payperiod" showincustomizationform="True" visibleindex="0">
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Status" fieldname="status" showincustomizationform="True" visibleindex="5">
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Method" fieldname="method" showincustomizationform="True" visibleindex="6">
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Amount" fieldname="amount" showincustomizationform="True" visibleindex="7">
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Comment" fieldname="comments" showincustomizationform="True" visibleindex="8">
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatadatecolumn caption="Date Insert" fieldname="date_insert" showincustomizationform="True" visibleindex="1">
								<propertiesdateedit displayformatstring="">
								</propertiesdateedit>
							</dx:gridviewdatadatecolumn>
							<dx:gridviewdatadatecolumn caption="Date Start" fieldname="date_start" showincustomizationform="True" visibleindex="2">
								<propertiesdateedit displayformatstring="">
								</propertiesdateedit>
							</dx:gridviewdatadatecolumn>
							<dx:gridviewdatadatecolumn caption="Date End" fieldname="date_end" showincustomizationform="True" visibleindex="3">
								<propertiesdateedit displayformatstring="">
								</propertiesdateedit>
							</dx:gridviewdatadatecolumn>
							<dx:gridviewdatadatecolumn caption="Date Return" fieldname="date_return" showincustomizationform="True" visibleindex="4">
								<propertiesdateedit displayformatstring="{0:yyyy-MM-dd}">
								</propertiesdateedit>
							</dx:gridviewdatadatecolumn>
						</columns>
						<settings showfilterrow="true" showtitlepanel="true" showheaderfilterbutton="true" showfilterrowmenu="true" /> 
					</dx:aspxgridview>
				</dx:contentcontrol>
			</contentcollection>
		</dx:tabpage>
		<dx:tabpage text="Transactions / Override">
			<contentcollection>
				<dx:contentcontrol runat="server">
					<asp:sqldatasource id="sds_transactions" runat="server" connectionstring="<%$ ConnectionStrings:MySQLdotnet %>" providername="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" selectcommand="SELECT a.id, a.dt, a.value_old, a.value_new, a.value_delta, b.member_fullname changed_by, a.action FROM vacation_transactions a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.member_id = @member_id ORDER BY a.id DESC">
						<selectparameters>
							<asp:parameter name="@member_id" />
						</selectparameters>
					</asp:sqldatasource>
					<dx:aspxcallbackpanel id="cbp_override_holiday" runat="server" clientinstancename="cbp_override_holiday" oncallback="cbp_override_holiday_Callback">
						<panelcollection>
							<dx:panelcontent>
								<table>
									<tr>
										<td valign="middle"><dx:aspxtextbox id="tb_current_holiday" clientinstancename="tb_current_holiday" runat="server" caption="Current Holiday" captionsettings-position="Top" width="50px" captionstyle-forecolor="Black"></dx:aspxtextbox></td>
										<td valign="bottom"><dx:aspxlabel id="lb_dollarhours" runat="server" /></td>
									</tr>
								</table>
								<br />
								<dx:aspxmemo id="memo_override" runat="server" caption="Override Reason?" clientinstancename="memo_override" captionsettings-position="Top" captionstyle-forecolor="Black" width="300px" height="150px"></dx:aspxmemo>
								<br />
								<dx:aspxbutton id="bt_override" runat="server" text="Add Override" clientinstancename="bt_override" autopostback="false">
									<clientsideevents click="function(s,e){cbp_override_holiday.PerformCallback();}"/>
								</dx:aspxbutton>
								<br />
								<br />
							</dx:panelcontent>
						</panelcollection>
						<clientsideevents endcallback="function(s,e){gv_transactions.Refresh();}" />
					</dx:aspxcallbackpanel>
					<dx:aspxgridview id="gv_transactions" clientinstancename="gv_transactions"  runat="server" autogeneratecolumns="False" datasourceid="sds_transactions" theme="NETheme01" width="100%">
						<columns>
							<dx:gridviewdatatextcolumn caption="Value Old" fieldname="value_old" showincustomizationform="True" visibleindex="1">
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Value New" fieldname="value_new" showincustomizationform="True" visibleindex="2">
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Value Delta" fieldname="value_delta" showincustomizationform="True" visibleindex="3">
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatatextcolumn caption="Changed By" fieldname="changed_by" showincustomizationform="True" visibleindex="4">
							</dx:gridviewdatatextcolumn>
							<dx:gridviewdatadatecolumn caption="Date" fieldname="dt" showincustomizationform="True" visibleindex="0">
								<propertiesdateedit displayformatstring="">
								</propertiesdateedit>
							</dx:gridviewdatadatecolumn>
							<dx:gridviewdatatextcolumn caption="Action" fieldname="action" showincustomizationform="True" visibleindex="5">
							</dx:gridviewdatatextcolumn>
						</columns>
						<settings showfilterrow="true" showheaderfilterbutton="true" showfilterrowmenu="true" /> 
					</dx:aspxgridview>
				</dx:contentcontrol>
			</contentcollection>
		</dx:tabpage>
	</tabpages>
</dx:aspxpagecontrol>

