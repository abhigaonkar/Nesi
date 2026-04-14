using System;
using System.Collections.Generic;
using System.Data;
using nesi.core;

namespace NESI.BLL.Pages.Timesheet.BreakTime
{
    public class BreakTimeService : IBreakTimeService
    {
        public BreakTimeRecordResult AddBreakTimeRecord(BreakTimeRecord recordParameter)
        {
            var record = this._AddBreakTimeRecord(recordParameter);
            return record;
        }

        public BreakTimeRecordRequirementRecord GetBranchBreakTimeRequirement(BranchBreakTimeRequirementInputParameter inputParameter)
        {
            return this._GetBranchBreakTimeRequirement(inputParameter);
        }

        public List<BreakTimeRecord> GetBreakTimeRecord(BreakTimeQueryParameter inputParameter)
        {
            var records = this._GetBreakTimeRecord(inputParameter);
            return records;
        }

        public BreakTimeRecordOnGivenDate GetBreakTimeRequirementOnSpecificDate(BreakTimeQueryParameter inputParmeter)
        {
            var requireBreakTimeRecordOnSpecificDate = this._GetBreakTimeRequirementOnSpecificDate(inputParmeter);
            return requireBreakTimeRecordOnSpecificDate;
        }

        public BreakTimeRecordResult UpdateBreakTimeRecord(BreakTimeRecord recordParameter)
        {
            return this._UpdateBreakTimeRecord(recordParameter);
        }

        #region private
        private BreakTimeRecordRequirementRecord _GetBranchBreakTimeRequirement(BranchBreakTimeRequirementInputParameter inputParameter)
        {
            var record = new BreakTimeRecordRequirementRecord() { monitor_breaktime = false};
            if (inputParameter == null || inputParameter.business_unit_id < 0)
            {
                return record;
            }

            var sql = "SELECT * FROM business_unit WHERE id = @v0";
            var dt = Toolbox.doSQL_dt(sql, new object[] {inputParameter.business_unit_id});
            if (dt == null || dt.Rows == null || dt.Rows.Count != 1)
            {
                return record;
            }

            var dr = dt.Rows[0];

            try
            {
                var col = dr["def_start_time"];
            }
            catch (Exception ex)
            {
                // script not run.
                return record;
            }

            TimeSpan def_start_time;
            if (dr["def_start_time"] == DBNull.Value ||
                string.IsNullOrWhiteSpace(dr["def_start_time"].ToString()) ||
                (!TimeSpan.TryParse(dr["def_start_time"].ToString(), out def_start_time)))
            {
                return record;
            }
            record.def_start_time = def_start_time;

            TimeSpan def_morning_start;
            if (dr["def_morning_start"] == DBNull.Value ||
                string.IsNullOrWhiteSpace(dr["def_morning_start"].ToString()) ||
                (!TimeSpan.TryParse(dr["def_morning_start"].ToString(), out def_morning_start)))
            {
                return record;
            }
            record.def_morning_start = def_morning_start;
            record.def_morning_dur = Convert.ToInt32(dr["def_morning_dur"]);

            TimeSpan def_lunch_start;
            if (dr["def_lunch_start"] == DBNull.Value ||
                string.IsNullOrWhiteSpace(dr["def_lunch_start"].ToString()) ||
                (!TimeSpan.TryParse(dr["def_lunch_start"].ToString(), out def_lunch_start)))
            {
                return record;
            }
            record.def_lunch_start = def_lunch_start;
            record.def_lunch_dur = Convert.ToInt32(dr["def_lunch_dur"]);


            TimeSpan def_afternoon_start;
            if (dr["def_afternoon_start"] == DBNull.Value ||
                string.IsNullOrWhiteSpace(dr["def_afternoon_start"].ToString()) ||
                (!TimeSpan.TryParse(dr["def_afternoon_start"].ToString(), out def_afternoon_start)))
            {
                return record;
            }
            record.def_afternoon_start = def_afternoon_start;
            record.def_afternoon_dur = Convert.ToInt32(dr["def_afternoon_dur"]);

            record.monitor_breaktime = Convert.ToBoolean(dr["monitor_breaktime"]);
            return record;
        }

