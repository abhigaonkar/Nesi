<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true"  Inherits="sections_hr_jobs_index" Title="Job Postings" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>










<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>
<asp:Content ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="side_menu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="body" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript">
		var inError		= false;
		var doneEditing = false;
		function check_handler(s,e)
			{
			var this_parent		= $(s.mainElement).parents("tr[class*=dxgvDataRow]:first");
			var this_id			= this_parent.find('.row_id').text();
			var this_css = $(s.mainElement).attr('class').replace("dxeBase_NETheme01 ", "").replace("dxeBase ", "").replace("dxeBase ", "").replace("dxichCellSys ", "").replace(" dxeTAR", "");
			var this_checked	= s.GetChecked();
			checkbox_handler.PerformCallback(this_id+","+this_css+","+this_checked);
			}
	</script>
	
	<dx:ASPxButton ID="bt_new" runat="server" Text="New Job Posting" AutoPostBack="False" Theme="NETheme01">
		<Image Url="~/images/icon/icon[add].gif">
		</Image>
		<ClientSideEvents Click="function(s, e) {
	gv_jobs.AddNewRow();
}" />
	</dx:ASPxButton>
	<dx:ASPxCallback ID="checkbox_handler" runat="server" ClientInstanceName="checkbox_handler"
		OnCallback="checkbox_handler_Callback">
		<ClientSideEvents BeginCallback="function(s, e) {
	please_wait('start')
}" CallbackComplete="function(s, e) {
	please_wait('stop')
}" />
	</dx:ASPxCallback>
	<br />
	<dx:ASPxGridView ID="gv_jobs" runat="server" AutoGenerateColumns="False" DataSourceID="ds_jobs"
		KeyFieldName="id" ClientInstanceName="gv_jobs" OnCustomButtonCallback="gv_jobs_CustomButtonCallback" Theme="NETheme01">
		<Templates>
			<EditForm>
				<dx:ASPxCallbackPanel ID="cbp_addedit" runat="server" ClientInstanceName="cbp_addedit"
					OnCallback="cbp_addedit_Callback" Width="200px" Theme="NETheme01">
					<PanelCollection>
						<dx:PanelContent runat="server">
				<table width="100%" cellpadding="2" cellspacing="0">
					<tr>
						<td colspan="2">
							<strong>Title:</strong></td>
					</tr>
					<tr>
						<td colspan="2">
							<dx:ASPxTextBox ID="t_title" runat="server" Text='<%# Bind("title") %>' Theme="NETheme01" Width="500px" ClientInstanceName="t_title">
							</dx:ASPxTextBox>
							<asp:HiddenField ID="hid_id" runat="server" Value='<%# Bind("id") %>' />
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<strong>Expiration Date:</strong></td>
					</tr>
					<tr>
						<td colspan="2">
							<dx:ASPxDateEdit ID="date_expiration" runat="server" Value='<%# Bind("expirationDate") %>' Theme="NETheme01" ClientInstanceName="date_expiration">
								<ClientSideEvents Init="function(s, e) {
	s.GetInputElement().disabled = true;
}" />
							</dx:ASPxDateEdit>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<strong>
							Display Order:</strong></td>
					</tr>
					<tr>
						<td colspan="2">
							<dx:ASPxTextBox ID="t_order" runat="server" Theme="NETheme01" Text='<%# Bind("displayOrder") %>'
								Width="170px" ClientInstanceName="t_order">
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<dx:ASPxPageControl id="pc" runat="server" ActiveTabIndex="0" Height="400px" Width="820px" Theme="NETheme01">
								<tabpages>
<dx:TabPage Text="Description"><ContentCollection>
<dx:ContentControl runat="server"><dx:ASPxHtmlEditor runat="server" Html='<%# Eval("description") %>' Width="775px" Height="350px" ID="html_description" ClientInstanceName="html_description" EnableTheming="False">
<Settings AllowInsertDirectImageUrls="False"></Settings>

<SettingsImageUpload>
<ValidationSettings AllowedFileExtensions=".jpe, .jpeg, .jpg, .gif, .png"></ValidationSettings>
</SettingsImageUpload>

