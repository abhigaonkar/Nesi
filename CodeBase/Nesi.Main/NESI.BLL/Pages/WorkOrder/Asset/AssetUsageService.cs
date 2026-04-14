using nesi.core;
using NESI.BLL.Pages.Timesheet.JobType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.WorkOrder.Asset
{
    public partial class AssetUsageService : IAssetUsage
    {
        public static string AssetType = "A";

        public AssetUsageAddResult Add(AssetUsageAddInputParameter parameter)
        {
            return this._Add(parameter);
        }

        public List<AssertUsageRecord> Get(AssetQueryParameter parameter)
        {
            return this._Get(parameter);
        }

        public AssetUsageUpdateResult Update(AssetUsageUpdateInputParameter parameter)
        {
            return this._Update(parameter);
        }

        public bool AssetAvailable(AssetAvailabeQueryParameter parameter)
        {
            JobTypeService jts = new JobTypeService();
            BusinessJobTypeConfigQueryParameter bjtcq = new BusinessJobTypeConfigQueryParameter
            {
                business_unit_id = parameter.BusinesssUnitID
            };

            BusinessJobTypeConfig result = jts.GetBusinessJobTypeConfig(bjtcq);

            return result.allow_jobtype_selection;
        }

        public List<ValidationRecord> ValidatonOnCreate(AssetUsageAddInputParameter parameter)
        {
            List<ValidationRecord> errors = new List<ValidationRecord> { };

            if (parameter == null)
            {
                var e1 = new ValidationRecord {  FieldName="", Error="Input Parameter is not valid."};
                errors.Add(e1);
                return errors;
            }

            // Mat type
            if (parameter.MatType != "11")
            {
                var e = new ValidationRecord { FieldName = "Source", Error = "Source is not Asset." };
                errors.Add(e);
            }

            // Asset ID
            if (string.IsNullOrWhiteSpace(parameter.AssetID))
            {
                var e = new ValidationRecord { FieldName = "AssetID", Error = "AssetID is blank." };
                errors.Add(e);
            }
            if (!string.IsNullOrWhiteSpace(parameter.AssetID))
            {
                var assetid = 0;
                if (!int.TryParse(parameter.AssetID, out assetid))
                {
                    var e = new ValidationRecord { FieldName = "AssetID", Error = "AssetID is not valid." };
                    errors.Add(e);
                }
                else
                {
                    if (assetid == 0)
                    {
                        var e = new ValidationRecord { FieldName = "AssetID", Error = "AssetID is Zero." };
                        errors.Add(e);
                    }
                }
            }

            // Chargeout type
            if (string.IsNullOrWhiteSpace(parameter.ChargeoutType))
            {
                var e = new ValidationRecord { FieldName = "ChargeoutType", Error = "Charge out Type is blank." };
                errors.Add(e);
            }
            if (!string.IsNullOrWhiteSpace(parameter.ChargeoutType))
            {
                var chargeoutType = 0;
                if (!int.TryParse(parameter.AssetID, out chargeoutType))
                {
                    var e = new ValidationRecord { FieldName = "ChargeoutType", Error = "ChargeoutType is not valid." };
                    errors.Add(e);
                }
                else
                {
                    if (chargeoutType == 0)
                    {
                        var e = new ValidationRecord { FieldName = "ChargeoutType", Error = "ChargeoutType is not valid." };
                        errors.Add(e);
                    }
                }
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

            // Work order
            if (parameter.workorderId <= 0)
            {
                var e = new ValidationRecord { FieldName = "workorderId", Error = "No workorder ID." };
                errors.Add(e);
            }

            // Customer
            if (parameter.customerId <= 0)
            {
                var e = new ValidationRecord { FieldName = "customerId", Error = "No Customer ID." };
                errors.Add(e);
            }

            // user
            if (parameter.userID <= 0)
            {
                var e = new ValidationRecord { FieldName = "userID", Error = "No User ID." };
                errors.Add(e);
            }

            // BU
            if (parameter.business_unit_id <= 0)
            {
                var e = new ValidationRecord { FieldName = "business_unit_id", Error = "No Business Unit ID." };
                errors.Add(e);
            }

            // current user
            if (parameter.currrentUser == null)
            {
                var e = new ValidationRecord { FieldName = "currrentUser", Error = "No Currrent User Setup." };
                errors.Add(e);
            }

            return errors;
        }

        #region private
        private List<AssertUsageRecord> _Get(AssetQueryParameter parameter)
        {
            List<AssertUsageRecord> list = new List<AssertUsageRecord> { };
            if (parameter == null || parameter.workOrderId <= 0)
            {
                return list;
            }

            var query = @"
SELECT wo_detail_current_id, wo_detail_current_description, wo_detail_current_master_id 
FROM wo_detail_current 
WHERE wo_detail_current_type = 'A' AND wo_detail_current_woprog_id = @v0";

            try
            {
                var dt = Toolbox.doSQL_dt(query, new object[] { parameter.workOrderId });
                if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                {
                    return list;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    var record = new AssertUsageRecord();

                    record.wo_detail_current_id = Convert.ToInt32(dr["wo_detail_current_id"].ToString());
                    record.masterID = Convert.ToInt32(dr["wo_detail_current_master_id"].ToString());
                    record.des = dr["wo_detail_current_description"].ToString();

                    list.Add(record);
                }
            }
            catch
            {
            }
            
            return list;
        }
        #endregion
    }

    public enum MatType
    {
        Materail = 1,






        Asset = 11
    }

    /*
     * Mat Type
     * 1 : material
     * 2 : labour
     * 3 : kitted
     * 4 : Grab parts from a group
     * 5 : Grab parts from a Quote
     * 6 : Grab parts from a Work Order 
     * 7 : Grab parts from a purchase Order 
     * 8 : Vendor RFQ (8)
     * 9 : Repair Material type (only for er work orders)
     * 10: ??   (keep this untouched...)
     * 11: Asset
     * 12: for future use.

     */
}
