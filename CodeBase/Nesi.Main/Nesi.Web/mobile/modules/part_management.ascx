<%@ Control Language="C#" AutoEventWireup="true" Inherits="mobile_modules_part_management" Codebehind="part_management.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
	<meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1' name='viewport' />
	<style type="text/css">
		* { font-family: arial; }
		.tab {border: 0; padding: 5px; margin: 0px; margin-top:5px; font-weight: bold; width: 14%; font-size: 1em; height: 40px; border-right: solid 1px #999;}
		.tab.inactive { background-color: #00447C; color: #fff;}
		.tab.active { background-color: #ddd; }
		.tab.disabled { background-color: #ddd; visibility:hidden;}
		.name_div { font-family: arial; font-size: 12px; background-color: #00447C; color: #fff; width: 100%; visibility:hidden; }
		.name_div * { margin: 0px; }
		.bwrap { width: 100px; white-space: normal; font-weight: bold; }
		
		</style>
	<script type="text/javascript">
	document.addEventListener("DOMContentLoaded", function(event) {
	Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
	Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
});
	function EndReqHandler()
		{
//		document.getElementById("modal_overlay").style.display		= "none";
		}
	function BeginReqHandler()
		{
//		document.getElementById("modal_overlay").style.display		= "";
	}
	function pop_search_open() {
		pop_search.Show();
		
		pop_search.PerformCallback();
	//	document.getElementById("pop_search_txt_search").focus();
	}
	function set_focus(InputID)
	{
	document.getElementById(InputID).focus();  
        }

        function bc(master_id)
        {
            
            createPopupWin("/_tools/print_barcode/index.aspx?master_id=" + master_id, "Print Barcode", 600, 200);
        
		//popbcprint.SetHeaderText("Print Labels for: " + master_id);
		//hidbcmastertid.Set("id",master_id);
		//popbcprint.Show();

        }
        function createPopupWin(pageURL, pageTitle, 
                    popupWinWidth, popupWinHeight) { 
            var left = (screen.width-popupWinWidth)/2; 
            var top = (screen.height-popupWinHeight)/4;
            var myWindow = window.open(pageURL, pageTitle,  
                    'resizable=yes, width=' + popupWinWidth 
                    + ', height=' + popupWinHeight + ', top=' 
                    + top + ', left=' + left); 
        } 
	</script>
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
			<div id="modal_overlay" style="display:none;z-index:999;background-image:url('/images/loading_panel.gif');background-repeat:no-repeat;background-position:center center;width:100%;height:100%;position:absolute;left:0px;top:0px;background-color:transparent;"></div>
			<div style="font-family: Arial; white-space: normal;" align="center">
				
				<asp:Button ID="tab_admin" runat="server" Text="Admin" OnClick="tab_admin_Click" UseSubmitBehavior="False" />
				<asp:Button ID="tab_transfer" runat="server" Text="Transfer" OnClick="tab_transfer_Click" UseSubmitBehavior="False" />
				<asp:Button ID="tab_po" runat="server" Visible="false" Text="Purchase Order" OnClick="tab_po_Click" UseSubmitBehavior="False" />
				<asp:Button ID="tab_annual" runat="server" Text="Annual Counts" OnClick="tab_annual_Click" UseSubmitBehavior="False" />
				<asp:Button ID="tab_request" runat="server" Text="Request" OnClick="tab_request_Click" UseSubmitBehavior="False" />
				<asp:Button ID="tab_commit" runat="server" Text="Commit" OnClick="tab_commmit_Click" UseSubmitBehavior="False" />
				<asp:Button ID="tab_return" runat="server" Text="Return" OnClick="tab_return_Click" UseSubmitBehavior="False" />
				<asp:Button ID="tab_history" runat="server" Text="History" OnClick="tab_history_Click" UseSubmitBehavior="False" />
				<asp:Button ID="tab_shopping" runat="server" Text="To Pull" OnClick="tab_shopping_Click" UseSubmitBehavior="False" />
                <asp:Button ID="tab_incorrect" runat="server" Text="Corrections" OnClick="tab_incorrect_Click" UseSubmitBehavior="False" />
				<br />
				<table id="tbl_new" runat="server" cellpadding="0" cellspacing="0" 
					style="background-color: #ccc;" width="100%">
					<tr>
						<td rowspan="2" align="center" width="100px" valign="top">
							<asp:TextBox ID="scanbox" runat="server" AutoPostBack="True" TabIndex="99" BorderStyle="Solid" BorderWidth="3px" OnTextChanged="new_scan" size="8" Width="80px" BackColor="#FF3300" Height="40px" Font-Size="14pt" type="search" ></asp:TextBox>
						</td>
						<td width="50"><strong id="label_bvwo" runat="server" style="width: 60px;">
							BV WlO#:</strong></td>
						<td width="100%">
							<asp:Label ID="lbbvwo" runat="server" Font-Names="Arial" 
								Font-Size="10pt" Width="100%"></asp:Label>
						</td>
						<td rowspan="3">
							<asp:Button ID="btn_reset" runat="server" Height="40px" 
								OnClick="btn_reset_Click" Style="font-weight: bold" Text="Reset" Width="70px" 
								UseSubmitBehavior="False" />
						</td>
					</tr>
					<tr>
						<td><asp:Label ID="lbl_masterid" runat="server" Font-Bold="False" Font-Names="Arial" Text="Master ID:" Width="60px"></asp:Label></td>
						<td>
							<asp:Label ID="lb_part_no" runat="server" Width="100%"></asp:Label>
						</td>
					</tr>
					<tr>
						<td align="center" valign="top" width="100px">
							<asp:Button ID="btn_search" runat="server" BorderColor="Black" BorderStyle="Solid" 
								BorderWidth="1px" Height="35px" 
								onClientClick="pop_search_open();  return false;" Text="Search" 
								UseSubmitBehavior="False" Width="100%" TabIndex="300" />
						</td>
						<td><asp:Label ID="lb_wo_no" runat="server" Font-Names="Arial" Font-Size="1px" 
								ForeColor="LightGray" Width="2px"></asp:Label></td>
						<td style="padding:2px" colspan="1">
							<asp:Label ID="lb_part_description" runat="server" Font-Names="Arial" 
								Font-Size="7pt" Height="20px" Width="100%"></asp:Label>
						</td>
					</tr>
				</table>
			</div>
			<div align="center">
				<div id="div_error" runat="server">
				<asp:Label ID="xfer_lb_warning" runat="server" Font-Bold="True" 
					Font-Size="11px" ForeColor="Red" Width="375px"></asp:Label>
				</div>
				<div style="margin-top: 3px;">
				<asp:TextBox ID="tb_777_description" placeholder="777 Description" runat="server" Height="40px" Font-Size="15pt" Width="98%" Visible="false" TabIndex="199"></asp:TextBox>
				</div>
			</div>
			<asp:MultiView ID="mv" runat="server" ActiveViewIndex="0" OnActiveViewChanged="mv_ActiveViewChanged">
				<asp:View ID="vw_admin" runat="server">
					<asp:Label ID="admin_lb_warning" runat="server" Font-Bold="True" Font-Size="11px" ForeColor="Red" Width="375px"></asp:Label>
					<table cellpadding="0" cellspacing="0" style="width: 100%; ">
						<tr>
							<td colspan="1" style="width: 55%">
								<b>[ FROM ] Location:</b><br />
								<asp:DropDownList ID="admin_int_location" runat="server" AutoPostBack="True" DataSourceID="ds_admin_int_location" DataTextField="name" DataValueField="id" OnSelectedIndexChanged="admin_int_location_SelectedIndexChanged" Width="100%" Height="35px" BackColor="#66CCFF">
								</asp:DropDownList>
								<br />
								<asp:SqlDataSource ID="ds_admin_int_location" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
	b.id,
	CONCAT(IF(b.type_id = 1, &quot;INT - &quot;, &quot;EXT - &quot;), Concat(b.name),&quot; (&quot;,IFNULL(a.qty,0),&quot;)&quot;)  name,
IFNULL(a.qty,0) qty
FROM
	inventory_location_master b
LEFT JOIN
inventory_location a ON b.id = a.location_master_id AND a.master_id = @master_id
WHERE
	b.business_unit_id = @business_unit_id

ORDER BY qty DESC, a.max DESC, name ASC">
									<SelectParameters>
										<asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" />
										<asp:SessionParameter Name="@master_id" SessionField="working_master_id" />
									</SelectParameters>
								</asp:SqlDataSource>
								<br />
							</td>
							<td style="padding-top: 13px; padding-left: 5px; width: 10%;" valign="top">
								<asp:Label ID="admin_lb_int_qty" runat="server" Font-Size="12px"></asp:Label>
							</td>
							<td style="width: 35%">
								<asp:Button ID="b_updatecb1" runat="server" Height="45px" 
									OnClick="b_updatecb1_Click" Text="Update Location 1 QTY" Width="100px" 
									BackColor="#66CCFF" CssClass="bwrap" UseSubmitBehavior="False" />
							</td>
						</tr>
						<tr>
							<td align="center" style="width: 55%" nowrap="nowrap" valign="middle">
								<asp:Button ID="admin_b_transfer" runat="server" Height="75px" 
									OnClick="admin_b_transfer_Click" 
									Text="Transfer (from location 1 to location 2)" Width="200px" 
									BackColor="#FFFFCC" CssClass="bwrap" UseSubmitBehavior="False" />
								&nbsp;&nbsp;
							</td>
							<td colspan="2" style="width: 45%; vertical-align: middle;" valign="middle" align="center">
								Qty<asp:TextBox ID="admin_txtQty" runat="server" Font-Names="Arial" 
									Font-Size="15pt" Height="40px" OnTextChanged="txtQty_TextChanged" size="8" 
									TabIndex="98" Width="80px" type="number" step="0.01"></asp:TextBox>
								&nbsp;&nbsp;
								<asp:Label ID="lbl_units" runat="server" Font-Size="16pt"></asp:Label>
							</td>
						</tr>
						<tr>
							<td colspan="1" style="width: 55%">
								<b>[ TO ] Location:</b><br />
								<asp:DropDownList ID="admin_ext_location" runat="server" AutoPostBack="True" DataSourceID="ds_admin_ext_location" DataTextField="name" DataValueField="id" Height="35px" OnSelectedIndexChanged="admin_ext_location_SelectedIndexChanged" Width="100%" BackColor="#FFCC66">
								</asp:DropDownList>
								<br />
								<asp:SqlDataSource ID="ds_admin_ext_location" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
	b.id,
	CONCAT(IF(b.type_id = 1, &quot;INT - &quot;, &quot;EXT - &quot;), Concat(b.name),&quot; (&quot;,IFNULL(a.qty,0),&quot;)&quot;)  name,
IFNULL(a.qty,0) qty
FROM
	inventory_location_master b
LEFT JOIN
inventory_location a ON b.id = a.location_master_id AND a.master_id = @master_id
WHERE
	b.business_unit_id = @business_unit_id

ORDER BY qty DESC, a.max DESC, name ASC">
									<SelectParameters>
										<asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" />
										<asp:SessionParameter Name="@master_id" SessionField="working_master_id" />
									</SelectParameters>
								</asp:SqlDataSource>
								<br />
							</td>
							<td valign="top" style="padding-top: 13px; padding-left: 5px; width: 10%;">
								<asp:Label ID="admin_lb_ext_qty" runat="server" Font-Size="12px"></asp:Label>
							</td>
							<td style="width: 35%">
								<asp:Button ID="b_updatecb2" runat="server" Height="45px" 
									OnClick="b_updatecb2_Click" Text="Update Location 2 QTY" Width="100px" 
									BackColor="#FFCC66" CssClass="bwrap" UseSubmitBehavior="False" />
							</td>
						</tr>
					</table>
					<table width="100%">
						<tr>
							<td colspan="3" valign="middle">
								<div style="text-align:center;font-weight:bold;">3rd party barcodes</div>
							</td>
						</tr>
						<tr>
							<td>
								<asp:TextBox ID="admin_txtnewcode" runat="server" BackColor="Yellow" Height="40px" size="8" Width="100px"></asp:TextBox>
							</td>
							<td> <asp:Button ID="admin_btn_addman" runat="server" Height="40px" 
									OnClick="btn_addman_Click" Text="Add MFR Barcode" UseSubmitBehavior="False" />
							</td>
							<td>
								<asp:Button ID="admin_btn_addvend" runat="server" Height="40px" 
									OnClick="btn_addvend_Click" Text="Add VDR Barcode" UseSubmitBehavior="False" />
							</td>
						</tr>
						<tr>
							<td align="center" colspan="3">
								<div id="div_added0" runat="server" style="font-size: 9pt; font-family: Arial">
								</div>
							</td>
						</tr>
						<tr>
							<td align="center" colspan="3" style="width: 65%; text-align: left;">
								<asp:Label ID="admin_lb_notice" runat="server" Font-Bold="True" Font-Size="11px" ForeColor="#009900"></asp:Label>
							</td>
						</tr>
					</table>
				</asp:View>
				<asp:View ID="vw_transfer" runat="server">
					<table cellpadding="2" cellspacing="0" style="width: 100%; ">
						<tr>
							<td style="width: 67%">
								<b style="font-size: 11px;">FROM Location:</b><br />
								<asp:DropDownList ID="xfer_from_location" runat="server" AutoPostBack="True" DataSourceID="ds_xfer_from_location" DataTextField="name" DataValueField="id" OnSelectedIndexChanged="xfer_int_location_SelectedIndexChanged" Width="100%" Height="35px">
								</asp:DropDownList>
								<br />
								<asp:SqlDataSource ID="ds_xfer_from_location" runat="server" 
									ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
									ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>">
									<SelectParameters>
										<asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" />
										<asp:SessionParameter Name="@master_id" SessionField="working_master_id" DefaultValue="0" />
									</SelectParameters>
								</asp:SqlDataSource>
								<br />
							</td>
							<td width="100px">
								<asp:Button ID="xfer_b_emptyfrom_bin" runat="server" BackColor="#66CC99" 
									CssClass="bwrap" Height="50px"  
                                     OnClientClick="txt_final_call.SetText('from_from_bin'); pop_invalid_qty.Show(); "
									
									Text="Report Invalid Qty in the FROM Bin" Width="100px" UseSubmitBehavior="False" />
							</td>
							<td style="padding-top: 13px; padding-left: 5px; width: 33%;" valign="top">
								<asp:Label ID="xfer_lb_int_qty" runat="server" Font-Size="12px"></asp:Label>
							</td>
						</tr>
						<tr>
							<td colspan="2" style="width: 67%; text-align: center;">
								&nbsp;
								<asp:Button ID="xfer_b_transfer" runat="server" BackColor="Yellow" 
									CssClass="bwrap" Font-Bold="False" Font-Size="12pt" Height="50px" 
									OnClick="xfer_b_move_Click" Text="Transfer ↓" Width="100px" UseSubmitBehavior="False" />
								&nbsp;</td>
							<td style="width: 33%; text-align: center; white-space: nowrap;" 
								valign="middle" nowrap="nowrap">
								QTY<asp:TextBox ID="xfer_txtQty" runat="server" Font-Names="Arial" 
									Font-Size="15pt" Height="50px" OnTextChanged="txtQty_TextChanged" size="8" 
									TabIndex="97" Type="Number" Step="0.0001" Width="80px"></asp:TextBox>
								
							</td>
						</tr>
						<tr>
							<td style="width: 67%">
								<b style="font-size: 11px;">TO&nbsp; Location:</b><br />
								<asp:DropDownList ID="xfer_to_location" runat="server" AutoPostBack="True" DataSourceID="ds_xfer_to_location" DataTextField="name" DataValueField="id" Height="35px" OnSelectedIndexChanged="xfer_ext_location_SelectedIndexChanged" Width="100%">
								</asp:DropDownList>
								<br />
								<asp:SqlDataSource ID="ds_xfer_to_location" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 0 id, '' name, 0 qty
UNION
SELECT
	a.id,
	CONCAT(IF(a.type_id = 1, &quot;INT - &quot;, &quot;EXT - &quot;), Concat(a.name),&quot; (&quot;,IFNULL(b.qty,0),&quot;)&quot;)  name,
IFNULL(b.qty, 0) qty
FROM
	inventory_location_master a
LEFT JOIN
inventory_location b ON a.id = b.location_master_id AND b.master_id = @master_id AND b.business_unit_id = @business_unit_id
WHERE
	a.business_unit_id = @business_unit_id
ORDER BY b.type_id, b.name, b.max DESC">
									<SelectParameters>
										<asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" />
										<asp:SessionParameter DefaultValue="0" Name="@master_id" SessionField="working_master_id" />
									</SelectParameters>
								</asp:SqlDataSource>
								<br />
							</td>
							<td style="width: 67%">
								<asp:Button ID="xfer_b_emptytobin" runat="server" BackColor="#66CC99" 
									CssClass="bwrap" Height="50px" 
									  OnClientClick="txt_final_call.SetText('from_to_bin'); pop_invalid_qty.Show(); " 
									Text="Report Invalid Qty in the TO Bin" Width="100px" UseSubmitBehavior="False" />
							</td>
							<td style="padding-top: 13px; padding-left: 5px; width: 33%;" valign="top">
								<asp:Label ID="xfer_lb_ext_qty" runat="server" Font-Size="12px"></asp:Label>
							</td>
						</tr>
						<tr>
							<td colspan="2" style="width: 67%">
								&nbsp;
							</td>
							<td valign="top" width="33%;">
								&nbsp;
							</td>
						</tr>
						<tr>
							<td align="center" width="33%">
								&nbsp;</td>
							<td align="center" style="padding-left: 5px" width="34%">
								<br />
							</td>
							<td align="center" valign="middle" width="33%">
								&nbsp;</td>
						</tr>
						<tr>
							<td colspan="3" align="center">
								&nbsp;
							</td>
						</tr>
						<tr>
							<td colspan="3" nowrap="nowrap" width="100%">
								<div style="white-space: nowrap">
									<table style="width: 100%;">
										<tr>
											<td>
												&nbsp;</td>
											<td width="100%">
												&nbsp;
											</td>
										</tr>
									</table>
								</div>
							</td>
						</tr>
						<tr>
							<td colspan="3">
								<div id="div_added" runat="server" style="font-size: 9pt; font-family: Arial">
								</div>
								<asp:Label ID="xfer_lb_notice" runat="server" Font-Bold="True" Font-Size="11px" ForeColor="#009900" Width="100%"></asp:Label>
								<br />
								&nbsp;
							</td>
						</tr>
					</table>
				</asp:View>
				<asp:View ID="vw_po" runat="server">
					<asp:Label ID="po_lb_warning" runat="server" Font-Bold="True" Font-Size="11px" ForeColor="Red" Width="375px"></asp:Label>
					<table cellpadding="0" cellspacing="0" style="width: 100%; ">
						<tr>
							<td colspan="2" style="width: 65%">
								<b style="font-size: 11px;">PO Line Item:</b><br />
								<asp:DropDownList ID="po_lineitem" runat="server" DataTextField="name" DataValueField="id" Width="100%" Height="35px" OnSelectedIndexChanged="po_lineitem_SelectedIndexChanged" AutoPostBack="True">
								</asp:DropDownList>
								<asp:SqlDataSource ID="ds_po_lineitem" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
a.po_details_id id,
CONCAT(b.poprog_bvpo, &quot; - (&quot;,a.po_details_part_no , &quot;) - &quot;, VENDOR_NAME(b.poprog_vendor_id),&quot; - (&quot;, a.po_details_qty_ordered - a.po_details_qty_received, &quot; Remaining)&quot;) name
FROM
po_details_current a
LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id
WHERE 
b.poprog_status = 3 AND
a.po_details_part_no IN(@master_id, a.po_details_part_no) AND
b.business_unit_id = @business_unit_id AND
b.poprog_id IN(@working_poprog_id, b.poprog_id) AND
a.po_details_qty_ordered - a.po_details_qty_received &gt; 0">
									<SelectParameters>
										<asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" DefaultValue="" />
										<asp:SessionParameter Name="@master_id" SessionField="working_master_id" DefaultValue="true" />
										<asp:SessionParameter DefaultValue="true" Name="@poprog_id" SessionField="working_poprog_id" />
									</SelectParameters>
								</asp:SqlDataSource>
								<br />
								<br />
							</td>
							<td style="padding-top: 13px; padding-left: 5px; width: 35%;" valign="top">
								<asp:Label ID="po_lb_po_qty" runat="server" Font-Size="12px"></asp:Label>
							</td>
						</tr>
						<tr>
							<td colspan="2" style="width: 65%">
								<b style="font-size: 11px;">Internal Location:</b><br />
								<asp:DropDownList ID="po_int_location" runat="server" AutoPostBack="True" DataSourceID="ds_po_int_location" DataTextField="name" DataValueField="id" OnSelectedIndexChanged="po_int_location_SelectedIndexChanged" Width="100%" Height="35px">
								</asp:DropDownList>
								<asp:SqlDataSource ID="ds_po_int_location" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
	a.id,
	Concat(a.name,&quot; (&quot;,IFNULL(b.qty,0),&quot;)&quot;)  name
FROM
	inventory_location_master a
LEFT JOIN
inventory_location b ON a.id = b.location_master_id AND b.master_id = @master_id
WHERE
	a.business_unit_id = @business_unit_id AND
	a.type_id = 1
ORDER BY b.qty desc, name asc, b.max DESC">
									<SelectParameters>
										<asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" />
										<asp:SessionParameter Name="@master_id" SessionField="working_master_id" />
									</SelectParameters>
								</asp:SqlDataSource>
								<br />
								<br />
							</td>
							<td style="padding-top: 13px; padding-left: 5px; width: 35%;" valign="top">
								<asp:Label ID="po_lb_int_qty" runat="server" Font-Size="12px"></asp:Label>
							</td>
						</tr>
						<tr>
							<td colspan="2" style="width: 65%">
								<b style="font-size: 11px;">External Location:</b><br />
								<asp:DropDownList ID="po_ext_location" runat="server" AutoPostBack="True" DataSourceID="ds_po_ext_location" DataTextField="name" DataValueField="id" OnSelectedIndexChanged="po_ext_location_SelectedIndexChanged" Width="100%" Height="35px">
								</asp:DropDownList>
								<asp:SqlDataSource ID="ds_po_ext_location" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
	a.id,
	Concat(a.name,&quot; (&quot;,IFNULL(b.qty,0),&quot;)&quot;)  name
FROM
	inventory_location_master a
LEFT JOIN
inventory_location b ON a.id = b.location_master_id AND b.master_id = @master_id
WHERE
	a.business_unit_id = @business_unit_id AND
	a.type_id = 2
ORDER BY qty desc, name asc, b.max DESC">
									<SelectParameters>
										<asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" />
									</SelectParameters>
								</asp:SqlDataSource>
								<br />
								<br />
							</td>
							<td style="padding-top: 13px; padding-left: 5px; width: 35%;" valign="top">
								<asp:Label ID="po_lb_ext_qty" runat="server" Font-Size="12px"></asp:Label>
							</td>
						</tr>
						<tr>
							<td align="center" width="33%">
								<asp:Button ID="po_b_rec_stock" runat="server" Height="50px" 
									OnClick="po_b_rec_stock_Click" Text="Receive to Internal" CssClass="bwrap" 
									Width="100%" UseSubmitBehavior="False" />
							</td>
							<td align="center" width="33%">
								<span style="font-family: Arial"><strong>QTY</strong></span><br />
								<asp:TextBox ID="po_txtQty" runat="server" Font-Names="Arial" Font-Size="15pt" 
									OnTextChanged="txtQty_TextChanged" size="8" TabIndex="96" Width="50%" 
									type="number" step="0.01"></asp:TextBox>
							</td>
							<td align="center" valign="middle" width="33%">
								<asp:Button ID="po_b_rec_location" runat="server" Height="50px" 
									OnClick="po_b_rec_location_Click" Text="Receive to External" CssClass="bwrap" 
									Width="100%" UseSubmitBehavior="False" />
							</td>
						</tr>
						<tr>
							<td align="center" colspan="3">
								&nbsp;
							</td>
						</tr>
						<tr>
							<td colspan="3">
								<div id="div_added1" runat="server" style="font-size: 9pt; font-family: Arial">
								</div>
								<asp:Label ID="po_lb_notice" runat="server" Font-Bold="True" Font-Size="11px" ForeColor="#009900" Width="100%"></asp:Label>
								<br />
								&nbsp;
							</td>
						</tr>
					</table>
				</asp:View>
				<asp:View ID="vw_annual" runat="server">
					<table cellpadding="2" cellspacing="0" width="100%" id="annual_table" runat="server" style="margin-top:3px;">
						<tr>
							<td colspan="2" runat="server" ID="annual_last_updated"></td>
						</tr>
						<tr>
							<td align="center" colspan="2"></td>
						</tr>
						<tr style="font-weight:bold;font-size:1.25em;background-color:#00447C;color:#fff;">
							<td align="center" width="50%">Selected Location</td>
							<td align="center" width="50%">&nbsp;</td>
						</tr>
						<tr>
							<td align="center"><asp:Label ID="annual_lb_location_name" runat="server" Text="--"></asp:Label></td>
							<td align="center"><asp:Label ID="annual_lb_location_qty" runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td align="center" colspan="2" style="height:2px"></td>
						</tr>
						<tr style="font-weight:bold;font-size:1.25em;background-color:#00447C;color:#fff;">
							<td align="center" colspan="2">New Quantity</td>
						</tr>
						<tr>
							<td colspan="2" align="center"><asp:TextBox ID="tb_annual_qty" runat="server" 
									style="font-size:3em;font-weight:bold;text-align:center;width:250px;" 
									type="number" step="0.01"></asp:TextBox></td>
						</tr>
						<tr>
							<td align="center" colspan="2" style="height:2px"></td>
						</tr>
						<tr>
							<td colspan="2" align="center" style="padding-bottom:25px;">
								<asp:Button ID="button_annual_qty" runat="server" Text="Set Quantity" 
									style="font-size:3em;font-weight:bold;text-align:center;width:250px;" 
									Enabled="False" onclick="button_annual_qty_Click" UseSubmitBehavior="False"></asp:Button>
							</td>
						</tr>
					</table>
					<div align="center" style="background-color:#00447C;color:#fff;">
						<b>Your Scan History</b>
					</div>
					<dx:ASPxGridView runat="server" ID="gv_annual_history" AutoGenerateColumns="False" onhtmldatacellprepared="gv_annual_history_HtmlDataCellPrepared" Width="100%" EnableTheming="False" Font-Size="9px">
						<Columns>
							<dx:GridViewDataTextColumn Caption="Date" FieldName="date_transferred" VisibleIndex="1">
								<HeaderStyle HorizontalAlign="Center" />
								<cellstyle horizontalalign="Center" wrap="True">
								</cellstyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Part" FieldName="masterid" VisibleIndex="2">
								<HeaderStyle HorizontalAlign="Center" />
								<cellstyle horizontalalign="Center">
								</cellstyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Location" FieldName="location" VisibleIndex="3">
								<HeaderStyle HorizontalAlign="Center" />
								<cellstyle horizontalalign="Left" wrap="True">
								</cellstyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Qty" FieldName="qty" VisibleIndex="4">
								<HeaderStyle HorizontalAlign="Center" />
								<cellstyle horizontalalign="Center">
								</cellstyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption=" #" VisibleIndex="0">
								<HeaderStyle HorizontalAlign="Center" />
								<cellstyle horizontalalign="Center" wrap="True">
								</cellstyle>
							</dx:GridViewDataTextColumn>
						</Columns>
						<settingsbehavior allowdragdrop="False" allowgroup="False" />
						<settingspager numericbuttoncount="5" pagesize="50">
						</settingspager>
						<paddings padding="0px" />
						<styles>
							<cell font-size="9px" wrap="True">
								<paddings padding="0px" />
							</cell>
						</styles>
					</dx:ASPxGridView>
					<div style="text-align:center;font-weight:bold;font-size:1em;" runat="server" id="annual_location"></div>
					<asp:HiddenField ID="hid_location_id" runat="server" />
					<br /><br />
				</asp:View>
				<asp:View ID="vw_request" runat="server">
					<asp:Label ID="lbl_error_request" runat="server" Font-Bold="True" Font-Size="11px" 
						ForeColor="Red" Width="375px"></asp:Label>
					<table cellpadding="2" cellspacing="0" style="width: 100%; ">
						<tr>
							<td rowspan="3">
								Qty:</td>
							<td width="100%" rowspan="3">
								<asp:TextBox ID="txt_request_qty" runat="server" Font-Names="Arial" 
									Font-Size="16pt" Height="40px" OnTextChanged="txtQty_TextChanged" size="8" 
									Step="0.0001" TabIndex="200" Type="Number" Width="120px" TextMode="Number"></asp:TextBox>
								<br />
							</td>
							<td nowrap="nowrap">
								<asp:Label ID="lbl_req_onwo" runat="server" Visible="False">Qtys on this work 
								order</asp:Label>
							</td>
							<td style="padding-top: 13px; padding-left: 5px; width: 33%;" valign="top" 
								rowspan="3">
								&nbsp;</td>
						</tr>
						<tr>
							<td>
								<asp:Label ID="lbl_req_qty" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Label ID="lbl_cmt_qty" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								Needed Date:</td>
							<td colspan="1">
								<asp:TextBox ID="dteRequired" runat="server" Font-Size="16pt" Height="30px" 
									 Width="120px" TabIndex="500" TextMode="Date"></asp:TextBox>
							</td>
							<td nowrap="nowrap" 
								style="width: 43%; text-align: center; white-space: nowrap;" valign="middle">
								<asp:Button ID="xfer_b_req" runat="server" BackColor="#99FF33" CssClass="bwrap" 
									Height="50px" OnClick="btnRequire_Click" Text="Request on WO" Width="100px" TabIndex="300" 
									UseSubmitBehavior="False" />
							</td>
						</tr>
						<tr>
							<td>
								&nbsp;</td>
							<td colspan="3">
								<br />
								&nbsp;
							</td>
						</tr>
					</table>
				</asp:View>
				<asp:View ID="vw_commit" runat="server">
					<asp:Label ID="lbl_error_commit" runat="server" Font-Bold="True" 
						Font-Size="11px" ForeColor="Red" Width="375px"></asp:Label>
					<table style="width:100%;">
						<tr>
							<td nowrap="nowrap">
								From :</td>
							<td style="width: 0%" width="100%">
								<asp:DropDownList ID="ddl_commit_from_location" runat="server" 
									AutoPostBack="True" DataSourceID="ds_xfer_from_location" DataTextField="name" 
									DataValueField="id" Height="35px" 
									OnSelectedIndexChanged="xfer_int_location_SelectedIndexChanged" Width="120px">
								</asp:DropDownList>
							</td>
							<td align="right" width="100%">
								<asp:Label ID="xfer_lb_int_qty0" runat="server" Font-Size="12px" 
									style="text-align: right" Width="50px"></asp:Label>
							</td>
							<td nowrap="nowrap">
								<asp:Button ID="xfer_b_emptytobin0" runat="server" BackColor="#66CC99" 
									CssClass="bwrap" Height="50px" 
                                    OnClientClick="txt_final_call.SetText('from_commit'); pop_invalid_qty.Show(); "
								
									Text="Report Invalid Qty in the FROM Bin" Width="100px" UseSubmitBehavior="False" />
							</td>
						</tr>
						<tr>
							<td nowrap="nowrap">
								Qty:</td>
							<td style="width: 0%" width="100%">
								<asp:TextBox ID="txt_commit_qty" runat="server" Font-Names="Arial" 
									Font-Size="16pt" Height="40px" OnTextChanged="txtQty_TextChanged" size="8" 
									Step="0.0001" TabIndex="200" Type="Number" Width="120px"></asp:TextBox>
							</td>
							<td align="right" width="100%">
								&nbsp;</td>
							<td nowrap="nowrap">
								<asp:Button ID="xfer_btn_Cmt" runat="server" BackColor="Aqua" CssClass="bwrap" 
									Height="50px" OnClick="btnCmt_Click" Text="Commit to WO" 
									UseSubmitBehavior="False" Width="100px" />
							</td>
						</tr>
						<tr>
							<td rowspan="3">
								&nbsp;</td>
							<td rowspan="3" colspan="2">
								&nbsp;</td>
							<td nowrap="nowrap">
								<asp:Label ID="lbl_req_onwo0" runat="server" Visible="False">Qtys on this WO</asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Label ID="lbl_req_qty0" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Label ID="lbl_cmt_qty0" runat="server"></asp:Label>
							</td>
						</tr>
					</table>
				</asp:View>
				<asp:View ID="vw_return" runat="server">
					<asp:Label ID="lbl_error_return" runat="server" Font-Bold="True" 
						Font-Size="11px" ForeColor="Red" Width="375px"></asp:Label>
					<table style="width:100%;">
						<tr>
							<td>
								To:</td>
							<td width="100px">
								<asp:DropDownList ID="ddl_return_to_location" runat="server" 
									AutoPostBack="True" DataSourceID="ds_xfer_to_location" DataTextField="name" 
									DataValueField="id" Height="35px" 
									OnSelectedIndexChanged="xfer_ext_location_SelectedIndexChanged" Width="120px">
								</asp:DropDownList>
							</td>
							<td align="right" width="100%">
								<asp:Label ID="xfer_lb_ext_qty0" runat="server" Font-Size="12px" 
									style="text-align: right" Width="50px"></asp:Label>
							</td>
							<td>
								<asp:Button ID="xfer_b_emptytobin1" runat="server" BackColor="#66CC99" 
									CssClass="bwrap" Height="50px" 
									OnClientClick="txt_final_call.SetText('from_return'); pop_invalid_qty.Show(); " 
									Text="Report Invalid Qty in the FROM Bin" Width="100px" UseSubmitBehavior="False" />
							</td>
						</tr>
						<tr>
							<td>
								Qty:</td>
							<td width="100px">
								<asp:TextBox ID="txt_return_qty" runat="server" Font-Names="Arial" 
									Font-Size="16pt" Height="40px" OnTextChanged="txtQty_TextChanged" size="8" 
									Step="0.0001" TabIndex="200" Type="" Width="120px"></asp:TextBox>
							</td>
							<td align="right" width="100%">
								&nbsp;</td>
							<td>
								<asp:Button ID="xfer_b_return" runat="server" BackColor="#CC66FF" 
									CssClass="bwrap" Height="50px" OnClick="xfer_b_tostock_Click" 
									Text="Remove from WO the [To] Location" UseSubmitBehavior="False" 
									Width="100px" />
							</td>
						</tr>
						<tr>
							<td rowspan="3">
								&nbsp;</td>
							<td colspan="2" rowspan="3">
								&nbsp;</td>
							<td>
								<asp:Label ID="lbl_req_onwo1" runat="server" Visible="False">Qtys on this WO</asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Label ID="lbl_req_qty1" runat="server"></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								<asp:Label ID="lbl_cmt_qty1" runat="server"></asp:Label>
							</td>
						</tr>
					</table>
				</asp:View>
				<asp:View ID="vw_history" runat="server">
					<asp:GridView ID="gv_history" runat="server" Width="100%" 
						AutoGenerateColumns="False" BorderStyle="None" DataSourceID="SqlDataSource1" 
						GridLines="Vertical" DataKeyNames="masterid" EnablePersistedSelection="True" 
						onselectedindexchanged="gv_search_SelectedIndexChanged">
						<AlternatingRowStyle BackColor="#EBEBEB" BorderStyle="None" />
						<Columns>
							<asp:CommandField ShowSelectButton="True" >
							<ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                            </asp:CommandField>
							<asp:BoundField DataField="masterid" HeaderText="Part" />
							<asp:BoundField DataField="qty" HeaderText="Qty">
								<ItemStyle HorizontalAlign="Center" />
								
							</asp:BoundField>
							<asp:BoundField DataField="description" HeaderText="Desc" />
							<asp:BoundField DataField="dt" HeaderText="Date" />
							<asp:BoundField DataField="note" HeaderText="Note" />
						</Columns>
						<HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="None" />
						<PagerSettings Visible="False" />
						<RowStyle BorderColor="#CCCCCC" BorderStyle="None" Height="40px" />
					</asp:GridView>
					<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
						ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
						ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
						SelectCommand="Call get_stocktransfer_history_for_member(@memberid)" 
						UpdateCommandType="StoredProcedure">
						<SelectParameters>
							<asp:SessionParameter Name="memberid" SessionField="memberid" />
						</SelectParameters>
					</asp:SqlDataSource>
					<br />
				</asp:View>
				<asp:View ID="vw_shopping" runat="server">
					
					<asp:GridView ID="gv_shopping" runat="server" AutoGenerateColumns="False" 
						BorderStyle="None" DataSourceID="SqlDataSource2" GridLines="Vertical" 
						Width="100%" ShowHeaderWhenEmpty="True" DataKeyNames="wo_detail_current_master_id" 
						EnablePersistedSelection="True" 
						onselectedindexchanged="gv_search_SelectedIndexChanged">
						<AlternatingRowStyle BackColor="#EBEBEB" BorderStyle="None" />
						<Columns>
							<asp:CommandField ShowSelectButton="True" >
							<ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                            </asp:CommandField>
							<asp:HyperLinkField DataTextField="wo_detail_current_master_id" Text="Part" 
								HeaderText="Part" />
							<asp:BoundField DataField="description" HeaderText="Description" />
							<asp:BoundField DataField="Location" HeaderText="Location" />
							<asp:BoundField DataField="QtyinStock" HeaderText="In Stock" >
								
								<ItemStyle HorizontalAlign="Center" />
								
							</asp:BoundField>
							<asp:BoundField DataField="_left" HeaderText="Pull">
								<ItemStyle HorizontalAlign="Center" />
								
							</asp:BoundField>
						</Columns>
						<HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="None" />
						<PagerSettings Visible="False" />
						<RowStyle BorderColor="#CCCCCC" BorderStyle="None" Height="40px" />
					</asp:GridView>
					<br />
					
					<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
						ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
						ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
						SelectCommand="SELECT        
a.wo_detail_current_master_id, 
CONCAT(FULL_PART_DESCRIPTION(a.wo_detail_current_master_id, true, d.country), IF(LENGTH(TRIM((wo_detail_current_notes))) &gt; 0, CONCAT(' \n\n****************\nNOTES: ',(wo_detail_current_notes),'\n****************'), ''))AS description, 
a.wo_detail_current_woprog_id,
round((a.wo_detail_current_qty_ordered - a.wo_detail_current_qty_committed),2) _left, 

IFNULL(GROUP_CONCAT((Concat(c.name,' '))), '') AS Location, 
SUM(IFNULL(b.qty, 0)) AS QtyinStock, 
e.tag_id
FROM            
wo_detail_current a 
LEFT OUTER JOIN
                         inventory_location b ON a.wo_detail_current_master_id = b.master_id AND b.id = a.business_unit_id LEFT OUTER JOIN
                         inventory_location_master c ON b.location_master_id = c.id  LEFT OUTER JOIN
                         business_unit d ON a.business_unit_id = d.id LEFT OUTER JOIN
                         inventory_item_master e ON a.wo_detail_current_master_id = e.master_id
WHERE
	(a.wo_detail_current_woprog_id = ?woid) AND 
	(a.wo_detail_current_master_id &lt; '99000') AND 
	(a.wo_detail_current_billtypeid NOT IN (3, 5, 6)) AND 
	(a.wo_detail_current_master_id &lt;&gt; '2139') AND
(a.wo_detail_current_master_id &lt;&gt; '9596') AND
(a.wo_detail_current_master_id &lt;&gt; '9595') AND
	c.type_id = 1
GROUP BY a.wo_detail_current_rec_no
ORDER BY round((a.wo_detail_current_qty_ordered - a.wo_detail_current_qty_committed),2) desc, location">
						<SelectParameters>
							<asp:ControlParameter ControlID="lb_wo_no" DefaultValue="" Name="woid" 
								PropertyName="Text" />
						</SelectParameters>
					</asp:SqlDataSource>
					
					<br />
				</asp:View>
			    <asp:View ID="vw_fixqtys" runat="server">
                    <asp:Label ID="admin_lb_warning0" runat="server" Font-Bold="True" Font-Size="11px" ForeColor="Red" Width="375px"></asp:Label>
                    <br />
                    <asp:GridView ID="gv_incorrect_qtys" runat="server" AutoGenerateColumns="False" BorderStyle="None" 
                        DataKeyNames="id" DataSourceID="SqlDataSource3" EnablePersistedSelection="True" 
                        GridLines="Vertical" 
                        onselectedindexchanged="gv_incorrect_qtys_SelectedIndexChanged" ShowHeaderWhenEmpty="True" Width="100%" CellPadding="4">
                        <AlternatingRowStyle BackColor="#EBEBEB" BorderStyle="None" />
                        <Columns>
                            <asp:HyperLinkField DataTextField="master_id" HeaderText="Part" Text="Part" />
                            <asp:BoundField DataField="description" HeaderText="Description" />
                            <asp:BoundField DataField="id" HeaderText="id" Visible="False" />
                            <asp:BoundField DataField="location_name" HeaderText="Location" />
                            <asp:BoundField DataField="reported_qty" HeaderText="Should Be" >
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="In Stock">
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <ItemTemplate>
                                   <asp:TextBox ID="tb_loc_fix" runat="server" Font-Size="13pt" Height="25px" Text='<%# Bind("QtyinStock") %>' TextMode="Number" Width="60px"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:CommandField SelectText="Save" ShowSelectButton="True">
                            <ItemStyle HorizontalAlign="Center" Font-Bold="True" Font-Size="14pt" />
                            </asp:CommandField>
                        </Columns>
                        <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="None" />
                        <PagerSettings Visible="False" />
                        <RowStyle BorderColor="#CCCCCC" BorderStyle="None" Height="40px" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
                        SelectCommand="SELECT
inventory_incorrect_levels.id,
inventory_incorrect_levels.location_id,
inventory_incorrect_levels.member_id AS EnteredBy_id,
EnteredBy.member_fullname AS EnteredBy_name,
inventory_incorrect_levels.date AS date_entered,
inventory_incorrect_levels.cleared_date,
inventory_incorrect_levels.cleared_by AS ClearedBy_id,
ClearedBy.member_fullname AS ClearedBy_name,
inventory_incorrect_levels.reported_qty,
inventory_incorrect_levels.notes,
inventory_incorrect_levels.master_id,
Location.`name` AS location_name,
inventory_incorrect_levels.incorrect_qty,
inventory_description.description,
inventory_location.qty QtyinStock,
Location.business_unit_id
FROM
inventory_incorrect_levels
LEFT JOIN member AS EnteredBy ON EnteredBy.Member_ID = inventory_incorrect_levels.member_id
LEFT JOIN member AS ClearedBy ON ClearedBy.Member_ID = inventory_incorrect_levels.cleared_by
INNER JOIN inventory_location_master AS Location ON inventory_incorrect_levels.location_id = Location.id
INNER JOIN inventory_description ON inventory_incorrect_levels.master_id = inventory_description.master_id
INNER JOIN inventory_location ON Location.id = inventory_location.location_master_id AND inventory_incorrect_levels.master_id = inventory_location.master_id 
                        where Location.business_unit_id=@business_unit_id and inventory_incorrect_levels.cleared_date is null ">
                        <SelectParameters>
                            <asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <br />
                </asp:View>
                <br />
			</asp:MultiView>

			<div style="text-align:right;margin-top:100px;padding:5px;">				
				<asp:Button ID="bt_signout" runat="server" Height="35px" onclick="bt_signout_Click" Text="Sign Out" Width="50%" UseSubmitBehavior="False" />
			</div>
			<dx:ASPxPopupControl ID="pop_invalid_qty" runat="server" ClientInstanceName="pop_invalid_qty" CloseAction="None" HeaderText="What Should the Qty Be?" Modal="True" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ModalBackgroundStyle-Opacity="50">
                <ClientSideEvents PopUp="function(s, e) {
	txt_pop_qty.SetFocus();
}" />
                <ModalBackgroundStyle Opacity="70">
                </ModalBackgroundStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <table style="width:100%;">
                            <tr>
                                <td colspan="3" style="text-align: center">
                                    <dx:ASPxTextBox ID="txt_pop_qty" runat="server" Width="170px" ClientInstanceName="txt_pop_qty" Native="True" Height="30px" Font-Size="16pt"></dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td><dx:ASPxTextBox ID="txt_final_call" runat="server" Width="170px" ClientInstanceName="txt_final_call" Native="True" Height="30px" ClientVisible="False"></dx:ASPxTextBox></td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_cancel_pop" runat="server" Text="Cancel" UseSubmitBehavior="False" OnClientClick="pop_invalid_qty.Hide();" />
                                </td>
                                <td>&nbsp;</td>
                                <td style="text-align: right">
                                    <asp:Button ID="btn_send_invalid_qty" runat="server" Text="Send-&gt;" OnClick="btn_send_invalid_qty_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
			<br />
			<dx:ASPxPopupControl ID="pop_search" runat="server" 
				ClientInstanceName="pop_search" CloseAction="CloseButton" 
				HeaderText="Search..." Height="300px" Modal="True" 
				PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
				Width="300px" AppearAfter="0" PopupAnimationType="None" 
				ShowPageScrollbarWhenModal="True" onwindowcallback="pop_search_WindowCallback" 
				PopupVerticalOffset="-150">
				<closebuttonimage url="~/images/icon/icon[minus].gif" width="25px"></closebuttonimage><HeaderStyle 
				HorizontalAlign="Left" VerticalAlign="Middle" /><modalbackgroundstyle opacity="0"></modalbackgroundstyle><contentcollection>
					<dx:PopupControlContentControl runat="server">
						<table style="width:100%;">
						<tr>
						<td width="100%" valign="top"><dx:ASPxPageControl 
								ID="pc_search" runat="server" ActiveTabIndex="0" ClientInstanceName="pc_search" Width="100%"><tabpages><dx:TabPage 
									Text="Sites I'm At Today"><contentcollection><dx:ContentControl runat="server">
									<asp:GridView 
										ID="gv_search0" runat="server" AutoGenerateColumns="False" 
										AutoGenerateSelectButton="True" BorderStyle="None" CellPadding="2" 
										DataKeyNames="id" EnablePersistedSelection="True" GridLines="Vertical" 
										OnSelectedIndexChanged="gv_search_SelectedIndexChanged" PageSize="100" 
										ShowHeader="False" Width="100%"><AlternatingRowStyle BackColor="#EBEBEB" 
										BorderColor="#CCCCCC" /><Columns><asp:BoundField DataField="id" 
											Visible="False" /><asp:BoundField DataField="num" HeaderText="WO" /><asp:BoundField 
											DataField="_name" HeaderText="Details" /></Columns><RowStyle 
										BorderColor="#CCCCCC" /><SelectedRowStyle BackColor="#CCCCCC" /></asp:GridView>
										</dx:ContentControl>
										</contentcollection>
										</dx:TabPage>
										<dx:TabPage Text="Search WO or Part"><contentcollection><dx:ContentControl runat="server"><asp:RadioButtonList 
											ID="rdo_search" runat="server" RepeatDirection="Horizontal" Width="100%" Font-Size="Large"><asp:ListItem Value="part">Part</asp:ListItem><asp:ListItem 
											Value="wo">Work Order</asp:ListItem></asp:RadioButtonList><table 
											style="width:100%;"><tr style="height: 40px"><td><dx:ASPxTextBox 
										ID="txt_search" runat="server" 
													AutoPostBack="True" ClientInstanceName="txt_search" Font-Names="Arial" 
													Font-Size="Large" NullText="... enter search text" 
													OnTextChanged="txt_search_TextChanged" Width="100%"><nulltextstyle 
													font-italic="True"></nulltextstyle></dx:ASPxTextBox></td></tr><tr><td><asp:GridView 
													ID="gv_search" runat="server" AutoGenerateColumns="False" 
													AutoGenerateSelectButton="True" BorderStyle="None" CellPadding="2" 
													DataKeyNames="id" EnablePersistedSelection="True" GridLines="Vertical" 
													OnSelectedIndexChanged="gv_search_SelectedIndexChanged" PageSize="100" 
													ShowHeader="False" Width="100%" EmptyDataText="No Results Found..."><AlternatingRowStyle BackColor="#EBEBEB" 
													BorderColor="#CCCCCC" /><Columns><asp:BoundField DataField="id" 
														Visible="False" /><asp:BoundField DataField="num" HeaderText="WO" /><asp:BoundField 
														DataField="_name" HeaderText="Details" /></Columns><EmptyDataRowStyle 
										BorderStyle="None" Font-Italic="True" /><RowStyle 
													BorderColor="#CCCCCC" /><SelectedRowStyle BackColor="#CCCCCC" /></asp:GridView></td></tr></table></dx:ContentControl></contentcollection></dx:TabPage></tabpages>
                            <ClientSideEvents ActiveTabChanged="function(s, e) {
	if (s.GetActiveTabIndex()=='0')
{
pop_search.PerformCallback();
}
	

}" />
                            <contentstyle>
													<Paddings PaddingTop="10px" /><Paddings PaddingTop="10px" /><Paddings PaddingTop="10px" /><Paddings PaddingTop="10px" /><Paddings PaddingTop="10px" /><Paddings PaddingTop="10px" /><Paddings PaddingTop="10px" /><Paddings PaddingTop="10px" /></contentstyle></dx:ASPxPageControl>
													</td>
													</tr>
													</table>
							</dx:PopupControlContentControl>
				</contentcollection>
			</dx:ASPxPopupControl>
			
			
		</ContentTemplate>
	</asp:UpdatePanel>