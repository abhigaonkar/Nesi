var addNewVendorPart = false;

var picklist =
{
	addline:
	{
		clear_zeros:
			function (s, e) {
				if (s.GetValue() == 0) {
					s.SetText('');
				}
				else {
					console.log(s.GetValue());
				}
			}
	},
	get_accessors:
		function () {
			var t = {
				origin: $('.hid_origin'),
				id: $(".hid_id"),
				is_exclude: $("#ctl00_cphMasterBody_hidPartIsExclude"),
				rev: $("#ctl00_cphMasterBody_hidRev"),
				footer_save: $("#ctl00_cphMasterBody_rp_Main_pnl_gv_which_footer_save"),
				cost: typeof (TextCost) != "undefined" && TextCost.GetInputElement() != null ? TextCost : null,
				bench_sell: $('.TextSell').find('input:text').val(),
				sell: typeof (addline_TextSell) != "undefined" && addline_TextSell.GetInputElement() != null ? addline_TextSell : null,
				sell_ext: typeof (addline_TMSellExt) != "undefined" && addline_TMSellExt.GetInputElement() != null ? addline_TMSellExt : null,
				bench_extd: $('.TMSellExt'),
				quote_extd: $('.TMQuoteExt'),
				division: $("#hidDivisionID"),
				qty: $(".QtyPart"),
				discount: $(".ASPxTextDiscount").size() == 0 ? 1 : $(".ASPxTextDiscount").val(),
				business_unit_id: $(".hid_business_unit_id"),
				error: $(".error_label"),
				info: $(".information_label"),
				mat_type: $("#ctl00_cphMasterBody_rp_Main_pnl_addnewline_ddlMatType"),
				com_qty: $("#ctl00_cphMasterBody_rp_Main_pnl_addnewline_TextComQty"),
				addline_comqty: $(".TextComQty").val(),
				location: typeof (addline_location) != "undefined" && addline_location.GetInputElement() != null ? addline_location : null,
			};
			return t;
		},
	vendor_part_toggle:
		function (obj) {
			var ta = $(obj).parents("table:first");
			var tr = ta.find("tr:first");
			var td = tr.find("td:nth-child(6)");
			var current = td.css("width");
			switch (current) {
				case "150px":
					td.css({ "width": "50px" });
					break;
				default:
					td.css({ "width": "150px" });
					break;
			}
		},
	vendor_part_no:
	{
		begin:
			function (s, e) {
				TextVendorPartNo.ClearItems();
				TextVendorPartNo.SetText("");
			},
		end:
			function (s, e) {
				var t = picklist.get_accessors();
				var vendor_part = $.trim($("#hid_vendor_part").val()).toString().toUpperCase();
				if (vendor_part != "" && TextVendorPartNo.FindItemByText(vendor_part) != null) {
					TextVendorPartNo.SetSelectedItem(TextVendorPartNo.FindItemByText(vendor_part));
					if (t.cost != null) {
						t.cost.SetValue(TextVendorPartNo.cpCOSTS[TextVendorPartNo.GetSelectedIndex()]);
					}
					$(".QtyPerPart").val(TextVendorPartNo.cpQTYPER[TextVendorPartNo.GetSelectedIndex()]);
					$("#hid_vendor_part").val("");
				}
				else {
					if (t.cost != null) {
						t.cost.SetValue(TextVendorPartNo.cpCOSTS[TextVendorPartNo.GetSelectedIndex()]);
					}
					$(".QtyPerPart").val(TextVendorPartNo.cpQTYPER[TextVendorPartNo.GetSelectedIndex()]);
					$("#hid_vendor_part").val("");
				}
				var cost = t.cost == null || t.cost.GetValue() == "" ? 0 : t.cost.GetValue() / 1;

				// Only set the value when adding new part
				if (addNewVendorPart === true) {
					addNewVendorPart = false;
				} else {
					TextVendorPartNo.SetValue('');
				}
			},
		click:
			function (s, e) {
				if ($.trim($(".TextPartNo").val()) == "") {
					alert("Please first enter a valid part number");
					$(".TextPartNo").focus();
				}
				else {
					pop_vendor_line.Show();
				}
			},
		index_change:
			function (s, e) {
				var t = picklist.get_accessors();
				if (t.cost != null) {
					t.cost.SetValue(TextVendorPartNo.cpCOSTS[s.GetSelectedIndex()]);
				}
				$(".QtyPerPart").val(TextVendorPartNo.cpQTYPER[s.GetSelectedIndex()]);
			}
	},
	wo:
	{
		block_scheduler:
			function (obj, row_id) {
				var query_string = { a: "block_schedule", blocking_schedule: $(obj).is(":checked") ? 1 : 0, row_id: row_id, woprog_id: $(".hid_id").val() };
				$.ajax({
					type: 'GET',
					url: "./pikclist.aspx",
					data: query_string,
					dataType: 'text',
					beforeSend:
						function () {
							var is_blocking = query_string.blocking_schedule == 1 ? "Setting blocking flag for this part." : "Releasing blocking flag for this part";
							please_wait("start", is_blocking);
						},
					success:
						function (t) {
							if (t != "SUCCESS") {
								alert(t);
							}
							else {
								agv.Refresh();
							}
						},
					complete:
						function () {
							please_wait("stop");
						},
					error:
						function (XMLHttpRequest, textStatus, errorThrown) {
							please_wait("stop");
						}
				});
			}
	},
	po:
	{
		new_vendor_price:
		{
			part_n_chk:
				function (s, e) {
					if ($.trim(s.GetText()) != "") {
						y = "check";
						cbp_vendor_line.PerformCallback("vendor_info|" + s.GetText());
					}
					else if (ASPxClientEdit.ValidateGroup("vs_new_vendor_line")) {
						lb_new_vendor_line_error.SetText("");
					}
				},
			cost_chk:
				function (s, e) {
					var cost_text = $.trim(s.GetText());
					var cost = cost_text == "" || isNaN(cost_text) ? 0 : cost_text / 1;
					var qty_text = $.trim(t_new_vendor_qty.GetText());
					var qty = qty_text == "" || isNaN(qty_text) ? 0 : qty_text / 1;

					if (cost_text != "") {
						if (cost <= 0) {
							alert("Not a valid cost.");
							s.SetText("");
							s.Focus();
						}
						else if (cost > 0 && qty > 0 && cost / qty <= 0.0005) {
							alert("This would result in a sell price of zero if it was ever used.");
							s.SetText("");
							s.Focus();
						}
					}
				},
			qty_check:
				function (s, e) {
					var cost = $.trim(t_new_vendor_cost.GetText());
					cost = cost == "" ? 0 : cost / 1;
					var this_text = $.trim(s.GetText());
					if (this_text != "") {
						if (isNaN(this_text)) {
							alert("Not a valid quantity, needs to be numeric.");
							s.SetText("");
							s.Focus();
						}
						else if (cost > 0 && cost / this_text <= 0.0005) {
							alert("This would result in a sell price of zero if it was ever used.");
							s.SetText("");
							s.Focus();
						}
					}
				},
			save:
				function (s, e) {
					if (ASPxClientEdit.ValidateGroup("vs_new_vendor_line") && lb_new_vendor_line_error.GetText() == "") {
						addNewVendorPart = true;
						y = "save";
						cbp_vendor_line.PerformCallback("save|true");
					}
				}
		}
	},
	update_error: function (msg) {
		var t = this.get_accessors();
		t.error.text(msg);
		t.error.css({ "display": msg != "" ? "block" : "none" });
	},
	update_info: function (msg) {
		var t = this.get_accessors();
		t.info.text(msg);
		t.info.css({ "display": msg != "" ? "block" : "none" });
	}
};

function do_sel_all(s, e) {
	$(document).find(".sel_box").each(function () {
		$(this).children("input:checkbox").attr("checked", s.GetChecked());
		row_selection_handler($(this).children("input:checkbox"));
	});
}




