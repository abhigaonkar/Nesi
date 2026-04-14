using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using nesi.core;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Employees;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeePrivileges : EmployeeEdit
	{
		private readonly NeMember current_user;

		public EmployeePrivileges(Employee user, int mid) : base(user, mid)
		{
			current_user = new NeMember(UserId);
			can_access = tab_enabled[5];
		}

		public object GetSwitchUserProfile()
		{
			var source = bllToolbox.doSQL_Array<LabelValueInt>(
				@"SELECT member_id value, member_fullname label 
FROM member WHERE member_status='Active' 
AND business_unit_id = 8 AND member_id 
NOT IN (SELECT mapped_member_id FROM user_switch WHERE member_id = @p0) 
ORDER BY member_lastname, member_fullname", member_id);
			var target = bllToolbox.doSQL_Array<LabelValueInt>(
				@"SELECT b.member_id value, b.member_fullname label 
FROM user_switch a
LEFT JOIN member b ON a.mapped_member_id = b.member_id
WHERE a.member_id = @v0 ORDER BY b.member_lastname, b.member_fullname", member_id);
			return new
			{
				source,
				target,
			};
		}

		public DataExtra SaveSwitchUser(int[] target)
		{
			bllToolbox.doSQL_void(@"delete from user_switch where member_id=@v0", member_id);
			foreach (var id in target)
			{
				bllToolbox.doSQL_void(@"INSERT INTO user_switch (member_id,mapped_member_id) values (@p0,@p1)", member_id, id);
			}
			return new DataExtra("success", GetSwitchUserProfile());
		}

		private List<PrivilegeTreeNode> GetGlobalPrivileges(bool is_member)
		{
			var list = bllToolbox.doSQL_List<PrivilegeTreeNode>(@"SELECT
a.id, 
IFNULL(b.id,0) link_id,
a.name, 
a.name label, 
a.description,
IF(user_id IS NULL, 0, 1) is_checked 
FROM privilege_global a 
LEFT JOIN privilege_global_link b 
on b.global_priv_id = a.id
AND b.type_id = @v0  AND b.user_id = @v1 ", is_member ? 1 : 2, member_id);



			return list;
		}


		private List<PrivilegeTreeNode> GetReportEnginePrivileges()
		{
			var o_list = bllToolbox.doSQL_List<PrivilegeTreeNode>(@"SELECT 
a.id,
ifnull(b.id,0) link_id,
a.name name,
b.name label,
IF(c.id IS NULL, 0, 1) is_checked,
'' description
FROM r_engine_category a 
LEFT JOIN r_engine_report b 
ON b.r_engine_category = a.id 
LEFT JOIN r_engine_permission c
on c.r_engine_report = b.id AND 
c.member = @v0  ORDER BY a.name,b.name ", member_id);

			var cs = o_list.Select(x => x.name).Distinct().ToList();
			var list = new List<PrivilegeTreeNode>();
			foreach (var c in cs)
			{
				var item = new PrivilegeTreeNode()
				{
					Label = c
				};
				var children = o_list.Where(x => x.name == c).Select(x => new PrivilegeTreeNode()
				{
					id = x.id,
					link_id = x.link_id,
					name = x.Label,
					Label = x.Label,
					is_checked = x.is_checked,
					description = x.description
				}).ToList();
				item.Children = children;
				list.Add(item);
			}

			return list;
		}


		private List<PagePrivilegeTreeNode> GetPagePrivileges(int buid, bool onlyMemberType)
		{
			var memEdit = new NeMember(member_id);
			var TypePriv = new NEMemberTypePrivilege();

			TypePriv.TypePages(Convert.ToInt32(memEdit.MemberTypeID.ToString()));
			TypePriv.TypePrivileges(Convert.ToInt32(memEdit.MemberTypeID.ToString()));



			var allpages = bllToolbox.doSQL_List<PagePrivilegeTreeNode>(@"SELECT 
page_id id,
ifnull(page_parent_id,0) link_id,
page_name `name`,
page_desc description,
page_scriptpath scriptpath,
1 is_page
FROM page order by page_id");

			var rootpages = allpages.Where(x => x.link_id == 0).ToList();

			var allprivs = bllToolbox.doSQL_List<PagePrivilegeTreeNode>(@"SELECT 
privilege_id id,
privilege_page_id link_id,
privilege_name name,
privilege_desc description,
0 is_page
FROM privilege");

			var list = new List<PagePrivilegeTreeNode>();
			foreach (var r in rootpages)
			{
				var newr = GetPagePrivilegeTreeNode(r, memEdit, TypePriv, buid, onlyMemberType);

				var childpages = allpages.Where(x => x.link_id == r.id).ToList();
				var rprivs = GetPagePrivilegeTree(allprivs, memEdit, TypePriv, buid, r.id, onlyMemberType);

				var newchildpages = new List<PagePrivilegeTreeNode>();
				foreach (var c in childpages)
				{
					var cp = GetPagePrivilegeTreeNode(c, memEdit, TypePriv, buid, onlyMemberType);
					cp.Children = GetPagePrivilegeTree(allprivs, memEdit, TypePriv, buid, c.id, onlyMemberType);
					newchildpages.Add(cp);
				}
				newr.Children = rprivs.Concat(newchildpages).ToList();
				list.Add(newr);
			}
			return list;
		}

		private PagePrivilegeTreeNode GetPagePrivilegeTreeNode(PagePrivilegeTreeNode r, NeMember memEdit, NEMemberTypePrivilege TypePriv, int buid, bool onlyMemberType)
		{
			r.Label = $"({r.id}) {r.name}";
			r.is_type_priv = TypePriv.AuthenticatedForTypePage(r.id);
			r.is_checked = !onlyMemberType ? memEdit.AuthenticatedForPage(r.id) : r.is_type_priv;



			if (buid != 99)
			{
				r.business_unit = Global.BusinessUnit.GetValue(buid)?.ddl_Name;
				r.members = bllToolbox.doSQL_Array<PrivilegeMembers>(@"Select DISTINCT member_fullname name,
membertype_name, ddl_name from member,memberpage,
business_unit,membertype  where member.member_id = memberpage_member_id 
and member.member_status = 'Active' and member_membertype_id =
membertype_id and business_unit_id = business_unit.id and business_unit.id=@v0 
and memberpage_page_id =@v1  order by name, member_firstname", buid, r.id);
			}
			else
			{
				r.business_unit = "All Business Units";
				r.members = bllToolbox.doSQL_Array<PrivilegeMembers>(@"Select DISTINCT member_fullname name,
membertype_name, ddl_name from member,memberpage,business_unit,
membertype  where member.member_id = memberpage_member_id 
and member.member_status = 'Active' and member_membertype_id = membertype_id
and business_unit_id = business_unit.id and memberpage_page_id =@v0 order by name, member_firstname",
					r.id);
			}
			return r;
		}

		private List<PagePrivilegeTreeNode> GetPagePrivilegeTree(List<PagePrivilegeTreeNode> allprivs, NeMember memEdit, NEMemberTypePrivilege TypePriv, int buid, int pageid, bool onlyMemberType)
		{
			var privs = allprivs.Where(x => x.link_id == pageid).ToList();
			foreach (var p in privs)
			{
				p.Label = $"({p.id}) {p.name}";
				p.is_type_priv = TypePriv.AuthenticatedForTypePrivilege(p.id);

				p.is_checked = !onlyMemberType ? memEdit.AuthenticatedForPrivilege(p.id) : p.is_type_priv;


				if (buid != 99)
				{
					p.business_unit = Global.BusinessUnit.GetValue(buid)?.ddl_Name;
					p.members = bllToolbox.doSQL_Array<PrivilegeMembers>(@"Select 
member_fullname name, 
membertype_name,
ddl_name 
from member,memberpageprivilege,
business_unit,membertype  
where member.member_id = memberpageprivilege_member_id
and member.member_status = 'Active'
and member_membertype_id = membertype_id
and business_unit_id = business_unit.id
and business_unit.id=@v0
and memberpageprivilege_privilege_id =@v1 
order by name, member_firstname", buid, p.id);
				}
				else
				{
					p.business_unit = "All Business Units";
					p.members = bllToolbox.doSQL_Array<PrivilegeMembers>(@"Select 
member_fullname name, 
membertype_name, 
ddl_name 
from member,
memberpageprivilege,
business_unit,membertype 
where member.member_id = memberpageprivilege_member_id 
and member.member_status = 'Active'
and member_membertype_id = membertype_id 
and business_unit_id = business_unit.id and
memberpageprivilege_privilege_id =@v0 order by name, member_firstname",
						p.id);
				}
			}
			return privs;
		}

		public object GetTreeList(int buid, bool onlyMemberType)
		{
			if (buid == 0)
			{
				buid = n1_member.business_unit_id;
			}

			return new
			{
				selected_business_unit_id = buid,
				business_unit_list = VisibleBusinessUnit(),
				global = GetGlobalPrivileges(true),
				report = GetReportEnginePrivileges(),
				page = GetPagePrivileges(buid, onlyMemberType)
			};
		}

		public DataExtra SaveTree(PagePrivilegeTreeNode[] model, int buid, bool onlyMemberType)
		{
			if (buid == 0)
			{
				buid = n1_member.business_unit_id;
			}

			NEUserPrivilege.clear(current_user, n1_member, 1);
			NEUserPage.clear(current_user, n1_member, 1);
			return SaveTreeNode(model, buid, onlyMemberType);
		}

		public DataExtra SaveTreeNode(PagePrivilegeTreeNode[] model, int buid, bool onlyMemberType)
		{
			if (buid == 0)
			{
				buid = n1_member.business_unit_id;
			}

			foreach (var node in model)
			{
				if (node.is_checked)
				{
				    if (node.is_page)
				    {
				        var page_id = node.id;
				        var upa = new NEUserPage
				        {
				            admin = current_user,
				            user = n1_member,
				            user_id = member_id,
				            page_id = page_id,
				            type_id = 1
				        };
				        upa.save();

				        if (node.Children != null && node.Children.Count > 0)
				        {
                            // update the page_id for all children nodes.
				            foreach (var c in node.Children)
				            {
				                c.link_id = upa.id;
				            }
				        }
				    }
				    else
					{
						var upr = new NEUserPrivilege
						{
							admin = current_user,
							user = n1_member,
							user_id = member_id,
							privilege_id = node.id,
							typepage_id = node.link_id,
							type_id = 1
						};
						upr.save();
					}
				}
				if (node.Children != null && node.Children.Count > 0)
				{
					SaveTreeNode(node.Children.ToArray(), buid, onlyMemberType);
				}
			}
			return new DataExtra("Privileges have been saved successfully.", null);
		}

		public DataExtra SaveGlobal(PrivilegeTreeNode[] model)
		{
			NeGlobalPrivilegeLink.clear(member_id, 1);
			foreach (var node in model)
			{
				if (!node.is_checked) continue;
				var gpl = node.link_id == 0
					? new NeGlobalPrivilegeLink()
					: new NeGlobalPrivilegeLink(node.link_id);
				gpl.global_priv_id = node.id;
				gpl.type_id = 1;
				gpl.user_id = member_id;
				gpl.save();
			}
			return new DataExtra("Privileges have been saved successfully.", null);
		}

		public DataExtra SaveReport(PrivilegeTreeNode[] model)
		{
			report_engine.permission.clear(member_id);
			return SaveReportNode(model);
		}

		public DataExtra SaveReportNode(PrivilegeTreeNode[] model)
		{
			foreach (var node in model)
			{
				if (node.is_checked && node.link_id > 0)
				{
					bllToolbox.doSQL_void(@"INSERT INTO r_engine_permission (r_engine_report,member) values (@v0,@v1)", node.link_id, member_id);
				}
				if (node.Children != null && node.Children.Count > 0)
				{
					SaveReportNode(node.Children.ToArray());
				}
			}
			return new DataExtra("Privileges have been saved successfully.", null);
		}
	}
}