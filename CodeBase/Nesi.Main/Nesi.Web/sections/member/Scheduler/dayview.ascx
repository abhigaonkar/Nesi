<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_scheduler_dayview" Codebehind="dayview.ascx.cs" %>
<%@ Register tagPrefix="dx" namespace="DevExpress.Web" assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register tagPrefix="dx" namespace="DevExpress.Web.ASPxScheduler" assembly="DevExpress.Web.ASPxScheduler.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register assembly="DevExpress.XtraScheduler.v19.2.Core, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>
<%@ Register src="inlineappform.ascx" tagname="inlineappform" tagprefix="uc1" %>
<%@ Register src="hourview.ascx" tagname="hourview" tagprefix="uc2" %>
<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<link type="text/css" rel="stylesheet" href="/css/scheduler.css" />
<script type="text/javascript" language="javascript" src="/js/scheduler.js?<%= nesi.core.Toolbox.do_RandomString(10) %>"></script>
    <div>
        <table style="font-family: Arial, Helvetica, sans-serif; border-collapse: collapse;" 
			width="100%">
            <tr>
                <td style="vertical-align: top" align="left" width="0%">
                    <table style="width:100%;">
						<tr>
							<td>
                    <dx:ASPxComboBox ID="ddlbranch" runat="server" AutoPostBack="True" 
						DataSourceID="sqlbranch" Font-Names="Arial" TextField="name" 
						ValueField="business_unit_id" ValueType="System.Int32" 
						onselectedindexchanged="ddlbranch_SelectedIndexChanged" style="margin-bottom: 0px" ClientInstanceName="ddlbranch">
					</dx:ASPxComboBox>
					<asp:SqlDataSource ID="sqlbranch" runat="server" 
						ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
						ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"  
						>
					</asp:SqlDataSource>
                			</td>
							<td>
                                &nbsp;</td>
							<td>
							    <asp:SqlDataSource ID="sqlpm" runat="server" 
							                       ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							                       ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							                       SelectCommand="SELECT
          0 _id,
          'Select PM' _name
UNION
          SELECT DISTINCT
                    a.member_id _id,
                    a.member_fullname _name
          FROM
                    member a
          INNER JOIN membertype ON membertype_id = a.member_membertype_id
					INNER JOIN member b on b.reports_to = a.member_id
          WHERE
                    membertype.is_team_leader = 1
          AND a.business_unit_id = ?cid
          AND a.Member_Status = 'Active'">
							        <SelectParameters>
							            <asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
							        </SelectParameters>
							    </asp:SqlDataSource>
							    <dx:ASPxComboBox ID="ddlpm" runat="server" ClientInstanceName="ddlpm" 
							                     DataSourceID="sqlpm" TextField="_name" 
							                     ValueField="_id" ValueType="System.Int32" AutoPostBack="True" Font-Names="Arial" 
							                     onselectedindexchanged="ddlpm_SelectedIndexChanged">
							    </dx:ASPxComboBox>
							</td>
							<td width="100%">
								&nbsp;</td>
						</tr>
						<tr>
							<td>
							    <dx:ASPxComboBox ID="ddlfilter" runat="server" ClientInstanceName="ddlfilter" TextField="_name" 
							                     ValueField="_id" ValueType="System.Int32" AutoPostBack="True" Font-Names="Arial" 
							                     onselectedindexchanged="ddlfilter_SelectedIndexChanged">
							        <Items>
							            <dx:ListEditItem Text="All Resources" Value="0" />
							            <dx:ListEditItem Text="Employees Only" Value="1" />
							        </Items>
							    </dx:ASPxComboBox>	
                			</td>
							<td>
                			    &nbsp;</td>
							<td nowrap="nowrap">
								&nbsp;</td>
							<td>
								&nbsp;</td>
						</tr>
						<tr>
							<td>
                			</td>
							<td>
									&nbsp;</td>
							<td nowrap="nowrap">
								&nbsp;</td>
							<td>
								&nbsp;</td>
						</tr>
						<tr>
							<td>
							    <dx:ASPxCallbackPanel ID="cb_mt" runat="server" ClientInstanceName="cb_mt" 
							                          oncallback="cb_mt_Callback" Width="200px" >
							        <SettingsLoadingPanel Text="" />
							        <PanelCollection>
							            <dx:PanelContent runat="server">
							                <dx:ASPxGridLookup ID="ASPxGridLookup1" runat="server" AllowUserInput="False" 
							                                   AnimationType="None" AutoGenerateColumns="False" DataSourceID="SqlDataSource2" 
							                                   KeyFieldName="id" MultiTextSeparator="," NullText="Show Membertype PlaceHolders" 
							                                   OnDataBound="ASPxGridLookup1_DataBound" SelectionMode="Multiple" 
							                                   TextFormatString="{1}"  Width="100%" Caption="Show These Placeholders">
							                    <GridViewProperties>
							                        <SettingsPager Mode="ShowAllRecords">
							                        </SettingsPager>
							                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True"></SettingsBehavior>
							                        <SettingsLoadingPanel Mode="Disabled" />
							                    </GridViewProperties>
							                    <Columns>
							                        <dx:GridViewDataTextColumn FieldName="id" ShowInCustomizationForm="True" 
							                                                   Visible="False" VisibleIndex="1">
							                        </dx:GridViewDataTextColumn>
							                        <dx:GridViewDataTextColumn Caption="Membertype" FieldName="name" 
							                                                   ShowInCustomizationForm="True" VisibleIndex="2" Width="300px" CellStyle-Wrap="False">
							                            <CellStyle Wrap="False"></CellStyle>
							                        </dx:GridViewDataTextColumn>
							                        <dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
							                                                  ShowSelectCheckbox="True" VisibleIndex="0">
							                        </dx:GridViewCommandColumn>
							                    </Columns>
							                    <ClientSideEvents TextChanged="function(s, e) {
		cb_mt.PerformCallback();
scheduler.Refresh();
}" />
							                </dx:ASPxGridLookup>
							                <asp:HiddenField ID="hdnmid" runat="server" />
							                <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
							                                   ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							                                   ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							                                   SelectCommand="Select membertype_id id, membertype_name name , if(a.id is null,0,1) _selected from membertype left join appointment_resource_member_link a on membertype.membertype_id = a.resource_id and a.member_id = ?mid where membertype.active =1 and membertype.is_scheduled=1  order by  membertype_name">
							                    <SelectParameters>
							                        <asp:ControlParameter ControlID="hdnmid" Name="mid" PropertyName="Value" />
							                    </SelectParameters>
							                </asp:SqlDataSource>
							            </dx:PanelContent>
							        </PanelCollection>
							    </dx:ASPxCallbackPanel>
                			</td>
							<td>
<asp:HiddenField runat="server" ID="hdnmid0"></asp:HiddenField>
                                </td>
							<td>
							    <dx:ASPxCallbackPanel ID="cb_mt_filter" runat="server" ClientInstanceName="cb_mt_filter" 
							                          oncallback="cb_mt_filter_Callback" Width="200px" >
							        <SettingsLoadingPanel Text="" />
							        <PanelCollection>
							            <dx:PanelContent runat="server">
							                <dx:ASPxGridLookup ID="gv_mt_filter" runat="server" AllowUserInput="False" 
							                                   AnimationType="None" AutoGenerateColumns="False" DataSourceID="SqlDataSource4" 
							                                   KeyFieldName="id" MultiTextSeparator="," NullText="Hide These Membertypes" 
							                                   OnDataBound="gv_mt_filter_DataBound" SelectionMode="Multiple" 
							                                   TextFormatString="{1}"  Width="100%" Caption="Hide These Membertypes">
							                    <GridViewProperties>
							                        <SettingsPager Mode="ShowAllRecords">
							                        </SettingsPager>
							                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True"></SettingsBehavior>
							                        <SettingsLoadingPanel Mode="Disabled" />
							                    </GridViewProperties>
							                    <Columns>
							                        <dx:GridViewDataTextColumn FieldName="id" ShowInCustomizationForm="True" 
							                                                   Visible="False" VisibleIndex="1">
							                        </dx:GridViewDataTextColumn>
							                        <dx:GridViewDataTextColumn Caption="Membertype" FieldName="name" 
							                                                   ShowInCustomizationForm="True" VisibleIndex="2" Width="300px" CellStyle-Wrap="False">
							                            <CellStyle Wrap="False"></CellStyle>
							                        </dx:GridViewDataTextColumn>
							                        <dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
							                                                  ShowSelectCheckbox="True" VisibleIndex="0">
							                        </dx:GridViewCommandColumn>
							                    </Columns>
							                    <ClientSideEvents TextChanged="function(s, e) {
		cb_mt_filter.PerformCallback();
