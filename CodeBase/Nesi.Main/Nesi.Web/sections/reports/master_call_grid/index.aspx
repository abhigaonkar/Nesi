<%@ 
Page Language="C#" 
MasterPageFile	= '../../../IntraDefault.master'
AutoEventWireup="true" 
Inherits="sections_reports_phone_comments_index" 
Title			= 'Master Phone call Grid' 
 Codebehind="index.aspx.cs" %>

<%@ Register Src="~/modules/phoneComments.ascx" TagPrefix="uc1" TagName="phoneComments" %>

<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				<div id='divSide' runat='server'>
				</div>
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
				<div id='divMenu' runat='server'></div>
    
</asp:Content>

<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' Runat='Server' >
    <uc1:phoneComments runat="server" ID="phoneComments" forTimeSheet="False" />
</asp:Content>
<asp:Content ID="Content4" runat="Server" 
	contentplaceholderid="header_placeholder">
    
</asp:Content>