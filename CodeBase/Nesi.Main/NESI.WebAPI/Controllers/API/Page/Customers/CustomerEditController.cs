using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using nesi.core;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Customers;

namespace NESI.WebAPI.Controllers.API.Page.Customers
{
	[RoutePrefix("api/Page/Customers/Edit")]
	public class CustomerEditController : CustomerControllerBase
	{
		[HttpGet]
		[Route("Profile/{cust_id}")]
		public IHttpActionResult GetProfile(int cust_id)
		{
			var o = new BLL.Pages.Customers.CustomerEdit(CurrentUser, cust_id);
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("Save/{cust_id}")]
		public IHttpActionResult Save([FromBody] DTO.ViewModels.Page.Customers.CustomerEdit model, int cust_id)
		{
			var o = new BLL.Pages.Customers.CustomerEdit(CurrentUser, cust_id);
			return Ok(o.Save(model));
		}

		[HttpGet]
		[Route("Address/{cust_id}/{address_id}")]
		public IHttpActionResult GetAddress(int cust_id, int address_id)
		{
			var o = new BLL.Pages.Customers.CustomerEdit(CurrentUser, cust_id);
			return Ok(o.GetAddress(cust_id, address_id));
		}

		[HttpPost]
		[Route("Address/{cust_id}/{address_id}")]
		public IHttpActionResult SaveAddress([FromBody] CustomerAddress model, int cust_id, int address_id)
		{
			var o = new BLL.Pages.Customers.CustomerEdit(CurrentUser, cust_id);
			return Ok(o.SaveAddress(model));
		}


		[HttpGet]
		[Route("AddressAccounting/{cust_id}/{address_id}")]
		public IHttpActionResult GetAccounting(int cust_id, int address_id)
		{

			var o = new BLL.Pages.Customers.CustomerEdit(CurrentUser, cust_id);
			return Ok(o.GetAccounting(cust_id, address_id));
		}

		[HttpPost]
		[Route("AddressAccounting/{cust_id}/{address_id}")]
		public IHttpActionResult SaveAccounting([FromBody] CustomerAccounting model, int cust_id, int address_id)
		{
			if (model.customer_id != cust_id || model.address_id != address_id)
			{
				return NotFound();
			}
			var o = new BLL.Pages.Customers.CustomerEdit(CurrentUser, cust_id);
			return OkD(o.SaveAccounting(model));
		}

		[HttpGet]
		[Route("AccountingSettings/Setting/{cust_id}")]
		public IHttpActionResult GetAccountingSetting(int cust_id)
		{
			var o = new BLL.Pages.Customers.CustomerAccountingSettings(CurrentUser, cust_id);
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("AccountingSettings/Setting/{cust_id}")]
		public IHttpActionResult SaveAccountingSetting([FromBody] CustomerAccountingSetting model, int cust_id)
		{
			if (model.customer_id != cust_id)
			{
				return NotFound();
			}
			var o = new BLL.Pages.Customers.CustomerAccountingSettings(CurrentUser, cust_id);
			return OkD(o.SaveSetting(model));
		}

		[HttpGet]
		[Route("AccountingSettings/ARNotes/{cust_id}")]
		public IHttpActionResult GetAccountingARNotes(int cust_id)
		{

			var o = new BLL.Pages.Customers.CustomerAccountingSettings(CurrentUser, cust_id);
			return Ok(o.ARNotes());
		}

		[HttpPost]
		[Route("AccountingSettings/Rates/{cust_id}/{address_id}/{bu_id}")]
		public IHttpActionResult SaveAccountingSaveRate([FromBody] CustomerRate model, int cust_id, int address_id, int bu_id)
		{
			var o = new BLL.Pages.Customers.CustomerAccountingSettings(CurrentUser, cust_id);
			model.business_unit_id = bu_id;
			model.address_id = address_id;
			return Ok(o.SaveRate(model));
		}

		[HttpPost]
		[Route("AccountingSettings/Rates/Delete/{cust_id}/{address_id}/{bu_id}")]
		public IHttpActionResult DeleteAccountingSaveRate([FromBody] CustomerRate model, int cust_id, int address_id, int bu_id)
		{
			var o = new BLL.Pages.Customers.CustomerAccountingSettings(CurrentUser, cust_id);
			model.business_unit_id = bu_id;
			model.address_id = address_id;
			return Ok(o.DeleteRate(model));
		}

		[HttpPost]
		[Route("AccountingSettings/ARNotes/{cust_id}")]
		public IHttpActionResult SaveAccountingARNotes([FromBody] CustomerAccountingARNotes model, int cust_id)
		{
			if (model.customer_id != cust_id)
			{
				return NotFound();
			}
			var o = new BLL.Pages.Customers.CustomerAccountingSettings(CurrentUser, cust_id);
			return OkD(o.SaveARNotes(model));
		}

		[HttpGet]
		[Route("AccountingSettings/Rates/{cust_id}/{address_id}")]
		public IHttpActionResult GetAccountingRates(int cust_id, int address_id)
		{
			var o = new BLL.Pages.Customers.CustomerAccountingSettings(CurrentUser, cust_id);
			return Ok(o.GetRates(address_id));
		}

