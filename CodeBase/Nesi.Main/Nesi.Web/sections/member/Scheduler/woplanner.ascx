<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_scheduler_woplanner" Codebehind="woplanner.ascx.cs" %>
<%@ Register tagPrefix="dx" namespace="DevExpress.Web" assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register tagPrefix="dx" namespace="DevExpress.Web.ASPxScheduler" assembly="DevExpress.Web.ASPxScheduler.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register assembly="DevExpress.XtraScheduler.v19.2.Core, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>
<%@ Register src="hourview.ascx" tagname="hourview" tagprefix="uc2" %>
<%@ Register src="inlinewoplanner.ascx" tagname="inlinewoplanner" tagprefix="uc1" %>
 <style type="text/css">
        .dropTargetActive {
            border-color: #ff0000;
        }
        .dropTargetHover {
           border-color: #ff0000;
        }
       
		#draggable3 {width: 500px; height: 500px; padding: 0em; padding-top: 0em; z-index:200; }
		
	
     .style3
	 {
		 height: 31px;
	 }
		.shadow
	{
		box-shadow: 20px 20px 20px rgba(0, 0, 0, 0.5);
		
	}
	
     .style4
	 {
		 font-family: Arial;
		 text-align: left;
	 }
		
	
     .style6
	 {
		 height: 82px;
	 }
		
	
     .style1
	 {
		 text-align: right;
	 }
		
	
     .style2
	 {
		 text-align: center;
	 }
		
	
     .style7
	 {
		 height: 82px;
		 font-family: Arial;
	 }
	 .style8
	 {
		 font-family: Arial;
	 }
		
	
     </style>
