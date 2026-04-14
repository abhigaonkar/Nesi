using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using DevExpress.Xpo;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using NESI.Common.Models;

namespace nesi.core
{
	/// <summary>
	/// Summary description for SalesOrder.
	/// </summary>
	public class NeWOProg
	{

		public bool woprog_isdownpayment { get; set; }
		public int woprog_iscredit { get; set; }
		public int woprog_isrebill { get; set; }
		public string woprog_whycredit { get; set; } = "";
		public double woprog_exp_labor { get; set; }
		public int woprog_assetid { get; set; }
		public int woprog_auto_invoice { get; set; }
		public string woprog_glposting_instructions { get; set; } = "";
		public int warranty { get; set; }
		public double benchmark_credit { get; set; }
		public double BenchmarkLabourSell { get; set; }
		public double BenchmarkMaterialSell { get; set; }
		public double TAX_MATERIAL { get; set; }
		public double TAX_LABOUR { get; set; }
		public double SATAX_MATERIAL { get; set; }
		public double SATAX_LABOUR { get; set; }
		public double TAX_MATADJUSTED { get; set; }
		public double TAX_LABADJUSTED { get; set; }
		public double TAX_QUOTEADJUSTED { get; set; }
		public double ThisWOLabourCost { get; set; }
		public double ThisWOMaterialCost { get; set; }
		public double ThisWOLabourBench { get; set; }
		public double ThisWOMaterialBench { get; set; }
		public int currency_id { get; set; } = 1;
		public int approved_by_parent_member_id { get; set; }
		public string default_material_sell_gl = "";
		public string default_labour_sell_gl = "";
		public bool inspection_required { get; set; }
		public string inspection_link { get; set; } = "";
		public bool IsIntercompany { get; set; }
		public bool labor_only { get; set; }
		public bool mat_only { get; set; }
		public bool sub_only { get; set; }
		public double fixed_labour_rate { get; set; }
		public double fixed_material_markup { get; set; }
		public bool use_fixed_labour_rate { get; set; }
		public bool use_fixed_material_markup { get; set; }
		public bool sustainability_project { get; set; }
		public double invoiced_currency_rate { get; set; }
		public int acting_ram { get; set; }
		public int bdm { get; set; }
		public double no_of_days_left { get; set; }
		public int no_of_guys_left { get; set; }
		public int woprog_tax1 { get; set; }
		public double woprog_invoice_tax { get; set; }
		public int woprog_onholdmemberid { get; set; }
		public int woprog_tax2 { get; set; }
		public int woprog_tax3 { get; set; }
		public int woprog_tax4 { get; set; }
		public int woprog_makeid { get; set; }
		public int woprog_modelid { get; set; }
		public int default_invoicetype { get; set; }
		public int woprog_warranty { get; set; }
		public double woprog_erquotedprice { get; set; }
		public string OrderNumber { get; set; }
		public string CustomerNumber { get; set; }
		public string CustomerName { get; set; } = "";
		public string woprog_whyhold { get; set; } = "";
		public string PONumber { get; set; }
		public DateTime OrderDate { get; set; }
		public string Status { get; set; } = "";
		public string Description { get; set; }
		public string strProjectManager { get; set; }
		public int intProjectManager { get; set; }
		public string ShipToAddress { get; set; }
		public int intCutByMember_ID { get; set; }
		public string strCutByMemberName { get; set; } = "";
		public bool HasProgressBills { get; set; }
		public string QuoteID { get; set; } = "";
		public int Quote_Id { get; set; }
		public int Quote_Revision { get; set; }
		public double Quote_Amount { get; set; }
		public string QuotedPrice { get; set; } = "";
		public string JobCost { get; set; } = "";
		public bool chkRD { get; set; }
		public bool chkServiceCall { get; set; }
		public string uncertainty { get; set; } = "";
		public string advancement { get; set; } = "";
		public string WOProg_CoreCompetency { get; set; } = "";
		public DateTime woprog_ts { get; set; }
		public int woprog_id { get; set; }
		public int business_unit_id { get; set; }
		public int revenue_line_id { get; set; }
		public DateTime woprog_scanned_date { get; set; }
		public DateTime woprog_CloseDateTime { get; set; }
		public string woprog_InvoiceNo { get; set; } = "";
		public DateTime woprog_InvoiceDate { get; set; }
		public int interbranch_woProg_id { get; set; }
		public int WOProg_Customer_ID { get; set; }
		public int woprog_Contact_ID { get; set; }
		public int woprog_Address_ID { get; set; }
		public DateTime woprog_Expected_StartDate { get; set; }
		public DateTime woprog_Expected_EndDate { get; set; }
		public double woprog_expected_sales_value { get; set; }
		public string woprog_Location_in_plant { get; set; } = "";
		public string special_instructions { get; set; } = "";
		public int woprog_ERID { get; set; }
		public double LabourTotalSell { get; set; }
		public double woprog_materialtotalSell { get; set; }
		public double woprog_timesheet_percentage { get; set; }
		public double woprog_hoursspentquoting { get; set; }
		public double woprog_labourcost { get; set; }
		public double woprog_materialcost { get; set; }
		public double TotalTimeAndMaterial { get; set; }
		public double woprog_grossmargin { get; set; }
		public int woprog_associate_woprog_id { get; set; }
		public bool IsJobCost { get; set; }
		public bool IsProgressBill { get; set; }
		public bool IsParent { get; set; }
		public bool IsChild { get; set; }
		public bool IsHistoric { get; set; }
		public bool IsTerminated { get; set; }
		public bool IsAdvancedStatus { get; set; }
		public bool IsTM { get; set; }
		public bool IsShell { get; set; }
		public bool AllowMobilePartManagement { get; set;}
		[MaxLength(50)]
		public string CustomerReferenceNumber { get; set; }
		public int woprog_sp_memberid { get; set; }
		public double ProgressBilled { get; set; }
		public double woprog_StillToBeBilled { get; set; }
		public int woprog_hold { get; set; }
		public int woprog_apply_discount { get; set; }
		public double net_total { get; private set; }
		public double tax_total { get; set; }
		public int woprog_vis_to_cust { get; set; }
		public int woprog_invoice_collection_status { get; set; }
		public string woprog_invoice_text_servicedates { get; set; } = "";
		public bool enableEditingScopeOnTimesheet { get; set; }
		public bool enable_prevailing_wages { get; set; }
		public int woprog_custpo_member_id { get; set; }
		public DateTime woprog_custpo_dt { get; set; }
		public bool woprog_verbal_quote { get; set; }
		public int parent_woprog_id { get; set; }
		public DateTime folders_last_checked { get; set; }
		public int woprog_dayscredit { get; set; } = 30;
		public int term_id { get; set; }
		public int woprog_creditcard_payment { get; set; }
		public int woprog_jobtag_id { get; set; }
		public bool IsCredit { get; set; }
		public bool IsRebill { get; set; }
		public bool IsQuoted { get; set; }
        public string close_reassign_reason { get; set; }
        public bool invoice_fb_issues { get; set; }

