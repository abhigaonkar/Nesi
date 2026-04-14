//using NESI.Common;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NESI.DataProcessor
//{
//    public class DataProcesserHelper
//    {
//        public static string GetProcessedData<T, T1>(QueryParam param, T custbl) where T : NESI.Common.IPageModelGenerator<T1>
//            where T1 : class
//        {
//            DataTable dtSummary = null;
//            string data;
//            var unionedDataTable = GetProcessedDataTables<T, T1>(param, custbl, out dtSummary);
//            var summaryData = JsonConvert.SerializeObject(dtSummary);

//            data = JsonConvert.SerializeObject(unionedDataTable);

//            return "{\"data\":" + data + ",\"summary\":" + summaryData + "}";
//        }

//        public static DataTable GetProcessedDataTables<T, T1>(QueryParam param, T custbl, out DataTable dtSummary) where T : NESI.Common.IPageModelGenerator<T1>
//         where T1 : class

//        {



//            var query = custbl.GetQueryable();


//            //            List<NESI.Common.Filter> filter = new List<NESI.Common.Filter>()
//            //{
//            //    //new Filter { PropertyName = "City" ,
//            //    //    Operation = Op .Equals, Value = "Mitrovice"  },
//            //    //new Filter { PropertyName = "Name" ,
//            //    //    Operation = Op .StartsWith, Value = "L"  },
//            //    new NESI.Common.Filter("manufacturer:Ok:Contains") 
//            //};


//            if (param.column_filter != null && param.column_filter.Length > 0)
//            {
//                var deleg = ExpressionBuilder.GetExpression<T1>(param.column_filter).Compile();

//                //            var filtered = query.Where(deleg);
//                query = query.Where<T1>(deleg).AsQueryable();
//            }
//            if (param.column_groupBy == null)

//                if (param.column_sort != null && param.column_sort.Length > 0)
//                {
//                    query = query.ApplySortingPaging<T1>(param.column_sort, param.page_count, param.page_size);

//                }
//                else
//                {

//                    var test = query.GroupBy(string.Format("new({0})", string.Join(",", param.column_groupBy)), "it")
//  .Select(string.Format("new({0})", string.Join(",", param.column_groupBy.Select(p => "Key." + p))));

//                }
//            var filteredCollection = query;



//            query.GroupBy(new string[] { "", "" });

//            var ds = NesiQueryBuilder.CallDataProcessor<T1>(filteredCollection, param, out dtSummary);

//            DataTable unionedDataTable = new DataTable();

//            if (param.mode == null || param.mode.Equals("summary") == false)
//            {
//                foreach (DataTable table in ds.Tables)
//                {
//                    unionedDataTable.Load(table.CreateDataReader());
//                }
//                //  schema = Common.ExtractSchema(custbl);
//            }
//            //else
//            //    schema = "[]";

//            return unionedDataTable;
//        }






//        private static string GetParameter(HttpRequestMessage Request, string query)
//        {
//            return Request.GetQueryNameValuePairs().Where(nv => nv.Key == query).Select(nv => nv.Value).FirstOrDefault();
//        }

//        public static string ExtractSchema<T>(T custbl) where T : NESI.Common.IPageModelSchemaGenerator
//        {
//            string schema;
//            List<NESI.Common.Schema> lsSchema = new List<NESI.Common.Schema>();
//            lsSchema = custbl.GetSchema();
//            schema = JsonConvert.SerializeObject(lsSchema);
//            return schema;
//        }

//        public static void ExporttoExcel(DataTable table)
//        {
//            HttpContext.Current.Response.Clear();
//            HttpContext.Current.Response.ClearContent();
//            HttpContext.Current.Response.ClearHeaders();
//            HttpContext.Current.Response.Buffer = true;
//            HttpContext.Current.Response.ContentType = "application/ms-excel";
//            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
//            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");

//            HttpContext.Current.Response.Charset = "utf-8";
//            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
//            //sets font
//            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
//            HttpContext.Current.Response.Write("<BR><BR><BR>");
//            //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
//            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
//              "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
//              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
//            //am getting my grid's column headers
//            int columnscount = table.Columns.Count;

//            for (int j = 0; j < columnscount; j++)
//            {      //write in new column
//                HttpContext.Current.Response.Write("<Td>");
//                //Get column headers  and make it as bold in excel columns
//                HttpContext.Current.Response.Write("<B>");
//                HttpContext.Current.Response.Write(table.Columns[j].ColumnName.ToString());
//                HttpContext.Current.Response.Write("</B>");
//                HttpContext.Current.Response.Write("</Td>");
//            }
//            HttpContext.Current.Response.Write("</TR>");
//            foreach (DataRow row in table.Rows)
//            {//write in new row
//                HttpContext.Current.Response.Write("<TR>");
//                for (int i = 0; i < table.Columns.Count; i++)
//                {
//                    HttpContext.Current.Response.Write("<Td>");
//                    HttpContext.Current.Response.Write(row[i].ToString());
//                    HttpContext.Current.Response.Write("</Td>");
//                }

//                HttpContext.Current.Response.Write("</TR>");
//            }
//            HttpContext.Current.Response.Write("</Table>");
//            HttpContext.Current.Response.Write("</font>");
//            HttpContext.Current.Response.Flush();
//            HttpContext.Current.Response.End();
//        }


//    }
//}
