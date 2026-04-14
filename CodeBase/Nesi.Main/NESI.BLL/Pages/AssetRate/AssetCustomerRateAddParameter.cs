using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.AssetRate
{
    public class AssetCustomerRateAddParameter
    {
        public int id { get; set; } // = 0
        public int business_unit_id { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public int asset_id { get; set; }
        public int customerId { get; set; }
        public decimal daily { get; set; }
        public decimal weekly { get; set; }
        public decimal monthly { get; set; }
    }
}