scheduler.Refresh();
}" />
							                </dx:ASPxGridLookup>
							                <asp:HiddenField ID="hdnmid2" runat="server" />
							                <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
							                                   ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							                                   ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							                                   SelectCommand="Select membertype_id id, membertype_name name , if(a.id is null,0,1) _selected from membertype left join appointment_resource_member_hide_link a on membertype.membertype_id = a.resource_id and a.member_id = ?mid where membertype.active =1 and membertype.is_scheduled=1  order by  membertype_name">
							                    <SelectParameters>
							                        <asp:ControlParameter ControlID="hdnmid2" Name="mid" PropertyName="Value" />
							                    </SelectParameters>
							                </asp:SqlDataSource>
							            </dx:PanelContent>
							        </PanelCollection>
							    </dx:ASPxCallbackPanel>
							</td>
							<td>
							</td>
						</tr>
					</table>
                </td>
            </tr>
            <tr>
                <td>
                        <table style="width:100%; border-collapse: collapse;" cellpadding="0">
							<tr>
								<td valign="top">
								 <div style="border: 5px solid #FFFFFF;">
                        			 <div style="vertical-align: middle;  white-space: nowrap;" 
										 align="left">
										 <table>
										 <tr>
										 <td>
										 <asp:Image ID="Image1" runat="server" Height="25px" 
										 ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_gohome.png" />
										 </td>
										 <td>
										 - Meet on Site
										 </td>
										 <td>
										 	 &nbsp;</td>
										 <td>
										 	 &nbsp;</td>
										 <td style="border: thick solid #FF0000; width: 120px; text-align: center;">
										 	On Call</td>
                                              <td style="border: thick solid #ffd800; text-align: center; vertical-align: middle; width: 120px;">
										 	Backup On Call</td>
                                              <td style="padding-left: 70px; padding-right: 10px;">
                    <dx:ASPxCheckBox ID="chk_showpp1" runat="server" AutoPostBack="True" 
						Text="Show Full Pay Period" Wrap="False" Font-Names="Arial">
					</dx:ASPxCheckBox>
                			                 </td>
                                              <td>
								<dx:ASPxButton ID="btWOPlanner" runat="server" AutoPostBack="False" 
									Text="Work Order Planner">
									<ClientSideEvents Click="function(s, e) {
	boing('index3.aspx',800,800,'wopplanner');
}" />
								</dx:ASPxButton>
                			                 </td>
										 </tr>
										 </table></div>
									 <dx:ASPxScheduler runat="server" ID="Scheduler" ClientInstanceName="scheduler" GroupType="Resource"
										 AppointmentDataSourceID="AppointmentDataSource" ResourceDataSourceID="ResourceDataSourcex"
										 Start="2016-08-01" ActiveViewType="Timeline"
										 OnAppointmentRowInserted="Scheduler_AppointmentRowInserted"
										 OnAppointmentRowInserting="Scheduler_AppointmentRowInserting"
										 OnAppointmentsInserted="Scheduler_AppointmentsInserted"
										 OnBeforeExecuteCallbackCommand="ASPxScheduler1_BeforeExecuteCallbackCommand" ClientIDMode="AutoID"
										 Font-Names="Arial" Width="100%" OnPopupMenuShowing="Scheduler_PopupMenuShowing"
										 OnPrepareAppointmentFormPopupContainer="Scheduler_PrepareAppointmentFormPopupContainer"
										 OnInitClientAppointment="Scheduler_InitClientAppointment"
										 OnAppointmentChanging="Scheduler_AppointmentChanging"
										 OnAppointmentFormShowing="Scheduler_AppointmentFormShowing1"
										 OnCustomJSProperties="Scheduler_CustomJSProperties" OnAppointmentDeleting="Scheduler_AppointmentDeleting"
										 OnCustomCallback="Scheduler_CustomCallback" OnHtmlTimeCellPrepared="Scheduler_HtmlTimeCellPrepared" OnVisibleIntervalChanged="Scheduler_VisibleIntervalChanged" EnablePagingGestures="False" OptionsCookies-CookiesID="sched" OptionsCookies-Enabled="True" Storage-DateTimeSavingMode="Storage" Storage-EnableReminders="False" Storage-EnableSmartFetch="False" Storage-EnableTimeZones="False" Storage-TimeZoneId="Eastern Standard Time" EnableClientRender="False">
										 <OptionsCustomization AllowAppointmentResize="None" />
										 <OptionsBehavior RecurrentAppointmentEditAction="Ask" ShowRemindersForm="False" />
										 <Views>
											 <WorkWeekView ShowFullWeek="true" Enabled="False">
												 <TimeRulers>
													 <cc1:TimeRuler></cc1:TimeRuler>
												 </TimeRulers>
												 <AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4"></AppointmentDisplayOptions>
											 </WorkWeekView>
											 <DayView Enabled="False" ShowWorkTimeOnly="True">
												 <TimeRulers>
													 <cc1:TimeRuler></cc1:TimeRuler>
												 </TimeRulers>
												 <AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4"></AppointmentDisplayOptions>
											 </DayView>
											 <WeekView Enabled="False">
											 </WeekView>
											 <MonthView Enabled="False">
											 </MonthView>

<FullWeekView ViewSelectorItemAdaptivePriority="7"><TimeRulers>
<cc1:TimeRuler></cc1:TimeRuler>
</TimeRulers>

<AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4"></AppointmentDisplayOptions>
</FullWeekView>

											 <AgendaView Enabled="false"></AgendaView>

											 <TimelineView IntervalCount="14" ShowMoreButtons="False" ResourcesPerPage="150">
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
														 <border bordercolor="#CCCCCC" borderstyle="Solid" borderwidth="1px" />
														 <BorderTop BorderStyle="None" />
													 </TimelineCellBody>
													 <TimelineDateHeader Font-Size="8pt">
														 <border bordercolor="Silver" borderstyle="Solid" />
													 </TimelineDateHeader>
													 <VerticalResourceHeader>
														 <Paddings Padding="0px" />
														 <BorderBottom BorderStyle="Solid" />
													 </VerticalResourceHeader>
													 <GroupSeparatorHorizontal>
													 </GroupSeparatorHorizontal>
													 <GroupSeparatorVertical>
													 </GroupSeparatorVertical>
												 </TimelineViewStyles>
												 <Templates>
													 <TimelineDateHeaderTemplate>
														 <table width="100%">
															 <tr>
																 <td align="center" width="40%">
																	 <dx:ASPxButton ID="btn_date_header" runat="server" AutoPostBack="False"
																		 Text='<%# Container.Interval.Start.DayOfWeek.ToString().Remove(3) + " " + Container.Interval.Start.Day %>'
																		 UseSubmitBehavior="False"
																		 Width="50px" Border-BorderStyle="None" ForeColor="Gray" ClientEnabled="False">
																	 </dx:ASPxButton>
																 </td>
															 </tr>
														 </table>
													 </TimelineDateHeaderTemplate>
													 <HorizontalAppointmentTemplate>
														 <uc1:inlineappform ID="inlineappform1" runat="server" />
													 </HorizontalAppointmentTemplate>
												 </Templates>
												 <TimelineViewStyles>
													 <TimelineCellBody Height="25px" VerticalAlign="Middle" />
												 </TimelineViewStyles>
												 <AppointmentDisplayOptions StartTimeVisibility="Always" AppointmentAutoHeight="True" SnapToCellsMode="Auto" ShowRecurrence="true"></AppointmentDisplayOptions>
												 <CellAutoHeightOptions Mode="FitToContent" MinHeight="40"></CellAutoHeightOptions>
											 </TimelineView>
										 </Views>
										 <ClientSideEvents AppointmentDeleting="function(s, e) {
}"
											 AppointmentDrop="function(s, e) {
 scheduler.cpOperation = e.operation;
}"
											 EndCallback="function(s, e) {
                    if (s.cpWarning!=&#39;&#39;) {
                        alert(s.cpWarning);
                        s.cpWarning=&#39;&#39;;
                    }
