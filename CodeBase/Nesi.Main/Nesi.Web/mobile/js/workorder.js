function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 30) + 'px';
  
}

	var mobile_wo		=	
		{
		toggle_sub:
			function(obj)
				{
				var next_sub			= $(obj).next(".sub");
				var is_next_visible		= next_sub.is(":visible");
				// Close all
				$(".sub").each(function()
					{
					$(this).hide();
					});
				if(!is_next_visible)
					{
					next_sub.show();
					}
				},
		cancel:
			function(s,e)
				{
				cbp.PerformCallback("cancel|0");
				},
		sell:
			function(s,e)
				{
				cbp.PerformCallback("sell|0");
				},
		addpart:
			function(_s, _e)
				{
				var id				= $("[id$='hdn_wo_id']").val();
				boing("/sections/member/inventory/shopping_cart.aspx?is_mobile=1&woprog_id="+id, "shopping_cart", 1024, 768);
				},
		reworks:
				{
				send:
					function(s,e)
						{
						if(memo_rework_reason.GetText().trim() == "")
							{
							alert("Please enter a reason WHY you are sending this work order to reworks");
							memo_rework_reason.Focus();
							}
						else
							{
							$("body").css({"overflow-y":"auto"});
							cbp.PerformCallback("reworks|0");
							}
						},
				show: 
					function()
						{
						$("body").css({"overflow-y":"hidden"});
						popup_reworks.Show();
						},
				hide:
					function(s,e)
						{
						$("body").css({"overflow-y":"auto"});
						popup_reworks.Hide();
						}
				},
		require:
			function(s,e)
				{
				cbp.PerformCallback("require|0");
				},
		signoff:
			function(s,e)
				{
				var id				= $("[id$='hdn_wo_id']").val();
				boing("/mobile/index.aspx?a=signoff&woprog_id="+id+"&from=wo", "preview", 1024, 768);
			},
		edit_po:
			function(s,e)
				{
				cbp.PerformCallback("edit_po|0");
				},
		commit:
			function(s,e)
				{
				cbp.PerformCallback("commit|0");
				},
		addnote:
			function(s,e)
				{
				if(new_note.GetText().trim() == "")
					{
					alert("Please add some text first.");
					}
				else
					{
					cbp_notes.PerformCallback("addnote|0");
					}
				},
		cbp:		{
				begin:
					function(s,e)
						{
						},
				end:
					function(s,e)
						{
						if(typeof s.cpremove_me !== 'undefined')
							{
							if(s.cpremove_me == 1)
								{
								setTimeout("$('#remove_me').fadeOut(100, function(){$(this).remove()});", 5000);
								delete s.cpremove_me;
								}
							}
						},
				error:
					function(s,e)
						{
						}
				},
		popup:		null,
		start_workorder: function()
			{
			mobile_wo.popup			= window.open('/sections/workorder/mobile_wo/index.aspx?id=0&is_mobile=1', 'new_wo', 'width=330,height=1000');
			},
		close_popup: function()
			{
			mobile_wo.popup.close();
			}
		};

		
	var fm		= 
		{
		navigate:
			function (_path)
				{
				window.cbp_fm.PerformCallback("navigate|"+_path);
				},
		download:
			function(_path)
				{
				$("#dl_frame").attr("src", "/mobile/index.aspx?a=download&path="+_path);
				},
		upload:
			{
			start:
			function()
				{
				var f	= window.up_file.GetText();
				if($.trim(f) === '')
					{
					alert('Please selected a file');
					}
				else
					{
					window.up_file.Upload();
					}
				}
			}
		};