<%@ Page Title="" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_customer_postal_search" Codebehind="postal_search.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="header_placeholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<dx:ASPxGridView ID="gv_postal" runat="server" AutoGenerateColumns="False" DataSourceID="sds_postal">
		<Columns>
			<dx:GridViewDataTextColumn Caption="Postal Prefix" FieldName="postal_definer" VisibleIndex="0">
				<Settings AutoFilterCondition="Contains" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Branch" FieldName="name" VisibleIndex="1">
				<Settings AutoFilterCondition="Contains" />
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsPager PageSize="30">
		</SettingsPager>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" />
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="sds_postal" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.postal_definer, b.name FROM territory_mapping a LEFT JOIN business_unit b ON a.business_unit_id = b.id ORDER BY b.name, a.postal_definer"></asp:SqlDataSource>
</asp:Content>

