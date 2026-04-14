if (typeof (jQuery) != "undefined") {
	jQuery.fn.center = function () {
		this.css("position", "absolute");
		this.css("top", ($(window).height() - this.height()) / 2 + $(window).scrollTop() + "px");
		this.css("left", ($(window).width() - this.width()) / 2 + $(window).scrollLeft() + "px");
		return this;
	}
	$.maxZ = $.fn.maxZ = function (opt) {
		var def = { inc: 10, group: "*" };
		$.extend(def, opt);
		var zmax = 0;
		$(def.group).each(function () {
			var cur = parseInt($(this).css('z-index'));
			zmax = cur > zmax ? cur : zmax;
		});
		if (!this.jquery)
			return zmax;

		return this.each(function () {
			zmax += def.inc;
			$(this).css("z-index", zmax);
		});
	}
}
//---------------------------------------------------------------------
function hide_children(obj, is_top) {
	if (!is_top) {
		$(obj).show(0);
	}
	else {
		$(obj).next('.sub_pages.closed').hide(0);
	}
}
function keep_showing(obj) {
	$(obj).parents('.sub_pages:first').hide(0);
}


//---------------------------------------------------------------------
var page_obj = {
	header:
	{
		o_timer: null,
		show:
			function () {
				$('#header').animate({ 'height': '400px' }, 100, 'linear',
					function () {
						$(this).css({ 'border-bottom': 'solid 1px #ccc' });
					});
				$('#mainlogo').animate(
					{ 'height': '60px' },
					250,
					function () {
						$("[id $= 'tiles']").show(50,
							function () {
								$('#tileset').sortable(
									{
										cursor: "move",
										placeholder: "placeholder"
									}
								);
								$('#tileset').disableSelection();
							});
					});
			},
		over:
			function (_e) {
				page_obj.header.o_timer = setTimeout("page_obj.header.show();", 750);
			},
		out:
			function (_e) {
				clearTimeout(page_obj.header.o_timer);
				$("[id $= 'tiles']").hide();
				$("#header").animate({ "height": "100px" }, 50, function () { $(this).css({ 'border-bottom': '0' }) });
				$('#mainlogo').animate({ 'height': '80px' }, 250);
			},
		show_users:
			function () {
				page_obj.notification.push("list_users|");
			}
	},
	scrollToAnchor(aName) {
		var aTag = $("a[name='" + aName + "']");
		$('html,body').animate({ scrollTop: aTag.offset().top }, 500);
	},
	sleep: function (time) {
		return new Promise((resolve) => setTimeout(resolve, time));
	},
	plcapture:
		function () {
			var ts_end = new Date().getTime();
			var diff = ts_start == undefined ? 0 : ts_end - ts_start;
			var tar = $("#ctl00_pl_id");
			if (tar.val() == undefined) {
				tar = $("#pl_id");
			}
			if (tar != undefined && tar != null && tar.val() != "" && !isNaN(tar.val())) {
				$.get("/_tools/page_obj/index.ashx", { a: "plcapture", id: tar.val(), elements: this.elements(), cl_process: diff }, null);
			}
			$("#maincontent").find("td:first").attr("valign", "top");
		},
	elements:
		function () {
			return document.getElementsByTagName('*').length;
		},

	fvr_header:
		function (type, obj) {
			var n_divs = $("#fvr_panel").find('div').size();
			var o = {
				o: $(obj).offset(),
				n: $("#fvr_panel").css("display"),
				ow: $(obj).width(),
				oh: $(obj).height(),
				pw: 250,
				ph: n_divs * 30
			};
			var l = o.o.left - 215;
			var t = o.o.top - 5;

			$("#fvr_panel").css({
				"top": t + "px",
				"left": l + "px",
				"width": o.pw + "px",
				"border": "solid 1px #000",
				"display": "none",
				"background-color": "black",
				"opacity": "0.8",
				"color": "white",
				"border-radius": "5px",
				"border": "none",
				"position": "absolute",
				"padding": "5px",
				"z-index": 150000
			}).show(0);
			setTimeout("$(document).mousemove(function(e){var original_element	= e.srcElement || e.originalTarget;if($(original_element).parents('#fvr_panel').size() == 0 && $(original_element).attr('id') != 'fvr_panel'){$('#fvr_panel').hide();$(document).unbind('mousemove');}});", 100);
		},
	bindtips: function () {
		$(".opt1").each(function () {

			if ($.fn.tip) {
				$(this).tip({ width: 300, disable_offset: true });
			}

		});
		$(".opt2").each(function () {
			if ($.fn.tip) {
				$(this).tip({
					width: 600,
					disable_offset: true,
					//    note_handler:
					//		function () {
					//		    $("#vacheader_tabs").tabs(
					//				{
					//				    event: "mouseover"
					//				});
					//		}
				});
			}
		});
	},
	bind_vid:
		function () {
			if ($(".video_available").size() > 0) {
				var o = {
					ow: $("body").width(),
					oh: $(window).height()
				};
				var va = $(".video_available");
			}
		},
	pop_vid:
		function (id) {
			boing('/sections/messaging/popup_videoplayer.aspx?page_id=' + id, 'video_player', 535, 380);
		},
	pop_wiki_help:
		function (id) {
			boing('/sections/messaging/popup_wiki_help.aspx?page_id=' + id, 'wiki_help', 800, 600);
		},
	toggle_menu_options:
		function (obj) {
			var curr = $(obj).text();
			curr = curr.substring(0, 4);
			var text = curr == "Show" ? "Hide All" : "Show All";
			$(obj).text(text);
			$("#menu").find(".closed").toggle();
		},
	update_panel_progress:
	{
		bind:
			function () {
				Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(page_obj.update_panel_progress.start);
				Sys.WebForms.PageRequestManager.getInstance().add_endRequest(page_obj.update_panel_progress.stop);
			},
		start:
			function () {
				please_wait("start");
			},
		stop:
			function (sender, args) {
				if (args != undefined && args != null) {
					if (args.get_error() != undefined) {
						var errorMessage = args.get_error().message.replace("Sys.WebForms.PageRequestManagerServerErrorException:", "");;
						args.set_errorHandled(true);
						alert(errorMessage);
					}
				}
				please_wait("stop");
			}
	},
	root_flyout:
		function (from_cancel) {
			var rfw = root_flyout_window;
			if (!from_cancel) {
				$("body").css({ "overflow-y": "hidden" });
				rfw.Show();
				root_flyout_subject.Focus();
				page_obj.global_ticket.paste_image.init();
			}
			else if (rfw.GetVisible()) {
				rfw.Hide();
				$("body").css({ "overflow-y": "scroll" });
			}
		},
	global_message:
	{
		alert:
			function (msg, duration, run_script) {
				var str_msg = msg == undefined ? "" : typeof msg === 'object' ? 'object' : typeof msg == 'function' ? 'function' : msg + '';
				if (str_msg !== "") {
					if ($("body").find(".global_alert_modal").size() == 0) {
						var id = Date.now();
						this.lock_page();
						var message_font_size = str_msg.length > 50 ? "1em" : "1.5em";
						str_msg = str_msg.replace(/(?:\r\n|\r|\n)/g, "<br/>");
						var w = "<div id='global_alert_modal_" + id + "' class='global_alert_modal' style='background-color:transparent;position:fixed;width:100%;height:100%;top:0px;left:0px;z-index:9000000'></div>\
															<div id='global_alert_" + id + "' style='width:400px; height:200px; position: absolute; top:200px;left:40%; border:solid 5px #fff;box-shadow:0px 0px 10px #888;z-index:9000001'>\
																<div style='width:300px; padding:50px;min-height:100px;opacity:1.0;background-color:#fff;z-index:9000002'></div>\
																<div style='width:380px; height:150px;color: #000;text-align:center;font-family: Arial; font-weight: bold; font-size:"+ message_font_size + ";position: absolute;top:20px;left:10px;overflow:hidden;overflow-y:auto;z-index:9000003'>" + msg + (duration == null ? "" : "<br/><i style='font-size:0.75em;'>This message will automatically go away in " + duration / 1000 + " seconds</i>") + "</div>";
						if (duration == null) {
							w += "<div style='width:380px; text-align:center;font-family: Arial; font-weight: bold; font-size: 1.5em;position: absolute;bottom:4px;left:10px;z-index:9000003'><button id='global_alert_confirm_" + id + "' type='button' onclick=\"page_obj.global_message.remove(" + id + "); " + run_script + "\">Okay</button></div>";
						}
						w += "</div>";
						$("body").append(w);
						$("#global_alert_confirm_" + id).focus();
						setTimeout("window.scroll(0,0)", 50);
						//$(window).on("keydown", function(e){if(e.keyCode == 13 ||  e.keyCode == 32){page_obj.global_message.remove(id);e.preventDefault();}});
						//$("#global_alert_"+id)
						if (duration != null) {

							setTimeout("page_obj.global_message.remove(" + id + ")", duration);

						}
					}
				}
			},
		confirm:
			function (msg, options) // not working yet... dont use
			{
				var id = Date.now();
				this.lock_page();
				var w = "<div id='global_alert_modal_" + id + "' style='background-color:transparent;position:fixed;width:100%;height:100%;top:0px;left:0px;z-index:9000000'></div>\
														<div id='global_alert_" + id + "' style='width:400px; height:200px; position: absolute; top: " + top + "px; left: 50%; margin-top: -100px; margin-left: -120px;border:solid 5px #fff;box-shadow:0px 0px 10px #888;z-index:9000001'>\
															<div style='width:300px; padding:50px;min-height:100px;background-color:#000;opacity:1.0;position: absolute;z-index:9000002'></div>\
															<div style='width:380px; height:150px;color: white;text-align:center;font-family: Arial; font-weight: bold; font-size: 1.5em;position: absolute;top:20px;left:10px;overflow:hidden;overflow-y:auto;z-index:9000003'>" + msg + "</div>\
															<div style='width:380px; text-align:center;font-family: Arial; font-weight: bold; font-size: 1.5em;position: absolute;bottom:4px;left:10px;z-index:9000003'><button id='global_alert_confirm_" + id + "' type='button' onclick=\"page_obj.global_alert.remove(" + id + ")\">Cancel</button></div>\
														</div>";
				$("body").append(w);
			},
		remove:
			function (id) {
				$('#global_alert_' + id).remove();
				$('#global_alert_modal_' + id).remove();
				$(window).on("keydown", null);
				this.unlock_page();
			},
		lock_page:
			function () {
				$("html").css({ "overflow-y": "hidden", "user-select": "none", "-webkit-touch-callout": "none", "-webkit-user-select": "none", "-khtml-user-select": "none", "-moz-user-select": "none", "-ms-user-select": "none" });
				$("body").css({ "overflow-y": "hidden", "user-select": "none", "-webkit-touch-callout": "none", "-webkit-user-select": "none", "-khtml-user-select": "none", "-moz-user-select": "none", "-ms-user-select": "none" });
			},
		unlock_page:
			function () {
				$("html").css({ "overflow-y": "auto", "user-select": "initial", "-webkit-touch-callout": "initial", "-webkit-user-select": "initial", "-khtml-user-select": "default", "-moz-user-select": "initial", "-ms-user-select": "initial" });
				$("body").css({ "overflow-y": "auto", "user-select": "initial", "-webkit-touch-callout": "initial", "-webkit-user-select": "initial", "-khtml-user-select": "default", "-moz-user-select": "initial", "-ms-user-select": "initial" });
			},
		removeall:
			function () {
				$('div[id^="global_alert_"]').each(function () { $(this).remove() });
				$('div[id^="global_alert_modal_"]').each(function () { $(this).remove() });
				this.unlock_page();
			}
	},
	global_ticket:
	{
		is_processing: false,
		cb:
		{
			begin:
				function (s, e) {
					page_obj.global_ticket.is_processing = true;
				},
			error:
				function (s, e) {

				},
			end:
				function (s, e) {
					page_obj.global_ticket.is_processing = false;
					if (cbp_global_ticket.cpCollapse !== undefined && cbp_global_ticket.cpCollapse) {
						page_obj.global_ticket.paste_image.init();
					}
					else {
						page_obj.global_ticket.paste_image.init();
					}
				}
		},
		paste_image:
		{
			init:
				function () {
					$(".interface .col_left .paste_image").pasteImageReader(
						function (results) {
							var dataURL, filename;
							$(".interface .col_left .paste_image").css({ "border": "solid 2px #090", "background-repeat": "no-repeat", "background-size": "100%", "background-image": "url('" + results.dataURL + "')" });
							$(".file_blob").attr("value", results.dataURL);
							$(".interface .col_left .paste_image .cancel").show();
							$(".cancelpasted").show();
							return filename = results.filename, dataURL = results.dataURL, results;
						});
				},
			remove:
				function () {
					$(".interface .col_left .paste_image").css({
						"border": "dashed 2px #999",
						"background-repeat": "no-repeat",
						"background-size": "auto",
						"background-image": "url('/images/ticket/bg[paste-sm].png')"
					});
					$(".interface .col_left .paste_image .cancel").hide();
					$(".file_blob").attr("value", "");
				}
		},
		vote:
			function (ticket_id, should_vote) {
				gv_existing_tickets.PerformCallback("v|" + ticket_id + "|" + should_vote);
			}
	},
	getSellPrice:
		function (cost, qty, bu_id, before, success, complete, error) {
			if (isNaN(qty)) return;
			var url = "/_tools/sell_price/index.aspx?cost=" + cost + "&qty=" + qty + "&bu_id=" + bu_id;
			var qtysellprice = 0;
			$.ajax(
				{
					type: 'GET',
					url: url,
					dataType: 'xml',
					beforeSend:
						function () {
							if (typeof (before) !== "undefined" && before != null && before != "") {
								eval(before);
							}
						},
					success:
						function (xml) {
							$(xml).find('PriceReturn').each(function () {
								qtysellprice = $(this).attr('price') / 1;
								if (typeof (success) !== "undefined" && success != null && success != "") {
									eval(success);
								}
							});
						},
					complete:
						function () {
							if (typeof (complete) !== "undefined" && complete != null && complete != "") {
								eval(complete);
							}
							return qtysellprice;
						},
					error:
						function (XMLHttpRequest, textStatus, errorThrown) {
							if (typeof (error) !== "undefined" && error != null && error != "") {
								eval(error);
							}
							return "0";
						}
				});
		},
	working_company: function (obj) { $.get("/_tools/page_obj/index.ashx", { a: "working_company", business_unit_id: $(obj).val() }, function (resp) { if (resp == "SUCCESS") { location.href = location.href; } else { alert("There was an error"); } }); },
	maxHeight: function () {
		var tags = document.getElementsByTagName('*');
		var maxh = 0;
		//var o;
		//var j;
		for (var i = 0; i < tags.length; i++) {
			var x = tags[i].getBoundingClientRect();
			if (x.top > maxh && x.height != 0) {
				maxh = x.top + x.height;
				//o = tags[i];
				//j = x;
			}
		}
		//console.log(maxh);
		//console.log(o);
		//console.log(j);
		return maxh;
	},
	scrollCheck:
	{
		bind: function () {
			var inFrame = window.self !== window.top;
			if (!inFrame) {
				//$(window).scroll(page_obj.scrollCheck.check);
			}
			else {
				//	$(window.top).scroll(page_obj.scrollCheck.check);
			}
		},
		check: function (ev) {
			var inFrame = window.self !== window.top;
			if (!inFrame) {
				if ($(window).scrollTop() + $(window).height() > $(document).height() - 50) {
					page_obj.scrollCheck.adjust();
				}
			}
			else {
				if (window.top != null && window.top.document != null && $(window.top).scrollTop() + $(window.top).height() > $(window.top.document).height() - 50) {
					page_obj.scrollCheck.adjust();
				}
			}
		},
		adjust: function () {
			var inFrame = window.self !== window.top;
			if (!inFrame) {
				$("body").height($("body").height() + 50);
			}
			else {
				var frame = $(window.top.document).find("#nesi1framediv");
				frame.height(frame.height() + 50);
			}
		}
	}
};
//---------------------------------------------------------------------
function inspect(a, b, c) {
	var d = '', e, f;
	if (c == null) {
		c = 0;
	}
	if (b == null) {
		b = 1;
	}
	if (b < 1) {
		return '<font color="red">Error: Levels number must be > 0</font>';
	}
	if (a == null || a == undefined) {
		return '<font color="red">Error: Object <b>NULL</b></font>';
	}
	d += '<ul>';
	for (property in a) {
		try {
			e = typeof (a[property]);
			d += "<li><b style='color:#00f;'>(" + e + ")</b> " + property + ((a[property] == null) ? (': <b>null</b>') : ('')) + '</li>';
			if ((e == 'object') && (a[property] != null) && (c + 1 < b)) {
				d += inspect(a[property], b, c + 1);
			}
		}
		catch (err) {
			if (typeof (err) == 'string') {
				f = err;
			}
			else if (err.message) {
				f = err.message;
			}
			else if (err.description) {
				f = err.description;
			}
			else {
				f = 'Unknown';
			}
			d += '<li><font color="red">(Error) ' + property + ': ' + f + '</font></li>';
		}
	}
	d += '</ul>';
	if ($('#inspector_asdfqwer1234').length == 0) {
		$("body").append("<div id='inspector_asdfqwer1234' style='display:none'></div>");
		$('#inspector_asdfqwer1234').dialog({ title: "", autoOpen: false, width: 600, height: 600 });
	}
	$('#inspector_asdfqwer1234').html(d).dialog('open');
}
//---------------------------------------------------------------------
function toggle_menu(obj) {
	if ($("#menu").is(':visible')) {
		$("#menu").hide(0);
		$("#menu_container").css({ "width": "" });
		$(obj).css({ "left": "0px" });
	}
	else {
		$("#menu").show(0);
		$("#menu_container").css({ "width": "180px" });
		$(obj).css({ "left": "180px" });
	}
}

