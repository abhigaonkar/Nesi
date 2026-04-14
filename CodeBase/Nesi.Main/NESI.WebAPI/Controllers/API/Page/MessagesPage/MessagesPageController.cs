using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.MessagesPage
{
	[PageAuthorizationFilter(29)]
	[RoutePrefix("api/Page/Messages")]
	public class MessagesPageController : EmployeeController
	{
		[HttpGet]
		[Route("Read/{typeId}/{messageId}")]
		public IHttpActionResult ReadMessage(int typeId, int messageId)
		{
			var o = new BLL.Pages.MessagesPage.MessagesPageBase(CurrentUser);
			return Ok(o.ReadMessage(typeId, messageId));
		}

		[HttpGet]
		[Route("FilePath")]
		public IHttpActionResult GetFilePath()
		{
			var o = new BLL.Core.FileManager.MessageAttachFile(CurrentUser);
			return OkD(o.BasePath);
		}

		[Route("UserList")]
		public IHttpActionResult GetUserList()
		{
			var o = new BLL.Pages.MessagesPage.MessagesPageBase(CurrentUser);
			return Ok(o.GetUserList());
		}
		[Route("Inbox")]
		public IHttpActionResult GetInbox()
		{
			var o = new BLL.Pages.MessagesPage.MessagesPageBase(CurrentUser);
			return Ok(o.GetInboxMessages(false));
		}

		[Route("Sent")]
		public IHttpActionResult GetSent()
		{
			var o = new BLL.Pages.MessagesPage.MessagesPageBase(CurrentUser);
			return Ok(o.GetOutboxMessages(false));
		}

		[Route("Deleted")]
		public IHttpActionResult GetDeleted()
		{
			var o = new BLL.Pages.MessagesPage.MessagesPageBase(CurrentUser);
			return Ok(o.GetDeleted());
		}

	
		[HttpPost]
		[Route("")]
		public IHttpActionResult SendMessage([FromBody] DTO.ViewModels.Page.MessagesPage.SendMessage model)
		{
			var o = new BLL.Pages.MessagesPage.MessagesPageBase(CurrentUser);
			return OkD(o.SendMessage(model));
		}

		[HttpPost]
		[Route("Delete/{type}")]
		public IHttpActionResult DeleteMessage([FromBody] int[] ids, int type)
		{
			var o = new BLL.Pages.MessagesPage.MessagesPageBase(CurrentUser);
			return OkD(o.deleteMessages(ids, type));
		}
	}
}
