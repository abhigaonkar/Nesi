#region #usings_form_cs
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.XtraScheduler;
#endregion #usings_form_cs
#region #templatecontainer
public class UserAppointmentFormTemplateContainer :
			 AppointmentFormTemplateContainer
{
	public UserAppointmentFormTemplateContainer(ASPxScheduler control)
		: base(control) { }
	public string woprog_id
	{
		get
		{
			var val = Appointment.CustomFields["woprog_id"];
			return (val == System.DBNull.Value) ? "0" : System.Convert.ToString(val);
		}
	}
	
}
#endregion #templatecontainer

#region #formcontroller
public class UserAppointmentFormController : AppointmentFormController
{
	public UserAppointmentFormController(ASPxScheduler control, Appointment apt)
		: base(control, apt) { }
	public string woprog_id
	{
		get { return (string)EditedAppointmentCopy.CustomFields["woprog_id"]; }
		set { EditedAppointmentCopy.CustomFields["woprog_id"] = value; }
	}
	
	string Source_woprog_id
	{
		get { return (string)SourceAppointment.CustomFields["woprog_id"]; }
		set { SourceAppointment.CustomFields["woprog_id"] = value; }
	}
	
}
#endregion