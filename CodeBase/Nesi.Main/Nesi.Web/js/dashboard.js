
	$('document').ready(function()
		{
		$(".gauge b").each(function()
			{
			var tar				= null;
			var _cbp_name		= $(this).parents(".gauge:first").attr("data-cbp");
			var _iframe_name	= $(this).parents(".gauge:first").attr("data-iframe");
			try
				{
				tar			= _cbp_name;
				}
			catch(ee)
				{
				_tar		= null;
				}
			var _type		= $(this).parents(".gauge:first").attr("data-type");
			var _level		= $(this).parents(".gauge:first").attr("data-level");
			var _clickable	= $(this).parents(".gauge:first").attr("data-clickable");
			if(_clickable != "false")
				{
				$(this).css({"cursor":"pointer"});
				$(this).click(function()
					{
					toggle_pane(_type, _level);
					});
				}
			});
		});
	function switch_company(obj)
		{
		$.get("./Dashboard.aspx", {	a:			"switch_company",
									business_unit_id:	$(obj).val()
									},
									function(resp)
										{
										if(resp == "SUCCESS")
											{
											$(obj).find("option").each(function()
												{
												var t		= $(this).text().trim();
												if(t.indexOf(">>") >= 0)
													{
													$(this).text(t.substring(2, t.length));
													}
												if($(this).attr("selected"))
													{
													$(this).text(">> "+t);
													}
												});
											load_gauges(true);
											division_switch.PerformCallback();
											}
										else
											{
											alert(resp);
											}
										});
		}
	function switch_division(s,e)
		{
		$.get("./Dashboard.aspx", {	a:				"switch_division",
									division_id:	s.GetValue()
									},
									function(resp)
										{
										if(resp == "SUCCESS")
											{
											$(s.mainElement).find("option").each(function()
												{
												var t		= $(this).text().trim();
												if(t.indexOf(">>") >= 0)
													{
													$(this).text(t.substring(2, t.length));
													}
												if($(this).attr("selected"))
													{
													$(this).text(">> "+t);
													}
												});
											load_gauges(true);
											}
										else
											{
											alert(resp);
											}
										});
		}
	function toggle_pane(t, level)
		{
	    getOneLessThanTop(parent).window.location.hash = '';
	    getOneLessThanTop(parent).window.location.hash = '#dd';
	    var obj = getOneLessThanTop(parent).frames['drilldown'].window["drilldown"];
		obj.PerformCallback(level);
	}
	function getOneLessThanTop(obj) {
			if (obj.parent != top)
				return obj.parent;
			return obj;
	}
	function handle_if_load(obj)
		{
		if($(obj).attr("src") != "")
			{
			$(obj).parents("div:first").css({"background-image":"","border":""});
			$(obj).fadeIn(100);
			}
		}
	function load_if(id, href, just_reload)
		{
		var target		= $("#"+id);
		target.parents("div:first").css({"background-image":"url('/images/indicator.gif')", "border":"dashed 1px #ccc","border-radius":"5px"});
		target.hide(0);
		if(just_reload)
			{
			document.getElementById(id).contentDocument.location.href = document.getElementById(id).contentDocument.location.href;;
			}
		else
			{
			get_ifdata(target, href);
			}
		}
	function get_ifdata(target,href)
		{
		    target.attr("src", href);					
		}
	function customize_toggle(obj, show)
		{
		var has_button		= $(obj).find(".customize").size() > 0;
		if(show)
			{
			if(has_button)
				{
				$(obj).find(".customize").css({"display":"inline"});
				}
			else
				{
				$(obj).append("<img src='/images/icon/icon[browse].gif' class='customize' title='Customize Threshold' onclick='customize(this)' width='16' height='16'/>");
				}
			}
		else
			{
			$(obj).find(".customize").css({"display":"none"});
			}
		}
	function customize(obj)
		{
		var _parent			= $(obj).parents(".tile");
		var _title			= $(obj).parents("tr:first").prev("tr").find(".subtitle").text();
		var type			= $(obj).parents("table:first").attr("data-level");
		var subtype			= $(obj).parents(".gauge").attr("data-level");
		var good			= $(obj).parents(".val").attr("data-good");
		var bad				= $(obj).parents(".val").attr("data-bad");
		var _min			= 0;
		var _max			= type == "hourutilization" ? 100 : type == "customers" ? 2500 : 10000000;
		var _step			= type == "hourutilization" ? 5 : type == "customers" ? 50 : 10000;
		var _badvalue		= type == "hourutilization" ? bad + "%" : type == "customers" ? bad + " Customers" : "$"+bad.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
		var _goodvalue		= type == "hourutilization" ? good + "%" : type == "customers" ? good + " Customers"  : "$"+good.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
		var height			= _parent.height()+10;
			$("body").append("\
			<div id='customize_window' data-type='"+type+"' data-subtype='"+subtype+"'>\
				<div class='subtitle'>\
					<b>"+_title+"</b><br/>\
					Thresholds\
				</div>\
				<center>\
				<b class='b'>Bad Threshold</b>\
				<div id='threshold_bad' class='threshold'></div>\
				<div id='amount_bad' value='' class='amount'>"+_badvalue+"</div>\
				<input type='hidden' id='hid_bad' value='"+bad+"' />\
				<b class='b'>Good Threshold</b>\
				<div id='threshold_good' class='threshold'></div>\
				<div id='amount_good' value='' class='amount'>"+_goodvalue+"</div>\
				<input type='hidden' id='hid_good' value='"+good+"' />\
				</center>\
				<div class='buttons'>\
					<button type='button' onclick='hide_customize()'>Close</button>\
					<button type='button' onclick='save_customize()'>Save</button>\
				</div>\
			</div>");
			$("#threshold_bad").slider(
				{
				animate:	true, 
				range:		"max",
				min:		_min,
				max:		_max,
				value:		bad,
				step:		_step,
				slide:		function(event, ui)
								{
								var type			= $("#customize_window").attr("data-type");
								$("#hid_bad").val(ui.value);
								if(type != "hourutilization" && type != "margin" && type != "salespipeline")
									{
									$("#amount_bad").text(ui.value + "%");
									}
								else if(type == "customers")
									{
									$("#amount_bad").text(ui.value + " Customers");
									}
								else
									{
									$("#amount_bad").text( "$"+ui.value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") );
									}
								}
				});
			$("#threshold_good").slider(
				{
				animate:	true, 
				min:		_min,
				max:		_max,
				value:		good,
				step:		_step,
				slide:		function(event, ui)
								{
								var type			= $("#customize_window").attr("data-type");
								$("#hid_good").val(ui.value);
								if(type != "hourutilization" && type != "margin" && type != "salespipeline")
									{
									$("#amount_good").text(ui.value + "%");
									}
								else if(type == "customers")
									{
									$("#amount_good").text(ui.value + " Customers");
									}
								else
									{
									$("#amount_good").text( "$"+ui.value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") );
									}
								}
				});
		}
	function hide_customize()
		{
		$("#customize_window").remove();
		}
	function save_customize()
		{
		var o		=	{
						a:			"set_threshold",
						type:		$("#customize_window").attr("data-type"),
						subtype:	$("#customize_window").attr("data-subtype"),
						good:		$("#hid_good").val() / 1,
						bad:		$("#hid_bad").val() / 1
						};
		if(o.good < o.bad)
			{
			alert("You can't have the bad threshold greater than the good threshold.");
			return;
			}
		if(o.good == o.bad)
			{
			alert("You can't have the bad & good thresholds equal to each other.");
			return;
			}
		$.get("/dashboard.aspx", o, function(resp)
			{
			if(resp == "SUCCESS")
				{
				location.href	= location.href;
				}
			else
				{
				alert(resp);
				}
			});
		}