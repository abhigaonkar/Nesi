
		var attribute_box;
		var value_box;
		var $n_prices			= $('#n_prices');
		var $x					= null;

		var branch_inventory	= {
			
			paste_image:
				{
					init:
					function () {
						$("#paste_box").pasteImageReader(
							function (results) {
								var dataURL, filename;
								$("#paste_box").css({ "border": "solid 2px #090", "background-repeat": "no-repeat", "background-size": "100%", "background-image": "url('" + results.dataURL + "')" });
								$(".inventory_file_blob").attr("value", results.dataURL);
								$(".inventory_cancelpasted").removeAttr('disabled');
								$("#_picture").val("").attr('disabled', true);
								return filename = results.filename, dataURL = results.dataURL, results;
							});
					},
					remove:
					function () {
						$("#paste_box").css({
							"border": "dashed 2px #999",
							"background-repeat": "no-repeat",
							"background-size": "auto",
							"background-image": "url('/images/ticket/bg[paste-sm].png')"
						});
						$(".inventory_cancelpasted").attr('disabled', true);
						$("#_picture").removeAttr('disabled');
						$(".inventory_file_blob").attr("value", "");
					}
				},
			toggle_consumable: null
			}
		function add_vendor()
			{
			}

		function working_branch(obj)
			{
			$(obj).attr('disabled', true);
			please_wait('begin', "Setting Active Branch");
			var vars			=	{
									a:				"working_branch",
									requested_id:	$(obj).val()
									}
			$.get("./index.aspx", vars,
					function(returned)
						{
						if(returned == "Success")
							{
							location.href = location.href;
							}
						else
							{
							alert(returned);
							$(obj).attr('disabled', false);
							}
						});
			}
		function fill_max(obj)
			{
			var max		= gv_orders.GetVisibleRowsOnPage();
			var o		= [];
			for(var i = 0;i < max ;i++)
				{
				var _parent			= $("#ctl00_cphMasterBody_uc_orders_tab_gv_orders tbody").find('tr[class*=dxgvDataRow]:eq('+i+')');
				if(_parent == null)
					{
					alert("There is a problem referencing the grid");
					return;
					}
				if(_parent.find(".master_id").size() == 0)
					{
					alert("Please make sure the 'master id' field is shown");
					return;
					}
				if(_parent.find(".qty_to_order").size() == 0)
					{
					alert("Please make sure the 'qty to order' field is shown");
					return;
					}
				var to_order_box	= _parent.find(".qty_to_order");
				var to_order_max	= _parent.find(".master_id").attr("data-toordermax");
				to_order_box.val(to_order_max);
				}
			}
		function resize_frame(type, height)
			{
			$("#"+type).animate({"height":height+50});
			}
		function legend(obj, show)
			{
			var target		= $("._legend");
			var txt			= $(obj).text();
			var width		= $(obj).width();
			if(show)
				{
				if(!txt.match(/^L\d+$/g))
					{
					$("#line_description").html("Description:<br/><div style='font-weight:normal;'>"+txt+"</div>").css({"background-color":"#ccc", "padding":"2px"});
					}
				target.css({"top":$(obj).offset().top+"px","left":($(obj).offset().left+width+10)+"px"});
				target.show();
				}
			else
				{	
				target.hide();
				}
			}
		function sh_rfq_panel(obj)
			{
			var o			=	{
								o:		$(obj).offset(),
								n:		$("#rfq_panel").size(),
								ow:		$(obj).width(),
								oh:		$(obj).height(),
								pw:		250,
								ph:		150
								};
			if(o.n == 0)
				{
				$("body").append("<div class='rfq_panel' id='rfq_panel'>\
				<table width='100%' cellpadding='10' cellspacing='0'><tr><td align='center' valign='middle' class='rfq_panel'><button class='rfq_panel dxbButton' style='width:100px;padding:10px;' type='button' onclick='rfq_dump(this, false)'>Cut New RFQ</button></td></tr>\
				<tr><td align='center' valign='middle' class='rfq_panel'><b style='color:#fff;' class='rfq_panel'>Use Existing RFQ:</b><br/><select class='rfq_panel' id='rfq_panel_select' style='width:150px;padding:5px;font-family:Tahoma;font-size:12px;'></select><button class='rfq_panel dxbButton' type='button' style='padding:7px;' onclick='rfq_dump(this, true)'>Use</button></td></tr></table></div>");
				up_rfq_panel();
				}
			
			var l			= o.o.left + (o.ow / 2) - (o.pw / 2);
			var t			= o.o.top + (o.oh / 2) - (o.ph / 2);

			$("#rfq_panel").css(	{
									"top"				: t+"px", 
									"left"				: l+"px", 
									"width"				: o.pw+"px", 
									"height"			: o.ph+"px",
									"border"			: "solid 2px #000",
									"display"			: "none",
									"background-color"	: "#999",
									"position"			: "absolute",
									"z-index"			: 150000
									}).show(0);
			setTimeout("$(document).mousemove(function(e){var original_element	= e.srcElement || e.originalTarget;if($(original_element).parents('#rfq_panel').size() == 0 && $(original_element).attr('id') != 'rfq_panel'){$('#rfq_panel').hide();$(document).unbind('mousemove');}});", 100);
			}
		
		function delete_multiple_locations(s,e)
			{
			var max		= gv_minmax.GetVisibleRowsOnPage();
			var o		= [];
			
			for(var i = 0;i < max ;i++)
				{
				var _parent		= $("#ctl00_cphMasterBody_gv_minmax_DXMainTable tbody").find('tr[class*=dxgvSelectedRow_NETheme01]:eq('+i+')');
				if(_parent.size() != 0)
					{
					var m_id		= _parent.find('.master_id').text();
					var lm_id		= _parent.find('.server_vals').attr("data-location_id");
					if(m_id != "")
						{
						var vals		=	{
											m_id:	m_id,
											lm_id:	lm_id
											};
						o.push(vals);
						}
					else
						{
						alert("You are missing the part # column, this is needed for this functionality");
						return;
						}

					}
				}
			if(o.length == 0)
				{
				alert("Please select some rows");
				}
			else
				{
				var query_string		=	{
											a:		"mass_delete_locations", 
											mmos:	JSON.stringify(o)
											};
				$.ajax( 
					{
					type:		"POST",
					cached:		false,
					url:		"./index.aspx",
					data:		query_string,
					dataType:	"text",
					beforeSend:	function()
									{
									gv_minmax.ShowLoadingPanel();
									},
					success:	function(ret)
									{
									if(ret != "SUCCESS")
										{
										alert(ret);
										}
									else
										{
										alert("Locations Removed, refreshing.");
										}
									},
					complete:	function()
									{
									gv_minmax.Refresh();
									gv_minmax.HideLoadingPanel();
									gv_minmax.HideLoadingDiv();
									}
					});
				}
			}
//---------------------------------------------------------------------
		function rfq_dump(obj, use_existing)
			{
			var max		= gv_orders.GetVisibleRowsOnPage();
			var parts	= [];
			var vends	= [];
			
			for(var i = 0;i < max ;i++)
				{
				var _parent		= $("#ctl00_cphMasterBody_uc_orders_tab_gv_orders tbody").find('tr[class*=dxgvDataRow]:eq('+i+')');
				if(_parent.find('.cb_inc').is(":checked"))
					{
					var c			= 0;
					var qty			= $.trim(_parent.find('.qty_to_order').val());
					var mid			= _parent.find('.vendor_name').attr("data-master_id");
					if(qty == "" || qty == "0" || qty == undefined || isNaN(qty))
						{
						alert("Invalid quantity for part #"+mid+" - Please fix");
						return false;
						}
					var part		=	{
										id:		mid / 1,
										q:		qty / 1
										};
					var vendor_id	= _parent.find('.vendor_name').attr("data-id");
					if(vendor_id == "" || vendor_id == "0" || vendor_id == undefined)
						{
						alert("Invalid vendor for part #"+mid+" - Please fix");
						return false;
						}
					if(vends.length > 0)
						{
						var exists		= false;
						for(var j = 0;j<vends.length;j++)
							{
							exists		= exists ? exists : vends[j] == vendor_id;
							}
						if(!exists)
							{
							vends.push(vendor_id);
							}
						}
					else
						{
						vends.push(vendor_id);
						}
					parts.push(part);
					}
				}
			if(vends.length == 0)
				{
				alert("Please select some parts...");
				return false;
				}
			var existing_rfq	= use_existing ? $("#rfq_panel_select").val() : 0;
			var query_string	=	{
									a:			"rfq_dump",
									r_id:		$("#rfq_panel_select").val(),
									p:			JSON.stringify(parts),
									v:			vends.join(",")
									};
			var success			= false;
			
			$.ajax( 
				{
				type:		"POST",
				cached:		false,
				url:		"./index.aspx",
				data:		query_string,
				dataType:	"text",
				beforeSend:	function()
								{
								gv_orders.ShowLoadingPanel();
								},
				success:	function(ret)
								{
								if(ret.indexOf("SUCCESS") == -1)
									{
									alert(ret);
									success		= false;
									}
								else
									{
									var rfq_info	= ret.split(",");
									var msg			= use_existing ? "Parts added to RFQ #"+rfq_info[1] : "New RFQ Cut - #"+rfq_info[1];
									msg				+= rfq_info[2] != "" ? "\n-----------------------\nThese warnings were returned also (you may need to alter the data):\n" : "";
									msg				+= rfq_info[2] != "" ? rfq_info[2].replace(/\|/g, "\n") : "";
									alert(msg);
									success		= true;
									}
								},
				complete:	function()
								{
								gv_orders.HideLoadingPanel();
								gv_orders.HideLoadingDiv();
								if(success)
									{
									gv_orders.Refresh();
									}
								}
				});
			}
//---------------------------------------------------------------------
		function up_rfq_panel()
			{
			var query_string	=	{
									a:			"xml_get_rfqs"
									};
			var options			= "<option value='0' selected>Choose RFQ";
			$.ajax({
					type:		"GET",
					cached:		false,
					url:		"./index.aspx",
					data:		query_string,
					dataType:	"xml",
					error:		function (xhr, ajaxOptions, thrownError)
									{
									},
					beforeSend:	function()
									{
									$("#rfq_panel_select").empty();
									},
					success:	function(xml)
									{
									$(xml).find('rfq').each(
										function()
											{
											var this_id			= $(this).children("id").text();
											var this_name		= $(this).children("name").text();
											options				+= "<option value='"+this_id+"'>("+this_id+") "+this_name+"</option>";
											});
									},
					complete:	function()
									{
									$("#rfq_panel_select").html(options);
									}});
			}