//---------------------------------------------------------------------
function isUserInputDataDirty_Select() {
	// Check property list
	var selects = [];
	$("#att_vals").find("select").each(function () {
		selects.push($(this).val());
	});

	for (var i = 0; i < selects.length; i++) {
		if (selects[i] != "0") {
			return true;
		}
	}

	return false;
}

function isUserInputDataDirty_VenderList() {
	var venderList = [];
	$('#new_pricing table tbody').find('tr').each(function () {
		var _vendor = $(this).find('.vendor').attr('data-id');
		venderList.push(_vendor);
	});

	for (var i = 0; i < venderList.length; i++) {
		if (venderList[i] != "" && venderList[i] != undefined) {
			return true;
		}
	}

	return false;
}

function onBlurEvent() {

	var tag_id = $("#TAG_ID").val();
	var hidPartCreated = $('#hidPartCreated').val();

	if (hidPartCreated == 1) {

		// The idea is that after one part is saved and also no related UI objects are changed, it should not show up the confirm when switch to another tag.
		// In terms of 'no related UI objects are changed', we need to check two points:
		// (1) When users selecting a different option, the hidPartCreated will be set to zero, so this code will not be hit.(see _branch.js line of 2525ish)
		// (2) When users select a new vendor after this save. (Note after saving, the vender list will be reset.) 
		// Conclusion: In case of 'hidPartCreated == 1', we still need to check the vender list should not be changed.
		var dirty = isUserInputDataDirty_VenderList();
		if (!dirty) {
			cleanup();
			return;
		}
	}

	if (tag_id == "") {
		var dirty1 = isUserInputDataDirty_Select();
		var dirty2 = isUserInputDataDirty_VenderList();

		if (dirty1 || dirty2) {
			var confirmed = confirm("It looks like you have started to fill out a part, are you sure you wish to change tags? \r\nDoing so will clear all information you have entered below.");
			if (!confirmed) {
				// Restore info
				var idValue = $("#TAG_ID").attr('data-saved-id');
				var nameValue = $("#TAG_ID").attr('data-saved-name');

				$("#TAG_ID").attr('title', '#' + idValue);
				$("#TAG_ID").attr('data-id', idValue);
				$("#TAG_ID").val(nameValue);

				return;
			}
		}

		cleanup();
	}
}