function reqdate_edit(obj) {
	var num_checked = poll_checked();
	if (num_checked == 0) {
		alert("You must first select some row(s) to be edited");
	}
	else {
		pop_date_required.Show();
	}
}
function transfer_multiple_toggle(obj) {
	var num_checked = poll_checked();
	if (num_checked == 0) {
		alert("You must first select some row(s) to be transferred");
	}
	else {
		TransferItems(0, false);
	}
}
function use_wo_handler(s) {
	var parent_row = $(s).parents("tr:first");
	var combo = parent_row.find(".loc_combo");
	if ($(s).is(":checked")) {
		combo.attr("disabled", true);
	}
	else {
		combo.removeAttr("disabled");
	}
}
function poll_checked() {
	var num_checked = 0;
	$(document).find(".sel_box").each(function () {
		if ($(this).children("input:checkbox").is(":checked")) {
			num_checked++;
		}
	});
	return num_checked;
}
function billtype_edit(obj) {
	var num_checked = poll_checked();
	if (num_checked == 0) {
		alert("You must first select some row(s) to be edited");
	}
	else {
		pop_billtype_edit.Show();
	}
}
function handle_xfer_save(s, e) {
	e.processOnServer = false;
	var sub_g = gv_xfer_multiple;
	var sub_page_index = sub_g.GetPageIndex();
	var sub_page_size = sub_g.pageRowSize;
	var sub_l_n = sub_page_index * sub_page_size;
	var sub_lines = sub_g.pageRowCount + sub_l_n;
	var selected_wo = xfer_wo_combo.GetValue();
	var should_send = true;
	for (var l = sub_l_n; l < sub_lines; l++) {
		var this_use_wo = $(sub_g.GetRow(l)).find(".xfer_chk_wo").is(":checked");
		if (this_use_wo && selected_wo == null) {
			should_send = false;
		}
	}
	if (should_send) {
		TransferItems(0, true);
	}
	else {
		alert("You have chosen to use a work order, but a work order is not selected");
	}
}
function check_commit(s, e) {
	var g = agv;
	var page_index = g.GetPageIndex();
	var page_size = g.pageRowSize;
	var t = picklist.get_accessors();

	var o = {};
	o.html = "<table>";
	o.counter = 0;
	o.valid = 0;
	o.zero_parts = 0;
	var l_n = page_index * page_size;
	var lines = g.pageRowCount + l_n;

	for (var l = l_n; l < lines; l++) {
		if (isNaN($(g.GetRow(l)).find(".recqty").val() / 1)) {
			alert("There is an invalid quantity in your submission.\nBreaking Submittal.");
			e.processOnServer = false;
			return false;
		}
		var n_combo_locs = $(g.GetRow(l)).find(".combo_location").size();
		var loc = n_combo_locs > 0 && $.trim($(g.GetRow(l)).find(".combo_location").find("input:text").val()) != ""
			? $.trim($(g.GetRow(l)).find(".combo_location").find("input:text").val())
			: $.trim($(g.GetRow(l)).find(".combo_location").text() != "")
				? $.trim($(g.GetRow(l)).find(".combo_location option:selected").text())
				: "";
		var qty = $(g.GetRow(l)).find(".recqty").val() / 1;
		var qty_avail = $(g.GetRow(l)).find(".qty_avail").text() / 1;
		var part_no = $(g.GetRow(l)).find(".part_no").text() / 1;
		var rec_no = $(g.GetRow(l)).find(".rec_no").text() / 1;
		var qty_req = $(g.GetRow(l)).find(".qty_req_back ") == null
			? 0
			: $(g.GetRow(l)).find(".qty_req_back ").text() / 1;
		var qty_com = $(g.GetRow(l)).find(".qty_com_back ") == null
			? 0
			: $(g.GetRow(l)).find(".qty_com_back ").text() / 1;
		if (t.origin.val() == "purchaseorder") {
			if (qty > 0 && qty_req > 0 && qty_com + qty > qty_req) {
				alert("For part number " + part_no + ", rec #" + rec_no + ", you have tried to receive more than ordered. Please edit the line and first increase the ordered quantity before trying to receive additional on this line.");
				e.processOnServer = false;
				return false;
			}
			else if (qty < 0 && qty_req > 0 && qty_com + qty < 0) {
				alert("For part number " + part_no + ", rec #" + rec_no + ", you have tried to unreceive past the zero point, not allowed.");
				e.processOnServer = false;
				return false;
			}

			if (qty < 0 && qty_req < 0 && qty_com + qty < qty_req) {
				alert("For part number " + part_no + ", rec #" + rec_no + ", you have tried to unreceive more than ordered. Please edit the line and first decrease the ordered quantity before trying to unreceive additional on this line.");
				e.processOnServer = false;
				return false;
			}
			else if (qty > 0 && qty_req < 0 && qty_com + qty < 0) {
				alert("For part number " + part_no + ", rec #" + rec_no + ", you have tried to unreceive past the zero point, not allowed.");
				e.processOnServer = false;
				return false;
			}

		}

		var is_exclude = $(g.GetRow(l)).find(".part_link").attr("data-is_exclude") == "true";
		var allowed_to_stock = $(g.GetRow(l)).find(".part_link").attr("data-allowed_to_stock") == "true";
		if (part_no == 0 && qty > 0) {
			o.zero_parts++;
		}
		if (qty != 0 && t.origin.val() == "purchaseorder") {
			o.valid++;
			o.part = part_no;
			var checked = $(g.GetRow(l).cells[20]).find('input:checkbox').is(":checked");
			var disabled = "";
			if (g.cp_is_wo[l]) {
				o.part = part_no;
				o.desc = $(g.GetRow(l)).find(".description").text();
				checked = checked || is_exclude ? "checked='checked'" : "";
				disabled = is_exclude ? " disabled='disabled'" : "";
				o.html += "<tr><td><input type='checkbox' data-row_n='" + l + "' onchange='check_commit_wo(this)' id='cb_" + l + "' " + checked + " " + disabled + " /></td><td style='width:75px' align='left'><label for='cb_" + l + "'><b>" + o.part + " </b></label></td><td align='left'><u><label for='cb_" + l + "' style='cursor:pointer;'>" + o.desc + "</label></u></td></tr>";
				if (checked) {
					check_commit_wo_direct(l, agv.GetRow(l), true);
				}
				o.counter++;
			}
		}
		if (qty != 0 && !isNaN(qty)) {
			if (loc == "" && !is_exclude && part_no < 900000 && allowed_to_stock) {
				alert("For part number " + part_no + ", there isn't a location chosen to commit from");
				e.processOnServer = false;
				return false;
			}
			else {
				o.valid++;
			}
		}
	}

	o.html = "<table><tr><td><input type='checkbox' data-row_n='1000' onclick='check_commit_wo_select_all(this," + lines + ")' id='cb_1000' /></td><td style='width:75px' align='left'><b>Select All</b></td><td align='left'></td></tr>" + o.html;

	//if (o.zero_parts > 0) {
	//    alert("Cannot receive a comment line");
	//    return;
	//}
	if (o.valid > 0 && o.counter > 0 && t.origin.val() == "purchaseorder") {
		o.html += o.counter > 0 ? "</table>" : "";
		pop_commit.Show();
		window.location.hash = "top";
		panel_pop.SetContentHTML(o.html);
	}
	else if (o.valid == 0 && t.origin.val() == "purchaseorder") {
		alert("Can't receive zero items");
		e.processOnServer = false;
		return false;
	}
	else if (o.valid == 0 && t.origin.val() == "workorder") {
		alert("Can't commit zero items");
		e.processOnServer = false;
		return false;
	}

	else if (o.valid > 0 && t.origin.val() == "workorder") {
		e.processOnServer = false;
		final_receival.DoClick();
	}
	else if (o.valid > 0) {
		e.processOnServer = false;
		final_receival.DoClick();
	}
}



$(".Description").on('keyup', function (e) {
	if (e.keyCode == 9) { // <- here confirm that tab is pressed.
		e.preventDefault(); // <- Prevent defaul functionality of tab.
		// your code if tab pressed.
	}
	if ($('.Description').val().length < 1) {
		$(".Description").css("background-color", "yellow");

	}
	else {
		$(".Description").css("background-color", "white");
	}
});

$(".TextPartNo").on('keyup', function (e) {
	if (e.keyCode == 9) { // <- here confirm that tab is pressed.
		e.preventDefault(); // <- Prevent defaul functionality of tab.
		// your code if tab pressed.
	}
	console.log(e.keyCode);
	$(".Description").css("background-color", "white");

});



//   setTimeout("$('#part_95959596_notice').remove();", 500);
//   setTimeout("$('#part_777_notice').remove();", 500);


function description_onfocus(obj) {

	$(obj).inventory(
		{
			business_unit_id: ctl00_cphMasterBody_hidCompanyID.value,
			vendor_id: ctl00_cphMasterBody_hidVendorID.value,
			force_clickable: ctl00_cphMasterBody_hidForceClickable.value,
			origin: ctl00_cphMasterBody_hidOrigin.value,
			click: function (row) {
				var t = picklist.get_accessors();
				if (ctl00_cphMasterBody_hidOrigin.value == 'workorder' && row.costprice < 0) {
					return false;
				}
				var sellprice = '0';
				var refqty = '0';
				var cost = '0';
				var testdesc = row.description;
				var used_sell = row.sellprice == 0 ? row.global_price_sell : row.sellprice;
				$('.TextPartNo').val(row.master_id);
				//MH (devex) - $('.TextSell').val(used_sell);
				try {
					addline_TextSell.SetValue(used_sell);
					$('.TMSellExt').val(used_sell);
				}
				catch (ex) { }
				$('.TMQuoteExt').val(used_sell);
				$('.VendPartNo').val(row.vendor_code);
				$('.TextAvailQty').text(row.onhand_qty);
				if ((row.vendor_qty == '') || (row.vendor_qty == '0')) {
					$('.QtyPerPart').val('1');
				}
				else {
					$('.QtyPerPart').val(row.vendor_qty);
				}
				$('#ctl00_cphMasterBody_hidPartIsExclude').val(row.is_exclude);
				if (ctl00_cphMasterBody_hidOrigin.value == 'workorder' || ctl00_cphMasterBody_hidOrigin.value == 'quote') {
					if (t.cost != null) {
						t.cost.SetValue(row.costprice);
					}
				}
				else {
					cost = row.vendor_sell;
					if (cost == 'undefined') {
						cost = '0';
						if (t.cost != null) {
							t.cost.SetValue(cost);
						}
					}
					else {
						if (t.cost != null) {
							t.cost.SetValue(row.vendor_sell);
						}
					}
				}
				try {
					if (addline_location != undefined && addline_location != null) {
						addline_location.PerformCallback(row.master_id);
						control_location_stock();
					}
				}
				catch (err) {
				}
				if (row.onhand_qty > 0) {
					$('.TextAvailQty').parents('td:first').animate({ 'background-color': '#090' });
					$('.TextAvailQty').parents('td:first').css({ 'color': '#fff', 'font-weight': 'bold', 'border-top': 'solid 1px #ccc' });
				}
				else {
					$('.TextAvailQty').parents('td:first').animate({ 'background-color': '#900' });
					$('.TextAvailQty').parents('td:first').css({ 'color': '#fff', 'font-weight': 'normal', 'border-top': 'solid 1px #ccc' });
				}
				if (refqty == '0') {
					refqty = '';
				}
				if (ctl00_cphMasterBody_hidOrigin.value == 'purchaseorder') {
					TextVendorPartNo.PerformCallback();
					$('.Description').attr('data-tag_id', row.tag_id);
					try {
						if (addline_location != undefined && addline_location != null) {
							addline_location.PerformCallback(row.master_id);
							control_location_stock();
						}
					}
					catch (err) {
					}
				}
				if (row.master_id == 777) {
					$('.Description').val('').focus().tip({ remove: true });
					var _o = $('.Description').offset();
					_o.height = $('.Description').height();
					$('.Description').parent().append("<div id='part_777_notice' style='padding:3px;background-color:#900;color:#fff;width:250px;height:20px;font-size:11px;font-weight:bold;position:absolute;top:" + (_o.top - _o.height) + "px;left:" + _o.left + "px;'>Please supply a description.</div>");
					setTimeout("$('#part_777_notice').remove();", 3500);
					$('.TextComQty').val('0').attr('disabled', true);
					//MH (devex) - $('.TextSell').val('0').attr('disabled', true);
					try {
						addline_TextSell.SetValue(0);
						addline_TextSell.SetEnabled(true);
					}
					catch (e) { }
					if (t.cost != null) {
						t.cost.SetValue('0');
						t.cost.SetEnabled(false);
					}
				}
				else if (row.master_id == 55555 || row.master_id == 55556 || row.master_id == 55557 || row.master_id == 55558 || row.master_id == 55559) {
					$('.Description').val('').focus().tip({ remove: true });
					var _o = $('.Description').offset();
					_o.height = $('.Description').height();
					$('.Description').parent().append("<div id='part_95959596_notice' style='padding:3px;background-color:#900;color:#fff;width:250px;height:20px;font-size:11px;font-weight:bold;position:absolute;top:" + (_o.top - _o.height - 7) + "px;left:" + _o.left + "px;'>Please supply a description.</div>");
					//setTimeout("$('#part_95959596_notice').remove();", 3500);
					try {
						addline_TextSell.SetValue(0);
						addline_TextSell.SetEnabled(true);
					} catch (e) { }
				}
				else {
					if (ctl00_cphMasterBody_hidOrigin.value != 'groupings') {
						QtyPartCIN.SetText(refqty);
						$('.QtyPart').focus();
						$('#ctl00_cphMasterBody_rp_Main_pnl_addnewline_TextQty_I').val(refqty);
						$('#ctl00_cphMasterBody_rp_Main_pnl_addnewline_TextQty').focus();

					}
					$(".Description").css("background-color", "white");
					$('.Description').val(testdesc).attr('data-tooltip', row.description).tip({ title: 'Full Description' });
					$('.TextComQty').val('0').removeAttr('disabled');
				}
			}
		});
	desc_resize(this, true);
}
function cost_editor(s, e) {
	var cost = s.GetText();
	var origin = $('.hid_origin').val();
	var qty = agv.GetEditor('qty').GetValue();
	var rec_qty = origin != 'purchaseorder' ? 0 : agv.GetEditor('qtyrec').GetValue();
	var newprice = 0;
	var partno = agv.GetEditor('part_no').GetValue();

	if (partno != 2139) {
		if (origin == 'workorder') {
			qty = agv.GetEditor('qtyrec').GetValue();
		}
		if (origin != 'purchaseorder') {
			var price = GetNewPrice(cost, qty);
			price = $('.hid_sellprice').val();
			newprice = GetSellPrice(cost, qty);
			newprice = $('.hid_sellprice').val();
			var extprice = newprice * qty;
			price = roundNumber(newprice, 3);
			extprice = roundNumber(extprice, 3);
			agv.GetEditor('sell').SetValue(price);
			agv.GetEditor('extTandM').SetValue(extprice);
			if (agv.GetEditor('extended_per') != null) {
				agv.GetEditor('extended_per').SetValue(extprice);
			}
		}
		else {
			var newcost = qty * cost;
			var newrec_cost = rec_qty * cost;
			newcost = roundNumber(newcost, 4);
			newrec_cost = roundNumber(newrec_cost, 4);
			agv.GetEditor('extTandM').SetValue(newcost);
			agv.GetEditor('extended_per').SetValue(newrec_cost);
		}
	}
}
function dt_req_click(s, e) {
	if (mass_date_req.GetDate() == null) {
		e.processOnServer = false;
		alert("Please select a date to set these row(s) to");
	}
	else {
		pop_date_required.Hide();
	}
}
function billtype_edit_click(s, e) {
	if (mass_billtype_edit.GetValue() == null) {
		e.processOnServer = false;
		alert("Please select a bill type to set these row(s) to");
	}
	else {
		pop_billtype_edit.Hide();
	}
}
function row_selection_handler(obj) {
	var tar = $(obj).parents("tr:first").find(".dxgv__cci").children("img[alt='delete_multiple']");
	var is_expanded = tar.attr("data-is_expanded");
	if (tar.length > 0 && (is_expanded == "" || is_expanded == null || is_expanded == undefined)) {
		tar.css({ "width": "16px", "height": "16px" }).attr("data-is_expanded", "true");
	}
	else if (is_expanded == "true") {
		tar.css({ "width": "0px", "height": "0px" }).attr("data-is_expanded", "");
	}
	tar = null;
	tar = $(obj).parents("tr:first").find(".transfer_multiple");
	is_expanded = tar.attr("data-is_expanded");
	if (tar.length > 0 && (is_expanded == "" || is_expanded == null || is_expanded == undefined)) {
		//tar.show().attr("data-is_expanded", "true");
	}
	else if (is_expanded == "true") {
		//tar.hide().attr("data-is_expanded", "");
	}
}
function delete_multiple(s, e) {
	if (confirm("Please confirm that you want to delete the selected line(s)")) {
		agv.PerformCallback("delete_multiple");
	}
}
function zero_chk(obj, isblur) {
	if (!isblur) {
		if ($(obj).val() == "0") {
			$(obj).val("");
		}
	}
	else {
		if ($(obj).val() == "") {
			$(obj).val("0");
		}
	}
}
var x;
var y;

