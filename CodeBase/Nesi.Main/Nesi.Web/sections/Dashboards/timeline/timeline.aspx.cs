using System;
using System.Data;
using System.Collections.Specialized;
using nesi.core;

public partial class sections_dashboards_timeline : System.Web.UI.Page
{
	
	NeMember current_user;
	private int page_id = 152;
	Toolbox _tools;
	public string xx = @"[new Date(2010, 7, 24, 16, 0, 0), , ""New Entry""]";

	public DataSet dt;
	protected NameValueCollection _q;
	
	protected void Page_Init()
	{
		_q = Request.QueryString;
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		_tools.dont_cache_page();
		
		if (Cache["ds_depend"] == null)
		{
			Cache["ds_depend"] = DateTime.Now;
		}

	}
	protected void Page_Load(object sender, EventArgs e)
	{
		
		if (!IsPostBack) 
		{

			timeline1.type = "Big Work Orders";
			
			timeline1.DataBind();

		

			


		}
		
	}

	
	

	
}
