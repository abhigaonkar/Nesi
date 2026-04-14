<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../../IntraDefault.master'
			AutoEventWireup	= 'true'
			Inherits		= 'inventory_start' 
			Title			= 'Inventory' 
			Validaterequest	= 'false'
 Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>



<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				<div id='divSide' runat='server'>
					<asp:SqlDataSource ID='Users' runat='server'></asp:SqlDataSource>
				</div>
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
				<div id='divMenu' runat='server'></div>
</asp:Content>

<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' Runat='Server'>
	<script type="text/javascript">
		var Branch_DSNS					= new Array(<asp:Literal id="BRANCH_DSNS" runat="server"></asp:Literal>);
		$(document).ready(function()
							{
							var _action			= "<asp:Literal id="ACTION_VAR" runat="server"></asp:Literal>";
							if(_action == "edit_linkage")
								{
								$('#tab_tag').click(function()
														{
														$('.submit').show();
														$('#trade_names').slideUp(150, function()
																							{
																							$('#tab_tag').attr('disabled', true);
																							$('#tab_tradenames').removeAttr('disabled');
																							$('#tag_defaults').slideDown(150);
																							});
														});
								$('#tab_tradenames').click(function()
														{
														$('.submit').hide();
														$('#tag_defaults').slideUp(150, function()
																							{
																							$('#tab_tradenames').attr('disabled', true);
																							$('#tab_tag').removeAttr('disabled');
																							$('#trade_names').slideDown(150);
																							populate_tradenames();
																							});
														});
								}
							});
		function handle_properties_toggle()
			{
			if(!$(".bordered").is(":visible"))
				{
				$('#tag_properties_push').html('<b>^^</b> TAG PROPERTIES <b>^^</b>');
				$('.bordered').fadeIn(0);
				}
			else
				{
				$('#tag_properties_push').html('<b>vv</b> TAG PROPERTIES <b>vv</b>');
				$('.bordered').fadeOut(0);
				}
			}
	</script>
	<script type='text/javascript' src='/js/inventory/_admin.js'>
	</script>
		<div id='inventory'>
			<div id='loading'><img src='/images/loading_panel.gif' alt="loading" /></div>
				<table class='framework' border='0' cellpadding='0' cellspacing='0'>
					<tr>
						<td valign='top'>
							<table id='submenu' border='0' cellpadding='0' cellspacing='0'>
								<tr>
									<td align='center' class='pagetitle' id='page_title' runat='server'></td>
									<td class='buttonbox' width='55'><button onclick="location.href='./index.aspx';" type='button' id='button_home' runat='server' title='Admin Home'><img src='/images/icon/icon[home].gif' alt="" /></button></td>
									<td class='buttonbox' width='55'><button onclick="location.href='./index.aspx?a=tag';" type='button' id='button_tags' runat='server' title='Administer Tags'><img src='/images/icon/icon[tags].gif' alt="" /></button></td>
									<td class='buttonbox' width='55'><button onclick="location.href='./index.aspx?a=att';" type='button' id='button_attributes' runat='server' title='Administer Attributes & Values'><img src='/images/icon/icon[attribute_values].gif' alt="" /></button></td>
									<td class='buttonbox' width='55'><button onclick="location.href='./index.aspx?a=browse';" type='button' id='button_browse' runat='server' title='Part Management'><img src='/images/icon/icon[browse].gif' alt="" /></button></td>
									<td class='buttonbox' width='55'><button onclick="location.href='./index.aspx?a=qc';" type='button' id='button_qc' runat='server' title='Quality Check'><img src='/images/icon/icon[ok].gif' alt="" /></button></td>
									<td class='buttonbox' width='55'><button onclick="location.href='./index.aspx?a=wo_parts';" type='button' id='button_wo_parts' runat='server' title='View Parts Currently on Work Orders'><img src='/images/icon/icon[search].gif' alt="" /></button></td>
									<td class='buttonbox' width='55'><button onclick="location.href='./index.aspx?a=splitparts';" type='button' id='button_split_parts' runat='server' title=''><img src='/images/icon/icon[replicate].gif' alt="" /></button></td>
								</tr>
								<tr>
									<td colspan="8" align="right" style="padding:5px;background-color:#def;"><b>Part Search: </b><input type='text' onkeydown="$(this).inventory({force_clickable: true, click: function(row)
																																															{
																																															location.href		= './index.aspx?a=edit_part&master_id='+row.master_id;
																																															}
																																														});" style="border:solid 1px #000; padding:2px;" /></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td valign='top' align='center' id='detail' runat='server'></td>
					</tr>
				</table>
		<div id='errormsg'>
	<input type="hidden" id="hidexp" class="hidexp" runat="server"/>
							<dx:aspxgridview id="gv_wo_unqced_parts" runat="server" AutoGenerateColumns="False" DataSourceID="ds_wo_unqced_parts" Visible="False" Width="100%">
								<Columns>
									<dx:GridViewDataTextColumn Caption="Part #" FieldName="master_id" VisibleIndex="0">
										<DataItemTemplate>
											<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/admin/inventory/index.aspx?a=edit_part&master_id={0}', 'inventory', 1200,800)&quot;, Eval(&quot;master_id&quot;)) %>"
												Text='<%# Eval("master_id") %>'>
											</dx:ASPxHyperLink>
										</DataItemTemplate>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Tag Name" FieldName="tag_name" VisibleIndex="1">
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Created By" FieldName="created_by" VisibleIndex="2">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="WO" FieldName="wo_num" VisibleIndex="4">
										<DataItemTemplate>
											<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}', 'wo', 1200,800)&quot;, Eval(&quot;wo_id&quot;), Eval(&quot;business_unit_id&quot;)) %>"
												Text='<%# Eval("wo_num") %>'>
											</dx:ASPxHyperLink>
										</DataItemTemplate>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="WO Status" FieldName="wo_status" VisibleIndex="5">
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="WO BU" FieldName="wo_company" VisibleIndex="6">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="PO" FieldName="po_num" VisibleIndex="8">
										<DataItemTemplate>
											<dx:ASPxHyperLink ID="ASPxHyperLink3" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}', 'po', 1200,800)&quot;, Eval(&quot;po_id&quot;)) %>"
												Text='<%# Eval("po_num") %>'>
											</dx:ASPxHyperLink>
										</DataItemTemplate>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="PO Status" FieldName="po_status" VisibleIndex="9">
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="PO BU" FieldName="po_company" VisibleIndex="10">
									</dx:GridViewDataTextColumn>
								</Columns>
								<SettingsBehavior ColumnResizeMode="Control" />
								<SettingsPager PageSize="50">
								</SettingsPager>
								<Settings ShowFilterRow="True" ShowHeaderFilterButton="True" ShowFilterBar="Visible" ShowFilterRowMenu="True" />
							</dx:aspxgridview>
							<asp:SqlDataSource ID="ds_wo_unqced_parts" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
                                 SelectCommand="call report_admin_inv_unqced_wo()"></asp:SqlDataSource>
							<asp:ScriptManager ID="sm" runat="server">
							</asp:ScriptManager>
							<asp:UpdatePanel ID="up" runat="server" ViewStateMode="Enabled">
								<ContentTemplate>
								
	<lc:LayoutControl runat="server" id="layout" />
									<dx:ASPxPanel ID="pane_splitparts" runat="server" Visible="false" Width="100%">
										<PanelCollection>
											<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
												<br />
												<dx:ASPxLabel ID="ASPxLabel1" runat="server" AssociatedControlID="combo_selecttag" Font-Bold="True" Font-Underline="True" Text="Select Tag">
												</dx:ASPxLabel>
												<dx:ASPxComboBox ID="combo_selecttag" runat="server" ClientInstanceName="combo_selecttag" DataSourceID="sds_selecttag" IncrementalFilteringMode="Contains" TextField="tag" ValueField="tag_id">
													<ClientSideEvents SelectedIndexChanged="function(s, e) {
	gv_splitparts.Refresh();
}" />
												</dx:ASPxComboBox>
												<asp:SqlDataSource ID="sds_selecttag" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
                                                    ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
                                                    SelectCommand="SELECT tag_id,URLDECODE(tag) tag FROM inventory_tag WHERE active = true ORDER BY tag"></asp:SqlDataSource>
												<br />
												<dx:ASPxGridView ID="gv_splitparts" runat="server" AutoGenerateColumns="False"
                                                     DataSourceID="sds_splitparts" Width="100%" ClientInstanceName="gv_splitparts" EnableTheming="True"
                                                     OnHtmlDataCellPrepared="gv_splitparts_HtmlDataCellPrepared" CssClass="splitparts" OnDataBinding="gv_splitparts_DataBinding" OnCustomCallback="gv_splitparts_CustomCallback" OnCustomJSProperties="gv_splitparts_CustomJSProperties">
													<ClientSideEvents EndCallback="bind_xfer" />
													<Columns>
														<dx:GridViewCommandColumn ShowInCustomizationForm="False" Visible="false" VisibleIndex="0" ShowClearFilterButton="true">
															
														</dx:GridViewCommandColumn>
														<dx:GridViewDataTextColumn Caption="Master ID" FieldName="master_id" ShowInCustomizationForm="True" VisibleIndex="1" Width="100px">
															<DataItemTemplate>
																<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/admin/inventory/index.aspx?a=edit_part&master_id={0}', 'inventory', 1200,800)&quot;, Eval(&quot;master_id&quot;)) %>" Text='<%# Eval("master_id") %>'>
																</dx:ASPxHyperLink>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="name" ShowInCustomizationForm="True" VisibleIndex="2" Width="150px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn FieldName="vendor_name" CellStyle-CssClass="vendor" ShowInCustomizationForm="True" VisibleIndex="4" Caption="Vendor">
															<DataItemTemplate>
																<div style="cursor:move;padding:2px;overflow:hidden" data-row_id='<%# Eval("id") %>' data-master_id='<%# Eval("master_id") %>' data-vendor_id='<%# Eval("vendor_id") %>' data-business_unit_id='<%# Eval("business_unit_id") %>' title="Drag me to another part to copy vendor pricing." class='move'><img src="/images/icon/icon[arrows].png" align="absmiddle" /><%# Eval("vendor_name") %></div>
															</DataItemTemplate>
															<CellStyle CssClass="vendor" Wrap="False">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn FieldName="vendor_code" ShowInCustomizationForm="True" VisibleIndex="5" Caption="Vendor Part #">
															<Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
															<DataItemTemplate>
																<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" CssClass="vendor_code" AutoResizeWithContainer="True" Text='<%# Eval("vendor_code") %>' Width="100%">
																</dx:ASPxTextBox>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn FieldName="qty" ShowInCustomizationForm="True" VisibleIndex="6" Caption="QTY per part" Width="50px">
															<DataItemTemplate>
																<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" CssClass="qty" AutoResizeWithContainer="True" Text='<%# Eval("qty") %>' Width="100%">
																</dx:ASPxTextBox>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn FieldName="cost" ShowInCustomizationForm="True" VisibleIndex="7" Caption="Cost" Width="50px">
															<DataItemTemplate>
																<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" CssClass="cost" AutoResizeWithContainer="True" Text='<%# Eval("cost") %>' Width="100%">
																</dx:ASPxTextBox>
															</DataItemTemplate>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Open POs" Name="open_pos" ShowInCustomizationForm="True" VisibleIndex="3" Width="150px">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="business_unit_id" FieldName="business_unit_id" ShowInCustomizationForm="False" Visible="False" VisibleIndex="8">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="vendor_id" FieldName="vendor_id" ShowInCustomizationForm="False" Visible="False" VisibleIndex="9">
														</dx:GridViewDataTextColumn>
													</Columns>
													<SettingsPager PageSize="50">
													</SettingsPager>
													<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
                                                    <ClientSideEvents BeginCallback="function(s,e){ please_wait('start');}" EndCallback="function(s,e){ please_wait('stop');}" />


                                                    <SettingsPopup  CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-HorizontalAlign="LeftSides"/>



													<Styles>
														<Cell Wrap="True">
														</Cell>
														<Table Wrap="True">
														</Table>
														<Row CssClass="tr">
														</Row>
													</Styles>
												</dx:ASPxGridView>
												<asp:SqlDataSource ID="sds_splitparts" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
                                                    ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
a.id,
a.master_id, 
c.id business_unit_id,
c.name, 
v.vendor_id,
v.vendor_name vendor_name, 
a.vendor_code vendor_code, 
a.qty, 
a.cost 

FROM inventory_price a 
LEFT JOIN inventory_item_master b ON a.master_id = b.master_id 
LEFT JOIN business_unit c ON a.business_unit_id = c.id 
LEFT JOIN vendor v ON a.vendor_id = v.vendor_id
WHERE 
b.tag_id = @tag_id AND 
b.active = true;">
													<SelectParameters>
														<asp:ControlParameter ControlID="combo_selecttag" Name="@tag_id" PropertyName="Value" />
													</SelectParameters>
												</asp:SqlDataSource>
											</dx:PanelContent>
										</PanelCollection>
									</dx:ASPxPanel>
								</ContentTemplate>
							</asp:UpdatePanel>
							<br />
						</div>
		</div>
</asp:Content>

