<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="TimeSheet_Report" Title="Time Sheet Report" Theme="NETheme01" EnableTheming="true" EnableEventValidation="true" Codebehind="TimeSheet_Report.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/modules/layout_control.ascx" TagPrefix="uc" TagName="layout_control" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterLeft" runat="Server">
    <div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterMenu" runat="Server">
    <div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" runat="Server">
    <script type="text/javascript"" language="javascript">

		function Clickheretoprint()
		{
			var disp_setting = "toolbar=yes,location=no,directories=yes,menubar=yes,";
			disp_setting += "scrollbars=yes,width=850, height=800, left=50, top=25";
			var content_value = document.getElementById("print_content").innerHTML;

			var currentStamp = new Date();
			var day = currentStamp.getDate();
			var month = currentStamp.getMonth() + 1;
			var year = currentStamp.getFullYear();
			var thisdate = month + "/" + day + "/" + year;

			var hours = currentStamp.getHours();
			var minutes = currentStamp.getMinutes();
			var suffix = "AM";
			if (hours >= 12)
			{
				suffix = "PM";
				hours = hours - 12;
			}
			if (hours == 0)
			{
				hours = 12;
			}

			if (minutes < 10)
			{
				minutes = "0" + minutes;
			}
			var thistime = hours + ":" + minutes + " " + suffix;

			var docprint = window.open("", "", disp_setting);
			var cssinfo = "<link rel='stylesheet' type='text/css' href='/css/timesheet_report-printable.css'>";
			var thisdatetime = "<div id='datetime'><b>" + thisdate + "</b> @ <i>" + thistime + "</i></div>";

			docprint.document.open();
			docprint.document.write("\
		<html>\
			<head>\
				<title>Report</title>\
		" + cssinfo + "\
			</head>\
			<body onLoad='self.print()'>\
		" + thisdatetime + "\
				<div id='printarea'>\
		" + content_value + "\
				</div>\
			</body>\
		</html>");
			docprint.document.close();
			docprint.focus();

		}
	$(document).ready(function()
		{
		page_obj.update_panel_progress.bind();
		});
	</script>

<asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
                            <uc:layout_control ID="layout" runat="server" GridviewID="gv_results" />