<SettingsImageSelector>
<CommonSettings AllowedFileExtensions=".jpe, .jpeg, .jpg, .gif, .png"></CommonSettings>
</SettingsImageSelector>
	<StylesDialogForm>
		<Content>
			<Border BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
		</Content>
	</StylesDialogForm>
	<Toolbars>
		<dx:HtmlEditorToolbar Name="StandardToolbar1">
			<Items>
				<dx:ToolbarCutButton>
				</dx:ToolbarCutButton>
				<dx:ToolbarCopyButton>
				</dx:ToolbarCopyButton>
				<dx:ToolbarPasteButton>
				</dx:ToolbarPasteButton>
				<dx:ToolbarPasteFromWordButton>
				</dx:ToolbarPasteFromWordButton>
				<dx:ToolbarUndoButton BeginGroup="True">
				</dx:ToolbarUndoButton>
				<dx:ToolbarRedoButton>
				</dx:ToolbarRedoButton>
				<dx:ToolbarRemoveFormatButton BeginGroup="True">
				</dx:ToolbarRemoveFormatButton>
				<dx:ToolbarSuperscriptButton BeginGroup="True">
				</dx:ToolbarSuperscriptButton>
				<dx:ToolbarSubscriptButton>
				</dx:ToolbarSubscriptButton>
				<dx:ToolbarInsertOrderedListButton BeginGroup="True">
				</dx:ToolbarInsertOrderedListButton>
				<dx:ToolbarInsertUnorderedListButton>
				</dx:ToolbarInsertUnorderedListButton>
				<dx:ToolbarIndentButton BeginGroup="True">
				</dx:ToolbarIndentButton>
				<dx:ToolbarOutdentButton>
				</dx:ToolbarOutdentButton>
				<dx:ToolbarInsertLinkDialogButton BeginGroup="True">
				</dx:ToolbarInsertLinkDialogButton>
				<dx:ToolbarUnlinkButton>
				</dx:ToolbarUnlinkButton>
				<dx:ToolbarInsertImageDialogButton Visible="False">
				</dx:ToolbarInsertImageDialogButton>
				<dx:ToolbarTableOperationsDropDownButton BeginGroup="True">
					<Items>
						<dx:ToolbarInsertTableDialogButton BeginGroup="True">
						</dx:ToolbarInsertTableDialogButton>
						<dx:ToolbarTablePropertiesDialogButton BeginGroup="True">
						</dx:ToolbarTablePropertiesDialogButton>
						<dx:ToolbarTableRowPropertiesDialogButton>
						</dx:ToolbarTableRowPropertiesDialogButton>
						<dx:ToolbarTableColumnPropertiesDialogButton>
						</dx:ToolbarTableColumnPropertiesDialogButton>
						<dx:ToolbarTableCellPropertiesDialogButton>
						</dx:ToolbarTableCellPropertiesDialogButton>
						<dx:ToolbarInsertTableRowAboveButton BeginGroup="True">
						</dx:ToolbarInsertTableRowAboveButton>
						<dx:ToolbarInsertTableRowBelowButton>
						</dx:ToolbarInsertTableRowBelowButton>
						<dx:ToolbarInsertTableColumnToLeftButton>
						</dx:ToolbarInsertTableColumnToLeftButton>
						<dx:ToolbarInsertTableColumnToRightButton>
						</dx:ToolbarInsertTableColumnToRightButton>
						<dx:ToolbarSplitTableCellHorizontallyButton BeginGroup="True">
						</dx:ToolbarSplitTableCellHorizontallyButton>
						<dx:ToolbarSplitTableCellVerticallyButton>
						</dx:ToolbarSplitTableCellVerticallyButton>
						<dx:ToolbarMergeTableCellRightButton>
						</dx:ToolbarMergeTableCellRightButton>
						<dx:ToolbarMergeTableCellDownButton>
						</dx:ToolbarMergeTableCellDownButton>
						<dx:ToolbarDeleteTableButton BeginGroup="True">
						</dx:ToolbarDeleteTableButton>
						<dx:ToolbarDeleteTableRowButton>
						</dx:ToolbarDeleteTableRowButton>
						<dx:ToolbarDeleteTableColumnButton>
						</dx:ToolbarDeleteTableColumnButton>
					</Items>
				</dx:ToolbarTableOperationsDropDownButton>
				<dx:ToolbarFullscreenButton BeginGroup="True">
				</dx:ToolbarFullscreenButton>
			</Items>
		</dx:HtmlEditorToolbar>
		<dx:HtmlEditorToolbar Name="StandardToolbar2">
			<Items>
				<dx:ToolbarParagraphFormattingEdit Width="120px">
					<Items>
						<dx:ToolbarListEditItem Text="Normal" Value="p" />
						<dx:ToolbarListEditItem Text="Heading  1" Value="h1" />
						<dx:ToolbarListEditItem Text="Heading  2" Value="h2" />
						<dx:ToolbarListEditItem Text="Heading  3" Value="h3" />
						<dx:ToolbarListEditItem Text="Heading  4" Value="h4" />
						<dx:ToolbarListEditItem Text="Heading  5" Value="h5" />
						<dx:ToolbarListEditItem Text="Heading  6" Value="h6" />
						<dx:ToolbarListEditItem Text="Address" Value="address" />
						<dx:ToolbarListEditItem Text="Normal (DIV)" Value="div" />
					</Items>
				</dx:ToolbarParagraphFormattingEdit>
				<dx:ToolbarFontNameEdit>
					<Items>
						<dx:ToolbarListEditItem Text="Times New Roman" Value="Times New Roman" />
						<dx:ToolbarListEditItem Text="Tahoma" Value="Tahoma" />
						<dx:ToolbarListEditItem Text="Verdana" Value="Verdana" />
						<dx:ToolbarListEditItem Text="Arial" Value="Arial" />
						<dx:ToolbarListEditItem Text="MS Sans Serif" Value="MS Sans Serif" />
						<dx:ToolbarListEditItem Text="Courier" Value="Courier" />
					</Items>
				</dx:ToolbarFontNameEdit>
				<dx:ToolbarFontSizeEdit>
					<Items>
						<dx:ToolbarListEditItem Text="1 (8pt)" Value="1" />
						<dx:ToolbarListEditItem Text="2 (10pt)" Value="2" />
						<dx:ToolbarListEditItem Text="3 (12pt)" Value="3" />
						<dx:ToolbarListEditItem Text="4 (14pt)" Value="4" />
						<dx:ToolbarListEditItem Text="5 (18pt)" Value="5" />
						<dx:ToolbarListEditItem Text="6 (24pt)" Value="6" />
						<dx:ToolbarListEditItem Text="7 (36pt)" Value="7" />
					</Items>
				</dx:ToolbarFontSizeEdit>
				<dx:ToolbarBoldButton BeginGroup="True">
				</dx:ToolbarBoldButton>
				<dx:ToolbarItalicButton>
				</dx:ToolbarItalicButton>
				<dx:ToolbarUnderlineButton>
				</dx:ToolbarUnderlineButton>
				<dx:ToolbarStrikethroughButton>
				</dx:ToolbarStrikethroughButton>
				<dx:ToolbarJustifyLeftButton BeginGroup="True">
				</dx:ToolbarJustifyLeftButton>
				<dx:ToolbarJustifyCenterButton>
				</dx:ToolbarJustifyCenterButton>
				<dx:ToolbarJustifyRightButton>
				</dx:ToolbarJustifyRightButton>
				<dx:ToolbarJustifyFullButton>
				</dx:ToolbarJustifyFullButton>
				<dx:ToolbarBackColorButton BeginGroup="True">
				</dx:ToolbarBackColorButton>
				<dx:ToolbarFontColorButton>
				</dx:ToolbarFontColorButton>
			</Items>
		</dx:HtmlEditorToolbar>
	</Toolbars>
	<SettingsResize AllowResize="True" MaxWidth="775" MinHeight="350" MinWidth="775" />
