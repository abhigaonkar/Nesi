using AutoMapper;
using NESI.BLL.Core.User;
using NESI.DTO.Models;
using NESI.DTO.Models.Core;
using NESI.DTO.Models.Users;
using NESI.DTO.ViewModels.Core;

namespace NESI.WebAPI
{
	public static class MappingConfig
	{
		public static void RegisterMaps()
		{
			AutoMapper.Mapper.Initialize(cfg =>
			{
				DTO.Mapper.MapperConfig.SetConfiguration(cfg);
				cfg.CreateMap<User, OnlineUser>();

			});
		}
	}
}