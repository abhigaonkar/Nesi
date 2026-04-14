<%@ Page Title="" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_workorder_if_associated" Codebehind="if_associated.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="header_placeholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<table><tr><td>
	<dx:ASPxComboBox ID="combo_availablewos" runat="server" DataSourceID="sds_available_wos" IncrementalFilteringMode="Contains" TextField="name" ValueField="id" ValueType="System.Int32" ClientInstanceName="combo_availablewos" oncallback="combo_availablewos_Callback">
	</dx:ASPxComboBox>
	<asp:SqlDataSource ID="sds_available_wos" runat="server" 
			ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
a.woprog_id id, CONCAT(a.woprog_bvwo, &quot; - &quot;, a.woprog_customername, &quot; - &quot;, LEFT(a.woprog_description, 100)) name 
FROM
woprog a
WHERE 
a.woprog_cutdatetime > curdate()-interval 900 day and
a.woprog_id != @woprog_id AND 
a.business_unit_id = @business_unit_id AND
IFNULL(a.woprog_bvwo, 0) != 0 AND
IFNULL(a.woprog_description, &quot;&quot;) != &quot;&quot; AND
a.woprog_id NOT IN (SELECT woprog_id_b FROM wo_associated WHERE woprog_id_b = a.woprog_id AND woprog_id_a = @woprog_id) AND
a.woprog_id NOT IN (SELECT woprog_id_a FROM wo_associated WHERE woprog_id_a = a.woprog_id AND woprog_id_b = @woprog_id)
">
		<SelectParameters>
			<asp:QueryStringParameter Name="@woprog_id" QueryStringField="woprog_id" />
			<asp:QueryStringParameter Name="@business_unit_id" QueryStringField="business_unit_id" />
		</SelectParameters>
	</asp:SqlDataSource></td><td>
			<dx:ASPxButton ID="b_link" runat="server" AutoPostBack="False" Text="Create Link" Theme="NETheme01">
				<ClientSideEvents Click="function(s, e) {
	cb_link.PerformCallback(combo_availablewos.GetValue());
}" />
				<Image Url="~/images/icon/icon[link].gif">
				</Image>
			</dx:ASPxButton>
		</td></tr></table>
	<dx:ASPxGridView ID="gv_change_orders" runat="server" AutoGenerateColumns="False" 
		ClientInstanceName="gv_change_orders" DataSourceID="sds_change_wos" 
		oncustombuttoncallback="gv_linkage_CustomButtonCallback" Width="100%" 
		Font-Names="Arial" 
		Theme="NETheme01">
		<ClientSideEvents CustomButtonClick="function(s, e) {
		e.processOnServer = confirm(&quot;Are you sure you want to delete this linkage?&quot;);
}" />
		<Columns>
			<dx:GridViewDataTextColumn FieldName="woprog_id" Visible="False" VisibleIndex="0">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="WO" FieldName="woprog_bvwo" 
				VisibleIndex="1" Width="150px">
				<DataItemTemplate>
					<dx:ASPxHyperLink Theme="NETheme01" ID="ASPxHyperLink3" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}', 0, 1024, 700)&quot;, Eval(&quot;woprog_id&quot;), Eval(&quot;business_unit_id&quot;)) %>" Text='<%# Eval("woprog_bvwo") %>' />
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="branch" VisibleIndex="2" Width="100px">
            </dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Description" FieldName="woprog_description" 
				VisibleIndex="3" Width="100%" MinWidth="100">
				<CellStyle HorizontalAlign="Left" Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
            
			
            <dx:GridViewDataTextColumn Caption="Invoice Amount" FieldName="invoice_amount" VisibleIndex="4" Width="90px">
                <PropertiesTextEdit DisplayFormatString="c2">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Status" FieldName="woprog_status" VisibleIndex="5" Width="100px" CellStyle-HorizontalAlign="Center">
                <CellStyle HorizontalAlign="Center">
				</CellStyle>
            </dx:GridViewDataTextColumn>
