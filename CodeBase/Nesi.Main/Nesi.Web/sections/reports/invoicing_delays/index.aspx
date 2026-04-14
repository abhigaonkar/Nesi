<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_invoicing_delays_index" Title="Invoicing Time" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"	Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
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
	<lc:LayoutControl runat="server" id="layout" />
	<dx:ASPxGridView ID="gv_invoicing_delays" runat="server" 
		AutoGenerateColumns="False" ClientInstanceName="gv_invoicing_delays" Width="100%" 
		Theme="NETheme01" oncustomcallback="gv_invoicing_delays_CustomCallback" 
		oncustomjsproperties="gv_invoicing_delays_CustomJSProperties">
		<TotalSummary>
			<dx:ASPxSummaryItem FieldName="to_scan" ShowInColumn="Time to Scan" 
				SummaryType="Average" DisplayFormat="{0:N1}" />
				
				<dx:ASPxSummaryItem FieldName="time_prep" ShowInColumn="Time to Prep/vendor PO" 
				SummaryType="Average" DisplayFormat="{0:N1}" />
				<dx:ASPxSummaryItem FieldName="questions" ShowInColumn="In Questions" 
				SummaryType="Average" DisplayFormat="{0:N1}" />
				<dx:ASPxSummaryItem FieldName="rework" ShowInColumn="In Rework" 
				SummaryType="Average" DisplayFormat="{0:N1}" />
				<dx:ASPxSummaryItem FieldName="pm_app" ShowInColumn="Waiting PM App" 
				SummaryType="Average" DisplayFormat="{0:N1}" />
				<dx:ASPxSummaryItem FieldName="bm_app" ShowInColumn="Waiting BM App" 
				SummaryType="Average" DisplayFormat="{0:N1}" />
				<dx:ASPxSummaryItem FieldName="cust_po" ShowInColumn="Waiting for Cust PO" 
				SummaryType="Average" DisplayFormat="{0:N1}" />
				<dx:ASPxSummaryItem FieldName="to_be_invoiced" ShowInColumn="Waiting to be Invoiced" 
				SummaryType="Average" DisplayFormat="{0:N1}" />
				<dx:ASPxSummaryItem FieldName="total_time" ShowInColumn="Total Processing Time" 
				SummaryType="Average" DisplayFormat="{0:N1}" />
				<dx:ASPxSummaryItem FieldName="total_nesi" ShowInColumn="Total Nesi" 
				SummaryType="Average" DisplayFormat="{0:N1}" />
					<dx:ASPxSummaryItem FieldName="total_branch" ShowInColumn="Total Branch" 
				SummaryType="Average" DisplayFormat="{0:N1}" />
					<dx:ASPxSummaryItem FieldName="rev" ShowInColumn="Invoiced Amt" 
				SummaryType="Sum" DisplayFormat="{0:N1}" />

		</TotalSummary>
		<Columns>
			<dx:GridViewDataTextColumn Caption="WO" FieldName="bvwo" VisibleIndex="0" Settings-AutoFilterCondition="Contains">
<Settings AutoFilterCondition="Contains"></Settings>
				<DataItemTemplate>
											<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
								NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/workorder/index.aspx?action=show&woprog_id={0}&business_unit_id={1}', 'workorder', 1200,800)&quot;, Eval(&quot;id&quot;), 0) %>" 
								Text='<%# Eval("bvwo") %>'>
							</dx:ASPxHyperLink>

				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Business Unit" VisibleIndex="1" 
				FieldName="business_unit">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="PM" FieldName="pm" VisibleIndex="2">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Customer" VisibleIndex="3" 
				FieldName="customer">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Time to Scan" FieldName="to_scan" 
				VisibleIndex="4">
			    <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Time to Prep/vendor PO" FieldName="time_prep" 
				VisibleIndex="5"> <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="In Questions" FieldName="questions" 
				VisibleIndex="6"> <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="In Rework" FieldName="rework" 
				VisibleIndex="7"> <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Waiting PM App" FieldName="pm_app" 
				VisibleIndex="8"> <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Waiting BM App" FieldName="bm_app" 
				VisibleIndex="9"> <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Waiting Parent BM Approval" FieldName="bm_parent_app" VisibleIndex="10"> <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

			<dx:GridViewDataTextColumn Caption="Waiting for Cust PO" FieldName="cust_po" 
				VisibleIndex="11"> <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Waiting to be Invoiced" 
				FieldName="to_be_invoiced" VisibleIndex="12"> <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Total Processing Time" FieldName="total_time" 
				VisibleIndex="13"> <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Total Nesi" FieldName="total_nesi" 
				VisibleIndex="14"> <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Total Branch" FieldName="total_branch" 
				VisibleIndex="15"> <PropertiesTextEdit DisplayFormatString="N1">
                </PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Invoiced Date" FieldName="invoice_date" 
				VisibleIndex="16">
			</dx:GridViewDataTextColumn>
			
			
			
			
			
			<dx:GridViewDataTextColumn Caption="Last Time Entry" FieldName="last_hour" 
				VisibleIndex="17">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Date Created" FieldName="wo_cut" 
				VisibleIndex="18">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="id" FieldName="id" VisibleIndex="19" 
				Visible="False">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Invoiced Amt" FieldName="rev" 
				VisibleIndex="20">
			</dx:GridViewDataTextColumn>
			
			
		    <dx:GridViewDataTextColumn Caption="WO Status" FieldName="wo_status" VisibleIndex="21">
            </dx:GridViewDataTextColumn>
			
            <dx:GridViewDataTextColumn Caption="Description" FieldName="woprog_description" VisibleIndex="22" MinWidth="50">
			</dx:GridViewDataTextColumn>
			
		    
			
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
			ShowGroupPanel="True" ShowHeaderFilterButton="True" ShowFooter="True" 
			ShowTitlePanel="True" ColumnMinWidth="20" />
		
	</dx:ASPxGridView>
	<dx:ASPxGridViewExporter ID="gve" runat="server" FileName="Invoicing_delays" GridViewID="gv_invoicing_delays"
		Landscape="True" BottomMargin="1" LeftMargin="1" RightMargin="1" TopMargin="1">
	</dx:ASPxGridViewExporter>
	

	<asp:SqlDataSource ID="sqlcompany" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		>
	</asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

