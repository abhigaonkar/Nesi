using System;
using NESI.Common.Models;
using nesi.core;

namespace NESI.BLL.Pages.Timesheet.BreakTime
{
    public class BreakTimeRecordLog
    {
        public BreakTimeRecord PrviousRecord { get; set; }
        public BreakTimeRecord PostRecord { get; set; }

        public  int SectionId { get; set; } // the id from log_section table
        public int BusinessId { get; set; } // the branch for this break time record.
        public int MemberId { get; set; } //who owns this breaktime record.
        public bool Is_manual { get; set; } //true
        public string TableName { get; set; } //breaktime
        public int TableId { get; set; } // the primary Key for this breaktime record.
        public int Alt_Table_Id { get; set; } // Person Edting this breaktime Record.
        public int ActionId { get; set; } // The Id from log_action table.
        public string Value_Old { get; set; }
        public string Value_New { get; set; }
    }


    public class BreakTimeRecordLogUpdater
    {
        public static bool Log(BreakTimeRecordLog logData)
        {
            if (!LoadData(logData))
            {
                return false;
            }

            var neLog = new NELog();
            neLog.section_id = logData.SectionId;
            neLog.business_unit_id = logData.BusinessId;
            neLog.member_id = logData.MemberId;
            neLog.is_manual = logData.Is_manual;
            neLog.table = logData.TableName;
            neLog.table_id = logData.TableId;
            neLog.alt_table_id = logData.Alt_Table_Id;
            neLog.action_id = logData.ActionId;
            neLog.value_old = logData.Value_Old;
            neLog.value_new = logData.Value_New;

            try
            {
                neLog.save();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private static bool LoadData(BreakTimeRecordLog logData)
        {
            //
            // Section ID, check table.
            //
            logData.SectionId = OpsLog.Section.TimeSheet;

            //
            // Action ID, check table.
            //
            logData.ActionId = OpsLog.Action.AdjustedBreakTimeRecord;

            // Other.
            logData.BusinessId = logData.PrviousRecord.business_unit_id;
            logData.MemberId = logData.PrviousRecord.member_id;
            logData.Is_manual = true;
            logData.TableName = OpsLog.Table.BreakTime;
            logData.TableId = logData.PrviousRecord.id;
            // logData.Alt_Table_Id: set by out side.

            logData.Value_Old = LoadValue(logData.PrviousRecord);
            if (string.IsNullOrEmpty(logData.Value_Old))
            {
                return false;
            }

            logData.Value_New = LoadValue(logData.PostRecord);
            if (string.IsNullOrEmpty(logData.Value_New))
            {
                return false;
            }

            return true;
        }

        private static string LoadValue(BreakTimeRecord record)
        {
            var template = @"
Start Time: {ST}
Morning Break Start: {MBS}
Morning Break Duration (mins): {MBD}
Lunch Break Start: {LBS}
Lunch Break Duration (mins): {LBD}
Afternoon Break Start: {ABS}
Afternoon Break Duration (mins): {ABD}";

            template = template.Replace("{ST}", record.start_time.ToString().Substring(0, 5));
            template = template.Replace("{MBS}", record.morning_break_start.ToString().Substring(0, 5));
            template = template.Replace("{MBD}", record.morning_break_duration.ToString());
            template = template.Replace("{LBS}", record.lunch_break_start.ToString().Substring(0, 5));
            template = template.Replace("{LBD}", record.lunch_break_duration.ToString());
            template = template.Replace("{ABS}", record.afternoon_break_start.ToString().Substring(0, 5));
            template = template.Replace("{ABD}", record.afternoon_break_duration.ToString());

            if (!string.IsNullOrEmpty(template) && template.Length > 255)
            {
                template = template.Substring(0, 255);
            }

            return template;
        }
    }
}

/*
Start Time: {ST} \r\n
Morning Break Start: {MBS} \r\n
Morning Break Duration (mins): {MBD} \r\n
Lunch Break Start: {LBS} \r\n
Lunch Break Duration (mins): {LBD} \r\n
Afternoon Break Start: {ABS} \r\n
Afternoon Break Duration (mins): {ABD} \r\n
 *
 */
