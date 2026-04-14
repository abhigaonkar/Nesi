using System;
using NESI.BLL.Base;
using NESI.Data.Entities;

namespace NESI.BLL.Common.ToolBox
{
	public class FailedLogin: BLLBase
	{
		public void AddLog(string name, string password, string hostAddress)
		{
			var entity = new failed_login
			{
				dt=DateTime.Now,
				user=name,
				pass = string.Empty,    //No longer store passwords in plain text
				ip_address = hostAddress
			};

			_db.failed_login.Add(entity);
			_db.SaveChanges();
		}

		
	}
}