</dx:ASPxHtmlEditor>
 </dx:ContentControl>
</ContentCollection>
</dx:TabPage>
									<dx:TabPage Text="Requirements">
										<ContentCollection>
											<dx:ContentControl runat="server">
												<dx:ASPxHtmlEditor runat="server" Html='<%# Eval("requirements") %>' Width="775px" Height="350px" ID="html_requirements" ClientInstanceName="html_requirements" EnableTheming="False">
													<Settings AllowInsertDirectImageUrls="False" />
													<SettingsImageUpload>
														<ValidationSettings AllowedFileExtensions=".jpe, .jpeg, .jpg, .gif, .png">
														</ValidationSettings>
													</SettingsImageUpload>
													<SettingsImageSelector>
														<CommonSettings AllowedFileExtensions=".jpe, .jpeg, .jpg, .gif, .png" />
													</SettingsImageSelector>
													<Toolbars>
														<dx:HtmlEditorToolbar Name="StandardToolbar1">
															<Items>
																<dx:ToolbarCutButton>
																</dx:ToolbarCutButton>
																<dx:ToolbarCopyButton>
																</dx:ToolbarCopyButton>
																<dx:ToolbarPasteButton>
																</dx:ToolbarPasteButton>
																<dx:ToolbarPasteFromWordButton>
																</dx:ToolbarPasteFromWordButton>
																<dx:ToolbarUndoButton BeginGroup="True">
																</dx:ToolbarUndoButton>
																<dx:ToolbarRedoButton>
																</dx:ToolbarRedoButton>
																<dx:ToolbarRemoveFormatButton BeginGroup="True">
																</dx:ToolbarRemoveFormatButton>
																<dx:ToolbarSuperscriptButton BeginGroup="True">
																</dx:ToolbarSuperscriptButton>
																<dx:ToolbarSubscriptButton>
																</dx:ToolbarSubscriptButton>
																<dx:ToolbarInsertOrderedListButton BeginGroup="True">
																</dx:ToolbarInsertOrderedListButton>
																<dx:ToolbarInsertUnorderedListButton>
																</dx:ToolbarInsertUnorderedListButton>
																<dx:ToolbarIndentButton BeginGroup="True">
																</dx:ToolbarIndentButton>
																<dx:ToolbarOutdentButton>
																</dx:ToolbarOutdentButton>
																<dx:ToolbarInsertLinkDialogButton BeginGroup="True">
																</dx:ToolbarInsertLinkDialogButton>
																<dx:ToolbarUnlinkButton>
																</dx:ToolbarUnlinkButton>
																<dx:ToolbarInsertImageDialogButton Visible="False">
																</dx:ToolbarInsertImageDialogButton>
																<dx:ToolbarTableOperationsDropDownButton BeginGroup="True">
																	<Items>
																		<dx:ToolbarInsertTableDialogButton BeginGroup="True">
																		</dx:ToolbarInsertTableDialogButton>
																		<dx:ToolbarTablePropertiesDialogButton BeginGroup="True">
																		</dx:ToolbarTablePropertiesDialogButton>
																		<dx:ToolbarTableRowPropertiesDialogButton>
																		</dx:ToolbarTableRowPropertiesDialogButton>
																		<dx:ToolbarTableColumnPropertiesDialogButton>
																		</dx:ToolbarTableColumnPropertiesDialogButton>
																		<dx:ToolbarTableCellPropertiesDialogButton>
																		</dx:ToolbarTableCellPropertiesDialogButton>
																		<dx:ToolbarInsertTableRowAboveButton BeginGroup="True">
																		</dx:ToolbarInsertTableRowAboveButton>
																		<dx:ToolbarInsertTableRowBelowButton>
																		</dx:ToolbarInsertTableRowBelowButton>
																		<dx:ToolbarInsertTableColumnToLeftButton>
																		</dx:ToolbarInsertTableColumnToLeftButton>
																		<dx:ToolbarInsertTableColumnToRightButton>
																		</dx:ToolbarInsertTableColumnToRightButton>
																		<dx:ToolbarSplitTableCellHorizontallyButton BeginGroup="True">
																		</dx:ToolbarSplitTableCellHorizontallyButton>
																		<dx:ToolbarSplitTableCellVerticallyButton>
																		</dx:ToolbarSplitTableCellVerticallyButton>
																		<dx:ToolbarMergeTableCellRightButton>
																		</dx:ToolbarMergeTableCellRightButton>
																		<dx:ToolbarMergeTableCellDownButton>
																		</dx:ToolbarMergeTableCellDownButton>
																		<dx:ToolbarDeleteTableButton BeginGroup="True">
																		</dx:ToolbarDeleteTableButton>
																		<dx:ToolbarDeleteTableRowButton>
																		</dx:ToolbarDeleteTableRowButton>
																		<dx:ToolbarDeleteTableColumnButton>
																		</dx:ToolbarDeleteTableColumnButton>
																	</Items>
																</dx:ToolbarTableOperationsDropDownButton>
																<dx:ToolbarFullscreenButton BeginGroup="True">
																</dx:ToolbarFullscreenButton>
															</Items>
														</dx:HtmlEditorToolbar>
														<dx:HtmlEditorToolbar Name="StandardToolbar2">
															<Items>
																<dx:ToolbarParagraphFormattingEdit Width="120px">
																	<Items>
																		<dx:ToolbarListEditItem Text="Normal" Value="p" />
																		<dx:ToolbarListEditItem Text="Heading  1" Value="h1" />
																		<dx:ToolbarListEditItem Text="Heading  2" Value="h2" />
																		<dx:ToolbarListEditItem Text="Heading  3" Value="h3" />
																		<dx:ToolbarListEditItem Text="Heading  4" Value="h4" />
																		<dx:ToolbarListEditItem Text="Heading  5" Value="h5" />
																		<dx:ToolbarListEditItem Text="Heading  6" Value="h6" />
																		<dx:ToolbarListEditItem Text="Address" Value="address" />
																		<dx:ToolbarListEditItem Text="Normal (DIV)" Value="div" />
																	</Items>
																</dx:ToolbarParagraphFormattingEdit>
																<dx:ToolbarFontNameEdit>
																	<Items>
																		<dx:ToolbarListEditItem Text="Times New Roman" Value="Times New Roman" />
																		<dx:ToolbarListEditItem Text="Tahoma" Value="Tahoma" />
																		<dx:ToolbarListEditItem Text="Verdana" Value="Verdana" />
																		<dx:ToolbarListEditItem Text="Arial" Value="Arial" />
																		<dx:ToolbarListEditItem Text="MS Sans Serif" Value="MS Sans Serif" />
																		<dx:ToolbarListEditItem Text="Courier" Value="Courier" />
																	</Items>
																</dx:ToolbarFontNameEdit>
																<dx:ToolbarFontSizeEdit>
																	<Items>
																		<dx:ToolbarListEditItem Text="1 (8pt)" Value="1" />
																		<dx:ToolbarListEditItem Text="2 (10pt)" Value="2" />
																		<dx:ToolbarListEditItem Text="3 (12pt)" Value="3" />
																		<dx:ToolbarListEditItem Text="4 (14pt)" Value="4" />
																		<dx:ToolbarListEditItem Text="5 (18pt)" Value="5" />
																		<dx:ToolbarListEditItem Text="6 (24pt)" Value="6" />
																		<dx:ToolbarListEditItem Text="7 (36pt)" Value="7" />
																	</Items>
																</dx:ToolbarFontSizeEdit>
																<dx:ToolbarBoldButton BeginGroup="True">
																</dx:ToolbarBoldButton>
																<dx:ToolbarItalicButton>
																</dx:ToolbarItalicButton>
																<dx:ToolbarUnderlineButton>
																</dx:ToolbarUnderlineButton>
																<dx:ToolbarStrikethroughButton>
																</dx:ToolbarStrikethroughButton>
																<dx:ToolbarJustifyLeftButton BeginGroup="True">
																</dx:ToolbarJustifyLeftButton>
																<dx:ToolbarJustifyCenterButton>
																</dx:ToolbarJustifyCenterButton>
																<dx:ToolbarJustifyRightButton>
																</dx:ToolbarJustifyRightButton>
																<dx:ToolbarJustifyFullButton>
																</dx:ToolbarJustifyFullButton>
																<dx:ToolbarBackColorButton BeginGroup="True">
																</dx:ToolbarBackColorButton>
																<dx:ToolbarFontColorButton>
																</dx:ToolbarFontColorButton>
															</Items>
														</dx:HtmlEditorToolbar>
													</Toolbars>
													<SettingsResize AllowResize="True" MaxWidth="775" MinHeight="350" MinWidth="775" />
												</dx:ASPxHtmlEditor>
											</dx:ContentControl>
										</ContentCollection>
									</dx:TabPage>
