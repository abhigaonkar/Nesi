using System;
using System.IO;
using System.Web.UI;
using DevExpress.Web;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;
using nesi.core;
using NESI.BLL.Pages.Timesheet.JobType;
using System.Web.UI.WebControls;
using NESI.BLL.Pages.Timesheet;
using NESI.Common.Models;
using NESI.DTO.ViewModels.Page.TimeSheet;

public partial class mobile_modules_schedule : System.Web.UI.UserControl
{
	Toolbox _tools;
	NeMember current_user;

    private object woprogId { get; set; }
    private ASPxComboBox scopecontrol { get; set; }

    protected void Page_Init(object sender, EventArgs e)
	{
		


	}

	protected void Page_Load(object sender, EventArgs e)
	{

        _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(1);
	    if (!IsPostBack)
	    {
	        dv.JSProperties["cp_alert"] = "";
	        hdn_mid.Value = current_user.id.ToString();
	        if (System.DateTime.Now.Hour > 23)
	        {
	            hdn_did.Value = System.DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
	        }
	        else
	        {
	            hdn_did.Value = System.DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd");
	        }

            var jobTypeInfo = GetJobTypeInfo(current_user.business_unit_id, current_user.id);
            hdn_hourtype.Value = "0"; //  jobTypeInfo.showJobType? 1.ToString() : 0.ToString() ;

            Session["mobile_oncall_dates"] = null;

	        oncall_chooseday.SelectedDate = DateTime.Now;
	        load_oncall();
        }
	    else
	    {
	        
	    }
		
		load_page();
		
	}
	protected void load_page()
	{

		dv.DataBind();
		load_current_week_entries();


    }
	private void load_oncall()
		{
        
			var dt_oncall = _tools.getSQL_datatable(@"SELECT
business_unit.ddl_name branch,
ifnull(member.member_fullname,'Not Set') member,
membertype.membertype_name title,
if(oncall_schedule.is_backup=1,'X','') is_backup,
cellphone_number.number cell
FROM
business_unit
left JOIN oncall_schedule ON oncall_schedule.business_unit_id = business_unit.id and date(oncall_schedule.date) = '"+Toolbox.MySQL_shortdt(oncall_chooseday.SelectedDate)+ @"'
left JOIN member ON oncall_schedule.member_id = member.Member_ID 
LEFT JOIN membertype ON member.member_membertype_id = membertype.membertype_id AND member.member_membertype_id = membertype.membertype_id
LEFT JOIN cellphone_number ON member.cellphone_number_id = cellphone_number.id
where  business_unit.enable_timesheet = 1
AND FIND_IN_SET(business_unit.id, @v0)
order by business_unit.id, oncall_schedule.is_backup desc", new object[] { new Current_User().visible_business_units });

			oncall_div.InnerHtml = "<table cellpadding=3px>";
			foreach(DataRow dr in dt_oncall.Rows)
			{
				if (dr["is_backup"].ToString() == "X")
				{
					oncall_div.InnerHtml += "<tr><td colspan='4'><b>" + dr["branch"] + "</b></td></tr><tr><td></td><td><font color='red'>" + dr["member"] + "</td><td>" + dr["title"] + "</td><td nowrap><a href=tel:'" + dr["cell"] + "'>" + dr["cell"] + "</a></font></td></tr>";
				}
				else
				{
					oncall_div.InnerHtml += "<tr><td colspan='4'><b>" + dr["branch"] + "</b></td></tr><tr><td></td><td>" + dr["member"] + "</td><td>" + dr["title"] + "</td><td nowrap><a href=tel:'" + dr["cell"] + "'>" + dr["cell"] + "</a></td></tr>";
				}
		    }

		    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "document.getElementById('spacerDiv').style.height = '"+ dt_oncall.Rows.Count * 45 + "px'", true);
        oncall_div.InnerHtml += "</table></br><font color='red'> * Red indicates Back Up</font>";
		}