if (s.cp_overview_refresh!=&#39;&#39;)
{
load_ov();
s.cp_overview_refresh=&#39;&#39;;
}
if (s.cp_gvworefresh!=null && s.cp_gvworefresh!=&#39;&#39;)
{
grid.PerformCallback(&#39;refresh&#39;);
s.cp_gvworefresh = &#39;&#39;;
}
}"
											 Init="schedulerObj.menuinit" 
											 AppointmentDoubleClick="function(s, e) {e.handled = true;}"></ClientSideEvents>
										 <Styles>
											 <PopupForm>
												 <Content BackColor="#E7F3EF">
												 </Content>
												 <Header BackColor="#00B239" Font-Names="Arial" Font-Size="16pt"
													 ForeColor="#EAEAEA" VerticalAlign="Middle">
													 <border borderstyle="None" />
												 </Header>
												 <ModalBackground Opacity="0">
												 </ModalBackground>
											 </PopupForm>
											 <Buttons>
												 <Style BackColor="White" />
											 </Buttons>
											 <LoadingPanel>
												 <Border borderstyle="None" />
											 </LoadingPanel>
											 <DayHeader Font-Size="8pt">
											 </DayHeader>
										 </Styles>
										 <Images>
											 <LoadingPanel Url="~/images/loading_panel.gif">
											 </LoadingPanel>
										 </Images>
										 <Templates>
											 <VerticalResourceHeaderTemplate>
												 <div class="ht <%# GetResourceColor(Container) %>">
													<div><%# Container.Resource.Caption %></div>
													<div><asp:Label ID="Label2" runat="server" Text='' Font-Size="XX-Small" /></div>
												 </div>
											 </VerticalResourceHeaderTemplate>
											 <ToolbarViewVisibleIntervalTemplate>
												 <div style="text-align: left; padding-left: 20px;  white-space: nowrap;" nowrap="nowrap" >
													 <%# Container.Scheduler.ActiveView.GetVisibleIntervals().Start.ToString("MMM dd yyyy") + " - " + Container.Scheduler.ActiveView.GetVisibleIntervals().End.ToString("MMM dd yyyy") %>
												 </div>
											 </ToolbarViewVisibleIntervalTemplate>
										 </Templates>
										 <OptionsCustomization AllowAppointmentResize="None" AllowAppointmentCopy="None"></OptionsCustomization>
										 <OptionsBehavior RecurrentAppointmentEditAction="Ask" ShowRemindersForm="False" ShowViewNavigatorGotoDateButton="True"></OptionsBehavior>

<OptionsCookies Enabled="True" CookiesID="sched"></OptionsCookies>

										 <OptionsToolTips AppointmentToolTipUrl="AppTooltip.ascx"></OptionsToolTips>
										 <OptionsView NavigationButtons-Visibility="Never"></OptionsView>
										 <OptionsLoadingPanel Text="" />
										 <ResourceNavigator Visibility="Never" />
									 	<ViewNavigator ShowGotoDateButton="True" ShowTodayButton="True" />
										 <Storage EnableReminders="False">
											 <Appointments AutoRetrieveId="True">
												 <CustomFieldMappings>
													 <dx:ASPxAppointmentCustomFieldMapping Member="woprog_id" Name="woprog_id" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="business_unit_id" Name="business_unit_id" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="quote_id" Name="quote_id" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="setby" Name="setby" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="scheduled_by" Name="scheduled_by" ValueType="Integer" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="assetid" Name="assetid" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="truckno" Name="truckno" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="membertype_id" Name="membertype_id" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="confirmed" Name="confirmed" ValueType="Boolean" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="notes" Name="notes" ValueType="String" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="meet_at_shop" Name="meet_at_shop" ValueType="Boolean" />
													 <dx:ASPxAppointmentCustomFieldMapping Member="date_of_entry" Name="date_of_entry" ValueType="DateTime" />
												 </CustomFieldMappings>
												 <Mappings AppointmentId="ID" End="EndDate" ResourceId="member_id"
													 Start="StartDate" Subject="Subject" Location="Location" Description="Description" Status="Status"></Mappings>
											 </Appointments>
											 <Resources>
												 <Mappings ResourceId="Member_ID" Caption="member_fullname"></Mappings>
											 </Resources>
										 </Storage>
									 </dx:ASPxScheduler>
						   </div>
                    			</td>
							</tr>
							</table>
                </td>
            </tr>
        </table>
		<div id="draggable3" class="ui-widget-content shadow" 
			style="top: 60px; right: 35px; position: fixed; width: 700px;  "><div align="center" 
										style="font-weight: 700; padding-bottom: 0.2em; padding-top:0.2em; background-color:#A5A2A5; color: White; cursor: move; font-family: Arial;">
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
	pg.SetVisible(false);
//	grid.SetVisible(false);
	btngrid_close.SetText('+');
}
else if (btngrid_close.GetText()=='+')
{
document.getElementById('draggable3').style.height= &quot;850px&quot;;
//	grid.SetVisible(true);
pg.SetVisible(true);
	btngrid_close.SetText('-');
}
}" />
								<Border BorderStyle="None" />
							</dx:ASPxButton>
						</td>
					</tr>
					</table>
			</div>
                    <dx:ASPxPageControl ID="pg" runat="server" 
				ActiveTabIndex="0" Width="100%" ClientInstanceName="pg" EnableCallBacks="True">
						<TabPages>
							<dx:TabPage Text="Work Orders">
								<ContentCollection>
									<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
									<dx:ASPxGridView ID="gvwos" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                        Width="100%" KeyFieldName="woprog_id" Font-Names="Arial" Font-Size="8pt" 
										Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="500" EnableTheming="True" 
										Theme="NETheme01" oncustomcallback="gvwos_CustomCallback" 
				onhtmldatacellprepared="gvwos_HtmlDataCellPrepared" OnHtmlRowPrepared="gvwos_HtmlRowPrepared" 
											OnRowUpdating="gvwos_RowUpdating">
<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" AutoFilterRowInputDelay="2400"></SettingsBehavior>
<SettingsEditing Mode="PopupEditForm"></SettingsEditing>
<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" VerticalScrollableHeight="600" 
											VerticalScrollBarMode="Auto"></Settings>
                        <Columns>
                            <dx:GridViewCommandColumn VisibleIndex="0" Caption=" " Width="25px"  ShowClearFilterButton="true"
								ButtonType="Link" ShowEditButton="True">
								<CellStyle VerticalAlign="Middle">
								</CellStyle>
							</dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="woprog_id" ReadOnly="True" 
								Visible="False" VisibleIndex="2" Width="35px">
                            	<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="woprog_bvwo" VisibleIndex="3" 
								Caption="WO" Width="55px" CellStyle-HorizontalAlign="Left" CellStyle-VerticalAlign="Middle" 
								ReadOnly="True">
                            	<Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="BeginsWith"></Settings>
								<DataItemTemplate>
                                <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
				NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}', 'workorder{0}', 1200,800)&quot;, Eval(&quot;woprog_id&quot;), Eval(&quot;business_unit_id&quot;)) %>" 
				Text='<%# Eval("woprog_bvwo") %>' Cursor="pointer" Font-Bold="false" Height=" ">
				</dx:ASPxHyperLink>      
                                </DataItemTemplate>
<CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Customer" FieldName="woprog_customername" 
								VisibleIndex="4" Width="70px" ReadOnly="True">
								<Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
<Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList"></Settings>
								<CellStyle Wrap="False">
								</CellStyle>
							</dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="woprog_description" VisibleIndex="5" 
								Caption="Description" Width="100%" ReadOnly="True">
                            	<Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>
                            	<CellStyle Wrap="False">
								</CellStyle>
                            </dx:GridViewDataTextColumn>
                             <dx:GridViewDataHyperLinkColumn Caption=" " ReadOnly="True" VisibleIndex="1" 
								Width="25px">
                                <Settings AllowSort="False"></Settings>
                                <EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                                <DataItemTemplate>
                                    <div class="draggable">
                                        <a href="#" title="Image Viewer" style="cursor: move">
                                            <img src="../../../images/icon/icon[drag].png" alt="" />
                                        </a>
                                        <input type="hidden" value='<%# Eval("woprog_id") %>' />
                                    </div>
                                </DataItemTemplate>
                            </dx:GridViewDataHyperLinkColumn>
                            <dx:GridViewDataTextColumn FieldName="TypeID" Visible="False">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="address" FieldName="woprog_address_id" 
								VisibleIndex="16" Width="0px" Visible="False">
							</dx:GridViewDataTextColumn>
                        	<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit_id" 
								Visible="False" VisibleIndex="17">
							</dx:GridViewDataTextColumn>
                        	<dx:GridViewDataTextColumn Caption="Status" FieldName="_status" 
								VisibleIndex="6" Width="60px" ReadOnly="True">
							</dx:GridViewDataTextColumn>
                        	<dx:GridViewDataDateColumn Caption="Start" FieldName="start_date" 
								VisibleIndex="7" Width="70px">
								<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
									EditFormatString="yyyy-MM-dd">
								</PropertiesDateEdit>
							</dx:GridViewDataDateColumn>
							<dx:GridViewDataDateColumn Caption="End" FieldName="end_date" VisibleIndex="8" 
								Width="70px">
								<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
									EditFormatString="yyyy-MM-dd">
								</PropertiesDateEdit>
							</dx:GridViewDataDateColumn>
							<dx:GridViewDataTextColumn Caption="Exp Hours" FieldName="exp_hours" 
								VisibleIndex="9" Width="40px">
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Hours To Date" FieldName="hours_to_date" 
								VisibleIndex="10" Width="50px" ReadOnly="True">
								<FilterCellStyle Wrap="True">
								</FilterCellStyle>
								<HeaderStyle Wrap="True" />
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Future Hours" FieldName="future_hours" 
								VisibleIndex="11" Width="50px" ReadOnly="True">
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="PM" FieldName="pm" ShowInCustomizationForm="True" 
								 VisibleIndex="18" Width="50px" ReadOnly="True"><CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Priority" FieldName="Priority" 
								Visible="False" VisibleIndex="15">
							</dx:GridViewDataTextColumn>
                        	<dx:GridViewDataTextColumn Caption="Sched_date" FieldName="_date" 
								ShowInCustomizationForm="True" Visible="False" VisibleIndex="14">
							</dx:GridViewDataTextColumn>
                        	<dx:GridViewDataTextColumn FieldName="_sc" ShowInCustomizationForm="True" 
								Visible="False" VisibleIndex="13">
							</dx:GridViewDataTextColumn>
                        </Columns>
                    	<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" 
							/>
						<SettingsPager PageSize="100" AlwaysShowPager="True">
						</SettingsPager>
										<SettingsEditing Mode="PopupEditForm" />
						<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" 
							VerticalScrollableHeight="600" ShowTitlePanel="True" />
										<SettingsText PopupEditFormCaption="Edit WO" />
<SettingsText PopupEditFormCaption="Edit WO"></SettingsText>
                    	<SettingsPopup>
							<EditForm Height="200px" HorizontalAlign="WindowCenter" Modal="True" 
								VerticalAlign="WindowCenter" Width="600px" />
<EditForm Width="600px" Height="200px" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Modal="True"></EditForm>
<HeaderFilter Width="300px" MinHeight="500px"></HeaderFilter>
						</SettingsPopup>
                    	<Styles>
							<Disabled ForeColor="Silver">
							</Disabled>
							<Header Wrap="True">
							</Header>
							<Cell VerticalAlign="Middle">
							</Cell>
						</Styles>
                    					<StylesPopup>
											<EditForm>
												<Header Font-Names="Arial" Font-Size="14pt">
												</Header>
												<ModalBackground Opacity="0">
												</ModalBackground>
											</EditForm>
										</StylesPopup>
                    					<Templates>
											<TitlePanel>
												<table style="width: 100%; font-family: Arial, Helvetica, sans-serif; font-size: x-small; color: #0000000;">
					<tr>
						<td>
							<strong>Legend:  </strong></td>
						<td nowrap="nowrap" style="background-color: #CCFFCC; color: #000000;">
							Scheduled in the Future</td>
						<td>
							&nbsp;</td>
						<td>
							</td>
						<td width="100%">
							&nbsp;</td>
						<td>
							&nbsp;</td>
					</tr>
				</table>
											</TitlePanel>
										</Templates>
                    </dx:ASPxGridView></dx:ContentControl>
								</ContentCollection>
							</dx:TabPage>
							<dx:TabPage Text="Quotes">
								<ContentCollection>
									<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
									<dx:ASPxGridView ID="gv_quotes" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_quotes"
                        Width="100%" KeyFieldName="quote_id" Font-Names="Arial" Font-Size="8pt" 
										Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="500" EnableTheming="True" 
										Theme="NETheme01" oncustomcallback="gvquotes_CustomCallback" 
				onhtmldatacellprepared="gvquotes_HtmlDataCellPrepared" OnRowUpdating="gv_quotes_RowUpdating">
<SettingsBehavior EnableRowHotTrack="True" AutoFilterRowInputDelay="2400"></SettingsBehavior>
<SettingsEditing Mode="PopupEditForm"></SettingsEditing>
<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" VerticalScrollableHeight="650" VerticalScrollBarMode="Auto"></Settings>
                        <Columns>
                            <dx:GridViewCommandColumn VisibleIndex="0" Caption=" " Width="25px" ShowEditButton="true" ShowClearFilterButton="true">
							</dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="quote_id" ReadOnly="True" 
								Visible="False" VisibleIndex="2" Width="35px">
                            	<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="quote_id_rev" VisibleIndex="3" 
								Caption="Quote" Width="60px" CellStyle-HorizontalAlign="Left" CellStyle-VerticalAlign="Middle" 
								ReadOnly="True">
                            	<Settings AutoFilterCondition="BeginsWith" />
<Settings AutoFilterCondition="BeginsWith"></Settings>
								<DataItemTemplate>
                                    <div class="draggable" style="color: #0000000;  text-align: center; vertical-align: middle;" align="center">
                                       <a href="#" title="Image Viewer" style="cursor: move; color: #0000000;">
                                            </a><input type="hidden" value='<%# Container.VisibleIndex %>' style="color: #0000000" align="left" />
													<a href="#" style="cursor: move; color: #0000000; text-align: left; vertical-align: middle;" title="Image Viewer">
													<%# Eval("quote_id_rev")%>
													</a>
                                    </div>
                                </DataItemTemplate>
<CellStyle HorizontalAlign="Left" VerticalAlign="Middle"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Customer" FieldName="quote_customername" 
								VisibleIndex="4" Width="70px" ReadOnly="True">
								<Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
<Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList"></Settings>
								<CellStyle Wrap="False">
								</CellStyle>
							</dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="quote_description" VisibleIndex="5" 
								Caption="Description" Width="100%" ReadOnly="True">
                            	<Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>
                            	<CellStyle Wrap="False">
								</CellStyle>
                            </dx:GridViewDataTextColumn>
                             <dx:GridViewDataHyperLinkColumn Caption=" " ReadOnly="True" VisibleIndex="1" 
								Width="25px">
                                <Settings AllowSort="False"></Settings>
                                <EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
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
                            <dx:GridViewDataTextColumn Caption="address" FieldName="quote_address_id" 
								VisibleIndex="13" Width="0px" Visible="False">
							</dx:GridViewDataTextColumn>
                        	<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="quote_business_unit_id" 
								Visible="False" VisibleIndex="14">
							</dx:GridViewDataTextColumn>
                        	<dx:GridViewDataTextColumn Caption="Status" FieldName="_status" 
								VisibleIndex="6" Width="90px" ReadOnly="True">
							    <Settings HeaderFilterMode="CheckedList" />
							</dx:GridViewDataTextColumn>
                        	<dx:GridViewDataDateColumn Caption="Cut Date" FieldName="open_date" 
								VisibleIndex="7" Width="70px" ReadOnly="True">
								<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
								</PropertiesDateEdit>
								<Settings AutoFilterCondition="Less" />
								<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
							</dx:GridViewDataDateColumn>
							<dx:GridViewDataDateColumn Caption="Due" FieldName="due_date" VisibleIndex="8" 
								Width="70px">
								<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
									EditFormatString="yyyy-MM-dd">
								</PropertiesDateEdit>
							</dx:GridViewDataDateColumn>
							<dx:GridViewDataTextColumn Caption="Exp Hours" FieldName="exp_hours" 
								VisibleIndex="9" Width="50px">
								<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Hours To Date" FieldName="hours_spent" 
								VisibleIndex="10" Width="50px" ReadOnly="True">
								<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
								<FilterCellStyle Wrap="True">
								</FilterCellStyle>
								<HeaderStyle Wrap="True" />
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Future Hours" FieldName="future_hours" 
								VisibleIndex="11" Width="50px" ReadOnly="True">
								<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Priority" FieldName="Priority" 
								Visible="False" VisibleIndex="12">
							</dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="By" FieldName="quoted_by" ShowInCustomizationForm="True" VisibleIndex="15" Width="50px">
                                <Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    	<SettingsBehavior EnableRowHotTrack="True" 
							/>
						<SettingsPager PageSize="150" Visible="False">
						</SettingsPager>
										<SettingsEditing Mode="PopupEditForm" />
						<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" 
							VerticalScrollableHeight="650" />
										<SettingsText PopupEditFormCaption="Edit Quote" />
<SettingsText PopupEditFormCaption="Edit Quote"></SettingsText>
                    	<SettingsPopup>
<EditForm Width="500px" Height="250px" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Modal="True"></EditForm>
<HeaderFilter Width="300px" MinHeight="500px"></HeaderFilter>
							<EditForm Height="250px" HorizontalAlign="WindowCenter" Modal="True" 
								VerticalAlign="WindowCenter" Width="500px" />
							<HeaderFilter MinHeight="500px" Width="300px" />
						</SettingsPopup>
                    	<Styles>
							<Header Wrap="True">
							</Header>
						</Styles>
                    					<StylesPopup>
											<EditForm>
												<Header Font-Names="Arial" Font-Size="14pt">
												</Header>
												<ModalBackground Opacity="0">
												</ModalBackground>
											</EditForm>
										</StylesPopup>
                    </dx:ASPxGridView>
									</dx:ContentControl>
								</ContentCollection>
							</dx:TabPage>
							<dx:TabPage Text="Service Calls" ClientVisible="False">
								<ContentCollection>
									<dx:ContentControl runat="server">
										<dx:ASPxGridView ID="gvwos0" runat="server" AutoGenerateColumns="False" 
											ClientInstanceName="grid" EnableTheming="True" Font-Names="Arial" 
											Font-Size="8pt" KeyFieldName="woprog_id" 
											OnCustomCallback="gvwos_CustomCallback" 
											OnHtmlDataCellPrepared="gvwos_HtmlDataCellPrepared" 
											OnHtmlRowPrepared="gvwos_HtmlRowPrepared" OnRowUpdating="gvwos_RowUpdating" 
											Theme="NETheme01" Width="100%">
											<Columns>
												<dx:GridViewCommandColumn ButtonType="Link" Caption=" " ShowEditButton="True"  ShowClearFilterButton="true"
													ShowInCustomizationForm="True" VisibleIndex="0" Width="25px">
													<CellStyle VerticalAlign="Middle">
													</CellStyle>
												</dx:GridViewCommandColumn>
												<dx:GridViewDataTextColumn FieldName="woprog_id" ReadOnly="True" 
													ShowInCustomizationForm="True" Visible="False" VisibleIndex="2" Width="35px">
													<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="WO" FieldName="woprog_bvwo" ReadOnly="True" 
													ShowInCustomizationForm="True" VisibleIndex="3" Width="50px">
													<Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="BeginsWith"></Settings>
													<DataItemTemplate>
														<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" Cursor="pointer" 
															Font-Bold="false" Height=" " 
															NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}', 'workorder{0}', 1200,800)&quot;, Eval(&quot;woprog_id&quot;), Eval(&quot;business_unit_id&quot;)) %>" 
															Text='<%# Eval("woprog_bvwo") %>'>
														</dx:ASPxHyperLink>
													</DataItemTemplate>
													<CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Customer" FieldName="woprog_customername" 
													ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="4" Width="70px">
													<Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
<Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList"></Settings>
													<CellStyle Wrap="False">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Description" FieldName="woprog_description" 
													ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="5" Width="100%">
													<Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>
													<CellStyle Wrap="False">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataHyperLinkColumn Caption=" " ReadOnly="True" 
													ShowInCustomizationForm="True" VisibleIndex="1" Width="25px">
													<Settings AllowSort="False" />
													<EditFormSettings Visible="False" />
<Settings AllowSort="False"></Settings>
<EditFormSettings Visible="False"></EditFormSettings>
													<DataItemTemplate>
														<div class="draggable">
															<a href="#" style="cursor: move" title="Image Viewer">
															<img src="../../../images/icon/icon[drag].png" alt="" />
															</a>
															<input type="hidden" value='<%# Eval("woprog_id") %>' />
														</div>
													</DataItemTemplate>
												</dx:GridViewDataHyperLinkColumn>
												<dx:GridViewDataTextColumn FieldName="TypeID" ShowInCustomizationForm="True" 
													Visible="False">
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="address" FieldName="woprog_address_id" 
													ShowInCustomizationForm="True" Visible="False" VisibleIndex="16" Width="0px">
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit_id" 
													ShowInCustomizationForm="True" Visible="False" VisibleIndex="17">
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Status" FieldName="_status" ReadOnly="True" 
													ShowInCustomizationForm="True" VisibleIndex="6" Width="80px">
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataDateColumn Caption="Start" FieldName="start_date" 
													ShowInCustomizationForm="True" VisibleIndex="7" Width="70px">
													<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
														EditFormatString="yyyy-MM-dd">
													</PropertiesDateEdit>
												</dx:GridViewDataDateColumn>
												<dx:GridViewDataDateColumn Caption="End" FieldName="end_date" 
													ShowInCustomizationForm="True" VisibleIndex="8" Width="70px">
													<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
														EditFormatString="yyyy-MM-dd">
													</PropertiesDateEdit>
												</dx:GridViewDataDateColumn>
												<dx:GridViewDataTextColumn Caption="Exp Hours" FieldName="exp_hours" 
													ShowInCustomizationForm="True" VisibleIndex="9" Width="50px">
													<CellStyle HorizontalAlign="Center">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Hours To Date" FieldName="hours_to_date" 
													ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="10" Width="50px">
													<FilterCellStyle Wrap="True">
													</FilterCellStyle>
													<HeaderStyle Wrap="True" />
													<CellStyle HorizontalAlign="Center">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Future Hours" FieldName="future_hours" 
													ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="11" Width="50px">
													<CellStyle HorizontalAlign="Center">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Priority" FieldName="Priority" 
													ShowInCustomizationForm="True" Visible="False" VisibleIndex="15">
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Sched_date" FieldName="_date" 
													ShowInCustomizationForm="True" Visible="False" VisibleIndex="14">
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn FieldName="_sc" ShowInCustomizationForm="True" 
													Visible="False" VisibleIndex="13">
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsBehavior AutoFilterRowInputDelay="2400" ColumnResizeMode="Control" 
												EnableRowHotTrack="True" />
<SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control" AutoFilterRowInputDelay="2400"></SettingsBehavior>
											<SettingsPager AlwaysShowPager="True" PageSize="100">
											</SettingsPager>
											<SettingsEditing Mode="PopupEditForm">
											</SettingsEditing>
											<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" 
												ShowTitlePanel="True" VerticalScrollableHeight="650" 
												VerticalScrollBarMode="Auto" />
											<SettingsText PopupEditFormCaption="Edit WO" />
<Settings ShowTitlePanel="True" ShowFilterRow="True" ShowHeaderFilterButton="True" VerticalScrollableHeight="600" VerticalScrollBarMode="Auto"></Settings>
<SettingsText PopupEditFormCaption="Edit WO"></SettingsText>
											<SettingsPopup>
												<EditForm Height="200px" HorizontalAlign="WindowCenter" Modal="True" 
													VerticalAlign="WindowCenter" Width="600px" />
												<HeaderFilter MinHeight="500px" Width="300px" />
