using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Customers
{


	 [ModelDefination("CustomerRequestAdminGrid")]  // Fixed: "Definition" not "Defination"
    public class CustomerRequestAdminGrid : ModelBase<CustomerRequestAdminGrid>
	{
		public int id { get; set; }
		public  string name { get; set; }
		public bool is_active { get; set; }
		public int? industrial_type_id { get; set; }

	}
}