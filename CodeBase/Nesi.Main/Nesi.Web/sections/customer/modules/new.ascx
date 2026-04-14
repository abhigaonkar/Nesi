<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_customer_modules_new" EnableTheming="True" Codebehind="new.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"	Namespace="DevExpress.Web" TagPrefix="dx" %>

<script type="text/javascript" src="/js/functions.js"></script>
					Please fill in the fields denoted with an asterisk (*) first.<br />
					Afterward you will be directed to the new customer's file.<br />
					<dx:ASPxLabel ID="lb_newerror" EncodeHtml="false" runat="server" Font-Bold="True" ForeColor="Red">
					</dx:ASPxLabel>
					<br />
					<table width="100%" cellpadding="2" cellspacing="0" style="font-family: Arial">
						<tr>
							<td colspan="3">
								<div style="width:300px;background-color:#0c3;color:#fff;padding:5px; font-size: 12px;font-weight:bold;">Basic Info 
									(Related to Billing Address)</div>
								<table cellspacing="0" cellpadding="3" style="background-color: #ddd; border: solid 1px #0c3; font-size: 12px; width: 100%;">
									<tr>
										<td style="width: 170px">Default Branch*:</td>
										<td>
											<dx:ASPxComboBox ID="newcustomer_com" Theme="NETheme01"  runat="server" TextField="name" ClientInstanceName="newcustomer_company" ValueField="id" ValueType="System.Int32" Width="100%"  MaxLength="60" DataSourceID="ods_branch" onselectedindexchanged="newcustomer_company_SelectedIndexChanged" AutoPostBack="True">
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField IsRequired="True" ErrorText="Please select a branch" />
												</ValidationSettings>
											</dx:ASPxComboBox>
											<asp:ObjectDataSource ID="ods_branch" runat="server" SelectMethod="units_active" TypeName="nesi.core.NeBusinessUnit"></asp:ObjectDataSource>
										</td>
									</tr>
									<tr>
										<td>Project Manager*:</td>
										<td>
											<dx:ASPxComboBox ID="newcustomer_projmgr" Theme="NETheme01"  runat="server" TextField="name" ValueField="member_id" ValueType="System.Int32" Width="100%"  MaxLength="60" DropDownStyle="DropDown" DataSourceID="ds_proj_manager" onselectedindexchanged="newcustomer_projmgr_SelectedIndexChanged">
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField IsRequired="True" ErrorText="Please select a project manager" />
												</ValidationSettings>
											</dx:ASPxComboBox>
											<asp:SqlDataSource ID="ds_proj_manager" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
	a.member_id member_id,
	CONCAT(b.name, ' - ', member_fullname) name
FROM 
	Member  a
LEFT JOIN
 business_unit b ON a.business_unit_id = b.id

                                                left join membertype mt on a.member_membertype_id = mt.membertype_id
WHERE 
	(a.business_unit_id = @business_unit_id AND 
	member_status = 'Active') OR
	(mt.considered_pm AND 
	member_status = 'Active') 
ORDER BY 
	b.name, a.member_firstname,a.member_lastname">
												<SelectParameters>
													<asp:ControlParameter ControlID="newcustomer_com" Name="@business_unit_id" PropertyName="Value" Type="Int32" />
												</SelectParameters>
											</asp:SqlDataSource>
										</td>
									</tr>
									<tr>
										<td>Account Manager*:</td>
										<td>
											<dx:ASPxComboBox ID="newcustomer_acctmgr" Theme="NETheme01"  runat="server" TextField="member_fullname" ValueField="member_id" ValueType="System.Int32" Width="100%"  MaxLength="60" DropDownStyle="DropDown" DataSourceID="ds_acctmgr">
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField IsRequired="True" ErrorText="Please select an account manager" />
												</ValidationSettings>
											</dx:ASPxComboBox>
	<asp:SqlDataSource ID="ds_acctmgr" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 0 member_id,'Please Select a User' member_fullname,0 orderby UNION SELECT a.member_id, CONCAT(b.name, ' - ', member_fullname) member_fullname, 1 orderby FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE member_status = 'Active' and member_membertype_id IN (4,5,8,11,12,17,27,29,34,35,36,37,38,39,40,41,46,49,52,53,54,55,56,59,60,66,71,75,76,79) 
