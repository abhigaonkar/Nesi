<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" 
    AutoEventWireup="true" Inherits="member_applicants" 
    Title="Applicants" EnableTheming="True" Theme="NETheme01" Codebehind="applicants.aspx.cs" %>


<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">

	<script type="text/javascript" language="javascript">
function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 10) + 'px';
	
 }
 var applicants		= {
						check_name:	
							function(s,e)
								{
								var f		= name_first.GetText();
								var l		= name_last.GetText();
								var id		= lblid.GetText();
								if(id == "" && f != "" && l != "")
									{
							//		cb_name_check.PerformCallback(f+"|"+l);
									}
								},
						check_name_cb:
							{
							begin:
								function(s,e)
									{
									please_wait('start', 'Checking if this applicant is a pre-existing user');
									},
							complete:
								function(s,e)
									{
									var res			= $.parseJSON(e.result);
									var has_matches	= false;
									var matches		= "";
									var direct_hit	= res.type == "first" || res.type == "nick";
									if(direct_hit)
										{
										matches			+= "<b>These are direct matches for this user.<br/> Please make sure you are not trying to create a duplicate employee.<br/><div align='center' style='padding:5px;border-radius:5px;background-color:#f00;color:#fff;'>If you need to reactivate an existing employee, please contact HR.</div></b><br/>";
										}
									else if(res.type == "last")
										{
										matches			+= "<b>These are possible matches based on the user's last name.<br/>Please make sure you are not trying to create a duplicate employee.<br/><div align='center' style='padding:5px;border-radius:5px;background-color:#f00;color:#fff;'>If you need to reactivate an existing employee, please contact HR.</div></b><br/>";
										}
									if(res.info.length > 0 && res.type != "nomatch")
										{
										matches			+= "<table width='100%' cellspacing='0' cellpadding='2'><thead style='background-color:#01457C;color:#fff;'><tr><th width='33%'>Name</th><th width='33%'>Status</th><th width='33%'>Business Unit</th></tr></thead><tbody>";
										for(var i = 0; i < res.info.length; i++)
											{
											var sub_res	= res.info[i];
											matches		+= "<tr><td align='center'><b>"+sub_res.fullname+"</b></td><td align='center'>"+sub_res.status+"</td><td align='center'>"+sub_res.branch+"</td></tr>";
											}
										matches			+= "</tbody></table>";
										}
									$("#td_matches").html(matches);
									},
							error:
								function(s,e)
									{
									alert("There was an error trying to validate the name, you may need to refresh the page.");
									please_wait('stop');
									},
							end:
								function(s,e)
									{
									please_wait('stop');
									}
							}
 };


 function clear_div(){

 
   
    
     
    


 }
 </script>
 	<dx:ASPxCheckBox ID="chk_all" runat="server" AutoPostBack="True" 
										ClientInstanceName="chk_all" oncheckedchanged="chk_all_CheckedChanged" 
										Text="Show ALL Applicants" TextAlign="Left" >
									</dx:ASPxCheckBox>
	<br />
			<lc:LayoutControl runat="server" id="layout" __is_private="True" GridviewID="gv_applicants" />
        	<dx:ASPxGridView ID="gv_applicants" runat="server" AutoGenerateColumns="False" 
				KeyFieldName="id" Width="100%" 
		onhtmleditformcreated="gv_applicants_HtmlEditFormCreated" ClientInstanceName="gv_applicants" 
		oncustomcallback="gv_applicants_CustomCallback" 
		onstartrowediting="gv_applicants_StartRowEditing" 
		oncustomjsproperties="gv_applicants_CustomJSProperties" 
		oninitnewrow="gv_applicants_InitNewRow" 
		oncancelrowediting="gv_applicants_CancelRowEditing" 
		oncommandbuttoninitialize="gv_applicants_CommandButtonInitialize" 
		Font-Overline="False" Theme="NETheme01" 
		onrowdeleting="gv_applicants_RowDeleting">
				<SettingsSearchPanel Visible="True" />
				<StylesPopup>
					<EditForm>
						<MainArea>
							<Paddings Padding="2px" />
						</MainArea>
					</EditForm>
				</StylesPopup>
		<Templates>
                    <TitlePanel>
                        &nbsp;<table style="width:100%;">
							<tr>
								<td>
									<dx:ASPxButton ID="btnAddApp" runat="server" AutoPostBack="False" 
									 Text="Add Applicant" Theme="NETheme01">
										<ClientSideEvents Click="function(s, e) {
