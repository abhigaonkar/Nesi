<%@ Page Language="C#" AutoEventWireup="true" Inherits="FileManager" Title="File Manager" CodeBehind="FileManager.aspx.cs" MasterPageFile	= '~/IntraDefault.master' %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
		<dx:ASPxFileManager ID="fm" runat="server" Width="100%" Theme="MaterialCompact">
			<SettingsToolbar ShowDownloadButton="True" />
			<Settings RootFolder="~\App_Data\" ThumbnailFolder="~\images\thumbs" EnableMultiSelect="true" />
			<SettingsFileList>
				<DetailsViewSettings>
					<Columns>
						<dx:FileManagerDetailsColumn FileInfoType="Thumbnail" Caption=" " Width="30px" VisibleIndex="0"></dx:FileManagerDetailsColumn>
						<dx:FileManagerDetailsColumn Caption="Name" Width="100%" VisibleIndex="1"></dx:FileManagerDetailsColumn>
						<dx:FileManagerDetailsColumn FileInfoType="LastWriteTime" Caption="Date modified" Width="80px" VisibleIndex="2"></dx:FileManagerDetailsColumn>
						<dx:FileManagerDetailsColumn FileInfoType="Size" Caption="Size" Width="70px" VisibleIndex="3"></dx:FileManagerDetailsColumn>
					</Columns>
				</DetailsViewSettings>
			</SettingsFileList>
			<SettingsEditing AllowCreate="True" AllowDelete="True" AllowMove="True" AllowDownload="true" AllowRename="True" TemporaryFolder="~\App_Data\UploadTemp\" />
			<SettingsUpload>
				<AdvancedModeSettings EnableMultiSelect="True" />
			</SettingsUpload>
		</dx:ASPxFileManager>
</ASP:CONTENT>