using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Quotes
{
	[PageAuthorizationFilter(65)]
	public class QuoteControllerBase : EmployeeController
    {
    }
}
