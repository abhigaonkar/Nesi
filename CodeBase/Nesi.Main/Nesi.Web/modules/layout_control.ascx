<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_layout_control" EnableTheming="True" Codebehind="layout_control.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Panel ID="panel_export" runat="server" Visible="False">
	<dx:ASPxHiddenField ID="h" runat="server" ClientInstanceName="h" SyncWithServer="true">
	</dx:ASPxHiddenField>
	<script type="text/javascript" src="/js/layout_saver.js"></script>
	<table cellpadding="2" cellspacing="0" width="100%" bgcolor="#DFDFDF" style="padding: 2px; border-color: #DFDFDF;">
		<thead style="background:#fff;" id="CorrectedLayouts" runat="server" Visible="False">
			<tr>
				<td colspan="3"></td>
				<td>
					<div style="color:#f00;text-align: right;padding:5px;">
						It was detected that you had layouts which were saved under an incorrect id - they have all been moved under the correct id.<br/>
						You may notice that there are possible duplicates in the layout selector below.<br/>
						Please take a moment and prune whichever layouts are no longer needed.
					</div>
				</td>
			</tr>
		</thead>
		<tbody>
			<tr>
				<td align="left" colspan="1" style="width: 5%">
					<dx:ASPxButton ID="bt_toggle_col" runat="server" AutoPostBack="False" Text="Toggle Columns" Width="130px" HorizontalAlign="Center" ToolTip="Use this to hide and show different columns.. don't forget to save once you've made the view you like." Theme="NETheme01" Height="25px">
						<Image Url="~/images/icon/icon[browse].gif" />
						<ClientSideEvents Click="layout_obj.gv_toggle_handler" />
					</dx:ASPxButton>
				</td>
				<td align="center" style="width: 5%">
					<dx:ASPxButton ID="btn_excel" runat="server" OnClick="btn_excel_Click" Text="Export to Excel" Width="130px" ClientVisible="False" HorizontalAlign="Center" Theme="NETheme01" Height="25px">
						<Image Url="~/images/icon/icon[excel].gif" />
					</dx:ASPxButton>
				</td>
				<td style="width: 130px" bgcolor="#DFDFDF">
					<dx:ASPxButton ID="btn_pdf" runat="server" OnClick="btn_pdf_Click" Text="Export to PDF" Width="130px" ClientVisible="False" Height="25px" HorizontalAlign="Center" Theme="NETheme01">
						<Image Url="~/images/icon/icon[pdf].gif" />
					</dx:ASPxButton>
				</td>
				<td align="right" bgcolor="#DFDFDF">
					<dx:ASPxDropDownEdit ID="dde_filter" runat="server" ClientInstanceName="dde_filter" DropDownWindowWidth="500px" AnimationType="None" NullText="Layout Name" Spacing="0" Width="450px" Font-Names="Arial" Height="25px" ToolTip="Use this tool to save your filtered view from below... or pull from someone elses view" Theme="NETheme01">
						<ButtonStyle ForeColor="Black" ImageSpacing="5px">
						</ButtonStyle>
						<DropDownWindowTemplate>
							<dx:ASPxGridView ID="gv_templates" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_templates" Cursor="pointer" DataSourceID="ds_templates" OnCustomJSProperties="gv_filters_CustomJSProperties" Width="100%" OnInit="gv_templates_Load">
								<ClientSideEvents RowClick="layout_obj.row_click_handler" />
								<Columns>
									<dx:GridViewDataTextColumn Caption="Default?" VisibleIndex="1">
										<Settings AllowDragDrop="False" />
										<DataItemTemplate>
											<input id="group1" type="radio" data-id="<%# Eval("gridviewlayouts_id") %>" style="<%# Eval("shown") %>" onclick="layout_obj.selection_handler(this)" name="default_group" <%# Eval("checked") %> />
										</DataItemTemplate>
										<CellStyle HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="ID" FieldName="GridviewLayouts_id" Name="id" Visible="False" VisibleIndex="0" Width="1%">
										<Settings AllowHeaderFilter="True" FilterMode="DisplayText" />
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Owner" FieldName="owner" VisibleIndex="3" Width="40%">
										<FilterCellStyle HorizontalAlign="Left">
										</FilterCellStyle>
										<CellStyle Wrap="False" HorizontalAlign="Center">
										</CellStyle>
										<Settings AllowHeaderFilter="True" FilterMode="DisplayText" AllowDragDrop="False" />
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Layout Name" FieldName="GridviewLayouts_Name" Name="name" VisibleIndex="5" Width="60%">
										<Settings AllowHeaderFilter="False" AllowDragDrop="False" />
										<CellStyle HorizontalAlign="Left">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn FieldName="GridviewLayout_Layout" Name="layout" Visible="False" VisibleIndex="4">
									</dx:GridViewDataTextColumn>
								</Columns>
								<SettingsBehavior EnableRowHotTrack="True" AllowSelectSingleRowOnly="True" />
								<SettingsPager NumericButtonCount="5" PageSize="20">
								</SettingsPager>
								<Settings ShowHeaderFilterButton="True" />
								<SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" >
									<CustomizationWindow HorizontalAlign="LeftSides" />
								</SettingsPopup>
							</dx:ASPxGridView>
							&nbsp;
						</DropDownWindowTemplate>
						<DropDownButton Width="20px">
						</DropDownButton>
						<Buttons>
							<dx:EditButton Text="Save" ToolTip="Save Current Layout" Width="50px">
								<Image Url="~/images/icon/icon[save].gif" />
							</dx:EditButton>
							<dx:EditButton Text="New" ToolTip="Clear current layout, starting a new one." Width="50px">
								<Image Url="~/images/icon/icon[newitem].gif" />
							</dx:EditButton>
							<dx:EditButton Text="Copy">
								<Image Url="~/images/icon/icon[copy].gif" />
							</dx:EditButton>
							<dx:EditButton Text="Delete">
								<Image Url="~/images/icon/icon[delete].gif" />
							</dx:EditButton>
						</Buttons>
						<Paddings Padding="8px" />
						<NullTextStyle ForeColor="Silver">
						</NullTextStyle>
						<ClientSideEvents ButtonClick="layout_obj.gv_save" />
					</dx:ASPxDropDownEdit>
				</td>
			</tr>
		</tbody>
	</table>
	<dx:ASPxCallback ID="cb_save_template" runat="server" ClientInstanceName="cb_save_template" OnCallback="cb_save_template_Callback" OnCustomJSProperties="cb_save_template_CustomJSProperties">
		<ClientSideEvents 
			BeginCallback		= "layout_obj.save_template_callback.begin" 
			EndCallback			= "layout_obj.save_template_callback.end" 
			CallbackError		= "layout_obj.save_template_callback.error" 
			CallbackComplete	= "layout_obj.save_template_callback.complete" />
	</dx:ASPxCallback>
	<asp:SqlDataSource ID="ds_templates" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" InsertCommand="SELECT * FROM gridviewlayouts WHERE gridviewlayouts_gridid = @page_name" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
	gridviewlayouts_id, 
	b.member_fullname owner,
	GridviewLayouts_Name GridviewLayouts_Name,
	GridviewLayouts_member_id,
	GridviewLayout_Layout,
	IF(is_default = TRUE AND gridviewlayouts_member_id = @member_id, 'CHECKED=CHECKED', '') checked,
	IF(gridviewlayouts_member_id = @member_id, 'display:block', 'display:none') shown,
	is_default
FROM 
	gridviewlayouts a
LEFT JOIN
	member b ON a.GridviewLayouts_member_id = b.member_id
WHERE 
	(GridviewLayouts_GridID = @page_name OR gridviewlayouts_gridid = @gv_id) AND
	TRIM(b.member_fullname) != ''
ORDER BY 
	owner ASC, is_default DESC, Gridviewlayouts_name" OnInit="ds_templates_Init">
		<SelectParameters>
			<asp:Parameter Name="@page_name" />
			<asp:Parameter Name="@member_id" />
			<asp:Parameter Name="@gv_id" />
		</SelectParameters>
	</asp:SqlDataSource>
	<dx:ASPxGridViewExporter ID="master_exporter" runat="server" FileName="" OnRenderBrick="master_exporter_RenderBrick">
	</dx:ASPxGridViewExporter>
</asp:Panel>
