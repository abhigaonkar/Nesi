<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_member_scheduler_default" Title="Scheduler" Codebehind="index1.aspx.cs" %>

<%@ Register src="inlineappform.ascx" tagname="inlineappform" tagprefix="uc1" %>


   
<%@ Register src="dayview.ascx" tagname="dayview" tagprefix="uc2" %>


   
<%@ Register src="single_member_view.ascx" tagname="single_member_view" tagprefix="uc3" %>


   
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
   
    <table style="width:100%;">
		
		<tr>
			<td>
   
    <uc2:dayview ID="dayview1" runat="server" />
   
   		 </td>
		</tr>
		<tr>
			<td>
				<uc3:single_member_view ID="single_member_view1" runat="server" /></td>
		</tr>
	</table>
	<br />
   
   </asp:Content>