<%@ Page Language="C#"MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_member_scheduler_index4" Title="On Call Schedule" Codebehind="index4.aspx.cs" %>
<%@ Register src="oncall.ascx" tagname="oncall" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	
</asp:Content>




<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
   
	
	
   

    <uc1:oncall ID="oncall1" runat="server" />
   
	
	
   

    </asp:Content>