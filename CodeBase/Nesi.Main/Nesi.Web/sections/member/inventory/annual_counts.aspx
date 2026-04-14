<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_member_inventory_annual_counts" Theme="NETheme01" Title="Annual Stock Counts" Codebehind="annual_counts.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
    <div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" runat="Server">
    <style type="text/css">
		.qty_column {
			background-color:lightyellow;
            opacity:0.8;
            
		}
	</style>

	<script type="text/javascript">
var annual_counts = 
	{
	set_company: 
		{
		run:
			function (arg)
			{
				cb_main.PerformCallback();
			}
		},
	chk_marker:
			{
			run:
			function (obj)
				{
				var run_all = false;
				var enabled = true;
				if (obj == undefined)
					{
					run_all = true;
					}
				if (!run_all)
					{
					var val = $(obj).attr("data-id") == undefined || $(obj).attr("data-id") == null ? $(obj).val() : $(obj).attr("data-id");
					if (!annual_counts.chk_marker.number_check(val))
						{
						$(obj).css({ "border": "solid 1px #f00" });
						enabled = false;
						}
					else
						{
						$(obj).css({ "border": "solid 1px #000" });
						}
					}
				else
					{
					var marker_id = $("#add_marker_id").val();
					var master_id = $("#add_master_id").attr("data-id");
					var location_id = $("#add_location_id").attr("data-id");
					var qty = $("#add_qty").val();
					if (!annual_counts.chk_marker.number_check(marker_id))
						{
						$("#add_marker_id").css({ "border": "solid 1px #f00" });
						enabled = false;
						}
						else
						{
						$("#add_marker_id").css({ "border": "solid 1px #000" });
						}

					if (!annual_counts.chk_marker.number_check(master_id))
						{
						$("#add_master_id").css({ "border": "solid 1px #f00" });
						enabled = false;
						}
					else
						{
						$("#add_master_id").css({ "border": "solid 1px #000" });
						}

					if (!annual_counts.chk_marker.number_check(location_id))
						{
						$("#add_location_id").css({ "border": "solid 1px #f00" });
						enabled = false;
						}
					else
						{
						$("#add_location_id").css({ "border": "solid 1px #000" });
						}

					if (!annual_counts.chk_marker.number_check(qty))
						{
						$("#add_qty").css({ "border": "solid 1px #f00" });
						enabled = false;
						}
					else
						{
						$("#add_qty").css({ "border": "solid 1px #000" });
						}
					}
				return enabled
				},
			number_check:
				function (val)
				{
				return val != undefined && val != null && val != "" && !isNaN(val) && val >= 0;
				}
			},
	save_marker:
		{
		run:
			function ()
				{
				var marker_id = $("#add_marker_id").val();
				var master_id = $("#add_master_id").attr("data-id");
				var location_id = $("#add_location_id").attr("data-id");
				var qty = $("#add_qty").val();
				PageMethods.save_marker(marker_id, master_id, location_id, qty, annual_counts.save_marker.complete, annual_counts.error, annual_counts.timeout);
				},
		complete:
			function (arg)
				{
				if (arg == "SUCCESS")
					{
					var marker_id = ($("#add_marker_id").val() / 1) + 1;
					$("#add_marker_id").val(marker_id);
					$("#add_master_id").val("").attr({ "data-id": "", "title": "" });
					$("#add_location_id").val("").attr({ "data-id": "", "title": "" });
					$("#add_qty").val("");
					gv_marker_entries.Refresh();
					$("#add_master_id").focus();
					}
				else
					{
					alert(arg);
					}
				}
		},
	reset_minmax:
		{
		run:
			function ()
				{
				PageMethods.reset_minmax(ddl_branch.GetValue(), annual_counts.reset_minmax.complete, annual_counts.error, annual_counts.timeout);
				},
		complete:
			function (arg)
				{
				if (arg == "SUCCESS")
					{
					alert("All external locations for this branch have been reset to zero");
					}
				else
					{
					alert(arg);
					}
				}
		},
	timeout:
		function (arg)
			{
			alert("Timeout occured");
			},
	error:
		function (arg)
			{
			alert(arg._message);
			},
	zero_out_location:
		function(location_master_id, master_id, business_unit_id)
			{
			if(confirm("Are you sure you want to zero out this location?"))
				{
				cb_zero.PerformCallback(location_master_id+"|"+master_id+"|"+business_unit_id);
				}
			},
	schedule_row_click:
		function(s,e)
			{
			var dates = s.cp_dates[e.visibleIndex].split('|'); 
			var stop	= false;
			if(dates[0] == '0' && dates[1] == '0')
				{
				alert('Please enter a start and an end date for this branch before selecting it.');
				stop	= true;
				} 
			else if(dates[0] == '0')
				{
				alert('Please enter a start date for this branch before selecting it.');
				stop	= true;
				} 
			else if(dates[1] == '0')
				{
				alert('Please enter an end date for this branch before selecting it.');
				stop	= true;
				} 
			e.cancel = s.IsEditing() || stop;
			}
	}
		function save_mmo(s, e)
		{
			var _parent = $(s.mainElement).parents("tr[class*=dxgvDataRow]:first");
			var o = {
				master_id: gv_marker_entries.cpid[s.cpIndex],
				onhand: _parent.find('.onhand').find('input:text').val(),
				//location: 999
				location: _parent.find('._location').val(),
				markerid: _parent.find('.markerid').find('input:text').val(),
				id: _parent.find('.id').val()
			};
			//	alert(o.location);
			cb_saveminmax.PerformCallback(JSON.stringify(o));
		}
		function mmo_rover(s, e) { $(s.mainElement).css({ "border": "solid 1px #f99" }); }
		function mmo_rout(s, e) { $(s.mainElement).css({ "border": "solid 1px transparent" }); }
		function populate_hidden(s)
		{
			var _parent = $(s.mainElement).parents("tr[class*=dxgvDataRow]:first");
			_parent.find("._location").val(s.GetValue());
			//	alert(_parent.find("._location").val());
		}
	function BeginReqHandler()
		{
		please_wait('start');
		}
	function EndReqHandler()
		{
		please_wait('end');
		}
	$(document).ready(function()
		{
		Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
		});
	</script>
	<br />
	
                <asp:SqlDataSource ID="ds_schedules" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
                    SelectCommand="
