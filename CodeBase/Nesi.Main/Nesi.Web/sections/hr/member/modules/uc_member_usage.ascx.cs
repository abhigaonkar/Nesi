using System;
using System.Collections.Generic;
using System.Data;
using DevExpress.Web;
using DevExpress.Xpo;
using nesi.core;

public partial class sections_hr_member_modules_uc_member_usage : System.Web.UI.UserControl
{
	public string Title { get; set; }
	public string Field { get; set; }
	private int _employee_id;
	private int _current_user;
	public int employee_id { get { return _employee_id; } set { _employee_id = value; } }
	public int current_user { get { return _current_user; } set { _current_user = value; } }
	NeMember loaded_user;
	DataTable employee_list;

	DataTable employee_list_tickets;
	Toolbox _tools;
	public bool has_open_items { get; set; }

//	protected void Page_Init(object sender, EventArgs e)
//	{
//		
//	
//	
//	}

	protected void Page_Load(object sender, EventArgs e)
	{
	_tools = new Toolbox();
		loaded_user = new NeMember(_employee_id);

		employee_list = Toolbox.doSQL_dt(@" SELECT a.member_id id, CONCAT('(', b.name,') ',member_fullname) text FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE a.member_status = 'Active' AND a.member_id != @v1  ORDER BY b.name, a.member_lastname, a.member_firstname", new object[] {  loaded_user.business_unit_id, loaded_user.id } );

		employee_list_tickets = Toolbox.doSQL_dt(@" SELECT a.member_id id, CONCAT('(', c.name, ') ',a.member_fullname) text FROM member a INNER JOIN ticketmanager b ON b.ticketmanager_member_id = a.Member_ID LEFT JOIN business_unit c ON a.business_unit_id = c.id WHERE a.member_id != @v0  and a.member_status = 'Active' GROUP BY a.member_id ORDER BY c.name, a.member_lastname, a.member_nickname", new object[] {  loaded_user.business_unit_id, loaded_user.id } );

		if (!IsPostBack)
		{
			tc.ActiveTab.Index = 0;
			if (_employee_id != 0)
			{
				lbl_title.Text = loaded_user.FullName + " is found in our system in the following places";
			}
			else
			{
				lbl_title.Text = "The following items are unassigned";
			}


			rebuild_tabs();
			DataBind();
		}
		else
		{
		
				DataBind();
		
		}
	}
//	private void InitializeComponent()
//	{
//	}
//	override protected void OnInit(EventArgs e)
//	{
//		this.Load += new System.EventHandler(this.Page_Load);
//		base.OnInit(e);
//	}
	public void rebuild_tabs()
	{
		var tab_count = 0;
		var start_tab = 0;
		foreach (Tab tp in tc.Tabs)
		{
			var rowcount = 0;
			switch (tp.Name)
			{


				case "Assigned Tickets":
					rowcount = populate_tickets_assigned().Rows.Count;
					break;
				case "WOs":
					rowcount = populate_workorders().Rows.Count;
					break;
				case "POs":
					rowcount = populate_purchaseorders().Rows.Count;
					break;
				case "Reports To":
					rowcount = populate_report_to().Rows.Count;
					break;
				case "Payroll Handler":
					rowcount = populate_payroll_handler().Rows.Count;
					break;
				case "Quotes":
					rowcount = populate_quotes().Rows.Count;
					break;
				case "Account Manager":
					rowcount = populate_accountmanagers().Rows.Count;
					break;
				case "Project Manager":
					rowcount = populate_projectmanagers().Rows.Count;
					break;
				case "Controls":
					rowcount = populate_controls().Rows.Count;
					break;
				case "ISR":
					rowcount = populate_ISR().Rows.Count;
					break;
				case "OSR":
					rowcount = populate_OSR().Rows.Count;
					break;
				case "RAM":
					rowcount = populate_RAM().Rows.Count;
					break;
				case "MAM":
					rowcount = populate_MAM().Rows.Count;
					break;
				case "CISR":
					rowcount = populate_CISR().Rows.Count;
					break;
				case "Quote Process":
					//						rowcount = populate_quoteprocess().Rows.Count;
					break;
				case "Offers Editing":
					rowcount = populate_offersediting().Rows.Count;
					break;
				case "Reviews to do":
					rowcount = populate_reviewstodo().Rows.Count;
					break;
				case "Scheduler":
					rowcount = populate_scheduler().Rows.Count;
					break;
				case "Ticket Group Man":
					rowcount = populate_ticketgroupman().Rows.Count;
					break;

				case "FAQs":
					rowcount = populate_faqs().Rows.Count;
					break;
				case "Cell Phones":
					rowcount = populate_cellphones().Rows.Count;
					break;
				case "Messageboard":
					rowcount = populate_messageboard().Rows.Count;
					break;

				case "Assets":
					rowcount = populate_assets().Rows.Count;
					break;
				case "RFQs":
					rowcount = populate_rfqs().Rows.Count;
					break;
				case "WO Freeze":
					rowcount = populate_wofreeze().Rows.Count;
					break;
				case "Ticket Admin":
					rowcount = populate_wofreeze().Rows.Count;
					break;
				case "PO Approval Level":
					rowcount = populate_po_dist().Rows.Count;
					break;

			}
			if (rowcount > 0)
			{

				start_tab = tp.Index;
				tab_count++;
				if (tab_count > 5)
				{
					tp.NewLine = true;
					tab_count = 0;
				}
				tp.Text = tp.Name + " (" + rowcount + ")";
			}
			else
			{
				tp.NewLine = false;
				tp.Visible = false;
			}
		}
		if (start_tab > 0)
		{
			Session["has_open_items" + _employee_id] = true;
			tc.ActiveTab.Index = start_tab;
		//	DataBind();
		}
		else
		{
			//	tc.ActiveTab.Index = -1;
			Session["has_open_items" + _employee_id] = false;
			tc.Visible = false;
			gv.Visible = false;
			lbl_title.Text = "No footprint found";
		}
	}

