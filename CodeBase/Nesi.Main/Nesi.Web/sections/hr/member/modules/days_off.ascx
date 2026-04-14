<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_member_modules_days_off" EnableTheming="True" Codebehind="days_off.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register Src="~/sections/hr/member/modules/days_off_new.ascx" TagPrefix="uc" TagName="days_off_new" %>
<style type="text/css">
	.style2
	{
		font-family: Calibri;
	}
</style>
<div class="days_off">
<dx:ASPxGridView ID="gv_days_off" ClientInstanceName="gv_days_off" runat="server" 
		AutoGenerateColumns="False" KeyFieldName="id" Width="100%" 
		oncommandbuttoninitialize="gv_days_off_CommandButtonInitialize" 
		onhtmleditformcreated="gv_days_off_HtmlEditFormCreated" 
		onrowdeleting="gv_days_off_RowDeleting" Theme="NETheme01" 
		EnableRowsCache="False">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" >
<Image Width="16px" Url="~/images/icon/icon[delete].gif"></Image>
																	</DeleteButton>
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" >
<Image Width="16px" Url="~/images/icon/icon[edit].gif"></Image>
																	</EditButton>
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" >
<Image Width="16px" Url="~/images/icon/icon[add].gif"></Image>
																	</NewButton>
																</SettingsCommandButton>
	<Columns>
		<dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="60px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true">
			
			
			
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" 
			VisibleIndex="1" Width="75px">
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataDateColumn Caption="Date Requested" FieldName="date_requested" VisibleIndex="5" Width="150px">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd h:mm tt">
			</PropertiesDateEdit>
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataDateColumn>
		<dx:GridViewDataDateColumn Caption="Start Date" FieldName="date_start" VisibleIndex="6" Width="150px">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd h:mm tt">
			</PropertiesDateEdit>
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataDateColumn>
		<dx:GridViewDataDateColumn Caption="End Date" FieldName="date_end" VisibleIndex="7" Width="150px">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd h:mm tt">
			</PropertiesDateEdit>
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataDateColumn>
		<dx:GridViewDataTextColumn Caption="Comments" FieldName="comments" VisibleIndex="10">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Requested By" FieldName="requestedby" VisibleIndex="4" Width="150px">
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Leave Type" FieldName="leavetype" VisibleIndex="8" Width="150px">
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" Name="business_unit" VisibleIndex="2" Width="150px">
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Employee" FieldName="employee" Name="employee" VisibleIndex="3" Width="200px">
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Request Type" FieldName="requesttype" VisibleIndex="9" Width="150px">
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
	</Columns>

	<SettingsBehavior ConfirmDelete="True" EnableRowHotTrack="True" />
	<SettingsPager PageSize="20">
	</SettingsPager>
	<SettingsEditing Mode="PopupEditForm" />
	<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" ShowFilterRowMenu="True" ShowTitlePanel="True" />
	<SettingsPopup>
		<EditForm Modal="True" Width="600px" HorizontalAlign="Center" />
		
