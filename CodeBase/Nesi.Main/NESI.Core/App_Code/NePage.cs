using System;
using System.Data;
using System.Collections;
using NESI.Common.Models;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NePage
	/// </summary>
	public class NePage : System.Web.UI.Page
		{
		public int id { get; set; }
		new public int ID { get; set; }
		public int ParentID { get; set; }
		public int parent_id { get; set; }
		public string Name { get; set; }
		public string Img { get; set; }
		public string Class { get; set; }
		public string Img_h { get; set; }
		public string Path { get; set; }
		public string Description { get; set; }
		public int Order { get; set; }
		public bool cust_enabled { get; set;}
		public bool vend_enabled { get; set;}
		public bool enabled { get; set;}
		public bool mobile_ready { get; set;}
		public bool desktop_ready { get; set; }
		public string mobile_icon { get; set;}
		public string mobile_url { get; set;}
		public string mobile_action { get; set;}
		public System.Collections.Generic.List<int> used_privileges		 { get; set; }

		public ArrayList Privileges { get; set; }
		public ArrayList ChildPages { get; set; }

		public NePage()
			{
			used_privileges	= new System.Collections.Generic.List<int>(new int[]{});
			}
		public NePage(int _id)
			{
			if(exists(_id))
				{
				init(_id);
				}
			else
				{
				throw new Exception("Page ID does not exist");
				}
			}
		public bool exists(int _id)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(page_id) FROM page WHERE page_id = @v0", _id) > 0;
			}
		private void init(int _id)
			{
			var dr			= Toolbox.doSQL_dt(@"SELECT * FROM page WHERE page_id = @v0  LIMIT 1", new object[] {  _id } ).Rows[0];
			ID					= _id;
			id					= _id;
			ParentID			= Convert.ToInt32(dr["page_parent_id"]);
			parent_id			= Convert.ToInt32(dr["page_parent_id"]);
			Name				= dr["page_name"].ToString();
			Description			= dr["Page_Desc"].ToString();
			Path				= dr["Page_ScriptPath"].ToString();
			Order				= Convert.ToInt32(dr["Page_Order"]);
			Img					= dr["Page_MainMenu_Img"].ToString();
			Img_h				= dr["Page_MainMenu_Img_H"].ToString();
			enabled				= Convert.ToInt32(dr["Page_Enabled"]) == 1;
			cust_enabled		= Convert.ToInt32(dr["Page_CustEnabled"]) == 1;
			vend_enabled		= Convert.ToInt32(dr["Page_VendorEnabled"]) == 1;
			Class				= dr["page_class"].ToString();
			mobile_ready		= Convert.ToBoolean(dr["page_mobile_ready"]);
			mobile_url			= dr["page_mobile_url"].ToString();
			mobile_icon			= dr["page_mobile_icon"].ToString();
			mobile_action		= dr["page_mobile_action"].ToString();
			desktop_ready		= Convert.ToBoolean(dr["page_desktop_ready"]);
			}

		// this is a horrible way for instantiating an object
		public NePage(int id, int parentid, string name, string desc, string path, int order, string img, string img_h)
			{
			ID = id;
			ParentID = parentid;
			Name = name;
			Img = img;
			Img_h = img_h;
			Description = desc;
			Path = path;
			Order = order;
			}

		public string get_page_desc(int pageid)
			{
			return Toolbox.doSQL_string("Select page_desc from page where page_id = @v0" , pageid);
			}
		public static string get_page_name(object pageid)
			{
			return Toolbox.doSQL_string("Select page_name from page where page_id =@v0 ", new object[] {
				pageid});
			}

		public void AddPrivilege(NePrivilege priv)
			{
			if(Privileges == null)
				{
				Privileges		= new ArrayList();
				}
			Privileges.Add(priv);
			}

		}
	/// <summary>
	/// Summary description for Privilege
	/// </summary>
	public class NeGlobalPrivilegeLink
		{
		public NeGlobalPrivilegeLink(){}
		public NeGlobalPrivilegeLink(int _id)
			{
			if(exists(_id))
				{
				init(_id);
				}
			}
		public int id					{ get; set; }
		public int type_id				{ get; set; }
		public int user_id				{ get; set; }
		public int global_priv_id		{ get; set; }
	
		public static void clear(int _user_id, int _type_id)
            {
            var _dt = Toolbox.doSQL_dt(@"SELECT id FROM privilege_global_link WHERE type_id = @v0  AND user_id = @v1 ", new object[] {  _type_id, _user_id } );
            foreach (DataRow _dr in _dt.Rows)
                {
                var id = Convert.ToInt32(_dr["id"]);
                var gpl = new NeGlobalPrivilegeLink(id);
                gpl.delete();
                }
            }
        public static bool exists(int _id)
            {
            return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM privilege_global_link WHERE id = @v0", _id) > 0;
            }
		private void init(int _id)
            {
			var _dt			= Toolbox.doSQL_dt(@" SELECT type_id, user_id, global_priv_id FROM privilege_global_link WHERE id = @v0  LIMIT 1 ", new object[] {  _id } );
			if(_dt.Rows.Count == 1)
				{
				var _dr			= _dt.Rows[0];
				id					= _id;
				type_id				= Convert.ToInt32(_dr["type_id"]);
				user_id				= Convert.ToInt32(_dr["user_id"]);
				global_priv_id		= Convert.ToInt32(_dr["global_priv_id"]);
				}
			}
		public void delete()
			{
			if(id == 0)
				{
				throw new Exception("Global Privilege Link not defined");
				}
			if(exists(id))
				{
				Toolbox.doSQL_void(@"DELETE FROM privilege_global_link WHERE id = @v0 LIMIT 1", id);
				}
			else
				{
				throw new Exception("Global Privilege Link doesn't exist");
				}
			}
		public void save()
			{
			if(id == 0)
				{
				// INSERT
				id	= Toolbox.doSQL_return_id(@"
INSERT INTO privilege_global_link
	(
	type_id,
	user_id,
	global_priv_id
	)
VALUES
	(
	@v0,
	@v1,
	@v2
	)
", new object[] {

					type_id,							// {0}
					user_id,							// {1}
					global_priv_id						// {2}
				});
				}
			else
				{
				// UPDATE
				Toolbox.doSQL_void(@"
UPDATE
	privilege_global_link
SET
	type_id			= @v0,
	user_id			= @v1,
	global_priv_id	= @v2
WHERE
	id = @v3
LIMIT 1
", new object[] {

					type_id,							// {0}
					user_id,							// {1}
					global_priv_id,						// {2}
					id									// {3}
				});
				}
			}
		}
	/// <summary>
	/// Summary description for Privilege
	/// </summary>
	public class NePrivilege
		{
		/*
privilege_id					ID
privilege_page_id				PageID
privilege_name					Name
privilege_desc					Description
privilege_order					Order
privilege_enabled				Enabled
privilege_custenabled			CustEnabled
privilege_vendorenabled			VendorEnabled
	 */
		public NePrivilege(){}
		public NePrivilege(int _id)
			{
			if(exists(_id))
				{
				init(_id);
				}
			}
		public NePrivilege(int _id, int _page_id, string _name, string _desc)
			{
			id				= _id;
			page_id			= _page_id;
			name			= _name;
			description		= _desc;
			}
		public int id					{ get; set; }
		public int page_id				{ get; set; }
		public string name				{ get; set; }
		public string description		{ get; set; }
		public int order				{ get; set; }
		public bool enabled				{ get; set; }
		public bool cust_enabled		{ get; set; }
		public bool vendor_enabled		{ get; set; }
		public NeMember admin			{ get; set; }
		public NeMember user			{ get; set; }
		public NeMemberType mt { get; set; }

		public static bool exists(int _id)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(privilege_id) FROM privilege WHERE privilege_id = @v0", _id) > 0;
			}
		private void init(int _id)
			{
			var _dt			= Toolbox.doSQL_dt(@" SELECT privilege_page_id page_id, privilege_name name, privilege_desc description, privilege_order this_order, IFNULL(privilege_enabled, 0) enabled, IFNULL(privilege_custenabled, 0) cust_enabled, IFNULL(privilege_vendorenabled, 0) vend_enabled FROM privilege WHERE privilege_id = @v0  LIMIT 1 ", new object[] {  _id } );
			if(_dt.Rows.Count == 1)
				{
				var _dr			= _dt.Rows[0];
				id					= _id;
				page_id				= Convert.ToInt32(_dr["page_id"]);
				name				= _dr["name"].ToString();
				description			= _dr["description"].ToString();
				order				= Convert.ToInt32(_dr["this_order"]);
				enabled				= Convert.ToBoolean(_dr["enabled"]);
				cust_enabled		= Convert.ToBoolean(_dr["cust_enabled"]);
				vendor_enabled		= Convert.ToBoolean(_dr["vend_enabled"]);
				}
			}
		public void delete()
			{
			if(id == 0)
				{
				throw new Exception("Privilege object not defined");
				}
			if(exists(id))
				{
				// Check links
				if(is_linked(id))
					{
					throw new Exception("Cannot delete, is still linked");
					}
				else
					{
					Toolbox.doSQL_void(@"DELETE FROM privilege WHERE privilege_id = @v0 LIMIT 1", id);
					}
				}
			else
				{
				throw new Exception("Privilege doesn't exist");
				}
			}
		public bool is_linked(int _id)
			{
			var _is_linked			= false;
			// check member page privilege
			if(Toolbox.doSQL_int(@"SELECT COUNT(memberpageprivilege_id) FROM memberpageprivilege WHERE memberpageprivilege_privilege_id = @v0 ", _id) > 0)
				{
				_is_linked			= true;				
				}
			// check member type page privilege
			if(Toolbox.doSQL_int(@"SELECT COUNT(membertypepageprivilege_id) FROM membertypepageprivilege WHERE membertypepageprivilege_privilege_id = @v0 ", _id) > 0)
				{
				_is_linked			= true;				
				}
			// check page privilege
			if(Toolbox.doSQL_int(@"SELECT COUNT(pageprivilege_id) FROM pageprivilege WHERE pageprivilege_privilege_id = @v0 ", _id) > 0)
				{
				_is_linked			= true;				
				}
			return _is_linked;
			}
		public void save()
			{
			if(id == 0)
				{
				// INSERT
				id	= Toolbox.doSQL_return_id(@"
INSERT INTO privilege
	(
	privilege_page_id,
	privilege_name,
	privilege_desc,
	privilege_order,
	privilege_enabled,
	privilege_custenabled,
	privilege_vendorenabled
	)
VALUES
	(
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v5,
	@v6
	)
", new object[] {

					page_id,							// {0}
					name,			// {1}
					description,	// {2}
					order,								// {3}
					enabled,							// {4}
					cust_enabled,						// {5}
					vendor_enabled						// {6}
				});
				}
			else
				{
				// UPDATE
				Toolbox.doSQL_void(@"
UPDATE
	privilege
SET
	privilege_page_id			= @v0,
	privilege_name				= @v1,
	privilege_desc				= @v2,
	privilege_order				= @v3,
	privilege_enabled			= @v4,
	privilege_custenabled		= @v5,
	privilege_vendorenabled		= @v6
WHERE
	privilege_id = @v7
LIMIT 1
", new object[] {
					page_id,							// {0}
					name,			// {1}
					description,	// {2}
					order,								// {3}
					enabled,							// {4}
					cust_enabled,						// {5}
					vendor_enabled,						// {6}
					id									// {7}
				});
				}
			}
		}
	public class NEUserPage
		{
		public int id					{ get; set; }
		public bool active				{ get; set; }
		public int type_id				{ get; set; }
		public int page_id				{ get; set; }
		public int user_id				{ get; set; }
		public NeMember admin			{ get; set; }
		public NeMember user			{ get; set; }

		public NEUserPage(){}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_id"></param>
		/// <param name="_type_id">1 = member, 2 = contact</param>
		public NEUserPage(int _id, int _type_id)
			{
			if(exists(_id, _type_id))
				{
				init(_id, _type_id);
				}
			}
		public static void clear(NeMember _admin, NeMember _user, int _type_id)
			{
			var _dt			= Toolbox.doSQL_dt(@"SELECT memberpage_id id FROM memberpage WHERE memberpage_member_id = @v0",new object[] {  _user.id}); 
			foreach(DataRow _dr in _dt.Rows)
				{
				var id				= Convert.ToInt32(_dr["id"]);
				var up		= new NEUserPage(id, _type_id);
				up.admin			= _admin;
				up.user				= _user;
				up.delete();
				}
			}
		public void delete()
			{
			if(exists(id, type_id))
				{
				var dt = Toolbox.doSQL_dt(@"SELECT page_id FROM page  WHERE page_parent_id = @v0 AND page_parent_id != 0", new object[] { page_id });
				foreach (DataRow dr in dt.Rows)
					{

					var NE_up = new NEUserPage(user_id, Convert.ToInt32(dr["page_id"]), type_id);
					NE_up.admin = admin;
					NE_up.user = user;
					NE_up.user_id = user_id;
					NE_up.type_id = type_id;
					NE_up.id = Convert.ToInt32(dr["page_id"]);
					NE_up.delete();

					}

				// Check if it's mapped in the user privilege table
				if(NEUserPrivilege.is_mapped(id, type_id))
					{
					// Delete all child nodes
					Toolbox.doSQL_void($@"DELETE FROM memberpageprivilege WHERE memberpageprivilege_memberpage_id = '{id}'");
					}
				Toolbox.doSQL_void($@"DELETE FROM memberpage WHERE memberpage_id = '{id}' LIMIT 1");
				write_log(true);
				}
			else
				{
				//	throw new Exception("This user page entry does not exist");
				}
			}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_user_id"></param>
		/// <param name="_page_id"></param>
		/// <param name="_type_id">1 = member, 2 = contact</param>
		public NEUserPage(int _user_id, int _page_id, int _type_id)
			{
			if(exists(_user_id, _page_id, _type_id))
				{
				id			= get_id(_user_id, _page_id, _type_id);
				user_id		= _user_id;
				page_id		= _page_id;
				type_id		= _type_id;
				active		= is_active(id, type_id);
				}
			}
		public static bool is_active(int _id, int _type_id)
			{
			return Toolbox.doSQL_bool($@"SELECT active FROM memberpage WHERE memberpage_id = '{_id}'",null);
			}
		public static bool exists(int _id, int _type_id)
			{
			return Toolbox.doSQL_int($@"SELECT COUNT(memberpage_id) FROM memberpage WHERE memberpage_id = '{_id}'") > 0;
			}
		public static bool exists(int _user_id, int _page_id, int _type_id)
			{
			return Toolbox.doSQL_int($@"SELECT COUNT(memberpage_id) FROM memberpage WHERE memberpage_member_id = '{_user_id}' AND memberpage_page_id = '{_page_id}'") > 0;
			}
		public static int get_id(int _user_id, int _page_id, int _type_id)
			{
			return Toolbox.doSQL_int($@"SELECT IFNULL(MAX(memberpage_id),0) FROM memberpage WHERE memberpage_member_id = '{_user_id}' AND memberpage_page_id = '{_page_id}'");
			}
		private void init(int _id, int _type_id)
			{
			var _dt			= Toolbox.doSQL_dt(@"
SELECT
	memberpage_member_id user_id,
	memberpage_page_id page_id
FROM
	memberpage
WHERE 
	memberpage_id = @v0
LIMIT 1
	", new object[] { _id });
			if (_dt.Rows.Count == 1)
				{
				var _dr			= _dt.Rows[0];
				id					= _id;
				page_id				= Convert.ToInt32(_dr["page_id"]);
				user_id				= Convert.ToInt32(_dr["user_id"]);
				type_id				= _type_id;
				}
			}
		private void write_log(bool is_delete)
			{
			var l = new NELog
						{
						is_manual        = true,
						business_unit_id = user.business_unit_id,
						table            = OpsLog.Table.MemberPage,
						table_id         = user.id,
						value_old        = is_delete ? id : 0,
						value_new        = is_delete ? 0 : id,
						member_id        = admin.id,
						alt_table_id     = page_id,
						action_id        = is_delete ? OpsLog.Action.DeletedUserPage : OpsLog.Action.AddedUserPage,
						section_id       = OpsLog.Section.PageClass
						};
			l.save();
			}
		public void save()
			{
			// Check parents
			var this_page		= new NePage(page_id);
			if(this_page.ParentID > 0)
				{
				// Check if this user has access to the parent page... if not, give it to them.
				if(!exists(user_id, this_page.parent_id, type_id))
					{
					var NE_up = new NEUserPage(this_page.parent_id, type_id)
									{
									admin   = admin,
									user    = user,
									page_id = this_page.parent_id,
									user_id = user_id,
									type_id = type_id
									};
					NE_up.save();
					}
				}
			if(id == 0)
				{
				// INSERT
				id	= Toolbox.doSQL_return_id($@"
INSERT INTO memberpage
	(	
	memberpage_member_id,
	memberpage_page_id
	)
VALUES
	(
	'{user_id}',
	'{page_id}'
	)
",null);
				write_log(false);
				}
			else
				{
				// UPDATE
				Toolbox.doSQL_void($@"
UPDATE
	memberpage
SET
	memberpage_page_id			= '{page_id}',
	memberpage_member_id		= '{user_id}'
WHERE
	memberpage_id = '{id}'
LIMIT 1
");
				}
			}
		}
	public class NeMTPage
		{
		public int id					{ get; set; }
		public int page_id				{ get; set; }
		public int mt_id				{ get; set; }
		public NeMember admin			{ get; set; }
		public NeMemberType mt			{ get; set; }

		public NeMTPage(){}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_id"></param>
		/// <param name="_type_id">1 = member, 2 = contact</param>
		public NeMTPage(int _id)
			{
			if(exists(_id))
				{
				init(_id);
				}
			}
		public static void clear(NeMember _admin, NeMemberType mt)
			{
			var used_id = mt.MemberTypeID;
			var _dt			= Toolbox.doSQL_dt(@"SELECT membertypepage_id id FROM membertypepage WHERE membertypepage_type_id = @v0", new object[] { used_id });
			foreach (DataRow _dr in _dt.Rows)
				{
				var id				= Convert.ToInt32(_dr["id"]);
				var up		= new NeMTPage(id);
				up.admin			= _admin;
				up.mt			= mt;
				up.delete();
				}
			}
		public void delete()
			{
			if(exists(id))
				{
				// first check if there is a deeper page.. then go there and wipe out privileges
				var dt = Toolbox.doSQL_dt(@"Select page_id from page  where Page_Parent_ID = @v0  and Page_Parent_ID !=0", new object[] { page_id });
				foreach (DataRow dr in dt.Rows)
					{
					
					var NE_up = new NeMTPage(mt_id,Convert.ToInt32(dr["page_id"]));
					NE_up.admin = admin;
					NE_up.mt = mt;
					NE_up.id = Convert.ToInt32(dr["page_id"]);
					NE_up.delete();

					}
				// Check if it's mapped in the user privilege table
				if(NEMTPrivilege.is_mapped(id))
					{
					// Delete all child nodes
					Toolbox.doSQL_void($@"DELETE FROM membertypepageprivilege WHERE membertypepageprivilege_membertypepage_id = '{id}'");
					}
			
				Toolbox.doSQL_void($@"DELETE FROM membertypepage WHERE membertypepage_id = '{id}' LIMIT 1");
				write_log(true);
				}
			else
				{
				//		throw new Exception("This user page entry does not exist");
				}
			}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_user_id"></param>
		/// <param name="_page_id"></param>
		/// <param name="_type_id">1 = member, 2 = contact</param>
		public NeMTPage(int _mt_id, int _page_id)
			{
			if(exists(_mt_id, _page_id))
				{
				id			= get_id(_mt_id, _page_id);
				mt_id		= _mt_id;
				page_id		= _page_id;
				}
			}
		public static bool exists(int _id)
			{
			return Toolbox.doSQL_int($@"SELECT COUNT(membertypepage_id) FROM membertypepage WHERE membertypepage_id = '{_id}'") > 0;
			}
	

		public static bool exists(int _user_id, int _page_id)
			{
			return Toolbox.doSQL_int($@"SELECT COUNT(membertypepage_id) FROM membertypepage WHERE membertypepage_type_id = '{_user_id}' AND membertypepage_page_id = '{_page_id}'") > 0;
			}
		public static int get_id(int _user_id, int _page_id)
			{
			return Toolbox.doSQL_int($@"SELECT IFNULL(MAX(membertypepage_id),0) FROM membertypepage WHERE membertypepage_type_id = '{_user_id}' AND membertypepage_page_id = '{_page_id}'");
			}
	
		private void init(int _id)
			{
			var _dt			= Toolbox.doSQL_dt(@"
SELECT
	membertypepage_type_id user_id,
	membertypepage_page_id page_id
FROM
	membertypepage
WHERE 
	membertypepage_id = @v0
LIMIT 1
	", new object[] { _id });
			if (_dt.Rows.Count == 1)
				{
				var _dr			= _dt.Rows[0];
				id					= _id;
				page_id				= Convert.ToInt32(_dr["page_id"]);
				mt_id				= Convert.ToInt32(_dr["user_id"]);
				}
			}
		private void write_log(bool is_delete)
			{
			var l = new NELog
						{
						is_manual        = true,
						business_unit_id = 999,
						table            = OpsLog.Table.MemberTypePage,
						table_id         = mt.MemberTypeID,
						value_old        = is_delete ? id : 0,
						value_new        = is_delete ? 0 : id,
						member_id        = admin.id,
						alt_table_id     = page_id,
						action_id        = is_delete ? OpsLog.Action.DeletedUserPage : OpsLog.Action.AddedUserPage,
						section_id       = OpsLog.Section.PageClass
						};
			l.save();
			}
		public void save()
			{
			var _prefix = "membertype";
			// Check parents
			var this_page		= new NePage(page_id);
			if(this_page.ParentID > 0)
				{
				// Check if this user has access to the parent page... if not, give it to them.
				if(!exists(mt_id, this_page.parent_id))
					{
					var NE_up		= new NeMTPage(this_page.parent_id);
					NE_up.admin				= admin;
					NE_up.mt				= mt;
					NE_up.page_id			= this_page.parent_id;
					NE_up.mt_id			= mt_id;
					NE_up.save();
					}
				}
			if(id == 0)
				{
				// INSERT
				id	= Toolbox.doSQL_return_id($@"
INSERT INTO memberpage
	(	
	memberpage_type_id,
	memberpage_page_id
	)
VALUES
	(
	'{mt_id}',
	'{page_id}'
	)
",null);
				write_log(false);
				}
			else
				{
				// UPDATE
				Toolbox.doSQL_void($@"
UPDATE
	memberpage
SET
	memberpage_page_id			= '{page_id}',
	memberpage_type_id		= '{mt_id}'
WHERE
	memberpage_id = '{id}'
LIMIT 1
");
				}
			}
		}
	public class NEMTPrivilege
		{
		public int id { get; set; }
	
		public int typepage_id  { get; set; }
		public int privilege_id  { get; set; }
		public int mt_id { get; set; }
		public NeMember admin { get; set; }
		public NeMemberType mt { get; set; }
		public NEMTPrivilege(){}
		public NEMTPrivilege(int _privilege_id, int _mt_id)
			{
			if(exists(_privilege_id, _mt_id))
				{
				init(_privilege_id, _mt_id);
				}
			}
		public NEMTPrivilege(int _id)
			{
			if(exists(_id))
				{
				init(_id);
				}
			}

		public static void clear(NeMember _admin, NeMember _user, int _type_id)
			{
			var _dt			= Toolbox.doSQL_dt(@"SELECT membertypepageprivilege_id id FROM membertypepageprivilege WHERE membertypepageprivilege_type_id =@v0", new object[] { _user.id });
			foreach (DataRow _dr in _dt.Rows)
				{
				var id					= Convert.ToInt32(_dr["id"]);
				var up		= new NEUserPrivilege(id, _type_id);
				up.admin				= _admin;
				up.user					= _user;
				up.delete();
				}
			}
		public void delete()
			{
			if(id == 0)
				{
				throw new Exception("Privilege object not defined");
				}
			Toolbox.doSQL_void($@"DELETE FROM membertypepageprivilege WHERE membertypepageprivilege_id = '{id}' LIMIT 1");
			write_log(true);
			}
		private void write_log(bool is_delete)
			{
			var l = new NELog
						{
						is_manual        = true,
						business_unit_id = 999,
						table            = OpsLog.Table.MemberTypePagePrivilege,
						table_id         = mt.MemberTypeID,
						value_old        = is_delete? id: 0,
						value_new        = is_delete? 0: id,
						member_id        = admin.id,
						alt_table_id     = privilege_id,
						action_id        = is_delete? OpsLog.Action.DeletedUserPrivilege: OpsLog.Action.AddedUserPrivilege,
						section_id       = OpsLog.Section.PrivilegeClass
						};
			l.save();
			}
		public static bool is_mapped(int _userpage_id)
			{
			return Toolbox.doSQL_int($@"SELECT COUNT(*) FROM membertypepageprivilege WHERE membertypepageprivilege_membertypepage_id = '{_userpage_id}'") > 0;
			}
		public static bool exists(int _id)
			{
			return Toolbox.doSQL_int($@"SELECT COUNT(*) FROM membertypepageprivilege WHERE membertypepageprivilege_id = '{_id}'") > 0;
			}
		public static bool exists(int _privilege_id, int _user_id)
			{
			return Toolbox.doSQL_int($@"SELECT COUNT(*) FROM membertypepageprivilege WHERE membertypepageprivilege_privilege_id = '{_privilege_id}' AND membertypepageprivilege_type_id = {_user_id}") > 0;
			}
		private void init(int _priv_id, int _user_id)
			{
			var _dt			= Toolbox.doSQL_dt(@"
SELECT
	membertypepageprivilege_id id,
	membertypepageprivilege_membertypepage_id typepage_id,
	membertypepageprivilege_privilege_id privilege_id,
	membertypepageprivilege_type_id user_id
FROM
	membertypepageprivilege
WHERE
	membertypepageprivilege_privilege_id = @v0 AND
	membertypepageprivilege_type_id =@v1
LIMIT 1
	",new object[] {  _priv_id, _user_id});
			if(_dt.Rows.Count == 1)
				{
				var _dr			= _dt.Rows[0];
				id					= Convert.ToInt32(_dr["id"]);
				typepage_id			= Convert.ToInt32(_dr["typepage_id"]);
				privilege_id		= Convert.ToInt32(_dr["privilege_id"]);
				mt_id				= Convert.ToInt32(_dr["user_id"]);
			
				}
			}
		private void init(int _id)
			{
			var _dt			= Toolbox.doSQL_dt(@"
SELECT
	membertypepageprivilege_id id,
	membertypepageprivilege_membertypepage_id typepage_id,
	membertypepageprivilege_privilege_id privilege_id,
	membertypepageprivilege_type_id user_id
FROM
	membertypepageprivilege
WHERE
	membertypepageprivilege_id = @v0
LIMIT 1
	", new object[] { _id });
			if (_dt.Rows.Count == 1)
				{
				var _dr			= _dt.Rows[0];
				id					= _id;
				typepage_id			= Convert.ToInt32(_dr["typepage_id"]);
				privilege_id		= Convert.ToInt32(_dr["privilege_id"]);
				mt_id				= Convert.ToInt32(_dr["user_id"]);
			
				}
			}
		public void save()
			{
			if(id == 0)
				{
				// INSERT
				id					= Toolbox.doSQL_return_id($@"
INSERT INTO membertypepageprivilege
	(
	membertypepageprivilege_membertypepage_id,
	membertypepageprivilege_privilege_id,
	membertypepageprivilege_type_id
	)
VALUES
	(
	'{typepage_id}',
	'{privilege_id}',
	'{mt_id}'
	)
",null);
				write_log(false);
				}
			else
				{
				// UPDATE
				Toolbox.doSQL_void($@"
UPDATE
	membertypepageprivilege
SET
	membertypepageprivilege_membertypepage_id = '{typepage_id}',
	membertypepageprivilege_privilege_id = '{privilege_id}',
	membertypepageprivilege_type_id = '{mt_id}'
WHERE
	membertypepageprivilege_id = '{id}'
LIMIT 1
");
				}
			}
		}
	public class NEUserPrivilege
		{
		public int id { get; set; }
		public int type_id { get; set; }
		public int typepage_id  { get; set; }
		public int privilege_id  { get; set; }
		public int user_id { get; set; }
		public NeMember admin { get; set; }
		public NeMember user { get; set; }
		public NEUserPrivilege(){}
		public NEUserPrivilege(int _privilege_id, int _user_id, int _type_id)
			{
			if(exists(_privilege_id, _user_id, _type_id))
				{
				init(_privilege_id, _user_id, _type_id);
				}
			}
		public NEUserPrivilege(int _id, int _type_id)
			{
			if(exists(_id, _type_id))
				{
				init(_id, _type_id);
				}
			}
		public static void clear(NeMember _admin, NeMember _user, int _type_id)
			{
			var _dt			= Toolbox.doSQL_dt(@"SELECT memberpageprivilege_id id FROM memberpageprivilege WHERE memberpageprivilege_member_id = @v0", new object[] { _user.id });
			foreach (DataRow _dr in _dt.Rows)
				{
				var id					= Convert.ToInt32(_dr["id"]);
				var up		= new NEUserPrivilege(id, _type_id);
				up.admin				= _admin;
				up.user					= _user;
				up.delete();
				}
			}
		public void delete()
			{
			if(type_id == 0 || id == 0)
				{
				throw new Exception("Privilege object not defined");
				}
			Toolbox.doSQL_void(@"DELETE FROM memberpageprivilege WHERE memberpageprivilege_id = @v0 LIMIT 1", id);
			write_log(true);
			}
		private void write_log(bool is_delete)
			{
			var l = new NELog
						{
						is_manual        = true,
						business_unit_id = user.business_unit_id,
						table            = OpsLog.Table.MemberPagePrivilege,
						table_id         = user.id,
						value_old        = is_delete? id: 0,
						value_new        = is_delete? 0: id,
						member_id        = admin.id,
						alt_table_id     = privilege_id,
						action_id        = is_delete? OpsLog.Action.DeletedUserPrivilege: OpsLog.Action.AddedUserPrivilege,
						section_id       = OpsLog.Section.PrivilegeClass
						};
			l.save();
			}
		public static bool is_mapped(int _userpage_id, int _type_id)
			{
			return Toolbox.doSQL_int($@"SELECT COUNT(*) FROM memberpageprivilege WHERE memberpageprivilege_memberpage_id = '{_userpage_id}'") > 0;
			}
		public static bool exists(int _id, int _type_id)
			{
			return Toolbox.doSQL_int($@"SELECT COUNT(*) FROM memberpageprivilege WHERE memberpageprivilege_id = '{_id}'") > 0;
			}
		public static bool exists(int _privilege_id, int _user_id, int _type_id)
			{
			return Toolbox.doSQL_int($@"SELECT COUNT(*) FROM memberpageprivilege WHERE memberpageprivilege_privilege_id = '{_privilege_id}' AND memberpageprivilege_member_id = {_user_id}") > 0;
			}
		private void init(int _priv_id, int _user_id, int _type_id)
			{
			var _dt			= Toolbox.doSQL_dt(@"
SELECT
	memberpageprivilege_id id,
	memberpageprivilege_memberpage_id typepage_id,
	memberpageprivilege_privilege_id privilege_id,
	memberpageprivilege_member_id user_id
FROM
	memberpageprivilege
WHERE
	memberpageprivilege_privilege_id = @v0 AND
	memberpageprivilege_member_id = @v1
LIMIT 1
	",new object[] {  _priv_id, _user_id});
			if(_dt.Rows.Count == 1)
				{
				var _dr			= _dt.Rows[0];
				id					= Convert.ToInt32(_dr["id"]);
				typepage_id			= Convert.ToInt32(_dr["typepage_id"]);
				privilege_id		= Convert.ToInt32(_dr["privilege_id"]);
				user_id				= Convert.ToInt32(_dr["user_id"]);
				type_id				= _type_id;
				}
			}
		private void init(int _id, int _type_id)
			{
			var _dt			= Toolbox.doSQL_dt(@"
SELECT
	memberpageprivilege_id id,
	memberpageprivilege_memberpage_id typepage_id,
	memberpageprivilege_privilege_id privilege_id,
	memberpageprivilege_member_id user_id
FROM
	memberpageprivilege
WHERE
	memberpageprivilege_id = @v0
LIMIT 1", new object[] { _id });
			if (_dt.Rows.Count == 1)
				{
				var _dr			= _dt.Rows[0];
				id					= _id;
				typepage_id			= Convert.ToInt32(_dr["typepage_id"]);
				privilege_id		= Convert.ToInt32(_dr["privilege_id"]);
				user_id				= Convert.ToInt32(_dr["user_id"]);
				type_id				= _type_id;
				}
			}
		public void save()
			{
			if(id == 0)
				{
				// INSERT
				id					= Toolbox.doSQL_return_id($@"
INSERT INTO memberpageprivilege
	(
	memberpageprivilege_memberpage_id,
	memberpageprivilege_privilege_id,
	memberpageprivilege_member_id
	)
VALUES
	(
	'{typepage_id}',
	'{privilege_id}',
	'{user_id}'
	)
",null);
				write_log(false);
				}
			else
				{
				// UPDATE
				Toolbox.doSQL_void($@"
UPDATE
	memberpageprivilege
SET
	memberpageprivilege_memberpage_id = '{typepage_id}',
	memberpageprivilege_privilege_id = '{privilege_id}',
	memberpageprivilege_member_id = '{user_id}'
WHERE
	memberpageprivilege_id = '{id}'
LIMIT 1
");
				}
			}
		}
	public class NEUserPrivilegeTemplate
		{
		public int id { get; set; }
		public DateTime dt_added  { get; set; }
		public int added_by_member_id  { get; set; }
		public int type  { get; set; }
		public string name  { get; set; }
		public NEUserPrivilegeTemplate(){}
		public NEUserPrivilegeTemplate(int _id)
			{
			if(exists(_id))
				{
				init(_id);
				}
			}
		public NEUserPrivilegeTemplate(string _name, int _type)
			{
			if(exists(_name, _type))
				{
				var _id					= Toolbox.doSQL_int(@"SELECT id FROM pageprivilege_template WHERE name = @v0 AND type = @v1",
					new object[] {
					_name, _type});
				init(_id);
				}
			}
		private void init(int _id)
			{
			var _dt			= Toolbox.doSQL_dt(@" SELECT id, dt_added, added_by_member_id, type, name FROM pageprivilege_template WHERE id = @v0  LIMIT 1 ", new object[] {  _id } );
			if(_dt.Rows.Count == 1)
				{
				var _dr			= _dt.Rows[0];
				id					= _id;
				dt_added			= Convert.ToDateTime(_dr["dt_added"]);
				added_by_member_id	= Convert.ToInt32(_dr["added_by_member_id"]);
				type				= Convert.ToInt32(_dr["type"]);
				name				= _dr["name"].ToString();
				}
			}
		public void clear()
			{
			// Delete all child nodes
			Toolbox.doSQL_void(@"DELETE FROM pageprivilege_template_item WHERE template_id = @v0", id);
			}
		public void delete()
			{
			clear();
			// Delete main node
			Toolbox.doSQL_void(@"DELETE FROM pageprivilege_template WHERE id = @v0 LIMIT 1", id);
			}
		public void save()
			{
			if(id == 0)
				{
				// Insert
				id	= Toolbox.doSQL_return_id(@"
INSERT INTO pageprivilege_template
	(
	dt_added,
	added_by_member_id,
	type,
	name
	)
VALUES
	(
	NOW(),
	@v0,
	@v1,
	@v2
	)
", new object[] {

					added_by_member_id,					// {0}
					type,								// {1}
					name			// {2}
				});
				}
			else
				{
				// Update
				Toolbox.doSQL_void(@"
UPDATE
	pageprivilege_template
SET
	name						= @v0
WHERE
	id = @v1
LIMIT 1", new object[] {

					name,			// {0}
					id									// {1}
				});
				}
			}
		public bool exists(int _id)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM pageprivilege_template WHERE id = @v0 ", _id) > 0;
			}
		public bool exists(string _name, int _type_id)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM pageprivilege_template WHERE name = @v0 AND type = @v1 ",
				new object[] {
				_name, _type_id}) > 0;
			}
		}
	public class NEUserPrivilegeTemplate_item
		{
		public int id  { get; set; }
		public int template_id  { get; set; }
		public string type  { get; set; }
		public int table_id  { get; set; }
		public NEUserPrivilegeTemplate_item(){}
		public NEUserPrivilegeTemplate_item(int _id)
			{
			if(exists(_id))
				{
				init(_id);
				}
			}
		public NEUserPrivilegeTemplate_item(int _table_id, int _template_id, string _type)
			{
			var _id					= Toolbox.doSQL_int(@"SELECT id FROM pageprivilege_template_item 
WHERE template_id = @v0 AND table_id = @v1 AND type = @v2 ", new object[] {
				_template_id, _table_id, _type});
			init(_id);
			}
		private void init(int _id)
			{
			var _dt			= Toolbox.doSQL_dt(@" SELECT id, template_id, type, table_id FROM pageprivilege_template_item WHERE id = @v0  LIMIT 1", new object[] {  _id } );
			if(_dt.Rows.Count == 1)
				{
				var _dr			= _dt.Rows[0];
				id					= _id;
				template_id			= Convert.ToInt32(_dr["template_id"]);
				type				= _dr["type"].ToString();
				table_id			= Convert.ToInt32(_dr["table_id"]);
				}
			}
		public void delete()
			{
			Toolbox.doSQL_void(@"DELETE FROM pageprivilege_template_item WHERE id = @v0 LIMIT 1", id);
			}
		public void save()
			{
			if(id == 0)
				{
				// Insert Only... you never need to update an item entry.
				id	= Toolbox.doSQL_return_id(@"
INSERT INTO pageprivilege_template_item
	(
	template_id,
	type,
	table_id
	)
VALUES
	(
	@v0,
	@v1,
	@v2
	)
", new object[] {

					template_id,						// {0}
					type,			// {1}
					table_id							// {2}
				});
				}
			}
		public bool exists(int _id)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM pageprivilege_template_item WHERE id = @v0 ", _id) > 0;
			}
		public bool exists(int _table_id, int _template_id, string _type)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM pageprivilege_template_item WHERE 
template_id = @v0 AND table_id = @v1 AND type = @v2", new object[] {
				_template_id, _table_id, _type}) > 0;
			}
		}
	}