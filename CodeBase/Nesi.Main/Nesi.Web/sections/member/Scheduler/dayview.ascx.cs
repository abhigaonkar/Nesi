using System;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web;

using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;
using NESI.Common.Models;
using nesi.core;

public partial class sections_member_scheduler_dayview : System.Web.UI.UserControl
{
	private int lastInsertedAppointmentId;
	
	public NeMember mymember;
	private static int _page_id = 108;  //Report Page
	private static Regex _numeric = new Regex(@"^\d+$");
	public string callback_control;
	public bool can_edit_all = false;
    public bool user_is_super = false;
    public bool can_view_all_branch = false;
	public bool no_confirm_required = false;
	protected void Page_Init()
	{
		mymember = Toolbox.do_handle_authentication(_page_id);
		hf["mid"] = mymember.id.ToString();
		hdnmid.Value = mymember.id.ToString();
	    hdnmid2.Value = mymember.id.ToString();
        hdnmid0.Value = mymember.id.ToString();
		can_edit_all=mymember.AuthenticatedForPrivilege(OpsPrivilege.EditAllAppointments)||mymember.business_unit.allow_open_scheduling;
		can_view_all_branch = mymember.AuthenticatedForPrivilege(OpsPrivilege.ViewAnyBranchSchedule) || mymember.business_unit.allow_open_scheduling;
		btWOPlanner.Visible = mymember.AuthenticatedForPrivilege(OpsPrivilege.WOPlanner);
	    sqlbranch.SelectCommand = "Select id business_unit_id,ddl_name name from business_unit where id in (" +
	                              new Current_User().visible_business_units + ") order by ddl_name";
        ddlbranch.DataBind();
        no_confirm_required = mymember.business_unit.allow_open_scheduling;
		if (!Page.IsPostBack)
		{
			ddlbranch.Value = mymember.business_unit_id;
			hf["is_wo"] = "1";
		    Session["start_date2"] = DateTime.Today.ToString("yyyy-MM-dd");

        }
	//    sqlbranch.SelectCommand = "Select id,ddl_name name from business_unit  where active = 'T' and id in (" +
	 //                             new Current_User().visible_business_units + ") order by ddl_name";
      //  sqlbranch.DataBind();
       // ddlbranch.DataBind();
        ddlbranch.ClientEnabled = can_view_all_branch;
		ddlpm.ClientEnabled =can_edit_all;
       
	}
	protected void Page_Load()
	{
	// 95 - view schedule
		// 96 - edit schedule
		// 97 - change branches

//		Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "hide_stuff", "draggable3.Style.Add('Display','none');",true);
	//	Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "hide_stuff", "alert(document.getElementById('draggable3'));", true);
		var callback_param = Page.Request.Params.Get("__CALLBACKPARAM");

		if (!IsPostBack)
		{
		    if (Session["sched_bu"] == null || ddlbranch.Items.FindByValue(Session["sched_bu"])==null)
		        {
		        Session["sched_bu"] = mymember.business_unit_id;

            }
			ddlbranch.Value = Session["sched_bu"];
            ddlpm.DataBind();
			if (ddlpm.Items.FindByValue(mymember.id)!=null)
			{
				ddlpm.Value = mymember.id;
			}
			else
			{
				ddlpm.Value = 0;
			}
			ddlfilter.SelectedIndex = 0;
			
			
			Scheduler.ActiveViewType = SchedulerViewType.Timeline;
			Scheduler.TimelineView.IntervalCount = 14;
		  
            Session["resource_view"] = null;
			Session["start_date"] = null;
			Session["scheduler_wos"] = null;
			Session["scheduler_quotes"] = null;
			Session["rec_member_sched"] = null;
			Session["rec_member_vac_sched"] = null;
			Session["start_date_overview"] = null;
			Session["on_call_schedule"] = null;
			Session["backup_on_call_schedule"] = null;
		}

		
		Scheduler.TimelineView.AppointmentDisplayOptions.StartTimeVisibility = AppointmentTimeVisibility.Always;
		Scheduler.TimelineView.AppointmentDisplayOptions.EndTimeVisibility = AppointmentTimeVisibility.Always;
		Scheduler.TimelineView.AppointmentDisplayOptions.TimeDisplayType = AppointmentTimeDisplayType.Text;
	//	Scheduler.TimelineView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never;
		default_view();

	}


    protected void default_view()
	{
		if (Session["resource_view"] == null)
		{
//			Session["resource_view"] = ddl_viewresources.Value.ToString();
		}

		if (Session["start_date"]!=null && Session["start_date"].ToString() != Scheduler.Start.ToString())
		{
			Session["scheduler_wos"] = null;
			Scheduler.JSProperties["cp_gvworefresh"] = "1";
		}
		
		Session["start_date"] = Session["start_date"] == null ? System.DateTime.Today.AddHours(6).AddMinutes(30) : Scheduler.ActiveView.GetVisibleIntervals().Start;
	   
		Scheduler.Start = chk_showpp1.Checked ? 
			Convert.ToDateTime(new NePayPeriod(Convert.ToInt32(NePayPeriod.get_payperiod_id(Convert.ToDateTime(Session["start_date"])))).StartDate).AddHours(6).AddMinutes(30) 
			: Convert.ToDateTime(Session["start_date"]);
//		Session["start_date_overview"] = Session["start_date_overview"] == null ? System.DateTime.Today.AddDays(-1) : sched_overview.ActiveView.GetVisibleIntervals().Start;
//		sched_overview.Start = Convert.ToDateTime(Session["start_date_overview"]);
		hdn_cid.Value = ddlbranch.Value.ToString();
		if (Session["on_call_schedule"] == null)
		{
			Session["on_call_schedule"] = Toolbox.doSQL_dt(@"Select member_id, date from oncall_schedule  where date > curdate() - interval 1 month and business_unit_id =@v0 and is_backup=0", new object[] { hdn_cid.Value });
		}
		if (Session["backup_on_call_schedule"] == null)
		{
			Session["backup_on_call_schedule"] = Toolbox.doSQL_dt(@"Select member_id, date from oncall_schedule  where date > curdate() - interval 1 month and business_unit_id =@v0 and is_backup=1", new object[] { hdn_cid.Value });
		}

		fill_wo_gridview(false);
		fill_quote_gridview(false);
	

		

	}


