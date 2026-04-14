using NESI.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NESI.DTO.Validation;

namespace NESI.DTO.ViewModels.Page.Reports
{
    public class Customer:ModelBase<Customer>
    {
        public int CustomerID { get; set; }

        [MaxLength(60)]
        [SQLInjection()]
        public string CustomerName { get; set; }

    }
}
