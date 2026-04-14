using System;
using nesi.core;

public partial class target : System.Web.UI.Page
{
    NeMember myMember;
    Toolbox _Tools = new Toolbox();
    int _page_id = 95;

    protected void Page_Load(object sender, EventArgs e)
        {
        SqlDataSource1.SelectCommand = "SELECT ID, ddl_name name FROM business_unit where id in (" + new Current_User().visible_business_units + ")";
        SqlDataSource1.DataBind();
        
            myMember = Toolbox.do_handle_authentication(_page_id);
        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);

        if (!IsPostBack)
        {
           

        }
    fill_grid();


    }
    protected void fill_grid()
    {
            var strsql = "Select * from target";
            ASPxGridView1.DataSource = _Tools.getSQL_datatable(strsql,null);
            ASPxGridView1.DataBind();
            ASPxGridView1.FilterEnabled = true;
    //        ASPxGridView1.FilterExpression = string.Format("[target_CompanyID]=" + myMember.business_unit_id + " or [target_CompanyID]=0");
    }

    
    protected void ASPxGridView1_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {


        //TODO: LL FIXING SQL Security

        var strsql = "Insert into target (target_target_type_id,target_StartDate,target_EndDate,target_active," +
						"business_unit_id,target_value) values " +
                        "(" + Convert.ToInt32(e.NewValues["target_target_type_id"]) + "," +
                        "'" + Convert.ToDateTime(e.NewValues["target_StartDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'" + Convert.ToDateTime(e.NewValues["target_EndDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                         Convert.ToInt32(e.NewValues["target_active"]) + "," +
                         Convert.ToInt32(e.NewValues["business_unit_id"]) + "," +
                         Convert.ToDouble(e.NewValues["target_value"]) + ")";
        _Tools.getSQL_void(strsql);
        e.Cancel = true;
        ASPxGridView1.CancelEdit();

        fill_grid();

    }
    protected void ASPxGridView1_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        int edittingkey = Convert.ToInt16(e.Keys[0]);
        var strsql = "Delete from target where target_id =@v0 ";
        _Tools.getSQL_void(strsql,
	        new object[] { edittingkey});
        e.Cancel = true;
        ASPxGridView1.CancelEdit();
        fill_grid();
    }
    protected void ASPxGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        //TODO: LL FIXING SQL Security
        int edittingkey = Convert.ToInt16(e.Keys[0]);
        var strsql = "Update target set " +
                        "target_target_type_id = " + Convert.ToInt32(e.NewValues["target_target_type_id"]) + "," +
                        "target_StartDate = '" + Convert.ToDateTime(e.NewValues["target_StartDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "target_EndDate = '" + Convert.ToDateTime(e.NewValues["target_EndDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "target_active = " + Convert.ToInt32(e.NewValues["target_active"]) + "," +
						"business_unit_id = " + Convert.ToInt32(e.NewValues["business_unit_id"]) + "," +
                        "target_value = " + Convert.ToDouble(e.NewValues["target_value"]) + " " +
                        "where target_id = " + edittingkey;
        _Tools.getSQL_void(strsql);
        e.Cancel = true;
        ASPxGridView1.CancelEdit();

        fill_grid();
    }
}
