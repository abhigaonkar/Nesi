using System;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;

namespace NESI.BLL.Layout.Banner
{
	public class Fvr : BLLBase
	{

		public Fvr(Employee user) : base(user)
		{
		}

		public DateTime StartDate =>
			 Global.BusinessUnit.GetValue(CurrentUser.BusinessUnitId.ToString()).Country == "CDN" ? new DateTime(2013, 9, 9) : new DateTime(2013, 11, 13);




		public DTO.ViewModels.CurrentUser.Layout.Fvr[] GetFvrs()
		{
			DTO.ViewModels.CurrentUser.Layout.Fvr[] r;

			if (!CurrentUser.BusinessUnit.fvr_lockout.GetValueOrDefault())
			{
				r = null;
			}

			// How old is the oldest?


			//var fvrsCount = db.Database.SqlQuery<int>(
			//    @"select count(*) from 
			//        (SELECT a.id, a.type, COUNT(b.id) files, 
			//        DATE_ADD(b.dt_insert, INTERVAL 30 DAY) due_by 
			//        FROM member_fvr_hdr a LEFT JOIN member_fvr_dtl b ON a.id = b.member_fvr_hdr_id 
			//        LEFT JOIN member_fvr_history c ON b.id = c.member_fvr_dtl_id 
			//        WHERE b.member_id = @p0  AND a.active = 1 AND b.active = 1 
			//        AND IFNULL(c.confirmed, 0) = 0 GROUP BY id)", _user.Id).FirstOrDefault();

			//if (fvrsCount == 0) return list.ToArray();

			//var oldestFvr = db.Database.SqlQuery<DateTime>(
			//    @"SELECT
			//IFNULL(MIN(b.dt_insert), DATE_SUB(@p1, INTERVAL 1 DAY)) dt
			//    FROM
			//member_fvr_hdr a
			//LEFT JOIN
			//member_fvr_dtl b ON a.id = b.member_fvr_hdr_id
			//LEFT JOIN
			//member_fvr_history c ON b.id = c.member_fvr_dtl_id
			//LEFT JOIN
			//member_fvr_tab d ON b.tab_index = d.tab_index AND a.type = d.type
			//WHERE
			//b.member_id = @p0
			//AND
			//a.active = 1 AND
			//IFNULL(c.confirmed, 0) = 0", _user.Id, StartDate
			//).FirstOrDefault();
			//oldestFvr = oldestFvr <= StartDate ? StartDate : oldestFvr;
			//var days = DateTime.Now.Subtract(oldestFvr).Days;

			var fvrdt = _db.Database.SqlQuery<DTO.ViewModels.CurrentUser.Layout.Fvr>(
				@"SELECT a.id, 
				b.member_id userid,
				a.expire_date DueBy 
			FROM member_fvr_hdr a LEFT JOIN member_fvr_dtl b ON a.id = b.member_fvr_hdr_id 
				LEFT JOIN member_fvr_history c ON b.id = c.member_fvr_dtl_id 
				WHERE b.member_id = @p0  AND a.active = 1 AND b.active = 1 AND IFNULL(c.confirmed, 0) = 0 GROUP BY id",
				CurrentUser.Id).ToList();

			r = fvrdt.ToArray();
			CurrentUser.FvrPassed = this.Passed(r);
			CurrentUser.HasFvr = r?.Length > 0;
			return r;
		}

		public bool Passed(DTO.ViewModels.CurrentUser.Layout.Fvr[] list = null)
		{
			if (list == null)
			{
				list = GetFvrs();
			}
			//Quick fix for if list is null so it doesnt crash
			try
			{
				return list.Count(x => x.DueBy < DateTime.Now) == 0;
			}
			catch //garbage code
			{
				return true;
			}

		}
	}
}