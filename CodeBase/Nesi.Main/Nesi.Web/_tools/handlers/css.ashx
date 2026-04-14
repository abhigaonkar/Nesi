<%@ webhandler language="C#" class="css" %>

using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

public class css : IHttpHandler
	{
	public void ProcessRequest(HttpContext context)
		{
		var _q							 = context.Request.QueryString;
		context.Response.ContentType	= "text/css";
		var filename					= "";

		switch(_q["p"])
			{
			case "1VDI": // 1st instance of VDI, VDI = Vendor Data Import
				filename				= "1VDI - vendor_data_import.css";
			break;
			}
		if(filename != "")
			{
			var root_path		= context.Server.MapPath("~/css/via_handler/");
			using (var r = new StreamReader(root_path+filename))
				{
				var css = r.ReadToEnd();
				context.Response.Write(css);
				}
			}
		}

	public bool IsReusable
		{
		get
			{
			return false;
			}
		}

	}