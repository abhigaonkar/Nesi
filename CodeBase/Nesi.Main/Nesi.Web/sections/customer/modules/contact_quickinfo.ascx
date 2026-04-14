<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_customer_modules_contact_quickinfo" Codebehind="contact_quickinfo.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<style type="text/css">

	.style1
	{
		font-size: small;
		font-weight: bold;
	}
	</style>
	<script type="text/javascript">
	
	</script>
    <div>
    
		<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%">
			<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<table ID="Table1" runat="server" style="width:100%; font-family: Arial;">
		<tr runat="server">
			<td runat="server" class="style1">
				ID:</td>
			<td runat="server" colspan="2">
				<dx:ASPxLabel ID="lbl_popcontact_id" runat="server">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr ID="Tr1" runat="server">
			<td ID="Td1" runat="server" class="style1">
				Name:</td>
			<td ID="Td2" runat="server" colspan="2">
				<dx:ASPxTextBox ID="txt_popcontact_name" runat="server" Font-Names="Arial" 
					Width="170px">
					<ValidationSettings ValidationGroup="newcontact">
						<RegularExpression ErrorText="You must enter both their first and last name." 
							ValidationExpression="^([a-zA-Z]+\s)([a-zA-Z](\.?)\s){0,1}([a-zA-Z'-]+)$" />
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr ID="Tr2" runat="server">
			<td ID="Td3" runat="server" class="style1">
				Title:</td>
			<td ID="Td4" runat="server" colspan="2">
				<dx:ASPxComboBox ID="dd_popcontact_title" runat="server" Font-Names="Arial" 
					DataSourceID="sqltitle" TextField="title_name" ValueField="title_id" 
					ValueType="System.Int32">
					<ValidationSettings ValidationGroup="newcontact">
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxComboBox>
				<asp:SqlDataSource ID="sqltitle" runat="server" 
					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
					SelectCommand="Select * from titles"></asp:SqlDataSource>
			</td>
		</tr>
		<tr ID="Tr3" runat="server">
			<td ID="Td5" runat="server" class="style1">
				Email:</td>
			<td ID="Td6" runat="server" colspan="2">
				<dx:ASPxTextBox ID="txt_popcontact_email" runat="server" Font-Names="Arial" 
					Width="170px">
					<ValidationSettings ValidationGroup="newcontact">
						<RegularExpression ErrorText="You must enter a valid email address" 
							ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr ID="Tr4" runat="server">
			<td ID="Td7" runat="server" class="style1">
				Phone:</td>
			<td ID="Td8" runat="server" colspan="2">
				<dx:ASPxTextBox ID="txt_popcontact_phone" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr ID="Tr5" runat="server">
			<td ID="Td9" runat="server" class="style1">
				Cell:</td>
			<td ID="Td10" runat="server" colspan="2">
				<dx:ASPxTextBox ID="txt_popcontact_cell" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr ID="Tr6" runat="server">
			<td ID="Td11" runat="server" class="style1">
				Facebook:</td>
			<td ID="Td12" runat="server" colspan="2">
				<dx:ASPxTextBox ID="txt_popcontact_facebook" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr ID="Tr7" runat="server">
			<td ID="Td13" runat="server" class="style1">
				Twitter:</td>
			<td ID="Td14" runat="server" colspan="2">
				<dx:ASPxTextBox ID="txt_popcontact_twitter" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr ID="Tr8" runat="server">
			<td ID="Td15" runat="server" class="style1">
				LinkedIn:</td>
			<td ID="Td16" runat="server" colspan="2">
				<dx:ASPxTextBox ID="txt_popcontact_linkedin" runat="server" Font-Names="Arial" 
					Width="170px">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr ID="Tr9" runat="server">
			<td ID="Td17" runat="server" class="style1">
				Status:</td>
			<td ID="Td18" runat="server" colspan="2">
				<dx:ASPxComboBox ID="ddl_popcontact_status" runat="server" Font-Names="Arial" 
					DataSourceID="sqlstatus" TextField="customer_or_contact_status" 
					ValueField="customer_or_contact_status_id" ValueType="System.Int32">
				</dx:ASPxComboBox>
				<asp:SqlDataSource ID="sqlstatus" runat="server" 
					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
					SelectCommand="Select * from customer_or_contact_status">
				</asp:SqlDataSource>
			</td>
		</tr>
		<tr ID="Tr10" runat="server">
			<td ID="Td19" runat="server" class="style1">
				&nbsp;</td>
			<td ID="Td20" runat="server" colspan="2">
				&nbsp;</td>
		</tr>
		<tr ID="Tr11" runat="server">
			<td ID="Td21" runat="server" class="style1">
				Birthday:</td>
			<td ID="Td22" runat="server" colspan="2">
				<dx:ASPxDateEdit ID="dte_popcontact_birthday" runat="server" 
					DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd" Font-Names="Arial">
				</dx:ASPxDateEdit>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style1">
				&nbsp;</td>
			<td runat="server" colspan="2">
				&nbsp;</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style1">
				Nesi Login Status:</td>
			<td runat="server" colspan="2">
				<dx:ASPxComboBox ID="ddl_popcontact_loginstatus" runat="server" 
					Font-Names="Arial">
					<Items>
						<dx:ListEditItem Text="InActive" Value="0" />
						<dx:ListEditItem Text="Active" Value="1" />
					</Items>
				</dx:ASPxComboBox>
			</td>
		</tr>
		<tr ID="Tr12" runat="server">
			<td ID="Td23" runat="server" class="style1">
				Nesi Login:</td>
			<td ID="Td24" runat="server" colspan="2">
				<dx:ASPxTextBox ID="txt_popcontact_username" runat="server" Font-Names="Arial" 
					Width="170px">
					<ValidationSettings ValidationGroup="newcontact">
						<RegularExpression ErrorText="You must enter a valid email address" 
							ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr runat="server">
			<td runat="server" class="style1">
				Nesi Password:</td>
			<td runat="server" colspan="2">
				<dx:ASPxTextBox ID="txt_popcontact_password" runat="server" Font-Names="Arial" 
					Width="170px">
					<ValidationSettings ValidationGroup="newcontact">
						<RegularExpression ErrorText="Password must be at least 6 characters long and must contain a number" 
							ValidationExpression="^(?=.*\d{1})(?=.*[a-zA-Z]{2}).{6,}$" />
					</ValidationSettings>
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr ID="Tr13" runat="server">
			<td ID="Td25" runat="server">
				&nbsp;</td>
			<td ID="Td26" runat="server" colspan="2">
				<dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" 
					ShowErrorsInEditors="True" ValidationGroup="newcontact" Visible="False">
				</dx:ASPxValidationSummary>
			</td>
		</tr>
		<tr ID="Tr14" runat="server">
			<td ID="Td27" runat="server">
				<dx:ASPxButton ID="btnsavecontactpopup" runat="server" AutoPostBack="False" 
					Font-Names="Arial" Text="Save and Close" Width="120px">
					<ClientSideEvents Click="function(s, e) {
if(ASPxClientEdit.ValidateGroup(&quot;newcontact&quot;))
{
//	pop_contact.PerformCallback('s');
//	pop_contact.Hide();
}
}" />
				</dx:ASPxButton>
			</td>
			<td ID="Td28" runat="server">
				&nbsp;</td>
			<td ID="Td29" runat="server">
				&nbsp;</td>
		</tr>
	</table>
				</dx:PanelContent>
</PanelCollection>
		</dx:ASPxCallbackPanel>
		<br />
		

    </div>