txt_check_last.SetText('');
txt_check_first.SetText('');                                            
txt_check_first.SetFocus();
                                            clear_div();
pop_new.Show();
	}" />
									</dx:ASPxButton>
								</td>
								<td>
									&nbsp;</td>
								<td width="100%" align="right">
								
								</td>
							</tr>
							<tr>
								<td colspan="3" nowrap="nowrap">
									If can&#39;t find the person you are looking for, perhaps they are already on the
									<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Cursor="pointer" 
										NavigateUrl="~/sections/hr/member/frame.aspx" Text="Employee Page" />
								</td>
							</tr>
						</table>
                    </TitlePanel>
                	<EditForm>
						
									<table width="100%">
										<tr>
											<td>
												<table>
													
													<tr>
														<td style="padding-right: 10px; text-align: center;" width="0%" class="style6" colspan="4">
															<dx:ASPxLabel ID="lbl_warning_gv_app" runat="server" ClientInstanceName="lbl_warning_gv_app" style="color: red">
                                                            </dx:ASPxLabel>
                                                        </td>
														<td rowspan="11" valign="top" width="100%" id="td_matches">
															&nbsp;</td>
													</tr>
													<tr>
                                                        <td class="style6" colspan="2" style="padding-right: 10px" width="0%"><strong>Details</strong></td>
                                                        <td>ID:</td>
                                                        <td class="style8">
                                                            <dx:ASPxLabel ID="lblid" runat="server" ClientInstanceName="lblid">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
													<tr>
														<td style="padding-right: 10px" width="0%" nowrap="nowrap">
															First Name
														</td>
														<td>
															<dx:ASPxTextBox ID="txtfirstname" runat="server" Width="170px" ClientInstanceName="name_first">
																<ClientSideEvents TextChanged="applicants.check_name" />
															</dx:ASPxTextBox>
														</td>
														<td>
															Last Name</td>
														<td class="style8">
															<dx:ASPxTextBox ID="txtlastname" runat="server" Width="170px" ClientInstanceName="name_last">
																<ClientSideEvents TextChanged="applicants.check_name" />
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="padding-right: 10px">
															Address</td>
														<td>
															<dx:ASPxTextBox ID="txtaddress" runat="server" Width="170px" 
																ClientInstanceName="txtaddress">
															</dx:ASPxTextBox>
														</td>
														<td>
															Apt</td>
														<td class="style8">
															<dx:ASPxTextBox ID="txtapt" runat="server" ClientInstanceName="txtapt" 
																Width="170px">
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="padding-right: 10px">
															City</td>
														<td>
															<dx:ASPxTextBox ID="txtcity" runat="server" ClientInstanceName="txtcity" 
																Width="170px">
															</dx:ASPxTextBox>
														</td>
														<td nowrap="nowrap">
															State/Province</td>
														<td class="style8">
															<dx:ASPxComboBox ID="ddlprovince" runat="server" 
																ClientInstanceName="ddlprovince" DataSourceID="SqlDataSource2" 
																TextField="Prov_Desc" ValueField="prov_abbv" IncrementalFilteringMode="StartsWith">
															</dx:ASPxComboBox>
															<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
																ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
																ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																SelectCommand="Select * from prov order by prov_desc"></asp:SqlDataSource>
														</td>
													</tr>
													<tr>
														<td style="padding-right: 10px">
															Country</td>
														<td>
															<dx:ASPxComboBox ID="ddlcountry" runat="server" ClientInstanceName="ddlcountry" 
															 IncrementalFilteringMode="StartsWith" 
																SelectedIndex="1">
																<Items>
																	<dx:ListEditItem Text="Canada" Value="CAN" />
																	<dx:ListEditItem Selected="True" Text="USA" Value="USA" />
																	<dx:ListEditItem Text="Other" Value="Other" />
																</Items>
															</dx:ASPxComboBox>
														</td>
														<td>
															Zip/Postal</td>
														<td class="style8">
															<dx:ASPxTextBox ID="txtzip" runat="server" ClientInstanceName="txtzip" 
																Width="170px">
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="padding-right: 10px" nowrap="nowrap">
															Cell Phone</td>
														<td>
															<dx:ASPxTextBox ID="txtcell" runat="server" ClientInstanceName="txtcell" 
																Width="170px">
															</dx:ASPxTextBox>
														</td>
														<td nowrap="nowrap">
															Home Phone</td>
														<td class="style8">
															<dx:ASPxTextBox ID="txthomephone" runat="server" 
																ClientInstanceName="txthomephone" Width="170px">
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td style="padding-right: 10px">
															Email</td>
														<td>
															<dx:ASPxTextBox ID="txtemail" runat="server" ClientInstanceName="txtemail" 
																Width="170px">
															</dx:ASPxTextBox>
														</td>
														<td nowrap="nowrap">
															&nbsp;</td>
														<td class="style8">
															&nbsp;</td>
													</tr>
													<tr>
														<td style="padding-right: 10px">
															Business Unit</td>
														<td>
															<dx:ASPxComboBox ID="ddlcompany" runat="server" DataSourceID="sqlcompany" 
																 IncrementalFilteringMode="StartsWith" 
																TextField="name" ValueField="id" ValueType="System.Int32" 
																Width="170px">
															</dx:ASPxComboBox>
														</td>
														<td nowrap="nowrap">
															</td>
														<td class="style8">
															
														</td>
													</tr>
													<tr>
														<td nowrap="nowrap" style="padding-right: 10px">
															Title</td>
														<td>
															<dx:ASPxComboBox ID="ddlmembertype" runat="server" DataSourceID="sqlmembertype" 
																IncrementalFilteringMode="StartsWith" 
																TextField="membertype_name" ValueField="membertype_id" ValueType="System.Int32" 
																Width="170px">
															</dx:ASPxComboBox>
														</td>
														<td nowrap="nowrap">
															Status</td>
														<td class="style8">
															<dx:ASPxComboBox ID="ddlstatus" runat="server" 
																IncrementalFilteringMode="StartsWith" 
																Width="170px">
																<Items>
																	<dx:ListEditItem Text="New" Value="New" />
																	<dx:ListEditItem Text="Offered" Value="Offered" />
																	<dx:ListEditItem Text="Suspended" Value="Suspended" />
																	<dx:ListEditItem Text="Rejected" Value="Rejected" />
																	<dx:ListEditItem Text="Deleted" Value="Deleted" />
																	<dx:ListEditItem Text="Hired" Value="Hired" />
																</Items>
															</dx:ASPxComboBox>
														</td>
													</tr>
													<tr>
														<td nowrap="nowrap" style="padding-right: 10px">
															&nbsp;</td>
														<td>
															&nbsp;</td>
														<td nowrap="nowrap">
															<dx:ASPxCallback ID="cb_name_check" runat="server" ClientInstanceName="cb_name_check" oncallback="cb_name_check_Callback">
																<ClientSideEvents BeginCallback="applicants.check_name_cb.begin" CallbackComplete="applicants.check_name_cb.complete" EndCallback="applicants.check_name_cb.end" CallbackError="applicants.check_name_cb.error" />
															</dx:ASPxCallback>
														</td>
														<td class="style8">
															&nbsp;</td>
													</tr>
													<tr>
														<td style="padding-right: 10px">
															Notes</td>
														<td colspan="4">
															<dx:ASPxTextBox ID="txtnotes" runat="server" BackColor="#FFFFCC" Height="30px" 
																Width="100%">
															</dx:ASPxTextBox>
														</td>
													</tr>
													<tr>
														<td colspan="5" style="padding-right: 10px">
															<dx:ASPxButton ID="btnsave" runat="server" AutoPostBack="False" 
																ClientInstanceName="btnsave" Text="Save" Theme="NETheme01">
																<ClientSideEvents Click="function(s, e) {
	
                                                                    if (lbl_warning.GetText()!='')
                                                                    {
                                                                    if (confirm('Are you sure you want to save this?  You might be creating a duplicate applicant.  Click OK to continue'))
                                                                    {
 pop_new.Hide();
                                                                    gv_applicants.PerformCallback(&quot;saveApp&quot;);
                                                                   
                                                                    }
                                                                    }
                                                                    else
                                                                    {
                                                                     pop_new.Hide();
                                                                     gv_applicants.PerformCallback(&quot;saveApp&quot;);
                                                                    }

}" />
															</dx:ASPxButton>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										
                                            <tr>
                                                        <td class="style6" colspan="4" style="padding-right: 10px" width="0%"><strong>Offers of Employment</strong></td>
                                                        
                                                    </tr>
