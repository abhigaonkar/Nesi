<%@ Page Language='C#' MasterPageFile='../../../IntraDefault.master' AutoEventWireup='true' Inherits='sections_reports_auto_reports' Title='Auto Report Settings' EnableTheming="True" Codebehind="index.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' runat='Server'>
    <div id='divSide' runat='server'>
		<asp:SqlDataSource ID='Users' runat='server'></asp:SqlDataSource>
	</div>
</asp:Content>
<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' runat='Server'>
    <div id='divMenu' runat='server'>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	
</asp:Content>
<asp:Content ID='Content4' ContentPlaceHolderID='cphMasterBody' runat='Server'>

		
	
	<table style="width:100%;">
		<tr>
			<td>
				<dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
					DataSourceID="SqlDataSource1" KeyFieldName="id" 
					onrowupdating="ASPxGridView1_RowUpdating" Theme="NETheme01" Width="100%" OnRowDeleting="ASPxGridView1_RowDeleting" OnRowInserting="ASPxGridView1_RowInserting">
					<Columns>
						<dx:GridViewCommandColumn VisibleIndex="0" Caption=" " ShowDeleteButton="True" ShowNewButtonInHeader="True">
						</dx:GridViewCommandColumn>
						<dx:GridViewDataTextColumn FieldName="id" Visible="False" VisibleIndex="1">
							<EditFormSettings Visible="False" />
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Interval (Days - 0 means Do Not Send)" 
							FieldName="interval" VisibleIndex="6" Width="100px">
							<CellStyle HorizontalAlign="Center">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn FieldName="interval_id" Visible="False" 
							VisibleIndex="3">
							<EditFormSettings Visible="False" />
						</dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="auto_reports_id" FieldName="auto_reports_id" Visible="False" VisibleIndex="7">
                            <EditFormSettings Visible="True" />
                        </dx:GridViewDataTextColumn>
					    <dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="business_unit_id" VisibleIndex="4">
                            <PropertiesComboBox DataSourceID="Sqlbranches" TextField="ddl_name" ValueField="id" ValueType="System.Int32">
                            </PropertiesComboBox>
                        </dx:GridViewDataComboBoxColumn>
					    <dx:GridViewDataComboBoxColumn Caption="Report" FieldName="auto_reports_id" VisibleIndex="2">
                            <PropertiesComboBox DataSourceID="Sqlreports" TextField="report_name" ValueField="id" ValueType="System.Int32">
                            </PropertiesComboBox>
                           
                        </dx:GridViewDataComboBoxColumn>
					</Columns>
					<SettingsBehavior ConfirmDelete="True" />
					<SettingsEditing Mode="Batch">
					</SettingsEditing>
				</dx:ASPxGridView>
				</td>
			<td class="style6">
				&nbsp;</td>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td>
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
auto_reports_schedule.auto_reports_id,
auto_reports_schedule.id,
auto_reports.report_name,
business_unit.ddl_name business_name,
auto_reports_schedule.`interval`,
auto_reports_schedule.last_modified,
business_unit.id business_unit_id
FROM
auto_reports_schedule
INNER JOIN auto_reports ON auto_reports_schedule.auto_reports_id = auto_reports.id
LEFT join business_unit ON auto_reports_schedule.business_unit_id = business_unit.id
WHERE
auto_reports_schedule.member_id = ?mid">
					<SelectParameters>
						<asp:ControlParameter ControlID="hdn_mid" Name="mid" PropertyName="Value" />
					</SelectParameters>
				</asp:SqlDataSource>
			    <asp:SqlDataSource ID="Sqlbranches" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
                    SelectCommand="call get_visible_business_units(?mid)">
					<SelectParameters>
						<asp:ControlParameter ControlID="hdn_mid" Name="mid" PropertyName="Value" />
					</SelectParameters>
			    </asp:SqlDataSource>
                 <asp:SqlDataSource ID="Sqlreports" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
                    SelectCommand="SELECT
auto_reports.id,
auto_reports.report_name
FROM
auto_reports
LEFT JOIN memberpageprivilege ON auto_reports.privilege_id = memberpageprivilege.MemberPagePrivilege_Privilege_ID
left JOIN privilege ON memberpageprivilege.MemberPagePrivilege_Privilege_ID = privilege.Privilege_ID
LEFT JOIN memberpage ON auto_reports.page_id = memberpage.MemberPage_Page_ID
left JOIN page ON page.page_id = memberpage.MemberPage_Page_ID
LEFT JOIN auto_reports_schedule ON auto_reports.id = auto_reports_schedule.auto_reports_id AND auto_reports_schedule.member_id = ?mid
WHERE
(( memberpageprivilege.MemberPagePrivilege_ID <> 0 AND
Privilege_Enabled = 1 and
memberpageprivilege.MemberPagePrivilege_Member_ID = ?mid)
 OR
( memberpage.MemberPage_ID <> 0 AND
page_enabled = 1 and 
memberpage.MemberPage_Member_ID = ?mid)) "
                     >
					<SelectParameters>
						<asp:ControlParameter ControlID="hdn_mid" Name="mid" PropertyName="Value" />
					</SelectParameters>


			    </asp:SqlDataSource>
			</td>
			<td class="style6">
				&nbsp;</td>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td colspan="3">
				<dx:ASPxButton ID="ASPxButton2" runat="server" Text="Save" Theme="NETheme01" ClientVisible="False">
				</dx:ASPxButton>
				<asp:HiddenField ID="hdn_mid" runat="server" />
				<asp:HiddenField ID="hdn_mtid" runat="server" />
				<br />
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
			<td class="style6">
				&nbsp;</td>
			<td>
				&nbsp;</td>
		</tr>
	</table>

		
	
    </table>

		
	
</asp:Content>
<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
    <style type="text/css">
		.style6
		{
			width: 9px;
		}
	</style>
</asp:Content>

