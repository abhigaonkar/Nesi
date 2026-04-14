using System;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using DevExpress.Xpo;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Employees;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeFootPrints : EmployeeEdit
	{
		public EmployeeFootPrints(Employee current_user, int mid) : base(current_user, mid)
		{
			can_access = tab_enabled[10];
		}

		private LabelValueStringCount[] GetTabList()
		{
			return bllToolbox.doSQL_List<LabelValueStringCount>(@"CALL `get_foot_prints_count`(@v0)", member_id)
				.Where(x => x.count > 0)
				.OrderBy(x => x.Label).ToArray();
		}

		public override object Profile()
		{
			var tabs = GetTabList();
			EmployeeFootPrintListItem options = null;
			if (tabs.Length > 0)
			{
				options = GetOptions(tabs[0].Value);
			}

			var employee_list = bllToolbox.doSQL_dt(@" SELECT a.member_id value,
CONCAT('(', b.name,') ',member_fullname) label FROM member a
LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE a.member_status = 'Active' 
AND a.member_id != @v1  ORDER BY b.name, a.member_lastname, a.member_firstname", n1_member.business_unit_id, member_id);

			var employee_list_tickets = bllToolbox.doSQL_dt(@" SELECT a.member_id value,
CONCAT('(', c.name, ') ',a.member_fullname) label FROM member a INNER JOIN ticketmanager b 
ON b.ticketmanager_member_id = a.Member_ID LEFT JOIN business_unit c ON a.business_unit_id = c.id 
WHERE a.member_id != @v0  and a.member_status = 'Active'
GROUP BY a.member_id ORDER BY c.name, a.member_lastname, a.member_nickname", n1_member.business_unit_id, member_id);

			return new
			{
				tabs,
				options,
				employee_list,
				employee_list_tickets
			};
		}


		public EmployeeFootPrintListItem GetOptions(string link)
		{

			switch (link)
			{
				case "accountmanagers":
					return new EmployeeFootPrintListItem("Reassign", populate_accountmanagers());
				case "assets":
					return new EmployeeFootPrintListItem("Reassign", populate_assets());
				case "cellphones":
					return new EmployeeFootPrintListItem("Reassign", populate_cellphones());
				case "cisr":
					return new EmployeeFootPrintListItem("Reassign", populate_CISR());
				case "controls":
					return new EmployeeFootPrintListItem("Reassign", populate_controls());
				case "faqs":
					return new EmployeeFootPrintListItem("Reassign", populate_faqs());
				case "isr":
					return new EmployeeFootPrintListItem("Reassign", populate_ISR());
				case "mam":
					return new EmployeeFootPrintListItem("Reassign", populate_MAM());
				case "messageboard":
					return new EmployeeFootPrintListItem("Close", populate_messageboard());
				case "offersediting":
					return new EmployeeFootPrintListItem("Reassign", populate_offersediting());
				case "osr":
					return new EmployeeFootPrintListItem("Reassign", populate_OSR());
				case "payroll_handler":
					return new EmployeeFootPrintListItem("Reassign", populate_payroll_handler());
				case "po_dist":
					return new EmployeeFootPrintListItem("Close", populate_po_dist());
				case "projectmanagers":
					return new EmployeeFootPrintListItem("Reassign", populate_projectmanagers());
				case "purchaseorders":
					return new EmployeeFootPrintListItem("Reassign", populate_purchaseorders());
				case "quoteprocess":
					return new EmployeeFootPrintListItem("Reassign", populate_quoteprocess());
				case "wofreeze":
					return new EmployeeFootPrintListItem("Close", populate_wofreeze());
				case "ram":
					return new EmployeeFootPrintListItem("Reassign", populate_RAM());
				case "report_to":
					return new EmployeeFootPrintListItem("Reassign", populate_report_to());
				case "reviewstodo":
					return new EmployeeFootPrintListItem("Reassign", populate_reviewstodo());
				case "rfqs":
					return new EmployeeFootPrintListItem("Reassign", populate_rfqs());
				case "scheduler":
					return new EmployeeFootPrintListItem("Close", populate_scheduler());
				case "ticketgroupman":
					return new EmployeeFootPrintListItem("Reassign", populate_ticketgroupman());
				case "workorders":
					return new EmployeeFootPrintListItem("Reassign", populate_workorders());
				case "quotes":
					return new EmployeeFootPrintListItem("Reassign", populate_quotes());
				case "tickets_assigned":
					return new EmployeeFootPrintListItem("Reassign", populate_tickets_assigned());
				case "opentickets":
					return new EmployeeFootPrintListItem("Reassign", populate_opentickets());
				default:
					return null;
			}
		}


		private LabelValueStringLink[] populate_po_dist()
		{
			return bllToolbox.doSQL_Array<LabelValueStringLink>(@" SELECT b.id value, CONCAT('Has PO Approval Threshold Set: ', b.amount_to, ' for ',a.name) label,
CONCAT('/business_units_edit.aspx?id=',a.id) link FROM business_unit a INNER JOIN business_unit_po_dist b ON a.id = b.business_unit_id WHERE b.member_id=@v0 ", member_id);
		}

		private LabelValueStringLink[] populate_faqs()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT who_to_ask_id value, Concat('Set as the go to person for this question: ', who_to_ask_question) label, 
Concat('/sections/hr/who_to_ask/index.aspx') link from who_to_ask where who_to_ask_memberid=@v0 ", member_id));
		}
		private LabelValueStringLink[] populate_cellphones()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT cellphone.id value, Concat(number,' - ',imei,' - ',cellphone_carrier.`name`) label,
Concat('/sections/hr/member/cellphones.aspx') link FROM cellphone 
INNER JOIN cellphone_status ON cellphone.cellphone_status_id = cellphone_status.id 
INNER JOIN cellphone_carrier ON cellphone.cellphone_carrier_id = cellphone_carrier.id 
where member_id = @v0  and cellphone_status_id = 1 ", member_id));

		}
		private LabelValueStringLink[] populate_messageboard()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"Select digitalsignage_id value, digitalsignage_eventtitle label, 
'/sections/member/messageboard/index.aspx' link from digitalsignage where digitalsignage_auditmemberid = @v0  and digitalsignageenddate > curdate() ", member_id));

		}

		private LabelValueStringLink[] populate_assets()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"Select assets_id value, Concat(assets_no,' - ',assets_make, ' ', assets_model) label, 
'/sections/assets/frame.aspx' link from assets where assets_owner = @v0  ", member_id));

		}
		private LabelValueStringLink[] populate_rfqs()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"Select rfq_header.id value, rfq_header.name label, 
