<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="wo_prog_search" Title="Work Order Search" Codebehind="wo_prog_search.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="header_" ContentPlaceHolderID="header_placeholder" runat="Server">
	<link href="/css/wo_search.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<asp:Panel ID="panSearch" runat="server" DefaultButton="btnSearch">
    <table border="0" cellpadding="2" cellspacing="0" style="width: 600px">
        <tr>
            <td colspan="4" style="height: 18px">Search for a Work Order by WO No/WoProgID or Customer Name</td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlCompany" runat="server"/>
            </td>  
            <td>
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" />
            </td>
            <td>    
                <asp:Label ID="lblHisthk" runat="server" Text="Show Work Order History"></asp:Label>
             </td>
             <td>   
                <asp:CheckBox ID="chkHistory" runat="server" />
            </td>
        </tr>
        <tr>
         <td> 
          <asp:TextBox ID="txtSearch" runat="server" CausesValidation="True"></asp:TextBox>
         </td>
         <td colspan="3">
            <asp:Label ID="Label1" runat="server" Text="Enter Work Order #"></asp:Label>
        </td>
        </tr>
        <tr> 
            <td>   
                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
            </td>
            <td colspan="3">
            <asp:Label ID="lblWoInst2" runat="server" Text="or Enter Part Description for search of invoiced Work Orders"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
    <p></p>
    <div id="divResults" runat="server">
    </div>
</asp:Content>
