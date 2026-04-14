<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_hr_i_manage" MasterPageFile="~/IntraDefault.master" EnableTheming="True"  Title="FVR Listing" Codebehind="manage.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>

<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<%@ Register src="modules/pop_manage_templates.ascx" tagname="pop_manage_templates" tagprefix="uc" %>


<%@ Register src="modules/fvr_by_template.ascx" tagname="fvr_by_template" tagprefix="uc" %>
<%@ Register Src="~/sections/hr/fvr/modules/tax_entity_fvr_settings.ascx" TagPrefix="uc" TagName="business_unit_fvr_settings" %>

<asp:Content ID="header" ContentPlaceHolderID="header_placeholder" runat="server">
	<style type="text/css">
		.tab
			{
			width:			150px;
			font-size:		11px;
			font-weight:	bold;
			overflow-x:		hidden;
			text-align:		left;
			}
		.content
			{
			min-height:		300px;
			}
		.label_top
			{
			display:		block;
			margin:			5px 0px 5px 0px;		
			}
		.label_oruseexisting
			{
			margin:			5px 0px 5px 0px;
			display:		block;
			}
		.tab_left_col
			{
			float:			left;
			}
		.tab_right_col
			{
			float:			right;
			}
		body
			{
			overflow-y:		scroll;
			}
	</style>
