<%@ webhandler language="C#" class="remote_barcode_print" %>

using System;
using System.Net;
using System.Web;
using System.Collections.Specialized;
using System.Collections.Generic;
using nesi.core;

public class remote_barcode_print : IHttpHandler
	{
	public void ProcessRequest(HttpContext context)
		{
		var _q			= context.Request.QueryString;
		context.Response.ContentType	= "text/plain";
		string[] allowed_ips			= {"::1", "", "10.0.0.5", "10.0.0.6", "192.168.0.117"};
		if(!string.IsNullOrEmpty(_q["r"]) && Toolbox.Contains(context.Request.UserHostAddress, allowed_ips))
			{
			var r						= Toolbox.do_value_from(_q["r"],false).Split('|');
			var master_id				= r[0];
			var business_unit_id				= r[1];
			var copies						= Convert.ToInt32(r[2]);
			var i						= new inventory();
			i.Load(master_id, business_unit_id);
			try
				{
				i.print_barcode_label(copies);
				context.Response.Write("SUCCESS");
				}
			catch(Exception ee)
				{
				context.Response.Write(ee.Message);
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