	public override void DataBind()
	{
//		base.DataBind();
		loaded_user = new NeMember(_employee_id);
		
		var btnclose = (ASPxButton)gv.FindFooterCellTemplateControl(gv.Columns[" "], "btnclose");
		var btnreassign = (ASPxButton)gv.FindFooterCellTemplateControl(gv.Columns[" "], "btnreassign");
		var btnleave = (ASPxButton)gv.FindFooterCellTemplateControl(gv.Columns[" "], "btnleave");
		if (tc.ActiveTab != null)
		{
			switch (tc.ActiveTab.Name)
			{
				case "Assigned Tickets":
					gv.DataSource = populate_tickets_assigned();
					break;
				case "WOs":
					gv.DataSource = populate_workorders();
					break;
				case "POs":
					gv.DataSource = populate_purchaseorders();
					break;
				case "Reports To":
					gv.DataSource = populate_report_to();
					break;
		
				case "Payroll Handler":
					gv.DataSource = populate_payroll_handler();
					break;
				case "Quotes":
					gv.DataSource = populate_quotes();
					break;
				case "Account Manager":
					gv.DataSource = populate_accountmanagers();
					break;
				case "Project Manager":
					gv.DataSource = populate_projectmanagers();
					break;
				case "Controls":
					gv.DataSource = populate_controls();
					break;
				case "ISR":
					gv.DataSource = populate_ISR();
					break;
				case "OSR":
					gv.DataSource = populate_OSR();
					break;
				case "RAM":
					gv.DataSource = populate_RAM();
					break;
				case "MAM":
					gv.DataSource = populate_MAM();
					break;
				case "CISR":
					gv.DataSource = populate_CISR();
					break;
				case "Quote Process":
					//				gv.DataSource = populate_quoteprocess();
					break;
				case "Offers Editing":
					gv.DataSource = populate_offersediting();
					break;
				case "Reviews to do":
					gv.DataSource = populate_reviewstodo();
					break;
				case "Scheduler":
					gv.DataSource = populate_scheduler();
					break;
				case "Ticket Group Man":
					gv.DataSource = populate_ticketgroupman();
					break;
				case "FAQs":
					gv.DataSource = populate_faqs();
					break;
				case "Cell Phones":
					gv.DataSource = populate_cellphones();
					break;
				case "Messageboard":
					gv.DataSource = populate_messageboard();
					break;

				case "Assets":
					gv.DataSource = populate_assets();
					break;
				case "RFQs":
					gv.DataSource = populate_rfqs();
					break;
				case "WO Freeze":
					gv.DataSource = populate_wofreeze();
					break;
				case "PO Approval Level":
					gv.DataSource = populate_po_dist();
					break;
				default:
					tc.Tabs.Clear();
//					ScriptManager.RegisterStartupScript(this, typeof(string), "key3", "window.opener.location.href = window.opener.location.href;", true);
					break;
			}
			gv.DataBind();
			if (gv.VisibleRowCount > 0)
			{
				tc.ActiveTab.Text = tc.ActiveTab.Name + " (" + gv.VisibleRowCount + ")";
			}
			else
			{
				if (IsPostBack)
				{
					rebuild_tabs();
					DataBind();
					usage_cb.JSProperties["cp_reload_parent"] = "1";
				}
			}
		}
		else
		{
			if (IsPostBack)
			{
				tc.Tabs.Clear();
//				ScriptManager.RegisterStartupScript(this, typeof(string), "key3", "window.opener.location.href = window.opener.location.href;", true);
			}
		
		}
	
	}
	protected void ddl_Init(object sender, EventArgs e)
	{
		var ddl = (ASPxComboBox)sender;
		if (tc.ActiveTab != null)
		{
			switch (tc.ActiveTab.Name)
			{
				case "Assigned Tickets":	ddl.DataSource = employee_list_tickets;break;
				case "WOs":					ddl.DataSource = employee_list;break;
				case "POs":					ddl.DataSource = employee_list;break;
				case "Reports To":			ddl.DataSource = employee_list;break;
				case "Payroll Handler":		ddl.DataSource = employee_list;break;
				case "Quotes":				ddl.DataSource = employee_list;break;
				case "Account Manager":		ddl.DataSource = employee_list;break;
				case "Project Manager":		ddl.DataSource = employee_list;break;
				case "Controls":			ddl.DataSource = employee_list;break;
				case "ISR":					ddl.DataSource = employee_list;break;
				case "OSR":					ddl.DataSource = employee_list;break;
				case "RAM":					ddl.DataSource = employee_list;break;
				case "MAM":					ddl.DataSource = employee_list;break;
				case "CISR":				ddl.DataSource = employee_list;break;			
				case "Quote Process":		ddl.DataSource = employee_list;break;
				case "Offers Editing":		ddl.DataSource = employee_list;break;
				case "Reviews to do":		ddl.DataSource = employee_list;break;
				case "Scheduler":			ddl.DataSource = employee_list;break;
				case "Ticket Group Man":	ddl.DataSource = employee_list;break;
				case "FAQs":				ddl.DataSource = employee_list;break;
				case "Cell Phones":			ddl.DataSource = employee_list;break;
				case "Inv Locations":		ddl.DataSource = employee_list;break;
				case "Assets":				ddl.DataSource = employee_list;break;
				case "RFQs":				ddl.DataSource = employee_list;break;
				case "PO Approval Level": ddl.DataSource = employee_list; break;
			}
					ddl.DataBind();
		}
	}

private DataTable	populate_po_dist()
{

	return Toolbox.doSQL_dt(@" SELECT b.id id, CONCAT('Has PO Approval Threshold Set: ', b.amount_to, ' for ',a.name) text, CONCAT('/company_edit.aspx?business_unit_id=',a.id) link FROM business_unit a INNER JOIN business_unit_po_dist b ON a.id = b.business_unit_id WHERE b.member_id=@v0 ", new object[] {  loaded_user.id } );
}	

private DataTable  populate_faqs()
{
	return (Toolbox.doSQL_dt(@"SELECT who_to_ask_id id, Concat('Set as the go to person for this question: ', who_to_ask_question) text, Concat('/sections/hr/who_to_ask/index.aspx') link from who_to_ask where who_to_ask_memberid=@v0 ", new object[] {  loaded_user.id } ));
}
private DataTable populate_cellphones()
{
	return (Toolbox.doSQL_dt(@"SELECT cellphone.id id, Concat(number,' - ',imei,' - ',cellphone_carrier.`name`) text, Concat('/sections/hr/member/cellphones.aspx') link FROM cellphone INNER JOIN cellphone_status ON cellphone.cellphone_status_id = cellphone_status.id INNER JOIN cellphone_carrier ON cellphone.cellphone_carrier_id = cellphone_carrier.id where member_id = @v0  and cellphone_status_id = 1 ", new object[] {  loaded_user.id } ));

}
private DataTable populate_messageboard()
{
	return (Toolbox.doSQL_dt(@"Select digitalsignage_id id, digitalsignage_eventtitle text, '/sections/member/messageboard/index.aspx' link from digitalsignage where digitalsignage_auditmemberid = @v0  and digitalsignageenddate > curdate() ", new object[] {  loaded_user.id } ));

}

private DataTable  populate_assets()
{
	return (Toolbox.doSQL_dt(@"Select assets_id id, Concat(assets_no,' - ',assets_make, ' ', assets_model) text, '/sections/assets/frame.aspx' link from assets where assets_owner = @v0  ", new object[] {  loaded_user.id}));

}
private DataTable  populate_rfqs()
{
return (Toolbox.doSQL_dt(@"Select rfq_header.id id, rfq_header.name text, concat('/sections/vendor_rfq/index.aspx?id=',rfq_header.id) link from rfq_header where member_id = @v0  and date_close > curdate() ", new object[] {  loaded_user.id}));
}
private DataTable  populate_wofreeze()
{
	return (Toolbox.doSQL_dt(@" Select appointments.id id, Concat('WO : ',appointments.subject ,' - frozen') text, '/sections/member/scheduler/index3.aspx' link from appointments where appointments.setby = @v0  and appointments.startdate>curdate() ", new object[] {  loaded_user.id}));
}

