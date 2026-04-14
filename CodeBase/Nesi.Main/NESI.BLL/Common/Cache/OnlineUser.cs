using System;
using NESI.BLL.Core.User;

namespace NESI.BLL.Common.Cache
{
	public class OnlineUser : MemoryCacher<User>
	{
		private const string CacheName = "OnlineUser";

		public OnlineUser() : base(CacheName)
		{

		}

		public OnlineUser(string name) : base(name)
		{
			
		}


		public void Set(Guid uid, User user)
		{
			var u = GetValue(uid);
			if (u == null)
			{
				Add(uid.ToString(),user, user.ExpiredTime);
			}
			else
			{
				Set(uid.ToString(), user);
			}
		}

		public User GetValue(Guid uid)
		{
			return base.GetValue(uid.ToString());
		}

		
		public User GetValueByUserId(int id)
		{
			return GetList().Find(u => u.Id == id);
		}

		public User GetValueByUserName(string userName)
		{
			return GetList().Find(u => u.UserName == userName);
		}
	}
}