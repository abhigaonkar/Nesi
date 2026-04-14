using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Employees;
using NESI.WebAPI.Infrastructures.Filters;
using nesi.core;


namespace NESI.WebAPI.Controllers.API.Page.Employees
{
	[RoutePrefix("api/Page/Employee/Offer")]
	public class EmployeeOfferController : EmployeesControllerBase
	{
		[HttpGet]
		[Route("Notes/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult GetNotes(int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetNotes(0));
		}

		[HttpPost]
		[Route("Notes/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult SaveNotes([FromBody] EmployeeOfferNotes model, int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveNotes(model));
		}

		[HttpGet]
		[Route("Responsibilities/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult GetResponsibilities(int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetResponsibilitiesProfile());
		}

		[HttpPost]
		[Route("Responsibilities/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult OperateResponsibilities([FromBody] EmployeeOfferResponsibilitiesOperation model, int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			switch (model.operation)
			{
				case "checked_one":
					return Ok(o.Select_One_Responsibilities(true, model.value));
				case "unchecked_one":
					return Ok(o.Select_One_Responsibilities(false, model.value));
				case "select_all":
					return Ok(o.Select_All_Responsibilities(true));
				case "unselect_all":
					return Ok(o.Select_All_Responsibilities(false));
				case "select_copy_from_prev":
					return Ok(o.Copy_From_Previous(true));
				case "unselect_copy_from_prev":
					return Ok(o.Copy_From_Previous(false));
				default:
					return NotFound();
			}
		}

		[HttpGet]
		[Route("Detail/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult GetDetail(int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetDetailProfile());
		}

		[HttpGet]
		[VisibleBusinessUnitFilter()]
		[Route("Detail/BM/{buid}")]
		public IHttpActionResult GetBMID(int buid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser);
			return OkD(o.GetBMId(buid));
		}

		[HttpPost]
		[Route("Detail/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult SaveDetail([FromBody] EmployeeOfferDetail model, int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveDetail(model));
		}

		[HttpGet]
		[Route("Extras/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult GetExtra(int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetExtraProfile());
		}

		[HttpPost]
		[Route("Extras/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult SaveExtra([FromBody] EmployeeOfferExtra model, int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveExtras(model));
		}

		[HttpGet]
		[Route("SignBack/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult GetSignBack(int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetSignBackProfile());
		}

		[HttpPost]
		[Route("SignBack/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult UploadSignBack([FromBody] DataString model, int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveSignBack(model.Data));
		}

		[HttpDelete]
		[Route("SignBack/{isapplicant}/{memberid}/{applicantid}/{offerid}/{id}")]
		public IHttpActionResult DeleteSignBack(int isapplicant, int memberid, int applicantid, int offerid, int id)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.DeleteSignBack(id));
		}

		[HttpGet]
		[Route("Wage/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult GetWage(int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetWageProfile());
		}

		[HttpPost]
		[Route("Wage/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult SaveWage([FromBody] EmployeeOfferWage model, int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			return Ok(o.SaveWage(model));
		}

		[HttpPost]
		[Route("WageNotes/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult GetWageNotes([FromBody] DataIdDouble model, int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetWageNote(model.value, model.id));
		}

		[HttpGet]
		[Route("MileStone/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult GetMileStones(int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.GetMileStoneProfile());
		}


		[HttpPost]
		[Route("MileStone/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult SaveMileStone([FromBody] EmployeeMileStone model, int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.SaveMileStone(model));
		}

		[HttpDelete]
		[Route("MileStone/{isapplicant}/{memberid}/{applicantid}/{offerid}/{id}")]
		public IHttpActionResult DeleteMileStone(int isapplicant, int memberid, int applicantid, int offerid, int id)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			return Ok(o.DeleteMileStone(id));
		}


		[HttpGet]
		[Route("FilePath")]
		public IHttpActionResult GetFilePath()
		{
			var o = new BLL.Core.FileManager.TempFile(CurrentUser);
			return OkD(o.BasePath);
		}

		[HttpPost]
		[Route("Status/{isapplicant}/{memberid}/{applicantid}/{offerid}")]
		public IHttpActionResult ChangeStatus([FromBody] DataString model, int isapplicant, int memberid, int applicantid, int offerid)
		{
			var o = new BLL.Pages.Employees.EmployeeOffer(CurrentUser, isapplicant == 1, memberid, applicantid, offerid);
			if (!o.can_access) return NotFound();
			if (model.Data.ToLower() == "Accepted".ToLower() && NECredit_card_purchase.HasUnintegratedTransactions(memberid) && !o.BUMatches)
			{
				return Ok(o.ReturnErrorMessage());
			}

		    if ( model.Data.ToLower() == "Accepted".ToLower() && !string.IsNullOrWhiteSpace(model.Data2) && model.Data2.ToLower() == "Overwrite".ToLower())
		    {
				return Ok(o.ChangeStatus_OverwriteAcceptedOffer(model.Data, model.Data2));	
		    }
		    else
		    {
		        return Ok(o.ChangeStatus(model.Data));
            }
		}

	}
}