	private DataTable populate_tickets_assigned()
	{
		return (Toolbox.doSQL_dt(@" SELECT a.ticketheader_id id, CONCAT('(', a.ticketheader_id, ' - ',c.ticket_group_name,' ) - ', a.ticketheader_issue) text, Concat('/sections/member/tickets/ticketpage.aspx?issue=',a.ticketheader_id) link FROM ticketheader AS a INNER JOIN ticketpage b ON a.ticketheader_module_id = b.ticketpage_id INNER JOIN ticket_group c ON b.ticketpage_ticket_group_id = c.ticket_group_id WHERE a.ticketheader_member_assigned_id = @v0  AND a.ticketheader_status_id != 5 ORDER BY a.ticketheader_id", new object[] {  loaded_user.id}));
		
	}
	private DataTable populate_report_to()
	{
		return (Toolbox.doSQL_dt(@" SELECT CONCAT('M-',member_id) id, CONCAT('(Member) ',member_fullname) text, Concat('/sections/hr/member/index.aspx?id=',member_id) link FROM member WHERE reports_to = @v0  AND member_status = 'Active' UNION SELECT CONCAT('O-',a.id) id, CONCAT('(Offer) ',b.member_fullname) text, Concat('/sections/hr/member/member_offer.aspx?id=',a.id) link FROM member_offers a INNER JOIN member b on a.memberid = b.member_id WHERE (a.status = 'Released' or a.status = 'Approved' or a.`status` = 'In Development') and a.reports_to = @v0  AND a.enddate > CURDATE() UNION SELECT CONCAT('A-',id) id, CONCAT('(Applicant) ',firstname, ' ', lastname) text, Concat('/sections/hr/member/applicants.aspx') link FROM applicants a WHERE addedbymemberid = @v0  AND status NOT IN ('Deleted','Hired') ", new object[] {  loaded_user.id}));
	}
	private DataTable populate_workorders()
	{
		return (Toolbox.doSQL_dt(@" SELECT woprog_id id, CONCAT('(',woprog_bvwo,') ', woprog_customername, ' - ', LEFT(woprog_description, 100)) text, Concat('/sections/workorder/index.aspx?id=', woprog_id) link FROM woprog WHERE woprog_status NOT IN ('Waiting To Be Invoiced', 'Invoiced', 'Deleted') AND woprog_pm_memberid = @v0  ", new object[] {  loaded_user.id}));
		
	}
	private DataTable populate_purchaseorders()
	{
		return (Toolbox.doSQL_dt(@" SELECT a.poprog_id id, CONCAT('(',c.name,') ',a.poprog_bvpo, ' - ', b.vendor_name) text, Concat('/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid=', poprog_id) link FROM poprog_header a LEFT JOIN vendor b ON a.poprog_vendor_id = b.vendor_id LEFT JOIN business_unit c ON a.business_unit_id = c.id WHERE a.poprog_status IN (1,3,5,9) AND a.poprog_cutby_member_id = @v0  ORDER BY c.name, a.poprog_bvpo", new object[] {  loaded_user.id}));
	}
	
