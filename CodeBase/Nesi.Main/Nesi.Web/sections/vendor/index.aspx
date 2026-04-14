<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_vendor_index"  EnableTheming="True" Theme="NETheme01" EnableEventValidation="false" Codebehind="index.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register src="../../modules/partner_skills.ascx" tagname="partner_skills" tagprefix="uc1" %>
<%@ Register src="phone.ascx" tagname="phone" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"/>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Vendor</title>
	<style type="text/css">
		body		{
					padding:			5px;
					margin:				5px;
					}
		.error		{
					overflow-y:			scroll;
					overflow-x:			hidden;
					white-space:		nowrap;
					}
		.style1
		{
			font-family: "Segoe UI";
		}
	</style>

</head>
    <body>
	    <script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	    <script type="text/javascript" src="/js/functions.js"></script>
		<script type="text/javascript" src="/js/jquery-ui-1.7.1.custom.min.js"></script>
        <script type="text/javascript" src="/js/vendor.js"></script>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="sm" runat="server" EnablePageMethods="true"></asp:ScriptManager>
		<div>
			<dx:ASPxCallbackPanel ID="cbp_main" runat="server" Width="100%"  OnCallback="cbp_main_Callback">
				<PanelCollection>
					<dx:PanelContent runat="server" DefaultButton="vend_search_b" SupportsDisabledAttribute="True">
					<table>
						<tr>
						<td align="right" style="height: 50px; width: 854px;">
						<div style="float: left; margin-left: 25px;">
						<dx:ASPxButton ID="b_new" runat="server" AutoPostBack="False" Text="New Vendor" UseSubmitBehavior="False" meta:resourcekey="b_new_Resource1">
							<ClientSideEvents Click="function(s, e) {
	pop_new.Show();
}" />
							<Image Url="~/images/icon/icon[add].gif">
							</Image>
							
						</dx:ASPxButton>
						</div>
						</td>
						<td>
						<div style="float: right; width: 160px; margin-right: 25px">
						<dx:ASPxTextBox ID="vend_search_t" runat="server" AutoResizeWithContainer="True" Width="170px" Native="True" 
						NullText="Search Criteria" EnableViewState="False" meta:resourcekey="vend_search_tResource1">
							<ClientSideEvents KeyDown="function(s, e) {
	if(e.htmlEvent.keyCode == 13)
		{
		e.htmlEvent.preventDefault();
		pc_main.SetActiveTab(pc_main.GetTab(0));
		cbp_main.PerformCallback('search');
		}
}" />
							</dx:ASPxTextBox>
						</div>
						</td>
						<td>
						<div style="float: right; width: 100px">
							
							<dx:ASPxButton ID="vend_search_b" runat="server" style="margin-bottom: 0px" Text="Find" AutoPostBack="False" UseSubmitBehavior="False" VerticalAlign="Middle" 
																	meta:resourcekey="vend_search_bResource1" Font-Names="Arial" Font-Size="9pt">
								<ClientSideEvents Click="function(s, e) {
	pc_main.SetActiveTab(pc_main.GetTab(0));
	cbp_main.PerformCallback('search');
}" />
								<Image Url="~/images/icon/icon[search].gif">
																</Image>
							</dx:ASPxButton>
						</div>
						</td>
						</tr>
						</table>
						<br />
						<dx:ASPxPageControl runat="server" ActiveTabIndex="1" 
							ClientInstanceName="pc_main" ID="pc_main" Width="100%" 
							>
							<TabPages>
								<dx:TabPage Text="Vendors">
									<ContentCollection>
										<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
											<dx:ASPxGridView runat="server" KeyFieldName="vendor_id" AutoGenerateColumns="False"
												DataSourceID="ds_vendor" ID="gv_vendor" Width="100%" ClientInstanceName="gv_vendor" SettingsLoadingPanel-ShowImage="False">
												<Columns>
													<dx:GridViewDataTextColumn Caption="ID" FieldName="vendor_id" ReadOnly="True" VisibleIndex="0"
														Width="50px" ShowInCustomizationForm="True">
														<HeaderStyle HorizontalAlign="Center" />
														<CellStyle HorizontalAlign="Center">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="BV Number" FieldName="vendor_number"
														VisibleIndex="1" Width="75px" ShowInCustomizationForm="True">
														<HeaderStyle HorizontalAlign="Center" />
														<CellStyle HorizontalAlign="Center">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Name" FieldName="vendor_name" 
														VisibleIndex="2" Width="100px" ShowInCustomizationForm="True">
														<Settings AutoFilterCondition="Contains" />
														<DataItemTemplate>
															<dx:ASPxHyperLink ID="hl_name" runat="server" Text='<%# Eval("vendor_name") %>' NavigateUrl="javascript:void();"
																OnInit="hl_name_Init">
															</dx:ASPxHyperLink>
														</DataItemTemplate>
														<HeaderStyle HorizontalAlign="Left" />
														<CellStyle Font-Bold="False">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Phone" FieldName="phone" VisibleIndex="4" Width="100px" ShowInCustomizationForm="True">
														<Settings AutoFilterCondition="Contains" />

														<HeaderStyle HorizontalAlign="Center" />
														<CellStyle HorizontalAlign="Left">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													
													<dx:GridViewDataTextColumn Caption="business_unit_id" VisibleIndex="8" Width="10px" Visible="False" FieldName="business_unit_id" ShowInCustomizationForm="True"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="vendor_number" FieldName="vendor_number" VisibleIndex="5" Width="10px" ShowInCustomizationForm="True" Visible="False"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Address" FieldName="vendor_address" ShowInCustomizationForm="True" VisibleIndex="3" Width="100%"></dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="WWW" FieldName="website" ShowInCustomizationForm="True" VisibleIndex="7" Width="120px">
														<DataItemTemplate>
															<dx:ASPxHyperLink ID="hl_wname" runat="server" NavigateUrl="javascript:void();" OnInit="hl_web_Init" Text='<%# Eval("website") %>'>
															</dx:ASPxHyperLink>
														</DataItemTemplate>
													</dx:GridViewDataTextColumn>
												</Columns>

<SettingsBehavior EnableRowHotTrack="True"></SettingsBehavior>

												<SettingsPager PageSize="25">
												</SettingsPager>
												<Settings ShowFilterRow="True" ShowFilterRowMenu="True"></Settings>

<SettingsLoadingPanel ShowImage="False"></SettingsLoadingPanel>

												<Styles>
													<Cell Wrap="False">
													</Cell>
													
													<DetailCell Wrap="False">
													</DetailCell>
												</Styles>
											</dx:ASPxGridView>
											<asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
												ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
  vendor_id, 
  vendor_number, 
  vendor_name, 
  IF(b.Address_PhoneFirst = &quot;&quot;, &quot;&quot;, CONCAT(&quot;(&quot;,b.Address_PhoneArea,&quot;) &quot;,b.Address_PhoneFirst,&quot;-&quot;,b.Address_PhoneLast)) phone,
  business_unit_id,
  vendor_number,
 CONCAT(b.Address_Addr1,',',b.Address_Addr2,',',b.Address_Addr3,' ',b.Address_City,',',b.Address_Prov) AS vendor_address, 
b.Address_Web AS website 
FROM 
  vendor a
LEFT JOIN
  address b
    ON b.Address_Table = 'Vendor' AND b.Address_Table_ID = a.Vendor_ID 
