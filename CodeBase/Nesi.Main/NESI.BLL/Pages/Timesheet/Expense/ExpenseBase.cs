using System;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Page.TimeSheet.Expense;

namespace NESI.BLL.Pages.Timesheet.Expense
{
	public class ExpenseBase : BLLBase
	{

		public Ne2PayPeriod CurrentOpenPayPeriod { get; set; }
		public DateTime MinDate { get; set; }
		public bool allow_unlinked_timesheet { get; set; }

		public ExpenseBase()
		{
			
		}
		public ExpenseBase(Employee user): base(user)
		{
			CurrentOpenPayPeriod = new Ne2PayPeriod(new Ne2PayPeriod().CurrentOpenPayPeriodId());
			MinDate = CurrentOpenPayPeriod.start_date;
			allow_unlinked_timesheet = CurrentUser.BusinessUnit.allow_unlinked_timesheet.GetValueOrDefault();
		}

		public DTO.ViewModels.Page.TimeSheet.Expense.WorkOrder[] GetWorkOrders(int buId)
		{
			return _db.Database.SqlQuery<DTO.ViewModels.Page.TimeSheet.Expense.WorkOrder>(
				@"
					SELECT
						a.woprog_id,
						b.customer_name customer_name,
						a.woprog_bvwo,
						a.woprog_description `description`
					FROM
						woprog a
					LEFT JOIN
					customer b ON a.woprog_customer_id = b.customer_id
					WHERE
						a.woprog_status = 'Open' AND
					a.labor_only=false and a.WOProg_Hold = 0 AND
						a.business_unit_id = @p0 AND
					a.woprog_bvwo != 'Not Entered'
					ORDER BY 
						b.customer_name, a.woprog_description DESC
					",
				buId
			).ToArray();
		}

	}
}