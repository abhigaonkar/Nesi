using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{
    public class BodyParams : ICloneable
    {
        public List<string> columns { get; set; }
        public int page_count = 1;

        public int page_size = 15;

        public bool? refreshCache = false;
        public int pagecount
        {
            set
            {
                if (value > 0)
                {
                    page_count = value;
                }
            }
        }

        public int pagesize
        {
            set
            {
                if (value > 0)
                {
                    page_size = value;
                }
            }
        }

        private string key = "";
        public string keyColumn {
            get { return key; }
            set
            {
                if (value != null && (column_sort == null || column_sort.Length == 0))
                {
                    column_sort = new SortOrder[] { new SortOrder(value) };
                }
            } 
        }

        public Filter[] column_filter = new Filter[0];
        public string filterBuilder { get; set; }
	    public string[] otherFilterBuilders { get; set; }
	    public string[] otherFilterConditions { get; set; }
	    public string bu_ids { get; set; }

		//add new for passing external report query parameter

		public Filter[] queryparam = new Filter[0];

        public string filtercolumn
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    column_filter = null;
                    column_filter = value.Split('`').Where(p => string.IsNullOrWhiteSpace(p) == false).Select(p => new Filter(p, false)).ToArray();
                }
            }
        }
        
        public SortOrder[] column_sort { get; set; }
        public string sortby
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    column_sort = value.Split('^').Where(p => string.IsNullOrWhiteSpace(p) == false).Select(p => new SortOrder(p)).ToArray();
                }

//                if (keyColumn != null && (column_sort == null || column_sort.Length == 0))
//                {
//                    column_sort = new SortOrder[] {new SortOrder(keyColumn)};
//                }
//                else
//                {
//                    column_sort = new SortOrder[] { new SortOrder("") };
//                }
            }
        }
        public string globalfilter { get; set; }
        public string[] column_groupBy = new string[] { };

        public string groupby
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    column_groupBy = value.Split('^');
                }
            }
        }

        public string[] Selectby = new string[] { };

        public string selectcolumn
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    Selectby = value.Split('^');
                }
            }
        }
        public string[] summary_by = new string[] {};

        public string summary
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    summary_by = value.Split('^');
                }
            }
        }
        public List<Filter[]> expand_by { get; set; }

        public string expandby
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    expand_by = value.Split('`').Select(p =>
                    {
                        return p.Split('^').Select(x => new Filter(x, false)).ToArray();
                    }).ToList();
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class QueryParam : ICloneable
    {

        public Filter[] column_filter { get; set; }

        public string global_filter { get; set; }
        public int page_count { get; set; }
        public int page_size { get; set; }

        public string[] column_groupBy { get; set; }
        public SortOrder[] column_sort { get; set; }

        public string[] Selectby { get; set; }

        //public string column_sortorder { get; set; }

      

        public string[] summary { get; set; }

        public List<Filter[]> expand { get; set; }
        public string mode { get; set; }
        private IEnumerable<KeyValuePair<string, string>> query;

        public QueryParam(IEnumerable<KeyValuePair<string, string>> query, string keyColumn)
        {
            this.query = query;
            page_count = 1;
            page_size = 15;
            column_groupBy = GetParameter(query, "groupby");
            expand = GetParameter(query, "expand", '|').Select(p =>
            {
                return p.Split(',').Select(x => new Filter(x, false)).ToArray();
            }).ToList();
            column_filter = GetParameter(query, "filtercolumn").Where(p=>string.IsNullOrWhiteSpace(p)==false).Select(p => new Filter(p, false)).ToArray();
            var filtervalue = GetParameter(query, "filtervalue");
            column_sort = GetParameter(query, "sortby").Where(p => string.IsNullOrWhiteSpace(p) == false).Select(p => new SortOrder(p)).ToArray();
            global_filter= GetParameter(query, "globalfilter").Where(p => string.IsNullOrWhiteSpace(p) == false).Select(p => p).FirstOrDefault();
            if (keyColumn != null && (column_sort == null || column_sort.Length == 0))
            {
                column_sort = new SortOrder[] { new SortOrder(keyColumn) };
            }

            Selectby = GetParameter(query, "selectcolumn");
            var size = GetParameter(query, "pagesize").FirstOrDefault();
            if (size != null)
                page_size = Convert.ToInt32(size);
            var count = GetParameter(query, "pagecount").FirstOrDefault();
            if (count != null)
                page_count = Convert.ToInt32(count);
         
           
            summary = GetParameter(query, "summary");
        }

        private string[] GetParameter(IEnumerable<KeyValuePair<string, string>> query, string param,char splitby=',')
        {
            var value = query.Where(nv => nv.Key == param &&  nv.Value!=null && !String.IsNullOrEmpty(nv.Value)).SelectMany(nv => nv.Value.Split(splitby)).ToArray();
            return value;
            //if (!string.IsNullOrWhiteSpace(value))

            //{
              //  return value.Split(',').ToArray();
           // }
            //else
              //  return new string[] { };
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
