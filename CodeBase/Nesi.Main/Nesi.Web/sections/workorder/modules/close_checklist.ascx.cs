using System;


public partial class sections_workorder_modules_close_checklist : System.Web.UI.UserControl
{	
	public int woprog_id { get; set; }
    public int member_id { get; set; }
	protected void Page_Load(object sender, EventArgs e)
	{
		
	if(!Visible) return;
		
		
		if (!IsPostBack)
		{
            hdn_tasks_woid.Value = woprog_id.ToString();
            //Fill dropdownlist with all projects names
            
		}
hdn_member_id.Value = member_id.ToString();
        gv_checklist.DataBind();
	}

	
}