WHERE 
  vendor_active = TRUE ORDER BY vendor_name"
												ID="ds_vendor"></asp:SqlDataSource>
										</dx:ContentControl>
									</ContentCollection>
								</dx:TabPage>
								<dx:TabPage Text="Detail">
									<ContentCollection>
										<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
											<table cellspacing="0" cellpadding="2" width="100%">
												<tbody>
													<tr>
														<td style="width: 5px">
														</td>
														<td style="WIDTH: 125px" class="style1">
															Vendor Name:</td>
														<td valign="middle">
															<dx:ASPxTextBox ID="t_name" runat="server" AutoResizeWithContainer="True" ClientInstanceName="t_name" Width="300px" ReadOnly="true">
																<ClientSideEvents KeyUp="NE_vendor.current_name_check.run" />
																<ValidationSettings Display="None" ValidationGroup="vs_current">
																	<RequiredField ErrorText="* Vendor Name is Required" IsRequired="True" />
																</ValidationSettings>
															</dx:ASPxTextBox>
														</td>
														<td valign="middle">
															&nbsp;</td>
														<td align="left" rowspan="4" style="width: 250px" valign="top">
															<dx:ASPxLabel ID="lb_vendor_id" runat="server" AssociatedControlID="l_vendor_id" Text="Vendor ID:" Width="85px"></dx:ASPxLabel>
															<dx:ASPxLabel ID="l_vendor_id" runat="server"></dx:ASPxLabel>
															<br />
															<dx:ASPxLabel ID="lb_vendor_number" runat="server" AssociatedControlID="l_vendor_number" Text="Vendor #:" Width="85px"></dx:ASPxLabel>
															<dx:ASPxLabel ID="l_vendor_number" runat="server"></dx:ASPxLabel>
															<br />
															<dx:ASPxLabel ID="lb_date_added" runat="server" AssociatedControlID="l_date_added" Text="Date Added:" Width="85px"></dx:ASPxLabel>
															<dx:ASPxLabel ID="l_date_added" runat="server"></dx:ASPxLabel>
															<br />
															<dx:ASPxLabel ID="lb_added_by" runat="server" AssociatedControlID="l_added_by" Text="Added By:" Width="85px"></dx:ASPxLabel>
															<dx:ASPxLabel ID="l_added_by" runat="server"></dx:ASPxLabel>
															<br />
															<dx:ASPxLabel ID="lb_qc_date" runat="server" AssociatedControlID="l_qc_date" Text="QC Date:" Width="85px"></dx:ASPxLabel>
															<dx:ASPxLabel ID="l_qc_date" runat="server"></dx:ASPxLabel>
														</td>
													</tr>
													<tr>
														<td style="width: 5px">
														</td>
														<td valign="top" class="style1">
															Business Unit:</td>
														<td valign="top">
															<dx:ASPxComboBox ID="cb_main_branch" runat="server" AutoResizeWithContainer="True" TextField="name" ValueField="id" ValueType="System.Int32" Width="300px"  ReadOnly="true">
															</dx:ASPxComboBox>
														</td>
														<td style="width: 50px">
														</td>
													</tr>
													<tr>
														<td style="width: 5px">&nbsp;</td>
														<td class="style1" valign="top">Is On Hold:</td>
														<td valign="top">
															<dx:ASPxCheckBox ID="ck_onhold" runat="server" CheckState="Unchecked"  ReadOnly="true"
																 Width="75px">
															</dx:ASPxCheckBox>
														</td>
														<td style="width: 50px">
															&nbsp;</td>
													</tr>
													<tr>
														<td style="width: 5px">
															&nbsp;</td>
														<td class="style1" valign="top">
															Is A Partner:</td>
														<td valign="top">
															<dx:ASPxCheckBox ID="ck_partner" runat="server" CheckState="Unchecked" ReadOnly="true"
																 Width="75px">
															</dx:ASPxCheckBox>
														</td>
														<td style="width: 50px">
															&nbsp;</td>
													</tr>
													<tr>
														<td style="width: 5px">
														</td>
														<td>
															<dx:ASPxButton ID="b_save_vendor" runat="server" AutoPostBack="False" 
																ClientInstanceName="b_save_vendor" Text="Save Vendor">
																<ClientSideEvents Click="function(s, e) {
	if(ASPxClientEdit.ValidateGroup(&quot;vs_current&quot;))
		{
		//s.SetEnabled(false);
		cbp_main.PerformCallback(&quot;save&quot;);
		}
}" />
																<Image Url="~/images/icon/icon[save].gif">
																</Image>
															</dx:ASPxButton>
														</td>
														<td>
															&nbsp;</td>
														<td style="width: 50px">
														</td>
														<td align="left" valign="middle" style="width: 250px">
															<asp:HiddenField runat="server" ID="hid_ts" />
														</td>
													</tr>
													<tr>
														<td colspan="5">
															<dx:ASPxPageControl runat="server" ActiveTabIndex="2" Width="100%" ID="pc_detail" TabSpacing="2px" ClientInstanceName="pc_detail" OnCallback="pc_detail_Callback" >
																<TabPages>
																	<dx:TabPage Text="Address">
																		<ContentCollection>
																			<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																				<table cellspacing="0" cellpadding="2" width="100%">
																					<tbody>
																						<tr>
																							<td style="color: green; width: 150px;">
																								&nbsp;</td>
																							<td nowrap="nowrap">
																								<dx:ASPxTextBox runat="server" Width="350px" ID="t_address1"
																									NullText="45 characters max" ReadOnly="true">
																									<NullTextStyle Font-Italic="True" Font-Names="Arial" ForeColor="#999999">
																									</NullTextStyle>
																									<ValidationSettings Display="None" ValidationGroup="vs_current">
																										<RequiredField ErrorText="* Line 1 of Address is Required" IsRequired="True" />
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																							<td rowspan="12" style="width: 325px" valign="top">
																								<dx:ASPxLabel ID="t_error" runat="server" Font-Size="11px" ForeColor="Red" CssClass="error" EncodeHtml="False" Height="400px" Width="100%">
																								</dx:ASPxLabel>
																								<br />
																								<dx:ASPxValidationSummary ID="vs_current" runat="server" Font-Size="11px" ValidationGroup="vs_current"  
																VerticalAlign="Top" Width="100%" TabIndex="10000" ClientInstanceName="vs_current">
																								</dx:ASPxValidationSummary>
																							</td>
																						</tr>
																						<tr>
																							<td style="color: green; width: 150px;">
																								<strong>Street Address:</strong></td>
																							<td>
																								<dx:ASPxTextBox runat="server" Width="350px" ID="t_address2" ReadOnly="true"
																								NullText="45 characters max">
																									<NullTextStyle Font-Italic="True" Font-Names="Arial" ForeColor="#999999">
																									</NullTextStyle>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="color: green; width: 150px;">
																							</td>
																							<td>
																								<dx:ASPxTextBox runat="server" Width="350px" ID="t_address3" 
																								NullText="45 characters max" ReadOnly="true">
																									<NullTextStyle Font-Italic="True" Font-Names="Arial" ForeColor="#999999">
																									</NullTextStyle>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="color: green; width: 150px;">
																							</td>
																							<td>
																								<dx:ASPxTextBox runat="server" Width="350px" ID="t_address4"
																								NullText="45 characters max" ReadOnly="true">
																									<NullTextStyle Font-Italic="True" Font-Names="Arial" ForeColor="#999999">
																									</NullTextStyle>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="color: green; width: 150px;">
																								<strong>City:</strong></td>
																							<td>
																								<dx:ASPxTextBox runat="server" Width="150px" ID="t_city" ReadOnly="true">
																									<ValidationSettings Display="None" ValidationGroup="vs_current">
																										<RequiredField ErrorText="* City is Required" IsRequired="True" />
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="color: green; width: 150px;">
																								<strong>Postal Code:</strong></td>
																							<td>
																								<dx:ASPxTextBox runat="server" Width="150px" ID="t_postal" ReadOnly="true">
																									<ValidationSettings Display="None" ValidationGroup="vs_current">
																										<RequiredField ErrorText="* Postal Code is Required" IsRequired="True" />
																									</ValidationSettings>
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="color: green; width: 150px;">
																								<strong>Prov/State:</strong></td>
																							<td>
																								<dx:ASPxComboBox ID="cb_provstate" runat="server" OnCallback="cb_provstate_Callback" clientinstancename="cb_provstate" TextField="prov_desc" ValueField="prov_abbv" Width="150px" ReadOnly="true">
																									<ValidationSettings Display="None" ValidationGroup="vs_current">
																										<RequiredField ErrorText="* Prov/State is Required" IsRequired="True" />
																									</ValidationSettings>
																								</dx:ASPxComboBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="color: green; width: 150px;">
																								<strong>Country:</strong></td>
																							<td>
																								<dx:ASPxComboBox ID="cb_country" valuefield="code" textfield="name" runat="server" Width="150px" ReadOnly="true">
																									<ClientSideEvents SelectedIndexChanged="function(s, e) {
	cb_provstate.PerformCallback(s.GetValue());
}" />
																									<ValidationSettings Display="None" ValidationGroup="vs_current">
																										<RequiredField ErrorText="* Country is Required" IsRequired="True" />
																									</ValidationSettings>
																								</dx:ASPxComboBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="color: green; width: 150px;">
																								<strong>Phone:</strong></td>
																							<td>
																								<table cellpadding="0" cellspacing="0">
																									<tbody>
																										<tr>
																											<td>
																												(</td>
																											<td>
																												<dx:ASPxTextBox ID="t_phone_area" runat="server" ReadOnly="true"
																													ClientInstanceName="t_phone_area" MaxLength="3" Width="35px" 
																													ClientEnabled="False">
																													<ClientSideEvents TextChanged="current_phone_check" />
																													<ValidationSettings Display="None" ValidationGroup="vs_current">
																														<RequiredField ErrorText="* Phone Area is Required" IsRequired="True" />
																													</ValidationSettings>
																												</dx:ASPxTextBox>
																											</td>
																											<td>
																												)</td>
																											<td>
																												<dx:ASPxTextBox ID="t_phone_prefix" runat="server" 
																													ClientInstanceName="t_phone_prefix" MaxLength="3" 
																													Width="35px" ClientEnabled="False" ReadOnly="true">
																													<ClientSideEvents TextChanged="current_phone_check" />
																													<ValidationSettings Display="None" ValidationGroup="vs_current">
																														<RequiredField ErrorText="* Phone Prefix is Required" IsRequired="True" />
																													</ValidationSettings>
																												</dx:ASPxTextBox>
																											</td>
																											<td>
																												-</td>
																											<td>
																												<dx:ASPxTextBox ID="t_phone_suffix" runat="server" 
																													ClientInstanceName="t_phone_suffix" MaxLength="10" 
																													Width="100px" EnableFocusedStyle="True" ClientEnabled="False" ReadOnly="true">
																													<ClientSideEvents TextChanged="current_phone_check" />
																													<ValidationSettings Display="None" ValidationGroup="vs_current">
																														<RequiredField ErrorText="* Phone Suffix is Required" IsRequired="True" />
																													</ValidationSettings>
																												</dx:ASPxTextBox>
																											</td>
																											<td>
																												x</td>
																											<td>
																												<dx:ASPxTextBox ID="t_phone_ext" runat="server" Width="50px" ReadOnly="true">
																												</dx:ASPxTextBox>
																											</td>
																										</tr>
																									</tbody>
																								</table>
																							</td>
																						</tr>
																						<tr>
																							<td style="color: green; width: 150px;">
																								<strong>Fax:</strong></td>
																							<td>
																								<table cellpadding="0" cellspacing="0">
																									<tbody>
																										<tr>
																											<td>
																												(</td>
																											<td>
																												<dx:ASPxTextBox ID="t_fax_area" runat="server" MaxLength="3" ReadOnly="true" 
																													Width="35px" ClientEnabled="False">
																												</dx:ASPxTextBox>
																											</td>
																											<td>
																												)</td>
																											<td>
																												<dx:ASPxTextBox ID="t_fax_prefix" runat="server" 
																													MaxLength="3" Width="35px" ClientEnabled="False" ReadOnly="true">
																												</dx:ASPxTextBox>
																											</td>
																											<td>
																												-</td>
																											<td>
																												<dx:ASPxTextBox ID="t_fax_suffix" runat="server" 
																													MaxLength="10" Width="100px" ClientEnabled="False" ReadOnly="true">
																												</dx:ASPxTextBox>
																											</td>
																										</tr>
																									</tbody>
																								</table>
																							</td>
																						</tr>
																						<tr>
																							<td style="color: green; width: 150px;">
																								<strong>Email:</strong></td>
																							<td>
																								<dx:ASPxTextBox runat="server" Width="300px" ID="t_email" ReadOnly="true">
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="color: green; width: 150px;" valign="top">
																								<strong>Web:</strong></td>
																							<td valign="top">
																								<dx:ASPxTextBox runat="server" Width="300px" ID="t_web" >
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																					</tbody>
																				</table>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage Text="Contacts">
																		<ContentCollection>
																			<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																				<dx:ASPxCallbackPanel ID="cbp_contacts" runat="server" ClientInstanceName="cbp_contacts"
																					OnCallback="cbp_contacts_Callback" Width="100%">
																					<PanelCollection>
																						<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                                            <dx:ASPxPageControl ID="ASPxPageControlSubContacts" runat="server" 
																								ActiveTabIndex="0" Width="99%" EnableCallBacks="True">
                                                                                                <TabPages>
                                                                                                    <dx:TabPage Name="NESIContacts" Text="NESI Contacts">
                                                                                                        <ContentCollection>
                                                                                                            <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                                                                                            
                                                                                                           
                                                                                                                <dx:ASPxGridView ID="gv_contacts" runat="server" AutoGenerateColumns="False" 
																													 KeyFieldName="contact_id" 
																													OnClientLayout="gv_contacts_ClientLayout" 
																													OnCustomButtonCallback="gv_contacts_CustomButtonCallback" 
																													OnCustomJSProperties="gv_contacts_CustomJSProperties" 
																													OnInitNewRow="gv_contacts_InitNewRow" OnRowDeleting="gv_contacts_RowDeleting" 
																													OnRowInserting="gv_contacts_RowInserting" 
																													OnRowUpdating="gv_contacts_RowUpdating" Width="100%">
                                                                                                                    <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px">