function cleanup() {
	$("#uniquestatus").html("");

	var box_target = "#att_vals";
	$(box_target).empty();

	$('.pricing').hide();
	$("#matched_parts").html("&nbsp;");

	$('#picture_box').dialog('close');

	$('#hidPartCreated').val(0);
}

//---------------------------------------------------------------------
function attach_ac(obj, which) {
	var prop = {
		link_url: "",
		attr: {
			o: null,
			h: 0,
			l: 0,
			t: 0
		}
	};
	if ($(obj).attr('data-isac') == 'false' || $(obj).attr('data-isac') == null || $(obj).attr('data-isac') == undefined) {
		var original_val = $(obj).val();
		$(obj).val('').attr('title', '');
		var url = "";
		if (which == "value") {
			var att_id = $(obj).attr('data-attribute_id');
			var _manufacturer = 0;
			$("body").find("input:hidden").each(function () {
				var this_id = $(this).attr("id");
				if (this_id != undefined && this_id.match(/attributebox_/g) && $(this).val() == "16") {
					_manu_exists = true;
					_manufacturer = $(this).parents(".attval:first").find("select").val();
					if (_manufacturer == 0) {
						$(obj).blur();
						return false;
					}
				}
			});
			url = '/_tools/ac_search/index.aspx?type=' + which + '&attribute_id=' + att_id + "&current_manufacturer=" + _manufacturer;
		}
		else if (which == "vendor") {
			var this_business_unit_id = "";
			var this_master_id = "";
			var this_tag_id = "";
			if ($(obj).attr('data-business_unit_id') != "" && $(obj).attr('data-business_unit_id') != undefined) {
				this_business_unit_id = $(obj).attr('data-business_unit_id');
			}
			if (($(obj).attr('data-master_id') == "" || $(obj).attr('data-master_id') == undefined) && ($(obj).attr('data-tag_id') == "" || $(obj).attr('data-tag_id') == undefined)) {
				//alert("Cannot add vendor autocomplete box without a valid master_id");
				//return false;
			}
			else if (($(obj).attr('data-tag_id') == "" || $(obj).attr('data-tag_id') == undefined)) {
				this_master_id = $(obj).attr('data-master_id');
			}
			if ($(obj).attr('data-tag_id') != "" && $(obj).attr('data-tag_id') != undefined) {
				this_tag_id = $(obj).attr('data-tag_id');
			}
			url = '/_tools/ac_search/index.aspx?type=' + which + '&business_unit_id=' + this_business_unit_id + '&master_id=' + this_master_id + "&tag_id=" + this_tag_id;
		}
		else {
			url = '/_tools/ac_search/index.aspx?type=' + which;
		}
		switch (which) {
			case "vendor":
				prop.title = "Click to load this vendor in a pop-up window";
				prop.link_url = "/sections/vendor/index.aspx?vendor_id=";
				break;
			case "customer":
				prop.title = "Click to load this customer in a pop-up window";
				prop.link_url = "/sections/customer/index.aspx?customer_id=";
				break;
		}
		var _width = $(obj).width() / 1;
		if (_width < 300) {
			_width = 300;
		}
		if ($(obj).attr('id') == 'i_search_criteria') {
			// Inventory Search Block ////////////////////////////////////////////////////////
			$(obj).keydown(function (e) {
				if (e.which == 13 && $(obj).val() != '') {
					return false;
				}
				else if (e.which == 13 && $(obj).val() == '') {
					return false;
				}
				if ((e.which == 8 || e.which == 46) && $(obj).val() == '' && !$('#i_search_attributes').is(':hidden')) {
					$('#i_search_attributes').fadeOut('fast');
					$('#i_search_results').empty();
				}
				else if (!$('#i_search_attributes').is(':hidden') && (e.which != 9 && e.which != 16 && e.which != 17 && e.which != 18 && e.which != 27 && e.which != 35 && e.which != 36)) {
					$('#i_search_attributes').fadeOut('fast');
					$('#i_search_results').empty();
				}
			});
			$(obj).keyup(function (e) {
				if ((e.which == 8 || e.which == 46) && $(obj).val() == '' && !$('#i_search_attributes').is(':hidden')) {
					$('#i_search_attributes').fadeOut('fast');
					$('#i_search_results').empty().animate({ 'min-height': '50px' });
				}
			});
			//////////////////////////////////////////////////////////////////////////////////
		}
		$(obj).autocomplete(url, {
			minChars: 1,
			delay: 400,
			autoFill: false,
			matchSubset: 1,
			matchContains: 1,
			datatype: which,
			max: 100,
			cacheLength: 10000,
			width: _width,
			selectOnly: 1,
			formatItem: function (row, i, n) {
				if (which == 'vendor') {
					if (row[1] == undefined) {
						return false;
					}
					else {
						return row[3] + ' - ' + row[0];
					}
				}
				else if (which == 'member') {
					var bgcolor = row[2] == 1 ? "#0c0" : "#c00";
					return "<span style='display:inline-block;width:13px;height:13px;border:solid 1px #999;border-radius:10px;background-color:" + bgcolor + "'>&nbsp;</span> " + row[4] + " -- " + row[0] + " - " + row[3];
				}
				else {
					return row[0];
				}
			}
		}).result(function (event, item) {
			if ($(obj).attr('class').match(/add_val/gi)) {
				$(obj).val(item[0].replace(/\&quot\;/gi, "\""));
				var selector = $(obj).parents('.attval:first').find('select');
				var selector_id = selector.attr('id');
				var should_confirm = true;
				//alert(selector_id);
				var collection = {
					a: "push_to_tag",
					attribute_value_id: item[1],
					attribute_id: $(obj).parents('.attval:first').find('.add_val').attr('data-attribute_id'),
					tag_id: $('#TAG_ID').attr('data-id')
				};

				//alert("ITEM 0:"+item[0]+"\nITEM 1:"+item[1]+"\nITEM 2:"+item[2]+"\nITEM 3:"+item[3]+"\nITEM 4:"+item[4]+"\nATTRIBUTE_ID:"+collection.attribute_id);
				var _manufacturer = 0;
				$("body").find("input:hidden").each(function () {
					var this_id = $(this).attr("id");
					if (this_id != undefined && this_id.match(/attributebox_/g) && $(this).val() == "16" && collection.attribute_id == "17") {
						_manufacturer = $(this).parents(".attval:first").find("select").val();
						if (_manufacturer == 0) {
							alert("You need to first choose a manufacturer, before setting a manufacturer part #");
							should_confirm = false;
						}
					}
				});
				if ($("#" + selector_id + " option[value=" + item[1] + "]").length > 0) {
					selector.val(item[1]);
					part_match(null);
					chk_part();
				}
				// It is used in another part, and the attribute is manufacturer part #, and it is used in the current tag already under the same attribute id
				else if (item[2] != "" && collection.attribute_id == "17") {
					var msg = url.match(/current_manufacturer/g)
						? "This attribute/value combination is already used in part #" + item[2] + "\nWould you like to goto this part?\n(Ok button = Yes, Cancel Button = No)"
						: "This value is already used in part #" + item[2] + "\nWould you like to goto this part?\n(Ok button = Yes, Cancel Button = No)";
					if (confirm(msg)) {
						location.href = "./index.aspx?a=get&tab=G&id=" + item[2];
					}
					else {
						$(obj).val("");
						$(obj).unbind();
						$(obj).focus(function () {
							attach_ac(obj, which);
						});
						$("#ac_link").remove();
						$(obj).attr('data-isac', 'false');
						$(obj).attr('data-id', '');
						$(obj).attr('title', '');
					}
				}
				else {
					$.get("./index.aspx", collection,
						function (_resp) {
							if (_resp == "SUCCESS") {
								selector.append($("<option></option>").attr("value", item[1]).attr('selected', true).text(item[0]));
								part_match(null);
								chk_part();
							}
							else {
								alert(_resp);
							}
						});
				}
				toggle_addval(obj);
			}
			if (item[0] == 'No Results') {
				$(obj).unbind();
				$(obj).focus(function () {
					attach_ac(obj, which);
				});
				$(obj).val('');
				$(obj).attr('data-isac', 'false');
				$(obj).attr('data-id', '');
				$(obj).attr('title', '');
				$(obj).blur();
				return false;
			}
			else {
				if ($(obj).attr('data-click') == undefined || $(obj).attr('data-click') == null) {
					if (which == 'vendor') {
						$(obj).val("(" + item[3] + ") " + item[0]);
					}
					$(obj).attr('title', '#' + item[1]);
					$(obj).attr('data-id', item[1]);

					$(obj).attr('data-saved-name', item[0]);
					$(obj).attr('data-saved-id', item[1]);

					if ($(obj).attr('id') == 'i_search_criteria') {
						if ($('#i_search_attributes').is(':hidden')) {
							$('#i_search_attributes').slideDown().i_build_attributes();
							$('#i_search_attributes').refine_search();
						}
						else {
							$('#i_search_attributes').i_build_attributes();
							$('#i_search_attributes').refine_search();
						}
					}
					if ($(obj).attr('class').match(/customer/g) && location.href.match(/\?a\=g/gi)) {
						update_customer(item[1]);
						update_contacts(item[1]);
						check_open(item[1]);
					}
					else if ($(obj).attr('id') != undefined && $(obj).attr('id').match(/customer/g) && location.href.match(/quote/gi)) {
						update_contacts(item[1], obj);
						check_open(item[1], true);
					}
					if ($(obj).attr('id') != undefined && $(obj).attr('id') == "TAG_ID" && location.href.match(/member\/inventory/gi)) {
						start_part(obj);
					}
					if (location.href.match(/member\/inventory/gi) && $(obj).attr('data-orders') == 'true') {
						order_v_info(obj, true);
					}
				}
				else {
					$(obj).attr('title', which.toUpperCase() + '#: ' + item[1]);
					$(obj).attr('data-id', item[1]);
					if (which == 'vendor') {
						$(obj).val("(" + item[3] + ") " + item[0]);
					}
					else {
						$(obj).val("(" + item[1] + ") " + item[0]);
					}
					eval($(obj).attr('data-click'));
				}
				prop.attr.o = $(obj).offset();
				prop.attr.l = ($(obj).width() / 1) + ($(obj).offset().left / 1) - 20;
				prop.attr.t = $(obj).offset().top + 1;
				prop.attr.h = $(obj).height();

				if (prop.link_url != "" && location.href.match(/a=get&sstab/g)) {
					prop.link_url += item[1];
					$(obj).parent().append("<div id='ac_link' style='width:20px;height:16px;text-align:right;background-color:#fff;position:absolute;font-weight:bold;left:" + prop.attr.l + "px;top:" + prop.attr.t + "px;'><img src='/images/icon/icon[link].gif' title='" + prop.title + "' style='cursor:pointer' onclick=\"boing('" + prop.link_url + "', '" + which + "', 1024,768);\" border='0'/></div>");
				}
			}
		});
		function formatMe(row) {
			return row[0] + " (#:" + row[1] + ")";
		}


		$(obj).blur(function () {
			if ($.trim($(obj).val()) == "") {
				$(obj).val(original_val);
				$(obj).unbind();
				$(obj).focus(function () {
					attach_ac(obj, which);
				});
				$("#ac_link").remove();
				$(obj).attr('data-isac', 'false');
				$(obj).attr('data-id', '');
				$(obj).attr('title', '');
			}
		});
		$(obj).attr('data-isac', 'true');
	}
	return true;
}
//---------------------------------------------------------------------
function control_min_max(obj, min, max) {
	if ($(obj).val() == "") {
		$(obj).val(min);
	}
	var current_value = $(obj).val() / 1;
	var valid_min = current_value >= min;
	var valid_max = current_value <= max;
	if (!valid_min) {
		$(obj).val(min);
	}
	else if (!valid_max) {
		$(obj).val(max);
	}
}
function only_int(event, is_minmax) {
	var e = event;
	var key = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
	var delta = e.wheelDelta ? e.wheelDelta : e.detail;
	var tar = e.target == undefined ? $(e.srcElement) : $(e.target);
	var current = tar.val() == "" ? 0 : tar.val() / 1;
	var _goodkeys = [96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 35, 36, 37, 39, 8, 9, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 189, 109];
	var _returned = $.inArray(key, _goodkeys);
	if (key == 38) { tar.focus().val(current + 1); }
	else if (key == 40 && current > 1) { tar.focus().val(current - 1); }
	else if (key == 33 && e.shiftKey && e.ctrlKey) { tar.focus().val(current + 1000); e.returnValue = false; }
	else if (key == 33 && e.shiftKey) { tar.focus().val(current + 100); e.returnValue = false; }
	else if (key == 33) { tar.focus().val(current + 10); e.returnValue = false; }
	else if (key == 34 && current - 1000 > 0 && e.shiftKey && e.ctrlKey) { tar.focus().val(current - 1000); e.returnValue = false; }
	else if (key == 34 && current - 100 > 0 && e.shiftKey) { tar.focus().val(current - 100); e.returnValue = false; }
	else if (key == 34 && current - 10 > 0) { tar.focus().val(current - 10); e.returnValue = false; }
	else if (key == 189 && tar.val().length > 0) { e.returnValue = false; }
	else {
		e.returnValue = e.shiftKey ? false : (_returned > -1);
	}
}
//---------------------------------------------------------------------
function only_numeric(event) {
	var e = event;
	var key = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
	var delta = e.wheelDelta ? e.wheelDelta : e.detail;
	var tar = e.target == undefined ? $(e.srcElement) : $(e.target);
	var current = tar.val() == "" ? 0 : tar.val() / 1;
	var _goodkeys = [96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 35, 36, 37, 39, 8, 9, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 190, 110, 189, 109];
	var _returned = $.inArray(key, _goodkeys);

	if (key == 38) { tar.focus().val(current + 1); }
	else if (key == 40 && current > 1) { tar.focus().val(current - 1); }
	else if (key == 33 && e.shiftKey && e.ctrlKey) { tar.focus().val(current + 1000); e.returnValue = false; }
	else if (key == 33 && e.shiftKey) { tar.focus().val(current + 100); e.returnValue = false; }
	else if (key == 33) { tar.focus().val(current + 10); e.returnValue = false; }
	else if (key == 34 && current - 1000 > 0 && e.shiftKey && e.ctrlKey) { tar.focus().val(current - 1000); e.returnValue = false; }
	else if (key == 34 && current - 100 > 0 && e.shiftKey) { tar.focus().val(current - 100); e.returnValue = false; }
	else if (key == 34 && current - 10 > 0) { tar.focus().val(current - 10); e.returnValue = false; }

	else if (key == 189 && current.length > 0) { e.returnValue = false; }
	{
		e.returnValue = e.shiftKey && key != 9 ? false : (_returned > -1);
	}
}
//---------------------------------------------------------------------
function please_wait(action, _caption) {
	if (action) {
		//N1 nesi fix
		//try {
		//	var t				= window.location != window.parent.location ? document.body : "body";
		//}catch(exc){
		//	var t				= "body";
		//}
		//console.log(parent.document.location == window.location);

		var t = "body";
		var caption = _caption == undefined || _caption == null || _caption == "" ? "Please wait..." : _caption;

		var topPos = 0;
		//t = top.document.body;
		topPos = top.window.pageYOffset + (top.window.innerHeight * .49);
		leftPos = top.window.pageXOffset + (top.window.innerWidth * .48);

		switch (action) {
			case "start":
			case "begin":
				if ("activeElement" in document) {
					document.activeElement.blur();
				}
				var div_index = $.maxZ() + 1;
				var img_index = $.maxZ() + 2;
				$(t).css({ 'overflow': 'hidden' });
				$(t).append("<div id='progress_1234567890' class='progress_bar' style='z-index:" + div_index + "; top:0px' align='center'></div><div class='progress_barimg' style='z-index:" + img_index + "; top:" + topPos + "px; left:" + leftPos + "px;'  align='center' id='progress_12345678901'><img src='/images/loading_panel.gif' title=\"" + caption + "\"/></div>");

				break;
			case "finish":
			case "end":
			case "stop":
				$(t).css({ 'overflow': 'visible' });
				$(t).find('#progress_1234567890').remove();
				$(t).find('#progress_12345678901').remove();
				break;
		}
	}
}
//---------------------------------------------------------------------
//---------------------------------------------------------------------
function toTitleCase(str) {
	return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
}
//---------------------------------------------------------------------
function functionlist() {
	return true;
}
//---------------------------------------------------------------------
function unescape_me(str) {
	str = "" + str;
	while (true) {
		var i = str.indexOf('+');
		if (i < 0)
			break;
		str = str.substring(0, i) + '%20' +
			str.substring(i + 1, str.length);
	}
	return unescape(str);
}

