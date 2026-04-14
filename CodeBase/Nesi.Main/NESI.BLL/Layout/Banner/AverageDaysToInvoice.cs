using System;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;

namespace NESI.BLL.Layout.Banner
{
    public class AverageDaysToInvoice : BLLBase
    {
        public AverageDaysToInvoice(Employee user): base(user)
        {
        }
    
        public DTO.ViewModels.CurrentUser.Layout.AverageDaysToInvoice GetAverageDaysToInvoice()
        {
            var woAvgMyBu = _db.Database.SqlQuery<double>(@"SELECT
  IFNULL(MAX(wo_avg_proc), 0)
FROM
  dashboard_snapshot_daily
WHERE business_unit_id = @P0
  AND wo_avg_proc < 300
LIMIT 1", CurrentUser.BusinessUnitId).FirstOrDefault();
            
            var woAvgAllBu = _db.Database.SqlQuery<double>(@"SELECT
    IFNULL(AVG(wo_avg_proc), 0)
  FROM
    dashboard_snapshot_daily
  WHERE business_unit_id != 8
    AND (
      FIND_IN_SET(business_unit_id, @p1)
      OR tax_entity_id = @p0
)
AND wo_avg_proc > 0
AND wo_avg_proc < 300", CurrentUser.TaxEntityId, CurrentUser.AssociatedBusinessUnits).FirstOrDefault();
            
            var avgDaysToInvoice = new DTO.ViewModels.CurrentUser.Layout.AverageDaysToInvoice
            {
                Name = CurrentUser.BusniessUnitName,
                ThisBusinessUnit = (int) Math.Abs(Math.Round(woAvgMyBu, 0)),
                AllBusinessUnit = (int) Math.Abs(Math.Round(woAvgAllBu, 0))
            };
            return avgDaysToInvoice;
        }
    }
}