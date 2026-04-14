using NESI.BLL.Base;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.Timesheet.Expense
{
	public class ExpenseHistory : ExpenseBase
	{
		public DTO.ViewModels.Page.TimeSheet.Expense.ExpenseHistory[] Histories;
		
		public ExpenseHistory(Employee user) : base(user)
		{
			Histories = GetExpenseHistory();
		}


		public DTO.ViewModels.Page.TimeSheet.Expense.ExpenseHistory[] GetExpenseHistory()
		{
			return GetObjectListFromSQL<DTO.ViewModels.Page.TimeSheet.Expense.ExpenseHistory>(
                @"
				SELECT 
				a.id_expense,
					if(a.approved = -1, '', a.approved) approved,
					a.approved approved_int,
					CAST(CONCAT(DATE_FORMAT(c.startdate, '%m/%d/%Y'), ' - ', DATE_FORMAT(c.enddate, '%m/%d/%Y')) AS CHAR) pay_period,
					a.item_text,
					a.id_payperiod,
					a.date_purchased,
					DATE_FORMAT(a.date_requested, '%m/%d/%Y') date_requested,
					URLDECODE(b.name_seller) seller,
					a.amount,
                            					if (a.approved=1,'Expense has been approved', IF(a.approved=0,'Expense has been denied','Expense is waiting to be approved')) tooltip,                                              
						m.member_fullname approved_by ,
				CONCAT(ec.expense_category_id,' - ',ec.Name) gl_name
				FROM 
					expense_reimbursement a 
				LEFT JOIN expense_seller AS b ON a.id_seller = b.id_seller
				LEFT JOIN payperiods AS c ON a.id_payperiod = c.PayperiodID
				LEFT JOIN member AS m ON a.approved_by = m.Member_ID
				INNER JOIN member ON a.id_member = member.Member_ID
				LEFT JOIN business_unit ON member.business_unit_id = business_unit.ID
				LEFT JOIN expense_category ec ON ec.expense_category_id = a.expense_category_id
				WHERE
					a.id_member = @p0
				ORDER BY a.date_requested DESC", CurrentUser.Id
			);
		}
	}
}