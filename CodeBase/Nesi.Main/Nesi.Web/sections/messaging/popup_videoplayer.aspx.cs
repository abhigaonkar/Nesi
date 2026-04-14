using System;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_messaging_popup_videoplayer : System.Web.UI.Page
	{
	Toolbox _tools;
	NeMember current_user;
	NameValueCollection _q;

	protected void Page_Init(object sender, EventArgs e)
		{
		_tools			= new Toolbox();
		current_user	= Toolbox.do_handle_authentication(1);
		_q				= Request.QueryString;
		}

	protected void Page_Load(object sender, EventArgs e)
		{
		var page_id		= 0;
		int.TryParse(_q["page_id"], out page_id);
		if(page_id == 0)
			{
			Toolbox.FriendlyException(Response, "The page wasn't supplied", "window.close()");
			}
		ddl_videos.DataSource			= Toolbox.doSQL_dt(@"SELECT b.file_id id,b.name FROM video_page_lnk a LEFT JOIN video b ON a.category_id = b.category_id WHERE a.page_id = @v0  AND b.category_id IN (SELECT category_id FROM video_privilege WHERE membertype_id = @v1 )", new object[] {  page_id, current_user.MemberTypeID } );		
		ddl_videos.ValueField			= "id";
		ddl_videos.TextField			= "name";
		ddl_videos.DataBind();
		var li					= new ListEditItem();
		li.Value						= 0;
		li.Text							= "Select Video";
		li.Selected						= true;
		ddl_videos.Items.Insert(0, li);
		}
	}