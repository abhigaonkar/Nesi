<%@ Page Language="C#" MasterPageFile='../../../IntraDefault.master' AutoEventWireup="true" Inherits="sections_reports_job_costs_index" Title='Job Costs' Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' runat='Server'>
	<div id='divSide' runat='server'>
		<asp:SqlDataSource ID='Users' runat='server'></asp:SqlDataSource>
	</div>
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
				<div id='divMenu' runat='server'></div>
</asp:Content>

<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' Runat='Server'>

	<asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Width="231px"></asp:Label>
	<dx:ASPxComboBox ID="ddlCompany" runat="server" TextField="ddl_name" ValueField="id" Caption="Select Business Unit" ClientInstanceName="ddlcompany">
		<ClientSideEvents SelectedIndexChanged="function(s,e){cbp.PerformCallback();}" />
	</dx:ASPxComboBox>
    <asp:Label ID="lblCurrentView" runat="server" Font-Italic="False" Font-Size="9pt"></asp:Label><br />
    &nbsp;<uc:layout_control ID="layout" runat="server" GridviewID="gv_jobcost" ShowExcelExport="True" ShowPDFExport="True" ShowToggle="True" />
	<dx:ASPxCallbackPanel ID="cbp" ClientInstanceName="cbp" runat="server" Width="100%">
		<PanelCollection>