<script type="text/javascript" language="javascript">
	function recurrenceDateChanged(s, e) {
		var selectedWeekends = [];
		var selectedDates = s.GetSelectedDates();
		if (chkallow_weekends.GetValue() == 0) {
			for (i = 0; i < selectedDates.length; i++) {
				if (selectedDates[i].getDay() == 6 || selectedDates[i].getDay() == 0) {
					selectedWeekends[selectedWeekends.length] = selectedDates[i];
				}
			}
			for (i = 0; i < selectedWeekends.length; i++) {
				s.DeselectDate(selectedWeekends[i]);
			}
		}
	}
	function DefaultViewMenuHandler(scheduler, s, e) {
		
		if (e.item.GetItemCount() <= 0) {
			if (e.item.name == "GotoDate") {
				scheduler.RaiseCallback('MNUVIEW|GotoDate')
			}
			if (e.item.name == "GotoToday") {
				scheduler.RaiseCallback('MNUVIEW|GotoToday')
			}
		}
		if (e.item.name == "dayoff") {
			scheduler.RaiseCallback("MYAPTMENU|" + e.item.name);
		}
		if (e.item.name == "_open") {
			pop_edit.cp_aptid = scheduler.GetSelectedAppointmentIds()[0]; pop_edit.Show(); pop_edit.PerformCallback(pop_edit.cp_aptid); 
		}
		if (e.item.name == "DeleteAppointment") {
			var apt = scheduler.GetAppointmentById(scheduler.GetSelectedAppointmentIds()[0]);
			var aptStatusID = apt.statusIndex;
			
			if (aptStatusID == 70) {
				scheduler.PerformCallback("DELETE_FREEZE|" + scheduler.GetSelectedAppointmentIds()[0]);
			}
			else {
				scheduler.RaiseCallback("MYAPTMENU|" + e.item.name);
			}
		}
		if (e.item.name == "OpenAppointment") {
			scheduler.RaiseCallback("MYAPTMENU|" + e.item.name);
		}
		if (e.item.name == "addrecurrence") {
			hf.Set('res', scheduler.GetSelectedAppointmentIds()[0]);
			pop_recurrence.Show();
			pop_recurrence.PerformCallback(scheduler.GetSelectedAppointmentIds()[0]);
		}
		if (e.item.name == "freeze") {
			pop_freeze.Show();
		}
	}
	function appointmentMenu_PopUp(s, e) {
		var openItem = e.item.GetItemByName("addevent");
		var openItem1 = e.item.GetItemByName("dayoff");
		if (openItem != null) {
			openItem.SetVisible(true);
			if (scheduler.GetSelectedResource() > 100000000) {
				openItem.SetVisible(false);
			}
		}
		if (openItem1 != null) {
			openItem1.SetVisible(true);
			if (scheduler.GetSelectedResource() > 100000000) {
				openItem1.SetVisible(false);
			}
		}
	}
    	function InitalizejQuery(s, e) {
    		$('.draggable').draggable({ helper: 'clone', appendTo: 'body', zIndex: 100 });
    		$('.droppable').droppable({
    			activeClass: "dropTargetActive",
    			hoverClass: "dropTargetHover",
    			drop: function (ev, ui) {
    				// Make a clone of the dragged item
    				var clone = (ui.draggable).clone();
    			//	alert($(ui.draggable).attr("id"));
    				if ($(ui.draggable).attr("id") != "draggable3") {
    					// Get a row index:
    					row = $(clone).find("input[type='hidden']").val();
    					if (row != 'N') {
    						hf.Set('row', row);
    						// Calculate an active time cell
    						var cell = scheduler.CalcHitTest(ev).cell;
    						// Initiate a scheduler callback to create an appointment based on a cell interval
    						if (cell != null) {
    							scheduler.getCellInfoProvider().initializeCell(cell);
    							hf.Set('res', cell.resource);
    							scheduler.RaiseCallback('CRTAPT|' + ASPx.DateUtils.GetInvariantDateTimeString(cell.interval.start));
    						}
    						else
    							alert('Drop the dragged item on a specific time cell.');
    					}
    				}
    				// Additional logic goes here...
    			}
    		}
             );
    	}
    	$(function () {
    		$("#draggable3").draggable();
    		
    	});
    </script>
    
   
	
    <div>
        <table style="font-family: Arial, Helvetica, sans-serif; border-collapse: collapse;">
            <tr>
                <td style="vertical-align: top; text-align: right;">
                    <dx:ASPxComboBox ID="ddlbranch" runat="server" AutoPostBack="True" 
						DataSourceID="sqlbranch" Font-Names="Arial" TextField="name" 
						ValueField="business_unit_id" ValueType="System.Int32" 
						onselectedindexchanged="ddlbranch_SelectedIndexChanged" Theme="NETheme01" Caption="Filter by Business Unit" Width="100%">
                        <CaptionSettings VerticalAlign="Middle" />
                        <CaptionCellStyle Width="150px">
                        </CaptionCellStyle>
					</dx:ASPxComboBox>
					<asp:SqlDataSource ID="sqlbranch" runat="server" 
						ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
						ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
						>
					</asp:SqlDataSource>
                </td>
                <td style="vertical-align: top" nowrap="nowrap">
                    &nbsp;</td>
                <td style="vertical-align: top">
                    &nbsp;</td>
                <td style="vertical-align: top" width="100%">
                    <dx:ASPxComboBox ID="ddl_viewresources" runat="server" AutoPostBack="True" 
						onselectedindexchanged="ddl_viewresources_SelectedIndexChanged" 
						SelectedIndex="0" ClientVisible="False" Font-Names="Arial">
						<Items>
							<dx:ListEditItem Selected="True" Text="All" Value="All" />
							<dx:ListEditItem Text="Licensed" Value="Licensed" />
							<dx:ListEditItem Text="Apprentices" Value="Apprentices" />
							<dx:ListEditItem Text="PM" Value="PM" />
						</Items>
					</dx:ASPxComboBox>
				</td>
                <td style="vertical-align: top">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: right;">
                    <dx:ASPxComboBox ID="ddlpm" runat="server" ClientInstanceName="ddlpm" 
						DataSourceID="sqlpm" TextField="_name" 
						ValueField="_id" ValueType="System.Int32" AutoPostBack="True" Font-Names="Arial" 
						Theme="NETheme01" onselectedindexchanged="ddlpm_SelectedIndexChanged" Caption="Filter by Reports To" Width="100%">
                        <CaptionSettings VerticalAlign="Middle" />
                        <CaptionCellStyle Width="150px">
                        </CaptionCellStyle>
					</dx:ASPxComboBox>
                </td>
                <td style="vertical-align: top" nowrap="nowrap">
                    &nbsp;</td>
                <td style="vertical-align: top" nowrap="nowrap">
                    &nbsp;</td>
                <td style="vertical-align: top" width="100%">
                    &nbsp;</td>
                <td style="vertical-align: top">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: right;">
                    <dx:ASPxComboBox ID="ddlcustomer" runat="server" ClientInstanceName="ddlcustomer" 
						DataSourceID="Sqlcustomer" TextField="woprog_customername" 
						ValueField="woprog_customer_id" ValueType="System.Int32" AutoPostBack="True" 
						Font-Names="Arial" onselectedindexchanged="ddlcust_SelectedIndexChanged" Theme="NETheme01"
						EnableCallbackMode="True" IncrementalFilteringMode="StartsWith" Caption="Filter by Customer" Width="100%">
                        <CaptionSettings VerticalAlign="Middle" />
                        <CaptionCellStyle Width="150px">
                        </CaptionCellStyle>
					</dx:ASPxComboBox>
                </td>
                <td style="vertical-align: top" nowrap="nowrap">
                    &nbsp;</td>
                <td style="vertical-align: top" nowrap="nowrap">
                    &nbsp;</td>
                <td style="vertical-align: top" width="100%">
                    &nbsp;</td>
                <td style="vertical-align: top">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="5">
                   
                        <table style="width:100%; border-collapse: collapse;" cellpadding="0">
							<tr>
								<td colspan="2" class="style3">
									<asp:SqlDataSource ID="sqlpm" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										
										
										SelectCommand="Select 0 _id, 'Show All' _name UNION Select distinct (reports_to) _id, get_name(reports_to) _name from member where business_unit_id = ?cid and member_status ='Active'">
										<SelectParameters>
											<asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
									<asp:SqlDataSource ID="Sqlcustomer" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										
										SelectCommand="
(
	SELECT 
		0 woprog_customer_id, 
		'Choose Customer Filter' woprog_customername, 1 priority) 
