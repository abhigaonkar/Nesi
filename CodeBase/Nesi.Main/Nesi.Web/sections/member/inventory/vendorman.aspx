<%@ Page Language="C#" AutoEventWireup="true"  Inherits="sections_member_inventory_vendorman"
 EnableTheming="True"  masterpagefile="~/IntraDefault.master" Title="Vendor Cross Reference" Codebehind="vendorman.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>


<ASP:CONTENT ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server"></div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server"></div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	
	
</ASP:CONTENT>
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript" language="javascript">
	function get_customer(id)
			{
			boing("/#/opens/11/vendors/"+id, "vendor", 1024,768);
			}
			</script>


	<uc1:layout_control ID="layout" runat="server"
		 />
<dx:ASPxGridView ID="gv" runat="server" ClientInstanceName="gv" 
		Theme="NETheme01" Width="100%" AutoGenerateColumns="False" 
		DataSourceID="SqlDataSource1" KeyFieldName="Vendor_ID" 
		oncustomcallback="gv_CustomCallback" 
		oncustomjsproperties="gv_CustomJSProperties">
	<Columns>
		<dx:GridViewCommandColumn Caption=" " ShowClearFilterButton="True" 
			VisibleIndex="0">
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn FieldName="Manufacturer" VisibleIndex="1">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Vendor_ID" ReadOnly="True" 
			VisibleIndex="2">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Vendor" VisibleIndex="3">
			<DataItemTemplate>
				<a href="javascript:void(-1)" onclick="get_customer(<%# Eval("Vendor_ID") %>)">
								<%#Container.Text %>
							</a>
			</DataItemTemplate>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Addr_1" VisibleIndex="4">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Addr_2" VisibleIndex="5">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="City" VisibleIndex="6">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="Phone" VisibleIndex="7">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataCheckColumn FieldName="Is_Partner" VisibleIndex="8">
		</dx:GridViewDataCheckColumn>
		<dx:GridViewDataTextColumn FieldName="Account_No" VisibleIndex="9">
		</dx:GridViewDataTextColumn>
	</Columns>
	<SettingsPager PageSize="50">
	</SettingsPager>
	<Settings ShowFilterRow="True" />
	<SettingsSearchPanel ShowClearButton="True" Visible="True" />
	</dx:ASPxGridView><asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT DISTINCT
inventory_attribute_value.`value` Manufacturer,
vendor.Vendor_ID,
vendor.Vendor_Name Vendor,
address.Address_Addr1 Addr_1,
address.Address_Addr2 Addr_2,
address.Address_City City,
address.address_phonefull Phone,
vendor.is_partner Is_Partner, 
vendor.Vendor_Account Account_No
FROM
inventory_attribute_value
INNER JOIN inventory_item_detail ON inventory_item_detail.attribute_value_id = inventory_attribute_value.attribute_value_id
LEFT JOIN inventory_price ON inventory_item_detail.master_id = inventory_price.master_id
INNER JOIN vendor ON inventory_price.vendor_id = vendor.Vendor_ID and vendor_name not like '%new electric%'
INNER JOIN address ON vendor.Vendor_ID = address.Address_Table_ID AND address.address_table = 'Vendor'
where inventory_attribute_value.attribute_id = 16

order by inventory_attribute_value.`value`,vendor.Vendor_Name"></asp:SqlDataSource>
</ASP:CONTENT>