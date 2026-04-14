<%@ WebHandler Language="C#" Class="quote_js" %>

using System.Web;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using nesi.core;

public class quote_js : IHttpHandler
	{
	// The intent of this file is to be a barrier between the javascript and the end user... 
	// I want us to have the ability to add comments and be able to manipulate javascript before it is received by the browser.
	public void ProcessRequest(HttpContext context)
		{
		var _tools				= new Toolbox();
		_tools.dont_cache_page();
		var _resp			= context.Response;
		var js					= @"";
		var js_sb			= new StringBuilder();
		var js_stream		= new StreamReader(HttpContext.Current.Server.MapPath("/js/")+"quote.js");
		while((js = js_stream.ReadLine()) != null)
			{
			js			= js.Trim();
			var re	= new Regex(@"//.+");
			js_sb.Append(re.Replace(js, " ")+"\n");
			}
		js_stream.Close();
		_resp.ContentType = "text/javascript";
		_resp.Write(js_sb.ToString());
		}

	public bool IsReusable
		{
		get
			{
			return false;
			}
		}

	}