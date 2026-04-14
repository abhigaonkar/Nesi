using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.BLL.Core.Employee;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet
{
	[RoutePrefix("api/Page/Timesheet/Quote")]
	public class PageTimeSheetQuoteController : TimeSheetControllerBase
	{
		[Route("Customer/{buid}/{userId}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetCustomerList(int buid,int userId)
		{
			var selectedUser = new Employee(userId);
			var list = new BLL.Pages.Timesheet.Timesheet().GetQuoteCustomerList(buid, selectedUser)
				.Select(x => new { Label = x.name, Value = x.id, Bu_Id=x.business_unit_id }).Distinct();
			return Ok(list);
		}
		[Route("Customer/Name/{custId}")]
		public IHttpActionResult GetCustomerNameById(int custId)
		{
			//TODO: implement check if the user can see the customer
			var obj = new BLL.Pages.Timesheet.Timesheet().GetQuoteCustomerById(custId).name;
			return OkD(obj);
		}

		[Route("Quote/{buid}/{custId}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetQuoteQuoteList(int buid,int custId)
		{
			var list = new BLL.Pages.Timesheet.Timesheet().GetQuoteQuoteList(buid, custId);
			return Ok(list);
		}
		[Route("Quote/Name/{quoteId}")]
		public IHttpActionResult GetQuoteQuoteName(int quoteId)
		{
			//TODO: implement check if the user can see the Quote

			var obj = new BLL.Pages.Timesheet.Timesheet().GetQuoteQuoteById(quoteId).Label;
			return OkD(obj);
		}

		/// <summary>
		/// update quote
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPatch]
		[Route("")]
		public IHttpActionResult UpdateQuote([FromBody] DTO.ViewModels.Page.TimeSheet.UpdateTimeSheetQuote model)
		{
			if (!CanSeeUser(model.SelectedBusinessUnitId, model.SelectedUserId)) return NotFound();

			return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).UpdateQuote(CurrentUser, model)));
		}

		/// <summary>
		/// insert Quote
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("")]
		public IHttpActionResult InsertQuote([FromBody] DTO.ViewModels.Page.TimeSheet.InsertTimeSheetQuote model)
		{

			var selectedUser = new Employee(model.SelectedUserId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).SaveQuote(selectedUser, model)));
		}

	}
}