	protected void load_current_week_entries()
	{
		var ci = System.Threading.Thread.CurrentThread.CurrentCulture;
		var fdow = ci.DateTimeFormat.FirstDayOfWeek;
		var start = DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - fdow));
		var Div0 = (HtmlContainerControl)cb_header.FindControl("Div0");
		var Div1 = (HtmlContainerControl)cb_header.FindControl("Div1");
		var Div2 = (HtmlContainerControl)cb_header.FindControl("Div2");
		var Div3 = (HtmlContainerControl)cb_header.FindControl("Div3");
		var Div4 = (HtmlContainerControl)cb_header.FindControl("Div4");
		var Div5 = (HtmlContainerControl)cb_header.FindControl("Div5");
		var Div6 = (HtmlContainerControl)cb_header.FindControl("Div6");
	
		Div0.InnerHtml = "Sun</br>";
		Div0.InnerHtml += NeMemberTime.get_entered_time(current_user.id, start.AddDays(0));
		Div1.InnerHtml = "Mon</br>";
		Div1.InnerHtml += NeMemberTime.get_entered_time(current_user.id, start.AddDays(1));
		Div2.InnerHtml = "Tue</br>";
		Div2.InnerHtml += NeMemberTime.get_entered_time(current_user.id, start.AddDays(2));
		Div3.InnerHtml = "Wed</br>";
		Div3.InnerHtml += NeMemberTime.get_entered_time(current_user.id, start.AddDays(3));
		Div4.InnerHtml = "Thu</br>";
		Div4.InnerHtml += NeMemberTime.get_entered_time(current_user.id, start.AddDays(4));
		Div5.InnerHtml = "Fri</br>";
		Div5.InnerHtml += NeMemberTime.get_entered_time(current_user.id, start.AddDays(5));
		Div6.InnerHtml = "Sat</br>";
		Div6.InnerHtml += NeMemberTime.get_entered_time(current_user.id, start.AddDays(6));

	}
	

	

	protected Color GetColor(int status, int quote_id)
	{
		if (status > 100)
			return Color.LimeGreen;
		if (status == 0)
		{
			if (quote_id == 0)
			{
				return Color.Green;
			}
			else
			{
				return ColorTranslator.FromHtml("#993300");
			}
		}
		if ((status > 50) && (status < 60))
		{
			return Color.Yellow;
		}
		if (status == 500)
		{
			return Color.LightYellow;
		}
		return Color.Transparent;
	}

	protected string error_msg(string text)
	{
		return string.Format(@"<b class='item'>&bullet; {0}</b></br>", text);
	}

	protected string GetTopDateFormat(int oncall)
	{
		return oncall == 1 ? "'!! ON CALL ' ddd, MMM dd" : "ddd, MMM dd";
		
	}

	protected Color GetfColor(int status, int quote_id)
	{
		if (status > 100)
			return Color.Black;
		if (status == 0)
				return Color.GhostWhite;
		if ((status > 50) && (status < 60))
		{
			return Color.Black;
		}

		return Color.Black;
	}
	

	protected bool Getvis(int status)
	{
		if (status > 100)
			return false;
		if (status == 0)
			return true;
		if ((status > 50) && (status < 60))
		{
			return true;
		}

		return false;
	}
	protected bool Getsavevis(DateTime _date)
	{
		if (_date.Date > System.DateTime.Today.Date)
			return false;
		if (_date.Date >= System.DateTime.Today.AddDays(-1))
			return true;
		if (_date.Date < System.DateTime.Today.AddDays(-1))
		{
			return false;
		}
		return false;
	}

	protected bool Get_Page_Enable(object wo_status, object wo_isonhold, object woId)
	{
        //
        // Warning should be reset when scheduler walking to 'Today'. (https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1656)
        //
        dv.JSProperties["cp_disable_page"] = "";

        GetScope(woId);

        if (wo_status.ToString() != OpsWOStatus.Open && wo_status.ToString() != "Waiting for Parts")
		{
			dv.JSProperties["cp_disable_page"] = "This work order is not open to time entries anymore";
			return false;
		}
		else if (wo_isonhold.ToString() == "1")
		{
			dv.JSProperties["cp_disable_page"] = "This work order is on hold";

			return false;
		}

		return true;
	}

    private void GetScope(object woId)
    {
        this.woprogId = woId;

        if (this.woprogId != null && !string.IsNullOrWhiteSpace(this.woprogId.ToString()))
        {
            var id = 0;
            if (int.TryParse(this.woprogId.ToString(), out id))
            {

                this.scopecontrol.DataSource = _tools.getSQL_datatable(@"Select id, task from woprog_tasks  where woprog_id =@v0 order by _order,id", new object[] { id });
                this.scopecontrol.DataBind();
            }
        }
    }

	protected bool Getphotovis(int status)
	{
		if (status > 100)
			return false;
		if (status == 0)
			return true;
		if ((status > 50) && (status < 60))
		{
			return false;
		}

		return false;
	}

	protected bool Getwovis(int woprog_id)
	{
		if (woprog_id > 0)
			return true;
		

		return false;
	}

    private string ExceedMaxHoursOnOneDay_InsertCase(int forWhow, DateTime onWhichDate, double hours, int PayTypeId)
    {
        if (PayTypeId == 8)
        {
            // mileage will not be checked by here.
            return "";
        }

        NESI.BLL.Pages.Timesheet.Timesheet ts = new NESI.BLL.Pages.Timesheet.Timesheet();
        CumulativeParameter parameter = new CumulativeParameter();
        parameter.OnWhichDate = onWhichDate;
        parameter.ForWho = forWhow;
        parameter.action = TimesheetAction.Add;
        parameter.newValue = hours;
        parameter.oldValue = 0;

        var result = ts.GetCumulativeInformation(parameter);
        if (result.DoesItExceedMaxValue)
        {
            return result.Errors[0].error;
        }
        else
        {
            // Not Exceeds
            return "";
        }
    }

    protected void dv_CustomCallback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		
		try
		{
		if (e.Parameter.Substring(0,2)=="ts")
		{
            //
            // JobType info getting from here.
            //
            bool showJobType = false;
            int selectedJobType = Convert.ToInt32(current_user.MemberTypeID);
            if ( !string.IsNullOrWhiteSpace(this.hdn_showJobType.Value) &&
                  this.hdn_showJobType.Value.ToLowerInvariant() == "true")
            {
                showJobType = true;
                var list = e.Parameter.Split('|');
                if (list.GetValue(5) != null)
                {
                    selectedJobType = Convert.ToInt32(e.Parameter.Split('|').GetValue(5));
                }
            }
            //
            // end of here...
            //

			var id = e.Parameter.Split('|').GetValue(1).ToString();
			var apt = new NeAppointment(Convert.ToInt32(id));
			var mt_obj = new NeMemberTime();
			var comm_obj = new NeWOComments();
			var cust_obj = new NECustomer();
			var mem_ts = new ASPxMemo();
            var scope_id = e.Parameter.Split('|').GetValue(6);

            mem_ts.Text = e.Parameter.Split('|').GetValue(2).ToString();

			//ASPxComboBox ddl_pt = new ASPxComboBox();
			var pt = Convert.ToInt32(e.Parameter.Split('|').GetValue(3).ToString());
                if (scope_id.ToString() == "null")
                {
                    scope_id = 0;

                }
                else
                {
                    scope_id = Convert.ToInt32(e.Parameter.Split('|').GetValue(6).ToString());
                }
                //ddl_pt.Value = Convert.ToInt32(e.Parameter.Split('|').GetValue(3).ToString());
                var sum_ts = apt.EndTime.Subtract(apt.StartTime).TotalHours;
			sum_ts = sum_ts > 8 ? sum_ts - 0.5 : sum_ts;
			var tb_hours = Convert.ToDouble(e.Parameter.Split('|').GetValue(4));

			var mysql_date = Toolbox.MySQL_shortdt(Convert.ToDateTime(apt.StartTime));
			var error_text = "";
			var commentid = 0;
			var commentchildid = 0;
			var has_parent_wo = false;
			var strChildDSN = "";
			var intChildCompanyID = (int) 0;
			var intFirstPart = 0;
			if (apt.woprog_id != 0)
			{
				#region wo entry
				if (apt.Status == 0)
				{
					var wo = new NeWOProg(apt.woprog_id);
					cust_obj = new NECustomer((int) wo.WOProg_Customer_ID);
					var branch = new NeBusinessUnit(wo.business_unit_id);
					if (wo.Status != OpsWOStatus.Open)
					{
						throw new Exception("This work order is not set to 'Open' status");// throw back an error that the work order is closed
					}
					else if (wo.woprog_hold == 1)
					{
						throw new Exception("This work order has been put on hold so time canot be put against it.");// throw back an error that the work order is closed
					}
					else  // ok, so lets make the entry
					{
					    if (!current_user.business_unit.allow_unlinked_timesheet)
					    {
					        var existingAtiveWageRecords = this.ExistingActiveWageRecords(current_user.id);
					        if (existingAtiveWageRecords == 0)
					        {
					            throw new Exception("There isn't an active wage record for the selected user");
					        }
					    }

					    var parent_wo = new NeWOProg();
						#region parent child work order
						if (wo.parent_woprog_id != 0)
						{
							parent_wo = new NeWOProg(wo.parent_woprog_id);
							intChildCompanyID = parent_wo.business_unit_id;
							has_parent_wo = true;
						}

						#endregion
						#region grab chargeout
						var labour_master_id = 0;
						try
						{
							labour_master_id = _tools.getSQL_int(@"SELECT id from membertype_chargeout WHERE membertype_id = @v0  AND business_unit_id = @v1  AND paytype_id = @v2 ", new object[] { selectedJobType, wo.business_unit_id, pt } );
						}
						catch
						{
							var em = new NeEMail();
							em.Subject = "NOTICE: Blank charge out for the member type '" + current_user.membertype.name + "'";
							em.Body = string.Format("{0} just tried to enter time, but the charge out for their member type is zero... please address this ASAP.", current_user.FullName);
							var bm_email = current_user.business_unit.branch_manager.NEEmail;
							em.To = bm_email == "" ? "it@" + Toolbox.app_setting("DomainForEmail") : bm_email;
							em.CC = "orgdev@" + Toolbox.app_setting("DomainForEmail");
						//	em.Bcc = "igalbraith@" + Toolbox.app_setting("DomainForEmail");
							em.isHTML = false;
							em.From = "it@" + Toolbox.app_setting("DomainForEmail");
							//			em.Send();
						}
						#endregion

                        //
                        // Validation on chargeout & id
                        //
                        var allowUnlinkTimesheet = current_user.business_unit.allow_unlinked_timesheet;
                        var error = @"This member type is not set up with a charge out rate in the selected business unit. <br/>
Until this is corrected, you will not be able to enter time on work orders in this business unit.";
                        if (labour_master_id == 0 && !allowUnlinkTimesheet)
                        {
                            dv.DataBind();
                            dv.JSProperties["cp_alert"] = error;
                            return;
                        }

                        if (labour_master_id != 0 && !allowUnlinkTimesheet)
                        {
                            var charge_out_rate = Toolbox.doSQL_double(@"CALL CUSTOMER_CHARGEOUT(@v0 ,@v1 )", new object[] { wo.WOProg_Customer_ID, labour_master_id });
                            if (charge_out_rate == 0)
                            {
                                dv.DataBind();
                                dv.JSProperties["cp_alert"] = error;
                                return;
                            }
                        }

                        if (has_parent_wo)
                        {
                            var parent_temp_chargeout_id = _tools.getSQL_int(@"SELECT IFNULL(MAX(id), 0) FROM membertype_chargeout where membertype_id = @v0  and paytype_id = @v1  and business_unit_id = @v2 ", new object[] { selectedJobType, pt, parent_wo.business_unit_id });
                            if (parent_temp_chargeout_id == 0)
                            {
                                dv.DataBind();
                                dv.JSProperties["cp_alert"] = error;
                                return;
                            }

                            var temp_chargeout = _tools.getSQL_double(@"CALL customer_chargeout(@v0 ,@v1 )", new object[] { parent_wo.WOProg_Customer_ID, parent_temp_chargeout_id });
                            if (temp_chargeout == 0)
                            {
                                dv.DataBind();
                                dv.JSProperties["cp_alert"] = error;
                                return;
                            }

                        }
                        //
                        // end of Validation on chargeout & id
                        //

                        // Limit daily cumulative time entry to 24 hours
                        var errInfo = ExceedMaxHoursOnOneDay_InsertCase(current_user.id, Convert.ToDateTime(mysql_date), tb_hours, Convert.ToInt32(pt));
                        if (!string.IsNullOrWhiteSpace(errInfo))
                        {
                            dv.DataBind();
                            dv.JSProperties["cp_alert"] = errInfo;
                            return;
                        }

                        #region comments
                        comm_obj.WorkOrderID = wo.OrderNumber;
						comm_obj.WoComment_Member_ID = current_user.id;
						comm_obj.Comments = mem_ts.Text;
						comm_obj.HoursWorked = tb_hours.ToString("c2");

						comm_obj.Personal = 0;
						comm_obj.PrintComments = 0;
						comm_obj.business_unit_id = wo.business_unit_id;
						comm_obj.CreatedDate = mysql_date;
						comm_obj.woprog_id = wo.woprog_id;
						comm_obj.EntryDate = mysql_date;
						try
						{
							comm_obj.AddWOComment(current_user.id, 0, out commentid, out commentchildid, false, comm_obj.woprog_id);
						}
						catch (Exception ex)
						{
							_tools.catch_error(ex);
							error_text += error_msg("Error Adding to DB.<br/><pre>" + ex.Message + "</pre>");
						}
						finally
						{
							mt_obj.MemberTimeWoCommentID = commentid;
							if (has_parent_wo)
							{
								mt_obj.MemberTimeWoCommentChildID = commentchildid;
							}
						}
						#endregion comments
						#region Save MemberTime Row
						try
						{
							#region Add MemberTime Line
							mt_obj.MemberTime_Premium = "false";
							mt_obj.Date = mysql_date;
							mt_obj.MemberTimeCustomerName = wo.CustomerName;
							mt_obj.MemberTimeWorkOrderID = wo.OrderNumber;
							mt_obj.membertime_woprog_id = wo.woprog_id;
							mt_obj.MemberTime_Cust_No = cust_obj.Customer_Number;
							mt_obj.WOType = "WO";
							//						mt_obj.wo_percent_complete = Convert.ToDouble(wo_ddl_percent_complete.SelectedValue);
							var temp_numberofhours = mt_obj.NumberOfHours;
							mt_obj.NumberOfHours = tb_hours;
							mt_obj.MemberTimePayTypeHoursID = Convert.ToInt32(pt);
							mt_obj.membertime_memberid = apt.member_id;
							mt_obj.MemberIDAudit = current_user.id;
							mt_obj.membertype_id					= selectedJobType; // Convert.ToInt32(current_user.MemberTypeID);
							mt_obj.membertype_chargeout_id			= labour_master_id;
							mt_obj.CreatedDate = mysql_date;
							mt_obj.MemberIDCreate = current_user.id;
							mt_obj.business_unit_id = wo.business_unit_id;
							if (has_parent_wo)
							{
								mt_obj.MemberTime_ChildCompanyID = intChildCompanyID;
								mt_obj.MemberTimeChildWorkOrderID = parent_wo.OrderNumber;
								mt_obj.MemberTimeChildCustomerName = parent_wo.CustomerName.Trim();
								mt_obj.membertime_child_woprog_id = parent_wo.woprog_id;
							}
							mt_obj.MemberTime_Mileage = "false";
							mt_obj.ModifiedDate = Toolbox.MySQLNow_long();
							mt_obj.MemberTime_Premium = "false";
							mt_obj.MemberTime_SRED = "false";
							mt_obj.MemberTime_Warranty = "false";
							mt_obj.MemberIDCreate = current_user.id;
							mt_obj.customer_id = wo.WOProg_Customer_ID;
                            mt_obj.scope_id = Convert.ToInt32(scope_id);

                            /*
                             * 1306: Membertime cost is zero for some entries.
                             * Using the same logic to get the costPrice before add a timesheet.
                             */
                            var allowUnlink = current_user.business_unit.allow_unlinked_timesheet;
                            var costPrice = allowUnlink ? 0 : FindTrueLabourCost(current_user.id, mt_obj.MemberTimePayTypeHoursID, current_user.business_unit_id);
						    mt_obj.MemberTime_CostPrice = costPrice;

                            //
                            // Bug 1887: Sell Price on time record is 0
                            //
                            // Mobile page checks - have a chargeout.
                            // 

                            // Moved line code from below 20 lines, so it can be called one time but used for multiple times.
                            var charge_out_rate = labour_master_id != 1000000 ? wo.use_fixed_labour_rate ? wo.fixed_labour_rate : _tools.getSQL_double(@"CALL customer_chargeout(@v0 ,@v1 )", new object[] { wo.WOProg_Customer_ID, labour_master_id }) : 0;

                            mt_obj.MemberTime_SellPrice = charge_out_rate;



                                //						if (is_editing)
                                //						{
                                //							mt_obj.UpdateMemberTime();
                                //						}
                                //						else
                                //						{
                                mt_obj.AddMemberTime(current_user.id);
							//						}
							#endregion
							NeWOProg.RefreshAndSaveServiceDatesText(mt_obj.membertime_woprog_id);
							#region variable definition
                            // Move below one line code UP, so comment out below one line.
							// var charge_out_rate = labour_master_id != 1000000 ? wo.use_fixed_labour_rate? wo.fixed_labour_rate: _tools.getSQL_double(@"CALL customer_chargeout(@v0 ,@v1 )", new object[] {  wo.WOProg_Customer_ID, labour_master_id } ) : 0;
							var Paytype = _tools.getSQL_string(@"Select description from paytypehours  where PayTypeHours_ID =@v0", new object[] { pt });
							var master_id = labour_master_id;
							var detail_obj = new NeWODetailCurrent();
							var EmpTagInv = new inventory();
							var WorkOrderDetails = new DataTable();
							var LastLabour = 0;
							var LastQuote = 0;
							var ChildWorkOrderDetails = has_parent_wo ? _tools.getSQL_datatable(@"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  ORDER BY wo_detail_current_rec_no", new object[] {  parent_wo.woprog_id } ) : new DataTable();
							var strLabor = branch.country == "CDN" ? "Labour" : "Labor";
							WorkOrderDetails = _tools.getSQL_datatable(@"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  ORDER BY wo_detail_current_rec_no", new object[] {  mt_obj.membertime_woprog_id } );
							var temp_chargeout_id = _tools.getSQL_int(@"SELECT IFNULL(MAX(id), 0) FROM membertype_chargeout where membertype_id = @v0  and paytype_id = @v1  and business_unit_id = @v2 ", new object[] {  selectedJobType, pt, mt_obj.business_unit_id } );
                            var copyBeforeUpdate = "";

							var partline = detail_obj.GetTimeSheetPartLine(current_user.id, mt_obj.MemberTimePayTypeHoursID, mt_obj.membertime_woprog_id, temp_chargeout_id);
							if (partline.Rows.Count == 1)
							{
							detail_obj		= new NeWODetailCurrent(Convert.ToInt32(partline.Rows[0]["wo_detail_current_id"]));
							}
							if(detail_obj.id == 0)
								{
								var temp_qty = detail_obj.qty_committed;
								var temp_hours = tb_hours;
								var used_qty = temp_hours;
								var already_exists = _tools.getSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] {  mt_obj.membertime_woprog_id, master_id } );
								var current_billtypeid = already_exists > 0 ? _tools.getSQL_int(@"SELECT IFNULL(MAX(wo_detail_current_billtypeid), 0) id FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] {  mt_obj.membertime_woprog_id, master_id } ) : 0;
								var current_cost = detail_obj.FindTrueLabourCost(current_user.id, Convert.ToInt32(pt), mt_obj.business_unit_id);
								var current_sell = already_exists > 0 ? _tools.getSQL_double(@"SELECT IFNULL(MAX(wo_detail_current_price_sell), 0) sell FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] {  mt_obj.membertime_woprog_id, master_id } ) : 0;
								var current_unit = already_exists > 0 ? _tools.getSQL_double(@"SELECT IFNULL(MAX(wo_detail_current_price_unit), 0) unit FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] {  mt_obj.membertime_woprog_id, master_id } ) : 0;

								detail_obj.added_by = current_user.id;
								detail_obj.date_added = Toolbox.MySQLNow_long();
								detail_obj.date_modified = Toolbox.MySQLNow_long();
								detail_obj.description = string.Format("{0} Hours {1}: {2}", current_user.FullName, strLabor, Paytype);

                                //
                                // Code change by https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1983/
                                //
                                copyBeforeUpdate = detail_obj.description;
                                var setting = NeMemberTime.WoLineOnlyJobType(branch.id);
                                if (setting)
                                {
                                    var jobType = NeMemberTime.JobTypeName(selectedJobType);
                                    detail_obj.description = string.Format("{0} Hours {1}: {2}", jobType, strLabor, Paytype);
                                }

                                detail_obj.master_id = master_id;
								detail_obj.memberid = mt_obj.membertime_memberid;
								detail_obj.paytypeid = Convert.ToInt32(mt_obj.MemberTimePayTypeHoursID);

								detail_obj.cost = current_cost;
								detail_obj.sell = wo.use_fixed_labour_rate ? wo.fixed_labour_rate : already_exists > 0 ? current_sell : charge_out_rate;
								detail_obj.unit = wo.use_fixed_labour_rate ?  wo.fixed_labour_rate :  already_exists > 0 ? current_unit : charge_out_rate;

								detail_obj.tax1 = branch.TaxLabour == 1 ? wo.woprog_tax1 : 0;
								detail_obj.tax2 = branch.TaxLabour == 1 ? wo.woprog_tax2 : 0;
								detail_obj.tax3 = branch.TaxLabour == 1 ? wo.woprog_tax3 : 0;
								detail_obj.tax4 = branch.TaxLabour == 1 ? wo.woprog_tax4 : 0;
								detail_obj.woprog_id = mt_obj.membertime_woprog_id;
								detail_obj.bvwo = Convert.ToInt32(wo.OrderNumber);
								detail_obj.business_unit_id = branch.id;
								detail_obj.type = "L";
								detail_obj.code = labour_master_id.ToString();
								detail_obj.origin = "Entered From Timesheet";
								detail_obj.issues = "";

								if (wo.QuoteID == "0")
									{
									detail_obj.billtypeid = already_exists > 0 ? current_billtypeid : 0;
									}
								else
									{
									detail_obj.billtypeid = already_exists > 0 ? current_billtypeid : 1;
									detail_obj.tax1 = 0;
									detail_obj.tax2 = 0;
									detail_obj.tax3 = 0;
									detail_obj.tax4 = 0;
									}
								}
							detail_obj.qty_committed = tb_hours;
							detail_obj.qty_invoiced = tb_hours;
							detail_obj.qty_ordered	= tb_hours;

                            #endregion variable definition
                            #region Tries to update wo_detail_current, returns rows affected to test if an insert is needed


                            // Get a copy of timesheet record after inserted.
                            var postRecord = new NeMemberTime(mt_obj.MemberTimeID);
                            var previous = new NeWODetailCurrent(detail_obj.id);

                            // Any committed change to timesheet record may trigger an update on lablor line item.
                            // First two parameters describes this changes.
                            // Previous states the labor line before change happens.
                            // detail_obj states the 'To be changed value'
                            // null state that not call for parent work order.
                            NeMemberTime.WOCostVisibility(null, postRecord, previous, detail_obj, null);


                            detail_obj.save(current_user, "schedule.ascx.cs - dv_CustomCallback #1", false);

							// The user's charge out for this branch... if there isn't one... use the sell.


							if (has_parent_wo)
							{
								if (wo.QuoteID == "0")
								{
									var childLineDetail = new NeWODetailCurrent(detail_obj.id);
									temp_chargeout_id = _tools.getSQL_int(@"SELECT IFNULL(MAX(id), 0) FROM membertype_chargeout where membertype_id = @v0  and paytype_id = @v1  and business_unit_id = @v2 ", new object[] { selectedJobType, pt, parent_wo.business_unit_id } );
									var temp_chargeout = temp_chargeout_id > 0
																	? _tools.getSQL_double(@"CALL customer_chargeout(@v0 ,@v1 )", new object[] {  parent_wo.WOProg_Customer_ID, temp_chargeout_id } )
																	: detail_obj.sell;
									var parent_company = new NeBusinessUnit(intChildCompanyID);
									var childpartline = detail_obj.GetTimeSheetPartLine(current_user.id, mt_obj.MemberTimePayTypeHoursID, mt_obj.membertime_child_woprog_id, temp_chargeout_id);
									detail_obj.id = childpartline.Rows.Count == 1 ? Convert.ToInt32(childpartline.Rows[0]["wo_detail_current_id"]) : 0;
									detail_obj.billtypeid = parent_wo.QuoteID == "0" ? 0 : 1;  // Change billtype
									detail_obj.tax1 = parent_company.TaxLabour == 1 && detail_obj.billtypeid != 1 ? parent_wo.woprog_tax1 : 0;
									detail_obj.tax2 = parent_company.TaxLabour == 1 && detail_obj.billtypeid != 1 ? parent_wo.woprog_tax2 : 0;
									detail_obj.tax3 = parent_company.TaxLabour == 1 && detail_obj.billtypeid != 1 ? parent_wo.woprog_tax3 : 0;
									detail_obj.tax4 = parent_company.TaxLabour == 1 && detail_obj.billtypeid != 1 ? parent_wo.woprog_tax4 : 0;
									detail_obj.master_id = temp_chargeout_id;
									detail_obj.woprog_id = parent_wo.woprog_id;
									detail_obj.business_unit_id = parent_company.id;
									detail_obj.cost = detail_obj.sell;
									detail_obj.sell = parent_wo.use_fixed_labour_rate ? parent_wo.fixed_labour_rate : temp_chargeout;
									detail_obj.unit = detail_obj.sell;
									detail_obj.bvwo = Convert.ToInt32(parent_wo.OrderNumber);
									var crecno = detail_obj.GetRecordNumber(mt_obj.business_unit_id.ToString(), mt_obj.MemberTimePayTypeHoursID.ToString(), mt_obj.membertime_child_woprog_id.ToString());
									if (crecno != "")
									{
										try
										{
											detail_obj.rec_no = Convert.ToInt32(crecno);
										}
										catch
										{
										}
									}

                                    //
                                    // Code change by https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1983/
                                    //
                                    var setting = NeMemberTime.WoLineOnlyJobType(parent_company.id);
                                    if (setting)
                                    {
                                        var jobType = NeMemberTime.JobTypeName(selectedJobType);
                                        detail_obj.description = string.Format("{0} Hours {1}: {2}", jobType, strLabor, Paytype);
                                    }
                                    else
                                    {
                                        // Not setting, then use the one from original.
                                        detail_obj.description = copyBeforeUpdate;
                                    }

                                    // Call for parent workk order. Here reuse the detail_obje but different values.
                                    var previousParent = new NeWODetailCurrent(detail_obj.id);
                                    NeMemberTime.WOCostVisibility(null, postRecord, previousParent, detail_obj, childLineDetail);

                                    detail_obj.save(current_user, "schedule.ascx.cs - dv_CustomCallback #2", false);
								}
							}

							#endregion Tries to update wo_detail_current, returns rows affected to test if an insert is needed


							#region Update Labour Totals
							NeWOProg.update_header_totals(mt_obj.membertime_woprog_id.ToString(), branch.id, wo.OrderNumber);
							if (has_parent_wo)
							{
								NeWOProg.update_header_totals(parent_wo.woprog_id.ToString(), intChildCompanyID, parent_wo.OrderNumber);
							}

							#endregion Update Labour Totals
						}
						catch (Exception ex)
						{
							_tools.catch_error(ex);

							error_text += error_msg(string.Format("An error occured when saving. Please try again later.<br/> <pre>{0}</pre>", ex));
							Toolbox.FriendlyException(Response, error_text, @"index.aspx?a=scheduler");
							throw (new Exception(error_text));
						}
						#endregion Save MemberTime Row
					}
				}
				#endregion
			}
			else if (apt.quote_id != 0)
			{
					 
					#region Quotes
			
					#region comments
				comm_obj.WorkOrderID = apt.quote_id.ToString();
					comm_obj.WoComment_Member_ID  = current_user.id;
					comm_obj.HoursWorked = tb_hours.ToString();
					comm_obj.Comments			  = "Quoted Job.  See Quote Manager for details.";
					comm_obj.Personal             = 0;
					comm_obj.PrintComments        = 0;
					comm_obj.business_unit_id           = current_user.business_unit.id;
					comm_obj.CreatedDate          = mysql_date;
					comm_obj.woprog_id			  = 0;
					comm_obj.EntryDate = mysql_date;
					try
						{
							comm_obj.AddWOComment(current_user.id, 0, out commentid, out commentchildid, false, comm_obj.woprog_id);
						}
					catch (Exception ex)
						{
						_tools.catch_error(ex);
						error_text				+= error_msg("Error Adding to DB.<br/><pre>" + ex.Message + "</pre>");
						}
					finally
						{
						mt_obj.MemberTimeWoCommentID = commentid;
						}

					#endregion comments		
					#region Add MemberTime Line
					try
						{
						var quote = new quote(apt.quote_id);
						var labour_master_id = _tools.getSQL_int(@"SELECT id from membertype_chargeout WHERE membertype_id = @v0  AND business_unit_id = @v1  AND paytype_id = @v2 ", new object[] {  current_user.MemberTypeID, quote.business_unit_id, 1 } );

						var temp_customer			= new NECustomer(Convert.ToInt32(quote.cust_id));
						mt_obj.MemberTime_Premium			= "false";
						mt_obj.Date							= mysql_date;
						mt_obj.MemberTimeCustomerName = temp_customer.Customer_Name;
						mt_obj.MemberTimeWorkOrderID		= apt.quote_id.ToString()+quote.Revision.ToString();
						mt_obj.ProductCode					= "lblb";
						mt_obj.MemberTime_Cust_No			= temp_customer.Customer_Number;
						mt_obj.WOType						= "Quote";
						mt_obj.CustomerName					= temp_customer.Customer_Name;
						mt_obj.membertime_woprog_id			= 0;
//						mt_obj.wo_percent_complete			= Convert.ToDouble(quote_ddl_percent_complete.SelectedValue);
						mt_obj.NumberOfHours				= Convert.ToDouble(tb_hours);
						mt_obj.MemberTimePayTypeHoursID		= 1;
						mt_obj.business_unit_id			= current_user.id;
						mt_obj.MemberIDAudit				= current_user.id;
						mt_obj.membertype_id				= Convert.ToInt32(current_user.MemberTypeID);
						mt_obj.membertype_chargeout_id		= labour_master_id;
						mt_obj.CreatedDate					= mysql_date;
						mt_obj.MemberIDCreate				= current_user.id;
						mt_obj.business_unit_id			= current_user.business_unit.id;
						mt_obj.MemberTime_Mileage			= "false";
						mt_obj.MemberTime_Premium			= "false";
						mt_obj.MemberTime_SRED				= "false";
						mt_obj.MemberTime_Warranty			= "false";
						mt_obj.MemberIDCreate				= current_user.id;
						mt_obj.customer_id					= temp_customer.id;
						mt_obj.ModifiedDate					= Toolbox.MySQLNow_long();
					
						mt_obj.AddMemberTime(current_user.id);
						#region update quote hours spent
					
							_tools.getSQL_void(@"UPDATE quote_master LEFT JOIN vw_membertime_quote ON LEFT (quote_master.quote_id, 6) = vw_membertime_quote.MemberTime_WorkOrder_ID
SET quote_master.hours_spent = ifnull(vw_membertime_quote.NumberOfHours,0)
WHERE
quote_master.quote_id = @v0", new object[] { apt.quote_id});
					
						#endregion
						}
					catch (Exception ex)
						{
						_tools.catch_error(ex);
						var Error = new NEErrorReport();
//						Error.WriteErrorReport(strMemberIDForError, Session["DSNName"].ToString(), string.Format("{0} Time Sheet LINE 927", ex));
						error_text			 += error_msg(string.Format("An error occured when saving. Please try again later.<br/> <pre>{0}</pre>", ex));
						}
					#endregion				
			
				#endregion
			}
			else
			{
				if ((apt.Status > 50) && (apt.Status < 60))
				{
					#region shoptime entry



					#region comments
					comm_obj.WorkOrderID = (apt.Status - 50).ToString();
					comm_obj.WoComment_Member_ID = apt.member_id;
					comm_obj.Comments = apt.notes;
					comm_obj.HoursWorked = tb_hours.ToString();
					comm_obj.MemberIDAudit = current_user.id;
					comm_obj.business_unit_id = apt.business_unit_id;
					comm_obj.CreatedDate = mysql_date;
					comm_obj.woprog_id = 0;
					comm_obj.EntryDate = mysql_date;
					comm_obj.AddWOComment(current_user.id, 0, out commentid, out commentchildid, false, 0);

					#endregion

					mt_obj.MemberTimeWoCommentID = commentid;
					mt_obj.MemberTime_Cust_No = (apt.Status - 50).ToString();
					mt_obj.MemberTimeCustomerName = _tools.getSQL_string(@" select ifnull((SELECT type FROM membertime_shop_type WHERE id = @v0 ),'Unknown')", new object[] {  mt_obj.MemberTime_Cust_No } );
					mt_obj.WOType = "Shop";
					mt_obj.MemberTimeWorkOrderID = (apt.Status - 50).ToString();
					mt_obj.membertime_woprog_id = 0;
					mt_obj.ProductCode = "lblb";
					mt_obj.wo_percent_complete = 100;
					mt_obj.NumberOfHours = Convert.ToDouble(tb_hours);
					mt_obj.MemberTimePayTypeHoursID = 1;
					mt_obj.business_unit_id = apt.member_id;
					mt_obj.MemberIDAudit = current_user.id;
					mt_obj.Date = mysql_date;
					var	labour_master_id = _tools.getSQL_int(@"SELECT id from membertype_chargeout WHERE membertype_id = @v0  AND business_unit_id = @v1  AND paytype_id = @v2 ", new object[] {  current_user.MemberTypeID, current_user.business_unit_id, 1 } );
					mt_obj.membertype_id					= Convert.ToInt32(current_user.MemberTypeID);
					mt_obj.membertype_chargeout_id			= labour_master_id;
					mt_obj.CreatedDate = mysql_date;
					mt_obj.MemberIDCreate = current_user.id;
					mt_obj.business_unit_id = apt.business_unit_id;
					mt_obj.ModifiedDate = mysql_date;
					mt_obj.MemberTime_Mileage = "false";
					mt_obj.MemberTime_Premium = "false";
					mt_obj.MemberTime_SRED = "false";
					mt_obj.MemberTime_Warranty = "false";
					mt_obj.MemberIDCreate = current_user.id;
					try
					{
						mt_obj.AddMemberTime(apt.member_id);
					}
					catch (Exception ex)
					{
						error_text += error_msg(string.Format("The following Error occured - {0}. ", ex));
						throw (new Exception(error_text));
					}

					#endregion
				}
			}

			dv.DataBind();
				dv.JSProperties["cp_alert"] = "Time Entry Saved";
		}
	}
		catch (Exception ex1)
		{
			dv.JSProperties["cp_alert"] = ex1.Message;
			dv.DataBind();
		}
	}

	protected void ASPxUploadControl1_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
	{
		
		e.CallbackData = SaveFile(e.UploadedFile);

	}
	protected string SaveFile(UploadedFile uploadedFile)
	{
		var id = hdn_woprog_id["aptid"].ToString();
		var apt = new NeAppointment(Convert.ToInt32(id));
		
		var fileName = string.Empty;
		try
		{
			var wo = new NeWOProg();
			if (uploadedFile.IsValid) 
			{
				var files = new NeFiles();
				if (apt.quote_id!=0)
				{
					files.ID = apt.quote_id;
					files.parentpage = "quote";
				}
				else
				{
					if (wo.woprog_id != 0)
					{
						wo = new NeWOProg(apt.woprog_id);
						files.ID = wo.woprog_id;
						files.parentpage = "workorder";
					}
				}
				
				var path = files.GetProjectFolder(files.ID, files.parentpage, "Pictures", current_user.business_unit_id);
				try
				{
					if (!Directory.Exists(path))
					{
						files.CreateFolder(files.ID, files.parentpage, current_user.business_unit_id);
					}
				}
				catch
				{
					return "Error Saving File to Server";
				}
				path = files.GetProjectFolder(files.ID, files.parentpage, "Pictures", current_user.business_unit_id);
				fileName = string.Format(@"{0}\Pictures\{1}", path, uploadedFile.FileName);
				if (File.Exists(fileName))
					File.Delete(fileName);
				uploadedFile.SaveAs(fileName); //uncomment this line
				if (apt.quote_id == 0)
				{
					return "Saved " + uploadedFile.FileName + " to" + Toolbox.app_setting("Domain") + "->Work Orders (" + wo.OrderNumber + ")->Project Folder->Pictures";
					
				}
				else 
				{
					return "Saved " + uploadedFile.FileName + " to "+ Toolbox.app_setting("Domain") + "->Quote (" + wo.QuoteID + ")->Pictures";
					
				}

			}
		}
		catch { }
		 
		return "Invalid Work Order or Picture";
	}



	protected void dv_PreRender(object sender, EventArgs e)
	{
//		ASPxDataView adv = (ASPxDataView)sender;
//		TextBox tb_hours = (TextBox)adv.FindControl("tb_hours");
//		tb_hours.Attributes["TextMode"] = "Number";

		if (!IsPostBack)
		{
			//	Dim dt As DataTable = CType(Me.SqlDataSource1.Select(New DataSourceSelectArguments()), DataView).ToTable()
			var dv1 = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
			var dt = dv1.ToTable();
			short index = 0;
			
			foreach (DataRow dr in dt.Rows)
			{
				if (Convert.ToDateTime(dr["StartDate"]).Date == System.DateTime.Today)
				{
					dv.JSProperties["cp_start_index"] = index.ToString();
					dv.TabIndex = index;
					break;
				}
				index++;
			}
		}
	}
	protected void cb_header_Callback(object sender, CallbackEventArgsBase e)
	{
		load_current_week_entries();
	}

	protected void cal_DayCellPrepared(object sender, CalendarDayCellPreparedEventArgs e)
	{
		if (Session["mobile_oncall_dates"] == null)
		{
			Session["mobile_oncall_dates"] = _tools.getSQL_datatable(@"Select *,if(is_backup=1,'lime','green') color from oncall_schedule  where oncall_schedule.member_id =@v0", new object[] { current_user.id });
		}		
		var dt = (DataTable)Session["mobile_oncall_dates"];
		var x = 0;
		
		if (dt.Rows.Count > 0)
		{
			var dr = dt.Select("[date] = '" + e.Date.ToString("yyyy-MM-dd") + "'");
			foreach (var dr1 in dr)
			{
				e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml(dr1["color"].ToString());
			}
		}
	}
	protected void pop_oncall_WindowCallback(object source, PopupWindowCallbackArgs e)
		{
		load_oncall();
		}


    private double FindTrueLabourCost(int userId, int payTypeId, int buId)
    {
        return _tools.getSQL_double(@"SELECT get_wage_cost(@v0,@v1,@v2)", new object[] { userId, payTypeId, buId });
    }

    private int ExistingActiveWageRecords(int userId)
    {
        return _tools.getSQL_int(@"SELECT COUNT(1) FROM memberwage WHERE memberwage_memberid = @v0 AND active = 'true'", 
            new object[] { userId });
    }

    #region JobType feature code
    private JobTypeInfo GetJobTypeInfo(int business_unit_id, int userId)
    {
        JobTypeRecordQueryParameter input = new JobTypeRecordQueryParameter();
        input.business_uint_id = business_unit_id;
        input.member_id = userId;
        IJobTypeService _iJobTypeService = new JobTypeService();
        var info = _iJobTypeService.GetJobTypeInfo(input);
        return info;
    }

    private void JobTypeSetup(int business_unit_id, int userId, object sender)
    {
        ASPxComboBox control = (ASPxComboBox)sender;
        if (control == null || (control.ID != "ASPxComboBox_jobTypes") )
        {
            return;
        }

        var jobTypeInfo = GetJobTypeInfo(business_unit_id, userId);

        this.hdn_showJobType.Value = jobTypeInfo.showJobType.ToString();

        if (jobTypeInfo.showJobType)
        {
            control.DataSource = jobTypeInfo.jobTypes;
            control.DataBind();
            control.Value = jobTypeInfo.defaultValue.membertype_id;

        }
    }

    private void JobTypeSetup_JobType_Row_Visibility_Check(int business_unit_id, int userId, object sender)
    {
        var row = (HtmlTableRow)sender;
        if (row == null ||
            !(row.ID == "tr_jobtype_info" || row.ID == "tr_jobtype_label") )
        {
            return;
        }

        var jobTypeInfo = GetJobTypeInfo(business_unit_id, userId);

        row.Visible = false;
        if (jobTypeInfo.showJobType)
        {
            row.Visible = true;
        }
    }

    //
    // Data binding for job types comb.
    //
    protected void ASPxComboBox_jobTypes_Init(object sender, EventArgs e)
    {
        if (current_user == null)
        {
            return;
        }

        JobTypeSetup(current_user.business_unit_id, current_user.id, sender);
    }

    protected void ASPxComboBox_scope_Init(object sender, EventArgs e)
    {
        ASPxComboBox control = (ASPxComboBox)sender;
        if (control == null || (control.ID != "ASPxComboBox_scope"))
        {
            return;
        }

        this.scopecontrol = control;
    }

    //
    // For job list comb row: show or hide.
    //
    protected void tr_jobtype_info_Init(object sender, EventArgs e)
    {
        if (current_user == null)
        {
            return;
        }

        JobTypeSetup_JobType_Row_Visibility_Check(current_user.business_unit_id, current_user.id, sender);
    }

    //
    // For job type label: show or hide.
    //
    protected void tr_jobtype_label_Init(object sender, EventArgs e)
    {
        if (current_user == null)
        {
            return;
        }

        JobTypeSetup_JobType_Row_Visibility_Check(current_user.business_unit_id, current_user.id, sender);
    }
    #endregion
}
