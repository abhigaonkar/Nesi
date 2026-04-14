<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="cust_qc" Title="Customer QC" EnableTheming="true" Theme="NETheme01" Codebehind="qc.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<lc:LayoutControl runat="server" id="layout" __is_private="True" ShowExcelExport="True" ShowPDFExport="True" ShowToggle="True" GridviewID="gv_customerqc" />
	<dx:ASPxGridView ID="gv_customerqc" runat="server" AutoGenerateColumns="False" DataSourceID="ds_customerqc" KeyFieldName="customer_id" Width="100%" oncustomcallback="gv_customerqc_CustomCallback" oncustomjsproperties="gv_MasterContacts_CustomJSProperties">
		<Styles GroupButtonWidth="28" >
			<Header SortingImageSpacing="5px" ImageSpacing="5px" HorizontalAlign="Center">
			</Header>
			<Cell Font-Size="12px">
			</Cell>
			<LoadingPanel ImageSpacing="8px">
			</LoadingPanel>
		</Styles>
		<SettingsLoadingPanel ImagePosition="Top"></SettingsLoadingPanel>
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Columns>
			<dx:GridViewDataTextColumn FieldName="customer_id" ReadOnly="True" Width="50px" Caption="ID" VisibleIndex="0">
				<DataItemTemplate>
					<asp:HyperLink ID="cust_link" runat="server" Text='<%# Eval("customer_id") %>' NavigateUrl='<%# string.Format("~/sections/customer/frame.aspx?customer_id={0}", Eval("customer_id")) %>'></asp:HyperLink>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="customer_number" Width="100px" Caption="Customer No" VisibleIndex="1">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<HeaderStyle Wrap="True" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="customer_name" Caption="Customer" VisibleIndex="2">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="phone" Caption="Phone" VisibleIndex="5">
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="ddl_name" Caption="Business Unit" VisibleIndex="6">
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Active WOs" FieldName="active_wo" VisibleIndex="7" Width="50px">
				<HeaderStyle Wrap="True" />
				<CellStyle HorizontalAlign="Center" Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Active Quotes" FieldName="active_quotes" VisibleIndex="8" Width="50px">
				<HeaderStyle Wrap="True" />
				<CellStyle HorizontalAlign="Center" Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="member_init" Caption="Added By" VisibleIndex="9">
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn FieldName="customer_createddatetime" Caption="Added DT" VisibleIndex="10">
				<DataItemTemplate>
					<asp:Label ID="date" runat="server" Text='<%# Eval("customer_createddatetime", "{0:G}") %>'></asp:Label>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="QC Level" FieldName="qc_level" VisibleIndex="4">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="3">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
		</Columns>
		<Paddings Padding="1px"></Paddings>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True"></Settings>
		<StylesEditors>
			<CalendarHeader Spacing="1px">
			</CalendarHeader>
			<ProgressBar Height="29px">
			</ProgressBar>
		</StylesEditors>
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="ds_customerqc" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
	a.customer_id,
	customer_number,
	customer_name customer_name, 
	CAST(CONCAT('(',address_phonearea, ') ', address_phonefirst,'-', address_phonelast) AS CHAR) phone,
	ddl_name,
	customer_createddatetime,
	IF(customer_qc_member_id IS NULL, 'QC LEVEL 1', IF(customer_qc2_member_id IS NULL, 'QC LEVEL 2', '')) qc_level,
	member_name(Customer_InitMember_ID) member_init,
	(SELECT COUNT(*) FROM woprog WHERE woprog_status = 'open' AND woprog_customer_id = a.customer_id) active_wo,
	(SELECT COUNT(*) FROM quote_master WHERE customer_id = a.customer_id AND status_id NOT IN (6,8.9)) active_quotes,
	d.customer_or_contact_status status
FROM 
	customer a
LEFT JOIN 
	address b ON
		b.address_table = 'Customer' AND 
		b.address_table_id = a.customer_id AND 
		b.address_type = 'B'
LEFT JOIN
	business_unit c ON
	a.business_unit_id = c.id
LEFT JOIN
	customer_sales_properties e 
		ON a.customer_id = e.customer_id AND 
		b.address_id = e.address_id
LEFT JOIN
	customer_or_contact_status d ON e.status_id = d.customer_or_contact_status_id 
WHERE 
	(a.customer_qc_member_id IS NULL OR a.customer_qc2_member_id IS NULL) AND 
	a.customer_name NOT LIKE '%custest%'
ORDER BY 
	active_wo, active_quotes ASC"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">
</asp:Content>
