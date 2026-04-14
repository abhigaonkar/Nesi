using System;
using System.Collections.Generic;
using NESI.DTO.Models.Core;

namespace NESI.DTO.ViewModels.Core
{
	public class OnlineUser
	{
		public string UserName { get; set; }
		public string FullName { get; set; }
		public string ActiveTime => CaculateActiveTime(LastActiveTime);

		public bool IsActive { get; set; }
		public bool IsContact { get; set; }
		public int Id { get; set; }
		public int BusinessUnitId { get; set; }
		public string BusinessUnitName { get; set; }
		public DateTime ExpiredTime { get; set; }
		public DateTime LastActiveTime { get; set; }
		public DateTime IssueTime { get; set; }
		public bool IsExpired => ExpiredTime < DateTime.Now;
		public bool IsLdapUser { get; set; }

		public object Profile { get; set; }
		public List<Models.Core.Page> Pages { get; set; }
		public List<Privilege> Privilges { get; set; }

        public static string CaculateActiveTime(DateTime time)
		{
			var dt = DateTime.Now.Subtract(time);
			return dt.TotalMinutes <= 1 ? (dt.TotalSeconds + 1).ToString("#0'sec'") : dt.TotalMinutes.ToString("#0'min'");
		}
	}
}