<Image Height="16px" Width="16px" Url="~/images/icon/icon[edit].gif"></Image>
                                                                                                                        </EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px">
<Image Height="16px" Width="16px" Url="~/images/icon/icon[add].gif"></Image>
                                                                                                                        </NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px">
<Image Height="16px" Width="16px" Url="~/images/icon/icon[delete].gif"></Image>
                                                                                                                        </DeleteButton>
</SettingsCommandButton>
																													<Columns>
																																<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
																																	ShowInCustomizationForm="True" VisibleIndex="0" Width="70px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true"   ShowNewButton="true">
																																	
																																	
																																	
																																	
																																	<CustomButtons>
																																		<dx:GridViewCommandColumnCustomButton ID="Merge">
																																			<Image Url="/images/icon/transfer.jpg" 
																																				ToolTip="Merge this contact with another one">
																																			</Image>
																																		</dx:GridViewCommandColumnCustomButton>
																																	</CustomButtons>
																																	<CellStyle Wrap="False">
																																	</CellStyle>
																																</dx:GridViewCommandColumn>
																																<dx:GridViewDataTextColumn Caption="ID" FieldName="contact_id" 
																																	ShowInCustomizationForm="True" VisibleIndex="1" ReadOnly="True" Width="65px">
																																</dx:GridViewDataTextColumn>
																																<dx:GridViewDataTextColumn Caption="Contact" FieldName="contact_name" 
																																	ShowInCustomizationForm="True" VisibleIndex="2" Width="150px">
																																</dx:GridViewDataTextColumn>
																																<dx:GridViewDataTextColumn Caption="Cell Phone" FieldName="contact_Cellphone" 
																																	ShowInCustomizationForm="True" VisibleIndex="3" Width="80px">
																																	<PropertiesTextEdit>
																																		<ValidationSettings ErrorText="Must be North American structure, use Extension if international number is required.">
																																			<RegularExpression ErrorText="Must be North American structure, use Extension if international number is required." />
																																		</ValidationSettings>
																																	</PropertiesTextEdit>
																																	<EditFormSettings Caption="Cell Phone (Format: 999-999-9999)" />
																																</dx:GridViewDataTextColumn>
																																<dx:GridViewDataTextColumn Caption="Ext." ShowInCustomizationForm="True" 
																																	VisibleIndex="4" FieldName="contact_extension" Width="50px">
																																</dx:GridViewDataTextColumn>
																																<dx:GridViewDataTextColumn Caption="Email (Login Name)" FieldName="contact_email" 
																																	ShowInCustomizationForm="True" VisibleIndex="5" Width="150px">
																																</dx:GridViewDataTextColumn>
																																<dx:GridViewDataTextColumn Caption="NESI PW" FieldName="contact_password" 
																																	ShowInCustomizationForm="True" VisibleIndex="6" Width="75px">
																																</dx:GridViewDataTextColumn>
																																<dx:GridViewDataComboBoxColumn Caption="Active" FieldName="contact_status" 
																																	ShowInCustomizationForm="True" VisibleIndex="7" Width="50px">
																																	<PropertiesComboBox>
																																		<Items>
																																			<dx:ListEditItem Text="Active" Value="Active" />
																																			<dx:ListEditItem Text="InActive" Value="InActive" />
																																		</Items>
																																	</PropertiesComboBox>
																																</dx:GridViewDataComboBoxColumn>
																																<dx:GridViewDataComboBoxColumn Caption="Login Enable" FieldName="login_enable" 
																																	ShowInCustomizationForm="True" VisibleIndex="8" Width="65px">
																																	<PropertiesComboBox>
																																		<Items>
																																			<dx:ListEditItem Text="Enabled" Value="1" />
																																			<dx:ListEditItem Text="Disabled" Value="0" />
																																		</Items>
																																	</PropertiesComboBox>
																																</dx:GridViewDataComboBoxColumn>
																																<dx:GridViewDataTextColumn Caption="Direct Line" FieldName="contact_directline" 
																																	ShowInCustomizationForm="True" VisibleIndex="9" Width="80px">
																																	<EditFormSettings Caption="Direct Line (Format: 999-999-9999)" />
																																</dx:GridViewDataTextColumn>
																															</Columns>
																													<SettingsBehavior AllowSelectByRowClick="True" EnableRowHotTrack="True" />

																															<SettingsPager PageSize="25">
																																<AllButton Text="All">
																																</AllButton>
																																<NextPageButton Text="Next &gt;">
																																</NextPageButton>
																																<PrevPageButton Text="&lt; Prev">
																																</PrevPageButton>
																															</SettingsPager>
																															
																															<Settings ShowFilterRow="True" ShowFilterRowMenu="True" 
																																ShowHeaderFilterButton="True" />

																													<Templates>
																																<EditForm>
																																	  <dx:ASPxGridViewTemplateReplacement ID="Editors" ReplacementType="EditFormEditors"
                                runat="server">
                            </dx:ASPxGridViewTemplateReplacement>
                            <div style="margin-top: 10px">
                                <div style="float: left; margin-left: 5px">
                                    <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" Text="Cancel" Width="100px"
                                        ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' />
                                </div>
                                <div style="float: right; margin-right: 5px">
                                    <dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" Text="Update" Width="100px"
                                        CssClass="input" ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' />
                                </div>
                            </div>
																																	</EditForm>
																															</Templates>
																												</dx:ASPxGridView>
                                                                                                            </dx:ContentControl>
                                                                                                        </ContentCollection>
                                                                                                    </dx:TabPage>
                                                                                                    <dx:TabPage Name="BVContacts" Text="BV Contacts">
                                                                                                        <ContentCollection>
                                                                                                            <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                                                                                            <table cellpadding="2" cellspacing="0" width="100%">
																					<tr>
																						<td align="center" rowspan="5" style="width: 150px" valign="middle">
																							<dx:ASPxListBox ID="lb_contacts" runat="server" SelectedIndex="0" Width="145px" Height="75px">
