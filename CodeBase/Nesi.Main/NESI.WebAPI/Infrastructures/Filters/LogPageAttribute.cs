using System;
using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using log4net;
using nesi.core;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class LogPageAttribute : ActionFilterAttributeBase
	{
		private const string userIdKey = "logPage_userId";
		private const string StartTimeKey = "logPage_startTime";
		private const string StopwatchKey = "StopwatchFilter.Value";
	    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public override void OnActionExecuting(HttpActionContext actionContext)
		{
			base.OnActionExecuting(actionContext);
			var user = GetCurrentUser(actionContext);
			actionContext.Request.Properties[userIdKey] = user?.Id;
			actionContext.Request.Properties[StartTimeKey] = DateTime.Now;
			actionContext.Request.Properties[StopwatchKey] = Stopwatch.StartNew();
        }

		public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
		{
			base.OnActionExecuted(actionExecutedContext);
			var request = actionExecutedContext.Request;
			var stopwatch = (Stopwatch)actionExecutedContext.Request.Properties[StopwatchKey];
			try
				{
				if (actionExecutedContext.Response == null || !actionExecutedContext.Response.IsSuccessStatusCode)
					{
					if (actionExecutedContext.Exception == null)
						{
						logger.Error($"Request failed {actionExecutedContext.Request.RequestUri}, StatusCode: {actionExecutedContext.Response?.StatusCode.ToString() ?? "NO-CODE"}");
						}
					else
						{
						logger.Error($"Request failed with unhandled Exception {actionExecutedContext.Request.RequestUri}, StatusCode: {actionExecutedContext.Response?.StatusCode.ToString() ?? "NO-CODE"}",
							actionExecutedContext.Exception);
						}
					}
				}
			catch (Exception e)
				{
				logger.Error($"Failed to log Error {actionExecutedContext.Request.RequestUri}", e);
				Console.WriteLine(e);
				}

			try
			{
			
                var model = new DTO.Models.Core.LogPage
                {
					dt = DateTime.Now,
					member_id = (int?)actionExecutedContext.Request.Properties[userIdKey] ?? default(int),
					ip_address = GetClientIPAddress(request),
					url = request.RequestUri.AbsolutePath,
					query_string = request.RequestUri.Query,
					request_start = (DateTime)actionExecutedContext.Request.Properties[StartTimeKey],
					request_end = DateTime.Now,
					cl_process = (int)stopwatch.ElapsedMilliseconds,
					host = request.RequestUri.Host
				};
				// MH: These are more needed, but can easily be a nuisance when logging... they are actively filtered
				if(	Toolbox.Contains(model.url, new [] { 
						FilteredURLs.Ping,
						FilteredURLs.Active,
						FilteredURLs.DefaultPage,
						FilteredURLs.Profiles,
						FilteredURLs.MobileMenus,
						FilteredURLs.Menus
						})
					) 
					{ 
					return;
					}

				var log=new BLL.Core.Logger.WebAPIPageLogger();
				log.AddLog(model);
			}
			catch (Exception e)
			{
				logger.Error($"Failed to log page {actionExecutedContext.Request.RequestUri}", e);
                Console.WriteLine(e);
			}
		}
	}
	public struct FilteredURLs
		{
		public const string Ping = "/api/CurrentUser/ping";
		public const string Active = "/api/CurrentUser/Active";
		public const string DefaultPage = "/api/Layout/DefaultPage/";
		public const string Profiles = "/api/Layout/Profiles";
		public const string MobileMenus = "/api/Layout/MobileMenus";
		public const string Menus = "/api/Layout/Menus";
			
		}
}