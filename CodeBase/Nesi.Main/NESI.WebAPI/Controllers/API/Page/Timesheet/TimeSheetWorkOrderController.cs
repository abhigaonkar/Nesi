using System;
using System.Linq;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.TimeSheet;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;

namespace NESI.WebAPI.Controllers.API.Page.Timesheet
{
	[RoutePrefix("api/Page/Timesheet/WorkOrder")]
	public class PageTimeSheetWorkOrderController : TimeSheetControllerBase
	{
		[Route("Customer/{buid}")]
		public IHttpActionResult GetCustomerList(int buid)
		{
			if (!CanSeeBusinessUnit(buid)) return BadRequest();
			var list = new BLL.Pages.Timesheet.Timesheet().GetCustomerWorkOrderList(buid, CurrentUser)
				.Select(x => new { x.Label, x.Value, custNo = x.customer_number }).Distinct();
			return Ok(list);
		}

		[Route("TsLitePaytypes/{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetTsLitePayTypes(int buid)
		{
			if (!CanSeeBusinessUnit(buid)) return BadRequest();
			var list = new BLL.Pages.Timesheet.Timesheet().GetTsLitePayTypes(buid);
			return Ok(list);
		}

		[Route("Labour/{buid}/{userId}/{payTypeId}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetlabourMasterId(int buid, int userId, int payTypeId)
		{

			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			var obj = new BLL.Pages.Timesheet.Timesheet().GetlabourMasterId(buid, selectedUser, payTypeId);

			return OkD(obj);
		}

		[Route("UnLinkTimeSheet/{buid}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetAllowUnlinkTimesheet(int buid)
		{

			if (!CanSeeBusinessUnit(buid)) return NotFound();

			var obj = new BLL.Pages.Timesheet.Timesheet().GetAllowUnlinkTimesheet(buid);

			return OkD(obj);
		}

		[Route("WOs/{buid}/{userId}/{customerId}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetWorkOrderList(int buid, int userId, int customerId)
		{

			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			var list = new BLL.Pages.Timesheet.Timesheet().LoadCustWOPROGWIPLIST(buid, CurrentUser, selectedUser, customerId)
				.Select(x => new { x.Label, x.Value, x.customer_id, x.scopes });

			return Ok(list);
		}

		[Route("WOs/{woId}")]
		public IHttpActionResult GetWorkOrderById(int woId)
		{
			var wo = new BLL.Core.Ne2WOProg(woId).Entity;
            
			if (woId>0 && !CanSeeBusinessUnit(wo.business_unit_id.GetValueOrDefault())) return BadRequest();

			return Ok(new BLL.Pages.Timesheet.Timesheet().GetWOPROGById(woId));
		}

        [Route("CutPO/{woId}")]
        public IHttpActionResult GetWorkOrderCUSTPOByWOID(int woId)
        {
            var wo = new BLL.Core.Ne2WOProg(woId).Entity;
            if (!CanSeeBusinessUnit(wo.business_unit_id.GetValueOrDefault())) return BadRequest();

            return Ok(ControllerHelper.ConvertoJsonData(new BLL.Pages.Timesheet.Timesheet().GetWOPROGById(woId).CutPO));

        }

        [Route("Customer/Address/Contact/{id}")]
        public IHttpActionResult GetContactByCustomer(int id)
        {
            return Ok(new BLL.Pages.Timesheet.Timesheet(CurrentUser).GetCustomerContactByCustomerId(id));
        }

        [Route("Address/Contact/{contact_id}")]
        public IHttpActionResult GetAddressByContact(int contact_id)
        {
            return OkD(new BLL.Pages.Timesheet.Timesheet(CurrentUser).GetAddressIdbyContactId(contact_id));
        }

        // [Route("Contact/{customerId}")]
        //// [VisibleBusinessUnitFilter()]
        // public IHttpActionResult GetContactList(int customerId)
        // {

        //    // var selectedUser = new Employee(userId);
        //    // if (!CanSeeUser(selectedUser)) return NotFound();

        //     var list = new BLL.Pages.Timesheet.Timesheet().LoadCustWOPROGWIPLIST(buid, CurrentUser, selectedUser, customerId)
        //         .Select(x => new { x.Label, x.Value, x.customer_id });

        //     return Ok(list);
        // }

		/// <summary>
		/// checks if this is scope on timesheet
		/// </summary>
		/// <param name="SelectedWorkOrderId">WO Id</param>
		/// <returns></returns>
		protected bool IsScopeOnTimesheet(int SelectedWorkOrderId)
		{
			return (SelectedWorkOrderId > 0) && (new nesi.core.NeWOProg(SelectedWorkOrderId)).enableEditingScopeOnTimesheet;
		}


        /// <summary>
        /// update work order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch]
		[Route("")]
		public IHttpActionResult UpdateWorkOrder([FromBody] DTO.ViewModels.Page.TimeSheet.UpdateTimeSheetWorkOrder model)
		{
			if (!CanSeeUser(model.SelectedBusinessUnitId, model.SelectedUserId)) return NotFound();

			if (!IsScopeOnTimesheet(model.SelectedWorkOrderId))
				if (string.IsNullOrWhiteSpace(model.MemberTime_WoComment) || (model.MemberTime_WoComment.Length < 10))
					return BadRequest();

			return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).UpdateWorkOrder(CurrentUser, model)));
		}

        [HttpPost]
        [Route("Signature/{WOID}/{IsDailySignoff}/{SelectedDate}")]
        public IHttpActionResult Insertsignature([FromBody] DTO.ViewModels.Page.TimeSheet.InsertSignature model,int WoID,bool IsDailySignoff,DateTime SelectedDate)
        { 
           return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).SaveSignature(model, WoID, IsDailySignoff,SelectedDate)));
        }

        /// <summary>
        /// insert work order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
		[Route("")]
		public IHttpActionResult InsertWorkOrder([FromBody] DTO.ViewModels.Page.TimeSheet.InsertTimeSheetWorkOrder model)
		{
			var selectedUser = new Employee(model.SelectedUserId);
			if (!CanSeeUser(selectedUser)) return NotFound();

            // For but 1973: Timesheet entry on a wrong date
            if (!SelectDateValid(model))
            {
                return Ok((CovertoJsonData("You are not allowed to create a timesheet record on selected date.")));
            }
			// If ! ScopeOnTimesheet - validate comment
			if (!IsScopeOnTimesheet(model.SelectedWorkOrderId))
				if (string.IsNullOrWhiteSpace(model.MemberTime_WoComment) || (model.MemberTime_WoComment.Length < 10))
					return BadRequest();

            if (model.entry_type != "multiple")
            {
                return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).SaveWorkOrder(selectedUser, model)));
            }
            else
            {
                return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).SaveMultipleWorkOrder(selectedUser, model)));
            }
		}

       

		[Route("Comment/{buid}/{userId}/{woId}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetCommentList(int buid, int userId, int woId)
		{
			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			var list = new BLL.Pages.Timesheet.Timesheet().LoadWOCommentsList(buid, userId, woId)
				.Select(x => new
				{
					Label = x.comment,
					Value = x.id
				}).DistinctBy(x => x.Label);

			return Ok(list);
		}

		[Route("CommentDisabled/{buid}/{userId}/{woId}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetCommentDisabled(int buid, int userId, int woId)
		{
			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			var o = new BLL.Pages.Timesheet.Timesheet().CommentDisabled(woId);
				
			return OkD(o);
		}

		[Route("IsPrevailingWage/{userId}/{timesheetId}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetIsPrevailingWage(int userId, int timesheetId)
        {
			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			var result = new BLL.Pages.Timesheet.Timesheet().IsPrevailingWage(timesheetId);

			return OkD(result);
		}

		[Route("IsPrevailingWageEnabledForWo/{userId}/{woId}")]
		[VisibleBusinessUnitFilter()]
		public IHttpActionResult GetIsPrevailingWageEnabledForWo(int userId, int woId)
		{
			var selectedUser = new Employee(userId);
			if (!CanSeeUser(selectedUser)) return NotFound();

			var result = new BLL.Pages.Timesheet.Timesheet().IsPrevailingWageEnabledForWo(woId);

			return OkD(result);
		}

		[Route("PreviewPdf/{WOID}/{IsDailySignoff}/{SelectedDate}")]
        public IHttpActionResult GetPreviewPdf(int WoID, bool IsDailySignoff,DateTime SelectedDate)
        {
            return Ok(CovertoJsonData(new BLL.Pages.Timesheet.Timesheet(CurrentUser).ReviewPDF(WoID, IsDailySignoff, SelectedDate)));
        }

        [HttpPost]
        [Route("Email")]
        public IHttpActionResult Email([FromBody] NESI.BLL.Pages.Timesheet.ContactEmailInput model)
        {
            var ts = new BLL.Pages.Timesheet.Timesheet(CurrentUser);
            var result = ts.SendEmailToContact(model);
            return Ok(CovertoJsonData(result));
        }


        // "This work order was not cut in NESI and cannot have time entered on it"

        // Your member type is not set up with a charge out rate in the selected business unit.
        // Until this is corrected, you will not be able to enter time on work orders in this business unit.


    }
}
