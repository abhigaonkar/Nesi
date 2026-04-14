<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_customer_modules_header"  EnableTheming="True" Codebehind="header.ascx.cs" %>
	
	<asp:HiddenField ID="hid_ts" runat="server" />
																				<table cellpadding="2" cellspacing="0" style="font-family: Segoe UI,Calibri; font-size: 12px" >
																					<tr>
																						<td align="left"  nowrap="nowrap"  
																							style="width: 125px;" nowrap="nowrap">
																							Customer Name*:</td>
																						<td align="left" style="width: 350px" valign="top">
																				<dx:ASPxTextBox ID="tb_name" runat="server" 
																					Height="25px" Width="400px" Native="True" NullText="Customer Name" meta:resourcekey="detail_customer_nameResource1" MaxLength="60" Theme="NETheme01">
																					
<ClientSideEvents TextChanged="function(s, e) {
cb_current_name_check.PerformCallback(s.GetText());
}"></ClientSideEvents>

																					<NullTextStyle CssClass="null">
																					</NullTextStyle>
																					
																					<ValidationSettings Display="None" ErrorText="" ValidationGroup="addr_valisum">
																						<RequiredField IsRequired="True" ErrorText="Customer Name is required" />
																					</ValidationSettings>

																				</dx:ASPxTextBox>
																							<br />
																							<dx:ASPxLabel ID="lb_current_name_check" runat="server" ClientInstanceName="lb_current_name_check" Font-Bold="True" ForeColor="Red">
																							</dx:ASPxLabel>
																							<dx:ASPxCallback ID="cb_current_name_check" runat="server" Theme="NETheme01" ClientInstanceName="cb_current_name_check"
									OnCallback="detail_customer_name_check_Callback">
																								<ClientSideEvents CallbackComplete="function(s, e) {
	var _r			= e.result.split(&quot;|&quot;);
	var _current	= bt_save.GetEnabled();
	var _enable		= _current ? true : false;
	if((_r[0]/1) == 0)
		{
		lb_current_name_check.SetText(&quot;&quot;);
		bt_save.SetEnabled(true);
		}
	else
		{
		lb_current_name_check.SetText(&quot;&lt;b style='color:red'&gt;Customer already exists under this name: &lt;a href='./index.aspx?customer_id=&quot;+unescape(_r[1])+&quot;'&gt;Click to Visit&lt;/a&gt;&lt;/b&gt;&quot;);
		bt_save.SetEnabled(false);
		}
}" />

																							</dx:ASPxCallback>
																						</td>
																					</tr>
																					<tr>
																						<td align="left" colspan="2" padding-top: 5px;">
																						</td>
																					</tr>
																					<tr>
																						<td align="left" colspan="2">
																					<table cellpadding="2" cellspacing="0" style="font-family: Segoe UI,Calibri; font-size: 12px" >
																					<tr>
																						<td align="left" >
																							Account Manager:<br />
																						</td>
																						<td align="left">
																							<asp:Label ID="h_lblam" runat="server" 
																								Text="Not Set"></asp:Label>
																						</td>
																					</tr>
																					<tr>
																						<td align="left" ">
																							Project Manager:</td>
																						<td align="left" style="width: 350px">
																							<asp:Label ID="h_lblpm" runat="server"  
																								Text="Not Set"></asp:Label>
																						</td>
																					</tr>
																					<tr>
																						<td align="left" nowrap="nowrap" style="color:#777;">
																							<div ">Branch:</div>
																							<div ">(based on proj.manager)</div></td>
																						<td align="left" style="width: 350px">
																							<asp:Label ID="h_lblbranch" runat="server" Theme="NETheme01"
																								Text="Unknown"></asp:Label>
																						</td>
																					</tr>
																				</table>
</td>
																					</tr>
																					<tr>
																						<td align="left" colspan="2">
																							&nbsp;</td>
																					</tr>
																					<tr>
																						<td align="left"  valign="top" style="width: 125px">
																							<dx:ASPxCheckBox ID="chk_on_hold" runat="server" Checked="True" 
																								CheckState="Checked" Layout="Flow" meta:resourceKey="on_holdResource1" 
																								Text="On Hold?" TextAlign="Left" Width="100px" Font-Bold="False" ClientInstanceName="chk_on_hold" Theme="NETheme01">
																								<ClientSideEvents CheckedChanged="function(s, e) {
	main_persistence_handler.Set(&quot;on_hold&quot;, s.GetChecked());
}" />
																							</dx:ASPxCheckBox>
																						</td>
																						<td align="left" nowrap="nowrap">
																							<dx:ASPxMemo ID="memo_whyhold" runat="server" Height="60px" Width="100%" 
																								meta:resourceKey="mem_on_holdResource1" ClientInstanceName="memo_whyhold" Theme="NETheme01">
																							</dx:ASPxMemo>
																							<dx:ASPxLabel ID="lbl_whohold" runat="server" ClientInstanceName="lbl_whohold" EncodeHtml="False" Theme="NETheme01">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td align="left">
																							Status:</td>
																						<td align="left" nowrap="nowrap" >
																							<dx:ASPxComboBox ID="cb_cust_status" runat="server" Native="True" TextField="customer_or_contact_status"
																					ValueField="customer_or_contact_status_id" ValueType="System.Int32" Width="100%" meta:resourcekey="detail_gen_companyResource1" 
																								ClientInstanceName="cb_cust_status" DataSourceID="ds_status" Theme="NETheme01"
																								oninit="cb_cust_status_Init">


																								<ValidationSettings Display="None" ErrorText="" ErrorTextPosition="Bottom">
																									<RequiredField ErrorText="Default branch is required" />
																								</ValidationSettings>
																							</dx:ASPxComboBox>
																						</td>
																					</tr>
																					<tr>
																						<td align="left">
																							Is A Partner:</td>
																						<td align="left" nowrap="nowrap" >
																							<dx:ASPxCheckBox ID="ck_partner" runat="server" CheckState="Unchecked" 
																Theme="NETheme01" Width="75px">
															</dx:ASPxCheckBox></td>
																					</tr>
																					<tr>
																						<td align="left"  valign="top" style="width: 125px">
																							<dx:ASPxButton ID="bt_save" runat="server" ClientInstanceName="bt_save" 
																								Height="25px" onclick="bt_save_Click" Width="55px" HorizontalAlign="Center" Theme="NETheme01">
																								<Image Height="16px" Url="~/images/icon/icon[save].gif" Width="16px">
																								</Image>
																							</dx:ASPxButton>
																						</td>
																						<td align="left" style="width: 350px" nowrap="nowrap">
																							<dx:ASPxLabel ID="lb_error" runat="server" Font-Bold="True" EncodeHtml="false" ForeColor="Red">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																				</table>
<asp:HiddenField ID="hdn_address_id" runat="server" />
<asp:HiddenField ID="hdn_customer_id" runat="server" />
<asp:SqlDataSource ID="ds_branches" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT Company_ID id, name name, Company_DSNBV7 from business_unit  WHERE active = 'T' AND active = 'T'"></asp:SqlDataSource>
<asp:SqlDataSource ID="ds_status" runat="server" 
	ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
	ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
	SelectCommand="SELECT * from customer_or_contact_status order by customer_or_contact_status_id"></asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_acctmgr" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 0 member_id,'Please Select a User' member_fullname,0 orderby UNION SELECT member_id, CONCAT(b.name, ' - ', member_fullname) member_fullname, 1 orderby FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE member_status = 'Active' and member_membertype_id IN (17,5,11,12,8,25,4,29,46, 36, 59, 60) 
ORDER BY orderby, member_fullname"></asp:SqlDataSource>
	
