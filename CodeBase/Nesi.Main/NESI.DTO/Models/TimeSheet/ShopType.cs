using AutoMapper.Attributes;

namespace NESI.DTO.Models.TimeSheet
{
	[MapsTo(typeof(NESI.Data.Entities.membertime_shop_type))]
	[MapsFrom(typeof(NESI.Data.Entities.membertime_shop_type))]
	public class MemberTimeShopType
	{
		public int id { get; set; }
		public string type { get; set; }
		public string status { get; set; }
	}
}