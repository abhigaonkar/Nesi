<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_customer_rates_index" Theme="NETheme01" EnableTheming="true" Title="Customer Rates" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"	Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script>
		function bind_tooltips()
			{
			$(".ttip").each(function()
				{
				$(this).tip();
				});
			}
		$(document).ready(function()
			{
			bind_tooltips();
			});
	</script>
	<br />
	<lc:LayoutControl runat="server" id="layout"  GridviewID="gv_customer_rates" ShowExcelExport="True" ShowPDFExport="True" ShowToggle="True" />
	<dx:ASPxGridView ID="gv_customer_rates" runat="server" 
		AutoGenerateColumns="False" ClientInstanceName="gv_customer_rates" Width="100%"  EnableCallBacks="True" Font-Names="Arial"  oncustomcallback="gv_CustomCallback"
		onhtmlrowcreated="gv_customer_rates_HtmlRowCreated" 
		onhtmldatacellprepared="gv_customer_rates_HtmlDataCellPrepared">
		<Columns>
			<dx:GridViewCommandColumn Caption=" " VisibleIndex="0" Width="1px" ShowClearFilterButton="true">
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" 
				VisibleIndex="1" Width="85px" MinWidth="10">
			    
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Customer" VisibleIndex="2" 
				FieldName="customer_name" Width="100%" MinWidth="10">
				<Settings FilterMode="DisplayText" />
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Date Set" VisibleIndex="3" 
				FieldName="last_updated" Width="100px" MinWidth="10">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataComboBoxColumn Caption="Rate Type" FieldName="membertype_id" 
				VisibleIndex="4" Width="150px">
				<PropertiesComboBox DataSourceID="sqlmembertype" TextField="name" 
					ValueField="id" ValueType="System.Int32">
				</PropertiesComboBox>
				<Settings HeaderFilterMode="CheckedList" />
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="Normal" FieldName="normal" VisibleIndex="5" 
				Width="70px">
				<PropertiesTextEdit DisplayFormatString="N2">
				</PropertiesTextEdit>
				<Settings AutoFilterCondition="GreaterOrEqual" />
				<CellStyle Font-Bold="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Rate" VisibleIndex="6" 
				FieldName="chargeout" Width="70px">
				<PropertiesTextEdit DisplayFormatString="N2">
				</PropertiesTextEdit>
				<Settings AutoFilterCondition="LessOrEqual" />
				<CellStyle BackColor="#FFEAEA" Font-Bold="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="From Date" FieldName="from_date" 
				VisibleIndex="7" Width="105px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<Settings AutoFilterCondition="LessOrEqual" />
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataDateColumn Caption="To Date" FieldName="to_date" 
				VisibleIndex="8" Width="105px" SortIndex="0" SortOrder="Ascending">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<Settings AutoFilterCondition="GreaterOrEqual" />
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			
			
		    <dx:GridViewDataTextColumn Caption="Customer ID" FieldName="customer_id" VisibleIndex="9">
            </dx:GridViewDataTextColumn>
			
			
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
			ShowGroupPanel="True" ShowHeaderFilterButton="True" />
	</dx:ASPxGridView>
	<dx:ASPxGridViewExporter ID="gve" runat="server" FileName="POReport" GridViewID="gv_customer_rates"
		Landscape="True" BottomMargin="1" LeftMargin="1" RightMargin="1" TopMargin="1">
	</dx:ASPxGridViewExporter>
	<asp:SqlDataSource ID="sqlmembertype" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="SELECT
membertype.MemberType_ID id,
membertype.MemberType_Name name
FROM
membertype">
	</asp:SqlDataSource>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

