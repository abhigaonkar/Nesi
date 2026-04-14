using System;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.BLL.Pages.HomePage;

public partial class modules_slow_page : System.Web.UI.UserControl
	{
	NeMember current_user;
	private Toolbox _tools;

	
	protected void Page_Init(object sender, EventArgs e)
		{
            _tools = new Toolbox();
            
	
		}
	protected void Page_Load(object sender, EventArgs e)
	{
        if (this.Visible)
        {
            gv_slowpages.ClientVisible = true;
            gv_slowpages.DataSource = new HomePageBase(null).GetSlowPage();
            gv_slowpages.DataBind();
        }
	}
	

}