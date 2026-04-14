using System;
using System.Collections.Specialized;
using System.Web.UI;
using nesi.core;

public partial class sections_member_quote_recon_frame : Page
	{
	protected void Page_Load(object sender, EventArgs e)
		{
		var _q			= Request.QueryString;
		var _tools					= new Toolbox();
		var current_user			= Toolbox.do_handle_authentication(1);
		Toolbox.StyleReferenceManager.AddStyleLinksToHead(this);
		
		if(string.IsNullOrEmpty(_q["quote_id"]))
			{
			Response.Clear();
			Response.Write("Quote # not supplied");
			Response.End();
			}
	
		recon1.quote_id = Convert.ToInt32(_q["quote_id"]);
	
		}
	}
