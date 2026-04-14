using MySql.Data.MySqlClient;
using nesi.core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESI.Common.Models;

namespace NESI.BLL.Pages.AssetRate
{
    public class AssetCustomerRateService : IAssetCustomerRate
    {
        public OperationResult Add(AssetCustomerRateUpdateParameter parameter)
        {
            return this._AddOrUpdate(parameter);
        }

        public OperationResult Delete(AssetCustomerRateDeleteParameter parameter)
        {
            return this._Delete(parameter);
        }

        public AssetCustomerRateResult Get(AssetCustomerRateQueryParameter parameter)
        {
            return this._Get(parameter);
        }

        public List<AssetCustomerRateRecord> GetAvailableAssetForWorkorderLine(AssetQueryForWorkOrderParameter parameter)
        {
            throw new NotImplementedException();
        }

        public OperationResult Update(AssetCustomerRateUpdateParameter parameter)
        {
            return this._AddOrUpdate(parameter);
        }

        public List<BuisnessUnitRecord> getAvailableBusinessUnits(GetVisiableBUParameter parameter)
        {
            return this._getAvailableBusinessUnits(parameter);
        }

        #region Get
        private AssetCustomerRateResult _Get(AssetCustomerRateQueryParameter parameter)
        {
            AssetCustomerRateResult result = new AssetCustomerRateResult
            {
                data = new List<AssetCustomerRateRecord> { },
                totalCount = 0
            };

            if (parameter == null || parameter.pageSize <= 0 || parameter.skipPage < 0 || parameter.business_unit_id <= 0 || parameter.customer_id <= 0)
            {
                return result;
            }

            int pageSize = parameter.pageSize;
            int skipSize = parameter.skipPage;

            var searchQuery = " 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(parameter.searchingstring))
            {
                searchQuery = @" (COALESCE( NULLIF(a.description, ''), CONCAT(b.asset_type_name, ' - ', a.assets_make, ' ', a.assets_model)) LIKE '%{0}%')";
                searchQuery = string.Format(searchQuery, parameter.searchingstring);
            }

            //
            // Get How many records in there.
            //
            var queryToGetTotal = @"
SELECT
  count(1)
FROM
  assets a
  INNER JOIN assets_type b ON a.assets_type = b.asset_type_id
  LEFT JOIN asset_customer_rate c ON c.asset_id = a.assets_ID AND c.customer_id = @v1      
WHERE a.business_unit_id = @v0 AND "
+ searchQuery;

            try
            {
                result.totalCount = Toolbox.doSQL_int(queryToGetTotal, new object[] { parameter.business_unit_id, parameter.customer_id });
            }
            catch
            {
            }

            if (result.totalCount == 0)
            {
                // No data.
                return result;
            }

            // Todo: Left join should limit to 1 record.
            var queryToGetData = @"
SELECT
  (a.business_unit_id * 100000000 + a.assets_ID  * 1000 + COALESCE(c.id, 0) ) keyID,
  a.assets_ID,
  COALESCE(  NULLIF(a.description, ''), CONCAT(b.asset_type_name, ' - ', a.assets_make, ' ', a.assets_model)) description,
  c.start_date,
  c.end_date,
  COALESCE(c.ts, a.ts) last_updated,
  COALESCE(c.daily,   a.daily) daily,
  COALESCE(c.weekly,  a.weekly) weekly,
  COALESCE(c.monthly, a.monthly) monthly,
  a.business_unit_id,
  COALESCE(c.id, 0) asset_customer_rate_id,
  COALESCE(c.customer_id, 0) customerId
FROM
  assets a
  INNER JOIN assets_type b ON a.assets_type = b.asset_type_id
  LEFT JOIN asset_customer_rate c ON c.asset_id = a.assets_ID AND c.customer_id = @v3      
WHERE a.business_unit_id = @v0 AND "
+ searchQuery
+ @"ORDER BY keyID DESC LIMIT @v1 OFFSET @v2";

            try
            {
                var dt = Toolbox.doSQL_dt(queryToGetData, new object[] { parameter.business_unit_id, pageSize, skipSize, parameter.customer_id });
                if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                {
                    return result;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    var record = new AssetCustomerRateRecord();

                    record.keyId = Convert.ToInt64(dr["keyID"].ToString()); // for sort

                    record.asset_id = Convert.ToInt32(dr["assets_ID"].ToString());
                    record.asset_des = "(" + record.asset_id.ToString() + ") " +  dr["description"].ToString();
                    record.id = Convert.ToInt32(dr["asset_customer_rate_id"].ToString());

                    record.daily = Convert.ToDecimal(dr["daily"].ToString());
                    record.monthly = Convert.ToDecimal(dr["monthly"].ToString());
                    record.weekly = Convert.ToDecimal(dr["weekly"].ToString());

                    record.customer_id = Convert.ToInt32(dr["customerId"].ToString());
                    record.start = (DateTime?)(dr.IsNull("start_date") ? (DateTime?)null : Convert.ToDateTime(dr["start_date"]));
                    record.end = (DateTime?)(dr.IsNull("end_date") ? (DateTime?)null : Convert.ToDateTime(dr["end_date"]));
                    record.lastupdate = (dr.IsNull("last_updated") ? "" : Convert.ToDateTime(dr["last_updated"]).ToString());
                    record.business_unit_id = Convert.ToInt32(dr["business_unit_id"].ToString());

                    record.can_delete = record.id;

                    result.data.Add(record);
                }
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }

            return result;
        }
        #endregion

