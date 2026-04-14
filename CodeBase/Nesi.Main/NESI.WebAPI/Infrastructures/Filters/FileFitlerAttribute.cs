using System.Web.Http.Controllers;
using NESI.BLL.Core.Employee;
using NESI.BLL.Core.Privileges;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class FileFitlerAttribute : NesiAutheticationFilterAttribute
	{
		public string Name { get; set; }
		public string Te_Name { get; set; }
		public ChekcPrivilegeType Type { get; set; }

		public FileFitlerAttribute(string name = "filename", string tename="buid", ChekcPrivilegeType type = ChekcPrivilegeType.Read)
		{
			Name = name;
			Type = type;
			Te_Name = tename;
		}
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			var user = base.GetCurrentUser(actionContext);
			var check = new NeCheckPrivilege((Employee)user);
			var filename = GetStringParamvalue(actionContext, Name);
			var te = GetIntParamvalue(actionContext, Te_Name);
			return check.CheckFile(filename, te, Type);
		}
	}
}