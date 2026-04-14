using nesi.core;
using NESI.BLL.Pages.Timesheet.JobType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.WorkOrder.Asset
{
    public partial class AssetUsageService
    {
        public List<ValidationRecord> ValidatonOnUpdate(AssetUsageUpdateInputParameter parameter)
        {
            List<ValidationRecord> errors = new List<ValidationRecord> { };

            if (parameter == null)
            {
                var e1 = new ValidationRecord { FieldName = "", Error = "Input Parameter is not valid." };
                errors.Add(e1);
                return errors;
            }

            // Description
            if (string.IsNullOrWhiteSpace(parameter.Description))
            {
                var e = new ValidationRecord { FieldName = "Description", Error = "Description is blank." };
                errors.Add(e);
            }

            // Sell
            if (string.IsNullOrWhiteSpace(parameter.SellPrice))
            {
                var e = new ValidationRecord { FieldName = "Sell Price ", Error = "Description is blank." };
                errors.Add(e);
            }
            if (!string.IsNullOrWhiteSpace(parameter.SellPrice))
            {
                double sellPrice = 0;
                if (!double.TryParse(parameter.SellPrice, out sellPrice))
                {
                    var e = new ValidationRecord { FieldName = "SellPrice", Error = "Sell Price is not valid." };
                    errors.Add(e);
                }
                else
                {
                    if (sellPrice == 0)
                    {
                        var e = new ValidationRecord { FieldName = "SellPrice", Error = "Sell Price is Zero." };
                        errors.Add(e);
                    }
                }
            }

            // Require date
            if (string.IsNullOrWhiteSpace(parameter.RequireDate))
            {
                var e = new ValidationRecord { FieldName = "RequireDate", Error = "Require Date is blank." };
                errors.Add(e);
            }
            if (!string.IsNullOrWhiteSpace(parameter.RequireDate))
            {
                var date = DateTime.Now;
                if (!DateTime.TryParse(parameter.RequireDate, out date))
                {
                    var e = new ValidationRecord { FieldName = "RequireDate", Error = "Require Date is not valid." };
                    errors.Add(e);
                }
            }

            // Bill Type
            if (string.IsNullOrWhiteSpace(parameter.BillType))
            {
                var e = new ValidationRecord { FieldName = "BillType", Error = "Bill Type is blank." };
                errors.Add(e);
            }

            return errors;
        }

        private AssetUsageUpdateResult _Update(AssetUsageUpdateInputParameter parameter)
        {
            var result = new AssetUsageUpdateResult {  Done = false, error = ""};

            var sqlToUpdate = @"
UPDATE wo_detail_current 
SET 
wo_detail_current_description = @v2,
wo_detail_current_price_sell = @v3,
wo_detail_current_price_unit = @v3,
wo_detail_current_date_required = @v4,
wo_detail_current_billtypeid = @v5
WHERE wo_detail_current_id = @v0 and wo_detail_current_woprog_id = @v1 AND wo_detail_current_type = 'A' ";

            try
            {
                var p = new object[] {
                    parameter.wo_detail_current_id, parameter.workorderId, // 0, 1
                    parameter.Description,
                    Convert.ToDouble( parameter.SellPrice),
                    string.Format("{0:yyyy-MM-dd}",  parameter.RequireDate),
                    parameter.BillType // 2, 3, 4, 5
                 };

                Toolbox.doSQL_void(sqlToUpdate, p);
                result.Done = true;
            }
            catch(Exception ex)
            {
                result.error = "Failed to update this asset line due to [ " + ex.Message + "]";
            }
            
            return result;
        }
                
    }
}
