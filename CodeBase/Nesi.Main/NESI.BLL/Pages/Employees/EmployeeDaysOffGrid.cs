using System;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeDaysOffGrid : BLLGridBase<DTO.ViewModels.Page.Employees.EmployeeDaysOffGrid>
	{
		public EmployeeDaysOffGrid()
		{

		}
		public EmployeeDaysOffGrid(Employee user) : base(user)
		{

		}
		public LabelValueInt[] GetRequestTypeList()
		{
			return new[]
			{
				 new LabelValueInt { Label = "Select Request Type", Value=0},
				new LabelValueInt { Label = "Phone", Value=1},
				new LabelValueInt { Label = "Online", Value=2},
				new LabelValueInt { Label = "Verbal", Value=3},
				new LabelValueInt { Label = "Form", Value=4},
			};
		}


		public LabelValueInt[] GetDayoffTypeList()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT id value,type label FROM vacation_type WHERE id != 4");
		}

		public EmployeeDaysOffGrid(Employee user, BodyParams param, int memberid) : base(user, param, new object[] { memberid })
		{
			this.query = @"
			SELECT 
	a.vacation_id id, 
	d.ddl_name business_unit,
	b.member_fullname employee,
	CAST(a.member_id AS SIGNED) member_id, 
	IFNULL(b.member_status, 'Not Active') member_status, 
	a.type_id,
	a.requesttype_id, 
	a.date_insert date_requested, 
	a.date_start,
	a.date_end,
	a.comments, 
	b.member_status status,
	e.member_fullname requestedby,
	c.type leavetype,
	f.type requesttype,
	a.payperiod_id,
	a.business_unit_id
FROM 
	vacation_master a 
INNER JOIN 
	member b on 
		a.member_id = b.member_id  
INNER JOIN
	vacation_type c ON 
		a.type_id = c.id 
INNER JOIN
	business_unit d ON 
		b.business_unit_id = d.id
INNER JOIN
	member e ON
		a.create_member_id = e.member_id
INNER JOIN
	(
	SELECT 1 id, 'Phone' type 
	UNION ALL
	SELECT 2 id, 'Online' type 
	UNION ALL
	SELECT 3 id, 'Verbal' type 
	UNION ALL
	SELECT 4 id, 'Form' type 
	) f ON a.requesttype_id = f.id
WHERE 
	a.type_id != 4 AND 
	b.member_id = @p0
ORDER BY
	d.name ASC,
	b.member_lastname ASC,
	b.member_fullname ASC,
	a.vacation_id DESC
			";
		}

		public DataExtra Save(DTO.ViewModels.Page.Employees.EmployeeDaysOffGrid model)
		{
			var dd = model.id == 0 ? new NeMemberDaysOff() : new NeMemberDaysOff(model.id);
			dd.member_id = model.member_id;
			dd.member_id_audit = UserId;
			dd.active = true;
			dd.requested_by = UserId;

			dd.member_id = model.member_id;
			var member = new NeMember(model.member_id);
			dd.business_unit_id = member.business_unit_id;
			var msg = "";
			if (model.date_start.DayOfWeek == DayOfWeek.Saturday || model.date_start.DayOfWeek == DayOfWeek.Sunday)
			{
				msg += ("Cannot have a start date be on a Saturday or Sunday\n");
			}
			if (model.date_end.DayOfWeek == DayOfWeek.Saturday || model.date_end.DayOfWeek == DayOfWeek.Sunday)
			{
				msg += ("Cannot have an end date be on a Saturday or Sunday\n");
			}
			if (model.date_end.Date < model.date_start.Date)
			{
				msg += ("The start date needs to be THE SAME or BEFORE the end date\n");
			}

			dd.date_requested_start = Toolbox.MySQL_longdt(model.date_start);
			dd.date_requested_end = Toolbox.MySQL_longdt(model.date_end);

			var c = bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM vacation_master 
WHERE date_start BETWEEN @v0 AND @v1 AND member_id = @v2 AND vacation_id != @v3",
	Toolbox.MySQL_shortdt(model.date_start), Toolbox.MySQL_shortdt(model.date_end), dd.member_id, dd.id);
			if (c > 0)
			{
				msg += ("There are already day(s) off for this user during the selected period(s) - OTHER THAN THIS REQUEST - please review existing requests before proceeding.\n");
			}
			if (model.requesttype_id == 0)
			{
				msg += ("Please select a request type\n");
			}
			else
			{
				dd.requesttype_id = model.requesttype_id;
			}
			if (model.type_id == 0)
			{
				msg += ("Please select a leave type\n");
			}
			else
			{
				dd.leavetype_id = model.type_id;
			}
			dd.comments = model.comments;
			if (msg == "")
			{
				dd.save();
				ClearCache(new object[] { model.member_id });
			}
			return new DataExtra()
			{
				Data = msg == "" ? "Days Off has been saved successfully." : msg,
				Extra = null
			};
		}

		public DataExtra Delete(int modelId, int mid)
		{
			var msg = "";
			var dd = new NeMemberDaysOff(modelId);
			try
			{
				dd.delete();
				ClearCache(new object[] { mid });
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				msg = e.Message;
			}
			return new DataExtra()
			{
				Data = msg == "" ? "Days Off has been saved successfully." : msg,
				Extra = null
			};
		}
	}
}