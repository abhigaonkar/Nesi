<%@ Page Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" Inherits="mobile_callRecord" Codebehind="callRecord.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>

	
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_body" runat="Server">
    


<script type="text/javascript">
	function go() {
	    submit_Click();
	}

	function move_on1() {

		setTimeout(function () { ddl_update_contact.Focus(); }, 100);
	}

	function move_on2() {

		setTimeout(function () { ddl_new_company0.Focus(); }, 100);
	}
	function move_on3() {

		
		setTimeout(function () { txt_notes.Focus(); }, 100);
	}
	
	
</script>
	
	
	

<asp:UpdatePanel runat="server" ID="up">
	<ContentTemplate>
        <div style="width:90%;padding:5%;">
		<table  cellpadding="3px" 
				
				style="border-collapse: collapse; padding-right: 6px; padding-left: 6px; font-family: Calibri; font-size: medium; font-weight: 700;" 
				>
		<tr>
		<td class="ui-priority-primary" style="padding-right: 10px; padding-left: 10px;" 
				colspan="2">
			Record a note on the following call:</td>
		</tr>
		<tr>
		<td class="ui-priority-primary" style="padding-right: 10px; padding-left: 10px;">
			<b><span style="font-size: large; font-family: Calibri">Number:</span></b><span 
				class="ui-priority-primary" style="font-size: large; font-family: Calibri">
			</span>
		</td>
		<td style="padding-right: 10px">
			<asp:TextBox ID="calledNumber" runat="server" ReadOnly="True" Width="100%"></asp:TextBox>
		</td>
		</tr>
		<tr>
		<td style="padding-right: 10px; padding-left: 10px;">
			&nbsp;</td>
		<td style="padding-right: 10px">
			&nbsp;</td>
		</tr>
		<tr>
		<td bgcolor="#CEE7FF" class="ui-priority-primary" 
				style="padding-right: 10px; padding-left: 10px;">
			Company:</td>
		<td bgcolor="#CEE7FF" style="padding-right: 10px">
			<asp:SqlDataSource ID="sql_update_company" runat="server" 
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" EnableCaching="True" 
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select address.Address_ID id, concat('(Cust) ',customer.customer_name,' - ',Address_Addr1) _name, 1 level from customer inner join address on address.Address_Table_ID = customer.customer_id and (address.address_table = 'Customer' or address.address_table='Worksite') where customer.Customer_Status!=6 and customer.Customer_Status !=4 
UNION
Select address.Address_ID id, concat('(Vendor) ',vendor_name,' - ',Address_Addr1) _name, 2 level from vendor inner join address on address.Address_Table_ID = vendor.Vendor_ID and address.address_table = 'Vendor'
order by level,  _name"></asp:SqlDataSource>
			<dx:ASPxComboBox ID="ddl_update_company" runat="server" 
				CallbackPageSize="10" ClientInstanceName="ddl_update_company" 
				DataSourceID="sql_update_company" DropDownRows="5" DropDownWidth="100px" 
				EnableCallbackMode="True" EnableViewState="False" FilterMinLength="2" 
				Font-Bold="False" Font-Names="Arial" Font-Size="14pt" Height="40px" 
				 TextField="_name" ValueField="id" ValueType="System.Int32" 
				Width="100%" DropDownStyle="DropDown">
                

                    
    


				<ClientSideEvents SelectedIndexChanged="function(s, e) {
	ddl_update_contact.PerformCallback();
	move_on1();
}" />
 
				<ItemStyle Paddings-PaddingBottom="5" Paddings-PaddingTop="5">
				<Paddings PaddingBottom="10px" PaddingTop="10px" Padding="10px" />
				<BorderBottom BorderStyle="Solid" BorderWidth="1px" />
				</ItemStyle>
				<SettingsLoadingPanel Enabled="False" Text="" />
				
				<ButtonStyle Width="25px">
				</ButtonStyle>
				<Paddings PaddingBottom="5px" PaddingTop="5px" />
			</dx:ASPxComboBox> 


			</td>
		<tr>
		<td bgcolor="#CEE7FF" class="ui-priority-primary" 
				style="padding-right: 10px; padding-left: 10px;">
			<b><span style="font-size: large; font-family: Calibri">Name:</span></b></td>
		<td bgcolor="#CEE7FF" style="padding-right: 10px">
			<asp:SqlDataSource ID="sql_update_contact" runat="server" 
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
contact.Contact_ID,
contact.Contact_Name
FROM
contact
WHERE
?id is not null and 
contact.address_id = ?id">
				<SelectParameters>
					<asp:ControlParameter ControlID="ddl_update_company" Name="id" 
						PropertyName="Value" />
				</SelectParameters>
			</asp:SqlDataSource>
			<dx:ASPxComboBox ID="ddl_update_contact" runat="server" 
				ClientInstanceName="ddl_update_contact" DataSourceID="sql_update_contact" 
				Font-Names="Arial" Height="40px" TextField="Contact_Name" 
				ValueField="Contact_ID" ValueType="System.Int32" Width="100%" CallbackPageSize="20" 
				DropDownWidth="200px" DropDownRows="20" DropDownStyle="DropDown" 
				oncallback="ddl_update_contact_Callback1" Font-Size="14pt"  
				>
				<ClientSideEvents SelectedIndexChanged="function(s, e) {
	
	move_on2();
}" />
				<ItemStyle>
				<Paddings Padding="10px" PaddingBottom="10px" PaddingTop="10px" />
				<BorderBottom BorderStyle="Solid" BorderWidth="1px" />
				</ItemStyle>
				<SettingsLoadingPanel Enabled="False" Text="" />
				<ButtonStyle Width="25px">
				</ButtonStyle>
			</dx:ASPxComboBox>
		</td>
		<tr>
		<td bgcolor="#CEE7FF" style="padding-right: 10px; padding-left: 10px;">
		</td>
		<td bgcolor="#CEE7FF" 
				style="padding-right: 10px; ">
			Or Add New Contact -&gt;<asp:TextBox ID="txt_new_contact" runat="server" 
				Height="40px" Width="100%" Font-Names="Arial" Font-Size="14pt" autocomplete="off"  AutoCompleteType="Disabled"></asp:TextBox>
			</td>
		<tr>
		<td bgcolor="#CEE7FF" style="padding-right: 10px; padding-left: 10px;">
			Number Type:</td>
		<td bgcolor="#CEE7FF" 
				style="padding-right: 10px; ">
			<dx:ASPxComboBox ID="ddl_new_company0" runat="server" CallbackPageSize="10" 
				ClientInstanceName="ddl_new_company0" EnableCallbackMode="True" 
				Font-Names="Arial" Height="40px"  Width="100%" Font-Size="14pt">
				<ClientSideEvents SelectedIndexChanged="function(s, e) {
	
	move_on3();
}" />
				<Items>
					<dx:ListEditItem Text="Cell" Value="Cell" />
					<dx:ListEditItem Text="Direct Line" Value="Direct Line" />
					<dx:ListEditItem Text="Company Line" Value="Company Line" />
				</Items>
				<ItemStyle>
				<Paddings Padding="10px" PaddingBottom="10px" PaddingTop="10px" />
				<BorderBottom BorderStyle="Solid" BorderWidth="1px" />
				</ItemStyle>
				<SettingsLoadingPanel Enabled="False" Text="" />
				<ButtonStyle Width="25px">
				</ButtonStyle>
			</dx:ASPxComboBox>
			</td>
		<tr>
		<td style="padding-right: 10px; padding-left: 10px;"><b></b></td>
		<td style="padding-right: 10px">
			&nbsp;</td>
		</tr>
		<tr>
		<td style="padding-right: 10px; padding-left: 10px;" bgcolor="#FFFFCC"><b>Notes:</b></td>
		<td style="padding-right: 10px" bgcolor="#FFFFCC">
			&nbsp;</td>
		</tr>
			<tr>
				<td bgcolor="#FFFFCC" colspan="2" 
					style="padding-right: 10px; padding-left: 10px;">
					<dx:ASPxMemo ID="txt_notes" runat="server" Height="71px" Width="100%" 
						ClientInstanceName="txt_notes" Font-Names="Arial" Font-Size="12pt">
					</dx:ASPxMemo>
					</td>
			</tr>
			<tr>
				<td align="left">
					&nbsp;</td>
				<td align="right">
					&nbsp;</td>
			</tr>
			<tr>
				<td align="left">
					<asp:Button ID="END" runat="Server" Height="40px" 
						Text="Close" Width="100px" />
				</td>
				<td align="right">
					<dx:ASPxButton ID="btn_save" runat="server" AutoPostBack="False" Height="40px" 
						onclick="submit_Click" Text="Save Entry" UseSubmitBehavior="False" 
						Width="100px">
						<ClientSideEvents Click="function(s, e) {

                            if (ddl_update_company.GetText() == ''){
                                e.processOnServer=false;
		                        Android.andAlert('inValid Data','You must select an company','noFun');
                                alert('You must select an company');
                            }
                            else if(ddl_update_contact.GetSelectedIndex()&lt;0)
	                        {
		                        if(ddl_update_company.GetSelectedIndex()&lt;0)
		                        {
		                        e.processOnServer=false;
		                            Android.andAlert('inValid Data','You must either select an existing contact or add a new one','noFun');
                                    alert('You must either select an existing contact or add a new one');
		                        }
		                        else
		                        {
		                        e.processOnServer=true;
		                        }
	                        }
	                        else
	                        {
	                        e.processOnServer=true;
	                        }


                        }" />
					</dx:ASPxButton>
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;</td>
			</tr>
		</table>
           
            </div>
			</ContentTemplate>
			</asp:UpdatePanel>
</asp:Content>
