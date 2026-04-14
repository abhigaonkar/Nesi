<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_customer_modules_phone" Codebehind="phone.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<dx:ASPxGridView ID="gv_phones" ClientInstanceName="gv_phones" runat="server" 
	Width="100%" DataSourceID="ds_phones" AutoGenerateColumns="False" 
	KeyFieldName="id" oncommandbuttoninitialize="gv_phones_CommandButtonInitialize" 
	Theme="NETheme01">
		<ClientSideEvents RowClick="function(s, e) {s.StartEditRow(e.visibleIndex);}" />
    <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
		<Columns>
			<dx:GridViewCommandColumn ButtonType="Image" Caption="Action" VisibleIndex="0" ShowInCustomizationForm="True" Width="50px" ShowEditButton="true" ShowClearFilterButton="true"   ShowNewButton="true">
				
				
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="Type" FieldName="type" VisibleIndex="1" Width="175px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Number" FieldName="number" VisibleIndex="2">
			</dx:GridViewDataTextColumn>
		</Columns>
	<Templates>
		<EditForm>
			<table width="100%" cellpadding="2" cellspacing="0">
				<tr>
					<td width="150"><b>Type: </b></td>
					<td><asp:DropDownList ID="ddl_type" runat="server" SelectedValue='<%# Eval("type") %>'>
						<asp:ListItem Value="">Please Select a Communications Type</asp:ListItem>
						<asp:ListItem Value="Fax">Fax</asp:ListItem>
						<asp:ListItem Value="LandLine">LandLine</asp:ListItem>
						</asp:DropDownList></td>
				</tr>
				<tr>
					<td><b>Phone Number: </b></td>
					<td><asp:TextBox ID="tb_number" runat="server" Text='<%# Eval("number") %>'></asp:TextBox></td>
				</tr>
				<tr>
					<td colspan="2" id="error_report" class="error_report" runat="server"></td>
				</tr>
				<tr>
					<td><asp:Button ID="btn_save" runat="server" Text="Save" onclick="btn_save_Click" /></td>
					<td><button type='button' onclick="gv_phones.CancelEdit();">Cancel</button></td>
				</tr>
			</table>
		</EditForm>
	</Templates>
</dx:ASPxGridView>
<asp:SqlDataSource ID="ds_phones" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
	phone_numbers_comm_type type, 
	phone_numbers_number number, 
	phone_numbers_id id 
FROM 
	phone_numbers 
WHERE 
	phone_numbers_type = 'Address' AND 
	phone_numbers_table_id = @address_id">
	<SelectParameters>
		<asp:ControlParameter ControlID="hdnaddressid" DefaultValue="0" 
			Name="@address_id" PropertyName="Value" />
	</SelectParameters>
</asp:SqlDataSource>

<asp:HiddenField ID="hdnaddressid" runat="server" />



