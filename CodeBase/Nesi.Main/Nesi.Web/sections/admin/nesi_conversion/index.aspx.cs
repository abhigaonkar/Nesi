using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class nesi_conversion : System.Web.UI.Page
{
	NeMember current_user;
	private const int _page_id					= 216; // from Page table in DB
	
    protected void Page_Load(object sender, EventArgs e)
		{
    	Response.Clear();
		var _tools								= new Toolbox();
		current_user								= Toolbox.do_handle_authentication(_page_id);
		var _q						= Request.QueryString;
        var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
      
       
		}

    
  
   
}
