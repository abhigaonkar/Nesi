using log4net;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core;
using NESI.BLL.EmbeddedFiles;
using NESI.BLL.EmbeddedResources;
using NESI.BLL.Repository;
using NESI.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using nesi.core;
using NESI.BLL.Core.User;
using NESI.Common.Security;

#pragma warning disable 1998

namespace NESI.WebAPI.Infrastructures.Authentication
{
    public class NesiAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string PassportGrantType = "passport";
        private readonly string PassportHash = "passport_hash";
        private readonly string InvalidGrantError = "invalid_grant";

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
#pragma warning restore 1998
        {
            try
            {
                using (var repo = new AuthRepository())
                {
                    if (context.UserName != null)
                    {
                        var user = repo.FindUser(context.UserName, context.Password, context.Request.RemoteIpAddress);

                        if (user == null)
                        {
                            /*
                             * NB
                             * The order of the next two lines matters if you want the message to be sent back to client
                             */
                            context.Rejected();
                            context.SetError(InvalidGrantError, "Invalid username/password");

                        }
                        else if (user.IsContact)
                        {
                            context.Rejected();
                            string errorContent =
                                AssemblyFileLoader.LoadFile<EmbeddedBLLResourceMarker, EmbeddedBLLResourceMarker>(
                                    "CustomerLoginDisabled.html");
                            context.SetError(InvalidGrantError, errorContent);
                        }
                        else
                        {
                            var ticket = CreateTicket(user, context.Options.AuthenticationType);
                            context.Validated(ticket);
                        }
                    }
                }

            }
            catch (NesiException nesiException)
            {
                Logger.Error("Failed to login due to error", nesiException);

                context.Rejected();
                context.SetError(InvalidGrantError, nesiException.Message);
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to login due to error", ex);
                context.Rejected();
                context.SetError(InvalidGrantError, "An error occurred while logging in");
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            // chance to change authentication ticket for refresh token requests
            var newId = new ClaimsIdentity(context.Ticket.Identity);
            newId.AddClaim(new Claim("newClaim", "refreshToken"));

            var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
            context.Validated(newTicket);
        }


        public override async Task GrantCustomExtension(OAuthGrantCustomExtensionContext context)
        {
            var passportId = context.Parameters[PassportHash];
            if (context.GrantType != PassportGrantType || string.IsNullOrEmpty(passportId))
            {
                context.Rejected();
                return;
            }

            var passport = new passport(passportId);
            if (!passport.check())
            {
                context.Rejected();
                context.SetError(InvalidGrantError, "Invalid passport");
                return;
            }

            using (var repo = new AuthRepository())
            {
                var user = repo.FindUser(passport.to_member_id);

                if (user == null)
                {
                    context.Rejected();
                    context.SetError(InvalidGrantError, "Invalid passport member");
                }
                else
                {
                    var ticket = CreateTicket(user, context.Options.AuthenticationType);
                    context.Validated(ticket);
                }
            }
        }

        private AuthenticationTicket CreateTicket(User user, string authenticationType)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (authenticationType == null) throw new ArgumentNullException(nameof(authenticationType));
            var uid = Guid.NewGuid();
            Global.OriginalUser.Set(uid, user);
            Global.OnlineUser.Set(uid, user);
            var identity = new ClaimsIdentity(authenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, uid.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.IsContact() ? "Contact" : "Employee"));
            identity.AddClaim(new Claim(NesiClaimTypes.NesiUserName, user.UserName));
            identity.AddClaim(new Claim(NesiClaimTypes.NesiInternalId, user.Id.ToString()));
            identity.AddClaim(new Claim(NesiClaimTypes.NesiIsContact, user.IsContact().ToString()));
            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {"guid", uid.ToString()},
                {"userId", user.Id.ToString()},
                {"isContact", user.IsContact().ToString().ToLower()},
                {"isDeveloper", user.isDeveloper.ToString().ToLower()},
                {"expireSecond", user.ExpireSecond.ToString()}
            });
            var ticket = new AuthenticationTicket(identity, props);
            return ticket;
        }
    }
}