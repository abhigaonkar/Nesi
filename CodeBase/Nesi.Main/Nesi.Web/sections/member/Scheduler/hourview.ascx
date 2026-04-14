<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_scheduler_hourview" Codebehind="hourview.ascx.cs" %>
<%@ Register tagPrefix="dx" namespace="DevExpress.Web" assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<%@ Register tagPrefix="dx" namespace="DevExpress.Web.ASPxScheduler" assembly="DevExpress.Web.ASPxScheduler.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>



<%@ Register assembly="DevExpress.XtraScheduler.v19.2.Core, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>

<%@ Register src="inlineappform.ascx" tagname="inlineappform" tagprefix="uc1" %>


    
    <script type="text/javascript" src="../../../js/draganddrop/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../../../js/draganddrop/jquery-ui-1.8.23.custom.min.js"></script>
    
    <script type="text/javascript" language="javascript">
    	function InitalizejQuery(s, e) {
    		$('.draggable1').draggable({ helper: 'clone', appendTo: 'body', zIndex: 100 });
    		$('.droppable1').droppable({
    			activeClass: "dropTargetActive",
    			hoverClass: "dropTargetHover",

    			drop: function (ev, ui) {
    				// Make a clone of the dragged item
    				var clone = (ui.draggable).clone();

    				// Get a row index:
    				row = $(clone).find("input[type='hidden']").val();
    				hf1.Set('row', row);

    				// Calculate an active time cell
    				var cell = scheduler1.CalcHitTest(ev).cell;

    				// Initiate a scheduler callback to create an appointment based on a cell interval
    				if (cell != null) {
    					
    					scheduler1.getCellInfoProvider().initializeCell(cell);

    					hf1.Set('res', cell.resource);
    					scheduler1.RaiseCallback('CRTAPT|' + ASPx.DateUtils.GetInvariantDateTimeString(cell.interval.start));
    				}
    				else
    					alert('Drop the dragged item on a specific time cell.');

    				// Additional logic goes here...
    			}
    		}
             );
    	}
    </script>
    
    <style type="text/css">
        .dropTargetActive {
            border: solid 5px red;
        }
        .dropTargetHover {
            border: solid 5px yellow;
        }
    </style>

    <div>
        <table style="font-family: Arial, Helvetica, sans-serif">
            <tr>
                <td>
                    <div class="droppable1">
                        <table style="width:100%;">
							<tr>
								<td valign="top">
                        <dx:ASPxScheduler runat="server" ID="Scheduler1" ClientInstanceName="scheduler1" GroupType="Resource"
                            AppointmentDataSourceID="AppointmentDataSource1" ResourceDataSourceID="ResourceDataSource1" 
                            Start="2014-07-24" ActiveViewType="Timeline" 
							onappointmentrowinserted="Scheduler_AppointmentRowInserted" 
							onappointmentrowinserting="Scheduler_AppointmentRowInserting" 
							onappointmentsinserted="Scheduler_AppointmentsInserted" 
							onbeforeexecutecallbackcommand="ASPxScheduler1_BeforeExecuteCallbackCommand" ClientIDMode="AutoID" 
										Font-Names="Arial" Width="1050px">
                           <Views>
                                <WorkWeekView ShowFullWeek="true" Enabled="False" >
                            		<TimeRulers>
<cc1:TimeRuler></cc1:TimeRuler>
</TimeRulers>
								</WorkWeekView>

