$.fn.tip = function(_override)
	{
	var _defaults	=	{
						title:			"NOTE",
						set_x:			10,
						set_y:			5,
						width:			350,
						disable_offset:	false,
						show_notes:		false,
						notes_id:		0,
						pre:			false,
						scrollable:		false,
						height:			350,
						fixed_height:	false,
						note_handler:	function(){}
						}
	var _s			= $.extend({}, _defaults, _override);
	$(this).unbind('mousemove').unbind('mouseover').unbind('mouseout').unbind('blur').css('cursor','pointer');
	_s.title		= $(this).attr('data-title') ? $(this).attr('data-title') : _s.title;
	_s.width		= $(this).attr('data-width') ? $(this).attr('data-width') : _s.width;
	var _t			= ($(this).find('.tip').size() == 0);
	var tooltip		= _t ? $("#tooltip_window") : $(this).find('.tip');
	var global_tip	= _t ? $(this).attr("data-tooltip") : $(tooltip).html();
	if($("#tooltip_window").size() == 0 && global_tip != undefined)
		{
		var this_style		= "";
		if(_s.pre)
			{
			this_style		+= "white-space:pre;word-wrap:break-word;";
			}
		if(_s.scrollable)
			{
			this_style		+= "overflow-y:scroll;";
			}
		$("body").append("<div id='tooltip_window' class='tip' style='"+this_style+"'></div>");
		}
	else if(global_tip == undefined)
		{
		$("body").append("<div id='tooltip_window' class='tip_sm'></div>");
		_s.width		="auto";
		}
	if(global_tip == undefined)
		{
		global_tip		= "";
		}
		
	$(this).mousemove(function(e)
		{
		// Only responsible for moving the tooltip... nothing else.
		var _x				= 0;
		var _y				= 0;
		if(!_s.disable_offset)
			{
			var isOffVert	= (eval(e.pageY+tooltip.height()) > $(window).height());
			_x			= eval(e.pageX+tooltip.width()) > $(window).width() 
							? e.pageX-tooltip.width()-(2*_s.set_x) 
							: e.pageX + _s.set_x;
			_y			= isOffVert 
							? e.pageY - tooltip.height() - _s.set_y 
							: e.pageY + _s.set_y;
			if(_y < 0)
				{
				_y = $(this).offset().top + _s.set_y;
				}
			if(_x < 0)
				{
				_x = $(this).offset().left + _s.set_x;
				}
			}
		else
			{
			_x			= e.pageX + _s.set_x;
			 _y			= e.pageY + _s.set_y;
			}
		if(!e.ctrlKey)
			{
			tooltip.css( 
				{
				'top'	: _y + 'px',
				'left'	: _x + 'px'
				});
			}
		});			
	$(this).mouseover(function(e)
		{
		// Responsible for showing the tooltip and it's initial position & width
		if(tooltip.size() == 0)
			{
			tooltip		= $("#tooltip_window");
			}
		var do_tip		= $(this).attr("data-dotip") == null ? true : $(this).attr("data-dotip") == "true";
		if(!do_tip) return;
		var html		= global_tip != "" 
							? _s.title == "FALSE"
								? "<div>"+global_tip+"</div>"
								: "<b>"+_s.title+"</b><div>"+global_tip+"</div>" 
							: "<b>"+_s.title+"</b>";
		if(_s.show_notes)
			{
			html		+= "<div id='tip_notes' style='max-height:300px;overflow-y:scroll;' align='center'></div>";
			}
		tooltip.html(html);
		if(_s.show_notes && _s.notes_id != 0)
			{
			var query_string		=	{
										a:			"xml_get_notes",
										woprog_id:	_s.notes_id,
										business_unit_id:	0
										};
			var _notes_html			= "<div align='center'>Hold the 'ctrl' button on the keyboard, while moving the mouse, to lock this tooltip.</div>";
			$.ajax({
					type:		"GET",
					cached:		false,
					url:		"./wo_prog_edit.aspx",
					data:		query_string,
					dataType:	"xml",
					error:		function (xhr, ajaxOptions, thrownError)
									{
									alert(thrownError);
									},
					beforeSend:	function()
									{
									$("#tip_notes").html("<hr/><img src='/images/loading_panel.gif' />");
									},
					success:	function(xml)
									{
									if($(xml).find("note").size() > 0)
										{
										$(xml).find('note').each(
											function()
												{
												var this_text		= $(this).children("note_text").text().replace("\n", "<br/>");
												var this_who		= $(this).children("who").text();
												var this_dt			= $(this).children("dt").text();
												var this_type		= $(this).children("type").text();
												var this_number		= $(this).children("number").text();
												_notes_html			+= "<div align='left' style='font-weight:bold;font-size:10px;border-bottom:solid 1px #ccc;background-color:#777;'>"+this_type+"#: "+this_number+"<br/>"+this_who+" @ "+this_dt+"</div><div align='left' style='padding-left:3px;background-color:#555'>"+this_text+"</div>";
												});
										}
									else
										{
										_notes_html				= "<hr/><br/>No notes available<br/><br/>";
										}
									},
					complete:	function()
									{
									$("#tip_notes").html(_notes_html);
									}});
			}
		var _x			= eval(e.pageX+tooltip.width()) > $(window).width() 
							? e.pageX-tooltip.width()-(2*_s.set_x) 
							: e.pageX + _s.set_x;
	//	console.log("e.pageX:"+e.pageX);
	//	console.log("_x:"+_x);
	///	console.log("_s.set_x:"+_s.set_x);
		//console.log("tooltip.width:"+tooltip.width());
		//console.log("window.width:"+$(window).width());
		var _y			= e.pageY + _s.set_y;
		if(_y < 0)
			{
			_y = $(this).offset().top + _s.set_y;;
			}
		if(_x < 0)
			{
			_x = $(this).offset().left + _s.set_x;
		//console.log("_x changed to "+_x);
			}
	//	console.log(_s.disable_offset);
		var _css		=	{
							'width'	: _s.width + 'px',
							'top'	: _y + 'px',
							'left'	: _x + 'px'
							};
	//	console.log(_css);
		if(_s.fixed_height)
			{
			_css["height"]	= _s.height+"px";
			}
		tooltip.css(_css);
		if(global_tip != "" || (_s.title != "" && global_tip == ""))
			{
			tooltip.show(0);
			if(_s.note_handler != null)
				{
				_s.note_handler();
				}
			}
		});
			
	$(this).mouseout(function(e)
		{
		if(!e.ctrlKey)
			{
			tooltip.html(global_tip);
			tooltip.hide(0);
			}
		});
			
	$(this).blur(function()
		{
		tooltip.html(global_tip);
		tooltip.hide(0);
		});
	$(document).keyup(function(e)
		{
		if(e.which == 27)
			{
			tooltip.hide();
			}
		});
	}

