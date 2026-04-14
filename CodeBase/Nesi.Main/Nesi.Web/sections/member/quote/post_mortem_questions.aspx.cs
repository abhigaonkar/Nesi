using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_member_quote_post_mortem_questions : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 167;
	Toolbox _tools;
	protected void Page_Init()
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = new NePage().get_page_desc(Convert.ToInt32(page_id));
		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var _q = Request.QueryString;
		if (!IsPostBack)
		{
		
		}
	

	}

	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters[0].ToString() == "a")
		{
			
			var tb = (ASPxTextBox)gv.FindTitleTemplateControl("tb");
			var ddl = (ASPxComboBox)gv.FindTitleTemplateControl("ddl_stage");
			if (tb.Text == "")
			{
				throw new Exception("You must enter a valid question");
			}
			if (ddl.Text == "")
			{
				throw new Exception("You must select a valid stage");
			}

			try
			{
				_tools.getSQL_void(@"Insert into quote_post_mortem_questions (question,status,stage) 
values (@v0,@v1,@v2)", new object[] {  tb.Text ,"Active", ddl.Text });
			}
			catch { }
			
			tb.Text = "";
			ddl.Text = "";
			
		}
		else if (e.Parameters[0].ToString() == "s")
		{
			var s = e.Parameters.Split('|');
			_tools.getSQL_void(@"Update quote_post_mortem_questions set status=@v0  where id  =@v1 " , new object[] { s[2], s[1]});
		}
		else if (e.Parameters[0].ToString() == "g")
		{
			var s = e.Parameters.Split('|');
			_tools.getSQL_void(@"Update quote_post_mortem_questions set stage=@v0  where id  =@v1 ", new object[] { s[2], s[1] });
		}
		gv.DataBind();
	}
	protected void ASPxComboBox2_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv.PerformCallback('s|{0}|' + s.GetValue()); }}", container.KeyValue);
	}
	protected void gv_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{

		_tools.getSQL_void(@"Delete from quote_post_mortem_questions where id = @v0", new object[] { e.Keys[0] });


		e.Cancel = true;
		gv.CancelEdit();
		gv.DataBind();
	}
	protected void ASPxComboBox3_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv.PerformCallback('g|{0}|' + s.GetValue()); }}", container.KeyValue);

	}
}
	

	