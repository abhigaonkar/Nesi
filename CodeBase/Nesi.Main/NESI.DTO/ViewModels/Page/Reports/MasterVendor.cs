// ReSharper disable InconsistentNaming
using NESI.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("MasterVendor")]
    public class MasterVendor:ModelBase<MasterVendor>
    {
        public int vendor_id { get; set; }

        [MaxLength(20)]
        [SQLInjection()]
        public string vendor_number { get; set; }

        [MaxLength(60)]
        [SQLInjection()]
        public string vendor_name { get; set; }

        public Nullable<DateTime> qc_date { get; set; }

        public bool is_partner { get; set; }

        public bool vendor_active { get; set; }

        [SQLInjection()]
        public string vendor_notes { get; set; }

        public bool vendor_hold { get; set; }

        public double total_purchase { get; set; }

        [MaxLength(45)]
        [SQLInjection()]
        public string address_city { get; set; }

        [SQLInjection()]
        public string address_prov { get; set; }

        [SQLInjection()]
        public string address_postal { get; set; }

        [MaxLength(3)]
        [SQLInjection()]
        public string address_country { get; set; }

        [SQLInjection()]
        public string phone { get; set; }

        public bool cprs { get; set; }

        public double last_12_months { get; set; }

        public double problems_12mo { get; set; }

        [SQLInjection()]
        public string address { get; set; }

        [SQLInjection()]
        public string address_web { get; set; }

        public string member_fullname { get; set; }

    }
}
