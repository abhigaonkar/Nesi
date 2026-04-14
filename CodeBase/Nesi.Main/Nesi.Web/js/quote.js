	var quote_obj	= {
		toggle_save_message: function()
			{
			var target					= $("#saved_message");
			var currently_hidden		= target.is(":hidden");
			if(currently_hidden)
				{
				target.fadeIn(500, function(){setTimeout("$('#saved_message').fadeOut(1250)", 500);});
				}
			},
		revision:
			{
			show:
				function(_id, _rev)
				{
				pop_revision.Show();
				memo_revision.Focus();
				//if(confirm('Please confirm that you would like to create a new version, doing so will kill this version.'))
				//	{
				//	location.href	= "./index.aspx?a=new_revision&quote_id="+_id+"&revision="+_rev;
				//	}
				},
			cancel:
			function(s,e)
				{
				var reason_text		= $.trim(memo_revision.GetText());
				var do_cancel		= true;
				if(reason_text != "")
					{
					do_cancel		= false;
					if(confirm("Are you sure you want to cancel?"))
						{
						do_cancel	= true;
						}
					}
				if(do_cancel)
					{
					memo_revision.SetText("");
					pop_revision.Hide();
					}
				},
			go:
			function(s,e)
				{
				var reason_text		= $.trim(memo_revision.GetText());
				if(reason_text == "")
					{
					alert("Please add a reason why you are revising this quote.");
					memo_revision.Focus();
					}
				else
					{
					if(confirm("Please verify that you wish to revise this quote"))
						{
						var base			=	{
												a:			"do_revision",
												q:			$('#this_quote_id').val(),
												r:			$('#quote_id select').val(),
												ts:			$('#quote_ts').val(),
												v:			reason_text
												}
						s.SetEnabled(false);
						$.post("/sections/member/quote/modules/quote_obj.ashx", 
							base, 
							function(r)
								{
								if(r.lastIndexOf("SUCCESS") !== 0)
									{
									alert(r);
									s.SetEnabled(true);
									}
								else
									{
									var ret			= r.split('|');
									var rev			= ret[1];
									location.href	= "./index.aspx?a=g&quote_id="+base.q+"&revision="+rev;
									}
								});
						}
					}
				}
			},
		update_order: function(arr)
			{
			var base			=	{
									a:			"update_order",
									q:			$('#this_quote_id').val(),
									r:			$('#quote_id select').val(),
									v:			arr.toString(),
									ts:			$('#quote_ts').val()
									}
			$.post("/sections/member/quote/modules/quote_obj.ashx", 
				base, 
				function(r)
					{
					if(r.lastIndexOf("SUCCESS") !== 0 && r != "InvalidState")
						{
						alert(r);
						}
					else if(r == "InvalidState")
						{
						$('#quote_connection img').attr('src', '/images/quote/decal/decal[bad_health].gif');
						$('#status_id').val("");
						lockdown(false);
						if(confirm("This quote has been edited outside of this window and will need to refresh in order to continue, would you like to refresh now?"))
							{
							location.href = location.href;
							}
						}
					else 
						{
						var ret		= r.split('|');
						var ticks	= ret[1];
						$('#quote_ts').val(ticks);
						quote_obj.toggle_save_message();
						}
					});
			},
		update_lastsent:
            function()
                {
				var quote_id			= $("#this_quote_id").val();
				var revision = $("#quote_id select").val();
				$.get("./index.aspx",
                        {
                            a: "last_sent_date",
                            quote_id: quote_id,
                            revision: revision
                        },
                        function (data) {
                            if (data == "FAILED") {
                                alert("There was a problem updating the last sent date");
                            }
                            else {
                                $("#last_fax_date > .date").html(data);
                                $("#last_fax_button").removeAttr("disabled");
								status_check();
                            }
                        });
                },
		load_quote: 
			function(id)
				{
				if(!isNaN(id))
					{
					if(id > 100000 && id.length >= 6)
						{
						if(window.opener == null)
							{
							boing('./index.aspx?a=get_quote&quote_id='+id, 'quote', 1060, 940);
							}
						else
							{
							location.href	= './index.aspx?a=g&quote_id='+id;
							}
						}
					}
				},
		update_status:
			function()
				{
				status_check();
				},
		s:	function(t, obj)
			{
			var url				= "/sections/member/quote/modules/quote_obj.ashx";
			var base			=	{
									a:			t,
									q:			$('#this_quote_id').val(),
									r:			$('#quote_id select').val(),
									v:			"",
									id:			0,
									ts:			$('#quote_ts').val()
									};
			var pl				=	{f:""};
			var zero_chk		= false;
			var blank_chk		= false;
			var refresh_parent	= false;
			switch(base.a)
				{
				case "s_customer":
					base.v		=  $(obj).attr('data-id');
					pl.f		= "check_QCed();check_active();";
					zero_chk	= true;
					blank_chk	= true;
				break;
				case "s_contact":
					base.v		= $(obj).val();
					pl.f		= "update_contact("+base.v+")";
					zero_chk	= true;
					blank_chk	= true;
				break;
				case "s_datedue":
					base.v		= $(obj).val();
					blank_chk	= true;
					break;
			    case "s_exp_podate":
			        base.v = $(obj).val();
			        blank_chk = true;
			        break;
				case "s_pctchance":
					base.v		= pct_chance.GetValue();
				break;
				case "s_pctchancereason":
					base.v		= $(obj).val();
					pl.f		= "chk_pct_reason($('#pct_chance_reason'));";
				break;
				case "s_pctchancenote":
					base.v		= $(obj).val();
				break;
				case "s_expcompletiondate":
					base.v		= $(obj).val();
					blank_chk	= true;
					zero_chk	= true;
				break;
				case "s_company":
					base.v		= $(obj).val();
					pl.f		= "fill_divisions();fill_quoters($('#quoted_company'))";
				break;
				case "s_department":
					base.v		= $(obj).val();
					pl.f		= "division_check()";
				break;
				case "s_quotedby":
					base.v		= $(obj).val();
					zero_chk	= true;
				break;
				case "s_dateprint":
					base.v			= "NOW()";
					pl.f			= "$('#lastfax_button').removeAttr('disabled');$('#received_button').removeAttr('disabled');quote_obj.update_status();";
					refresh_parent	= true;
				break;
				case "s_datesent":
					base.v			= "NOW()";
					pl.f			= "$('#last_verified_button').removeAttr('disabled');quote_obj.update_status();";
					refresh_parent	= true;
				break;
				case "s_dateverified":
					base.v			= "NOW()";
					pl.f			= "$('#received_button').removeAttr('disabled');quote_obj.update_status();";
					refresh_parent	= true;
				break;
				case "s_quotedprice":
					base.v			= $(obj).val();
					refresh_parent	= true;
				break;
				case "s_pricetype":
					base.v			= $(obj).val();
					pl.f			= base.v == "6" ? "$('#price_to').show()" : "$('#price_to').hide()";
				break;
				case "s_expvalue":
				break;
				case "s_lockquoter":
				case "s_uscurrency":
				case "s_follow_up":
				case "s_inflationterm":
				case "s_includetitle":
					base.v			= $(obj).is(':checked') ? 1 : 0;
				break;
				case "s_jobdescription":
				case "s_custspecdoc":
				case "s_quotedpriceto":
				case "s_pctdown":
				case "s_netdue":
				case "s_customterm":
					base.v			= $(obj).val();
				break;
				case "s_addressid":
					base.v			= $(obj).val();
					pl.f			= "update_customer($('#customer').attr('data-id'), "+base.v+")";
				break;
				case "s_notesrow":
				case "s_detailrow":
					base.v			= $(obj).val();
					base.id			= $(obj).parents("div:first").attr("data-row_id");
				break;
				}
			if(base.a != "" && $(obj).attr("data-ov") != base.v)
				{
				var do_save		= true;
				if(blank_chk && base.v == "")
					{
					do_save		= false;
					$(obj).addClass("is_error");
					// Add error checking
					}
				if(zero_chk && parseInt(base.v) == 0)
					{
					do_save		= false;
					$(obj).addClass("is_error");
					// Add error checking
					}
				if(do_save)
					{
					$.ajax(
						{
						type:"POST",
						dataType: "text",
						url: url, 
						data: base, 
						beforeSend: 
							function()
								{
								$(obj).addClass("is_processing");
								},
						success:
							function(r)
								{
								if(r.lastIndexOf("SUCCESS") !== 0 && r != "InvalidState")
									{
									alert(r);
									$(obj).addClass("is_error");
									}
								else if(r == "InvalidState")
									{
									$('#quote_connection img').attr('src', '/images/quote/decal/decal[bad_health].gif');
									$('#status_id').val("");
									lockdown(false);
									$(obj).addClass("is_error");
									if(confirm("This quote has been edited outside of this window and will need to refresh in order to continue, would you like to refresh now?"))
										{
										location.href = location.href;
										}
									}
								else 
									{
									var ret		= r.split('|');
									var ticks	= ret[1];
									$('#quote_ts').val(ticks);
									if(pl.f != "")
										{
										eval(pl.f);
										}
									$(obj).removeClass("is_error");
									}
								},
						complete: 
							function()
								{
								$(obj).removeClass("is_processing");
								$(obj).attr("data-ov", base.v);
								if(!$(obj).hasClass("is_error"))
									{
									if(refresh_parent)
										{
										if (window.opener != null && window.opener.location.href.match(/sections\/member\/quote/g))
											{
											window.opener.location.href = window.opener.location.href;
											}
										}
									quote_obj.toggle_save_message();
									}
								},
						error: 
							function()
								{
								$(obj).addClass("is_error");
								}
						});
					}
				}
			else
				{
				$(obj).removeClass("is_error");
				}
			},
		resize_textboxes: function(_type)
			{
			var _target		= _type == 0 ? "#quote_detail" : "#quote_notes";
			$(_target).find("textarea").each(function()
				{
				var is_expanded = $(this).attr("data-original_height") != null;
				if(!is_expanded)
					{
					$(this).attr("data-original_height", $(this).css("height"));
					$(this).css("height", this.scrollHeight+"px");
					}
				else
					{
					$(this).css("height", $(this).attr("data-original_height"));
					$(this).removeAttr("data-original_height");
					}
				});
			}

		};
			function toggle_lock_quoted_by(obj)
				{
				var is_locked			= $(obj).is(":checked");
				var is_new_quote		= $(obj).attr("data-is_new_quote") == "true";
				var quoted_by_box		= is_new_quote ? "#newquote_quotedby" : "#quoted_by";
				var is_disabled			= $(quoted_by_box).is(":disabled");
				if($(quoted_by_box).val() != "0" || is_disabled)
					{
					if(is_locked)
						{
						$(quoted_by_box).attr("disabled", true);
						}
					else
						{
						$(quoted_by_box).removeAttr("disabled");
						if(!is_new_quote)
							{
							fill_quoters($('#quoted_company'));
							}
						}
					if(!is_new_quote)
						{
						quote_obj.s("s_lockquoter", obj);
						}
					}
				else
					{
					$(obj).removeAttr('checked');
					alert("You must choose a quoter before you can lock into them.");
					}
				}
			function do_static_functions()
				{
				check_QCed();
				check_active();
				//setInterval('status_check()', 5000);
				if($('#show_help').is(':checked'))
					{
					$('.c, .h').each(function()
									{
									$(this).tip();
									});
					}
				/*
				$('#add_worksheet_row').find('input:text').each(function()
					{
					$(this).blur(function()
								{
								$(this).css({'background-color':'#cfc'});
								});
					if($(this).attr('class') == 'description')
						{
						$(this).keydown(function(ev)
							{
							var val		= $(this).val();
							var e		= ev.which;
							if(e == 40)
								{
								$(this).inventory();
								}
							if(e == 13)
								{
								$(this).inventory();
								}
							var _parent		= $(this).parent().parent();
							if((e < 95 || e > 106) && (e < 47 || e > 58) && (e != 0 && e != 8 && e != 9 && e != 13 && e != 16 && e != 17 && e != 18 && e != 40 && e != 46))
								{
								_parent.find('.part_n').val('');
								_parent.find('.sellprice').val('');
								_parent.find('b').text('$0.00');
								_parent.find('i').text('0.00');
								}
							});
						}
					});
				*/
				$('#copy_pane').dialog(
					{
					autoOpen: false,
					resizable: false,
					modal:true,
					title: 'Copy from Other Quotes',
					draggable: false,
					width: 500
					});
				$('#quote_id input:text').keydown(function(e)
						{
						if(e.keyCode == 13)
							{
							quote_obj.load_quote($(this).val());
							e.preventDefault();
							}
						});
				$('#open_quotes').dialog(
					{
					autoOpen:	false,
					resizable:	false,
					modal:		true,
					draggable:	false,
					title:		'OPEN QUOTES',
					width:		850,
					height:		450
					});
				$('#new_price_pane').dialog(
					{
					autoOpen:	false,
					resizable:	false,
					modal:		true,
					draggable:	false,
					closeOnEscape: false,
					open:		function(event, ui)
									{
									$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
									$(this).find('input:text').focus();
									},
					close:		function(event, ui)
									{
									_sellprice_parent_obj.focus();
									},
					title:		'PLEASE ADD COST PRICE',
					width:		550,
					height:		150
					});
				var tmpDate = new Date();
				var dateToday = new Date(tmpDate.getTime() - 7*24*60*60*1000 );
				$('#completion_date').datepicker({dateFormat:'yy-mm-dd', minDate: dateToday});
				$('#date_due').datepicker({ dateFormat: 'yy-mm-dd', minDate: dateToday });
				$('#exp_podate').datepicker({ dateFormat: 'yy-mm-dd', minDate: dateToday });
				$('#schedule_date').datepicker({ dateFormat: 'yy-mm-dd', minDate: dateToday });
				$('#print_pane').dialog(
					{
					autoOpen:	false,
					resizable:	true,
					modal:		true,
					title:		'PRINT PANE',
					close:		function()
									{
									$(this).children('iframe').attr('src', 'about:blank');
									if (window.opener != null && window.opener.location.href.match(/sections\/member\/quote/g))
										{
										window.opener.location.href = window.opener.location.href;;
										}
									lockdown(false);
                                    quote_obj.update_lastsent();
									},
					draggable:	true,
					width: 770,
					height:800
					});
								
				$('#quote_addcontact').dialog(
					{
					autoOpen:	false,
					title:		'ADD CONTACT',
					resizable:	false,
					modal:		true,
					draggable:	false,
					width:		450
					});
								
				$('#search_pane').dialog(
					{
					autoOpen:	false,
					resizable:	false,
					draggable:	false,
					modal:		true,
					title:		'Quote Search',
					width:		1200,
					height:		800
					});
								
				$('#kill_pane').dialog(
					{
					autoOpen:	false,
					resizable:	false,
					modal:		true,
					draggable:	false,
					title:		'KILL QUOTE QUESTIONNAIRE',
					width:		600
					});
				$('#kill_pane input:text').each(
					function(i)
						{
						var url					= './index.aspx?a=ac_competitors';
						$(this).autocomplete(url,
							{
							minChars:		1,
							delay:			0,
							autoFill:		false,
							matchSubset:	1,
							matchContains:	0,
							maxItemsToShow:	100,
							cacheLength:	100,
							width:			300,
							formatResult:
								function(data, value)
									{
									return value;
									},
							selectOnly:		1
							}).result(
								function(event, item)
									{
									$('#competitor_id').val(item[1]);
									});
						/*
						$('.v .customer').result(
						function(event, data, formatted)
						{
						$('#customer_id').val(data[1]);
						update_customer(data[1]);
						});
						*/
												

						});	
				$('#quote_detail .entries ul').sortable(
												{
												update:			function()
																	{
																	var tab			= $(this).parents('div').attr('id');
																	
																	
																	    reorder_tab(tab);
																	    save_quote(false, false);
																	
																	},
												helperclass:	'sortablehelper',
												activeclass:	'sortableactive',
												tolerance:		'pointer'
												});
				$('#quote_notes .entries ul').sortable(
												{
												update:			function()
																	{
																	var tab			= $(this).parents('div').attr('id');
																	reorder_tab(tab);
																	save_quote(false,false);
																	},
												helperclass:	'sortablehelper',
												activeclass:	'sortableactive',
												tolerance:		'pointer'
												});
				load_followup_history();
				division_check();
				setTimeout("lockdown()", 100);
				}
			function check_QCed()
				{
				var prevButtonEnabled = false;			
				var prevButtonClick = "";
				$.ajax(	{
						type: "GET",
						url: "./index.aspx",
						dataType: "text",
						data:	{
								a:			"is_QCed",
								cust_id:	$('#customer').attr('data-id')
								},
						beforeSend: function()
							{
							prevButtonClick = $("#received_button").attr("onclick");
							$("#received_button").removeAttr("onclick");
							prevButtonEnabled = $("#received_button").is(":enabled");
							$("#received_button").attr("disabled", "disabled");
							},
						success: function(_val)
							{
							if(_val != 'True')
								{
								$("#is_QCed").attr("src", "/images/quote/decal/decal[notqc].png").attr("title", "Customer has NOT been QC'ed. Limitations will be put in place");
								}
							else
								{
								$("#is_QCed").attr("src", "/images/quote/decal/decal[isqc].png").attr("title", "Customer has been QC'ed.");
								}
							},
						complete: function()
							{
							if(prevButtonEnabled)
								{
								$("#received_button").removeAttr("disabled");
								}
							
							$("#received_button").attr("onclick",prevButtonClick);
							},
						error: function()
							{
							alert("There was an issue retrieving the customer's QC status");
							}
					});
	
				}
			function update_date(obj, t)
				{
				if($('#completion_date').val() == '')
					{
					alert('You must fill out the completion date before you are able to use this quote');
					}
				else
					{
					var date = new Date();
					var yyyy = date.getFullYear();
					var mm = date.getMonth() + 1;
					mm < 10?mm = '0'+mm:mm;
					var dd = date.getDate();
					dd < 10?dd = '0'+dd:dd;
					var hh = date.getHours();
					hh < 10?hh = '0'+hh:hh;
					var min = date.getMinutes();
					min < 10?min = '0'+min:min;
					var ss = date.getSeconds();
					ss < 10?ss = '0'+ss:ss;
					var mysqlDateTime = yyyy + '-' + mm + '-' + dd + ' ' + hh + ':' + min + ':' + ss;
					switch(t)
						{
						case 0:
							quote_obj.s("s_dateprint", obj);
						break;
						case 1:
							quote_obj.s("s_datesent", obj);
						break;
						case 2:
							quote_obj.s("s_dateverified", obj);
						break;
						}
					$(obj).parents("tr:first").find(".date").text(mysqlDateTime);
					}
				}
			function update_exp_val(obj)
				{
				var parent		= $(obj).parents("td:first");
				var inc_exp_val	= $("#new_expected_value").val().trim() == "" ? 0 : $("#new_expected_value").val() / 1;
				if(inc_exp_val == 0)
					{
					alert("Please supply an expected value");
					$("#new_expected_value").focus();
					}
				else
					{
					var query_string				=	{
														a:			"up_exp_val",
														quote_id:	$('#this_quote_id').val(),
														revision:	$('#quote_id select').val(),
														exp_val:	inc_exp_val
														}
					$.get("index.aspx", query_string, 
						function(j)
							{
							if(j == "SUCCESS")
								{
								parent.text("$"+inc_exp_val.toFixed(2));
								}
							else if(j == "FAILED")
								{
								location.href = location.href;
								}
							});
					}
				}
			function stop_quote(this_quote_id, this_revision, redir_quote_id, redir_revision, is_new)
				{
				if(is_new == undefined)
					{
					if(confirm("If the current quote ("+this_quote_id+" v"+this_revision+") is yours and you click this link it will clear it and redirect you to quote # "+redir_quote_id+" v"+redir_revision+".\n\nIf the current quote isn't yours it will just redirect you to the selected quote.\n\nPlease confirm this is what you want done."))
						{
						location.href='./index.aspx?a=stop_redir&this_quote_id='+this_quote_id+'&this_revision='+this_revision+'&redir_quote_id='+redir_quote_id+'&redir_revision='+redir_revision;
						return true;
						}
					else
						{
						return false;
						}
					}
				else
					{
					location.href='./index.aspx?a=g&quote_id='+redir_quote_id+'&revision='+redir_revision;;
					return true;
					}
				}
			function chk_pct_reason(obj)
				{
				if($(obj).val() == 6)
					{
					$("#pct_chance_note").slideDown("fast").focus();
					}
				else
					{
					$("#pct_chance_note").slideUp("fast");
					}
				}
			function duplicate_quote(id, rev, obj)
				{
				$(obj).attr('disabled', true);
				setTimeout("alert('Duplicate quote created, redirecting...');location.href='./index.aspx?a=duplicate_quote&quote_id="+id+"&revision="+rev+"';", 500);
				}
			function update_header()
				{
				var query_string				=	{
													a:			"uh",
													quote_id:	$('#this_quote_id').val(),
													revision:	$('#quote_id select').val()
													}
				$.get("index.aspx", query_string, 
					function(j)
						{
						var o			= $.parseJSON(j);
						if(o.status != "error")
							{
							$("#tm_price").text(o.tm_pricing);
							$("#worksheet_price").text(o.worksheet_total);
							$("#tm_total_label").text(o.tm_pricing);
							$("#worksheet_total_label").text(o.worksheet_total);
							$('#quote_ts').val(o.ticks);
							}
						});
				query_string.a					= "ug";
				$.get("index.aspx", query_string, 
					function(j)
						{
						var o			= $.parseJSON(j);
						for(var i = 0; i < o.length; i++)
							{
							var s		= o[i];
							$("#s_"+s.id).text("Section Total: "+s.total);
							}
						});
				};
			function set_active_revision(obj)
				{
				$(obj).attr('disabled', true);
				var q					=	{
											a:			"set_active_revision",
											quote_id:	$('#this_quote_id').val(),
											revision:	$('#quote_id select').val()
											}
				$.get("index.aspx", q, function(resp)
											{
											if(resp != "SUCCESS")
												{
												alert(resp);
												$(obj).removeAttr('disabled');
												$(obj).removeAttr('checked');
												}
											else
												{
												location.href = location.href;
												}
											});
				}
			function check_open(customer_id, is_new)
				{
				var this_quote_id		= $('#this_quote_id').val();
				var this_revision		= $('#quote_id select').val();
				var customer_name		= "";
				var to_append			= "";
				var _url				= "";
				if(is_new == undefined)
					{
					_url				= "./index.aspx?a=xml_openquotes&quote_id="+this_quote_id+"&customer_id="+customer_id;
					customer_name		= $('#customer').val();
					}
				else
					{
					_url				= "./index.aspx?a=xml_openquotes&customer_id="+customer_id;
					customer_name		= $('#newquote_customer').val();
					}
				$.ajax(
						{
						type:		'GET',
						url:		_url,
						dataType:	'xml',
						beforeSend:	function()
										{
										
										},
						success:	function(xml)
										{
										if($(xml).find('quote').size() > 0)
											{
											to_append					= "<div align='center'  style='overflow-y:scroll;max-height:300px;'>These quotes are currently open for <b style='color:#09c;'>"+customer_name+"</b> <br />\
											Please ensure that they haven't already contacted us for a quote on this job.<br /> <br /> \
											<table style='font-size:11px;' width='800' cellspacing='0'>\
												<thead style='color:#fff;background-color:#000;'>\
													<tr>\
														<th>Quote #</th>\
														<th>Branch</th>\
														<th>PM</th>\
														<th>Date Printed</th>\
														<th>Job Description</th>\
													</tr>\
												</thead>\
												<tbody>";
											$(xml).find('quote').each(function()
																		{
																		var quote_id			= $(this).find('quote_id').text();
																		var revision			= $(this).find('revision').text();
																		var business_unit_id			= $(this).find('business_unit_id').text();
																		var name		= $(this).find('name').text();
																		var member_id			= $(this).find('member_id').text();
																		var member_name			= $(this).find('member_name').text();
																		var last_print_date		= $(this).find('last_print_date').text();
																		if(last_print_date == "")
																			{
																			last_print_date		= "--";
																			}
																		var job_description		= $(this).find('job_description').text();
																		if(is_new == undefined)
																			{
																			to_append				+= "<tr onclick='stop_quote("+this_quote_id+","+this_revision+","+quote_id+","+revision+");' onmouseover=\"$(this).css({'background-color':'#09c', 'color':'#fff'});\" onmouseout=\"$(this).css({'background-color':'#fff', 'color':'#000'});\" style='cursor:pointer;'>\
																										<td align='center'><b>"+quote_id+" v"+revision+"</b></td>\
																										<td align='center'>"+name+"</td>\
																										<td align='center'>"+member_name+"</td>\
																										<td align='center'>"+last_print_date+"</td>\
																										<td align='left'>"+job_description+"</td>\
																									</tr>";
																			}
																		else
																			{
																			to_append				+= "<tr onclick='stop_quote("+this_quote_id+","+this_revision+","+quote_id+","+revision+", true);' onmouseover=\"$(this).css({'background-color':'#09c', 'color':'#fff'});\" onmouseout=\"$(this).css({'background-color':'#fff', 'color':'#000'});\" style='cursor:pointer;'>\
																										<td align='center' style='border-right:solid 1px #000;'><b>"+quote_id+" v"+revision+"</b></td>\
																										<td align='center' style='border-right:solid 1px #000;'>"+name+"</td>\
																										<td align='center' style='border-right:solid 1px #000;'>"+member_name+"</td>\
																										<td align='center' style='border-right:solid 1px #000;'>"+last_print_date+"</td>\
																										<td align='left'>"+job_description+"</td>\
																									</tr>";
																			}
																		});
											to_append					+= "</table></div>";
											}
										else
											{
											if(is_new == undefined)
												{
												return false;
												}
											else
												{
												to_append					= "<center style='margin:10px;font-size:11px;font-weight:bold;'>No current quotes for this customer</center>";
												}
											}
										},
						complete:	function()
										{
										if(is_new == undefined && to_append != "")
											{
											$('#open_quotes').html(to_append).dialog('open');
											}
										else
											{
											$('#newquote_openquotes').html(to_append);
											}
										},
						error:		function()
										{
										}
						});
				}

			function check_active()
				{
				$.get("./index.aspx",
						{
						a:			"is_active",
						cust_id:	$('#customer').attr('data-id')
						},
						function(on_hold)
							{
							if(on_hold == 'False')
								{
								$("#is_active").attr("src", "/images/quote/decal/decal[active].png").attr("title", "Customer is active.");
								$("link[media='screen']").attr("href", "/css/quote.css?ts"+Math.random());
								$('.contact_info .header img').attr('src', '/images/quote/button/button[editcustomer].png');
								$("#netdue").removeAttr("disabled");
								$("#percentdown").removeAttr("disabled");
								}
							else
								{
								$("#is_active").attr("src", "/images/quote/decal/decal[onhold].png").attr("title", "Customer is on hold");								
								$("link[media='screen']").attr("href", "/css/quote_hold.css?ts"+Math.random());
								$('.contact_info .header img').attr('src', '/images/quote/button/button[editcustomer-hold].png');
								var current_netdue		= $(".netdue").val();
								var current_pctdown		= $(".percentdown").val();
								$(".netdue").val(5).attr("disabled", true);
								$(".percentdown").val(100).attr("disabled", true);
								if(current_netdue != 5 || current_pctdown != 100)
									{
									quote_obj.s('s_netdue', $(".netdue"));
									quote_obj.s('s_pctdown', $(".percentdown"));
									}
								}
							});
				}

			function new_wo()
				{
				var business_unit_id			= $("#quoted_company").val();
				var quoteid = $('#this_quote_id').val() ;
				var qq = quoteid.concat($('#quote_id select').val());
				var is_qced = $("#is_QCed").attr("src").match(/isqc/g) != null;
				if(!is_qced)
					{
					alert("A work order cannot be cut for this customer - They haven't been QC'ed");
					}
				else 
					{
					if(window.opener.window)
						{
							//window.opener.window.document.location = "/sections/workorder/index.aspx?woprog_id=0&business_unit_id="+business_unit_id;
							boing("/sections/workorder/index.aspx?woprog_id=0&business_unit_id=" + business_unit_id + "&fromquoteid=" +  qq, "WO", 1024, 700);
						}
					else
						{
							boing("/sections/workorder/index.aspx?woprog_id=0&business_unit_id=" + business_unit_id + "&fromquoteid=" + qq, "WO", 1024, 700);
						}
					window.close();
					}
				/*
											function()
												{
												window.opener.$("#ctl00_cphMasterBody_btnAdd").click();
												window.opener.document.focus();
												});*/
				}

			function update_available_addresses(cust_id)
				{
				var url						= "./index.aspx?a=xml_addresses&customer_id="+cust_id;
				var _options				= "";
				$.ajax(
						{
						type:		'GET',
						url:		url,
						dataType:	'xml',
						beforeSend: function()
										{
										$("#address_id").html("");
										},
						success:	function(xml)
										{
										$(xml).find('address').each(
											function()
												{
												var id				= $(this).find('id').text();
												var type			= $(this).find('type').text();
												var addr			= $(this).find('addr').text();
												_options			+= "<option value='"+id+"'>"+type+" - "+addr+"</option>";
												});
										},
						complete:	function()
										{
										$("#address_id").html(_options);
										}
						}
					   );
				}

			function update_customer(cust_id, address_id)
				{
				var url						= address_id == undefined ? './index.aspx?a=xml_customerinfo&cust_id='+cust_id : './index.aspx?a=xml_customerinfo&cust_id='+cust_id+'&address_id='+address_id;
				$.ajax(
						{
						type:		'GET',
						url:		url,
						dataType:	'xml',
						beforeSend: function()
										{
										please_wait("start");
										},
						success:	function(xml)
										{
										$(xml).find('info').each(
											function(){
												var address_1		= $(this).attr('address_1');
												var address_2		= $(this).attr('address_2');
												var address_3		= $(this).attr('address_3');
												var address_4		= $(this).attr('address_4');
												var city			= $(this).attr('city');
												var state			= $(this).attr('state');
												var postal			= $(this).attr('postal');
												var country			= $(this).attr('country');
												var phone			= $(this).attr('phone');
												var fax				= $(this).attr('fax');
												$("#_address_1").html(address_1);
												$("#_address_2").html(address_2);
												$("#_address_3").html(address_3);
												$("#_address_4").html(address_4);
												$("#_city").html(city);
												$("#_state").html(state);
												$("#_postal").html(postal);
												$("#_country").html(country);
												$("#_phone").html(phone);
												$("#_fax").html(fax);
												$("#_mobile").html("");
												$("#_email").html("");
												$("#_extension").html("");
												});
										},
						complete:	function()
										{
										if(address_id == undefined)
											{
											update_available_addresses(cust_id)
											}
										please_wait("stop");
										}
						}
					   );
				}

			function follow_up_window()
				{
				load_followup_history();
				}

			function add_detail(which, obj)
				{
				$('#quote_connection img').attr('src', '/images/quote/decal/decal[connection].gif');
				var _type				= which	== 'detail' ? 1 : 2;
				var _defs				=	{
											a:				"new_detail",
											quote_id:		$('#this_quote_id').val(),
											revision:		$('#quote_id select').val(),
											type:			_type
											};
				$.get("./index.aspx", 
						_defs, 
						function(returned)
							{
							if(returned.match(/^\d+,\d+$/g))
								{
								var returned_ids			= returned.split(",");
								var which_id				= '#quote_'+which;
								var count					= $(which_id+' .entries ul').find('div').size() + 1;
								var worksheet_button		= _type == 1 && returned_ids[1] != 0 ? "<button type='button' onclick='get_section(this,"+returned_ids[1]+")' id='s_"+returned_ids[1]+"'>Section Total: $0.00</button>" : "";
								var div		= "\
	<li>\
		<div data-row_id='"+returned_ids[0]+"'>\
			<table cellpadding='0' cellspacing='0' width='100%'>\
					<tr>\
						<td class='c' align='center'><span id='row_n'>"+count+"</span></td>\
						<td class='t'><textarea onload='_resize(this)' onfocus='expand(this)' onchange=\"quote_obj.s('s_" + which + "row', this);\"></textarea></td>\
						<td class='action'>\
							<button type='button' onclick=\"this.disabled=true;del_extratext(this, '"+which+"');\">X</button>"+worksheet_button+"\
						</td>\
					</tr>\
			</table>\
		</div>\
	</li>";
								$(which_id+' .entries ul').append(div);
								$(which_id+' .entries ul').find('textarea:last').focus();
								}
							else if(data == "EXISTS")
								{
								alert("Could not delete this detail, it is linked to a section which contains parts.\n You must first delete the parts before this detail can be deleted.");
								}
							else
								{
								alert("Could not create new row due to a server issue");
								}
							$('#quote_connection img').attr('src', '/images/quote/decal/decal[good_health].gif');
							$(obj).removeAttr('disabled');
							});
				}

		function get_section(obj,section_id)
			{
			var this_parent			= $(obj).parents('div:first');
			var this_quote_id		= $('#this_quote_id').val();
			var this_revision		= $('#quote_id select').val();
			var this_detail_text	= $.trim(this_parent.find("textarea").val());
			if(this_detail_text.length == 0)
				{
				alert("Text needs to be added to this detail before it's useable as a section in the worksheet.");
				}
			else
				{
				var url		= "/sections/member/picklist/pikclist.aspx?id="+this_quote_id+"&rev="+this_revision+"&origin=quote&section_id="+section_id;
				boing(url, "WorkSheetPickList", 975, 800);
				}
			}

		function del_extratext(obj, which)
			{
			var _type			= which	== 'detail' ? 1 : 2;
			var _type_string	= _type == 1 ? "detail" : "note";
			$('#quote_connection img').attr('src', '/images/quote/decal/decal[connection].gif');
			var _div			= $(obj).parents("div:first");
			var _defs			=	{
									a:			"del_extratext",
									id:			_div.attr('data-row_id'),
									del_assoc:	false
									};
			if($(_div).parents('li').find('button').size() == 2)
				{
				if(!confirm("Please confirm that you want to delete this "+_type_string))
					{
					$(obj).removeAttr('disabled');
					return;
					}
				else if(_type == 1)
					{
					if(confirm("Would you also like to delete the worksheet items & the associated section also?"))
						{
						_defs.del_assoc			= true;
						}
					}
				}
			$.get("./index.aspx", _defs, function(returned)
											{
											if(returned == "SUCCESS")
												{
												$(_div).parents('li').fadeOut(10, function()
																					{
																					$(this).remove();
																					reorder_tab("quote_"+which);
																					});
												}
											else
												{
												alert(returned);
												$(obj).removeAttr('disabled');
												}
											$('#quote_connection img').attr('src', '/images/quote/decal/decal[good_health].gif');
											});
			}

			function update_contact(contact_id)
				{
				var url						= './index.aspx?a=xml_contactinfo&contact_id='+contact_id;
				$.ajax(
						{
						type:		'GET',
						url:		url,
						dataType:	'xml',
						success:	function(xml)
										{
										$(xml).find('info').each(
											function(){
												var mobile			= $(this).attr('mobile');
												var email			= $(this).attr('email');
												var extension		= $(this).attr('extension');
												$("#_mobile").html(mobile);
												$("#_email").html("<a href='"+email+"'>"+email+"</a>");
												$("#print_pane input").val(email);
												$("#_extension").html(extension);
												});
										}
						}
					   );
				}

			function update_contacts(cust_id, obj)
				{
				var _contacts			= "";
				var _disabled			= false;
				if(obj == undefined)
					{
					$.ajax(
							{
							type:		'GET',
							url:		'./index.aspx?a=xml_contacts&cust_id='+cust_id,
							dataType:	'xml',
							beforeSend:	function()
											{
											$('.v .contact').empty();
											$('.v .contact').append('<option value=0 selected>Loading Contacts...</option>');
											$('.v .contact').attr('disabled', true);
											},
							complete:	function()
											{
											if(!_disabled)
												{
												$('.v .contact').removeAttr('disabled');
												}
											$('.v .contact').html(_contacts);
											},
							success:	function(xml)
											{
											$('.v .contact').empty();
											_contacts						+='<option value=0 selected>Please Select Contact</option>';
											$(xml).find('contact').each(
												function(){
													var this_id				= $(this).attr('id');
													var this_name			= $(this).attr('name');
													_contacts				+= "<option value="+this_id+">"+this_name+"</option>";
													});
											},
							error:		function(XMLHttpRequest, textStatus, errorThrown)
											{
											_contacts				= '<option value=0 selected>No Contacts Set - Please Add &gt;</option>';
											_disabled				= true;
											}
							}
						   );
					}
				else
					{
					var _select			= $('#newquote_contact');
					$.ajax(
							{
							type:		'GET',
							url:		'./index.aspx?a=xml_contacts&cust_id='+cust_id,
							dataType:	'xml',
							beforeSend:	function()
											{
											_select.empty();
											_select.append('<option value=0 selected>Loading Contacts...</option>');
											_select.attr('disabled', true);
											},
							success:	function(xml)
											{
											_select.empty();
											_contacts						+='<option value=0 selected>Please Select Contact</option>';
											$(xml).find('contact').each(
												function(){
													var this_id				= $(this).attr('id');
													var this_name			= $(this).attr('name');
													_contacts				+= "<option value="+this_id+">"+this_name+"</option>";
													});
											},
							complete:	function()
											{
											if(!_disabled)
												{
												_select.removeAttr('disabled');
												}
											_select.html(_contacts);
											},
							error:		function(XMLHttpRequest, textStatus, errorThrown)
											{
											_contacts				= '<option value=0 selected>No Contacts Set - Please Add &gt;</option>';
											_disabled				= true;
											}
							}
						   );
					}
				}

			function _resize(obj)
			{
	//		    $(this).css('height', '100px');
			}

			function expand(obj)
				{	
				}
			function customer_edit(obj)
				{
				var _customer_id		= $('#customer').attr('data-id');
				if(_customer_id != "")
					{
					var _url					= "/sections/customer/index.aspx?customer_id="+_customer_id;
					var _page_name				= "EditCustomer";
					var _width					= "1024";
					var _height					= "550";
					var _editwindow				= boing(_url, _page_name, _width, _height);
					}
				else
					{
					return false;
					}
				}
			function reorder_tab(tab)
				{
				var n_div				= $('#'+tab+' .entries ul').find('li > div').size();
				var curr_div_n			= 0;
				var this_arr			= [];
				$('#'+tab+' .entries ul').find('li > div').each(function()
														{
														var row_id		= $(this).attr("data-row_id");
														$(this).find('#row_n').text(curr_div_n+1);
														this_arr.push(row_id+"|"+curr_div_n);
														curr_div_n++;
														});
				quote_obj.update_order(this_arr);
				}
			function toggle_detail()
				{
				$('#quote_information').toggle('slide',{ direction: 'right' });
				}
			function format_number(obj)
				{
				if($(obj).val() != "" && !isNaN($(obj).val()))
					{
					var _val		= $(obj).val() / 1;
					$(obj).val(_val.toFixed(2));
					}
				}
			function gatekeeper_startquote(obj)
				{
				var submit_button			= $('#newquote_submit');
				var q						=	{
												a:					"start_quote",
												customer_id:		$('#newquote_customer').attr('data-id'),
												contact_id:			$('#newquote_contact').val(),
												date_due:			$('#newquote_datedue').val(),
												job_description:	$('#newquote_description').val(),
												business_unit_id:			$('#newquote_branch').val(),
												lock_quoted_by:		$('#lock_quoter').is(':checked'),
												division_id:		$('#newquote_division').val(),
												quoted_by:			$('#newquote_quotedby').val(),
												estimated_value:	$('#newquote_value').val(),
												date_completion: $('#newquote_completedate').val(),
				                                exp_podate:$('#newquote_exp_podate'.val())
												};
				if(
					q.customer_id != "" && 
					q.customer_id != "0" && 
					q.contact_id != "" && 
					q.contact_id != "0" && 
					q.date_due != "" &&
                    q.exp_podate != "" &&
					q.job_description != "" &&  
					q.business_unit_id != "0" && 
					q.quoted_by != "0" &&
					q.estimated_value != "0" &&
					q.estimated_value != "" &&
					q.date_completion != ""
					)
					{
					submit_button.removeAttr('disabled');
					if(obj != undefined)
						{
						    submit_button.attr('disabled', true);

						    boing("index.aspx?" + $.param(q, true), "quote_count", 1050, 920);
						    window.opener.location.href = window.opener.location.href;
//						location.href	=	"./index.aspx?"+$.param(q, true);
						}
					}
				else
					{
					submit_button.attr('disabled', true);
					}
				}
			function fill_quoters(obj)
				{
				var is_search				= $(obj).attr("class") == "subquotedby_company";
				var target					= is_search ? $(".subquotedby_name") : $('#quoted_by');
				var quoters					= is_search ? "<option value='0' selected>All</option>" : "<option value='0'>Choose Quoter</option>";
				var business_unit_id				= $(obj).val();
				if(business_unit_id != 0 && is_search || (!is_search && business_unit_id != 0 && !$('#lock_quoter').is(':checked')))
					{
					var vars			=	{
											a:			"xml_quoters",
											business_unit_id: business_unit_id
											}
					$.ajax(
							{
							type:		'GET',
							url:		'./index.aspx',
							data:		vars,
							dataType:	'xml',
							beforeSend:	function()
											{
											target.empty();
											target.addClass("is_processing");
											},
							success:	function(xml)
											{
											$(xml).find('member').each(
												function()
													{
													var member_id			= $(this).find('id').text();
													var member_name			= $(this).find('name').text();
													quoters					+= "<option value='"+member_id+"'>"+member_name+"</option>";
													});
											},
							complete:
								function()
									{
									target.append(quoters);
									target.removeClass("is_processing");
									},
							error:
								function()
									{
									alert("There was an error retrieving quoters");
									}
							}
							);
					}
						
				}


			function fill_divisions()
				{
				var obj				= $("#division");
				var options			= "";
				$.ajax(
						{
						type:		'GET',
						url:		'./index.aspx?a=xml_divisions&business_unit_id='+$("#quoted_company").val(),
						dataType:	'xml',
						beforeSend:	function()
										{
										$(obj).addClass("is_processing");
										$(obj).empty();
										},
						success:	function(xml)
										{
										$(xml).find('division').each(
											function()
												{
												var def_division	= $(obj).attr("data-default_division");
												var _id				= $(this).attr('id');
												var _name			= unescape_me($(this).attr('name'));
												var selected_text	= def_division == _id ? "selected" : "";
												options				+= "<option value='"+_id+"' "+selected_text+">"+_name+"</option>";
												});
										},
						complete:
							function()
								{
								$(obj).append(options);
								$(obj).removeClass("is_processing");
								}
						}
					   );
						
				}

			function clear_lineitem(obj)
				{
				var _parent				= $(obj).parents('tr:first');
				default_section(obj);
				if($(obj).val() == '')
					{
					_parent.find('.part_n').html('&nbsp;');
					_parent.find('.code').html('&nbsp;');
					_parent.find('.quantity').val('1');
					$(obj).attr('data-is_QTY', 'false');
					_parent.find('.extended_per').val('0.00').attr('title', '');
					_parent.find('.per_sell').attr('data-original_sell', '0');
					_parent.find('.extended_tm').text('$0.00');
					_parent.find('.per_sell').val('0.00');
					return true;
					}
				else
					{
					return false;
					}
				}

			function default_section(obj)
				{
				var _parent				= $(obj).parents('tr:first');
				var section				= "";
				var current_section		= _parent.find('.section_select').val();
				var master_section		= $('#section_selector').val();
				var master_size			= $('#section_selector').children().size();
				var first_section		= $('#section_selector').children(':nth-child(2)').val();
				if(current_section != master_section && master_section != '0')
					{
					section				= master_section;
					}
				else if((current_section == '0' && master_section != '0' && master_size > 1) || (current_section == '0' && master_section == '0' && master_size > 1))
					{
					section				= first_section;
					}
				else if(current_section != '0')
					{
					section				= current_section;
					}
				if(section == "")
					{
					$('#add_section_pane').dialog('open');
					}
				else
					{
					_parent.find('.section_select').val(section);
					}
				}


			function show_help(obj)
				{
				var _checked		= false;
				if($('#show_help').is(':checked'))
					{
					$('#show_help').attr('checked', false);
					}
				else
					{
					_checked		= true;
					$('#show_help').attr('checked', true);
					}
				$.get('./index.aspx',
						{
						a:			'show_help',
						show:		_checked
						},
						function(_returned)
							{
							if(_returned == 'SUCCESS')
								{
								if(_checked)
									{
									$('.c, .h').each(function()
													{
													$(this).tip();
													});
									}
								else
									{
									$('.tip').hide();
									$('.c, .h').each(function()
													{
													$(this).unbind('mousemove');
													$(this).unbind('mouseover');
													$(this).unbind('blur');
													$(this).unbind('mouseout');
													});
									}
								}
							else
								{
								alert("There was an issue toggling the ability to show help");
								}
							});
				}

			function addCommas(nStr)
				{
				nStr += '';
				x = nStr.split('.');
				x1 = x[0];
				x2 = x.length > 1 ? '.' + x[1] : '';
				var rgx = /(\d+)(\d{3})/;
				while (rgx.test(x1))
					{
					x1 = x1.replace(rgx, '$1' + ',' + '$2');
					}
				return x1 + x2;
				}

			
			function return_sellprice(obj)
				{
				var q			=	{
									a:				"return_sellprice",
									cost:			$('#new_price_pane .new_price').val(),
									master_id:		$('#new_price_pane .new_price').attr('data-master_id')
									};
				
				$.get("./index.aspx", q, 
							function(returned_val)
								{
								var _is_QTY				= false;
								var _parent_row			= $(_sellprice_parent_obj).parents('tr:first');
								_parent_row.find('.per_sell').attr('disabled', true);
								if(returned_val.match(/^[0-9.\-]+$/))
									{
									_parent_row.find('.per_sell').attr('disabled', false);
									_parent_row.find('.extended_per').attr('disabled',true);
									_parent_row.find('.per_sell').attr('data-cost', $('#new_price_pane .new_price').val());
									_parent_row.find('.per_sell').val(parseFloat(returned_val).toFixed(2)).attr('data-original_sell', returned_val).attr('title', "TM Price: "+parseFloat(returned_val).toFixed(2));
									_parent_row.find('.extended_per').val(parseFloat(returned_val).toFixed(2));
									_parent_row.find('.extended_tm').text("$"+parseFloat(returned_val).toFixed(2));
									apply_quantity(_sellprice_parent_obj, false, false);
									}
								_parent_row.find('.per_sell').attr('disabled', false);
								$('#new_price_pane .new_price').val('').attr('data-master_id', '');
								$('#new_price_pane').dialog('close');
								});
				}

			function check_custom(obj)
				{
				var _parent				= $(obj).parents('tr:first');
				var my					=	{
											master_id:		$.trim(_parent.find('.part_n').text()),
											value:			$.trim($(obj).val())
											}
				if((my.value == "" || parseFloat(my.value) == 0) && my.master_id == "")
					{
					$(obj).blur();
					_sellprice_parent_obj			= obj;
					$("#new_price_pane").dialog('open');
					}
				}

			var requester				= $.get();
			function apply_quantity(obj, is_quantity_field, is_master_row)
				{
				if(is_master_row == undefined)
					{
					is_master_row		= false;
					}
				var _parent				= $(obj).parents('tr:first');
				var _description		= _parent.find('.description');
				var _master_id			= _parent.find('.part_n').text();
				var _extended_per		= _parent.find('.extended_per').val();
				var _per_sell			= _parent.find('.per_sell').val();
				var _qty				= _parent.find('.quantity').val();
				if(_qty == "0")
					{
					_parent.find('.quantity').val("1");
					_qty				= 1;
					}
				if(_extended_per == "")
					{
					_extended_per		= (_qty / 1) * ( _per_sell /1);
					}
				var _original_sell		= _parent.find('.per_sell').attr('data-original_sell');
				if($.trim(_master_id) == "")
					{
					_parent.find('.per_sell').attr('data-original_sell', _per_sell).attr('title', "TM Price: "+(_per_sell/1).toFixed(2));
					}
				var _is_QTY				= _description.attr('data-is_QTY');
				if(_is_QTY == undefined)
					{
					_is_QTY				= false;
					}
				var total				= 0;
				if(!_is_QTY.toString().match(/true/i))
					{
					total				= _qty * _per_sell;
					if(total > 0 && is_master_row)
						{
						_parent.find('.extended_tm').text("$"+total.toFixed(2));
						_parent.find('.extended_per').val(total.toFixed(2));
						}
					retotal_worksheet();
					}
				else if(is_quantity_field == true && $(obj).attr('class') != 'extended_per')
					{
					requester.abort();
					requester			= $.get("./index.aspx",
							{
							a:			"adjusted_qty_sell",
							master_id:	_master_id,
							quantity:	_qty,
							sell:		_per_sell
							},
							function(data)
								{
								if(data != '')
									{
									_parent.find('.extended_per').val(((data/1)*_qty).toFixed(2));
									_parent.find('.per_sell').val((data/1).toFixed(2));
									_parent.find('.per_sell').attr('title', "TM Price: "+(data/1).toFixed(2)).attr('data-original_sell', (data/1).toFixed(2));
									if(is_master_row)
										{
										_parent.find('.extended_tm').text("$"+(_qty * data).toFixed(2));
										}
									retotal_worksheet();
									};
								});
					}
				else
					{
					retotal_worksheet();
					}
				}

			function retotal_worksheet(obj)
				{
				var extended				=	{
												per:			0,
												tm:				0
												};
				$('#quote_worksheet .sheet tbody').find('tr').each(
					function()
						{
						var row					=	{
													quantity:				($(this).find('.quantity').val()/ 1),
													per_sell:				($(this).find('.per_sell').val() / 1),
													per_sell_disabled:		$(this).find('.per_sell').is(':disabled'),
													per_extended:			($(this).find('.extended_per').val() / 1),
													per_extended_disabled:	$(this).find('.extended_per').is(':disabled'),
													tm_sell:				($(this).find('.per_sell').attr('data-original_sell') / 1),
													tm_extended:			0
													};
						row.tm_extended			= row.quantity * row.tm_sell;
						if(row.per_extended_disabled == true && row.per_sell_disabled == false)
							{
							row.per_extended		= row.quantity * row.per_sell;
							$(this).find('.extended_per').val(row.per_extended.toFixed(2));
							}
						extended.tm				+= row.tm_extended;
						extended.per			+= row.per_extended;
						$(this).find('.extended_per').attr('title', (row.per_extended/1).toFixed(2));
						$(this).find('.extended_tm').text("$"+row.tm_extended.toFixed(2));
						});
				$('#extended_total').text("$"+extended.per.toFixed(2));
				$('#extended_tms_total').text("$"+extended.tm.toFixed(2));				
				update_tm();				// Get's the server version of what the TM should be at
				update_worksheet_total();	// Get's the server version of what the Quoted total is at
				}
				
