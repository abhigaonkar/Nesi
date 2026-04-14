<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_wo_part_history_index" Title="WO Part History Grid"  EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"	Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:content>
<asp:content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
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
	<lc:layoutcontrol runat="server" id="layout" />
	<dx:aspxgridview id="gv_wo_part_history_grid" runat="server" 
		autogeneratecolumns="False"
		width="100%" ClientInstanceName="gv_wo_part_history_grid" 
		OnCustomJSProperties="gv_wo_part_history_grid_CustomJSProperties" 
		OnCustomCallback="gv_wo_part_history_grid_CustomCallback" 
		OnHtmlDataCellPrepared="gv_wo_part_history_grid_HtmlDataCellPrepared" Theme="NETheme01">

        <Columns>
            <dx:GridViewDataTextColumn Caption="WO" FieldName="wo" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Branch" FieldName="branch" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Department" FieldName="department" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="WO_Status" FieldName="wo_status" VisibleIndex="4">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Master_ID" FieldName="master_id" VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Comm Cost" FieldName="cost" VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Comm Qty" FieldName="qty_comm" VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Total" FieldName="total_cost" VisibleIndex="8">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Description" FieldName="description" VisibleIndex="9">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn Caption="Timestamp" FieldName="ts" VisibleIndex="0">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm:ss">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
        </Columns>

<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" 
			AutoFilterRowInputDelay="6000" EnableCustomizationWindow="True"></SettingsBehavior>

        <SettingsSearchPanel Visible="True" />

<Styles>



<Cell Wrap="False"></Cell>
</Styles>

<SettingsPager PageSize="50" AlwaysShowPager="True"></SettingsPager>
        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="total_cost" ShowInColumn="Total Cost" SummaryType="Sum" />
        </TotalSummary>

<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" 
			ShowGroupPanel="True" ShowFilterBar="Visible" ColumnMinWidth="15" ShowFooter="True"></Settings>
		
        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" 
			CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-Height="250px" 
			CustomizationWindow-HorizontalOffset="5" CustomizationWindow-VerticalOffset="5">
        

<CustomizationWindow Height="250px" HorizontalAlign="LeftSides" VerticalAlign="TopSides" HorizontalOffset="5" VerticalOffset="5"></CustomizationWindow>
		</SettingsPopup>
        

		<ClientSideEvents EndCallback="bind_tooltips" />
</dx:aspxgridview>
	<dx:aspxgridviewexporter ID="gve" runat="server" FileName="PurchaseReport" GridViewID="gv_wo_part_history_grid"
		Landscape="True" BottomMargin="1" LeftMargin="1" RightMargin="1" TopMargin="1">
	</dx:aspxgridviewexporter>
</asp:content>
<asp:content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:content>
<asp:content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:content>


