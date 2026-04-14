<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="ERRepairs" Title="ER Repairs Grid" Codebehind="index.aspx.cs" %>

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
	<script type="text/javascript">
		function q(id, rev)
			{
			boing("/sections/member/quote/index.aspx?a=g&quote_id="+id+"&revision="+rev, "quote", 1035, 800);
			}
			function w(id)
			{
			boing("/sections/workorder/index.aspx?woprog_id="+id, "wo", 1035, 800);
			}
			function i(id)
			{
			boing("/sections/member/inventory/index.aspx?a=get&tab=G&id=" + id,"inventory",1035,800);
			}
    </script>
	<lc:LayoutControl runat="server" id="layout" />
            <dx:aspxgridview id="gv_er" runat="server" autogeneratecolumns="False" Width="100%"
                clientinstancename="gv_er" cssfilepath="~/App_Themes/PlasticBlue/{0}/styles.css"
                csspostfix="PlasticBlue" datasourceid="SqlDataSource1" OnCommandButtonInitialize="Aspxgridview1_CommandButtonInitialize" OnHtmlDataCellPrepared="Aspxgridview1_HtmlDataCellPrepared" OnCustomCallback="gv_er_CustomCallback" OnCustomJSProperties="gv_er_CustomJSProperties" Font-Names="Arial" Font-Size="9pt">

<Styles CssPostfix="PlasticBlue" CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css">
<Header SortingImageSpacing="10px" ImageSpacing="10px" Font-Size="Small"></Header>
    <Row Font-Size="Smaller">
    </Row>
    <Cell Wrap="False" Font-Names="Arial" Font-Size="9pt">
        <Paddings Padding="3px" />
    </Cell>
    <DetailCell Font-Names="Arial" Font-Size="8pt">
    </DetailCell>
    <PreviewRow Font-Names="Arial" Font-Size="6pt">
    </PreviewRow>
    <Table>
        <Paddings Padding="2px" />
    </Table>
</Styles>

<SettingsPager PageSize="50" NumericButtonCount="50" Position="TopAndBottom" >
<AllButton Text="All"></AllButton>

<NextPageButton Text="Next &gt;"></NextPageButton>

<PrevPageButton Text="&lt; Prev"></PrevPageButton>
</SettingsPager>
<GroupSummary>
<dx:ASPxSummaryItem SummaryType="Count" FieldName="Customer" ShowInColumn="Customer"></dx:ASPxSummaryItem>
<dx:ASPxSummaryItem SummaryType="Count" FieldName="Status" ShowInColumn="Status"></dx:ASPxSummaryItem>
<dx:ASPxSummaryItem SummaryType="Sum" FieldName="Job Cost" DisplayFormat="c" ShowInColumn="Branch"></dx:ASPxSummaryItem>
</GroupSummary>

<ImagesFilterControl>
<LoadingPanel Url="~/App_Themes/PlasticBlue/Editors/Loading.gif"></LoadingPanel>
</ImagesFilterControl>

<Images SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css">
<LoadingPanelOnStatusBar Url="~/App_Themes/PlasticBlue/GridView/gvLoadingOnStatusBar.gif"></LoadingPanelOnStatusBar>

<LoadingPanel Url="~/App_Themes/PlasticBlue/GridView/Loading.gif"></LoadingPanel>
</Images>

<ClientSideEvents RowClick="function(s, e) {

}"></ClientSideEvents>