<dx:GridViewCommandColumn ButtonType="Image" Caption="Delete Link" VisibleIndex="6" Width="100px" ShowClearFilterButton="true" Name="Delete">
				
				<CustomButtons>
					<dx:GridViewCommandColumnCustomButton ID="Delete0">
						<Image AlternateText="Delete Link" Height="16px" Url="~/images/icon/icon[delete].gif" Width="16px">
						</Image>
					</dx:GridViewCommandColumnCustomButton>
				</CustomButtons>
			</dx:GridViewCommandColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<Settings ShowTitlePanel="True" ShowFooter="True" />
        <SettingsText Title="Change Orders Within this Business Unit" />
		<TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="invoice_amount" ShowInColumn="Invoice Amount" ShowInGroupFooterColumn="Invoice Amount" SummaryType="Sum" ValueDisplayFormat="C2" />
        </TotalSummary>
		<Styles>
			<Header HorizontalAlign="Center">
			</Header>
		</Styles>
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="sds_change_wos" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL change_workorders(@woprog_id, 1)">
		<SelectParameters>
			<asp:QueryStringParameter Name="@woprog_id" QueryStringField="woprog_id" />
		</SelectParameters>
	</asp:SqlDataSource>
			<br />
	<dx:ASPxGridView ID="gv_linkage" runat="server" AutoGenerateColumns="False" 
		ClientInstanceName="gv_linkage" DataSourceID="sds_linkage" 
		 Width="100%" 
		Font-Names="Arial"  
		Theme="NETheme01">
		<ClientSideEvents CustomButtonClick="function(s, e) {
		e.processOnServer = confirm(&quot;Are you sure you want to delete this linkage?&quot;);
}" />
		<Columns>
			<dx:GridViewDataTextColumn FieldName="woprog_id" Visible="False" VisibleIndex="0">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="WO" FieldName="woprog_bvwo" 
				VisibleIndex="1" Width="150px">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}', 0, 1024, 700)&quot;, Eval(&quot;woprog_id&quot;), Eval(&quot;business_unit_id&quot;)) %>" Text='<%# Eval("woprog_bvwo") %>' />
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="branch" VisibleIndex="2" Width="100px">
            </dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Description" FieldName="woprog_description" 
				VisibleIndex="3" Width="100%" MinWidth="100">
				<CellStyle HorizontalAlign="Left" Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
            
			
            <dx:GridViewDataTextColumn Caption="Invoice Amount" FieldName="invoice_amount" VisibleIndex="4" Width="90px">
                <PropertiesTextEdit DisplayFormatString="c2">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Status" FieldName="woprog_status" VisibleIndex="5" Width="100px">
            </dx:GridViewDataTextColumn>
<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="6" Width="100px" ShowClearFilterButton="true">
				
				
			</dx:GridViewCommandColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<Settings ShowTitlePanel="True" ShowFooter="True" />
        <SettingsText Title="Progress Bills Within this Business Unit" />
		<TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="invoice_amount" ShowInColumn="Invoice Amount" ShowInGroupFooterColumn="Invoice Amount" SummaryType="Sum" ValueDisplayFormat="C2" />
        </TotalSummary>
		<Styles>
			<Header HorizontalAlign="Center">
			</Header>
		</Styles>
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="sds_linkage" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL associated_workorders(@woprog_id, 1)">
		<SelectParameters>
			<asp:QueryStringParameter Name="@woprog_id" QueryStringField="woprog_id" />
		</SelectParameters>
	</asp:SqlDataSource>
			<br />
	<dx:ASPxGridView ID="gv_linkage0" runat="server" AutoGenerateColumns="False" 
		ClientInstanceName="gv_linkage" DataSourceID="sds_linkage0" 
		 Width="100%" 
		Font-Names="Arial" 
		Theme="NETheme01">
		
		<Columns>
			<dx:GridViewDataTextColumn FieldName="woprog_id" Visible="False" VisibleIndex="0">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="WO" FieldName="woprog_bvwo" 
				VisibleIndex="1" Width="150px">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}', 0, 1024, 700)&quot;, Eval(&quot;woprog_id&quot;), Eval(&quot;business_unit_id&quot;)) %>" Text='<%# Eval("woprog_bvwo") %>' />
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="branch" VisibleIndex="2" Width="100px">
            </dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Description" FieldName="woprog_description" 
				VisibleIndex="3" Width="100%" MinWidth="100">
				<CellStyle HorizontalAlign="Left" Wrap="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			
            <dx:GridViewDataTextColumn Caption="Invoice Amount" FieldName="invoice_amount" VisibleIndex="4" Width="90px">
                <PropertiesTextEdit DisplayFormatString="c2">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Status" FieldName="woprog_status" VisibleIndex="5" Width="100px">
            </dx:GridViewDataTextColumn>
           
<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="6" Width="100px"  ShowClearFilterButton="true" >
				
				
			</dx:GridViewCommandColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<Settings ShowTitlePanel="True" ShowFooter="True" />
        <SettingsText Title="Other business units (Child Work Orders)" />
		<TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="C2" FieldName="invoice_amount" ShowInColumn="Invoice Amount" ShowInGroupFooterColumn="Invoice Amount" SummaryType="Sum" ValueDisplayFormat="C2" />
        </TotalSummary>
		<Styles>
			<Header HorizontalAlign="Center">
			</Header>
		</Styles>
	</dx:ASPxGridView>
			<dx:ASPxCallback ID="cb_link" runat="server" ClientInstanceName="cb_link" oncallback="cb_link_Callback">
				<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" CallbackComplete="function(s, e) {
		combo_availablewos.PerformCallback();
		
                    gv_change_orders.Refresh();
		please_wait(&quot;stop&quot;);
}" />
			</dx:ASPxCallback>
	<asp:SqlDataSource ID="sds_linkage0" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select a.woprog_id, Concat(name,' - ',business_unit.ddl_name) branch, a.business_unit_id,a.WOProg_StillToBeBilled invoice_amount,a.woprog_status, a.woprog_description ,a.woprog_bvwo  from woprog a inner join business_unit on business_unit.id = a.business_unit_id  where a.parent_woprog_id =@woprog_id">
		<SelectParameters>
			<asp:QueryStringParameter Name="@woprog_id" QueryStringField="woprog_id" />
		</SelectParameters>
	</asp:SqlDataSource>
			</asp:Content>

