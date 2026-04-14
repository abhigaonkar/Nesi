using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{
    public class SortOrder
    {
        public enum Order
        {
            Ascending,
            Descending
        }
        public string ColumnName { get; set; }
        public Order ColumnOrder { get; set; }


        public string Direction = "ASC";

        public string GetExpression()
        {

            return $"{ColumnName} {Direction}";
        }


        public SortOrder(string query)
        {
            var arr = query.Split(new[] { "||" }, StringSplitOptions.None);

            this.ColumnName = arr[0];
            if (arr.Count() == 2)
            {
                Direction = arr[1];
                ColumnOrder = Order.Descending;
            }
        }
		 
	    public override string ToString()
	    {
		    return GetExpression();
	    }
    }
}
