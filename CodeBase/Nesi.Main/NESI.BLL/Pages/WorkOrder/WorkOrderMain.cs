using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.Common.Models;
using NESI.DTO.ViewModels.Page.WorkOrder;

// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace NESI.BLL.Pages.WorkOrder
{
	public class WorkOrderMain : WorkOrderEdit
	{
		public WorkOrderMain(Employee user, int buid, string woid) : base(user, buid, woid)
		{
		}

		public object Profile()
		{
			var entity = new DTO.ViewModels.Page.WorkOrder.WorkOrderMainHeader();
			wo.fast_update_header_totals();
			entity = (DTO.ViewModels.Page.WorkOrder.WorkOrderMainHeader)MapperFrom(entity, wo);
			entity.taxes = wo.TAX_LABOUR + wo.TAX_MATERIAL;

			if (CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault(false))
			{
				entity.taxes_adj = wo.TAX_LABADJUSTED + wo.TAX_MATADJUSTED;
			}
			else
			{
				entity.taxes_adj = 0;
			}
			entity.prev_woprog_id = wo.woprog_id > 0 ? bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(woprog_id), 0) FROM woprog WHERE woprog_id < @v0  AND business_unit_id = @v1  LIMIT 1", wo.woprog_id, wo.business_unit_id) : 0;
			entity.next_woprog_id = wo.woprog_id > 0 ? bllToolbox.doSQL_int(@"SELECT IFNULL(MIN(woprog_id), 0) FROM woprog WHERE woprog_id > @v0  AND business_unit_id = @v1  LIMIT 1", wo.woprog_id, wo.business_unit_id) : 0;
			if (!_can_view_gross)
			{
				entity.woprog_grossmargin = 0;
				entity.margin_whole = 0;
			}
			else
			{
				var tablename = "current";
				if (wo.Status != "Waiting Parent BM Approval" && wo.Status != "Waiting For PO" && wo.Status != "Waiting Parent BM Approval")
				{
					tablename = "history";
				}
				if (wo.QuoteID != "0")
				{
					var margin_whole = Get_margin_whole_job(Convert.ToDouble(wo.QuotedPrice), tablename);
					margin_whole = margin_whole.ToString().Contains("E") || margin_whole.ToString().Contains("Infinity")
						? 0
						: margin_whole;
					entity.margin_whole = margin_whole;
				}
				else
				{
					entity.margin_whole = 0;
				}
			}
			entity.Description = entity.Description.Replace("\u0000", "");
			var pdf_path = GetPdf_Path();
			if (wo.Status != "Invoiced" && wo.Status != OpsWOStatus.Open && wo.Status != "To Be Invoiced")
			{
				if (Toolbox.MySQL_shortdt(wo.woprog_scanned_date) == "2005-12-01" || Toolbox.MySQL_shortdt(wo.woprog_scanned_date) == "")
				{
					entity.day_since_scan = "This has not been scanned.";
				}
				else
				{
					var i = ((TimeSpan)(DateTime.Now.Date - wo.woprog_scanned_date.Date)).Days;
					entity.day_since_scan = i + " day(s) since being scanned ";
					entity.day_since_scan += i > 5 ? " !!" : i >= 10 ? " !!!" : "";
				}
				if (wo.woprog_iscredit == 1)
				{
					entity.day_since_scan += " **CREDIT **";
				}
				else if (wo.woprog_isrebill == 1)
				{
					entity.day_since_scan += " **REBILL **";
				}
			}
			else
			{
				entity.day_since_scan = "";
			}

			entity.ts_ticks = GetTsTicket();
			entity.comment_visible = wo.Status != "Invoiced" && wo.Status != "To Be Invoiced";
			entity.cant_delete_reason = GetCantDeleteReason();
			var button_saveGeneral = new WorkOrderButtonStyle
			{
				id = "saveGeneral",
				label = "Save",
				icon = "fa-save",
				tooltip = "Save / Create this Work Order",
				visible = true,
				enabled = true
			};


			var button_invoicePreview = new WorkOrderButtonStyle
			{
				id = "invoicePreview",
				label = "Preview",
				icon = "fa-eye",
				tooltip = "Invoice Preview",
				visible = true,
				enabled = true
			};
			var button_sendPMForQuestions = new WorkOrderButtonStyle
			{
				id = "sendPMForQuestions",
				label = "Questions",
				icon = "fa-question-circle",
				tooltip = "Send To Questions Column",
				parent = "Send To",
				visible = true,
				enabled = true
			};
			var button_sendToRework = new WorkOrderButtonStyle
			{
				id = "sendToRework",
				label = "Rework",
				icon = "fa-redo",
				tooltip = "Send to Rework Column",
				parent = "Send To",
				visible = true,
				enabled = true
			};


			var button_delete = new WorkOrderButtonStyle
			{
				id = "delete",
				label = "Delete",
				icon = "fa-times",
				tooltip = "Delete WO",
				visible = true,
				enabled = true
			};
			var button_refresh = new WorkOrderButtonStyle
			{
				id = "refresh",
				label = "Refresh",
				icon = "fa-sync",
				tooltip = "Refresh WO",
				visible = true,
				enabled = true
			};

			var button_print = new WorkOrderButtonStyle
			{
				id = "print",
				label = "Print",
				icon = "fa-print",
				tooltip = "Print WO",
				visible = true,
				enabled = true
			};

			var button_back = new WorkOrderButtonStyle
			{
				id = "back",
				label = "Back",
				icon = "fa-backward",
				tooltip = "Back to previous page",
				visible = true,
				enabled = true
			};

			var button_signOff = button_logic("signOff");
			var button_sendToPMApproval = new WorkOrderButtonStyle(); //button_logic("sendToPMApproval");
			var button_sendToWaitCustPO = new WorkOrderButtonStyle(); //button_logic("sendToWaitCustPO");
			var button_sendToWaitingToBeInvoiced = new WorkOrderButtonStyle(); // button_logic("sendToWaitingToBeInvoiced");
			var button_approvedByPM = new WorkOrderButtonStyle(); // button_logic("approvedByPM");
			var un_po = bllToolbox.doSQL_int(@"SELECT COUNT(po_details_id) FROM po_details_current WHERE po_details_woprog_id = @v0 
AND po_details_line_active = 1 and is_gl_account = false", woprog_id) > 0;

			switch (wo.Status)
			{
				case "Invoiced":
					button_saveGeneral.visible = false;
					button_approvedByPM.visible = false;
					button_invoicePreview.visible = _can_see_invoice_preview_button;
					button_sendPMForQuestions.visible = false;
					button_sendToPMApproval.visible = false;
					button_sendToRework.visible = false;
					button_sendToWaitCustPO.visible = false;
					button_sendToWaitingToBeInvoiced.visible = false;
					button_delete.visible = false;
					break;
				#region Waiting to be Invoiced
				case OpsWOStatus.WaitingToBeInvoiced:
					button_saveGeneral.visible = false;
					button_approvedByPM.visible = false;
					button_invoicePreview.visible = _can_see_invoice_preview_button;
					button_sendPMForQuestions.visible = false;
					button_sendToPMApproval.visible = false;
					button_sendToRework.visible = false;
					button_sendToWaitCustPO = button_logic("sendTOWaitCustPO");
					button_sendToWaitingToBeInvoiced.visible = false;
					break;
				#endregion Waiting to be Invoiced
				#region Open
				case OpsWOStatus.Open:
					button_saveGeneral.visible = true;
					button_approvedByPM.visible = false;
					button_invoicePreview.visible = false;
					button_sendPMForQuestions.visible = false;
					button_sendToPMApproval.visible = false;
					button_sendToRework.visible = false;
					button_sendToWaitCustPO.visible = false;
					button_sendToWaitingToBeInvoiced.visible = false;
					button_signOff.visible = false;
					break;
				#endregion Open
				#region Waiting PM Approval
				case "Waiting PM Approval":
					button_saveGeneral.visible = true;
					button_invoicePreview.visible = _can_see_invoice_preview_button;
					button_sendPMForQuestions.visible = false;
					button_sendToPMApproval.visible = false;
					button_sendToRework.visible = true;
					//button_logic(bt_signoff);
					button_sendToWaitCustPO.visible = false;
					if (un_po)
					{
						button_sendToWaitingToBeInvoiced = button_logic("sendToWaitingToBeInvoiced");
						button_sendToWaitCustPO = button_logic("sendToWaitCustPO");
						button_approvedByPM = button_logic("approvedByPM");
					}
					break;
				#endregion Waiting PM Approval
				#region Waiting BM Approval
				case "Waiting BM Approval":
					button_saveGeneral.visible = true;
					button_approvedByPM.visible = false;
					//button_logic(bt_signoff);
					button_invoicePreview.visible = _can_see_invoice_preview_button;
					button_sendToWaitCustPO.visible = false;
					if (un_po)
					{
						button_sendPMForQuestions.visible = true;
						button_sendToWaitingToBeInvoiced = button_logic("sendToWaitingToBeInvoiced");
						button_sendToWaitCustPO = button_logic("sendToWaitCustPO");
						button_approvedByPM = button_logic("approvedByPM");
					}
					else
					{
						button_sendPMForQuestions.visible = true;
					}
					button_sendToRework.visible = true;
					break;
				#endregion Waiting BM Approval
				#region In Progress / Initial Prep
				case "In Progress":
				case "Initial Prep":
					button_saveGeneral.visible = true;
					button_invoicePreview.visible = _can_see_invoice_preview_button;
					button_sendToWaitCustPO.visible = false;
					if (un_po)
					{
						button_sendPMForQuestions.visible = true;
						button_sendToWaitingToBeInvoiced = button_logic("sendToWaitingToBeInvoiced");
						button_sendToWaitCustPO = button_logic("sendToWaitCustPO");
						button_approvedByPM = button_logic("approvedByPM");
						button_sendToPMApproval = button_logic("sendToPMApproval");
					}
					button_sendPMForQuestions.visible = true;
					button_sendToRework.visible = true;
					if (wo.woprog_associate_woprog_id != 0 && wo.Status == "Initial Prep")
					{
						if (!_can_edit_2139_costs)
						{
							button_approvedByPM.visible = false;
							button_invoicePreview.visible = false;
							button_sendPMForQuestions.visible = false;
							button_sendToPMApproval.visible = false;
							button_sendToRework.visible = false;
							button_sendToWaitCustPO.visible = false;
							button_sendToWaitingToBeInvoiced.visible = false;
							button_signOff.visible = false;
						}
					}
					break;
				#endregion In Progress / Initial Prep
				#region Questions For PM
				case "Questions For PM":
					button_saveGeneral.visible = true;
					button_invoicePreview.visible = _can_see_invoice_preview_button;
					button_sendPMForQuestions.visible = false;
					button_sendToRework.visible = true;
					button_sendToWaitCustPO.visible = false;
					if (un_po)
					{
						button_sendToWaitingToBeInvoiced = button_logic("sendToWaitingToBeInvoiced");
						button_sendToWaitCustPO = button_logic("sendToWaitCustPO");
						button_approvedByPM = button_logic("approvedByPM");
						button_sendToPMApproval = button_logic("sendToPMApproval");
					}
					break;
				#endregion Questions For PM
				#region Waiting For PO
				case "Waiting For PO":
					button_saveGeneral.visible = true;
					button_approvedByPM.visible = false;
					button_invoicePreview.visible = _can_see_invoice_preview_button;
					button_sendPMForQuestions.visible = false;
					button_sendToPMApproval.visible = false;
					button_sendToRework.visible = CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault(false);
					button_sendToWaitCustPO.visible = false;
					if (un_po)
					{
						button_sendToWaitingToBeInvoiced = button_logic("sendToWaitingToBeInvoiced");
					}
					else
					{
						button_sendPMForQuestions.visible = true;
					}

					break;
				case "Waiting Parent BM Approval":
					button_saveGeneral.visible = true;
					button_approvedByPM.visible = false;
					button_invoicePreview.visible = _can_see_invoice_preview_button;
					button_sendPMForQuestions.visible = false;
					button_sendToPMApproval.visible = false;
					button_sendToRework.visible = wo.business_unit_id == business_unit_id;
					button_sendToWaitCustPO.visible = false;

					break;
				#endregion "Waiting For PO"
				#region Rework
				case "Rework":
					button_saveGeneral.visible = true;
					button_invoicePreview.visible = _can_see_invoice_preview_button;
					button_sendToRework.visible = false;
					//button_logic(bt_signoff);
					button_sendToWaitCustPO.visible = false;
					if (un_po)
					{
						button_approvedByPM = button_logic("approvedByPM");
						button_sendPMForQuestions.visible = true;
						button_sendToPMApproval = button_logic("sendToPMApproval");
						button_sendToWaitingToBeInvoiced = button_logic("sendToWaitingToBeInvoiced");
					}
					else
					{
						button_sendPMForQuestions.visible = true;
					}
					break;
					#endregion Rework
			}

			var buttons = new
			{
				button_back,
				button_saveGeneral,
				button_invoicePreview,
				button_print,
				button_approvedByPM,
				button_delete,
				button_signOff,
				button_send = new
				{
					button_sendToWaitCustPO,
					button_sendPMForQuestions,
					button_sendToPMApproval,
					button_sendToRework,
					button_sendToWaitingToBeInvoiced
				}
			};


			return new
			{
				rename_file_enabled = pdf_path != @"/images/NoScan.pdf",
				pdf_path,
				entity,
				buttons
			};
		}

		private WorkOrderButtonStyle button_logic(string _b)
		{
			var error_base = "";
			var button = new DTO.ViewModels.Page.WorkOrder.WorkOrderButtonStyle { id = _b };
			switch (_b)
			{
				case "sendToWaitingToBeInvoiced":
					error_base = All_Systems_Go("Invoice");
					button.label = "Waiting to be Invoiced";
					button.icon = "fa-file-alt";
					button.parent = "Send To";
					if (_can_approve_bm || wo.Status == "Waiting For PO" && _can_approve_pm || _can_approve_dm && _same_dept)
					{
						button.visible = true;
						button.enabled = error_base == "";
						if (wo.woprog_creditcard_payment == 0)
						{
							button.tooltip = !button.enabled ? error_base.Replace("\\n", " -- ") : "Final approval, go ahead and invoice";
							//button_sendToWaitingToBeInvoiced.Attributes["onclick"] = button_sendToWaitingToBeInvoiced.Enabled ? base_script + ",'action=approveBranch','','toinvoice');" : "alert('" + error_base + "');";
						}
						else
						{
							button.tooltip = !button.enabled ? error_base.Replace("\\n", " -- ") : "Final approval, this will go to cust po to process the credit card";
							//button_sendToWaitingToBeInvoiced.Attributes["onclick"] = button_sendToWaitingToBeInvoiced.Enabled ? base_script + ",'action=waitpo','','waitpo');" : "alert('" + error_base + "');";
							if (CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault())  // if it's a nesi staffer
							{
								button.tooltip = !button.enabled ? error_base.Replace("\\n", " -- ") : "Final Approval, credit card has been processed, send it to waiting to be invoiced..";
								//button_sendToWaitingToBeInvoiced.Attributes["onclick"] = button_sendToWaitingToBeInvoiced.Enabled ? base_script + ",'action=approveBranch','','toinvoice');" : "alert('" + error_base + "');";
							}
						}
						if (error_base != "")
						{
							//var error_link = error_base.Contains("less than cost") ? "Click <a href=\"javascript:boing('./index.aspx?woprog_id=" + hidWOProgID.Value + "&a=less_than_cost', 'quote', 750,400);\">here</a> to fill out why parts are being sold at less than cost <br/>" : "";
							button.err_msg = error_base;
						}
					}
					break;

				case "approvedByPM":
					error_base = All_Systems_Go("");
					button.label = "Save";
					button.icon = "fa-thumbs-up";

					if (_can_approve_pm)
					{
						button.visible = true;
						button.enabled = error_base == "";
						button.tooltip = !button.enabled ? error_base.Replace("\\n", " -- ") : "Approve (by PM) to next step!";
						//button_approvedByPM.Attributes["onclick"] = button_approvedByPM.Enabled ? base_script + ",'action=approve');" : "alert('There are errors with this work order that need to be addressed.\\n Please go over the issues column in the line items tab.');";
						if (error_base != "")
						{
							//var error_link = error_base.Contains("less than cost") ? "Click <a href=\"javascript:boing('./index.aspx?woprog_id=" + hidWOProgID.Value + "&a=less_than_cost', 'quote', 750,400);\">here</a> to fill out why parts are being sold at less than cost <br/>" : "";
							//lblError.Text = error_link + error_base.Replace("\\n", "<br/>");
							button.err_msg = error_base;
						}
					}

					break;
				case "sendToWaitCustPO":
					button.label = "Waiting Customer PO";
					button.icon = "fa-shopping-cart";
					button.tooltip = "Send to Waiting Customer PO Column";
					button.parent = "Send To";

					error_base = All_Systems_Go("");
					button.visible = CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault(false) || CurrentUser.MemberType.membertype_id == 5;
					button.enabled = error_base == "" || error_base.Contains("incomplete PO lines");
					//if (wo.Status == OpsWOStatus.WaitingToBeInvoiced)
					//{
					//	//button_sendToWaitCustPO.Attributes["onclick"] = button_sendToWaitCustPO.Enabled && _current_user.business_unit_id == 11 ? base_script + ",'action=waitpo','movetocurrent=yes');" : "alert('There are errors with this work order that need to be addressed.\\n Please go over the issues column in the line items tab.');";
					//}
					//else
					//{
					//	//button_sendToWaitCustPO.Attributes["onclick"] = button_sendToWaitCustPO.Enabled && _current_user.business_unit_id == 11 ? base_script + ",'action=waitpo','','waitpo');" : "alert('There are errors with this work order that need to be addressed.\\n Please go over the issues column in the line items tab.');";
					//}
					if (!button.enabled && button.visible)
					{
						button.tooltip = error_base.Replace("\\n", " -- ");
					}
					if (error_base != "")
					{
						//lblError.Text = error_base.Replace("\\n", "<br/>");
						button.err_msg = error_base;
					}
					break;
				case "sendToPMApproval":
					button.id = "sendToPMApproval";
					button.label = "Waiting PM Approval";
					button.icon = "fa-thumbs-up";
					button.tooltip = "Send to PM for their approval";
					button.parent = "Send To";

					error_base = All_Systems_Go("");
					button.visible = true;
					button.enabled = error_base == "";
					button.tooltip = !button.enabled ? error_base.Replace("\\n", " -- ") : "Send to PM for their approval";
					//button_sendToPMApproval.Attributes["onclick"] = button_sendToPMApproval.Enabled ? base_script + ",'action=waitapproval');" : "alert('There are errors with this work order that need to be addressed.\\n Please go over the issues column in the line items tab.');";
					if (error_base != "")
					{
						//lblError.Text = error_base.Replace("\\n", "<br/>");
						button.err_msg = error_base;
					}
					break;
				case "signOff":
					button.visible = true;
					button.label = "Sign Off";
					button.icon = "fa-pen-alt";
					button.enabled = true;
					//bt_signoff.Attributes["onclick"] = "boing('/sections/workorder/wosignoff.aspx?woid=" + _wo.woprog_id + "', 'signoff', 670,960)";
					//bt_signoff.Attributes["onclick"] = "boing('../../sections/reports/invoice_preview/index.aspx?id=" + _wo.woprog_id + "&is_signoff=true','sign_off',800,800)";
					//bt_signoff.Style.Add("cursor", "pointer");
					button.tooltip = "Print Sign Off Sheet";
					break;
			}

			return button;
		}

		protected string All_Systems_Go(string _type)
		{
			var issue = "";

			if (_type == "Invoice")
			{
				//	fill_page_info("General");
				var so = new NeSalesOrder();
				var bu = new NeBusinessUnit(wo.business_unit_id);
				var doBv = bu.DSN != "";
				so.DSN = bu.DSN;
				var temp_customer = new NECustomer((int)wo.WOProg_Customer_ID);
				var temp_address = new NEAddress(wo.woprog_Address_ID);
				string lockedby;
				//if (doBv && (lockedby = so.CheckOrderLock(wo.OrderNumber)) != "")
				//{
				//	return "Work order locked in bv by " + lockedby;
				//}
				//if ((temp_customer.Address.csp.po_required || temp_address.csp.po_required) && string.IsNullOrEmpty(txtCustPO.Text))
				//{
				//	return "Customer requires a valid purchase order before invoicing";
				//}
				var has_open_childs = false;
				if (NeWOProg.get_child_workorders(wo.woprog_id).Rows.Count > 0)
				{
					var drs = NeWOProg.get_child_workorders(wo.woprog_id).Select("woprog_status<>'Invoiced' and woprog_status<>'Deleted' and woprog_status<>'Waiting To Be Invoiced'");
					has_open_childs = drs.Any();
				}

				if (has_open_childs)
				{
					return "This is a parent work order that has open child work orders that need to be closed first.  Review on the linked work orders tab.";
				}
			}
			#region Active PO Lines Check
			var active_polines = bllToolbox.doSQL_int(@"SELECT COUNT(po_details_id) FROM po_details_current
WHERE po_details_line_active = 1 AND po_details_woprog_id = @v0 and po_details_current.is_gl_account = false AND po_details_part_no != 0", woprog_id);
			if (active_polines > 0)
			{
				//pc_main.ActiveTabIndex = 4;
				//fill_page_info("POs");
				return "There are incomplete PO lines related to this work order";
			}
			#endregion Active PO Lines Check
			#region AP Problems Check
			var ap_problems = bllToolbox.doSQL_int(@"SELECT COUNT(poprog_id) FROM poprog_header 
WHERE poprog_status = 10 AND poprog_id IN (SELECT po_details_poprog_id FROM po_details_current WHERE po_details_woprog_id = @v0 and po_details_current.is_gl_account = false )", woprog_id);
			if (ap_problems > 0)
			{
				//pc_main.ActiveTabIndex = 4;
				//fill_page_info("POs");
				return "There are linked PO's that are currenly in AP Problems";
			}
			#endregion AP Problems Check
			var dt = bllToolbox.doSQL_dt(@"Select (wo_detail_current_price_sell-wo_detail_current_price_cost) as spread, wo_detail_current_master_id master_id
from wo_detail_current  where wo_detail_current_woprog_id =@v0 AND IFNULL(wo_detail_current_notes,'') NOT LIKE 'A_%|%'", woprog_id);
			foreach (DataRow dr in dt.Rows)
			{
				if (Convert.ToDouble(dr["spread"]) < 0 && wo.woprog_iscredit == 0)
				{
					issue += dr["master_id"] + " is being sold at less than cost\\n";
				}
			}
			if (issue != "")
			{
				issue = "The following parts are being sold at less than cost: \\n" + issue;
				return issue;
			}
			//if (issue == "")
			//{
			//	lblError.Text = issue;
			//	lblError.ClientVisible = false;
			//}
			return issue;
		}



		private string[] GetCantDeleteReason()
		{
			var list = new List<string>();
			var curr_wo_count = bllToolbox.doSQL_dt(@"SELECT CAST((SELECT COUNT(*) FROM wo_detail_current
WHERE wo_detail_current_woprog_id = @v0 ) AS UNSIGNED) curr, CAST((SELECT COUNT(*) FROM wo_detail_history WHERE wo_detail_history_woprog_id = @v0 ) AS UNSIGNED) hist", woprog_id).Rows[0];
			var curr_wo_line_items = Convert.ToInt32(curr_wo_count["curr"]);
			var hist_wo_line_items = Convert.ToInt32(curr_wo_count["hist"]);
			var wo_po_links = bllToolbox.doSQL_dt(@" SELECT b.poprog_id, b.poprog_bvpo FROM po_details_current a
LEFT JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_woprog_id = @v0
and a.is_gl_account = false  AND a.po_details_line_active = 1 AND po_details_part_no != 0 
GROUP BY poprog_id, poprog_bvpo", woprog_id);
			var curr_wo_comm_lines = curr_wo_line_items != 0 && bllToolbox.doSQL_int(@"SELECT count(wo_detail_current_qty_committed) 
FROM wo_detail_current  WHERE wo_detail_current_qty_committed != 0 and wo_detail_current_woprog_id =@v0",
										 woprog_id) != 0;
			if (wo.Status == OpsWOStatus.WaitingToBeInvoiced || wo.Status == "Invoiced")
			{
				list.Add("Work orders in the '" + wo.Status + "' status can't be deleted");
			}
			if (hist_wo_line_items > 0)
			{
				list.Add("Work orders that have items in the history table can't be deleted");
			}
			if (curr_wo_comm_lines)
			{
				list.Add("This work order has items committed on it that need to be removed BEFORE it can be deleted");
			}
			if (wo_po_links.Rows.Count > 0)
			{
				var pos = "";
				foreach (DataRow dr in wo_po_links.Rows)
				{
					var poprog_id = dr["poprog_id"];
					var poprog_bvpo = dr["poprog_bvpo"];
					pos +=
						$"<br/><a style='color:#ffc' href='/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={poprog_id}'>PO #:{poprog_bvpo}</a>";
				}
				list.Add("This work order has open purchase order(s) with active line items linked to it that need to be deactivated or deleted BEFORE this work order can be deleted:" + pos);
			}

			return list.ToArray();
		}

		public long GetTsTicket()
		{
			return bllToolbox.doSQL_datetime(@"SELECT woprog_ts FROM woprog  WHERE woprog_id =@v0", woprog_id).Ticks;
		}

		protected double Get_margin_whole_job(double true_sell, string _tablename)
		{
			var strsql = string.Format(@"SELECT
ifnull(sum(wo_detail_{0}_qty_committed * wo_detail_{0}_price_cost),0)
FROM
wo_detail_{0}
WHERE
wo_detail_{0}_woprog_id = @v0 and 
wo_detail_{0}_billtypeid NOT IN (3,6,8,9,11,12)", _tablename);
			var true_cost = bllToolbox.doSQL_double(strsql, wo.woprog_id);

			return (true_sell - true_cost) / true_sell;
		}
	}
}