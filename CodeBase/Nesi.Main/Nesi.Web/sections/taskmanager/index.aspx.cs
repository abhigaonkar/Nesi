using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI;
using DevExpress.Web;
using nesi.core;

public partial class sections_taskmanager_index : Page
	{
	Toolbox _tools;
	private NeMember current_user;
	private const int _page_id			= 35;
	NameValueCollection _q;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools									= new Toolbox();
		current_user							= Toolbox.do_handle_authentication(_page_id);
		}

	protected void Page_Load(object sender, EventArgs e)
		{
		Session["visible_business_unit_ids"] = new Current_User().visible_business_units;

		_q = Request.QueryString;

			if ((_q["task_id"] != null) && (_q["task_id"] != "0"))
			{
				hdn_taskid.Value = _q["task_id"];
			}
			else
			{
				hdn_taskid.Value = "0";
			}
			if (!IsPostBack)
			{
				loadpage();
				load_membergrid();
			}
			
				
				
		

		}

	protected void loadpage()
	{
		if (hdn_taskid.Value == "0")
		{
			ddlowner.Value = current_user.id;
			ddlpriority.SelectedIndex = 0;
			lblcreated.Text = current_user.FullName2;
			ddldept.Value = current_user.business_unit_id;
			lblid.Text = "0";
			lblpercent.Text = "0 %";

			ASPxPageControl1.TabPages[1].ClientEnabled = false;
			ASPxPageControl1.TabPages[2].ClientEnabled = false;
			ASPxPageControl1.TabPages[3].ClientEnabled = false;

			
		}
		else
		{

			var task = new NeTask(Convert.ToInt32(hdn_taskid.Value));
			txttask.Text = task.name;
			lblid.Text = task.id.ToString();
			ddlowner.Value = task.owner_int;
			ddlpriority.Value = task.priority;
			lblpercent.Text = task.percent_complete + " %";
			dteDue.Date = task.date_due;
			txtexphours.Text = task.expectedhours.ToString();
			lblcreated.Text = task.cutby;
			ddlparent.Value = task.parent_task;
			ddldept.Value = task.dept;
			ASPxMemo1.Text = task.notes;
			load_membergrid();
			chkemail.Checked = Convert.ToBoolean(task.emails);
			chkmessageboard.Checked = Convert.ToBoolean(task.messageboard);
			spndayswarning.Value = task.notify_warning_days;
			chkrecurring.Checked = Convert.ToBoolean(task.recurring);
			txtrec_days.Text = task.recurring_frequency.ToString();
			ddlresolution.Text = task.recurring_resolution;
			chkprivate.Checked = task.isprivate;

			//HtmlContainerControl frame = (HtmlContainerControl) I99;
			//frame.Attributes.Add("src", "list.aspx?parent_id=" + hdn_taskid.Value);
			//frame.Attributes.Add("height", "649px");
			//frame.Attributes.Add("Scrolling", "no");

			//HtmlContainerControl file_frame = (HtmlContainerControl)files_frame;
			//file_frame.Attributes.Add("src", "/filemanager.aspx?parent_page=task_files&id=" + hdn_taskid.Value);

			if ((task.owner_int != current_user.id)&&(task.cutby_int!=current_user.id))
			{
				var isparentowner=0;
				if (task.parent_task != 0)
				{
					var parent_task = new NeTask(Convert.ToInt32(task.parent_task));
					if ((parent_task.owner_int == current_user.id)||(parent_task.cutby_int==current_user.id))
					{
						isparentowner = 1;
					}
				}
				if ((task.parent_task == 0)&&(isparentowner==0))
				{
					txttask.ClientEnabled = false;
					lblid.ClientEnabled = false;
					ddlowner.ClientEnabled = false;
					ddlpriority.ClientEnabled = false;
					lblpercent.ClientEnabled = false;
					dteDue.ClientEnabled = false;
					txtexphours.ClientEnabled = false;
					lblcreated.ClientEnabled = false;
					ddlparent.ClientEnabled = false;
					ddldept.ClientEnabled = false;
					gv_members.Enabled = false;
					ddladdmember.ClientEnabled = false;
					ASPxMemo1.Text = task.notes;
					load_membergrid();
					chkemail.ClientEnabled = false;
					chkmessageboard.ClientEnabled = false;
					spndayswarning.ClientEnabled = false;
					chkrecurring.ClientEnabled = false;
					txtrec_days.ClientEnabled = false;
					ddlresolution.ClientEnabled = false;
					chkprivate.ClientEnabled = false;
				}
			}


		}
		if (_q["parent_id"] != null)
		{
			ddlparent.Value = Convert.ToInt32(_q["parent_id"]);
			ASPxPageControl1.TabPages[3].ClientEnabled = false;
		}
		else
		{
			ddlparent.Value = 0;
		}


	}

	protected void gv_members_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		if (lblid.Text == "0")
		{
			if ((Session["temp_add_member_to_task"] != null) && (Convert.ToString(Session["temp_add_member_to_task"]) != ""))
			{
				var x = Session["temp_add_member_to_task"].ToString().TrimEnd(',').Split(',');
				var new_session = "";
				var z = 0;
				while (z < x.Length)
				{
					if (e.Values[0].ToString() != x[z])
					{
						new_session += x[z] + ",";
					}
					z++;
				}
				Session["temp_add_member_to_task"] = new_session;
			}
		}
		else
		{

			_tools.getSQL_void(@"delete from task_member  where task_member_task_id = @v0 and task_member_member_id = @v1", new object[] { lblid.Text,ddladdmember.Value });

		}
		load_membergrid();
		e.Cancel = true;
		gv_members.CancelEdit();
	}
	
	protected void cb_members_Callback(object sender, CallbackEventArgsBase e)
	{
		if (ddladdmember.Text != "")
		{
			// first check if it's already been added
			if (_tools.getSQL_int(@"Select ifnull((Select count(task_member_id) from task_member  where task_member_member_id =@v0 and task_member_task_id =@v1 ),0) ", new object[] { ddladdmember.Value,e.Parameter }) == 0)
			{
				// if it's clean, now decide if you're putting into the table, or just saving to a session variable for now
				if (lblid.Text == "0")
				{
					var check_dup = false;
					if ((Session["temp_add_member_to_task"] != null) && (Convert.ToString(Session["temp_add_member_to_task"]) != ""))
					{
						var x = Session["temp_add_member_to_task"].ToString().TrimEnd(',').Split(',');
						var z = 0;
						while (z < x.Length)
						{
							if (ddladdmember.Value.ToString() == x[z])
							{
								check_dup = true;
							}
							z++;
						}
					}

					if (check_dup == false)
					{
						Session["temp_add_member_to_task"] += ddladdmember.Value + ",";
					}
				}
				else
				{
					Session["temp_add_member_to_task"] = null;
					_tools.getSQL_void(@"Insert into task_member (task_member_task_id,task_member_member_id)  values (@v0,@v1)",new object[] { lblid.Text,ddladdmember.Value } );
					
				}

			}
		}
		load_membergrid();
	}

	protected void load_membergrid()
	{
		var dt = _tools.getSQL_datatable("select task_member_member_id,get_name(task_member_member_id) member from task_member order by get_name(task_member_member_id)",null);
		if ((Session["temp_add_member_to_task"] != null) && (Convert.ToString(Session["temp_add_member_to_task"]) != ""))
		{
			var x = Session["temp_add_member_to_task"].ToString().TrimEnd(',').Split(',');
			var z = 0;
			while (z < x.Length)
			{
				dt.Rows.Add(x[z], _tools.getSQL_string("Select get_name(@v0)",new object[] {  x[z] }));
				z++;
			}
		}
		gv_members.DataSource = dt;
		gv_members.DataBind();

	}

	protected void cb_main_Callback(object sender, CallbackEventArgsBase e)
	{
#region validate
		if (dteDue.Text == "")
		{
			throw new Exception("You must select a valid due date");
		}
		if (txttask.Text == "")
		{
			throw new Exception("You must Enter a valid Task Name");
		}
		if (ddldept.Text == "")
		{
			throw new Exception("You must select a valid department");
		} 
		if (ddlowner.Text == "")
		{
			throw new Exception("You must select a valid owner");
		}
		if (chkrecurring.Checked)
		{
			if (ddlresolution.Text == "")
			{
				throw new Exception("You must select a valid resolution for the recurring task");
			}
			try { var x = Convert.ToInt32(txtrec_days.Text); }
			catch { throw new Exception("You must enter a time period for the recurring task"); }
		}

		#endregion
		#region add new
		if (hdn_taskid.Value == "0")
		{
			var task = new NeTask();
			task.id = 0;
			task.name = txttask.Text;
			task.notes = ASPxMemo1.Text;
			task.owner_int = Convert.ToInt32(ddlowner.Value);
			task.cutby_int = current_user.id32;
			task.date_due = Convert.ToDateTime(dteDue.Date);
			task.date_started = System.DateTime.Today;
			task.emails = chkemail.Checked;
			task.messageboard = chkmessageboard.Checked;
			task.notify_warning_days = Convert.ToInt32(spndayswarning.Value);
			task.parent_task = Convert.ToInt32(ddlparent.Value);
			task.percent_complete = 0;
			task.recurring = chkrecurring.Checked;
			if (chkrecurring.Checked)
			{
				task.recurring_frequency = Convert.ToInt32(txtrec_days.Text);
				task.recurring_resolution = ddlresolution.Text;
			}
			task.status_int = 1;
			task.text = false;
			task.isprivate = chkprivate.Checked;
			task.priority = Convert.ToInt32(ddlpriority.Value);
			task.dept = Convert.ToInt32(ddldept.Value);
			task.expectedhours = Convert.ToInt32(txtexphours.Text);
			task.save();
			hdn_taskid.Value = task.id.ToString();
			_tools.getSQL_void(@"Insert into task_history (task_history_date,task_history_memberid,task_history_note,task_history_taskid)  values (@v0,@v1,'Task Created',@v2)",new object[] { System.DateTime.Today.ToString("yyyy-MM-dd"),current_user.id,task.id } );
		}
		else
		{
			var action = "";
			var task = new NeTask(Convert.ToInt32(hdn_taskid.Value));
			if (task.owner_int != Convert.ToInt32(ddlowner.Value))
			{
				action = "Task ownership changed from " + task.owner + " to " +
				         _tools.getSQL_string(@"Select get_name(@v0)",new object[] { Convert.ToInt32(ddlowner.Value) } ) + System.Environment.NewLine;
			}
			if (task.date_due != dteDue.Date)
			{
				action = "Due Date changed from " + task.date_due.ToString("yyyy-MM-dd") + " to " + dteDue.Date.ToString("yyyy-MM-dd") + System.Environment.NewLine;
			}
			task.name = txttask.Text;
			task.notes = ASPxMemo1.Text;
			task.owner_int = Convert.ToInt32(ddlowner.Value);
			task.cutby_int = current_user.id32;
			task.date_due = Convert.ToDateTime(dteDue.Date);
			task.emails = chkemail.Checked;
			task.messageboard = chkmessageboard.Checked;
			task.expectedhours = Convert.ToInt32(txtexphours.Text);
			task.notify_warning_days = Convert.ToInt32(spndayswarning.Value);
				task.parent_task = Convert.ToInt32(ddlparent.Value);
				task.priority = Convert.ToInt32(ddlpriority.Value);
				task.dept = Convert.ToInt32(ddldept.Value);

			task.recurring = chkrecurring.Checked;
			task.isprivate = chkprivate.Checked;
			if (chkrecurring.Checked)
			{
				task.recurring_frequency = Convert.ToInt32(txtrec_days.Text);
				task.recurring_resolution = ddlresolution.Text;
			}
			task.text = false;
			task.save();

			#region decide what to save for history
			if (action != "")
			{
				_tools.getSQL_void(@"Insert into task_history (task_history_date,task_history_memberid,task_history_note,task_history_taskid)  values (@v0,@v1,@v2,@v3)",new object[] { System.DateTime.Today.ToString("yyyy-MM-dd"),current_user.id,action,task.id } );
			}

			#endregion


		}
		
		#endregion
		#region update



		#endregion
	}

	protected void gv_history_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters == "add_note")
		{
			var mem = (ASPxMemo)gv_history.FindTitleTemplateControl("mem_history");

			_tools.getSQL_void(@"Insert into task_history (task_history_date,task_history_memberid,task_history_note,task_history_taskid)  values (@v0,@v1,@v2,@v3)",new object[] { System.DateTime.Today.ToString("yyyy-MM-dd"),current_user.id,_tools.value_to(mem.Text),hdn_taskid.Value } );
		}
		gv_history.DataBind();
	}
}