<EditForm Width="600px" Height="200px" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Modal="True"></EditForm>
<HeaderFilter Width="300px" MinHeight="500px"></HeaderFilter>
											</SettingsPopup>
											<Styles>
												<Header Wrap="True">
												</Header>
												<Disabled ForeColor="Silver">
												</Disabled>
												<Cell VerticalAlign="Middle">
												</Cell>
											</Styles>
											<StylesPopup>
												<EditForm>
													<Header Font-Names="Arial" Font-Size="14pt">
													</Header>
													<ModalBackground Opacity="0">
													</ModalBackground>
												</EditForm>
											</StylesPopup>
											<Templates>
												<TitlePanel>
													<table style="width: 100%; font-family: Arial, Helvetica, sans-serif; font-size: x-small; color: #0000000;">
														<tr>
															<td>
																<strong>Legend: </strong>
															</td>
															<td nowrap="nowrap" style="background-color: #CCFFCC; color: #000000;">
																Scheduled in the Future</td>
															<td>
																&nbsp;</td>
															<td width="100%">
																&nbsp;</td>
															<td>
																&nbsp;</td>
														</tr>
													</table>
												</TitlePanel>
											</Templates>
										</dx:ASPxGridView>
									</dx:ContentControl>
								</ContentCollection>
							</dx:TabPage>
						</TabPages>
