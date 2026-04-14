using NESI.BLL.Base;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.Employee;
using NESI.BLL.Core.Member;
using NESI.BLL.Core.User;
using NESI.WebAPI.Infrastructures.Filters;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
// ReSharper disable PossibleInvalidCastException

namespace NESI.WebAPI.Controllers.Base
{
    [NesiAutheticationFilter]
	public class ApiControllerBase : ApiController
	{
		protected Guid UId => Guid.Parse(RequestContext.Principal.Identity.Name);
		protected User OriginalUser => Global.OriginalUser.GetValue(UId);
		protected bool OriginalUserIsContact => bool.Parse(getClaims("isContact"));
		protected User CurrentUser => Global.OnlineUser.GetValue(UId);
		protected string CurrentUserName => CurrentUser.UserName;
		protected int CurrentUserId => CurrentUser.Id;

		protected Employee CurrentEmployee
		{
			get
			{
			    var currentUser = CurrentUser; //Avoid un-necessary calls
			    if (currentUser is Employee employee) return employee;

			    var e = new Employee(0);
			    BLLBase.MapperFrom(e, currentUser);

			    switch (currentUser)
			    {
			        case Customer _:
			            e.ExtraType = "customer";
                        break;
			        case Vendor _:
			            e.ExtraType = "vendor";
                        break;
			        case Contact _:
			            e.ExtraType = "contact";
                        break;
			        case null:
			            break;
                    default:
                        e.ExtraType = "user";
                        System.Diagnostics.Debug.Assert(false, $"Unknown user type {currentUser.GetType().Name}");
			            break;
                   
                }
			    return e;
			}
		}

		private string getClaims(string type)
		{
			return HttpContext.Current.GetOwinContext().Authentication.User.Claims.AsQueryable().FirstOrDefault(c => c.Type == type)?.Value;
		}

		public static object CovertoJsonData(object value)
		{
			return ControllerHelper.ConvertoJsonData(value);
		}

		public IHttpActionResult OkD(object obj)
		{
			return Ok(ControllerHelper.ConvertoJsonData(obj));
		}
	}

}

