<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_po_line_grid_index" Title="PO Line Item Grid" EnableTheming="true" Theme="NETheme01" Codebehind="index.aspx.cs" %>

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
	<lc:LayoutControl runat="server" id="layout" />
	<dx:aspxgridview id="gv_po_line_grid" runat="server" 
		autogeneratecolumns="False"
		width="100%" ClientInstanceName="gv_po_line_grid" 
		OnCustomJSProperties="gv_po_line_grid_CustomJSProperties" 
		OnCustomCallback="gv_po_line_grid_CustomCallback" 
		OnHtmlDataCellPrepared="gv_po_line_grid_HtmlDataCellPrepared" 
		Font-Names="Arial" KeyFieldName="id">

<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" 
			AutoFilterRowInputDelay="6000" EnableCustomizationWindow="True"></SettingsBehavior>

        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="{0:N2}" FieldName="ext_cost" ShowInColumn="cost_ext" ShowInGroupFooterColumn="cost_ext" SummaryType="Sum" />
        </TotalSummary>

<Styles>
<Header Font-Bold="True"></Header>

	<RowHotTrack ForeColor="Black">
	</RowHotTrack>

<Cell Wrap="False"></Cell>
	<TitlePanel HorizontalAlign="Left">
		<BackgroundImage ImageUrl="~/images/foundation/bg/bg[main].png" VerticalPosition="bottom" />
	</TitlePanel>
	<GroupPanel Font-Bold="True" ForeColor="White">
		<BackgroundImage ImageUrl="~/images/foundation/bg/bg[header].png" Repeat="RepeatX"
			VerticalPosition="top" />
	</GroupPanel>
</Styles>

<SettingsPager PageSize="50" AlwaysShowPager="True"></SettingsPager>
<Columns>
    <dx:GridViewDataHyperLinkColumn Caption="PO" FieldName="po" VisibleIndex="0">
         <DataItemTemplate>
			<dx:ASPxHyperLink ID="hl_po" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}', 'po', 1200,800)&quot;, Eval(&quot;poid&quot;)) %>"
				Text='<%# Bind("po") %>'>
			</dx:ASPxHyperLink>
        </DataItemTemplate>
    </dx:GridViewDataHyperLinkColumn>
    <dx:GridViewDataTextColumn Caption="Vend Name" FieldName="vendor" 
		VisibleIndex="1">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" VisibleIndex="2">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataHyperLinkColumn Caption="WO" FieldName="wo" VisibleIndex="3">
	<PropertiesHyperLinkEdit NavigateUrlFormatString="" NullDisplayText="">
        </PropertiesHyperLinkEdit>
	</dx:GridViewDataHyperLinkColumn>
    <dx:GridViewDataHyperLinkColumn Caption="Master ID" FieldName="master_id" 
		VisibleIndex="4">
		<PropertiesHyperLinkEdit NavigateUrlFormatString="" NullDisplayText="{0}">
        </PropertiesHyperLinkEdit>
    </dx:GridViewDataHyperLinkColumn>
    <dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
		VisibleIndex="5">
        <DataItemTemplate>
			<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text='<%# Eval("description") %>'
				Width="100%" OnDataBound="header_Init">
			</dx:ASPxLabel>
        </DataItemTemplate>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Vendor No" FieldName="vendor_partno" 
		VisibleIndex="6">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Qty On Order" FieldName="qty_onorder" 
		VisibleIndex="7">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Rec'd to Date" FieldName="rec_to_date"
        VisibleIndex="8">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" VisibleIndex="9">
        <DataItemTemplate>
			<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text='<%# Eval("notes") %>'
				Width="100%" OnDataBound="header_Init">
			</dx:ASPxLabel>
        </DataItemTemplate>
        <HeaderTemplate>
			<dx:ASPxLabel ID="ASPxLabel1" runat="server" OnInit="header_Init" Text="Notes" Width="100%">
			</dx:ASPxLabel>
        </HeaderTemplate>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataDateColumn Caption="Date Req." VisibleIndex="10" 
		Visible="False">
        <PropertiesDateEdit DisplayFormatString="MM/dd/yy">
        </PropertiesDateEdit>
    </dx:GridViewDataDateColumn>
    <dx:GridViewDataTextColumn Caption="PO Status" FieldName="po_status" 
		VisibleIndex="11">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataDateColumn Caption="PO Exp. Date" FieldName="date_expected" 
		VisibleIndex="12">
        <PropertiesDateEdit DisplayFormatString="MM/dd/yy">
        </PropertiesDateEdit>
    </dx:GridViewDataDateColumn>
    <dx:GridViewDataTextColumn Caption="Days Left"
        VisibleIndex="13" FieldName="daysleft">
    </dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="woid" FieldName="woid" Visible="False" 
		VisibleIndex="22">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="business_unit_id" FieldName="business_unit_id" 
		Visible="False" VisibleIndex="23">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="poid" FieldName="poid" Visible="False" 
		VisibleIndex="20">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="vendorid" FieldName="vendorid" 
		Visible="False" VisibleIndex="18">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Active" FieldName="active" 
		VisibleIndex="14">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Purchaser" FieldName="membername" 
		VisibleIndex="15">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="id" FieldName="id" Visible="False" 
		VisibleIndex="17">
	</dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Cost" FieldName="cost" Visible="False" 
		VisibleIndex="19">
	</dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="cost_ext" FieldName="ext_cost" Visible="False" 
		VisibleIndex="21">
	</dx:GridViewDataTextColumn>
</Columns>

<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowGroupPanel="True" ShowFilterBar="Visible" ShowFooter="True"></Settings>

        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-Height="250px" CustomizationWindow-HorizontalOffset="5" CustomizationWindow-VerticalOffset="5">

<CustomizationWindow Height="250px" HorizontalAlign="LeftSides" VerticalAlign="TopSides" HorizontalOffset="5" VerticalOffset="5"></CustomizationWindow>
        </SettingsPopup>
        
        <SettingsLoadingPanel Mode="Disabled" />
		<ClientSideEvents EndCallback="function(s,e){ bind_tooltips(); please_wait('stop');}" BeginCallback="function(s,e){ please_wait('start');}" />
</dx:aspxgridview>
	<dx:ASPxGridViewExporter ID="gve" runat="server" FileName="POReport" GridViewID="gv_po_line_grid"
		Landscape="True" BottomMargin="1" LeftMargin="1" RightMargin="1" TopMargin="1">
	</dx:ASPxGridViewExporter>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

