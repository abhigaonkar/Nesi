
		var attribute_box;
		var value_box;

		function create_tag(obj)
			{
			var _tag					= $(obj).val();
			if(_tag != "")
				{
				location.href	= './index.aspx?a=newtag&tag='+_tag;
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
		function clear_vendor_pricing()
			{
			$('#references').find('.branch_box').each(function()
														{
														$(this).find('.vendor_pricing').empty().hide();
														$(this).find('.part_number').val("");
														$(this).val("");
														});
			}



		var part_is_unique;
		function chk_uniqueness(master_id)
			{
			var url					= "./index.aspx?a=chk_uniqueness";
			if(master_id != null)
				{
				url					+= "&master_id="+master_id;
				}
			part_is_unique			= "";
			var Tag_ID				= $("#TAG_ID").attr("value");
			var n_attributes		= $("#n_attributes").attr("value");
			var val_array			= [];
			var uni_com;
			var blank_field			= false;
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
			if(!blank_field)
				{
				$.ajax({
						url:		url+"&tag="+Tag_ID+"&tag_pattern="+TagPattern,
						success:	function(data)
										{
										switch(data)
											{
											case "UNIQUE":		part_is_unique		= true;
											break;
											case "COMMON":		
											default:			part_is_unique		= false;
											break;
											}
										},
						async:		false});
				}
			else
				{
				part_is_unique		= false;
				}
			}
		function get_vendor_pricing(obj, get_all)
			{
			if(get_all != undefined && get_all == true)
				{
				$('#references').find('.branch_box').each(function()
															{
															$(this).find('.part_number').val($(obj).prev('input:text').val());
															var _parent				= $(this);
															var CODE				= $(obj).prev('input:text').val();
															var DSN					= _parent.attr('data-DSN');
															var _html				= "<table cellpadding='2' cellspacing='0' class='prices'>\
																						<thead>\
																							<tr>\
																								<th width='5%'>&nbsp;</th>\
																								<th width='10%'>Vendor #</th>\
																								<th width='60%'>Vendor Name</th>\
																								<th width='10%'>Cost Price</th>\
																								<th width='15%'>Vendor Part No</th>\
																							</tr>\
																						</thead>\
																						<tbody>";
															if(CODE != "")
																{
																$.ajax(
																		{	
																		type:				"GET",
																		url:				"./index.aspx?a=xml_bv_special_pricing&CODE="+CODE+"&DSN="+DSN,
																		dataType:			"xml",
																		beforeSend:			function()
																								{
																								_parent.find('.vendor_pricing').show().html("<div align='center'><img src='/images/loading_panel.gif' /></div>");
																								},
																		success:			function(xml)
																								{
																								if($(xml).find('vendor_pricing').size() > 0)
																									{
																									$(xml).find('vendor_pricing').each(function()
																																			{
																																			var vendor_code		= $(this).find('vendor_code').text();
																																			var vendor_name		= $(this).find('vendor_name').text();
																																			var cost_price		= $(this).find('cost_price').text();
																																			var vendor_part_no	= $(this).find('vendor_part_no').text();
																																			_html				+= "\
																																								<tr class='vendor_row'>\
																																									<td><input type='checkbox' checked /></td>\
																																									<td class='vendor'>"+vendor_code+"</td>\
																																									<td>"+vendor_name+"</td>\
																																									<td class='cost'>"+cost_price+"</td>\
																																									<td class='part_no'>"+vendor_part_no+"</td>\
																																								</tr>";
																																			});
																									}
																								else
																									{
																									_html	= "Vendor pricing not available";
																									}
																								},
																		complete:			function()
																								{
																								_parent.find('.vendor_pricing').empty().html(_html);
																								}
																		});
																}
															else
																{
																return false;
																}
															});
				}
			else
				{
				var _parent				= $(obj).parents('.branch_box:first');
				var CODE				= $(obj).prevAll('input:text:first').val();
				var DSN					= _parent.attr('data-DSN');
				var _html				= "<table cellpadding='2' cellspacing='0' class='prices'>\
											<thead>\
												<tr>\
													<th width='5%'>&nbsp;</th>\
													<th width='10%'>Vendor #</th>\
													<th width='60%'>Vendor Name</th>\
													<th width='10%'>Cost Price</th>\
													<th width='15%'>Vendor Part No</th>\
												</tr>\
											</thead>\
											<tbody>";
				if(CODE != "")
					{
					$.ajax(
							{	
							type:				"GET",
							url:				"./index.aspx?a=xml_bv_special_pricing&CODE="+CODE+"&DSN="+DSN,
							dataType:			"xml",
							beforeSend:			function()
													{
													please_wait('begin');
													},
							success:			function(xml)
													{
													if($(xml).find('vendor_pricing').size() > 0)
														{
														$(xml).find('vendor_pricing').each(function()
																								{
																								var vendor_code		= $(this).find('vendor_code').text();
																								var vendor_name		= $(this).find('vendor_name').text();
																								var cost_price		= $(this).find('cost_price').text();
																								var vendor_part_no	= $(this).find('vendor_part_no').text();
																								_html				+= "\
																													<tr class='vendor_row'>\
																														<td><input type='checkbox' checked /></td>\
																														<td class='vendor'>"+vendor_code+"</td>\
																														<td>"+vendor_name+"</td>\
																														<td class='cost'>"+cost_price+"</td>\
																														<td class='part_no'>"+vendor_part_no+"</td>\
																													</tr>";
																								});
														}
													else
														{
														_html	= "No vendor pricing available";
														}
													},
							complete:			function()
													{
													_parent.find('.vendor_pricing').show().empty().html(_html);
													please_wait('end');
													}
							});
					}
				else
					{
					return false;
					}
				}
			}
		function markup_handler(obj)
			{
			if($(obj).is(":checked"))
				{
				$("#t_fixed_markup").removeAttr("disabled");
				}
			else
				{
				$("#t_fixed_markup").attr("disabled", true);
				}
			}
		function save_markup(obj)
			{
			please_wait("start", "Saving...");
			var amount			= $("#t_fixed_markup").val() / 1;
			var is_checked		= $("#c_fixed_markup").is(":checked");
			if(amount < 0 && is_checked)
				{
				alert("Markup cannot be less zero");
				please_wait("stop");
				return;
				}
			$.get("index.aspx",
				{
				a:				"save_markup",
				tag_id:			$("#tag_id").val(),
				is_fixed:		is_checked,
				markup_amount:	amount
				},
				function(resp)
					{
					if(resp != "SUCCESS")
						{
						alert("There was an issue saving the fixed markup for this tag.\n Server Responded:\n"+resp);
						}
					else
						{
						handle_properties_toggle();
						}
					please_wait("stop");
					alert("Saved");
					});
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
		function populate_tradenames()
			{
			var _defs			=	{
									a:				"xml_tag_tradename",
									tag_id:			$("#tag_id").val()
									}
			var _body			= "";
			$.ajax(
					{	
					type:				"GET",
					url:				"./index.aspx",
					data:				_defs,
					dataType:			"xml",
					beforeSend:			
						function()
							{
							if($("#progress_1234567890").length == 0)
								{
								please_wait("start");
								}
							$('#trade_names tbody').html("");
							},
					success:			
						function(xml)
							{
							if($(xml).find('tradename').size() > 0)
								{
								$(xml).find('tradename').each(function()
									{
									var id					= $(this).find('id').text();
									var tradename			= $(this).find('name').text();
									_body					+= "\
<tr>\
	<td><input type='text' class='tradename' onkeydown=\"if(checkEnter()){$(this).parents('tr:first').find('button:first').click();}\"  value=\""+tradename+"\" style='width:99%'/></td>\
	<td><button type='button' style='width:99%' onclick='save_tradename(this)' data-id='"+id+"'><img src='/images/icon/icon[save].gif' /></td>\
	<td><button type='button' style='width:99%' onclick='delete_tradename(this)' data-id='"+id+"'><img src='/images/icon/icon[delete].gif' /></td>\
</tr>";
									});
								}
							else
								{
								var _error		= $(xml).find('error').text();
								_body			= "<tr><td colspan='2' align='center'>"+_error+"</td></tr>";
								}
							},
					complete:
						function()
							{
							$('#trade_names tbody').html(_body);
							please_wait("end");
							},
					error:
						function()
							{
							$('#trade_names tbody').html("<tr><td colspan='2'>There was an issue retrieving available tradenames</td></tr>");
							}
					});
			}
		function save_tradename(obj)
			{
			var _parent				= $(obj).parents('tr:first');
			var _defs				=	{
										a:			"save_tradename",
										id:			$(obj).attr('data-id'),
										tag_id:		$("#tag_id").val(),
										tradename:	_parent.find('.tradename').val()
										};
			if(_defs.tradename == "")
				{
				alert("Please supply a tradename");
				_parent.find('.tradename').focus();
				return;
				}
			else
				{
				please_wait("start");
				}
			$.get("./index.aspx", _defs, function(_returned)
				{
				if(_returned == "SUCCESS")
					{
					if(_defs.id == "")
						{
						_parent.find('.tradename').val("")
						}
					populate_tradenames();
					}
				else
					{
					alert(_returned);
					please_wait("end");
					}
				});
			}
		function delete_tradename(obj)
			{
			var _defs				=	{
										a:			"delete_tradename",
										id:			$(obj).attr('data-id')
										};
			if(confirm("Are you sure you want to delete this tradename?"))
				{
				please_wait("start");
				}
			else
				{
				return;
				}
				
			$.get("./index.aspx", _defs, function(_returned)
				{
				if(_returned == "SUCCESS")
					{
					populate_tradenames();
					}
				else
					{
					alert(_returned);
					please_wait("end");
					}
				});
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
		function unapprove_value(attribute_value_id, attribute_id)
			{
			if(attribute_value_id != '')
				{
				if(confirm('Are you sure you to send this back to QC?'))
					{
					location.href	= './index.aspx?a=unapprove_value&attribute_value_id='+attribute_value_id+'&aid='+attribute_id;
					}
				}
			}

/*
		DESCRIPTOR / "ORDER OF" LOGIC
*/

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

		function send_value(attribute_id)
			{
			var new_value				= $("#new_value").val();
			new_value					= new_value;
			if(new_value != null)
				{
				location.href			= './index.aspx?a=add_value&value='+new_value+'&aid='+attribute_id;
				return true;
				}
			else
				{
				return false;
				}
			}

		function descriptor_order_chk(selector)
			{
			var this_value			= $(selector).attr("value");
			var order_ids			= new Array();
			$(".n_order").each(function()
				{
				if(!$(this).is(":hidden") && $(this).attr("value") == this_value && $(this).attr("id") != $(selector).attr("id"))
					{
					alert("This order option has already been chosen");
					selector.selectedIndex = 0;
					}
				})
			tag_att_val_chk();
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

		function bind_xfer(s,e)
			{
			$(".splitparts").find("table").each(function()
				{
				$(this).css({"overflow":"auto"});
				});
			$(".splitparts .tr").find(".vendor").each(function()
				{
				this.onselectstart = function () { return false; };
				$(this).draggable({ revert: true, opacity:0.85, handle: '.move', stack: 'td', axis: 'y', revertDuration: 0 });
				$(this).droppable(
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
						split_move(this, ui.draggable);
						$(this).css({"background-color" : ""});
						/*
						var to_row_id		= $(this).parents('tr:first').find(".qty").attr("data-id");
						var from_row_id		= $(ui.draggable).parents('.tr:first').find(".qty").attr("data-id");
						var from_max_qty	= $(ui.draggable).parents('.tr:first').find(".qty").attr("data-original_qty");
						$("#location_from").val(from_row_id);
						$("#location_to").val(to_row_id);
						$("#transfer_window").dialog("open");
						$("#transfer_qty").focus();
						$("#transfer_maxqty").text(from_max_qty);
						*/
						}
					});

				});
			}
		function split_save(obj)
			{
			var _parent			= $(obj).parents(".tr:first");
			var _move			= _parent.find(".move");
			var r				=	{
									a:					"save_split",
									ss_vendor_code:		_parent.find(".vendor_code input:text").val(),
									ss_qty_per:			_parent.find(".qty input:text").val(),
									ss_cost:			_parent.find(".cost input:text").val(),
									ss_row_id:			_move.attr("data-row_id"),
									ss_tag_id:			combo_selecttag.GetValue(),
									ss_business_unit_id:		_move.attr("data-business_unit_id"),
									ss_vendor_id:		_move.attr("data-vendor_id"),
									ss_master_id:		_move.attr("data-master_id"),
									ss_link:			""
									};
			_parent.find(".attribute").each(function()
				{
				r.ss_link		+= $(this).val()+",";
				});
			r.ss_link			= r.ss_link.substring(0, r.ss_link.length-1);
			
			$.ajax(
					{
					type:				"GET",
					url:				"./index.aspx",
					async:				false,
					data:				r,
					dataType:			"text",
					success:			function(resp)
											{
											if(resp != "SUCCESS")
												{
												alert(resp);
												}
											},
					beforeSend:			function()
											{
											please_wait("start", "Saving...");
											},
					complete:			function()
											{
											please_wait("stop");
											gv_splitparts.Refresh();
											},
					error:				function(XMLHttpRequest, textStatus, errorThrown)
											{
											alert("Received "+errorThrown+"\nFull Error: "+XMLHttpRequest.responseText);
											}
					});
			}
		function split_move(to, from)
			{
			var elem_from		= $(from).find(".move");
			var elem_to			= $(to).find(".move");
			var f				=	{
									row_id:		elem_from.attr("data-row_id"),
									master_id:	elem_from.attr("data-master_id"),
									vendor_id:	elem_from.attr("data-vendor_id"),
									business_unit_id:	elem_from.attr("data-business_unit_id")
									};
			var t				=	{
									row_id:		elem_to.attr("data-row_id"),
									master_id:	elem_to.attr("data-master_id"),
									vendor_id:	elem_to.attr("data-vendor_id"),
									business_unit_id:	elem_to.attr("data-business_unit_id")
									};
			var r	=	{
									a:					"save_move",
									f_row_id:			f.row_id,
									f_master_id:		f.master_id,
									f_vendor_id:		f.vendor_id,
									f_business_unit_id:		f.business_unit_id,

									t_row_id:			t.row_id,
									t_master_id:		t.master_id,
									t_vendor_id:		t.vendor_id,
									t_business_unit_id:		t.business_unit_id
									};
			var _refresh			= false;
			$.ajax(
					{
					type:				"GET",
					url:				"./index.aspx",
					async:				false,
					data:				r,
					dataType:			"text",
					beforeSend:			function()
											{
											please_wait("start", "Saving...");
											},
					success:			function(resp)
											{
											if(resp != "SUCCESS")
												{
												alert(resp);
												}
											else
												{
												_refresh		= true;
												}
											},
					error:				function(XMLHttpRequest, textStatus, errorThrown)
											{
											alert("Received "+errorThrown+"\nFull Error: "+XMLHttpRequest.responseText);
											},
					complete:			function()
											{
											please_wait("stop");
											if(_refresh)
												{
												gv_splitparts.Refresh();
												}
											}
					});
			}
		function company_select(selected_business_unit_id)
			{
			var company_select			= "<select onchange='refactor_pricing(this);'><option value='0'>SELECT COMPANY</option>";
			var company_countries		= "";
			$.ajax(
					{	
					type:				"GET",
					url:				"./index.aspx?a=xml_company",
					async:				false,
					dataType:			"xml",
					success:			function(xml)
											{
											$(xml).find('company').each(
												function()
													{
													var business_unit_id			= $(this).attr("id");
													var name		= $(this).attr("name");
													var is_canadian			= $(this).attr("is_canadian");
													switch(is_canadian)
														{
														case "true":
														case "True":		is_canadian		= "1";
														break;
														case "false":
														case "False":		is_canadian		= "0";
														break;
														}
													var selectedtext		= "";
													if(business_unit_id == selected_business_unit_id)
														{
														selectedtext		= "selected";
														}
													else
														{
														selectedtext		= "";
														}
													company_countries		+= "<input type='hidden' id='is_canadian"+business_unit_id+"' value='"+is_canadian+"'/>";
													company_select			+= "<option value='"+business_unit_id+"' "+selectedtext+">"+name.toUpperCase()+"</option>";
													}); // end each
											}
					});
			company_select				+= "</select>"+company_countries;
			return company_select;
			}			

		function clear_me(entity)
			{
			entity.value	= "";
			}	
			


		function load_unqcedpics()
			{
			var _html				= "<table cellpadding='2' cellspacing='0' width='100%' class='pics'>";
			$.ajax({
					type:			"GET",
					url:			"./index.aspx?a=xml_pics_not_qced",
					dataType:		"xml",
					error:			function(XMLHttpRequest, textStatus, errorThrown)
										{
										alert(errorThrown);
										},
					beforeSend:		function()
										{
										$("#unqcedpics").html("<img src='/images/loading_panel.gif' style='margin:100px;' />");
										},
					success:		function(xml)
										{
										var _windowwidth			= $("#unqcedpics").width();
										var _numberofimages			= $(xml).find('pic').size();
										var _columns				= Math.floor(_windowwidth/204);
										//alert("c"+_columns);
										var _rows					= Math.ceil(_numberofimages / _columns);
										//alert("r"+_rows);
										var i						= 0;
										if(_numberofimages > 0)
											{
											for(var r = 0;r < _rows;r++)
												{
												//alert("Working on row #"+r);
												_html					+= "<tr>";
												for(var c=0;c < _columns;c++)
													{
													//alert("Working on col #"+c);
													if(i < _numberofimages)
														{
														var _img			= new Array();
														var id				= $(xml).find('pic:eq('+i+')').children("id").text();
														var by_who			= $(xml).find('pic:eq('+i+')').children("by_who").text();
														var tag_id			= $(xml).find('pic:eq('+i+')').children("tag_id").text();
														var tag				= $(xml).find('pic:eq('+i+')').children("tag").text();
														var master_id		= $(xml).find('pic:eq('+i+')').children("master_id").text();
														var description		= $(xml).find('pic:eq('+i+')').children("description").text();
														description			= description;
														description			= "<li>"+description.replace(/\|/g, "<li>");
														var insert_dt		= $(xml).find('pic:eq('+i+')').children("insert_dt").text();
														var name	= $(xml).find('pic:eq('+i+')').children("name").text();
														var x 				= $(xml).find('pic:eq('+i+')').children("x").text();
														var y 				= $(xml).find('pic:eq('+i+')').children("y").text();
														_html				+= "<td class='photo_slot' align='center'>\
																					<a href='/_tools/inventory_picture/index.aspx?id="+id+"&is_master=false'><img class='img' src='/_tools/inventory_picture/index.aspx?id="+id+"&is_master=false' width='200' height='150' /></a>\
																					<div class='info'>\
																						<center><button type='button' onclick='qc_pic("+id+", 2, "+master_id+")' style='font-size:11px;'><b>DELETE</b></button></center>\
																						<table cellpadding='0' cellspacing='0'>\
																							<tr><td class='c'>TAG:</td><td class='v'>"+tag+"</td></tr>\
																							<tr><td class='c'>PART #:</td>\<td class='v'>"+master_id+"</td></tr>\
																							<tr><td class='c' colspan='2'><u>DESCRIPTION</u></td></tr>\
																							<tr><td class='v' colspan='2' valign='top'><ul>"+description+"</ul></td></tr>\
																							<tr><td class='c'>WHO:</td><td class='v'>"+by_who+"</td></tr>\
																							<tr><td class='c'>BRANCH:</td><td class='v'>"+name+"</td></tr>\
																							<tr><td class='c'>WHEN:</td><td class='v'>"+insert_dt+"</td></tr>\
																						</table>\
																						<center><button type='button' onclick='qc_pic("+id+", 0, "+master_id+")' class='deny'><img src='/images/icon/icon[deny].gif' /></button> <button type='button' onclick='qc_pic("+id+", 1, "+master_id+")' class='approve'><img src='/images/icon/icon[approve].gif' /></button></center>\
																					</div>\
																				</td>";
														//alert(_html.length);
														description			= null;
														}
													else
														{
														_html				+= "<td>&nbsp;</td>";
														}
													i++;
													} // end columns
												_html					+= "</tr>";
												} // end rows
											_html						+= "</table>";
											}
										else
											{
											_html						+= "<div align='center' style='padding:100px;'>There are currently no pics to QC<br/><br/><button onclick='load_unqcedpics();'>refresh page</button></div>";
											}
										},
					complete:		function()
										{
										$("#unqcedpics").html(_html);
										}
					});
			}

		function qc_pic(pic_id, which, master_id)
			{
			$.get("./index.aspx",
					{
					a:			"handle_pic",
					id:			pic_id,
					which:		which,
					master_id:	master_id
					},
					function(returned)
						{
						if(returned == "SUCCESS")
							{
							load_unqcedpics();
							}
						else
							{
							alert("I'm sorry I was unable to properly handle your request.");
							}
						});
			}

		function price_save(obj, type)
			{
			$(obj).attr('disabled', true);
			var tr						= $(obj).parents('tr:eq(0)');
			var business_unit_id				= "";
			var action;
			var price_id				= "";
			if(type == 1)
				{
				business_unit_id					= tr.find('.company').find('select').val();
				action						= "new_price"
				}
			else
				{
				business_unit_id					= tr.find('.business_unit_id').find('input:hidden').val();
				action						= "edit_price";
				if(tr.attr('data-price-id') != "")
					{
					price_id				= tr.attr('data-price-id');
					}
				}
			var vendor_id				= tr.find('.vendor').find('input:text').attr('data-id');
			var part_number				= tr.find('.part_number').find('input').val();
			var total_cost				= tr.find('.total_cost').find('input').val();
			var qty						= tr.find('.qty').find('input:text').val();
			var benchmark				= type == 1 ? false : tr.find('.benchmark').find('input:checkbox').is(":checked");
			var unit_cost				= (total_cost / 1) / (qty / 1);
			var master_id				= $('#master_id').val();
			var props					=	{
											a:					action,
											price_id:			price_id,
											master_id:			master_id,
											business_unit_id:			business_unit_id,
											vendor_id:			$.trim(vendor_id),
											part_number:		$.trim(part_number),
											qty:				qty,
											total_cost:			total_cost,
											unit_cost:			unit_cost,
											benchmark:			benchmark
											};
			if(vendor_id != "" && total_cost != "" && qty != "" && unit_cost != "" && business_unit_id != "0")
				{
				$.get("./index.aspx", 
					props, 
					function(data)
						{
						if(data == "Success")
							{
							get_pricing();
							}
						else
							{
							alert(data);
							}
						$(obj).removeAttr('disabled');
						});
				}
			else
				{
				alert("You cannot submit an incomplete vendor row");
				$(obj).removeAttr('disabled');
				}
			}
		function validate_benchmark(obj)
			{
			var current_state		= $(obj).is(":checked");
			var this_row_class		= $(obj).parents("tr:first").attr('class');
			if(current_state)
				{
				$("#existing_pricing").find('tr.'+this_row_class).each(
					function()
						{
						$(this).find("input:checkbox").each(
							function()
								{
								$(this).removeAttr("checked");
								});
						});
				$(obj).attr("checked", true);
				}
			}

		var $n_prices			= $('#n_prices');
		
		function get_pricing()
			{
				var master_id			= $('#master_id').val();
				$.ajax(
							{
							type:		"GET",
							url:		"./index.aspx?a=xml_prices&master_id="+master_id,
							dataType:	"xml",
							beforeSend:	function()
											{
											please_wait('begin');
											$('#existing_pricing').empty();
											//$('.add_price .company').html(company_selector);
											$('.add_price .part_number input:text').val('');
											$('.add_price .total_cost input:text').val('');
											$('.add_price .qty input:text').val('1');
											$('.add_price .vendor input:text').val('');
											$('.add_price .unit_cost').text('0.00000');
											},
							success:	function(xml)
											{
											var to_append							= "<center>\
											<table cellspacing='0' cellpadding='2' class='price_sheet'>\
												<thead>\
													<tr class='h'>\
														<th width='50'>BRANCH</th>\
														<th width='25'>BENCH</td>\
														<th width='125'>LAST EDITED</td>\
														<th>VENDOR</th>\
														<th width='150'>VEN. PART #</th>\
														<th width='75'>TOTAL COST</th>\
														<th width='65'>QTY</th>\
														<th width='100'>UNIT COST</th>\
														<th width='50' colspan='2'>&nbsp;</th>\
													</tr>\
												</thead>\
												<tbody>";
											$(xml).find('price').each(
												function()
													{
													var price_id					= $(this).find("id").text();
													var business_unit_id					= $(this).find("business_unit_id").text();
													var is_canadian					= $(this).find("is_canadian").text();
													var name				= $(this).find("name").text();
													var edited_dt					= $(this).find("edited_dt").text();
													var benchmark					= $(this).find("benchmark").text();
													var benchmark_checked			= benchmark == "1" ? " checked='checked'" : "";
													name					= name.replace("NE ", "");
													var name_length			= name.length;
													if(name_length > 16)
														{
														name				= name.substring(0, 16)+'..';
														}
													var total						= $(this).find("total").text();
													var cost						= $(this).find("cost").text();
													var qty							= $(this).find("qty").text();
													var _suffix						= "";
													var vendor_id					= $(this).find("vendor_id").text();
													var vendor_number				= $(this).find("vendor_number").text();
													var vendor_name					= $(this).find("vendor_name").text();
													var reference					= $(this).find("reference").text();
													to_append						+= "\
	<tr data-price-id='"+price_id+"' class='"+name.substring(0,4)+business_unit_id+"'>\
		<td class='company'><div style='width:70px;overflow-x:hidden;white-space:nowrap;' title=\""+name+"\">"+name+"<input type='hidden' class='business_unit_id' value='"+business_unit_id+"'/><input type='hidden' class='is_canadian' value='"+is_canadian+"'/></div></td>\
		<td class='benchmark'><input type='checkbox' onclick='validate_benchmark(this)' "+benchmark_checked+"/></td>\
		<td class='last_edit' style='font-size:11px;' align='center'>"+edited_dt+"</td>\
		<td class='vendor'><input type='text' onfocus=\"attach_ac(this, 'vendor')\" data-isac='false' data-business_unit_id='"+business_unit_id+"' data-master_id='"+master_id+"' value=\"("+vendor_number+") "+vendor_name+"\" data-id=\""+vendor_id+"\" /></td>\
		<td class='part_number'><input type='text' style='text-align:center;' value='"+reference+"'></td>\
		<td class='total_cost' width='75'><input type='text' style='text-align:center;' value='"+total+"' onkeydown='refactor_pricing(this)' onkeyup='refactor_pricing(this)'></td>\
		<td width='65' class='qty'><input type='text' value='"+qty+"' style='text-align:center;' onkeydown='refactor_pricing(this)' onkeyup='refactor_pricing(this)'></td>\
		<td class='unit_cost' align='center'>"+cost+_suffix+"</td>\
		<td width='25'><button type='button' onclick='price_save(this, 0);'><img src='/images/icon/icon[save].gif' align='absmiddle'></button></td>\
		<td width='25'><button type='button' onclick='delete_price(this);'><img src='/images/icon/icon[delete].gif' align='absmiddle'></button></td>\
	</tr>";
													});
											to_append						+= "</tbody></table></center>";
											$('#existing_pricing').html(to_append);
											},
							error:		function()
											{
											$('#existing_pricing').html("<div>pricing not set</div>");
											},
							complete:	function()
											{
											please_wait('end');
											$(document).ready(function()
																{
																$('.price_sheet').tablesorter({
																							headers: {
																										3:{sorter:"currency"},
																										5:{sorter:false}
																										}
																							});
																});
											//refresh_sellprices();
											}		
							}
						   );
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
								//refresh_sellprices();
								}
							else
								{
								alert(returned);
								}
							});
		    }


		function refresh_sellprices()
			{
			var master_id			= $('#master_id').val();
			var _body				= "";
			$.ajax(	{
					type:		"GET",
					url:		"./index.aspx?a=xml_sellprices&master_id="+master_id,
					dataType:	"xml",
					beforeSend:	function()
									{
									$('#sell_price_table tbody').empty();
									$('#sell_price_table tbody').html("<tr><td colspan='10' align='center'><img src='/images/loading_panel.gif' /></td></tr>");
									},
					success:	function(xml)
									{
									$('#sell_price_table tbody').empty();
									$(xml).find('price').each(
										function()
											{
											var business_unit_id		= $(this).attr('business_unit_id');
											var name	= $(this).attr('name').replace("NE ", "");
											var sell_string		= $(this).attr('sell_string');
											var sell_double		= $(this).attr('sell_double');
											var in_bv			= $(this).attr('in_bv');
											var bv_code			= $(this).attr('linked_code');
											var bv_sell			= $(this).attr('linked_sell');
											var bv_cost			= $(this).attr('linked_cost');
											if(in_bv == "N/A")
												{
												in_bv			= "<i style='color:#f00'>"+in_bv+"</i>";
												}
											else
												{
												in_bv			= "<b>"+in_bv+"</b";
												}
											if(bv_sell == "$0.00")
												{
												bv_sell			= "<i style='color:#f00'>N/A</i>";
												}
											if(bv_cost == "$0.00")
												{
												bv_cost			= "<i style='color:#f00'>N/A</i>";
												}
											_body				+= "\
	<tr data-business_unit_id='"+business_unit_id+"'>\
		<td><b>"+name+"</b></td>\
		<td width='150' align='center'><input value='"+sell_double+"' size='5'><button onclick='edit_sell(this)' type='button'><img src='/images/icon/icon[save].gif'/></button></td>\
		<td align='center'>"+in_bv+"</td>\
		<td align='center'>"+bv_code+"</td>\
		<td align='center'>"+bv_sell+"</td>\
		<td align='center'>"+bv_cost+"</td>\
	</tr>";
											});
									},
					error:		function()
									{
									},
					complete:	function()
									{
									$('#sell_price_table tbody').html(_body);
									}
					});
			}



		function refactor_pricing(obj)
			{
			var _parent			= $(obj).parents('tr:first');
			if(_parent.find('.qty input').val()	== ".")
				{
				_parent.find('.qty input').val("0.");
				return true;
				}
			var	_is_canadian	= _parent.find('.is_canadian').val();
			var	_qty			= parseFloat(_parent.find('.qty input').val().replace(",", ""));
			var _total_cost		= parseFloat(_parent.find('.total_cost input').val().replace(",", ""));
			var _unit_cost		= 0;
			var _tartotal_cost	= _parent.find('.total_cost input');
			var _tarunit_cost	= _parent.find('.unit_cost');
			var _tarqty			= _parent.find('.qty input');
			_unit_cost			= (_total_cost / _qty);
			if(_unit_cost == Infinity)
				{
				_unit_cost	= 0;
				}
			if(isNaN(_unit_cost))
				{
				_tarqty.val("1");
				_tarunit_cost.text("0.00000");
				}
			else
				{	
				_tarunit_cost.text(_unit_cost.toFixed(5));
				}
			return true;
			}

		function delete_price(obj)
			{
			$(obj).parents('tr:first').children().css({'background-color':'#f00', 'color':'#fff', 'text-decoration':'line-through'});
			var price_id			= $(obj).parents('tr:first').attr('data-price-id');
			if(confirm("Confirm you want this price line deleted"))
				{
				$.get("./index.aspx", 
						{
						a:			"delete_price",
						price_id:	price_id
						}, 
						function(returned)
							{
							if(returned == "Success")
								{
								get_pricing();
								}
							else
								{
								alert("There was an error deleting this price");
								}
							return true;
							});
				}
			else
				{
				$(obj).parents('tr:first').children().css({'background-color':'#fff', 'color':'#000', 'text-decoration':''});
				return false;
				}
			}


		function edit_attribute(aid, obj)
			{
			var current_name			= $(obj).text();
			if(current_name.match(/"/g))
				{
				current_name			= current_name.replace(/\"/g, "&quot;");
				}
			var is_edittable			= $("#attribute_is_edittable");
			if(is_edittable.val() == 'yes' && aid != "" && current_name != "")
				{
				is_edittable.val('no');
				var newHTML				= '<input id="attribute_edit" type="text" style="min-height:32px;" onkeypress="if(checkEnter(event)){submit_new_attribute('+aid+');}" value="'+current_name.trim()+'" />';
				$("#attribute_name").html(newHTML);
				$("#attribute_edit").focus();
				$("#attribute_edit").select();
				return true;
				}
			else
				{
				return false;
				}
			}


		function submit_new_attribute(aid)
			{
			if(aid != "")
				{
				var new_name					= $("#attribute_edit").val().toString().trim();
				if(new_name != "")
					{
					var url							= './index.aspx?a=editatt&aid='+aid+'&new_name='+escape(new_name);
					location.href					= url;
					}	
				}
			}


		function approve(id, type_of, where_am_i)
			{
			var url			= "./index.aspx?a=approve&id="+id+"&type_of="+type_of;
			$.ajax(	{
					type:	"GET",
					url:	url,
					cache:	false,
					success: 
						function(msg)
							{
							switch(msg)
								{
								case "SUCCESS":
									if(type_of == "tag")
										{
										switch(where_am_i)
											{
											case "edit_linkage": 
												$("#approve"+id).attr("disabled", "true");
												history.go(-1);
											break;
											case "tag":	
												$("#qc"+id).attr("disabled", "true");
												$("#qc"+id).parent().css({'background-color': '#cfc'});
												$("#qc"+id).parent().attr("title", "Approved By: You");
											break;
											}
										}
									else
										{
										$("#qc"+id).parent().parent().remove();
										}
								break;
								case "FAILED":
									alert("Could not approve "+type_of);
								break;
								}
							 }
					});
			}


		function change_value(improper_value_id, proper_value_id, obj)
			{
			var url			= "./index.aspx?a=change_value&improper_value_id="+improper_value_id+"&proper_value_id="+proper_value_id;
			$.ajax({
			   type: "GET",
			   url: url,
			   success: 
				function(msg)
					{
					switch(msg)
						{
						case "SUCCESS":		$(obj).parents('tr:first').remove();
						break;
						case "FAILED":		alert("Could switch the values");
						break;
						}
					 }
			 });
			}



		function edit_tag(tag_id, obj)
			{
			if($(obj).attr('data-editable') == "true" || $(obj).attr('data-editable') == null)
				{
				$(obj).attr("data-editable", "false");
				var _value			= $(obj).children('div').text();
				if(_value.match(/"/g))
					{
					_value				= _value.replace(/\"/g, "&quot;");
					}
				var _html				= '<input type="text" onkeypress="if(checkEnter(event)){submit_edited_tagname(this, '+tag_id+');}" value="'+_value+'" />';
				//_html					+= '<button type="button" class="button" onclick="submit_edited_tagname(this, '+tag_id+');"><img src="/images/icon/icon[send].gif"></button>';
				$(obj).html(_html);
				$(obj).children('input').focus();
				$(obj).children('input').select();
				return true;
				}
			else
				{
				return false;
				}
			}

		

		function submit_edited_tagname(obj, tag_id)
			{
			var edited_tag_name				= $(obj).val();
			var _parent						= $(obj).parent();
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
											_parent.html("<div style='cursor:pointer;width:100%;'>"+edited_tag_name+"</div>");
											_parent.attr("data-editable", "true");
							break;
							default: alert("Error: "+msg);
							break;
							}
						}
					});
				return true;
				}
			else
				{
				return false;
				}
			}


		function edit_value(obj)
			{
			var _parent				= $(obj).parent();
			var _value				= $(obj).html();
			if(_value.match(/"/g))
				{
				_value				= _value.replace(/\"/g, "&quot;");
				}
			var newHTML				= "<input type='text' class='new_value'  value=\""+_value+"\" />";
			_parent.html(newHTML);
			$('.new_value').keyup(function(e)
									{
									if(e.which == 13)
										{
										submit_new_value(this);
										return true;
										}
									else
										{
										return false;
										}
									});
			_parent.children('input:text').focus();
			_parent.children('input:text').select();
			return true;
			}


		function submit_new_value(obj)
			{
			var target;
			var _parent			= $(obj).parent();
			switch(obj.nodeName)
				{
				case "INPUT":		target = $(obj);
				break
				case "BUTTON":		target = _parent.children('input:text');
				}
			$.post("index.aspx",
						{
						action:					"editvalue",
						attribute_value_id:		_parent.attr('data-row_id'),
						aid:					_parent.attr('data-aid'),
						new_value:				target.val()
						},
						function(v)
							{
							if(v == "SUCCESS")
								{
								_parent.html("<div onclick='edit_value(this);' width='100%' style='cursor:pointer;'>"+target.val()+"</div>");
								}
							else
								{
								alert("There was an issue submitted this value.");
								}
							});
			}

		function populate_tax_defaults()
			{
			var _html				= "";
			$.ajax({
					type:		"GET",
					url:		'index.aspx',
					data:		'a=xml_tax_defaults&tag_id='+$('#tag_id').val(),
					beforeSend:	function()
										{
										$('#tax_defaults').empty();
										},
					success:	function(xml)
									{
									$(xml).find('tax').each(function()
																{
																var tax_id			= $(this).children('id').text();
																var business_unit_id		= $(this).children('business_unit_id').text();
																var tax_id_1		= $(this).children('tax_id_1').text();
																var tax_id_2		= $(this).children('tax_id_2').text();
																var tax_id_3		= $(this).children('tax_id_3').text();
																var tax_id_4		= $(this).children('tax_id_4').text();
																var tax_id_5		= $(this).children('tax_id_5').text();
																_html				+= "\
<div>\
	<div>"+tax_id+"</div>\
	<div>"+select_box('company', business_unit_id)+"</div>\
	<div>"+tax_id_1+"</div>\
	<div>"+tax_id_2+"</div>\
	<div>"+tax_id_3+"</div>\
	<div>"+tax_id_4+"</div>\
	<div>"+tax_id_5+"</div>\
</div>";
																});
									},
					error:		function()
									{
									alert('Error retrieving tax defaults');
									},
					complete:	function()
									{
									$('#tax_defaults').html(_html);
									}
					});
			}


		function preselect_push(row)
			{
			var row_id		= $(row).closest("div").attr('id').replace(/div_/, "");
			$('#available_'+row_id+' option:selected').remove().appendTo('#selected_'+row_id+', #predefined_selected_'+row_id);
			tag_att_val_chk();
			}


		function preselect_pull(row)
			{
			var row_id		= $(row).closest("div").attr('id').replace(/div_/, "");
			var current_val	= $('#selected_'+row_id+' option:selected').val();
			$('#selected_'+row_id+' option:selected').remove().appendTo('#available_'+row_id);
			$('#predefined_selected_'+row_id+' option[value="'+current_val+'"]').remove();
			tag_att_val_chk();
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

		function add_attribute(obj)
			{
			var _target			= $(obj).parent().children('input:text');
			if(_target.val().length > 3)
				{
				location.href		= "./index.aspx?a=add_attribute&attribute="+encodeURIComponent(_target.val());
				}
			else
				{
				alert("Attribute is not int enough");
				}
			}


		function add_attribute_row()
			{
			var threshold					= $("#AttributeThreshold") != undefined ? $("#AttributeThreshold").val() : 0;
			var HasDependants				= $("#HasDependants") != undefined ? $("#HasDependants").val() : false;
			
			var counter_target				= document.getElementById("n_AttributeRows");
			var counter						= counter_target.value;
			var even_odd					= is_even(counter) ? "even" : "odd";
			var attribute_id				= "attribute_"+counter;
			var available_id				= "available_"+counter;
			var selected_id					= "selected_"+counter;
			var target						= document.getElementById("attributes");
			var html						= "";
			html							+= "\
<div class='attribute_row_"+even_odd+"' style='display:none;' id='div_"+counter+"'>\
	<span class='itemize_row'>\
		<input type='checkbox' value ='"+counter+"' class='descriptor' onclick='if(this.checked == 1){{descriptors(1, "+counter+");}}else{{descriptors(0, "+counter+");}}'>\
		<select id='order_"+counter+"'  name='order_"+counter+"' onchange='descriptor_order_chk(this)' class='n_order' style='display:none;'>";
		
			var max_descriptors				= counter+1;

			for(i=0;i<=10;i++)
				{
				var this_text;
				var this_selected;
				if(i == 0)
					{
					this_text				= "&nbsp;";
					this_selected			= " selected";
					}
				else
					{
					this_text				= i;
					this_selected			= "";
					}
				html						+= "<option value='"+i+"'"+this_selected+">"+this_text;
				}
			html							+= "\
		</select></span>\
	<span class='remove_row'><img onclick=\"remove_certain_attribute_row(this);\" src='/images/inventory/button/button[remove].png' /></span>\
	<table width='100%' id='table_"+counter+"' cellpadding='0' cellspacing='0'>\
		<tr>\
			<td colspan='3'><select  class='attribute' id='"+attribute_id+"' name='attribute_"+counter+"' onchange='tag_att_val_chk();if(this.value != 0){populate_selects(this.id, this.value);}else{reset_attribute_row("+counter+")}'></select></td>\
			<td>&nbsp;</td>\
		</tr>\
		<tr class='cat_head'>\
			<td>Available Values</td>\
			<td>&nbsp;</td>\
			<td>Selected Values</td>\
		</tr>\
		<tr>\
			<td align='center' width='45%'><select name='"+available_id+"' id='"+available_id+"' class='preselected' multiple></select></td>\
			<td align='center' width='10%'>\
				<button type='button' class='action' onclick='preselect_push(this);'>add &gt;</button><br/>\
				<button type='button' class='action' onclick='preselect_pull(this);'>&lt; delete</button>\
			</td>\
			<td align='center' width='45%'>\
				<select name='"+selected_id+"' id='"+selected_id+"' class='preselected' multiple disabled></select><br/>\
				<b>Fill all existing parts with this default value</b>\
				<select name='predefined_"+selected_id+"' id='predefined_"+selected_id+"' class='predefined_selected' disabled></select>\
			</td>\
		</tr>\
	</table>\
</div>";
			target.innerHTML				+= html;
			populate_attributes(attribute_id);
			$("div#div_"+counter).slideDown(0);
			counter_target.value			= counter / 1 + 1;
			if(counter < threshold && HasDependants == true)
				{
				$("#remove_row").attr("disabled", true);
				$("#save_tag").attr("disabled", true);
				}
			else
				{
				$("#remove_row").attr("disabled", false);
				$("#save_tag").attr("disabled", false);
				}
			reorder_attrows();
			tag_att_val_chk();
			}


			function populate_selects(i, aid)
				{
				i = i.replace(/attribute_/, "");
				populate_available(i, aid);
				populate_selected(i, aid);
				}


			function populate_attributes(att_id)
				{
				var options				= "<option value='0'>SELECT ATTRIBUTE</option>";
				$.ajax(	{
						type:			"GET",
						url:			"./index.aspx?a=xml_attributes",
						dataType:		"xml",
						beforeSend:		function()
											{
											$("#"+att_id).empty().append("<option value='0'>Please Wait</option>").attr('disabled', true);
											},
						success:		function(xml)
											{
											$(xml).find('attribute').each(
												function()
													{
													var id				= $(this).attr("id");
													var name			= $(this).attr("name");
													options				+= "<option value='"+id+"'>"+name+"</option>";
													});
											},
						complete:		function()
											{
											$("#"+att_id).empty().append(options).removeAttr('disabled');
											}});
				}

			function update_tag_properties(obj)
				{
				$.get("./index.aspx",
					{
					a:			"update_property",
					prop:		$(obj).attr('data-type'),
					val:		$(obj).val(),
					tag_id:		$('#tag_id').val()
					},
					function(data)
						{
						if(data != "SUCCESS")
							{
							alert(data);
							}
						});
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
				var options					= "";
				$.ajax(	{
						type:			"GET",
						url:			"./index.aspx?a=available_xml_values&tag_id="+i+"&aid="+aid,
						dataType:		"xml",
						beforeSend:		function()
											{
											$("#available_"+i).empty();
											$("#selected_"+i).empty();
											please_wait('begin');
											},
						success:		function(xml)
											{
											$(xml).find('value').each(
												function()
													{
													var id					= $(this).attr("id");
													var name				= $(this).attr("name");
													options					+= "<option value='"+id+"'>"+name+"</option>";
													});
											},
						complete:		function()
											{
											$("#available_"+i).empty().append(options);
											$("#selected_"+i).removeAttr("disabled");
											$("#predefined_selected_"+i).removeAttr("disabled");
											please_wait('end');
											}
						});
				}


			function populate_selected(i,aid)
				{
				var selected_target							= "selected_"+i;
				$("#"+selected_target).empty();
				$("#predefined_"+selected_target).empty();
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


		function remove_certain_attribute_row(obj)
			{
			
			var threshold					= 0;
			var HasDependants				= "False";

			if($("#HasDependants"))
				{
				HasDependants				= $("#HasDependants").val();
				}
			if($("#AttributeThreshold"))
				{
				threshold					= $("#AttributeThreshold").val();
				}
			
			var counter_target				= $("#n_AttributeRows");
			var parent						= $("#attributes");
			var nRows						= counter_target.val();
			if(nRows != 0)
				{
				nRows							= nRows / 1 -1;
				if(nRows == 0)
					{
					$("#save_tag").attr('disabled',true);
					}
				$(obj).parents('div:first').remove();
				reorder_attrows();
				counter_target.val(nRows);

				switch(HasDependants)
					{
					case "True":	
						if(nRows <= threshold)
							{
							$("#remove_row").attr("disabled", true);
							$("#save_tag").attr("disabled", true);
							}
						else
							{
							$("#remove_row").removeAttr("disabled");
							$("#save_tag").removeAttr("disabled");
							}
					break;
					case "False":	
						if(nRows == 0)
							{
							$("#remove_row").attr("disabled", true);
							$("#save_tag").attr("disabled", true);
							}
						else
							{
							$("#remove_row").removeAttr("disabled");
							$("#save_tag").removeAttr("disabled");
							}
					break;
					}
				}
			else
				{
				$("#remove_row").attr('disabled', true);
				$("#save_tag").attr('disabled', true);
				}
			tag_att_val_chk();
			}


		function remove_attribute_row()
			{
			var threshold					= 0;
			var HasDependants				= "False";

			if(document.getElementById("HasDependants"))
				{
				HasDependants				= document.getElementById("HasDependants").value;
				}
			if(document.getElementById("AttributeThreshold"))
				{
				threshold					= document.getElementById("AttributeThreshold").value;
				}


			var counter_target				= document.getElementById("n_AttributeRows");
			var parent						= document.getElementById("attributes");
			var nRows						= counter_target.value;

			if(nRows != 0)
				{
				nRows							= nRows / 1 -1;
				if(nRows == 0)
					{
					document.getElementById("save_tag").disabled		= true;
					}
			//	var child						= document.getElementById("div_"+nRows);
				$("div#div_"+nRows).remove();
				counter_target.value			= nRows;

				switch(HasDependants)
					{
					case "True":	if(nRows <= threshold)
										{
										$("#remove_row").attr("disabled", true);
										$("#save_tag").attr("disabled", true);
										}
									else
										{
										$("#remove_row").attr("disabled", false);
										$("#save_tag").attr("disabled", false);
										}
					break;
					case "False":	if(nRows == 0)
										{
										$("#remove_row").attr("disabled", true);
										$("#save_tag").attr("disabled", true);
										}
									else
										{
										$("#remove_row").attr("disabled", false);
										$("#save_tag").attr("disabled", false);
										}
					break;
					}
				}
			else
				{
				document.getElementById("remove_row").disabled		= true;
				document.getElementById("save_tag").disabled		= true;
				}

			tag_att_val_chk();
			}


		function check_bv(DSN)
			{
			var part_number_boxname		= "#PARTNUMBER_"+DSN;
			var part_number				= $(part_number_boxname).val();
			if(part_number == "")
				{
				alert("BLANK PART #");
				return false;
				}
			else
				{
				$.ajax(
						{
						type:			"GET",
						url:			"?a=xml_part_history&CODE="+part_number,
						dataType:		"xml",
						beforeSend:		function()
											{
											$("#loading").show();
											},
						success:		function(xml)
											{
											$(xml).find('branch').each(	
																		function()
																			{
																			var branch_id		= $(this).attr("id");
																			var vendor_code		= $(this).attr("vendor_code");
																			var cost			= $(this).attr("cost");
																			cost				= "$"+(cost / 1).toPrecision(2);
																			var last_sold		= $(this).attr("last_sold");
																			var last_bought		= $(this).attr("last_bought");
																			if(vendor_code != "N/A" && cost != "" && last_sold != "")
																				{
																				$("#PARTNUMBER_"+branch_id).attr("value", part_number);
																				$("#HISTORY_"+branch_id+" #VENDOR").html(vendor_code);
																				$("#HISTORY_"+branch_id+" #COST").html(cost);
																				$("#HISTORY_"+branch_id+" #SOLD").html(last_sold);
																				$("#HISTORY_"+branch_id+" #BOUGHT").html(last_bought);
																				$("#HISTORY_"+branch_id+" .STATUS").attr("src", "/images/inventory/filler/filler[status-success].gif");
																				if(	vendor_code == "" && 
																					branch_id == "" && 
																					cost == "" && 
																					last_sold == "")
																					{
																					$("#HISTORY_"+branch_id+" .STATUS").attr("src", "/images/inventory/filler/filler[status-error].gif");
																					}
																				else
																					{
																					$("#HISTORY_"+branch_id+" .STATUS").attr("src", "/images/inventory/filler/filler[status-success].gif");
																					}
																				}
																			else
																				{
																				$("#PARTNUMBER_"+branch_id).attr("value", "");
																				$("#HISTORY_"+branch_id+" #VENDOR").html("");
																				$("#HISTORY_"+branch_id+" #COST").html("");
																				$("#HISTORY_"+branch_id+" #SOLD").html("");
																				$("#HISTORY_"+branch_id+" #BOUGHT").html("");
																				$("#HISTORY_"+branch_id+" .STATUS").attr("src", "/images/inventory/filler/filler[status-error].gif");
																				}
																			}
																	);
											chk_part();
											},
						error:			function()
											{
											$("#loading").hide();
											},
						complete:		function()
											{
											$("#loading").hide();
											}
						});
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


		function search_inventory(bvid)
			{
			if(bvid)
				{
				$("#BV_RESULTS").empty();
				$("#search_shade").css({"opacity": "0.85"});
				$("#search_shade").fadeIn("slow");
				$("#search_popup").fadeIn("slow");
				$("#BRANCH_BV").empty();
				$("#BRANCH_BV").append(bvid);
				center_popup("#search_popup");
				var q			= $("#PARTNUMBER_"+bvid).attr("value");
				$("#search_popup #KEYWORD").attr("value", q);
				$("#search_popup #KEYWORD").focus();
				}
			else
				{
				$("#search_popup").fadeOut("slow");
				$("#search_shade").fadeOut("slow");
				}
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


		function find_parts(DSN)
			{
			var part_output						= "<table cellpadding='2' cellspacing='0'><tr><td class='h'>Doesn't Exist<span id='n_nonexists'></span></td><td class='h'>Exists<span id='n_exists'></span></td></tr><tr>";
			var n_exists						= 0;
			var n_nonexists						= 0;
			$.ajax(
					{
					type: "GET",
					url: "./index.aspx?a=xml_nonexistant_woparts&DSN="+DSN,
					dataType: "xml",
					beforeSend:	function()
									{
									$("#wo_parts").empty();
									$("#loading").show();
									},
					success:function(xml)
							{
							part_output			+= "<td valign='top'>";
							$(xml).find('part').each(
												function()
													{
													var CODE			= $(this).attr("code");
													CODE				= CODE.replace(/\+/, " ");
													var COUNT			= $(this).attr("count");
													part_output			+= "<div><b>"+CODE+"</b> <i>("+COUNT+") unit(s) on current sales orders</i></div>";
													n_nonexists++;
													}
												);
							},
					complete: function ()
							{
							part_output			+= "</td>";
							$.ajax(
									{
									type: "GET",
									url: "./index.aspx?a=xml_existant_woparts&DSN="+DSN,
									dataType: "xml",
									beforeSend:	function()
													{
													},
									success:function(xml)
											{
											part_output			+= "<td valign='top'>";
											$(xml).find('part').each(
																function()
																	{
																	var CODE			= $(this).attr("code");
																	CODE				= CODE.replace(/\+/, " ");
																	part_output			+= "<div><b>"+CODE+"</b></div>";
																	n_exists++;
																	}
																);
											part_output			+= "</td>";
											$("#loading").hide();
											},
									error:function(xml)
											{
											part_output			+= "<td>There was a problem retrieving parts</td>";
											$("#loading").hide();
											},
									complete: function()
											{
											part_output			+= "</tr></table>";
											$("#wo_parts").append(part_output);
											$("#n_exists").append(n_exists+" parts");
											$("#n_nonexists").append(n_nonexists+" parts");
											$("#loading").hide();
											}
									});
							},
					error:function(xml)
							{
							part_output			+= "<td>There was a problem retrieving parts</td>";
							$("#loading").hide();
							}
					});
			}


		function search_bv()
			{
			var DSN			= $("#BRANCH_BV").html();
			var Q			= $("#KEYWORD").attr("value");
			$.ajax(
					{
					type:		"GET",
					url:		"./index.aspx?a=xml_lookup_part&Q="+Q+"&DSN="+DSN,
					dataType:	"xml",
					beforeSend:	function()
										{
										$("#BV_RESULTS").empty();
										$('#loading').show();
										},
					success:	function(xml)
										{
										$(xml).find('match').each(
															function()
																{
																var CODE			= $(this).attr("code");
																var DESCRIPTION		= $(this).attr("description");
																$("#BV_RESULTS").append("<div class='match'><div class='code'><button type='button' style='cursor:pointer;' onclick=\"$('#PARTNUMBER_"+DSN+"').val($(this).text());search_inventory();\">"+CODE+"</button></div><div class='description'>"+DESCRIPTION+"</div></div>");
																}
															);
										},
					error:		function(xml)
									{
									$("#BV_RESULTS").append("No results found");
									$('#loading').hide('slow');
									},
					complete:	function()
									{
									$('#loading').hide();
									}
					});
			}


		function build_att_val_boxes(tag_id)
			{
			var i						= 0;
			var att_vals				= $('#att_vals');
			var references				= $('#this_references');
			for(j=0; j < Branch_DSNS.length; j++)
				{
				var branch				= Branch_DSNS[j];
				$("#PARTNUMBER_"+branch).attr("value", "");
				$("#HISTORY_"+branch+" #VENDOR").html("");
				$("#HISTORY_"+branch+" #COST").html("");
				$("#HISTORY_"+branch+" #SOLD").html("");
				$("#HISTORY_"+branch+" #BOUGHT").html("");
				$("#HISTORY_"+branch+" #STATUS").attr("src", "/images/inventory/filler/filler[status].gif");
				}


			$.ajax(
						{
						type:		"GET",
						url:		"./index.aspx?a=xml_tag_att_val&tag_id="+tag_id,
						dataType:	"xml",
						beforeSend: function()
										{
										att_vals.empty();
										$("#loading").show();
										references.hide();
										},
						success:	function(xml)
										{
										att_vals.empty();
										$('#n_attributes').attr('value', $(xml).find('attribute').size());
										$(xml).find('attribute').each(	
																	function()
																		{
																		var id							= $(this).attr("id");
																		var name						= $(this).attr("name");
																		var html						= "<div class='att'>"+name+"</div>";
																		html							+= "<input type='hidden' id='attributebox_"+i+"' class='attribute' value='"+id+"'><div class='val'><input type='hidden' value='"+id+"'><select onchange='chk_part();' name='' data-attribute_id='"+id+"' id='valuebox_"+i+"'><option value='0' selected>CHOOSE VALUE</option>";
																		$(this).find('value').each(
																									function()
																										{
																										var value_id				= $(this).attr("id");
																										var value_name				= $(this).attr("name");
																										html						+= "<option value='"+value_id+"'>"+value_name+"</option>";
																										}
																									);
																		html							+= "</select><br/><input class='value_add' type='text' style='width:92%;font-size:11px;'/>";//<button onclick='ShowNewValue(this, "+i+");'><img src='/images/icon/icon[add].gif'/></button><div class='flyout' style='position:absolute;display:none;' id='newvaluebox_"+i+"'><b>new value</b><br/><input type='text'onkeypress='if(this.value != \"\" && checkEnter(event)){PushNewValue("+i+")}' id='newvalue_"+i+"'/></div></div>";
																		att_vals.append(html);
																		i++;
																		}
																);
										//$("#add_reference_button").fadeIn(2500);
										},
						complete:	function()
										{
										$("#loading").hide();
										att_vals.show();
										references.show();
										$(att_vals).append("\
<div id='stuff'></div>\
<script>\
$('body').find('.value_add').each(\
function()\
	{\
	$(this).bind('keydown', \
	function(e)\
		{\
		if(e.which == 13)\
			{\
			add_value(this);\
			$(this).blur();\
			return false;\
			}\
		});\
	}\
	);\
</script>");
										}
						}
					);
			}

		function add_value(obj)
			{
			var _val				= $(obj).val();
			var _parent				= $(obj).parents('.val:eq(0)');
			var _att				= _parent.find('input:hidden').val();
			var _valbox				= _parent.find('select');
			var _tag_id				= $('#TAG_ID').val();
			$.get('./index.aspx',
						{
						'a':		'tag_value',
						'att':		_att,
						'val':		_val,
						'tag_id':	_tag_id
						},
						function(ret)
							{
							if(ret.match(/^([0-9]+)$/g))
								{
								_valbox.append("<option value='"+ret+"'>"+_val+"</option>").val(ret);
								$(obj).val('');
								chk_part();
								}
							else if(ret == "Rollback")
								{
								alert("There was a problem adding this value to the tag preset.");
								}
							else if(ret == "Exists")
								{
								alert("This value already exists in the database and is either not linked to this tag, or has not been qc'ed.");
								$(obj).val('');
								}
							else
								{
								alert("Couldn't save value");
								}
							});
			}

		function chk_BranchPart(business_unit_id)
			{
				return true;
			var box_length			= $("#PARTNUMBER_"+business_unit_id).attr("value").length;
			if(box_length == 0)
				{
				var history			= "#HISTORY_"+business_unit_id;
				$(history+" #VENDOR").html("");
				$(history+" #COST").html("");
				$(history+" #SOLD").html("");
				$(history+" #BOUGHT").html("");
				$(history+" #STATUS").attr("src", "/images/inventory/filler/filler[status].gif");
				}
			}


		function sortNumber(a,b)
			{
			return a - b;
			}


		function chk_part()
			{
			var n_attributes			= $("#n_attributes").attr("value");
			var chk						= 0;
			for(i=0;i<=n_attributes;i++)
				{
				var attribute_box		= "#attributebox_"+i;
				//alert($(attribute_box).attr('value'));
				var part_box			= "#valuebox_"+i;
				if($(part_box).attr("value") == 0)
					{
					chk++;
					}
				}
			if(chk <= 0)
				{
				$("#SAVE_BUTTON").attr("disabled", false);
				$("#SAVE_BUTTON").css("cursor", "pointer");
				$("#SAVE_BUTTON img").attr("src", "/images/icon/icon[save].gif");
				}
			else
				{
				$("#SAVE_BUTTON").attr("disabled", true);
				$("#SAVE_BUTTON").css("cursor", "");
				$("#SAVE_BUTTON img").attr("src", "/images/icon/icon[save][disabled].gif");
				return false;
				}
			}
//

		function PushNewPart()
			{
			var master_id			= $("#master_id").val();
			var tag_id				= $("#TAG_ID").val();
			var n_attributes		= $("#n_attributes").val();
			var type_of				= $("#type_of").val();
			var branch_boxes		= 0;
			if(master_id != "")
				{
				chk_uniqueness(master_id);
				}
			else
				{
				chk_uniqueness();
				}
				
			$('#references').find('.branch_box').each(function()
														{
														if($(this).find('input:checkbox:checked').size() > 0)
															{
															branch_boxes++;
															}
														});
			var $querystring		=	{
										tag:				tag_id,
										attributes_n:		n_attributes,
										old_data_n:			branch_boxes,
										type:				type_of,
										att_vals:			[],
										old_data:			[]
										};
			// Attributes & Values
			$('#att_vals').find('select').each(function()
											{
											$querystring.att_vals.push($(this).val());
											});
			$querystring.att_vals	= $querystring.att_vals.sort(sortNumber);
			
			// Old Referencial Data
			$('#references').find('.branch_box').each(function()
														{
														if($(this).find('input:checkbox:checked').size() > 0)
															{
															var q						=	{
																							dsn:		$(this).attr('data-DSN'),
																							part_no:	$(this).find('.part_number').val(),
																							pricing_no:	0,
																							pricing:	[]
																							};
															q.pricing_no				= $(this).find('input:checkbox:checked').size();
																							
															$(this).find('.vendor_row').each(function()
																						{
																						var _this					= this;
																						if($(this).find('input:checkbox').is(':checked'))
																							{
																							var _v					= $(_this).find('.vendor').text();
																							var _c					= $(_this).find('.cost').text();
																							var _p					= $(_this).find('.part_no').text();
																							q.pricing.push({
																											v:		_v,
																											c:		_c,
																											p:		_p
																											});
																							}
																						});
															$querystring.old_data.push(q);
															}
														});
			switch(part_is_unique)
				{
				case true:	$.ajax(
									{
									type:		"POST",
									cached:		false,
									url:		"./index.aspx",
									data:		$querystring,
									beforeSend:	function()
													{
													$("#SAVE_BUTTON").attr("disabled", "disabled");
													please_wait('begin');
													},
									error:		function (xhr, ajaxOptions, thrownError)
													{
													$("#SAVE_BUTTON").removeAttr("disabled");
													document.write(xhr.responseText);
													},
									success:	function(msg)
													{
													switch(msg)
														{
														case "MASTER FAILED":			alert("There was a problem inserting the master part number, possibly due to a formatting issue in the description");
														break;
														case "DETAIL FAILED":			alert("There was a problem inserting the part detail");
														break;
														case "INVALID REQUEST":			alert("There was an unknown error saving this information");
														break; 
														case "CANT CLEAR REFERENCES":	alert("Tried, but failed clearing past part references");
														break; 
														case "CANT CLEAR DETAILS":		alert("Tried, but failed clearing past part details");
														break; 
														case "CANT ROLLBACK":			alert("Part was entered although there was a problem inserting your reference, and the part could not be rolled back.\n Please talk to Matt about this.");
														break; 
														case "BAD REFERENCE":			alert("The reference part number submitted could not be entered due to there already being a reference for that part number.\n Your request has been rolled back.");
														break;
														default:
															if(msg.match(/SUCCESS/g))
																{
																var split_results		= msg.split("-");
																var new_master_id		= split_results[1];
																location.href			= "./index.aspx?a=edit_part&master_id="+new_master_id;
																}
															else
																{
																alert(msg);
																}							
														break;
														}
													},
									complete:	function()
													{
													please_wait('end');
													}
									});
				break;
				case false:	alert("If I saved this, it would create a duplicate part.\n\n	Part not saved.\n\n	Please try again.");
				break;
				}
			}

//

		function PushEditPart()
			{
			var tag_id				= $("#TAG_ID").attr("value");
			var n_attributes		= $("#n_attributes").attr("value");
			var master_id			= $("#master_id").attr("value");
			var valbox_pre			= "#valuebox_";
			var att_pre				= "attribute_";
			var val_pre				= "value_";
			var av_id				= "";
			var type_of				= $("#type_of").attr("value");
			if(master_id != "")
				{
				chk_uniqueness(master_id);
				}
			else
				{
				chk_uniqueness();
				}

			var post_data;
			switch(type_of)
				{
				case "EDIT":	post_data			= "a=editpart&master_id="+master_id+"&tag_id="+tag_id+"&n_attributes="+n_attributes;
				break;
				case "NEW":		post_data			= "a=newpart&tag_id="+tag_id+"&n_attributes="+n_attributes;
				break;
				}
			console.log(post_data);
			for(i = 0;i<n_attributes;i++)
				{
				av_id				= $(valbox_pre+i).attr("value");
				post_data			+= "&AV"+i+"="+av_id;
				}
			//for(var j = 0;j < Branch_DSNS.length;j++)
			//	{
			//	eval("var "+Branch_DSNS[j]+" = $('#PARTNUMBER_"+Branch_DSNS[j]+"').val();");
			//	}
			//
			var appendature			= "";
			//for(var k = 0;k < Branch_DSNS.length;k++)
			//	{
			//	var dsn				= Branch_DSNS[k];
			//	appendature			+= "&"+Branch_DSNS[k]+"="+eval(dsn);
			//	}
			post_data				= post_data+appendature;
			switch(part_is_unique)
				{
				case true:	$.ajax(
									{
									type:		"GET",
									cached:		false,
									url:		"./index.aspx?"+post_data,
									beforeSend:	function()
													{
													$("#SAVE_BUTTON").attr("disabled", "disabled");
													please_wait('begin');
													},
									error:		function (xhr, ajaxOptions, thrownError)
													{
													$("#SAVE_BUTTON").removeAttr("disabled");
													document.write(xhr.responseText);
													},
									success:	function(msg)
													{
													switch(msg)
														{
														case "MASTER FAILED":			alert("There was a problem inserting the master part number, possibly due to a formatting issue in the description");
														break;
														case "DETAIL FAILED":			alert("There was a problem inserting the part detail");
														break;
														case "INVALID REQUEST":			alert("There was an unknown error saving this information");
														break; 
														case "CANT CLEAR REFERENCES":	alert("Tried, but failed clearing past part references");
														break; 
														case "CANT CLEAR DETAILS":		alert("Tried, but failed clearing past part details");
														break; 
														case "CANT ROLLBACK":			alert("Part was entered although there was a problem inserting your reference, and the part could not be rolled back.\n Please talk to Matt about this.");
														break; 
														case "BAD REFERENCE":			alert("The reference part number submitted could not be entered due to there already being a reference for that part number.\n Your request has been rolled back.");
														break;
														case "SUCCESS":					alert("Saved");
														break;
														default:						alert(msg);
														}
													},
									complete:	function()
													{
													please_wait('end');
													}
									});
				break;
				case false:	alert("If I saved this, it would create a duplicate part.\n\n	Part not saved.\n\n	Please try again.");
				break;
				}
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
			$(child_dropdown_target).append("<option selected>"+newValue.toUpperCase()+"</option>");
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

		function select_selected()
			{
			var howmany						= $("#n_AttributeRows").attr("value");
			for(i=0; i < howmany; i++)
				{
				var target_name				= "#selected_"+i;
				$(target_name).each(function()
											{
											$(target_name+" option").attr('selected', 'selected');
											});	
				}
			$('#aspnetForm').submit();
			$('#save_tag').attr('disabled', true);
			}

		function tag_att_val_chk()
			{
			var counter_target				= document.getElementById("n_AttributeRows");
			var howmany						= counter_target.value;
			switch(howmany)
				{
				case 0:						document.getElementById("save_tag").disabled		= true;
											return false;
				break;
				default:					var attribute_pre				= "attribute_";
											var selected_pre				= "selected_";
											var save_disabled				= false;				// for blatant errors
											var duplicate_attributes		= false;
											var misordered_attributes		= false;

											var attribute_array				= new Array();
											var attribute_array_length		= 0;
											var SelectedCounter				= 0;
											var attribute_target			= "";
											var selected_target				= "";
											var attribute_target_name		= "";
											var selected_target_name		= "";

											for(i=0;i<howmany;i++)
												{
												var not_in_range			= false;
												attribute_target_name		= attribute_pre+i;
//alert(attribute_target_name);
												selected_target_name		= selected_pre+i;
												attribute_target			= document.getElementById(attribute_target_name);
												selected_target				= document.getElementById(selected_target_name);
//document.getElementById("errormsg").innerHTML += "["+i+"] The "+attribute_target.tagName+" tag \nidentified as: '"+$("#attribute_"+i).attr('id')+"'\n with a name of: '"+$("#attribute_"+i).attr('name')+"'\n has a value of: "+$("#attribute_"+i).attr('value')+"<br/>";
												var select_length			= selected_target.options.length;

												attribute_array[i]			= $("#attribute_"+i).attr('value');
//alert("attribute_array["+i+"] = "+attribute_array[i]);
											// Check if there is any attributes NOT defined
												if($("#attribute_"+i).attr('value') == 0)
													{
													save_disabled			= true;
													}
											// Make sure there are SOME values selected
												if($("#selected_"+i)[0].options.length < 1)
													{
													save_disabled			= true;
													}
												else
													{
													//alert($("#selected_"+i).length);
													}
												}//end for block

										// Check for duplicate attributes
											attribute_array_length			= attribute_array.length;
											if(array_has_duplicates(attribute_array))
												{
//alert(attribute_array.join("\n"));
												duplicate_attributes								= true;
												}

											if(!descriptors_evaluate())
												{
												misordered_attributes								= true;
												}

//alert("save_disabled = "+save_disabled+"\n duplicate_attributes = "+duplicate_attributes);
											if(save_disabled == true  || duplicate_attributes == true)
												{
												document.getElementById("save_tag").disabled		= true;
												return true;
												}
											else
												{
												document.getElementById("save_tag").disabled		= false;
												return false;
												}
				break;
				}
			}


		function reorder_attrows()
			{
			var tables						= document.getElementById("attributes").getElementsByTagName('table');
			for(t=0;t<tables.length;t++)
				{

				// ESTABLISH THE INITIAL TARGET
				var table				= tables[t];


				// RECOLOR ROWS DEPENDING ON IF THEY ARE EVEN/ODD

				if(is_even(t))	
					{
					$(table).parent().attr('class', 'attribute_row_even');
					}
				else
					{
					$(table).parent().attr('class', 'attribute_row_odd');
					}

				// ESTABLISH CONTAINER OF INNER SELECTS
				var selects				= document.getElementById(table.id).getElementsByTagName('select');	
				if(selects[0].disabled == false)
					{
					selects[0].id		= "attribute_"+t;
					selects[0].name		= "attribute_"+t;
					}

				selects[1].id			= "available_"+t;
				selects[1].name			= "available_"+t;

				//alert("renaming '"+$(selects[2]).attr('id')+"' to 'selected_"+t+"'");
				$(selects[2]).attr('id', 'selected_'+t);
				$(selects[2]).attr('name', 'selected_'+t);

			// REORDER HIDDEN FIELDS
				var hidden				= document.getElementById(table.id).getElementsByTagName('input');
				if(hidden.length != 0)
					{
					hidden[0].id			= "attribute_"+t;
					hidden[0].name			= "attribute_"+t;
					}

				$(table).attr('id', 'table_'+t);
				$(table).parent().attr('id','div_'+t);
				}
			$('#n_AttributeRows').attr('value', tables.length);
			}

		function populate_browser()
			{
			var tag_id			= $("#TAG_ID").attr("value");
			var onchange_event	= "";
			var html			= "";
			$.ajax({
					type:		"GET",
					cached:		false,
					url:		"./index.aspx?a=prepop_xml_tag_att_val&tag_id="+tag_id,
					dataType:	"xml",
					error:		function (xhr, ajaxOptions, thrownError){$("#results").text("There is a problem communicating with the database - "+ajaxOptions);},
					beforeSend:	function()
									{
									$("#results").empty();
									$("#parts").empty();
									html					= "<div class='header'>FILTERS</div>";
									$("#loading").show();
									},
					success:	function(xml)
									{
									var i					= 0;
									var this_disabled		= "";
									var n_attributes		= $(xml).find('attribute').size();
									var type;
									$('#n_attributes').attr('value', n_attributes);
									$(xml).find('attribute').each(
										function()
											{
											var att_id						= $(this).attr("id");
											var att_name					= $(this).attr("name");
											html							+= "<div id='att_"+i+"'><input type='hidden' id='att_id' value='"+att_id+"'><input type='hidden' id='n' value='"+i+"'>";
											html							+= "<div class='att_name'>"+att_name+"</div><select id='val_"+i+"' class='val' onchange='part_match();'><option value='0'>SELECT VALUE";
											$(this).find('value').each(
												function()
													{
													var val_id				= $(this).attr('id');
													var val_name			= $(this).attr('name');
													html					+= "<option value='"+val_id+"'>"+val_name;
													});
											html							+= "</select></div>";
											i++;
											}
										);
									},
					complete:	function()
									{
									$('#loading').hide();
									$("#results").show();
									$("#parts").empty();
									$("#results").append(html);
									part_match(0);
									}
					});
			}
		
		
		function part_match(type)
			{
			if(!type)
				{
				type					= 0;
				}
			var max_atts				= $('#n_attributes').attr('value');
			var tag_id					= $('#TAG_ID').attr('value');
			var tag_pattern				= "";
			var att_vals				= [];
			for(var i=0;i<max_atts;i++)
				{
				var val_id				= $("#val_"+i).attr("value");
				att_vals[i]				= val_id;
				}
			var TagPattern			= att_vals.sort(sortNumber);
			//var url				= "?a=xml_match_parts&tag_id=9&attvals=1;
			var url					= "?a=xml_match_parts&tag_id="+tag_id+"&attvals="+TagPattern;
			var html;
			$.ajax({	type:			"GET",
						cached:			false,
						url:			url,
						dataType:		"xml",	
						beforeSend:	function()
										{
										$("#loading").show();
										$("#parts").empty();
										},
						success:		function(xml)
											{
											html	= "<div class='header'>PARTS</div><div class='scrollbox'><table width='100%' cellpadding='2' cellspacing='0'>";
											$(xml).find('matches').each(
												function()
													{
													var size_of				= $(this).find('part').size();
													if(size_of > 0)
														{
														$(this).find('part').each(
															function()
																{
																var master_id		= $(this).attr('masterid');
																var number			= $(this).attr('number');
																var description		= $(this).attr('description');
																var active			= $(this).attr('active');
																var checked			= "";
																if(active == "1")
																	{
																	checked			= "checked='true'";
																	}
																else
																	{
																	checked			= "";
																	}
																html				+= "<tr class='part' id='row_"+master_id+"'>\
																							<td class='numbers'><a onclick='location.href=\"?a=edit_part&master_id="+master_id+"&tag_id="+tag_id+"\";' title='View this part's info'>("+master_id+") "+description+"</a></td>\
																							<td class='buttons'>\
																								<input type='checkbox' title='Delete this part' onclick='toggle_part("+master_id+", this)' value='"+active+"' "+checked+"/>\
																							</td>\
																						</tr>";
																});
														}
													else
														{
														html		+= "<tr class='nothing'><td>Nothing matches this criteria</td></tr>";
														}
													});
											html		+= "</table></div>";
											},
						complete:		function()
											{
											$("#parts").append(html);
											$("#parts").show();
											$("#loading").hide();
											},
						error:			function()
											{
											alert("There was an issue loading the parts for this tag");
											}
						});
			}

		function view_part(master_id, number)
			{

			var url						= "./index.aspx?a=xml_part_att_val&master_id="+master_id;
			var tag_id					= "";
			var html;

			$.ajax({	type:			"GET",
						cached:			false,
						url:			url,
						dataType:		"xml",
						beforeSend:		function()
											{
											$("#part_info .part_number").html("");
											$("#part_info #part_attvals").html("<img src='/images/loading_panel.gif' vspace='50'/>");
											$("#part_detail").css({display:"block",opacity:0}).animate( {opacity:0.75, backgroundColor:'#000'}, 500);
											$("#part_info").css({display:"block",opacity:0}).animate( {opacity:1}, 500);
											center_popup("#part_info");
											},
						error:			function()
											{
											alert("There was an error retrieving the information on this part");
											},
						success:		function(xml)
											{
											html	= "<table width='100%' cellpadding='5' cellspacing='0' class='attvals'>";
											$(xml).find('info').each(
												function()
													{
													tag_id				= $(this).children('tag_id').text();
													var attribute		= $(this).children('attribute').text();
													var value			= $(this).children('value').text();
													html				+= "<tr><td class='attribute'>"+attribute+":</td><td class='value'>"+value+"</td></tr>";
													});
											html	+= "</table><button type='button' onclick=\"location.href='./index.aspx?a=edit_part&master_id="+master_id+"&tag_id="+tag_id+"';\">View Complete Part</button>";
											},
						complete:		function()
											{
											$("#part_attvals").html(html);
											$("#part_info .part_number").html(number);
											}
						});
			}

		function close_part()
			{
			$("#part_info").hide();
			$("#part_detail").animate( {opacity:0}).css({display:"none"});
			}

// ---------------------------------------------------------------------
		function toggle_part(master_id, obj)
			{
			$(obj).attr('disabled', true);
			var string_val		= "";
			if($(obj).val() == "0" || $(obj).val() == 'false')
				{
				string_val		= "This will disable this part from use, are you sure you want to do this?";
				}
			else
				{
				string_val		= "This will enable this part for use, are you sure you want to do this?";
				}
			if(master_id != "")
				{
				please_wait('begin');
				$.get("index.aspx",
	                        {
	                        a:              "toggle_part",
	                        master_id:		master_id
	                        },
							function(data)
								{
								if(data == "success")
									{
									if(location.href.match('edit_part'))
										{
										// Change button text
										switch($(obj).val().toLowerCase())
											{
											case "true":
												$(obj).html("<b>Enable Part</b><div style='font-size:10px;'>(currently disabled)</div>");
												$(obj).val("false");
											break;
											case "false":
												$(obj).html("<b>Disable Part</b><div style='font-size:10px;'>(currently enabled)</div>");
												$(obj).val("true");
											break;
											}
										}
									else
										{
										// 
										}
									}
								else
									{
									alert("There was a problem toggling the active state of this part, consult tech support before retrying this");
									}
								$(obj).attr('disabled', false);
								please_wait('end');
								});
				}
			else
				{
				$(obj).attr('disabled', false);
				}
			}
// ---------------------------------------------------------------------
		function merge_part(obj)
			{
			$(obj).attr('disabled', true);
			var string_val			= "";
			var from_master_id		= $(obj).attr('data-old_master_id');
			var to_master_id		= $(obj).prevAll("input:text").val();
			if(from_master_id != undefined && from_master_id != "0" && from_master_id != 'false')
				{
				string_val		= "This will merge "+from_master_id+" into "+to_master_id+".\nAre you sure you want this done?";
				}
				
			if(from_master_id != "" && to_master_id != "")
				{
				if(confirm(string_val))
					{
					location.href			= "./index.aspx?a=merge_part&from_master_id="+from_master_id+"&to_master_id="+to_master_id;
					}
				else
					{
					$(obj).attr('disabled', false);
					}
				}
			else
				{
				alert("You must supply a part to merge to.");
				$(obj).attr('disabled', false);
				}
			}
// ---------------------------------------------------------------------