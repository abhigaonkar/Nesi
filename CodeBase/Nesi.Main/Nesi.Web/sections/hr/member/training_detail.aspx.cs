using System;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class sections_hr_member_training_detail : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 155;

	int trainingid = 0;
	Toolbox _tools;
	private bool can_edit = false;
	protected void Page_Load(object sender, EventArgs e)
	{

		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		can_edit = current_user.AuthenticatedForPrivilege(155);
		var _q = Request.QueryString;
		if (!IsPostBack)
		{
			Session["trainingid"] = null;
			Session["training_history"] = null;
		}
		if (Session["trainingid"] == null)
		{
			trainingid = string.IsNullOrEmpty(_q["id"]) || _q["id"] == "0" ? 0 : Convert.ToInt32(_q["id"]);
			Session["trainingid"] = trainingid;
			
			hdnid.Value = Session["trainingid"].ToString();
		}
		else
		{
			trainingid = Convert.ToInt32(Session["trainingid"]);
		}

		if (!IsPostBack)
		{
			if (trainingid != 0)
			{
				populate_newform();
			}
			else
			{
				populate_newform();
			}
		}
		fill_history();

	}
	protected void populate_newform()
	{
		
		if (trainingid > 0)  // if you're editing
		{
			lblid.Text = trainingid.ToString();
			hdnid.Value = lblid.Text;
			var dt = _tools.getSQL_datatable(@"Select * from training_header  where id =@v0", new object[] { trainingid });
			txtname.Text = dt.Rows[0]["name"].ToString();
			txtnotes.Text = dt.Rows[0]["notes"].ToString();
			chk_isinternal.Value = Convert.ToInt32(dt.Rows[0]["is_internal"].ToString());
			txtci.Text = dt.Rows[0]["contact_info"].ToString();
			txtpn.Text = dt.Rows[0]["phone_number"].ToString();
			txturl.Text = dt.Rows[0]["url"].ToString();
			txtcost.Text = Convert.ToDouble(dt.Rows[0]["cost"]).ToString("c2");
			spnquality.Value = Convert.ToInt32(dt.Rows[0]["quality"]);
			lbl_last_modified.Text = dt.Rows[0]["last_modified"]== DBNull.Value ?"": Convert.ToDateTime(dt.Rows[0]["last_modified"]).ToString("yyyy-MM-dd");
			ASPxPageControl1.TabPages[1].ClientEnabled = true;
			ASPxPageControl1.TabPages[2].ClientEnabled = true;
			btnsave.Text = "Save";
			div_files.InnerHtml = "Files for : " + txtname.Text + System.Environment.NewLine;
			div_files.InnerHtml += "<iframe src='/filemanager.aspx?parent_page=training_files&id=" + lblid.Text + "' frameborder='no' width='100%' height='220px' scrolling='auto'></iframe>";
			hdnids2.Value = lblid.Text;
			Session["trainingid"] = Convert.ToInt32(lblid.Text);
			fill_history();
		}
		else
		{
			ASPxPageControl1.TabPages[1].ClientEnabled = false;
			ASPxPageControl1.TabPages[2].ClientEnabled = false;
			txtname.Text = "";
			txturl.Text = "";
			txtcost.Text = "";
			txtpn.Text = "";
			txtci.Text = "";
			spnquality.Value = 0;
			chk_isinternal.Value = 0;
			txtnotes.Text = "";
			Session["trainingid"] = null;
		}

	}

	protected void fill_history()
	{
		if (Session["training_history"] == null)
		{
			Session["training_history"] = _tools.getSQL_datatable(@"SELECT training_header_history.id, training_header_history.training_header_id, training_header_history.member_id, training_header_history.business_unit_id, training_header_history.date, training_header_history.external_id, training_header_history.notes, training_header_history.score, training_header_history.membertype_id, training_header_history.cap_training_schedule_id, training_header_history.certs_issued, cap_training_schedule.start_time, cap_training_schedule.end_time, cap_training_schedule.location FROM training_header_history left join cap_training_schedule on training_header_history.cap_training_schedule_id = cap_training_schedule.id  where training_header_history.training_header_id=@v0", new object[] { trainingid });
		}
		gv_history.DataSource = Session["training_history"];
		gv_history.DataBind();
	}

	
	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		double cost = 0;
		try
		{
			if (txtcost.Text.Length > 0)
			{
				cost = double.Parse(txtcost.Text.Replace("$", ""));
			}
		}
		catch { }
		if (trainingid == 0)
		{
			if (!can_edit)
			{
				throw new Exception("Not Authorized");
			}
			_tools.getSQL_void(@"Insert into training_header (name,notes,is_internal,phone_number,contact_info,url,quality,cost) 
values(@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7)", 
new object[] {   txtname.Text,
txtnotes.Text,
Convert.ToInt32(chk_isinternal.Checked) ,
txtpn.Text ,
txtci.Text ,
txturl.Text,
Convert.ToInt32(spnquality.Value),
	cost });

			lblid.Text = _tools.getSQL_int(@"Select id from training_header order by id desc limit 1"  , null).ToString();
			Session["trainingid"] = Convert.ToInt32(lblid.Text);
			trainingid = Convert.ToInt32(lblid.Text);
			Session["training_history"] = null;
			populate_newform();
			

		}
		else
		{
			if (!can_edit)
			{
				throw new Exception("Not Authorized");
			}
			_tools.getSQL_void(@"Update training_header set name='" + txtname.Text + "', notes='" + txtnotes.Text + "',is_internal=" + Convert.ToInt32(chk_isinternal.Checked) + ",phone_number='" + txtpn.Text + "',contact_info='" + txtci.Text + "',url='" + txturl.Text + "',quality=" + Convert.ToInt32(spnquality.Value) + ",cost=" + cost + " where id = " + trainingid);

		}
	}



	protected void gv_history_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{

	}
	protected void cb_Callback1(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var cb = (ASPxCallbackPanel)sender;
		var ddlmember = (ASPxComboBox) cb.FindControl("ddlmember");
		var ddl_cap_schedule = (ASPxComboBox)cb.FindControl("ddl_cap_schedule");
		var ddlcompany = (ASPxComboBox)cb.FindControl("ddlcompany");
		var txtscore = (ASPxTextBox)cb.FindControl("txtscore");
		
		
		var memnotes = (ASPxMemo)cb.FindControl("memnotes");
		var lblerror = (ASPxLabel)cb.FindControl("lblerror");
		var div_schedule_details = (ASPxMemo)cb.FindControl("div_schedule_details");
		var btn_issue_cert = (ASPxButton)cb.FindControl("btn_issue_cert");
		cb.JSProperties["cp_canceledit"] = "0";
		cb.JSProperties["cp_refresh"] = "0";
		lblerror.Text = "";

		if (e.Parameter == "save")
		{
			var cts = new NeCapTraining_Schedule(Convert.ToInt32(ddl_cap_schedule.Value));
			if (ddlmember.Text == "")
			{
				lblerror.Text = "You must select a valid member";
			}
			if (ddlcompany.Text == "")
			{
				lblerror.Text = "You must select a valid branch";
			}
			if (lblerror.Text == "")
			{
				NeCapTraining_History nch;
				if (Session["gv_training_history_id"] == null)
				{
					nch = new NeCapTraining_History();
					nch.id = 0;
					cb.JSProperties["cp_alert"] = "Training Log Added...";
				}
				else
				{
					nch = new NeCapTraining_History(Convert.ToInt32(Session["gv_training_history_id"]));
					nch.id = Convert.ToInt32(Session["gv_training_history_id"]);
					cb.JSProperties["cp_alert"] = "Training Log Edited...";
				}
				nch.date = cts.date;
				nch.start_time = cts.start_time;
				nch.end_time = cts.end_time;
				nch.member_id = Convert.ToInt32(ddlmember.Value);
				nch.business_unit_id = Convert.ToInt32(ddlcompany.Value);
				nch.certs_issued = false;
				nch.cap_training_schedule_id = cts.id;
				nch.score = Convert.ToDouble(txtscore.Text);
				nch.training_header_id = Convert.ToInt32(hdnid.Value);
				nch.external_id = "";
				nch.save();

			//	cb.JSProperties["cp_alert"] = "Training Log Added...";
				cb.JSProperties["cp_refresh"] = "1";
				cb.JSProperties["cp_canceledit"] = "1";

				gv_history.CancelEdit();

				if (nch.date > System.DateTime.Today)
				{
					#region send calendar invte
					try
					{
						var email = new NeEMail();
						email.To = new NeMember(Convert.ToInt32(nch.member_id)).business_unit.branch_manager.NEEmail;
						var org_dev = _tools.getSQL_string(@"select ifnull((Select member_neemail from member  where member_membertype_id = 40 and member_status = 'Active' order by member_id limit 1),'')" , null);
						var trainer = _tools.getSQL_string(@"select ifnull((Select member_neemail from member  where member_membertype_id = 64 and member_status = 'Active' order by member_id limit 1),'')" , null);
						var members_email = new NeMember(Convert.ToInt32(nch.member_id)).NEEmail.Contains("nomail") ? new NeMember(Convert.ToInt32(nch.member_id)).Email : new NeMember(Convert.ToInt32(nch.member_id)).NEEmail;

						email.CC = org_dev + ";" + trainer + ";" + (new NeMember(Convert.ToInt32(nch.member_id)).reports_to != 0 ? new NeMember(new NeMember(Convert.ToInt32(nch.member_id)).reports_to).NEEmail : "");
						email.Subject = "Training Scheduled for " + new NeMember(Convert.ToInt32(nch.member_id)).FullName + " on " + cts.date.ToString("yyyy-MM-dd");
						email.Body = "Training " + new NeCapTraining(nch.training_header_id).name + System.Environment.NewLine;
						email.Body += "Date: " + cts.date.ToString("yyyy-MM_dd") + System.Environment.NewLine;
						email.Body += "Start Time: " + cts.start_time.Hours + ":" + cts.start_time.Minutes.ToString().PadRight(2, '0') + System.Environment.NewLine;
						email.Body += "End Time: " + cts.end_time.Hours + ":" + cts.end_time.Minutes.ToString().PadRight(2, '0') + System.Environment.NewLine;
						email.Body += "Location: " + cts.location;
						email.From = "admin@" + Toolbox.app_setting("DomainForEmail");
						email.Send();

						NeCapTraining_Schedule.send_calendar_request(nch.id);
					}
					catch { }


					#endregion


				}
				
			}
		}
		else if (e.Parameter == "Issue")
		{

			if (ddl_cap_schedule.Value != null && ddl_cap_schedule.Value.ToString() != "")
			{
				var th = new NeCapTraining_History(Convert.ToInt32(gv_history.GetRowValues(gv_history.EditingRowVisibleIndex, "id")));
				if (th.date > System.DateTime.Today)
				{
					th.date = System.DateTime.Today;

				}
				var dt = _tools.getSQL_datatable(@"Select * from certificate_training_link  where training_header_id =@v0", new object[] { th.training_header_id });
				foreach (DataRow dr in dt.Rows)
				{
					_tools.getSQL_void(@"Insert into certificate_history (certificate_id,member_id,business_unit_id,date,notes,score) 
values (@v0,@v1,@v2,@v3,@v4,@v6)", new object[] {
						dr["certificate_id"],
					ddlmember.Value ,
					ddlcompany.Value ,
					th.date.ToString("yyyy-MM-dd"),
					memnotes.Text,
					txtscore.Text});

				}
				btn_issue_cert.ClientEnabled = false;
				cb.JSProperties["cp_alert"] = "Certificates Added to History for " + ddlmember.Text;
				cb.JSProperties["cp_refresh"] = "1";
				cb.JSProperties["cp_canceledit"] = "1";
				_tools.getSQL_void(@"update training_header_history set certs_issued = 1 
				where training_header_history.id =@v0 limit 1", new object[] {  Session["gv_training_history_id"] });
			}

		}
		else
		{
			if (ddl_cap_schedule.Value != "")
			{
				var cts = new NeCapTraining_Schedule(Convert.ToInt32(ddl_cap_schedule.Value));
				div_schedule_details.Text = "Location: " + cts.location + System.Environment.NewLine;
				div_schedule_details.Text += "Start Time: " + cts.start_time + System.Environment.NewLine;
				div_schedule_details.Text += "End Time: " + cts.end_time + System.Environment.NewLine;
				div_schedule_details.Text += "Max Persons: " + cts.max_fill + System.Environment.NewLine;
				div_schedule_details.Text += "Enrolled: " + Toolbox.doSQL_int(@"Select count(id) from training_header_history where cap_training_schedule_id = @v0" , cts.id);

			}
			
		}
	
		
	}
	
	protected void ddlmember_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var cb = (ASPxComboBox)sender;
		cb.DataBind();
	}
	
	protected void gv_history_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		var cb = (ASPxCallbackPanel)gv_history.FindEditFormTemplateControl("cb");
		var btn_issue_cert = (ASPxButton)cb.FindControl("btn_issue_cert");
		btn_issue_cert.ClientVisible = false;
		if (gv_history.IsEditing)
		{
			Session["gv_training_history_id"] = e.EditingKeyValue.ToString();
			
		}
		else
		{
			Session["gv_training_history_id"] = null;
		}

	}
	protected void gv_history_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
	{
		Session["gv_training_history_id"] = null;
	}
