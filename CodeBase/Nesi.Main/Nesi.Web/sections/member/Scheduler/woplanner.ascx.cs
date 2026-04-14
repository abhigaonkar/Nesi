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

public partial class sections_member_scheduler_woplanner : System.Web.UI.UserControl
	{
	private int lastInsertedAppointmentId;

	public NeMember mymember;
	public Toolbox _tools;
	private static int _page_id = 108;  //Report Page
	private static Regex _numeric = new Regex(@"^\d+$");
	public string callback_control;
	public int can_edit_all = 0;


	protected void Page_Init()
		{
		_tools = new Toolbox();
		mymember = Toolbox.do_handle_authentication(_page_id);
		hf["mid"] = mymember.id.ToString();
		can_edit_all = Convert.ToInt32(mymember.AuthenticatedForPrivilege(167));
		sqlbranch.SelectCommand =
			"Select id business_unit_id,ddl_name name from business_unit  where active = 'T' and id in(" +
			new Current_User().visible_business_units + ") order by name ";
		if (!Page.IsPostBack)
			{
            if (Session["wo_planner_bu"]==null)
            {
                Session["wo_planner_bu"]= mymember.business_unit_id;
            }
			if (Session["wo_planner_customer"] == null)
            {
                if (ddlpm.Items.FindByValue(mymember.id) != null)
                {
                    Session["wo_planner_customer"] = mymember.id;
                }
                else
                {
                    Session["wo_planner_customer"] = 0;
                }
               
            }
            if (Session["wo_planner_pm"] == null)
            {
                Session["wo_planner_pm"] = 0;
            }

        }
        ddlpm.Value = Session["wo_planner_pm"];
            ddlbranch.Value = Session["wo_planner_bu"];
            ddlcustomer.Value = Session["wo_planner_customer"];

        }
	protected void Page_Load()
		{

		ddlpm.DataBind();
		if (!IsPostBack)
			{
			
			Scheduler.ActiveViewType = SchedulerViewType.Timeline;
			Scheduler.TimelineView.IntervalCount = 14;
			Session["wop_resource_view"] = null;
			Session["wop_start_date"] = null;
			Session["wop_scheduler_emps"] = null;
			Session["wop_rec_member_sched"] = null;
			Session["wop_rec_member_vac_sched"] = null;
			}


		Scheduler.TimelineView.AppointmentDisplayOptions.StartTimeVisibility = AppointmentTimeVisibility.Always;
		Scheduler.TimelineView.AppointmentDisplayOptions.EndTimeVisibility = AppointmentTimeVisibility.Always;
		Scheduler.TimelineView.AppointmentDisplayOptions.TimeDisplayType = AppointmentTimeDisplayType.Text;
		//	Scheduler.TimelineView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never;
		default_view();
		}

	public string CalcDateTimeTotals(TimelineDateHeaderTemplateContainer container)
		{
		var control = (ASPxScheduler)container.NamingContainer.NamingContainer.NamingContainer.NamingContainer;
		var interval = container.Interval;
		var apts = control.Storage.GetAppointments(interval);
		var total = TimeSpan.Zero;
		foreach (var apt in apts)
			{
			if (apt.StatusId == 98 || apt.StatusId == 99)
				{
				//total = total.Add(new TimeSpan(8,0,0));
				}
			else
				{
				if (apt.StatusId == 0)
					{
					total = total.Add(apt.Duration.Subtract(new TimeSpan(0, 30, 0)));
					}
				}
			}
		return total.TotalHours.ToString();
		}

	public string CalcResourceTotals(ResourceHeaderTemplateContainer container)
		{

		return "";
		}

	protected void default_view()
		{
		if (Session["wop_resource_view"] == null)
			{
			Session["wop_resource_view"] = ddl_viewresources.Value.ToString();
			}
		if (Session["wop_start_date"] == null)
			{
			Session["wop_start_date"] = System.DateTime.Today.AddHours(6).AddMinutes(30);
			}
		else
			{
			Session["wop_start_date"] = Scheduler.TimelineView.GetVisibleIntervals().Start;
			}


		if (Session["wop_resource_view"].ToString() == "All")
			{

			}
		//	Scheduler.DataBind();


		Scheduler.Start = Convert.ToDateTime(Session["wop_start_date"]);

		hdn_cid.Value = ddlbranch.Value.ToString();

		if (Session["wop_scheduler_emps"] == null)
			{
			if (ddlpm.Value.ToString() == "0")
				{

				Session["wop_scheduler_emps"] = _tools.getSQL_datatable(@"(SELECT member_id,member_fullname,membertype_name, 1 ord,
membertype.membertype_id FROM member,membertype  where member.member_membertype_id = membertype.membertype_id and membertype.show_on_ratesheet = 1 
and member_status = 'Active' and business_unit_id =@v0) UNION (select (membertype_id + 100000000) member_id,'Anyone' member_fullname, membertype_name,
3 ord,membertype.membertype_id from membertype where show_on_ratesheet = 1) order by ord,member_fullname ", new object[] { ddlbranch.Value });
				}
			else
				{
				Session["wop_scheduler_emps"] = _tools.getSQL_datatable(@"(SELECT member_id,member_fullname,membertype_name, 
1 ord,membertype.membertype_id
FROM member,membertype  where member.member_membertype_id = membertype.membertype_id and member_status = 'Active' 
and membertype.show_on_ratesheet = 1 and business_unit_id =@v0 and member.reports_to =@v1  ) 
union (SELECT member_id,member_fullname,membertype_name, 2 ord,membertype.membertype_id FROM member,
membertype where member.member_membertype_id = membertype.membertype_id and membertype.show_on_ratesheet = 1 
and member_status = 'Active' and business_unit_id =@v0  and member.reports_to !=@v1 ) 
UNION (select (membertype_id + 100000000) member_id, 'Anyone' member_fullname, membertype_name,
3 ord,membertype.membertype_id from membertype where show_on_ratesheet = 1) order by ord,member_fullname ",
new object[] { ddlbranch.Value, ddlpm.Value });
				}
			}

		gvemps.DataSource = Session["wop_scheduler_emps"];
		gvemps.DataBind();



		}

	protected void ASPxScheduler1_BeforeExecuteCallbackCommand(object sender, DevExpress.Web.ASPxScheduler.SchedulerCallbackCommandEventArgs e)
		{
		//		default_view();

		if (e.CommandId == SchedulerCallbackCommandId.AppointmentSave)
			{
			//	e.Command = new UserAppointmentSaveCallbackCommand((ASPxScheduler)sender);
			Scheduler.CancelUpdate();
			}
		if (e.CommandId == "CRTAPT")
			{
			e.Command = new CreateAppointmentCallbackCommand3((ASPxScheduler)sender);

			}
		else if (e.CommandId == "FORWARD")
			{
			e.Command = new CustomNavigateForwardCallbackCommand2((ASPxScheduler)sender);
			}
		else if (e.CommandId == "BACK")
			{
			e.Command = new CustomNavigateBackwardCallbackCommand2((ASPxScheduler)sender);
			}
		else if (e.CommandId == "MYAPTMENU")
			{
			e.Command = new CreateDayOffAppointment2((ASPxScheduler)sender);
			}
		else if (e.CommandId == "APTSAVE")
			{
			//			e.Command = new SaveAppointmentCallbackCommand1((ASPxScheduler)sender);
			}
		else if (e.CommandId == "APTSCHANGE")
			{

			//		e.Command = new SaveAppointmentCallbackCommand2((ASPxScheduler)sender);
			}



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
	protected void ASPxLabel1_Init1(object sender, EventArgs e)
		{
		var lbl = (ASPxLabel)sender;
		var container = lbl.NamingContainer as TimelineCellBodyTemplateContainer;
		lbl.Text = container.ID;
		}
	protected void Scheduler_PopupMenuShowing(object sender, DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs e)
		{
		var menu = e.Menu;
		if (e.Menu.Id == SchedulerMenuItemId.AppointmentMenu)
			{
			var item0 = e.Menu.Items[0];
			var item1 = e.Menu.Items[5];
			e.Menu.Items.Clear();
			//	e.Menu.Items.Add(item0);
			//			menu.ClientSideEvents.ItemClick = String.Format("function(s, e) {{ pop_hour.cp_aptid = scheduler.GetSelectedAppointmentIds()[0]; pop_hour.Show(); }}", Scheduler.ClientInstanceName);
			MenuHelper2.AddMenuItem(menu, 0, "Open", "_open");


			e.Menu.Items.Add(item1);
			//		e.Menu.Items.Add(new MenuItem("Add Recurrence","addrecurrence"));
			menu.ClientSideEvents.ItemClick = string.Format("function(s, e) {{ DefaultViewMenuHandler({0}, s, e); }}", Scheduler.ClientInstanceName);
			MenuHelper2.AddMenuItem(menu, 2, "Add Recurrence", "addrecurrence");


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
			MenuHelper2.AddMenuItem(menu, 2, "Freeze Day", "freeze");

			}

		}


	protected void Scheduler_AppointmentFormShowing(object sender, DevExpress.Web.ASPxScheduler.AppointmentFormEventArgs e)
		{
		e.Container = new UserAppointmentFormTemplateContainer((ASPxScheduler)sender);

		//		if (e.Action == SchedulerFormAction.Create)
		//			e.Container.Caption = "New Schedule Entry";
		//		else e.Container.Caption = "Edit Schedule";
		}
	protected void Scheduler_PrepareAppointmentFormPopupContainer(object sender, ASPxSchedulerPrepareFormPopupContainerEventArgs e)
		{

		//	e.Popup.HeaderText = new NeAppointment(Convert.ToInt32(AppointmentIdHelper.GetAppointmentId(Scheduler.SelectedAppointments[0]))).Subject;
		//	e.Popup.Width = System.Web.UI.WebControls.Unit.Pixel(600);
		//	e.Popup.Height = System.Web.UI.WebControls.Unit.Pixel(400);
		//	e.Popup.Border.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
		}

	public string GetClickHandler(object x)
		{
		//return "function(s, e) { pop_hour.PerformCallback('" + x + "');pop_hour.Show();}";
		return "function(s, e) { pop_hour.Show();}";
		//	return "";
		}

	protected void ddl_viewresources_SelectedIndexChanged(object sender, EventArgs e)
		{
		Session["wop_resource_view"] = ddl_viewresources.Value.ToString();
		default_view();
		}
	protected void Scheduler_HtmlTimeCellPrepared(object handler, ASPxSchedulerTimeCellPreparedEventArgs e)
		{
		if (e.Interval.Start.DayOfWeek != DayOfWeek.Sunday && e.Interval.Start.DayOfWeek != DayOfWeek.Saturday)
			{
			if (_tools.getSQL_int(@"Select get_days_hours(@v0,@v1)", new object[] { e.Resource.Id, e.Interval.Start.Date.ToString("yyyy-MM-dd HH:mm:ss") }) == 1)
				{

				}
			else
				{
				e.Cell.BackColor = System.Drawing.Color.LightSalmon;
				}
			}
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
		args.Properties["cpaptid"] = "0";
		args.Properties["cptype"] = args.Appointment.Description;
		args.Properties["cpinterval"] = args.Appointment.Start.ToString("MMM dd  h:mm tt") + " to " + args.Appointment.End.ToString("h:mm tt");
		args.Properties.Add("setby", args.Appointment.CustomFields["setby"]);
		args.Properties.Add("member_id", args.Appointment.CustomFields["member_id"]);
		if (can_edit_all == 1)
			{
			args.Properties.Add("thisuser", mymember.id);
			args.Properties.Add("scheduled_by", mymember.id);
			}
		else
			{
			args.Properties.Add("thisuser", mymember.id);
			//			args.Properties.Add("scheduled_by", args.Appointment.CustomFields["scheduled_by"]);
			}
		if (Convert.ToString(args.Appointment.ResourceId) != "0")
			{
			args.Properties["cpaptid"] = args.Appointment.Id;
			args.Properties["cpwoprogid"] = args.Appointment.ResourceId;
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
		if (Convert.ToInt32(apt.CustomFields["member_id"]) > 100000000)
			{
			apt.CustomFields["membertype_id"] = Convert.ToInt32(apt.CustomFields["member_id"]) - 100000000;
			}
		else
			{
			apt.CustomFields["membertype_id"] = 0;
			}
		if (Convert.ToInt32(apt.CustomFields["member_id"]) < 100000000)
			{
			var _mem_sched_by = _tools.getSQL_int(@"Select ifnull((Select reports_to from member  where member_id =@v0),0) ", new object[] { apt.CustomFields["member_id"] });

			if (apt.StatusId == 99)
				{
				Scheduler.CustomJSProperties += new CustomJSPropertiesEventHandler(Scheduler_CustomJSProperties);
				Scheduler.JSProperties["cpWarning"] = "You can't move vacation entries";
				e.Cancel = true;
				}
			else
				{
				if (!mymember.AuthenticatedForPrivilege(167))
					{
					if (mymember.id != _mem_sched_by)
						{
						if (mymember.id.ToString() != apt.CustomFields["setby"].ToString())
							{
							Scheduler.JSProperties["cpWarning"] = "You are only allowed to adjust appointments you set or that are set to be scheduled by you.  See your branch manager or service coordinator to change these settings.";
							e.Cancel = true;
							}
						else
							{
							Scheduler.JSProperties["cpWarning"] = "";
							e.Cancel = false;
							}
						}
					else
						{
						Scheduler.JSProperties["cpWarning"] = "";
						e.Cancel = false;
						}
					}
				else
					{
					Scheduler.JSProperties["cpWarning"] = "";
					e.Cancel = false;
					}
				}
			}
		}

	protected void Scheduler_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
		{
		object x;
		if (Scheduler.JSProperties.TryGetValue("cpWarning", out x) == false)
			{
			e.Properties.Add("cpWarning", "");
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
		Session["wop_scheduler_emps"] = null;
        Session["wo_planner_bu"] = ddlbranch.Value;

        ddlpm.DataBind();
		default_view();
		}
	protected void ddlpm_SelectedIndexChanged(object sender, EventArgs e)
		{
		Session["wop_scheduler_emps"] = null;
        Session["wo_planner_pm"] = ddlpm.Value;
        default_view();
		}

    protected void ddlcust_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["wop_scheduler_emps"] = null;
        Session["wo_planner_customer"] = ddlcustomer.Value;
        default_view();
    }

    public string GetResourceColor(ResourceHeaderTemplateContainer container)
		{
		//		if (ddlpm.Value.ToString() == _tools.getSQL_string(@"Select scheduled_by from member  where member_id =@v0", new object[] { container.Resource.Id)) });
		//		}
		//		else
		//		{
		//			return System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Transparent);
		//		}
		return "";
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
			if (Convert.ToInt32(Scheduler.SelectedAppointments[0].StatusId) == OpsSchedulerStatus.FreezeDay)  // if it's a freeze day
				{
				if (ASPxCalendar1.SelectedDates.Count > 0)
					{
					foreach (var cs in ASPxCalendar1.SelectedDates)
						{
						_tools.getSQL_void(@"insert into wo_freeze (woprog_id,dt_start,dt_end,notes,setby)
values(@v0,@v1,@v2,@v3,@v4)",
							new object[] {  Scheduler.SelectedResource.Id,
							cs.Date.ToString("yyyy-MM-dd") + " " + Scheduler.SelectedAppointments[0].Start.ToString("HH:mm:00"),
							cs.Date.ToString("yyyy-MM-dd") + " " + Scheduler.SelectedAppointments[0].End.ToString("HH:mm:00"),
							Scheduler.SelectedAppointments[0].Description,
								mymember.id });
						}
					pop_recurrence.JSProperties["cp_bind"] = "1";
					}
				}
			else
				{
				var apt1 = new NeAppointment(Convert.ToInt32(Scheduler.SelectedAppointments[0].Id));
				if (ASPxCalendar1.SelectedDates.Count > 0)
					{
					foreach (var cs in ASPxCalendar1.SelectedDates)
						{
						if (cs.DayOfWeek != DayOfWeek.Saturday && cs.DayOfWeek != DayOfWeek.Sunday || chkallow_weekends.Checked)
							{

							var apt = Scheduler.Storage.CreateAppointment(AppointmentType.Normal);
							apt.Description = apt1.Description;
							apt.Subject = apt1.Subject;
							apt.Location = apt1.Location;
							apt.CustomFields["woprog_id"] = apt1.woprog_id;
							apt.CustomFields["quote_id"] = apt1.quote_id;
							apt.CustomFields["business_unit_id"] = apt1.business_unit_id;
							apt.CustomFields["assetid"] = apt1.assetid;
							apt.CustomFields["member_id"] = apt1.member_id;

							apt.CustomFields["setby"] = mymember.id;
							//	apt.CustomFields["confirmed"] = 0;
							apt.CustomFields["membertype_id"] = apt1.membertype_id.ToString();
							apt.StatusId = OpsSchedulerStatus.Requested;
							if (mymember.id == Toolbox.doSQL_int(@"select ifnull((Select reports_to from member where reports_to!=0 and member_id = " +apt1.member_id + "),100000000)") || 
								mymember.id.ToString() == apt1.member_id.ToString())
								{
								//	apt.CustomFields["confirmed"] = 1;
								apt.StatusId = apt1.Status;
								}
							else
								{
								apt.StatusId = OpsSchedulerStatus.Requested;
								}

							apt.Start = cs.Date.AddHours(apt1.StartTime.Hour).AddMinutes(apt1.StartTime.Minute);
							if (apt1.EndTime.Hour == 0 && apt1.EndTime.Minute == 0)
								{
								apt.End = cs.Date.AddDays(1);
								}
							else
								{
								apt.End = cs.Date.AddHours(apt1.EndTime.Hour).AddMinutes(apt1.EndTime.Minute);
								}
							apt.ResourceId = Scheduler.SelectedAppointments[0].ResourceId;
							Scheduler.Storage.Appointments.Add(apt);
							}
						}
					pop_recurrence.JSProperties["cp_bind"] = "1";
					}

				}
			}
		else
			{
			var aptid = e.Parameter;
			lblaptid.Text = aptid;
			if (Convert.ToInt32(aptid) < 200000000)
				{
				var apt = new NeAppointment(Convert.ToInt32(aptid));
				ASPxCalendar1.HighlightToday = false;
				ASPxCalendar1.SelectedDate = apt.StartTime.Date;
				lbl_customer_rec.Text = apt.Description;
				lbl_description_rec.Text = _tools.getSQL_string(@"Select woprog_description from woprog  where woprog_id =@v0", new object[] { apt.woprog_id });
				lbl_original_date.Text = apt.StartTime.Date.ToLongDateString();
				lbl_start_and_end.Text = apt.StartTime.ToShortTimeString() + " to " + apt.EndTime.ToShortTimeString();
				lbl_member_rec.Text = Scheduler.SelectedResource.Caption;
				ASPxCalendar1.MinDate = System.DateTime.Today;
				//		Session["rec_member_sched"] = _tools.getSQL_datatable(@"Select date(appointments.startdate) m, Status from appointments  where appointments.resourceid =@v0", new object[] { Scheduler.SelectedResource.Id });
				Session["wop_rec_member_sched"] = _tools.getSQL_datatable(@"Select date(appointments.startdate) m, `Status` from appointments  where appointments.resourceid =@v0 UNION SELECT vacation_master.date_start m, 99 `Status` FROM vacation_master inner join vacation_status on vacation_status.status_id = vacation_master.Status where (vacation_master.Status = 1 or vacation_master.Status = 3) and vacation_master.member_id=@v1  and date_start is not null and date_end is not null", new object[] { Scheduler.SelectedResource.Id, Scheduler.SelectedResource.Id });
				}
			else
				{
				ASPxCalendar1.SelectedDate = Scheduler.SelectedAppointments[0].Start.Date;
				lbl_customer_rec.Text = Scheduler.SelectedAppointments[0].Description;
				lbl_member_rec.Text = Scheduler.SelectedResource.Caption;
				Session["wop_rec_member_sched"] = _tools.getSQL_datatable(@"Select date(wo_freeze.dt_start) m, 70 `Status` from wo_freeze  where wo_freeze.woprog_id =@v0", new object[] { Scheduler.SelectedResource.Id });

				}


			}

		}
	protected void ASPxCalendar1_DayCellPrepared(object sender, CalendarDayCellPreparedEventArgs e)
		{
		if (Session["wop_rec_member_sched"] != null)
			{
			var dt = (DataTable)Session["wop_rec_member_sched"];
			if (e.Date >= System.DateTime.Today)
				{
				if (dt.Select("m='" + e.Date + "'").Length > 0)
					{
					var dr = dt.Select("m='" + e.Date.ToString("yyyy-MM-dd") + "'").CopyToDataTable();
					if (Convert.ToString(dr.Rows[0][1]) == "70")
						{

						e.Cell.Attributes["style"] = "pointer-events: none; color: LightGray; background-color: LimeGreen";
						}

					}
				}
			}
		}
	protected void Scheduler_CustomJSProperties1(object sender, CustomJSPropertiesEventArgs e)
		{

		}
	protected void pop_edit_WindowCallback1(object source, DevExpress.Web.PopupWindowCallbackArgs e)
		{
		if (e.Parameter.Length > 0)
			{
			if (e.Parameter == "Save")
				{
				var apt = new NeAppointment(Convert.ToInt32(Scheduler.SelectedAppointments[0].Id));
				apt.assetid = Convert.ToInt32(ddl_truck.Value);
				apt.StartTime = new DateTime(apt.StartTime.Year, apt.StartTime.Month, apt.StartTime.Day, te_start.DateTime.Hour, te_start.DateTime.Minute, 0);
				apt.EndTime = new DateTime(apt.EndTime.Year, apt.EndTime.Month, apt.EndTime.Day, te_end.DateTime.Hour, te_end.DateTime.Minute, 0);
				apt.Id = Convert.ToInt32(Scheduler.SelectedAppointments[0].Id);
				apt.Save();
				}
			else if (e.Parameter == "Delete")
				{

				Toolbox.doSQL_void(@"Delete from appointments where id = @v0 limit 1", new object[] { Scheduler.SelectedAppointments[0].Id });
				}
			else
				{
				var apt = new NeAppointment(Convert.ToInt32(e.Parameter));
				var wo = new NeWOProg(Convert.ToInt32(apt.woprog_id));
				var contact = new NEContact(Convert.ToInt32(wo.woprog_Contact_ID));

				ddl_truck.Value = apt.assetid;
				lblcontact.Text = contact.name;
				lblphone.Text = contact.cellphone;
				lbllocation.Text = wo.ShipToAddress;
				te_start.DateTime = apt.StartTime;
				te_end.DateTime = apt.EndTime;
				lbldate.Text = apt.StartTime.ToLongDateString();
				hl_customer.Text = wo.CustomerName;
				hl_customer.ClientSideEvents.Click = "function (s,e){ boing('../../../sections/customer/index.aspx?customer_id=" + wo.WOProg_Customer_ID + "&from_wo=T','customer',1100,900);}";
				hl_wo.ClientSideEvents.Click = "function (s,e){ boing('../../workorder/index.aspx?woprog_id=" + wo.woprog_id + "&business_unit_id=" + wo.business_unit_id + "','workorder',1100,900);}";
				hl_wo.Text = wo.OrderNumber;
				lblsetby.Text = new NeMember(Convert.ToInt32(apt.setby)).FullName;
				if (apt.ResourceId < 100000000)
					{
					lblresource.Text = Toolbox.doSQL_string(@"Select member_fullname from member where member_id =@v0 ", new object[] { apt.ResourceId });
					}
				else
					{
					lblresource.Text = Toolbox.doSQL_string("Select membertype_name from membertype where membertype_id =@v0 ", new object[] { apt.ResourceId - 100000000 });
					}
				mem_notes.Text = apt.Description;
				}
			}
		}

	protected void gvemps_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{
		if (e.VisibleIndex >= 0)
			{
			if (Convert.ToInt32(ddlpm.Value) == _tools.getSQL_int("select ifnull((Select reports_to from member where reports_to!=0 and member_id = @v0),100000000)", new object[] { e.KeyValue }))
				{
				e.Row.BackColor = System.Drawing.Color.Orange;
				}
			}
		}
	protected void pop_freeze_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
		{
		if (e.Parameter == "save")
			{
			_tools.getSQL_void(@"Insert into wo_freeze (woprog_id,dt_start,dt_end,notes,setby) 
			values (@v0,@v1,@v2,@v3,@v4)",
				new object[] {  Scheduler.SelectedResource.Id,
				te_start2.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
				te_end2.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
				mem_freeze_notes.Text,
				mymember.id });

			mem_freeze_notes.Text = "";
			pop_freeze.JSProperties["cp_close"] = "1";

			}
		else
			{
			te_start2.DateTime = Scheduler.SelectedInterval.Start.AddHours(7.5);
			te_end2.DateTime = te_start2.DateTime.AddHours(8.5);
			mem_freeze_notes.Text = "";
			pop_freeze.JSProperties["cp_close"] = "0";

			}
		}
	protected void Scheduler_CustomCallback(object sender, CallbackEventArgsBase e)
		{
		if (e.Parameter != null)
			{
			var stuff = e.Parameter.Split('|');
			if (stuff[0] == "DELETE_FREEZE")
				{
				_tools.getSQL_void(@"Delete from wo_freeze where id =@v0 ",
					new object[] { Convert.ToInt32(stuff[1]) - 200000000 });
				Scheduler.DataBind();
				}
			}
		}

   
}

