<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_messaging_modules_category_admin" Codebehind="category_admin.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<p>
	<table>
		<tr>
			<td><b>New Category:</b></td>
			<td><dx:ASPxTextBox ID="tb_categoryname" runat="server" Width="170px"></dx:ASPxTextBox></td>
			<td><dx:ASPxButton ID="bt_savenewcategory" runat="server" Text="Save" onclick="bt_savenewcategory_Click"></dx:ASPxButton></td>
			<td><b style="color:#f00;" runat="server" id="error"></b></td>
		</tr>
	</table>
</p>
<dx:ASPxGridView ID="gv_categories" runat="server" ClientInstanceName="gv_categories" AutoGenerateColumns="False" DataSourceID="ds_categories2" KeyFieldName="id" Width="800px" onrowdeleting="gv_categories_RowDeleting" onrowupdating="gv_categories_RowUpdating" ondetailrowexpandedchanged="gv_categories_DetailRowExpandedChanged" onfocusedrowchanged="gv_categories_FocusedRowChanged">
    <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
    <CancelButton Text="Cancel" Image-Url="~/images/icon/icon[cancel].gif" Image-Width="16px" Image-Height="16px"></CancelButton>
</SettingsCommandButton>
	<Columns>
		<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" ShowInCustomizationForm="False" Visible="False" VisibleIndex="0">
			<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Name" FieldName="name" VisibleIndex="1">
		</dx:GridViewDataTextColumn>
		<dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="2" Width="50px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true" ShowCancelButton="true">
			
			
			
			
		</dx:GridViewCommandColumn>
	</Columns>
	<SettingsBehavior EnableRowHotTrack="True" />
	<SettingsDetail AllowOnlyOneMasterRowExpanded="True" ShowDetailRow="True" />

<SettingsBehavior EnableRowHotTrack="True"></SettingsBehavior>

	<SettingsPager PageSize="50">
	</SettingsPager>

	<Settings ShowTitlePanel="True" />