//---------------------------------------------------------------------
		function populate_browser()
			{
			var onchange_event	= "";
			var html			= "";
			var query_string	= {
									a:			"prepop_xml_tag_att_val",
									tag_id:		$("#tag_id").attr('data-id')
								  };
			if(query_string.tag_id == "")
				{
				browser_match(999999);
				}
			else
				{
				$.ajax({
						type:		"GET",
						cached:		false,
						url:		"./index.aspx",
						data:		query_string,
						dataType:	"xml",
						error:		function (xhr, ajaxOptions, thrownError)
										{
										$("#results").html(xhr.responseText);
										},
						beforeSend:	function()
										{
										$("#results").empty();
										$("#parts").empty();
										},
						success:	function(xml)
										{
										var i					= 0;
										var this_disabled		= "";
										var n_attributes		= $(xml).find('attribute').size();
										var type;
										$(xml).find('attribute').each(
											function()
												{
												var att_id						= $(this).attr("id");
												var att_name					= $(this).attr("name");
												html							+= "<div id='att_"+i+"'><input type='hidden' id='att_id' value='"+att_id+"'><input type='hidden' id='n' value='"+i+"'>";
												html							+= "<div class='att_name' style='margin-top: 5px;font-size: 11px;background-color: #036;color: white;font-weight: bold;text-align: left;padding: 5px;width: 200px;'>"+att_name+"</div><select id='val_"+i+"' class='val' onchange='browser_match();' style='font-size: 11px;font-weight: bold;cursor: pointer;border: solid 1px #090;width: 210px;margin-bottom: 10px;'><option value='0'>SELECT VALUE</option>";
												$(this).find('value').each(
													function()
														{
														var val_id				= $(this).attr('id');
														var val_name			= $(this).attr('name');
														html					+= "<option value='"+val_id+"' title=\""+val_name+"\">"+val_name+"</option>";
														});
												html							+= "</select></div>";
												i++;
												}
											);
										},
						complete:	function()
										{
										$("#results").show();
										$("#parts").empty();
										$("#results").append(html);
										browser_match(0);
										}
						});
				}
			}

		function os_h(obj)
			{
			$(obj).attr('disabled', true);
			please_wait("start");
			var _parent			= $(obj).parents('tr:first');
			var _defs			=	{
									a:				"adjust_minmax",
									master_id:		$(obj).attr('data-master_id'),
									poprog_id:		$(obj).attr('data-poprog_id'),
									min:			_parent.find('.qty_min').val() / 1,
									max:			_parent.find('.qty_max').val() / 1,
									onhand:			_parent.find('.qty_stock').val() / 1,
									delivery_date:	""
									};
			if(_defs.master_id == 777)
				{
				alert("You can't save a min, max or onhand for this part");
				$(obj).removeAttr('disabled');
				please_wait("stop");
				return false;
				}
				
			_defs.min				= _defs.min == "" ? 0 : _defs.min;
			_defs.max				= _defs.max == "" ? 0 : _defs.max;
			_defs.onhand			= _defs.onhand == "" ? 0 : _defs.onhand;
			_defs.delivery_date		= _parent.find("input[name$=delivery_date]").size() > 0 ? _parent.find("input[name$=delivery_date]").val() : "";
			var sd					= _defs.delivery_date != "" ? _defs.delivery_date.split("/") : "";
			_defs.delivery_date		= sd.length > 0 ? sd[2]+"-"+sd[0]+"-"+sd[1] : "";
			if(isNaN(_defs.min))
				{
				alert("Minimum value must be a number, and cannot be equal to or less than zero");
				please_wait("end");
				$(obj).removeAttr('disabled');
				_parent.find('.qty_min').focus().val("");
				return false;
				}
			if(isNaN(_defs.max))
				{
				alert("Maximum value must be a number, and must be greater than zero");
				please_wait("end");
				$(obj).removeAttr('disabled');
				_parent.find('.qty_max').focus().val("");
				return false;
				}
			if(isNaN(_defs.onhand))
				{
				alert("On hand quantity needs to be a number.");
				please_wait("end");
				$(obj).removeAttr('disabled');
				_parent.find('.qty_stock').focus().val("");
				return false;
				}
			$.get("./index.aspx", _defs, function(_returned)
								{
								if(_returned == "SUCCESS")
									{
									please_wait("end");
									$(obj).removeAttr('disabled');
									}
								else	
									{
									please_wait("end");
									$(obj).removeAttr('disabled');
									alert(_returned);
									}
								});
			}

		function browser_match(type)
			{
			if(!type)
				{
				type					= 0;
				}
			var max_atts				= $('.att_name').size();
			var tag_pattern				= "";
			var att_vals				= [];
			if(type != 999999)
				{
				for(var i=0;i<max_atts;i++)
					{
					var val_id				= $("#val_"+i).attr("value");
					if(val_id != "0")
						{
						att_vals[i]				= val_id;
						}
					}
				}
			var TagPattern				= att_vals.sort(sortNumber);
			var query_string			= {
											a:			"xml_browse_parts",
											tag_id:		$('#tag_id').attr('data-id'),
											vendor_id:	$('#vendor_id').attr('data-id'),
											attvals:	TagPattern.toString(),
											type:		type
										  };
			var html;
			$.ajax({	type:			"GET",
						cached:			false,
						url:			"index.aspx",
						data:			query_string,
						dataType:		"xml",	
						beforeSend:	function()
										{
										please_wait("start");
										$("#parts").empty();
										html	= "<div class='scrollbox'>\
														<table width='100%' cellpadding='1' cellspacing='0'>\
															<thead>\
																<tr>\
																	<th>#</th>\
																	<th>Description</th>\
																	<th>Ven Part #</th>\
																	<th>Ven Price</th>\
																	<th>Qty Per</th>\
																	<th>Leadtime</th>\
																	<th>Updated</th>\
																	<th colspan='3'>&nbsp;</th>\
																</tr>\
															</thead>\
															<tbody>\
														";
										},
						success:		function(xml)
											{
											$(xml).find('matches').each(
												function()
													{
													var size_of				= $(this).find('part').size();
													var tab_i				= 20;
													if(size_of > 0)
														{
														$(this).find('part').each(
															function()
																{
																var price_id		= $(this).find('price_id').text();
																var master_id		= $(this).find('master_id').text();
																var number			= $(this).find('number').text();
																var description		= $(this).find('description').text();
																var active			= $(this).find('active').text();
																var cost			= $(this).find('cost').text()/1;
																cost				= cost == 0 ? "" : cost.toPrecision();
																var qty				= $(this).find('qty').text();
																qty					= qty == 0 ? "" : qty;
																var last_updated	= $(this).find('last_updated').text();
																var lead_time		= $(this).find('lead_time').text();
																var vendor_code		= $(this).find('vendor_code').text();
																last_updated		= last_updated == "" ? "--" : last_updated;
																var checked			= "";
																if(active == "1")
																	{
																	checked			= "checked='true'";
																	}
																else
																	{
																	checked			= "";
																	}
																html				+= "<tr class='part' data-row_id='"+price_id+"' data-master_id='"+master_id+"'>\
																							<td class='master_id'><a href='/redir.aspx?url=%2Fsections%2Fmember%2Finventory%2Findex.aspx%3Fa%3Dget%26tab%3DG%26id%3D" + master_id + "' title='This will open in a new tab/window' target='_blank'>" + master_id + "</a></td>\
																							<td class='description'><div class='desc' data-tooltip=\""+description.replace('\"', '&quot;')+"\">"+description+"</div></td>";
																html				+= "<td class='vendor_code'><input type='text' tabindex='"+tab_i+"' style='width:125px;' data-original_value=\""+vendor_code+"\" value=\""+vendor_code+"\" /></td>"; tab_i++;
																html				+= "<td class='num'><input class='cost' type='text' tabindex='"+tab_i+"' style='width:50px;' data-original_value='"+cost+"' value='"+cost+"' /></td>"; tab_i++;
																html				+= "<td class='num'><input class='qty' type='text' tabindex='"+tab_i+"' onkeydown='only_int(event)' style='width:50px;' data-original_value='"+qty+"' value='"+qty+"' /></td>"; tab_i++;
																html				+= "<td class='num'><input class='lead_time' type='text' style='width:50px;' tabindex='"+tab_i+"' data-original_value='"+lead_time+"' value='"+lead_time+"' /></td>\
																							<td class='date' title='If no information is changed and you just save the row, this date will not change.'>"+last_updated+"</td>"; tab_i++;
																html				+= "<td class='action'><button type='button' tabindex='"+tab_i+"' onclick='browser_save(this)' title='Save Changes'><img src='/images/icon/icon[save].gif' /></button></td>"; tab_i++;
																html				+= "<td class='action'><button type='button' tabindex='"+tab_i+"' onclick='browser_undo(this)' title='Undo changes' ><img src='/images/icon/icon[undo].gif'/></button></td>"; tab_i++;
																html				+= "<td class='action'><button type='button' tabindex='"+tab_i+"' onclick='browser_delete(this)' title='Delete this vendor row'><img src='/images/icon/icon[delete].gif' /></button></td>\
																						</tr>"; tab_i++;
																});
														}
													else
														{
														html		+= "<tr class='nothing'><td>Nothing matches this criteria</td></tr>";
														}
													});
											html		+= "</tbody></table></div>";
											},
						complete:		function()
											{
											$("#parts").append(html);
											$("#parts").show();
											$(".desc").each(function(){$(this).tip({title:"Description"})});
											please_wait("end");
											},
						error:			function(xhr, ajaxOptions, thrownError)
											{
											document.write(xhr.responseText);
											}
						});
			}
			
		function browser_delete(row)
			{
			var p					= $(row).parents('tr:first');
			p.css({'background-color':'#fcc','text-decoration':'line-through', 'color':'#f00'});
			p.find('input').css({'text-decoration':'line-through', 'color':'#f00'});
			var price_id			= p.attr('data-row_id');
			if(price_id != "0" && confirm("Please confirm that you want to delete this vendor row?"))
				{
				please_wait('start');
				$.get('./index.aspx',
							{
							'a':		'delete_price',
							'price_id':	price_id
							},
							function(_returned)
								{
								if(_returned == "SUCCESS")
									{
									please_wait('end');
									if($("#results").children().size() > 0)
										{
										setTimeout("browser_match(0)", 500);
										}
									else
										{
										setTimeout("browser_match(999999)", 500);
										}
									}
								else
									{
									please_wait('end');
									alert(_returned);
									}
								});
				}
			else if(price_id == "0")
				{
				alert("This is not a populated row, nothing to delete.");
				p.css({'background-color':'#fff','text-decoration':'none', 'color':'#000'});
				p.find('input').css({'text-decoration':'none', 'color':'#000'});
				return false;
				}
			else
				{
				p.css({'background-color':'#fff','text-decoration':'none', 'color':'#000'});
				p.find('input').css({'text-decoration':'none', 'color':'#000'});
				return false;
				}
			}

		function browser_save(row)
			{
			please_wait('start');
			var p			= $(row).parents('tr:first');
			var t			= {
								price_id:		p.attr('data-row_id'),
								vendor_id:		$('#vendor_id').attr('data-id'),
								master_id:		p.attr('data-master_id'),
								//-----
								vendor_code:	$.trim(p.find('.vendor_code input').val()),
								costprice:		$.trim(p.find('.cost').val()),
								qty:			p.find('.qty').val(),
								lead_time:		p.find('.lead_time').val(),
								is_new:			(p.attr('data-row_id') == "0")
							  };
			if(t.vendor_code.length == 0)
				{
				alert("Please provide the vendor's part number");
				please_wait('end');
				p.find('.vendor_code input').focus();
				return false;
				}
			else if(t.costprice == "" || t.costprice == "0" || isNaN(t.costprice))
				{
				alert("Please provide a valid vendor price");
				please_wait('end');
				p.find('.cost').val("").focus();
				return false;
				}
			else if(t.qty == "" || t.qty == "0" || isNaN(t.qty))
				{
				alert("Please provide a valid quantity");
				please_wait('end');
				p.find('.qty').val("").focus();
				return false;
				}
			else if(t.lead_time == "" || t.lead_time == "0" || isNaN(t.lead_time))
				{
				alert("Please provide a valid lead time");
				please_wait('end');
				p.find('.lead_time').val("").focus();
				return false;
				}
			else
				{
				var _type		= t.is_new ? "new" : "edit";
				var save_defs	= {
									a:				_type+"_price",
									vendor_id:		t.vendor_id,
									part_number:	t.vendor_code,
									master_id:		t.master_id,
									price_id:		t.price_id,
									qty:			t.qty,
									costtotal:		(t.qty * t.costprice),
									costprice:		t.costprice,
									lead_time:		t.lead_time
								  };
				
				$.get('./index.aspx',save_defs,
							function(val)
								{
								if(val == "Success")
									{
									please_wait('end');
									if($("#results").children().size() > 0)
										{
										setTimeout("browser_match(0)", 500);
										}
									else
										{
										setTimeout("browser_match(999999)", 500);
										}
									}
								else
									{
									please_wait('end');
									alert(val);
									}
								});
				}
			}

		function browser_undo(row)
			{
			$(row).parents('tr:first').find('input:text').each(function()
																{
																if($(this).val() != $(this).attr('data-original_value'))
																	{
																	$(this).val($(this).attr('data-original_value'));
																	}
																});
			}

		function edit_sell(obj)
		    {
		    var _parent				= $(obj).parents('tr:first');
		    var vars				=	{
										a:				"edit_sell",
										business_unit_id:		_parent.attr('data-business_unit_id'),
										master_id:		$('#master_id').val(),
										sell_price:		_parent.find('input:text').val()
										};
		   $(obj).attr('disabled', true);
            $.get("./index.aspx", vars,
                        function(returned)
							{
							$(obj).removeAttr('disabled');
							if(returned.match(/^[\d\.]+/g))
								{
								location.href = location.href;
								}
							else
								{
								alert(returned);
								}
							});
		    }

		function reset_pricing()
			{
			$("#bv_part_number").val("").focus();
			$("#bv_pricing").html("&nbsp;");
			}

		function del_row_handler(obj)
			{
			$(obj).parents('tr:eq(0)').remove(); 
			}

		function start_part(obj)
			{
			var val;
			var tag_id;
			val			= $(obj).val();
			tag_id		= $(obj).attr('data-id');
			if(val != "" && tag_id != "" && tag_id != undefined )
				{
				build_att_val_boxes(tag_id);
				}
			}

		function add_price_row(obj)
			{
			var _tag_id		= $('#TAG_ID').attr('data-id');
			var _html			=	"\
								<tr class='yes'>\
									<td class='cb'><input title='include line' type='checkbox' checked='true' disabled/></td>\
									<td class='b' align='left'><input class='vendor' type='text' data-isac='false' data-tag_id='"+_tag_id+"' onfocus=\"attach_ac(this, 'vendor') \"/></td>\
									<td class='c'><input class='partnumber' type='text' /></td>\
									<td class='d'><input class='costinput' onkeyup='refactor_pricing(this)' type='text' value='0' /></td>\
									<td class='e'><input class='qtyperinput' onkeyup='refactor_pricing(this)' type='text' value='1' /></td>\
									<td class='g sellbase'>$0.00000</td>\
									<td class='i' width='25'><button onclick='del_row_handler(this)' type='button'>x</button></td>\
								</tr>";
			$('#new_pricing_body').append(_html);
			$(obj).next(".vendor").focus();
							
			}
			

																	//		<td><input style='text-align:center;' type='text' onkeyup='refactor_pricing(this)' onblur='check_avg(this)' class='costinput' value='0' /></td>\
																	//		<td><input style='text-align:center;' type='text' onkeyup='refactor_pricing(this)' onblur='check_avg(this)' class='qtyperinput' value='1' /></td>\
		function check_avg(obj)
			{
			var _baseprice		= 0;
			var _parent			= $(obj).parents('tr:first');
			var _cost			= parseFloat(_parent.find('.costinput').val());
			var	_qtyper			= parseFloat(_parent.find('.qtyperinput').val());
			var _tarcost		= _parent.find('.costinput');
			var _tarqtyper		= _parent.find('.qtyperinput');
			_baseprice			= (_cost / _qtyper).toFixed(5);
			var avgadjusted		= $(obj).parents('table:first').attr('data-avgadjusted_cost') / 1;
			var _tarbase		= _parent.find('.sellbase');
			if(_baseprice > avgadjusted)
				{
				if($(obj).attr('class') == "costinput" && _qtyper == 1)
					{
					}
				else
					{
					if(!confirm("The supplied vendor cost is over 25% higher than the average existing cost, are you sure you want to use this price?"))
						{
						_tarqtyper.val("1");
						_tarbase.text("$0.00");
						_tarcost.val("").focus();
						}
					}
				}
			
			}

		function refactor_pricing(obj)
			{
			var country			= $('#country').val();
			var _parent			= $(obj).parents('tr:first');
			var	_qtyper			= parseFloat(_parent.find('.qtyperinput').val());
			var _cost			= parseFloat(_parent.find('.costinput').val());
			var tag_uom_canada	= "";
			var tag_uom_usa		= "";
			var _branch_threshold	= $(obj).attr('data-avgadjusted_price');
			var _tarqtyper		= _parent.find('.qtyperinput');
			var _tarqtysold		= _parent.find('.qtysoldinput');
			var _tarcost		= _parent.find('.costinput');
			var _tarunit		= _parent.find('.sellunit');
			var _tarbase		= _parent.find('.sellbase');
			var _baseprice		= 0;
			_baseprice			= (_cost / _qtyper).toFixed(5);
			if(isNaN(_baseprice) && _tarcost.val() == ".")
				{
				_tarcost.val("0.");
				_tarqtyper.val("1");
				_tarbase.html('&nbsp;');
				}
			else
				{
				_baseprice		= (_baseprice / 1).toFixed(5);
				_tarbase.text('$'+_baseprice);
				}
			return true;
			}

		function clear_associated(obj)
			{
			$(obj).parents('tr:first').find('.linked_id').each(function()
																{
																$(this).removeAttr('data-id').removeAttr('data-isac').val('').focus();
																}
																);
			}

		function populate_barcodes()
			{
			var _returned			=	"";
			var _master_id			=	$('#MASTER_ID').val();
			var _defs				=	{
										type:			"GET",
										url:			"./index.aspx?a=xml_barcodes&master_id="+_master_id,
										dataType:		"xml",
										beforeSend:		function()
															{
															$("#inventory_branch .barcodes tbody").html("<tr><td colspan='4' align='center' style='padding-top:50px;'><img src='/images/loading_panel.gif'/></td></tr>");
															},
										success:		function(xml)
															{
															if($(xml).find("row").size() > 0)
																{
																$(xml).find("row").each(
																	function()
																		{
																		var _row		=	{
																							id:					$(this).find("id").text(),
																							barcode_no:			$(this).find("barcode_no").text(),
																							table_type:			$(this).find("table_type").text(),
																							table_id:			$(this).find("table_id").text(),
																							ref_name:			$(this).find("ref_name").text(),
																							dt_created:			$(this).find("dt_created").text(),
																							member_id_added:	$(this).find("member_id_added").text(),
																							member_name:		$(this).find("member_name").text(),
																							is_active:			$(this).find("is_active").text()
																							}
																		var _select		= _row.table_type == 'M' ? "<option value='V'>Vendor</option><option value='M' SELECTED>Manufacturer</option>" : "<option value='V' SELECTED>Vendor</option><option value='M'>Manufacturer</option>";
																		var _checkbox	= _row.is_active == 'True' ? "<input type='checkbox' checked='checked' />": "<input type='checkbox' />";
																		_returned		+= "\
																			<tr data-id='"+_row.id+"'>\
																				<td align='center'><select onchange='clear_associated(this)'>"+_select+"</select></td>\
																				<td align='center'><input type='text' class='linked_id' data-master_id='"+_master_id+"' data-id='"+_row.table_id+"' onfocus='attacher(this);' value=\""+_row.ref_name+"\"/></td>\
																				<td align='center'><input type='text' class='barcode' onkeydown='catch_enter(this)' value=\""+_row.barcode_no+"\"></td>\
																				<td align='center' width='20'>"+_checkbox+"</td>\
																				<td align='center'><button type='button' onclick='save_barcode(this)'><img src='/images/icon/icon[save].gif' align='absmiddle' /></button></td>\
																			</tr>";
																		});
																}
															else
																{
																_returned			= "<tr><td colspan='4' align='center' style='padding-top:50px;'>"+$(xml).find('error').text()+"</td></tr>";
																}
															},
										error:			function()
															{
															
															},
										complete:		function()
															{
															$("#inventory_branch .barcodes tbody").html(_returned);
															}
										}

			$x = $.ajax(_defs);
			}

		function save_barcode(obj)
			{
			$(obj).attr('disabled', true);
			var _parent			= $(obj).parents('tr:first');
			var _thead			= $(obj).parents('table:first').find('thead');
			var id				= _parent.attr('data-id') == undefined ? "" : _parent.attr('data-id');
			var _defs			=	{
									a:				"save_barcode",
									id:				id,
									master_id:		$('#MASTER_ID').val(),
									barcode_no:		_parent.find('.barcode').val(),
									table_type:		_parent.find('select').val(),
									table_id:		_parent.find('.linked_id').attr('data-id'),
									table_name:		_parent.find('.linked_id').val(),
									is_active:		_parent.find('input:checkbox').is(':checked')
									}
			if(_defs.table_id == undefined || _defs.table_id == null || _defs.table_id == "" || _defs.table_name == "")
				{
				alert("Associated Vendor / Manufacturer Not Set");
				$(obj).removeAttr('disabled');
				_parent.find('.linked_id').focus();
				return false;
				}
			else if(_defs.barcode_no == undefined || _defs.barcode_no == "")
				{
				alert("Barcode # Not Set");
				$(obj).removeAttr('disabled');
				_parent.find('.barcode').focus();
				return false;
				}
			else
				{
				please_wait('begin');
				$.get('./index.aspx', _defs, 
					function(_ret)
						{
						if(id == "")
							{
							_thead.find('select').val('V');
							_thead.find('.linked_id').attr('data-id', '').val('');
							_thead.find('.barcode').val('');
							_thead.find('button').removeAttr('disabled');
							}
						populate_barcodes();
						please_wait('end');
						});
				}
			}

		function validate_benchmark(obj)
			{
			var current_state		= $(obj).is(":checked");
			if(current_state)
				{
				$("#this_branch").find("input:checkbox.bench").each(
					function()
						{
						$(this).removeAttr("checked");
						});
				$(obj).attr("checked", true);
				}
			}

		function validate_preferred(obj)
			{
			var current_state		= $(obj).is(":checked");
			if(current_state)
				{
				$("#this_branch").find("input:checkbox.is_preferred").each(
					function()
						{
						$(this).removeAttr("checked");
						});
				$(obj).attr("checked", true);
				}
			}

		function push_price(obj, _new)
			{
			if($(obj).attr("disabled") == "disabled")
				{
				return false;
				}
			var _type				= "";
			if(_new)
				{
				_type				= "new";
				}
			else
				{
				_type				= "edit";
				}
			please_wait('begin');
			var _parent			= $(obj).parents('tr:first');
			var _master_id		= $('#MASTER_ID').val();
			var _vendor_id		= _parent.find('.vendor').attr('data-id');
			var _price_id		= _parent.attr('data-priceid');
			var _reference_id	= _parent.attr('data-referenceid');
			var _lead_time		= _parent.find('.lead_time').children('input:text').val();
			var _part_number	= _parent.find('.partnumber').val();
			var	_qty			= parseFloat(_parent.find('.qtyperinput').val());
			var _benchmark		= _new ? false : _parent.find(".bench").is(":checked");
			var _is_preferred	= _new ? false : _parent.find(".is_preferred").is(":checked");
			if(_vendor_id == "" || _vendor_id == null || _vendor_id == undefined)
				{
				alert("Vendor is not set");
				please_wait('end');
				return false;
				}
			if(_lead_time == "")
				{
				alert("Lead Time is not set");
				please_wait('end');
				return false;
				}
			if(_qty <= 0)
				{
				alert("Quantity must be greater than or zero.");
				_parent.find('.qtyperinput').val('1');
				refactor_pricing(obj);
				please_wait('end');
				return false;
				}
			var _costtotal		= parseFloat(_parent.find('.costinput').val());
			var _costprice		= (_costtotal / _qty).toFixed(5);
			
			if(parseFloat(_costprice) <= 0 || isNaN(parseFloat(_costprice)))
				{
				alert("Pricing is not set");
				_parent.find('.costinput').val('0');
				refactor_pricing(obj);
				please_wait('end');
				return false;
				}
			$(obj).attr('disabled', true);
			$.get('./index.aspx',
						{
						a:				_type+"_price",
						vendor_id:		_vendor_id,
						part_number:	_part_number,
						master_id:		_master_id,
						lead_time:		_lead_time,
						price_id:		_price_id,
						qty:			_qty,
						costtotal:		_costtotal,
						costprice:		_costprice,
						benchmark:		_benchmark,
						is_preferred:	_is_preferred
						},
						function(val)
							{
							if(val == "Success")
								{
								populate_pricing(1, "", false, true, true);
								}
							else
								{
								alert(val);
								}
							$(obj).attr('disabled', false);
							please_wait('end');
							});
			}

		function chk_rowstate()
			{
			$("#this_branch").find("tbody tr").each(
					function()
						{
						var _parent			= $(this);
						var cur				=	{
												vendor_id:		_parent.find('.vendor').attr('data-id'),
												benchmark:		_parent.find(".bench").is(":checked"),
												is_preferred:	_parent.find(".is_preferred").is(":checked"),
												vendor_code:	_parent.find('.partnumber').val(),
												cost:			parseFloat(_parent.find('.costinput').val()),
												qty:			parseFloat(_parent.find('.qtyperinput').val())
												}
						var ori				=	{
												vendor_id:		_parent.attr('data-orig_vendor_id'),
												benchmark:		(_parent.attr('data-orig_benchmark') == "true"),
												is_preferred:	(_parent.attr('data-orig_is_preferred') == "true"),
												vendor_code:	_parent.attr('data-orig_vendor_code'),
												cost:			parseFloat(_parent.attr('data-orig_cost')),
												qty:			parseFloat(_parent.attr('data-orig_qty'))
												};
						var has_changed		=	cur.vendor_id != ori.vendor_id || 
												cur.benchmark != ori.benchmark ||
												cur.is_preferred != ori.is_preferred ||
												cur.vendor_code != ori.vendor_code ||
												cur.cost != ori.cost ||
												cur.qty != ori.qty;
						if(has_changed)
							{
							_parent.find('.save').attr('src', '/images/icon/icon[save].gif');
							}
						else
							{
							_parent.find('.save').attr('src', '/images/icon/icon[save][disabled].gif');				
							}
						});
			}

		function populate_pricing(_use, _disabler, _historic, _can_edit, _see_cost)
			{
			var master_id			= $('#MASTER_ID').val();
			var _html				= "";
			var _url				= "";
			if(_historic)
				{
				_url				= "./index.aspx?a=xml_price_history&master_id="+master_id+"&use="+_use;
				}
			else
				{
				_url				= "./index.aspx?a=xml_price_info&master_id="+master_id+"&use="+_use;
				}
			var counter				= 0;
			$x = $.ajax({	type:		"GET",
					url:		_url,
					dataType:	"xml",
					beforeSend:	function()
									{
									var this_html			= "<div align='center' style='padding:50px;'><b><blink>Refreshing Pricing</blink></b><br/><img vspace='5' src='/images/loading_panel.gif'/></div>";
									switch(_use)
										{
										case 0: $('#not_this_branch').empty().append(this_html);
										break;
										case 1:	$('#this_branch').empty().append(this_html);
										break;
										}
									},
					success:	function(xml)
									{
									var country			= $('#country').val();
									var tag_uom_canada			= $('#canadian_sold_as').val();
									var tag_uom_usa				= $('#usa_sold_as').val();
									var tag_uom					= "";
									var tag_uom_name			= country == "CDN" ? tag_uom_canada : tag_uom_usa;
									switch(tag_uom_name)
										{
										case "1":
											tag_uom_name	= "/ft";
										break;
										case "2":
											tag_uom_name	= "/m";
										break;
										case "3":
											tag_uom_name	= "/ea";
										break;
										}
									var _total_price				= 0;
									var _prices_qty					= $(xml).find('price').size();
									if(_use == 1 && _disabler == "" && !_historic)
										{
										if($(xml).find('price').size() > 0)
											{
											$(xml).find('price').each(function()
																		{
																		_total_price		+= $(this).children('cost').text() /1;
																		});
											}
										}
									var _avg_cost					= _total_price / _prices_qty;
									if(_historic)
										{
										_html					+= "<table width='100%' cellpadding='0' cellspacing='0' class='tablesorter'>";
										}
									else
										{
										_html					+= "<table width='100%' data-avgadjusted_cost='"+(_avg_cost*1.25).toFixed(5)+"' cellpadding='0' cellspacing='0' class='pricing'>";
										}
									var _tooltip			= _use == 0 ? "Converted to your branches unit sold as" : "";
									var _colspan			= _historic ? "5" : "4";
									_html					+= "\
																<thead>\
																	<tr>\
																		<th width='80'>Branch</th>";
									if(_historic)
										{
										_html					+= "\
																		<th width='125'>Edited DT</th>";
										}
									else if(_see_cost)
										{
										_html			+= "\   <th width='45'>Pref.</th>";
										}
									_html					+= "\
																		<th width='35'>Note</th>\
																		<th width='20' title='Lead time is in days'>Lead Time</th>\
																		<th width='100'>Vendor</th>\
																		<th width='100'>Vendor Part #</th>";
									if(_see_cost)
										{
										_html					+= "\
																		<th width='50' title='For this Vendor Part #'>Cost</th>\
																		<th width='30' title=\"Of our part#, in the vendor's part#\">Qty</th>\
																		<th width='75' title='"+_tooltip+"'>Cost<br/><i style='font-size:9px;'>Base PR.</th>";
										}
									_html					+= "\
																		<th width='50' title='Unit of Measure sold as'>UOM</th>";
									if(_historic)
										{
										_html							+= "<th width='35'>Origin</th>";
										}
									else
										{
										_html							+= "<th width='35'>&nbsp;</th>";
										}
									_html								+= "</tr>\
																</thead>";
									var business_unit_id						= $("#branch_selector").val();
									if(_use == 1 && _see_cost && _can_edit && !_historic)
										{
										_html			+= "		<tfoot>\
																		<tr>\
																			<td colspan='"+_colspan+"' style='background-color:#fff;border-bottom:0;'>&nbsp;<input type='hidden' value='' class='price_id'/></td>\
																			<td><input type='text' class='vendor' data-isac='false' onfocus=\"attach_ac(this, 'vendor')\"  data-master_id='"+master_id+"' data-business_unit_id='"+business_unit_id+"' value='' /></td>\
																			<td><input type='text' class='partnumber' value='' /></td>\
																			<td><input style='text-align:center;' type='text' onkeyup='refactor_pricing(this)' onblur='check_avg(this)' class='costinput' value='0' /></td>\
																			<td><input style='text-align:center;' type='text' onkeyup='refactor_pricing(this)' onblur='check_avg(this)' class='qtyperinput' value='1' /></td>\
																			<td align='center'><b class='sellbase'>$0.00</b></td>\
																			<td align='center'><b class='sellunit'>"+tag_uom_name+"</b></td>\
																			<td width='35' align='center'><button onclick='push_price(this, true);' style='width:50px;margin-top:0px;margin-bottom:0px;' type='button'><img src='/images/icon/icon[send].gif' style='margin-top:0px;margin-bottom:0px;' /></button></td>\
																		</tr>\
																	</tfoot>\
																	<tbody>";
										}
									else
										{
										_html				+= "\
																	<tbody>";
										}
									counter							= $(xml).find('price').size();
									if($(xml).find('price').size() > 0)
										{
										$(xml).find('price').each(function()
																	{
																	var dsn					= $(this).children('dsn').text();
																	var business_unit_id			= $(this).children('business_unit_id').text();
																	var lead_time			= $(this).children('lead_time').text();
																	var last_edited			= $(this).children('last_edited').text();
																	var price_age			= $(this).children('price_age').text();
																	var tag_price_expiry	= $(this).children('tag_price_expiry').text();
																	var qty					= $(this).children('qty').text();
																	var reference_id		= $(this).children('reference_id').text();
																	var vendor_name			= $(this).children('vendor_name').text();
																	var member_name			= $(this).children('member').text();
																	var vendor_id			= $(this).children('vendor_id').text();
																	var vendor_number		= $(this).children('vendor_number').text();
																	var note				= $(this).children('note').text();
																	var price_id			= $(this).children('price_id').text();
																	var benchmark			= ($(this).children('benchmark').text() == 1);
																	var is_preferred		= ($(this).children('is_preferred').text() == 1);
																	var part				= unescape($(this).children('part').text()).replace(/\"/g, "&quot;");
																	var cost				= $(this).children('cost').text();
																	var country				= $(this).children('country').text();
																	var sold_as				= "";
																	var sold_as_id			= "";
																	var origin				= $.trim($(this).children('origin').text());
																	var origin_text			= $.trim($(this).children('origin').text());
																	var last_cost			= $(this).children('last_cost').text() / 1;
																	var last_date			= $(this).children('last_date').text();
																	var last_po				= $(this).children('last_po').text();
																	var origin_match		= /PO:(.+)/i.exec(origin);
																	var poprog_id			= $(this).children('poprog_id').text();
																	if(origin == undefined || origin == "undefined")
																		{
																		origin				= "";
																		}
																	if(origin_match)
																		{
																		origin				= poprog_id != "" ? "<strong>NESI PO</strong><br/><a href='/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid="+poprog_id+"'>"+origin_match[1]+"</a>" : "<strong>BV PO</strong><br/>"+origin_match[1];
																		origin_text			= poprog_id != "" ? "<strong>NESI PO</strong><br/>"+origin_match[1] : "<strong>BV PO</strong><br/>"+origin_match[1];
																		}
																	if(country == "CDN")
																		{
																		sold_as				= $(this).children('canadian_sold_as').text();
																		sold_as_id			= $(this).children('canadian_sold_as_id').text();
																		}
																	else
																		{
																		sold_as				= $(this).children('usa_sold_as').text();
																		sold_as_id			= $(this).children('usa_sold_as_id').text();
																		}
																	var tag_id				= $(this).children('tag_id').text();
																	var _savedisabled		= "";
																	var _savetext			= "";
																	var _copydisabled		= "";
																	var _button				= "";
																	var _copytext			= "";
																	var _selected			= "";
																	var total_cost			= $(this).children('total').text().replace(",","") / 1;
																	var per_unit_cost		= $(this).children('cost').text();
																	var base_cost			= $(this).children('base_cost').text();
																	var _select;
																	if(!_historic && tag_id != '571' && tag_id != '704')
																		{
																		if(_use == 1 && _can_edit)
																			{
																			_savedisabled		= "";
																			_button				= "<img src='/images/icon/icon[save].gif' onclick='push_price(this, false);' class='save' title='Save this row' "+_savedisabled+"/><img src='/images/icon/icon[delete].gif' onclick='delete_price(this, false);' title='Delete this vendor line' "+_savedisabled+"/>";
																			}
																		else if(_use == 0 && _can_edit)
																			{
																			_savedisabled		= "disabled='disabled' ";
																			_button				= "<img src='/images/icon/icon[copy].gif' title='Copy this vendors price to your branch' onclick='copy_price(this, false);' "+_savedisabled+"/>";
																			}
																		else
																			{
																			_savedisabled		= "disabled='disabled'";
																			_button				= origin;
																			}
																		}
																	else
																		{
																		_savedisabled		= "disabled='disabled'";
																		_button				= origin;
																		}
																	var row_style		= "";
																	var _tooltip		= "<table>";
																		if(member_name != ""){ _tooltip += "<tr><td width='125'><strong>Last Edited By:</strong></td><td width='200'>"+member_name+"</td></tr>"; }
																		if(last_edited != ""){ _tooltip += "<tr><td><strong>Edit Timestamp:</strong></td><td>"+last_edited+"</td></tr>"; }
																		if(origin_text != ""){ _tooltip += "<tr><td><strong>Last Update Origin:</strong></td><td>"+origin_text+"</td></tr>"; }
																		if(last_po != ""){ _tooltip += "<tr><td><strong>Last PO#:</strong></td><td>"+last_po+"</td></tr>"; }
																		if(last_date != ""){ _tooltip += "<tr><td><strong>Last PO Date:</strong></td><td>"+last_date+"</td></tr>"; }
																		if(last_cost != "" && _see_cost){ _tooltip += "<tr><td><strong>Last Cost on PO:</strong></td><td>$"+last_cost+"</td></tr>"; }
																		if(note != ""){ _tooltip += "<tr><td><strong>Note:</strong></td><td width='200'>"+note+"</td></tr>"; }
																	_tooltip += "</table>";
																	_html				+= "\
																			<tr data-note=\""+note+"\" data-tooltip=\""+_tooltip+"\" class='tr' data-referenceid='"+reference_id+"' data-priceid='"+price_id+"' "+row_style+" data-orig_benchmark=\""+benchmark+"\" data-orig_vendor_id=\""+vendor_id+"\" data-orig_vendor_code=\""+part+"\" data-orig_cost=\""+total_cost.toFixed(4)+"\" data-orig_qty=\""+qty+"\">\
																				<td class='dsn' align='left'>"+dsn+"</td>";
																	var benchmark_checked		= benchmark ? " checked='checked'" : "";
																	var is_preferred_checked	= is_preferred ? " checked='checked'" : "";
																	if(_historic)
																		{
																		_html			+= "\
																				<td class='lead_time' align='center'>"+last_edited+"</td>";
																		}
																	else if(_use == 1 && _see_cost & _can_edit)
																		{
																		_html			+= "\<td><input type='checkbox' class='is_preferred' value='' onchange='chk_rowstate(this)' onclick='validate_preferred(this)' "+is_preferred_checked+"/></td>";
																		}
																	else if(_see_cost)
																		{
																		_html			+= "\<td><input type='checkbox' class='is_preferred' value='' "+is_preferred_checked+" disabled/></td>";
																		}
																	var note_img		= note.length > 0 ? "note" : "note_blank";
																	var note_click		= _use == 1 && !_historic ? "<img onclick='note_show(this);' data-type='pricing' src='/images/icon/icon["+note_img+"].gif'/>" : "<img src='/images/icon/icon["+note_img+"].gif'/>";
																	_html				+= "\
																				<td>"+note_click+"</td>\
																				<td class='lead_time' align='center'><input type='text' onkeydown='only_int(event)' style='text-align:center' value='"+lead_time+"' "+_savedisabled+"/></td>\
																				<td><input type='text' class='vendor' data-isac='false' title='#"+vendor_id+" " + vendor_name + "' onfocus=\"attach_ac(this, 'vendor')\" data-id='"+vendor_id+"' data-business_unit_id='"+business_unit_id+"' data-master_id='"+master_id+"' value=\"("+vendor_number+") "+vendor_name+"\" "+_savedisabled+"/></td>\
																				<td align='center'><input type='text' class='partnumber' value=\""+part+"\" "+_savedisabled+"/></td>";
																	if(_see_cost)
																		{
																		_html				+= "\
																				<td><input style='text-align:center;' type='text' onkeyup='refactor_pricing(this)' onblur='check_avg(this)' class='costinput' value='"+total_cost.toFixed(4)+"' "+_savedisabled+"/></td>\
																				<td><input style='text-align:center;' type='text' onkeyup='refactor_pricing(this)' onblur='check_avg(this)' class='qtyperinput' value='"+qty+"' "+_savedisabled+"/></td>\
																				<td align='center'><b class='sellbase' title='Originally $"+cost+" /"+sold_as+"'>$"+(base_cost /1).toFixed(4)+"</b></td>";
																		}
																	_html				+= "\
																				<td align='center'><b class='sellunit'>/"+sold_as+"</b></td>\
																				<td width='60' nowrap='nowrap' align='center'>"+_button+"</td>\
																			</tr>";
																	});
											_html					+= "	</tbody>\
																		</table>\
					<div id='pages_"+_use+"' align='center' style='display:none;'>\
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
					</div><br/><br/><br/>";
										}
									else
									    {
									    _html                           += "	</tbody>\
																		</table><br/>";
									    }
									},
					complete:	function()
									{
									switch(_use)
										{
										case 0: 
											if(_html.length == 0)
									            {
									            _html       = "<center style='padding:25px'>Pricing Not Available</center>";
									            }
									        $('#not_this_branch').html(_html);
									        $('body').find('.tr').each(function()
																					{
																					$(this).tip({title: 'Line Information'});
																					});
									        if(_historic && counter > 0)
												{
												$('#not_this_branch').find('table').tablesorter({
																						sortList:	[[0,0],[1,0]],
																						headers:	{ 
																									0: {sorter:'text'}, 
																									1: {sorter:'date'},
																									2: {sorter:'text'},
																									3: {sorter:'text'},
																									4: {sorter:'currency'},
																									5: {sorter:'number'},  
																									6: {sorter:'currency'},
																									7: {sorter:'text'},
																									8: {sorter:'text'}      
																									}
																						}).tablesorterPager({positionFixed:false, size:10,container: $("#pages_"+_use)});
												if(counter > 10)
													{
													$("#pages_"+_use).show();
													}
												}
										break;
										case 1: 
											if(_html.length == 0)
									            {
									            _html       = "<center style='padding:25px'>Pricing Not Available</center>";
									            }
									        $('#this_branch').html(_html);
									        if(_historic && counter > 0)
												{
												$('#this_branch').find('table').tablesorter({
																						sortList:	[[0,0],[1,0]],
																						headers:	{ 
																									0: {sorter:'text'}, 
																									1: {sorter:'date'},
																									2: {sorter:'text'},
																									3: {sorter:'text'},
																									4: {sorter:'currency'},
																									5: {sorter:'number'},  
																									6: {sorter:'currency'},
																									7: {sorter:'text'},
																									8: {sorter:'text'}      
																									}
																						}).tablesorterPager({positionFixed:false, size:10,container: $("#pages_"+_use)});
												if(counter > 10)
													{
													$("#pages_"+_use).show();
													}
												}
									        $('body').find('.tr').each(function()
																					{
																					$(this).tip({title: 'Line Information', width: '450px'});
																					});
										break;
										}
										
									},
					error:		function()
									{
									
									}
					});
			}
		function save_rfq_vendor(s, e)
			{
			please_wait("start");
			var _defs		=	{
								a:				"save_rfq_vendor",
								vendor_id:		$(".t_vendor").attr("data-id"),
								contact_id:		$(".s_contact").val()
								}
			$.get("./index.aspx", _defs,  function(_returned)
								{
								if(_returned == "SUCCESS")
									{
									please_wait("stop");
									gv_vendors.Refresh();
									$(".t_vendor").unbind();
									$(".t_vendor").focus(function()
													{
													attach_ac($(".t_vendor"), which);
													});
									$(".t_vendor").val('');
									$(".t_vendor").attr('data-isac', 'false');
									$(".t_vendor").attr('data-id', '');
									$(".t_vendor").attr('title', '');
									$(".s_contact").html("Pick a Contact").attr("disabled", true);
									}
								else	
									{
									please_wait("stop");
									alert(_returned);
									}
								});
			}
		function note_show(obj)
			{
			var exists			= $("#note_window").size() > 0;
			var type			= $(obj).attr('data-type');
			var note			= type == "pricing" ? $(obj).parents("tr:first").attr("data-note") : $(obj).attr('data-note');
			var id				= type == "pricing" ? $(obj).parents("tr:first").attr("data-priceid") : $(obj).attr('data-id');
			if(exists && $("#note_window"))
				{
				$("#note_window").remove();
				}
			var html			= "<div id='note_window' data-id='"+id+"'>";
			html				+= "<b>Note:</b>\
									<br/><textarea style='width:99%;height:100px;' maxlength='512'></textarea>\
									<br><button style='font-size:11px;font-weight:bold;' onclick='handle_note(this)' data-id='"+id+"' data-type='"+type+"' type='button'><img align='absmiddle' src='/images/icon/icon[save].gif'/> Save</button>\
									<button style='font-size:11px;font-weight:bold;' onclick=\"$('#note_window').remove();\"><img align='absmiddle' src='/images/icon/icon[delete].gif'/> Close</button>";
			html				+= "</div>";
			$("body").append(html);
			var off			= $(obj).offset();
			$("#note_window").css({	"position"			: "absolute",
									"top"				: off.top+"px", 
									"left"				: off.left+25+"px",
									"width"				: "300px",
									"padding"			: "5px",
									"border"			: "solid 1px #cc7",
									"background-color"	: "#ff7"
									}).find("textarea").focus().val(note);
			}
		function handle_note(obj)
			{
			//$(obj).attr("disabled", true);
			//please_wait("start");
			var _defs				=   {
										a:			"handle_note",
										id:			$(obj).attr('data-id'),
										type:		$(obj).attr('data-type'),
										note:		$("#note_window").find("textarea").val()
										};
			$.get("./index.aspx", _defs, function(_returned)
								{
								if(_returned == "SUCCESS")
									{
									if(_defs.type == "consignment")
										{
										$('#note_window').remove();
										please_wait("stop");
										gv_consignment.Refresh();
										}
									else
										{
										$('#note_window').remove();
										please_wait("stop");
										populate_pricing(1, '', false, true, true);
										}
									}
								else	
									{
									please_wait("stop");
									$(obj).removeAttr('disabled');
									alert(_returned);
									}
								});
			}
		var q		= 0;
		function cb_incall(obj)
			{
			$('.cb_inc').each(function()
								{
								var _parent			= $(this).parents('tr:first');
								var vendor_name		= _parent.find('.vendor_name');
								if(vendor_name.attr('data-default_vendor_id') != 'N/A')
									{
									$(this).attr('checked', $(obj).is(":checked"));
									q++;
									preload_vendor($(this));
									}
								});
			if(!$(obj).is(":checked"))
				{
				$("input:text").each(function()
							{
							$(this).css({"border":"solid 1px #abadb3"});
							});
				}
			}
		function preload_vendor(obj)
			{
			var _parent					= $(obj).parents('tr:first');
			var vendor_name				= _parent.find('.vendor_name');
			if(vendor_name.attr('data-default_vendor_id') != 'N/A' && vendor_name.val() == "")
				{
				vendor_name.attr('data-id', vendor_name.attr('data-default_vendor_id')).val(vendor_name.attr('data-default_vendor_name')).each(function(){order_v_info(vendor_name, false)});
				}
			}
		function order_v_info(obj, from_ac)
			{
			var _parent					= $(obj).parents('tr:first');
			var cb						= _parent.find('.cb_inc');
			var vendor_name				= _parent.find('.vendor_name');
			var vendor_code				= _parent.find('.vendor_code');
			var vendor_price			= _parent.find('.vendor_price');
			var vendor_qty				= _parent.find('.vendor_qty');
			var vendor_lead = _parent.find('.vendor_lead');
			var vendor_date = _parent.find('.vendor_date');
			if(from_ac == undefined || from_ac == false)
				{
				vendor_code.val(vendor_name.attr('data-vendor_code'));
				vendor_price.val(vendor_name.attr('data-cost'));
				vendor_qty.val(vendor_name.attr('data-qty_per'));
				vendor_lead.val(vendor_name.attr('data-lead'));
                vendor_date.val(vendor_name.attr('data-_date'));
				}
			else
				{
				gv_orders.ShowLoadingPanel();
				$.get('./index.aspx',
							{
							    'a': 'json_vend_info',
							    'master_id': $(obj).attr('data-master_id'),
							    'vendor_number': $(obj).attr('data-id')
							},
							function (_returned) {
							    var r = $.parseJSON(_returned);
							    var p = r.cost / 1 == 0 ? null : r.cost;
							    cb.attr('checked', true);
							    vendor_price.val(p);
							    vendor_code.val(unescape(r.vendor_code));
							    vendor_qty.val(r.qty);
							    vendor_lead.val(r.lead);
							        vendor_date.val(r._date);
							   
							    gv_orders.HideLoadingPanel();
							    gv_orders.HideLoadingDiv();
							});
				}
			}
		function delete_price(obj)
			{
			if($(obj).attr("disabled") == "disabled")
				{
				return false;
				}
			$.ajaxSetup({cache: false})
			$(obj).attr('disabled', true).attr('title', 'Please Wait, Deleting...');
			var _parent			= $(obj).parents('tr:first');
			var _price_id		= _parent.attr('data-priceid');
			$.get('./index.aspx',
						{
						'a':		'delete_price',
						'price_id':	_price_id
						},
						function(_returned)
							{
							if(_returned == "SUCCESS")
								{
								populate_pricing(1, "", false, true, true);
								populate_pricing(0, "1", false, true, true);
								}
							else
								{
								alert("There was an issue deleting this price from your branch.");
								populate_pricing(1, "", false, true, true);
								populate_pricing(0, "1", false, true, true);
								}
							});
			}
		function copy_price(obj)
			{
			$.ajaxSetup({cache: false})
			$(obj).attr('disabled', true).attr('title', 'Please Wait, Copying...');
			var _parent			= $(obj).parents('tr:first');
			var query_string	=	{
									'a':		'copy_price',
									'price_id':	_parent.attr('data-priceid')
									};
			$.ajax(		{
						type:			"GET",
						url:			"./index.aspx",
						datatype:		"text",
						data:			query_string,
						cache:			false,
						beforeSend:	
							function()
								{
								please_wait("start", "Copying Price...");
								},
						success:
							function(resp)
								{
								if(resp == "SUCCESS")
									{
									populate_pricing(1, "", false, true, true);
									populate_pricing(0, "1", false, true, true);
									}
								else
									{
									alert("There was an issue copying this price to your branch, an email has been sent to this page's author for debugging.");
									populate_pricing(1, "", false, true, true);
									populate_pricing(0, "1", false, true, true);
									}
								},
						complete:
							function()
								{
								please_wait("stop");
								},
						error:
							function()
								{
								alert("There was an error copying the price");
								}
						});
			}
		function adjust_minmax(obj)
			{
			$(obj).attr('disabled', true);
			please_wait("start");
			var _parent			= $(obj).parents('table:first');
			var _defs			=	{
									a:				"adjust_minmax",
									master_id:		$('#MASTER_ID').val(),
									min:			$('#min').val() / 1,
									max:			$('#max').val() / 1,
									onhand:			$('#onhand').val() / 1
									}
			_defs.min				= _defs.min == "" ? 0 : _defs.min;
			_defs.max				= _defs.max == "" ? 0 : _defs.max;
			_defs.onhand			= _defs.onhand == "" ? 0 : _defs.onhand;
			if(isNaN(_defs.min))
				{
				alert("Minimum value must be a number");
				please_wait("end");
				$(obj).removeAttr('disabled');
				$('#min').focus().val("");
				return false;
				}
			if(isNaN(_defs.max))
				{
				alert("Maximum value must be a number");
				please_wait("end");
				$(obj).removeAttr('disabled');
				$('#max').focus().val("");
				return false;
				}
			if(isNaN(_defs.onhand))
				{
				alert("On hand value must be a number.");
				please_wait("end");
				$(obj).removeAttr('disabled');
				$('#onhand').focus().val("");
				return false;
				}
			$.get("./index.aspx", _defs, function(_returned)
								{
								if(_returned == "SUCCESS")
									{
									location.href = location.href;
									}
								else	
									{
									please_wait("end");
									$(obj).removeAttr('disabled');
									alert(_returned);
									}
								});
			}
		function bv_pricing()
			{
			var obj			= $('#bv_part_number');
			if(obj.attr('data-checked') == 'false')
				{
				alert("The part supplied cannot be checked for validity");
				return false;
				}
			if(obj.attr('data-exists') == 'true')
				{
				var _master	= obj.attr('data-exists-id');
				if(confirm("This part is already referenced on part #"+_master+". would you like to go to that part?"))
					{
					location.href			= "./index.aspx?a=get&tab=G&id="+_master;
					}
				return false;
				}
			var _code		= obj.val();
			if(_code.length > 0)
				{
				var _html;
				$x = $.ajax({
					type:		"GET",
					url:		"index.aspx",
					data:		"a=xml_bv_pricing&code="+_code,
					beforeSend:	function()
									{
									obj.blur();
									$("#bv_pricing").empty().append("<img style='margin:60px;' src='/images/loading_panel.gif'>");
									},
					error:		function()
									{
									$("#bv_pricing").empty().append("<b style='color:#f00;display:block;margin:60px;'>There were no matches for the supplied part.</b>");
									},
					success:	function(xml)
									{
									$("#bv_pricing").empty();
									if($(xml).find('BV_PRICING').size() > 0)
										{
										_html				= "<table cellspacing='0' cellpadding='2' id='bv_pricing_table'>\
																	<thead id='bv_pricing_head'>\
																		<td class='cb'>&nbsp;</td>\
																		<th data-class='a' width='45'>BRANCH</th>\
																		<th data-class='b'>VENDOR&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>\
																		<th data-class='c' width='150'>VENDOR PART #</th>\
																		<th data-class='d' width='65'>Cost for this Vendor Part #</th>\
																		<th data-class='e' width='65'>Qty per this Vendor Part #</th>\
																		<th data-class='i' width='60' title='This is the price before the markup formula is applied'>SELL<br/><i>BASE PR.</i></th>\
																		<th data-class='g' width='60'>UOM<br/><i>SOLD AS</i></th>\
																		<td class='del'>&nbsp;</td>\
																	</thead>\
																	<tbody id='bv_pricing_body'>";
										$(xml).find('BV_PRICING').each(
											function()
												{
												var _unit_selected, _selected, _disabled, _rowstate = "";
												var _branch			= $(this).children('DSN').text();
												if($("#branch_dsn").val() != _branch)
													{
													_disabled		= "disabled";
													_rowstate		= "no";
													}
												else
													{
													_disabled		= "";
													_rowstate		= "yes";
													}
												var _vendor_num		= $.trim($(this).children('vend_no').text());
												var _description	= $(this).children('description').text();
												_description		= _description.replace(/\"/g, "&quot;");
												
												var _datetime		= $(this).children('datetime').text();
												var _date			= new Array();
												_date["y"]			= parseInt(_datetime.substring(0,4));
												_date["m"]			= parseInt(_datetime.substring(4,6)) - 1;
												_date["d"]			= parseInt(_datetime.substring(6,8));
												_date["h"]			= parseInt(_datetime.substring(8,10));
												_date["i"]			= parseInt(_datetime.substring(10,12));
												_date["s"]			= parseInt(_datetime.substring(12,14));
												var _dt				= new Date(_date["y"],_date["m"],_date["d"],_date["h"],_date["i"],_date["s"]);
												var _vendor_name	= $(this).children('vendor').text();
												var _our_code		= $(this).children('code').text();
												var _cost			= $(this).children('cost').text();
												_cost				= ((_cost/1).toPrecision(2) / 1).toFixed(2);
												var _divisor		= 1;
												var _sell_base		= parseFloat(_cost) / _divisor;
												_sell_base			= (_sell_base.toPrecision(2) / 1).toFixed(2);
												var _code			= "";
												if($(this).children('vend_code').text() == "")
													{
													_code			= _our_code;
													}
												else
													{
													_code			= $(this).children('vend_code').text();
													}
												if(_vendor_name != "" && _cost > 0)
													{
													_html		+=	"\
																		<tr class='"+_rowstate+"' title='"+_dt+"'>\
																			<td class='cb'><input title='include line' type='checkbox' "+_disabled+" checked/></td>\
																			<td class='a'>"+_branch+"</td>\
																			<td class='b' align='left' data-id='"+_vendor_num+"'>"+_vendor_name+"</td>\
																			<td class='c' width='150' title=\""+_description+"\">"+_code+"</td>\
																			<td class='d'><input class='costinput' onkeydown='refactor_pricing(this)' type='text' size='2' value='"+_cost+"' "+_disabled+"/></td>\
																			<td class='e'><input class='qtyperinput' onkeydown='refactor_pricing(this)' type='text' size='2' value='"+_divisor+"' "+_disabled+"/></td>\
																			<td class='i'><b class='sellbase'>$"+_sell_base+"</b></td>\
																			<td class='g'><b class='sellunit'>ea</b></td>\
																			<td class='del'><img src='/images/foundation/button/button[delrowdisabled].gif' "+_disabled+"/></td>\
																		</tr>";
													}
												});
										_html				+= "</tbody></table>";
										}
									else
										{
										_html				= "<div style='margin:60px;color:#c00;'><b>Nothing available in any branches for that part number</b></div>";
										}
									},
					complete:	function()
									{
									$("#bv_pricing").html(_html);
									$("#bv_pricing_table").fadeIn('slow');
									}
					});
				}
			else
				{
				alert("I can't use blank part numbers...");
				}
			}
		function attach_dropdowns()
			{
			$('.vendor').each(
							function(i)
								{
								var url					= './index.aspx?a=vendorquery';
								$(this).autocomplete(url,	{
															minChars:1,
															delay:0,
															autoFill:false,
															matchSubset: 1,
															matchContains:0,
															maxItemsToShow:100,
															cacheLength:100,
															width:350,
															formatResult:
																function(data, value)
																	{
																	return data[1];
																	},
															selectOnly:1
															});
								});
			}
		function sortNumber(a,b)
			{
			return a - b;
			}
		function PushPart(obj, redir)
			{
			var tag_id				= $("#TAG_ID").attr('data-id');
			var n_attributes		= $("#n_attributes").val();
			var valbox_pre			= "#valuebox_";
			var att_pre				= "attribute_";
			var val_pre				= "value_";
			var bv_code				= $("#bv_part_number").val();
			var av_id				= "";
			var bv_price_counter	= $('#bv_pricing table tbody').find('tr').size();
			var new_price_counter	= $('#new_pricing table tbody').find('tr').size();
			var payload				= {
											tag_id: tag_id,
											att_vals: [],
											prices: []
											};
			var post_data			=   {
                                        a: "newpart",
										payload: {}
											
			                            }
			var att_vals			= [];
			$("#att_vals").find("select").each(function()
													{
													payload.att_vals.push($(this).val());
													});
			var total_price_count			= 0;
			var i							= 0;
			// Iterate through new price rows, if they exist
			$('#new_pricing table tbody').find('tr').each(function()
															{
															var _vendor			= $(this).find('.vendor').attr('data-id');
															var _part			= $(this).find('.partnumber').val();
															var _costtotal		= $(this).find('.costinput').val();
															var _qty			= $(this).find('.qtyperinput').val();
															var _costprice		= 0;
															_costprice			= _costtotal / _qty;
															if(_vendor != "" && _part != "" && (_costtotal != "" && _costtotal != "0") && _qty != "")
																{
																var price_obj		=	{
																						VendorId:_vendor, 
																						VendorPartNo: _part, 
																						CostTotal: _costtotal, 
																						Quantity: _qty, 
																						CostPrice: _costprice
																						};
																payload.prices.push(price_obj);
																}
															});
			if(payload.prices.length == 0)
			    {
			    alert("You must have at least one valid vendor/price set before you can save this part.");
				return;
			    }
			post_data.payload		= JSON.stringify(payload);
			total_price_count		+= i;
			$.ajax(	{
					type:			"POST",
					url:			"./index.aspx",
					cache:			false,
					dataType:		"text",
					data:			post_data,
					beforeSend:		function()
										{
										$('#SAVE_BUTTON').attr('disabled', true);
										please_wait("begin");
										},
					error:			function (xhr, error)
										{
										document.write(xhr.responseText);
										},
					success:		function(resp)
										{
										if(resp.match(/\d+/g))
											{
											if(resp.match(/[a-z]+/g))
												{
												alert("Successfully saved part.\nAlthough there were errors saving the pricing:\n\n"+resp);
												}
											else
												{
												if(redir)
													{
													location.href		= "./index.aspx?a=get&tab=G&id="+resp;
													}
												else
													{
													please_wait("end");
													$('#picture_box_iframe').attr('src', './index.aspx?a=link_picture&id='+resp);
													$('#picture_box_print').click(function(){bc(resp);});
													alert("Successfully saved part #"+resp);
                           							$('#picture_box').dialog({width:700,height:500});
													}
												}
											}
										else
											{
											alert("Cannot save part. \nThere is already a part matching the supplied criteria. \nPlease refresh the page and retry.");
											}
										},
					complete:		function()
										{
										$('#SAVE_BUTTON').removeAttr('disabled');
										chk_part();
										$('#new_pricing table tbody').find('tr').remove();
										add_price_row();
                                        please_wait("end");
  
                                        $('#hidPartCreated').val(1);
										}
					});
			}
		function chk_reference(obj)
			{
			var code				= $(obj).val();
			if(code != "")
				{
				$.ajax(	{
						type:		"GET",
						url:		"index.aspx",
						data:		"a=xmlchk_reference&code="+code,
						error:		function()
										{
										alert("Error Checking Reference");
										},
						beforeSend:	function()
										{
										$(obj).next('button').attr('disabled', true);										
										},
						success:	function(xml)
										{
										$(xml).find('response').each(function()
											{
											var exists		= $(this).find('exists').text();
											var master_id	= $(this).find('master_id').text();
											if(exists == "True")
												{
												$(obj).attr('data-exists', 'true');
												$(obj).attr('data-checked', 'true');												
												$(obj).attr('data-exists-id', master_id);
												//$(obj).css({'background-color':'#0f0'});
												}
											else
												{
												$(obj).attr('data-exists', 'false');												
												$(obj).attr('data-checked', 'true');												
												//$(obj).css({'background-color':'#f00'});
												}
											});
										},
						complete:	function()
										{
										$(obj).next('button').attr('disabled', false);
										}
						});
				}
			else
				{
				$(obj).attr('data-checked', 'false');												
				return false;
				}
			}
		function new_tag()
			{
			var new_tag_target			= document.getElementById("new_tag");
			var new_tag					= escape(new_tag_target.value);
			if(new_tag != null)
				{
				location.href	= './index.aspx?a=newtag&tag='+new_tag;
				}
			else
				{
				return false;
				}
			}
		function delete_tag(tag_id)
			{
			if(tag_id != '')
				{
				if(confirm('Are you sure you to delete this tag?'))
					{
					location.href	= './index.aspx?a=deletetag&tag_id='+tag_id;
					}
				}
			}
		function delete_attribute(attribute_id)
			{
			if(attribute_id != '')
				{
				if(confirm('Are you sure you to delete this attribute?'))
					{
					location.href	= './index.aspx?a=delete_aid&attribute_id='+attribute_id;
					}
				}
			}
		function delete_value(attribute_value_id, attribute_id)
			{
			if(attribute_value_id != '')
				{
				if(confirm('Are you sure you to delete this value?'))
					{
					location.href	= './index.aspx?a=delete&attribute_value_id='+attribute_value_id+'&aid='+attribute_id;
					}
				}
			}
		function descriptors(which, order_id)
			{
			var target			= "#n_descriptors";
			var order_target	= "order_"+order_id;
			var current			= parseInt($(target).attr("value"));
			var decrement		= current - 1;
			var increment		= current + 1;
			var n_attributes	= parseInt($("#n_attributes").attr("value"));
			var max_attributes	= 0;
			if(n_attributes < 4)
				{
				max_attributes	= n_attributes;
				}
			else
				{
				max_attributes	= 4;
				}
			switch(which)
				{
				case 0:			$(target).attr("value",decrement );
								document.getElementById(order_target).selectedIndex = 0;
								$("#"+order_target).hide();
				break;
				case 1:			$(target).attr("value",increment );
								$("#"+order_target).show();
				break;
				}
			if($(target).attr("value") == max_attributes)
				{
				descriptor_state(1);
				}
			else
				{
				descriptor_state(0);
				}
			}
		function descriptor_state(state)
			{
			if(state == 1)
				{
				var which		= true;
				}
			else
				{
				var which		= false;
				}
			$(".descriptor").each(function()
				{
				if(!$(this).is(":checked"))
					{
					$(this).attr("disabled", which);
					}
				});
			}
		function descriptors_evaluate()
			{
			var n_attributes		= parseInt($("#n_attributes").attr("value"));
			if(n_attributes > 4)
				{
				n_attributes		= 4;
				}
			var i					= 0;
			$(".n_order").each(function()
				{
				if(!$(this).is(":hidden") && parseInt($(this).attr("value")) != 0)
					{
					i++;
					}
				})
			if(i == n_attributes)
				{
				return true;
				}
			else
				{
				return false;
				}
			}
		function edit_tag(tag_id, current_value)
			{
			var sub_target			= "div_"+tag_id;
			var target				= document.getElementById(sub_target);
			var newID				= 'div_'+tag_id+'_edit';

			var newHTML				= '<input id="'+newID+'" type="text" onkeypress="if(checkEnter(event)){submit_edited_tagname('+newID+', '+tag_id+');}" value="'+unescape(current_value)+'">';
			newHTML					+= '<button class="button" onclick="submit_edited_tagname('+newID+', '+tag_id+');"><img src="/images/icon/icon[send].gif"></button>';
			target.innerHTML		= newHTML;
			document.getElementById(newID).focus();
			document.getElementById(newID).select();
			return true;
			}
		function submit_edited_tagname(tag_div_id, tag_id)
			{
			var edited_tag_name				= escape(tag_div_id.value);
			if(edited_tag_name != "")
				{
				$.ajax(
					{
					type: "GET",
					url: 'index.aspx',
					data: 'a=edit_tag&tag_id='+tag_id+'&new_name='+edited_tag_name,
					success: function(msg)
						{
						switch(msg)
							{
							case "SUCCESS":	$("#qc"+tag_id).attr("title", "Has not been approved");
											$("#qc"+tag_id).attr('checked', false);
											$("#qc"+tag_id).parent().css({'background-color': '#fff'});
							break;
							default: alert("Error: "+msg);
							break;
							}
						document.getElementById("div_"+tag_id).innerHTML = "<div id='div_"+tag_id+"' onclick='edit_tag("+tag_id+",\""+edited_tag_name+"\");' style='cursor:pointer;width:100%;'>"+unescape(edited_tag_name)+"</div>";
						}
					});
				return true;
				}
			else
				{
				return false;
				}
			}
		function edit_value(attribute_value_id, row_id, aid, current_value)
			{
			var sub_target			= attribute_value_id;
			var target				= document.getElementById(sub_target);
			var newID				= attribute_value_id+'_edit';
			var newHTML				= '<input id="'+newID+'" type="text" onkeypress="if(checkEnter(event)){submit_new_value('+attribute_value_id+', '+attribute_value_id+'_edit, '+row_id+', '+aid+');}" value="'+current_value+'">';
			newHTML					+= '<button class="button" onclick="submit_new_value('+attribute_value_id+', '+attribute_value_id+'_edit, '+row_id+', '+aid+');"><img src="/images/icon/icon[send].gif"></button>';
			target.innerHTML		= newHTML;
			document.getElementById(newID).focus();
			document.getElementById(newID).select();
			return true;
			}
		function submit_new_value(original_attribute_value_id, new_attribute_value_id, row_id, aid)
			{
			var attribute_value_id			= original_attribute_value_id.id;
			var change_value_to				= new_attribute_value_id.value;
			change_value_to					= escape(change_value_to.replace(/[\+]/, "&#43;"));
			var on_this_row					= row_id;

			$.ajax(
				{
				type: "POST",
				url: 'index.aspx',
				data: 'action=editvalue&attribute_value_id='+row_id+'&new_value='+change_value_to+'&aid='+aid,
				success: function(msg)
					{
					document.getElementById("value_"+row_id).innerHTML = "<div onclick='edit_value(\"value_"+row_id+"\", "+row_id+", "+aid+", \""+change_value_to+"\");' width='100%' style='cursor:pointer;'>"+unescape(change_value_to)+"</div>";
					}
				});
			}
		function reset_attribute_row(row_id)
			{
			}
		function is_even(x)
			{
			return (x%2)?false:true;
			}
		function get_values()
			{
			var attribute_target			= document.getElementById("attribute_list");
			var attribute					= attribute_target.value;
			location.href="./index.aspx?a=att&aid="+attribute;
			}
		function populate_selects(i, aid)
			{
			i = i.replace(/attribute_/, "");
			populate_available(i, aid);
			populate_selected(i, aid);
			}
		function populate_attributes(att_id)
			{
			$("#"+att_id).find('option').remove().end()
			$("#"+att_id).append("<option value='0' SELECTED>SELECT ATTRIBUTE</option>");
			$.ajax(
						{
						type: "GET",
						url: "./index.aspx?a=xml_attributes",
						dataType: "xml",
						success:function(xml){
							$(xml).find('attribute').each(
								function(){
									var id							= $(this).attr("id");
									var name						= $(this).attr("name");
									$("#"+att_id).append("<option value='"+id+"'>"+name+"</option>");
									});
							}
						}
						);
			}
		function populate_default(i, aid)
			{
			var value_target							= "value_"+i;
			$("#"+value_target).find('option').remove().end();
			$("#"+value_target).append("<option value='0' SELECTED>SELECT VALUE</option>");
			$.ajax(
						{
						type: "GET",
						url: "./index.aspx?a=xml_values&aid="+aid,
						dataType: "xml",
						success:function(xml){
							$(xml).find('value').each(
								function(){
									var id							= $(this).attr("id");
									var name						= $(this).attr("name");
									$("#value_"+i).append("<option value='"+id+"'>"+name+"</option>");
									});
							}
						}
						);
			$("#"+value_target).removeAttr("disabled");
			}
		function populate_available(i,aid)
			{
			var available_target						= "available_"+i;
			$("#"+available_target).find('option').remove().end();
			var url										= "./index.aspx?a=available_xml_values&tag_id="+i+"&aid="+aid;
			$.ajax(
						{
						type: "GET",
						url: "./index.aspx?a=available_xml_values&tag_id="+i+"&aid="+aid,
						dataType: "xml",
						success:function(xml){
							$(xml).find('value').each(
								function(){
									var id							= $(this).attr("id");
									var name						= $(this).attr("name");
									$("#available_"+i).append("<option value='"+id+"'>"+name+"</option>");
									});
							}
						}
						);
			$("#selected_"+i).removeAttr("disabled");
			}
		function populate_selected(i,aid)
			{
			var selected_target							= "selected_"+i;
			$("#"+selected_target).find('option').remove().end();
			$.ajax(
						{
						type: "GET",
						url: "./index.aspx?a=selected_xml_values&tag_id="+i+"&aid="+aid,
						dataType: "xml",
						success:function(xml){
							$(xml).find('value').each(
								function(){
									var id							= $(this).attr("id");
									var name						= $(this).attr("name");
									$(selected_target).append("<option value='"+id+"'>"+name+"</option>");
									});
							}
						}
						);
			}
		function ToggleSelect(selectBox,selectAll)
			{
			// have we been passed an ID
			if (typeof selectBox == "string")
				{
				selectBox = document.getElementById(selectBox);
				}
			// is the select box a multiple select box?
			if (selectBox.type == "select-multiple")
				{
				var BoxLength					= selectBox.options.length;
				if(BoxLength > 1)
					{
					var selectedState				= selectBox.options[0].selected;
					for (var i = 0; i < BoxLength; i++)
						{
						switch(selectedState)
							{
							case true:				selectBox.options[i].selected	= false;
							break;
							case false:				selectBox.options[i].selected	= true;
							break;
							}
						}
					return true;
					}
				else
					{
					return false;
					}
				}
			}
		function array_has_duplicates(A)
			{
			var i, j, n;
			n=A.length;
			for (i=0; i<n; i++)
				{
				for (j=i+1; j<n; j++)
					{
					if (A[i] == A[j]) return true;
					}
				}
			return false;
			}
		function center_popup(popup_name)
			{
			//request data for centering
			var windowWidth			= document.documentElement.clientWidth;
			var windowHeight		= document.documentElement.clientHeight;
			var popupHeight			= $(popup_name).height();
			var popupWidth			= $(popup_name).width();
			//centering
			$(popup_name).css({
			"position": "absolute",
			"top": windowHeight/2-popupHeight/2,
			"left": windowWidth/2-popupWidth/2
			});
			//only need force for IE6

			$("#search_shade").css({
			"height": windowHeight
			});

			}
		function add_value(obj)
			{
			var _parent				= $(obj).parents('.attval:eq(0)');
			var _input				= _parent.find('.add_val');
			var _val				= _input.val();
			var _att				= _input.attr('data-attribute_id');
			var _valbox				= _parent.find('select');
			var _tag_id				= $('#TAG_ID').attr('data-id');
			if($.trim(_val) != "")
				{
				$.get('./index.aspx',
							{
							'a':		'add_value',
							'att':		_att,
							'val':		_val,
							'tag_id':	_tag_id
							},
							function(ret)
								{
								if(ret.match(/^([0-9]+)$/g))
									{
									_valbox.append("<option value='"+ret+"'>"+_val+"</option>").val(ret);
									chk_part();
                                    toggle_addval(obj);

                                    // When added a new value successfully, we need to set it to 0.
                                    $('#hidPartCreated').val(0);
									}
								else if(ret == "Rollback")
									{
									alert("There was a problem adding this value to the tag preset. \nPlease contact the inventory manager for further information.");
									}
								else if(ret == "Exists")
									{
									alert("This value already exists in the database and is either not linked to this tag, or has not been qc'ed. \nPlease contact the inventory manager for further information.");
									}
								else
									{
									alert("Couldn't save value");
									}
								});
				}
			}
		function toggle_addval(obj)
			{
			var _manufacturer		= 0;
			var _manu_exists		= false;
			var _this_att_id		= $(obj).parents('.attval:first').find('.add_val').attr('data-attribute_id');
			if(_this_att_id == "17")
				{
				$("body").find("input:hidden").each(function()
					{
					var this_id	= $(this).attr("id");
					if(this_id != undefined && this_id.match(/attributebox_/g) && $(this).val() == "16")
						{
						_manu_exists			= true;
						_manufacturer			= $(this).parents(".attval:first").find("select").val();
						if(_manufacturer == 0)
							{
							_alert		= true;
							$(obj).blur();
							}
						}
					});
				}
			if(_manu_exists && _manufacturer == 0 && _this_att_id == "17")
				{
				alert("Please choose a manufacturer first");
				}
			else
				{
				var _target				= $(obj).parents('.attval:first').find('.addval');
			
				if(_target.is(':hidden'))
					{
					_target.fadeIn(150, function()
											{
											$(this).children('input:text').val("").focus();
											});
					}
				else
					{
					_target.fadeOut(150, function()
											{
											$(this).children('input:text').val("").attr('data-isac', '');
											});
					}
				}
			}
		function build_att_val_boxes(tag_id, master_id)
			{
			$("#uniquestatus").html("");
			var pattern					= typeof(master_id) === "undefined" ? "" : master_id.toString();
			//console.log(pattern);
			if(pattern.match(/\,/g))
				{
			    pattern = pattern.split(",");
				}
			var i						= 0;
			var box_target				= "#att_vals";
			var branch					= $("#DSN").attr("value");
			var _error					= false;
			
			$.ajax(
				{
				type:			"GET",
				url:			"./index.aspx?a=xml_tag_att_val&tag_id="+tag_id,
				dataType:		"xml",
				beforeSend:
					function()
						{
						please_wait("begin");
						$('#bv_pricing').html("");
						$('#new_pricing_body').html("");
						$(box_target).empty();
						$("#this_references").css("display", "block");
						$("#new_part_info").hide();
						$('#bv_part_number').val("")
						$('.pricing').hide();
						$n_prices.val(0);
						add_price_row();
						},
				error:			
					function(xhr, ajaxOptions, thrownError)
									{
									alert("There was an error retrieving the attributes and values for this tag.\n Matt is working on this problem.\n----------\n"+xhr.responseText);
									_error 		= true;
									},
				success:		
					function(xml)
						{
						$('#n_attributes').attr('value', $(xml).find('attribute').size());
						$(xml).find('attribute').each(	
														function()
															{
															var id							= $(this).attr("id");
															var name						= $(this).attr("name");
															var html						= "";
															
															html							= "\
														<div class='attval'>\
															<div class='addval_box'>\
																<div class='addval' style='display:none;'>New Value<br/>\
																	<input type='text' class='add_val' onfocus=\"attach_ac(this, 'value', "+id+");\" data-attribute_id='"+id+"'/><button type='button' onclick='add_value(this)'>submit</button>\
																</div>\
																<img class='addval_button' onclick='toggle_addval(this)' title='Click to toggle add value drawer' src='/images/inventory/button/button[add].gif'/>\
															</div>"+
															"<div class='att'>"+name+"</div>";
															html							+= "<input type='hidden' id='attributebox_"+i+"' value='"+id+"'>"+
															"<div class='val'>"+
																"<select onchange='chk_part();part_match(this)' name='' id='valuebox_"+i+"'>"+
																	"<option value='0' selected>Choose Value</option>";
															$(this).find('value').each(
																					function()
																						{
																						var value_id				= $(this).attr("id");
																						var value_name				= $(this).attr("name");
																						var value_title				= $(this).attr("name").replace(/"/g, "&quot;");
																						var selected				= "";
																						if (pattern != "" && pattern.indexOf(value_id) > -1)
																							{
																							selected				= " SELECTED";
																							}
																						html						+= "\
																	<option value='"+value_id+"' title=\""+value_title+"\""+selected+">"+value_name+"</option>";
																						}
																					);
															html							+= "\
																</select>\
															</div>\
														</div>";
															$(box_target).append(html);
															i++;
															}
													);
						
						},
				complete:
					function()
						{
						$("#matched_parts").html("&nbsp;");
						if(typeof(master_id) !== "undefined" && !master_id.toString().match(/,/g))
							{
							part_match(null,master_id);
							}
						else
							{
							part_match();
							}
						if(_error == false)
							{
							$(".pricing").show();
							please_wait("end");
							chk_part();
							}
						}
				});
			}
		function chk_BranchPart(business_unit_id)
			{
			var box_length			= $("#PARTNUMBER_"+business_unit_id).attr("value").length;
			if(box_length == 0)
				{
				var history			= "#HISTORY_"+business_unit_id;
				$(history+" #VENDOR").html("");
				$(history+" #COST").html("");
				$(history+" #SOLD").html("");
				$(history+" #BOUGHT").html("");
				$(history+" #STATUS").attr("src", "/images/inventory/filler/filler[status].gif");
				return true;
				}
			}
		function sortNumber(a,b)
			{
			return a - b;
			}
		var part_is_unique			= false;
		function chk_part(master_id)
			{
			var url					= "./index.aspx?a=chk_uniqueness";
			if(master_id != null)
				{
				url					+= "&master_id="+master_id;
				}
			part_is_unique			= "";
			var Tag_ID				= $("#TAG_ID").attr("data-id");
			var n_attributes		= $("#n_attributes").attr("value");
			var val_array			= [];
			var uni_com;
			var blank_field			= false;
			var matched_part_id		= "";
			var is_active			= false;
			for(var i=0;i<n_attributes;i++)
				{
				var Valuebox		= "#valuebox_"+i;
				var Value			= $(Valuebox).attr("value");
				if(Value == 0)
					{
					blank_field		= true;
					}
				val_array[i]		= Value;
				}
			var TagPattern			= val_array.sort(sortNumber);
			var TagPattern			= TagPattern.toString();
			if(TagPattern.charAt(TagPattern.length-1) == ",")
				{
				TagPattern			= TagPattern.toString().substr(0, TagPattern.length-1);
				}
			var handler				= function(){PushPart(this, false);}
			if(!blank_field)
				{
				$.ajax({
						url:		url+"&tag="+Tag_ID+"&tag_pattern="+TagPattern,
						type:		'GET',
						dataType:	'xml',
						error:		function(errortext)
										{
										alert(errortext.response);
										},
						beforeSend:	function()
										{
										$("#uniquestatus").html("<div align='center'><img src='/images/loading_panel.gif'></div>");
										},
						success:	function(xml)
										{
										//document.write(url+"&tag="+Tag_ID+"&tag_pattern="+TagPattern);
										$(xml).find('response').each(function()
																		{
																		part_is_unique					= ($(this).find('unique').text().toLowerCase() === 'true');
																		matched_part_id					= $(this).find('master_id').text();
																		is_active						= ($(this).find('active').text().toLowerCase() === 'true');
																		});
										},
						complete:	function()
										{
										if(part_is_unique)
											{
											$("#uniquestatus").html("<b style='color:#0a0;padding:20px;display:block;'><img src='/images/icon/icon[approve].gif' align='absmiddle' /> Part is unique</b>");
											$("#SAVE_BUTTON").css({"cursor":"pointer"}).removeAttr("disabled").unbind("click").bind("click", handler);
											}
										else
											{
											if(is_active)
												{
												$("#uniquestatus").html("<b style='color:#c00;padding:20px;display:block;'><img src='/images/icon/icon[deny].gif' align='absmiddle' />Part is not unique, the part you have tried to create is <a style='color:#f00;' title='Click to view part' href='./index.aspx?a=get&tab=G&id="+matched_part_id+"'>part #"+matched_part_id+"</a></b>");
												}
											else
												{
												$("#uniquestatus").html("<b style='color:#c00;padding:20px;display:block;'><img src='/images/icon/icon[deny].gif' align='absmiddle' />Part is not unique, the part you have tried to create is part #"+matched_part_id+", unfortunately it is not an active part. Please talk with the inventory administrator.</b>");
												}
											$("#SAVE_BUTTON").attr("disabled", true).css({"cursor":""}).unbind("click");
											}
										return part_is_unique;
										}
						});
				}
			else
				{
				part_is_unique				= false;
				$("#SAVE_BUTTON").attr("disabled", true).css({"cursor":""}).unbind("click");
				$("#uniquestatus").html("");
				return false;
				}
			}
		var bv_part_okay				= false; 
		var bv_part						= ""; 
		function clear_me(obj)
			{
			obj.value	= "";
			}
		function _StringFormatInline()
			{
				var txt = this;
				for(var i=0;i<arguments.length;i++)
				{
					var exp = new RegExp('\\{' + (i) + '\\}','gm');
					txt = txt.replace(exp,arguments[i]);
				}
				return txt;
			}
		function _StringFormatStatic()
			{
				for(var i=1;i<arguments.length;i++)
				{
					var exp = new RegExp('\\{' + (i-1) + '\\}','gm');
					arguments[0] = arguments[0].replace(exp,arguments[i]);
				}
				return arguments[0];
			}
		if(!String.prototype.format)
			{
			String.prototype.format = _StringFormatInline;
			}
		if(!String.format)
			{
			String.format = _StringFormatStatic;
			}
		function ShowNewValue(button, div_id)
			{
			var parent_x					= button.offsetLeft;
			var parent_y					= button.offsetTop;
			var child_target				= "#newvaluebox_"+div_id;
			var child_textbox_target		= "#newvalue_"+div_id;
			$(child_target).css("left", (parent_x+240));
			var current_state				= $(child_target).css('display');
			switch(current_state)
				{
				case "block":				$("#this_references").fadeTo(250, 1);
											$("#add_reference_button").fadeTo(250, 1);
											$("#this_references").find('img').each(function (){$(this).attr('disabled', false)});
											$("#this_references").find('select').each(function (){$(this).attr('disabled', false)});
											$("#this_references").find('input').each(function (){$(this).attr('disabled', false)});
											$("#add_reference_button").attr('disabled', false);

											$(child_target).fadeOut(250);
				break;
				case "none":				
				default:					$("#this_references").fadeTo(250, .25);
											$("#add_reference_button").fadeTo(250, .25);
											$("#this_references").find('img').each(function (){$(this).attr('disabled', true)});
											$("#this_references").find('select').each(function (){$(this).attr('disabled', true)});
											$("#this_references").find('input').each(function (){$(this).attr('disabled', true)});
											$("#add_reference_button").attr('disabled', true);
											$(child_target).fadeIn(250, function(){$(child_textbox_target).focus()});
				break;	
				}
			}
		function PushNewValue(div_id)
			{
			var child_target				= "#newvaluebox_"+div_id;
			var child_dropdown_target		= "#valuebox_"+div_id;
			var child_textbox_target		= "#newvalue_"+div_id;
			var newValue					= $(child_textbox_target).attr("value");
			$(child_dropdown_target).append("<option selected>"+newValue+"</option>");
			$(child_textbox_target).attr("value", "")
			$(child_target).fadeOut(250);
			$("#this_references").fadeTo(250, 1);
			$("#add_reference_button").fadeTo(250, 1);
			$("#this_references").find('img').each(function (){$(this).attr('disabled', false)});
			$("#this_references").find('select').each(function (){$(this).attr('disabled', false)});
			$("#this_references").find('input').each(function (){$(this).attr('disabled', false)});
			$("#add_reference_button").attr('disabled', false);
			}
		function NewReference()
			{
			var html				= "<div>";
			html					+= "<table cellspacing='0' cellpadding='0' class='reference_each'>";
			html					+= "<tr>";
			html					+= "<td colspan='4'><img style='cursor:pointer;' onclick='$(this).parent().parent().parent().parent().fadeOut(250, function(){$(this).remove()});' src='/images/inventory/button/button[removereference].png' /></td>";
			html					+= "</tr>";
			html					+= "<tr>";
			html					+= "<td rowspan='2' width='7'>";
			html					+= "<img src='/images/inventory/filler/filler[reference-left].png' height='100%'/>";
			html					+= "</td>";
			html					+= "<td bgcolor='white'>";
			html					+= "<select><option>Choose Type<option>Vendor<option>Branch</select>";
			html					+= "</td>";
			html					+= "<td bgcolor='white'>";
			html					+= "<select disabled></select>";
			html					+= "</td>";
			html					+= "<td rowspan='2' width='7'><img src='/images/inventory/filler/filler[reference-right].png' height='100%'/></td>";
			html					+= "</tr>";
			html					+= "<tr><td colspan='2' bgcolor='white'>their part number<br/><input type='text' onclick='$(this).datepicker();'/></td></tr></table></div>"
			$("#this_references").append(html);
			//alert($("#this_references").find('div').length);
			}
		function fill_values(obj)
			{
			if(typeof(obj) == "object")
				{
				var _pattern			= $(obj).attr('data-pattern');
				var _pattern_array		= _pattern.split(",");
				for(i = 0;i<_pattern_array.length;i++)
					{
					$('#valuebox_'+i).val(_pattern_array[i]);
					}
				$(obj).blur();
				$('#valuebox_0').focus();
				chk_part();
				part_match();
				}
			else
				{
				var _pattern_array		= obj.split(",");
				for(i = 0;i<_pattern_array.length;i++)
					{
					$('#valuebox_'+i).val(_pattern_array[i]);
					}
				$('#valuebox_0').focus();
				chk_part();
				part_match();
				}
			}
		function part_match(obj, _master_id)
			{
			var max_atts				= $('#n_attributes').attr('value');
			var tag_id					= $('#TAG_ID').attr('data-id');
			var tag_pattern				= "";
			var att_vals				= [];
			for(var i=0;i<max_atts;i++)
				{
				var val_id				= $("#valuebox_"+i).val();
				if(val_id > 0)
					{
					att_vals[i]			= val_id;
					}
				}
			var TagPattern			= att_vals.sort(sortNumber);
			var TagPattern			= TagPattern.toString();
			if(TagPattern.charAt(TagPattern.length-1) == ",")
				{
				TagPattern			= TagPattern.toString().substr(0, TagPattern.length-1);
				}
			var url					= "./index.aspx?a=xml_match_parts&tag_id="+tag_id+"&attvals="+TagPattern;
			var html				= "";
			var use_this			= "";
			$x = $.ajax({	type:			"GET",
						cached:			false,
						url:			url,
						dataType:		"xml",
						beforeSend:		function()
											{
											$("#matched_parts").html("<div align='center' width='100%'><img src='/images/loading_panel.gif' vspace='10'/></div>");

                                            // When picking up another option, we will set it to 0.
                                            $('#hidPartCreated').val(0);
											},
						error:			function()
											{
											$("#matched_parts").empty();
											},
						success:		function(xml)
											{
											var size_of				= $(xml).find('part').size();
											if(size_of > 0)
												{
												html	= "<br/><table width='100%' cellpadding='2' cellspacing='0'>\
															<thead>\
																<tr>\
																	<th></th>\
																	<th>Part #</th>\
																	<th>Vendor Code</th>\
																	<th>Description</th>\
																	<th>&nbsp;</th>\
																	<th>&nbsp;</th>\
																</tr>\
															</thead>\
															<tbody>";
												$(xml).find('part').each(
													function()
														{
														var master_id		= $(this).attr('master_id');
														var description		= $(this).attr('description');
														var _pattern		= $(this).attr('attval');
														var _vendor_code = $(this).attr('vendor_code')
														//console.log("Master IDS: "+_master_id + " " + master_id);
														if(_master_id != undefined && _master_id == master_id)
															{
														    use_this = _pattern;
														    //console.log("Using this:"+use_this);
															}
														html				+= "<tr><td><button type='button' onclick='bc("+master_id+")'><img src='/images/icon/icon[print_barcode].GIF'/></button></td><td style='border-right:solid 1px #ccc;width:50px;'><b>"+master_id+
																					"</b></td><td style='border-right:solid 1px #ccc;' align='center'>"+_vendor_code+
																					"</td><td align='left'>"+description+
																					"</td><td width='75'><button type='button' onclick=\"location.href='./index.aspx?a=get&tab=G&id="+master_id+
																					"'\" style='width:75px;font-family:arial;font-size:11px;'>Goto Part</button></td><td width='75'><button type='button' style='width:75px;font-family:arial;font-size:11px;' data-pattern='"+_pattern+
																					"' onclick='fill_values(this);'>Fill Values</button></td></tr>";
														});
												html		+= "</tbody></table>";
												}
											else
												{
												html		= "<div style='padding:10px;' align='center' width='100%'>Nothing matches this criteria</div>";
												}
											},
						complete:		function()
											{
											if(html.length != undefined && html.length > 0)
												{
												$("#matched_parts").html(html);
												}
											else
												{
												$("#matched_parts").html("&nbsp");
												}
											if(use_this != "")
												{
												fill_values(use_this);
												//console.log("Used this:" + use_this);
                                            }
											}
						});
			}
		function view_part(master_id, number)
			{
			$("#part_info .part_number").html("");
			$("#part_info #part_attvals").html("");
			$("#part_detail").css({display:"block",opacity:0}).animate( {opacity:0.75, backgroundColor:'#000'}, 500);
			$("#part_info").css({display:"block",opacity:0}).animate( {opacity:1}, 500);

			var url					= "?a=xml_part_att_val&master_id="+master_id;
			var html;

			$.ajax({	type:			"GET",
						cached:			false,
						url:			url,
						dataType:		"xml",
						success:		function(xml)
											{
											html	= "<table width='100%' cellpadding='5' cellspacing='0' class='attvals'>";
											$(xml).find('attvals').each(
												function()
													{
													var size_of				= $(this).find('attribute').size();
													if(size_of > 0)
														{
														$(this).find('attribute').each(
															function()
																{
																var attribute		= $(this).attr('name');
																var value			= $(this).attr('value');
																html				+= "<tr><td class='attribute'>"+attribute+":</td><td class='value'>"+value+"</td></tr>";});
														}
													else
														{
														html		+= "<tr class='nothing'><td>Nothing matches this criteria</td></tr>";
														}
													});
											html		+= "</table>";
											$("#part_attvals").append(html);
											}
						});
			$("#part_info .part_number").html(number);
			center_popup("#part_info");
			}
		function close_part()
			{
			$("#part_info").hide();
			$("#part_detail").animate( {opacity:0}).css({display:"none"});
			}
		function populate_part_locations()
			{
			var query_string		=	{
										a:			"xml_part_locations",
										id:			$('#MASTER_ID').val()
										};

			$.ajax({		type:			"GET",
							cached:			false,
							url:			"./index.aspx",
							data:			query_string,
							dataType:		"xml",
							beforeSend:		function()
												{
												please_wait("start", "Fetching Locations...");
												$("#stock_detail tbody").html("");
												},
							success:		function(xml)
												{
												var rows		= "";
												var options		= "";
												$(xml).find('location').each(
													function()
														{
														var v	=	{
																	id:						$(this).find("id").text(),
																	location_master_id:		$(this).find("location_master_id").text(),
																	name:					$(this).find("name").text(),
																	qty:					$(this).find("qty").text(),
																	min:					$(this).find("min").text(),
																	max:					$(this).find("max").text()
																	};
														var l		= "";
														var t_id	= 0;
														for(var i = 0;i<location_list.length;i++)
															{
															var l_info		= location_list[i];
															var selected	= l_info.location_master_id == v.location_master_id ? " selected" : "";
															t_id			= l_info.location_master_id == v.location_master_id ? l_info.type_id : t_id;
															l				+= "<option value='"+l_info.location_master_id+"'"+selected+">"+l_info.name+"</option>";
															}
														var t	= t_id == 1 ? "Internal" : "External";
														
														var r	= "<tr data-id='"+v.id+"'>\
																	<td align='center'><select class='name' onchange='location_switch(this);'>"+l+"</select></td>\
																	<td align='center'><input type='text' class='qty' data-original_value=\""+v.qty+"\" onkeydown='only_numeric(event)' value='"+v.qty+"' /><span title='Drag me to another quantity' class='xfer'>&nbsp;</span></td>\
																	<td align='center'><input type='text' class='min' onkeydown='only_numeric(event)' value='"+v.min+"' /></td>\
																	<td align='center'><input type='text' class='max' onkeydown='only_numeric(event)' value='"+v.max+"' /></td>\
																	<td align='center'><b class='type'>"+t+"</b></td>\
																	<td align='center'><button type='button' onclick='save_location(this, false);'><img src='/images/icon/icon[save].gif' align='absmiddle' /></button><button type='button' onclick='delete_location(this, false);'><img src='/images/icon/icon[delete].gif' align='absmiddle' /></button></td>\
																	</tr>";
														var o	= "<option value='"+v.id+"'>"+v.name+"</option>";
														rows	+= r;
														options	+= o;
														});
												$("#stock_detail tbody").html(rows);
												$("#stock_detail thead").find(".name").focus();
												$("#location_from").html(options);
												$("#location_to").html(options);
												},
							complete:		function()
												{
												please_wait("end");
												$("#stock_detail tbody").find(".qty").each(function()
													{
													this.onselectstart = function () { return false; };
													$(this).parent().draggable({ revert: true, handle: '.xfer', stack: 'td', axis: 'y', revertDuration: 0 });
													$(this).parent().droppable(
														{
														over: function(event, ui)
															{
															$(this).css({"background-color" : "#fc0"});
															},
														out: function(event, ui)
															{
															$(this).css({"background-color" : ""});
															},
														drop: function(event, ui)
															{
															$(this).css({"background-color" : "#cfc"});
															var to_row_id		= $(this).parents('tr:first').attr("data-id");
															var from_row_id		= $(ui.draggable).parents('tr:first').attr("data-id");
															var from_max_qty	= $(ui.draggable).parents('tr:first').find(".qty").attr("data-original_value");
															$("#location_from").val(from_row_id);
															$("#location_to").val(to_row_id);
															$("#transfer_window").dialog("open");
															$("#transfer_qty").focus();
															$("#transfer_maxqty").text(from_max_qty);
															}
														});
													});
												//$("#stock_detail").tablesorter();
												},
							error:		function()
												{
												//alert("There was an error loading locations");
												}
												});
			}


		var location_list			= [];
		function populate_locations(to_var)
			{
			if(to_var == undefined)
				{
				to_var				= false;
				}
			var query_string		=	{
										a:			"xml_locations"
										};

			var rows		= to_var ? {} : "";
			$.ajax({		type:			"GET",
							cached:			false,
							url:			"./index.aspx",
							data:			query_string,
							dataType:		"xml",
							beforeSend:		function()
												{
												if(!to_var)
													{
													please_wait("start", "Fetching Locations...");
													$("#location_master tbody").html("");
													}
												},
							success:		function(xml)
												{
												$(xml).find('location').each(
													function()
														{
														var v	=	{
																	id:			$(this).find("id").text(),
																	name:		$(this).find("name").text(),
																	type_id:	$(this).find("type_id").text(),
																	c:			$(this).find("c").text()
																	};
														if(!to_var)
															{
															var t	= v.type_id == "1" 
																		? "<option value='1' selected>Internal</option><option value='2'>External</option>" 
																		: "<option value='1'>Internal</option><option value='2' selected>External</option>";
															var r	= "<tr data-id='"+v.id+"' data-children='"+v.c+"'>\
																		<td align='center' class='cid'>"+v.id+"</td>\
																		<td align='center' class='cname'><input type='text' class='name' value=\""+v.name+"\" /></td>\
																		<td align='center' class='ctype'><select class='type'>"+t+"</select></td>\
																		<td align='center' class='caction'><button type='button' onclick='save_location(this, true);'><img src='/images/icon/icon[save].gif' align='absmiddle' /></button><button type='button' onclick='delete_location(this, true);'><img src='/images/icon/icon[delete].gif' align='absmiddle' /></button></td>\
																		</tr>";
															rows	+= r;
															}
														else
															{
															rows	=	{
																		location_master_id: v.id,
																		name:				v.name,
																		type_id:			v.type_id
																		}
															location_list.push(rows);
															}
														});
												},
							complete:		function()
												{
												if(!to_var)
													{
													please_wait("end");
													$("#location_master tbody").html(rows);
													}
												else
													{
													var l		= "";
													var t		= "";
													for(var i = 0;i<location_list.length;i++)
														{
														var l_info		= location_list[i];
														var selected	= i == 0 ? "selected" : "";
														if(i == 0)
															{
															t			= l_info.type_id == 1 ? "Internal" : "External";
															}
														l				+= "<option value='"+l_info.location_master_id+"'>"+l_info.name+"</option>";
														}
													$("#stock_detail .add_line .name").html(l);
													$("#stock_detail .add_line .type").text(t);
													populate_part_locations();
													}
												},
							error:		function()
												{
												//alert("There was an error loading locations");
												}
												});
			}
		function location_switch(obj)
			{
			var _parent		= $(obj).parents('tr:first');
			var id			= $(obj).val();
			var t			= "";
			for(var i = 0;i<location_list.length;i++)
				{
				var l_info		= location_list[i];
				if(id == l_info.location_master_id)
					{
					t	= l_info.type_id == 1 ? "Internal" : "External";
					}
				}
			_parent.find(".type").text(t);
			}
		function bind_xfer(s,e)
			{
			try{$('#ctl00_cphMasterBody_gv_partlocations_Title_add_name')[0].selectedIndex = -1;}catch(e){}
			$(".stock_detail .tr").find(".qty").each(function()
				{
				var can_drag		= $(this).attr("data-draggable");
				if(can_drag == "yes")
					{
					this.onselectstart = function () { return false; };
					$(this).parent().draggable({ revert: true, handle: '.xfer', stack: 'td', axis: 'y', revertDuration: 0 });
					$(this).parent().droppable(
						{
						over: function(event, ui)
							{
							$(this).css({"background-color" : "#fc0"});
							},
						out: function(event, ui)
							{
							$(this).css({"background-color" : ""});
							},
						drop: function(event, ui)
							{
							$(this).css({"background-color" : "#cfc"});
							var to_row_id		= $(this).parents('tr:first').find(".qty").attr("data-id");
							var from_row_id		= $(ui.draggable).parents('.tr:first').find(".qty").attr("data-id");
							var from_max_qty	= $(ui.draggable).parents('.tr:first').find(".qty").attr("data-original_qty");
							$("#location_from").val(from_row_id);
							$("#location_to").val(to_row_id);
							$("#transfer_window").dialog("open");
							$("#transfer_qty").focus();
							$("#transfer_maxqty").text(from_max_qty);
							}
						});
					}
				});
			}
var ne_inv_branch =
		{
		reset_all_minmax:
			function(s,e)
				{
				if(confirm("Are you sure you wish to reset the MIN/MAX Levels for ALL parts, and ALL locations in your branch?"))
					{
					var vars		=
						{
						a:					"reset_minmax_levels"
						};
					$.ajax(	{
							type:			"GET",
							url:			"./index.aspx",
							dataType:		"text",
							cache:			false,
							data:			vars,
							beforeSend:
								function()
									{
									please_wait('start', "Resetting Min/Max Levels");
									},
							success:
								function(res)
									{
									if(res != "SUCCESS")
										{
										alert("There was an error resetting the min/max levels - "+res);
										}
									else
										{
										alert("Successfully reset all min/max levels");
										}
									},
							complete:
								function()
									{
									please_wait('stop');
									},
							error:
								function()
									{
									please_wait('stop');
									}
							});
					}
				}
		}

		function save_location(obj, is_master)
			{
			var _parent				= $(obj).parents('.tr:first');
			var _id					= "";
			if(is_master)
				{
				_id					= _parent.find(".name").attr("data-id") != undefined ? _parent.find(".name").attr("data-id") : "";
				}
			else
				{
				_id					= _parent.find(".qty").attr("data-id") != undefined ? _parent.find(".qty").attr("data-id") : "";
				}
			var vars;
			if(!is_master)
				{
				vars				=	{
										a:					"save_location",
										id:					_id,
										master_id:			$('#MASTER_ID').val(),
										location_master_id:	_parent.find(".name").val(),
										qty:				_parent.find(".qty").find("input:text").val() / 1,
										original_qty:		_parent.find(".qty").find("input:text").attr("data-original_qty") / 1,
										min:				_parent.find(".min").val() / 1,			
										max:				_parent.find(".max").val() / 1
										};
				}
			else
				{
				vars				=	{
										a:					"save_master_location",
										id:					_id,
										name:				_parent.find(".name").val(),
										type_id:			_parent.find(".type").val()
										};
				}
			var valid			= true;
			if(!is_master)
				{
				if($.trim(vars.qty) == "")
					{
					valid			= false;
					}
				if($.trim(vars.min) == "")
					{
					valid			= false;
					}
				if($.trim(vars.max) == "")
					{
					valid			= false;
					}
				}
			if(valid)
				{
				if(!is_master && vars.min > vars.max && vars.min != 0 && vars.max != 0)
					{
					alert("Your MAX value is less than your MIN value. Not saving.");
					}
				else if(!is_master && isNaN(vars.qty))
					{
					alert("Your QTY value is not a valid number. Not saving.");
					}
				else if(!is_master && vars.qty < 0 && vars.original_qty >= 0)
					{
					alert("Your QTY value is negative. Not saving.");
					}
				else if(!is_master && isNaN(vars.min))
					{
					alert("Your MIN value is not a valid number. Not saving.");
					}
				else if(is_master && $.trim(vars.name) == '')
					{
					alert("The name supplied is invalid");
					}
				else if(!is_master && isNaN(vars.max))
					{
					alert("Your MAX value is not a valid number. Not saving.");
					}
				else if(!is_master && vars.min < 0)
					{
					alert("Your MIN value can't be less than zero. Not saving.");
					}
				else if(!is_master && vars.max < 0)
					{
					alert("Your MAX value can't be less than zero. Not saving.");
					}
				else
					{
					//alert(vars.a+"\n"+vars.id+"\n"+vars.location_master_id+"\n"+vars.qty+"\n"+vars.min+"\n"+vars.max);
					var is_error		= false;
					$.ajax(	{
							type:			"GET",
							url:			"./index.aspx",
							dataType:		"html",
							cache:			false,
							data:			vars,
							beforeSend:		function()
												{
												please_wait('start');
												},
							success:		function(resp)
												{
												if(resp != 'SUCCESS')
													{
													alert(resp);
													is_error			= true;
													}
												},
							error:			function (xhr, error)
												{
												alert("There was an error saving the location\n"+xhr.responseText);
												is_error			= true;
												},
							complete:		function()
												{
												please_wait('stop');
												if(is_master)
													{
													if(!is_error)
														{
														gv_master_locations.Refresh();
														}
													//populate_locations();
													}
												else
													{
													populate_part_locations();
													gv_partlocations.Refresh();
													}
												if(!is_master && vars.id == "")
													{
													$('#ctl00_cphMasterBody_gv_partlocations_Title_add_name')[0].selectedIndex = -1;
													_parent.find(".qty").find("input:text").val('');
													_parent.find(".min").val('');	
													_parent.find(".max").val('');
													}
												else if(is_master && !is_error && vars.id == "")
													{
													_parent.find(".name").val("");
													}
												}
							});
					}
				}
			else
				{
				alert("There are blank fields, not saving");
				}
			}
		function delete_location(obj, is_master)
			{
			var _parent				= $(obj).parents('.tr:first');
			var _id					= "";
			if(is_master)
				{
				_id					= _parent.find(".name").attr("data-id") != undefined ? _parent.find(".name").attr("data-id") : "";
				}
			else
				{
				_id					= _parent.find(".qty").attr("data-id") != undefined ? _parent.find(".qty").attr("data-id") : "";
				}
			var vars;
			var qty					= 0;
			if(is_master)
				{
				vars				=	{
										a:				"delete_location_master",
										id:				_id
										};
				}
			else
				{
				vars				=	{
										a:				"delete_location",
										master_id:		$('#MASTER_ID').val(),
										id:				_id
										};
				qty					= _parent.find(".qty").attr("data-original_qty") / 1;
				if(qty != 0)
					{
					alert("You can't delete a location that has a quantity assigned to it.");
					return false;
					}
				}
			var conf			= is_master ? "Are you sure you want to delete this location?" : "Are you sure you want to delete this location for this part?";
			if(confirm(conf))
				{
				 $.ajax(	{
							type:			"GET",
							url:			"./index.aspx",
							dataType:		"html",
							cache:			false,
							data:			vars,
							beforeSend:		function()
												{
												$(obj).attr("disabled", "disabled");
												please_wait('begin');
												},
							success:		function(resp)
												{
												if(resp == "SUCCESS" && !is_master)
													{
													location.href = location.href;
													//gv_partlocations.Refresh();
													//_parent.fadeOut();
													}
												else if(resp == "SUCCESS" && is_master)
													{
													gv_master_locations.Refresh();
													}
												else
													{
													alert(resp);
													}
												},
							complete:		function()
												{
												please_wait('end');
												},
							error:			function (xhr, error)
												{
												alert("There was an error deleting this location");
												please_wait('end');
												$(obj).removeAttr("disabled");
												}
							});
				}
			else
				{
				return false;
				}
			}
	function location_xfer(obj)
		{
		var v			=	{
							a:				"location_qty_xfer",
							to_id:			$("#location_to").val(),
							from_id:		$("#location_from").val(),
							qty:			$("#transfer_qty").val(),
							max:			$("#transfer_maxqty").text()
							};
		if(v.to_id == v.from_id)
			{
			alert("You cannot transfer from and to the same location");
			return false;
			}
		if(v.qty == "" || v.qty == 0)
			{
			alert("Cannot submit a blank quantity, or a quantity of zero.");
			$("#transfer_qty").focus();
			return false;
			}
		if(v.qty / 1 < 0)
			{
			alert("Cannot submit a negative quantity");
			$("#transfer_qty").focus();
			return false;
			}
		if(v.qty / 1 > v.max / 1)
			{
			alert("Cannot transfer a quantity greater than the max quantity.");
			$("#transfer_qty").val(v.max).focus();
			return false;
			}
		if(isNaN(v.qty))
			{
			alert("Your QTY is not a valid number");
			$("#transfer_qty").val(v.max).focus();
			return false;
			}

		$.ajax(	{
				type:			"GET",
				url:			"./index.aspx",
				dataType:		"text",
				cache:			false,
				data:			v,
				beforeSend:		function()
									{
									$(obj).attr("disabled", true);
									},
				success:		function(resp)
									{
									if(resp == "SUCCESS")
										{
										populate_part_locations();
										gv_partlocations.Refresh();
										$("#transfer_qty").val("");
										$("#transfer_window").dialog("close");
										}
									else
										{
										alert(resp);
										}
									},
				complete:		function()
									{
									$(obj).removeAttr("disabled");
									},
				error:			function (xhr, error)
									{
									alert("There was an error transferring stock")
									}
				});
		}