#region MenuHelper
public class MenuHelper2
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

public class CustomNavigateForwardCallbackCommand2 : NavigateForwardCallbackCommand
	{
	public CustomNavigateForwardCallbackCommand2(ASPxScheduler scheduler) : base(scheduler) { }

	protected override void ExecuteCore()
		{
		//base.ExecuteCore();
		Control.Start = Control.Start.AddDays(1);
		}
	}

public class CustomNavigateBackwardCallbackCommand2 : NavigateBackwardCallbackCommand
	{
	public CustomNavigateBackwardCallbackCommand2(ASPxScheduler scheduler) : base(scheduler) { }

	protected override void ExecuteCore()
		{
		//base.ExecuteCore();
		Control.Start = Control.Start.AddDays(-1);
		}
	}

public class CreateDayOffAppointment2 : MenuAppointmentCallbackCommand
	{
	public override string Id { get { return "MYAPTMENU"; } }
	public override bool RequiresControlHierarchy { get { return true; } }
	private DateTime start;
	protected DateTime Start { get { return start; } set { start = value; } }


	public CreateDayOffAppointment2(ASPxScheduler control)
		: base(control)
		{

		}

	protected override void ParseParameters(string parameters)
		{
		base.ParseParameters(parameters);
		if (parameters == "dayoff")
			{
			Setdayoff();
			}
		}

	protected override void ExecuteCore()
		{
		base.ExecuteCore();
		}

	protected void Setdayoff()
		{

		}
	}

