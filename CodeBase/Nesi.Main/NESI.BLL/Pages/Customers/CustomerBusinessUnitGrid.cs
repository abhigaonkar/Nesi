using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.Customers
{
    public class CustomerBusinessUnitGrid : BLLGridBase<DTO.ViewModels.Page.Customers.CustomerBusinessUnitGrid>
    {
        public CustomerBusinessUnitGrid()
        {

        }

        public CustomerBusinessUnitGrid(Employee user) : base(user)
        {

        }

        public CustomerBusinessUnitGrid(Employee user, BodyParams param, int customer_id) : base(user, param)
        {
            this.query_params = new object[] { customer_id };
            this.query = @"
			SELECT 
                   c.customer_id As cust_id,
                   bu.ddl_Name AS business_unit_name,
                   rb.netsuite_employee_name AS bdm
                        FROM customer_business_unit cbu
                        INNER JOIN customer c ON c.customer_id = cbu.customer_id
                        INNER JOIN business_unit bu ON bu.ID = cbu.business_unit_id
                        LEFT JOIN netsuite_sales_rep rb ON rb.netsuite_employee_internal_id = cbu.ram
                        WHERE c.customer_id = @p0 AND c.active = 1 AND bu.Active = 'T'
                        ORDER BY bu.ddl_Name ASC";

        }
    }
}