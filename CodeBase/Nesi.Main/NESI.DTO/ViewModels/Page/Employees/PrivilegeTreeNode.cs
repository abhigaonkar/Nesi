using System.Collections.Generic;
using NESI.DTO.ViewModels.Core;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class PrivilegeTreeNode : TreeNode
	{
		public int id { get; set; }
		public bool is_checked { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public int link_id { get; set; }
		public new List<PrivilegeTreeNode> Children { get; set; }
	}


	public class PrivilegeMembers
	{
		public string name { get; set; }
		public string membertype_name { get; set; }
		public string ddl_name { get; set; }
	}

	public class PagePrivilegeTreeNode : PrivilegeTreeNode
	{
		public bool is_type_priv { get; set; }
		public string business_unit { get; set; }
		public string scriptpath { get; set; }
		public bool is_page { get; set; }
		public PrivilegeMembers[] members { get; set; }
		public new List<PagePrivilegeTreeNode> Children { get; set; }
	}
}