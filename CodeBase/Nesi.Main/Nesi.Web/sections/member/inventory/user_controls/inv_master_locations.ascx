<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_inventory_master_locations" Codebehind="inv_master_locations.ascx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

	


	

	

	
<script>
	function copy_location(obj)
		{
		var _id			= $(obj).attr("data-id");
		pop_copy.Show();
		pop_copy.PerformCallback(_id);
		}
	function view_location(obj)
		{
		var _id			= $(obj).attr("data-id");
		pop_view.Show();
		pop_view.PerformCallback(_id);
		}
	function merge_location(obj)
		{
		var _id			= $(obj).attr("data-id");
		pop_merge.Show();
		pop_merge.PerformCallback(_id);
		}
</script>
<table id='location_master' cellpadding='2' cellspacing='0'>
	<thead>
		<tr>
			<th class='cname' align='left'>Name</th>
			<th class='ctype' align='center'>Type</th>
			<th class='caction' align='center'>&nbsp;</th>
		</tr>
		<tr class='add_line tr'>
			<td class='cname' align='center' ><input type='text' class='name' /></td>
			<td class='ctype' align='center' style='width:210px;'><select class='type'><option value='1'>Internal</option><option value='2'>External</option></select></td>
			<td class='caction' align='center'><button type='button' onclick='save_location(this, true);'><img src='/images/icon/icon[save].gif' align='absmiddle' /> Save</button></td>
		</tr>
	</thead>
	<tbody>
	</tbody>
</table>
<br />
	<asp:SqlDataSource ID="sds_master_locations"  ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"  runat="server" SelectCommand="
