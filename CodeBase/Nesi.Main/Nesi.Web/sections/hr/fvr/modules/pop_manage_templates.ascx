<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_fvr_modules_pop_manage_templates" Codebehind="pop_manage_templates.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register Src="template_detail.ascx" TagName="template_detail" TagPrefix="uc1" %>
<%@ Register Src="template_page.ascx" TagName="template_page" TagPrefix="uc1" %>

<script type="text/javascript">
	var fvr_template		= 
		{
		toggle_page:
			function(obj, is_chkbox, show_files)
				{
				var parent_tr			= $(obj).parents("tr:first");
				var chk					= is_chkbox 
											? $(obj) 
											: parent_tr.find("input:checkbox");
				var toggle_state		= is_chkbox ? chk.is(":checked") : !chk.is(":checked");
				if(!is_chkbox)
					{
					if(toggle_state)
						chk.attr("checked", "checked");
					else
						chk.removeAttr("checked");
					}
				if(show_files)
					{
					parent_tr.parents("table:first").find(".div_list_files").css({"display": (toggle_state ? "" : "none") });
					parent_tr.parents("table:first").find(".div_file_buttons").css({"display": (toggle_state ? "" : "none") });
					parent_tr.parents("table:first").find(".div_url").css({"display": (toggle_state ? "" : "none") });
					parent_tr.parents("table:first").find(".div_isrequired").css({"display": (toggle_state ? "" : "none")});
					}
				else
					{
					parent_tr.parents("table:first").find(".div_isrequired").css({"display": "none" });
					}
				},
		newhire_control:
			function(obj, is_chkbox)
				{
				var parent_tr			= $(obj).parents("tr:first");
				var chk					= is_chkbox 
											? $(obj) 
											: parent_tr.find("input:checkbox");
				$(document).find(".newhireack").each(
					function()
						{
						if($(this).attr("id") != chk.attr("id"))
							{
							$(this).removeAttr("checked");
							fvr_template.toggle_page(this, true, true);
							}
						}
					)

				},
		preview_file:
			function(s,e)
				{
				var file_list	= eval($(s.mainElement).parents(".template_page:first").attr("data-fclientid"));
				var id = file_list.GetValue();
				if(id != null)
					{
					boing("/_tools/get_file/index.aspx?file_id="+id+"&iframe=true", 'preview', 768, 480); 
					}
				else
					{
					alert("Please select a file");
					}
				},
		load:
			function(s,e)
				{
				cbp_detail.PerformCallback("existing|"+s.GetValue());
				}
		};
</script>
		<table cellpadding="2" cellspacing="0" width="100%">
			<tr>
				<td width="200" valign="top" height="25"><dx:ASPxButton runat="server" ID="bt_newtemplate" Width="100%" Text="New Template" AutoPostBack="False">
					<ClientSideEvents Click="function(s, e) {
	if(window['pc_type'] != undefined)
		{
		pc_type.SetActiveTabIndex(0);
		}
	list_available_templates.UnselectAll();
	cbp_detail.PerformCallback(&quot;new|0&quot;);
}" />
					</dx:ASPxButton></td>
				<td rowspan="2" valign="top">
					<div style="border:solid 1px #ccc;min-height:400px;">
						<dx:ASPxCallbackPanel ID="cbp_detail" runat="server" Width="100%" ClientInstanceName="cbp_detail" oncallback="cbp_detail_Callback">
							<ClientSideEvents EndCallback="function(s, e) {
	if(s.cpResult != undefined && s.cpResult == 'refresh_available')
		{
		list_available_templates.PerformCallback();
		}
}" />
							<PanelCollection>
								<dx:PanelContent runat="server" SupportsDisabledAttribute="True"></dx:PanelContent>
							</PanelCollection>
						</dx:ASPxCallbackPanel>
					</div>
				</td>
			</tr>
			<tr>
				<td valign="top"><dx:ASPxListBox ID="list_available_templates" ClientInstanceName="list_available_templates" runat="server" Width="200px" Height="300px" oncallback="list_available_templates_Callback"><ClientSideEvents SelectedIndexChanged="fvr_template.load" /></dx:ASPxListBox></td>
			</tr>
		</table>