        public NeWOProg()
		{
		}
		public NeWOProg(int _business_unit_id, string OrderNumber)
		{
			var id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(woprog_id),0) FROM woprog WHERE business_unit_id= @v0  AND WOProg.Woprog_BVWO = @v1 ", new object[] { _business_unit_id, OrderNumber });
			if (id == 0)
			{
				throw new Exception("No Work Order Exists");
			}
			Load(id);
			}
		public NeWOProg(int woid)
		{
			Load(woid);
		}
		public void Load(int woid)
		{
			woprog_id = woid;
			using (var conn = Toolbox.connect())
			{
				var strsql = "SELECT woprog.*, member.Member_FirstName " +
							"FROM customer INNER JOIN ((woprog INNER JOIN member ON woprog.WOProg_PM_MemberID = member.Member_ID)) ON customer.Customer_ID = woprog.woprog_customer_ID " +
							"WHERE ((woprog.WOProg_ID=@v0))";
				var wo = Toolbox.doSQL_dt(conn, strsql, new object[] { woid });
				if (wo.Rows.Count != 1) return;

				var dr = wo.Rows[0];
				business_unit_id = Toolbox.ReturnZeroIfNull_int(dr["business_unit_id"]);
				if(business_unit_id == 0)
					{
					Toolbox.do_errorLog($"Work order {woid} has a business unit id of zero");
					return;
					}
				var businessUnitActive = Toolbox.doSQL_string(conn, @"SELECT active FROM business_unit WHERE id = @v0", new object[]{ business_unit_id }) == "T";
				if(!businessUnitActive)
					{
					business_unit_id = 0;
					Toolbox.do_errorLog($"Work order {woid} was loaded, though it's business unit is inactive.");
					return;
					}
				Description = dr["WOProg_Description"].ToString();
				chkRD = Toolbox.ReturnZeroIfNull_int(dr["WOProg_RDFlag"]) == 1;
				chkServiceCall = Toolbox.ReturnZeroIfNull_int(dr["WOProg_ServiceCall"]) == 1;
				CustomerName = dr["WOProg_CustomerName"].ToString();
				CustomerNumber = dr["WOProg_Customer_ID"].ToString();
				revenue_line_id = Toolbox.ReturnZeroIfNull_int(dr["revenue_line_id"]);
				intCutByMember_ID = Toolbox.ReturnZeroIfNull_int(dr["WOProg_CutBy_MemberID"]);
				woprog_invoice_collection_status = Toolbox.ReturnZeroIfNull_int(dr["woprog_invoice_collection_status"]);
				intProjectManager = Toolbox.ReturnZeroIfNull_int(dr["WOProg_PM_MemberID"]);
				OrderDate = Toolbox.ReturnBlankDateTimeIfNull(dr["WOProg_CutDateTime"]);
				net_total = Toolbox.ReturnZeroIfNull_double(dr["WOProg_InvoicedNetTotal"]);
				OrderNumber = dr["WOProg_BVWO"].ToString();
				PONumber = dr["WOProg_CustPO"].ToString();

				QuoteID = Toolbox.ReturnZeroIfNull_int(dr["WOProg_QuoteID"]).ToString();
				if(QuoteID != "0")
					{
					if(QuoteID.Length > 6)
						{ 
						quote.splice(QuoteID, out int Q_Id, out int Q_Revision);
						Quote_Id = Q_Id;
						Quote_Revision = Q_Revision;
						}
					else
						{
						Toolbox.do_errorLog($"Invalid quote id length for WO {woid} - Quote: {QuoteID}");
						}
					Quote_Amount = Toolbox.ReturnZeroIfNull_double(dr["WOProg_QuotedAmount"]);
					QuotedPrice = Quote_Amount.ToString();
					IsQuoted = true;
					}


				Status = dr["WOProg_Status"].ToString();
				uncertainty = dr["WOProg_Uncertainty"].ToString();
				advancement = dr["WOProg_Advancement"].ToString();
				WOProg_CoreCompetency = dr["WOProg_CoreCompetency"].ToString();
				WOProg_Customer_ID = Toolbox.ReturnZeroIfNull_int(dr["WOProg_Customer_ID"]);
				inspection_required = dr["inspection_required"] != DBNull.Value && Convert.ToBoolean(dr["inspection_required"]);
				inspection_link = Toolbox.ReturnBlankIfNull_string(dr["inspection_link"]);

				woprog_erquotedprice = Convert.ToDouble(dr["woprog_erquotedprice"]);
				woprog_makeid = Toolbox.ReturnZeroIfNull_int(dr["woprog_makeid"]);
				woprog_modelid = Toolbox.ReturnZeroIfNull_int(dr["woprog_modelid"]);
				woprog_warranty = Toolbox.ReturnZeroIfNull_int(dr["woprog_ERwarranty"]);
				woprog_creditcard_payment = Toolbox.ReturnZeroIfNull_int(dr["woprog_creditcard_payment"]);
				woprog_dayscredit = Toolbox.ReturnZeroIfNull_int(dr["woprog_dayscredit"]);
				woprog_vis_to_cust = Toolbox.ReturnZeroIfNull_int(dr["woprog_vis_to_cust"]);
				woprog_iscredit = Toolbox.ReturnZeroIfNull_int(dr["woprog_iscredit"]);
				woprog_isrebill = Toolbox.ReturnZeroIfNull_int(dr["woprog_isrebill"]);
				IsCredit = woprog_iscredit == 1;
				IsRebill = woprog_isrebill == 1;
				woprog_ts = Convert.ToDateTime(dr["woprog_ts"]);
				woprog_whycredit = dr["woprog_whycredit"].ToString();
				//	_woprog_invoice_collection_status	= Convert.ToInt16(drWOProg["woprog_invoice_collection_status"]);
				LabourTotalSell = Toolbox.ReturnZeroIfNull_double(dr["WOprog_LabourTotalSell"]);
				woprog_materialtotalSell = Toolbox.ReturnZeroIfNull_double(dr["WOProg_MaterialTotalSell"]);
				ProgressBilled = Toolbox.ReturnZeroIfNull_double(dr["WOProg_ProgressBilled"]);
				TotalTimeAndMaterial = Toolbox.ReturnZeroIfNull_double(dr["WOProg_TotalTandM"]);
				woprog_grossmargin = Toolbox.ReturnZeroIfNull_double(dr["WOProg_GrossMargin"]);
				woprog_StillToBeBilled = Toolbox.ReturnZeroIfNull_double(dr["WOProg_StillToBeBilled"]);
				woprog_timesheet_percentage = Toolbox.ReturnZeroIfNull_double(dr["WOProg_Timesheet_Percentage"]);
				woprog_hoursspentquoting = Toolbox.ReturnZeroIfNull_double(dr["woprog_hoursspentquoting"]);
				woprog_custpo_member_id = Toolbox.ReturnZeroIfNull_int(dr["woprog_custpo_member_id"]);
				woprog_materialcost = Toolbox.ReturnZeroIfNull_double(dr["woprog_materialcost"]);
				woprog_labourcost = Toolbox.ReturnZeroIfNull_double(dr["woprog_laborcost"]);
				woprog_custpo_dt = Toolbox.ReturnBlankDateTimeIfNull(dr["woprog_custpo_dt"]);
				woprog_verbal_quote = Toolbox.ReturnZeroIfNull_int(dr["woprog_verbal_quote"]) == 1;
				woprog_exp_labor = Toolbox.ReturnZeroIfNull_double(dr["woprog_exp_labor"]);
				woprog_assetid = Toolbox.ReturnZeroIfNull_int(dr["woprog_assetid"]);
				woprog_auto_invoice = Toolbox.ReturnZeroIfNull_int(dr["woprog_auto_invoice"]);
				default_invoicetype = Toolbox.ReturnZeroIfNull_int(dr["woprog_default_invoicetype"]);
				woprog_isdownpayment = Convert.ToBoolean(dr["woprog_isdownpayment"]);
				woprog_glposting_instructions = dr["woprog_glposting_instructions"].ToString();
				benchmark_credit = Convert.ToDouble(dr["benchmark_credit"]);
				BenchmarkLabourSell = Toolbox.ReturnZeroIfNull_double(dr["benchmark_labor_sell"]);
				BenchmarkMaterialSell = Toolbox.ReturnZeroIfNull_double(dr["benchmark_material_sell"]);
				currency_id = Toolbox.ReturnZeroIfNull_int(dr["currency_id"]);
				approved_by_parent_member_id = Toolbox.ReturnZeroIfNull_int(dr["approved_by_parent_member_id"]);
				woprog_ERID = Toolbox.ReturnZeroIfNull_int(dr["WOProg_ERID"]);
				woprog_Expected_EndDate = Toolbox.ReturnBlankDateTimeIfNull(dr["woprog_expected_enddate"]);
				woprog_Expected_StartDate = Toolbox.ReturnBlankDateTimeIfNull(dr["WOProg_Expected_StartDate"]);
				woprog_Contact_ID = Toolbox.ReturnZeroIfNull_int(dr["WOProg_Contact_ID"]);
				special_instructions = Toolbox.ReturnBlankIfNull_string(dr["WOProg_specialInstructions"]);
				woprog_whyhold = Toolbox.ReturnBlankIfNull_string(dr["woprog_whyhold"]);
				woprog_Location_in_plant = Toolbox.ReturnBlankIfNull_string(dr["WOProg_Location_In_plant"]);
				woprog_Address_ID = Toolbox.ReturnZeroIfNull_int(dr["WoProg_Address_ID"]);
				woprog_CloseDateTime = Toolbox.ReturnBlankDateTimeIfNull(dr["WOProg_CloseDateTime"]);
				woprog_onholdmemberid = Toolbox.ReturnZeroIfNull_int(dr["woprog_onholdmemberid"]);
				warranty = Toolbox.ReturnZeroIfNull_int(dr["warranty"]);
				woprog_InvoiceDate = Toolbox.ReturnBlankDateTimeIfNull(dr["WOProg_InvoiceDate"]);
				woprog_InvoiceNo = dr["WOProg_InvoiceNo"].ToString();
				interbranch_woProg_id = Toolbox.ReturnZeroIfNull_int(dr["WOProg_Interbranch_WOProg_ID"]);
				woprog_scanned_date = Toolbox.ReturnBlankDateTimeIfNull(dr["WOProg_OpenDateTime"]);
				woprog_associate_woprog_id = Toolbox.ReturnZeroIfNull_int(dr["WOProg_Associate_WOProg_ID"]);
				IsProgressBill = woprog_associate_woprog_id > 0;
				IsJobCost = woprog_associate_woprog_id == 0 && Quote_Id > 0;
				IsTM = woprog_associate_woprog_id == 0 && Quote_Id == 0;
				parent_woprog_id = Toolbox.ReturnZeroIfNull_int(dr["parent_woprog_id"]);
				IsParent = parent_woprog_id == 0;
				IsChild = parent_woprog_id > 0;
				IsAdvancedStatus = Toolbox.Contains(Status, new[] { OpsWOStatus.WaitingBMApproval, OpsWOStatus.WaitingPMApproval, OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced });
				IsHistoric = Toolbox.Contains(Status, new[] { OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced });
				IsTerminated = Toolbox.Contains(Status, new[] { OpsWOStatus.Deleted, OpsWOStatus.ClosedReassigned });				
				woprog_sp_memberid = Toolbox.ReturnZeroIfNull_int(dr["WOProg_SP_MemberID"]);
				woprog_expected_sales_value = Toolbox.ReturnZeroIfNull_double(dr["WOProg_Expected_Sales_Value"]);
				woprog_apply_discount = Convert.ToInt16(dr["WOProg_Apply_Discount"].ToString());
				default_labour_sell_gl = dr["default_labour_sell_gl"].ToString();
				default_material_sell_gl = dr["default_material_sell_gl"].ToString();
				fixed_labour_rate = Toolbox.ReturnZeroIfNull_double(dr["fixed_labour_rate"]);
				fixed_material_markup = Toolbox.ReturnZeroIfNull_double(dr["fixed_material_markup"]);
				sustainability_project = Toolbox.ReturnZeroIfNull_int(dr["sustainability_project"]) == 1;
				use_fixed_labour_rate = Toolbox.ReturnZeroIfNull_int(dr["use_fixed_labour_rate"]) == 1;
				use_fixed_material_markup = Toolbox.ReturnZeroIfNull_int(dr["use_fixed_material_markup"]) == 1;
				invoiced_currency_rate = Toolbox.ReturnZeroIfNull_double(dr["invoiced_currency_rate"]);
				acting_ram = Toolbox.ReturnZeroIfNull_int(dr["acting_ram"]);
				bdm = Toolbox.ReturnZeroIfNull_int(dr["bdm"]);
				woprog_jobtag_id = Toolbox.ReturnZeroIfNull_int(dr["woprog_jobtag_id"]);
				no_of_guys_left = Toolbox.ReturnZeroIfNull_int(dr["no_of_guys_left"]);
				no_of_days_left = Toolbox.ReturnZeroIfNull_double(dr["no_of_days_left"]);
				folders_last_checked = Toolbox.ReturnBlankDateTimeIfNull(dr["folders_last_checked"]);
				enableEditingScopeOnTimesheet = Convert.ToBoolean(dr["enableEditingScopeOnTimesheet"]);
				enable_prevailing_wages = Convert.ToBoolean(dr["enable_prevailing_wages"]);
				term_id = Toolbox.ReturnZeroIfNull_int(dr["term_id"]);
				woprog_invoice_tax = Toolbox.ReturnZeroIfNull_double(dr["woprog_invoice_tax"]);
				close_reassign_reason = Toolbox.ReturnBlankIfNull_string(dr["close_reassign_reason"]);
				CustomerReferenceNumber = Toolbox.ReturnBlankIfNull_string(dr["customer_ref"]);
				IsShell = Toolbox.ReturnZeroIfNull_int(dr["is_shell_wo"]) == 1;
				// woprog_billing_email= Toolbox.ReturnBlankIfNull_string(dr["billing_email"]);
				//woprog_controller_id = (int?)dr["controller_id"];
				AllowMobilePartManagement = Toolbox.ReturnZeroIfNull_int(dr["allow_mobile_part_management"]) == 1;

				var _taxes = Toolbox.doSQL_dt(conn, @"SELECT address_tax1 tax1, address_taxex1 taxex1, address_tax2 tax2, address_taxex2 taxex2,address_tax3 tax3, address_taxex3 taxex3,address_tax4 tax4, address_taxex4 taxex4 FROM address where address_table_id = @v0  AND address_table = 'Customer' and address_type = 'B' LIMIT 1", new object[] { WOProg_Customer_ID });
				var _tax_ids = new List<string> { "", "", "", "" };
				var _tax_exs = new List<string> { "", "", "", "" };
				if (_taxes.Rows.Count > 0)
				{
					var taxes_dr = _taxes.Rows[0];
					_tax_ids[0] = taxes_dr["tax1"].ToString();
					_tax_ids[1] = taxes_dr["tax2"].ToString();
					_tax_ids[2] = taxes_dr["tax3"].ToString();
					_tax_ids[3] = taxes_dr["tax4"].ToString();
					_tax_exs[0] = taxes_dr["taxex1"].ToString();
					_tax_exs[1] = taxes_dr["taxex2"].ToString();
					_tax_exs[2] = taxes_dr["taxex3"].ToString();
					_tax_exs[3] = taxes_dr["taxex4"].ToString();
				}
				//_woprog_id = Convert.ToInt16(drWOProg["WOProg_QuotedPrice_Type"]);
				woprog_id = Convert.ToInt32(dr["WOProg_ID"]);
				woprog_tax1 = Convert.ToInt32(dr["WOProg_tax1"]);
				woprog_tax2 = Convert.ToInt32(dr["WOProg_tax2"]);
				woprog_tax3 = Convert.ToInt32(dr["WOProg_tax3"]);
				woprog_tax4 = Convert.ToInt32(dr["WOProg_tax4"]);
				woprog_hold = Convert.ToInt32(dr["WOProg_hold"]);
				try
				{
					woprog_invoice_text_servicedates = Toolbox.do_value_from(Toolbox.doSQL_string(conn, @"Select ifnull(MAX(woprog_invoice_text_servicedates),'') from woprog_invoice_text  where woprog_invoice_text_woprog_id =@v0", new object[] { woid }), false);
				}
				catch
				{
					woprog_invoice_text_servicedates = "";
				}
				IsIntercompany = parent_woprog_id > 0;
				HasProgressBills = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM woprog WHERE woprog_associate_woprog_id = @v0 AND woprog_status != 'Deleted'", new object[]{woid}) > 0;

				var table_ = Status == OpsWOStatus.WaitingToBeInvoiced || Status == OpsWOStatus.Invoiced ? "history" : "current";
				tax_total = 0;
				var _tax_codes = "";
				_tax_codes += _tax_exs[0] == "" ? _tax_ids[0] + "," : "";
				_tax_codes += _tax_exs[1] == "" ? _tax_ids[1] + "," : "";
				_tax_codes += _tax_exs[2] == "" ? _tax_ids[2] + "," : "";
				_tax_codes += _tax_exs[3] == "" ? _tax_ids[3] + "," : "";
				_tax_codes = _tax_codes.Length > 0 ? _tax_codes.TrimEnd(',') : "";
				strCutByMemberName = intCutByMember_ID == 0 ? "Unknown" : Toolbox.doSQL_string(conn, @"SELECT IFNULL(MAX(member_fullname), 'Unknown') from member where member_ID = @v0  LIMIT 1", new object[] { intCutByMember_ID });
				strProjectManager = intProjectManager == 0 ? "Unknown" : Toolbox.doSQL_string(conn, @"SELECT IFNULL(MAX(member_fullname), 'Unknown') from member where member_ID = @v0  LIMIT 1", new object[] { intProjectManager });
				if (DateTime.Now.Subtract(folders_last_checked).TotalDays > 1) // Adding a day lag to check the work order folder structure.
				{
					var path = new NeFiles().GetProjectFolder(woid);
					NeFiles.chk_wo_folders(path);
					Toolbox.doSQL_void(conn, @"UPDATE woprog SET folders_last_checked = NOW(), woprog_ts = woprog_ts WHERE woprog_id = @v0", new object[] { woid });
				}
				labor_only = Convert.ToBoolean(dr["labor_only"]);
				mat_only = Convert.ToBoolean(dr["mat_only"]);
				sub_only = Convert.ToBoolean(dr["sub_only"]);
				invoice_fb_issues = Convert.ToBoolean(dr["invoice_fb_issues"]);


			}
		}
		/// <summary>
		/// Class object for holding progress bill properties from the description of a WO
		/// </summary>
		public class ProgressProperties
		{
			public bool IsProgress { get; set; }
			public bool IsFixed { get; set; }
			public bool IsDownPayment { get; set; }
			public bool IsPercent { get; set; }
			public double FlatAmount { get; set; }
			public double PercentAmount { get; set; }
		}
		public static bool CheckForPdf(object id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE woprog_id= @v0 AND (netsuite_creditmemo_pdf IS NOT NULL OR netsuite_invoice_pdf IS NOT NULL) ", new object[] { id })==1;
		}
		public static byte[] GeneratePDF(object id, bool is_credit)
		{
			return Toolbox.doSQL_BLOB(@"SELECT IFNULL(netsuite_invoice_pdf,netsuite_creditmemo_pdf) FROM woprog WHERE woprog_id=@v0 AND woprog_iscredit=@v1 ", new object[] {id,is_credit }, false);         
		}
		/// <summary>
		/// Clear past statuses for a work order & reopen a work order
		/// </summary>
		/// <param name="_wo"></param>
		public static void reopen_workorder(NeWOProg _wo)
		{
			if (Toolbox.Contains(_wo.Status, new[] { OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced })) throw new Exception("Cannot reopen a WO in Waiting to be Invoiced or Invoiced statuses");
			//var businessUnit = new NeBusinessUnit(_wo.business_unit_id);
			//if (businessUnit.DSN != "")
			//{
			//	using (var bv_conn = BVDB.connect(businessUnit.DSN))
			//	{
			//		BVDB.doSQL_void(bv_conn, @"UPDATE sales_order_header SET status='O'  WHERE number=?", new object[] { _wo.OrderNumber });
			//	}
			//}
			_wo.Status = OpsWOStatus.Open;
			_wo.Save();
			Toolbox.doSQL_void(@"DELETE FROM woprogstatus  WHERE woprogstatus_woprog_id=@v0", new object[] { _wo.woprog_id });
		}
		public static void add_history(int woprog_id, int member_id, string status, string dt)
		{
			Toolbox.doSQL_void(@" INSERT INTO woprogstatus ( WOProgStatus_WOProg_ID, WOProgStatus_Member_ID, WOProgStatus_Status, WOProgStatus_DateTime ) VALUES ( @v0 , @v1 , @v2 , @v3  )", new object[] { woprog_id, member_id, status, dt });
		}
		public void Save()
		{
			// Sorted lines by name
			var sql = @"
UPDATE 
	woprog
SET
	woprog_address_id					= @v18,
	woprog_advancement					= @v11,
	woprog_apply_discount				= @v53,
	woprog_assetid						= @v50,
	woprog_associate_woprog_id			= @v26,
	woprog_auto_invoice					= @v52,
	woprog_bvwo							= @v1,
	business_unit_id					= @v0,
	woprog_contact_id					= @v17,
	woprog_corecompetency				= @v12,
	woprog_creditcard_payment			= @v40,
	woprog_customer_id					= @v9,
	woprog_customername					= @v2,
	woprog_custpo						= @v6,
	woprog_custpo_dt					= @v7,
	woprog_custpo_member_id				= @v8,
	woprog_cutby_memberid				= @v15,
	woprog_dayscredit					= @v35,
	woprog_default_invoicetype			= @v51,
	woprog_description					= @v3,
	woprog_erid							= @v32,
	woprog_erquotedprice				= @v36,
	woprog_erwarranty					= @v37,
	woprog_exp_labor					= @v49,
	woprog_expected_enddate				= @v21,
	woprog_expected_sales_value			= @v38,
	woprog_expected_startdate			= @v19,
	woprog_hold							= @v39,
	woprog_hoursspentquoting			= @v25,
	woprog_invoice_collection_status	= @v46,
	woprog_iscredit						= @v43,
	woprog_isdownpayment				= @v54,
	woprog_isrebill						= @v44,
	woprog_location_in_plant			= @v20,
	woprog_makeid						= @v33,
	woprog_modelid						= @v34,
	woprog_onholdmemberid				= @v42,
	woprog_opendatetime					= @v58,
	woprog_pm_memberid					= @v5,
	woprog_quotedamount					= @v24,
	woprog_quoteid						= @v4,
	woprog_rdflag						= @v13,
	woprog_servicecall					= @v14,
	woprog_sp_memberid					= @v27,
	woprog_specialinstructions			= @v22,
	woprog_status						= @v56,
	woprog_tax1							= @v28,
	woprog_tax2							= @v29,
	woprog_tax3							= @v30,
	woprog_tax4							= @v31,
	woprog_timesheet_percentage			= @v57,
	woprog_uncertainty					= @v10,
	woprog_verbal_quote					= @v48,
	woprog_vis_to_cust					= @v41,
	warranty							= @v55,
	woprog_whycredit					= @v45,
	woprog_whyhold						= @v23,
	currency_id							= @v59,
	approved_by_parent_member_id		= @v60,
	default_labour_sell_gl				= @v61,
	default_material_sell_gl			= @v62,
	parent_woprog_id					= @v63,
	inspection_link                     = @v64,
	inspection_required                 = @v65,
	labor_only                          = @v66,
	mat_only                            = @v67,
	sub_only                            = @v68,
	fixed_labour_rate                   = @v69,
	fixed_material_markup               = @v70,
	sustainability_project              = @v71,
	use_fixed_labour_rate               = @v72,
	use_fixed_material_markup           = @v73,
	invoiced_currency_rate              = @v74,
	acting_ram                          = @v75,
	woprog_jobtag_id                    = @v76,
	no_of_guys_left                     = @v77,
	no_of_days_left                     = @v78,
	revenue_line_id                     = @v79,
	term_id                             = @v80,
    close_reassign_reason               = @v81,
	is_shell_wo							= @v82,
	customer_ref						= @v83,
	allow_mobile_part_management		= @v84,
    bdm									= @v85,
	enable_prevailing_wages				= @v86,
    invoice_fb_issues   				= @v87
WHERE 
	woprog_id = @v47";

			try
			{
				Toolbox.doSQL_void(sql, new object[] {
					business_unit_id,												                                                                    // 0
					OrderNumber,													                                                                    // 1
					CustomerName.Replace("'",""),									                                                                    // 2
					Description,								                                                                                        // 3
					QuoteID,														                                                                    // 4
					intProjectManager,												                                                                    // 5
					PONumber,														                                                                    // 6
					Toolbox.MySQL_longdt(woprog_custpo_dt)==""? "NULL" :  Toolbox.MySQL_longdt(woprog_custpo_dt).ToString() ,							// 7
					woprog_custpo_member_id,										                                                                    // 8
					WOProg_Customer_ID,											                                                                        // 9
					uncertainty,													                                                                    // 10
					advancement,													                                                                    // 11
					WOProg_CoreCompetency,											                                                                    // 12
					chkRD,															                                                                    // 13
					chkServiceCall,												                                                                        // 14
					intCutByMember_ID,												                                                                    // 15
					0,												                                                                                    // 16
					woprog_Contact_ID,												                                                                    // 17
					woprog_Address_ID,												                                                                    // 18
					Toolbox.MySQL_shortdt(woprog_Expected_StartDate)=="" ? "NULL" :Toolbox.MySQL_shortdt(woprog_Expected_StartDate),				    // 19
					woprog_Location_in_plant,										                                                                    // 20
					Toolbox.MySQL_shortdt(woprog_Expected_EndDate)=="" ? "NULL" :Toolbox.MySQL_shortdt(woprog_Expected_EndDate),				        // 21
					special_instructions,						                                                                                        // 22
					woprog_whyhold,												                                                                        // 23
					QuotedPrice=="" ? "0": QuotedPrice,													                                                // 24
					woprog_hoursspentquoting,										                                                                    // 25
					woprog_associate_woprog_id,									                                                                        // 26
					woprog_sp_memberid,											                                                                        // 27
					woprog_tax1,													                                                                    // 28
					woprog_tax2,													                                                                    // 29
					woprog_tax3,													                                                                    // 30
					woprog_tax4,													                                                                    // 31
					woprog_ERID,													                                                                    // 32
					woprog_makeid,													                                                                    // 33
					woprog_modelid,												                                                                        // 34
					woprog_dayscredit,												                                                                    // 35
					woprog_erquotedprice,											                                                                    // 36
					woprog_warranty,												                                                                    // 37
					woprog_expected_sales_value,									                                                                    // 38
					woprog_hold,													                                                                    // 39
					woprog_creditcard_payment,										                                                                    // 40
					woprog_vis_to_cust,											                                                                        // 41
					woprog_onholdmemberid,											                                                                    // 42
					woprog_iscredit,												                                                                    // 43
					woprog_isrebill,												                                                                    // 44
					woprog_whycredit,												                                                                    // 45
					woprog_invoice_collection_status,								                                                                    // 46
					woprog_id,														                                                                    // 47
					woprog_verbal_quote,											                                                                    // 48
					woprog_exp_labor,												                                                                    // 49	
					woprog_assetid,												                                                                        // 50
					default_invoicetype,											                                                                    // 51
					woprog_auto_invoice,											                                                                    // 52
					woprog_apply_discount,											                                                                    // 53
					woprog_isdownpayment,											                                                                    // 54
					warranty,														                                                                    // 55
					Status==""?"null":Status,															                                                // 56
					Toolbox.ReturnZeroIfNull_double(woprog_timesheet_percentage),	                                                                    // 57
					woprog_scanned_date.Year <= 2005 ? "NULL" : Toolbox.MySQL_longdt(woprog_scanned_date),                                              // 58
					currency_id,                                                                                                                        // 59
					approved_by_parent_member_id,                                                                                                       // 60
					default_labour_sell_gl,                                                                                                             // 61
					default_material_sell_gl,                                                                                                           // 62
					parent_woprog_id,                                                                                                                   // 63
					inspection_link,                                                                                                                    // 64
					inspection_required,                                                                                                                // 65
					labor_only,                                                                                                                         // 66
					mat_only,                                                                                                                           // 67
					sub_only,                                                                                                                           // 68
					fixed_labour_rate,                                                                                                                  // 69
					fixed_material_markup,                                                                                                              // 70
					sustainability_project,                                                                                                             // 71
					use_fixed_labour_rate,                                                                                                              // 72
					use_fixed_material_markup,                                                                                                          // 73
					invoiced_currency_rate,                                                                                                             // 74
					acting_ram,                                                                                                                         // 75
					woprog_jobtag_id,                                                                                                                   // 76
					no_of_guys_left,                                                                                                                    // 77
					no_of_days_left,                                                                                                                    // 78
					revenue_line_id,                                                                                                                    // 79
					term_id,                                                                                                                            // 80
                    close_reassign_reason,                                                                                                              // 81
					IsShell,																															// 82
					CustomerReferenceNumber,																											// 83
					AllowMobilePartManagement,																											// 84
					bdm,                                                                                                                                 // 85
					enable_prevailing_wages,																											 // 86
				    invoice_fb_issues,
				});
			}
			catch (Exception ee)
			{
				Toolbox.do_errorLog_errorStack(ee);
				throw;
			}
		}
		public void SaveWorkOrder()
		{
			Save();
		}