        #region add/update
        private string _Already_InsertedInfo = "This record has been updated by other user, please cancel this change then refresh to see the new value.";
        private OperationResult _AddOrUpdate(AssetCustomerRateUpdateParameter parameter)
        {
            OperationResult result = new OperationResult { okay = 0, message = "", errors = new List<ErrorRecord> { } };
            if (parameter == null)
            {
                result.message = "No valid parameter passed in.";
                return result;
            }

            if (parameter.start.HasValue)
            {
                parameter.start = new DateTime(parameter.start.Value.Year, parameter.start.Value.Month, parameter.start.Value.Day, 0, 0, 0);
            }

            if (parameter.end.HasValue)
            {
                parameter.end = new DateTime(parameter.end.Value.Year, parameter.end.Value.Month, parameter.end.Value.Day, 23, 59, 59);
            }

            var errors = _Validatioin(parameter);
            if (errors.Count() > 0)
            {
                result.errors = errors;
                result.message = errors[0].error; // "Parameters are invalid.";
                return result;
            }

            if (parameter.id == 0)
            {
                // Add a new record in asset_customer_rate table.
                errors = this._Add(parameter);
            }
            else
            {
                // update the existing value in asset_customer_rate.
                errors = this._Update(parameter);
            }

            result.message = "Customer asset record has been saved successsfully.";
            if (errors.Count() > 0)
            {
                result.errors = errors;
                result.message = "Cannot add/update the customer rate for this asset. ( " + errors[0].error + " )";
                return result;
            }

            result.okay = 1;
            return result;
        }

        private List<ErrorRecord> _Add(AssetCustomerRateUpdateParameter parameter)
        {
            var errors = new List<ErrorRecord> { };

            var sql = @"
INSERT INTO asset_customer_rate (
  start_date,
  end_date,
  asset_id,
  customer_id,
  daily,
  weekly,
  monthly
)
VALUES
  (
    @v0,
    @v1,
    @v2,
    @v3,
    @v4,
    @v5,
    @v6
  )";

            var params4Sql = new object[]
            {
                parameter.start,
                parameter.end,
                parameter.asset_id,
                parameter.customerId,
                parameter.daily,
                parameter.weekly,
                parameter.monthly
            };

            var id = 0;
            try
            {
                id = Toolbox.doSQL_return_id(sql, params4Sql);
            }
            catch (MySqlException ex)
            {
                // what if it already created.
                if (ex.Number == 1062)
                {
                    errors.Add(new ErrorRecord { error = this._Already_InsertedInfo, field = "" });
                }
            }

            if (id == 0)
            {
                errors.Add(new ErrorRecord { error = "Not able to create a new customer rate.", field = "" });
            }

            return errors;
        }

        private List<ErrorRecord> _Update(AssetCustomerRateUpdateParameter parameter)
        {
            var errors = new List<ErrorRecord> { };


            var sql = @"
UPDATE
  asset_customer_rate
SET
  start_date = @v0,
  end_date =  @v1,
  daily = @v2,
  weekly =  @v3,
  monthly = @v4
WHERE id = @v5 AND asset_id = @v6 AND customer_id = @v7";

            var params4Sql = new object[]
            {
                parameter.start,
                parameter.end,
                parameter.daily,
                parameter.weekly,
                parameter.monthly,
                parameter.id,
                parameter.asset_id,
                parameter.customerId,
            };

            //
            // Todo: Check existing...
            //

            try
            {
                // Update a-not-existing-item will be causing do nothing in db side, and the angular side will be updated to see the default value.
                Toolbox.doSQL_void(sql, params4Sql);
            }
            catch (Exception ex)
            {
                errors.Add(new ErrorRecord {  error = ex.Message, field=""});
            }

            return errors;
        }