concat('/sections/vendor_rfq/index.aspx?id=',rfq_header.id) link from rfq_header where member_id = @v0  and date_close > curdate() ", member_id));
		}
		private LabelValueStringLink[] populate_wofreeze()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@" Select appointments.id value, Concat('WO : ',appointments.subject ,' - frozen') label,
'/sections/member/scheduler/index3.aspx' link from appointments where appointments.setby = @v0  and appointments.startdate>curdate() ", member_id));
		}

		private LabelValueStringLink[] populate_tickets_assigned()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@" SELECT a.ticketheader_id value, 
CONCAT('(', a.ticketheader_id, ' - ',c.ticket_group_name,' ) - ', a.ticketheader_issue) label, 
Concat('/sections/member/tickets/ticketpage.aspx?issue=',a.ticketheader_id) link FROM 
ticketheader AS a INNER JOIN ticketpage b ON a.ticketheader_module_id = b.ticketpage_id 
INNER JOIN ticket_group c ON b.ticketpage_ticket_group_id = c.ticket_group_id 
WHERE a.ticketheader_member_assigned_id = @v0  AND a.ticketheader_status_id != 5 ORDER BY a.ticketheader_id", member_id));

		}
		private LabelValueStringLink[] populate_report_to()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@" SELECT CONCAT('M-',member_id) value,
