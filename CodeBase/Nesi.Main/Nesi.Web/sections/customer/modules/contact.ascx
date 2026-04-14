<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_customer_modules_contact" Codebehind="contact.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>





<div>
	<dx:ASPxHiddenField ID="main_persistence_handler" runat="server" ClientInstanceName="main_persistence_handler">
	</dx:ASPxHiddenField>
	<button type="button" onclick="var custid = $('#<%# hdn_customer_id.ClientID %>').val();var url = '/sections/hr/member_access/index.aspx?type=contact&business_unit_id=';boing(url+custid, 'Contact Access', 900, 900);">Nesi.ca Privileges and Access</button>
	<asp:SqlDataSource ID="ds_ddl_location" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT '0' value, 'Select an Address' text UNION SELECT address_id value, CONCAT('(',address_type,') - ', URLDECODE(address_addr1)) text FROM address WHERE address_table IN ('Customer', 'Worksite') AND address_table_id = @customer_id" OnSelecting="ds_ddl_location_Selecting">
		<SelectParameters>
			<asp:Parameter DefaultValue="0" Name="@customer_id" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_contacts" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
	a.contact_name,
	a.name_first,
	a.name_last, 
	a.contact_name, 
	a.contact_name, 
	a.contact_id,
	a.contact_cellphone,
	a.contact_title,
	a.contact_directline, 
	a.contact_extension, 
	a.contact_email, 
	a.contact_password, 
	a.contact_status, 
	a.facebook,
	a.twitter,
	a.linkedin,
	a.contact_login_enabled login_enabled, 
	a.contact_status_id as status,
	a.address_id,
 a.stopsurveys,
 (select count(woprog.WOProg_ID) from woprog where woprog.woprog_customer_id = a.contact_cust_id and WOProg_Contact_ID = a.contact_id and a.contact_type='Customer') wos,
	(select count(quote_master.quote_id) from quote_master where quote_master.customer_id = a.contact_cust_id and quote_master.Contact_ID = a.contact_id and a.contact_type='Customer') quotes

FROM 
	contact a
LEFT JOIN
	customer_or_contact_status b ON 
		b.customer_or_contact_status_id = a.contact_status_id
