using System;
using System.Reflection;
using log4net;
using NESI.BLL.Common.Shared;

namespace NESI.WebAPI.Initialization    //Namespace to avoid collision with other Global class
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start(object sender, EventArgs e)
        {
            //
            //  Configure logging
            //
            log4net.Config.XmlConfigurator.Configure();
            logger.Info($"Application started in {Configuration.Environment}");
#if DEBUG
            logger.Info($"DEBUG symbol defined for this environment");
#else
            logger.Info($"DEBUG symbol NOT defined for this environment");
#endif

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            logger.Error("Unhandled application error", Server.GetLastError());
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}