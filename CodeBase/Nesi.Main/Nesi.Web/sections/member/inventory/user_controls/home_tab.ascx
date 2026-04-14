<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="home_tab.ascx.cs" Inherits="sections_member_inventory_user_controls_home"  EnableTheming="True" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


			<p>
</p>


			<dx:ASPxGridView ID="gv_tocreate" runat="server" 
		ClientInstanceName="gv_tocreate" AutoGenerateColumns="False" 
		 Width="100%" Font-Names="Arial" 
		Font-Size="9pt" Theme="NETheme01" KeyFieldName="id" 
	oncustomcallback="gv_tocreate_CustomCallback" SettingsBehavior-ColumnResizeMode="Control">
		<Columns>
			<dx:GridViewCommandColumn VisibleIndex="0" Caption=" " Visible="False" ShowClearFilterButton="true" >
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="Added By" FieldName="added_by" 
				VisibleIndex="4" Width="100px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Date Added" FieldName="date_added" 
				VisibleIndex="5" Width="75px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataDateColumn Caption="Requested Date" FieldName="date_req" 
				VisibleIndex="6" Width="75px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="WO" FieldName="bvwo" VisibleIndex="7" 
				Width="80px">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text='<%# Eval("bvwo") %>' NavigateUrl='<%# "/redir.aspx?url=" + HttpUtility.UrlEncode(string.Format("/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}", Eval("woprog_id"), Eval("business_unit_id"))) %>' Target="_blank" Wrap="True" />
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Branch" FieldName="companyname" 
				VisibleIndex="1" Width="100px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="QTY Req" FieldName="qty_req" VisibleIndex="9" Width="75px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Description" FieldName="descrip" VisibleIndex="8" CellStyle-Wrap="True" Width="300px" MinWidth="20">
				
				<DataItemTemplate>

				<dx:ASPxMemo ID="tb_descrip" runat="server" oninit="tb_desc_Init" 
					Text='<%# Eval("descrip") %>' Theme="NETheme01" Width="100%" Height="35px" Rows="2">
				</dx:ASPxMemo>

			</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" VisibleIndex="10" MinWidth="20" Width="100%">
			<DataItemTemplate>

				<dx:ASPxMemo ID="tb_notes" runat="server" oninit="tb_notes_Init" 
					Text='<%# Eval("notes") %>' Theme="NETheme01" Width="100%" Rows="2" Height="35px">
				</dx:ASPxMemo>

			</DataItemTemplate>
				
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="New Part #" VisibleIndex="3" Visible="false" Width="150px" FieldName="bvwo">
				<DataItemTemplate>
					<input id="Text1" class="part_n" style="width:75px;" type="text" onfocus="$(this).inventory();" />
					<button type="button" id="button1" runat="server" 
						onclick='<%# string.Format("apply_part(this, {0}, {1})", Eval("business_unit_id"), Eval("id")) %>' 
						style="font-family:arial;font-size:11px;font-weight:bold; width: 90px;">apply part #</button>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Left" Wrap="False">
					<Paddings Padding="0px" />
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="id" FieldName="id" Visible="False" 
				VisibleIndex="11">
			</dx:GridViewDataTextColumn>
		</Columns>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" />
				<SettingsText Title="Parts To Create" />
		<Styles>
			<Header HorizontalAlign="Center">
			</Header>
			
		</Styles>
	</dx:ASPxGridView>
	<dx:ASPxGridView ID="gv_merge" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_merge" KeyFieldName="master_id" Width="100%" OnAfterPerformCallback="gv_merge_AfterPerformCallback" OnCustomFilterExpressionDisplayText="gv_merge_CustomFilterExpressionDisplayText" Theme="NETheme01">
		<Styles>
			<Header Font-Bold="True" HorizontalAlign="Center">
			</Header>
			
		</Styles>
		<SettingsPager NumericButtonCount="5">
		</SettingsPager>
		<SettingsText Title="Merged Parts" />
		<Columns>
			<dx:GridViewDataTextColumn Caption="Old #" FieldName="master_id" ReadOnly="True"
				VisibleIndex="0" Width="45px">
				<EditFormSettings Visible="False" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="New #" FieldName="new_id" VisibleIndex="2" Width="45px">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" CssClass="placeHolderNewID" NavigateUrl='<%# string.Format("/sections/member/inventory/index.aspx?a=get&tab=G&&id={0}", Eval("new_id")) %>'
						Text='<%# Eval("new_id") %>' Wrap="True">
					</dx:ASPxHyperLink>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Merged" FieldName="edit_date" VisibleIndex="4"
				Width="50px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="On Hand" FieldName="onhand_qty" VisibleIndex="6" Width="35px">
				<Settings AllowSort="True" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Printed"  VisibleIndex="9" Width="25px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn VisibleIndex="7" Width="25px" Caption=" ">
				<DataItemTemplate>
					<dx:ASPxImage ID="ASPxImage1" runat="server" Cursor="pointer" ImageUrl="~/images/icon/icon[print_barcode].GIF">
						<ClientSideEvents Click="function(s, e) {
please_wait('start');
bc($(s.mainElement).parents('tr:first').find('.placeHolderNewID').text());
gv_merge.Refresh();
please_wait('stop');
}" />
					</dx:ASPxImage>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Description Old" FieldName="description_old" VisibleIndex="1" CellStyle-Wrap="True" Width="200px">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Description New" FieldName="description_new" VisibleIndex="3" CellStyle-Wrap="True" Width="200px">
			</dx:GridViewDataTextColumn>
		</Columns>
		<Settings ShowHeaderFilterButton="True" ShowTitlePanel="True" ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" />
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="ds_merge" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand='
	SELECT 
	a.master_id, 
	a.new_id, 
	a.edit_date,
	b.min_qty,
	b.max_qty,
	b.onhand_qty,
	c.name location,
	de.description,
	IF(e.value_new = 1, "Yes", "No") printed
FROM 
	inventory_item_master a
LEFT JOIN 
	inventory_branch b 
	ON  
		a.new_id = b.master_id AND 
		b.business_unit_id = @business_unit_id
LEFT JOIN inventory_location c
	ON 
		a.new_id = c.master_id AND 
		c.business_unit_id = @business_unit_id
LEFT JOIN inventory_description de
	ON 
		a.new_id = de.master_id
LEFT JOIN 
	(
	SELECT associated_table_id master_id, value_new FROM log WHERE business_unit_id = @business_unit_id AND section_id = 8 AND action_id = 6 AND associated_table = "inventory_item_master"
	) e ON a.master_id = e.master_id 
WHERE 
	a.tag_id != 718 AND 
	a.new_id IS NOT NULL
GROUP BY printed, master_id
ORDER BY a.new_id DESC
'>
		<SelectParameters>
			<asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" />
		</SelectParameters>
	</asp:SqlDataSource>