CONCAT('(Member) ',member_fullname) label, Concat('/#/opens/127/employees/',member_id) link
FROM member WHERE reports_to = @v0  AND member_status = 'Active'
UNION SELECT CONCAT('O-',a.id) value, CONCAT('(Offer) ',b.member_fullname) label, 
Concat('/#/opens/127/employees/',b.member_id,'/offer/',a.id) link 
FROM member_offers a INNER JOIN member b on a.memberid = b.member_id 
WHERE (a.status = 'Released' or a.status = 'Approved' or a.`status` = 'In Development') 
and a.reports_to = @v0  AND a.enddate > CURDATE() UNION SELECT CONCAT('A-',id) value, 
CONCAT('(Applicant) ',firstname, ' ', lastname) label, Concat('/#/opens/138/applicants/',id) link 
FROM applicants a WHERE addedbymemberid = @v0  AND status NOT IN ('Deleted','Hired') ", member_id));
		}
		private LabelValueStringLink[] populate_workorders()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@" SELECT woprog_id value,
CONCAT('(',woprog_bvwo,') ', woprog_customername, ' - ', LEFT(woprog_description, 100)) label,
Concat('/sections/workorder/index.aspx?woprog_id=', woprog_id) link 
FROM woprog WHERE woprog_status NOT IN ('Waiting To Be Invoiced', 'Invoiced', 'Deleted') AND woprog_pm_memberid = @v0  ", member_id));

		}
		private LabelValueStringLink[] populate_purchaseorders()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@" SELECT a.poprog_id value,
CONCAT('(',c.name,') ',a.poprog_bvpo, ' - ', b.vendor_name) label, 
Concat('/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid=', poprog_id) link 
FROM poprog_header a LEFT JOIN vendor b ON a.poprog_vendor_id = b.vendor_id 
LEFT JOIN business_unit c ON a.business_unit_id = c.id WHERE a.poprog_status IN (1,3,5,9)
AND a.poprog_cutby_member_id = @v0  ORDER BY c.name, a.poprog_bvpo", member_id));
		}

		private LabelValueStringLink[] populate_payroll_handler()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@" SELECT a.member_id value,
CONCAT('(', b.name, ') ', member_fullname) label, 
Concat('/#/opens/127/employees/',a.member_id) link
FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE payroll_handler = @v0  AND member_status = 'Active' ORDER BY member_lastname, member_nickname", member_id));
		}
		private LabelValueStringLink[] populate_quotes()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@" SELECT quote_id value,
CONCAT('(',quote_id,') - ', b.customer_name, ' - ', a.job_description) label,
Concat('/#/opens/65/quotes/',quote_id,'/',revision) link 
FROM quote_master a LEFT JOIN customer b ON a.customer_id = b.customer_id WHERE active_revision=1 AND status_id IN (5,4,2,1,3) AND quoted_by = @v0 ", member_id));

		}
		private LabelValueStringLink[] populate_accountmanagers()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT c.address_id value,
CONCAT(a.customer_name, ' - ', c.address_addr1) label, 
Concat('/#/opens/10/customers/',a.customer_id)
link FROM
customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id
LEFT JOIN address c ON b.address_id = c.address_id WHERE b.account_manager = @v0
AND c.address_id != 0 ORDER BY a.customer_name", member_id));
		}
		private LabelValueStringLink[] populate_projectmanagers()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT c.address_id value,
CONCAT(a.customer_name, ' - ', c.address_addr1) label,
Concat('/#/opens/10/customers/',a.customer_id) link 
FROM customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id 
LEFT JOIN address c ON b.address_id = c.address_id WHERE b.project_mgr_member_id = @v0  
AND c.address_id != 0 ORDER BY a.customer_name", member_id));
		}
		private LabelValueStringLink[] populate_controls()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT c.address_id value, 