	protected void fill_wo_gridview (bool close)
	{
			#region WO gridview
		if (Session["scheduler_wos"] == null)
		{
	//		ov.Attributes["src"]= "if_overview.aspx";
	//		ov.DataBind();

		    string last_wo = Toolbox.doSQL_string("Select group_concat(a) from (Select woprog_id a from woprog " +
		                                    "where business_unit_id = @v0 and woprog_status = 'Open' and woprog_hold = 0 order by woprog_id desc limit 3)b",
		        new object[] {ddlbranch.Value});


            if (ddlpm.Value.ToString() == "0" || can_edit_all)
		        {
		        Session["scheduler_wos"] = Toolbox.doSQL_dt(@"SELECT
          woprog.woprog_id woprog_id,
          TRIM(LEADING '0' FROM woprog_bvwo) woprog_bvwo,
          woprog_customername,
          woprog.business_unit_id,
          woprog_description,
          woprog_address_id,
          woprog_status _status,
          woprog_expected_startdate start_date,
          woprog_expected_enddate end_date,
          woprog_exp_labor exp_hours,
	woprog_servicecall _sc,

    round((Select ifnull((Select sum(membertime.NumberOfHours) from membertime where membertime.MemberTime_WOProg_id = woprog.woprog_id and membertime.MemberTime_Mileage = 'false'),'')),1) hours_to_date,
	round((Select ifnull((Select sum(timestampdiff(HOUR,appointments.StartDate,appointments.EndDate)) from appointments where appointments.`Status` = 0 and appointments.woprog_id = woprog.woprog_id and appointments.StartDate > curdate()),'')),1) future_hours,

 if(find_in_set(woprog.woprog_id,@v1), '0-Last Cut', if(woprog_servicecall,'1-Service Calls',  if ((curdate()>woprog_expected_enddate and round((Select ifnull((Select sum(timestampdiff(HOUR,appointments.StartDate,appointments.EndDate)) from appointments where  appointments.`Status` = 0 and appointments.woprog_id = woprog.woprog_id and appointments.StartDate > curdate()),0)),1)=0 ),'2-Job is Past Due and Not Scheduled'
					,'3-Open Work Orders between Start and End Date'))) Priority,  h._date,
member.member_fullname pm
FROM
          woprog 
		  inner join member on member_id = woprog.woprog_pm_memberid
		  Left Join (Select min(appointments.StartDate) _date, appointments.woprog_id from appointments where appointments.startdate>= curdate() group by appointments.woprog_id) as h on h.woprog_id = woprog.woprog_id  
WHERE
                    woprog_status = 'Open'
and woprog_hold = 0 
AND woprog.business_unit_id = @v0
AND '" + Scheduler.Start.ToString("yyyy-MM-dd") + @"' > (
          woprog_expected_startdate - INTERVAL 15 DAY
)
"+
/*AND netsuite_job_internal_id IS NOT NULL */
@"
ORDER BY
			if(find_in_set(woprog.woprog_id,@v1), 0,	if(woprog_servicecall,1,	if (('" + Scheduler.Start.ToString("yyyy-MM-dd") +
		                                                           @"'>woprog_expected_enddate and round((Select ifnull((Select sum(timestampdiff(HOUR,appointments.StartDate,appointments.EndDate)) from appointments where  appointments.`Status` = 0 and appointments.woprog_id = woprog.woprog_id and appointments.StartDate > '" +
		                                                           Scheduler.Start.ToString("yyyy-MM-dd") + @"'),0)),1)=0 ),2
					,3))) ,
          woprog_status DESC,
          woprog_expected_enddate
LIMIT 450", new object[] {ddlbranch.Value, last_wo });

	
			}
			else
			{
				Session["scheduler_wos"] = Toolbox.doSQL_dt(@"SELECT
           woprog.woprog_id woprog_id,
          TRIM(LEADING '0' FROM woprog_bvwo) woprog_bvwo,
          woprog_customername,
          woprog.business_unit_id,
          woprog_description,
          woprog_address_id,
          woprog_status _status,
          woprog_expected_startdate start_date,
          woprog_expected_enddate end_date,
          woprog_exp_labor exp_hours,
			woprog_servicecall _sc,
          round((Select ifnull((Select sum(membertime.NumberOfHours) from membertime where membertime.MemberTime_WOProg_id = woprog.woprog_id and membertime.MemberTime_Mileage = 'false'),'')),1) hours_to_date,
					round((Select ifnull((Select sum(timestampdiff(HOUR,appointments.StartDate,appointments.EndDate)) from appointments where appointments.`Status` = 0 and appointments.woprog_id = woprog.woprog_id and appointments.StartDate > curdate()),'')),1) future_hours,
					
 if(find_in_set(woprog.woprog_id,@v2), '0-Last Cut', if(woprog_servicecall,'1-Service Calls',  if ((curdate()>woprog_expected_enddate and round((Select ifnull((Select sum(timestampdiff(HOUR,appointments.StartDate,appointments.EndDate)) from appointments where  appointments.`Status` = 0 and appointments.woprog_id = woprog.woprog_id and appointments.StartDate > curdate()),0)),1)=0 ),'2-Job is Past Due and Not Scheduled'
					,'3-Open Work Orders between Start and End Date'))) Priority,  h._date,
member.member_fullname pm
FROM
          woprog 
 inner join member on member_id = woprog.woprog_pm_memberid
		  Left Join (Select min(appointments.StartDate) _date, appointments.woprog_id from appointments where appointments.startdate>= curdate() group by appointments.woprog_id) as h on h.woprog_id = woprog.woprog_id  
WHERE
                    woprog_status = 'Open'
and woprog_hold = 0 
AND woprog.business_unit_id = @v0
AND (woprog_pm_memberid = @v1 or woprog_servicecall = 1)
AND '" + Scheduler.Start.ToString("yyyy-MM-dd") + @"' > (
          woprog_expected_startdate - INTERVAL 15 DAY
)
ORDER BY
					if(find_in_set(woprog.woprog_id,@v2), 0,	if(woprog_servicecall,1,	if (('" + Scheduler.Start.ToString("yyyy-MM-dd") +
				                                            @"'>woprog_expected_enddate and round((Select ifnull((Select sum(timestampdiff(HOUR,appointments.StartDate,appointments.EndDate)) from appointments where  appointments.`Status` = 0 and appointments.woprog_id = woprog.woprog_id and appointments.StartDate > '" +
				                                            Scheduler.Start.ToString("yyyy-MM-dd") + @"'),0)),1)=0 ),2
					,3)))  ,
          woprog_status DESC,
          woprog_expected_enddate
LIMIT 250", new object[] { ddlbranch.Value,ddlpm.Value,last_wo });

	
			}
		}
		
		gvwos.DataSource = Session["scheduler_wos"];
		var dt = (DataTable)Session["scheduler_wos"];
		gvwos0.DataSource = dt;
		gvwos0.FilterExpression = "[_sc] = 1";
		gvwos0.DataBind();
		gvwos.DataBind();

		if (close == true)
		{
			gvwos.CancelEdit();
		}
		else
		{
			gvwos.GroupBy(gvwos.Columns["Priority"]);
			gvwos.ExpandAll();
		}
		#endregion


	}

	protected void fill_quote_gridview (bool close)
	{

		#region quote gridview
		if (Session["scheduler_quotes"] == null)
		{

			if (ddlpm.Value.ToString() == "0")
			{
				Session["scheduler_quotes"] = Toolbox.doSQL_dt(@"SELECT quote_master.quote_id, concat(quote_master.quote_id,' R',quote_master.revision) quote_id_rev, quote_master.revision, quote_status.`status` _status, quote_master.open_date, quote_master.date_due due_date, quote_master.job_description quote_description, customer.customer_name quote_customername, quote_master.business_unit_id quote_business_unit_id, ifnull(quote_schedule.allowedquotetime,0) exp_hours, 0 typeid, quote_master.address_id quote_address_id, Get_QuotedHours(quote_master.quote_id,quote_master.revision) hours_spent, round((Select ifnull((Select sum(timestampdiff(HOUR,appointments.StartDate,appointments.EndDate)) from appointments  where appointments.`Status` = 0 and appointments.quote_id = quote_master.quote_id and appointments.StartDate > curdate()),'')),1) future_hours, member_fullname quoted_by FROM quote_master INNER JOIN quote_status ON quote_master.status_id = quote_status.id INNER JOIN customer ON quote_master.customer_id = customer.customer_id inner join member on member.member_id = quote_master.quoted_by LEFT JOIN quote_schedule ON quote_master.quote_id = quote_schedule.quoteid WHERE quote_master.business_unit_id =@v0 and quote_master.status_id <> 9 and quote_master.status_id <> 6 and quote_master.status_id <> 7 and quote_master.status_id <> 8 order by quote_id desc", new object[] { ddlbranch.Value });


			}
			else
			{
				Session["scheduler_quotes"] = Toolbox.doSQL_dt(@"SELECT quote_master.quote_id, concat(quote_master.quote_id,' R',quote_master.revision) quote_id_rev, quote_master.revision, quote_status.`status` _status, quote_master.open_date, quote_master.date_due due_date, quote_master.job_description quote_description, customer.customer_name quote_customername, quote_master.business_unit_id quote_business_unit_id, ifnull(quote_schedule.allowedquotetime,0) exp_hours, 0 typeid, quote_master.address_id quote_address_id, Get_QuotedHours(quote_master.quote_id,quote_master.revision) hours_spent, round((Select ifnull((Select sum(timestampdiff(HOUR,appointments.StartDate,appointments.EndDate)) from appointments  where appointments.`Status` = 0 and appointments.quote_id = quote_master.quote_id and appointments.StartDate > curdate()),'')),1) future_hours, member_fullname quoted_by FROM quote_master INNER JOIN quote_status ON quote_master.status_id = quote_status.id INNER JOIN customer ON quote_master.customer_id = customer.customer_id inner join member on member.member_id = quote_master.quoted_by LEFT JOIN quote_schedule ON quote_master.quote_id = quote_schedule.quoteid WHERE quote_master.quoted_by =@v0 and quote_master.business_unit_id =@v1  and quote_master.status_id <> 9 and quote_master.status_id <> 6 and quote_master.status_id <> 7 and quote_master.status_id <> 8 order by quote_id desc", new object[] { ddlpm.Value.ToString(),ddlbranch.Value });


			}
		}

		gv_quotes.DataSource = Session["scheduler_quotes"];
		gv_quotes.DataBind();
		if (close == true)
		{
			gv_quotes.CancelEdit();
		}
		#endregion

	}

    protected void ASPxScheduler1_BeforeExecuteCallbackCommand(object sender, DevExpress.Web.ASPxScheduler.SchedulerCallbackCommandEventArgs e)
	{
//		default_view();
		
		if (e.CommandId == SchedulerCallbackCommandId.AppointmentSave)
		{
		//	e.Command = new UserAppointmentSaveCallbackCommand((ASPxScheduler)sender);
			Scheduler.CancelUpdate();
		}
        if (e.CommandId == "copy")
        {
            if (Scheduler.SelectedAppointments[0] != null)
            {
                Session["copied_scheduler_apt"] = Scheduler.SelectedAppointments[0].Id.ToString();
                Scheduler.JSProperties["cpWarning"] = "Schedule Copied";
            }
            return;
        }
		if (e.CommandId == "CRTAPT")
		{
			e.Command = new CreateAppointmentCallbackCommand1((ASPxScheduler)sender);
			Scheduler.JSProperties["cp_gvworefresh"] = "1";
		}
		if (e.CommandId == "CRTAPTQ")
		{
			e.Command = new CreateAppointmentquote((ASPxScheduler)sender);
	//		Scheduler.JSProperties["cp_gvworefresh"] = "1";
		}
        if (e.CommandId == "CRTAPTS")
        {
            e.Command = new CreateAppointmentCallbackSCalls((ASPxScheduler)sender);
            Scheduler.JSProperties["cp_gvworefresh"] = "1";
        }
        else if(e.CommandId == "FORWARD")
			{
			int diffDate = 1;
			if (chk_showpp1.Checked) diffDate = 14;
			e.Command = new CustomNavigateForwardCallbackCommand((ASPxScheduler)sender,diffDate);
        }
		else if (e.CommandId == "BACK")
		{
		int diffDate = 1;
		if (chk_showpp1.Checked) diffDate = 14;
			e.Command = new CustomNavigateBackwardCallbackCommand((ASPxScheduler)sender, diffDate);
		}
		else if (e.CommandId == "DELETE")
		{
			delete_appt();
		//	Scheduler.JSProperties["cp_gvworefresh"] = "1";
		}
		else if (e.CommandId == "MYAPTMENU")
		{
			e.Command = new CreateDayOffAppointment((ASPxScheduler)sender);
		}
		else if (e.CommandId == "APTSCHANGE")
		{
			gvwos.JSProperties["cp_gvworefresh"] = "T";
		}
		else if (e.CommandId == "CONFIRM")
		{
			confirm_appt();
		}
        else if (e.CommandId == "TENTATIVE")
        {
            tent_appt();
        }
		else if (e.CommandId == "MEET_ON_SITE")
		{
			meet_on_site();
		}
		else if (e.CommandId == "MEET_AT_SHOP")
		{
			meet_at_shop();
		}
        else if (e.CommandId == "APPROVE")
        {
            alter_vacation(3);
        }
        else if (e.CommandId == "DENY")
        {
            alter_vacation(2);
        }
        else if (e.CommandId == "PENDING")
        {
            alter_vacation(1);
        }
    }

   

    protected void alter_vacation(int _type_of)
    {
        var x = 0;
        while (x < Scheduler.SelectedAppointments.Count)
        {
            if (Convert.ToInt32(Scheduler.SelectedAppointments[x].Id) > 100000000)
            {
                int id = Convert.ToInt32(Scheduler.SelectedAppointments[x].Id) - 100000000;
                NeMember mem = new NeMember(Convert.ToInt32(Scheduler.SelectedAppointments[x].ResourceId));
                if (mem.Status == "Active")
                {
                    Toolbox.doSQL_void(@"UPDATE vacation_master SET status = @v0  WHERE vacation_id = @v1 ", new object[] { _type_of, id });
                    Toolbox.doSQL_void(string.Format(@"INSERT INTO vacation_log (dt, vacation_id, member_id, action) VALUES (now(), @v0 , @v1 , 'User updated status to {0}' )", _type_of), new object[] { id, mymember.id });
                }
                x++;
            }
            Scheduler.CancelUpdate();
            Scheduler.JSProperties["cpWarning"] = "";
            Scheduler.DataBind();
        }
    }

	protected void meet_on_site()
	{
		var x = 0;
		while (x < Scheduler.SelectedAppointments.Count)
		{
			if (Convert.ToInt32(Scheduler.SelectedAppointments[x].Id) < 100000000)
			{
				var apt = new NeAppointment(Convert.ToInt32(Scheduler.SelectedAppointments[x].Id));
				apt.meet_at_shop = false;
				apt.Id = Scheduler.SelectedAppointments[x].Id;
				apt.Save();

			}
			x++;
		}
		Scheduler.CancelUpdate();
		Scheduler.JSProperties["cpWarning"] = "";
		Scheduler.DataBind();
	}
	protected void meet_at_shop()
	{
		var x = 0;
		while (x < Scheduler.SelectedAppointments.Count)
		{
			if (Convert.ToInt32(Scheduler.SelectedAppointments[x].Id) < 100000000)
			{
				var apt = new NeAppointment(Convert.ToInt32(Scheduler.SelectedAppointments[x].Id));
				apt.meet_at_shop = true;
				apt.Id = Scheduler.SelectedAppointments[x].Id;
				apt.Save();

			}
			x++;
		}
		Scheduler.CancelUpdate();
		Scheduler.JSProperties["cpWarning"] = "";
        Scheduler.JSProperties["cp_gvworefresh"] = "";
		Scheduler.DataBind();
	}

	protected void confirm_appt()
	{
		var x = 0;
		while (x < Scheduler.SelectedAppointments.Count)
		{
			if (Convert.ToInt32(Scheduler.SelectedAppointments[x].Id) < 100000000)
			{
				var apt = new NeAppointment(Convert.ToInt32(Scheduler.SelectedAppointments[x].Id));
				apt.Status = 0;
				apt.Id = Scheduler.SelectedAppointments[x].Id;
				apt.Save();

			}
			x++;
		}
		Scheduler.CancelUpdate();
		Scheduler.JSProperties["cpWarning"] = "";
//		Scheduler.JSProperties["cp_overview_refresh"] = "1";
		Scheduler.JSProperties["cp_gvworefresh"] = "";
		Scheduler.DataBind();
	}
    protected void tent_appt()
    {
        var x = 0;
        while (x < Scheduler.SelectedAppointments.Count)
        {
            if (Convert.ToInt32(Scheduler.SelectedAppointments[x].Id) < 100000000)
            {
                var apt = new NeAppointment(Convert.ToInt32(Scheduler.SelectedAppointments[x].Id));
                apt.Status = OpsSchedulerStatus.Requested;
                apt.Id = Scheduler.SelectedAppointments[x].Id;
                apt.Save();

            }
            x++;
        }
        Scheduler.CancelUpdate();
        Scheduler.JSProperties["cpWarning"] = "";
        //		Scheduler.JSProperties["cp_overview_refresh"] = "1";
       
        Scheduler.DataBind();
    }

	protected void delete_appt()
	{
		var x = 0;
		while (x < Scheduler.SelectedAppointments.Count)
		{

			if (Convert.ToInt32(Scheduler.SelectedAppointments[x].Id) > 100000000)
			{
				if (Scheduler.SelectedAppointments[0].StatusId >= 100  && Scheduler.SelectedAppointments[x].StatusId != 104)  // 104 is the status for vacation.. which we can't delete
				{
					var vacation_type = Toolbox.doSQL_int(@"Select ifnull((Select type_id from vacation_master  where vacation_id=@v0 limit 1),0) ", new object[] { Convert.ToInt32(Scheduler.SelectedAppointments[x].Id) - 100000000 });
					Toolbox.doSQL_void(@"delete from vacation_master 
where vacation_master.vacation_id =@v0 limit 1 ", new object[] { Convert.ToInt32(Scheduler.SelectedAppointments[x].Id) - 100000000 });
					if (vacation_type == 3)
					{
						Toolbox.doSQL_void(@"Delete from MemberNote where Comments like 'ID: " + (Convert.ToInt32(Scheduler.SelectedAppointments[x].Id) - 100000000) + "%' limit 1");
					}
				}

			}
			else
			{

				Toolbox.doSQL_void(@"delete from appointments where id = " + Convert.ToInt32(Scheduler.SelectedAppointments[x].Id) + " limit 1");
			}
			x++;
		}
		Scheduler.CancelUpdate();
		Scheduler.JSProperties["cpWarning"] = "";
	//	Scheduler.JSProperties["cp_overview_refresh"] = "1";
		Scheduler.JSProperties["cp_gvworefresh"] = "1";
		Scheduler.DataBind();
		
	}
	protected void SchedulingDataSource_Inserted(object sender, System.Web.UI.WebControls.SqlDataSourceStatusEventArgs e)
	{
		this.lastInsertedAppointmentId = Toolbox.doSQL_int("Select ID from appointments order by id desc limit 1");
	}
	protected void Scheduler_AppointmentRowInserting(object sender, ASPxSchedulerDataInsertingEventArgs e)
	{
		e.NewValues.Remove("ID");
	}
	protected void Scheduler_AppointmentRowInserted(object sender, ASPxSchedulerDataInsertedEventArgs e)
	{
		e.KeyFieldValue = this.lastInsertedAppointmentId;
	}
	protected void Scheduler_AppointmentsInserted(object sender, PersistentObjectsEventArgs e)
	{
		((ASPxSchedulerStorage)sender).SetAppointmentId((Appointment)e.Objects[0], lastInsertedAppointmentId);
	}

    protected void Scheduler_PopupMenuShowing(object sender, DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs e)
	{
	//	if (mymember.AuthenticatedForPrivilege(96))
	//	{
			var menu = e.Menu;
			if (e.Menu.Id == SchedulerMenuItemId.AppointmentMenu)
			{
				var item0 = e.Menu.Items[0];
				var item1 = e.Menu.Items[5];
				e.Menu.Items.Clear();
				//	e.Menu.Items.Add(item0);
				//			menu.ClientSideEvents.ItemClick = String.Format("function(s, e) {{ pop_hour.cp_aptid = scheduler.GetSelectedAppointmentIds()[0]; pop_hour.Show(); }}", Scheduler.ClientInstanceName);
				MenuHelper.AddMenuItem(menu, 0, "Edit Entry", "_open");


				e.Menu.Items.Add(item1);
				//		e.Menu.Items.Add(new MenuItem("Add Recurrence","addrecurrence"));
				menu.ClientSideEvents.ItemClick = string.Format("function(s, e) {{ DefaultViewMenuHandler({0}, s, e); }}", Scheduler.ClientInstanceName);
				MenuHelper.AddMenuItem(menu, 2, "Add Recurrence", "addrecurrence");
                MenuHelper.AddMenuItem(menu, 3, "Confirm", "confirm");
                MenuHelper.AddMenuItem(menu, 4, "Tentative", "tentative");

                MenuHelper.AddMenuItem(menu, 5, "Email", "email");
				MenuHelper.AddMenuItem(menu, 6, "Meet on Site", "MEET_ON_SITE");
				MenuHelper.AddMenuItem(menu, 7, "Meet at Shop", "MEET_AT_SHOP");
                MenuHelper.AddMenuItem(menu, 8, "Approve", "APPROVE");
                MenuHelper.AddMenuItem(menu, 9, "Deny", "DENY");
                MenuHelper.AddMenuItem(menu, 10, "Pending", "PENDING");

        }
			if (e.Menu.Id == SchedulerMenuItemId.DefaultMenu)
			{

				e.Menu.ClientSideEvents.PopUp = "appointmentMenu_PopUp";
				var item0 = e.Menu.Items[2];
				var item1 = e.Menu.Items[3];
				e.Menu.Items.Clear();
				e.Menu.Items.Add(item0);
				e.Menu.Items.Add(item1);
				menu.ClientSideEvents.ItemClick = string.Format("function(s, e) {{ DefaultViewMenuHandler({0}, s, e); }}", Scheduler.ClientInstanceName);

				var daysOffTypes = Toolbox.doSQL_dt(@"SELECT * FROM vacation_type WHERE active = 1 AND show_in_scheduler = 1 ORDER BY order_in_scheduler", new object[]{ });
				var daysOffIterator = 2;
				foreach(DataRow drDaysOffType in daysOffTypes.Rows)
					{
					var typeName = Toolbox.ReturnBlankIfNull_string(drDaysOffType["type"]);
					var typeId = Toolbox.ReturnZeroIfNull_int(drDaysOffType["id"]);
					MenuHelper.AddMenuItem(menu, daysOffIterator, typeName, typeId.ToString());
					daysOffIterator++;
					}
				MenuHelper.AddMenuItem(menu, daysOffIterator, "Add Event", "addevent");
			/*
				MenuHelper.AddMenuItem(menu, 2, "Unpaid Day Off", "1");
				MenuHelper.AddMenuItem(menu, 3, "App School", "5");
				MenuHelper.AddMenuItem(menu, 4, "Discipline Day Off", "6");
				MenuHelper.AddMenuItem(menu, 5, "Illness Day - Unpaid", "2");
				MenuHelper.AddMenuItem(menu, 6, "Illness Day - Paid", "10");
				MenuHelper.AddMenuItem(menu, 7, "Self-Isolation", "11");
				MenuHelper.AddMenuItem(menu, 8, "Late Appearence", "3");
				MenuHelper.AddMenuItem(menu, 9, "Shortage of Work", "7");
				MenuHelper.AddMenuItem(menu, 10, "Add Event", "addevent");
			 */
			}
	//	}
	//	else
	//	{
	//		e.Menu.Items.Clear();
	//	}
		
	}


    protected void Scheduler_PrepareAppointmentFormPopupContainer(object sender, ASPxSchedulerPrepareFormPopupContainerEventArgs e)
	{
		
	//	e.Popup.HeaderText = new NeAppointment(Convert.ToInt32(AppointmentIdHelper.GetAppointmentId(Scheduler.SelectedAppointments[0]))).Subject;
	//	e.Popup.Width = System.Web.UI.WebControls.Unit.Pixel(600);
	//	e.Popup.Height = System.Web.UI.WebControls.Unit.Pixel(400);
	//	e.Popup.Border.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
	}

    protected void Scheduler_InitClientAppointment(object sender, InitClientAppointmentEventArgs args)
	{
		args.Properties.Add(ClientSideAppointmentFieldNames.StatusId, args.Appointment.StatusId);
		args.Properties["cpCustomer"] = "";
		args.Properties["cpwodesc"] = "";
		args.Properties["cpbvwo"] = "";
		args.Properties["cphours"] = "";
		args.Properties["cphoursexp"] = "";
		args.Properties["cplblhourslabel"] = "";
		args.Properties["cplblhoursexplabel"] = "";
		args.Properties["cpwoprogid"] = "";
		args.Properties["cpquoteid"] = "";
		args.Properties["cpaptid"] = "";
		args.Properties["cptype"] = args.Appointment.Description;
		args.Properties["cpinterval"] = args.Appointment.Start.ToString("MMM dd  h:mm tt") + " to " +  args.Appointment.End.ToString("h:mm tt");
		args.Properties.Add("setby", args.Appointment.CustomFields["setby"]);
		args.Properties.Add("member_id", args.Appointment.ResourceId);
		args.Properties.Add("can_edit_all", can_edit_all);
        if (args.Appointment.StatusId>=100 && args.Appointment.StatusId<106)
        {
            user_is_super = NeMember.is_supervisor(Convert.ToInt32(args.Appointment.ResourceId), mymember.id32);
        }
        args.Properties.Add("user_is_super", user_is_super);
        args.Properties.Add("meet_at_shop", Convert.ToInt16(args.Appointment.CustomFields["meet_at_shop"]));

		if (can_edit_all)
		{
			args.Properties.Add("thisuser", mymember.id);
			args.Properties.Add("scheduled_by", mymember.id);
		}
		else
		{
			args.Properties.Add("thisuser", mymember.id);
			args.Properties.Add("scheduled_by", args.Appointment.CustomFields["scheduled_by"]);
		}
		if (Convert.ToString(args.Appointment.CustomFields["woprog_id"]) != "0")
		{
			args.Properties["cpaptid"] = args.Appointment.Id;
			args.Properties["cpwoprogid"] = args.Appointment.CustomFields["woprog_id"];
		}
		else if (Convert.ToString(args.Appointment.CustomFields["quote_id"]) != "0")
		{
			args.Properties["cpaptid"] = args.Appointment.Id;
			args.Properties["cpquoteid"] = args.Appointment.CustomFields["quote_id"];
		}
		else if (Convert.ToString(args.Appointment.StatusId) == "98")
		{
			args.Properties["cpCustomer"] = "";
			args.Properties["cpwodesc"] = "";
			args.Properties["cpbvwo"] = "Day Off";
		}
		else if (Convert.ToString(args.Appointment.StatusId) == "99")
		{
			args.Properties["cpCustomer"] = "";
			args.Properties["cpwodesc"] = "";
			args.Properties["cpbvwo"] = "Vacation";
		}
		else if (Convert.ToString(args.Appointment.StatusId) == "200")
		{
			args.Properties["cpCustomer"] = "";
			args.Properties["cpwodesc"] = "";
			args.Properties["cpbvwo"] = "Stat Holiday";
		}
		

	}
	protected void Scheduler_AppointmentChanging(object sender, PersistentObjectCancelEventArgs e)
	{
		
		var apt = (Appointment)e.Object;
		
	var _reports_to = Toolbox.doSQL_int(@"select ifnull((Select member.reports_to 
from member inner join member member2 on member2.member_id = member.reports_to inner join membertype 
on member2.member_membertype_id = membertype.membertype_id and membertype.is_team_leader=true where member.member_id =@v0),0)", new object[] { apt.ResourceId});
	var _sched_by = Toolbox.doSQL_int(@"select ifnull((Select member.scheduled_by from member inner join member member2 
on member2.member_id = member.reports_to inner join membertype
on member2.member_membertype_id = membertype.membertype_id and membertype.is_team_leader=true where member.member_id =@v0),0)", new object[] { apt.ResourceId });


		if (Convert.ToInt32(apt.ResourceId) > 100000000)
		{
			apt.CustomFields["membertype_id"] = Convert.ToInt32(apt.ResourceId) - 100000000;
		}
		else
		{
			
			apt.CustomFields["membertype_id"] = 0;
		}

		if (apt.StatusId == 104) // if its a vacation entry
		{
			Scheduler.CustomJSProperties += new CustomJSPropertiesEventHandler(Scheduler_CustomJSProperties);
			Scheduler.JSProperties["cpWarning"] = "You can't move vacation entries";
			e.Cancel = true;
		}
		else if (apt.StatusId == OpsSchedulerStatus.Holiday)  // if its a stat holiday
		{
			Scheduler.CustomJSProperties += new CustomJSPropertiesEventHandler(Scheduler_CustomJSProperties);
			Scheduler.JSProperties["cpWarning"] = "You can't move stat holidays";
			e.Cancel = true;
		}
		else if (apt.StatusId == 500)
		{
			Scheduler.CustomJSProperties += new CustomJSPropertiesEventHandler(Scheduler_CustomJSProperties);
			Scheduler.JSProperties["cpWarning"] = "You can't move or edit CAP training in the scheduler module.  Please contact the Org Dev if you have a conflict.";
			e.Cancel = true;
		}
		else
		{
			if (!mymember.AuthenticatedForPrivilege(OpsPrivilege.EditAllAppointments) && !no_confirm_required)  // if the person CAN't edit all appointments
			{
				if (_sched_by == 0)  // if the user isnt scheduled by a team leader (as desginated by their membertype)
				{
					if (Toolbox.doSQL_int(@"Select count(id) from wo_freeze  where woprog_id =@v0 and date(dt_start) <=@v1  and date(dt_end) >=@v2", new object[] { apt.CustomFields["woprog_id"],apt.Start.ToString("yyyy-MM-dd"), apt.End.ToString("yyyy-MM-dd")})>0)
					{
						Scheduler.JSProperties["cpWarning"] = "This work order has been frozen for this date, sorry";
						e.Cancel = true;
					}
					else
					{
						//	apt.CustomFields["confirmed"] = true;
						if (apt.StatusId == OpsSchedulerStatus.Requested)
						{
							apt.StatusId = 0;
						}
						else if(apt.StatusId == OpsSchedulerStatus.SickDayUnpaid)
							{
							var sickDay = new VacationRequest(Convert.ToInt32(apt.Id) - 100000000)
											{
											date_start  = apt.Start,
											date_end    = apt.End,
											member_id   = Convert.ToInt32(apt.ResourceId),
											date_return = apt.End.Date.AddDays(1)
											};
							sickDay.save_record();
							}
						//Toolbox.doSQL_void(@"update appointments set confirmed=1, statusid = 0  where id =@v0", new object[] { apt.Id });
						Scheduler.JSProperties["cpWarning"] = "";
						//		Scheduler.JSProperties["cp_overview_refresh"] = "1";
						e.Cancel = false;
					}
				}
				else
				{
					if (mymember.id != _reports_to && mymember.id.ToString() != apt.ResourceId.ToString()) // if the user is NOT the supervisor of the person
					{
						if (mymember.id.ToString() != apt.CustomFields["setby"].ToString())
						{
							Scheduler.JSProperties["cpWarning"] = "You are only allowed to adjust appointments you set or that are set to be scheduled by you.  See your branch manager or service coordinator to change these settings.";
							e.Cancel = true;
						}
						else
						{
							//	apt.CustomFields["confirmed"] = false;
							if (apt.StatusId == 0)
							{
								apt.StatusId = OpsSchedulerStatus.Requested;
							}

							Scheduler.JSProperties["cpWarning"] = "";
							//		Scheduler.JSProperties["cp_overview_refresh"] = "1";
							e.Cancel = false;
						}
					}
					else
					{
						if (Toolbox.doSQL_int(@"Select count(id) from wo_freeze  where woprog_id =@v0 and date(dt_start) <=@v1  and date(dt_end) >=@v2", new object[] { apt.CustomFields["woprog_id"],apt.Start.ToString("yyyy-MM-dd"), apt.End.ToString("yyyy-MM-dd")})> 0)
						{
							Scheduler.JSProperties["cpWarning"] = "This work order has been frozen for this date, sorry";
							e.Cancel = true;
						}
						else
						{
							//	apt.CustomFields["confirmed"] = true;
							if (apt.StatusId == OpsSchedulerStatus.Requested)
							{
								apt.StatusId = 0;
							}
							//Toolbox.doSQL_void(@"update appointments set confirmed=1, statusid = 0  where id =@v0", new object[] { apt.Id });
							Scheduler.JSProperties["cpWarning"] = "";
							//		Scheduler.JSProperties["cp_overview_refresh"] = "1";
							e.Cancel = false;
						}
					}
				}
			}
			else  // if the person can edit all appointments
			{
                if (apt.StatusId == 105)
                {
                    Toolbox.doSQL_void(@"update vacation_master set date_start = @v0, date_end = @v1, date_return=@v2 where vacation_id = @v3 limit 1", new object[] { apt.Start, apt.End, apt.End.ToString("yyyy-MM-dd HH:mm:ss"), (Int64)apt.Id- 100000000 });
                    e.Cancel = true;
                }

				if (Toolbox.doSQL_int(@"Select count(id) from wo_freeze  where woprog_id =@v0 and date(dt_start) <=@v1  and date(dt_end) >=@v2", new object[] { apt.CustomFields["woprog_id"],apt.Start.ToString("yyyy-MM-dd"), apt.End.ToString("yyyy-MM-dd")})> 0)
				{
					Scheduler.JSProperties["cpWarning"] = "This work order has been frozen for this date, sorry";
					e.Cancel = true;
				}
				else
				{
					if (Convert.ToInt32(apt.ResourceId) > 100000000)
					{
						if (apt.StatusId == 0)
						{
							apt.StatusId = OpsSchedulerStatus.Requested;
						}

					}
					Scheduler.JSProperties["cpWarning"] = "";
			//		Scheduler.JSProperties["cp_overview_refresh"] = "1";
					e.Cancel = false;
				}
			}

		}
	}

	protected void Scheduler_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
	{
		object x;
		if (Scheduler.JSProperties.TryGetValue("cpWarning",out x)==false)
		{e.Properties.Add("cpWarning", "");
		}
		if (Scheduler.JSProperties.TryGetValue("cpOperation", out x) == false)
		{
			e.Properties.Add("cpOperation", "");
		}

	}
	protected void Scheduler_AppointmentFormShowing1(object sender, DevExpress.Web.ASPxScheduler.AppointmentFormEventArgs e)
	{

//		e.Container = new UserAppointmentFormTemplateContainer((ASPxScheduler)sender);
		pop_edit.ShowOnPageLoad = true;
		pop_edit.JSProperties["cp_aptid"] = e.Appointment.Id;
		

	}
	protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
	{
		Session["scheduler_wos"] = null;
		Session["scheduler_quotes"] = null;
		Session["on_call_schedule"] = null;
		Session["backup_on_call_schedule"] = null;
	    Session["sched_bu"] = ddlbranch.Value;
        default_view();
		ddlpm.DataBind();
		if (ddlpm.Items.FindByValue(mymember.id) == null)
		{
			ddlpm.Value = 0;
		}
		
		//ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "load_ov", "<script> load_ov(); </script>", true);
	}
	protected void ddlpm_SelectedIndexChanged(object sender, EventArgs e)
	{
		Session["scheduler_wos"] = null;
		Session["scheduler_quotes"] = null;
		default_view();
	}

	public string GetResourceColor(ResourceHeaderTemplateContainer container)
	{

		if (Convert.ToInt32(container.Resource.Id) > 100000000)
		{
			return "gr"; // Golden Rod
		}
		else
		{
			var sched_by = Toolbox.doSQL_string(@"Select ifnull((Select reports_to from member  where member_id =@v0),0) ", new object[] { container.Resource.Id });
			if (ddlpm.Value.ToString() == sched_by||ddlpm.Value.ToString()==container.Resource.Id.ToString())
			{
				return "o"; // Orange
			}
			else
			{
				return "trans"; // Nothing
			}
		}
		
	}

	public string GetResourceHeight()
	{
		return Scheduler.TimelineView.Styles.TimelineCellBody.Height.ToString();
	}
	protected void pop_recurrence_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{
		pop_recurrence.JSProperties["cp_bind"] = "0";
		if (e.Parameter == "save")
		{
			

			#region days off etc
			if (Convert.ToInt32(Scheduler.SelectedAppointments[0].Id) > 100000000)
			{
				if (ASPxCalendar1.SelectedDates.Count > 0)
				{
					foreach (var cs in ASPxCalendar1.SelectedDates)
					{
						if (cs.DayOfWeek != DayOfWeek.Saturday && cs.DayOfWeek != DayOfWeek.Sunday||chkallow_weekends.Checked)
						{
							var mid = Scheduler.SelectedAppointments[0].ResourceId.ToString();
							var start_dt = cs.Date.AddHours(Scheduler.SelectedAppointments[0].Start.Hour).AddMinutes(Scheduler.SelectedAppointments[0].Start.Minute);
							var end_dt = cs.Date.AddHours(Scheduler.SelectedAppointments[0].End.Hour).AddMinutes(Scheduler.SelectedAppointments[0].End.Minute);

							int vacation_type = Convert.ToInt16(Scheduler.SelectedAppointments[0].StatusId - 100);
							var type_name = Toolbox.doSQL_string(@"Select ifnull((Select type from vacation_type where id =@v0),'Unknown')", new object[] { vacation_type});
							var pp = Toolbox.doSQL_int(@"Select PayperiodID from payperiods 
where StartDate <= @v0 and EndDate >= @v0 limit 1",
								new object[] { start_dt.ToString("yyyy-MM-dd")} );

							Toolbox.doSQL_void(@"Insert into vacation_master (
type_id,
date_insert,
date_start,	
date_end,	
date_return,	
payperiod_id,	
hours_requested,	
member_id,	
status,	
payment_method,	
payment_amount,	
separate_check,	
comments,	
create_member_id,	
business_unit_id	
) Values (@v0,curtime(),@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13)", new object[] {
 vacation_type ,
start_dt.ToString("yyyy-MM-dd HH:mm:00"),
end_dt.ToString("yyyy-MM-dd HH:mm:00"),
end_dt.ToString("yyyy-MM-dd HH:mm:00"),
 pp ,
0,
 mid ,
3,
1,
0,
0,
 type_name,
mymember.id ,
ddlbranch.Value });
						}
						// add days off discipline stuff here
					

					}
					pop_recurrence.JSProperties["cp_bind"] = "1";
				}
			}
			#endregion
			#region all wo schedules
			else
			{
				chk_others.DataBind();
				var apt1 = new NeAppointment(Convert.ToInt32(Scheduler.SelectedAppointments[0].Id));
				if (ASPxCalendar1.SelectedDates.Count > 0)
				{
					var dt_send_email = new DataTable();
					dt_send_email.Columns.Add("apt_id");
/*
					foreach (object other_member_1 in chk_others.SelectedValues)
					{
						if (apt1.member_id.ToString()!=other_member_1.ToString())
						{
							Appointment apt = Scheduler.Storage.CreateAppointment(AppointmentType.Normal);
							apt.Description = apt1.Description;
							apt.Subject = apt1.Subject;
							apt.Location = apt1.Location;
							//	apt.ResourceId = Scheduler.SelectedAppointments[0].ResourceId;
							apt.ResourceId = other_member_1;
							apt.CustomFields["woprog_id"] = apt1.woprog_id;
							apt.CustomFields["quote_id"] = apt1.quote_id;
							apt.CustomFields["business_unit_id"] = apt1.business_unit_id;
							apt.CustomFields["assetid"] = apt1.assetid;
							//	apt.CustomFields["member_id"] = apt1.member_id;
							apt.CustomFields["member_id"] = other_member_1;
							apt.CustomFields["setby"] = mymember.id;
							apt.CustomFields["membertype_id"] = apt1.membertype_id;
							apt.StatusId = 97;
							apt.Start = apt1.StartTime;
							apt.End = apt1.EndTime;
							if ((mymember.AuthenticatedForPrivilege(167)) || (mymember.id.ToString() == other_member_1.ToString()) || (mymember.id == (Toolbox.do_int("select ifnull((Select reports_to from member where reports_to!=0 and member_id = " + Convert.ToInt32(apt1.member_id) + "),100000000)"))))
							{
								apt.StatusId = apt1.Status;
							}
							if (Convert.ToString(apt1.woprog_id) != "0")
							{
								if ((Toolbox.doSQL_int("Select count(id) from wo_freeze where woprog_id = " + String.Format("{0}", apt1.woprog_id) + " and date(dt_start) <= '" + apt.Start.ToString("yyyy-MM-dd") + "' and date(dt_end) >='" + apt.Start.ToString("yyyy-MM-dd") + "'",null) == 0))
								{
									Scheduler.Storage.Appointments.Add(apt);
								}
							}
							else
							{
								Scheduler.Storage.Appointments.Add(apt);
							}
						}
					}
*/


					foreach (var cs in ASPxCalendar1.SelectedDates)
					{
						if (cs.DayOfWeek != DayOfWeek.Saturday && cs.DayOfWeek != DayOfWeek.Sunday || chkallow_weekends.Checked)
						{
							foreach (var other_member in chk_others.SelectedValues)
							{

                                var is_on_vacation = Toolbox.doSQL_int(@"Select count(vacation_master.vacation_id) from vacation_master  where vacation_master.member_id =@v0 and vacation_master.status = 3 and vacation_master.date_start =@v1  limit 1 ", new object[] { other_member,cs.Date.ToString("yyyy-MM-dd") })>0;
             //                   bool is_holiday = (Toolbox.doSQL_int(@"Select is_holiday(@v0,,new object[] { cs.Date.ToString("yyyy-MM-dd"),other_member,)")==1 } );
                                var is_unavailable = Toolbox.doSQL_int(@"Select count(appointments.id) from appointments  where member_id =@v0 and status=69 and startdate>=@v1  and enddate<=@v2 ", new object[] { other_member,cs.Date,cs.Date}) > 0;

                                if ((!is_unavailable || chk_allow_unavailable.Checked) && (!is_on_vacation || chk_allow_adding_vacation_days.Checked))
                                {

                                    var apt = Scheduler.Storage.CreateAppointment(AppointmentType.Normal);
                                    apt.Description = apt1.Description;
                                    apt.Subject = apt1.Subject;
                                    apt.Location = apt1.Location;
                                    //	apt.ResourceId = Scheduler.SelectedAppointments[0].ResourceId;
                                    apt.ResourceId = other_member;
                                    apt.CustomFields["woprog_id"] = apt1.woprog_id;
                                    apt.CustomFields["quote_id"] = apt1.quote_id;
                                    apt.CustomFields["business_unit_id"] = apt1.business_unit_id;
                                    apt.CustomFields["assetid"] = apt1.assetid;
                                    //	apt.CustomFields["member_id"] = apt1.member_id;
                                    apt.CustomFields["member_id"] = other_member;
                                    apt.CustomFields["setby"] = mymember.id;
                                    apt.CustomFields["membertype_id"] = Toolbox.doSQL_string(@"Select member_membertype_id from member  where member_id =@v0 limit 1 ", new object[] { other_member.ToString() });
                                    apt.StatusId = OpsSchedulerStatus.Requested;
                                    apt.Start = cs.Date.AddHours(apt1.StartTime.Hour).AddMinutes(apt1.StartTime.Minute);

                                    //			if ((mymember.AuthenticatedForPrivilege(167)) || (mymember.id.ToString() == hf["res"].ToString()) || (mymember.id == (Toolbox.doSQL_int("select ifnull((Select reports_to from member where reports_to!=0 and member_id = " + Convert.ToInt32(apt1.member_id) + "),100000000)"))))

                                    //if ((Convert.ToInt32(apt1.setby) == mymember.id) || NeMember.is_supervisor(Convert.ToInt32(apt1.setby), mymember.id) || (can_edit_all))
                                    var mem_reports_to = Toolbox.doSQL_int(@"select ifnull((Select member.reports_to from member inner join member member2 
on member2.member_id = member.reports_to inner join membertype on member2.member_membertype_id = membertype.membertype_id and membertype.is_team_leader=true 
where member.member_id =@v0 ),100000000)", new object[] { Convert.ToInt32(other_member)});
                                    var mem_sched_by = Toolbox.doSQL_int(@"select ifnull((Select member.scheduled_by
from member inner join member member2 on member2.member_id = member.reports_to inner join membertype
on member2.member_membertype_id = membertype.membertype_id and membertype.is_team_leader=true where member.member_id =@v0),100000000)",new object[] { Convert.ToInt32(other_member) });
									if (mymember.AuthenticatedForPrivilege(OpsPrivilege.EditAllAppointments) ||
										mymember.business_unit.allow_open_scheduling ||
										mymember.id.ToString() == other_member.ToString() ||
										mymember.id == mem_sched_by
										|| mymember.id == mem_reports_to
										|| mem_sched_by == 0 || mem_sched_by == 100000000

                                        )
                                    {
                                        apt.StatusId = apt1.Status;
                                    }

                                    if (apt1.EndTime.Hour == 0 && apt1.EndTime.Minute == 0)
                                    {
                                        apt.End = cs.Date.AddDays(1);
                                    }
                                    else
                                    {
                                        if (apt1.EndTime.Date.Date == apt1.StartTime.Date.Date)
                                        {
                                            apt.End = cs.Date.AddHours(apt1.EndTime.Hour).AddMinutes(apt1.EndTime.Minute);
                                        }
                                        else
                                        {
                                            apt.End = cs.Date.AddHours(apt1.EndTime.Hour + 24).AddMinutes(apt1.EndTime.Minute);
                                        }
                                    }

                                    if (Convert.ToString(apt1.woprog_id) != "0")
                                    {
                                        if (Toolbox.doSQL_int(@"Select count(id) from wo_freeze 
where woprog_id = @v0 and date(dt_start) <= @v1  and date(dt_end) >=@v1",
												new object[] {
																string.Format("{0}", apt1.woprog_id),apt.Start.ToString("yyyy-MM-dd")
																}
												) == 0)
                                        {
                                            Scheduler.Storage.Appointments.Add(apt);
                                        }
                                    }
                                    else
                                    {
                                        Scheduler.Storage.Appointments.Add(apt);
                                    }
                                    dt_send_email.Rows.Add(apt.Id.ToString());
                                }
							}
						}
					}
				//	new NeAppointment().send_email_for_approval(dt_send_email);
					pop_recurrence.JSProperties["cp_bind"] = "1";

				}
			}
			#endregion
		}
		else
		{
            chk_others.UnselectAll();
            ASPxCalendar1.SelectedDates.Clear();
			chk_others.DataBind();
			if (chk_others.Items.FindByValue(Convert.ToInt32(Scheduler.SelectedResource.Id))!=null)
			{
				chk_others.Items.FindByValue(Convert.ToInt32(Scheduler.SelectedResource.Id)).Selected = true;
			}

			var aptid = e.Parameter;
	//		chk_others.DataSource = Toolbox.doSQL_dt(@"Select member_id,member_fullname from member inner join membertype on membertype.membertype_id = member.member_membertype_id and membertype.is_scheduled=1  where business_unit_id=@v0 and member_status='Active' order by member_fullname", new object[] { hdn_cid.Value });
	//		chk_others.DataBind();
			ASPxCalendar1.HighlightToday = false;
			ASPxCalendar1.MinDate = System.DateTime.Today;
			Session["rec_member_sched"] = Toolbox.doSQL_dt(@"Select date(appointments.startdate) m, `Status`,member_id from appointments  where appointments.resourceid =@v0 UNION SELECT vacation_master.date_start m, 99 `Status`, vacation_master.member_id member_id FROM vacation_master inner join vacation_status on vacation_status.status_id = vacation_master.Status where (vacation_master.Status = 1 or vacation_master.Status = 3) and vacation_master.member_id=@v1  and date_start is not null and date_end is not null", new object[] { Scheduler.SelectedResource.Id,Scheduler.SelectedResource.Id });

			#region app school
			if (Convert.ToInt32(aptid) > 100000000)
			{
				var dt = Toolbox.doSQL_dt(@"Select * from vacation_master inner join vacation_type on vacation_type.id = vacation_master.type_id inner join vacation_status on vacation_status.status_id=vacation_master.status  where vacation_master.vacation_id =@v0", new object[] { Convert.ToInt32(e.Parameter) - 100000000 });
				foreach (DataRow dr in dt.Rows)
				{
					var assignee = new NeMember(Convert.ToInt32(dr["member_id"]));


					if (dr["type_id"].ToString() == "3") // if it's a late day
					{
						lbl_start_and_end.Text = Convert.ToDateTime(dr["date_start"]).ToString("yyyy-MM-dd HH:mm") + " - " + Convert.ToDateTime(dr["date_end"]).ToString("HH:mm");
						lbl_description_rec.Text = dr["comments"].ToString();
					}
					else
					{
						lbl_start_and_end.Text = Convert.ToDateTime(dr["date_start"]).ToString("yyyy-MM-dd");
						lbl_description_rec.Text = dr["status1"].ToString();
					}
			
					
					lbl_original_date.Text = "";
					lbl_customer_rec.Text = dr["type"].ToString();
					ASPxCalendar1.SelectedDate = Convert.ToDateTime(dr["date_start"]).Date;
					
					
					
					lbl_member_rec.Text = assignee.FullName;
				}

			}
			#endregion
			#region wo schedules
			else
			{
				lblaptid.Text = aptid;
				var apt = new NeAppointment(Convert.ToInt32(aptid));
				//ASPxCalendar1.SelectedDate = apt.StartTime.Date;
				lbl_customer_rec.Text = apt.Description;
				if (Convert.ToString(apt.woprog_id) != "0")
				{
					lbl_description_rec.Text = Toolbox.doSQL_string(@"Select ifnull((Select woprog_description from woprog  where woprog_id =@v0),'')", new object[] { apt.woprog_id });
				}
				else
				{
					lbl_description_rec.Text = "";
				}
				lbl_original_date.Text = apt.StartTime.Date.ToString("yyyy-MM-dd");
				lbl_start_and_end.Text = apt.StartTime.ToShortTimeString() + " to " + apt.EndTime.ToShortTimeString();
				lbl_member_rec.Text = Scheduler.SelectedResource.Caption;
			}
			#endregion
		}

	}
	protected void ASPxCalendar1_DayCellPrepared(object sender, CalendarDayCellPreparedEventArgs e)
	{
		if (Session["rec_member_sched"] != null)
		{
            if (e.Date.ToString("yyyy-MM-dd") == lbl_original_date.Text)
            {
                e.Cell.Font.Bold = true;
                e.Cell.ForeColor = System.Drawing.Color.Orange;
            }
            return;

			var dt = (DataTable)Session["rec_member_sched"];
			if (e.Date >= System.DateTime.Today)
			{
				if (dt.Select("m='" + e.Date + "'").Length > 0)
				{
					var dr = dt.Select("m='" + e.Date + "'").CopyToDataTable();
					if (Convert.ToString(dr.Rows[0][1]) == "98")  // if it s a day off
					{

					e.Cell.Attributes["style"] = "pointer-events: none; color: LightGray; background-color: LimeGreen";
					}
					else if (Convert.ToString(dr.Rows[0][1]) == "99") // if its a holiday
					{

						e.Cell.Attributes["style"] = "pointer-events: none; color: white; background-color: LightGreen";
					}
                    else if (Convert.ToString(dr.Rows[0][1]) == "69") // if its a holiday
                    {

                        e.Cell.Attributes["style"] = "pointer-events: none; color: white; background-color: LightGray";
                    }
					else
					{
						if (Convert.ToString(dr.Rows[0][1]) == "97")  // if its not confirmed
						{
							if (Convert.ToInt32(dr.Rows[0][2]) < 100000000)
							{
								e.Cell.Font.Bold = true;
                                e.Cell.ForeColor = System.Drawing.Color.Orange;
							}
						}
						else  // if the person is already booked
						{
                            
                            e.Cell.Font.Bold = true;
                            e.Cell.ForeColor = System.Drawing.Color.Green;
						}
					}
				}
			}
		}
	}

    protected void pop_edit_WindowCallback1(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{
		if (e.Parameter.Length > 0)
		{
			if (e.Parameter == "Save")
			{
				var apt = new NeAppointment(Convert.ToInt32(Scheduler.SelectedAppointments[0].Id));
				apt.assetid = Convert.ToInt32(ddl_truck.Value);
				apt.StartTime = new DateTime(apt.StartTime.Year, apt.StartTime.Month, apt.StartTime.Day, te_start.Date.Hour, te_start.Date.Minute, 0);
				apt.EndTime = new DateTime(te_end.Date.Year, te_end.Date.Month, te_end.Date.Day, te_end.Date.Hour, te_end.Date.Minute, 0);
				apt.Id = Scheduler.SelectedAppointments[0].Id;
				apt.notes = mem_notes.Text;
				apt.Save();
                if (apt.woprog_id!=0)
                {
                    Toolbox.doSQL_void(@"update woprog 
set woprog_location_in_plant = @v0 ,
woprog_specialinstructions =@v1
where woprog_id = @v2" ,
	                new object[] {  txt_location.Text,
						mem_notes.Text,apt.woprog_id});
                }
                
			}
			else if (e.Parameter == "Delete")
			{

				Toolbox.doSQL_void(@"Delete from appointments where id = @v0 limit 1" , new object[] { Scheduler.SelectedAppointments[0].Id});
			}
			else
			{
				var apt = new NeAppointment(Convert.ToInt32(e.Parameter));
				ddl_truck.Value = apt.assetid;
				te_start.Date = apt.StartTime;
				te_end.Date = apt.EndTime;
				lbldate.Text = apt.StartTime.ToLongDateString();
				mem_notes.Text = apt.notes;
				if (apt.member_id > 100000000)
				{
					lblresource.Text = Toolbox.doSQL_string(@"Select membertype_name from membertype where membertype_id =@v0 " , apt.membertype_id);
				}
				else
				{
					lblresource.Text = Toolbox.doSQL_string("Select member_fullname from member where member_id =@v0 " , apt.member_id);
				}
			
				lblsetby.Text = new NeMember(Convert.ToInt32(apt.setby)).FullName;
				lbl_date_of_entry.Text = apt.date_of_entry.ToString("yyyy-MM-dd HH:mm:ss");
				if (Convert.ToInt32(apt.setby) == mymember.id32 || NeMember.is_supervisor(Convert.ToInt32(apt.setby), mymember.id) || can_edit_all)
				{
					btndelete.ClientVisible = true;
				}
				else
				{
					btndelete.ClientVisible = false;
				}



				if (Convert.ToInt32(apt.woprog_id) != 0)
				{
					var wo = new NeWOProg(Convert.ToInt32(apt.woprog_id));
                    var addr = new NEAddress(wo.woprog_Address_ID);
					var contact = new NEContact(Convert.ToInt32(wo.woprog_Contact_ID));
					lblcontact.Text = contact.name;
					lblphone.Text = contact.cellphone;
                    lbl_desc.Text = wo.Description;
                    txt_address.Text = addr.Addr1 + ", " + addr.City;
                    txt_location.ClientEnabled = true;
                    txt_location.Text = wo.woprog_Location_in_plant;
                    mem_notes.Text = wo.special_instructions;
					hl_customer.Text = wo.CustomerName;
					hl_customer.ClientSideEvents.Click = "function (s,e){ boing('../../../sections/customer/index.aspx?customer_id=" + wo.WOProg_Customer_ID + "&from_wo=T','customer',1100,900);}";
					hl_wo.ClientSideEvents.Click = "function (s,e){ boing('../../workorder/index.aspx?woprog_id=" + wo.woprog_id + "&business_unit_id=" + wo.business_unit_id + "','workorder',1100,900);}";
					hl_wo.Text = wo.OrderNumber;
				}
				else
				{
					var quote = new quote(Convert.ToInt32(apt.quote_id));
					var addr = new NEAddress(quote.address_id);
					var contact = new NEContact(Convert.ToInt32(quote.contact_id));
					lblcontact.Text = contact.name;
					lblphone.Text = contact.cellphone;
                    txt_address.Text = addr.Addr1;
                    txt_location.Text = "";
                    lbl_desc.Text = quote.txtJobDescription;
                    txt_location.ClientEnabled = false;
					hl_customer.Text = quote.txtCustomerName;
					hl_customer.ClientSideEvents.Click = "function (s,e){ boing('../../../sections/customer/index.aspx?customer_id=" + quote.cust_id + "&from_wo=T','customer',1100,900);}";
					hl_wo.ClientSideEvents.Click = "function (s,e){ boing('/sections/member/quote/index.aspx?a=g&quote_id=" + quote.QuoteID + "&revision=" + quote.Revision + "', 'quote', 1035,800);}";
					hl_wo.Text = quote.QuoteID + " R" + quote.Revision;
					ddl_truck.ClientVisible = false;

				}

			}
		}
	}

    protected void pop_ts_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{
		var vacation_type = 0;
		var type_name = "";


		if (e.Parameter == "3")
		{
			pop_ts.JSProperties.Add("cp_typeid",e.Parameter);
			vacation_type = Convert.ToInt16(e.Parameter);
			type_name = Toolbox.doSQL_string(@"Select ifnull((Select type from vacation_type where id = @v0),'Unknown')",vacation_type);
			lbl_ts_pop_type.Text = type_name;
			lbl_ts_pop_date.Text = Scheduler.SelectedInterval.Start.Date.ToString("MMM dd");
			ts_pop_start.DateTime = Scheduler.SelectedInterval.Start.Date.AddHours(7.5);
			ts_pop_end.DateTime = Scheduler.SelectedInterval.Start.Date.AddHours(11);
			pop_ts.JSProperties["cp_hide"] = "0";
		}

		if (e.Parameter.Contains("save"))
		{

			vacation_type = Convert.ToInt16(e.Parameter.Split('|').GetValue(1));
			type_name = Toolbox.doSQL_string("Select ifnull((Select type from vacation_type where id  = @v0),'Unknown')", vacation_type);

			var dt_start = ts_pop_start.DateTime;
			var dt_end = ts_pop_end.DateTime;
			var setby = mymember.id32;
			var start_date = Scheduler.SelectedInterval.Start.Date;
			var resourceid = Convert.ToInt32(Scheduler.SelectedResource.Id);

			var mid = Scheduler.SelectedResource.Id.ToString();
			var pp = Toolbox.doSQL_int(@"Select PayperiodID from payperiods 
where StartDate <= @v0 and EndDate >= @v0 limit 1",
				new object[] { start_date.ToString("yyyy-MM-dd")}
);
			var sql = @"Insert into vacation_master (
type_id,
date_insert,
date_start,	
date_end,	
date_return,	
payperiod_id,	
hours_requested,	
member_id,	
status,	
payment_method,	
payment_amount,	
separate_check,	
comments,	
create_member_id,	
business_unit_id	
) Values (@v0,curtime(),@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13)";
			var paramObjects=
				new object[] {		
vacation_type ,
 ts_pop_start.DateTime.ToString("yyyy-MM-dd hh:mm:00"),
ts_pop_end.DateTime.ToString("yyyy-MM-dd hh:mm:00") ,
 ts_pop_end.DateTime.ToString("yyyy-MM-dd hh:mm:00"),
 pp ,
0,
mid ,
3,
1,
0,
0,
 ts_pop_notes.Text ,
mymember.id,
 ddlbranch.Value };
			Toolbox.doSQL_void(sql,paramObjects);
			
		
			pop_ts.JSProperties["cp_hide"] = "1";
			if (chk_add_to_disc_file.Checked)
			{
				var id = Toolbox.doSQL_int(@"Select vacation_id from vacation_master  where member_id=@v0 order by vacation_id desc limit 1", new object[] { mid });
				var note = "ID: " + id + "  Late Appearence: from " + ts_pop_start.DateTime.ToString("HH:mm") + " to " + ts_pop_end.DateTime.ToString("HH:mm") + " - " + ts_pop_notes.Text;
				save_disc_note(Convert.ToInt32(mid), note, ts_pop_start.DateTime.Date);
			}

		}
		

	}

	protected void save_disc_note(int member_id,string comment, DateTime dte)
	{
		var date = dte.ToString("yyyy-MM-dd");

		var note = comment;
		if (date != "" && note != "")
		{
			var dis = new NeDisciplinary();
			var mem_ = new NeMember(Convert.ToInt32(member_id));

			dis.active = true;
			dis.comments = note;
			dis.business_unit_id = mem_.business_unit.id;
			dis.date = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
			dis.member_id_audit = mymember.id32;
			dis.member_note_added_by_member_id = mymember.id32;
			dis.member_note_member_id = mem_.id32;
			dis.note_type = "Late Appearence";
			dis.add(mem_.id);

		
		}


	}

	protected void pop_event_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{
		if (e.Parameter.Contains("save"))
		{

			var dt_start = te_start2.DateTime;
			var dt_end = te_end2.DateTime;
			var setby = mymember.id32;
			var comments = mem_event_notes.Text;
			var event_type = ddl_events.Value.Equals(69) ? 69 : Convert.ToInt32(ddl_events.Value) + 50;
			var whole_branch = chk_event_whole_branch.Checked;
			var mandatory = chk_mandatory.Checked;
			var pointperson = ddl_events.Value.Equals(69) ? mymember.id32 : Convert.ToInt32(ddl_point.Value);
			var start_date = Scheduler.SelectedInterval.Start.Date;
			var resourceid = Convert.ToInt32(Scheduler.SelectedResource.Id);
			var apt = new NeAppointment();
			apt.Id = 0;
			if (e.Parameter.Split('|').Length>1)
			{
				if (Convert.ToInt32(e.Parameter.Split('|').GetValue(1)) != 0)
				{
					apt = new NeAppointment(Convert.ToInt32(e.Parameter.Split('|').GetValue(1)));
					apt.Id = Convert.ToInt32(e.Parameter.Split('|').GetValue(1));
				}
			}
		
			if (whole_branch)
			{
				
				apt.business_unit_id = Convert.ToInt32(ddlbranch.Value);
				apt.Description = ddl_events.Value.Equals(69) ? "Unavailable" : ddl_events.Text + " - Point Person " + ddl_point.Text;
				apt.StartTime = dt_start.Date.AddHours(dt_start.Hour).AddMinutes(dt_start.Minute);
				apt.EndTime = dt_end.Date.AddHours(dt_end.Hour).AddMinutes(dt_end.Minute);
				apt.member_id = (int) resourceid;
				apt.ResourceId = resourceid;
				apt.notes = comments;
				apt.setby = (int) setby;
				apt.Status = event_type;
                apt.date_of_entry = System.DateTime.Now;
				apt.Subject = mandatory ? "Mandatory: " + ddl_events.Text : ddl_events.Text;
				apt.Save();


			}
			else
			{
				apt.business_unit_id = Convert.ToInt32(ddlbranch.Value);
				apt.Description = ddl_events.Value.Equals(69) ? "Unavailable" : ddl_events.Text + " - Point Person " + ddl_point.Text;
				apt.StartTime = dt_start.Date.AddHours(dt_start.Hour).AddMinutes(dt_start.Minute);
				apt.EndTime = dt_end.Date.AddHours(dt_end.Hour).AddMinutes(dt_end.Minute);
				apt.member_id = (int) resourceid;
				apt.ResourceId = resourceid;
				apt.notes = comments;
				apt.setby = (int) setby;
				apt.Status = event_type;
				apt.Subject = mandatory ? "Mandatory: " + ddl_events.Text : ddl_events.Text;
                apt.date_of_entry = System.DateTime.Now;
				apt.Save();
			}

			pop_event.JSProperties["cp_close"] = "1";
		}
		else if (e.Parameter.Length > 0)
		{
			if (Convert.ToInt32(e.Parameter) < 400000000)
			{
				var apt = new NeAppointment(Convert.ToInt32(e.Parameter));
				pop_event.JSProperties["cp_aptid"] = e.Parameter;
				var x = apt.Description.LastIndexOf("Point Person") + 13;
				ddl_point.Text = apt.Description.Substring(x);
				mem_event_notes.Text = apt.notes;
				te_start2.DateTime = apt.StartTime;
				te_end2.DateTime = apt.EndTime;
				if (apt.Subject.StartsWith("Mandatory"))
				{
					chk_mandatory.Checked = true;
					ddl_events.Text = apt.Subject.Split(':').GetValue(1).ToString();
				}
				else
				{
					chk_mandatory.Checked = false;
					ddl_events.Text = apt.Subject;
				}
			}
			else
			{
				pop_event.JSProperties["cp_aptid"] = e.Parameter;
				var nth = new NeCapTraining_History(Convert.ToInt32(e.Parameter)-400000000);
				var ts = new NeCapTraining_Schedule(nth.cap_training_schedule_id);
				var ct = new NeCapTraining(nth.training_header_id);
				var x = nth.id;
				mem_event_notes.Text = ts.location + System.Environment.NewLine + nth.notes;
				te_start2.DateTime = new DateTime(nth.date.Year,nth.date.Month,nth.date.Day,nth.start_time.Hours,nth.start_time.Minutes,nth.start_time.Seconds);
				te_end2.DateTime = new DateTime(nth.date.Year,nth.date.Month,nth.date.Day,nth.end_time.Hours,nth.end_time.Minutes,nth.end_time.Seconds);
				ddl_events.Text="Training";
				ddl_events.ClientEnabled = false;
				btnsaveevent.ClientEnabled = false;
				mandatory_row.Visible = false;
				point_person_row.Visible = false;
			}
			pop_event.JSProperties["cp_close"] = "";
		}
		else if (e.Parameter=="")
		{
			te_start2.DateTime = Scheduler.SelectedInterval.Start.Date.AddHours(7.5);
			te_end2.DateTime = Scheduler.SelectedInterval.Start.Date.AddHours(16);
			mem_event_notes.Text = "";
			ddl_events.SelectedIndex = 0;
			ddl_point.SelectedIndex = 0;
			pop_event.JSProperties["cp_close"] = "";
			pop_event.JSProperties["cp_aptid"] = "0";

		}
	}

    protected void gvwos_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters == "refresh")
		{
			Session["scheduler_wos"] = null;

			default_view();
		}
		else if (e.Parameters.Split('|').Length == 3)
		{
			var woid = e.Parameters.Split('|').GetValue(0).ToString();
			var value = e.Parameters.Split('|').GetValue(1).ToString();
			var type = e.Parameters.Split('|').GetValue(2).ToString();
			if (type == "s")
			{
				Toolbox.doSQL_void(@"Update woprog set WOProg_Status = @v0  where woprog_id =@v1  limit 1", 
					new object[] {  value,woid}
					);
				Toolbox.doSQL_void(@"Insert into woprogstatus (woprogstatus_woprog_id,woprogstatus_member_id,woprogstatus_datetime,woprogstatus_status) 
values (@v0,@v1,curtime(),@v2)", 
					new object[] { woid, mymember.id , value});
				Session["scheduler_wos"] = null;
				default_view();
			}
		}
	}

	protected void gvquotes_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
	
		/*		if (e.Parameters == "refresh")
				{
					Session["scheduler_quotes"] = null;
					default_view();
				}
				else if (e.Parameters.Split('|').Length == 3)
				{
					string woid = e.Parameters.Split('|').GetValue(0).ToString();
					string value = e.Parameters.Split('|').GetValue(1).ToString();
					string type = e.Parameters.Split('|').GetValue(2).ToString();
					if (type == "s")
					{
						Toolbox.doSQL_void(@"Update woprog  set WOProg_Status =@v0  where woprog_id =@v0 limit 1 ", new object[] { value,woid });
						Toolbox.doSQL_void(@"Insert into woprogstatus (woprogstatus_woprog_id,woprogstatus_member_id,woprogstatus_datetime,woprogstatus_status)  values (@v0,@v1,curtime(),@v2)",new object[] { woid,mymember.id,value }  );
						Session["scheduler_quotes"] = null;
						default_view();
					}
				}
		 */
	}


	protected void Scheduler_AppointmentDeleting(object sender, PersistentObjectCancelEventArgs e)
	{
		Scheduler.JSProperties["cp_gvworefresh"] = "T";
		
	}


    protected void Scheduler_CustomCallback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter != "")
		{
			Session["start_date"] = Convert.ToDateTime(e.Parameter.Remove(e.Parameter.IndexOf("00:00:00")));
			Scheduler.Start = Convert.ToDateTime(e.Parameter.Remove(e.Parameter.IndexOf("00:00:00")));
		
		}
	}

	protected void pop_email_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{

		if (e.Parameter.Contains("send"))
		{
			try
			{
				var email = new NeEMail();
				var lbl = (ASPxLabel)pop_email.FindControl("lbl_pop_id");
				var apt = new NeAppointment(Convert.ToInt32(e.Parameter.Split('|').GetValue(1)));

				var add = new NEAddress(Convert.ToInt32(apt.Location));
				var addr = add.Addr1 + "," + add.Addr2 + "," + add.City + "," + add.Prov;
				email.To = txt_email_to.Text;
				//	email.To = "aketelaars@newelectric.com";
				email.Subject = txt_email_subject.Text;
				email.CC = txt_email_cc.Text;
				email.Body = mem_email_message.Html;
				email.From = mymember.NEEmail;
				email.isHTML = true;


				string[] contents = { "BEGIN:VCALENDAR",
                              "PRODID:-//Flo Inc.//FloSoft//EN",
                              "BEGIN:VEVENT",
                              "DTSTART:" + apt.StartTime.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
                              "DTEND:" + apt.EndTime.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
                              "LOCATION:" + addr, 
							  
	                     "DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + mem_email_message.Html,
                              "SUMMARY:" + apt.Subject, "PRIORITY:3", 
	                     "END:VEVENT", "END:VCALENDAR" };



				System.IO.File.WriteAllLines(Server.MapPath("schedule.ics"), contents);

				var mailAttachment = new System.Net.Mail.Attachment(Server.MapPath("schedule.ics"));

				//ADD THE ATTACHMENT TO THE EMAIL

				email.Attachment = mailAttachment;

				email.Send();
				pop_email.ShowOnPageLoad = false;
				//		NeCalendar.Set_Appt(new DateTime(2015, 2, 14, 8, 0, 0), new DateTime(2015, 2, 14, 10, 0, 0), "test", 8);
			}
			catch { }


		}
		else if (e.Parameter != "")
		{
			var apt = new NeAppointment(Convert.ToInt32(e.Parameter));
			var lbl = (ASPxLabel)pop_email.FindControl("lbl_pop_id");
			lbl.Text = e.Parameter;
			if (Convert.ToInt32(e.Parameter) >= 100000000)
			{
				var dt = Toolbox.doSQL_dt(@"Select * from vacation_master inner join vacation_type on vacation_type.id = vacation_master.type_id  where vacation_master.vacation_id =@v0", new object[] { Convert.ToInt32(e.Parameter) - 100000000 });
				foreach (DataRow dr in dt.Rows)
				{
					var assignee = new NeMember(Convert.ToInt32(dr["member_id"]));
					txt_email_to.Text = assignee.NEEmail;
					txt_email_subject.Text = "NE Schedule Notification - " + dr["type"];
					mem_email_message.Html = "<font face='arial'><b>Start:</b> " + Convert.ToDateTime(dr["date_start"]).ToString("yyyy-MM-ddd HH:mm:ss") + "<br>";
					mem_email_message.Html += "<font face='arial'><b>End:</b> " + Convert.ToDateTime(dr["date_end"]).ToString("yyyy-MM-ddd HH:mm:ss") + "<br>";
					mem_email_message.Html += "<font face='arial'><b>Notes:</b> " + dr["comments"] + "<br></font>";

				}
			}
		}
		else
		{
			var apt = new NeAppointment(Convert.ToInt32(Scheduler.SelectedAppointments[0].Id));
			var lbl = (ASPxLabel)pop_email.FindControl("lbl_pop_id");
			lbl.Text = Scheduler.SelectedAppointments[0].Id.ToString();
			var assignee = new NeMember(Convert.ToInt32(apt.member_id));
			//	NeMember assignee = new NeMember(Convert.ToInt32(8));
			if (assignee.NEEmail != "" && !assignee.NEEmail.Contains("nomail"))
			{
				txt_email_to.Text = assignee.NEEmail;
			}
			else if (assignee.Email != "")
			{
				txt_email_to.Text = assignee.NEEmail;
			}
			txt_email_subject.Text = "NE Schedule Notification - " + apt.Subject;
			if (apt.woprog_id != 0)
			{
				txt_email_subject.Text = "NE Schedule Notification WO - " + apt.Subject;
			}
			else if (apt.quote_id != 0)
			{
				txt_email_subject.Text = "NE Schedule Notification Quote - " + apt.Subject;
			}
			mem_email_message.Html = "<font face='arial'><b>Start:</b> " + apt.StartTime.ToString("yyyy-MM-ddd HH:mm:ss") + "<br>";
			mem_email_message.Html += "<font face='arial'><b>End:</b> " + apt.EndTime.ToString("yyyy-MM-ddd HH:mm:ss") + "<br>";
			mem_email_message.Html += "<br>";
			var wo_details = "";
			if (apt.woprog_id != 0)
			{
				var wo = new NeWOProg(Convert.ToInt32(apt.woprog_id));
				wo_details += "<font face='arial'><b>WO Description:</b>" + wo.Description + "<br></font>";
				wo_details += "<font face='arial'><b>Special Instruction:</b>" + wo.special_instructions + "<br></font>";
				wo_details += "<font face='arial'><b>Contact:</b>" + new NEContact(wo.woprog_Contact_ID).name + "<br></font>";
				wo_details += "<font face='arial'><b>Address:</b>" + new NEAddress(wo.woprog_Address_ID).Addr1 + "," + new NEAddress(wo.woprog_Address_ID).City + "<br></font>";
				

				var dt1 = Toolbox.doSQL_dt(@"Select member_fullname from member,appointments  where date(appointments.startdate) =@v0 and appointments.woprog_id =@v1  and appointments.status = 0 and appointments.member_id = member.member_id and member.member_id !=@v2 ", new object[] { apt.StartTime.ToString("yyyy-MM-dd"),apt.woprog_id,apt.member_id });
				if (dt1.Rows.Count > 0)
				{

					mem_email_message.Html += "<font face='arial'><b>Other People Working On This Job On This Date:</b><br></font>";
					foreach (DataRow dr1 in dt1.Rows)
					{
						mem_email_message.Html += "<font face='arial'>" + dr1[0] + "<br></font>";
					}
					mem_email_message.Html += "<br>";
				}
			}
			mem_email_message.Html += "<font face='arial'><b>Notes:</b> " + apt.notes + "<br></font>";
			mem_email_message.Html += "<font face='arial'>" + wo_details + "<br></font>";

		}

	}
	protected void gvwos_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.DataColumn.FieldName.Equals("woprog_description"))
		{
			e.Cell.ToolTip = Toolbox.ReturnBlankIfNull_string(e.CellValue).ToString();
		}
	}
	protected void gvquotes_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.DataColumn.FieldName.Equals("quote_description"))
		{
			e.Cell.ToolTip = Toolbox.ReturnBlankIfNull_string(e.CellValue).ToString();
		}
	}
	protected void pop_event_Init(object sender, EventArgs e)
	{
		pop_event.JSProperties["cp_aptid"] = "";
	}
	protected void gvwos_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.VisibleIndex >= 0)
		{
			if (gv.GetRowValues(e.VisibleIndex, "_date") != DBNull.Value)
			{
				e.Row.BackColor = System.Drawing.Color.LightGreen;
			}
		
		}
	}
	protected void pop_ts_Init(object sender, EventArgs e)
	{
	
		//pop_ts.JSProperties["cp_typeid"] = "";
	}
	

	
	
	protected void ResourceDataSourcex_Selecting(object sender, System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs e)
	{
		if (ddlfilter.Value.ToString() == "1")
		{
			ResourceDataSourcex.FilterExpression = "ord <> 2";
		}
		else
		{
			ResourceDataSourcex.FilterExpression = "";
		}
	
	}
	protected void ddlfilter_SelectedIndexChanged(object sender, EventArgs e)
	{

	}
	protected void gvwos_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{

		var wo = new NeWOProg(Convert.ToInt32(gvwos.GetRowValues(gvwos.EditingRowVisibleIndex, "woprog_id")));
		wo.woprog_Expected_StartDate = Convert.ToDateTime(e.NewValues["start_date"]);
		wo.woprog_Expected_EndDate = Convert.ToDateTime(e.NewValues["end_date"]);
		wo.woprog_exp_labor = Convert.ToDouble(e.NewValues["exp_hours"]);
		wo.SaveWorkOrder();
		Session["scheduler_wos"] = null;
		fill_wo_gridview(true);
		e.Cancel = true;
		
	}
	protected void gv_quotes_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		try
		{
			Toolbox.doSQL_void(@"update quote_master 
set date_due = @v0
where quote_id =@v1 ",
				new object[] { Convert.ToDateTime(e.NewValues["due_date"]).ToString("yyyy-MM-dd"),
				Convert.ToInt32(gv_quotes.GetRowValues(gv_quotes.EditingRowVisibleIndex, "quote_id"))});
		}
		catch { }

		Session["scheduler_quotes"] = null;
		fill_quote_gridview(true);
		e.Cancel = true;
	
	}
	
	protected void cb_mt_Callback(object sender, CallbackEventArgsBase e)
	{
		var whatever = ASPxGridLookup1.GridView.GetSelectedFieldValues("id");
		Toolbox.doSQL_void(@"Delete from appointment_resource_member_link where member_id =@v0 " ,
			new object[] {	
			hdnmid.Value});
		foreach (var o in whatever)
		{
			Toolbox.doSQL_void(@"insert into appointment_resource_member_link (member_id, resource_id) values(@v0,@v1)",
				new object[] { 	hdnmid.Value ,o });
		}
	}

    protected void cb_mt_filter_Callback(object sender, CallbackEventArgsBase e)
        {
        var whatever = gv_mt_filter.GridView.GetSelectedFieldValues("id");
        Toolbox.doSQL_void(@"Delete from appointment_resource_member_hide_link where member_id =@v0 ",
            new object[] {
                hdnmid2.Value});
        foreach (var o in whatever)
            {
            Toolbox.doSQL_void(@"insert into appointment_resource_member_hide_link (member_id, resource_id) values(@v0,@v1)",
                new object[] { hdnmid2.Value, o });
            }
        }

    protected void ASPxGridLookup1_DataBound(object sender, EventArgs e)
	{
		var gv = ASPxGridLookup1.GridView;
		int x;
		for (x = 0; x < gv.VisibleRowCount; x++)
		{
			if (gv.GetRowValues(x, "_selected").ToString() == "1")
			{
				gv.Selection.SelectRow(x);
			}
		}
	}

    protected void gv_mt_filter_DataBound(object sender, EventArgs e)
        {
        var gv = gv_mt_filter.GridView;
        int x;
        for (x = 0; x < gv.VisibleRowCount; x++)
            {
            if (gv.GetRowValues(x, "_selected").ToString() == "1")
                {
                gv.Selection.SelectRow(x);
                }
            }
        }

    protected void Scheduler_HtmlTimeCellPrepared(object handler, ASPxSchedulerTimeCellPreparedEventArgs e)
	{
		var dt = (DataTable)Session["on_call_schedule"];
		var dt1 = (DataTable)Session["backup_on_call_schedule"];
		e.Cell.CssClass += " droppable";
        TimeInterval cellInterval = e.Interval;
 if (cellInterval.Start.DayOfWeek == DayOfWeek.Sunday || cellInterval.Start.DayOfWeek == DayOfWeek.Saturday)
            e.Cell.BackColor = System.Drawing.Color.LightGray;
		switch (dt.Rows.Count>0||dt1.Rows.Count>0 )
		{
			case true:
				try
				{
					if (dt.Select("[member_id]=" + e.Resource.Id + " and [date] = '" + e.Interval.Start.ToString("yyyy-MM-dd") + "'").Length > 0)
					{
						e.Cell.BackColor = System.Drawing.Color.Red;
					}
					else
					{
						if (dt1.Select("[member_id]=" + e.Resource.Id + " and [date] = '" + e.Interval.Start.ToString("yyyy-MM-dd") + "'").Length > 0)
						{
							e.Cell.BackColor = System.Drawing.Color.Orange;
						}
					}
				}
				catch { }
				break;
		}

       

    }



    protected void Scheduler_VisibleIntervalChanged(object sender, EventArgs e)
    {
    if (Scheduler.Start < Convert.ToDateTime(Session["start_date2"]).AddDays(-14)||Scheduler.Start > Convert.ToDateTime(Session["start_date2"]).AddDays(28))
        {
        Session["start_date2"] = Scheduler.Start.ToString("yyyy-MM-dd");
        SqlDataSource1.DataBind();
        Scheduler.DataBind();
        }
    }
}