	private DataTable populate_payroll_handler()
	{
		return (Toolbox.doSQL_dt(@" SELECT a.member_id id, CONCAT('(', b.name, ') ', member_fullname) text, Concat('/sections/hr/member/index.aspx?id=',a.member_id) link FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE payroll_handler = @v0  AND member_status = 'Active' ORDER BY member_lastname, member_nickname", new object[] {  loaded_user.id}));
	}
	private DataTable populate_quotes()
	{
		return (Toolbox.doSQL_dt(@" SELECT quote_id id, CONCAT('(',quote_id,') - ', b.customer_name, ' - ', a.job_description) text, Concat('/#/opens/65/quotes/',quote_id,'/',revision) link FROM quote_master a LEFT JOIN customer b ON a.customer_id = b.customer_id WHERE active_revision=1 AND status_id IN (5,4,2,1,3) AND quoted_by = @v0 ", new object[] {  loaded_user.id}));

	}
	private DataTable populate_accountmanagers()
	{
		return(Toolbox.doSQL_dt(@"SELECT c.address_id id, CONCAT(a.customer_name, ' - ', c.address_addr1) text, Concat('/sections/customer/index.aspx?Customer_id=',a.customer_id) link FROM customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id LEFT JOIN address c ON b.address_id = c.address_id WHERE b.account_manager = @v0  AND c.address_id != 0 ORDER BY a.customer_name", new object[] {  loaded_user.id}));
	}
	private DataTable populate_projectmanagers()
	{
		return (Toolbox.doSQL_dt(@"SELECT c.address_id id, CONCAT(a.customer_name, ' - ', c.address_addr1) text,Concat('/sections/customer/index.aspx?Customer_id=',a.customer_id) link FROM customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id LEFT JOIN address c ON b.address_id = c.address_id WHERE b.project_mgr_member_id = @v0  AND c.address_id != 0 ORDER BY a.customer_name", new object[] {  loaded_user.id}));
	}
	private DataTable populate_controls()
	{
		return (Toolbox.doSQL_dt(@"SELECT c.address_id id, CONCAT(a.customer_name, ' - ', c.address_addr1) text,Concat('/sections/customer/index.aspx?Customer_id=',a.customer_id) link FROM customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id LEFT JOIN address c ON b.address_id = c.address_id WHERE b.controls_mgr_member_id = @v0  AND c.address_id != 0 ORDER BY a.customer_name", new object[] {  loaded_user.id}));
	}
	private DataTable populate_ISR()
	{
		return (Toolbox.doSQL_dt(@"SELECT c.address_id id, CONCAT(a.customer_name, ' - ', c.address_addr1) text,Concat('/sections/customer/index.aspx?Customer_id=',a.customer_id) link FROM customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id LEFT JOIN address c ON b.address_id = c.address_id WHERE b.isr_member_id = @v0  AND c.address_id != 0 ORDER BY a.customer_name", new object[] {  loaded_user.id}));
	}
	private DataTable populate_OSR()
	{
		return (Toolbox.doSQL_dt(@"SELECT c.address_id id, CONCAT(a.customer_name, ' - ', c.address_addr1) text,Concat('/sections/customer/index.aspx?Customer_id=',a.customer_id) link FROM customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id LEFT JOIN address c ON b.address_id = c.address_id WHERE b.osr_member_id = @v0  AND c.address_id != 0 ORDER BY a.customer_name", new object[] {  loaded_user.id}));
	}
	private DataTable populate_RAM()
	{
		return (Toolbox.doSQL_dt(@"SELECT c.address_id id, CONCAT(a.customer_name, ' - ', c.address_addr1) text,Concat('/sections/customer/index.aspx?Customer_id=',a.customer_id) link FROM customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id LEFT JOIN address c ON b.address_id = c.address_id WHERE b.ram_member_id = @v0  AND c.address_id != 0 ORDER BY a.customer_name", new object[] {  loaded_user.id}));
	}
	private DataTable populate_MAM()
	{
		return (Toolbox.doSQL_dt(@"SELECT c.address_id id, CONCAT(a.customer_name, ' - ', c.address_addr1) text,Concat('/sections/customer/index.aspx?Customer_id=',a.customer_id) link FROM customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id LEFT JOIN address c ON b.address_id = c.address_id WHERE b.mam_member_id = @v0  AND c.address_id != 0 ORDER BY a.customer_name", new object[] {  loaded_user.id}));
	}
	private DataTable populate_CISR()
	{
		return (Toolbox.doSQL_dt(@"SELECT c.address_id id, CONCAT(a.customer_name, ' - ', c.address_addr1) text,Concat('/sections/customer/index.aspx?Customer_id=',a.customer_id) link FROM customer a LEFT JOIN customer_sales_properties b ON a.customer_id = b.customer_id LEFT JOIN address c ON b.address_id = c.address_id WHERE b.cisr_member_id = @v0  AND c.address_id != 0 ORDER BY a.customer_name", new object[] {  loaded_user.id}));
	}
	
