using System;
using System.Collections.Generic;
using System.Linq;
using NESI.BLL.Layout.Menu;
using NESI.DTO.Models.Core;

namespace NESI.BLL.Core.User
{
	public partial class User
	{
		public List<MenuItem> GetMenus()
		{
			return new MenuGenerator(
				Pages.Where<Page>(m =>
					m.menu_enabled == true &&
					(this.IsEmployee() ||
					 (m.menu_customer_enabled == true && this.IsCustomer()) ||
					 (m.menu_vendor_enabled == true && this.IsVendor()))).ToList()
			).GetMenus();
		}

		public List<MenuItem> GetMobileMenus()
		{
			return new MenuGenerator(
				Pages.Where<Page>(m =>
					m.menu_mobile_enabled == true &&
					this.IsEmployee()).ToList()
			).GetMobileMenus();
		}
		protected Page GetMenuPageByPageId(int id, bool isMobile)
		{
			if (id == 0)
			{
				if (!isMobile && this.DefaultPageId != 0)
				{
					id = AuthorizePage(this.DefaultPageId) ? this.DefaultPageId : 1;
				}
				else
				{
					id = 1;
				}
			}
			// if not authorized to access the page return null;
			return !AuthorizePage(id) ? null : Pages.First<Page>(p => p.page_id == id);
		}

		public MenuRouter GetMenuRouter(int id, bool isMobile)
		{
			var page = GetMenuPageByPageId(id, isMobile);
			if (page == null) return null;
			if (id == 222 || id == 2)
			{
				BLL.Common.Cache.Global.Refresh();
			}
			var menuitem = MenuGenerator.GetMenuItem(page, true, isMobile);
			if (menuitem.Type == MenuType.Nesi1 || (isMobile && menuitem.Type == MenuType.BlankPage))
			{
				return new MenuRouter()
				{
					Id = menuitem.Id,
					Url = GetOldNESI_URL(menuitem.Url),
					Type = menuitem.Type,
				};
			}
			else
			{
				return new MenuRouter()
				{
					Id = menuitem.Id,
					Type = menuitem.Type,
					Url = menuitem.Url
				};
			}


		}

        /// <summary>
        /// Updated to no longer append creds to url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
	    protected string GetOldNESI_URL(string url) => url;

	}
}
