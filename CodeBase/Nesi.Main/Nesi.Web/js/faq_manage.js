var faq_manage			=	{
							section:
								{
								show_edit:
									function(s,e)
										{
										setTimeout("$('.section_tb_name').focus()", 500);
										},
								show_new:
									function(s,e)
										{
										gv_sections.AddNewRow();
										$(".section_tb_name").val("");
										$(".section_error").text("");
										setTimeout("$('.section_tb_name').focus()", 500);
										},
								cancel:
									function()
										{
										gv_sections.CancelEdit();
										},
								catch_enter:
									function(obj, e)
										{
										var key			= e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
										if(key == 13)
											{
											$("#save_section").click();
											e.returnValue		= false;
											}
										},
								handle_callback:
									function(s,e)
										{
										if(s.cpIsEdit)
											{
											setTimeout("$('.section_tb_name').focus()", 350);
											}
										}
									,
								save:
									function()
										{
										var name		= $(".section_tb_name").val();
										if($.trim(name) != "")
											{
											gv_sections.UpdateEdit();
											}
										else
											{
											$(".section_error").text("Please enter a section name");
											}
										}
								},
							faq:
								{
								active_row:			-1,
								show_new:
									function(s,e)
										{
										gv_faq.AddNewRow();
										$(".faq_tb_subject").val("");
										$(".cb_visible input:checkbox").removeAttr("checked");
										},
								cancel:
									function()
										{
										gv_faq.CancelEdit();
										},
								catch_enter:
									function(obj, e)
										{
										var key			= e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
										if(key == 13)
											{
											e.returnValue		= false;
											}
										},
								row_click:
									function(s,e)
										{
										if (faq_manage.faq.active_row != e.visibleIndex)
											{
											faq_manage.faq.active_row		= e.visibleIndex;
											s.ExpandDetailRow(e.visibleIndex);
											}
										else
											{
											s.CollapseDetailRow(e.visibleIndex);
											faq_manage.faq.active_row		= -1;
											}
										},
								row_collapsing:
									function(s,e)
										{
										faq_manage.faq.active_row		= -1;
										},
								row_expanding:	
									function(s,e)
										{
										faq_manage.faq.active_row		= e.visibleIndex;
										},
								save:
									function()
										{
										$(".faq_error").html("");
										var _subject		= $(".faq_tb_subject").val();
										var _body			= html_body.GetHtml();
										if($.trim(_subject) == "" || $.trim(_body) == "")
											{
											var error = "";
											if($.trim(_body) == "")
												{
												error	+= "<div>Please supply the text of this FAQ article.</div>";
												}
											if($.trim(_subject) == "")
												{
												error	+= "<div>Please enter a subject</div>";
												}
											$(".faq_error").html(error);
											}
										else
											{
											gv_faq.UpdateEdit();
											gv_sections.Refresh();
											}
										}
								}
							};