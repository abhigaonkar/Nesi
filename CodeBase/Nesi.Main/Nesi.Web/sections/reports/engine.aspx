<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/IntraDefault.master" Inherits="sections_reports_engine" CodeBehind="engine.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/modules/layout_control.ascx" TagPrefix="uc" TagName="layout_control" %>
<asp:Content ContentPlaceHolderID="header_placeholder" runat="server">
	<script type="text/javascript" src="/js/layout_saver.js"></script>
	<script type="text/javascript" src="/js/engine.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphMasterBody" runat="server">
	<div id="div_no_reports" runat="server" visible="false">
		<div>It doesn't look like your account has access to any reports.</div>
	</div>
	<table cellpadding="2" cellspacing="0" width="100%">
		<tr>
			<td style="width: 10%;" valign="top">
				<div style="font-size: 1.6em; margin-bottom: 5px;">Reports</div>
				<dx:ASPxMenu ID="report_menu" runat="server" Orientation="Vertical" >
					<ItemStyle Paddings-Padding="2px"  />
					<ClientSideEvents ItemClick="engine_client.menu_click" />
				</dx:ASPxMenu>
			</td>
			<td style="width: 90%;" valign="top">
				<dx:ASPxCallbackPanel ID="cbp_grid" runat="server" ClientInstanceName="cbp_grid" OnCallback="cbp_grid_Callback">
					<PanelCollection>
						<dx:PanelContent>
							<div id="ReportInfo" runat="server">
								<dx:ASPxHyperLink ID="reportName" runat="server" Text="" ToolTip="Click to get a fresh reload of the grid" Theme="MaterialCompact" ForeColor="Black" Font-Size="Large" NavigateUrl="javascript:void()">
									<ClientSideEvents Click="engine_client.grid.refresh" />
								</dx:ASPxHyperLink>
										
								<input type="hidden" id="hid_layout" runat="server" value="" class="hid_layout" />
								<input type="hidden" id="hid_report_id" runat="server" value="" class="hid_report_id" />
								<input runat="server" id="hid_paras" type="hidden" class="hid_paras" />
								<div align="right">
									<div runat="server" id="permitted_users" style="width: 150px; color: #00437C !important;display:none;" data-title="Permitted Users" class="permitted_users">
										<label for="img_member">Permitted users</label>
										<img src="/images/icon/icon[member].gif" width="16" height="16" id="img_member" alt="Permitted users" />
									</div>
								</div>
							</div>
							<br />
							<br />
							<div id="div_filters" runat="server">
							</div>
							<uc:layout_control runat="server" ID="layout_control" GridviewID="gv" __page_name="Report Engine"></uc:layout_control>
							<dx:ASPxGridView runat="server" ClientInstanceName="gv" Theme="NETheme01" ID="gv" OnCustomJSProperties="gv_CustomJSProperties" OnDataBound="gv_DataBound" OnCustomCallback="gv_CustomCallback">
								<SettingsPager PageSize="50"></SettingsPager>
								<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" ShowFooter="True" ShowTitlePanel="True" ShowHeaderFilterButton="True"></Settings>
								<SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
								<SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit"></SettingsAdaptivity>
								<SettingsPopup>
									<HeaderFilter MinHeight="140px"></HeaderFilter>
								</SettingsPopup>
								<SettingsSearchPanel Visible="True"></SettingsSearchPanel>
								<Styles>
									<Footer HorizontalAlign="Center"></Footer>
									<TitlePanel HorizontalAlign="Left" BackColor="Transparent" ForeColor="Black"></TitlePanel>
								</Styles>
							</dx:ASPxGridView>
						</dx:PanelContent>
					</PanelCollection>
					<ClientSideEvents BeginCallback="engine_client.callback.begin"
						CallbackError="engine_client.callback.error"
						EndCallback="engine_client.callback.end"
						Init="engine_client.callback.init" />
				</dx:ASPxCallbackPanel>
			</td>
		</tr>
	</table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
