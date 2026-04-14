using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerQuotesGrid : BLLGridBase<DTO.ViewModels.Page.Customers.CustomerQuotesGrid>
	{
		public CustomerQuotesGrid()
		{

		}

		public CustomerQuotesGrid(Employee user) : base(user)
		{

		}

		public CustomerQuotesGrid(Employee user, BodyParams param, int customer_id, int address_id) : base(user, param)
		{
			this.query_params = new object[] {customer_id, address_id };
			this.query = $@"
			SELECT 
CONCAT(a.quote_id,a.revision) id,
a.quote_id, 
a.revision,
MEMBER_NAME(a.quoted_by) quoted_by, 
DATE_FORMAT(a.open_date,'%Y-%m-%d') open_date,
URLDECODE(a.job_description) job_description,
b.status
  FROM quote_master a
LEFT JOIN quote_status b ON a.status_id = b.id
WHERE a.customer_id = @p0 AND a.quoted_by != 711 AND a.address_id = @p1 AND a.active_revision = true ORDER BY open_date DESC";
		}
	}
}