Select 
	ibo.id,
	ibo.annual_count_start_date start_date, 
	ibo.annual_count_end_date end_date, 
	c.name name, 
	c.id business_unit_id  
from 
	business_unit c
LEFT join 
	inventory_branch_options ibo ON ibo.business_unit_id = c.id 
WHERE c.active = 'T' AND c.has_inventory = 1 AND c.id IN (SELECT DISTINCT warehouse_bu_id FROM business_unit)
order by 
	c.name, ibo.annual_count_start_date " 
                    
                    UpdateCommand="update inventory_branch_options set annual_count_start_date=?start_date, annual_count_end_date=?end_date where id=?id limit 1"
                  
                    >
                    
                   <UpdateParameters>
                         <asp:Parameter Name="id" Type="Int32" />
                       <asp:Parameter Name="start_date" Type="DateTime" />
                        <asp:Parameter Name="end_date" Type="DateTime"  />
                   </UpdateParameters>

                 

                </asp:SqlDataSource>
				<dx:aspxgridview	id                   = "gv_schedules" 
									clientinstancename   = "gv_schedules" 
									datasourceid         = "ds_schedules" 
									runat                = "server" 
									autogeneratecolumns  = "False" 
									keyfieldname         = "id" 
									theme                = "NETheme01" 
									oncustomjsproperties = "gv_schedules_CustomJSProperties" 
									width                = "100%"
									>
					<clientsideevents rowclick="annual_counts.schedule_row_click" selectionchanged="function(s, e) { cb_main.PerformCallback();}" />
					<columns>
						<dx:gridviewcommandcolumn caption=" " showeditbutton="True" showincustomizationform="True" width="50px" visibleindex="0">
						</dx:gridviewcommandcolumn>
						<dx:gridviewdatatextcolumn caption="Business Unit ID" fieldname="business_unit_id" visibleindex="5" Visible="False" readonly="True">
						</dx:gridviewdatatextcolumn>
						<dx:gridviewdatatextcolumn caption="Business Unit" fieldname="name" visibleindex="1" readonly="True">
						</dx:gridviewdatatextcolumn>
						<dx:gridviewdatatextcolumn caption="id" fieldname="id" visible="False" visibleindex="2">
						</dx:gridviewdatatextcolumn>
						<dx:gridviewdatadatecolumn caption="Start Date" fieldname="start_date" visibleindex="3">
							<propertiesdateedit displayformatstring="yyyy-MM-dd" editformat="Custom" editformatstring="yyyy-MM-dd">
							</propertiesdateedit>
						</dx:gridviewdatadatecolumn>
						<dx:gridviewdatadatecolumn caption="End Date" fieldname="end_date" visibleindex="4">
							<propertiesdateedit displayformatstring="yyyy-MM-dd" editformat="Custom" editformatstring="yyyy-MM-dd">
							</propertiesdateedit>
						</dx:gridviewdatadatecolumn>
					</columns>
					<settingsbehavior allowselectsinglerowonly="True" confirmdelete="True" allowselectbyrowclick="True" />
					<settingsediting mode="Inline">
					</settingsediting>
					<settings showtitlepanel="True" showheaderfilterbutton="True" />
					<settingstext title="Annual Count Schedules (Select branch for details)" />
					<styles>
						<selectedrow backcolor="#FFCC00">
						</selectedrow>
					</styles>
				</dx:aspxgridview>
	<dx:ASPxCallbackPanel ID="cb_main" runat="server" ClientInstanceName="cb_main" OnCallback="ASPxCallbackPanel1_Callback" Width="100%">
		<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" CallbackError="function(s, e) {
	please_wait(&quot;end&quot;);
}" EndCallback="function(s, e) {
	please_wait(&quot;end&quot;);
}" />
		<PanelCollection>
			<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<br />
				
                <br />
				
				<br />
                                                <asp:ScriptManager runat="server" ID="sm">
	</asp:ScriptManager>
												
				<dx:ASPxPageControl ID="pc" runat="server" ActiveTabIndex="1" Width="100%">
					<TabPages>
						<dx:TabPage Text="All Branch Items">
							<ContentCollection>
								<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
									<table style="width: 100%; white-space: nowrap">
										<tr>
											<td nowrap="nowrap" style="white-space: nowrap; color: #0033CC;">
											    <asp:SqlDataSource ID="ds_remaining" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
                                                    ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
                                                    SelectCommand="call get_inventory_locations_with_qtys(@business_unit_id,@start_date,@end_date)">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="business_unit_id" SessionField="working_business_unit_id" />
                                                        <asp:SessionParameter Name="start_date" SessionField="working_dte_datestart" />
                                                        <asp:SessionParameter Name="end_date" SessionField="working_dte_dateend" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
												
											    <uc1:layout_control ID="layout" runat="server" export_filename="annual_counts" ShowToggle="false" is_private="True" GridviewID="gv_remaining" />
												
											</td>
										</tr>
										<tr>
											<td class="style2">
												<dx:aspxgridview id="gv_remaining" runat="server" autogeneratecolumns="False" 
                                                    clientinstancename="gv_remaining" width="100%" keyfieldname="id" 
                                                    onhtmldatacellprepared="gv_remaining_HtmlDataCellPrepared" onrowupdating="gv_remaining_RowUpdating" 
                                                    oncustomcallback="gv_remaining_CustomCallback" oncustomjsproperties="gv_remaining_CustomJSProperties" datasourceid="ds_remaining">
													<clientsideevents batcheditendediting="function(s, e) {}" 
                                                        begincallback="function(s, e) {gv_remaining.SetEnabled(false);please_wait('start');}" endcallback="function(s, e) {gv_remaining.SetEnabled(true);please_wait('stop');}" />
													<totalsummary>
														<dx:aspxsummaryitem displayformat="C2" fieldname="dollar_delta" showincolumn="$ Delta" summarytype="Sum" />
														<dx:aspxsummaryitem displayformat="C2" fieldname="dollar_balance" showincolumn="dollar_balance" summarytype="Sum" />
													</totalsummary>
													<columns>
														<dx:gridviewcommandcolumn showincustomizationform="True" visible="False" visibleindex="0">
															<headerstyle horizontalalign="Center" wrap="True" />
														</dx:gridviewcommandcolumn>
														<dx:gridviewdatatextcolumn caption="Location" fieldname="location" showincustomizationform="True" visibleindex="1" width="100px" readonly="True">
															<editformsettings visible="False" />
															<cellstyle horizontalalign="Center">
															</cellstyle>
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="Part" fieldname="master_id" showincustomizationform="True" visibleindex="2" width="55px" readonly="True">
															<editformsettings visible="False" />
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="Description" fieldname="description" showincustomizationform="True" visibleindex="3" width="100%" readonly="True">
															<editformsettings visible="False" />
															<cellstyle horizontalalign="Left" wrap="True">
															</cellstyle>
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="Pre Annual Count Qty" fieldname="prev_qty" showincustomizationform="True" width="80px" visibleindex="4">
															<editformsettings visible="False" />
															<headerstyle horizontalalign="Center" wrap="True" />
															<cellstyle horizontalalign="Center">
															</cellstyle>
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="Post Annual Count Qty" fieldname="post_qty" showincustomizationform="True" visibleindex="5" width="75px" readonly="True">
															<editformsettings visible="False" />
															<headerstyle horizontalalign="Center" wrap="True" />
															<cellstyle wrap="True" horizontalalign="Center" paddings-paddingright="10px">
																<paddings paddingright="10px" />
															</cellstyle>
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="Qty Adjustment" showincustomizationform="True" width="90px" visibleindex="6" fieldname="newqty" editformsettings-caption="test">
															<editformsettings caption="test" />
															<headerstyle horizontalalign="Center" wrap="True" />
															<cellstyle horizontalalign="Center" cssclass="qty_column">
															</cellstyle>
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="Cost" fieldname="cost" showincustomizationform="True" visibleindex="7" width="75px" readonly="True">
															<propertiestextedit displayformatstring="C2"></propertiestextedit>
															<editformsettings visible="False" />
															<headerstyle horizontalalign="Center" />
															<cellstyle horizontalalign="Center">
															</cellstyle>
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="Qty Delta" fieldname="qty_delta" showincustomizationform="True" visible="True" visibleindex="8" width="50px" readonly="True">
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="$ Delta" fieldname="dollar_delta" showincustomizationform="True" visible="True" visibleindex="9" width="50px" readonly="True">
															<propertiestextedit displayformatstring="C2"></propertiestextedit>
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatadatecolumn caption="Last Used" fieldname="last_used" readonly="True" showincustomizationform="True" visibleindex="10" width="90px">
															<propertiesdateedit displayformatstring="yyyy-MM-dd HH:mm:ss" editformat="Custom" editformatstring="yyyy-MM-dd HH:mm:ss"></propertiesdateedit>
															<editformsettings visible="False" />
															<headerstyle horizontalalign="Center" />
														</dx:gridviewdatadatecolumn>
														<dx:gridviewdatadatecolumn caption="Last Counted" fieldname="last_inv_count" readonly="True" showincustomizationform="True" visibleindex="11" width="90px">
															<propertiesdateedit displayformatstring="yyyy-MM-dd HH:mm:ss" editformat="Custom" editformatstring="yyyy-MM-dd HH:mm:ss"></propertiesdateedit>
															<editformsettings visible="False" />
															<headerstyle horizontalalign="Center" />
														</dx:gridviewdatadatecolumn>
														<dx:gridviewdatatextcolumn caption="Count Entries" showincustomizationform="True" visibleindex="12" width="100px" fieldname="count_hist" readonly="True">
															<editformsettings visible="False" />
														</dx:gridviewdatatextcolumn>



														<dx:gridviewdatatextcolumn caption="id" fieldname="id" showincustomizationform="True" visible="False" visibleindex="13">
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="location_master_id" fieldname="location_master_id" showincustomizationform="True" visible="False" visibleindex="14">
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="$ Total" width="75px" fieldname="dollar_balance" showincustomizationform="True" visible="True" visibleindex="15">
															<propertiestextedit displayformatstring="C2"></propertiestextedit>
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="qty" fieldname="qty" showincustomizationform="True" visible="False" visibleindex="16">
														</dx:gridviewdatatextcolumn>
														<dx:gridviewdatatextcolumn caption="Is Consumable" fieldname="is_consumable" showincustomizationform="True" visibleindex="17" width="40px">
														</dx:gridviewdatatextcolumn>
													</columns>
													<settingsbehavior enablerowhottrack="True" columnresizemode="Control" />
													<settingspager pagesize="300">
													</settingspager>
													<settingsediting mode="Batch">
													</settingsediting>
													<settings showfilterrow="True" showfilterrowmenu="True" showheaderfilterbutton="True" showtitlepanel="True" showgrouppanel="True" showfooter="True" />
													<settingssearchpanel visible="True" />
													<styleseditors>
														<textbox>
															<paddings padding="0px" />
														</textbox>
													</styleseditors>
													<templates>
														<titlepanel>
															<dx:aspxbutton id="btnZeroOut" runat="server" clientinstancename="btnZeroOut" 
                                                                onclick="btnZeroOut_Click" ondatabinding="btnZeroOut_DataBinding" text="Update all uncounted items to 0" width="150px">
																<clientsideevents click="function(s, e) {
	e.processOnServer = confirm(&quot;Are you sure you want to zero out all uncounted item locations?&quot;);
}" />
															</dx:aspxbutton>
														</titlepanel>
													</templates>
												</dx:aspxgridview>
												<br />
                                               
                                                
											</td>
										</tr>
									</table>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
						<dx:TabPage Text="Zeroed Out Locations">
							<ContentCollection>
								<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
								
									<dx:ASPxGridView ID="gv_zeroed" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_zeroed" Width="100%" KeyFieldName="id" DataSourceID="ds_zeroed_entries" OnDataBound="gv_zeroed_DataBound">
										<Columns>
											<dx:GridViewDataTextColumn Caption="Part #" FieldName="master_id" ShowInCustomizationForm="True" VisibleIndex="0" Width="75px">
												<CellStyle HorizontalAlign="Center">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Location" FieldName="location" ShowInCustomizationForm="True" VisibleIndex="1" Width="100px">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Qty" FieldName="qty" ShowInCustomizationForm="True" VisibleIndex="2" Width="75px">
												<CellStyle HorizontalAlign="Center">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Description" FieldName="description" ShowInCustomizationForm="True" VisibleIndex="3" Width="100%">
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Count Date" FieldName="date_transferred" ShowInCustomizationForm="True" VisibleIndex="5" Width="75px">
												<CellStyle HorizontalAlign="Center">
												</CellStyle>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Employee" FieldName="employee" ShowInCustomizationForm="True" VisibleIndex="4">
											</dx:GridViewDataTextColumn>
										</Columns>
										<SettingsBehavior EnableRowHotTrack="True" />
										<SettingsPager PageSize="50">
										</SettingsPager>
										<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowGroupPanel="True"/>
									</dx:ASPxGridView>
									<asp:SqlDataSource ID="ds_zeroed_entries" runat="server" 
                                        ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT
	a.masterid master_id,
	a.post_onhand_qty-a.pre_onhand_qty qty,
	c.description description,
	a.ts date_transferred,
	d.name location,
	f.member_fullname employee
