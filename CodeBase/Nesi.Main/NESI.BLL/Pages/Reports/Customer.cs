
using System.Collections.Generic;
using NESI.Data.Entities;
using System;
using System.Linq;
using NESI.Common.Interface;
using System.Linq.Expressions;
using NESI.BLL.Base;
using MySql.Data.MySqlClient;
using NESI.DTO.ViewModels.Page.Reports;
using NESI.Common;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{
    public class Customer : BLLBase, IModelGenerator<DTO.ViewModels.Page.Reports.Customer>
    {
        public List<string> SetColumnList { get; set; }
        public int GetRecordCountFromDataTable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dictionary<string, string> GetColumnSummary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object GroupSummary { get; set; }
        public object CaculationsOnAllColumns { get; set; }
	    public Task<int> CreateModel(DTO.ViewModels.Page.Reports.Customer model)
	    {
		    throw new NotImplementedException();
	    }

	    public Task<int> EditModel(DTO.ViewModels.Page.Reports.Customer model)
	    {
		    throw new NotImplementedException();
	    }

	    public Task<bool> DeleteModel(int id)
	    {
		    throw new NotImplementedException();
	    }

	    public DataTable GetDatafromSource()
        {
	        return null;
        }

        public List<DTO.ViewModels.Page.Reports.Customer> GetDataTableList()
        {
            throw new NotImplementedException();
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
		public List<DTO.ViewModels.Page.Reports.Customer> GetGlobalSearchList(string searchText)
        {
            throw new NotImplementedException();
        }

	    public object ProcessExtra(DataTable table)
	    {
		    return null;
	    }

	    public void SetGroupStore(string item,  List<DTO.ViewModels.Page.Reports.Customer> cache)
	    {
		    
	    }

	    public void SetSchemaStore(IReport cache)
	    {

	    }

	    public List<string> GetDistinct(BodyParams _param)
	    {
		    var res = GetDataFromStore();
		    var colName = _param.Selectby[0];
		    return (res.Rows.Cast<DataRow>().Select(row => row[colName].ToString())).Distinct().ToList();
	    }

		public List<NESI.DTO.ViewModels.Page.Reports.Customer> GetCustomers(string filter)
        {
            List<NESI.DTO.ViewModels.Page.Reports.Customer> cust = new List<DTO.ViewModels.Page.Reports.Customer>();

            string query = @"SELECT customer_id, customer_name FROM CUSTOMER WHERE customer_name LIKE '%{0}%'";

            DataTable dbResult = new DataTable();
            try
            {
                using (MySqlConnection conn = (MySqlConnection)_db.Database.Connection)
                {
                    using (MySqlCommand cmd = new MySqlCommand(String.Format(query, filter), conn))
                    {
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.Text;
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            sda.Fill(dbResult);
                        }
                    }
                }

                cust = ConvertDataTable(dbResult);
                if (cust != null && cust.Count > 0)
                {
                    return cust = cust.Select(c => new NESI.DTO.ViewModels.Page.Reports.Customer { CustomerID = c.CustomerID, CustomerName = c.CustomerName }).ToList();
                }
            }
            catch (System.Exception ex)
            {

            }
            return cust.Select(c => new NESI.DTO.ViewModels.Page.Reports.Customer { CustomerID = c.CustomerID, CustomerName = c.CustomerName }).ToList();
        }

        private List<DTO.ViewModels.Page.Reports.Customer> ConvertDataTable(DataTable dt)
        {
            List<DTO.ViewModels.Page.Reports.Customer> data = new List<DTO.ViewModels.Page.Reports.Customer>();
            foreach (DataRow row in dt.Rows)
            {
                //T item = GetItem<T>(row);
                data.Add(new DTO.ViewModels.Page.Reports.Customer()
                {
                    CustomerID = Convert.ToInt32(row["customer_id"]),
                    CustomerName = Convert.ToString(row["customer_name"])
                });
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        public IQueryable<DTO.ViewModels.Page.Reports.Customer> GetQueryable(Expression<Func<DTO.ViewModels.Page.Reports.Customer, bool>> match = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DTO.ViewModels.Page.Reports.Customer> GlobalSearch(string searchText, Expression<Func<DTO.ViewModels.Page.Reports.Customer, bool>> match)
        {
            throw new NotImplementedException();
        }

        public DataTable GetDataFromStore()
        {
			return null;
		}

	    public IReport GetSchemaFromStore()
	    {
		    return null;
	    }

	    public List<DTO.ViewModels.Page.Reports.Customer> GetGroupFromStore(string item)
	    {
		    return null;
	    }

	    public List<DTO.ViewModels.Page.Reports.Customer> ConvertToList(List<DataRow> dtRows, List<DTO.ViewModels.Page.Reports.Customer> wolist, bool addGroupbyMeta = false, string[] metadata = null)
        {
            throw new NotImplementedException();
        }

        public object GetProcessExtra()
        {
            return null;
        }
    }
}
