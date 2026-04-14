using System;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeDaysOffVacationRequestGrid : BLLGridBase<DTO.ViewModels.Page.Employees.EmployeeDaysOffVacationRequestGrid>
	{
		public EmployeeDaysOffVacationRequestGrid()
		{

		}
		public EmployeeDaysOffVacationRequestGrid(Employee user) : base(user)
		{

		}
	

		public EmployeeDaysOffVacationRequestGrid(Employee user, BodyParams param, int memberid) : base(user, param, new object[] { memberid })
		{
			this.query = @"
			SELECT
  a.`vacation_id`,
  a.payperiod_id,
  a.date_insert,
  a.date_start,
  a.date_end,
  DATE(a.date_return) date_return,
  b.status,
  c.method,
  a.payment_amount amount,
  a.comments,
  CONCAT(
    '(',
    d.payperiodid,
    ') ',
    DATE(d.startdate),
    ' - ',
    DATE(d.enddate)
  ) payperiod
FROM
  vacation_master a
  LEFT JOIN vacation_status b
    ON a.status = b.status_id
  LEFT JOIN vacation_payment_method c
    ON a.payment_method = c.method_id
  LEFT JOIN payperiods d
    ON a.payperiod_id = d.payperiodid
WHERE a.member_id = @p0
  AND a.type_id = 4
ORDER BY a.vacation_id DESC
			";
		}

	}
}