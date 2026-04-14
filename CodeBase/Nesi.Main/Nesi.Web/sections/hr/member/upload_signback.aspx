<%@ Page Title="" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_member_upload_signback" Codebehind="upload_signback.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header_placeholder" Runat="Server">
	</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
			<table style="width:300px; font-family: Arial; font-size: 11px;">
				<tr valign="top">
					<td>
						<dx:ASPxButtonEdit ID="txt_file" runat="server" AutoPostBack="True" 
							Caption="*You can only have one signed upload, the last upload will overwrite the original." 
							ClientVisible="False" onbuttonclick="ASPxButtonEdit1_ButtonClick" Width="100%">
							<ClientSideEvents ButtonClick="function(s, e) {
	if (e.buttonIndex==1)
{
	if (confirm('Are you sure you want to delete this file?'))
	{
e.processOnServer=true;
}
	else
	{
e.processOnServer=false;

}
	
}
}" />
							<Buttons>
								<dx:EditButton Text="View">
								</dx:EditButton>
								<dx:EditButton>
									<Image Url="~/images/icon/icon[delete].gif">
									</Image>
								</dx:EditButton>
							</Buttons>
							<CaptionSettings Position="Top" />
							<CaptionStyle Font-Size="10px">
							</CaptionStyle>
						</dx:ASPxButtonEdit>
						<br />
			<dx:ASPxUploadControl ID="uc_response_newhire" runat="server" Width="100%" 
							ShowUploadButton="True" onfileuploadcomplete="upload_handler" 
							ShowProgressPanel="True" FileUploadMode="OnPageLoad" 
							ClientSideEvents-FileUploadComplete="function(s,e){parent.thisRefresh();}" 
							Font-Bold="False" Font-Size="9pt" Font-Names="Arial" 
							NullText="Uploads must be PDF" ToolTip="Uploads must be PDF">
<ClientSideEvents FileUploadComplete="function(s,e){parent.__doPostBack('bt_refresh', '');}"></ClientSideEvents>
										<ValidationSettings AllowedFileExtensions=".pdf, .flv, .f4v" MultiSelectionErrorText="Attention! 

The following {0} files are invalid because they exceed the allowed file size ({1}) or their extensions are not allowed.

{2}">
										</ValidationSettings>
					<UploadButton>
						<Image Url="~/images/icon/icon[save-green].gif" />
					</UploadButton>
				
				<ButtonStyle ForeColor="#009933" ImageSpacing="5px">
				</ButtonStyle>

			</dx:ASPxUploadControl>
					</td>
					<td></br>

									<asp:Panel runat="server" Font-Names="Arial" Font-Size="12px" 
							ID="pnl_showfile">
	<div runat="server" ID="pnl_showfile_html"></div>
</asp:Panel>
									</td>
				</tr>
			</table>
</asp:Content>

