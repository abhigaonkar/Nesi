using System;
using nesi.core;

public partial class sections_messaging_modules_video_admin : System.Web.UI.UserControl
	{
	protected void Page_Load(object sender, EventArgs e)
		{

		}
	protected void gv_videos_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var id					= (int) e.Keys["id"];
		var v				= new NEVideo(id);
		v.delete();
		gv_videos.DataBind();
		e.Cancel				= true;
		}
	protected void gv_videos_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var id					= (int) e.Keys["id"];
		var v				= new NEVideo(id);
		v.description			= e.NewValues["description"].ToString();
		v.name					= e.NewValues["name"].ToString();
		v.category_id			= Convert.ToInt32(e.NewValues["category_id"]);
		v.save();
		gv_videos.DataBind();
		gv_videos.CancelEdit();
		e.Cancel				= true;
		}
	protected void gv_videos_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{
		var category_id		= (int) gv_videos.GetRowValues(e.VisibleIndex, "category_id");
		if(e.DataColumn.Name == "selected" && category_id != 42)
			{
			e.Cell.Text		= "";
			}
		}
	protected void gv_videos_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
		{
		if(e.Parameters.Contains("|"))
			{
			var paras		= e.Parameters.Split('|');
			if(paras[0] == "togglevideo")
				{
				Toolbox.doSQL_void(@"UPDATE video SET is_selected = FALSE");
				if(paras[2] == "checked")
					{
					Toolbox.doSQL_void(@"UPDATE video SET is_selected = TRUE WHERE id = @v0 LIMIT 1",paras[1]);
					}
				}
			}
		gv_videos.DataBind();
		}
}