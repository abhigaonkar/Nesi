<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_messaging_modules_video_view" Codebehind="video_view.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<div style=""></div>
<div style="float:left;width:400px;padding:3px;border: solid 1px #ccc;margin:2px;">
	<div style="font-weight:bold;margin:5px;">Category</div>
	<dx:ASPxComboBox ID="combo_category" runat="server" ValueType="System.Int32" ClientInstanceName="combo_category" Width="100%" DataSourceID="ds_categories1" TextField="name" ValueField="id" oncallback="combo_category_Callback" AutoPostBack="True" onselectedindexchanged="combo_category_SelectedIndexChanged">
		<ClientSideEvents EndCallback="function(s, e) {
	s.SetSelectedIndex(-1);
}" />
	</dx:ASPxComboBox>
<asp:SqlDataSource ID="ds_categories1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id, a.name FROM video_category a RIGHT JOIN video b ON b.category_id = a.id AND b.category_id != 42 AND b.category_id IN (SELECT category_id FROM video_privilege WHERE membertype_id = @membertype_id) GROUP BY id HAVING id IS NOT NULL">
	<SelectParameters>
		<asp:SessionParameter Name="@membertype_id" SessionField="membertype_id" />
	</SelectParameters>
	</asp:SqlDataSource>

	<br />
	<dx:ASPxListBox ID="combo_videos" runat="server" ValueType="System.Int32" ClientInstanceName="combo_videos" Width="100%" Height="300px" TextField="name" ValueField="file_id" oncallback="combo_videos_Callback">
		<ClientSideEvents SelectedIndexChanged="function(s, e) {
	var id		= s.GetValue();
	$(&quot;.right_pane&quot;).html(&quot;&lt;iframe src='/_tools/get_file/index.aspx?file_id=&quot;+id+&quot;&amp;iframe=true' frameborder='0' width='540' height='350' /&gt;&quot;);
}" />
	</dx:ASPxListBox>
</div>
<div style="float:left;width:550px; padding:3px;background:#eee;height:360px; border: solid 1px #ccc;margin:2px;" id="pane_right" runat="server" class="right_pane"></div>
