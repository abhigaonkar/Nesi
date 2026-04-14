using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorHomeSearch : BLLGridBase<DTO.ViewModels.Page.Vendors.VendorHomeSearch>
	{
		public VendorHomeSearch()
		{

		}

		public VendorHomeSearch(Employee user) : base(user)
		{

		}

		public VendorHomeSearch(Employee user, BodyParams param, string criteria) : base(user, param, new object[] { criteria })
		{

			this.query = @"
SELECT
  vendor_id,
  vendor_number,
  vendor_name,
  IF(
    b.Address_PhoneFirst = '',
    '',
    CONCAT(
      b.Address_PhoneArea,
      b.Address_PhoneFirst,
      b.Address_PhoneLast
    )
  ) phone,
  CONCAT(
    b.Address_Addr1,
    ',',
    b.Address_Addr2,
    ',',
    b.Address_Addr3,
    ' ',
    b.Address_City,
    ',',
    b.Address_Prov
  ) AS vendor_address,
  b.Address_Web AS website
FROM
  vendor a
  LEFT JOIN address b
    ON b.Address_Table = 'Vendor'
    AND b.Address_Table_ID = a.Vendor_ID
WHERE vendor_active = TRUE and b.Active=true
ORDER BY vendor_name
		";
		}

	}
}