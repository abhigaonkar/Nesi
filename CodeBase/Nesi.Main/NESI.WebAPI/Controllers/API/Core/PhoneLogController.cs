using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.BLL.Core.Employee;
using NESI.WebAPI.Controllers.Base;

namespace NESI.WebAPI.Controllers.API.Core
{
	[RoutePrefix("api/Core/PhoneLog")]
	public class PhoneLogController : EmployeeController
	{
		[Route("{userId}/{date}")]
		public IHttpActionResult GetPhoneLogByExt(int userId, DateTime date)
		{
			// TODO: Check privilege.

			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return BadRequest();
			var list = new BLL.Core.Ne2PhoneLog().GetPhoneLogByExt(selectedUser.EmployeeProfile.Member_PhoneExtension, date);
			return Ok(list.Select(x => new
			{
				x.phone_log_id,
				x.phone_log_from_number,
				x.phone_log_from_name,
				x.phone_log_to_number,
				x.phone_log_to_name,
				x.phone_log_duration,
				x.notes
			}));
		}

		[HttpPatch]
		[Route("")]
		public IHttpActionResult UpdatePhoneLogByExt([FromBody] DTO.ViewModels.Core.UpdatePhoneLog data)
		{
			// TODO: Check privilege.
			var selectedUser = new Employee(data.UserId);
			if (!CanSeeUser(selectedUser)) return BadRequest();
			return Ok(CovertoJsonData(new BLL.Core.Ne2PhoneLog().UpdatePhoneLog(selectedUser.EmployeeProfile.Member_PhoneExtension, data.Id, data.Notes)));
		}
	}
}