FROM
	inventory_annual_counts_history a
LEFT JOIN 
	inventory_description c ON a.masterid = c.master_id
LEFT JOIN 
	inventory_location_master d ON a.locationid = d.id
LEFT JOIN
	inventory_cost e ON a.masterid = e.master_id AND a.business_unit_id = e.business_unit_id
LEFT JOIN
	member f ON a.memberid = f.member_id
WHERE
	a.business_unit_id = @business_unit_id and 
	a.date_transferred BETWEEN @date_start AND DATE_ADD(@date_end, INTERVAL 1 DAY) AND
	a.origin = 'zero out'">
										<SelectParameters>
											<asp:SessionParameter DefaultValue="" Name="@business_unit_id" SessionField="working_business_unit_id" />
                                            <asp:SessionParameter Name="@date_start" SessionField="working_dte_datestart" />
                                            <asp:SessionParameter Name="@date_end" SessionField="working_dte_dateend" />
										</SelectParameters>
									</asp:SqlDataSource>
								</dx:ContentControl>
							</ContentCollection>
						</dx:TabPage>
					</TabPages>
				</dx:ASPxPageControl>
				<br />
				<br />
	<dx:ASPxCallback ID="cb_saveminmax" runat="server" ClientInstanceName="cb_saveminmax" OnCallback="cb_saveminmax_Callback">
		<ClientSideEvents BeginCallback="function(s, e) {
	please_wait('start');
}" CallbackComplete="function(s, e) {
	please_wait('stop');
}" CallbackError="function(s, e) {
	please_wait('stop');
}" />
	</dx:ASPxCallback>
				<dx:ASPxCallback ID="cb_zero" runat="server" ClientInstanceName="cb_zero" OnCallback="cb_zero_Callback">
					<ClientSideEvents BeginCallback="function(s, e) {
	please_wait('start');
}" CallbackComplete="function(s, e) {
	please_wait('stop');
	cb_main.PerformCallback();
}" CallbackError="function(s, e) {
	please_wait('stop');
}" />
				</dx:ASPxCallback>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>
		<div align="left">
				<div align="left" style="width:1024px">
					<table>
						<tr>
							<td width="150">
								<dx:ASPxButton ID="bt_export" runat="server" OnClick="bt_export_Click" Text="Export Remaining Items" Visible="True">
									<Image Url="~/images/icon/icon[excel].gif">
									</Image>
								</dx:ASPxButton>
							</td>
							<td width="150">
								<dx:ASPxButton ID="bt_export_zeroed" runat="server" OnClick="bt_export_zeroed_Click" Text="Export Zeroed Out Items" Visible="True">
									<Image Url="~/images/icon/icon[excel].gif">
									</Image>
								</dx:ASPxButton>
							</td>
							<td width="150">
								<dx:ASPxButton ID="bt_import" runat="server" AutoPostBack="False" Text="CSV Importer" Width="150px" Visible="False">
									<ClientSideEvents Click="function(s, e) {
	boing(&quot;./annual_counts_import.aspx?business_unit_id=&quot;+ddl_branch.GetValue(), &quot;AnnualImporter&quot;, 750, 550);
}" />
									<Image Url="~/images/icon/icon[popup].gif">
									</Image>
								</dx:ASPxButton>
							</td>
							<td width="150">
								<dx:ASPxButton ID="bt_resetexternals" runat="server" AutoPostBack="False" Text="Reset Externals" Visible="False" Width="150px">
									<ClientSideEvents Click="function(s, e) {
	if(confirm(&quot;Are you sure you want to reset ALL external locations MIN/MAX values for this branch?&quot;))
		{
		annual_counts.reset_minmax.run()
		}
}" />
									<Image Url="~/images/icon/icon[delete].gif">
									</Image>
								</dx:ASPxButton>
							</td>
							<td>
								<dx:ASPxButton ID="bt_export_wo_snapshot" runat="server" OnClick="bt_export_wo_snapshot_Click" Text="Export Open WO Snapshot">
									<Image Url="~/images/icon/icon[excel].gif">
									</Image>
								</dx:ASPxButton>

							</td>
						</tr>
					</table>
				</div>
			</div>
	<br />
	<dx:ASPxGridViewExporter runat="server" ID="gv_export" OnRenderBrick="gv_export_RenderBrick">
	</dx:ASPxGridViewExporter>
	<br />
	&nbsp;
            
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" runat="Server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="header_placeholder">
    <style type="text/css">
		.style2 { text-align: left; }
	</style>
</asp:Content>