CONCAT(a.customer_name, ' - ', c.address_addr1) label,
Concat('/#/opens/10/customers/',a.customer_id) link 
FROM customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id 
LEFT JOIN address c ON b.address_id = c.address_id WHERE b.controls_mgr_member_id = @v0 
AND c.address_id != 0 ORDER BY a.customer_name", member_id));
		}
		private LabelValueStringLink[] populate_ISR()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT c.address_id value, 
CONCAT(a.customer_name, ' - ', c.address_addr1) label,
Concat('/#/opens/10/customers/',a.customer_id) link 
FROM customer a LEFT JOIN customer_sales_properties b 
ON a.customer_id = b.customer_id LEFT JOIN address c 
ON b.address_id = c.address_id WHERE b.isr_member_id = @v0 
AND c.address_id != 0 ORDER BY a.customer_name", member_id));
		}
		private LabelValueStringLink[] populate_OSR()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT c.address_id value,
CONCAT(a.customer_name, ' - ', c.address_addr1) label,
Concat('/#/opens/10/customers/',a.customer_id) link
FROM customer a LEFT JOIN customer_sales_properties b 
ON a.customer_id = b.customer_id LEFT JOIN address c 
ON b.address_id = c.address_id WHERE b.osr_member_id = @v0 
AND c.address_id != 0 ORDER BY a.customer_name", member_id));
		}
		private LabelValueStringLink[] populate_RAM()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT c.address_id value, 
CONCAT(a.customer_name, ' - ', c.address_addr1) label,
Concat('/#/opens/10/customers/',a.customer_id) link 
FROM customer a LEFT JOIN customer_sales_properties b 
ON a.customer_id = b.customer_id LEFT JOIN address c 
ON b.address_id = c.address_id WHERE b.ram_member_id = @v0 
AND c.address_id != 0 ORDER BY a.customer_name", member_id));
		}
		private LabelValueStringLink[] populate_MAM()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT c.address_id value, 
CONCAT(a.customer_name, ' - ', c.address_addr1) label,
Concat('/#/opens/10/customers/',a.customer_id) link 
FROM customer a LEFT JOIN customer_sales_properties b
ON a.customer_id = b.customer_id LEFT JOIN address c 
ON b.address_id = c.address_id WHERE b.mam_member_id = @v0 
AND c.address_id != 0 ORDER BY a.customer_name", member_id));
		}
		private LabelValueStringLink[] populate_CISR()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT c.address_id value,
CONCAT(a.customer_name, ' - ', c.address_addr1) label,
Concat('/#/opens/10/customers/',a.customer_id) link 
FROM customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id
LEFT JOIN address c ON b.address_id = c.address_id WHERE b.cisr_member_id = @v0 
AND c.address_id != 0 ORDER BY a.customer_name", member_id));
		}
		private LabelValueStringLink[] populate_opentickets()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@" SELECT a.ticketheader_id value, 
CONCAT('(', a.ticketheader_id, ' - ',c.ticket_group_name,' ) - ', a.ticketheader_issue) label,
Concat('/sections/member/tickets/ticketpage.aspx?issue=',a.ticketheader_id) link FROM 
ticketheader AS a INNER JOIN ticketpage b ON a.ticketheader_module_id = b.ticketpage_id
INNER JOIN ticket_group c ON b.ticketpage_ticket_group_id = c.ticket_group_id 
WHERE a.ticketheader_createdby_member_id = @v0  AND a.ticketheader_status_id != 5
ORDER BY a.ticketheader_id", member_id));
		}
		private LabelValueStringLink[] populate_quoteprocess()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@" SELECT a.quoteid value,
