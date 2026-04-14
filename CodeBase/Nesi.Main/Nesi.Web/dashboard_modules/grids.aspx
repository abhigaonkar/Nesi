<%@ Page Language="C#" AutoEventWireup="true" Theme="" Inherits="dashboard_modules_grids" Codebehind="grids.aspx.cs" %>
<%@ OutputCache Duration="60" VaryByParam="business_unit_id" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link type="text/css" href="/css/dashboard.css" rel="Stylesheet" />
</head>
<body>
	<script type="text/javascript" src="/js/functions.js"></script>
    <form id="form1" runat="server">
    <div>
		<table cellpadding="3" cellspacing="0" width="100%">
			<tr>
				<td width="33%" valign="top">
    	<dx:ASPxGridView ID="gv_worst" DataSourceID="ds_worst" runat="server" Width="100%" AutoGenerateColumns="False">
			<Columns>
				<dx:GridViewDataTextColumn Caption="Customer" FieldName="customer" VisibleIndex="0" Width="15%">
					<DataItemTemplate>
						<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/customer/index.aspx?customer_id={0}', 'customer', 1035,800)&quot;, Eval(&quot;customer_id&quot;)) %>"  Text='<%# Eval("customer") %>' Font-Size="11px"/>
					</DataItemTemplate>
					<CellStyle Wrap="False" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="WO #" FieldName="wo" VisibleIndex="1" Width="8%">
					<DataItemTemplate>
						<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/workorder/index.aspx?woprog_id={0}', 'wo', 1035,800)&quot;, Eval(&quot;woprog_id&quot;)) %>"  Text='<%# Eval("wo") %>' Font-Size="11px"/>
					</DataItemTemplate>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Margin" FieldName="thismargin" VisibleIndex="2" Width="10%">
					<PropertiesTextEdit DisplayFormatString="C0">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Lab Sell" FieldName="labor_sell" VisibleIndex="4" Width="10%">
					<PropertiesTextEdit DisplayFormatString="C0">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Lab Cost" FieldName="labor_cost" VisibleIndex="3" Width="10%">
					<PropertiesTextEdit DisplayFormatString="C0">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="matcost" ShowInCustomizationForm="True" Width="10%" Caption="Mat. Cost" VisibleIndex="5">
<PropertiesTextEdit DisplayFormatString="C0"></PropertiesTextEdit>

<CellStyle HorizontalAlign="Center" Font-Size="11px">
<Paddings Padding="2px"></Paddings>
</CellStyle>
</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Mat. Sell" FieldName="matsell" VisibleIndex="6" Width="10%">
					<PropertiesTextEdit DisplayFormatString="C0">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
			</Columns>
			<SettingsBehavior ColumnResizeMode="Control" />
			<Settings ShowTitlePanel="True" />
			<SettingsText Title="5 Worst Jobs (based on $ margin)" />
			<Styles>
				<Header Font-Bold="True" Font-Size="10px" HorizontalAlign="Center">
				</Header>
				<TitlePanel BackColor="#009900" Font-Bold="True" Font-Size="12px">
				</TitlePanel>
			</Styles>
		</dx:ASPxGridView>
    
	<asp:SqlDataSource ID="ds_worst" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
c.customer,
CONCAT('0',(c.wo/1)) wo,
c.customer_id customer_id,
c.woprog_id,
c.invoiced,
ROUND((c.matsell+c.labor_sell) - (c.matcost+c.labor_cost), 2) thismargin,
c.labor_cost,
c.labor_sell,
c.matcost,
c.matsell
FROM
	(
	SELECT 
	  a.woprog_customername customer,
	  a.woprog_bvwo wo,
	  a.woprog_customer_id customer_id,
	  a.woprog_id,
	  a.woprog_grossmargin margin, 
	  ROUND(a.woprog_stilltobebilled		+ (SELECT IFNULL(SUM(b.woprog_stilltobebilled), 0) 		FROM woprog b WHERE b.woprog_associate_woprog_id = a.woprog_id), 2) invoiced, 
	  ROUND(a.woprog_laborcost 			+ (SELECT IFNULL(SUM(b.woprog_laborcost), 0) 			FROM woprog b WHERE b.woprog_associate_woprog_id = a.woprog_id), 2) labor_cost,
	  ROUND(a.woprog_labourtotalsell 		+ (SELECT IFNULL(SUM(b.woprog_labourtotalsell), 0) 		FROM woprog b WHERE b.woprog_associate_woprog_id = a.woprog_id), 2) labor_sell, 
	  ROUND(a.woprog_materialcost 		+ (SELECT IFNULL(SUM(b.woprog_materialcost), 0) 		FROM woprog b WHERE b.woprog_associate_woprog_id = a.woprog_id), 2) matcost, 
	  ROUND(a.woprog_materialtotalsell 	+ (SELECT IFNULL(SUM(b.woprog_materialtotalsell), 0) 	FROM woprog b WHERE b.woprog_associate_woprog_id = a.woprog_id), 2) matsell
	FROM 
	  woprog a 
	WHERE 
	  a.business_unit_id = ?company and 
	  a.woprog_department = IF(?division = 1, a.woprog_department, ?division) and 
	  a.woprog_status = 'Open' and 
	  a.woprog_cutdatetime &gt;= DATE_SUB(now(), interval 6 month) and 
	  a.woprog_description not like '%progress%' AND 
	  a.woprog_customername NOT LIKE 'New Electric%' AND
	  a.woprog_iscredit = 0 AND 
	  a.woprog_isrebill = 0 AND
	  a.woprog_grossmargin &gt; 0
	 ) c
