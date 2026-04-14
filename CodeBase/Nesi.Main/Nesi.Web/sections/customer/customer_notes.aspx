<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" EnableTheming="false" AutoEventWireup="true" Inherits="customer_notes" Title="Customer notes" Codebehind="customer_notes.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">


	<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
		oncallback="cb_Callback" Width="200px">
		<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxLabel ID="lblerror" runat="server" ClientInstanceName="lblerror" 
		ClientVisible="False" style="color: #FF3300" Text="ASPxLabel">
	</dx:ASPxLabel>
	<dx:ASPxPageControl ID="pc" runat="server" ActiveTabIndex="2" 
		Font-Names="Arial" Font-Size="9pt"  Width="100%">
		<TabPages>
			<dx:TabPage Name="CollectionNotes" Text="Customer AR Notes (Private)">
				<TabStyle ForeColor="#000099" Height="25px">
				</TabStyle>
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Save" Visible ="false" Enabled="false">
							<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback('save_ar_notes');
}" />
						</dx:ASPxButton>
						<dx:ASPxMemo ID="customer_arnotes" runat="server" BackColor="#FFFFCC" 
							Height="150px" Width="100%" ClientInstanceName="customer_arnotes">
							
						</dx:ASPxMemo>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Special Invoicing Instructions (If no email address exists, this will appear on the invoice)">
				<TabStyle ForeColor="#9900CC" Wrap="True" Height="25px">
				</TabStyle>
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Save" Visible ="false" Enabled="false">
							<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback('save_invoicing_instructions');
}" />
						</dx:ASPxButton>
						<dx:ASPxMemo ID="customer_memo" runat="server" BackColor="#FFFFCC" 
							Height="150px" Width="100%" ClientInstanceName="customer_memo">
							
						</dx:ASPxMemo>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Sales Notes (Seen by sales and PM's)">
				<TabStyle ForeColor="#663300" Height="25px">
				</TabStyle>
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table style="width:100%;">
							<tr>
								<td>
									<dx:ASPxMemo ID="memaddsalesnote" runat="server" 
										ClientInstanceName="memaddsalesnote" Height="71px" Width="600px" Font-Names="Arial">
										
									</dx:ASPxMemo>
								</td>
								<td width="100%">
									<dx:ASPxButton ID="btnaddsalesnote" runat="server" AutoPostBack="False" 
										ClientInstanceName="btnaddsalesnote"  
										Text="Add Note" Font-Names="Arial" HorizontalAlign="Center" Visible ="false" Enabled="false">
										<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback('add_sales_note');
}" />
										<Image Url="~/images/icon/icon[add].gif">
										</Image>
									</dx:ASPxButton>
								</td>
							</tr>
						</table>
						<br />
						<dx:ASPxMemo ID="customer_salesnotes" runat="server" BackColor="#FFFFCC" 
							Height="150px" Width="100%" ReadOnly="True" ClientInstanceName="customer_salesnotes">
							
						</dx:ASPxMemo>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Name="Public Notes" Text="Public Notes (Everyone can see) ">
				<TabStyle ForeColor="Red" Height="25px">
				</TabStyle>
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table style="width:100%;">
							<tr>
								<td>
									<dx:ASPxMemo ID="memaddpublicnotes" runat="server" 
										ClientInstanceName="memaddpublicnotes" Font-Names="Arial" Height="71px" 
										Width="600px">
									</dx:ASPxMemo>
								</td>
								<td width="100%">
									<dx:ASPxButton ID="btnaddpublicnotes" runat="server" AutoPostBack="False" 
										ClientInstanceName="btnaddpublicnotes" Font-Names="Arial" 
										HorizontalAlign="Center" Text="Add Note" Visible ="false" Enabled="false">
										<ClientSideEvents Click="function(s, e) {
	cb.PerformCallback('add_public_note');
}" />
										<Image Url="~/images/icon/icon[add].gif">
										</Image>
									</dx:ASPxButton>
								</td>
							</tr>
						</table>
						<br />
						<dx:ASPxMemo ID="mem_customer_notes" runat="server" BackColor="#FFFFCC" 
							Height="150px" Width="100%" ClientInstanceName="mem_customer_notes">
							
						</dx:ASPxMemo>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="AR Emails (Private)" Visible="false">
				<TabStyle Height="25px" ForeColor="#CC3300">
				</TabStyle>
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<span class="current_payroll"><strong>
						<br />
						</strong></span>
						<table style="width:100%;">
							<tr>
								<td style="font-size: 9pt">
									<span class="current_payroll"><strong>All emails to/from NESI staff to the 
									customers </strong><em><strong>Invoice</strong></em><strong> and </strong><em>
									<strong>Autostatement</strong></em><strong> contacts are found here.</strong></span></td>
							</tr>
							<tr>
								<td>
									<dx:ASPxGridView ID="gv_emails" runat="server" AutoGenerateColumns="False" 
										ClientInstanceName="gv_emails" Font-Names="Arial" Font-Size="9pt" 
										KeyFieldName="emaillog_id" Width="100%" OnHtmlEditFormCreated="gv_emails_HtmlEditFormCreated">
                                        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
										<Columns>
											<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
												ShowInCustomizationForm="True" VisibleIndex="0" ShowEditButton="true" >
												
											</dx:GridViewCommandColumn>
											<dx:GridViewDataDateColumn Caption="Date" FieldName="emaillog_timestamp" 
												ShowInCustomizationForm="True" SortIndex="0" SortOrder="Descending" 
												VisibleIndex="1" Width="100px">
												<PropertiesDateEdit DisplayFormatString="">
												</PropertiesDateEdit>
												<EditFormSettings Visible="False" />
											</dx:GridViewDataDateColumn>
											<dx:GridViewDataTextColumn Caption="To" FieldName="emaillog_to" 
												ShowInCustomizationForm="True" VisibleIndex="2" Width="100px">
												<EditFormSettings Visible="False" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="From" FieldName="emaillog_from" 
												ShowInCustomizationForm="True" VisibleIndex="3" Width="100px">
												<EditFormSettings Visible="False" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Subject" FieldName="emaillog_subject" 
												ShowInCustomizationForm="True" VisibleIndex="4" Width="100%">
												<EditFormSettings Visible="False" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn FieldName="emaillog_id" 
												ShowInCustomizationForm="True" VisibleIndex="5">
												<EditFormSettings Visible="False" />
											</dx:GridViewDataTextColumn>
											<dx:GridViewDataTextColumn Caption="Body" FieldName="emaillog_body" 
												ShowInCustomizationForm="True" Visible="False" VisibleIndex="6">
												<EditFormSettings Caption=" " Visible="True" />
												<EditItemTemplate>
													<dx:ASPxHtmlEditor ID="aremails" runat="server" ActiveView="Preview" 
														ClientEnabled="False" Height="250px" 
														Width="100%" ClientInstanceName="aremails">
														<Settings AllowDesignView="False" AllowHtmlView="False" />
														<Settings AllowDesignView="False" AllowHtmlView="False" />
														<SettingsImageUpload>
															<ValidationSettings AllowedFileExtensions=".jpe, .jpeg, .jpg, .gif, .png" MultiSelectionErrorText="Attention! 

