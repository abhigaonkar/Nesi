using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NESI.BLL.Base;
using NESI.Common.Interface;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Page.Reports;
using System.Data;
using MySql.Data.MySqlClient;
using System.Runtime.Caching;
using NESI.BLL.Core.Employee;
using NESI.Common;
using System.Threading.Tasks;
using NESI.Common.Models;
using NESI.DTO.ViewModels.Core;
using nesi.core;

namespace NESI.BLL.Pages.Reports
{
    public class MasterWorkOrderV2 : BLLBase, IModelGenerator<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>, IModelEditor<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>
    {
		// MemoryCache memCache = MemoryCache.Default;
		protected readonly BLL.Common.Cache.DatatableCacher cacher = BLL.Common.Cache.Global.Datatable;
	    protected Common.Cache.IReportCacher schema_cacher = Common.Cache.Global.IReport;
	    protected Common.Cache.MemoryCacher<List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>> group_cacher = new Common.Cache.MemoryCacher<List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>>();
	    protected Common.Cache.MemoryCacher<string> flag_cacher = new Common.Cache.MemoryCacher<string>();


	    public int GetRecordCountFromDataTable { get; set; }
        // public List<string> GetColumnSummary { get; set; }
        public Dictionary<string, string> GetColumnSummary { get; set; }
        public List<string> SetColumnList { get; set; }
        public object GroupSummary { get; set; }
        public object CaculationsOnAllColumns { get; set; }

		protected string memCacheKey => typeof(MasterWorkOrderV2) + CurrentUser.Guid + CurrentUser.Id + "_" + query_params_string();
        private readonly string memCacheKeyActingRam = "";
        private readonly BodyParams param;
        private bool view_dollar_totals = false;
        bool view_total_time = false;
        bool view_margin = false;

        private readonly string acting_Ram_sql = @"
            SELECT 0 Member_ID, 'No One' member_fullname
            UNION
           SELECT * FROM 
            (SELECT member.member_id, CONCAT(member_fullname, '(', bu.ddl_name, ')') member_fullname
            FROM member INNER JOIN business_unit bu ON bu.id = member.business_unit_id
            WHERE member_membertype_id IN(8, 11, 27, 34, 35, 37, 38, 39, 46, 49, 52, 53, 54, 55, 56, 59, 4, 5, 12, 17, 24, 25, 41)
        AND member_status = 'Active'
        ORDER BY bu.ddl_name ASC, member_fullname)a";

        public MasterWorkOrderV2()
        {

        }


		public MasterWorkOrderV2(Employee currentUser, BodyParams param) : base(currentUser)
        {
            this.param = param;
            this.memCacheKeyActingRam = "ActingRam2_" + this.CurrentUser.Id;
            this.view_margin = this.CurrentUser.AuthenticatedForPrivilege(58);
            this.view_dollar_totals = this.CurrentUser.AuthenticatedForPrivilege(59);

            if (param != null)
            {
                if (param.refreshCache.HasValue && param.refreshCache.Value)
                {
                    try
                    {
                        cacher.Delete(this.memCacheKey);
                    }
                    catch (Exception ex)
                    {
                        string e = "";
                    }
                    
                }
            }
        }


	    protected string query_params_string()
	    {
		    var r = "";
		    if (param.queryparam != null && param.queryparam.Length > 0)
		    {
			    foreach (var q in param.queryparam)
			    {
				    if (q != null)
				    {
					    r += q.ToString();
				    }
			    }
		    }
		    r += bu_ids_string;
		    return r;
	    }

	    public string bu_ids_string
	    {
		    get
		    {
			    if (string.IsNullOrEmpty(param?.bu_ids) || param.bu_ids.Length <= 3)
			    {
				    return CurrentUser.BusinessUnitId.ToString();
			    }
			    else
			    {
				    var list = param.bu_ids.Substring(3).Split(',');
				    return string.Join(",", list.Where(bu => CurrentUser.VisibleBusinessUnits.Contains(bu)).ToArray());
			    }
		    }
	    }

