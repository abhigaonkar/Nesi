using System;
using NESI.BLL.Common.Shared;
using NESI.BLL.Common.ToolBox;
using NESI.BLL.Core.Member;
using NESI.BLL.Core.User;

namespace NESI.BLL.Repository
{
	public class AuthRepository : IDisposable
	{
        /// <summary>
        /// Finds user by name and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
		public User FindUser(string userName, string password, string ipAddress)
		{
			var user = new UserFactory().GetUser(userName);
			if (user !=null && user.IsActive &&  UserManager.IsCorrectPasswordForUser(user, password)) return user;
            //
            //  Log the failed login attempt, will not log the password in plaintext
            //
			new FailedLogin().AddLog(userName, password, ipAddress);
			return null;
		}

	    public User FindUser(int memberId)
	    {
	        var user = new UserFactory().GetUser(memberId);
	        if (user != null && user.IsActive) return user;
	        return null;
	    }

		public void Dispose()
		{
		}
	}
}