
			var active_option			= "";

			$(document).ready(function()
								{
								$.ajaxSetup ({ cache: false});
								$('html').css({'background-image':'url()'});
								for(var i = 1;i <= 2; i++)
									{
									$('#option_'+i).find('*').each(function(){$(this).attr('disabled', true)});
									$('#option_'+i).parent().find('input:checkbox').removeAttr('checked');
									$('#option_'+i).fadeTo('fast', .5);
									}
								$('#submit_button').attr('disabled',true);
								$('#start_date').datepicker({
															dateFormat:	'yy-mm-dd',
															beforeShowDay: $.datepicker.noWeekends, 
															minDate: 'today',
															showAnim: 'slideDown',
															onSelect: function()
																		{
																		var dateMin = $('#start_date').datepicker('getDate');
																		$('#end_date').datepicker("option", {minDate: new Date(dateMin.getFullYear(), dateMin.getMonth(), dateMin.getDate())});
																		setTimeout("$('#end_date').focus()", 250);
																		}
															});
								$('#start_date').focus(function()
															{
															check_request()
															});
								$('#end_date').datepicker({
															dateFormat: 'yy-mm-dd',
															showAnim: 'slideDown',
															beforeShowDay: $.datepicker.noWeekends,
															onSelect: function()
																		{
																		var dateMin = $('#end_date').datepicker('getDate');
																		$('#return_date').datepicker("option", {minDate: new Date(dateMin.getFullYear(), dateMin.getMonth(), dateMin.getDate())});
																		setTimeout("$('#return_date').focus()", 250);
																		}
															});
								$('#end_date').focus(function()
															{
															var dateMin = $('#start_date').datepicker('getDate'); 
															if(dateMin == null)
																{
																$('#start_date').focus();
																}
															else
																{
																check_request();
																}
															});
								$('#return_date').datepicker({
															showAnim: 'slideDown',
															dateFormat: 'yy-mm-dd',
															beforeShowDay: $.datepicker.noWeekends, 
															onSelect: function(){$('#hours_1').focus(); check_request();}
															});
								$('#return_date').focus(function()
															{
															var dateMin = $('#end_date').datepicker('getDate');
															if(dateMin == null)
																{
																$('#end_date').focus();
																}
															else
																{
																check_request();
																}
															});
								});
				function parseDate(str) {
					var mdy = str.split('-')
					return new Date(mdy[0], mdy[1]-1, mdy[2]);
				}

				function workday_count(dDate1, dDate2) 
				{ // input given as Date objects
					var iWeeks, iDateDiff, iAdjust = 0;
					if (dDate2 < dDate1) return -1; // error code if dates transposed
					var iWeekday1 = dDate1.getDay(); // day of week
					var iWeekday2 = dDate2.getDay();
					iWeekday1 = (iWeekday1 == 0) ? 7 : iWeekday1; // change Sunday from 0 to 7
					iWeekday2 = (iWeekday2 == 0) ? 7 : iWeekday2;
					if ((iWeekday1 > 5) && (iWeekday2 > 5)) iAdjust = 1; // adjustment if both days on weekend
					iWeekday1 = (iWeekday1 > 5) ? 5 : iWeekday1; // only count weekdays
					iWeekday2 = (iWeekday2 > 5) ? 5 : iWeekday2;

					// calculate difference in weeks (1000mS * 60sec * 60min * 24hrs * 7 days = 604800000)
					iWeeks = Math.floor((dDate2.getTime() - dDate1.getTime()) / 604800000)

					if (iWeekday1 <= iWeekday2) {
					  iDateDiff = (iWeeks * 5) + (iWeekday2 - iWeekday1)
					} else {
					  iDateDiff = ((iWeeks + 1) * 5) - (iWeekday1 - iWeekday2)
					}

					iDateDiff -= iAdjust // take into account both days on weekend

					return (iDateDiff + 1); // add 1 because dates are inclusive
				}
				

			function next_step(obj)
				{
				$(obj).attr('disabled',true);
				var o				= active_option;
				//var separate_check	= new Array('', 'Yes', 'No');
				var types			= new Array('', 'Scheduled Vacation', 'Vacation Withdrawal');
				var type			= types[o];
				var _parent			= $('#option_'+o);
				var startdate,enddate, returndate, hours, money, outstanding, unpaid, note, appended_list;//, separate;
				var _points			= $('.review .points table');
				_points.empty();
				var to_append		= "";
				if(o != 2)
					{
					to_append		+= "<tr><td width='35%'>Type of Withdrawal: </td><td>"+type+"</td></tr>";
					}
				unpaid				= $('#cb_unpaid_1').is(':checked');
				note				=	{
										text:		$('#note').val(),
										visible:	!$('#note').is(':hidden'),
										size:		$('#note').size()
										}
				switch(o)
					{
					case 1:
						to_append		+= "\
							<tr><td width='35%'>Vacation Start Date: </td><td>"+format_date($('#start_date').val())+"</td></tr>\
							<tr><td width='35%'>Vacation End Date: </td><td>"+format_date($('#end_date').val())+"</td></tr>\
							<tr><td width='35%'>You Will Be Returning: </td><td>"+format_date($('#return_date').val())+"</td></tr>";
						startdate			= $('#start_date').val();
						enddate				= $('#end_date').val();
						var d_start			= parseDate(startdate);
						var d_end			= parseDate(enddate);
						var days			= workday_count(d_start, d_end);
						returndate			= $('#return_date').val();
						outstanding			= $('#cb_alloutstanding_'+o).is(':checked');
						unpaid				= $('#cb_unpaid_1').is(':checked');
						///////
						hours				= unpaid ? 0 : days*8;
						if(isNaN(hours))
							{
							hours			= 0;
							}
						if(isNaN(money))
							{
							money			= 0;
							}
						
						if((hours != 0 || money != 0) && !outstanding && !unpaid)
							{
							to_append		+= "<tr><td width='35%'>Type Of Payment: </td><td>Vacation Pay</td></tr>";
							if(o == 2)
								{
								to_append		+= "<tr><td width='35%'>Money Requested: </td><td>$"+money.toFixed(2)+"</td></tr>";
								}
							}
						else if(outstanding)
							{
							to_append		+= "<tr><td width='35%'>Type of Payment: </td><td>All Outstanding</td></tr>";
							}
						else if(unpaid)
							{
							to_append		+= "<tr><td width='35%'>Type of Payment: </td><td>Unpaid</td></tr>";
							}
						//if(!unpaid)
						//	{
						//	separate_check		= separate_check[2];//$('#separate_check_1').val()];
						//	}
						var affected_payrolls;
						$.get("./index.aspx", 
							{
							a:				"get_payperiods",
							start_date:		startdate,
							end_date:		enddate
							},
							function(v)
								{
								if(v != 'FAILED' && v != '')
									{
									affected_payrolls	= v.split(",");
									var temp			= "";
									var _count			= 0;
									var _match			= "";
									var _icon			= "";
									//var _m				= $('#alloutstanding_requests').val();
									var _arr			= new Array();
									//if(_m.length > 1)
									//	{
									//	_arr			= _m.split(',');
									//	}
										
									for(var i=0;i<affected_payrolls.length;i++)
										{
										_match		= affected_payrolls[i].match(/^((\w+) \d{2} \d{4}) /);
										_match		= RegExp.$1;
										if(outstanding)
											{
											if($.inArray(_match, _arr) < 0)
												{
												_icon		= "<img align='absmiddle' style='margin:2px;' src='/images/icon/icon[ok].gif'>";
												}
											else
												{
												_icon		= "<img align='absmiddle' style='margin:2px;' src='/images/icon/icon[incomplete].gif'>";
												_count++;
												}
											}
										temp		+= _icon+" "+affected_payrolls[i]+"<br/>";
										}
									to_append		+= "<tr><td valign='top'>Affected Payrolls: </td><td>"+temp+"</td></tr>";
									if(_count > 0)
										{
										to_append		+= "<tr><td colspan='2' align='center' style='color:#f00;'>The marked pay period(s) above already have requests attached to them.<br/> You can only make an all outstanding request on pay periods that do not have pre-existing requests.<br/><br/>Please fix before proceeding.</td></tr>";
										$('#review_submit').attr('disabled', true);
										}
									else
										{
										$('#review_submit').removeAttr('disabled');
										}
								var intersecting_holidays;
								$.get("./index.aspx", 
									{
									a:				"intersecting_holidays",
									start_date:		startdate,
									end_date:		enddate
									},
									function(ve)
										{
										console.log(ve);
										if(ve != 'FAILED' && ve != "")
											{
											intersecting_holidays	= ve.split(",");
											var temp			= "";
											for(var i = 0;i <intersecting_holidays.length;i++)
												{
												temp		+= "<div>"+intersecting_holidays[0]+"</div>";
												}
											to_append			+= "<tr style='background-color:#f00;color:#fff;font-size:1.25em;'><td valign='top'>Holidays during the requested dates: </td><td>"+temp+"</td></tr>";
											to_append			+= "<tr><td colspan='2' style='color:#f00;font-size:1.35em;border:solid 1px #f00;'>You <i>may</i> receive these holiday(s) paid - If so, you don't need to request vacation pay during these holiday(s).<br/> Please talk with your supervisor, first.</td></tr>";
											if(note.size > 0 && note.visible)
												{
												to_append		+= "<tr><td width='35%'>Attached Note:</td><td>"+note.text+"</td></tr>";
												}
											_points.append(to_append);
											if(!unpaid)
												{
												$.get("./index.aspx",
													{
													a:				"check_available",
													date:			startdate,
													},
													function(av)
														{
														to_append		= "<tr><td width='35%'>Hours Requested: </td><td style='font-size:1.75em;'>"+(outstanding ? "All Outstanding" : hours.toFixed(2))+"</td></tr>";
														var bgcolor			= av < hours ? "rgba(255, 0, 0, 0.35)" : "rgba(0, 153, 0, 0.43)";
												console.log(outstanding);
															var warning			= av < hours && !outstanding
																					? "The requested hours are greater than what we are estimating you will have available by this date. <br/><b>You may not receive the requested amount.</b>" 
																					: outstanding ? "" :  "At this point, it appears you should have enough vacation available to honor this request";
													//	to_append			+= "<tr style='background-color:"+bgcolor+";color:#000;font-size:1em;'><td valign='top'>Estimated Hours Available at the <br/> requested start date: </td><td style='font-size:1.75em'>"+av+"<br/><div style='font-size:0.6em'>"+warning+"</div></td></tr>";
														_points.append(to_append);
														});
												}
											$('.schedule_vacation').fadeOut('fast', function(){$('.review').fadeIn('fast')});
											}
										else if(ve == "")
											{
											if(note.size > 0 && note.visible)
												{
												to_append		+= "<tr><td width='35%'>Attached Note:</td><td>"+note.text+"</td></tr>";
												}
												_points.append(to_append);
												if(!unpaid)
													{
													$.get("./index.aspx",
														{
														a:				"check_available",
														date:			startdate,
														},
														function(av)
															{
															to_append		= "<tr><td width='35%'>Hours Requested: </td><td style='font-size:1.75em;'>"+(outstanding ? "All Outstanding" : hours.toFixed(2))+"</td></tr>";
															var bgcolor			= av < hours ? "rgba(255, 0, 0, 0.35)" : "rgba(0, 153, 0, 0.43)";
															var warning			= av < hours  && !outstanding
																					? "The requested hours are greater than what we are estimating you will have available by this date. <br/><b>You may not receive the requested amount.</b>" 
																					: outstanding ? "" : "At this point, it appears you should have enough vacation available to honor this request";
															//to_append			+= "<tr style='background-color:"+bgcolor+";color:#000;font-size:1em;'><td valign='top'>Estimated Hours Available at the <br/> requested start date: </td><td style='font-size:1.75em'>"+av+"<br/><div style='font-size:0.6em'>"+warning+"</div></td></tr>";
															_points.append(to_append+"<tr style='background-color:#fff;color:#000;font-size:1em;'><td style='font-size:1.75em' colspan='2' align='center'>Please check your most recent paystub for your available vacation</td></tr>");
															});
													}
											$('.schedule_vacation').fadeOut('fast', function(){$('.review').fadeIn('fast')});
											}
										else
											{
											alert("There was an error processing this request, please cut a ticket.");
											$(obj).removeAttr('disabled');
											}
										});
									}
								else
									{
									alert("There was an error processing this request, please cut a ticket.");
									$(obj).removeAttr('disabled');
									}
								});
					break;
					case 2:
						hours				= parseFloat($('#hours_'+o).val());
						money				= parseFloat($('#money_'+o).val());
						outstanding			= $('#cb_alloutstanding_'+o).is(':checked');
						var hours_str		= "";
						if(isNaN(hours))
							{
							hours			= 0;
							}
						if(isNaN(money))
							{
							money			= 0;
							}
						if(hours != 0 || money != 0)
							{
							to_append		+= "<tr><td width='35%'>Type of Payment: </td><td>Vacation Pay</td></tr>";
							to_append		+= "<tr><td width='35%'>Money Requested: </td><td>$"+money.toFixed(2)+"</td></tr>";
							to_append		+= "<tr><td width='35%'>Hours Requested: </td><td>"+hours.toFixed(2)+"</td></tr>";
							hours_str		= hours.toFixed(2);
							}
						else if(outstanding)
							{
							hours_str		= "All Outstanding";
							}
						//separate_check		= separate_check[2];//$('#separate_check_2').val()];
						var affected_payrolls;
						$.get("./index.aspx", 
							{
							a:				"get_payperiod"
							},
							function(v)
								{
								to_append		+= "<tr><td width='35%'>Applicable Pay Period(s): </td><td>"+v+"</td></tr>";
								if(note.text.length > 0 && note.visible)
									{
									to_append		+= "<tr><td width='35%'>Attached Note:</td><td>"+note.text+"</td></tr>";
									}
								_points.append(to_append);
								$.get("./index.aspx",
									{
									a:				"check_available",
									date:			"today",
									},
									function(av)
										{
										to_append		= "<tr><td width='35%'>Hours Requested: </td><td style='font-size:1.75em;'>"+hours_str+"</td></tr>";
										var bgcolor			= av < hours ? "rgba(255, 0, 0, 0.35)" : "rgba(0, 153, 0, 0.43)";
											var warning			= av < hours && !outstanding
																	? "The requested hours are greater than what we are estimating you have available. <br/><b>You may not receive the requested amount.</b>" 
																	: outstanding ? "" : "At this point, it appears you should have enough vacation available to honor this request";
										//to_append			+= "<tr style='background-color:"+bgcolor+";color:#000;font-size:1em;'><td valign='top'>Estimated Hours Available: </td><td style='font-size:1.75em'>"+av+"<br/><div style='font-size:0.6em'>"+warning+"</div></td></tr>";
										_points.append(to_append);
										$('.schedule_vacation').fadeOut('fast', function(){$('.review').fadeIn('fast')});
										});
								});
					break;
					}
				}
				

			function toggle_option(opt, obj)
				{
				var targ;
				var e				= window.event;
				if(e.target)
					{
					targ			= e.target;
					}
				else if (e.srcElement)
					{
					targ			= e.srcElement
					}
				if(targ.nodeName != 'INPUT' || (targ.nodeName == 'INPUT' && $(targ).parents('.option:eq(0)').find('input:hidden:first').val() != 'true'))
					{
					var alt_opt;
					switch(opt)
						{
						case 1: alt_opt		= 2;
						break;
						case 2: alt_opt		= 1;
						break;
						}
					active_option				= opt;
					$('#option_'+opt).parent().find('input:hidden:first').val('true');
					$('#option_'+alt_opt).parent().find('input:hidden:first').val('false');
					$('#option_'+alt_opt).find('*').each(function(){$(this).attr('disabled', true)});
					$('#option_'+alt_opt).parent().find('input:checkbox').removeAttr('checked');
					//$('#option_'+alt_opt).find('#separate_check_'+alt_opt).val(0);
					$('#option_'+alt_opt).fadeTo('fast', .5);
					$('#option_'+alt_opt).parent().css({'border-color': '#ddd'});
					$('#option_'+alt_opt).parent().children('.option_head_wrapper').css({'background-color':'#ddd'});
					$('#option_'+alt_opt).parent().find('.option_head').css({'color':'#aaa'});
					$('#option_'+alt_opt).parent().find('.option_head_sub').css({'color':'#aaa'});
					$('#option_'+alt_opt).parent().hover(function()
															{
															if($(this).css('border-color') != '#7ac')
																{
																$(this).css({'border-color': '#bcd'})
																}
															},
														function()
															{
															if($(this).css('border-color') == '#bcd')
																{
																$(this).css({'border-color': '#ddd'})
																}
															});
					$('#option_'+alt_opt).find('input').each(function(){$(this).val("")});
					$('#option_'+opt).find('*').each(function(){$(this).removeAttr('disabled')});
					$('#option_'+opt).fadeTo('fast', 1);
					$('#option_'+opt).parent().children('.option_head_wrapper').css({'background-color':'#7ac'});
					$('#option_'+opt).parent().find('.option_head').css({'color':'#fff'});
					$('#option_'+opt).parent().find('.option_head_sub').css({'color':'#fff'});
					$('#option_'+opt).parent().css({'background-color': '#bcd'});
					$('#option_'+opt).parent().css({'border-color': '#7ac'});
					$('#option_'+alt_opt).parent().css({'background-color': '#fff'});
					$(targ).focus();
					check_request();
					}
				}

			function payment_type_toggle(obj)
				{
				var this_id			= $(obj).attr('id');
				var alt_cb			= new Array();
				var is_outstanding	= this_id.match(/oustanding/g);
				switch(this_id)
					{
					case "cb_alloutstanding_1": 
						alt_cb		= ["cb_unpaid_1"];
					break;
					case "cb_unpaid_1":			
						alt_cb		= ["cb_alloutstanding_1"];
					break;
					}
				for(var i = 0;i <= alt_cb.length;i++)
					{
					$('#'+alt_cb[i]).attr('checked', false);
					}
				//$('#separate_check').removeAttr('disabled');
				$('#hours_2').val("");
				$('#money_2').val("");
				check_request();
				}

			function payment_amount_toggle()
				{
				var method					= $("#payment_method").val();
				var total_hours				= $("#total_hours");
				var pay_rate				= parseFloat($("#pay_rate").val());
				var hours					= parseFloat(total_hours.val());
				var available				= parseFloat($("#available_vacation").val());
				var amount					= $("#amount");
				switch(method)
					{
					case "2":	if(pay_rate * hours > 350)
								{
								//$('.separate_check').show();
								}
							else
								{
								//$('.separate_check input:checkbox').attr('checked', false);
								//$('.separate_check').hide();
								}
								
							amount.css({"display": "inline"});
							if(total_hours.val() > available)
								{
								amount.val(available);
								}
							else
								{
								amount.val(total_hours.val());
								}
							$("#amount_type").text("hr(s)");
						//	calculate_money();
					break;
					case "3":if(pay_rate * available > 350)
								{
								//$('.separate_check').show();
								}
							else
								{
								//$('.separate_check input:checkbox').attr('checked', false);
								//$('.separate_check').hide();
								}
							$("#amount").attr("value", "");
							$("#amount").css({"display": "none"});
							$("#amount_type").html("");
							$(".money").html("");
					break;
					default:	$("#amount").attr("value", "");
								$("#amount").css({"display": "none"});
								$("#amount_type").html("");
								$(".money").html("");
								//$('.separate_check input:checkbox').attr('checked', false);
								//$('.separate_check').hide();
					break;
					}
				}


			//function separate_check()
			//	{
			//	var pay_rate		= parseFloat($("#pay_rate").val());
			//	var hours			= parseFloat($("#amount").val());
			//	if(pay_rate * hours > 350)
			//		{
			//		$('.separate_check').show();
			//		}
			//	else
			//		{
			//		$('.separate_check input:checkbox').attr('checked', false);
			//		$('.separate_check').hide();
			//		}
			//	}

				
			function toggle_notes(vacation_id)
				{
				$("#make_note_"+vacation_id).slideToggle("fast", function(){if($("#notes_"+vacation_id).children().size() > 0){$("#notes_"+vacation_id).slideToggle("fast")}else{$("#notes_"+vacation_id).show();}});
				}


			function check_amount()
				{
				var hours			= parseFloat($("#available_vacation").attr("value"));
				var current_amount	= parseFloat($("#amount").attr("value"));
				//calculate_money();
	
				if(current_amount > hours)
					{
					$("#amount").attr("value", "");
					$(".money").html("");
					}
				}


			function roundNumber(num, dec)
				{
				var result			= Math.round( Math.round( num * Math.pow( 10, dec + 1 ) ) / Math.pow( 10, 1 ) ) / Math.pow(10,dec);
				return result.toFixed(2);
				}


			function calculate_money(obj, which, max)
				{
				var pay_rate		= parseFloat($("#pay_rate").val());
				var decision		= false;
				if(!isNaN($(obj).val()) && $(obj).val() > -1)
					{
					var hours			= parseFloat($(obj).val());
					if(hours > 0 && which == '1')
						{
						$('#available_note').fadeIn();
						}
					else
						{
						$('#available_note').fadeOut();
						}
					if(hours)
						{
						var end_amount		= pay_rate * hours;
						$("#money_"+which).val(roundNumber(end_amount, 2));
						$('#cb_alloutstanding_'+which).removeAttr('checked');
						$('#cb_unpaid_1').removeAttr('checked');
						decision			= true;
						}
					else
						{
						$("#money_"+which).val("");
						$('#cb_alloutstanding_'+which).removeAttr('checked');
						decision			= false;
						}
					}
				else
					{
					$(obj).val("");
					$("#money_"+which).val("");
					decision			= false;
					}
				check_request();
				return decision;
				}

				
			function calculate_hours(obj, which)
				{
				var pay_rate		= parseFloat($("#pay_rate").val());
				var money			= parseFloat($(obj).val());
				var decision		= false;
				if(!isNaN($(obj).val()) && $(obj).val() > -1)
					{
					if(money > 0 && which == '1')
						{
						$('#available_note').fadeIn();
						}
					else
						{
						$('#available_note').fadeOut();
						}
					if(money)
						{
						var end_amount		= money / pay_rate;
						$("#hours_"+which).val(roundNumber(end_amount, 2));
						$('#cb_alloutstanding_'+which).removeAttr('checked');
						$('#cb_unpaid_1').removeAttr('checked');
						decision			= true;
						}
					else
						{
						$("#hours_"+which).val("");
						$('#cb_alloutstanding_'+which).removeAttr('checked');
						decision			= false;
						}
					}
				else
					{
					$(obj).val("");
					$("#hours_"+which).val("");
					decision				= false;
					}
				check_request();
				return decision;
				}


			function format_date(str)
				{
				var _date			= new Date(str.replace(/^(\d{4})-(\d{2})-(\d{2})$/g, '$2/$3/$1'));
				var _months			= ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
				var _days			= ["Sunday", "Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"];
				var _return			= _days[_date.getDay()]+", "+_months[_date.getMonth()]+" "+_date.getDate()+", "+_date.getFullYear();
				return _return;
				}

				
			//function unpaid(obj)
			//	{
			//	if($(obj).is(':checked'))
			//		{
			//		$('#separate_box').slideUp('fast');
			//		}
			//	else
			//		{
			//		$('#separate_box').slideDown('fast');
			//		}
			//	}

			function alloutstanding_available(start_date)
				{
				$.get('./index.aspx',	{a:	"all_outstanding_available_for",start_date:	start_date},function(v)	{if(v != 'FAILED'){return v;}});
				}
				


				
			function prevstep()
				{
				$('.review').fadeOut('fast', function(){$('.schedule_vacation').fadeIn('fast', function(){check_request();})});
				}

				
			function check_request()
				{
				var available		= parseFloat($("#available_vacation").val());
				var start_date		= $("#start_date").val();
				var end_date		= $("#end_date").val();
				var return_date		= $("#return_date").val();
				var total_hours		= $("#total_hours").val();
				var check			= 0;

				var o				= active_option;
				var target			= $('#option_'+o);
				var	tar_hours		= target.find('#hours_'+o).val();
				var tar_money		= target.find('#money_'+o).val();
				var tar_outstanding	= target.find('#cb_alloutstanding_'+o).is(':checked');
				var tar_unpaid		= false;
				if(o == 1)
					{
					tar_unpaid		= target.find('#cb_unpaid_'+o).is(':checked');
					}
				if(o == 1 && (start_date == "" || end_date == "" || return_date == ""))
					{
					check++;
					}
				if(o == 2 && (((tar_hours == '' && tar_money == '') || (parseFloat(tar_hours) <= 0 || parseFloat(tar_money) <= 0)) && !tar_outstanding))
					{
					check++;
					}
				console.log(check);
				if(check == 0)
					{
					$("#submit").removeAttr("disabled");
					}
				else
					{
					$("#submit").attr("disabled", true);
					}
				}


			function check_note(vacation_id)
				{
				if(!vacation_id)
					{
					var note			= $("#notes").attr("value");
					if(note.length > 0)
						{
						$("#add_note").attr("disabled", false);
						return true;
						}
					else
						{
						$("#add_note").attr("disabled", true);
						return false;
						}
					}
				else
					{
					var note			= $("#note_"+vacation_id).attr("value");
					if(note.length > 0)
						{
						$("#add_note_"+vacation_id).attr("disabled", false);
						return true;
						}
					else
						{
						$("#add_note_"+vacation_id).attr("disabled", true);
						return false;
						}
					}
				}


			function add_note(vacation_id)
				{
				var note				= escape($("#note_"+vacation_id).val());
				var url					= "/sections/member/vacation/?a=note-add&vacation_id="+vacation_id+"&note="+note;
				$("#note_"+vacation_id).attr("value", "");
				$("#view_notes_"+vacation_id).css({'background-color':'#0f0'});
				$("#view_notes_"+vacation_id).attr('title', 'view/post notes');
				$("#notes_"+vacation_id+" > i").remove();
				$.ajax({
				   type: "GET",
				   url: url,
				   success: function(msg)
								{
								switch(msg)
									{
									case "SUCCESS":		$("#notes_"+vacation_id).append("<div class='past_note'><div class='name_plate'>You just added</div><div class='note_text'>"+unescape(note)+"</div></div>", function(){$(this).fadeIn("slow");});
									break;
									case "FAILED":		alert("Could not submit note, get Matt");
									break;
									default:			alert(msg);
									break;
									}
								 },
					error: function(msg)
								{
								alert(msg);
								}									
					});
				}


			function submit_request()
				{
				var vv					= {};
				//var $.vv.o, $.vv.start_date, $.vv.end_date, $.vv.return_date, $.vv.hours, $.vv.money, $.vv.unpaid, $.vv.alloutstanding, $.vv.separate_check, $.vv.note;
				vv.unpaid				= false;
				vv.alloutstanding		= false;
				vv.o					= active_option;
				switch(vv.o)
					{
					case 1:		vv.start_date			= $('#start_date').val();
								vv.end_date			= $('#end_date').val();
								vv.return_date		= $('#return_date').val();
								vv.money				= 0;
								vv.hours				= 0;
								vv.alloutstanding		= $('#cb_alloutstanding_'+vv.o).is(':checked');
								vv.unpaid				= $('#cb_unpaid_'+vv.o).is(':checked');
								//$.vv.separate_check		= $('#separate_check_1').val();
								
					break;
					case 2:		vv.hours				= $('#hours_'+vv.o).val();
								vv.money				= $('#money_'+vv.o).val();
								vv.alloutstanding		= $('#cb_alloutstanding_'+vv.o).is(':checked');
								//$.vv.separate_check		= $('#separate_check_2').val();
					break;
					default:	alert("You shouldn't be seeing this!");
					break;
					}
				if(vv.hours == '')
					{
					vv.hours			= 0;
					}
				if(vv.money == '')
					{
					vv.money			= 0;
					}
				vv.note			= escape($('#note').val());
				//if($.vv.separate_check == 2)
				//	{
				//	$.vv.separate_check	= 0;
				//	}
				$.get('./index.aspx',
						{
						a:				'submit',					
						o:				vv.o,						
						start_date:		vv.start_date,			
						end_date:		vv.end_date,				
						return_date:	vv.return_date,			
						unpaid:			vv.unpaid,				
						hours:			vv.hours,					
						money:			vv.money,					
						alloutstanding:	vv.alloutstanding,		
						note:			vv.note					
						},
						function(res)
							{
							if(res != 'SUCCESS')
								{
								alert(res);
								}
							else
								{
								location.href	= "./index.aspx";
								}
							});
				}


			function cancel_request(vacation_id)
				{
				var url			= "/sections/member/vacation/index.aspx?a=cancel&vacation_id="+vacation_id;
				$.ajax({
				   type: "GET",
				   url: url,
				   success: 
					function(msg)
						{
						switch(msg)
							{
							case "SUCCESS":	
								location.href = location.href;
							break;
							case "FAILED":	alert("Your Vacation Request could not be cancelled");
							break;
							}
						 }
				 });
				}