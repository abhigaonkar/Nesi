<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_member_modules_it" Codebehind="it.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<div style="border:solid 1px #EAEAEA; margin:10px;">
<div class="sub_h">
<table cellpadding="2" cellspacing="0" width="100%">
<tr>
<td width="50%">Login / Email Information</td>
<td width="50%" align="right">
	<dx:ASPxButton ID="bt_savepanel" runat="server" Text="Save Panel" CausesValidation="False" onclick="bt_savepanel_Click">
		<Image Url="~/images/icon/icon[save].gif">
		</Image>
	</dx:ASPxButton>
</td>
</tr>
</table>
</div>
<table style="width:100%;">
	<tr>
		<td class="c" width="200">
			NESI Username:</td>
		<td width="200" >
			<dx:ASPxTextBox ID="tb_nesi_user" runat="server" Width="170px">
			</dx:ASPxTextBox>
		</td>
		<td>
			&nbsp;</td>
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="c">
			NESI Password:</td>
		<td >
			<dx:ASPxTextBox ID="tb_nesi_pass" runat="server" Width="170px">
			</dx:ASPxTextBox>
		</td>
		<td>
			&nbsp;</td>
		<td>
			<img src="/images/icon/icon[attention].gif" id="img_pass_sync_error" runat="server" width="16" height="16" visible="false" /></td>
	</tr>
	<tr>
		<td class="c">
			Old Employee ID:</td>
		<td >
			<dx:ASPxTextBox ID="tb_old_employee_id" runat="server" Width="170px">
			</dx:ASPxTextBox>
		</td>
		<td>
			&nbsp;</td>
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="c">
			Old Employee Login:</td>
		<td >
			<dx:ASPxTextBox ID="tb_old_employee_login" runat="server" Width="170px">
			</dx:ASPxTextBox>
		</td>
		<td>
			&nbsp;</td>
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="c">
			Company Email Address:</td>
		<td >
			<dx:ASPxTextBox ID="tb_email_address" runat="server" Width="170px">
			</dx:ASPxTextBox>
		</td>
		<td>
			<dx:ASPxCheckBox ID="chkemail" runat="server" ClientEnabled="True" 
				Text="Should have Email">
			</dx:ASPxCheckBox>
		</td>
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="c">
			LDAP Username:</td>
		<td >
			<dx:ASPxTextBox ID="tb_ldap" runat="server" Width="170px">
			</dx:ASPxTextBox>
		</td>
		<td>
			&nbsp;</td>
		<td>
			&nbsp;</td>
	</tr>
	<tr style="display:none">
		<td class="c">
			LDAP Password:</td>
		<td >
			<dx:ASPxTextBox ID="tb_ldap_pass" runat="server" Width="170px">
			</dx:ASPxTextBox>
		</td>
		<td>
			&nbsp;</td>
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="c">
			Extension:</td>
		<td >
			<dx:ASPxTextBox ID="tb_ext" runat="server" Width="170px">
			</dx:ASPxTextBox></td>
		<td>
			<dx:ASPxCheckBox ID="chkext" runat="server" ClientEnabled="True" 
				Text="Should have and Extension">
			</dx:ASPxCheckBox>
		</td>
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="c">
			Cellphone ID (Hardware):</td>
		<td >
			
			<dx:ASPxComboBox ID="ASPxComboBox1" runat="server" 
				DataSourceID="SqlDataSource1" TextField="_number" ValueField="id" 
				ValueType="System.Int32" IncrementalFilteringMode="StartsWith">
			</dx:ASPxComboBox>
			<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
	0 id,
	'No Cellphone' _number
UNION 
SELECT
	a.id,
	CONCAT(a.imei,' - ',b.name) _number
FROM
	cellphone a,
	cellphone_status b
WHERE
	a.cellphone_status_id = b.id and 
	ifnull((select MAX(member_id) from member where cellphone_id = a.id limit 1),0) = 0
UNION
SELECT
	a.id,
	CONCAT(imei,' - ',name) _number
FROM
	cellphone a,
	cellphone_status b,
	member c
where 
	a.cellphone_status_id = b.id and 
	a.id = c.cellphone_id and 
	c.member_id = ?mid">
				<SelectParameters>
					<asp:ControlParameter ControlID="hdncompid" Name="cid" PropertyName="Value" />
					<asp:ControlParameter ControlID="hdnid" Name="mid" PropertyName="Value" />
				</SelectParameters>
			</asp:SqlDataSource>
			
		<td>
			<dx:ASPxCheckBox ID="chkcell" runat="server" 
				Text="Should have Cellphone">
			</dx:ASPxCheckBox>
		</td>
			
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="c">
			Company Cellphone Number (SIM):</td>
		<td >
			
			<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" 
				DataSourceID="SqlDataSource2" TextField="_number" ValueField="id" 
				ValueType="System.Int32" IncrementalFilteringMode="StartsWith">
			</dx:ASPxComboBox>
			<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
				SelectCommand="
SELECT 
	0 id,
	'No Number' _number
UNION 
SELECT
	a.id,
	concat(a.number,' - ',a.status) _number
FROM
	cellphone_number a
WHERE
	a.status='Active' and 
	ifnull((select MAX(member_id) from member where cellphone_number_id = a.id limit 1),0) = 0
UNION
SELECT
	id,
	concat(a.number,' - ',a.status) _number
FROM
	cellphone_number a
where 
	ifnull((select member_id from member where cellphone_number_id = a.id limit 1),0) = 0
union
select a.id,Concat(a.number,' - ',a.status) from cellphone_number a,member b where a.id = b.cellphone_number_id and b.member_id = ?mid">
				<SelectParameters>
					<asp:ControlParameter ControlID="hdncompid" Name="cid" PropertyName="Value" />
					<asp:ControlParameter ControlID="hdnid" Name="mid" PropertyName="Value" />
				</SelectParameters>
			</asp:SqlDataSource>
			
		<td>
			&nbsp;</td>
			
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="c">
			Barcode Scanner:</td>
		<td >
			
			&nbsp;<td>
			<dx:ASPxCheckBox ID="chkbc" runat="server" 
				Text="Should have Barcode Scanner">
			</dx:ASPxCheckBox>
		</td>
			
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="c">
			Laptop:</td>
		<td >
			
			&nbsp;<td>
			<dx:ASPxCheckBox ID="chklaptop" runat="server" 
				Text="Should have Laptop">
			</dx:ASPxCheckBox>
		</td>
			
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="c">
			Include in Mobile Contact List:</td>
		<td >
			
			<dx:ASPxCheckBox ID="chk_include_in_mobile" runat="server">
			</dx:ASPxCheckBox>
		<td>
			&nbsp;</td>
			
		<td>
			&nbsp;</td>
	</tr>
	
	<tr>
		<td class="c">
			Last Mobile Login:</td>
		<td >
			
			<dx:ASPxLabel ID="lbl_last_mobile_login" runat="server" Theme="NETheme01">
            </dx:ASPxLabel>
        <td>
			&nbsp;</td>
			
		<td>
			&nbsp;</td>
	</tr>
	<tr>
		<td class="c" colspan="3">
			<dx:ASPxLabel id="lbl_error" EncodeHtml="false" runat="server" ForeColor="Red"></dx:ASPxLabel></td>
	</tr>
</table>
</div>
<asp:HiddenField ID="hdnid" runat="server" />
<asp:HiddenField ID="hdncompid" runat="server" />
