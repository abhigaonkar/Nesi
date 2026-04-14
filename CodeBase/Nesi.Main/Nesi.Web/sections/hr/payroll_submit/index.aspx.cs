using nesi.core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

//using System.Net.Mail;



public partial class PayrollSubmittal : System.Web.UI.Page
	{
	NeMember current_user;
	private const int _page_id				= 44; // from Page table in DB
	private const string _page_description		= "Payroll Submittal";
	private bool can_see_wage;
	private List<string> reports_to_chain		= new List<string>();
	int c_id32	= 0;
	int c_id64	= 0;
	NeBusinessUnit this_company	= new NeBusinessUnit();
	bool is_debug	= false;
	NameValueCollection _q;
	payroll.approval_packet packet_app				= new payroll.approval_packet();

	protected void Page_Init(object sender, EventArgs e)
		{
		_q									= Request.QueryString;
#if DEBUG
        is_debug = !string.IsNullOrEmpty(_q["debug"]);
#endif

        if (is_debug)
			{
			packet_app.handler				= new NeMember(Convert.ToInt32(_q["approver_id"]));
			packet_app.payperiod_id			= Convert.ToInt32(_q["payperiod_id"]);
			packet_app.is_sent				= false;
			current_user					= packet_app.handler;
			}
		else
			{
			current_user					= Toolbox.do_handle_authentication(_page_id);
			packet_app.handler				= current_user;
			packet_app.payperiod_id			= payroll.current_pay_period();
			}
		sds_branches.SelectParameters[0].DefaultValue	= current_user.id.ToString();
		can_see_wage						= current_user.AuthenticatedForPrivilege(101);
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var menu							= new NeMenu(current_user, _page_id);
		divMenu.InnerHtml					= menu.MenuHTML;
		divSide.InnerHtml					= shared.PrintSidePanelHTML(current_user);
		var lbltemp							= (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text						= _page_description;
		if(combo_branch_selector.Value == null)
			{
			combo_branch_selector.Value			= current_user.business_unit.id;
			}
		packet_app.business_unit_id						= Convert.ToInt32(combo_branch_selector.Value);
		packet_app.your_to_approve					= nesi.core.payroll.get_full_list(packet_app.payperiod_id, packet_app.business_unit_id, packet_app.handler.id32, true, cb_showall.Checked ? 1 : 2);
		packet_app.your_to_approve_full				= nesi.core.payroll.get_full_list(packet_app.payperiod_id, packet_app.business_unit_id, packet_app.handler.id32, false, cb_showall.Checked ? 1 : 2);
		packet_app.full_branch_list					= nesi.core.payroll.get_full_list(packet_app.payperiod_id, packet_app.business_unit_id, 0, true, 0);
		var payperiod							= new NePayPeriod(packet_app.payperiod_id);
		if(is_debug)
			{
			//packet_app.your_to_approve.Clear();
			//packet_app.your_to_approve.Add(11);
			//packet_app.full_branch_list		= packet_app.your_to_approve;
			}
		sds_branches.SelectParameters[1].DefaultValue		= payperiod.StartDate;
		sds_branches.SelectParameters[2].DefaultValue		= payperiod.Enddate;
		sds_branches.SelectParameters[3].DefaultValue		= cb_showall.Checked ? "1" : "2";
		combo_branch_selector.DataBind();
		combo_branch_selector.ClientVisible = combo_branch_selector.Items.Count != 0;
		if(combo_branch_selector.Items.FindByValue(current_user.business_unit.id) == null)
			{
			combo_branch_selector.Items.Add(current_user.business_unit.name, current_user.business_unit.id);
			}
		var payroll								= new payroll(packet_app.business_unit_id, packet_app.payperiod_id);
		if(is_debug)
			{
			packet_app.is_sent					= false;
			}
		else
			{
			packet_app.is_sent						= payroll.sent;
			}

		var is_sent								= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM payroll_progress WHERE payperiod_id = @v0 AND business_unit_id = @v1 AND member_id = @v2", 
													new object[] { packet_app.payperiod_id, packet_app.business_unit_id, packet_app.handler.id}) == 1;
		var p_yourdone_branch_in_progress		= payperiod.end_date < DateTime.Now && packet_app.full_branch_list.Count > 0 && packet_app.your_to_approve.Count == 0 && is_sent;
		var p_branchdone_others_in_progress		= packet_app.is_sent && payperiod.end_date < DateTime.Now && packet_app.full_branch_list.Count == 0;
		var p_closed							= payperiod.end_date > DateTime.Now;
		var p_available							= !packet_app.is_sent && payperiod.end_date < DateTime.Now && (packet_app.your_to_approve.Count > 0 || !p_branchdone_others_in_progress && !p_yourdone_branch_in_progress);

		if(!p_closed && Toolbox.doSQL_int(@"SELECT COUNT(*) 
FROM (SELECT id, name text, REPORTS_TO_SANS_STATUS(@v0,id, @v1, @v2, 1) ids, 1 order_id from business_unit  
WHERE istest = 'F' GROUP BY id HAVING ids != '') w", new object[] { current_user.id, payperiod.StartDate, payperiod.Enddate}) > 0)
			{
			cb_showall.ClientVisible = true;
			}
		if(!is_debug)
			{
			if(p_closed)
				{
				panel_payroll_closed.Visible		= true;
				div_payroll_closed.InnerHtml		= string.Format("Payroll is currently closed, and will be available after <b>{0:yyyy-MM/dd hh:mm:ss}</b>.", payperiod.end_date);
				pc.Visible							= false;
				panel_youredone_inprogress.Visible	= false;
				panel_branchdone_inprogress.Visible	= false;
				}
			else if(p_yourdone_branch_in_progress)
				{
				panel_payroll_closed.Visible		= false;
				pc.Visible							= false;
				panel_youredone_inprogress.Visible	= true;
				panel_branchdone_inprogress.Visible	= false;
				}
			else if(p_branchdone_others_in_progress)
				{
				panel_payroll_closed.Visible		= false;
				pc.Visible							= false;
				panel_youredone_inprogress.Visible	= false;
				panel_branchdone_inprogress.Visible	= true;
				}
			else if(p_available)
				{
				panel_payroll_closed.Visible		= false;
				pc.Visible							= true;
				panel_youredone_inprogress.Visible	= false;
				panel_branchdone_inprogress.Visible	= false;
				}
			}
		else
			{
			panel_payroll_closed.Visible		= false;
			div_payroll_closed.InnerHtml		= string.Format("Payroll is currently closed, and will be available after <b>{0:yyyy-MM/dd hh:mm:ss}</b>.", payperiod.end_date);
			pc.Visible							= true;
			panel_youredone_inprogress.Visible	= false;
			panel_branchdone_inprogress.Visible	= false;
			}
		uc_summary.packet					= packet_app;
		uc_approval.packet					= packet_app;
		if(packet_app.your_to_approve.Count > 0)
			{
			packet_app.current_employee			= !string.IsNullOrEmpty(_q["id"]) 
													? new NeMember(Convert.ToInt32(_q["id"])) 
													: new NeMember(payroll.get_user_at(packet_app.your_to_approve, packet_app.payperiod_id, packet_app.business_unit_id, packet_app.handler.id32, 0, 3));
			uc_approval.load();
			//pc.TabPages[0].ClientEnabled		= true;
			pc.ActiveTabIndex					= 0;
			}
		else 
			{
			//pc.TabPages[0].ClientEnabled		= false;
			pc.ActiveTabIndex					= 1;
			}
	}
	protected void cbp_payroll_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{

		}

	protected void pc_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{

		}
	} 