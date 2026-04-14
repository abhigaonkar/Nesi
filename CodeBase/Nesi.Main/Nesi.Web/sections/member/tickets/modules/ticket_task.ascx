<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_tickets_modules_ticket_task" Codebehind="ticket_task.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<div class="task">
	<div class="delete" data-id="<%# this.id %>">
		<dx:ASPxImage ID="img_delete" runat="server" ImageUrl="/images/icon/icon[delete].gif" Width="16px" Cursor="pointer" ClientEnabled="<%# this.enabled %>" Height="16px">
			<ClientSideEvents Click="local_ticket.task.do_delete" />
		</dx:ASPxImage>
	</div>
	<div class="tk_body" data-id="<%# this.id %>">
		<dx:ASPxMemo Text='<%# this.body %>' runat="server" ID="memo_body" Width="100%" Height="50px" ClientEnabled="<%# this.enabled %>">
			<ClientSideEvents TextChanged="local_ticket.task.edit_text" />
		</dx:ASPxMemo>
		<div class="spacer"></div>
		<dx:ASPxComboBox runat="server" ID="combo_task_assign" ClientEnabled="<%# this.enabled %>" AutoPostBack="false" CssClass="assign" NullText="Select Assignee" ValueField="Key" ValueType="System.Int32" Value="<%# this.assignee %>" TextField="Value">
			<ClientSideEvents SelectedIndexChanged="local_ticket.task.setassignee" />
		</dx:ASPxComboBox>
	</div>
	<div class="done" align="center">
		<input type="checkbox" runat="server" id="chk_task_completed" data-id="<%# this.id %>" title="Click to toggle the complete status of this task" onclick="local_ticket.task.complete(this)" checked="<%# this.complete %>" />
	</div>
</div>