using System;

public partial class sections_vendor_rfq_modules_menu : System.Web.UI.UserControl
	{
    protected void Page_Load(object sender, EventArgs e)
		{

		}
	protected void cbp_menu_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		Response.RedirectLocation		= "index.aspx?logoff";
		}
	}
