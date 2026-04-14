using NESI.Common;
using NESI.Data.Entities;
using NESI.WebAPI.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NESI.WebAPI.Controllers.API.Page.Reports
{
    public class LayoutController : ApiControllerBase
    {

        private readonly BLL.Pages.Reports.Layout _layoutBuilder;

        public LayoutController()
        {
            _layoutBuilder = new BLL.Pages.Reports.Layout();
        }

        [Route("api/Layout/GetLayouts/{gridLayoutId}/{memberId}")]
        [HttpGet]
        public async Task<Response<NESI.DTO.ViewModels.Page.Reports.GridViewLayout>> GetLayouts(string gridLayoutId, string memberId)
        {
            try
            {
                int layoutMemberId;
                layoutMemberId = int.TryParse(memberId, out layoutMemberId) ? layoutMemberId : -999;
               
                QueryParam param = new QueryParam(Request.GetQueryNameValuePairs(), "id");
                var response = await _layoutBuilder.GetLayouts(gridLayoutId, layoutMemberId,CurrentEmployee ,param);
                return response;
            }
            catch (Exception)
            {
                
            }
            return new Response<DTO.ViewModels.Page.Reports.GridViewLayout>();
        }

        /// <summary>
        /// get all members related to layout
        /// </summary>
        /// <param name="gridLayoutId"></param>
        /// <returns></returns>
        [Route("api/Layout/Members/{gridLayoutId}")]
        [HttpGet]
        public object GetAllLayoutMembers(string gridLayoutId)
        {
            var lsLayouts = _layoutBuilder.GetAllLayoutMembers(gridLayoutId, CurrentEmployee);

            return lsLayouts;
        }

	    /// <summary>
	    /// get all layouts of particular member
	    /// </summary>
	    /// <param name="Id"></param>
	    /// <param name="layoutId"></param>
	    /// <returns></returns>
	    [Route("api/Layout/MemberLayout/{layoutId}")]
        [HttpGet]
        public async Task<List<NESI.DTO.ViewModels.Page.Reports.GridViewLayout>> GetMemberLayoutById(int layoutId)
        {
            var lsLayouts = await _layoutBuilder.GetMemberLayoutById(layoutId);

            return lsLayouts;
        }


        [Route("api/Layout/AddLayout")]
        [HttpPost]
        public async Task<int> AddLayout(NESI.DTO.ViewModels.Page.Reports.GridViewLayout layout)
        {


            return await _layoutBuilder.AddLayout(layout);

        }

        [Route("api/Layout/UpdateLayout")]
        [HttpPost]
        public async Task<bool> UpdateLayout(NESI.DTO.ViewModels.Page.Reports.GridViewLayout layout)
        {

            
            return await _layoutBuilder.UpdateLayout(layout);

        }

        [Route("api/Layout/UpdateLayout/Status/{gridLayoutId}/{memberId}")]
        [HttpPost]
        public async Task<bool> UpdateLayoutStatus(NESI.DTO.ViewModels.Page.Reports.GridViewLayout layout, string gridLayoutId, string memberId)
        {


            return await _layoutBuilder.UpdateLayoutStatus(layout, gridLayoutId, Convert.ToInt32(memberId), CurrentEmployee);

        }



        [Route("api/Layout/DeleteLayout/{Id}")]
        [HttpPost]
        public async Task<bool> DeleteLayout(string Id)
        {

            return await _layoutBuilder.DeleteLayout(Convert.ToInt32(Id));

        }





    }
}
