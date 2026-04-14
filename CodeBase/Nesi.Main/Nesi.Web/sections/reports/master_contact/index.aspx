<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true"  Inherits="master_contact" Title="Master Contact Grid" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<script type="text/javascript" language="javascript">
		function resizeIframe(obj) {
			obj.style.height = (obj.contentWindow.document.body.scrollHeight + 30) + 'px';

		}
	</script>
	<lc:LayoutControl runat="server" ID="layout" __is_private="True" />
	<dx:ASPxGridView ID="gv_MasterContacts" runat="server" 
		AutoGenerateColumns="False" ClientInstanceName="gv_MasterContacts" 
		KeyFieldName="Contact_ID" OnCustomCallback="gv_MasterContacts_CustomCallback" 
		OnCustomJSProperties="gv_MasterContacts_CustomJSProperties" Font-Names="Arial" 
		Font-Size="9pt" Width="100%" 
		OnHtmlRowPrepared="gv_MasterContacts_HtmlRowPrepared" Theme="NETheme01">
		<Templates>
			<TitlePanel>
				<div style="width:150px;border:solid 1px #000;">
				<div style="color:#fff;background-color:#000;font-weight:bold;padding:2px;text-align:center;">Legend</div>
				<div style="color:#f00;font-weight:bold;padding:2px;">NESI Login Enabled</div>
				</div>
			</TitlePanel>
			<EditForm>
				<iframe id="editframe" runat="server" width="100%" frameborder="0"></iframe>
				<br />
			</EditForm>
		</Templates>
		<GroupSummary>
			<dx:ASPxSummaryItem DisplayFormat="Contacts:{0}" FieldName="Contact_ID" ShowInColumn="Customer Name" SummaryType="Count" />
			<dx:ASPxSummaryItem DisplayFormat="Contacts:{0}" FieldName="Contact_ID" ShowInColumn="Vendor Name" SummaryType="Count" />
		</GroupSummary>
		<Columns>
			<dx:GridViewDataTextColumn Caption="ID" FieldName="contact_id" ReadOnly="True" Visible="False" VisibleIndex="0" Width="1px" ShowInCustomizationForm="False">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Name" FieldName="contact_name" VisibleIndex="1" Width="150px">
				<Settings AutoFilterCondition="Contains" />
				<CellStyle Font-Bold="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Email" FieldName="contact_email" VisibleIndex="2" Width="100px">
				<Settings AutoFilterCondition="Contains" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Title" FieldName="contact_title" VisibleIndex="3" Width="100px">
				<Settings AutoFilterCondition="Equals" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Cell" FieldName="contact_cellphone" VisibleIndex="4" Width="75px">
				<Settings AutoFilterCondition="Contains" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Ext" FieldName="contact_extension" VisibleIndex="5" Width="40px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Login" FieldName="contact_login" VisibleIndex="6" Width="50px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Pass" FieldName="contact_password" ReadOnly="True" Visible="False" VisibleIndex="7" Width="50px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Type" FieldName="contact_type" VisibleIndex="8" Width="50px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Status" FieldName="contact_status_id" VisibleIndex="9" Width="50px">
				<PropertiesComboBox DataSourceID="sql_status" TextField="customer_or_contact_status" ValueField="customer_or_contact_status_id" ValueType="System.Int32">
				</PropertiesComboBox>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataCheckColumn Caption="Login Enabled" FieldName="contact_login_enabled" VisibleIndex="10" Width="50px">
				<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
				</PropertiesCheckEdit>
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="chk_loginenabled" runat="server" CheckState="Unchecked" Value='<%# Eval("contact_login_enabled") %>' ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0" oncustomjsproperties="chk_CustomJSProperties">
										<ClientSideEvents CheckedChanged="function(s, e) {
	cb_action.PerformCallback(&quot;login_enabled|&quot;+s.cpID+&quot;|&quot;+s.GetChecked());
}" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataTextColumn Caption="Direct Line" FieldName="contact_directline" VisibleIndex="11" Width="100px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Customer Name" FieldName="customer_name" VisibleIndex="13" Width="150px">
				<PropertiesTextEdit DisplayFormatString="{0}" Width="200px">
				</PropertiesTextEdit>
				<DataItemTemplate>
					<asp:HyperLink ID="hl_customer" runat="server" Font-Bold="True" Font-Size="11px" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/customer/index.aspx?customer_id={0}','customer',1035,800);&quot;,Eval(&quot;customer_id&quot;)) %>" Target="_blank" Text='<%# Eval("customer_name").ToString() %>' Width="100%"></asp:HyperLink>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Vendor Name" FieldName="vendor_name" VisibleIndex="12" Width="100px">
				<Settings AutoFilterCondition="Contains" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataCheckColumn Caption="Stop Surveys" VisibleIndex="14" FieldName="stopsurveys" Width="50px">
				<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
				</PropertiesCheckEdit>
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="chk_stopsurveys" runat="server" CheckState="Unchecked" Value='<%# Eval("stopsurveys") %>' ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0" oncustomjsproperties="chk_CustomJSProperties">
						<ClientSideEvents CheckedChanged="function(s, e) {
	cb_action.PerformCallback(&quot;stop_surveys|&quot;+s.cpID+&quot;|&quot;+s.GetChecked());
}" />
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataTextColumn Caption="customer_id" FieldName="customer_id" 
				Visible="False" VisibleIndex="20" ShowInCustomizationForm="False">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Address" FieldName="address_addr1" Visible="False" VisibleIndex="15">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="City" FieldName="address_city" Visible="False" VisibleIndex="16">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Prov/State" FieldName="address_prov" Visible="False" VisibleIndex="17">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Project Mgr" FieldName="project_mgr" Visible="False" VisibleIndex="18">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" Visible="False" VisibleIndex="19">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Work Orders" FieldName="wos" 
				VisibleIndex="21" Width="75px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Quotes" FieldName="quotes" 
				VisibleIndex="22" Width="75px">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control" AutoFilterRowInputDelay="6000" EnableCustomizationWindow="True"/>
		
        

		<SettingsPager AlwaysShowPager="True" NumericButtonCount="5" PageSize="100" Position="TopAndBottom">
			<AllButton Text="All">
			</AllButton>
		</SettingsPager>
		
        <SettingsEditing Mode="PopupEditForm" />
		<SettingsText CommandUpdate="Save" PopupEditFormCaption="Edit Contact" />
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True" ShowTitlePanel="True" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
       
        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" 
			CustomizationWindow-VerticalAlign="TopSides" EditForm-Height="700px" 
			EditForm-HorizontalAlign="WindowCenter" EditForm-Modal="True" 
			EditForm-VerticalAlign="WindowCenter" EditForm-Width="1100px">