<dx:TabPage Text="Contact Info"><ContentCollection>
<dx:ContentControl runat="server"><dx:ASPxHtmlEditor runat="server" Html='<%# Eval("contactInfo") %>' Width="775px" Height="350px" ID="html_contactinfo" ClientInstanceName="html_contactinfo">
<Settings AllowInsertDirectImageUrls="False"></Settings>

<SettingsImageUpload>
<ValidationSettings AllowedFileExtensions=".jpe, .jpeg, .jpg, .gif, .png"></ValidationSettings>
</SettingsImageUpload>

<SettingsImageSelector>
<CommonSettings AllowedFileExtensions=".jpe, .jpeg, .jpg, .gif, .png"></CommonSettings>
</SettingsImageSelector>
	<Toolbars>
		<dx:HtmlEditorToolbar Name="StandardToolbar1">
			<Items>
				<dx:ToolbarCutButton>
				</dx:ToolbarCutButton>
				<dx:ToolbarCopyButton>
				</dx:ToolbarCopyButton>
				<dx:ToolbarPasteButton>
				</dx:ToolbarPasteButton>
				<dx:ToolbarPasteFromWordButton>
				</dx:ToolbarPasteFromWordButton>
				<dx:ToolbarUndoButton BeginGroup="True">
				</dx:ToolbarUndoButton>
				<dx:ToolbarRedoButton>
				</dx:ToolbarRedoButton>
				<dx:ToolbarRemoveFormatButton BeginGroup="True">
				</dx:ToolbarRemoveFormatButton>
				<dx:ToolbarSuperscriptButton BeginGroup="True">
				</dx:ToolbarSuperscriptButton>
				<dx:ToolbarSubscriptButton>
				</dx:ToolbarSubscriptButton>
				<dx:ToolbarInsertOrderedListButton BeginGroup="True">
				</dx:ToolbarInsertOrderedListButton>
				<dx:ToolbarInsertUnorderedListButton>
				</dx:ToolbarInsertUnorderedListButton>
				<dx:ToolbarIndentButton BeginGroup="True">
				</dx:ToolbarIndentButton>
				<dx:ToolbarOutdentButton>
				</dx:ToolbarOutdentButton>
				<dx:ToolbarInsertLinkDialogButton BeginGroup="True">
				</dx:ToolbarInsertLinkDialogButton>
				<dx:ToolbarUnlinkButton>
				</dx:ToolbarUnlinkButton>
				<dx:ToolbarInsertImageDialogButton Visible="False">
				</dx:ToolbarInsertImageDialogButton>
				<dx:ToolbarTableOperationsDropDownButton BeginGroup="True">
					<Items>
						<dx:ToolbarInsertTableDialogButton BeginGroup="True">
						</dx:ToolbarInsertTableDialogButton>
						<dx:ToolbarTablePropertiesDialogButton BeginGroup="True">
						</dx:ToolbarTablePropertiesDialogButton>
						<dx:ToolbarTableRowPropertiesDialogButton>
						</dx:ToolbarTableRowPropertiesDialogButton>
						<dx:ToolbarTableColumnPropertiesDialogButton>
						</dx:ToolbarTableColumnPropertiesDialogButton>
						<dx:ToolbarTableCellPropertiesDialogButton>
						</dx:ToolbarTableCellPropertiesDialogButton>
						<dx:ToolbarInsertTableRowAboveButton BeginGroup="True">
						</dx:ToolbarInsertTableRowAboveButton>
						<dx:ToolbarInsertTableRowBelowButton>
						</dx:ToolbarInsertTableRowBelowButton>
						<dx:ToolbarInsertTableColumnToLeftButton>
						</dx:ToolbarInsertTableColumnToLeftButton>
						<dx:ToolbarInsertTableColumnToRightButton>
						</dx:ToolbarInsertTableColumnToRightButton>
						<dx:ToolbarSplitTableCellHorizontallyButton BeginGroup="True">
						</dx:ToolbarSplitTableCellHorizontallyButton>
						<dx:ToolbarSplitTableCellVerticallyButton>
						</dx:ToolbarSplitTableCellVerticallyButton>
						<dx:ToolbarMergeTableCellRightButton>
						</dx:ToolbarMergeTableCellRightButton>
						<dx:ToolbarMergeTableCellDownButton>
						</dx:ToolbarMergeTableCellDownButton>
						<dx:ToolbarDeleteTableButton BeginGroup="True">
						</dx:ToolbarDeleteTableButton>
						<dx:ToolbarDeleteTableRowButton>
						</dx:ToolbarDeleteTableRowButton>
						<dx:ToolbarDeleteTableColumnButton>
						</dx:ToolbarDeleteTableColumnButton>
					</Items>
				</dx:ToolbarTableOperationsDropDownButton>
				<dx:ToolbarFullscreenButton BeginGroup="True">
				</dx:ToolbarFullscreenButton>
			</Items>
		</dx:HtmlEditorToolbar>
		<dx:HtmlEditorToolbar Name="StandardToolbar2">
			<Items>
				<dx:ToolbarParagraphFormattingEdit Width="120px">
					<Items>
						<dx:ToolbarListEditItem Text="Normal" Value="p" />
						<dx:ToolbarListEditItem Text="Heading  1" Value="h1" />
						<dx:ToolbarListEditItem Text="Heading  2" Value="h2" />
						<dx:ToolbarListEditItem Text="Heading  3" Value="h3" />
						<dx:ToolbarListEditItem Text="Heading  4" Value="h4" />
						<dx:ToolbarListEditItem Text="Heading  5" Value="h5" />
						<dx:ToolbarListEditItem Text="Heading  6" Value="h6" />
						<dx:ToolbarListEditItem Text="Address" Value="address" />
						<dx:ToolbarListEditItem Text="Normal (DIV)" Value="div" />
					</Items>
				</dx:ToolbarParagraphFormattingEdit>
				<dx:ToolbarFontNameEdit>
					<Items>
						<dx:ToolbarListEditItem Text="Times New Roman" Value="Times New Roman" />
						<dx:ToolbarListEditItem Text="Tahoma" Value="Tahoma" />
						<dx:ToolbarListEditItem Text="Verdana" Value="Verdana" />
						<dx:ToolbarListEditItem Text="Arial" Value="Arial" />
						<dx:ToolbarListEditItem Text="MS Sans Serif" Value="MS Sans Serif" />
						<dx:ToolbarListEditItem Text="Courier" Value="Courier" />
					</Items>
				</dx:ToolbarFontNameEdit>
				<dx:ToolbarFontSizeEdit>
					<Items>
						<dx:ToolbarListEditItem Text="1 (8pt)" Value="1" />
						<dx:ToolbarListEditItem Text="2 (10pt)" Value="2" />
						<dx:ToolbarListEditItem Text="3 (12pt)" Value="3" />
						<dx:ToolbarListEditItem Text="4 (14pt)" Value="4" />
						<dx:ToolbarListEditItem Text="5 (18pt)" Value="5" />
						<dx:ToolbarListEditItem Text="6 (24pt)" Value="6" />
						<dx:ToolbarListEditItem Text="7 (36pt)" Value="7" />
					</Items>
				</dx:ToolbarFontSizeEdit>
				<dx:ToolbarBoldButton BeginGroup="True">
				</dx:ToolbarBoldButton>
				<dx:ToolbarItalicButton>
				</dx:ToolbarItalicButton>
				<dx:ToolbarUnderlineButton>
				</dx:ToolbarUnderlineButton>
				<dx:ToolbarStrikethroughButton>
				</dx:ToolbarStrikethroughButton>
				<dx:ToolbarJustifyLeftButton BeginGroup="True">
				</dx:ToolbarJustifyLeftButton>
				<dx:ToolbarJustifyCenterButton>
				</dx:ToolbarJustifyCenterButton>
				<dx:ToolbarJustifyRightButton>
				</dx:ToolbarJustifyRightButton>
				<dx:ToolbarJustifyFullButton>
				</dx:ToolbarJustifyFullButton>
				<dx:ToolbarBackColorButton BeginGroup="True">
				</dx:ToolbarBackColorButton>
				<dx:ToolbarFontColorButton>
				</dx:ToolbarFontColorButton>
			</Items>
		</dx:HtmlEditorToolbar>
	</Toolbars>
	<SettingsResize AllowResize="True" MaxWidth="775" MinHeight="350" MinWidth="775" />
