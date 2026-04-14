<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_accounting_terms" MasterPageFile="~/IntraDefault.master" Codebehind="terms.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Data.Linq" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Xpo.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Xpo" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript">
		$(document).ready(function()
			{
			page_obj.update_panel_progress.bind();
			});
	</script>
	<br />
	<dx:ASPxGridView ID="gv_term" runat="server" Theme="NeTheme01" AutoGenerateColumns="False" DataSourceID="xpo_ds" KeyFieldName="term_id" OnRowInserted="gv_term_RowInserted" OnRowUpdated="gv_term_RowUpdated"  OnCellEditorInitialize="gv_term_CellEditorInitialize" >
		<Columns>
			<dx:GridViewCommandColumn ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0">
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn FieldName="term_code" VisibleIndex="2" Caption="Code" >
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="term_desc" VisibleIndex="3" Caption="Description">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="term_daysdiscount" VisibleIndex="4" Caption="Days Discount">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="term_discountrate" VisibleIndex="5" Caption="Discount Rate" >
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="term_daysbeforedue" VisibleIndex="6" Caption="Days till Due">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsPager PageSize="100">
		</SettingsPager>
	</dx:ASPxGridView>
	<dx:XpoDataSource ID="xpo_ds" runat="server" ServerMode="True" TypeName="ne_xpo.cs.term">
</dx:XpoDataSource>
	<br />
	<br />
	<br />
	<br />
</asp:Content>