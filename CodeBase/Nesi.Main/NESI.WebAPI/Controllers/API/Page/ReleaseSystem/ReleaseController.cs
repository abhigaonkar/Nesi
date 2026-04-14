using System.Web.Http;
using NESI.DTO.ViewModels.CurrentUser;
using NESI.DTO.ViewModels.Page.ReleaseSystem;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.ReleaseSystem
{
	[RoutePrefix("api/ReleaseSystem")]
	[PageAuthorizationFilter(166)]
	public class ReleaseController : EmployeeController
    {

	    [HttpPost]
	    [Route("RebootTime")]
	    public IHttpActionResult SetRebootTime([FromBody] RebootTime model)
	    {
		    return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).SetRebootTime(model));

	    }

	    [Route("RebootTime/{host}")]
	    public IHttpActionResult GetRebootTime(string host)
	    {
		    return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).GetRebootTime(host));
		}
//		[Route("SwitchTag/{targetPath}/{tagPath}")]
//	    public IHttpActionResult SwitchTag(string targetPath, string tagPath)
//			{
//			return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).Switch(targetPath, tagPath));
//			}
//	    [Route("CreateTag/")]
//	    public IHttpActionResult CreateTag()
//		    {
//		    return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).CreateTag());
//		    }
//	    [Route("GetTagList/")]
//	    public IHttpActionResult GetTagList()
//		    {
//		    return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).GetTagList());
//		    }
//	    [Route("GetLatestTagPath/")]
//	    public IHttpActionResult GetLatestTagPath(string type)
//		    {
//			return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).GetLatestTag());
//		    }
//	    [Route("SwitchTagToLatest/{type}")]
//	    public IHttpActionResult SwitchBetaTagToLatest(string type)
//		    {
//		    switch (type)
//			    {
//				case "DevBeta": return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).SwitchDevBetaToLatest());
//				case "Beta": return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).SwitchBetaToLatest());
//				case "Live": return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).SwitchLiveToLatest());
//				case "Is": return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).SwitchIsToLatest());
//				case "Print": return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).SwitchPrintToLatest());
//				default: return OkD("Invalid type");
//			    }
//		    return OkD(new BLL.Pages.ReleaseSystem.ReleaseSystem(CurrentUser).GetLatestTag());
//		    }

	}
}