UNION 
(
	SELECT 
		DISTINCT a.woprog_customer_id, 
		a.woprog_customername,
		2 priority 
	FROM 
		woprog a 
	WHERE 
		a.business_unit_id = ?cid AND 
		a.woprog_status = 'Open' AND 
		a.woprog_pm_memberid IN (SELECT member_id FROM member WHERE reports_to = ?pmid)
)
order by 
	priority,woprog_customername">
										<SelectParameters>
											<asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
											<asp:ControlParameter ControlID="ddlpm" Name="pmid" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>
								</td>
								<td class="style3">
									</td>
							</tr>
							<tr>
								<td valign="top" colspan="2">
								 <div class="droppable" style="border: 5px solid #FFFFFF;">
									 <dx:ASPxScheduler runat="server" ID="Scheduler" ClientInstanceName="scheduler" GroupType="Resource"
										 AppointmentDataSourceID="AppointmentDataSource" ResourceDataSourceID="ResourceDataSourcex"
										 Start="2017-09-20" ActiveViewType="Timeline"
										 OnAppointmentRowInserted="Scheduler_AppointmentRowInserted"
										 OnAppointmentRowInserting="Scheduler_AppointmentRowInserting"
										 OnAppointmentsInserted="Scheduler_AppointmentsInserted"
										 OnBeforeExecuteCallbackCommand="ASPxScheduler1_BeforeExecuteCallbackCommand" ClientIDMode="AutoID"
										 Font-Names="Arial" Width="100%" OnPopupMenuShowing="Scheduler_PopupMenuShowing"
										 OnPrepareAppointmentFormPopupContainer="Scheduler_PrepareAppointmentFormPopupContainer"
										 OnInitClientAppointment="Scheduler_InitClientAppointment"
										 OnAppointmentChanging="Scheduler_AppointmentChanging"
										 OnAppointmentFormShowing="Scheduler_AppointmentFormShowing1"
										 OnCustomJSProperties="Scheduler_CustomJSProperties" OnCustomCallback="Scheduler_CustomCallback">
										 <Views>
											 <WorkWeekView ShowFullWeek="true" Enabled="False">
												 <TimeRulers>
													 <cc1:TimeRuler></cc1:TimeRuler>
												 </TimeRulers>
											 </WorkWeekView>
											 <DayView Enabled="False" ShowWorkTimeOnly="True">
												 <TimeRulers>
													 <cc1:TimeRuler></cc1:TimeRuler>
												 </TimeRulers>
											 </DayView>
											 <AgendaView Enabled="false"></AgendaView>
											 <WeekView Enabled="False">
											 </WeekView>
											 <MonthView Enabled="False">
											 </MonthView>
											 <TimelineView IntervalCount="14" ShowMoreButtons="False">
												 <WorkTime Start="07:30:00" End="17:00:00"></WorkTime>
												 <Scales>
													 <cc1:TimeScaleYear Enabled="False" />
													 <cc1:TimeScaleQuarter Enabled="False" />
													 <cc1:TimeScaleMonth Enabled="False" />
													 <cc1:TimeScaleWeek Enabled="False" Visible="False" />
													 <cc1:TimeScaleDay />
													 <cc1:TimeScaleHour DisplayFormat="H:mm" Width="10" Enabled="False" />
												 </Scales>
												 <WorkTime End="17:00:00" Start="07:30:00" />
												 <TimelineViewStyles TimelineDateHeader-HorizontalAlign="Center" TimelineDateHeader-VerticalAlign="Middle">
													 <TimelineCellBody VerticalAlign="Middle">
														 <Border BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" />
														 <BorderTop BorderStyle="None" />
														 <BorderBottom BorderStyle="None" />
														 <Border BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"></Border>
														 <BorderTop BorderStyle="None"></BorderTop>
														 <BorderBottom BorderStyle="None"></BorderBottom>
													 </TimelineCellBody>
													 <TimelineDateHeader Font-Size="8pt">
														 <Border BorderColor="Silver" BorderStyle="None" />
														 <BorderTop BorderStyle="None" />
														 <BorderBottom BorderStyle="None" />
														 <Border BorderColor="Silver" BorderStyle="None"></Border>
														 <BorderTop BorderStyle="None"></BorderTop>
														 <BorderBottom BorderStyle="None"></BorderBottom>
													 </TimelineDateHeader>
													 <VerticalResourceHeader BorderBottom-BorderStyle="None" Width="150px">
														 <Paddings Padding="0px" />
														 <BorderBottom BorderStyle="None"></BorderBottom>
													 </VerticalResourceHeader>
													 <GroupSeparatorHorizontal>
														 <Border BorderStyle="None" />
													 </GroupSeparatorHorizontal>
													 <GroupSeparatorVertical>
														 <Border BorderStyle="None" />
													 </GroupSeparatorVertical>
												 </TimelineViewStyles>
												 <Templates>
													 <TimelineDateHeaderTemplate>
														 <table width="100%" align="center">
															 <tr>
																 <td align="center">
																	 <dx:ASPxButton ID="btn_date_header" runat="server" AutoPostBack="False"
																		 Text='<%# Container.Interval.Start.DayOfWeek.ToString().Remove(3) + " " + Container.Interval.Start.Day %>'
																		 UseSubmitBehavior="False"
																		 ClientSideEvents-Click='<%# GetClickHandler(Container.Interval.Start) %>'
																		 Width="50px" Border-BorderStyle="None" ForeColor="Gray" ClientEnabled="False">
																	 </dx:ASPxButton>
																 </td>
															 </tr>
															 <tr>
																 <td>
																	 <asp:Label ID="Label1" runat="server" Text='<%# CalcDateTimeTotals(Container) %>' Font-Size="XX-Small" /></td>
															 </tr>
														 </table>
													 </TimelineDateHeaderTemplate>
													 <HorizontalAppointmentTemplate>
														 <uc1:inlinewoplanner ID="inlinewoplanner1" runat="server" />
													 </HorizontalAppointmentTemplate>
												 </Templates>
												 <AppointmentDisplayOptions
													 StartTimeVisibility="Always" SnapToCellsMode="Auto" AppointmentHeight="45" />
												 <TimelineViewStyles>
													 <TimelineCellBody Height="25px" VerticalAlign="Middle" />
												 </TimelineViewStyles>
												 <CellAutoHeightOptions MinHeight="40" />
												 <AppointmentDisplayOptions StartTimeVisibility="Always" AppointmentAutoHeight="True" SnapToCellsMode="Auto"></AppointmentDisplayOptions>
												 <CellAutoHeightOptions Mode="FitToContent"></CellAutoHeightOptions>
											 </TimelineView>

