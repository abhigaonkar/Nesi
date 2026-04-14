using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_hr_master_review_questions : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 144;
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

		
	}
	

	
	protected void ASPxTextBox1_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxMemo;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_review.PerformCallback('{0}|' + s.GetValue()+ '|q'); }}", container.KeyValue);

	}
	protected void ASPxComboBox2_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv_review.PerformCallback('{0}|' + s.GetValue()+ '|s'); }}", container.KeyValue);

	}
	
	protected void gvrq_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var p = e.Parameters.Split('|');
		var gvrq = (ASPxGridView)sender;
		if (p.Length == 1)
		{
			
			var tb = (ASPxTextBox)gvrq.FindTitleTemplateControl("txt_new");
			var mem = (ASPxMemo)gvrq.FindTitleTemplateControl("mem_new");
			var ddlgroup = (ASPxComboBox)gvrq.FindTitleTemplateControl("ddlgroup");

			if (tb.Text == "")
			{
				throw new Exception("The questions can not be blank");
			}

			_tools.getSQL_void(@"insert into emp_review_items (item,description,`group`,status)
values(@v0,@v1,@v2,@v3)", new object[] {
				 tb.Text, mem.Text, ddlgroup.Value , "Active"});
			
		}
		else
		{
			if (p[2] == "g")
			{
				_tools.getSQL_void(@"update emp_review_items set `group` = @v0  where id = @v1", new object[] {
					p[1] , p[0]});
			}
			else if (p[2] == "i")
			{
				_tools.getSQL_void(@"update emp_review_items set item = @v0 where id = @v1", new object[] {
					p[1], p[0]});
			}
			else if (p[2] == "d")
			{
				_tools.getSQL_void(@"update emp_review_items set description =@v0 where id = @v1" ,new object[] { 
				p[1], p[0]
			})
			;
			}
			else if (p[2] == "s")
			{
				_tools.getSQL_void(@"update emp_review_items set status =  @v0  where id = @v1", new object[] {
					p[1] , p[0]});
			}
		}
		gvrq.DataBind();
	}
	
	protected void ddl_rowgroup_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gvrq.PerformCallback('{0}|' + s.GetValue()+ '|g'); }}", container.KeyValue);

	}
	protected void txt_questionrow_Init(object sender, EventArgs e)
	{
		var item = sender as ASPxMemo;
		var container = item.NamingContainer as GridViewDataItemTemplateContainer;
		item.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gvrq.PerformCallback('{0}|' + s.GetValue()+ '|i'); }}", container.KeyValue);

	}
	protected void mem_descrow_Init(object sender, EventArgs e)
	{
		var desc = sender as ASPxMemo;
		var container = desc.NamingContainer as GridViewDataItemTemplateContainer;
		desc.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gvrq.PerformCallback('{0}|' + s.GetValue()+ '|d'); }}", container.KeyValue);

	}
	protected void ddlstatus_row_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gvrq.PerformCallback('{0}|' + s.GetValue()+ '|s'); }}", container.KeyValue);

	}
	protected void gv_cr_review_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var p = e.Parameters.Split('|');
		var gv_cr_review = (ASPxGridView)sender;
		if (p.Length == 1)
		{
			var tb = (ASPxTextBox)gv_cr_review.FindTitleTemplateControl("tb_newreview");
			var ddlcr = (ASPxComboBox)gv_cr_review.FindTitleTemplateControl("ddl_new_cr_review");
			if (tb.Text == "")
			{
				throw new Exception("Review Question is Blank");
			}
			if (ddlcr.Text == "")
			{
				throw new Exception("You must select a core responsibility to add link this review question to");
			}
			_tools.getSQL_void(@"insert into cr_review
(cr_review_question,cr_review_status,cr_review_cr_id) 
values(@v0,@v1,@v2)",
				new object[] {
 tb.Text ,"Active", ddlcr.Value});
			tb.Text = "";
		}
		else
		{
			if (p[2] == "q")
			{
				_tools.getSQL_void(@"update cr_review set cr_review_question = @v0  where cr_review_id =@v1 " , new object[] {
					p[1], p[0]});
			}
			else if (p[2] == "s")
			{
				_tools.getSQL_void(@"update cr_review set cr_review_status =@v0   where cr_review_id =@v1 ", new object[] {
					p[1], p[0]});
			}
			else if (p[2] == "c")
			{
				_tools.getSQL_void(@"update cr_review set cr_review_cr_id =v0  where cr_review_id =@v1 ", new object[] {
					p[1], p[0]});
			}
		}
		gv_cr_review.DataBind();
	}
	

	protected void txt_cr_review_row_Init(object sender, EventArgs e)
	{
var ddl = sender as ASPxMemo;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_cr_review.PerformCallback('{0}|' + s.GetValue()+ '|q'); }}", container.KeyValue);

	}
	protected void ddl_cr_review_crid_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv_cr_review.PerformCallback('{0}|' + s.GetValue()+ '|c'); }}", container.KeyValue);

	}
	protected void ddl_cr_review_status_Init(object sender, EventArgs e)
	{
var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv_cr_review.PerformCallback('{0}|' + s.GetValue()+ '|s'); }}", container.KeyValue);

	}
	protected void gv_s_review_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (!can_edit)
		{
			throw new Exception("Not Authorized");
		}
		var p = e.Parameters.Split('|');
		var gv_s_review = (ASPxGridView)sender;
		if (p.Length == 1)
		{
			var tb = (ASPxTextBox)gv_s_review.FindTitleTemplateControl("tb_newreview0");
			var ddlcr = (ASPxComboBox)gv_s_review.FindTitleTemplateControl("ddl_new_cr_review0");
			if (tb.Text == "")
			{
				throw new Exception("Review Question is Blank");
			}
			if (ddlcr.Text == "")
			{
				throw new Exception("You must select a skill to add link this review question to");
			}
			_tools.getSQL_void(@"insert into master_skill_review_questions 
(master_skill_review_questions_question,master_skill_review_questions_status,master_skill_review_questions_skill_id) 
values(@v0,@v1,@v2)", new object[] {
				tb.Text ,"Active", ddlcr.Value });
			tb.Text = "";
		}
		else
		{
			if (p[2] == "q")
			{
				_tools.getSQL_void(@"update master_skill_review_questions 
set master_skill_review_questions_question =@v0  where master_skill_review_questions_id =@v1 ", new object[] {
p[1],
 p[0]});
			}
			else if (p[2] == "s")
			{
				_tools.getSQL_void(@"update master_skill_review_questions 
set master_skill_review_questions_status = @v0  where master_skill_review_questions_id =@v1 ", new object[] {
					p[1], p[0]});
			}
			else if (p[2] == "sk")
			{
				_tools.getSQL_void(@"update master_skill_review_questions 
set master_skill_review_questions_skill_id = @v0 where master_skill_review_questions_id = @v1", new object[] {
					p[1], p[0]});
			}
		}
		gv_s_review.DataBind();
	}
	protected void ddl_cr_review_crid0_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv_s_r.PerformCallback('{0}|' + s.GetValue()+ '|sk'); }}", container.KeyValue);

	}
	protected void txt_cr_review_row0_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxMemo;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_s_r.PerformCallback('{0}|' + s.GetValue()+ '|q'); }}", container.KeyValue);

	}
	protected void ddl_cr_review_status0_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv_s_r.PerformCallback('{0}|' + s.GetValue()+ '|s'); }}", container.KeyValue);

	}
	
}
	

	