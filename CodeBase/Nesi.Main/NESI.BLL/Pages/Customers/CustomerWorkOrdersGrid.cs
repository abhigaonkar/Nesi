using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerWorkOrdersGrid : BLLGridBase<DTO.ViewModels.Page.Customers.CustomerWorkOrdersGrid>
	{
		public CustomerWorkOrdersGrid()
		{

		}

		public CustomerWorkOrdersGrid(Employee user) : base(user)
		{

		}

		public CustomerWorkOrdersGrid(Employee user, BodyParams param, int customer_id, int address_id) : base(user, param)
		{
			this.query_params = new object[] { CurrentUser.VisibleBusinessUnits, customer_id, address_id };
			this.query = $@"
			SELECT
			a.woprog_id,
			a.WOProg_CutDateTime cut_dt,
				c.contact_name,
			a.WOProg_CloseDateTime close_dt,
				a.woprog_bvwo wo_number,
				a.woprog_description wo_description,
				a.woprog_status status,
				IF(a.WOProg_QuoteID = 0 OR a.woprog_quoteid < 100000, NULL, SUBSTRING(a.WOProg_QuoteID, 1, 6)) quote_id,
			IF(a.WOProg_QuoteID = 0 OR a.woprog_quoteid < 100000, NULL, SUBSTRING(a.WOprog_quoteid, 7, 3)) rev,
			a.business_unit_id,
			a.woprog_bvwo,
			CONCAT(b.Address_Addr1, ',', b.Address_City) addy
				FROM
			woprog a
			left join
			address b on b.address_id = a.woprog_address_id
			LEFT JOIN
			contact c on a.woprog_contact_id = c.contact_id
			WHERE
			find_in_set(a.business_unit_id, @p0) and
			a.woprog_customer_id = @p1 AND
			a.woprog_bvwo != '' and
				((@p2 != '' and @p2 != 0 and @p2 is not null and a.woprog_address_id = @p2)
			or (@p2 = 0) or (@p2 = '') or (@p2 is null))
			ORDER BY a.woprog_ID DESC";
		}
	}
}