<FullWeekView><TimeRulers>
<cc1:TimeRuler></cc1:TimeRuler>
</TimeRulers>
</FullWeekView>
										 </Views>
										 <Storage EnableReminders="False">
											 <Appointments AutoRetrieveId="True">
												 <CustomFieldMappings>
													 <dx:ASPxAppointmentCustomFieldMapping Member="business_unit_id" Name="business_unit_id" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="quote_id" Name="quote_id" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="setby" Name="setby" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="assetid" Name="assetid" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="truckno" Name="truckno" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="member_fullname" Name="member_fullname" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="member_id" Name="member_id" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="membertype_id" Name="membertype_id" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="notes" Name="notes" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="confirmed" Name="confirmed" ValueType="Boolean" />
												 </CustomFieldMappings>
												 <Mappings AppointmentId="ID" End="EndDate" ResourceId="woprog_id"
													 Start="StartDate" Subject="Subject" Location="Location" Description="Description" Status="Status"></Mappings>
											 </Appointments>
											 <Resources>
												 <Mappings ResourceId="woprog_id" Caption="wo_desc"></Mappings>
											 </Resources>
										 </Storage>
										 <OptionsCustomization AllowAppointmentResize="None" />
										 <OptionsBehavior RecurrentAppointmentEditAction="Ask"
											 ShowRemindersForm="False" />
										 <OptionsView NavigationButtons-Visibility="Never" />
										 <ResourceNavigator Visibility="Never" />
										 <ClientSideEvents Init="function(s, e) {
	scheduler.menuManager.UpdateAptSubMenu = function (menu, submenuName, subMenuItemIndex){
               var apt = scheduler.GetAppointmentById(scheduler.GetSelectedAppointmentIds()[0]); 
               var statusid = apt.GetStatusId();
			   s.cpWarning = '';
if (statusid == 99) 
{
menu.rootItem.items[0].SetVisible(false); 
menu.rootItem.items[1].SetVisible(false); 
menu.rootItem.items[2].SetVisible(false); 
}
else if (statusid == 200) 
{
menu.rootItem.items[0].SetVisible(false); 
menu.rootItem.items[1].SetVisible(false); 
menu.rootItem.items[2].SetVisible(false); 
}
else if (statusid == 98) 
{
menu.rootItem.items[0].SetVisible(false); 
menu.rootItem.items[1].SetVisible(true); 
menu.rootItem.items[2].SetVisible(false); 
}
else 
{
menu.rootItem.items[0].SetVisible(true); 
menu.rootItem.items[1].SetVisible(true); 
menu.rootItem.items[2].SetVisible(true);
}          
}
}"
											 EndCallback="function(s, e) {
                    if (s.cpWarning!='') {
                        alert(s.cpWarning);
                        s.cpWarning='';
                    }
}" />
										 <Styles>
											 <PopupForm>
												 <Content BackColor="#E7F3EF">
												 </Content>
												 <Header BackColor="#00B239" Font-Names="Arial" Font-Size="16pt"
													 ForeColor="#EAEAEA" VerticalAlign="Middle">
													 <Border BorderStyle="None" />
													 <Border BorderStyle="None"></Border>
												 </Header>
												 <ModalBackground Opacity="0">
												 </ModalBackground>
											 </PopupForm>
											 <DayHeader Font-Size="8pt">
											 </DayHeader>
										 </Styles>
										 <Templates>
											 <VerticalResourceHeaderTemplate>
												 <div style="padding: 0px 0px 0px 0px; width: 150px; height: 45px; font-family: Arial, Helvetica, sans-serif; font-size: 9pt; vertical-align: middle;">
													 <table style="font-family: Arial; font-size: xx-small; text-align: left;">
														 <tr>
															 <td><%# Container.Resource.Caption %></td>
														 </tr>
													 </table>
												 </div>
											 </VerticalResourceHeaderTemplate>
										 </Templates>
										 <OptionsCustomization AllowAppointmentResize="None"></OptionsCustomization>
										 <OptionsBehavior RecurrentAppointmentEditAction="Ask" ShowRemindersForm="False"></OptionsBehavior>
										 <OptionsToolTips AppointmentToolTipUrl="woplanner_Tooltip.ascx"></OptionsToolTips>
										 <OptionsView NavigationButtons-Visibility="Never"></OptionsView>
										 <ResourceNavigator Visibility="Never"></ResourceNavigator>
									 </dx:ASPxScheduler>
								 </div>
                    			</td>
								<td valign="top">
                    				</td>
							</tr>
							<tr>
								<td>
								
                				</td>
								<td>
                    				&nbsp;</td>
								<td>
									&nbsp;</td>
							</tr>
						</table>
                 
                </td>
            </tr>
        </table>
		<div id="draggable3" class="ui-widget-content shadow" 
										
			style="top: 100px; right: 50px; position: fixed; width: 400px; height: 750px; "><div align="center" 
										
										
				
				style="font-weight: 700; padding-bottom: 0.2em; padding-top:0.2em; background-color:#A5A2A5; color: White; cursor: move; font-family: Arial; ">
				<table style="width:100%;">
					<tr>
						<td>
							&nbsp;</td>
						<td align="center" width="100%">
							Drag Me</td>
						<td width="0%" align="right">
							<dx:ASPxButton ID="btngrid_close" runat="server" AutoPostBack="False" 
								BackColor="Transparent" ClientInstanceName="btngrid_close" Text="-" 
								Width="20px" Font-Bold="True" ForeColor="Gray">
								<ClientSideEvents Click="function(s, e) {
