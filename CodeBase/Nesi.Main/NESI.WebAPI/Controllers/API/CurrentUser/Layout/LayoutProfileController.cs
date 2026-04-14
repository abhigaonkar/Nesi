using System.Web.Http;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.CurrentUser.Layout;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.CurrentUser.Layout
{
    [RoutePrefix("api/Layout")]
    public class LayoutProfileController : ApiControllerBase
    {

        [Route("Profiles")]
        public IHttpActionResult Get()
        {
            return Ok(CurrentUser.GetLayoutProfiles());
        }

        [Route("Profiles")]
        public IHttpActionResult Put([FromBody] Profile[] profiles)
        {
            return Ok(CovertoJsonData(CurrentUser.SaveProfiles(profiles)));
        }
        [HttpPost]
        [Route("SyncLdap")]
        public IHttpActionResult SyncLdap([FromBody] DataString model)
        {
            return OkD(CurrentUser.SyncLdap(model.Data));
        }
    }
}