using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.WorkOrder.Asset
{
    public class AssetUsageUpdateInputParameter : AssetUsageAddInputParameter
    {
        public int wo_detail_current_id { get; set; }
    }
}
