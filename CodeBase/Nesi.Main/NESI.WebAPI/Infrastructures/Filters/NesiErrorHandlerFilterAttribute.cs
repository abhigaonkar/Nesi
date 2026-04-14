using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Owin;
using NESI.BLL.Core.User;
using NESI.Data.Entities;

namespace NESI.WebAPI.Infrastructures.Filters
{
	public class NesiErrorHandlerFilterAttribute : ExceptionFilterAttribute
	{

		protected OwinContext GetOwinContext(HttpRequestMessage request)
		{
			if (request.Properties.ContainsKey("MS_OwinContext"))
			{
				return (OwinContext)request.Properties["MS_OwinContext"];
			}
			else
			{
				return null;
			}
		}

		protected string GetUserId(OwinContext actionContext)
		{
			return actionContext.Authentication.User.Claims.AsQueryable().FirstOrDefault(c => c.Type == "userId")
				?.Value;
		}

		public override void OnException(HttpActionExecutedContext context)
		{
			try
			{
				var owin = GetOwinContext(context.Request);
				string Ipaddress;
				int userId;
				if (owin == null)
				{
					Ipaddress = ":11";
					userId = 0;
				}
				else
				{
					Ipaddress = owin.Request.RemoteIpAddress;
					userId = Convert.ToInt32(GetUserId(owin));
				}
				var model = new DTO.Models.Core.ErrorLog
				{
					Member_ID = userId,
					dt = DateTime.Now,
					error_short = context.Exception.Message,
					level = 1,
					host_url = context.Request.RequestUri.Host,
					full_stacktrace = context.Exception.StackTrace,
					user_ip = Ipaddress,
					origin = "WEBAPI",
					error_on_page = context.Request.RequestUri.PathAndQuery,
				};

				var log = new NESI.BLL.Core.Logger.ClientErrorHandler();
				log.AddLog(model);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				// throw;
			}

		}
	}
}