using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("CustStatusHistory")]
    public class CustStatusHistory : ModelBase<CustStatusHistory>
    {
        public long customer_number { get; set; }
        public string customer_name { get; set; }
        public string date { get; set; }

        public string status { get; set; }
        public string name { get; set; }
        public string business_unit { get; set; }
        public string origin { get; set; }
        public string notes { get; set; }
    }
}
