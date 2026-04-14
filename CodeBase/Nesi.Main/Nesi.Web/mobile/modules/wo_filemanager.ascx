<%@ Control Language="C#" AutoEventWireup="true" Inherits="mobile_modules_wo_filemanager" Codebehind="wo_filemanager.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<style type="text/css">
	.folders
		{

		}
	.folders .working_folder
		{
		margin-bottom: 10px;
		}
	.folders .sub_folder
		{
		text-align: left;
		}
	.files
		{
		margin-top:		5px;
		margin-bottom:	5px;
		}
	.files .file
		{
		text-align: left;
		}
	.fbutton
		{
		width: 100% !important;
		text-align: left;
		margin-bottom: 3px;
		height: 25px;
		padding-left: 5px;
		}
</style>
<dx:aspxcallbackpanel id="cbp_fm" clientinstancename="cbp_fm" runat="server" width="100%" theme="NETheme01" oncallback="cbp_fm_OnCallback">
	<panelcollection>
		<dx:panelcontent runat="server">
			<div id="folders" runat="server" class="folders" align="center"></div>
			<br />
			<div id="files" runat="server" class="files"></div>
			<dx:aspxuploadcontrol id="up_file" clientinstancename="up_file" runat="server" width="100%" onfileuploadcomplete="up_file_FileUploadComplete">
				
<AdvancedModeSettings>
<FileListItemStyle CssClass="pending dxucFileListItem"></FileListItemStyle>
</AdvancedModeSettings>
				<clientsideevents filesuploadcomplete="function(_s,_e)
					{
					var base_path			= $('.working_folder').attr('data-path');
					window.cbp_fm.PerformCallback('refresh|'+base_path);
					}" fileinputcountchanged="
				function(_s,_e)
					{
					if(_s.GetText() != '')
						{
						$('.upload').removeAttr('disabled');
						}
					else
						{
						$('.upload').attr('disabled', true);
						}
					}" />
			</dx:aspxuploadcontrol>
			<br />
			<button type="button" class="fbutton upload  whitetext" onclick="fm.upload.start();" style="text-align:center">Upload File</button>
			<iframe width="1" height="1" id="dl_frame" src="about:blank" frameborder="0"></iframe>
		</dx:panelcontent>
	</panelcollection>
</dx:aspxcallbackpanel>
<br />
<br />
<br />
<br />