#region MenuHelper
public class MenuHelper
{
	public static void RemoveMenuItem(ASPxSchedulerPopupMenu menu, string menuItemName)
	{
		var item = menu.Items.FindByName(menuItemName);
		if (item != null)
			menu.Items.Remove(item);
	}
	public static void AddMenuItem(ASPxSchedulerPopupMenu menu, int index, string caption, string menuItemName)
	{
		var items = menu.Items;
		var item = new MenuItem(caption, menuItemName);
		
		items.Insert(index, item);
	}
}
	#endregion

public class CustomNavigateForwardCallbackCommand : NavigateForwardCallbackCommand
{
	private int _diffDate;

	public CustomNavigateForwardCallbackCommand(ASPxScheduler scheduler,int diffDate) : base(scheduler)
		{
		_diffDate = diffDate;
		}


	protected override void ExecuteCore()
	{
		//base.ExecuteCore();
		Control.Start = Control.Start.AddDays(_diffDate);
	}
}

public class CustomNavigateBackwardCallbackCommand : NavigateBackwardCallbackCommand
{
	private int _diffDate;

	public CustomNavigateBackwardCallbackCommand(ASPxScheduler scheduler, int diffDate) : base(scheduler)
		{
		_diffDate = diffDate;
		}

	protected override void ExecuteCore()
	{
		//base.ExecuteCore();
		Control.Start = Control.Start.AddDays(-_diffDate);
	}
}

