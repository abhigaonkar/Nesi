using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nesi.core;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Page.TimeSheet;

namespace NESI.BLL.Pages.Timesheet
{
    public class TimesheetTransferService : ITimesheetTransfer
    {
        private Employee _selected;
        private Employee _currentUser;
        public TimesheetTransferService(Employee current, Employee selected)
        {
            this._selected = selected;
            this._currentUser = current;
        }

        public bool IsTransferable(DTO.ViewModels.Page.TimeSheet.Timesheet timesheet)
        {
            return this._IsTransferable(timesheet);
        }

        public TimesheetTransferResult Transfer(TimesheetTransferData data)
        {
            return this._Transfer(data);
        }

        private TimesheetTransferResult _Transfer(TimesheetTransferData data)
        {
            TimesheetTransferResult result = new TimesheetTransferResult();
            try
            {
                result = this._InnerTransfer(data);
            }
            catch (Exception ex)
            {
                result.okay = false;
                result.summary = "Failed to transfer this record due to error " + ex.Message;
            }

            return result;
        }

        private TimesheetTransferResult _InnerTransfer(TimesheetTransferData data)
        {
            if (data.targetTimesheetInfo.transfer_type.ToLowerInvariant() == "WO".ToLowerInvariant())
            {
                return this._InnerTransfer_Wo_to_Wo(data);

            }
            else
            {
                return this._InnerTransfer_Wo_to_Shop(data);
            }
        }

        #region Wo to Wo
        private TimesheetTransferResult _InnerTransfer_Wo_to_Wo(TimesheetTransferData data)
        {
            TimesheetTransferResult result = new TimesheetTransferResult();

            //
            // Construct two data
            //
            if(!this._InnerTransfer_Wo_to_Wo_PrepareData(data))
            {
                result.okay = false;
                result.summary = "Information is not enough.";
                return result;
            }

            //
            // Validation goes first
            //
            var list = this._InnerTransfer_Wo_to_Wo_Validation(data);
            if (list.Count != 0)
            {
                result.okay = false;
                result.summary = list[0].info; // currently angular side supports simple error info.
                result.errors = list;
                return result;
            }

            //
            // First add a negative work order timessheet record.
            //
            var negativeResult = this._InnerTransfer_Wo_to_WoOrShop_Negative(data);
            if (!(negativeResult.IndexOf("success", StringComparison.Ordinal) > -1))
            {
                result.okay = false;
                result.summary = negativeResult;
                return result;
            }

            //
            // second create a positive work order timesheet record.
            //
            var positiveResult = this._InnerTransfer_Wo_to_WoOrShop_Positive(data, true);
            if( !(positiveResult.IndexOf("success", StringComparison.Ordinal) > -1) )
            {
                // There is a risk here how to remove the negative on
                result.okay = false;
                result.summary = positiveResult;
                return result;
            }

            //
            // Set up chain:  new positive => new negative => orignal => orignal
            //
            this._InnerTransfer_Wo_to_Wo_Chain(data, true);

            result.okay = true;
            result.summary = "The transfer operation is successful.";
            return result;
        }

        private bool _InnerTransfer_Wo_to_Wo_Chain(TimesheetTransferData data, Boolean isWO)
        {
            bool okay = true;

            var sql0 = @"
UPDATE
  membertime
SET
  OrigMemberTime_ID = @v0
WHERE MemberTime_ID = @v1";

            //
            // positive points to negative
            //
            var targetid = 0;
            if (isWO)
            {
                targetid = data.ToBeAddedPositiveTimesheetRecord.IdForTransfer;
            }
            else
            {
                targetid = data.ToBeAddedPositveShopTimeRecord.IdForTransfer;
            }

            var params4Sql = new object[]
            {
                data.ToBeAddedNegativeTimesheetRecord.IdForTransfer,
                targetid
            };

            try
            {
                // Update a-not-existing-item will be causing do nothing in db side, and the angular side will be updated to see the default value.
                Toolbox.doSQL_void(sql0, params4Sql);
            }
            catch (Exception ex)
            {
            }

            //
            // negative points to orignal
            //
            var params4Sql2 = new object[]
            {
                data.originalTimesheetInfo.MemberTime_ID,
                data.ToBeAddedNegativeTimesheetRecord.IdForTransfer,
            };

            try
            {
                // Update a-not-existing-item will be causing do nothing in db side, and the angular side will be updated to see the default value.
                Toolbox.doSQL_void(sql0, params4Sql2);
            }
            catch (Exception ex)
            {

            }


            return okay;
        }