<ClientSideEvents SelectedIndexChanged="function(s, e) {
	cbp_contacts.PerformCallback(&quot;bv_con_change&quot;);
}"></ClientSideEvents>
																								<Items>
																									<dx:ListEditItem Selected="True" Text="Contact 1" Value="1" />
																									<dx:ListEditItem Text="Contact 2" Value="2" />
																									<dx:ListEditItem Text="Contact 3" Value="3" />
																								</Items>
																							</dx:ASPxListBox>
																						</td>
																						<td align="right" colspan="5" style="height: 25px" valign="middle">
																							<dx:ASPxButton ID="b_save_contact" runat="server" AutoPostBack="False" Text="Save Contact">
																								<ClientSideEvents Click="function(s, e) {
	cbp_contacts.PerformCallback(&quot;save&quot;);
}" />

																								<Image Url="~/images/icon/icon[save].gif">
																								</Image>
																							</dx:ASPxButton>
																						</td>
																					</tr>
																					<tr>
																						<td style="width: 60px; height: 25px; color: green;" valign="middle">
																							<strong>Name:</strong></td>
																						<td colspan="4" valign="middle">
																							<dx:ASPxTextBox ID="t_contact_name" runat="server" Width="170px">
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
																					<tr>
																						<td style="width: 60px; height: 25px; color: green;" valign="middle">
																							<strong>Phone:</strong></td>
																						<td colspan="4" valign="middle">
																							<table cellpadding="2" cellspacing="0">
																								<tr>
																									<td valign="middle">
																										(</td>
																									<td style="width: 35px" valign="middle">
																										<dx:ASPxTextBox ID="t_contact_phone_area" runat="server" MaxLength="3" Width="35px">
																										</dx:ASPxTextBox>
																									</td>
																									<td style="width: 15px" valign="middle">
																										)</td>
																									<td style="width: 35px" valign="middle">
																										<dx:ASPxTextBox ID="t_contact_phone_prefix" runat="server" Width="35px" MaxLength="3">
																										</dx:ASPxTextBox>
																									</td>
																									<td valign="middle">
																										-</td>
																									<td valign="middle">
																										<dx:ASPxTextBox ID="t_contact_phone_suffix" runat="server" MaxLength="4" Width="35px">
																										</dx:ASPxTextBox>
																									</td>
																									<td valign="middle">
																										x</td>
																									<td valign="middle">
																										<dx:ASPxTextBox ID="t_contact_phone_ext" runat="server" Width="50px" MaxLength="10">
																										</dx:ASPxTextBox>
																									</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																					<tr>
																						<td style="width: 60px; height: 25px; color: green;" valign="middle">
																							<strong>Cell:</strong></td>
																						<td colspan="4" valign="middle">
																							<table cellpadding="2" cellspacing="0">
																								<tr>
																									<td valign="middle">
																										(</td>
																									<td style="width: 35px" valign="middle">
																										<dx:ASPxTextBox ID="t_contact_fax_area" runat="server" MaxLength="3" Width="35px">
																										</dx:ASPxTextBox>
																									</td>
																									<td style="width: 15px" valign="middle">
																										)</td>
																									<td style="width: 35px" valign="middle">
																										<dx:ASPxTextBox ID="t_contact_fax_prefix" runat="server" Width="35px" MaxLength="3">
																										</dx:ASPxTextBox>
																									</td>
																									<td valign="middle">
																										-</td>
																									<td valign="middle">
																										<dx:ASPxTextBox ID="t_contact_fax_suffix" runat="server" Width="35px" MaxLength="4">
																										</dx:ASPxTextBox>
																									</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																					<tr>
																						<td style="width: 60px; height: 25px; color: green;" valign="middle">
																							<strong>Email:</strong></td>
																						<td colspan="4" valign="middle">
																							<dx:ASPxTextBox ID="t_contact_email" runat="server" Width="170px">
																							</dx:ASPxTextBox>
																						</td>
																					</tr>
																				</table>
                                                                                                            </dx:ContentControl>
                                                                                                        </ContentCollection>
                                                                                                    </dx:TabPage>
                                                                                                </TabPages>
                                                                                            </dx:ASPxPageControl>
																						</dx:PanelContent>
																					</PanelCollection>
																				</dx:ASPxCallbackPanel>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage Text="Accounting">
																		
																		<ContentCollection>
																			<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																				<table width="500" cellpadding="2" cellspacing="0">
																					<tbody>
																						<tr>
																							<td style="WIDTH: 200px; color: green;">
																								<strong>Terms:</strong></td>
																							<td>
																								<dx:ASPxComboBox runat="server" ID="cb_terms" TextField="text" ValueField="value" ValueType="System.Int32" ReadOnly="true" >
																									<ValidationSettings Display="None" ValidationGroup="vs_current">
																										<RequiredField ErrorText="* Terms is a Required Field" IsRequired="True" />
																									</ValidationSettings>
																								</dx:ASPxComboBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="width: 200px; color: green">
																								<strong>PO Exempt (Not PO's needed):</strong></td>
																							<td>
																								<asp:CheckBox runat="server" ID="ck_poexempt"></asp:CheckBox>
																							</td>
																						</tr>
                                                                                        
																						<tr>
                                                                                            <td style="width: 200px; color: green">&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                        </tr>
																						<tr>
																							<td style="width: 200px; color: green">
																								<strong>Print CPRS:</strong></td>
																							<td>
																								<asp:CheckBox runat="server" ID="ck_cprs"></asp:CheckBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="width: 200px; color: green">
																								<strong>Identification No. Type:</strong></td>
																							<td>
																								<dx:ASPxComboBox ID="cb_id_type" runat="server">
																									<Items>
																										<dx:ListEditItem Text="Business" Value="B" />
																										<dx:ListEditItem Text="S.I.N (Social)" Value="S" />
																									</Items>
																								</dx:ASPxComboBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="width: 200px; color: green">
																								<strong>Identification Number:</strong></td>
																							<td>
																								<dx:ASPxTextBox ID="t_id_number" runat="server" Width="170px">
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr style="display:none;">
																							<td style="width: 200px; color: green;">
																								<strong>Credit Type:</strong></td>
																							<td>
																								<dx:ASPxComboBox runat="server" ID="cb_credit_type" SelectedIndex="0" ValueType="System.Int32">
																									<Items>
																										<dx:ListEditItem Selected="True" Text="Unlimited" Value="1" />
																										<dx:ListEditItem Text="No Credit" Value="0" />
																										<dx:ListEditItem Text="Limit" Value="2" />
																									</Items>
																								</dx:ASPxComboBox>
																							</td>
																						</tr>
																						<tr  style="display:none;">
																							<td style="width: 200px; color: green">
																								<strong>Credit Limit:</strong></td>
																							<td>
																								<dx:ASPxTextBox runat="server" Width="170px" ID="t_credit_limit">
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="width: 200px; color: green">
																								<strong>Account Number:</strong></td>
																							<td>
																								<dx:ASPxTextBox runat="server" Width="170px" ID="t_account_number">
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																						<tr>
																							<td style="width: 200px; color: green">
																								<strong>Buyer Name:</strong></td>
																							<td>
																								<dx:ASPxTextBox runat="server" Width="170px" ID="t_buyer_name">
																								</dx:ASPxTextBox>
																							</td>
																						</tr>
																					    <tr  style="display:none;">
                                                                                           <td style="width: 200px; color: green">
																								<strong>Tax 1:</strong></td>
																							<td>
																								<dx:ASPxComboBox runat="server" ID="ddl_tax1" SelectedIndex="0" ValueType="System.Int32" DataSourceID="sql_tax" TextField="tax_name" ValueField="tax_id">
																								</dx:ASPxComboBox>
																							</td>
                                                                                        </tr>
                                                                                        <tr  style="display:none;">
                                                                                           <td style="width: 200px; color: green">
																								<strong>Tax 2:</strong></td>
																							<td>
																								<dx:ASPxComboBox runat="server" ID="ddl_tax2" SelectedIndex="0" ValueType="System.Int32" DataSourceID="sql_tax" TextField="tax_name" ValueField="tax_id">
																								</dx:ASPxComboBox>
																							</td>
                                                                                        </tr>
                                                                                        <tr  style="display:none;">
                                                                                           <td style="width: 200px; color: green">
																								<strong>Tax 3:</strong></td>
																							<td>
																								<dx:ASPxComboBox runat="server" ID="ddl_tax3" SelectedIndex="0" ValueType="System.Int32" DataSourceID="sql_tax" TextField="tax_name" ValueField="tax_id">
																								</dx:ASPxComboBox>
																							</td>
                                                                                        </tr>
                                                                                        <tr  style="display:none;">
                                                                                           <td style="width: 200px; color: green">
																								<strong>Tax 4:</strong></td>
																							<td>
																								<dx:ASPxComboBox runat="server" ID="ddl_tax4" SelectedIndex="0" ValueType="System.Int32" DataSourceID="sql_tax" TextField="tax_name" ValueField="tax_id">
																								</dx:ASPxComboBox>
																							    <asp:SqlDataSource ID="sql_tax" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select 0 tax_id, 'none' tax_name union Select tax_id, tax_name from tax where is_active = 1"></asp:SqlDataSource>
																							</td>
                                                                                        </tr>
																					</tbody>
																				</table>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage Text="Notes">
																		
																		<ContentCollection>
																			<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																						<table>
																							<tbody>
																								<tr>
																									<td height="225" valign="top">
																										<asp:Label ID="memo_error" runat="server" ForeColor="Red"></asp:Label>
																										<br />
																										<dx:ASPxTextBox ID="t_notes" runat="server" Height="200px" MaxLength="30" Width="350px">
																										</dx:ASPxTextBox>
																									</td>
																									<td rowspan="3" valign="top">
																										<div ID="t_past_memo" runat="server" style="overflow-y:scroll; min-height: 1000px;">
																										</div>
																									</td>
																								</tr>
																								<tr>
																									<td height="25">
																										<dx:ASPxLabel ID="t_memolabel" runat="server" Text="NotePad">
																										</dx:ASPxLabel>
																									</td>
																								</tr>
																								<tr>
																									<td valign="top">
																										<dx:ASPxMemo ID="t_memo" runat="server" ClientInstanceName="t_memo" Rows="8" Width="350px">
																										</dx:ASPxMemo>
																										<dx:ASPxButton ID="btn_memo" runat="server" AutoPostBack="False" Text="Save Memo">
																											<ClientSideEvents Click="NE_vendor.save_memo" />
																											<Image Url="~/images/icon/icon[add].gif">
																											</Image>
																										</dx:ASPxButton>
																										<dx:ASPxCallback ID="cb_save_memo" runat="server" ClientInstanceName="cb_save_memo" OnCallback="cb_save_memo_Callback">
																											<ClientSideEvents BeginCallback="function(s, e) {
	please_wait('start');
}" CallbackComplete="function(s, e) {
	please_wait('stop');}" CallbackError="function(s, e) {
		please_wait('stop');
}" />
																										</dx:ASPxCallback>
																									</td>
																								</tr>
																							</tbody>
																						</table>
																					<asp:HiddenField ID="hidvendno" runat="server" />
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage Text="Purchases">
																		
																		<ContentCollection>
																			<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																				<asp:HiddenField ID="hid_vendor_id" runat="server" />
																				<dx:ASPxGridView ID="gv_pos" runat="server" 
																					AutoGenerateColumns="False" Width="100%" ClientInstanceName="gv_pos">
																					<Columns>
																						<dx:GridViewDataTextColumn Caption="PO#" VisibleIndex="1" Width="100px" ShowInCustomizationForm="True">
																							<DataItemTemplate>
																								<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" Font-Bold="True" NavigateUrl="javascript:void(0);"
																									 OnDataBound="hl_wo_Init" Text='<%# Eval("po_number") %>'>
																								</dx:ASPxHyperLink>
																							</DataItemTemplate>
																							<HeaderStyle HorizontalAlign="Center" />
																							<CellStyle Font-Bold="True" HorizontalAlign="Center">
																							</CellStyle>
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="Cut Date" FieldName="cut_dt" VisibleIndex="3"
																							Width="75px" ShowInCustomizationForm="True">
																							<HeaderStyle HorizontalAlign="Center" />
																							<CellStyle HorizontalAlign="Center">
																							</CellStyle>
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="Close Date" FieldName="close_dt" VisibleIndex="4"
																							Width="75px" ShowInCustomizationForm="True">
																							<HeaderStyle HorizontalAlign="Center" />
																							<CellStyle HorizontalAlign="Center">
																							</CellStyle>
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="Description" FieldName="po_description" VisibleIndex="5" ShowInCustomizationForm="True">
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="7" Width="150px" ShowInCustomizationForm="True">
																							<HeaderStyle HorizontalAlign="Center" />
																							<CellStyle HorizontalAlign="Center">
																							</CellStyle>
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="Busniess Unit" FieldName="branch" ShowInCustomizationForm="True" VisibleIndex="2">
																						</dx:GridViewDataTextColumn>
																					</Columns>
																					<Settings ShowFilterRow="True" ShowFooter="True" ShowFilterBar="Visible" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
																					<Styles>
																						<Header Font-Bold="True">
																						</Header>
																					</Styles>
																				</dx:ASPxGridView>
																				&nbsp;</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage Name="phonenumbers" NewLine="True" Text="Phone Numbers">
																		<ContentCollection>
																			<dx:ContentControl runat="server">
																				<dx:ASPxGridView ID="gv_phones" runat="server" AutoGenerateColumns="False" 
																					DataSourceID="ds_phones" KeyFieldName="id" Width="100%" ClientInstanceName="gv_phones" 
																				oncustombuttoncallback="gv_CustomButtonCallback" 
																				 oncustomcallback="gv_CustomCallback"
																					 OnRowInserting="gv_phones_RowInserting" 
																					OnRowUpdating="gv_phones_RowUpdating" Settings-ShowStatusBar="Visible" Settings-ShowFooter="True" SettingsEditing-Mode="Batch">
																					<ClientSideEvents  CustomButtonClick="function(s, e) {