WHERE 
	a.contact_cust_id	= @customer_id AND 
	a.address_id		= @address_id AND
	a.contact_type		= 'Customer'" OnSelecting="ds_ddl_location_Selecting">
		<SelectParameters>
			<asp:Parameter DefaultValue="0" Name="@customer_id" />
			<asp:Parameter DefaultValue="0" Name="@address_id" />
		</SelectParameters>
	</asp:SqlDataSource>
	<dx:ASPxGridView ID="gv_contacts" ClientInstanceName="gv_contacts" runat="server" AutoGenerateColumns="False" OnCustomJSProperties="gv_contact_custom_js" Font-Names="Arial" Font-Size="9pt" KeyFieldName="contact_id" OnRowUpdating="gv_contacts_RowUpdating" OnInitNewRow="gv_contacts_InitNewRow" OnRowDeleting="gv_contacts_RowDeleting" OnRowInserting="gv_contacts_RowInserting" OnCustomButtonCallback="gv_contacts_CustomButtonCallback" OnClientLayout="gv_contacts_ClientLayout" Width="100%" OnHtmlEditFormCreated="gv_contacts_HtmlEditFormCreated" OnCellEditorInitialize="gv_contacts_CellEditorInitialize" DataSourceID="ds_contacts" OnDataBinding="gv_contacts_DataBinding" OnDataBound="gv_contacts_DataBound" style="margin-top: 14px">
		<ClientSideEvents CustomButtonClick="function(s, e) {main_persistence_handler.Set(&quot;current_contact_id&quot;, s.cpid[e.visibleIndex]);pop_merge.Show();}" RowClick="function(s, e) {s.StartEditRow(e.visibleIndex);}" Init="function(s, e) {
	s.Refresh();
}"></ClientSideEvents>
        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
		<Columns>
			<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" Width="70px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true"   ShowNewButton="true">
				
				
				
				
				<CustomButtons>
					<dx:GridViewCommandColumnCustomButton ID="Merge">
						<Image Url="/images/icon/transfer.jpg" ToolTip="Merge this contact with another one">
						</Image>
					</dx:GridViewCommandColumnCustomButton>
				</CustomButtons>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="ID" FieldName="contact_id" Visible="false" VisibleIndex="1" ReadOnly="True" Width="65px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Name" FieldName="contact_name" VisibleIndex="3" Width="150px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Cell Phone" FieldName="contact_cellphone" VisibleIndex="4" Width="80px">
				<PropertiesTextEdit>
					<ValidationSettings ErrorText="Must be North American structure, use Extension if international number is required.">
						<RegularExpression ErrorText="Must be North American structure, use Extension if international number is required." />
					</ValidationSettings>
				</PropertiesTextEdit>
				<EditFormSettings Caption="Cell Phone (Format: 999-999-9999)" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Ext." FieldName="contact_extension" VisibleIndex="6" Width="50px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Email (Login Name)" FieldName="contact_email" VisibleIndex="7" Width="150px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="NESI PW" FieldName="contact_password" VisibleIndex="8" Width="75px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Active" FieldName="contact_status" VisibleIndex="9" Width="50px">
				<PropertiesComboBox>
					<Items>
						<dx:ListEditItem Text="Active" Value="Active" />
						<dx:ListEditItem Text="InActive" Value="InActive" />
					</Items>
				</PropertiesComboBox>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataComboBoxColumn Caption="Status" FieldName="status" VisibleIndex="10" Width="65px" UnboundExpression="8">
				<PropertiesComboBox DataSourceID="ds_contactstatus" TextField="customer_or_contact_status" ValueField="customer_or_contact_status_id" ValueType="System.Int32">
				</PropertiesComboBox>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataComboBoxColumn Caption="Login Enable" FieldName="login_enabled" VisibleIndex="11" Width="65px">
				<PropertiesComboBox>
					<Items>
						<dx:ListEditItem Text="Enabled" Value="1" />
						<dx:ListEditItem Text="Disabled" Value="0" />
					</Items>
				</PropertiesComboBox>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="Direct Line" VisibleIndex="5" FieldName="contact_directline" Width="80px">
				<EditFormSettings Caption="Direct Line (Format: 999-999-9999)" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Title" FieldName="contact_title" VisibleIndex="2" Width="100px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataCheckColumn Caption="Stop Surveys" FieldName="stopsurveys" 
				VisibleIndex="12" Width="60px">
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataTextColumn Caption="WOs" FieldName="wos" 
				ToolTip="WOs since the beginning of time" VisibleIndex="13">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Quotes" FieldName="quotes" 
				ToolTip="Quotes since the beginning of time" VisibleIndex="14">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior ConfirmDelete="True" />
		<SettingsPager PageSize="25">
			<AllButton Text="All">
			</AllButton>
			<NextPageButton Text="Next &gt;">
			</NextPageButton>
			<PrevPageButton Text="&lt; Prev">
			</PrevPageButton>
		</SettingsPager>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
		<Templates>
			<EditForm>
				<div class='editors'>
					<table cellpadding="2" cellspacing="0">
						<tr>
							<td class='c'>
								First Name:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_firstname" CssClass="name_first" Text='<%# Bind("name_first") %>'></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Last Name:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_lastname" CssClass="name_last" Text='<%# Bind("name_last") %>'></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Name:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_name" CssClass="t_name" Text='<%# Bind("contact_name") %>'></asp:TextBox>
								<button type="button" onclick="NE_Customer.contact.default_name(this)">
									Default Name</button>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Title:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_title" Text='<%# Bind("contact_title") %>'></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Email Address:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_email" Text='<%# Bind("contact_email") %>'></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Cell #:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_cell" Text='<%# Bind("contact_cellphone") %>'></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Direct #:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_direct" Text='<%# Eval("contact_directline") %>'></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Extension:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_extension" Text='<%# Eval("contact_extension") %>'></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Password:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_password" Text='<%# Eval("contact_password") %>'></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Facebook:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_facebook" Text='<%# Eval("facebook") %>'></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Twitter:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_twitter" Text='<%# Eval("twitter") %>'></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Linkedin:
							</td>
							<td class='v'>
								<asp:TextBox runat="server" ID="t_linkedin" Text='<%# Eval("linkedin") %>'></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Status:
							</td>
							<td class='v'>
								<asp:DropDownList runat="server" DataTextField="text" DataValueField="value" DataSourceID="ds_status" SelectedValue='<%# Eval("status") %>' ID="ddl_status" ValueType="System.Int32">
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Login Enabled?:
							</td>
							<td class='v'>
								<dx:ASPxCheckBox ID="chk_loginenabled" runat="server" CheckState="Unchecked" 
									Text=" " Value='<%# Eval("login_enabled") %>' ValueChecked="1" 
									ValueType="System.Int32" ValueUnchecked="0">
								</dx:ASPxCheckBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Active:
							</td>
							<td class='v'>
								<dx:ASPxComboBox ID="ddl_active" runat="server" Native="True" Value='<%# Bind("contact_status") %>' ValueType="System.String">
									<Items>
										<dx:ListEditItem Text="Active" Value="Active" />
										<dx:ListEditItem Text="Inactive" Value="Inactive" />
									</Items>
								</dx:ASPxComboBox>
							</td>
						</tr>
						<tr>
							<td class='c'>
								Location:
							</td>
							<td class='v'>
								<dx:ASPxComboBox runat="server" ID="ddl_location" ValueField="value" TextField="text" DataSourceID="ds_ddl_location" Native="True" Value='<%# Bind("address_id") %>' ValueType="System.Int32">
								</dx:ASPxComboBox>
							</td>
						</tr>
						<tr>
							<td class="c">
								Stop Surveys:</td>
							<td class="v">
								<dx:ASPxCheckBox ID="chk_stop_surveys" runat="server" CheckState="Unchecked" 
									Text=" " Value='<%# Eval("stopsurveys") %>' ValueChecked="1" 
									ValueType="System.Int32" ValueUnchecked="0">
								</dx:ASPxCheckBox>
							</td>
						</tr>
					</table>
				</div>
				<div style="margin-top: 10px">
					<div style="float: left; width: 10%;">
						<dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" Text="Save" Width="100px" CssClass="input" ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' />
					</div>
					<div style="float: left; width: 75%;" align="center" id="template_div" runat="server" data-user_id='<%# Eval("contact_id") %>'>
						<table cellpadding="3" cellspacing="0" id="templates">
							<tr>
								<td>
									Apply a privilege template:
								</td>
								<td>
									<dx:ASPxComboBox ID="priv_templates" ClientInstanceName="priv_templates" runat="server" DataSourceID="ds_templates" TextField="name" ValueField="id" ValueType="System.Int32">
									</dx:ASPxComboBox>
								</td>
								<td>
									<dx:ASPxButton ID="btn_apply" runat="server" ClientSideEvents-Click="function(s,e){if(confirm('Are you sure? This action will overwrite all existing privileges.')){NE_Customer.contact.do_apply_template.run();}}" Text="Apply" Width="100px" />
								</td>
							</tr>
						</table>
					</div>
					<div style="float: left; width: 10%">
						<dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" Text="Cancel" Width="100px" ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' />
					</div>
				</div>
			</EditForm>
		</Templates>
	</dx:ASPxGridView>
									<asp:SqlDataSource ID="ds_status" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT null value, &quot;Please Select&quot; text UNION SELECT customer_or_contact_status_id value, customer_or_contact_status text FROM customer_or_contact_status"></asp:SqlDataSource>

	<asp:SqlDataSource ID="ds_templates" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT * FROM pageprivilege_template WHERE type = 2;"></asp:SqlDataSource>
	<asp:SqlDataSource ID="ds_contactstatus" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT * FROM customer_or_contact_status"></asp:SqlDataSource>
	<dx:ASPxPopupControl ID="pop_merge" runat="server" ClientInstanceName="pop_merge" CssPostfix="BlackGlass" Font-Names="Arial" HeaderText="Merge Contact" Width="600px" AnimationType="None" LoadContentViaCallback="OnFirstShow" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" Theme="Default">
		<ClientSideEvents Shown="function(s,e)
	{
	s.PerformCallback();
	}" EndCallback="function(s, e) {
	
	combo_merge_from.SetValue(main_persistence_handler.Get(&quot;current_contact_id&quot;));
	combo_merge_to.SetValue(null);
}"></ClientSideEvents>
		<ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
		</ContentStyle>
		<HeaderStyle>
			<Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
			<Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
		</HeaderStyle>
		<ContentCollection>
			<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
				<table style="width: 100%;">
					<tr>
						<td align="center">
							<b>Merge this contact</b>
						</td>
						<td align="center">
							<dx:ASPxLoadingPanel ID="lp_merge" runat="server" ClientInstanceName="lp_merge" Modal="True" Text="Merging&amp;hellip;">
								<LoadingDivStyle BackColor="Transparent">
								</LoadingDivStyle>
							</dx:ASPxLoadingPanel>
						</td>
						<td align="center">
							<b>Into this contact</b>
						</td>
					</tr>
					<tr>
						<td align="center">
							<dx:ASPxComboBox ID="combo_merge_from" runat="server" ValueField="contact_id" TextField="contact_name" ClientInstanceName="combo_merge_from">
							</dx:ASPxComboBox>
						</td>
						<td align="center">
							<dx:ASPxCallback ID="cb_merge" runat="server" ClientInstanceName="cb_merge" OnCallback="cb_merge_Callback">
								<ClientSideEvents BeginCallback="function(s, e) {
