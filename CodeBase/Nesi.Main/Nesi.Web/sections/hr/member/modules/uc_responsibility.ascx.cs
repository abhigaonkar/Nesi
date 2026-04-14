using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

public partial class sections_hr_member_modules_uc_responsibility : System.Web.UI.UserControl
	{
	public string Title { get; set; }
	public string Field { get; set; }
	[System.ComponentModel.Description("Sets the DataSource of the 'Responsibility' DDL"),Category("Data")] 
	public object DDLResponsibility_DataSource
		{
		get {return this.ddl_responsibility.DataSource;}
		set {this.ddl_responsibility.DataSource = value;}
		}
	[Description("Sets the DataSource of the 'Assign To' DDL"),Category("Data")] 
	public object DDLAssignTo_DataSource
		{
		get {return this.ddl_assignto.DataSource;}
		set {this.ddl_assignto.DataSource = value;}
		}
	[Description("Accessor for the 'Responsibility' DDL"),Category("Layout")] 
	public DropDownList DDLResponsibility
		{
		get
			{
			return this.ddl_responsibility;
			}
		}
	[Description("Accessor for the 'Assign To' DDL"),Category("Layout")] 
	public DropDownList DDLAssignTo
		{
		get {return this.ddl_assignto;}
		}
	[Description("Sets the DataSource of the 'Notification' Label"),Category("Layout")] 
	public HtmlContainerControl NotificationLabel
		{
		get {return lb_notification;}
		}
	[Description("Whether or not to show the 'Cancel' button"),Category("Layout")]
	public bool show_cancel_button  { get; set; }
	public bool hide_sendto_ddl  { get; set; }
	public bool show_mass_send_button  { get; set; }
	public event EventHandler SendButtonAction;
	public event EventHandler CancelAction;
	[Browsable(true)]
[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public event ImageClickEventHandler CancelButtonAction
		{
		add { bt_cancel.Click += value; }
		remove { bt_cancel.Click -= value; }
		}
	public event EventHandler MassSendAction;
	protected void Page_Load(object sender, EventArgs e)
		{
		if(!IsPostBack)
			{
			bt_cancel.Visible		= show_cancel_button;
			bt_mass_send.Visible	= show_mass_send_button;
			if(hide_sendto_ddl)
				{
				ddl_assignto.Visible	= false;
				bt_send.Visible			= false;
				}
			}
		}

    override protected void OnInit(EventArgs e)
		{
		  this.Load += new System.EventHandler(this.Page_Load);
		  base.OnInit(e);
		}
	public override void DataBind()
		{
		base.DataBind();
		if(this.ddl_responsibility.Items.Count == 0)
			{
			this.Visible			= false;
			}
		}
	protected void bt_send_Click(object sender, ImageClickEventArgs e)
		{
		var returned_status							= "";
		var is_error									= false;
		if(ddl_responsibility.SelectedValue == "")
			{
			is_error			= true;
			returned_status		= "Please select a responsibility.";
			}
		else if(ddl_assignto.SelectedValue == "")
			{
			is_error			= true;
			returned_status		= "Please select an employee to assign this to.";
			}
		else
			{
			returned_status		= "Saved";
			}
		var this_style		= is_error ? "color:#900;" : "color:#090;";
		lb_notification.InnerHtml	= string.Format("<b style='{0}'>{1}</b>", this_style, returned_status);
		if(!is_error)
			{
			ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), string.Format(@"setTimeout(""$('#{0}').fadeOut()"", 5000);", lb_notification.ClientID), true);
			}
		if(SendButtonAction != null)
			{
			SendButtonAction(sender, e);
			}
		}
	protected void bt_mass_send_Click(object sender, ImageClickEventArgs e)
		{
		var returned_status							= "";
		var is_error									= false;
		if(ddl_responsibility.SelectedValue == "")
			{
			is_error			= true;
			returned_status		= "Please select a responsibility.";
			}
		else if(ddl_assignto.SelectedValue == "")
			{
			is_error			= true;
			returned_status		= "Please select an employee to assign this to.";
			}
		else
			{
			returned_status		= "Saved";
			}
		var this_style		= is_error ? "color:#900;" : "color:#090;";
		lb_notification.InnerHtml	= string.Format("<b style='{0}'>{1}</b>", this_style, returned_status);
		if(!is_error)
			{
			ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), string.Format(@"setTimeout(""$('#{0}').fadeOut()"", 5000);", lb_notification.ClientID), true);
			}
		if(MassSendAction != null)
			{
			MassSendAction(sender, e);
			}
		}
	protected void bt_cancel_Click(object sender, ImageClickEventArgs e)
		{
		var returned_status			= "Removed/Canceled";
		var is_error					= false;
		var this_style				= is_error ? "color:#900;" : "color:#090;";
		lb_notification.InnerHtml		= string.Format("<b style='{0}'>{1}</b>", this_style, returned_status);
		if(!is_error)
			{
			ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), string.Format(@"setTimeout(""$('#{0}').fadeOut()"", 5000);", lb_notification.ClientID), true);
			}
		if(CancelAction != null)
			{
			CancelAction(sender, e);
			}
		}
}