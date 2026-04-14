<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_fvr_modules_fvr_by_template" Codebehind="fvr_by_template.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<dx:ASPxCallbackPanel ID="cbp_templatesave" runat="server" Width="100%" ClientInstanceName="cbp_templatesave" oncallback="cbp_templatesave_Callback">
	<ClientSideEvents EndCallback="function(s, e) {
	if(s.cpSaved == &quot;true&quot;)
		{
		alert(&quot;FVR's Sent&quot;);
		gv_details.Refresh();
		}
	else
		{
		alert(&quot;Error occurred: &quot;+s.cpSaved);
		}
}" />
	<PanelCollection>
		<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
			<table width="100%" cellpadding="5" cellspacing="0">
				<tr>
					<td width="175" valign="top" style="background-color:#eee;"><b>Step 1:</b><div style="font-size:11px;color:#777;">(select users that will receive this FVR)</div></td>
				</tr>
				<tr>
					<td>
						<dx:ASPxListBox runat="server" ID="list_employees" ClientInstanceName="list_employees" DataSourceID="ds_users" Height="250px" SelectionMode="Multiple" TextField="name" ValueField="id" Width="100%">
						</dx:ASPxListBox>
						<asp:SqlDataSource ID="ds_users" runat="server" 
							ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
							ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
							></asp:SqlDataSource>
					</td>
				</tr>
				<tr>
					<td valign="top" style="background-color:#eee;"><b>Step 2:</b><div style="font-size:11px;color:#777;">(select a template for this FVR)</div></td>
				</tr>
				<tr>
					<td>
						<dx:ASPxComboBox runat="server" ID="combo_templates" ClientInstanceName="combo_templates" DataSourceID="ds_templates" TextField="name" ValueField="id" Width="100%">
						</dx:ASPxComboBox>
						<asp:SqlDataSource ID="ds_templates" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, name FROM member_fvr_template_header ORDER BY name"></asp:SqlDataSource>
					</td>
				</tr>
				<tr>
					<td valign="top" style="background-color:#eee;"><b>Step 3:</b><div style="font-size:11px;color:#777;">(send this FVR)</div></td>
				</tr>
				<tr>
					<td>
						<dx:ASPxButton ID="bt_send" runat="server" Text="Send" AutoPostBack="False">
							<ClientSideEvents Click="function(s, e) {
var do_save 		= true;
if(list_employees.GetSelectedItems().length == 0)
	{
	do_save			= false;
	alert(&quot;Please select some users to send this FVR to&quot;);
	}
else if(combo_templates.GetValue() == null)
	{
	do_save			= false;
	alert(&quot;Please select a template to use for this FVR&quot;);
	}
if(do_save)
	{
	cbp_templatesave.PerformCallback(&quot;save&quot;);
	}
}" />
						</dx:ASPxButton>
					</td>
				</tr>
			</table>
		</dx:PanelContent>
	</PanelCollection>
</dx:ASPxCallbackPanel>
