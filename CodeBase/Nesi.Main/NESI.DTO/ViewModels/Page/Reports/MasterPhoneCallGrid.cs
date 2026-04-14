using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("MasterPhoneCallGrid")]
    public class MasterPhoneCallGrid : ModelBase<MasterPhoneCallGrid>
    {
        public string phone_log_date { get; set; }
        public string phone_log_from_number { get; set; }
        public string phone_log_to_number { get; set; }
        public string phone_log_from_name { get; set; }
        public string phone_log_to_name { get; set; }
        public long phone_log_duration { get; set; }
        public string notes { get; set; }

        public long phone_log_time { get; set; }
        public long phone_log_direction { get; set; }
        public string imei_ds { get; set; }
        public long calltime_nb { get; set; }
        public long calltype_fg { get; set; }
        public long numbertype_fg { get; set; }
        public long phone_log_id { get; set; }
    }
}
