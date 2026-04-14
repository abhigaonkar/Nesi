<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_hr_fvr_if_settings" MasterPageFile="/nonframe.master" Codebehind="if_settings.aspx.cs" %>

<%@ Register Src="~/sections/hr/fvr/modules/tax_entity_fvr_settings.ascx" TagPrefix="uc" TagName="business_unit_fvr_settings" %>
<asp:Content runat="server" ContentPlaceHolderID="header_placeholder">
	<script type="text/javascript" src="/js/fvr_manage.js"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphMasterBody">
	<uc:business_unit_fvr_settings runat="server" ID="business_unit_fvr_settings" />
</asp:Content>