//---------------------------------------------------------------------
function checkEnter() {
	var key = window.event.keyCode ? window.event.keyCode : window.event.which;
	return (key == 13);
}

//---------------------------------------------------------------------
function screen_width() {
	var screenwidth = 0;
	if (typeof (window.innerWidth) == 'number')									//Non-IE
	{
		screenwidth = window.innerWidth;
	}
	else if (document.documentElement && (document.documentElement.clientWidth))	//IE 6+ in 'standards compliant mode'
	{
		screenwidth = document.documentElement.clientWidth;
	}
	else if (document.body && (document.body.clientWidth))						//IE 4 compatible
	{
		screenwidth = document.body.clientWidth;
	}
	return screenwidth;
}

//---------------------------------------------------------------------
function screen_height() {
	var screenheight = 0;
	if (typeof (window.innerWidth) == 'number')								    //Non-IE
	{
		screenheight = window.innerHeight;
	}
	else if (document.documentElement && (document.documentElement.clientHeight))//IE 6+ in 'standards compliant mode'
	{
		screenheight = document.documentElement.clientHeight;
	}
	else if (document.body && (document.body.clientHeight))						//IE 4 compatible
	{
		screenheight = document.body.clientHeight;
	}
	return screenheight;
}

//---------------------------------------------------------------------
function changeIt(imageName, objName) {
	var obj = document.getElementById(objName);
	var imgTag = "<IMG id='" + objName + "' SRC='" + imageName + "'>";
	obj.src = imageName;
}

