<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_member_modules_discipline_tab" Codebehind="discipline_tab.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>

	

	<style type="text/css">
		.style1
		{
			font-family: Calibri;
			font-size: small;
		}
	</style>
	<script type="text/javascript">
function preview(id)
			{
			boing("/_tools/get_file/index.aspx?file_id="+id+"&iframe=true", preview, 768, 480); 
			}
			</script>
<table border="0" cellpadding="0" cellspacing="0" class="main_border" 
																					width="100%">
																					<tr>
																					<td>
																						<table style="width:100%;">
																											<tr>
																												<td width="100%" class="style1" nowrap="nowrap" style="width: -100%">
																													Enter Date of Discipline:</td>
																												<td width="100%" colspan="2" style="width: 0%">
																													<dx:ASPxDateEdit ID="dte" runat="server" ClientInstanceName="dte" 
																														DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																														EditFormatString="yyyy-MM-dd" NullText="Select Date" Width="120px" AllowUserInput="False" Theme="NETheme01">
																													</dx:ASPxDateEdit>
																												</td>
																												<td width="100%">
																													&nbsp;</td>
																												<td>
																													&nbsp;</td>
																											</tr>
																											<tr>
																												<td width="100%" colspan="4">
																													&nbsp;</td>
																												<td>
																													&nbsp;</td>
																											</tr>
																											<tr>
																												<td width="100%" colspan="4">
																													<dx:ASPxHtmlEditor ID="mem" runat="server" ClientInstanceName="mem" 
																														Height="200px" Width="100%">
																														<Settings AllowHtmlView="False" AllowPreview="False" />
																													</dx:ASPxHtmlEditor>
																												</td>
																												<td>
																													<dx:ASPxButton ID="ASPxButton1" runat="server" Text="Add" 
																														onclick="ASPxButton1_Click" Theme="NETheme01">
																														<ClientSideEvents Click="function(s, e) {
if (mem.GetHtml()!='')
{
if (dte.GetText()!='Select Date')
{
	
}
else
{
alert('You must select a valid date');
}
}
}" />
																													</dx:ASPxButton>
																												</td>
																											</tr>
																											<tr>
																												<td width="100%" nowrap="nowrap" style="width: 0%; " class="style1" colspan="2">
																													Add File (Optional):</td>
																												<td width="100%" style="width: 50%" colspan="2">
																													<dx:ASPxUploadControl ID="auc" runat="server" 
																														AddUploadButtonsHorizontalPosition="Right" ClientInstanceName="auc" 
																														
																														Width="280px">
																														<ValidationSettings AllowedFileExtensions=".doc, .docx, .pdf, .rtf, .xls, .xlsx, .png, .jpeg, .jpg" 
																															MaxFileSize="1000000" MaxFileSizeErrorText="Files can't be bigger than 1Mb">
																														</ValidationSettings>
																													</dx:ASPxUploadControl>
																												</td>
																												<td>
																													&nbsp;</td>
																											</tr>
																										</table>
																					</td>
																					</tr>
																					<tr>
																						<td style="text-align: left">
																							<br />
																							<dx:ASPxGridView ID="gv_disc" runat="server" AutoGenerateColumns="False" 
																								ClientInstanceName="gv_disc" KeyFieldName="MemberNote_ID" 
																								 onrowupdating="gv_disc_RowUpdating" 
																								Theme="NETheme01" Font-Names="Arial" Width="100%" onrowdeleting="gv_disc_RowDeleting" oncancelrowediting="gv_disc_CancelRowEditing" 
																								oncommandbuttoninitialize="gv_disc_CommandButtonInitialize">
																								<Columns>
																									<dx:GridViewCommandColumn Caption=" " VisibleIndex="0"  ShowEditButton="true" ShowDeleteButton="true">
																										
																										
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewCommandColumn>
																									<dx:GridViewDataDateColumn Caption="Date" FieldName="Date" VisibleIndex="1">
																										<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																											EditFormatString="yyyy-MM-dd" Width="120px">
																										</PropertiesDateEdit>
																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataDateColumn>
																									<dx:GridViewDataTextColumn Caption="Entered By" FieldName="addedby" 
																										VisibleIndex="2">
																										<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>

																										<CellStyle Wrap="False">
																										</CellStyle>
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataMemoColumn Caption="Notes" FieldName="Comments" 
																										VisibleIndex="3" Width="100%">
																										<PropertiesMemoEdit Height="300px" Width="100%">
																											<Style Font-Names="Arial">
																											</Style>
																										</PropertiesMemoEdit>
																										<DataItemTemplate>
																											<dx:ASPxHtmlEditor ID="hl" runat="server" ActiveView="Html" 
																												EnableTheming="True" Font-Names="Arial" Height="90px" 
																												Html='<%# Eval("Comments") %>' Theme="iOS" Width="90%">
																												<Settings AllowDesignView="False" AllowHtmlView="False" />
																												<Border BorderStyle="None" />
																												<Settings AllowDesignView="False" AllowHtmlView="False" />
																												<Border BorderStyle="None" />
																											</dx:ASPxHtmlEditor>
																										</DataItemTemplate>
																										<EditItemTemplate>
																											<dx:ASPxHtmlEditor ID="ASPxHtmlEditor1" runat="server" Height="250px" 
																												Html='<%# Bind("comments") %>' Width="100%">
																												<Settings AllowHtmlView="False" AllowPreview="False" />
																												<Settings AllowHtmlView="False" AllowPreview="False" />
																											</dx:ASPxHtmlEditor>
																										</EditItemTemplate>
																									</dx:GridViewDataMemoColumn>
																									
																									<dx:GridViewDataTextColumn Caption="File" FieldName="file_id" VisibleIndex="4"  
																										Width="300px">
																									<DataItemTemplate>
																										<dx:ASPxHyperLink ID="link" runat="server" NavigateUrl='<%# string.Format("javascript:preview({0})", Eval("file_id")) %>' Text='<%# Eval("filename") %>' />
																										</DataItemTemplate>
																										<EditItemTemplate>
																											<dx:ASPxCallbackPanel ID="del_cb" runat="server" ClientInstanceName="del_cb" 
																												oncallback="del_cb_Callback" Width="100%">
																												<PanelCollection>
																													<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
																														<table>
																															<tr>
																																<td>
																																	<dx:ASPxHyperLink ID="link" runat="server" Font-Names="Arial" 
																																		NavigateUrl='<%# string.Format("javascript:preview({0})", Eval("file_id")) %>' 
																																		Text='<%# Eval("filename") %>'>
																																	</dx:ASPxHyperLink>
																																</td>
																																<td>
																																	<dx:ASPxButton ID="del_file" runat="server" AutoPostBack="False" 
																																		 OnInit="del_file_Init" Text="Delete">
																																	</dx:ASPxButton>
																																</td>
																																<tr>
																																	<td colspan="2">
																																		<dx:ASPxUploadControl ID="auc1" runat="server" 
																																			AddUploadButtonsHorizontalPosition="Right" ClientInstanceName="auc1" 
																																			Width="280px" OnFileUploadComplete="auc1_FileUploadComplete"  ShowUploadButton="True" OnInit="auc1_Init" ShowProgressPanel="True">
																																			<ValidationSettings AllowedFileExtensions=".doc, .docx, .pdf, .rtf, .xls, .xlsx, .png, .jpeg, .jpg, .bmp" 
																																				MaxFileSize="1000000" MaxFileSizeErrorText="Files can't be bigger than 1Mb">
																																			</ValidationSettings>
																																			<ClientSideEvents FileUploadComplete="function(s, e) {
	del_cb.PerformCallback('refresh');
}" />
																																		</dx:ASPxUploadControl>
																																	</td>
																																</tr>
																															</tr>
																														</table>
																													</dx:PanelContent>
																												</PanelCollection>
																											</dx:ASPxCallbackPanel>
																										</EditItemTemplate>
																										<CellStyle HorizontalAlign="Left">
																										</CellStyle>
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataTextColumn Caption="id" FieldName="MemberNote_ID" 
																										Visible="False" VisibleIndex="5" ReadOnly="True" PropertiesTextEdit-Width="50px">
																										<EditFormSettings Visible="True" />
