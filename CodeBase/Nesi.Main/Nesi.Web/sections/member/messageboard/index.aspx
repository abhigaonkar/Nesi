
<%@ Page Language="C#" MasterPageFile="../../../IntraDefault.master" AutoEventWireup="true"  Inherits="messageboard" Title="MessageBoard Admin" EnableTheming="True" EnableEventValidation="False" Codebehind="index.aspx.cs" %>



<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


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
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type ="text/javascript">


</script>
   
    <asp:HiddenField ID="hdncompanyid" runat="server" />
    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" 
		Width="100%" Height="900px" Theme="NETheme01" ActiveTabIndex="1" AutoPostBack="True">
		<TabPages>
			<dx:TabPage Text="Events and News">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
							KeyFieldName="DigitalSignage_ID" OnRowDeleting="ASPxGridView1_RowDeleting" 
							OnRowInserting="ASPxGridView1_RowInserting" 
							OnRowUpdating="ASPxGridView1_RowUpdating" Width="100%" Theme="NETheme01">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" >
<Image Width="16px" Url="~/images/icon/icon[delete].gif"></Image>
                                                                    </DeleteButton>
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" >
<Image Width="16px" Url="~/images/icon/icon[edit].gif"></Image>
                                                                    </EditButton>
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" >
<Image Width="16px" Url="~/images/icon/icon[add].gif"></Image>
                                                                    </NewButton>
																	<CancelButton Image-Url="~/images/icon/icon[cancel].gif" Image-Width="16px" Text="Cancel" >
<Image Width="16px" Url="~/images/icon/icon[cancel].gif"></Image>
                                                                    </CancelButton>
																</SettingsCommandButton>
							<Columns>
								<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
									ShowInCustomizationForm="True" VisibleIndex="0" Width="70px"  ShowEditButton="true" ShowNewButton="true" ShowCancelButton="true" ShowUpdateButton="true">
									
									
									
									
									
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn FieldName="DigitalSignage_ID" ReadOnly="True" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="business_unit_id" 
									ShowInCustomizationForm="True" VisibleIndex="2">
									<PropertiesComboBox DataSourceID="SqlDataSource1" TextField="name" 
										ValueField="id" ValueType="System.Int32">
									</PropertiesComboBox>
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataTextColumn Caption="Event Title" 
									FieldName="DigitalSignage_EventTitle" ShowInCustomizationForm="True" 
									VisibleIndex="3">
									<PropertiesTextEdit>
										<ValidationSettings CausesValidation="True" SetFocusOnError="True">
											<RequiredField IsRequired="True" />
										</ValidationSettings>
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataDateColumn Caption="Date" 
									FieldName="DigitalSignage_DateofEvent" ShowInCustomizationForm="True" 
									VisibleIndex="4">
									<PropertiesDateEdit DisplayFormatInEditMode="True" 
										EditFormatString="yyyy-MM-dd">
										<ValidationSettings CausesValidation="True" SetFocusOnError="True">
											<RequiredField IsRequired="True" />
										</ValidationSettings>
									</PropertiesDateEdit>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataDateColumn Caption="Start Showing" 
									FieldName="DigitalSignageStartDate" ShowInCustomizationForm="True" 
									VisibleIndex="5">
									<PropertiesDateEdit DisplayFormatInEditMode="True" 
										EditFormatString="yyyy-MM-dd" DisplayFormatString="yyyy-MM-dd">
										<ValidationSettings CausesValidation="True" SetFocusOnError="True">
											<RequiredField IsRequired="True" />
										</ValidationSettings>
									</PropertiesDateEdit>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataDateColumn Caption="End Showing" 
									FieldName="DigitalSignageEndDate" ShowInCustomizationForm="True" 
									VisibleIndex="6">
									<PropertiesDateEdit DisplayFormatInEditMode="True" 
										EditFormatString="yyyy-MM-dd" DisplayFormatString="yyyy-MM-dd">
										<ValidationSettings CausesValidation="True" SetFocusOnError="True">
											<RequiredField IsRequired="True" />
										</ValidationSettings>
									</PropertiesDateEdit>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataTextColumn Caption="Message" FieldName="DigitalSignage_Text" 
									ShowInCustomizationForm="True" VisibleIndex="7">
									<PropertiesTextEdit>
										<ValidationSettings CausesValidation="True" SetFocusOnError="True">
											<RequiredField IsRequired="True" />
										</ValidationSettings>
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataCheckColumn Caption="Every Year?" 
									FieldName="DigitalSignageRecurringEveryYear" ShowInCustomizationForm="True" 
									VisibleIndex="8">
									<PropertiesCheckEdit ConvertEmptyStringToNull="False" ValueChecked="1" 
										ValueType="System.Int32" ValueUnchecked="0">
									</PropertiesCheckEdit>
								</dx:GridViewDataCheckColumn>
								<dx:GridViewDataCheckColumn Caption="Active?" FieldName="DigitalSignage_Active" 
									ShowInCustomizationForm="True" VisibleIndex="9">
									<PropertiesCheckEdit ConvertEmptyStringToNull="False" ValueChecked="1" 
										ValueType="System.Int32" ValueUnchecked="0">
									</PropertiesCheckEdit>
								</dx:GridViewDataCheckColumn>
								<dx:GridViewDataTextColumn Caption="Employee" FieldName="Employee" 
									ShowInCustomizationForm="True" VisibleIndex="10">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsBehavior ConfirmDelete="True" EnableRowHotTrack="True" />
							<SettingsPager PageSize="25">
							</SettingsPager>
							<SettingsEditing Mode="Inline" />
							<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
								ShowHeaderFilterButton="True" />
						</dx:ASPxGridView>
						<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							
							SelectCommand="">
						</asp:SqlDataSource>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Name="Images" Text="Images" NewLine="False" TabStyle-VerticalAlign="Top">
