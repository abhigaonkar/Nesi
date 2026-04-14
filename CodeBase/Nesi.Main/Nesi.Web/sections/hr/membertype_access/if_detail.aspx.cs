using System;
using System.Data;
using System.Collections.Specialized;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web;
using System.Web.Services;
using nesi.core;

public partial class sections_hr_membertype_access_if_detail : System.Web.UI.Page
{
	protected Toolbox _tools;
	protected NeMember current_user;
	protected NeMemberType _mt;
	protected NameValueCollection _q;
	bool is_priv;
	int page_id;
	int privilege_id;
	string this_key = "";
	int mt_id = 0;
	bool include_all = false;
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		_q = Request.QueryString;
		if (_q.Count == 0)
		{
		    current_user = Toolbox.do_handle_authentication(173);
        }
		else
		{
			this_key = _q["key"];
			include_all = _q["include_all"] == "true";
			if (Session["session"] != null)
			{
			    current_user = Toolbox.do_handle_authentication(173);
                if (current_user.AuthenticatedForPrivilege(99) != true)
				{
					Response.Redirect("~/Default.aspx", false);
					return;
				}
			}
			else
			{
				Response.Redirect("~/Default.aspx", false);
				return;
			}

		}

		//	lbldescription.Text			= "Pages & Privileges for : " + _mt.name;
		is_priv = this_key.Substring(0, 1) == "P";
		page_id = is_priv ? 0 : Convert.ToInt32(this_key);
		privilege_id = is_priv ? Convert.ToInt32(this_key.Remove(0, 1)) : 0;
		if (is_priv)
		{
			lbltitle.Text = "(" + privilege_id + ") Privilege: " + Toolbox.doSQL_string(@"Select privilege_name from privilege where privilege_id =@v0", new object[] { privilege_id });
			lbldescription.Text = Toolbox.doSQL_string("Select privilege_desc from privilege where privilege_id =@v0", new object[] { privilege_id });
			chkEnabled.Value = Toolbox.doSQL_int(@"Select privilege_enabled from privilege where privilege_id =@v0", new object[] { privilege_id });
		}
		else
		{
			lbltitle.Text = "(" + page_id + ") Page: " + Toolbox.doSQL_string(@"Select page_name from page where page_id =@v0", new object[] { page_id });
			lbldescription.Text = Toolbox.doSQL_string("Select page_desc from page where page_id =@v0", new object[] { page_id });
			chkEnabled.Value = Toolbox.doSQL_int("Select page_enabled from page where page_id =@v0", new object[] { page_id });
		}