CONCAT(b.quote_id, ' - ', c.customer_name) label,
Concat('/#/opens/65/quotes/',b.quote_id,'/',b.revision) link FROM
quote_schedule a INNER JOIN quote_master b ON a.quoteid = b.quote_id 
AND b.active_revision = TRUE and (b.status_id < 6 or b.status_id > 9)
LEFT JOIN customer c ON b.customer_id = c.customer_id WHERE a.pointperson = @v0 
or a.estimator = @v0  or (a.stage1screening_mid = @v0  
and a.stage1screening_date is null) or (a.schedule_produced_mid = @v0 
and a.schedule_produced_date is null) or (a.manpower_information_collected_mid = @v0 
and a.manpower_information_collected_date is null) or (a.customer_info_collected_mid = @v0  
and a.customer_info_collected_date is null) or (a.market_info_collected_mid = @v0 
and a.market_info_collected_date is null) or (a.finance_info_collected_mid = @v0 
and a.finance_info_collected_date is null) or (a.recon_report_created_mid = @v0 
and a.recon_report_created_date is null) or (a.quote_delivery_strategy_mid = @v0  
and a.quote_delivery_strategy_date is null) or (a.recon_report_created_mid = @v0  
and a.recon_report_created_date is null) or (a.project_estimated_mid = @v0  
and a.project_estimated_date is null) or (a.worksheet_review_mid = @v0 
and a.worksheet_review_date is null) or (a.stage6_final_review_mid = @v0  
and a.stage6_final_review_date is null) or (a.quote_delivered_mid = @v0  
and a.quote_delivered_date is null) or (a.stage6_final_review_mid = @v0  
and a.stage6_final_review_date is null) or (a.followup1_mid = @v0  
and a.followup1_date is null) or (a.followup2_mid = @v0  and a.followup2_date is null)
or (a.convert_or_kill_mid = @v0  and a.convert_or_kill_date is null) 
or (a.post_mortem_complete_mid = @v0  and a.post_mortem_complete_date is null)
or (a.rt1 = @v0  and (a.rt1_s4_approved is null or rt1_f_approved is null)) 
or (a.rt2 = @v0  and (a.rt2_s4_approved is null or rt2_f_approved is null)) 
or (a.rt3 = @v0  and (a.rt3_s4_approved is null or rt3_f_approved is null)) 
or (a.rt4 = @v0  and (a.rt4_s4_approved is null or rt4_f_approved is null)) 
ORDER BY id", member_id));
		}
		private LabelValueStringLink[] populate_offersediting()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"(SELECT a.id value,
