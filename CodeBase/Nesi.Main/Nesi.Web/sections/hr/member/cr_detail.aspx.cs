using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Data;
using DevExpress.Web.ASPxTreeList;
using nesi.core;

public partial class sections_hr_member_cr_detail : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 137;

	int crid = 0;
	int id = 0;
	int comp_id = 0;
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
			//			Session["crid"] = null;
		}


		//		if (Session["crid"] == null)
		//		{
		crid = string.IsNullOrEmpty(_q["id"]) || _q["id"] == "0" ? 0 : Convert.ToInt32(_q["id"]);
		//		Session["crid"] = crid;
		//		}
		//		else
		//		{
		//			crid = Convert.ToInt32(Session["crid"]);
		//		}

		if (!IsPostBack)
		{
			if (crid != 0)
			{
				populate_newform();
			}
			else
			{

			}
		}

		var user = new NeMember(id);
		comp_id = user.business_unit_id;


	}
	protected void populate_newform()
	{

		var ASPxRoundPanel1 = (ASPxRoundPanel)ASPxPageControl1.FindControl("ASPxRoundPanel1");
		var ASPxRoundPanel2 = (ASPxRoundPanel)ASPxPageControl1.FindControl("ASPxRoundPanel2");
		var ASPxRoundPanel3 = (ASPxRoundPanel)ASPxPageControl1.FindControl("ASPxRoundPanel3");
		var ASPxRoundPanel4 = (ASPxRoundPanel)ASPxPageControl1.FindControl("ASPxRoundPanel4");
		if (crid > 0)  // if you're editing
		{
			var cr = new NeCoreResponsibilities(crid);

			var lbid = (ASPxLabel)ASPxRoundPanel1.FindControl("lbid");
			var hdnids = (HiddenField)ASPxRoundPanel2.FindControl("hdnids");
			var hdnids0 = (HiddenField)ASPxRoundPanel3.FindControl("hdnids0");
			var hdnids1 = (HiddenField)ASPxRoundPanel4.FindControl("hdnids1");


			lbid.Text = crid.ToString();
			hdnids.Value = lbid.Text;
			hdnids0.Value = lbid.Text;
			hdnids1.Value = lbid.Text;
			hdnids2.Value = lbid.Text;
			hdnids3.Value = lbid.Text;
			ddlgroup.Value = cr.cr_group_id;
			txtdaily.Text = cr.daily;
			txtweekly.Text = cr.weekly;
			txtmonthly.Text = cr.monthly;
			txtquarterly.Text = cr.quarterly;
			txtannually.Text = cr.annually;
			txtas_required.Text = cr.as_required;
			txtcore.Text = cr.core_responsibility;
			mem.Text = cr.description;
			ddlstatus.Text = cr.status;

			ASPxPageControl1.TabPages[1].ClientEnabled = true;
			ASPxPageControl1.TabPages[2].ClientEnabled = true;
			ASPxPageControl1.TabPages[3].ClientEnabled = true;
			btnsave.Text = "Save";

		}
		else
		{
			ASPxPageControl1.TabPages[1].ClientEnabled = false;
			ASPxPageControl1.TabPages[2].ClientEnabled = false;
			ASPxPageControl1.TabPages[3].ClientEnabled = false;
		}

	}


	protected void ASPxComboBox2_Init(object sender, EventArgs e)
	{

		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv_skills.PerformCallback('{0}|' + s.GetValue()+ '|p'); }}", container.KeyValue);

	}



	protected void gv_cr_review_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var p = e.Parameters.Split('|');
		var ASPxRoundPanel1 = (ASPxRoundPanel)ASPxPageControl1.FindControl("ASPxRoundPanel1");
		var lbid = (ASPxLabel)ASPxRoundPanel1.FindControl("lbid");
		//	ASPxGridView gv_cr_review = (ASPxGridView)sender;
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}

		if (p.Length == 1)
		{
			var tb = (ASPxMemo)gv_cr_review.FindTitleTemplateControl("tb_newreview");
			_tools.getSQL_void(@"insert into cr_review (cr_review_question,cr_review_status,cr_review_cr_id) 
values(@v0,@v1,@v2)", new object[] { tb.Text, "Active", crid });
			tb.Text = "";
		}
		else
		{
			if (p[2] == "q")
			{
				_tools.getSQL_void(@"update cr_review 
set cr_review_question = @v0 where cr_review_id =@v1 ", new object[] { p[1], p[0] });
			}
			else if (p[2] == "s")
			{
				_tools.getSQL_void(@"update cr_review 
set cr_review_status = = @v0 where cr_review_id =@v1 " , new object[] { p[1], p[0] });
			}
		}
		gv_cr_review.DataBind();

	}
	protected void ASPxTextBox1_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxMemo;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_cr_review.PerformCallback('{0}|' + s.GetValue()+ '|q'); }}", container.KeyValue);

	}
	protected void ASPxComboBox3_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv_cr_review.PerformCallback('{0}|' + s.GetValue()+ '|s'); }}", container.KeyValue);

	}
	protected void gv_cr_review_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		_tools.getSQL_void(@"delete from cr_review where cr_review_id =@v0 ", new object[] { e.Keys[0] });
		var s = (ASPxGridView)sender;
		s.CancelEdit();
		e.Cancel = true;
	}

	protected void gv_skills_q_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var p = e.Parameters.Split('|');
		var gv_skills_q = (ASPxGridView)sender;
		if (p.Length == 1)
		{

		}
		else
		{
			if (p[2] == "q")
			{
				_tools.getSQL_void(@"update master_skill_review_questions 
set master_skill_review_questions_question =@v0 where master_skill_review_questions_id =@v1 ", new object[] { p[1], p[0] });
			}
		}
		gv_skills_q.DataBind();
	}
	protected void tb_skills_question_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxMemo;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_skills_q.PerformCallback('{0}|' + s.GetValue()+ '|q'); }}", container.KeyValue);

	}
	protected void gv_skills_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var ASPxRoundPanel1 = (ASPxRoundPanel)ASPxPageControl1.FindControl("ASPxRoundPanel1");
		var lbid = (ASPxLabel)ASPxRoundPanel1.FindControl("lbid");
		//	ASPxGridView gv_skills = (ASPxGridView)sender;
		var cb = (ASPxComboBox)gv_skills.FindTitleTemplateControl("ddl_skills");
		var ddlskillpriority = (ASPxComboBox)gv_skills.FindTitleTemplateControl("ddlskillpriority");
		var p = e.Parameters.Split('|');

		if (p.Length == 3)
		{
			if (p[2] == "p")
			{
				_tools.getSQL_void(@"update cr_skills_link set cr_skills_priority_id = @v0 where id = @v1" , new object[] { p[1], p[0] });
				gv_skills.DataBind();
			}
			else if (p[2] == "a")
			{
				if (_tools.getSQL_int(@"Select count(cr_id) from cr_skills_link  where cr_id =@v0 and skills_id =@v1 ", new object[] { crid,p[0] }) == 0)
				{
					_tools.getSQL_void(@"Insert into cr_skills_link (cr_id,skills_id,cr_skills_priority_id) 
values (@v0,@v1,@v2)", new object[] { crid, p[0], p[1] });
					cb.SelectedIndex = -1;
					ddlskillpriority.SelectedIndex = -1;
					cb.Text = null;
					ddlskillpriority.Value = null;
					gv_skills.DataBind();
				}
				else
				{
					cb.SelectedIndex = -1;
					ddlskillpriority.SelectedIndex = -1;
					cb.Text = "";
					cb.DataBind();
					ddlskillpriority.Value = null;
					throw new Exception("This skill is already linked to this core responsibility");

				}
			}
		}
		else if (p[0] != "")
		{
			if (_tools.getSQL_int(@"Select count(cr_id) from cr_skills_link  where cr_id =@v0 and skills_id =@v1 ", new object[] { crid,cb.Value }) == 0)
			{
				_tools.getSQL_void(@"Insert into cr_skills_link (cr_id,skills_id,cr_skills_priority_id)
values (@v0,@v1,@v2)", new object[] { crid, cb.Value, ddlskillpriority.Value });
				cb.SelectedIndex = -1;
				ddlskillpriority.SelectedIndex = -1;
				cb.Text = null;
				ddlskillpriority.Value = null;
				gv_skills.DataBind();
			}
			else
			{
				cb.SelectedIndex = -1;
				ddlskillpriority.SelectedIndex = -1;
				cb.Text = "";
				cb.DataBind();
				ddlskillpriority.Value = null;
				throw new Exception("This skill is already linked to this core responsibility");

			}

		}


	}
	protected void gv_skills_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		//	ASPxRoundPanel ASPxRoundPanel1 = (ASPxRoundPanel)gvcore.FindEditFormTemplateControl("ASPxRoundPanel2");
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var gv_skills = (ASPxGridView)sender;
		_tools.getSQL_void(@"Delete from cr_skills_link where id = @v0", new object[] { e.Keys[0] });
		gv_skills.DataBind();
		gv_skills.CancelEdit();
		e.Cancel = true;
	}
	protected void gv_skills_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		//		ASPxRoundPanel ASPxRoundPanel1 = (ASPxRoundPanel)gvcore.FindEditFormTemplateControl("ASPxRoundPanel2");
		var gv_skills = (ASPxGridView)sender;
		if (e.VisibleIndex >= 0)
		{
			if (e.DataColumn.FieldName == "name")
			{
				e.Cell.ToolTip = gv_skills.GetRowValues(e.VisibleIndex, "description").ToString();
			}
		}

	}

	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}

		if (txtcore.Text == "")
		{
			throw new Exception("You must enter a core responsibility");
		}

		if (crid == 0)
		{

			_tools.getSQL_void(@"Insert into core_responsibilities 
(core_responsibility,member_id,date_added,status,description,daily,weekly,monthly,quarterly,annually,as_required,cr_group_id) 
				values(@v0,@v1,curdate(),@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10)",
				new object[] { txtcore.Text,
					current_user.id ,
					ddlstatus.Text ,
					mem.Text,
				 txtdaily.Text,
				txtweekly.Text,
				txtmonthly.Text,
				txtquarterly.Text ,
				txtannually.Text ,
				txtas_required.Text,
				ddlgroup.Value });


			lbid.Text = _tools.getSQL_int(@"Select id from core_responsibilities order by id desc limit 1", null).ToString();
			//		ScriptManager.RegisterStartupScript(this, this.GetType(), "binder", "location.href='cr_detail.aspx?id=" + lbid.Text +"'", true);
			ASPxWebControl.RedirectOnCallback("cr_detail.aspx?id=" + lbid.Text);
			crid = Convert.ToInt32(lbid.Text);
			ASPxPageControl1.TabPages[1].ClientEnabled = true;
			ASPxPageControl1.TabPages[2].ClientEnabled = true;
			ASPxPageControl1.TabPages[3].ClientEnabled = true;

			populate_newform();
		}
		else
		{
			_tools.getSQL_void(@"update core_responsibilities set core_responsibility = @v0
												,member_id =@v1
												,status=@v2
												,description=@v3
												,weekly=@v4
												,daily=@v5
												,monthly=@v6
												,quarterly=@v7
												,annually=@v8
												,as_required=@v9
												,cr_group_id=@v10
												where id =@v11",
				new object[] {txtcore.Text,
					current_user.id,
ddlstatus.Text,mem.Text,
					txtweekly.Text,
					txtdaily.Text,
txtmonthly.Text,
					txtquarterly.Text,
					txtannually.Text,
					txtas_required.Text,
 ddlgroup.Value,crid
					});

		}

		populate_newform();

	}
	protected void gv_q_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var ddl_skills0 = (ASPxComboBox)gv_q.FindTitleTemplateControl("ddl_skills0");
		if (_tools.getSQL_int(@"select count(id) from cr_certificates  where cr_id =@v0 and certificates_id =@v1 ", new object[] { crid,ddl_skills0.Value }) == 0)
		{
			_tools.getSQL_void(@"Insert into cr_certificates (cr_id,certificates_id) values (@v0,@v1)", new object[] { crid, ddl_skills0.Value });
			gv_q.DataBind();
		}

	}


	protected void gv_q_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}

		_tools.getSQL_void(@"delete from cr_certificates where id =@v0 limit 1", new object[] { e.Keys["id"]});

		e.Cancel = true;
		gv_q.CancelEdit();
		gv_q.DataBind();
	}





	protected void cbmt_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		if (e.Parameter[0].ToString() == "a")
		{
			foreach (var tn in tlmt.GetSelectedNodes())
			{
				if (_tools.getSQL_int(@"select ifnull((Select count(membertype_id) from membertype_responsibilities  where membertype_id =@v0 and core_responsibility_id =@v1 ),0) ", new object[] { tn.GetValue("id"),crid }) == 0)
				{
					_tools.getSQL_void(@"Insert into membertype_responsibilities (membertype_id,core_responsibility_id,mt_cr_priority) 
values (@v0,@v1,@v2)", new object[] { tn.GetValue("id") ,crid,9});
				}
			}

			tlmt.UnselectAll();
		}
		else if (e.Parameter[0].ToString() == "r")
		{
			foreach (var tn in tlmt2.GetSelectedNodes())
			{
				_tools.getSQL_void(@"delete from membertype_responsibilities where id= @v0 limit 1", new object[] { tn.GetValue("id")});
			}

			tlmt2.UnselectAll();
		}
		tlmt2.DataBind();
	}
	protected void ASPxPageControl1_ActiveTabChanged(object source, TabControlEventArgs e)
	{
		if (e.Tab.VisibleIndex == 3)
		{
			gv_q0.DataBind();
		}
		else if (e.Tab.VisibleIndex == 2)
		{
			gv_skills_q.DataBind();
		}
	}
	protected void ASPxButton5_Click(object sender, EventArgs e)
	{
		var dt_crs = _tools.getSQL_datatable(@"Select * from core_responsibilities"  , null);
		foreach (DataRow cr in dt_crs.Rows)
		{
			var dt_skills = _tools.getSQL_datatable(@"Select * from cr_skills_link  where cr_id =@v0", new object[] { cr["id"] });
			foreach (DataRow skill in dt_skills.Rows)
			{
				var dt_certs = _tools.getSQL_datatable(@"Select * from certificate_skill_link  where skill_id =@v0", new object[] { skill["skills_id"] });
				foreach (DataRow cert in dt_certs.Rows)
				{
					if (_tools.getSQL_int(@"Select count(id) from cr_certificates  where cr_id =@v0 and certificates_id =@v1 ", new object[] { cr["id"],cert["certificate_id"] }) == 0)
					{
						_tools.getSQL_void(@"Insert into cr_certificates (cr_id,certificates_id)
values (@v0,@v1)", new object[] { cr["id"] , cert["certificate_id"] });
					}

				}
			}
		}

	}
}