public class CreateDayOffAppointment : MenuAppointmentCallbackCommand
{
	public override string Id { get { return "MYAPTMENU"; } }
	public override bool RequiresControlHierarchy { get { return true; } }
	private DateTime start;
	protected DateTime Start { get { return start; } set { start = value; } }
	

	public CreateDayOffAppointment(ASPxScheduler control)
		: base(control)
	{

	}

	protected override void ParseParameters(string parameters)
	{
		base.ParseParameters(parameters);
		if (parameters != "") { Setdayoff(parameters); }
	}


	protected void Setdayoff(string p)
	{

		if (Convert.ToInt32(Control.SelectedResource.Id) < 100000000)
		{
			var hf = (ASPxHiddenField)this.Control.Parent.FindControl("hf");
			var ddlbranch = (ASPxComboBox)this.Control.Parent.FindControl("ddlbranch");
			var mid = hf["mid"].ToString();
			var start_dt = Control.SelectedInterval.Start;
			if (p.Equals("unavailable"))
			{
					var apt = this.Control.Storage.CreateAppointment(AppointmentType.Normal);
					apt.Description = string.Format("UNAVAILABLE");
					apt.Subject = string.Format("UNAVAILABLE");
					apt.Location = "0";
					apt.CustomFields["woprog_id"] = string.Format("0");
					apt.CustomFields["quote_id"] = "0";
					apt.CustomFields["business_unit_id"] = ddlbranch.Value;
					apt.CustomFields["setby"] =  mid;
					apt.StatusId = 96;
					apt.Start = Control.SelectedInterval.Start;
					apt.End = apt.Start.AddHours(24);
					apt.ResourceId = Convert.ToInt32(Control.SelectedResource.Id);
		
					this.Control.Storage.Appointments.Add(apt);


			}

			if (!p.Equals("unavailable"))
			{
				var _status = 98;
				int vacation_type = Convert.ToInt16(p);
				var type_name = Toolbox.doSQL_string(@"Select ifnull((Select type from vacation_type where id = @v0),'Unknown')", new object[] { p});
				var pp = Toolbox.doSQL_int(@"Select PayperiodID from payperiods 
where StartDate <= @v0 and EndDate >= @v0 limit 1",
					new object[] { start_dt.ToString("yyyy-MM-dd") }
);
				/*
				MenuHelper.AddMenuItem(menu, 2, "Unpaid Day Off", "1"); - Save in vacation table
				MenuHelper.AddMenuItem(menu, 3, "Training-School", "5"); - Save in vacation table
				MenuHelper.AddMenuItem(menu, 4, "Discipline Day Off", "6"); - Save in vacation table
				MenuHelper.AddMenuItem(menu, 5, "Illness Day", "2"); - Save in vacation table
				MenuHelper.AddMenuItem(menu, 6, "Late Appearence", "3"); - Save in vacation table
				MenuHelper.AddMenuItem(menu, 7, "Shortage of Work", "7"); - Save in vacation table
				*/
				Toolbox.doSQL_void(@"Insert into vacation_master (
type_id,
date_insert,
date_start,	
date_end,	
date_return,	
payperiod_id,	
hours_requested,	
member_id,	
status,	
payment_method,	
payment_amount,	
separate_check,	
comments,	
create_member_id,	
business_unit_id	
) Values (@v0,curdate(),@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13)", new object[] {
p,
start_dt.Date.ToString("yyyy-MM-dd 7:30:00") ,
start_dt.Date.ToString("yyyy-MM-dd 16:00:00") ,
start_dt.Date.AddDays(1).ToString("yyyy-MM-dd 7:30:00"),
pp ,
0,
Control.SelectedResource.Id ,
3,
1,
0,
0,
type_name ,
mid ,
ddlbranch.Value });

		
			}
			this.Control.DataBind();
		//	this.Control.JSProperties["cp_overview_refresh"] = "1";
		}
	}
}


