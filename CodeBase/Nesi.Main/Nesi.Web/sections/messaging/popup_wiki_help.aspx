<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_messaging_popup_wiki_help" Codebehind="popup_wiki_help.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Nesi.ca Wiki Help</title>
		<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
		  <script type="text/javascript">

		  	function OnCustomCommand(htmlEditor, e) 
			{
		  		
	//			if (htmlEditor.InCallback())
	//	  			return;
	//	  		switch (e.commandName) {
	//	  			case "load":
	//	  				htmlEditor.PerformDataCallback(e.commandName, LoadContent);
	//	  				SetStatusText("Loading…");
	//	  				break;
	//	  			case "save":
		  				htmlEditor.PerformDataCallback(e.commandName, SaveCompleted);
		  				SetStatusText("Saving…");
	//	  				break;
//		  		}

		  	}
		  	function SetStatusText(text) {
		  		lbStatusText.SetText(text);
		  	}
		  	function LoadContent(htmlEditor, result) {
		  		if (result != null)
		  			htmlEditor.SetHtml(result);
		  		SetStatusText("");
		  	}
		  	function SaveCompleted() {
		  		SetStatusText("");
		  	}
    </script>
	</head>
	<body>
		<form id="form1" runat="server">
		<div style="height: 45px;">
		<table width="100%"><tr><td><dx:aspxlabel ID="ASPxLabel2" runat="server" 
				Text="Help File" Font-Size="14pt" Font-Names="Arial" Font-Bold="True" /></td></tr><tr><td>
				<dx:aspxlabel ID="ASPxLabel1" runat="server" 
					ClientInstanceName="lbStatusText" /></td></tr>
                
					
					</table>
            </div>
			<div>
				<table width="100%" cellpadding="5" cellspacing="0">
					<tr>
						<td>
							
							<dx:ASPxHtmlEditor ID="htmlEditor" runat="server" 
								ClientInstanceName="htmlEditor" Height="550px" Width="100%" 
								oncustomdatacallback="htmlEditor_CustomDataCallback">
								<ClientSideEvents CustomCommand="function(s, e) {
	OnCustomCommand(s,e);

}" />
								<Toolbars>
									<dx:HtmlEditorToolbar>
									</dx:HtmlEditorToolbar>
									<dx:HtmlEditorToolbar Name="StandardToolbar1">
										<Items>
											<dx:CustomToolbarButton CommandName="save" Text="Save">
												<Image Url="~/images/icon/icon[save].gif">
												</Image>
											</dx:CustomToolbarButton>
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
											<dx:ToolbarInsertImageDialogButton>
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
											<dx:ToolbarBackColorButton BeginGroup="True">
											</dx:ToolbarBackColorButton>
											<dx:ToolbarFontColorButton>
											</dx:ToolbarFontColorButton>
										</Items>
									</dx:HtmlEditorToolbar>
								</Toolbars>
								
								
								<Settings AllowHtmlView="False" />
								
								
							</dx:ASPxHtmlEditor>
							
						</td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</html>
