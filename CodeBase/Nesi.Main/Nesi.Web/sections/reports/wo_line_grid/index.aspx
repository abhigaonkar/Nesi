<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_wo_line_grid_index" Title="WO Line Item Grid"  EnableTheming="True" Codebehind="index.aspx.cs" %>

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
	<dx:aspxgridview id="gv_wo_line_grid" runat="server" 
		autogeneratecolumns="False" 
		width="100%" ClientInstanceName="gv_wo_line_grid" 
		OnCustomJSProperties="gv_wo_line_grid_CustomJSProperties" 
		OnCustomCallback="gv_wo_line_grid_CustomCallback" 
		OnHtmlDataCellPrepared="gv_wo_line_grid_HtmlDataCellPrepared" Theme="NETheme01">

<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" 
			AutoFilterRowInputDelay="6000" EnableCustomizationWindow="True"></SettingsBehavior>

        <SettingsSearchPanel Visible="True" />

<Styles>



<Cell Wrap="False"></Cell>
</Styles>

<SettingsPager PageSize="50" AlwaysShowPager="True"></SettingsPager>
        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="total_cost" ShowInColumn="Total Cost" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="total_sell" ShowInColumn="Total Sell" SummaryType="Sum" />
        </TotalSummary>
<Columns>
    <dx:GridViewDataTextColumn Caption="WO" FieldName="wo" VisibleIndex="0" unboundtype="String">
        <DataItemTemplate>
			<dx:ASPxHyperLink ID="hl" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}', 'wo', 1200,800)&quot;, Eval(&quot;wo_detail_current_woprog_id&quot;)) %>"
				Text='<%# Eval("wo") %>'>
			</dx:ASPxHyperLink>
        </DataItemTemplate>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Cust Name" FieldName="customer" VisibleIndex="1">
        <DataItemTemplate>
			<dx:ASPxHyperLink ID="hl" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/customer/frame.aspx?customer_id={0}', 'customer', 1200,800)&quot;, Eval(&quot;customer_id&quot;)) %>"
				Text='<%# Eval("customer") %>'>
			</dx:ASPxHyperLink>
        </DataItemTemplate>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="PM" FieldName="PM" VisibleIndex="2">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="WO Status" FieldName="wo_status" VisibleIndex="4">
        <Settings HeaderFilterMode="CheckedList" />
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Master ID" FieldName="master_id" VisibleIndex="5">
        <DataItemTemplate>
			<dx:ASPxHyperLink ID="hl" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/member/inventory/index.aspx?a=get&id={0}&tab=G', 'inventory', 1200,800)&quot;, Eval(&quot;master_id&quot;)) %>"
				Text='<%# Eval("master_id") %>'>
			</dx:ASPxHyperLink>
        </DataItemTemplate>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Description" FieldName="description" VisibleIndex="6">
        <Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
        <DataItemTemplate>
			<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text='<%# Eval("description") %>'
				Width="100%" OnDataBound="header_Init">
			</dx:ASPxLabel>
        </DataItemTemplate>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Qty Req" FieldName="qty_required" VisibleIndex="7">
        <CellStyle BackColor="#C0FFC0">
		</CellStyle>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Qty Cmtd." FieldName="qty_committed" VisibleIndex="8">
        <CellStyle BackColor="#FFC0C0">
		</CellStyle>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Qty Unfulfilled" FieldName="qty_unfulfilled"
        VisibleIndex="9">
        <CellStyle BackColor="#FFFFCC">
		</CellStyle>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Qty Avail Internal" FieldName="int_onhand_qty" VisibleIndex="10">
        <CellStyle Font-Bold="True">
		</CellStyle>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Qty On PO's Not Received" FieldName="not_received"
        VisibleIndex="11">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" VisibleIndex="12">
        <DataItemTemplate>
			<dx:ASPxLabel ID="ASPxLabel2" runat="server" Text='<%# Eval("notes") %>'
				Width="100%" OnDataBound="header_Init">
			</dx:ASPxLabel>
        </DataItemTemplate>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataDateColumn Caption="Date Req." FieldName="date_required" VisibleIndex="13">
        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
        </PropertiesDateEdit>
    </dx:GridViewDataDateColumn>
    <dx:GridViewDataDateColumn Caption="Date Rec." FieldName="date_received" VisibleIndex="14">
        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
        </PropertiesDateEdit>
    </dx:GridViewDataDateColumn>
    <dx:GridViewDataTextColumn Caption="PO" FieldName="po" VisibleIndex="15">
        <DataItemTemplate>
			<dx:ASPxHyperLink ID="hl_po" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}', 'po', 1200,800)&quot;, Eval(&quot;poprog_id&quot;)) %>"
				Text='<%# Bind("po") %>'>
			</dx:ASPxHyperLink>
        </DataItemTemplate>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="PO Status" FieldName="po_status" VisibleIndex="16">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataDateColumn Caption="PO Exp. Date" FieldName="date_expected" VisibleIndex="17">
        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
        </PropertiesDateEdit>
    </dx:GridViewDataDateColumn>
    <dx:GridViewDataTextColumn Caption="Still Not Ordered" FieldName="StillNotOrdered"
        VisibleIndex="18">
        <CellStyle Font-Bold="True">
		</CellStyle>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Days Left" FieldName="DaysLeft" VisibleIndex="19">
    </dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Qty Avail External" FieldName="ext_onhand_qty" VisibleIndex="20">
        <CellStyle Font-Bold="True">
		</CellStyle>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Cost" FieldName="cost" VisibleIndex="21" Visible="false">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Sell" FieldName="sell" VisibleIndex="22" Visible="false">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Billtype" FieldName="billtype_id" VisibleIndex="23" Visible="false">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Quote ID" FieldName="quote_id" VisibleIndex="24" Visible="false">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Is Consumable?" FieldName="is_consumable" VisibleIndex="25" Visible="false">
    </dx:GridViewDataTextColumn>
     <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" VisibleIndex="26" Visible="false">
         <Settings HeaderFilterMode="CheckedList" />
    </dx:GridViewDataTextColumn>
     <dx:GridViewDataTextColumn Caption="Total Cost" FieldName="total_cost" VisibleIndex="27" Visible="false">
    </dx:GridViewDataTextColumn>
     <dx:GridViewDataTextColumn Caption="Total Sell" FieldName="total_sell" VisibleIndex="28" Visible="false">
    </dx:GridViewDataTextColumn>
	<dx:gridviewdatatextcolumn caption="Job Type" fieldname="job_type" visible="False" visibleindex="29">
	</dx:gridviewdatatextcolumn>
    <dx:GridViewDataTextColumn Caption="Billtype" FieldName="wo_lineitem_billtype_name" MinWidth="20" VisibleIndex="30" Width="50px">
    </dx:GridViewDataTextColumn>
</Columns>

<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" 
			ShowGroupPanel="True" ShowFilterBar="Visible" ColumnMinWidth="15" ShowFooter="True"></Settings>
		
        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" 
			CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-Height="250px" 
			CustomizationWindow-HorizontalOffset="5" CustomizationWindow-VerticalOffset="5">
        

<CustomizationWindow Height="250px" HorizontalAlign="LeftSides" VerticalAlign="TopSides" HorizontalOffset="5" VerticalOffset="5"></CustomizationWindow>
		</SettingsPopup>
        <SettingsLoadingPanel Mode="Disabled" />
		<ClientSideEvents EndCallback="function(s,e){ bind_tooltips(); please_wait('stop');}" BeginCallback="function(s,e){ please_wait('start');}" />
</dx:aspxgridview>

	<dx:aspxgridviewexporter ID="gve" runat="server" FileName="PurchaseReport" GridViewID="gv_wo_line_grid"
		Landscape="True" BottomMargin="1" LeftMargin="1" RightMargin="1" TopMargin="1">
	</dx:aspxgridviewexporter>
</asp:content>
<asp:content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:content>
<asp:content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:content>


