<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="taskmanagerlist	" Title="Task Manager" Codebehind="list.aspx.cs" %>


<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>







<%@ Register src="../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>


 



 <asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">

	<dx:ASPxCallbackPanel ID="cb_chkprivate" runat="server" 
						ClientInstanceName="cb_chkprivate" oncallback="cb_chkprivate_Callback">
		<LoadingPanelStyle HorizontalAlign="Center" VerticalAlign="Middle">
		</LoadingPanelStyle>
		<PanelCollection>
			<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>

	<script type="text/javascript">

 function resizeIframe(obj)
 {

   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 10) + 'px';
	
 }

 function Onddlstatus_Init(s, e) {
    ChangeComboColors(); 

}

function ChangePriorityComboColors(s)
{
if (s.GetValue()=="1")
	{
	s.GetMainElement().style.backgroundColor="white";
	s.GetInputElement().style.backgroundColor="white";
	}
	else if (s.GetValue()=="2")
	{
	s.GetMainElement().style.backgroundColor="red";
	s.GetInputElement().style.color="white";
	s.GetInputElement().style.backgroundColor="red";
	}
	else if (s.GetValue()=="3")
	{
	s.GetMainElement().style.backgroundColor="darkorange";
	s.GetInputElement().style.backgroundColor="darkorange";
	}
	else if (s.GetValue()=="4")
	{
	s.GetMainElement().style.backgroundColor="khaki";
	s.GetInputElement().style.backgroundColor="khaki";
	}
	else if (s.GetValue()=="6")
	{
	s.GetMainElement().style.backgroundColor="lightyellow";
	s.GetInputElement().style.backgroundColor="lightyellow";
	}
		else if (s.GetValue()=="5")
	{
	s.GetMainElement().style.backgroundColor="palegreen";
	s.GetInputElement().style.backgroundColor="palegreen";
	}

}
 function ChangeStatusComboColors(s) 
 {
	if (s.GetValue()=="1")
	{
	s.GetMainElement().style.backgroundColor="white";
	s.GetInputElement().style.backgroundColor="white";
	}
	else if (s.GetValue()=="2")
	{
	s.GetMainElement().style.backgroundColor="lightgreen";
	s.GetInputElement().style.backgroundColor="lightgreen";
	}
	else if (s.GetValue()=="3")
	{
	s.GetMainElement().style.backgroundColor="green";
	s.GetInputElement().style.backgroundColor="green";
	s.GetInputElement().style.color="white";
	}
	else if (s.GetValue()=="4")
	{
	s.GetMainElement().style.backgroundColor="lightyellow";
	s.GetInputElement().style.backgroundColor="lightyellow";
	}
	else if (s.GetValue()=="5")
	{
	s.GetMainElement().style.backgroundColor="blue";
	s.GetInputElement().style.backgroundColor="blue";
	s.GetInputElement().style.color="white";
	}
}


 </script>

<table width="100%">
<tr>
<td width="100%">
	<asp:SqlDataSource ID="sql_priority" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select * from ticketpriority order by ticketpriority_name">
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sql_members" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select member_id, get_name (member_id) from member">
	</asp:SqlDataSource>
	<asp:SqlDataSource runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
        Select id, ddl_name name from business_unit  where find_in_set(id,@visible_business_unit_ids)
        " ID="SqlDataSource5">
	    <SelectParameters>
	        <asp:SessionParameter SessionField="visible_business_unit_ids" Name="@visible_business_unit_ids"  />
	    </SelectParameters>

	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sql_task_status" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select * from task_status"></asp:SqlDataSource>


	<lc:LayoutControl runat="server" id="layout" GridviewID="gv" />
	<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" 
		ClientInstanceName="gv" KeyFieldName="id" Width="100%" 
		onhtmleditformcreated="gv_HtmlEditFormCreated" 
		oncancelrowediting="gv_CancelRowEditing" oncustomcallback="gv_CustomCallback" 
		oncustomjsproperties="gv_CustomJSProperties" Font-Names="Arial" 
		onstartrowediting="gv_StartRowEditing" oninitnewrow="gv_InitNewRow" 
		onhtmldatacellprepared="gv_HtmlDataCellPrepared">
        <SettingsCommandButton>
            <EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
            <NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
            <DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
        </SettingsCommandButton>
		<Columns>
			<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" ShowEditButton="true" ShowClearFilterButton="true"
				Width="20px">
				
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" 
				VisibleIndex="1" Width="35px">
				<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Name" FieldName="name1" VisibleIndex="2" 
				Width="200px">
				<Settings AllowSort="True" FilterMode="DisplayText" />
