<%@ Page Language="C#" MasterPageFile="../../../IntraDefault.master" AutoEventWireup="true" Inherits="ext_locations" Title="Member External locations" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>








<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' runat='Server'>
	<div id='divSide' runat='server'>
	</div>
</asp:Content>
<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' runat='Server'>
	<div id='divMenu' runat='server'>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<script type="text/javascript">
	</script>
	<asp:ScriptManager runat="server" ID="ScriptManager1">
	</asp:ScriptManager>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<table cellspacing="0" cellpadding="2">
				<tr>
					<td colspan="3">
						<dx:ASPxComboBox	ID                 = "cb_branch" 
											runat              = "server" 
											AutoPostBack       = "True" 
											ClientInstanceName = "cb_branch"
											DataSourceID       = "ds_branches"
											AnimationType      = "None"
											TextField          = "name" 
											ValueField         = "id" 
											Width			   = "100%" 
											ValueType          = "System.Int32">
						</dx:ASPxComboBox>
					</td>
				</tr>
				<tr>
					<td colspan="3" align="center">
						<br />
						When removing an employee's default location, choose the <b style="color:#f00;">"Not Set"</b> option.
						<br />
						<br />
					</td>
				</tr>
				<tr>
					<td colspan="3">
						<dx:ASPxGridView ID="gv_default_locations" runat="server" AutoGenerateColumns="False" DataSourceID="ds_default_locations" KeyFieldName="member_id" OnRowUpdating="gv_default_locations_RowUpdating" onhtmldatacellprepared="gv_default_locations_HtmlDataCellPrepared" Width="450px">

							<Columns>
								<dx:GridViewCommandColumn Caption=" " VisibleIndex="0" ShowEditButton="true">
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="Employee" FieldName="member_fullname" ReadOnly="True" VisibleIndex="1" Width="200px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataComboBoxColumn Caption="Default External Location" FieldName="location" VisibleIndex="2">
									<PropertiesComboBox DataSourceID="ds_locations" TextField="name" ValueField="id" ValueType="System.Int32">
									</PropertiesComboBox>
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataTextColumn FieldName="member_id" ReadOnly="True" VisibleIndex="3" Visible="False">
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsPager Mode="ShowAllRecords">
							</SettingsPager>
							<SettingsEditing Mode="Inline" />
						</dx:ASPxGridView>
						<asp:SqlDataSource ID="ds_locations" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
	0 id, 
	'Not Set' name 
UNION 
SELECT
	id, 
	name 
FROM 
	inventory_location_master 
WHERE 
	type_id = 2 AND 
	business_unit_id = ?companyid">
							<SelectParameters>
								<asp:ControlParameter ControlID="cb_branch" Name="companyid" PropertyName="Value" />
							</SelectParameters>
						</asp:SqlDataSource>
						<asp:SqlDataSource ID="ds_default_locations" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT
	member_id,
	member_fullname,
	member_default_location  location
FROM
	member
WHERE 
	member_status = 'Active' and 
	business_unit_id = ?companyid">
							<SelectParameters>
								<asp:ControlParameter ControlID="cb_branch" Name="companyid" PropertyName="Value" />
							</SelectParameters>
						</asp:SqlDataSource>
						<asp:SqlDataSource ID="ds_branches" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
	id,
	ddl_name name 
FROM 
	business_unit 
WHERE 
	active ='T'"></asp:SqlDataSource>
					</td>
				</tr>
			</table>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
