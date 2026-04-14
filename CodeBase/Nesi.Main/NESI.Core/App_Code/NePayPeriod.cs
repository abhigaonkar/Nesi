using DevExpress.Xpo;
using System;
using System.Data;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NePayPeriod
	/// </summary>
	public class NePayPeriod
		{
		public int payperiodID { get; set; }
		public int id { get; set; }
		public string StartDate { get; set; }
		public string Enddate { get; set; }
		public DateTime start_date {get; set; }
		public DateTime end_date {get; set; }
		public bool is_complete { get; set; }
		public bool completed { get; set; }
		public bool straight_eight_stat { get {return payperiodID >= 318; }}

		public NePayPeriod(){}
		public DataTable LoadPayPeriodList()
			{
			return Toolbox.doSQL_dt(@"SELECT payperiodid, CONCAT(DATE(startdate), ' - ', DATE(enddate)) paydate FROM payperiods  WHERE completed = 1 ORDER BY startdate DESC" , null);
			}
		public static int CurrentOpenPayPeriodId()
			{
			return Toolbox.doSQL_int(@"SELECT IFNULL(MIN(payperiodid), 0) FROM payperiods WHERE completed = 0 ORDER BY payperiodid ASC limit 1");
			}
		public static int CurrentPayPeriodId()
			{
			return Toolbox.doSQL_int(@"SELECT IFNULL(MIN(payperiodid), 0) FROM payperiods WHERE startdate < NOW() AND enddate > NOW() limit 1");
			}
		public int CurrentPayPeriod()
			{
			return Toolbox.doSQL_int(@"SELECT IFNULL(MIN(payperiodid), 0) FROM payperiods WHERE startdate < NOW() AND enddate > NOW() limit 1");
			}
		public static string PayPeriod()
			{
			return Toolbox.doSQL_string("SELECT CONCAT(DATE(startdate), ' - ', DATE(enddate)) FROM payperiods WHERE startdate <= CURDATE() and enddate > CURDATE()");
			}
		public NePayPeriod(int _id)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM payperiods WHERE payperiodid = @v0 ", new object[] {  _id } );
			if (dt.Rows.Count == 1)
				{
				Init(dt.Rows[0]);
				}
			}
		private void Init(DataRow _dr)
			{
			id = Convert.ToInt32(_dr["payperiodid"]);
			payperiodID = Convert.ToInt32(_dr["payperiodid"]);
			start_date = Convert.ToDateTime(_dr["Startdate"]);
			StartDate = start_date.ToString("yyyy-MM-dd");
			end_date = Convert.ToDateTime(_dr["Enddate"]);
			Enddate = end_date.ToString("yyyy-MM-dd"); 
			completed = Convert.ToBoolean(_dr["completed"]);
			is_complete = Convert.ToBoolean(_dr["completed"]);
			}
		public static int get_payperiod_id(DateTime _d)
			{
			return Toolbox.doSQL_int(@"SELECT IFNULL(MAX(payperiodid), 0) FROM payperiods WHERE startdate <= @v0 AND enddate >= @v0 LIMIT 1", Toolbox.MySQL_shortdt(_d));
			}
		public void Save()
			{
			using (var uow = new UnitOfWork())
				{
				var pp = uow.GetObjectByKey<ne_xpo.cs.payperiods>(id);
				pp.completed = completed ? 1: 0;
				pp.Save();
				uow.CommitChanges();
				}
			}
		}
	}