using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_member_quote_filter_questions : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 158;
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
			var w = (ASPxSpinEdit)gv.FindTitleTemplateControl("w");
			var tb = (ASPxTextBox)gv.FindTitleTemplateControl("tb");
			var kq = (ASPxCheckBox)gv.FindTitleTemplateControl("kq");
			if (tb.Text == "")
			{
				throw new Exception("You must enter a valid question");
			}

			try
			{
				_tools.getSQL_void(@"Insert into quote_filter_questions (filter_question,kills_quote,max_score,status) 
values (@v0,@v1,@v2,@v3)", new object[] {
				tb.Text, Convert.ToInt16(kq.Value == DBNull.Value ? 0 : kq.Value), Convert.ToInt16(w.Value) ,"Active"});
			}
			catch { }
			w.Value = 1;
			tb.Text = "";
			kq.Value = 0;
			
		}
		else if (e.Parameters[0].ToString() == "s")
		{
			var s = e.Parameters.Split('|');
			_tools.getSQL_void(@"Update quote_filter_questions set status=@v0  where id  =@v1 ", new object[] { s[2]  + s[1]});
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

		_tools.getSQL_void(@"Delete from quote_filter_questions where id  = @v0", new object[] { e.Keys[0] });


		e.Cancel = true;
		gv.CancelEdit();
		gv.DataBind();
	}
}
	

	