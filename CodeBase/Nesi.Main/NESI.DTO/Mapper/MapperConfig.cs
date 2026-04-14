using AutoMapper;
using AutoMapper.Attributes;
using NESI.DTO.Models.Core;
using NESI.DTO.Models.Users;

namespace NESI.DTO.Mapper
{
	public static class MapperConfig
	{
		public static void SetConfiguration(IMapperConfigurationExpression cfg)
		{
				typeof(Member).Assembly.MapTypes(cfg);
				typeof(Contact).Assembly.MapTypes(cfg);
				typeof(Page).Assembly.MapTypes(cfg);
				typeof(Privilege).Assembly.MapTypes(cfg);
				typeof(BusinessUnit).Assembly.MapTypes(cfg);
				typeof(TaxEntity).Assembly.MapTypes(cfg);
		}


		public static void Initialize()
		{
			AutoMapper.Mapper.Initialize(SetConfiguration);			
		}
	}
}