function resizeIframe(obj) {
    obj.style.height = (obj.contentWindow.document.body.scrollHeight + 30) + 'px';

}

function lookup()
		{
		var o			= get($("#t").val());
		if(o != null)
			{
			var locations	= "";
			//for(var i = 0;i < o.l.length;i++)
			//	{
			//	var j		= o.l[i];
			//	locations	+= j.id+" - "+j.n+" - "+j.q+"<br/>";
			//	}
			$("#tt").html(o.d);
			}
		}
	function get(k)
		{
		var x			= JSON.parse(sessionStorage.getItem("inventory"));
		for (var i = 0; i < x.length; i++) 
			{
			if (x[i]["id"] == k)
				{
				return x[i];
				}
			}
		return null;
		}
	$(document).ready(function()
		{
		sessionStorage.removeItem("inventory");		
		if(sessionStorage.getItem("inventory") == null)
			{
			$.get("/mobile/index.aspx",
				{
				a: "persist",
				generate: "true"
				}, 
				function(ret)
					{
					//alert(ret.length*2);
					sessionStorage.setItem("inventory", ret);
					});
			}
			$('html').bind('keypress', function(e)
{
   if(e.keyCode == 13)
   {
      return false;
   }
});
		});