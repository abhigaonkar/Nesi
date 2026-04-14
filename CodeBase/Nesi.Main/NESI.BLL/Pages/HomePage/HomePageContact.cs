using NESI.BLL.Base;

namespace NESI.BLL.Pages.HomePage
{
	public class HomePageContact
	{
		public bool show_quote { get; set; }
		public bool show_faq { get; set; }
		public bool show_wo { get; set; }
		public int buid { get; set; }

		public HomePageContact(Core.User.User user)
		{
			show_wo = user.AuthorizePage(12);
			show_quote = user.AuthorizePage(111);
			show_faq = user.AuthorizePage(147);
			buid = user.BusinessUnitId;
		}
	}
}