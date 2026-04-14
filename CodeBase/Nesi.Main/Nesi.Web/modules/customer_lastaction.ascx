<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_customer_lastaction" Codebehind="customer_lastaction.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

	<script type="text/javascript">
		function lastaction_callback_complete(s,e)
			{
			var js_action			= $("#<%= hid_js_closeaction.ClientID %>").val();
			eval(js_action);
			}
		function lastaction_do_callback(s,e)
			{
			var customer_id			= $("#<%= hid_customer_id.ClientID %>").val();
			var address_id			= $("#<%= hid_address_id.ClientID %>").val();	
			var is_valid = true;
			var errors = "";
			if(lastaction_dt.GetText() == "")
				{
				is_valid = false;
				errors += "- Date not supplied\n";
				}
			if(lastaction_action.GetText() == "")
				{
				is_valid = false;
				errors += "- Action not supplied\n";
				}
			if(lastaction_employee.GetText() == "")
				{
				is_valid = false;
				errors += "- Employee not supplied\n";
				}
			if(lastaction_origin.GetText() == "")
				{
				is_valid = false;
				errors += "- Origin not supplied\n";
				}
			if(is_valid)
				{
				var data		= 	{
									customer_id: customer_id,
									address_id: address_id,
									date: lastaction_dt.GetText(),
									action: lastaction_action.GetValue(),
									employee: lastaction_employee.GetValue(),
									origin: lastaction_origin.GetValue(),
									note: lastaction_notes.GetText()
									};
				cb_customer_lastaction_action.PerformCallback(JSON.stringify(data));
				}
			else
				{
				alert("There were errors with your submission\n"+errors);
				}
			}
	</script>
<table cellpadding="2" cellspacing="0" style="text-align:left" width="100%">
	<tr>
		<td><b>Customer:</b>
			<dx:ASPxLabel ID="lastaction_customername" runat="server">
			</dx:ASPxLabel>
		</td>
	</tr>
	<tr>
		<td><b>Location:</b>
			<dx:ASPxLabel ID="lastaction_customerlocation" runat="server">
			</dx:ASPxLabel>
		</td>
	</tr>
	<tr>
		<td><dx:ASPxDateEdit ID="lastaction_dt" runat="server" HelpText="Date of Action:" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" ClientInstanceName="lastaction_dt">
			<HelpTextSettings Position="Top">
			</HelpTextSettings>
			<HelpTextStyle Font-Bold="True" Font-Size="11px" ForeColor="#777777">
			</HelpTextStyle>
			</dx:ASPxDateEdit></td>
	</tr>
	<tr>
		<td><dx:ASPxComboBox ID="lastaction_action" runat="server" HelpText="Action:" DataSourceID="ds_historyaction" TextField="action" ValueField="id" ValueType="System.Int32" ClientInstanceName="lastaction_action">
			<HelpTextSettings Position="Top">
			</HelpTextSettings>
			<HelpTextStyle Font-Bold="True" Font-Size="11px" ForeColor="#777777">
			</HelpTextStyle></dx:ASPxComboBox></td>
	</tr>
	<tr>
		<td><dx:ASPxComboBox ID="lastaction_employee" runat="server" HelpText="Employee:" DataSourceID="ds_historyactionmembers" TextField="name" ValueField="id" ValueType="System.Int32" ClientInstanceName="lastaction_employee">
			<HelpTextSettings Position="Top">
			</HelpTextSettings>
			<HelpTextStyle Font-Bold="True" Font-Size="11px" ForeColor="#777777">
			</HelpTextStyle></dx:ASPxComboBox></td>
	</tr>
	<tr>
		<td><dx:ASPxComboBox ID="lastaction_origin" runat="server" HelpText="Origin:" DataSourceID="ds_origin" TextField="name" ValueField="id" ValueType="System.Int32" ClientInstanceName="lastaction_origin">
			<HelpTextSettings Position="Top">
			</HelpTextSettings>
			<HelpTextStyle Font-Bold="True" Font-Size="11px" ForeColor="#777777">
			</HelpTextStyle></dx:ASPxComboBox></td>
	</tr>
	<tr>
		<td><dx:ASPxMemo ID="lastaction_notes" runat="server" ClientInstanceName="lastaction_notes" Rows="10" Columns="75" HelpText="Notes:">
			<HelpTextSettings Position="Top">
			</HelpTextSettings>
			<HelpTextStyle Font-Bold="True" Font-Size="11px" ForeColor="#777777">
			</HelpTextStyle></dx:ASPxMemo></td>
	</tr>
	<tr>
		<td><dx:ASPxButton ID="lastaction_button" runat="server" Text="Save Event" AutoPostBack="False">
			<ClientSideEvents Click="lastaction_do_callback" />
			<Image Height="16px" Url="~/images/icon/icon[save].gif" Width="16px">
			</Image>
			</dx:ASPxButton></td>
	</tr>
</table>
<dx:ASPxCallback ID="cb_customer_lastaction_action" runat="server" ClientInstanceName="cb_customer_lastaction_action" oncallback="cb_customer_lastaction_action_Callback">
	<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;, &quot;Saving...&quot;);
}" CallbackComplete="lastaction_callback_complete" EndCallback="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
</dx:ASPxCallback>
	<asp:HiddenField ID="hid_customer_id" runat="server" />
	<asp:HiddenField ID="hid_address_id" runat="server" />
	<asp:HiddenField ID="hid_js_closeaction" runat="server" />
	<asp:SqlDataSource ID="ds_historyaction" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT history_action_type_id id,history_action_type_action action FROM history_action_type WHERE history_action_type_action NOT LIKE &quot;%edit&quot; ORDER BY action"></asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_historyactionmembers" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT member_id id, member_fullname name FROM member WHERE member_status = 'Active' ORDER BY member_fullname"></asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_origin" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, name FROM customer_origin ORDER BY name"></asp:SqlDataSource>
	
