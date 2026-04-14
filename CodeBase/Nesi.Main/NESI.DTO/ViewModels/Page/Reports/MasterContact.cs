// ReSharper disable InconsistentNaming
using NESI.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("MasterContact")]
    public class MasterContact : ModelBase<MasterContact>
    {
        public int? customer_id;
        public int address_address_id;
        public int? project_mgr_id;

        public int contact_id { get; set; }

        [SQLInjection()]
        public string contact_name { get; set; }

        [MaxLength(255)]
        [SQLInjection()]
        public string contact_email { get; set; }

        [MaxLength(255)]
        [SQLInjection()]
        public string contact_title { get; set; }

        [MaxLength(255)]
        [SQLInjection()]
        public string contact_cellphone { get; set; }

        [SQLInjection()]
        public string contact_status { get; set; }

        [MaxLength(255)]
        [SQLInjection()]
        public string contact_extension { get; set; }

        public string contact_password { get; set; }

        [SQLInjection()]
        public string contact_type { get; set; }

        public Nullable<int> contact_status_id { get; set; }

        public bool contact_login_enabled { get; set; }

        [SQLInjection]
        public string contact_directline { get; set; }

        public int? contact_cust_id { get; set; }

        [SQLInjection]
        public string customer_name { get; set; }

        [MaxLength(60)]
        [SQLInjection()]
        public string vendor_name { get; set; }

        public bool stopsurveys { get; set; }

        [MaxLength(135)]
        [SQLInjection()]
        public string address_addr1 { get; set; }

        [MaxLength(45)]
        [SQLInjection()]
        public string address_city { get; set; }

        [MaxLength(16)]
        [SQLInjection()]
        public string address_postal { get; set; }

        [MaxLength(2)]
        [SQLInjection()]
        public string address_prov { get; set; }

        [SQLInjection]
        public string project_mgr { get; set; }

        [SQLInjection]
        public string business_unit { get; set; }

        public int? wos { get; set; }

        public int? quotes { get; set; }

        [SQLInjection]
        public string contact_login { get; set; }
        
    }
}
