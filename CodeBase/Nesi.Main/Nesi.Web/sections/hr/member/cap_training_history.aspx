<%@ Page Title="Cap Training History Grid" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_hr_member_cap_training_history" EnableTheming="True" Theme="NETheme01" Codebehind="cap_training_history.aspx.cs" %>	



<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>
	
<%@ Register src="../../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>
	
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">

	<script type="text/javascript">
 function bind_tooltips()
			{
			$(".opt1").each(function()
				{
				$(this).tip();
				});
			}
		$(document).ready(function()
			{
	//		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			bind_tooltips();
			});

function EndReqHandler()
	{
	bind_tooltips();
	
	}
	</script>

<table width = "100%">
<tr>
<td>
	<lc:LayoutControl ID="LayoutControl1" runat="server" GridviewID="gv" />
<dx:ASPxGridView ID="gv" runat="server" ClientInstanceName="gv" 
		Width="100%" AutoGenerateColumns="False" 
		Font-Names="Arial" KeyFieldName="id" Theme="NETheme01" 
		DataSourceID="SqlDataSource1" oncustomcallback="gv_CustomCallback" 
		oncustomjsproperties="gv_CustomJSProperties" onrowdeleting="gv_RowDeleting" 
		oncommandbuttoninitialize="gv_CommandButtonInitialize" 
		oncustombuttoninitialize="gv_CustomButtonInitialize">
	<ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_alert!=null &amp;&amp; s.cp_alert!='')
{
alert(s.cp_alert);
s.cp_alert='';
please_wait('stop');
}
}" />
	<TotalSummary>
		<dx1:ASPxSummaryItem DisplayFormat="{0:N1}" FieldName="time_diff" 
			SummaryType="Sum" />
	</TotalSummary>
	<Columns>
		<dx:GridViewCommandColumn Caption=" " VisibleIndex="0" Width="40px" ShowDeleteButton="true">
			
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn FieldName="id" 
			ReadOnly="True" VisibleIndex="1">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Training Name" VisibleIndex="2">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataDateColumn FieldName="Training Date" VisibleIndex="3">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" 
				EditFormatString="yyyy-MM-dd">
			</PropertiesDateEdit>
		</dx:GridViewDataDateColumn>
		<dx:GridViewDataTextColumn FieldName="Issue_Certs" 
			VisibleIndex="7">
			<DataItemTemplate>
				<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" 
					oninit="ASPxButton2_Init" Text="Issue" Theme="NETheme01" 
					>
					<ClientSideEvents CheckedChanged="function(s, e) {
}" />
				</dx:ASPxButton>
			</DataItemTemplate>
		</dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="Location" VisibleIndex="5"></dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="memberid" VisibleIndex="6" 
			ReadOnly="True" Visible="False">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Employee" 
			VisibleIndex="4">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTimeEditColumn Caption="Start Time" FieldName="starttime" 
			VisibleIndex="8">
			<PropertiesTimeEdit DisplayFormatString="">
			</PropertiesTimeEdit>
		</dx:GridViewDataTimeEditColumn>
		<dx:GridViewDataTimeEditColumn Caption="End Time" FieldName="endtime" 
			VisibleIndex="9">
			<PropertiesTimeEdit DisplayFormatString="">
			</PropertiesTimeEdit>
		</dx:GridViewDataTimeEditColumn>
		<dx1:GridViewDataTextColumn Caption="Duration" FieldName="time_diff" 
			VisibleIndex="10">
			<PropertiesTextEdit DisplayFormatString="N1">
			</PropertiesTextEdit>
		</dx1:GridViewDataTextColumn>
		<dx1:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" 
			VisibleIndex="11">
		</dx1:GridViewDataTextColumn>
	</Columns>
	<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
	<SettingsPager Mode="ShowAllRecords">
	</SettingsPager>
	<SettingsEditing EditFormColumnCount="7" Mode="PopupEditForm" />
	<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowTitlePanel="True" 
		ShowFilterRowMenu="True" ShowFooter="True" ColumnMinWidth="10" 
		ShowGroupPanel="True" ShowFilterRowMenuLikeItem="True" 
		ShowHeaderFilterButton="True" />
	<SettingsText PopupEditFormCaption="Add / Edit CAP Schedule" 
		Title="CAP Training History" />
	<SettingsPopup>
		<EditForm HorizontalAlign="WindowCenter" 
			VerticalAlign="WindowCenter" Width="950px" Height="720px" />
	</SettingsPopup>
	<Styles>
		
	</Styles>
	 <StylesPopup>
		 <EditForm>
			 <Content BackColor="White">
				 <Border BorderColor="White" BorderStyle="Solid" BorderWidth="10px" />
				
		
			 </Content>
			
		 </EditForm>
	</StylesPopup>
	</dx:ASPxGridView>
		
	
	<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
training_header_history.id AS id,
training_header.`name` AS `Training Name`,
training_header_history.date AS `Training Date`,
training_header_history.start_time AS starttime,
training_header_history.end_time AS endtime,
if(training_header_history.certs_issued=0,'True','False') AS Issue_Certs,
cap_training_schedule.location AS Location,
member.Member_ID AS memberid,
member.member_fullname AS Employee,
business_unit.ddl_name business_unit,
HOUR(timediff(training_header_history.end_time,training_header_history.start_time))+(MINUTE(timediff(training_header_history.end_time,training_header_history.start_time))/60) time_diff
FROM
training_header
INNER JOIN training_header_history ON training_header_history.training_header_id = training_header.id
INNER JOIN member ON training_header_history.member_id = member.Member_ID
INNER JOIN cap_training_schedule ON training_header_history.cap_training_schedule_id = cap_training_schedule.id
INNER JOIN business_unit ON training_header_history.business_unit_id = business_unit.id
where  find_in_set(business_unit.id,@visibleBU)
ORDER BY
training_header_history.date DESC
">
	    <SelectParameters>
	        <asp:SessionParameter Name="@visibleBU" SessionField="visibleBU" Type="String" />
	    </SelectParameters>
	</asp:SqlDataSource>
	<br />
		
	
</td>
</tr>
</table>



</asp:Content>
<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		
	</style>
	</asp:Content>