protected void gv_history_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		var cb = (ASPxCallbackPanel)gv_history.FindEditFormTemplateControl("cb");
		var ddlmember = (ASPxComboBox)cb.FindControl("ddlmember");
		var ddl_cap_schedule = (ASPxComboBox)cb.FindControl("ddl_cap_schedule");
		var ddlcompany = (ASPxComboBox)cb.FindControl("ddlcompany");
		var txtscore = (ASPxTextBox)cb.FindControl("txtscore");
		var txtext = (ASPxTextBox)cb.FindControl("txtext");
		var dte_date = (ASPxDateEdit)cb.FindControl("dte_date");
		var memnotes = (ASPxMemo)cb.FindControl("memnotes");
		var lblerror = (ASPxLabel)cb.FindControl("lblerror");
		var div_schedule_details = (ASPxMemo)cb.FindControl("div_schedule_details");
		var btn_issue_cert = (ASPxButton)cb.FindControl("btn_issue_cert");

		var th = new NeCapTraining_History(Convert.ToInt32(gv_history.GetRowValues(gv_history.EditingRowVisibleIndex, "id")));
		ddlmember.Value = Convert.ToInt32(th.member_id);
		ddlcompany.Value = th.business_unit_id;
		txtscore.Text = th.score.ToString();
		memnotes.Text = th.notes;
		ddl_cap_schedule.Value = Convert.ToInt32(th.cap_training_schedule_id);
		btn_issue_cert.ClientEnabled = !th.certs_issued;

		if (gv_history.IsNewRowEditing)
		{
			btn_issue_cert.ClientVisible = false;
		}

		if (ddl_cap_schedule.Value != "")
		{
			var cts = new NeCapTraining_Schedule(Convert.ToInt32(ddl_cap_schedule.Value));
			div_schedule_details.Text = "Location: " + cts.location + System.Environment.NewLine;
			div_schedule_details.Text += "Start Time: " + cts.start_time + System.Environment.NewLine;
			div_schedule_details.Text += "End Time: " + cts.end_time + System.Environment.NewLine;
			div_schedule_details.Text += "Max Persons: " + cts.max_fill + System.Environment.NewLine;
			div_schedule_details.Text += "Enrolled: " + Toolbox.doSQL_int(@"Select count(id) from training_header_history where cap_training_schedule_id = @v0" , cts.id);
		}
			
	}
	protected void gv_history_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		Session["training_history"] = null;
		fill_history();
	}
	protected void gv_history_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
	NeCapTraining_Schedule.send_cancel_calendar(Convert.ToInt32(e.Keys[0]));
		_tools.getSQL_void(@"delete from training_header_history where id = @v0" , new object[] { e.Keys[0]});

	
	
		e.Cancel = true;
		gv_history.CancelEdit();
		Session["training_history"] = null;
		fill_history();
		gv_history.JSProperties["cp_refresh_schedule"]="1";
	}
	protected void gv_q_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var ddl_skills0 = (ASPxComboBox)gv_q.FindTitleTemplateControl("ddl_skills0");


		if (_tools.getSQL_int(@"select count(id) from certificate_training_link  where training_header_id=@v0 and certificate_id =@v1 ", new object[] { Session["trainingid"],ddl_skills0.Value }) == 0)
		{
			_tools.getSQL_void(@"Insert into certificate_training_link (training_header_id,certificate_id) 
values (@v0,@v1)", new object[] {  Session["trainingid"] , ddl_skills0.Value });
			ddl_skills0.Text = "";
			gv_q.DataBind();
		}
	}


	protected void gv_q_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		_tools.getSQL_void(@"delete from certificate_training_link where id = @v0 and training_header_id=@v1  limit 1",
			new object[] { 
				e.Keys["id"], Session["trainingid"]
				});

		e.Cancel = true;
		gv_q.CancelEdit();
		gv_q.DataBind();
	}
	protected void gv_schedule_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		if (e.NewValues["date"] == null)
		{
			throw new Exception("You must select a valid date");
		}
		if (e.NewValues["start_time"] == null)
		{
			throw new Exception("You must select a valid start time");
		}
		if (e.NewValues["end_time"] == null)
		{
			throw new Exception("You must select a valid end time");
		}
		if ((e.NewValues["location"] == null)||(e.NewValues["location"] == ""))
		{
			throw new Exception("You must select a valid location");
		}
		if (hdnid.Value=="")
		{
			throw new Exception("Missing training ID number");
		}

		_tools.getSQL_void(@"Insert into cap_training_schedule 
(training_header_id,date,start_time,end_time,location,max_fill) 
values (@v0,@v1,@v2,@v3,@v4,@v5)", new object[] { hdnid.Value, Convert.ToDateTime(e.NewValues["date"]).ToString("yyyy-MM-dd") ,
		(TimeSpan)(e.NewValues["start_time"]) ,
		(TimeSpan)(e.NewValues["end_time"]) ,
		e.NewValues["location"],
		e.NewValues["max_fill"] });
		e.Cancel = true;
		gv_schedule.CancelEdit();
		gv_schedule.DataBind();
	}

	protected void gv_schedule_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
	{
		gv_schedule.SettingsText.PopupEditFormCaption = "Add to Training Schedule";
	}
	protected void gv_schedule_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		gv_schedule.SettingsText.PopupEditFormCaption = "Edit Training Schedule";
	}
	protected void gv_schedule_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{

		if (e.NewValues["date"] == null)
		{
			throw new Exception("You must select a valid date");
		}
		if (e.NewValues["start_time"] == null)
		{
			throw new Exception("You must select a valid start time");
		}
		if (e.NewValues["end_time"] == null)
		{
			throw new Exception("You must select a valid end time");
		}
		if ((e.NewValues["location"] == null) || (e.NewValues["location"] == ""))
		{
			throw new Exception("You must select a valid location");
		}
		if (hdnid.Value == "")
		{
			throw new Exception("Missing training ID number");
		}
		#region cancel all old calendars
		var dt = _tools.getSQL_datatable(@"Select id from training_header_history  where cap_training_schedule_id=@v0", new object[] { e.Keys[0] });
		foreach (DataRow dr in dt.Rows)
		{
			NeCapTraining_Schedule.send_cancel_calendar(Convert.ToInt32(dr["id"]));
		}
		#endregion
	 

		_tools.getSQL_void(@"update cap_training_schedule set date = @v0 " 
			+ ", start_time = @v1" 
			+ ", end_time = @v2"  
			+ ", location = @v3"  
			+ ", max_fill = @v4" 
			+ " where id = @v5" , new object[] {
			Convert.ToDateTime(e.NewValues["date"]).ToString("yyyy-MM-dd"),
			(TimeSpan)(e.NewValues["start_time"]),
			(TimeSpan)(e.NewValues["end_time"]),
			e.NewValues["location"],
			e.NewValues["max_fill"],
			e.Keys[0]
				});


		_tools.getSQL_void(@"update training_header_history set date  =@v0, start_time =@v1, end_time =@v2 where cap_training_schedule_id =@v3 ",
			new object[] {
	Convert.ToDateTime(e.NewValues["date"]).ToString("yyyy-MM-dd"),
		(TimeSpan)(e.NewValues["start_time"]),
			(TimeSpan)(e.NewValues["end_time"]),
			e.Keys[0]
	});

		#region cancel all old calendars
		 dt = _tools.getSQL_datatable(@"Select id from training_header_history  where cap_training_schedule_id=@v0", new object[] { e.Keys[0] });
		foreach (DataRow dr in dt.Rows)
		{
			NeCapTraining_Schedule.send_calendar_request(Convert.ToInt32(dr["id"]));
		}
		#endregion
		
		/*
#region set all new schedules
		dt = _tools.getSQL_datatable(@"Select id from training_header_history  where cap_training_schedule_id=@v0", new object[] { e.Keys[0] });
		foreach (DataRow dr in dt.Rows)
		{
			NeCapTraining_Schedule.send_calendar_request(Convert.ToInt32(dr["id"]));
		}

#endregion
*/
		e.Cancel = true;

		gv_schedule.CancelEdit();
		gv_schedule.DataBind();
	}
	protected void gv_schedule_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		
		_tools.getSQL_void(@"Delete from cap_training_schedule where id =@v0",new object[] { e.Keys[0]});

		var dt = _tools.getSQL_datatable(@"Select id from training_header_history  where cap_training_schedule_id=@v0", new object[] { e.Keys[0] });
		foreach (DataRow dr in dt.Rows)
		{
			NeCapTraining_Schedule.send_cancel_calendar(Convert.ToInt32(dr["id"]));
			_tools.getSQL_void("Delete from training_header_history where id =@v0 limit 1", new object[] { dr["id"]});
		}

		e.Cancel = true;
		gv_schedule.DataBind();
	}




	protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
	{

	}
}
	

	