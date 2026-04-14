var NE_Customer	=	{
					resize:				
						function()
							{
							var width		= cust_main_panel.GetWidth();
							if(parent.resize_frame)
								{
								parent.resize_frame(width);
								}
							$(".frame").css({"width":width-500+"px"});
							//$("#cust_main_cbpanel").css({"width":width+"px"});
							},
					postal:				{
										run: 
											function()
												{

												},
										complete:	
											function(arg)
												{
												alert(arg);
												}
										},
					territory:			{
										run:		
											function()
												{
												var is_new		= new_customer_popup.IsDisplayed();
												if(is_new)
													{
													var postal		= newcustomer_postal.GetText();
													var country		= newcustomer_country.GetValue();
													if(country == "CDN")
														{
														if(postal != "")
															{
															PageMethods.chk_territory(postal, country, this.complete, NE_Customer.error, NE_Customer.timeout);
															}
														}
													}
												},
										complete:	
											function(arg)
												{
												var o = $.parseJSON(arg);
												if(o.business_unit_id != 0)
													{
													newcustomer_postal.SetText(o.postal);
													newcustomer_company.SetValue(o.business_unit_id);
													newcustomer_provstate.SetValue("ON");
													}
												}
										},
					origin_control:			{
											sales:	{
													toggle_status:	
														function()
															{
															$("#status_alert").show();
															combo_lastorigin.SetEnabled(true);
															combo_lastorigin.SetValue(1);
															btn_save.SetEnabled(false);
															},
													chk:
														function(s)
															{
															btn_save.SetEnabled(s.GetValue() != 1);
															}
													}
											},
					location:				{
											get:
												{
												run:		
													function(type, customer_id, address_id)
														{
														PageMethods.get_location(type, customer_id, address_id, this.complete, NE_Customer.error, NE_Customer.timeout);
														},
												complete:	
													function(arg)
														{
														alert(arg);
														//var o = $.parseJSON(arg);
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
					get_wo:
						function(arg)
							{
							boing("/wo_prog_frame.aspx?action=show&woprog_id="+arg, "wo", 1060, 1280);
							},
					get_quote:
						function(arg)
							{
							boing("/sections/member/quote/index.aspx?a=get_quote&quote_id="+arg, "quote", 1060, 940);
							},
					validate_email:
						function(obj, addr)
							{
							addr		= $.trim(addr);
							if (addr.match(/ /g))
								{
								var rep_addr = addr.replace(" ", "");
								obj.SetText(rep_addr);
								return this.validate_email(obj, obj.GetText());
								}
							else if (addr.match(/\;/g))
								{
								return this.validate_multiple_emails(obj, addr, ";");
								}
							else
								{
								    var regex = /[A-Z0-9._%+-]+@[A-Z0-9.-]+.[A-Z]{2,4}/igm;

								if (!regex.test(addr))
									{
									obj.SetText(obj.GetText().replace(addr, ""));
									alert("The email address '" + addr + "' was not valid, it has been removed.");
									}
								return regex.test(addr);
								}
							},
					validate_multiple_emails:
						function (obj, addr, seperator)
							{
							if (addr != '')
								{
								var result = addr.split(seperator);
								for (var i = 0; i < result.length; i++)
									{
									if (result[i] != '')
										{
										if (!this.validate_email(obj, result[i]))
											{
											return false;
											}
										}
									}
								}
							return true;
							},
						contact:
								{
								do_apply_template:
									{
									run:
										function()
											{
											var user_id			= $("#templates").parents("div:first").attr("data-user_id") / 1;
											var template_id		= priv_templates.GetValue();
											if(template_id != null && user_id != null)
												{
												PageMethods.do_apply_template(template_id, user_id, this.complete, NE_Customer.contact.error, NE_Customer.contact.timeout);
												}
											else
												{
												alert("Please select a template");
												}
											},
									complete:
										function(arg)
											{
											alert("Successfully applied template to contact");
											}
									},
								default_name:
									function(obj)
										{
										$(".t_name").val($(".name_first").val()+" "+$(".name_last").val());
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
								},
					tab_change:
						function(s,e)
							{
							var tab = right_tabs.GetActiveTab().GetText();
							NE_Customer.resize();
							var customer_id		= $('input[id*="_uc_header_hdn_customer_id"]').val();
							main_persistence_handler.Set("active_tab_index", e.tab.index);
							if(tab == "")
								{
								if(bv_data.loadingPanelElement == null)
									{
									bv_data.PerformCallback();
									}
								}
							else if(tab == "Accounting")
								{
								var rates_if		= $(".rates_if");
								var aa				= rates_if.attr("data-aa");
								if(aa == "t")
									{
									rates_if.attr("src", "./rates.aspx?customer_id="+customer_id);
									}
								}

							}
				}