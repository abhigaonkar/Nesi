using System;
//using System.Collections.Generic;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Drawing;

public partial class sections_member_scheduler_overviewform : System.Web.UI.UserControl
{
	HorizontalAppointmentTemplateContainer Container { get { return (HorizontalAppointmentTemplateContainer)Parent; } }
    HorizontalAppointmentTemplateItems Items { get { return Container.Items; } }

    protected void Page_Load(object sender, EventArgs e) {

		this.lblsched.Text = Convert.ToString(Math.Round((Convert.ToDouble(Container.AppointmentViewInfo.Appointment.CustomFields["daysoff"]) + Convert.ToDouble(Container.AppointmentViewInfo.Appointment.Location)) / 8, 1));
		this.lblheadcount.Text = Convert.ToString(Math.Round(Convert.ToDouble(Container.AppointmentViewInfo.Appointment.Description) / 8,1));
		this.lblrequested.Text = Convert.ToString(Math.Round(Convert.ToDouble(Container.AppointmentViewInfo.Appointment.Subject) / 8, 1));

		if (Convert.ToDouble(this.lblsched.Text) >= Convert.ToDouble(this.lblheadcount.Text))
		{
			this.lblsched.BackColor = System.Drawing.Color.Green;
			this.lblsched.ForeColor = System.Drawing.Color.White;
		}
		else if (Convert.ToDouble(this.lblsched.Text) < Convert.ToDouble(this.lblheadcount.Text))
		{
			this.lblsched.BackColor = System.Drawing.Color.Red;
			this.lblsched.ForeColor = System.Drawing.Color.White;
		}

		if (Convert.ToDouble(Container.AppointmentViewInfo.Appointment.Subject) == 0)
		{
			this.lblrequested.BackColor = System.Drawing.Color.Green;
			this.lblrequested.ForeColor = System.Drawing.Color.White;
		}



		
        
    }
    
}
