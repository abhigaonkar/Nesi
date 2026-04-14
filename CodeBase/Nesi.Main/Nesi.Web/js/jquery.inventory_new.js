//-------------------------------------------------------------------------------------------------
(
function($)
	{
	var sc_settings, sc_css				= {};
	$.fn.inventory = function(settings_override)
		{
		var settings_defaults			=	{
											parent_node:			this,
											business_unit_id:				null,
											vendor_id:				null,
											x:						[null, null, null],			// For cancelling ajax calls -- Leave this property alone.
											x_start:				[null, null, null],
											x_end:					[null, null, null],
											o_load:					[0,0,0],
											force_clickable:		false,
											origin:					null,			// Added for picklist purposes... only used there.
											allow_gl:				true,			// If this is set to false, any part with a tag id of 843 will not be returned
											allow_nonstock:			true,			// If this is set to false, any part with a tag id of 673 will not be returned
											show_nosell:			true,			// If this is set to false, any part without at least a global sell will not be returned
											click:
												function(row)
													{
													$(sc_settings.parent_node).val(row.master_id);
													$(sc_settings.parent_node).focus();
													},
											zindex:					9999,
											key_up:					function(e){return true;},
											key_down:				function(e){return true;},
											parts_header:			"\
											<br/>\
											<table class='tablesorter' cellpadding='2' cellspacing='0' style='font-family:arial;font-size:11px;'>\
												<thead>\
													<tr>\
														<th width='16' style='background-color:#4682b4 !important;color:#fff !important;'>&nbsp;</th>\
														<th width='180' style='background-color:#4682b4 !important;color:#fff !important;'>Description</th>\
														<th width='60' style='background-color:#4682b4 !important;color:#fff !important;'>Part #</th>\
														<th width='55' style='background-color:#4682b4 !important;color:#fff !important;'>Cost</th>\
														<th width='55' style='background-color:#4682b4 !important;color:#fff !important;'>Sell</th>\
														<th width='170' style='background-color:#4682b4 !important;color:#fff !important;'>Last<br/>Purchased</th>\
														<th width='75' style='background-color:#4682b4 !important;color:#fff !important;'>Internal OnHand</th>\
                                                        <th width='75' style='background-color:#4682b4 !important;color:#fff !important;'>External OnHand</th>\
														<th width='40' style='background-color:#4682b4 !important;color:#fff !important;'>Total Internal OnHand</th>\
														<th width='40' style='background-color:#4682b4 !important;color:#fff !important;' >Total External OnHand</th>\
													</tr>\
												</thead>\
												<tbody>",
											parts_body:				""
											};
		if(!settings_override)
			{
			settings_override			= {};
			}
		sc_settings					= $.extend({}, settings_defaults, settings_override);
		if(!$(this).is('input:text'))
			{
			sc_settings.parent_node		= $(this).find('input:text');
			}
		var offset						= $(sc_settings.parent_node).offset();
		var height						= $(sc_settings.parent_node).height();
		var width						= $(sc_settings.parent_node).width();
		var padding						= 0;
		var _float						= "'left'";
		var _float_xy					= "-153px";
		if(width > 153)
			{
			padding						= 205;
			_float						= "'left'";
			_float_xy					= "-153px";
			}
			
        var origin_x_offset				= sc_settings.origin=="purchaseorder" ? 200 :  offset.left - 745 + width;
		
		var _attributes_css, _container_css, _tabs_css;
		_attributes_css				= { 'name'				: '.inv_attributes',
										'border'			: 'solid 1px #999',
										'display'			: 'none',
										'text-align'		: 'center',
										'width'				: '100%'};
		_tabs_css					= { 'name'				: '.inv_tabs',
										'border'			: '0',
										'padding-top'		: '5px',
										'width'				: '225px',
										'position'			: 'absolute',
										'background-color'	: 'transparent',
										'margin-top'		: '-22px'};
		_container_css				= { 'name'				: '.inv_container',
										'border'			: '0',
										'position'			: 'absolute',
										'top'				: offset.top + height + 6+"px",
										'left'				: origin_x_offset+"px",
										'height'			: 'auto',
										'background-color'	: 'transparent',
										'width'				: '757px',
										'box-shadow'		: '0px 0px 5px #000',
										'z-index'			: sc_settings.zindex};
		if((offset.left - 757) < 0)
			{
			_tabs_css.right				= "0px";
			_container_css.top			= offset.top + height + 4+"px";
			_container_css.left			= offset.left+"px";
			}
		if (navigator.userAgent.indexOf('MSIE') !=-1)
			{
			_tabs_css["margin-top"]	= "-22px";
			}
		var criteria_defaults		= { 'name'				: '.inv_criteria',
										'border-top'		: 'solid 1px #999',
										'border-left'		: 'solid 1px #999',
										'border-right'		: 'solid 1px #999',
										'border-bottom'		: 'dashed 1px #ddd',
										'background-color'	: '#fff',
										'font-family'		: 'arial',
										'font-weight'		: 'bold',
										'padding'			: '4px',
										'font-size'			: '12px'};

		var results_defaults		= { 'name'					: '.inv_results',
										'border-bottom'			: 'solid 1px #999',
										'border-left'			: 'solid 1px #999',
										'border-right'			: 'solid 1px #999',
										'background-color'		: '#fff',
										'font-family'			: 'arial',
										'padding'				: '5px',
										'overflow-y'			: 'hidden',
										'overflow-x'			: 'hidden',
										'background-image'		: "url(/images/foundation/bg/bg[main].png)",
										'background-position'	: 'bottom left',
										'background-repeat'		: 'repeat-x',
										'clear'					: 'left',
										'height'				: 'auto'};

		var attributes_defaults		= _attributes_css;
		var tabs_defaults			= _tabs_css;
		var container_defaults		= _container_css;
		sc_css.container			= $.extend({}, container_defaults);
		sc_css.tabs					= $.extend({}, tabs_defaults);
		sc_css.results				= $.extend({}, results_defaults);
		sc_css.attributes			= $.extend({}, attributes_defaults);
		sc_css.criteria				= $.extend({}, criteria_defaults);
		sc_css.active_tab			= {	'name'				: '.inv_active_tab',
										'border-top'		: 'solid 1px #ccc',
										'border-left'		: 'solid 1px #ccc',
										'border-right'		: 'solid 1px #ccc',
										'border-bottom'		: 'solid 1px #ccc',
										'height'			: '20',
										'font-weight'		: 'bold',
										'disabled'			: 'true',
										'background-color'	: '#eee',
										'color'				: '#4682b4',
										'font-size'			: '11px',
										'margin-left'		: '1px',
										'width'				: '100px'};
		sc_css.inactive_tab			= { 'name'				: '.inv_inactive_tab',
										'border-top'		: 'solid 1px #ccc',
										'border-left'		: 'solid 1px #ccc',
										'border-right'		: 'solid 1px #ccc',
										'border-bottom'		: 'solid 1px #ccc',
										'height'			: '20',
										'font-weight'		: 'bold',
										'disabled'			: 'false',
										'cursor'			: 'pointer',
										'background-color'	: '#fff',
										'color'				: '#bbb',
										'font-size'			: '11px',
										'margin-left'		: '1px',
										'width'				: '100px'};
		sc_css.options_panel		= {
										'name'				: '.inv_options_panel',
										'width'				: '757px',
										'background-color'	: '#eee',
										'font-size'			: '11px',
										'color'				: '#000',
										'min-height'		: '30px'
										};
		function to_css(obj)
			{
			var ret			= obj.name+" {";
			$.each(obj, function(i,val)
							{
							if(i != 'name')
								{
								ret		+= i+":"+val+";";
								}
							});
			ret				+= "} \n";
			return ret;
			}
		if($(sc_settings.parent_node).attr('data-is_inventory') == undefined)
			{
			$(sc_settings.parent_node).attr('data-is_inventory', 'true');
			$("head").append("<style>"+
				to_css(sc_css.active_tab)+
				to_css(sc_css.options_panel)+
				to_css(sc_css.inactive_tab)+
				to_css(sc_css.criteria)+
				to_css(sc_css.attributes)+
				to_css(sc_css.results)+
				to_css(sc_css.tabs)+
				to_css(sc_css.container)+
				"</style>");
			$(document).keydown(function(e)
				{
				var key = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
				var daE	= document.activeElement;
				if(key == 27)
					{
					$('document').hideresult_window();
					if(sc_settings.x[0] != null)
						{
						sc_settings.x[0].abort();
						}
					if(sc_settings.x[1] != null)
						{
						sc_settings.x[1].abort();
						}
					if(sc_settings.x[2] != null)
						{
						sc_settings.x[2].abort();
						}
					return false;
					}
				});
										
			$(document).bind('mousedown', function(e)
				{
				if(!$(e.target).is('#i_search_container') && $(e.target).parents('#i_search_container').length == 0 && !$(e.target).attr('data-is_inventory') && $(e.target).parents('.ac_results').length == 0)
					{ 
					$('document').hideresult_window();
					}
				});
					
			$(sc_settings.parent_node).keypress(function(e)
				{							
				var key = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
				var val = $(sc_settings.parent_node).val();
				sc_settings.key_down(e);
				if(!e.shiftKey && (key == 13 || key == 40))
					{
					if(val.length > 0 && $.trim(val).length < 1)
						{
						alert('Query must be greater than 1 character');
						return false;
						}
					else if(val.length == 0)
						{
						$(sc_settings.parent_node).showresult_window(false);
						$('#i_search_criteria').focus();
						}
					else
						{
						$(sc_settings.parent_node).showresult_window(true);
						$('#i_search_criteria_box').hide();
						if(sc_settings.x[0] == null && sc_settings.x[1] == null && sc_settings.x[2] == null )
							{
							$(sc_settings.parent_node).keyword_search();
							}
						}
					return false;
					}
				});
			$(sc_settings.parent_node).keyup(function(e)
				{
				sc_settings.key_up(e);
				});
			$(sc_settings.parent_node).focus(function()
				{
				if($('#i_search_attributes').is(':visible'))
					{
					$('#i_search_attributes').slideUp('fast');
					$(sc_settings.parent_node).i_tab($('#i_tab_k'));
					}
				});
			}
		}
//-------------------------------------------------------------------------------------------------
	$.fn.i_build_attributes	= function(business_unit_id)
		{
		var tag_id			= $('#i_search_criteria').attr('data-id');
		var boxes			= "<div style='background-color:#aaa;color:#fff;font-weight:bold;padding:2px;font-size:11px;' align='center'>Criteria</div>";
		$.ajax(	{
				type:		"GET",
				url:		"/_tools/inventory_search/index.aspx?a=xml_tag_att_val&tag_id="+tag_id,
				dataType:	"xml",
				cache:		false,
				beforeSend:	function()
								{
								$('#i_search_results').animate({'min-height':'200px'});
								$('#i_search_attributes').empty().html(boxes+"<div style='width:100%;padding-top:10px;padding-bottom:10px;' align='center'><img src='/images/loading_panel.gif'/></div>");
								},
				success:	function(xml)
								{
								if($(xml).find('attribute').size() > 0)
									{
									$(xml).find('attribute').each(
										function()
											{
											var attribute_id		= $(this).attr('id');
											var attribute_name		= $(this).attr('name');
											boxes					+= "<select class='i_attribute_box' onchange='$(this).refine_search();' style='width:98%;font-size:11px;font-weight:bold;margin-bottom:2px;'><option value='0' style='background-color:#ccc;'>"+attribute_name+"</option>";
											$(this).find('value').each(function()
												{
												var value_name		= $(this).attr('name');
												var value_id		= $(this).attr('id');
												boxes				+= "<option title=\""+value_name+"\" value=\""+value_id+"\">"+value_name+"</option>";
												});
											boxes					+= "</select><br/>";
											});
									}
								else
									{
									boxes				+= "Error";
									}
								},
				complete:	function()
								{
								$('#i_search_attributes').empty().html(boxes);
								},
				error:		function(asdf)
								{
								$('#i_search_attributes').empty().html("Error");
								}
				});
		return true;
		}
//-------------------------------------------------------------------------------------------------
	$.fn.i_tab = function(obj)
		{
		if(sc_settings.x != undefined) 
			{
			if(sc_settings.x[0] != null)
				{
				sc_settings.x[0].abort();
				}
			if(sc_settings.x[1] != null)
				{
				sc_settings.x[1].abort();
				}
			if(sc_settings.x[2] != null)
				{
				sc_settings.x[2].abort();
				}
			}
		var this_tab        = $(obj).attr('id');
		var alt_tab         = this_tab == "i_tab_k" ? "i_tab_f" : "i_tab_k";
		$('#'+this_tab).attr('class', 'inv_active_tab').attr('disabled', true);
		$('#'+alt_tab).attr('class', 'inv_inactive_tab').removeAttr('disabled');
		if(this_tab == "i_tab_k")
			{
			if(!$('#i_search_attributes').is(':hidden'))
				{
				$('#i_search_attributes').slideUp('fast');
				}
			$('#i_search_results').css({'border-top':'solid 1px #999'}).animate({'min-height':'50px'}).empty().html("\
		<div id='i_result_master_id'></div>\
		<div id='i_pages_master_id' align='center' style='display:none;'>\
			<form>\
				<button type='button' class='first'>&lt;&lt;</button>\
				<button type='button' class='prev'>&lt;</button>\
				<input type='text' size='5' class='pagedisplay'/>\
				<button type='button' class='next'>&gt;</button>\
				<button type='button' class='last'>&gt;&gt;</button>\
				<select class='pagesize'>\
					<option selected='selected' value='10'>10</option>\
					<option value='20'>20</option>\
					<option value='30'>30</option>\
					<option  value='40'>40</option>\
				</select>\
			</form>\
		</div>\
		<div id='i_result_reference'></div>\
		<div id='i_pages_reference' align='center' style='display:none;'>\
			<form>\
				<button type='button' class='first'>&lt;&lt;</button>\
				<button type='button' class='prev'>&lt;</button>\
				<input type='text' size='5' class='pagedisplay'/>\
				<button type='button' class='next'>&gt;</button>\
				<button type='button' class='last'>&gt;&gt;</button>\
				<select class='pagesize'>\
					<option selected='selected' value='10'>10</option>\
					<option value='20'>20</option>\
					<option value='30'>30</option>\
					<option  value='40'>40</option>\
				</select>\
			</form>\
		</div>\
		<div id='i_result_description'></div>\
		<div id='i_pages_description' align='center' style='display:none;'>\
			<form>\
				<button type='button' class='first'>&lt;&lt;</button>\
				<button type='button' class='prev'>&lt;</button>\
				<input type='text' size='5' class='pagedisplay'/>\
				<button type='button' class='next'>&gt;</button>\
				<button type='button' class='last'>&gt;&gt;</button>\
				<select class='pagesize'>\
					<option selected='selected' value='10'>10</option>\
					<option value='20'>20</option>\
					<option value='30'>30</option>\
					<option  value='40'>40</option>\
				</select>\
			</form>\
		</div>\
		<div id='i_result_vendor_code'></div>\
		<div id='i_pages_vendor_code' align='center' style='display:none;'>\
			<form>\
				<button type='button' class='first'>&lt;&lt;</button>\
				<button type='button' class='prev'>&lt;</button>\
				<input type='text' size='5' class='pagedisplay'/>\
				<button type='button' class='next'>&gt;</button>\
				<button type='button' class='last'>&gt;&gt;</button>\
				<select class='pagesize'>\
					<option selected='selected' value='10'>10</option>\
					<option value='20'>20</option>\
					<option value='30'>30</option>\
					<option  value='40'>40</option>\
				</select>\
			</form>\
		</div>\
		<div style='height:20px;'>&nbsp;</div>");
			$('#i_search_criteria_box').slideUp('fast');
			if($(sc_settings.parent_node).val() != "")
				{
				$(this).keyword_search();
				}
			}
		else
			{
			$('#i_search_results').css({'border-top':'solid 1px transparent'});
			$('#i_search_criteria_box').slideDown('fast', function()
				{
				$('#i_search_results').empty();
				if($('#i_search_criteria').attr('data-id') != '' && $('#i_search_criteria').attr('data-id') != undefined)
					{
					$('#i_search_attributes').fadeIn('fast', function()
						{
						$().refine_search();
						});
					}
				else
					{
					$('#i_search_results').animate({'min-height':'50px'});
					}
				$('#i_search_criteria').focus();
				});
			}
		}
//-------------------------------------------------------------------------------------------------
	$.fn.hideresult_window  = function()
		{
		if(sc_settings.x[0] != null)
			{
			sc_settings.x[0].abort();
			}
		if(sc_settings.x[1] != null)
			{
			sc_settings.x[1].abort();
			}
		if(sc_settings.x[2] != null)
			{
			sc_settings.x[2].abort();
			}
		$(sc_settings.parent_node).removeAttr('data-is_inventory');
		$('#i_search_container').remove();
		$(document).removeAttr('onkeydown');
		}
//-------------------------------------------------------------------------------------------------
$.fn.showresult_window  = function(is_keyword)
   {
   var button_key        = "class='inv_active_tab' disabled='true' ";
   var button_filter     = "class='inv_inactive_tab'";
   var criteria_default  = "";
   if(!is_keyword)
		{
		button_key           = "class='inv_inactive_tab'";
		button_filter        = "class='inv_active_tab' disabled='true' ";
		criteria_default     = "Tag: <input type='text' id='i_search_criteria' style='width:500px;font-weight:bold;font-size:11px' data-isac='false' onfocus=\"attach_ac(this, 'inventory_tag')\"/>";
		}
		
	if($('#i_search_container').size() == 0)
		{
		var to_append		= "\
		<div id='i_search_container' class='inv_container'>\
			<span id='i_search_tabs' class='inv_tabs'>\
				<button id='i_tab_k' onclick='$(this).i_tab(this);' "+button_key+">keyword</button>\
				<button id='i_tab_f' onclick='$(this).i_tab(this);' "+button_filter+">filter</button>\
			</span>\
			<div id='i_search_options_box' class='inv_options_panel'>\
				<div style='margin:3px;float:left;cursor:pointer;' title=\"Only show parts that have locations with minimum qty levels set.\">\
				<input id='i_op_isstocked' onclick='$(this).handle_toggle_options(this)' style='cursor:pointer;' type='checkbox' />\
				<label style='cursor:pointer;' for='i_op_isstocked'>Is Stocked?</label>\
				</div>\
                <div style='margin:3px;float:left;cursor:pointer;' title=\"Show parts that have been used on work orders, sorted by usage.\">\
                <input id='i_op_bywousage' onclick='$(this).handle_toggle_options(this)' style='cursor:pointer;' type='checkbox' />\
				<label style='cursor:pointer;' for='i_op_bywousage'>Commonly Used on WOs?</label> -or-\
				</div>\
                <div style='margin:3px;float:left;cursor:pointer;' title=\"Show parts that have been used on purchase orders, sorted by usage.\">\
                <input id='i_op_bypousage' onclick='$(this).handle_toggle_options(this)' style='cursor:pointer;' type='checkbox' />\
				<label style='cursor:pointer;' for='i_op_bypousage'>Commonly Purchased?</label>\
				</div>\
			</div>\
			<div id='i_search_attributes' class='inv_attributes' style='background-color:#fff;'>&nbsp;</div>\
			<div id='i_search_criteria_box' class='inv_criteria'>Tag: <input type='text' id='i_search_criteria' style='width:600px;font-weight:bold;font-size:11px' data-isac='false' onfocus=\"attach_ac(this, 'inventory_tag')\"/></div>\
			<div id='i_search_results' class='inv_results'>\
					<div align='center'><button type='button' style='font-size:11px;' onclick=\"$('#i_search_legend').toggle();\">Toggle Legend</button></div>\
					<div id='i_search_legend' align='center' style='padding:5px;display:none;'>\
					<table cellpadding='2' cellspacing='5' style='border:solid 1px #999;border-radius:5px;margin-top:5px;'>\
						<tr>\
							<td style='border:solid 1px #ccc;'><b>Bold text</b></td>\
							<td>Branch has stock on hand</td>\
						</tr>\
						<tr>\
							<td style='border:solid 1px #ccc;color:#090;'>Green Text</td>\
							<td>Branch stocks this part</td>\
						</tr>\
						<tr>\
							<td style='border:solid 1px #ccc;background-color:#39f;color:#fff;'><b>Blue background, white text</b></td>\
							<td>Part has min/max set</td>\
						</tr>\
					</table>\
					</div>\
					<div id='i_result_master_id'></div>\
					<div id='i_pages_master_id' align='center' style='display:none;'>\
						<form>\
							<button type='button' class='first'>&lt;&lt;</button>\
							<button type='button' class='prev'>&lt;</button>\
							<input type='text' size='5' class='pagedisplay'/>\
							<button type='button' class='next'>&gt;</button>\
							<button type='button' class='last'>&gt;&gt;</button>\
							<select class='pagesize'>\
								<option selected='selected'  value='10'>10</option>\
								<option value='20'>20</option>\
								<option value='30'>30</option>\
								<option  value='40'>40</option>\
							</select>\
						</form>\
					</div>\
					<div id='i_result_reference'></div>\
					<div id='i_pages_reference' align='center' style='display:none;'>\
						<form>\
							<button type='button' class='first'>&lt;&lt;</button>\
							<button type='button' class='prev'>&lt;</button>\
							<input type='text' size='5' class='pagedisplay'/>\
							<button type='button' class='next'>&gt;</button>\
							<button type='button' class='last'>&gt;&gt;</button>\
							<select class='pagesize'>\
								<option selected='selected'  value='10'>10</option>\
								<option value='20'>20</option>\
								<option value='30'>30</option>\
								<option  value='40'>40</option>\
							</select>\
						</form>\
					</div>\
					<div id='i_result_description'></div>\
					<div id='i_pages_description' align='center' style='display:none;'>\
						<form>\
							<button type='button' class='first'>&lt;&lt;</button>\
							<button type='button' class='prev'>&lt;</button>\
							<input type='text' size='5' class='pagedisplay'/>\
							<button type='button' class='next'>&gt;</button>\
							<button type='button' class='last'>&gt;&gt;</button>\
							<select class='pagesize'>\
								<option selected='selected'  value='10'>10</option>\
								<option value='20'>20</option>\
								<option value='30'>30</option>\
								<option  value='40'>40</option>\
							</select>\
						</form>\
					</div>\
					<div id='i_result_vendor_code'></div>\
					<div id='i_pages_vendor_code' align='center' style='display:none;'>\
						<form>\
							<button type='button' class='first'>&lt;&lt;</button>\
							<button type='button' class='prev'>&lt;</button>\
							<input type='text' size='5' class='pagedisplay'/>\
							<button type='button' class='next'>&gt;</button>\
							<button type='button' class='last'>&gt;&gt;</button>\
							<select class='pagesize'>\
								<option selected='selected'  value='10'>10</option>\
								<option value='20'>20</option>\
								<option value='30'>30</option>\
								<option  value='40'>40</option>\
							</select>\
						</form>\
					</div>\
					<div style='height:20px;'>&nbsp;</div>\
			</div>\
		</div>";
		$('body').append(to_append);
		//$('#i_search_container').css(sc_css.container);
		//$('#i_search_criteria_box').css(sc_css.criteria);
		//$('#i_search_tabs').css(sc_css.tabs);
		//$('#i_search_tabs button.active').css(sc_css.active_tab);
		//$('#i_search_tabs button.inactive').css(sc_css.inactive_tab);
		//$('#i_search_results').css(sc_css.results);
		//$('#i_search_attributes').css(sc_css.attributes);
		}
	}
//-------------------------------------------------------------------------------------------------
$.fn.handle_toggle_options		= function(obj)
	{
	var o_id			= $(obj).attr("id");
	var o_ischecked		= $(obj).is(":checked");
	switch(o_id)
		{
		case "i_op_isstocked":
			
		break;
		case "i_op_bywousage":
			if(o_ischecked && $("#i_op_bypousage").is(":checked"))
				{
				$("#i_op_bypousage").removeAttr("checked");
				}
		break;
		case "i_op_bypousage":
			if(o_ischecked && $("#i_op_bywousage").is(":checked"))
				{
				$("#i_op_bywousage").removeAttr("checked");
				}
		break;
		}
	if(sc_settings.x[0] != null)
		{
		sc_settings.x[0].abort();
		}
	if(sc_settings.x[1] != null)
		{
		sc_settings.x[1].abort();
		}
	if(sc_settings.x[2] != null)
		{
		sc_settings.x[2].abort();
		}
	var _tab         = $("#i_tab_k").attr('class') == 'inv_active_tab' ? "i_tab_k" : "i_tab_f";
	switch(_tab)
		{
		case "i_tab_k":
			if(sc_settings.parent_node.val().trim() != "")
				{
				$().keyword_search();
				}
			else
				{
				sc_settings.parent_node.focus();
				}
		break;
		case "i_tab_f":
			if($('#i_search_criteria').val().trim() != "")
				{
				$().refine_search();
				}
			else
				{
				$('#i_search_criteria').focus();
				}
		break;
		}
	}
//-------------------------------------------------------------------------------------------------
$.fn.refine_search		= function(start, length, total)
	{
	var n_atts					= $('#i_search_attributes').find('select').size();
	var tag_id					= $('#i_search_criteria').attr('data-id');
	if(tag_id == "")
		{
		alert("Please supply the tag/category you are searching against.");
		return;
		}
	var att_vals				= [];
	var tag_pattern				= "";
	var parts					= "";
	var incomplete				= false;
	$('#i_search_attributes').find('select').each(function()
		{
		if($(this).val() != "0")
			{
			att_vals.push($(this).val());
			}
		else
			{
			incomplete	= true;
			}
		});
	if(start == undefined)
		{
		start					= 0;
		}
	if(length == undefined)
		{
		length					= 0;
		}
	if(total == undefined)
		{
		total					= 0;
		}
	var end						= 0;
	var querytime				= 0;
	var pages					= 0;
	var o_load					= 0;
	var query_string			= {
								a:				"xml_match_parts",
								tag_id:			tag_id,
								attvals:		att_vals.toString(),
								vendor_id:		sc_settings.vendor_id,
								allow_gl:		sc_settings.allow_gl,
								allow_nonstock:	sc_settings.allow_nonstock,
								show_nosell:	sc_settings.show_nosell,
								business_unit_id:		sc_settings.business_unit_id,
								origin:			sc_settings.origin,
								is_stocked:		$('#i_op_isstocked').is(':checked'),
								wo_usage:		$('#i_op_bywousage').is(':checked'),
								po_usage:		$('#i_op_bypousage').is(':checked')
								};
	sc_settings.x[0]			= $.ajax(	{
			type:		"GET",
			url:		"/_tools/inventory_search/index.aspx",
			data:		query_string,
			dataType:	"xml",
			cache:		false,
			beforeSend:	function()
							{
							$('#i_search_results').animate({'min-height':'200px'});
							$('#i_search_results').empty().html("<div style='padding-top:50px;font-size:12px;' align='center'>Finding Matches<br/><div style='font-size:10px;'>Results are limited to 200 rows.</div><img src='/images/loading_panel.gif' width='16' height='16'/></div>");
							parts					= 	sc_settings.parts_header;
							},
			success:	function(xml)
							{
							start					= $(xml).find('parts').attr('start') / 1;
							length					= $(xml).find('parts').attr('length') / 1;
							total					= $(xml).find('parts').attr('total') / 1;
							querytime				= $(xml).find('parts').attr('querytime') / 1;
							o_load					= $(xml).find('parts').attr('objectload') / 1;
							pages					= Math.ceil(total / length);
							if(isNaN(start))
								{
								start				= 0;
								}
							if(isNaN(length))
								{
								length				= 0;
								}
							if(isNaN(total))
								{
								total				= 0;
								}
							if(total < length)
								{
								length				= total;
								}
							if(start+length > total)
								{
								end					= total;
								}
							else
								{
								end					= start+length;
								}
							if((total != undefined && total > 0))
								{
								$(xml).find('part').each(function(){parts = $(this).format_results(parts);});
								}
							else
								{
								total						= 0;
								var create					= true;
								$('#i_search_attributes').find('select').each(function()
									{
									if($(this).val() == "0")
										{
										create				= false;
										}
									});
								var create_part				= "";
								if(!incomplete)
									{
									create_part				= "<button type='button' onclick=\"boing('/sections/member/inventory/index.aspx?a=start_part&tag_id="+tag_id+"&pattern="+att_vals+"', 'newpart', 1280, 768);\"> Create Part? </button>";
									}
								var create_777				= sc_settings.origin == "workorder" && create
																? " OR <button type='button' onclick='$(this).sc_create777();'>Create 777</button>" 
																: sc_settings.origin == "workorder" && !create 
																	? " OR fill in all values and you will be able to create a 777 part."
																	: ".";
								parts						= "\
											<table width='100%' cellpadding='2' cellspacing='0' style='font-family:arial;font-size:11px;padding-top:25px;'>\
												<tr>\
													<td align='center'><b style='font-size:11px;'>No parts returned.</b><br/>"+create_part+"<br/>Please refine your criteria above"+create_777+"</td>\
												</tr>";
								}
							},
			complete:	function()
							{
							parts									+= "</tbody></table>";
							if(total > 0)
								{
								parts								+= "\
								<div id='i_pages' align='center' style='display:none;'>\
									<form>\
										<button type='button' class='first'>&lt;&lt;</button>\
										<button type='button' class='prev'>&lt;</button>\
										<input type='text' size='5' class='pagedisplay'/>\
										<button type='button' class='next'>&gt;</button>\
										<button type='button' class='last'>&gt;&gt;</button>\
										<select class='pagesize'>\
											<option value='10'>10</option>\
											<option value='20' selected='selected'>20</option>\
											<option value='30'>30</option>\
											<option value='40'>40</option>\
										</select>\
									</form>\
								</div>";
								var header			= "<b style='font-size:12px'>Parts Matching The Above Criteria</b>";
								$('#i_search_results').html(header+parts);
								try
									{
								$('#i_search_results').find('.tablesorter').tablesorter({
																						//sortList:	[[4,1],[6,1],[1,0]],
																						headers:	{ 
																									0: {sorter:false},
																									1: {sorter:'text'}, 
																									2: {sorter:'digit'},
																									3: {sorter:'currency'},
																									4: {sorter:'currency'},
																									5: {sorter:'date'},
																									6: {sorter:'digit'}
																									}
																						}).tablesorterPager({positionFixed:false, size:20,container: $("#i_pages")});
								if(total > 10)
									{
									$('#i_pages').find('button,input, select').css({'font-size':'11px', 'font-weight':'bold'});
									$('#i_pages').css({'display':'block'});
									}
								else
									{
									$('#i_pages').css({'display':'none'});
									}
									}
								catch(ex)
									{
									}
								}
							else
								{
								$('#i_search_results').html(parts);
								}
							var to_height				= $('#i_search_results').find('.tablesorter').height()+50;
							if(to_height == null || total == 0)
								{
								to_height				= 100;
								}
							$('#i_search_results').animate({'min-height': to_height});
							sc_settings.x[0]		= null;
							},
			error:		function(xhr, textStatus, errorThrown)
							{
							$('#i_search_results').html("There was an error trying to retrieve parts matching the supplied criteria.");
							sc_settings.x[0]		= null;
							}
			});
	}
$.fn.sc_create777 = function()
	{
	// Build description
	var description		= "";
	var c				= $('#i_search_attributes').find('select').length;
	$('#i_search_attributes').find('select').each(function(i,val)
		{
		var att_name	= $("option:first", this).text();
		var att_val		= $("option:selected", this).text();
		if(att_val == att_name)
			{
			att_val		= "N/A";
			}
		description		+= att_name+": "+att_val;
		if(i != c - 1)
			{
			description		+= ", ";
			}
		});
	// Run 777 code
	$(".Description").val(description);
	$(".part_no").val("777");
	$('document').hideresult_window();
	// $('.TextCost').val('0').attr('disabled', true);
	// $('.TextComQty').val("0").attr("disabled", true)
    // $('.QtyPart').focus();

    var objectOfMasterId = $('.part_no');
    $(".Description").attr('data-showTipForDescription', "hidden");
    objectOfMasterId.trigger("blur");
	$(".Description").val(description);
	$('.QtyPart').focus();
	}
//-------------------------------------------------------------------------------------------------
$.fn.keyword_search	= function(start, length, total, page)
	{
	var search_types			= ["vendor_code", "description", "master_id"];
	var keyword					= $(sc_settings.parent_node).val();
	$.each(search_types, function(i, search_type)
		{
		if(start == undefined){start= 0;}
		if(length == undefined){length= 0;}
		if(total == undefined){total= 0;}
		var end						= 0;
		var querytime				= 0;
		var pages					= 0;
		var o_load					= 0;
		var parts					= "";
		var query_string			= {
									q:				keyword,
									search_type:	search_type,
									start:			start,
									length:			length,
									total:			total,
									business_unit_id:		sc_settings.business_unit_id,
									vendor_id:		sc_settings.vendor_id,
									allow_gl:		sc_settings.allow_gl,
									allow_nonstock:	sc_settings.allow_nonstock,
									show_nosell:	sc_settings.show_nosell,
									origin:			sc_settings.origin,
									is_stocked:		$('#i_op_isstocked').is(':checked'),
									wo_usage:		$('#i_op_bywousage').is(':checked'),
									po_usage:		$('#i_op_bypousage').is(':checked')
									};
		var _section_defs			=	{
										master_id:		{
														title:			"Parts With Matching Spark Ops Part #'s", 
														bgcolor:		"#eee",
														bordercolor:	"#eee"
														},
										reference:		{
														title:			"Parts With Matching Old Part #'s",
														bgcolor:		"#eee",
														bordercolor:	"#eee"
														},
										description:	{
														title:			"Parts With Matching Descriptions",
														bgcolor:		"#eee",
														bordercolor:	"#eee"
														},
										vendor_code:	{
														title:			"Parts With Matching Vendor Part #'s",
														bgcolor:		"#eee",
														bordercolor:	"#eee"
														}
										};
		var _title					= _section_defs[search_type].title;
		var _sectionBgColor			= _section_defs[search_type].bgcolor;
		var _sectionBorderColor		= _section_defs[search_type].bordercolor;
	sc_settings.x[i]				= $.ajax(	{
			type:		"GET",
			url:		"/_tools/inventory_search/index.aspx",
			data:		query_string,
			dataType:	"xml",
			cache:		false,
			beforeSend:	function()
							{
							$('#i_search_results').animate({'min-height': '425px'}).css({"border-top":"solid 1px #999"});
							$('#i_result_'+search_type).show();
							$('#i_result_'+search_type).html("<div style='margin-top:10px;font-size:11px;margin-bottom:10px;padding:10px;border:solid 1px #ccc;' align='center'><b>Checking For "+_title+"</b><br/><img src='/images/loading_panel.gif' width='16' height='16'/></div>");
							$('#i_result_'+search_type).prev('b').show();
							$('#i_pages_'+search_type).hide();
							parts					= "<br/><b class='result_title'>"+_title+"</b>"+sc_settings.parts_header;
							sc_settings.x_start[i]	= new Date().getTime();
							},
			success:	function(xml)
							{
							if(parts == "")
								{
								parts				= "<br/><b class='result_title'>"+_title+"</b>"+sc_settings.parts_header;
								}
							start					= $(xml).find('parts').attr('start') / 1;
							length					= $(xml).find('parts').attr('length') / 1;
							total					= $(xml).find('parts').attr('total') / 1;
							querytime				= $(xml).find('parts').attr('querytime') / 1;
							o_load					= $(xml).find('parts').attr('objectload') / 1;
							pages					= Math.ceil(total / length);
							if(isNaN(start))
								{
								start				= 0;
								}
							if(isNaN(length))
								{
								length				= 0;
								}
							if(isNaN(total))
								{
								total				= 0;
								}
							if(total < length)
								{
								length				= total;
								}
							if(start+length > total)
								{
								end					= total;
								}
							else
								{
								end					= start+length;
								}
							if((total != undefined && total > 0))
								{
								$(xml).find('part').each(function(){parts	= $(this).format_results(parts);});
								}
							else
								{
								total						= 0;
								parts						= "<div style='color:#999;' align='center'>No parts match your search criteria, please refine your keywords</div>";
								}
							},
			complete:	function()
							{
							parts						+= "</tbody></table>";
							if(total > 0)
								{
								var header			= "<div style='color:#06f;padding:5px;height:15px;display:block;'><span style='float:right;width:35%;text-align:right;color:#999;'>Query took <i style='color:#f00;'><b>"+querytime.toFixed(5)+"</b></i> seconds</span></div>";
								$('#i_result_'+search_type).css({'display':'block'}).html(parts).prev('b').css({'display':'block'});
								try
									{
								$('#i_result_'+search_type).children('.tablesorter').tablesorter(
																							{
																							//sortList:	[[6,1],[4,1],[1,0]],
																							headers:	{ 
																										0: {sorter:false},
																										1: {sorter:'text'}, 
																										2: {sorter:'digit'},
																										3: {sorter:'currency'},
																										4: {sorter:'currency'},
																										5: {sorter:'date'},
																										6: {sorter:'currency'},
																										7: { sorter: 'currency' }
																										}
																							}).tablesorterPager(	{
																													positionFixed:false,
																													container: $("#i_pages_"+search_type)
																													});
									}
								catch(ex)
									{}
								var to_height				= $('#i_search_results').find('.tablesorter').height()+50;
								if(to_height == null || total == 0)
									{
									to_height				= 100;
									}
								$('#i_search_results').animate({'min-height': to_height});
									if(total > 10)
										{
										$('#i_pages_'+search_type).find('button,input, select').css({'font-size':'11px', 'font-weight':'bold'});
										$('#i_pages_'+search_type).css({'display':'block'});
										}
									else
										{
										$('#i_pages_'+search_type).css({'display':'none'});
										}
								}
							else
								{
								$('#i_result_'+search_type).css({'display':'none'}).prev('b').css({'display':'none'});
								$('#i_pages_'+search_type).css({'display':'none'});
								}
							sc_settings.x[i]		= null;
							sc_settings.x_end[i]	= new Date().getTime();
							sc_settings.o_load[i]	= o_load;
							if(sc_settings.x_end[0] != null && sc_settings.x_end[1] != null && sc_settings.x_end[2] != null)
								{
								$('#i_result_'+search_types[0]).children('.result_title:first').attr("title", "Search Time: "+(sc_settings.x_end[0] - sc_settings.x_start[0])+"ms, Object Load:"+sc_settings.o_load[0]+"ms");
								$('#i_result_'+search_types[1]).children('.result_title:first').attr("title", "Search Time: "+(sc_settings.x_end[1] - sc_settings.x_start[1])+"ms, Object Load:"+sc_settings.o_load[1]+"ms");
								$('#i_result_'+search_types[2]).children('.result_title:first').attr("title", "Search Time: "+(sc_settings.x_end[2] - sc_settings.x_start[2])+"ms, Object Load:"+sc_settings.o_load[2]+"ms");
								sc_settings.x_start		= [null, null, null];
								sc_settings.x_end		= [null, null, null];
								}
							},
			error:		function(xhr, textStatus, errorThrown)
							{
							$('#i_result_'+search_type).html("There was an error trying to retrieve parts matching the supplied criteria.");
							sc_settings.x[i]		= null;
							sc_settings.x_start[i]	= null;
							sc_settings.x_end[i]	= null;
							}
			});
		});
	}
$.fn.format_results	= function(parts)
	{
	var master_id					= $(this).find('master_id').text();
	if(master_id == 777 && sc_settings.origin != "workorder")
		{
		return parts;
		}
	var bv_code						= $(this).find('bv_code').text();
	var description					= $(this).find('description').text().replace(/\"/g, "&quot;").replace(/\</g, "&lt;").replace(/\>/g, "&gt;");
	var sold_as						= $(this).find('sold_as').text();
	sold_as							= sold_as == "" || sold_as == undefined ? "EACH" : sold_as;
	var sellprice					= $(this).find('sellprice').text();
	var sellprice_n					= $(this).find('sellprice').text() / 1;
	sellprice						= sellprice/1 == 0 ? "<span style='color:#ccc'>--<span>" : "$"+(sellprice/1).toFixed(3);
	var costprice					= $(this).find('costprice').text();
	var costprice_n					= $(this).find('costprice').text();
	costprice						= costprice/1 == 0 ? "<span style='color:#ccc'>--<span>" : "$"+(costprice/1).toFixed(3);
	var tag_id						= $(this).find('tag_id').text();
	var tag_name					= $(this).find('tag_name').text();
	var from_business_unit_id				= $(this).find('from_business_unit_id').text();
	var to_business_unit_id				= $(this).find('to_business_unit_id').text();
	var global_price_cost			= $(this).find('global_price_cost').text();
	var global_price_sell			= $(this).find('global_price_sell').text();
	var global_price_sell_n			= $(this).find('global_price_sell').text();
	var show_cost					= $(this).find('show_cost').text() == "True";
	var show_sell					= $(this).find('show_sell').text() == "True";
	var has_pic						= $(this).find('has_pic').text() == "True";
	global_price_sell				= global_price_sell/1 == 0 ? "<span style='color:#ccc'>--<span>" : "$"+(global_price_sell/1).toFixed(3);
	var is_QTY						= $(this).find('is_QTY').text();
	var global_price_price_id		= $(this).find('global_price_price_id').text();
	var has_minmax					= $(this).find('has_minmax').text() == "True";
	var last_vendor_dt				= $(this).find('last_vendor_dt').text();
	last_vendor_dt					= last_vendor_dt == "--" ? "<span style='color:#ccc'>--<span>" : last_vendor_dt;
	var vendor_sell					= $(this).find("vendor_sell").text();
	var vendor_code					= $(this).find("vendor_code").text();
	var vendor_qty					= $(this).find("vendor_qty").text();
	var onhand_qty					= $(this).find("onhand_qty").text();
	var onhand_qty_n				= $(this).find("onhand_qty").text() /1;
	onhand_qty						= onhand_qty == "0" ? "<span style='color:#ccc'>--<span>" : onhand_qty;
	var is_exclude					= $(this).find("is_exclude").text();
	var is_stocked					= $(this).find("is_stocked").text() == "True";
    var int_onhand_qty				= $(this).find("int_onhand_qty").text();
    var ext_onhand_qty				= $(this).find("ext_onhand_qty").text();
	var total_int_onhand_qty		= $(this).find("total_int_onhand_qty").text();
	var total_ext_onhand_qty		= $(this).find("total_ext_onhand_qty").text();
	var bar_bgcolor					= "";
	var bar_color					= "";
	var bar_cursor					= "";
	var bar_onclick					= "";
	var global_price_sell_bgcolor	= "";
	var global_price_sell_style		= "";
	var global_price_sell_color		= "";
	var branch_sell_bgcolor			= "";
	var branch_sell_color			= "";
	var is_available				= "";
	if(global_price_sell != 0 && sellprice == 0)
		{
		branch_sell_bgcolor		= "#393;";
		branch_sell_color		= "#fff;";
		}
	else
		{
		branch_sell_bgcolor		= "transparent;";
		branch_sell_color		= "#006;";
		}

	is_available				= "";
	bar_cursor					= "pointer";
	bar_bgcolor					= "#def;";
	global_price_sell_bgcolor	= "#393;";
	global_price_sell_color		= "#fff;";
	bar_color					= "#000;";
	global_price_sell_style		= "";
	bar_onclick					= "'$(this).return_val(this, false);'";
	var bar_weight		= onhand_qty_n > 0 ? "bold" : "normal";
	var font_color		= is_stocked ? "#090 !important" : "#000";
	var desc_span		= "<td align='left'><div style='width:300px;overflow:hidden;white-space:nowrap;border-right: solid 3px transparent;color:"+font_color+";'>"+description+"</b></div></td>";
	var part_n_bg		= has_minmax ? "#39f;color:#fff;" : "#eee";
	var part_n_span		= "<td align='center' style='background-color:"+part_n_bg+";min-width:40px;'>"+master_id+"</td>";
	var updated_span	= "<td align='center'>"+last_vendor_dt+"</td>";
	var cost_span		= show_cost ? "<td align='center'>"+costprice+"</td>" : "<td align='center'>&nbsp;</td>";
	var sell_span		= show_sell ? "<td align='center'>"+sellprice+"</td>" : "<td align='center'>&nbsp;</td>";
	var img_span		= has_pic ? "<td width='16'><img src='/images/icon/icon[photo].gif' width='16' height='16' onmouseover='$(this).sf_image("+master_id+", this)' onmouseout='$(this).hf_image()' /></td>" : "<td width='20'><img src='/images/pixel.gif' width='16' height='16' /></td>";
	var onhand_span = "<td align='center'>" + onhand_qty + "</td>";
	var int_onhand_span = "<td align='center'>" + int_onhand_qty + "</td>";
	var ext_onhand_span = "<td align='center'>" + ext_onhand_qty + "</td>";
	var total_int_onhand_span = "<td align='center'>" + total_int_onhand_qty + "</td>";
	var total_ext_onhand_span = "<td align='center'>" + total_ext_onhand_qty + "</td>";
	parts				+= "\
						<tr 	class							= 'part'\
								onclick							= "+bar_onclick+" \
								data-master_id					= \""+master_id+"\" \
								data-bv_code					= \""+bv_code+"\" \
								data-sold_as					= \""+sold_as+"\" \
								data-description				= \""+description+"\" \
								data-sellprice					= \""+sellprice_n+"\" \
								data-costprice					= \""+costprice_n+"\" \
								data-is_QTY						= \""+is_QTY+"\" \
								data-global_price_cost			= \""+global_price_cost+"\" \
								data-from_business_unit_id			= \""+from_business_unit_id+"\" \
								data-to_business_unit_id				= \""+to_business_unit_id+"\" \
								data-global_price_price_id		= \""+global_price_price_id+"\" \
								data-global_price_sell			= \""+global_price_sell_n+"\" \
								data-tag_id						= \""+tag_id+"\" \
								data-tag_name					= \""+tag_name+"\"\
								data-last_vendor_dt				= \""+last_vendor_dt+"\"\
								data-vendor_sell				= \""+vendor_sell+"\"\
								data-vendor_code				= \""+vendor_code+"\"\
								data-onhand_qty                 = \""+onhand_qty_n+"\"\
								data-is_exclude                 = \""+is_exclude+"\"\
								data-vendor_qty					= \""+vendor_qty+"\"\
                                data-int_onhand_qty				= \"" + int_onhand_qty + "\"\
                                data-ext_onhand_qty				= \"" + ext_onhand_qty + "\"\
								data-total_int_onhand_qty		= \"" + total_int_onhand_qty + "\"\
                                data-total_ext_onhand_qty		= \"" + total_ext_onhand_qty + "\"\
								style							= 'font-size:10px;border-style:solid;border-width:1px;border-color:#777;cursor:"+bar_cursor+";font-family:verdana;width:625px;font-weight:"+bar_weight+";'\
								onmouseover						= \"$(this).find('div').each(function(){$(this).css({'white-space':'normal', 'overflow-x':'visible'});});\"\
								onmouseout						= \"$(this).find('div').each(function(){$(this).css({'white-space':'nowrap', 'overflow-x':'hidden'});});\">"+
								img_span+
								desc_span+
								part_n_span+
								cost_span+
								sell_span+
								updated_span+
								int_onhand_span +
								ext_onhand_span+
								total_int_onhand_span +
								total_ext_onhand_span
						"</tr>";
	return parts;
	}
$.fn.sf_image		= function(m_id, obj)
	{
	var offset		= $(obj).offset();
	var style		= "background-color:#fff;background-image:url('/images/loading_panel.gif');background-repeat:no-repeat;background-position:center center;z-index:10000;border-radius:5px;position:absolute;top:"+(offset.top-100)+"px;left:"+(offset.left - 100)+"px;border:solid 2px #000;width:100px;height:100px;";
	if($("#sf_image_container").size() == 0)
		{
		$("body").append("<div id='sf_image_container' style=\""+style+"\"><img src='/_tools/inventory_picture/index.aspx?id="+m_id+"' width='100' height='100' style='border-radius:5px;' /><div style='background-color:#000;color:#fff;font-weight:bold;font-size:10px;text-align:center;padding:2px;position:relative;top:-15px;z-index:10001;opacity:.85;'>#"+m_id+"</div></div>");
		}
	else
		{
		$("#sf_image_container").attr("style", style).children("img").attr("src", "/_tools/inventory_picture/index.aspx?id="+m_id).parent().children("div").text("#"+m_id).parent().show(0);
		}
	}
$.fn.hf_image		= function()
	{
	if($("#sf_image_container").size() > 0)
		{
		$("#sf_image_container").hide(0);
		}
	}
$.fn.return_val		= function(obj)
	{
	var row					=			{
										master_id:				$(obj).attr('data-master_id'),
										bv_code:				$(obj).attr('data-bv_code'),	
										description:			$(obj).attr('data-description'),	
										sold_as:				$(obj).attr('data-sold_as'),	
										sellprice:				$(obj).attr('data-sellprice'),	
										global_price_sell:		$(obj).attr('data-global_price_sell'),	
										costprice:				$(obj).attr('data-costprice'),	
										global_price_cost:		$(obj).attr('data-global_price_cost'),
										global_price_price_id:	$(obj).attr('data-global_price_price_id'),
										tag_id:					$(obj).attr('data-tag_id'),
										tag_name:				$(obj).attr('data-tag_name'),
										is_QTY:					$(obj).attr('data-is_QTY'),
										from_business_unit_id:		$(obj).attr('data-from_business_unit_id'),
										to_business_unit_id:			$(obj).attr('data-to_business_unit_id'),
										vendor_sell:			$(obj).attr("data-vendor_sell"),
										vendor_code:			$(obj).attr("data-vendor_code"),
										vendor_qty:				$(obj).attr("data-vendor_qty"),
										onhand_qty:             $(obj).attr("data-onhand_qty"),
										is_exclude:				$(obj).attr("data-is_exclude"),
                                        int_onhand_qty:             $(obj).attr("data-int_onhand_qty"),
										ext_onhand_qty:			$(obj).attr("data-ext_onhand_qty"),
										total_int_onhand_qty:			$(obj).attr("data-total_int_onhand_qty"),
										total_ext_onhand_qty:			$(obj).attr("data-total_ext_onhand_qty")
										};
	if(row.is_QTY == undefined)
		{
		row.is_QTY						= false;
		}
	sc_settings.click(row);
	if($("#aspnetForm"))
		{
		$("#aspnetForm").unbind('submit');
		}
	
	$('document').hideresult_window();
	
	}
//-------------------------------------------------------------------------------------------------
	}
	)(jQuery)