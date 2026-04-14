using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.WorkOrder
{
	public class WorkOrderBusinessUnitDetail : BLLGridBase<DTO.ViewModels.Page.WorkOrder.WorkOrderBusinessUnitDetail>
	{
		public WorkOrderBusinessUnitDetail()
		{

		}


		public WorkOrderBusinessUnitDetail(Employee user, BodyParams param, int buid) : base(user, param, new object[] { buid })
		{
			var can_view_margin = CurrentUser.AuthenticatedForPrivilege(81);
			var can_view_total_TM = CurrentUser.AuthenticatedForPrivilege(60);

			this.query = @"
SELECT
  a.woprog_id,
  a.business_unit_id,
  a.woprog_bvwo,
  a.business_unit_id,
  a.woprog_vis_to_cust,
  a.woprog_invoiceno,
  a.woprog_customer_id customer_id,
  a.woprog_customername,
  a.woprog_status status,
  a.woprog_custpo woprog_custpo,
  a.woprog_opendatetime open_dt,
  d.member_fullname cutby,"
+(can_view_total_TM? "a.woprog_stilltobebilled": "0 woprog_stilltobebilled") +
				@",
  IF(
    CAST(a.woprog_quoteid AS UNSIGNED) > 100000,
    LEFT(a.woprog_quoteid, 6),
    NULL
  ) quote_id,
  IF(
    CAST(a.woprog_quoteid AS UNSIGNED) > 100000,
    SUBSTRING(a.woprog_quoteid, 7, 2),
    NULL
  ) revision,
  a.woprog_description,
  e.member_fullname acct_manager,
  f.member_fullname pm,
  a.woprog_cutdatetime,
  g.Contact_Name as Contact_Name,
  h._date workorder_schedule_date,
  woprog_hold hold,
  woprog_ts last_modified,
"
	+ (can_view_margin ? " woprog_grossmargin margin" : "0 margin") + 
@"
FROM
  woprog a
  LEFT JOIN customer b
    ON a.woprog_customer_id = b.customer_id
    and a.business_unit_id = @v0
  LEFT JOIN customer_sales_properties csp
    ON csp.customer_id = a.woprog_customer_id
    AND csp.address_id = a.woprog_address_id
  LEFT JOIN business_unit c
    ON a.business_unit_id = c.id
    and a.business_unit_id = @v0
  LEFT JOIN member d
    ON a.woprog_cutby_memberid = d.member_id
    and a.business_unit_id = @v0
  LEFT JOIN member e
    ON csp.account_manager = e.member_id
  LEFT JOIN member f
    ON a.woprog_pm_memberid = f.member_id
    and a.business_unit_id = @v0
  LEFT JOIN contact g
    ON a.woprog_contact_id = g.contact_id
    and g.contact_type = 'Customer'
    and a.business_unit_id = @v0
  LEFT JOIN
    (SELECT
      MIN(hh.startdate) _date,
      hh.woprog_id
    FROM
      appointments hh
    WHERE hh.business_unit_id = @v0
      AND hh.startdate >= CURDATE()
    GROUP BY hh.woprog_id) h
    ON h.woprog_id = a.woprog_id
WHERE a.business_unit_id = @v0
  AND a.woprog_status != 'Deleted'
  and (
    a.WOProg_InvoiceDate > (curdate() - interval 5 year)
    or (a.woprog_status != 'Invoiced')
  )
ORDER BY a.woprog_id DESC
";
		}

	}
}
