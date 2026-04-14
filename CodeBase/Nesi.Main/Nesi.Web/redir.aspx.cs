using nesi.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class redir : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var url = Request.QueryString["url"];
        if (string.IsNullOrEmpty(url))
        {
            Response.Redirect("index.html");
        }else
        {
            

                if (url.Contains("?"))
                {
                    url += "&is_n1=true";
                }
                else
                {
                    url += "?is_n1=true";
                }

                Response.Redirect("/#/home/0/" + Server.UrlEncode( url));
                // /home.aspx?is_n1
            



        }
    }
}
