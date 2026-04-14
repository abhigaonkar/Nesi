<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="member_vacation_viewer" Title="Vacation Viewer" Codebehind="vacation_viewer.aspx.cs" %>


<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>



<%@ Register assembly="DevExpress.Web.ASPxScheduler.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxScheduler" tagprefix="dxwschs" %>
<%@ Register assembly="DevExpress.XtraScheduler.v19.2.Core, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select id, name from business_unit  where active = 'T' and id in (get_visible_business_units_group_concat(?mid))">
	    <SelectParameters>
	        
	        <asp:ControlParameter ControlID="hdn_mid" Name="mid" PropertyName="Value" />
	    </SelectParameters>
    
    </asp:SqlDataSource>
	
	<table style="width:100%;">
		<tr>
			<td>
	<dx:ASPxComboBox ID="ddlbranch" runat="server" AutoPostBack="True" 
		 TextField="name" ValueField="id" 
		ValueType="System.Int32" Theme="NETheme01">
	</dx:ASPxComboBox>
	
			</td>
			<td nowrap="nowrap" width="30%">
				<dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" AutoPostBack="True" 
					Text="Show All Employees" Wrap="False">
				</dx:ASPxCheckBox>
			</td>
			<td width="100%">
				&nbsp;</td>
		</tr>
	</table>
	<dxwschs:ASPxScheduler ID="cal" runat="server" 
		AppointmentDataSourceID="sql_vacations" ClientIDMode="AutoID" 
		ClientInstanceName="cal" ResourceDataSourceID="sqlmember" Start="2013-12-01" 
		ActiveViewType="Timeline" Font-Names="Arial" GroupType="Resource">
		<Storage>
			<Appointments AutoRetrieveId="True">
				<Mappings AppointmentId="vacation_id" End="date_return" ResourceId="member_id" 
					Start="date_start" Subject="status" />
				<CustomFieldMappings>
					<dxwschs:ASPxAppointmentCustomFieldMapping Member="STATUS" Name="STATUS" />
				</CustomFieldMappings>
			</Appointments>
			<Resources>
				<Mappings Caption="_name" ResourceId="member_id" />
			</Resources>
		</Storage>
		<Views>
			<DayView Enabled="False">
				<TimeRulers>
					<cc1:TimeRuler>
					</cc1:TimeRuler>
				</TimeRulers>
			</DayView>
			<WorkWeekView Enabled="False">
				<TimeRulers>
					<cc1:TimeRuler>
					</cc1:TimeRuler>
				</TimeRulers>
			</WorkWeekView>
			<WeekView Enabled="False">
			</WeekView>
			<MonthView Enabled="False">
			</MonthView>
			<TimelineView IntervalCount="60" ResourcesPerPage="100" ShowMoreButtons="False">
				<Scales>
					<cc1:TimeScaleYear DisplayFormat="yyyy-MM-dd" Enabled="False" Visible="False" />
					<cc1:TimeScaleQuarter Enabled="False" Visible="False" />
					<cc1:TimeScaleMonth DisplayFormat="MMMM-yyyy" />
					<cc1:TimeScaleWeek DisplayFormat="MMMM dd" />
					<cc1:TimeScaleDay DisplayFormat="dd" Width="5" />
					<cc1:TimeScaleHour Enabled="False" Visible="False" />
					<cc1:TimeScale15Minutes Enabled="False" Visible="False" />
				</Scales>
				<TimelineViewStyles>
					<TimelineCellBody Height="25px">
					</TimelineCellBody>
					<VerticalResourceHeader BackColor="#FFFFCC" Width="200px">
					</VerticalResourceHeader>
				</TimelineViewStyles>
				<Templates>
					<TimelineDateHeaderTemplate>
						<%# GetDisplayText(Container) %>
					</TimelineDateHeaderTemplate>
				</Templates>
				<AppointmentDisplayOptions AppointmentAutoHeight="True" 
					EndTimeVisibility="Never" StartTimeVisibility="Never" TimeDisplayType="Text" />
				<CellAutoHeightOptions Mode="FitToContent" />
			</TimelineView>
		</Views>
		<Styles>
			<NavigationButton>
				<Paddings PaddingBottom="0px" PaddingTop="0px" />
			</NavigationButton>
			<VerticalResourceHeader Width="250px" BackColor="#FFCCCC">
			</VerticalResourceHeader>
			<Appointment Wrap="False">
			</Appointment>
			<LeftTopCorner>
				<Paddings PaddingRight="150px" />
			</LeftTopCorner>
		</Styles>
		<OptionsCustomization AllowAppointmentCopy="None" AllowAppointmentCreate="None" 
			AllowAppointmentDelete="None" AllowAppointmentDrag="None" 
			AllowAppointmentDragBetweenResources="None" AllowAppointmentEdit="None" 
			AllowAppointmentMultiSelect="False" AllowAppointmentResize="None" />
		<OptionsView NavigationButtons-NextCaption="Next Period" 
			NavigationButtons-PrevCaption="Prev Period" />
		<ResourceNavigator Visibility="Always" />
	</dxwschs:ASPxScheduler>
	<asp:SqlDataSource ID="sql_vacations" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="SELECT
a.vacation_id,
a.date_start,
a.date_end,
a.member_id,
b.`status`,
concat('Vacation - ', c.member_fullname) AS `SUBJECT`,
c.business_unit_id,
c.member_fullname AS _name
FROM
vacation_master a
INNER JOIN vacation_status b ON a.`status` = b.status_id
INNER JOIN member c ON a.member_id = c.Member_ID
WHERE
          a.date_start &gt; (
                    curdate() + interval - 30 DAY
          ) and c.business_unit_id in (get_visible_business_units_group_concat(?mid))  AND a.type_id = 4">
		<SelectParameters>
			<asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
		    <asp:ControlParameter ControlID="hdn_mid" Name="mid" PropertyName="Value" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:HiddenField ID="hdn_mid" runat="server" />
	<asp:SqlDataSource ID="sqlmember" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="SELECT
a.member_id,
b.member_fullname AS _name
FROM
vacation_master a
INNER JOIN member b ON a.member_id = b.Member_ID

where
a.date_start &gt; (curdate() + interval - 30 DAY ) AND
b.business_unit_id = ?cid 
GROUP BY a.member_id
order by b.member_fullname ">
		<SelectParameters>
			<asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
		
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sqlmember0" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="SELECT member.member_id,
member_fullname AS _name
FROM member 
where
member_status='Active' AND
business_unit_id = ?cid
order by member_fullname ">
		<SelectParameters>
			<asp:ControlParameter ControlID="ddlbranch" Name="cid" PropertyName="Value" />
			
		</SelectParameters>
	</asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">

	<script type="text/javascript" language="javascript">
		function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 20) + 'px';
 }
	
 </script>

</asp:Content>

