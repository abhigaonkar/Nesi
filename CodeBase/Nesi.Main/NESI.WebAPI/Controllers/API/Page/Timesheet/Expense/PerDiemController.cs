using System.Web.Http;
using NESI.BLL.Pages.Timesheet.Expense;
using NESI.DTO.ViewModels.Page.TimeSheet.Expense;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet.Expense
{
	[RoutePrefix("api/Page/Timesheet/Expense/PerDiem")]
	public class PageExpensePerDiemController : TimeSheetControllerBase
	{
		[Route("Profile")]
		public IHttpActionResult GetCurrentUserPerDiemProfile()
		{
			return Ok(new ExpensePerDiem(CurrentUser));
		}

		[Route("Profile/{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetUserPerDiemProfile(int buid)
		{
			return Ok(new ExpensePerDiem(CurrentUser).GetPerDiemProfile(buid));
		}

		

		[HttpPost]
		[Route("Save")]
		public IHttpActionResult SavePerDiem([FromBody] AddExpensePerDiem model)
		{
			return OkD(new ExpensePerDiem(CurrentUser).Save(0, model));
		}
	}
}
