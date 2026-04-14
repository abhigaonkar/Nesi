using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Data;
using nesi.core;

public partial class sections_hr_member_modules_days_off : System.Web.UI.UserControl
	{
	NeMember current_user;
	NameValueCollection _q;
	bool can_edit_all;
	private const int _page_id			= 127; // from Page table in DB
	private int _active_member_id = 0;
	public int active_member_id	 { get {return _active_member_id;} set {_active_member_id = value;} }
	Toolbox _tools;
	JavaScriptSerializer jSON;
	private bool is_branch_daysoff  = false;
	public SqlDataSource this_ds_members_active  {get {return this.ds_members_active; }}
	int active_payperiod_id			= 0;
	sections_hr_member_modules_days_off_new days_off_new;
	DateTime default_start;
	protected void Page_Init(object sender, EventArgs e)
		{
		current_user				= Toolbox.do_handle_authentication(_page_id);
		_q							= Request.QueryString;
		can_edit_all				= current_user.AuthenticatedForPrivilege(127);
		is_branch_daysoff			= current_user.AuthenticatedForPrivilege(166);
		
		days_off_new				= (sections_hr_member_modules_days_off_new) pop_new.FindControl("uc_days_off_new");
		

		active_payperiod_id			= NePayPeriod.CurrentOpenPayPeriodId();
		jSON						= new JavaScriptSerializer();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		fill_gv();
		if(!IsPostBack)
			{
			reset_new_entry();
			}
		}
	private void fill_gv()
		{
		var where_clause			= "1=1";
		if(active_member_id == 0 && !Request.RawUrl.Contains("member/index.aspx"))
			{
			if(string.IsNullOrEmpty(_q["id"]))
				{
				// Figure out if they should see EVERYONE - Privilege 127
				var reports_to		= Toolbox.doSQL_string(@"SELECT reports_to("+current_user.id+")");
				where_clause			= can_edit_all
											? string.Format(" {0} = {0} ", current_user.id)
											: is_branch_daysoff
												? string.Format(" b.business_unit_id = '{0}'", current_user.business_unit_id)
												: string.Format(" FIND_IN_SET(b.member_id, '{0}') ", reports_to);
				gv_days_off.Settings.ShowGroupPanel			= true;
				}
			else
				{
				gv_days_off.Columns["business_unit"].Visible		= false;
				gv_days_off.Columns["employee"].Visible		= false;
				int.TryParse(_q["id"], out _active_member_id);
				if(_active_member_id == 0)
					{
					this.Visible			= false;
					}
				else
					{
					var c_member			= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_id = @v0", _active_member_id);
					if(c_member == 0)
						{
						this.Visible			= false;
						}
					where_clause					= string.Format(" b.member_id = '{0}' ", _active_member_id);
					days_off_new.new_row_employee.Style["display"]	= "none";
					}
				}
			}
		else
			{
			gv_days_off.Columns["business_unit"].Visible		= false;
			gv_days_off.Columns["employee"].Visible		= false;
			if(_active_member_id == 0)
				{
				this.Visible			= false;
				}
			else
				{
				var c_member			= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_id = @v0", _active_member_id);
				if(c_member == 0)
					{
					this.Visible			= false;
					}
				where_clause					= string.Format(" b.member_id = '{0}' ", _active_member_id);
				days_off_new.new_row_employee.Style["display"]	= "none";
				}

			}
		ds_members.SelectCommand	= string.Format(@"
SELECT 
	b.member_id id,
	CONCAT('(', a.ddl_name, ') ', b.member_fullname) text
FROM
	member b
LEFT JOIN
	business_unit a ON
		b.business_unit_id = a.id
WHERE 
	{0}
ORDER BY
	a.name,
	b.member_lastname,
	b.member_firstname", where_clause);
		ds_members_active.SelectCommand	= string.Format(@"
SELECT 
	b.member_id id,
	CONCAT('(', a.ddl_name, ') ', b.member_fullname) text,
	a.name,
	b.member_firstname,
	b.member_lastname
FROM
	member b
LEFT JOIN
	business_unit a ON
		b.business_unit_id = a.id
WHERE 
	b.member_status = 'Active' AND
	{0}
ORDER BY
	name,
	member_lastname,
	member_firstname", where_clause);
		gv_days_off.DataSource		= Toolbox.doSQL_dt(string.Format(@"
SELECT 
	a.vacation_id id, 
	d.ddl_name business_unit,
	b.member_fullname employee,
	a.member_id member_id, 
	IFNULL(b.member_status, 'Not Active') member_status, 
	a.type_id,
	a.requesttype_id, 
	a.date_insert date_requested, 
	a.date_start,
	a.date_end,
	a.comments, 
	b.member_status status,
	e.member_fullname requestedby,
	c.type leavetype,
	f.type requesttype,
	a.payperiod_id,
	a.business_unit_id
FROM 
	vacation_master a 
INNER JOIN 
	member b on 
		a.member_id = b.member_id  
INNER JOIN
	vacation_type c ON 
		a.type_id = c.id 
INNER JOIN
	business_unit d ON 
		b.business_unit_id = d.id
INNER JOIN
	member e ON
		a.create_member_id = e.member_id
INNER JOIN
	(
	SELECT 1 id, 'Phone' type 
	UNION ALL
	SELECT 2 id, 'Online' type 
	UNION ALL
	SELECT 3 id, 'Verbal' type 
	UNION ALL
	SELECT 4 id, 'Form' type 
	) f ON a.requesttype_id = f.id
WHERE 
	a.type_id NOT IN (4,14) AND 
	{0}
ORDER BY
	a.date_insert DESC", where_clause),null);
		days_off_new.new_ds_members_active.SelectCommand	= ds_members_active.SelectCommand;
		gv_days_off.DataBind();
		}
	protected void gv_days_off_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
		{
		var dr				= gv_days_off.GetDataRow(e.VisibleIndex);
		if(dr != null)
			{
			var id					= gv_days_off.GetDataRow(e.VisibleIndex)["id"];
			var _payperiod_id		= gv_days_off.GetDataRow(e.VisibleIndex)["payperiod_id"];
			var this_payperiod_id	= 0;
			if(_payperiod_id != null)
				{
				int.TryParse(_payperiod_id.ToString(), out this_payperiod_id);
				}
			var _business_unit_id			= gv_days_off.GetDataRow(e.VisibleIndex)["business_unit_id"];
			var this_business_unit_id		= 0;
			if(_business_unit_id != null)
				{
				int.TryParse(_business_unit_id.ToString(), out this_business_unit_id);
				}
			if(id != null)
				{
				if(this_payperiod_id < active_payperiod_id && (e.ButtonType	== ColumnCommandButtonType.Edit || e.ButtonType == ColumnCommandButtonType.Delete) && ((is_branch_daysoff && current_user.business_unit_id == (int) this_business_unit_id) || can_edit_all) )
					{
					e.Visible				= false;
					}
				}
			}
		}
	private void reset_new_entry()
		{
		var payperiodobj									= new NePayPeriod(NePayPeriod.CurrentOpenPayPeriodId());
		default_start										= new DateTime(payperiodobj.start_date.Year,payperiodobj.start_date.Month, payperiodobj.start_date.Day, 8, 0, 0);
		days_off_new.new_combo_employee.SelectedItem		= null;
		days_off_new.new_dte_start.MinDate					= default_start;
		days_off_new.new_dte_start.Value					= null;
		days_off_new.new_dte_end.Value						= null;
		days_off_new.new_memo_comments.Text					= "";
		days_off_new.new_combo_dayoff_type.SelectedIndex	= -1;
		days_off_new.new_combo_request_type.SelectedIndex	= 1;
		}
	protected void pop_new_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
		{
		reset_new_entry();
		}
	protected void cb_edit_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		days_off_obj o;
		o						= jSON.Deserialize<days_off_obj>(e.Parameter);
		var dd		= new NeMemberDaysOff(o.id);
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
		var c			= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM vacation_master 
WHERE date_start BETWEEN @v0 AND @v1 AND member_id = @v2 AND vacation_id != @v3", new object[] { Toolbox.MySQL_shortdt(dt_start), Toolbox.MySQL_shortdt(dt_end), dd.member_id, dd.id});
		if(c > 0)
			{
			throw new Exception("There are already day(s) off for this user during the selected period(s) - OTHER THAN THIS REQUEST - please review existing requests before proceeding.");
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
	protected void gv_days_off_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		var combo_employee		= (ASPxComboBox) gv_days_off.FindEditFormTemplateControl("combo_employee");
		fill_gv();
		if(gv_days_off.IsEditing)
			{
//			combo_employee.ClientEnabled		= false;
			}
		}
	protected void gv_days_off_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var mdo		= new NeMemberDaysOff(e.Keys[0]);
		mdo.delete();
		fill_gv();
		e.Cancel = true;
		}
	protected void gv_days_off_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{

		e.Cancel = true;
		gv_days_off.CancelEdit();

	}
}