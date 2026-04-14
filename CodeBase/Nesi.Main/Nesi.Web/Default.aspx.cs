using nesi.core;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class _Default : NePage
{

	protected void Page_PreInit(object sender, EventArgs e)
	{
		
	}
	protected void Page_Init(object sender, EventArgs e)
	{
	    RedirectN2();
	}
	protected void Page_Load(object sender, EventArgs e)
	{

	  
	}

    private void RedirectN2()
    {
       //origin
        var q					= Request.QueryString;
        var origin = "";
        if (q["origin"] != null)
        {
            origin = Server.UrlEncode( q["origin"]);
            var redirectN2 = $@" top.document.location.href =""/#/signin?origin={origin}""";
            DoJS(redirectN2);
        }
        else
        {
            var redirectN2 = $@" top.document.location.href =""/#/signin""";
            DoJS(redirectN2);
        }

        
    }
    private void DoJS(string myJS)
    {
        Response.Write($"<script type='text/javascript'>{myJS}</script>");
    }
}