<TabStyle VerticalAlign="Top"></TabStyle>
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxFileManager ID="ASPxFileManager1" runat="server" Width="100%" 
							Height="500px">
							<Styles>
								<Toolbar Height="100px">
								</Toolbar>
								<ToolbarItem Wrap="False">
								</ToolbarItem>
							</Styles>
							<SettingsEditing AllowDelete="True" AllowRename="True" />
							<SettingsToolbar ShowFilterBox="False" ShowPath="False" 
								ShowDownloadButton="True" />
							<settingsupload>
								<validationsettings multiselectionerrortext="Attention! 

The following {0} files are invalid because they exceed the allowed file size ({1}) or their extensions are not allowed. These files have been removed from selection, so they will not be uploaded. 

{2}" MaxFileSize="500000" NotAllowedFileExtensionErrorText="Only .jpg is allowed">
								</validationsettings>
							</settingsupload>
						</dx:ASPxFileManager>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Chat Style Message">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
				
						<dx:ASPxComboBox ID="ddlcompany" runat="server" AutoPostBack="True" 
							ClientInstanceName="ddlcompany"  DataSourceID="sqlcompanys"
							AnimationType="None" TextField="name" ValueField="id" 
							ValueType="System.Int32" Theme="NETheme01">
						</dx:ASPxComboBox>
				
						<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
							OnCallback="cb_Callback" Width="100%">
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
									<dx:ASPxGridView ID="gv_chat" runat="server" AutoGenerateColumns="False" 
										ClientInstanceName="gv_chat" DataSourceID="sqlchat" 
										KeyFieldName="messageboard_chat_id" Width="100%" Font-Names="Arial" Font-Size="9pt" 
										OnRowDeleting="gv_chat_RowDeleting" OnRowUpdating="gv_chat_RowUpdating" Theme="NETheme01">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" >
<Image Width="16px" Url="~/images/icon/icon[delete].gif"></Image>
                                                                    </DeleteButton>
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" >
<Image Width="16px" Url="~/images/icon/icon[edit].gif"></Image>
                                                                    </EditButton>
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" >
<Image Width="16px" Url="~/images/icon/icon[add].gif"></Image>
                                                                    </NewButton>
																	<CancelButton Image-Url="~/images/icon/icon[cancel].gif" Image-Width="16px" Text="Cancel" >