        private BreakTimeRecordResult _AddBreakTimeRecord(BreakTimeRecord recordParameter)
        {
            var record = new BreakTimeRecordResult() {success = false, operationType = OperationType.Add, date= DateTime.Now, locked = false};
            if(recordParameter == null ||
               recordParameter.business_unit_id == 0 ||
               recordParameter.member_id == 0 ||
               recordParameter.entered_by_member_id == 0 ||
               recordParameter.id != 0)
            {
                record.reason = "Input parameters are invalid.";
                return record;
            }

            // Check No record already inserted with info like bu, member and Date.
            var query = new BreakTimeQueryParameter
            {
                business_unit_id = recordParameter.business_unit_id,
                member_id = recordParameter.member_id,
                date = recordParameter.date
            };

            var list = this._GetBreakTimeRecord(query);
            if (list.Count > 0)
            {
                // Breaktime Records is already added, return ID.
                record.success = true;
                record.reason = "Breaktime record is successfully inserted.";
                record.id = list[0].id;
                return record;
            }

            var insertSQL = @"
INSERT INTO breaktime (
  DATE,
  business_unit_id,
  member_id,
  start_time,
  morning_break_start,
  morning_break_duration,
  lunch_break_start,
  lunch_break_duration,
  afternoon_break_start,
  afternoon_break_duration,
  entered_by_member_id
)
VALUES
  (
    @v0,
    @v1,
    @v2,
    @v3,
    @v4,
    @v5,
    @v6,
    @v7,
    @v8,
    @v9,
    @v10
  )
";
            var parameters = new object[]
            {
                recordParameter.date,
                recordParameter.business_unit_id,
                recordParameter.member_id,
                recordParameter.start_time,
                recordParameter.morning_break_start,
                recordParameter.morning_break_duration,
                recordParameter.lunch_break_start,
                recordParameter.lunch_break_duration,
                recordParameter.afternoon_break_start,
                recordParameter.afternoon_break_duration,
                recordParameter.entered_by_member_id
            };

            var id = 0;
            id = Toolbox.doSQL_return_id(insertSQL, parameters);
            if (id == 0)
            {
                record.reason = "Cannot save the Breaktime record.";
                record.id = id;
            }
            else
            {
                record.success = true;
                record.reason = "Breaktime record is successfully inserted.";
                record.id = id;
            }

            return record;
        }

        private List<BreakTimeRecord> _GetBreakTimeRecord(BreakTimeQueryParameter inputParameter)
        {
            var list = new List<BreakTimeRecord> { };
            if (inputParameter == null ||
                inputParameter.business_unit_id == 0 ||
                inputParameter.member_id == 0 ||
                inputParameter.date == null)
            {
                return list;
            }

            var sql = "SELECT * FROM breaktime WHERE business_unit_id = @v0 AND member_id = @v1 AND DATE = @v2";
            var parameters = new object[]
            {
                inputParameter.business_unit_id,
                inputParameter.member_id,
                inputParameter.date.ToString("yyyy-MM-dd")
            };

            var dt = Toolbox.doSQL_dt(sql, parameters);
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                return list;
            }

            foreach (DataRow dr in dt.Rows)
            {
                var record = new BreakTimeRecord();

                record.id = Convert.ToInt32(dr["id"].ToString());
                record.date = Convert.ToDateTime(dr["DATE"].ToString());
                record.business_unit_id = Convert.ToInt32(dr["business_unit_id"].ToString());
                record.member_id = Convert.ToInt32(dr["member_id"].ToString());

                record.start_time = TimeSpan.Parse(dr["start_time"].ToString());
                record.morning_break_start = TimeSpan.Parse(dr["morning_break_start"].ToString());
                record.morning_break_duration = Convert.ToInt32(dr["morning_break_duration"].ToString());
                record.lunch_break_start = TimeSpan.Parse(dr["lunch_break_start"].ToString());
                record.lunch_break_duration = Convert.ToInt32(dr["lunch_break_duration"].ToString());
                record.afternoon_break_start = TimeSpan.Parse(dr["afternoon_break_start"].ToString());
                record.afternoon_break_duration = Convert.ToInt32(dr["afternoon_break_duration"].ToString());
                record.entered_by_member_id = Convert.ToInt32(dr["entered_by_member_id"].ToString());

                list.Add(record);
            }

