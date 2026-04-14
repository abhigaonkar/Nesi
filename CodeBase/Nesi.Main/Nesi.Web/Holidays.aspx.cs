using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class Holidays : Page
	{
	NeMember current_user;
	bool is_admin			= false;
	private const int _page_id = 40;
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user			= Toolbox.do_handle_authentication(_page_id);
		var menu			= new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml	= menu.MenuHTML;
		divSide.InnerHtml	= shared.PrintSidePanelHTML(current_user);
		var lbltemp		= (Label) Page.Master.FindControl("lblHeading");
		if (menu.PageDescription != null)
			{
			lbltemp.Text = menu.PageDescription;
			}
		is_admin			= current_user.AuthenticatedForPrivilege(161);
		gv_holidays.Settings.ShowTitlePanel		= is_admin;
		gv_holidays.Columns["Action"].Visible	= is_admin;
		}

	protected void HolidaysGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
		}

	protected void lbAddHoliday_Click(object sender, EventArgs e)
		{
		}
	protected void bt_save_Click(object sender, EventArgs e)
		{
		var name				= (ASPxTextBox) gv_holidays.FindTitleTemplateControl("name");
		var date				= (ASPxDateEdit) gv_holidays.FindTitleTemplateControl("date");
		var cb_canadian			= (ASPxCheckBox) gv_holidays.FindTitleTemplateControl("cb_canadian");
		var cb_american			= (ASPxCheckBox) gv_holidays.FindTitleTemplateControl("cb_american");
		var holidays		= new NeHolidays();
		holidays.name			= name.Text;
		holidays.date			= date.Text;
		holidays.is_canada		= cb_canadian.Checked ? 1 : 0;
		holidays.is_america		= cb_american.Checked ? 1 : 0;
		holidays.create_id		= current_user.id;
		holidays.audit_id		= current_user.id;
		holidays.save();
		name.Text				= "";
		date.Text				= "";
		cb_american.Checked		= false;
		cb_canadian.Checked		= false;
		gv_holidays.DataBind();
		}
	protected void gv_holidays_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		int active_id			= Convert.ToInt16(e.Keys[0]);
		if(active_id > 0)
			{
			var holidays		= new NeHolidays(active_id);
			holidays.audit_id		= current_user.id;
			holidays.is_active		= 0;
			holidays.save();
			}
		gv_holidays.DataBind();
		e.Cancel			= true;
		}
}