SELECT 
a.id,
CONCAT(IF(a.type_id = 1, 'Inl - ', 'Exl - '), a.name) name,
a.name barename,
a.type_id, 
(SELECT COUNT(id) FROM inventory_location WHERE location_master_id = a.id) n_parts,
(SELECT COUNT(member_default_location) FROM member WHERE member_default_location = a.id) n_users,
(SELECT COUNT(location_master_id) FROM poprog_header WHERE location_master_id = a.id AND poprog_status IN (1,2,3,4,5,9)) n_pos
FROM inventory_location_master a WHERE a.business_unit_id = @bu_id ORDER BY name">
		<SelectParameters>
			<asp:SessionParameter Name="@bu_id" SessionField="working_warehouse_bu_id" />
		</SelectParameters>
	</asp:SqlDataSource>
	<p>
		<b>Note</b><br />
		- Copy &amp; View buttons will only be available for locations with current part mappings.<br />
		- Delete buttons will only be enabled for locations with no current part mappings.<br />
		- Copy function is only available for locations of the same type (Internal to Internal, External to External).</p>
	<dx:ASPxGridView ID="gv_master_locations" runat="server" 
		AutoGenerateColumns="False" DataSourceID="sds_master_locations" Width="100%" 
		ClientInstanceName="gv_master_locations"  KeyFieldName="id" 
		oncustomcallback="gv_master_locations_CustomCallback" PreviewFieldName="id" onhtmldatacellprepared="gv_master_locations_HtmlDataCellPrepared">
		<Columns>
			<dx:GridViewCommandColumn VisibleIndex="0" Caption=" " 
				ShowSelectCheckbox="True" Width="40px" ShowClearFilterButton="true" >
				
				<HeaderTemplate>
					<dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" 
						Text="Select All">
						<ClientSideEvents CheckedChanged="function(s, e) {
	if (s.GetChecked())
	gv_master_locations.SelectAllRowsOnPage();
	else
	gv_master_locations.UnselectAllRowsOnPage();

}" />
					</dx:ASPxCheckBox>
				</HeaderTemplate>
				<FooterTemplate>
					<dx:ASPxButton ID="btnPrintLocations" runat="server" AutoPostBack="False" 
						ClientInstanceName="btnPrintLocations" HorizontalAlign="Center" Width="25px">
						<ClientSideEvents Click="function(s, e) {
	gv_master_locations.PerformCallback('print_bc');
}" />
						<Image Url="~/images/icon/icon[print_barcode].GIF">
						</Image>
					</dx:ASPxButton>
				</FooterTemplate>
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="ID" Visible="True" FieldName="id" VisibleIndex="1" Width="5%">
				<CellStyle HorizontalAlign="Center" Font-Bold="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Name" FieldName="name" VisibleIndex="2">
				<DataItemTemplate>
					<input type="text" value='<%# HttpUtility.HtmlEncode(Eval("barename")) %>' style="width:100%;font-family:arial;font-size:12px;" class="name" data-id='<%# Eval("id") %>' />
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Left">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Type" FieldName="type_id" VisibleIndex="3" Width="210">
				<PropertiesComboBox>
					<Items>
						<dx:ListEditItem Text="Internal" Value="1" />
						<dx:ListEditItem Text="External" Value="2" />
					</Items>
				</PropertiesComboBox>
				<DataItemTemplate>
					<asp:DropDownList ID="DropDownList2" runat="server" CssClass="type" SelectedValue='<%# Bind("type_id") %>' Width="100%">
						<asp:ListItem Value="1">Internal</asp:ListItem>
						<asp:ListItem Value="2">External</asp:ListItem>
					</asp:DropDownList>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<HeaderStyle HorizontalAlign="Center">
				</HeaderStyle>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="Action" VisibleIndex="4" Width="200px">
				<DataItemTemplate>
					<button type='button' onclick='save_location(this, true);'><img src='/images/icon/icon[save].gif' align='absmiddle'
					 /></button><button type='button' onclick='delete_location(this, true);' ID="delete_button" runat="server"><img src='/images/icon/icon[delete].gif' align='absmiddle'
					 /></button><button type='button' onclick='copy_location(this, true);' data-id='<%# Eval("id") %>' id="copy_button" runat="server"><img src='/images/icon/icon[copy].gif' align='absmiddle'
					 /></button><button type='button' onclick='merge_location(this);' data-id='<%# Eval("id") %>' ID="merge_button" runat="server"><img src='/images/icon/icon[transfer].gif' align='absmiddle'
					 /></button><button type='button' title='View Parts Attached to this Location' onclick='view_location(this, true);' data-id='<%# Eval("id") %>' id="view_button" runat="server"><img src='/images/icon/icon[zoom].gif' align='absmiddle' /></button>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<HeaderStyle HorizontalAlign="Center">
				</HeaderStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Parts" FieldName="n_parts" Visible="False" VisibleIndex="5">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior EnableRowHotTrack="True" />
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" 
			ShowHeaderFilterButton="True" ShowFilterBar="Auto" ShowFooter="True" ShowTitlePanel="True" />
		<Styles>
			<Row CssClass="tr">
			</Row>
		</Styles>
		<Templates>
			<TitlePanel>
				<div align="right">
				<dx:ASPxButton ID="bt_zero_minmax" runat="server" Text="Reset ALL Min/Max Levels to Zero" AutoPostBack="False" Width="150px">
					<ClientSideEvents Click="ne_inv_branch.reset_all_minmax" />
					<Image Url="~/images/icon/icon[warning].gif">
					</Image>
				</dx:ASPxButton></div>
			</TitlePanel>
		</Templates>
	</dx:ASPxGridView>
	<dx:aspxgridviewexporter id="gve_master_locations" gridviewid="gv_master_locations" runat="server"></dx:aspxgridviewexporter><br /><br />
	<dx:aspxbutton id="bt_export" runat="server" text="Export" onclick="bt_export_Click" image-url="/images/icon/icon[excel].gif" image-height="16px" image-width="16px" >

	</dx:aspxbutton>
