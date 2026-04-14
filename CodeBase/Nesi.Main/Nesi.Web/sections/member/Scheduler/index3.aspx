<%@ Page Language="C#" MasterPageFile="~/nonframe.master" AutoEventWireup="true" Inherits="sections_member_scheduler_default3" Title="Work Order Planner" Codebehind="index3.aspx.cs" %>

<%@ Register src="inlineappform.ascx" tagname="inlineappform" tagprefix="uc1" %>


   
<%@ Register src="hourview.ascx" tagname="hourview" tagprefix="uc2" %>


   
<%@ Register src="woplanner.ascx" tagname="woplanner" tagprefix="uc3" %>



<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
   
	
    
   

    <uc3:woplanner ID="woplanner1" runat="server" />
   
	
    
   

   </asp:Content>