        private string _InnerTransfer_Wo_to_Wo_Positive(TimesheetTransferData data)
        {
            var ts = new BLL.Pages.Timesheet.Timesheet(this._currentUser);
            var positiveResult = ts.SaveWorkOrder(this._selected, data.ToBeAddedPositiveTimesheetRecord);
            // var id = data.ToBeAddedNegativeTimesheetRecord.IdForTransfer;
            return positiveResult;
        }

        private string _InnerTransfer_Wo_to_Wo_Negative(TimesheetTransferData data)
        {
            var positiveResult = new BLL.Pages.Timesheet.Timesheet(this._currentUser).SaveWorkOrder(this._selected, data.ToBeAddedNegativeTimesheetRecord);
            return positiveResult;
        }

        private int MasterIdForParentWorkOrder(int woid, int payTypeHour, bool sendEmail, int memberTypeId, string jobTypeName)
        {
            var Wo = new Ne2WOProg(woid);

            //
            // Use same logic from workorder.cs file.
            //
            var woId = Wo.Entity?.parent_woprog_id.GetValueOrDefault();
            if (woId > 1)
            {
                var customer = new Ne2Customer(Convert.ToInt32(Wo.Entity.woprog_customer_id));
                // if customer is ne company then get parent work order
                if (customer.IsFound && customer.IsNECompany)
                {
                    var parentWo = new Ne2WOProg(Wo.Entity.parent_woprog_id.GetValueOrDefault());

                    // Recalculate the parent master id in parent business unit.
                    Timesheet ts = new Timesheet();
                    var id = ts.GetlabourMasterIdForSelectedJobType(parentWo.Entity.business_unit_id.Value, null, payTypeHour, false, memberTypeId, "");
                    return id;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private double GetSellPriceOnOriginalRecord(TimesheetTransferData data, int workOrderId)
        {
            //
            // Reason: due to this new changes: Sell Price on time record is 0 (1887)
            // In our system, there are around ~20K wo timesheet records with 0 sell price. Fix it when transfering.
            //
            var sellPrice =Toolbox.doSQL_double(
             @"SELECT IFNULL(MAX(wo_detail_current_price_sell), 0) sell FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ",
             new object[] { workOrderId, data.originalTimesheetInfo.membertype_chargeout_id });
            return sellPrice;
        }

        private void FixSellPriceOnOriginalRecord(TimesheetTransferData data, int workOrderId, double sellprice)
        {
            if (sellprice == 0 || data.originalTimesheetInfo.MemberTime_ID == 0)
            {
                return;
            }

            // keep the ts untouched.
            var sql = @"
UPDATE
  membertime
SET
  MemberTime_SellPrice = @v1,
  ts = ts
WHERE MemberTime_ID = @v0
  AND MemberTime_SellPrice = 0
";

            var params4Sql = new object[]
            {
                data.originalTimesheetInfo.MemberTime_ID,
                sellprice
            };

            try
            {
                Toolbox.doSQL_void(sql, params4Sql);
            }
            catch (Exception ex)
            {
            }
        }

        private bool _InnerTransfer_Wo_to_Wo_PrepareData(TimesheetTransferData data)
        {
            data.ToBeAddedPositiveTimesheetRecord = new InsertTimeSheetWorkOrder();
            data.ToBeAddedNegativeTimesheetRecord = new InsertTimeSheetWorkOrder();
            data.ToBeAddedPositveShopTimeRecord = new InsertTimeSheetShop();

            //
            // Get the original prices from original record.
            //
            Timesheet ts = new Timesheet();
            var entity = ts.GetbyId(data.originalTimesheetInfo.MemberTime_ID);
            if (entity == null)
            {
                return false;
            }

            data.originalTimesheetInfo.cost = entity.MemberTime_CostPrice;
            data.originalTimesheetInfo.sell = entity.MemberTime_SellPrice;

            //
            // The sell price issue: 1905 [sell price on transfer] make sure still having correct sell price even original one has zero.
            //
            if (data.originalTimesheetInfo.sell == 0)
            {
                data.originalTimesheetInfo.sell = GetSellPriceOnOriginalRecord(data, entity.MemberTime_WOProg_id.Value);
                this.FixSellPriceOnOriginalRecord(data, entity.MemberTime_WOProg_id.Value, data.originalTimesheetInfo.sell);
            }

            //
            // Positive record
            //

            data.ToBeAddedPositiveTimesheetRecord.Date = DateTime.Now; // data.timesheetValue.selectedDate;
            data.ToBeAddedPositiveTimesheetRecord.SelectedBusinessUnitId = data.timesheetValue.businessUnitId;
            data.ToBeAddedPositiveTimesheetRecord.SelectedUserId = data.timesheetValue.userId;
            data.ToBeAddedPositiveTimesheetRecord.PayTypeId = data.originalTimesheetInfo.PayTypeId.Value;    
         
            // Must provide province.
            data.ToBeAddedPositiveTimesheetRecord.prov_id = data.targetTimesheetInfo.prov_id.Value;
            

            data.ToBeAddedPositiveTimesheetRecord.MemberTime_WoComment_ID = 0;
            data.ToBeAddedPositiveTimesheetRecord.NumberOfHours = data.originalTimesheetInfo.Hours;
            data.ToBeAddedPositiveTimesheetRecord.Rating = data.originalTimesheetInfo.rating;
            data.ToBeAddedPositiveTimesheetRecord.PercentComplete = data.originalTimesheetInfo.wo_percent_complete;
            data.ToBeAddedPositiveTimesheetRecord.MemberTime_WoComment = data.targetTimesheetInfo.Comments + " ( Transferred from " + data.originalTimesheetInfo.bvwo + " [positive record] )";

            data.ToBeAddedPositiveTimesheetRecord.SelectedCustomerId = (int)data.targetTimesheetInfo.CustId;
            data.ToBeAddedPositiveTimesheetRecord.SelectedCustomerName = data.targetTimesheetInfo.membertime_customer_name;

            data.ToBeAddedPositiveTimesheetRecord.SelectedWorkOrderId = int.Parse(data.targetTimesheetInfo.WorkorderId);
            data.ToBeAddedPositiveTimesheetRecord.SelectedWorkOrderName = data.targetTimesheetInfo.scope_name; // Note: Borrow this in angular side to pass the workorder name.
            data.ToBeAddedPositiveTimesheetRecord.SelectedTsLiteType = 0;

            data.ToBeAddedPositiveTimesheetRecord.entry_type = "single";
            data.ToBeAddedPositiveTimesheetRecord.selectedJobType = data.targetTimesheetInfo.membertype_id.Value;
            data.ToBeAddedPositiveTimesheetRecord.allow_jobtype_selection = data.timesheetValue.allow_jobtype_selection;

            // Transfer setup
            data.ToBeAddedPositiveTimesheetRecord.IsTransfer = true;
            data.ToBeAddedPositiveTimesheetRecord.transferInfo = "P"; // Positive
            data.ToBeAddedPositiveTimesheetRecord.originalChargeoutId = (int)data.originalTimesheetInfo.membertype_chargeout_id.Value;
            data.ToBeAddedPositiveTimesheetRecord.OriginalPrice = data.originalTimesheetInfo.sell;
            data.ToBeAddedPositiveTimesheetRecord.OriginalCost = data.originalTimesheetInfo.cost;
            data.ToBeAddedPositiveTimesheetRecord.orginal_branch_can_see_jobtype = data.timesheetValue.allow_jobtype_selection;
            data.ToBeAddedPositiveTimesheetRecord.originalMemberTypeid = (int)data.originalTimesheetInfo.membertype_id;

            //
            // Negative record
            //
            this._InnerTransfer_Wo_to_Any_PrepareNegativeData(data);

            return true;
        }

        private List<TransferErrorRecord> _InnerTransfer_Wo_to_Wo_Validation(TimesheetTransferData data)
        {
            var list = new List<TransferErrorRecord> { };

            //
            // From task: https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1805/
            // WO cannot be the originating WO (though it can be transferred back if there was a mistake)
            //
            if (data.originalTimesheetInfo.MemberTime_WOProg_id == int.Parse(data.targetTimesheetInfo.WorkorderId))
            {
                // Now we have the same workorders on orignal and target. 
                if (data.timesheetValue.allow_jobtype_selection)
                {
                    //
                    // The users can see the job type button, so you can select the same ones.
                    //

                    // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1856/ Transfer - original can be the target conditionallly.
                    return list;
                }

                var error = new TransferErrorRecord
                {
                    field = "Work Order",
                    info = "The target work order cannot be the originating work order."
                };

                list.Add(error);
                return list;
            }

            return list;
        }

        #endregion

        #region Wo to Shop
        private TimesheetTransferResult _InnerTransfer_Wo_to_Shop(TimesheetTransferData data)
        {
            TimesheetTransferResult result = new TimesheetTransferResult();

            //
            // Construct two data
            //
            if (!this._InnerTransfer_Wo_to_Shop_PrepareData(data))
            {
                result.okay = false;
                result.summary = "Information is not enough.";
                return result;
            }

            //
            // Validation goes first
            //
            var list = this._InnerTransfer_Wo_to_Shop_Validation(data);
            if (list.Count != 0)
            {
                result.okay = false;
                result.summary = "Validation Errors";
                result.errors = list;
                return result;
            }

            //
            // First add a negative work order timessheet record, this part is same.
            //
            var negativeResult = this._InnerTransfer_Wo_to_WoOrShop_Negative(data);
            if (!(negativeResult.IndexOf("success", StringComparison.Ordinal) > -1))
            {
                result.okay = false;
                result.summary = negativeResult;
                return result;
            }

            //
            // then create a positive work order timesheet record.
            //
            var positiveResult = this._InnerTransfer_Wo_to_WoOrShop_Positive(data, false);
            if (!(positiveResult.IndexOf("success", StringComparison.Ordinal) > -1))
            {
                // There is a risk .
                result.okay = false;
                result.summary = positiveResult;
                return result;
            }


            //
            // Set up chain:  new positive => new negative => orignal => orignal
            //
            this._InnerTransfer_Wo_to_Wo_Chain(data, false);

            result.okay = true;
            result.summary = "The transfer operation is successful.";
            return result;
        }

        private bool _InnerTransfer_Wo_to_Shop_PrepareData(TimesheetTransferData data)
        {
            data.ToBeAddedPositiveTimesheetRecord = new InsertTimeSheetWorkOrder();
            data.ToBeAddedNegativeTimesheetRecord = new InsertTimeSheetWorkOrder();
            data.ToBeAddedPositveShopTimeRecord = new InsertTimeSheetShop();

            //
            // Get the original prices from original record.
            //
            Timesheet ts = new Timesheet();
            var entity = ts.GetbyId(data.originalTimesheetInfo.MemberTime_ID);
            if (entity == null)
            {
                return false;
            }

            data.originalTimesheetInfo.cost = entity.MemberTime_CostPrice;
            data.originalTimesheetInfo.sell = entity.MemberTime_SellPrice;

            //
            // Positive record
            //

            data.ToBeAddedPositveShopTimeRecord.Date = DateTime.Now; // data.timesheetValue.selectedDate;
            data.ToBeAddedPositveShopTimeRecord.SelectedBusinessUnitId = data.timesheetValue.businessUnitId;
            data.ToBeAddedPositveShopTimeRecord.SelectedUserId = data.timesheetValue.userId;
            data.ToBeAddedPositveShopTimeRecord.PayTypeId = 1; // Regular time
            data.ToBeAddedPositveShopTimeRecord.MemberTime_WoComment_ID = 0;
            data.ToBeAddedPositveShopTimeRecord.NumberOfHours = data.originalTimesheetInfo.Hours;
            data.ToBeAddedPositveShopTimeRecord.Rating = data.originalTimesheetInfo.rating;

            data.ToBeAddedPositveShopTimeRecord.PercentComplete = 100;
            data.ToBeAddedPositveShopTimeRecord.MemberTime_WoComment = data.targetTimesheetInfo.Comments + " ( Transferred from " + data.originalTimesheetInfo.bvwo + "  [positive record])";

            data.ToBeAddedPositveShopTimeRecord.SelectedShopTimeTypeId = (int)data.targetTimesheetInfo.membertime_shop_type_id;
            data.ToBeAddedPositveShopTimeRecord.SelectedShopTimeTypeName = data.targetTimesheetInfo.scope_name;
            data.ToBeAddedPositveShopTimeRecord.internal_project_id = 0;

            // Transfer setup
            data.ToBeAddedPositveShopTimeRecord.IsTransfer = true;
            data.ToBeAddedPositveShopTimeRecord.transferInfo = "P"; // Positive
            data.ToBeAddedPositveShopTimeRecord.originalChargeoutId = (int)data.originalTimesheetInfo.membertype_chargeout_id.Value;
            data.ToBeAddedPositveShopTimeRecord.OriginalPrice = data.originalTimesheetInfo.sell;
            data.ToBeAddedPositveShopTimeRecord.OriginalCost = data.originalTimesheetInfo.cost;
            data.ToBeAddedPositveShopTimeRecord.originalMemberTypeid = (int)data.originalTimesheetInfo.membertype_id;

            //
            // Negative record
            //
            this._InnerTransfer_Wo_to_Any_PrepareNegativeData(data);

            return true;
        }

        private List<TransferErrorRecord> _InnerTransfer_Wo_to_Shop_Validation(TimesheetTransferData data)
        {
            var list = new List<TransferErrorRecord> { };

            // Put something here.

            return list;
        }


        #endregion

        #region common
        private bool _InnerTransfer_Wo_to_Any_PrepareNegativeData(TimesheetTransferData data)
        {
            //
            // Negative record
            //
            data.ToBeAddedNegativeTimesheetRecord.Date = DateTime.Now;  //data.originalTimesheetInfo.Date;
            data.ToBeAddedNegativeTimesheetRecord.SelectedBusinessUnitId = data.originalTimesheetInfo.BusinessUnitId;
            data.ToBeAddedNegativeTimesheetRecord.SelectedUserId = (int)data.originalTimesheetInfo.membertime_memberid;
            data.ToBeAddedNegativeTimesheetRecord.PayTypeId = data.originalTimesheetInfo.PayTypeId.Value;
            if (data.originalTimesheetInfo.prov_id.HasValue)
            {
                data.ToBeAddedNegativeTimesheetRecord.prov_id = data.originalTimesheetInfo.prov_id.Value;
            }
            data.ToBeAddedNegativeTimesheetRecord.MemberTime_WoComment_ID = 0;
            data.ToBeAddedNegativeTimesheetRecord.NumberOfHours = (-1) * data.originalTimesheetInfo.Hours; // Negative will be always in there.
            data.ToBeAddedNegativeTimesheetRecord.Rating = data.originalTimesheetInfo.rating;
            data.ToBeAddedNegativeTimesheetRecord.PercentComplete = data.originalTimesheetInfo.wo_percent_complete;
            data.ToBeAddedNegativeTimesheetRecord.MemberTime_WoComment = data.originalTimesheetInfo.Comments + " ( Transferred from " + data.originalTimesheetInfo.bvwo + " [negative record] )";
            data.ToBeAddedNegativeTimesheetRecord.SelectedCustomerId = (int)data.originalTimesheetInfo.MemberTime_Customer_ID;
            data.ToBeAddedNegativeTimesheetRecord.SelectedCustomerName = data.originalTimesheetInfo.membertime_customer_name;
            data.ToBeAddedNegativeTimesheetRecord.SelectedWorkOrderId = int.Parse(data.originalTimesheetInfo.membertime_workorder_id);
            data.ToBeAddedNegativeTimesheetRecord.SelectedWorkOrderName = data.originalTimesheetInfo.bvwo; // take a id see...
            data.ToBeAddedNegativeTimesheetRecord.SelectedTsLiteType = 0;
            data.ToBeAddedNegativeTimesheetRecord.entry_type = "single";
            data.ToBeAddedNegativeTimesheetRecord.selectedJobType = data.originalTimesheetInfo.membertype_id.Value;
            data.ToBeAddedNegativeTimesheetRecord.allow_jobtype_selection = data.timesheetValue.allow_jobtype_selection;

            // Transfer setup
            data.ToBeAddedNegativeTimesheetRecord.IsTransfer = true;
            data.ToBeAddedNegativeTimesheetRecord.transferInfo = "N"; //negative
            data.ToBeAddedNegativeTimesheetRecord.originalChargeoutId = (int)data.originalTimesheetInfo.membertype_chargeout_id.Value;
            data.ToBeAddedNegativeTimesheetRecord.OriginalPrice = data.originalTimesheetInfo.sell;
            data.ToBeAddedNegativeTimesheetRecord.OriginalCost = data.originalTimesheetInfo.cost;
            data.ToBeAddedNegativeTimesheetRecord.originalMemberTypeid = (int)data.originalTimesheetInfo.membertype_id;

            // for parent.
            data.ToBeAddedNegativeTimesheetRecord.orginalChargeoutIdForParentWorkOrderLaborLIine = MasterIdForParentWorkOrder(
                data.originalTimesheetInfo.MemberTime_WOProg_id.Value,
                data.originalTimesheetInfo.HourTypeId,
                false,
                (int)data.originalTimesheetInfo.membertype_id,
                "");

            return true;
        }

        private string _InnerTransfer_Wo_to_WoOrShop_Positive(TimesheetTransferData data, bool isWo)
        {
            string positiveResult = "";
            var ts = new BLL.Pages.Timesheet.Timesheet(this._currentUser);
            if (isWo)
            {
                positiveResult = ts.SaveWorkOrder(this._selected, data.ToBeAddedPositiveTimesheetRecord);
            }
            else
            {
                positiveResult = ts.SaveShop(this._selected, data.ToBeAddedPositveShopTimeRecord);
            }
            // var id = data.ToBeAddedNegativeTimesheetRecord.IdForTransfer;
            return positiveResult;
        }

        private string _InnerTransfer_Wo_to_WoOrShop_Negative(TimesheetTransferData data)
        {
            var positiveResult = new BLL.Pages.Timesheet.Timesheet(this._currentUser).SaveWorkOrder(this._selected, data.ToBeAddedNegativeTimesheetRecord);
            return positiveResult;
        }

        #endregion

        private bool _IsTransferable(DTO.ViewModels.Page.TimeSheet.Timesheet timesheet)
        {
            if (timesheet == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(timesheet.WOType))
            {
                return false;
            }

            //
            // Rule 1: only for timesheet record.
            //
            if (timesheet.WOType.ToLower() != "wo".ToLower())
            {
                return false;
            }

            //
            // Mileage record is anoher of wo, so check again.
            //
            if (timesheet.MemberTime_PayTypeHours_ID.HasValue)
            {
                if (timesheet.MemberTime_PayTypeHours_ID.Value == 8)
                {
                    return false;
                }
            }

            //
            // Rule 2: only for positive hours timesheet record.
            //
            if (timesheet.Hours <= 0)
            {
               
                return false;
            }

            //
            // Rule 3: Previous pay period check
            //
            if (!this._IsTransferable_FromPreviousPayPeriod(timesheet))
            {
                // Not from previous pay period, can't transfer
                return false;
            }

            //
            // Rule 4: related workorder not invoiced.
            //
            if (this._IsTransferable_IsInvoiced(timesheet))
            {
                // invoiced, can't transfer.
                return false;
            }

            //
            // Rule 5: not transfered before.
            //
            if (this._IsTransferable_IsTransferedBefore(timesheet))
            {
                return false;
            }

            return true;
        }

        private bool _IsTransferable_IsTransferedBefore(DTO.ViewModels.Page.TimeSheet.Timesheet timesheet)
        {
            //
            // Todo: Need a transfer flag? if so it needs to add a new column in membertime table?
            //

            //
            // current way: by using the chain setting.
            //
            if (!timesheet.OrigMemberTime_ID.HasValue)
            {
                //
                // Do one more check due to no pointer from original one.
                //

                var query = "SELECT COUNT(1) FROM membertime  WHERE OrigMemberTime_ID = @v0";

                var params4Sql = new object[]
                {
                    timesheet.MemberTime_ID
                };


                var referencedCount = 0;
                try
                {
                    referencedCount = Toolbox.doSQL_int(query, params4Sql);
                }
                catch (Exception ex)
                {
                }

                if (referencedCount > 0)
                {
                    return true;
                }
                
                // no value
                return false;
            }

            if (timesheet.MemberTime_ID == timesheet.OrigMemberTime_ID)
            {
                return true;
            }

            if (timesheet.MemberTime_ID != timesheet.OrigMemberTime_ID)
            {
                //
                // Not allowed to transfer positive record. (TBD)
                //
                return true;
            }

            return false;
        }

        public bool _IsTransferable_FromPreviousPayPeriod(DTO.ViewModels.Page.TimeSheet.Timesheet timesheet)
        {
            // The logic from Matt:
            // SELECT payperiodid-1 pastpayperiodid FROM payperiods WHERE completed = 0 ORDER BY payperiodid ASC  LIMIT 1;

            var sql = @"SELECT payperiodid pastpayperiodid FROM payperiods WHERE completed = 0 ORDER BY payperiodid ASC  LIMIT 1";

            var params4Sql = new object[]
            {
            };

            var currentpayperiod = 0;

            try
            {
                currentpayperiod = Toolbox.doSQL_int(sql, params4Sql);
            }
            catch (Exception ex)
            {
                currentpayperiod = 0;
            }

            if (currentpayperiod == 0)
            {
                // If can't get a value from db, say not in prepayperiod.
                return false;
            }

            if (currentpayperiod > timesheet.payperiod_id)
            {
                return true;
            }

            // Not in prepayperiod.
            return false; // for testing return true, must change to false.
        }

        public bool _IsTransferable_IsInvoiced(DTO.ViewModels.Page.TimeSheet.Timesheet timesheet)
        {
            Timesheet ts = new Timesheet();
            var entity = ts.GetWOPROGById(timesheet.MemberTime_WOProg_id.Value);
            if (entity == null)
            {
                // if not found, return true will not be allowed to touch.
                return true;
            }

            if (string.IsNullOrWhiteSpace(entity.status))
            {
                return false;
            }

            if (entity.status.ToLower() == "Invoiced".ToLower())
            {
                // invoiced.
                return true;
            }

            return false;
        }
    }
}
