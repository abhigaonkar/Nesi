using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeesHomeGrid : BLLGridBase<DTO.ViewModels.Page.Employees.EmployeesHomeGrid>
	{
		public EmployeesHomeGrid()
		{

		}

		public EmployeesHomeGrid(Employee user) : base(user)
		{
		}


		public EmployeesHomeGrid(Employee user, BodyParams param) : base(user, param, new object[] { user.Id })
		{
			this.query = @"
					call sp_employee_grid_n2_2(@p0,'{bu_ids}')
			";
		}

		public override DataTable ExtraFilterDataTable(DataTable dt)
		{
			var dt_last_mobile = new ProfileBase(CurrentUser).GetPropertyAllMemberValue("last_mobile_login");
			var bllList = new EmployeeList(CurrentUser);
			var can_view_all_reviews = CurrentUser.AuthenticatedForPrivilege(144);
			var can_edit_other_branches = CurrentUser.AuthenticatedForPrivilege(6);

			foreach (DataRow dr in dt.Rows)
			{

				dr.BeginEdit();
				var bu_id = Convert.ToInt32(dr["bu_id"]);
				var m_id = Convert.ToInt32(dr["memberid"]);
				if (bllList.edit_button_enabled(bu_id, m_id))
				{
					dr["edit"] = "1";

				}
				else
				{
					dr["edit"] = "0";
					dr["day_off"] = "";
				}
				var o = dt_last_mobile.FirstOrDefault(x => x.Value == m_id);
				if (o != null)
				{
					dr["last_mobile_login"] = o.Label;
				}
				else
				{
					dr["last_mobile_login"] = DBNull.Value;
				}

				if (!can_view_all_reviews)
				{
					dr["gen_score"] = "0";
					dr["cr_score"] = "0";
					dr["last_review_date"] = DBNull.Value;
				}
				if (!can_edit_other_branches)
				{
					dr["last_agreement"] = DBNull.Value;
					dr["mo_status"] = "";
				}

				dr.AcceptChanges();
			}


			return dt;
		}


		public DataTable AdvanceSearch(string q)
		{
			var where_clause = new StringBuilder();
			var strip_chars = new Regex("[-(,)\"']");
			q = strip_chars.Replace(q, " ").Trim();
			if (q.Contains(" "))
			{
				var qs = q.Split(' ');
				foreach (var q_i in qs)
				{
					if (!string.IsNullOrEmpty(q_i))
					{
						where_clause.AppendFormat(@"
	(
	a.member_firstname LIKE '%{0}%' OR 
	a.member_lastname LIKE '%{0}%' OR
	a.member_nickname LIKE '%{0}%' OR 
	a.member_id LIKE '%{0}%' OR 
	a.member_phoneextension LIKE '%{0}%' OR
	a.member_necellareacode LIKE '%{0}%' OR 
	a.member_necellphonefirst LIKE '%{0}%' OR 
	a.member_necellphonelast LIKE '%{0}%' OR
	b.name LIKE '%{0}%'
	) AND", Toolbox.AddSlashes(q_i));
					}
				}
			}
			else
			{
				where_clause.AppendFormat(@"
	(
	a.member_firstname LIKE '%{0}%' OR 
	a.member_lastname LIKE '%{0}%' OR
	a.member_nickname LIKE '%{0}%' OR 
	a.member_id LIKE '%{0}%' OR
	a.member_phoneextension LIKE '%{0}%' OR 
	a.member_necellareacode LIKE '%{0}%' OR 
	a.member_necellphonefirst LIKE '%{0}%' OR 
	a.member_necellphonelast LIKE '%{0}%' OR
	b.name LIKE '%{0}%'
	) AND ", Toolbox.AddSlashes(q));
			}
			var n1 = new NeMember(UserId);
			var cansee_homephone = CurrentUser.AuthenticatedForPrivilege(33) || n1.is_backoffice;
			var active_search = cansee_homephone ? "true = true" : " a.member_status = 'Active'";
			var dt = bllToolbox.doSQL_dt($@"
SELECT 
	a.member_id id,
	a.member_fullname name, 
	IF(	c.number IS NOT NULL, 
		CONCAT(	c.number, 
				IF(member_phoneextension != '', 
					CONCAT(' x', member_phoneextension), 
					'')
				), 
		IF(member_phoneextension != '', CONCAT(' x', member_phoneextension), ' No Phone')
		) number, 
	b.ddl_name matches,
	a.member_status status
FROM 
	member a
LEFT JOIN
	business_unit b ON
		a.business_unit_id = b.id
LEFT JOIN
	cellphone_number c ON 
		a.cellphone_number_id = c.id 
WHERE 
	a.business_unit_id != 8 AND find_in_set(a.business_unit_id, @v0) AND
	{where_clause.ToString()} 
	{active_search.ToString()} 
ORDER BY
	a.member_status, b.name,a.member_nickname ,a.member_lastname
", CurrentUser.VisibleBusinessUnits);
			return dt;
		}
	}
}