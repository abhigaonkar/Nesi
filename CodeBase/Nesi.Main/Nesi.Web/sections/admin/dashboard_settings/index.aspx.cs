using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_admin_dashboard_settings_index : System.Web.UI.Page
{

    Toolbox _tools;
    private NeMember current_user;
    private const int _page_id = 200;

    protected void Page_Load(object sender, EventArgs e)
    {
        _tools = new Toolbox();
        current_user = Toolbox.do_handle_authentication(_page_id);

        var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
        var lbltemp = (Label)Page.Master.FindControl("lblHeading");
        lbltemp.Text = "Nesi IT Dashboard settings";
    }
    protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {

        if (!current_user.AuthenticatedForPrivilege(12))
        {
            throw new Exception("Sorry you do not have permission to Edit Vendors");
        }
        var gv = sender as ASPxGridView;
        if (e.Parameters != null)
        {
            var parameters = e.Parameters.Split('|');

            if (parameters[0] == "DELETE")
            {
                var key = gv.GetRowValues(Convert.ToInt32(parameters[1]), "id").ToString();

                Toolbox.doSQL_void(@"DELETE FROM it.dashboard_eventviewer_servers where id = @v0 limit 1",key );
                gv_settings.CancelEdit();
            }
            gv.DataBind();
        }
    }
    protected void gv_settings_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
     

		var key = gv_settings.GetRowValues(e.VisibleIndex, "id").ToString();
        Toolbox.doSQL_void(@"DELETE FROM it.dashboard_eventviewer_servers where id =@v0 limit 1",key);
        gv_settings.DataBind();

	}
    protected void gv_settings_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {

    }
}