<%@ Page Title="" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_hr_fvr_modules_upload" Codebehind="upload.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<link href="/App_Themes/mobile/Default.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="/js/fvr.js"></script>
	<br/>
	<br/>
		<div style="padding:10px;">
			You must upload a scan of the filled out document before you can proceed.
			<dx:ASPxUploadControl ID="uc_response_newhire" runat="server" Width="220px" ShowUploadButton="True" ClientInstanceName="client_upload" onfileuploadcomplete="upload_handler" ShowProgressPanel="True" FileUploadMode="OnPageLoad" ClientSideEvents-FileUploadComplete="function(s,e){parent.thisRefresh();}" Font-Bold="True" Font-Size="13px">
<ClientSideEvents FileUploadComplete="function(s,e){if(e.errorText == ''){parent.__doPostBack('bt_refresh', '');}}"></ClientSideEvents>
										<ValidationSettings AllowedFileExtensions=".pdf, .flv, .f4v" MultiSelectionErrorText="Attention! 

The following {0} files are invalid because they exceed the allowed file size ({1}) or their extensions are not allowed.

{2}">
										</ValidationSettings>

				
				<ButtonStyle ForeColor="#009933" ImageSpacing="5px">
				</ButtonStyle>

			</dx:ASPxUploadControl>
			<div style="color:red;font-size:11px; font-family: Arial, Helvetica, sans-serif;">
				<br />
			<button type="button" class="whitetext aligncenter" id="btn_upload_instructions" runat="server">Upload Instructions</button>
			<div id="upload_instructions" style="display: none;">
				<ul style="padding-left:0px;margin-left:0px;">
					<li>- Uploads must be a PDF.</li>
					<li>- New USERS must DOWNLOAD, PRINT and UPLOAD it back into the FVR.</li>
					<li>- You can either:
						<ul>
							<li>A: Download the app "Adobe Fill & Sign" and fill out the PDF directly from your <a href='https://play.google.com/store/apps/details?id=com.adobe.fas&hl=en' target="blank">Android </a>or <a href='https://itunes.apple.com/us/app/adobe-fill-sign-easy-pdf-form-filler/id950099951?mt=8' target="blank">Iphone</a>. </li>
							<li>B: Download, print, sign and upload from home, where you have full control of the doc and security. </li>
							<li>C:
								<ol>
								<li>Download, print and sign at the branch.</li>
								<li>Then from the scanner in the branch scan to their personal email address manually.</li>
								<li>Next, on the timesheet machine, open your email, download the scan to desktop, upload to nesi and then delete the file from the desktop</li>
								</ol></li>
						</ul>
					</li>
				</ul>
				<a name="instruction_bottom"/>
			</div>
			</div>
	</div>
</asp:Content>