//---------------------------------------------------------------------
function changeprevious(imageName, objName) {
	var obj = document.getElementById(objName);
	var imgTag = "<IMG id='" + objName + "' SRC='" + imageName + "'>";
	obj.src = imageName;
}

//---------------------------------------------------------------------
function isNumber(number) {
	if (number.search(/^[0-9\.]/)) {
		return false;
	}
	else {
		return true;
	}
}

/*	------------------------- Total Payroll Hours Section ---------------------------------*/


function state_check(id) {
	var Target = document.getElementById(id);
	var Value = Target.value;

	if (Value == "0") {
		Target.value = "";
	}
	Target.focus();
}

function ExportBusiness(PID) {
	var BID = document.getElementById("BID").value;
	var URL = './index.aspx?A=EXPORTBUSINESS&P=' + PID + '&BID=' + BID;
	if (window.open(URL, "download", "width=1,height=1")) {
		location.href = location.href;
	}
}


function get_xml_doc(url) {
	xml_doc = new ActiveXObject('Microsoft.XMLDOM');
	xml_doc.async = false;
	xml_doc.load(url);
	parse_xml_doc();
}

function parse_xml_doc() {
	switch (which) {
		case "Inv_Att": var attributes = xml_doc.getElementsByTagName('attribute');
			var n_attributes = attributes.length;
			if (n_attributes > 0) {
				for (i = 1; i <= n_attributes; i++) {
					var sub_i = i - 1;
					var attribute = attributes.item(sub_i);
					var AttributeName = attribute.getAttribute('name');
					var AttributeId = attribute.getAttribute('id');
					attribute_box[i] = new Option(AttributeName, AttributeId);
				}
			}
			break;
		case "Inv_Val": var values = xml_doc.getElementsByTagName('value');
			var n_values = values.length;
			if (n_values > 0) {
				value_box.length = 0;
				value_box[0] = new Option('SELECT VALUE', 0);
				for (i = 1; i <= n_values; i++) {
					var sub_i = i - 1;
					var value = values.item(sub_i);
					var ValueName = value.getAttribute('name');
					var ValueId = value.getAttribute('id');
					value_box[i] = new Option(ValueName, ValueId);
				}
			}
			else {
				value_box.length = 0;
				value_box[0] = new Option('No Values', 0);
				value_box.disabled = true;
			}
			break;
		case "Inv_Pre": var values = xml_doc.getElementsByTagName('value');
			var n_values = values.length;
			var box_height = 0;
			if (n_values > 0) {
				value_box.style.backgroundColor = "#FFFFFF";
				box_height = 160;
				value_box.style.height = box_height + "px";
				value_box.length = 0;
				for (i = 0; i < n_values; i++) {
					var sub_i = i;
					var value = values.item(sub_i);
					var ValueName = value.getAttribute('name');
					var ValueId = value.getAttribute('id');
					value_box[i] = new Option(ValueName, ValueId);
				}
			}
			else {
				value_box.length = 0;
				value_box.disabled = true;
				value_box[0] = new Option('NO VALUES AVAILABLE FOR THIS ATTRIBUTE, PLEASE ADD VALUES BEFORE PROCEEDING', 0);
				value_box.style.height = "25px";
				value_box.style.backgroundColor = "#660000";
			}
			break;
	}
}

