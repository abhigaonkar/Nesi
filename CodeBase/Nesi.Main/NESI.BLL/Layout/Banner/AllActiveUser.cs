using System;
using System.Linq;
using Microsoft.Ajax.Utilities;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.Member;

namespace NESI.BLL.Layout.Banner
{
	public class AllActiveUser
	{
		public BLL.Core.Member.ActiveUser[] GetAllActiveUsers()
		{
			return Global.OnlineUser.GetList()
				.Where(user => user.ExpiredTime > DateTime.Now && (DateTime.Now - user.PingTime).TotalSeconds <
				               60 * BLL.Common.Shared.Configuration.TimeOutOfPingTime)
				.OrderByDescending(user => user.LastActiveTime)
				.DistinctBy(x => x.Id)
				.Select(
					m => new ActiveUser(m)
				).ToArray();
		}
	}
}