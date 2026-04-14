using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;
using NESI.Common;
using NESI.Common.Interface;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Page.Reports;

namespace NESI.BLL.Pages.Reports
{
	public class CustomerAsset : BLLBase, IModelGenerator<DTO.ViewModels.Page.Reports.CustomerAsset>, IModelEditor<DTO.ViewModels.Page.Reports.CustomerAsset>
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
		private readonly string queryParamString = "";

		private const string query = @"
            SELECT 
                a.id, 
                TRIM(a.name) NAME, 
                TRIM(b.customer_name) customer_name, 
                a.customer_id, 
                a.address_id, 
                c.member_id,
                TRIM(a.description) description, 
                a.active, 
                TRIM(c.member_fullname) addedby, 
                a.manufacturer, 
                a.model,
                CONCAT('[',d.address_type,'] - ',d.address_addr1) location
            FROM 
                customer_asset a 
            LEFT JOIN customer b ON a.customer_id = b.customer_id 
            LEFT JOIN member c ON c.member_id = a.addedby
            LEFT JOIN address d ON a.address_id = d.address_id AND d.address_table = 'customer'";

		public CustomerAsset()
		{ }

		public CustomerAsset(Employee currentUser, BodyParams param) : base(currentUser)
		{
			this.param = param;

			this.memCacheKey = "CustomerAsset_" + this.CurrentUser.Id;
			if (!string.IsNullOrEmpty(currentUser.ExtraType))
			{
				var type = currentUser.ExtraType;
				if (type == "customer")
				{
					var customer = (Core.Member.Customer)currentUser.ExtraUser;
					if (customer != null)
					{
						this.memCacheKey = "CustomerAsset_customer_" + customer.CustomerId;
						this.queryParamString = $@" where a.customer_id={customer.CustomerId}";
					}
				}
			}

			if (param != null)
			{
				if (param.queryparam != null && param.queryparam.Length == 2)
				{
					this.memCacheKey = "CustomerAsset_" + this.CurrentUser.Id + param.queryparam[0].value + param.queryparam[1].value;
					this.queryParamString = $@" where a.customer_id={param.queryparam[0].value.ToString()} and a.address_id={param.queryparam[1].value.ToString()}";
				}
				if (param.refreshCache.HasValue && param.refreshCache.Value)
				{
					cacher.Delete(this.memCacheKey);
				}
			}
		}

		public DataTable GetGroupSummaryFromStore(string item)
		{
			return null;
		}

		public void SetGroupSummaryStore(string item, DataTable cache)
		{

		}

		public List<string> GetDistinct(BodyParams _param)
		{
			var res = GetDataFromStore();
			var colName = _param.Selectby[0];
			return (res.Rows.Cast<DataRow>().Select(row => row[colName].ToString())).Distinct().ToList();
		}

		public DataTable GetFilterDataFromStore(string item)
		{
			return null;
		}

		public void SetFilterDataStore(string item, DataTable cache)
		{
		}

		public object ProcessExtra(DataTable table)
		{
			return null;
		}

		public IReport GetSchemaFromStore()
		{
			return null;
		}

		public List<DTO.ViewModels.Page.Reports.CustomerAsset> GetGroupFromStore(string item)
		{
			return null;
		}

		public void SetGroupStore(string item, List<DTO.ViewModels.Page.Reports.CustomerAsset> cache)
		{

		}

		public void SetSchemaStore(IReport cache)
		{

		}

