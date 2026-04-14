using System;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_messaging_popup_wiki_help : System.Web.UI.Page
	{
	Toolbox _tools;
	NeMember current_user;
	NameValueCollection _q;
	int page_id = 0;

	protected void Page_Init(object sender, EventArgs e)
		{
		_tools			= new Toolbox();
		current_user	= Toolbox.do_handle_authentication(1);
		_q				= Request.QueryString;
		}

	protected void Page_Load(object sender, EventArgs e)
		{
			htmlEditor.Settings.AllowDesignView = current_user.AuthenticatedForPrivilege(173);
		int.TryParse(_q["page_id"], out page_id);
		if (!IsPostBack)
		{
			if (page_id == 0)
			{
				Toolbox.FriendlyException(Response, "The page ID wasn't supplied", "window.close()");
			}
			else
			{
				htmlEditor.Html = _tools.getSQL_string(@"Select ifnull((Select wiki from page_wiki  where page_id =@v0 limit 1),'') ", new object[] { page_id });
			}
			
		}
		
		}
	protected void htmlEditor_CustomDataCallback(object sender, CustomDataCallbackEventArgs e)
	{
		switch (e.Parameter)
		{
			case "save":
				SetHtml();
				break;
			case "load":
				e.Result = GetHtml();
				break;
		}
	}
	private string GetHtml()
	{
		var html = Session["html_wiki_help"];
		if (html == null)
			return "There isn`t saved document";
		else
			return html.ToString();
	}

	private void SetHtml()
	{
		Session["html_wiki_help"] = htmlEditor.Html;
		if (_tools.getSQL_int(@"Select count(id) from page_wiki  where page_id =@v0", new object[] { page_id }) > 0)
		{
			_tools.getSQL_void(@"update page_wiki 
set wiki=@v0,
last_member_id=@v1
where page_id = @v2" ,
				new object[]
				{
					Session["html_wiki_help"],
					current_user.id,
					page_id
				});
		}
		else
		{
			_tools.getSQL_void(@"insert into page_wiki (wiki,last_member_id,last_modified,page_id) 
values(@v0,@v1,curdate(),@v2)",new object[] {  Session["html_wiki_help"], current_user.id , page_id });
		}

	}
}