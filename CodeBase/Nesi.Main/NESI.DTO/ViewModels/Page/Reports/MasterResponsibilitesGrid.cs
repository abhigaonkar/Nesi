using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("MasterResponsibilitesGrid")]
    public class MasterResponsibilitesGrid : ModelBase<MasterResponsibilitesGrid>
    {
        public long crid { get; set; }
        public string name { get; set; }
        public string core_responsibility { get; set; }
        public string title { get; set; }
        public string should_be { get; set; }
        public string offered_title { get; set; }
        public string offered_cr { get; set; }
    }
}
