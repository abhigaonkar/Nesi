using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Customers
    {

        [ModelDefination("CustomerBusinessUnitGrid")]
        public class CustomerBusinessUnitGrid : ModelBase<CustomerBusinessUnitGrid>
        {
            public int customer_id { get; set; }
            public string business_unit_name { get; set; }
            public string bdm { get; set; }
        }
    }


