<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_scheduler_single_member_view" Codebehind="single_member_view.ascx.cs" %>
<%@ Register tagPrefix="dx" namespace="DevExpress.Web" assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<%@ Register tagPrefix="dx" namespace="DevExpress.Web.ASPxScheduler" assembly="DevExpress.Web.ASPxScheduler.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>



<%@ Register assembly="DevExpress.XtraScheduler.v19.2.Core, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>
<%@ Register src="inlineappform.ascx" tagname="inlineappform" tagprefix="uc1" %>

<%@ Register src="hourview.ascx" tagname="hourview" tagprefix="uc2" %>

<%@ Register src="overviewform.ascx" tagname="overviewform" tagprefix="uc3" %>

<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>

 <style type="text/css">
        .dropTargetActive {
            border-color: #ff0000;
        }
        .dropTargetHover {
           border-color: #ff0000;
        }
       
		#draggable3 {width: 500px; height: 500px; padding: 0em; padding-top: 0em; z-index:200; }
		
	
     	 .TimeMarkerClass
        {
           position: absolute;
        background-color: Red;
        border: 1px solid #9da0aa;
        width: 1px;
        font-size: 1px;
        border-top-width: 0;
        border-bottom-width: 0; 
        }
	
     </style>
<script type="text/javascript" language="javascript">

	function recurrenceDateChanged(s, e) {
		var selectedWeekends = [];
		var selectedDates = s.GetSelectedDates();
		for (i = 0; i < selectedDates.length; i++) {
			if (selectedDates[i].getDay() == 6 || selectedDates[i].getDay() == 0) {
				selectedWeekends[selectedWeekends.length] = selectedDates[i];
			}
		}
		for (i = 0; i < selectedWeekends.length; i++) {
			s.DeselectDate(selectedWeekends[i]);
		}
	}


	function DefaultViewMenuHandler(single_view, s, e) {
		if (e.item.GetItemCount() <= 0) {
			if (e.item.name == "GotoDate") {
				single_view.RaiseCallback('MNUVIEW|GotoDate')
			}
			if (e.item.name == "GotoToday") {
				single_view.RaiseCallback('MNUVIEW|GotoToday')
			}
		}
		

		if (e.item.name == "1") {
			single_view.RaiseCallback("MYAPTMENU|" + e.item.name);
		}
		else if (e.item.name == "5") {
			single_view.RaiseCallback("MYAPTMENU|" + e.item.name);
		}
		else if (e.item.name == "6") {
			single_view.RaiseCallback("MYAPTMENU|" + e.item.name);
		}
		else if (e.item.name == "2") {
			single_view.RaiseCallback("MYAPTMENU|" + e.item.name);
		}
		else if (e.item.name == "3") {
			single_view.RaiseCallback("MYAPTMENU|" + e.item.name);
		}
	
		else if (e.item.name == "7") {
			single_view.RaiseCallback("MYAPTMENU|" + e.item.name);
		}
		if (e.item.name == "_open") 
		{
			var aptid = single_view.GetSelectedAppointmentIds()[0];


			var apt = single_view.GetAppointmentById(single_view.GetSelectedAppointmentIds()[0]);
			var statusid = apt.GetStatusId();

			if ((statusid > 60)||(statusid==0)) {
				pop_edit.cp_aptid = single_view.GetSelectedAppointmentIds()[0]; pop_edit.Show(); pop_edit.PerformCallback(aptid);
			}
			else {
				pop_event.Show(); pop_event.PerformCallback(pop_edit.cp_aptid);
			}
		}
		if (e.item.name == "DeleteAppointment") {

			single_view.RaiseCallback("DELETE");
		}
		if (e.item.name == "OpenAppointment") {
			single_view.RaiseCallback("MYAPTMENU|" + e.item.name);
		}
		if (e.item.name == "addrecurrence") {
			hf.Set('res', scheduler.GetSelectedAppointmentIds()[0]);
			pop_recurrence.Show();
			pop_recurrence.PerformCallback(single_view.GetSelectedAppointmentIds()[0]);
		}
		if (e.item.name == "confirm") {

			single_view.RaiseCallback("CONFIRM");
		}
		if (e.item.name == "MEET_AT_SHOP") {
			single_view.RaiseCallback("MEET_AT_SHOP");
		}
		if (e.item.name == "MEET_ON_SITE") {
			single_view.RaiseCallback("MEET_ON_SITE");
		}
		if (e.item.name == "addevent") {

			pop_event.Show(); pop_event.PerformCallback();
		}
		if (e.item.name == "email") {

			pop_email.Show();
			pop_email.PerformCallback(single_view.GetSelectedAppointmentIds()[0]);
		}
	}

	function appointmentMenu_PopUp(s, e) {

		var openItem = e.item.GetItemByName("addevent");
		
		var openItem2 = e.item.GetItemByName("1");
		var openItem3 = e.item.GetItemByName("2");
		var openItem4 = e.item.GetItemByName("3");
		var openItem5 = e.item.GetItemByName("5");
		var openItem6 = e.item.GetItemByName("6");
		var openItem7 = e.item.GetItemByName("7");

		if (openItem != null) {
			openItem.SetVisible(true);
			if (single_view.GetSelectedResource() > 100000000) {
				openItem.SetVisible(false);
			}
		}
	
		if (openItem2 != null) {
			openItem2.SetVisible(true);
			if (single_view.GetSelectedResource() > 100000000) {
				openItem2.SetVisible(false);
			}
		}
		if (openItem3 != null) {
			openItem3.SetVisible(true);
			if (single_view.GetSelectedResource() > 100000000) {
				openItem3.SetVisible(false);
			}
		}
		if (openItem4 != null) {
			openItem4.SetVisible(true);
			if (single_view.GetSelectedResource() > 100000000) {
				openItem4.SetVisible(false);
			}
		}
		if (openItem5 != null) {
			openItem5.SetVisible(true);
			if (single_view.GetSelectedResource() > 100000000) {
				openItem5.SetVisible(false);
			}
		}
		if (openItem6 != null) {
			openItem6.SetVisible(true);
			if (single_view.GetSelectedResource() > 100000000) {
				openItem6.SetVisible(false);
			}
		}
		if (openItem7 != null) {
			openItem7.SetVisible(true);
			if (single_view.GetSelectedResource() > 100000000) {
				openItem7.SetVisible(false);
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
    						var cell = single_view.CalcHitTest(ev).cell;

    						// Initiate a scheduler callback to create an appointment based on a cell interval

    						if (cell != null) {

    				//			var drop_date = cell.interval.start;
    				//			var apt_id = cell.id;
    				//			var isok = PageMethods.Check_wo_date(drop_date,apt_id)
    							single_view.getCellInfoProvider().initializeCell(cell);

    							hf.Set('res', cell.resource);
    							single_view.RaiseCallback('CRTAPT|' + ASPx.DateUtils.GetInvariantDateTimeString(cell.interval.start));
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
						onselectedindexchanged="ddlbranch_SelectedIndexChanged" Theme="NETheme01" style="margin-bottom: 0px">
					</dx:ASPxComboBox>
					<asp:SqlDataSource ID="sqlbranch" runat="server" 
						ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
						ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
						SelectCommand="Select id business_unit_id,name from business_unit  where active = 'T' order by name ">
					</asp:SqlDataSource>
                			</td>
							<td>
                    			&nbsp;</td>
							<td width="100%">
								&nbsp;</td>
						</tr>
						<tr>
							<td>
                    <dx:ASPxComboBox ID="ddlpm" runat="server" ClientInstanceName="ddlpm" 
						DataSourceID="sqlpm" TextField="_name" 
						ValueField="_id" ValueType="System.Int32" AutoPostBack="True" Font-Names="Arial" 
						Theme="NETheme01" onselectedindexchanged="ddlpm_SelectedIndexChanged">
					</dx:ASPxComboBox>
                			</td>
							<td>
								&nbsp;</td>
							<td>
								&nbsp;</td>
						</tr>
						<tr>
							<td>
								<table style="width:100%;">
									<tr>
										<td align="center" bgcolor="#006600" 
											style="border: 1px solid #000000; color: #CCFFCC;">
											Confirmed</td>
									</tr>
									<tr>
										<td align="center" bgcolor="#31CE31" 
											style="border: 1px solid #000000; color: #003300;">
											Vacation or Day Off</td>
									</tr>
									<tr>
										<td align="center" style="border: 1px solid #000000; color: #006600;">
											Tentative</td>
									</tr>
									<tr>
										<td align="center" 
											style="border: 1px solid #000000; background-color: #FFFF00;">
											Event</td>
									</tr>
									<tr>
										<td align="center" 
											style="border: 1px solid #000000; vertical-align: top;">
					<asp:Image runat="server" ImageUrl="~/images/IconsButtons/32px-Crystal_Clear_action_gohome.png" 
												Height="25px" ID="Image1"></asp:Image>

				&nbsp;- Meet on Site</td>
									</tr>
									<tr>
										<td align="center" 
											style="border: 1px solid #000000; vertical-align: top; color: #FFFFFF;" bgcolor="Red">
											 </asp:Image>

				&nbsp;On Call</td>
									</tr>
								</table>
							</td>
							<td>
								&nbsp;</td>
							<td>
									<asp:SqlDataSource ID="sqlpm" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										
										
										
										SelectCommand="Select distinct (member_id) _id, member_fullname _name from member where business_unit_id = ?cid and member_status ='Active' order by member_fullname">
										<SelectParameters>
											<asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
										</SelectParameters>
									</asp:SqlDataSource>

       							 
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
								 <div class="droppable" style="border: 5px solid #FFFFFF;">
                        <dx:ASPxScheduler runat="server" ID="single_view" ClientInstanceName="single_view" GroupType="Resource"
                            AppointmentDataSourceID="AppointmentDataSource" ResourceDataSourceID="ResourceDataSourcex" 
                            Start="2014-09-29" ActiveViewType="Month" 
							onappointmentrowinserted="Scheduler_AppointmentRowInserted" 
							onappointmentrowinserting="Scheduler_AppointmentRowInserting" 
							onappointmentsinserted="Scheduler_AppointmentsInserted" 
							onbeforeexecutecallbackcommand="ASPxScheduler1_BeforeExecuteCallbackCommand" ClientIDMode="AutoID" 
							Font-Names="Arial" Width="100%" onpopupmenushowing="Scheduler_PopupMenuShowing" 
							onprepareappointmentformpopupcontainer="Scheduler_PrepareAppointmentFormPopupContainer" 			
							oninitclientappointment="Scheduler_InitClientAppointment"
							onappointmentchanging="Scheduler_AppointmentChanging" 
							
							onappointmentformshowing="Scheduler_AppointmentFormShowing1" 
							oncustomjsproperties="Scheduler_CustomJSProperties" onappointmentdeleting="Scheduler_AppointmentDeleting" 
										 oncustomcallback="Scheduler_CustomCallback" Theme="Office2010Blue" onhtmltimecellprepared="single_view_HtmlTimeCellPrepared" 		
							
										>
                           <Views>
                                <WorkWeekView ShowFullWeek="true" >
                            		<TimeRulers>
<cc1:TimeRuler></cc1:TimeRuler>
</TimeRulers>
								</WorkWeekView>

<DayView ShowWorkTimeOnly="True"><TimeRulers>
<cc1:TimeRuler></cc1:TimeRuler>
</TimeRulers>
</DayView>
                            	<MonthView>
									<Templates>
										<HorizontalSameDayAppointmentTemplate>
											<uc1:inlineappform ID="inlineappform1" runat="server" />
										</HorizontalSameDayAppointmentTemplate>
									</Templates>
									<CellAutoHeightOptions Mode="FitToContent" />
								</MonthView>
											 <AgendaView Enabled="false"></AgendaView>
                            	<TimelineView IntervalCount="14" ShowMoreButtons="False" ResourcesPerPage="150" 
									Enabled="False">
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
											<BorderBottom BorderStyle="None"/>
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
										<VerticalResourceHeader BorderBottom-BorderStyle="None">
											<Paddings Padding="0px" />

<Paddings Padding="0px"></Paddings>

<BorderBottom BorderStyle="None"></BorderBottom>
										</VerticalResourceHeader>
										<GroupSeparatorHorizontal>
											<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
										</GroupSeparatorHorizontal>
										<GroupSeparatorVertical>
											<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
										</GroupSeparatorVertical>
                           
                        
                     
									</TimelineViewStyles>
									<Templates>
										<TimelineDateHeaderTemplate>
										<table width="100%" align="center"><tr><td align="center">
											<dx:ASPxButton ID="btn_date_header" runat="server" AutoPostBack="False" 
												Text= '<%# Container.Interval.Start.DayOfWeek.ToString().Remove(3) + " " + Container.Interval.Start.Day %>' 
												UseSubmitBehavior="False"
												ClientSideEvents-Click='<%# GetClickHandler(Container.Interval.Start) %>'
												 Width="50px" Border-BorderStyle="None" ForeColor="Gray" ClientEnabled="False">												
											</dx:ASPxButton>
											</td></tr><tr><td><asp:Label ID="Label1" runat="server" Text="" Font-Size="XX-Small" /></td></tr></table>
										</TimelineDateHeaderTemplate>
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
											<dx:ASPxAppointmentCustomFieldMapping Member="setby" Name="setby" 
											ValueType="String" />
											<dx:ASPxAppointmentCustomFieldMapping Member="scheduled_by" Name="scheduled_by" 
											ValueType="Integer" />
										<dx:ASPxAppointmentCustomFieldMapping Member="assetid" Name="assetid" 
											ValueType="String" />
										<dx:ASPxAppointmentCustomFieldMapping Member="truckno" Name="truckno" 
											ValueType="String" />
											<dx:ASPxAppointmentCustomFieldMapping Member="membertype_id" 
											Name="membertype_id" ValueType="String" />
											<dx:ASPxAppointmentCustomFieldMapping Member="confirmed" 
											Name="confirmed" ValueType="Boolean" />
											<dx:ASPxAppointmentCustomFieldMapping Member="notes" 
											Name="notes" ValueType="String" />
											<dx:ASPxAppointmentCustomFieldMapping Member="meet_at_shop" 
											Name="meet_at_shop" ValueType="Boolean" />
									</CustomFieldMappings>
									<Mappings AppointmentId="ID" End="EndDate" ResourceId="member_id" 
										Start="StartDate" Subject="Subject" Location="Location" Description="Description" Status="Status">
									</Mappings>
                                </Appointments>
                                <Resources>
									<Mappings ResourceId="Member_ID" Caption="member_fullname"></Mappings>
                                </Resources>
                            </Storage>
                            <OptionsCustomization AllowAppointmentResize="None" 
								AllowAppointmentConflicts="Custom" AllowAppointmentCopy="None" AllowAppointmentCreate="None" 
								AllowAppointmentDelete="None" AllowAppointmentDrag="None" 
								AllowAppointmentDragBetweenResources="None" AllowAppointmentEdit="None" />
                            <OptionsBehavior RecurrentAppointmentEditAction="Ask" 
								ShowRemindersForm="False" />
                           
							<ClientSideEvents Init="function(s, e) {
							
			   single_view.menuManager.UpdateAptSubMenu = function (menu, submenuName, subMenuItemIndex){
               var apt = single_view.GetAppointmentById(single_view.GetSelectedAppointmentIds()[0]); 
			   var statusid = apt.GetStatusId();
			   s.cpWarning = '';
				if (statusid == 99) 
				{
				menu.rootItem.items[0].SetVisible(false); 
				menu.rootItem.items[1].SetVisible(false); 
				menu.rootItem.items[2].SetVisible(false);
				menu.rootItem.items[3].SetVisible(false);
				menu.rootItem.items[4].SetVisible(false);
				menu.rootItem.items[5].SetVisible(false);
				menu.rootItem.items[6].SetVisible(false);
				menu.rootItem.items[7].SetVisible(false); 
				menu.rootItem.items[8].SetVisible(false); 
				}
				
				else if (statusid == 97)
				{
					menu.rootItem.items[0].SetVisible(true); 
					menu.rootItem.items[1].SetVisible(true); 
					menu.rootItem.items[2].SetVisible(true); 
					if (apt.member_id&lt;10000)
					{
					
						if ((apt.scheduled_by==apt.thisuser))
						{
							menu.rootItem.items[3].SetVisible(true);
						}
						else
{
menu.rootItem.items[3].SetVisible(false);
}
					}
					else
					{
						menu.rootItem.items[3].SetVisible(false);
					}
					menu.rootItem.items[4].SetVisible(false);
					menu.rootItem.items[5].SetVisible(false);
					menu.rootItem.items[6].SetVisible(false);
				}
				else if (statusid == 0)
				{
				menu.rootItem.items[0].SetVisible(true); 
				menu.rootItem.items[1].SetVisible(true); 
				menu.rootItem.items[2].SetVisible(true); 
				menu.rootItem.items[3].SetVisible(false);
				menu.rootItem.items[4].SetVisible(true);
				menu.rootItem.items[5].SetVisible(true);
				menu.rootItem.items[6].SetVisible(true);
				}
				else if (statusid &lt; 60)
				{
				menu.rootItem.items[0].SetVisible(true); 
				menu.rootItem.items[1].SetVisible(true); 
				menu.rootItem.items[2].SetVisible(false); 
				menu.rootItem.items[3].SetVisible(false);
				menu.rootItem.items[4].SetVisible(true);
				menu.rootItem.items[5].SetVisible(true);
				menu.rootItem.items[6].SetVisible(true);
				}
				else if (statusid &lt; 98)
				{
				
				menu.rootItem.items[0].SetVisible(true); 
				menu.rootItem.items[1].SetVisible(true); 
				menu.rootItem.items[2].SetVisible(true);
				menu.rootItem.items[3].SetVisible(true);
				menu.rootItem.items[4].SetVisible(true);
				menu.rootItem.items[5].SetVisible(true);
				menu.rootItem.items[6].SetVisible(true);
				menu.rootItem.items[7].SetVisible(true);
				menu.rootItem.items[8].SetVisible(false); 
				}  
				else if (statusid =104)
				{
						menu.rootItem.items[0].SetVisible(false); 
						menu.rootItem.items[1].SetVisible(false); 
						menu.rootItem.items[2].SetVisible(false); 
						menu.rootItem.items[3].SetVisible(false);
						menu.rootItem.items[4].SetVisible(false);
						menu.rootItem.items[5].SetVisible(false);
						menu.rootItem.items[6].SetVisible(false);
				}
				
				if (apt.member_id&gt;100000000)
				{
					if ((apt.setby!=apt.thisuser)&amp;&amp;(apt.can_edit_all=0))
					{
						menu.rootItem.items[1].SetVisible(false); 
						menu.rootItem.items[2].SetVisible(false); 
						menu.rootItem.items[3].SetVisible(false);
						menu.rootItem.items[4].SetVisible(false);
						menu.rootItem.items[5].SetVisible(false);
						menu.rootItem.items[6].SetVisible(false);
						menu.rootItem.items[7].SetVisible(false);
					}   
				}
				

			
			}
}
" EndCallback="function(s, e) {
                    if (s.cpWarning!='') {
                        alert(s.cpWarning);
                        s.cpWarning='';
                    }


}" AppointmentDrop="function(s, e) {
	
 single_view.cpOperation = e.operation;

}" AppointmentDeleting="function(s, e) {


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
                    <div style="padding: 0px 0px 0px 0px; background-color: <%# GetResourceColor(Container) %>; width: 100px; height: 45px; font-family: Arial, Helvetica, sans-serif; font-size: 9pt; vertical-align: middle;">
					    <table><tr><td><%# Container.Resource.Caption %></td></tr><tr><td><asp:Label ID="Label2" runat="server" Text='' Font-Size="XX-Small" /></td></tr></table>
                    </div>
                </VerticalResourceHeaderTemplate>
                
            					<HorizontalAppointmentTemplate>
									<uc1:inlineappform ID="inlineappform1" runat="server" />
								</HorizontalAppointmentTemplate>
                
            </Templates>

<OptionsCustomization AllowAppointmentResize="None"></OptionsCustomization>

<OptionsBehavior RecurrentAppointmentEditAction="Ask" ShowRemindersForm="False"></OptionsBehavior>

<OptionsToolTips AppointmentToolTipUrl="AppTooltip.ascx"></OptionsToolTips>

<OptionsView NavigationButtons-Visibility="Never" ></OptionsView>

<ResourceNavigator Visibility="Never"></ResourceNavigator>
                        </dx:ASPxScheduler>
						   </div>
                    			</td>
							</tr>
							</table>
                 
                </td>
            </tr>
        </table>

         <asp:SqlDataSource ID="ResourceDataSourcex" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
			SelectCommand="SELECT Member_ID, member_fullname FROM member where business_unit_id = ?cid and member_id = ?pid ORDER BY member_fullname">
			 <SelectParameters>
				 <asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
				 <asp:ControlParameter ControlID="ddlpm" Name="pid" PropertyName="Value" />
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
			InsertCommand="INSERT INTO [Appointments] ([EventType], [StartDate], [EndDate], [AllDay], [Subject], [Location], [Description], [Status], [Label], [ResourceID], [RecurrenceInfo], [ReminderInfo], [ContactInfo],[woprog_id],[quote_id],[business_unit_id],[setby],[assetid],[member_id],[membertype_id],[confirmed],[notes]) VALUES (@EventType, @StartDate, @EndDate, @AllDay, @Subject, @Location, @Description, @Status, @Label, @member_id, @RecurrenceInfo, @ReminderInfo, @ContactInfo,@woprog_id,@quote_id,@cid,@setby,@assetid,@member_id,@membertype_id,@confirmed,@notes)"
			UpdateCommand="UPDATE [Appointments] SET [EventType] = @EventType, [StartDate] = @StartDate, [EndDate] = @EndDate, [AllDay] = @AllDay, [Subject] = @Subject, [Location] = @Location, [Description] = @Description, [Status] = @Status, [Label] = @Label, [ResourceID] = @member_id, [RecurrenceInfo] = @RecurrenceInfo, [ReminderInfo] = @ReminderInfo, [ContactInfo] = @ContactInfo, [woprog_id]=@woprog_id, [setby]=@setby, [assetid]=@assetid,[member_id]=@member_id, [membertype_id]=@membertype_id,[confirmed]=@confirmed,[notes]=@notes WHERE [ID] = @ID"
			DeleteCommand="Delete from appointments where id = @id"
			SelectCommand="CALL scheduler_single_member_view(@mid, @cid)"
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
				<asp:Parameter Name="setby" Type="String" />
				<asp:Parameter Name="assetid" Type="String" />
				<asp:Parameter Name="member_id" Type="String" />
				<asp:Parameter Name="membertype_id" Type="String" />
				<asp:Parameter Name="confirmed" Type="Boolean" />
				<asp:Parameter Name="notes" Type="String" />
				<asp:Parameter Name="meet_at_shop" Type="Boolean" />
            </InsertParameters>
			
             <SelectParameters>
				 <asp:ControlParameter ControlID="ddlbranch" Name="@cid" PropertyName="Value" />
				  <asp:ControlParameter ControlID="ddlpm" Name="@mid" PropertyName="Value" />
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
            </UpdateParameters>
        </asp:SqlDataSource>
        
        
       
    </div>
    



    