var is_processing = 0;
function process_handler(add) {
	// Matt: I made this a function to more easily view the current value of is_processing, without being in Chrome's developer mode
	if (add) {
		is_processing++;
	}
	else {
		is_processing--;
	}
}
function kit_builder() {
	var defs = {
		a: "kitted_handler",
		id: add_kit.GetValue(),
		name: add_kit.GetText()
	};
	var t = picklist.get_accessors();

	if (t.mat_type.val() == 3) // only gets called if it's a kit
	{
		var tooltip = "";
		$.get("./pikclist.aspx", defs,
			function (j) {
				var o = $.parseJSON(j);
				if (o.status == undefined) {
					tooltip = "<table cellspacing='0' cellpadding='5' style='font-weight:bold;width:100%;background-color:#555;'><thead><tr style='background-color:#8f8;color:#000;'><th align='center'>#</th><th>Description</th><th width='50' align='center'>QTY</th></tr></thead><tbody>";
					$.each(o.items, function (i, oi) {
						tooltip += "<tr><td align='center' style='border-right:solid 1px #000;'>" + oi.master_id + "</td><td style='border-right:solid 1px #000;'>" + oi.description + "</td><td align='center'>" + oi.qty + "</td></tr>";
					});
					tooltip += "</tbody></table>";
				}
				else {
					tooltip = o.message;
					tooltip = o.message;
				}
				$(".add_kit").attr("data-tooltip", tooltip).tip({ title: "<strong style='color:#fff;font-family:arial black'>Kit (" + defs.name + ") Contents</strong>", width: 550 });
			});
	}
}
function get_accessors() {
}

var createdCodeID = 0;
function check_save(s, e) {
	if (is_processing > 0) {
		e.processOnServer = false;
		return false;
	}
	$(".Description").focus().blur();
	var t = picklist.get_accessors();
	if (t.origin.val() == "workorder") {
		var comm_qty = $('.add_line').find(".TextComQty").val() / 1;
		var avail_qty = $('.add_line').find(".qty_avail").text() / 1;
		if (t.cost != null && t.cost.GetValue() > 0 && t.bench_sell == 0) {
			alert("It looks like you pressed the save button before we could get the sell price for this part... please try again in a moment.");
			cost_handler(t.cost, null);
			e.processOnServer = false;
			return false;
		}
		var is_exclude = t.is_exclude.val() == "True";

		var mat_type = t.mat_type.val() / 1;
		if (!is_exclude && comm_qty > 0 && mat_type == 1 && price_obj.allowed_to_stock == "True") {
			if (addline_location.GetText() == "") {
				alert("Please first select a location to commit from");
				e.processOnServer = false;
				return false;
			}
		}
	}
	else {
		var cost = t.cost != null ? t.cost.GetValue() : 0;
		var qtyper = $('.QtyPerPart').val();
		var actualcost = cost / qtyper;
		var checkpart = $('.TextPartNo').val();
		var qty = $('.QtyPart').val();

		if (t.origin.val() == "purchaseorder") {
			// Only for Po items
			var currentUser = $(".hid_Current_Login_User").val();
			var businessUnit = $(".hid_business_unit_id").val();
			var poID = $(".hid_id").val();
			var quantity = $('.QtyPart').val();
			var currentCodeID = TextVendorPartNo.GetValue();
			if (createdCodeID > 0 && (createdCodeID == currentCodeID)) {
				// ReEnter, no checks.
				// console.log(createdCodeID + " / " + currentCodeID);
			} else {

				if (t.is_exclude.val() === "False") {
					createdCodeID = checkBetterPrice(businessUnit,
						checkpart,
						cost,
						qtyper,
						quantity,
						currentUser,
						poID);
					if (createdCodeID > 0) {
						// Let users check ui.
						e.processOnServer = false;
						return false;
					}
				}
			}
		}

		var save = CheckPOCost(actualcost, checkpart, qtyper, qty);
		var origin = $('.hid_origin').val();
		if (!save) {
			e.processOnServer = false;
		}
		else {
			//LoadingPanel.Show();
		}
		bind_tooltips();
	}
}
function control_location_stock(s, e) {
	if (s == null || s == undefined || s.uniqueID.match(/addline/g)) {
		s = addline_location;
	}
	var t = picklist.get_accessors();
	var avail_box = $(s.mainElement).parents("tr:first").find(".qty_avail");
	var part_no_box = $(s.mainElement).parents("tr:first").find(".part_no");
	var query_string = {};

	if (s.uniqueID.match(/addline/g)) {
		query_string = {
			a: "location_qty",
			business_unit_id: t.business_unit_id.val(),
			master_id: part_no_box.val(),
			location_id: s.GetValue()
		};
	}
	else {
		query_string = {
			a: "location_qty",
			business_unit_id: t.business_unit_id.val(),
			master_id: part_no_box.text(),
			location_id: s.GetValue()
		};
	}
	if (query_string.location_id != null) {
		$.ajax({
			type: 'GET',
			url: "./pikclist.aspx",
			data: query_string,
			dataType: 'text',
			beforeSend:
				function () {
					please_wait("start", "Fetching Available Quantity");
				},
			success:
				function (t) {
					avail_box.text(t);
				},
			complete:
				function () {
					please_wait("stop");
				},
			error:
				function (XMLHttpRequest, textStatus, errorThrown) {

				}
		});
	}
}
function swapColumns(table_id, colIndex1, colIndex2) {
	var table = document.getElementById(table_id);
}
function desc_resize(obj, isExpanding) {
	return true;
	if ($(obj).attr("data-resizer")) {
		if ($(obj).attr("data-resizer") == "true") {
			$(obj).attr("data-resizer", false);
			$(obj).animate({ "width": $(obj).attr("data-resizer-width") });
		}
		else {
			$(obj).attr("data-resizer", true);
			$(obj).animate({ "width": "350px" });
		}
	}
	else {
		$(obj).attr("data-resizer", true);
		$(obj).attr("data-resizer-width", $(obj).width() + "px");
		$(obj).css({ "width": "350px" });
	}
}
//swapColumns("add_line_table", 5, 1);


