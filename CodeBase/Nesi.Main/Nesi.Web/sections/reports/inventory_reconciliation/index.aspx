<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_inventory_reconciliation_index" Title="Inventory Reconcile Report" Theme="NETheme01" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<script type="text/javascript">

 function resizeIframe(obj)
 {

   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 10) + 'px';
	
 }
 </script>

	<lc:LayoutControl runat="server" id="layout" GridviewID="gv_counts" />
    <br />
	<dx:ASPxCallbackPanel ID="pc" runat="server" Width="100%" ClientInstanceName="pc" oncallback="pc_Callback">
		<PanelCollection>
			<dx:PanelContent>
    <table style="width:100%;">
		<tr>
			<td> Business Unit

    		</td>
			<td>
			    <dx:ASPxComboBox ID="ddlCompany" runat="server" TextField="ddl_name" ValueField="id" ValueType="System.Int32">
			    </dx:ASPxComboBox></td>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td>
			    Start
			</td>
			<td>
			    <dx:ASPxDateEdit ID="dte_start" runat="server" DisplayFormatString="yyyy-MM-dd" 
			                     EditFormat="Custom" EditFormatString="yyyy-MM-dd">
			    </dx:ASPxDateEdit></td>
			<td width="100%">
				&nbsp;</td>
		</tr>
		<tr>
			<td>End
			</td>
			<td>
			    <dx:ASPxDateEdit ID="dte_end" runat="server" DisplayFormatString="yyyy-MM-dd" 
			                     EditFormat="Custom" EditFormatString="yyyy-MM-dd">
			    </dx:ASPxDateEdit></td>
			<td>
				<dx:ASPxButton ID="ASPxButton2" runat="server" Text="Apply" 
					AutoPostBack="False">
					<ClientSideEvents Click="function(s, e) {
	pc.PerformCallback();
}" />
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
    <br />
				
	<dx:aspxgridview id="gv_counts" runat="server" autogeneratecolumns="False"
		width="100%" ClientInstanceName="gv_counts" 
		OnCustomJSProperties="gv_counts_CustomJSProperties" 
		OnCustomCallback="gv_counts_CustomCallback" Font-Names="Arial" 
		KeyFieldName="x" onhtmleditformcreated="gv_counts_HtmlEditFormCreated" 
		onstartrowediting="gv_counts_StartRowEditing"  OnHtmlDataCellPrepared="gv_counts_HtmlDataCellPrepared">

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
        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[search].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
<Columns>
    <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" ShowEditButton="true" 
		Width="50px">
		
	</dx:GridViewCommandColumn>
    <dx:GridViewDataTextColumn Caption="Part #" FieldName="x" VisibleIndex="1"
        Width="100px">
        <DataItemTemplate>
			<dx:ASPxHyperLink ID="hl_po" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/member/inventory/index.aspx?a=get&tab=G&id={0}', 'inventory', 1035,800)&quot;, Eval(&quot;x&quot;)) %>"
				Text='<%# Eval("x") %>'>
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
    <dx:GridViewDataTextColumn Caption="Opening Qty" FieldName="_start" VisibleIndex="3" 
		Width="100px">
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Closing Qty" FieldName="_end" VisibleIndex="4"
        Width="100px">
        <CellStyle HorizontalAlign="Center">
        </CellStyle>
    </dx:GridViewDataTextColumn>
    <dx:GridViewDataTextColumn Caption="Opening DB" FieldName="db_start" 
		VisibleIndex="5" Width="100px">
    	<PropertiesTextEdit DisplayFormatString="C2">
		</PropertiesTextEdit>
    </dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Closing DB" FieldName="db_end" VisibleIndex="6" 
		Width="100px">
		<PropertiesTextEdit DisplayFormatString="c2">
		</PropertiesTextEdit>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Delta Qty" FieldName="_qty_delta" 
		VisibleIndex="7" Width="70px">
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataTextColumn Caption="Delta DB" FieldName="_db_delta" 
		SortIndex="0" SortOrder="Descending" VisibleIndex="8" Width="70px">
		<PropertiesTextEdit DisplayFormatString="c2">
		</PropertiesTextEdit>
	</dx:GridViewDataTextColumn>
	<dx:GridViewDataSpinEditColumn Caption="Delta QTY %" FieldName="qty_diff_percent" MinWidth="50" ShowInCustomizationForm="True" VisibleIndex="9" Width="50px">
		<PropertiesSpinEdit DecimalPlaces="2" DisplayFormatString="{0}%" NumberFormat="Percent">
		</PropertiesSpinEdit>
	</dx:GridViewDataSpinEditColumn>
</Columns>

		<SettingsEditing Mode="PopupEditForm" />

<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowFilterBar="Visible" ShowFooter="True"></Settings>
		<SettingsText PopupEditFormCaption="Transactions" />


        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-Height="250px" CustomizationWindow-HorizontalOffset="5" CustomizationWindow-VerticalOffset="5"
            EditForm-Height="700px" 
			EditForm-HorizontalAlign="WindowCenter" EditForm-Modal="True" 
			EditForm-VerticalAlign="WindowCenter" EditForm-Width="1000px" 
        >

<EditForm Width="1000px" Height="700px" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Modal="True"></EditForm>

<CustomizationWindow Height="250px" HorizontalAlign="LeftSides" VerticalAlign="TopSides" HorizontalOffset="5" VerticalOffset="5"></CustomizationWindow>
		</SettingsPopup>

        <TotalSummary>
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="_db_delta" 
				ShowInColumn="Delta DB" SummaryType="Sum" ValueDisplayFormat="C2" 
				ShowInGroupFooterColumn="Delta DB" />
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="db_end" ShowInColumn="db_end" 
				SummaryType="Sum" ValueDisplayFormat="C2" />
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="db_start" 
				ShowInColumn="db_start" SummaryType="Sum" ValueDisplayFormat="C2" />
		</TotalSummary>
		<Templates>
			<EditForm>
				<iframe ID="I1" runat="server" allowtransparency="true" frameborder="0" Scrolling="auto"  
					marginheight="0" name="I1" width="100%" height="650"></iframe>
			</EditForm>
		</Templates>
</dx:aspxgridview>
		</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>
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