<Columns>
    <dx:GridViewDataTextColumn Caption="Part" FieldName="master_id" VisibleIndex="1">
    <DataItemTemplate>
			&nbsp;<dx:ASPxHyperLink ID="hl_part" runat="server" NavigateUrl="javascript:void(0);"
				OnInit="hl_part_Init" Text='<%# Eval("master_id") %>' Font-Bold="True">
			</dx:ASPxHyperLink>
		</DataItemTemplate>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Repair ID" FieldName="id" VisibleIndex="0">
        <EditFormSettings Visible="False" />
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Quoted Sell" FieldName="quoted_sell" VisibleIndex="5">
        <PropertiesTextEdit DisplayFormatString="C2">
        </PropertiesTextEdit>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="WO Sell" FieldName="wo_sell" VisibleIndex="6">
        <PropertiesTextEdit DisplayFormatString="C2">
        </PropertiesTextEdit>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataDateColumn Caption="Date Entered" FieldName="date_entered" VisibleIndex="7">
        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
        </PropertiesDateEdit>
        <CellStyle HorizontalAlign="Right">
        </CellStyle>
    </dx:GridViewDataDateColumn>
    <dx:GridViewDataDateColumn Caption="Date Returned" FieldName="date_returned" VisibleIndex="8">
        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
        </PropertiesDateEdit>
        <CellStyle HorizontalAlign="Right">
        </CellStyle>
    </dx:GridViewDataDateColumn>
    <dx:GridViewDataTextColumn Caption="Customer" FieldName="Customer" VisibleIndex="4">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="PO" FieldName="Cust_po" VisibleIndex="9">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="WO Status" FieldName="wo_status" VisibleIndex="10">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Repair Status" FieldName="repair_status" VisibleIndex="11">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Repair Note" FieldName="repair_note" VisibleIndex="12">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn FieldName="branch_id" Visible="False" VisibleIndex="13">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn FieldName="quote_description" Visible="False" VisibleIndex="14">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Description" FieldName="part_description" VisibleIndex="2">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="BVWO" FieldName="wo_detail_current_bvwo" VisibleIndex="15">
    <DataItemTemplate>
			&nbsp;<dx:ASPxHyperLink ID="hl_wo" runat="server" NavigateUrl="javascript:void(0);"
				OnInit="hl_wo_Init" Text='<%# Eval("wo_detail_current_bvwo") %>' Font-Bold="True">
			</dx:ASPxHyperLink>
		</DataItemTemplate>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn FieldName="WOProg_Description" Visible="False" VisibleIndex="16">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn FieldName="woprog_id" Visible="False" VisibleIndex="17">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Serial" FieldName="serial" VisibleIndex="18">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Quote" FieldName="quote_id" VisibleIndex="19">
     <DataItemTemplate>
			&nbsp;<dx:ASPxHyperLink ID="hl_quote" runat="server" NavigateUrl="javascript:void(0);"
				OnInit="hl_quote_Init" Text='<%# Eval("quote_id") %>' Font-Bold="True">
			</dx:ASPxHyperLink>
		</DataItemTemplate>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Invoice" FieldName="InvoiceNo" VisibleIndex="20">
    </dx:GridViewDataTextColumn>
     
    <dx:GridViewDataDateColumn Caption="Invoice Date" FieldName="InvoiceDate" VisibleIndex="21">
        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
        </PropertiesDateEdit>
        <CellStyle HorizontalAlign="Right">
        </CellStyle>
    </dx:GridViewDataDateColumn>
    <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="branch" VisibleIndex="3">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Quote_rev" FieldName="rev" Visible="False" VisibleIndex="22" >
    </dx:GridViewDataTextColumn>
</Columns>
<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowFilterBar="Visible" UseFixedTableLayout="True"></Settings>

<StylesEditors>
<CalendarHeader Spacing="11px"></CalendarHeader>

<ProgressBar Height="25px"></ProgressBar>
</StylesEditors>
                <Templates>
                    <EditForm>
						<dx:ASPxCallbackPanel ID="cbp_edit_note" runat="server" Width="500px" ClientInstanceName="cbp_edit_note" OnCallback="cbp_edit_note_Callback">
							<PanelCollection>
								<dx:PanelContent runat="server">
                        <table cellpadding="2" cellspacing="0" width="700">
                            <tr>
                                <td style="width: 150px">
									<strong>
                                    Expected End Date:</strong></td>
                                <td>
                                    <dx:ASPxDateEdit ID="date_expected" runat="server" DateOnError="Today" EditFormat="Custom"
                                        EditFormatString="yyyy-MM-dd" AnimationType="None" Value='<%# Eval("completion") %>'>
                                    </dx:ASPxDateEdit>
									&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px">
									<strong>
                                    Due Date:</strong></td>
                                <td>
                                    <dx:ASPxDateEdit ID="date_due" runat="server" DateOnError="Today" EditFormat="Custom"
                                        EditFormatString="yyyy-MM-dd" AnimationType="None" Value='<%# Eval("date_due") %>'>
                                    </dx:ASPxDateEdit>
                                </td>
                            </tr>
							<tr>
								<td style="width: 150px">
									<strong>Success %:</strong></td>
								<td>
									<dx:ASPxSpinEdit ID="spin_success_pct" runat="server" Height="21px" MaxValue="100" NumberType="Integer" Width="50px" Value='<%# Eval("success_percent") %>'>
									</dx:ASPxSpinEdit>
								</td>
							</tr>
                            <tr>
                                <td style="width: 150px">
									<strong>
                                    Notes:</strong></td>
                                <td>
                                    <dx:ASPxMemo ID="note" runat="server" Height="71px" Text='<%# Eval("notes") %>'
                                        Width="400px">
                                    </dx:ASPxMemo>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px" align="center">
									<dx:ASPxButton ID="btn_update" runat="server" AutoPostBack="False" Text="Update">
										<Image Url="~/images/icon/icon[save].gif">
										</Image>
										<ClientSideEvents Click="function(s, e) {
	cbp_edit_note.PerformCallback();
}" />
									</dx:ASPxButton>
								</td>
                                <td>
									<dx:ASPxButton ID="btn_cancel" runat="server" AutoPostBack="False" Text="Cancel">
										<Image Url="~/images/icon/icon[undo].gif">
										</Image>
										<ClientSideEvents Click="function(s, e) {
	gv_quotes.CancelEdit();
}" />
									</dx:ASPxButton>
								</td>
                            </tr>
                        </table>
								</dx:PanelContent>
							</PanelCollection>
							<ClientSideEvents EndCallback="function(s, e) {
	gv_quotes.CancelEdit();
}" />
						</dx:ASPxCallbackPanel>
                    </EditForm>
                </Templates>
                <SettingsEditing EditFormColumnCount="1" Mode="EditForm" />
                <SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control" EnableCustomizationWindow="True"/>
                <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides"/>
</dx:aspxgridview>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT * FROM [master_er_report]"></asp:SqlDataSource>
</asp:Content>

