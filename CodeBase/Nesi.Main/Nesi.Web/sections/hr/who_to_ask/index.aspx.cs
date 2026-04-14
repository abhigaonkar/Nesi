using System;
using DevExpress.Web;
using nesi.core;

public partial class hr_whotoask_index : System.Web.UI.Page
{
	NeMember myMember;
	private const int _page_id = 130; // from Page table in DB
	
	Toolbox _tools;

	protected void Page_Init(object sender, EventArgs e)
	{
		
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
	}

	protected void Page_Load(object sender, EventArgs e)
	{


		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		if (!IsPostBack && !IsCallback)
		{

		}
	

	}
	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		try
		{
			var sql = "";
			var p = e.Parameter.Split('|');
			if (p[2] == "m")
			{
				sql = @"Update who_to_ask set who_to_ask_memberid=@v1 where who_to_ask_id=@v0" ;
			}
			else if (p[2] == "mt")
			{
				sql = @"Update who_to_ask set who_to_ask_membertype=@v1  where who_to_ask_id=@v0";
			}
			else if (p[2] == "q")
			{
				sql = @"Update who_to_ask set who_to_ask_question=@v1 where who_to_ask_id=@v0";
			}
			_tools.getSQL_void(sql, new object[] { p[1],p[0]});
		}
		catch
		{
			throw new Exception("Couldn't Update Row");
		}
	}
	protected void ASPxComboBox2_Init(object sender, EventArgs e)
	{
		var owner = sender as ASPxComboBox;
		var container = owner.NamingContainer as GridViewDataItemTemplateContainer;
		owner.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ cb.PerformCallback('{0}|' + s.GetValue()+ '|m'); }}", container.KeyValue);

	}
	protected void gv_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		var member = e.NewValues["who_to_ask_memberid"]==null? "0": e.NewValues["who_to_ask_memberid"].ToString();
		var type = e.NewValues["who_to_ask_membertype"] == null ? "0" : e.NewValues["who_to_ask_membertype"].ToString();
		

		try
		{
			_tools.getSQL_void(@"Insert into who_to_ask (who_to_ask_memberid,who_to_ask_membertype,who_to_ask_question) 
values (@v0,@v1,@v2)",
				new object[] {
				member,type, e.NewValues["who_to_ask_question"]});

		}
		catch { }

		e.Cancel = true;
		gv.CancelEdit();
	}
	protected void gv_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		_tools.getSQL_void(@"Delete from who_to_ask where who_to_ask_id = @v0" , new object[] { e.Keys[0]});

		e.Cancel = true;
		gv.CancelEdit();
		gv.DataBind();
	}
	protected void ASPxComboBox3_Init(object sender, EventArgs e)
	{
		var owner = sender as ASPxComboBox;
		var container = owner.NamingContainer as GridViewDataItemTemplateContainer;
		owner.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ cb.PerformCallback('{0}|' + s.GetValue()+ '|mt'); }}", container.KeyValue);

	}
	protected void ASPxTextBox1_Init(object sender, EventArgs e)
	{
		var txt = sender as ASPxTextBox;
		var container = txt.NamingContainer as GridViewDataItemTemplateContainer;
		txt.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb.PerformCallback('{0}|' + s.GetText()+ '|q'); }}", container.KeyValue);
	}
}
