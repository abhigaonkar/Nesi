using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.AssetRate
{
    public interface IAssetCustomerRate
    {
        OperationResult Add(AssetCustomerRateUpdateParameter parameter);
        OperationResult Update(AssetCustomerRateUpdateParameter parameter);
        OperationResult Delete(AssetCustomerRateDeleteParameter parameter);
        AssetCustomerRateResult Get(AssetCustomerRateQueryParameter parameter);

        List<BuisnessUnitRecord> getAvailableBusinessUnits(GetVisiableBUParameter parameter);

        // for wo pick list usage.
        List<AssetCustomerRateRecord> GetAvailableAssetForWorkorderLine(AssetQueryForWorkOrderParameter parameter);
    }
}
