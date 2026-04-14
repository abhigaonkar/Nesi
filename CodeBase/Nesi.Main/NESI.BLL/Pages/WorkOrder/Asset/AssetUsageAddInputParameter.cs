using nesi.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NESI.BLL.Pages.WorkOrder.Asset
{
    public class AssetUsageAddInputParameter
    {
        public string MatType { get; set; }
        public string AssetID { get; set; }
        public string ChargeoutType { get; set; }
        public string Charegout { get; set; }
        public string Description { get; set; }
        public string SellPrice { get; set; } // May be same as chargeout
        public string RequireDate { get; set; }
        public string BillType { get; set; }

        // requried
        public int workorderId { get; set; }
        public int customerId { get; set; }
        public int quotedId { get; set; }
        public int userID { get; set; }
        public int business_unit_id { get; set; }
        public int tax1 { get; set; }
        public int tax2 { get; set; }
        public int tax3 { get; set; }
        public int tax4 { get; set; }

        public NeMember currrentUser {get;set;}
    }
}
