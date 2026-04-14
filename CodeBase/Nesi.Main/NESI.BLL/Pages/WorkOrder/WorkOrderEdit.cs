using System;
using System.IO;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.Common.Models;

namespace NESI.BLL.Pages.WorkOrder
{
	public class WorkOrderEdit : WorkOrderFileBase
	{
		protected string strID { get; set; }
		protected int woprog_id { get; set; }
		protected NeBusinessUnit wo_bu { get; set; }

		protected NeWOProg wo { get; set; }

		public bool can_access { get; set; }


		private const int priv_approve_pm = 15;   // PM Approval Level
		private const int priv_approve_bm = 16;   // BM Approval Level
		private const int priv_add_wo = 25;   // Add work orders to the system
		private const int priv_put_wo_on_hold = 55;   // Put a WO on hold
		private const int priv_view_cost = 58;   // View Cost Information (under edit existing WO)
		private const int priv_view_dollar_totals_all_branches = 59;   // View the dollar totals in all branches
		private const int priv_view_gross = 81;   // view gross margin percentage, this also allwos them to see sell prices, but not costs.
		private const int priv_create_credit_rebill_wo = 109;  // Cut Credit or Rebill WO's
		private const int priv_edit_2139_costs = 139;  // Edit the costs on 2139 lines
													   //private const int priv_set_45_days_credit = 172;  // Allow 45 days credit 
		private const int priv_approve_dm = 191;  // Department level approval to invoice
		private const int priv_skip_date_restriction = 193;  // Department level approval to invoice
		private const int priv_see_invoice_preview_button = 3;    // View the invoice preview button


		private const int page_view_customers = 10;   // Allows the user to see customers


		protected bool _can_approve_bm;

		protected bool _can_approve_pm;

		protected bool _can_approve_dm;

		protected bool _can_view_cost;

		protected bool _can_view_gross;

		protected bool _can_add_wo;

		protected bool _can_put_wo_on_hold;

		protected bool _can_view_dollar_totals_all_branches;

		protected bool _can_see_invoice_preview_button;

		protected bool _can_edit_2139_costs;

		protected bool _can_create_credit_rebill_wo;

		protected bool _can_view_customers;

		protected bool _same_dept;

		protected bool _skip_date_restriction;


		public WorkOrderEdit(Employee user, int buid, string strWo) : base(user)
		{
			business_unit_id = buid;
			strID = strWo;

			try
			{
				woprog_id = Convert.ToInt32(strWo);
				if (woprog_id < 20000 || woprog_id > 20000 && bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog  WHERE woprog_id =@v0", woprog_id) != 1)
				{
					can_access = false;
					return;
				}
				wo = new NeWOProg(woprog_id);
				wo_bu=new NeBusinessUnit(wo.business_unit_id);
				can_access = wo.business_unit_id == business_unit_id && CurrentUser.IsVisibleBusinessUnitId(business_unit_id);
			}
			catch (Exception)
			{
				can_access = false;
				return;
			}


			_can_approve_bm = CurrentUser.AuthenticatedForPrivilege(priv_approve_bm);
			_can_approve_dm = CurrentUser.AuthenticatedForPrivilege(priv_approve_dm);
			_can_approve_pm = CurrentUser.AuthenticatedForPrivilege(priv_approve_pm);
			_can_add_wo = CurrentUser.AuthenticatedForPrivilege(priv_add_wo);
			_can_view_cost = CurrentUser.AuthenticatedForPrivilege(priv_view_cost);
			_can_view_gross = CurrentUser.AuthenticatedForPrivilege(priv_view_gross);
			_can_put_wo_on_hold = CurrentUser.AuthenticatedForPrivilege(priv_put_wo_on_hold);
			_can_view_dollar_totals_all_branches = CurrentUser.AuthenticatedForPrivilege(priv_view_dollar_totals_all_branches);
			_can_see_invoice_preview_button = CurrentUser.AuthenticatedForPrivilege(priv_see_invoice_preview_button);
			_can_edit_2139_costs = CurrentUser.AuthenticatedForPrivilege(priv_edit_2139_costs);
			_can_create_credit_rebill_wo = CurrentUser.AuthenticatedForPrivilege(priv_create_credit_rebill_wo);
			_can_view_customers = CurrentUser.AuthorizePage(page_view_customers);
			_skip_date_restriction = CurrentUser.AuthenticatedForPrivilege(priv_skip_date_restriction);


		}

		public string GetPdf_Path()
		{
			var pdf_path = "";
			var file_name = "";
			if ((wo.woprog_scanned_date.Year > 2000 || wo.OrderNumber == "") && wo.Status != "Invoiced")
			{
				file_name = @"/" + strID + @".pdf";
				pdf_path = NeTaxEntity.BaseFolder(wo.business_unit_id, true) + @"/WOs" + file_name;
			}
			else
			{
				file_name = @"/Invoiced/" + strID + @".pdf";
				pdf_path = NeTaxEntity.BaseFolder(wo.business_unit_id, true) + @"/WOs";
			}
			var scanned_file = new Core.FileManager.WorkOrderScannedFile(woprog_id);
			file_name= scanned_file.GetPath(file_name.Replace(@"/", ""));
			if (wo.Status == OpsWOStatus.Open || !File.Exists(file_name))
			{
				pdf_path = @"/images/NoScan.pdf";
			}
			return pdf_path;
		}

	}
}