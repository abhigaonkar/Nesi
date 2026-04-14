using System;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web;

using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using System.Globalization;
using System.Text.RegularExpressions;
using nesi.core;

public partial class sections_member_scheduler_hourview : System.Web.UI.UserControl
{
	private int lastInsertedAppointmentId;

	public NeMember mymember;
	private static int _page_id = 108;  //Report Page
	private static Regex _numeric = new Regex(@"^\d+$");
	public string callback_control;
	public DateTime dt;
	public int cid;

	protected void Page_Init()
	{
		mymember = Toolbox.do_handle_authentication(_page_id);
		
	}
	protected void Page_Load()
	{
		dt = System.DateTime.Today;
		if (!IsPostBack)
		{

			if (dt.Date > (System.DateTime.Today.AddDays(-200)))
			{
				
			
			}
		}

		bind();
	}

	public void bind()
	{
		if (cid != 0)
		{
			hdn_cid.Value = cid.ToString();
		}
		else
		{
			cid = 8;
			hdn_cid.Value = cid.ToString();
		}
		gridDS1.DataBind();
		ResourceDataSource1.DataBind();
		Scheduler1.ActiveViewType = SchedulerViewType.Timeline;
		Scheduler1.TimelineView.IntervalCount = 28;
		var scales = Scheduler1.TimelineView.Scales;
		scales.BeginUpdate();
		try
		{
			Scheduler1.TimelineView.IntervalCount = 28;
			scales.Clear();
			scales.Add(new CustomTimeScaleDay());
			scales.Add(new CustomTimeScaleHalfHour());
		}
		finally
		{
			scales.EndUpdate();
		}
		Scheduler1.Start = dt.AddHours(6).AddMinutes(30);
		Scheduler1.TimelineView.AppointmentDisplayOptions.StartTimeVisibility = AppointmentTimeVisibility.Always;
		Scheduler1.TimelineView.AppointmentDisplayOptions.EndTimeVisibility = AppointmentTimeVisibility.Always;
		Scheduler1.TimelineView.AppointmentDisplayOptions.TimeDisplayType = AppointmentTimeDisplayType.Text;
		Scheduler1.TimelineView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Always;
		Scheduler1.DataBind();
	}

	protected void ASPxScheduler1_BeforeExecuteCallbackCommand(object sender, DevExpress.Web.ASPxScheduler.SchedulerCallbackCommandEventArgs e)
	{
		if (e.CommandId == "CRTAPT")
		{
			e.Command = new CreateAppointmentCallbackCommand2((ASPxScheduler)sender);
		}
		else if(e.CommandId == "FORWARD") 
		{
			e.Command = new CustomNavigateForwardCallbackCommand1((ASPxScheduler)sender);
        }
		else if (e.CommandId == "BACK")
		{
			e.Command = new CustomNavigateBackwardCallbackCommand1((ASPxScheduler)sender);
		}
        
	}
	protected void SchedulingDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e)
	{
		this.lastInsertedAppointmentId = Toolbox.doSQL_int(@"Select ID from appointments order by id desc limit 1");
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
			if (e.Menu.Id == SchedulerMenuItemId.DefaultMenu)
			{
				var item0 = e.Menu.Items[2];
				var item1 = e.Menu.Items[3];
				var item2 = new DevExpress.Web.MenuItem("ZOOM");
				e.Menu.Items.Clear();
				e.Menu.Items.Add(item2);
				e.Menu.Items.Add(item0);
				e.Menu.Items.Add(item1);
			}
	}
}

public class CustomNavigateForwardCallbackCommand1 : NavigateForwardCallbackCommand
{
	public CustomNavigateForwardCallbackCommand1(ASPxScheduler scheduler) : base(scheduler) { }

	protected override void ExecuteCore()
	{
		//base.ExecuteCore();
		Control.Start = Control.Start.AddDays(1);
	}
}

public class CustomNavigateBackwardCallbackCommand1 : NavigateBackwardCallbackCommand
{
	public CustomNavigateBackwardCallbackCommand1(ASPxScheduler scheduler) : base(scheduler) { }

	protected override void ExecuteCore()
	{
		//base.ExecuteCore();
		Control.Start = Control.Start.AddDays(-1);
	}
}



public class CreateAppointmentCallbackCommand2 : SchedulerCallbackCommand
{
	public override string Id { get { return "CRTAPT"; } }

	private DateTime start;
	protected DateTime Start { get { return start; } set { start = value; } }

	public CreateAppointmentCallbackCommand2(ASPxScheduler control)
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
		var grid = (ASPxGridView)this.Control.Parent.FindControl("gvwos1");
		var hf = (ASPxHiddenField)this.Control.Parent.FindControl("hf1");
		var rowValues = (object[])grid.GetRowValues(Convert.ToInt32(hf["row"]), new string[] { "woprog_id", "woprog_bvwo", "woprog_customername", "woprog_address_id", "business_unit_id" });

		var apt = this.Control.Storage.CreateAppointment(AppointmentType.Normal);
		apt.Description = string.Format("{0} - {1}", rowValues[1].ToString().TrimStart('0'), rowValues[2]);
		apt.Subject = string.Format("{0}", rowValues[2]);
		apt.Location = string.Format("{0}", rowValues[3]);
		apt.CustomFields["woprog_id"] = string.Format("{0}", rowValues[0]);
		apt.CustomFields["quote_id"] = "0";
		apt.CustomFields["business_unit_id"] = string.Format("{0}", rowValues[4]);
		apt.StatusId = 0;
		apt.Start = Start;
		apt.End = Start.AddHours(8);
		apt.ResourceId = Convert.ToInt32(hf["res"]);

		this.Control.Storage.Appointments.Add(apt);
		this.Control.DataBind();
		//	this.Control.ActiveView.SelectAppointment(apt);

		//	ShowAppointmentFormByServerIdCallbackCommand showAppointmentFormByServerIdCallbackCommand = new ShowAppointmentFormByServerIdCallbackCommand(this.Control);

		//	showAppointmentFormByServerIdCallbackCommand.Execute(AppointmentIdHelper.GetAppointmentId(apt).ToString());
	}
}