		tree_priv_list.ClearNodes(); // clear the priv list
		CreatePopupNodes();          // create membertype list
		CreatePopupNodes2();         // create who has priv list
		tree_priv_list.ExpandAll();  // expand everything
		tree_memlist.ExpandAll();  // exapnd everything
	}

	private void CreatePopupNodes()
	{

		var dt = Toolbox.doSQL_dt(@"Select * from membertype order by active desc, membertype_name"  , null);
		foreach (DataRow dr in dt.Rows)
		{
			var w = (Convert.ToBoolean(dr["active"]) == true ? "Active" : "Not Active");
			var pages = CreatepopNodeCore(dr["membertype_id"].ToString(), "<font color='Black'>" + dr["membertype_name"] + " (" + w + @") </font>");
		}
	}
	private TreeListNode CreatepopNodeCore(object key, string text)
	{
		var node = tree_memlist.AppendNode(key);
		node.AllowSelect = false;
		node.AllowSelect = true;
		node["MemberType_Name"] = text;
		return node;
	}
	private void CreatePopupNodes2()
	{
		DataTable dt;


		if (is_priv)
		{
			#region query
			dt = Toolbox.doSQL_dt(@" SELECT MEMBERtype_id _mtid, MEMBERtype_NAME, a.membertypepageprivilege_privilege_id id FROM membertypepageprivilege a INNER JOIN membertype b ON b.membertype_id = a.membertypepageprivilege_type_id  WHERE b.active = 1 and a.membertypepageprivilege_privilege_id =@v0 order by b.active desc, b.membertype_name ", new object[] { privilege_id });
			#endregion query
		}
		else
		{
			#region query
			dt = Toolbox.doSQL_dt(@" SELECT MEMBERtype_id _mtid, MEMBERtype_NAME, a.membertypepage_page_id id FROM membertypepage a INNER JOIN membertype b ON b.membertype_id = a.membertypepage_type_id  WHERE b.active = 1 AND a.membertypepage_page_id =@v0 order by active desc, membertype_name ", new object[] { page_id });
			#endregion query					
		}


		foreach (DataRow dr in dt.Rows)
		{
			var id = dr["_mtid"];
			var key = "MT" + id;
			var name = dr["MEMBERtype_NAME"].ToString();


			var pages = CreatepopNodeCore2(key, "<font color='DarkBlue'>" + name + "</font>");
		}



	}
	private TreeListNode CreatepopNodeCore2(object key, string text)
	{

		var node = tree_priv_list.AppendNode(key);
		node.AllowSelect = true;
		node["MemberType_Name"] = text;
		return node;
	}
	protected void btnAdd_Click(object sender, EventArgs e)
	{
		foreach (var tn in tree_memlist.GetSelectedNodes())
		{
			var mt_id = Convert.ToInt32(tn.Key);  // gather all select membertype ids
			NeMemberType mt;
			mt = new NeMemberType(mt_id);
			var priv_id = 0;
			var is_priv = this_key.Substring(0, 1) == "P";
			if (is_priv)
			{
				#region Privilege
				priv_id = Convert.ToInt32(this_key.Remove(0, 1));
				var p = new NePrivilege(priv_id);
				p.admin = current_user;
				var user_priv = new NEMTPrivilege();
				user_priv.admin = current_user;
				user_priv.mt = mt;
				var user_page = new NeMTPage();
				user_page.admin = current_user;
				user_page.mt = mt;
				page_id = p.page_id;
				var exists = NEMTPrivilege.exists(priv_id, mt_id);
				if (!exists)  // if it doesn't exist , check to see if the page exists
				{
					exists = NeMTPage.exists(mt_id, page_id);
					if (!exists)
					{
						user_page.mt_id = mt_id;
						user_page.page_id = page_id;
						user_page.save();
					}
					else
					{
						user_page.id = NeMTPage.get_id(mt_id, page_id);
					}
					user_priv.mt_id = mt_id;
					user_priv.typepage_id = user_page.id;
					user_priv.privilege_id = priv_id;
					user_priv.save();
				}
				#endregion Privilege
			}
			else
			{
				#region Page
				page_id = Convert.ToInt32(this_key);
				if (!NeMTPage.exists(mt_id, page_id))
				{
					var NE_up = new NeMTPage();
					NE_up.admin = current_user;
					NE_up.mt = mt;
					NE_up.page_id = page_id;
					NE_up.mt_id = mt_id;
					NE_up.save();
				}
				#endregion Page

			}

		}

		tree_priv_list.ClearNodes();
		CreatePopupNodes2();
		tree_priv_list.ExpandAll();
		tree_memlist.UnselectAll();
		return;
	}
	protected void btn_delete_Click(object sender, EventArgs e)
	{
		foreach (var tn in tree_priv_list.GetSelectedNodes())
		{
			var user_id = Convert.ToInt32(tn.Key.Remove(0, 2));
			var _user_id = (int)user_id;
			var mt = new NeMemberType(_user_id);
			var priv_id = 0;
			var page_id = 0;
			var is_priv = this_key.Substring(0, 1) == "P";
			if (is_priv)
			{
				priv_id = Convert.ToInt32(this_key.Remove(0, 1));
				var p = new NePrivilege(priv_id);
				page_id = p.page_id;
				var NE_up = new NEMTPrivilege(priv_id, user_id);
				NE_up.admin = current_user;
				NE_up.mt = mt;
				NE_up.delete();
			}
			else
			{
				page_id = Convert.ToInt32(this_key);
				var NE_up = new NeMTPage(user_id, page_id);
				NE_up.admin = current_user;
				NE_up.mt = mt;
				NE_up.delete();
			}
		}

		tree_priv_list.ClearNodes();
		CreatePopupNodes2();
		tree_priv_list.ExpandAll();
		tree_priv_list.UnselectAll();

	}
	protected void btnApply_Click(object sender, EventArgs e)
	{
		string priv_id;
		string page_id;



		if (this_key.Substring(0, 1) == "P")
		{
			priv_id = this_key.Remove(0, 1);
			Toolbox.doSQL_void("Update privilege set privilege_enabled =@v0  where privilege_id =@v1", new object[] { Convert.ToInt16(chkEnabled.Value), priv_id });
			btnApply.ClientEnabled = false;
			chkEnabled.Value = _tools.getSQL_int(@"Select privilege_enabled from privilege  where privilege_id =@v0", new object[] { priv_id });


		}
		else
		{
			page_id = this_key;
			Toolbox.doSQL_void("Update page set page_enabled =@v0   where page_id =@v1 ",new object[] { Convert.ToInt16(chkEnabled.Value), page_id });
			btnApply.ClientEnabled = false;
			chkEnabled.Value = _tools.getSQL_int(@"Select page_enabled from page  where page_id =@v0", new object[] { page_id });


		}
	}
	protected void tree_CustomCallback(object sender, TreeListCustomCallbackEventArgs e)
	{
		var tl = (ASPxTreeList)sender;
		var state = e.Argument;
		switch (state)
		{
			case "true":
				tl.SelectAll();
				break;
			case "false":
				tl.UnselectAll();
				break;
		}
	}
	protected void tree_SelectionChanged(object sender, EventArgs e)
	{
		var tl = (ASPxTreeList)sender;
		foreach (var tn in tl.GetSelectedNodes())
		{
			tn.Expanded = tn.HasChildren;
		}
	}
	[WebMethod]
	public static void set_activekey(string key)
	{

	}
	protected void tree_CustomCallback1(object sender, TreeListCustomCallbackEventArgs e)
	{
		this_key = e.Argument;

		if (this_key.Substring(0, 1) == "P")
		{
			lbldescription.Text = _tools.getSQL_string(@"Select privilege_desc from privilege  where privilege_id =@v0", new object[] { this_key.Remove(0,1) });
			chkEnabled.Value = _tools.getSQL_int(@"Select privilege_enabled from privilege  where privilege_id =@v0", new object[] { this_key.Remove(0,1) });

		}
		else
		{
			lbldescription.Text = _tools.getSQL_string(@"Select page_desc from page  where page_id =@v0", new object[] { this_key });
			chkEnabled.Value = _tools.getSQL_int(@"Select page_enabled from page  where page_id =@v0", new object[] { this_key });
		}

		tree_priv_list.ClearNodes();
		CreatePopupNodes2();
		tree_priv_list.ExpandAll();
		tree_memlist.ExpandAll();

	}
	protected void popup1_WindowCallback(object source, PopupWindowCallbackArgs e)
	{
		var o = e.Parameter.Split('|');
		this_key = o[0];

		if (this_key.Substring(0, 1) == "P")
		{
			lbldescription.Text = _tools.getSQL_string(@"Select privilege_desc from privilege  where privilege_id =@v0", new object[] { this_key.Remove(0,1) });
			chkEnabled.Value = _tools.getSQL_int(@"Select privilege_enabled from privilege  where privilege_id =@v0", new object[] { this_key.Remove(0,1) });

		}
		else
		{
			lbldescription.Text = _tools.getSQL_string(@"Select page_desc from page  where page_id =@v0", new object[] { this_key });
			chkEnabled.Value = _tools.getSQL_int(@"Select page_enabled from page  where page_id =@v0", new object[] { this_key });
		}

		tree_priv_list.ClearNodes();
		CreatePopupNodes2();
		tree_priv_list.ExpandAll();
		tree_memlist.ExpandAll();
	}
}