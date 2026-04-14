<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="slugging" Title="Slugging Averages Report" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
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
        <ContentTemplate>
	<lc:LayoutControl runat="server" id="layout" />
            <dx:aspxgridview id="Aspxgridview1" runat="server" autogeneratecolumns="False"
                clientinstancename="grid" cssfilepath="~/App_Themes/PlasticBlue/{0}/styles.css"
                csspostfix="PlasticBlue" datasourceid="SqlDataSource1"
                width="100%">

<Styles CssPostfix="PlasticBlue" CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css">
<Header SortingImageSpacing="10px" ImageSpacing="10px" Font-Size="Small" Wrap="False"></Header>
    <Row Font-Size="Smaller">
    </Row>
</Styles>

<SettingsPager PageSize="600" NumericButtonCount="600" AlwaysShowPager="True">
<AllButton Text="All"></AllButton>
</SettingsPager>
<GroupSummary>
    <dx:ASPxSummaryItem DisplayFormat="c" FieldName="JobCost" ShowInColumn="Branch" ShowInGroupFooterColumn="Branch"
        SummaryType="Sum" Tag="T+M" />
    <dx:ASPxSummaryItem DisplayFormat="c" FieldName="Invoiced Amount" ShowInColumn="Branch"
        ShowInGroupFooterColumn="Branch" SummaryType="Sum" Tag="Invoiced" />
</GroupSummary>

<ImagesFilterControl>
<LoadingPanel Url="~/App_Themes/PlasticBlue/Editors/Loading.gif"></LoadingPanel>
</ImagesFilterControl>

<Images SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css">
<LoadingPanelOnStatusBar Url="~/App_Themes/PlasticBlue/GridView/gvLoadingOnStatusBar.gif"></LoadingPanelOnStatusBar>

<LoadingPanel Url="~/App_Themes/PlasticBlue/GridView/Loading.gif"></LoadingPanel>
</Images>
<Columns>
    <dx:GridViewDataTextColumn FieldName="business_unit" Caption="Business Unit" VisibleIndex="0">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn FieldName="PM" VisibleIndex="1">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn FieldName="Printed" VisibleIndex="2">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn FieldName="Received" VisibleIndex="3">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn FieldName="Bases" VisibleIndex="4">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn FieldName="slugging" VisibleIndex="5">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn FieldName="Total Quoted" VisibleIndex="6">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn FieldName="Total Received" VisibleIndex="7">
    </dx:GridViewDataTextColumn>
</Columns>

<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowGroupPanel="True" ShowFilterBar="Visible" ShowTitlePanel="True" ShowGroupedColumns="True" ShowGroupFooter="VisibleIfExpanded"></Settings>

<StylesEditors>
<CalendarHeader Spacing="11px"></CalendarHeader>

<ProgressBar Height="25px"></ProgressBar>
</StylesEditors>
                <Templates>
                    <TitlePanel>
                        &nbsp;
                    </TitlePanel>
                    <EditForm>
                        &nbsp;
                    </EditForm>
                </Templates>
                <SettingsEditing EditFormColumnCount="1" Mode="EditForm" />
                <SettingsText CommandUpdate="sss" />
</dx:aspxgridview>
            &nbsp;<dx:ASPxGridViewExporter ID="GridExport" runat="server" FileName="WorkOrders"
                GridViewID="Aspxgridview1">
            </dx:ASPxGridViewExporter>
            <br />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT neintranet.vwslugging.* FROM neintranet.vwslugging">
            </asp:SqlDataSource>
        </ContentTemplate>
	<dx:ASPxGridView ID="gv_slugging" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_slugging"
		CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue"
		DataSourceID="SqlDataSource1" OnCustomCallback="gv_slugging_CustomCallback" OnCustomJSProperties="gv_slugging_CustomJSProperties"
		Width="100%">
		<Templates>
			<TitlePanel>
				&nbsp;
			</TitlePanel>
			<EditForm>
				&nbsp;
			</EditForm>
		</Templates>
        <SettingsBehavior EnableCustomizationWindow="true"/>
        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="Above"/>
		<Styles CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue">
			<Header Font-Size="Small" ImageSpacing="10px" SortingImageSpacing="10px" Wrap="False">
			</Header>
			<Row Font-Size="Smaller">
			</Row>
		</Styles>
		<SettingsPager AlwaysShowPager="True" NumericButtonCount="600" PageSize="600">
			<AllButton Text="All">
			</AllButton>
		</SettingsPager>
		<GroupSummary>
			<dx:ASPxSummaryItem DisplayFormat="c" FieldName="JobCost" ShowInColumn="business_unit" ShowInGroupFooterColumn="business_unit"
				SummaryType="Sum" Tag="T+M" />
			<dx:ASPxSummaryItem DisplayFormat="c" FieldName="Invoiced Amount" ShowInColumn="business_unit"
				ShowInGroupFooterColumn="business_unit" SummaryType="Sum" Tag="Invoiced" />
		</GroupSummary>
		<ImagesFilterControl>
			<LoadingPanel Url="~/App_Themes/PlasticBlue/Editors/Loading.gif">
			</LoadingPanel>
		</ImagesFilterControl>
		<Images SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css">
			<LoadingPanelOnStatusBar Url="~/App_Themes/PlasticBlue/GridView/gvLoadingOnStatusBar.gif">
			</LoadingPanelOnStatusBar>
			<LoadingPanel Url="~/App_Themes/PlasticBlue/GridView/Loading.gif">
			</LoadingPanel>
		</Images>
		<SettingsEditing EditFormColumnCount="1" Mode="EditForm" />
		<SettingsText CommandUpdate="sss" />
		<Columns>
			<dx:GridViewDataTextColumn FieldName="business_unit" Caption="Business Unit" VisibleIndex="0">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="PM" VisibleIndex="1">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="Printed" VisibleIndex="2">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="Received" VisibleIndex="3">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="Bases" VisibleIndex="4">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="slugging" VisibleIndex="5">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="Total Quoted" VisibleIndex="6">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="Total Received" VisibleIndex="7">
			</dx:GridViewDataTextColumn>
		</Columns>
		<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupedColumns="True"
			ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True" ShowHeaderFilterButton="True"
			ShowTitlePanel="True" />
		<StylesEditors>
			<CalendarHeader Spacing="11px">
			</CalendarHeader>
			<ProgressBar Height="25px">
			</ProgressBar>
		</StylesEditors>
	</dx:ASPxGridView>
</asp:Content>

