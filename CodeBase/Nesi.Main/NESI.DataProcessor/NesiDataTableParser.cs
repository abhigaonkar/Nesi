using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DataProcessor
{
    public class NesiDataTableParser
    {
        public DataTable GetDataTableFromObjects<TDataClass>(List<TDataClass> dataList)
       where TDataClass : class
        {
            try
            {            
            Type t = typeof(TDataClass);
            DataTable dt = new DataTable(t.Name);
            foreach (PropertyInfo pi in t.GetProperties())
            {
                dt.Columns.Add(new DataColumn(pi.Name));
            }
            if (dataList != null)
            {
                foreach (TDataClass item in dataList)
                {
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dr[dc.ColumnName] =
                          item.GetType().GetProperty(dc.ColumnName).GetValue(item, null);
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));
                    }
                }
                return objT;
            }).ToList();
        }
        public IList<Type> GetColumnTypes(DataTable Table)
        {
            IList<Type> types = new List<Type>();
            DataRow row = Table.Rows[0];
            foreach (object item in row.ItemArray)
            {
                types.Add(item.GetType());
            }
            return types;
        }
        
    }
}