WHERE 
	ROUND(((c.matsell - c.matcost) / c.matsell), 2) != 0
order by 
 thismargin asc 
limit 5">
		<SelectParameters>
			<asp:SessionParameter Name="company" SessionField="dashboard_business_unit_id" />
			<asp:SessionParameter Name="division" SessionField="dashboard_division_id" />
		</SelectParameters>
	</asp:SqlDataSource></td>
				<td width="33%" valign="top"><dx:ASPxGridView ID="gv_largest" DataSourceID="ds_largest" runat="server" Width="100%" AutoGenerateColumns="False">
			<Columns>
				<dx:GridViewDataTextColumn Caption="Customer" FieldName="customer" VisibleIndex="0" Width="45%">
					<DataItemTemplate>
						<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/customer/index.aspx?customer_id={0}', 'customer', 1035,800)&quot;, Eval(&quot;customer_id&quot;)) %>"  Text='<%# Eval("customer") %>' Font-Size="11px"/>
					</DataItemTemplate>
					<CellStyle Wrap="False" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="WO #" FieldName="wo" VisibleIndex="1" Width="15%">
					<DataItemTemplate>
						<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/workorder/index.aspx?woprog_id={0}', 'wo', 1035,800)&quot;, Eval(&quot;woprog_id&quot;)) %>"  Text='<%# Eval("wo") %>' Font-Size="11px"/>
					</DataItemTemplate>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Revenue" FieldName="revenue" VisibleIndex="3" Width="25%">
					<PropertiesTextEdit DisplayFormatString="C0">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
			</Columns>
			<SettingsBehavior ColumnResizeMode="Control" />
			<Settings ShowTitlePanel="True" />
			<SettingsText Title="5 Largest Open Jobs (By Revenue)" />
			<Styles>
				<Header Font-Bold="True" Font-Size="10px" HorizontalAlign="Center">
				</Header>
				<TitlePanel BackColor="#009900" Font-Bold="True" Font-Size="12px">
				</TitlePanel>
			</Styles>
		</dx:ASPxGridView>
    
	<asp:SqlDataSource ID="ds_largest" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
  a.woprog_customername customer,
  a.woprog_customer_id customer_id,
  a.woprog_id,
  CONCAT('0',(a.woprog_bvwo/1)) wo,
  INVOICED_GET_NETTOTAL(a.woprog_id)  revenue
FROM 
  woprog a 
LEFT JOIN
  business_unit b 
	ON a.business_unit_id = b.id
WHERE 
	a.business_unit_id = ?company and 
	a.woprog_department = IF(?division = 1, a.woprog_department, ?division) and 
	a.woprog_status = 'Open' and 
	a.woprog_cutdatetime &gt;= ?fiscal_start  and 
	a.woprog_description NOT LIKE '%progress%' AND
	a.woprog_description NOT LIKE '%Down Payment%' AND 
	a.woprog_iscredit = FALSE AND 
	a.woprog_isrebill = FALSE
ORDER BY 
  revenue DESC 
limit 5">
		<SelectParameters>
			<asp:SessionParameter Name="company" SessionField="dashboard_business_unit_id" />
			<asp:SessionParameter Name="division" SessionField="dashboard_division_id" />
			<asp:Parameter Name="fiscal_start" />
		</SelectParameters>
	</asp:SqlDataSource></td>
				<td width="33%" valign="top"><dx:ASPxGridView ID="gv_margin" DataSourceID="ds_margin" runat="server" Width="100%" AutoGenerateColumns="False">
			<Columns>
				<dx:GridViewDataTextColumn Caption="Customer" FieldName="customer" VisibleIndex="0" Width="15%">
					<DataItemTemplate>
						<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/customer/index.aspx?customer_id={0}', 'customer', 1035,800)&quot;, Eval(&quot;customer_id&quot;)) %>"  Text='<%# Eval("customer") %>' Font-Size="11px"/>
					</DataItemTemplate>
					<CellStyle Wrap="False" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="WO #" FieldName="wo" VisibleIndex="1" Width="8%">
					<DataItemTemplate>
						<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/workorder/index.aspx?woprog_id={0}', 'wo', 1035,800)&quot;, Eval(&quot;woprog_id&quot;)) %>"  Text='<%# Eval("wo") %>' Font-Size="11px"/>
					</DataItemTemplate>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Margin" FieldName="thismargin" VisibleIndex="2" Width="10%">
					<PropertiesTextEdit DisplayFormatString="C0">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Lab Sell" FieldName="labor_sell" VisibleIndex="4" Width="10%">
					<PropertiesTextEdit DisplayFormatString="C0">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Lab Cost" FieldName="labor_cost" VisibleIndex="3" Width="10%">
					<PropertiesTextEdit DisplayFormatString="C0">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="matcost" ShowInCustomizationForm="True" Width="10%" Caption="Mat. Cost" VisibleIndex="5">
