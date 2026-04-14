using System.Collections.Generic;
using NESI.BLL.Mebmer;

namespace NESI.Cache
{
	public class OnlineUser : MemoryCacher<NESI.DTO.Models.OnlineUser>
	{
		private const string CacheName = "OnlineUser";

		public OnlineUser() : base(CacheName)
		{

		}

		public void Set(User user)
		{
			var u = GetValue(user.Id);
			if (u == null)
			{
				Add(user.Id.ToString(),AutoMapper.Mapper.Map<NESI.DTO.Models.OnlineUser>(user), user.ExpiredTime);
			}
			else
			{
				Set(user.Id, AutoMapper.Mapper.Map<NESI.DTO.Models.OnlineUser>(user));
			}
		}
	}
}