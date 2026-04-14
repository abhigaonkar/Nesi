(
function($)
	{
	var parent_node;
	$.fn.inventory = function()
		{
		if($("#aspnetForm"))
			{
			$("#aspnetForm").submit(function(){return false;}); 
			}
			
		var e				= window.event;
		var _bool			= false;
		if(e)
			{
			if($('#inventory_search_popup').css('display') == 'block' )
				{
				parent_node			= this;
				inv_hide(this);
				}
			else if(e.keyCode in _list(['13', '40']))
				{
				parent_node			= this;
				inv_show(this);
				}
			}
			
		function _list(a)
			{
			var o = {};
			for(var i=0;i<a.length;i++)
				{
				o[a[i]]='';
				}
			return o;
			}
			
		function inv_show(obj)
			{
			var _obj;
			if(window.location.href.match(/quote/g))
				{
				_obj	= $(obj).parent();
				}
			else
				{
				_obj	= obj;
				}
			var _offset	= _obj.offset();
			var _window	= $(window);
			//alert($(obj).offset().left+" "+_offset.left);
			var _height	= _obj.height();
			var _width	= _obj.width();
			var _top	= _offset.top+_height+6;
			var _left	= _offset.left;
			
			if(window.location.href.match(/NewWOProgShow/)){
			    var _top	= 0;
			    var _left	= 0; 
			}
			
			
			if((_left+450) > _window.width())
				{
				_left	= _window.width() - _width - 317;
				}
			$("body").append("\
<div id='inventory_search_popup' style='display:none;background-color:#ccc;padding:2px;position:fixed;z-index:30000;width:450px;border:solid 1px #999;top:"+_top+"px;left:"+_left+"px'>	\
<input type='text' class='query' style='width:80%;font-size:11px;font-weight:bold;'/><button onclick='$(this).inventory_query();' style='margin-left:3px;font-size:11px;font-weight:bold;'>search</button> \
<div id='inventory_search_results' style='display:none;font-size:10px;expression( this.scrollHeight > 249 ? \"250px\" : \"auto\" );height:250px;overflow:scroll;background-color:#ddd;border:solid 1px #888;font-weight:bold;' align='center'>\
</div>	 														\
</div>");
			var _search	= $('#inventory_search_popup');
			_search.fadeIn('fast',
				function()
					{
					$(this).find('.query').focus();
					if($(parent_node).val() != "")
						{
						$(this).children('.query').val($(parent_node).val());
						$(parent_node).val('');
						if($(this).children('.query').val().length > 2)
							{
							$(this).inventory_query();
							}
						}
					$(document).bind('click', 
						function(e)
							{
							if(!$(e.target).is('#inventory_search_popup') && !$(e.target).parents().is('#inventory_search_popup'))
								{
								inv_hide();
								}
							});
					$(document).keydown(function(e)
							{
							if(e.which == 27)
								{
								inv_hide();
								}
							});
					$('#inventory_search_popup input:text').bind('keydown', function(e){if(e.which == 13){$(this).inventory_query();}});
					$('#inventory_search_popup input:text').bind('keydown', function(e){if(e.which == 38){$(this).inventory_hide();}});
					});
			}

		function inv_hide(obj)
			{
			if(obj)
				{
				$(document).unbind('click');
				$('#inventory_search_popup').fadeOut('fast', function(){$(this).remove();$(obj).inventory();});
				}
			else
				{
				$(document).unbind('click');
				$('#inventory_search_popup').fadeOut('fast', function(){$(this).remove();});
				}
			var query			= $('#inventory_search_popup input:text').val();
			if(window.location.href.match(/quote/g))
				{
				$(parent_node).parent().parent().find('.description').val(query);
				}
			}
		return _bool;
		}
	$.fn.inventory_return	= function(master_id, real_master_id, tag_id, description)
								{
								if(window.location.href.match(/member\/quote/g))
									{
									$(parent_node).parent().parent().find('.part_n').html(real_master_id);
									$(parent_node).parent().parent().find('.code').html("&nbsp;");
									$(parent_node).parent().parent().find('.description').val(description);
									$(parent_node).focus();
									set_pricing(parent_node, true);
									//get_part_description(master_id, parent_node);
									}
								else if(window.location.href.match(/member\/inventory/g))
									{
									location.href		= "./index.aspx?a=get&tab=G&id="+real_master_id;
									}
								else if(window.location.href.match(/admin\/inventory/g))
									{
									location.href		= "./index.aspx?a=edit_part&master_id="+real_master_id;
									}
							    else if(window.location.href.match(/WOProgChangeLine/))
									{
								    var sellprice = "0";
								    $('.IsPartNo').val(real_master_id);
									$('.IsDesc').val(description);
									$('.IsPriceEach').val(sellprice);
									}
								else if(window.location.href.match(/NewWOProgShow/))
									{
									 var price = "0";
									 if($(parent_node).attr('id').match(/Text1/g))
									 {
									 $('.ASPxDescLable').html(description);
									 $('.ASPxMasterIDLabel').html(real_master_id);
									 $('.ASPxCodeLabel').html(real_master_id);
									 $('.ASPxSellPriceLabel').html(price);
									 $('.ASPxCodeLabel').click();
									 }
									 else
									 {
									   $('.txtpartID').val(real_master_id);
									   $('.txtDescription').val(description);
									   $('.txtQTY').focus();
									}
									}	
								else if(window.location.href.match(/WO/))
									{
								    var sellprice = "0"; 
								    $('.IsPartNo').val(real_master_id);
									$('.IsDesc').val(description);
									$('.IsPriceEach').val(sellprice);
									}
								else if (window.location.href.match(/PartFastAdd/g))
									{
									$('.txtpartID').val(real_master_id);
									$('.txtDescription').val(description);
									$('.txtQTY').focus();
									}
								else
									{
									$(parent_node).val(real_master_id);
									}
								$(document).unbind('click');
								if($("#aspnetForm"))
									{
									$("#aspnetForm").unbind('submit');
									}
								
								$('#inventory_search_popup').fadeOut('fast', function(){$(this).remove();});
								}
								
	$.fn.inventory_bv_return = function(_code, _description, _sellprice)
								{
								if(window.location.href.match(/member\/quote/g))
									{
									$(parent_node).parent().parent().find('.sellprice').val(_sellprice.toFixed(2));
									$(parent_node).parent().parent().find('.code').html(_code);
									$(parent_node).parent().parent().find('.part_n').html("&nbsp;");
									$(parent_node).parent().parent().find('.description').val(_description);
									$(parent_node).focus();
									//set_pricing(parent_node, real_master_id, tag_id);
									//get_part_description(master_id, parent_node);
									}
								else if(window.location.href.match(/NewWOProgShow/gi))
									{
									var price = "0";
									$('.ASPxDescLable').html(_description);
									$('.ASPxMasterIDLabel').html("0");
									$('.ASPxCodeLabel').html(_code);
									$('.ASPxSellPriceLabel').html(_sellprice.toFixed(2));
									$('.ASPxCodeLabel').click();
									}
								else if (window.location.href.match(/PartFastAdd/gi))
									{
									$('.txtpartID').val(_code);
									$('.txtDescription').val(_description);
									$('.txtQTY').focus();
									}	
								else if(window.location.href.match(/WOProgChangeLine/gi))
									{
									var message = "Do Not Use BV Parts";
									$('.IsDesc').val(message);
									}
								else if(window.location.href.match(/WO/gi))
									{ 
									var message = "Do Not Use BV Parts";
									$('.IsDesc').val(message);
									}
								else
									{
									$(parent_node).val(_code);
									}
								$(document).unbind('click');
								$('#inventory_search_popup').fadeOut('fast', function(){$(this).remove();});
								}
								
	$.fn.inventory_query	= function()
		{
		var isBV			= false;
		var query			= $('#inventory_search_popup input:text').val();
		var to_append		= "";
		if(query)
			{
		if(query.length >= 2)
			{
			var url;
			query				= query.replace(/^\s+|\s+$/g,"");
			if(query.match(/^BV:/gi))
				{
				isBV			= true;
				query			= query.replace(/^BV:/gi, "");
				url				= "/_tools/inventory_search/index.aspx?q="+query+"&BV=true";
				}
			else
				{
				isBV			= false;
				//query			= query.replace(/-/g, "");
				//query			= query.replace(/\"/g, "%22");
				query			= query.replace(/[ ,]/g, "|");
				url				= "/_tools/inventory_search/index.aspx?q="+query;
				//document.write(url);
				}
			$.ajax(
				{
				type:			'GET',
				url:			url,
				dataType:		'xml',
				beforeSend:		function()
									{
									$('#inventory_search_results').html("");
									$('#inventory_search_popup input:text').css({'background-image':"url('/images/loading_panel.gif')", 'background-repeat':'no-repeat', 'background-position':'center center'});
									$('#inventory_search_results').show();
									},
				success:		function(xml)
									{
									$('#inventory_search_results').html("");
									var size					= $(xml).find('part').size();
									if(size > 0)
										{
										if(isBV)
											{
											$(xml).find('part').each(function()
																		{
																		var _code				= unescape_me($(this).attr('code'));
																		var _sellprice			= $(this).attr('sellprice');
																		// onclick=\"$(this).inventory_return('"+tag_id+"-"+master_id+"', "+master_id+", "+tag_id+", $(this).children('.text').text())\"
																		var _description		= unescape_me($(this).attr('description'));
																		to_append				+= "\
																		<div style=\"cursor:pointer;background-color:#fff;background-repeat:repeat-x;background-image:url('/images/inventory/bg/bg[search_results].png');background-position:left bottom;padding-bottom:5px;padding-top:5px;border-bottom:solid 1px #bbb;background-image:text-align:left;\" onclick=\"$(this).inventory_bv_return('"+_code+"', $(this).children('.text').text(), "+_sellprice+")\">\
																			<div style='text-align:left;padding:2px;font-size:11px;' class='text'><b style='color:red;'>"+_code+"</b> - "+_description+"</div>\
																		</div>";
																		});
											}
										else
											{
											$(xml).find('part').each(function()
																		{
																		var tag					= $(this).attr('tag');
																		var tag_id				= $(this).attr('tag_id');
																		var master_id			= $(this).attr('master_id');
																		var description			= $(this).attr('description');//.replace(/,/g, "<br/>");
																		to_append				+= "\
																		<div style=\"cursor:pointer;background-color:#fff;background-repeat:repeat-x;background-image:url('/images/inventory/bg/bg[search_results].png');background-position:left bottom;padding-bottom:5px;padding-top:5px;border-bottom:solid 1px #bbb;background-image:text-align:left;\" onclick=\"$(this).inventory_return('"+tag_id+"-"+master_id+"', "+master_id+", "+tag_id+", $(this).children('.text').text())\">\
																			<div style='text-align:left;padding:2px;font-size:11px;' class='text'><b style='color:red;'>("+master_id+") "+tag+"</b> - "+description+"</div>\
																		</div>";
																		});
											}
										}
									else
										{
										to_append				= "<div align='center' style='font-family:arial;font-size:12px;font-weight:bold;padding:5px;'>No Matches.</div>";
										}
									},
				error:			function(xml, text_status, error)
									{
									alert(xml.responseText);
									},
				complete:		function(xml_status, text_status)
									{
									$('#inventory_search_results').html(to_append);
									$('#inventory_search_popup input:text').css({'background-image':""});
									}
				});
			}
		else
			{
			alert("Your submission (\""+query+"\") was too short.\nAll queries need to be at least 3 characters int.");
			}
			}
		}
	}
)(jQuery)
