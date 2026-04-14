using System.Data;
using System.Linq;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.WorkOrder;

namespace NESI.BLL.Pages.WorkOrder
{
	public class WorkorderWoComments : WorkOrderEdit
	{
		public WorkorderWoComments(Employee user, int buid, string woid) : base(user, buid, woid)
		{

		}

		public object Profile()
		{

			return new
			{
				comments = GetComments(),
				timesheet_comments = GetTimesheetComment()
			};
		}

		private DataTable GetComments()
		{
			if (!this.can_access) return null;

			return bllToolbox.doSQL_dt(@"
SELECT 
	member_fullname name, 
	CAST(woprogcomment_datetime AS char) woprogcomment_datetime, 
	woprogcomment_text
FROM 
	woprogcomment,member,woprog
WHERE  
	woprogcomment_woprog_id = @v0 AND 
	woprogcomment_woprog_id=woprog_id AND 
	woprogcomment_member_id=member_id AND 
	woprogcomment_deleted='F'
order by woprogcomment_datetime desc
", woprog_id);

		}

		private DTO.ViewModels.Page.WorkOrder.WorkOrderWoComment GetTimesheetComment()
		{
			if (!this.can_access) return null;

			var timesheet_comments_list = bllToolbox.doSQL_Array<DTO.ViewModels.Page.WorkOrder.WorkOrderWoComment>(@"
SELECT 
wocomment_id, 
comments, 
CAST(printcomments AS UNSIGNED) printcomments
FROM vwwocomments  WHERE woprog_id =@v0 AND member_id = 0 and comments != '' 

", wo.OrderNumber);
			WorkOrderWoComment timesheet_comments;
			if (timesheet_comments_list.Length > 0)
			{
				timesheet_comments = timesheet_comments_list[0];
			}
			else
			{
				timesheet_comments = new DTO.ViewModels.Page.WorkOrder.WorkOrderWoComment
				{
					comments = "",
					wocomment_id = 0,
					printComments = false
				};
			}
			timesheet_comments.woprog_id = woprog_id;
			timesheet_comments.CustomerName = wo.CustomerName;
			timesheet_comments.OrderNumber = wo.OrderNumber;
			timesheet_comments.Status = wo.Status;
			return timesheet_comments;
		}

		public DataExtra SaveTimesheetComments(DTO.ViewModels.Page.WorkOrder.WorkOrderWoComment model)
		{
			model.woprog_id = woprog_id;
			model.CustomerName = wo.CustomerName;
			model.OrderNumber = wo.OrderNumber;
			model.Status = wo.Status;
			var old = GetTimesheetComment();
			if (old.wocomment_id == 0)
			{
				bllToolbox.doSQL_void(@"
INSERT INTO wocomment 
	(
	workorder_id, 
	comments, 
	personal, 
	printcomments, 
	member_id_audit,
	created_date, 
	modified_date, 
	wocomment_member_id, 
	business_unit_id,
	woprog_id
	) 
VALUES 
	(
	@v0 , 
	@v1 ,
	0,
	@v2 , 
	0 , 
	NOW(),
	NOW(), 
	0, 
	@v3 ,
	@v4 
	)
", model.OrderNumber, model.comments, model.printComments ? 1 : 0, business_unit_id, woprog_id);
			}
			else
			{
				bllToolbox.doSQL_void(@"
UPDATE wocomment SET comments =@v0  ,printcomments =@v1, wocomment_member_id = 0  ,modified_date = NOW() WHERE wocomment_id = @v2
", model.comments, model.printComments ? 1 : 0, old.wocomment_id);
			}
			return new DataExtra("Timesheet comments have been saved successfully.", GetTimesheetComment());
		}

		public DataExtra AddComments(string model)
		{
			const string sql = @"INSERT INTO woprogcomment (
woprogcomment_woprog_id,
woprogcomment_member_id, 
woprogcomment_text,
woprogcomment_x, 
woprogcomment_y, 
woprogcomment_datetime) VALUES(
@v0, @v1, @v2, 5, 5, NOW())";
			bllToolbox.doSQL_void(sql, woprog_id, UserId, model);

			return new DataExtra("Comments has been saved successfully.", GetComments());
		}


	}
}