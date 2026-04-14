function menu_show(obj)
	{
	var l			= $(obj).css("left");
	if(l == "0px")
		{
		$(".menu_toggle").css({"left":"260px"});
		$(".menu").css({"left":"0px"});
		//$(".logo").css({"margin-left":"auto"});
		}
	else
		{
		$(".menu_toggle").css({"left":"0px"});
		$(".menu").css({"left":"-260px"});
		//$(".logo").css({"margin-left":"-32px"});
		}
	}
function logoff()
	{
	location.href		= "index.aspx?a=logoff";
	}
function page(a)
	{
	location.href		= a == "" ? "index.aspx" : "index.aspx?a="+a;
	}					
$(document).bind('mousedown', function(e)
	{
	if(	!$(e.target).is('.menu')	&& 	!$(e.target).is('.menu_toggle') &&
		$(e.target).parents('.menu').length == 0
		)
		{
		$(".menu_toggle").css({"left":"0px"});
		$(".menu").css({"left":"-260px"});
		//$(".logo").css({"margin-left":"-32px"});
		}
	});
	
$(document).ready(function()
	{
	var supportsOrientationChange = "onorientationchange" in window,
    orientationEvent = supportsOrientationChange ? "orientationchange" : "resize";
	window.addEventListener(orientationEvent, orientate, false);
	orientate();
	});
function orientate()
	{
	var	supportsOrientationChange	= "onorientationchange" in window;
	if(supportsOrientationChange)
		{
		var orient_class	= window.orientation == 0 ? "orientation p" : "orientation l";
		$(".orientation").attr("class", orient_class);
		}
	else
		{
		$(".orientation").remove();
		}
	}