		public List<DTO.ViewModels.Page.Reports.CustomerAsset> ConvertToList(List<DataRow> dtRows, List<DTO.ViewModels.Page.Reports.CustomerAsset> wolist, bool addGroupByMeta = false, string[] metadata = null)
		{
			foreach (DataRow row in dtRows)
			{
				DTO.ViewModels.Page.Reports.CustomerAsset _tmp =
					new DTO.ViewModels.Page.Reports.CustomerAsset();

				if (row.Table.Columns.Contains("id"))
					_tmp.id = Convert.ToInt32(row["id"]);
				if (row.Table.Columns.Contains("name"))
					_tmp.name = (string)(row.IsNull("name") ? null : Convert.ToString(row["name"]));
				if (row.Table.Columns.Contains("customer_name"))
					_tmp.customer_name = (string)(row.IsNull("customer_name") ? null : Convert.ToString(row["customer_name"]));
				if (row.Table.Columns.Contains("customer_id"))
					_tmp.customer_id = (int?)(row.IsNull("customer_id") ? (int?)null : Convert.ToInt32(row["customer_id"]));
				if (row.Table.Columns.Contains("member_id"))
					_tmp.member_id = (int?)(row.IsNull("member_id") ? (int?)null : Convert.ToInt32(row["member_id"]));
				if (row.Table.Columns.Contains("address_id"))
					_tmp.address_id = (int?)(row.IsNull("address_id") ? (int?)null : Convert.ToInt32(row["address_id"]));
				if (row.Table.Columns.Contains("description"))
					_tmp.description = (string)(row.IsNull("description") ? null : Convert.ToString(row["description"]));
				if (row.Table.Columns.Contains("active"))
					_tmp.active = Convert.ToBoolean(row["active"]);
				if (row.Table.Columns.Contains("addedby"))
					_tmp.addedby = (string)(row.IsNull("addedby") ? null : Convert.ToString(row["addedby"]));
				if (row.Table.Columns.Contains("manufacturer"))
					_tmp.manufacturer = (string)(row.IsNull("manufacturer") ? null : Convert.ToString(row["manufacturer"]));
				if (row.Table.Columns.Contains("model"))
					_tmp.model = (string)(row.IsNull("model") ? null : Convert.ToString(row["model"]));
				if (row.Table.Columns.Contains("location"))
					_tmp.location = (string)(row.IsNull("location") ? null : Convert.ToString(row["location"]));

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
		public DataTable GetDatafromSource()
		{
			DataTable dbResult = new DataTable();

			using (MySqlConnection conn = (MySqlConnection)_db.Database.Connection)
			{
				using (MySqlCommand cmd = new MySqlCommand(query + queryParamString, conn))
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

		/// <summary>
		/// Create new Asset in customer asset table
		/// </summary>
		/// <param name="ca"></param>
		/// <returns></returns>
		public async Task<int> CreateModel(DTO.ViewModels.Page.Reports.CustomerAsset ca)
		{
			try
			{
				NESI.Data.Entities.customer_asset camodel = new NESI.Data.Entities.customer_asset();
				camodel.Name = ca.name;
				camodel.Description = ca.description;
				camodel.Active = ca.active;
				camodel.manufacturer = ca.manufacturer;
				camodel.model = ca.model;
				if (ca.address_id != null)
					camodel.address_id = ca.address_id;
				if (ca.customer_id != null)
					camodel.customer_id = ca.customer_id;

				camodel.addedby = this.CurrentUser.Id;


				_db.customer_asset.Add(camodel);

				await _db.SaveChangesAsync();

				cacher.Delete(this.memCacheKey);

				//return CreatedAtRoute("DefaultApi", new { id = ca.id }, camodel);
				return camodel.ID;
			}
			catch
			{
				return 0;
			}
		}

		/// <summary>
		/// Edit asset in customer asset table
		/// </summary>
		/// <param name="ca"></param>
		/// <returns></returns>
		public async Task<int> EditModel(DTO.ViewModels.Page.Reports.CustomerAsset ca)
		{
			try
			{
				NESI.Data.Entities.customer_asset camodel = _db.customer_asset.First(i => i.ID == ca.id);
				if (ca.name != null)
					camodel.Name = ca.name;
				if (ca.name != null)
					camodel.Description = ca.description;
				if (ca.name != null)
					camodel.Active = ca.active;
				if (ca.name != null)
					camodel.manufacturer = ca.manufacturer;
				if (ca.model != null)
					camodel.model = ca.model;
				if (ca.address_id != null)
					camodel.address_id = ca.address_id;
				if (ca.customer_id != null)
					camodel.customer_id = ca.customer_id;

				if (ca.member_id != null)
					camodel.addedby = ca.member_id;


				_db.SaveChanges();

				cacher.Delete(this.memCacheKey);

				return camodel.ID;
			}
			catch
			{
				return 0;
			}
		}


		public async Task<string> DeleteModel(int id)
		{
			try
			{
				//Check if ASSET is linked to work order. If linked then do not delete the asset.
				var existence_count = 0;
				existence_count = nesi.core.Toolbox.doSQL_int(@"SELECT COUNT(woprog_id) FROM woprog WHERE woprog_assetid = @v0 ", new object[] { id });

				if (existence_count > 0)
				{
					return "This asset is currently linked to work orders and cannot be deleted";
				}
				else
				{
					NESI.Data.Entities.customer_asset camodel = _db.customer_asset.First(i => i.ID == id);

					_db.customer_asset.Remove(camodel);
					_db.SaveChanges();

					cacher.Delete(this.memCacheKey);

					return "success";
				}
			}
			catch
			{
				throw;
			}
		}

		public IQueryable<DTO.ViewModels.Page.Reports.CustomerAsset> GetQueryable(Expression<Func<DTO.ViewModels.Page.Reports.CustomerAsset, bool>> match)
		{
			throw new NotImplementedException();
		}

		public IQueryable<DTO.ViewModels.Page.Reports.CustomerAsset> GlobalSearch(string searchText, Expression<Func<DTO.ViewModels.Page.Reports.CustomerAsset, bool>> match)
		{
			throw new NotImplementedException();
		}

		public List<DTO.ViewModels.Page.Reports.CustomerAsset> GetDataTableList()
		{
			throw new NotImplementedException();
		}

		public object GetProcessExtra()
		{
			return null;
		}
	}
}
