using System;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using System.Net;
using System.Web;

public partial class spellchecker : System.Web.UI.Page
	{
    protected void Page_Load(object sender, EventArgs e)
		{
		Response.Clear();
		Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
		Response.Cache.SetValidUntilExpires(false); 
		Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
		Response.Cache.SetCacheability(HttpCacheability.NoCache);
		Response.Cache.SetNoStore();
		Response.ContentType				= "text/xml";
		Response.Write("");
		Response.End();
		var _q				= Request.QueryString;
		if(	_q["text"] != null)
			{
			var req_body					= string.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?><spellrequest><text>{0}</text></spellrequest>", _q["text"]);
			var uri							= new Uri("https://www.google.com/tbproxy/spell?lang=en");
			var curler			= (HttpWebRequest) WebRequest.Create(uri);
			var data						= Encoding.UTF8.GetBytes(req_body);
			curler.Method					= "POST";
			curler.ContentType				= "application/x-www-form-urlencoded";
			curler.ContentLength			= data.Length;
			var curler_stream			= curler.GetRequestStream();
			curler_stream.Write(data, 0, data.Length);
			curler_stream.Close();
			var response		= (HttpWebResponse)curler.GetResponse();
			var response_stream			= response.GetResponseStream();
			var read_stream		= new StreamReader(response_stream, Encoding.UTF8);
			var curler_string			= read_stream.ReadToEnd();
			read_stream.Close();
			Response.Write(curler_string);
			}
		else
			{
			Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<spellresult error=\"1\"/>");
			}
		Response.End();
		}
	}
	