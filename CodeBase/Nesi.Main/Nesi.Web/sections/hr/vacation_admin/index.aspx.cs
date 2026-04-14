using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Specialized;
using nesi.core;
using DevExpress.Web;
using System.Linq;
using MySql.Data.MySqlClient;

public partial class VacationAdmin : System.Web.UI.Page
	{
	public NeMember myMember;
	private const int _page_id			= 5; // from Page table in DB
	private const string _page_description	= "Vacation Administration";
    string _page_name = "vacation_admin";
    NameValueCollection _q;
//	Toolbox _tools;
    ASPxDropDownEdit _dde_filter;
    Panel _panel_export;
    static string _default_filter = "";
    SqlDataSource _ds_templates;
    ASPxHiddenField _h;
	Vacation_Admin this_vacation_admin;

    protected void Page_Init(object sender, EventArgs e)
		{
	//	_tools							= new Toolbox();
		_q								= Request.QueryString;
		myMember						= Toolbox.do_handle_authentication(5);
        this_vacation_admin				= new Vacation_Admin();
		this_vacation_admin._user			= myMember;
        lc.__page_name = _page_name;
        lc.used_gv = gv;
        _h = (ASPxHiddenField)lc.FindControl("h");
        _ds_templates = (SqlDataSource)lc.FindControl("ds_templates");
        _dde_filter = (ASPxDropDownEdit)lc.FindControl("dde_filter");
        _panel_export = (Panel)lc.FindControl("panel_export");
        _panel_export.Visible = true;
        Toolbox.do_dont_cache_page(this.Response);
        _h.Set("gridview_id", "gv");
        _ds_templates.SelectParameters["@page_name"].DefaultValue = string.Format("{0}", _page_name);
        _ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();

    }
    protected void Page_Load(object sender, EventArgs e)
		{
		var menu							= new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml					= menu.MenuHTML;
		divSide.InnerHtml					= shared.PrintSidePanelHTML(myMember);
		var lbltemp						= (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text						= _page_description;
		var can_see_wage					= myMember.AuthenticatedForPrivilege(11);
        _h.Set("gridview_id", "gv");
		var Output						= "";
		var Output_sb				= new StringBuilder();
		string type_of;
		string vacation_id;
		string full_name;
		string date_start;
		string date_end	;
        string date_insert;
        string date_return;
		string hours_requested;
		string note_date;
		string payperiod_id;
		string payment_method;
		decimal payment_amount;
		string note;
		DataTable _dt;
		DataTable _subdt;

        if (Request.QueryString["a"] != null)
			{
			switch(Request.QueryString["a"])
				{
				#region process
				case "process":
					Response.Clear();
					type_of							= Request.QueryString["type_of"];
					vacation_id						= Request.QueryString["vacation_id"];
					if(vacation_id.Contains(","))
						{
						var processed				= true;
						this_vacation_admin.vacation_id = vacation_id;
						foreach(var _id in vacation_id.Split(','))
							{
							if(!this_vacation_admin.Request_Process(_id, type_of))
								{
								processed			= false;
								}
							}
						if(processed)
							{
                            VacationRequest vr = new VacationRequest(vacation_id);
							vr.email_requester(myMember, vacation_id); 

                            Response.Write("SUCCESS");
							}
						else
							{
							Response.Write("FAILED");
							}
						}
					else
						{
						if(this_vacation_admin.Request_Process(vacation_id, type_of))
							{
                            VacationRequest vr = new VacationRequest(vacation_id);
                            vr.email_requester(myMember, vacation_id);
							Response.Write("SUCCESS");
							  
                        }
						else
							{
							Response.Write("FAILED");
							}
						}
					Response.End();
				break;
				#endregion process
				#region note-add
				case "note-add":		Response.Clear();
										this_vacation_admin.member_id		= myMember.id.ToString();
										vacation_id							= Request.QueryString["vacation_id"];
										this_vacation_admin.vacation_id		= vacation_id;
										note								= Request.QueryString["note"];
										if(this_vacation_admin.Request_Note_Add(note))
											{
											Response.Write("SUCCESS");
											}
										else
											{
											Response.Write("FAILED");
											}
										Response.End();
										
				break;
				#endregion note-add
				#region json_req
				case "json_req":		Response.Clear();
										var this_dt			= Toolbox.doSQL_dt(@" SELECT a.vacation_id id, CONCAT(b.member_firstname,' ',b.member_lastname) member_name, c.name, a.date_start start, a.date_end end FROM vacation_master a LEFT JOIN member b ON a.member_id = b.member_id LEFT join business_unit c ON b.business_unit_id = c.id  WHERE c.business_unit_id =@v0 AND b.member_status = 'Active' AND a.status = 3", new object[] { myMember.business_unit_id });
										Output						= "[";
										var i						= 0;
										foreach(DataRow dr in this_dt.Rows)
											{
											var this_id			= dr["id"].ToString();
											var start			= dr["start"].ToString();
											var end				= dr["end"].ToString();
											var member_name		= dr["member_name"].ToString();
											var name		= dr["name"].ToString();
											Output					+= string.Format(@"
{{
""id"":{0},
""title"":""{1} off ({2})"",
""start"":""{3}"",
""end"":""{4}"",
""url"":"".\/index.aspx?a=edit-request&vacation_id={0}""
}}", this_id, member_name, name, start, end);
											i++;
											if(i != this_dt.Rows.Count)
												{
												Output += ",";
												}
											}
										Output						+= "]";
										Response.Write(Output);
										Response.End();
				break;
				#endregion json_req
				#region email-process
				case "email-process":	Response.Clear();
										type_of			= Request.QueryString["type_of"];
										vacation_id		= Request.QueryString["vacation_id"];
										if(vacation_id.Contains(","))
											{
											var processed				= true;
											this_vacation_admin.vacation_id = vacation_id;
											foreach(var _id in vacation_id.Split(','))
												{
												if(!this_vacation_admin.Request_Process(_id, type_of))
													{
													processed			= false;
													}
												}
											if(processed)
												{
												Response.Write(@"
<script>
	alert('Request processed');
	location.href	= '/sections/hr/vacation_admin/index.aspx';
</script>");
												}
											else
												{
												Response.Write("There was a problem updating the vacation request. Please talk to Matt about this.");
												}
											}
										else
											{
											if(this_vacation_admin.Request_Process(vacation_id, type_of))
												{
												Response.Write(@"
<script>
	alert('Request processed');
	location.href	= '/sections/hr/vacation_admin/index.aspx';
</script>");
												}
											else
												{
												Response.Write("There was a problem updating the vacation request. Please talk to Matt about this.");
												}
											}
										Response.End();
				break;
				#endregion email-process
				#region edit-request
				case "edit-request":	
					vacation_id							= Request.QueryString["vacation_id"];
					this_vacation_admin.vacation_id		= vacation_id;
					this_vacation_admin.Set_Member_id();
					var available_hours				= Convert.ToDecimal(this_vacation_admin.Available_Hours());
					_dt									= this_vacation_admin.Request_Get();
					if(_dt.Rows.Count > 0)
						{
						foreach(DataRow dr in _dt.Rows)
							{
							date_start						= dr["date_start"].ToString();
							date_end						= dr["date_end"].ToString();
							date_return						= dr["date_return"].ToString();
							hours_requested					= dr["hours_requested"].ToString();
							payment_amount					= Convert.ToDecimal(dr["payment_amount"]);
							payperiod_id					= dr["payperiod_id"].ToString();
							var payperiod_select			= this_vacation_admin.PayPeriodBuilder(payperiod_id);
							//available_hours					= available_hours;
							payment_method					= dr["payment_method"].ToString();
							//string separate_check			= dr["separate_check"].ToString() == "1" ? "checked" : "";
							var Payment_Methods_Select	= "";
							var Payment_Methods		= this_vacation_admin.Payment_Methods();

							if(Payment_Methods.Rows.Count > 0)
								{
								var display			= "";
								Payment_Methods_Select		= @"
				<select id='payment_method' onchange='payment_amount_toggle();check_request();'>";
								var payout_type			= payment_method == "2" ? "hr(s)" : "";

								foreach(DataRow sub_dr in Payment_Methods.Rows)
									{	
									var method_id		= sub_dr["method_id"].ToString();
									var method			= (string) sub_dr["method"];
									var selected			= method_id == payment_method ? " selected" : "";
									Payment_Methods_Select	+= string.Format(@"
					<option value='{0}' {2}>{1}", method_id, method, selected);
									}
								display = payment_method != "2" ? " style='display:none'" : "";
								var total_amount = payment_amount != 0
									               ? payment_amount*this_vacation_admin.UserPayRate()
									               : 0;
								Payment_Methods_Select		+= string.Format(@"
				</select> 
<input type='text' id='amount' size='1' onfocus='this.select();check_request();' onblur='check_request();' value='{1}' onkeyup='check_amount();check_request();' {0}> <b id='amount_type'>{2}</b>", display, payment_amount, payout_type);
								}
							else
								{
								Payment_Methods_Select		= "Could Not Get Payment Methods.";
								}


							Output_sb.AppendFormat(@"
				<input type='hidden' id='available_vacation' value='{4}'>
				<input type='hidden' id='vacation_id' value='{6}'>
				<input type='hidden' id='pay_rate' value='{8}'>
<script>
	$(document).ready(function(){{
$('#start_date').datepicker({{dateFormat: 'yy-mm-dd',beforeShowDay: $.datepicker.noWeekends }});
$('#end_date').datepicker({{dateFormat: 'yy-mm-dd',beforeShowDay: $.datepicker.noWeekends }});
$('#return_date').datepicker({{dateFormat: 'yy-mm-dd',beforeShowDay: $.datepicker.noWeekends }});
}});
</script>
				<table cellpadding ='2' cellspacing='0' class='schedule_vacation'>
					<tr>
						<td style='font-family:arial;color:red;font-size:20px;'>{11}</td>
						<td></td>
					</tr>					
					<tr>
						<td class='c'>Start Vacation:</td>
						<td class='v'><input type='text' class='date' value='{1}' id='start_date' onfocus=""check_request();"" onkeyup='check_request();' onblur='check_request();'><div class='format'>YYYY-MM-DD</div></td>
					</tr>
					<tr>
						<td class='c'>End Vacation:</td>
						<td class='v'><input type='text' class='date' value='{2}' id='end_date' onfocus=""check_request();"" onkeyup='check_request();' onblur='check_request();'><div class='format'>YYYY-MM-DD</div></td>
					</tr>
					<tr>
						<td class='c'>Returning to Work:</td>
						<td class='v'><input type='text' class='date' value='{3}' id='return_date' onfocus=""check_request();"" onkeyup='check_request();' onblur='check_request();'><div class='format'>YYYY-MM-DD</div></td>
					</tr>
					<tr>
						<td class='c'>Pay Period:</td>
						<td class='v'>{9}</td>
					<tr>
						<td class='c'>Total # of working hours:</td>
						<td class='v'><input type='text' class='hours' id='total_hours' value='{5}' onfocus='check_request();' onkeyup='check_request();' onblur='check_request();'></td>
					</tr>
					<tr>
						<td class='c' valign='top'>Type of Pay:</td>
						<td class='v'>{0}<div class='report'>({7}) Additional vacation hour(s) available<br/></td>
					</tr>
					<tr>
						<td colspan='2' align='center' class='button'><button id='request_vacation' type='button' onclick='update_request()' disabled>UPDATE REQUEST</button><hr style='margin-top:15px;'/></td>
					</tr>
					<tr>
						<td class='c' valign='top'>Past Notes:</td>
						<td class='v'>
							<div id='past_notes'>
				", Payment_Methods_Select, date_start, date_end, date_return, (available_hours + Convert.ToDecimal(payment_amount)), hours_requested, vacation_id, available_hours.ToString("N2"), this_vacation_admin.UserPayRate().ToString("N2"), payperiod_select, 0,new NeMember(Convert.ToInt32(this_vacation_admin.member_id)).FullName);
						
					_subdt						= this_vacation_admin.Past_Notes();
					if(_subdt.Rows.Count > 0)
						{
						foreach(DataRow _subdr in _subdt.Rows)
							{
							note				= _subdr["note"].ToString();
							full_name			= _subdr["name_full"].ToString();
							note_date			= _subdr["date"].ToString();
							Output_sb.AppendFormat(@"
							<div class='past_note'>
								<div class='name_plate'><b>{1}</b> on <i>{2}</i></div>
								<div class='note_text'>{0}</div>
							</div>", 
								note, 
								full_name, 
								note_date
								);
							}
						}
					else
						{
						Output_sb.Append("<i>No note history</i>");
						}	
												Output_sb.Append(@"
							</div>
						</td>
					</tr>
					<tr>
						<td class='c' valign='top'>Add Note:<br><div class='optional'>*Optional*</div></td>
						<td class='v'><textarea class='notes' id='notes' onkeyup='check_note();'></textarea></td>
					</tr>
					<tr>
						<td colspan='2' align='center' class='button'><button id='add_note' type='button' onclick='send_note()' disabled>ADD NOTE</button></td>
					</tr>
				</table>");
												}
											}
										else
											{
											Output_sb.Append("Failed to load request");
											}
				break;
				#endregion edit-request
				#region update-request
				case "update-request":			
						Response.Clear();
						try
							{
							vacation_id							= (string) Request.QueryString["vacation_id"];
							this_vacation_admin.vacation_id		= vacation_id;
							var start_date					= (string) Request.QueryString["start_date"];
							var end_date						= (string) Request.QueryString["end_date"];
							var return_date					= (string) Request.QueryString["return_date"];
							var total_hours					= (string) Request.QueryString["total_hours"];
							//string separate_check				= (string) Request.QueryString["separate_check"];
							payment_method						= (string) Request.QueryString["payment_method"];
							var amount						= (string) Request.QueryString["amount"];
							if(amount == "")
								{
								amount							= "0";
								}
							var notes						= (string) Request.QueryString["notes"];
							payperiod_id						= (string) Request.QueryString["payperiod_id"];
							this_vacation_admin.payperiod_id	= payperiod_id;
						//	this_vacation_admin.Request_Update(start_date, end_date, return_date, total_hours, payment_method, amount);

							if(amount != "" && payment_method != "1")
								{
								var hours					= Convert.ToDecimal(amount);
								this_vacation_admin.Deduct_Vacation(hours.ToString(), "A");
								}
							else
								{
								this_vacation_admin.Deduct_Vacation("0", "C");
								}

							Response.Write("SUCCESS");
							}
						catch (Exception ee)
							{
							Response.Write("FAILED"+ee);
							}
						Response.End();
				break;
				#endregion update-request
				}
			}
	
        #region load gv
        if (!IsCallback && !IsPostBack)
        {
            var gl = new NeGridLayouts(Convert.ToInt32(myMember.id), string.Format("{0}", _page_name));
            if (gl.GridLayoutID != 0)
            {
                gv.LoadClientLayout(gl.GridLayout_Layout);
                _h.Set("ID", gl.GridLayoutID);
                _h.Set("NAME", gl.GridLayout_Name);
                _dde_filter.Text = gl.GridLayout_Name;
            }
            else
            {
                gv.FilterExpression = "[status] = 'DENIED'";
                gl.GridLayout_Layout = gv.SaveClientLayout();
                gl.member_id = myMember.id;
                gl.GridLayout_Name = "Denied";
                gl.GridLayout_Gridid = string.Format("{0}", _page_name);
                gl.SaveGridLayout();

                gl = new NeGridLayouts();

                gv.FilterExpression = "[status] = 'APPROVED'";
                gl.GridLayout_Layout = gv.SaveClientLayout();
                gl.member_id = myMember.id;
                gl.GridLayout_Name = "Approved";
                gl.GridLayout_Gridid = string.Format("{0}", _page_name);
                gl.SaveGridLayout();

                gl = new NeGridLayouts();

                gv.FilterExpression = "[status] = 'PENDING'";
                gl.GridLayout_Layout = gv.SaveClientLayout();
                gl.member_id = myMember.id;
                gl.GridLayout_Name = "Pending";
                gl.is_default = true;
                gl.GridLayout_Gridid = string.Format("{0}", _page_name);
                gl.SaveGridLayout();

                _h.Set("ID", gl.GridLayoutID);
                _h.Set("NAME", gl.GridLayout_Name);
                _dde_filter.Text = gl.GridLayout_Name;
            }
        }

        #endregion
        fill_grid();

		} // Closes OnLoad Class

    private void fill_grid()
    {
        gv.DataSource = Toolbox.doSQL_dt(@"CALL ds_vacation_admin(@v0, @v1, @v2)", new object[] { myMember.id, myMember.AuthenticatedForPrivilege(165), new Current_User().visible_business_units });
        gv.DataBind();
    }

 protected void gv_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
    {
        if (sender == null || e == null || e.VisibleIndex < 0)
        {
            return;
        }

        var grid = (ASPxGridView)sender;
        var commentsColumn = grid.GetRowValues(e.VisibleIndex, "comments");
        var comments = "";
        if (commentsColumn != null)
        {
            comments = commentsColumn.ToString();
        }

        e.Cell.ToolTip = comments;
    }

    protected void gv_CustomButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCustomButtonEventArgs e)
    {
    if ((e.VisibleIndex >= 0)&&((e.ButtonID=="Approve")||(e.ButtonID == "Deny")))
        {
            e.Visible = DevExpress.Utils.DefaultBoolean.False;
            if (gv.GetRowValues(e.VisibleIndex, "completed").ToString() == "0" && gv.GetRowValues(e.VisibleIndex, "_ph_count").ToString() == "0")
            {
                e.Visible = DevExpress.Utils.DefaultBoolean.True;
            }
           
        }
    }

    protected void gv_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        var this_vacation_admin = new Vacation_Admin();
        this_vacation_admin._user = myMember;
        this_vacation_admin.vacation_id = gv.GetRowValues(e.VisibleIndex, "vacation_id").ToString();
        if (e.ButtonID == "Approve")
        {
            if (this_vacation_admin.Request_Process(this_vacation_admin.vacation_id, "3"))
            {
                VacationRequest vr = new VacationRequest(this_vacation_admin.vacation_id);
                vr.email_requester(myMember, this_vacation_admin.vacation_id);
            }
        }
        else if (e.ButtonID == "Deny")
        {
            if (this_vacation_admin.Request_Process(this_vacation_admin.vacation_id, "2"))
            {
                VacationRequest vr = new VacationRequest(this_vacation_admin.vacation_id);
                vr.email_requester(myMember, this_vacation_admin.vacation_id);
            }
        }


        fill_grid();

    }

    protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters != "")
        {
            gv.LoadClientLayout(e.Parameters);
        }
        else
        {
            gv.FilterExpression = "";
            for (var i = 0; i < gv.Columns.Count; i++)
            {
                var column = gv.Columns[i] as GridViewDataColumn;
                if (column != null)
                {
                    var col = column;
                    if (col.GroupIndex > -1)
                    {
                        gv.UnGroup(col);
                    }
                    col.Visible = true;
                }
            }
        }
    }

    protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        var gv = (ASPxGridView)sender;
        e.Properties["cpExp"] = gv.SaveClientLayout();
    }

    public class Vacation_Admin
    {
        string sql;
        public NeMember _user;

        public string vacation_id;
        public string member_id;
        public string payperiod_id;

      


        public bool Request_Process(string vacation_id, string type_of)
        {
            try
            {
                Toolbox.doSQL_void(@"UPDATE vacation_master SET status = @v0  WHERE vacation_id = @v1 ", new object[] { type_of, vacation_id });
                Toolbox.doSQL_void(string.Format(@"INSERT INTO vacation_log (dt, vacation_id, member_id, action) VALUES (now(), @v0 , @v1 , 'User updated status to {0}' )", type_of), new object[] { vacation_id, _user.id });
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool Request_Note_Add(string note)
        {
            if (note.Length == 0)
            {
                return true;
            }
            Toolbox.do_value_from(string.Format(@"
	INSERT INTO vacation_note (vacation_id,member_id,date_insert, note) VALUES
	(
	{0},
	{1},
	now(),
	""{2}""
	)
	", vacation_id, member_id, note));
            return true;
        }
        public DataTable all_future_vacations()
        {
            return Toolbox.doSQL_dt(@" SELECT a.vacation_id, full_name, DATE_FORMAT(date_start, '%Y-%m-%d') date_start, DATE_FORMAT(date_end, '%Y-%m-%d') date_end, A.member_id, a.date_insert, a.comments, a.payment_method, business_unit.ddl_name, a.status, a.date_return, b.completed FROM vacation a left join business_unit on A.business_unit_id = business_unit.id  LEFT JOIN payperiods b on a.payperiod_id = b.payperiodid WHERE b.completed = false AND a.status_id in (1,3) AND a.type_id = 4 AND a.date_start IS NOT NULL and date_start>(curdate()-interval 1 day) and find_in_set(business_unit_id,@v0 ) ORDER BY a.date_start", new object[] { new Current_User().visible_business_units }); ;
        }
		

        public DataTable Upcoming_Requests(string business_unit_id)
        {
            return Toolbox.doSQL_dt(@" SELECT vacation_id, concat(full_name,'-',business_unit.ddl_name) full_name, DATE_FORMAT(date_start, '%Y-%m-%d') date_start, DATE_FORMAT(date_end, '%Y-%m-%d') date_end, DATE_FORMAT(date_return, '%Y-%m-%d') date_return, amount, payment_method, vacation.member_id, date_insert FROM vacation left join business_unit on vacation.business_unit_id = business_unit.id WHERE status_id = 1 AND type_id = 4 AND date_start IS NOT NULL and find_in_set(business_unit_id,@v0 )", new object[] { new Current_User().visible_business_units }); ;
        }
        public DataTable Upcoming_Vacations(string business_unit_id)
        {
            return Toolbox.doSQL_dt(@" SELECT a.vacation_id, full_name, DATE_FORMAT(date_start, '%Y-%m-%d') date_start, DATE_FORMAT(date_end, '%Y-%m-%d') date_end, A.member_id, a.date_insert, a.comments, a.payment_method, business_unit.ddl_name, a.status, a.date_return FROM vacation a left join business_unit on A.business_unit_id = business_unit.id  LEFT JOIN payperiods b on a.payperiod_id = b.payperiodid WHERE b.completed = false AND a.status_id = 3 AND a.type_id = 4 AND a.date_start IS NOT NULL and date_start>(curdate()-interval 1 day) and find_in_set(business_unit_id,@v0 ) ORDER BY a.date_start", new object[] { new Current_User().visible_business_units }); ;
        }
        public DataTable Request_Get()
        {
            return Toolbox.doSQL_dt(@" SELECT DATE_FORMAT(date_start, '%Y-%m-%d') date_start, DATE_FORMAT(DATE_ADD(date_end, INTERVAL 1 DAY), '%Y-%m-%d') date_end, DATE_FORMAT(date_return, '%Y-%m-%d') date_return, hours_requested, payment_method, payperiod_id, payment_amount, date_insert FROM vacation_master  WHERE vacation_id =@v0", new object[] { vacation_id });
        }
        public DataTable Payment_Methods()
        {
            return Toolbox.doSQL_dt(@"SELECT * FROM vacation_payment_method", null);
        }
        public void Set_Member_id()
        {
            member_id = Toolbox.doSQL_string(@"SELECT member_id FROM vacation  WHERE vacation_id =@v0", new object[] { vacation_id });
        }
        public decimal Available_Hours()
        {
            var deposited = Math.Round(Deposited_Money(), 2, MidpointRounding.AwayFromZero);
            var withdrawn = Math.Round(Withdrawn_Money(), 2, MidpointRounding.AwayFromZero);
            var payrate = Math.Round(UserPayRate(), 2, MidpointRounding.AwayFromZero);
            var result = (deposited - withdrawn) / payrate;
            return (result);
        }
        public decimal Deposited_Money()
        {
            return Convert.ToDecimal(Toolbox.doSQL_double(@"SELECT CAST(IFNULL((SUM(hours*payrate)), 0) AS DECIMAL(8,2)) HOURS FROM bankedpay_ledger  WHERE type = 'V' AND member_id =@v0", new object[] { member_id }));
        }
        public decimal Withdrawn_Money()
        {
            return Convert.ToDecimal(Toolbox.doSQL_double(@"SELECT CAST(IFNULL((SUM(hours*payrate)), 0) AS DECIMAL(8,2)) HOURS FROM bankedpay_ledger  WHERE type in ('A', 'T') AND member_id =@v0", new object[] { member_id }));
        }
        public decimal UserPayRate()
        {
            var PayType = UserPayType();
            switch (PayType)
            {
                case "Salary":
                    sql = "SELECT FORMAT((wage / 80),2) AS WAGE FROM currentwage WHERE member_id =@v0";
                    break;
                default:
                    sql = "SELECT FORMAT(wage,2) AS WAGE FROM currentwage WHERE member_id =@v0";
                    break;
            }
            var wage = Toolbox.doSQL_double(sql, new object[] { member_id });
            var payrate = Math.Round(Convert.ToDecimal(wage), 2, MidpointRounding.AwayFromZero);
            return payrate;
        }
        public DataTable Past_Notes()
        {
            return Toolbox.doSQL_dt(@" SELECT b.member_fullname name_full, date_format(a.date_insert, '%m/%d/%Y @ %l:%i%p') date, urldecode(a.note) note FROM vacation_note a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.vacation_id=@v0  ORDER BY a.date_insert", new object[] { vacation_id });
        }
        public string UserPayType()
        {
            return Toolbox.doSQL_string(@"SELECT paytype FROM currentwage  WHERE member_id =@v0", new object[] { member_id });
        }
        public string PayPeriodBuilder(string selected_payperiod)
        {
            var sb = new StringBuilder();
            sb.Append("<select id='pay_period' onchange='check_request();'>");
            var dt = Toolbox.doSQL_dt(@"SELECT payperiodid, DATE_FORMAT(startdate, '%b %d, %Y') startdate, DATE_FORMAT(enddate, '%b %d, %Y') enddate FROM PAYPERIODS  WHERE completed = 0 order by payperiodid", null);
            foreach (DataRow dr in dt.Rows)
            {
                payperiod_id = dr["payperiodid"].ToString();
                var selected = payperiod_id == selected_payperiod ? " selected" : "";
                var start_date = dr["startdate"].ToString();
                var end_date = dr["enddate"].ToString();
                sb.AppendFormat("\n<option value='{0}'{3}>({0}): {1} - {2}", payperiod_id, start_date, end_date, selected);
            }
            sb.Append("\n</select>");
            return sb.ToString();
        }
        public void Deduct_Vacation(string amount, string type)
        {
            if (payperiod_id != null)
            {
                sql = string.Format(@"
UPDATE bankedpay_ledger 
SET	hours = {0},
type = '{3}',
payperiod_id = {2}
WHERE ref_id = {1}", amount, vacation_id, payperiod_id, type);
                Toolbox.doSQL_void(@" UPDATE bankedpay_ledger SET hours = @v0 , type = @v3 , payperiod_id = @v2  WHERE ref_id = @v1 ", new object[] { amount, vacation_id, payperiod_id, type });
            }
            else
            {
                sql = string.Format(@"
UPDATE bankedpay_ledger 
SET	hours = {0},
type = '{2}'
WHERE ref_id = {1}", amount, vacation_id, type);
                Toolbox.doSQL_void(@" UPDATE bankedpay_ledger SET hours = @v0 , type = @v2  WHERE ref_id = @v1 ", new object[] { amount, vacation_id, type });
            }

        }
        public void Request_Update(string start_date, string end_date, string return_date, string total_hours, string payment_method, string amount)
        {
            Toolbox.doSQL_void(@" UPDATE vacation_master SET date_start = @v0 , date_end = @v1 , date_return = @v2 , status = 1, hours_requested = @v3 , payment_method = @v4 , payment_amount = @v5 , payperiod_id = @v7  WHERE vacation_id = @v6 ", new object[] { start_date, end_date, return_date, total_hours, payment_method, amount, vacation_id, payperiod_id });
        }
    }




   

    protected void gv_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
    {
        if (e.VisibleIndex>=0)
        {
            if (Convert.ToInt32(gv.GetRowValues(e.VisibleIndex,"_ph_count"))>0 || Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "completed"))==1)
            {
                e.Visible = false;
            }
        }
    }

    protected void gv_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (e.Column.FieldName=="date_start" || e.Column.FieldName=="date_end" || e.Column.FieldName == "date_return")
        {
            NePayPeriod pp = new NePayPeriod(new NePayPeriod().CurrentPayPeriod());
            (e.Editor as ASPxDateEdit).MinDate = pp.start_date;
        }
    }

    protected void gv_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {

    }

    protected void gv_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        NePayPeriod pp = new NePayPeriod(new NePayPeriod().CurrentPayPeriod());
       
        var this_vacation_admin = new Vacation_Admin();
        this_vacation_admin._user = myMember;

        var vacation_id = e.Keys[0].ToString();
        this_vacation_admin.vacation_id = vacation_id;
        var start_date = Convert.ToDateTime(e.NewValues["date_start"]).ToString("yyyy-MM-dd");
        var end_date = Convert.ToDateTime(e.NewValues["date_end"]).ToString("yyyy-MM-dd");
        var return_date = Convert.ToDateTime(e.NewValues["date_return"]).ToString("yyyy-MM-dd");
        var payment_method = e.NewValues["payment_method_id"].ToString();
      
        var amount = e.NewValues["amount"].ToString();
        if (amount == "")
        {
            amount = "0";
        }
     //   var notes = e.NewValues["notes"].ToString();
        var payperiod_id = NePayPeriod.get_payperiod_id(Convert.ToDateTime(start_date)).ToString();
        this_vacation_admin.payperiod_id = payperiod_id;

        if (e.NewValues["date_start"].ToString() != e.OldValues["date_start"].ToString() && Convert.ToDateTime(e.NewValues["date_start"]) < pp.start_date)
        {
            throw new Exception("You must select a start date greater than " + pp.start_date);
        }
        if (e.NewValues["date_end"].ToString() != e.OldValues["date_end"].ToString() && Convert.ToDateTime(e.NewValues["date_end"]) < pp.start_date)
        {
            throw new Exception("You must select an end date greater than " + pp.start_date);
        }
        if (e.NewValues["date_return"].ToString() != e.OldValues["date_return"].ToString() && Convert.ToDateTime(e.NewValues["date_return"]) < pp.start_date)
        { 
            throw new Exception("You must select a return date greater than " + pp.start_date);
        }
		
        if (Convert.ToDateTime(e.NewValues["date_return"]) < (Convert.ToDateTime(e.OldValues["date_start"])))
        {
            throw new Exception("You must select a return date that is after the starting date");
        }
        if (Convert.ToDateTime(e.NewValues["date_return"]) < (Convert.ToDateTime(e.OldValues["date_end"])))
        {
            throw new Exception("You must select a return date that is after the ending date");
        }

        this_vacation_admin.Request_Update(start_date, end_date, return_date, "0", payment_method, amount);

        if (amount != "" && payment_method != "1")
        {
            var hours = Convert.ToDecimal(amount);
            this_vacation_admin.Deduct_Vacation(hours.ToString(), "A");
        }
        else
        {
            this_vacation_admin.Deduct_Vacation("0", "C");
        }

        gv.CancelEdit();
        e.Cancel = true;

        fill_grid();
    }

    protected void cb_notes_Callback(object source, CallbackEventArgs e)
    {
        ASPxCallback cb = (ASPxCallback)source;
        if ((e.Parameter!=null)&& e.Parameter!="")
        {
            var t = e.Parameter;
            var id = gv.GetRowValues(gv.EditingRowVisibleIndex, "vacation_id");
            if (e.Parameter.Length >= 1000)
            {
                t = e.Parameter.Substring(e.Parameter.Length - 1001, 999);
            }
            Toolbox.doSQL_void("Insert into vacation_note (member_id,note,vacation_id,date_insert) values(@v0,@v1,@v2,now())", new object[] { myMember.id32, t, id });
            cb.JSProperties["cp_message"] = "Note Added";
        }
        else
        {


        }
    }
} // Closes Page Class


