using System.Linq;
using NESI.BLL.Base;
using NESI.DTO.Models.Core;

namespace NESI.BLL.Core
{
	public class Ne2Customer : BLLBase
	{
		public DTO.Models.Core.Customer Entity;
		public bool IsFound => Entity != null;

        public int NEbusiness_unit_id;

        public bool IsNECompany => NEbusiness_unit_id> 0;

		public Ne2Customer(int id)
		{
			Entity = AutoMapper.Mapper.Map<Customer>(
				_db.customer.FirstOrDefault(x => x.customer_id == id)
			);

            NEbusiness_unit_id = GetNewBusiness_unit_id(id)?? 0;
            
        }


        public int? GetNewBusiness_unit_id(int id)
        {
            int? result= bllToolbox.doSQL_int(@"select ifnull((Select business_unit_id from internal_companyno WHERE Internal_CompanyNo_Intranet_CustID =@v0 limit 1),0)", id);           
            return result;
        }
	}
}