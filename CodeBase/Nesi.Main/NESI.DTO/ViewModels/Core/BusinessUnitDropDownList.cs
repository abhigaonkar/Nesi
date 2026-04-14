// ReSharper disable InconsistentNaming

using AutoMapper.Attributes;

namespace NESI.DTO.ViewModels.Core
{
	[MapsFrom(typeof(NESI.DTO.Models.Core.BusinessUnit))]
	public class BusinessUnitDropDownList
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int tax_entity_id { get; set; }
		public string tax_entity_name { get; set; }
		public string ddl_name { get; set; }
	}
}