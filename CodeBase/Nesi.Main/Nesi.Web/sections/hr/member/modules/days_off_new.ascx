<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_member_modules_days_off_new" EnableTheming="True" Codebehind="days_off_new.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

	<style type="text/css">
		.caption
		{
			color: #666666;
			font-weight: bold;
			font-family: Calibri;
			font-size: 1em;
		}
	</style>
	<script type="text/javascript">
	</script>
	<table id="table_new" runat="server">
		<tr ID="row_foremployee_tr" runat="server" visible="false">
			<td runat="server" align="left" id="row_foremployee_td" colspan="2"></td>
		</tr>
		<tr ID="row_employee" runat="server">
			<td runat="server" align="left" style="height: 25px;" class="caption">
				*Employee:</td>
			<td runat="server"><input type="hidden" id="id_single_employee" runat="server" class="id_single_employee" />
				<dx:ASPxComboBox ID="combo_employee" runat="server" 
					DataSourceID="ds_members_active" DropDownStyle="DropDown" TextField="text" 
					ValueField="id" ValueType="System.Object" Width="100%" 
					ClientInstanceName="new_combo_employee" IncrementalFilteringMode="Contains" 
					Theme="MaterialCompact" NullText="Select Employee">
					<ValidationSettings ValidationGroup="daysoff">
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxComboBox>
			</td>
		</tr>
		<tr>
			<td align="left" width="265" style="height: 25px;" class="caption">
				*Start Date:</td>
			<td style="height: 25px;">
				<dx:ASPxDateEdit ID="dte_start" runat="server" 
					ClientInstanceName="new_dte_start" AllowNull="False" AllowUserInput="True" 
					EditFormat="DateTime" Theme="MaterialCompact" NullText="Select Start">
					<TimeSectionProperties Visible="True">
					</TimeSectionProperties>
					<ClientSideEvents DateChanged="function(s,e){CalculateHours(false);}"></ClientSideEvents>
					<ValidationSettings ValidationGroup="daysoff">
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxDateEdit>
			</td>
		</tr>
		<tr>
			<td align="left" style="height: 25px;" class="caption">
				*End Date:</td>
			<td style="height: 25px;">
				<dx:ASPxDateEdit ID="dte_end" runat="server" ClientInstanceName="new_dte_end" 
					AllowNull="False" AllowUserInput="True" EditFormat="DateTime" Theme="MaterialCompact" NullText="Select End">
					<TimeSectionProperties Visible="True">
					</TimeSectionProperties>
					<ClientSideEvents DateChanged="function(s,e){CalculateHours(false);}"></ClientSideEvents>
					<ValidationSettings ValidationGroup="daysoff">
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxDateEdit>
			</td>
		</tr>
		<tr>
			<td class="caption">Total Hours:</td>
			<td class="total_hours_new" style="font-size:1.5em;color:#006400;font-weight:bold;"></td>
		</tr>
		<tr>
			<td align="left" style="height: 25px;" class="caption">*Request Type:</td>
			<td style="height: 25px;">		
				<dx:ASPxComboBox ID="combo_request_type" runat="server" 
					ValueType="System.Int32" Width="100%"  DropDownStyle="DropDown" 
					ClientInstanceName="new_combo_request_type" Theme="MaterialCompact" NullText="Select Request Type">
					<Items>
						<dx:ListEditItem Text="Phone" Value="1" />
						<dx:ListEditItem Text="Online" Value="2" Selected="True" />
						<dx:ListEditItem Text="Verbal" Value="3" />
						<dx:ListEditItem Text="Form" Value="4" />
					</Items>
					<ValidationSettings ValidationGroup="daysoff">
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxComboBox>
						
			</td>
		</tr>
		<tr>
			<td align="left" valign="top" class="caption">
				*Day Off Type:</td>
			<td valign="top">
				<dx:ASPxComboBox ID="combo_dayoff_type" runat="server" DropDownStyle="DropDown" 
					SelectedIndex="0" ValueType="System.Int32" Width="100%" 
					ClientInstanceName="new_combo_dayoff_type" DataSourceID="ds_types" 
					TextField="type" ValueField="id" 
					Theme="MaterialCompact" EnableTheming="True" TextFormatString="{0}" NullText="Select Days Off Type">
					<Columns>
						<dx:ListBoxColumn Caption="Type" FieldName="type">
						</dx:ListBoxColumn>
						<dx:ListBoxColumn Caption="Paid" FieldName="paid">
						</dx:ListBoxColumn>
					</Columns>
					<ValidationSettings ValidationGroup="daysoff">
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxComboBox>
			</td>
		</tr>
		<tr>
			<td align="left" valign="top" class="caption">
				<b>*Comments:</b></td>
			<td valign="top">
				<dx:ASPxMemo ID="memo_comments" runat="server" Height="100px" Width="250px" 
					ClientInstanceName="new_memo_comments" Theme="MaterialCompact">
					<ValidationSettings>
						<RequiredField IsRequired="True" />
					</ValidationSettings>
				</dx:ASPxMemo>
			</td>
		</tr>
		<tr>
			<td align="left" valign="top" class="caption">
				&nbsp;</td>
			<td valign="top">
				<table>
					<tr>
						<td>				
				<dx:ASPxButton ID="bt_save" runat="server" Text="Save" Width="100px" AutoPostBack="False" 
								Theme="MaterialCompact" EnableTheming="True" ValidationGroup="daysoff">
