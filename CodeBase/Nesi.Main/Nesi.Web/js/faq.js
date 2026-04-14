var faq		=	{
				expand_section:
					function(obj)
						{
						var _parent			= $(obj).parents(".g:first");
						var is_expanded		= false;
						if($(obj).attr('class') == "d")
							{
							is_expanded		= $(obj).next("a").attr("data-is_expanded") == "true";
							}
						else
							{
							is_expanded		= $(obj).attr("data-is_expanded") == "true";
							}
						if(is_expanded)
							{
							if($(obj).attr('class') == "d")
								{
								$(obj).text("[+]");
								$(obj).next("a").attr("data-is_expanded", "false");
								}
							else
								{
								$(obj).prev(".d").text("[+]");
								$(obj).attr("data-is_expanded", "false");
								}
							_parent.children(".a").each(function(){$(this).slideUp(100);});
							}
						else
							{
							if($(obj).attr('class') == "d")
								{
								$(obj).next("a").attr("data-is_expanded", "true");
								$(obj).text("[-]");
								}
							else
								{
								$(obj).attr("data-is_expanded", "true");
								$(obj).prev(".d").text("[-]");
								}
							_parent.children(".a").each(function(){$(this).slideDown(100);});
							}
						},
				view_article:
						{
						run:
							function(obj)
								{
								var id				= $(obj).attr('data-level');
								$(".r .head").text($(obj).text());
								$(".body").html("");
								please_wait("start", "Loading Article");
								PageMethods.get_article(id, this.complete, faq.error, faq.timeout);
								},
						complete:
							function(arg)
								{
								please_wait("stop");
								$(".body").html(arg);
								}
						},
				timeout: 
					function(arg)
						{
						alert("Timeout occured");
						},
				error: 
					function(arg)
						{
						alert("Error has occured: " + arg._message);
						}
				};