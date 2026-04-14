using System;
using DevExpress.Web;
using nesi.core;

public partial class sections_messaging_modules_category_admin : System.Web.UI.UserControl
	{
	protected void Page_Load(object sender, EventArgs e)
		{

		}
	protected void bt_savenewcategory_Click(object sender, EventArgs e)
		{
		if(string.IsNullOrEmpty(tb_categoryname.Text))
			{
			error.InnerText		= "Please provide a name for this category";
			tb_categoryname.Focus();
			}
		else
			{
			var v				= new NEVideo();
			var cat	= new NEVideo.Category();
			cat.name				= tb_categoryname.Text.Trim();
			try
				{
				cat.save();
				gv_categories.DataBind();
				tb_categoryname.Text	= "";
				error.InnerText			= "";
				tb_categoryname.Focus();
				}
			catch (Exception ee)
				{
				error.InnerText			= ee.Message;
				gv_categories.DataBind();
				tb_categoryname.Focus();
				}
			}
		}
	protected void gv_categories_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var id					= (int) e.Keys["id"];
		var c		= new NEVideo.Category(id);
		c.delete();
		gv_categories.DataBind();
		e.Cancel				= true;
		}
	protected void gv_categories_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var id					= (int) e.Keys["id"];
		var c		= new NEVideo.Category(id);
		c.name					= e.NewValues["name"].ToString();
		c.save();
		gv_categories.DataBind();
		gv_categories.CancelEdit();
		e.Cancel				= true;
		}
	private void populate_available_mt(ASPxListBox lb, int selected_id)
		{
		lb.DataBind();
		}
	private void populate_selected_mt(ASPxListBox lb, int selected_id)
		{
		lb.DataBind();
		}
	protected void gv_categories_DetailRowExpandedChanged(object sender, DevExpress.Web.ASPxGridViewDetailRowEventArgs e)
		{
		var gv			= (ASPxGridView) sender;
		var id					= (int) gv.GetRowValues(e.VisibleIndex, "id");
		if(e.Expanded)
			{
			var lb_at				= (ASPxListBox) gv.FindDetailRowTemplateControl(e.VisibleIndex, "available_types");
			var lb_st				= (ASPxListBox) gv.FindDetailRowTemplateControl(e.VisibleIndex, "selected_types");
			var lb_ap				= (ASPxListBox) gv.FindDetailRowTemplateControl(e.VisibleIndex, "available_pages");
			var lb_sp				= (ASPxListBox) gv.FindDetailRowTemplateControl(e.VisibleIndex, "selected_pages");
			Session["vid_category_index"]	= e.VisibleIndex;
			Session["vid_category_id"]		= id;
			lb_at.DataBind();
			lb_st.DataBind();
			lb_ap.DataBind();
			lb_sp.DataBind();
			}
		else
			{
			Session["vid_category_index"]	= -1;
			Session["vid_category_id"]		= 0;
			}
		}
	protected void cb_move_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		}
	protected void available_types_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var lb			= (ASPxListBox) sender;
		var id					= Convert.ToInt32(e.Parameter);
		//(lb, id);
		}
	protected void selected_types_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var lb			= (ASPxListBox) sender;
		var id					= Convert.ToInt32(e.Parameter);
		//populate_selected_mt(lb, id);
		}
	protected void bt_add_Click(object sender, EventArgs e)
		{
		var vis_index			= Convert.ToInt32(Session["vid_category_index"]);
		var category_id			= Convert.ToInt32(Session["vid_category_id"]);
		var lb_a		= (ASPxListBox) gv_categories.FindDetailRowTemplateControl(vis_index, "available_types");
		var lb_s		= (ASPxListBox) gv_categories.FindDetailRowTemplateControl(vis_index, "selected_types");
		if (lb_a.SelectedItems.Count > 0)
			{
            moveItems(true, lb_s, lb_a, category_id, true);
			}
		}
	protected void bt_remove_Click(object sender, EventArgs e)
		{
		var vis_index			= Convert.ToInt32(Session["vid_category_index"]);
		var category_id			= Convert.ToInt32(Session["vid_category_id"]);
		var lb_a		= (ASPxListBox) gv_categories.FindDetailRowTemplateControl(vis_index, "available_types");
		var lb_s		= (ASPxListBox) gv_categories.FindDetailRowTemplateControl(vis_index, "selected_types");
		if (lb_s.SelectedItems.Count > 0)
			{
            moveItems(false, lb_a, lb_s, category_id, true);
			}
		}
	protected void bt_addpage_Click(object sender, EventArgs e)
		{
		var vis_index			= Convert.ToInt32(Session["vid_category_index"]);
		var category_id			= Convert.ToInt32(Session["vid_category_id"]);
		var lb_a		= (ASPxListBox) gv_categories.FindDetailRowTemplateControl(vis_index, "available_pages");
		var lb_s		= (ASPxListBox) gv_categories.FindDetailRowTemplateControl(vis_index, "selected_pages");
		if (lb_a.SelectedItems.Count > 0)
			{
            moveItems(true, lb_s, lb_a, category_id, false);
			}
		}
	protected void bt_removepage_Click(object sender, EventArgs e)
		{
		var vis_index			= Convert.ToInt32(Session["vid_category_index"]);
		var category_id			= Convert.ToInt32(Session["vid_category_id"]);
		var lb_a		= (ASPxListBox) gv_categories.FindDetailRowTemplateControl(vis_index, "available_pages");
		var lb_s		= (ASPxListBox) gv_categories.FindDetailRowTemplateControl(vis_index, "selected_pages");
		if (lb_s.SelectedItems.Count > 0)
			{
            moveItems(false, lb_a, lb_s, category_id, false);
			}
		}
	private void moveItems(bool add, ASPxListBox targetlistBox, ASPxListBox sourcelistBox, int category_id, bool is_priv)
		{
		if(add)
			{
			for (var i = 0; i < sourcelistBox.SelectedItems.Count; i++)
				{
				var _id			= Convert.ToInt32(sourcelistBox.SelectedItems[i].Value);
				if(is_priv)
					{
					var p			= new NEVideo.Privilege();
					p.category_id				= category_id;
					p.membertype_id				= _id;
					p.save();
					}
				else
					{
					var pl			= new NEVideo.PageLink();
					pl.category_id				= category_id;
					pl.page_id					= _id;
					pl.save();
					}
				}
			}
		else
			{
			for (var i = 0; i < sourcelistBox.SelectedItems.Count; i++)
				{
				var _id			= Convert.ToInt32(sourcelistBox.SelectedItems[i].Value);
				if(is_priv)
					{
					var p			= new NEVideo.Privilege(_id, category_id);
					p.delete();
					}
				else
					{
					var pl			= new NEVideo.PageLink(_id, category_id);
					pl.delete();
					}
				}
			}
		targetlistBox.DataBind();
		sourcelistBox.DataBind();
		targetlistBox.UnselectAll();
		sourcelistBox.UnselectAll();
		}
	protected void gv_categories_FocusedRowChanged(object sender, EventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		gv.ExpandRow(gv.FocusedRowIndex);
		}
}