<dx:ASPxPopupControl ID="pop_copy" runat="server" ClientInstanceName="pop_copy" HeaderText="Copy Location" Height="486px" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"  Width="850px" onwindowcallback="pop_copy_WindowCallback" AllowDragging="True">
	<ClientSideEvents Closing="function(s, e) {
		gv_master_locations.Refresh();
	
}" />
	<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
			<table>
				<tr>
					<td><dx:ASPxLabel ID="lb_which_location" runat="server" AssociatedControlID="l_which_location" style="font-weight: 700" Text="Template Location: " Width="150px"></dx:ASPxLabel></td>
					<td><dx:ASPxLabel ID="l_which_location" runat="server" AssociatedControlID="lb_which_location"></dx:ASPxLabel></td>
				</tr>
				<tr>
					<td><dx:ASPxLabel ID="lb_to_location" runat="server" AssociatedControlID="ddl_destinationlocation" style="font-weight: 700" Text="Destination Location: " Width="150px"></dx:ASPxLabel></td>
					<td><dx:ASPxComboBox ID="ddl_destinationlocation" runat="server" ValueType="System.String" DataSourceID="sds_locations" TextField="name" ValueField="id" ClientInstanceName="ddl_destinationlocation"></dx:ASPxComboBox>
						<asp:SqlDataSource ID="sds_locations" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id, CONCAT(IF(a.type_id = 1, 'Int - ' , 'Ext - '), a.name) name FROM inventory_location_master a where  a.id != @inv_master_location_id AND a.business_unit_id = @bu_id AND name != '0.0.0' AND type_id = (SELECT type_id FROM inventory_location_master WHERE id = @inv_master_location_id) order by a.type_id, a.name">
							<SelectParameters>
								<asp:SessionParameter DefaultValue="" Name="@inv_master_location_id" SessionField="inv_master_location_id" />
								<asp:SessionParameter Name="@bu_id" SessionField="working_warehouse_bu_id" />
							</SelectParameters>
						</asp:SqlDataSource></td>
				</tr>
				<tr>
					<td colspan="2">
						<dx:ASPxButton ID="bt_copy_loc" runat="server" AutoPostBack="False" Text="Copy Links" Width="150px">
							<ClientSideEvents Click="function(s, e) {
	if(ddl_destinationlocation.GetValue() == null)
		{
		alert(&quot;Please choose a destination location&quot;);
		ddl_destinationlocation.Focus();
		}
	else
		{
		if(confirm(&quot;Are you sure you want to do this? This is an undoable procedure.&quot;))
			{
			cb_copy_location.PerformCallback(ddl_destinationlocation.GetValue());
			}
		}
}" />
							<Image Url="~/images/icon/icon[copy].gif">
							</Image>
						</dx:ASPxButton>
						<dx:ASPxCallback ID="cb_copy_location" runat="server" ClientInstanceName="cb_copy_location" oncallback="cb_copy_location_Callback" style="margin-bottom: 0px">
							<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;, &quot;Copying part mappings&quot;);
}" CallbackComplete="function(s, e) {
	please_wait(&quot;stop&quot;);
	if(e.result != &quot;SUCCESS&quot;)
		{
		alert(e.result);
		}
	else
		{
		alert(&quot;Successfully copied mappings&quot;);
		}
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
						</dx:ASPxCallback>
					</td>
				</tr>
			</table>
											
								
											<br />
											All of these part mappings will be copied into the destination location.<br /> If a part exists in both the template location and the destination location, that part will not be affected.
								<dx:ASPxGridView ID="gv_current_mappings" runat="server" AutoGenerateColumns="False" DataSourceID="sds_current_mappings" KeyFieldName="id" Width="100%">
									<Columns>
										<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" VisibleIndex="0" Visible="False">
											<EditFormSettings Visible="False" />
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="master_id" VisibleIndex="1" Caption="Master ID" Width="10%">
											<DataItemTemplate>
												<dx:ASPxHyperLink ID="hl" runat="server" NavigateUrl='<%# string.Format("javascript:boing(\"/sections/member/inventory/index.aspx?a=get&tab=G&&id={0}\", \"quote\", 1200,730)", Eval("master_id")) %>' Text='<%# Eval("master_id") %>' />
											</DataItemTemplate>
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="descr" VisibleIndex="2" Caption="Description" Width="75%">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="qty" VisibleIndex="3" Caption="Qty" Width="5%">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="min" VisibleIndex="4" Caption="MIN" Width="5%">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="max" VisibleIndex="5" Caption="MAX" Width="5%">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsBehavior EnableRowHotTrack="True" />
									<SettingsPager PageSize="16">
									</SettingsPager>
									<SettingsText EmptyDataRow="No parts are linked to this location." />
								</dx:ASPxGridView>

		</dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>

								<asp:SqlDataSource ID="sds_current_mappings" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id,a.master_id,b.description descr, a.qty, a.min, a.max FROM inventory_location a LEFT JOIN inventory_description b ON a.master_id = b.master_id where  a.location_master_id= @inv_master_location_id order by master_id;">
									<SelectParameters>
										<asp:SessionParameter DefaultValue="" Name="@inv_master_location_id" SessionField="inv_master_location_id" />
									</SelectParameters>
											</asp:SqlDataSource>
<dx:ASPxPopupControl ID="pop_view" runat="server" ClientInstanceName="pop_view" HeaderText="View Links" Height="486px" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"  Width="850px" onwindowcallback="pop_view_WindowCallback" AllowDragging="True">
	<ClientSideEvents Closing="function(s, e) {
		gv_master_locations.Refresh();
	
}" />
	<ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
											<dx:ASPxLabel ID="lb_view_which_location" runat="server" AssociatedControlID="l_view_which_location" style="font-weight: 700" Text="Template Location: ">
								</dx:ASPxLabel>
								<dx:ASPxLabel ID="l_view_which_location" runat="server" AssociatedControlID="lb_view_which_location">
								</dx:ASPxLabel>
											<br />
											<dx:ASPxGridViewExporter ID="b_gv_export" runat="server" GridViewID="gv_current_mappings">
											</dx:ASPxGridViewExporter>
											<dx:ASPxButton ID="bt_gv_export" runat="server" onclick="bt_gv_export_Click" Text="Export">
												<Image Url="~/images/icon/icon[excel].gif">
												</Image>
											</dx:ASPxButton>
											<dx:ASPxButton ID="bt_delete_loc" runat="server" AutoPostBack="False" Text="Delete 0 Quantity Links" Width="200px">
												<ClientSideEvents Click="function(s, e) {
		if(confirm(&quot;Are you sure you want to delete all location links with a zero quantity? This is an undoable procedure.&quot;))
			{
			cb_delete_location.PerformCallback();
			}
}" />
												<Image Url="~/images/icon/icon[copy].gif">
												</Image>
											</dx:ASPxButton>
											<dx:ASPxCallback ID="cb_delete_location" runat="server" ClientInstanceName="cb_delete_location" oncallback="cb_delete_location_Callback" style="margin-bottom: 0px">
												<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;, &quot;Deleting part links&quot;);
}" CallbackComplete="function(s, e) {
	please_wait(&quot;stop&quot;);
	if(e.result != &quot;SUCCESS&quot;)
		{
		alert(e.result);
		gv_view_current_mappings.Refresh();
		}
	else
		{
		alert(&quot;Successfully deleted links&quot;);
		gv_view_current_mappings.Refresh();
		}
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
											</dx:ASPxCallback>
								<br />
								<dx:ASPxGridView ID="gv_view_current_mappings" runat="server" AutoGenerateColumns="False" DataSourceID="sds_current_mappings" KeyFieldName="id" Width="100%" ClientInstanceName="gv_view_current_mappings" oncustomcallback="gv_view_current_mappings_CustomCallback">
									<Columns>
										<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" VisibleIndex="0" Visible="False">
											<EditFormSettings Visible="False" />
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="master_id" VisibleIndex="1" Caption="Master ID" Width="10%">
											<DataItemTemplate>
												<dx:ASPxHyperLink ID="hl" runat="server" NavigateUrl='<%# string.Format("javascript:boing(\"/sections/member/inventory/index.aspx?a=get&tab=G&&id={0}\", \"quote\", 1200,730)", Eval("master_id")) %>' Text='<%# Eval("master_id") %>' />
											</DataItemTemplate>
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="descr" VisibleIndex="2" Caption="Description" Width="75%">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="qty" VisibleIndex="3" Caption="Qty" Width="5%">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="min" VisibleIndex="4" Caption="MIN" Width="5%">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="max" VisibleIndex="5" Caption="MAX" Width="5%">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Delete" VisibleIndex="6">
											<DataItemTemplate>
												<dx:ASPxButton ID="bt_delete" runat="server" AutoPostBack="False" oninit="bt_delete_Init">
													<Image Url="~/images/icon/icon[delete].gif">
													</Image>
												</dx:ASPxButton>
											</DataItemTemplate>
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsBehavior EnableRowHotTrack="True" />
									<SettingsPager PageSize="16">
									</SettingsPager>
									<SettingsText EmptyDataRow="No parts are linked to this location." />
								</dx:ASPxGridView>

		</dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_merge" runat="server" ClientInstanceName="pop_merge" HeaderText="Merge Location" Height="486px" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"  Width="850px" onwindowcallback="pop_merge_WindowCallback" AllowDragging="True">
	<ClientSideEvents Closing="function(s, e) {
		gv_master_locations.Refresh();
	
}" />
	<ContentCollection>



