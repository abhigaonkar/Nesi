<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_customer_modules_location" EnableViewState="true" EnableTheming="True"  Codebehind="location.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<%@ Register src="phone.ascx" tagname="phone" tagprefix="uc" %>
<%@ Register src="contact.ascx" tagname="contact" tagprefix="uc" %>
<%@ Register src="sales.ascx" tagname="sales" tagprefix="uc" %>
<%@ Register src="notes.ascx" tagname="notes" tagprefix="uc" %>
<%@ Register src="files.ascx" tagname="files" tagprefix="uc" %>
<%@ Register src="assets.ascx" tagname="assets" tagprefix="uc" %>
<div class='location_detail'>
	
	All values listed below are <u>location specific</u>, unless otherwise denoted.
	<br />
	<dx:ASPxLabel ID="lb_error" runat="server" EncodeHtml="False" Font-Bold="True" ForeColor="Red">
						</dx:ASPxLabel>
	<dx:ASPxPageControl ID="pc_location" runat="server" ActiveTabIndex="0" 
		Width="100%" Theme="NETheme01">
		<TabPages>
			<dx:TabPage Name="address" NewLine="True" Text="Address">
				<ContentCollection>
					<dx:ContentControl runat="server">
								<table cellpadding="2" cellspacing="0" class="location">
									<tr>
										<td class='c'>ID:</td>
										<td class='v'>
											<asp:Label ID="a_lblid" runat="server"></asp:Label>
										</td>
										<td class='error_report' rowspan='19' align="left" valign="top" runat="server" 
											id="error_report"></td>
									</tr>
									<tr>
										<td class="c">
											Customer Or Worksite Address?</td>
										<td class="v">
											<asp:DropDownList ID="a_table" runat="server">
												<asp:ListItem Selected="True" Value="Customer">Customer</asp:ListItem>
												<asp:ListItem>Worksite</asp:ListItem>
											</asp:DropDownList>
										</td>
									</tr>
									<tr>
										<td class="c">
											Description (Name):</td>
										<td class="v">
											<asp:TextBox ID="a_desc" runat="server" MaxLength="60"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td rowspan="4" valign="top" class='c'>Address*:</td>
										<td class='v'><asp:TextBox ID="a_addr1" runat="server" MaxLength="45"></asp:TextBox></td>
									</tr>
									<tr>
										<td class='v'><asp:TextBox ID="a_addr2" runat="server" MaxLength="45"></asp:TextBox></td>
									</tr>
									<tr>
										<td class='v'><asp:TextBox ID="a_addr3" runat="server" MaxLength="45"></asp:TextBox></td>
									</tr>
									<tr>
										<td class='v'><asp:TextBox ID="a_addr4" runat="server" MaxLength="45"></asp:TextBox></td>
									</tr>
									<tr>
										<td class='c'>City*:</td>
										<td class='v'><asp:TextBox ID="a_city" runat="server" MaxLength="45"></asp:TextBox></td>
									</tr>
									<tr>
										<td class='c'>Prov/State*:</td>
										<td class='v'><asp:DropDownList ID="a_provstate" runat="server" DataSourceID="ds_provstate" DataTextField="name" DataValueField="id"></asp:DropDownList><asp:SqlDataSource ID="ds_provstate" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" runat="server" SelectCommand="SELECT prov_abbv id, TRIM(prov_desc) name FROM prov ORDER BY prov_abbv"></asp:SqlDataSource></td>
									</tr>
									<tr>
										<td class='c'>Postal Code*:</td>
										<td class='v'><asp:TextBox ID="a_postal" runat="server"></asp:TextBox></td>
									</tr>
									<tr>
										<td class='c'>Country*:</td>
										<td class='v'><asp:DropDownList ID="a_country" runat="server" DataSourceID="ds_country" DataTextField="name" DataValueField="id"></asp:DropDownList><asp:SqlDataSource ID="ds_country" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" runat="server" SelectCommand="SELECT country_code id, TRIM(country_name) name FROM country ORDER BY name" /></td>
									</tr>
									<tr>
										<td class='c'>Phone*:</td>
										<td class='v' valign="middle">
											<div style="vertical-align: middle; white-space: nowrap;">(<asp:TextBox ID="p_area" 
													runat="server" Width="55px"></asp:TextBox>
												)<asp:TextBox ID="p_prefix" runat="server" Width="55px"></asp:TextBox>
												-<asp:TextBox ID="p_suffix" runat="server" Width="55px"></asp:TextBox>
												Ext.<asp:TextBox ID="p_ext" runat="server" Width="55px"></asp:TextBox>
											</div>
										</td>
									</tr>
									<tr>
										<td class='c'>Fax:</td>
										<td class='v' valign="middle">
											<div>(<asp:TextBox ID="f_area" runat="server" Width="55px"></asp:TextBox>
												)<asp:TextBox ID="f_prefix" runat="server" Width="55px"></asp:TextBox>
												-<asp:TextBox ID="f_suffix" runat="server" Width="55px"></asp:TextBox>
											</div>
										</td>
									</tr>
									<tr>
										<td class='c'>Website:</td>
										<td class='v'><asp:TextBox ID="a_website" runat="server"></asp:TextBox></td>
									</tr>
									<tr>
										<td class='c'>GPS Coordinates:</td>
										<td class='v'><asp:TextBox ID="a_gpscoordinates" runat="server"></asp:TextBox></td>
									</tr>
									<tr>
										<td class='c'>Facebook:</td>
										<td class='v'><asp:TextBox ID="a_facebook" runat="server"></asp:TextBox></td>
									</tr>
									<tr>
										<td class='c'>Twitter:</td>
										<td class='v'><asp:TextBox ID="a_twitter" runat="server"></asp:TextBox></td>
									</tr>
									<tr>
										<td class='c'>Linkedin:</td>
										<td class='v'><asp:TextBox ID="a_linkedin" runat="server"></asp:TextBox></td>
									</tr>
									<tr>
										<td class='c'>&nbsp;</td>
										<td class='v'>&nbsp;</td>
									</tr>
									<tr>
										<td class='c'>&nbsp;</td>
										<td class='v'>&nbsp;</td>
									</tr>
									<tr>
										<td class='c'>&nbsp;</td>
										<td class='v'>&nbsp;</td>
									</tr>
									<tr>
										<td class='c'>&nbsp;</td>
										<td class='v'>&nbsp;</td>
									</tr>
									<tr>
										<td colspan='2'>
											<dx:ASPxButton ID="b_save_address" runat="server" 
												OnClick="b_save_address_Click" Text="Save" Theme="NETheme01">
												<ClientSideEvents Click="function(s, e) {
	s.disabled=true;
}" />
											</dx:ASPxButton>
										</td>
									</tr>
								</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Name="phone" Text="Phone Numbers">
				<ContentCollection>
					<dx:ContentControl runat="server">
						<dx:ASPxGridView ID="gv_phones" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_phones" DataSourceID="ds_phones" KeyFieldName="id" 
							OnCustomButtonCallback="gv_CustomButtonCallback" 
							OnCustomCallback="gv_CustomCallback" OnRowInserting="gv_phones_RowInserting" 
							OnRowUpdating="gv_phones_RowUpdating" Theme="NETheme01" Width="100%">
							<ClientSideEvents BatchEditStartEditing="function(s, e) {
 
