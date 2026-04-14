using System;
using System.Web;
using nesi.core;

public partial class sections_admin_errorMessageBoard_it_dashboard : System.Web.UI.Page
{
    NeMember current_user;
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( !HttpContext.Current.Request.UserHostAddress.Contains("72.14.168.138") && 
             !HttpContext.Current.Request.UserHostAddress.Contains("23.96.59.193" ) &&
             !HttpContext.Current.Request.UserHostAddress.Contains("184.149.11.18") &&
             !HttpContext.Current.Request.UserHostAddress.StartsWith("192.168") &&
             !HttpContext.Current.Request.IsLocal )
        {
            current_user = Toolbox.do_handle_authentication(1);
        }
    }
}