<SettingsDetail ShowDetailRow="True" AllowOnlyOneMasterRowExpanded="True"></SettingsDetail>

	<Templates>
		<TitlePanel>
			<asp:HiddenField ID="hid_detail" runat="server" />
		</TitlePanel>
		<DetailRow>
			<div style="background-color:#ccc;padding:5px;font-size:14px;font-weight:bold;">Membertype Privileges</div>
			<table width="100%">
				<tr>
					<td width="30%">[<a href="javascript:void(0)" onclick="available_types.SelectAll();">Select All</a>]&nbsp;[<a href="javascript:void(0)" onclick="available_types.UnselectAll();">Unselect All</a>]<br /> <br />
					<dx:ASPxListBox runat="server" ID="available_types" Height="350px" ClientInstanceName="available_types" Width="100%" SelectionMode="CheckColumn" oncallback="available_types_Callback" DataSourceID="ds_available" TextField="name" ValueField="id" ValueType="System.Int32"></dx:ASPxListBox>
						<asp:SqlDataSource ID="ds_available" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT membertype_id id, membertype_name name FROM membertype WHERE membertype_id NOT IN (SELECT membertype_id FROM video_privilege WHERE category_id = @category_id) ORDER BY name">
							<SelectParameters>
								<asp:SessionParameter Name="@category_id" SessionField="vid_category_id" />
							</SelectParameters>
						</asp:SqlDataSource>
					</td>
					<td width="30%" align="center">
						<div>
							<dx:ASPxButton ID="bt_add" runat="server" Text="&gt;&gt; Add" Width="75%" onclick="bt_add_Click"></dx:ASPxButton>
						</div>
						<div>
							
							<dx:ASPxButton ID="bt_remove" runat="server" Text="&lt;&lt; Remove" Width="75%" onclick="bt_remove_Click"></dx:ASPxButton>
						</div>
					</td>
					<td width="30%">[<a href="javascript:void(0)" onclick="selected_types.SelectAll();">Select All</a>]&nbsp;[<a href="javascript:void(0)" onclick="selected_types.UnselectAll();">Unselect All</a>]<br /> <br />
					<dx:ASPxListBox runat="server" ID="selected_types" Height="350px" ClientInstanceName="selected_types" Width="100%" SelectionMode="CheckColumn" oncallback="selected_types_Callback" DataSourceID="ds_selected" TextField="name" ValueField="id" ValueType="System.Int32"></dx:ASPxListBox>
						<asp:SqlDataSource ID="ds_selected" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT membertype_id id, membertype_name name FROM membertype WHERE membertype_id IN (SELECT membertype_id FROM video_privilege WHERE category_id = @category_id) ORDER BY name">
							<SelectParameters>
								<asp:SessionParameter Name="@category_id" SessionField="vid_category_id" />
							</SelectParameters>
						</asp:SqlDataSource>
					</td>
				</tr>
			</table>
			<div style="background-color:#ccc;padding:5px;font-size:14px;font-weight:bold;">Page Links</div>
			<table width="100%">
				<tr>
					<td width="30%">[<a href="javascript:void(0)" onclick="available_pages.SelectAll();">Select All</a>]&nbsp;[<a href="javascript:void(0)" onclick="available_pages.UnselectAll();">Unselect All</a>]<br /> <br />
					<dx:ASPxListBox runat="server" ID="available_pages" Height="350px" ClientInstanceName="available_pages" Width="100%" SelectionMode="CheckColumn" DataSourceID="ds_available_pages" TextField="name" ValueField="id" ValueType="System.Int32"></dx:ASPxListBox>
						<asp:SqlDataSource ID="ds_available_pages" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT page_id id, if(page_parent_id = 0, CONCAT('[',page_name,']'), CONCAT('[',(SELECT page_name FROM page WHERE page_id = a.page_parent_id),'] - ',a.page_name)) name FROM page a WHERE page_id NOT IN (SELECT page_id FROM video_page_lnk WHERE category_id = @category_id) ORDER BY name">
							<SelectParameters>
								<asp:SessionParameter Name="@category_id" SessionField="vid_category_id" />
							</SelectParameters>
						</asp:SqlDataSource>
					</td>
					<td width="30%" align="center">
						<div>
							<dx:ASPxButton ID="bt_add_page" runat="server" Text="&gt;&gt; Add" Width="75%" onclick="bt_addpage_Click"></dx:ASPxButton>
						</div>
						<div>
							
							<dx:ASPxButton ID="bt_remove_page" runat="server" Text="&lt;&lt; Remove" Width="75%" onclick="bt_removepage_Click"></dx:ASPxButton>
						</div>
					</td>
					<td width="30%">[<a href="javascript:void(0)" onclick="selected_pages.SelectAll();">Select All</a>]&nbsp;[<a href="javascript:void(0)" onclick="selected_pages.UnselectAll();">Unselect All</a>]<br /> <br />
					<dx:ASPxListBox runat="server" ID="selected_pages" Height="350px" ClientInstanceName="selected_pages" Width="100%" SelectionMode="CheckColumn" DataSourceID="ds_selected_pages" TextField="name" ValueField="id" ValueType="System.Int32"></dx:ASPxListBox>
						<asp:SqlDataSource ID="ds_selected_pages" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT page_id id, if(page_parent_id = 0, CONCAT('[',page_name,']'), CONCAT('[',(SELECT page_name FROM page WHERE page_id = a.page_parent_id),'] - ',a.page_name)) name FROM page a WHERE page_id IN (SELECT page_id FROM video_page_lnk WHERE category_id = @category_id) ORDER BY name">
							<SelectParameters>
								<asp:SessionParameter Name="@category_id" SessionField="vid_category_id" />
							</SelectParameters>
						</asp:SqlDataSource>
					</td>
				</tr>
			</table>
		</DetailRow>
	</Templates>
</dx:ASPxGridView>
<asp:SqlDataSource ID="ds_categories2" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, name FROM video_category"></asp:SqlDataSource>

