using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.AssetRate
{
    public class AssetCustomerRateQueryParameter
    {
        public int business_unit_id { get; set; }
        public int customer_id { get; set; }
        public string searchingstring { get; set; }
        public int pageSize { get; set; }
        public int skipPage { get; set; }
    }
}
