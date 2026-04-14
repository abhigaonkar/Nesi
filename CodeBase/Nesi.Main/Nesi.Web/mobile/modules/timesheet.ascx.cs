using System;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using DevExpress.Web;
using System.Web.UI;
using NESI.Common.Models;
using nesi.core;

public partial class mobile_modules_timesheet : System.Web.UI.UserControl
	{
	Toolbox _tools;
	NeMember _current_user;
	public bool only_wo { get; set; }
	public int workorder_id {get;set;}
	string _visibleBusinessUnits = "";
	public string payperiod_start {get; set; }
	public string payperiod_end {get; set; }
	public bool can_switch_business_units = false;
	    private NeBusinessUnit bu;
    protected void Page_Init(object _sender, EventArgs _e)
		{
		_tools				= new Toolbox();
		_current_user		= Toolbox.do_handle_authentication(28);
		var current_payperiod = NePayPeriod.CurrentOpenPayPeriodId();
		var payperiod_obj = new NePayPeriod(current_payperiod);
		_visibleBusinessUnits = new Current_User().visible_business_units;
		payperiod_start	= string.Format("new Date('{0:yyyy/MM/dd}')",payperiod_obj.start_date.AddDays(1));
		payperiod_end	= string.Format("new Date('{0:yyyy/MM/dd}')",DateTime.Now.AddDays(1));
		if(Session["mob_working_membertime_id"] == null || !IsPostBack)
			{
			Session["mob_working_membertime_id"]	= 0;
			}
		if(Session["mob_working_date"] == null || !IsPostBack)
			{
			Session["mob_working_date"]	= DateTime.Now.ToShortDateString();
			}
		bizdev_tb_calls.Attributes["type"]			= "number";
		bizdev_tb_dropoffs.Attributes["type"]		= "number";
		bizdev_tb_emails.Attributes["type"]			= "number";
		bizdev_tb_hours.Attributes["type"]			= "number";
		bizdev_tb_meetings.Attributes["type"]		= "number";
		bizdev_tb_quoteopps.Attributes["type"]		= "number";
		fill_bizdev_branches();
		var dd = Math.Round(payperiod_obj.start_date.Subtract(DateTime.Now.Date).TotalDays, 0);
        if (_current_user.AuthenticatedForPrivilege(29))
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setDays", string.Format("daysDatePick = {0};", dd), true);

		}

	protected void Page_Load(object _sender, EventArgs _e)
		{
		current_date.InnerText	= DateTime.Now.ToShortDateString();



		    if (!IsPostBack)
		        {
		        FillBusinessUnits();
		        wo_ddl_business_unit.DataBind();
		        wo_ddl_business_unit.SelectedValue = _current_user.business_unit_id.ToString();
		        bu = new NeBusinessUnit(wo_ddl_business_unit.SelectedValue);
		        if (!bu.allow_unlinked_timesheet)
		            {
		            fill_wo_customer_ddl();
		            fill_wo_ddl();
		            }
		        ddl_tabs.SelectedIndex = 0;
filltab(0);
		        }
		    bu = new NeBusinessUnit(wo_ddl_business_unit.SelectedValue);

		    if (only_wo)
		        {
		        mv_tabs.ActiveViewIndex = 0;
		        bt_signoff.Visible = false;
		        ASPxUploadControl1.Visible = false;
		        tr_customer.Style["display"] = "none";
		        tr_wo.Style["display"] = "none";
		        tr_scan.Style["display"] = "none";
		        ddl_tabs.Visible = false;
		        //tr_or.Style["display"]	= "none";
		        filltab(0);
		        }

        if (bu.allow_unlinked_timesheet)
		        {
		        wo_txt_customer.Visible = true;
		        wo_txt_workorder.Visible = true;
		        wo_ddl_workorder.Visible = false;
		        wo_ddl_customer.Visible = false;
		        bt_signoff.Visible = false;
		        }
		    else
		        {
		        wo_txt_customer.Visible = false;
		        wo_txt_workorder.Visible = false;
		        wo_ddl_workorder.Visible = true;
		        wo_ddl_customer.Visible = true;
		        bt_signoff.Visible = true;

        }
        populate_entries();
		btn_toggle_entries.Visible			= todays_entries.Rows.Count > 0;
		should_entries_button_be_shown();
		}
	public override void DataBind()
		{
	
		    if (!bu.allow_unlinked_timesheet)
		        {
		        do_databind();
		        }
		    }
	private void do_databind()
		{
		if (IsPostBack || workorder_id <= 0) return;
		var wo								= new NeWOProg(workorder_id);
		fill_wo_customer_ddl();
		wo_ddl_customer.SelectedValue		= new NECustomer((int) wo.WOProg_Customer_ID).Customer_Number;
		fill_wo_ddl();
		wo_ddl_workorder.SelectedValue		= workorder_id.ToString();
		}
	protected void filltab(int _index)
		{
		date.Text		= Session["mob_working_date"].ToString();
		mv_tabs.ActiveViewIndex	= _index;
		switch(_index)
			{
			case 0://work order
				FillBusinessUnits();
			    if (!bu.allow_unlinked_timesheet)
			        {
			        fill_wo_customer_ddl();
			        fill_wo_ddl();
			        }

			    break;
			case 1://quote
			   
			        fill_quote_customers();
			        fill_quote_ddl();
			      
			    break;
			case 2:		fill_bizdev_branches();
						
			break;
			case 3:		fill_shop_types();
			break;
			}
		}
	private void FillBusinessUnits()
		{
		var taxEntity			= new NeTaxEntity(_current_user.business_unit.tax_entity_id);
        
		var limiter				= can_switch_business_units 
										? "(id = 8 OR FIND_IN_SET(id, @v0))" 
										: taxEntity.allow_interbu_ts 
											? "tax_entity_id = "+_current_user.business_unit.tax_entity_id 
											: "id = "+_current_user.business_unit_id;
		wo_ddl_business_unit.DataSource	= Toolbox.doSQL_dt(string.Format(@"
SELECT 
	id, 
	ddl_name name 
FROM 
	business_unit 
WHERE 
	enable_timesheet = 1 and
	{0}
ORDER BY 
	ddl_name", limiter), 
			new object[] { _visibleBusinessUnits });
		wo_ddl_business_unit.DataBind();
		}
	protected void pc_ActiveTabChanged(object _source, DevExpress.Web.TabControlEventArgs _e)
		{
		Session["mob_working_membertime_id"]	= 0;
		filltab(_e.Tab.Index);
		clear_panels();
		populate_entries();
		}
	protected void quote_ddl_customer_SelectedIndexChanged(object _sender, EventArgs _e)
		{
		fill_quote_ddl();
		}
	protected void wo_ddl_customer_SelectedIndexChanged(object _sender, EventArgs _e)
		{
		fill_wo_ddl();
		}
	private void fill_quote_customers()
		{
       
		    if (!bu.allow_unlinked_timesheet)
		        {
		        quote_ddl_customer.DataSource = GetCustomersAndDetails();
		        quote_ddl_customer.DataTextField = "name";
		        quote_ddl_customer.DataValueField = "id";
		        quote_ddl_customer.DataBind();
		        quote_ddl_customer.Visible = true;
		        }
		    else
		        {
		        quote_ddl_customer.Visible = false;
		        quote_tb_quote.Visible = true;
		        }
		    }
	private void fill_bizdev_branches()
		{
		var branches					= new NeBusinessUnit();
		bizdev_ddl_branch.DataSource		= NeBusinessUnit.units_active();
		bizdev_ddl_branch.DataTextField		= "name";
		bizdev_ddl_branch.DataValueField	= "id";
		bizdev_ddl_branch.DataBind();
		bizdev_ddl_branch.SelectedValue		= bizdev_ddl_branch.Items.FindByValue(_current_user.business_unit_id.ToString()) == null ? "1" : _current_user.business_unit_id.ToString();
		}
	private void fill_shop_types()
		{

		var dt = Toolbox.doSQL_dt(@"select * from membertime_shop_type where status='active' order by type",null);
		shop_ddl_shoptype.DataSource = dt;
		shop_ddl_shoptype.DataValueField = "id";
		shop_ddl_shoptype.DataTextField = "type";
		shop_ddl_shoptype.DataBind();
		}
	private void fill_wo_customer_ddl()
		{

            var current_customer_number			= "";
		var mt						= new NeMemberTime();
		if((int) Session["mob_working_membertime_id"] > 0)
			{
			mt						= new NeMemberTime(Convert.ToInt32(Session["mob_working_membertime_id"]));
			current_customer_number				= mt.MemberTime_Cust_No;
			}
		else
			{
			mt.business_unit_id = Convert.ToInt32(wo_ddl_business_unit.SelectedValue);
			}

		//var dt = NECustomer.LoadCustomerWorkOrderList(mt.business_unit_id, _current_user);
		//wo_ddl_customer.DataSource  = dt.AsEnumerable().Select(_row => new
		//												{
		//												id = _row.Field<int>("customer_number"),
		//												name = _row.Field<string>("customer_name")
		//												}).Distinct();
		wo_ddl_customer.DataBind();
		    if (current_customer_number != "")
		        {
		        wo_ddl_customer.Items.FindByValue(current_customer_number).Selected = true;
		        fill_wo_ddl();
		        }


		    }
	private void fill_wo_ddl()
		{
		var current_wo_number			= 0;
		var current_childwo_number		= "";
		var current_customer_no			= "";
		var current_childcustomer_no		= "";
		if((int) Session["mob_working_membertime_id"] > 0)
			{
			var mt						= new NeMemberTime(Convert.ToInt32(Session["mob_working_membertime_id"]));
			current_wo_number					= mt.WOType == "WO" ? mt.woprog_id : 0;
			current_childwo_number				= mt.WOType == "WO" ? mt.MemberTimeChildWorkOrderID : "";
			current_customer_no					= mt.WOType == "WO" ? mt.MemberTime_Cust_No : "";
			current_childcustomer_no			= mt.WOType == "WO" ? mt.MemberTime_Child_Cust_No : "";
			}
		var customer_no					= Toolbox.ReturnZeroIfNull_int(wo_ddl_customer.SelectedValue);
		var c_list             = new NeExWorkOrder();

		    var customer_id = 0;
		    int.TryParse(wo_ddl_customer.SelectedValue, out customer_id);
        var wo_list               = c_list.LoadCustWOPROGWIPLIST(customer_id, wo_ddl_business_unit.SelectedValue, _current_user.id);
		wo_ddl_workorder.DataSource     = clean_wolist(wo_list);
		wo_ddl_workorder.DataTextField  = "text";
		wo_ddl_workorder.DataValueField = "value";
		wo_ddl_workorder.DataBind();
		row_signoff.Visible				= false;
		if(current_wo_number > 0)
			{
			wo_ddl_workorder.Items.FindByValue(current_wo_number.ToString()).Selected	= true;
		current_wo_number				= Convert.ToInt32(wo_ddl_workorder.SelectedValue);
		var temp_wo				= new NeWOProg( current_wo_number);
		row_signoff.Visible				= temp_wo.woprog_id > 0 && customer_no > 0;
			}
		//hid_woprog_id.Value				= temp_wo.woprog_id.ToString();
		}
	private DataTable GetCustomersAndDetails()
		{
		
		return Toolbox.doSQL_dt(@" 
SELECT 
	CONCAT(c.ddl_name,' => ', b.customer_name) name, 
	b.customer_id id,
	0 orderby,
	c.ddl_name,
	c.id business_unit_id 
FROM 
	quote_master a
INNER JOIN 
	customer b ON
	b.customer_id = a.customer_id AND 
	a.status_id NOT IN (6,7,8,9)  
INNER JOIN
	business_unit c ON c.id = a.business_unit_id AND c.istest = 'F' AND c.active = 'T'
WHERE 
	a.business_unit_id = @v0
GROUP BY a.customer_id, c.ddl_name
UNION 
SELECT 
	CONCAT(c.ddl_name,' => ', b.customer_name) name, 
	b.customer_id id, 
	1 orderby,
	c.ddl_name,
	c.id business_unit_id
FROM 
	quote_master a
INNER JOIN 
	customer b ON
	b.customer_id = a.customer_id AND 
	a.status_id NOT IN (6,7,8,9)
INNER JOIN
	business_unit c ON c.id = a.business_unit_id AND c.istest = 'F' AND c.active = 'T'
WHERE 
	FIND_IN_SET(a.business_unit_id, @v1) AND
	a.business_unit_id != @v0
GROUP BY a.customer_id, c.ddl_name
ORDER BY  
	orderby,
	ddl_name,
	name", new object[] { _current_user.business_unit_id, _visibleBusinessUnits } );

		}
	private void fill_quote_ddl()
		{
		    if (!bu.allow_unlinked_timesheet)
		        {
		        quote_ddl_customer.Visible = true;
		        quote_ddl_quote.Visible = true;
		        quote_tb_quote.Visible = false;
		        quote_tb_customer.Visible = false;
		        using (var conn = Toolbox.connect())
		            {
		            var custs = GetCustomersAndDetails();
		            var buID = (from d in custs.AsEnumerable()
		                where
		                d.Field<int>("id") == Convert.ToInt32(quote_ddl_customer.SelectedValue) &&
		                d.Field<string>("name") == quote_ddl_customer.SelectedItem.Text
		                select d.Field<int>("business_unit_id")).FirstOrDefault();
		            if (buID == 0) return;
		            quote_ddl_quote.DataSource = Toolbox.doSQL_dt(conn,
		                @" SELECT CONCAT(a.quote_id, a.revision) value, CONCAT(a.quote_id, ' v', a.revision, ' - ', a.job_description, ' - $', a.quoted_price) text FROM quote_master a LEFT JOIN customer b ON a.customer_id = b.customer_id LEFT join business_unit c ON a.business_unit_id = c.id  WHERE a.status_id NOT IN (6,7,9,8) and b.customer_id =@v0 AND a.business_unit_id = @v1 ORDER BY c.name, a.quote_id desc",
		                new object[] {quote_ddl_customer.SelectedValue, buID});

		            quote_ddl_quote.DataTextField = "text";
		            quote_ddl_quote.DataValueField = "value";
		            quote_ddl_quote.DataBind();
		            }
		        }
		    else
		        {
		        quote_ddl_customer.Visible = false;
		        quote_ddl_quote.Visible = false;
		        quote_tb_quote.Visible = true;
		        quote_tb_customer.Visible = true;
        }
		    }
	private static DataTable clean_quotelist(ArrayList _quote_list)
		{
		var temp_quote_dt = new DataTable();
		if (_quote_list.Count > 0)
			{
			temp_quote_dt.Columns.Add("value", Type.GetType("System.String"));
			temp_quote_dt.Columns.Add("text", Type.GetType("System.String"));
			foreach (quote quote in _quote_list)
				{
				var _quote = temp_quote_dt.NewRow();
				_quote["value"] = quote.QuoteID.ToString();
				_quote["text"] = quote.All.Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");
				temp_quote_dt.Rows.Add(_quote);
				}
			}
		return temp_quote_dt;
		}
	private static DataTable clean_wolist(ArrayList _wo_list)
		{
		var temp_wo_dt = new DataTable();
		if (_wo_list.Count > 0)
			{
			temp_wo_dt.Columns.Add("value", Type.GetType("System.String"));
			temp_wo_dt.Columns.Add("text", Type.GetType("System.String"));
			foreach (NeWOProg ex in _wo_list)
				{
				var _ex = temp_wo_dt.NewRow();
				_ex["value"] = ex.woprog_id.ToString();
				_ex["text"] = ex.Description.Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");
				temp_wo_dt.Rows.Add(_ex);
				}
			}
		return temp_wo_dt;
		}
	protected void btn_toggle_entries_Click(object _sender, EventArgs _e)
		{
		var btn					= (Button) _sender;
		pnl_entries.Visible			= !pnl_entries.Visible;
		btn.Text					= pnl_entries.Visible ? "Hide Time Entries" : "Show Time Entries";
		populate_entries();
		}
	private void should_entries_button_be_shown()
		{
		if (date.Text == "") return;
		var dt				= new NeMemberTime().CurrentMemberTimeSummary(_current_user.id, date.Text);
		btn_toggle_entries.Visible	= dt.Rows.Count > 0;
		populate_entries();
		}
	private void populate_entries()
		{
		if(todays_entries.Rows.Count > 0)
			{
			todays_entries.Rows.Clear();
			}
		if(pnl_entries.Visible && date.Text != "")
			{
			//double hours			= 0;
			//double miles			= 0;
			var dt			= Toolbox.doSQL_dt(@" Select MT.MemberTime_ID, MT.business_unit_id , MT.Date , MT.MemberTime_WorkOrder_ID workorder_id, MT.MemberTime_Cust_No, MT.MemberTime_Customer_Name customer_name, if(MemberTime_Mileage = 'false', MT.NumberofHours, 0 ) as Hours, if(MemberTime_Mileage = 'true', MT.NumberofHours, 0) as Miles, MT.MemberTime_WoComment_ID, MT.MemberTime_PayTypeHours_ID, MT.Created_Date, Hour.Description as HourType, M.Member_User as MTBy, MT.MemberTime_Premium, WOC.Comments as Comments, MT.WOType from ((membertime MT left Join wocomment WOC ON MT.MemberTime_WoComment_ID = WOC.WoComment_ID) LEFT JOIN paytypehours hour ON MT.MemberTime_PayTypeHours_ID = hour.PayTypeHours_ID) LEFT JOIN member M ON MT.business_unit_id = M.Member_ID Where MT.Date = @v0  AND MT.membertime_memberid = @v1 ", new object[] {  Toolbox.MySQL_shortdt(Convert.ToDateTime(date.Text)), _current_user.id} );
			if(dt.Rows.Count > 0)
				{
				var thtr					= new HtmlTableRow();
				var th_cu					= new HtmlTableCell("th");
				th_cu.InnerHtml						= "Customer";
				th_cu.Style["background-color"]		= "#4682b4";
				th_cu.Style["color"]				= "#fff";	
				var th_wo					= new HtmlTableCell("th");
				th_wo.InnerHtml						= "Type of Work";
				th_wo.Style["background-color"]		= "#4682b4";
				th_wo.Style["color"]				= "#fff";	
				var th_hr					= new HtmlTableCell("th");
				th_hr.InnerHtml						= "Hours";
				th_hr.Style["background-color"]		= "#4682b4";
				th_hr.Style["color"]				= "#fff";	
				var th_ac					= new HtmlTableCell("th");
				th_ac.InnerHtml						= "Actions";
				th_ac.Style["background-color"]		= "#4682b4";
				th_ac.Style["color"]				= "#fff";	
				thtr.Cells.Add(th_cu);
				thtr.Cells.Add(th_wo);
				thtr.Cells.Add(th_hr);
				thtr.Cells.Add(th_ac);
				todays_entries.Rows.Add(thtr);

				foreach(DataRow dr in dt.Rows)
					{
					var tr					= new HtmlTableRow();
					var td_cu				= new HtmlTableCell();
					var comments					= dr["Comments"] == null ? "N/A" : dr["Comments"].ToString();
					var possible_quote				= 0;
					int.TryParse(dr["customer_name"].ToString(), out possible_quote);
					var cust_name				= possible_quote == 0 ? dr["customer_name"].ToString() : "Quoted Job";
					var is_hr						= cust_name == "Human Resources";
					td_cu.InnerHtml					= is_hr ? "<img src='/images/icon/icon[calendar].gif' width='16' height='16' align='absmiddle' /> Human Resources (Time Off)" : cust_name;
					td_cu.Attributes["class"]		= "cu";
					td_cu.Align						= "left";
					td_cu.Style["color"]			= is_hr ? "#f00" : "#000";
					var td_wo				= new HtmlTableCell();
					var this_id						= Convert.ToInt32(dr["workorder_id"]);
					    var id = (this_id > 99991000) && !bu.allow_unlinked_timesheet
					        ? "Shop Time"
					        : this_id == 0
					            ? "Business Dev."
					            : dr["WOType"] + " #: " + this_id;
															
					td_wo.Align						= "center";
					td_wo.InnerText					= is_hr ? "" : id;
					td_wo.Attributes["class"]		= "wo";
					td_wo.Style["color"]			= is_hr ? "#f00" : "#000";
					var td_hr				= new HtmlTableCell();
					td_hr.Align						= "center";
					td_hr.Attributes["class"]		= "hr";
					td_hr.InnerText					= dr["Hours"].ToString();
					td_hr.Style["color"]			= is_hr ? "#f00" : "#000";
					var td_ac				= new HtmlTableCell();
					td_ac.Attributes["class"]		= "ac";
					td_ac.Align						= "center";
					var bt_edit					= new Button();
					bt_edit.ID						= "btedit_"+dr["MemberTime_ID"];
					bt_edit.Click					+= new EventHandler(edit_entry);
					bt_edit.CssClass				= "button";
					bt_edit.Text					= "Edit";
					var bt_delete				= new Button();
					var this_membertime_id			= Convert.ToInt32(dr["MemberTime_ID"]);
					bt_delete.ID					= "btdelete_"+this_membertime_id;
					bt_delete.OnClientClick			= "return confirm('Are you sure you want to delete this entry?');";
					bt_delete.Click					+= new EventHandler(delete_entry);
					bt_delete.CssClass				= "button";
					bt_delete.Text					= "Delete";
					if(!is_hr)
						{
						td_ac.Controls.Add(bt_edit);
						td_ac.Controls.Add(bt_delete);
						}
					tr.Cells.Add(td_cu);
					tr.Cells.Add(td_wo);
					tr.Cells.Add(td_hr);
					tr.Cells.Add(td_ac);
					if((int) Session["mob_working_membertime_id"] == this_membertime_id)
						{
						tr.Attributes["class"]		= "selected";
						}
					todays_entries.Rows.Add(tr);
                    tr=new HtmlTableRow();
                    HtmlTableCell td_comments = new HtmlTableCell();
					    td_comments.ColSpan = 4;
					    td_comments.InnerHtml = "<span class='comments' ><b>Comments: </b>" + comments + "</span>";
                td_comments.Style.Add("border-bottom", "solid");
					    td_comments.Style.Add("border-bottom-width", "1px");
					    td_comments.Style.Add("border-bottom-color", "grey");
                    tr.Cells.Add(td_comments);
					
                  
                    todays_entries.Rows.Add(tr);
                }
				}
			}
		else
			{
			Session["mob_working_membertime_id"]	= 0;
			}
		}
	private Toolbox.boolstr CheckOpenPayperiod()
		{
		var payperiod_id		= NePayPeriod.get_payperiod_id(Convert.ToDateTime(date.Text));
		var returnedStr			= "";
		if(payperiod_id > 0)
			{
			var c				= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM payroll_hours WHERE member_id = @v0  AND payperiod_id = @v1 ", new object[] { _current_user.id, payperiod_id } );
			returnedStr			= c > 0 
									? "Payroll has already been submitted for this date - no changes are allowed after payroll for a user has been submitted."
									: "";
			bizdev_b_submit.Enabled = c == 0;
			quote_b_submit.Enabled = c == 0;
			shop_b_submit.Enabled = c == 0;
			wo_b_submit.Enabled = c == 0;
			return new Toolbox.boolstr { message =returnedStr, success = c == 0};
			}
		return new Toolbox.boolstr() { message = "Date isn't returning a payperiod", success = false};
		}
	protected void date_TextChanged(object _sender, EventArgs _e)
		{
		should_entries_button_be_shown();
		Session["mob_working_date"]				= Convert.ToDateTime(date.Text).ToShortDateString();
		Session["mob_working_membertime_id"]	= 0;
		clear_panels();
		btn_toggle_entries_Click(btn_toggle_entries, null);
		var bs = CheckOpenPayperiod();
		error_div.InnerHtml = error_msg(bs.message);
		}
	private void clear_panels()
		{
		#region WO
		if(mv_tabs.ActiveViewIndex == 0)
			{
			    if (!bu.allow_unlinked_timesheet)
			        {
			        fill_wo_customer_ddl();
			        wo_ddl_customer.SelectedIndex = -1;
			        wo_ddl_customer.Enabled = true;


			        wo_ddl_workorder.Items.Clear();
			        wo_ddl_workorder.SelectedIndex = -1;
			        wo_ddl_workorder.Enabled = true;
			        wo_tb_workorder.Enabled = true;
			        }
			    wo_ddl_percent_complete.SelectedIndex	        = -1;
			wo_ddl_percent_complete.Enabled				    = true;
			wo_tb_hours.Text						        = "";
			wo_ddl_hourtype.SelectedIndex			        = -1;
			wo_ddl_hourtype.Enabled						    = true;
			wo_tb_comment.Text						        = "";
			wo_b_cancel.Visible						        = false;
			}
		#endregion
		#region Quotes
		else if(mv_tabs.ActiveViewIndex == 1)
			{
			fill_quote_customers();
			quote_ddl_customer.SelectedIndex			    = -1;
			quote_ddl_customer.Enabled				        = true;
			fill_quote_ddl();
			quote_ddl_quote.SelectedIndex				    = -1;
			quote_ddl_quote.Enabled				            = true;
			quote_ddl_percent_complete.SelectedIndex	    = 0;
			quote_ddl_percent_complete.Enabled				= true;
			quote_tb_hours.Text							    = "";
			//quote_tb_comment.Text						    = "";
			quote_b_cancel.Visible						    = false;
			}
		#endregion
		#region Business Development
		else if(mv_tabs.ActiveViewIndex == 2)
			{
			fill_bizdev_branches();
			bizdev_tb_hours.Text						    = "";
			bizdev_ddl_branch.Enabled				        = true;
			bizdev_tb_calls.Text						    = "";
			bizdev_tb_meetings.Text						    = "";
			bizdev_tb_dropoffs.Text						    = "";
			bizdev_tb_emails.Text						    = "";
			bizdev_tb_vms.Text							    = "";
			bizdev_tb_quoteopps.Text					    = "";
			bizdev_b_cancel.Visible						    = false;
			}
		#endregion
		#region Shop
		else if(mv_tabs.ActiveViewIndex == 3)
			{
			fill_shop_types();
			shop_ddl_shoptype.Enabled				        = true;
			shop_tb_hours.Text						        = "";
			shop_tb_comment.Text					        = "";
			shop_b_cancel.Visible					        = false;
			}
		#endregion
		}
	protected void edit_entry(object _sender, EventArgs _e)
		{
		var btn								= (Button) _sender;
		var membertime_id						= Convert.ToInt32(btn.ID.Split('_')[1]);
		Session["mob_working_membertime_id"]	= membertime_id;
		var mt							= new NeMemberTime(Convert.ToInt32(membertime_id));
		error_div.InnerHtml						= "";
				var SelWoComment = new NeWOComments(Convert.ToInt32(mt.MemberTimeWoCommentID.ToString()));
		switch(mt.WOType)
			{
			case "WO":
				wo_ddl_business_unit.SelectedValue = mt.business_unit_id.ToString();

			    if (!bu.allow_unlinked_timesheet)
			        {
			        fill_wo_customer_ddl();
			        wo_ddl_customer.ClearSelection();
			        wo_ddl_customer.Items.FindByValue(mt.MemberTime_Cust_No).Selected = true;
			        wo_ddl_customer.Enabled = false;
			        // Fill / Set the work order
			        fill_wo_ddl();
			        wo_ddl_workorder.ClearSelection();
			        wo_ddl_workorder.Items.FindByValue(mt.woprog_id.ToString()).Selected = true;
			        wo_ddl_workorder.Enabled = false;
			        wo_tb_workorder.Enabled = false;
			        // Set the hours type
			        wo_ddl_hourtype.ClearSelection();
			        wo_ddl_hourtype.Items.FindByValue(mt.MemberTimePayTypeHoursID.ToString()).Selected = true;
			        wo_ddl_hourtype.Enabled = false;
			        }
			    else
			        {
			        wo_txt_customer.Text = mt.CustomerName;
			        wo_txt_workorder.Text = mt.WorkOrderID;

			        }
			    // Set the page control index
                    mv_tabs.ActiveViewIndex			= 0;
				// Put in the saved hours
				wo_tb_hours.Text			= mt.NumberOfHours.ToString();
				// Set the percent done
				wo_ddl_percent_complete.ClearSelection();
				wo_ddl_percent_complete.Items.FindByValue(mt.wo_percent_complete.ToString()).Selected		= true;
				// Set the customer
				
				// Set the comment text
				var comment				= Toolbox.doSQL_string(@"SELECT comments FROM WOComment WHERE wocomment_id = "+mt.MemberTimeWoCommentID);
				wo_tb_comment.Text			= comment;
				wo_b_cancel.Visible			= true;
			break;
			case "Quote":
				fill_quote_customers();
				mv_tabs.ActiveViewIndex			= 1;
				quote_ddl_customer.Enabled	= false;
				quote_ddl_quote.Enabled		= false;
				quote_ddl_customer.SelectedValue = mt.Cust_No;
				fill_quote_ddl();
			    if (bu.allow_unlinked_timesheet)
			        {
			        quote_tb_customer.Text = mt.CustomerName;
			        quote_tb_quote.Text = mt.WorkOrderID;
			        }
				quote_ddl_quote.SelectedValue = mt.WorkOrderID;
				quote_tb_hours.Text			= mt.NumberOfHours.ToString();
				quote_ddl_percent_complete.ClearSelection();
				quote_ddl_percent_complete.Items.FindByValue(mt.wo_percent_complete.ToString()).Selected	= true;
				quote_b_cancel.Visible		= true;
			break;
			case "Telem":
				mv_tabs.ActiveViewIndex			= 2;
				bizdev_tb_hours.Text		= mt.NumberOfHours.ToString();
				bizdev_b_cancel.Visible		= true;
				var Telemcomment = "";
				Telemcomment = SelWoComment.Comments;
				
				bizdev_tb_calls.Text = Regex.Matches(Telemcomment, "\\d+")[0].Value;
				bizdev_tb_meetings.Text = Regex.Matches(Telemcomment, "\\d+")[1].Value;
				bizdev_tb_dropoffs.Text = Regex.Matches(Telemcomment, "\\d+")[2].Value;
				bizdev_tb_emails.Text = Regex.Matches(Telemcomment, "\\d+")[3].Value;
				bizdev_tb_vms.Text = Regex.Matches(Telemcomment, "\\d+")[4].Value;
				bizdev_tb_quoteopps.Text = Regex.Matches(Telemcomment, "\\d+")[5].Value;
			break;
			case "Shop":
				mv_tabs.ActiveViewIndex			= 3;
				fill_shop_types();
				shop_ddl_shoptype.SelectedValue = mt.WorkOrderID;
				shop_tb_hours.Text			= mt.NumberOfHours.ToString();
				shop_tb_comment.Text		= SelWoComment.Comments;
				shop_b_cancel.Visible		= true;
				shop_ddl_shoptype.Enabled	= false;
			break;
			}
		populate_entries();
		}
	protected void delete_entry(object _sender, EventArgs _e)
		{
		var btn			= (Button) _sender;
		var membertime_id	= Convert.ToInt32(btn.ID.Split('_')[1]);

		string product_code;
		var str_child_dsn = "";
		var has_child	= false;
		var str_member_id_for_error = "";
		try
			{
			var member_time_det = new NeMemberTime(Convert.ToInt32(membertime_id));
			var del_value = member_time_det.NumberOfHours;
			var wo_type	= member_time_det.WOType.ToUpper();
			var paytypehoursid = member_time_det.PayTypeHoursID.ToString();
			if (wo_type != "TELEM" && wo_type != "SHOP" && wo_type != "QUOTE")
				{
				var statuscheck = new NeWOProg(member_time_det.membertime_woprog_id);
				if (statuscheck.Status == "Invoiced" || statuscheck.Status == OpsWOStatus.WaitingToBeInvoiced)
					{
					throw new Exception("You Can Not Delete This Entry It is Already Invoiced");
					}
				if (statuscheck.woprog_hold == 1)
					{
					throw new Exception("You Can Not Delete This Entry The Work Order is on Hold");
					}
				if (del_value > 0)
					{
					var pay_type = Toolbox.doSQL_string(@"SELECT paytype_id FROM member WHERE member_id =" + _current_user.id);
					if (pay_type != "2" && pay_type != "3")
						{
						var datecheck = member_time_det.Date;
						var datefix = Convert.ToDateTime(datecheck);
						datecheck = datefix.ToString("yyyy-MM-dd");
						var payperiodid = _tools.getSQL_int(@"SELECT PayperiodID FROM payperiods  WHERE StartDate <=@v0 AND EndDate >=@v1  limit 1 ", new object[] { datecheck,datecheck });
						var payperiodinfo = new NePayPeriod(Convert.ToInt32(payperiodid));
						var current_payperiod = Toolbox.doSQL_int(@"CALL _payperiod()");
						if ((payperiodid < current_payperiod) && (_current_user.business_unit_id != 11))
						{

							throw new Exception("Only a member of NESI can delete an entry from a previous pay period.");

						}
						var hourstouse = _tools.getSQL_double(@"SELECT IFNULL(SUM(hours), 0) AS deposited FROM bankedpay_ledger WHERE type='D' AND member_id = @v0  AND payperiod_id = @v1 ", new object[] {  _current_user.id, payperiodid } );
						if (paytypehoursid != "1")
						{
							hourstouse = 0;
						}
						var hoursintimesheet = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(NumberofHours), 0) FROM membertime WHERE MemberTime_PayTypeHours_ID = " + paytypehoursid + " AND business_unit_id = @v0  AND DATE >= @v1  AND DATE <= @v2 ", new object[] {  _current_user.id, payperiodinfo.StartDate, payperiodinfo.Enddate } );
						var hoursdifference = hoursintimesheet - del_value;
						if (hoursdifference < hourstouse && hourstouse != 0)
							{
							throw new Exception("You cannot Delete the hours because you don't have this many yet for this pay period.");
							}
						}
					}
				}
			str_member_id_for_error = _current_user.id.ToString();
			if ((member_time_det.MemberTime_ChildCompanyID != 0 && !member_time_det.child_shoptime)&&!bu.allow_unlinked_timesheet)
				{
				var childcompany = new NeBusinessUnit();
				str_child_dsn = childcompany.GetBUDSN(Convert.ToInt32(member_time_det.MemberTime_ChildCompanyID));
				has_child = true;
				Session["childdsn"] = str_child_dsn;
				var childstatuscheck = new NeWOProg(Convert.ToInt32(member_time_det.MemberTimeChildWorkOrderID));
				if (childstatuscheck.Status == "Invoiced" || childstatuscheck.Status == OpsWOStatus.WaitingToBeInvoiced)
					{
					throw new Exception("You Can Not Delete This Child Entry It is Already Invoiced");
					}
				if (childstatuscheck.Status == "Hold")
					{
					throw new Exception("You Cannot Delete This Entry the Child Work Order is on Hold");
					}
				}
			if (wo_type == "QUOTE")
				{
				member_time_det.MemberIDAudit = _current_user.id;
				member_time_det.DeleteMemberTimeDet();
				#region update quote hours spent

				    if (!bu.allow_unlinked_timesheet)
				        {
				        _tools.getSQL_void(
				            @"UPDATE quote_master LEFT JOIN vw_membertime_quote ON LEFT (quote_master.quote_id, 6) = vw_membertime_quote.MemberTime_WorkOrder_ID
SET quote_master.hours_spent = ifnull(vw_membertime_quote.NumberOfHours,0)
WHERE
quote_master.quote_id = " + quote_ddl_quote.SelectedValue);
				        }

				    #endregion
				
				}
			else if (wo_type == "TELEM")
				{
				member_time_det.MemberIDAudit = _current_user.id;
				member_time_det.DeleteMemberTimeDet();
				}
			else if (wo_type == "SHOP")
				{
				member_time_det.MemberIDAudit = _current_user.id;
				member_time_det.DeleteMemberTimeDet();
				}
			else
				{
				var tempstore_wobv = new NeExWorkOrder();
				tempstore_wobv.DSN = _current_user.business_unit.DSN;
				var str_child_work_order = "";
				if (has_child)
					{
					str_child_work_order = member_time_det.MemberTimeChildWorkOrderID;
					}
				try
					{
					double dbl_wo_time_total = 0;
					double dbl_child_wo_time_total = 0;
					member_time_det.MemberIDAudit = _current_user.id;
					member_time_det.DeleteMemberTimeDet();
					if ((member_time_det.MemberTime_Mileage == "false")&&(!bu.allow_unlinked_timesheet))
						{
						if (wo_type != "QUOTE")
							{
							dbl_wo_time_total = _tools.getSQL_double(@" SELECT IFNULL(SUM(membertime.NumberofHours),0) AS SumOfNumberofHours FROM membertime WHERE business_unit_id = @v0  AND membertime_workorder_id = @v1  AND business_unit_id =@v2  AND membertime_paytypehours_id = @v3  AND membertime_cust_no != 'Quote'", new object[] {  member_time_det.business_unit_id, member_time_det.MemberTimeWorkOrderID, member_time_det.business_unit_id, member_time_det.MemberTimePayTypeHoursID  } );
							if (has_child)
								{
								dbl_child_wo_time_total = _tools.getSQL_double(@" SELECT IFNULL(SUM(membertime.numberofhours),0) AS SumOfNumberofHours FROM membertime WHERE business_unit_id = @v0  AND membertime_child_workorder_id = @v1  AND membertime_child_companyid =@v2  AND membertime_paytypehours_id = @v3  AND membertime_cust_no != 'Quote'", new object[] {  member_time_det.business_unit_id, member_time_det.MemberTimeWorkOrderID, member_time_det.business_unit_id, member_time_det.MemberTimePayTypeHoursID  } );
								}
							del_value                               = del_value * -1;
							var master_id                           = 0;
							var line_bu				= new NeBusinessUnit(member_time_det.business_unit_id);
							var detail               = new NeWODetailCurrent();
							var paytype                         = _tools.getSQL_string(@"SELECT Description FROM paytypehours  WHERE PayTypeHours_ID =@v0", new object[] { member_time_det.MemberTimePayTypeHoursID });
							var str_labor                        = line_bu.country == "CDN" ? "Labour" : "Labor";
							detail.date_modified = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
							detail.master_id     = master_id;
							detail.qty_committed = del_value;
							detail.qty_invoiced  = del_value;
							detail.qty_ordered	= del_value;
							detail.woprog_id     = member_time_det.membertime_woprog_id;
							detail.bvwo          = Convert.ToInt32(member_time_det.MemberTimeWorkOrderID);
							detail.code          = master_id.ToString();
							detail.description   = _current_user.FullName + " Hours " + str_labor + ": " + paytype;
							detail.memberid                        = member_time_det.member_id;
							detail.paytypeid                       = Convert.ToInt32(member_time_det.MemberTimePayTypeHoursID);
							var update_number                       = detail.DeleteWODetailCurrentFromTimeSheetTemp(member_time_det.membertime_woprog_id.ToString());
							var child_member_company           = new NeBusinessUnit(member_time_det.MemberTime_ChildCompanyID);
							if (has_child)
								{
								detail.business_unit_id = Convert.ToInt32(member_time_det.MemberTime_ChildCompanyID);
								detail.DeleteWODetailCurrentFromTimeSheetTemp(member_time_det.membertime_child_woprog_id.ToString());
								}
							}
						}
					}
				catch (Exception ex)
					{
					var error = new NEErrorReport();
					error.WriteErrorReport(_current_user.id.ToString(), _current_user.business_unit.DSN, ex.Message + " Time Sheet LINE 618");
					throw new Exception(string.Format("The following Error occured  {0}.", ex.Message));
					}
				}
			should_entries_button_be_shown();
			}
		catch (Exception ex)
			{
			if (ex.ToString() != "NoLongerOpen")
				{
				throw new Exception(string.Format("The following Error occured  {0}.", ex.Message));
				}
			else
				{
				throw new Exception("You Cannot Delete a Time Sheet Entry for a Work Order That is no Longer Open. Press Cancel.");
				}
			}
		}
	protected void quote_ddl_quote_SelectedIndexChanged(object _sender, EventArgs _e)
		{

		}
	protected void b_cancel_Click(object _sender, EventArgs _e)
		{
		Session["mob_working_membertime_id"]	= 0;
		clear_panels();
		populate_entries();
		}
	protected void wo_tb_workorder_TextChanged(object _sender, EventArgs _e)
		{
		var tb			= (TextBox) _sender;
		if(test_wo_bc(tb))
			{
			var split_bc	= tb.Text.Split('-');
			var woprog_id		= Convert.ToInt32(split_bc[1]);
			var wo			= new NeWOProg(woprog_id);
			fill_wo_customer_ddl();
			var cust		= new NECustomer(Convert.ToInt32(wo.WOProg_Customer_ID));
			if(wo_ddl_customer.Items.FindByValue(cust.Customer_Number) != null)
				{
				wo_ddl_customer.ClearSelection();
				wo_ddl_customer.Items.FindByValue(cust.Customer_Number).Selected		= true;
				fill_wo_ddl();
				if(wo_ddl_workorder.Items.FindByValue(wo.OrderNumber) != null)
					{
					wo_ddl_workorder.ClearSelection();
					wo_ddl_workorder.Items.FindByValue(wo.OrderNumber).Selected		= true;
					}
				}
			tb.Text			= "";
			tb.Attributes["placeholder"]	= "Scan WO barcode";
			}
		}
	private bool test_wo_bc(TextBox _tb)
		{
		var r				= new Regex("^002-[0-9]+$");
		var m				= r.Match(_tb.Text).Success;
		if(!m)
			{
			_tb.Attributes["placeholder"]	= "Not a valid WO";
			_tb.Text							= "";
			_tb.Focus();
			}
		return m;
		}
	protected string error_msg(string _text)
		{
		return _text == "" ? "" : string.Format(@"<b class='item'>&bullet; {0}</b></br>", _text);
		}
	protected void b_submit_Click(object _sender, EventArgs _e)
		{
		var int_first_part				= 0;
		var is_valid					= true;
		var error_text				= "";
		var str_member_id_for_error		= "0";
		double hours					= 0;
		var this_wo				= new NeWOProg();
		var linked_wo				= new NeWOProg();
		var bd						= new bizdev();
		var shop					= new shoptime();
		    NeBusinessUnit bu = new NeBusinessUnit(wo_ddl_business_unit.SelectedValue);
        var is_editing					= Session["mob_working_membertime_id"]	!= null && Convert.ToInt32(Session["mob_working_membertime_id"]) > 0;
		#region Validation
		switch(mv_tabs.ActiveViewIndex)
			{
			#region Work Order
			case 0:
			    if (bu.allow_unlinked_timesheet)
			        {
			        if (wo_txt_workorder.Text == "")
			            {
			            is_valid = false;
			            error_text += error_msg("Please Enter a work order number");
			            }
			        if (wo_txt_customer.Text == "")
			            {
			            is_valid = false;
			            error_text += error_msg("Please Enter a Customer");
			            }
                }
			    else
			        {
			        if (!only_wo)
			            {
			            if (wo_ddl_customer.SelectedValue == "0")
			                {
			                is_valid = false;
			                error_text += error_msg("Please select a customer");
			                }
			            if (wo_ddl_workorder.SelectedValue == "0")
			                {
			                is_valid = false;
			                error_text += error_msg("Please select a work order");
			                }
			            try
			                {
			                var woprog_id = only_wo ? Session["mobile_woprog_id"] : wo_ddl_workorder.SelectedValue;
			                this_wo = new NeWOProg(Convert.ToInt32(woprog_id));
			                if (this_wo.parent_woprog_id > 1)
			                    {
			                    linked_wo = new NeWOProg(this_wo.parent_woprog_id);
			                    }
			                }
			            catch (Exception ee)
			                {
			                is_valid = false;
			                error_text += error_msg("There was an error when loading the selected work order: " + ee.Message);
			                }

			            }
			        }
			    if (wo_tb_comment.Text.Length < 10)
			            {
			            is_valid = false;
			            error_text += error_msg("Please enter a detailed comment greater than 10 characters.");
			            }
			        double.TryParse(wo_tb_hours.Text, out hours);
			        if (wo_tb_hours.Text == "" || hours == 0 || hours < 0)
			            {
			            is_valid = false;
			            error_text += error_msg("Hours are blank or they're an invalid value.");
			            }
			       
			        
			    break;
			#endregion
			#region Quotes
			case 1:
			    if (bu.allow_unlinked_timesheet)
			        {
			        if (quote_tb_quote.Text == "")
			            {
			            is_valid = false;
			            error_text += error_msg("Please enter a quote number");
			            }
			        if (quote_tb_customer.Text == "")
			            {
			            is_valid = false;
			            error_text += error_msg("Please enter a customer name");
			            }
			        }
			    else
			        {
			        if (quote_ddl_customer.SelectedValue == "0")
			            {
			            is_valid = false;
			            error_text += error_msg("Please select a customer");
			            }
			        if (quote_ddl_quote.SelectedValue == "0")
			            {
			            is_valid = false;
			            error_text += error_msg("Please select a quote");
			            }
			        }
			    //if (quote_tb_comment.Text.Length < 10)
				//	{
				//	is_valid = false;
				//	error_text += error_msg("Please enter A Detailed Comment Greater Than 10 Characters.");
				//	}
				double.TryParse(quote_tb_hours.Text, out hours);
				if(quote_tb_hours.Text == "" || hours == 0 || hours < 0)
					{
					is_valid = false;
					error_text += error_msg("Hours are blank or they are in invalid value.");
					}
			break;
			#endregion
			#region Business Development
			case 2:
				bd					 = new bizdev	{	
													business_unit		= bizdev_ddl_branch.SelectedValue == "" ? new NeBusinessUnit() : new NeBusinessUnit(bizdev_ddl_branch.SelectedValue),
													hours		= Toolbox.ReturnZeroIfNull_double(bizdev_tb_hours.Text),
												 	calls		= Toolbox.ReturnZeroIfNull_double(bizdev_tb_calls.Text), 
												 	meetings	= Toolbox.ReturnZeroIfNull_double(bizdev_tb_meetings.Text),
												 	dropoffs	= Toolbox.ReturnZeroIfNull_double(bizdev_tb_dropoffs.Text),
												 	emails		= Toolbox.ReturnZeroIfNull_double(bizdev_tb_emails.Text),
												 	vms			= Toolbox.ReturnZeroIfNull_double(bizdev_tb_vms.Text),
												 	quoteopps	= Toolbox.ReturnZeroIfNull_double(bizdev_tb_quoteopps.Text) 
												 	};
				if(bd.business_unit.id == 0)
					{
					is_valid                           = false;
					error_text						+= error_msg("Please select a branch");
					}
				if(bd.hours == 0 || bd.hours < 0)
					{
					is_valid = false;
					error_text += error_msg("Hours are blank or they are in invalid value.");
					}
				if(bd.calls == 0 && bd.meetings == 0 && bd.dropoffs == 0 && bd.emails == 0 && bd.vms == 0 && bd.quoteopps == 0)
					{
					is_valid = false;
					error_text += error_msg("Please fill out at least one of the following boxes: calls, meetings, drop offs, emails, vms or quote opps");
					}
			break;
			#endregion
			#region Shop time
			case 3:
				shop				 = new shoptime { 
													type_id		= Toolbox.ReturnBlankIfNull_string(shop_ddl_shoptype.SelectedValue),
													hours		= Toolbox.ReturnZeroIfNull_double(shop_tb_hours.Text),
													comments	= Toolbox.ReturnBlankIfNull_string(shop_tb_comment.Text)
													};
				if(shop.type_id == "0")
					{
					is_valid                           = false;
					error_text						+= error_msg("Please select a shop time type");
					}
				double.TryParse(shop_tb_hours.Text, out hours);
				if(shop.hours == 0 || shop.hours < 0)
					{
					is_valid = false;
					error_text += error_msg("Hours are blank or they are in invalid value.");
					}
				if (shop.comments.Length < 10)
					{
					is_valid = false;
					error_text += error_msg("Please enter A Detailed Comment Greater Than 10 Characters.");
					}
			break;
			    #endregion
			   
        }

		#endregion Validation
		var bs = CheckOpenPayperiod();
		if(!bs.success)
			{
			is_valid = false;
			error_text = error_msg(bs.message);
			}
		if (is_valid)
			{
			#region Variable Definition / Partial error checking
			var commentid                  = 0;
			var commentparentid             = 0;
			var is_linked_wo               = false;
			var strparent_dsn              = "";
			var intparent_business_unit_id           = 0;
			str_member_id_for_error             = _current_user.id.ToString();
			var mt_obj             = is_editing ? new NeMemberTime(Convert.ToInt32(Session["mob_working_membertime_id"])) : new NeMemberTime();
			var comm_obj			= is_editing ? new NeWOComments(mt_obj.WoCommentID) : new NeWOComments();
			var mysql_date				= Toolbox.MySQL_shortdt(Convert.ToDateTime(date.Text));
			if (mv_tabs.ActiveViewIndex == 0 && this_wo.parent_woprog_id > 1)
				{
				is_linked_wo              = true;
				var temp_customer			= new NECustomer((int) this_wo.WOProg_Customer_ID);
				var isnecompany				= temp_customer.IsNEcompany ? temp_customer.NEbusiness_unit_id : 0;
				if(isnecompany > 0 && isnecompany != _current_user.business_unit_id)
					{
					strparent_dsn			= new NeBusinessUnit().GetBUDSN(isnecompany);
					if (strparent_dsn == "")
						{
						is_linked_wo = false;
						error_text				+= error_msg("The Parent Company Does not Have a Valid BV Company Number");
						}
					}
				}
			if(mt_obj.business_unit_id == 0)
				{
				if(mv_tabs.ActiveViewIndex == 0)
					{
					mt_obj.business_unit_id = Convert.ToInt32(wo_ddl_business_unit.SelectedValue);
					}
				else
					{
					mt_obj.business_unit_id = _current_user.business_unit_id;
					}
				}
			var labour_master_id = 0;
			try
			    {
			    labour_master_id = bu.allow_unlinked_timesheet ? 0: _tools.getSQL_int(@"SELECT id from membertype_chargeout WHERE membertype_id = @v0  AND business_unit_id = @v1  AND paytype_id = @v2 ", new object[] {  _current_user.MemberTypeID, mt_obj.business_unit_id, wo_ddl_hourtype.SelectedValue } );
				}
			catch
				{
				var em			= new NeEMail();
				em.Subject			= "NOTICE: Blank charge out for the member type '"+_current_user.membertype.name+"'";
				em.Body				= string.Format("{0} just tried to enter time, but the charge out for their member type is zero... please address this ASAP.", _current_user.FullName);
				var bm_email		= _current_user.business_unit.branch_manager.NEEmail;
				em.To				= bm_email == "" ? "orgdev@" + Toolbox.app_setting("DomainForEmail") : bm_email;
				em.CC				= "orgdev@" + Toolbox.app_setting("DomainForEmail");
				//em.Bcc				= "igalbraith@" + Toolbox.app_setting("DomainForEmail");
				em.isHTML			= false;
				em.From				= "it@" + Toolbox.app_setting("DomainForEmail");
				em.Send();
				error_text			+= error_msg("Your member type is not set up with a charge out rate. Until this is corrected, you will not be able to enter time. A notice has been emailed to your branch manager to set this up.");
				}
			#endregion Variable Definition / Partial error checking
			
			var comments							= "";
			var	tev		= new NeMemberTime.time_entry_vars();
			  
                switch (mv_tabs.ActiveViewIndex)
				{
				case 0: // WO
					tev.selected_business_unit_id = Convert.ToInt32(wo_ddl_business_unit.SelectedValue);
					tev.entrytype		= 1;
					tev.comment			= wo_tb_comment.Text;
				    if (!bu.allow_unlinked_timesheet)
				        {
				        var temp_customer = new NECustomer((int) this_wo.WOProg_Customer_ID);
				        tev.customer_number = temp_customer.Customer_Number;
				        tev.customer_name = temp_customer.Customer_Name;
				        tev.wo = this_wo;
                    }
				    else
				        {
				        tev.customer_name = wo_txt_customer.Text;
				        tev.customer_number = "0";

				        }
				   
					if (!bu.allow_unlinked_timesheet && is_linked_wo)
						{
						linked_wo						= new NeWOProg(this_wo.parent_woprog_id);
						tev.parent_wo					= linked_wo;
						}
					tev.hours			= Convert.ToDouble(wo_tb_hours.Text);
					tev.paytype_id		= Convert.ToInt32(wo_ddl_hourtype.SelectedValue);
					tev.percent_done	= Convert.ToDouble(quote_ddl_percent_complete.SelectedValue);
					tev.woprog_bvwo		= bu.allow_unlinked_timesheet? wo_txt_workorder.Text : this_wo.OrderNumber;
					tev.sell_price = 0;
					tev.cost_price = 0;
					tev.rating = Convert.ToInt16(rc_rating.Value);
				break;
				case 1: // Quote
					tev.selected_business_unit_id = _current_user.business_unit_id;
					tev.entrytype				= 2;
					tev.comment					= "Quoted Job.  See Quote Manager for details.";
					tev.customer_number			= bu.allow_unlinked_timesheet ? "0": quote_ddl_customer.SelectedValue;
					tev.customer_name			= bu.allow_unlinked_timesheet ? quote_tb_customer.Text: quote_ddl_customer.SelectedItem.Text.Trim();
					tev.hours					= Convert.ToDouble(quote_tb_hours.Text);
					tev.paytype_id				= 1;
					tev.percent_done			= Convert.ToDouble(quote_ddl_percent_complete.SelectedValue);
					tev.woprog_bvwo				= bu.allow_unlinked_timesheet ? quote_tb_quote.Text : quote_ddl_quote.SelectedValue;
					tev.sell_price = 0;
					tev.cost_price = bu.allow_unlinked_timesheet ? 0 : new NeWODetailCurrent().FindTrueLabourCost(_current_user.id, tev.paytype_id, tev.selected_business_unit_id);
					tev.rating = Convert.ToInt16(rc_rating.Value);
				break;
				case 2: // Biz Dev
					tev.selected_business_unit_id = _current_user.business_unit_id;
					tev.entrytype		= 3;
					tev.comment			= string.Format(@"Calls: {0}  Meetings: {1}  Dropoffs: {2}  Emails: {3}  VMs: {4}  QUOTEOPPs: {5}", 
															bd.calls,		// {0}
															bd.meetings, 	// {1}
															bd.dropoffs, 	// {2}
															bd.emails, 		// {3}
															bd.vms, 		// {4}
															bd.quoteopps	// {5}
															);
					tev.customer_number	= shop_ddl_shoptype.SelectedValue;
					tev.customer_name	= bd.business_unit.name;
					tev.hours			= bd.hours;
					tev.paytype_id		= 1;
					tev.percent_done	= 100;
					tev.woprog_bvwo		= "0";
					tev.cost_price = bu.allow_unlinked_timesheet ? 0 : new NeWODetailCurrent().FindTrueLabourCost(_current_user.id, tev.paytype_id, tev.selected_business_unit_id);
					tev.rating = Convert.ToInt16(rc_rating.Value);
				break;
				case 3: // Shop
					tev.selected_business_unit_id = _current_user.business_unit_id;
					tev.entrytype		= 4;
					tev.comment			= shop.comments;
					tev.customer_number	= shop_ddl_shoptype.SelectedValue;
                    tev.customer_name	= _tools.getSQL_string(@"select type from membertime_shop_type where id=@v0 ", new object[] {  tev.customer_number } );
					tev.hours			= Convert.ToDouble(shop_tb_hours.Text);
					tev.paytype_id		= 1;
					tev.percent_done	= 100;
					tev.woprog_bvwo		= shop.type_id;
					tev.cost_price = bu.allow_unlinked_timesheet ? 0 : new NeWODetailCurrent().FindTrueLabourCost(_current_user.id, tev.paytype_id, tev.selected_business_unit_id);
					tev.rating = Convert.ToInt16(rc_rating.Value);
				break;
				}
			try
				{
				if(is_editing)
					{
					NeMemberTime.time_entry_edit(new NeMemberTime.time_entry_vars()
						{ 
						advancement				= "",
						do_advancement			= false,
						do_uncertainty			= false,
						bizdev_calls			= bd.calls.ToString(),
						bizdev_faxes			= bd.dropoffs.ToString(),
						bizdev_emails			= bd.emails.ToString(),
						bizdev_mailers			= bd.vms.ToString(),
						bizdev_meetings			= bd.meetings.ToString(),
						bizdev_quoteopps		= bd.quoteopps.ToString(),
						comment					= tev.comment,
						customer_name			= tev.customer_name,
						customer_number			= tev.customer_number,
						parent_business_unit_id		= tev.parent_business_unit_id,
						membertime_id			= mt_obj.MemberTimeID,
						old_hours				= mt_obj.NumberOfHours,
						parent_wo =	linked_wo,
						wo = this_wo,
						entering_user			= _current_user,
						loaded_user				= _current_user,
						date					= Toolbox.MySQL_shortdt(Convert.ToDateTime(date.Text)),
						todays_date				= Toolbox.MySQL_shortdt(Convert.ToDateTime(date.Text)),
						entrytype				= tev.entrytype,
						hours					= tev.hours,
						is_child_wo				= is_linked_wo,
						is_shop_time			= false,
						master_id				= labour_master_id,
						paytype_id				= tev.paytype_id,
						percent_done			= tev.percent_done,
						selected_comment_id		= 0,
						selected_comment_text	= "",
						selected_business_unit_id		= tev.selected_business_unit_id,
						uncertainty				= "",
						woprog_bvwo				= tev.woprog_bvwo,
						cost_price = bu.allow_unlinked_timesheet ? 0: mt_obj.MemberTime_CostPrice,
						sell_price = tev.sell_price,
						rating = tev.rating
					
						});
					}
				else
					{
                    var record = new NeMemberTime.time_entry_vars()
                    {
                        advancement = "",
                        do_advancement = false,
                        do_uncertainty = false,
                        bizdev_calls = bd.calls.ToString(),
                        bizdev_faxes = bd.dropoffs.ToString(),
                        bizdev_emails = bd.emails.ToString(),
                        bizdev_mailers = bd.vms.ToString(),
                        bizdev_meetings = bd.meetings.ToString(),
                        bizdev_quoteopps = bd.quoteopps.ToString(),
                        comment = tev.comment,
                        membertime_id = 0,
                        customer_name = tev.customer_name,
                        customer_number = tev.customer_number,
                        entering_user = _current_user,
                        loaded_user = _current_user,
                        date = Toolbox.MySQL_shortdt(Convert.ToDateTime(date.Text)),
                        todays_date = Toolbox.MySQL_shortdt(Convert.ToDateTime(date.Text)),
                        entrytype = tev.entrytype,
                        parent_wo = linked_wo,
                        wo = this_wo,
                        hours = tev.hours,
                        is_child_wo = is_linked_wo,
                        is_shop_time = false,
                        master_id = labour_master_id,
                        paytype_id = tev.paytype_id,
                        percent_done = tev.percent_done,
                        selected_comment_id = 0,
                        selected_comment_text = "",
                        selected_business_unit_id = tev.selected_business_unit_id,
                        uncertainty = "",
                        woprog_bvwo = tev.woprog_bvwo,
                        cost_price = tev.cost_price,
                        sell_price = tev.sell_price,
                        rating = tev.rating
                    };

                    NeMemberTime.time_entry_create(ref record);
					}
				}
			catch (Exception ee)
				{
				error_text		+= error_msg(string.Format("The following Error occured - {0}. ", ee.Message));
				}
			
			if(error_text == "")
				{
				if(is_editing)
					{
					Session["mob_working_membertime_id"]		= 0;
					}
				if(!pnl_entries.Visible)
					{
					btn_toggle_entries_Click(btn_toggle_entries, null);
					should_entries_button_be_shown();
					}
				else
					{
					populate_entries();
					}
				clear_panels();
				}
			}
		error_div.InnerHtml		= error_text;
		//    ddl_tabs.SelectedIndex = 0;
		 //   mv_tabs.ActiveViewIndex = 0;
		 //   filltab(0);
    }

	protected void ASPxUploadControl1_FileUploadComplete(object _sender, DevExpress.Web.FileUploadCompleteEventArgs _e)
	{
		_e.CallbackData = save_file(_e.UploadedFile);

	}
	protected string save_file(UploadedFile _uploaded_file)
	{
		var file_name = string.Empty;
		var wo = new NeWOProg(Convert.ToInt32(wo_ddl_workorder.SelectedValue));
		if ((_uploaded_file.IsValid)&&(wo_ddl_workorder.SelectedItem.Value!=""))
		{
			var files = new NeFiles();
			files.ID = wo.woprog_id;
			files.parentpage = "workorder";
			var path = files.GetProjectFolder(files.ID, files.parentpage, "Pictures", wo.business_unit_id);
			try
			{
				if (!Directory.Exists(path))
				{
					files.CreateFolder(files.ID, files.parentpage, wo.business_unit_id);
				}
			}
			catch
			{
				return "Error Saving File to Server";
			}
			path = files.GetProjectFolder(files.ID, files.parentpage, "Pictures", wo.business_unit_id);
			file_name = string.Format(@"{0}\Pictures\{1}", path, _uploaded_file.FileName);
			if (File.Exists(file_name))
				File.Delete(file_name);
			_uploaded_file.SaveAs(file_name); //uncomment this line
			return "Saved " + _uploaded_file.FileName + " to "+ Toolbox.app_setting("Domain") + ">Work Orders (" + wo.OrderNumber + ")->Project Folder->Pictures";

		}
		return "Invalid Work Order or Picture";
	}


	private class bizdev
		{
		public NeBusinessUnit business_unit { get; set; }
		public int business_unit_id { get; set; }
		public double hours { get; set; }
		public double calls { get; set; }
		public double meetings { get; set; }
		public double dropoffs { get; set; }
		public double emails { get; set; }
		public double vms { get; set; }
		public double quoteopps { get; set; }
		}
	private class shoptime
		{
		public string type_id { get; set; }
		public double hours { get; set; }
		public string comments { get; set; }
		}
	protected void wo_ddl_workorder_SelectedIndexChanged(object _sender, EventArgs _e)
		{
		if(wo_ddl_workorder.SelectedValue != "")
			{
			var temp_wo				= new NeWOProg(Convert.ToInt32(wo_ddl_workorder.SelectedValue));
			row_signoff.Visible				= NeMember.at_jobsite_today(_current_user.id, temp_wo.woprog_id, temp_wo.woprog_Address_ID);
			
			//hid_woprog_id.Value				= temp_wo.woprog_id.ToString();
			}
		}
	protected void ddl_tabs_SelectedIndexChanged(object sender, EventArgs e)
		{
		filltab(Convert.ToInt32(ddl_tabs.SelectedValue));
		}

	protected void wo_ddl_business_unit_OnSelectedIndexChanged(object _sender, EventArgs _e)
		{
		fill_wo_customer_ddl();
		fill_wo_ddl();
		}
	}		 