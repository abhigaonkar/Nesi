using System;
using System.Collections.Generic;
using System.Data;
using core;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Common.Shared;
using NESI.BLL.Core.Employee;
using NESI.Common.Models;

namespace NESI.BLL.Pages.HomePage
{
	public class HomePageBase : BLLBase
	{
		protected NeMember myMember;

		public HomePageBase(Employee employee) : base(employee)
		{
			myMember = new NeMember(UserId);
		}

		public object GetProfile()
		{
			return new
			{
				is_developer = CurrentUser.isDeveloper,
				show_branchpo = myMember.MemberTypeID == OpsMemberTypes.BranchManager || myMember.MemberTypeID == OpsMemberTypes.RegionalManager,
				show_pane= new List<int>(new[]
											{
											OpsMemberTypes.ProjectManager,
											OpsMemberTypes.BranchManager,
											OpsMemberTypes.DepartmentManager,
											OpsMemberTypes.Engineer,
											OpsMemberTypes.AutomationProjectManager,
											OpsMemberTypes.PlcProgrammer,
											OpsMemberTypes.RegionalManager,
											OpsMemberTypes.ServiceCoordinator,
											OpsMemberTypes.GeneralManager,
											OpsMemberTypes.DirectorOfBranchDevelopment,
											OpsMemberTypes.DirectorOfOperations
											}).Contains(myMember.MemberTypeID)
			};
		}

		

		public DataTable GetTodoList()
		{
			var dt_final = new DataTable();
			dt_final.Columns.Add(new DataColumn("link"));
			dt_final.Columns.Add(new DataColumn("description"));
			dt_final.Columns.Add(new DataColumn("overdue"));
			dt_final.Columns.Add(new DataColumn("star"));
			dt_final.Columns.Add(new DataColumn("link2"));
			dt_final.Columns.Add(new DataColumn("n2_page"));
			dt_final.Columns.Add(new DataColumn("n2_params"));


			TodoList.getToDoList(dt_final, myMember);

			return dt_final;
		}


		public DataTable GetAutoBingoList()
		{
		return Toolbox.doSQL_dt(@"CALL UPDATE_BINGO(FALSE)", new object[]{});
		}


		public DataTable GetInvoiceServiceRunTime()
		{
			var dt = Toolbox.doSQL_dt(@" SELECT FUN_time(request_start) Last_Start, FUN_TIME(request_end) as Last_Finished , TIMESTAMPDIFF(MINUTE, request_end, NOW()) as Last_Ran, TIMESTAMPDIFF(MINUTE, request_start, request_end) as ran_for, (IFNULL(request_end, request_start) < now() - interval 55 minute) as isBroken FROM log_page  where dt > curdate() - interval 1 day AND host = 'is' AND member_id = 1316 AND url = '/invoice_service/invoice_service.aspx' order by request_start desc limit 0 ", null);
			return dt;
		}