	    public string bu_ids_type
	    {
		    get
		    {
			    if (string.IsNullOrEmpty(param?.bu_ids))
			    {
				    return "[N]";
			    }
			    else
			    {
				    return param.bu_ids.Substring(0, 3);
			    }
		    }
	    }
		public object ProcessExtra(DataTable table)
	    {
		    return null;
	    }
		public List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> GetDataTableList()
        {
            DataTable dbResult = new DataTable();
            List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> wolist = new List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>();
            var res = (DataTable)cacher.GetValue(memCacheKey);
            if (res != null)
            {
                dbResult = res;
            }
            else
            {
                dbResult = GetDatafromSource();
            }

            DataTable filteredTable = dbResult;

            // FILTER THE RESULT
            if (param.column_filter != null && param.column_filter.Length > 0)
            {
                string filterExpression = string.Empty;
                foreach (var filter in param.column_filter)
                {
                    Filter flt = new Filter()
                    {
                        coulumnname = param.column_filter[0].coulumnname,
                        Operation = param.column_filter[0].Operation,
                        value = param.column_filter[0].value
                    };

                    if (!String.IsNullOrEmpty(filterExpression))
                        filterExpression += " AND " + flt.GetExpression();
                    else
                        filterExpression = flt.GetExpression();

                }

                var defaultView = dbResult.DefaultView;
                defaultView.RowFilter = filterExpression;
                filteredTable = defaultView.ToTable();
            }

            //if (queryParam.column_groupBy != null && queryParam.column_groupBy.Length > 0)
            //{
            //    List<string> groups = queryParam.column_groupBy.ToList();
            //    IList<DataTableAggregateFunction> fieldsForCalculation = new List<DataTableAggregateFunction>();
            //    foreach (var item in groups)
            //    {
            //        fieldsForCalculation.Add(new DataTableAggregateFunction()
            //        {
            //            enmFunction = AggregateFunction.Count,
            //            ColumnName = item,
            //            OutPutColumnName = item + "_Count"
            //        });
            //    }

            //    DataTable groupedData = filteredTable.GetGroupedBy(groups, fieldsForCalculation);
            //    this.GetRecordCountFromDataTable = groupedData.Rows.Count;
            //    var result = groupedData.AsEnumerable();
            //    var lstDatarows = result.Skip((queryParam.page_count - 1) * queryParam.page_size).Take(queryParam.page_size).ToList();

            //    var groupedList = ConvertToList(lstDatarows, wolist);

            //    //GET EXPENDED COLUMNS

            //    if (queryParam.expand != null && queryParam.expand.Count > 0)
            //    {
            //        var resultExpanded = GetExpandedRows(dbResult, result).AsEnumerable().ToList();
            //        var expandedList = ConvertToList(resultExpanded, wolist);


            //        List<DTO.ViewModels.Page.Reports.MasterWorkOrder> mergedList =
            //            new List<DTO.ViewModels.Page.Reports.MasterWorkOrder>();
            //        var expandList = queryParam.expand[0];

            //        foreach (var expandOn in expandList)
            //        {
            //            foreach (var item in groupedList)
            //            {
            //                if (!MatchExpandedProperties(expandOn, item))
            //                {
            //                    mergedList.Add(item);
            //                }
            //                else
            //                {
            //                    mergedList.AddRange(expandedList);
            //                }
            //            }
            //        }
            //    }

            //    return ConvertToList(lstDatarows, wolist);
            //}
            //else
            //{
            //    this.GetRecordCountFromDataTable = filteredTable.Rows.Count;
            //    var result = filteredTable.AsEnumerable();
            //    var lstDatarows = result.Skip((queryParam.page_count - 1) * queryParam.page_size).Take(queryParam.page_size).ToList();
            //    return ConvertToList(lstDatarows, wolist);
            //}

            this.GetRecordCountFromDataTable = filteredTable.Rows.Count;
            var result = filteredTable.AsEnumerable();
            var lstDatarows = result.Skip((param.page_count - 1) * param.page_size).Take(param.page_size).ToList();
            return ConvertToList(lstDatarows, wolist, false);
        }

        private DataTable GetActingRamMembers()
        {
            DataTable dbResult = new DataTable();
            var res = (DataTable)cacher.GetValue(memCacheKeyActingRam);
            if (res != null)
            {
                dbResult = res;
            }
            else
            {
                dbResult = Toolbox.doSQL_dt(acting_Ram_sql, null);
                cacher.Delete(this.memCacheKeyActingRam);
                cacher.Add(memCacheKeyActingRam, dbResult,
                    DateTimeOffset.UtcNow.AddMinutes(
                        Convert.ToDouble(NESI.BLL.Common.Shared.Configuration.MemCacheAbsoluteTimeout)));
            }
            return dbResult;
        }

	    public List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> GetGroupFromStore(string item)
	    {

		    return group_cacher.GetValue(GetGroupKey(item));
	    }