Concat('Offer for ',b.member_fullname,' - ',a.status, ' - Start date:', date(a.startdate)) label,
Concat('/#/opens/127/employees/',a.memberid,'/offer/', a.id) link 
from member_offers a LEFT JOIN member b ON a.memberid = b.member_id WHERE a.enteredby = @v0 
AND a.status NOT IN ('Previous', 'Deleted', 'Closed', 'Accepted') and a.memberid > 0 
ORDER BY member_fullname) union (
SELECT a.id VALUE,
CONCAT('Offer for ',b.firstname,' ', b.lastname,' - ',a.status, ' - Start date:', DATE(a.startdate)) label,
CONCAT('/#/opens/138/applicants/',a.`applicantid`,'/offer/', a.id) link 
FROM member_offers a LEFT JOIN applicants b ON a.`applicantid` = b.id WHERE a.enteredby = @v0
AND a.status NOT IN ('Previous', 'Deleted', 'Closed', 'Accepted') AND (a.`isapplicant`) order by label
)  ", member_id));
		}
		private LabelValueStringLink[] populate_reviewstodo()
		{
			return (bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT a.id value,
Concat('Review for ',b.member_fullname,' - ',a.status, ' - scheduled for: ', a.date) label,
Concat('/#/opens/127/employees/',a.member_id,'/reviews/',a.id) link 
FROM emp_review a LEFT JOIN member b ON a.member_id = b.member_id 
WHERE a.reviewed_by_id = @v0  and a.status NOT IN ('Closed', 'Delivered')
AND b.member_status = 'Active' ORDER BY b.member_fullname", member_id));
		}
		private LabelValueStringLink[] populate_scheduler()
		{
			return bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT a.id value, 
Concat('Scheduled ',c.member_fullname,' for ',a.subject, ' on ',date(a.startdate),' - ', appointment_status.status) label, 
Concat('/sections/member/scheduler/index1.aspx') link
FROM appointments a LEFT JOIN member b ON a.setby = b.member_id 
left join appointment_status on a.status = appointment_status.id 
LEFT JOIN member c ON a.resourceid = c.member_id WHERE a.setby = @v0 
AND b.member_status = 'Active' and a.startdate>curdate() ORDER BY b.member_fullname", member_id);
		}
		private LabelValueStringLink[] populate_ticketgroupman()
		{
			return bllToolbox.doSQL_Array<LabelValueStringLink>(@"SELECT ticket_group_id value, ticket_group_name label, 
'' link FROM ticket_group WHERE ticket_group_administrator = @v0  ", member_id);
		}

		public DataExtra Reassign(EmployeeFootPrintsReassign model)
		{

			if (model.button == "Close")
			{
				foreach (var o in model.items)
				{
					var item = o.Value;

					switch (model.type)
					{
						case "scheduler":
							bllToolbox.doSQL_void(@"UPDATE appointments SET setby = @v0  WHERE id = @v1  LIMIT 1", 0, item);
							break;
						case "messageboard":
							bllToolbox.doSQL_void(@"DELETE FROM digitalsignage where digitalsignage_id=@v0 ", item);
							break;
						case "wofreeze":
							bllToolbox.doSQL_void(@"Delete appointments WHERE id = @v0  LIMIT 1", item);
							break;
						case "po_dist":
							bllToolbox.doSQL_void(@"delete from business_unit_PO_dist where id = @v0  limit 1", item);
							break;
					}
				}
			}
			else if (model.button == "Reassign")
			{
				var value_to = model.to_member_id;
				foreach (var o in model.items)
				{
					customer_sales_properties csp;
					var item = o.Value;
					switch (model.type)
					{
						case "accountmanagers":
							csp = new customer_sales_properties(Convert.ToInt32(item))
							{
								am_member_id = Convert.ToInt32(value_to)
							};
							csp.save();
							break;
						case "isr":
							csp = new customer_sales_properties(Convert.ToInt32(item))
							{
								isr_member_id = Convert.ToInt32(value_to)
							};
							csp.save();
							break;
						case "osr":
							csp = new customer_sales_properties(Convert.ToInt32(item))
							{
								osr_member_id = Convert.ToInt32(value_to)
							};
							csp.save();
							break;
						case "ram":
							csp = new customer_sales_properties(Convert.ToInt32(item))
							{
								ram_member_id = Convert.ToInt32(value_to)
							};
							csp.save();
							break;
						case "mam":
							csp = new customer_sales_properties(Convert.ToInt32(item))
							{
								mam_member_id = Convert.ToInt32(value_to)
							};
							csp.save();
							break;
						case "cisr":
							csp = new customer_sales_properties(Convert.ToInt32(item))
							{
								cisr_member_id = Convert.ToInt32(value_to)
							};
							csp.save();
							break;
						case "opentickets":
							bllToolbox.doSQL_void(@"UPDATE ticketheader SET ticketheader_createdby_member_id = @v0  WHERE ticketheader_id = @v1 ", value_to, item);
							break;
						case "offersediting":
							bllToolbox.doSQL_void(@"UPDATE member_offers SET enteredby = @v0  WHERE id = @v1  LIMIT 1", value_to, item);
							break;
						case "reviewstodo":
							bllToolbox.doSQL_void(@"UPDATE emp_review SET reviewed_by_id = @v0  WHERE id = @v1  LIMIT 1", value_to, item);
							break;
						case "scheduler":
							bllToolbox.doSQL_void(@"UPDATE appointments SET setby = @v0  WHERE id = @v1  LIMIT 1", value_to, item);
							break;
						case "ticketgroupman":
							bllToolbox.doSQL_void(@"UPDATE ticket_group SET ticket_group_administrator = @v0  WHERE ticket_group_id = @v1 ", value_to, item);
							break;
						case "faqs":
							bllToolbox.doSQL_void(@"UPDATE who_to_ask SET who_to_ask_memberid = @v0  WHERE who_to_ask_id = @v1 ", value_to, item);
							break;
						case "cellphones":
							bllToolbox.doSQL_void(@"UPDATE cellphone SET member_id = @v0  WHERE id = @v1 ", value_to, item);
							break;
						case "assets":
							bllToolbox.doSQL_void(@"UPDATE assets SET assets_owner = @v0  WHERE assets_id = @v1 ", value_to, item);
							break;
						case "rfqs":
							bllToolbox.doSQL_void(@"UPDATE rfq_header SET member_id = @v0  WHERE id = @v1 ", value_to, item);
							break;
						case "wofreeze":
							bllToolbox.doSQL_void(@"UPDATE appointments SET setby = @v0  where id = @v1  limit 1", value_to, item);
							break;
						case "po_dist":
							bllToolbox.doSQL_void(@"UPDATE business_unit_po_dist SET member_id = @v0  where id = @v1  limit 1", value_to, item);
							break;
						case "controls":
							csp = new customer_sales_properties(Convert.ToInt32(item));
							csp.controls_mgr_member_id = Convert.ToInt32(value_to);
							csp.save();
							break;
						case "payroll_handler":
							bllToolbox.doSQL_void(@"UPDATE member SET payroll_handler = @v0  WHERE member_id = @v1 ", value_to, item);
							break;
						case "projectmanagers":
							csp = new customer_sales_properties(Convert.ToInt32(item)); csp.project_mgr_member_id = Convert.ToInt32(value_to); csp.save();
							break;
						case "purchaseorders":
							bllToolbox.doSQL_void(@"UPDATE poprog_header SET poprog_cutby_member_id = @v0  WHERE poprog_id = @v1 ", value_to, item);
							break;
						case "quoteprocess":
							update_quote_process("stage1screening_mid", value_to, item, "stage1screening_date");
							update_quote_process("schedule_produced_mid", value_to, item, "schedule_produced_date");
							update_quote_process("manpower_information_collected_mid", value_to, item, "manpower_information_collected_date");
							update_quote_process("customer_info_collected_mid", value_to, item, "customer_info_collected_date");
							update_quote_process("market_info_collected_mid", value_to, item, "market_info_collected_date");
							update_quote_process("finance_info_collected_mid", value_to, item, "finance_info_collected_date");
							update_quote_process("recon_report_created_mid", value_to, item, "recon_report_created_date");
							update_quote_process("quote_delivery_strategy_mid", value_to, item, "quote_delivery_strategy_date");
							update_quote_process("project_estimated_mid", value_to, item, "project_estimated_date");
							update_quote_process("worksheet_review_mid", value_to, item, "worksheet_review_date");
							update_quote_process("stage6_final_review_mid", value_to, item, "stage6_final_review_date");
							update_quote_process("quote_delivered_mid", value_to, item, "quote_delivered_date");
							update_quote_process("followup1_mid", value_to, item, "followup1_date");
							update_quote_process("followup2_mid", value_to, item, "followup2_date");
							update_quote_process("convert_or_kill_mid", value_to, item, "convert_or_kill_date");
							update_quote_process("post_mortem_complete_mid", value_to, item, "post_mortem_complete_date");
							bllToolbox.doSQL_void(@"UPDATE quote_schedule SET pointperson = @v0  WHERE pointperson = @v2  and quoteid = @v1  limit 1", value_to, item, member_id);
							bllToolbox.doSQL_void(@"UPDATE quote_schedule SET estimator = @v0  WHERE estimator = @v2  and quoteid = @v1  limit 1", value_to, item, member_id);
							bllToolbox.doSQL_void(@"UPDATE quote_schedule SET rt1 = @v0  WHERE (rt1_s4_approved is null or rt1_f_approved is null) and rt1 = @v2  and quoteid = @v1  limit 1", value_to, item, member_id);
							bllToolbox.doSQL_void(@"UPDATE quote_schedule SET rt2 = @v0  WHERE (rt2_s4_approved is null or rt2_f_approved is null) and rt2 = @v2  and quoteid = @v1  limit 1", value_to, item, member_id);
							bllToolbox.doSQL_void(@"UPDATE quote_schedule SET rt3 = @v0  WHERE (rt3_s4_approved is null or rt3_f_approved is null) and rt3 = @v2  and quoteid = @v1  limit 1", value_to, item, member_id);
							bllToolbox.doSQL_void(@"UPDATE quote_schedule SET rt4 = @v0  WHERE (rt4_s4_approved is null or rt4_f_approved is null) and rt4 = @v2  and quoteid = @v1  limit 1", value_to, item, member_id);
							break;
						case "report_to":
							var split_from_id = item.ToString().Split('-');
							var from_id = split_from_id[1];
							switch (split_from_id[0])
							{
								case "M":
									if (!NeMember.Check_for_circular_org_chart(Convert.ToInt32(from_id), value_to))
									{
										bllToolbox.doSQL_void(@"UPDATE member SET reports_to = @v0  WHERE member_id = @v1 ", value_to, from_id);
									}
									break;
								case "O":
									if (!NeMember.Check_for_circular_org_chart(Convert.ToInt32(from_id), value_to))
									{
										bllToolbox.doSQL_void(@"UPDATE member_offers SET reports_to = @v0  WHERE id = @v1 ", value_to, from_id);
									}
									break;
								case "A":
									bllToolbox.doSQL_void(@"UPDATE applicants SET addedbymemberid = @v0  WHERE id = @v1 ", value_to, from_id);
									break;
							}
							break;
						case "workorders":
							bllToolbox.doSQL_void(@"UPDATE woprog SET woprog_pm_memberid = @v0  WHERE woprog_id = @v1 ", value_to, item);
							break;
						case "quotes":
							bllToolbox.doSQL_void(@"UPDATE quote_master SET quoted_by = @v0  WHERE quote_id = @v1 ", value_to, item);
							break;
						case "tickets_assigned":
							var t = new NETickets(Convert.ToInt32(item));
							// find out if the assignee is a member of the ticket group
							var x = bllToolbox.doSQL_int(@"Select count(ticketmanager_id) from ticketmanager  where ticketmanager_member_id =@v0 and ticketmanager_group_id =@v1 ",
									value_to, t.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_id);
							if (x == 0)// if not, add them to that group
							{
								using (var uow = new UnitOfWork())
								{
									var m = new ne_xpo.cs.ticketmanager(uow)
									{
										ticketmanager_member_id = uow.GetObjectByKey<ne_xpo.cs.member>(Convert.ToInt32(value_to)),
										ticketmanager_group_id =
											uow.GetObjectByKey<ne_xpo.cs.ticket_group>(t.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_id)
									};
									m.Save();
									uow.CommitChanges();
								}
							}
							bllToolbox.doSQL_void(@"UPDATE ticketheader SET ticketheader_member_assigned_id = @v0  WHERE ticketheader_id = @v1 ", value_to, item);
							break;
						default:
							break;
					}
				}
			}
			return new DataExtra(
				"Success.", new
				{
					tabs = GetTabList(),
					options = GetOptions(model.type)
				});
		}

		protected void update_quote_process(string field, object value_to, object item, string added_where)
		{
			bllToolbox.doSQL_void(string.Format(@"UPDATE quote_schedule SET {0}  = @v0
			WHERE {0}  = @v1  and quoteid = @v2  and {1} is null limit 1", field, added_where), value_to, member_id, item);

		}
	}
}