<tr>
											<td >
												<dx:ASPxGridView ID="gv_offers" runat="server" AutoGenerateColumns="False" 
													ClientInstanceName="gv_offers" ClientVisible="False" KeyFieldName="id" 
													OnCancelRowEditing="gv_offers_CancelRowEditing" 
													OnHtmlEditFormCreated="gv_offers_HtmlEditFormCreated" 
													OnRowDeleting="gv_offers_RowDeleting" Width="100%" OnCommandButtonInitialize="gv_offers_CommandButtonInitialize" 
													Theme="NETheme01">
													<SettingsCommandButton>
														<EditButton  Image-Url="~/images/icon/icon[edit].gif" />
														<DeleteButton Image-Url="~/images/icon/icon[delete].gif" />
													</SettingsCommandButton>
													<Columns>
														<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true" Width="40px">
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewCommandColumn>
														<dx:GridViewDataDateColumn Caption="Date" FieldName="date" MinWidth="10" 
															VisibleIndex="1" Width="90px">
															<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																EditFormatString="yyyy-MM-dd">
															</PropertiesDateEdit>
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataDateColumn>
														<dx:GridViewDataComboBoxColumn Caption="Author" FieldName="enteredby" 
															MinWidth="10" VisibleIndex="2" Width="70px">
															<PropertiesComboBox DataSourceID="sqlmember" TextField="name" 
																ValueField="member_id" ValueType="System.Int32">
															</PropertiesComboBox>
														</dx:GridViewDataComboBoxColumn>
														<dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="business_unit_id" 
															MinWidth="10" Visible="False" VisibleIndex="3" 
															Width="70px">
															<PropertiesComboBox DataSourceID="sqlcompany" TextField="name" 
																ValueField="id" ValueType="System.Int32">
															</PropertiesComboBox>
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataComboBoxColumn>
														<dx:GridViewDataTextColumn Caption="Wage" FieldName="wage" Visible="False" 
															VisibleIndex="4" Width="50px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataDateColumn Caption="Start Date" FieldName="startdate" 
															VisibleIndex="5" Width="90px">
															<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																EditFormatString="yyyy-MM-dd">
															</PropertiesDateEdit>
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataDateColumn>
														<dx:GridViewDataDateColumn Caption="End Date" FieldName="enddate" 
															VisibleIndex="6" Width="90px">
															<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																EditFormatString="yyyy-MM-dd">
															</PropertiesDateEdit>
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataDateColumn>
														<dx:GridViewDataTextColumn Caption="Title" FieldName="membertypeid" 
															VisibleIndex="10" Width="70px">
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataCheckColumn Caption="Vehicle" FieldName="gets_vehicle" 
															Visible="False" VisibleIndex="11" Width="50px">
														</dx:GridViewDataCheckColumn>
														<dx:GridViewDataCheckColumn Caption="Cell Phone" FieldName="gets_phone" 
															Visible="False" VisibleIndex="12" Width="50px">
														</dx:GridViewDataCheckColumn>
														<dx:GridViewDataCheckColumn Caption="Laptop" FieldName="gets_laptop" 
															Visible="False" VisibleIndex="13" Width="50px">
														</dx:GridViewDataCheckColumn>
														<dx:GridViewDataTextColumn Caption="Reports To" FieldName="reports_to" 
															VisibleIndex="14" Width="60px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataCheckColumn Caption="Comp?" FieldName="has_comp" 
															VisibleIndex="15" Width="50px">
														</dx:GridViewDataCheckColumn>
														<dx:GridViewDataMemoColumn Caption="Comp Details" FieldName="comp_details" 
															Visible="False" VisibleIndex="16" Width="60px">
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataMemoColumn>
														<dx:GridViewDataMemoColumn Caption="Notes" FieldName="notes" VisibleIndex="17" 
															Width="100%">
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataMemoColumn>
														<dx:GridViewDataCheckColumn Caption="Salary" FieldName="is_salary" 
															Visible="False" VisibleIndex="18" Width="50px">
														</dx:GridViewDataCheckColumn>
														<dx:GridViewDataCheckColumn Caption="Signed" FieldName="is_signed" 
															VisibleIndex="19" Width="60px">
														</dx:GridViewDataCheckColumn>
														<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
															VisibleIndex="23" Width="100px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="ID" FieldName="id" Visible="False" 
															VisibleIndex="22">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="memberid" FieldName="memberid" 
															Visible="False" VisibleIndex="21">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="isapplicant" FieldName="isapplicant" 
															Visible="False" VisibleIndex="25">
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
													<SettingsEditing Mode="PopupEditForm" />
													<Settings ShowFilterRow="True" ShowTitlePanel="True" 
														VerticalScrollBarMode="Auto" VerticalScrollableHeight="150" />
													
													<SettingsPopup>
														<EditForm HorizontalAlign="WindowCenter" 
															VerticalAlign="TopSides" Width="1100px" Height="800px" AllowResize="True" />
													</SettingsPopup>
													<Styles>
														<DetailRow Wrap="False">
														</DetailRow>
														<DetailCell Wrap="False">
														</DetailCell>
														<Cell Wrap="False">
														</Cell>
													</Styles>
													<StylesPopup>
														<EditForm>
															<PopupControl>
																<Paddings Padding="10px" />
															</PopupControl>
															<Content>
																<Paddings Padding="20px" />
															</Content>
															
														</EditForm>
													</StylesPopup>
													<Templates>
														<EditForm>
															<iframe ID="IFrame_Offer" runat="server" frameborder="0" height="700" 
																name="IFrame_Offer" width="100%"></iframe>
														</EditForm>
														<TitlePanel>
															<table style="width: 100%;">
																<tr>
																	<td align="left">
																		<dx:ASPxButton ID="btnAddNew" runat="server" AutoPostBack="False" 
																			ClientInstanceName="btnAddNew" HorizontalAlign="Center" Text="Add New" Theme="NETheme01">
																			<ClientSideEvents Click="function(s, e) {
	gv_offers.AddNewRow();
                                                                                
}" />
																			<Image Url="~/images/icon/icon[add].gif">
																			</Image>
																		</dx:ASPxButton>
																	</td>
																	<td>
																		&nbsp;</td>
																	<td>
																		&nbsp;</td>
																</tr>
															</table>
														</TitlePanel>
													</Templates>
												</dx:ASPxGridView>
											</td>
										</tr>
										<tr id="tr_app_files" runat="server" visible="False">
											<td valign="top" >
											
												<span class="style6">
												<strong style="vertical-align: top; margin-right: 0px; margin-bottom: 0px; padding-right: 0px; padding-bottom: 0px;">
												Applicant&nbsp; Files</strong></span><iframe ID="div_files" runat="server" 
													frameborder="0" name="I1" scrolling="no" 
													style="margin: 0px; padding: 0px; vertical-align: top" width="100%" marginwidth="0"></iframe>
													
												<br />
												<asp:HiddenField ID="hdnid1" runat="server" />
													
											</td>
										</tr>
										
									</table>
						
						
						
					</EditForm>
                </Templates>
				<Columns>
					<dx:GridViewCommandColumn VisibleIndex="0" Width="85px" Caption=" " ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true">
						
						
						
					</dx:GridViewCommandColumn>
					<dx:GridViewDataTextColumn Caption="Name" FieldName="_name" 
						VisibleIndex="1" Name="name" Width="100%">
						<EditFormSettings Visible="True" />
						<CellStyle Font-Bold="True" Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataComboBoxColumn Caption="Application Status" FieldName="status" 
						Name="status" VisibleIndex="2" Width="50px">
						<PropertiesComboBox>
							<Items>
								<dx:ListEditItem Text="New" Value="New" />
								<dx:ListEditItem Text="Offered" Value="Offered" />
								<dx:ListEditItem Text="Suspended" Value="Suspended" />
								<dx:ListEditItem Text="Rejected" Value="Rejected" />
								<dx:ListEditItem Text="Hired" Value="Hired" />
								<dx:ListEditItem Text="Deleted" Value="Deleted" />
							</Items>
						</PropertiesComboBox>
						<SettingsHeaderFilter Mode="CheckedList">
                        </SettingsHeaderFilter>
						<EditFormSettings Visible="True" />
						<CellStyle HorizontalAlign="Center" Wrap="False">
						</CellStyle>
                         <DataItemTemplate>
                             <dx:ASPxComboBox ID="ddl_status" Width="100px" runat="server" ValueType="System.String" Value='<%# Eval("status") %>' Theme="NETheme01" OnInit="ddl_status_Init">
                                 <Items>
								<dx:ListEditItem Text="New" Value="New" />
								<dx:ListEditItem Text="Offered" Value="Offered" />
								<dx:ListEditItem Text="Suspended" Value="Suspended" />
								<dx:ListEditItem Text="Rejected" Value="Rejected" />
								<dx:ListEditItem Text="Hired" Value="Hired" />
								<dx:ListEditItem Text="Deleted" Value="Deleted" />
							</Items>
                             </dx:ASPxComboBox>
                            
                        </DataItemTemplate>
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataDateColumn Caption="Date Entered" FieldName="dateentered" 
						VisibleIndex="3" Width="150px">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
							EditFormatString="yyyy-MM-dd">
						</PropertiesDateEdit>
						<EditFormSettings Visible="False" />
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataComboBoxColumn Caption="Entered By" FieldName="addedbymemberid" 
						VisibleIndex="4" Width="100px">
						<PropertiesComboBox DataSourceID="Sqlmember" TextField="name" 
							ValueField="member_id" ValueType="System.Int32">
						</PropertiesComboBox>
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataComboBoxColumn Caption="Membertype" FieldName="membertypeid" 
						VisibleIndex="5" Width="100%">
						<PropertiesComboBox DataSourceID="sqlmembertype" TextField="membertype_name" 
							ValueField="membertype_id" ValueType="System.Int32">
						</PropertiesComboBox>
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="business_unit_id" 
						VisibleIndex="6" Width="100px">
						<PropertiesComboBox DataSourceID="sqlcompany" TextField="name" 
							ValueField="id" ValueType="System.Int32">
						</PropertiesComboBox>
						<SettingsHeaderFilter Mode="CheckedList">
                        </SettingsHeaderFilter>
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataComboBoxColumn>
				
					<dx:GridViewDataTextColumn Caption="Offer Status" FieldName="offer_status" 
						VisibleIndex="10" Width="125px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Cell Phone" FieldName="cellphone" 
						VisibleIndex="12" Width="75px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Email" FieldName="email" VisibleIndex="14" 
						Width="75px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
				    <dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" VisibleIndex="15" Width="150px">
                        <DataItemTemplate>
                            <table style="width:100%;">
                                <tr>
                                    <td width="100%">
                                        <dx:ASPxMemo ID="mem_newnote" runat="server" OnInit="mem_newnotes_Init" Height="30px" Theme="NETheme01" Width="100%">
                            </dx:ASPxMemo>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_add_note" runat="server" OnInit="mem_btn_add_notes_Init" Text="Add" Theme="NETheme01" AutoPostBack="False">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <dx:ASPxMemo ID="mem_notes" runat="server" BackColor="#FFFFCC" OnInit="mem_notes_Init" Rows="2" Text='<%# Eval("notes") %>' Theme="NETheme01" Width="250px" Height="75px">
                                        </dx:ASPxMemo>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
				</Columns>
				<SettingsBehavior ColumnResizeMode="Control" EnableCustomizationWindow="True" />
				<SettingsPager PageSize="50">
				</SettingsPager>
				<SettingsEditing Mode="PopupEditForm" />
				<Settings ShowFilterRow="True" ShowFilterRowMenu="True" 
					ShowHeaderFilterButton="True" ShowTitlePanel="True" ShowFilterBar="Visible" />
				<SettingsText PopupEditFormCaption="Applicant Details" />
				<SettingsPopup>
					<EditForm HorizontalAlign="WindowCenter" MinHeight="600px" MinWidth="950px" 
						VerticalAlign="TopSides" Width="980px" />
				</SettingsPopup>
				<Styles>
					
				</Styles>
			</dx:ASPxGridView>
			<asp:SqlDataSource ID="sqlcompany" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		>
	</asp:SqlDataSource>
			
			
			 <dx:ASPxPopupControl ID="pop_new" runat="server" AppearAfter="0" ClientInstanceName="pop_new" 
                 CloseAction="CloseButton" HeaderText="New Applicant" MinHeight="100px" Modal="True" 
                 OnWindowCallback="pop_new_WindowCallback" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" 
                 Theme="NETheme01" Width="800px" ShowPageScrollbarWhenModal="True" >
   
                 <ClientSideEvents PopUp="function(s, e) {
	
                     clear_div;
}" EndCallback="function(s, e) {
lbl_warning.SetText(lbl_warning.GetText());
	if (s.cp_close!=null)
{
if (s.cp_close=='proceed')
{

gv_applicants.AddNewRow();
                   
             s.cp_close=null;      
                    
}
}
}" />
   
                 <ModalBackgroundStyle Opacity="0">
                 </ModalBackgroundStyle>
   
                <ContentCollection>
