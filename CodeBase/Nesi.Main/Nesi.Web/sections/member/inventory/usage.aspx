<%@ Page Language="C#" AutoEventWireup="true" Theme="NETheme01" Inherits="sections_member_inventory_usage" Codebehind="usage.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>









<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Usage</title>
</head>
<body>
    <form id="form1" runat="server">
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script>
		function resize()
			{
			var height		= $("#gv_WO").height() == null ? $("#gv_PO").height() + 150: $("#gv_WO").height() + 150;
			var type		= $("#gv_WO").height() == null ? "if_po" : "if_wo";
			if(parent.resize_frame)
				{
			//	parent.resize_frame(type, height);
				}
			}
		$(document).ready(function()
							{
							resize();
							});
	</script>
    <div>
		<lc:LayoutControl runat="server" id="layout" />
		<dx:ASPxGridView ID="gv_PO" runat="server" AutoGenerateColumns="False" DataSourceID="ds_PO"
			Width="100%" ClientInstanceName="gv_PO" OnCustomCallback="Load_Layout" OnCustomJSProperties="gv_CustomJSProperties">
			<Columns>
				<dx:GridViewDataTextColumn Caption="PO#" FieldName="po_n" VisibleIndex="0" Width="125px">
					<DataItemTemplate>
						<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" Target="_blank" NavigateUrl='<%# string.Format("/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}", Eval("poprog_id")) %>'
							Text='<%# Eval("po_n") %>'>
						</dx:ASPxHyperLink>
					</DataItemTemplate>
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="1"
					Width="150px">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Date Cut" FieldName="date_cut" VisibleIndex="2"
					Width="150px" UnboundType="DateTime">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Quantity" FieldName="qty" VisibleIndex="3" Width="75px">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="WO#" FieldName="wo_n" VisibleIndex="4" Width="125px">
					<DataItemTemplate>
						<dx:ASPxHyperLink ID="ASPxHyperLink3" runat="server" Target="_blank" NavigateUrl='<%# string.Format("/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}", Eval("woprog_id"), Eval("business_unit_id")) %>'
							Text='<%# Eval("wo_n") %>'>
						</dx:ASPxHyperLink>
					</DataItemTemplate>
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Unit Price" FieldName="price" VisibleIndex="5"
					Width="75px">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Vendor" FieldName="name" VisibleIndex="6">
					<CellStyle HorizontalAlign="Left">
					</CellStyle>
				</dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Qty Rec'd" FieldName="qty_recd" VisibleIndex="7" Width="75px">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
			</Columns>
			<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
			<SettingsText EmptyDataRow="No Information Available" />
			<ClientSideEvents EndCallback="function(s, e) {
	resize();
                please_wait('stop');
}" />
            <SettingsBehavior EnableCustomizationWindow="true"/>
            <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides"/>
		</dx:ASPxGridView>
		<asp:SqlDataSource ID="ds_PO" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
b.poprog_id, 
d.status_type status,
vendor_name(b.poprog_vendor_id) name, 
b.poprog_bvpo po_n, 
DATE_FORMAT(b.poprog_cutdate , '%m/%d/%Y') date_cut, 
(a.po_details_qty_ordered * a.po_details_vendor_qty_per) qty, 
            (a.po_details_qty_received * a.po_details_vendor_qty_per) qty_recd, 
a.po_details_cost / a.po_details_vendor_qty_per price, 
if(a.is_gl_account = false, c.woprog_bvwo, null) wo_n, 
if(a.is_gl_account = false, c.woprog_id, null) woprog_id, 
c.business_unit_id 
FROM 
po_details_current a 
LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id 
LEFT JOIN woprog c ON a.po_details_woprog_id = c.woprog_id AND c.woprog_status IS NOT NULL and a.is_gl_account=false
LEFT JOIN poprog_status d ON b.poprog_status = d.poprog_status_id
WHERE a.po_details_part_no = @master_id AND b.business_unit_id = @business_unit_id ORDER BY po_n DESC
">
			<SelectParameters>
				<asp:QueryStringParameter Name="@master_id" QueryStringField="master_id" />
				<asp:QueryStringParameter Name="@business_unit_id" QueryStringField="business_unit_id" />
			</SelectParameters>
		</asp:SqlDataSource>
		<dx:ASPxCallbackPanel id="cbp_main" runat="server" ClientInstanceName="cbp_main"
			Width="100%">
			<panelcollection>