		public void fast_update_header_totals()
		{
			using(var conn = Toolbox.connect())
			{ 
			//  NeWOProg.update_header_totals(_woprog_id.ToString(), _business_unit_id, _OrderNumber);
			// return;
			var dblQuantity = 0.00;
			var dblCost = 0.00;
			var dblPrice = 0.00;
			double laborcost = 0;
			double materialcost = 0;
			double laborprice = 0;
			double materialprice = 0;
			double bench_material = 0;
			double bench_labor = 0;
			double dblCostMultiplier = 1;
			var code = "";
			var SQL = "";
			double invisiblecredit = 0;
			double progressbillings = 0;
			double progressbillings_2 = 0;
			double visiblecredit = 0;
			double donotinclude = 0;
			double BalanceForward = 0;
			double visiblenocharge = 0;
			double regular = 0;
			double jobcostforquote = 0;
			double quoted = 0;
			double quoted_2 = 0;
			double grossmarginlaborcost = 0;
			double grossmarginmaterialcost = 0;
			var intPlacement = 0;
			var intPlacement2 = 0;
			var tablename = Status == OpsWOStatus.Invoiced || Status == OpsWOStatus.WaitingToBeInvoiced ? "wo_detail_history" : "wo_detail_current";
			double discount = 0;
			double tax_material_this = 0;
			double tax_material_Adjust = 0;
			double tax_labour_this = 0;
			double tax_labour_adjust = 0;
			double tax_quoted_adjust = 0;
			double satax_material = 0;
			double satax_labour = 0;

			var strProgBillSel = "SELECT IFNULL(SUM(WOProg_InvoicedNetTotal), 0) AS PBSUM FROM woprog WHERE WOProg_Associate_WOProg_ID = @v0";
			try
			{
				progressbillings = Toolbox.doSQL_double(conn, strProgBillSel, new object[] { woprog_id });
			}
			catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
			var PayCost = Toolbox.doSQL_dt(conn, @"SELECT a.Member_ID, a.Member_Inv_BVNumber, b.WAGE, b.PAYTYPE FROM member AS a,currentwage AS b  WHERE a.Member_ID = b.Member_ID AND a.Member_Status='Active'", null);
			try
			{
				var details = Toolbox.doSQL_dt(conn, string.Format(@"SELECT * FROM {0} WHERE {0}_woprog_id = '{1}' AND {0}_billtypeid!=4 and {0}_billtypeid!=6 and {0}_billtypeid!=8 and {0}_billtypeid!=9", tablename, woprog_id), null);
				foreach (DataRow dr in details.Rows)
				{
					discount = Convert.ToDouble(dr[tablename + "_discount"]);
					discount = 1 - discount / 100;
					dblCostMultiplier = 1.25;
					code = dr[tablename + "_code"].ToString().ToUpper();
					double.TryParse(dr[tablename + "_price_cost"].ToString(), out dblCost);
					double.TryParse(dr[tablename + "_price_sell"].ToString(), out dblPrice);
					double.TryParse(dr[tablename + "_qty_committed"].ToString(), out dblQuantity);
					var linetype = dr[tablename + "_type"].ToString();
					var line_cost = Convert.ToDouble(dr[tablename + "_price_cost"]);
					var billtype_id = Convert.ToInt32(dr[tablename + "_billtypeid"]);
					var extd_cost = Math.Round(dblCost * dblQuantity, 4, MidpointRounding.AwayFromZero);
					var extd_sell = Math.Round(dblPrice * dblQuantity, 3, MidpointRounding.AwayFromZero);
					var extd_sell_disc = Math.Round(dblPrice * dblQuantity * discount, 3, MidpointRounding.AwayFromZero);
					#region Labour Rows
					if (code.StartsWith("LB") && billtype_id != 2 && billtype_id != 10)
					{
						intPlacement = code.IndexOf("DT");
						intPlacement2 = code.LastIndexOf("DT");
						if (intPlacement != -1)
						{
							if (intPlacement == intPlacement2 && intPlacement > 3)
							{
								code = code.Replace("DT", "");
								dblCostMultiplier += 1;
							}
							else if (intPlacement != intPlacement2 && intPlacement2 > 3)
							{
								code = code.Remove(intPlacement2, 2);
								dblCostMultiplier += 1;
							}
						}
						intPlacement = code.IndexOf("OT");
						intPlacement2 = code.LastIndexOf("OT");
						if (intPlacement != -1)
						{
							if (intPlacement == intPlacement2 && intPlacement > 3)
							{
								code = code.Replace("OT", "");
								dblCostMultiplier += .5;
							}
							else if (intPlacement != intPlacement2 && intPlacement2 > 3)
							{
								code = code.Remove(intPlacement2, 2);
								dblCostMultiplier += .5;
							}
						}
						intPlacement = code.IndexOf("SP");
						intPlacement2 = code.LastIndexOf("SP");
						if (intPlacement != -1)
						{
							if (intPlacement == intPlacement2 && intPlacement > 3)
							{
								code = code.Replace("SP", "");
								dblCostMultiplier += .1;
							}
							else if (intPlacement != intPlacement2 && intPlacement2 > 3)
							{
								code = code.Remove(intPlacement2, 2);
								dblCostMultiplier += .1;
							}
						}
						code = code.Trim();
						foreach (DataRow row in PayCost.Rows)
						{
							if (code == row[1].ToString() && row[3].ToString().Trim() != "Owner/NA")
							{
								dblCost = Convert.ToDouble(row[2].ToString()) * dblCostMultiplier;
							}
							else if (code == row[1].ToString() && row[3].ToString().Trim() == "Owner/NA")
							{
								dblCost = 0;
							}
						}
						laborcost += extd_cost;
						if (dblCost != 0)
						{
							grossmarginlaborcost += extd_cost;
						}
					}
					else if (linetype == "L" && billtype_id != 7)
					{
						laborcost += extd_cost;
						if (dblCost != 0)
						{
							grossmarginlaborcost += extd_cost;
						}
					}
					#endregion
					else if (billtype_id != 6 && billtype_id != 8 &&
							!(billtype_id == 3 && line_cost < 0.02)
							&& billtype_id != 9)
					{
						materialcost += extd_cost;
						if (dblCost != 0)
						{
							grossmarginmaterialcost += extd_cost;
						}
					}
					#region switch by billtypeid and build your totals
					switch (billtype_id)
					{
						case 0:
							regular += Convert.ToDouble(Math.Round(Math.Round((decimal)dblPrice * (decimal)dblQuantity, 3, MidpointRounding.AwayFromZero) * (decimal)discount, 2, MidpointRounding.AwayFromZero));
							if (linetype == "L")
							{
								laborprice += extd_sell_disc;
								bench_labor += extd_sell_disc;
							}
							else
							{
								materialprice += Convert.ToDouble(Math.Round(Math.Round((decimal)dblPrice * (decimal)dblQuantity, 3, MidpointRounding.AwayFromZero) * (decimal)discount, 2, MidpointRounding.AwayFromZero));
								bench_material += extd_sell;
							}
							break;
						case 1:
							jobcostforquote += extd_sell;
							if (linetype == "L")
							{
								bench_labor += extd_sell_disc;
							}
							else
							{
								bench_material += extd_sell;
							}
							break;
						case 2:
							invisiblecredit += extd_sell;
							if (linetype == "L")
							{
								laborprice += extd_sell_disc;
								bench_labor += extd_sell_disc;
							}
							else
							{
								materialprice += extd_sell;
								bench_material += extd_sell;
							}
							break;
						case 3:
							quoted += extd_sell;
							break;
						case 5:
							donotinclude += extd_sell;
							if (linetype == "L")
							{
								bench_labor += extd_sell_disc;
							}
							else
							{
								bench_material += extd_sell;
							}
							break;
						case 7:
							visiblenocharge += extd_sell_disc;
							break;
						case 9:
							progressbillings += extd_sell;
							break;
						case 10:
							visiblecredit += Convert.ToDouble(Math.Round(Math.Round((decimal)dblPrice * (decimal)dblQuantity, 3, MidpointRounding.AwayFromZero) * (decimal)discount, 2, MidpointRounding.AwayFromZero));
							if (linetype == "L")
							{
								laborprice += extd_sell;
								bench_labor += extd_sell_disc;
							}
							else
							{
								materialprice += Convert.ToDouble(Math.Round(Math.Round((decimal)dblPrice * (decimal)dblQuantity, 3, MidpointRounding.AwayFromZero) * (decimal)discount, 2, MidpointRounding.AwayFromZero));
								bench_material += Convert.ToDouble(Math.Round(Math.Round((decimal)dblPrice * (decimal)dblQuantity, 3, MidpointRounding.AwayFromZero) * (decimal)discount, 2, MidpointRounding.AwayFromZero));
							}
							break;
						case 11:
							quoted_2 += Math.Round(dblPrice * dblQuantity, 3, MidpointRounding.AwayFromZero);
							break;
						case 12:
							progressbillings_2 += Math.Round(dblPrice * dblQuantity, 3, MidpointRounding.AwayFromZero);
							break;
						case 13:
							BalanceForward += Convert.ToDouble(Math.Round(Math.Round((decimal)dblPrice * (decimal)dblQuantity, 3, MidpointRounding.AwayFromZero) * (decimal)discount, 2, MidpointRounding.AwayFromZero));
						break;
					}
					#endregion
				}
			}
			catch (Exception ex)
			{
					Toolbox.do_errorLog_errorStack(ex);
			}
			#region tax stuff
			var add = new NEAddress(woprog_Address_ID);
			var comp = new NeBusinessUnit(business_unit_id);
			var tax1 = new NeTax(woprog_tax1);
			var tax2 = new NeTax(woprog_tax2);
			var tax3 = new NeTax(woprog_tax3);
			var tax4 = new NeTax(woprog_tax4);
			var query_tax = string.Format(@"
SELECT
	ifnull(sum(((100-a.{0}_discount)*0.01)*a.{0}_qty_committed*a.{0}_price_sell*((ifnull(tax1.tax_percentage,0)+ifnull(tax2.tax_percentage,0)+ifnull(tax3.tax_percentage,0)+ifnull(tax4.tax_percentage,0))/100)),0) tax_total 
FROM
	{0} a
LEFT JOIN 
	tax tax1 ON a.{0}_tax1 = tax1.tax_id
LEFT JOIN 
	tax tax2 ON a.{0}_tax2 = tax2.tax_id
LEFT JOIN 
	tax tax3 ON a.{0}_tax3 = tax3.tax_id
LEFT JOIN 
	tax tax4 ON a.{0}_tax4 = tax4.tax_id
	", tablename);
			if (comp.TaxLabour == 1)
			{
				tax_labour_this = laborprice * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
				tax_labour_adjust = Toolbox.doSQL_double(conn, string.Format(@"
{0}
WHERE
	a.{1}_woprog_id = {2} and 
	a.{1}_type = 'L' and 
	a.{1}_billtypeid in (0,2,10)", query_tax, tablename, woprog_id), null);
			}
			if (comp.TaxMaterial == 1)
			{
				tax_material_this = materialprice * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
				tax_material_Adjust = Toolbox.doSQL_double(conn, string.Format(@"
{0}
WHERE
	a.{1}_woprog_id = {2} AND 
	a.{1}_type = 'M' AND 
	a.{1}_billtypeid in (0,2,10)", query_tax, tablename, woprog_id), null);

			}
			if (comp.selfassess_tax == 1)  // if company self assess
			{
				if (add.Tax1Exempt == "")  // if address is not tax exempt
				{
					if (comp.TaxQuotedJobs == 0 && QuoteID != "0")  // if it's a quoted job, the customer is not exempt and the branch doesn't charge tax on quoted jobs.. time to self assess material 
					{
						satax_material = materialcost * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
					}
					else if (comp.TaxMaterial == 1 && materialprice != 0 && laborprice == 0)
					{
						satax_material = materialcost * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
					}
				}
				else
				{
					if (comp.TaxMaterial == 1 && materialprice != 0 && laborprice == 0)
					{
						tax_material_this = materialprice * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
						tax_material_Adjust = Toolbox.doSQL_double(conn, string.Format(@"
{0}
WHERE
	a.{1}_woprog_id = {2} AND 
	a.{1}_type = 'M' AND 
	a.{1}_billtypeid IN (0,2,10)", query_tax, tablename, woprog_id), null);
					}
				}
			}
			if (comp.TaxQuotedJobs == 0 && QuoteID != "0")  // if it's a quoted job, the customer is not exempt and the branch doesn't charge tax on quoted jobs.. time to self assess material 
			{
				tax_material_this = 0;
			}
			if (comp.TaxQuotedJobs == 1)
			{
				tax_material_this = (quoted + quoted_2 + progressbillings_2 + regular + invisiblecredit + visiblecredit - laborprice) * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
				tax_material_Adjust = Toolbox.doSQL_double(conn, string.Format(@"
{0}
WHERE
	a.{1}_woprog_id = {2} AND
	(a.{1}_type = 'M' OR a.{1}_type = 'Q') AND 
	a.{1}_billtypeid in (0,2,3,10,11)", query_tax, tablename, woprog_id), null);
			}
			#endregion
			woprog_labourcost = laborcost;
			LabourTotalSell = laborprice;
			woprog_materialcost = materialcost;
			TAX_MATERIAL = tax_material_this;
			TAX_LABOUR = tax_labour_this;
			SATAX_LABOUR = satax_labour;
			SATAX_MATERIAL = satax_material;
			//Created these variables for cl, or CLEAN numbers... Long decimal placed numbers cause problems when divided for gross margin.
			var cl_quoted = Math.Round(quoted, 2, MidpointRounding.AwayFromZero);
			var cl_regular = Math.Round(regular, 2, MidpointRounding.AwayFromZero);
			var cl_invisiblecredit = Math.Round(invisiblecredit, 2, MidpointRounding.AwayFromZero);
			var cl_visiblecredit = Math.Round(visiblecredit, 2, MidpointRounding.AwayFromZero);
			var cl_jobcostforquote = Math.Round(jobcostforquote, 2, MidpointRounding.AwayFromZero);
			var cl_donotinclude = Math.Round(donotinclude, 2, MidpointRounding.AwayFromZero);
			var cl_visiblenocharge = Math.Round(visiblenocharge, 2, MidpointRounding.AwayFromZero);
			var cl_quoted_2 = Math.Round(quoted_2, 2, MidpointRounding.AwayFromZero);
			var cl_progressbillings = Math.Round(progressbillings, 2, MidpointRounding.AwayFromZero);
			var cl_progressbillings_2 = Math.Round(progressbillings_2, 2, MidpointRounding.AwayFromZero);
			woprog_materialtotalSell = Convert.ToDouble(Math.Round(Convert.ToDecimal(materialprice), 2, MidpointRounding.AwayFromZero));

			TotalTimeAndMaterial = cl_regular + cl_jobcostforquote + cl_donotinclude + cl_visiblenocharge; // +invisiblecredit + visiblecredit;
			TAX_LABADJUSTED = tax_labour_adjust;
			TAX_MATADJUSTED = tax_material_Adjust;
			TAX_QUOTEADJUSTED = tax_quoted_adjust;
			BenchmarkLabourSell = bench_labor;
			BenchmarkMaterialSell = bench_material;
			ThisWOLabourBench = BenchmarkLabourSell;
			ThisWOLabourCost = laborcost;
			ThisWOMaterialBench = BenchmarkMaterialSell;
			ThisWOMaterialCost = materialcost;
			if (QuoteID != "0")
			{
				BenchmarkLabourSell = Toolbox.doSQL_double(conn, @"SELECT GET_TOTAL_LABOR_BENCHMARK_SELL_FROM_ALL_WORKORDERS(@v0)", new object[] { woprog_id });
				BenchmarkMaterialSell = Toolbox.doSQL_double(conn, @"SELECT GET_TOTAL_MATERIAL_BENCHMARK_SELL_FROM_ALL_WORKORDERS(@v0)", new object[] { woprog_id });
				TotalTimeAndMaterial = BenchmarkLabourSell + BenchmarkMaterialSell;			
				woprog_labourcost = GetTotalLineCost(conn, woprog_id, "L");
				woprog_materialcost = GetTotalLineCost(conn, woprog_id, "M");
			}
			ProgressBilled = Toolbox.doSQL_double(conn, @"SELECT JOBCOST_PROGRESSBILLED(@v0)", new object[] { woprog_id });
			woprog_StillToBeBilled = QuoteID != "0" 
				? double.Parse(QuotedPrice) - ProgressBilled 
				: cl_quoted + cl_quoted_2 + cl_progressbillings_2 + cl_regular + cl_invisiblecredit + cl_visiblecredit; ; // +progressbillvalue;
			woprog_grossmargin = (cl_quoted + cl_quoted_2 + cl_progressbillings_2 + cl_regular + cl_invisiblecredit + cl_visiblecredit + BalanceForward -
								(grossmarginlaborcost + grossmarginmaterialcost)) /
								(cl_quoted + cl_quoted_2 + cl_progressbillings_2 + cl_regular + cl_invisiblecredit + cl_visiblecredit + BalanceForward);
			if (woprog_grossmargin.ToString().Contains("Infinity") || double.IsNaN(woprog_grossmargin) || woprog_StillToBeBilled == 0)
			{
				woprog_grossmargin = 0;
			}
			woprog_grossmargin = woprog_grossmargin > 100 ? 100 : woprog_grossmargin;
			SetTotals(woprog_id);
			}
		}
		public static double GetTotalLineCost(MySqlConnection _connection, int _woprog_id, string _type)
		{
			if(_type != "L" && _type != "M") return 0d;
			return Toolbox.doSQL_double(_connection, @"SELECT ROUND(IFNULL(GET_TOTAL_COST_FROM_ALL_WORKORDERS(@v0, @v1), 0), 2)", new object[] { _woprog_id, _type });
		}
		public static void update_header_totals(string wo_id, int _business_unit_id, string wo_number, MySqlConnection connection = null, MySqlTransaction transaction = null)
		{
			var n = new NeSalesOrder();
			var success = n.getSalesOrderValues(wo_id);
			var u = new NeWOProg
			{
				business_unit_id = _business_unit_id,
				OrderNumber = wo_number,
				LabourTotalSell = n.benchmark_labor_sell,
				woprog_materialtotalSell = n.MaterialPriceSell,
				woprog_labourcost = n.LaborCost,
				woprog_materialcost = n.MaterialCost,
				woprog_StillToBeBilled = n.StillToBeBilled,
				TotalTimeAndMaterial = n.TotalTAndM,
				ProgressBilled = n.ProgressBilled,
				woprog_grossmargin = n.GrossMargin,
				BenchmarkLabourSell = n.benchmark_labor_sell,
				BenchmarkMaterialSell = n.benchmark_material_sell,
				ThisWOLabourBench = n.THIS_LABORPRICESELL,
				ThisWOLabourCost = n.THIS_LABORCOST,
				ThisWOMaterialBench = n.THIS_MATERIALPRICESELL,
				ThisWOMaterialCost = n.THIS_MATERIALCOST
			};
			if (success)
			{
				if (connection == null || transaction == null)
					u.SetTotals(Convert.ToInt32(wo_id));
				else
					u.SetTotals(Convert.ToInt32(wo_id), connection, transaction);
			}
			if (connection == null || transaction == null)
				NeWODetailCurrent.reorder_lines(Convert.ToInt32(wo_id));
			else
				NeWODetailCurrent.reorder_lines(Convert.ToInt32(wo_id), connection, transaction);
		}
		public void SetTotals(int woid, MySqlConnection connection, MySqlTransaction transaction)
		{
			try
			{
				Toolbox.doSQL_void(connection,
@"  UPDATE 
		woprog 
	SET 
		woprog_labourtotalsell      = @v0, 
		woprog_materialtotalsell    = @v1, 
		woprog_materialcost         = @v2, 
		woprog_progressbilled       = @v3, 
		woprog_stilltobebilled      = @v4,
		woprog_laborcost            = @v5, 
		woprog_grossmargin          = @v6, 
		woprog_totaltandm           = @v7, 
		benchmark_labor_sell        = @v9, 
		benchmark_material_sell     = @v10, 
		this_wo_labour_cost         = @v11, 
		this_wo_material_cost       = @v12, 
		this_wo_labour_bench        = @v13, 
		this_wo_material_bench      = @v14, 
		woprog_ts                   = IF(ROUND(woprog_progressbilled, 2) != ROUND(@v3, 2), NOW() , woprog_ts)  
	WHERE 
		woprog_id = @v8 
	LIMIT 1",
					new object[]
					{
						LabourTotalSell,
						woprog_materialtotalSell,
						woprog_materialcost,
						ProgressBilled,
						woprog_StillToBeBilled,
						woprog_labourcost,
						woprog_grossmargin,
						TotalTimeAndMaterial,
						woid,
						BenchmarkLabourSell,
						BenchmarkMaterialSell,
						ThisWOLabourCost,
						ThisWOMaterialCost,
						ThisWOLabourBench,
						ThisWOMaterialBench
					}, transaction);
			}
			catch (Exception ex)
			{
				Toolbox.do_errorLog_errorStack(ex);
				throw ex;
			}
		}
		public void SetTotals(int woid)
		{  
			try
			{
				Toolbox.doSQL_void(
@"  UPDATE 
		woprog 
	SET 
		woprog_labourtotalsell      = @v0, 
		woprog_materialtotalsell    = @v1, 
		woprog_materialcost         = @v2, 
		woprog_progressbilled       = @v3, 
		woprog_stilltobebilled      = @v4,
		woprog_laborcost            = @v5, 
		woprog_grossmargin          = @v6, 
		woprog_totaltandm           = @v7, 
		benchmark_labor_sell        = @v9, 
		benchmark_material_sell     = @v10, 
		this_wo_labour_cost         = @v11, 
		this_wo_material_cost       = @v12, 
		this_wo_labour_bench        = @v13, 
		this_wo_material_bench      = @v14, 
		woprog_ts                   = IF(ROUND(woprog_progressbilled, 2) != ROUND(@v3, 2), NOW() , woprog_ts)  
	WHERE 
		woprog_id = @v8 
	LIMIT 1",
					new object[]
					{
						LabourTotalSell, 
						woprog_materialtotalSell, 
						woprog_materialcost,
						ProgressBilled, 
						woprog_StillToBeBilled, 
						woprog_labourcost, 
						woprog_grossmargin,
						TotalTimeAndMaterial, 
						woid, 
						BenchmarkLabourSell, 
						BenchmarkMaterialSell,
						ThisWOLabourCost, 
						ThisWOMaterialCost, 
						ThisWOLabourBench, 
						ThisWOMaterialBench
					});
			}
			catch (Exception ex)
			{
				Toolbox.do_errorLog_errorStack(ex);
			}
		}
		public DataTable GetOpenWorkOrders(int co_id, int cu_id, bool is_progress)
		{
			if (is_progress)
			{
				return Toolbox.doSQL_dt(@" SELECT woprog_id,
    woprog_bvwo,
    CONCAT('WO#: 0',CAST(woprog_bvwo AS UNSIGNED), ' - ', LEFT(woprog_description, 100)) description 
FROM 
    woprog
WHERE 
    business_unit_id = @v0 AND
    woprog_customer_id = @v1 AND
    woprog_bvwo != 'Not Entered' AND
    woprog_status IN('Open','Rework') AND 
    IFNULL(woprog_quoteid, 0) != 0 AND 
    IFNULL(woprog_associate_woprog_id, 0) = 0",
    new object[] { co_id, cu_id });
			}
			else
			{
				return Toolbox.doSQL_dt(@" SELECT woprog_id,
    woprog_bvwo, 
    CONCAT('WO#: 0',CAST(woprog_bvwo AS UNSIGNED), ' - ', woprog_status , ' - ', LEFT(woprog_description, 100)) description 
FROM 
    woprog 
WHERE 
    business_unit_id = @v0  AND
    woprog_customer_id = @v1  AND 
    woprog_status != 'Deleted' AND 
    woprog_iscredit = FALSE AND
    woprog_isrebill = FALSE AND
    woprog_bvwo != 'Not Entered' AND
    IFNULL(woprog_associate_woprog_id, 0) = 0", 
    new object[] { co_id, cu_id });
			}
		}
		public static double total_billed(MySqlConnection _conn, int _id)
		{
			// Check if it's being passed a child, if so, we need the parent.
			var parent_ref = Toolbox.doSQL_int(_conn, @"SELECT woprog_associate_woprog_id FROM woprog WHERE woprog_id = @v0 ", new object[] { _id });
			if (parent_ref != 0)
			{
				_id = parent_ref;
			}
			return Toolbox.doSQL_double(_conn, string.Format(@"
SELECT 
	SUM(s)
FROM 
	(
	SELECT IFNULL(SUM({1}_qty_committed * {1}_price_sell),0) s FROM {1} where {1}_woprog_id = {0} and {1}_billtypeid IN (9,12) 
	UNION 
	SELECT IFNULL(SUM({2}_qty_committed*{2}_price_sell),0) s FROM {2} WHERE {2}_woprog_id = {0} AND {2}_billtypeid IN (9,12)
	) w", _id, "wo_detail_current", "wo_detail_history"), null); // I did this parameterized because I really don't like typing out wo_detail_#####
		}
		public static int[] GetChildren(int parentWoprogId)
			{
			var children = new int[]{};
			if(parentWoprogId == 0) return children;
			children = Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(woprog_id) FROM woprog WHERE parent_woprog_id = @v0", new object[] { parentWoprogId }).Split(',').Select(int.Parse).ToArray();
			return children;
			}
		public static int[] GetOpenChildren(int parentWoprogId, bool includeQuoted)
			{
			var children = new int[]{};
			if(parentWoprogId == 0) return children;
			var cChildren = includeQuoted 
								? Toolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE parent_woprog_id = @v0 AND woprog_quoteid > 0 AND woprog_status NOT IN (@v1, @v2)", new object[] { parentWoprogId, OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced })
								: Toolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE parent_woprog_id = @v0 AND woprog_quoteid = 0 AND woprog_status NOT IN (@v1, @v2)", new object[] { parentWoprogId, OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced });
			if(cChildren == 0) return children;
			children = includeQuoted
							? Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(woprog_id) FROM woprog WHERE parent_woprog_id = @v0 AND woprog_quoteid > 0 AND woprog_status NOT IN (@v1, @v2)", new object[] { parentWoprogId, OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced }).Split(',').Select(int.Parse).ToArray()
							: Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(woprog_id) FROM woprog WHERE parent_woprog_id = @v0 AND woprog_quoteid = 0 AND woprog_status NOT IN (@v1, @v2)", new object[] { parentWoprogId, OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced }).Split(',').Select(int.Parse).ToArray();
			return children;
			}
		public static int[] GetInvoicedChildren(int parentWoprogId, bool includeQuoted)
			{
			var children = new int[]{};
			if(parentWoprogId == 0) return children;
			var cChildren = includeQuoted
								? Toolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE parent_woprog_id = @v0 AND woprog_quoteid > 0 AND woprog_status IN (@v1, @v2)", new object[] { parentWoprogId, OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced })
								: Toolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE parent_woprog_id = @v0 AND woprog_quoteid = 0 AND woprog_status IN (@v1, @v2)", new object[] { parentWoprogId, OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced });
			if(cChildren == 0) return children;
			children = includeQuoted
							? Toolbox.doSQL_string(@"SELECT IFNULL(GROUP_CONCAT(woprog_id), '') FROM woprog WHERE parent_woprog_id = @v0 AND woprog_quoteid > 0 AND woprog_status IN (@v1, @v2)", new object[] { parentWoprogId, OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced }).Split(',').Select(int.Parse).ToArray()
							: Toolbox.doSQL_string(@"SELECT IFNULL(GROUP_CONCAT(woprog_id), '') FROM woprog WHERE parent_woprog_id = @v0 AND woprog_quoteid = 0 AND woprog_status IN (@v1, @v2)", new object[] { parentWoprogId, OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced }).Split(',').Select(int.Parse).ToArray();
			return children;
			}
		public static DataTable get_child_workorders(int parent_woprog_id)
		{
			var dt = Toolbox.doSQL_dt(@"select woprog_id,woprog_status from woprog  where parent_woprog_id=@v0 and parent_woprog_id!=0", new object[] { parent_woprog_id });
			return dt;
		}
		public string RefreshServiceDatesText(int woprogid)
		{
			var mindate = Convert.ToDateTime(Toolbox.doSQL_string(@"Select ifnull(min(date),now()) from membertime  where membertime_woprog_id =@v0", new object[] { woprogid }));
			var maxdate = Convert.ToDateTime(Toolbox.doSQL_string(@"Select ifnull(max(date),now()) from membertime  where membertime_woprog_id =@v0", new object[] { woprogid }));
			if (mindate.Date != maxdate.Date)
			{
				return "Service Dates: " + mindate.Date.ToLongDateString() + " - " + maxdate.Date.ToLongDateString();
			}
			else
			{
				return "Service Date: " + mindate.Date.ToLongDateString();
			}
		}

		public static void RefreshAndSaveServiceDatesText(int _woprogid, MySqlConnection connection = null, MySqlTransaction transaction = null)
		{
			var _tools = new Toolbox();
			var wo = new NeWOProg();
			try
			{
				var c = Toolbox.doSQL_int(@"SELECT COUNT(membertime_id) FROM membertime WHERE membertime_woprog_id = @v0 ", new object[] { _woprogid });
				if (c > 0)
				{
					var mindate = Convert.ToDateTime(_tools.getSQL_string(@"Select min(date) from membertime  where membertime_woprog_id =@v0", new object[] { _woprogid }));
					var maxdate = Convert.ToDateTime(_tools.getSQL_string(@"Select max(date) from membertime  where membertime_woprog_id =@v0", new object[] { _woprogid }));

					string servicedates;
					if (mindate.Date != maxdate.Date)
					{
						servicedates = "Service Dates: " + mindate.Date.ToLongDateString() + " - " + maxdate.Date.ToLongDateString();
					}
					else
					{
						servicedates = "Service Date: " + mindate.Date.ToLongDateString();
					}
					if (connection == null || transaction == null)
						wo.SaveServiceDatesText(servicedates, _woprogid);
					else
						wo.SaveServiceDatesText(servicedates, _woprogid, connection, transaction);
				}
			}
			catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }
			wo = null;
		}
		public void SaveServiceDatesText(string _text, int woprogid, MySqlConnection connection = null, MySqlTransaction transaction = null)
		{
			var x = Toolbox.doSQL_int(@"select ifnull(count(woprog_invoice_text_woprog_id),0) from woprog_invoice_text  where woprog_invoice_text_woprog_id =@v0", new object[] { woprogid });

			if (x == 0)
			{
				Toolbox.doSQL_void(connection,
					@"insert into woprog_invoice_text (woprog_invoice_text_servicedates,woprog_invoice_text_woprog_id)  values (@v0,@v1)",
					new object[] { _text, woprogid }, transaction);
			}
			else
			{
				Toolbox.doSQL_void(connection, @"Update woprog_invoice_text  set woprog_invoice_text_servicedates =@v0  where woprog_invoice_text_woprog_id =@v1",
					new object[] { _text, woprogid }, transaction);
			}

		}

		public void print_barcode_label(int copies)
		{
			try
			{
				var printDoc = new PrintDocument();
				var WOPrinter = new NeBusinessUnit(business_unit_id);
				var yy = new PaperSize("Custom Paper Size", 220, 99);
				printDoc.DefaultPageSettings.PaperSize = yy;
				printDoc.DefaultPageSettings.Margins.Left = 1;
				printDoc.DefaultPageSettings.PrinterSettings.Copies = (short)copies;
				printDoc.PrinterSettings.PrinterName = WOPrinter.BarCodePrinter;
				printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
				printDoc.Print();
			}
			catch
			{
				throw new Exception("Printing Barcode Failed");
			}
		}
		private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
		{
			var rightAlign = new StringFormat();
			rightAlign.Alignment = StringAlignment.Far;
			rightAlign.LineAlignment = StringAlignment.Far;
			var leftAlign = new StringFormat();
			leftAlign.Alignment = StringAlignment.Near;
			leftAlign.LineAlignment = StringAlignment.Near;
			var printFont = new Font("Arial", 9);
			var printFont1 = new Font("Arial", 8);
			var printFontdesc = new Font("Arial", 5);
			var barcodefont = new Font("Free 3 of 9", 24);
			var rect = new Rectangle(10, 32, 195, 50);
			var br = new SolidBrush(Color.Black);
			e.Graphics.DrawRectangle(Pens.Transparent, rect);
			e.Graphics.DrawString(OrderNumber, printFont, br, 10, 51, leftAlign);
			e.Graphics.DrawString("Work Order Bar Code", printFont1, br, 10, 71, leftAlign);
			e.Graphics.DrawString(CustomerName, printFontdesc, br, rect);
			e.Graphics.DrawString("*002-" + woprog_id + "*", barcodefont, br, 15, 5);
		}
		public static Dictionary<OpsWOStatus.Enums, string> wo_status
		{
			get
			{
			var d = new Dictionary<OpsWOStatus.Enums, string>
						{
							{OpsWOStatus.Enums.Deleted, OpsWOStatus.Deleted},
							{OpsWOStatus.Enums.Open, OpsWOStatus.Open},
							{OpsWOStatus.Enums.JustScanned, OpsWOStatus.JustScanned},
							{OpsWOStatus.Enums.InitialPrep, OpsWOStatus.InitialPrep},
							{OpsWOStatus.Enums.Invoiced, OpsWOStatus.Invoiced},
							{OpsWOStatus.Enums.QuestionsForPM, OpsWOStatus.QuestionsForPM},
							{OpsWOStatus.Enums.Rework, OpsWOStatus.Rework},
							{OpsWOStatus.Enums.WaitingBMApproval, OpsWOStatus.WaitingBMApproval},
							{OpsWOStatus.Enums.WaitingForParts, OpsWOStatus.WaitingForParts},
							{OpsWOStatus.Enums.WaitingForPO, OpsWOStatus.WaitingForPO},
							{OpsWOStatus.Enums.WaitingPMApproval, OpsWOStatus.WaitingPMApproval},
							{OpsWOStatus.Enums.WaitingToBeInvoiced, OpsWOStatus.WaitingToBeInvoiced},
							{OpsWOStatus.Enums.WaitingParentBMApproval, OpsWOStatus.WaitingParentBMApproval},
							{OpsWOStatus.Enums.ClosedReassigned, OpsWOStatus.ClosedReassigned}
						};
			return d;
			}
		}
        public enum MoveScanType
        { 
            /// <summary>
            ///  For scan to be attached 
            /// </summary>
            New,
            /// <summary>
            ///  For scan to be replaced 
            /// </summary>
            Replace
        }
		public static void move_scans(string _file_server, string _wo_path, string _old_filename, string _new_filename, MoveScanType scanType)
			{
			string step = "";
           
			try
				{
                step = "Initial check if new file exists";
				var fileExists = File.Exists(_file_server + @"\WOs\" + _new_filename);
                var isUnlinkedScan = File.Exists(_wo_path + _old_filename);
                if (fileExists == true) scanType = MoveScanType.Replace;// overriding if a file with new name exists, the user may want to replace it with new scan
                if (scanType == MoveScanType.Replace)
                { 
				//move and rename remote file ///////////////////////////////////////////////////////////
				
				step = "Copying";
			//	File.Copy(_wo_path + _old_filename, string.Format(@"{0}\WorkPro\{1}", _wo_path, _new_filename),fileExists);
				step = "Deleting";
				File.Delete(_file_server + @"\WOs\"+ _new_filename);
				if (_old_filename != _new_filename)
					{
                    // rename it locally so it can be displayed
                   
					//step = "Existence Check";
					//if (fileExists)
						//{
						step = "Deleting Scan";
						File.Delete(_file_server + @"\WOs\" + _new_filename);

						step = "Moving Scan";
						File.Move(_file_server + @"\WOs\" + _old_filename, _file_server + @"\WOs\" + _new_filename);
						//}
					
                    }
                }
                else if(scanType==MoveScanType.New)
                {
                    if (isUnlinkedScan)
                    {
                        File.Move(_wo_path + _old_filename, _file_server + @"\WOs\" + _new_filename);
                    }
                    else
                    {
                        File.Move(_file_server + @"\WOs\" + _old_filename, _file_server + @"\WOs\" + _new_filename);
                        // Moved it but we need to delete the old file too
                        File.Delete(_file_server + @"\WOs\" + _old_filename);
                    }
                }
               
				}
			catch (Exception ee)
				{
				Toolbox.do_errorLog(ee, $@"WO Path: {_wo_path} -- Step: {step} -- Path: {_file_server} -- ScanFile: {_old_filename} -- File Name: {_new_filename}");
				throw;
				}
			}
		public static bool InAdvancedStatus(string status)
		{
			return Toolbox.Contains(status, new[] { "Waiting BM Approval", "Waiting PM Approval", OpsWOStatus.Invoiced, "Waiting to be Invoiced" });
		}
		public static bool InHistoricStatus(string status)
		{
		return Toolbox.Contains(status, new[] { OpsWOStatus.Invoiced, "Waiting to be Invoiced" });
		}
		/// <summary>
		/// Check that the work order can move to the specified status
		/// </summary>
		/// <param name="wo"><para>This should be a fresh WO object, not changed by any prior code</para></param>
		/// <param name="_status"></param>
		public static void check_before_move(NeWOProg wo, string _status)
		{
			var WOCustomer = new NECustomer(wo.WOProg_Customer_ID);
			var MovingToAdvancedStatus = InAdvancedStatus(_status);
			var ErrorList = new List<string>();

            using (var nesi_conn = Toolbox.connect())
			{
                

                if (Toolbox.Contains(wo.Status, new[] { OpsWOStatus.Invoiced, "Waiting to be Invoiced" }))
				{
					ErrorList.Add("This work order has been invoiced since you last loaded this page, and cannot be moved.. please reload.");
				}

				if (MovingToAdvancedStatus) // Before a WO can move to a "Processing" status, a few items need be checked
				{

                    //Lines in History Check
                    var n_hist = Toolbox.doSQL_int(nesi_conn, @"SELECT COUNT(*) FROM wo_detail_history WHERE wo_detail_history_woprog_id =@v0", new object[] { wo.woprog_id });
                    if(n_hist > 0)
                    {
                        ErrorList.Add("This work order already exists in the history table - " + n_hist);
                    }

                    // AP Problems Check
                    var chkApProblems = Toolbox.doSQL_string(nesi_conn, @"SELECT IFNULL(GROUP_CONCAT(DISTINCT b.poprog_bvpo), '') FROM po_details_current a LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_woprog_id = @v0 and a.is_gl_account=false  AND b.poprog_status = 10", new object[] { wo.woprog_id });
					if (chkApProblems != "")
					{
						ErrorList.Add("Cannot move this work order because a linked purchase order is in AP Problems - " + chkApProblems);
					}
					// PO Questions Check
					var chkPOQuestions = Toolbox.doSQL_string(nesi_conn, @"SELECT IFNULL(GROUP_CONCAT(DISTINCT b.poprog_bvpo), '') FROM po_details_current a LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_woprog_id = @v0 and a.is_gl_account=false  AND b.poprog_status = 9", new object[] { wo.woprog_id });
					if (chkPOQuestions != "")
					{
						ErrorList.Add("Cannot move this work order because a linked purchase order is in Questions - " + chkPOQuestions);
					}
					// 777 Parts Check
					var chk777Parts = Toolbox.doSQL_int(nesi_conn, @"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_master_id = 777 AND wo_detail_current_woprog_id =@v0", new object[] { wo.woprog_id });
					if (chk777Parts > 0)
					{
						ErrorList.Add($"This Work Order contains ({chk777Parts}) parts with the master_id of 777, work order cannot be advanced.");
					}
					// PO Comments Check
					var chkPOComments = Toolbox.doSQL_string(nesi_conn, @"SELECT IFNULL(GROUP_CONCAT(DISTINCT LPAD(po_details_poprog_id, 10, '0')), '') FROM po_details_current WHERE po_details_woprog_id = @v0 AND po_details_part_no = '' AND po_details_line_active = 1", new object[] { wo.woprog_id });
					if (chkPOComments != "")
					{
						ErrorList.Add($@"Check with your purchaser, the following PO's have an active comment line holding up the status movement on this WO - {chkPOComments}");
					}
					// Unapproved Expenses Check
					var chkUnapprovedExpenses = Toolbox.doSQL_int(nesi_conn, @"SELECT COUNT(*) FROM wo_detail_current a LEFT JOIN expense_reimbursement b ON a.wo_detail_current_consignment_id = b.id_expense WHERE wo_detail_current_consignment_id > 0 AND wo_detail_current_master_id IN (55556) AND wo_detail_current_origin = 'Expense Reimbursement' AND wo_detail_current_woprog_id = @v0  AND b.approved = -1 AND b.id_payperiod > 0", new object[] { wo.woprog_id });
					if (chkUnapprovedExpenses > 0)
					{
						ErrorList.Add($"This work order contains ({chkUnapprovedExpenses}) unapproved expense reimbursements, work order cannot be advanced.");
					}
					// Child WO Check
					var chkChildWorkOrders = Toolbox.doSQL_int(nesi_conn, @"SELECT COUNT(*) FROM woprog WHERE woprog_status NOT IN ('Waiting to be Invoiced', 'Invoiced') AND (woprog_associate_woprog_id = @v0  OR parent_woprog_id = @v0 )", new object[] { wo.woprog_id });
					if (chkChildWorkOrders > 0 && _status == OpsWOStatus.WaitingToBeInvoiced)
					{
						ErrorList.Add("This work order has child work orders associated with it that are not invoiced... Cannot move this to be invoiced yet");
					}
					// PO Required Check
					if (wo.PONumber.Trim() == "" && WOCustomer.PORequired == "T" && _status == OpsWOStatus.WaitingToBeInvoiced)
					{
						ErrorList.Add("This customer requires a PO in order to be invoiced.");
					}
				}
			}
			if (ErrorList.Any())
			{
				throw new Exception("The following problem(s) need to be resolved:\n" + string.Join("\n", ErrorList));
			}
		}

		public static void move_status(NeWOProg wo, NeMember user, string move_to_current, OpsWOStatus.Enums _status)
		{
			var this_status = wo_status[_status];
			using (var nesi_conn = Toolbox.connect())
			{

					switch (_status)
					{
						case OpsWOStatus.Enums.InitialPrep:
							#region Initial Prep
							send_invoice_warning_child_wos(wo.woprog_id);
							add_history(wo.woprog_id, user.id, OpsWOStatus.InitialPrep, Toolbox.MySQLNow_long());
							add_history(wo.woprog_id, user.id, OpsWOStatus.InitialPrep, Toolbox.MySQLNow_long());
							wo.woprog_scanned_date = DateTime.Now;
							wo.woprog_timesheet_percentage = 100;
							wo.Status = OpsWOStatus.InitialPrep;
							if (wo.woprog_verbal_quote)
							{
								wo.Description = $"{wo.Description} as per verbal quote {wo.woprog_expected_sales_value:C2}";
							}
							wo.SaveWorkOrder();
							#region Get Time Sheet Comments and add to comment box
							var CommentLine = new StringBuilder();
							var CommentsTable = Toolbox.doSQL_dt(nesi_conn, @"SELECT comments FROM wocomment, member WHERE wocomment_member_id = member_id AND wocomment_member_id != 0 AND woprog_id = @v0  AND wocomment.business_unit_id = @v1  ORDER BY Created_Date", new object[] { wo.woprog_id, wo.business_unit_id });
							foreach (DataRow dr in CommentsTable.Rows)
							{
								CommentLine.AppendFormat("{0}\n", dr["comments"]);
							}
							var ChildCommentsTable = Toolbox.doSQL_dt(nesi_conn, @"SELECT membertime_wocomment_child_id id FROM membertime WHERE membertime_child_woprog_id = @v0  AND membertime_wocomment_child_id != 0", new object[] { wo.woprog_id });
							foreach (DataRow dr in ChildCommentsTable.Rows)
							{
								try
								{
									var child_comment = Toolbox.doSQL_string(nesi_conn, @"SELECT IFNULL(CONCAT(DATE_FORMAT(created_date,'%Y-%m-%d'), '-', member_fullname, '-', Comments), "") FROM wocomment WHERE wocomment_id =@v0 ", new object[] { dr["id"] });
									CommentLine.Append(child_comment);
								}
								catch (Exception Exce) { Toolbox.do_errorLog_errorStack(Exce); }
							}
							var output = CommentLine.ToString();
							if (CommentLine.Length > 0)
							{
								output = "----- \n" + CommentLine;
								var exists = Toolbox.doSQL_int(nesi_conn, @"SELECT COUNT(*) FROM wocomment WHERE woprog_id = @v0  AND wocomment.business_unit_id = @v1  AND wocomment_member_id = '0'", new object[] { wo.woprog_id, wo.business_unit_id });
								if (exists > 0)
								{
									Toolbox.doSQL_void(nesi_conn, @" UPDATE wocomment SET comments = CONCAT(comments,'\n ',@v0 ) , modified_date = NOW() WHERE woprog_id = @v1  AND wocomment_member_id = 0 AND wocomment.business_unit_id = @v2 ", new object[] { output, wo.woprog_id, wo.business_unit_id });
								}
								else
								{
									Toolbox.doSQL_void(nesi_conn, @" INSERT INTO wocomment ( workorder_id, wocomment_member_id, comments, personal, printcomments, member_id_audit, created_date, modified_date, business_unit_id , woprog_id ) VALUES ( @v0 , 0, @v1 , 0, 0, @v2 , NOW(), NOW(), @v3 , @v4  )", new object[] { wo.OrderNumber, output, user.id, wo.business_unit_id, wo.woprog_id });
								}
							}
							#endregion
							#endregion Initial Prep
							break;
						case OpsWOStatus.Enums.QuestionsForPM:
							#region Questions For PM
							add_history(wo.woprog_id, user.id, this_status, Toolbox.MySQLNow_long());
							wo.Status = this_status;
							wo.SaveWorkOrder();
							Toolbox.doSQL_void(nesi_conn, @"UPDATE passport SET active = false, valid = true WHERE url_yes LIKE CONCAT('%woprog_id=',@v0,'&%')", new object[] { wo.woprog_id });
							#endregion Questions For PM
							break;
                     case OpsWOStatus.Enums.ClosedReassigned:
                        #region
                        add_history(wo.woprog_id, user.id, this_status, Toolbox.MySQLNow_long());
                        wo.Status = this_status;
                        wo.SaveWorkOrder();
                        Toolbox.doSQL_void(nesi_conn, @"UPDATE passport SET active = false, valid = true WHERE url_yes LIKE CONCAT('%woprog_id=',@v0,'&%')", new object[] { wo.woprog_id });
                        break;
                    #endregion

                    case OpsWOStatus.Enums.Rework:
							#region Rework
							send_invoice_warning_child_wos(wo.woprog_id);
							add_history(wo.woprog_id, user.id, this_status, Toolbox.MySQLNow_long());
							wo.Status = this_status;
							wo.SaveWorkOrder();
							Toolbox.doSQL_void(nesi_conn, @"UPDATE passport SET active = false, valid = true WHERE url_yes LIKE CONCAT('%woprog_id=',@v0,'&%')", new object[] { wo.woprog_id });
							#endregion Rework
							break;
						case OpsWOStatus.Enums.WaitingPMApproval:
							#region Waiting PM Approval
							send_invoice_warning_child_wos(wo.woprog_id);
							add_history(wo.woprog_id, user.id, this_status, Toolbox.MySQLNow_long());
							wo.Status = this_status;
							wo.SaveWorkOrder();
							#endregion Waiting PM Approval
							break;
						case OpsWOStatus.Enums.WaitingBMApproval:
							#region Waiting BM Approval
							Toolbox.doSQL_void(nesi_conn, @"UPDATE wo_detail_current SET wo_detail_current_billtypeid = 5 WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_qty_committed = 0 AND wo_detail_current_billtypeid = 0", new object[] { wo.woprog_id });
							add_history(wo.woprog_id, user.id, this_status, Toolbox.MySQLNow_long());
							wo.Status = this_status;
							wo.SaveWorkOrder();

							Toolbox.doSQL_void(nesi_conn, @"UPDATE passport SET active = false, valid = true WHERE url_yes LIKE CONCAT('%woprog_id=',@v0,'&%')", new object[] { wo.woprog_id });
							send_invoice_warning_child_wos(wo.woprog_id);
							#endregion Waiting BM Approval
							break;
                    case OpsWOStatus.Enums.WaitingForPO:
                        #region Waiting For PO
                        var move_to = move_to_current != "" && move_to_current == "yes";
                        if (move_to)
                        {
                            NeWODetailCurrent.move_to_current(wo.woprog_id);
                        }
						//Fix for 2760. Delete the child work order line (55559) from the parent work order when
						//the child work order is moved from "Waiting To Be Invoiced" to "Waiting For PO" status 
						//in order to avoid duplication of the child work order line (55559) on the parent work order, 
						//when the child work order is re approved (moved from "Waiting BM Approval" status to "Waiting To Be Invoiced")
						if (wo.parent_woprog_id > 1)
                        {
                            var sql = $@"SELECT IFNULL(MAX(wo_detail_current_id), 0) id
                                        FROM
                                            wo_detail_current wodc
                                        WHERE
                                            wodc.wo_detail_current_origin ='{OpsWOLineOrigin.ChildWO}'
                                            AND wodc.wo_detail_current_woprog_id = @v0
                                            AND wodc.wo_detail_current_code = '{OpsSpecialPart.NewChildWO}'
                                            AND wodc.child_woprog_id = @v1
                                        LIMIT 1";

                           var childWoLineId = Toolbox.doSQL_int(nesi_conn, sql, new object[]{ wo.parent_woprog_id, wo.woprog_id});
                           if (childWoLineId > 0)
                           {
                               NeWODetailCurrent.delete_workorder_line(childWoLineId, OpsSpecialPart.NewChildWO, user);
                           }
                        }

                        //insert db
                        Toolbox.doSQL_void(nesi_conn, @"INSERT INTO WOProgStatus (WOProgStatus_WOProg_ID,WOProgStatus_Member_ID,WOProgStatus_Status,WOProgStatus_DateTime) VALUES (@v0,@v1,'Waiting For PO',NOW())", new object[] {wo.woprog_id, user.id});
                        Toolbox.doSQL_void(nesi_conn, @"UPDATE WOProg set WOProg_Status='Waiting For PO' WHERE WOProg_ID=@v0", new object[] {wo.woprog_id});

                        Toolbox.doSQL_void(nesi_conn, string.Format(@"UPDATE passport SET active = false, valid = true WHERE url_yes LIKE ""%woprog_id={0}&%""", wo.woprog_id), null);
                        #endregion Waiting For PO
                        break;
                   case OpsWOStatus.Enums.WaitingParentBMApproval:
							#region Waiting Parent BM Approval
							add_history(wo.woprog_id, user.id, this_status, Toolbox.MySQLNow_long());
							wo.Status = this_status;
							wo.SaveWorkOrder();

							Toolbox.doSQL_void(nesi_conn, @"UPDATE passport SET active = false, valid = true WHERE url_yes LIKE CONCAT('%woprog_id=',@v0,'&%')", new object[] { wo.woprog_id });
							#endregion Waiting Parent BM Approval
							break;
						case OpsWOStatus.Enums.WaitingToBeInvoiced:
							#region Waiting to be Invoiced
							if(wo.inspection_required && string.IsNullOrWhiteSpace(wo.inspection_link))
							{
								throw new Exception("Invalid Inspection Link");
							}
							var addressid = Toolbox.doSQL_string(nesi_conn, @"SELECT woprog_address_id FROM woprog  WHERE woprog_id =@v0", new object[] { wo.woprog_id });
							var poreq = Toolbox.doSQL_int(nesi_conn, @"SELECT IFNULL(MAX(po_required), 0) FROM customer_sales_properties  WHERE address_id =@v0", new object[] { addressid });
							if (wo.parent_woprog_id > 1 && new NeWOProg(wo.parent_woprog_id).business_unit_id.ToString() == user.id.ToString())
							{
								#region check to see if its a child work order - then send it to the parent bm to approve
								send_invoice_warning_child_wos(wo.woprog_id);
								add_history(wo.woprog_id, user.id, OpsWOStatus.WaitingParentBMApproval, Toolbox.MySQLNow_long());
								wo.Status = OpsWOStatus.WaitingParentBMApproval;
								wo.SaveWorkOrder();
								#endregion
							}
							else if (poreq == 1 && wo.PONumber.Trim() == "" && wo.parent_woprog_id == 0)
							{
								#region PO Required
								#region Transfer Items from History Back to Current
								var move = move_to_current != "" && move_to_current == "yes";
								if (move)
								{
									var bs = NeWODetailCurrent.can_move_to_current(wo.woprog_id);
									if (bs.success)
									{
										NeWODetailCurrent.move_to_current(wo.woprog_id);
									}
									else
									{
										throw new Exception(bs.message);
									}
								}
								#endregion Transfer Items from History Back to Current
								//insert db
								add_history(wo.woprog_id, user.id, OpsWOStatus.WaitingForPO, Toolbox.MySQLNow_long());
								wo.Status = OpsWOStatus.WaitingForPO;
								wo.SaveWorkOrder();
								throw new Exception("This customer requires a PO number before the work order can be approved. The work order had been moved to waiting for customer PO.");
								#endregion PO Required
							}
							else
							{
								update_header_totals(wo.woprog_id.ToString(), wo.business_unit_id, wo.OrderNumber);
								var bs = NeWODetailHistory.can_move_to_history(wo.woprog_id);
								if (bs.success)
								{
									NeWODetailHistory.move_to_history(wo.woprog_id);
								}
								else
								{
									throw new Exception(bs.message);
								}
								//insert db
								try
								{
									add_history(wo.woprog_id, user.id, OpsWOStatus.WaitingToBeInvoiced, Toolbox.MySQLNow_long());
								}
								catch (Exception ee)
								{
									shared.alert_debug("Work order status bug from ticket 9539", ee.ToString());
								}
								Toolbox.doSQL_void(nesi_conn, @"UPDATE woprog SET woprog_status='Waiting To Be Invoiced' WHERE woprog_id=@v0  LIMIT 1", new object[] { wo.woprog_id });
								Toolbox.doSQL_void(nesi_conn, @"UPDATE passport SET active = false, valid = true WHERE url_yes LIKE CONCAT('%woprog_id=',@v0,'&%')", new object[] { wo.woprog_id });
							}
							// Check if this WO is a Progress bill, if true then update the timestamp on the Jobcost 
							if(wo.woprog_associate_woprog_id > 0)
							{
								Toolbox.doSQL_void(nesi_conn, @"UPDATE woprog SET woprog_ts=DATE_ADD(NOW(), INTERVAL 1 SECOND) WHERE woprog_id=@v0 LIMIT 1", new object[] { wo.woprog_associate_woprog_id });
							
							}

							#endregion Waiting to be Invoiced
							break;
						case OpsWOStatus.Enums.Invoiced:
							if (wo.Status != OpsWOStatus.WaitingToBeInvoiced) return;
							var invoiceTax = 0D;
							var invoiceNumber = "";
							var invoiceNetTotal = 0D;
							var lag = 0D;
							
							// Until then I am assuming only netsuite stuff is touching this.
							Toolbox.doSQL_void(nesi_conn, @"
UPDATE 
	woprog 
SET 
	woprog_status = 'Invoiced',
	woprog_closedatetime = NOW(),
	woprog_invoicedate = NOW(),
	woprog_invoice_tax = @v0, 
	woprog_invoiceno = @v1, 
	woprog_invoicednettotal = @v2, 
	woprog_invoicebalance = @v2, 
	lag = @v3  
WHERE 
	woprog_id = @v4 ", new object[] { invoiceTax, invoiceNumber, invoiceNetTotal, lag, wo.woprog_id });
							add_history(wo.woprog_id, 9999, OpsWOStatus.Invoiced, Toolbox.MySQLNow_long());
							break;
					}
			}
		}
		public static void EmailInvoice(MemoryStream file, NeWOProg _wo, NeMember _member)
		{
			var attachment = new Attachment(file, (_wo.woprog_InvoiceNo == ""
													  ? "Work Order: " + _wo.woprog_id
													  : "Invoice: " + _wo.woprog_InvoiceNo) + ".pdf");

			var oakvilleBusinessUnit = 1; // Choosing Oakville as it's historically always been the main branch.
			var bu = new NeBusinessUnit(_wo.business_unit_id);
			var modelBusinessUnit = _wo.business_unit_id != oakvilleBusinessUnit 
										? new NeBusinessUnit(oakvilleBusinessUnit) 
										: bu;
			
			
			var body = @"<div style=""font-family:'Segoe UI',Calibri,sans-serif;font-size:1em;"">Please find the attached invoice.  <br/> If you have any questions, please don't hesitate to call us at (800) 204-4153.<br/><br/>";
			body += bu.country != "USA"
				? $@"Please remit payment to:<br/>{bu.name}<br/>{modelBusinessUnit.address}</br>{modelBusinessUnit.city}, {modelBusinessUnit.provstate}, {modelBusinessUnit.country}<br/>{modelBusinessUnit.postal}</div>"
				: "</div>";
			var message = new NeEMail();
			//{
			//	To = _wo.net_total <= 0
			//					  ? "aradmin@" + Toolbox.app_setting("DomainForEmail")
			//					  : "aradmin@" + Toolbox.app_setting("DomainForEmail"),
			//	From = "AR@" + Toolbox.app_setting("DomainForEmail"),
			//	isHTML = true,
			//	Subject = string.Format("Invoice: {0} from {1}", _wo.woprog_InvoiceNo, bu.name),
			//	Body = body,
			//	Attachment = attachment
			//};
			if (Toolbox.app_setting("debug_redirect") == "1")
				message.To = Toolbox.app_setting("debug_redirect_email");
			else
				message.To = _wo.net_total <= 0
								  ? "aradmin@" + Toolbox.app_setting("DomainForEmail")
								  : "aradmin@" + Toolbox.app_setting("DomainForEmail");

				message.From = "AR@" + Toolbox.app_setting("DomainForEmail");

			message.isHTML = true;
			message.Subject = string.Format("Invoice: {0} from {1}", _wo.woprog_InvoiceNo, bu.name);
			message.Body = body;
			message.Attachment = attachment;

			message.Send();
			Toolbox.doSQL_void(@"UPDATE woprog SET woprog_last_email_date = now() WHERE woprog_id = @v0 ", new object[] { _wo.woprog_id });
			Toolbox.doSQL_void(@"INSERT INTO collection_history ( date, woprog_id, member_id, action ) VALUES ( NOW(), @v1 , @v0 , 'Original Invoice Emailed' )", new object[] { _member.id, _wo.woprog_id });
		}
		public static void send_invoice_warning_child_wos(int woprog_id)
		{
			var wo = new NeWOProg(woprog_id);
			foreach (DataRow dr_child_wos in get_child_workorders(Convert.ToInt32(wo.woprog_id)).Rows)
			{
				var status = dr_child_wos["woprog_status"].ToString();
				if (status != OpsWOStatus.Invoiced && status != OpsWOStatus.WaitingToBeInvoiced && status != "Deleted")
				{
					try
					{
						var child_wo = new NeWOProg(Convert.ToInt32(dr_child_wos["woprog_id"].ToString()));
						//var email_child_pm = new NeEMail
						//{
						//	To = new NeMember(child_wo.intProjectManager).NEEmail,
						//	CC = new NeBusinessUnit(child_wo.business_unit_id).branch_manager.NEEmail,
						//	Subject = "WO " + wo.OrderNumber + " from " + new NeBusinessUnit(wo.business_unit_id).name +
						//								   " is being invoiced.. your WO " + child_wo.OrderNumber +
						//								   " needs to be finalized ASAP.",
						//	isHTML = true,
						//	Body = @"<a href='"+Toolbox.app_setting("Domain") +"/sections/workorder/index.aspx?woprog_id="+dr_child_wos["woprog_id"].ToString().Trim()+"'>Click Here to Go To Work Order</a>",
						//	From = "admin@" + Toolbox.app_setting("DomainForEmail")
						//};
						//email_child_pm.Send();
						var email_child_pm = new NeEMail();
						if (Toolbox.app_setting("debug_redirect") == "1")
							email_child_pm.To = Toolbox.app_setting("debug_redirect_email");
						else
							email_child_pm.To = new NeMember(child_wo.intProjectManager).NEEmail;

						email_child_pm.CC = new NeMember(child_wo.intProjectManager).NEEmail;

						email_child_pm.Subject = "WO " + wo.OrderNumber + " from " + new NeBusinessUnit(wo.business_unit_id).name +
														   " is being invoiced.. your WO " + child_wo.OrderNumber +
														   " needs to be finalized ASAP.";
						email_child_pm.isHTML = true;

						email_child_pm.Body = @"<a href='" + Toolbox.app_setting("Domain") + "/sections/workorder/index.aspx?woprog_id=" + dr_child_wos["woprog_id"].ToString().Trim() + "'>Click Here to Go To Work Order</a>";
						email_child_pm.From = "admin@" + Toolbox.app_setting("DomainForEmail");
						email_child_pm.Send();
					}
					catch (Exception ee)
					{
						Toolbox.do_errorLog_errorStack(ee);
					}
				}
			}

		}
        public static void ValidateTEStructureInvoiceFolder(int buid)
        {
            var baseTEStructure = NeTaxEntity.BaseFolder(buid, false);
            var invoicedFolderStructure = Path.Combine(baseTEStructure, "WOs/Invoiced");
            if(!Directory.Exists(invoicedFolderStructure))
            {
                Directory.CreateDirectory(invoicedFolderStructure);
            }
        }
		public static void consolidate_project_folders(int _id, string _number, string _correct_customer_name)
		{
			var nef = new NeFiles();
			var wo = new NeWOProg(_id);
			var fileServer = NeTaxEntity.BaseFolder(wo.business_unit_id, false);
			var base_folder = fileServer + @"\ProjectFolders";
			var matching_folders = Directory.GetDirectories(base_folder, string.Format("WO{0}-*", _id));
			var to_folder = string.Format("WO{0}-{1}-{2}", _id, _number, NeFiles.clean_filename(_correct_customer_name));
			var to_path = Path.Combine(base_folder, to_folder);
			foreach (var m in matching_folders)
			{
				if (m == to_path) continue;
				nef.copy_all(new DirectoryInfo(m), new DirectoryInfo(to_path), true);
			}
		}
		public class Jobtag
		{
			public string name { get; set; }
			public int created_by { get; set; }
			public DateTime created_dt { get; set; }
			public int netsuite_id { get; set; }
			public Jobtag(int _id)
			{
				load(_id);
			}
			private void load(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var jt = uow.GetObjectByKey<ne_xpo.cs.woprog_jobtag>(_id);
					if (jt == null) return;
					name = jt.name;
					created_by = Convert.ToInt32(jt.created_by);
					created_dt = jt.created_dt;
					netsuite_id = jt.netsuite_id;
				}
			}
		}
	}
	public class woprog_comment
	{
		/*
		woprogcomment_id
		woprogcomment_woprog_id
		woprogcomment_member_id
		woprogcomment_text
		woprogcomment_x
		woprogcomment_y
		woprogcomment_datetime
		woprogcomment_deleted
		 */
		public int id { get; set; }
		public int woprog_id { get; set; }
		public int member_id { get; set; }
		public string text { get; set; }
		public DateTime datetime { get; set; }
		public string deleted { get; set; }
		public woprog_comment() { }
		public woprog_comment(int _id)
		{
			if (exists(_id))
			{
				load(_id);
			}
		}
		private bool exists(int _id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM woprogcomment WHERE woprogcomment_id = @v0 ", new object[] { _id }) > 0;
		}
		private void load(int _id)
		{
			var _dt = Toolbox.doSQL_dt(@"SELECT * FROM woprogcomment WHERE woprogcomment_id = @v0 ", new object[] { _id });
			if (_dt.Rows.Count > 0)
			{
				var _dr = _dt.Rows[0];
				woprog_id = Convert.ToInt32(_dr["woprogcomment_woprog_id"]);
				member_id = Convert.ToInt32(_dr["woprogcomment_member_id"]);
				text = _dr["woprogcomment_text"].ToString();
				datetime = Convert.ToDateTime(_dr["woprogcomment_datetime"]);
				deleted = _dr["woprogcomment_deleted"].ToString();
			}
		}
		public void save()
		{
			if (id == 0) // Insert
			{
				id = Toolbox.doSQL_return_id(@" INSERT INTO woprogcomment ( woprogcomment_woprog_id, woprogcomment_member_id, woprogcomment_text, woprogcomment_datetime, woprogcomment_deleted ) VALUES ( @v0 , @v1 , @v2 , NOW(), @v3  )", new object[] { woprog_id, member_id, text, deleted });
			}
		}
	}
	public class wo_associated
	{
		public int id { get; set; }
		public int woprog_id_a { get; set; }
		public int woprog_id_b { get; set; }
		public bool loaded { get; set; }
		public bool saved { get; set; }
		public wo_associated() { }
		public wo_associated(int _id)
		{
			load(_id);
		}
		public bool exists(int _id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM wo_associated WHERE id = @v0 ", new object[] { _id }) > 0;
		}
		private void load(int _id)
		{
			if (exists(_id))
			{
				var _dt = Toolbox.doSQL_dt(@"SELECT * FROM wo_associated WHERE id = @v0 ", new object[] { _id });
				if (_dt.Rows.Count > 0)
				{
					var _dr = _dt.Rows[0];
					id = _id;
					woprog_id_a = Convert.ToInt32(_dr["woprog_id_a"]);
					woprog_id_b = Convert.ToInt32(_dr["woprog_id_b"]);
					loaded = true;
				}
			}
		}
		public void save()
		{
			if (id == 0) // Insert
			{
				if (woprog_id_a > 0 && woprog_id_b > 0)
				{
					id = Toolbox.doSQL_return_id(@"INSERT INTO wo_associated (woprog_id_a, woprog_id_b) VALUES (@v0 , @v1 )", new object[] { woprog_id_a, woprog_id_b });
					saved = true;
				}
			}
			else // Update
			{
				if (woprog_id_a > 0 && woprog_id_b > 0)
				{
					Toolbox.doSQL_void(@"UPDATE wo_associated SET woprog_id_a = @v0  AND woprog_id_b = @v1  WHERE id = @v2  LIMIT 1", new object[] { woprog_id_a, woprog_id_b, id });
					saved = true;
				}
			}
		}
		public void delete()
		{
			Toolbox.doSQL_void(@"DELETE FROM wo_associated WHERE id = @v0  LIMIT 1", new object[] { id });
		}
		public static void delete(int woprog_id_a, int woprog_id_b)
		{
			if (woprog_id_a != 0 && woprog_id_b != 0)
			{
				Toolbox.doSQL_void(@"DELETE FROM wo_associated WHERE (woprog_id_a = @v0  AND woprog_id_b = @v1 ) OR (woprog_id_a = @v1  AND woprog_id_b = @v0 )", new object[] { woprog_id_a, woprog_id_b });
			}
		}
	}
}