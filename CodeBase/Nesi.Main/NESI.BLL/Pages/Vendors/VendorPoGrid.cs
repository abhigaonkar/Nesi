using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorPoGrid : BLLGridBase<DTO.ViewModels.Page.Vendors.VendorPoGrid>
	{
		public VendorPoGrid()
		{

		}

		public VendorPoGrid(Employee user) : base(user)
		{

		}

		public VendorPoGrid(Employee user, BodyParams param, int vendor_id) : base(user, param)
		{
			this.query_params = new object[] { vendor_id };
			this.query = $@"SELECT
  a.poprog_id,
  a.poprog_cutdate,
  a.poprog_closedDate,
  a.poprog_bvpo,
  a.poprog_order_description,
  b.status_type,
  a.business_unit_id,
  c.name business_unit_name
FROM
  poprog_header a
  LEFT JOIN poprog_status b
    ON a.poprog_status = b.poprog_status_id
  LEFT JOIN business_unit c
    ON a.business_unit_id = c.id
WHERE a.poprog_vendor_id = @p0
AND a.poprog_bvpo != ''
ORDER BY a.poprog_bvpo DESC";
		}

	}

}