<dx:PanelContent runat="server">
	<div style="width: 512px;border:solid 1px #999;margin:10px;">
	<div style="color:red;padding:2px;margin:2px;">Red text indicates being over the quoted amount</div>
	<div style="color:orange;padding:2px;margin:2px;">Orange text indicates getting close to quoted amount</div>
	<div style="color:white;padding:2px;margin:2px;background-color:red;">Red background indicates that figures are in the negative</div>
	</div>
	<dx:ASPxGridView ID="gv_jobcost" runat="server" AutoGenerateColumns="False" DataSourceID="sds_jobcost" KeyFieldName="woprog_id" Theme="NETheme01" ClientInstanceName="gv_jobcost"  OnHtmlDataCellPrepared="gv_jobcost_HtmlDataCellPrepared" OnCustomCallback="gv_jobcost_CustomCallback" OnCustomJSProperties="gv_jobcost_CustomJSProperties">
		
        
        <SettingsLoadingPanel Mode="Disabled" />
		<ClientSideEvents EndCallback="function(s,e){ bind_tooltips(); }" BeginCallback="function(s,e){ }" />
        <ClientSideEvents BeginCallback="function(s, e) {
	ddlcompany.SetEnabled(false);please_wait('start');
}" CallbackError="function(s, e) {
	ddlcompany.SetEnabled(true);please_wait('stop');}" EndCallback="function(s, e) {
	ddlcompany.SetEnabled(true);please_wait('stop');}" />
		<TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="quoted_amount" ShowInColumn="Quoted Amount ($)" ShowInGroupFooterColumn="Quoted Amount ($)" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="quoted_labor_hours" ShowInColumn="Quoted Labor (hrs)" ShowInGroupFooterColumn="Quoted Labor (hrs)" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="labor_hours_remaining" ShowInColumn="Quoted Labor Remaining (hrs)" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="material_quote_price" ShowInColumn="Quoted Material Cost ($)" ShowInGroupFooterColumn="Quoted Material Cost ($)" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="to_be_billed" ShowInColumn="To Be Billed on WO ($)" ShowInGroupFooterColumn="To Be Billed on WO ($)" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="bench_sell" ShowInColumn="WO Benchmark Sell ($)" ShowInGroupFooterColumn="WO Benchmark Sell ($)" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="to_be_billed" ShowInColumn="To Be Billed on WO ($)" ShowInGroupFooterColumn="To Be Billed on WO ($)" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="labor_cost_tot" ShowInColumn="WO Labor (hrs)" ShowInGroupFooterColumn="WO Labor (hrs)" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="material_actual_price" ShowInColumn="WO Material Cost ($)" ShowInGroupFooterColumn="WO Material Cost ($)" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="matl_used" ShowInColumn="WO Material Sell ($)" ShowInGroupFooterColumn="WO Material Sell ($)" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="total_cost" ShowInColumn="WO Total Cost ($)" ShowInGroupFooterColumn="WO Total Cost ($)" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="total_labor_cost" ShowInColumn="Total Labor Cost" ShowInGroupFooterColumn="Total Labor Cost" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="total_material_cost" ShowInColumn="Projected Material Cost" ShowInGroupFooterColumn="Projected Material Cost" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="quote_margin_dollars" ShowInColumn="Quote Margin" ShowInGroupFooterColumn="Quote Margin" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="projected_labor_cost" ShowInColumn="Projected Labor Cost" ShowInGroupFooterColumn="Projected Labor Cost" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="projected_material_cost" ShowInColumn="Projected Material Cost" ShowInGroupFooterColumn="Projected Material Cost" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="projected_total_cost" ShowInColumn="Projected Total Cost" ShowInGroupFooterColumn="Projected Total Cost" SummaryType="Sum" />
       
            
        </TotalSummary>
		<Columns>
			<dx:GridViewDataTextColumn Caption="PM" FieldName="pm" ShowInCustomizationForm="True" VisibleIndex="0" Width="150px">
				<Settings AutoFilterCondition="Contains" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="wo" ShowInCustomizationForm="True" VisibleIndex="1" Caption="WO #" MinWidth="150">
				<Settings AutoFilterCondition="Contains" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="status" ShowInCustomizationForm="True" VisibleIndex="2" Caption="WO Status">
				<Settings AutoFilterCondition="Contains" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="quoteid" ShowInCustomizationForm="True" VisibleIndex="3" Caption="Quote #">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="customer_name" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Customer Name">
				<Settings AutoFilterCondition="Contains" />
				<CellStyle HorizontalAlign="Left">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="descript" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Description" MinWidth="250" Width="250px">
				<Settings AutoFilterCondition="Contains" />
				<CellStyle HorizontalAlign="Left" Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="quoted_amount" ShowInCustomizationForm="True" VisibleIndex="6" Caption="Quoted Amount ($)">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="matl_used" ShowInCustomizationForm="True" VisibleIndex="7" Caption="WO Material Sell ($)" ToolTip="The total extended sell price of material used on this &amp; all associated WO's">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="bench_sell" ShowInCustomizationForm="True" VisibleIndex="8" Caption="WO Benchmark Sell ($)" ToolTip="The total labor &amp; material sell minus the total cost from open PO's">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="to_be_billed" ShowInCustomizationForm="True" VisibleIndex="9" Caption="To Be Billed on WO ($)" ToolTip="Amount left to bill on work order">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="prog_billed" ShowInCustomizationForm="True" VisibleIndex="10" Caption="Progress Billed (%)" ToolTip="The total percentage that has been progress billed">
				<PropertiesTextEdit DisplayFormatString="P2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="total_cost" ShowInCustomizationForm="True" VisibleIndex="11" Caption="WO Total Cost ($)" ToolTip="The total labor and material cost from the work order plus the total cost of all required materials not yet committed &amp; ">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="quoted_amount_used" ShowInCustomizationForm="True" VisibleIndex="12" Caption="Quoted Amount Used (%)" ToolTip="The quoted amount minus total material and labor sell minus required materials not yet committed cost divided by the quoted amount">
				<PropertiesTextEdit DisplayFormatString="P2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="overall_margin" ShowInCustomizationForm="True" VisibleIndex="13" Caption="Overall Margin (%)" ToolTip="The total quoted amount minus labor &amp; material cost, divided by the quoted amount">
				<PropertiesTextEdit DisplayFormatString="P2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="quoted_labor_hours" ShowInCustomizationForm="True" VisibleIndex="14" Caption="Quoted Labor (hrs)" ToolTip="The number of hours quoted on the quote worksheet">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="actual_labor_hours" ShowInCustomizationForm="True" VisibleIndex="15" Caption="WO Labor (hrs)" ToolTip="The actual number of hours used on the work order">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="labor_cost_tot" ShowInCustomizationForm="True" VisibleIndex="16" Caption="WO Labor Cost ($)" ToolTip="The total cost dollar value of this and all other associated work orders.">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="labor_cost_per" ShowInCustomizationForm="True" VisibleIndex="17" Caption="WO Labor Cost Per ($)" ToolTip="The &quot;Labor Cost ($)&quot; divided by &quot;Actual Labor (hrs)&quot;">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="labor_used" ShowInCustomizationForm="True" VisibleIndex="18" Caption="Quoted Labor Used (%)" ToolTip="&quot;Actual Labor (hrs)&quot; divided by &quot;Quoted Labor (hrs)&quot;">
				<PropertiesTextEdit DisplayFormatString="P2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="material_quote_price" ShowInCustomizationForm="True" VisibleIndex="19" Caption="Quoted Material Cost ($)" ToolTip="The total cost of all material lines from the quote">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="material_actual_price" ShowInCustomizationForm="True" VisibleIndex="25" Caption="WO Material Cost ($)" ToolTip="Total material cost from this work order">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="material_used" ShowInCustomizationForm="True" VisibleIndex="20" Caption="Quoted Material Used (%)" ToolTip="&quot;Quoted Material Cost ($)&quot; divided by &quot;Actual Material Cost ($)&quot;">
				<PropertiesTextEdit DisplayFormatString="P2" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="labor_hours_remaining" ShowInCustomizationForm="True" VisibleIndex="21" Caption="Quoted Labor Remaining (hrs)" ToolTip="&quot;Quoted Labor (hrs)&quot; minus the &quot;Actual Labor (hrs)&quot;">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Service Address" FieldName="service_addr" MinWidth="50" ShowInCustomizationForm="True" VisibleIndex="26">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Cut Date" FieldName="cut_date" MinWidth="50" ShowInCustomizationForm="True" VisibleIndex="27">
			</dx:GridViewDataDateColumn>
		    <dx:GridViewDataDateColumn Caption="Invoiced Date" FieldName="invoiced_date" ShowInCustomizationForm="True" VisibleIndex="28">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="projected_labor_cost" MinWidth="50" ShowInCustomizationForm="True" VisibleIndex="29" Caption="Projected Labor Cost" ToolTip="&quot;Projected Labor Cost&quot;">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="projected_material_cost" MinWidth="50" ShowInCustomizationForm="True" VisibleIndex="30" Caption="Projected Material Cost" ToolTip="&quot;Projected Material Cost&quot;">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="projected_total_cost" MinWidth="50" ShowInCustomizationForm="True" VisibleIndex="31" Caption="Projected Total Cost" ToolTip="&quot;Projected Total Cost&quot;">
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="projected_margin" MinWidth="50" ShowInCustomizationForm="True" VisibleIndex="32" Caption="Projected Margin" ToolTip="&quot;Projected Margin&quot;">
				<PropertiesTextEdit DisplayFormatString="P2" />
			</dx:GridViewDataTextColumn>
            	<dx:GridViewDataTextColumn FieldName="quote_margin_p" ShowInCustomizationForm="True" VisibleIndex="33" Caption="Quote Margin (%)" >
				<PropertiesTextEdit DisplayFormatString="P2" />
			</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="quote_margin_dollars" MinWidth="50" ShowInCustomizationForm="True" VisibleIndex="34" Caption="Quote Margin" >
				<PropertiesTextEdit DisplayFormatString="C2" />
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" ShowGroupPanel="True" ShowHeaderFilterButton="True" ColumnMinWidth="25" ShowFooter="True" />
		<SettingsDataSecurity AllowDelete="False" />
		<Styles>
			<Header HorizontalAlign="Center" Wrap="True">
			</Header>
			<Cell HorizontalAlign="Center">
			</Cell>
		</Styles>
	</dx:ASPxGridView>
	<br />
	<asp:SqlDataSource ID="sds_jobcost" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL report_jobcost(@business_unit_id)">
		<SelectParameters>
			<asp:ControlParameter ControlID="ddlCompany" Name="@business_unit_id" PropertyName="Value" />
		</SelectParameters>
	</asp:SqlDataSource>
			</dx:PanelContent>
</PanelCollection>
	</dx:ASPxCallbackPanel>
	<br />

    <div style="display:none;">Gain/Loss = (Quoted Amount * Percent Complete From Time Sheet) - Total   
	</div>
</asp:Content>
