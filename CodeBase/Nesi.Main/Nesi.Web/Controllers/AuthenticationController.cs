using log4net;
using nesi.core;
using Nesi.Web.Models;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;
using NESI.BLL.Layout.Banner;
using NESI.BLL.Repository;
using NESI.Common.Exceptions;
using NESI.Common.Security;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Nesi.Web.Controllers
{

    [RoutePrefix("api/authentication")]
    public class AuthenticationController : ApiController
    {
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(LoginModel loginModel)
        {
            try
            {
                Toolbox.DoSignOut();
                var member = Toolbox.Authenticate(loginModel.Username, loginModel.Password);
                if (member.Authenticated)
                {
                    Toolbox.CreateSession(member);
                    return Ok();
                }

                return Unauthorized();
            }
            catch (NesiException nex)
            {
                Logger.Error("Error logging in to N1", nex);
                return InternalServerError(nex);
            }
            catch (Exception ex)
            {
                Logger.Error("Error logging in to N1", ex);
                return InternalServerError();
            }
        } 

        [HttpPost]
        [Route("switchUser")]
        [Authorize]
        public IHttpActionResult SwitchUser(SwitchUserModel switchUser)
        {
            try
            {
                var memberId = GetMemberIdClaim();
                if (memberId == null)
                {
                    Logger.Error($"Unable to switch users, no authorized user or no valid ID claim");
                    return Unauthorized();
                }
                var currentUserGuid = this.GetUserId();

                var currentUser = new AuthRepository().FindUser(memberId.Value);
                //check if user is already logged in
                NeMember member;
                if (memberId == switchUser.Id)
                {
                    //get N1 user from session
                    var mySession = HttpContext.Current.Session["session"];

                    if ( mySession != null )
                    {
                        var n1CurrentUser = new ne_session(Convert.ToInt32(mySession.ToString()));
                        //Check if session is valid and validate if needed
                        if (n1CurrentUser.member_id == switchUser.Id)
                        {
                            return Ok();
                        }
                    }
                    else
                    {
                        //session is null need to rebuild
                        member = Toolbox.Authenticate(switchUser.Id);
                        if (member.Authenticated)
                        {
                            Toolbox.CreateSession(member);
                            return Ok();
                        }
                    }
                    
                }
                Logger.Debug($"Switching identity from member {memberId} to {switchUser.Id}");
                
                if (currentUserGuid == null || currentUser == null || !currentUser.IsEmployee())
                {
                    Logger.Error($"Unable to load current user for id {memberId.Value}, or user not an employee");
                    return Unauthorized();
                }
                var userSwitcher = new SwtichUser(currentUserGuid.Value, currentUser as Employee);

               
                if (!userSwitcher.CanSwitchUser)
                {
                    Logger.Error($"Switching identity from member {memberId} to {switchUser.Id} restricted");
                    return Unauthorized();
                }

                Toolbox.DoSignOut();
                
                member = Toolbox.Authenticate(switchUser.Id);
                if (member.Authenticated)
                {
                    Toolbox.CreateSession(member);
                    return Ok();
                }
                return Unauthorized();
            }
            catch (NesiException nex)
            {
                Logger.Error("Error logging in to N1", nex);
                return InternalServerError(nex);
            }
            catch (Exception ex)
            {
                Logger.Error("Error logging in to N1", ex);
                return InternalServerError();
            }
        } 
        /// <summary>
        /// Use the web api authorization pipeline to authenticate a member using the
        /// provided OAuth token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("token-login")]
        public IHttpActionResult AuthorizedLogin()
        {
            try
            {
                Logger.Debug($"Authenticating user using OAuth token. Current session: {HttpContext.Current.Session.SessionID}");
                var memberId = GetMemberIdClaim();

                if (memberId == null)
                {
                    Logger.Error($"Unable to process authorized user, no valid ID claim");
                    return Unauthorized();
                }
                Toolbox.DoSignOut();
                var member = Toolbox.Authenticate(memberId.Value);
                if (member.Authenticated)
                {
                    Toolbox.CreateSession(member);
                    Logger.Debug($"Completed authenticating user using OAuth token. Current session: {HttpContext.Current.Session.SessionID}");

                    return Ok();
                }
                return Unauthorized();
            }
            catch (NesiException nex)
            {
                Logger.Error("Error logging in to N1", nex);
                return InternalServerError(nex);
            }
            catch (Exception ex)
            {
                Logger.Error("Error logging in to N1", ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            try
            {
                Toolbox.DoSignOut();
                return Ok();
            }
            catch (NesiException nex)
            {
                Logger.Error("Error logging out of N1", nex);
                return InternalServerError(nex);
            }
            catch (Exception ex)
            {
                Logger.Error("Error logging out of N1", ex);
                return InternalServerError();
            }
        }

#if DEBUG
         [HttpGet]
        [Route("sessions")]
        public IHttpActionResult GetSessions()
        {
            return Ok(Global.CurrentSessions.Keys);
        }
#endif
        /// <summary>
        /// Gets the claim corresponding to the currently authorized identity,
        /// or null if no such claim exists
        /// </summary>
        /// <returns></returns>
        private int? GetMemberIdClaim()
        {
            var claims = HttpContext.Current.User is ClaimsPrincipal principal ? principal.Claims : null;
            var memberIdStr = claims?.SingleOrDefault(c => c.Type == NesiClaimTypes.NesiInternalId)?.Value;

            if (memberIdStr != null && int.TryParse(memberIdStr, out var memberId))
            {
                return memberId;
            }
            return null;
        }

        /// <summary>
        /// Safely extract the claims identity name guid and return it; 
        /// Return null if non existent or invalid
        /// </summary>
        private Guid? GetUserId() => 
            Guid.TryParse((HttpContext.Current.User as ClaimsPrincipal)?.Identity?.Name, out var userId)? userId: new Guid?();

    }
}
;