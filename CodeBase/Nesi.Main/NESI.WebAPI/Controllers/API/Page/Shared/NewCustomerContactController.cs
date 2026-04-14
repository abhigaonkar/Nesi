using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Page.Shared
{
    [RoutePrefix("api/Shared/Customer/Contact")]
    public class NewCustomerContactController : EmployeeController
    {
        [Route("Titles")]
        public IHttpActionResult GetTitles()
        {
            return Ok(new BLL.Pages.Shared.NewCustomerContact().GetCustomerTitles());
        }

        [HttpPost]
        [Route("New/{custId}/{addId}")]
        public IHttpActionResult NewContact(int custId, int addId, [FromBody] DTO.ViewModels.Page.Shared.NewCustomerContact model)
        {
            return Ok(new BLL.Pages.Shared.NewCustomerContact().AddNew(custId, addId, model));
        }

        [HttpPost]
        [Route("Edit/{Contact_id}")]
        public IHttpActionResult UpdateContact(int Contact_id,[FromBody] DTO.ViewModels.Page.Shared.CustomerContact model)
        {
            model.Contact_ID = Contact_id;
            return Ok(new BLL.Pages.Shared.NewCustomerContact().EditContact(model));
        }

        [Route("Edit/{Contact_id}")]
        public IHttpActionResult GetContact(int Contact_id)
        {

            return Ok(new BLL.Pages.Shared.NewCustomerContact().GetContact(Contact_id));

        }
    }
}