<PropertiesTextEdit DisplayFormatString="C0"></PropertiesTextEdit>

<CellStyle HorizontalAlign="Center" Font-Size="11px">
<Paddings Padding="2px"></Paddings>
</CellStyle>
</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Mat. Sell" FieldName="matsell" VisibleIndex="6" Width="10%">
					<PropertiesTextEdit DisplayFormatString="C0">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center" Font-Size="11px">
						<Paddings Padding="2px" />
					</CellStyle>
				</dx:GridViewDataTextColumn>
			</Columns>
			<SettingsBehavior ColumnResizeMode="Control" />
			<Settings ShowTitlePanel="True" />
			<SettingsText Title="5 Best Jobs (based on $ margin)" />
			<Styles>
				<Header Font-Bold="True" Font-Size="10px" HorizontalAlign="Center">
				</Header>
				<TitlePanel BackColor="#009900" Font-Bold="True" Font-Size="12px">
				</TitlePanel>
			</Styles>
		</dx:ASPxGridView>
    
	<asp:SqlDataSource ID="ds_margin" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
c.customer,
CONCAT('0',(c.wo/1)) wo,
c.customer_id customer_id,
c.woprog_id,
c.invoiced,
ROUND((c.matsell+c.labor_sell) - (c.matcost+c.labor_cost), 2) thismargin,
c.labor_cost,
c.labor_sell,
c.matcost,
c.matsell
FROM
	(
	SELECT 
	  a.woprog_customername customer,
	  a.woprog_bvwo wo,
	  a.woprog_customer_id customer_id,
	  a.woprog_id,
	  a.woprog_grossmargin margin, 
	  ROUND(a.woprog_stilltobebilled		+ (SELECT IFNULL(SUM(b.woprog_stilltobebilled), 0) 		FROM woprog b WHERE b.woprog_associate_woprog_id = a.woprog_id), 2) invoiced, 
	  ROUND(a.woprog_laborcost 			+ (SELECT IFNULL(SUM(b.woprog_laborcost), 0) 			FROM woprog b WHERE b.woprog_associate_woprog_id = a.woprog_id), 2) labor_cost,
	  ROUND(a.woprog_labourtotalsell 		+ (SELECT IFNULL(SUM(b.woprog_labourtotalsell), 0) 		FROM woprog b WHERE b.woprog_associate_woprog_id = a.woprog_id), 2) labor_sell, 
	  ROUND(a.woprog_materialcost 		+ (SELECT IFNULL(SUM(b.woprog_materialcost), 0) 		FROM woprog b WHERE b.woprog_associate_woprog_id = a.woprog_id), 2) matcost, 
	  ROUND(a.woprog_materialtotalsell 	+ (SELECT IFNULL(SUM(b.woprog_materialtotalsell), 0) 	FROM woprog b WHERE b.woprog_associate_woprog_id = a.woprog_id), 2) matsell
	FROM 
	  woprog a 
	WHERE 
	  a.business_unit_id = ?company and 
	  a.woprog_department =  IF(?division = 1, a.woprog_department, ?division) and 
	  a.woprog_status = 'Open' and 
	  a.woprog_cutdatetime &gt;= DATE_SUB(now(), interval 6 month) and 
	  a.woprog_description not like '%progress%' AND 
	  a.woprog_customername NOT LIKE 'New Electric%' AND
	  a.woprog_iscredit = 0 AND 
	  a.woprog_isrebill = 0 AND
	  a.woprog_grossmargin &gt; 0
	 ) c
WHERE 
	ROUND(((c.matsell - c.matcost) / c.matsell), 2) != 0
order by 
 thismargin DESC
limit 5">
		<SelectParameters>
			<asp:SessionParameter Name="company" SessionField="dashboard_business_unit_id" />
			<asp:SessionParameter Name="division" SessionField="dashboard_division_id" />
		</SelectParameters>
	</asp:SqlDataSource></td>
			</tr>
		</table>
    </div>
    </form>
</body>
</html>