edit_mode2.SetVisible(true);
hl_save2.SetVisible(true);
hl_cancel2.SetVisible(true);
	

}" CustomButtonClick="function(s, e) {



if (e.buttonID=='delete')
{
if (confirm('Are you sure you want to delete this phone number?'))
	{
	gv_phones.PerformCallback('DELETE|'+e.visibleIndex);
	}
}

}" />
							<Columns>
								<dx:GridViewCommandColumn ShowEditButton="True" ShowInCustomizationForm="True" 
									ShowNewButtonInHeader="True" VisibleIndex="0">
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
											<dx:ListEditItem Selected="True" Text="LandLine" Value="LandLine" />
										</Items>
									</PropertiesComboBox>
								</dx:GridViewDataComboBoxColumn>
							</Columns>
							<SettingsBehavior ConfirmDelete="True" />
							<SettingsPager Mode="ShowAllRecords" Visible="False">
							</SettingsPager>
							<SettingsEditing Mode="Batch">
							</SettingsEditing>
							<Settings ShowFooter="True" ShowStatusBar="Visible" />
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
											<td align="left" 
												style="text-decoration: blink; font-family: Arial, Helvetica, sans-serif; font-size: 14px" 
												width="100%">
												<dx:ASPxLabel ID="ASPxLabel2" runat="server" ClientInstanceName="edit_mode2" 
													ClientVisible="False" Font-Bold="True" ForeColor="Red" Text="Edit Mode">
												</dx:ASPxLabel>
											</td>
											<td>
												<dx:ASPxHyperLink ID="hl_save" runat="server" ClientInstanceName="hl_save2" 
													Cursor="pointer" Text="Save" Width="50px">
													<ClientSideEvents Click="function(s, e){edit_mode2.SetVisible(false);hl_save2.SetVisible(false);hl_cancel2.SetVisible(false); gv_phones.UpdateEdit(); }" />
												</dx:ASPxHyperLink>
											</td>
											<td>
												<dx:ASPxHyperLink ID="hl_cancel" runat="server" ClientInstanceName="hl_cancel2" 
													Cursor="pointer" Text="Cancel">
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
						
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Name="contacts" Text="Contacts">
				<ContentCollection>
					<dx:ContentControl runat="server">
						<uc:contact ID="uc_contacts" runat="server" />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Name="sales" Text="Sales">
				<ContentCollection>
					<dx:ContentControl runat="server">
						<uc:sales ID="uc_sales" runat="server" />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Name="notes" Text="Notes">
				<ContentCollection>
					<dx:ContentControl runat="server">
						<uc:notes ID="uc_notes" runat="server" />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Files">
				<ContentCollection>
					<dx:ContentControl runat="server">
						<uc:files ID="uc_files" runat="server" />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Assets">
				<ContentCollection>
					<dx:ContentControl runat="server">
						<uc:assets ID="uc_assets" runat="server" />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="BV Contacts" Name="bv_contacts">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table cellpadding="2" cellspacing="0" class="bv_contacts">
							<tr>
								<td align="center">
								</td>
								<td align="center" class="h">
									Contact 1
								</td>
								<td align="center" class="h">
									Contact 2
								</td>
								<td align="center" class="h" style="height: 23px;">
									Contact 3
								</td>
							</tr>
							<tr>
								<td class="c">
									Name:
								</td>
								<td>
									<dx:ASPxTextBox ID="bv_contact1_name" runat="server" Native="True" Width="170px">
									</dx:ASPxTextBox>
								</td>
								<td>
									<dx:ASPxTextBox ID="bv_contact2_name" runat="server" Native="True" Width="170px">
									</dx:ASPxTextBox>
								</td>
								<td>
									<dx:ASPxTextBox ID="bv_contact3_name" runat="server" Native="True" Width="170px">
									</dx:ASPxTextBox>
								</td>
							</tr>
							<tr>
								<td class="c">
									Phone:
								</td>
								<td align="left">
									(<dx:ASPxTextBox ID="bv_contact1_phone1" runat="server" MaxLength="3"  Native="True" Width="30px">
									</dx:ASPxTextBox>
									)
									<dx:ASPxTextBox ID="bv_contact1_phone2" runat="server" MaxLength="3"  Native="True" Width="30px">
									</dx:ASPxTextBox>
									-<dx:ASPxTextBox ID="bv_contact1_phone3" runat="server" MaxLength="4"  Native="True" Width="40px">
									</dx:ASPxTextBox>
									<dx:ASPxTextBox ID="bv_contact1_phoneext" runat="server"  Native="True" Width="30px">
									</dx:ASPxTextBox>
								</td>
								<td align="left">
									(<dx:ASPxTextBox ID="bv_contact2_phone1" runat="server" MaxLength="3"  Native="True" Width="30px">
									</dx:ASPxTextBox>
									)
									<dx:ASPxTextBox ID="bv_contact2_phone2" runat="server" MaxLength="3"  Native="True" Width="30px">
									</dx:ASPxTextBox>
									-<dx:ASPxTextBox ID="bv_contact2_phone3" runat="server" MaxLength="4"  Native="True" Width="40px">
									</dx:ASPxTextBox>
									<dx:ASPxTextBox ID="bv_contact2_phoneext" runat="server"  Native="True" Width="30px">
									</dx:ASPxTextBox>
								</td>
								<td align="left">
									(<dx:ASPxTextBox ID="bv_contact3_phone1" runat="server" MaxLength="3"  Native="True" Width="30px">
									</dx:ASPxTextBox>
									)
									<dx:ASPxTextBox ID="bv_contact3_phone2" runat="server" MaxLength="3"  Native="True" Width="30px">
									</dx:ASPxTextBox>
									-<dx:ASPxTextBox ID="bv_contact3_phone3" runat="server" MaxLength="4"  Native="True" Width="40px">
									</dx:ASPxTextBox>
									<dx:ASPxTextBox ID="bv_contact3_phoneext" runat="server"  Native="True" Width="30px">
									</dx:ASPxTextBox>
								</td>
							</tr>
							<tr>
								<td rowspan="1" class="c">
									Fax:
								</td>
								<td align="left" rowspan="1">
									(<dx:ASPxTextBox ID="bv_contact1_fax1" runat="server" Native="True" Width="30px" MaxLength="3" >
									</dx:ASPxTextBox>
									)
									<dx:ASPxTextBox ID="bv_contact1_fax2" runat="server" Native="True" Width="30px" MaxLength="3" >
									</dx:ASPxTextBox>
									-<dx:ASPxTextBox ID="bv_contact1_fax3" runat="server" Native="True" Width="40px" MaxLength="4" >
									</dx:ASPxTextBox>
								</td>
								<td align="left" rowspan="1">
									(<dx:ASPxTextBox ID="bv_contact2_fax1" runat="server" Native="True" Width="30px" MaxLength="3" >
									</dx:ASPxTextBox>
									)
									<dx:ASPxTextBox ID="bv_contact2_fax2" runat="server" Native="True" Width="30px" MaxLength="3" >
									</dx:ASPxTextBox>
									-<dx:ASPxTextBox ID="bv_contact2_fax3" runat="server" Native="True" Width="40px" MaxLength="4" >
									</dx:ASPxTextBox>
								</td>
								<td align="left" rowspan="1">
									(<dx:ASPxTextBox ID="bv_contact3_fax1" runat="server" Native="True" Width="30px" MaxLength="3" >
									</dx:ASPxTextBox>
									)
									<dx:ASPxTextBox ID="bv_contact3_fax2" runat="server" Native="True" Width="30px" MaxLength="3" >
									</dx:ASPxTextBox>
									-<dx:ASPxTextBox ID="bv_contact3_fax3" runat="server" Native="True" Width="40px" MaxLength="4" >
									</dx:ASPxTextBox>
								</td>
							</tr>
							<tr>
								<td rowspan="1" class="c">
									Email:
								</td>
								<td rowspan="1">
									<dx:ASPxTextBox ID="bv_contact1_email" runat="server" Native="True" Width="170px" >
									</dx:ASPxTextBox>
								</td>
								<td rowspan="1">
									<dx:ASPxTextBox ID="bv_contact2_email" runat="server" Native="True" Width="170px" >
									</dx:ASPxTextBox>
								</td>
								<td rowspan="1">
									<dx:ASPxTextBox ID="bv_contact3_email" runat="server" Native="True" Width="170px" >
									</dx:ASPxTextBox>
								</td>
							</tr>
						</table>
						<asp:Button ID="save_bvcontacts" runat="server" OnClick="save_bvcontacts_Click" Text="Save Contacts" />
						<br />
						
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		    <dx:TabPage Text="Accounting">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <table style="width:100%;">
                            <tr>
                                <td class="c">&nbsp;</td>
                                <td class="v">
                                    &nbsp;</td>
                                <td class="v">&nbsp;</td>
                                <td class="v">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="c">Default GL Account:</td>
                                <td class="v">
                                    <dx:ASPxComboBox ID="cb_gl_account" runat="server" AutoResizeWithContainer="True" Native="True" TextField="text" ValueField="id" ValueType="System.Int32" ClientEnabled="False">
                                    </dx:ASPxComboBox>
                                </td>
                                <td class="v">&nbsp;</td>
                                <td class="v">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="c">Taxes 1:</td>
                                <td class="v">
                                    <div class="tax_select">
                                        <asp:DropDownList ID="a_tax1" runat="server" DataSourceID="ds_tax" DataTextField="name" DataValueField="id">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td class="v">
                                    <div class="tax_input">
                                        <asp:TextBox ID="a_tax1_ex" runat="server"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="c">Taxes 2:</td>
                                <td class="v">
                                    <div class="tax_select">
                                        <asp:DropDownList ID="a_tax2" runat="server" DataSourceID="ds_tax" DataTextField="name" DataValueField="id">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td class="v">
                                    <div class="tax_input">
                                        <asp:TextBox ID="a_tax2_ex" runat="server"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="c">Taxes 3:</td>
                                <td class="v">
                                    <div class="tax_select">
                                        <asp:DropDownList ID="a_tax3" runat="server" DataSourceID="ds_tax" DataTextField="name" DataValueField="id">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td class="v">
                                    <div class="tax_input">
                                        <asp:TextBox ID="a_tax3_ex" runat="server"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="c">Taxes 4:</td>
                                <td class="v">
                                    <div class="tax_select">
                                        <asp:DropDownList ID="a_tax4" runat="server" DataSourceID="ds_tax" DataTextField="name" DataValueField="id">
                                        </asp:DropDownList>
                                    </div>
                                </td>
                                <td class="v">
                                    <div class="tax_input">
                                        <asp:TextBox ID="a_tax4_ex" runat="server"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="c">
                                    <dx:ASPxButton ID="b_save_address0" runat="server" OnClick="b_save_address_accounting_Click" Text="Save" Theme="NETheme01">
                                        <ClientSideEvents Click="function(s, e) {
	s.disabled=true;
}" />
                                    </dx:ASPxButton>
                                </td>
                                <td class="v">&nbsp;</td>
                                <td class="v">&nbsp;</td>
                                <td class="v">&nbsp;</td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
		</TabPages>
		<TabStyle VerticalAlign="Top" Height="25px">
		</TabStyle>
		<ContentStyle VerticalAlign="Top">
		</ContentStyle>
	</dx:ASPxPageControl>
    <asp:HiddenField ID="hdnaddressid" runat="server" />
	<asp:Button ID="b_cancel_edit" runat="server" onclick="b_cancel_edit_Click" Text="Cancel" />
</div>
<asp:HiddenField ID="hdn_address_id" runat="server" />
<asp:HiddenField ID="hdn_customer_id" runat="server" />
<asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 0 id, 'Not Set' name UNION SELECT tax_id id, CONCAT(tax_name,' - ', tax_percentage, '%') name FROM tax WHERE is_active = 1 ORDER BY id" ID="ds_tax"></asp:SqlDataSource>


			

