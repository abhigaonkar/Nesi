using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using nesi.core;

public partial class member_vacation_viewer : Page
	{
	NeMember current_user;
	private const int _page_id = 162; // from Page table in DB
	private const string _page_name = "vacation_viewer";
	private Toolbox _tools = new Toolbox();

	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
        hdn_mid.Value = current_user.id32.ToString();
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
				
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		    ddlbranch.DataSource = _tools.getSQL_datatable(@"Select id, ddl_name name from business_unit  where find_in_set(id,@v0) ", new object[] { new Current_User().visible_business_units });
		    ddlbranch.DataBind();
        if (!IsPostBack)
			{
				ddlbranch.Value = current_user.business_unit_id;
				
				cal.GoToToday();

			}
			else
			{
				if (ASPxCheckBox1.Checked)
				{
					cal.ResourceDataSourceID = "sqlmember0";
					cal.DataBind();
				}
				else
				{
					cal.ResourceDataSourceID = "sqlmember";
					cal.DataBind();
				}
				
			}

		
		}

	protected string GetDisplayText(DevExpress.Web.ASPxScheduler.TimelineDateHeaderTemplateContainer container)
	{
		if (container.Interval.Duration.Days > 2)
		{
			return (container.Interval.Start.ToString("MMMM dd") + "-" + container.Interval.End.AddDays(-1).ToString("dd"));
		}
		else
			return container.Interval.Start.ToString("dd");
	}
	
}