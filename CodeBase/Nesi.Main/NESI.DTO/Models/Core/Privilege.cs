using AutoMapper.Attributes;
// ReSharper disable InconsistentNaming

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(NESI.Data.Entities.privilege))]
	[MapsFrom(typeof(NESI.Data.Entities.privilege))]
	public class Privilege
	{
		public int Privilege_ID { get; set; }
		public int Privilege_Page_ID { get; set; }
		public string Privilege_Name { get; set; }
		public string privilege_desc { get; set; }
		public int Privilege_Order { get; set; }
		public int Privilege_Enabled { get; set; }
		public int Privilege_CustEnabled { get; set; }
		public int Privilege_VendorEnabled { get; set; }
		public int requires_privilege_id { get; set; }

	}
}