public class CreateAppointmentCallbackCommand3 : SchedulerCallbackCommand
	{
	public override string Id { get { return "CRTAPT"; } }

	private DateTime start;
	protected DateTime Start { get { return start; } set { start = value; } }


	public CreateAppointmentCallbackCommand3(ASPxScheduler control)
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
		var grid = (ASPxGridView)this.Control.Parent.FindControl("gvemps");
		var hf = (ASPxHiddenField)this.Control.Parent.FindControl("hf");
		var rowValues = (object[])grid.GetRowValues(Convert.ToInt32(hf["row"]), new string[] { "member_id", "member_fullname", "membertype_name", "membertype_id" });
		//		object[] rowValues = (object[])grid.GetRowValues(Convert.ToInt32(hf["row"]), new string[] { "woprog_id", "woprog_bvwo", "woprog_customername", "woprog_address_id", "business_unit_id" });




		var wo = new NeWOProg(Convert.ToInt32(hf["res"]));
		if (Toolbox.doSQL_int(@"Select count(id) from wo_freeze
where woprog_id = @v0  and date(dt_start) <=@v1  and date(dt_end) >=@v1",
				new object[] {
								hf["res"],Start.ToString("yyyy-MM-dd")
								}) > 0)
			{
			throw new Exception("That work order (" + wo.OrderNumber.TrimStart('0') + ") is frozen on that day.  See the WO Planner for details.");
			}
		var mid = hf["mid"].ToString();
		var apt = this.Control.Storage.CreateAppointment(AppointmentType.Normal);
		apt.Description = string.Format("{0} - {1}", wo.OrderNumber.TrimStart('0'), wo.CustomerName);
		apt.Subject = string.Format("{0}", wo.CustomerName);
		apt.Location = string.Format("{0}", wo.woprog_Address_ID);

		apt.CustomFields["quote_id"] = "0";
		apt.CustomFields["business_unit_id"] = string.Format("{0}", wo.business_unit_id);
		apt.StatusId = 97;

		if (Convert.ToInt32(rowValues[0]) > 100000000)
			{
			apt.CustomFields["member_id"] = 0;
			}
		else
			{
			apt.CustomFields["member_id"] = Convert.ToInt32(rowValues[0]);
			if (Convert.ToInt32(mid) == Toolbox.doSQL_int(@"select ifnull((Select reports_to from member where reports_to!=0
 and member_id = " + rowValues[0] + "),100000000)") || mid == apt.CustomFields["member_id"].ToString())
				{
				apt.StatusId = 0;
				}
			}
		apt.CustomFields["membertype_id"] = Convert.ToInt32(rowValues[3]);
		apt.CustomFields["setby"] = mid;
		apt.CustomFields["assetid"] = Toolbox.doSQL_int(@"Select ifnull((Select assets_id from assets where assets_type = 1 
and assets_owner =@v0),0) limit 1", new object[] { rowValues[0] });
		apt.Start = Start.AddHours(7.5);
		apt.End = apt.Start.AddHours(8.5);
		apt.ResourceId = wo.woprog_id;

		this.Control.Storage.Appointments.Add(apt);
		this.Control.DataBind();
		//	this.Control.ActiveView.SelectAppointment(apt);

		//	ShowAppointmentFormByServerIdCallbackCommand showAppointmentFormByServerIdCallbackCommand = new ShowAppointmentFormByServerIdCallbackCommand(this.Control);

		//	showAppointmentFormByServerIdCallbackCommand.Execute(AppointmentIdHelper.GetAppointmentId(apt).ToString());
		}
	}

