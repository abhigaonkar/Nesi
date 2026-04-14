<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true"  Inherits="sections_reports_revenue_projection_index" Title="Revenue Projection" Codebehind="index.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <br />
    <dx:ASPxDateEdit ID="dte_start_month" runat="server" AutoPostBack="True" Caption="Start Month:" DisplayFormatString="MMM-yyyy" EditFormat="Custom" EditFormatString="MMM-yyyy" OnDateChanged="ASPxDateEdit1_DateChanged" Theme="NETheme01">
        <CaptionCellStyle Width="125px">
        </CaptionCellStyle>
    </dx:ASPxDateEdit>
    <br />
    <dx:ASPxDateEdit ID="dte_end_month" runat="server" AutoPostBack="True" Caption="Up to Month Ending:" DisplayFormatString="MMM-yyyy" EditFormat="Custom" EditFormatString="MMM-yyyy" OnDateChanged="ASPxDateEdit1_DateChanged" Theme="NETheme01">
        <CaptionCellStyle Width="125px">
        </CaptionCellStyle>
    </dx:ASPxDateEdit>
    <br />
    <br />
    <uc1:layout_control ID="layout" runat="server" />
   		<dx:ASPxGridView ID="gv" runat="server" autogeneratecolumns="False" 
				ClientInstanceName="gv" Font-Names="Arial" OnCustomCallback="gv_CustomCallback" 
				OnCustomJSProperties="gv_CustomJSProperties" 
				width="100%" SettingsPager-PageSize="50" Theme="NETheme01">
				<TotalSummary>
                    <dx:ASPxSummaryItem DisplayFormat="c2" FieldName="Projected_Revenue" ShowInColumn="Total Projected Revenue" ShowInGroupFooterColumn="Total Projected Revenue" SummaryType="Sum" />
                    <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="Variance_db" ShowInColumn="Variance_db" ShowInGroupFooterColumn="Variance_db" SummaryType="Sum" />
                    <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="Month_Budget" ShowInColumn="Budget" ShowInGroupFooterColumn="Budget" SummaryType="Sum" />
                </TotalSummary>
				<GroupSummary>
                    <dx:ASPxSummaryItem DisplayFormat="c2" FieldName="Projected_Revenue" ShowInGroupFooterColumn="Total Projected Revenue" SummaryType="Sum" />
                    <dx:ASPxSummaryItem DisplayFormat="c2" FieldName="Month_Budget" ShowInGroupFooterColumn="Budget" SummaryType="Sum" />
                    <dx:ASPxSummaryItem DisplayFormat="c2" FieldName="Variance_db" ShowInGroupFooterColumn="Variance_db" SummaryType="Sum" />
                </GroupSummary>
				<Columns>
                    <dx:GridViewDataTextColumn FieldName="Branch" VisibleIndex="1" GroupIndex="1" SortIndex="1" SortOrder="Ascending" Caption="Business Unit">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Invoiced" VisibleIndex="3" Visible="False">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Process For Invoice" FieldName="In_Process" VisibleIndex="4">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Open Work Orders" FieldName="Expected_to_close" VisibleIndex="5">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Quotes" FieldName="Quotes_expected_to_be_completed" VisibleIndex="6">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Total Projected Revenue" FieldName="Projected_Revenue" VisibleIndex="7">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                        <CellStyle BackColor="#FF9999">
                        </CellStyle>
                        <FooterCellStyle BackColor="#FF9999">
                        </FooterCellStyle>
                        <GroupFooterCellStyle BackColor="#FF9999">
                        </GroupFooterCellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Budget" FieldName="Month_Budget" VisibleIndex="8">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                        <CellStyle BackColor="#99CCFF">
                        </CellStyle>
                        <FooterCellStyle BackColor="#99CCFF">
                        </FooterCellStyle>
                        <GroupFooterCellStyle BackColor="#99CCFF">
                        </GroupFooterCellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Variance_db" FieldName="Variance_db" VisibleIndex="9">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Country" VisibleIndex="0" GroupIndex="0" SortIndex="0" SortOrder="Ascending">
                        <GroupFooterTemplate>
                            Total:
                        </GroupFooterTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
				<SettingsBehavior AutoExpandAllGroups="True" ColumnResizeMode="Control" 
					EnableRowHotTrack="True" EnableCustomizationWindow="True"/>
				<Styles>
					<Cell Wrap="False">
					</Cell>
				    <Footer Font-Bold="True">
                    </Footer>
                    <GroupFooter Font-Bold="True">
                    </GroupFooter>
				    <BatchEditCell BackColor="#FFFFCC">
                    </BatchEditCell>
				</Styles>
				<SettingsPager PageSize="50" Mode="ShowAllRecords">
				</SettingsPager>
           

				           

				<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
					ShowGroupPanel="True" ShowHeaderFilterButton="True" GroupFormat="{1}" ShowFooter="True" ShowGroupedColumns="True" ShowGroupFooter="VisibleIfExpanded" />

        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" 
					CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-Height="250px" 
					CustomizationWindow-HorizontalOffset="5" CustomizationWindow-VerticalOffset="5">


			<CustomizationWindow Height="250px" HorizontalAlign="LeftSides" 
				HorizontalOffset="5" VerticalAlign="TopSides" VerticalOffset="5" />
				</SettingsPopup>


			</dx:ASPxGridView>
    <p>
        <br />
    </p>
    <p>
    </p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

