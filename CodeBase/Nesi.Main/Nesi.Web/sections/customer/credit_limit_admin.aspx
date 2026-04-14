<%@ Page Title="" Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_customer_credit_limit_admin" Codebehind="credit_limit_admin.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header_placeholder" Runat="Server">
<div id="divSide" runat="server">
    <asp:SqlDataSource ID="Users" runat="server"></asp:SqlDataSource>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
<div id="divMenu" runat="server"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<lc:LayoutControl runat="server" id="layout" __is_private="True" GridviewID="gv_credit_limit" />
	<dx:ASPxGridView ID="gv_credit_limit" runat="server" AutoGenerateColumns="False" DataSourceID="ds_credit_limit" KeyFieldName="id" Width="100%" oncustomcallback="gv_credit_limit_CustomCallback" oncustomjsproperties="gv_credit_limit_CustomJSProperties">
		<Columns>
			<dx:GridViewDataTextColumn FieldName="number" VisibleIndex="1" Caption="Number" Width="75px">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="hl_cust" runat="server" NavigateUrl='<%# "/redir.aspx?url=" + HttpUtility.UrlEncode(string.Format("/sections/customer/frame.aspx?customer_id={0}", Eval("id"))) %>' Target="_blank" Text='<%# Eval("number") %>' />
				</DataItemTemplate>
				<HeaderStyle HorizontalAlign="Center" />
				<CellStyle Font-Bold="True" HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="0" Visible="false">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="name" VisibleIndex="2" Caption="Name">
				<HeaderStyle HorizontalAlign="Left" />
				<CellStyle Font-Bold="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn FieldName="qc1_datetime" VisibleIndex="4" Caption="QC1 Date" Width="100px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<HeaderStyle HorizontalAlign="Center" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataDateColumn FieldName="qc2_datetime" VisibleIndex="5" Caption="QC2 Date" Width="100px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<HeaderStyle HorizontalAlign="Center" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn FieldName="status" VisibleIndex="3" Caption="Status" Width="75px">
				<HeaderStyle HorizontalAlign="Center" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="credit_limit" VisibleIndex="6" Caption="Credit Limit" Width="100px">
				<PropertiesTextEdit DisplayFormatString="C0">
				</PropertiesTextEdit>
				<HeaderStyle HorizontalAlign="Center" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Expected Balance (WO)" FieldName="total_expected_balance_wo" VisibleIndex="7" Width="150px">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
				<HeaderStyle HorizontalAlign="Center" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Expected Balance (Quote)" FieldName="total_expected_balance_quote" VisibleIndex="8" Width="150px">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
				<HeaderStyle HorizontalAlign="Center" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Typical Payment Days" VisibleIndex="9" Width="100px" FieldName="typical_payment_days">
				<HeaderStyle HorizontalAlign="Center" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Max Overages (Past 6mo)" VisibleIndex="10" Width="100px" FieldName="max_overages">
				<HeaderStyle HorizontalAlign="Center" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior EnableCustomizationWindow="True" EnableRowHotTrack="True" />
		<SettingsPager PageSize="35">
		</SettingsPager>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="ds_credit_limit" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand=" 
SELECT 
	a.customer_id id, 
	a.customer_number_int number, 
	URLDECODE(a.customer_name) name, 
	a.customer_qc_datetime qc1_datetime, 
	a.customer_qc2_datetime qc2_datetime, 
	b.customer_or_contact_status status, 
	a.customer_creditlimit credit_limit,
	IFNULL(SUM(c.woprog_stilltobebilled),0) total_expected_balance_wo,
	IFNULL(SUM(d.quoted_price),0) total_expected_balance_quote,
	0 typical_payment_days,
	0 max_overages
FROM 
	customer a
LEFT JOIN
	customer_or_contact_status b ON
		a.customer_status = b.customer_or_contact_status_id
LEFT JOIN
	woprog c ON
		c.woprog_customer_id = a.customer_id AND
		c.woprog_status = &quot;Open&quot; AND
		c.business_unit_id != 8
LEFT JOIN
	quote_master d ON
		d.customer_id = a.customer_id AND
		d.status_id &lt;= 4 AND
		d.business_unit_id != 8
GROUP BY
	a.customer_id"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

