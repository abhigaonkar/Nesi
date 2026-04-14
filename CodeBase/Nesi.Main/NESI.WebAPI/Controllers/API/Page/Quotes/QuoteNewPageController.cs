using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using nesi.core;
using NESI.DTO.ViewModels.Core;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Quotes
{
    [RoutePrefix("api/Page/Quotes/New")]
    public class QuoteNewPageController : QuoteControllerBase
    {
        [Route("Profile")]
        public IHttpActionResult GetProfile()
        {
            return Ok(new BLL.Pages.Quotes.QuoteNew(CurrentUser));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddNew([FromBody] DTO.ViewModels.Page.Quotes.QuoteNew model)
        {
            return Ok(new BLL.Pages.Quotes.QuoteNew(CurrentUser).Save(model));
        }


        [Route("UserList/{buid}")]
        [VisibleBusinessUnitFilter]
        public IHttpActionResult GetUserListByBuid(int buid)
        {
            return Ok(new BLL.Pages.Quotes.QuoteNew(CurrentUser).GetUserListByBusinessUnit(buid, false));
        }

        [HttpPost]
        [Route("Customer/Filter")]
        public IHttpActionResult GetCustomerListByFilter([FromBody] DataString filter)
        {
            return Ok(new BLL.Pages.Quotes.QuoteNew(CurrentUser).FilterCustomer(filter.Data, filter.Data2, filter.Data3,filter.Data4,filter.Data5));
        }
        [Route("Customer/Address/{id}")]
        public IHttpActionResult GetAddressByCustomerId(int id)
        {
            return Ok(new BLL.Pages.Quotes.QuoteNew(CurrentUser).GetCustomerAddressByCustomerId(id));
        }
        [Route("Customer/Address/Contact/{id}")]
        public IHttpActionResult GetContactByCustomer(int id)
        {
            return Ok(new BLL.Pages.Quotes.QuoteNew(CurrentUser).GetCustomerContactByCustomerId(id));
        }
        [Route("Customer/Quotes/{id}")]
        public IHttpActionResult GetQuotesByCustomerId(int id)
        {
            return Ok(new BLL.Pages.Quotes.QuoteNew(CurrentUser).GetQuotesByCustomerId(id));
        }

        [Route("Customer/BDM/{customerId}/{buid}")]
        public IHttpActionResult GetBDMByCustomerId(int customerId, int buid)
        {
            try
            {
                var bdmData = new BLL.Pages.Quotes.QuoteNew(CurrentUser).GetBDMByCustomerId(customerId, buid);
                return Ok(bdmData);
            }
            catch (Exception ex)
            {
                // Log the error and return a proper error response
                return InternalServerError(ex);
            }
        }

        [Route("Customer/Profile/{id}")]
        public IHttpActionResult GetProfileByCustomerId(int id)
        {
            var q = new BLL.Pages.Quotes.QuoteNew(CurrentUser);
           
            return Ok(
                new
                {
                    Name = q.GetCustomerNameByCustomerId(id),
                    Address = q.GetCustomerAddressByCustomerId(id),
                    Contact = q.GetCustomerContactByCustomerId(id),
                    Quotes = q.GetQuotesByCustomerId(id)
                 
                });
        }


    }
}