<HeaderFilter MinHeight="140px"></HeaderFilter>
	</SettingsPopup>
	<Styles>
		<Header HorizontalAlign="Center">
		</Header>
		<EditForm>
			<Paddings Padding="10px" />
		</EditForm>
		<TitlePanel Font-Names="Arial" Font-Size="12px">
		</TitlePanel>
	</Styles>
	<StylesPopup>
		<EditForm>
			<Content>
				<Paddings Padding="15px" />
			</Content>
			<ModalBackground Opacity="0">
			</ModalBackground>
		</EditForm>
	</StylesPopup>
																<ClientSideEvents EndCallback="OnEndCallback" />
	<Templates>
		<TitlePanel>
			<dx:ASPxButton ID="bt_new" runat="server" AutoPostBack="False" Text="New Entry" 
				Width="120px" Theme="NETheme01">
				<ClientSideEvents Click="function(s, e) {pop_new.Show();new_combo_employee.SetSelectedIndex(-1);pop_new.PerformCallback();}" />
				<Image Url="~/images/icon/icon[add].gif">
				</Image>
			</dx:ASPxButton>
		</TitlePanel>
		<EditForm>
			<table width="100%">
				<tr>
					<td width="150" align="left" class="style2"><strong>ID:</strong></td>
					<td colspan="2" >
						<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" ReadOnly="True" 
							Text='<%# Eval("id") %>' Theme="MaterialCompact" Width="170px">
						</dx:ASPxTextBox>
					</td>
				</tr>
				<tr>
					<td align="left" class="style2" width="150">
						<b>Employee:</b></td>
					<td colspan="2" >
						<dx:ASPxComboBox ID="combo_employee" runat="server" 
							ClientInstanceName="edit_combo_employee" DataSourceID="ds_members_active" 
							DropDownStyle="DropDown" TextField="text" Theme="MaterialCompact" 
							Value='<%# Bind("member_id") %>' ValueField="id" ValueType="System.Int32" 
							Width="100%">
							<ValidationSettings ValidationGroup="daysoffedit">
								<RequiredField IsRequired="True" />
							</ValidationSettings>
						</dx:ASPxComboBox>
					</td>
				</tr>
				<tr>
					<td align="left" class="style2"><b>Start Date:</b></td>
					<td colspan="2">
						<dx:ASPxDateEdit ID="dte_start" runat="server" Width="100%" AllowNull="False" 
										 AllowUserInput="False" Value='<%# Bind("date_start") %>' 
										 ClientInstanceName="edit_dte_start" EditFormat="DateTime" Theme="MaterialCompact">
							<TimeSectionProperties Visible="True">
							</TimeSectionProperties>
							<ClientSideEvents DateChanged="function(s,e){CalculateHours(true);}"></ClientSideEvents>
							<ValidationSettings ValidationGroup="daysoffedit">
								<RequiredField IsRequired="True" />
							</ValidationSettings>
						</dx:ASPxDateEdit>
					</td>
				</tr>
				<tr>
					<td align="left" class="style2"><b>End Date:</b></td>
					<td colspan="2">
						<dx:ASPxDateEdit ID="dte_end" runat="server" Width="100%" AllowNull="False" 
										 AllowUserInput="False" Value='<%# Bind("date_end") %>' 
										 ClientInstanceName="edit_dte_end" EditFormat="DateTime" Theme="MaterialCompact">
							<TimeSectionProperties Visible="True">
							</TimeSectionProperties>
							<ClientSideEvents DateChanged="function(s,e){CalculateHours(true);}"></ClientSideEvents>
							<ValidationSettings ValidationGroup="daysoffedit">
								<RequiredField IsRequired="True" />
							</ValidationSettings>
						</dx:ASPxDateEdit>
					</td
				</tr>
				<tr>
					<td class="caption">Total Hours:</td>
					<td class="total_hours_edit" style="font-size:1.5em;color:#006400;font-weight:bold;"></td>
				</tr>
				<tr>
					<td align="left" class="style2"><b>Request Type:</b></td>
					<td colspan="2">
						
						<dx:ASPxComboBox ID="combo_request_type" runat="server" 
										 ValueType="System.Int32" Width="100%" Value='<%# Bind("requesttype_id") %>' 
										 ClientInstanceName="edit_combo_request_type" DropDownStyle="DropDown" 
										 Theme="MaterialCompact">
							<Items>
								<dx:ListEditItem Text="Select Request Type" Value="0" />
								<dx:ListEditItem Text="Phone" Value="1" />
								<dx:ListEditItem Text="Online" Value="2" />
								<dx:ListEditItem Text="Verbal" Value="3" />
								<dx:ListEditItem Text="Form" Value="4" />
							</Items>
							<ValidationSettings ValidationGroup="daysoffedit">
								<RequiredField IsRequired="True" />
							</ValidationSettings>
						</dx:ASPxComboBox>
						
					</td>
				</tr>
				<tr>
					<td align="left" class="style2"><b>Day Off Type:</b></td>
					<td colspan="2">
						<dx:ASPxComboBox ID="combo_dayoff_type" runat="server" 
							Value='<%# Bind("type_id") %>' ClientInstanceName="edit_combo_dayoff_type" 
							DataSourceID="ds_types" ValueType="System.Int32" Width="100%" TextField="type" 
							ValueField="id" DropDownStyle="DropDown"  TextFormatString="{0}" Theme="MaterialCompact">
							<Columns>
								<dx:ListBoxColumn Caption="Type" FieldName="type">
								</dx:ListBoxColumn>
								<dx:ListBoxColumn Caption="Paid" FieldName="paid">
								</dx:ListBoxColumn>
							</Columns>
							<ValidationSettings ValidationGroup="daysoffedit">
								<RequiredField IsRequired="True" />
							</ValidationSettings>
						</dx:ASPxComboBox>
					</td>
				</tr>
				<tr>
					<td valign="top" align="left" class="style2"><b>Comments:</b></td>
					<td colspan="2">
						<dx:ASPxMemo ID="tb_comments" runat="server" Height="71px" 
							Text='<%# Bind("comments") %>' Width="100%" 
							ClientInstanceName="edit_memo_comments" Theme="MaterialCompact">
							<ValidationSettings ValidationGroup="daysoffedit">
								<RequiredField IsRequired="True" />
							</ValidationSettings>
						</dx:ASPxMemo>
					</td>
				</tr>
				<tr>
					<td valign="top">
						&nbsp;</td>
					<td>
						<dx:ASPxButton ID="bt_save" runat="server" AutoPostBack="False" Text="Save" 
							Theme="MaterialCompact" Width="100px" ValidationGroup="daysoffedit">
							<ClientSideEvents Click="function(s, e) {
	if(ASPxClientEdit.ValidateGroup('daysoffedit'))
		{
	var data		= 
		{
		id:				$('.hid_id').val(),
		employee:		edit_combo_employee.GetValue(),
		dt_start:		edit_dte_start.GetDate(),
		dt_end:			edit_dte_end.GetDate(),
		request_type:	edit_combo_request_type.GetValue(),
		leave_type:		edit_combo_dayoff_type.GetValue(),
		comments:		edit_memo_comments.GetText()
		}
	cb_edit.PerformCallback(JSON.stringify(data));
		}
}" />
							<Image Url="~/images/icon/icon[save].gif">
							</Image>
						</dx:ASPxButton>
					</td>
					<td>
						<b>
						<dx:ASPxButton ID="bt_cancel" runat="server" AutoPostBack="False" Text="Cancel" 
							Theme="MaterialCompact" Width="121px">
							<ClientSideEvents Click="function(s, e) {
	gv_days_off.CancelEdit();
}" />
							<Image Url="~/images/icon/icon[cancel].gif">
							</Image>
						</dx:ASPxButton>
						</b>
					</td>
				</tr>
			</table>
			<input type="hidden" id="hid_id" class="hid_id" runat="server" value='<%# Bind("id") %>' />
		</EditForm>
	</Templates>
