using nesi.core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace NESI.BLL.Pages.Shared.Reports
	{
	public class DailyProjectSummary
    {

       static decimal SubTotal_Labour = 0;
        static decimal SubTotal_Miscellaneous = 0;
        static decimal SubTotal_Rentals = 0;
        static decimal SubTotal_Mileage = 0;
        public void DailyProjectReport()
        {

          

        }

        public static string GetBaseFilePath(int business_unit)
        {
            var baseFolder = Path.Combine(Toolbox.GetRequiredAppSetting("UNC_base_path"), "nesi_files", "TE");
            var teId = NeTaxEntity.TaxEntityFromUnit(business_unit);
            var teBaseFolder = $@"{baseFolder}\TE{teId}";

            return teBaseFolder;
        }
        public static string GetSignoffFilePath(int business_unit, int WOID)
        {
            NeFiles ne = new NeFiles();
            var teBaseFolder = ne.GetProjectFolder(WOID, "workorder", "", business_unit);
            return teBaseFolder;
        }

        private static double GetTheSumOfMileageRecord(int WOID, DateTime SelectedDate)
        {
            NeWOProg WO = new NeWOProg(WOID);
            var dt_manpower = new DataTable();

            if (WOID == 0 || SelectedDate == null)
            {
                return 0;
            }

            //
            // Query from Matt, just add COALESCE to hanlde Null case.
            //
            var query = @"
SELECT COALESCE(SUM(a.mileage_value*a.MemberTime_SellPrice), 0) 
FROM membertime a 
WHERE a.membertime_woprog_id = @v0 AND a.date = @v1 AND a.MemberTime_PayTypeHours_ID = 8";

            var sum = Toolbox.doSQL_double(query, new object[] { WOID, SelectedDate });
            return sum;
        }

        private static DataTable GetLabour(int WOID, DateTime SelectedDate)
        {
            NeWOProg WO = new NeWOProg(WOID);
            var dt_manpower = new DataTable();

            if (Toolbox.ReturnZeroIfNull_int(WOID) > 0)
            {
                /*
                dt_manpower = Toolbox.doSQL_dt(@"SELECT 
                    DATE_FORMAT (a.date,'%d %b %y') DATE,
                    m.member_fullname Employee,
                    c.membertype_name WorkType,
                    IF(a.MemberTime_PayTypeHours_ID = 1, SUM(a.NumberOfHours), 0) RegularHours,
                    IF(a.MemberTime_PayTypeHours_ID = 1, COALESCE(b.chargeout, mc.chargeout), 0) rt_rate,
                    IF(a.MemberTime_PayTypeHours_ID = 2, SUM(a.NumberOfHours), 0) OvertimeHours,
                    IF(a.MemberTime_PayTypeHours_ID = 2,COALESCE(b.chargeout, mc.chargeout), 0) ot_rate,
                    IF(a.MemberTime_PayTypeHours_ID = 3, SUM(a.NumberOfHours), 0) TravelHours,
                    IF(a.MemberTime_PayTypeHours_ID = 3, COALESCE(b.chargeout, mc.chargeout), 0) tt_rate,
                    (IF(a.MemberTime_PayTypeHours_ID = 1, SUM(a.NumberOfHours), 0) * (IF(a.MemberTime_PayTypeHours_ID = 1, COALESCE(b.chargeout, mc.chargeout), 0) ) +IF(a.MemberTime_PayTypeHours_ID = 2, SUM(a.NumberOfHours), 0) *(IF(a.MemberTime_PayTypeHours_ID = 2, COALESCE(b.chargeout, mc.chargeout), 0))
                    + IF(a.MemberTime_PayTypeHours_ID = 3, SUM(a.NumberOfHours), 0) * (IF(a.MemberTime_PayTypeHours_ID = 3, COALESCE(b.chargeout, mc.chargeout), 0))) Total,
                    a.MemberTime_WOProg_id,
                    IFNULL(d.comments,'') Remarks
                    FROM
                        membertime a
                   INNER JOIN
                        member m ON a.membertime_memberid = m.member_id
                   INNER JOIN
                        membertype c ON a.membertype_Id = c.membertype_Id
                   INNER JOIN membertype_chargeout  mc 
                            ON a.membertype_Id=mc.membertype_Id AND a.business_unit_id=mc.business_unit_id AND a.MemberTime_PayTypeHours_ID=mc.paytype_id
                   LEFT JOIN customer_rate b  ON mc.id = b.base_chargeout_id  AND  b.to_date >= NOW() AND b.customer_id=@v1 
                   LEFT JOIN    wocomment d ON a.MemberTime_WoComment_ID = d.WoComment_ID
                   WHERE
                       membertime_woprog_id = @v0 and
                       a.date=@v2
                      GROUP BY a.date, a.membertime_memberid
                      ORDER BY a.date, a.membertime_memberid
                      ", new object[] { WOID, WO.WOProg_Customer_ID, SelectedDate });
                */

                return GetLabour_NoSumInQueryVersion(WOID, SelectedDate, WO.WOProg_Customer_ID);
            }

            return dt_manpower;
        }

        private static DataTable GetLabour_NoSumInQueryVersion(int WOID, DateTime SelectedDate, int customerId)
        {
            var dt_manpower = new DataTable();

            //
            // The query to get all the data which based on the query from above previous one but with big different.
            //
            var query = @"
SELECT
  DATE_FORMAT(a.date, '%d %b %y') DATE,
  m.member_fullname Employee,
  c.membertype_name WorkType,
  a.MemberTime_PayTypeHours_ID hourType,
  a.NumberOfHours,
  COALESCE(b.chargeout, mc.chargeout) rate,
  a.MemberTime_WOProg_id woid,
  IFNULL(d.comments, '') Remarks
FROM
  membertime a
  INNER JOIN member m
    ON a.membertime_memberid = m.member_id
  INNER JOIN membertype c
    ON a.membertype_Id = c.membertype_Id
  INNER JOIN membertype_chargeout mc
    ON a.membertype_Id = mc.membertype_Id
    AND a.business_unit_id = mc.business_unit_id
    AND a.MemberTime_PayTypeHours_ID = mc.paytype_id
  LEFT JOIN customer_rate b
    ON mc.id = b.base_chargeout_id
    AND b.to_date >= NOW()
    AND b.customer_id = @v1
  LEFT JOIN wocomment d
    ON a.MemberTime_WoComment_ID = d.WoComment_ID
WHERE membertime_woprog_id = @v0
  AND a.date = @v2 and a.MemberTime_PayTypeHours_ID in (1, 2, 7)
  ORDER BY c.membertype_id, a.MemberTime_PayTypeHours_ID
";

            //
            // Query to get all records for regular time, over time and travel time.
            //
            var table_info = Toolbox.doSQL_dt(query, new object[] { WOID, customerId, SelectedDate });
            if (table_info == null || table_info.Rows == null || table_info.Rows.Count == 0)
            {
                // Return nothing if we get nothing.
                return dt_manpower;
            }

            //
            // Put in a list to do the groupingby stuff.
            //
            List<ManPowerRecord> list = new List<ManPowerRecord> { };
            foreach (DataRow row in table_info.Rows)
            {
                var r = new ManPowerRecord();
                r.date = Convert.ToDateTime(row["DATE"]);
                r.employee = row["Employee"].ToString();
                r.workType = row["WorkType"].ToString();
                r.hourtype = Convert.ToInt32(row["hourType"]);
                r.hours = Convert.ToDouble(row["NumberOfHours"]);
                r.rate = Convert.ToDouble(row["rate"]);
                r.woid = Convert.ToInt32(row["woid"]);
                r.remarks = row["remarks"].ToString();
                list.Add(r);
            }

            //
            // Group by based on four keys.
            //
            var goupList = from record in list
                       group record by new { record.date, record.employee, record.workType, record.woid}  into groupInfo
                       select new ManPowerGroupRecord
                       {
                           date = groupInfo.Key.date,
                           employee = groupInfo.Key.employee,
                           workType = groupInfo.Key.workType,
                           woid = groupInfo.Key.woid,
                           regularHours = groupInfo.Where(x=> x.hourtype == 1).Sum(x=> x.hours),
                           overTimeHours = groupInfo.Where(x => x.hourtype == 2).Sum(x => x.hours),
                           travelTimeHours = groupInfo.Where(x => x.hourtype == 7).Sum(x => x.hours),
                       };
            var groupListDate = goupList.ToList();

            //
            // NOW we will find OUT the rate AND remarks (comments).
            //
            foreach (var groupedRecord in groupListDate)
            {
                //
                // Get ALL records related TO EACH job type.
                //
                var relatedList = list.Where(x => x.workType == groupedRecord.workType && x.employee == groupedRecord.employee).ToList();

                //
                // Hanlde the remarks: put ALL UNIQUE comments together.
                //
                var remarks = "";
                var addedlist = new List<string>{ };
                foreach (var r in relatedList)
                {
                    if(!string.IsNullOrWhiteSpace(r.remarks))
                    {
                        if(string.IsNullOrWhiteSpace(remarks))
                        {
                            remarks = r.remarks;
                            addedlist.Add(r.remarks);
                        }
                        else
                        {
                            if(!addedlist.Contains(r.remarks))
                            {
                                remarks += "<br/>" + r.remarks;
                                addedlist.Add(r.remarks);
                            }
                        }
                    }
                }

                groupedRecord.remarks = remarks;

                //
                // Handle total: HOUR * rate
                //
                var total = 0.0;
                var lastemp = "";
                foreach (var r in relatedList)
                {
                    // IF (lastemp == "" || lastemp == r.employee)
                    //  {
                    total += r.hours * r.rate;
                    //  }
                    // lastemp = r.employee;

                }

                groupedRecord.total = total;

                //
                // Rates - NOT averge but here easy TO DO it IN the future.
                //
                var regularOne = relatedList.Where(x => x.hourtype == 1).FirstOrDefault();
                if(regularOne != null)
                {
                    groupedRecord.regularRates = regularOne.rate;
                }

                var overTimeOne = relatedList.Where(x => x.hourtype == 2).FirstOrDefault();
                if(overTimeOne != null)
                {
                    groupedRecord.overTimeRates = overTimeOne.rate;
                }

                var travelOne = relatedList.Where(x => x.hourtype == 7).FirstOrDefault();
                if(travelOne != null)
                {
                    groupedRecord.travelTimeRates = travelOne.rate;
                }
            }

            //
            // RETURN DATA based ON page render algorithm.
            //
            dt_manpower.Columns.Add("DATE", typeof(string));
            dt_manpower.Columns.Add("Employee", typeof(string));
            dt_manpower.Columns.Add("WorkType", typeof(string));
            dt_manpower.Columns.Add("RegularHours", typeof(double));
            dt_manpower.Columns.Add("rt_rate", typeof(double));
            dt_manpower.Columns.Add("OvertimeHours", typeof(double));
            dt_manpower.Columns.Add("ot_rate", typeof(double));
            dt_manpower.Columns.Add("TravelHours", typeof(double));
            dt_manpower.Columns.Add("tt_rate", typeof(double));
            dt_manpower.Columns.Add("Total", typeof(double));
            dt_manpower.Columns.Add("MemberTime_WOProg_id", typeof(int));
            dt_manpower.Columns.Add("Remarks", typeof(string));

            foreach (var grouppedRecord in groupListDate)
            {
                var dr = dt_manpower.NewRow();
                dr["DATE"] = grouppedRecord.date.ToString("d MMM yyyy");
                dr["Employee"] = grouppedRecord.employee;
                dr["WorkType"] = grouppedRecord.workType;
                dr["RegularHours"] = grouppedRecord.regularHours;
                dr["rt_rate"] = grouppedRecord.regularRates;
                dr["OvertimeHours"] = grouppedRecord.overTimeHours;
                dr["ot_rate"] = grouppedRecord.overTimeRates;
                dr["TravelHours"] = grouppedRecord.travelTimeHours;
                dr["tt_rate"] = grouppedRecord.travelTimeRates;
                dr["Total"] = grouppedRecord.total;
                dr["MemberTime_WOProg_id"] = grouppedRecord.woid;
                dr["Remarks"] = grouppedRecord.remarks;
                dt_manpower.Rows.Add(dr);
            }

            return dt_manpower;
        }

        public static DataTable GetMISCELLANEOUS(int WOID, DateTime SelectedDate)
        {
            var dt_miscellaneous = new DataTable();

            if (Toolbox.ReturnZeroIfNull_int(WOID) > 0)
            {
                dt_miscellaneous = Toolbox.doSQL_dt(@"CALL DS_PROJ_SUMMARY_MISC(@v0, @v1)", new object[] { WOID, SelectedDate });
            }

            return dt_miscellaneous;
        }



        public static DataTable GetRentals(int WOID, DateTime SelectedDate)
        {
            var dt_rentals = new DataTable();

            if (Toolbox.ReturnZeroIfNull_int(WOID) > 0)
            {
                 dt_rentals = Toolbox.doSQL_dt(@"CALL DS_PROJ_SUMMARY_RENTAL(@v0, @v1)", new object[] { WOID, SelectedDate });
            }

            return dt_rentals;
        }

        public static string GetDailyProjectHeader(int WOID,string currentuesr_Fullname)
        {
            var WO = new NeWOProg(WOID);
            var Contact = new NEContact(WO.woprog_Contact_ID);
            var Bu = new NeBusinessUnit(WO.business_unit_id);
            
           // var teBaseFolder = GetBaseFilePath(WO.business_unit_id);
           

             var DailyProjectHeader = $@"<br><br><h2 style='text-align:center;font-family:Arial;font-weight:normal; font-size: 20;'>DAILY PROJECT SUMMARY</h2>
     <table style='width:100%; height:auto; border: 1px solid #bebebe;border-collapse: collapse; '>
     <tr>
         <td rowspan='4' align='center' width='25%'><img alt src='{Common.Shared.Configuration.HostName}\assets\images\Logos\{Toolbox.ReturnBlankIfNull_string(Bu.logo_file)}' style='width:55%;height:auto;margin-left:8px;'/></td>
     </tr>
     <tr>
        <td width='50%' style='border: 1px solid #bebebe;font-family:Arial;'><font size='0.8'>PROJECT TITLE</font><br><font size='0.4'>{Toolbox.ReturnBlankIfNull_string(WO.Description)}</font></td>
        <td style='border: 1px solid #bebebe;font-family:Arial;'><font size='0.8'>PROJECT NO.</font><br><font size='0.4'>{WO.woprog_id}</font></td>
     </tr>
     <tr>
       <td width='50%' style='border: 1px solid #bebebe;font-family:Arial;'><font size='0.8'>CLIENT</font><br><font size='0.4'>{Toolbox.ReturnBlankIfNull_string(WO.CustomerName)}</font></td>
       <td style='border: 1px solid #bebebe;font-family:Arial;'><font size='0.8'>DATE</font><br><font size='0.4'>{DateTime.Now.ToString("d MMM yyyy")}</font></td>
     </tr>
     <tr>
       <td width='50%' style='border: 1px solid #bebebe;font-family:Arial;'><font size='0.8'>CLIENT CONTACT </font><br><font size='0.4'>{Toolbox.ReturnBlankIfNull_string(Contact.Contact_Name)}</font></td>
       <td style='border: 1px solid #bebebe;font-family:Arial;'><font size='0.8'>AUTHOR </font><br><font size='0.4'>{Toolbox.ReturnBlankIfNull_string(currentuesr_Fullname)}</font></td>
     </tr>
</table>";

            return DailyProjectHeader;

        }

        public static string GetLabourTable(int WOID, DateTime SelectedDate)
        {
            //
            // Trigger get the mileage in here.
            //
            SubTotal_Mileage = (decimal)GetTheSumOfMileageRecord(WOID, SelectedDate);

            var Labour_html = @"<table style='width:100%; height:auto; border: 1px solid #bebebe;border-collapse: collapse;'>
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Date</th>
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Employee</th>
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Work Type</th> 
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Regular Hours</th>
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Rate</th>
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Overtime Hours</th>
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Rate</th>
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Travel Hours</th>
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Rate</th>
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Total</th>
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Work Order#</th>
<th style='border: 1px solid #bebebe;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Remarks</th>
" + GetLabourGrid(GetLabour(WOID, SelectedDate)) + @"
</table>";

            return Labour_html;

        }


        public static string GetMiscellaneousTable(int WOID, DateTime SelectedDate)
        {
            var Miscellaneous_html = @"<table style='width:100%; height:auto; border: 1px solid #bebebe;border-collapse: collapse;'>
<th style='border: 1px solid #3E3E42;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Description</th>
<th style='border: 1px solid #3E3E42;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Work Order#</th> 
<th style='border: 1px solid #3E3E42;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>QTY</th>
<th style='border: 1px solid #3E3E42;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Price</th>
<th style='border: 1px solid #3E3E42;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#ccc;'>Total</th>
" + GetMiscellaneousGrid(GetMISCELLANEOUS(WOID, SelectedDate)) + @"
</table>";
            return Miscellaneous_html;
        }

        public static string GeRentalsTable(int WOID, DateTime SelectedDate)
        {
            var Rental_html = @"<table style='width:100%; height:auto; border: 1px solid #3E3E42;border-collapse: collapse;'>
<th style='border: 1px solid #3E3E42;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#cccccc;'>Equipment</th>
<th style='border: 1px solid #3E3E42;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#cccccc;'>Unit #</th> 
<th style='border: 1px solid #3E3E42;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#cccccc;'>Work Order#</th> 
<th style='border: 1px solid #3E3E42;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#cccccc;'>QTY</th>
<th style='border: 1px solid #3E3E42;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#cccccc;'>Price</th>
<th style='border: 1px solid #3E3E42;  text-align: left;  font-size: 11; font-weight:normal;font-family:Arial;background-color:#cccccc;'>Total</th>
" + GetRentalsGrid(GetRentals(WOID, SelectedDate)) + @"
</table>";
            return Rental_html;
        }


        public static string GetApprovalsTable(string img,string ProjectManager,string SubmitBy,string ApprovalBy)
        {
            var imghtml = "";
            if (img != "")
            {
				imghtml = "<img alt src='" + img + @"' style='width:386px;height:150px;margin-left:10px;'/>";
            }
                var Approvals_html = $@"<table style='width:100%; height:auto; border: 1px solid #bebebe;border-collapse: collapse;'>
  <tr>
    <td style='border: 1px solid #bebebe;border-right:0px;text-align: left;vertical-align:text-center; font-size: 11; font-weight:normal;font-family:Arial;background-color:#cccccc;'>Category</td>
    <td style='border: 1px solid #bebebe;border-left:0px;text-align: right;vertical-align:text-center; font-size: 11; font-weight:normal;font-family:Arial;background-color:#cccccc;'>Subtotal</td>
     <td rowspan='7' style='width:50%;border: 1px solid #bebebe;vertical-align:text-top; font-size:8; font-weight:normal; font-family:Arial;'>
  CUSTOMER SIGNATURE/AFE STAMP <br>{imghtml}
  </td>
  </tr>
  
</tr>
  <tr>
    <td style='border: 1px solid #bebebe;border-right:0px;border-bottom:0px;text-align: left;vertical-align:text-center; font-size: 11; font-weight:normal;font-family:Arial;'>Labour Charges</td>
    <td style='border: 1px solid #bebebe;border-left:0px;border-bottom:0px;text-align: right;vertical-align:text-center; font-size: 11; font-weight:normal;font-family:Arial;'>${string.Format("{0:N2}", SubTotal_Labour)}</td>
  </tr>
   <tr>
    <td style='border: 1px solid #bebebe;border-right:0px;border-top:0px;border-bottom:0px;text-align: left;vertical-align:text-center; font-size: 11; font-weight:normal;font-family:Arial;'>Equipment Charges</td>
    <td style='border: 1px solid #bebebe;border-left:0px;border-top:0px;border-bottom:0px;text-align: right;vertical-align:text-center; font-size: 11; font-weight:normal;font-family:Arial;'>${string.Format("{0:N2}", SubTotal_Rentals)}</td>
  </tr>
  <tr>
    <td style='border: 1px solid #bebebe;border-right:0px;border-top:0px;text-align: left;vertical-align:text-center; font-size: 11; font-weight:normal;font-family:Arial;'>Material & Unit Charges</td>
    <td style='border: 1px solid #bebebe;border-left:0px;border-top:0px;text-align: right;vertical-align:text-center; font-size: 11; font-weight:normal;font-family:Arial;'>${string.Format("{0:N2}", SubTotal_Miscellaneous )}</td>
  </tr>
   <tr>
    <td style='Height:70px;border: 1px solid #bebebe;border-right:0px;text-align: left;vertical-align:text-center; font-size: 16; font-weight:bold;font-family:Arial;'>Ticket Total</td>
    <td style='Height:70px;border: 1px solid #bebebe;border-left:0px;text-align: right;vertical-align:text-center; font-size: 16; font-weight:bold;font-family:Arial;'>${string.Format("{0:N2}", SubTotal_Labour + SubTotal_Miscellaneous + SubTotal_Rentals )}</td>
  </tr>
   <tr>
    <td style='width:25%;border: 1px solid #bebebe;border-bottom:0px;text-align: left;vertical-align:text-top;font-weight:normal;font-amily:Arial;font-size: 8;'>PROJECT MANAGER</td>
    <td  style='width:25%;border: 1px solid #bebebe;border-bottom:0px;text-align: left;vertical-align:text-top;font-weight:normal;font-amily:Arial;font-size: 8;'>SUBMITTED BY</td>
  </tr>
   <tr>
  	   <td style='border: 1px solid #bebebe;border-top:0px;text-align: left;vertical-align:text-center; font-size: 11; font-weight:normal;font-family:Arial;'>{ProjectManager}</td>
       <td style='border: 1px solid #bebebe;border-top:0px;text-align: left;vertical-align:text-center; font-size: 11; font-weight:normal;font-family:Arial;'>{SubmitBy}</td>
   </tr>
    <tr>
     <td style='width:25%;border: 1px solid #bebebe;border-bottom:0px;;text-align: left;vertical-align:text-top;font-weight:normal;font-amily:Arial;font-size: 8;'>APPROVAL DATE</td>
     <td  style='width:25%;border: 1px solid #bebebe;border-bottom:0px;text-align: left;vertical-align:text-top;font-weight:normal;font-amily:Arial;font-size: 8;'>APPROVED BY</td>
     <td rowspan='2' style='border: 1px solid #bebebe;text-align: left;vertical-align:text-center;font-weight:normal;font-amily:Arial;font-size: 11;'>CLIENT W/O NO.:</td>
  </tr>
   <tr>
  	      <td style='border: 1px solid #bebebe;border-top:0px;text-align: left;vertical-align:text-top;font-weight:normal;font-amily:Arial;font-size: 11;'>{DateTime.Now.ToString("d MMM yyyy")}</td>
          <td style='border: 1px solid #bebebe;border-top:0px;text-align: left;vertical-align:text-top;font-weight:normal;font-amily:Arial;font-size: 11;'>{ApprovalBy}</td>
   </tr>
  
</table>";

            return Approvals_html;

        }


        public static string GetGeneralDescriptionofWork(int WOID)
        {
            var GeneralDescriptionofWork_html = @"";
            var WO = new NeWOProg(WOID);

            GeneralDescriptionofWork_html = GeneralDescriptionofWork_html + @"<table style='width:100%; height:auto; border: 1px solid #bebebe; border-collapse: collapse;'>
<tr>
   <td style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>" + Toolbox.ReturnBlankIfNull_string(WO.Description) + @"</td>
</tr>
</table>";

            return GeneralDescriptionofWork_html;


        }

        public static string GetLabourGrid(DataTable dt_manpower)
        {
            var manpower_html = @"";
            decimal rh = 0;
            decimal oh = 0;
            decimal th =0;
            decimal total = 0;
            SubTotal_Labour = 0;

            foreach (DataRow dr in dt_manpower.Rows)
            {
                manpower_html += @"<tr>";
                for (int i = 0; i < dt_manpower.Columns.Count; i++)
                {
                  
                    if (dt_manpower.Columns[i].Caption == "RegularHours")
                    {
                        rh += Convert.ToDecimal(dr[i]);
                        manpower_html = manpower_html + @"<td style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>" + string.Format("{0:N2}", dr[i]) + @"</td>";
                    }
                    else if (dt_manpower.Columns[i].Caption == "OvertimeHours")
                    {
                        oh += Convert.ToDecimal(dr[i]);
                        manpower_html = manpower_html + @"<td style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>" + string.Format("{0:N2}", dr[i]) + @"</td>";
                    }
                    else if (dt_manpower.Columns[i].Caption == "TravelHours")
                    {
                        th += Convert.ToDecimal(dr[i]);
                        manpower_html = manpower_html + @"<td style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>" + string.Format("{0:N2}", dr[i]) + @"</td>";
                    }
                    else if (dt_manpower.Columns[i].Caption.Contains("rate"))
                    {
                        manpower_html = manpower_html + @"<td style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>$" + string.Format("{0:N2}", dr[i]) + @"</td>";
                    }
                    else if (dt_manpower.Columns[i].Caption == "Total")
                    {
                        total += Convert.ToDecimal(dr[i]);
                        manpower_html = manpower_html + @"<td style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>$" + string.Format("{0:N2}", dr[i]) + @"</td>";
                    }
                    else if (dt_manpower.Columns[i].Caption.Contains("Remarks"))
                    {
                        manpower_html = manpower_html + @"<td style='border: 1px solid #bebebe; text-align: left;'><pre style=' font-size: 10; font-weight:normal; font-family:Arial;'>" +  dr[i] + @"</pre></td>";
                    }
                    else
                    {
                        manpower_html = manpower_html + @"<td style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>" +dr[i] + @"</td>";
                    }

                }

                SubTotal_Labour = total;
                manpower_html = manpower_html + @"</tr>";

            }

            if (manpower_html.Contains("<tr>"))
            {
                manpower_html = manpower_html + @"<tr><td colspan='3' style='text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>Labour Total</td><td  style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>" + string.Format("{0:N2}",rh) + "</td><td></td><td  style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>" + string.Format("{0:N2}",oh) + "</td><td></td><td  style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>" + string.Format("{0:N2}", th) + "</td><td></td><td style='border: 1px solid #bebebe;text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>$" + string.Format("{0:N2}", total) +"</td><td  style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'></td><td  style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'> </td></tr>";
            }


            return manpower_html;
        }

        public static string GetMiscellaneousGrid(DataTable dt_miscellaneous)
        {
            var Miscellaneous_html = @"";
            decimal total = 0;
            SubTotal_Miscellaneous = 0;

            foreach (DataRow dr in dt_miscellaneous.Rows)
            {
                Miscellaneous_html = Miscellaneous_html + @"<tr>";
                for (int i = 0; i < dt_miscellaneous.Columns.Count; i++)
                {
                    if (dt_miscellaneous.Columns[i].Caption == "total")
                    {
                        total += Convert.ToDecimal(dr[i]);

                        Miscellaneous_html = Miscellaneous_html + @"<td style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>$" + string.Format("{0:N2}", dr[i]) + @"</td>";
                    }
                    else
                    {
                        Miscellaneous_html = Miscellaneous_html + @"<td style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>" + dr[i] + @"</td>";
                    }
                }

				Miscellaneous_html += @"</tr>";

            }

            SubTotal_Miscellaneous = total;

            if (Miscellaneous_html.Contains("<tr>"))
            {
                Miscellaneous_html = Miscellaneous_html + @"<tr><td colspan='4' style='text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>Material & Unit Charges Total</td><td  style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>$" + string.Format("{0:N2}",total) + "</td></tr>";
            }


            return Miscellaneous_html;
        }


        public static string GetRentalsGrid(DataTable dt_rental)
        {
            var Rentals_html = @"";
            decimal total = 0;
            SubTotal_Rentals = 0;

            foreach (DataRow dr in dt_rental.Rows)
            {
                Rentals_html = Rentals_html + @"<tr>";
                for (int i = 0; i < dt_rental.Columns.Count; i++)
                {
                    if (dt_rental.Columns[i].Caption == "total")
                    {
                        total = total + Convert.ToDecimal(dr[i]);
                        Rentals_html = Rentals_html + @"<td style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>$" + string.Format("{0:N2}", Convert.ToDecimal(dr[i])) + @"</td>";
                    }
                    else
                    {
                        Rentals_html = Rentals_html + @"<td style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>" + dr[i] + @"</td>";
                    }
                }

                Rentals_html = Rentals_html + @"</tr>";

            }
            SubTotal_Rentals = total;

            if (Rentals_html.Contains("<tr>"))
            {
                Rentals_html = Rentals_html + @"<tr><td colspan='5' style='text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>Equipment Charges Total</td><td  style='border: 1px solid #bebebe; text-align: left; font-size: 10; font-weight:normal; font-family:Arial;'>$" + string.Format("{0:N2}", total) + "</td></tr>";
            }


            return Rentals_html;
        }


        public static Byte[] HtmlToPdf()
        {
            var Renderer = new IronPdf.HtmlToPdf();
            var PDF = Renderer.RenderHtmlAsPdf("<p>Any TEXT</p>");
            return PDF.Stream.ToArray();
        }


    }

    #region Signoff report stuff
    public class ManPowerRecord
    {
        public DateTime date { get; set; }
        public string employee { get; set; }
        public string workType { get; set; }
        public int hourtype { get; set; }
        public double hours { get; set; }
        public double rate { get; set; }
        public int woid { get; set; }
        public string remarks { get; set; }
    }

    public class ManPowerGroupRecord
    {
        public DateTime date { get; set; }
        public string employee { get; set; }
        public string workType { get; set; }
        public int woid { get; set; }
        public double regularHours { get; set; }
        public double regularRates { get; set; }
        public double overTimeHours { get; set; }
        public double overTimeRates { get; set; }
        public double travelTimeHours { get; set; }
        public double travelTimeRates { get; set; }
        public double total { get; set; }
        public string remarks { get; set; }
    }
    #endregion
}
