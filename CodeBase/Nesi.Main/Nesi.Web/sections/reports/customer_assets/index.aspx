<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true"  Inherits="sections_reports_customer_assets_index" Title="Customer Assets" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<script type="text/javascript">
		function email(id) {
			boing("/sections/reports/invoice_preview/index.aspx?id=" + id, 'invoice_preview', 850, 850);
		}
	</script>
	<lc:LayoutControl runat="server" ID="layout" is_private="True" />
	<asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
	<asp:UpdatePanel ID="up" runat="server">
	<ContentTemplate>
	<dx:ASPxGridView ID="gv_customer_assets" runat="server" AutoGenerateColumns="False" 
			ClientInstanceName="gv_customer_assets" KeyFieldName="id" 
			OnCellEditorInitialize="gv_customer_assets_CellEditorInitialize" 
			OnCustomCallback="gv_customer_assets_CustomCallback" 
			OnCustomJSProperties="gv_customer_assets_CustomJSProperties" 
			OnRowInserting="gv_customer_assets_RowInserting" 
			OnRowDeleting="gv_customer_assets_RowDeleting" 
			OnRowUpdating="gv_customer_assets_RowUpdating" Width="100%" 
			OnClientLayout="gv_customer_assets_ClientLayout" 
			onhtmleditformcreated="gv_customer_assets_HtmlEditFormCreated" 
			oninitnewrow="gv_customer_assets_InitNewRow" Theme="NETheme01">
        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
		<Columns>
			<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" Width="60px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true"   ShowNewButton="true">
				
				
				
				
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" VisibleIndex="1" Width="50px">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Asset Name" FieldName="name" VisibleIndex="4" Width="100px">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Customer" FieldName="customer_id" VisibleIndex="2" Width="100px">
				<PropertiesComboBox CallbackPageSize="25" AnimationType="None" EnableCallbackMode="True" IncrementalFilteringDelay="350" IncrementalFilteringMode="StartsWith" TextField="customer_n" ValueField="customer_id" ValueType="System.Int32">
					<ClientSideEvents SelectedIndexChanged="function(s, e) {
	cb_location.PerformCallback(s.GetValue());
}" />
				</PropertiesComboBox>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="Description" FieldName="description" VisibleIndex="5" Width="100%">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataCheckColumn Caption="Active" FieldName="active" VisibleIndex="8" Width="50px">
				<PropertiesCheckEdit AllowGrayedByClick="False" ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
				</PropertiesCheckEdit>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataTextColumn Caption="Added By Who?" FieldName="addedby" VisibleIndex="10" Width="100px">
				<EditFormSettings Visible="False" />
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Manufacturer" FieldName="manufacturer" VisibleIndex="6" Width="250px">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Model" FieldName="model" VisibleIndex="7" Width="100px">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Location" FieldName="address_id" VisibleIndex="3" Width="200px">
				<PropertiesComboBox ClientInstanceName="cb_location" EnableCallbackMode="True">
				</PropertiesComboBox>
			</dx:GridViewDataComboBoxColumn>
		</Columns>
		<SettingsBehavior ConfirmDelete="True" ColumnResizeMode="Control" />
		<SettingsPager PageSize="25">
		</SettingsPager>
		<SettingsEditing EditFormColumnCount="4" Mode="PopupEditForm" />
		<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" ShowHeaderFilterButton="True" />
		<SettingsText ConfirmDelete="Are you sure you want to delete this asset?  This history will also be deleted..." />
		<SettingsLoadingPanel ImagePosition="Top" />
		<SettingsPopup>
			<EditForm HorizontalAlign="LeftSides" Modal="True" />
		</SettingsPopup>
		<StylesPopup>
			<EditForm>
				<ModalBackground Opacity="0">
				</ModalBackground>
			</EditForm>
		</StylesPopup>
		<Templates>
			<EditForm>
				<table>
					<tr>
						<td>Customer:</td>
						<td>
							<dx:ASPxComboBox ID="cb_customer" runat="server" ValueType="System.Int32" DataSourceID="ds_customers" TextField="name" ValueField="id" Value='<%# Bind("customer_id") %>' EnableCallbackMode="True" IncrementalFilteringMode="Contains" Theme="NETheme01">
								<ClientSideEvents SelectedIndexChanged="function(s, e) {
	cb_location.PerformCallback();
}" />
							</dx:ASPxComboBox>
						</td>	
						<td rowspan="8">
							<iframe id="frm_files" runat="server" frameborder="0" height="450" width="800"></iframe></td>
					</tr>
					<tr>
						<td>Location:</td>
						<td>
							<dx:ASPxComboBox ID="cb_location" runat="server" ValueType="System.Int32"  DataSourceID="ds_locations" TextField="name" ValueField="id" Value='<%# Bind("address_id") %>' ClientInstanceName="cb_location" EnableCallbackMode="True" IncrementalFilteringMode="Contains" oncallback="cb_location_Callback" Theme="NETheme01">
								<ClientSideEvents BeginCallback="function(s, e) {
	please_wait('start', 'Refreshing Locations');
}" CallbackError="function(s, e) {
	please_wait('stop');
}" EndCallback="function(s, e) {
	please_wait('stop');
}" />
							</dx:ASPxComboBox>
						</td>	
					</tr>
					<tr>
						<td>Asset Name:</td>
						<td>
							<dx:ASPxTextBox ID="tb_name" runat="server" Width="170px" Text='<%# Bind("name") %>' Theme="NETheme01">
							</dx:ASPxTextBox>
						</td>	
					</tr>
					<tr>
						<td>Description:</td>
						<td>
							<dx:ASPxTextBox ID="tb_description" runat="server" Width="170px" Text='<%# Bind("description") %>' Theme="NETheme01">
							</dx:ASPxTextBox>
						</td>	
					</tr>
					<tr>
						<td>Manufacturer:</td>
						<td>
							<dx:ASPxTextBox ID="tb_manufacturer" runat="server" Width="170px" Text='<%# Bind("manufacturer") %>' Theme="NETheme01">
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td>Model:</td>
						<td>
							<dx:ASPxTextBox ID="tb_model" runat="server" Width="170px" Text='<%# Bind("model") %>' Theme="NETheme01">
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td>Active:</td>
						<td>
							<dx:ASPxCheckBox ID="chk_active" runat="server" Checked="True" Value='<%# Bind("active") %>' Theme="NETheme01"></dx:ASPxCheckBox>
						</td>
					</tr>
					<tr style="height: 100%">
						<td></td>
						<td>
							
						</td>
					</tr>
				</table>
				<div style="margin-top: 10px; margin-bottom: 20px; padding-bottom: 20px;">
					<div style="float: left; margin-left: 5px">
						<dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" Text="Cancel" Width="100px" ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' Theme="NETheme01" />
					</div>
					<div style="float: left; margin-left: 10px">
						<dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" Text="Save" Width="100px" CssClass="input" ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' Theme="NETheme01" />
					</div>
					<asp:SqlDataSource ID="ds_customers" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT customer_id id,customer_name name FROM customer WHERE customer_status NOT IN (4,5,6) ORDER BY customer_name"></asp:SqlDataSource>
					<asp:SqlDataSource ID="ds_locations" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT address_id id, CONCAT('[',address_type,'] - ', address_addr1) name FROM address WHERE address_table = 'Customer' AND address_table_id = @customer_id
">
						<SelectParameters>
							<asp:ControlParameter ControlID="cb_customer" Name="customer_id" PropertyName="Value" Type="Int32" />
						</SelectParameters>
					</asp:SqlDataSource>
				</div>
			</EditForm>
		</Templates>
	</dx:ASPxGridView>
		<br />
	</ContentTemplate>
	</asp:UpdatePanel>
	<br />
</asp:Content>
