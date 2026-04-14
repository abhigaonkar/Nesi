using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.AssetRate
{
    public class AssetCustomerRateRecord
    {
        public long keyId { get; set; }
        public int id { get; set; }
        public int business_unit_id { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public int asset_id {get;set;}
        public string asset_des { get; set; }
        public int customer_id { get; set; }
        public decimal daily { get; set; }
        public decimal weekly { get; set; }
        public decimal monthly { get; set; }
        public string lastupdate { get; set;}

        public int can_delete { get; set; }
    }

    public class AssetCustomerRateResult
    {
        public List<AssetCustomerRateRecord> data { get; set; }
        public int totalCount { get; set; }
    }
}
