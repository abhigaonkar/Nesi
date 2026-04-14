using DevExpress.Utils;
using DevExpress.XtraReports.Security;
using log4net;
using nesi.core;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Reflection;
using System.Security;
using System.Security.Policy;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using static core.XpoUtility;
using Configuration = NESI.BLL.Common.Shared.Configuration;


namespace Nesi.Web
{
    public class Global : HttpApplication
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ne_page_log pl;

#if DEBUG
        internal static ConcurrentDictionary<string, string> CurrentSessions { get; } = new ConcurrentDictionary<string, string>();
#endif

        protected void Application_PostAuthorizeRequest(object sender, EventArgs e)
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != null &&
                   HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
#if DEBUG
            Logger.Debug($"Session {Session.SessionID} has started");
            CurrentSessions.AddOrUpdate(Session.SessionID, Session.SessionID, (k,v)=>v);
#endif
        }

        protected void Session_End(Object sender, EventArgs e)
        {
#if DEBUG
            Logger.Debug($"Session {Session.SessionID} has ended");
            CurrentSessions.TryRemove(Session.SessionID, out var _);
#endif
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            //
            //  Configure logging
            //
            Logger.Info($"Application started in mode {Configuration.Environment}");

#if DEBUG
            Logger.Info($"DEBUG symbol defined for this environment");
#else
            Logger.Info($"DEBUG symbol NOT defined for this environment");

#endif

            NETheme01.ThemesProviderEx.Register();
            // DevExpress.Security.Resources.AccessSettings.StaticResources.TrySetRules(DirectoryAccessRule)
            // UrlAccessSecurityLevelSetting.SecurityLevel = UrlAccessSecurityLevel.Unrestricted;
            var evidence = new Evidence(new EvidenceBase[] { new Zone(SecurityZone.MyComputer) }, new EvidenceBase[] { });
            //FileIOPermission filePermission = new FileIOPermission(PermissionState.Unrestricted);

            var restrictPermissions = new IPermission[] { 
                // Uncommenting the following line will cause a security exception on an attempt to access the file system by report scripts. 
                // filePermission,  
            };


		ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);
		// ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Deny, evidence, restrictPermissions);
            StoreCoreXpoAssemblies(ConfigurationManager.ConnectionStrings["MySQLXPO"].ConnectionString);
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            string url;
            var host = Request.Url.Host;
            url = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
            var s = System.Web.HttpContext.Current.Session;
            var resp = System.Web.HttpContext.Current.Response;
            var isOverridden = s?["override_member_id"] != null;
            if(isOverridden)
                {
                if(!Request.RawUrl.StartsWith("/api/") && string.IsNullOrEmpty(Request.QueryString["override_member_id"]))
                    {
                    url                             = Request.Url.AbsoluteUri;
                    var uriBuilder                  = new UriBuilder(url);
                    var query                       = HttpUtility.ParseQueryString(uriBuilder.Query);
                    query["override_member_id"]     = s["override_member_id"].ToString();
                    uriBuilder.Query                = query.ToString();
                    resp.Redirect(uriBuilder.ToString());
                    return;
                    }
                }
            var bypassFVRs = Toolbox.app_setting("bypass_fvrs") == "1";

            if (!bypassFVRs && url.Contains("aspx") && !url.Contains("fvr") && !url.Contains("default.aspx") && !url.Contains("/api/") && !url.Contains("invoice_service") && !url.Contains("/mobile/") && !url.Contains("/get_file/") && !url.Contains("/_tools/inventory_search/"))
            {
                if (s != null && s["session"] != null)
                {
                    var sess = s["session"].ToString();
                    if (s["passport"] == null && s["shown_warning"] == null)
                    {
                        var current_user = s["profile"] != null ? (NeMember)s["profile"] : new NeMember(s["session"].ToString());

                        if (!current_user.isContact && current_user.business_unit != null && current_user.business_unit.fvr_lockout)
                        {
                            // Do they have any open FVR's?
                            var frms = member_fvr_hdr.chk_member(current_user.id);
                            var n_fvrs = frms.Rows.Count;
                            if (n_fvrs > 0)
                            {
                                // Yes
                                // How old is the oldest?
                                var oldest_fvr = Toolbox.doSQL_datetime(@" SELECT MIN(a.expire_date) expire_date FROM member_fvr_hdr a LEFT JOIN member_fvr_dtl b ON a.id = b.member_fvr_hdr_id LEFT JOIN member_fvr_history c ON b.id = c.member_fvr_dtl_id LEFT JOIN member_fvr_tab d ON b.tab_index = d.id AND a.type = d.type WHERE b.member_id = @v0  AND a.active = 1 AND IFNULL(c.confirmed, 0) = 0", new object[] { current_user.id });
                                var hasExpired = oldest_fvr < DateTime.Now;
                                // >= 15 days?
                                if (hasExpired && current_user.business_unit.fvr_lockout)
                                {
                                    // Lockout.
                                    resp.Redirect("~/default.aspx");
                                }
                                // < 15 days?
                                else
                                {
                                    // Have they been shown the warning?

                                    if (s["shown_warning"] == null)
                                    {
                                        resp.Redirect("~/default.aspx");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (s != null)
                {
                    Session["allowpopup"] = "true";
                    //s["passport"] = null;
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var c = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            c.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
            c.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            c.DateTimeFormat.ShortTimePattern = "HH:mm";
            c.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            c.DateTimeFormat.DateSeparator = "-";
            System.Threading.Thread.CurrentThread.CurrentCulture = c;
        }

        protected void Application_PreRequestHandlerExecute()
        {
            var abso_path = HttpContext.Current.Request.Url.AbsolutePath;
            if (abso_path.Contains("aspx") && !abso_path.Contains("_tools/inventory_search") && !abso_path.Contains("invoice_service") && !HttpContext.Current.Request.RawUrl.Contains("status"))
            {
                var s = HttpContext.Current.Session;
                var m = new NeMember();
                try
                {
                    m = s != null && s["profile"] != null ? s["profile"] as NeMember : new NeMember();
                }
                catch
                {
                }
                if (m == null)
                {
                    //TODO refactor don't think this is ever actually called
                    m = Toolbox.do_handle_authentication(1);
                    s["profile"] = m;
                }

                pl = new ne_page_log
                {
                    ip_address = HttpContext.Current.Request.UserHostAddress,
                    member_id = m.id,
                    url = HttpContext.Current.Request.Url.AbsolutePath,
                    query_string = HttpContext.Current.Request.Url.Query,
                    request_start = DateTime.Now
                };
                pl.save_asax();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PostRequestHandlerExecute()
        {
            var abso_path = HttpContext.Current.Request.Url.AbsolutePath;

            if (pl != null && abso_path.Contains("aspx") && !abso_path.Contains("_tools/inventory_search") && !abso_path.Contains("invoice_service") && !HttpContext.Current.Request.RawUrl.Contains("status"))
            {
                pl.request_end = DateTime.Now;
                pl.save_asax();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_Error(object sender, EventArgs e)
        {
            var h = HttpContext.Current;
            var r = h.Request;
            Page p;
            var show_error = true;

            if (h.Handler is Page page)
            {
                p = page;
                show_error = !p.IsPostBack && !p.IsCallback;
            }
            var lasterr = Server.GetLastError();

            Logger.Error("Unhandled application error", lasterr);

            if (show_error)
            {
                if (lasterr != null && !lasterr.ToString().Contains("webresource"))
                {
                    var objErr = Server.GetLastError().GetBaseException();

                    if (!HttpContext.Current.Request.IsLocal)
                    {
                        Toolbox.do_errorLog_errorStack(objErr);
                    }
                    var err = "";
                    if (objErr.Message.Contains("Unable to connect to any of the specified MySQL hosts"))
                    {
                        err = @"<div style='font-size:20px;font-weight:bold;color:#138;font-family:arial;border:solid 1px #ccc;padding:10px;position:absolute;width:500px;height:150px;border-radius:10px;top: 50%;left: 50%;margin-top: -50px;margin-left: -250px;' align='center'>
								<img src='/images/icon/icon[database].gif' width='94' height='94' />
								<br/>
								Cannot connect to NESI's database server.
								<div style='font-size:12px;color:#000'>The IT department has been alerted and will fix it <u style='color:#f00'>ASAP</u>.</div>
							</div>";
                        Response.StatusCode = 500;
                    }
                    else
                    {
                        var trace = new System.Diagnostics.StackTrace(objErr, true);
                        if (!Request.Url.AbsoluteUri.Contains("logout.aspx"))
                        {
                            err = "<script type='text/javascript' src='/js/jquery-1.3.2.min.js'></scrip" + "t>" + "<font face='Arial'><div style='font-size:20px;font-weight:bold;color:#f00;'>" + objErr.Message + "</div><br><hr>" +
                                "<div style='font-size:12px;'><b>Page Address: </b>" + r.Url +
                                "</div><br>" +
                                "<br><div id='stack'><pre style='font-size:12px;font-family:Arial;'>" + objErr.StackTrace.Replace(" in ", " in <b>").Replace(":line", "</b>:line") + "</pre></div>";

                        }
                    }

                    Response.Write(err);
                }
                Server.ClearError();
                Response.End();
            }
        }

    }
}