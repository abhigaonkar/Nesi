using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorMarket : VendorEditBase
	{
		public VendorMarket(Employee user, int id) : base(user, id)
		{

		}

		public new object Profile()
		{

			return new
			{
				linecardList = bllToolbox.doSQL_Array<string>(@"SELECT DISTINCT
urldecode(inventory_attribute_value.`value`)
FROM
inventory_attribute_value
INNER JOIN inventory_item_detail ON inventory_item_detail.attribute_value_id = inventory_attribute_value.attribute_value_id
LEFT JOIN inventory_price ON inventory_item_detail.master_id = inventory_price.master_id
INNER JOIN vendor ON inventory_price.vendor_id = vendor.Vendor_ID AND vendor.Vendor_ID = @v0 
where inventory_attribute_value.attribute_id = 16", vendor_id),
				competitorList = bllToolbox.doSQL_Array<LabelValueInt>(@"
SELECT DISTINCT
  vendor.Vendor_Name label,
  vendor.Vendor_ID value
FROM
  inventory_price,
  vendor
WHERE inventory_price.master_id IN
  (SELECT DISTINCT
    inventory_price.master_id
  FROM
    inventory_price,
    inventory_item_master,
    inventory_tag
  WHERE inventory_price.vendor_id = @v0
    AND inventory_item_master.master_id = inventory_price.master_id
    AND inventory_item_master.tag_id = inventory_tag.tag_id
    AND inventory_tag.is_exclude = FALSE)
  AND inventory_price.business_unit_id = @v1
  AND inventory_price.vendor_id = vendor.vendor_id
  AND inventory_price.vendor_id != vendor.vendor_id", vendor_id, CurrentUser.Id)
			};
		}
	}
}