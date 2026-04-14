using System;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Common.Cache;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Core.Member
{
	public class Customer : Contact
	{
		public DTO.Models.Users.Customer CustomerProfile { get; }
		public int CustomerId { get; set; }

		public Customer(DTO.Models.Users.Contact con) : base(con)
		{
			CustomerProfile = AutoMapper.Mapper
				.Map<DTO.Models.Users.Customer>(Db.customer
				.FirstOrDefault(x => x.customer_id == con.Contact_Cust_ID));
			GetCustomerValue();
		}

		private void GetCustomerValue()
		{
			BvNumber = CustomerProfile.Customer_Number;
			Company = CustomerProfile.customer_name;
			CustomerId = CustomerProfile.customer_id;
			BusinessUnitId = CustomerProfile.business_unit_id.GetValueOrDefault();
			BusinessUnit = Global.BusinessUnit.GetValue(BusinessUnitId.ToString());
			BusniessUnitName = BusinessUnit.Name;
			VisibleBusinessUnits = BusinessUnitId.ToString();
			VisibleBusinessUnitIdList = new[] { BusinessUnitId.ToString() };
			VisibleBusinessUnitList = BLLBase.GetVisibileBusinessUnitDropDownLists2(VisibleBusinessUnitIdList);
		}
	}
}