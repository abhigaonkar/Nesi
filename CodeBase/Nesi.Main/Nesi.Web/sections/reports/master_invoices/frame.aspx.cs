using System;
using System.Web.UI.WebControls;
using nesi.core;

public partial class master_invoice_frame : System.Web.UI.Page
{
    NeMember myMember;
	static string _page_id = "113";
 

    protected void Page_Load(object sender, EventArgs e)
    {
       Response.Redirect("index.aspx");
    }
}