function load_XML(url) {
	var xmlHttpReq = false;
	var self = this;
	if (window.XMLHttpRequest)																// Mozilla/Safari
	{
		self.xmlHttpReq = new XMLHttpRequest();
	}
	else if (window.ActiveXObject)															// IE
	{
		self.xmlHttpReq = new ActiveXObject("Microsoft.XMLHTTP");
	}
	self.xmlHttpReq.open('GET', url, true);
	self.xmlHttpReq.send(null);
	self.xmlHttpReq.onreadystatechange = function () {
		if (self.xmlHttpReq.readyState == 4) {
			process_XML(self.xmlHttpReq.responseText);
		}
	}
}


function process_XML(str) {
	switch (which) {
		case "Inv_Att": var attributes = xml_doc.getElementsByTagName('attribute');
			var n_attributes = attributes.length;
			if (n_attributes > 0) {
				for (i = 1; i <= n_attributes; i++) {
					var sub_i = i - 1;
					var attribute = attributes.item(sub_i);
					var AttributeName = attribute.getAttribute('name');
					var AttributeId = attribute.getAttribute('id');
					attribute_box[i] = new Option(AttributeName, AttributeId);
				}
			}
			break;
		case "Inv_Val": var values = xml_doc.getElementsByTagName('value');
			var n_values = values.length;
			if (n_values > 0) {
				value_box.length = 0;
				value_box[0] = new Option('SELECT VALUE', 0);
				for (i = 1; i <= n_values; i++) {
					var sub_i = i - 1;
					var value = values.item(sub_i);
					var ValueName = value.getAttribute('name');
					var ValueId = value.getAttribute('id');
					value_box[i] = new Option(ValueName, ValueId);
				}
			}
			else {
				value_box.length = 0;
				value_box[0] = new Option('No Values', 0);
				value_box.disabled = true;
			}
			break;
		case "Inv_Pre": var values = xml_doc.getElementsByTagName('value');
			var n_values = values.length;
			var box_height = 0;
			if (n_values > 0) {
				value_box.style.backgroundColor = "#FFFFFF";
				if (n_values <= 10) {
					box_height = n_values * 16;
				}
				else {
					box_height = 160;
				}
				value_box.style.height = box_height + "px";
				value_box.length = 0;
				for (i = 0; i < n_values; i++) {
					var sub_i = i;
					var value = values.item(sub_i);
					var ValueName = value.getAttribute('name');
					var ValueId = value.getAttribute('id');
					value_box[i] = new Option(ValueName, ValueId);
				}
			}
			else {
				value_box.length = 0;
				value_box.disabled = true;
				value_box[0] = new Option('NO VALUES AVAILABLE FOR THIS ATTRIBUTE, PLEASE ADD VALUES BEFORE PROCEEDING', 0);
				value_box.style.height = "25px";
				value_box.style.backgroundColor = "#660000";
			}
			break;
		case "Payroll":
			switch (str) {
				case "Exists": alert("This employee's time was populated while you were reviewing.\nYou will be directed to the next available employee.");
					location.href = "./";

				case "Success": var NextID = document.getElementById("DB_next_id").value;
					location.href = "./";
					break;
				case "Failed": document.getElementById("send").disabled = false;
					alert("I am sorry, there was a problem sending payroll for this user.\nPlease contact the system administrator.");
					break;
			}
			break;
	}
}



function toggle(id) {
	var target = document.getElementById(id);
	var display = target.style.display;
	switch (display) {
		case "block": target.style.display = "none";

			break;
		default: target.style.display = "block";

			break;
	}
}

var bc_t;
function bc_out() {
	var bc = document.getElementById('bcode');
	if (bc != null) {
		bc_t = setTimeout("bc_destroy()", 100);
	}
}
function bc_destroy() {
	var bc = document.getElementById('bcode');
	if (bc) {
		bc.parentNode.removeChild(bc);
	}
}
function bc_in() {
	clearTimeout(bc_t);
}
function bc_click(id) {

	createPopupWin("/_tools/print_barcode/index.aspx?master_id=" + master_id, "Print Barcode", 600, 200);
	//var targ		= event.srcElement;
	//   please_wait("start");

	//$.get("/_tools/inventory_barcode/index.aspx", 
	//	{
	//	master_id:id
	//	}, 
	//	function(ret)
	//		{
	//		if(ret != "SUCCESS")
	//			{
	//			alert(ret);
	//			}
	//		please_wait("stop");
	//		});

}
function createPopupWin(pageURL, pageTitle,
	popupWinWidth, popupWinHeight) {
	var left = (screen.width - popupWinWidth) / 2;
	var top = (screen.height - popupWinHeight) / 4;
	var myWindow = window.open(pageURL, pageTitle,
		'resizable=yes, width=' + popupWinWidth
		+ ', height=' + popupWinHeight + ', top='
		+ top + ', left=' + left);
}
function bc(obj) {
	bc_destroy();
	var j = {
		left: $(obj).offset().left + $(obj).width(),
		top: $(obj).offset().top
	};
	var b = "<button id='bcode' onclick='bc_click(" + $(obj).text() + ")' style='font-size:11px;font-weight:bold;position:absolute;z-index:1000000;left:" + j.left + "px;top:" + j.top + "px;' onmouseover='bc_in()' onmouseout='bc_out()' type='button'><img src='/images/icon/icon[print_barcode].gif' align='absmiddle'> Print " + $(obj).text() + "</button>";
	$("body").append(b);
	$(obj).unbind().hover(function () {
		bc_in();
	},
		function () {
			bc_out();
		});
}