	    public void SetGroupStore(string item, List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> cache)
	    {
		    group_cacher.Set(GetGroupKey(item), cache);
	    }


	    public DataTable GetFilterDataFromStore(string item)
	    {
		    return cacher.GetValue(memCacheKey + "_filterData" + item);
	    }

	    public void SetFilterDataStore(string item, DataTable cache)
	    {
		    cacher.Set(memCacheKey + "_filterData" + item, cache);
	    }

	    public DataTable GetGroupSummaryFromStore(string item)
	    {
		    return cacher.GetValue(GetGroupKey(item));
	    }

	    public void SetGroupSummaryStore(string item, DataTable cache)
	    {
		    cacher.Set(GetGroupKey(item), cache);
	    }

	    public IReport GetSchemaFromStore()
	    {
		    try
		    {
			    return schema_cacher.GetValue(memCacheKey);
		    }
		    catch (Exception e)
		    {
			    Console.WriteLine(e);
			    return null;
		    }
	    }

	    public void SetSchemaStore(IReport cache)
	    {
		    schema_cacher.Set(memCacheKey, cache);
	    }


	    public DataTable GetDatafromSource()
	    {

		    var dbResult = new DataTable();
		    using (MySqlConnection conn = (MySqlConnection)_db.Database.Connection)
		    {
			    using (MySqlCommand cmd = new MySqlCommand("report_wo_master_v2", conn))
			    {
				    cmd.CommandTimeout = 400;
				    cmd.CommandType = CommandType.StoredProcedure;
				    cmd.Parameters.AddWithValue("@view_margin", this.view_margin);
				    cmd.Parameters.AddWithValue("@view_dollar_totals", this.view_dollar_totals);
				    cmd.Parameters.AddWithValue("@visible_business_units", bu_ids_string);

					conn.Open();
				    dbResult.Load(cmd.ExecuteReader(), LoadOption.Upsert);
			    }
		    }
		    cacher.Clear(memCacheKey);
		    cacher.Set(memCacheKey, dbResult);
		    return dbResult;
	    }



		//public DataTable GetDatafromSource()
  //      {
  //          DataTable dbResult = new DataTable();

  //          using (MySqlConnection conn = (MySqlConnection)_db.Database.Connection)
  //          {
  //              using (MySqlCommand cmd = new MySqlCommand("report_wo_master_v2", conn))
  //              {
  //                  cmd.CommandTimeout = 400;
  //                  cmd.CommandType = CommandType.StoredProcedure;
  //                  cmd.Parameters.AddWithValue("@view_margin", this.view_margin);
  //                  cmd.Parameters.AddWithValue("@view_dollar_totals", this.view_dollar_totals);
  //                  cmd.Parameters.AddWithValue("@visible_business_units", this.CurrentUser.VisibleBusinessUnits);

  //                  using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
  //                  {
  //                      sda.Fill(dbResult);
  //                      cacher.Delete(this.memCacheKey);
  //                      cacher.Add(memCacheKey, dbResult, DateTimeOffset.UtcNow.AddMinutes(Convert.ToDouble(NESI.BLL.Common.Shared.Configuration.MemCacheAbsoluteTimeout))); //MEM CACHE
  //                  }
  //              }
  //          }

  //          return dbResult;
  //      }


        public List<string> GetDistinct(BodyParams _param)
        {
	        var res = GetDataFromStore();
            var colName = _param.Selectby[0];
	       return (from DataRow row in res.Rows select row[colName].ToString()).Distinct().ToList();
        }

	    protected string GetGroupKey(string item)
	    {
		    var subkey = "_Group_";
		    if (param.column_groupBy != null && param.column_groupBy.Length > 0)
		    {
			    subkey = subkey + param.column_groupBy[0];
		    }
		    if (param.expand_by != null && param.expand_by.Count > 0)
		    {
			    foreach (var es in param.expand_by)
			    {
				    if (es != null && es.Length > 0)
				    {
					    foreach (var e in es)
					    {

						    subkey += e.coulumnname + "_" + e.value;

					    }
				    }
			    }

		    }
		    if (param.column_sort != null && param.column_sort.Length > 0)
		    {
			    foreach (var s in param.column_sort)
			    {
				    subkey += s.ToString();
			    }
		    }
		    return memCacheKey + subkey + param.filterBuilder + "_" + item;
	    }


		public async Task<int> EditModel(DTO.ViewModels.Page.Reports.MasterWorkOrder model)
        {
            return 0;
        }

