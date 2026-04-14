<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_huddle_modules_task" EnableViewState="true" EnableTheming="True" Codebehind="task.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<dx:ASPxCallbackPanel ID="task_cbp_handler" runat="server" oncallback="task_cbp_handler_Callback">
	<PanelCollection>
		<dx:PanelContent>
<table cellpadding="2" cellspacing="0" class="task" id="task_table" runat="server" style="border:solid 1px #047;min-height:40px;margin:2px;border-radius:5px;height:40px;width:100%;max-width:880px;background-color:#fff;">
	<tr>
		<td rowspan="3" style="width:30px;background-color:#047;cursor:pointer;" id="draghandle" class="draghandle" runat="server">&nbsp;</td>
		<td colspan="4" style="background-color:#ddd;color:#000;padding:5px;">
			<div style="float:left;width:90%;">
				<b><%# this.huddle_source %>: </b><span id="link_box" runat="server" style="color:#000;"></span>
			</div>

			
			<div id="Div2" style="float:right;width:80px;" runat="server">
<table><tr><td>
<img runat="server" ID="icon_expand" src="/images/icon/icon[expand].gif" 
					width="16" height="16" onclick="huddle.existing_huddle.task.expand_task(this)" 
					alt="Expand Task" title="Expand Task" style="cursor:pointer;"></img>

</img>

	</img>
</img>

	</img>

</img>

	</img>

</img>

	</img>

</td>
<td>

				<img runat="server" ID="icon_refresh" src="/images/icon/icon[refresh].gif" 
					width="16" height="16" onclick="huddle.existing_huddle.task.refresh_task(this)" 
					alt="Refresh Task" title="Refresh Task" style="cursor:pointer;"></img>

</img>

				</img>
</img>

				</img>

</img>

				</img>

</img>

				</img>

</td>
<td>
	<img runat="server" ID="icon_delete" src="/images/icon/icon[delete].gif" 
					width="16"
					
					onclick="huddle.existing_huddle.task.delete_task(this);"
					height="16" alt="Delete Task" visible="<%# this.show_delete %>"   title="Delete Task" style="cursor:pointer;"></img>

</img>

	</img>
</img>

	</img>

</img>

	</img>

</img>

	</img>

</td>
	</tr>
</table>
			</div>	
			
		</td>
	</tr>
	<tr>
		<td width="200" height="35">
			<div style="font-size:10px;color:#999;font-weight:bold;">Assigned To</div>
			<div style="padding:5px;">
				<%# this.assigned_to %>
			</div>
		</td>
		<td width="100" align="center">
			<div style="font-size:10px;color:#999;font-weight:bold;">&nbsp;</div>
			<div>
			<dx:ASPxComboBox ID="status" runat="server" Width="100%" ValueType="System.Int32" SelectedIndex="<%# this.status_index %>">
				<Items>
					<dx:ListEditItem Value="0" Text="Not Started"/>
					<dx:ListEditItem Value="1" Text="In Progress"/>
					<dx:ListEditItem Value="2" Text="Completed"/>
				</Items>
				<ClientSideEvents SelectedIndexChanged="huddle.existing_huddle.task.update_status" />
			</dx:ASPxComboBox>
			</div>
		</td>
		<td width="100" align="center">
			<div style="font-size:10px;color:#999;font-weight:bold; text-align: left;">Date Due</div>
			<div style="padding:5px;">
				<%# this.date_due %>
			</div>
		</td>
		<td rowspan="2">
			<div class="comment" runat="server" id="comment_div" visible="false" style="padding:10px;">
			<textarea class="comment_box" 
					style="height:100px;width:95%; font-family: 'Segoe UI'; font-size: 14px;" 
					rows="10" cols="20" 
					onkeydown="huddle.existing_huddle.task.comment.handle_keydown(this, event)" 
					runat="server" id="comment_box" placeholder="Add Comment"></textarea>
			<dx:ASPxButton ID="button_new_comment" runat="server" AutoPostBack="false" Text="Send">
				<ClientSideEvents Click="huddle.existing_huddle.task.comment.submit" />
			</dx:ASPxButton>
			</div>
			<div class="comments" runat="server" id="comments_div" visible="false" style="padding:10px;">
			&nbsp;
				
			</div>
		</td>
	</tr>
	<tr>
		<td colspan="3">
			<div style="padding:5px;" class="description" id="description_div" runat="server" visible="false">
			<div style="font-size:10px;color:#999;font-weight:bold;">Description</div>
			<%# this.description %>
			</div>
			<dx:ASPxComboBox ID="combo_copy" runat="server" TextField="name" 
				ValueField="id" Caption="Copy to other future huddle:" >
				<ClientSideEvents SelectedIndexChanged="huddle.existing_huddle.task.copy_task" />
			</dx:ASPxComboBox>
		</td>
	</tr>
</table>
<dx:ASPxCallback ID="monitor" runat="server" oncallback="monitor_Callback"></dx:ASPxCallback>
		</dx:PanelContent>
	</PanelCollection>
</dx:ASPxCallbackPanel>
