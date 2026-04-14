<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_inventory_user_controls_orders_tab" Codebehind="orders_tab.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>








<%@ Register Assembly="DevExpress.Xpo.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Xpo" TagPrefix="dx" %>


	
	<span id="wo_selector" runat="server"></span>
	<input type="hidden" id="hidexp" class="hidexp" runat="server"/>
			<dx:ASPxGridView id="gv_orders" runat="server" 
				AutoGenerateColumns="False" KeyFieldName="id" Cursor="pointer" 
				Settings-ShowTitlePanel="true" 
				OnHtmlDataCellPrepared="gv_orders_HtmlDataCellPrepared" 
				ClientInstanceName="gv_orders" 
				OnCustomJSProperties="gv_orders_CustomJSProperties" 
				OnCustomCallback="Load_Layout" >
				<SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control" 
					AutoFilterRowInputDelay="3000" EnableCustomizationWindow="true"/>
				<SettingsPager PageSize="50" AlwaysShowPager="True">
				</SettingsPager>
				<Border BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
				<Columns>
					<dx:GridViewDataTextColumn Caption="" FieldName="master_id" MinWidth="25" VisibleIndex="0" Width="75px">
						<DataItemTemplate>
							<a class="master_id" target="_blank" data-toordermax='<%# Eval("qty_to_order_to_max") %>' href='<%# "/redir.aspx?url=" + HttpUtility.UrlEncode(string.Format("/sections/member/inventory/index.aspx?a=get&tab=G&id={0}", Eval("master_id"))) %>'><%# Eval("master_id") %></a> <img src='/images/icon/icon[print_barcode].gif' alt='Print Barcode' style="cursor:pointer;margin-left:5px;" onclick='bc(<%# Eval("master_id") %>)' title='Print Barcode' width='16' height='16' align='absmiddle' />
						</DataItemTemplate>
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Description" MinWidth="25" FieldName="description" VisibleIndex="1"
						Width="200px">
						<DataItemTemplate>
						<div onmouseover='legend(this, true)' onmouseout='legend(this, false)'><%# Eval("description") %></div>
						</DataItemTemplate>
						<HeaderStyle BackColor="#3399FF" ForeColor="White" />
						<Settings FilterMode="DisplayText" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="On Order" MinWidth="25" FieldName="qty_on_order" VisibleIndex="2"
						Width="75px">
						<Settings AutoFilterCondition="Greater" />
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Required" MinWidth="25" FieldName="qty_required" VisibleIndex="3"
						Width="75px">
						<Settings AutoFilterCondition="Greater" />
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center" BackColor="#C0FFC0">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="To Order" MinWidth="25" FieldName="qty_to_order" VisibleIndex="4"
						Width="50px">
						<Settings AutoFilterCondition="Greater" />
						<DataItemTemplate>
							<input id="t_to_order" runat="server" class="qty_to_order" style="font-size:11px;" onclick='this.select();this.focus();'  value='<%# bind("qty_to_order") %>' size='2' type="text" />
						</DataItemTemplate>
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center" Font-Bold="True">
						</CellStyle>
					</dx:GridViewDataTextColumn>

					<dx:GridViewDataTextColumn Caption="To Order (max)" MinWidth="25" FieldName="qty_to_order_to_max" VisibleIndex="25"
						Width="50px">
						<Settings AutoFilterCondition="Greater" />
						<DataItemTemplate>
							<span><%# Eval("qty_to_order_to_max") %></span>
						</DataItemTemplate>
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center" Font-Bold="True">
						</CellStyle>
					</dx:GridViewDataTextColumn>

					<dx:GridViewDataTextColumn Caption="Stock (int)" MinWidth="25" FieldName="qty_stock_int" VisibleIndex="5"
						Width="80px">
						<Settings AutoFilterCondition="Less" />
						<DataItemTemplate>
							<input type="text" id="t_qty_stock_int" runat="server" class="qty_stock_int" style="font-size:11px;" onclick='this.select();this.focus();'  value='<%# bind("qty_stock_int") %>' size='2'/> 
						</DataItemTemplate>
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Stock (ext)" MinWidth="25" 
						FieldName="qty_stock_ext" VisibleIndex="6"
						Width="80px">
						<Settings AutoFilterCondition="Less" />
						<DataItemTemplate>
							<input type="text" id="t_qty_stock_ext" runat="server" class="qty_stock_ext" style="font-size:11px;" onclick='this.select();this.focus();'  value='<%# bind("qty_stock_ext") %>' size='2'/> 
						</DataItemTemplate>
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Default Vendor" FieldName="vendor_name" 
						VisibleIndex="7" MinWidth="25" Width="50px">
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="PO" MinWidth="25" FieldName="poprog_bvpo" VisibleIndex="8"
						Width="50px">
						<Settings AllowAutoFilter="False" AllowHeaderFilter="False" AllowSort="False" ShowFilterRowMenu="False" />
						<DataItemTemplate>
							<asp:HyperLink ID="link_po" runat="server" NavigateUrl='<%# string.Format("~/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}", Eval("poprog_id")) %>'
								Text='<%# Bind("poprog_bvpo") %>'></asp:HyperLink>
						</DataItemTemplate>
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Location" MinWidth="25" 
						FieldName="location" VisibleIndex="10"
						Width="50px">
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="WOs Requesting Parts" MinWidth="25" 
						FieldName="wo_parts" Name="wo_parts"
						VisibleIndex="11" Width="250px">
						<Settings  AllowGroup="False" 
							SortMode="DisplayText" />
						<HeaderStyle BackColor="#3399FF" ForeColor="White" Wrap="True" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataComboBoxColumn Caption="Process" VisibleIndex="12" 
						MinWidth="25" CellStyle-HorizontalAlign="Center" Width="50px">
						<DataItemTemplate>
							<input type="checkbox" onclick='preload_vendor(this)' runat="server" class='cb_inc' id="cb_inc"/>
						</DataItemTemplate>
						<HeaderStyle BackColor="#33FF99" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
						<HeaderTemplate>
							<div align="center">
							Process<br />
							<input type="checkbox" class='cb_incall' onclick='cb_incall(this)'/>
							</div>
						</HeaderTemplate>
						<FooterTemplate>
							<div align="center">
							<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Width="75" Text="Cut PO's"
								ToolTip="Cut PO's for all selected rows">
								<Image Url="~/images/icon/icon[add].gif">
								</Image>
								<ClientSideEvents Click="cut_visible_pos" />
							</dx:ASPxButton>
							<button type="button" class="dxbButton rfq_panel" onmouseover="sh_rfq_panel(this)" style="width:87px;padding-top:5px;padding-bottom:5px;">RFQ</button>
							</div>
						</FooterTemplate>
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataTextColumn Caption="Vendor" VisibleIndex="13" MinWidth="25"
						Width="125px">
						<Settings AllowAutoFilter="False" AllowHeaderFilter="False" AllowSort="False" />
						<DataItemTemplate>
							<input size="10" id="vendorname" type="text" onclick='this.focus();this.select()' class="vendor_name" data-isac='false' onfocus="attach_ac(this, 'vendor');" data-orders="true" data-master_id='<%# Eval("master_id")%>' data-to_order_qty='<%# Eval("qty_to_order")%>' data-default_vendor_id='<%# Eval("vendor_id") %>' data-cost='<%# Eval("t_cost") %>' data-vendor_code='<%# Eval("vendor_code") %>' data-qty_per='<%# Eval("qty_per") %>' data-lead='<%# Eval("lead") %>' data-_date='<%# Eval("_date") %>' data-default_vendor_name='<%# Eval("vendor_name") %>' data-business_unit_id='<%# Session["working_warehouse_bu_id"] %>' />
						</DataItemTemplate>
						<HeaderStyle BackColor="#33FF99" Font-Bold="True" HorizontalAlign="Center" />
						<CellStyle Font-Bold="False" HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Vend. Part #" VisibleIndex="14" 
						MinWidth="25" Width="125px">
						<Settings AllowAutoFilter="False" AllowHeaderFilter="False" />
						<DataItemTemplate>
							<input size="10" type="text" onclick='this.focus();this.select()' class="vendor_code"/>
						</DataItemTemplate>
						<HeaderStyle BackColor="#33FF99" HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Vend. Price" VisibleIndex="15" 
						MinWidth="25" Width="80px">
						<DataItemTemplate>
							<input class="vendor_price" size="5" onclick='this.focus();this.select()' type="text"/>
						</DataItemTemplate>
						<HeaderStyle BackColor="#33FF99" HorizontalAlign="Center" Wrap="True" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="QTY per" VisibleIndex="16" MinWidth="25" 
						Width="75px">
					    <Settings AllowDragDrop="False" />
						<DataItemTemplate>
							<input size="3" type="text" onclick='this.focus();this.select()' class="vendor_qty"/>
						</DataItemTemplate>
						<HeaderStyle BackColor="#33FF99" HorizontalAlign="Center" Wrap="True" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Vend. Lead Time (d)" VisibleIndex="18" 
						MinWidth="25" Width="50px">
						<Settings AllowDragDrop="False" />
						<DataItemTemplate>
							<input class="vendor_lead" onclick='this.focus();this.select()' size="5" type="text"/>
						</DataItemTemplate>
						<HeaderStyle BackColor="#33FF99" HorizontalAlign="Center" Wrap="True" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Days Until Needed" 
						FieldName="days_until_due" ToolTip="Days until the minimum request date" 
						VisibleIndex="20" Width="60px">
						<Settings AutoFilterCondition="LessOrEqual" />
						<HeaderStyle Wrap="True" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Min Req Date" FieldName="minreqdate" 
						MinWidth="25" VisibleIndex="19" Width="125px">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
						</PropertiesDateEdit>
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn Caption="Est. Delivery" FieldName="delivery_date" 
						MinWidth="25" VisibleIndex="9" Width="50px">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
						</PropertiesDateEdit>
						<DataItemTemplate>
							<dx:ASPxDateEdit ID="delivery_date" runat="server" CssClass="delivery_date" 
								Value='<%# Eval("delivery_date") %>' Width="100px">
							</dx:ASPxDateEdit>
						</DataItemTemplate>
						<HeaderStyle BackColor="#3399FF" ForeColor="White" HorizontalAlign="Center" />
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn Caption="Price Date" VisibleIndex="17" Width="80px">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
						</PropertiesDateEdit>
							
						<DataItemTemplate>
						<input class="vendor_date" onclick='this.focus();this.select()' size="5" type="text" disabled="disabled" />
							
						</DataItemTemplate>
							
						<HeaderStyle BackColor="#33FF99" HorizontalAlign="Center" />
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="Is Consumable" 
						FieldName="is_consumable"
						VisibleIndex="21" Width="60px">
						<HeaderStyle Wrap="True" />
					</dx:GridViewDataTextColumn>
				</Columns>
				<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowFooter="True" ShowFilterBar="Visible" ShowTitlePanel="false" />

        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" 
					CustomizationWindow-VerticalAlign="TopSides">

<CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="TopSides"></CustomizationWindow>
				</SettingsPopup>

				<Styles>
					<Cell Wrap="False" Font-Size="11px">
					</Cell>
					<Header Font-Bold="True" Wrap="True">
					</Header>
					<LoadingDiv Opacity="0">
					</LoadingDiv>
				</Styles>
				<SettingsLoadingPanel Text="Please Wait" />
				<ClientSideEvents Init="gv_orders_init" EndCallback="function(s,e){ gv_orders_init();please_wait('stop');}" />
			</dx:ASPxGridView>
		<asp:SqlDataSource ID="ds_vendor" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT vendor_id, vendor_name FROM vendor WHERE vendor_active = true ORDER BY vendor_name">
		</asp:SqlDataSource>