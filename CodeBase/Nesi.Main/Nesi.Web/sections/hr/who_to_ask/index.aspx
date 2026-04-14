<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="hr_whotoask_index" Title="Who To Ask Admin" EnableTheming="True" Theme="NETheme01" Codebehind="index.aspx.cs" %>


<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>








<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript">

 function resizeIframe(obj)
 {

   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 20) + 'px';
	
	if (obj.contentWindow.document.body.scrollHeight<800)
	{
	obj.style.height='800px';
	}
 }


 </script>

<table width="100%">
<tr>
<td width="100%">
								


	<dx:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
		oncallback="cb_Callback" Width="200px">
	</dx:ASPxCallbackPanel>
								


</td>
</tr>


<tr>
<td width="100%">
								


	<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" 
		ClientInstanceName="gv" DataSourceID="SqlDataSource4" 
		KeyFieldName="who_to_ask_id" Width="100%" onrowdeleting="gv_RowDeleting" 
		onrowinserting="gv_RowInserting">
		
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																	<CancelButton Image-Url="~/images/icon/icon[cancel].gif" Image-Width="16px" Text="Cancel" />
																</SettingsCommandButton>
		<Columns>
			<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0"  ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true" ShowCancelButton="true"
				Width="60px">
				
				
				
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="ID" FieldName="who_to_ask_id" 
				ReadOnly="True" VisibleIndex="1" Visible="False">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Member" FieldName="who_to_ask_memberid" 
				VisibleIndex="3" Width="100px">
				<PropertiesComboBox DataSourceID="SqlDataSource2" TextField="membername" 
					ValueField="member_id" ValueType="System.Int32">
				</PropertiesComboBox>
				<DataItemTemplate>
					<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" 
						DataSourceID="SqlDataSource2" oninit="ASPxComboBox2_Init" 
						TextField="membername" Value='<%#Eval("who_to_ask_memberid") %>' 
						ValueField="member_id" ValueType="System.Int32" Width="100%" EnableCallbackMode="True" 
						IncrementalFilteringMode="StartsWith">
					</dx:ASPxComboBox>
				</DataItemTemplate>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataComboBoxColumn Caption="Member Type" 
				FieldName="who_to_ask_membertype" VisibleIndex="4" Width="100px">
				<PropertiesComboBox DataSourceID="SqlDataSource3" TextField="membertype_name" 
					ValueField="membertype_id" ValueType="System.Int32">
				</PropertiesComboBox>
				<DataItemTemplate>
					<dx:ASPxComboBox ID="ASPxComboBox3" runat="server" 
						DataSourceID="SqlDataSource3" oninit="ASPxComboBox3_Init" 
						TextField="membertype_name" Value='<%# Eval("who_to_ask_membertype") %>' 
						ValueField="membertype_id" ValueType="System.Int32" Width="100%" EnableCallbackMode="True">
					</dx:ASPxComboBox>
				</DataItemTemplate>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="Question" FieldName="who_to_ask_question" 
				VisibleIndex="2" Width="100%">
				<DataItemTemplate>
					<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" oninit="ASPxTextBox1_Init" 
					Text='<%# Eval("who_to_ask_question") %>'
						Width="100%">
					</dx:ASPxTextBox>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<SettingsPager Mode="ShowAllRecords">
		</SettingsPager>
		<SettingsEditing Mode="Inline" />
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="SELECT
who_to_ask.who_to_ask_id,
who_to_ask.who_to_ask_memberid,
who_to_ask.who_to_ask_membertype,
urldecode(who_to_ask.who_to_ask_question) who_to_ask_question
FROM
who_to_ask
"></asp:SqlDataSource>
								


	<asp:SqlDataSource runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="select 0 member_id ,'No One' membername, 1 business_unit_id 
union
Select member_id, concat(get_name(member_id),' - ',business_unit_name(business_unit_id)) membername, business_unit_id from member where member_status = 'Active' order by business_unit_id,get_name(member_id) " 
		ID="SqlDataSource2"></asp:SqlDataSource>
	<asp:SqlDataSource runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="select 0  membertype_id ,'No One' membertype_name
union
SELECT membertype_id, membertype_name FROM membertype WHERE (active = 1) ORDER BY membertype_name" 
		ID="SqlDataSource3"></asp:SqlDataSource>
								


</td>
</tr>


</table>



</asp:Content>

