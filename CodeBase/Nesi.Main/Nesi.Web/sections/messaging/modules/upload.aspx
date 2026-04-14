<%@ Page Title="" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_messaging_upload" Codebehind="upload.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header_placeholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<div style="padding:10px;">
	<dx:ASPxComboBox ID="combo_category" runat="server" ValueType="System.Int32" ClientInstanceName="combo_category" Width="100%" DataSourceID="ds_categories" TextField="name" ValueField="id">
		<ClientSideEvents EndCallback="function(s, e) {
	s.SetSelectedIndex(-1);
}" />
	</dx:ASPxComboBox>
<asp:SqlDataSource ID="ds_categories" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, name FROM video_category"></asp:SqlDataSource>
			<br />
			<b style="color:#f00;" runat="server" id="error"></b>
		<dx:ASPxMemo ID="memo_description" runat="server" ClientInstanceName="memo_description" Height="150px" Width="100%">
		</dx:ASPxMemo>
		<br />
			<br />
			<dx:ASPxUploadControl ID="uc_response_newhire" runat="server" Width="280px" ShowUploadButton="True" ClientInstanceName="client_upload" onfileuploadcomplete="upload_handler" ShowProgressPanel="True" FileUploadMode="OnPageLoad" ClientSideEvents-FileUploadComplete="function(s,e){parent.thisRefresh();}" Font-Bold="True" Font-Size="13px">
										<ValidationSettings AllowedFileExtensions=".flv, .f4v, .mp4" MultiSelectionErrorText="Attention! 
The following {0} files are invalid because they exceed the allowed file size ({1}) or their extensions are not allowed.
{2}">
										</ValidationSettings>

				<ClientSideEvents FileUploadComplete="function(s, e) {
	if(e.isValid &amp;&amp; window.opener)
		{
		combo_category.SetSelectedIndex(-1);
		memo_description.SetText('');
		window.opener.window.gv_videos.Refresh();
		combo_category.Focus();
		}
}" />
                
                <UploadButton>
					<Image Url="~/images/icon/Icon[save-green].gif">
					</Image>
				</UploadButton>
				
				<ButtonStyle ForeColor="#009933" ImageSpacing="5px">
				</ButtonStyle>

			</dx:ASPxUploadControl>
	</div>
</asp:Content>

