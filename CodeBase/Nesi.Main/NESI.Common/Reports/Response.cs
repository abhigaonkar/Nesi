using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{
   public class Response<T> where T: class
    {
        public string request { get; set; }

        //public QueryParam param { get; set; }
        public List<T> data { get; set; }
        public int TotalCount { get; set; }
		public object extra { get; set; }
        public Dictionary<string, string> ColumnSummary { get; set; }
    }
}