if (btngrid_close.GetText()=='-')
{
	document.getElementById('draggable3').style.height= &quot;50px&quot;;
	
	grid.SetVisible(false);
	btngrid_close.SetText('+');
}
else if (btngrid_close.GetText()=='+')
{
document.getElementById('draggable3').style.height= &quot;750px&quot;;
	grid.SetVisible(true);
	btngrid_close.SetText('-');
}
}" />
								<Border BorderStyle="None" />
							</dx:ASPxButton>
						</td>
					</tr>
					</table></div>
                    <dx:ASPxGridView ID="gvemps" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                        Width="400px" KeyFieldName="member_id" Font-Names="Arial" Font-Size="8pt" 
										Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="750" EnableTheming="True" 
										Theme="NETheme01" onhtmlrowprepared="gvemps_HtmlRowPrepared" Visible="true">
                        <Columns>
                            <dx:GridViewCommandColumn Visible="False" VisibleIndex="0">
								
							</dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="member_id" ReadOnly="True" 
								Visible="False" VisibleIndex="2" Width="35px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="membertype_name" VisibleIndex="3" 
								Caption="Title" Width="120px">
                            	<Settings AutoFilterCondition="BeginsWith" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Name" FieldName="member_fullname" 
								VisibleIndex="4" Width="70px">
								<Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
								<CellStyle Wrap="False">
								</CellStyle>
							</dx:GridViewDataTextColumn>
                            <dx:GridViewDataHyperLinkColumn Caption=" " ReadOnly="True" VisibleIndex="1" 
								Width="25px">
                                <Settings AllowSort="False"></Settings>
                                <EditFormSettings Visible="False" />
                                <DataItemTemplate>
                                    <div class="draggable">
                                        <a href="#" title="Image Viewer" style="cursor: move">
                                            <img src="../../../images/icon/icon[drag].png" alt="" />
                                        </a>
                                        <input type="hidden" value='<%# Container.VisibleIndex %>' />
                                    </div>
                                </DataItemTemplate>
                            </dx:GridViewDataHyperLinkColumn>
                            <dx:GridViewDataTextColumn FieldName="TypeID" Visible="False">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    	<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" />
						<SettingsPager PageSize="100" Visible="False">
						</SettingsPager>
						<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" 
							VerticalScrollableHeight="700" />
                    	<SettingsPopup>
							<HeaderFilter MinHeight="700px" Width="300px" />
						</SettingsPopup>
                    </dx:ASPxGridView></div>
         <asp:SqlDataSource ID="ResourceDataSourcex" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT CAST(woprog_id AS UNSIGNED) woprog_id,Concat(trim(leading '0' from woprog_bvwo),'-',woprog_customername,' ',woprog_description) wo_desc  FROM woprog
              where woprog_status = 'Open' and business_unit_id = ?cid and ((woprog_pm_memberid IN (SELECT member_id FROM member WHERE reports_to = ?pid))or ?pid=0) and ((woprog_customer_id = ?cuid)or ?cuid=0)  order by woprog_bvwo " 
			>
			 <SelectParameters>
				 <asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
				 <asp:ControlParameter ControlID="ddlpm" Name="pid" PropertyName="Value" />
				  <asp:ControlParameter ControlID="ddlcustomer" Name="cuid" PropertyName="Value" />
			 </SelectParameters>
        </asp:SqlDataSource>
        <dx:ASPxHiddenField ID="hf" runat="server" ClientInstanceName="hf" SyncWithServer="true">
        </dx:ASPxHiddenField>
        
        <asp:HiddenField ID="hdn_cid" runat="server" />
        
        
            
        <dx:ASPxGlobalEvents ID="ASPxGlobalEvents1" runat="server">
            <ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" />
        </dx:ASPxGlobalEvents>
       
        <asp:SqlDataSource ID="AppointmentDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
			InsertCommand="INSERT INTO [Appointments] ([EventType], [StartDate], [EndDate], [AllDay], [Subject], [Location], [Description], [Status], [Label], [ResourceID], [RecurrenceInfo], [ReminderInfo], [ContactInfo],[woprog_id],[quote_id],[business_unit_id],[setby],[assetid],[member_id],[membertype_id],[confirmed],[notes]) VALUES (@EventType, @StartDate, @EndDate, @AllDay, @Subject, @Location, @Description, @Status, @Label, @member_id, @RecurrenceInfo, @ReminderInfo, @ContactInfo,@woprog_id,@quote_id,@business_unit_id,@setby,@assetid,@member_id,@membertype_id,@confirmed,@notes)"
			UpdateCommand="UPDATE [Appointments] SET [EventType] = @EventType, [StartDate] = @StartDate, [EndDate] = @EndDate, [AllDay] = @AllDay, [Subject] = @Subject, [Location] = @Location, [Description] = @Description, [Status] = @Status, [Label] = @Label, [ResourceID] = @member_id, [RecurrenceInfo] = @RecurrenceInfo, [ReminderInfo] = @ReminderInfo, [ContactInfo] = @ContactInfo, [woprog_id]=@woprog_id, [setby]=@setby, [assetid]=@assetid, [member_id]=@member_id,[membertype_id]=@membertype_id,[confirmed]=@confirmed,[notes]=@notes WHERE [ID] = @ID"
			DeleteCommand="Delete from appointments where id = @id"
			SelectCommand="
SELECT 
  * 
