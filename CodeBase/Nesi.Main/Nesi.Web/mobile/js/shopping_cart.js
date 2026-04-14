		
		var cart		=
			{
			// inv, tags, atts, vals are container objects.. they get populated when the search occurs
			inv_base: null,
			inv_unique_base: null,
			inv_workingset: null,
			selected_values: [],
			selected_atts: [],
			included_ids: [],
			tags: null,
			atts: null,
			vals: null,
			d_refine: null,
			d_results: null,
			scroll_to_top:
				function()
					{
					$("html, body").animate({ scrollTop: 0 }, 250);
					},
			update_panel_progress:
				{
				bind:
				function () {
				Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(cart.update_panel_progress.start);
				Sys.WebForms.PageRequestManager.getInstance().add_endRequest(cart.update_panel_progress.stop);
				},
				start:
				function () {
					please_wait("start");
				},
				stop:
				function (sender, args) {
					if (args != undefined && args != null) {
						if (args.get_error() != undefined) {
							var errorMessage = args.get_error().message;
							args.set_errorHandled(true);
							alert(errorMessage);
						}
					}
					cart.bind();
					please_wait("stop");
                    set_controls();
					var i = parent.window.document.getElementsByClassName("if_shopping_cart")[0];
					if(i === undefined) return;
					i.height = (parent.window.document.body.scrollHeight - 100) + 'px';
					i.height = '500px';
					
				}
				},
			control_handler:
				{
				type:
					function(_s,_e)
						{
						var t		= _s.GetValue();
						if (t === 3 || t === 4 || t === 6)
                            {
                            window.ddl_type_id.PerformCallback(_s.GetValue());
                            window.ddl_type_id.SetVisible(true);
                            $(".tb_search").hide();
							switch(t)
								{
								case 30:
								$(".type_id input:text").val("Select a group");
								break;
								case 40:
								$(".type_id input:text").val("Select a kit");
								break;
								case 3:
								$(".type_id input:text").val("Select a work order");
								break;
								case 4:
								$(".type_id input:text").val("Select a quote");
								break;
							  
								}
                            }     
						else
                            {
							window.ddl_type_id.SetVisible(false);
                            $(".tb_search").show();
                            }   
						},
				cb_search:
					{
					endcallback:
						function(_s, _e)
							{
							if (s.cp_alert!=null && s.cp_alert !== "")
								{
								alert(s.cp_alert);
								s.cp_alert = null;
								}
							window.gv_cart.PerformCallback();
							window.gv_other_parts.PerformCallback("refresh");
							window.cbp_header.PerformCallback();
							}
					}
				},
			contents:
				{
				bt_delete:
					{
					verify: 
						function(_s, _e)
							{
							_e.ProcessOnServer = confirm("Are you sure you want to delete this shopping cart and its contents?");
							}
					},
				bt_consolidate:
					{
					verify: 
						function(_s, _e)
							{
							if (confirm("Are you sure you want to consolidate your cart?"))
								{
								gv_cart.PerformCallback("consolidate");
								}
							}
					},
				bt_clear:
					{
					verify:
						function(_s, _e)
							{
							if (confirm("Are you sure you want to clear out your cart?"))
								{
								gv_cart.PerformCallback("clear");
								}
							}
					},
				gv_cart:
					{
					endcallback:
						function(_s, _e)
							{
							if (_s.cp_refresh_other_grid === 1)
								{
								_s.cp_refresh_other_grid = null;
								window.gv_other_parts.PerformCallback("refresh");
								window.cbp_header.PerformCallback();
								}
							},
					rowclick:
						function(_s,_e)
							{
							if(_s.GetFocusedRowIndex() !== _e.visibleIndex)
								{
								_s.SetFocusedRowIndex(_e.visibleIndex);
								window.gv_other_parts.PerformCallback("refresh");
								window.cbp_header.PerformCallback();
								}
							}
					},
				gv_otherparts:
					{
					endcallback:
						function(_s, _e)
							{
							if (_s.cp_refresh_other_grid === 1)
								{
								_s.cp_refresh_other_grid = null;
						//		window.gv_cart.PerformCallback("refresh");
						//		window.cbp_header.PerformCallback();
								}
							},
					add_part:
						{
						click:
							function(_s,_e)
								{
								window.gv_other_parts.PerformCallback("add_other");
								}
						}
					}
				},
			checkout:
				{
				controls:
					{
					toggle_quantities:
						function(obj)
							{
							}
					}
				},
			add_items:
				{
				    parts: [],
				    check:
                        function(obj)
                        {
					    
                            cart.add_items.parts		= [];
                            $(".inv").each(function()
                            {
						    
                                var master_id = $(this).find(".ma")[0].childNodes[0].textContent ;
                                //		    console.log(master_id);

                                var qty_req = $(this).find(".qty_req")[0].value;
                                var qty_t = $(this).find(".qty_t")[0].value;
                                var qty_s = $(this).find(".qty_s")[0].value;
                                qty_req = $.trim(qty_req)== "" ? 0 : qty_req/1;
                                qty_t = $.trim(qty_t) == "" ? 0 : qty_t/1;
                                qty_s = $.trim(qty_s) == "" ? 0 : qty_s/1;


                                if (master_id > 0 && ((qty_req > 0) || (qty_t > 0) || (qty_s > 0))) {
                                    cart.add_items.parts.push({ m: master_id, q: qty_req, s: qty_s, t: qty_t });

                                }

                            });
                            var l = cart.add_items.parts.length;
                            if(l > 0)
                            {
                      //          $(".addtocart").removeClass("inactive").addClass("active").removeAttr("disabled").text("Add "+l+" part(s) to WO");
                            }
                            else
                            {
                      //          $(".addtocart").removeClass("active").addClass("inactive").attr("disabled",true).text("Add parts to WO");
                            }
                           
                        }
                ,

				check_noNegativeValues:
				        function () {
				            var found = 0;
				            $(".inv").each(function () {

				                var master_id = $(this).find(".ma")[0].childNodes[0].textContent;
				                var qty_req = $(this).find(".qty_req")[0].value;
				                var qty_t = $(this).find(".qty_t")[0].value;
				                var qty_s = $(this).find(".qty_s")[0].value;
				                qty_req = $.trim(qty_req) == "" ? 0 : qty_req / 1;
				                qty_t = $.trim(qty_t) == "" ? 0 : qty_t / 1;
				                qty_s = $.trim(qty_s) == "" ? 0 : qty_s / 1;


				                if (master_id > 0 && ((qty_req > 0) || (qty_t > 0) || (qty_s > 0))) {

				                } else {
				                    if ((qty_req < 0) || (qty_t < 0) || (qty_s < 0)) {
				                        found = 1;
				                    }
				                }

				            });

				            if (found == 1) {
				                // any numbers is negative, trigger jquery validation by click button. 
				                var searchButton = document.getElementById('cb_search_btn_search');
				                if (searchButton != null) {
				                    searchButton.click();
				                }

				                return false;
				            }

				            return true;
				        }
				    ,

				validate:
                    function (_obj) {
                        var onhand = $(_obj).attr("data-internal_qty") / 1;
                        var truck = $(_obj).attr("data-truck_qty") / 1;
                        var _class = $(_obj).attr("class");
                        var qty = $(_obj)[0].value/1;
                        
                      
                        if (_class == "qty_t")
                        {
                            if (truck == 9999999999)
                            {
                                confirm("You need to select a truck first.");
                                $(_obj)[0].value = '';
                            }
                            if (qty>truck)
                            {
                                confirm("You only have " + truck + " on the truck.. taking more will force the quantity into negative at this location.");
                              //  $(_obj)[0].value = truck;
                            }
                        }
                        else if (_class == "qty_s")
                        {
                            if (qty > onhand) {
                                confirm("You only have " + onhand + " in stock.. taking more will force the quantity into negative at this location.");
                               // $(_obj)[0].value = onhand;
                            }
                        }

                    }
                    ,
				run: 
					function() {
                        if (!this.check_noNegativeValues()) {
                            return;
                        }

					    var l = cart.add_items.parts.length;
					
						if(l === 0)
							{
							alert("Please select some parts");
							return;
							}
						var part_json		= "{data :"+JSON.stringify(cart.add_items.parts)+"}";
					cb_addpart.PerformCallback(part_json);
						},
				cb:
					{
					begin:
						function(_s,_e)
							{
							please_wait("start");
							},
						
					end:
						function(_s,_e)
							{
						    please_wait("stop");

                       
						    


							},
							
					complete:
						function(_s,_e)
							{
						//    cbp_header.PerformCallback();
						    
						    //			gv_cart.PerformCallback("refresh");
						    var is_annual = $(".hdn_is_annual").val();
                            
						    var rows = _e.result.split(',');
					//	    console.log(_e.result);
						    //	    console.log(rows.length);
						    if (is_annual != 1) {
						        for (var i = 0; i < rows.length; i++) {
						            var fields = rows[i].split('|');
						            var master_id = fields[0];
						            //	        console.log(master_id);
						            var new_v = fields[1] / 1;
						            //	        console.log(new_v);
						            var target = $(".t" + master_id);
						            var c_v = target.find(".qr").text() / 1;
						            //	        console.log(c_v);
						            var woprog_id = $(".hdn_woprog_id").val();
						            //        if (woprog_id == 0) {
						            target.find(".qr").text(c_v - new_v);
						            //       }
						            c_v = target.find(".qc").text() / 1;
						            target.find(".qc").text(c_v + new_v);
						        }

						        if (cb_addpart.cp_alert != null && cb_addpart.cp_alert != '') {
						            confirm(cb_addpart.cp_alert);
						            cb_addpart.cp_alert = null;
						        }
						    }
						    else {
						        parent.close_annual_addpart();
						        
						    }
							},
							
					error:
						function(_s,_e)
							{
							please_wait("stop");
							},
					}
				},
			selected_attvals:
				{
				add:
					function(_obj)
					{
					    var search_path = $("#search_path");
						var aid					= $(_obj).attr("data-aid")/1;
						var vid					= $(_obj).attr("data-vid")/1;
						var div					= cart.make_element("div", {"class" : "av", onclick : "cart.selected_attvals.remove("+vid+", "+aid+");", "id" : "val"+vid}, "");
						//var delete_img			= cart.make_element("input", {type : "image", "src" : "/images/icon/icon[delete].gif", "style" : "width:16px;height:16px;", "align" : "absmiddle", "id" : "p"+vid}, "");
						var label_img			= cart.make_element("label", {"for" : "p"+vid}, $(_obj).parent().parent().prev(".a").attr("data-att") + " : "+$(_obj).next("label").attr("data-v"));
						//div.appendChild(delete_img);
						div.appendChild(label_img);
						search_path.append(div);
						cart.selected_values.push(vid);
						cart.selected_atts.push(aid);
						search_path.show();
						cart.refine.run();
						},
				remove:
					function(_v_id, _a_id)
						{
						var index_v				= cart.selected_values.indexOf(_v_id);
						var index_a				= cart.selected_atts.indexOf(_a_id);
						if(index_v > -1 && index_a > -1)
							{
							cart.selected_atts.splice(index_a, 1);
							cart.selected_values.splice(index_v, 1);
							$("#val"+_v_id).remove();
							}
						cart.refine.run();
						if(cart.selected_values.length === 0)
							{
							$("#search_path").hide();
							}
						}
				},
			refine:
				{
				handle:
					function(_obj)
						{
						var cb		= $(_obj);
						if(cb.is(":checked"))
							{
							cart.selected_attvals.add(cb);
							}
						cart.refine.run();
						},
				run:
					function()
						{
						cart.d_refine			= $("#div_refine");
						var allowed_values		= cart.results(false, 1); // Done to populate the included_ids object & allowed_values
						var allowed_parts		= cart.results(false, 2); // Done to populate the included_ids object & allowed_values
						var search_path = $("#search_path");
						cart.d_refine.empty();
						var target_atts			= [];
						var target_vals = [];
						var woprog_id = $(".hdn_woprog_id").val();
						var location_id = $(".hdn_location_id").val();
						if(cart.selected_values.length > 0)
							{
							var local_parts			= [];
							for(var i = 0; i < cart.selected_values.length; i++)
								{
								var vv				= cart.selected_values[i];
								var parts			= cart.filter.parts_by_value(allowed_parts, vv);
								$.each(parts, function() // Filter to only parts that include the selected value
									{
									if(local_parts.indexOf(this.m) === -1)
										{
										local_parts.push(this.m);
										}
									});

								$.each(local_parts, function() // Now only include values and attributes in those parts
									{
									var m_id				= this / 1;
									var part				= cart.filter.part_by_id(m_id);
									for(var vi = 0; vi < part.v.length;vi++)
										{
										var ret_val		= cart.filter.value(part.v[vi]);
										var v_obj		= {id : ret_val.id, v: ret_val.v, aid: ret_val.aid }
										var val_exists	= false;
										for(var vii = 0; vii < target_vals.length; vii++)
											{
											var vii_obj		= target_vals[vii];
											if(vii_obj.id === ret_val.id)
												{
												val_exists	= true;
												}
											}
										if(!val_exists)
											{
											target_vals.push(v_obj);
											}
										}
									for(var ai = 0; ai < part.a.length; ai++)
										{
										var ret_att		= cart.filter.attribute(part.a[ai]);
										var a_obj		= {"id" : ret_att.id, "a": ret_att.a };
										var att_exists	= false;
										for(var aii = 0; aii < target_atts.length; aii++)
											{
											var aii_obj		= target_atts[aii];
											if(aii_obj.id === ret_att.id)
												{
												att_exists	= true;
												}
											}
										if(!att_exists)
											{
											target_atts.push(a_obj);
											}
										}
									});
								}
							}
						else
							{
							target_atts		= cart.atts;
							target_vals		= cart.vals;
							}
						var c_shown_atts		= 0;
						var c_shown_vals = 0;
						
						var back_button			= cart.make_element("button", {type:"button","class":"button back spacer", onclick:"cart.refine.toggle.flyout();"}, "Close Filter");
						cart.d_refine.append(back_button);
						cart.d_refine.append(search_path);
						var div_n_subresults	=  cart.make_element("div", {"id":"n_subresults"}, "");
						cart.d_refine.append(div_n_subresults);
						var frag_attval			= document.createDocumentFragment();
						$.each(target_atts, function()
							{
							var a_obj			= this;
							c_shown_atts++;
							var vals_obj		= cart.filter.values_by_predefined_att(target_vals, a_obj["id"]);
							var c				= 0;
							var vals_div		= cart.make_element("div", {"class" : "vals"}, "");
							$.each(vals_obj, function()
								{
								var v_obj			= this;
								var v_id			= v_obj["id"] / 1;
								var a_id			= v_obj["aid"] / 1;
								if(cart.selected_values.indexOf(v_id) === -1 && cart.selected_atts.indexOf(a_id) === -1 && allowed_values.indexOf(v_id) > -1)
									{
									c++;
									var sub_div			= cart.make_element("div", {"class" : "v", "data-id" : v_id}, "");
									var n_parts			= cart.filter.n_parts(allowed_parts, v_id);
									var chk				= cart.make_element("input", {"data-aid":a_id,  "data-vid" : v_id, "id" : "val"+v_id, "onclick":"cart.refine.handle(this);", "type":"checkbox"}, "");
									var val_text		= cart.make_element("label", {"for" : "val"+v_id, "data-v" : v_obj["v"]}, v_obj["v"]+" ("+n_parts+" parts)");
									sub_div.appendChild(chk);
									sub_div.appendChild(val_text);
									vals_div.appendChild(sub_div);
									}
								});
							var att_div			= cart.make_element("div", {"class" : "a open", "data-id": a_obj["id"], "data-att": a_obj["a"], onclick: "cart.refine.toggle.group(this);"}, a_obj["a"]+" ("+c+" options)");
							if(c > 1)
								{
								frag_attval.appendChild(att_div);
								frag_attval.appendChild(vals_div);
								c_shown_vals++;
								}
							});
					
						cart.d_refine.append(frag_attval);
                        
						if(c_shown_vals === 0 && !cart.d_refine.is(":hidden"))
							{
						    cart.refine.toggle.flyout();
						   
							}
						if(c_shown_vals !== 0)
							{
							$(".button.refine").show();
							$(".button.addtocart").show();
							}
						else
							{
							$(".button.refine").hide();
							$(".button.addtocart").show();
						}

						if (woprog_id == "0" && location_id == "0") {
						    $(".button_placeholder").hide();
						    $(".caption_row").hide();
						    $(".qty_row").hide();

						}

						cart.results(true, 0);
					},
				toggle:
					{
					group:
						function(_obj)
							{
							var this_vals		= $(_obj).next(".vals");
							var is_hidden		= this_vals.is(":hidden");
							this_vals.toggle();
							$(_obj).removeClass("open").removeClass("close");
							$(_obj).addClass(is_hidden ? "close" : "open");
							},
					flyout:
						function()
							{
							cart.d_refine			= $("#div_refine");
							if(cart.d_refine.is(":hidden"))
								{
								$("html").css({overflow:"hidden"});
								}
							else
								{
								$("html").css({overflow:"scroll"});
								}
							cart.d_refine.toggle();
						
							}
					}
				},
			results:
				function(_do_append, _arr_type)
				{
				    var woprog_id = $(".hdn_woprog_id").val();
				    var location_id = $(".hdn_location_id").val();
				    var is_annual = $(".hdn_is_annual").val();

					cart.d_results		= $("#div_results");
					//cart.d_results.hide();
					var allowed_values	= [];
					var allowed_parts	= [];
					if(_do_append)
						{
						cart.included_ids	= [];
						cart.d_results.empty();
						}
					var table					= !_do_append ? null : cart.make_element("table", {id:"results_table", cellpadding:0, cellspacing:0,width:"100%"}, "");
					var thead					= !_do_append ? null : cart.make_element("thead", {}, "");
					var thead_tr				= !_do_append ? null : cart.make_element("tr", {}, "");
					if(_do_append)
						{
					    thead_tr.appendChild(cart.make_element("th", {}, "Part"));
						thead_tr.appendChild(cart.make_element("th", {}, "Still Needed"));
						thead_tr.appendChild(cart.make_element("th", {}, "On WO"));
						thead_tr.appendChild(cart.make_element("th", {}, "Require on WO"));
						thead_tr.appendChild(cart.make_element("th", {}, "Commit on WO from Truck"));
						thead_tr.appendChild(cart.make_element("th", {}, "Commit on WO from Stock"));
		//				thead.appendChild(thead_tr);
		//				table.appendChild(thead);
						}
					var tbody					= !_do_append ? null : cart.make_element("tbody", {}, "");
					var trs						= document.createDocumentFragment();
					var limit					= 10000;
					var limit_i					= 0;
					for(var i = 0; i < cart.inv_base.length; i++)
						{
						if(_do_append)
							{
							limit				= 1000;
							}
						var i_obj				= cart.inv_base[i];
						
						var v_array				= i_obj["v"];
						var a_array				= i_obj["a"];
						var tag = cart.filter.tag(i_obj["t"])[0].t;
						var master_id =   i_obj["m"] / 1 ;
						var tr_tbody = !_do_append ? null : cart.make_element("tr", { "class": "inv" }, "");
						var td_tbody = !_do_append ? null : cart.make_element("td", {}, "");
						var tbl_tbody = !_do_append ? null : cart.make_element("table", { "width": "100%","cellpadding":"0","cellspacing":"0" }, "");
						var tbl_top_row = !_do_append ? null : cart.make_element("table", { "class": "t" + master_id, "width": "100%", "cellpadding": "0", "cellspacing": "0" }, "");
						var tr_tbl_top_row = !_do_append ? null : cart.make_element("tr", { "class": "inv1" }, "");
						var td_tbl_top_row = !_do_append ? null : cart.make_element("td", {}, "");

						var td_still_needed_caption = !_do_append ? null : cart.make_element("td", { "class": "top_right_caption" }, is_annual==0? "In Stock:":"");
						var td_on_wo_caption = !_do_append ? null : cart.make_element("td", { "class": "top_right_caption" }, is_annual==0? "On WO:":"");

						if ((woprog_id == 0)&&(is_annual==0)) {
						    td_still_needed_caption = !_do_append ? null : cart.make_element("td", { "class": "top_right_caption" }, "In Stock:");
						    if (location_id != 0) {
						        td_on_wo_caption = !_do_append ? null : cart.make_element("td", { "class": "top_right_caption" }, "On Truck:");
						    }
						}
						var tr2 = !_do_append ? null : cart.make_element("tr", {},"");

						var tr_top_row = !_do_append ? null : cart.make_element("tr", { "class": "inv1" }, "");
                        var td_tr_top_row = !_do_append?null:cart.make_element("td",{"colspan":"3"},"")

                        var tr_qtyrow = !_do_append ? null : cart.make_element("tr", { "class": "qty_row" }, "");

                        var tr_captions = !_do_append ? null : cart.make_element("tr", { "class": "caption_row" }, "");

						var caption_td_req			= !_do_append ? null : cart.make_element("td", {"class" : "caption"}, "Add to Req Qty");
						var caption_td_truck = !_do_append ? null : cart.make_element("td", { "class": "caption" }, "Take from Truck");
						var caption_td_stock = !_do_append ? null : cart.make_element("td", { "class": "caption" }, "Take from Stock");

						var input_td_req			= !_do_append ? null : cart.make_element("td", {"class" : "in l"}, "");
						var input_td_truck = !_do_append ? null : cart.make_element("td", { "class": "in l" }, "");
						var input_td_stock = !_do_append ? null : cart.make_element("td", { "class": "in l" }, "");
						var id_td_masterid = !_do_append ? null : cart.make_element("td", { "class": "ma l", "data-internal_qty": i_obj["q"], "data-truck_qty": i_obj["tr"] }, master_id);
						var id_div_description = !_do_append ? null : cart.make_element("td", { "class": "ma de", "onclick": "pop_part('" + master_id + "');" }, tag + " - " + i_obj["d"]);


						var qty_req_td = !_do_append ? null : cart.make_element("td", { "class": "qr" }, i_obj["q"]);
						var qty_cmt_td = !_do_append ? null : cart.make_element("td", { "class": "qc" }, i_obj["w"]);

						if (woprog_id == 0) {

						    if (location_id != 0 && is_annual == 0) {
						        qty_req_td = !_do_append ? null : cart.make_element("td", { "class": "qr" }, i_obj["q"]);
						        qty_cmt_td = !_do_append ? null : cart.make_element("td", { "class": "qc" }, i_obj["tr"]);
						    }
						}

						var input_req = !_do_append ? null : cart.make_element("input", { "class": "qty_req", type: "number", min: 0, "data-internal_qty": i_obj["q"], "data-truck_qty": i_obj["tr"], onchange: "cart.add_items.check(this);", onkeyup: is_annual==0?"cart.add_items.validate(this);":"" }, "");
						var input_com_truck = !_do_append ? null : cart.make_element("input", { "class": "qty_t", type: "number", min: 0, "data-internal_qty": i_obj["q"], "data-truck_qty": i_obj["tr"], onchange: "cart.add_items.check(this);", onkeyup: is_annual == 0 ? "cart.add_items.validate(this);" : "" }, "");
						var input_com_stock = !_do_append ? null : cart.make_element("input", { "class": "qty_s", type: "number", min: 0, "data-internal_qty": i_obj["q"], "data-truck_qty": i_obj["tr"], onchange: "cart.add_items.check(this);", onkeyup: is_annual ==0 ? "cart.add_items.validate(this);" : "" }, "");
						var filter_matches = cart.filter.matches(v_array);
						var id_already_exists	= cart.included_ids.length > 0 ? cart.included_ids.indexOf(master_id) > -1 : false;
						var show_row			= !id_already_exists && filter_matches;
						if(show_row)
							{
							if(_do_append && limit_i < limit)
							{
							    tr_tbody.appendChild(td_tbody);
							    td_tbody.appendChild(tbl_tbody);
							    td_tr_top_row.appendChild(tbl_top_row);
							    tr_top_row.appendChild(td_tr_top_row);  
							    
							    tbl_top_row.appendChild(tr_tbl_top_row);
							    tr_tbl_top_row.appendChild(id_td_masterid);// add master id td to tr row
							    tr_tbl_top_row.appendChild(td_still_needed_caption);
							    tr_tbl_top_row.appendChild(qty_req_td);
                                tr2.appendChild(id_div_description); // add description to master id td
							    tr2 .appendChild(td_on_wo_caption);
							    tr2 .appendChild(qty_cmt_td);
                                tbl_top_row.appendChild(tr2);

							    

							    tr_captions.appendChild(caption_td_req);
							   
							    tr_captions.appendChild(caption_td_stock);
                                tr_captions.appendChild(caption_td_truck);
							    
							    input_td_req.appendChild(input_req); // add req input to  td
							    tr_qtyrow.appendChild(input_td_req); // add req input td td to row

							    input_td_stock.appendChild(input_com_stock)
							    tr_qtyrow.appendChild(input_td_stock);
                                  input_td_truck.appendChild(input_com_truck);
							    tr_qtyrow.appendChild(input_td_truck);

							    tbl_tbody.appendChild(tr_top_row);
							    tbl_tbody.appendChild(tr_captions);
							    
							    tbl_tbody.appendChild(tr_qtyrow);
							  
							    tbody.appendChild(tr_tbody);
								}
							cart.included_ids.push(master_id);
							limit_i++;
							}
						if(filter_matches)
							{
							allowed_parts.push(i_obj);
							$.each(v_array, function()
								{
								var v		= this / 1;
								if(allowed_values.indexOf(v) === -1)
									{
									allowed_values.push(v);
									}
								});
							}
						};
					if(_arr_type !== 0)
						{
						if(_arr_type === 1)
							{
							return allowed_values;
							}
						else
							{
							return allowed_parts;
							}
						}
					else
						{
						if(limit_i > 0)
							{
							table.appendChild(tbody);
							cart.d_results.append(table);
				//			$("#results_table").tablesorter();
							}
						var warning		= "";
						if(limit_i > 100)
							{
							var add		= !$(".tb_search").is(":hidden") ? "add words to your search criteria, or" : "";
							warning		= "<div class='warning'>You may want to "+add+" filter your results by choosing from below.</div>";
							}
					    //			$("#refine_button_pane").html("<div class='n'>"+limit_i+" part(s) returned</div>"+warning);
						$("#refine_button_pane").html("<button type='button' style='height:35px; background-color:white; font-size:2.0em;  width:100%;border-style:none;' onclick='cart.refine.toggle.flyout();'>Refine results (" + limit_i + ")</button>");
						$("#n_subresults").html("<div class='n'>"+limit_i+" part(s) returned</div>"+warning);

						if (woprog_id == "0" && location_id == "0") {
						    $(".button_placeholder").hide();
						    $(".btn_777").hide();
						    $(".qty_req").hide();
						    $(".qty_t").hide();
						    $(".qty_s").hide();
						    $(".top_right_caption").hide();
						    $(".qc").hide();
						    $(".qr").hide();
						    $(".caption").hide();
						}
						if (is_annual == 1)
						{
						    $(".qty_req").hide();
						    $(".qty_t").hide();
						    $(".top_right_caption").hide();
						    $(".caption").hide();
						    $(".qc").hide();
						    $(".qr").hide();
						}

						return true;
						}
					},
			make_element: // Supply this the type of element you want to create, all of it's attributes in a simple object, and text if desired
				function( _type, _attributes, _text)
					{
					var e = document.createElement(_type);
					for(var p in _attributes)
						{
						if (_attributes.hasOwnProperty(p))
							{
							e.setAttribute(p, _attributes[p]);
							}
						}
						e.textContent	= _text;
					return e;
					},
			change_tab:
				function(s,e)
					{
					if(parent.window != null)
						{
						//parent.window.resizeIframe(parent.window.document.getElementsByClassName("if_shoppingcart")[0]);
						}
					},
			bind: function()
				{
				if(cart.atts != null)
					{
					cart.included_ids = [];
					cart.selected_atts = [];
					cart.selected_values = [];
					cart.refine.run(false);
					}
				},
			filter:
				{
				tag:
					function(_id)
						{
						var match = [];
						for (var i = 0; i < cart.tags.length; i++)
							{
							var t = cart.tags[i]["id"];
							if (t / 1 === _id / 1) 
								match.push(cart.tags[i]);
							}
						return match;
						},
				attribute:
					function(_id)
						{
						var match = [];
						for (var i = 0; i < cart.atts.length; i++)
							{
							var t = cart.atts[i]["id"];
							if (t / 1 === _id / 1) 
								match.push(cart.atts[i]);
							}
						return match[0];
						},
				value:
					function(_id)
						{
						var match = [];
						for (var i = 0; i < cart.vals.length; i++)
							{
							var t = cart.vals[i]["id"];
							if (t / 1 === _id / 1) 
								match.push(cart.vals[i]);
							}
						return match[0];
						},
				matches:
					function(_part_attvals)
						{
						//part_attvals.filter(function(_av){return cart.selected_values.indexOf(_av) > -1}).length === cart.selected_values.length;
						var match = [];
						for (var i = 0; i < _part_attvals.length; i++)
							{
							var v = _part_attvals[i]/1;
							if (cart.selected_values.indexOf(v) > -1) 
								match.push(v[i]);
							}
						return match.length === cart.selected_values.length;
						},
				n_parts:
					function(_allowed_parts, _v_id)
						{
						//allowed_parts.filter(function(_inv){return _inv.vid === v_id;}).length
						var match = [];
						for (var i = 0; i < _allowed_parts.length; i++)
							{
							var inv_obj		= _allowed_parts[i];
							if(inv_obj.v.indexOf(_v_id) > -1)
								match.push(inv_obj);
							}
						return match.length;
						},
				parts_by_value:
					function(_allowed_parts, _v_id)
						{
						var match = [];
						for (var i = 0; i < _allowed_parts.length; i++)
							{
							var inv_obj		= _allowed_parts[i];
							if(inv_obj.v.indexOf(_v_id) > -1)
								match.push(inv_obj);
							}
						return match;
						},
				part_by_id:
					function(_master_id)
						{
						var match = [];
						for (var i = 0; i < cart.inv_base.length; i++)
							{
							var inv_obj		= cart.inv_base[i];
							if(inv_obj.m === _master_id)
								match.push(inv_obj);
							}
						return match[0];
						},
				values_by_predefined_att:
					function(_target_vals, _id)
						{
						//var vals_obj		= target_vals.filter(function(_v){return _v.aid === a_obj["id"];});
						var match = [];
						for (var i = 0; i < _target_vals.length; i++)
							{
							var val_obj		= _target_vals[i];
							if(val_obj.aid === _id)
								match.push(val_obj);
							}
						return match;
						}

				}
			};

		function pop_part(id) {
		    var x = document.getElementById("pop_part");
		    x.style.visibility = 'visible';
		    cb_part.PerformCallback(id);
		    cb_fix_qty.PerformCallback(id);
		}
		function pop_777() {
		    var x = document.getElementById("pop_777");
		    x.style.visibility = 'visible';
		    cb_777.PerformCallback();
		   
		}
		function close_pop_part() {
		    var x = document.getElementById("pop_part");
		    x.style.visibility = 'hidden';
		    return false;
		}
		function close_pop_777()
		{
		    var x = document.getElementById("pop_777");
		    x.style.visibility = 'hidden';
		    return false;
		}

		function pop_fix_qty(id) {
		    var x = document.getElementById("pop_fix_qty");
		    x.style.visibility = 'visible';

		}
		function close_fix_qty() {
		    var x = document.getElementById("pop_fix_qty");
		    x.style.visibility = 'hidden';
		    return false;
		}