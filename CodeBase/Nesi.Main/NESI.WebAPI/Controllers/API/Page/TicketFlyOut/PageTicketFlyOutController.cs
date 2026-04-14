using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Page.TicketFlyOut
{
	[RoutePrefix("api/Page/TicketFlyOut")]
	public class PageTicketFlyOutController : EmployeeController
	{
		[Route("Group")]
		public IHttpActionResult GetGroup()
		{
			return Ok(new BLL.Pages.TicketFlyOut.TicketFlyOut().GetddlGroups());
		}
		[Route("RelatedTickets/{pageid}")]
		public IHttpActionResult GetRelatedTickets(int pageid)
		{
		//	if (!CurrentUser.AuthorizePage(pageid)) return NotFound();
			return Ok(new BLL.Pages.TicketFlyOut.TicketFlyOut(CurrentUser).GetRelatedTickets(pageid));
		}

		[Route("Group/{groupId}/{pageId}")]
		public IHttpActionResult GetTypesAndPerts(int groupId, int pageId)
		{
			var t = new BLL.Pages.TicketFlyOut.TicketFlyOut();
			var p = new BLL.Core.FileManager.TicketFile(CurrentUser);

			return Ok(new
			{
				Types = t.GetddlTypes(groupId),
				Perts = t.GetddlPerts(groupId),
				TicketPageId = t.GetTicketPageId(groupId, pageId),
				FilePath = p.GetUserTempPath()
			});
		}

		[HttpPost]
		[Route("Save")]
		public IHttpActionResult SaveTicket(DTO.ViewModels.Page.TicketFlyOut.AddTicket model)
		{
			return OkD(new BLL.Pages.TicketFlyOut.TicketFlyOut(CurrentUser).Save(model));
		}

		[HttpDelete]
		[Route("File")]
		public IHttpActionResult DeleteFile(string name)
		{
			var path = new BLL.Core.FileManager.TicketFile(CurrentUser).GetUserTempPath();
			path = System.IO.Path.Combine(path, name);
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}
			return OkD("File has been deleted successfully.");
		}

		[HttpPost]
		[Route("File")]
		public IHttpActionResult DirectUpLoadFile(string name)
		{
			var httpRequest = HttpContext.Current.Request;
			var path = new BLL.Core.FileManager.TicketFile(CurrentUser).GetUserTempPath();
			path = System.IO.Path.Combine(path, name);
			try
			{
				if (System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);
				}
				using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
				{
					httpRequest.InputStream.CopyTo(fileStream);
				}
				return OkD(path);
			}
			catch (Exception)
			{
				return BadRequest();
			}

		}

		[HttpGet]
		[Route("File")]
		public IHttpActionResult GetFile(string name)
		{
			var file = new BLL.Pages.TicketFlyOut.TicketFlyOut(CurrentUser).GetFileBlob(name);
			return string.IsNullOrEmpty(file) ? NotFound() : OkD(file);
		}

	}
}