<DayView Enabled="False" ShowWorkTimeOnly="True"><TimeRulers>
<cc1:TimeRuler></cc1:TimeRuler>
</TimeRulers>
</DayView>
                            	<WeekView Enabled="False">
								</WeekView>
								<MonthView Enabled="False">
								</MonthView>
                            	<TimelineView IntervalCount="31" ShowMoreButtons="False">
									<Scales>
										<cc1:TimeScaleYear Enabled="False" />
										<cc1:TimeScaleQuarter Enabled="False" />
										<cc1:TimeScaleMonth Enabled="False" />
										<cc1:TimeScaleWeek Enabled="False" Visible="False" />
										<cc1:TimeScaleDay />
										<cc1:TimeScaleHour Enabled="False" DisplayFormat="H:mm" Width="25" />
									</Scales>
									<WorkTime End="17:00:00" Start="07:30:00" />
									<TimelineViewStyles>
										<TimelineCellBody>
											<Border BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" />
											<BorderTop BorderStyle="None" BorderWidth="1px" />
											<BorderBottom BorderStyle="None" BorderWidth="1px" />
										</TimelineCellBody>
										<TimelineDateHeader Font-Size="8pt">
											<Paddings Padding="0px" PaddingBottom="2px" />
										</TimelineDateHeader>
										<GroupSeparatorHorizontal>
											<Border BorderStyle="None" />
										</GroupSeparatorHorizontal>
										<GroupSeparatorVertical>
											<Border BorderStyle="None" />
										</GroupSeparatorVertical>
                           
                        
                     
									</TimelineViewStyles>
									<Templates>
										<HorizontalAppointmentTemplate>
											<uc1:inlineappform ID="inlineappform1" runat="server" />
										</HorizontalAppointmentTemplate>
									</Templates>
									<AppointmentDisplayOptions AppointmentAutoHeight="True" 
										StartTimeVisibility="Always" />
									
                        <TimelineViewStyles>
                            <TimelineCellBody Height="20px" />
                        </TimelineViewStyles>
                        <CellAutoHeightOptions Mode="FitToContent" />
                 
								</TimelineView>
                            </Views>
                            <Storage EnableReminders="False">
                                <Appointments AutoRetrieveId="True">
									<CustomFieldMappings>
										<dx:ASPxAppointmentCustomFieldMapping Member="woprog_id" Name="woprog_id" 
											ValueType="String" />
										<dx:ASPxAppointmentCustomFieldMapping Member="business_unit_id" Name="business_unit_id" 
											ValueType="String" />
										<dx:ASPxAppointmentCustomFieldMapping Member="quote_id" Name="quote_id" 
											ValueType="String" />
									</CustomFieldMappings>
									<Mappings AppointmentId="ID" End="EndDate" ResourceId="ResourceId" 
										Start="StartDate" Subject="Subject" Location="Location" Description="Description" Status="Status">
									</Mappings>
                                </Appointments>
                                <Resources>
									<Mappings ResourceId="Member_ID" Caption="member_fullname"></Mappings>
                                </Resources>
                            </Storage>
                            <OptionsBehavior RecurrentAppointmentEditAction="Ask" />
                            <OptionsToolTips ShowAppointmentToolTip="False" />
                            <OptionsView NavigationButtons-Visibility="Never" />
                            <ResourceNavigator Visibility="Never" />
							<Styles>
								<DayHeader Font-Size="8pt">
								</DayHeader>
							</Styles>
							<Templates>
                <VerticalResourceHeaderTemplate>
                    <table style="margin: 0px auto;">
                        <tr>
                            <td style="padding: 2px; white-space: nowrap;">
                                <%# Container.Resource.Caption %>
                            </td>
                        </tr>
                    </table>
                </VerticalResourceHeaderTemplate>
            </Templates>
                        </dx:ASPxScheduler>
                    			</td>
								<td valign="top">
                    				&nbsp;</td>
							</tr>
							<tr>
								<td>
                    <dx:ASPxGridView ID="gvwos1" runat="server" DataSourceID="gridDS1" AutoGenerateColumns="False" ClientInstanceName="grid1"
                        Width="600px" KeyFieldName="ID" Font-Names="Arial" Font-Size="8pt">
                        <Columns>
                            <dx:GridViewCommandColumn Visible="False" VisibleIndex="0">
								
							</dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="woprog_id" ReadOnly="True" 
								Visible="False" VisibleIndex="2" Width="35px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="woprog_bvwo" VisibleIndex="3" 
								Caption="WO" Width="80px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Customer" FieldName="woprog_customername" 
								VisibleIndex="4" Width="70px">
								<CellStyle Wrap="False">
								</CellStyle>
							</dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="woprog_description" VisibleIndex="5" 
								Caption="Description" Width="100%">
                            	<CellStyle Wrap="False">
								</CellStyle>
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataHyperLinkColumn Caption=" " ReadOnly="True" VisibleIndex="1" 
								Width="25px">
                                <Settings AllowSort="False"></Settings>
                                <EditFormSettings Visible="False" />
                                <DataItemTemplate>
                                    <div class="draggable1">
                                        <a href="#" title="Image Viewer">
                                            <img src="../../../images/icon/icon[drag].png" alt="" />
                                        </a>
                                        <input type="hidden" value='<%# Container.VisibleIndex %>' />
                                    </div>
                                </DataItemTemplate>
                            </dx:GridViewDataHyperLinkColumn>
                            <dx:GridViewDataTextColumn FieldName="TypeID" Visible="False">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="address" FieldName="woprog_address_id" 
								VisibleIndex="6" Width="0px">
							</dx:GridViewDataTextColumn>
                        	<dx:GridViewDataTextColumn Caption="company" FieldName="business_unit_id" 
								Visible="False" VisibleIndex="7">
							</dx:GridViewDataTextColumn>
                        </Columns>
                    	<SettingsBehavior ColumnResizeMode="Control" />
						<SettingsPager PageSize="50" Visible="False">
						</SettingsPager>
						<Settings ShowFilterRow="True" />
                    </dx:ASPxGridView>
                				</td>
								<td>
									&nbsp;</td>
							</tr>
						</table>
                    </div>
                </td>
            </tr>
        </table>
         <asp:SqlDataSource ID="ResourceDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
             
			
			
			
			SelectCommand="SELECT Member_ID, member_fullname FROM member where business_unit_id = ?cid and member_status = 'Active' and member_membertype_id in(2,4,11,12,13,17,18,19,20,21,23,24,25,26,31,33) order by member_fullname">
			 <SelectParameters>
				 <asp:ControlParameter ControlID="hdn_cid" Name="cid" PropertyName="Value" />
			 </SelectParameters>
        </asp:SqlDataSource>
        <dx:ASPxHiddenField ID="hf1" runat="server" ClientInstanceName="hf1" 
			SyncWithServer="true">
        </dx:ASPxHiddenField>
        
        <dx:ASPxGlobalEvents ID="ASPxGlobalEvents2" runat="server">
            <ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" />
        </dx:ASPxGlobalEvents>
       
        <asp:SqlDataSource ID="gridDS1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
            
			
			SelectCommand="SELECT woprog_id,woprog_bvwo,woprog_customername,business_unit_id,woprog_description,woprog_address_id  FROM woprog where woprog_status = 'Open' and business_unit_id = ?cid order by woprog_bvwo">
			<SelectParameters>
				<asp:ControlParameter ControlID="hdn_cid" Name="cid" PropertyName="Value" />
			</SelectParameters>
		</asp:SqlDataSource>
            
        <asp:HiddenField ID="hdn_cid" runat="server" />
            
        <asp:SqlDataSource ID="AppointmentDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
			InsertCommand="INSERT INTO [Appointments] ([EventType], [StartDate], [EndDate], [AllDay], [Subject], [Location], [Description], [Status], [Label], [ResourceID], [RecurrenceInfo], [ReminderInfo], [ContactInfo],[woprog_id],[quote_id],[business_unit_id]) VALUES (@EventType, @StartDate, @EndDate, @AllDay, @Subject, @Location, @Description, @Status, @Label, @ResourceID, @RecurrenceInfo, @ReminderInfo, @ContactInfo,@woprog_id,@quote_id,@cid)"
			UpdateCommand="UPDATE [Appointments] SET [EventType] = @EventType, [StartDate] = @StartDate, [EndDate] = @EndDate, [AllDay] = @AllDay, [Subject] = @Subject, [Location] = @Location, [Description] = @Description, [Status] = @Status, [Label] = @Label, [ResourceID] = @ResourceID, [RecurrenceInfo] = @RecurrenceInfo, [ReminderInfo] = @ReminderInfo, [ContactInfo] = @ContactInfo, [woprog_id]=@woprog_id WHERE [ID] = @ID"
			DeleteCommand="Delete from appointments where id = @id"
			SelectCommand="SELECT * FROM appointments"
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
				<asp:Parameter Name="cid" Type="String" />
				<asp:Parameter Name="quote_id" Type="String" />
            </InsertParameters>
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
                <asp:Parameter Name="ID" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
        
       
    </div>
    