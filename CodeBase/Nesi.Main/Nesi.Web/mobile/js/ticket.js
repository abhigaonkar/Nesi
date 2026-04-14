var submit = 0;
var mobile_ticket =
	{
		resize_frame:
			function () {
				//var height		= $("#if_ticket")[0].contentWindow.document.body.scrollHeight;
				//$("#if_ticket").css({"height":height+"px"});
			},
		unload:
			function () {
				$(".if_modal.existing").hide();
				$(".if_modal.new").hide();
				$("#if_newticket").attr("src", "");
				$("#if_existingticket").attr("src", "");

				var html = $('html');
				var scrollPosition = html.data('scroll-position');
				html.css('overflow', html.data('previous-overflow'));
				window.scrollTo(scrollPosition[0], scrollPosition[1])
			},
		remove_bg:
			function () {
				$("#if_newticket").css({ "background-image": "" });
				$("#if_existingticket").css({ "background-image": "" });
			},
		search:
			function (s, e) {
				if ($.trim(text_search.GetText()) == "") {
					alert("Please enter a search criteria");
				}
				else {
					ticket_cbp.PerformCallback("search|0");
				}
			},
		cancelsearch:
			function (s, e) {
				ticket_cbp.PerformCallback("cancelsearch|0");
			},
		load:
			function (id, new_window) {
				if (new_window)
					{
					window.open("/sections/member/tickets/ticketpage.aspx?issue=" + id + "&is_mobile=1");
					}
				else
					{
					location.href = "/sections/member/tickets/ticketpage.aspx?issue=" + id + "&is_mobile=1";
					}
			},
		new_ticket:
			{
			go:
				function()
					{
					location.href = "/mobile/index.aspx?a=new_ticket";
					},
			check:
				function(obj, e)
					{
					var t			= $(".ticket_body").val().trim();
					var l			= t.length;
					if($(".ticket_body").val().trim() == "" || l < 10)
						{
						alert("Please also fill out the ticket body");
						return false;
						}
					else
                    {
                        if (++submit > 1) {
                            return false;
                        }
                        return true;
						}
					}
			}
	}