        public async Task<DataExtra> EditModel2(DTO.ViewModels.Page.Reports.MasterWorkOrderV2 model)
        {
            int result = 0;
            string e = "";
            try
            {
                var woprogmodel = _db.woprog.First(i => i.woprog_id == model.woprog_id.Value);

                if (woprogmodel.WOProg_Status == "Deleted" || woprogmodel.WOProg_Status == OpsWOStatus.Invoiced)
                {
                    model.Okay = false;
                    return new DataExtra()
                    {
                        Data = string.Format("[{0}] work order can't be updated.", woprogmodel.WOProg_Status),
                        Extra = model
                    };
                }

                //if (model.customer != null)
                //    woprogmodel.woprog_customername = model.customer;
                if (model.description != null) // okay
                    woprogmodel.woprog_description = model.description;
                if (model.expected_sales != null) // okay
                    woprogmodel.woprog_expected_sales_value = model.expected_sales.Value;
                if (model.bvwo != null)
                    woprogmodel.woprog_bvwo = model.bvwo;
                if (model.expected_enddate != null) // okay
                    woprogmodel.woprog_expected_enddate = model.expected_enddate;
                if (model.exp_hours != null) // okay
                    woprogmodel.woprog_exp_labor = model.exp_hours;

                var notesmodel = // okay
                    _db.woprog_project_notes.FirstOrDefault(i => i.woprog_project_notes_woprogid == woprogmodel.woprog_id && i.woprog_project_notes_type == "W");
                if (notesmodel != null)
                {
                    if (model.notes != null)
                        notesmodel.woprog_project_notes_notes = model.notes;
                }
                else
                {
                    woprog_project_notes prognotes = new woprog_project_notes();

                    prognotes.woprog_project_notes_notes = model.notes;
                    prognotes.woprog_project_notes_woprogid = Convert.ToInt32(model.woprog_id.Value);
                    prognotes.woprog_project_notes_type = "W";
                    prognotes.woprog_project_notes_Datetime = DateTime.Now;
                    prognotes.woprog_project_notes_memberid = this.CurrentUser.Id;

                    _db.woprog_project_notes.Add(prognotes);
                }

                await _db.SaveChangesAsync();

                if (model.acting_ram != null) // active ram
                    bllToolbox.doSQL_void(@"UPDATE woprog SET acting_ram = @v0  WHERE woprog_id = @v1", model.acting_ram, model.woprog_id.Value);

                try
                {
                    cacher.Delete(this.memCacheKey);
                    cacher.Delete(this.memCacheKeyActingRam);
                }
                catch (Exception ex)
                {
                }
                model.Okay = true;
                result = 1;
            }
            catch (Exception ex)
            {
                e = ex.Message;
                model.Okay = false;
                result = 0;
            }

            if (result > 0)
            {
                return new DataExtra()
                {
                    Data = "Information has been saved successfully",
                    Extra = model
                };
            }
            else
            {
                return new DataExtra()
                {
                    Data = e,
                    Extra = model
                };
            }
        }

	    public Task<int> EditModel(DTO.ViewModels.Page.Reports.MasterWorkOrderV2 model)
	    {
		    throw new NotImplementedException();
	    }

	    public Task<bool> DeleteModel(int id)
        {
            throw new NotImplementedException();
        }


        public Task<int> CreateModel(DTO.ViewModels.Page.Reports.MasterWorkOrderV2 model)
        {
            throw new NotImplementedException();
        }



        public IQueryable<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> GetQueryable(Expression<Func<DTO.ViewModels.Page.Reports.MasterWorkOrderV2, bool>> match)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> GlobalSearch(string searchText, Expression<Func<DTO.ViewModels.Page.Reports.MasterWorkOrderV2, bool>> match)
        {
            throw new NotImplementedException();
        }

        public List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> GetGlobalSearchList(string searchText)
        {
            throw new NotImplementedException();
        }

        public DataTable GetDataFromStore()
        {
            var res = cacher.GetValue(memCacheKey);
            return res ?? GetDatafromSource();
        }

	 

	    //public List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> ConvertToList(List<DataRow> dtRows, List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> wolist, bool addGroupbyMeta = false, string[] metadata = null)
		//{
		//    throw new NotImplementedException();
		//}

		IQueryable<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> IModelGenerator<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>.GetQueryable(Expression<Func<DTO.ViewModels.Page.Reports.MasterWorkOrderV2, bool>> match)
        {
            throw new NotImplementedException();
        }

