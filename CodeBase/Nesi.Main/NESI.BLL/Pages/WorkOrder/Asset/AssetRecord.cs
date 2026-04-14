using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.WorkOrder.Asset
{
    public class AssetRecord
    {
        public static string Daily_ChargeoutType = "Daily";
        public static string Weekly_ChargeoutType = "Weekly";
        public static string Monthly_ChargeoutType = "Monthly";

        public int id { get; set; }
        public string des { get; set; }
        public decimal chargeout { get; set; }
        public string chargeoutType { get; set; }
    }
}
