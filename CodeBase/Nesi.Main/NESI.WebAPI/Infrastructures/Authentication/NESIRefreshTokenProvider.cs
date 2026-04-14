using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using NESI.BLL.Common.Cache;

namespace NESI.WebAPI.Infrastructures.Authentication
{
	public class NesiRefreshTokenProvider : IAuthenticationTokenProvider
	{
		private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens;
		private readonly DateTime _expiryDate;

		public NesiRefreshTokenProvider(DateTime expiryDate)
		{
			_refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();
			_expiryDate = expiryDate;
		}

#pragma warning disable 1998
		public async Task CreateAsync(AuthenticationTokenCreateContext context)
#pragma warning restore 1998
		{
			var guid = Guid.NewGuid().ToString();

			// maybe only create a handle the first time, then re-use for same client
			// copy properties and set the desired lifetime of refresh token
			var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
			{
				IssuedUtc = context.Ticket.Properties.IssuedUtc,
				ExpiresUtc = this._expiryDate  //  DateTime.Now.AddYears(1)

			};
			var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

			_refreshTokens.TryAdd(guid, refreshTokenTicket);

			// consider storing only the hash of the handle
			context.SetToken(guid);

			return;
		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{
			AuthenticationTicket ticket;
			if (_refreshTokens.TryRemove(context.Token, out ticket))
			{
				// reset userIssueTime and expired time
				var key = ticket.Identity.Name;
				if (key == null) return;
				var user = Global.OnlineUser.GetValue(Guid.Parse(key));
				if (user == null) return;
				user.SetIssueTime();
				var ouser = Global.OriginalUser.GetValue(Guid.Parse(key));
				if (ouser == null) return;
				ouser.SetIssueTime();
				ticket.Properties.Dictionary["expireSecond"] = user.ExpireSecond.ToString();
				context.SetTicket(ticket);
				// BLL.Cache.Global.OnlineUser.Set(Guid.Parse(key),user);
			}

			return;
		}

		public void Create(AuthenticationTokenCreateContext context)
		{
			throw new NotImplementedException();
		}

		public void Receive(AuthenticationTokenReceiveContext context)
		{
			throw new NotImplementedException();
		}
	}
}