using System.Web.Http.Controllers;
using NESI.BLL.Core.Employee;
using NESI.BLL.Core.Privileges;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class QuoteFilterAttribute : NesiAutheticationFilterAttribute
	{
		public string Name { get; set; }
		public string Bu_Name { get; set; }
		public string Rev_Name { get; set; }
		public ChekcPrivilegeType Type { get; set; }

		public QuoteFilterAttribute(string name = "quoteId", string revname = "revision", string buname="buid", ChekcPrivilegeType type = ChekcPrivilegeType.Read)
		{
			Name = name;
			Type = type;
			Rev_Name = revname;
			Bu_Name = buname;
		}
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			var user = base.GetCurrentUser(actionContext);
			var check = new NeCheckPrivilege((Employee)user);
			var quote_id = GetIntParamvalue(actionContext, Name);
			var rev = GetIntParamvalue(actionContext, Rev_Name);
			var business_unit_id = GetIntParamvalue(actionContext, Bu_Name);
			return check.CheckQuote(quote_id, rev, business_unit_id, Type);
		}
	}
}