if (e.buttonID=='delete')
{
if (confirm('Are you sure you want to delete this phone number?'))
	{
	gv_phones.PerformCallback('DELETE|'+e.visibleIndex);
	}
}

}" 

 BatchEditStartEditing="function(s, e) {
 
edit_mode2.SetVisible(true);
hl_save2.SetVisible(true);
hl_cancel2.SetVisible(true);
	

}" 


/>

																					<Columns>
																						<dx:GridViewCommandColumn  ShowEditButton="True" 
																							ShowInCustomizationForm="True" ShowNewButtonInHeader="True" VisibleIndex="0">
																							<CustomButtons>
													<dx:GridViewCommandColumnCustomButton ID="delete" Text="Delete">
													</dx:GridViewCommandColumnCustomButton>
												</CustomButtons>
																						</dx:GridViewCommandColumn>
																						<dx:GridViewDataTextColumn Caption="Number" FieldName="number" 
																							ShowInCustomizationForm="True" VisibleIndex="2">
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" 
																							ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
																							<EditFormSettings Visible="False" />
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataCheckColumn Caption="Default" FieldName="_default" 
																							ShowInCustomizationForm="True" VisibleIndex="4">
																						</dx:GridViewDataCheckColumn>
																						<dx:GridViewDataCheckColumn Caption="Active" FieldName="active" 
																							ShowInCustomizationForm="True" VisibleIndex="5">
																						</dx:GridViewDataCheckColumn>
																						<dx:GridViewDataComboBoxColumn Caption="Type" FieldName="type" 
																							ShowInCustomizationForm="True" VisibleIndex="1">
																						<PropertiesComboBox>
													
													<Items>
														
														<dx:ListEditItem Text="Fax" Value="Fax" />
														
														<dx:ListEditItem Text="LandLine" Value="LandLine" Selected="True" />
														
													</Items>
													
												</PropertiesComboBox>
																						</dx:GridViewDataComboBoxColumn>
																					</Columns>
																					<SettingsPager Mode="ShowAllRecords" Visible="False">
										</SettingsPager>
																					<SettingsBehavior ConfirmDelete="True" />
																					<SettingsEditing Mode="Batch">
																					</SettingsEditing>

