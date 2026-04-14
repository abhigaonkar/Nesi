<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_hr_member_test" Codebehind="days_off.aspx.cs" %>

<%@ Register src="modules/days_off.ascx" tagname="days_off" tagprefix="uc" %>
<%@ Register src="modules/days_off_new.ascx" tagname="days_off_new" tagprefix="uc" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript">
		function CalculateHours(isEdit)
			{
			var hoursHtml = "";
			var targetTd = "";
			var valid = true;
			var startObj = null;
			var endObj = null;
			if(isEdit)
				{
				targetTd = ".total_hours_edit";
				startObj = edit_dte_start;
				endObj = edit_dte_end;
				}
			else
				{
				targetTd = ".total_hours_new";
				startObj = new_dte_start;
				endObj = new_dte_end;
				}
			if(endObj.GetDate() != null)
				{
				startObj.SetMaxDate(endObj.GetDate());
				}
			endObj.SetMinDate(startObj.GetDate());
			if(endObj.GetDate() == null || startObj.GetDate() == null)
				{
				hoursHtml = "Both dates not selected?";
				valid = false;
				}

			if(valid)
				{
				var origDiff =(endObj.GetDate().getTime() - startObj.GetDate().getTime()) / 1000;
				var diff = origDiff / (60 * 60); // 60 seconds in a minute, 60 minutes in an hour
                var hours = Math.abs(diff);
				var days = hours > 24
								? weekdaysBetween(startObj.GetDate(), endObj.GetDate())+1
								: 1;
				hoursHtml = hours < 24
								? hours.toFixed(2) + " hours"
								: days+" weekdays @ 8 hours per";
				}
            $(targetTd).html(hoursHtml);
			}
		function weekdaysBetween(startDate, endDate) {
			if (startDate < endDate) {
				var s = startDate;
				var e = endDate;
				} else {
				var s = endDate;
				var e = startDate;
				}
			var diffDays = Math.floor((e - s) / 86400000);
			var weeksBetween = Math.floor(diffDays / 7);
			if (s.getDay() == e.getDay()) {
				var adjust = 0;
				} else if (s.getDay() == 0 && e.getDay() == 6) {
				var adjust = 5;
				} else if (s.getDay() == 6 && e.getDay() == 0) {
				var adjust = 0;
				} else if (e.getDay() == 6 || e.getDay() == 0) {
				var adjust = 5-s.getDay();
				} else if (s.getDay() == 0 || s.getDay() == 6) {
				var adjust = e.getDay();
				} else if (e.getDay() > s.getDay() ) {
				var adjust = e.getDay()-s.getDay();
				} else {
				var adjust = 5+e.getDay()-s.getDay();
				}
			return (weeksBetween * 5) + adjust;
			}

		function OnEndCallback(s, e) {  
			if (gv_days_off.IsEditing()) {  
				var popup = s.GetPopupEditForm();  
				popup.Shown.AddHandler(function (s, e) { CalculateHours(true); }); 
				}  
			} 
	</script>
    <uc:days_off ID="uc_days_off" runat="server" />
    <uc:days_off_new ID="uc_days_off_new" runat="server" Visible="false" />
</asp:Content>