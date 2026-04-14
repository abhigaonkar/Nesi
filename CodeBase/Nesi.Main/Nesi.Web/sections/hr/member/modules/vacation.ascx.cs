using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using nesi.core;

public partial class sections_hr_member_modules_vacation : System.Web.UI.UserControl
	{
	public NeMember admin {get; set;}
	public NeMember user {get; set;}
	protected void Page_Load(object sender, EventArgs e)
		{
		}
	public override void DataBind()
		{
		tb_current_holiday.Text									= user.Holiday.ToString();
		lb_dollarhours.Text										= user.Country == "USA" ? "Hours" : "Dollars";
		sds_requests.SelectParameters[0].DefaultValue			= user.id.ToString();
		sds_transactions.SelectParameters[0].DefaultValue		= user.id.ToString();
		gv_transactions.DataBind();
		gv_vacations.DataBind();
		}
	protected void cbp_override_holiday_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		using(var conn = Toolbox.connect())
			{
			double amount;
			if(memo_override.Text.Trim() == "")
				{
				throw new Exception("Please add a reason why you are overriding their current allotment");
				}
			double.TryParse(tb_current_holiday.Text, out amount);
			if(amount > 0)
				{
				var current				= user.Holiday;
				var holiday_delta		= amount - current;
				var this_user			= user;
				payroll.vacation.add_adjustment(conn, ref this_user, admin, memo_override.Text.Trim(), holiday_delta); 
				memo_override.Text		= "";
				gv_transactions.DataBind();
				gv_vacations.DataBind();
				}
			else
				{
				throw new Exception("Cannot add override for a negative amount");
				}
			}
		}
}