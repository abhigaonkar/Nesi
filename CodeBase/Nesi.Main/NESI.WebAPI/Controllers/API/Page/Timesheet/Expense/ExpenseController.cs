using System.Web.Http;
using NESI.BLL.Core.Employee;
using NESI.BLL.Pages.Timesheet.Expense;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.TimeSheet.Expense;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using ExpenseHistory = NESI.BLL.Pages.Timesheet.Expense.ExpenseHistory;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet.Expense
{
	[RoutePrefix("api/Page/Timesheet/Expense")]
	public class PageTimeSheetExpenseController : TimeSheetControllerBase
	{
		[Route("Profile")]
		public IHttpActionResult GetCurrentUserExpenseProfile()
		{
			return Ok(new ExpenseBLL(CurrentUser));
		}
		[Route("Profile/{userId}")]
		public IHttpActionResult GetUserExpenseProfile(int userId)
		{
			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			return Ok(new ExpenseBLL(CurrentUser).GetProfile(selectedUser));
		}

        [Route("Currency/{woNumber}")]
        public IHttpActionResult GetWorkorderCurrency(int woNumber)
        {
            int result = new ExpenseBLL(CurrentUser).GetWorkorderCurrency(woNumber);
            return Ok(result);
        }

        [HttpPost]
		[Route("Seller")]
		public IHttpActionResult GetExpenseSeller([FromBody] DataString searchString)
		{

			return Ok(new ExpenseBLL().GetExpenseSeller(searchString.Data));
		}


		[Route("History")]
		public IHttpActionResult GetExpenseHistory()
		{

			return Ok(new ExpenseHistory(CurrentUser));
		}
		[Route("ReceiptRequired/{userId}/{categoryId}")]
		public IHttpActionResult GetReceiptRequired(int userId, int categoryId)
		{
			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();
			return OkD(new ExpenseBLL().MustProvideReceipt(selectedUser, categoryId));
		}

		[HttpPost]
		[Route("Save")]
		public IHttpActionResult AddExpense([FromBody] AddExpense model)
		{
			var selectedUser = new Employee(model.Id_member);
			if (!CanSeeUser(selectedUser)) return NotFound();

			return OkD(new ExpenseBLL(CurrentUser).SaveExpense(0, selectedUser, model));
		}

		[HttpDelete]
		[Route("Delete/{id}")]
		public IHttpActionResult DeleteExpense(int Id)
		{
			return OkD(new ExpenseBLL(CurrentUser).DeleteExpense(Id));
		}
	}
}