function boing(b_loc, b_pagename, b_width, b_height) {
	var s_width = screen.width;
	var s_height = screen.height;
	var b_leftpos = (s_width - b_width) / 2;
	var b_toppos = (s_height - b_height) / 2;
	//
	if (b_height == undefined) {
		b_height = s_height;
	}
	if (b_width == undefined) {
		b_width = s_width;
	}

	var b_settings = "left=" + b_leftpos;
	b_settings += ", top=" + b_toppos;
	b_settings += ", toolbar=no";
	/* b_settings			+= ", location=yes"; */
	b_settings += ", directories=no";
	b_settings += ", status=yes";
	b_settings += ", menubar=no";
	b_settings += ", scrollbars=yes";
	b_settings += ", resizable=yes";
	b_settings += ", width=" + b_width;
	b_settings += ", height=" + b_height;

	var b_win = window.open(b_loc, b_pagename, b_settings);
	b_win.focus();
}

function boing2(b_loc, b_pagename, b_width, b_height) {
	var s_width = screen_width();
	var s_height = screen_height();
	var b_leftpos = (s_width - b_width) / 2;
	var b_toppos = (s_height - b_height) / 2;
	if (b_height == undefined) {
		b_height = s_height;
	}
	if (b_width == undefined) {
		b_width = s_width;
	}

	var b_settings = "screenX=" + b_leftpos;
	b_settings += ", screenY=" + b_toppos;
	b_settings += ", left=" + b_leftpos;
	b_settings += ", top=" + b_toppos;
	b_settings += ", toolbar=0";
	/* b_settings			+= ", location=1"; */
	b_settings += ", directories=0";
	b_settings += ", status=0";
	b_settings += ", menubar=0";
	b_settings += ", scrollbars=0";

	b_settings += ", width=" + b_width;
	b_settings += ", height=" + b_height;

	var b_win = window.open(b_loc, b_pagename, b_settings);
	b_win.focus();
}
String.prototype.strReverse = function () {
	var newstring = "";
	for (var s = 0; s < this.length; s++) {
		newstring = this.charAt(s) + newstring;
	}
	return newstring;
	//strOrig = ' texttotrim ';
	//strReversed = strOrig.revstring();
};
function password_strength(pwd) {
	var orig_pwd = "";
	var oScorebar = $("scorebar");
	var oScore = $("score");
	var oComplexity = $("complexity");
	var nScore = 0, nLength = 0, nAlphaUC = 0, nAlphaLC = 0, nNumber = 0, nSymbol = 0, nMidChar = 0, nRequirements = 0, nAlphasOnly = 0, nNumbersOnly = 0, nUnqChar = 0, nRepChar = 0, nRepInc = 0, nConsecAlphaUC = 0, nConsecAlphaLC = 0, nConsecNumber = 0, nConsecSymbol = 0, nConsecCharType = 0, nSeqAlpha = 0, nSeqNumber = 0, nSeqSymbol = 0, nSeqChar = 0, nReqChar = 0, nMultConsecCharType = 0;
	var nMultRepChar = 1, nMultConsecSymbol = 1;
	var nMultMidChar = 2, nMultRequirements = 2, nMultConsecAlphaUC = 2, nMultConsecAlphaLC = 2, nMultConsecNumber = 2;
	var nReqCharType = 3, nMultAlphaUC = 3, nMultAlphaLC = 3, nMultSeqAlpha = 3, nMultSeqNumber = 3, nMultSeqSymbol = 3;
	var nMultLength = 4, nMultNumber = 4;
	var nMultSymbol = 6;
	var nTmpAlphaUC = "", nTmpAlphaLC = "", nTmpNumber = "", nTmpSymbol = "";
	var sAlphaUC = "0", sAlphaLC = "0", sNumber = "0", sSymbol = "0", sMidChar = "0", sRequirements = "0", sAlphasOnly = "0", sNumbersOnly = "0", sRepChar = "0", sConsecAlphaUC = "0", sConsecAlphaLC = "0", sConsecNumber = "0", sSeqAlpha = "0", sSeqNumber = "0", sSeqSymbol = "0";
	var sAlphas = "abcdefghijklmnopqrstuvwxyz";
	var sNumerics = "01234567890";
	var sSymbols = "~!@#$%^&*()_+";
	var sComplexity = "";
	var nMinPwdLen = 8;
	if (document.all) { var nd = 0; } else { var nd = 1; }
	if (pwd) {
		nScore = parseInt(pwd.length * nMultLength);
		nLength = pwd.length;
		var arrPwd = pwd.replace(/\s+/g, "").split(/\s*/);
		var arrPwdLen = arrPwd.length;

		for (var a = 0; a < arrPwdLen; a++) {
			if (arrPwd[a].match(/[A-Z]/g)) {
				if (nTmpAlphaUC !== "") { if ((nTmpAlphaUC + 1) == a) { nConsecAlphaUC++; nConsecCharType++; } }
				nTmpAlphaUC = a;
				nAlphaUC++;
			}
			else if (arrPwd[a].match(/[a-z]/g)) {
				if (nTmpAlphaLC !== "") { if ((nTmpAlphaLC + 1) == a) { nConsecAlphaLC++; nConsecCharType++; } }
				nTmpAlphaLC = a;
				nAlphaLC++;
			}
			else if (arrPwd[a].match(/[0-9]/g)) {
				if (a > 0 && a < (arrPwdLen - 1)) { nMidChar++; }
				if (nTmpNumber !== "") { if ((nTmpNumber + 1) == a) { nConsecNumber++; nConsecCharType++; } }
				nTmpNumber = a;
				nNumber++;
			}
			else if (arrPwd[a].match(/[^a-zA-Z0-9_]/g)) {
				if (a > 0 && a < (arrPwdLen - 1)) { nMidChar++; }
				if (nTmpSymbol !== "") { if ((nTmpSymbol + 1) == a) { nConsecSymbol++; nConsecCharType++; } }
				nTmpSymbol = a;
				nSymbol++;
			}
			var bCharExists = false;
			for (var b = 0; b < arrPwdLen; b++) {
				if (arrPwd[a] == arrPwd[b] && a != b) {
					bCharExists = true;
					nRepInc += Math.abs(arrPwdLen / (b - a));
				}
			}
			if (bCharExists) {
				nRepChar++;
				nUnqChar = arrPwdLen - nRepChar;
				nRepInc = (nUnqChar) ? Math.ceil(nRepInc / nUnqChar) : Math.ceil(nRepInc);
			}
		}

		for (var s = 0; s < 23; s++) {
			var sFwd = sAlphas.substring(s, parseInt(s + 3));
			var sRev = sFwd.strReverse();
			if (pwd.toLowerCase().indexOf(sFwd) != -1 || pwd.toLowerCase().indexOf(sRev) != -1) { nSeqAlpha++; nSeqChar++; }
		}

		for (var s = 0; s < 8; s++) {
			var sFwd = sNumerics.substring(s, parseInt(s + 3));
			var sRev = sFwd.strReverse();
			if (pwd.toLowerCase().indexOf(sFwd) != -1 || pwd.toLowerCase().indexOf(sRev) != -1) { nSeqNumber++; nSeqChar++; }
		}
		for (var s = 0; s < 8; s++) {
			var sFwd = sSymbols.substring(s, parseInt(s + 3));
			var sRev = sFwd.strReverse();
			if (pwd.toLowerCase().indexOf(sFwd) != -1 || pwd.toLowerCase().indexOf(sRev) != -1) { nSeqSymbol++; nSeqChar++; }
		}
		if (nAlphaUC > 0 && nAlphaUC < nLength) {
			nScore = parseInt(nScore + ((nLength - nAlphaUC) * 2));
			sAlphaUC = "+ " + parseInt((nLength - nAlphaUC) * 2);
		}
		if (nAlphaLC > 0 && nAlphaLC < nLength) {
			nScore = parseInt(nScore + ((nLength - nAlphaLC) * 2));
			sAlphaLC = "+ " + parseInt((nLength - nAlphaLC) * 2);
		}
		if (nNumber > 0 && nNumber < nLength) {
			nScore = parseInt(nScore + (nNumber * nMultNumber));
			sNumber = "+ " + parseInt(nNumber * nMultNumber);
		}
		if (nSymbol > 0) {
			nScore = parseInt(nScore + (nSymbol * nMultSymbol));
			sSymbol = "+ " + parseInt(nSymbol * nMultSymbol);
		}
		if (nMidChar > 0) {
			nScore = parseInt(nScore + (nMidChar * nMultMidChar));
			sMidChar = "+ " + parseInt(nMidChar * nMultMidChar);
		}

		if ((nAlphaLC > 0 || nAlphaUC > 0) && nSymbol === 0 && nNumber === 0) {  // Only Letters
			nScore = parseInt(nScore - nLength);
			nAlphasOnly = nLength;
			sAlphasOnly = "- " + nLength;
		}
		if (nAlphaLC === 0 && nAlphaUC === 0 && nSymbol === 0 && nNumber > 0) {  // Only Numbers
			nScore = parseInt(nScore - nLength);
			nNumbersOnly = nLength;
			sNumbersOnly = "- " + nLength;
		}
		if (nRepChar > 0) {  // Same character exists more than once
			nScore = parseInt(nScore - nRepInc);
			sRepChar = "- " + nRepInc;
		}
		if (nConsecAlphaUC > 0) {  // Consecutive Uppercase Letters exist
			nScore = parseInt(nScore - (nConsecAlphaUC * nMultConsecAlphaUC));
			sConsecAlphaUC = "- " + parseInt(nConsecAlphaUC * nMultConsecAlphaUC);
		}
		if (nConsecAlphaLC > 0) {  // Consecutive Lowercase Letters exist
			nScore = parseInt(nScore - (nConsecAlphaLC * nMultConsecAlphaLC));
			sConsecAlphaLC = "- " + parseInt(nConsecAlphaLC * nMultConsecAlphaLC);
		}
		if (nConsecNumber > 0) {  // Consecutive Numbers exist
			nScore = parseInt(nScore - (nConsecNumber * nMultConsecNumber));
			sConsecNumber = "- " + parseInt(nConsecNumber * nMultConsecNumber);
		}
		if (nSeqAlpha > 0) {  // Sequential alpha strings exist (3 characters or more)
			nScore = parseInt(nScore - (nSeqAlpha * nMultSeqAlpha));
			sSeqAlpha = "- " + parseInt(nSeqAlpha * nMultSeqAlpha);
		}
		if (nSeqNumber > 0) {  // Sequential numeric strings exist (3 characters or more)
			nScore = parseInt(nScore - (nSeqNumber * nMultSeqNumber));
			sSeqNumber = "- " + parseInt(nSeqNumber * nMultSeqNumber);
		}
		if (nSeqSymbol > 0) {  // Sequential symbol strings exist (3 characters or more)
			nScore = parseInt(nScore - (nSeqSymbol * nMultSeqSymbol));
			sSeqSymbol = "- " + parseInt(nSeqSymbol * nMultSeqSymbol);
		}

		/* Determine complexity based on overall score */
		if (nScore > 100) { nScore = 100; } else if (nScore < 0) { nScore = 0; }
		if (nScore >= 0 && nScore < 20) { sComplexity = "Very Weak"; }
		else if (nScore >= 20 && nScore < 40) { sComplexity = "Weak"; }
		else if (nScore >= 40 && nScore < 60) { sComplexity = "Good"; }
		else if (nScore >= 60 && nScore < 80) { sComplexity = "Strong"; }
		else if (nScore >= 80 && nScore <= 100) { sComplexity = "Very Strong"; }

		return (sComplexity);
	}
}


