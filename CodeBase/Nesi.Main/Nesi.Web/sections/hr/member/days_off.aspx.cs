using System;
using System.Collections.Specialized;
using nesi.core;

public partial class sections_hr_member_test : System.Web.UI.Page
	{
	NeMember current_user;
	Toolbox _tools;
	NameValueCollection _q;
	private const int _page_id = 127; // from Page table in DB
	DateTime default_start;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools						= new Toolbox();
		current_user				= Toolbox.do_handle_authentication(_page_id);
		_q							= Request.QueryString;
		}

	protected void Page_Load(object sender, EventArgs e)
		{
		if(!string.IsNullOrEmpty(_q["single"]))
			{
			uc_days_off.Visible = false;
			uc_days_off_new.Visible = true;
			uc_days_off_new.new_row_employee.Style["display"]	= "none";

			if(!IsPostBack)
				{//DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Date, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second
					var payperiodobj									= new NePayPeriod(NePayPeriod.CurrentOpenPayPeriodId());
				default_start										= new DateTime(payperiodobj.start_date.Year,payperiodobj.start_date.Month, payperiodobj.start_date.Day, 8, 0, 0);
				DateTime today_morning                              = new DateTime(DateTime.Now.Year,DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
			
				uc_days_off_new.new_combo_employee.SelectedIndex				= 0;
				uc_days_off_new.new_dte_start.MinDate							= default_start;
				uc_days_off_new.new_dte_start.Date 								= default_start <= today_morning ? today_morning : default_start;  // in case default start is tomorrow
				uc_days_off_new.new_dte_end.Date								= DateTime.Now;
				uc_days_off_new.new_memo_comments.Text							= "";
				uc_days_off_new.new_combo_dayoff_type.SelectedIndex				= 0;
				uc_days_off_new.new_combo_request_type.SelectedIndex			= 0;
				var user_fullname											= Toolbox.doSQL_string(@"SELECT IFNULL(MAX(member_fullname), 'ERROR')
FROM member WHERE member_id = @v0", _q["id"]);
				uc_days_off_new.new_row_foremployee_td.InnerHtml				= "<b>Creating a new 'day off' entry for: </b><b style='color:#f00;'>"+user_fullname+"</b><hr />";
				uc_days_off_new.new_row_foremployee_tr.Visible					= true;
				uc_days_off_new.new_bt_cancel.ClientSideEvents.Click			= @"function(s, e) { if(confirm('Are you sure you want to close this window?')) { window.close(); } }";
				uc_days_off_new.new_cb_new.ClientSideEvents.CallbackComplete	= @"function(s, e) { if(e.result == 'SUCCESS') { if(confirm('Successfully saved entry. Would you like to create another for this employee?')) { location.href = location.href; } else { window.close(); } } please_wait('stop');}";
				uc_days_off_new.new_ds_members_active.SelectCommand				= uc_days_off.this_ds_members_active.SelectCommand;
				uc_days_off_new.new_combo_request_type.Focus();
				}
			}
		}
	}