<ClientSideEvents ActiveTabChanged="function(s, e) {
	if ((s.GetActiveTabIndex()==0))
		{
		hf.Set(&#39;is_wo&#39;,&#39;1&#39;);
		}	
    else if((s.GetActiveTabIndex()==2)){
        hf.Set(&#39;is_wo&#39;,&#39;2&#39;);
        }
	else
		{
		hf.Set(&#39;is_wo&#39;,&#39;0&#39;);
		}
}"></ClientSideEvents>
			</dx:ASPxPageControl>
                    </div>
         <asp:SqlDataSource ID="ResourceDataSourcex" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
             SELECT Member_ID, member_fullname, 1 ord 
             FROM member inner join membertype on member.member_membertype_id = membertype.membertype_id 
             where business_unit_id = ?cid and member_status = 'Active' and (member.reports_to = ?pid or member.Member_ID =?pid) and member.reports_to!=0 and membertype.is_scheduled = 1 
             and membertype.membertype_id not in (Select appointment_resource_member_hide_link.resource_id from appointment_resource_member_hide_link where appointment_resource_member_hide_link.member_id = ?mid) 
             UNION
             SELECT distinct mt_id_us Member_ID, membertype_name member_fullname, 2 ord 
             FROM membertype inner JOIN appointment_resource_member_link on membertype.membertype_id = appointment_resource_member_link.resource_id and appointment_resource_member_link.member_id = ?mid 
             where membertype.is_scheduled = 1 
             UNION
             SELECT Member_ID, member_fullname, 3 ord 
             FROM member inner join membertype on member.member_membertype_id = membertype.membertype_id 
             where business_unit_id = ?cid and ((member.reports_to != ?pid and member.member_id!=?pid) or member.reports_to=0) and member_startdate&lt;=curdate() and member_termdate &gt; curdate() and membertype.is_scheduled = 1  and member_status = 'Active' 
            and membertype.membertype_id not in (Select appointment_resource_member_hide_link.resource_id from appointment_resource_member_hide_link where appointment_resource_member_hide_link.member_id = ?mid) 
               ORDER BY ord,member_fullname
" onselecting="ResourceDataSourcex_Selecting">
			 <SelectParameters>
				 <asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
				 <asp:ControlParameter ControlID="ddlpm" Name="pid" PropertyName="Value" />
				  <asp:ControlParameter ControlID="hdnmid0" Name="mid" PropertyName="Value" />
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
			InsertCommand="INSERT INTO [Appointments] ([EventType], [StartDate], [EndDate], [AllDay], [Subject], [Location], [Description], [Status], [Label], [ResourceID], [RecurrenceInfo], [ReminderInfo], [ContactInfo],[woprog_id],[quote_id],[business_unit_id],[setby],[assetid],[member_id],[membertype_id],[confirmed],[notes],[date_of_entry]) VALUES (@EventType, @StartDate, @EndDate, @AllDay, @Subject, @Location, @Description, @Status, @Label, @member_id, @RecurrenceInfo, @ReminderInfo, @ContactInfo,@woprog_id,@quote_id,@cid,@setby,@assetid,@member_id,@membertype_id,@confirmed,@notes,now())"
			UpdateCommand="UPDATE [Appointments] SET [EventType] = @EventType, [StartDate] = @StartDate, [EndDate] = @EndDate, [AllDay] = @AllDay, [Subject] = @Subject, [Location] = @Location, [Description] = @Description, [Status] = @Status, [Label] = @Label, [ResourceID] = @member_id, [RecurrenceInfo] = @RecurrenceInfo, [ReminderInfo] = @ReminderInfo, [ContactInfo] = @ContactInfo, [woprog_id]=@woprog_id, [setby]=@setby, [assetid]=@assetid,[member_id]=@member_id, [membertype_id]=@membertype_id,[confirmed]=@confirmed,[notes]=@notes WHERE [ID] = @ID"
			DeleteCommand="Delete from appointments where id = @id"
			SelectCommand="call `ds_scheduler`(?cid,?_start)"
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
                <asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
				<asp:Parameter Name="quote_id" Type="String" />
				<asp:Parameter Name="setby" Type="String" />
				<asp:Parameter Name="assetid" Type="String" />
				<asp:Parameter Name="member_id" Type="String" />
				<asp:Parameter Name="membertype_id" Type="String" />
				<asp:Parameter Name="confirmed" Type="Boolean" />
				<asp:Parameter Name="notes" Type="String" />
				<asp:Parameter Name="meet_at_shop" Type="Boolean" />
                <asp:Parameter Name="date_of_entry" Type="DateTime" />
            </InsertParameters>
             <SelectParameters>
				 <asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
			     <asp:SessionParameter Name="_start" SessionField="start_date2" />
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
				<asp:Parameter Name="cid" Type="String" />
				<asp:Parameter Name="quote_id" Type="String" />
				<asp:Parameter Name="setby" Type="String" />
                <asp:Parameter Name="ID" Type="Int32" />
				<asp:Parameter Name="assetid" Type="String" />
				<asp:Parameter Name="member_id" Type="String" />
				<asp:Parameter Name="membertype_id" Type="String" />
				<asp:Parameter Name="confirmed" Type="Boolean" />
					<asp:Parameter Name="notes" Type="String" />
					<asp:Parameter Name="meet_at_shop" Type="Boolean" />
                 <asp:Parameter Name="date_of_entry" Type="DateTime" />
            </UpdateParameters>
        </asp:SqlDataSource>
       <dx:ASPxPopupControl ID="pop_edit" runat="server" ClientInstanceName="pop_edit" 
			PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
			ShowPageScrollbarWhenModal="True"  
			PopupAnimationType="None" Height="400px" 
			onwindowcallback="pop_edit_WindowCallback1"  
			Width="600px" style="font-family: Arial, Helvetica, sans-serif" AllowDragging="True" CloseAction="CloseButton" 
			HeaderText="Edit Schedule" Modal="True" SettingsLoadingPanel-Delay="200" SettingsLoadingPanel-ImagePosition="Left">
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
								ClientInstanceName="hl_customer" Cursor="pointer">
							</dx:ASPxHyperLink>
						</td>
					</tr>
					<tr>
						<td class="style4" nowrap="nowrap" style="height: 15px">
							Work Order</td>
						<td align="left" style="height: 15px">
							<dx:ASPxHyperLink ID="hl_wo" runat="server" ClientInstanceName="hl_wo" 
								Cursor="pointer" Wrap="False">
							</dx:ASPxHyperLink>
						</td>
						<td align="left" style="height: 15px" width="100%">&nbsp;
						</td>
					</tr>
                    <tr>
						<td align="center" class="style4" style="text-align: left">
							</td>
						<td align="center" colspan="2" style="text-align: left">
							<dx:ASPxLabel runat="server" ClientInstanceName="lbl_desc" Font-Size="9px" 
								ID="lbl_desc"></dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							Contact</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxLabel runat="server" ClientInstanceName="lblcontact"  
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
							Address</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxLabel ID="txt_address" runat="server" Width="100%">
                            </dx:ASPxLabel>
						</td>
					</tr>
                    <tr>
						<td class="style4" style="height: 15px">
							Location</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxTextBox ID="txt_location" runat="server" Width="100%">
                            </dx:ASPxTextBox>
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
								DataSourceID="sql_trucks" TextField="_name" 
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
							<dx:ASPxDateEdit ID="te_start" runat="server" ClientInstanceName="te_start" 
								DisplayFormatString="MMM d HH:mm" EditFormat="DateTime" 
								EditFormatString="MMM d HH:mm">
								<TimeSectionProperties Visible="True">
								</TimeSectionProperties>
								<ClientSideEvents DateChanged="function(s, e) {
	check_startdate(s);
}" />
							</dx:ASPxDateEdit>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							End Time</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxDateEdit ID="te_end" runat="server" ClientInstanceName="te_end" 
								DisplayFormatString="MMM d HH:mm" EditFormat="Custom" 
								EditFormatString="MMM d HH:mm">
								<TimeSectionProperties Visible="True">
								</TimeSectionProperties>
								<ClientSideEvents DateChanged="function(s, e) {
	check_enddate(s);
}" />
							</dx:ASPxDateEdit>
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
							Date of Entry</td>
						<td align="left" colspan="2" style="height: 15px">
							<dx:ASPxLabel ID="lbl_date_of_entry" runat="server" ClientInstanceName="lbl_date_of_entry" 
								Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td class="style4" style="height: 15px">
							Notes / WO Special Instructions:</td>
						<td align="left" style="height: 15px" colspan="2">
							<dx:ASPxMemo ID="mem_notes" runat="server" ClientInstanceName="mem_notes" 
								Height="71px" Width="100%">
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
											HorizontalAlign="Center" Text="Save and Close" Width="100px">
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
    <dx:ASPxPopupControl ID="pop_recurrence" runat="server" 
	ClientInstanceName="pop_recurrence" HeaderText="Add Recurrence" Modal="True" 
	PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" 
	style="font-family: Arial, Helvetica, sans-serif"  AllowDragging="True" Height="300px" Width="800px" 
	onwindowcallback="pop_recurrence_WindowCallback" ShowPageScrollbarWhenModal="True" CloseAction="CloseButton">
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
					ClientInstanceName="lbl_customer_rec" Text=" ">
				</dx:ASPxLabel>
			</td>
			<td colspan="2">
				&nbsp;</td>
			<td colspan="2">
				&nbsp;</td>
			<td class="style1">
				<dx:ASPxLabel ID="lbl_original_date" runat="server" 
					ClientInstanceName="lbl_original_date" Text=" " Wrap="False">
				</dx:ASPxLabel>
			</td>
		    <td>
                <dx:ASPxCheckBox ID="chk_allow_adding_vacation_days" runat="server" CheckState="Unchecked" Text="Allow Adding to Vacation Days">
                </dx:ASPxCheckBox>
            </td>
		</tr>
		<tr>
			<td colspan="5" valign="top" width="100%" class="auto-style1">
				<dx:ASPxLabel ID="lbl_member_rec" runat="server" 
					ClientInstanceName="lbl_member_rec" Text=" ">
				</dx:ASPxLabel>
			</td>
			<td class="style1" valign="top">
				<dx:ASPxLabel ID="lbl_start_and_end" runat="server" 
					ClientInstanceName="lbl_start_and_end" Text=" " Wrap="False">
				</dx:ASPxLabel></td>
		    <td valign="top">
                <dx:ASPxCheckBox ID="chk_allow_unavailable" runat="server" CheckState="Unchecked" Text="Allow Adding to Unavailable Days">
                    <RootStyle Wrap="False">
                    </RootStyle>
                </dx:ASPxCheckBox>
              </td>
		</tr>
		<tr>
			<td colspan="5" valign="top" width="100%">
				<dx:ASPxLabel ID="lbl_description_rec" runat="server" 
					ClientInstanceName="lbl_description_rec" Text=" ">
				</dx:ASPxLabel>
			</td>
			<td class="style1" valign="top">
				&nbsp;</td>
		    <td class="style1" valign="top">
                <dx:ASPxCheckBox ID="chkallow_weekends" runat="server" CheckState="Unchecked" ClientInstanceName="chkallow_weekends" style="margin-bottom: 0px" Text="Allow Adding to Weekends">
                </dx:ASPxCheckBox>
            </td>
		</tr>
		<tr>
			<td colspan="6" style="vertical-align: top">
				<dx:ASPxCalendar ID="ASPxCalendar1" runat="server" Columns="2" 
					EnableMultiSelect="True" EnableTheming="True" EnableYearNavigation="False" 
					Font-Names="Arial" Rows="2" ShowClearButton="False" ShowShadow="False" 
					ShowTodayButton="False" Width="100%" CssClass="calendar_override" OnDayCellPrepared="ASPxCalendar1_DayCellPrepared"
					>
					<MonthGridPaddings Padding="10px" />
					<ClientSideEvents SelectionChanged="function(s, e) {
	recurrenceDateChanged(s, e);
}" />
				</dx:ASPxCalendar>
			</td>
		    <td style="vertical-align: top">
                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select member_id,member_fullname from member inner join membertype on membertype.membertype_id = member.member_membertype_id and membertype.is_scheduled=1 where member.business_unit_id=@cid and member_status='Active' order by member_fullname">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdn_cid" Name="cid" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <dx:ASPxCheckBoxList ID="chk_others" runat="server" ClientInstanceName="chk_others" DataSourceID="SqlDataSource3" RepeatColumns="2" TextField="member_fullname" ValueField="member_id" ValueType="System.Int32" Width="250px" ItemSpacing="5px">
                </dx:ASPxCheckBoxList>
            </td>
		</tr>
		<tr>
			<td colspan="2">
				<dx:ASPxLabel ID="lblaptid" runat="server" ClientInstanceName="lblaptid" 
					ForeColor="Transparent" Text=" ">
				</dx:ASPxLabel>
				<table style="width: 100px;">
					<tr>
						<td class="style2" >
							</td>
					</tr>
					<tr>
						<td class="style2" >
							</td>
					</tr>
				</table>
			</td>
			<td colspan="2" style="text-align: right">
				&nbsp;</td>
			<td colspan="2" align="right">
				<dx:ASPxButton ID="btnsaverecur" runat="server" AutoPostBack="False" 
					EnableTheming="True" Text="Save">
					<ClientSideEvents Click="function(s, e) {
	pop_recurrence.PerformCallback('save');
}" />
				</dx:ASPxButton>
			</td>
		    <td align="right">&nbsp;</td>
		</tr>
	</table>
	<br />
			</dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_event" runat="server" 
	ClientInstanceName="pop_event" CloseAction="CloseButton" FooterText="" 
	HeaderText="Event" Modal="True" PopupHorizontalAlign="WindowCenter" 
	PopupVerticalAlign="WindowCenter" style="font-family: Arial, Helvetica, sans-serif"  
	onwindowcallback="pop_event_WindowCallback" oninit="pop_event_Init">
		<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_close==&quot;1&quot;)
		{
scheduler.Refresh();
		pop_event.Hide();
		}
}" />
		<ModalBackgroundStyle Opacity="0">
		</ModalBackgroundStyle>
		<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<table style="width:100%;">
		<tr>
			<td nowrap="nowrap">
				Select Event:</td>
			<td>
				<dx:ASPxComboBox ID="ddl_events" runat="server" 
					DataSourceID="sql_events" TextField="type" ValueField="id" 
					ValueType="System.Int32">
					<ClientSideEvents SelectedIndexChanged="function(s, e) {
	if (s.GetValue()==69)
{
ddl_point.SetVisible(false);
                        lbl_point.SetVisible(false);
                        lbl_mand.SetVisible(false);
					chk_mandatory.SetVisible(false);
}
else
{
ddl_point.SetVisible(true);
                          lbl_point.SetVisible(true);
                        lbl_mand.SetVisible(true);
					chk_mandatory.SetVisible(true);
}
}" />
					<Items>
						<dx:ListEditItem Text="Safety Meeting" Value="51" />
						<dx:ListEditItem Text="Company Party" Value="52" />
						<dx:ListEditItem Text="Internal Meeting" Value="53" />
						<dx:ListEditItem Text="Customer Meeting" Value="54" />
					</Items>
				</dx:ASPxComboBox>
				<asp:SqlDataSource ID="sql_events" runat="server" 
					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
					SelectCommand="Select * from membertime_shop_type where status = 'active' union select 69 id, 'Unavailable' type, 'active' status">
				</asp:SqlDataSource>
			</td>
		</tr>
		<tr>
			<td class="style5">
				&nbsp;</td>
			<td class="style5">
			</td>
		</tr>
		<tr>
			<td nowrap="nowrap">
				Start Time:</td>
			<td>
				<dx:ASPxTimeEdit ID="te_start2" runat="server">
				</dx:ASPxTimeEdit>
			</td>
		</tr>
		<tr>
			<td>
				End Time:</td>
			<td>
				<dx:ASPxTimeEdit ID="te_end2" runat="server" 
					EditFormat="Custom">
				</dx:ASPxTimeEdit>
			</td>
		</tr>
		<tr id="mandatory_row" runat="server">
			<td>  <dx:ASPxLabel ID="lbl_mand" runat="server" ClientInstanceName="lbl_mand" 
					Text="Mandatory?">
				</dx:ASPxLabel>
				</td>
			<td>
				<dx:ASPxCheckBox ID="chk_mandatory" ClientInstanceName="chk_mandatory"  runat="server" CheckState="Unchecked" 
					Text=" ">
				</dx:ASPxCheckBox>
			</td>
		</tr>
		<tr id="point_person_row" runat="server">
			<td>
                <dx:ASPxLabel ID="lbl_point" runat="server" ClientInstanceName="lbl_point" 
					Text="Point Person:">
				</dx:ASPxLabel>
				</td>
			<td>
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
					SelectCommand="Select member_id,member_fullname from member where business_unit_id = ?cid and member_status = 'Active' order by member_fullname">
					<SelectParameters>
						<asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
					</SelectParameters>
				</asp:SqlDataSource>
				<dx:ASPxComboBox ID="ddl_point" runat="server" 
					DataSourceID="SqlDataSource1" TextField="member_fullname" 
					ValueField="member_id" ValueType="System.Int32" ClientInstanceName="ddl_point">
				</dx:ASPxComboBox>
			</td>
		</tr>
		<tr>
			<td class="style6">
				Notes:</td>
			<td class="style6">
				<dx:ASPxMemo ID="mem_event_notes" runat="server" Height="71px" 
					Width="170px">
				</dx:ASPxMemo>
			</td>
		</tr>
		<tr>
			<td nowrap="nowrap" style="visibility: hidden">
				Set for Whole Branch?</td>
			<td>
				<dx:ASPxCheckBox ID="chk_event_whole_branch" runat="server" 
					CheckState="Unchecked" Text=" " ClientVisible="False">
				</dx:ASPxCheckBox>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
			<td align="right" style="text-align: right">
				<dx:ASPxButton ID="btnsaveevent" runat="server" AutoPostBack="False" 
					EnableTheming="True" Text="Save">
					<ClientSideEvents Click="function(s, e) {
	pop_event.PerformCallback('save|'+ pop_event.cp_aptid);
}" />
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
			</dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>
    <p>
		<dx:ASPxPopupControl ID="pop_ts" runat="server" 
			style="font-family: Arial, Helvetica, sans-serif"  
			HeaderText="Select Times" Modal="True" ClientInstanceName="pop_ts" 
			PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ModalBackgroundStyle-Opacity="0"
			onwindowcallback="pop_ts_WindowCallback" oninit="pop_ts_Init">
		<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_hide=='1')
{
s.Hide();
scheduler.Refresh();
}
}" />
<ModalBackgroundStyle Opacity="0"></ModalBackgroundStyle>
			<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<table style="width:100%;">
		<tr>
			<td>
				<dx:ASPxLabel ID="lbl_ts_pop_date" runat="server" Text="Unknown" 
					ClientInstanceName="lbl_ts_pop_date" Wrap="False">
				</dx:ASPxLabel>
			<td>
				<dx:ASPxLabel ID="lbl_ts_pop_type" runat="server" Text="Unknown" 
					ClientInstanceName="lbl_ts_pop_type" Wrap="False">
				</dx:ASPxLabel>
			</td>
			</tr>
		<tr>
			<td nowrap="nowrap">
				Start Time:
			</td>
			<td>
				<dx:ASPxTimeEdit ID="ts_pop_start" runat="server" ClientInstanceName="ts_pop_start" 
					Theme="NETheme01">
				</dx:ASPxTimeEdit></td>
		</tr>
		<tr>
			<td>
				End Time:
			</td>
			<td><dx:ASPxTimeEdit ID="ts_pop_end" runat="server" ClientInstanceName="ts_pop_end" 
					Theme="NETheme01">
				</dx:ASPxTimeEdit>
				</td>
		</tr>
		<tr>
			<td>
				Notes:</td>
			<td>
				<dx:ASPxMemo ID="ts_pop_notes" runat="server" Height="71px" 
					Width="170px">
				</dx:ASPxMemo>
			</td>
		</tr>
        <tr>
			<td>
				Add to Discipline File?:</td>
			<td>
                <dx:ASPxCheckBox ID="chk_add_to_disc_file" runat="server" ClientInstanceName="chk_add_to_disc_file"></dx:ASPxCheckBox>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
			<td>
				<dx:ASPxButton ID="btn_save_time" runat="server" AutoPostBack="False" 
					Text="Save">
					<ClientSideEvents Click="function(s, e) {
