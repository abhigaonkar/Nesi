<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_member_modules_uc_responsibility" Codebehind="uc_responsibility.ascx.cs" %>
<div style="width: 700px;margin-bottom:2px;">
	<div id="c_title" runat="server" style="font-weight: bold">
		<%= Title %></div>
	<asp:DropDownList ID="ddl_responsibility" runat="server" CssClass="assign_ddl" DataTextField="text" DataValueField="id" Width="350px">
	</asp:DropDownList>
	<asp:DropDownList ID="ddl_assignto" runat="server" DataTextField="text" DataValueField="id" Width="250px">
	</asp:DropDownList>
	<asp:ImageButton ID="bt_send" runat="server" ImageUrl="~/images/icon/icon[send].gif" OnClick="bt_send_Click" ToolTip="Change this one responsibility" />
	<asp:ImageButton ID="bt_mass_send" runat="server" ImageUrl="~/images/icon/icon[fastforward].gif" Visible="false" OnClientClick="javascript:return confirm('Are you sure you want to assign ALL of these items to the selected user?');" OnClick="bt_mass_send_Click" ToolTip="Assign ALL responsibilities on the left to the selected person on the right" />
	<asp:ImageButton ID="bt_cancel" runat="server" ImageUrl="~/images/icon/icon[delete].gif" OnClick="bt_cancel_Click" ToolTip="CANCEL" />
	<span id="lb_notification" runat="server" style="padding-left: 10px;"></span>
</div>