</asp:Content>
<ASP:CONTENT ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</ASP:CONTENT>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript" src="/js/fvr_manage.js"></script>
	<asp:ScriptManager ID="sm" runat="server" EnablePageMethods="True">
	</asp:ScriptManager>
	<script type="text/javascript" src="/js/fvr.js"></script>
	<table>
		<tr>
			<td>
	<dx:ASPxButton ID="b_new" runat="server" AutoPostBack="False" ClientInstanceName="b_new" Text="New" Width="175px" Theme="NETheme01">
		<ClientSideEvents Click="function(s, e) {
	pop_new.Show();
	pop_new.PerformCallback('clear');

}" />
		<Image Url="~/images/icon/icon[add].gif">
		</Image>
	</dx:ASPxButton>
			</td>
			<td>
				<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Edit Categories" Width="175px" Theme="NETheme01">
					<ClientSideEvents Click="function(s, e) {
	pop_categories.Show();
}" />
					<Image Url="~/images/icon/icon[edit].gif">
					</Image>
				</dx:ASPxButton>
			</td>
			<td>
				<dx:ASPxButton ID="bt_template" runat="server" AutoPostBack="False" Text="Edit Templates" Width="175px" Theme="NETheme01">
					<ClientSideEvents Click="function(s, e) {
	pop_templates.Show();
}" />
					<Image Url="~/images/icon/icon[edit].gif">
					</Image>
				</dx:ASPxButton></td>
			<td>
				<dx:ASPxButton ID="bt_bytemplate" runat="server" AutoPostBack="False" Text="New FVR using Template" Width="175px" Theme="NETheme01">
					<ClientSideEvents Click="function(s, e) {
	pop_newbytemplate.Show();
}" />
					<Image Url="~/images/icon/icon[edit].gif">
					</Image>
				</dx:ASPxButton>
			</td>
			<td>
				<dx:ASPxButton ID="bt_tesettings" runat="server" AutoPostBack="False" Text="Tax Entity / Business Unit Settings" Width="250px" Theme="NETheme01">
					<ClientSideEvents Click="function(s, e) {
	pop_tesettings.Show();
}" />
					<Image Url="~/images/icon/icon[edit].gif">
					</Image>
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
			
						<dx:ASPxCallback ID="callback_remove" runat="server" ClientInstanceName="callback_remove" oncallback="callback_remove_Callback">
							<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" CallbackComplete="function(s, e) {
	b_refresh_new.ClickInternalButton();
	please_wait(&quot;stop&quot;);
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
	</dx:ASPxCallback>
						<dx:ASPxCallback ID="callback_toggleactive" runat="server" ClientInstanceName="callback_toggleactive" oncallback="callback_toggleactive_Callback">
							<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" CallbackComplete="function(s, e) {
	please_wait(&quot;stop&quot;);
	lb_files.PerformCallback();
	combo_ed_default_file.PerformCallback();
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
	</dx:ASPxCallback>
			
						<dx:ASPxCallback ID="callback_remove_category" runat="server" ClientInstanceName="callback_remove_category" oncallback="callback_remove_Callback">
							<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" CallbackComplete="function(s, e) {
	please_wait(&quot;stop&quot;);
	gv_categories.Refresh();
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
	</dx:ASPxCallback>
						<dx:ASPxCallback ID="callback_setdefault" runat="server" ClientInstanceName="callback_setdefault" oncallback="callback_setdefault_Callback">
							<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;);
}" CallbackComplete="function(s, e) {
	b_refresh_new.ClickInternalButton();
	please_wait(&quot;stop&quot;);
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
	</dx:ASPxCallback>
			
						<table cellspacing='0' cellpadding='5' style='font-size:12px;font-weight:bold;color:#000;text-align:left;border:solid 1px #000;margin:5px;'>
							<tr><td style='background-color:#000;color:#fff;' align='center'>Legend</td></tr>
							<tr><td style='background-color:#C8FFC8'>Active = True / Confirmed = True</td></tr>
							<tr><td style='background-color:#FFC8C8'>Active = True / Confirmed = False</td></tr>
							<tr><td style='background-color:#EEEEEE'>Active = False</td></tr>
						</table>
	<lc:LayoutControl runat="server" id="layout" GridviewID="gv_details" ShowExcelExport="True" ShowPDFExport="True" ShowToggle="True" />
	
	<asp:UpdatePanel ID="up_fvrs" runat="server">
		<ContentTemplate>
			<dx:ASPxGridView ID="gv_details" runat="server" AutoGenerateColumns="False" DataSourceID="sds_details" onhtmldatacellprepared="gv_details_HtmlDataCellPrepared" ClientInstanceName="gv_details" KeyFieldName="id" onhtmleditformcreated="gv_details_HtmlEditFormCreated" onrowupdating="gv_details_RowUpdating" onrowdeleting="gv_details_RowDeleting" onhtmlrowprepared="gv_details_HtmlRowPrepared" oncustomcallback="gv_details_CustomCallback" oncustomjsproperties="gv_details_CustomJSProperties" oncommandbuttoninitialize="gv_details_CommandButtonInitialize" EnableRowsCache="False" Theme="NETheme01">
				<Columns>
					<dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="65px" Caption=" " MinWidth="65" ShowSelectCheckbox="True">
						
						
						<HeaderTemplate>
							<dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" Text="All" Theme="NETheme01">
								<ClientSideEvents CheckedChanged="function(s, e) {
		fvr.toggle_selection(s.GetChecked());
}" />
							</dx:ASPxCheckBox>
						</HeaderTemplate>
						<FooterTemplate>
							<dx:ASPxButton ID="b_deletemultiple" runat="server" AutoPostBack="False" ClientInstanceName="b_deletemultiple" Width="32px" Theme="NETheme01">
								<ClientSideEvents Click="function(s, e) {
	fvr.delete_selection.run();
}" />
								<Image Url="~/images/icon/icon[delete].gif">
								</Image>
							</dx:ASPxButton>
						</FooterTemplate>
					</dx:GridViewCommandColumn>
					<dx:GridViewDataTextColumn Caption="Date Added" FieldName="dt_insert" VisibleIndex="4" Width="150px">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" VisibleIndex="6" Width="150px">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Employee" FieldName="name" VisibleIndex="7">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Category" FieldName="tab" VisibleIndex="8">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Confirmed?" FieldName="confirmed" VisibleIndex="12" Width="75px">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
						<CellStyle HorizontalAlign="Left">
						</CellStyle>
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Last Edit" FieldName="ts" VisibleIndex="13" Width="150px">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Category File" FieldName="filename" VisibleIndex="9" Width="150px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Active" FieldName="active" VisibleIndex="5" Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Req ID" FieldName="req_id" VisibleIndex="2" Width="50px">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Type" FieldName="type" VisibleIndex="3" Width="50px">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Cut By" FieldName="cut_by" VisibleIndex="1">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Uploaded File" FieldName="uploaded_filename" VisibleIndex="10">
						<DataItemTemplate>
							<dx:ASPxHyperLink ID="link" runat="server" Theme="NETheme01" NavigateUrl='<%# string.Format("javascript:preview({0})", Eval("uploaded_file_id")) %>' Text='<%# Eval("uploaded_filename") %>' />
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Upload Required" FieldName="upload_required" VisibleIndex="11">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="HR Status" FieldName="hrstatus" VisibleIndex="14">
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="16">
						<DataItemTemplate>
							<asp:DropDownList ID="combo_status" runat="server" data-detail_id='<%# Eval("id") %>' onchange="fvr.change_status.run(this);" Width="100%" SelectedValue='<%# Eval("status_id") %>'>
								<asp:ListItem Text="Not Set" Value="1" />
								<asp:ListItem Text="Read" Value="2" />
								<asp:ListItem Text="Uploaded" Value="3" />
								<asp:ListItem Text="Reviewed" Value="4" />
								<asp:ListItem Text="Completed" Value="5" />
							</asp:DropDownList>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
				</Columns>
				<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" EnableRowHotTrack="True" />
				<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" EnableRowHotTrack="True" />
				<SettingsPager PageSize="50">
				</SettingsPager>
				<SettingsEditing Mode="EditForm" />
				<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" 
					ShowFilterBar="Visible" ShowFilterRowMenu="True" ShowGroupPanel="True" 
					ShowFooter="True" ColumnMinWidth="25" />
				<SettingsText ConfirmDelete="Are you sure you want to delete this FVR?" />
				<SettingsEditing Mode="EditForm" />
				<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" ShowHeaderFilterButton="True" />
				<SettingsText ConfirmDelete="Are you sure you want to delete this FVR?" />
				<Styles>
					<Header HorizontalAlign="Center">
					</Header>
					<RowHotTrack BackColor="White" Font-Bold="False">
					</RowHotTrack>
				</Styles>
				<Templates>
					<EditForm>
						<table cellpadding="5" cellspacing="0">
							<tr>
								<td width="65" rowspan="4"><button type="button" onclick="fvr.prev();" style="width:100%;height:30px;">&lt;&lt;prev</button></td>
								<td width="75"><b>Employee:</b></td>
								<td width="123"><%# Eval("name") %></td>
								<td rowspan="4" width="65"><button type="button" onclick="fvr.next();" style="width:100%;height:30px;">next &gt;&gt;</button></td>
							</tr>
							<tr>
								<td><b>Active:</b></td>
								<td>
									<dx:ASPxComboBox ID="combo_status" runat="server" Value='<%# Eval("active") %>' Theme="NETheme01">
										<Items>
											<dx:ListEditItem Text="True" Value="True" />
											<dx:ListEditItem Text="False" Value="False" />
										</Items>
									</dx:ASPxComboBox>
								</td>
							</tr>
							<tr>
								<td><b>Expire Date:</b></td>
								<td><dx:ASPxDateEdit runat="server" ID="expire_date"></dx:ASPxDateEdit></td>
							</tr>
							<tr>
								<td class="style1">
									&nbsp;</td>
								<td width="123">
									<table>
										<tr>
											<td>
									<dx:ASPxButton ID="bt_save" runat="server" AutoPostBack="False" Text="Save" Width="100px" Theme="NETheme01">
										<ClientSideEvents Click="function(s, e) {
	gv_details.UpdateEdit();
}" />
										<Image Url="~/images/icon/icon[save].gif">
										</Image>
									</dx:ASPxButton></td>
									<td>
									<dx:ASPxButton ID="bt_cancel" runat="server" AutoPostBack="False" Text="Cancel" Width="100px" Theme="NETheme01">
										<ClientSideEvents Click="function(s, e) {
	gv_details.CancelEdit();
}" />
										<Image Url="~/images/icon/icon[cancel].gif">
										</Image>
									</dx:ASPxButton>
									</td>
										</tr>
									</table>
									<dx:ASPxButton ID="b_refresh_edit" runat="server" ClientInstanceName="b_refresh_edit" ClientVisible="False" Text="Refresh" onclick="b_refresh_Click" Theme="NETheme01">
									</dx:ASPxButton>
								</td>
							</tr>
						</table>
					</EditForm>
				</Templates>
			</dx:ASPxGridView>
			<asp:SqlDataSource ID="sds_details" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL member_fvr_details(@req_member_id)">
				<SelectParameters>
					<asp:SessionParameter Name="@req_member_id" SessionField="fvr_req_member_id" />
				</SelectParameters>
			</asp:SqlDataSource>
	
	<dx:ASPxPopupControl ID="pop_templates" Theme="NETheme01" runat="server" ClientInstanceName="pop_templates" AnimationType="None" HeaderText="Templates" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" Width="950px" CloseAction="CloseButton" Modal="True" PopupAnimationType="None" RenderIFrameForPopupElements="True" ShowPageScrollbarWhenModal="True">
		<ContentCollection>
			<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
				<uc:pop_manage_templates ID="pop_manage_templates" runat="server" />
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>
	<dx:ASPxPopupControl ID="pop_newbytemplate" runat="server"  Theme="NETheme01" ClientInstanceName="pop_newbytemplate" AnimationType="None" HeaderText="New FVR using Template" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" Width="500px" CloseAction="CloseButton" Modal="True" PopupAnimationType="None" RenderIFrameForPopupElements="True" onwindowcallback="pop_newbytemplate_WindowCallback">
		<ClientSideEvents PopUp="function(s, e) {
	s.PerformCallback();
}" />
		<ContentCollection>
			<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
				<uc:fvr_by_template ID="fvr_by_template" runat="server" />
			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>
	<dx:ASPxPopupControl ID="pop_new" runat="server" Theme="NETheme01" 
				ClientInstanceName="pop_new" AnimationType="None" HeaderText="New  FVR" 
				PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" Width="550px" 
				CloseAction="CloseButton" PopupAnimationType="None" 
				onwindowcallback="pop_new_WindowCallback" AllowDragging="True">
		
		<LoadingPanelImage Url="~/images/loading_panel.gif">
		</LoadingPanelImage>
		
