using System;
using System.Collections.Specialized;
using nesi.core;

public partial class this_barcode : System.Web.UI.Page
	{
	NeMember _member;
	private const int _page_id								= 1; // from Page table in DB
    protected void Page_Load(object sender, EventArgs e)
		{
		Response.Clear();
		var _tools			= new Toolbox();
		_member					= Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();
		var _q	= Request.QueryString;
		if(!string.IsNullOrEmpty(_q["master_id"]))
			{
			var i			= new inventory();
			var business_unit_id		= Session["working_business_unit_id"] != null ? Convert.ToInt32(Session["working_business_unit_id"]) : _member.business_unit_id;
			i.Load(_q["master_id"], business_unit_id);
			try
				{
				i.print_barcode_label(1);
				Response.Write("SUCCESS");
				}
			catch (Exception ee)
				{
				Response.Write(ee.Message);
				}
			}
		Response.End();
		}
	}
