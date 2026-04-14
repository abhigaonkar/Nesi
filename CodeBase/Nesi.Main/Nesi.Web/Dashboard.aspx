<%@ Page language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="Dashboard" Title="Dashboard" Codebehind="Dashboard.aspx.cs" %>
<%@ MasterType TypeName="IntraDefault" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<ASP:CONTENT ID="Content5" ContentPlaceHolderID="header_placeholder" Runat="Server">
</ASP:CONTENT>
<ASP:CONTENT ID="Content3" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    	a</div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content1" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</ASP:CONTENT>

<ASP:CONTENT ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript" src="/js/dashboard.js"></script>
	<script type="text/javascript">
	

		$("document").ready(function()
			{
			load_gauges(false);
			});
		function load_gauges(just_reload)
			{
			load_if("<%=ifr_wo.ClientID %>", "/dashboard_modules/wosnapshot.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
			load_if("<%=ifr_rev.ClientID %>", "/dashboard_modules/revenue.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
	        load_if("<%=ifr_margin.ClientID %>", "/dashboard_modules/margin.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
			load_if("<%=ifr_gross.ClientID %>", "/dashboard_modules/gross.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
			load_if("<%=ifr_hourutilization.ClientID %>", "/dashboard_modules/hourutilization.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
			load_if("<%=ifr_grids.ClientID %>", "/dashboard_modules/grids.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
			load_if("<%=ifr_drilldown.ClientID %>", "/dashboard_modules/drilldown.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
			load_if("<%=ifr_salespipeline.ClientID %>", "/dashboard_modules/sales_pipeline.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
			load_if("<%=ifr_net.ClientID %>", "/dashboard_modules/net.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);		
			load_if("<%=ifr_customers.ClientID %>", "/dashboard_modules/customers.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
			load_if("<%=ifr_worstcustomers.ClientID %>", "/dashboard_modules/10worstcustomers.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
		    load_if("<%=ifr_inventory.ClientID %>", "/dashboard_modules/inventory.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
            load_if("<%=ifr_headcount.ClientID %>", "/dashboard_modules/headcount.aspx?business_unit_id=<%=business_unit_id.Value %>", just_reload);
			}
	</script>
	<dx:ASPxPanel ID="panel_overview" runat="server" Width="100%" Visible="false">
		<PanelCollection>
			<dx:PanelContent ID="overview_panel" runat="server" SupportsDisabledAttribute="True">
				<div class="ov_box">
					<div class="title">Company Overview<div class="subtitle"><asp:Label ID="lbl_last_updated" runat="server"></asp:Label></div></div>
					<div class="ov_body">
                          <br />
                        <dx:ASPxComboBox ID="cbo_dtes" runat="server" OnSelectedIndexChanged="cbo_dtes_SelectedIndexChanged" TextField="_date" Theme="NETheme01" ValueField="_id" AutoPostBack="True">
                        </dx:ASPxComboBox>
                        <br />
						<lc:LayoutControl runat="server" id="layout" GridviewID="gv_dashoverview" />
						<dx:ASPxGridView ID="gv_dashoverview" runat="server" AutoGenerateColumns="False" OnCustomCallback="gv_dashoverview_CustomCallback" OnCustomJSProperties="gv_dashoverview_CustomJSProperties" DataSourceID="sds_dashoverview" Width="100%" OnSummaryDisplayText="gv_dashoverview_SummaryDisplayText">
							<TotalSummary>
								<dx:ASPxSummaryItem FieldName="rev_mtd" ShowInColumn="Rev MTD" ShowInGroupFooterColumn="Rev MTD" SummaryType="Sum" DisplayFormat="C0" ValueDisplayFormat="{0}" />
								<dx:ASPxSummaryItem FieldName="rev_ytd" ShowInColumn="Rev YTD" ShowInGroupFooterColumn="Rev YTD" SummaryType="Sum" DisplayFormat="C0" />
                                <dx:ASPxSummaryItem FieldName="rev_bmtd" ShowInColumn="Rev Budget MTD" ShowInGroupFooterColumn="Rev Budget MTD" SummaryType="Sum" DisplayFormat="C0" ValueDisplayFormat="{0}" />
								<dx:ASPxSummaryItem FieldName="rev_bytd" ShowInColumn="Rev Budget YTD" ShowInGroupFooterColumn="Rev Budget YTD" SummaryType="Sum" DisplayFormat="C0" />
								<dx:ASPxSummaryItem FieldName="lab_mtd_dol" ShowInColumn="Lab. MTD" ShowInGroupFooterColumn="Lab. MTD" SummaryType="Sum" DisplayFormat="C0" />
								<dx:ASPxSummaryItem FieldName="lab_ytd_dol" ShowInColumn="Lab. YTD" ShowInGroupFooterColumn="Lab. YTD" SummaryType="Sum" DisplayFormat="C0" />
								<dx:ASPxSummaryItem FieldName="mat_mtd_dol" ShowInColumn="Mat. MTD" ShowInGroupFooterColumn="Mat. MTD" SummaryType="Sum" DisplayFormat="C0" />
								<dx:ASPxSummaryItem FieldName="mat_ytd_dol" ShowInColumn="Mat. YTD" ShowInGroupFooterColumn="Mat. YTD" SummaryType="Sum" DisplayFormat="C0" />
								<dx:ASPxSummaryItem FieldName="gross_mtd_dol" ShowInColumn="Gross MTD" ShowInGroupFooterColumn="Gross MTD" SummaryType="Sum" DisplayFormat="C0" />
								<dx:ASPxSummaryItem FieldName="gross_ytd_dol" ShowInColumn="Gross YTD" ShowInGroupFooterColumn="Gross YTD" SummaryType="Sum" DisplayFormat="C0" />
								<dx:ASPxSummaryItem FieldName="inv_balance" ShowInColumn="Inv. Balance" ShowInGroupFooterColumn="Inv. Balance" SummaryType="Sum" DisplayFormat="C0" />
								<dx:ASPxSummaryItem FieldName="ar_90" ShowInColumn="AR &gt; 90" ShowInGroupFooterColumn="AR &gt; 90" SummaryType="Sum" DisplayFormat="C0"/>
								<dx:ASPxSummaryItem FieldName="cust_mtd" ShowInColumn="Cust. MTD" 
									ShowInGroupFooterColumn="Cust. MTD" SummaryType="Sum" DisplayFormat="{0:n0}"/>
								<dx:ASPxSummaryItem FieldName="cust_ytd" ShowInColumn="Cust. YTD" 
									ShowInGroupFooterColumn="Cust. YTD" SummaryType="Sum" DisplayFormat="{0:n0}"/>
								<dx:ASPxSummaryItem FieldName="wo_next_30" ShowInColumn="WO $ Next 30" ShowInGroupFooterColumn="WO $ Next 30" SummaryType="Sum" DisplayFormat="C0" />
								<dx:ASPxSummaryItem FieldName="sales_pipe_30" ShowInColumn="Sales Pipe 30" ShowInGroupFooterColumn="Sales Pipe 30" SummaryType="Sum" DisplayFormat="C0"/>
								<dx:ASPxSummaryItem FieldName="sales_pipe_60" ShowInColumn="Sales Pipe 60" ShowInGroupFooterColumn="Sales Pipe 60" SummaryType="Sum" DisplayFormat="C0" />
								<dx:ASPxSummaryItem FieldName="headcount" ShowInColumn="Head Count" ShowInGroupFooterColumn="Head Count" SummaryType="Sum" DisplayFormat="{0:n0}" />
								<dx:ASPxSummaryItem FieldName="truck_count" ShowInColumn="Truck Count" ShowInGroupFooterColumn="Truck Count" SummaryType="Sum" DisplayFormat="{0:n0}" />
								<dx:ASPxSummaryItem FieldName="parts_to_qc" ShowInColumn="Parts to QC" ShowInGroupFooterColumn="Parts to QC" SummaryType="Sum" DisplayFormat="{0:n0}" />
								<dx:ASPxSummaryItem FieldName="billable_hours" ShowInColumn="Billable Hours" ShowInGroupFooterColumn="Billable Hours" SummaryType="Sum" DisplayFormat="{0:n2}" />
								<dx:ASPxSummaryItem FieldName="lab_mtd_pct" ShowInColumn="Lab. MTD %" ShowInGroupFooterColumn="Lab. MTD %" SummaryType="Average" ValueDisplayFormat="{0}" />
								<dx:ASPxSummaryItem FieldName="lab_ytd_pct" ShowInColumn="Lab. YTD %" ShowInGroupFooterColumn="Lab. YTD %" SummaryType="Average" />
								<dx:ASPxSummaryItem FieldName="mat_mtd_pct" ShowInColumn="Mat. MTD %" ShowInGroupFooterColumn="Mat. MTD %" SummaryType="Average" />
								<dx:ASPxSummaryItem FieldName="mat_ytd_pct" ShowInColumn="Mat. YTD %" ShowInGroupFooterColumn="Mat. YTD %" SummaryType="Average" />
								<dx:ASPxSummaryItem FieldName="gross_mtd_pct" ShowInColumn="Gross MTD %" ShowInGroupFooterColumn="Gross MTD %" SummaryType="Average" />
								<dx:ASPxSummaryItem FieldName="gross_ytd_pct" ShowInColumn="Gross YTD %" ShowInGroupFooterColumn="Gross YTD %" SummaryType="Average" />
								<dx:ASPxSummaryItem DisplayFormat="{0:C2}" FieldName="net_mtd_dol" ShowInColumn="Net MTD $" SummaryType="Sum"   />
								<dx:ASPxSummaryItem FieldName="net_mtd_pct" ShowInColumn="Net MTD %" ShowInGroupFooterColumn="Net MTD %" SummaryType="Average"  />
								<dx:ASPxSummaryItem DisplayFormat="{0:C2}" FieldName="net_ytd_dol" ShowInColumn="Net YTD $" SummaryType="Sum" />
								<dx:ASPxSummaryItem FieldName="net_ytd_pct" ShowInColumn="Net YTD %" ShowInGroupFooterColumn="Net YTD %" SummaryType="Average"  />
								<dx:ASPxSummaryItem DisplayFormat="{0:C2}" FieldName="net_bmtd_dol" ShowInColumn="Net BMTD $" SummaryType="Sum" />
								<dx:ASPxSummaryItem FieldName="net_bmtd_pct" ShowInColumn="Net BMTD %" ShowInGroupFooterColumn="Net BMTD %" SummaryType="Average"  />
								<dx:ASPxSummaryItem DisplayFormat="{0:C2}" FieldName="net_bytd_dol" ShowInColumn="Net BYTD $" SummaryType="Sum" />
								<dx:ASPxSummaryItem FieldName="net_bytd_pct" ShowInColumn="Net BYTD %" ShowInGroupFooterColumn="Net BYTD %" SummaryType="Average" />

                                <dx:ASPxSummaryItem DisplayFormat="{0:C2}" FieldName="gm_bytd" ShowInColumn="GM Budget YTD" SummaryType="Sum" />
                                <dx:ASPxSummaryItem DisplayFormat="{0:C2}" FieldName="gm_bmtd" ShowInColumn="GM Budget MTD" SummaryType="Sum" />
                                <dx:ASPxSummaryItem DisplayFormat="{0:P0}" FieldName="gm_pbytd" ShowInColumn="GM Budget % YTD" SummaryType="Average" />
                                <dx:ASPxSummaryItem DisplayFormat="{0:P0}" FieldName="gm_pbmtd" ShowInColumn="GM Budget % MTD" SummaryType="Average" />
								
                                
							</TotalSummary>
							<Columns>
								<dx:GridViewDataTextColumn Caption="Business Unit" ShowInCustomizationForm="True" VisibleIndex="1" FieldName="name" MinWidth="100" Settings-HeaderFilterMode="CheckedList">
<Settings HeaderFilterMode="CheckedList"></Settings>
									<DataItemTemplate>
										<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" NavigateUrl='<%# string.Format("./dashboard.aspx?a=load_branch&business_unit_id={0}", Eval("business_unit_id") ) %>' Text='<%# Eval("name") %>' />
									</DataItemTemplate>
									<CellStyle Wrap="False" HorizontalAlign="Left">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Rev MTD" ShowInCustomizationForm="True" 
									VisibleIndex="3" FieldName="rev_mtd" MinWidth="20" ToolTip="Revenue Month to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
									<CellStyle BackColor="#CCFFCC">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Rev YTD" ShowInCustomizationForm="True" 
									VisibleIndex="4" FieldName="rev_ytd" MinWidth="20" ToolTip="Revenue Fiscal Year to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
									<CellStyle BackColor="#CCFF99">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Lab. MTD" ShowInCustomizationForm="True" 
									VisibleIndex="5" FieldName="lab_mtd_dol" MinWidth="20" ToolTip="Labour Month to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Mat. MTD" ShowInCustomizationForm="True" 
									VisibleIndex="9" FieldName="mat_mtd_dol" MinWidth="20" ToolTip="Material Month To Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Mat. YTD" ShowInCustomizationForm="True" 
									VisibleIndex="11" FieldName="mat_ytd_dol" MinWidth="20" ToolTip="Material Fiscal Year To Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Gross MTD" ShowInCustomizationForm="True" 
									VisibleIndex="13" FieldName="gross_mtd_dol" MinWidth="20" ToolTip="Gross Margin Month to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
									<CellStyle BackColor="#FFFFCC">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Gross YTD" ShowInCustomizationForm="True" 
									VisibleIndex="15" FieldName="gross_ytd_dol" MinWidth="20" ToolTip="Gross margin fiscal year to date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
									<CellStyle BackColor="#FFFF99">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Inv. Balance" 
									ShowInCustomizationForm="True" VisibleIndex="17" FieldName="inv_balance" 
									MinWidth="20" ToolTip="Inventory Balance">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="AR &gt; 90" ShowInCustomizationForm="True" 
									VisibleIndex="18" FieldName="ar_90" MinWidth="20" 
									ToolTip="Outstanding Accounts Receivable Older than 90 days old.">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="HU MTD" ShowInCustomizationForm="True" 
									VisibleIndex="19" FieldName="hu_mtd" MinWidth="20" ToolTip="Hour Utilization Month to Date">
									<PropertiesTextEdit DisplayFormatString="P2">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="HU YTD" ShowInCustomizationForm="True" 
									VisibleIndex="20" FieldName="hu_ytd" MinWidth="20" ToolTip="Hour Utilization Year to Date">
									<PropertiesTextEdit DisplayFormatString="P2">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Cust. YTD" ShowInCustomizationForm="True" 
									VisibleIndex="22" FieldName="cust_ytd" MinWidth="20" ToolTip="Number of customers year to date">
									<CellStyle BackColor="#66FFFF">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Cust. MTD" ShowInCustomizationForm="True" 
									VisibleIndex="21" FieldName="cust_mtd" MinWidth="20" ToolTip="Number of customers month to date">
									<CellStyle BackColor="#CCFFFF">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="WO $ Next 30" 
									ShowInCustomizationForm="True" VisibleIndex="23" FieldName="wo_next_30" 
									MinWidth="20" ToolTip="Work orders closing in the next 30 days">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Sales Pipe 30" 
									ShowInCustomizationForm="True" VisibleIndex="24" FieldName="sales_pipe_30" 
									MinWidth="20" ToolTip="Open Quotes 90% chance expected to be completed within the next 30 days">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Sales Pipe 60" 
									ShowInCustomizationForm="True" VisibleIndex="25" FieldName="sales_pipe_60" 
									MinWidth="20" ToolTip="Open Quotes 90% chance expected to be completed within the next 60 days">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Head Count" ShowInCustomizationForm="True" 
									VisibleIndex="26" FieldName="headcount" MinWidth="20" ToolTip="Total branch personel">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Truck Count" ShowInCustomizationForm="True" 
									VisibleIndex="27" FieldName="truck_count" MinWidth="20" ToolTip="Number of branch vehicles">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Parts to QC" ShowInCustomizationForm="True" 
									VisibleIndex="28" FieldName="parts_to_qc" MinWidth="20" ToolTip="Parts waiting to QC">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Billable Hours" 
									ShowInCustomizationForm="True" VisibleIndex="29" FieldName="billable_hours" 
									MinWidth="20" ToolTip="Total billable hours in the past 30 days">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="AVG WO Processing" 
									ShowInCustomizationForm="True" VisibleIndex="30" FieldName="wo_avg_proc" 
									MinWidth="20" ToolTip="Average days from last time entry to invoicing.">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Lab. YTD" FieldName="lab_ytd_dol" 
									ShowInCustomizationForm="True" VisibleIndex="7" MinWidth="20" ToolTip="Labour Fiscal Year to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Lab. MTD %" FieldName="lab_mtd_pct" 
									MinWidth="10" ShowInCustomizationForm="True" VisibleIndex="6" 
									ToolTip="Labour Month to Date Percentage of Month to Date Revenue">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Lab. YTD %" FieldName="lab_ytd_pct" 
									MinWidth="10" ShowInCustomizationForm="True" VisibleIndex="8" 
									ToolTip="Labour Fiscal Year to Date Percentage of Year to Date Revenue">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Mat. MTD %" FieldName="mat_mtd_pct" 
									MinWidth="10" ShowInCustomizationForm="True" VisibleIndex="10" 
									ToolTip="Material Month To Date Percentage of Month to Date Revenue">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Mat. YTD %" FieldName="mat_ytd_pct" 
									MinWidth="10" ShowInCustomizationForm="True" VisibleIndex="12" 
									ToolTip="Material Fiscal Year to Date Percentage of Year to Date Revenue">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Gross MTD %" FieldName="gross_mtd_pct" 
									MinWidth="10" ShowInCustomizationForm="True" VisibleIndex="14" 
									ToolTip="Gross Margin Month to Date Percentage of Month to Date Revenue">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Gross YTD %" FieldName="gross_ytd_pct" 
									MinWidth="10" ShowInCustomizationForm="True" VisibleIndex="16" 
									ToolTip="Gross Margin Fiscal Year to date percentage of fiscal year to date revenue">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Region" FieldName="region" 
									ShowInCustomizationForm="True" VisibleIndex="0" MinWidth="20" ToolTip="Regional Area">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Net MTD $" FieldName="net_mtd_dol" 
									ShowInCustomizationForm="True" VisibleIndex="31" MinWidth="10" ToolTip="Net Income Month to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
									<CellStyle BackColor="#FFCCFF">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Net MTD %" FieldName="net_mtd_pct" 
									ShowInCustomizationForm="True" VisibleIndex="32" MinWidth="20" 
									ToolTip="Net Income month to date as a percentage of month to date revenue">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Net YTD $" FieldName="net_ytd_dol" 
									ShowInCustomizationForm="True" VisibleIndex="33" MinWidth="20" 
									ToolTip="Net Income Fiscal Year to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
									<CellStyle BackColor="#FF99CC">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Net YTD %" FieldName="net_ytd_pct" 
									ShowInCustomizationForm="True" VisibleIndex="34" MinWidth="20" 
									ToolTip="Net Income Fiscal Year to Date as a percentage of Year to Date Revenue">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Net BMTD $" FieldName="net_bmtd_dol" 
									ShowInCustomizationForm="True" VisibleIndex="35" MinWidth="20" 
									ToolTip="Net Income Budget Month to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Net BMTD %" FieldName="net_bmtd_pct" 
									ShowInCustomizationForm="True" VisibleIndex="36" MinWidth="20" 
									ToolTip="Net Income Month to Date as a percentage of Month to Date Revenue">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Net BYTD $" FieldName="net_bytd_dol" 
									ShowInCustomizationForm="True" VisibleIndex="37" MinWidth="20" ToolTip="Net Income Year to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Net BYTD %" FieldName="net_bytd_pct" 
									ShowInCustomizationForm="True" VisibleIndex="38" MinWidth="20" 
									ToolTip="Net Income year to date as a percentage of year to date revenue">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Rev Budget MTD" ShowInCustomizationForm="True" 
									VisibleIndex="39" FieldName="rev_bmtd" MinWidth="20" ToolTip="Budget Revenue Month to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
									<CellStyle BackColor="#CCFFCC">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Rev Budget YTD" ShowInCustomizationForm="True" 
									VisibleIndex="40" FieldName="rev_bytd" MinWidth="20" ToolTip="Budget Revenue Fiscal Year to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
									<CellStyle BackColor="#CCFF99">
									</CellStyle>
								</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="GM Budget YTD" ShowInCustomizationForm="True" 
									VisibleIndex="41" FieldName="gm_bytd" MinWidth="20" ToolTip="Budget Gross Margin Fiscal Year to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
    							</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="GM Budget MTD" ShowInCustomizationForm="True" 
									VisibleIndex="42" FieldName="gm_bmtd" MinWidth="20" ToolTip="Budget Gross Margin  Month to Date">
									<PropertiesTextEdit DisplayFormatString="C0">
									</PropertiesTextEdit>
    							</dx:GridViewDataTextColumn>
                                 <dx:GridViewDataTextColumn Caption="GM Budget % YTD" ShowInCustomizationForm="True" 
									VisibleIndex="43" FieldName="gm_pbytd" MinWidth="20" ToolTip="Budget Gross Margin % Fiscal Year to Date">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
    							</dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="GM Budget % MTD" ShowInCustomizationForm="True" 
									VisibleIndex="44" FieldName="gm_pbmtd" MinWidth="20" ToolTip="Budget Gross Margin %  Month to Date">
									<PropertiesTextEdit DisplayFormatString="P0">
									</PropertiesTextEdit>
    							</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior ColumnResizeMode="Control" />
							<SettingsPager PageSize="50">
							</SettingsPager>
							<Settings ShowFilterBar="Visible" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowFooter="True" ShowGroupPanel="True" />
							<Styles>
								<Header Wrap="True">
								</Header>
								<Cell Wrap="False" HorizontalAlign="Center">
								</Cell>
							    <Footer HorizontalAlign="Center">
                                </Footer>
							</Styles>
						</dx:ASPxGridView>
					</div>
					<div class="ov_body" id="ov_body" runat="server">
						<asp:SqlDataSource ID="sds_dashoverview" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
                            call get_daily_snapshot_at_date(@mid,@dt);
                            ">
							<SelectParameters>
								<asp:ControlParameter Name="@mid" ControlID="hdn_mid" />
                                <asp:ControlParameter Name="@dt" ControlID="cbo_dtes" />
								<asp:SessionParameter Name="@show_only_branch" SessionField="show_only_branch" DbType="String" />
							</SelectParameters>
						</asp:SqlDataSource>
					</div>
				</div>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxPanel>
    <asp:HiddenField runat="server" ID="hdn_mid"/>
	<dx:ASPxPanel ID="panel_bm" runat="server" Width="1100px" Visible="false">
		<PanelCollection>
		<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
			<div class='heading'>
				<div style="clear:both;" id="manager_title" runat="server">Business Unit Manager Dashboard</div>
				<div class='switcher' style="float:left;width:200px;padding:5px;" id="company_switch_div" runat="server">
					<b style='display:block;font-size:12px;'>Business Unit</b>
					<asp:DropDownList runat="server" ID="company_switch" DataTextField="name" DataValueField="business_unit_id" />
				</div>
				
			</div>
			<div style="clear:both;">
			<a href="./dashboard.aspx?a=load_all" id="show_all_link" runat="server" style="display:block;width:250px;padding:3px;font-family:arial;font-weight:bold;color:#000;">Back to Viewing All Branches</a>
				<asp:HiddenField ID="business_unit_id" Value="" runat="server" />
				<table cellpadding="2" cellspacing="0">
					<tr>
						<td valign="top" width="465" rowspan="1">
							<div class="if" style="width:465px;height:1000px;"><iframe id="ifr_wo" runat="server" frameborder="0" width="465" height="1000" scrolling="no"></iframe></div>
							<div class="if" style="width:465px;height:375px;"><iframe id="ifr_worstcustomers" runat="server" frameborder="0" height="375" name="i_worstcustomers" scrolling="auto" width="465px"></iframe></div>
						</td>
						<td valign='top'>
							<div class='tiles'>
								<div class="if" style="width:210px;height:205px;"><iframe id="ifr_rev" runat="server" frameborder="0" width="210" height="205" scrolling="no"></iframe></div>
								<div class="if" style="width:210px;height:205px;"><iframe id="ifr_net" runat="server" frameborder="0" width="210" height="205" scrolling="no"></iframe></div>
								<div class="if" style="width:210px;height:205px;"><iframe id="ifr_margin" runat="server" frameborder="0" width="210" height="205" scrolling="no"></iframe></div>
								<div class="if" style="width:210px;height:205px;"><iframe id="ifr_hourutilization" runat="server" frameborder="0" width="210" height="205" scrolling="no"></iframe></div>
								<div class="if" style="width:210px;height:205px;"><iframe id="ifr_gross" runat="server" frameborder="0" width="210" height="205" scrolling="no"></iframe></div>
								<div class="if" style="width:210px;height:205px;"><iframe id="ifr_customers" runat="server" frameborder="0" width="210" height="205" scrolling="no"></iframe></div>
								<div class="if" style="width:210px;height:255px;"><iframe id="ifr_salespipeline" runat="server" frameborder="0" width="210" height="355" scrolling="no" name="I1"></iframe></div>
								<div class="if" style="width:210px;height:200px;"><iframe id="ifr_inventory" runat="server" frameborder="0" width="210" height="200" scrolling="no"></iframe></div>
							</div>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<hr />
								<div class="if" style="width:100%;height:150px;"><iframe id="ifr_grids" runat="server" frameborder="0" width="100%" height="150" scrolling="no"></iframe></div>
							<hr />
						</td>
					</tr>
                    <tr>
						<td colspan="2">
							<a name="hc"/>
							<div class="if" style="width:100%;"><iframe id="ifr_headcount" runat="server" frameborder="0" width="100%" height="250" scrolling="yes" horizontalscrolling="no" verticalscrolling="yes" name="headcount"></iframe></div>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<a name="dd"/>
							<div class="if" style="width:100%;"><iframe id="ifr_drilldown" runat="server" frameborder="0" width="100%" height="750" scrolling="yes" horizontalscrolling="no" verticalscrolling="yes" name="drilldown"></iframe></div>
						</td>
					</tr>
                   
				</table>
			</div>
		</dx:PanelContent>
</PanelCollection>
	</dx:ASPxPanel>
</ASP:CONTENT>
        