<ClientSideEvents Closing="function(s, e) {
	gv_details.Refresh();
}"></ClientSideEvents>

		<ModalBackgroundStyle BackColor="Transparent">
		</ModalBackgroundStyle>
		<SettingsLoadingPanel Text="" />
		<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
			<table style="width:100%;" cellpadding="5" cellspacing="0">
				<tr>
					<td colspan="3" style="width: 325px">
						<dx:ASPxLabel ID="l_error" runat="server" EncodeHtml="False" Font-Bold="True" Font-Size="11px" ForeColor="Red">
						</dx:ASPxLabel>
					</td>
				</tr>
				<tr>
					<td colspan="2">
					<table cellpadding="2" cellspacing="0">
						<tr>
						<td>
						<b>Type of FVR:</b></td>
						<td>
						<dx:ASPxComboBox ID="combo_type" runat="server" AutoPostBack="True" Native="True" onselectedindexchanged="combo_type_SelectedIndexChanged" SelectedIndex="0" Width="175px">
							<ClientSideEvents SelectedIndexChanged="function(s, e) {
	lb_tabs.UnselectAll();
}" />
							<Items>
								<dx:ListEditItem Selected="True" Text="New Hire" Value="NEWHIRE" />
								<dx:ListEditItem Text="Exit" Value="EXIT" />
								<dx:ListEditItem Text="Renew" Value="RENEW" />
							</Items>
						</dx:ASPxComboBox></td>
						</tr></table>
					</td>
					<td>
						&nbsp;</td>
				</tr>
				<tr>
					<td align="center">
						<strong>Business Units(es)</strong></td>
					<td align="center">
						<strong>Employee(s)</strong></td>
					<td align="center">
						<strong>Categories</strong></td>
				</tr>
				<tr>
					<td align="center" valign="top" rowspan="2">
						<dx:ASPxListBox ID="lb_company" runat="server" Theme="NETheme01" AutoPostBack="True" EnableSynchronization="True" Height="400px" onselectedindexchanged="lb_company_SelectedIndexChanged" SelectionMode="CheckColumn" TextField="name" ValueField="id" ValueType="System.Int32" Width="250px" ClientInstanceName="lb_company">
						</dx:ASPxListBox>
					</td>
					<td align="center" rowspan="2" valign="top">
						<dx:ASPxListBox ID="lb_employees" Theme="NETheme01" runat="server" 
							AutoPostBack="True" Height="400px" 
							onselectedindexchanged="lb_employees_SelectedIndexChanged" 
							SelectionMode="CheckColumn" TextField="name" ValueField="id" 
							ValueType="System.Int32" Width="300px" ClientInstanceName="lb_employees" 
							Enabled="False">
							<ClientSideEvents SelectedIndexChanged="function(s, e) {
	if(lb_tabs.GetEnabled())
		{
		e.processOnServer = false;
		}
}" />
						</dx:ASPxListBox>
						<br />
						<dx:ASPxLabel ID="l_totalusers" runat="server" EncodeHtml="False">
						</dx:ASPxLabel>
					</td>
					<td align="center" valign="top">
						<dx:ASPxListBox ID="lb_tabs" Theme="NETheme01" runat="server" ClientInstanceName="lb_tabs" Height="400px" SelectionMode="CheckColumn" Width="250px" AutoPostBack="True" onselectedindexchanged="lb_tabs_SelectedIndexChanged" ValueType="System.Int32" Enabled="False">
						</dx:ASPxListBox>
						<br />
					</td>
				</tr>
				<tr>
					<td align="center" valign="top">
						</td>
				</tr>
				<tr>
					<td colspan="3" align="center">
						<dx:ASPxPageControl ID="pc_content" Theme="NETheme01" runat="server" TabPosition="Left" Width="100%" ClientInstanceName="pc_content">
							<TabStyle CssClass="tab" Height="25px">
							</TabStyle>
							<ContentStyle CssClass="content">
							</ContentStyle>
						</dx:ASPxPageControl>
						</td>
				</tr>
				<tr>
					<td align="center" colspan="3">
						<div style="width:150px;padding-top:20px;">
						<div class="tab_left_col">
						<dx:ASPxButton ID="b_save" runat="server" Theme="NETheme01" onclick="b_save_Click" Text="Save">
							<Image Url="~/images/icon/icon[save].gif">
							</Image>
						</dx:ASPxButton>
						</div>
						<div class="tab_right_col">
						<dx:ASPxButton ID="b_close" runat="server" Theme="NETheme01" Text="Close">
							<ClientSideEvents Click="function(s, e) {
	pop_new.Hide();
}" />
							<Image Url="~/images/icon/icon[popup].gif">
							</Image>
						</dx:ASPxButton></div>
						</div>
						<dx:ASPxButton ID="b_refresh_new" Theme="NETheme01" runat="server" ClientInstanceName="b_refresh_new" ClientVisible="False" onclick="b_refresh_Click" Text="Refresh">
						</dx:ASPxButton>
					</td>
				</tr>
			</table>
			</dx:PopupControlContentControl>
