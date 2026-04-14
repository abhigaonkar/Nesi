using nesi.core;
using NESI.BLL.Base;
using NESI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.NewLaborRates
{
    public class NewLaborRateService : BLLBase, INewLaborRate
    {
        #region api
        public UpdateResult CreateNewLaborRate(UpdateParameter parameter)
        {
            return this._CreateNewLaborRate(parameter);
        }

        public UpdateResult DeleteNewLaborRate(DeleteParameter parameter)
        {
            return this._DeleteNewLaborRate(parameter);
        }

        public List<GenericLaborRateRecord> GetNewLaborRateList(GetNewLaborRateListParameter parameter)
        {
            return this._GetNewLaborRateList(parameter);
        }

        #endregion

        #region private - Getting
        private List<GenericLaborRateRecord> _GetNewLaborRateList(GetNewLaborRateListParameter parameter)
        {
            //
            // Validation
            //
            if (parameter == null || parameter.customerId == 0 || parameter.businessUnitId == 0)
            {
                return new List<GenericLaborRateRecord> { };
            }

            //
            // Query to get the data
            //
            var list = this._GetNewLaborRateList_GetData(parameter);
            return list;
        }

        private List<GenericLaborRateRecord> _GetNewLaborRateList_GetData(GetNewLaborRateListParameter parameter)
        {
            var visible = true;

            var list = new List<GenericLaborRateRecord> { };

            var query = @"

SELECT  r.customer_id, c.business_unit_id, c.paytype_id, COALESCE(r.chargeout, c.chargeout) chargeout , r.from_date, r.to_date, p.Description
FROM chargeout c
INNER JOIN paytypehours p ON p.PayTypeHours_ID = c.paytype_id 
LEFT JOIN customer_rate r ON c.id = r.base_chargeout_id AND r.customer_id = @v1
WHERE c.paytype_id IN (7, 8)
AND c.business_unit_id = @v0
AND (r.customer_id IS NULL OR r.customer_id = @v2 )
GROUP BY c.paytype_id
";

            // Quer parameters
            var parameters = new object[] { parameter.businessUnitId, parameter.customerId, parameter.customerId };

            try
            {
                var dt = Toolbox.doSQL_dt(query, parameters );
                if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                {
                    // No data. this may happen if no data in 'chargeout' table.
                    return list;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    var record = new GenericLaborRateRecord();

                    record.customer_id = dr.IsNull("customer_id") ? 0 : Convert.ToInt32(dr["customer_id"].ToString());
                    record.business_unit = Convert.ToInt32(dr["business_unit_id"].ToString());

                    record.paytype_id = Convert.ToInt32(dr["paytype_id"].ToString());
                    record.chargeout = Convert.ToDecimal(dr["chargeout"].ToString());

                    record.from = (DateTime?)(dr.IsNull("from_date") ? (DateTime?)null : Convert.ToDateTime(dr["from_date"]));
                    record.to = (DateTime?)(dr.IsNull("to_date") ? (DateTime?)null : Convert.ToDateTime(dr["to_date"]));

                    record.paytype = dr["Description"].ToString();

                    record.visible = visible;

                    record.valid = true;

                    list.Add(record);
                }
            }
            catch(Exception ex)
            {
                // Error in here.
            }

            return list;
        }
        #endregion

        #region private - creating
        private UpdateResult _CreateNewLaborRate(UpdateParameter parameter)
        {
            //
            // Validation
            //
            var result = this.Validation(parameter);
            if (!result.okay)
            {
                return result;
            }
            
            //
            // Generate records.
            //
            var r = this._CreateNewLaborRate_Generate_Redcords(parameter);
            return r;
        }


        private UpdateResult _CreateNewLaborRate_Generate_Redcords(UpdateParameter parameter)
        {
            var r = new UpdateResult();
            r.okay = true;
            r.error = "Customer rate has been saved successfully.";

            //
            // Step 1: Get all (Travel/Mileage) records in chargeout table for given business unit, then focus on membetype field.
            //
            var chargeoutRecords = _db.chargeout.Where(
                x => x.business_unit_id == parameter.business_unit &&
                     x.paytype_id == parameter.paytype_id
            ).ToList();

            if (chargeoutRecords == null || chargeoutRecords.Count() == 0)
            {
                // We don't set up chargeout for this given business for travel or mileage.
                r.okay = false;
                r.error = "You should setup the charge out for this branch on chargeout table first.";
                return r;
            }

            var chargeoutList = chargeoutRecords.Select(x => x.id).ToList();

            //
            // Step 2: Get all customer records created before in customer_rate table for given business unit(over master id) & customer.
            //
            var customerRecords = _db.customer_rate.Where(
                x => x.customer_id == parameter.customer_id &&
                     chargeoutList.Contains(x.base_chargeout_id)
                ).ToList();

            //
            // Step 3: (a) any membertype show in [step 1] but NOT  show in [step 2], we will create a new record in customer_rate table;
            //         (b) any membertype show in [step 1] but also show in [step 2], we will update the existing record in customer_rate table.
            //
            bool allDone = true;
            DateTime now = DateTime.Now;
            foreach (var record in chargeoutRecords)
            {
                bool found = false;
                customer_rate foundRecord = null;
                foreach (var customer in customerRecords)
                {
                    if (record.id == customer.base_chargeout_id)
                    {
                        found = true;
                        foundRecord = customer;
                        break;
                    }
                }

                try
                {
                    if (found)
                    {
                        //
                        // (b) - update
                        //
                        var cr = _db.customer_rate.Where(x => x.id == foundRecord.id).First();
                        cr.chargeout = parameter.chargeout;
                        cr.from_date = parameter.from.Value.Date;
                        cr.to_date = parameter.to.Value.Date;
                        cr.last_updated = now;
                    }
                    else
                    {
                        //
                        // (a) - insert
                        //
                        var cr = new customer_rate();
                        cr.address_id = 0;
                        cr.notification_sent = 0;
                        cr.from_date = parameter.from.Value.Date;
                        cr.to_date = parameter.to.Value.Date;
                        cr.last_updated = now;
                        cr.chargeout = parameter.chargeout; // from ui
                        cr.base_chargeout_id = record.id; // from chargeout table
                        cr.customer_id = parameter.customer_id; // from ui

                        _db.customer_rate.Add(cr);
                    }
                }
                catch (Exception ex)
                {
                    //
                    // once has problem....
                    //
                    allDone = false;
                    break;
                }
            }

            if (!allDone)
            {
                //
                // Failed to add, if so we do nothing. _db is local, so no need to revert any changes.
                //
                r.okay = false;
                r.error = "Save not done, please try again";
            }
            else
            {
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception e)
                {
                    r.okay = false;
                    r.error = "Failed to save the customer rate.";
                }
            }
            
            return r;
        }

        private UpdateResult Validation(UpdateParameter parameter)
        {
            UpdateResult r = new UpdateResult();
            if (parameter == null)
            {
                r.okay = false;
                r.error = "No valid inputs";
                return r;
            }

            if (parameter.business_unit == 0)
            {
                r.okay = false;
                r.error = "No business unit selected";
                return r;
            }

            if (parameter.customer_id == 0)
            {
                r.okay = false;
                r.error = "No Customer selected";
                return r;
            }

            if (parameter.chargeout <= 0)
            {
                r.okay = false;
                r.error = "Charge out can't be zero or less.";
                return r;
            }

            if (parameter.paytype_id == 7 || parameter.paytype_id == 8)
            {
                // supported pay hour type.
            }
            else
            {
                r.okay = false;
                r.error = "You can only setup the charge out for travel or mileage.";
                return r;
            }

            //
            // Check the from & to.
            //
            if (parameter.to == null || parameter.from == null)
            {
                r.okay = false;
                r.error = "Please enter a valid date range. The From date must precede the To date.";
                return r;
            }

            if (parameter.to != null && parameter.from != null)
            {
                // both are not null.
                if (parameter.from < parameter.to)
                {
                    r.okay = true;
                    r.error = "";
                    return r;
                }
            }

            // from & to not good.
            r.okay = false;
            r.error = "Please enter a valid date range. The From date must precede the To date.";
            return r;
        }

        #endregion

        #region delete
        private UpdateResult _DeleteNewLaborRate(DeleteParameter parameter)
        {
            //
            // Validation
            //
            var result = this.ValidationOnDelete(parameter);
            if (!result.okay)
            {
                return result;
            }

            return this._DeleteNewLaborRate_DeleteRecords(parameter);
        }

        private UpdateResult _DeleteNewLaborRate_DeleteRecords(DeleteParameter parameter)
        {
            var r = new UpdateResult();
            r.okay = true;
            r.error = "Customer rate has been deleted successfully.";

            //
            // Step 1: Get all (Travel/Mileage) records in chargeout table for given business unit, then focus on membetype field.
            //
            var chargeoutRecords = _db.chargeout.Where(
                x => x.business_unit_id == parameter.business_unit &&
                     x.paytype_id == parameter.paytype_id
            ).ToList();

            if (chargeoutRecords == null || chargeoutRecords.Count() == 0)
            {
                // We don't set up chargeout for this given business for travel or mileage.
                r.okay = true;
                r.error = "No chargeout Records were found.";
                return r;
            }

            var chargeoutList = chargeoutRecords.Select(x => x.id).ToList();

            //
            // Step 2: Get all customer records created before in customer_rate table for given business unit(over master id) & customer.
            //
            var customerRecords = _db.customer_rate.Where(
                x => x.customer_id == parameter.customer_id &&
                     chargeoutList.Contains(x.base_chargeout_id)
                ).ToList();

            if (customerRecords == null || customerRecords.Count() == 0)
            {
                // No recors in customer_rate table, so just say all gone.
                r.error = "No customer rate records were found to be deleted.";
                return r;
            }

            //
            // Step 3: delete all found records.
            //
            foreach (var record in customerRecords)
            {
                _db.customer_rate.Remove(record);
            }

            //
            // Commit ....
            //
            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                r.okay = false;
                r.error = "Failed to delete the customer rate records.";
            }

            return r;
        }

        private UpdateResult ValidationOnDelete(DeleteParameter parameter)
        {
            UpdateResult r = new UpdateResult();
            if (parameter == null)
            {
                r.okay = false;
                r.error = "No valid inputs";
                return r;
            }

            if (parameter.business_unit == 0)
            {
                r.okay = false;
                r.error = "No business unit selected";
                return r;
            }

            if (parameter.customer_id == 0)
            {
                r.okay = false;
                r.error = "No Customer selected";
                return r;
            }

            if (parameter.paytype_id == 7 || parameter.paytype_id == 8)
            {
                // supported pay hour type.
            }
            else
            {
                r.okay = false;
                r.error = "You can only setup the charge out for travel or mileage.";
                return r;
            }

            r.okay = true;
            return r;
        }
        #endregion
    }
}