FROM
  (SELECT 
    a.AllDay,
    CONCAT('Truck ', b.assets_No) AS truckno,
    a.ID,
    a.EventType,
    a.StartDate,
    a.EndDate,
    a.Subject,
    a.Location,
    a.Description,
    a.Status,
    a.Label,
    a.ResourceId,
    a.RecurrenceInfo,
    a.ReminderInfo,
    a.ContactInfo,
    a.business_unit_id,
   CAST(a.woprog_id AS UNSIGNED) woprog_id,
    a.quote_id,
    a.setby,
    a.assetid,
    IF(
      (
        (a.membertype_id > 0) && (a.member_id = 0)
      ),
      membertype_name,
      member_fullname
    ) member_fullname,
    a.member_id,
    a.membertype_id,
    a.confirmed,
    a.notes 
  FROM
    appointments a 
    LEFT JOIN assets b 
      ON a.assetid = b.assets_ID 
      AND b.assets_type = 1 
    LEFT JOIN member c 
      ON a.member_id = c.member_id 
    LEFT JOIN membertype d 
      ON a.membertype_id = d.membertype_id 
  WHERE a.business_unit_id = @cid
    AND a.woprog_id != 0 
  UNION
  SELECT 
    0 AllDay,
    '' truckno,
    id + 200000000 ID,
    '' EventType,
    dt_start StartDate,
    dt_end EndDate,
    'WO Freeze' `Subject`,
    '' Location,
    notes Description,
    70 `Status`,
    '' Label,
    0 ResourceId,
    '' RecurrenceInfo,
    '' ReminderInfo,
    '' ContactInfo,
    0 business_unit_id,
    CAST(woprog_id AS UNSIGNED) woprog_id,
    0 quote_id,
    setby setby,
    0 assetid,
    '' member_fullname,
    0 member_id,
    0 membertype_id,
    '1' confirmed,
    notes 
  FROM
    wo_freeze 
  UNION
  SELECT 
    0 AllDay,
    '' truckno,
    a.Holidays_ID + 300000000 ID,
    NULL EventType,
    DATE_ADD(
      a.Holidays_Date,
      INTERVAL 450 MINUTE
    ) StartDate,
    DATE_ADD(
      a.Holidays_Date,
      INTERVAL 960 MINUTE
    ) EndDate,
    a.Holidays_Name `Subject`,
    '' Location,
    a.Holidays_Name Description,
    200 `Status`,
    '' Label,
    b.WOProg_ID ResourceID,
    '' RecurrenceInfo,
    '' ReminderInfo,
    '' ContactInfo,
    ?cid business_unit_id,
    CAST(b.woprog_id AS UNSIGNED) woprog_id,
    0 quote_id,
    '' setby,
    0 assetid,
    '' member_fullname,
    0 member_id,
    0 membertype_id,
    0 confirmed,
    '' notes 
  FROM
    holidays a,
    woprog b 
    INNER join business_unit c 
      ON b.business_unit_id = c.id 
  WHERE b.business_unit_id = @cid 
    AND a.Holidays_Date > (CURDATE() - INTERVAL 1 MONTH) 
    AND b.WOProg_Status = 'Open' 
    AND IF(
      (c.country = 'USA'),
      (a.holidays_america = 1),
      (a.holidays_canada = 1)
    )) asdf 
ORDER BY member_id,
  startdate,
  enddate,
  woprog_id 
