using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Employees;

namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/Review")]
	public class EmployeeReviewController : EmployeesControllerBase
	{
		[HttpGet]
		[Route("List/{memberid}")]
		public IHttpActionResult GetList(int memberid)
		{
			var o = new BLL.Pages.Employees.EmployeeReview(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.ListProfile());
		}

		
		[HttpDelete]
		[Route("List/{memberid}/{review_id}")]
		public IHttpActionResult DeleteList(int memberid,int review_id)
		{
			var o = new BLL.Pages.Employees.EmployeeReview(CurrentUser, memberid);
			if (!o.can_access) return NotFound();
			return Ok(o.DeleteList(review_id));
		}

		[HttpGet]
		[Route("Profile/{memberid}/{review_id}")]
		public IHttpActionResult GetProfile(int memberid,int review_id)
		{
			var o = new BLL.Pages.Employees.EmployeeReview(CurrentUser, memberid, review_id);
			if (!o.can_access) return NotFound();
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("Profile/{memberid}/{review_id}")]
		public IHttpActionResult SaveProfile([FromBody] EmployeeReviewProfile model, int memberid, int review_id)
		{
			var o = new BLL.Pages.Employees.EmployeeReview(CurrentUser, memberid, review_id);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveProfile(model));
		}

		[HttpPost]
		[Route("PrintCopy/{memberid}/{review_id}")]
		public IHttpActionResult printCopy([FromBody] DataInt model, int memberid, int review_id)
		{
			var o = new BLL.Pages.Employees.EmployeeReview(CurrentUser, memberid, review_id);
			if (!o.can_access) return NotFound();
			return Ok(o.PrintCopy());
		}

		[HttpGet]
		[Route("Scores/{memberid}/{review_id}")]
		public IHttpActionResult GetScores(int memberid, int review_id)
		{
			var o = new BLL.Pages.Employees.EmployeeReview(CurrentUser, memberid, review_id);
			if (!o.can_access) return NotFound();
			return Ok(o.GetScores());
		}

		[HttpPost]
		[Route("SaveReview/{memberid}/{review_id}")]
		public IHttpActionResult SaveReview([FromBody] EmployeeReviewItem model, int memberid, int review_id)
		{
			var o = new BLL.Pages.Employees.EmployeeReview(CurrentUser, memberid, review_id);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveReviewItem(model));
		}
	

		[HttpPost]
		[Route("SaveMileStone/{memberid}/{review_id}")]
		public IHttpActionResult SaveMileStone([FromBody] EmployeeReviewMileStone model, int memberid, int review_id)
		{
			var o = new BLL.Pages.Employees.EmployeeReview(CurrentUser, memberid, review_id);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveReviewMileStone(model));
		}
	}
}