		public DataTable GetSlowPage()
		{
			return bllToolbox.doSQL_dt(@"SELECT
			  url,
			  COUNT(id) count_id,
			  SUM(
				IF(
				  TIMEDIFF(
					IFNULL(render_end, request_end),
					request_start
				  ) > 3,
				  1,
				  0
				)
			  ) count_over_three,
			  AVG(
				TIMEDIFF(
				  IFNULL(render_end, request_end),
				  request_start
				)
			  ) avg_time,
			  SUM(
				IF(
				  TIMEDIFF(
					IFNULL(render_end, request_end),
					request_start
				  ) > 3,
				  TIMEDIFF(
					IFNULL(render_end, request_end),
					request_start
				  ) - 3,
				  0
				)
			  ) t_over_three,
			  SUM(
				TIMEDIFF(
				  IFNULL(render_end, request_end),
				  request_start
				)
			  ) t_total
			FROM
			  log_page
			WHERE dt > (CURDATE() - INTERVAL 7 DAY)
			  AND url <> '/invoice_service/invoice_service.aspx'
			  AND HOST != 'localhost'
			  AND HOST != 'devbeta-api.nesi.ca'
			  AND HOST != 'alpha-api.nesi.ca'
			  AND HOST != 'api.nesi.ca'
			GROUP BY url
			ORDER BY t_over_three DESC
			LIMIT 12");
		}


		public DataTable GetwoStatus()
		{
			return bllToolbox.doSQL_dt(@"
				SELECT 
				woprog_status.woprog_status_status as status, 
				Count(woprog.WOProg_ID) as count,
				datediff(curdate(),ifnull(MIN(woprog.woprog_opendatetime),curdate())) as age
				FROM woprog_status Left Join woprog ON woprog_status.woprog_status_status = woprog.WOProg_Status
				and woprog.WOProg_Status <>  'Invoiced' AND woprog.WOProg_Status <> 'Open' AND
				woprog.WOProg_PM_MemberID = @v0 AND woprog.business_unit_id = @v1 GROUP BY woprog_status.woprog_status_status  order by woprog_status.woprog_status_id
				", myMember.id, myMember.business_unit_id);
		}

		public DataTable GetwoStatus_branch()
		{
			return bllToolbox.doSQL_dt(@"
									SELECT 
						a.woprog_status_status status, 
						COUNT(b.WOProg_ID) count,
						DATEDIFF(CURDATE(),IFNULL(MIN(b.woprog_opendatetime),CURDATE())) age
					FROM 
						woprog_status a
					LEFT JOIN 
						woprog b ON 
							a.woprog_status_status = b.woprog_status AND 
							b.business_unit_id = @v0 
					WHERE 
						b.woprog_status NOT IN ('Invoiced','Open','Deleted') 
					GROUP BY 
						status 
					order by 
						b.woprog_status
				", myMember.business_unit_id);
		}

		public DataTable Getquotes()
		{
			return bllToolbox.doSQL_dt(@"
									SELECT 
	quote_status.status as status, 
	Count(quote_master.quote_ID) as count,
	datediff(curdate(),ifnull(MIN(quote_master.last_print_date),curdate())) as Age
FROM quote_status Left Join quote_master ON quote_status.id = quote_master.Status_id and quote_status.id <= 5 
AND quote_master.quoted_by = @v0 GROUP BY quote_status.id
				", myMember.id);
		}

		public DataTable Getbranchquotes()
		{
			return bllToolbox.doSQL_dt(@"
						SELECT 
	member.member_fullname as status, 
	Count(quote_master.quote_id) AS count, 
	datediff(curdate(),ifnull(MIN(quote_master.last_print_date),curdate())) AS Age 
FROM 
	quote_status 
LEFT JOIN 
	quote_master ON quote_status.id = quote_master.status_id AND quote_master.business_unit_id = @v0 and quote_status.id = 4 
INNER JOIN 
	member ON quote_master.quoted_by = member.Member_ID 
GROUP BY 
	quoted_by,quote_status.id 

order by 
	member.member_fullname
				", myMember.business_unit_id);
		}

		public DataTable Getwowaitingpm()
		{
			return bllToolbox.doSQL_dt(@"
						SELECT member_fullname as status, 
Count(DISTINCT woprog.woprog_id) AS count, datediff(curdate(),ifnull(MIN(woprog.woprog_opendatetime),curdate())) AS Age
FROM woprog_status LEFT JOIN woprog ON woprog_status.woprog_status_status= woprog.WOProg_Status
AND woprog.business_unit_id = @v0 and woprog_status.woprog_status_status = 'Waiting PM Approval'  INNER JOIN member ON woprog.WOProg_PM_MemberID = member.Member_ID
			                            GROUP BY woprog.WOProg_PM_MemberID,woprog_status.woprog_status_status
				", myMember.business_unit_id);
		}

		public DataTable Getbranchpo()
		{
			return bllToolbox.doSQL_dt(@"
						SELECT 
	a.status_type as status, 
	Count(b.poprog_id) as count,
	datediff(curdate(),ifnull(MIN(b.poprog_cutdate),curdate())) as Age 
FROM 
	poprog_status a
Left Join poprog_header b ON a.poprog_status_id = b.poprog_status and a.poprog_status_id NOT IN (4,6,7,8)
WHERE 
	b.business_unit_id = @v0 AND
	b.nesi_cut_po = false AND
	b.poprog_order_description NOT LIKE '%NESI%'
GROUP BY 
	a.poprog_status_id 
				", myMember.business_unit_id);
		}
	}
}