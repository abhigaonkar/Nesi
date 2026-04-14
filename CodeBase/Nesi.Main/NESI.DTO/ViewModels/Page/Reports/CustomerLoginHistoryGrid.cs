using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("CustomerLoginHistoryGrid")]
    public class CustomerLoginHistoryGrid : ModelBase<CustomerLoginHistoryGrid>
    {
        public string date { get; set; }
        public string contact { get; set; }
        public string cust { get; set; }
        public long customer_id { get; set; }
        public long contact_id { get; set; }
    }
}
