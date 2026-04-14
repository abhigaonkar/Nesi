using System.Collections.Generic;
using System.Linq;
using NESI.DTO.Models.Core;

namespace NESI.BLL.Layout.Menu
{
    public class MenuGenerator
    {
        private readonly List<Page> _pages;

        private readonly MenuItem _signOutItem = new MenuItem()
        {
            Id = 300,
            MenuId = "Menu_999",
            Label = "SignOut",
            Icon = "fas fa-fw fa-sign-out",
            Url = "/signout",
            Type = MenuType.RouterLink
        };
        private readonly MenuItem _changeToDesktop = new MenuItem()
        {
            Id = 300,
            MenuId = "Menu_990",
            Label = "View Destop",
            Icon = "fas fa-fw fa-desktop",
            Url = "changeToDestop()",
            Type = MenuType.Command
        };

	  
	    public MenuGenerator(List<Page> pages)
        {
            _pages = pages;
        }

        public List<MenuItem> GetMenus()
        {
            var list = new List<MenuItem>();
            foreach (var page in _pages.Where(m => m.menu_parent_id == 0)
                                       .OrderBy(m => m.menu_order )
                                       .ToList())
            {
                var item = GetMenuItem(page);
                item.Items = GetSubMenus(page);
                list.Add(item);
            }
            list.Add(_signOutItem);
            return list;
        }

        public static MenuItem GetMenuItem(Page page, bool isChild = false, bool isMobile = false)
        {
            return new MenuItem()
            {
                Id = page.page_id,
                MenuId = "Menu_" + (isMobile? "M_" : "") + page.page_id,
                Label = page.menu_name,
                Icon = page.menu_icon_class,
                Url = isMobile && !string.IsNullOrEmpty(page.menu_mobile_router) ? page.menu_mobile_router : page.menu_router,
                Type = (MenuType) (isMobile ? page.menu_mobile_router_type : page.menu_type).GetValueOrDefault(), 
                //Badge = GetBadge(page.page_id, out string style),
                //BadgeStyleClass = style,
                Target = isMobile ? null : page.menu_target
            };
        }


	    protected List<MenuItem> GetSubMenus(Page page)
        {
            var list = new List<MenuItem>();
            //  if (!string.IsNullOrEmpty(page.menu_router)) list.Add(GetMenuItem(page, true));

            _pages.Where(m => m.menu_parent_id == page.page_id)
                .OrderBy(m => m.menu_name)
                .ToList()
                .ForEach(
                    m => list.Add(
                        GetMenuItem(m, true)
                    ));

            return list;
        }

        public List<MenuItem> GetMobileMenus()
        {
            var list = _pages.OrderBy(m => m.page_id)
                .ToList()
                .Select(page => GetMenuItem(page, false, true))
                .ToList();
    //        list.Add(_changeToDesktop);
            list.Add(_signOutItem);
            return list;
        }



    }
}