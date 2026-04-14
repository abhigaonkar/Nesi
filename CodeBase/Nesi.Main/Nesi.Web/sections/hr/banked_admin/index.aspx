<%@ Page Language="C#" MasterPageFile="../../../IntraDefault.master" AutoEventWireup="true" Inherits="BankedAdmin" Title="Banked Admin" Theme="NETheme01" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
		<asp:SqlDataSource ID="Users" runat="server"></asp:SqlDataSource>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<script type='text/javascript' src='/js/hr/banked.js'></script>
	<div id='banked_admin' align="left">
		<input type="hidden" id="member_ID" value="<asp:Literal Runat='server' ID='memberID'></asp:Literal>" />
		<table class='framework' border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td valign="top" align="left" id="detail" class="detail" runat="server">
				</td>
			</tr>
		</table>
		<dx:ASPxCallbackPanel ID="cbp" runat="server" ClientInstanceName="cbp" Width="100%" oncallback="cbp_Callback">
			<PanelCollection>
<dx:PanelContent runat="server">
	<dx:ASPxComboBox ID="cb_branch" runat="server" Caption="Select Branch" DataSourceID="ods_branch" TextField="name" ValueField="id" ValueType="System.Int32">
		<ClientSideEvents SelectedIndexChanged="function(s, e) {
	cbp.PerformCallback();
}" />
	</dx:ASPxComboBox>
	<asp:ObjectDataSource ID="ods_branch" runat="server" SelectMethod="units_active" TypeName="nesi.core.NeBusinessUnit"></asp:ObjectDataSource>
	
    <dx:ASPxGridViewExporter ID="gve_bankedadmin" runat="server" GridViewID="gv_bankedadmin" OnRenderBrick="gve_bankedadmin_RenderBrick">
	</dx:ASPxGridViewExporter>
	<dx:ASPxButton ID="bt_export" runat="server" OnClick="bt_export_Click" Text="Export to Excel">
		<Image Url="~/images/icon/icon[excel].gif">
		</Image>
	</dx:ASPxButton>
	<br />
	<br />
	<dx:ASPxGridView ID="gv_bankedadmin" runat="server" AutoGenerateColumns="False" DataSourceID="sds_bankedadmin" Width="750px" OnHtmlDataCellPrepared="gv_bankedadmin_HtmlDataCellPrepared">
		<Columns>
			<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" ShowInCustomizationForm="True" VisibleIndex="0" Name="business_unit">
				<Settings FilterMode="DisplayText" />
				<CellStyle HorizontalAlign="Center"></CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Name" FieldName="FULL_NAME" ShowInCustomizationForm="True" VisibleIndex="1">
				<CellStyle HorizontalAlign="Center"></CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Status" FieldName="status" ShowInCustomizationForm="True" VisibleIndex="2">
				<CellStyle HorizontalAlign="Center"></CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Balance" FieldName="balance" ShowInCustomizationForm="True" VisibleIndex="3" Name="balance">
				<Settings FilterMode="DisplayText" />
				<CellStyle HorizontalAlign="Center"></CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Hours" FieldName="hours" ShowInCustomizationForm="True" VisibleIndex="5" Name="hours">
				<Settings FilterMode="DisplayText" />
				<CellStyle HorizontalAlign="Center"></CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Withdraw" ShowInCustomizationForm="True" VisibleIndex="6" Width="35px" Name="add">
				<DataItemTemplate>
					<dx:ASPxButton ID="bt" runat="server" Width="100%" Height="20px" AutoPostBack="false" OnInit="bt_init">
						<Image Url="~/images/icon/icon[add].gif">
						</Image>
						<ClientSideEvents Click="bank_admin.withdraw" />
					</dx:ASPxButton>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Deposit" ShowInCustomizationForm="True" VisibleIndex="7" Width="35px" Name="add">
				<DataItemTemplate>
					<dx:ASPxButton ID="bt" runat="server" Width="100%" Height="20px" AutoPostBack="false" Tooltip="" OnInit="bt_init">
						<Image Url="~/images/icon/icon[remove].gif">
						</Image>
						<ClientSideEvents Click="bank_admin.deposit" />
					</dx:ASPxButton>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Deduct" ShowInCustomizationForm="True" VisibleIndex="8" Width="35px" Name="deduct">
				<DataItemTemplate>
					<dx:ASPxButton ID="bt" runat="server" Width="100%" Height="20px" AutoPostBack="false" ToolTip="This is used when money is owed to the user's business unit." OnInit="bt_init">
						<Image Url="~/images/icon/icon[cancel].gif">
						</Image>
						<ClientSideEvents Click="bank_admin.deduct" />
					</dx:ASPxButton>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Ledger" ShowInCustomizationForm="True" VisibleIndex="9" Width="35px" Name="ledger">
				<DataItemTemplate>
					<dx:ASPxButton ID="bt" runat="server" Width="100%" Height="20px" AutoPostBack="false" OnInit="bt_init">
						<Image Url="~/images/icon/icon[report].gif">
						</Image>
						<ClientSideEvents Click="bank_admin.ledger" />
					</dx:ASPxButton>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Payout" ShowInCustomizationForm="True" VisibleIndex="10" Width="35px" Name="payout">
				<DataItemTemplate>
					<dx:ASPxButton ID="bt" runat="server" Width="100%" Height="20px" AutoPostBack="false" OnInit="bt_init">
						<Image Url="~/images/icon/icon[bank].gif">
						</Image>
						<ClientSideEvents Click="bank_admin.payout" />
					</dx:ASPxButton>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="sds_bankedadmin" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="	SELECT *, balance/wage hours, wage FROM (SELECT 
		A.member_id id, 
Get_Wage(a.member_id) current_wage,
		a.member_fullname FULL_NAME, 
		a.member_status status,
		d.ddl_name business_unit,
		a.member_nickname FIRST_NAME, 
(SELECT IFNULL(SUM(hours * payrate), 0) AS banked FROM bankedpay_ledger WHERE type='D' AND member_id = a.member_id) balance,
		C.wage WAGE 
	FROM 
		member A
	LEFT JOIN 
		currentwage C 
			ON A.member_id = C.member_id 
	LEFT JOIN
		business_unit d	ON a.business_unit_id = d.id
	WHERE 
		A.business_unit_id = @business_unit_id AND 
		a.paytype_id = 1 AND
		C.wage != 0 
	ORDER BY 
		a.member_status, a.member_fullname) asdf">
		<SelectParameters>
			<asp:ControlParameter ControlID="cb_branch" Name="@business_unit_id" PropertyName="Value" />
		</SelectParameters>
	</asp:SqlDataSource>
				</dx:PanelContent>
</PanelCollection>
		</dx:ASPxCallbackPanel>
		<br />
	</div>
</asp:Content>