        [HttpGet]
        [Route("AccountingSettings/Rates/OverrideSetting/{buid}")]
        public IHttpActionResult GetBuOverrideSetting(int buid)
        {
            var o = new BLL.Pages.Customers.CustomerAccountingSettings(CurrentUser, 0);
            return Ok(o.GetOverriddeSetting(buid));
        }

        [HttpGet]
		[Route("AccountingSettings/Rates/{cust_id}/{address_id}/{bu_id}")]
		public IHttpActionResult GetAccountingRatesByBuid(int cust_id, int address_id, int bu_id)
		{
			var o = new BLL.Pages.Customers.CustomerAccountingSettings(CurrentUser, cust_id);
			return Ok(o.GetRatesList(bu_id, address_id));
		}

		[HttpGet]
		[Route("Contact/{cust_id}/{add_id}")]
		public IHttpActionResult GetContactProfile(int cust_id, int add_id)
		{
			var o = new BLL.Pages.Customers.CustomerContact(CurrentUser, cust_id, add_id);
			return Ok(o.Profile());
		}



		[HttpPost]
		[Route("Contact/{cust_id}/{add_id}")]
		public IHttpActionResult InsertContact([FromBody] DTO.ViewModels.Page.Customers.CustomerContact model, int cust_id, int add_id)
		{
			var o = new BLL.Pages.Customers.CustomerContact(CurrentUser, cust_id, add_id);
			return Ok(o.SaveContact(model));
		}

		[HttpDelete]
		[Route("Contact/{cust_id}/{add_id}/{contact_id}")]
		public IHttpActionResult DeleteContact(int cust_id, int add_id, int contact_id)
		{
			var o = new BLL.Pages.Customers.CustomerContact(CurrentUser, cust_id, add_id);
			return Ok(o.Delete(contact_id));
		}

		[HttpPost]
		[Route("Contact/IsDuplicate/{cust_id}/{add_id}/{contact_id}")]
		public IHttpActionResult IsDuplicate([FromBody] DTO.ViewModels.Core.DataIdString model, int cust_id, int add_id, int contact_id)
		{
			var o = new BLL.Pages.Customers.CustomerContact(CurrentUser, cust_id, add_id);

			switch (model.id)
			{
				case 1: // Is_DuplicateName 
					return OkD(o.Is_DuplicateName(model.value, contact_id));
				case 2: // Is_DuplicateEmail
					return OkD(o.Is_DuplicateEmail(model.value, contact_id));
				case 3: // Is_DuplicateCellPhone
					return OkD(o.Is_DuplicateCellPhone(model.value, contact_id));
				case 4: //Is_DuplicateDirectLine
					return OkD(o.Is_DuplicateDirectLine(model.value, contact_id));
				default:
					return OkD(false);
			}
		}

		[HttpGet]
		[Route("Contact/RandomPassword")]
		public IHttpActionResult RandomPassword()
		{
			return OkD(Toolbox.do_RandomString(8));
		}


		[HttpGet]
		[Route("Specialties/{cust_id}")]
		public IHttpActionResult GetSpecialtiesProfile(int cust_id)
		{
			var o = new BLL.Pages.Customers.CustomerSpecialties(CurrentUser, cust_id);
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("Specialties/{cust_id}")]
		public IHttpActionResult SaveSpecialties([FromBody] DTO.ViewModels.Core.DataIdString model, int cust_id)
		{
			var o = new BLL.Pages.Customers.CustomerSpecialties(CurrentUser, cust_id);
			return Ok(o.Save(model));
		}

		[HttpDelete]
		[Route("Specialties/{cust_id}/{skill_id}")]
		public IHttpActionResult DeleteSpecialties(int cust_id, int skill_id)
		{
			var o = new BLL.Pages.Customers.CustomerSpecialties(CurrentUser, cust_id);
			return Ok(o.Delete(skill_id));
		}

		[HttpPost]
		[Route("ChangeQC/{cust_id}")]
		public IHttpActionResult ChangeQC([FromBody] DTO.ViewModels.Core.DataInt model, int cust_id)
		{
			var o = new BLL.Pages.Customers.CustomerEdit(CurrentUser, cust_id);
			return Ok(o.ChangeQC(model.Data));
		}


		[HttpGet]
		[Route("PhoneNumber/{cust_id}/{add_id}")]
		public IHttpActionResult GetPhoneProfile(int cust_id, int add_id)
		{
			var o = new BLL.Pages.Customers.CustomerPhoneNumber(CurrentUser, cust_id, add_id);
			return Ok(o.Profile());
		}

		[HttpPost]
		[Route("PhoneNumber/{cust_id}/{add_id}")]
		public IHttpActionResult SavePhone([FromBody] DTO.ViewModels.Page.Customers.CustomerPhoneNumber model, int cust_id, int add_id)
		{
			var o = new BLL.Pages.Customers.CustomerPhoneNumber(CurrentUser, cust_id, add_id);
			return Ok(o.Save(model));
		}

		[HttpDelete]
		[Route("PhoneNumber/{cust_id}/{add_id}/{phone_id}")]
		public IHttpActionResult DeletePhone(int cust_id, int add_id, int phone_id)
		{
			var o = new BLL.Pages.Customers.CustomerPhoneNumber(CurrentUser, cust_id, add_id);
			return Ok(o.Delete(phone_id));
		}

	}
}
