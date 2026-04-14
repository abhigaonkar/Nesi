using System;
using System.Threading.Tasks;
using System.Web.Http;
using NESI.Common;
using NESI.WebAPI.Controllers.Base;
using NESI.WebAPI.Infrastructures.Filters;
using bllT = NESI.BLL.Pages.Vendors.VendorNESIContactsGrid;
using dtoT = NESI.DTO.ViewModels.Page.Vendors.VendorNESIContactsGrid;

namespace NESI.WebAPI.Controllers.API.Page.Vendors
{
	[PageAuthorizationFilter(10)]
	[RoutePrefix("api/Page/VendorNESIContactsGrid")]
	public class VendorNESIContactsGridController : EmployeeGridControllerBase<dtoT, bllT>
	{
		protected int _vendor_id;

		public VendorNESIContactsGridController()
		{
			key = "contact_id";
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

	

		[HttpPost]
		[Route("Search")]
		public Response<dtoT> SearchAll([FromBody] BodyParams param)
		{
			return SearchResults(GetObject(param), param);
		}

		[Route("GetDistinct")]
		[HttpPost]
		public IHttpActionResult GetDistinct([FromBody] BodyParams param)
		{
			return Ok(base.GetDistinct(GetObject(param), param));
		}

		[HttpPost]
		[Route("Create")]
		public IHttpActionResult Create([FromBody] dtoT model)
		{
			var o = new bllT(CurrentEmployee);
			return Ok(o.Save(model));
		}

		[HttpPost]
		[Route("Delete/{id}")]
		public IHttpActionResult Delete(int id, [FromBody] dtoT model)
		{
			var o = new bllT(CurrentEmployee);
			return Ok(o.Delete(id));
		}
		[HttpPost]
		[Route("Edit")]
		public IHttpActionResult Edit([FromBody] dtoT model)
		{
			var o = new bllT(CurrentEmployee);
			return Ok(o.Save(model));
		}


		[HttpPost]
		[Route("statusList")]
		public IHttpActionResult GetRequestTypeList()
		{

			return Ok(new bllT().GetStatusList());
		}
	}
}
