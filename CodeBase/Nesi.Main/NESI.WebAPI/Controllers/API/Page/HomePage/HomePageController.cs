using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Page.HomePage
{
	[RoutePrefix("api/Page/HomePage")]
	public class HomePageController : EmployeeController
	{
		[Route("Profile")]
		public IHttpActionResult GetProfile()
		{
			var o = new BLL.Pages.HomePage.HomePageBase(CurrentUser);
			return Ok(o.GetProfile());
		}

		[Route("ToDo")]
		public IHttpActionResult GetToDoList()
		{
			var o = new BLL.Pages.HomePage.HomePageBase(CurrentUser);
			return Ok(o.GetTodoList());
		}

		[Route("AutoBingo")]
		public IHttpActionResult GetAutoBingoList()
		{
		    var o = new BLL.Pages.HomePage.HomePageBase(CurrentUser);
            return Ok(o.GetAutoBingoList());
		}

		[Route("InvoiceService")]
		public IHttpActionResult GetInvoiceService()
		{
			var o = new BLL.Pages.HomePage.HomePageBase(CurrentUser);
			return Ok(o.GetInvoiceServiceRunTime());
		}

		[Route("SlowPage")]
		public IHttpActionResult GetSlowPage()
		{
			var o = new BLL.Pages.HomePage.HomePageBase(CurrentUser);
			return Ok(o.GetSlowPage());
		}

		[Route("WoStatus")]
		public IHttpActionResult GetWoStatus()
		{
			var o = new BLL.Pages.HomePage.HomePageBase(CurrentUser);
			return Ok(o.GetwoStatus());
		}

		[Route("WoStatusBranch")]
		public IHttpActionResult GetwoStatus_branch()
		{
			var o = new BLL.Pages.HomePage.HomePageBase(CurrentUser);
			return Ok(o.GetwoStatus_branch());
		}

		[Route("Quotes")]
		public IHttpActionResult Getquotes()
		{
			var o = new BLL.Pages.HomePage.HomePageBase(CurrentUser);
			return Ok(o.Getquotes());
		}

		[Route("BranchQuotes")]
		public IHttpActionResult Getbranchquotes()
		{
			var o = new BLL.Pages.HomePage.HomePageBase(CurrentUser);
			return Ok(o.Getbranchquotes());
		}


		[Route("WoWaitingPM")]
		public IHttpActionResult Getwowaitingpm()
		{
			var o = new BLL.Pages.HomePage.HomePageBase(CurrentUser);
			return Ok(o.Getwowaitingpm());
		}

		[Route("Branchpo")]
		public IHttpActionResult Getbranchpo()
		{
			var o = new BLL.Pages.HomePage.HomePageBase(CurrentUser);
			return Ok(o.Getbranchpo());
		}
	}
}
