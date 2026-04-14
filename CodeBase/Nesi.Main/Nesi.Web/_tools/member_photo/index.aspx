<%@ Page Title="" Language="C#" Theme=""  MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="_tools_member_photo_index" Codebehind="index.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" runat="server" contentplaceholderid="cphMasterBody">
	<div width="100%" align="center" style="width: 100%">
	<dx:ASPxUploadControl ID="uc_image" runat="server" Width="280px" onfileuploadcomplete="uc_image_FileUploadComplete" ShowUploadButton="True">
		<ValidationSettings MaxFileSize="100000">
        </ValidationSettings>
		<ClientSideEvents FileUploadComplete="function(s, e) {
	parent.toggle_picture_change(null);
}" />
	</dx:ASPxUploadControl></div>
</asp:Content>

