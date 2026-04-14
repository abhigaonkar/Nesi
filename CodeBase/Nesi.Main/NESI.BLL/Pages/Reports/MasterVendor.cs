using System.Linq;
using NESI.Data.Entities;
using System;
using NESI.Common.Interface;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NESI.BLL.Base;
using NESI.DTO.ViewModels.Page.Reports;
using System.Collections.Generic;
using NESI.Common;
using System.Data;
using System.Runtime.Caching;
using System.Text;
using MySql.Data.MySqlClient;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.Reports
{
	public class MasterVendor : BLLBase, IModelGenerator<DTO.ViewModels.Page.Reports.MasterVendor>, IModelEditor<DTO.ViewModels.Page.Reports.MasterVendor>
	{
		protected readonly BLL.Common.Cache.DatatableCacher cacher = BLL.Common.Cache.Global.Datatable;
		protected BLL.Common.Cache.IReportCacher schema_cacher = BLL.Common.Cache.Global.IReport;
		public int GetRecordCountFromDataTable { get; set; }
		public Dictionary<string, string> GetColumnSummary { get; set; }
		public List<string> SetColumnList { get; set; }
        public object GroupSummary { get; set; }
        public object CaculationsOnAllColumns { get; set; }

        private readonly string memCacheKey = "";
		private readonly BodyParams param;

		private const string query = @"
            SELECT
                vendor.Vendor_ID,
                vendor.vendor_number,
                vendor.Vendor_Name,
                IF(vendor.Vendor_CPRS='T',TRUE,FALSE) CPRS,
                vendor.Vendor_QC_DateTime AS qc_date,
                vendor.Vendor_Active,
                IF(vendor.is_partner=1,TRUE,FALSE) is_partner,
                IF(vendor.Vendor_Hold='T',TRUE,FALSE) vendor_hold,
                vendor.vendor_notes,
                (SELECT IFNULL((SELECT ROUND(SUM(poprog_header.poprog_total_recCost), 2) FROM poprog_header WHERE poprog_header.poprog_vendor_id = vendor.vendor_id),0)) AS total_purchase,
                (SELECT IFNULL((SELECT ROUND(SUM(poprog_header.poprog_total_recCost), 2) FROM poprog_header WHERE poprog_header.poprog_vendor_id = vendor.vendor_id AND poprog_header.poprog_cutdate>CURDATE()-INTERVAL 370 DAY),0)) AS last_12_months,
                (SELECT IFNULL((SELECT COUNT(poprog_header.poprog_id) FROM poprog_header WHERE poprog_header.ap_problem_last_notice_sent IS NOT NULL AND (poprog_header.poprog_cutdate>CURDATE()-INTERVAL 370 DAY) AND poprog_header.poprog_vendor_id = vendor.vendor_id),0)) AS problems_12mo,
                CONCAT(address.Address_Addr1,', ',address.Address_Addr2) address,
                address.Address_City,
                address.Address_Prov,
                address.Address_Postal,
                address.Address_Country,
                CONCAT('(',address.Address_PhoneArea,') ',address.Address_PhoneFirst,'-',address.Address_PhoneLast) phone,
                address.Address_Web,
                vendor.nesi_member_id nesi_member_id,
                m.member_fullname
            FROM
                vendor
            INNER JOIN address ON vendor.Vendor_ID = address.address_table_id AND address.address_table = 'Vendor'
            LEFT JOIN member m ON vendor.nesi_member_id = m.member_id AND m.business_unit_id = 11 AND m.member_status = 'Active'";

		public MasterVendor(Employee currentUser, BodyParams param) : base(currentUser)
		{
			this.param = param;
			this.memCacheKey = "MasterVendor_" + this.CurrentUser.Id;
			if (param != null)
			{
				if (param.refreshCache.HasValue && param.refreshCache.Value)
				{
					cacher.Delete(this.memCacheKey);
				}
			}
		}

		public List<string> GetDistinct(BodyParams _param)
		{
			var res = GetDataFromStore();
			var colName = _param.Selectby[0];
			return (res.Rows.Cast<DataRow>().Select(row => row[colName].ToString())).Distinct().ToList();
		}

		public DataTable GetGroupSummaryFromStore(string item)
		{
			return null;
		}

		public void SetGroupSummaryStore(string item, DataTable cache)
		{

		}

		public IReport GetSchemaFromStore()
		{
			return null;
		}

		public DataTable GetFilterDataFromStore(string item)
		{
			return null;
		}

		public void SetFilterDataStore(string item, DataTable cache)
		{
		}

		public List<DTO.ViewModels.Page.Reports.MasterVendor> GetGroupFromStore(string item)
		{
			return null;
		}

		public void SetGroupStore(string item,  List<DTO.ViewModels.Page.Reports.MasterVendor> cache)
		{
		}

		public void SetSchemaStore(IReport cache)
		{

		}

		public List<DTO.ViewModels.Page.Reports.MasterVendor> ConvertToList(List<DataRow> dtRows, List<DTO.ViewModels.Page.Reports.MasterVendor> wolist, bool addGroupByMeta = false, string[] metadata = null)
		{
			foreach (DataRow row in dtRows)
			{
				DTO.ViewModels.Page.Reports.MasterVendor _tmp =
					new DTO.ViewModels.Page.Reports.MasterVendor();

				if (row.Table.Columns.Contains("Vendor_id"))
					_tmp.vendor_id = Convert.ToInt32(row["Vendor_id"]);
				if (row.Table.Columns.Contains("vendor_number"))
					_tmp.vendor_number = (string)(row.IsNull("vendor_number") ? null : row["vendor_number"].ToString());
				if (row.Table.Columns.Contains("Vendor_Name"))
					_tmp.vendor_name = (string)(row.IsNull("Vendor_Name") ? null : row["Vendor_Name"].ToString());
				if (row.Table.Columns.Contains("CPRS"))
					_tmp.cprs = Convert.ToBoolean(row["CPRS"]);
				if (row.Table.Columns.Contains("vendor_hold"))
					_tmp.vendor_hold = Convert.ToBoolean(row["vendor_hold"]);
				if (row.Table.Columns.Contains("qc_date"))
				{
					_tmp.qc_date =
						(DateTime?)(row.IsNull("qc_date") ? (DateTime?)null : Convert.ToDateTime(row["qc_date"]));
					if (_tmp.qc_date.HasValue)
						_tmp.qc_date = Convert.ToDateTime(_tmp.qc_date.Value.ToShortDateString());
				}
				if (row.Table.Columns.Contains("Vendor_Active"))
					_tmp.vendor_active = ((int)(row.IsNull("Vendor_Active") ? 0 : Convert.ToInt32(row["Vendor_Active"]))) == 1;
				if (row.Table.Columns.Contains("is_partner"))
					_tmp.is_partner = Convert.ToBoolean(row["is_partner"]);
				if (row.Table.Columns.Contains("vendor_notes"))
					_tmp.vendor_notes = (string)(row.IsNull("vendor_notes") ? null : row["vendor_notes"].ToString());
				if (row.Table.Columns.Contains("total_purchase"))
					_tmp.total_purchase = (double)(row.IsNull("total_purchase") ? 0 : Convert.ToDouble(row["total_purchase"]));
				if (row.Table.Columns.Contains("last_12_months"))
					_tmp.last_12_months = (double)(row.IsNull("last_12_months") ? 0 : Convert.ToDouble(row["last_12_months"]));
				if (row.Table.Columns.Contains("problems_12mo"))
					_tmp.problems_12mo = (double)(row.IsNull("problems_12mo") ? 0 : Convert.ToDouble(row["problems_12mo"]));
				if (row.Table.Columns.Contains("address"))
					_tmp.address = (string)(row.IsNull("address") ? null : row["address"].ToString());
				if (row.Table.Columns.Contains("Address_City"))
					_tmp.address_city = (string)(row.IsNull("Address_City") ? null : row["Address_City"]);
				if (row.Table.Columns.Contains("Address_Prov"))
					_tmp.address_prov = (string)(row.IsNull("Address_Prov") ? null : row["Address_Prov"]);
				if (row.Table.Columns.Contains("Address_Postal"))
					_tmp.address_postal = (string)(row.IsNull("Address_Postal") ? null : row["Address_Postal"]);
				if (row.Table.Columns.Contains("Address_Country"))
					_tmp.address_country = (string)(row.IsNull("Address_Country") ? null : row["Address_Country"]);
				if (row.Table.Columns.Contains("phone"))
					_tmp.phone = (string)(row.IsNull("phone") ? null : row["phone"]);
				if (row.Table.Columns.Contains("Address_Web"))
					_tmp.address_web = (string)(row.IsNull("Address_Web") ? null : row["Address_Web"]);
				if (row.Table.Columns.Contains("member_fullname"))
					_tmp.member_fullname = (string)(row.IsNull("member_fullname") ? null : row["member_fullname"]);

				if (metadata == null)
				{
					if (param.column_groupBy != null && param.column_groupBy.Length > 0 && addGroupByMeta)
					{
						_tmp.Metadata = new List<string>();
						for (int i = param.column_groupBy.Length; i < dtRows[0].Table.Columns.Count; i++)
						{
							_tmp.Metadata.Add(Convert.ToString(row[i]));
						}
					}
				}
				else
				{
					_tmp.Metadata = new List<string>();
					_tmp.Metadata.AddRange(metadata);

				}
				wolist.Add(_tmp);
			}
			return wolist;
		}

		public object ProcessExtra(DataTable table)
		{
			return null;
		}

		public DataTable GetDatafromSource()
		{
			DataTable dbResult = new DataTable();

			using (MySqlConnection conn = (MySqlConnection)_db.Database.Connection)
			{
				using (MySqlCommand cmd = new MySqlCommand(query, conn))
				{
					cmd.CommandTimeout = 300;
					cmd.CommandType = CommandType.Text;
					using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
					{
						sda.Fill(dbResult);

						cacher.Delete(this.memCacheKey);
						cacher.Add(memCacheKey, dbResult,
							DateTimeOffset.UtcNow.AddMinutes(
								Convert.ToDouble(NESI.BLL.Common.Shared.Configuration.MemCacheAbsoluteTimeout)));
					}
				}
			}

			return dbResult;
		}

		public DataTable GetDataFromStore()
		{
			DataTable dbResult = new DataTable();
			var res = (DataTable)cacher.GetValue(memCacheKey);
			if (res != null)
			{
				dbResult = res;
			}
			else
			{
				dbResult = GetDatafromSource();
			}
			return dbResult;
		}

		public Task<int> CreateModel(DTO.ViewModels.Page.Reports.MasterVendor model)
		{
			throw new NotImplementedException();
		}

		public async Task<int> EditModel(DTO.ViewModels.Page.Reports.MasterVendor masterVendor)
		{
			try
			{
				var camodel = _db.vendor.First(i => i.Vendor_ID == masterVendor.vendor_id);
				if (masterVendor.vendor_number != null)
					camodel.vendor_number = masterVendor.vendor_number;
				if (masterVendor.vendor_name != null)
					camodel.vendor_name = masterVendor.vendor_name;
				camodel.Vendor_Hold = (string)(masterVendor.vendor_hold ? "T" : "F");//masterVendor.vendor_Hold;
				if (masterVendor.vendor_notes != null)
					camodel.vendor_notes = masterVendor.vendor_notes;
				if (masterVendor.vendor_active)
					camodel.Vendor_Active = masterVendor.vendor_active ? 1 : 0;
				camodel.is_partner = masterVendor.is_partner;
				camodel.Vendor_CPRS = (string)(masterVendor.cprs ? "T" : "F");

				var addr = _db.address.FirstOrDefault(a => a.address_table_id == masterVendor.vendor_id && a.address_table == "Vendor");
				if (addr != null)
				{
					if (masterVendor.address_web != null)
						addr.Address_Web = masterVendor.address_web;
				}

				_db.SaveChanges();

				cacher.Delete(this.memCacheKey);
				return camodel.Vendor_ID;
			}
			catch
			{
				return 0;
			}
		}

		public Task<string> DeleteModel(int id)
		{
			throw new NotImplementedException();
		}

		public IQueryable<DTO.ViewModels.Page.Reports.MasterVendor> GetQueryable(Expression<Func<DTO.ViewModels.Page.Reports.MasterVendor, bool>> match)
		{
			throw new NotImplementedException();
		}

		public IQueryable<DTO.ViewModels.Page.Reports.MasterVendor> GlobalSearch(string searchText, Expression<Func<DTO.ViewModels.Page.Reports.MasterVendor, bool>> match)
		{
			throw new NotImplementedException();
		}

		public List<DTO.ViewModels.Page.Reports.MasterVendor> GetDataTableList()
		{
			throw new NotImplementedException();
		}

        public object GetProcessExtra()
        {
            return null;
        }
    }
}
