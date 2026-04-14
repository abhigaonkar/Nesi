using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
    public class InsertSignature
    {
       public string cutpo { get; set; }

        public string signatureField { get; set; }

        public string printname { get; set; }

        public int? customer_contact { get; set; }

        public string send_to_contact_ids { get; set; }

    }
}
