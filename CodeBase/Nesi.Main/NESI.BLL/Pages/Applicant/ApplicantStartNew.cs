using System.Data;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Applicant
{
	public class ApplicantStartNew : ApplicantBase
	{
		public ApplicantStartNew(Employee user) : base(user)
		{

		}

		public ApplicantStartNew(Employee user, int applicantId) : base(user, applicantId)
		{

		}


		public DataExtra GetExistingApplicants(LabelValueString model)
		{
			var name_first = string.IsNullOrEmpty(model.Label) ? "" : model.Label;
			var name_last = string.IsNullOrEmpty(model.Value) ? "" : model.Value;
			//var check_results = "";
			var stuff = "";

			var mem_avoid_mist = "(";
			DataTable _matches_employees;
			var _matches_applicants = bllToolbox.doSQL_dt(@"SELECT applicants.id id,
Concat('*Applicant: ',applicants.firstname,' ',applicants.lastname, ' of ', applicants.city) col_1, 
if (member.Member_ID is not null, (Concat(membertype.membertype_name ,' on ', 
date(applicants.dateentered), ' Status:',applicants.`status`,' - Employee Status: ',member_hrstatus.status)),
(Concat(membertype.membertype_name ,' on ', date(applicants.dateentered), ' Status:',applicants.`status`))) col_2,
member.Member_Status member_status, 
emp_trm.term_date term_date,
emp_trm.reason_roe roe,
member.Member_ID member_id, 
member_hrstatus.status hr_status, 
business_unit.name business_unit_name,
membertype.membertype_name membertype_name,
applicants.dateentered dateentered,
applicants.`status` app_status
FROM applicants 
INNER JOIN business_unit ON applicants.business_unit_id = business_unit.ID 
INNER JOIN membertype ON applicants.membertypeid = membertype.membertype_id
LEFT JOIN member ON applicants.becomes_memberid = member.Member_ID
LEFT JOIN emp_trm ON member.Member_ID = emp_trm.trm_member_id 
left join member_hrstatus on member_hrstatus.id = member.member_hrstatus_id 
where applicants.business_unit_id!=8 and (applicants.firstname like  
CONCAT('%',@v0,'%')  and applicants.lastname like CONCAT('%',@v1,'%')
and (member.member_status is null)) ", name_first, name_last);
			foreach (DataRow dr in _matches_applicants.Rows)
			{
				//		check_results.InnerHtml += string.Format("<tr><td><a href='javascript:boing(&quot;applicants.aspx?id={0}&quot;,&quot;hello&quot;,900,900)'>* Applicant: {1}</a></td><td>{2}</td><td>{3}</td></tr>", dr["id"], dr["name"], "", "");
				//check_results.InnerHtml += string.Format("<tr><td><a href='javascript:gv_applicants.PerformCallback(&quot;open_app|{0}&quot;);'>* Applicant: {1}</a></td><td>{2}</td><td>{3}</td></tr>", dr["id"], dr["name"], "", "");
				//check_results +=
				//	$"<tr><td><a href='javascript:gv_applicants.PerformCallback(&quot;open_app|{dr["id"]}&quot;);'>* Applicant: {dr["app_name"]}</a></td><td>{dr["name"]}</td><td>{dr["membertype_name"]}</td><td>{dr["app_status"]}</td><td>{dr["hr_status"]}</td><td>{dr["reason_roe"]}</td></tr>";

				if (dr["Member_ID"] != null)
				{
					mem_avoid_mist += dr["Member_ID"].ToString();
				}
			}
			if (mem_avoid_mist.Length > 1)
			{
				mem_avoid_mist = mem_avoid_mist.TrimEnd(',') + ")";
				stuff = @"SELECT 
if((date(a.Member_TermDate)>curdate()),
Concat(a.member_fullname, ' of ', a.member_city , '-', membertype.membertype_name ,' from ',business_unit.Name, ' was hired on ', date(a.member_startdate), ' Status:',member_hrstatus.status,' and ', Member_Status),
Concat(a.member_fullname, ' of ', a.member_city , '-', membertype.membertype_name ,' from ',business_unit.Name, ' was hired on ', date(a.member_startdate), ' and their status was terminated on ',a.Member_TermDate)) name,
CONCAT('**Employee: ',a.member_fullname) col_1,
business_unit.name col_2,
a.member_status member_status,
emp_trm.term_date term_date,
emp_trm.reason_roe roe,
a.member_id member_id,
member_hrstatus.status hr_status,
membertype.membertype_name membertype_name,
'' app_status,
'' dateentered
FROM member a 
LEFT JOIN business_unit ON a.business_unit_id = business_unit.id 
left JOIN membertype ON a.member_membertype_id = membertype.membertype_id
LEFT JOIN emp_trm ON a.Member_ID = emp_trm.trm_member_id
left join member_hrstatus on member_hrstatus.id = a.member_hrstatus_id
WHERE a.member_id not in @v2 and ((a.member_firstname like CONCAT('%',@v0,'%') AND a.member_lastname Like CONCAT('%',@v1,'%')) ) order by a.member_id desc LIMIT 10";
				_matches_employees = bllToolbox.doSQL_dt(stuff, name_first, name_last, mem_avoid_mist);

			}
			else
			{
				stuff = @"SELECT 
if((date(a.Member_TermDate)>curdate()),
Concat(a.member_fullname, ' of ', a.member_city ,'-', membertype.membertype_name ,' from ',business_unit.Name, ' was hired on ', date(a.member_startdate), ' Status:',member_hrstatus.status,' and ', Member_Status),
Concat(a.member_fullname, ' of ', a.member_city ,'-', membertype.membertype_name ,' from ',business_unit.Name, ' was hired on ', date(a.member_startdate), ' and their status was terminated on ',a.Member_TermDate)) _stuff,
CONCAT('**Employee: ',a.member_fullname) col_1,
business_unit.name col_2,
a.member_status member_status,
emp_trm.term_date term_date,
emp_trm.reason_roe roe,
a.member_id member_id,
member_hrstatus.status hr_status,
membertype.membertype_name,
'' app_status,
'' dateentered
FROM member a 
LEFT JOIN business_unit ON a.business_unit_id = business_unit.id 
left JOIN membertype ON a.member_membertype_id = membertype.membertype_id
LEFT JOIN emp_trm ON a.Member_ID = emp_trm.trm_member_id
left join member_hrstatus on member_hrstatus.id = a.member_hrstatus_id
WHERE  (a.member_firstname like CONCAT('%',@v0,'%') AND a.member_lastname Like CONCAT('%',@v1,'%')) order by a.member_id desc  LIMIT 10";
				_matches_employees = bllToolbox.doSQL_dt(stuff, name_first, name_last);

			}
			return new DataExtra("success...", new
			{
				employee_list = _matches_employees,
				applicant_list = _matches_applicants
			});
		}

		public object CheckApplicantName(LabelValueString model)
		{
			var name_first = model.Label;
			var name_last = model.Value;
			// Check direct - First Name / Last Name
			var c_first = bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_firstname = @v0  AND member_lastname = @v1 ", name_first, name_last);
			// Check nick - Nickname / Last Name
			var c_nick = bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_nickname = @v0  AND member_lastname = @v1 ", name_first, name_last);
			// Check last - Last Name
			var c_last = bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_lastname like CONCAT(@v0 ,'%') ", name_last);
			var this_type = c_first > 0 ? "first" : c_nick > 0 ? "nick" : c_last > 0 ? "last" : "nomatch";
			var used_column = c_first > 0 ? "first" : c_nick > 0 ? "nick" : c_last > 0 ? "last" : "";
			var _matches = new DataTable();
			if (c_first > 0 || c_nick > 0)
			{
				_matches = bllToolbox.doSQL_dt($@"SELECT a.member_fullname, b.name,
a.member_status FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id
WHERE a.member_{used_column}name = @v0  AND a.member_lastname = @v1  LIMIT 10", name_first, name_last);
			}
			else if (c_last > 0)
			{
				_matches = bllToolbox.doSQL_dt(@"SELECT a.member_fullname,
b.name, a.member_status FROM member a LEFT JOIN business_unit b 
ON a.business_unit_id = b.id WHERE a.member_lastname like CONCAT(@v0 ,'%')  LIMIT 10", name_last);
			}

			return new
			{
				type = this_type,
				used_column,
				list = _matches
			};
		}
	}
}