</dx:ASPxGridView>

</div>
<asp:SqlDataSource ID="ds_members" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
<asp:SqlDataSource ID="ds_members_active" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
<asp:SqlDataSource ID="ds_types" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id,a.type, a.paid FROM vacation_type a WHERE a.active = 1 AND a.id != 4 ORDER BY a.order_in_ddl, a.type"></asp:SqlDataSource>
<dx:ASPxPopupControl ID="pop_new" runat="server" AllowDragging="True" 
	AutoUpdatePosition="True" ClientInstanceName="pop_new" HeaderText="New Entry" 
	PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" 
	PopupVerticalAlign="TopSides" Width="550px" CloseAction="CloseButton" 
	Modal="True" onwindowcallback="pop_new_WindowCallback" Theme="NETheme01">
	<ClientSideEvents Shown="function(s, e) {
new_combo_employee.Focus();
}" />

	<ModalBackgroundStyle BackColor="Transparent">
	</ModalBackgroundStyle>
	<ContentCollection>
		<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
			<uc:days_off_new ID="uc_days_off_new" runat="server" />
		</dx:PopupControlContentControl>
	</ContentCollection>
</dx:ASPxPopupControl>


<dx:ASPxCallback ID="cb_edit" runat="server" ClientInstanceName="cb_edit" oncallback="cb_edit_Callback">
	<ClientSideEvents BeginCallback="function(s, e) {
	please_wait('start');
}" CallbackComplete="function(s, e) {
	if(e.result == &quot;SUCCESS&quot;)
		{
		alert(&quot;Successfully saved entry&quot;);
		gv_days_off.CancelEdit();
		}
	please_wait('stop');
}" CallbackError="function(s, e) {
	please_wait('stop');
}" />
</dx:ASPxCallback>