<Settings ShowFooter="True" ShowStatusBar="Visible"></Settings>

																					
										<Styles>
											<SelectedRow BackColor="#FF9900">
											</SelectedRow>
											<CommandColumn HorizontalAlign="Left">
											</CommandColumn>
											<StatusBar HorizontalAlign="Right" Wrap="False">
											</StatusBar>
											<BatchEditModifiedCell BackColor="#FFFFCC">
											</BatchEditModifiedCell>
										</Styles>
											<Templates>
											<StatusBar>
												<table style="width:100%;">
													<tr>
														<td style="text-decoration: blink; font-family: Arial, Helvetica, sans-serif; font-size: 14px" 
															width="100%" align="left">
															<dx:ASPxLabel ID="ASPxLabel2" runat="server" ClientInstanceName="edit_mode2" 
																ClientVisible="False" Font-Bold="True" ForeColor="Red" Text="Edit Mode">
															</dx:ASPxLabel>
														</td>
														<td>
															<dx:ASPxHyperLink ID="hl_save" runat="server" Cursor="pointer" Text="Save" 
																Width="50px" ClientInstanceName="hl_save2">
																<ClientSideEvents Click="function(s, e){edit_mode2.SetVisible(false);hl_save2.SetVisible(false);hl_cancel2.SetVisible(false); gv_phones.UpdateEdit(); }" />
															</dx:ASPxHyperLink>
														</td>
														<td>
															<dx:ASPxHyperLink ID="hl_cancel" runat="server" Cursor="pointer" Text="Cancel" ClientInstanceName="hl_cancel2">
																<ClientSideEvents Click="function(s, e){  edit_mode2.SetVisible(false);hl_save2.SetVisible(false);hl_cancel2.SetVisible(false); gv_phones.CancelEdit();  }" />
															</dx:ASPxHyperLink>
														</td>
													</tr>
												</table>
											</StatusBar>
										</Templates>

																				</dx:ASPxGridView>
																				<asp:SqlDataSource ID="ds_phones" runat="server" 
																					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
	phone_numbers_comm_type type, 
	phone_numbers_number number, 
	phone_numbers_id id,
 phone_numbers_default _default,
