
	var NE_vendor	=	{
						hid_vendor_id: 0,
						set_vendor_id:
							function()
								{
								NE_vendor.hid_vendor_id = $("[id$='hid_vendor_id']").val();
								},
						current_name_check:
								{
								run:		
									function(s,e)
										{
										NE_vendor.set_vendor_id();
										var ee			= e.htmlEvent;
										var key			= ee.keyCode ? ee.keyCode : ee.which ? ee.which : ee.charCode;
										if(s.IsValueChanged() && key != 17 && key != 16)
											{
											var current_name		= s.GetText();
											if(current_name != "")
												{
												b_save_vendor.SetEnabled(false);
												PageMethods.chk_name(current_name, NE_vendor.hid_vendor_id,  NE_vendor.current_name_check.complete, NE_vendor.error, NE_vendor.timeout);
												}
											else
												{
												b_save_vendor.SetEnabled(false);
												}
											}
										},
								complete:	
									function(arg)
										{
										if(arg)
											{
											alert("This is a direct duplicate named vendor... save is disabled");
											}
										else
											{
											b_save_vendor.SetEnabled(true);
											}
										}
								},
						timeout:	
							function(arg)
								{
								alert("Timeout occured");
								},
						error:	
							function(arg)
								{
								alert(arg._message);
								},
						save_memo:
							function(s,e)
								{
								NE_vendor.set_vendor_id();
								pc_detail.PerformCallback(t_memo.GetText()+"|"+NE_vendor.hid_vendor_id);
                                 },
                        handle_vendor_request:
                            function (obj) {
                                NE_vendor.set_vendor_id();
                                var vendor_id = NE_vendor.hid_vendor_id;
                                console.log(vendor_id);
                                if (vendor_id == null) {
                                   vendor_id = 0;
                                 }
                                 boing("/modules/request.aspx?type=2&id=" + vendor_id, "Vendor_request" + Math.random(), 500, 900);
                                },
						}
	function row_click(s, e, index)
		{
		pc_main.SetActiveTab(pc_main.GetTab(1));
		persist.Set("row_index", index);
		cbp_main.PerformCallback();
		}
	function current_name_check(s,e)
		{
		if(t_name.GetText() != "")
			{
			cbp_main.PerformCallback("name_check");
			}
		}
	function new_name_check(s,e)
		{
		if(t_new_name.GetText() != "")
			{
		//		cbp_main.PerformCallback("name_check");
			}
		}
	function current_phone_check(s, e) 
		{
		if(t_phone_area.GetText() != "" && t_phone_prefix.GetText() != "" && t_phone_suffix.GetText() != "")
				{
				cbp_main.PerformCallback("phone_check");
				}
		}
	function new_phone_check(s, e) 
		{
		if(t_new_phone_area.GetText() != "" && t_new_phone_prefix.GetText() != "" && t_new_phone_suffix.GetText() != "")
				{
		//		pop_new.PerformCallback("phone_check");
				}
		}
	function add_to_branch(from, to, id)
		{
		please_wait('begin');
		$.get("./index.aspx",
				{
				add_to_branch:		true,
				frombusiness_unit_id:		from,
				tobusiness_unit_id:		to,
				vendor_id:			id
				},
				function(ret)
					{
					if(ret == "SUCCESS")
						{
						please_wait('stop');
						gv_vendor.Refresh();
						}
					else
						{
						please_wait('stop');
						alert(ret);
						}
					});
		}