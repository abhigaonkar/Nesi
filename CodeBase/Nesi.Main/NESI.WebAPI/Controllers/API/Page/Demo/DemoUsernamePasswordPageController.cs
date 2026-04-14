using System;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Page.Demo
{
    [RoutePrefix("api/Page/Demo")]
    public class DemoUsernamePasswordPageController : EmployeeController
    {

        [Route("getUserName")]
        public IHttpActionResult GetUserName() => throw new NotImplementedException();

        [HttpPost]
        [Route("checkpassword")]
        public IHttpActionResult CheckPassword([FromBody] DTO.ViewModels.Page.Demo.CheckUserNamePassword model) => throw new NotImplementedException();
    }
}