//=================================================================================================================
			function update_worksheet_row(obj)
				{
				$(obj).attr('disabled',true);
				
				var this_row				= $(obj).parents('tr:first');
				var this_row_id				= this_row.find('.row_id').val();
				var this_section_id			= this_row.find('.section_select').val();
				var this_part_no			= this_row.find('.part_n').html();
				var this_per_sell			= this_row.find('.per_sell').val();
				var per_sell_disabled		= this_row.find('.per_sell').is(':disabled');
				var per_extended_disabled	= this_row.find('.extended_per').is(':disabled');
				var per_extended			= this_row.find('.extended_per').val();
				var this_qty			= this_row.find('.quantity').val();
				if(this_qty == "")
					{
					this_qty = "1";
					}
				if(per_extended == "")
					{
					per_extended		= (this_qty / 1) * ( this_per_sell /1);
					}
				if(this_part_no == "&nbsp;")
					{
					this_part_no		= "";
					}
				var this_code			= this_row.find('.code').html();
				if(this_code == "&nbsp;")
					{
					this_code			= "";
					}
				var this_description	= this_row.find('.description').val();
				var this_cost			= this_row.find('.per_sell').attr('data-cost');
				if(this_cost == undefined)
					{
					this_cost			= 0;
					}
				var this_original_sell	= this_row.find('.per_sell').attr('data-original_sell');
				
				if(this_per_sell == "")
					{
					this_sell = "NULL";
					}
				if(this_original_sell == "" || this_original_sell == undefined)
					{
					this_original_sell	= "NULL";
					}
					
				if(this_row_id != "" && this_row_id != "undefined" && this_section_id != "" && this_per_sell != "NULL" && parseFloat(this_per_sell) > 0 && parseFloat(this_qty) > 0)
					{
					$.get( "./index.aspx",
							{
							a:				"worksheet_update",
							section_id:		this_section_id,	
							part_no:		escape(this_part_no),
							code:			this_code,
							description:	this_description,
							cost:			this_cost,
							per_sell:		this_per_sell,
							original_sell:	this_original_sell,
							per_extended:	per_extended,
							qty:			this_qty,
							row_id:			this_row_id
							},
							function(data)
								{
								this_row.css({'background-color':'#fff'});
								update_tm();
								update_worksheet_total()
								if(data != 'SUCCESS')
									{
									alert("error:"+data);
									}
								else
									{
									apply_quantity(obj, false);
									}
								$(obj).removeAttr('disabled');
								});
					}
				else if(this_part_no != "")
					{
					this_row.css({'background-color':'#f99'});
								$(obj).removeAttr('disabled');
					}
				else
					{
								$(obj).removeAttr('disabled');				
					}
				}

			function print_quote(type, obj)
				{
				var quote_id			= $("#this_quote_id").val();
				var revision = $("#quote_id select").val();
				var status_id = $('#status_id').val();
				if (type == "w") {
				    if ((status_id == "1") || (status_id == "2") || (status_id == "3") || (status_id == "10") || (status_id == "12")) {
				        if (confirm('Would you like to update the last printed date?')) {

				            $.get("./index.aspx",
                                    {
                                        a: "last_print_date",
                                        quote_id: quote_id,
                                        revision: revision
                                    },
                                    function (data) {
                                        if (data == "FAILED") {
                                            alert("There was a problem updating the last printed date");
                                        }
                                        else {
                                            $("#last_print_date > .date").html(data);
                                            $("#last_print_button").removeAttr("disabled");
											$("#lastfax_button").removeAttr("disabled");
                                        }
                                    });
				        }
				    }
				}
				//$('#print_pane center').show();
				$('#print_type').val(type);
				if(type == "service_report")
					{
					$('#print_pane iframe').attr('src', '/sections/reports/service_report/index.aspx?wo='+$(obj).attr('data-wo'));
					}
				else
					{
					$('#print_pane iframe').attr('src', '/sections/reports/print_quote/index.aspx?quoteid='+quote_id+''+revision+'&type='+type);
					}
				}

			function print_worksheet(type)
				{
				var quote_id			= $('#this_quote_id').val();
				var revision			= $('#quote_id select').val();
				//$('#print_pane center').hide();
				$('#print_pane iframe').attr('src', './index.aspx?a=print_worksheet&quote_id='+quote_id+'&revision='+revision+'&type='+type);
				$('#print_pane').dialog('open');
				}

			function priceto(obj)
				{
				var objval			= $(obj).val();
				if(objval.length > 0)
					{
					var val			= parseFloat(objval);
					if(isNaN(val))
						{
						$(obj).val("");
						$(obj).focus();
						}
					else
						{
						if(val < parseFloat($('#quoted_price').val()))
							{
							alert("Your 'quoted to' price has to be higher than your quoted from price.\nI will clear it.");
							$(obj).val("");
							$(obj).focus();
							}
						else
							{
							quote_obj.s("s_quotedpriceto", obj);
							}
						}
					}
				}

			function cleanArray(actual)
				{
				var newArray = new Array();
				for(var i = 0; i<actual.length; i++)
					{
					if (actual[i])
						{
						newArray.push(actual[i]);
						}
					}
				return newArray;
				}

			function division_check()
				{
				var division_id			= $("#division").val() / 1;
				switch(division_id)
					{
					case 3:
						$("#tab_detail").parents("td:first").hide();
						$("#opt_print_wo").parents("td:first").hide();
						$("#cust_spec_doc").parents("tr:first").hide();
						$("#inflation_term").parents("tr:first").hide();
						$("#percent_down").parents("tr:first").hide();
						$("#option_service_report button").show();
					break;
					default:
						$("#tab_detail").parents("td:first").show();
						$("#opt_print_wo").parents("td:first").show();
						$("#cust_spec_doc").parents("tr:first").show();
						$("#inflation_term").parents("tr:first").show();
						$("#percent_down").parents("tr:first").show();
						$("#option_service_report button").hide();
					break;
					}
				}
