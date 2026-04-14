<%@ Page Title="Cap Schedule Grid" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_hr_member_cap_schedule" EnableTheming="True" Theme="NETheme01" Codebehind="cap_schedule.aspx.cs" %>	



<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>

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
		oncustomjsproperties="gv_CustomJSProperties">
	<Columns>
		<dx:GridViewDataTextColumn FieldName="id" 
			ReadOnly="True" VisibleIndex="0" Visible="False">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataDateColumn FieldName="Date" VisibleIndex="1" 
			Width="125px" SortIndex="0" SortOrder="Descending">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
				EditFormatString="yyyy-MM-dd">
			</PropertiesDateEdit>
		</dx:GridViewDataDateColumn>
		<dx:GridViewDataTextColumn FieldName="Training Name" VisibleIndex="2" 
			Width="50%">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Location" 
			VisibleIndex="3" Width="50%">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Max Fill" 
			VisibleIndex="6" Width="75px">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Enrolled" VisibleIndex="7" Width="75px">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTimeEditColumn Caption="Start Time" FieldName="start_time" 
			VisibleIndex="4" Width="70px">
			<PropertiesTimeEdit DisplayFormatString="">
			</PropertiesTimeEdit>
		</dx:GridViewDataTimeEditColumn>
		<dx:GridViewDataTimeEditColumn Caption="End Time" FieldName="end_time" 
			VisibleIndex="5" Width="70px">
			<PropertiesTimeEdit DisplayFormatString="">
			</PropertiesTimeEdit>
		</dx:GridViewDataTimeEditColumn>
	</Columns>
	<SettingsBehavior ColumnResizeMode="Control" />
	<SettingsPager Mode="ShowAllRecords">
	</SettingsPager>
	<SettingsEditing EditFormColumnCount="7" Mode="PopupEditForm" />
	<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowTitlePanel="True" 
		ShowFilterRowMenu="True" ShowFooter="True" ColumnMinWidth="10" />
	<SettingsText PopupEditFormCaption="Add / Edit CAP Schedule" 
		Title="Cap Schedule Grid" />
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
	 <Templates>
                    
					
                </Templates>
	</dx:ASPxGridView>
		
	
	<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
cap_training_schedule.id id,
date(cap_training_schedule.date) `Date`,
training_header.`name` `Training Name`,
cap_training_schedule.start_time,
cap_training_schedule.end_time,
cap_training_schedule.location `Location`,
cap_training_schedule.max_fill `Max Fill`,
(Select count(training_header_history.id) from training_header_history where training_header_history.cap_training_schedule_id = cap_training_schedule.id) Enrolled
FROM
cap_training_schedule
INNER JOIN training_header ON cap_training_schedule.training_header_id = training_header.id
ORDER BY
cap_training_schedule.date DESC"></asp:SqlDataSource>
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