</ContentCollection>
	</dx:ASPxPopupControl>
			<dx:ASPxPopupControl ID="pop_categories" runat="server" Theme="NETheme01" HeaderText="Manage Categories" Modal="True" Width="900px" ClientInstanceName="pop_categories" AnimationType="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" PopupAnimationType="None">
				<ModalBackgroundStyle BackColor="Transparent">
				</ModalBackgroundStyle>
				<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxGridView ID="gv_categories" Theme="NETheme01" runat="server" AutoGenerateColumns="False" DataSourceID="sds_categories" KeyFieldName="id" OnHtmlDataCellPrepared="gv_categories_HtmlDataCellPrepared" OnRowUpdating="gv_categories_RowUpdating" Width="100%" OnHtmlEditFormCreated="gv_categories_HtmlEditFormCreated" ClientInstanceName="gv_categories">
		<Columns>
			<dx:GridViewCommandColumn ButtonType="Image" Caption="Action" ShowInCustomizationForm="True" VisibleIndex="7" Width="50px" ShowEditButton="True">
				
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="0" Visible="False">
				<EditFormSettings Visible="False" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn FieldName="ts" ShowInCustomizationForm="False" VisibleIndex="1" Caption="Last Edited">
				<PropertiesDateEdit DisplayFormatString="g">
				</PropertiesDateEdit>
				<EditFormSettings Visible="False" />
				<CellStyle Font-Size="10px" HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn FieldName="type" ShowInCustomizationForm="True" VisibleIndex="2">
				<EditFormSettings Visible="False" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="tab_index" ShowInCustomizationForm="False" Visible="False" VisibleIndex="3">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Name" FieldName="name" ShowInCustomizationForm="True" VisibleIndex="4" Width="225px">
				<DataItemTemplate>
					<dx:ASPxTextBox ID="ASPxTextBox1"  Theme="NETheme01" runat="server" Text='<%# Eval("name") %>' Width="100%" ClientEnabled="False">
					</dx:ASPxTextBox>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Default File" FieldName="default_file_id" ShowInCustomizationForm="True" VisibleIndex="5">
				<Settings AllowAutoFilter="False" AllowSort="False" FilterMode="DisplayText" ShowInFilterControl="False" />
				<DataItemTemplate>
					<dx:ASPxComboBox ID="combo_files" Theme="NETheme01" runat="server" DataSourceID="sds_files" TextField="name" ValueField="id" Width="100%" ClientEnabled="False">
					</dx:ASPxComboBox>
					<asp:SqlDataSource ID="sds_files" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="select id,CONCAT(name,&quot;.&quot;,ext) name from filestore.files where sub_folder_id &lt; 25 and page_id = 123 and folder_id = if(@type = &quot;NEWHIRE&quot;, 0, if(@type = &quot;EXIT&quot;, 1, IF(@type = &quot;RENEW&quot;, 2, NULL))) AND sub_folder_id = @category ORDER BY name">
						<SelectParameters>
							<asp:Parameter Name="@type" />
							<asp:Parameter Name="@category" />
						</SelectParameters>
					</asp:SqlDataSource>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataCheckColumn Caption="Upload Required?" FieldName="upload_required" ShowInCustomizationForm="True" VisibleIndex="6" Width="100px" ReadOnly="True">
				<PropertiesCheckEdit AllowGrayed="True">
				</PropertiesCheckEdit>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataCheckColumn>
		</Columns>
		<SettingsPager PageSize="15">
		</SettingsPager>
		<SettingsEditing Mode="EditForm" />
                                <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
		<Styles>
			<Header HorizontalAlign="Center">
			</Header>
		</Styles>
		<Templates>
			<EditForm>
				<table style="width:100%;">
					<tr>
						<td valign="top" width="150" rowspan="3">
							<table width="400">
								<tr>
									<td width="150">
										<strong>Name:</strong></td>
									<td>
										<dx:ASPxTextBox ID="text_ed_name" runat="server" Theme="NETheme01" Text='<%# Bind("name") %>' Width="170px">
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td width="150">
										<strong>Default File:</strong></td>
									<td>
										<dx:ASPxComboBox ID="combo_ed_default_file" runat="server" Theme="NETheme01" ClientInstanceName="combo_ed_default_file" DataSourceID="sds_ed_default_file" ondatabound="combo_ed_default_file_DataBound" TextField="name" ValueField="id">
											<ClientSideEvents SelectedIndexChanged="function(s, e) {
	hid_default_file.SetValue(s.GetValue());
}" />
										</dx:ASPxComboBox>
										<asp:SqlDataSource ID="sds_ed_default_file" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="select id,CONCAT(name,&quot;.&quot;,ext) name, IF(active, 'T', 'F') active from filestore.files where sub_folder_id &lt; 25 and page_id = 123 and folder_id = if(@type = &quot;NEWHIRE&quot;, 0, if(@type = &quot;EXIT&quot;, 1, IF(@type = &quot;RENEW&quot;, 2, NULL))) AND sub_folder_id = @category AND active = 1 ORDER BY name">
											<SelectParameters>
												<asp:Parameter Name="@type" />
												<asp:Parameter Name="@category" />
											</SelectParameters>
										</asp:SqlDataSource>
										<asp:SqlDataSource ID="sds_ed_all_file" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="select id,CONCAT(name,&quot;.&quot;,ext) name, IF(active, 'T', 'F') active from filestore.files where sub_folder_id &lt; 25 and page_id = 123 and folder_id = if(@type = &quot;NEWHIRE&quot;, 0, if(@type = &quot;EXIT&quot;, 1, IF(@type = &quot;RENEW&quot;, 2, NULL))) AND sub_folder_id = @category ORDER BY name">
											<SelectParameters>
												<asp:Parameter Name="@type" />
												<asp:Parameter Name="@category" />
											</SelectParameters>
										</asp:SqlDataSource>
										<dx:ASPxTextBox ID="hid_default_file" runat="server" Theme="NETheme01" ClientInstanceName="hid_default_file" ClientVisible="False" Value='<%# Bind("default_file_id") %>' Width="170px">
										</dx:ASPxTextBox>
									</td>
								</tr>
								<tr>
									<td width="150">
										<strong>Upload Required:</strong></td>
									<td>
										<dx:ASPxCheckBox ID="check_ed_uploadreq" Theme="NETheme01" runat="server" CheckState="Unchecked" Value='<%# Bind("upload_required") %>'>
										</dx:ASPxCheckBox>
									</td>
								</tr>
								<tr>
									<td style="font-weight: 700" width="150">
										&nbsp;</td>
									<td valign="top">
										&nbsp;</td>
								</tr>
								<tr>
									<td colspan="2" style="font-weight: 700" width="150">
										<table width="100%">
											<tr>
												<td align="right">
													<dx:ASPxButton ID="bt_save" runat="server" Theme="NETheme01" AutoPostBack="False" Text="Save">
														<ClientSideEvents Click="function(s, e) {
	gv_categories.UpdateEdit();
}" />
														<Image Url="~/images/icon/icon[save].gif">
														</Image>
													</dx:ASPxButton>
												</td>
												<td align="left">
													<dx:ASPxButton ID="bt_cancel" runat="server" Theme="NETheme01" AutoPostBack="False" Text="Cancel">
														<ClientSideEvents Click="function(s, e) {
		gv_categories.CancelEdit();
}" />
														<Image Url="~/images/icon/icon[cancel].gif">
														</Image>
													</dx:ASPxButton>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
						<td colspan="3">
							<strong>Existing Files Linked to this Category</strong><dx:ASPxListBox ID="lb_files" runat="server" ClientInstanceName="lb_files" DataSourceID="sds_ed_all_file" Height="150px" Rows="10" TextField="name" ValueField="id" Width="100%">
								<Columns>
									<dx:ListBoxColumn Caption="File" FieldName="name" Width="85%" />
									<dx:ListBoxColumn Caption="Active" FieldName="active" Width="15%" />
								</Columns>
							</dx:ASPxListBox>
						</td>
					</tr>
					<tr>
						<td width="150" align="center">
							<dx:ASPxButton ID="bt_preview" runat="server" AutoPostBack="False" Theme="NETheme01" Text="Preview File" Width="150px">
								<ClientSideEvents Click="function(s, e) {
	preview(lb_files.GetValue())
}" />
								<Image Url="~/images/icon/icon[zoom].gif">
								</Image>
							</dx:ASPxButton>
						</td>
						<td width="150" align="center">
							<dx:ASPxButton ID="bt_delete" runat="server" AutoPostBack="False" Theme="NETheme01" Text="Remove File" Width="150px">
								<ClientSideEvents Click="function(s, e) {
	remove_file(lb_files.GetValue(), false, true);
}" />
								<Image Url="~/images/icon/icon[delete].gif">
								</Image>
							</dx:ASPxButton>
						</td>
						<td width="150" align="center">
							<dx:ASPxButton ID="bt_toggleactive" runat="server" AutoPostBack="False" Theme="NETheme01" Text="Toggle Active" Width="150px">
								<ClientSideEvents Click="function(s, e) {
	toggle_active_file(lb_files.GetValue());
}" />
								<Image Url="~/images/icon/icon[refresh].gif">
								</Image>
							</dx:ASPxButton>
						</td>
					</tr>
					<tr>
						<td valign="top" colspan="3">
							<strong>
							<br />
							<br />
							Add a new file</strong><dx:ASPxUploadControl ID="uploader" runat="server" FileUploadMode="OnPageLoad" onfileuploadcomplete="upload_handler" ShowProgressPanel="True" ShowUploadButton="True" Width="100%">
								<ValidationSettings AllowedFileExtensions=".pdf, .flv, .f4v, .mp4" MultiSelectionErrorText="Attention! 