	private DataTable populate_quoteprocess()
	{
		return (Toolbox.doSQL_dt(@" SELECT a.quoteid id, CONCAT(b.quote_id, ' - ', c.customer_name) text, Concat('/sections/member/quote/index.aspx?a=g&quote_id=',b.quote_id,'&revision=',b.revision) link FROM quote_schedule a INNER JOIN quote_master b ON a.quoteid = b.quote_id AND b.active_revision = TRUE and (b.status_id < 6 or b.status_id > 9) LEFT JOIN customer c ON b.customer_id = c.customer_id WHERE a.pointperson = @v0  or a.estimator = @v0  or (a.stage1screening_mid = @v0  and a.stage1screening_date is null) or (a.schedule_produced_mid = @v0  and a.schedule_produced_date is null) or (a.manpower_information_collected_mid = @v0  and a.manpower_information_collected_date is null) or (a.customer_info_collected_mid = @v0  and a.customer_info_collected_date is null) or (a.market_info_collected_mid = @v0  and a.market_info_collected_date is null) or (a.finance_info_collected_mid = @v0  and a.finance_info_collected_date is null) or (a.recon_report_created_mid = @v0  and a.recon_report_created_date is null) or (a.quote_delivery_strategy_mid = @v0  and a.quote_delivery_strategy_date is null) or (a.recon_report_created_mid = @v0  and a.recon_report_created_date is null) or (a.project_estimated_mid = @v0  and a.project_estimated_date is null) or (a.worksheet_review_mid = @v0  and a.worksheet_review_date is null) or (a.stage6_final_review_mid = @v0  and a.stage6_final_review_date is null) or (a.quote_delivered_mid = @v0  and a.quote_delivered_date is null) or (a.stage6_final_review_mid = @v0  and a.stage6_final_review_date is null) or (a.followup1_mid = @v0  and a.followup1_date is null) or (a.followup2_mid = @v0  and a.followup2_date is null) or (a.convert_or_kill_mid = @v0  and a.convert_or_kill_date is null) or (a.post_mortem_complete_mid = @v0  and a.post_mortem_complete_date is null) or (a.rt1 = @v0  and (a.rt1_s4_approved is null or rt1_f_approved is null)) or (a.rt2 = @v0  and (a.rt2_s4_approved is null or rt2_f_approved is null)) or (a.rt3 = @v0  and (a.rt3_s4_approved is null or rt3_f_approved is null)) or (a.rt4 = @v0  and (a.rt4_s4_approved is null or rt4_f_approved is null)) ORDER BY id", new object[] {  loaded_user.id}));
	}
	private DataTable populate_offersediting()
	{
		return (Toolbox.doSQL_dt(@"SELECT a.id id, Concat('Offer for ',b.member_fullname,' - ',a.status, ' - Start date:', date(a.startdate)) text, Concat('/sections/hr/member/member_offer.aspx?id=',a.id) link from member_offers a LEFT JOIN member b ON a.memberid = b.member_id WHERE a.enteredby = @v0  AND a.status NOT IN ('Previous', 'Deleted', 'Closed', 'Accepted') and a.memberid > 0 ORDER BY member_fullname", new object[] {  loaded_user.id}));
	}
	private DataTable populate_reviewstodo()
	{
		return (Toolbox.doSQL_dt(@"SELECT a.id, Concat('Review for ',b.member_fullname,' - ',a.status, ' - scheduled for: ', a.date) text, Concat('/sections/hr/member/review_list_for_member.aspx?id=',a.id,'&memberid=',b.member_id) link FROM emp_review a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.reviewed_by_id = @v0  and a.status NOT IN ('Closed', 'Delivered') AND b.member_status = 'Active' ORDER BY b.member_fullname", new object[] {  loaded_user.id}));
	}
	private DataTable populate_scheduler()
	{
		return Toolbox.doSQL_dt(@"SELECT a.id, Concat('Scheduled ',c.member_fullname,' for ',a.subject, ' on ',date(a.startdate),' - ', appointment_status.status) text, Concat('/sections/member/scheduler/index1.aspx') link FROM appointments a LEFT JOIN member b ON a.setby = b.member_id left join appointment_status on a.status = appointment_status.id LEFT JOIN member c ON a.resourceid = c.member_id WHERE a.setby = @v0  AND b.member_status = 'Active' and a.startdate>curdate() ORDER BY b.member_fullname", new object[] {  loaded_user.id } );
	}
	private DataTable populate_ticketgroupman()
	{
		return Toolbox.doSQL_dt(@"SELECT ticket_group_id id, ticket_group_name text, '' link FROM ticket_group WHERE ticket_group_administrator = @v0  ", new object[] {  loaded_user.id } );
	}

	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var fieldValues = gv.GetSelectedFieldValues(new string[] { "id" });
		if (fieldValues.Count > 0)
		{
			if (e.Parameters.Length > 0)
			{
				if (e.Parameters == "close")
				{
					foreach (var item in fieldValues)
					{
						switch (tc.ActiveTab.Name)
						{
							case "Team Leader": Toolbox.doSQL_void(@"UPDATE member SET scheduled_by = @v0  WHERE member_id = @v1  limit 1", new object[] {  0, item } ); break;
							case "Scheduler": Toolbox.doSQL_void(@"UPDATE appointments SET setby = @v0  WHERE id = @v1  LIMIT 1", new object[] {  0, item } ); break;
							case "Messageboard": Toolbox.doSQL_void(@"DELETE FROM digitalsignage where digitalsignage_id=@v0 ", new object[] {  item } ); break;
							case "WO Freeze": Toolbox.doSQL_void(@"Delete appointments WHERE id = @v0  LIMIT 1", new object[] {  item } ); break;
							case "PO Approval Level": Toolbox.doSQL_void(@"delete from business_unit_PO_dist where id = @v0  limit 1", new object[] {  item } ); break;
						}
					}
				}
				else if (e.Parameters == "Leave")
				{

				}
				else
				{
					#region reassign
					var value_to = e.Parameters.Split('|').GetValue(1).ToString();
					customer_sales_properties csp;
					foreach (var item in fieldValues)
					{
						switch (tc.ActiveTab.Name)
						{
							case "Assigned Tickets":
								var t = new NETickets(Convert.ToInt32(item));
								// find out if the assignee is a member of the ticket group
								var x = _tools.getSQL_int(@"Select count(ticketmanager_id) from ticketmanager  where ticketmanager_member_id =@v0 and ticketmanager_group_id =@v1 ", new object[] { value_to,t.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_id });
								if (x == 0)// if not, add them to that group
								{
									using (var uow = new UnitOfWork())
									{
										var m = new ne_xpo.cs.ticketmanager(uow);
										m.ticketmanager_member_id = uow.GetObjectByKey<ne_xpo.cs.member>(Convert.ToInt32(value_to));
                                        m.ticketmanager_group_id = uow.GetObjectByKey<ne_xpo.cs.ticket_group>(t.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_id);
										m.Save();
										uow.CommitChanges();
									}
								}
								Toolbox.doSQL_void(@"UPDATE ticketheader SET ticketheader_member_assigned_id = @v0  WHERE ticketheader_id = @v1 ", new object[] {  value_to, item } ); break;

							case "WOs": Toolbox.doSQL_void(@"UPDATE woprog SET woprog_pm_memberid = @v0  WHERE woprog_id = @v1 ", new object[] {  value_to, item } ); break;
							case "POs": Toolbox.doSQL_void(@"UPDATE poprog_header SET poprog_cutby_member_id = @v0  WHERE poprog_id = @v1 ", new object[] {  value_to, item } ); break;
							case "Reports To":
								var split_from_id = item.ToString().Split('-');
								var from_id = split_from_id[1];
								switch (split_from_id[0])
								{

									case "M":
										if (!NeMember.Check_for_circular_org_chart(Convert.ToInt32(from_id), Convert.ToInt32(value_to)))
										{
											Toolbox.doSQL_void(@"UPDATE member SET reports_to = @v0  WHERE member_id = @v1  ", new object[] {  value_to, from_id } );
										}
										break;
									case "O":
										if (!NeMember.Check_for_circular_org_chart(Convert.ToInt32(from_id), Convert.ToInt32(value_to)))
										{
											Toolbox.doSQL_void(@"UPDATE member_offers SET reports_to = @v0  WHERE id = @v1  and status !='Previous'", new object[] {  value_to, from_id } );
										}
										break;
									case "A":
										Toolbox.doSQL_void(@"UPDATE applicants SET addedbymemberid = @v0  WHERE id = @v1  and status!='Hired' and status!='Deleted'", new object[] {  value_to, from_id } );
										break;
								}
								break;
								case "Payroll Handler": Toolbox.doSQL_void(@"UPDATE member SET payroll_handler = @v0  WHERE member_id = @v1  and member_id !=@v0 ", new object[] {  value_to, item } ); break;
							case "Quotes": Toolbox.doSQL_void(@"UPDATE quote_master SET quoted_by = @v0  WHERE quote_id = @v1 ", new object[] {  value_to, item } ); break;
							case "Account Manager": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.am_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "Project Manager": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.project_mgr_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "Controls": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.controls_mgr_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "ISR": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.isr_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "OSR": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.osr_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "RAM": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.ram_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "MAM": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.mam_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "CISR": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.cisr_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "Quote Process":
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
								Toolbox.doSQL_void(@"UPDATE quote_schedule SET rt1 = @v0  WHERE (rt1_s4_approved is null or rt1_f_approved is null) and rt1 = @v2  and quote_schedule_id = @v1  limit 1", new object[] {  value_to, item, loaded_user.id } );
								Toolbox.doSQL_void(@"UPDATE quote_schedule SET rt2 = @v0  WHERE (rt2_s4_approved is null or rt2_f_approved is null) and rt2 = @v2  and quote_schedule_id = @v1  limit 1", new object[] {  value_to, item, loaded_user.id } );
								Toolbox.doSQL_void(@"UPDATE quote_schedule SET rt3 = @v0  WHERE (rt3_s4_approved is null or rt3_f_approved is null) and rt3 = @v2  and quote_schedule_id = @v1  limit 1", new object[] {  value_to, item, loaded_user.id } );
								Toolbox.doSQL_void(@"UPDATE quote_schedule SET rt4 = @v0  WHERE (rt4_s4_approved is null or rt4_f_approved is null) and rt4 = @v2  and quote_schedule_id = @v1  limit 1", new object[] {  value_to, item, loaded_user.id } );
								break;
							case "Offers Editing": Toolbox.doSQL_void(@"UPDATE member_offers SET enteredby = @v0  WHERE id = @v1  LIMIT 1", new object[] {  value_to, item } ); break;
							case "Reviews to do": Toolbox.doSQL_void(@"UPDATE emp_review SET reviewed_by_id = @v0  WHERE id = @v1  LIMIT 1", new object[] {  value_to, item } ); break;
							case "Scheduler": Toolbox.doSQL_void(@"UPDATE appointments SET setby = @v0  WHERE id = @v1  LIMIT 1", new object[] {  value_to, item } ); break;
							case "Ticket Group Man": Toolbox.doSQL_void(@"UPDATE ticket_group SET ticket_group_administrator = @v0  WHERE ticket_group_id = @v1 ", new object[] {  value_to, item } ); break;

							case "FAQs": Toolbox.doSQL_void(@"UPDATE who_to_ask SET who_to_ask_memberid = @v0  WHERE who_to_ask_id = @v1 ", new object[] {  value_to, item } ); break;
							case "Cell Phones": Toolbox.doSQL_void(@"UPDATE cellphone SET member_id = @v0  WHERE id = @v1 ", new object[] {  value_to, item } ); break;
							case "Assets": Toolbox.doSQL_void(@"UPDATE assets SET assets_owner = @v0  WHERE assets_id = @v1 ", new object[] {  value_to, item } ); break;
							case "RFQs": Toolbox.doSQL_void(@"UPDATE rfq_header SET member_id = @v0  WHERE id = @v1 ", new object[] {  value_to, item } ); break;
							case "WO Freeze": Toolbox.doSQL_void(@"UPDATE appointments SET setby = @v0  where id = @v1  limit 1", new object[] {  value_to, item } ); break;
							case "PO Approval Level": Toolbox.doSQL_void(@"UPDATE business_unit_po_dist SET member_id = @v0  where id = @v1  limit 1", new object[] {  value_to, item } ); break;


						}
					}
					#endregion
				}
				DataBind();
				//		ScriptManager.RegisterStartupScript(this, typeof(string), "key3", "window.opener.location.href = window.opener.location.href;", true);

				gv.Selection.UnselectAll();
			}
		}
	}

	protected void update_quote_process(string field, object value_to, object item, string added_where)
	{
		Toolbox.doSQL_void(string.Format(@"UPDATE quote_schedule SET {0}  = @v0
WHERE {0}  = @v1  and quote_schedule_id = @v2  and {1} is null limit 1", field, added_where), new object[] { value_to, loaded_user.id, item } );

	}

	protected void btnclose_Init(object sender, EventArgs e)
	{
		var btn = (ASPxButton)sender;
		if (tc.ActiveTab != null)
		{
			switch (tc.ActiveTab.Name)
			{
				case "Assigned Tickets": btn.ClientVisible = false; break;
				case "WOs": btn.ClientVisible = false; break;
				case "POs": btn.ClientVisible = false; break;
				case "Reports To": btn.ClientVisible = false; break;
			
				case "Payroll Handler": btn.ClientVisible = false; break;
				case "Quotes": btn.ClientVisible = false; break;
				case "Account Manager": btn.ClientVisible = false; break;
				case "Project Manager": btn.ClientVisible = false; break;
				case "Controls": btn.ClientVisible = false; break;
				case "ISR": btn.ClientVisible = false; break;
				case "OSR": btn.ClientVisible = false; break;
				case "RAM": btn.ClientVisible = false; break;
				case "MAM": btn.ClientVisible = false; break;
				case "CISR": btn.ClientVisible = false; break;			
				case "Quote Process": btn.ClientVisible = false; break;
				case "Offers Editing": btn.ClientVisible = false; break;
				case "Reviews to do": btn.ClientVisible = false; break;
				case "Scheduler": btn.ClientVisible = true; break;
				case "Ticket Group Man": btn.ClientVisible = false; break;
				case "FAQs": btn.ClientVisible = false; break;
				case "Messageboard": btn.ClientVisible = true; break;
				case "Cell Phones": btn.ClientVisible = false; break;
				case "Inv Locations": btn.ClientVisible = false; break;
				case "Assets": btn.ClientVisible = false; break;
				case "RFQs": btn.ClientVisible = false; break;
				case "WO Freeze": btn.ClientVisible = true; break;
				case "PO Approval Level": btn.ClientVisible = true; break;
			}
		}

	}

	protected void btnleave_Init(object sender, EventArgs e)
	{	
		var btn = (ASPxButton)sender;
		if (tc.ActiveTab != null)
		{
			switch (tc.ActiveTab.Name)
			{
				case "Assigned Tickets": btn.ClientVisible = false; break;

				case "WOs": btn.ClientVisible = false; break;
				case "POs": btn.ClientVisible = false; break;
				case "Reports To": btn.ClientVisible = false; break;
				
				case "Payroll Handler": btn.ClientVisible = false; break;
				case "Quotes": btn.ClientVisible = false; break;
				case "Account Manager": btn.ClientVisible = false; break;
				case "Project Manager": btn.ClientVisible = false; break;
				case "Controls": btn.ClientVisible = false; break;
				case "ISR": btn.ClientVisible = false; break;
				case "OSR": btn.ClientVisible = false; break;
				case "RAM": btn.ClientVisible = false; break;
				case "MAM": btn.ClientVisible = false; break;
				case "CISR": btn.ClientVisible = false; break;			
				case "Quote Process": btn.ClientVisible = false; break;
				case "Offers Editing": btn.ClientVisible = false; break;
				case "Reviews to do": btn.ClientVisible = false; break;
				case "Scheduler": btn.ClientVisible = false; break;
				case "Ticket Group Man": btn.ClientVisible = false; break;
				case "FAQs": btn.ClientVisible = false; break;
				case "Messageboard": btn.ClientVisible = false; break;
				case "Cell Phones": btn.ClientVisible = false; break;
				case "Inv Locations": btn.ClientVisible = false; break;
				case "Assets": btn.ClientVisible = false; break;
				case "RFQs": btn.ClientVisible = false; break;
				case "WO Freeze": btn.ClientVisible = false; break;
				case "PO Approval Level": btn.ClientVisible = false; break;
			}
		}
	}
	
	protected void btnreassign_Init(object sender, EventArgs e)
	{
		var btn = (ASPxButton)sender;
		if (tc.ActiveTab != null)
		{
			switch (tc.ActiveTab.Name)
			{
				case "Assigned Tickets": btn.ClientVisible = true; break;

				case "WOs": btn.ClientVisible = true; break;
				case "POs": btn.ClientVisible = true; break;
				case "Reports To": btn.ClientVisible = true; break;
			
				case "Payroll Handler": btn.ClientVisible = true; break;
				case "Quotes": btn.ClientVisible = true; break;
				case "Account Manager": btn.ClientVisible = true; break;
				case "Project Manager": btn.ClientVisible = true; break;
				case "Controls": btn.ClientVisible = true; break;
				case "ISR": btn.ClientVisible = true; break;
				case "OSR": btn.ClientVisible = true; break;
				case "RAM": btn.ClientVisible = true; break;
				case "MAM": btn.ClientVisible = true; break;
				case "CISR": btn.ClientVisible = true; break;				
				case "Quote Process": btn.ClientVisible = true; break;
				case "Offers Editing": btn.ClientVisible = true; break;
				case "Reviews to do": btn.ClientVisible = true; break;
				case "Scheduler": btn.ClientVisible = true; break;
				case "Ticket Group Man": btn.ClientVisible = true; break;
				case "FAQs": btn.ClientVisible = true; break;
				case "Messageboard": btn.ClientVisible = false; break;
				case "Cell Phones": btn.ClientVisible = true; break;
				case "Inv Locations": btn.ClientVisible = true; break;
				case "Assets": btn.ClientVisible = true; break;
				case "RFQs": btn.ClientVisible = true; break;
				case "WO Freeze": btn.ClientVisible = false; break;
				case "PO Approval Level": btn.ClientVisible = true; break;

			}
		}
	}
	protected void tc_ActiveTabChanged(object source, DevExpress.Web.TabControlEventArgs e)
	{
		DataBind();
	}
	protected void ASPxButton1_Click(object sender, EventArgs e)
	{
		DataBind();
	}
	protected void usage_cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var fieldValues = gv.GetSelectedFieldValues(new string[] { "id" });
		if (fieldValues.Count > 0)
		{
			if (e.Parameter.Length > 0)
			{
				if (e.Parameter == "close")
				{
					foreach (var item in fieldValues)
					{
						switch (tc.ActiveTab.Name)
						{
							
							case "Scheduler": Toolbox.doSQL_void(@"UPDATE appointments SET setby = @v0  WHERE id = @v1  LIMIT 1", new object[] {  0, item } ); break;
							case "Messageboard": Toolbox.doSQL_void(@"DELETE FROM digitalsignage where digitalsignage_id=@v0 ", new object[] {  item } ); break;
							case "WO Freeze": Toolbox.doSQL_void(@"Delete appointments WHERE id = @v0  LIMIT 1", new object[] {  item } ); break;
							case "PO Approval Level": Toolbox.doSQL_void(@"delete from business_unit_PO_dist where id = @v0  limit 1", new object[] {  item } ); break;


						}
					}
				}
				else if (e.Parameter == "Leave")
				{

				}
				else
				{
					#region reassign
					var value_to = e.Parameter.Split('|').GetValue(1).ToString();
					customer_sales_properties csp;
					foreach (var item in fieldValues)
					{
						switch (tc.ActiveTab.Name)
						{
							case "Assigned Tickets":
								var t = new NETickets(Convert.ToInt32(item));
								// find out if the assignee is a member of the ticket group
								var x = _tools.getSQL_int(@"Select count(ticketmanager_id) from ticketmanager  where ticketmanager_member_id =@v0 and ticketmanager_group_id =@v1 ", new object[] { value_to,t.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id });
								if (x == 0)// if not, add them to that group
								{
									using(var uow = new UnitOfWork())
										{
										var m		= new ne_xpo.cs.ticketmanager(uow);
										m.ticketmanager_member_id		= uow.GetObjectByKey<ne_xpo.cs.member>(Convert.ToInt32(value_to));
										m.ticketmanager_group_id		= uow.GetObjectByKey<ne_xpo.cs.ticket_group>(t.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id);
										m.Save();
										uow.CommitChanges();
										}
								}
								Toolbox.doSQL_void(@"UPDATE ticketheader SET ticketheader_member_assigned_id = @v0  WHERE ticketheader_id = @v1 ", new object[] {  value_to, item } ); break;

							case "WOs": Toolbox.doSQL_void(@"UPDATE woprog SET woprog_pm_memberid = @v0  WHERE woprog_id = @v1 ", new object[] {  value_to, item } ); break;
							case "POs": Toolbox.doSQL_void(@"UPDATE poprog_header SET poprog_cutby_member_id = @v0  WHERE poprog_id = @v1 ", new object[] {  value_to, item } ); break;
							case "Reports To":
								var split_from_id = item.ToString().Split('-');
								var from_id = split_from_id[1];
								switch (split_from_id[0])
								{
									case "M":
										Toolbox.doSQL_void(@"UPDATE member SET reports_to = @v0  WHERE member_id = @v1 ", new object[] {  value_to, from_id } );
										break;
									case "O":
										Toolbox.doSQL_void(@"UPDATE member_offers SET reports_to = @v0  WHERE id = @v1 ", new object[] {  value_to, from_id } );
										break;
									case "A":
										Toolbox.doSQL_void(@"UPDATE applicants SET addedbymemberid = @v0  WHERE id = @v1 ", new object[] {  value_to, from_id } );
										break;
								}
								break;
							
							case "Payroll Handler": Toolbox.doSQL_void(@"UPDATE member SET payroll_handler = @v0  WHERE member_id = @v1 ", new object[] {  value_to, item } ); break;
							case "Quotes": Toolbox.doSQL_void(@"UPDATE quote_master SET quoted_by = @v0  WHERE quote_id = @v1 ", new object[] {  value_to, item } ); break;
							case "Account Manager": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.am_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "Project Manager": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.project_mgr_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "Controls": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.controls_mgr_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "ISR": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.isr_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "OSR": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.osr_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "RAM": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.ram_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "MAM": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.mam_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "CISR": csp = new customer_sales_properties(Convert.ToInt32(item)); csp.cisr_member_id = Convert.ToInt32(value_to); csp.save(); break;
							case "Quote Process":
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
								Toolbox.doSQL_void(@"UPDATE quote_schedule SET rt1 = @v0  WHERE (rt1_s4_approved is null or rt1_f_approved is null) and rt1 = @v2  and quote_schedule_id = @v1  limit 1", new object[] {  value_to, item, loaded_user.id } );
								Toolbox.doSQL_void(@"UPDATE quote_schedule SET rt2 = @v0  WHERE (rt2_s4_approved is null or rt2_f_approved is null) and rt2 = @v2  and quote_schedule_id = @v1  limit 1", new object[] {  value_to, item, loaded_user.id } );
								Toolbox.doSQL_void(@"UPDATE quote_schedule SET rt3 = @v0  WHERE (rt3_s4_approved is null or rt3_f_approved is null) and rt3 = @v2  and quote_schedule_id = @v1  limit 1", new object[] {  value_to, item, loaded_user.id } );
								Toolbox.doSQL_void(@"UPDATE quote_schedule SET rt4 = @v0  WHERE (rt4_s4_approved is null or rt4_f_approved is null) and rt4 = @v2  and quote_schedule_id = @v1  limit 1", new object[] {  value_to, item, loaded_user.id } );
								break;
							case "Offers Editing": Toolbox.doSQL_void(@"UPDATE member_offers SET enteredby = @v0  WHERE id = @v1  LIMIT 1", new object[] {  value_to, item } ); break;
							case "Reviews to do": Toolbox.doSQL_void(@"UPDATE emp_review SET reviewed_by_id = @v0  WHERE id = @v1  LIMIT 1", new object[] {  value_to, item } ); break;
							case "Scheduler": Toolbox.doSQL_void(@"UPDATE appointments SET setby = @v0  WHERE id = @v1  LIMIT 1", new object[] {  value_to, item } ); break;
							case "Ticket Group Man": Toolbox.doSQL_void(@"UPDATE ticket_group SET ticket_group_administrator = @v0  WHERE ticket_group_id = @v1 ", new object[] {  value_to, item } ); break;

							case "FAQs": Toolbox.doSQL_void(@"UPDATE who_to_ask SET who_to_ask_memberid = @v0  WHERE who_to_ask_id = @v1 ", new object[] {  value_to, item } ); break;
							case "Cell Phones": Toolbox.doSQL_void(@"UPDATE cellphone SET member_id = @v0  WHERE id = @v1 ", new object[] {  value_to, item } ); break;
							case "Assets": Toolbox.doSQL_void(@"UPDATE assets SET assets_owner = @v0  WHERE assets_id = @v1 ", new object[] {  value_to, item } ); break;
							case "RFQs": Toolbox.doSQL_void(@"UPDATE rfq_header SET member_id = @v0  WHERE id = @v1 ", new object[] {  value_to, item } ); break;
							case "WO Freeze": Toolbox.doSQL_void(@"UPDATE appointments SET setby = @v0  where id = @v1  limit 1", new object[] {  value_to, item } ); break;
							case "PO Approval Level": Toolbox.doSQL_void(@"UPDATE business_unit_po_dist SET member_id = @v0  where id = @v1  limit 1", new object[] {  value_to, item } ); break;




						}
					}
					#endregion
				}
				DataBind();
				//		ScriptManager.RegisterStartupScript(this, typeof(string), "key3", "window.opener.location.href = window.opener.location.href;", true);

				gv.Selection.UnselectAll();
				
			}
		}
	
	}
}