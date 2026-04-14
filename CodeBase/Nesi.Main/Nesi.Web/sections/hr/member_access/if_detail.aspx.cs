using System;
using System.Data;
using System.Collections.Specialized;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web;
using System.Web.Services;
using nesi.core;

public partial class sections_hr_member_access_if_detail : System.Web.UI.Page
	{
	protected Toolbox _tools;
	protected NeMember current_user;
	protected NameValueCollection _q;
	protected string _listtype = "contact";
	protected int _customerid = 0;
	string this_key		= "";
	bool include_all		= false;
	protected void Page_Init(object sender, EventArgs e)
		{
		 current_user = Toolbox.do_handle_authentication(96);  // human resource page , edit employee privilegee
        _tools = new Toolbox();
		_q = Request.QueryString;
		if (_q.Count == 0)
		{
		   
			_listtype = "member";
			}
		else
			{
			this_key	= _q["key"];
			_listtype	= _q["type"];
			include_all	= _q["include_all"] == "true";
			
				if (!current_user.AuthenticatedForPage(173))
					{
					//Response.Redirect("~/Default.aspx", false);
					return;
					}
				
			_customerid = Convert.ToInt32(_q["id"]);
			
			}
		if (_listtype == "member")
			{
			var member = new NeMember(Convert.ToInt32(_customerid));
			lbldescription.Text			= "Pages & Privileges for : " + member.FullName;
			}
		else if(_listtype == "contact")
			{
			var cust = new NECustomer(Convert.ToInt32(_customerid));
			lbldescription.Text			= "Contacts for : " + cust.Customer_Name;
			chkEnabled.ClientVisible = false;
			btnApply.Visible			= false;
			}
		tree_priv_list.ClearNodes();
		CreatePopupNodes();
		CreatePopupNodes2();
		tree_priv_list.ExpandAll();
		tree_memlist.ExpandAll();
		}

	private void CreatePopupNodes()
		{
		if (_listtype == "member")
			{
			var dt = Toolbox.doSQL_dt(@"Select * from vw_active_business_units"  , null);
			foreach (DataRow dr in dt.Rows)
				{
				var pages = CreatepopNodeCore(dr["id"].ToString(), "<b><font color='Black'>" + dr["name"] + "</font></b>", "", null);
				}
			dt = Toolbox.doSQL_dt(@"Select member_id,member_fullname as member_name,business_unit_id,membertype_name from member,membertype  where member_membertype_id = membertype_id and member_status = 'Active' order by member_fullname" , null);
			foreach (DataRow dr in dt.Rows)
				{
				object key = "M" + dr["member_id"];
				var pages = CreatepopNodeCore(key.ToString(), "<font color='DarkBlue'>" + dr["member_name"] + "</font>", dr["membertype_name"].ToString(), tree_memlist.FindNodeByKeyValue(dr["business_unit_id"].ToString()));
				}
			}
		else if (_listtype == "contact")
			{
			var custwhere = include_all ? "" : "  contact_cust_id = " + _customerid+" and";
			var dt = Toolbox.doSQL_dt(@"Select * from customer  where customer_id =@v0", new object[] { _customerid });
			foreach (DataRow dr in dt.Rows)
				{
				var pages = CreatepopNodeCore(dr["customer_id"].ToString(), "<b><font color='Black'>" + _tools.value_from(dr["customer_name"].ToString()) + "</font></b>", "", null);
				}
			dt = Toolbox.doSQL_dt("Select contact_id,contact_name,contact_cust_id from contact where "+custwhere+" contact_status = 'Active' and contact_type = 'Customer' and TRIM(contact_email) != '' order by contact_name",null);
			foreach (DataRow dr in dt.Rows)
				{
				object key = "M" + dr["contact_id"];
				var pages = CreatepopNodeCore(key.ToString(), "<font color='DarkBlue'>" + dr["contact_name"] + "</font>", "", tree_memlist.FindNodeByKeyValue(dr["contact_cust_id"].ToString()));
				}

			}
		}
	private TreeListNode CreatepopNodeCore(object key, string page, string text, TreeListNode parentNode)
		{
		var node = tree_memlist.AppendNode(key, parentNode);
		node.AllowSelect = key.ToString().Substring(0, 1) == "M";
	    node["Business Unit/Name"] = page;
		node["MemberType"] = text;
		return node;
		}
	private void CreatePopupNodes2()
		{
			DataTable dt;
			var is_priv		= this_key.Substring(0, 1) == "P";
			var page_id			= is_priv ? 0 : Convert.ToInt32(this_key);
			var privilege_id	= is_priv ? Convert.ToInt32(this_key.Remove(0, 1)) : 0;		
			if (is_priv)
				{
					lbltitle.Text = "(" + privilege_id + ") Privilege: " + Toolbox.doSQL_string(@"Select privilege_name from privilege where privilege_id =@v0",new object[]{privilege_id});
				lbldescription.Text = Toolbox.doSQL_string(@"Select privilege_desc from privilege where privilege_id =@v0",new object[]{privilege_id});
				chkEnabled.Value = Toolbox.doSQL_int(@"Select privilege_enabled from privilege where privilege_id =@v0",new object[]{privilege_id});
				}
			else
				{
					lbltitle.Text = "(" + page_id + ") Page: " + Toolbox.doSQL_string("Select page_name from page where page_id =@v0",new object[]{page_id});
				lbldescription.Text = Toolbox.doSQL_string("Select page_desc from page where page_id =@v0",new object[]{page_id});
				chkEnabled.Value = Toolbox.doSQL_int("Select page_enabled from page where page_id =@v0",new object[]{page_id});
				}
	
			if (_listtype == "member")
				{
				if (is_priv)
					{
					#region query
					dt = Toolbox.doSQL_dt(@" SELECT c.id business_unit_id, c.name, b.member_fullname as membername, a.memberpageprivilege_privilege_id FROM memberpageprivilege a INNER JOIN member b ON b.member_id = a.memberpageprivilege_member_id INNER JOIN business_unit c ON b.business_unit_id = c.id  WHERE b.member_status = 'Active' and a.memberpageprivilege_privilege_id =@v0 GROUP BY business_unit_id order by name,b.member_fullname", new object[] { privilege_id });
					#endregion query
					}
				else
					{
					#region query
					dt = Toolbox.doSQL_dt(@" SELECT c.id business_unit_id, c.name, b.member_fullname as membername, a.memberpage_page_id FROM memberpage a INNER JOIN member b ON b.member_id = a.memberpage_member_id INNER JOIN business_unit c ON b.business_unit_id = c.id  WHERE b.member_status = 'Active' AND a.memberpage_page_id =@v0 GROUP BY business_unit_id order by name,b.member_fullname", new object[] { page_id });
					#endregion query					
					}
				foreach (DataRow dr in dt.Rows)
					{
					var id			= dr["business_unit_id"].ToString();
					var name			= dr["name"].ToString();
					var pages	= CreatepopNodeCore2(id, "<b><font color='Black'>" + name + "</font></b>", "", null);
					}

				// now set up the member level connecting to the company higher level
				if (is_priv)
					{
					#region query
					dt = Toolbox.doSQL_dt(@" SELECT b.member_id id, a.memberpageprivilege_privilege_id, MEMBER_NAME(b.member_id) as name, b.business_unit_id parent_id, c.membertype_name FROM memberpageprivilege a INNER JOIN member b ON b.member_id = a.memberpageprivilege_member_id INNER JOIN membertype c ON b.member_membertype_id = c.membertype_id  WHERE b.member_status = 'Active' and a.memberpageprivilege_privilege_id =@v0 ORDER BY b.member_fullname,name", new object[] { privilege_id });
					#endregion query
					}
				else
					{
					#region query
					dt = Toolbox.doSQL_dt(@" SELECT b.member_id id, a.memberpage_page_ID, MEMBER_NAME(b.member_id) as name, b.business_unit_id parent_id, c.membertype_name FROM memberpage a INNER JOIN member b ON b.member_id = a.memberpage_member_id INNER JOIN membertype c ON b.member_membertype_id = c.membertype_id  WHERE b.member_status = 'Active' and a.memberpage_page_id =@v0 ORDER BY b.member_fullname, name", new object[] { page_id });
					#endregion query
					}
				foreach (DataRow dr in dt.Rows)
					{
					var id				= dr["id"];
					var key				= "M" + id;
					var name			= dr["name"].ToString();
					var parent_id	= dr["parent_id"].ToString();
					var membertype_name	= dr["membertype_name"].ToString();
					var pages	= CreatepopNodeCore2(key, "<font color='DarkBlue'>" + name + "</font>", membertype_name, tree_priv_list.FindNodeByKeyValue(parent_id));
					}
				}
			else if (_listtype == "contact")
				{
				var custwhere = include_all ? "" : "  and b.contact_cust_id = " + _customerid;
				if (is_priv)
					{
					#region query
					dt = Toolbox.doSQL_dt(@"
SELECT 
	c.customer_id id, 
	c.customer_name name, 
	b.contact_name as membername, 
	a.contactpageprivilege_privilege_id 
FROM 
	contactpageprivilege a 
INNER JOIN 
	contact b ON b.contact_ID = a.contactpageprivilege_contact_id 
INNER JOIN 
	customer c ON b.contact_cust_ID = c.customer_ID 
WHERE 
	b.contact_status = 'Active' and 
	b.contact_type = 'Customer' and 
	TRIM(b.contact_email) != '' AND
	a.contactpageprivilege_privilege_id = " + privilege_id + custwhere + @" 
GROUP BY 
	customer_id",null);
					#endregion query
					}
				else
					{
					#region query
					dt = Toolbox.doSQL_dt(@"
SELECT 
	c.customer_id id, 
	c.customer_name name, 
	b.contact_name as membername, 
	a.contactpage_page_id 
FROM 
	contactpage a 
INNER JOIN 
	contact b ON b.contact_id = a.contactpage_contact_id 
INNER JOIN 
	customer c ON b.contact_cust_id = c.customer_id 
WHERE 
	b.contact_status = 'Active' and 
	TRIM(b.contact_email) != '' AND
	a.contactpage_page_id = " + page_id + @" and 
	b.contact_type = 'Customer' " + custwhere + @" 
GROUP BY 
	customer_id",null);
				#endregion query
			}
				foreach (DataRow dr in dt.Rows)
					{
					var id				= dr["id"];
					var name			= Toolbox.do_value_from(dr["name"], false);
					var pages = CreatepopNodeCore2(id, "<b><font color='Black'>" + name + "</font></b>", "", null);
					}

				// now set up the member level connecting to the company higher level
				if (is_priv)
					{
					#region query
					dt = Toolbox.doSQL_dt(@"
SELECT 
	b.contact_id id,
	a.contactpageprivilege_privilege_id, 
	b.contact_name as name,
	b.contact_cust_id parent_id
FROM 
	contactpageprivilege a 
INNER JOIN 
	contact b ON b.contact_id = a.contactpageprivilege_contact_id 
WHERE 
	b.contact_status = 'Active' and 
	TRIM(b.contact_email) != '' AND
	a.contactpageprivilege_privilege_id = " + privilege_id + custwhere + @" 
ORDER BY 
	b.contact_name", null);
				#endregion query
			}
				else
					{
					#region query
					dt = Toolbox.doSQL_dt(@"
SELECT 
	b.contact_id id,
	a.contactpage_page_id, 
	b.contact_name as name,
	b.contact_cust_id  parent_id
FROM 
	contactpage a 
INNER JOIN 
	contact b ON b.contact_id = a.contactpage_contact_id 
WHERE 
	b.contact_status = 'Active' and 
	TRIM(b.contact_email) != '' AND
	a.contactpage_page_id = " + page_id + custwhere + @" 
ORDER BY 
	b.contact_name", null);
				#endregion query
			}
				foreach (DataRow dr in dt.Rows)
					{
					var id				= dr["id"];
					var key			= "M" +id;
					var parent_id	= dr["parent_id"].ToString();
					var name			= dr["name"];
					var pages = CreatepopNodeCore2(key, "<font color='DarkBlue'>" + name + "</font>", "", tree_priv_list.FindNodeByKeyValue(parent_id));
					}
				}

		}
	private TreeListNode CreatepopNodeCore2(object key, string page, string text, TreeListNode parentNode)
		{

		var node = tree_priv_list.AppendNode(key, parentNode);
		node.AllowSelect = key.ToString().Substring(0, 1) == "M";
		node["Business Unit/Name"] = page;
		node["MemberType"] = text;
		return node;
		}
	protected void btnAdd_Click(object sender, EventArgs e)
		{
		foreach (var tn in tree_memlist.GetSelectedNodes())
			{
			var user_id		= Convert.ToInt32(tn.Key.Remove(0, 1));
			var type_id		= _listtype == "member" ? 1 : _listtype == "contact" ?  2 : 0;
			NeMember user;
			if(type_id == 2)
				{
				var co	= new NEContact(Convert.ToInt32(user_id));
				if(!Toolbox.CheckEmail(co.Contact_Email))
					{
					throw new Exception(string.Format("The contact ({0}) does not have a valid email address, stopping privilege process", co.Contact_Name));
					}
				user			= new NeMember(co.nesi_member_id);
				}
			else
				{
				user			= new NeMember((int) user_id);
				}
			var priv_id		= 0;
			var page_id		= 0;
			var is_priv	= this_key.Substring(0, 1) == "P";
			if (is_priv)
				{
				#region Privilege
				priv_id							= Convert.ToInt32(this_key.Remove(0, 1));
				var p					= new NePrivilege(priv_id);
				p.admin							= current_user;
				p.user							= user;
				var user_priv		= new NEUserPrivilege();
				user_priv.admin					= current_user;
				user_priv.user					= user;
				var user_page			= new NEUserPage();
				user_page.admin					= current_user;
				user_page.user					= user;
				page_id							= p.page_id;
				var exists						= NEUserPrivilege.exists(priv_id, user_id, type_id);
				if (!exists)  // if it doesn't exist , check to see if the page exists
					{
					exists						= NEUserPage.exists(user_id, page_id, type_id);
					if(!exists)
						{
						user_page.user_id			= user_id;
						user_page.page_id			= page_id;
						user_page.type_id			= type_id;
						user_page.save();
						}
					else
						{
						user_page.id				= NEUserPage.get_id(user_id, page_id, type_id);
						}
					user_priv.user_id				= Convert.ToInt32(user_id);
					user_priv.typepage_id			= Convert.ToInt32(user_page.id);
					user_priv.privilege_id			= priv_id;
					user_priv.type_id				= type_id;
					user_priv.save();
					}
				#endregion Privilege
				}
			else
				{
				#region Page
				page_id								= Convert.ToInt32(this_key);
				if(!NEUserPage.exists(user_id, page_id, type_id))
					{
					var NE_up					= new NEUserPage();
					NE_up.admin							= current_user;
					NE_up.user							= user;
					NE_up.page_id						= page_id;
					NE_up.user_id						= user_id;
					NE_up.type_id						= type_id;
					NE_up.save();
					}
				#endregion Page
				}
			}

		tree_priv_list.ClearNodes();
		CreatePopupNodes2();
		tree_priv_list.ExpandAll();
		tree_memlist.UnselectAll();
		}
	protected void btn_delete_Click(object sender, EventArgs e)
		{
		foreach (var tn in tree_priv_list.GetSelectedNodes())
			{
			var user_id		= Convert.ToInt32(tn.Key.Remove(0, 1));
			var _user_id	= (int) user_id;
			var type_id		= _listtype == "member" ? 1 : _listtype == "contact" ?  2 : 0;
			NeMember user;
			if(type_id == 2)
				{
				var co	= new NEContact(user_id);
				user			= new NeMember(co.nesi_member_id);
				}
			else
				{
				user			= new NeMember((int) user_id);
				}
			var priv_id		= 0;
			var page_id		= 0;
			var is_priv	= this_key.Substring(0, 1) == "P";
			if (is_priv)
				{
				priv_id					= Convert.ToInt32(this_key.Remove(0, 1));
				var p			= new NePrivilege(priv_id);
				page_id					= p.page_id;
				var NE_up	= new NEUserPrivilege(priv_id, (int) user_id, type_id);
				NE_up.admin				= current_user;
				NE_up.user				= user;
 				NE_up.delete();
				}
			else
				{
				page_id					= Convert.ToInt32(this_key);
				var	NE_up		= new NEUserPage((int) user_id, page_id, type_id);
				NE_up.admin				= current_user;
				NE_up.user				= user;
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
			Toolbox.doSQL_void(@"Update privilege set privilege_enabled =@v1  where privilege_id =@v0",new object[]{priv_id, Convert.ToInt16(chkEnabled.Value) });
			btnApply.ClientEnabled = false;
			chkEnabled.Value = _tools.getSQL_int(@"Select privilege_enabled from privilege  where privilege_id =@v0", new object[] { priv_id });


			}
		else
			{
			page_id = this_key;
			Toolbox.doSQL_void(@"Update page set page_enabled = @v1  where page_id =@v0",new object[]{page_id, Convert.ToInt16(chkEnabled.Value) });
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
		foreach(var tn in tl.GetSelectedNodes())
			{
			tn.Expanded		= tn.HasChildren;
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
		var o		= e.Parameter.Split('|');
		this_key	= o[0];

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