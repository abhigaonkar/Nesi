using System;
using System.Web;
using nesi.core;

public partial class sections_admin_errorMessageBoard_dev_errorLog : System.Web.UI.Page
{
    //private Toolbox _tools;
    NeMember current_user;
    protected void Page_Load(object sender, EventArgs e)
    {
       // _tools = new Toolbox();
        if (!HttpContext.Current.Request.UserHostAddress.Contains("72.14.168.138") && 
            !HttpContext.Current.Request.UserHostAddress.Contains("23.96.59.193") &&
            !HttpContext.Current.Request.UserHostAddress.Contains("184.149.11.18") &&
            !HttpContext.Current.Request.UserHostAddress.StartsWith("192.168") &&
            !HttpContext.Current.Request.IsLocal)
        {
            current_user = Toolbox.do_handle_authentication(1);

        }
        //crash_log1.auto_refresh_defult = true;
    }

   
}