public class SaveAppointmentCallbackCommand2 : SchedulerCallbackCommand
	{
	public override string Id { get { return "APTSCHANGE"; } }

	private DateTime start;
	protected DateTime Start { get { return start; } set { start = value; } }

	public SaveAppointmentCallbackCommand2(ASPxScheduler control)
		: base(control)
		{

		}

	protected override void ParseParameters(string parameters)
		{


		}

	protected override void ExecuteCore()
		{
		//base.ExecuteCore();

		SaveAppointment();
		}

	protected void SaveAppointment()
		{
		var wo = Control.SelectedAppointments[0].ResourceId.ToString();

		var subject = Control.SelectedAppointments[0].Subject;
		/*		Appointment apt = this.Control.Storage.CreateAppointment(AppointmentType.Normal);
				apt.Description = String.Format("{0} - {1}", rowValues[1].ToString().TrimStart('0'), rowValues[2]);
				apt.Subject = String.Format("{0}", rowValues[2]);
				apt.Location = String.Format("{0}", rowValues[3]);
				apt.CustomFields["woprog_id"] = String.Format("{0}", rowValues[0]);
				apt.CustomFields["quote_id"] = "0";
				apt.CustomFields["business_unit_id"] = String.Format("{0}", rowValues[4]);
				apt.StatusId = 0;
				apt.Start = Start;
				apt.End = Start.AddHours(8);
				apt.ResourceId = Convert.ToInt32(hf["res"]);

				this.Control.Storage.Appointments.Add(apt);
		 */
		//	this.Control.DataBind();
		//	this.Control.ActiveView.SelectAppointment(apt);

		//	ShowAppointmentFormByServerIdCallbackCommand showAppointmentFormByServerIdCallbackCommand = new ShowAppointmentFormByServerIdCallbackCommand(this.Control);

		//	showAppointmentFormByServerIdCallbackCommand.Execute(AppointmentIdHelper.GetAppointmentId(apt).ToString());

		}


	}


