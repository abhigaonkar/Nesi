var sig_obj			=
		{
		printed_name:
			{
			focus:
				function(_obj)
					{
					sig_obj.scroll_to_class(".printed_name");
					},
			blur:
				function(_obj)
					{
					sig_obj.scroll_reset();
					}
			},
		signee:
			{
			handle_check:
				function(_obj)
					{
					$(".printed_name").css({'display': !$(_obj).is(":checked") ? "none" : "block"});
					}
			},
		scroll_to_class:
			function(_c)
				{
				$("html,body").animate({scrollTop: $(_c).offset().top}, 100);
				},
		scroll_reset:
			function(_c)
				{
				$("html,body").animate({scrollTop: 0}, 100);
				},
		}
var save_contact	=
	{
		start:	function(s,e){please_wait("start");},
		stop:	function(s,e){please_wait("stop");},
		run: 
			function(_s,_e)
				{
				var name			= tb_name.GetText();
				var email			= tb_email.GetText();
				var name_check		= $.trim(name) == "";
				var email_check		= $.trim(email) == "";
				var contact_id		= $(".hid_contact_id").val();
				var error			= "";
				if(name_check && !email_check)
					{
					error			= "Please enter a name (both first & last)";
					}
				else if(!name_check && email_check)
					{
					error			= "Please enter an email address";
					}
				else if(name_check && email_check)
					{
					error			= "Please enter a name (both first & last) & email address";
					}
				else
					{
					if(contact_id == "")
						{
						cb_newcontact.PerformCallback(name+"|"+email);
						}
					else
						{
						cb_existingcontact.PerformCallback(name+"|"+email+"|"+contact_id);
						}
					}
				if(error != "")
					{
					alert(error);
					}
				},
		combo_contact_end_cb:
			function(_s, _e)
				{
				var contact_id		= $(".hid_contact_id").val();
				window.combo_contact.SetValue(contact_id);
				},
		success:
			function(s,e)
			{
				var contact_id	= e.result;
				var multiple_recipients	= combo_contact.GetValue() === 1;
				if(!multiple_recipients)
					{
					$(".hid_contact_id").val(contact_id);
					window.combo_contact.PerformCallback();
					}
				else
					{
					window.cbp_recipient_list.PerformCallback();
					}
				popup_newcontact.Hide();
				tb_name.SetText("");
				tb_email.SetText("");
				alert("Successfully saved contact");
			},
		select_last:
			function(s,e)
			{
				var c			= combo_contact.GetItemCount();
				combo_contact.SetSelectedIndex(c - 1);
			},
		handle_selection_change:
			function(_s,_e)
				{
				var show_printed		= _s.GetValue() === 0; 
				var multiple_recipients	= _s.GetValue() === 1; 
				var chk_diff_checked	= $("#chk_diff_signee").is(":checked");
				var chk_diff_hidden		= $(".chk_diff").is(":hidden");
				$(".printed_name").css({'display': show_printed || chk_diff_checked || multiple_recipients ? "block" : "none"}); 
				if(show_printed || multiple_recipients)
					{
					$(".printed_name").focus();
					$(".chk_diff").hide();
					$("#chk_diff_signee").attr("checked", false);
					if(multiple_recipients)
						{
						sig_obj.scroll_to_class(".multiple_recipients");
						}
					}
				else if(chk_diff_hidden)
					{
					$(".chk_diff").show();
					}
				if(!multiple_recipients)
					{
					recipient_list.UnselectAll();
					}
				recipient_list.SetClientVisible(multiple_recipients);
				if(multiple_recipients)
					{
					window.cbp_recipient_list.PerformCallback();
					}
				}
	}