<Image Width="16px" Url="~/images/icon/icon[cancel].gif"></Image>
                                                                    </CancelButton>
																</SettingsCommandButton>
										<Columns>
											<dx:GridViewCommandColumn ButtonType="Image" Caption=" "  ShowEditButton="true" ShowDeleteButton="true"
												ShowInCustomizationForm="True" VisibleIndex="0" Width="75px">
												
												
												
												
												<CellStyle Wrap="False">
												</CellStyle>
											</dx:GridViewCommandColumn>
											<dx:GridViewDataTextColumn Caption="id" FieldName="messageboard_chat_id" 
												ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1" Width="1px">
												<EditFormSettings Visible="False" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataDateColumn Caption="Date" FieldName="messageboard_date" 
												ShowInCustomizationForm="True" VisibleIndex="2" Width="120px">
												<PropertiesDateEdit DisplayFormatInEditMode="True" 
													DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
													EditFormatString="yyyy-MM-dd" AnimationType="None">
												</PropertiesDateEdit>
											</dx:GridViewDataDateColumn>
											<dx:GridViewDataTextColumn Caption="Note" FieldName="text" 
												ShowInCustomizationForm="True" VisibleIndex="3" Width="100%">
												<PropertiesTextEdit Width="400px">
												</PropertiesTextEdit>
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataComboBoxColumn Caption="Business Unit" 
												FieldName="business_unit_id" ShowInCustomizationForm="True" 
												VisibleIndex="4" Width="100px">
												<PropertiesComboBox DataSourceID="sqlcompanys" AnimationType="None" 
													TextField="name" ValueField="id" ValueType="System.Int32">
												</PropertiesComboBox>
											</dx:GridViewDataComboBoxColumn>
											<dx:GridViewDataComboBoxColumn Caption="Member" 
												FieldName="messageboard_memberid" ShowInCustomizationForm="True" 
												VisibleIndex="5" Width="100px">
												<PropertiesComboBox DataSourceID="sqlmembers" AnimationType="None" 
													TextField="name" ValueField="id" ValueType="System.Int32">
												</PropertiesComboBox>
											</dx:GridViewDataComboBoxColumn>
										</Columns>
										<SettingsBehavior ColumnResizeMode="Control" />
										<SettingsEditing EditFormColumnCount="1" Mode="Inline" />
										<Settings ShowTitlePanel="True" />
										<Styles>
											<CommandColumn Spacing="10px">
											</CommandColumn>
										</Styles>
										<Templates>
											<TitlePanel>
												<table style="width:100%;">
													<tr>
														<td>
															<dx:ASPxTextBox ID="txtaddnote" runat="server" BackColor="#FFFFCC" 
																ClientInstanceName="txtaddnote" Width="400px">
															</dx:ASPxTextBox>
														</td>
														<td class="dxtcLeftAlignCell" width="100%">
															<dx:ASPxButton ID="btnaddnote" runat="server" AutoPostBack="False" 
																ClientInstanceName="btnaddnote" Text="Add">
																<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback('add_note')
}" />
															</dx:ASPxButton>
														</td>
														<td>
															&nbsp;</td>
													</tr>
												</table>
											</TitlePanel>
											<EditForm>
												<dx:ASPxGridViewTemplateReplacement ID="Editors" runat="server" 
													ReplacementType="EditFormEditors" />
												<div style="margin-top: 10px; margin-bottom: 20px; padding-bottom: 20px;">
													<div style="float: left; margin-left: 5px">
														<dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" 
															ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' 
															Text="Cancel" Width="100px" />
													</div>
													<div style="float: left; margin-left: 10px">
														<dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" 
															ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' 
															CssClass="input" Text="Save" Width="100px" />
													</div>
												</div>
											</EditForm>
										</Templates>
									</dx:ASPxGridView>
								</dx:PanelContent>
							</PanelCollection>
						</dx:ASPxCallbackPanel>
						<asp:SqlDataSource ID="sqlchat" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT messageboard_chat_id, messageboard_date, urldecode(messageboard_text) text, business_unit_id, messageboard_memberid FROM messageboard_chat where 
business_unit_id = ?business_unit_id order by messageboard_chat_id desc">
							<SelectParameters>
								<asp:ControlParameter ControlID="ddlcompany" Name="business_unit_id" 
									PropertyName="Value" />
							</SelectParameters>
						</asp:SqlDataSource>
						<asp:SqlDataSource ID="sqlmembers" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							SelectCommand="Select member_id as id, member_fullname as name from member where member_status = 'Active' and business_unit_id = ?id order by member_nickname ">
							<SelectParameters>
								<asp:ControlParameter ControlID="ddlcompany" Name="id" 
									PropertyName="Value" />
							</SelectParameters>
						</asp:SqlDataSource>
						<asp:SqlDataSource ID="sqlcompanys" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							SelectCommand="">
						</asp:SqlDataSource>
				
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>
	</dx:ASPxPageControl>
   
    </ASP:CONTENT>
