using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NESI.BLL.Base;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;

// ReSharper disable InconsistentNaming

namespace NESI.BLL.Core
{
	public class Ne2PayPeriod: BLLBase
	{
		public int payperiodID { get; set; }
		public int id { get; set; }
		public string StartDate { get; set; }
		public string Enddate { get; set; }
		public DateTime start_date { get; set; }
		public DateTime end_date { get; set; }
		public bool is_complete { get; set; }
		public bool completed { get; set; }
		public bool straight_eight_stat => payperiodID >= 318;


		public DTO.ViewModels.Core.PayPeriod[] LoadPayPeriodList()
		{
			return _db.Database.SqlQuery<PayPeriod>(@"SELECT payperiodid,
				CONCAT(DATE(startdate), ' - ', DATE(enddate)) PayDate,
				startDate, EndDate
					FROM payperiods  WHERE completed = 1 
							ORDER BY startdate DESC").ToArray();
		}
		public int CurrentOpenPayPeriodId()
		{
			return _db.Database.SqlQuery<int>(@"SELECT IFNULL(MIN(payperiodid), 0) FROM payperiods WHERE completed = 0 ORDER BY payperiodid ASC limit 1").FirstOrDefault();
		}
		public int CurrentPayPeriodId()
		{
			return _db.Database.SqlQuery<int>(@"SELECT IFNULL(MIN(payperiodid), 0) FROM payperiods WHERE startdate < NOW() AND enddate > NOW() limit 1").FirstOrDefault();
		}
		public int CurrentPayPeriodId(DateTime d)
		{
			return _db.Database.SqlQuery<int>(@"SELECT IFNULL(MAX(payperiodid), 0) FROM payperiods WHERE startdate <= @p0 AND enddate >= @p0 limit 1", d).FirstOrDefault();
		}

		public DTO.ViewModels.Core.PayPeriod PayPeriod()
		{
			return _db.Database.SqlQuery<DTO.ViewModels.Core.PayPeriod>(
				@"SELECT payperiodid, 
				CONCAT(DATE(startdate), ' - ', DATE(enddate)) PayDate,
				startDate, EndDate
				FROM payperiods WHERE startdate <= CURDATE() and enddate > CURDATE()").FirstOrDefault();
		}

		public Ne2PayPeriod()
		{
			
		}
		public Ne2PayPeriod(int _id)
		{
			Init(_id);
		}
		private void Init(int Id)
		{
			var entity = (from e in _db.payperiods
						  where e.PayperiodID == Id
						  select e).FirstOrDefault();
			if (entity == null) return;

			id = entity.PayperiodID;
			payperiodID = id;
			start_date = entity.StartDate.GetValueOrDefault();
			StartDate = start_date.ToString("yyyy-MM-dd");
			end_date = entity.EndDate.GetValueOrDefault();
			Enddate = end_date.ToString("yyyy-MM-dd");
			completed = Convert.ToBoolean(entity.completed.GetValueOrDefault());
			is_complete = completed;
		}
		public int get_payperiod_id(DateTime _d)
		{
			return _db.Database.SqlQuery<int>(@"SELECT IFNULL(MAX(payperiodid), 0) 
					FROM payperiods WHERE startdate <= @p0 AND enddate >= @p0 LIMIT 1", _d).FirstOrDefault();
		}
	}
}
