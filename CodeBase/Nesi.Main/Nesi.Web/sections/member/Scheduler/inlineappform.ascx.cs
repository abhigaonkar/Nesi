using System;
//using System.Collections.Generic;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Drawing;
using nesi.core;

public partial class sections_member_scheduler_inlineappform : System.Web.UI.UserControl
	{
	HorizontalAppointmentTemplateContainer Container { get { return (HorizontalAppointmentTemplateContainer)Parent; } }
	HorizontalAppointmentTemplateItems Items { get { return Container.Items; } }

	protected void Page_Load(object sender, EventArgs e)
		{
		var key		= 0;
		var resId	= 0;
		int.TryParse(Container.AppointmentViewInfo.Appointment.StatusKey.ToString(), out key);
		int.TryParse(Container.AppointmentViewInfo.Appointment.ResourceId.ToString(), out resId);

		lblwo.InnerText = Container.AppointmentViewInfo.Appointment.Description.Split('-').GetValue(0).ToString();
		lblsubject.InnerText = Container.AppointmentViewInfo.Appointment.Subject;
		lbltruck.InnerText = Convert.ToString(Container.AppointmentViewInfo.Appointment.CustomFields["truckno"]);
		lblhours.InnerText = Container.AppointmentViewInfo.Appointment.Start.ToString("HH:mm") + "-" + Container.AppointmentViewInfo.Appointment.End.ToString("HH:mm");
		Image1.Visible = Toolbox.ReturnZeroIfNull_int(Container.AppointmentViewInfo.Appointment.CustomFields["meet_at_shop"]) != 0;
		if (key == 99) // 99 denotes uncofirmed day
			{
			lblsubject.InnerText = Container.AppointmentViewInfo.Appointment.Subject;
			lblhours.InnerText = Container.AppointmentViewInfo.Appointment.Start.ToString("MMM dd") + "-" + Container.AppointmentViewInfo.Appointment.End.ToString("MMM dd");
			dc.Style["background-color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGreen);
			}

		else if (key >= 100)// 98 denotes day off or vacation
			{
			dc.Style["background-color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LimeGreen);
			lblwo.InnerText = Container.AppointmentViewInfo.Appointment.Subject;
			if (key == 104)
				{
				lblsubject.InnerText = Container.AppointmentViewInfo.Appointment.Description;
				}
			else if (key != 500)
				{
				lblsubject.InnerText = Container.AppointmentViewInfo.Appointment.Description.Split('-').GetValue(0).ToString().Length > 30 ? Container.AppointmentViewInfo.Appointment.Description.Split('-').GetValue(0).ToString().Substring(0, 29) : Container.AppointmentViewInfo.Appointment.Description.Split('-').GetValue(0).ToString();
				}
			else if (key == 500)
				{
				dc.Style["background-color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightYellow);
				lblsubject.InnerText = Container.AppointmentViewInfo.Appointment.Description.Split('-').GetValue(0).ToString().Length > 30 ? Container.AppointmentViewInfo.Appointment.Description.Split('-').GetValue(0).ToString().Substring(0, 29) : Container.AppointmentViewInfo.Appointment.Description.Split('-').GetValue(0).ToString();
				}
			lblhours.InnerText = Container.AppointmentViewInfo.Appointment.Start.ToString("HH:mm") + "-" + Container.AppointmentViewInfo.Appointment.End.ToString("HH:mm");

			}

		else if (key >= 50 && key <= 69)// 98 denotes day off
			{
			if (key == 69)
				{
				dc.Style["background-color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGray);
				}
			else
				{
				dc.Style["background-color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Yellow);
				}
			}

		else
			{

			if (resId == 0)
				{
				dc.Style["background-color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.WhiteSmoke);
				dc.Style["color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.DarkGray);
				}
			else if (key == 96)
				{
				dc.Style["background-color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.WhiteSmoke);
				dc.Style["color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Gray);
				}
			else
				{
				if (key == 97)
					{
					dc.Style["background-color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.WhiteSmoke);
					dc.Style["color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.DarkGreen);
					//	this.lblhours.Text = "";
					}
				else
					{
					if (Convert.ToString(Container.AppointmentViewInfo.Appointment.CustomFields["quote_id"]).Equals("0"))
						{
						dc.Style["background-color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.DarkGreen);
						dc.Style["color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGreen);
						}
					else
						{
						dc.Style["background-color"] = "#993300";
						dc.Style["color"] = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGray);
						}
					}
				}

			}
		if ((Convert.ToString(Container.AppointmentViewInfo.Appointment.CustomFields["woprog_id"]).Equals("0")) && (Convert.ToString(Container.AppointmentViewInfo.Appointment.CustomFields["quote_id"]).Equals("0")) && (Convert.ToInt32(key) <= 99))
			{
			lbltruck.InnerText = " ";
			lblsubject.InnerText = " ";
			//		this.lblhours.Text = " ";
			}


		}
	string GetLocationText()
		{
		var location = Container.AppointmentViewInfo.Appointment.CustomFields["woprog_id"].ToString();

		if (string.IsNullOrEmpty(location))
			return string.Empty;
		else
			return string.Format("({0})", location);

		}
	}
