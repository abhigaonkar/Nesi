using NESI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("Location")]
    public class Address : ModelBase<Address>
    {
        public int address_id { get; set; }

        [MaxLength(15)]
        [SQLInjection()]
        public string address_table { get; set; }

        public int address_table_id { get; set; }

        [SQLInjection()]
        public string Address_Type { get; set; }

        [MaxLength(60)]
        [SQLInjection()]
        public string Address_Desc { get; set; }

        [SQLInjection()]
        [MaxLength(135)]
        public string Address_Addr1 { get; set; }

        [MaxLength(135)]
        [SQLInjection()]
        public string Address_Addr2 { get; set; }

        [MaxLength(135)]
        [SQLInjection()]
        public string Address_Addr3 { get; set; }

        [MaxLength(135)]
        [SQLInjection()]
        public string Address_Addr4 { get; set; }

        [MaxLength(45)]
        [SQLInjection()]
        public string Address_City { get; set; }

        [MaxLength(120)]
        [SQLInjection()]
        public string Address_Email { get; set; }

        [MaxLength(120)]
        [SQLInjection()]
        public string Address_Web { get; set; }

        [MaxLength(150)]
        [SQLInjection()]
        public string facebook { get; set; }

        [SQLInjection()]
        public string Location { get; set; }
    }
}
