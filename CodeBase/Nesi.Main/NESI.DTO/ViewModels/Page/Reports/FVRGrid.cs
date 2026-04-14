using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("FVRGrid")]
    public class FVRGrid : ModelBase<FVRGrid>
    {
        public long id { get; set; }
        public string active { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
        public string business_unit { get; set; }
        public string cut_by { get; set; }
        public string employee { get; set; }
        public string filename { get; set; }
        public string confirmed { get; set; }
        public string upload_required { get; set; }
        public string uploaded_filename { get; set; }
        public string hr_status { get; set; }
        public string fvr_status { get; set; }
    }
}
