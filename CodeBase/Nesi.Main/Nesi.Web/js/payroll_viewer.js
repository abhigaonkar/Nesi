
var payroll_viewer = {
	toggle_complete:
		function (state, payperiod_id, business_unit_id) {
			if (payperiod_id != "") {
				var _company_appendage = "";
				if (business_unit_id) {
					_company_appendage = "&c_id=" + business_unit_id;
				}
				location.href = "./index.aspx?A=CHANGECOMPLETE&P=" + payperiod_id + "&STATE=" + state + _company_appendage;
			}
			else {
				return false;
			}
		},
	toggle_sent:
	{
		current_obj: null,
		run:
			function (obj) {
				this.current_obj = $(obj);
				var is_checked = this.current_obj.is(":checked");
				if (confirm("Are you sure you want to toggle this branches payroll sent status?")) {
					var pp_id = this.current_obj.attr("data-pp_id");
					var c_id = this.current_obj.attr("data-c_id");
					var saying = is_checked ? "Marking branches payroll as sent" : "Removing branches payroll sent status";
					please_wait("start", saying);
					PageMethods.toggle_sent(pp_id, c_id, is_checked, this.complete, payroll_viewer.error, payroll_viewer.timeout);
				}
				else {
					this.current_obj.attr("checked", !is_checked);
				}
			},
		complete:
			function (arg) {
				if (arg != "SUCCESS") {
					alert(arg);
					var is_checked = payroll_viewer.toggle_sent.current_obj.is(":checked");
					payroll_viewer.toggle_sent.current_obj.attr("checked", !is_checked);
				}
				else {
					var is_checked = payroll_viewer.toggle_sent.current_obj.is(":checked");
					var this_class = is_checked ? "icon sent" : "icon inprogress";
					payroll_viewer.toggle_sent.current_obj.parents(".company:first").find(".icon").attr("class", this_class);
				}
				please_wait("stop");
			}
	},
	timeout:
		function (arg) {
			alert("Timeout occured");
			please_wait("stop");
		},
	error:
		function (arg) {
			alert(arg._message);
			please_wait("stop");
		},
	do_export:
	{
		payroll:
			function (obj) {
				var payperiod_id = $("#payperiod_id").val();
				var adp_company_code = $("#adp_company_code").val();
				if (adp_company_code == "0") {
					alert("Please select a valid entity to export");
					return;
				}
				var url = './export.ashx?a=exportbusiness&p_id=' + payperiod_id + '&adp_company_code=' + adp_company_code;
				window.open(url, "download", "width=450,height=100");
				location.href = location.href;
			},
		wage:
			function (obj) {
				var adp_company_code = $("#adp_company_code").val();
				var payperiod_id = $("#payperiod_id").val();
				if (adp_company_code == "0") {
					alert("Please select a valid entity to export");
					return;
				}
				var url = './export.ashx?a=wage&adp_company_code=' + adp_company_code + "&payperiod_id=" + payperiod_id;
				window.open(url, "download", "width=450,height=100");
			}
	}
};