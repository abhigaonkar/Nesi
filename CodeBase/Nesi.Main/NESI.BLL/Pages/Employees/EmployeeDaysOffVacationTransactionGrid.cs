using System;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeDaysOffVacationTransactionGrid : BLLGridBase<DTO.ViewModels.Page.Employees.EmployeeDaysOffVacationTransactionGrid>
	{
		public EmployeeDaysOffVacationTransactionGrid()
		{

		}
		public EmployeeDaysOffVacationTransactionGrid(Employee user) : base(user)
		{

		}
	

		public EmployeeDaysOffVacationTransactionGrid(Employee user, BodyParams param, int memberid) : base(user, param, new object[] { memberid })
		{
			this.query = @"
			SELECT
  a.id,
  a.dt,
  a.value_old,
  a.value_new,
  a.value_delta,
  b.member_fullname changed_by,
  a.action
FROM
  vacation_transactions a
  LEFT JOIN member b
    ON a.member_id = b.member_id
WHERE a.member_id = @p0
ORDER BY a.id DESC
			";
		}

	}
}