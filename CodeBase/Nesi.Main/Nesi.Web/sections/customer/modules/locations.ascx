<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_customer_modules_locations" EnableViewState="true" EnableTheming="True" Codebehind="locations.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register src="location.ascx" tagname="location" tagprefix="uc" %>
<%@ Register src="phone.ascx" tagname="phone" tagprefix="uc" %>
<%@ Register src="contact.ascx" tagname="contact" tagprefix="uc" %>
<%@ Register src="sales.ascx" tagname="sales" tagprefix="uc" %>
<div class="current_locations" runat="server" id="current_locations">
	<dx:ASPxGridView ID="gv_locations" runat="server" DataSourceID="ds_locations" 
		AutoGenerateColumns="False" KeyFieldName="address_id" Width="100%" 
		onhtmleditformcreated="gv_locations_HtmlEditFormCreated" 
		EnableCallBacks="False" Theme="NETheme01" EnableCallbackAnimation="True">
		<ClientSideEvents RowClick="function(s, e) {
		s.StartEditRow(e.visibleIndex);
}" />
<ClientSideEvents RowClick="function(s, e) {
		s.StartEditRow(e.visibleIndex);
}"></ClientSideEvents>

        <SettingsCommandButton>
            <EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
            <NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
        </SettingsCommandButton>

		<Columns>
			<dx:GridViewDataTextColumn Caption="Name" FieldName="description" 
				VisibleIndex="1" Visible="False">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Addr 1" FieldName="addr1" VisibleIndex="3" 
				MinWidth="20" Width="100%">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Addr 2" FieldName="addr2" VisibleIndex="4" 
				MinWidth="10" Width="10%">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Addr 3" FieldName="addr3" VisibleIndex="5" 
				MinWidth="10" Width="10%">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Addr 4" FieldName="addr4" VisibleIndex="6" 
				MinWidth="10" Width="5%">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="City" FieldName="city" VisibleIndex="7" 
				Width="50px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Prov/State" FieldName="provstate" 
				VisibleIndex="8" Width="30px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Postal" FieldName="postal" VisibleIndex="9" 
				Width="40px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Country" FieldName="country" 
				VisibleIndex="10" Width="30px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Type" FieldName="addr_type" 
				VisibleIndex="2" Width="40px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewCommandColumn ButtonType="Image" Caption="Action" VisibleIndex="0" ShowEditButton="true" ShowNewButton="true" ShowClearFilterButton="true"
				Width="50px">				
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="Status" FieldName="statuss" 
				VisibleIndex="11" Width="50px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Branch" FieldName="branch" 
				VisibleIndex="12" Width="50px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Account Manager" FieldName="am" 
				VisibleIndex="13" Width="75px">
				<HeaderStyle Wrap="True" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Location Type" FieldName="address_table" 
				VisibleIndex="14" Width="50px">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior AllowSelectByRowClick="True" />
		<SettingsDetail AllowOnlyOneMasterRowExpanded="True" ShowDetailButtons="False" ShowDetailRow="False" />

<SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="Control"></SettingsBehavior>

<SettingsDetail ShowDetailButtons="False" AllowOnlyOneMasterRowExpanded="True"></SettingsDetail>

		<Styles>
			<LoadingPanel VerticalAlign="Middle">
			</LoadingPanel>
		</Styles>

		<Templates>
			<EditForm>
				<uc:location ID="uc_location_detail" runat="server" />
			</EditForm>
		</Templates>
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="ds_locations" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		
		SelectCommand="SELECT
a.Address_ID AS address_id,
IF (
          address_type = 'B',
          'Billing',
          'Shipping'
) AS addr_type,
a.Address_Desc AS description,
a.Address_Addr1 AS addr1,
a.Address_Addr2 AS addr2,
a.Address_Addr3 AS addr3,
a.Address_Addr4 AS addr4,
a.Address_City AS city,
a.Address_Prov AS provstate,
a.Address_Postal AS postal,
a.Address_Country AS country,
IFNULL(c.customer_or_contact_status, 'Not Set') AS statuss,
member.member_fullname AS am,
business_unit.name AS branch,
a.address_table 
FROM
address AS a
LEFT JOIN customer_sales_properties AS b ON a.Address_ID = b.address_id
LEFT JOIN customer_or_contact_status AS c ON b.status_id = c.customer_or_contact_status_id
LEFT JOIN customer_sales_properties ON customer_sales_properties.customer_id = b.customer_id AND customer_sales_properties.address_id = b.address_id
LEFT JOIN member ON member.Member_ID = customer_sales_properties.account_manager
LEFT JOIN business_unit ON member.business_unit_id = business_unit.id
WHERE
          (address_table = 'Customer' or address_table='Worksite') AND address_table_id = ?customer_id">
		<SelectParameters>
			<asp:ControlParameter ControlID="hdn_customer_id" DefaultValue="0" Name="?customer_id" PropertyName="Value" />
		</SelectParameters>
	</asp:SqlDataSource>
</div>
<asp:HiddenField ID="hdn_address_id" runat="server" />
<asp:HiddenField ID="hdn_customer_id" runat="server" />