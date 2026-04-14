using System.Web.Http.Controllers;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class PageAuthorizationFilterAttribute : NesiFilterAttributeBase
	{
		protected int PageId { get; set; }
		public PageAuthorizationFilterAttribute(int pageId)
		{
			PageId = pageId;
		}
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			var user = base.GetCurrentUser(actionContext);
			return user !=null &&  user.AuthorizePage(PageId);
		}
	}
}