phone_numbers_active active
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
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage Text="Emails">
																		
																		<ContentCollection>
																			<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																				<dx:ASPxGridView ID="gv_emails" runat="server" AutoGenerateColumns="False"
																					Width="100%" ClientInstanceName="gv_emails" >
																					<Columns>
																						<dx:GridViewDataTextColumn Caption="Date" FieldName="date" FixedStyle="Left" VisibleIndex="0" ShowInCustomizationForm="True">
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="From Address" FieldName="from_address" VisibleIndex="1" ShowInCustomizationForm="True">
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="To Address" FieldName="to_address" VisibleIndex="2" ShowInCustomizationForm="True">
																						</dx:GridViewDataTextColumn>
																						
																					</Columns>
																				</dx:ASPxGridView>
																				&nbsp;</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage Text="Phone Calls">
																		
																		<ContentCollection>
																			<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																				<dx:ASPxGridView ID="gv_phonecalls" runat="server"
																					AutoGenerateColumns="False" Width="100%" ClientInstanceName="gv_phonelog" OnInit="gv_phonelog_Init">
																					<Columns>
																						<dx:GridViewDataTextColumn Caption="Date" FieldName="phone_log_date" VisibleIndex="0"
																							Width="125px" ShowInCustomizationForm="True">
																							<DataItemTemplate>
																								<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("phone_log_date", "{0:d}") %>'>
																								</dx:ASPxLabel>
																							</DataItemTemplate>
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="From #" FieldName="phone_log_from_number" VisibleIndex="1" ShowInCustomizationForm="True">
																							<CellStyle HorizontalAlign="Center">
																							</CellStyle>
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="From Name" FieldName="phone_log_from_name" VisibleIndex="2" ShowInCustomizationForm="True">
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="To #" FieldName="phone_log_to_number" VisibleIndex="3" ShowInCustomizationForm="True">
																							<CellStyle HorizontalAlign="Center">
																							</CellStyle>
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="To Name" FieldName="phone_log_to_name" VisibleIndex="4" ShowInCustomizationForm="True">
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="Duration" FieldName="phone_log_duration" VisibleIndex="5"
																							Width="50px" ShowInCustomizationForm="True">
																						</dx:GridViewDataTextColumn>
																						<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" 
																							ShowInCustomizationForm="True" VisibleIndex="6">
																						</dx:GridViewDataTextColumn>
																					</Columns>
																					<Styles>
																						<Header Font-Bold="False" HorizontalAlign="Center">
																						</Header>
																						<Cell HorizontalAlign="Center">
																						</Cell>
																					</Styles>
																				</dx:ASPxGridView>
																				&nbsp;</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage Text="Files">
																		
																		<ContentCollection>
																			<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																			<iframe id="files_frame" runat="server" height="900" scrolling="no" width="1000" style="border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none" frameborder="0"></iframe>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage Text="Market">
																		<ContentCollection>
																			<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																				<table style="width:100%;">
																					<tr>
																						<td width="400px" font-weight: 700; font-family: 'Segoe UI'; padding-left: 
																							5px;" bgcolor="White">
																							Line Card (Manufacturers they sell):</td>
																						<td>
																							&nbsp;</td>
																						<td width="400px" font-weight: 700;  font-family: 'Segoe UI'; padding-left: 
																							5px;" bgcolor="White">
																							Competitors from this Vendor (linked to your Business Unit):</td>
																						<td>
																							</td>
																						<td>
																							&nbsp;</td>
																					</tr>
																					<tr>
																						<td width="400px" rowspan="2" valign="top" font-family: 'Segoe UI'>
																							<div id="div_linecard" runat="server" bgcolor="White" 
																								></div></td>
																						<td rowspan="2" width="20px">
																							&nbsp;</td>
																						<td width="400px" rowspan="2" valign="top" font-family: 'Segoe UI'>
																							<div id="div_competitors" runat="server" bgcolor="White"
																								></div></td>
																						<td>
																							</td>
																						<td>
																							
																							&nbsp;</td>
																					</tr>
																					<tr>
																						<td>
																							&nbsp;</td>
																						<td>
																							&nbsp;</td>
																					</tr>
																				</table>
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																	<dx:TabPage Text="Specialties">
																		<ContentCollection>
																			<dx:ContentControl runat="server">
																				<uc1:partner_skills ID="partner_skills1" runat="server" />
																				<br />
																			</dx:ContentControl>
																		</ContentCollection>
																	</dx:TabPage>
																</TabPages>

<ClientSideEvents TabClick="function(s, e) {
	
    var tab = s.GetActiveTabIndex();
    if (tab == 5){
        cbp_main.PerformCallback('phone_refresh');
    }else{
        gv_phonelog.PerformCallback();
	    gv_emails.PerformCallback();
	    gv_pos.PerformCallback();
    }
	console.log(tab);
}"></ClientSideEvents>

																
															</dx:ASPxPageControl>
														</td>
													</tr>
												</tbody>
											</table>
										</dx:ContentControl>
									</ContentCollection>
								
								</dx:TabPage>
							</TabPages>
							<SettingsLoadingPanel Enabled="False" />
							<ContentStyle BackColor="#FDFDFD">
                            </ContentStyle>
						</dx:ASPxPageControl>
						<dx:ASPxHiddenField ID="persist" runat="server" ClientInstanceName="persist"></dx:ASPxHiddenField>
						<dx:ASPxHiddenField ID="hidParent" runat="server" ClientInstanceName="hidParent"></dx:ASPxHiddenField>
						<dx:ASPxPopupControl ID="pop_new" runat="server" ClientInstanceName="pop_new" CloseAction="CloseButton"
							AnimationType="None" HeaderText="New Vendor" Modal="True" PopupHorizontalAlign="WindowCenter"
							PopupVerticalAlign="TopSides" Width="650px" OnWindowCallback="pop_new_WindowCallback">
							<ModalBackgroundStyle Opacity="0">
							</ModalBackgroundStyle>
							<ContentCollection>
								<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
									<dx:ASPxCallbackPanel ID="cbp_new" runat="server" ClientInstanceName="cbp_new" OnCallback="cbp_new_Callback">