<dx:PopupControlContentControl runat="server">

                  
    <table style="width:100%;">
        <tr>
            <td>
                <dx:ASPxTextBox ID="txt_check_first" runat="server" Height="30px" Theme="NETheme01" Width="170px"  NullText="Enter First Name" ClientInstanceName="txt_check_first">
                    <ClientSideEvents KeyPress="function(s, e) {
	btn_add_final.SetVisible(false);
}" />
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
            <td style="margin-left: 80px">
                <dx:ASPxTextBox ID="txt_check_last" runat="server" Height="30px" Theme="NETheme01" Width="170px" NullText="Enter Last Name" ClientInstanceName="txt_check_last">
                    <ClientSideEvents KeyPress="function(s, e) {
	btn_add_final.SetVisible(false);
}" />
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
            <td>
                
                <dx:ASPxButton ID="btn_check" runat="server" Height="25px"  Theme="NETheme01" Width="150px" AutoPostBack="False" ClientInstanceName="btn_check" Text="Check -&gt;">
                    <ClientSideEvents Click="function(s, e) {
                        pop_new.PerformCallback();
}" />
                </dx:ASPxButton>
                
            </td>
        </tr>
       
        <tr>
            <td>
                &nbsp;</td>
            <td style="margin-left: 80px">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
       
        <tr>
            <td id="td_results" colspan="3"><div id="check_results" class="check_results" runat="server"></div></td>
        </tr>
         <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <dx:ASPxButton ID="btn_add_final" runat="server" AutoPostBack="False" ClientInstanceName="btn_add_final" ClientVisible="False" Height="25px" Text="Proceed to Add -&gt;" Theme="NETheme01" Width="150px" UseSubmitBehavior="False">
                    <ClientSideEvents Click="function(s, e) {
                       
                    lbl_warning.SetText(pop_new.cp_warning);
                        //gv_applicants.AddNewRow();
                        gv_applicants.PerformCallback('AddApp|' + lbl_warning.GetText());
                   //     pop_new.Hide();

                        
}" />
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
                 




</dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>
			
			<br />
	<asp:SqlDataSource ID="sqlmembertype" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select membertype_id,membertype_name from membertype">
	</asp:SqlDataSource>
	
	<asp:SqlDataSource ID="Sqlmember" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select member_id,member_fullname name from member ORDER BY member_lastname, member_nickname">
	</asp:SqlDataSource>
	<asp:HiddenField ID="hdnid" runat="server" />
	<dx:ASPxTextBox ID="lbl_warning" runat="server" ClientInstanceName="lbl_warning" Width="170px" ClientVisible="False">
    </dx:ASPxTextBox>
	<br />
	<br />
</asp:Content>

<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style6
		{
			font-size: medium;
		}
		.style8 { WIDTH: 21%; }
	</style>
	</asp:Content>