function getParameterByName(name, url) {
	if (!url) url = window.location.href;
	name = name.replace(/[\[\]]/g, "\\$&");
	var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
		results = regex.exec(url);
	if (!results) return null;
	if (!results[2]) return '';
	return decodeURIComponent(results[2].replace(/\+/g, " "));
}
function redirectAngular() {
	var inNonFrame = false;
	try {
		inNonFrame = document.getElementsByTagName("body")[0].id.match(/.+NonFrameMasterPageBodyTag/gi).length == 1;
	} catch (err) {
		console.log(err);
	}

	if (top.document.getElementById("Body") == null && getParameterByName('is_n1') != 'true' && !inNonFrame) {
		if (location.href.indexOf('age_id=') >= 0) {
			//get pageID and send to angular
			var index = location.href.indexOf('age_id=');
			//  console.log(index);
			var indexAnd = location.href.indexOf('&');
			//   console.log(indexAnd);
			if (indexAnd != -1)
				var page = location.href.substring(index + 7, indexAnd);
			else
				var page = location.href.substring(index + 7);
			//   console.log(page);

			var pageId = page;
			top.location.href = "/#/home/1/" + pageId + "/default";
			//	return false;
		} else if (location.href.indexOf('?') >= 0) {
			top.location.href = "/#/home/0/" + encodeURIComponent(location.pathname + location.search);
			// return false;
		} else {
			top.location.href = "/#/home/0/" + encodeURIComponent(location.pathname + "?is_n1=true");
			// return false;
		}
	}
};

var resizeTick = 0;
document.addEventListener("DOMContentLoaded", function () {
	if (parent) {
		try {
			if (parent.myCustomFlag) {
				return;
			}
		} catch (e) {
			return;
		}
		setIframeSIze();
		// setup an interval to send a message to the parent window every second. interval is in milliseconds.
		var _interval = 1000;
		setInterval(function () { setIframeSIze(); }, _interval);
		return;
	}
	//}
});
if (window.parent) {
	window.parent.window.addEventListener("resize", function () {
		resizeTick = 0;
	});
}

function setIframeSIze() {
	// get the document width and height
	var _height = $('body').height();
	var _width = top.document.getElementById("nesi1iframe").contentWindow.document.body.scrollWidth;
	var isAtMaxWidth = screen.availWidth - window.innerWidth === 0;
	var _title = document.title;


	var maxHeight = page_obj.maxHeight();

	if (_height > 12000) {
		_height = 12000;
	} else if (_height < maxHeight) {
		_height = maxHeight;
	}
	if (parent.parent.location != parent.location) {
		_width = parent.document.body.scrollWidth;
		_title = parent.document.title;
	}



	top.document.title = _title;
	top.$("#nesi1framediv").height(_height);
	if (isAtMaxWidth || resizeTick == 0) {
		top.$("#nesi1framediv").width(_width);
		resizeTick++;
	}
    /*if (!(navigator.userAgent.search("mobile") > 0 || navigator.userAgent.search("phone") > 0)) {
        top.$("#nesi1framediv").width(_width);
        _width = window.outerWidth + 'px';
        //top.document.getElementById("nesi1iframe").scrolling = "yes";
        top.$("#topbar").width(_width);
    } else {
        top.$("#nesi1framediv").width(_width);
    }*/

}

var myCustomFlag = true;