<%@ Page Language="C#" AutoEventWireup="true" Theme="" Inherits="dashboard_modules_10worstcustomers" Codebehind="10worstcustomers.aspx.cs" %>
<%@ OutputCache Duration="60" VaryByParam="business_unit_id" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
	<link type="text/css" href="/css/dashboard.css" rel="Stylesheet" />
</head>
<body>
	<script type='text/javascript' src='/js/jquery-1.3.2.min.js'></script>
	<script type="text/javascript" src="/js/dashboard.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
    <form id="form1" runat="server">
    <div>
    
    	<dx:ASPxGridView ID="gv_worst" DataSourceID="ds_worst" runat="server" Width="100%" AutoGenerateColumns="False">
			<Columns>
				<dx:GridViewDataTextColumn Caption="Customer" FieldName="name" VisibleIndex="1" Width="68%">
					<DataItemTemplate>
						<dx:ASPxHyperLink ID="ASPxHyperLink1" ForeColor="Maroon" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/customer/index.aspx?customer_id={0}', 'customer', 1035,800)&quot;, Eval(&quot;id&quot;)) %>"  Text='<%# HttpUtility.UrlDecode(Eval("name").ToString()) %>'/>
					</DataItemTemplate>
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Avg DSO" VisibleIndex="4" Width="10%" FieldName="dso">
					<PropertiesTextEdit DisplayFormatString="N0">
					</PropertiesTextEdit>
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Customer #" FieldName="number" VisibleIndex="0" Width="10%">
					<DataItemTemplate>
						<dx:ASPxHyperLink ID="ASPxHyperLink1" ForeColor="Maroon" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:toggle_pane(null,'worstcustomers|{0}')&quot;, Eval(&quot;id&quot;)) %>"  Text='<%# Eval("number") %>'/>
					</DataItemTemplate>
					<CellStyle Font-Bold="True" HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Days Credit" VisibleIndex="2" FieldName="days_credit" Width="10%">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Acct Mgr" VisibleIndex="3" FieldName="acct_mgr" Width="10%">
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
			</Columns>
			<SettingsBehavior ColumnResizeMode="Control" />
			<Settings ShowTitlePanel="True" />
			<SettingsText Title="Top 10 Worst Customers (based on A/R)" />
			<Styles>
				<Header Font-Bold="True" Font-Size="10px" HorizontalAlign="Center">
				</Header>
				<TitlePanel BackColor="#990000" Font-Bold="True" Font-Size="12px">
				</TitlePanel>
			</Styles>
		</dx:ASPxGridView>
    
	<asp:SqlDataSource ID="ds_worst" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT
	a.woprog_customer_id id,
	b.customer_number number,
	b.customer_name name,
	MEMBER_NAME(b.customer_account_manager) acct_mgr,
	b.customer_creditdays days_credit,
	AVG(DATEDIFF(a.woprog_invoice_paid_date, a.woprog_invoicedate)) dso
FROM
	woprog a
LEFT JOIN
	customer b
         ON a.woprog_customer_id = b.customer_id
WHERE
	a.business_unit_id = ?company AND
	a.woprog_invoice_paid_date IS NOT NULL AND
	a.woprog_invoicedate IS NOT NULL AND
	a.woprog_status = 'Invoiced' AND
	a.woprog_iscredit = FALSE AND
	a.woprog_isrebill = FALSE AND
	b.customer_status NOT IN (4,5,6) AND
	b.customer_name NOT LIKE '%NEW%ELECTRIC%' AND
	(SELECT COUNT(woprog_id) FROM woprog USE INDEX (customer_status) WHERE woprog_customer_id = a.woprog_customer_id AND woprog_status = 'Open') > 0 AND
	a.woprog_closedatetime > DATE_SUB(NOW(), INTERVAL 1 YEAR)
GROUP BY
	a.woprog_customer_id
order by
	dso desc limit 10">
		<SelectParameters>
			<asp:SessionParameter Name="company" SessionField="dashboard_business_unit_id" />
		</SelectParameters>
	</asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