<PropertiesTextEdit Width="50px"></PropertiesTextEdit>

<EditFormSettings Visible="True"></EditFormSettings>
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataTextColumn Caption="memberid" FieldName="MemberNote_Member_ID" 
																										Visible="False" VisibleIndex="6">
																									</dx:GridViewDataTextColumn>
																									<dx:GridViewDataTextColumn Caption="MemberNote_Addedby_Member_ID" FieldName="MemberNote_Addedby_Member_ID" 
																										Visible="False" VisibleIndex="7">
																									</dx:GridViewDataTextColumn>

																									

																								</Columns>
																								<SettingsBehavior ConfirmDelete="True" />

<SettingsBehavior ConfirmDelete="True"></SettingsBehavior>

																								<SettingsPager Mode="ShowAllRecords" Visible="False">
																								</SettingsPager>
																								<SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1" />
																								<Settings ShowTitlePanel="True" />
																								<SettingsText PopupEditFormCaption="Edit Entry" />

<SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1"></SettingsEditing>

<Settings ShowTitlePanel="True"></Settings>

<SettingsText PopupEditFormCaption="Edit Entry"></SettingsText>

																								<SettingsPopup>
																									<EditForm HorizontalAlign="Center" Modal="True" VerticalAlign="WindowCenter" 
																										Height="500px" Width="1000px" />
<EditForm HorizontalAlign="Center" Width="1000px" Height="500px" VerticalAlign="WindowCenter" Modal="True"></EditForm>
																								</SettingsPopup>
																								<Styles>
																									<EditForm Font-Names="Arial">
																									</EditForm>
																								</Styles>
																								
																							</dx:ASPxGridView>
																						</td>
																					</tr>
																				</table>
																				
																				<asp:HiddenField ID="hdnaddedid" runat="server" />
<asp:HiddenField ID="hdnid" runat="server" />
<asp:HiddenField ID="hdncompid" runat="server" />
