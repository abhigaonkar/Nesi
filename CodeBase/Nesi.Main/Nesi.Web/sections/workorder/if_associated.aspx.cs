using System;
using System.Collections.Specialized;
using System.Web.UI;
using DevExpress.Web;
using nesi.core;

public partial class sections_workorder_if_associated : Page
	{
	private NameValueCollection _q;

	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		_q = Request.QueryString;
		Toolbox.do_handle_authentication(1);
		}

	protected void cb_link_Callback(object source, CallbackEventArgs e)
		{
		if (!string.IsNullOrEmpty(e.Parameter))
			{
			var link_woprog_id = Convert.ToInt32(e.Parameter);
			var this_woprog_id = Convert.ToInt32(_q["woprog_id"]);
			var wo_assoc = new wo_associated();
			wo_assoc.woprog_id_a = this_woprog_id;
			wo_assoc.woprog_id_b = link_woprog_id;
			try
				{
				wo_assoc.save();
				e.Result = "success";
				}
			catch (Exception ee)
				{
				e.Result = ee.ToString();
				}
			}
		}
	protected void combo_availablewos_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var combo			= (ASPxComboBox) sender;
		combo.DataBind();
		}
	protected void gv_linkage_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
		{
		var gv		= (ASPxGridView) sender;
		if(e.ButtonID == "Delete0")
			{
			var woprog_id_a 		= Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, new string[] { "woprog_id" }).ToString());
			var woprog_id_b 		= Convert.ToInt32(_q["woprog_id"]);
			wo_associated.delete(woprog_id_a, woprog_id_b);
			gv.DataBind();
			}
		}

	protected void gv_linkage_CommandButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (Convert.ToBoolean(gv_linkage.GetRowValues(e.VisibleIndex, "show_delete")) == false)
			{
				e.Visible = DevExpress.Utils.DefaultBoolean.False;
			}
			else
			{
				e.Visible = DevExpress.Utils.DefaultBoolean.True;
			}
		}
	}
}