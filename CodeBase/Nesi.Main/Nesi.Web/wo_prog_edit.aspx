<%@ page language="C#" masterpagefile="~/IntraDefault.master" autoeventwireup="true" inherits="wo_prog_edit" title="" Codebehind="wo_prog_edit.aspx.cs" %>

<asp:content id="Content1" contentplaceholderid="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:content>
<asp:content id="Content2" contentplaceholderid="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphMasterSubMenu" runat="Server">
</asp:content>
<asp:content id="Content5" runat="server" contentplaceholderid="header_placeholder">
	<script type="text/javascript">
		function _wo_info(obj, init) {
			var _bgcolor = init ? "red" : "blue";
			$(obj).css({ "background-color": _bgcolor });
		}
		function load_po(id) {
			boing('/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid=' + id, 'PO', 1280, 720);
		}
		$(document).ready(function () {
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			bind_tooltips();
		});
		function bind_tooltips() {
			$(document).find(".opt").each(function () {
				var _props = {
					text: $(this).text(),
					value: $(this).attr('data-value'),
					url: $(this).attr('data-url'),
					show_notes: $(this).attr('data-show_notes'),
					notes_id: $(this).attr('data-notes_id')
				};
				_props.notes_url = _props.notes_url == undefined ? "" : _props.notes_url;
				_props.show_notes = _props.show_notes == undefined ? false : _props.show_notes;
				var _click = function (e) {
					if (e.ctrlKey) {
						window.open(_props.url);
					}
					else {
						location.href = _props.url;
					}
				}
				if (_props.show_notes) {
					$(this).click(_click).tip({
						title: _props.text,
						width: 550,
						show_notes: _props.show_notes,
						notes_id: _props.notes_id
					});
				}
				else {
					$(this).click(_click).tip({
						title: _props.text,
						width: 400
					});
				}
			});
		}
		function EndReqHandler() {
			bind_tooltips();
			please_wait("stop");
		}
		function BeginReqHandler() {
			please_wait("start");
		}
	</script>
</asp:content>