        IQueryable<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> IModelGenerator<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>.GlobalSearch(string searchText, Expression<Func<DTO.ViewModels.Page.Reports.MasterWorkOrderV2, bool>> match)
        {
            throw new NotImplementedException();
        }

        DataTable IModelGenerator<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>.GetDatafromSource()
        {
            throw new NotImplementedException();
        }

        //DataTable IModelGenerator<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>.GetDataFromStore()
        //{
        //    throw new NotImplementedException();
        //}

        public List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> ConvertToList(List<DataRow> dtRows, List<DTO.ViewModels.Page.Reports.MasterWorkOrderV2> wolist, bool addGroupbyMeta = false, string[] metadata = null)
        {
            var dtActingRam = GetActingRamMembers();

            foreach (DataRow row in dtRows)
            {
                DTO.ViewModels.Page.Reports.MasterWorkOrderV2 _tmp =
                    new DTO.ViewModels.Page.Reports.MasterWorkOrderV2();

                if (row.Table.Columns.Contains("woprog_id"))
                    _tmp.woprog_id = (long?)(row.IsNull("woprog_id") ? (long?)null : Convert.ToInt64(row["woprog_id"]));
                if (row.Table.Columns.Contains("assoc_id"))
                    _tmp.assoc_id = (string)(row.IsNull("assoc_id") ? null : row["assoc_id"].ToString().Trim());
                if (row.Table.Columns.Contains("business_unit"))
                    _tmp.business_unit = (string)(row.IsNull("business_unit") ? null : Convert.ToString(row["business_unit"]));
                if (row.Table.Columns.Contains("BVWO"))
                    _tmp.bvwo = (string)(row.IsNull("BVWO") ? null : Convert.ToString(row["BVWO"]));
                if (row.Table.Columns.Contains("business_unit_id"))
                    _tmp.business_unit_id = (int?)(row.IsNull("business_unit_id") ? (int?)null : Convert.ToInt32(row["business_unit_id"]));
                if (row.Table.Columns.Contains("Customer"))
                    _tmp.customer = (string)(row.IsNull("Customer") ? null : Convert.ToString(row["Customer"]));
                if (row.Table.Columns.Contains("Description"))
                    _tmp.description = (string)(row.IsNull("Description") ? null : Convert.ToString(row["Description"].ToString()));
                if (row.Table.Columns.Contains("Quote"))
                    _tmp.quote = (string)(row.IsNull("Quote") ? null : Convert.ToString(row["Quote"]));
                if (row.Table.Columns.Contains("Invoiced_Amount"))
                    _tmp.invoiced_amount = (double?)(row.IsNull("Invoiced_Amount") ? (double?)null : Convert.ToDouble(row["Invoiced_Amount"]));
                if (row.Table.Columns.Contains("QuotedAmount"))
                    _tmp.quotedamount = (double?)(row.IsNull("QuotedAmount") ? (double?)null : Convert.ToDouble(row["QuotedAmount"]));
                if (row.Table.Columns.Contains("JobCost"))
                    _tmp.jobcost = (double?)(row.IsNull("JobCost") ? (double?)null : Convert.ToDouble(row["JobCost"]));
                if (row.Table.Columns.Contains("Remainder"))
                    _tmp.remainder = (double?)(row.IsNull("Remainder") ? (double?)null : Convert.ToDouble(row["Remainder"]));
                if (row.Table.Columns.Contains("to_be_billed"))
                    _tmp.to_be_billed = (double?)(row.IsNull("to_be_billed") ? (double?)null : Convert.ToDouble(row["to_be_billed"]));
                if (row.Table.Columns.Contains("WOID"))
                    _tmp.woid = (long?)(row.IsNull("WOID") ? (long?)null : Convert.ToInt64(row["WOID"]));
                if (row.Table.Columns.Contains("Cut_Date"))
                    _tmp.cut_date = (DateTime?)(row.IsNull("Cut_Date") ? (DateTime?)null : Convert.ToDateTime(row["Cut_Date"]));
                if (row.Table.Columns.Contains("Status"))
                    _tmp.status = (string)(row.IsNull("Status") ? null : Convert.ToString(row["Status"]));
                if (row.Table.Columns.Contains("pm"))
                    _tmp.pm = (string)(row.IsNull("pm") ? null : Convert.ToString(row["pm"]));
                if (row.Table.Columns.Contains("expected_enddate"))
                    _tmp.expected_enddate = (DateTime?)(row.IsNull("expected_enddate") ? (DateTime?)null : Convert.ToDateTime(row["expected_enddate"]));
                if (row.Table.Columns.Contains("expected_startdate"))
                    _tmp.expected_startdate = (DateTime?)(row.IsNull("expected_startdate") ? (DateTime?)null : Convert.ToDateTime(row["expected_startdate"]));
                if (row.Table.Columns.Contains("expected_sales"))
                    _tmp.expected_sales = (decimal?)(row.IsNull("expected_sales") ? (decimal?)null : Convert.ToDecimal(row["expected_sales"]));
                if (row.Table.Columns.Contains("progress"))
                    _tmp.progress = (double?)(row.IsNull("progress") ? (double?)null : Convert.ToDouble(row["progress"]));
                if (row.Table.Columns.Contains("invoice_date"))
                    _tmp.invoice_date = (DateTime?)(row.IsNull("invoice_date") ? (DateTime?)null : Convert.ToDateTime(row["invoice_date"]));
                if (row.Table.Columns.Contains("notes"))
                    _tmp.notes = (string)(row.IsNull("notes") ? null : Convert.ToString(row["notes"]));
                if (row.Table.Columns.Contains("completion"))
                    _tmp.completion = (double?)(row.IsNull("completion") ? (double?)null : Convert.ToDouble(row["completion"]));
                if (row.Table.Columns.Contains("account_manager"))
                    _tmp.account_manager = (string)(row.IsNull("account_manager") ? null : Convert.ToString(row["account_manager"]));
                if (row.Table.Columns.Contains("tmmargin"))
                    _tmp.tmmargin = (double?)(row.IsNull("tmmargin") ? (double?)null : Convert.ToDouble(row["tmmargin"]));
                if (row.Table.Columns.Contains("grossmargin"))
                    _tmp.grossmargin = (double?)(row.IsNull("grossmargin") ? (double?)null : Convert.ToDouble(row["grossmargin"]));
                if (row.Table.Columns.Contains("progressbilling"))
                    _tmp.progressbilling = (double?)(row.IsNull("progressbilling") ? (double?)null : Convert.ToDouble(row["progressbilling"]));
                if (row.Table.Columns.Contains("laborcost"))
                    _tmp.laborcost = (decimal?)(row.IsNull("laborcost") ? (decimal?)null : Convert.ToDecimal(row["laborcost"]));
                if (row.Table.Columns.Contains("materialcost"))
                    _tmp.materialcost = (decimal?)(row.IsNull("materialcost") ? (decimal?)null : Convert.ToDecimal(row["materialcost"]));
                if (row.Table.Columns.Contains("woprog_last_email_date"))
                    _tmp.woprog_last_email_date = (DateTime?)(row.IsNull("woprog_last_email_date") ? (DateTime?)null : Convert.ToDateTime(row["woprog_last_email_date"]));
                if (row.Table.Columns.Contains("woprog_invoiceno"))
                    _tmp.woprog_invoiceno = (string)(row.IsNull("woprog_invoiceno") ? null : Convert.ToString(row["woprog_invoiceno"]));
                if (row.Table.Columns.Contains("days_since_cut"))
                    _tmp.days_since_cut = (int?)(row.IsNull("days_since_cut") ? (int?)null : Convert.ToInt32(row["days_since_cut"]));
                if (row.Table.Columns.Contains("days_since_inv"))
                    _tmp.days_since_inv = (int?)(row.IsNull("days_since_inv") ? (int?)null : Convert.ToInt32(row["days_since_inv"]));
                if (row.Table.Columns.Contains("sc"))
                    _tmp.sc = (bool?)(row.IsNull("sc") ? (bool?)null : (Convert.ToInt32(row["sc"]) > 0 ? true : false));
                if (row.Table.Columns.Contains("po"))
                    _tmp.po = (string)(row.IsNull("po") ? null : row["po"].ToString().Trim());
                if (row.Table.Columns.Contains("rd"))
                    _tmp.rd = (bool?)(row.IsNull("rd") ? (bool?)null : (Convert.ToInt32(row["rd"]) > 0 ? true : false));
                if (row.Table.Columns.Contains("gross_profit"))
                    _tmp.gross_profit = (double?)(row.IsNull("gross_profit") ? (double?)null : Convert.ToDouble(row["gross_profit"]));
                if (row.Table.Columns.Contains("wo_total"))
                    _tmp.wo_total = (double?)(row.IsNull("wo_total") ? (double?)null : Convert.ToDouble(row["wo_total"]));
                if (row.Table.Columns.Contains("hold"))
                    _tmp.hold = (bool?)(row.IsNull("hold") ? (bool?)null : (Convert.ToInt16(row["hold"]) > 0 ? true : false));
                if (row.Table.Columns.Contains("asset"))
                    _tmp.asset = (string)(row.IsNull("asset") ? null : Convert.ToString(row["asset"]));
                if (row.Table.Columns.Contains("city"))
                    _tmp.city = (string)(row.IsNull("city") ? null : Convert.ToString(row["city"]));
                if (row.Table.Columns.Contains("province"))
                    _tmp.province = (string)(row.IsNull("province") ? null : Convert.ToString(row["province"]));
                if (row.Table.Columns.Contains("service_addr"))
                    _tmp.service_addr = (string)(row.IsNull("service_addr") ? null : Convert.ToString(row["service_addr"]));
                if (row.Table.Columns.Contains("customer_req_po"))
                    _tmp.customer_req_po = (string)(row.IsNull("customer_req_po") ? null : Convert.ToString(row["customer_req_po"]));
                if (row.Table.Columns.Contains("days_till_start"))
                    _tmp.days_till_start = (int?)(row.IsNull("days_till_start") ? (int?)null : Convert.ToInt32(row["days_till_start"]));
                if (row.Table.Columns.Contains("days_till_close"))
                    _tmp.days_till_close = (int?)(row.IsNull("days_till_close") ? (int?)null : Convert.ToInt32(row["days_till_close"]));
                if (row.Table.Columns.Contains("exp_hours"))
                    _tmp.exp_hours = (double?)(row.IsNull("exp_hours") ? (double?)null : Convert.ToDouble(row["exp_hours"]));
                if (row.Table.Columns.Contains("invoice_lag"))
                    _tmp.invoice_lag = (double?)(row.IsNull("invoice_lag") ? (double?)null : Convert.ToDouble(row["invoice_lag"]));
                if (row.Table.Columns.Contains("warranty"))
                    _tmp.warranty = (int?)(row.IsNull("warranty") ? (int?)null : Convert.ToInt32(row["warranty"]));
                if (row.Table.Columns.Contains("scanned_date"))
                    _tmp.scanned_date = (DateTime?)(row.IsNull("scanned_date") ? (DateTime?)null : Convert.ToDateTime(row["scanned_date"]));
                if (row.Table.Columns.Contains("days_late"))
                    _tmp.days_late = (int?)(row.IsNull("days_late") ? (int?)null : Convert.ToInt32(row["days_late"]));
                if (row.Table.Columns.Contains("country"))
                    _tmp.country = (string)(row.IsNull("country") ? null : Convert.ToString(row["country"]));
                if (row.Table.Columns.Contains("inspection_link"))
                    _tmp.inspection_link = (string)(row.IsNull("inspection_link") ? null : Convert.ToString(row["inspection_link"]));
                if (row.Table.Columns.Contains("inspection_required"))
                    _tmp.inspection_required = (bool?)(row.IsNull("inspection_required") ? (bool?)null : (Convert.ToInt16(row["inspection_required"]) > 0 ? true : false));
                if (row.Table.Columns.Contains("last_date_worked"))
                    _tmp.last_date_worked = (DateTime?)(row.IsNull("last_date_worked") ? (DateTime?)null : Convert.ToDateTime(row["last_date_worked"]));
                if (row.Table.Columns.Contains("total_hours_worked"))
                    _tmp.total_hours_worked = (double?)(row.IsNull("total_hours_worked") ? (double?)null : Convert.ToDouble(row["total_hours_worked"]));
                if (row.Table.Columns.Contains("woprog_associate_woprog_id"))
                    _tmp.woprog_associate_woprog_id = (string)(row.IsNull("woprog_associate_woprog_id") ? null : Convert.ToString(row["woprog_associate_woprog_id"]));
                if (row.Table.Columns.Contains("days_in_bucket"))
                    _tmp.days_in_bucket = (int?)(row.IsNull("days_in_bucket") ? (int?)null : Convert.ToInt32(row["days_in_bucket"]));
                if (row.Table.Columns.Contains("ram"))
                    _tmp.ram = (string)(row.IsNull("ram") ? null : Convert.ToString(row["ram"]));
                if (row.Table.Columns.Contains("laborcost"))
                    _tmp.laborcost = (decimal?)(row.IsNull("laborcost") ? (decimal?)null : Convert.ToDecimal(row["laborcost"]));
                if (row.Table.Columns.Contains("materialcost"))
                    _tmp.materialcost = (decimal?)(row.IsNull("materialcost") ? (decimal?)null : Convert.ToDecimal(row["materialcost"]));
                if (row.Table.Columns.Contains("hours_left"))
                    _tmp.hours_left = (double?)(row.IsNull("hours_left") ? (double?)null : Convert.ToDouble(row["hours_left"]));

                if (row.Table.Columns.Contains("acting_ram"))
                    _tmp.acting_ram = (string)(row.IsNull("acting_ram") ? "0" : Convert.ToString(row["acting_ram"]));

                if (row.Table.Columns.Contains("open_pos"))
                    _tmp.open_pos = Convert.ToBoolean(row["open_pos"]);
                if (row.Table.Columns.Contains("quote_costs"))
                    _tmp.quote_costs = (double?)(row.IsNull("quote_costs") ? (double?)null : Convert.ToDouble(row["quote_costs"]));
                if (row.Table.Columns.Contains("sustainability_project"))
                    _tmp.sustainability_project = (int?)(row.IsNull("sustainability_project") ? (int?)null : Convert.ToInt32(row["sustainability_project"]));

                if (row.Table.Columns.Contains("tm_sell"))
                    _tmp.tm_sell = (decimal?)(row.IsNull("tm_sell") ? (decimal?)null : Convert.ToDecimal(row["tm_sell"]));

                if (row.Table.Columns.Contains("hours_spent"))
                    _tmp.hours_spent = (double?)(row.IsNull("hours_spent") ? (double?)null : Convert.ToDouble(row["hours_spent"]));

                if (row.Table.Columns.Contains("isr"))
                    _tmp.isr = (string)(row.IsNull("isr") ? null : Convert.ToString(row["isr"]));

                if (row.Table.Columns.Contains("jobtag"))
                    _tmp.jobtag = (string)(row.IsNull("jobtag") ? null : Convert.ToString(row["jobtag"]));

                if (row.Table.Columns.Contains("quotedhours"))
                    _tmp.quotedhours =  row.IsNull("quotedhours") ? null : (decimal?)row["quotedhours"];
                if (row.Table.Columns.Contains("controls_person"))
                    _tmp.controls_person = (string)(row.IsNull("controls_person") ? null : Convert.ToString(row["controls_person"]));
                if (param.column_groupBy != null && param.column_groupBy.Length > 0 && addGroupbyMeta)
                {
                    _tmp.Metadata = new List<string>();
                    for (int i = param.column_groupBy.Length; i < dtRows[0].Table.Columns.Count; i++)
                    {
                        _tmp.Metadata.Add(Convert.ToString(row[i]));
                    }
                }
                else
                {

                }
                wolist.Add(_tmp);
            }



            return wolist;
        }