function part_handler(obj, e) {
	var _parent = $(obj).parents("tr:first");
	var key = e.keyCode ? e.keyCode : e.which;
	var t = picklist.get_accessors();
	if (key == 13) {
		return false;
	}
	$(".Description").css("background-color", "white");
	$(".TextCost").css("background-color", "white");
	_vars = {
		a: "g",
		id: $(obj).val(),
		business_unit_id: t.business_unit_id.val(),
		origin: t.origin.val()
	};
	if (_vars.id == 55560) {
		alert("You can not add a Pure Revenue Line to a Purchase Order");
		$(obj).val("").focus();
		return false;
	}
	if (_vars.id == 777 && _vars.origin != "workorder") {
		alert("Part 777 can't be added to anything but a work order");
		$(obj).val("").focus();
		return false;
	} if (_vars.id == 60000) {
		return false;
	}
	if ($(obj).attr('data-original_value') == undefined) {
		$(obj).attr('data-original_value', "");
	}
	if (_vars.id == "" && _vars.id != $(obj).attr('data-original_value')) {
		$('.Description').val("").attr("data-tooltip", "").unbind('focus').attr('data-is_inventory', 'false').css({ 'cursor': 'text' });
		$("#tooltip_window").remove();
		if (t.cost != null) {
			t.cost.SetEnabled(true);
		}
		//MH (devex) - t.bench_sell.val('');
		if (_vars.origin != "purchaseorder") {
			if (addline_TextSell) {
				addline_TextSell.SetValue('');
			}
		}
		//MH (devex) - t.bench_extd.val('');
		addline_TMSellExt.SetValue("");
		t.quote_extd.val('');
		t.qty.val('');
		$(obj).removeAttr('data-original_value');
	}
	else if ($(obj).attr("data-original_value") != _vars.id) {
		var in_error = false;
		if ($(obj).val().trim() != "") {
			x = $.ajax({
				type: "GET",
				url: "/_tools/part_info/index.aspx",
				data: _vars,
				beforeSend: function () {
					$('.Description').val("");
					$(".QtyPart").val("");
					//MH (devex) - $('.TextSell').val("");
					try { addline_TextSell.SetValue(""); } catch (e) { }
					if (t.cost != null) {
						t.cost.SetValue("");
					}
					$('.TMQuoteExt').val("");
					//MH (devex) - $('.TMSellExt').val("");
					try {
						addline_TMSellExt.SetValue("");
					}
					catch (ex) { }
					$('.TextComQty').val("");
					$('.TextAvailQty').text("");
					if (_vars.origin == "purchaseorder") {
						TextVendorPartNo.PerformCallback();
					}
					process_handler(true);
				},
				success: function (xml) {
					if ($(xml).find('error').size() > 0) {
						var error = $(xml).find('error').text();
						alert(error);
						in_error = true;
						$(obj).val("").focus();
					}
					else {
						var description = "";
						if (_vars.id == 777) {
							$('.TextComQty').val("0").attr("disabled", true);
							//MH (devex) - $('.TextSell').val('0').attr('disabled', true);
							addline_TextSell.SetValue("0");
							addline_TextSell.SetEnabled(false);
							if (t.cost != null) {
								t.cost.SetValue('0');
								t.cost.SetEnabled(false);
							}
						}
						else {
							description = $(xml).find('description_full').text();
						}
						if (description != "") {
							$('.Description').val(description).attr("data-tooltip", description).tip({ title: "Full Description" });
						}
						else {
							$('.Description').tip({ remove: true });
						}
						var sell_price = $(xml).find('sell_price').text();
						var exclude = $(xml).find('exclude_part').text();
						var tag_id = $(xml).find('tag_id').text();
						var onhand = $(xml).find('onhand').text() / 1;
						var cost = $(xml).find('cost_price').text();
						var should_be_part = $(xml).find('should_be_part').text();
						$(obj).val(should_be_part);
						_vars.id = should_be_part;
						$(".Description").attr('data-tag_id', tag_id);
						cost = cost == 0 ? "" : cost;
						if (_vars.origin == "quote" && (sell_price / 1) == 0) {
							alert("Part doesn't have a sell price");
							$(obj).val("").focus();
							return false;
						}

						if (_vars.origin != "purchaseorder") {
							if (t.cost != null) {
								t.cost.SetEnabled(false);
							}
						}
						t.is_exclude.val(exclude);
						if (exclude == "True" && _vars.id != 777) {
							//MH (devex) - t.bench_sell.val('');
							try { addline_TextSell.SetValue(''); } catch (e) { }
							if ((t.origin.val() == "workorder") || (t.origin.val() == "quote")) {
								if (t.cost != null) {
									t.cost.SetEnabled(true);
								}
								var _o = $(".TextCost").offset();
								_o.height = $(".TextCost").height();
								$(".TextCost").val("");
								$(".TextCost").css("background-color", "yellow");
								if (_vars.id == 2139) {
									t.sell.SetEnabled(true);
									t.location.SetEnabled(false);
								}
							}
							else if (t.origin.val() == "purchaseorder") {
								if (t.cost != null) {
									t.cost.SetEnabled(true);
								}
							}
							else {
								if (t.cost != null) {
									t.cost.SetEnabled(false);
								}
							}
						}
						else {
							document.getElementById("ctl00_cphMasterBody_hidPartIsExclude").value = 'False';
							if (t.cost != null) {
								t.cost.SetValue(cost);
							}
							//MH (devex) - t.bench_sell.val(sell_price);
							try { addline_TextSell.SetValue(sell_price); } catch (e) { }
							//addline_TextSell.SetValue(sell_price);
							if (onhand > 0) {
								$('.TextAvailQty').parents("td:first").animate({ "background-color": "#090" });
								$('.TextAvailQty').parents("td:first").css({ "color": "#fff", "font-weight": "bold", "border-top": "solid 1px #ccc" });
							}
							else {
								$('.TextAvailQty').parents("td:first").animate({ "background-color": "#900" });
								$('.TextAvailQty').parents("td:first").css({ "color": "#fff", "font-weight": "normal", "border-top": "solid 1px #ccc" });
							}
							$('.TextAvailQty').text(onhand);
							$('.TMQuoteExt').val("");
							//MH (devex) - $('.TMSellExt').val("");
							try { addline_TMSellExt.SetValue(""); } catch (ex) { }
							$("#ctl00_cphMasterBody_rp_Main_pnl_addnewline_TextQty_I").val(" ");
						}
						try {
							if (addline_location != undefined && addline_location != null) {
								addline_location.PerformCallback(should_be_part);
								control_location_stock();
							}
						}
						catch (err) {
						}
					}
				},
				complete: function () {
					try {
						$('.Description').css({ "background-image": "" });
						$(obj).attr('data-original_value', _vars.id);
						if (!in_error && _vars.origin != "purchaseorder") {
							if (_vars.id == 777) {
								var _o = $(".Description").offset();
								_o.height = $(".Description").height();
								$(".Description").parent().append("<div id='part_777_notice' style='background-color:transparent;color:red;width:150px;font-size:11px;font-weight:bold;position:absolute;top:" + (_o.top + 5) + "px;left:" + (_o.left + 50) + "px;'>Please supply a description.</div>");
								setTimeout("$('#part_777_notice').remove();", 3500);
							}
							else if (_vars.id == 55555 || _vars.id == 55556 || _vars.id == 55557 || _vars.id == 55558 || _vars.id == 55559) {
								var _o = $(".Description").offset();
								_o.height = $(".Description").height();
								$(".Description").css("background-color", "yellow");
								$(".Description").val("");
							}
						}
						else if (_vars.origin == "purchaseorder") {
							if (_vars.id == 55555) {
								var _o = $(".Description").offset();
								_o.height = $(".Description").height();

								$(".Description").css("background-color", "yellow");
								$(".Description").val("");
							}
						}
					}
					catch (ex) {
						console.log(ex);
					}
					process_handler(false);
				},
				error: function () {
					process_handler(false);
				}
			});

			if (_vars.origin == "purchaseorder") {
				$("#ctl00_cphMasterBody_rp_Main_pnl_addnewline_TextQty").val("");
			}
		}
	}
	else if ($(obj).attr("data-original_value") == _vars.id && _vars.id == 777 && $('#part_777_notice').size() == 0 && $(".Description").val().trim().length == 0) {
		var _o = $(".Description").offset();
		_o.height = $(".Description").height();
		$(".Description").css("background-color", "yellow");
		$(".Description").val("");

	}
	else if ($(obj).attr("data-original_value") == _vars.id && (_vars.id == 55555 || _vars.id == 55556 || _vars.id == 55557 || _vars.id == 55558 || _vars.id == 55559) && $('#part_95959596_notice').size() == 0 && $(".Description").val().trim().length == 0) {
		var _o = $(".Description").offset();
		_o.height = $(".Description").height();
		$(".Description").css("background-color", "yellow");
		$(".Description").val("");
	}
}

