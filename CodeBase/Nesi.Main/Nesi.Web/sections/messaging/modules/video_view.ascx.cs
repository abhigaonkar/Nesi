using System;
using DevExpress.Web;
using nesi.core;

public partial class sections_messaging_modules_video_view : System.Web.UI.UserControl
	{
	NeMember myMember;
    private const int _page_id = 153; // from Page table in DB
	protected void Page_Init(object sender, EventArgs e)
		{
        var _tools = new Toolbox();
        myMember = Toolbox.do_handle_authentication(_page_id);
		Session["membertype_id"]			= myMember.MemberTypeID.ToString();
		}
	protected void combo_category_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var cb = (ASPxComboBox)sender;
		cb.DataBind();
		}
	protected void combo_videos_SelectedIndexChanged(object sender, EventArgs e)
		{
		pane_right.InnerHtml = string.Format("<iframe src='/_tools/get_file/index.aspx?file_id={0}&iframe=true' frameborder='0' width='540' height='350' />", combo_videos.Value);
		}
	protected void combo_category_SelectedIndexChanged(object sender, EventArgs e)
		{
		var c = (ASPxComboBox)sender;
		combo_videos.DataSource = Toolbox.doSQL_dt(@"SELECT file_id, name FROM video WHERE category_id = @v0 ", new object[] {  c.Value } );
		combo_videos.DataBind();
		combo_videos.UnselectAll();

		}
	protected void combo_videos_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var c = (ASPxListBox)sender;
		c.DataBind();
		}
}