<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
			<table>
				<tr>
					<td><dx:ASPxLabel ID="merge_lb_which_location" runat="server" AssociatedControlID="merge_l_which_location" style="font-weight: 700" Text="Start Location: " Width="150px"></dx:ASPxLabel></td>
					<td><dx:ASPxLabel ID="merge_l_which_location" runat="server" AssociatedControlID="merge_lb_which_location"></dx:ASPxLabel></td>
				</tr>
				<tr>
					<td><dx:ASPxLabel ID="merge_lb_to_location" runat="server" AssociatedControlID="merge_ddl_destinationlocation" style="font-weight: 700" Text="Destination Location: " Width="150px"></dx:ASPxLabel></td>
					<td><dx:ASPxComboBox ID="merge_ddl_destinationlocation" runat="server" ValueType="System.String" DataSourceID="merge_sds_locations" TextField="name" ValueField="id" ClientInstanceName="merge_ddl_destinationlocation"></dx:ASPxComboBox>
						<asp:SqlDataSource ID="merge_sds_locations" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id, CONCAT(IF(a.type_id = 1, 'Int - ' , 'Ext - '), a.name) name FROM inventory_location_master a where  a.id != @inv_master_location_id AND a.business_unit_id = @bu_id AND name != '0.0.0' AND type_id = (SELECT type_id FROM inventory_location_master WHERE id = @inv_master_location_id) order by a.type_id, a.name">
							<SelectParameters>
								<asp:SessionParameter DefaultValue="" Name="@inv_master_location_id" SessionField="inv_master_location_id" />
								<asp:SessionParameter Name="@bu_id" SessionField="working_warehouse_bu_id" />
							</SelectParameters>
						</asp:SqlDataSource></td>
				</tr>
				<tr>
					<td colspan="2">
						<dx:ASPxButton ID="bt_merge_loc" runat="server" AutoPostBack="False" Text="Merge All Links" Width="150px">
							<ClientSideEvents Click="function(s, e) {
	if(merge_ddl_destinationlocation.GetValue() == null)
		{
		alert(&quot;Please choose a destination location&quot;);
		merge_ddl_destinationlocation.Focus();
		}
	else
		{
		if(confirm(&quot;Are you sure you want to do this? This is an undoable procedure.&quot;))
			{
			cb_merge_location.PerformCallback(merge_ddl_destinationlocation.GetValue());
			}
		}
}" />
							<Image Url="~/images/icon/icon[copy].gif">
							</Image>
						</dx:ASPxButton>
						<dx:ASPxCallback ID="cb_merge_location" runat="server" ClientInstanceName="cb_merge_location" oncallback="cb_merge_location_Callback" style="margin-bottom: 0px">
							<ClientSideEvents BeginCallback="function(s, e) {
	please_wait(&quot;start&quot;, &quot;Moving part Links&quot;);
}" CallbackComplete="function(s, e) {
	please_wait(&quot;stop&quot;);
	if(e.result != &quot;SUCCESS&quot;)
		{
		alert(e.result);
		}
	else
		{
		alert(&quot;Successfully moved Links&quot;);
		gv_merge_current_mappings.Refresh();
		}
}" CallbackError="function(s, e) {
	please_wait(&quot;stop&quot;);
}" />
						</dx:ASPxCallback>
					</td>
				</tr>
			</table>
											
								
											<br />
											All of these part mappings will be moved into the destination location.<br /> 
							<dx:ASPxGridView ID="gv_merge_current_mappings" runat="server" AutoGenerateColumns="False" DataSourceID="sds_current_mappings" KeyFieldName="id" Width="100%" ClientInstanceName="gv_merge_current_mappings">
									<Columns>
										<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" VisibleIndex="0" Visible="False">
											<EditFormSettings Visible="False" />
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="master_id" VisibleIndex="1" Caption="Master ID" Width="10%">
											<DataItemTemplate>
												<dx:ASPxHyperLink ID="hl0" runat="server" NavigateUrl='<%# string.Format("javascript:boing(\"/sections/member/inventory/index.aspx?a=get&tab=G&&id={0}\", \"quote\", 1200,730)", Eval("master_id")) %>' Text='<%# Eval("master_id") %>' />
											</DataItemTemplate>
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="descr" VisibleIndex="2" Caption="Description" Width="75%">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="qty" VisibleIndex="3" Caption="Qty" Width="5%">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="min" VisibleIndex="4" Caption="MIN" Width="5%">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn FieldName="max" VisibleIndex="5" Caption="MAX" Width="5%">
											<CellStyle HorizontalAlign="Center">
											</CellStyle>
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsBehavior EnableRowHotTrack="True" />
									<SettingsPager PageSize="16">
									</SettingsPager>
									<SettingsText EmptyDataRow="No parts are linked to this location." />
								</dx:ASPxGridView>

		</dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>

								
