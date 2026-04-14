<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="master_inventory" Theme="NETheme01" Title="Master Inventory Grid" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Xpo" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Data.Linq" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<lc:LayoutControl runat="server" id="layout" />
            <dx:aspxgridview id="gv_MasterInventory" runat="server" autogeneratecolumns="False"
                clientinstancename="gv_MasterInventory"
                csspostfix="SoftOrange"
                width="100%" OnCustomCallback="gv_MasterInventory_CustomCallback" OnCustomJSProperties="gv_MasterInventory_CustomJSProperties" EnableTheming="True" Theme="Default">

<Styles GroupButtonWidth="28">
<Header SortingImageSpacing="5px" ImageSpacing="5px" Font-Size="Small"></Header>
    <Row Font-Size="Smaller">
    </Row>
    <LoadingPanel ImageSpacing="8px">
    </LoadingPanel>
</Styles>

<SettingsPager PageSize="50" NumericButtonCount="50" AlwaysShowPager="True" Position="TopAndBottom">
<AllButton Text="All"></AllButton>
</SettingsPager>
<GroupSummary>
    <dx:ASPxSummaryItem DisplayFormat="c" FieldName="JobCost" ShowInColumn="Branch" ShowInGroupFooterColumn="Branch"
        SummaryType="Sum" Tag="T+M" />
    <dx:ASPxSummaryItem DisplayFormat="c" FieldName="Invoiced Amount" ShowInColumn="Branch"
        ShowInGroupFooterColumn="Branch" SummaryType="Sum" Tag="Invoiced" />
</GroupSummary>

<Columns>
	<dx:GridViewDataTextColumn Caption="Master ID" FieldName="masterid" VisibleIndex="0">
		<Settings AllowHeaderFilter="True" />
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Branch" FieldName="branch" VisibleIndex="2">
		<Settings AllowHeaderFilter="True" />
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Description" FieldName="description" VisibleIndex="3">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Tag" FieldName="tagname" VisibleIndex="4">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Is QTY" FieldName="is_qty" VisibleIndex="5">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Is Exclude" FieldName="is_exclude" VisibleIndex="6">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Location" FieldName="location" VisibleIndex="1">
	</dx:GridViewDataTextColumn>
</Columns>

<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowGroupPanel="True" ShowFilterBar="Visible" ShowTitlePanel="True" ShowGroupedColumns="True" ShowGroupFooter="VisibleIfExpanded" ShowFooter="True"></Settings>

<StylesEditors>

<ProgressBar Height="29px"></ProgressBar>
    <CalendarHeader Spacing="1px">
    </CalendarHeader>
</StylesEditors>
                
                <SettingsEditing EditFormColumnCount="1" Mode="EditForm" />
                <SettingsBehavior EnableRowHotTrack="True" EnableCustomizationWindow="True"/>
                <SettingsLoadingPanel ImagePosition="Top" />
                <Paddings Padding="1px" />
        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides">
<CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="TopSides"></CustomizationWindow>
				</SettingsPopup>
</dx:aspxgridview>
	<dx:XpoDataSource runat="server" ID="xpo_ds">
	</dx:XpoDataSource>
	<br />
	        
</asp:Content>