public class CreateAppointmentquote : SchedulerCallbackCommand
{
	public override string Id { get { return "CRTAPT"; } }

	private DateTime start;
	protected DateTime Start { get { return start; } set { start = value; } }


	public CreateAppointmentquote(ASPxScheduler control)
		: base(control)
	{

	}

	protected override void ParseParameters(string parameters)
	{
		//base.ParseParameters(parameters);
		Start = DateTime.Parse(parameters,
			CultureInfo.GetCultureInfo("en-US"));

	}

	protected override void ExecuteCore()
	{
		//base.ExecuteCore();

		CreateAppointment();
	}

	protected void CreateAppointment()
	{
		var pg = (ASPxPageControl)this.Control.Parent.FindControl("pg");
		var grid = (ASPxGridView)pg.FindControl("gv_quotes");
		var hf = (ASPxHiddenField)this.Control.Parent.FindControl("hf");
		var rowValues = (object[])grid.GetRowValues(Convert.ToInt32(hf["row"]), new string[] { "quote_id", "quote_id_rev", "quote_customername", "quote_address_id", "quote_business_unit_id" });
		var mid = hf["mid"].ToString();


		var apt = this.Control.Storage.CreateAppointment(AppointmentType.Normal);
		apt.Description = string.Format("Q{0} - {1}", rowValues[1].ToString().TrimStart('0'), rowValues[2]);
		apt.Subject = string.Format("{0}", rowValues[2]);
		apt.Location = string.Format("{0}", rowValues[3]);
		apt.CustomFields["quote_id"] = string.Format("{0}", rowValues[0]);
		apt.CustomFields["woprog_id"] = "0";
		apt.CustomFields["business_unit_id"] = string.Format("{0}", rowValues[4]);
		//	apt.CustomFields["confirmed"] = 0;
		apt.CustomFields["membertype_id"] = 0;
		apt.StatusId = OpsSchedulerStatus.Requested;

		if (Convert.ToInt32(hf["res"]) > 100000000)
		{
			apt.CustomFields["membertype_id"] = Convert.ToInt32(hf["res"]) - 100000000;
		}
		else
		{
			var temp_mem = new NeMember(Convert.ToInt32(mid));
			if (temp_mem.AuthenticatedForPrivilege(OpsPrivilege.EditAllAppointments) || temp_mem.business_unit.allow_open_scheduling || Convert.ToInt32(mid) == 
				Toolbox.doSQL_int(@"select ifnull((Select reports_to from member where reports_to!=0 and member_id =@v0),100000000)",
					new object[] {  Convert.ToInt32(hf["res"])}
					))
			{
				apt.StatusId = 0;
			}
		}
		apt.CustomFields["setby"] = mid;

		apt.CustomFields["assetid"] = 0;
		apt.Start = Start.AddHours(7.5);
		apt.End = apt.Start.AddHours(1);
		apt.ResourceId = Convert.ToInt32(hf["res"]);

		this.Control.Storage.Appointments.Add(apt);
		this.Control.DataBind();

		if (apt.StatusId == OpsSchedulerStatus.Requested && apt.CustomFields["membertype_id"].ToString() == "0")
		{
			var app_list = new DataTable();
			app_list.Columns.Add("apt_id");
			app_list.Rows.Add(apt.Id);
			new NeAppointment().send_email_for_approval(app_list);
		}
		//	this.Control.ActiveView.SelectAppointment(apt);

		//	ShowAppointmentFormByServerIdCallbackCommand showAppointmentFormByServerIdCallbackCommand = new ShowAppointmentFormByServerIdCallbackCommand(this.Control);

		//	showAppointmentFormByServerIdCallbackCommand.Execute(AppointmentIdHelper.GetAppointmentId(apt).ToString());
	}
}