"
            OnInserted="SchedulingDataSource_Inserted">
			 <DeleteParameters>
                <asp:Parameter Name="ID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="EventType" Type="Int32" />
                <asp:Parameter Name="StartDate" Type="DateTime" />
                <asp:Parameter Name="EndDate" Type="DateTime" />
                <asp:Parameter Name="AllDay" Type="Boolean" />
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="Location" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Status" Type="Int32" />
                <asp:Parameter Name="Label" Type="Int32" />
                <asp:Parameter Name="ResourceID" Type="Int32" />
                <asp:Parameter Name="RecurrenceInfo" Type="String" />
                <asp:Parameter Name="ReminderInfo" Type="String" />
                <asp:Parameter Name="ContactInfo" Type="String" />
				<asp:Parameter Name="woprog_id" Type="String" />
                <asp:ControlParameter ControlID="ddlbranch" Name="business_unit_id" PropertyName="Value" />
				<asp:Parameter Name="quote_id" Type="String" />
				<asp:Parameter Name="setby" Type="String" />
				<asp:Parameter Name="assetid" Type="String" />
				<asp:Parameter Name="member_id" Type="String" />
				<asp:Parameter Name="membertype_id" Type="String" />
				<asp:Parameter Name="confirmed" Type="Boolean" />
					<asp:Parameter Name="notes" Type="String" />
            </InsertParameters>
			
             <SelectParameters>
				 <asp:ControlParameter ControlID="ddlbranch" Name="@cid" PropertyName="Value" />
			 </SelectParameters>
			
            <UpdateParameters>
                <asp:Parameter Name="EventType" Type="Int32" />
                <asp:Parameter Name="StartDate" Type="DateTime" />
                <asp:Parameter Name="EndDate" Type="DateTime" />
                <asp:Parameter Name="AllDay" Type="Boolean" />
                <asp:Parameter Name="Subject" Type="String" />
                <asp:Parameter Name="Location" Type="String" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Status" Type="Int32" />
                <asp:Parameter Name="Label" Type="Int32" />
                <asp:Parameter Name="ResourceID" Type="Int32" />
                <asp:Parameter Name="RecurrenceInfo" Type="String" />
                <asp:Parameter Name="ReminderInfo" Type="String" />
                <asp:Parameter Name="ContactInfo" Type="String" />
				<asp:Parameter Name="woprog_id" Type="String" />
				<asp:Parameter Name="business_unit_id" Type="String" />
				<asp:Parameter Name="quote_id" Type="String" />
				<asp:Parameter Name="setby" Type="String" />
                <asp:Parameter Name="ID" Type="Int32" />
				<asp:Parameter Name="assetid" Type="String" />
				<asp:Parameter Name="member_id" Type="String" />
								<asp:Parameter Name="membertype_id" Type="String" />
				<asp:Parameter Name="confirmed" Type="Boolean" />
					<asp:Parameter Name="notes" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>
        
        <br />
        
       <dx:ASPxPopupControl ID="pop_edit" runat="server" ClientInstanceName="pop_edit" 
			PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
			ShowPageScrollbarWhenModal="True"  
			PopupAnimationType="None" Height="400px" 
			onwindowcallback="pop_edit_WindowCallback1"  
			Width="600px" Theme="NETheme01" AllowDragging="True" CloseAction="CloseButton" 
			HeaderText="Edit Schedule" Modal="True">
			<ContentStyle HorizontalAlign="Center">
			</ContentStyle>
			<ModalBackgroundStyle Opacity="0">
			</ModalBackgroundStyle>
			<ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
				<table width="100%">
					<tr>
						<td align="center" class="style4" style="text-align: left">
							Customer</td>
						<td align="center" colspan="2" style="text-align: left">
							<dx:ASPxHyperLink ID="hl_customer" runat="server" 
								ClientInstanceName="hl_customer" Cursor="pointer" Theme="NETheme01">
							</dx:ASPxHyperLink>
						</td>
					</tr>
					<tr>
						<td class="style4" nowrap="nowrap" style="height: 15px">
							Work Order</td>
						<td align="left" style="height: 15px">
							<dx:ASPxHyperLink ID="hl_wo" runat="server" ClientInstanceName="hl_wo" 
								Cursor="pointer" Theme="NETheme01" Wrap="False">
							</dx:ASPxHyperLink>
						</td>
						<td align="left" style="height: 15px" width="100%">
							<table style="width:100%;">
								<tr>
									<td>
										<dx:ASPxButton ID="btnsuspend" runat="server" AutoPostBack="False" 
											Text="Wait for Parts" Theme="NETheme01">
											<ClientSideEvents Click="function(s, e) {
	
	
}" />
										</dx:ASPxButton>
									</td>
									<td>
										<dx:ASPxButton ID="btnsuspend0" runat="server" AutoPostBack="False" 
											Text="Wait for Customer Schedule" Theme="NETheme01">
											<ClientSideEvents Click="function(s, e) {
	
	
}" />
										</dx:ASPxButton>
									</td>
									<td>
										&nbsp;</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							Contact</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxLabel runat="server" ClientInstanceName="lblcontact" Theme="NETheme01" 
								ID="lblcontact"></dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							Resource</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxLabel ID="lblresource" runat="server" ClientInstanceName="lblresource" 
								Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							Location</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxLabel ID="lbllocation" runat="server" ClientInstanceName="lbllocation" 
								Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px" nowrap="nowrap">
							Phone Number</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxLabel ID="lblphone" runat="server" ClientInstanceName="lblphone" 
								Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							Truck</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxComboBox ID="ddl_truck" runat="server" ClientInstanceName="ddl_truck" 
								DataSourceID="sql_trucks" TextField="_name" Theme="NETheme01" 
								ValueField="assets_id" ValueType="System.Int32">
							</dx:ASPxComboBox>
							<asp:SqlDataSource ID="sql_trucks" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								SelectCommand="Select assets_id,concat('Truck ',assets_no) _name from assets where assets_type = 1 and business_unit_id = ?cid">
								<SelectParameters>
									<asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
								</SelectParameters>
							</asp:SqlDataSource>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							Date</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxLabel ID="lbldate" runat="server" ClientInstanceName="lbldate" 
								Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							Start Time</td>
						<td align="left" colspan="2" style="height: 15px">
							<dx:ASPxTimeEdit ID="te_start" runat="server" ClientInstanceName="te_start" 
								Theme="NETheme01">
							</dx:ASPxTimeEdit>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							End Time</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxTimeEdit ID="te_end" runat="server" ClientInstanceName="te_end" 
								Theme="NETheme01">
							</dx:ASPxTimeEdit>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							Scheduled By</td>
						<td align="left" colspan="2" style="height: 15px">
							<dx:ASPxLabel ID="lblsetby" runat="server" ClientInstanceName="lblsetby" 
								Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							Notes:</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxMemo ID="mem_notes" runat="server" ClientInstanceName="mem_notes" 
								Height="71px" Theme="NETheme01" Width="300px">
							</dx:ASPxMemo>
						</td>
					</tr>
					<tr>
						<td class="style4" colspan="3" style="height: 15px">
							<table style="width:100%;">
								<tr>
									<td>
										&nbsp;</td>
									<td>
										&nbsp;</td>
									<td>
										&nbsp;</td>
								</tr>
								<tr>
									<td align="center">
										<dx:ASPxButton ID="btnok" runat="server" AutoPostBack="False" 
											HorizontalAlign="Center" Text="Ok" Theme="NETheme01" Width="100px">
											<ClientSideEvents Click="function(s, e) {
pop_edit.PerformCallback('Save');
	scheduler.Refresh();
	pop_edit.Hide();
}" />
										</dx:ASPxButton>
									</td>
									<td align="center">
										<dx:ASPxButton ID="btndelete" runat="server" AutoPostBack="False" Text="Delete" 
											Theme="NETheme01" Width="100px">
											<ClientSideEvents Click="function(s, e) {
if (confirm('Are you sure you want to delete this schedule entry?'))
{
	pop_edit.PerformCallback('Delete');
	scheduler.Refresh();
	pop_edit.Hide();
}
}" />
										</dx:ASPxButton>
									</td>
									<td align="center">
										<dx:ASPxButton ID="btncancel" runat="server" AutoPostBack="False" Text="Cancel" 
											Theme="NETheme01" Width="100px">
											<ClientSideEvents Click="function(s, e) {
	
	pop_edit.Hide();
}" />
										</dx:ASPxButton>
									</td>
								</tr>
								<tr>
									<td>
										&nbsp;</td>
									<td>
										&nbsp;</td>
									<td>
										&nbsp;</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				</dx:PopupControlContentControl>
