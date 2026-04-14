#region #usings_callback_cs
using DevExpress.Web;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
#endregion #usings_callback_cs
#region #callbackcommand
public class UserAppointmentSaveCallbackCommand :
			AppointmentFormSaveCallbackCommand
{
	public UserAppointmentSaveCallbackCommand(ASPxScheduler control)
		: base(control)
	{
	
	
	
	}
	protected internal new UserAppointmentFormController Controller
	{
		get { return (UserAppointmentFormController)base.Controller; }
	}

	protected override void AssignControllerValues()
	{
		var woprog_id = (ASPxTextBox)FindControlByID("woprog_id");
		Controller.woprog_id = System.Convert.ToString(woprog_id.Text);
		base.AssignControllerValues();
	}
	protected override AppointmentFormController
			CreateAppointmentFormController(Appointment apt)
	{
		return new UserAppointmentFormController(Control, apt);
	}
}
#endregion