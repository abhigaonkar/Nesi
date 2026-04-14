<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_messaging_modules_video_admin" Codebehind="video_admin.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<p>
	<dx:ASPxButton ID="bt_new" runat="server" AutoPostBack="False" Text="Add Video">
		<ClientSideEvents Click="function(s, e) {
	boing(&quot;./modules/upload.aspx&quot;, &quot;upload_video&quot;, 500, 350);
}" />
		<Image Height="16px" Url="~/images/icon/icon[add].gif" Width="16px">
		</Image>
	</dx:ASPxButton>
</p>
<script type="text/javascript">
	function toggle_welcome(obj)
		{
		var is_selected		= $(obj).is(":checked") ? "checked" : "unchecked";
		gv_videos.PerformCallback("togglevideo|"+$(obj).val()+"|"+is_selected);
		}
</script>
<dx:ASPxGridView ID="gv_videos" runat="server" ClientInstanceName="gv_videos" AutoGenerateColumns="False" DataSourceID="ds_videos" KeyFieldName="id" EnableRowsCache="False" onrowdeleting="gv_videos_RowDeleting" onrowupdating="gv_videos_RowUpdating" onhtmldatacellprepared="gv_videos_HtmlDataCellPrepared" oncustomcallback="gv_videos_CustomCallback">
    <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
        <CancelButton Text="Delete" Image-Url="~/images/icon/icon[cancel].gif" Image-Width="16px" Image-Height="16px"></CancelButton>
</SettingsCommandButton>
	<Columns>
		<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" VisibleIndex="0">
			<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Name" FieldName="name" VisibleIndex="2" Width="200px">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Link" VisibleIndex="4" Name="link" Width="50px">
			<EditFormSettings Visible="False" />
			<DataItemTemplate>
				<a href="javascript:void(0)"  onclick='boing("/_tools/get_file/index.aspx?file_id=<%# Eval("file_id") %>&iframe=true", "", 600, 440)'><img src='/images/icon/icon[link].gif' alt="View Video" width="16" height="16" /></a>
			</DataItemTemplate>
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataMemoColumn Caption="Description" FieldName="description" VisibleIndex="3" Width="300px">
			<PropertiesMemoEdit Columns="50" Rows="10">
			</PropertiesMemoEdit>
		</dx:GridViewDataMemoColumn>
		<dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="6" Width="50px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true" ShowCancelButton="true">
			
			
			
			
		</dx:GridViewCommandColumn>
		<dx:GridViewDataComboBoxColumn Caption="Category" FieldName="category_id" VisibleIndex="1" Width="250px">
			<PropertiesComboBox DataSourceID="ds_categories3" TextField="name" ValueField="id">
			</PropertiesComboBox>
		</dx:GridViewDataComboBoxColumn>
		<dx:GridViewDataTextColumn Caption="Selected" Name="selected" VisibleIndex="5" Width="50px">
			<EditFormSettings Visible="False" />
			<DataItemTemplate>
				<input type="checkbox"  onchange="toggle_welcome(this)" value='<%# Eval("id") %>' <%# Eval("is_selected").ToString() == "True" ? "checked" : "" %> />
			</DataItemTemplate>
			<CellStyle HorizontalAlign="Center">
			</CellStyle>
		</dx:GridViewDataTextColumn>
	</Columns>
	<SettingsPager PageSize="50">
	</SettingsPager>
</dx:ASPxGridView>
<asp:SqlDataSource ID="ds_videos" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id, a.name, a.description, a.file_id, a.category_id, a.is_selected FROM video a"></asp:SqlDataSource>

<asp:SqlDataSource ID="ds_categories3" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT id, name FROM video_category"></asp:SqlDataSource>
			

