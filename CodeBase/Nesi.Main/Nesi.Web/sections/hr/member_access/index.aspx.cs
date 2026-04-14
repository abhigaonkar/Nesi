using System;
using System.Data;
using System.Collections.Specialized;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web;
using System.Web.Services;
using nesi.core;

public partial class sections_hr_member_access_index : System.Web.UI.Page
{
	protected Toolbox _tools;
	protected NeMember current_user;
	protected NameValueCollection _q;
	protected string _listtype = "contact";
	protected int _business_unit_id = 0;
	protected int _user_id = 0;
	protected string w = "contact";
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		_q = Request.QueryString;
		if (_q["type"] != null)
		{
		    current_user = Toolbox.do_handle_authentication(96);
		}
		int.TryParse(_q["business_unit_id"], out _business_unit_id);
		int.TryParse(_q["user_id"], out _user_id);
		CreateNodes();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		lb_error.Text = "";
		lb_notice.Text = "";
		if (!IsPostBack)
		{
			_listtype = "member";

			ddl_type.Value = _listtype == "member" ? 1 : 2;
			if (_business_unit_id != 0)
			{
				ddl_company.Value = _business_unit_id == 0 ? 0 : _business_unit_id;
			}
			if (_user_id != 0)
			{
				ddl_user.Value = _user_id == 0 ? 0 : _user_id;
			}
		}
		else
		{
			_listtype = "member";
			_business_unit_id = ddl_company.Value == null ? 0 : (int)ddl_company.Value;
			_user_id = ddl_user.Value == null ? 0 : (int)ddl_user.Value;
		}
		populate_companies();
		populate_users();
		CreateNodes();
		if (ddl_user.Value != null && !IsPostBack)
		{
			populate_pagepriv();
		}
	}
	private void populate_companies()
	{
		if (ddl_type.Value != null)
		{
			switch ((int)ddl_type.Value)
			{
				case 1:
					ddl_company.DataSource = _tools.getSQL_datatable(@"Select id,ddl_name from business_unit  where find_in_set(id,@v0) and active = 'T'", new object[] { new Current_User().visible_business_units });
					ddl_company.TextField = "ddl_name";
					ddl_company.ValueField = "id";
					ddl_company.DataBind();
					break;
			}
		}
	}
	private void populate_users()
	{
		if (ddl_type.Value != null && ddl_company.Value != null)
		{
			switch ((int)ddl_type.Value)
			{
				case 1:
					ddl_user.DataSource = NeMember.GetEmployees((int)ddl_company.Value);
					ddl_user.TextField = "name";
					ddl_user.ValueField = "id";
					ddl_user.DataBind();
					break;
			}
		}
	}
	private void CreateNodes()
	{
		if (ddl_type.Value != null)
		{
			var _type_id = (int)ddl_type.Value;
			var page_cust_enabled = _type_id == 2 ? " AND page_custenabled = TRUE" : "";
			var priv_cust_enabled = _type_id == 2 ? " AND privilege_custenabled = TRUE" : "";
			var _type = _type_id == 1 ? "member" : "contact";
			var dt = Toolbox.doSQL_dt(string.Format("SELECT * FROM page WHERE page_parent_id = 0 {0} ORDER BY page_name", page_cust_enabled), null);
			foreach (DataRow dr in dt.Rows)
			{
				var id = dr["page_id"];
				var name = "(" + id + ")  " + dr["page_name"];
				var desc = dr["page_desc"];
				var pages = CreateNodeCore(id, "<a class='page' href='javascript:void(0)' data-key='" + id + "' data-id='" + _business_unit_id + "' data-type='" + _type + "' onclick='item_click(this, event)'>" + name + "</a>", desc, null);
			}
			dt = Toolbox.doSQL_dt(string.Format("SELECT * FROM page WHERE page_parent_id != 0 {0} ORDER BY page_name", page_cust_enabled), null);
			foreach (DataRow dr in dt.Rows)
			{
				var id = dr["page_id"];
				var name = "(" + id + ")  " + dr["page_name"];
				var desc = dr["page_desc"];
				var parent_id = dr["page_parent_id"];
				var pages = CreateNodeCore(id, "<a class='page' href='javascript:void(0)' data-key='" + id + "' data-id='" + _business_unit_id + "' data-type='" + _type + "' onclick='item_click(this, event)'>" + name + "</a>", desc, tree.FindNodeByKeyValue(parent_id.ToString()));
			}
			dt = Toolbox.doSQL_dt(string.Format("SELECT * FROM privilege WHERE privilege_page_id != 0 {0} ORDER BY privilege_name", priv_cust_enabled), null);
			foreach (DataRow dr in dt.Rows)
			{
				var id = dr["privilege_id"];
				var page_id = dr["privilege_page_id"];
				var desc = dr["privilege_desc"];
				var name = "(" + id + ")  " + dr["privilege_name"];
				var key = "P" + id;
				var pages = CreateNodeCore(key, "<a class='priv' href='javascript:void(0)' data-key='" + key + "' data-id='" + _business_unit_id + "' data-type='" + _type + "' onclick='item_click(this, event)'>" + name + "</a>", desc, tree.FindNodeByKeyValue(page_id.ToString()));
			}
			tree.DataBind();
		}
	}
	private void populate_pagepriv()
	{
		clear_tree();
		var prefix = (int)ddl_type.Value == 1 ? "member" : "contact";
		var page_dt = Toolbox.doSQL_dt(string.Format(@"SELECT {0}page_id id, {0}page_page_id page_id FROM {0}page WHERE {0}page_{0}_id = @v0", prefix), new object[] { ddl_user.Value });
		var priv_dt = Toolbox.doSQL_dt(string.Format(@"SELECT {0}pageprivilege_id id, {0}pageprivilege_privilege_id privilege_id FROM {0}pageprivilege WHERE {0}pageprivilege_{0}_id = @v0", prefix), new object[] { ddl_user.Value });
		foreach (DataRow dr in page_dt.Rows)
		{
			var id = dr["id"];
			var page_id = dr["page_id"];
			var key = page_id.ToString();
			var tln = tree.FindNodeByKeyValue(key);
			if (tln != null)
			{
				tln.Selected = true;
				if (tln.HasChildren)
				{
					tln.Expanded = true;
				}

			}
		}
		foreach (DataRow dr in priv_dt.Rows)
		{
			var id = dr["id"];
			var privilege_id = dr["privilege_id"];
			var key = "P" + privilege_id;
			var tln = tree.FindNodeByKeyValue(key);
			if (tln != null)
			{
				if (tln.HasChildren)
				{
					tln.Expanded = true;
				}
				tln.Selected = true;
			}
		}
		bt_savetouser.Visible = true;
	}
	private TreeListNode CreateNodeCore(object key, string page, object text, TreeListNode parentNode)
	{
		if (tree.FindNodeByKeyValue(key.ToString()) == null)
		{
			var node = tree.AppendNode(key, parentNode);
			node["Page"] = page;
			node["Description"] = text.ToString();
			return node;
		}
		else
		{
			return tree.FindNodeByKeyValue(key.ToString());
		}
	}
	protected void tree_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
	{
		var key = e.Argument;
		var node = tree.FindNodeByKeyValue(key);
		var pop = (ASPxPopupControl)popup1;
		popup1.ShowOnPageLoad = true;
		popup1.HeaderText = node.GetValue("Page").ToString();
		popup1.ShowHeader = true;
	}
	protected void tree_FocusedNodeChanged(object sender, EventArgs e)
	{
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
			var x = tn.Level;
			var tnparent = tn;
			while (x > 1)
			{
				tnparent = tnparent.ParentNode;
				tnparent.Selected = true;
				x--;
			}
			tn.Expanded = tn.HasChildren && tn.Selected;
		}
		foreach (var tn in tl.GetAllNodes())
		{
			if (tn.Selected == false)
			{
				foreach (TreeListNode tn1 in tn.ChildNodes)
				{
					foreach (TreeListNode tn2 in tn1.ChildNodes)
					{
						foreach (TreeListNode tn3 in tn2.ChildNodes)
						{
							foreach (TreeListNode tn4 in tn3.ChildNodes)
							{
								foreach (TreeListNode tn5 in tn4.ChildNodes)
								{
									foreach (TreeListNode tn6 in tn5.ChildNodes)
									{
										tn6.Selected = false;
									}
									tn5.Selected = false;
								}
								tn4.Selected = false;
							}
							tn3.Selected = false;
						}
						tn2.Selected = false;
					}
					tn1.Selected = false;
				}

			}
		}

	}
	[WebMethod]
	public static void set_activekey(string key)
	{

	}
	protected void popup1_WindowCallback(object source, PopupWindowCallbackArgs e)
	{
		var o = e.Parameter.Split('|');

		popup1.HeaderText = "Page/Privilege: " + o[1];
		popup1.ShowHeader = true;
	}
	protected void bt_savetemplate_Click(object sender, EventArgs e)
	{
		var type = _listtype == "member" ? 1 : 2;
		var in_error = false;
		var notice = "";
		var error = "";
		var UP_temp = new NEUserPrivilegeTemplate();
		var name = pagepriv_templates.Text.Trim();
		var template_id = 0;
		if (pagepriv_templates.Value != null)
		{
			int.TryParse(pagepriv_templates.Value.ToString(), out template_id);
		}
		if (pagepriv_templates.Value == null)
		{
			error = "Please select a template";
			in_error = true;
		}
		else if (template_id == 0)
		{
			error = "Can't create new template here.";
			in_error = true;
		}
		else if (tree.GetSelectedNodes().Count == 0)
		{
			error = "Please select some pages / privileges";
			in_error = true;
		}
		else if (name == "")
		{
			error = "Please supply a name for this template";
			in_error = true;
		}
		else if (UP_temp.exists(name, type) && new NEUserPrivilegeTemplate(name, type).id != template_id)
		{
			error = "A template by this name already exists, please supply a different name.";
			in_error = true;
		}
		lb_error.Text = error;
		if (!in_error)
		{
			UP_temp = new NEUserPrivilegeTemplate(template_id);
			UP_temp.name = name;
			UP_temp.type = type;
			UP_temp.save();
			UP_temp.clear();
			foreach (var tn in tree.GetSelectedNodes())
			{
				var this_key = tn.Key;
				var is_priv = this_key.Substring(0, 1) == "P";
				var item_id = is_priv ? Convert.ToInt32(this_key.Remove(0, 1)) : Convert.ToInt32(this_key);
				var i = new NEUserPrivilegeTemplate_item();
				i.template_id = UP_temp.id;
				i.type = is_priv ? "Privilege" : "Page";
				i.table_id = item_id;
				i.save();
				notice = "Saved Template.";
				lb_notice.Text = notice;
			}
			clear_selection();
		}
	}
	private void clear_selection()
	{
		lb_error.Text = "";
		pagepriv_templates.SelectedIndex = -1;
		tb_newtemplate.Text = "";
		if (ddl_user.Value == null)
		{
			clear_tree();
		}
	}
	private void clear_tree()
	{
		foreach (var tn in tree.GetAllNodes())
		{
			tn.Selected = false;
			tn.Expanded = false;
		}
	}
	protected void bt_resettemplate_Click(object sender, EventArgs e)
	{
		clear_selection();
	}
	protected void bt_deletetemplate_Click(object sender, EventArgs e)
	{
		var error = "";
		var type = _listtype == "member" ? 1 : 2;
		var UP_temp = new NEUserPrivilegeTemplate();
		var in_error = false;
		if (pagepriv_templates.Value == null)
		{
			error = "Please select a template";
			in_error = true;
		}
		lb_error.Text = error;
		if (!in_error)
		{
			UP_temp = new NEUserPrivilegeTemplate((int)pagepriv_templates.Value);
			UP_temp.delete();
		}
		clear_selection();
		pagepriv_templates.DataBind();
	}
	protected void bt_savenewtemplate_Click(object sender, EventArgs e)
	{
		var type = _listtype == "member" ? 1 : 2;
		var UP_temp = new NEUserPrivilegeTemplate();
		var in_error = false;
		var error = "";
		var notice = "";
		var name = tb_newtemplate.Text.Trim();
		if (name == "")
		{
			error = "Please supply a name for this template";
			in_error = true;
		}
		else if (UP_temp.exists(name, type))
		{
			error = "A template by this name already exists, please supply a different name.";
			in_error = true;
		}
		else if (tree.GetSelectedNodes().Count == 0)
		{
			error = "Please select some pages / privileges";
			in_error = true;
		}
		lb_error.Text = error;
		if (!in_error)
		{
			UP_temp.name = name;
			UP_temp.type = type;
			UP_temp.added_by_member_id = current_user.id;
			UP_temp.save();
			foreach (var tn in tree.GetSelectedNodes())
			{
				var this_key = tn.Key;
				var is_priv = this_key.Substring(0, 1) == "P";
				var id = is_priv ? Convert.ToInt32(this_key.Remove(0, 1)) : Convert.ToInt32(this_key);
				var i = new NEUserPrivilegeTemplate_item();
				i.template_id = UP_temp.id;
				i.type = is_priv ? "Privilege" : "Page";
				i.table_id = id;
				i.save();
				notice = "Saved Template.";
				lb_notice.Text = notice;
			}
			clear_selection();
			CreateNodes();
			pagepriv_templates.DataBind();
		}
	}
	protected void pagepriv_templates_SelectedIndexChanged(object sender, EventArgs e)
	{
		var combo = (ASPxComboBox)sender;
		clear_tree();
		try
		{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM pageprivilege_template_item WHERE template_id = @v0 ", new object[] { combo.Value });
			foreach (DataRow dr in dt.Rows)
			{
				var id = dr["table_id"];
				var type = dr["type"].ToString();
				var key = type == "Privilege" ? "P" + id : id.ToString();
				var tln = tree.FindNodeByKeyValue(key);
				if(tln == null) continue;
				if (tln.HasChildren)
					{
					tln.Expanded = true;
					}
				tln.Selected = true;
			}
		}
		catch (Exception ee)
		{
			combo.SelectedIndex = -1;
			foreach (var tn in tree.GetSelectedNodes())
			{
				tn.Selected = false;
				tn.Expanded = false;
			}
			lb_error.Text = "An error occurred loading this template";
			_tools.catch_error(ee);
		}
	}
	protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
	{
		ddl_company.SelectedIndex = -1;
		ddl_user.SelectedIndex = -1;
		clear_selection();
		tree.ClearNodes();
		CreateNodes();
	}
	protected void ddl_company_SelectedIndexChanged(object sender, EventArgs e)
	{
		ddl_user.Value = null;
		bt_savetouser.Visible = false;
		clear_selection();
		tree.ClearNodes();
		CreateNodes();
	}
	protected void ddl_user_SelectedIndexChanged(object sender, EventArgs e)
	{
		clear_tree();
		clear_selection();
		tree.ClearNodes();
		CreateNodes();
		populate_pagepriv();
	}
	protected void bt_savetouser_Click(object sender, EventArgs e)
	{
		var in_error = false;
		var error = "";
		var notice = "";
		if (tree.GetSelectedNodes().Count == 0)
		{
			error = "Please select some pages / privileges";
			in_error = true;
		}
		lb_error.Text = error;
		if (!in_error)
		{
			var user_id = (int)ddl_user.Value;
			var type_id = (int)ddl_type.Value;
			var user_priv = new NEUserPrivilege();
			var user_page = new NEUserPage();
			NeMember user;
			if (type_id == 2)
			{
				var co = new NEContact(user_id);
				if (!Toolbox.CheckEmail(co.Contact_Email))
				{
					throw new Exception(string.Format("The contact ({0}) does not have a valid email address, stopping privilege process", co.Contact_Name));
				}
				user = new NeMember(co.nesi_member_id);
			}
			else
			{
				user = new NeMember((int)user_id);
			}
			NEUserPrivilege.clear(current_user, user, type_id);
			NEUserPage.clear(current_user, user, type_id);

            foreach (var tn in tree.GetSelectedNodes())
			{
				var priv_id = 0;
				var page_id = 0;
				var is_priv = tn.Key.Substring(0, 1) == "P";
				if (is_priv)
				{
					#region Privilege
					priv_id = Convert.ToInt32(tn.Key.Remove(0, 1));
					var p = new NePrivilege(priv_id);
					p.admin = current_user;
					p.user = user;
					user_priv = new NEUserPrivilege();
					user_priv.admin = current_user;
					user_priv.user = user;
					user_page = new NEUserPage();
					user_page.admin = current_user;
					user_page.user = user;
					page_id = p.page_id;
					var exists = NEUserPrivilege.exists(priv_id, (int)user_id, type_id);
					if (!exists)  // if it doesn't exist , check to see if the page exists
					{
						exists = NEUserPage.exists((int)user_id, page_id, type_id);
						if (!exists)
						{
							user_page.user_id = (int)user_id;
							user_page.page_id = page_id;
							user_page.type_id = type_id;
							user_page.save();
						}
						else
						{
							user_page.id = NEUserPage.get_id((int)user_id, page_id, type_id);
						}
						user_priv.user_id = user_id;
						user_priv.typepage_id = Convert.ToInt32(user_page.id);
						user_priv.privilege_id = priv_id;
						user_priv.type_id = type_id;
						user_priv.save();
					}
					#endregion Privilege
				}
				else
				{
					#region Page
					page_id = Convert.ToInt32(tn.Key);
					if (!NEUserPage.exists((int)user_id, page_id, type_id))
					{
						var NE_up = new NEUserPage();
						NE_up.admin = current_user;
						NE_up.user = user;
						NE_up.page_id = page_id;
						NE_up.user_id = (int)user_id;
						NE_up.type_id = type_id;
						NE_up.save();
					}
					#endregion Page
				}
				notice = "Saved User Privileges/Pages.";
				lb_notice.Text = notice;
			}
        }
        populate_pagepriv();
	}
}