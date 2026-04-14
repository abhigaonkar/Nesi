using System;
using System.Web.Http;
using NESI.Common;
using NESI.DTO.ViewModels.Core;
using NESI.WebAPI.Infrastructures.Filters;
using NESI.WebAPI.Controllers.Base;
using bllT = NESI.BLL.Pages.CustomerRequestAdmin.CustomerRequestAdminGrid;
using dtoT = NESI.DTO.ViewModels.Page.Customers.CustomerRequestAdminGrid;
using System.Data;
using System.Linq;

namespace NESI.WebAPI.Controllers.API.Page.CustomerRequestAdmin
{
    [PageAuthorizationFilter(267)]
    [RoutePrefix("api/Page/CustomerRequestAdminGrid")]
    public class CustomerRequestAdminGridController : EmployeeGridControllerBase<dtoT, bllT>
    {
        public CustomerRequestAdminGridController()
        {
            key = "id";
            moduleName = "CustomerRequestAdminGrid";
        }
        
        protected override bllT GetObject(BodyParams param, params object[] extra_params)
        {
            param.keyColumn = key;
            var entity = param.queryparam.Length > 0 && (param.queryparam[1].coulumnname == "" || param.queryparam[1].coulumnname == null) ? param.queryparam[1].value.ToString() : "cust_national_account_mgr";
            //var entity = extra_params.Length > 0 ? extra_params[0]?.ToString() : "cust_national_account_mgr";
            return new bllT(this.CurrentEmployee, param, entity) { SetColumnList = param.columns };
        }
        [HttpGet]
        [Route("IndustrialTypes")]
        public IHttpActionResult GetIndustrialTypes()
        {
            DataTable dt = new bllT().GetIndustrialTypes(); // your working method

            // DataTable → List<dynamic>  (pure array)
            var list = dt.AsEnumerable()
                         .Select(r => new { id = r.Field<int>("id"), name = r.Field<string>("name") })
                         .ToList();

            return Ok(list);   // Angular now receives [ {...}, {...} ]
        }
        [Route("")]
        [HttpGet]
        public new Report GetSchema()
        {
            return base.GetSchema().Result;
        }

        [HttpPost]
        [Route("Search")]
        public Response<dtoT> SearchAll([FromBody] BodyParams param)
        {
            var entity = (param.queryparam?.Length > 0)
                         ? param.queryparam[0]?.value?.ToString()
                         : "cust_national_account_mgr";
            return SearchResults(GetObject(param, entity), param);
        }

        [HttpPost]
        [Route("Delete/{id}")]                                              
        public IHttpActionResult Delete(int id, [FromBody] LabelValueString entity)
        {
            var o = new bllT(CurrentEmployee, new BodyParams(), entity.Value);
            return OkD(o.Delete(id));
        }

        [HttpPost]
        [Route("Update/{id}")]
        public IHttpActionResult Update(int id, [FromBody] UpdateCustomerRequestModel model)
        {
            try
            {
                var o = new bllT(CurrentEmployee, new BodyParams(), model.Value);

                // Handle different update scenarios based on what fields are provided
                if (!string.IsNullOrEmpty(model.name))
                {
                    var result = o.Update("name", model.name, id);
                    if (result.StartsWith("Error"))
                        return BadRequest(result);
                }

                if (model.is_active.HasValue)
                {
                    var result = o.Update("is_active", (model.is_active ?? false) ? "1" : "0", id);
                    if (result.StartsWith("Error"))
                        return BadRequest(result);
                }
                if (model.industrial_type_id.HasValue && model.Value == "cus_end_market_segment")
                {
                    var result = o.Update("industrial_type_id", model.industrial_type_id.Value.ToString(), id);
                    if (result.StartsWith("Error"))
                        return BadRequest(result);
                }

                return Ok("Record updated successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create([FromBody] CreateCustomerRequestModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Model is required");

                if (string.IsNullOrEmpty(model.EntityName))
                    return BadRequest("EntityName is required");

                if (string.IsNullOrEmpty(model.Name))
                    return BadRequest("Name is required");

                if (model.EntityName == "cus_end_market_segment" && !model.IndustrialTypeId.HasValue)
                    return BadRequest("Industrial Type is required for End Market Segment");

                var o = new bllT(CurrentEmployee, new BodyParams(), model.EntityName);
                var result = o.Create(model.Name, model.IsActive, model.IndustrialTypeId);
                //var result = o.Create(model.Name, model.IsActive);

                if (result.StartsWith("Error"))
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetDistinct")]
        [HttpPost]
        public IHttpActionResult GetDistinct([FromBody] BodyParams param)
        {
            var entity = (param.queryparam?.Length > 0)
                         ? param.queryparam[0]?.value?.ToString()
                         : "cust_national_account_mgr";
            return Ok(base.GetDistinct(GetObject(param, entity), param));
        }
    }

    // Model for Create operation
    public class CreateCustomerRequestModel
    {
        public string EntityName { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public int? IndustrialTypeId { get; set; }  // ADD TH
    }

    // Model for Update operation
    public class UpdateCustomerRequestModel : LabelValueString
    {
        public string name { get; set; }
        public bool? is_active { get; set; }
        public int? industrial_type_id { get; set; }
    }
}