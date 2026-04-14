using System;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Common.Cache;
using NESI.Common.Exceptions;
using NESI.Common.Models;

namespace NESI.BLL.Core.Member
{
	public class Vendor : Contact
	{
		public DTO.Models.Users.Vendor VendorProfile { get; }
		public int VendorId { get; set; }
		public Vendor(DTO.Models.Users.Contact con) : base(con)
		{
			VendorProfile = AutoMapper.Mapper
				.Map<DTO.Models.Users.Vendor>(this.Db.vendor
				.FirstOrDefault(x=> x.Vendor_ID == con.Contact_Cust_ID));
			GetVenderValue();
		}

        /// <summary>
        /// Override to ensure not called for vendors
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="curDateTime"></param>
        /// <returns></returns>
	    public sealed  override NesiOperationResult SetPassword(string newPassword, DateTime curDateTime)=>
            throw new NesiValidationException("Operation not supported for vendors");


        private void GetVenderValue()
		{
			this.BvNumber = VendorProfile.vendor_number;
			this.Company = VendorProfile.Vendor_Name;
			this.VendorId = VendorProfile.Vendor_ID;
			this.BusinessUnitId = VendorProfile.business_unit_id;
			this.BusinessUnit = Global.BusinessUnit.GetValue(this.BusinessUnitId.ToString());
			this.BusniessUnitName = this.BusinessUnit.Name;
			this.VisibleBusinessUnits = BusinessUnitId.ToString();
			this.VisibleBusinessUnitIdList = new[] { BusinessUnitId.ToString() };
			this.VisibleBusinessUnitList = BLLBase.GetVisibileBusinessUnitDropDownLists2(this.VisibleBusinessUnitIdList);
		}
	}
}