function grid_sellchange(s, e) {
	var price = s.GetText();
	var origin = $('.hid_origin').val();
	var qty = agv.GetEditor('qty').GetValue();
	var discount, multiplier = 1;
	var billtype = origin == 'workorder' ? agv.GetEditor('wo_detail_current_billtypeid').GetValue() : 0;
	if (origin != 'workorder') {
		if (origin == 'quote') {

		}
		qty = agv.GetEditor('qty').GetValue();
	}
	else {
		discount = agv.GetEditor('wo_detail_current_discount') != null ? agv.GetEditor('wo_detail_current_discount').GetValue() : '0';
		if (discount != '0') {
			multiplier = 1 - (discount / 100);
		}
		if (billtype != '0' && billtype != '1' && billtype != '2' && billtype != '10') {
			multiplier = 1;
		}
		qty = agv.GetEditor('qtyrec').GetValue();
	}
	var newprice = qty * price * multiplier;
	newprice = roundNumber(newprice, 3);
	agv.GetEditor('extTandM').SetValue(newprice);
}
function grid_discount_handler(s, e) {
	var origin = $('.hid_origin').val();
	var discount_chked = false;
	if (origin == "quote") {
		discount_chked = quotediscount.GetChecked();
	}
	else if (origin == "workorder") {
		discount_chked = $(".chkdiscount").find("input:checkbox").is(":checked");
	}
	if (discount_chked) {
		if (s.GetValue() > 100) {
			s.SetValue(100);
		}
		var qty = 0;
		switch (origin) {
			case "workorder":
				qty = agv.GetEditor('qtyrec').GetValue();
				break;
			case "quote":
				qty = agv.GetEditor('qty').GetValue();
				break;
			default:
				qty = s.GetText();
				break;
		}
		if (origin == 'workorder' || origin == 'quote') {
			var multiplier = 1;
			var discount = agv.GetEditor('wo_detail_current_discount') != null ? agv.GetEditor('wo_detail_current_discount').GetValue() : '0';
			var billtype = origin == 'workorder' ? agv.GetEditor('wo_detail_current_billtypeid').GetValue() : 0;
			if (discount != '0') {
				multiplier = 1 - (discount / 100);
			}
			if (billtype != '0' && billtype != '1' && billtype != '2' && billtype != '10') {
				multiplier = 1;
			}
			var partno = agv.GetEditor('part_no').GetValue();
			var id = agv.cplineid;
			var costprice = agv.GetEditor('cost').GetValue();
			var sellprice = agv.GetEditor('sell').GetValue();
			var ext_price = roundNumber(qty * sellprice * multiplier, 3);
			if (origin == 'workorder') {
				agv.GetEditor('extTandM').SetValue(ext_price);
			}
			else {
				agv.GetEditor('extTandM').SetValue(ext_price);
				agv.GetEditor('extended_per').SetValue(ext_price);
			}
		}
	}
	else {
		s.SetValue(0);
	}
}
function grid_recd_handler(s, e) {
	var origin = $('.hid_origin').val();
	var realqty = agv.cpOrigValue;
	var totrecqty = s.GetText();
	var newqty = Number(realqty) + Number(totrecqty);
	var partno = agv.GetEditor('part_no').GetValue();
	var costcheck = agv.GetEditor('cost').GetValue();
	var id = agv.cpOrigValue;
	if (origin == 'purchaseorder') {
		agv.GetEditor('qtyrec').SetValue(newqty);
		var cost = agv.GetEditor('cost').GetValue();
		var newcost = newqty * cost;
		newcost = roundNumber(newcost, 3);
		agv.GetEditor('extended_per').SetValue(newcost);
	}
	else if (origin == 'workorder') {
		agv.GetEditor('qtyrec').SetValue(newqty);
		var nowprice = agv.GetEditor('sell').GetValue();




		if ((partno == "" || partno == undefined || partno == null) && partno !== 0) {
			partno = "labor";
		}
		if (partno == 0) {
			partno = "";
		}
		var master_id = partno;
		var data = {
			master_id: master_id,
			qty: newqty,
			business_unit_id: $(".hid_business_unit_id").val(),
			original_sell: (nowprice == null ? 0 : nowprice),
			id: $(".hid_id").val(),
			origin: $(".hid_origin").val(),
			rev: $("#ctl00_cphMasterBody_hidRev").val(),
			lineid: id,
			cost: costcheck
		};
		var sellprice = 0;
		var costprice = 0;

		$.ajax(
			{
				type: 'POST',
				url: "/_tools/picklist_price/index.aspx",
				data: data,
				dataType: 'xml',
				async: true,
				beforeSend: function () {
				},
				success: function (xml) {
					costprice = $(xml).find('cost_price').text();
					sellprice = $(xml).find('sell_price').text();
					//console.log(data);
					//console.log(costprice);
					//console.log(sellprice);
				},
				complete: function () {
					var ext_price = roundNumber(newqty * sellprice * multiplier, 3);
					agv.GetEditor('sell').SetValue(sellprice);
					agv.GetEditor('extTandM').SetValue(ext_price);
				},
				error: function (XMLHttpRequest, textStatus, errorThrown) {
					alert(errorThrown);
				}
			});


	}
}
function grid_recdtodate_handler(s, e) {
	var qty = s.GetText();
	var origin = $('.hid_origin').val();
	if (origin == 'workorder') {
		var multiplier = 1;
		var discount = agv.GetEditor('wo_detail_current_discount') != null ? agv.GetEditor('wo_detail_current_discount').GetValue() : '0'
		var billtype = agv.GetEditor('wo_detail_current_billtypeid').GetValue();
		if (discount != '0') {
			multiplier = 1 - (discount / 100);
		}
		if (billtype != '0' && billtype != '1' && billtype != '2' && billtype != '10') {
			multiplier = 1;
		}
		var partno = agv.GetEditor('part_no').GetValue();
		if (partno == '') {
			partno = 0;
		}
		var id = agv.cplineid;
		var costcheck = agv.GetEditor('cost').GetValue();
		var nowprice = agv.GetEditor('sell').GetValue();




		if ((partno == "" || partno == undefined || partno == null) && partno !== 0) {
			partno = "labor";
		}
		if (partno == 0) {
			partno = "";
		}
		var master_id = partno;
		var data = {
			master_id: master_id,
			qty: qty,
			business_unit_id: $(".hid_business_unit_id").val(),
			original_sell: (nowprice == null ? 0 : nowprice),
			id: $(".hid_id").val(),
			origin: $(".hid_origin").val(),
			rev: $("#ctl00_cphMasterBody_hidRev").val(),
			lineid: id,
			cost: costcheck
		};
		var sellprice = 0;
		var costprice = 0;

		$.ajax(
			{
				type: 'POST',
				url: "/_tools/picklist_price/index.aspx",
				data: data,
				dataType: 'xml',
				async: true,
				beforeSend: function () {
				},
				success: function (xml) {
					costprice = $(xml).find('cost_price').text();
					sellprice = $(xml).find('sell_price').text();
				},
				complete: function () {
					var ext_price = roundNumber(newqty * sellprice * multiplier, 3);
					agv.GetEditor('sell').SetValue(sellprice);
					agv.GetEditor('extTandM').SetValue(ext_price);
				},
				error: function (XMLHttpRequest, textStatus, errorThrown) {
					alert(errorThrown);
				}
			});
	}
}
function grid_qty_handler(s, e) {
	var origin = $('.hid_origin').val();
	var qty = 0;

	if (origin != 'workorder' && origin != 'purchaseorder') {
		switch (origin) {
			case "workorder":
				qty = agv.GetEditor('qtyrec').GetValue();
				break;
			default:
				qty = s.GetText();
				break;
		}
		var partno = agv.GetEditor('part_no').GetValue();
		if (partno == '' || partno == null) {
			partno = 0;
		}
		var costcheck = agv.GetEditor('cost').GetValue();
		var nowprice = agv.GetEditor('sell').GetValue();
		if ((partno == "" || partno == undefined || partno == null) && partno !== 0) {
			partno = "labor";
		}
		var line_id = $(s.mainElement).attr("data-line_id") == undefined ? $(".hid_id").val() : $(s.mainElement).attr("data-line_id");
		var master_id = partno;
		var data = {
			master_id: master_id,
			qty: qty,
			business_unit_id: $(".hid_business_unit_id").val(),
			original_sell: (nowprice == null ? 0 : nowprice),
			id: line_id,
			origin: $(".hid_origin").val(),
			rev: $("#ctl00_cphMasterBody_hidRev").val(),
			lineid: 0,
			cost: costcheck
		};
		var sellprice = 0;
		var costprice = 0;

		$.ajax(
			{
				type: 'POST',
				url: "/_tools/picklist_price/index.aspx",
				data: data,
				dataType: 'xml',
				async: true,
				beforeSend: function () {
				},
				success: function (xml) {
					costprice = $(xml).find('cost_price').text();
					sellprice = $(xml).find('sell_price').text();
				},
				complete: function () {
					var multiplier = 1;
					var discount = agv.GetEditor('wo_detail_current_discount').GetValue();
					if (discount != '0') {
						multiplier = 1 - (discount / 100);
					}
					if (partno == '') {
						multiplier = 1;
					}
					if (partno >= 990000 && partno <= 2000000) {
						multiplier = 1;
					}
					var ext_price = roundNumber(qty * sellprice * multiplier, 3);
					agv.GetEditor('cost').SetValue(costprice);
					agv.GetEditor('sell').SetValue(sellprice);
					agv.GetEditor('extTandM').SetValue(ext_price);
					if (origin == 'quote') {
						agv.GetEditor('extended_per').SetValue(ext_price);
					}
				},
				error: function (XMLHttpRequest, textStatus, errorThrown) {
					alert(errorThrown);
				}
			});
	}
	else if (origin == 'purchaseorder') // It is a PO - Change Ord'd (Extd Ordered)
	{
		var cost = agv.GetEditor('cost').GetValue();
		var qty = s.GetText();
		agv.GetEditor('extTandM').SetValue(cost * qty)
	}
}
function qty_handler(s, e) {
	var t = picklist.get_accessors();
	var partno = 0;
	var qty = 0;
	var origin = t.origin.val();
	if (origin == 'quote' || origin == 'purchaseorder') {
		qty = s.GetText();
	}
	else if (origin == 'workorder') {
		if (t.addline_comqty != undefined) {
			qty = t.addline_comqty == "" ? 1 : t.addline_comqty;
		}
		else {
			qty = s.GetText();
		}
	}
	var com_qty = t.com_qty.val();
	if (origin == 'rfq') {
		return;
	}
	if ($('.TextPartNo').val() != undefined) {
		partno = $('.TextPartNo').val();
		if (partno == 0) {
			partno = "";
		}
	}
	else if (window['add_kit'] != undefined && window['ddlLabourChargeType'] == undefined) {
		partno = add_kit.GetValue();
		if (partno == 0) {
			partno = "";
		}
	}
	else if (window['ddlLabourChargeType'] != undefined) {
		partno = "labor";
	}
	else {
		partno = "";
	}
	var price = 0;
	var exclude = $('ctl00_cphMasterBody_hidPartIsExclude').val();
	var multiplier = 1;
	var discount = t.discount == 1 || t.discount == 0 ? 1 : (1 - t.discount / 100);
	if (origin != 'purchaseorder') {
		var costcheck = t.cost != null ? t.cost.SetValue('') : 0;
		if (origin == 'workorder') {
			var billtype = 0; //ddlBillType.GetValue();
			var ee = document.getElementById('ctl00_cphMasterBody_rp_Main_pnl_addnewline_ddlMatType');
			var exclude = document.getElementById('ctl00_cphMasterBody_hidPartIsExclude').value
			var materialtype = ee.options[ee.selectedIndex].value;
			if (materialtype == '1' && exclude == 'False') {
				var available = $('.TextAvailQty').text();

			}
		}
		if (origin == 'quote') {
			if (discount == 1) {
				multiplier = 1;
			}
			else {
				multiplier = 1 - discount;
			}
			if (partno == '') {
				multiplier = 1;
			}
			var ee = document.getElementById('ctl00_cphMasterBody_rp_Main_pnl_addnewline_ddlMatType');
			var materialtype = ee.options[ee.selectedIndex].value;
			if (materialtype == '2') {
				multiplier = 1;
			}
		}

		var c = $(".hid_business_unit_id").val();
		var origin = $(".hid_origin").val();
		var id = $(".hid_id").val();
		var rev = $("#ctl00_cphMasterBody_hidRev").val();
		if ((partno == "" || partno == undefined || partno == null) && partno !== 0) {
			partno = "labor";
		}
		if (partno == 0) {
			partno = "";
		}
		var price = addline_TextSell.GetValue();
		var master_id = partno;
		var data = {
			master_id: master_id,
			qty: qty,
			business_unit_id: c,
			original_sell: (price == null ? 0 : price),
			id: id,
			origin: origin,
			rev: rev,
			lineid: 0,
			cost: cost
		};
		var sellprice = 0;
		var costprice = 0;

		$.ajax(
			{
				type: 'POST',
				url: "/_tools/picklist_price/index.aspx",
				data: data,
				dataType: 'xml',
				async: true,
				beforeSend: function () {
					process_handler(true);
				},
				success: function (xml) {
					costprice = $(xml).find('cost_price').text();
					if (costprice == 0) {
						costprice = "0";
					}
					if (sellprice == 0) {
						sellprice = "0";
					}
					sellprice = $(xml).find('sell_price').text();
					price_obj.is_exclude = $(xml).find('is_exclude').text();
					price_obj.allowed_to_stock = $(xml).find('allowed_to_stock').text();
					$(".hid_sellprice").val(sellprice);
				},
				complete: function () {
					try {
						if (t.cost != null) {
							t.cost.SetValue(costprice);
						}
						//MH (devex) - $('.TextSell').val(newprice);
						addline_TextSell.SetValue(sellprice);
						var discount = t.discount == 1 || t.discount == 0 ? 1 : (1 - t.discount / 100);
						//console.log(t);
						var extended = roundNumber(sellprice * qty * discount, 3);
						addline_TMSellExt.SetValue(extended);
						$('.TMQuoteExt').val(extended);
						var origin = $('.hid_origin').val();
						var costcheck = t.cost != null ? t.cost.GetValue() : 0;
						switch (origin) {
							case 'quote':
								if ($('.TextPartNo').val() == '' || price_obj.is_exclude == 'True') {
									addline_TextSell.SetValue("");
									if (t.cost != null) {
										t.cost.SetEnabled(true);
										t.cost.SetValue('');
									}
									addline_TMSellExt.SetValue("");
								}
								else {
									if (t.cost != null) {
										t.cost.SetEnabled(false);
									}
								}
								break;
							case 'workorder':
								if (price_obj.is_exclude == 'True' && master_id != 777) {
									var setcost = '';
									if (costcheck != '' || costcheck != '0') {
										setcost = costcheck;
									}
									var info_text = master_id == 2139
										? "Please enter a cost & sell for this quote line"
										: "Please enter a cost for this part";
									picklist.update_info(info_text);
									if (t.cost != null) {
										t.cost.SetEnabled(true);
										t.cost.SetValue(setcost);
									}
									t.sell.SetEnabled(master_id == 2139);
									if (master_id == 2139 && t.sell.GetValue() > 0) {
										t.sell_ext = t.sell.GetValue() / data.qty;
									}
								}
								break;
						}
					}
					catch (ex) {
						console.log(ex);
					}
					process_handler(false);
				},
				error: function (XMLHttpRequest, textStatus, errorThrown) {
					alert(errorThrown);
					process_handler(false);
				}
			});
	}
	else {
		var cost = t.cost != null ? t.cost.GetValue() : 0;
		var newcost = qty * cost;
		newcost = roundNumber(newcost, 4);
		addline_TMSellExt.SetValue(newcost);
		process_handler(false);
	}

}

function cost_keydown(s, e) {

	only_numeric(e);
	setTimeout("$('#part_cost_notice').remove();", 500);
}


