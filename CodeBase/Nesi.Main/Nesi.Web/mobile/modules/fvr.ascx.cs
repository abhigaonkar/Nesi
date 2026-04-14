using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nesi.core;

public partial class mobile_modules_fvr : shared.mobile_subpage
	{
	private NeMember current_user;
	protected void Page_Load(object sender, EventArgs e)
		{
		current_user		= Toolbox.do_handle_authentication(1);
		GetFVRs();
		if(current_fvrs.InnerHtml == "")
			{
			Response.Redirect("/mobile/index.aspx");
			}
		}
	 private void GetFVRs()
		{
		var fvrs				= member_fvr_hdr.chk_member(current_user.id);
		current_fvrs.InnerHtml	= "";
		foreach (DataRow dr in fvrs.Rows)
			{
			var id = dr["id"].ToString();
			var dueBy = Convert.ToDateTime(dr["due_by"]);
			current_fvrs.InnerHtml		+= string.Format(@"<div><button type='button' class='whitetext smallbutton' onclick=""fvr.client.load({0});"">{0}</button> Due: {1}</div>", id, Toolbox.MySQL_shortdt(dueBy));
			}
		}
	}