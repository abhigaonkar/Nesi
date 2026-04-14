using System.Linq;
using System.Web;
using System.Web.Http.OData;

namespace NESI.WebAPI.Controllers.Base
{
	
    public class ODataControllerBase : ODataController
	{
		protected string CurrentUserName => RequestContext.Principal.Identity.Name;
		protected int CurrentUserId => int.Parse(getClaims("userId"));
		protected bool CurrentUserIsContact => bool.Parse(getClaims("isContact"));

		private string getClaims(string type)
		{
			return HttpContext.Current.GetOwinContext().Authentication.User.Claims.AsQueryable().FirstOrDefault(c => c.Type == type)?.Value;
		}

	}
}
