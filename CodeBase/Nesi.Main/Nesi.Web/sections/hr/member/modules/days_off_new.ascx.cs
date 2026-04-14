using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using NESI.Common.Models;
using nesi.core;

public partial class sections_hr_member_modules_days_off_new : System.Web.UI.UserControl
	{
	public ASPxComboBox new_combo_employee => combo_employee;
	public ASPxComboBox new_combo_dayoff_type => combo_dayoff_type;
	public ASPxComboBox new_combo_request_type => combo_request_type;
	public ASPxDateEdit new_dte_start => dte_start;
	public ASPxDateEdit new_dte_end => dte_end;
	public ASPxMemo new_memo_comments => memo_comments;
	public HtmlTableRow new_row_employee => row_employee;
	public SqlDataSource new_ds_members_active => ds_members_active;
	public HtmlTableRow new_row_foremployee_tr => row_foremployee_tr;
	public HtmlTableCell new_row_foremployee_td => row_foremployee_td;
	public ASPxButton new_bt_save => bt_save;
	public ASPxButton new_bt_cancel => bt_cancel;
	public ASPxCallback new_cb_new => cb_new;

	NeMember current_user;
	NameValueCollection _q;
	bool can_edit_all;
	JavaScriptSerializer jSON;
	protected void Page_Init(object sender, EventArgs e)
		{
		current_user				= Toolbox.do_handle_authentication(OpsPage.Employees);
		_q							= Request.QueryString;
		can_edit_all				= current_user.AuthenticatedForPrivilege(OpsPrivilege.CanEditAllEmployees);
		jSON						= new JavaScriptSerializer();
		}
	protected void cb_new_Callback(object source, CallbackEventArgs e)
		{
		days_off_obj o;
		o						= jSON.Deserialize<days_off_obj>(e.Parameter);
		var dd		= new NeMemberDaysOff();
		dd.member_id_audit		= current_user.id;
		dd.active				= true;
		dd.requested_by			= current_user.id;
		dd.date_requested		= Toolbox.MySQLNow_short();
		
		if(string.IsNullOrEmpty(_q["id"]) && o.employee == 0)
			{
			throw new Exception("Please select an employee");
			}
		else if(!string.IsNullOrEmpty(_q["id"]))
			{
			dd.member_id		= Convert.ToInt32(_q["id"]);
			var member		= new NeMember(dd.member_id);
			dd.business_unit_id		= member.business_unit_id;
			}
		else
			{
			dd.member_id		= (int) o.employee;
			var member		= new NeMember(dd.member_id);
			dd.business_unit_id		= member.business_unit_id;
			}

		var dt_start		= new DateTime();
		DateTime.TryParse(o.dt_start, out dt_start);
		var dt_end			= new DateTime();
		DateTime.TryParse(o.dt_end, out dt_end);
		if(dt_start.DayOfWeek == DayOfWeek.Saturday || dt_start.DayOfWeek == DayOfWeek.Sunday)
			{
			throw new Exception("Cannot have a start date be on a Saturday or Sunday");
			}
		if(dt_end.DayOfWeek == DayOfWeek.Saturday || dt_end.DayOfWeek == DayOfWeek.Sunday)
			{
			throw new Exception("Cannot have an end date be on a Saturday or Sunday");
			}
		if(dt_end.Hour<7 || dt_end.Hour>17 || dt_start.Hour<7 || dt_start.Hour>17)
			{
			throw new Exception("Please select a time between 7AM and 5PM");
			}
		if(dt_end.Date < dt_start.Date)
			{
			throw new Exception("The start date needs to be THE SAME or BEFORE the end date");
			}
		else
			{
			dd.date_requested_start		= Toolbox.MySQL_longdt(dt_start);
			dd.date_requested_end		= Toolbox.MySQL_longdt(dt_end);
			}
		// Check for days between time
		var c			= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM vacation_master WHERE date_start BETWEEN @v0 AND @v1 AND member_id = @v2 AND status IN (1,3,5) AND type_id = 4",
			new object[] { Toolbox.MySQL_shortdt(dt_start), Toolbox.MySQL_longdt(dt_end), dd.member_id});
		if(c > 0)
			{
			throw new Exception("There are already day(s) off for this user during the selected period(s), please review existing requests before proceeding.");
			}

		if(o.request_type == 0)
			{
			throw new Exception("Please select a request type");
			}
		else
			{
			dd.requesttype_id	= o.request_type;
			}
		if(o.leave_type == 0)
			{
			throw new Exception("Please select a leave type");
			}
		else
			{
			dd.leavetype_id		= o.leave_type;
			}
		// Adding validation for possibility of too many days selected
		if(dt_start.Year < DateTime.Now.Year - 1 || dt_end.Year < DateTime.Now.Year - 1)
			{
			throw new Exception("Please validate the selected dates.");
			}
		// Arbitrary validation, though need to contain excessive entries.
		if(Math.Abs(dt_start.Subtract(dt_end).TotalHours) > 1000)
			{
			throw new Exception("Too many selected hours.");
			}
		dd.status				= "New";
		dd.comments				= o.comments;
		dd.save();
		e.Result				= "SUCCESS";
		}
	private class days_off_obj
		{
		public int id			{ get; set; }
		public int employee		{ get; set; }
		public string dt_start  { get; set; }
		public string dt_end	{ get; set; }
		public int request_type	{ get; set; }
		public int leave_type	{ get; set; }
		public string comments	{ get; set; }
		}
}