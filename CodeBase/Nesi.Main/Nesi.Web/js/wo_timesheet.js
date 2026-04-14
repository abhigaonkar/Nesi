var timesheet = 
	{
	project_files: 
		{
		show: function ()
			{
			pop_files.Show();
			var woId = ddlWorkOrder.GetValue();
			$("#if_files").attr("src", "/filemanager.aspx?id="+woId+"&parent_page=workorder");
			}
		},
	perdiem:
		{
		wo_row_click: function(s, e)
			{
			// Grabs the values for the row with e.visibleIndex, selecting only the columns in the second parameter,
			// sending the resultant values to the function ValueParser, the parameter v in that function is implied.
			gv_workorders.GetRowValues(e.visibleIndex, 'woprog_id;woprog_bvwo;customer_name', wo_timesheet.perdiem.ValueParser);
			},
		 ValueParser: function(v)
			{
			var id			= v[0];
			var wo			= v[1];
			var customer	= v[2];
			perdiem_dde_workorder.SetKeyValue(id);
			perdiem_dde_workorder.SetText(wo + " - " + customer);
			pop_wos.Hide();
			perdiem_cb_shop.SetChecked(false);
			},
		perdiem_date_proc: function(s, _type)
			{
			var obj			= _type == 0 ? eval("perdiem_date_end") : eval("perdiem_date_start");
			var one_day		= 24*60*60*1000;
			var o			= 	{
								empname: $('.empname').val(),
								s_date:  perdiem_date_start.GetText(),
								e_date:	 perdiem_date_end.GetText(),
								d_s_date: perdiem_date_start.GetDate(),
								d_e_date: perdiem_date_end.GetDate(),
								o_s_date: eval("perdiem_date_start"),
								o_e_date: eval("perdiem_date_end"),
								country: $('.country').val()
								};
			if(s != null && _type != null)
				{
				if(obj.GetText() != '')
					{
					if(o.d_e_date < o.d_s_date)
						{
						alert("The end date has to be great than, or equal to the start date");
						s.SetText("");
						s.SetDate(null);
						s.ToggleDropDown();
						}
					else
						{
						timesheet.perdiem.perdiem_date_proc(null, null);
						}
					}
				else
					{
					obj.ToggleDropDown();
					}
				}
			else if(obj.GetText() != '')
				{
				var diff	= Math.round(Math.abs((o.d_e_date.getTime() - o.d_s_date.getTime())/(one_day))+1);
				var per		= perdiem_t_rate.GetText() / 1;
				perdiem_lb_calculated.SetText("$"+diff*per);
				perdiem_lb_desc.SetText('{Employee Name} - Per Diem ('+o.s_date+') to ('+o.e_date+')');
				}
			},
		 perdiem_processed: function(s,e)
			{
			if(s.cpResult !== undefined)
				{
				if(s.cpResult == "SUCCESS")
					{
					alert("Successfully saved your perdiem request");
					}
				else if(s.cpResult == "SUCCESSCLOSE")
					{
					window.opener.gv_expensereport.Refresh();
					alert("Perdiem saved, reloading page.");
					window.close();
					}
				else
					{
					alert(s.cpResult);
					}
				delete s.cpResult;
				}
			},
		save:
			function (s,e)
				{
				if(s.CauseValidation())
					{
					if(!perdiem_cb_shop.GetChecked() && perdiem_c_workorder.GetText() == '')
						{
						alert('Please either select a work order, or check shop expense');
						e.processOnServer = false;
						}
					else
						{
						cbp_new.PerformCallback('save_perdiem');
						}
					}
				},
		workorder_changed:
			function (s,e)
				{
				var wo = s.GetValue();
				perdiem_cb_shop.SetChecked(wo == 0);
				},
		shop_check_changed: function (s,e)
			{
			var chk = s.GetChecked();
			if(chk)
				{
				perdiem_c_workorder.SetValue(null);
				}
			}
		},
	expense:
		{
		reload_parent:
			function ()
				{
				alert("Expense saved, reloading page.");
				var url = location.href;
				if(window.self !== window.top)
					{
					url = window.top.location.href;//window.opener.gv_expensereport.Refresh();
					url = url.indexOf("id_expense") > -1 ? "/sections/member/expense/index.aspx" : window.top.location.href;
					}
				please_wait("start");
				window.top.location.href = url;
				}
		}
};