<%@ Page Language="C#" AutoEventWireup="true" Theme="" MasterPageFile="~/nonFrame.master" Inherits="sections_member_inventory_history" Codebehind="history.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ContentPlaceHolderID="cphMasterBody" runat="server">

	<script>
		function resize()
			{
			var height		= $("#gv_history").height()+250;
			var type		= "if_qh";
			if(parent.resize_frame)
				{
				parent.resize_frame(type, height);
				}
			}
		$(document).ready(function()
							{
							//resize();
							});
	</script>
    <div>
			<dx:ASPxPageControl ID="pc" runat="server" ActiveTabIndex="0" Width="100%" Theme="NETheme01">
				<TabPages>
					<dx:TabPage Text="Inventory Log">
						<ContentCollection>
							<dx:ContentControl runat="server">
								<dx:ASPxGridView ID="gv_history" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_history" DataSourceID="ds_history" OnHtmlDataCellPrepared="gv_history_HtmlDataCellPrepared" Width="100%" Theme="NETheme01">
									<ClientSideEvents EndCallback="function(s, e) {
	resize();
}" />
									<Columns>
										<dx:GridViewDataTextColumn Caption="Date" FieldName="dt" ShowInCustomizationForm="True" VisibleIndex="0">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Origin" FieldName="section" ShowInCustomizationForm="True" VisibleIndex="2">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Action" FieldName="action" ShowInCustomizationForm="True" VisibleIndex="3">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Member Name" FieldName="member_name" ShowInCustomizationForm="True" VisibleIndex="4">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Changed From" FieldName="value_old" ShowInCustomizationForm="True" VisibleIndex="5">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Changed To" FieldName="value_new" ShowInCustomizationForm="True" VisibleIndex="6">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Location" FieldName="location" ShowInCustomizationForm="True" VisibleIndex="1">
											<Settings AutoFilterCondition="Contains" />
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Manual" FieldName="is_manual" ShowInCustomizationForm="True" VisibleIndex="7">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Del" ShowInCustomizationForm="False" Visible="False" VisibleIndex="8">
											<DataItemTemplate>
												<button onclick='cb_del.PerformCallback(<%# Eval("id") %>)' type="button">
													Del
												</button>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsPager NumericButtonCount="50" PageSize="50">
									</SettingsPager>
									<SettingsBehavior EnableCustomizationWindow="True" EnableRowHotTrack="True" />
									<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
									<SettingsText EmptyDataRow="No Information Available" />
									<SettingsPopup>
										<CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="TopSides" />
									</SettingsPopup>
								</dx:ASPxGridView>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Text="Stock Transfer">
						<ContentCollection>
							<dx:ContentControl runat="server">
								<dx:ASPxGridView ID="gv_stocktransfer" runat="server" Theme="NETheme01" Width="100%" AutoGenerateColumns="False" DataSourceID="sds_stock">
									<Columns>
										<dx:GridViewDataTextColumn FieldName="name" ShowInCustomizationForm="True" VisibleIndex="0" Caption="Member Name">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="type" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Type of Xfer">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="transferred_from" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Xfer From">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="transferred_to" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Xfer To">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="qty" ShowInCustomizationForm="True" VisibleIndex="5" Caption="QTY">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="note" ShowInCustomizationForm="True" VisibleIndex="6" Caption="Note">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Date" FieldName="dt" ShowInCustomizationForm="True" VisibleIndex="1">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsPager NumericButtonCount="50" PageSize="50">
									</SettingsPager>
									<SettingsBehavior EnableCustomizationWindow="True" EnableRowHotTrack="True" />
									<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
									<SettingsText EmptyDataRow="No Information Available" />
									<SettingsPopup>
										<CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="TopSides" />
									</SettingsPopup>
								</dx:ASPxGridView>
								<asp:SqlDataSource ID="sds_stock" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
	b.member_fullname name, 
	c.name type, 
a.stock_transfer_date_added dt,
	IFNULL(CASE a.stock_transfer_type 
		WHEN 1 THEN (SELECT name FROM inventory_location_master WHERE id = a.stock_transfer_from_id)
		WHEN 2 THEN (SELECT poprog_bvpo FROM poprog_header WHERE poprog_id = a.stock_transfer_from_id)
		WHEN 3 THEN (SELECT woprog_bvwo FROM woprog WHERE woprog_id = a.stock_transfer_from_id)
		WHEN 4 THEN (SELECT woprog_bvwo FROM woprog WHERE woprog_id = a.stock_transfer_from_id)
		WHEN 5 THEN (SELECT name FROM inventory_location_master WHERE id = a.stock_transfer_from_id)
		ELSE NULL
	END, '--') transferred_from,  
	IFNULL(CASE a.stock_transfer_type 
		WHEN 1 THEN (SELECT woprog_bvwo FROM woprog WHERE woprog_id = a.stock_transfer_to_id)
		WHEN 2 THEN (SELECT name FROM inventory_location_master WHERE id = a.stock_transfer_to_id)
		WHEN 3 THEN (SELECT name FROM inventory_location_master WHERE id = a.stock_transfer_to_id)
		WHEN 4 THEN (SELECT name FROM inventory_location_master WHERE id = a.stock_transfer_to_id)
		WHEN 5 THEN (SELECT poprog_bvpo FROM poprog_header WHERE poprog_id = a.stock_transfer_to_id)
		ELSE NULL
	END, '--') transferred_to,  
	a.stock_transfer_quantity qty, 
	urldecode(stock_transfer_note) note 
FROM 
	stock_transfer a 
LEFT JOIN 
	member b 
		ON a.stock_transfer_member_id = b.member_id 
LEFT JOIN
	stock_transfer_type c ON a.stock_transfer_type = c.id
where 
	a.business_unit_id = @business_unit_id and 
	a.stock_transfer_master_id = @master_id 
order by stock_transfer_id desc;">
			<SelectParameters>
				<asp:QueryStringParameter Name="@master_id" QueryStringField="master_id" />
				<asp:QueryStringParameter Name="@business_unit_id" QueryStringField="business_unit_id" />
			</SelectParameters>
								</asp:SqlDataSource>
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
				</TabPages>
			</dx:ASPxPageControl>
    
    </div>
		<asp:SqlDataSource ID="ds_history" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
	a.id,
	a.dt,
	IFNULL(g.name, CONCAT('(Deleted Location - ID#: ',a.associated_table_id,')')) location,
	c.name action,
	CAST(member_fullname AS CHAR(100)) member_name,
	URLDECODE(a.value_old) value_old,
	URLDECODE(a.value_new) value_new,
	IF(a.is_manual = 0, 'False', 'True') is_manual,
	b.name section
FROM
	log a
LEFT JOIN
	log_section b ON a.section_id = b.id
LEFT JOIN
	log_action c ON a.action_id = c.id
LEFT JOIN
	member d ON a.member_id = d.member_id
LEFT join
	business_unit e ON a.business_unit_id = e.id
LEFT JOIN
	inventory_location f ON a.associated_table_id = f.id
LEFT JOIN
	inventory_location_master g ON f.location_master_id = g.id
WHERE
	a.associated_table	= &quot;inventory_location&quot; AND
	a.associated_alt_table_id	= @master_id AND
	a.business_unit_id = @business_unit_id
ORDER BY
a.dt DESC">
			<SelectParameters>
				<asp:QueryStringParameter Name="@master_id" QueryStringField="master_id" />
				<asp:QueryStringParameter Name="@business_unit_id" QueryStringField="business_unit_id" />
			</SelectParameters>
		</asp:SqlDataSource>
</asp:Content>