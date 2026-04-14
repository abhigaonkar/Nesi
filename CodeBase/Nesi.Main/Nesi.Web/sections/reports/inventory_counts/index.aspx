<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_inventory_counts_index" Title="Inventory Counts Report" EnableTheming="true" Theme="NETheme01" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>









<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<lc:LayoutControl runat="server" id="layout" GridviewID="gv_counts" />
    <br />
    <dx:ASPxComboBox ID="ddlCompany" runat="server" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"
        TextField="name" ValueField="id" ValueType="System.Int32" AutoPostBack="True">
    </dx:ASPxComboBox>
    <dx:ASPxCheckBox ID="chkbox_show_locations" runat="server" AutoPostBack="True" 
		Checked="True" CheckState="Checked" 
		oncheckedchanged="ASPxCheckBox1_CheckedChanged" Text="Show Locations">
	</dx:ASPxCheckBox>
    <br />
	<dx:aspxgridview id="gv_counts" runat="server" autogeneratecolumns="False"
		width="100%" ClientInstanceName="gv_counts" 
		OnCustomJSProperties="gv_counts_CustomJSProperties" 
		OnCustomCallback="gv_counts_CustomCallback" 
		onhtmldatacellprepared="gv_counts_HtmlDataCellPrepared" Font-Names="Arial" 
		ondatabound="gv_counts_DataBound">

<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" AutoExpandAllGroups="True" EnableCustomizationWindow="True"></SettingsBehavior>

<Styles>
<Header Font-Bold="True"></Header>

<Cell Wrap="False"></Cell>
	<TitlePanel>
		<BackgroundImage ImageUrl="~/images/foundation/bg/bg[main].png" VerticalPosition="bottom" />
	</TitlePanel>
	<GroupPanel Font-Bold="True" ForeColor="White">
		<BackgroundImage ImageUrl="~/images/foundation/bg/bg[header].png" Repeat="RepeatX"
			VerticalPosition="top" />
	</GroupPanel>
</Styles>

<SettingsPager PageSize="50"></SettingsPager>
<Columns>
    <dx:GridViewDataTextColumn Caption="Tag" FieldName="tag" VisibleIndex="0" 
		Width="200px">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Part #" FieldName="master_id" VisibleIndex="1"
        Width="100px">
        <DataItemTemplate>
			<dx:ASPxHyperLink ID="hl_po" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/member/inventory/index.aspx?a=get&tab=G&id={0}', 'inventory', 1035,800)&quot;, Eval(&quot;master_id&quot;)) %>"
				Text='<%# Eval("master_id") %>'>
			</dx:ASPxHyperLink>
        </DataItemTemplate>
        <CellStyle HorizontalAlign="Center">
        </CellStyle>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
		VisibleIndex="2" Width="100%">
    	<Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
    	<CellStyle Wrap="False">
		</CellStyle>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Cost" FieldName="cost" VisibleIndex="3" 
		Width="100px">
    	<PropertiesTextEdit DisplayFormatString="C2">
		</PropertiesTextEdit>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Qty Onhand" FieldName="qty_onhand" VisibleIndex="4"
        Width="100px">
        <CellStyle HorizontalAlign="Center">
        </CellStyle>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Extd Cost" FieldName="extdcost" 
		VisibleIndex="5" Width="100px">
    	<PropertiesTextEdit DisplayFormatString="C2">
		</PropertiesTextEdit>
    </dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Location" FieldName="loc" VisibleIndex="6" 
		Width="100px">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="MIN Stock $" FieldName="mincost" VisibleIndex="7" 
		Width="100px">
		<PropertiesTextEdit DisplayFormatString="C2">
		</PropertiesTextEdit>
	</dx:GridViewDataTextColumn>
</Columns>

<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowGroupPanel="True" VerticalScrollBarStyle="Virtual" ShowFilterBar="Visible" ShowFooter="True"></Settings>
		
        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-Height="250px" CustomizationWindow-HorizontalOffset="5" CustomizationWindow-VerticalOffset="5"/>
        <GroupSummary>
            <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="extdcost" SummaryType="Sum" Tag="extdcostsum" />
            <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="mincost" SummaryType="Sum" Tag="mincostsum" />
        </GroupSummary>
        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="extdcost" ShowInColumn="Extd Cost" ShowInGroupFooterColumn="Extd Cost" SummaryType="Sum" />
        	<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="mincost" SummaryType="Sum" Tag="mincostsum" ShowInColumn="MIN Stock $" ShowInGroupFooterColumn="MIN Stock $"/>
        </TotalSummary>
</dx:aspxgridview>
    <br />
    &nbsp;
	<dx:ASPxGridViewExporter ID="gve" runat="server" FileName="CountsReport" GridViewID="gv_counts"
		Landscape="True" BottomMargin="1" LeftMargin="1" RightMargin="1" TopMargin="1">
	</dx:ASPxGridViewExporter>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

