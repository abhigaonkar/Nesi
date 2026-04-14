using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.WorkOrder.Asset
{
    public interface IAssetUsage
    {
        AssetUsageAddResult Add(AssetUsageAddInputParameter parameter);
        AssetUsageUpdateResult Update(AssetUsageUpdateInputParameter parameter);
        List<AssertUsageRecord> Get(AssetQueryParameter parameter);
        bool AssetAvailable(AssetAvailabeQueryParameter parameter);
        List<ValidationRecord> ValidatonOnCreate(AssetUsageAddInputParameter parameter);

        List<ValidationRecord> ValidatonOnUpdate(AssetUsageUpdateInputParameter parameter);
    }
}
