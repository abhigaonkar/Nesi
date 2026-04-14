using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.DTO.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{
    public class MasterPurchasesGrid : BLLGridBase<DTO.ViewModels.Page.Reports.MasterPurchasesGrid>
    {
        public MasterPurchasesGrid()
        {
        }

        public MasterPurchasesGrid(Employee user) : base(user)
        {
        }

        public MasterPurchasesGrid(Employee user, BodyParams param) : base(user, param, new object[] { })
        {
            this.query = "CALL get_master_purchases_grid_n2(@p0)";
            this.query_params = new object[] { user.Id };
        }

        public LabelValueInt[] GetApStatus()
        {
            var aps = bllToolbox.doSQL_Array<LabelValueInt>(@"select apstatus_id value, apstatus_name label from apstatus");
            return aps;
        }

        public DataExtra UpdateApStatus(DTO.ViewModels.Page.Reports.MasterPurchasesGrid purchase)
        {
            DataExtra result = new DataExtra()
            {
                Data = "AP Status has been updated successfully",
                Extra = ""
            };

            bllToolbox.doSQL_void(
                @"Update poprog_header  set poprog_apstatus=@v0  where poprog_id =@v1",
                new object[] {
                    Convert.ToInt32(purchase.poprog_apstatus),
                    Convert.ToInt32(purchase.poprog_id)}
                );

            ClearCache(new object[] { this.CurrentUser.Id});

            return result;
        }

        public DataExtra UpdateSingleMasterPurchase(DTO.ViewModels.Page.Reports.MasterPurchasesGrid purchase)
        {
            DataExtra result = new DataExtra()
            {
                Data = "Information has been saved successfully",
                Extra = purchase
            };

            try
            {
                bllToolbox.doSQL_void(@"
                    Update poprog_header
                    set poprog_apstatus=@v0,
                    poprog_hasproblem_notes=@v1
                    where poprog_id =@v2",
                    new object[]
                    {
                        purchase.poprog_apstatus,
                        purchase.poprog_hasproblem_notes,
                        purchase.poprog_id
                    });

                ClearCache(new object[] { this.CurrentUser.Id });
            }
            catch (Exception ex)
            {
                result.Data = string.Format("[{0}] Purchase can't be updated.", purchase.poprog_id);
                result.Extra = ex.Message;
            }

            return result;
        }


        public List<KeyValuePair<string, bool>> UpdateMultiplePurchases(DTO.ViewModels.Page.Reports.MasterPurchasesGrid[] purchases)
        {
            List<KeyValuePair<string, bool>> list = new List<KeyValuePair<string, bool>>();

            if (purchases == null || purchases.Length == 0)
            {
                return list;
            }

            foreach (var purchase in purchases)
            {
                var result = this.UpdateSingleMasterPurchase(purchase);
                var r = result.Data as string;
                if (r == "Information has been saved successfully")
                {
                    list.Add(new KeyValuePair<string, bool>(purchase.poprog_id.ToString(), true));
                }
                else
                {
                    list.Add(new KeyValuePair<string, bool>(purchase.poprog_id.ToString(), false));
                }
            }

            return list;
        }

    }
}
