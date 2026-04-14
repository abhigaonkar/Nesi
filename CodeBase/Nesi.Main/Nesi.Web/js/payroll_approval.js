
var approval =
{
	tab_changed:
		function (s, e) {
			if (e.tab.index === 1) {
				cbp_summary.PerformCallback("refresh");
			}
		},
	end_callback:
		function (s, e) {
			if (s.GetMainElement().id.match(/cbp_approval/g)) {
				page_obj.bindtips();
				approval.calculate_hours(null, null, true);
			}
			if (typeof s.cpDoJS != "undefined" && s.cpDoJS != "") {
				eval(s.cpDoJS);
				delete (s.cpDoJS);
			}
		},
	change_branch:
		function (s, e) {
			cbp_payroll.PerformCallback();
		},
	gv_init:
		function (s, e) {
			approval.calculate_hours(null, null, false);
		},
	expense:
	{
		do:
			function (_obj, _row_id, _decision) {
				var t = _decision ? "approve" : "deny";
				var denyapp = _decision ? 1 : 0;
				if (confirm("Are you sure you want to " + t + " this expense/perdiem?")) {
					cbp_approval.PerformCallback("expense|" + _row_id + "|" + denyapp);
				}
			}
	},
	vacation:
	{
		do:
			function (_obj, _row_id, _decision) {
				var t = _decision ? "approve" : "deny";
				var denyapp = _decision ? 1 : 0;
				if (confirm("Are you sure you want to " + t + " this vacation?")) {
					cbp_approval.PerformCallback("vacation|" + _row_id + "|" + denyapp);
				}
			}
	},
	focus:
	{
		got:
			function (s, e) {
				if (s.GetText() / 1 == 0) {
					s.SetText("");
				}
				s.SelectAll();
			},
		lost:
			function (s, e) {
				if (s.GetText() == "" || s.GetText() / 1 == 0) {
					s.SetText("0.00");
				}
			}
	},
	dollarize:
		function (n) {
			return n.toLocaleString('US', { style: 'currency', currency: 'USD' });
		},
	line_total:
		function (rt, ot, dt, rtsp, otsp, dtsp, pto = 0) {
			var wage = $("input[id$=hid_wage]").val() / 1;
			return (rt * wage) + (ot * wage * 1.5) + (dt * wage * 2) + (rtsp * wage * 1.1) + (otsp * wage * 1.65) + (dtsp * wage * 2.2) + (pto * wage);
		},
	calculate_hours:
		function (s, e, is_total_box) {
			if (s != null) {
				var val = s.GetText();
				if (isNaN(val)) {
					s.SetText(0);
				}
			}
			var total = 0;
			var db_total = $("input[id$=hid_total_hours]").val() / 1;
			var wage = $("input[id$=hid_wage]").val() / 1;
			var is_salary = $("input[id$=hid_is_salary]").val() == "True";
			var all_outstanding = $("input[id$=hid_all_outstanding]").val() / 1 == 1;
			var vacation_total = $("input[id$=hid_total_vacation]").val() / 1;
			var holiday_total = $("input[id$=hid_total_holiday]").val() / 1;
			var basepay_total = $("input[id$=hid_total_basepay]").val() / 1;
			var text_box_array;
			if (!is_total_box) 
				{
				text_box_array = $("table[id$=gv_hours_DXMainTable]").find(".rt_box").find("input:text");

				text_box_array.each(function () {
					var this_value = $(this).val() / 1;
					var this_enabled = $(this).is(":enabled");
					total += this_value;
				});
				tb_rt.SetText((all_outstanding ? total : total - vacation_total).toFixed(2));
				approval.calculate_hours(null, null, true);
				}
			else 
				{
				var rt = tb_rt.GetText() / 1;
				var ot = tb_ot.GetText() / 1;
				var dt = tb_dt.GetText() / 1;
				var rtsp = tb_rtsp.GetText() / 1;
				var otsp = tb_otsp.GetText() / 1;
				var dtsp = tb_dtsp.GetText() / 1;
				total = rt + ot + dt + rtsp + otsp + dtsp;
				var final_hourly = approval.line_total(rt, ot, dt, rtsp, otsp, dtsp, 0);
				var final_payout = basepay_total + final_hourly;
				lb_finalpayout.SetText(approval.dollarize(final_payout));
				lb_verify_total.SetText(approval.dollarize(final_hourly));
				var unapproved_items = $("body").find(".unapproved").size();
				var total_holiday = 0;
				text_box_array = $("table[id$=gv_hours_DXMainTable]").find(".rt_box").find("input:text");
				var week_1_total = 0;
				var week_2_total = 0;
				var week_3_total = 0;
				var was_there_week_3 = false;
				text_box_array.each(function () // Loop through each RT box
					{
					var this_value = $(this).val() / 1;
					var this_tr = $(this).parents(".dxgvDataRow_NETheme01:first");
					var this_type = this_tr.find(".row_type").text();
					if (this_type != "Per Diem" && this_type != "Expense") 
						{
						var is_week_1 = $(this).parents("table:first").hasClass("w1");
						var is_week_2 = $(this).parents("table:first").hasClass("w2");
						var is_week_3 = $(this).parents("table:first").hasClass("w3");
						if (is_week_3) 
							{
							was_there_week_3 = true;
							}
						var this_rt = this_tr.find(".rt_box").find("input:text").val() / 1;
						var this_ot = this_tr.find(".ot").text() / 1;
						var this_dt = this_tr.find(".dt").text() / 1;
						var this_rtsp = this_tr.find(".rtsp").text() / 1;
						var this_otsp = this_tr.find(".otsp").text() / 1;
						var this_dtsp = this_tr.find(".dtsp").text() / 1;
						var this_paid_time_off = this_tr.find(".paid_time_off").text() / 1;
						var this_total = approval.line_total(this_rt, this_ot, this_dt, this_rtsp, this_otsp, this_dtsp, this_paid_time_off);
						var this_dollars = approval.dollarize(this_total);
						this_tr.find(".line_total").text(this_dollars);

						if (is_week_1) 
							{
							week_1_total += this_rt;
							}
						else if (is_week_2) 
							{
							week_2_total += this_rt;
							}
						else if (is_week_3) 
							{
							week_3_total += this_rt;
							}
						var is_holiday = $(this).parents("table:first").hasClass("holiday");
						if (is_holiday) 
							{
							total_holiday += this_rt;
							}
						}
					});

				$("input[id$=hid_total_holiday]").val(total_holiday);
				if (!was_there_week_3) 
					{
					$(".rt_group_summary:eq(0)").text(week_1_total.toFixed(2));
					$(".linetotal_group_summary:eq(0)").text(approval.dollarize(week_1_total * wage));
					$(".rt_group_summary:eq(1)").text(week_2_total.toFixed(2));
					$(".linetotal_group_summary:eq(1)").text(approval.dollarize(week_2_total * wage));
					$(".rt_total_summary").text((week_1_total + week_2_total).toFixed(2));
					}
				else 
					{
					$(".rt_group_summary:eq(0)").text(week_1_total.toFixed(2));
					$(".linetotal_group_summary:eq(0)").text(approval.dollarize(week_1_total * wage));
					$(".rt_group_summary:eq(1)").text(week_2_total.toFixed(2));
					$(".linetotal_group_summary:eq(1)").text(approval.dollarize(week_2_total * wage));
					$(".rt_group_summary:eq(2)").text(week_3_total.toFixed(2));
					$(".linetotal_group_summary:eq(2)").text(approval.dollarize(week_3_total * wage));
					$(".rt_total_summary").text((week_1_total + week_2_total + week_3_total).toFixed(2));
					}
				var db_holiday_total = (db_total + total_holiday).toFixed(2);
				var hours_equal = total == db_holiday_total;
				var salaried_hours_chk = true;
				var appendage = hours_equal && !is_salary 
									? "" 
									: "<span style='height:15px;display:block;color:red;font-size:13px;'>You have assigned " + total + " hours of " + db_holiday_total + " hours.</span>";
				if (is_salary) 
					{
					salaried_hours_chk = rt <= 80;
					hours_equal = salaried_hours_chk;
					appendage = salaried_hours_chk
						? ""
						: "<span style='height:15px;display:block;color:red;font-size:13px;'>You cannot assign more than 80 hours to a salaried employee, you can modify the RT hours in the textbox above and resubmit.</span>";
					}
				if (unapproved_items > 0 && gv_hours.GetVisible()) 
					{
					appendage += "<br/><span style='height:15px;display:block;color:red;font-size:13px;'>There are unapproved expenses, per diems or vacations above.</span>";
					}
				$("#div_status_box").html(appendage);
				bt_submit.SetEnabled(salaried_hours_chk && hours_equal && unapproved_items == 0);
			}
		},
	send_hours:
		function (s, e, user) {
			var rt = tb_rt.GetText();
			var ot = tb_ot.GetText();
			var dt = tb_dt.GetText();
			var rtsp = tb_rtsp.GetText();
			var otsp = tb_otsp.GetText();
			var dtsp = tb_dtsp.GetText();

			var rt_original = $("input[id$=hid_rt_original]").val() / 1;
			var ot_original = $("input[id$=hid_ot_original]").val() / 1;
			var dt_original = $("input[id$=hid_dt_original]").val() / 1;
			var rtsp_original = $("input[id$=hid_rtsp_original]").val() / 1;
			var otsp_original = $("input[id$=hid_otsp_original]").val() / 1;
			var dtsp_original = $("input[id$=hid_dtsp_original]").val() / 1;

			var total_holiday_original = $("input[id$=hid_total_holiday]").val() / 1;

			var holiday_array = $("table[id$=gv_hours_DXMainTable]").find(".holiday").find("input:text");
			var total_holiday = 0;
			holiday_array.each(function () {
				var this_value = $(this).val() / 1;
				total_holiday += this_value;
			});
			var packet =
			{
				RegularTime: rt - total_holiday,
				OverTime: ot,
				DoubleTime: dt,
				RegularTimeShiftPremium: rtsp,
				OverTimeShiftPremium: otsp,
				DoubleTimeShiftPremium: dtsp,
				rt_original: rt_original - total_holiday_original,
				ot_original: ot_original,
				dt_original: dt_original,
				rtsp_original: rtsp_original,
				otsp_original: otsp_original,
				dtsp_original: dtsp_original,
				StatPay: total_holiday
			}
			cbp_approval.PerformCallback("save|" + user + "|" + JSON.stringify(packet));
		},
	finalize_payroll:
		function (s, e) {
			if (confirm("Are you absolutely sure you wish to submit payroll?")) {
				cbp_summary.PerformCallback("submit");
			}
		},
	clear:
		function (s, e, row_id, user_id) {
			cbp_approval.PerformCallback("clear|" + row_id + "|" + user_id);
		},
	get_user:
		function (_s, _e) {
			cbp_approval.PerformCallback('get_user|' + _s.GetValue());
		}
}