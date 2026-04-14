using System;
using System.Web.UI;
using DevExpress.Web.ASPxScheduler;
using nesi.core;

public partial class sections_member_scheduler_woplanner_Tooltip : ASPxSchedulerToolTipBase
{
	public override bool ToolTipShowStem { get { return false; } }
	public override string ClassName { get { return "ASPxClientAppointmentToolTip"; } }
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

	}
	protected override Control[] GetChildControls()
	{
		var controls = new Control[] { lblInterval, lbltype };
	
		return controls;
	
	}


	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var x = e.Parameter;
		
	
			try
			{

				var apt = new NeAppointment(Convert.ToInt32(x));
		
				lblInterval.Text = apt.StartTime.ToString("MMM dd  h:mm tt") + " to " + apt.EndTime.ToString("h:mm tt");
				



				
			}
			catch { }
		
	}
}