        private OperationResult _Delete(AssetCustomerRateDeleteParameter parameter)
        {
            OperationResult result = new OperationResult { okay = 0, message = "", errors = new List<ErrorRecord> { } };

            if (parameter == null || parameter.id <= 0 || parameter.asset_id <= 0 || parameter.customerId <= 0)
            {
                // We do more checks...
                result.message = "No valid parameter passed in.";
                return result;
            }

            var sql = @"
DELETE FROM asset_customer_rate
WHERE id = @v0 AND asset_id = @v1 AND customer_id = @v2";

            var params4Sql = new object[]
            {
                parameter.id,
                parameter.asset_id,
                parameter.customerId,
            };

            try
            {
                Toolbox.doSQL_void(sql, params4Sql);
                result.okay = 1;
                result.message = "Customer asset rate has been deleted.";
            }
            catch (Exception ex)
            {
                result.errors.Add(new ErrorRecord { error = ex.Message, field = "" });
                result.message = result.errors[0].error;
            }

            return result;
        }


        private List<ErrorRecord> _Validatioin(AssetCustomerRateUpdateParameter parameter)
        {
            var errors = new List<ErrorRecord> { };
            
            if (parameter.start.HasValue && parameter.end.HasValue)
            {
                if (parameter.start >= parameter.end)
                {
                    errors.Add(new ErrorRecord {  field= "",  error = "Please enter a valid date range. The From date must precede the To date." });
                    return errors;
                }
            }

            if (parameter.start.HasValue && !parameter.end.HasValue)
            {
                errors.Add(new ErrorRecord { field = "", error = "Please enter a valid date range. The From date must precede the To date." });
                return errors;
            }

            if (!parameter.start.HasValue && parameter.end.HasValue)
            {
                errors.Add(new ErrorRecord { field = "", error = "Please enter a valid date range. The From date must precede the To date." });
                return errors;
            }

            if (parameter.daily < 0)
            {
                errors.Add(new ErrorRecord { field = "", error = "Daily Fee should not be less than 0" });
                return errors;
            }

            if (parameter.weekly < 0)
            {
                errors.Add(new ErrorRecord { field = "", error = "Weekly Fee should not be less than 0" });
                return errors;
            }

            if (parameter.monthly < 0)
            {
                errors.Add(new ErrorRecord { field = "", error = "Monthly Fee should not be less than 0" });
                return errors;
            }

            return errors;
        }

        #endregion

        #region visible business units
        private List<BuisnessUnitRecord> _getAvailableBusinessUnits(GetVisiableBUParameter parameter)
        {
            var list = new List<BuisnessUnitRecord> { };
            // Reducing the bu list on the basis of privilege. 
            var CurrentUser = new NeMember(parameter.userId);
            var is_allowed_to_switch_bu = CurrentUser.AuthenticatedForPrivilege(OpsPrivilege.ChargeoutSettingsBusinessUnitSwitch);

            var sql = is_allowed_to_switch_bu ?
                            @"SELECT
                                a.id VALUE,
                                a.ddl_name label
                            FROM
                                business_unit a
                            INNER JOIN tax_entity b
                            ON a.tax_entity_id = b.id
                            WHERE b.is_active
                            AND a.name NOT LIKE 'MASTER%'
                            ORDER BY a.name"
                         :
                            @"SELECT 
                                id VALUE,
                                ddl_name label
                            FROM
                                business_unit
                            WHERE 
                            id = @v0";

            try
            {
                var paramsList = is_allowed_to_switch_bu ? new object[] { } : new object[] { CurrentUser.business_unit_id };
                var dt = Toolbox.doSQL_dt(sql, paramsList);
                if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                {
                    return list;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    var record = new BuisnessUnitRecord();

                    record.id = Convert.ToInt32(dr["VALUE"].ToString());
                    record.name = dr["label"].ToString();

                    list.Add(record);
                }

                return list;
            }
            catch
            {
            }

            return list;
        }
        #endregion
    }
}
