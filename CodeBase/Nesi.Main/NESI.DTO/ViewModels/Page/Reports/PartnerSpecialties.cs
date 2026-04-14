using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("PartnerSpecialties")]
    public class PartnerSpecialties : ModelBase<PartnerSpecialties>
    {
        public string type { get; set; }
        public string skill { get; set; }
        public string dateAdded { get; set; }
        public string addedBy { get; set; }
        public string name { get; set; }
        public string business_unit { get; set; }
        public string phone { get; set; }
        public long id { get; set; }
        public bool is_partner { get; set; }
    }
}
