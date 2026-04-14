using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common.Models;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Shared;
using NESI.DTO.ViewModels.Page.WorkOrder;

namespace NESI.BLL.Pages.WorkOrder
{
	public class WorkOrderBase : BLLBase
	{


		public bool can_view_margin { get; set; }
		public bool can_view_total_TM { get; set; }
		public bool can_cut_WO { get; set; }
		public OrderCompanySummary[] visible_businessUnit { get; set; }
		public int[] selected_businessUnits { get; set; }
		public int[] openned_tax_entities { get; set; }

		protected string custid;
		protected NeMember myMember;

		public WorkOrderBase(Employee user) : base(user)
		{
			custid = "%";
			myMember = new NeMember(UserId);
			if (myMember.customerID != 0)
			{
				custid = myMember.customerID.ToString();
			}
			can_view_margin = CurrentUser.AuthenticatedForPrivilege(81);
			can_view_total_TM = CurrentUser.AuthenticatedForPrivilege(60);
			can_cut_WO = CurrentUser.AuthenticatedForPrivilege(25);
		}

		public object Profile()
		{
			visible_businessUnit = GetCompanies();
			var profile = new NESI.BLL.Base.ProfileBase(CurrentUser);
			selected_businessUnits = profile.GetHomeLayout_selected_business_units("WorkOrder");
			openned_tax_entities = profile.GetHomeLayout_openned_tax_entities("WorkOrder");
			return this;
		}



		public OrderCompanySummary[] GetCompanies(string buids = "")
		{
			return bllToolbox.doSQL_List<OrderCompanySummary>(@"call wo_overview_business_units(@v0, @v1, @v2, @v3)", buids != "" ? buids : CurrentUser.VisibleBusinessUnits, CurrentUser.TaxEntityId, CurrentUser.BusinessUnitId, custid).ToArray();
		}


