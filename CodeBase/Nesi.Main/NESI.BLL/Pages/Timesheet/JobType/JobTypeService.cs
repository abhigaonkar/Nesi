using nesi.core;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Timesheet.JobType
{
    public class JobTypeService : IJobTypeService
    {
        public JobTypeInfo GetJobTypeInfo(JobTypeRecordQueryParameter jobTypeRecordQueryParameter)
        {
            // The client must pass business and member ID info.
            JobTypeInfo info = new JobTypeInfo(jobTypeRecordQueryParameter) { };
            if (jobTypeRecordQueryParameter == null || 
                jobTypeRecordQueryParameter.business_uint_id <= 0 ||
                jobTypeRecordQueryParameter.member_id <= 0 )
            {
                return info;
            }

            BusinessJobTypeConfigQueryParameter p1 = new BusinessJobTypeConfigQueryParameter {  business_unit_id = jobTypeRecordQueryParameter.business_uint_id };
            info.businessJobTypeConfig = this.GetBusinessJobTypeConfig(p1);
            if (info.businessJobTypeConfig.okay == false)
            {
                // Now we can't get the business levle config, so we will not show the job types.
                return info;
            }

            //
            // Now we successfully got the business levle config, so we can go ahead. 
            //

            info.showJobType = info.businessJobTypeConfig.okay && info.businessJobTypeConfig.allow_jobtype_selection;

            info.jobTypeRecordQueryParameter.allow_jobtype_selection = info.businessJobTypeConfig.allow_jobtype_selection;
           // info.jobTypes = this.GetAvailableJobTypesForGivenEmployee(info.jobTypeRecordQueryParameter);
            info.jobTypes = this.GetJobTypeListBasedOnCustomer(info.jobTypeRecordQueryParameter.customer_id,info.jobTypeRecordQueryParameter.business_uint_id,info.jobTypeRecordQueryParameter.member_id);
            info.defaultValue = info.jobTypeRecordQueryParameter.defaultValue;
            return info;
        }

        #region don't call directly - may be removed.
        public List<JobTypeRecord> GetAvailableJobTypesForGivenEmployee(JobTypeRecordQueryParameter jobTypeQueryParameter)
        {
            var list = this._GetAvailableJobTypesForGivenEmployee(jobTypeQueryParameter);
            return list;
        }

        public BusinessJobTypeConfig GetBusinessJobTypeConfig(BusinessJobTypeConfigQueryParameter businessJobTypeConfigQueryParameter)
        {
            var config = this._GetBusinessJobTypeConfig(businessJobTypeConfigQueryParameter);
            return config;
        }
        #endregion

        #region private
        private BusinessJobTypeConfig _GetBusinessJobTypeConfig(BusinessJobTypeConfigQueryParameter businessJobTypeConfigQueryParameter)
        {
            //
            // Any failed to get the config will cause not to show the job type list.
            //
            BusinessJobTypeConfig config = new BusinessJobTypeConfig {  business_unit_id = 0, allow_jobtype_selection = false, okay = false};

            if (businessJobTypeConfigQueryParameter == null)
            {
                return config;
            }

            if (businessJobTypeConfigQueryParameter.business_unit_id <= 0)
            {
                return config;
            }

            config.business_unit_id = businessJobTypeConfigQueryParameter.business_unit_id;

            var sql = "SELECT allow_jobtype_selection FROM business_unit WHERE id = @v0";
            var dt = Toolbox.doSQL_dt(sql, new object[] { config.business_unit_id });
            if (dt == null || dt.Rows == null || dt.Rows.Count != 1)
            {
                return config;
            }

            var dr = dt.Rows[0];

            try
            {
                // This happens when script not run to create above column.
                config.allow_jobtype_selection = Convert.ToBoolean(dr["allow_jobtype_selection"]);
            }
            catch (Exception ex)
            {
                return config;
            }

            config.okay = true;
            return config;
        }

        private List<JobTypeRecord> _GetAvailableJobTypesForGivenEmployee(JobTypeRecordQueryParameter jobTypeQueryParameter)
        {
            var list = this._GetAvailableJobTypesForGivenEmployee_Lateral_Downward_Constrain(jobTypeQueryParameter);
            return list;
        }

        private List<JobTypeRecord> _GetAvailableJobTypesForGivenEmployee_Lateral_Downward_Constrain(JobTypeRecordQueryParameter jobTypeQueryParameter)
        {
            List<JobTypeRecord> list = new List<JobTypeRecord>() { };

            //
            // First to have itself in the list
            //
            var queryItself = @"
SELECT membertype_id, membertype_name,  membertype.reports_to 
FROM member 
INNER JOIN membertype ON member.member_membertype_id = membertype.membertype_id
WHERE member.Member_ID = @v0
";

            var dtItself = Toolbox.doSQL_dt(queryItself, new object[] { jobTypeQueryParameter.member_id });
            if (dtItself == null || dtItself.Rows == null || dtItself.Rows.Count != 1)
            {
                // should never happen...
                return list;
            }

            var row = dtItself.Rows[0];
            var currentRecord = new JobTypeRecord();
            currentRecord.membertype_id = Convert.ToInt32(row["membertype_id"].ToString());
            currentRecord.membertype_name = row["membertype_name"].ToString();
            currentRecord.reports_to = Convert.ToInt32(row["reports_to"].ToString());
            list.Add(currentRecord);

            jobTypeQueryParameter.member_type_id = currentRecord.membertype_id;
            jobTypeQueryParameter.defaultValue.membertype_id = currentRecord.membertype_id;
            jobTypeQueryParameter.defaultValue.membertype_name = currentRecord.membertype_name;
            jobTypeQueryParameter.defaultValue.reports_to = currentRecord.reports_to;

            if (jobTypeQueryParameter.allow_jobtype_selection == false)
            {
                jobTypeQueryParameter.okayOnGetJobTypes = true;
                return list;
            }

            /*
             * Now added more based on requirement.
             * https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1540/
             *  Employees of a business unit with the above denoted "flag" are allowed to select a membertype at will,
             *  based on the membertype reports to hierarchy.

             *  Lateral & downward moves are fine, but an electrician cannot add time as a project manager.
             *  A project manager cannot add time as a branch manager.
             */

            //
            // After talking to Matt, using reports_to flat for now.
            //

            //var query = "SELECT * FROM membertype  WHERE active = 1 and reports_to = @v0";
            //var dt = Toolbox.doSQL_dt(query, new object[] { currentRecord.reports_to });
            //if (dt == null || dt.Rows == null || dt.Rows.Count <= 0)
            //{
            //    return list;
            //}

            //foreach (DataRow dr in dt.Rows)
            //{
            //    var record = new JobTypeRecord();
            //    record.membertype_id = Convert.ToInt32(dr["membertype_id"].ToString());
            //    record.membertype_name = dr["membertype_name"].ToString();
            //    record.reports_to = Convert.ToInt32(row["reports_to"].ToString());

            //    if (record.membertype_id != currentRecord.membertype_id)
            //    {
            //        list.Add(record);
            //    }
            //}

            // list = GetJobTypeListBasedOnReportTo(currentRecord.reports_to); 
            // get list based on Customer rates set
            if (jobTypeQueryParameter.customer_id > 0 && jobTypeQueryParameter.business_uint_id>0 && jobTypeQueryParameter.member_id>0)
            { 
                list = GetJobTypeListBasedOnCustomer(jobTypeQueryParameter.customer_id,jobTypeQueryParameter.business_uint_id,jobTypeQueryParameter.member_id);
            }
            //
            // End of ...
            //

            jobTypeQueryParameter.okayOnGetJobTypes = true;
            return list;
        }

        private List<JobTypeRecord> GetJobTypeListBasedOnReportTo(int reportTo)
        {
            //
            // Get all active member types.
            //
            List<JobTypeRecord> list = new List<JobTypeRecord>() { };
            List<JobTypeRecord> resultList = new List<JobTypeRecord>() { };
            var query = "SELECT membertype_id,  membertype_name, reports_to FROM membertype  WHERE active = 1";
            var dt = Toolbox.doSQL_dt(query, new object[] { });
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                return resultList;
            }

            foreach (DataRow dr in dt.Rows)
            {
                var record = new JobTypeRecord();
                record.membertype_id = Convert.ToInt32(dr["membertype_id"].ToString());
                record.membertype_name = dr["membertype_name"].ToString();
                record.reports_to = Convert.ToInt32(dr["reports_to"].ToString());
                list.Add(record);
            }

            //
            // Now we have all records: ~100 in total. we need to remove some of them.
            //

            //
            // (1) lateral structure (same report_to): This is the first batch to be added in the list.
            //
            var laterals = list.Where(item => item.reports_to == reportTo).ToList();
            if (laterals == null || laterals.Count() == 0)
            {
                return resultList;
            }
            resultList.AddRange(laterals);

            //
            // (2) downward 
            //
            int report_to_depth = 13;
            int current_depth = 0;
            List<int> includedList = laterals.Select(x => x.membertype_id).ToList();
            List<int> reportToList = laterals.Select(x => x.membertype_id).ToList();
            List <JobTypeRecord> downwards = new List<JobTypeRecord>() { };

            while (current_depth < report_to_depth)
            {
                var children = list.Where(item => (reportToList.Contains(item.reports_to)) && (!includedList.Contains(item.membertype_id)) ).ToList();
                if (children == null || children.Count() == 0)
                {
                    break;
                }

                // Added them in.
                downwards.AddRange(children);

                // Update the report to List so it will becoming the new parents.(parent)
                reportToList = children.Select(x => x.membertype_id).ToList();

                // added the new included items. 
                includedList.AddRange(children.Select(x => x.membertype_id).ToList());

                // avoid endless looping.
                current_depth++;
            }

            //
            // Remove the duplicated items
            //
            if (downwards.Count() == 0)
            {
                return resultList;
            }

            foreach (var item in downwards)
            {
                var found = false;

                // check the list...
                foreach (var record in resultList)
                {
                    if (record.membertype_id == item.membertype_id)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                  //  item.membertype_name = item.membertype_name + DateTime.Now.ToShortTimeString();

                    resultList.Add(item);
                }
            }

            return resultList;
        }
        private List<JobTypeRecord> GetJobTypeListBasedOnCustomer(int customer_id,int business_unit_id,int member_id)
        {
            List<JobTypeRecord> resultList = new List<JobTypeRecord>() { };
           ///add union here
           
            var sql = @" SELECT
    e.membertype_id,
    e.membertype_name,
    e.reports_to,
    GROUP_CONCAT(DISTINCT c.paytype_id),
    GROUP_CONCAT(DISTINCT (IF(c.paytype_id<7,c.paytype_id,NULL)) ORDER BY c.paytype_id) paytypeAllowed,
    (    SELECT 
            GROUP_CONCAT(DISTINCT paytype_id) 
        FROM 
            customer_rate aa
        INNER JOIN 
            membertype_chargeout cc ON aa.base_chargeout_id = cc.id 
        INNER JOIN 
            business_unit dd ON cc.business_unit_id=dd.ID
        INNER JOIN 
            membertype ee ON cc.membertype_id = ee.membertype_id
        WHERE 
            aa.customer_id = @v0 AND 
            dd.id = @v1 AND 
            aa.chargeout > 0 AND 
            cc.paytype_id IN (7,8) AND 
            CURDATE() BETWEEN aa.from_date AND aa.to_date
    ) AS extraPaytypes 
FROM
    customer_rate a 
INNER JOIN 
    membertype_chargeout c ON a.base_chargeout_id = c.id 
INNER JOIN 
    business_unit d ON c.business_unit_id=d.ID
INNER JOIN 
    membertype e ON c.membertype_id = e.membertype_id
WHERE 
    a.customer_id = @v0 
    AND d.id = @v1 
    AND a.chargeout > 0 
    AND c.paytype_id < 7 
    AND CURDATE() BETWEEN a.from_date AND a.to_date
GROUP BY 
    c.membertype_id

ORDER BY 
   membertype_name  ";
            var dt = Toolbox.doSQL_dt(sql, new object[] { customer_id,business_unit_id,member_id});
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                return resultList;
            }
          //  var list = dt.AsEnumerable().OrderBy(a => a.Field<int> ("member_id") == member_id ? 0 : 1).ThenBy(a => a.Field<string>("membertype_name"));
            foreach (DataRow dr in dt.Rows)
            {
                //extra payments will incule travel and mileage
                var record = new JobTypeRecord
                {
                    membertype_id = Convert.ToInt32(dr["membertype_id"]),
                    membertype_name = dr["membertype_name"].ToString(),
                    reports_to = Convert.ToInt32(dr["reports_to"]),
                    paytypeAllowed = dr["paytypeAllowed"].ToString(),
                    extraPaytypes = Toolbox.ReturnBlankIfNull_string(dr["extraPaytypes"]).Split(',')
                };
                 
                resultList.Add(record);
            }

            //
            // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/2049/
            //
            // resultList = GetJobTypeListBasedOnCustomer_CheckOnSelf(customer_id, business_unit_id, member_id, resultList);

            return resultList;
        }

        private List<JobTypeRecord> GetJobTypeListBasedOnCustomer_CheckOnSelf(int customer_id, int business_unit_id, int member_id, List<JobTypeRecord> list)
        {
            // 
            // The GetJobTypeListBasedOnCustomer limits the travel & mileage only available fro job-type-enabled branches.
            // Now this function will only follow that to fix the problem.
            //
            // Goal: Handle the case of only configure the trave/mileage but not others types for yourself.
            //


            //
            // Step 0: get my self info.
            //
            var queryItself = @"
SELECT membertype_id, membertype_name,  membertype.reports_to 
FROM member 
INNER JOIN membertype ON member.member_membertype_id = membertype.membertype_id
WHERE member.Member_ID = @v0
";

            var dtItself = Toolbox.doSQL_dt(queryItself, new object[] { member_id });
            if (dtItself == null || dtItself.Rows == null || dtItself.Rows.Count != 1)
            {
                // should never happen...
                return list;
            }

            var row = dtItself.Rows[0];
            var currentRecord = new JobTypeRecord();
            var myself_memberType_id = Convert.ToInt32(row["membertype_id"].ToString());

            //
            // Step 1: Query the tests on mileage / travel
            //
            var query = @"SELECT
  e.membertype_id,
  e.membertype_name,
  e.reports_to,
  c.paytype_id,
  a.chargeout
  
FROM
  customer_rate a
  INNER JOIN membertype_chargeout c
    ON a.base_chargeout_id = c.id
  INNER JOIN business_unit d
    ON c.business_unit_id = d.ID
  INNER JOIN membertype e
    ON c.membertype_id = e.membertype_id
WHERE a.customer_id = @v0
  AND d.id = @v1
  AND a.chargeout > 0
  AND e.membertype_id = @v2
  AND c.paytype_id IN (7, 8)
  AND CURDATE() BETWEEN a.from_date AND a.to_date";

            var dt = Toolbox.doSQL_dt(query, new object[] { customer_id, business_unit_id, myself_memberType_id });
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                // no more config
                return list;
            }

            List<JobTypeRecord> listForSelf = new List<JobTypeRecord> { };
            foreach (DataRow dr in dt.Rows)
            {
                //extra payments will incule travel and mileage
                var record = new JobTypeRecord
                {
                    membertype_id = Convert.ToInt32(dr["membertype_id"]),
                    membertype_name = dr["membertype_name"].ToString(),
                    reports_to = Convert.ToInt32(dr["reports_to"]),
                    paytypeAllowed = dr["paytype_id"].ToString() // Borrow this to save paytype_id
                };

                listForSelf.Add(record);
            }

            //
            // Step 2: Merge into one record.
            //
            JobTypeRecord selfRecord = new JobTypeRecord
            {
                membertype_id = listForSelf[0].membertype_id,
                membertype_name = listForSelf[0].membertype_name,
                reports_to = listForSelf[0].reports_to,
                paytypeAllowed = ""
            };

            var extraPaytypes = new List<string> { };
            foreach (var item in listForSelf)
            {
                extraPaytypes.Add(item.paytypeAllowed);
            }

            selfRecord.extraPaytypes = extraPaytypes.ToArray();

            //
            // Step3: Check whether it is inside the list, if not, add it in.
            //
            var found = false;
            foreach (var item in list)
            {
                if (item.membertype_id == selfRecord.membertype_id)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                list.Add(selfRecord);
            }

            //
            // myself_memberType_id
            //
            // Travel or mileage should be only available on myself member type?
            //
            foreach (var item in list)
            {
                if (item.membertype_id != myself_memberType_id)
                {
                    item.extraPaytypes = new string[] { };
                }
                else
                {
                    item.extraPaytypes = selfRecord.extraPaytypes; 
                }
            }

            return list;
        }
     #endregion
     }
}
