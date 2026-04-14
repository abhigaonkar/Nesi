using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("CustomerRatesGrid")]
    public class CustomerRatesGrid : ModelBase<CustomerRatesGrid>
    {
        
        public string business_unit { get; set; }
        public string customer_name { get; set; }
        public DateTime last_updated { get; set; }

        public string membertype_name { get; set; }
        public double normal { get; set; }
        public double chargeout { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public long customer_id { get; set; }
    }
}