<SettingsLoadingPanel Enabled="False"></SettingsLoadingPanel>
										<PanelCollection>
											<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
												<table cellpadding="5" cellspacing="0" width="100%">
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
															Name*:</td>
														<td style="width: 250px">
															<dx:ASPxTextBox ID="t_new_name" runat="server" Width="100%" ClientInstanceName="t_new_name" TabIndex="1">
																<ClientSideEvents LostFocus="new_name_check" />
																<ValidationSettings Display="None" ValidationGroup="new">
																	<RequiredField ErrorText="* Vendor Name is Required" IsRequired="True" />
																</ValidationSettings>
															</dx:ASPxTextBox>
														</td>
														<td rowspan="14" valign="top">
															&nbsp;<dx:ASPxLabel ID="t_new_error" runat="server" Font-Size="11px" ForeColor="Red" EncodeHtml="False" Height="400px" Width="100%" CssClass="error">
															</dx:ASPxLabel>
															<dx:ASPxValidationSummary ID="vs_new" runat="server" Font-Size="11px" ValidationGroup="new"
																VerticalAlign="Top" Width="100%" TabIndex="10000" ClientInstanceName="vs_new">
															</dx:ASPxValidationSummary>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
														</td>
														<td style="width: 250px">
															<dx:ASPxTextBox ID="t_new_address_1" runat="server" AutoResizeWithContainer="True"
																NullText="Line 1" Width="100%" TabIndex="2" ClientInstanceName="t_new_address_1">
																<ValidationSettings Display="None" ValidationGroup="new">
																	<RequiredField ErrorText="* Line 1 of Address is Required" IsRequired="True" />
																</ValidationSettings>
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
															Address:</td>
														<td style="width: 250px">
															<dx:ASPxTextBox ID="t_new_address_2" runat="server" AutoResizeWithContainer="True"
																NullText="Line 2" Width="100%" TabIndex="3">
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
														</td>
														<td style="width: 250px">
															<dx:ASPxTextBox ID="t_new_address_3" runat="server" AutoResizeWithContainer="True"
																NullText="Line 3" Width="100%" TabIndex="4">
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
														</td>
														<td style="width: 250px">
															<dx:ASPxTextBox ID="t_new_address_4" runat="server" AutoResizeWithContainer="True"
																NullText="Line 4" Width="100%" TabIndex="5">
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
															City*:</td>
														<td style="width: 250px">
															<dx:ASPxTextBox ID="t_new_city" runat="server" Width="100%" TabIndex="6">
																<ValidationSettings Display="None" ValidationGroup="new">
																	<RequiredField ErrorText="* City is Required" IsRequired="True" />
																</ValidationSettings>
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
															Prov/State*:</td>
														<td style="width: 250px">
															<dx:ASPxComboBox ID="cb_new_provstate" runat="server" IncrementalFilteringMode="StartsWith" TabIndex="7" TextField="prov_desc" ValueField="prov_abbv">
																<ValidationSettings Display="None" ValidationGroup="new">
																	<RequiredField ErrorText="* Prov/State is Required" IsRequired="True" />
																</ValidationSettings>
															</dx:ASPxComboBox>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
															Postal Code*:</td>
														<td style="width: 250px">
															<dx:ASPxTextBox ID="t_new_postal" runat="server" TabIndex="8" Width="170px">
																<ValidationSettings Display="None" ValidationGroup="new">
																	<RequiredField ErrorText="* Postal Code is Required" IsRequired="True" />
																</ValidationSettings>
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
															Country*:</td>
														<td style="width: 250px">
															<dx:ASPxComboBox ID="cb_new_country" runat="server" IncrementalFilteringMode="StartsWith"  valuefield="code" textfield="name" TabIndex="9">
																<ValidationSettings Display="None" ValidationGroup="new">
																	<RequiredField ErrorText="* Country is Required" IsRequired="True" />
																</ValidationSettings>
															</dx:ASPxComboBox>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
															Phone*:</td>
														<td style="width: 250px">
															<table cellpadding="0" cellspacing="0">
																<tbody>
																	<tr>
																		<td>
																			(</td>
																		<td>
																			<dx:ASPxTextBox ID="t_new_phone_area" runat="server" ClientInstanceName="t_new_phone_area" MaxLength="3" TabIndex="10" Width="35px" >
																				<ClientSideEvents TextChanged="new_phone_check" />
																				<ValidationSettings Display="None" ValidationGroup="new">
																					<RegularExpression ErrorText="*Not a valid phone area code" ValidationExpression="\d{3}" />
																					<RequiredField ErrorText="* Phone Area Code is Required" IsRequired="True" />
																				</ValidationSettings>
																			</dx:ASPxTextBox>
																		</td>
																		<td>
																			)</td>
																		<td>
																			<dx:ASPxTextBox ID="t_new_phone_prefix" runat="server" ClientInstanceName="t_new_phone_prefix" MaxLength="3" TabIndex="11" Width="35px" >
																				<ClientSideEvents TextChanged="new_phone_check" />
																				<ValidationSettings Display="None" ValidationGroup="new">
																					<RegularExpression ErrorText="*Not a valid phone prefix" ValidationExpression="\d{3}" />
																					<RequiredField ErrorText="* Phone Prefix is Required" IsRequired="True" />
																				</ValidationSettings>
																			</dx:ASPxTextBox>
																		</td>
																		<td>
																			-</td>
																		<td>
																			<dx:ASPxTextBox ID="t_new_phone_suffix" runat="server" ClientInstanceName="t_new_phone_suffix" MaxLength="4" TabIndex="12" Width="100px" >
																				<ClientSideEvents TextChanged="new_phone_check" />
																				<ValidationSettings Display="None" ValidationGroup="new">
																					<RegularExpression ErrorText="*Not a valid phone suffix" ValidationExpression="\d{4}" />
																					<RequiredField ErrorText="* Phone Suffix is Required" IsRequired="True" />
																				</ValidationSettings>
																			</dx:ASPxTextBox>
																		</td>
																		<td>
																			x</td>
																		<td>
																			<dx:ASPxTextBox ID="t_new_phone_ext" runat="server" TabIndex="13" Width="50px">
																				<ValidationSettings ValidationGroup="new">
																					<RegularExpression ErrorText="*Not a valid phone extension" ValidationExpression="\d+" />
																				</ValidationSettings>
																			</dx:ASPxTextBox>
																		</td>
																	</tr>
																</tbody>
															</table>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
															Fax:</td>
														<td style="width: 250px">
															<table cellpadding="0" cellspacing="0">
																<tbody>
																	<tr>
																		<td>
																			(</td>
																		<td>
																			<dx:ASPxTextBox ID="t_new_fax_area" runat="server" MaxLength="3" TabIndex="14" Width="35px">
																				<ValidationSettings Display="None" ValidationGroup="new">
																					<RegularExpression ErrorText="*Not a valid fax area code" ValidationExpression="\d{3}" />
																				</ValidationSettings>
																			</dx:ASPxTextBox>
																		</td>
																		<td>
																			)</td>
																		<td>
																			<dx:ASPxTextBox ID="t_new_fax_prefix" runat="server" MaxLength="3" TabIndex="15" Width="35px">
																				<ValidationSettings Display="None" ValidationGroup="new">
																					<RegularExpression ErrorText="*Not a valid fax prefix" ValidationExpression="\d{3}" />
																				</ValidationSettings>
																			</dx:ASPxTextBox>
																		</td>
																		<td>
																			-</td>
																		<td>
																			<dx:ASPxTextBox ID="t_new_fax_suffix" runat="server" MaxLength="4" TabIndex="16" Width="100px">
																				<ValidationSettings Display="None" ValidationGroup="new">
																					<RegularExpression ErrorText="*Not a valid fax suffix" ValidationExpression="\d{4}" />
																				</ValidationSettings>
																			</dx:ASPxTextBox>
																		</td>
																	</tr>
																</tbody>
															</table>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
															Email:</td>
														<td style="width: 250px">
															<dx:ASPxTextBox ID="t_new_email" runat="server" TabIndex="17" Width="170px">
																<ValidationSettings ValidationGroup="new">
																	<RegularExpression ErrorText="*Not a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
																</ValidationSettings>
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
															Website:</td>
														<td style="width: 250px">
															<dx:ASPxTextBox ID="t_new_website" runat="server" TabIndex="17" Width="170px">
																<ValidationSettings>
																	<RegularExpression ErrorText="" />
																</ValidationSettings>
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="font-weight: bold; width: 85px; color: green;">
															Terms*:</td>
														<td style="width: 250px">
															<dx:ASPxComboBox ID="cb_new_terms" runat="server" TabIndex="18" TextField="text" ValueField="value">
																<ValidationSettings ValidationGroup="new">
																	<RequiredField ErrorText="* Please Select a Term" IsRequired="True" />
																</ValidationSettings>
															</dx:ASPxComboBox>
														</td>
													</tr>
													<tr>
														<td colspan="3" style="font-weight: bold" align="right">
															<dx:ASPxButton ID="b_new_save" runat="server" AutoPostBack="False" Text="Start Vendor" ValidationGroup="new" TabIndex="19">
																<ClientSideEvents Click="function(s, e) {
	if(ASPxClientEdit.ValidateGroup(&quot;new&quot;))
		{
		s.SetEnabled(false);
		cbp_new.PerformCallback('start_vendor');
		}
}" />
															</dx:ASPxButton>
														</td>
													</tr>
												</table>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxCallbackPanel>
								</dx:PopupControlContentControl>
							</ContentCollection>
							<ClientSideEvents PopUp="function(s, e) {
	t_new_name.Focus();
}" BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" EndCallback="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
							
						</dx:ASPxPopupControl>
					</dx:PanelContent>
				</PanelCollection>
<SettingsLoadingPanel Text="" Enabled="False" ShowImage="False"></SettingsLoadingPanel>

				<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" EndCallback="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />

			</dx:ASPxCallbackPanel>
			&nbsp;&nbsp;</div>
	</form>
</body>
</html>
