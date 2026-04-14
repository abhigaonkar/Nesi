using System;
using DevExpress.Web;
using nesi.core;

public partial class sections_vendor_rfq_modules_review : System.Web.UI.UserControl
	{
	protected void Page_Load(object sender, EventArgs e)
		{
		
		}
	protected string note_handler(object container)
		{
		var c		= container as GridViewDataItemTemplateContainer;
		var row_id								= c != null ? Convert.ToInt32(c.KeyValue) : 0;
		var li							= new rfq_lineitem(row_id);
		var button_picture					= li.vendor_note == "" ? "note_blank" :"note";
		var return_val						= string.Format(@"<img src='/images/icon/icon[{0}].gif' class='note' style='cursor:pointer' onclick='note_show(this)' data-id='{1}' data-note=""{2}"" data-tooltip=""{2}"" />", button_picture, li.id, li.vendor_note);
		return return_val;
		}
	protected void b_save_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
		{
		var bt								= (ASPxButton) sender;
		var container	= bt.NamingContainer as GridViewDataItemTemplateContainer;
		var row_id									= Convert.ToInt32(gv_review.GetRowValues(container.VisibleIndex, "id"));
		e.Properties["cpId"]						= row_id;
		}
}
