using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Vendors
{
	[RoutePrefix("api/Page/Vendors/Specialties")]
    public class VendorSpecialtiesController : VendorControllerBase
    {


	    [HttpGet]
	    [Route("{vendor_id}")]
	    public IHttpActionResult GetPhoneProfile(int vendor_id)
	    {
		    var o = new BLL.Pages.Vendors.VendorSpecialties(CurrentUser, vendor_id);
		    return Ok(o.Profile());
	    }

	    [HttpPost]
	    [Route("{vendor_id}")]
	    public IHttpActionResult SavePhone([FromBody]  DTO.ViewModels.Core.DataIdString model, int vendor_id)
	    {
		    var o = new BLL.Pages.Vendors.VendorSpecialties(CurrentUser, vendor_id);
		    return Ok(o.Save(model));
	    }

	    [HttpDelete]
	    [Route("{vendor_id}/{phone_id}")]
	    public IHttpActionResult DeletePhone(int vendor_id,int phone_id)
	    {
		    var o = new BLL.Pages.Vendors.VendorSpecialties(CurrentUser, vendor_id);
		    return Ok(o.Delete(phone_id));
	    }

	}
}
