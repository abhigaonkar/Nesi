using System;
using System.Web.Http.Controllers;
using NESI.BLL.Core.Employee;
using NESI.BLL.Core.Privileges;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class VisibleBusinessUnitFilterAttribute : NesiAutheticationFilterAttribute
	{
		public string Name { get; set; }
		public VisibleBusinessUnitFilterAttribute(string name = "buid")
		{
			Name = name;
		}
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			var user = base.GetCurrentUser(actionContext);
			var check = new NeCheckPrivilege((Employee)user);
			var business_unit_id = GetIntParamvalue(actionContext, Name);
			return check.CheckBusinessUnitId(business_unit_id);
		}
	}
}