		var layout_obj		=
			{
			save_template_callback:
				{
				begin:
					function(s,e)
						{
						//console.log("Starting Callback");
						please_wait("start");
						},
				end:
					function(s,e)
						{
						//console.log("Ended Callback");
						please_wait("stop");
						},
				error:
					function(s,e)
						{
						//console.log("Errored during Callback");
						please_wait("stop");
						},
				complete:
					function(s,e)
						{
						//console.log("Completed Callback");
						please_wait("stop");
						if(e.result.indexOf(",") != -1)
							{
							var re		= e.result.split(',');
							if(re[1] != 'invalid')
								{
								dde_filter.SetText(re[1]);
								}
							if(re[2] != '')
								{
								alert(re[2]);
								}
							else if(re[1] != 'invalid')
								{
								alert("Successfully saved layout '"+re[1]+"'");
								}
							if(re[1] == '')
								{
								location.reload();
								}
							if(re[1] != 'invalid')
								{
								h.Set('ID', re[0]);
								}
							}
						else
							{
							alert(e.result);
							}
						}
				},
			selection_handler:
				function(obj)
					{
					var layout_id	= $(obj).attr("data-id");
					cb_save_template.PerformCallback("setdefault^"+layout_id);
					$(obj).attr("selected", true);
					return false;
					},
			gv_toggle_handler:
				function(s,e)
					{
					var gv_id		= eval(h.Get("gridview_id"));
					if(gv_id.IsCustomizationWindowVisible())
						{
						gv_id.HideCustomizationWindow();
						}
					else
						{
						gv_id.ShowCustomizationWindow();
						}
					},
			row_click_handler: function (s, e) {
				console.log("Row click handler triggered");

				if (e.htmlEvent.target.tagName != "INPUT") {
					console.log("Event target is not an input element");

					var current_id = h.Get("ID");
					var this_id = gv_templates.cpIDs[e.visibleIndex];
					var is_owner = gv_templates.cpISOWNERs[e.visibleIndex];
					var owner = gv_templates.cpOWNERs[e.visibleIndex];
					var gv_id = eval(h.Get("gridview_id"));

					console.log("Current ID: " + current_id);
					console.log("This ID: " + this_id);
					console.log("Is Owner: " + is_owner);
					console.log("Owner: " + owner);
					console.log("Gridview ID: " + gv_id);

					if (current_id != this_id) {
						console.log("Current ID is different from this ID, updating layout");

						h.Set("ID", this_id);
						h.Set("NAME", gv_templates.cpNAMEs[e.visibleIndex]);
						var layout_name = is_owner ? gv_templates.cpNAMEs[e.visibleIndex] : owner + "'s " + gv_templates.cpNAMEs[e.visibleIndex];

						console.log("Layout Name: " + layout_name);

						dde_filter.SetText(layout_name);
						gv_id.PerformCallback(gv_templates.cpLAYOUTs[e.visibleIndex]); // This relies on cp or Client Properties, which are set on the cs end.
						dde_filter.HideDropDown();
					} else {
						console.log("Layout already loaded, no action taken");
						alert("Layout already loaded");
						return false;
					}
				} else {
					console.log("Event target is an input element, no action taken");
				}
			},

			gv_checkcallback:
				function(gv)
					{
					return gv.InCallback();
					},
			process_save:
				function(l)
					{
					console.log(l);
					var gv_id			= eval(h.Get("gridview_id"));
					if(!layout_obj.gv_checkcallback(gv_id))
						{
						console.log("Not in callback");
						var id				= h.Get('ID');
						var o				= 	{
												type:	"save",
												id:		id,
												name:	l,
												exp:	gv_id.cpExp,
												gv:		h.Get("gridview_id")
												};
						if($.trim(o.name) == "")
							{
							console.log("Cannot save a blank template, please provide a name");
							s.Focus();
							}
						else
							{
							console.log("Going to perform callback");
							var json_s		= JSON.stringify(o);
							console.log(json_s);
							console.log(json_s.length);
							cb_save_template.PerformCallback(json_s);
							h.Set('NAME', l);
							}
						gv_templates.Refresh();
						}
					else
						{
						console.log("In callback");
						setTimeout("layout_obj.process_save(\""+l+"\")", 50);
						}
					},
			gv_save:
				function(s,e)
					{
					var gv_id			= eval(h.Get("gridview_id"));		
					console.log(e.buttonIndex	);
					console.log(s.GetText());
					if(e.buttonIndex == 0) // Save
						{
						layout_obj.process_save(s.GetText());
						gv_id.Refresh();
						}
					else if(e.buttonIndex == 1 && confirm("Are you sure you want to clear the current layout?")) // New
						{
						h.Set('ID', null);
						h.Set('NAME', null);
						dde_filter.SetText("");
						gv_id.PerformCallback("new");
						}
					else if(e.buttonIndex == 2) // Copy
						{
						var id			= h.Get('ID');
						var o			= 	{
											type:	"copy",
											id:		null,
											name:	s.GetText()+" Copy",
											exp:	gv_id.cpExp,
											gv:		h.Get("gridview_id")
											};
						console.log(o.exp);
						if($.trim(o.name) == "")
							{
							return;
							}
						else
							{
							console.log("This should be saving...");
							cb_save_template.PerformCallback(JSON.stringify(o));
							h.Set('NAME', s.GetText());
							}
						gv_templates.Refresh();
						}
					else if(e.buttonIndex == 3) // Delete
						{
						var id			= h.Get('ID');
						var o			= 	{
											type:	"delete",
											id:		id
											};
						if(confirm("Are you sure you want to delete this layout?"))
							{
							cb_save_template.PerformCallback(JSON.stringify(o));
							}
						gv_templates.Refresh();
						}
					}

			};