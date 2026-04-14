<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_business_unit_fvr_settings" Codebehind="business_unit_fvr_settings.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<table>
	<tr>
		<td>Default Onboarding FVR packet:</td>
		<td></td>
	</tr>
	<tr>
		<td>Welcome Email Template:</td>
		<td></td>
	</tr>
	<tr>
		<td colspan="2">Schedule</td>
	</tr>
	<tr>
		<td colspan="2">
			<dxwschs:ASPxScheduler ID=ASPxScheduler1 ActiveViewType="Month" runat="server">
				<Views>
					<MonthView Enabled="true"></MonthView>
					<DayView Enabled="True"></DayView>
					<TimelineView Enabled="False"></TimelineView>
					<WeekView Enabled="false"></WeekView>
					<FullWeekView Enabled="false"></FullWeekView>
				</Views>

			</dxwschs:ASPxScheduler>
		</td>
	</tr>
</table>	