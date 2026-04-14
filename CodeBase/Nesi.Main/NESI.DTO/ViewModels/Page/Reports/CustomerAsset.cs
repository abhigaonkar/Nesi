// ReSharper disable InconsistentNaming
using NESI.Common;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Runtime.Serialization;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("CustomerAsset")]
    public class CustomerAsset:ModelBase<CustomerAsset>
    {
        public int id { get; set; }

        [MaxLength(255)]
        [SQLInjection()]
        public string name { get; set; }

        [MaxLength(60)]
        [SQLInjection()]
        public string customer_name { get; set; }
        public int? customer_id { get; set; }
        public int? address_id { get; set; }

        [MaxLength(2000)]
        [SQLInjection()]
        public string description { get; set; }
        public bool active { get; set; }

        [SQLInjection()]
        public string addedby { get; set; }

        [MaxLength(255)]
        [SQLInjection()]
        public string manufacturer { get; set; }

        [MaxLength(255)]
        public string model { get; set; }

        [MaxLength(200)]
        [SQLInjection()]
        public string location { get; set; }

        public int? member_id { get; set; }

        //[MaxLength(100)]
        //[SQLInjection()]
        //public string member_FullName { get; set; }



    }
}
