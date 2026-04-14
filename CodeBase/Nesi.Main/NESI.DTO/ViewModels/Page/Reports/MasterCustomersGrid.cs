using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("MasterCustomersGrid")]
    public class MasterCustomersGrid : ModelBase<MasterCustomersGrid>
    {
        
        public long iD { get; set; }
        public string bVNo { get; set; }

    }
}