The following {0} files are invalid because they exceed the allowed file size ({1}) or their extensions are not allowed. These files have been removed from selection, so they will not be uploaded. 

{2}">
															</ValidationSettings>
														</SettingsImageUpload>
														<SettingsImageSelector>
															<CommonSettings AllowedFileExtensions=".jpe, .jpeg, .jpg, .gif, .png" />
															<CommonSettings AllowedFileExtensions=".jpe, .jpeg, .jpg, .gif, .png" />
														</SettingsImageSelector>
														<SettingsDocumentSelector>
															<CommonSettings AllowedFileExtensions=".rtf, .pdf, .doc, .docx, .odt, .txt, .xls, .xlsx, .ods, .ppt, .pptx, .odp" />
															<CommonSettings AllowedFileExtensions=".rtf, .pdf, .doc, .docx, .odt, .txt, .xls, .xlsx, .ods, .ppt, .pptx, .odp" />
														</SettingsDocumentSelector>
													</dx:ASPxHtmlEditor>
												</EditItemTemplate>
											</dx:GridViewDataTextColumn>
										</Columns>
										<SettingsBehavior AllowSelectByRowClick="True" EnableRowHotTrack="True" 
											ProcessSelectionChangedOnServer="True" />
										<SettingsEditing EditFormColumnCount="1" />
										<Styles>
											<Header BackColor="#FFFFCC" Font-Bold="True">
											</Header>
											<Cell Font-Size="9pt">
											</Cell>
										</Styles>
										<Templates>
											<EditForm>
												<dx:ASPxGridViewTemplateReplacement ID="editors99" runat="server" 
													ReplacementType="EditFormEditors" />
												<div style="float: left; margin-left: 5px">
													<dx:ASPxButton ID="btnCancel9" runat="server" AutoPostBack="false" 
														ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' 
														Text="Close" Width="100px" />
												</div>
											</EditForm>
										</Templates>
									</dx:ASPxGridView>
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;</td>
							</tr>
						</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Invoice Notes (Read Only)">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
					<dx:ASPxGridView ID="gv_invoice_notes_all" runat="server" 
													AutoGenerateColumns="False" Width="100%" ClientInstanceName="gv_invoice_notes_all">
													<Columns>
														<dx:GridViewCommandColumn ShowInCustomizationForm="True" VisibleIndex="0" ShowClearFilterButton="true">
															
														</dx:GridViewCommandColumn>
														<dx:GridViewDataDateColumn Caption="Date" FieldName="Date" 
															ShowInCustomizationForm="True" VisibleIndex="1" Width="125px">
															<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
																EditFormatString="yyyy-MM-dd"></PropertiesDateEdit>
															<CellStyle Wrap="False">
															</CellStyle>
														</dx:GridViewDataDateColumn>
														<dx:GridViewDataTextColumn Caption="Invoice" FieldName="Invoice" 
															ShowInCustomizationForm="True" VisibleIndex="2" Width="80px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Note" FieldName="Note" 
															ShowInCustomizationForm="True" VisibleIndex="3" Width="100%">
															<DataItemTemplate>
																<dx:ASPxMemo ID="ASPxMemo1" runat="server" Width="100%" Text='<%# Eval("Note") %>'>
																	<Border BorderColor="#CCCCCC" />
																</dx:ASPxMemo>
															</DataItemTemplate>
															<CellStyle>
																<Paddings Padding="0px" />
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="By" FieldName="By" 
															ShowInCustomizationForm="True" VisibleIndex="4" Width="100px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Work Order" FieldName="wo" 
															ShowInCustomizationForm="True" VisibleIndex="5" Width="80px">
														</dx:GridViewDataTextColumn>
													</Columns>
													<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
												</dx:ASPxGridView>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>

<SettingsLoadingPanel ShowImage="False"></SettingsLoadingPanel>

		<TabStyle Width="150px" Height="25px" Wrap="True">
		</TabStyle>
	</dx:ASPxPageControl>
			</dx:PanelContent>
</PanelCollection>
	</dx:ASPxCallbackPanel>
	<br />


	</asp:Content>

