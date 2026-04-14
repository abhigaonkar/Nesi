using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using NESI.BLL.Base;

namespace NESI.BLL.Pages.AdminTools
{
    public class IntegErrorBase : BLLBase
    {
        public DataTable GetErrorMonths()
        {
            return bllToolbox.doSQL_dt(@"
 SELECT DATE_FORMAT(start_time,  '%Y-%m')  ym from integration_audit.audit_integration_entity GROUP BY ym
");
        }
        public DataTable GetIntegrationErrors(string duration)
        {
            duration += "-01";
            DataTable d = bllToolbox.doSQL_dt(@"
SELECT 
    a.id,
    b.batch_code,
    a.batch_id,
    a.app_code,
    a.entity_id,
    c.message_type,
    c.message,
    a.source_system,
    a.entity_type,
    a.current_status,
    a.current_retry_count,
    date(a.start_time) as startdate,time(a.start_time) as starttime,
    date(a.completion_time) as completiondate,time(a.completion_time) as completiontime,
    timediff(a.completion_time, a.start_time) Dur
FROM 
	integration_audit.audit_integration_entity a
LEFT JOIN 
	integration_audit.audit_message_log c ON a.id = c.entity_id
LEFT JOIN 
	integration_audit.audit_session_batch b ON a.batch_id  =  b.id
WHERE 
	a.start_time Between @v0 AND LAST_DAY(@v0) order by ID desc
", new object[] { duration });
            return d;
        }

    }
}
