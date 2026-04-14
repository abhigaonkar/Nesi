using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class training : Page
	{
	NeMember mymember;
	private static int _PAGE_ID = 55;  //training Page
	private static string _history_see = "47";
	private static string _history_edit = "113";
	private static string _admin = "46";
	protected bool can_see_history;
	Toolbox _tools;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		mymember = Toolbox.do_handle_authentication(_PAGE_ID);
		can_see_history = mymember.AuthenticatedForPrivilege(Convert.ToInt32(_history_edit));
 ds_companies.SelectCommand = "SELECT id, ddl_name name from business_unit  WHERE find_in_set(id,'" + new Current_User().visible_business_units + "')";
        ds_companies.DataBind();
		}

	protected void Page_Load(object sender, EventArgs e)
		{
       

        var _q = Request.QueryString;
		if (!IsPostBack)
			{
			if (Session["working_business_unit_id"] != null)
				{
				ddlcompany.Value = Convert.ToInt32(Session["working_business_unit_id"]);
				}
			else
				{
				ddlcompany.Value = mymember.business_unit_id;
				Session["working_business_unit_id"] = ddlcompany.Value.ToString();
				}
			}

		var menu = new NeMenu(mymember, Convert.ToInt32(_PAGE_ID));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(mymember);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = _tools.getSQL_string(@"SELECT Page_Desc FROM page  WHERE Page_Id =@v0", new object[] { _PAGE_ID });

		fill_history();
		if (mymember.AuthenticatedForPrivilege(Convert.ToInt32(_admin)))
			{
			ASPxPageControl1.TabPages[0].ClientEnabled = true;
			}
		else
			{
			ASPxPageControl1.ActiveTabIndex = 1;
			}
		if (mymember.AuthenticatedForPrivilege(Convert.ToInt32(_history_see)))
			{
			ASPxPageControl1.TabPages[2].ClientEnabled = true;
			ASPxPageControl1.ActiveTabIndex = 2;
		}
		else
			{
			ASPxPageControl1.ActiveTabIndex = 1;
			}
		}

	protected void fill_history()
		{
		ddlcompany.DataBind();
		gv_history.DataBind();
		gv_history.FilterExpression = "business_unit_id = " + ddlcompany.Value;
		}

	protected void gv_modules_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{
		if (e.VisibleIndex >= 0)
			{
			e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#" + gv_modules.GetRowValues(e.VisibleIndex, "colour"));
			}
		}
	protected void gv_modules_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		try
			{
			var mod		= new NeTrainingModule(Convert.ToInt32(e.Keys[0]));
			mod.name					= e.NewValues["name"] == null ? "" : e.NewValues["name"].ToString();
			mod.description				= e.NewValues["description"] == null ? "" : e.NewValues["description"].ToString();
			mod.is_active				= e.NewValues["is_active"].ToString() == "1";
			mod.is_certificate			= e.NewValues["is_certificate"].ToString() == "True";
			mod.is_mandatory			= e.NewValues["is_mandatory"].ToString();
			mod.colour					= mod.colour;
			mod.save();
			}
		catch
			{
			throw new Exception("Could not update training table");
			}

		e.Cancel = true;
		gv_modules.CancelEdit();
		gv_curriculum.DataBind();

		}
	protected void gv_modules_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
		try
			{
			var mod		= new NeTrainingModule();
			mod.name					= e.NewValues["name"] == null ? "" : e.NewValues["name"].ToString();
			mod.description = e.NewValues["description"] == null ? "" : e.NewValues["description"].ToString();
			mod.is_active				= e.NewValues["is_active"].ToString() == "1";
			if(e.NewValues.Contains("is_certificate") && e.NewValues["is_certificate"] != null)
				{
				mod.is_certificate			= e.NewValues["is_certificate"].ToString() == "True";
				}
			mod.is_mandatory			= e.NewValues["is_mandatory"].ToString();
			mod.colour					= "FFFFFF";
			mod.save();
			}
		catch(Exception ee)
			{
			_tools.catch_error(ee);
			throw new Exception("Could not insert into training table");
			}
		e.Cancel = true;
		gv_modules.CancelEdit();
		gv_curriculum.DataBind();
		}
	protected void gv_modules_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var gv					= (ASPxGridView) sender;
		var mod			= new NeTrainingModule((int) e.Keys[0]);
		if(mod.is_loaded)
			{
			if(!mod.delete(mod.id))	
				{

				}
			}
		e.Cancel = true;
		gv_curriculum.DataBind();
		}
	protected void gv_curriculum_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
		var curr		= new NETrainingCurriculum();
		curr.module_id					= (int) e.NewValues["module_id"];
		curr.day						= (int) e.NewValues["day"];
		curr.year						= (int) e.NewValues["year"];
		curr.description				= (string) e.NewValues["description"];
		curr.type_id					= (int) e.NewValues["type_id"];
		curr.paid						= Convert.ToBoolean(e.NewValues["paid"]);
		curr.save();

		e.Cancel = true;
		gv_curriculum.CancelEdit();
		gv_curriculum.DataBind();
		}
	protected void gv_curriculum_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var curr		= new NETrainingCurriculum((int) e.Keys[0]);
		curr.module_id					= (int) e.NewValues["module_id"];
		curr.day						= (int) e.NewValues["day"];
		curr.year						= (int) e.NewValues["year"];
		curr.description				= (string) e.NewValues["description"];
		curr.type_id					= (int) e.NewValues["type_id"];
		curr.paid						= Convert.ToBoolean(e.NewValues["paid"]);
		curr.save();

		e.Cancel = true;
		gv_curriculum.CancelEdit();
		gv_curriculum.DataBind();
		}
	protected void gv_curriculum_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var curr		= new NETrainingCurriculum((int) e.Keys[0]);
		if(curr.is_loaded)
			{
			if(!curr.delete(curr.id))
				{

				}
			}
		e.Cancel = true;
		gv_curriculum.CancelEdit();
		}

	protected void gv_history_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		var gvdc = (GridViewDataColumn)gv.Columns["Files"];
		gvdc.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
		}

	protected void gv_history_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
		var gv				= (ASPxGridView)sender;
		var hist		= new NETrainingHistory();
		hist.teacher_name			= e.NewValues["teacher_name"] == null ? "" : e.NewValues["teacher_name"].ToString();
		hist.comments				= e.NewValues["notes"] == null ? "" : e.NewValues["notes"].ToString();
		hist.module_id				= Convert.ToInt32(e.NewValues["module_id"]);
		hist.member_id				= Convert.ToInt32(e.NewValues["member_id"]);
		hist.date					= Convert.ToDateTime(e.NewValues["date"]);
		hist.business_unit_id				= Convert.ToInt32(ddlcompany.Value);
		hist.date_expires			= Convert.ToDateTime(e.NewValues["date_expires"]);
		hist.save();

		var gvdc = (GridViewDataColumn)gv.Columns["Files"];
		gvdc.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
		gv.JSProperties["cp_newrowid"] = hist.id;
		e.Cancel = true;
		gv.CancelEdit();

		}
	protected void gv_history_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var id = Convert.ToInt32(e.Values["id"]);
		var hist		= new NETrainingHistory(id);
		if(hist.is_loaded)
			{
			if(!hist.delete(id))
				{

				}
			}
		e.Cancel = true;
		gv_history.CancelEdit();
		fill_history();
		}
	protected void gv_history_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var id	= Convert.ToInt32(e.Keys[0]);
		var gv = (ASPxGridView)sender;
		var t = new NETrainingHistory(id);
		t.teacher_name = e.NewValues["teacher_name"] == null ? "" : e.NewValues["teacher_name"].ToString();
		t.comments = e.NewValues["notes"] == null ? "" : e.NewValues["notes"].ToString();
		t.module_id = Convert.ToInt32(e.NewValues["module_id"]);
		t.member_id = Convert.ToInt32(e.NewValues["member_id"]);
		t.date = Convert.ToDateTime(e.NewValues["date"]);
		t.business_unit_id = Convert.ToInt32(ddlcompany.Value);
		t.date_expires = Convert.ToDateTime(e.NewValues["date_expires"]);
		t.save();

		e.Cancel = true;
		gv_history.CancelEdit();
		}
	protected void gv_history_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
		{
		e.Cell.Enabled = can_see_history;
		}
	protected void gv_history_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
		{
		e.Enabled = can_see_history;
		}
	protected void ddlcompany_SelectedIndexChanged(object sender, EventArgs e)
		{
        ds_companies.DataBind();
		Session["working_business_unit_id"] = ddlcompany.Value.ToString();
		fill_history();
		}

	}