public class CreateAppointmentCallbackCommand1 : SchedulerCallbackCommand
{
	public override string Id { get { return "CRTAPT"; } }

	private DateTime start;
	protected DateTime Start { get { return start; } set { start = value; } }
	

	public CreateAppointmentCallbackCommand1(ASPxScheduler control)
		: base(control)
	{

	}

	protected override void ParseParameters(string parameters)
	{
		//base.ParseParameters(parameters);
		Start = DateTime.Parse(parameters,
			CultureInfo.GetCultureInfo("en-US"));

	}

	protected override void ExecuteCore()
	{
		//base.ExecuteCore();

		CreateAppointment();
	}

	protected void CreateAppointment()
	{
		var pg = (ASPxPageControl)this.Control.Parent.FindControl("pg");
		var grid = (ASPxGridView)pg.FindControl("gvwos");
		var hf = (ASPxHiddenField)this.Control.Parent.FindControl("hf");
//		object[] rowValues = (object[])grid.GetRowValues(Convert.ToInt32(hf["row"]), new string[] { "woprog_id", "woprog_bvwo", "woprog_customername", "woprog_address_id", "business_unit_id" });
		var rowValues = (object[])grid.GetRowValuesByKeyValue(Convert.ToInt32(hf["row"]), new string[] { "woprog_id", "woprog_bvwo", "woprog_customername", "woprog_address_id", "business_unit_id" });
	
		var mid = hf["mid"].ToString();

		if (Toolbox.doSQL_int(@"Select count(id) from wo_freeze 
where woprog_id =@v0 and date(dt_start) <=@v1 and date(dt_end) >=@v1",
				new object[] { string.Format("{0}", rowValues[0]), Start.ToString("yyyy-MM-dd") }
				) > 0)
		{
			throw new Exception("That work order (" + rowValues[1].ToString().TrimStart('0') + ") is frozen on that day.  See the WO Planner for details.");
		}

		var apt = this.Control.Storage.CreateAppointment(AppointmentType.Normal);
		apt.Description = string.Format("{0} - {1}", rowValues[1].ToString().TrimStart('0'), rowValues[2]);
		apt.Subject = string.Format("{0}", rowValues[2]);
		apt.Location = string.Format("{0}", rowValues[3]);
		apt.CustomFields["woprog_id"] = string.Format("{0}", rowValues[0]);
		apt.CustomFields["quote_id"] = "0";
		apt.CustomFields["business_unit_id"] = string.Format("{0}", rowValues[4]);
	//	apt.CustomFields["confirmed"] = 0;
		apt.CustomFields["membertype_id"] = 0;
		apt.StatusId = OpsSchedulerStatus.Requested;

		if (Convert.ToInt32(hf["res"]) > 100000000)
		{
			apt.CustomFields["membertype_id"] = Convert.ToInt32(hf["res"]) - 100000000;
		}
		else
		{
			var temp_mem = new NeMember(Convert.ToInt32(mid));  // set by
			var mem_reports_to = Toolbox.doSQL_int(@"select ifnull((Select member.reports_to
from member inner join member member2 on member2.member_id = member.reports_to inner 
join membertype on member2.member_membertype_id = membertype.membertype_id
and membertype.is_team_leader=true where member.member_id =@v0 ),100000000)",
				new object[] {  Convert.ToInt32(hf["res"])}
);
			var mem_sched_by = Toolbox.doSQL_int(@"select ifnull((Select member.scheduled_by from member inner join member member2 
on member2.member_id = member.reports_to inner join membertype on member2.member_membertype_id = membertype.membertype_id 
and membertype.is_team_leader=true where member.member_id = @v0 ),100000000)",
				new object[] { Convert.ToInt32(hf["res"]) }
			);
			if (temp_mem.AuthenticatedForPrivilege(OpsPrivilege.EditAllAppointments) ||
				temp_mem.business_unit.allow_open_scheduling ||
			   mid == hf["res"].ToString() ||
			   Convert.ToInt32(mid) == mem_sched_by
				|| Convert.ToInt32(mid) == mem_reports_to
				|| mem_sched_by==0 || mem_sched_by==100000000
				)
			
			{
				//		apt.CustomFields["confirmed"] = 1;

				apt.StatusId = 0;
			}
		}
		apt.CustomFields["setby"] = mid;
		
		apt.CustomFields["assetid"] = Toolbox.doSQL_int(@"Select ifnull((Select assets_id from assets where assets_type = 1 and 
assets_owner =@v0 limit 1),0) limit 1",
			new object[] {  hf["res"]});

		apt.Start = Start.AddHours(7.5);
		apt.End = apt.Start.AddHours(8.5);
		apt.ResourceId = Convert.ToInt32(hf["res"]);
		Control.Storage.Appointments.Add(apt);
		
		if (apt.StatusId == OpsSchedulerStatus.Requested && apt.CustomFields["membertype_id"].ToString()=="0")
		{
				var app_list = new DataTable();
				app_list.Columns.Add("apt_id");
				app_list.Rows.Add(apt.Id);
	//			new NeAppointment().send_email_for_approval(app_list);
		}
	Control.DataBind();
		
		//	this.Control.ActiveView.SelectAppointment(apt);

		//	ShowAppointmentFormByServerIdCallbackCommand showAppointmentFormByServerIdCallbackCommand = new ShowAppointmentFormByServerIdCallbackCommand(this.Control);

		//	showAppointmentFormByServerIdCallbackCommand.Execute(AppointmentIdHelper.GetAppointmentId(apt).ToString());
	}
}