<asp:UpdatePanel ID="up" runat="server">
	<ContentTemplate>
    <div id="toprinter"><a href="javascript:Clickheretoprint()">
                        <img src="images/icon/icon[print].gif" height="25px" />
                        </a></div>   
    <div id="print_content">
        <div id="timesheet_report_search">
            <table cellpadding="5" cellspacing="0">
                <tr>
                    <td class="c">Select Start Date</td>
                    <td class="style5">
                        <dx:ASPxDateEdit ID="de_start" runat="server" ClientInstanceName="de_start" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
							<ClientSideEvents DateChanged="function(s, e) {
	ddl_payperiod.SetSelectedIndex(0);
	if(s.GetDate() &gt; de_end.GetDate())
		{
		s.SetDate(null);
		s.SetText(&quot;&quot;);
		}
}" />
						</dx:ASPxDateEdit>
                     </td>
					<td class="checkboxes" rowspan="10">
						<div class="title">Include</div><br />
                        <asp:CheckBox ID="chkVacations" runat="server" Text="Vacations" /><br />                       
						<br />
					</td>
				</tr>
				<tr>
                    <td class="c">Select End Date</td>
                    <td class="v" style="width: 392px">
                        <dx:ASPxDateEdit ID="de_end" runat="server" ClientInstanceName="de_end" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
							<ClientSideEvents DateChanged="function(s, e) {
	ddl_payperiod.SetSelectedIndex(0);
	if(s.GetDate() &lt; de_start.GetDate())
		{
		s.SetDate(null);
		s.SetText(&quot;&quot;);
		}
}" />
						</dx:ASPxDateEdit>
                    </td>
                </tr>
                <tr>
                    <td class="c">Payperiod</td>
                    <td class="v" style="width: 392px">
                        <dx:ASPxComboBox ID="ddl_payperiod" runat="server" ClientInstanceName="ddl_payperiod" TextField="PayDate" ValueField="payperiodID" ValueType="System.Int32" Width="300px">
						</dx:ASPxComboBox>
					</td>
				</tr>
				
                <tr>
                    <td class="c"></td>
                    <td align="left">
                        <dx:ASPxButton ID="bt_submit" runat="server" Text="Search" onclick="bt_submit_Click">
							<Image Url="~/images/icon/icon[search].gif">
							</Image>
						</dx:ASPxButton>
					</td>
                </tr>
            </table>
		</div>
        <div id='timesheet_report_results'>
			
       

	
	
        <dx:ASPxGridView ID="gv_results" runat="server" AutoGenerateColumns="False" OnCustomCallback="gv_results_CustomCallback" OnCustomJSProperties="gv_results_CustomJSProperties" Theme="NETheme01" Width="100%">
            <TotalSummary>
                <dx:ASPxSummaryItem DisplayFormat="Total Hours: {0:#,##0.00}" FieldName="hours" ShowInColumn="Hours" SummaryType="Sum" />
            </TotalSummary>
            <GroupSummary>
                <dx:ASPxSummaryItem DisplayFormat="Sum = {0:N2}" FieldName="hours" SummaryType="Sum" />
            </GroupSummary>
            <SettingsResizing ColumnResizeMode="Control" />
            <Columns>
                <dx:GridViewDataDateColumn Caption="Date" FieldName="date" VisibleIndex="0" Width="5%">
                    <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="name" VisibleIndex="1" Width="10%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Employee" FieldName="employee" VisibleIndex="2" Width="10%">
                </dx:GridViewDataTextColumn>
                  <dx:GridViewDataTextColumn Caption="Employee Status" FieldName="member_status" VisibleIndex="3" Width="10%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Type" FieldName="wotype" VisibleIndex="4" Width="5%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Cust No" FieldName="custno" VisibleIndex="5" Width="7%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Customer" FieldName="customername" VisibleIndex="6" Width="15%">
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Work Order" FieldName="workorder" VisibleIndex="7" Width="7%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Parent WO" FieldName="childworkorder" VisibleIndex="8" Width="7%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Hour Type" FieldName="hourtype" VisibleIndex="9" Width="5%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Hours" FieldName="hours" VisibleIndex="10" Width="5%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="% Complete" FieldName="completed" VisibleIndex="11" Width="5%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Comments" cellstyle-wrap="True" FieldName="comments" VisibleIndex="12" Width="10%">
                    <CellStyle Wrap="True">
                    </CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Netsuite Hours Type" FieldName="_netsuite_hours_type" VisibleIndex="13">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Netsuite Hours Type ID" FieldName="_netsuite_hours_type_id" VisibleIndex="14">
                </dx:GridViewDataTextColumn>
	            <dx:GridViewDataTextColumn Caption="Payroll ID" FieldName="payroll_id" Visible="False" VisibleIndex="15">
	            </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn Caption="Date Added" FieldName="Created_Date" VisibleIndex="16" Visible="false">
                    <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm" EditFormat="Custom" EditFormatString="yyyy-MM-dd HH:mm">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn Caption="Start Time" FieldName="start_time" VisibleIndex="17"   Width="5%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Morning Break Start" FieldName="morning_break_start" VisibleIndex="18"   Width="5%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Morning Break Duration" FieldName="morning_break_duration" VisibleIndex="19"   Width="5%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Lunch Break Start" FieldName="lunch_break_start" VisibleIndex="20"   Width="5%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Lunch Break Duration" FieldName="lunch_break_duration" VisibleIndex="21"  Width="5%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Afternoon Break Start" FieldName="afternoon_break_start" VisibleIndex="22"   Width="5%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Afternoon Break Duration" FieldName="afternoon_break_duration" VisibleIndex="23"   Width="5%">
                </dx:GridViewDataTextColumn>         
                <dx:GridViewDataTextColumn Caption="Internal Project" FieldName="internal_project" Visible="False" VisibleIndex="24" Width="3%">
	            </dx:GridViewDataTextColumn>
                  <dx:GridViewDataTextColumn Caption="Province/State" FieldName="Province" Visible="true" VisibleIndex="25" Width="5%">
	            </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Scope/Task" FieldName="Scope" Visible="true" VisibleIndex="26" Width="10%">
                     <CellStyle Wrap="True">
                    </CellStyle>
	            </dx:GridViewDataTextColumn>
                 <dx:GridViewDataTextColumn Caption="Job-Type" FieldName="membertype_name" VisibleIndex="27" Visible="true" Width="5%"></dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Work Order Description" FieldName="woprog_description" VisibleIndex="28" Visible="true" Width="15%"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Tax Entity" FieldName="public_name" VisibleIndex="29" Width="10%">
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" />
            <SettingsPager PageSize="50">
            </SettingsPager>
            <Settings ColumnMinWidth="30" ShowFilterBar="Visible" ShowFilterRow="True" ShowFooter="True" ShowGroupPanel="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" />
            <Styles>
                <Header HorizontalAlign="Center">
                </Header>
                <Cell HorizontalAlign="Center">
                </Cell>
            </Styles>
            <SettingsAdaptivity>
                <AdaptiveDetailLayoutProperties>
                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="340" />
                </AdaptiveDetailLayoutProperties>
            </SettingsAdaptivity>
            <Templates>
                <TitlePanel>
                </TitlePanel>
            </Templates>
        </dx:ASPxGridView>
	
             </div>
         </div>
  
         
   
	    	
	</ContentTemplate>
</asp:UpdatePanel>

    
  </asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
    &nbsp;
</asp:Content>

<asp:Content ID="Content5" runat="server" contentplaceholderid="header_placeholder">
    <style type="text/css">
		.style5 { width: 392px; }
	</style>
</asp:Content>