		public DataTable GetBusinessUnitSummaryDetail(int bu_id)
		{
			return bllToolbox.doSQL_dt(@"
SELECT
  a.woprog_id,
  a.woprog_bvwo,
  a.business_unit_id,
  a.woprog_vis_to_cust,
  a.woprog_invoiceno,
  a.woprog_customer_id customer_id,
  a.woprog_customername,
  a.woprog_status status,
  a.woprog_custpo woprog_custpo,
  a.woprog_opendatetime open_dt,
  d.member_fullname cutby,
  a.woprog_stilltobebilled,
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
  h._date,
  woprog_hold hold,
  woprog_ts last_modified,
  woprog_grossmargin margin
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
", bu_id);
		}

		public BusinessUnitSummary[] GetSummaryByBusinessUnit(int[] bu_ids)
		{
			var companies = GetCompanies(string.Join(",", bu_ids));
			if (companies.Length == 0) { return null; }
			var list = new List<BusinessUnitSummary>();
			for (var i = 0; i < bu_ids.Length; i++)
			{
				var bu_id = bu_ids[i];
				var company = companies[i];
				var summary = new BusinessUnitSummary();
				var bu = BLL.Common.Cache.Global.BusinessUnit.GetValue(bu_id);
				var te = BLL.Common.Cache.Global.TaxEntity.GetValue(bu.tax_entity_id);
				summary.tax_entity_id = te.id;
				var NewOpenPOs = @"
SELECT 
	DISTINCT(CAST(woprog_bvwo AS SIGNED)) 
FROM 
	poprog_header, 
	po_details_current, 
	woprog 
WHERE 
	poprog_status IN (1,2,5,3,9) AND 
	poprog_header.business_unit_id = @p0 AND 
	poprog_id = po_details_poprog_id AND 
	po_details_line_active = TRUE AND 
	po_details_woprog_id = woprog_id and
    po_details_current.is_gl_account = false
    and
	woprog_customer_id Like @p1";
				var NewOpenPOsTable = bllToolbox.doSQL_dt(NewOpenPOs, bu_id, custid);
				var po_list = "";

				if (NewOpenPOsTable.Rows.Count > 0)
				{
					po_list = (from DataRow _newpo in NewOpenPOsTable.Rows select _newpo[0].ToString()).Aggregate(po_list, (current, newpo) => current + (newpo + ",")).TrimEnd(',');
					summary.open_pos = bllToolbox.doSQL_Object<CountValue>(
						$@"SELECT COUNT(*) count, IFNULL(SUM(WOProg_StillToBeBilled), 0) value 
FROM WOProg WHERE business_unit_id = @v0  AND WOProg_CloseDateTime IS NULL AND WOProg_Status != 'Open' 
AND WOProg_Hold = 0 AND CAST(WOProg_BVWO as DECIMAL(10)) IN ({po_list})", bu_id);

				}

				if (summary.open_pos != null)
				{
					summary.open_pos.value = !can_view_total_TM ? 0 : summary.open_pos.value;
					summary.open_pos.name = "Open POs";
				}
				summary.total = new CountValue
				{
					name = "Total",
					count = company.total,
					value = !can_view_total_TM ? 0 : company.dollars
				};
				var company_path = company.path;

				string strJustScanned;
				if (!myMember.isContact)
				{
					try
					{
						if (Directory.Exists(company_path))
						{
							var dirJust = new DirectoryInfo(company_path);
							var arrFiles = dirJust.GetFiles("*.pdf");
							strJustScanned = arrFiles.Length.ToString();
						}
						else
						{
							strJustScanned = "N/A";
						}

					}
					catch (Exception ee)
					{
						strJustScanned = "N/A";
					}
				}
				else
				{
					strJustScanned = "";
				}
				summary.just_scanned = !can_view_total_TM ? "N/A" : strJustScanned;
				summary.businessUnit_id = bu_id;
				summary.businessUnit_name = bu.ddl_Name;
				summary.open = new CountValue() { name = OpsWOStatus.Open };
				summary.open_vendor_pos = new CountValue() { name = "Open PO's" };
				summary.bm_approval = new CountValue() { name = "BM Approval" };
				summary.pm_approval = new CountValue() { name = "PM Approval" };
				summary.to_be_invoiced = new CountValue() { name = "To Be Invoiced" };
				summary.questions = new CountValue() { name = "Question" };
				summary.on_hold = new CountValue() { name = "On Holld" };
				summary.rework = new CountValue() { name = "Rework" };
				summary.waiting_cut_po = new CountValue() { name = "Waiting Cust PO" };
				summary.init_prep = new CountValue() { name = "Init Prep" };
				summary.being_processed = new CountValue() { name = "Being Processed" };


				int c = 0;
				double v = 0.0;
				var dtTotals = bllToolbox.doSQL_List<CountValue>(@"SELECT COUNT(*) AS COUNT, IF(woprog_hold = 1, 'Hold',woprog_status) AS NAME, 
SUM(woprog_stilltobebilled) VALUE FROM woprog 
WHERE woprog.business_unit_id = @v0  
  AND  woprog_status NOT IN ('Deleted', 'Invoiced') GROUP BY NAME;", bu_id);
				/* Current statuses: 
				-- Not used -- 'Deleted',
				'Initial Prep',
				'Invoiced',
				'Open',
				'Questions For PM',
				'Rework',
				'Waiting BM Approval',
				'Waiting for Parts',
				'Waiting For PO',
				'Waiting PM Approval',
				'Waiting Parent BM Approval',
				'Waiting To Be Invoiced' 
				 */
				foreach (var t in dtTotals)
				{
					c = c + t.count;
					v = v + t.value;
					if (!can_view_total_TM)
					{
						v = 0;
						t.value = 0;
					}
					switch (t.name.ToLower())
					{
						case "open":
							summary.open = t;
							break;
						case "waiting bm approval":
							summary.bm_approval = t;
							break;
						case "waiting pm approval":
							summary.pm_approval = t;
							break;
						case "waiting to be invoiced":
							summary.to_be_invoiced = t;
							break;
						case "questions for pm":
							summary.questions = t;
							break;
						case "hold":
							summary.on_hold = t;
							break;
						case "rework":
							summary.rework = t;
							break;
						case "waiting for po":
							summary.waiting_cut_po = t;
							break;
						case "initial prep":
							summary.init_prep = t;
							break;
						case "in progress":
							summary.being_processed = t;
							break;
						default:
							break;
					}
				}

				summary.being_processed.count = c - summary.open.count - summary.on_hold.count;
				summary.being_processed.value = v - summary.open.value - summary.on_hold.value;
				list.Add(summary);
			}
			return list.ToArray();
		}
	}
}