        Task<int> IModelEditor<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>.CreateModel(DTO.ViewModels.Page.Reports.MasterWorkOrderV2 model)
        {
            throw new NotImplementedException();
        }

        Task<int> IModelEditor<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>.EditModel(DTO.ViewModels.Page.Reports.MasterWorkOrderV2 model)
        {
            throw new NotImplementedException();
        }

        Task<string> IModelEditor<DTO.ViewModels.Page.Reports.MasterWorkOrderV2>.DeleteModel(int id)
        {
            throw new NotImplementedException();
        }

        private string SelectCommand = @"SELECT
		0 Member_ID,
		'No One' member_fullname


union


    Select* from(SELECT
        member.member_id,
        concat(
            member_fullname,
			'(',
            bu.ddl_name,
			')'
		) member_fullname
    FROM

        member
    INNER JOIN business_unit bu ON bu.id = member.business_unit_id
    WHERE

        member_membertype_id IN(
			8,
			11,
			27,
			34,
			35,
			37,
			38,
			39,
			46,
			49,
			52,
			53,
			54,
			55,
			56,
			59,
			4,
			5,
			12,
			17,
			24,
			25,
			41
		)

    AND member_status = 'Active'

    ORDER BY

        bu.ddl_name asc, member_fullname)a";

        public LabelValueInt[] getAllRams()
        {
            List<LabelValueInt> result = new List<LabelValueInt>();

            var data = bllToolbox.doSQL_dt(this.SelectCommand);

            foreach (DataRow _subdr in data.Rows)
            {
                var id = _subdr[0].ToString();
                var name = _subdr[1].ToString();

                result.Add(new LabelValueInt() { Label = name, Value = int.Parse(id) });
            }

            return result.ToArray();
        }

        public object GetProcessExtra()
        {
            return null;
        }
    }
}
