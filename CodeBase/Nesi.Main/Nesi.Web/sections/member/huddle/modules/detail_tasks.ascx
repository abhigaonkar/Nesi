<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_huddle_modules_detail_tasks" EnableTheming="True" Codebehind="detail_tasks.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="task.ascx" tagname="task" tagprefix="uc" %>
<dx:ASPxCallbackPanel id="cbp_addtask" ClientInstanceName="cbp_addtask" runat="server" oncallback="cbp_addtask_Callback">
	<ClientSideEvents EndCallback="function(s, e) {
	cbp_tasks.PerformCallback();
}" />
	<PanelCollection>
		<dx:PanelContent>
<div style="border:solid 1px #ccc;margin:2px;padding:2px;border-radius:5px;background-color:#cfc;" >
	<div style="float:left;width:16%;margin-left:2px;padding:2px;">
		<div><b>Assigned To</b></div>
		<div>
			<dx:ASPxComboBox ID="add_task_assigned_to" runat="server" Width="100%" 
				ValueField="id" TextField="name" ValueType="System.Int32" Theme="NETheme01">
				<ClientSideEvents SelectedIndexChanged="huddle.existing_huddle.task.handle_assigned_to" />
			</dx:ASPxComboBox>
		</div>
	</div>
	<div style="float:left;width:10%;margin-left:2px;padding:2px;">
		<div><b>Source</b></div>
		<div>
			<dx:ASPxComboBox ID="add_task_source" NullText="Select Source" ValueField="id" 
				TextField="name" runat="server" Width="100%" ValueType="System.Int32" 
				Theme="NETheme01">
				<ClientSideEvents SelectedIndexChanged="huddle.existing_huddle.task.handle_source" />
			</dx:ASPxComboBox>
		</div>
	</div>
	<div style="float:left;width:20%;margin-left:2px;padding:2px;" id="div_ticket" runat="server" visible="false">
		<div><b>Ticket</b></div>
		<div>
			<dx:ASPxComboBox ID="add_task_ticket" NullText="Select Ticket" ValueField="id" 
				TextField="name" runat="server" Width="100%" ValueType="System.Int32" 
				Theme="NETheme01">
				<Columns>
					<dx:ListBoxColumn Caption="#" FieldName="id" Width="50px" />
					<dx:ListBoxColumn Caption="Issue" FieldName="name" Width="300px" />
					<dx:ListBoxColumn Caption="Priority" FieldName="priority" Width="75px" />
					<dx:ListBoxColumn Caption="Status" FieldName="status" Width="100px" />
					<dx:ListBoxColumn Caption="Commited" FieldName="committed" />
				</Columns>
				<ClientSideEvents SelectedIndexChanged="huddle.existing_huddle.task.handle_ticket" />
			</dx:ASPxComboBox>
		</div>
	</div>
	<div style="float:left;width:20%;margin-left:2px;padding:2px;" id="div_text" runat="server" visible="false">
		<div><b>Text</b></div>
		<div>
			<dx:ASPxTextBox ID="add_task_text" NullText="Task Text" runat="server" 
				Width="100%" Theme="NETheme01">
			</dx:ASPxTextBox>
		</div>
	</div>
	<div style="float:left;width:10%;margin-left:2px;padding:2px;">
		<div><b>Status</b></div>
		<div>
			<dx:ASPxComboBox ID="add_task_status" runat="server" Width="100%" 
				ValueType="System.Int32" Theme="NETheme01">
				<Items>
					<dx:ListEditItem Value="0" Text="Not Started" Selected="true" />
					<dx:ListEditItem Value="1" Text="In Progress" Selected="false" />
					<dx:ListEditItem Value="2" Text="Completed" Selected="false" />
				</Items>
			</dx:ASPxComboBox>
		</div>
	</div>
	<div style="float:left;width:16%;margin-left:2px;padding:2px;">
		<div><b>Date Due</b></div>
		<div>
			<dx:ASPxDateEdit ID="add_task_date_due" runat="server" AllowUserInput="false" 
				Width="100%" Theme="NETheme01">
			</dx:ASPxDateEdit>
		</div>
	</div>
	<div style="float:left;width:16%;margin-left:2px;padding:2px;">
		<div style="height:10px;">&nbsp;</div>
		<div>
			<dx:ASPxButton ID="add_task_save" runat="server" Width="50px" 
				AutoPostBack="false" Height="100%" Text="Save" Theme="NETheme01">
				<Image Url="/images/icon/icon[save].gif" Width="16px" Height="16px"></Image>
				<ClientSideEvents Click="huddle.existing_huddle.task.save" />
			</dx:ASPxButton>
		</div>
	</div>
	<div style="clear:both;padding:2px;" id="div_link" runat="server">
		<div style="padding:2px;"><b>Link</b></div>
		<div style="padding:2px;">
			<dx:ASPxTextBox ID="add_task_link" NullText="Task Link" runat="server" 
				Width="100%" ClientInstanceName="add_task_link" Theme="NETheme01">
			</dx:ASPxTextBox>
		</div>
	</div>
	<div style="clear:both;padding:2px;">
		<div style="padding:2px;cursor:pointer;"><b>Description <i style="font-size:9px;">(optional)</i></b></div>
		<div style="padding:2px;" id="add_task_description_div">
			<dx:ASPxMemo ID="add_task_description" 
				ClientInstanceName="add_task_description" runat="server" Height="71px" 
				Width="100%" Theme="NETheme01">
			</dx:ASPxMemo>
		</div>
	</div>
</div>
<ul class="task_li" style="list-style:none">
<dx:ASPxCallbackPanel id="cbp_tasks" runat="server" ClientInstanceName="cbp_tasks" OnCallback="cbp_tasks_Callback">
	
	<PanelCollection>
<dx:PanelContent runat="server"></dx:PanelContent>
</PanelCollection>
<ClientSideEvents EndCallback="huddle.existing_huddle.task.action_handler.end"/>
</dx:ASPxCallbackPanel>
</ul>
		</dx:PanelContent>
	</PanelCollection>
</dx:ASPxCallbackPanel>
<asp:SqlDataSource ID="ds_tasks" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
<asp:SqlDataSource ID="ds_assignedto" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>

<dx:ASPxCallback ID="cb_task" runat="server" ClientInstanceName="cb_task" oncallback="cb_task_Callback">
	<ClientSideEvents BeginCallback="huddle.existing_huddle.task.action_handler.begin" CallbackComplete="huddle.existing_huddle.task.action_handler.complete" CallbackError="huddle.existing_huddle.task.action_handler.error" EndCallback="huddle.existing_huddle.task.action_handler.end" />
</dx:ASPxCallback>
