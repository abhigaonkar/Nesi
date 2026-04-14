using System;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web;
using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using System.Text.RegularExpressions;
using System.Data;
using nesi.core;

public partial class sections_member_scheduler_single_member_view : System.Web.UI.UserControl
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

		if (!Page.IsPostBack)
		{
			ddlbranch.Value = mymember.business_unit_id;
		}


		ddlbranch.ClientEnabled = mymember.AuthenticatedForPrivilege(97);

		ddlpm.ClientEnabled = mymember.AuthenticatedForPrivilege(96);

		if (!mymember.AuthenticatedForPrivilege(96))
		{
			single_view.OptionsCustomization.AllowAppointmentEdit = UsedAppointmentType.None;
			single_view.OptionsCustomization.AllowAppointmentDrag = UsedAppointmentType.None;
			single_view.OptionsCustomization.AllowAppointmentDelete = UsedAppointmentType.None;
			single_view.OptionsCustomization.AllowDisplayAppointmentForm = AllowDisplayAppointmentForm.Never;

		}
	}
	protected void Page_Load()
	{
		// 95 - view schedule
		// 96 - edit schedule
		// 97 - change branches

		if (!this.Visible)
		{
			return;
		}

		if (!IsPostBack)
		{
			ddlbranch.Value = mymember.business_unit_id;

			ddlpm.DataBind();
			if (ddlpm.Items.FindByValue(mymember.id) != null)
			{
				ddlpm.Value = mymember.id;
			}
			else
			{
				ddlpm.Value = 0;
			}



			Session["resource_view"] = null;
			Session["start_date"] = null;
			Session["scheduler_wos"] = null;
			Session["rec_member_sched"] = null;
			Session["rec_member_vac_sched"] = null;
			Session["start_date_overview"] = null;
			Session["on_call_schedule"] = null;
		}


		//	Scheduler.TimelineView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never;
		default_view();

	}



	protected void default_view()
	{
		ResourceDataSourcex.DataBind();
		Session["start_date"] = Session["start_date"] == null ? System.DateTime.Today.AddHours(6).AddMinutes(30) : single_view.ActiveView.GetVisibleIntervals().Start;
		single_view.Start = Convert.ToDateTime(Session["start_date"]);
		hdn_cid.Value = ddlbranch.Value.ToString();

		if (Session["on_call_schedule"] == null)
		{
			Session["on_call_schedule"] = _tools.getSQL_datatable(@"Select member_id, date from oncall_schedule  where date > curdate() - interval 1 month and business_unit_id =@v0", new object[] { hdn_cid.Value });
		}

		single_view.DataBind();

	}

	protected string GetDayText(DevExpress.Web.ASPxScheduler.TimelineDateHeaderTemplateContainer container)
	{
		return container.Interval.Start.ToString("d");
	}

	protected void ASPxScheduler1_BeforeExecuteCallbackCommand(object sender, DevExpress.Web.ASPxScheduler.SchedulerCallbackCommandEventArgs e)
	{
		//		default_view();
	int diffDate = 1;
	ASPxScheduler control = (ASPxScheduler)sender;
	switch (control.ActiveViewType)
		{
			case SchedulerViewType.Day:
				diffDate = 1;
				break;
			case SchedulerViewType.FullWeek:
				diffDate = 7;
				break;
			case SchedulerViewType.Month:
				diffDate = 30;
				break;
			case SchedulerViewType.WorkWeek:
				diffDate = 7;
				break;
			case SchedulerViewType.Week:
				diffDate = 7;
				break;
			default:
				break;
		}

		if (e.CommandId == SchedulerCallbackCommandId.AppointmentSave)
		{
			//	e.Command = new UserAppointmentSaveCallbackCommand((ASPxScheduler)sender);
			single_view.CancelUpdate();
		}

		else if (e.CommandId == "FORWARD")
		{

			e.Command = new CustomNavigateForwardCallbackCommand_SingleMember(control, diffDate);
		}
		else if (e.CommandId == "BACK")
		{
			e.Command = new CustomNavigateBackwardCallbackCommand_SingleMemeber(control, diffDate);
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
		if (mymember.AuthenticatedForPrivilege(96))
		{
			var menu = e.Menu;
			if (e.Menu.Id == SchedulerMenuItemId.AppointmentMenu)
			{
				var item0 = e.Menu.Items[0];
				var item1 = e.Menu.Items[5];
				e.Menu.Items.Clear();
				//	e.Menu.Items.Add(item0);
				//			menu.ClientSideEvents.ItemClick = String.Format("function(s, e) {{ pop_hour.cp_aptid = scheduler.GetSelectedAppointmentIds()[0]; pop_hour.Show(); }}", Scheduler.ClientInstanceName);
				MenuHelper1.AddMenuItem(menu, 0, "Open", "_open");


				e.Menu.Items.Add(item1);
				//		e.Menu.Items.Add(new MenuItem("Add Recurrence","addrecurrence"));
				menu.ClientSideEvents.ItemClick = string.Format("function(s, e) {{ DefaultViewMenuHandler({0}, s, e); }}", single_view.ClientInstanceName);
				MenuHelper1.AddMenuItem(menu, 2, "Add Recurrence", "addrecurrence");
				MenuHelper1.AddMenuItem(menu, 3, "Confirm", "confirm");
				MenuHelper1.AddMenuItem(menu, 4, "Email", "email");
			}
			if (e.Menu.Id == SchedulerMenuItemId.DefaultMenu)
			{

				e.Menu.ClientSideEvents.PopUp = "appointmentMenu_PopUp";
				var item0 = e.Menu.Items[2];
				var item1 = e.Menu.Items[3];
				e.Menu.Items.Clear();
				e.Menu.Items.Add(item0);
				e.Menu.Items.Add(item1);
				menu.ClientSideEvents.ItemClick = string.Format("function(s, e) {{ DefaultViewMenuHandler({0}, s, e); }}", single_view.ClientInstanceName);


				MenuHelper1.AddMenuItem(menu, 2, "Unpaid Day Off", "1");
				MenuHelper1.AddMenuItem(menu, 3, "App School", "5");
				MenuHelper1.AddMenuItem(menu, 4, "Discipline Day Off", "6");
				MenuHelper1.AddMenuItem(menu, 5, "Illness Day", "2");
				MenuHelper1.AddMenuItem(menu, 6, "Late Appearence", "3");
				MenuHelper1.AddMenuItem(menu, 7, "Shortage of Work", "7");
				MenuHelper1.AddMenuItem(menu, 8, "Add Event", "addevent");

			}
		}
		else
		{
			e.Menu.Items.Clear();
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

	protected void pop_hour_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{
		object x = e.Parameter;
		//		hourview1.cid = Convert.ToInt32(ddlbranch.Value);
		//	hourview1.dt = Convert.ToDateTime(x);
		//hourview1.bind();

	}
	protected void ddl_viewresources_SelectedIndexChanged(object sender, EventArgs e)
	{
		//			Session["resource_view"] = ddl_viewresources.Value.ToString();
		default_view();
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
		args.Properties["cpaptid"] = "";
		args.Properties["cptype"] = args.Appointment.Description;
		args.Properties["cpinterval"] = args.Appointment.Start.ToString("MMM dd  h:mm tt") + " to " + args.Appointment.End.ToString("h:mm tt");
		args.Properties.Add("setby", args.Appointment.CustomFields["setby"]);
		args.Properties.Add("member_id", args.Appointment.ResourceId);
		if (can_edit_all == 1)
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


	}
	protected void Scheduler_AppointmentChanging(object sender, PersistentObjectCancelEventArgs e)
	{

		var apt = (Appointment)e.Object;


		var _mem_sched_by = _tools.getSQL_int(@"Select ifnull((Select reports_to from member  where member_id =@v0),0) ", new object[] { apt.ResourceId });
		if (Convert.ToInt32(apt.ResourceId) > 100000000)
		{
			apt.CustomFields["membertype_id"] = Convert.ToInt32(apt.ResourceId) - 100000000;
		}
		else
		{

			apt.CustomFields["membertype_id"] = 0;
		}

		if (apt.StatusId == 104)
		{
			single_view.CustomJSProperties += new CustomJSPropertiesEventHandler(Scheduler_CustomJSProperties);
			single_view.JSProperties["cpWarning"] = "You can't move vacation entries";
			e.Cancel = true;
		}
		else
		{
			if (!mymember.AuthenticatedForPrivilege(167))  // if the person CAN't edit all appointments
			{
				if (mymember.id != _mem_sched_by)
				{
					if ((mymember.id.ToString() != apt.CustomFields["setby"].ToString()))
					{
						single_view.JSProperties["cpWarning"] = "You are only allowed to adjust appointments you set or that are set to be scheduled by you.  See your branch manager or service coordinator to change these settings.";
						e.Cancel = true;
					}
					else
					{
						//	apt.CustomFields["confirmed"] = false;

						apt.StatusId = 97;
						single_view.JSProperties["cpWarning"] = "";
						single_view.JSProperties["cp_overview_refresh"] = "1";
						e.Cancel = false;
					}
				}
				else
				{
					if ((_tools.getSQL_int(@"Select count(id) from wo_freeze  where woprog_id =@v0 and date(dt_start) <=@v1  and date(dt_end) >=@v2", new object[] { apt.CustomFields["woprog_id"], apt.Start.ToString("yyyy-MM-dd"), apt.End.ToString("yyyy-MM-dd") }) > 0))
					{
						single_view.JSProperties["cpWarning"] = "This work order has been frozen for this date, sorry";
						e.Cancel = true;
					}
					else
					{
						//	apt.CustomFields["confirmed"] = true;
						apt.StatusId = 0;
						//_tools.getSQL_void(@"update appointments set confirmed=1, statusid = 0  where id =@v0", new object[] { apt.Id });
						single_view.JSProperties["cpWarning"] = "";
						single_view.JSProperties["cp_overview_refresh"] = "1";
						e.Cancel = false;
					}
				}
			}
			else  // if the person can edit all appointments
			{
				if ((_tools.getSQL_int(@"Select count(id) from wo_freeze  where woprog_id =@v0 and date(dt_start) <=@v1  and date(dt_end) >=@v2 ", new object[] { apt.CustomFields["woprog_id"], apt.Start.ToString("yyyy-MM-dd"), apt.End.ToString("yyyy-MM-dd") }) > 0))
				{
					single_view.JSProperties["cpWarning"] = "This work order has been frozen for this date, sorry";
					e.Cancel = true;
				}
				else
				{
					if (Convert.ToInt32(apt.ResourceId) < 100000000)  // if the dragged item is an actual member
					{
						apt.StatusId = 0;  // set it to confirmed
					}
					else
					{
						apt.StatusId = 97;   // set it to waiting for confirm
					}
					single_view.JSProperties["cpWarning"] = "";
					single_view.JSProperties["cp_overview_refresh"] = "1";
					e.Cancel = false;
				}
			}

		}
	}

	protected void Scheduler_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
	{
		object x;
		if (single_view.JSProperties.TryGetValue("cpWarning", out x) == false)
		{
			e.Properties.Add("cpWarning", "");
		}
		if (single_view.JSProperties.TryGetValue("cpOperation", out x) == false)
		{
			e.Properties.Add("cpOperation", "");
		}

	}
	protected void Scheduler_AppointmentFormShowing1(object sender, DevExpress.Web.ASPxScheduler.AppointmentFormEventArgs e)
	{

		//		e.Container = new UserAppointmentFormTemplateContainer((ASPxScheduler)sender);
		//		pop_edit.ShowOnPageLoad = true;
		//		pop_edit.JSProperties["cp_aptid"] = e.Appointment.Id;

	}
	protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
	{
		Session["on_call_schedule"] = null;
		Session["scheduler_wos"] = null;
		default_view();
	}
	protected void ddlpm_SelectedIndexChanged(object sender, EventArgs e)
	{
		Session["scheduler_wos"] = null;
		default_view();
	}

	public string GetResourceColor(ResourceHeaderTemplateContainer container)
	{

		if (Convert.ToInt32(container.Resource.Id) > 100000000)
		{
			return System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGoldenrodYellow);
		}
		else
		{
			var sched_by = _tools.getSQL_string(@"Select ifnull((Select reports_to from member  where member_id =@v0),0) ", new object[] { container.Resource.Id });
			if (ddlpm.Value.ToString() == sched_by)
			{
				return System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Orange);
			}
			else
			{
				return System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Transparent);
			}
		}

	}

	public string GetResourceHeight()
	{
		return single_view.TimelineView.Styles.TimelineCellBody.Height.ToString();
	}
	protected void pop_recurrence_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{

	}
	protected void ASPxCalendar1_DayCellPrepared(object sender, CalendarDayCellPreparedEventArgs e)
	{
		if (Session["rec_member_sched"] != null)
		{
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
					else
					{
						if (Convert.ToString(dr.Rows[0][1]) == "97")  // if its not confirmed
						{
							if (Convert.ToInt32(dr.Rows[0][2]) < 100000000)
							{
								e.Cell.Attributes["style"] = "pointer-events: none; color: LightGray; background-color: Gray";
							}
						}
						else
						{
							e.Cell.Attributes["style"] = "pointer-events: none; color: LightGray; background-color: DarkGreen";
						}
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
		/*		if (e.Parameter.Length > 0)
				{
					if (e.Parameter == "Save")
					{
						NeAppointment apt = new NeAppointment(Convert.ToInt32(single_view.SelectedAppointments[0].Id));
						apt.assetid = Convert.ToInt32(ddl_truck.Value);
						apt.StartTime = new DateTime(apt.StartTime.Year, apt.StartTime.Month, apt.StartTime.Day, te_start.DateTime.Hour, te_start.DateTime.Minute, 0);
						apt.EndTime = new DateTime(apt.EndTime.Year, apt.EndTime.Month, apt.EndTime.Day, te_end.DateTime.Hour, te_end.DateTime.Minute, 0);
						apt.Id = single_view.SelectedAppointments[0].Id;
						apt.notes = Toolbox.AddSlashes(mem_notes.Text);
						apt.Save();
					}
					else if (e.Parameter == "Delete")
					{

						Toolbox.doSQL_void(@"Delete from appointments  where id =@v0 limit 1 ", new object[] { single_view.SelectedAppointments[0].Id });
					}
					else
					{
						NeAppointment apt = new NeAppointment(Convert.ToInt32(e.Parameter));
						NeWOProg wo = new NeWOProg(Convert.ToInt32(apt.woprog_id));
						NEContact contact = new NEContact(Convert.ToInt32(wo.woprog_Contact_ID));

						ddl_truck.Value = apt.assetid;
						lblcontact.Text = contact.name;
						lblphone.Text = contact.cellphone;
						lbllocation.Text = wo.ShipToAddress;
						te_start.DateTime = apt.StartTime;
						te_end.DateTime = apt.EndTime;
						lbldate.Text = apt.StartTime.ToLongDateString();
						hl_customer.Text = wo.CustomerName;
						mem_notes.Text = apt.notes;
						hl_customer.ClientSideEvents.Click = "function (s,e){ boing('../../../sections/customer/index.aspx?customer_id=" + wo.WOProg_Customer_ID + "&from_wo=T','customer',1100,900);}";
						hl_wo.ClientSideEvents.Click = "function (s,e){ boing('../../workorder/index.aspx?woprog_id=" + wo.woprog_id + "&business_unit_id=" + wo.business_unit_id.ToString() + "','workorder',1100,900);}";
						hl_wo.Text = wo.OrderNumber;
						lblsetby.Text = new NeMember(Convert.ToInt32(apt.setby)).FullName;
						if (apt.member_id > 100000)
						{
							lblresource.Text = Toolbox.doSQL_string(@"Select membertype_name from membertype  where membertype_id =@v0", new object[] { (apt.membertype_id) });
						}
						else
						{
							lblresource.Text = Toolbox.doSQL_string(@"Select member_fullname from member  where member_id =@v0", new object[] { apt.member_id });
						}
						mem_notes.Text = apt.Description;
					}
				}
		 */
	}


	protected void pop_event_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{
		/*		if (e.Parameter == "save")
				{

					DateTime dt_start = te_start2.DateTime;
					DateTime dt_end = te_end2.DateTime;
					int setby = mymember.id;
					string comments = mem_event_notes.Text;
					int event_type = Convert.ToInt32(ddl_events.Value) + 50;
					bool whole_branch = chk_event_whole_branch.Checked;
					bool mandatory = chk_mandatory.Checked;
					int pointperson = Convert.ToInt32(ddl_point.Value);
					DateTime start_date = single_view.SelectedInterval.Start.Date;
					int resourceid = Convert.ToInt32(single_view.SelectedResource.Id);
					NeAppointment apt = new NeAppointment();
					if (whole_branch)
					{

						apt.business_unit_id = Convert.ToInt32(ddlbranch.Value);
						apt.Description = ddl_events.Text + " - Point Person " + ddl_point.Text  ;
						apt.StartTime = dt_start.Date.AddHours(dt_start.Hour).AddMinutes(dt_start.Minute);
						apt.EndTime = dt_end.Date.AddHours(dt_end.Hour).AddMinutes(dt_end.Minute);
						apt.member_id = resourceid;
						apt.ResourceId = resourceid;
						apt.notes = comments;
						apt.setby = setby.ToString();
						apt.Status = event_type;
						apt.Subject = mandatory ? "Mandatory: " + ddl_events.Text : ddl_events.Text;
						apt.Save();


					}
					else
					{
						apt.business_unit_id = Convert.ToInt32(ddlbranch.Value);
						apt.Description = ddl_events.Text + " - Point Person " + ddl_point.Text;
						apt.StartTime = dt_start.Date.AddHours(dt_start.Hour).AddMinutes(dt_start.Minute);
						apt.EndTime = dt_end.Date.AddHours(dt_end.Hour).AddMinutes(dt_end.Minute);
						apt.member_id = resourceid;
						apt.ResourceId = resourceid;
						apt.notes = comments;
						apt.setby = setby.ToString();
						apt.Status = event_type;
						apt.Subject = mandatory ? "Mandatory: " + ddl_events.Text : ddl_events.Text;
						apt.Id = 0;
						apt.Save();
					}





					pop_event.JSProperties["cp_close"] = "1";
				}
				else if (e.Parameter=="")
				{
					te_start2.DateTime = single_view.SelectedInterval.Start.Date.AddHours(7.5);
					te_end2.DateTime = single_view.SelectedInterval.Start.Date.AddHours(16);

					pop_event.JSProperties["cp_close"] = "";
				}
		 */
	}
	protected void Scheduler_AllowAppointmentConflicts(object sender, AppointmentConflictEventArgs e)
	{


	}

	protected void Scheduler_AppointmentDeleting(object sender, PersistentObjectCancelEventArgs e)
	{
		single_view.JSProperties["cp_gvworefresh"] = "T";

	}


	protected void ASPxLabel1_Init(object sender, EventArgs e)
	{
		var lbl = (ASPxLabel)sender;
		if (lbl.Text != "")
		{
			if (Convert.ToDouble(lbl.Text) == 0)
			{
				lbl.BackColor = System.Drawing.Color.Green;
			}
			else if (Convert.ToDouble(lbl.Text) < 0)
			{
				lbl.BackColor = System.Drawing.Color.White;
			}
			else if (Convert.ToDouble(lbl.Text) > 0)
			{
				lbl.BackColor = System.Drawing.Color.LightSalmon;
			}
		}


	}
	protected void Scheduler_CustomCallback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter != "")
		{
			Session["start_date"] = Convert.ToDateTime(e.Parameter.Remove(e.Parameter.IndexOf("00:00:00")));
			single_view.Start = Convert.ToDateTime(e.Parameter.Remove(e.Parameter.IndexOf("00:00:00")));

		}
	}

	protected void pop_email_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{
		/*		if (e.Parameter == "send")
				{
					NeEMail email = new NeEMail();
					email.To = txt_email_to.Text;
				//	email.To = "aketelaars@newelectric.com";
					email.Subject = txt_email_subject.Text;
						email.CC = txt_email_cc.Text;
						email.Body = mem_email_message.Html;
						email.From = mymember.NEEmail;
						email.isHTML = true;
						email.Send();
						pop_email.ShowOnPageLoad = false;
				}
				else if (e.Parameter !="")
				{
					NeAppointment apt = new NeAppointment(Convert.ToInt32(e.Parameter));

					if (Convert.ToInt32(e.Parameter) >= 100000)
					{
						DataTable dt = _tools.getSQL_datatable(@"Select * from vacation_master inner join vacation_type on vacation_type.id = vacation_master.type_id  where vacation_master.vacation_id =@v0", new object[] { (Convert.ToInt32(e.Parameter) - 100000) });
						foreach (DataRow dr in dt.Rows)
						{
							NeMember assignee = new NeMember(Convert.ToInt32(8));
							txt_email_to.Text = assignee.NEEmail;
							txt_email_subject.Text = "NE Schedule Notification - " + dr["type"];
							mem_email_message.Html = "<font face='arial'><b>Start:</b> " + Convert.ToDateTime(dr["date_start"]).ToString("yyyy-MM-ddd HH:mm:ss") + "<br>";
							mem_email_message.Html += "<font face='arial'><b>End:</b> " + Convert.ToDateTime(dr["date_end"]).ToString("yyyy-MM-ddd HH:mm:ss") + "<br>";
							mem_email_message.Html += "<font face='arial'><b>Notes:</b> " + dr["comments"] + "<br></font>";
						}
					}
					else
					{
							NeMember assignee = new NeMember(Convert.ToInt32(apt.member_id));
					//	NeMember assignee = new NeMember(Convert.ToInt32(8));
						txt_email_to.Text = assignee.NEEmail;
						txt_email_subject.Text = "NE Schedule Notification - " + apt.Subject;
						mem_email_message.Html = "<font face='arial'><b>Start:</b> " + apt.StartTime.ToString("yyyy-MM-ddd HH:mm:ss") + "<br>";
						mem_email_message.Html += "<font face='arial'><b>End:</b> " + apt.EndTime.ToString("yyyy-MM-ddd HH:mm:ss") + "<br>";
						mem_email_message.Html += "<font face='arial'><b>Notes:</b> " + apt.notes + "<br></font>";
					}
				}
		 */
	}
	protected void single_view_HtmlTimeCellPrepared(object handler, ASPxSchedulerTimeCellPreparedEventArgs e)
	{
		var dt = (DataTable)Session["on_call_schedule"];
		if (dt.Select("[member_id]=" + e.Resource.Id + " and [date] = '" + e.Interval.Start.ToString("yyyy-MM-dd") + "'").Length > 0)
		{
			e.Cell.BackColor = System.Drawing.Color.Red;
		}
	}
}

#region MenuHelper1
public class MenuHelper1
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

public class CustomNavigateForwardCallbackCommand_SingleMember : NavigateForwardCallbackCommand
{
	private int _diffDate;

	public CustomNavigateForwardCallbackCommand_SingleMember(ASPxScheduler scheduler, int diffDate) : base(scheduler)
	{
		_diffDate = diffDate;
	}

	protected override void ExecuteCore()
	{
		//base.ExecuteCore();
		if (_diffDate > 7)
			{
			Control.Start = Control.Start.AddMonths(1);
			}
		else
		{
			Control.Start = Control.Start.AddDays(_diffDate);
		}
	}
}

public class CustomNavigateBackwardCallbackCommand_SingleMemeber : NavigateBackwardCallbackCommand
{
	private int _diffDate;
	public CustomNavigateBackwardCallbackCommand_SingleMemeber(ASPxScheduler scheduler,int diffDate) : base(scheduler)
		{
		_diffDate = diffDate;
		}


	protected override void ExecuteCore()
	{
		//base.ExecuteCore();
	if (_diffDate > 7)
		{
		Control.Start = Control.Start.AddMonths(-1);
		}
	else
		{
		Control.Start = Control.Start.AddDays(-_diffDate);
		}
	}
}



