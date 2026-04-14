using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class corporations : System.Web.UI.Page
{
	NeMember current_user;
	private const int _page_id					= 213; // from Page table in DB
	
    protected void Page_Load(object sender, EventArgs e)
		{
    	Response.Clear();
		var _tools								= new Toolbox();
		current_user								= Toolbox.do_handle_authentication(_page_id);
		var _q						= Request.QueryString;
        var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
      
       
		}

    
    protected void sql_companies_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
  /*      SqlDataSourceView due = (SqlDataSourceView)sender;
        due.
        GridViewDataItemTemplateContainer container = due.NamingContainer as GridViewDataItemTemplateContainer;
        object key = container.KeyValue;
        e.Command.Parameters[0].Value = key;
   */
    }
    protected void sql_companies_DataBinding(object sender, EventArgs e)
    {
      
            var due = (SqlDataSource)sender;
            var x = due.ID;
            if (x == "sql_company_group")
            {
                var g = "";
            }
            var container = due.NamingContainer as GridViewDataItemTemplateContainer;
            var key = container.KeyValue;
            if (key!=null && due != null)
            {
                due.SelectParameters.Add(new Parameter("id", DbType.Int32, key.ToString()));
                due.DeleteParameters.Add(new Parameter("id", DbType.Int32, key.ToString()));
                due.InsertParameters.Add(new Parameter("id", DbType.Int32, key.ToString()));
            }
     
    }

    protected void sql_company_group_DataBinding(object sender, EventArgs e)
    {
        var due = (SqlDataSource)sender;
        var x = due.ID;
        if (x == "sql_company_group")
        {
            var g = "";
        }
        var container = due.NamingContainer as GridViewDataItemTemplateContainer;
        var key = container.KeyValue;
        if (key != null && due != null )
        {
            due.SelectParameters.Add(new Parameter("id", DbType.Int32, key.ToString()));
            due.DeleteParameters.Add(new Parameter("id", DbType.Int32, key.ToString()));
            due.InsertParameters.Add(new Parameter("id", DbType.Int32, key.ToString()));
        }
    }

   
}
