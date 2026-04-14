<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_master_customer_status_index" Title="Master Customer Status" EnableTheming="true" Theme="NETheme01" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"	Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<asp:ScriptManager ID="sm" runat="server">
	</asp:ScriptManager>
	<lc:LayoutControl runat="server" id="layout" GridviewID="gv_mastercustomerstatus" ShowExcelExport="True" ShowPDFExport="True" ShowToggle="True" />
	
    <ASP:UPDATEPROGRESS ID="up_prog" runat="server" DisplayAfter="100" AssociatedUpdatePanelID="up">
        <PROGRESSTEMPLATE>
        <div id="Layer1" style="position:absolute; left: 0px; top: 0px; width: 100%; text-align: center;" class="update_progress">
          <img id="Img1" style="box-shadow: 0px 0px 20px #0c0" src="/images/loading_panel.gif" alt="progressing" />
        </div>
        </PROGRESSTEMPLATE>
    </ASP:UPDATEPROGRESS>
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>

	<dx:ASPxGridView ID="gv_mastercustomerstatus" runat="server" AutoGenerateColumns="False" oncustomcallback="gv_CustomCallback" OnCustomJSProperties="gv_CustomJSProperties" DataSourceID="ds_mastercustomerstatus" Width="100%">
		<Columns>
			<dx:GridViewDataTextColumn Caption="Customer Name" FieldName="customer_name" VisibleIndex="1">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Customer #" FieldName="customer_number" VisibleIndex="2">
				<PropertiesTextEdit DisplayFormatString="{0}">
				</PropertiesTextEdit>
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="hl_customer" runat="server" 
					NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/customer/index.aspx?customer_id={0}', 'customer', 1200,800)&quot;, Eval(&quot;customer_id&quot;) )%>" 
					Text='<%# Eval("customer_number") %>' />
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Customer Status" FieldName="status" VisibleIndex="3">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Date" FieldName="date" VisibleIndex="0">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Origin" FieldName="origin" VisibleIndex="6">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Employee Name" FieldName="name" VisibleIndex="4">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" VisibleIndex="5">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" VisibleIndex="7">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Settings ShowFooter="True" ShowFilterRow="True" ShowGroupPanel="True" />
		<Styles>
			<Cell HorizontalAlign="Center">
			</Cell>
			<Header HorizontalAlign="Center">
			</Header>
			<DetailCell HorizontalAlign="Center">
			</DetailCell>
			<AlternatingRow BackColor="#CCEEFF">
			</AlternatingRow>
			<Footer Font-Bold="True" HorizontalAlign="Center">
			</Footer>
		</Styles>
	</dx:ASPxGridView>
			<asp:SqlDataSource ID="ds_mastercustomerstatus" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL ds_master_customer_status()">
			</asp:SqlDataSource>
		</ContentTemplate>
	</asp:UpdatePanel>
	</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

