using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.WorkOrder.Asset
{
    public class AssetUsageAddResult
    {
        public bool Done { get; set; }
        public string error { get; set; }
        public int wo_detail_current_id { get; set; }
    }
}
