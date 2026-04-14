using System;
using System.Collections.Specialized;
using nesi.core;

public partial class sections_member_inventory_inv_notes : System.Web.UI.Page
	{
	int business_unit_id			= 0;
	int master_id			= 0;
	NameValueCollection _q	= new NameValueCollection();
	NeMember current_user	= new NeMember();
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(1);
		_q					= Request.QueryString;
		int.TryParse(_q["id"], out master_id);
		int.TryParse(Session["working_business_unit_id"].ToString(), out business_unit_id);
		}
	protected void cb_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		Toolbox.doSQL_void(@"INSERT INTO inventory_note 
(dt, master_id, business_unit_id, member_id, note) 
VALUES (NOW(), @v0, @v1, @v2, @v3)", new object[] { master_id, business_unit_id, current_user.id, e.Parameter});
		}
	}