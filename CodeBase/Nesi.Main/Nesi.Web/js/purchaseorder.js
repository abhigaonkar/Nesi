var po = {
	id: 0,
	obj_ts: null,
	bv_status_interval: null,
	bv_status:
	{
		set_timer:
			function () {
				//po.bv_status_interval	= setInterval("po.bv_status.run()", 10000);
			},
		run:
			function () {
				//PageMethods.bv_status(poprog_id, this.complete, po.error, po.timeout);
			},
		complete:
			function (arg) {
				var o = $.parseJSON(arg);
				var src = "";
				var title = "";
				switch (o.status) {
					case "good":
						src = "/images/quote/decal/decal[good_health].gif";
						title = "PO is currently not in use by anyone in BV";
						break;
					case "bad":
						src = "/images/quote/decal/decal[bad_health].gif";
						title = o.error;
						break;
					case "locked":
						src = "/images/quote/decal/decal[locked].gif"
						title = "Locked by " + o.locked_by;
						break;
				}

				$("#hb_indicator").attr({ "src": src, "title": title });
			}
	},
	timeout:
		function (arg) {
			clearInterval(po.bv_status_interval);
		},
	error:
		function (arg) {
			clearInterval(po.bv_status_interval);
		},
	tab_switch:
		function (s, e) {
			var poprog_id = $(".poprog_id").val();
			var vendor_id = ddl_vendor.GetValue();
			var if_picklist = $(".if_picklist");
			var if_vendor = $(".if_vendor");
			var if_files = $(".if_files");
			switch (e.tab.index) {
				case 3:
					if (if_picklist.attr("src") == undefined) {
						if_picklist.css(
							{
								"background-image": "url('/images/loading_panel.gif')",
								"background-repeat": "no-repeat",
								"background-position": "center center"
							});
						if_picklist.attr("src", "/sections/member/picklist/pikclist.aspx?id=" + poprog_id + "&rev=0&origin=purchaseorder&iframe=yes");
					}
					break;
				case 6:
					if ((if_vendor.attr("src") == undefined) && vendor_id != undefined && vendor_id != 0) {
						if_vendor.css(
							{
								"background-image": "url('/images/loading_panel.gif')",
								"background-repeat": "no-repeat",
								"background-position": "center center"
							});
						if_vendor.attr("src", "/#/opens/11/vendors/" + vendor_id);
					}
					break;
				case 7:
				case 8:
					if (if_files.attr("src") == undefined) {
						console.log(1234);
						if_files.css(
							{
								"background-image": "url('/images/loading_panel.gif')",
								"background-repeat": "no-repeat",
								"background-position": "center center"
							});
						if_files.attr("src", "/FileManager.aspx?parent_page=purchase_order&id=" + poprog_id);
					}
					break;
			}
		},
	handle_lastupdated: function (obj) {
		please_wait("start");
		$.get("./po_prog_add.aspx",
			{
				a: "update_ts",
				poprog_id: po.id
			},
			function () {
				po.update_ts();
				please_wait("stop");
			}
		);

	},
	update_ts() {
		$.get("./po_prog_add.aspx",
			{
				a: "get_ts",
				poprog_id: po.id
			},
			function (resp) {
				if (resp.match(/\d+/g)) {

					var vals = resp.split("|");
					$("#" + po.obj_ts).val(vals[0]);
					$("[id$='lbl_lastmodified']").text(vals[1]);
				}
			});
	}
}