<ClientSideEvents Click="function(s, e) {
	if(ASPxClientEdit.ValidateGroup('daysoff'))
		{
	var used_id		= new_combo_employee.GetValue() == null ? 0 : new_combo_employee.GetValue();

	var data		= 
		{
		employee:		used_id,
		dt_start:		new_dte_start.GetText(),
		dt_end:			new_dte_end.GetText(),
		request_type:	new_combo_request_type.GetValue(),
		leave_type:		new_combo_dayoff_type.GetValue(),
		comments:		new_memo_comments.GetText()
		}
	if(data.dt_start &gt; data.dt_end)
		{
		alert('The start date is greater than the end date');
		}
	else
		{
		cb_new.PerformCallback(JSON.stringify(data));
		}
	}
}"></ClientSideEvents>

					<Image Url="~/images/icon/icon[save].gif">
					</Image>
				</dx:ASPxButton></td>
						<td>
							<dx:ASPxButton ID="bt_cancel" runat="server" Text="Cancel" Width="118px" 
								AutoPostBack="False" Theme="MaterialCompact">
					<ClientSideEvents Click="function(s, e) {
	pop_new.Hide();
}" />
<ClientSideEvents Click="function(s, e) {
	pop_new.Hide();
}"></ClientSideEvents>

					<Image Url="~/images/icon/icon[cancel].gif">
					</Image>
				</dx:ASPxButton></td>
					</tr>
				</table>

			</td>
		</tr>
		</table>
<dx:ASPxCallback ID="cb_new" runat="server" ClientInstanceName="cb_new" oncallback="cb_new_Callback">
	<ClientSideEvents BeginCallback="function(s, e) {
	please_wait('start');
}" CallbackComplete="function(s, e) {
	if(e.result == &quot;SUCCESS&quot;)
		{
		alert(&quot;Successfully saved entry&quot;);
		gv_days_off.Refresh();
		pop_new.PerformCallback();
		}
	please_wait('stop');
}" CallbackError="function(s, e) {
	please_wait('stop');
}" />
</dx:ASPxCallback>
<asp:SqlDataSource ID="ds_members_active" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
<asp:SqlDataSource ID="ds_types" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id,a.type, a.paid FROM vacation_type a WHERE a.active = 1 AND a.id != 4 ORDER BY a.order_in_ddl, a.type"></asp:SqlDataSource>