lp_merge.Show();
}" EndCallback="function(s, e) {
}" CallbackComplete="function(s, e) {
lp_merge.Hide();
if(e.result == 'SUCCESS')
	{
	alert(&quot;Successfully merged contacts. This window will now close.&quot;);
	pop_merge.Hide();
	gv_contacts.Refresh();
	}
else
	{
	alert(e.result);
	}
}" />
								<ClientSideEvents CallbackComplete="function(s, e) {
lp_merge.Hide();
if(e.result == &#39;SUCCESS&#39;)
	{
	alert(&quot;Successfully merged contacts. This window will now close.&quot;);
	pop_merge.Hide();
	gv_contacts.Refresh();
	}
else
	{
	alert(e.result);
	}
}" BeginCallback="function(s, e) {
lp_merge.Show();
}" EndCallback="function(s, e) {
}"></ClientSideEvents>
							</dx:ASPxCallback>
						</td>
						<td align="center">
							<dx:ASPxComboBox ID="combo_merge_to" runat="server" ValueField="contact_id" TextField="contact_name" ClientInstanceName="combo_merge_to">
								<ClientSideEvents SelectedIndexChanged="function(s, e) {
	if(s.GetValue() == combo_merge_from.GetValue())
		{
		alert(&quot;You cannot merge the selected contact to itself.&quot;);
		s.SetValue(null);
		}
}" />
							</dx:ASPxComboBox>
						</td>
					</tr>
					<tr>
						<td>
							&nbsp;
						</td>
						<td align="center">
							<dx:ASPxButton ID="b_process_merge" runat="server" Text="Do Merge" AutoPostBack="False">
								<ClientSideEvents Click="function(s, e) {
	if(combo_merge_from.GetText() != &quot;&quot; &amp;&amp; combo_merge_to.GetText() != &quot;&quot;)
		{
	if(confirm(&quot;Are you sure that you want to merge &quot;+combo_merge_from.GetText()+&quot; into &quot;+combo_merge_to.GetText()+&quot;?&quot;))
		{
		cb_merge.PerformCallback(combo_merge_from.GetValue()+&quot;|&quot;+combo_merge_to.GetValue());
		}
		}
	else
		{
		alert(&quot;Both a FROM and a TO contact have to be selected.&quot;);
		}
}" />
							</dx:ASPxButton>
						</td>
						<td>
							&nbsp;
						</td>
					</tr>
				</table>
			</dx:PopupControlContentControl>
		</ContentCollection>
		<ClientSideEvents Shown="function(s,e)
	{
	s.PerformCallback();
	}" EndCallback="function(s, e) {
	
	combo_merge_from.SetValue(main_persistence_handler.Get(&quot;current_contact_id&quot;));
	combo_merge_to.SetValue(null);
}" />
	</dx:ASPxPopupControl>

</div>

<asp:HiddenField ID="hdn_address_id" runat="server" />
<asp:HiddenField ID="hdn_customer_id" runat="server" />