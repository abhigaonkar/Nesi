
	var local_ticket	= 
		{
		vote:
			function(ticket_id, should_vote)
				{
				if(typeof(gv_tickets) != "undefined")
					{
					gv_tickets.PerformCallback("v|"+ticket_id+"|"+should_vote);
					}
				else if(typeof(client_cbp_left) != "undefined" && client_cbp_left.GetVisible())
					{
					client_cbp_left.PerformCallback("v|"+ticket_id+"|"+should_vote);
					}
				else
					{
					cb.PerformCallback("v|"+ticket_id+"|"+should_vote)
					}
				},
		handle_status:
			function(s,e)
				{
				var status				= s.GetValue();
				var t_user_selector		= s.GetMainElement().id.indexOf("popup") > -1 ? $(".popup_waitingon_user") : $(".row_waiting");
				t_user_selector.css({"display": status == 3 || status == 9 ? "block" : "none" });
				},
		task:
			{
			add:
                function (s, e) {
                    var newTask = $.trim(memo_new_task.GetText());
                    if (newTask == "") {
                        alert("Please add some text to the task.");
                    } else {
                        task_list.PerformCallback('new|' + newTask);
                    }
                },
			setassignee:
				function(s,e)
					{
					task_list.PerformCallback('setassignee|'+$(s.mainElement).parents('.tk_body:first').attr('data-id')+'|'+s.GetValue());
					},
			complete:
				function(obj)
					{
					task_list.PerformCallback('complete|' + $(obj).attr('data-id') + '|' + $(obj).is(':checked'));
					},
			edit_text:
				function(s,e)
					{
					task_list.PerformCallback('edittask|'+$(s.mainElement).parents('.tk_body:first').attr('data-id')+'|'+s.GetText());
					},
			do_delete:
				function(s,e)
					{
					task_list.PerformCallback('delete|'+$(s.mainElement).parents('.delete:first').attr('data-id'));
				},
			taskList:
			    {
			    begin:
                    function (s, e) {
                        btAddTask.SetEnabled(false);
                        please_wait('Start');
                    },
			    end:
                    function(s, e) {
                        if (s.cpResult == 'SUCCESS') {
                            memo_new_task.SetText('');
                            memo_new_task.Focus();
                        }
                        please_wait('Stop');
                        btAddTask.SetEnabled(true);
                        delete s.cpResult;
                    }
    			}
			},
		handle_image:
			function(obj, event)
				{
				var file = $(obj).attr("data-file");
				var id = $(obj).attr("data-id");
				var is_fs = document.fullscreenElement || document.webkitFullscreenElement || document.mozFullScreenElement || document.msFullscreenElement;
				var fs_enabled = "fullscreenEnabled" in document || "webkitFullscreenEnabled" in document || "mozFullScreenEnabled" in document || "msFullscreenEnabled" in document;
				if(!event.shiftKey && is_fs)
					{ 
					if ("exitFullscreen" in document)
						{
						document.exitFullscreen();
						}
					else if ("webkitExitFullscreen" in document)
						{
						document.webkitExitFullscreen();
						}
					else if ("mozCancelFullScreen" in document)
						{
						document.mozCancelFullScreen();
						}
					else if ("msExitFullscreen" in document)
						{
						document.msExitFullscreen();
						}
					}
				else
					{
					if (!event.shiftKey && fs_enabled)
						{
						obj.requestFullScreen = obj.msRequestFullscreen || obj.webkitRequestFullScreen || obj.mozRequestFullScreen || obj.requestFullScreen;
						obj.requestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
						}
					else if(!is_fs)
						{
						if(file != '' && id != '')
							{
							boing('./include.ashx?file='+file+'&download=false', 'ticketattachment_'+id, $(window).width(), $(window).height());
							}
						}
					}
				},
		handle_unload:
			{
			handle_submit:
				function(s,e)
					{
					window.onbeforeunload	= null; 
					},
			set:
				function()
					{
					window.onbeforeunload	= local_ticket.handle_unload.run;
					},
			remove:
				function()
					{
					window.onbeforeunload	= null; 
					},
			run:
				function()
					{
					return "Your changes have not been saved.";
					},
			check:
				function (s,e)
					{
					var t		= s.GetHtml();
					if(t.length > 0)
						{
						local_ticket.handle_unload.set();
						}
					else
						{
						local_ticket.handle_unload.remove();
						}
					}
			}
		};
			function handle_collapse(obj)
				{
				var p_m		= $(obj).parents(".message:first");
				var p_c		= $(obj).parents(".col");
				if(p_m.size() === 1) // It's a message
					{
					p_m.children(".m_body:first").toggle();
					return;
					}
				if(p_c.size() === 1) // It's a column
					{
					var is_vis		= p_c.is(":visible");
					if(is_vis)
						{
						
						}
					else
						{
						//p_c.css({"float":"left"});
						}
					p_c.children(".body:first").toggle();
					return;
					}
				}
			function handle_ticket_updated(should_close, s, e)
				{
				var o		= window.opener;
				if(s == null || (s != null && s.cpError == false))
					{
					alert('Ticket Updated');
					if(o != null)
						{
						if(o.location.href.match(/tickets\/list/g))
							{
							o.gv_tickets.Refresh();
							}
						else if(window.opener.location.href.match(/huddle\//g))
							{
							o.gv.Refresh();
							o.gv_source.Refresh();
							}
						else if(typeof o.gv_existing_tickets !== 'undefined')
							{
							o.gv_existing_tickets.Refresh();
							}
						else 
							{
							o.location.href = o.location.href;
							}
						}

					if(should_close)
						{
						window.close();
						var href		= check_mobile() ? "/mobile/index.aspx?a=tickets" : "/default.aspx";
						$("body").html("<div align='center'>Couldn't automatically close window.<br/><br/><button type='button' onclick=\"location.href = '"+href+"';\">Go Home</button><button type='button' onclick=\"location.href = location.href;\">Reload Page</button></div>");
						return;
						}
					}
				else if(s != null && s.cpAction == "addtracker" && s.cpError == false)
					{
					}
				else if(s != null && s.cpError && s.cpErrorAction != null)
					{
					eval(s.cpErrorAction);
					}
				if(s != null)
					{
					delete s.cpErrorAction;
					delete s.cpAction;
					delete s.cpError;
					}
				}
			function check_mobile()
				{
				var check = false;
				(function(a){if(/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a)||/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0,4)))check = true})(navigator.userAgent||navigator.vendor||window.opera);
				return check;
				}
			function handle_pasted(obj)
				{
				$(".tb_pastebox").css({ "border":"dashed 3px #999", "background-repeat":"no-repeat","background-size":"100%", backgroundImage: "url('/images/ticket/bg[paste].png')"});
				$(".mainfileupload").parents("tr:first").show();
				$(".cancelpasted").hide();
				}
			function bind_pastebox()
				{
				$(".tb_pastebox").css({ "padding":"5px", "border":"dashed 3px #999","background-repeat":"no-repeat","background-size":"155px 120px","background-position" : "center", backgroundImage: "url('/images/ticket/bg[paste].png')"});
				$(".tb_pastebox").pasteImageReader(
					function(results)
						{
						var dataURL, filename;
						$(".tb_pastebox").css({ "border":"solid 3px #090", "background-repeat":"no-repeat","background-size":"100%","overflow-y":"scroll", backgroundImage: "url('" + results.dataURL + "')"});
						// alert(results.dataURL);
						$(".fileblob").attr("value", results.dataURL);
						$(".mainfileupload").parents(".br:first").hide();
						$(".cancelpasted").show();
						return filename = results.filename, dataURL = results.dataURL, results;
						});
				}
			$(document).ready(function()
				{
				setTimeout("bind_pastebox()", 500);
				});
			function handle_chat_tabchange(s,e)
				{
				switch(e.tab.index)
					{
					case 0:
					break;
					case 1:
						if(typeof memo_symptom !== 'undefined')
							{
							memo_symptom.Focus();
							}
					break;
					case 2:
						if(typeof memo_cause !== 'undefined')
							{
							memo_cause.Focus();
							}
					break;
					case 3:
						if(typeof memo_solution !== 'undefined')
							{
							memo_solution.Focus();
							}
					break;
					case 4:
						if(typeof memo_roi !== 'undefined')
							{
							memo_roi.Focus();
							}
					break;
					}
				}
			function handle_sidechat(id)
				{
				pc_chat.PerformCallback(id);
				}
			function handle_reopen(s,e)
				{
				var reason		= memo_whyreopen.GetText();
				if(reason == "" || reason.length < 5)
					{
					alert("You must supply a reason why you this ticket is to be reopened");
					memo_whyreopen.Focus();
					return false;
					}
				if(confirm("Are you sure you want this ticket reopened?"))
					{
					cb_request_reopen.PerformCallback(reason);
					}
				}
			function img_expand(img)
				{
				boing($(img).attr("src"), 'embedded_ticket_attachment', $(window).width(), $(window).height());
				}