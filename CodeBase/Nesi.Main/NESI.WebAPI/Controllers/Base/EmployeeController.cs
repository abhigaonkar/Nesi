using NESI.BLL.Core;
using NESI.BLL.Core.Employee;
using NESI.WebAPI.Infrastructures.Filters;
using System;

namespace NESI.WebAPI.Controllers.Base
{
	[NesiEmployeeFilter]
	public class EmployeeController : ApiControllerBase
	{
		protected new Employee CurrentUser => (Employee)base.CurrentUser;
		protected new Employee OriginalUser => (Employee)base.OriginalUser;


	
		protected bool CanSeeUser(Employee user)
		{
			return CanSeeUser(user.BusinessUnitId, user.Id);
		}

		// if currentUser can not see the business unit where the userid belongs,
		// or current user can not switch businessunit
		protected bool CanSeeUser(int buid, int userId)
		{
			return CurrentUser.IsVisibleBusinessUnitId(buid)
			       && CurrentUser.AuthenticatedForPrivilege(42)
			       || CurrentUser.Id == userId;
		}
		protected bool CanSeeBusinessUnit(int buid)
		{
			return CurrentUser.IsVisibleBusinessUnitId(buid);
		}

        //
        // For but 1973: Timesheet entry on a wrong date.(https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1973/)
        //
        protected bool SelectDateValid(DTO.ViewModels.Page.TimeSheet.InsertTimeSheetWorkOrder model)
        {
            var CanPickAnyDate = CurrentUser.AuthenticatedForPrivilege(29);
            var CurrentPayPeriod = new Ne2PayPeriod(new Ne2Payroll().Working_pay_period());

            var MinDate = !CanPickAnyDate
                ? DateTime.Today.AddDays(-1)
                 : CurrentPayPeriod.start_date;

            var MaxDate = DateTime.Today;

            if (model.LocalDate.Date >= MinDate && model.LocalDate.Date <= MaxDate)
            {
                return true;
            }

            return false;
        }
	}
}
