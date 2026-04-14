using System.Data;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeEmploymentAgreements : EmployeeEdit
	{
		protected bool isapplicant;
		protected int applicantid;


		public EmployeeEmploymentAgreements(Employee current_user, bool is_applicant, int applicant_id) : base(current_user)
		{
			isapplicant = is_applicant;
			applicantid = applicant_id;
			can_access = true;
		}

		public EmployeeEmploymentAgreements(Employee current_user, int mid) : base(current_user, mid)
		{
			isapplicant = false;
			applicantid = 0;
			can_access = tab_enabled[8];
		}

		public DataTable GetList()
		{
			return bllToolbox.doSQL_dt(@"
SELECT
  a.id,
  a.date,
  a.memberid,
  a.enteredby,
  e.`member_fullname` author,
  a.business_unit_id,
  a.wage,
  a.startdate,
  a.enddate,
  a.vacation_interval_1,
  a.vacation_interval_2,
  a.vacation_interval_3,
  a.vacation_amount_1,
  a.vacation_amount_2,
  a.vacation_amount_3,
  a.gets_vehicle,
  a.gets_phone,
  a.gets_laptop,
  a.has_comp,
  a.comp_details,
  a.notes,
  a.is_salary,
  a.is_signed,
  a.`status`,
  c.member_fullname reports_to,
  b.membertype_name membertype_name,
  a.isapplicant
FROM
  member_offers a
  INNER JOIN membertype AS b
    ON a.membertypeid = b.membertype_id
  LEFT JOIN member c
    ON a.reports_to = c.member_id
  LEFT JOIN member d
    ON a.memberid = d.member_id
  LEFT JOIN member e
    ON a.`enteredby` = e.member_id
WHERE
" +
(!isapplicant ? @" a.memberid = @p0 " : "a.`applicantid`=@p0") +
@"
ORDER BY a.date DESC
			", !isapplicant ? member_id : applicantid);
		}

		public DataExtra Delete(int id)
		{
			var mo = new NeMemberOffer(id);
			if (mo.is_signed)
			{
				return new DataExtra("You can not delete offers that have already been signed", GetList());
			}
			if (mo.status != "In Development" && mo.status != "Closed")
			{
				return new DataExtra("You can not delete offers unless they are of the status 'In Development' or 'Closed'", GetList());
			}
			bllToolbox.doSQL_void(@"Delete from member_offers  where id =@v0", id);
			return new DataExtra("Offer has been deleted successfully.", GetList());
		}
	}
}