The following {0} files are invalid because they exceed the allowed file size ({1}) or their extensions are not allowed. These files have been removed from selection, so they will not be uploaded. 

{2}">
								</ValidationSettings>
								<ClientSideEvents FileUploadComplete="function(s, e) {
	if(e.errorText == &quot;&quot;) 
		{
		gv_categories.Refresh();
		}
}" />
								<Paddings Padding="5px" />
								<Border BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" />
							</dx:ASPxUploadControl>
						</td>
					</tr>
				</table>
			</EditForm>
		</Templates>
	</dx:ASPxGridView>
	<br />
	<asp:SqlDataSource ID="sds_categories" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT * FROM member_fvr_tab WHERE id != 2 ORDER BY type, tab_index, name"></asp:SqlDataSource>
	<br />
					</dx:PopupControlContentControl>
</ContentCollection>
	</dx:ASPxPopupControl>
	<dx:ASPxPopupControl ID="pop_tesettings" runat="server"  Theme="NETheme01" ClientInstanceName="pop_tesettings" AnimationType="None" HeaderText="Tax Entity / Business Unit Settings" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" Width="1024px" CloseAction="CloseButton" Modal="True" PopupAnimationType="None" Height="750px" RenderIFrameForPopupElements="True" OnWindowCallback="pop_tesettings_OnWindowCallback">
		<ContentCollection>
			<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
				<iframe src="./if_settings.aspx" height="750" frameborder="0" width="100%" scrolling="no"></iframe>

			</dx:PopupControlContentControl>
		</ContentCollection>
	</dx:ASPxPopupControl>
	<br />
		</ContentTemplate>
	</asp:UpdatePanel>
	
</asp:Content>