if (ts_pop_end.GetDate()&lt;=ts_pop_start.GetDate())
{
alert('The end date must be after the start date');
}
else
{
	pop_ts.PerformCallback('save|'+ pop_ts.cp_typeid);
	}
}" />
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
				</dx:PopupControlContentControl>
</ContentCollection>
		</dx:ASPxPopupControl>
		<br />
</p>
    <dx:ASPxPopupControl ID="pop_email" runat="server" 
	ClientInstanceName="pop_email" HeaderText="Email Schedule" Height="300px" 
	onwindowcallback="pop_email_WindowCallback" PopupHorizontalAlign="WindowCenter" 
	PopupVerticalAlign="WindowCenter" 
	style="font-family: Arial, Helvetica, sans-serif" 
	Width="800px">
		<ClientSideEvents PopUp="function(s, e) {
	pop_email.PerformCallback();
}" />
		<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<table style="width: 100%; font-family: Arial;">
		<tr>
			<td nowrap="nowrap">
				To (separate with ; ):</td>
			<td width="100%">
				<dx:ASPxTextBox ID="txt_email_to" runat="server" 
					Theme="NETheme01" Width="100%">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td>
				CC:</td>
			<td>
				<dx:ASPxTextBox ID="txt_email_cc" runat="server" Width="100%">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td>
				Subject:</td>
			<td>
				<dx:ASPxTextBox ID="txt_email_subject" runat="server" 
					Width="100%">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td valign="top">
				Message:</td>
			<td>
				<dx:ASPxHtmlEditor ID="mem_email_message" runat="server" 
					ClientInstanceName="mem_email_message" Height="200px" 
					Width="100%" Font-Names="Arial">
					<Settings AllowPreview="False" AllowContextMenu="False" AllowHtmlView="False" />
				</dx:ASPxHtmlEditor>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxLabel ID="lbl_pop_id" runat="server" ClientInstanceName="lbl_pop_id" 
					ForeColor="Transparent">
				</dx:ASPxLabel>
				</td>
			<td align="right">
				<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Send" 
					Theme="NETheme01" ClientInstanceName="btn_send_email">
					<ClientSideEvents Click="function(s, e) {
	go_email();
}" />
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
			</dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>
    