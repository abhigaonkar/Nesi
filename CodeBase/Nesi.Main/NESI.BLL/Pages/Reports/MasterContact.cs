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
	public class MasterContact : BLLBase, IModelGenerator<DTO.ViewModels.Page.Reports.MasterContact>, IModelEditor<DTO.ViewModels.Page.Reports.MasterContact>
	{
		protected readonly BLL.Common.Cache.DatatableCacher cacher = BLL.Common.Cache.Global.Datatable;
		protected BLL.Common.Cache.IReportCacher schema_cacher = BLL.Common.Cache.Global.IReport;
		public int GetRecordCountFromDataTable { get; set; }
		public Dictionary<string, string> GetColumnSummary { get; set; }
		public List<string> SetColumnList { get; set; }
		private readonly string memCacheKey = "";
		private readonly BodyParams param;
		public object GroupSummary { get; set; }
        public object CaculationsOnAllColumns { get; set; }

        private const string query = @"
            SELECT
	            a.contact_id,
	            a.contact_name,
	            a.contact_email,
	            a.contact_title,
	            a.contact_cellphone,
	            cs.customer_or_contact_status contact_status,
	            a.contact_extension,
	            a.contact_login,
	            a.contact_password,
	            a.contact_type,
	            a.contact_status_id,
	            a.contact_login_enabled,
	            a.contact_directline,
	            a.contact_cust_id,
	            b.customer_name,
	            c.vendor_name,
	            a.stopsurveys,
	            d.address_addr1,
	            d.address_city,
	            d.Address_Postal,
	            d.address_prov,
	            f.member_fullname project_mgr,
	            g.ddl_name business_unit,
	            b.customer_id,
            (SELECT COUNT(woprog.WOProg_ID) FROM woprog WHERE woprog.woprog_customer_id = b.customer_id AND WOProg_Contact_ID = a.contact_id AND a.contact_type='Customer') wos,
	            (SELECT COUNT(quote_master.quote_id) FROM quote_master WHERE quote_master.customer_id = b.customer_id AND quote_master.Contact_ID = a.contact_id AND a.contact_type='Customer') quotes
            FROM
	            contact a
            LEFT JOIN 
	            customer b ON 
		            a.contact_cust_id = b.customer_id AND 
		            a.contact_type = 'Customer'
            LEFT JOIN 
	            vendor c ON 
		            a.contact_cust_id = c.vendor_id AND 
		            a.contact_type = 'Vendor'
            LEFT JOIN
	            address d ON a.address_id = d.address_id 
            LEFT JOIN
	            customer_sales_properties e ON d.address_id = e.address_id
            LEFT JOIN
	            member f ON e.project_mgr_member_id = f.member_id
            LEFT JOIN
	            business_unit g ON f.business_unit_id = g.id
            LEFT JOIN 
		        customer_or_contact_status cs ON cs.customer_or_contact_status_id = a.contact_status_id ";

		public MasterContact()
		{ }

		public MasterContact(Employee currentUser, BodyParams param) : base(currentUser)
		{
			this.param = param;
			this.memCacheKey = "MasterContact_" + this.CurrentUser.Id;
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

		public DataTable GetFilterDataFromStore(string item)
		{
			return cacher.GetValue(memCacheKey + "_filterData" + item);
		}

		public void SetFilterDataStore(string item, DataTable cache)
		{
			cacher.Set(memCacheKey + "_filterData" + item, cache);
		}

		private List<DTO.ViewModels.Page.Reports.MasterContact> GroupRecords(List<DTO.ViewModels.Page.Reports.MasterContact> wolist, List<string> expandGroups, DataTable filteredTable, IList<DataTableAggregateFunction> fieldsForCalculation, string item)
		{
			fieldsForCalculation.Add(new DataTableAggregateFunction()
			{
				enmFunction = AggregateFunction.Count,
				ColumnName = item,
				OutPutColumnName = item + "_Count"
			});
			DataTable groupedData = filteredTable.GetGroupedBy(expandGroups, fieldsForCalculation);
			var result = groupedData.AsEnumerable();
			wolist.Clear();
			var groupedList = ConvertToList(result.ToList(), wolist, true);
			return groupedList;
		}
		public object ProcessExtra(DataTable table)
		{
			return null;
		}


		public void SetGroupStore(string item, List<DTO.ViewModels.Page.Reports.MasterContact> cache)
		{

		}

		public void SetSchemaStore(IReport cache)
		{

		}

		public DataTable GetGroupSummaryFromStore(string item)
		{
			return null;
		}

		public void SetGroupSummaryStore(string item, DataTable cache)
		{

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

					// extra parameter for query
					if (param.queryparam != null && param.queryparam.Length > 0)
					{
						foreach (var item in param.queryparam)
						{
							if (!string.IsNullOrWhiteSpace(item.coulumnname))
							{
								cmd.Parameters.AddWithValue(item.coulumnname, item.value);
							}
						}
					}

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

		public IReport GetSchemaFromStore()
		{
			return null;
		}

		public List<DTO.ViewModels.Page.Reports.MasterContact> GetGroupFromStore(string item)
		{
			return null;
		}

		public List<DTO.ViewModels.Page.Reports.MasterContact> ConvertToList(List<DataRow> dtRows, List<DTO.ViewModels.Page.Reports.MasterContact> wolist, bool addGroupByMeta = false, string[] metadata = null)
		{
			foreach (DataRow row in dtRows)
			{
				DTO.ViewModels.Page.Reports.MasterContact _tmp =
					new DTO.ViewModels.Page.Reports.MasterContact();

				if (row.Table.Columns.Contains("contact_id"))
					_tmp.contact_id = Convert.ToInt32(row["contact_id"]);
				if (row.Table.Columns.Contains("contact_name"))
					_tmp.contact_name = (string)(row["contact_name"] == DBNull.Value ? null : Convert.ToString(row["contact_name"]));
				if (row.Table.Columns.Contains("contact_email"))
					_tmp.contact_email = (string)(row.IsNull("contact_email") ? null : Convert.ToString(row["contact_email"]));
				if (row.Table.Columns.Contains("contact_title"))
					_tmp.contact_title = (string)(row.IsNull("contact_title") ? null : Convert.ToString(row["contact_title"]));
				if (row.Table.Columns.Contains("contact_cellphone"))
					_tmp.contact_cellphone = (string)(row.IsNull("contact_cellphone") ? null : row["contact_cellphone"]);
				if (row.Table.Columns.Contains("contact_status"))
					_tmp.contact_status = (string)(row.IsNull("contact_status") ? null : Convert.ToString(row["contact_status"]));
				if (row.Table.Columns.Contains("contact_extension"))
					_tmp.contact_extension = (string)(row.IsNull("contact_extension") ? null : Convert.ToString(row["contact_extension"]));
				if (row.Table.Columns.Contains("contact_login"))
					_tmp.contact_login = (string)(row.IsNull("contact_login") ? null : Convert.ToString(row["contact_login"]));
				if (row.Table.Columns.Contains("contact_password"))
					_tmp.contact_password = (string)(row.IsNull("contact_password") ? null : row["contact_password"]);
				if (row.Table.Columns.Contains("contact_type"))
					_tmp.contact_type = (string)(row.IsNull("contact_type") ? null : row["contact_type"]);
				if (row.Table.Columns.Contains("contact_status_id"))
					_tmp.contact_status_id = (int?)(row.IsNull("contact_status_id") ? (int?)null : Convert.ToInt32(row["contact_status_id"]));
				if (row.Table.Columns.Contains("contact_login_enabled"))
					_tmp.contact_login_enabled = (bool)(row.IsNull("contact_login_enabled") ? false : (Convert.ToInt16(row["contact_login_enabled"]) > 0 ? true : false));
				if (row.Table.Columns.Contains("contact_directline"))
					_tmp.contact_directline = (string)(row.IsNull("contact_directline") ? null : row["contact_directline"]);
				if (row.Table.Columns.Contains("contact_cust_id"))
					_tmp.contact_cust_id = (int?)(row.IsNull("contact_cust_id") ? (int?)null : Convert.ToInt32(row["contact_cust_id"]));
				if (row.Table.Columns.Contains("customer_name"))
					_tmp.customer_name = (string)(row.IsNull("customer_name") ? null : Convert.ToString(row["customer_name"]));
				if (row.Table.Columns.Contains("vendor_name"))
					_tmp.vendor_name = (string)(row.IsNull("vendor_name") ? null : Convert.ToString(row["vendor_name"]));
				if (row.Table.Columns.Contains("stopsurveys"))
					_tmp.stopsurveys = (bool)(row.IsNull("stopsurveys") ? false : (Convert.ToInt16(row["stopsurveys"]) > 0 ? true : false));
				if (row.Table.Columns.Contains("address_addr1"))
					_tmp.address_addr1 = (string)(row["address_addr1"] == DBNull.Value ? null : Convert.ToString(row["address_addr1"]));
				if (row.Table.Columns.Contains("address_city"))
					_tmp.address_city = (string)(row.IsNull("address_city") ? null : Convert.ToString(row["address_city"]));
				if (row.Table.Columns.Contains("Address_Postal"))
					_tmp.address_postal = (string)(row.IsNull("Address_Postal") ? null : Convert.ToString(row["Address_Postal"]));
				if (row.Table.Columns.Contains("address_prov"))
					_tmp.address_prov = (string)(row.IsNull("address_prov") ? null : Convert.ToString(row["address_prov"]));
				if (row.Table.Columns.Contains("project_mgr"))
					_tmp.project_mgr = (string)(row.IsNull("project_mgr") ? null : Convert.ToString(row["project_mgr"]));
				if (row.Table.Columns.Contains("business_unit"))
					_tmp.business_unit = (string)(row.IsNull("business_unit") ? null : row["business_unit"].ToString());
				if (row.Table.Columns.Contains("customer_id"))
					_tmp.customer_id = (int?)(row.IsNull("customer_id") ? (int?)null : Convert.ToInt32(row["customer_id"]));
				if (row.Table.Columns.Contains("wos"))
					_tmp.wos = (int?)(row.IsNull("wos") ? (int?)null : Convert.ToInt32(row["wos"]));
				if (row.Table.Columns.Contains("quotes"))
					_tmp.quotes = (int?)(row.IsNull("quotes") ? (int?)null : Convert.ToInt32(row["quotes"]));
				if (metadata == null)
				{
					if (param.column_groupBy != null && param.column_groupBy.Length > 0 && addGroupByMeta)
					{
						_tmp.Metadata = new List<string>();
						for (int i = param.column_groupBy.Length; i < dtRows[0].Table.Columns.Count; i++)
						{
							_tmp.Metadata.Add(row[i].ToString());
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
		public async Task<int> CreateModel(DTO.ViewModels.Page.Reports.MasterContact model)
		{
			throw new NotImplementedException();
		}
		public async Task<int> EditModel(DTO.ViewModels.Page.Reports.MasterContact ca)
		{
			try
			{
				NESI.Data.Entities.contact cmodel = _db.contact.FirstOrDefault(i => i.Contact_ID == ca.contact_id);

				if (cmodel != null && cmodel.Contact_ID != null)
				{
					cmodel.Contact_Login_Enabled = ca.contact_login_enabled ? 1 : 0;
					cmodel.stopsurveys = ca.stopsurveys ? 1 : 0;
					var resultsupdated = _db.SaveChanges();

					cacher.Delete(this.memCacheKey);

					if (resultsupdated > 0)
						return cmodel.Contact_ID;
				}
				return 0;
			}
			catch
			{
				return 0;
			}
		}


		public async Task<string> DeleteModel(int id)
		{
			throw new NotImplementedException();
		}
		public IQueryable<DTO.ViewModels.Page.Reports.MasterContact> GetQueryable(Expression<Func<DTO.ViewModels.Page.Reports.MasterContact, bool>> match)
		{
			throw new NotImplementedException();
		}
		public IQueryable<DTO.ViewModels.Page.Reports.MasterContact> GlobalSearch(string searchText, Expression<Func<DTO.ViewModels.Page.Reports.MasterContact, bool>> match)
		{
			throw new NotImplementedException();
		}
		public List<DTO.ViewModels.Page.Reports.MasterContact> GetDataTableList()
		{
			throw new NotImplementedException();
		}

        public object GetProcessExtra()
        {
            return null;
        }
    }
}
