using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class AllowFormHtmlAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			HttpContext.Current.Items["AllowFormHtml"] = true;
		}
	}
}