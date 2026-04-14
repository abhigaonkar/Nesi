using System;
using DevExpress.Web;
using nesi.core;

public partial class sections_member_tickets_modules_ticket_task : NETickets.ticket_task.ticket_task_uc
	{
	public ASPxImage delete_button { get { return img_delete; } }
	protected void Page_Load(object sender, EventArgs e)
		{
		combo_task_assign.DataSource	= this.users;
		combo_task_assign.DataBind();
		this.DataBind();
		}
	}