</dx:ASPxHtmlEditor>
</dx:ContentControl>
</ContentCollection>
</dx:TabPage>
</tabpages>
							</dx:ASPxPageControl></td>
					</tr>
					<tr>
						<td align="right">
							<dx:ASPxButton ID="bt_cancel" runat="server" Text="Cancel" AutoPostBack="False" Theme="NETheme01">
								<Image Url="~/images/icon/icon[undo].gif">
								</Image>
								<ClientSideEvents Click="function(s, e) {
	gv_jobs.CancelEdit();
}" />
							</dx:ASPxButton>
						</td>
						<td align="center" style="width:150px">
							<dx:ASPxButton ID="bt_save" runat="server" Text="Save" AutoPostBack="False" Theme="NETheme01">
								<Image Url="~/images/icon/icon[save].gif">
								</Image>
								<ClientSideEvents Click="function(s, e) {
	cbp_addedit.PerformCallback(&quot;asdf&quot;);
}" />
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
						</dx:PanelContent>
					</PanelCollection>
					<ClientSideEvents CallbackError="function(s, e) {
	inError = true;
}" EndCallback="function(s, e) {
	if(!inError)
		{
		doneEditing = true;
		gv_jobs.CancelEdit();
		}
}" BeginCallback="function(s, e) {
	if(inError)
		{
		inError = false;
		}
}" />
				</dx:ASPxCallbackPanel>
			</EditForm>
		</Templates>
		<Columns>
			<dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowEditButton="true" VisibleIndex="0">
				<CustomButtons>
					<dx:GridViewCommandColumnCustomButton ID="delete" Text="">
					</dx:GridViewCommandColumnCustomButton>
				</CustomButtons>
			</dx:GridViewCommandColumn>

			<dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" VisibleIndex="0"
				Width="25px">
				<EditFormSettings Visible="True" />
				<HeaderStyle HorizontalAlign="Center" />
				<CellStyle CssClass="row_id" HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataCheckColumn Caption="Active" FieldName="active" VisibleIndex="1">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_active" runat="server" Checked='<%# Bind("active") %>' CssClass="active" Theme="NETheme01">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataDateColumn Caption="Expiration" FieldName="expirationDate" VisibleIndex="2">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="Title" FieldName="title" VisibleIndex="3">
				<CellStyle Font-Bold="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Order" FieldName="displayOrder" VisibleIndex="4">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="description" Visible="False" VisibleIndex="5">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="requirements" Visible="False" VisibleIndex="6">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="contactInfo" Visible="False" VisibleIndex="7">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataCheckColumn Caption="Oakville" FieldName="locationOakville" VisibleIndex="8">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_oakville" Theme="NETheme01" runat="server" Checked='<%# Bind("locationoakville") %>' CssClass="locationoakville">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataCheckColumn Caption="Cambridge" FieldName="locationCambridge" VisibleIndex="9">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_cambridge"  Theme="NETheme01" runat="server" Checked='<%# Bind("locationcambridge") %>' CssClass="locationcambridge">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataCheckColumn Caption="Vaughan" FieldName="locationVaughan" VisibleIndex="10">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_vaughan" runat="server" Theme="NETheme01" Checked='<%# Bind("locationvaughan") %>' CssClass="locationvaughan">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataCheckColumn Caption="St. Creek" FieldName="locationStoney" VisibleIndex="11">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_stoney" runat="server" Theme="NETheme01" Checked='<%# Bind("locationstoney") %>' CssClass="locationstoney">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataCheckColumn Caption="Plymouth" FieldName="locationPlymouth" VisibleIndex="12">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_plymouth" runat="server" Theme="NETheme01" Checked='<%# Bind("locationplymouth") %>' CssClass="locationplymouth">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataCheckColumn Caption="St. Heights" FieldName="locationSterling" VisibleIndex="13">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_sterling" runat="server" Theme="NETheme01" Checked='<%# Bind("locationsterling") %>' CssClass="locationsterling">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataCheckColumn Caption="Charlotte" FieldName="locationCharlotte" VisibleIndex="14">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_charlotte" runat="server" Theme="NETheme01" Checked='<%# Bind("locationcharlotte") %>' CssClass="locationcharlotte">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataCheckColumn Caption="Barrie" FieldName="locationbarrie" VisibleIndex="16">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_barrie" runat="server" Theme="NETheme01" Checked='<%# Bind("locationbarrie") %>' CssClass="locationbarrie">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataCheckColumn Caption="Brampton" FieldName="locationBrampton" VisibleIndex="17">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_brampton" runat="server" Theme="NETheme01" Checked='<%# Bind("locationbrampton") %>' CssClass="locationbrampton">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataCheckColumn Caption="Whitby" FieldName="locationwhitby" VisibleIndex="18">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_whitby" runat="server" Theme="NETheme01" Checked='<%# Bind("locationwhitby") %>' CssClass="locationwhitby">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>          
			<dx:GridViewDataCheckColumn Caption="Mississauga" FieldName="locationMississauga" VisibleIndex="19">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_mississauga" runat="server" Theme="NETheme01" Checked='<%# Bind("locationmississauga") %>' CssClass="locationmississauga">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataCheckColumn Caption="Fresno" FieldName="locationfresno" VisibleIndex="19">
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="cb_fresno" runat="server" Theme="NETheme01" Checked='<%# Bind("locationfresno") %>' CssClass="locationfresno">
						<ClientSideEvents CheckedChanged="check_handler" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
		    <dx:GridViewDataCheckColumn Caption="Belleville" FieldName="locationbelleville" VisibleIndex="20">
		        <DataItemTemplate>
		            <dx:ASPxCheckBox ID="cb_belleville" runat="server" Theme="NETheme01" Checked='<%# Bind("locationbelleville") %>' CssClass="locationbelleville">
		                <ClientSideEvents CheckedChanged="check_handler" />
		            </dx:ASPxCheckBox>
		        </DataItemTemplate>
		    </dx:GridViewDataCheckColumn>
		</Columns>
		<Styles>
			<Header Font-Bold="False">
			</Header>
			<Cell>
				<Border BorderColor="#CCCCCC" />
			</Cell>
		</Styles>

         <SettingsPopup EditForm-Height="570px" EditForm-HorizontalAlign="WindowCenter"
			EditForm-Modal="True" EditForm-VerticalAlign="WindowCenter" EditForm-Width="730px" 
			EditForm-ShowHeader="true" EditForm-VerticalOffset="0">

<EditForm HorizontalAlign="WindowCenter" Width="730px" Height="570px" VerticalAlign="WindowCenter" VerticalOffset="0" Modal="True"></EditForm>
		</SettingsPopup>

		<ClientSideEvents EndCallback="function(s, e) {
	if(doneEditing)
		{
		location.href = location.href;
		}
}" />
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="ds_jobs" runat="server" ConnectionString="<%$ ConnectionStrings:GoDaddy %>" ProviderName="<%$ ConnectionStrings:GoDaddy.ProviderName %>" 
        SelectCommand="SELECT * FROM wp_newelec_jobs ORDER BY displayorder"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="sub_body" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="left" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>