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
        public static string module = "AssetUsageService - Add";
        private AssetUsageAddResult _Add(AssetUsageAddInputParameter parameter)
        {
            AssetUsageAddResult result = new AssetUsageAddResult { Done = false, error = "", wo_detail_current_id = 0 };

            NeWODetailCurrent current = new NeWODetailCurrent();

            try
            {
                var rec_no = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(wo_detail_current_rec_no), 1)+1 FROM wo_detail_current WHERE wo_detail_current_woprog_id=@v0 ",
                    new object[] { parameter.workorderId });

                current.added_by = parameter.userID;
                current.added_by_module = AssetUsageService.module; // Unique module
                current.billtypeid = int.Parse(parameter.BillType);
                current.business_unit_id = parameter.business_unit_id;
                current.bvwo = parameter.workorderId;
                current.code = parameter.AssetID; // save Asset ID in code
                current.cost = 0;
                current.rec_no = rec_no;
                current.date_added = Toolbox.MySQLNow_long();
                current.date_modified = current.date_added;
                current.date_required = parameter.RequireDate;
                current.description = parameter.Description;
                current.is_active = false;
                current.master_id = int.Parse(parameter.AssetID);
                current.memberid = 0;
                current.origin = "Manually Added Asset Usage Record from Work order";
                current.qty_committed = 1;
                current.qty_ordered = 1;
                current.qty_invoiced = 1;
                current.type = AssetUsageService.AssetType; //' A' for asset
                current.woprog_id = parameter.workorderId;
                current.sell = double.Parse(parameter.SellPrice);
                current.unit = current.sell;

                current.tax1 = parameter.tax1;
                current.tax2 = parameter.tax2;
                current.tax3 = parameter.tax3;
                current.tax4 = parameter.tax4;

                current.save(parameter.currrentUser, current.added_by_module, true);

                result.Done = true;
                result.wo_detail_current_id = current.id;
            }
            catch (Exception ex)
            {
                result.Done = false;
                result.error = ex.Message;
                result.wo_detail_current_id = 0;
            }

            return result;
        }
    }
}