function cost_handler(s, e) {
	var t = picklist.get_accessors();

	if (t.qty.val() == "") {
		t.qty.val("1");
	}
	var defs = {
		a: "cost_handler",
		cost: s.GetValue() / 1,
		qty: t.qty.val(),
		origin: t.origin.val()
	};
	if (defs.cost == 0) {
		return false;
	}
	if (isNaN(defs.cost)) {
		s.SetValue("");
		return false;
	}
	var master_id = $('.TextPartNo').val();
	$.ajax({

		type: "GET",
		url: "./pikclist.aspx",
		data: defs,
		beforeSend:
			function () {
				process_handler(true);
			},
		success:
			function (j) {
				var o = $.parseJSON(j);
				//console.log(o);
				if (o.status == 'success') {
					if (t.origin.val() == "purchaseorder") {
						addline_TMSellExt.SetValue(o.ext_cost);
					}
					if (master_id != 2139 && t.origin.val() != "purchaseorder") {
						try {
							addline_TextSell.SetValue(o.sell);
						}
						catch (err) { console.log(err); }
						addline_TMSellExt.SetValue(o.ext_sell);
						t.quote_extd.val(o.ext_sell);
					}
				}
			},
		complete:
			function () {
				process_handler(false);
			},

		error:
			function (XMLHttpRequest, textStatus, errorThrown) {
				alert(errorThrown);
				process_handler(false);
			}
	});
}
function repair_info(val) {
	var t = picklist.get_accessors();
	var defs = {
		a: "repair_info",
		id: val
	};
	$.get("./pikclist.aspx", defs,
		function (j) {
			var o = $.parseJSON(j);
			if (o.status == 'success') {
				//MH (devex) - t.bench_sell.val(o.sell);
				addline_TextSell.SetValue(o.sell);
				//MH (devex) - t.bench_extd.val(o.ext_sell);
				addline_TMSellExt.SetValue(o.ext_sell);
				t.quote_extd.val(o.ext_sell);
			}
		});
}
function refactor_handler(obj) {
	var t = picklist.get_accessors();
	var defs = {
		a: "refactor_handler",
		id: t.id.val(),
		rev: t.rev.val(),
		origin: t.origin.val()
	};
	$.get("./pikclist.aspx", defs,
		function (o) {
			if (o == "SUCCESS") {
				location.href = location.href;
			}
			else {
				alert(o);
			}
		});
}
var keyValue;
function bind_tooltips() {
	$("body").find(".kitted_image").each(function () { $(this).parents("td:first").tip({ title: "Kit Contents", width: 450 }) });
	$("body").find('.list').each(function () {
		$(this).val('');
		$(this).change(function () { selection_change(this) });
	});
	$("body").find(".opt").each(function () {
		$(this).tip({ title: $(this).text(), width: 400, pre: true, scrollable: true, fixed_height: true });
	}
	);
	var h = picklist.get_accessors();
	if (window[h.footer_save.val()] != null) {
		if (h.footer_save != undefined && h.footer_save != null && h.footer_save.val() != null && eval(h.footer_save.val()) != undefined && h.footer_save.val() != "") {
			eval(h.footer_save.val()).SetClientVisible(true);
		}
	}
	$("body").find(".recqty").each(function () {
		$(this).keydown(function (e) {
			if (e.which == 9 && !e.shiftKey) {
				return false;
			}
			else if (e.which == 9 && e.shiftKey) {
				$(this).parents("tr:first").prev().find(".recqty").focus().select();
				return false;
			}
		});
		$(this).keyup(function (e) {
			if (e.which == 9 && !e.shiftKey) {
				$(this).parents("tr:first").next().find(".recqty").focus().select();
			}
		});
	});
	//$("#tooltip_window").hide(0);
}
function grid_part_handler(partid, qty) {
	_vars = {
		a: "g",
		id: partid,
		business_unit_id: $(".hid_business_unit_id").val(),
		origin: $(".hid_origin").val()
	};
	var in_error = false;
	if (partid != "" && partid != "60000" && partid == "55560") {
		x = $.ajax({
			type: "GET",
			url: "/_tools/part_info/index.aspx",
			data: _vars,
			beforeSend: function () {

			},
			success:
				function (xml) {
					if ($(xml).find('error').size() > 0) {
						var error = $(xml).find('error').text();
						alert(error);
						in_error = true;
						$(obj).val("").focus();
					}
					else {
						var description = $(xml).find('description').text();
						var cost_price = $(xml).find('cost_price').text() / 1;
						var sell_price = $(xml).find('sell_price').text() / 1;
						var exclude = $(xml).find('exclude_part').text();
						var qty = agv.GetEditor('qty').GetValue();
						var extd = (sell_price * qty).toFixed(3);
						agv.GetEditor('description').SetValue(description);
						agv.GetEditor('sell').SetValue(sell_price);
						agv.GetEditor('extTandM').SetValue(extd);
						agv.GetEditor('extended_per').SetValue(extd);
						agv.GetEditor('cost').SetValue(cost_price);
					}
				},
			complete: function () { },
			error: function () { }
		});

		if (_vars.origin == "purchaseorder") {
			var url = "/_tools/part_info_vendor/index.aspx?partid=" + _vars.id + "&compid=" + _vars.business_unit_id + "&id=" + $(".hid_id").val();
			var vendorpartno = "";
			var vendorcost = "";
			var vendorqtyper = "";
			$.ajax(
				{
					type: 'GET',
					url: url,
					dataType: 'xml',
					async: false,
					beforeSend: function () {

					},
					success: function (xml) {
						// This is for if it successfully receives a response it can work with
						$(xml).find('purchaseorder').each(function () {
							vendorpartno = $(this).attr('vednpartno');
							vendorcost = $(this).attr('vendorcost');
							vendorqtyper = $(this).attr('vendorqty');
							if (vendorqtyper == '') {
								vendorqtyper = '1';
							}
							agv.GetEditor('vend_part_no').SetValue(vendorpartno);
							agv.GetEditor('cost').SetValue(vendorcost);
							agv.GetEditor('qty_per_part').SetValue(vendorqtyper);
							var newprice = qty * vendorcost;
							agv.GetEditor('extTandM').SetValue(newprice);
						})
					},
					complete: function () {

					},
					error: function (XMLHttpRequest, textStatus, errorThrown) {

					}

				});
		}

	}
}
function CheckVendorPartNo(partno, vendpartno) {
	var url = "/_tools/part_info_vendor/index.aspx?partid=" + partno + "&compid=" + $(".hid_business_unit_id").val() + "&id=" + $(".hid_id").val() + "&vendpartno=" + vendpartno;
	var checkvendorpartno = "";
	$.ajax(
		{
			type: 'GET',
			url: url,
			dataType: 'xml',
			async: false,
			beforeSend: function () {

			},
			success: function (xml) {
				// This is for if it successfully receives a response it can work with
				$(xml).find('purchaseorder').each(function () {
					checkvendorpartno = $(this).attr('checkvendorpart');
					if (checkvendorpartno != "") {
						alert('This Vendor Part Number is Already associated with another part');
						agv.GetEditor('vend_part_no').SetValue('');
					}
				})
			},
			complete: function () {

			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {

			}

		});
}
function OnViewPicture(element, key) {
	callbackPanel.SetContentHtml("");
	popup.ShowAtElement(element);
	keyValue = key;
}
function popup_Shown(s, e) {
	callbackPanel.PerformCallback(keyValue);
}
function showNotes(lineid, e) {
	$("#ctl00_cphMasterBody_hidNotesID").val(lineid);
	if (lineid != "0") {
		var url = "/_tools/get_notes/index.aspx?id=" + lineid + "&source=" + $(".hid_origin").val();
		var notes = "";
		var rec = "";
		var part = "";
		$.ajax(
			{
				type: 'GET',
				url: url,
				dataType: 'xml',

				beforeSend: function () {

				},
				success: function (xml) {
					notes = $(xml).find('body').text();
					part = $(xml).find('part').text();
					rec = $(xml).find('rec').text();
				},
				complete: function () {
					$(".note_history").val(notes);
					$(".textNotes").val("");
					$("#ctl00_cphMasterBody_ASPxpuNotes_imgbnotesupdate").css({ "display": "" });
					$("#ctl00_cphMasterBody_ASPxpuNotes_imgbtnPOLineActive").css({ "display": "none" });
					if (rec != "") {
						NotesPopUp.SetHeaderText("Notes for Part #:" + part + " - Rec #:" + rec);
					}
					else {
						NotesPopUp.SetHeaderText("Notes for Part #:" + part);
					}

					NotesPopUp.ShowAtPos(e.x - 200, e.y);
				},
				error: function (XMLHttpRequest, textStatus, errorThrown) {
				}

			}
		);

	}

	//NotesPopUp.Show();  

}
function showNotesForInactive(lineid, e) {
	document.getElementById("ctl00_cphMasterBody_hidNotesID").value = lineid;
	var notes = "";
	var rec = "";
	var part = "";
	if (lineid != "0") {
		var url = "/_tools/get_notes/index.aspx?id=" + lineid + "&source=" + $(".hid_origin").val();
		var notes = "";
		$.ajax(
			{
				type: 'GET',
				url: url,
				dataType: 'xml',
				async: false,

				beforeSend: function () {

				},
				success: function (xml) {
					notes = $(xml).find('body').text();
					part = $(xml).find('part').text();
					rec = $(xml).find('rec').text();
					// This is for if it successfully receives a response it can work with
					$(xml).find('notesreturn').each(function () {
						notes = $(this).text();
						$("#ctl00_cphMasterBody_ASPxpuNotes_textNotes").val(notes);
					})
				},
				complete: function () {
					$(".note_history").val(notes);
					$(".textNotes").val("");
					$("#ctl00_cphMasterBody_ASPxpuNotes_imgbnotesupdate").css({ "display": "none" });
					$("#ctl00_cphMasterBody_ASPxpuNotes_imgbtnPOLineActive").css({ "display": "" });
					NotesPopUp.SetHeaderText("Please indicate why you are marking this item Complete or Incomplete");
					NotesPopUp.ShowAtPos(e.x - 200, e.y);
				},
				error: function (XMLHttpRequest, textStatus, errorThrown) {
					return "0";
				}

			}
		);

	}
}
function showHistory(lineid) {
	document.getElementById("ctl00_cphMasterBody_hidHistoryID").value = lineid;
	if (lineid != "0") {
		var url = "/_tools/get_history/index.aspx?id=" + lineid + "&source=" + $(".hid_origin").val();
		var notes = "";
		$.ajax(
			{
				type: 'GET', url: url, dataType: 'xml', async: false, beforeSend: function () {
				}, success: function (xml) {
					/* This is for if it successfully receives a response it can work with*/
					$(xml).find('historyreturn').each(function () {
						notes = $(this).text();
						$("#ctl00_cphMasterBody_ASPxpuHistory_textHistory").val(notes);
					});
				}, complete: function () {
					return notes;
				}, error: function (XMLHttpRequest, textStatus, errorThrown) {
					return "0";
				}
			}
		);
	}
	HistoryPopUp.SetHeaderText("History");
	HistoryPopUp.Show();
}
function showOrigin(lineid) {
	document.getElementById("ctl00_cphMasterBody_hidHistoryID").value = lineid;
	if (lineid != "0") {
		var url = "/_tools/get_origin/index.aspx?id=" + lineid + "&source=" + $(".hid_origin").val();
		var notes = "";
		$.ajax(
			{
				type: 'GET', url: url, dataType: 'xml', async: false, beforeSend: function () {
				}, success: function (xml) {
					/* This is for if it successfully receives a response it can work with*/
					$(xml).find('originreturn').each(function () {
						notes = $(this).text();
						$("#ctl00_cphMasterBody_ASPxpuHistory_textHistory").val(notes);
					});
				}, complete: function () {
					return notes;
				}, error: function (XMLHttpRequest, textStatus, errorThrown) {
					return "0";
				}
			}
		);
	}
	HistoryPopUp.SetHeaderText("Origin");
	HistoryPopUp.Show();
}
function showTaxes(lineid, e) {
	document.getElementById("ctl00_cphMasterBody_TaxPopUpInfo_hidTaxLineID").value = lineid;
	if (lineid != "0") {
		var url = "/_tools/get_taxes/index.aspx?id=" + lineid + "&source=" + $(".hid_origin").val();
		var notes = "";
		var tax1 = "";
		var tax2 = "";
		var tax3 = "";
		var tax4 = "";
		var taxname1 = "";
		var taxname2 = "";
		var taxname3 = "";
		var taxname4 = "";

		$.ajax(
			{
				type: 'GET', url: url, dataType: 'xml', async: false, beforeSend: function () {
				}, success: function (xml) {
					/* This is for if it successfully receives a response it can work with*/
					$(xml).find('taxinfo').each(function () {
						tax1 = $(this).attr('tax1');
						tax2 = $(this).attr('tax2');
						tax3 = $(this).attr('tax3');
						tax4 = $(this).attr('tax4');
						taxname1 = $(this).attr('taxname1');
						taxname2 = $(this).attr('taxname2');
						taxname3 = $(this).attr('taxname3');
						taxname4 = $(this).attr('taxname4');


						if (tax1 != "0") {
							document.getElementById("ctl00_cphMasterBody_TaxPopUpInfo_rbTax1").checked = true;
						}
						else {
							document.getElementById("ctl00_cphMasterBody_TaxPopUpInfo_rbTax1").checked = false;
						}
						if (tax2 != "0") {
							document.getElementById("ctl00_cphMasterBody_TaxPopUpInfo_rbTax2").checked = true;
						}
						else {
							document.getElementById("ctl00_cphMasterBody_TaxPopUpInfo_rbTax2").checked = false;
						}
						if (tax3 != "0") {
							document.getElementById("ctl00_cphMasterBody_TaxPopUpInfo_rbTax3").checked = true;
						}
						else {
							document.getElementById("ctl00_cphMasterBody_TaxPopUpInfo_rbTax3").checked = false;
						}
						if (tax4 != "0") {
							document.getElementById("ctl00_cphMasterBody_TaxPopUpInfo_rbTax4").checked = true;
						}
						else {
							document.getElementById("ctl00_cphMasterBody_TaxPopUpInfo_rbTax4").checked = false;
						}
					})
				}, complete: function () {
					return notes;
				}, error: function (XMLHttpRequest, textStatus, errorThrown) {
					return "0";
				}
			}
		);
	}
	var pos_x = e.x;
	var pos_y = e.y;
	TaxPopUpInfo.ShowAtPos(pos_x, pos_y);
	//TaxPopUpInfo.Show();
}
function TransferItems(lineid, process) {
	var _type = "C";
	if (lineid == "0") {
		var g = agv;
		var page_index = g.GetPageIndex();
		var page_size = g.pageRowSize;
		var t = picklist.get_accessors();
		var l_n = page_index * page_size;
		var lines = g.pageRowCount + l_n;
		var line_ids = new Array();
		for (var l = l_n; l < lines; l++) {
			var qtyrec = $(g.GetRow(l)).find(".qtyrec").text() / 1;
			var is_checked = $(g.GetRow(l)).find(".sel_box").find('input:checkbox').is(":checked");
			if (is_checked && qtyrec > 0) {
				line_ids.push(g.cp_line_ids[l]);
			}
		}
		if (line_ids.length == 0) {
			alert("Please select lines that have a quantity committed to them");
			return;
		}
		lineid = line_ids.join(",");
		_type = "M";
	}
	var url = "./pikclist.aspx";
	var vars = {
		a: "transfer_data",
		id: lineid,
		type: _type
	}
	var _results = new Array();
	var master_id = "";
	$.ajax({
		type: 'GET',
		url: url,
		data: vars,
		dataType: 'xml',
		async: true,
		beforeSend:
			function () {
				please_wait("start", "");
			},
		success:
			function (xml) {
				if (!process) {
					$(xml).find('line').each(function () {
						var contents = {
							id: $(this).find("id").text(),
							part_id: $(this).find("part_id").text(),
							qty: $(this).find("qty").text(),
							origin: $(this).find("origin").text(),
							type: _type,
							use_wo: false,
							dest_id: 0
						};
						_results.push(contents);
					});
				}
				else {
					var sub_g = gv_xfer_multiple;
					var sub_page_index = sub_g.GetPageIndex();
					var sub_page_size = sub_g.pageRowSize;
					var sub_l_n = sub_page_index * sub_page_size;
					var sub_lines = sub_g.pageRowCount + sub_l_n;
					var selected_wo = xfer_wo_combo.GetValue();
					for (var l = sub_l_n; l < sub_lines; l++) {
						var this_use_wo = $(sub_g.GetRow(l)).find(".xfer_chk_wo").is(":checked");
						var location_box_ref = $(sub_g.GetRow(l)).find(".loc_combo").attr("id") + "_L";
						var location_box = eval(location_box_ref);
						var this_dest_id = this_use_wo ? selected_wo : location_box.GetValue();
						var contents = {
							id: $(sub_g.GetRow(l)).find(".xfer_hid_id").val(),
							part_id: $(sub_g.GetRow(l)).find(".xfer_lb_part_no").text(),
							qty: $(sub_g.GetRow(l)).find(".xfer_lb_qty").val(),
							origin: $(sub_g.GetRow(l)).find(".xfer_lb_origin").text(),
							use_wo: this_use_wo,
							dest_id: this_dest_id
						};
						_results.push(contents);
					}
				}
			},
		complete:
			function () {
				if (!process) {
					TransferPopUp.Show();
					xfer_cbp.PerformCallback(JSON.stringify(_results));
					please_wait("stop");
					$(".xfer_wo_qty").focus();
				}
				else {
					var is_wo = xfer_wo_combo.GetSelectedIndex() > -1;
					var is_ext = xfer_external_combo.GetSelectedIndex() > -1;
					var is_int = xfer_internal_combo.GetSelectedIndex() > -1;
					var destination = is_wo ? "wo" : is_ext ? "ext" : is_int ? "int" : "";
					var destination_id = is_wo ? xfer_wo_combo.GetValue() : is_ext ? xfer_external_combo.GetValue() : is_int ? xfer_internal_combo.GetValue() : 0;
					var xfer_reason = $(".txtTransferReason").text();
					xfer_cbp.PerformCallback(xfer_reason + "|" + JSON.stringify(_results));
					please_wait("stop");
				}
			},
		error:
			function (XMLHttpRequest, textStatus, errorThrown) {
				alert("There was an error retrieving the transfer values for this row - Error Given:" + errorThrown);
			}
	});
}
function GetNewPrice(_cost, _qty) {
	var url = "/sections/member/picklist/pikclist.aspx";
	var _data = {
		a: "cost_handler",
		cost: _cost,
		qty: _qty
	};
	var sellprice = 0;
	$.ajax(
		{
			type: 'GET',
			url: url,
			data: _data,
			dataType: 'json',
			async: false,
			beforeSend:
				function () {
					please_wait("start", "Getting Sell Price");
				},
			success:
				function (resp) {
					if (resp.status == "success") {
						sellprice = resp.sell;
						$(".hid_sellprice").val(sellprice);
					}
					else {
						alert("There was an error retrieving the sell price");
					}
				},
			complete:
				function () {
					please_wait("stop");
					return sellprice;
				},
			error:
				function () {
					please_wait("stop");
					return 0;
				}

		});

}
var price_obj = {
	master_id: null,
	is_exclude: null,
	allowed_to_stock: null
};



function roundNumber(number, digits) {
	var multiple = Math.pow(10, digits);
	var rndedNum = Math.round(number * multiple) / multiple;
	return rndedNum;
}
function CheckPOCost(cost, part, qtyper, qty) {
	var reccost;
	var c = $('.hid_business_unit_id').val();
	var url = "/_tools/get_cost/index.aspx?q=" + part + "&c=" + c;
	if ($(".hid_origin").val() == "purchaseorder") {
		$.ajax(
			{
				type: 'GET',
				url: url,
				dataType: 'xml',
				async: false,
				beforeSend: function () {
				},
				success: function (xml) {
					// This is for if it successfully receives a response it can work with
					reccost = $(xml).find('cost').text();
				},
				complete: function () {

				},
				error: function (XMLHttpRequest, textStatus, errorThrown) {
					reccost = "0";
				}

			}
		);
		var upper = reccost * 1.25;
		var lower = reccost * 0.50;
		var variance = (Math.abs((cost - reccost) / reccost) * 100).toFixed(1);
		var answer;
		if ((cost > upper) && reccost != 0) {
			answer = confirm("Warning the cost you have entered is higher than your current business unit cost by " + variance + "%.  Do you want to continue? ");
		}
		else if ((cost < lower) && reccost != 0) {
			answer = confirm("Warning the cost you have entered is lower than your current business unit cost by " + variance + "%.  It might indicate a problem with the Qty /Part value that you have entered.  Do you want to continue? ");
		}
		else if (qty == qtyper && qty > 1 && qtyper > 1) {
			var v = qtyper * qty;
			answer = confirm("It appears that your quantity per and quantity are the same amount, please confirm the intended final quantity is " + v + " .");
		}
		else {
			answer = true;
		}
		return answer;
	}
	else {
		return true;
	}
}
function GetSellPrice(cost, qty) {
	var url = "/_tools/sell_price/index.aspx?cost=" + cost + "&qty=" + qty;
	var qtysellprice = "0";
	$.ajax(
		{
			type: 'GET',
			url: url,
			dataType: 'xml',
			async: false,
			beforeSend:
				function () {
				},
			success:
				function (xml) {
					// This is for if it successfully receives a response it can work with
					$(xml).find('PriceReturn').each(function () {
						qtysellprice = $(this).attr('price');
						$(".hid_sellprice").val(qtysellprice);
					});
				},
			complete:
				function () {
					return qtysellprice;
				},
			error:
				function (XMLHttpRequest, textStatus, errorThrown) {
					return "0";
				}
		});
}

function print_barcode(line_id) {
	var o = picklist.get_accessors();
	boing("./modules/pop_po_barcodes.aspx?poprog_id=" + o.id.val() + "&business_unit_id=" + o.business_unit_id.val() + "&line_id=" + line_id, "PObarcodes", 800, 150);
}

function print_barcode_at_server(line_id, c, qty) {
	var url = "/_tools/print_barcode/index.aspx?lineid=" + line_id + "&c=" + c;
	$.ajax(
		{
			type: 'GET',
			url: url,
			dataType: 'xml',
			async: false,
			beforeSend:
				function () {
				},
			success:
				function (xml) {
				},
			complete:
				function () {
				},
			error:
				function (XMLHttpRequest, textStatus, errorThrown) {
				}
		});
	please_wait("stop");
}

function update_header_totals() {
	var c = $('.hid_business_unit_id').val();
	var origin = $(".hid_origin").val();
	var id = $(".hid_id").val();
	var v = {
		RegLab2: "0",
		RegMat2: "0",
		RegTot2: "0",
		VNCLab2: "0",
		VNCMat2: "0",
		VNCTot2: "0",
		BlnLab2: "0",
		BlnMat2: "0",
		BlnTot2: "0",
		ICRLab2: "0",
		ICRMat2: "0",
		ICRTot2: "0",
		DNILab2: "0",
		DNIMat2: "0",
		DNITot2: "0",
		VCLab2: "0",
		VCMat2: "0",
		VCTot2: "0",
		AllLab: "0",
		AllMat: "0",
		AllTot: "0",
		quoteid: "0",
		erid: "0"
	};

	if (origin == "workorder") {
		var url = "/_tools/picklist_header_totals/index.aspx?id=" + id + "&c=" + c + "&origin=" + origin;

		$.ajax(
			{
				type: 'GET',
				url: url,
				dataType: 'xml',
				async: false,
				beforeSend:
					function () {

					},
				success:
					function (xml) {
						v.RegLab2 = $(xml).find('RegLab2').text();
						v.RegMat2 = $(xml).find('RegMat2').text();
						v.RegTot2 = $(xml).find('RegTot2').text();
						v.VNCLab2 = $(xml).find('VNCLab2').text();
						v.VNCMat2 = $(xml).find('VNCMat2').text();
						v.VNCTot2 = $(xml).find('VNCTot2').text();
						v.BlnLab2 = $(xml).find('BlnLab2').text();
						v.BlnMat2 = $(xml).find('BlnMat2').text();
						v.BlnTot2 = $(xml).find('BlnTot2').text();
						v.ICRLab2 = $(xml).find('ICRLab2').text();
						v.ICRMat2 = $(xml).find('ICRMat2').text();
						v.ICRTot2 = $(xml).find('ICRTot2').text();
						v.DNILab2 = $(xml).find('DNILab2').text();
						v.DNIMat2 = $(xml).find('DNIMat2').text();
						v.DNITot2 = $(xml).find('DNITot2').text();
						v.VCLab2 = $(xml).find('VCLab2').text();
						v.VCMat2 = $(xml).find('VCMat2').text();
						v.VCTot2 = $(xml).find('VCTot2').text();
						v.AllLab = $(xml).find('AllLab').text();
						v.AllMat = $(xml).find('AllMat').text();
						v.AllTot = $(xml).find('AllTot').text();
						v.quoteid = $(xml).find('quoteid').text();
						v.erid = $(xml).find('erid').text();

					},
				complete:
					function () {
						$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblRegularLabour2").html(v.RegLab2);
						$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblRegularMaterial2").html(v.RegMat2);
						$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblRegularTotal2").html(v.RegTot2);

						$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblVNCLabour2").html(v.VNCLab2);
						$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblVNCMaterial2").html(v.VNCMat2);
						$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblVNCTotal2").html(v.VNCTot2);

						$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblLabour2").html(v.AllLab);
						$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblMaterial2").html(v.AllMat);
						$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblTotal2").html(v.AllTot);


						if (v.quoteid == "0" && v.erid == "0") {
							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblBlendedLabour2").html(v.BlnLab2);
							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblBlendedMaterial2").html(v.BlnMat2);
							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblBlendedTotal2").html(v.BlnTot2);

							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblICRLabour2").html(v.ICRLab2);
							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblICRMaterial2").html(v.ICRMat2);
							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblICRTotal2").html(v.ICRTot2);

							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblDNILabour2").html(v.DNILab2);
							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblDNIMaterial2").html(v.DNIMat2);
							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblDNITotal2").html(v.DNITot2);

							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblVCLabour2").html(v.VCLab2);
							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblVCMaterial2").html(v.VCMat2);
							$("ctl00_cphMasterBody_rp_Main_pnl_WODetails_lblVCTotal2").html(v.VCTot2);
						}
					},
				error:
					function (XMLHttpRequest, textStatus, errorThrown) {
						alert(XMLHttpRequest.responseText);
					}
			});
	}

}
$("document").ready(function () {
	Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
	Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
	//Sys.WebForms.PageRequestManager.getInstance()._events._list.endRequest[0]();
	bind_tooltips();
});
function update_grid() {
	setTimeout("agv.SortBy('', 'ASC')", 500);
}
function setFilter(f) {
	agv.ApplyFilter("[wo_detail_current_rec_no] = " + f);
}
function EndReqHandler(sender, args) {

	page_obj.update_panel_progress.stop();
	if (args != undefined && args.get_error() != undefined) {
		var err = args.get_error().toString().replace(/Sys.+\Exception:/g, "").trim();
		alert(err);
	}
	var h = picklist.get_accessors();
	if (window[h.footer_save.val()] != null) {
		eval(h.footer_save.val()).SetClientVisible(true);
	}
	switch ($('.hid_origin').val()) {
		case "purchaseorder":
			if (parent != undefined && parent != null) {
				try {
					parent.update_ts();
					parent.update_header();
				}
				catch (err) { }
			}
			break;
		case "workorder":
			if (parent != undefined && parent != null) {
				try {
					parent.update_ts();
					parent.update_header();
				}
				catch (err) { }
			}
			break;
		case "quote":
			if (opener != undefined && opener != null) {
				try {
					opener.update_header();
				}
				catch (err) { }
			}
			break;
	}
	bind_tooltips();

}

function BeginReqHandler() {

	page_obj.update_panel_progress.start();
}
function print_all(s, e) {
	var o = picklist.get_accessors();
	boing("./modules/pop_po_barcodes.aspx?poprog_id=" + o.id.val() + "&business_unit_id=" + o.business_unit_id.val(), "PObarcodes", 800, 768);
}

function check_commit_wo_select_all(obj, n) {

	var l_n = 0;
	var lines = n;

	for (var l = l_n; l < lines; l++) {
		var cb = $("#cb_" + l);
		if (!cb.is(":disabled")) {
			cb.attr("checked", $(obj).is(":checked"));
			check_commit_wo($("#cb_" + l));
		}

	}

}

function check_commit_wo(obj) {
	check_commit_wo_direct($(obj).attr("data-row_n"), agv.GetRow($(obj).attr("data-row_n")), $(obj).is(':checked'));
}
function check_commit_wo_direct(row_n, gv_row, is_checked) {
	$(gv_row).find('.cb_commit').children("input:checkbox").attr("checked", is_checked);
}

var _ddlwo_switchfader;
function ddlwo_switch(s, e) {
	var origin = $('.hid_origin').val();
	if (origin == 'purchaseorder') {
		var id = WOHeadComboBox.GetValue();
		var gl_shown = (id >= 32 && id <= 57) ? "block" : "none";
		$("#lbl_gl_warning").css("display", gl_shown);
		if (gl_shown == "block") {
			clearTimeout(_ddlwo_switchfader);
			_ddlwo_switchfader = setTimeout("$('#lbl_gl_warning').fadeOut()", 5000);
		}
		else {
			clearTimeout(_ddlwo_switchfader);
		}
	}
	else if (origin == 'quote') {
		var val = s.GetValue();

		if (val != 0) {
			var desc = s.GetText().replace(/^\d+ -/, "");
			$(".Description").val(desc);
			repair_info(val);
		}
	}

	showorhidden_expense_categories();


}

function showorhidden_expense_categories() {

	var flag = (WOHeadComboBox.cpFLAG[WOHeadComboBox.GetSelectedIndex()]);

	var expensecategoryshow = (flag == 1) ? "none" : "table-cell";


	var id = WOHeadComboBox.GetValue();
	if (flag == 1) {
		$('.hid_WO').val(id);
	}
	else {
		$('.hid_WO').val("");
	}
}

function checkBetterPrice(businessUnit, checkpart, cost, qtyper, quantity, currentUser, poID) {
	// console.log('Step 1 : Check the low price option from other BUs inside same tax entity.');
	var dataFromServer = null;
	var info = { businessUnit: businessUnit, checkpart: checkpart, cost: cost, qtyper: qtyper, currentUser: currentUser, poID: poID };
	var materialInfo = { materialInfo: info };
	$.ajax(
		{
			type: 'POST',
			url: 'pikclist.aspx/CheckLowPrice',
			contentType: 'application/json; charset=utf-8',
			async: false,
			dataType: 'json',
			data: JSON.stringify(materialInfo),
			beforeSend: function () {
			},
			success: function (data) {
				dataFromServer = data;
			},
			complete: function () {
			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {
			}

		});

	// console.dir(dataFromServer);

	if (dataFromServer === null || dataFromServer.d.valid === 0) {
		// No low price found, return and let it go.
		return 0;
	}

	// console.log('Step 2 : Show the suggested price');
	lowPriceOption = dataFromServer.d;

	var answer = confirm("The business unit " +
		lowPriceOption.businessUnit +
		" purchased this same item, from the same vendor on " +
		lowPriceOption.purchaseDate +
		" for $" +
		lowPriceOption.total +
		" with a " +
		"quantity/per" +
		" of " +
		lowPriceOption.quantityPer +
		", would you instead like to use their pricing?");

	if (answer === false) {
		return 0;
	}

	console.log('Step 3 :  Copy inventory item');
	var inventoryDataFromServer = null;
	var inventory = {
		fromBusinessUnit: lowPriceOption.businessUnitId,
		part: lowPriceOption.part,
		toBusinessUnit: businessUnit,
		currentUser: currentUser,
		vendor: lowPriceOption.vendor,
		code: lowPriceOption.code,
		qtyPer: lowPriceOption.quantityPer,
		cost: lowPriceOption.packageCost,
		total: lowPriceOption.total
	};
	var CopyInfo = { inventoryCopyInfo: inventory };
	$.ajax(
		{
			type: 'POST',
			url: 'pikclist.aspx/CopyInventoryItem',
			contentType: 'application/json; charset=utf-8',
			async: false,
			dataType: 'json',
			data: JSON.stringify(CopyInfo),
			beforeSend: function () {
			},
			success: function (data) {
				inventoryDataFromServer = data;
			},
			complete: function () {
			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {
			}
		});

	console.dir(inventoryDataFromServer);
	if (inventoryDataFromServer === null || inventoryDataFromServer.d.valid === 0) {
		// Show a confirm if sth goes wrong?
		alert("An error occured while checking for recent purchases, please submit a ticket with steps on how to reproduce this error.");
		return 0;
	}

	var newpart = inventoryDataFromServer.d;

	// console.log('Step 4 :  UI updating');

	// code
	if (TextVendorPartNo.FindItemByText(newpart.code) === null) {
		TextVendorPartNo.AddItem(newpart.code, newpart.idOfCode);
	}

	TextVendorPartNo.SetValue(newpart.idOfCode);

	// cost
	$(".TextCost").val(newpart.total);

	// qtyper
	$('.QtyPerPart').val(newpart.qtyPer);

	// Ext'd
	$('#ctl00_cphMasterBody_rp_Main_pnl_addnewline_TextTMExtd_I').val(quantity * newpart.total);

	return newpart.idOfCode;
}

function ShowPOInvoiceButton(show) {
	if (show == "Yes") {
		window.parent.window.invoiceButton.SetVisible(true);
	} else {
		window.parent.window.invoiceButton.SetVisible(false);
	}
}

function updatePoLineCompleteState(list) {
	if (list == null && list.length == null) {
		return;
	}

	if (!Array.isArray(list)) {
		return;
	}

	// console.log(list);

	var i = 0;
	for (i = 0; i < list.length; i++) {
		var object = document.getElementById(list[i].poLineId);
		var state = list[i].poLineState;
		if (object == null) {
			continue;
		}

		if (state == 1) {
			object.src = "/images/icon/icon[ok].gif";
		} else {
			object.src = "/images/icon/icon[ok_notyet].gif";
		}
	}

}
