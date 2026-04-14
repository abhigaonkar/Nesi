using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("CustomerSurveysGrid")]
    public class CustomerSurveysGrid : ModelBase<CustomerSurveysGrid>
    {
        public DateTime date { get; set; }
        public string bvwo { get; set; }
        public string woprog_description { get; set; }
        public string cust_name { get; set; }
        public string name { get; set; }
        public double rating { get; set; }
        public string notes { get; set; }
        public string clean { get; set; }
        public string ontime { get; set; }
        public string callme { get; set; }

        public long customer_id { get; set; }
        public long woprog_id { get; set; }
        public long business_unit_id { get; set; }
        public long id { get; set; }
    }
}