<Settings FilterMode="DisplayText" AllowSort="True"></Settings>
				<DataItemTemplate>
					<dx:ASPxTextBox ID="txtname" runat="server" oninit="txtname_Init" Width="100%"
					 Value='<%#Eval("name1") %>' ClientInstanceName="txtname" BackColor="#EBEBEB">
						<Border BorderStyle="None" />
					</dx:ASPxTextBox>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Owner" FieldName="owner" 
				VisibleIndex="3" Width="100px">
				<PropertiesComboBox DataSourceID="sql_members" TextField="get_name (member_id)" 
					ValueField="member_id" ValueType="System.Int32" CallbackPageSize="10" 
					AnimationType="None" EnableCallbackMode="True">
				</PropertiesComboBox>
				<DataItemTemplate>
					<dx:ASPxComboBox ID="ddlowner" runat="server" DataSourceID="sql_members" 
						oninit="ddlowner_Init" TextField="get_name (member_id)" 
						Value='<%# Eval("owner") %>' ValueField="member_id" 
						ValueType="System.Int32" Width="100%" CallbackPageSize="10" AnimationType="None" 
						EnableCallbackMode="True">
					</dx:ASPxComboBox>
				</DataItemTemplate>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataDateColumn Caption="Started" FieldName="date_started" 
				VisibleIndex="4" Width="90px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd" Width="75px">
				</PropertiesDateEdit>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataDateColumn Caption="Completed" FieldName="date_completed" 
				VisibleIndex="5" Width="90px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd" Width="75px">
				</PropertiesDateEdit>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataComboBoxColumn Caption="Status" FieldName="status" 
				VisibleIndex="6" Width="75px">
				<PropertiesComboBox DataSourceID="sql_task_status" TextField="status" 
					ValueField="id" ValueType="System.Int32">
				</PropertiesComboBox>
				<DataItemTemplate>
					<dx:ASPxComboBox ID="ddlstatus" runat="server" DataSourceID="sql_task_status" 
						oninit="ddlstatus_Init" TextField="status" 
						Value='<%# Eval("status") %>' ValueField="id" 
						ValueType="System.Int32" Width="100%" ClientInstanceName="ddlstatus">
						<ClientSideEvents Init="function(s, e) {
	ChangeStatusComboColors(s)
}" ValueChanged="function(s, e) {
	ChangeStatusComboColors(s)
}"/>
					</dx:ASPxComboBox>
				</DataItemTemplate>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataComboBoxColumn Caption="Cut By" FieldName="cutby" 
				VisibleIndex="7" Width="100px">
				<PropertiesComboBox DataSourceID="sql_members" TextField="get_name (member_id)" 
					ValueField="member_id" ValueType="System.Int32" CallbackPageSize="10" 
					AnimationType="None" EnableCallbackMode="True">
				</PropertiesComboBox>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" VisibleIndex="8" 
				Width="200px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Parent" FieldName="parent_task" 
				Visible="False" VisibleIndex="9" Width="75px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataCheckColumn Caption="Private" FieldName="private" 
				VisibleIndex="10" Width="40px">
				<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" 
					ValueUnchecked="0">
				</PropertiesCheckEdit>
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="dtl_chkprivate" runat="server" 
						OnInit="dtl_chkprivate_Init" 
						Value='<%# Eval("private") %>' CheckState="Unchecked" ValueChecked="1" 
						ValueType="System.Int32" ValueUnchecked="0">
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataDateColumn Caption="Due" FieldName="date_due" VisibleIndex="11" 
				Width="90px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd" Width="75px">
				</PropertiesDateEdit>
				<DataItemTemplate>
					<table style="border-width: 0px; margin: 0px; padding: 0px; width: 100%;">
						<tr>
							<td>
								<dx:ASPxLabel ID="lbldue" runat="server" ClientInstanceName="lbldue" 
									Text='<%# Eval("date_due1") %>'>
								</dx:ASPxLabel>
							</td>
							<td>
								<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" 
									Height="15px" oninit="ASPxButton1_Init" Text="Change">
								</dx:ASPxButton>
							</td>
							<td>
								&nbsp;</td>
						</tr>
					</table>
				</DataItemTemplate>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataCheckColumn Caption="Recurring" FieldName="recurring" 
				VisibleIndex="12" Width="35px">
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataTextColumn Caption="Frequency" FieldName="recurring_frequency" 
				VisibleIndex="13" Width="40px">
				<HeaderStyle Wrap="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Days Warning" 
				FieldName="notify_warning_days" VisibleIndex="16" Width="40px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataCheckColumn Caption="Send Emails" FieldName="emails" 
				VisibleIndex="17" Width="35px">
			<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" 
					ValueUnchecked="0">
				</PropertiesCheckEdit>
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="dtl_chkemails" runat="server" 
						OnInit="dtl_chkmessagboard_Init" 
						Value='<%# Eval("emails") %>' CheckState="Unchecked" ValueChecked="1" 
						ValueType="System.Int32" ValueUnchecked="0">
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataCheckColumn Caption="Use Messageboard" FieldName="messageboard" 
				VisibleIndex="18" Width="35px">
			<PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" 
					ValueUnchecked="0">
				</PropertiesCheckEdit>
				<DataItemTemplate>
					<dx:ASPxCheckBox ID="dtl_chkmessageboard" runat="server" 
						OnInit="dtl_chkmessagboard_Init" 
						Value='<%# Eval("messageboard") %>' CheckState="Unchecked" ValueChecked="1" 
						ValueType="System.Int32" ValueUnchecked="0">
					</dx:ASPxCheckBox>
				</DataItemTemplate>
			</dx:GridViewDataCheckColumn>
			<dx:GridViewDataComboBoxColumn Caption="Resolution" 
				FieldName="recurring_resolution" VisibleIndex="20" Width="50px">
				<PropertiesComboBox>
					<Items>
						<dx:ListEditItem Text="Days" Value="1" />
						<dx:ListEditItem Text="Months" Value="2" />
						<dx:ListEditItem Text="Years" Value="3" />
						<dx:ListEditItem Text="Quarter" Value="4" />
					</Items>
				</PropertiesComboBox>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataComboBoxColumn Caption="Priority" FieldName="priority" 
				VisibleIndex="21" Width="75px">
				<PropertiesComboBox DataSourceID="sql_priority" TextField="ticketpriority_name" 
					ValueField="ticketpriority_id" ValueType="System.Int32">
				</PropertiesComboBox>
				<DataItemTemplate>
					<dx:ASPxComboBox ID="ddlpriority" runat="server" DataSourceID="sql_priority" 
						oninit="ddlpriority_Init" TextField="ticketpriority_name" 
						Value='<%# Eval("priority") %>' ValueField="ticketpriority_id" 
						ValueType="System.Int32" Width="100%">
						<ClientSideEvents Init="function(s, e) {
	ChangePriorityComboColors(s)
}" ValueChanged="function(s, e) {
	ChangePriorityComboColors(s)
}"/>
					</dx:ASPxComboBox>
				</DataItemTemplate>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="dept" 
				VisibleIndex="23" Width="100px">
				<PropertiesComboBox DataSourceID="SqlDataSource5" TextField="name" 
					ValueField="id" ValueType="System.Int32">
				</PropertiesComboBox>
				<DataItemTemplate>
					<dx:ASPxComboBox ID="ddldept" runat="server" ClientInstanceName="ddldept" 
						DataSourceID="SqlDataSource5" oninit="ddldept_Init" TextField="name" 
						Value='<%# Eval("dept") %>' ValueField="id" ValueType="System.Int32" 
						Width="100%">
					</dx:ASPxComboBox>
				</DataItemTemplate>
			</dx:GridViewDataComboBoxColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<Settings ShowFilterRow="True" ShowGroupPanel="True" ShowTitlePanel="True" 
			GridLines="Vertical" ShowFilterBar="Visible" ShowFilterRowMenu="True" 
			ShowHeaderFilterButton="True" />

<SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>

<SettingsEditing Mode="PopupEditForm" ></SettingsEditing>

<SettingsPopup EditForm-Width="1000px" 
			EditForm-Height="630px" EditForm-HorizontalAlign="WindowCenter" 
			EditForm-VerticalAlign="WindowCenter"/>

<Settings ShowTitlePanel="True" ShowFilterRow="True" ShowFilterRowMenu="True" 
			ShowHeaderFilterButton="True" ShowGroupPanel="True" ShowFilterBar="Visible" 
			GridLines="Vertical"></Settings>

		<Styles>
			<Header BackColor="#FFFF66">
			</Header>
			<FilterRow BackColor="#FFFF66">
			</FilterRow>
			<HeaderPanel BackColor="#FFFF66">
			</HeaderPanel>
			<FilterBar BackColor="#FFFF66">
			</FilterBar>
		</Styles>
        <StylesPopup EditForm-Header-BackColor="#CC3300" EditForm-Header-ForeColor="White" EditForm-Header-Font-Bold="true" EditForm-Header-Font-Names="Arial" EditForm-Header-Font-Size="18pt"
        EditForm-MainArea-Paddings-Padding="10px"/>
		<Templates>
			<TitlePanel>
				<table style="width:100%;">
					<tr>
						<td>
							<dx:ASPxButton ID="btnAdd" runat="server" HorizontalAlign="Center" 
								Text="Add New" VerticalAlign="Middle" AutoPostBack="False" ClientInstanceName="btnAdd" 
								Height="70px">
								<ClientSideEvents Click="function(s, e) {
	gv.AddNewRow();
}" />
								<Image Url="~/images/icon/icon[add].gif">
								</Image>
							</dx:ASPxButton>
						</td>
						<td>
							<dx:ASPxRadioButtonList ID="rdoowners" runat="server" 
								ClientInstanceName="rdoowners" SelectedIndex="0" TextWrap="False">
								<ClientSideEvents ValueChanged="function(s, e) {
	gv.PerformCallback('none');
}" />
								<Items>
									<dx:ListEditItem Selected="True" Text="Show All" Value="0" />
									<dx:ListEditItem Text="My Tasks" Value="1" />
									<dx:ListEditItem Text="Tasks I'm Part of" Value="2" />
								</Items>
							</dx:ASPxRadioButtonList>
						</td>
						<td>
							<dx:ASPxRadioButtonList ID="rdodue" runat="server" ClientInstanceName="rdodue" 
								SelectedIndex="0" TextWrap="False">
								<ClientSideEvents ValueChanged="function(s, e) {
	gv.PerformCallback('none');
}" />
								<Items>
									<dx:ListEditItem Selected="True" Text="Show All" Value="0" />
									<dx:ListEditItem Text="Due This Week" Value="1" />
									<dx:ListEditItem Text="Within Warning Days" Value="2" />
								</Items>
							</dx:ASPxRadioButtonList>
						</td>
						<td valign="top" width="60%">
							<dx:ASPxCheckBox ID="chklst_showold" runat="server" AllowGrayedByClick="False" 
								CheckState="Unchecked" ClientInstanceName="chklst_showold" 
								Text="Show Old Tasks" ValueChecked="1" ValueType="System.Int32" 
								ValueUnchecked="0">
								<ClientSideEvents CheckedChanged="function(s, e) {
	gv.PerformCallback('none');
}" />
							</dx:ASPxCheckBox>
						</td>
					</tr>
				</table>
			</TitlePanel>
			<EditForm>
				<iframe ID="I1" runat="server" allowtransparency="true" frameborder="0" Scrolling="no"  
					marginheight="0" name="I1" width="100%" height="650"></iframe>
			</EditForm>
		</Templates>
	</dx:ASPxGridView>
	<br />
	<dx:ASPxPopupControl ID="pop_dd" runat="server" ClientInstanceName="pop_dd" 
		CloseAction="CloseButton" Font-Names="Arial" HeaderText="Change Due Date" 
		Modal="True" PopupHorizontalAlign="WindowCenter" 
		PopupVerticalAlign="WindowCenter">
		<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<table style="width: 100%; font-family: Arial;">
		<tr>
			<td colspan="2" nowrap="nowrap">
				<dx:ASPxLabel ID="lbl_dd_changeError" runat="server" 
					ClientInstanceName="lbl_dd_changeError" Font-Bold="True" Font-Names="Arial" 
					ForeColor="Red" Width="100%">
				</dx:ASPxLabel>
			</td>
		</tr>
		<tr>
			<td nowrap="nowrap">
				<dx:ASPxHiddenField ID="hdd" runat="server" ClientInstanceName="hdd">
				</dx:ASPxHiddenField>
			</td>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td nowrap="nowrap">
				New Due Date:</td>
			<td>
				<dx:ASPxDateEdit ID="dte_dd" runat="server" ClientInstanceName="dte_dd" 
					DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd">
				</dx:ASPxDateEdit>
			</td>
		</tr>
		<tr>
			<td nowrap="nowrap" valign="top">
				Reason For Date Change:</td>
			<td>
				<dx:ASPxTextBox ID="txtwhychange" runat="server" BackColor="#FFFFCC" 
					ClientInstanceName="txtwhychange" Height="50px" Width="250px">
				</dx:ASPxTextBox>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
			<td>
				&nbsp;</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxButton ID="btnsavedue" runat="server" ClientInstanceName="btnsavedue" 
					OnClick="btnsavedue_Click" Text="Save">
					<ClientSideEvents Click="function(s, e) {
	cb_chkprivate.PerformCallback();
pop_dd.Hide();
}" />
				</dx:ASPxButton>
			</td>
			<td>
				&nbsp;</td>
		</tr>
	</table>
			</dx:PopupControlContentControl>
</ContentCollection>
	</dx:ASPxPopupControl>
</td>
</tr>


</table>
</asp:Content>


