using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using nesi.core;

public partial class sections_admin_appCommandCenter_index : System.Web.UI.Page
{

    Toolbox _tools;
    private NeMember current_user;
    private const int _page_id = 202;

    protected void Page_Load(object sender, EventArgs e)
    {
        _tools = new Toolbox();
        current_user = Toolbox.do_handle_authentication(_page_id);

        var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
        var lbltemp = (Label)Page.Master.FindControl("lblHeading");
        lbltemp.Text = "NESI App Command Center";


        var topics_dt = Toolbox.doSQL_dt(@"SELECT 'global' as topic, 'ldap' as topic"  , null);

        var topic_lst = new List<string>();

        foreach (DataRow dr in topics_dt.Rows)
        {
            topic_lst.Add(dr["topic"].ToString());
        }
        topics.DataSource = topic_lst;
        topics.DataBind();
    }

}