<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_messaging_videos" MasterPageFile="~/IntraDefault.master" Codebehind="videos.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register src="modules/video_admin.ascx" tagname="video_admin" tagprefix="uc1" %>
<%@ Register src="modules/video_view.ascx" tagname="video_view" tagprefix="uc2" %>
<%@ Register src="modules/category_admin.ascx" tagname="category_admin" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server"><div id="divMenu" runat="server"></div></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server"><div id="divSide" runat="server"></div></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
			<dx:ASPxPageControl ID="p" runat="server" ActiveTabIndex="0" Width="100%">
				<TabPages>
					<dx:TabPage Name="videos" Text="Videos">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
								<uc2:video_view ID="video_view1" runat="server" />
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Name="admin_category" Text="Category Admin">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
								<uc3:category_admin ID="category_admin1" runat="server" />
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
					<dx:TabPage Name="admin_video" Text="Video Admin">
						<ContentCollection>
							<dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
								<uc1:video_admin ID="video_admin1" runat="server" />
							</dx:ContentControl>
						</ContentCollection>
					</dx:TabPage>
				</TabPages>
				<ClientSideEvents TabClick="function(s, e) {
switch(e.tab.name)
	{
	case &quot;videos&quot;:
		combo_category.PerformCallback();
		combo_videos.PerformCallback();
		combo_videos.SetSelectedIndex(-1);
	break;
	case &quot;admin_category&quot;:
		gv_categories.Refresh();
	break;
	case &quot;admin_video&quot;:
		gv_videos.Refresh();
	break;
	}
}" />
				<clientsideevents tabclick="function(s, e) {
switch(e.tab.name)
	{
	case &quot;videos&quot;:
		combo_category.PerformCallback();
		combo_videos.PerformCallback();
		combo_videos.SetSelectedIndex(-1);
	break;
	case &quot;admin_category&quot;:
		gv_categories.Refresh();
	break;
	case &quot;admin_video&quot;:
		gv_videos.Refresh();
	break;
	}
}" />
			</dx:ASPxPageControl>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>