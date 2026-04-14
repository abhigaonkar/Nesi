using NESI.BLL.Common.Cache;
using NESI.BLL.Core.User;
using NESI.DTO.Mapper;

namespace Nesi.Web
{
    public static class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                MapperConfig.SetConfiguration(cfg);
                cfg.CreateMap<User, OnlineUser>();
            });
        }
    }
}