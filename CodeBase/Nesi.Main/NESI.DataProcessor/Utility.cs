using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DataProcessor
{
    //public class QueryParam
    //{
       
    //    public  Filter[] column_filter { get; set; }
    //    public  int page_count { get; set; }
    //    public  int page_size { get; set; }

    //    public string[] column_groupBy { get; set; }
    //    public SortBy[] column_sort { get; set; }

    //    public string[] Selectby { get; set; }
        
    //    public  string column_sortorder { get; set; }

    //    public bool childdata { get; set; }

    //    public  string[] summary { get; set; }
    //    public string mode { get; set; }
    //    private HttpRequestMessage request;

    //    public QueryParam(HttpRequestMessage request)
    //    {
    //        this.request = request;

    //        column_groupBy = GetParameter(request, "groupby");
    //        column_filter = GetParameter(request, "filtercolumn").Select(p=>new Filter(p)).ToArray();
    //        var filtervalue = GetParameter(request, "filtervalue");
    //        column_sort = GetParameter(request, "sortby").Select(p=>new SortBy(p)).ToArray();
    //        ;
    //        Selectby = GetParameter(request, "selectcolumn");
    //         page_size =Convert.ToInt32(  GetParameter(request, "pagesize").FirstOrDefault());
    //         page_count = Convert.ToInt32(GetParameter(request, "pagecount").FirstOrDefault());
    //         childdata = Convert.ToBoolean( GetParameter(request, "innerdata").FirstOrDefault());
    //        mode = GetParameter(request, "mode").FirstOrDefault();
    //        summary = GetParameter(request, "summary");
    //    }

    //    private  string[] GetParameter(HttpRequestMessage Request, string query)
    //    {
    //        var value= Request.GetQueryNameValuePairs().Where(nv => nv.Key == query).Select(nv => nv.Value).FirstOrDefault();
    //        if (!string.IsNullOrWhiteSpace(value))
    //        {
    //            return value.Split(',').ToArray();
    //        }
    //        else
    //            return new string[] { };
    //    }
    //}

//    public class SortBy
//    {

//      public  string ColumnName;

//public        string Direction ="ASC";

//        public string GetExpression() {

//            return string.Format("{0} {1}", ColumnName, Direction);
//        }

//        public SortBy(string query)
//        {
//            var arr = query.Split(':');

//        this.ColumnName=    arr[0];
//            if (arr.Count() == 2)
//                Direction = arr[1];
//        }
//    }
}
