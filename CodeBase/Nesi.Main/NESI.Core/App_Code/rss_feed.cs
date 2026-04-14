using System;
using System.Web;
using System.Data;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for rss_feed
	/// </summary>
	public class rss_feed
		{
		public int id { get; set; }
		public int table_id { get; set; }
		public string table_name { get; set; }
		public string item_title { get; set; }
		public DateTime date_created { get; set; }
		public DateTime date_expires { get; set; }
		public string item_text {get; set;}
		public string link {get; set;}
		public rss_feed(){}
		public rss_feed(int _id)
			{
			id		= _id;
			load();
			}

		public void load()
			{
			try
				{
				var rss		= Toolbox.doSQL_dt(@"SELECT * FROM rss_feed WHERE id = @v0  LIMIT 1", new object[] {  id } ).Rows[0];
				table_id		= (int) rss["table_id"];
				table_name		= (string) rss["table_name"];
				item_title		= (string) rss["item_text"];
				date_created	= (DateTime) rss["date_created"];
				date_expires	= (DateTime) rss["date_expires"];
				item_text		= (string) rss["item_text"];
				link			= (string) rss["link"];
				}
			catch
				{
				throw new Exception("RSS id not set to an existing row id");
				}
			}
		public void save()
			{
			//	date_expires		= date_expires == null ? DateTime.Now.AddDays(14) : date_expires; result of this evaluation is always false, and date_expires gets reassigned itself

			// Update
			Toolbox.doSQL_void(@"
UPDATE 
	rss_feed 
SET 
	table_name = @v0, 
	table_id = @v1, 
	item_title = @v2, 
	date_expires = @v3, 
	item_text = @v4,
	link = @v6
	WHERE 
id = @v5
LIMIT 1", new object[] {
				table_name,							// {0}
				table_id,							// {1}
				Toolbox.do_value_to(item_title),	// {2}
				Toolbox.MySQL_longdt(date_expires),	// {3}
				Toolbox.do_value_to(item_text),		// {4}
				id,									// {5}
				link								// {6}
			});
			

			//This code was never executed
			/*
        else
			{
			// Insert
			id		= Toolbox.doSQL_return_id(@" INSERT INTO rss_feed ( table_name, table_id, item_title, date_created, date_expires, item_text, link ) VALUES ( @v0 , @v1 , @v2 , NOW(), @v3 , @v4 , @v5  )", new object[] {  table_name, table_id, Toolbox.do_value_to(item_title), Toolbox.MySQL_longdt(date_expires), Toolbox.do_value_to(item_text), link  } );
			}
        */
			}
		public void feed(string _table, int _id)
			{
			var rss_title		= "";
			var rss_description	= "";
			var rss_link			= "";
			var rss_build_date	= DateTime.Now.ToString("r");
			var rss_feed_type	= "";
			switch(_table)
				{
					case "member":
						rss_feed_type		= "User";
						var _user		= new NeMember(_id);
						rss_title			= "Newsfeed for "+_user.FullName;
						rss_description		= "This is a news feed of items pertaining to "+_user.FullName;
						rss_link			= Toolbox.app_setting("Domain");
						break;
					case "company":
						rss_feed_type		= "Branch";
						var _branch	= new NeBusinessUnit(_id);
						rss_title			= "Newsfeed for "+_branch.name;
						rss_description		= "This is a news feed of items pertaining to "+_branch.name;
						rss_link			= Toolbox.app_setting("Domain");
						break;
				}
			HttpResponse _response;
			if (HttpContext.Current != null)
				{
				_response			= HttpContext.Current.Response;
				}
			else
				{
				_response = new HttpResponse(null);
				}
			_response.Clear();
			_response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
			_response.Cache.SetValidUntilExpires(false);
			_response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
			_response.Cache.SetCacheability(HttpCacheability.NoCache);
			_response.Cache.SetNoStore();
			_response.ContentType			= "text/xml";
			_response.Write(string.Format(@"<?xml version=""1.0"" encoding=""UTF-8"" ?>
<rss version=""2.0"">
	<channel>
		<title>{0}</title>
		<description>{1}</description>
		<link>{2}</link>
		<lastBuildDate>{3}</lastBuildDate>
		<pubDate>{3}</pubDate>
		<language>en-us</language>
", rss_title, rss_description, rss_link, rss_build_date));
			var _feed			= "";
			var _dt			= Toolbox.doSQL_dt(@"SELECT item_title, item_text, link FROM rss_feed WHERE table_name = @v0  AND table_id = @v1  AND date_expires > NOW()", new object[] {  _table, _id } );
			if(_dt.Rows.Count == 0)
				{
				_feed				= string.Format(@"
		<item>
			<title>No available items available</title>
			<link>{2}</link>
			<guid>{2}</guid>
			<pubDate>{0}</pubDate>
			<description>There were no available items for this {1}.</description>
		</item>", rss_build_date, rss_feed_type, Toolbox.app_setting("Domain"));
				}
			else
				{
				foreach(DataRow _dr in _dt.Rows)
					{
					var _title	= Toolbox.do_value_from(_dr["item_title"], true);
					var _desc	= Toolbox.do_value_from(_dr["item_text"], true);
					var _link	= _dr["link"].ToString();
					_feed				+= string.Format(@"
		<item>
			<title>{0}</title>
			<link>{3}</link>
			<guid>{4}</guid>
			<pubDate>{1}</pubDate>
			<description>{2}</description>
		</item>", _title, _desc, rss_build_date, _link, Toolbox.app_setting("Domain"));
					}
				}
			_feed					+= @"
	</channel>
</rss>";
			_response.Write(_feed);
			_response.End();
			}
		}
	}