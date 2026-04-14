using System;
//using System.Collections.Generic;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Drawing;
using nesi.core;
using NESI.Common.Models;

public partial class sections_member_scheduler_inlinewoplanner : System.Web.UI.UserControl
	{
	HorizontalAppointmentTemplateContainer Container { get { return (HorizontalAppointmentTemplateContainer)Parent; } }
	HorizontalAppointmentTemplateItems Items { get { return Container.Items; } }

	protected void Page_Load(object sender, EventArgs e)
	{
        //todo this was a temp fix for null_fullname
	    var temp = Container.AppointmentViewInfo.Appointment.CustomFields["member_fullname"];
        lblsubject.Text = temp == null ? "No Employee Found" : temp.ToString();
        if (Convert.ToInt32(Container.AppointmentViewInfo.Appointment.CustomFields["member_id"]) == 0)
			{
			lbltruck.Text = "";
			if ((Container.AppointmentViewInfo.Appointment.StatusId == OpsSchedulerStatus.FreezeDay) || (Container.AppointmentViewInfo.Appointment.StatusId == OpsSchedulerStatus.Holiday))
				{
				lblsubject.Text = Container.AppointmentViewInfo.Appointment.Subject;
				}

			}
		else
			{
			lbltruck.Text = Convert.ToString(Container.AppointmentViewInfo.Appointment.CustomFields["truckno"]);

			}

		//	this.lblsubject.Text = Container.AppointmentViewInfo.Appointment.Subject;
		lblhours.Text = Container.AppointmentViewInfo.Appointment.Start.ToString("HH:mm") + "-" + Container.AppointmentViewInfo.Appointment.End.ToString("HH:mm");
		var statusId = Toolbox.ReturnZeroIfNull_int(Container.AppointmentViewInfo.Appointment.StatusKey);
		var memberId = Toolbox.ReturnZeroIfNull_int(Container.AppointmentViewInfo.Appointment.CustomFields["member_id"]);
		var woprogId = Toolbox.ReturnZeroIfNull_int(Container.AppointmentViewInfo.Appointment.CustomFields["woprog_id"]);
		if (statusId == OpsSchedulerStatus.VacationDay) // 99 denotes vacation day
			{
			lblhours.Text = Container.AppointmentViewInfo.Appointment.Start.ToString("MMM dd") + "-" + Container.AppointmentViewInfo.Appointment.End.ToString("MMM dd");
			ASPxCallbackPanel1.BackColor = System.Drawing.Color.LightGreen;
			}
		else if (statusId == OpsSchedulerStatus.DayOff)// 98 denotes day off
			{
			ASPxCallbackPanel1.BackColor = System.Drawing.Color.LimeGreen;
			}
		else if (statusId >= OpsSchedulerStatus.Shop && statusId <= OpsSchedulerStatus.BusinessDevelopmentMeeting)
			{
			ASPxCallbackPanel1.BackColor = System.Drawing.Color.Yellow;
			}
		else
			{
			if (memberId == 0)
				{
				ASPxCallbackPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
				ASPxCallbackPanel1.ForeColor = System.Drawing.Color.DarkGray;
				}
			else
				{
				if (statusId == OpsSchedulerStatus.Requested)
					{
					ASPxCallbackPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
					ASPxCallbackPanel1.ForeColor = System.Drawing.Color.DarkGreen;
					}
				else
					{
					//ASPxCallbackPanel1.BackColor = System.Drawing.Color.DarkGreen;
					ASPxCallbackPanel1.ForeColor = System.Drawing.Color.DarkGreen;
					}
				}
			}
		if (woprogId == 0)
			{
			lbltruck.Text = " ";
			lblhours.Text = " ";
			}


		}

	protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{

		}
	}