<asp:content id="Content4" contentplaceholderid="cphMasterBody" runat="Server">
	<asp:scriptmanager id="sm" runat="server"></asp:scriptmanager>
	<asp:updatepanel id="up" runat="server">
		<contenttemplate>
			<span id="spanMSG" runat="server"></span>

			<table border="0" cellpadding="2" cellspacing="0" style="font-size: 10pt; font-family: Arial" width="1040px">
				<tr>
					<td width="100%" valign="top" colspan="3">
						<div id="divCoName" runat="server" style="font-size: 25px; font-weight: bold;"></div>
					</td>
					<td valign="top" align="center" nowrap="nowrap">&nbsp;</td>
					<td align="center" valign="top">
						<div id="div_refresh_button" runat="server"></div>
					</td>
					<td align="right" valign="top" nowrap="nowrap">
						<div id="div_add_new_button" runat="server"></div>
					</td>
					<td valign="top" align="center">&nbsp;</td>
				</tr>
				<tr>
					<td align="left" nowrap="nowrap" valign="top"><span style="font-size: 10pt; vertical-align: middle">Only Show Work Orders for:</span></td>
					<td align="left" nowrap="nowrap" valign="top">
						<asp:dropdownlist id="ddlpm" runat="server" autopostback="True" datatextfield="name" datavaluefield="id" font-names="Arial" font-size="10pt" onselectedindexchanged="ddlpm_SelectedIndexChanged" width="150px">
						</asp:dropdownlist>
					</td>
					<td align="center" nowrap="nowrap" valign="top">&nbsp;</td>
					<td align="center" valign="top">&nbsp;</td>
					<td align="right" nowrap="nowrap" style="font-size: 10pt; vertical-align: middle" valign="top">&nbsp;</td>
					<td align="center" valign="top">&nbsp;</td>
				</tr>
				<tr>
					<td align="left" nowrap="nowrap" valign="top"><font size="2">Sorted By: </font></td>
					<td align="left" nowrap="nowrap" valign="top">
						<asp:dropdownlist id="DropDownList1" runat="server" autopostback="True" font-names="Arial" font-size="10pt" onselectedindexchanged="DropDownList1_SelectedIndexChanged" width="150px">
							<asp:listitem selected="True" value="WOProg_OpenDateTime">Scanned Date</asp:listitem>
							<asp:listitem value="WOProg_CustomerName">Customer Name</asp:listitem>
							<asp:listitem value="WOProg_BVWO">Work Order Number</asp:listitem>
						</asp:dropdownlist>
					</td>
					<td align="center" nowrap="nowrap" valign="top" width="100%">&nbsp;</td>
					<td align="center" valign="top">&nbsp;</td>
					<td align="right" nowrap="nowrap" style="font-size: 10pt; vertical-align: middle" valign="top">&nbsp;</td>
					<td align="center" valign="top">&nbsp;</td>
				</tr>
				<tr>
					<td align="center" nowrap="nowrap" valign="top" colspan="3">&nbsp;</td>
					<td align="center" valign="top">&nbsp;</td>
					<td align="right" nowrap="nowrap" style="font-size: 10pt; vertical-align: middle" valign="top">&nbsp;</td>
					<td align="center" valign="top">&nbsp;</td>
				</tr>
			</table>
			<asp:label id="ErrorLabel" runat="server" font-bold="True" font-names="Arial" font-size="Large" forecolor="Red" visible="False"></asp:label><br />
			<table border="0" cellpadding="2" id="prog_edit" style="overflow: hidden; font-family: Arial; height: 560px">
				<tr>
					<td valign="top" align="center" style="width: 200px; height: 343px" rowspan="2">
						<div class='lb_hdr'>Just Scanned</div>
						<div style=" height: 350px;" id="div_list_just_scanned" class='lb' runat="server"></div>
					</td>
					<td valign="top" align="center" rowspan="3" style="width: 200px;" rowspan="2">
						<div class='lb_hdr'>Initial Prep </div>
						<div style=" height: 615px;" id="div_list_initial_prep" class='lb' runat="server"></div>

					</td>
					<td valign="top" align="center" style="width: 200px; height: 343px" rowspan="2">
						<div class='lb_hdr'>Rework</div>
						<div style=" height: 350px;" id="div_list_rework" class='lb' runat="server"></div>
						<div style="font-weight: bold; color: #060; margin-bottom: 5px;" id="div_total_rework" runat="server"></div>
					</td>
					<td valign="top" align="center" style="width: 200px; height: 343px" rowspan="2">
						<div class='lb_hdr'>Waiting PM Approval</div>
						<div style=" height: 350px;" id="div_list_pm_approval" class='lb' runat="server"></div>
						<div style="font-weight: bold; color: #060; margin-bottom: 5px;" id="div_total_pm_approval" runat="server"></div>

					</td>
					<td valign="top" align="center" style="width: 200px; height: 175px">
						<div class='lb_hdr'>Waiting to be Invoiced</div>
						<div style=" height: 184px;" id="div_list_waiting_invoice" class='lb' runat="server"></div>
						<div style="font-weight: bold; color: #060; margin-bottom: 5px;" id="div_total_waiting_invoice" runat="server"></div>
					</td>
				</tr>
				<tr>
					<td align="center" style="width: 200px; height: 175px" valign="top">
						<div class='lb_hdr'>Waiting Parent BM Approval</div>
						<div id="div_list_waiting_other_branch" runat="server" class="lb" style=" height: 125px;">
						</div>
						<div id="div_total_waiting_other_branch" runat="server" style="font-weight: bold; color: #060; margin-bottom: 5px;"></div>
					</td>
				</tr>
				<tr>
					<td valign="top" align="center">
						<div class='lb_hdr'>Open POs</div>
						<div style=" height: 210px;" id="div_list_open_pos" class='lb' runat="server"></div>
					</td>
					<td valign="top" align="center">
						<div class='lb_hdr'>Questions</div>
						<div style=" height: 210px;" id="div_list_questions" class='lb' runat="server"></div>
					</td>

					<td valign="top" align="center">
						<div class='lb_hdr'>Waiting BM Approval</div>
						<div style=" height: 210px;" id="div_list_bm_approval" class='lb' runat="server"></div>
					</td>
					<td valign="top" align="center">
						<div class='lb_hdr'>Waiting Cust PO</div>
						<div style=" height: 210px;" id="div_list_cust_po" class='lb' runat="server"></div>
					</td>
				</tr>
				<tr>
					<td align="center" height="10px" valign="top">
						<div id="spanHoldPOTotal" runat="server" style="font-weight: bold; color: #060;">
						</div>
					</td>
					<td align="center" height="10px" style="width: 200px;" valign="top">
						<div id="div_total_initial" runat="server" style="font-weight: bold; color: #060;">
						</div>
					</td>
					<td align="center" height="10px" valign="top">
						<div id="div_total_questions" runat="server" style="font-weight: bold; color: #060;">
						</div>
					</td>
					<td align="center" height="10px" valign="top">
						<div id="div_total_bm_approval" runat="server" style="font-weight: bold; color: #060;">
						</div>
					</td>
					<td align="center" height="10px" valign="top">
						<div id="div_total_cust_po" runat="server" style="font-weight: bold; color: #060;">
						</div>
					</td>
				</tr>
			</table>
			<asp:label id="lblerrordebug" runat="server"></asp:label><br />
			<table cellpadding="2" cellspacing="0" style="width: 1040px;text-align:center;">
				<tr>
					<td width="102" style="color:#999;">Other Dept</td>
					<td width="102" style="">Dept 100</td>
					<td width="102" style="color:#f00;">Dept 200</td>
					<td width="102" style="color:navy">Dept 400</td>
					<td width="102" style="background-color:bisque;">Progress Billing</td>
					<td width="102" style="background-color:cyan">Quote</td>
					<td width="102" style="background-color:pink">On Hold</td>
					<td width="102" style="font-weight:bold;">Credit Card</td>
					<td width="102" style="background-color:#ff0">Is a Child WO</td>
					<td width="102" style="background-color:orange">Other Branch</td>
				</tr>
			</table>
			<br />

		</contenttemplate>
	</asp:updatepanel>

</asp:content>
