using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_hr_master_skills : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 143;
	Toolbox _tools;
	private bool can_edit = false;
	protected void Page_Load(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		can_edit = current_user.AuthenticatedForPrivilege(155);
		var _q = Request.QueryString;
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = new NePage().get_page_desc(Convert.ToInt32(page_id));
		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);

		var cb = (ASPxComboBox)gvskills.FindTitleTemplateControl("ddlbranch");

		if (!IsPostBack)
		{
			cb.Text = current_user.business_unit_name;
			cb.Value = current_user.business_unit_id;
			cb.DataBind();
			Session["gv_skill_id"] = null;
		}
		hdncompany.Value = cb.Value.ToString();
		if (Session["gv_skill_id"] != null)
		{
			hdnskill.Value = Session["gv_skill_id"].ToString();
		}


	}


	protected void gvtypes_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var pc = (ASPxPageControl)gvskills.FindEditFormTemplateControl("ASPxPageControl1");
		var ASPxRoundPanel1 = (ASPxRoundPanel)pc.FindControl("ASPxRoundPanel1");
		var skill = (ASPxTextBox)ASPxRoundPanel1.FindControl("txtskill");
		var ddlstatus = (ASPxComboBox)ASPxRoundPanel1.FindControl("ddlstatus");
		var lbid = (ASPxLabel)ASPxRoundPanel1.FindControl("lbid");
		var mem = (ASPxMemo)ASPxRoundPanel1.FindControl("mem");
		var mem2 = (ASPxMemo)ASPxRoundPanel1.FindControl("mem2");
		var chk = (ASPxCheckBox)ASPxRoundPanel1.FindControl("chkcanbetrained");
		var ddltype = (ASPxComboBox)ASPxRoundPanel1.FindControl("ddltype");

		if (skill.Text == "")
		{
			throw new Exception("You must enter a skill");
		}
		_tools.getSQL_void(@"update master_skills set name = @v0
												,status=@v1
												,description=@v2
												,course=@v3
												,can_be_trained=@v4
												,last_modified=curdate()
												,skilltype=@v5 
												where id =@v6" , new object[] {
			skill.Text,
			ddlstatus.Text,
			mem.Text,
			mem2.Text,
			chk.Checked,
			ddltype.Text,
			lbid.Text
		});
		gvskills.CancelEdit();
		gvskills.DataBind();
		e.Cancel = true;

	}
	protected void gvtypes_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var pc = (ASPxPageControl)gvskills.FindEditFormTemplateControl("ASPxPageControl1");
		var ASPxRoundPanel1 = (ASPxRoundPanel)pc.FindControl("ASPxRoundPanel1");
		var core = (ASPxTextBox)ASPxRoundPanel1.FindControl("txtskill");
		var ddlstatus = (ASPxComboBox)ASPxRoundPanel1.FindControl("ddlstatus");
		var ddltype = (ASPxComboBox)ASPxRoundPanel1.FindControl("ddltype");
		var lbid = (ASPxLabel)ASPxRoundPanel1.FindControl("lbid");
		var mem = (ASPxMemo)ASPxRoundPanel1.FindControl("mem");
		var mem2 = (ASPxMemo)ASPxRoundPanel1.FindControl("mem2");
		var chk = (ASPxCheckBox)ASPxRoundPanel1.FindControl("chkcanbetrained");


		if (core.Text == "")
		{
			throw new Exception("You must enter a skill");
		}

		_tools.getSQL_void(@"Insert into master_skills (name,status,description,course,can_be_trained,skilltype) 
													values(@v0,@v1,@v2,@v3,@v4,@v5)", new object[] {
			core.Text, ddlstatus.Text, mem.Text, 
			mem2.Text, chk.Checked, ddltype.Text });

		gvskills.CancelEdit();
		gvskills.DataBind();
		e.Cancel = true;

		var newid = _tools.getSQL_int(@"Select id from master_skills order by id desc limit 1"  , null);
		gvskills.StartEdit(gvskills.FindVisibleIndexByKeyValue(newid));


	}

	protected void gvskills_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
	{
		var pc = (ASPxPageControl)gvskills.FindEditFormTemplateControl("ASPxPageControl1");
		var ASPxRoundPanel1 = (ASPxRoundPanel)pc.FindControl("ASPxRoundPanel1");
		var rp_review = (ASPxRoundPanel)pc.FindControl("rp_review");


		if (gvskills.EditingRowVisibleIndex >= 0)
		{
			var lbid = (ASPxLabel)ASPxRoundPanel1.FindControl("lbid");
			lbid.Text = gvskills.GetRowValues(gvskills.EditingRowVisibleIndex, "id").ToString();
			hdnskill.Value = lbid.Text;
			var hdnids = (HiddenField)rp_review.FindControl("hdnids");
			Session["gv_skill_id"] = lbid.Text;
			var btnsave = (ASPxButton)gvskills.FindEditFormTemplateControl("btnsave");
			btnsave.Text = "Save and Close";
		}
		if (gvskills.IsNewRowEditing)
		{
			var ddltype = (ASPxComboBox)ASPxRoundPanel1.FindControl("ddltype");
			ddltype.Text = "General";
			rp_review.ClientVisible = false;
			Session["gv_skill_id"] = null;
		}


	}

	protected void gvskills_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (e.DataColumn.FieldName == "name")
			{
				e.Cell.ToolTip = gvskills.GetRowValues(e.VisibleIndex, "description").ToString();
			}
		}
	}



	protected void ddlbranch_Init(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			var c = (ASPxComboBox)sender;
			c.Value = current_user.business_unit_id;
			c.DataBind();
		}
	}
	protected void gvskills_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (gvskills.GetRowValues(e.VisibleIndex, "a").ToString() == "0")
			{
				e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			}
		}

	}
	protected void ddltype_DataBound(object sender, EventArgs e)
	{
		if (gvskills.IsNewRowEditing)
		{
			var pg = (ASPxPageControl)gvskills.FindEditFormTemplateControl("ASPxPageControl1");
			var ASPxRoundPanel1 = (ASPxRoundPanel)pg.FindControl("ASPxRoundPanel1");
			var ddltype = (ASPxComboBox)ASPxRoundPanel1.FindControl("ddltype");
			ddltype.Text = "General";

		}

	}
	protected void gvskills_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var editedrow = gvskills.EditingRowVisibleIndex;
		if (e.Parameters[0].ToString() != "x")
		{
			if (e.Parameters == "next")
			{
				gvskills.StartEdit(editedrow + 1);
			}
			else
			{
				gvskills.StartEdit(editedrow - 1);
			}
		}
		if (e.Parameters[0].ToString() == "x")
		{
			var search = e.Parameters.Split('|').GetValue(1).ToString();
			if (search == "")
			{
				gvskills.DataBind();
			}
			else
			{

				gvskills.DataBind();
				gvskills.FilterExpression = "[name] like '%" + search + "%' or [description] like '%" + search + "%'";
				gvskills.FilterEnabled = true;

			}
		}
	}
	protected void gv_review_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var p = e.Parameters.Split('|');
		var pc = (ASPxPageControl)gvskills.FindEditFormTemplateControl("ASPxPageControl1");
		var ASPxRoundPanel1 = (ASPxRoundPanel)pc.FindControl("ASPxRoundPanel1");

		var lbid = (ASPxLabel)ASPxRoundPanel1.FindControl("lbid");

		var gv_review = (ASPxGridView)sender;
		if (p.Length == 1)
		{

			var tb = (ASPxMemo)gv_review.FindTitleTemplateControl("txtnewreview");
			_tools.getSQL_void(@"insert into master_skill_review_questions 
(master_skill_review_questions_question,master_skill_review_questions_status,master_skill_review_questions_skill_id) 
values(@v0,@v1,@v2)",
				new object[] { tb.Text ,"Active", lbid.Text });
			tb.Text = "";
		}
		else
		{
			if (p[2] == "q")
			{
				_tools.getSQL_void(@"update master_skill_review_questions
set master_skill_review_questions_question = @v0 
where master_skill_review_questions_id =@v1 ", new object[] {  p[1],p[0]});
			}
			else if (p[2] == "s")
			{
				_tools.getSQL_void(@"update master_skill_review_questions 
set master_skill_review_questions_status = @v0 where master_skill_review_questions_id = @v1", new object[] { p[1], p[0] });
			}
		}


		gv_review.DataBind();

	}
	protected void ASPxTextBox1_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxTextBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_review.PerformCallback('{0}|' + s.GetValue()+ '|q'); }}", container.KeyValue);

	}
	protected void ASPxComboBox2_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv_review.PerformCallback('{0}|' + s.GetValue()+ '|s'); }}", container.KeyValue);

	}
	protected void gv_review_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var s = (ASPxGridView)sender;
		_tools.getSQL_void(@"Delete from master_skill_review_questions where master_skill_review_questions_id =@v0 " , new object[] { e.Keys[0]});
		s.DataBind();
		e.Cancel = true;
		s.CancelEdit();
	}
	protected void gvskills_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{

		gvskills.SettingsText.PopupEditFormCaption = gvskills.GetRowValuesByKeyValue(e.EditingKeyValue, "name").ToString();
	}
	protected void ASPxMemo1_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxMemo;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_review.PerformCallback('{0}|' + s.GetValue()+ '|q'); }}", container.KeyValue);

	}

	protected void gv_certs_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var gvcerts = (ASPxGridView)sender;
		var ddlcerts = (ASPxComboBox)gvcerts.FindTitleTemplateControl("ddlcerts");
		var ddlcerts0 = (ASPxComboBox)gvcerts.FindTitleTemplateControl("ddlcerts0");

		_tools.getSQL_void(@"Insert into certificate_skill_link (certificate_id,skill_id,alternate_certificate_id) 
values (@v0,@v1,@v2)", new object[] { ddlcerts.Value, Session["gv_skill_id"], (ddlcerts0.Value == null ? 0 : ddlcerts0.Value) });

		ddlcerts.Text = "";
		ddlcerts0.Text = "";
		gvcerts.DataBind();


	}
	protected void gv_certs_RowDeleting1(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		var gvcerts = (ASPxGridView)sender;
		_tools.getSQL_void(@"Delete from certificate_skill_link where id = @v0 limit 1", new object[] { e.Keys[0]});

		e.Cancel = true;
		gvcerts.CancelEdit();
		gvcerts.DataBind();

	}

}

	