/*

 __                     ____             _        
/ _\ __ ___   _____    /___ \_   _  ___ | |_  ___ 
\ \ / _` \ \ / / _ \  //  / / | | |/ _ \| __|/ _ \
_\ \ (_| |\ V /  __/ / \_/ /| |_| | (_) | |_|  __/
\__/\__,_| \_/ \___| \___,_\ \__,_|\___/ \__|\___|


*/

			function save_quote(show_errors, goback)
				{
				return true;
				// Create the targets
				var has_error			= false;
				var error				= "";
				var customer_id			= $('#customer').attr('data-id');
				var contact_id			= $('#contact_id').val();
				var quoted_by			= $('#quoted_by').val();
				if(contact_id == "" || contact_id == "0" || quoted_by == "" || quoted_by == "0")
					{
					has_error			= true;
					if(quoted_by == "" || quoted_by == "0")
						{
						$('#quoted_by').css({'background-color':'#f9a'});
						var _count_error_div	= $('#quoted_by').parent().find(".error").size();
						if(_count_error_div == 0)
							{
							$('#quoted_by').parent().append("<div class='error' style='font-size:10px;color:#f00;'>This field needs to be corrected before this quote can save.</div>");
							}
						error				= "Please select a 'Quoted By' member";
						}
					}
				else
					{
					$('#contact_id').css({'background-color':'#fff'});
					$('#contact_id').parent().find(".error").remove();
					$('#quoted_by').css({'background-color':'#fff'});
					$('#quoted_by').parent().find(".error").remove();
					}
				var quoted_company		= $('#quoted_company').val();
				var quoted_price		= $('#quoted_price').val();
				quoted_price			= quoted_price.replace(/\,/g, "");
				var price_to			= $('#price_to').val();
				var quoted_pricetype	= $('#quoted_pricetype').val();
				var takeoff_price		= $('#takeoff_price').val();
				var revision			= $('#quote_id select').val();
				var us_currency			= $('#us_currency').attr('checked') ? 1 : 0;
				var inflation_term		= $('#inflation_term').attr('checked') ? 1 : 0;
				var percent_down		= $('#percent_down').val();
				var net_due				= $('#net_due').val();
				var status_id			= $('#status_id').val();
				var pct_chance			= $('#pct_chance').find('input:text').val();
				var pct_chance_reason	= $('#pct_chance_reason').val();
				var pct_chance_note		= $('#pct_chance_note').val();
				var address_id			= $('#address_id').val();
				var include_title		= $('#include_title').attr('checked') ? 1 : 0;
				var date_due = escape($('#date_due').val());
				var exp_podate = escape($('#exp_podate').val());
				var completion_date     = escape($('#completion_date').val());
				var custom_term			= escape($('#custom_term').val());
				var job_description		= escape($('#job_description').val());
				var cust_spec_doc		= escape($('#cust_spec_doc').val());
				var last_print_date		= escape($('#last_print_date > .date').text());
				var last_fax_date		= escape($('#last_fax_date > .date').text());
				var verified_date		= escape($('#verified_date > .date').text());
				var division_id = $('#division').val();
				var currency = $('#us_currency').attr('checked') ? 1 : 2;
				var tab_array			= new Array('detail','notes');
				var notes				= new Array();
				var details				= new Array();
				for(i = 0;i < tab_array.length;i++)
					{
					var tab_name			= tab_array[i];
					var n_div				= 0;
					$('#quote_'+tab_name+' .entries ul').find('li > div').each(
						function()
							{
							var textarea	= $(this).find('textarea').val();
							var row_id		= $(this).attr('data-row_id');
								switch(tab_name)
									{
									case 'notes':
										notes[n_div]	= row_id+"]:["+escape($.trim(textarea));
									break;
									case 'detail':	
										details[n_div]	= row_id+"]:["+escape($.trim(textarea));	
									break;
									}
							n_div++;
							});
					var n_div				= 0;
					}
				notes			= cleanArray(notes);
				details			= cleanArray(details);
				if(has_error == false)
					{
					var this_bool		= false;
					$.ajax({
							type:		"POST",
							url:		"index.aspx", 
							data:		{
										a:					"save",
										quote_id:			$('#this_quote_id').val(),
										status_id:			status_id,
										customer_id:		customer_id,
										revision:			revision,
										contact_id:			contact_id,
										completion_date:    completion_date,
										date_due:			date_due,
										job_description:	job_description,
										cust_spec_doc:		cust_spec_doc,
										quoted_by:			quoted_by,
										quoted_company:		quoted_company,
										last_print_date:	last_print_date,
										last_fax_date:		last_fax_date,
										verified_date:		verified_date,
										quoted_price:		quoted_price,
										price_to:			price_to,
										status_id:			status_id,
										quoted_pricetype:	quoted_pricetype,
										takeoff_price:		takeoff_price,
										address_id:			address_id,
										us_currency:		us_currency,
										inflation_term:		inflation_term,
										percent_down:		percent_down,
										pct_chance:			pct_chance,
										pct_chance_reason:	pct_chance_reason,
										pct_chance_note:	pct_chance_note,
										net_due:			net_due,
										custom_term:		custom_term,
										include_title:		include_title,
										division_id:		division_id,
										'details[]':		[details],
										'notes[]': [notes],
										
										currency: currency,
										exp_podate: exp_podate
										},
							beforeSend:	function()
											{
											$('#quote_connection img').attr('src', '/images/quote/decal/decal[connection].gif');
											},
							success:	function(xml)
											{
											if(xml != "SUCCESS")
												{
												alert(xml);
												}
											},
							error:		function(xml)
											{
											//status_check();
											},
							complete:	function()
											{
											if(goback)
												{
												location.href	='./index.aspx';
												}
											$('#quote_connection img').attr('src', '/images/quote/decal/decal[good_health].gif');
											get_status($('#this_quote_id').val(), revision);
											}});
					}
				else
					{
					alert(error);
					tab_control($("#tab_general"), "general");
					}
				//alert('customer_id: '+customer_id+'\n'+'contact_id: '+contact_id+'\n'+'date_due: '+date_due+'\n'+'job_description: '+job_description+'\n'+'cust_spec_doc: '+cust_spec_doc+'\n'+'quoted_by: '+quoted_by+'\n'+'last_print_date: '+last_print_date+'\n'+'last_fax_date: '+last_fax_date+'\n'+'verified_date: '+verified_date+'\n'+'quoted_price: '+quoted_price+'\n'+'quoted_pricetype: '+quoted_pricetype+'\n'+'takeoff_price: '+takeoff_price+'\n'+'tm_pricing: '+tm_pricing+'\n'+'us_currency: '+us_currency+'\n'+'inflation_term: '+inflation_term+'\n'+'percent_down: '+percent_down+'\n'+'net_due: '+net_due+'\n'+'custom_term: '+custom_term);
				}

			function update_tm()
				{
				var quote_id				= $('#this_quote_id').val();
				var revision				= $('#quote_id select').val();
				$.get("./index.aspx",
					{
					a:			"tm_price",
					quote_id:	quote_id,
					revision:	revision
					},
					function(data)
						{
						if(data != "FAILED" && data.length < 50)
							{
							//$('#tm_price').text(data);
							//$('#tm_total_label').text(data);
							}
						else if(data.length > 50)
							{
							//alert("Could not update tm pricing");
							}
						else
							{
							//alert("Failed to get updated TM pricing");
							}
						});
				}

			function update_worksheet_total()
				{
				var quote_id				= $('#this_quote_id').val();
				var revision				= $('#quote_id select').val();
				$.get("./index.aspx",
					{
					a:			"worksheet_total",
					quote_id:	quote_id,
					revision:	revision
					},
					function(data)
						{
						if(data != "FAILED" && data.length < 50)
							{
							//$('#worksheet_price').text('$'+data);
							//$('#worksheet_total_label').text('$'+data);
							}
						else if(data.length > 50)
							{
							//alert("Could not update worksheet total.");
							}
						else
							{
							//alert("Failed to get updated worksheet total");
							}
						});
				}


			function kill_resurrect(obj)
				{
				var quote_id				= $('#this_quote_id').val();
				var revision				= $('#quote_id select').val();
				var status_id				= $('#status_id').val();
				var status_to_send			= "";
				var prev_status_id			= $('#prev_status_id').val();
				var competitor				= $('#kill_pane').find('input:text').val();
				var competitor_id			= $('#competitor_id').val();
				if(competitor == "")
					{
					competitor				= "NULL";
					competitor_id			= "NULL";
					}
				if(status_id == "6" || status_id == "9")
					{
					if(prev_status_id == "NULL")
						{
						prev_status_id			= "2";
						}
						
					status_to_send				= prev_status_id;
					}
				else
					{
					status_to_send				= "6";
					}
				var why_lose					= $("#why_lose").val();
				if($.trim(why_lose) == "")
					{
					alert("You must at least supply the reason why we lost the job.");
					$("#why_lose").focus();
					return false;
					}
				var who_competitor				= $("#who_competitor").val();
				var what_price					= $("#what_price").val();
				/*
				reasons				= new Array();
				$('#kill_pane').find('input:checkbox:checked').each(function()
					{
					reasons.push($(this).val());
					});
				*/
				$(obj).attr('title', 'Please Wait...');
				$(obj).attr('disabled', 'disabled');
				$.get("./index.aspx",
						{
						    a: 'status_update',
						    quote_id: quote_id,
						    status_id: status_to_send,
						    revision: revision,
						    why_lose: why_lose,
						    who_competitor: who_competitor,
						    what_price: what_price
						},
						function (data) {
						    if (data == "SUCCESS") {
						        if (status_id == "6") {
						            $('#status_id').val(prev_status_id);
						            $('#prev_status_id').val("");
						            $('#toggleimg').attr("src", "/images/icon/icon[dead].gif");
						            $('#toggletext').text("Kill Quote");
						            $('#kill_quote').removeAttr('disabled');
						            location.href = location.href;
						        }
						        else {
						            $('#prev_status_id').val(status_id);
						            $('#status_id').val("6");
						            $('#toggleimg').attr("src", "/images/icon/icon[alive].gif");
						            $('#toggletext').text("It's Alive");
						            $('#resurrect_quote').removeAttr('disabled');
						            //	location.href = "./index.aspx";
						            window.opener.location.href = window.opener.location.href;
						            window.close();
						        }
						    }
						    else {
						        $(obj).removeAttr('disabled');
						        //	alert("There was a problem changing the quote's status.\nPlease talk to the IT department regarding this error.\n"+data);
						    }
						    $(obj).attr('title', '');
						});
				}

			function status_toggle(obj)
				{
				$('#kill_pane').dialog('open');
				
				switch($('#status_id').val())
					{
					case '6':
					case '9':	$('#kill_quote').attr('disabled', 'disabled');
								$('#resurrect_quote').removeAttr('disabled');
					break;
					default:	$('#resurrect_quote').attr('disabled', 'disabled');
								$('#kill_quote').removeAttr('disabled');
					break;
					}
				}

			/*	
			function get_sellprice(obj, amount)
				{
				var markedup_amount		= 0;
				if(amount > 5000)
					{
					markedup_amount		= amount * 1.35;
					}
				else
					{
					markedup_amount		= Math.Round(((1.35+(2.4*(1/Math.Exp(0.14*amount))))*amount)*100)/100;
					}
				$(parent_node).parent().parent().find('.extended_per').html("$"+markedup_amount);
				}
			*/

			function push_contact(obj)
				{
				var customer_id				= $('#customer').attr('data-id');
				if(customer_id == undefined)
					{
					customer_id				= $('#newquote_customer').attr('data-id');
					}
				var obj_parent				= $(obj).parents().find('table:first');
				var address_id				= $("#address_id").val();
				var contact_name			= obj_parent.find('.contact_name').val();
				var contact_title			= obj_parent.find('.contact_title').val();
				var contact_email			= obj_parent.find('.contact_email').val();
				var contact_ext				= obj_parent.find('.contact_ext').val();
				var contact_cell			= obj_parent.find('.contact_cell').val();
				var vars					=	{
												a:					"push_contact",
												address_id:			address_id,
												customer_id:		customer_id,
												name:				contact_name,
												title_id:			contact_title,
												email:				contact_email,
												ext:				contact_ext,
												cell:				contact_cell
												};
				$.get('index.aspx',vars	,
						function(data)
							{
							if(data != "FAILED" && data.match(/^\d+$/gi))
								{
								if($('#contact_id').val() != undefined)
									{
									$('#contact_id').append("<option value='"+data+"'>"+contact_name+"</option>").removeAttr('disabled');
									$('#quote_addcontact').dialog('close');
									$('#contact_id').val(data.toString());
									quote_obj.s("s_contact", $('#contact_id'));
									}
								}
							else
								{
								alert("There was an error saving this contact's information.\nWhat was returned:\n"+data);
								}
							});
				}

			function get_part_description(part_scheme, parent_node)
				{
				if(part_scheme != "")
					{
					var description		= "";
					var url				= "/_tools/inventory_search/index.aspx?q="+part_scheme;
					$.ajax(
							{
							url:		url,
							dataType:	"xml",
							type:		"GET",
							success: function(xml)
										{
										$(xml).find('thispart').each(function()
																{
																description			= $(this).attr('tag')+" - "+$(this).attr('description');
																});
										},
							error:	function()
										{
										//alert("there was a problem");
										},
							complete: function(XMLHttpRequest, textStatus)
										{
										//description					= description+",";
										//$(parent_node).parent().find('table:first').remove();
										//var altered_description		= description.replace(/([\w ]+): ([\w ]+),/g, "<span style='font-weight:bold;'>$1:</td><td>$2</td></span>");
										//$(parent_node).parent().find('div:first').html(altered_description);
										}
							});
					}
				}

			function set_pricing(obj, clear)
				{
				var master_id			= $.trim($(obj).parents('tr:first').find('.part_n').text());
				var _parent				= $(obj).parents('tr:first');
				var _is_QTY				= _parent.find('.description').attr('data-is_QTY');
				if(Boolean(_is_QTY) == true && !clear)
					{
					return false;
					}
				else
					{
					if(master_id != "")
						{
						$.get("./index.aspx",
								{
								a:			"get_costprice",
								master_id:	master_id
								},
								function(data)
									{
									if(data != "FAILED")
										{
										var sellprice			= 0;
										var _qty_target			= _parent.find('.quantity');
										var _sell_target		= _parent.find('.extended_per');
										var _cost_target		= _parent.find('.per_sell');
										var _total_target		= _parent.find('b');
										$.get("./index.aspx",
													{
													a:			"get_sellprice",
													master_id:	master_id
													},
													function(sub_data)
														{
														if(sub_data != "FAILED")
															{
															sellprice			= sub_data;
															var this_qty		= _qty_target.val();
															if(_sell_target.val() == "" || clear == true)
																{
																if(this_qty == '')
																	{
																	_qty_target.val(1);
																	this_qty		= 1;
																	}
																var total			= this_qty * sellprice;
																_sell_target.val(sellprice);
																_total_target.text(total);
																}
															}
														else
															{
															if(_sell_target.val() == "")
																{
																_sell_target.val('0');
																_total_target.text('$0.00');
																}
															}
														});
										
										_cost_target.val("0.00");
										/*
										var markedup_amount		= 0;
										if(tag_id != 571)
											{
											if(data > 5000)
												{
												//markedup_amount		= data * 1.35;
												}
											else
												{
												markedup_amount			= Math.round(((1.35+(2.4*(1/Math.exp(0.14*data))))*data)*100)/100;
												}
											}
										else
											{
											markedup_amount			= data;
											}
										*/
										}
									});
						}
					}
				}

			function worksheet_del_row(row_id)
				{
				$.get("./index.aspx",
							{
							a:			"del_worksheet_row",
							row_id:		row_id
							},
							function(data)
								{
								if(data != "SUCCESS")
									{
									alert("failed");
									}
								});
				}

			function get_status(quote_id, revision)
				{
				$.get("./index.aspx",
							{
							a:			"get_status",
							quote_id:	quote_id,
							revision:	revision
							},
							function(data)
								{
								if(data == "FAILED")
									{
									//alert("Could not update status of quote");
									}
								else
									{
									$("#status_id").val(data);
									}
								});
				}

			function load_sections(section_id)
				{
				var quote_id				= $('#this_quote_id').val();
				var rev						= $('#quote_id select').val();
				var url						= "./index.aspx?a=xml_sections&quote_id="+quote_id+"&rev="+rev;
				var target					= $('#section_selector');
				var current_filter			= target.val();
				target.children().remove();
				var sub_target				= $('#add_section_pane table tbody');
				var current_section			= "";
				var target_html				= "";
				var sub_target_html			= "";
				var selected				= "";
				$.ajax({
						type:				'GET',
						url:				url,
						dataType:			'xml',
						beforeSend:			function()
												{
												$('#save_animation').fadeIn('fast');
												if(current_filter == "0")
													{
													target_html			= "<option value='0' selected>VIEW ALL SECTIONS</option>";
													}
												else
													{
													target_html			= "<option value='0'>VIEW ALL SECTIONS</option>";
													}
												},
						error:				function(data)
												{
												$('#save_animation').fadeOut('fast');
												//alert("There was an error retrieving the sections for this quote.");
												},
						success:	function(xml)
										{
										sub_target.children().remove();										
										if($(xml).find('error_in_xmit').size() == 0)
											{
											var n_part_row			= $(".sheet").find('.section_select').size();
											$(xml).find('section').each(
												function()
													{
													var id					= $(this).attr('id');
													if(id == current_filter)
														{
														selected			= " SELECTED";
														}
													else
														{
														selected			= "";
														}
													var name				= unescape_me($(this).attr('name'));
													target_html				+= "<option value='"+id+"'"+selected+">VIEW ONLY ["+name+"]</option>";
													sub_target_html			+= "<tr><td onclick='rename_section(this)' style='cursor:pointer;'><input type='hidden' value='"+id+"' /><span class='name' style='width:100%;'>"+name+"</span></td><td align='center'><button onmouseup=\"del_section("+id+");\"><img src='/images/icon/icon[delete].gif' /></button></td></tr>";
													});
													
											if(n_part_row > 0)
												{
												$(".sheet").find('.section_select').each(
														function()
															{
															var parent_this					= $(this);
															current_section					= $(this).val();
															if($(this).val() == "0")
																{
																current_section				= $(this).parent().children('.this_section_id').val();
																}
															$(this).attr('disabled', false);
															$(this).children().remove();
															$(this).html("<option value='0'>SECTIONS</option>");
															$(xml).find('section').each(
																function()
																	{
																	var id					= $(this).attr('id');
																	var name				= unescape_me($(this).attr('name'));
																	if(current_section != 0)
																		{
																		if(id == current_section || id == section_id)
																			{
																			selected			= " SELECTED";
																			}
																		else
																			{
																			selected			= "";
																			}
																		}
																	parent_this.append("<option value='"+id+"' "+selected+">"+name+"</option>");
																	});
															});
												}
											}
										else
											{
											sub_target_html				= "<tr><td colspan='2' class='none' align='center'>no sections have been defined.</td></tr>";
											}
										},
						complete:		function()
											{
											target.append(target_html);
											sub_target.append(sub_target_html);
											$('#add_worksheet_row .section_select').val($('#section_selector').val());
											}
						});
				}

			function save_section(obj)
				{
				var _target				= $(obj).parents('table:first').find('input:text');
				$(obj).parents('tr:first').children().each(function(){$(this).attr('disabled',true)});
				var this_value			= $.trim(_target.val());
				if(this_value != "")
					{
					$.get("./index.aspx",
								{
								a:			"save_section",
								quote_id:	$('#this_quote_id').val(),
								rev:		$('#quote_id select').val(),
								n:			this_value
								},
								function(data)
									{
									if(data == "SUCCESS")
										{
										load_sections();
										$('#add_worksheet_row .section_select').val('option:last');
										}
									else
										{
										alert("failed");
										}
									$(obj).parents('tr:first').children().each(function(){$(this).attr('disabled',false)});
									_target.val("").focus();
									});
					}
				}

			function rename_section(obj)
				{
				if($(obj).find('input:text').size() == 0)
					{
					var current_name		= $(obj).find('.name').text();
					$(obj).find('span').html("<input type='text' title='Press Enter to Save' value=\""+current_name+"\" onkeydown=\"if(event.keyCode == 13){update_section(this);}\" style='width:98%;font-weight:bold;'/>");
					$(obj).find('input:text').select();
					}
				}

			function update_section(obj)
				{
				var this_value			= $.trim($(obj).val());
				var this_id				= $(obj).parent().parent().find('input:hidden').val();
				if(this_value != "")
					{
					$.get("./index.aspx",
								{
								a:			"update_section",
								quote_id:	$('#this_quote_id').val(),
								revision:	$('#quote_id select').val(),
								section_id:	this_id,
								name:		this_value
								},
								function(data)
									{
									if(data == "SUCCESS")
										{
										load_sections();
										$(obj).parent().text(this_value);
										$(obj).parent().parent().click("rename_section(this)");
										}
									else
										{
										alert("failed");
										}
									});
					}
				}

			function del_section(section_id)
				{
				if(confirm("Are you sure you want to delete this section, including all associated part references?"))
					{
					$.get("./index.aspx",
								{
								a:			"del_section",
								section_id:	section_id
								},
								function(data)
									{
									if(data == "SUCCESS")
										{
										var quote_id				= $('#this_quote_id').val();
										var revision				= $('#quote_id select').val();
										$('.sheet tbody').children().remove();
										load_sections();
										get_worksheet(quote_id, revision, 0, true);
										}
									else
										{
										alert("failed");
										}
									});
					}
				}

			function load_section(obj)
				{
				var section_id		= $(obj).val();
				if(section_id == "0")
					{
					$('.sheet tbody').children().each(function(){$(this).remove();});
					get_worksheet($('#this_quote_id').val(),$('#quote_id select').val(), 0, true);
					}
				else
					{
					$('.sheet tbody').children().each(function(){$(this).remove();});
					get_worksheet($('#this_quote_id').val(),$('#quote_id select').val(), section_id);	
					}
				}

			function change_section(obj)
				{
				if($(obj).val() != 0)
					{
					if($(obj).parent().children('.row_id').val() == "")
						{
						var quote_id			= $('#this_quote_id').val();
						var revision			= $('#quote_id select').val();
						var row_id				= "0";
						$.get( "./index.aspx",
								{
								a:			"new_worksheet_row_id",
								quote_id:	quote_id,
								revision:	revision,
								section_id: $(obj).val()
								},
								function(data)
									{
									if(data.match(/^\d+$/))
										{
										$(obj).parent().children('.row_id').val(data)
										}
									else
										{
										alert("There was a problem retrieving this row's unique ID\nPlease talk to the IT department regarding this error.");
										}
									});
						}
					else
						{
						$.get( "./index.aspx",
								{
								a:			"switch_section_for_worksheet_row",
								row_id:		$(obj).parent().children('.row_id').val(),
								section_id: $(obj).val()
								},
								function(data)
									{
									if(data != "SUCCESS")
										{
										alert("There was a problem changing the section for that row entry.\nPlease talk to the IT department regarding this error.");
										}
									else
										{
										if($('#section_selector').val() != "0" && $('#section_selector').val() != $(obj).val())
											{
											$(obj).parent().parent().remove();
											}
										}
									});
						}
					}
				}

			function save_appointment(obj)
				{
				$(obj).attr('disabled', true);
				var _parent			= $(obj).parent().parent().parent();
				var _date			= _parent.find('#schedule_date').val();
				var _note			= _parent.find('#follow_up_note').val();
				if(_note == "" || _date == "")
					{
					alert("You can't submit an incomplete followup");
					}
				else
					{
				var _id				= _parent.find('#schedule_id').val();
				var _quote_id		= $('#this_quote_id').val();
				var _revision		= $('#quote_id select').val();
				$.get('./index.aspx',
						{
						a:			"save_appointment",
						quote_id:	_quote_id,
						revision:	_revision,
						note:		escape(_note),
						date:		_date,
						_id:		_id
						},
						function(v)
							{
							if(v == 'SUCCESS')
								{
								_parent.find('#schedule_date').val("");
								_parent.find('#follow_up_note').val("");
								_parent.find('#schedule_id').val("");
								load_followup_history();
								follow_up_window();
								}
							else
								{
								alert("There was a problem submitting the appointment");
								}
							});
					}
				$(obj).removeAttr('disabled');
				}

			function shuffle_notes(id, obj)
				{
				if(!$("#note_"+id).is(':hidden'))
					{
					$("#note_"+id).slideUp(150);
					}
				else
					{
					$(".n:visible").slideUp(100);
					$("#note_"+id).slideDown(150);
					}
				}

			function load_followup_history()
				{
				var _quote_id		= $('#this_quote_id').val();
				var _revision		= $('#quote_id select').val();
				var _url			= "./index.aspx?a=xml_followup_history&quote_id="+_quote_id+"&revision="+_revision;
				
				var _content		= "";
				$.ajax({
						type:			'GET',
						url:			_url,
						dataType:		'xml',
						beforeSend:		function()
											{
											},
						success:		function(xml)
											{
											if($(xml).find('item').size() > 0)
												{
											$(xml).find('item').each(function()
												{
												var id				= $(this).children('id').text();
												var name			= $(this).children('name').text();
												var type			= $(this).children('type').text();
												var note			= unescape_me($(this).children('note').html());
												if(type == 'f')
													{
													type			= "Follow Up";
													}
												else if(type == 'h')
													{
													type			= "History Item";
													}
												else if(type == 'n')
													{
													type			= "Note";
													//note			= note.replace("\n", "<br/>");
													}
												else if(type == 's')
													{
													type			= "Status Changed";
													note			= note.replace(" from: '", " from: <b style='color:#f00'>'");
													note			= note.replace(" to '", "</b> to <b style='color:#f00'>'");
													note			= note+"</b>";
													}
												var date			= $(this).children('date').text();
												_content			+= "\
												<tr style='cursor:pointer;' class='tr' onmouseup=\"shuffle_notes('"+id+"', this);\">\
													<td class='t' align='center'>"+date+"</td>\
													<td class='t' align='center'>"+type+"</td>\
													<td class='t' align='center'>"+name+"</td>\
												</tr>\
												<tr>\
													<td colspan='3'><div class='n' id='note_"+id+"'>"+note+"</div></td>\
												</tr>";
												});
												}
											else
												{
												_content			+= "\
												<tr class='tx'>\
													<td colspan='3' class='x'>No history recorded.</td>\
												</tr>";
												}
											},
						error:			function(XMLHttpRequest, textStatus, errorThrown)
											{
											//alert("There was an issue retrieving the follow up history");
											},
						complete:		function()
											{
											$('#quote_history').html(_content);
											$('#quote_history .tr').each(function()
												{
												$(this).hover(
												function()
													{
													$(this).css({'background-color':'#fff'});
													}, 
												function()
													{
													$(this).css({'background-color':'#ccc'});
													});
												});
											}
						});
										
				}

			function section_box(obj)
				{
				$('#add_unit').dialog(
									{
									resizable: false, 
									autoOpen: false, 
									height: 50, 
									modal:true,
									width: 300,
									closeOnEscape: true, 
									close: function()
											{
											
											} 
									});
				}

			function lockdown(remove)
				{
				if(remove == undefined)
					{
					remove	= false;
					}
				if(remove)
					{
					$("input, select, textarea, button").removeAttr('disabled');
					}
				else
					{
					var status_id = $('#status_id').val();
					var current_killres_img	= $('#toggleimg').attr("src");
					switch(status_id)
						{
						case "1": 
							$('#last_print_button').attr('disabled', 'disabled');
							$('#last_verified_button').attr('disabled', 'disabled');
							$('#last_fax_button').attr('disabled', 'disabled');
							$('#quoted_by').removeAttr('disabled');
						break;
						case "2":   
							$('#last_print_button').removeAttr("disabled");
							$('#last_fax_button').removeAttr("disabled");
							$('#quoted_by').removeAttr('disabled');
						break;
						case "3": 
						case "7":	
						case "4":
						    $('#quote_general .v *').each(function()
								{
								var this_override			= $(this).attr("data-unlock") == null ? "False" :$(this).attr("data-unlock");
								var node_name				= $(this).get(0).nodeName;
								var is_input				= node_name === "INPUT" || node_name === "SELECT" || node_name === "TEXTAREA" || node_name === "BUTTON";
								if(this_override != "True" && is_input)
									{
									$(this).attr('disabled', 'disabled');
									}
								else if(is_input)
									{
								console.log(this_override+" - "+$(this).attr("id")+" - "+node_name);
									
									}
								});
						    $('#quote_detail .entries ul').sortable("destroy");
						    $('#quote_notes .entries ul').sortable("destroy");
							var quoter_locked			= $("#lock_quoter").is(":checked");
							if(quoter_locked)
								{
								$("#quoted_by").attr("disabled", "disabled");
								}
							if(current_killres_img != "/images/icon/icon[help].gif")
								{
								$('#toggleimg').attr('src', '/images/icon/icon[dead].gif');
								}
							$('#toggletext').text("Kill Quote");
							$('#quote_detail').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#quote_notes').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#quote_worksheet *').attr('disabled', 'disabled');
							$('#quote_worksheet').find('.print_buttons .header_button').removeAttr('disabled');
							$('#option_status').removeAttr('disabled');
							$('#last_print_button').attr('disabled', 'disabled');
							if(status_id != "3")
								{
								$('#last_verified_button').attr('disabled', 'disabled');
								if(status_id == "4")
									{
									$('#received_button').removeAttr('disabled');
									}
								}
							else
								{
								$('#last_verified_button').removeAttr('disabled');
								$('#received_button').attr('disabled', 'disabled');
								}
							$('#last_fax_button').attr('disabled', 'disabled');
							$('#tr_pc *').removeAttr('disabled');
							$('#tr_ec *').removeAttr('disabled');
							$('#quoted_by').removeAttr('disabled');
							$('#quoted_by').children().each(function(){$(this).removeAttr('disabled');});
						break;	
						case "6":
							$('#quote_general .v *').attr('disabled', 'disabled');
							$('#quote_detail .entries ul').sortable("destroy");
							$('#quote_notees .entries ul').sortable("destroy");
							$('#quote_detail').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#quote_notes').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#received_button').attr('disabled', 'disabled');
							$('#quote_worksheet *').attr('disabled', 'disabled');
							$('#toggleimg').attr('src', '/images/icon/icon[alive].gif');
							//$('#toggletext').text("It's Alive");
							$('#revision_quote').attr("disabled", true);
							$('#quote_worksheet').find('.print_buttons .header_button').removeAttr('disabled');
							$('#option_status').removeAttr('disabled');
							$('#last_print_button').attr('disabled', 'disabled');
							$('#last_verified_button').attr('disabled', 'disabled');
							$('#last_fax_button').attr('disabled', 'disabled');
						break;
						case "8":
							$('#quote_general .v *').attr('disabled', 'disabled');
							$('#received_button').attr('disabled', 'disabled');
							$('#quote_detail .entries ul').sortable("destroy");
							$('#quote_notees .entries ul').sortable("destroy");
							$('#quote_detail').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#quote_notes').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#quote_worksheet *').attr('disabled', 'disabled');
							$('#quote_maintenance *').attr('disabled', 'disabled');
							$('#option_status button').attr('disabled', 'disabled');
							$('#received_button').attr('disabled', 'disabled');
							$('#revision_quote').attr("disabled", true);
							$('#quote_worksheet').find('.print_buttons .header_button').removeAttr('disabled');
							$('#last_print_button').attr('disabled', 'disabled');
							$('#last_verified_button').attr('disabled', 'disabled');
							$('#last_fax_button').attr('disabled', 'disabled');
							$('#quoted_by').removeAttr('disabled');
							$('#quoted_by').children().each(function(){$(this).removeAttr('disabled');});
						break;
						case "9":
							$('#quote_general .v *').attr('disabled', 'disabled');
							$('#received_button').attr('disabled', 'disabled');
							$('#quote_detail .entries ul').sortable("destroy");
							$('#quote_notees .entries ul').sortable("destroy");
							$('#quote_detail').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#quote_notes').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#quote_worksheet *').attr('disabled', 'disabled');
							$('#quote_maintenance *').attr('disabled', 'disabled');
							$('#option_status button').attr('disabled', 'disabled');
							$('#revision_quote').attr("disabled", true);
							$('#received_button').attr('disabled', 'disabled');
							$('#quote_worksheet').find('.print_buttons .header_button').removeAttr('disabled');
						break;
						case "12":
							$('#last_print_button').attr('disabled', 'disabled');
							$('#last_verified_button').attr('disabled', 'disabled');
							$('#last_fax_button').attr('disabled', 'disabled');
						break;
						case "10":
							$("input, select, textarea, button").attr('disabled', 'disabled');
							$('#last_print_button').attr('disabled', 'disabled');
							$('#last_verified_button').attr('disabled', 'disabled');
							$('#last_fax_button').attr('disabled', 'disabled');
						break;
						case "11": 
						    $('#quote_detail .entries ul').sortable("destroy");
						    $('#quote_notees .entries ul').sortable("destroy");
							$('#quote_detail').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#quote_notes').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#option_status button').attr('disabled', 'disabled');
							$('#revision_quote').attr("disabled", true);
							$('#received_button').attr('disabled', 'disabled');
							$('#quote_worksheet').find('.print_buttons .header_button').removeAttr('disabled');
							$('#last_print_button').attr('disabled', 'disabled');
							$('#last_verified_button').attr('disabled', 'disabled');
							$('#last_fax_button').attr('disabled', 'disabled');
						break;
					    case "13": 
					        $('#quote_detail .entries ul').sortable("destroy");
					        $('#quote_notees .entries ul').sortable("destroy");
							$('#quote_detail').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#quote_notes').find("*").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
							$('#tab_worksheet').attr('disabled', 'disabled');
							$('#option_status button').attr('disabled', 'disabled');
							$('#revision_quote').attr("disabled", true);
							$('#received_button').attr('disabled', 'disabled');
							$('#quote_worksheet').find('.print_buttons .header_button').removeAttr('disabled');
						break;
						default:		
							$('document').find("input, select, textarea, button").each(function()
								{
								if($(this).attr('data-keepenabled') != "1")
									{
									$(this).attr('disabled', 'disabled');
									}
								});
						break;
						}
					}
				}

			function status_check()
				{
				var status_id				= $('#status_id').val();
				var quote_id				= $('#this_quote_id').val();
				var version					= $('#quote_id select').val();
				$.get( "./index.aspx",
							{
							a:				"get_status",
							quote_id:		quote_id,
							revision:		version
							},
							function(resp)
								{
								var statuses		= new Array(" ", "Waiting to be Quoted", "Waiting to be Sent", "Waiting to be Verified", "Waiting for Approval", "Follow up today", "Dead Quote", "Placeholder", "P.O Received", "Revisioned", "Waiting for Stage 1 Go", "Waiting for Stage 4 Go", "Waiting for Final Review", "Waiting Post Mortem");
								var status			= $("status").text();
								if(status != statuses[resp])
									{
									$("#status").text(statuses[resp]);
									}
								if(resp != status_id)
									{								
									$('#status_id').val(resp);
									lockdown(false);
									return true;
									}
								else if (!isNaN(resp))
									{
									var health					= $('#quote_connection img').attr('src');
									if(health == "/images/quote/decal/decal[bad_health].gif" && resp != '8' && resp != '9')
										{
										$('#quote_connection img').attr('src', '/images/quote/decal/decal[good_health].gif');
										$('#quote_connection img').attr('title', 'The connection to the server is good');
										lockdown(true);
										}
									return true;
									}
								else
									{
									$('#quote_connection img').attr('src', '/images/quote/decal/decal[bad_health].gif');
									$('#quote_connection img').attr('title', 'The connection to the server has been lost, please minimize this window and log into NESI in a different browser window.');
									lockdown();
									}
								});
				}

			function reason_map_update(obj)
				{
				var quote_id				= $('#this_quote_id').val();
				var revision				= $('#quote_id select').val();
				var checked					= $(obj).attr('checked') == true?1:0;
				var this_id					= $(obj).val();
				$.get( "./index.aspx",
						{
						a:				"reason_update",
						quote_id:		quote_id,
						revision:		revision,
						reason_id:		this_id,
						checked:		checked
						},
						function(data)
							{
							if(data != "SUCCESS")
								{
								alert("Could not update this check box");
								$(obj).attr('checked', false);
								}
							});
				}

			function search_quotes(obj)
				{
				var _parent				= $(obj).parent().parent();
				var job_description		= escape(_parent.find('.jobdescription').val());
				var business_unit_id			= _parent.find('.subquotedby_company').val();
				var quoted_by			= _parent.find('.subquotedby_name').val();
				var customer_name		= escape(_parent.find('.customer_name').val());
				var quote_id			= _parent.find('.quote_number').val();
				var quoted_before		= _parent.find('.quoted_before').val();
				var quoted_after		= _parent.find('.quoted_after').val();
				var status_id			= _parent.find('.status').val();
				var appendature			= "";
				var url					= "./index.aspx?a=xml_quotesearch&job_description="+job_description+"&quoted_by="+quoted_by+"&customer_name="+customer_name+"&quoted_before="+quoted_before+"&quoted_after="+quoted_after+"&status_id="+status_id+"&quote_id="+quote_id+"&business_unit_id="+business_unit_id;
				//document.write(url);
				$.ajax({
						type:			'GET',
						url:			url,
						dataType:		'xml',
						beforeSend:		function()
											{
											$('#quote_search_results').html("<img src='/images/loading_panel.gif' vspace='50'/>");
											},
						success:		function(xml)
											{
											if($(xml).find('error').size() > 0)
												{
												$(xml).find('error').each(
													function()
														{
														$('#quote_search_results').append($(this).attr('text'));
														});
												
												}
											else
												{
												appendature						= "\
																<table class='results' cellpadding='2' cellspacing='0'>\
																	<thead>\
																		<th>date</th>\
																		<th>quote #</th>\
																		<th>Desc.</th>\
																		<th>customer</th>\
																		<th>price</th>\
																		<th>quoted by</th>\
																		<th>contact</th>\
																		<th>status</th>\
																	</thead>\
																	<tbody>";
												$(xml).find('quote').each(
													function()
														{
														var quote_id			= $(this).children('quote_id').text();
														var revision			= $(this).children('revision').text();
														var customer_name		= unescape_me($(this).children('customer_name').text());
														//if(customer_name.length > 10)
													//		{
												//			customer_name		= customer_name.substring(0,10)+"..";
												//			}
														var open_date			= $(this).children('open_date').text();
														var quoted_price		= $(this).children('quoted_price').text();
														var job_description_o, job_description_n		= "";
														job_description_o		= $(this).children('job_description').text();
														job_description_o		= job_description_o.replace(/\"/g, "&quot;");
														if(job_description_o.length == 0)
															{
															job_description_n	= "--";
															}
														else if(job_description_o.length > 20)
															{
															job_description_n	= job_description_o.substring(0,20)+"..";
															}
														else
															{
															job_description_n	= job_description_o;
															}
														var quoted_by			= unescape_me($(this).children('quoted_by').text());
														var contact_name		= unescape_me($(this).children('contact_name').text());
														var status				= $(this).children('status').text();
														var window_name			= new Date().getTime();
														appendature				+= "\
																			<tr onclick=\"boing('./index.aspx?a=g&quote_id="+quote_id+"&revision="+revision+"', 'quote_searched"+window_name+"', 1035, 750);\" title=\""+job_description_o+"\">\
																				<td>"+open_date+"</td>\
																				<td align='center'><b>"+quote_id+" v"+revision+"</b></td>\
																				<td>"+job_description_n+"</td>\
																				<td align='left'>"+customer_name+"</td>\
																				<td>"+quoted_price+"</td>\
																				<td>"+quoted_by+"</td>\
																				<td>"+contact_name+"</td>\
																				<td align='center'>"+status+"</td>\
																			</tr>";
														});
												appendature						+= "</tbody></table>";
												}
											},
						complete:		function()
											{
											$('#quote_search_results').html(appendature);
											$('#quote_search_results table').tablesorter(
												{
													headers:
														{
														0:{sorter:'date'},
														1:{sorter:'currency'},
														4:{sorter:'currency'}
														}
												});
											}
						});
				}

			function update_po_info(type, val)
				{
				$.get('./',
						{
						a:			'update_po_info',
						quote_id:	$('#this_quote_id').val(),
						revision:	$('#quote_id select').val(),
						type:		type,
						value:		escape(val)
						},
						function(data)
							{
							if(data != "SUCCESS")
								{
								alert(data);
								}
							});
				}

			var current_tab_id				= "general";
			function tab_control(obj, tab_id)
				{
				var quote_id				= $('#this_quote_id').val();
				var revision				= $('#quote_id select').val();
				var status_id				= $('#status_id').val();
				$('.tip').hide();
				update_tm();
				if(tab_id == "strategy")
					{
					if($("#if_strategy").attr("src") == "about:blank")
						{
						$("#if_strategy").attr("src", "./recon_frame.aspx?quote_id="+quote_id+"&rev="+revision);
						}
					}
				if(tab_id == "folder")
					{
					if($("#if_folder").attr("src") == "about:blank")
						{
						$("#if_folder").attr("src", "./folder.aspx?quote_id="+quote_id);
						}
					}
				$('#quote_tabs').find('button').each(function(){$(this).attr('disabled', true).attr('class', 'active')});
				$("#quote_"+current_tab_id).hide(0,function()
															{
															$("#quote_"+tab_id).show(0);
															$('#quote_tabs').find('button').each(function(){$(this).attr('disabled', false).attr('class', 'normal')});
															$("#tab_"+tab_id).attr('disabled', true).attr('class', 'active');
															current_tab_id			= tab_id;
															//$("#tab_"+current_tab_id).attr('class', 'normal').removeAttr('disabled');
											});
				lockdown();
				$(obj).blur();
				}