</ContentCollection>
		</dx:ASPxPopupControl>
    	
        
       
    </div>
    <dx:ASPxPopupControl ID="pop_freeze" runat="server" 
	ClientInstanceName="pop_freeze" CloseAction="CloseButton" FooterText="" 
	HeaderText="Freeze Work Order" Modal="True" PopupHorizontalAlign="WindowCenter" 
	PopupVerticalAlign="WindowCenter" Theme="NETheme01" 
	onwindowcallback="pop_freeze_WindowCallback">
		<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_close==&quot;1&quot;)
{
pop_freeze.Hide();
scheduler.Refresh();
}
}" PopUp="function(s, e) {
	pop_freeze.PerformCallback();
}" />
		<ModalBackgroundStyle Opacity="0">
		</ModalBackgroundStyle>
		<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<table style="width:100%;">
		<tr>
			<td class="style8" nowrap="nowrap">
				Start Time:</td>
			<td>
				<dx:ASPxTimeEdit ID="te_start2" runat="server" Theme="NETheme01">
				</dx:ASPxTimeEdit>
			</td>
		</tr>
		<tr>
			<td class="style8">
				End Time:</td>
			<td>
				<dx:ASPxTimeEdit ID="te_end2" runat="server" Theme="NETheme01">
				</dx:ASPxTimeEdit>
			</td>
		</tr>
		<tr>
			<td class="style8">
				&nbsp;</td>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td class="style7">
				Notes:</td>
			<td class="style6">
				<dx:ASPxMemo ID="mem_freeze_notes" runat="server" Height="71px" Theme="NETheme01" 
					Width="170px">
				</dx:ASPxMemo>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
			<td align="right" style="text-align: right">
				<dx:ASPxButton ID="btnsaveevent" runat="server" AutoPostBack="False" 
					EnableTheming="True" Text="Save" Theme="NETheme01">
					<ClientSideEvents Click="function(s, e) {
	pop_freeze.PerformCallback('save');
	
}" />
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
			</dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>
    <p>
    <dx:ASPxPopupControl ID="pop_recurrence" runat="server" 
	ClientInstanceName="pop_recurrence" HeaderText="Add Recurrence" Modal="True" 
	PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" 
	Theme="NETheme01" AllowDragging="True" Height="300px" Width="600px" 
	onwindowcallback="pop_recurrence_WindowCallback">
		<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_bind=='1')
	{
scheduler.Refresh();
s.Hide();
}
}" />
		<ModalBackgroundStyle Opacity="0">
		</ModalBackgroundStyle>
		<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<table style="width:100%;">
		<tr>
			<td>
				<dx:ASPxLabel ID="lbl_customer_rec" runat="server" 
					ClientInstanceName="lbl_customer_rec" Text=" " Theme="NETheme01">
				</dx:ASPxLabel>
			</td>
			<td colspan="2">
				&nbsp;</td>
			<td colspan="2">
				&nbsp;</td>
			<td class="style1">
				<dx:ASPxLabel ID="lbl_original_date" runat="server" 
					ClientInstanceName="lbl_original_date" Text=" " Theme="NETheme01" Wrap="False">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td colspan="5" valign="top" width="100%">
				<dx:ASPxLabel ID="lbl_member_rec" runat="server" 
					ClientInstanceName="lbl_member_rec" Text=" " Theme="NETheme01">
				</dx:ASPxLabel>
			</td>
			<td class="style1" valign="top">
				<dx:ASPxLabel ID="lbl_start_and_end" runat="server" 
					ClientInstanceName="lbl_start_and_end" Text=" " Theme="NETheme01" Wrap="False">
				</dx:ASPxLabel></td>
		</tr>
		<tr>
			<td colspan="5" valign="top" width="100%">
				<dx:ASPxLabel ID="lbl_description_rec" runat="server" 
					ClientInstanceName="lbl_description_rec" Text=" " Theme="NETheme01">
				</dx:ASPxLabel>
			</td>
			<td class="style1" valign="top">
				
			<dx:ASPxCheckBox ID="chkallow_weekends" runat="server" ClientInstanceName="chkallow_weekends" Text="Allow Weekends">
			</dx:ASPxCheckBox>
			</td>
		</tr>
		<tr>
			<td colspan="6">
				<dx:ASPxCalendar ID="ASPxCalendar1" runat="server" Columns="2" 
					EnableMultiSelect="True" EnableTheming="True" EnableYearNavigation="False" 
					Font-Names="Arial" Rows="2" ShowClearButton="False" ShowShadow="False" 
					ShowTodayButton="False" Theme="NETheme01" Width="100%" 
					OnDayCellPrepared="ASPxCalendar1_DayCellPrepared">
					<MonthGridPaddings Padding="10px" />
					<ClientSideEvents SelectionChanged="function(s, e) {
	recurrenceDateChanged(s, e);
}" />
				</dx:ASPxCalendar>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<dx:ASPxLabel ID="lblaptid" runat="server" ClientInstanceName="lblaptid" 
					ForeColor="Transparent" Text=" ">
				</dx:ASPxLabel>
				<table style="width: 100px;">
					<tr>
						<td class="style2" style="background-color: white">
							Legend</td>
					</tr>
					<tr>
						<td class="style2" style="background-color: LimeGreen">
							Frozen</td>
					</tr>
				</table>
			</td>
			<td colspan="2" style="text-align: right">
				&nbsp;</td>
			<td colspan="2" align="right" width="100%">
				<dx:ASPxButton ID="btnsaverecur" runat="server" AutoPostBack="False" 
					EnableTheming="True" Text="Save" Theme="NETheme01">
					<ClientSideEvents Click="function(s, e) {
	pop_recurrence.PerformCallback('save');
	
}" />
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
	<br />
			</dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>
    </p>
    