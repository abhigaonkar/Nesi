using System;
using System.Data;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.BLL.Pages.Employees;
using NESI.Common;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Applicant
{
	public class ApplicantHomeGrid : BLLGridBase<DTO.ViewModels.Page.Applicant.ApplicantHomeGrid>
	{
		public bool show_title_panel { get; set; }
		public bool auth_for_edit { get; set; }
		public bool auth_for_edit_all { get; set; }
		public bool is_branch_manager_hr { get; set; }


		public ApplicantHomeGrid()
		{

		}

		public ApplicantHomeGrid(Employee user) : base(user)
		{

		}
		public ApplicantHomeGrid(Employee user, BodyParams param) : base(user, param, new object[] { user.Id, user.BusinessUnitId })
		{
			show_title_panel = CurrentUser.AuthenticatedForPrivilege(5);
			auth_for_edit = CurrentUser.AuthenticatedForPrivilege(32);
			auth_for_edit_all = CurrentUser.AuthenticatedForPrivilege(152);
			is_branch_manager_hr = CurrentUser.AuthenticatedForPrivilege(33);
            
			var where = "";
			if (param.queryparam?.Length == 0 || param.queryparam?[0].value.ToString() == "0")
			{
				where = " and (`status` !='Hired' AND `status` !='Deleted' AND `status` !='Rejected' )";
			}
			var sql = @"SELECT DISTINCT 
applicants.id,
applicants.addedbymemberid,
m.`member_fullname` addedbyname,
CONCAT(applicants.firstname,' ',applicants.lastname) AS `name`,
applicants.dateentered,
applicants.`status`,
applicants.membertypeid,
mt.membertype_name,
applicants.business_unit_id,
b.ddl_name business_unit_name,
applicants.cellphone,
applicants.becomes_memberid,
applicants.email,
IFNULL((SELECT `status` FROM member_offers WHERE member_offers.applicantid = applicants.id ORDER BY member_offers.id DESC LIMIT 1),'Unknown') offer_status,
applicants.notes
FROM
applicants 
INNER JOIN member m ON applicants.`addedbymemberid`=m.`Member_ID`
INNER JOIN business_unit b ON applicants.`business_unit_id`=b.id
INNER JOIN membertype mt ON applicants.`membertypeid`=mt.membertype_id
WHERE find_in_set(applicants.business_unit_id,'{bu_ids}') ";

			var groupby = @"GROUP BY applicants.id
			ORDER BY applicants.id";

			if (auth_for_edit_all)
			{
				this.query = $@"{sql} {where} {groupby}";
			}
			else if (is_branch_manager_hr)
			{
				this.query = $@"{sql} 
				AND applicants.business_unit_id = @p1 OR applicants.addedbymemberid = @p0
				{where}
				{groupby}";
			}
			else
			{
				this.query = $@"{sql} 
				AND applicants.addedbymemberid=@p0 
				{where}
				{groupby}";
			}
		}

		public string Delete(int id)
		{
			bllToolbox.doSQL_void(@"Delete from applicants  where id =@v0", id);
			bllToolbox.doSQL_void(@"Delete from member_offers  where isapplicant = 1 and applicantid =@v0", id);
			this.ClearCache(new object[] { UserId });
			return "Applicant has been deleted successfully.";
		}

		public string Update(string field, string value, int id)
		{
			var sql = "UPDATE applicants SET {0}=@p0 WHERE id=@p1";
			switch (field)
			{
				case "status":
					sql = "UPDATE applicants SET status=@p0 WHERE id=@p1";
					break;
				case "notes":
					sql = "UPDATE applicants SET notes=@p0 WHERE id=@p1";
					break;
				default:
					return "Error operation";
			}
			bllToolbox.doSQL_void(sql, value, id);
			return "Applicant has been updated successfully.";
		}
	}
}