using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerInvoicingInstructionsGrid : BLLGridBase<DTO.ViewModels.Page.Customers.CustomerInvoicingInstructionsGrid>
	{
		public CustomerInvoicingInstructionsGrid()
		{

		}

		public CustomerInvoicingInstructionsGrid(Employee user) : base(user)
		{

		}

		public CustomerInvoicingInstructionsGrid(Employee user, BodyParams param, int customer_id) : base(user, param)
		{
			this.query_params = new object[] { customer_id };
			this.query = $@"SELECT
			  a.`ar_notes_id` AS `id`,
			  a.ar_notes_ts AS `Date`,
			  a.ar_notes_note AS `Note`,
			  c.member_fullname AS `By`,
			  b.woprog_invoiceno AS Invoice,
			  b.woprog_bvwo AS `wo`
			FROM
			  ar_notes a
			  INNER JOIN woprog b
				ON a.ar_notes_woprogid = b.woprog_id
			  LEFT JOIN member c
				ON a.ar_notes_memberid = c.member_id
			WHERE b.woprog_customer_id = @p0
			ORDER BY a.ar_notes_ts DESC";
		}
	}
}