<EditForm Width="1100px" Height="700px" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Modal="True"></EditForm>

<CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="TopSides"></CustomizationWindow>
		</SettingsPopup>

		<Border BorderColor="Silver" />
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="Sqlmembers" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand='SELECT Member_ID, member_fullname as Member FROM activememberforandy where (membertype_name = "Project Manager" or membertype_name = "Branch Manager" or membertype_name = "Regional Manager" or membertype_name = "Programmer" or membertype_name = "Inside Sales" or membertype_name = "Automation Project Manager") and member_id <> 674 and member_id <>753'></asp:SqlDataSource>
	<asp:SqlDataSource ID="sql_status" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT * FROM customer_or_contact_status"></asp:SqlDataSource>
	<asp:SqlDataSource ID="sqlcontact_history" runat="server"></asp:SqlDataSource>
	<dx:ASPxCallback ID="cb_action" runat="server" ClientInstanceName="cb_action" oncallback="cb_action_Callback">
		<ClientSideEvents BeginCallback="function(s, e) {
	please_wait('start');
}" EndCallback="function(s, e) {
	please_wait('stop');
}" CallbackComplete="function(s, e) {
	if(e.result != null)
		{
		alert(e.result);
		}
	else
		{
		gv_MasterContacts.Refresh();		
		}
}" />
	</dx:ASPxCallback>
	<br />
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="header_placeholder">
</asp:Content>