            // locked property
            foreach (var record in list)
            {
                if (this.AlreadyPaidDate(record.date, record.member_id))
                {
                    record.locked = true;
                }
            }
            return list;
        }

        private BreakTimeRecordOnGivenDate _GetBreakTimeRequirementOnSpecificDate(BreakTimeQueryParameter inputParmeter)
        {
            /*
             * Checking steps one by one:
             * ===============================================================================================================================
             * (1) if monitor_breaktime for the currrent branch is FALSE, no need to show breaktime stuff.
             * (2) if on the given date, the expected breaktime record has been inserted,  show 'Edit breaktime record button' stuff.
             * (3) if there already has some timesheet records for this given date, no need to show breaktime stuff. It is more likely about not applying this to old dates.
             * (4) in the case of 'Payroll has already been approved for this date, time can't be edited or deleted by non-payroll staff.', no break time record stuff show up.
             * (5) Otherwise, the breaktime record is required for this given date.
             */

            //
            // If no valid parameters, show nothing about 'Break time' stuff.
            //
            var record = new BreakTimeRecordOnGivenDate
            {
                recordId =  0 , showAddingOverlay = false, showEditingButton = false, memberId = 0, date = "",
                breaktimeRecord = new BreakTimeRecord { id = 0 }
            };

            if (inputParmeter == null ||
                inputParmeter.business_unit_id == 0 ||
                inputParmeter.member_id == 0 ||
                inputParmeter.date == null)
            {
                // If inputs are invalid..., show nothing on UI.
                return record;
            }

            //
            // (1) if monitor_breaktime for the currrent branch is FALSE, no need to show breaktime stuff.
            //
            BranchBreakTimeRequirementInputParameter query = new BranchBreakTimeRequirementInputParameter()
            {
                business_unit_id = inputParmeter.business_unit_id,
                member_id = inputParmeter.member_id
            };

            var branchSetting = this._GetBranchBreakTimeRequirement(query);
            if (branchSetting.monitor_breaktime == false)
            {
                return record;
            }

            //
            // (2) if on the given date, the expected breaktime record has been inserted,  show 'Edit breaktime record button' stuff.
            //
            BreakTimeQueryParameter query2 = new BreakTimeQueryParameter()
            {
                business_unit_id = inputParmeter.business_unit_id,
                member_id =  inputParmeter.member_id,
                date = inputParmeter.date
            };

            var records = this._GetBreakTimeRecord(query2);
            if (records.Count > 0)
            {
                // Alreay had one record inserted, no need to add one again.
                record.recordId = records[0].id;
                record.showAddingOverlay = false;
                record.showEditingButton = true;
                record.memberId = records[0].member_id;
                record.date = records[0].date.ToLongDateString();
                record.breaktimeRecord = records[0];
                return record;
            }

            //
            // (3) if there already has some timesheet record for this given date, no need to show breaktime stuff. It is more likely not applying this to old dates.
            //
            var count = this.GetTimesheetRecordCount( inputParmeter.business_unit_id, inputParmeter.member_id, inputParmeter.date);
            if (count > 0)
            {
                record.recordId = 0;
                record.showAddingOverlay = false;
                record.showEditingButton = false;
                return record;
            }

            //
            // (4) in the case of 'Payroll has already been approved for this date, time can't be edited or deleted by non-payroll staff.', no break time record stuff show up.
            //
            var paid = this.AlreadyPaidDate(inputParmeter.date, inputParmeter.member_id);
            if (paid)
            {
                record.recordId = 0;
                record.showAddingOverlay = false;
                record.showEditingButton = false;
                return record;
            }

            //
            // (5) Otherwise, the breaktime record is required for this given date.
            //
            record.showAddingOverlay = true;
            record.showEditingButton = false;

            return record;
        }

