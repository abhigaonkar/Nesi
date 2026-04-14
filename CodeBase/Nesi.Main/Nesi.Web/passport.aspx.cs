using nesi.core;
using System;
using System.Web.UI;
using NESI.BLL.Common.Shared;
using NESI.BLL.Core.Member;

public partial class pass_port : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        _tools.dont_cache_page();

        var _q = Request.QueryString;

        if (string.IsNullOrEmpty(_q["req"]) || string.IsNullOrEmpty(_q["type"]))
        {
            Toolbox.FriendlyException(Response, "You are missing the your Passport number!",
                $"{Request.Url.Host}?signout=1");
            return;
        }

        var passport = new passport(_q["req"]);
        if (!passport.check())
        {
            Toolbox.FriendlyException(Response,
                "Your Passport has expired, or the associated item has already been handled.",
                $"{Request.Url.Host}?signout=1");
            return;
        }

        if (passport.valid < 0)
        {
            _tools.set_plain_header();
            Response.Write("Invalid Request");
        }

        if (Session["session"] != null && Session["session"].ToString() == "0")
        {
            Session["session"] = null;
        }

        if (Session["session"] != null)
        {
            Toolbox.DoSignOut();
        }

        var apiAuthenticationTicket = new NesiWebApiClient().AuthenticateWithPassport(passport);

        if (apiAuthenticationTicket == null || apiAuthenticationTicket.UserId != passport.to_member_id)
        {
            Toolbox.FriendlyException(Response, "Unable to process your request.",
                $"{Request.Url.Host}?signout=1");
            return;
        }

        //
        //  Grab the active user from the web api so that we can send it to the 
        //  front end for login purposes
        //
        var activeUser = new NesiWebApiClient().GetActiveUser(apiAuthenticationTicket);
        if (activeUser == null)
        {
            Toolbox.FriendlyException(Response, "Unable to process your request. Invalid user.",
                $"{Request.Url.Host}?signout=1");
            return;
        }

        //
        // Create our local member and session ( we're now logged in to N1 )
        //
        var currentUser = new NeMember(passport.to_member_id);
        if (!currentUser.Authenticated) throw new InvalidOperationException("Invalid passport");
        var session = Toolbox.CreateSession(currentUser, false);
        currentUser.SessionID = session.id;
        var redirectUrl = passport.process((_q["type"] == "approve"));

        new ResponseUtility(Response).ForceApiLoginAndRedirect(apiAuthenticationTicket, activeUser, redirectUrl);
    }
}
