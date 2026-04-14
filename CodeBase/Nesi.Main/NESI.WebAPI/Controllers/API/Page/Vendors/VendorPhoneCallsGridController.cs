using System;
using System.Threading.Tasks;
using System.Web.Http;
using NESI.Common;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using bllT = NESI.BLL.Pages.Vendors.VendorPhoneCallsGrid;
using dtoT = NESI.DTO.ViewModels.Page.Vendors.VendorPhoneCallsGrid;

namespace NESI.WebAPI.Controllers.API.Page.Vendors
{
	[PageAuthorizationFilter(10)]
	[RoutePrefix("api/Page/VendorPhoneCallsGrid")]
	public class VendorPhoneCallsGridController : EmployeeGridControllerBase<dtoT, bllT>
	{
		protected int _vendor_id;

		public VendorPhoneCallsGridController()
		{
			key = "phone_log_id";
		}

		protected override bllT GetObject(BodyParams param, params object[] extra_param)
		{
			param.keyColumn = key;
			if (extra_param != null && extra_param.Length == 1)
			{
				_vendor_id = Convert.ToInt32(extra_param[0]);
			}
			else if (param.queryparam.Length == 1)
			{
				_vendor_id = Convert.ToInt32(param.queryparam[0].value);
			}
			return new bllT(this.CurrentEmployee, param, _vendor_id) { SetColumnList = param.columns };
		}

		[Route("")]
		[HttpGet]
		public new Task<Report> GetSchema()
		{
			return base.GetSchema();
		}

		public void GetParams(BodyParams param)
		{
			if (param.queryparam.Length != 1) return;
			_vendor_id = Convert.ToInt32(param.queryparam[0].value);
		}

		[HttpPost]
		[Route("Search")]
		public Response<dtoT> SearchAll([FromBody] BodyParams param)
		{
			GetParams(param);
			return SearchResults(GetObject(param), param);
		}

		[Route("GetDistinct")]
		[HttpPost]
		public IHttpActionResult GetDistinct([FromBody] BodyParams param)
		{
			GetParams(param);
			return Ok(base.GetDistinct(GetObject(param), param));
		}

	}
}