ORDER BY orderby, member_fullname"></asp:SqlDataSource>
										</td>
									</tr>
									<tr>
										<td valign="top">Origin*:</td>
										<td class="v" valign="top">
											<dx:ASPxComboBox ID="new_combo_origin" Theme="NETheme01"  runat="server" DataSourceID="ds_origin" TextField="name" ValueField="id" ValueType="System.Int32" ClientInstanceName="new_combo_origin">
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField ErrorText="Please provide how we got this customer" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxComboBox>
											<div style='font-size: 10px; font-weight: normal;'>
												(These can be administered on the sales tab)</div>
											<asp:SqlDataSource ID="ds_origin" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, name FROM customer_origin"></asp:SqlDataSource>
										</td>
									</tr>
									<tr>
										<td style="width: 170px">Name*:</td>
										<td>
											<dx:ASPxTextBox runat="server" Theme="NETheme01" AutoCompleteType="None" NullText="Enter Customer Name" Width="90%" ClientInstanceName="popup_name" ID="newcustomer_name" >
												<ClientSideEvents TextChanged="function(s, e) {nc_name_check.PerformCallback(s.GetValue());}" ></ClientSideEvents>
												
												<ValidationSettings Display="Dynamic" ErrorDisplayMode="Text" ValidationGroup="new_customer">
													<RequiredField IsRequired="True" ErrorText="Please provide the customer's name"></RequiredField>
												</ValidationSettings>
											</dx:ASPxTextBox>
											<br />
											<dx:ASPxLabel ID="lb_name_check" runat="server" ClientInstanceName="lb_name_check" Font-Bold="True" ForeColor="Red">
											</dx:ASPxLabel>
											<dx:ASPxCallback ID="nc_name_check" Theme="NETheme01"  runat="server" ClientInstanceName="nc_name_check" OnCallback="nc_name_check_Callback">
												<ClientSideEvents CallbackComplete="function(s, e) {
	var _r				= e.result.split(&quot;,&quot;);
	var _current		= btn_save_new_customer.GetEnabled();
	var _current_txt	= lb_phone_check.GetText();
	var _enable			= (_current || _current_txt == &quot;&quot;) ? true : false;
	if((_r[0]/1) == 0)
		{
		lb_name_check.SetText(&quot;&quot;);
		btn_save_new_customer.SetEnabled(_enable);
		}
	else
		{
		lb_name_check.SetText(&quot;&lt;b style='color:red'&gt;Customer already exists under this name: &lt;a href='./index.aspx?customer_id=&quot;+unescape(_r[1])+&quot;'&gt;Click to Visit&lt;/a&gt;&lt;/b&gt;&quot;);
		btn_save_new_customer.SetEnabled(false);
		}
}" />
											</dx:ASPxCallback>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td colspan="3">
								<div style="width:300px;background-color:#0c3;color:#fff;padding:5px; font-size: 12px;font-weight:bold;">Billing Address</div>
								<table cellspacing="0" cellpadding="3" style="background-color: #ddd; border: solid 1px #0c3; font-size: 12px; width: 100%;">
									<tr>
										<td width="170" rowspan="4" valign="top">
											Billing Address:<br />
											<strong style="font-size: 10px">(Line 1 is required)</strong>
										</td>
										<td>
											<dx:ASPxTextBox ID="newcustomer_addr1" AutoCompleteType="None" Theme="NETheme01" runat="server" NullText="Line 1*" Width="90%" ClientInstanceName="popup_addr1"  MaxLength="45">
												
												<ValidationSettings Display="Dynamic" EnableCustomValidation="True" ErrorDisplayMode="Text" ErrorText="" ValidationGroup="new_customer">
													<RequiredField ErrorText="Please provide at least one address line" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>
											<dx:ASPxTextBox ID="newcustomer_addr2" Theme="NETheme01"  runat="server" NullText="Line 2" Width="90%"  MaxLength="45">
												<NullTextStyle CssClass="null">
												</NullTextStyle>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>
											<dx:ASPxTextBox ID="newcustomer_addr3" Theme="NETheme01"  runat="server" NullText="Line 3" Width="90%"  MaxLength="45">
												<NullTextStyle CssClass="null">
												</NullTextStyle>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>
											<dx:ASPxTextBox ID="newcustomer_addr4" Theme="NETheme01"  runat="server" NullText="Line 4" Width="90%"  MaxLength="45">
												<NullTextStyle CssClass="null">
												</NullTextStyle>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>City*:</td>
										<td>
											<dx:ASPxTextBox ID="newcustomer_ci1" Theme="NETheme01"  runat="server"  Width="50%"  >
												<ValidationSettings ValidationGroup="new_customer" Display="Dynamic">
													<RequiredField ErrorText="Please provide the customer's city" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>Prov/State*:</td>
										<td class="v">
											<dx:ASPxComboBox ID="newcustomer_provstate" Theme="NETheme01"  ClientInstanceName="newcustomer_provstate" runat="server" ValueType="System.String" >
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField IsRequired="True" ErrorText="Please provide a province or state" />
												</ValidationSettings>
											</dx:ASPxComboBox>
										</td>
									</tr>
									<tr>
										<td>Postal Code*:</td>
										<td class="v">
											<dx:ASPxTextBox ID="newcustomer_postal" Theme="NETheme01"  ClientInstanceName="newcustomer_postal" runat="server"  Width="35%"  >
												<ClientSideEvents TextChanged="function(s, e) {}" />
												<ValidationSettings ValidationGroup="new_customer" Display="Dynamic">
													<RequiredField ErrorText="Please provide a postal code" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td>Country*:</td>
										<td>
											<dx:ASPxComboBox ID="newcustomer_country" Theme="NETheme01"  ClientInstanceName="newcustomer_country" runat="server" ValueType="System.String" >
												<ClientSideEvents SelectedIndexChanged="function(s, e) {}" />
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField IsRequired="True" ErrorText="Please select a country" />
												</ValidationSettings>
											</dx:ASPxComboBox>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td colspan="3">
								<div style="width:300px;background-color:#0c3;color:#fff;padding:5px; font-size: 12px;font-weight:bold;">Phone / Fax</div>
								<table cellspacing="0" cellpadding="3" style="background-color:#ddd; border:solid 1px #0c3; font-size: 12px;width:100%;">
									<tr>
										<td width="170"><b>Phone*:</b></td>
										<td>
											<table cellpadding="0" cellspacing="0" style="font-size:12px;">
												<tr>
													<td align="center" valign="middle">(</td>
													<td align="center" valign="middle">
														<dx:ASPxTextBox ID="newcustomer_phone_area" Theme="NETheme01"  ClientInstanceName="newcustomer_phone_area" runat="server" Width="35px" MaxLength="3" >
															<ClientSideEvents TextChanged="function(s, e) {
				if(nc_phone_check.InCallback())
					{
					nc_phone_check.DoEndCallback();
					}
				var p		=	{
								area:			newcustomer_phone_area.GetValue(),
								prefix:			newcustomer_phone_prefix.GetValue(),
								suffix:			newcustomer_phone_suffix.GetValue()
								};
				if(
				p.area != '' &amp;&amp; 
				p.area != null &amp;&amp; 
				p.prefix != '' &amp;&amp; 
				p.prefix != null &amp;&amp; 
				p.suffix != '' &amp;&amp; 
				p.suffix != null) 
					{
					nc_phone_check.PerformCallback(p.area+' '+p.prefix+' '+p.suffix);
					}
			}" />
															<ValidationSettings ValidationGroup="new_customer">
																<RequiredField ErrorText="Phone's area code is required" IsRequired="True" />
															</ValidationSettings>
														</dx:ASPxTextBox>
													</td>
													<td align="center" valign="middle">)</td>
													<td align="center" valign="middle">
														<dx:ASPxTextBox ID="newcustomer_phone_prefix" Theme="NETheme01"  ClientInstanceName="newcustomer_phone_prefix" runat="server" Width="35px" MaxLength="3" >
															<ClientSideEvents TextChanged="function(s, e) {
				if(nc_phone_check.InCallback())
					{
					nc_phone_check.DoEndCallback();
					}
				var p		=	{
								area:			newcustomer_phone_area.GetValue(),
								prefix:			newcustomer_phone_prefix.GetValue(),
								suffix:			newcustomer_phone_suffix.GetValue()
								};
				if(
				p.area != '' &amp;&amp; 
				p.area != null &amp;&amp; 
				p.prefix != '' &amp;&amp; 
				p.prefix != null &amp;&amp; 
				p.suffix != '' &amp;&amp; 
				p.suffix != null) 
					{
					nc_phone_check.PerformCallback(p.area+' '+p.prefix+' '+p.suffix);
					}
			}" />
															<ValidationSettings ValidationGroup="new_customer">
																<RequiredField ErrorText="Phone's prefix is required" IsRequired="True" />
															</ValidationSettings>
														</dx:ASPxTextBox>
													</td>
													<td align="center" valign="middle">-</td>
													<td align="center" valign="middle">
														<dx:ASPxTextBox ID="newcustomer_phone_suffix" Theme="NETheme01"  ClientInstanceName="newcustomer_phone_suffix" runat="server" Width="50px" MaxLength="4" >
															<ClientSideEvents TextChanged="function(s, e) {
				if(nc_phone_check.InCallback())
					{
					nc_phone_check.DoEndCallback();
					}
				var p		=	{
								area:			newcustomer_phone_area.GetValue(),
								prefix:			newcustomer_phone_prefix.GetValue(),
								suffix:			newcustomer_phone_suffix.GetValue()
								};
				if(
				p.area != '' &amp;&amp; 
				p.area != null &amp;&amp; 
				p.prefix != '' &amp;&amp; 
				p.prefix != null &amp;&amp; 
				p.suffix != '' &amp;&amp; 
				p.suffix != null) 
					{
					nc_phone_check.PerformCallback(p.area+' '+p.prefix+' '+p.suffix);
					}
			}" />
															<ValidationSettings ValidationGroup="new_customer">
																<RequiredField ErrorText="Phone's suffix is required" IsRequired="True" />
															</ValidationSettings>
														</dx:ASPxTextBox>
													</td>
													<td align="center" valign="middle"><b>Ext.</b></td>
													<td align="center" valign="middle">
														<dx:ASPxTextBox ID="newcustomer_phone_ext" Theme="NETheme01"  runat="server" Native="True" Width="35px" >
															<ValidationSettings ValidationGroup="new_customer">
															</ValidationSettings>
														</dx:ASPxTextBox>
													</td>
													<td align="left" valign="middle">
														<dx:ASPxLabel ID="lb_phone_check" runat="server" ClientInstanceName="lb_phone_check" ></dx:ASPxLabel>
														<dx:ASPxCallback ID="nc_phone_check" runat="server" Theme="NETheme01"  ClientInstanceName="nc_phone_check" OnCallback="nc_phone_check_Callback">
															<ClientSideEvents CallbackComplete="function(s, e) {
				var _r	= e.result.split(&quot;,&quot;);
				var _enabled		= btn_save_new_customer.GetEnabled();
				var _current_txt	= lb_name_check.GetText();
				var _enable			= (_enabled || _current_txt == &quot;&quot;) ? true : false;
				if((_r[0]/1) == 0)
					{
					lb_phone_check.SetText(&quot;&quot;);
					//btn_save_new_customer.SetEnabled(_enable);
					}
				else
					{
					lb_phone_check.SetText(&quot;&lt;b style='color:red'&gt;Phone Exists on (&lt;a href='./index.aspx?customer_id=&quot;+_r[2]+&quot;'&gt;&quot;+unescape(_r[1])+&quot;&lt;/a&gt;)&lt;/b&gt;&quot;);
					//btn_save_new_customer.SetEnabled(false);
					}
			}"></ClientSideEvents>
														</dx:ASPxCallback>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td><b>Fax:</b></td>
										<td>
											<table cellspacing="0" cellpadding="0" style="font-size:12px;">
												<tbody>
													<tr>
														<td valign="middle" align="center">(</td>
														<td valign="middle" align="center">
															<dx:ASPxTextBox ID="newcustomer_fax_area" runat="server" Width="35px" MaxLength="3">
																</dx:ASPxTextBox>
														</td>
														<td valign="middle" align="center">)</td>
														<td valign="middle" align="center">
															<dx:ASPxTextBox ID="newcustomer_fax_prefix" runat="server" Width="35px" MaxLength="3">
																</dx:ASPxTextBox>
														</td>
														<td valign="middle" align="center">-</td>
														<td valign="middle" align="center">
															<dx:ASPxTextBox ID="newcustomer_fax_suffix" runat="server" Width="50px" MaxLength="4">
																</dx:ASPxTextBox>
														</td>
													</tr>
												</tbody>
											</table>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td colspan="3">
								<div style="width:300px;background-color:#0c3;color:#fff;padding:5px; font-size: 12px;font-weight:bold;">Billing Info</div>
								<table cellspacing="0" cellpadding="3" style="background-color:#ddd; border:solid 1px #0c3; font-size: 12px;width:100%;">
									<tr>
										<td width="170"><b>AP Auto Collection Email*:</b></td>
										<td colspan="2">
											<dx:ASPxTextBox ID="newcustomer_email" Theme="NETheme01"  runat="server" Native="True"  Size="35">
												<ClientSideEvents TextChanged="function(s, e) {main_persistence_handler.Set(&quot;newcustomer_email&quot;, s.GetText());}" Init="function(s, e) {main_persistence_handler.Set(&quot;newcustomer_email&quot;, s.GetText());}" />
												<ValidationSettings ValidationGroup="new_customer">
													<RegularExpression ErrorText="" />
													<RequiredField ErrorText="Please provide an AP email address" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxTextBox>
										</td>
									</tr>
									<tr>
										<td><b>GL Receivables*:</b></td>
										<td colspan="2">
											<dx:ASPxComboBox ID="newcustomer_gl_receivables" Theme="NETheme01"  runat="server" AutoResizeWithContainer="True" ValueType="System.String" >
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField ErrorText="A GL account must be specified" IsRequired="True" />
												</ValidationSettings>
											</dx:ASPxComboBox>
										</td>
									</tr>
									<tr>
										<td>&nbsp;</td>
										<td>
											&nbsp;</td>
										<td>
											&nbsp;</td>
									</tr>
									<tr>
										<td><strong>Tax 1*:</strong></td>
										<td>
											<dx:ASPxComboBox ID="newcustomer_tax1" runat="server" Theme="NETheme01"  AutoResizeWithContainer="True" ValueType="System.String" >
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField ErrorText="A Tax ID must be specified"  />
												</ValidationSettings>
											</dx:ASPxComboBox>
										</td>
										<td>
											&nbsp;</td>
									</tr>
									<tr>
										<td><strong>Tax 2*:</strong></td>
										<td>
											<dx:ASPxComboBox ID="newcustomer_tax2" Theme="NETheme01" runat="server" AutoResizeWithContainer="True" ValueType="System.String" >
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField ErrorText="A Tax ID must be specified"  />
												</ValidationSettings>
											</dx:ASPxComboBox>
										</td>
										<td>
											&nbsp;</td>
									</tr>
                                    <tr>
										<td><strong>Tax 3*:</strong></td>
										<td>
											<dx:ASPxComboBox ID="newcustomer_tax3" Theme="NETheme01"  runat="server" AutoResizeWithContainer="True" ValueType="System.String" >
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField ErrorText="A Tax ID must be specified" />
												</ValidationSettings>
											</dx:ASPxComboBox>
										</td>
										<td>
											&nbsp;</td>
									</tr>
                                    <tr>
										<td><strong>Tax 4*:</strong></td>
										<td>
											<dx:ASPxComboBox ID="newcustomer_tax4" Theme="NETheme01"  runat="server" AutoResizeWithContainer="True" ValueType="System.String" >
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField ErrorText="A Tax ID must be specified"  />
												</ValidationSettings>
											</dx:ASPxComboBox>
										</td>
										<td>
											&nbsp;</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr id="sales_reps" runat="server" visible="false">
							<td colspan="3">
								<div style="width:300px;background-color:#0c3;color:#fff;padding:5px; font-size: 12px;font-weight:bold;">Sales Reps</div>
								<table cellspacing="0" cellpadding="3" style="background-color:#ddd; border:solid 1px #0c3; font-size: 12px;width:100%;">
									<tr>
										<td width="170"><b>ISR:</b></td>
										<td><dx:ASPxComboBox ID="newcustomer_isr" Theme="NETheme01"  runat="server" SelectedIndex="0" ValueType="System.Int32">
												<Items>
													<dx:ListEditItem Text="Please select an ISR" Selected="True"  Value="0" />
													<dx:ListEditItem Text="Sean Kawakami"  Value="1394" />
													<dx:ListEditItem Text="Phil Irwin"  Value="1141" />
													<dx:ListEditItem Text="Marko Mrkonjic"  Value="1463" />
												</Items>
												<ValidationSettings ValidationGroup="new_customer">
													<RequiredField ErrorText="" />
												</ValidationSettings>
											</dx:ASPxComboBox></td>
									</tr>
									<tr>
										<td><b>OSR:</b></td>
										<td><dx:ASPxComboBox ID="newcustomer_osr" Theme="NETheme01"  runat="server" SelectedIndex="0" ValueType="System.Int32">
												<Items>
													<dx:ListEditItem Text="Please select an OSR" Selected="True"  Value="0" />
													<dx:ListEditItem Text="Joe Vrbanac"  Value="1464" />
													<dx:ListEditItem Text="Jimm Villeneuve"  Value="1397" />
												</Items>
											</dx:ASPxComboBox></td>
									</tr>
									<tr>
										<td><b>RAM:</b></td>
										<td><dx:ASPxComboBox ID="newcustomer_ram" Theme="NETheme01"  runat="server" SelectedIndex="0" ValueType="System.Int32">
												<Items>
													<dx:ListEditItem Text="Please select a RAM" Selected="True"  Value="0" />
													<dx:ListEditItem Text="Brian Cook"  Value="1362" />
													<dx:ListEditItem Text="Sean Curran"  Value="784" />
												</Items>
											</dx:ASPxComboBox></td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td style="width: 170px">
							</td>
							<td align="left" style="width: 350px">
								<dx:ASPxValidationSummary ID="newcustomer_valisum" runat="server" RenderMode="BulletedList"
									ValidationGroup="new_customer" >
								</dx:ASPxValidationSummary>
							</td>
							<td align="right">
								<dx:ASPxButton ID="btn_save_new_customer" Theme="NETheme01" runat="server" AutoPostBack="False" Text="Start Customer"
									UseSubmitBehavior="False" ValidationGroup="new_customer" 
									 
									ClientInstanceName="btn_save_new_customer" Font-Names="Arial" Font-Size="9pt" onclick="btn_save_new_customer_Click">
									<ClientSideEvents Click="function(s, e) {
	if(!ASPxClientEdit.ValidateGroup(&quot;new_customer&quot;))
		{
		e.processOnServer	= false;
		}
                                        else
                                        {
                                        please_wait('start');
                                        }
}" />

									<Image Url="~/images/icon/icon[send].gif">
									</Image>
								</dx:ASPxButton>
							</td>
						</tr>
					</table>
 