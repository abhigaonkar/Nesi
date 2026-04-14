<%@ Control Language="C#" AutoEventWireup="true"  Inherits="modules_event_viewer" EnableTheming="True" Codebehind="event_viewer.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>


            

<%@ Register src="layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>



            

			<dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" Text="Enable Timer" 
				CheckState="Unchecked">
				<ClientSideEvents CheckedChanged="function(s, e) {
	timer.SetEnabled(s.GetChecked());
}" />
			</dx:ASPxCheckBox>
			


			 
	<dx:ASPxGridView ID="gv" runat="server" ClientInstanceName="gv" Width ="100%" OnHtmlDataCellPrepared="gv_watch_HtmlDataCellPrepared"
				 AutoGenerateColumns="True" 
				oncustomcallback="gv_CustomCallback"  
				KeyFieldName="source" PreviewFieldName="message" OnRowDataBound="gv_RowDataBound">

		<SettingsBehavior ColumnResizeMode="Control" AllowGroup="False" />
		<SettingsPager Visible="False">
        </SettingsPager>
		<Settings ShowPreview="true" ShowTitlePanel="True" GridLines="Horizontal" />
		<SettingsText Title="Event Viewer" />
		<SettingsLoadingPanel ImagePosition="Top" ShowImage="False" Mode="Disabled" />
		<SettingsSearchPanel Visible="false" />
		<SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
		<Styles>
		</Styles>
	</dx:ASPxGridView>
	<dx:ASPxTimer ID="eventViewerTimer" runat="server" Interval="25000" 
				ClientSideEvents-Tick='function (s,e){gv.PerformCallback("refresh");}' 
				ClientInstanceName="timer" Enabled="False">
	</dx:ASPxTimer>


            