        private int GetTimesheetRecordCount(int businessId, int memberid, DateTime date)
        {
            var sql = @"
SELECT MemberTime_ID 
FROM membertime 
WHERE membertime_memberid = @v0 
AND business_unit_id = @v1 
AND Date = @v2
";
            var parameters = new object[]
            {
                memberid,
                businessId,
                date.ToString("yyyy-MM-dd")
            };

            var dt = Toolbox.doSQL_dt(sql, parameters);
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                return 0;
            }
            
            return dt.Rows.Count;
        }

        private bool AlreadyPaidDate(DateTime date, int userId)
        {
            var payperiod_id = NePayPeriod.get_payperiod_id(date);
            if (payperiod_id < 0)
            {
                return false;
            }

            var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM payroll_hours WHERE member_id = @v0  AND payperiod_id = @v1 ",
                new object[] { userId, payperiod_id });

            return c > 0;
        }

        private BreakTimeRecordResult _UpdateBreakTimeRecord(BreakTimeRecord recordParameter)
        {
            var recordResult = new BreakTimeRecordResult() {operationType = OperationType.Edit, success = false};

            if (recordParameter == null ||
                recordParameter.id == 0 ||
                recordParameter.member_id == 0 ||
                recordParameter.business_unit_id == 0)
            {
                recordResult.reason = "Parameters not valid.";
                return recordResult;
            }

            //
            // May check whether we have that record by ID...
            //

            var sql = @"
UPDATE
  breaktime
SET
  start_time = @v0,
  morning_break_start = @v1,
  morning_break_duration = @v2,
  lunch_break_start = @v3,
  lunch_break_duration = @v4,
  afternoon_break_start = @v5,
  afternoon_break_duration = @v6
WHERE id = @v7 
  AND member_id = @v8
  AND business_unit_id = @v9
";
            var parameters = new object[]
            {
                recordParameter.start_time,
                recordParameter.morning_break_start,
                recordParameter.morning_break_duration,
                recordParameter.lunch_break_start,
                recordParameter.lunch_break_duration,
                recordParameter.afternoon_break_start,
                recordParameter.afternoon_break_duration,
                recordParameter.id, //query part.
                recordParameter.member_id,
                recordParameter.business_unit_id
            };

            Toolbox.doSQL_void(sql, parameters);

            // Read the record again
            var query = new BreakTimeQueryParameter
            {
                business_unit_id = recordParameter.business_unit_id,
                member_id = recordParameter.member_id,
                date = recordParameter.date
            };

            var records = this._GetBreakTimeRecord(query);
            if (records.Count == 0)
            {
                recordResult.reason = "No Records Found.";
                return recordResult;
            }

            // Replace with new value.

            recordResult.id = recordParameter.id;
            recordResult.date = recordParameter.date;
            recordResult.reason = "Break Time Record is updated successfully.";
            recordResult.business_unit_id = recordParameter.business_unit_id;
            recordResult.member_id = recordParameter.member_id;

            recordResult.start_time = records[0].start_time;
            recordResult.morning_break_start = records[0].morning_break_start;
            recordResult.morning_break_duration = records[0].morning_break_duration;

            recordResult.lunch_break_start = records[0].lunch_break_start;
            recordResult.lunch_break_duration = records[0].lunch_break_duration;

            recordResult.afternoon_break_start = records[0].afternoon_break_start;
            recordResult.afternoon_break_duration = records[0].afternoon_break_duration;

            recordResult.success = true;
            return recordResult;
        }

        #endregion
    }
}