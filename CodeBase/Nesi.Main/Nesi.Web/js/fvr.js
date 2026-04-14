		var fvr			=	{
							gv:				null,
							new_branch_selected:
								function(s,e)
									{
									var values	= s.GetSelectedValues().join();
									if(values.indexOf("999999") != -1)
										{
										// Select All
										s.SelectAll();
										values	= s.GetSelectedValues().join();
										s.UnselectValues(["999999", "999998"]);
										lb_employees.PerformCallback(values);
										}
									else if(values.indexOf("999998") != -1)
										{
										// Deselect All
										values	= "";
										s.UnselectAll();
										lb_employees.PerformCallback(values);
										}
									else
										{
										//lb_employees.clientEnabled = true;
										//console.log(values);
										lb_employees.PerformCallback(values);
										}
									},
							new_member_selected:
								function(s,e)
									{
									var values	= s.GetSelectedValues().join();
									//alert(values);
									if(values.indexOf("999999") != -1)
										{
										// Select All
										s.SelectAll();
										values	= s.GetSelectedValues().join();
										s.UnselectValues(["999999", "999998"]);
										lb_tabs.SetEnabled(true);
										}
									else if(values.indexOf("999998") != -1)
										{
										// Deselect All
										s.UnselectAll();
										values	= "";
										lb_tabs.SetEnabled(false);
										}
									else if(values.length > 0)
										{
										//lb_employees.clientEnabled = true;
										//console.log(values);
										lb_tabs.SetEnabled(true);
										}
									else
										{
										lb_tabs.SetEnabled(false);
										}
									},
							new_tab_selected:
								function(s,e)
									{
									var values	= s.GetSelectedValues().join();
									var nh_can		= values.indexOf("5") != -1;
									var nh_mi		= values.indexOf("26") != -1;
									var nh_nc		= values.indexOf("27") != -1;
									if(	nh_can && (nh_mi || nh_nc) || (nh_mi && nh_nc) )
										{
										s.UnselectValues(["26", "27"]);
										values	= s.GetSelectedValues().join();
										cbp_content.PerformCallback("");
										}
									//alert(values);
									if(values.indexOf("999999") != -1)
										{
										// Select All
										s.SelectAll();
										values	= s.GetSelectedValues().join();
										s.UnselectValues(["999999", "999998"]);
										cbp_content.PerformCallback("");
										}
									else if(values.indexOf("999998") != -1)
										{
										// Deselect All
										s.UnselectAll();
										values	= "";
										}
									else if(values.length > 0)
										{
										cbp_content.PerformCallback("");
										}
									else
										{
										cbp_content.PerformCallback("");
										}
									},
							new_save:
								function(s,e)
									{
									var payload		=	{
														type: new_combo_type.GetValue(),
														members: lb_employees.GetSelectedValues().join(),
														categories: []
														};
									// Basic model of a category
									//	{
									//	c_id: ##,
									//	f_id: ##,
									//	is_req: true/false
									//	}
									var errors		= "";
									// Check companys
									if(lb_company.GetSelectedValues().join() == "")
										{
										errors		+= "\n- Select a branch";
										}
									// Check Members
									if(payload.members.length == 0)
										{
										errors		+= "\n- Select an employee";
										}
									if(lb_tabs.GetSelectedValues().join() == "")
										{
										errors		+= "\n- Select a category";
										}
									else
										{
										// Check that each category has a file assigned to it.
										// Get each of the h_client_id's, these are used for knowing what the suffix to client instance names, are.
										$(".h_client_id").each(function()
											{
											var this_def			= $(this).val().split("|");
											var this_cat_id			= this_def[0];
											var this_suffix			= this_def[1];
											var friendly_name		= this_def[2];
											var file_id				= eval("existing_files"+this_suffix).GetValue();
											var is_req				= eval("chkbox_needupload"+this_suffix).GetChecked();
											if(file_id == null)
												{
												errors				+= "\n- Select a file for category: ("+friendly_name+")";
												}
											else
												{
												var cat				=	{
																		c_id: this_cat_id,
																		f_id: file_id,
																		is_req: is_req
																		};
												payload.categories.push(cat);
												}
											});
										}
									// Check Tabs
									if(errors != "")
										{
										alert("Please review these errors:"+errors);
										}
									else
										{
										alert(JSON.stringify(payload));
										}
									
									},
							prev:
								function()
									{
									fvr.gv			= gv_details;
									if(fvr.gv.cpeditindex - 1 >= 0)
										{
										fvr.gv.StartEditRow(fvr.gv.cpeditindex - 1);
										}
									else
										{
										fvr.gv.StartEditRow(fvr.gv.cpmaxindex);
										}
									},
							next:
								function()
									{
									fvr.gv			= gv_details;
									if(fvr.gv.cpeditindex + 1 > fvr.gv.cpmaxindex)
										{
										fvr.gv.StartEditRow(0);
										}
									else
										{
										fvr.gv.StartEditRow(fvr.gv.cpeditindex + 1);
										}
									},
							toggle_selection:
								function(is_checked)
									{
									fvr.gv			= gv_details;
									var rowsize		= fvr.gv.pageRowSize;
									var min			= fvr.gv.pageIndex * rowsize;
									var max			= fvr.gv.pageIndex == 0 ? fvr.gv.GetVisibleRowsOnPage() : min + fvr.gv.GetVisibleRowsOnPage();
									for(var i = min; i < max; i++)
										{
										var conf			= fvr.gv.cpconfirmed[i];
										if(conf == "F")
											{
											fvr.gv.SelectRow(i, is_checked);
											}
										}
									},
							delete_selection:
									{
									run:
										function()
											{
											fvr.gv			= gv_details;
											var rowsize		= fvr.gv.pageRowSize;
											var min			= fvr.gv.pageIndex * rowsize;
											var max			= fvr.gv.pageIndex == 0 ? fvr.gv.GetVisibleRowsOnPage() : min + fvr.gv.GetVisibleRowsOnPage();
											var n			= new Array();
											for(var i = min; i < max; i++)
												{
												var id			= fvr.gv.cpids[i];
												if(fvr.gv._isRowSelected(i))
													{
													n.push(id);
													}
												}
											if(n.length > 0)
												{
												var msg		= "Are you sure you want to delete this FVR?";
												if(n.length > 1)
													{
													msg		= "Are you sure you wish to delete these "+n.length+" FVRs?";
													}
												if(confirm(msg))
													{
													PageMethods.delete_rows(n.toString(), this.complete, fvr.error, fvr.timeout);
													}
												}
											},
									complete:
										function(arg)
											{
											fvr.gv		= gv_details;
											fvr.gv.Refresh();
											}
									},
							client:
									{
									has_upload_box:
										function()
											{
											return top.frames['if_upload'] != undefined;
											},
									employee_information:
										{
										original_values: {},
										current_values: {},
										snapshot_values: function()
											{
											var current_values = {
												first_name: $("input[id$='first_name']").val(),
												middle_initial: $("input[id$='middle_initial']").val(),
												last_name: $("input[id$='last_name']").val(),
												preferred_name: $("input[id$='preferred_name']").val(),
												address: $("input[id$='address']").val(),
												city: $("input[id$='city']").val(),
												zip_code: $("input[id$='zip_code']").val(),
												prov_state: $("input[id$='prov_state']").val(),
												country: $("input[id$='country']").val(),
												phone_area: $("input[id$='phone_area']").val(),
												phone_pre: $("input[id$='phone_pre']").val(),
												phone_suffix: $("input[id$='phone_suffix']").val(),
												work_area: $("input[id$='work_area']").val(),
												work_pre: $("input[id$='work_pre']").val(),
												work_suffix: $("input[id$='work_suffix']").val(),
												extension: $("input[id$='extension']").val(),
												birthdate: $("input[id$='birthdate']").val(),
												social: $("input[id$='social']").val(),
												email_address: $("input[id$='email_address']").val(),
												work_email: $("input[id$='work_email']").val(),
												drivers_license: $("input[id$='drivers_license']").val(),
												electric_license: $("input[id$='electric_license']").val(),
												apprentice_contract: $("input[id$='apprentice_contract']").val(),
												emergency_first_name: $("input[id$='emergency_first_name']").val(),
												emergency_last_name: $("input[id$='emergency_last_name']").val(),
												emergency_phone_area: $("input[id$='emergency_phone_area']").val(),
												emergency_phone_pre: $("input[id$='emergency_phone_pre']").val(),
												emergency_phone_suffix: $("input[id$='emergency_phone_suffix']").val()
											};
											return current_values;
											},
										initial_snapshot: function(_do_delay)
											{
											if(typeof(_do_delay) !== "undefined" && _do_delay == true)
												{
												setTimeout("fvr.client.employee_information.initial_snapshot();", 250);
												}
											else
												{
												fvr.client.employee_information.original_values		= fvr.client.employee_information.snapshot_values();
												}
											},
										check_for_changes: function()
											{
											fvr.client.employee_information.current_values		= fvr.client.employee_information.snapshot_values();
											return JSON.stringify(fvr.client.employee_information.current_values) != JSON.stringify(fvr.client.employee_information.original_values);
											}
										},
									has_file:
										function()
											{
											var filename		= top.frames['if_upload'].client_upload.GetText();
											return $.trim(filename) != "";
											},
									check_for_acknowledge:
										function(obj)
											{
											if(fvr.client.has_upload_box())
												{
													alert("Please make sure to upload the filled out document before acknowledging that this has been read.");
													$(obj).removeAttr("checked");
												}
											else if(fvr.client.employee_information.check_for_changes())
												{
												alert("There have been changes since this form was loaded. \nBefore you can confirm this information is completed, you must first save your changes.");
												$(obj).removeAttr("checked");
												}
											else if(confirm("Are you sure you want to change/submit this?"))
												{
												fvr.client.do_acknowledge.run(obj);
												}
											else
												{
												$(obj).removeAttr("checked");
												}
											},
									toggle_upload_instructions:
										function(_isMobile)
											{
											var up_instructions	= $('#upload_instructions');
											if(up_instructions.is(":visible"))
												{
												up_instructions.toggle();
												}
											else
												{
												if(_isMobile)
													{
													up_instructions.toggle(0, function (){window.location.hash = 'instruction_bottom';});
													}
												else
													{
													up_instructions.toggle(0);
													}
												}
											},
									do_acknowledge:
											{
											run:
												function(obj)
													{
													if($(obj).attr("data-id") != "")
														{
														var dtl_id				= $(obj).attr("data-detail_id");
														var type				= $(obj).attr("data-type");
														var subtype				= $(obj).attr("data-subtype");
														var is_checked			= $(obj).is(":checked");
														PageMethods.do_acknowledge(dtl_id, is_checked, type, subtype, this.complete, fvr.error, fvr.timeout);
														}
													},
											complete:
												function(arg)
													{
													if(arg == "CLOSEME")
														{
													    alert("That was the last file that needed to be verified. \nThis window will now close and your browser will be refreshed.");

													    //Add a line to localstorage that FVr's are done.
													    localStorage.setItem("FVR_check", "1");

														if(opener != null)
															{
															opener.location.href = opener.location.href;
															window.close();
															}
														else if(parent.location != window.location)
															{
															parent.location.href = parent.location.href;
															}
														else
															{
															location.href = location.href;
															}
														}
													else if(arg == "Saved")
														{
														__doPostBack('bt_refresh','');
														}
													else
														{
														alert(arg);
														$(".cb").removeAttr("checked");
														}
													}
											},
									remove_file:
											{
											run:
												function(obj)
													{
													if($(obj).attr("data-id") != "")
														{
														var dtl_id				= $(obj).attr("data-id");
														PageMethods.do_remove_file(dtl_id, this.complete, fvr.error, fvr.timeout);
														}
													},
											complete:
												function(arg)
													{
													__doPostBack('bt_refresh','');
													}
											},
									load: function (id)
										{
										var url	= "/sections/hr/fvr/index.aspx?id="+id+"&is_mobile=1";
										$(".fvr_frame").attr("src", url);
										}
									},
							change_status:
									{
									run:
										function(obj)
											{
											fvr.gv			= gv_details;
											var dtl_id		= $(obj).attr('data-detail_id');
											var status_id	= $(obj).val();
											PageMethods.change_status(dtl_id, status_id, this.complete, fvr.error, fvr.timeout);
											},
									complete:
										function(arg)
											{
											fvr.gv		= gv_details;
											fvr.gv.Refresh();
											}
									},
							start_callback:
								function(s,e)
									{
									please_wait("start");
									},
							end_callback:
								function(s,e)
									{
									please_wait("stop");
									},
							timeout: 
								function(arg)
									{
									alert("Timeout occured");
									},
							error: 
								function(arg)
									{
									alert("Error has occured: " + arg._message);
									}
							}

							
		function preview(id)
			{
			if(id != null)
				{
				boing("/_tools/get_file/index.aspx?file_id="+id+"&iframe=true", preview, 768, 480); 
				}
			else
				{
				alert("Please select a file");
				}
			}		
		function remove_file(id, is_edit, is_edit_category)
			{
			if(id != null)
				{
				 if(is_edit_category && combo_ed_default_file.GetValue() == id)
						{
						alert("Before this can be removed, you must first remove this file as the default file for this category.");
						return;
						}
				if(confirm("Are you sure you want to remove this file?"))
					{
					if(is_edit && !is_edit_category)
						{
						callback_remove_edit.PerformCallback(id); 
						}
					else if(!is_edit_category)
						{
						callback_remove.PerformCallback(id); 
						}
					else if(is_edit_category)
						{
						if(combo_ed_default_file.GetValue() == id)
							{
							alert("Before this can be removed, you must first remove this file as the default file for this category.");
							return;
							}
						callback_remove_category.PerformCallback(id); 
						}
					}
				}
			else
				{
				alert("Please select a file");
				}
			}
		function setdefault(id, cat_id, fvr_type, is_edit)
			{
			if(id != null)
				{
				callback_setdefault.PerformCallback(id+"|"+cat_id+"|"+fvr_type);
				}
			else
				{
				alert("Please select a file");
				}
			}
	$("document").ready(function()
		{
		//Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
		if(typeof(Sys) !== "undefined")
			{
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			}
		});
	function EndReqHandler()
		{
		//
		}
