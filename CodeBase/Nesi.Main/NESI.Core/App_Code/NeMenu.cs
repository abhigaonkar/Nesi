using System.Web;
using System.Linq;
using System.Text;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeMenu
	/// </summary>
	public class NeMenu
		{
		public string MenuHTML  { get; set; }
		public string PageDescription { get; set; }
		public NeMenu(NeMember _user, int page_id)
		{
			PageDescription = "";
			MenuHTML = "";
		    return;
			create_menu(_user, page_id, false);
			}
		public NeMenu(NeMember _user, int page_id, bool is_mobile)
		{
			PageDescription = "";
			MenuHTML = "";
		    return;
			create_menu(_user, page_id, is_mobile);
			}
		private void create_menu(NeMember _user, int page_id, bool is_mobile)
			{
			var menu_sb		= new StringBuilder();
			if(!is_mobile)
				{
				menu_sb.Append(@"
				<div id='menu'>");
				}
			if(_user != null && _user.Pages != null && _user.Pages.Count > 0)
				{
				if(is_mobile)
					{
					#region Mobile Menu
					foreach(NePage page in _user.Pages)
						{
						if(page.mobile_ready)
							{
							menu_sb.AppendFormat(@"<div class='o' onclick=""page('{0}');""><img src='/images/icon/icon[{1}].gif' align='absmiddle' />{2}</div>", page.mobile_action, page.mobile_icon, page.Name);
							}
						}
					#endregion Mobile Menu
					}
				else
					{
					#region Full Site Menu
					menu_sb.Append(@"
							<a class='item null' onclick='page_obj.toggle_menu_options(this)' style='text-decoration:underline;cursor:pointer;'>Show All</a>");
					var this_parent_id			= GetParent(page_id, _user);
					foreach (NePage page in _user.Pages)
						{
						if(page.ParentID != 0) continue;
						if(!page.desktop_ready) continue;
						if(!_user.isContact&&_user.hrstatus_id == 1 && page.ID != 1) continue;
						if(_user.isContact && page.ID == 12) continue;

						PageDescription	 = page_id == page.ID ? page.Description : PageDescription;

						var children				= GetChildren(page.ID, page_id,_user, is_mobile);
						menu_sb.Append(string.Format(@"
					<a href='{0}' data-has_children='{3}' target='_top' class='item {1}' alt='{2}' title='{2}'></a>", page.Path, page.Class, HttpUtility.HtmlEncode(page.Description), children != ""));
						var closed				= page.ID == page_id || page.ID == this_parent_id ? "" : "closed"; 
						menu_sb.AppendFormat(@"<div class='sub_pages {0}'>{1}</div>", closed, children);
						}
					#endregion Full Site Menu
					}
				if(!is_mobile)
					{
					menu_sb.Append(@"
							<a href='/default.aspx?sign_out=true' target='_top' class='item signout'></a>");
					}
				else
					{
					menu_sb.Append(@"<div class='o' onclick=""page('logoff');""><img src='/images/icon/icon[logoff].gif' align='absmiddle' />Log Off</div>");
					}
				MenuHTML		= menu_sb.ToString();
				}
			}

		private int GetParent(int _id, NeMember _user)
			{
			foreach (NePage page in _user.Pages)
				{
				if (_id == page.ID)
					{
					return page.ParentID;
					}
				}
			return 0;
			}
		private string GetChildren(int _id, int current_id, NeMember _user, bool is_mobile)
			{
			var children_sb = new StringBuilder();
			var children		= from NePage page in _user.Pages
				where page.ParentID == _id 
				orderby page.Name ascending
				select page;
			foreach(var p in children)
				{
				var show_item			= false;
				if(p.mobile_ready && p.desktop_ready) // Both Interfaces
					{
					show_item		= true;
					}
				else if(p.mobile_ready && !p.desktop_ready && is_mobile) // Only Mobile
					{
					show_item		= true;
					}
				else if(!p.mobile_ready && p.desktop_ready && !is_mobile) // Only Desktop
					{
					show_item		= true;
					}
				if(show_item)
					{
					var highlight		= p.ID == current_id ? "active" : "";
					children_sb.AppendFormat(@"
				<div class='subitem {2}'><a href='{0}'  target='_top'>{1}</a></div>", p.Path, p.Name, highlight);
					}
				}
			return children_sb.ToString();
			}
		}
	}