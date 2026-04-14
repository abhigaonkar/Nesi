using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using NESI.Data.Entities;

namespace NESI.WebAPI.Controllers.OData
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using NESI.Data.Entities;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<business_unit>("business_unit");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
	
    // ReSharper disable once InconsistentNaming
	public class Business_UnitController : ODataController
    {
        private readonly NESIMySQL _db = new NESIMySQL();

        // GET: odata/business_unit
        [EnableQuery]
        public IQueryable<business_unit> Getbusiness_unit()
        {
			return _db.business_unit;
        }

        // GET: odata/business_unit(5)
        [EnableQuery]
        public SingleResult<business_unit> Getbusiness_unit([FromODataUri] int key)
        {
            return SingleResult.Create(_db.business_unit.Where(businessUnit => businessUnit.ID == key));
        }

        // PUT: odata/business_unit(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<business_unit> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            business_unit businessUnit = await _db.business_unit.FindAsync(key);
            if (businessUnit == null)
            {
                return NotFound();
            }

            patch.Put(businessUnit);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!business_unitExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(businessUnit);
        }

        // POST: odata/business_unit
        public async Task<IHttpActionResult> Post(business_unit businessUnit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.business_unit.Add(businessUnit);
            await _db.SaveChangesAsync();

            return Created(businessUnit);
        }

        // PATCH: odata/business_unit(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<business_unit> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            business_unit businessUnit = await _db.business_unit.FindAsync(key);
            if (businessUnit == null)
            {
                return NotFound();
            }

            patch.Patch(businessUnit);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!business_unitExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(businessUnit);
        }

        // DELETE: odata/business_unit(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            business_unit businessUnit = await _db.business_unit.FindAsync(key);
            if (businessUnit == null)
            {
                return NotFound();
            }

            _db.business_unit.Remove(businessUnit);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

	    protected bool business_unitExists(int key) => _db.business_unit.Count(e => e.ID == key) > 0;
    }

}