/**
 * Created new class for service call
 */
public class CreateAppointmentCallbackSCalls : SchedulerCallbackCommand
{
    public override string Id { get { return "CRTAPTS"; } }

    private DateTime start;
    protected DateTime Start { get { return start; } set { start = value; } }


    public CreateAppointmentCallbackSCalls(ASPxScheduler control)
        : base(control)
    {

    }

    protected override void ParseParameters(string parameters)
    {
        //base.ParseParameters(parameters);
        Start = DateTime.Parse(parameters,
            CultureInfo.GetCultureInfo("en-US"));

    }

    protected override void ExecuteCore()
    {
        //base.ExecuteCore();

        CreateAppointment();
    }

    protected void CreateAppointment()
    {
        var pg = (ASPxPageControl)this.Control.Parent.FindControl("pg");
        var grid = (ASPxGridView)pg.FindControl("gvwos0");
        var hf = (ASPxHiddenField)this.Control.Parent.FindControl("hf");
        //		object[] rowValues = (object[])grid.GetRowValues(Convert.ToInt32(hf["row"]), new string[] { "woprog_id", "woprog_bvwo", "woprog_customername", "woprog_address_id", "business_unit_id" });
        var rowValues = (object[])grid.GetRowValuesByKeyValue(Convert.ToInt32(hf["row"]), new string[] { "woprog_id", "woprog_bvwo", "woprog_customername", "woprog_address_id", "business_unit_id" });

        var mid = hf["mid"].ToString();

        if (Toolbox.doSQL_int(@"Select count(id) from wo_freeze 
where woprog_id =@v0 and date(dt_start) <=@v1 and date(dt_end) >=@v1",
				new object[] { string.Format("{0}", rowValues[0]), Start.ToString("yyyy-MM-dd") }
				) > 0)
        {
            throw new Exception("That work order (" + rowValues[1].ToString().TrimStart('0') + ") is frozen on that day.  See the WO Planner for details.");
        }

        var apt = this.Control.Storage.CreateAppointment(AppointmentType.Normal);
        apt.Description = string.Format("{0} - {1}", rowValues[1].ToString().TrimStart('0'), rowValues[2]);
        apt.Subject = string.Format("{0}", rowValues[2]);
        apt.Location = string.Format("{0}", rowValues[3]);
        apt.CustomFields["woprog_id"] = string.Format("{0}", rowValues[0]);
        apt.CustomFields["quote_id"] = "0";
        apt.CustomFields["business_unit_id"] = string.Format("{0}", rowValues[4]);
        //	apt.CustomFields["confirmed"] = 0;
        apt.CustomFields["membertype_id"] = 0;
        apt.StatusId = OpsSchedulerStatus.Requested;

        if (Convert.ToInt32(hf["res"]) > 100000000)
        {
            apt.CustomFields["membertype_id"] = Convert.ToInt32(hf["res"]) - 100000000;
        }
        else
        {
            var temp_mem = new NeMember(Convert.ToInt32(mid));  // set by
            var mem_reports_to = Toolbox.doSQL_int(@"select ifnull((Select member.reports_to
from member inner join member member2 on member2.member_id = member.reports_to inner 
join membertype on member2.member_membertype_id = membertype.membertype_id
and membertype.is_team_leader=true where member.member_id =@v0 ),100000000)",
                new object[] { Convert.ToInt32(hf["res"]) }
);
            var mem_sched_by = Toolbox.doSQL_int(@"select ifnull((Select member.scheduled_by from member inner join member member2 
on member2.member_id = member.reports_to inner join membertype on member2.member_membertype_id = membertype.membertype_id 
and membertype.is_team_leader=true where member.member_id = @v0 ),100000000)",
                new object[] { Convert.ToInt32(hf["res"]) }
            );
            if (temp_mem.AuthenticatedForPrivilege(OpsPrivilege.EditAllAppointments) ||
                temp_mem.business_unit.allow_open_scheduling ||
               mid == hf["res"].ToString() ||
               Convert.ToInt32(mid) == mem_sched_by
                || Convert.ToInt32(mid) == mem_reports_to
                || mem_sched_by == 0 || mem_sched_by == 100000000
                )

            {
                //		apt.CustomFields["confirmed"] = 1;

                apt.StatusId = 0;
            }
        }
        apt.CustomFields["setby"] = mid;

        apt.CustomFields["assetid"] = Toolbox.doSQL_int(@"Select ifnull((Select assets_id from assets where assets_type = 1 and 
assets_owner =@v0 limit 1),0) limit 1",
            new object[] { hf["res"] });

        apt.Start = Start.AddHours(7.5);
        apt.End = apt.Start.AddHours(8.5);
        apt.ResourceId = Convert.ToInt32(hf["res"]);
        Control.Storage.Appointments.Add(apt);

        if (apt.StatusId == OpsSchedulerStatus.Requested && apt.CustomFields["membertype_id"].ToString() == "0")
        {
            var app_list = new DataTable();
            app_list.Columns.Add("apt_id");
            app_list.Rows.Add(apt.Id);
            //			new NeAppointment().send_email_for_approval(app_list);
        }
        Control.DataBind();

        //	this.Control.ActiveView.SelectAppointment(apt);

        //	ShowAppointmentFormByServerIdCallbackCommand showAppointmentFormByServerIdCallbackCommand = new ShowAppointmentFormByServerIdCallbackCommand(this.Control);

        //	showAppointmentFormByServerIdCallbackCommand.Execute(AppointmentIdHelper.GetAppointmentId(apt).ToString());
    }

}