using MySql.Data.MySqlClient;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.Common.Interface;
using NESI.DTO.ViewModels.Page.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{
    public class AdvancePayReport :  BLLBase, IModelGenerator<DTO.ViewModels.Page.Reports.AdvancePayReportGrid>, IModelEditor<DTO.ViewModels.Page.Reports.AdvancePayReportGrid>
    {
        #region query from aspx page
        private string selectCommand = @"SELECT
woprog.business_unit_id AS ```Parent Branch```,
bu.ddl_name AS ParentBranch,
woprog.woprog_id AS JobCostWO,
woprog.woprog_customername AS Customer,
woprog.woprog_department AS `'Main WO Dept'`,
        woprog.woprog_description description,
left(woprog.woprog_quoteid, LENGTH(trim(BOTH '' from woprog.woprog_quoteid)) - 1) AS q_id,
  right(woprog.woprog_quoteid, 1) AS rev,
   round(Get_QuoteWorksheet_Cost(if(left(woprog.woprog_quoteid, LENGTH(trim(BOTH '' from woprog.woprog_quoteid)) - 1) = '', 0, left(woprog.woprog_quoteid, LENGTH(trim(BOTH '' from woprog.woprog_quoteid)) - 1))
   , right(woprog.woprog_quoteid, 1)), 2) AS Quoted_Costs,
      woprog.WOProg_LaborCost AS Main_Labour_Cost,
woprog.WOProg_MaterialCost AS Main_Material_Cost,
wo_child.woprog_company_id AS `'Div or Child Branch'`,
bu_branch.ddl_name AS divorChildBusinessUnit,
wo_child.woprog_customername AS DivorChildCustomerName,
wo_child.woprog_invoicedate AS Invoicedate,
if(left(wo_child.woprog_customername,3)='DIV',wo_child.woprog_id,'') AS divtoDivWO,
if(left(wo_child.woprog_customername,3)='DIV',wo_child.WOProg_Status,'') AS divtoDivWOStatus,
if(left(wo_child.woprog_customername,3)='DIV',wo_child.this_wo_labour_bench,0) AS divtoDivTMLabourSell,
if(left(wo_child.woprog_customername,3)='DIV',wo_child.this_wo_material_bench,'') AS divtoDivTMMaterialSell,
wo_child.woprog_glposting_instructions AS postingNotes,
if(left(wo_child.woprog_customername,3)!='DIV',wo_child.woprog_id,'') AS childWO,
if(left(wo_child.woprog_customername,3)!='DIV',wo_child.WOProg_Status,'') AS childWOStatus,
if(left(wo_child.woprog_customername,3)!='DIV' AND woprog.woprog_quoteid = '0', wo_child.this_wo_labour_bench,0) AS childTMLabourSell,
 if(left(wo_child.woprog_customername,3)!='DIV' AND woprog.woprog_quoteid = '0', wo_child.this_wo_material_bench,0) AS childMMaterialSell,
  wo_child.woprog_department AS ```Div or Child WO Dept```,
woprog.woprog_status as main_wo_status,
woprog.WOProg_QuotedAmount quoted_price,
        (Select sum(a.WOProg_InvoicedNetTotal) from woprog a where a.WOProg_Associate_WOProg_ID = woprog.woprog_id) progress_billed_so_far,
woprog.woprog_expected_startdate startdate,
woprog.woprog_expected_enddate enddate,
        woprog.WOProg_InvoiceDate,
        woprog.WOProg_QuotedAmount-(Select sum(a.WOProg_InvoicedNetTotal) from woprog a where a.WOProg_Associate_WOProg_ID = woprog.woprog_id) final_invoiced

FROM
woprog
INNER JOIN business_unit AS bu ON woprog.business_unit_id = bu.id
LEFT JOIN woprog AS wo_child ON woprog.woprog_id = wo_child.parent_woprog_id
LEFT JOIN business_unit AS bu_branch ON wo_child.woprog_company_id = bu_branch.id

WHERE
((wo_child.WOProg_InvoiceDate >= '2017-01-01')
or
((select count(b.woprog_id) from woprog b where b.WOProg_Associate_WOProg_ID= woprog.woprog_id and b.WOProg_Associate_WOProg_ID!=0)>0  )
or(woprog.WOProg_InvoiceDate>='2017-01-01' and woprog.woprog_quoteid>0)
) AND find_in_set(woprog.business_unit_id,'{bu_ids}')
ORDER BY
woprog.woprog_bvwo ASC";
        #endregion

        public List<string> SetColumnList { get; set; }
        public Dictionary<string, string> GetColumnSummary { get; set; }
        public int GetRecordCountFromDataTable { get; set; }
        public object GroupSummary { get; set; }
        public object CaculationsOnAllColumns { get; set; }

        protected readonly BLL.Common.Cache.DatatableCacher cacher = BLL.Common.Cache.Global.Datatable;
	    protected BLL.Common.Cache.IReportCacher schema_cacher = BLL.Common.Cache.Global.IReport;

	    protected string memCacheKey => typeof(AdvancePayReport) + CurrentUser.Guid + CurrentUser.Id + "_" + query_params_string();
        private readonly string memCacheKeyBV = "";
        private readonly BodyParams param;

        public AdvancePayReport(Employee currentUser, BodyParams param) : base(currentUser)
        {
            this.param = param;
            this.memCacheKeyBV = "memCacheKeyBV_" + this.CurrentUser.Id;

	        if (param?.refreshCache == null || !param.refreshCache.Value) return;
	        cacher.Clear(this.memCacheKey);
	        cacher.Clear(this.memCacheKeyBV);
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
			    if (string.IsNullOrEmpty(param.bu_ids))
			    {
				    return "[N]";
			    }
			    else
			    {
				    return param.bu_ids.Substring(0, 3);
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

	    public DataTable GetFilterDataFromStore(string item)
	    {
		    return null;
	    }

	    public void SetFilterDataStore(string item, DataTable cache)
	    {
	    }

		public IReport GetSchemaFromStore()
	    {
		    return null;
		}

	    public List<AdvancePayReportGrid> GetGroupFromStore(string item)
	    {
		    return null;
	    }

	    public void SetGroupStore(string item,  List<AdvancePayReportGrid> cache)
	    {
		  
	    }

	    public void SetSchemaStore(IReport cache)
	    {

	    }

		public List<AdvancePayReportGrid> ConvertToList(List<DataRow> dtRows, List<AdvancePayReportGrid> wolist, bool addGroupbyMeta = false, string[] metadata = null)
        {
            foreach (DataRow row in dtRows)
            {
                DTO.ViewModels.Page.Reports.AdvancePayReportGrid _tmp = new DTO.ViewModels.Page.Reports.AdvancePayReportGrid();
                try
                {
                   
                    if (row.Table.Columns.Contains("JobCostWO"))
                        _tmp.woprog_id = (long)(row.IsNull("JobCostWO") ? (long?)null : Convert.ToInt64(row["JobCostWO"]));

                    if (row.Table.Columns.Contains("ParentBranch"))
                        _tmp.parentBranch = (string)(row.IsNull("ParentBranch") ? null : Convert.ToString(row["ParentBranch"]));
                    if (row.Table.Columns.Contains("JobCostWO"))
                        _tmp.jobCostWO = (long)(row.IsNull("JobCostWO") ? (long?)null : Convert.ToInt64(row["JobCostWO"]));

                    if (row.Table.Columns.Contains("customer"))
                        _tmp.customer = (string)(row.IsNull("customer") ? null : Convert.ToString(row["customer"]));
                    if (row.Table.Columns.Contains("q_id"))
                        _tmp.q_id = (string)(row.IsNull("q_id") ? null : Convert.ToString(row["q_id"]));
                    if (row.Table.Columns.Contains("Quoted_Costs"))
                        _tmp.quoted_Costs = (double?)(row.IsNull("Quoted_Costs") ? (double?)null : Convert.ToDouble(row["Quoted_Costs"]));
                    if (row.Table.Columns.Contains("Main_Labour_Cost"))
                        _tmp.main_Labour_Cost = (double?)(row.IsNull("Main_Labour_Cost") ? (double?)null : Convert.ToDouble(row["Main_Labour_Cost"]));
                    if (row.Table.Columns.Contains("Main_Material_Cost"))
                        _tmp.main_Material_Cost = (double?)(row.IsNull("Main_Material_Cost") ? (double?)null : Convert.ToDouble(row["Main_Material_Cost"]));
                    if (row.Table.Columns.Contains("divorChildBusinessUnit"))
                        _tmp.divorChildBusinessUnit = (string)(row.IsNull("divorChildBusinessUnit") ? null : Convert.ToString(row["divorChildBusinessUnit"]));
                    if (row.Table.Columns.Contains("DivorChildCustomerName"))
                        _tmp.divorChildCustomerName = (string)(row.IsNull("DivorChildCustomerName") ? null : Convert.ToString(row["DivorChildCustomerName"]));
                    if (row.Table.Columns.Contains("Invoicedate"))
                        _tmp.invoiceDate = (DateTime?)(row.IsNull("Invoicedate") ? (DateTime?)null : Convert.ToDateTime(row["Invoicedate"]));

                    if (row.Table.Columns.Contains("divtoDivWO"))
                        _tmp.divtoDivWO = (string)(row.IsNull("divtoDivWO") ? null : Convert.ToString(row["divtoDivWO"]));
                    if (row.Table.Columns.Contains("divtoDivWOStatus"))
                        _tmp.divtoDivWOStatus = (string)(row.IsNull("divtoDivWOStatus") ? null : Convert.ToString(row["divtoDivWOStatus"]));
                    if (row.Table.Columns.Contains("divtoDivTMLabourSell"))
                        _tmp.divtoDivTMLabourSell = (double?)(row.IsNull("divtoDivTMLabourSell") ? (double?)null : Convert.ToDouble(row["divtoDivTMLabourSell"]));
                    if (row.Table.Columns.Contains("divtoDivTMMaterialSell"))
                        _tmp.divtoDivTMMaterialSell = (string)(row.IsNull("divtoDivTMMaterialSell") ? null : Convert.ToString(row["divtoDivTMMaterialSell"]));

                    if (row.Table.Columns.Contains("postingNotes"))
                        _tmp.postingNotes = (string)(row.IsNull("postingNotes") ? null : Convert.ToString(row["postingNotes"]));
                    if (row.Table.Columns.Contains("childWO"))
                        _tmp.childWO = (string)(row.IsNull("childWO") ? null : Convert.ToString(row["childWO"]));
                    if (row.Table.Columns.Contains("childWOStatus"))
                        _tmp.childWOStatus = (string)(row.IsNull("childWOStatus") ? null : Convert.ToString(row["childWOStatus"]));
                    if (row.Table.Columns.Contains("childTMLabourSell"))
                        _tmp.childTMLabourSell = (double?)(row.IsNull("childTMLabourSell") ? (double?)null : Convert.ToDouble(row["childTMLabourSell"]));
                    if (row.Table.Columns.Contains("childMMaterialSell"))
                        _tmp.childMMaterialSell = (double?)(row.IsNull("childMMaterialSell") ? (double?)null : Convert.ToDouble(row["childMMaterialSell"]));

                    if (row.Table.Columns.Contains("main_wo_status"))
                        _tmp.main_wo_status = (string)(row.IsNull("main_wo_status") ? null : Convert.ToString(row["main_wo_status"]));
                    if (row.Table.Columns.Contains("quoted_price"))
                        _tmp.quoted_price = (double?)(row.IsNull("quoted_price") ? (double?)null : Convert.ToDouble(row["quoted_price"]));
                    if (row.Table.Columns.Contains("progress_billed_so_far"))
                        _tmp.progress_billed_so_far = (double?)(row.IsNull("progress_billed_so_far") ? (double?)null : Convert.ToDouble(row["progress_billed_so_far"]));

                    if (row.Table.Columns.Contains("startdate"))
                        _tmp.startdate = (DateTime?)(row.IsNull("startdate") ? (DateTime?)null : Convert.ToDateTime(row["startdate"]));
                    if (row.Table.Columns.Contains("enddate"))
                        _tmp.enddate = (DateTime?)(row.IsNull("enddate") ? (DateTime?)null : Convert.ToDateTime(row["enddate"]));
                    if (row.Table.Columns.Contains("wOProg_InvoiceDate"))
                        _tmp.wOProg_InvoiceDate = (DateTime?)(row.IsNull("wOProg_InvoiceDate") ? (DateTime?)null : Convert.ToDateTime(row["wOProg_InvoiceDate"]));
                    if (row.Table.Columns.Contains("description"))
                        _tmp.description = (string)(row.IsNull("description") ? null : Convert.ToString(row["description"]));
                    if (row.Table.Columns.Contains("final_invoiced"))
                        _tmp.final_invoiced = (double?)(row.IsNull("final_invoiced") ? (double?)null : Convert.ToDouble(row["final_invoiced"]));

                    if (row.Table.Columns.Contains("rev"))
                        _tmp.rev = (row.IsNull("rev") ? (int?)null : Convert.ToInt32(row["rev"]));
                }
                catch(Exception ex)
                {
                    string e = ex.Message;
                }



                if (metadata == null)
                {
                    if (param.column_groupBy != null && param.column_groupBy.Length > 0 && addGroupbyMeta)
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

        public List<string> GetDistinct(BodyParams param)
        {
            DataTable dbResult = new DataTable();
            List<DTO.ViewModels.Page.Reports.AdvancePayReportGrid> wolist = new List<DTO.ViewModels.Page.Reports.AdvancePayReportGrid>();
            var res = (DataTable)cacher.GetValue(memCacheKey);
            if (res != null)
            {
                dbResult = res;
            }
            else
            {
                dbResult = GetDatafromSource();
            }

            var colName = param.Selectby[0];


            DataView view = new DataView(dbResult);
            DataTable distinctValues = view.ToTable(true, colName);

            var result = distinctValues.AsEnumerable();
            var lstDatarows = result.Skip((param.page_count - 1) * param.page_size).Take(param.page_size).ToList();

            List<string> strDistinct = new List<string>();
            foreach (DataRow row in lstDatarows)
                if (row[0] != DBNull.Value)
                    strDistinct.Add(row[0].ToString().Trim());

            //distinctValues.AsEnumerable().ToList<string>();
            return strDistinct;
        }

        public Task<int> CreateModel(AdvancePayReportGrid model)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteModel(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditModel(AdvancePayReportGrid model)
        {
            throw new NotImplementedException();
        }


	    public DataTable GetDatafromSource()
        {
            DataTable dbResult = new DataTable();

	        if (bu_ids_type == "[S]")
	        {
		        selectCommand = selectCommand.Replace("{bu_ids}", bu_ids_string);
	        }

	        using (MySqlConnection conn = (MySqlConnection)_db.Database.Connection)
            {
                using (MySqlCommand cmd = new MySqlCommand(this.selectCommand, conn))
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

        public IQueryable<AdvancePayReportGrid> GetQueryable(Expression<Func<AdvancePayReportGrid, bool>> match)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AdvancePayReportGrid> GlobalSearch(string searchText, Expression<Func<AdvancePayReportGrid, bool>> match)
        {
            throw new NotImplementedException();
        }

        public object ProcessExtra(DataTable table)
        {
            return null;
        }

        public object GetProcessExtra()
        {
            return null;
        }
    }
}