<dx:PanelContent runat="server"><dx:ASPxCheckBox runat="server" Checked="True" Text="Current WO's" Visible="False" Font-Bold="True" ForeColor="Black" ID="chk_wos">
<ClientSideEvents CheckedChanged="function(s, e) {
	h.Set(&quot;historicWO&quot;, s.GetChecked());
	gv_WO.Refresh();
}"></ClientSideEvents>
</dx:ASPxCheckBox>
 <dx:ASPxHiddenField runat="server" ClientInstanceName="h" ID="h"></dx:ASPxHiddenField>
 <dx:ASPxGridView runat="server" ClientInstanceName="gv_WO" AutoGenerateColumns="False" DataSourceID="ds_WO" Width="100%" ID="gv_WO" OnCustomJSProperties="gv_CustomJSProperties" OnCustomCallback="Load_Layout">
<ClientSideEvents EndCallback="function(s, e) {
	resize();
    please_wait('stop');
}"></ClientSideEvents>
<Columns>
	<dx:GridViewDataTextColumn Caption="WO#" FieldName="wo_n" VisibleIndex="0" Width="125px">
		<DataItemTemplate>
<dx:ASPxHyperLink id="ASPxHyperLink1" runat="server" Target="_blank" Text='<%# Eval("wo_n") %>' NavigateUrl='<%# string.Format("/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}", Eval("woprog_id"), Eval("business_unit_id")) %>'>
						</dx:ASPxHyperLink> 
		</DataItemTemplate>
		<CellStyle HorizontalAlign="Center">
		</CellStyle>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Date Cut" FieldName="date_cut" UnboundType="DateTime"
		VisibleIndex="1" Width="150px">
		<CellStyle HorizontalAlign="Center">
		</CellStyle>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Comm Qty" FieldName="qty_com" VisibleIndex="3"
		Width="75px">
		<CellStyle HorizontalAlign="Center">
		</CellStyle>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Unit Price" FieldName="price" VisibleIndex="4"
		Width="75px">
		<CellStyle HorizontalAlign="Center">
		</CellStyle>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Customer" FieldName="name" VisibleIndex="6">
		<CellStyle HorizontalAlign="Left">
		</CellStyle>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Req Qty" FieldName="qty_req" VisibleIndex="2"
		Width="75px">
	</dx:GridViewDataTextColumn>
</Columns>

<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True"></Settings>

<SettingsText EmptyDataRow="No Information Available"></SettingsText>

<SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides"/>
<SettingsBehavior EnableCustomizationWindow="true"/>

<Styles>

<TitlePanel HorizontalAlign="Left" BackColor="White"></TitlePanel>
</Styles>

<Templates><TitlePanel>
					&nbsp;
				
</TitlePanel>
</Templates>
</dx:ASPxGridView>
 <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
 SELECT * FROM 
	(
	SELECT 
		customer_name(b.woprog_customer_id) name, 
		DATE_FORMAT(b.woprog_cutdatetime , '%m/%d/%Y') date_cut, 
		a.wo_detail_current_qty_committed qty_com, 
		a.wo_detail_current_qty_ordered qty_req,
		a.wo_detail_current_price_sell price, 
		b.woprog_bvwo wo_n, 
		b.woprog_id, 
		b.business_unit_id,
		b.woprog_customer_id customer_id
	FROM wo_detail_current a 
	LEFT JOIN woprog b ON a.wo_detail_current_woprog_id = b.woprog_id 
	WHERE a.wo_detail_current_master_id = @master_id AND b.business_unit_id = @business_unit_id
UNION
	SELECT 
		customer_name(b.woprog_customer_id) name, 
		DATE_FORMAT(b.woprog_cutdatetime , '%m/%d/%Y') date_cut, 
		a.wo_detail_history_qty_committed qty_com, 
		a.wo_detail_history_qty_ordered qty_req,
		a.wo_detail_history_price_sell price, 
		b.woprog_bvwo wo_n, 
		b.woprog_id, 
		b.business_unit_id,
		b.woprog_customer_id customer_id
	FROM wo_detail_history a 
	LEFT JOIN woprog b ON a.wo_detail_history_woprog_id = b.woprog_id 
WHERE a.wo_detail_history_master_id = @master_id AND b.business_unit_id = @business_unit_id
	) x 
ORDER BY wo_n DESC" ID="ds_WO" OnInit="ds_WO_Init"><SelectParameters>
<asp:QueryStringParameter QueryStringField="master_id" Name="@master_id"></asp:QueryStringParameter>
<asp:QueryStringParameter QueryStringField="business_unit_id" Name="@business_unit_id"></asp:QueryStringParameter>
</SelectParameters>
</asp:SqlDataSource>
 </dx:PanelContent>
</panelcollection>
		</dx:ASPxCallbackPanel>
		&nbsp;&nbsp;
    
    </div>
    </form>
</body>
</html>
