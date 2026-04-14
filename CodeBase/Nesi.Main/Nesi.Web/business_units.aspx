<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="business_units" Title="Business Units" CodeBehind="business_units.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="modules/layout_control.ascx" TagName="layout_control" TagPrefix="uc1" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">

	<span id="spanMSG" runat="server"></span>
	<div id="divSearchMSG" runat="server">
		<asp:SqlDataSource ID="sql_te" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select id,public_name from tax_entity "></asp:SqlDataSource>
		<asp:SqlDataSource ID="sql_bu" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select id, name from business_unit"></asp:SqlDataSource>
		<asp:SqlDataSource ID="sql_mt" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select membertype_id,membertype_name from membertype where active = 1"></asp:SqlDataSource>
		<asp:SqlDataSource ID="sqlGrid" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL GET_BUSINESS_UNIT_EDIT_LIST(@member_id)">
			<SelectParameters>
				<asp:Parameter Name="@member_id" />
			</SelectParameters>
		</asp:SqlDataSource>
		<asp:SqlDataSource ID="sql_member" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select member_id,member_fullname from member"></asp:SqlDataSource>
		<uc1:layout_control ID="layout" runat="server" />
	</div>

	<dx:ASPxGridView ID="gv" runat="server" Theme="MaterialCompact" AutoGenerateColumns="False"
		ClientInstanceName="gv" Width="100%" KeyFieldName="bu_id"
		OnCustomCallback="gv_CustomCallback" OnCustomJSProperties="gv_CustomJSProperties" DataSourceID="sqlGrid" OnHtmlDataCellPrepared="gv_HtmlDataCellPrepared">
		<Columns>
			<dx:GridViewDataTextColumn Caption="Address" FieldName="Address" VisibleIndex="5">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Active" FieldName="active" VisibleIndex="6">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Tax Entity" FieldName="tax_entity_id" VisibleIndex="0">
				<PropertiesComboBox DataSourceID="sql_te" TextField="public_name" ValueField="id" ValueType="System.Int32">
				</PropertiesComboBox>
				<Settings HeaderFilterMode="CheckedList" />
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="name" VisibleIndex="2">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server"
						NavigateUrl="<%# string.Format(&quot;javascript:boing('/business_units_edit.aspx?id={0}', 'edit_business_unit{0}', 1200,800)&quot;, Eval(&quot;bu_id&quot;)) %>" OnDataBound="ASPxHyperLink1_OnDataBound"
						Text='<%# Eval("name") %>'>
					</dx:ASPxHyperLink>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Manager" FieldName="bm_id" VisibleIndex="3">
				<PropertiesComboBox DataSourceID="sql_member" TextField="member_fullname" ValueField="member_id" ValueType="System.Int32">
				</PropertiesComboBox>
				<Settings HeaderFilterMode="CheckedList" />
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataComboBoxColumn Caption="Title" FieldName="mb_mt_id" VisibleIndex="4">
				<PropertiesComboBox DataSourceID="sql_mt" TextField="membertype_name" ValueField="membertype_id" ValueType="System.Int32">
				</PropertiesComboBox>
				<Settings HeaderFilterMode="CheckedList" />
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="ID" FieldName="bu_id" VisibleIndex="1">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Warehouse" FieldName="warehouse_bu_id" VisibleIndex="9">
				<PropertiesComboBox DataSourceID="sql_bu" TextField="name" ValueField="id" ValueType="System.Int32">
				</PropertiesComboBox>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="NetSuite ID" FieldName="netsuite_bu_internal_id" VisibleIndex="8">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsPager Mode="ShowAllRecords">
		</SettingsPager>
		<SettingsResizing ColumnResizeMode="Control"></SettingsResizing>
		<Settings ShowFilterRow="True" ShowGroupPanel="True" />
		<SettingsBehavior EnableRowHotTrack="True"></SettingsBehavior>
		<Styles>
			<Header>
				<Paddings Padding="3px" />
			</Header>
			<Cell HorizontalAlign="Center">
				<Paddings Padding="3px" />
			</Cell>
		</Styles>
	</dx:ASPxGridView>
</asp:Content>

