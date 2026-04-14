using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NESI.BLL.Core.Employee;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet
{
	[RoutePrefix("api/Page/Timesheet/Shop")]
	public class PageTimeSheetShopController : TimeSheetControllerBase
	{
		[Route("ShopType")]
		public IHttpActionResult GetShopTypeList()
		{
            var list = new BLL.Pages.Timesheet.Timesheet(CurrentUser).GetShopTypeList(CurrentUser)
                .Select(x => new { Label = x.type, Value = x.id });
			return Ok(list);
		}

		/// <summary>
		/// update shop
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPatch]
		[Route("")]
		public IHttpActionResult UpdateShop([FromBody] DTO.ViewModels.Page.TimeSheet.UpdateTimeSheetShop model)
		{
			if (!CanSeeUser(model.SelectedBusinessUnitId, model.SelectedUserId)) return NotFound();

			return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).UpdateShop(CurrentUser, model)));
		}

		/// <summary>
		/// insert Shop
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("")]
		public IHttpActionResult InsertShop([FromBody] DTO.ViewModels.Page.TimeSheet.InsertTimeSheetShop model)
		{
			var selectedUser = new Employee(model.SelectedUserId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).SaveShop(selectedUser, model)));
		}

	}
}
