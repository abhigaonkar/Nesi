using System.Web.Http.Controllers;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class PrivilegeAuthorizationFilterAttribute : NesiFilterAttributeBase
	{
		protected int PrivilegeId { get; set; }
		public PrivilegeAuthorizationFilterAttribute(int privilegeId)
		{
			PrivilegeId = privilegeId;
		}
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			var user = base.GetCurrentUser(actionContext);
			return user != null && user.AuthenticatedForPrivilege(PrivilegeId);
		}
	}
}