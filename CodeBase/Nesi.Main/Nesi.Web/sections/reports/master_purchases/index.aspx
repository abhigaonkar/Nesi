<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true"  Inherits="sections_reports_master_purchases_index" Title="Master Purchases" EnableTheming="True" Theme="NETheme01" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>









<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<lc:LayoutControl runat="server" id="layout" />
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:ScriptManager ID="ScriptManager1" runat="server">
			</asp:ScriptManager>
			<ASP:UPDATEPROGRESS ID="UPDATEPROGRESS1" runat="server" DisplayAfter="100" AssociatedUpdatePanelID="UpdatePanel1">
        <PROGRESSTEMPLATE>
        <div id="Layer1" style="position:fixed; left: 0px; top: 0px; width: 100%; padding-top: 200px; text-align: center;" class="update_progress">
          <img id="Img1" src="/images/loading_panel.gif" alt="progressing" />
        </div>
        </PROGRESSTEMPLATE>
    </ASP:UPDATEPROGRESS>
			<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
				oncallback="cb_Callback" Width="200px">
				<LoadingPanelStyle HorizontalAlign="Center" VerticalAlign="Middle">
				</LoadingPanelStyle>
				<PanelCollection>
					<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
					</dx:PanelContent>
				</PanelCollection>
			</dx:ASPxCallbackPanel>
				
			<dx:ASPxGridView ID="gv_purchases" runat="server" autogeneratecolumns="False" 
				ClientInstanceName="gv_purchases" Font-Names="Arial" 
				KeyFieldName="poprog_id" OnCustomCallback="gv_purchases_CustomCallback" 
				OnCustomJSProperties="gv_purchases_CustomJSProperties" 
				onpageindexchanged="gv_purchases_PageIndexChanged" width="100%" SettingsPager-PageSize="50">
				<SettingsBehavior AutoExpandAllGroups="True" ColumnResizeMode="Control" 
					EnableRowHotTrack="True" EnableCustomizationWindow="True"/>
				<Styles>
					
					<Cell Wrap="False">
					</Cell>
					
				
				</Styles>
				<SettingsPager PageSize="50">
				</SettingsPager>
				<Columns>
				    <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" 
				                               VisibleIndex="0" Width="70px">
				        <Settings AutoFilterCondition="BeginsWith" />
				    </dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="2" 
						Width="75px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Date Cut" FieldName="date_cut" 
						VisibleIndex="3" Width="95px">
						<PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="PO #" FieldName="poprog_bvpo" 
						VisibleIndex="4" Width="60px">
						<DataItemTemplate>
							<dx:ASPxHyperLink ID="hl_po" runat="server" 
								NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}', 'po', 1035,800)&quot;, Eval(&quot;poprog_id&quot;)) %>" 
								Text='<%# Eval("poprog_bvpo") %>'>
							</dx:ASPxHyperLink>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="WO #" FieldName="woprog_bvwo" 
						VisibleIndex="5" Width="60px">
						<DataItemTemplate>
							<dx:ASPxHyperLink ID="hl_wo" runat="server" 
								NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/workorder/index.aspx?woprog_id={0}&business_unit_id={1}&fromwo=', 'wo', 1035,800)&quot;, Eval(&quot;woprog_id&quot;), Eval(&quot;business_unit_id&quot;)) %>" 
								OnDataBound="hl_wo_Init" Text='<%# Eval("woprog_bvwo") %>'>
							</dx:ASPxHyperLink>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Vendor" FieldName="name_vendor" 
						VisibleIndex="6" Width="75px">
						<DataItemTemplate>
							<dx:ASPxHyperLink ID="hl_vendor" runat="server" 
								NavigateUrl="<%# string.Format(&quot;javascript:boing('/#/opens/11/vendors/{0}', 'vendor', 1035,800)&quot;, Eval(&quot;vendor_id&quot;)) %>" 
								Text='<%# Eval("name_vendor") %>'>
							</dx:ASPxHyperLink>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Total Cost" FieldName="total_cost" 
						VisibleIndex="7" Width="50px">
						<PropertiesTextEdit DisplayFormatString="{0:c2}">
						</PropertiesTextEdit>
						<CellStyle BackColor="#CCFFFF">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Date Required" FieldName="date_required" 
						VisibleIndex="8" Width="90px">
						<PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Date Ordered" FieldName="date_ordered" 
						VisibleIndex="9" Width="90px">
						<PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Date Received" FieldName="date_received" 
						VisibleIndex="10" Width="90px">
						<PropertiesTextEdit DisplayFormatString="MM/dd/yyyy">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Cut By" FieldName="name_cut_by" 
						VisibleIndex="11" Width="60px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="QTY Ordered" FieldName="qty_ordered" 
						VisibleIndex="12" Width="50px">
						<CellStyle BackColor="#CCFFCC">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="QTY Received" FieldName="qty_received" 
						VisibleIndex="13" Width="50px">
						<CellStyle BackColor="#FFCCCC">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Requested By" FieldName="name_requested_by" 
						VisibleIndex="14" Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataComboBoxColumn Caption="AP Status" FieldName="poprog_apstatus" 
						VisibleIndex="15" Width="80px">
						<PropertiesComboBox EnableFocusedStyle="False">
						</PropertiesComboBox>
						<DataItemTemplate>
							<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" 
								DataSourceID="SqlDataSource1" oninit="ASPxComboBox2_Init" 
								TextField="apstatus_name" Value='<%# Eval("poprog_apstatus") %>' 
								ValueField="apstatus_id" ValueType="System.Int32" Width="100%">
							</dx:ASPxComboBox>
						</DataItemTemplate>
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataTextColumn Caption="AP Notes" 
						FieldName="poprog_hasproblem_notes" VisibleIndex="16" Width="100%">
						<Settings AllowSort="False" />
						<DataItemTemplate>
							<dx:ASPxMemo ID="ASPxMemo1" runat="server" Height="15px" 
								oninit="ASPxMemo1_Init" 
								Text='<%# Eval("poprog_hasproblem_notes") %>' Width="100%">
								<ClientSideEvents Init="function(s, e) {
	 var text = s.GetText().replace(/\r/g, '');
	if (text.length &gt;1 )
		{
			s.SetHeight(40);
			s.GetMainElement().style.backgroundColor = &quot;yellow&quot;
			s.GetInputElement().style.backgroundColor = &quot;yellow&quot;
		}

}" />
							</dx:ASPxMemo>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="id" FieldName="poprog_id" Visible="False" 
						VisibleIndex="17">
					</dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Description" FieldName="poprog_description" Visible="False" 
						VisibleIndex="18">
					</dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Project Manager" FieldName="pm_name" Visible="False" 
						VisibleIndex="19">
					</dx:GridViewDataTextColumn>
				</Columns>
				<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
					ShowGroupPanel="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" />

        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" 
					CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-Height="250px" 
					CustomizationWindow-HorizontalOffset="5" CustomizationWindow-VerticalOffset="5">


			<CustomizationWindow Height="250px" HorizontalAlign="LeftSides" 
				HorizontalOffset="5" VerticalAlign="TopSides" VerticalOffset="5" />
				</SettingsPopup>


			</dx:ASPxGridView>
		</ContentTemplate>
	</asp:UpdatePanel>
	
	<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select * from apstatus"></asp:SqlDataSource>
	
	<br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
	<dx:ASPxGridViewExporter ID="gve" runat="server" FileName="PurchaseReport" GridViewID="gv_purchases"
		Landscape="True" BottomMargin="1" LeftMargin="1" RightMargin="1" TopMargin="1">
	</dx:ASPxGridViewExporter>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

