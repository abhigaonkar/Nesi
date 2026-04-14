using System.Web.Http.Controllers;
using NESI.BLL.Core.Employee;
using NESI.BLL.Core.Privileges;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class WorkOrderFitlerAttribute : NesiAutheticationFilterAttribute
	{
		public string Name { get; set; }
		public string Bu_Name { get; set; }
		public ChekcPrivilegeType Type { get; set; }

		public WorkOrderFitlerAttribute(string name = "str_wo_id", string buname="buid", ChekcPrivilegeType type = ChekcPrivilegeType.Read)
		{
			Name = name;
			Type = type;
			Bu_Name = buname;
		}
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			var user = base.GetCurrentUser(actionContext);
			var check = new NeCheckPrivilege((Employee)user);
			var wo_id = GetIntParamvalue(actionContext, Name);
			var business_unit_id = GetIntParamvalue(actionContext, Bu_Name);
			return check.CheckWorkOrder(wo_id, business_unit_id, Type);
		}
	}
}