using System;
using System.Collections.Generic;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.BLL.Layout.Menu;

namespace NESI.BLL.Layout.Banner
{
	public partial class ToDo: BLLBase
	{

		public ToDo(Employee user): base(user)
		{
		}

		public List<MenuItem> BadageMenus()
		{
			var menus = CurrentUser.GetMenus();
			var todos = GetToDo();
			foreach (var menu in menus)
			{
				if (menu.Items != null && menu.Items.Count > 0)
				{
					foreach (var submenu in menu.Items)
					{
						submenu.Badge = todos.Count(m => m.PageId == submenu.Id.ToString());
					}
					menu.Badge = (todos.Count(m => m.PageId == menu.Id.ToString()) +
					              menu.Items.Sum(x => Convert.ToInt32(x.Badge)));
				}
				else
				{
					menu.Badge = todos.Count(m => m.PageId == menu.Id.ToString());
				}
			}

			return menus;
		}

	}
}


