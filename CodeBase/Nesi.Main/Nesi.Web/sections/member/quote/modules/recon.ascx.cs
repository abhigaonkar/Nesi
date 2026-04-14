using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class sections_member_quote_modules_recon : UserControl
	{
	public int quote_id  
		{
		get { var _quote_id = 0; if (ViewState["quote_id"] == null) { return _quote_id; } else { int.TryParse(ViewState["quote_id"].ToString(), out _quote_id); return _quote_id; } } 
		set {ViewState["quote_id"] = value.ToString();}
		}
	

	
	private const int _page_id			= 65;
	NeMember current_user;
	NeQuoteSchedule qs;
	Toolbox _tools			= new Toolbox();
	quote quote;
	NeBusinessUnit comp;
	List<int> reports_to_list;
	string quote_level = "0";

	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		current_user				= Toolbox.do_handle_authentication(Convert.ToInt32(_page_id));
		var _reports_to_list		= Toolbox.doSQL_string(@"SELECT IFNULL(REPORTS_TO(@v0), '')", current_user.id);
        if(_reports_to_list != "")
			{
			reports_to_list				= new List<int>(_reports_to_list.Split(',').Select(s => int.Parse(s)));
			}
		else
			{
			reports_to_list				= new List<int>();
			}
		}
	protected void Page_Load(object sender, EventArgs e)
	{
		hdn_quote_id.Value = quote_id.ToString();
		qs = new NeQuoteSchedule(hdn_quote_id.Value);
	

		quote = new quote(Convert.ToInt32(hdn_quote_id.Value));
		hdn_rev.Value = quote.Revision.ToString();
		comp = new NeBusinessUnit(quote.business_unit_id);
		 if (quote.expected_value >= comp.quote_level_3_start)
		{
			quote_level = "3";
			lbl_lvl.Text = "This is a Level 3 Quote, so ALL steps are required";
			
			if (!IsPostBack)
			{
				if (hdn_quote_id.Value != "")
				{
					fill_schedule();
				}
			}
			populate_recon_scores();
		}
		else if (quote.expected_value >= comp.quote_level_2_start)
		{
			quote_level = "2";
			lbl_lvl.Text = "This is a Level 2 Quote, so some steps are disabled";
			cust_info_tr.Visible = false;
		
			
			finance_info_tr.Visible = false;
			followup_two_tr.Visible = false;
			recon_created_tr.Visible = false;
			manpower_info_tr.Visible = false;
			market_info_tr.Visible = false;
			stage4_tr.Visible = false;
			if (!IsPostBack)
			{
				if (hdn_quote_id.Value != "")
				{
					fill_schedule();
				}
			}
		}
		

		hdn_cid.Value = quote.business_unit_id.ToString();
		
	}

	
	protected void fill_schedule()
	{
		//string completed_color = "#9CFF9C";
		//string incomplete_color = "White";
		hdn_quote_id.Value = quote_id.ToString();
		qs = new NeQuoteSchedule(hdn_quote_id.Value);
		if (qs.id != 0)
		{
			#region populate schedule
			dte_openquote.Date = qs.opendate;
			ddl_openquote.DataBind();
			if (ddl_openquote.Items.FindByValue(qs.openmid) == null)
			{
				var sub_m		= new NeMember(qs.openmid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_openquote.Items.Add(new ListEditItem(item_text, qs.openmid));
			}
			ddl_openquote.Value = qs.openmid;
			mem.Text = qs.opennotes;

			dte_screeningcompleted.Date = qs.stage1screening_date;
			ddl_screeningcompleted.DataBind();
			if (ddl_screeningcompleted.Items.FindByValue(qs.stage1screening_mid) == null){
				var sub_m		= new NeMember(qs.stage1screening_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_screeningcompleted.Items.Add(new ListEditItem(item_text, qs.stage1screening_mid));}
			ddl_screeningcompleted.Value = qs.stage1screening_mid;
			mem_screeningcompleted.Text = qs.stage1screening_notes;

			dte_scheduleproduced.Date = qs.schedule_produced_date;
			ddl_scheduleproduced.DataBind();
			if (ddl_scheduleproduced.Items.FindByValue(qs.schedule_produced_mid) == null)
			{
				var sub_m		= new NeMember(qs.schedule_produced_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_scheduleproduced.Items.Add(new ListEditItem(item_text, qs.schedule_produced_mid));
			}
			ddl_scheduleproduced.Value = qs.schedule_produced_mid;
			mem_scheduleproduced.Text = qs.schedule_produced_notes;
			chk_scheduleproduced.ImageUrl = qs.schedule_produced_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 
//			schedule_title.BgColor = qs.schedule_produced_date_completed != null ? completed_color : incomplete_color;
		
			dte_manpower.Date = qs.manpower_information_collected_date;
			ddl_manpower.DataBind();
			if (ddl_manpower.Items.FindByValue(qs.manpower_information_collected_mid) == null)
			{
				var sub_m		= new NeMember(qs.manpower_information_collected_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_manpower.Items.Add(new ListEditItem(item_text, qs.manpower_information_collected_mid));
			}
			ddl_manpower.Value = qs.manpower_information_collected_mid;
			mem_manpower.Text = qs.manpower_information_collected_notes;
			chk_manpower.ImageUrl = qs.manpower_information_collected_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 
//			manpower_title.BgColor = qs.manpower_information_collected_date_completed != null ? completed_color : incomplete_color;
			

			dte_customerinfo.Date = qs.customer_info_collected_date;
			ddl_customerinfo.DataBind();
			if (ddl_customerinfo.Items.FindByValue(qs.customer_info_collected_mid) == null)
			{
				var sub_m		= new NeMember(qs.customer_info_collected_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_customerinfo.Items.Add(new ListEditItem(item_text, qs.customer_info_collected_mid));
			}
			ddl_customerinfo.Value = qs.customer_info_collected_mid;
			mem_customerinfo.Text = qs.customer_info_collected_notes;
			chk_customerinfo.ImageUrl = qs.customer_info_collected_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 
			
//			customer_title.BgColor = qs.customer_info_collected_date_completed != null ? completed_color : incomplete_color;
			
			dte_marketinfo.Date = qs.market_info_collected_date;
			ddl_marketinfo.DataBind();
			if (ddl_marketinfo.Items.FindByValue(qs.market_info_collected_mid) == null)
			{
				var sub_m		= new NeMember(qs.market_info_collected_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_marketinfo.Items.Add(new ListEditItem(item_text, qs.market_info_collected_mid));
			}
			ddl_marketinfo.Value = qs.market_info_collected_mid;
			mem_marketinfo.Text = qs.market_info_collected_notes;
			chk_marketinfo.ImageUrl = qs.market_info_collected_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 

//			market_title.BgColor = qs.market_info_collected_date_completed != null ? completed_color : incomplete_color;

			dte_financeinfo.Date = qs.finance_info_collected_date;
			ddl_financeinfo.DataBind();
			if (ddl_financeinfo.Items.FindByValue(qs.finance_info_collected_mid) == null)
			{
				var sub_m		= new NeMember(qs.finance_info_collected_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_financeinfo.Items.Add(new ListEditItem(item_text, qs.finance_info_collected_mid));
			}
			ddl_financeinfo.Value = qs.finance_info_collected_mid;
			mem_financeinfo.Text = qs.finance_info_collected_notes;
			chk_financeinfo.ImageUrl = qs.finance_info_collected_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 

	//		finance_title.BgColor = qs.finance_info_collected_date_completed != null ? completed_color : incomplete_color;

			dte_reconinfo.Date = qs.recon_report_created_date;
			ddl_reconinfo.DataBind();
			if (ddl_reconinfo.Items.FindByValue(qs.recon_report_created_mid) == null)
			{
				var sub_m		= new NeMember(qs.recon_report_created_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_reconinfo.Items.Add(new ListEditItem(item_text, qs.recon_report_created_mid));
			}
			ddl_reconinfo.Value = qs.recon_report_created_mid;
			mem_reconinfo.Text = qs.recon_report_created_notes;
			chk_reconinfo.ImageUrl = qs.recon_report_created_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 

	//		recon_report_title.BgColor = qs.recon_report_created_date_completed != null ? completed_color : incomplete_color;

			if ((quote.status==11)&&(qs.recon_report_created_date_completed!=null))
			{
				stage4_title.InnerHtml = "<span class='blink_me'><strong>Stage 4 Go-No Go Review Meeting</strong></span>";
			}
			dte_gonogo.Date = qs.stage4_gono_date;
			mem_gonogo.Text = qs.stage4_gono_notes;
			if ((qs.rt1_s4_approved == null) || (qs.rt2_s4_approved == null) || (qs.rt3_s4_approved == null) || (qs.rt4_s4_approved == null))
			{
				chk_gonogo.ImageUrl = "~/images/icon/CheckGrey.png"; 

		//		stage4_title2.BgColor = incomplete_color;
			}
			else
			{
				chk_gonogo.ImageUrl = "~/images/icon/CheckGreen.png";

		//		stage4_title2.BgColor = completed_color;
			}

			
			dte_sellstrategy.Date = qs.quote_delivery_strategy_date;
			ddl_sellstrategy.DataBind();
			if (ddl_sellstrategy.Items.FindByValue(qs.quote_delivery_strategy_mid) == null)
			{
				var sub_m		= new NeMember(qs.quote_delivery_strategy_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_sellstrategy.Items.Add(new ListEditItem(item_text, qs.quote_delivery_strategy_mid));
			}
			ddl_sellstrategy.Value = qs.quote_delivery_strategy_mid;
			mem_sellstrategy.Text = qs.quote_delivery_strategy_notes;
	//		strategy_title.BgColor = qs.quote_delivery_strategy_date_completed != null ? completed_color : incomplete_color;
			chk_sellstrategy.ImageUrl = qs.quote_delivery_strategy_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 
			

			dte_projectestimated.Date = qs.project_estimated_date;
			ddl_projectestimated.DataBind();
			if (ddl_projectestimated.Items.FindByValue(qs.project_estimated_mid) == null)
			{
				var sub_m		= new NeMember(qs.project_estimated_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_projectestimated.Items.Add(new ListEditItem(item_text, qs.project_estimated_mid));
			}
			ddl_projectestimated.Value = qs.project_estimated_mid;
			mem_projectestimated.Text = qs.project_estimated_notes;
		//	estimated_title.BgColor = qs.project_estimated_date_completed != null ? completed_color : incomplete_color;
			chk_projectestimated.ImageUrl = qs.project_estimated_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 

			dte_worksheetreview.Date = qs.worksheet_review_date;
			ddl_worksheetreview.DataBind();
			if (ddl_worksheetreview.Items.FindByValue(qs.worksheet_review_mid) == null)
			{
				var sub_m		= new NeMember(qs.worksheet_review_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_worksheetreview.Items.Add(new ListEditItem(item_text, qs.worksheet_review_mid));
			}
			ddl_worksheetreview.Value = qs.worksheet_review_mid;
			mem_worksheetreview.Text = qs.worksheet_review_notes;
			
//			ws_review_title.BgColor = qs.worksheet_review_date_completed != null ? completed_color : incomplete_color;
			chk_worksheetreview.ImageUrl = qs.worksheet_review_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 

			dte_finalreview.Date = qs.stage6_final_review_date;
			mem_finalreview.Text = qs.stage6_final_review_notes;


			if ((quote.status==12)&&(qs.worksheet_review_date_completed!=null))
			{
				stage6_title.InnerHtml = "<span class='blink_me'><strong>Stage 6 Final Review Meeting</strong></span>";
			}
			if ((quote_level=="3")&&((qs.rt1_f_approved == null) || (qs.rt2_f_approved == null) || (qs.rt3_f_approved == null) || (qs.rt4_f_approved == null)))
			{
				chk_finalreview.ImageUrl = "~/images/icon/CheckGrey.png"; 
//				stage6_title2.BgColor = incomplete_color;
			}
			else if ((quote_level == "2") && ((qs.rt1_f_approved == null) && (qs.rt2_f_approved == null) && (qs.rt3_f_approved == null) && (qs.rt4_f_approved == null)))
			{
				chk_finalreview.ImageUrl = "~/images/icon/CheckGrey.png";
//				stage6_title2.BgColor = incomplete_color;
			}
			else
			{
				chk_finalreview.ImageUrl = "~/images/icon/CheckGreen.png";
//				stage6_title2.BgColor = completed_color;
			}

			dte_quotedelivered.Date = qs.quote_delivered_date;
			ddl_quotedelivered.DataBind();
			if (ddl_quotedelivered.Items.FindByValue(qs.quote_delivered_mid) == null)
			{
				var sub_m		= new NeMember(qs.quote_delivered_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_quotedelivered.Items.Add(new ListEditItem(item_text, qs.quote_delivered_mid));
			}
			chk_quotedelivered.ImageUrl = qs.quote_delivered_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 
//			quote_delivered_title.BgColor = qs.quote_delivered_date_completed != null ? completed_color : incomplete_color;
			ddl_quotedelivered.Value = qs.quote_delivered_mid;
			mem_quotedelivered.Text = qs.quote_delivered_notes;


			dte_followup1.Date = qs.followup1_date;
			ddl_followup1.DataBind();
			if (ddl_followup1.Items.FindByValue(qs.followup1_mid) == null)
			{
				var sub_m		= new NeMember(qs.followup1_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_followup1.Items.Add(new ListEditItem(item_text, qs.followup1_mid));
			}
			ddl_followup1.Value = qs.followup1_mid;
			mem_followup1.Text = qs.followup1_notes;

			dte_followup2.Date = qs.followup2_date;
			ddl_followup2.DataBind();
			if (ddl_followup2.Items.FindByValue(qs.followup2_mid) == null)
			{
				var sub_m		= new NeMember(qs.followup2_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_followup2.Items.Add(new ListEditItem(item_text, qs.followup2_mid));
			}
			ddl_followup2.Value = qs.followup2_mid;
			mem15.Text = qs.followup2_notes;

			dte_convertkill.Date = qs.convert_or_kill_date;
			ddl_convertkill.DataBind();
			if (ddl_convertkill.Items.FindByValue(qs.convert_or_kill_mid) == null)
			{
				var sub_m		= new NeMember(qs.convert_or_kill_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_convertkill.Items.Add(new ListEditItem(item_text, qs.convert_or_kill_mid));
			}
			ddl_convertkill.Value = qs.convert_or_kill_mid;
			mem16.Text = qs.convert_or_kill_notes;
			chk_convertkill.ImageUrl = qs.convert_or_kill_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 

//			convert_kill_title.BgColor = qs.convert_or_kill_date_completed != null ? completed_color : incomplete_color;

			dte_postmortem.Date = qs.post_mortem_complete_date;
			ddl_postmortem.DataBind();
			if (ddl_postmortem.Items.FindByValue(qs.post_mortem_complete_mid) == null)
			{
				var sub_m		= new NeMember(qs.post_mortem_complete_mid);
				var item_text	= sub_m.business_unit.ddl_name+" - "+sub_m.FullName;
				ddl_postmortem.Items.Add(new ListEditItem(item_text, qs.post_mortem_complete_mid));
			}
			ddl_postmortem.Value = qs.post_mortem_complete_mid;
			mem_postmortem.Text = qs.post_mortem_complete_notes;
			chk_postmortem.ImageUrl = qs.post_mortem_complete_date_completed != null ? "~/images/icon/CheckGreen.png" : "~/images/icon/CheckGrey.png"; 

//			post_mortem_title.BgColor = qs.post_mortem_complete_date_completed != null ? completed_color : incomplete_color;

			if (quote_level == "2")
			{
				chk_rt1_f.ClientVisible = qs.rt1 == comp.branch_manager.id;
				chk_rt2_f.ClientVisible = qs.rt2 == comp.branch_manager.id;
				chk_rt3_f.ClientVisible = qs.rt3 == comp.branch_manager.id;
				chk_rt4_f.ClientVisible = qs.rt4 == comp.branch_manager.id;
			}
			
			chk_rt1_s4.Text = new NeMember(Convert.ToInt32(qs.rt1)).Nickname;
			chk_rt2_s4.Text = new NeMember(Convert.ToInt32(qs.rt2)).Nickname;
			chk_rt3_s4.Text = new NeMember(Convert.ToInt32(qs.rt3)).Nickname;
			chk_rt4_s4.Text = new NeMember(Convert.ToInt32(qs.rt4)).Nickname;

			chk_rt1_s4.Checked = qs.rt1_s4_approved != null;
			chk_rt2_s4.Checked = qs.rt2_s4_approved != null;
			chk_rt3_s4.Checked = qs.rt3_s4_approved != null;
			chk_rt4_s4.Checked = qs.rt4_s4_approved != null;

			lbl_dte_rt1_s4.Text = qs.rt1_s4_approved!=null?_tools.getSQL_string("Select fun_time('" + Convert.ToDateTime(qs.rt1_s4_approved).ToString("yyyy-MM-dd HH:mm:ss") + "')",null): "";
			lbl_dte_rt2_s4.Text = qs.rt2_s4_approved != null ? _tools.getSQL_string("Select fun_time('" + Convert.ToDateTime(qs.rt2_s4_approved).ToString("yyyy-MM-dd HH:mm:ss") + "')", null) : "";
			lbl_dte_rt3_s4.Text = qs.rt3_s4_approved != null ? _tools.getSQL_string("Select fun_time('" + Convert.ToDateTime(qs.rt3_s4_approved).ToString("yyyy-MM-dd HH:mm:ss") + "')", null) : "";
			lbl_dte_rt4_s4.Text = qs.rt4_s4_approved != null ? _tools.getSQL_string("Select fun_time('" + Convert.ToDateTime(qs.rt4_s4_approved).ToString("yyyy-MM-dd HH:mm:ss") + "')", null) : "";

			chk_rt1_f.Text = chk_rt1_s4.Text;
			chk_rt2_f.Text = chk_rt2_s4.Text;
			chk_rt3_f.Text = chk_rt3_s4.Text;
			chk_rt4_f.Text = chk_rt4_s4.Text;

			chk_rt1_f.Checked = qs.rt1_f_approved != null;
			chk_rt2_f.Checked = qs.rt2_f_approved != null;
			chk_rt3_f.Checked = qs.rt3_f_approved != null;
			chk_rt4_f.Checked = qs.rt4_f_approved != null;

			lbl_dte_rt1_f.Text = qs.rt1_f_approved != null ? _tools.getSQL_string("Select fun_time('" + Convert.ToDateTime(qs.rt1_f_approved).ToString("yyyy-MM-dd HH:mm:ss") + "')", null) : "";
			lbl_dte_rt2_f.Text = qs.rt2_f_approved != null ? _tools.getSQL_string("Select fun_time('" + Convert.ToDateTime(qs.rt2_f_approved).ToString("yyyy-MM-dd HH:mm:ss") + "')", null) : "";
			lbl_dte_rt3_f.Text = qs.rt3_f_approved != null ? _tools.getSQL_string("Select fun_time('" + Convert.ToDateTime(qs.rt3_f_approved).ToString("yyyy-MM-dd HH:mm:ss") + "')", null) : "";
			lbl_dte_rt4_f.Text = qs.rt4_f_approved != null ? _tools.getSQL_string("Select fun_time('" + Convert.ToDateTime(qs.rt4_f_approved).ToString("yyyy-MM-dd HH:mm:ss") + "')", null) : "";


			html_estimating.Html = qs.estimating_concerns;
			html_sell.Html = qs.sell_strategy;
			#endregion
		}

		var hours = _tools.getSQL_double(@"select ifnull((Select sum(numberofhours) from membertime  where wotype = 'Quote' and left(membertime_workorder_id,6) =@v0),0) ", new object[] { quote_id });
		lblestimate.Text = hours + " of " + qs.allowedquotetime + " hours spent so far on ESTIMATING";


		#region permissions

		var clear_manpower = (ASPxButton)gv_manpower.FindTitleTemplateControl("btnclear_manpower_recon");
		var clear_market = (ASPxButton)gv_market.FindTitleTemplateControl("btnclear_market_recon");
		var clear_customer = (ASPxButton)gv_cr.FindTitleTemplateControl("btnclear_cust_recon");
		var clear_finance = (ASPxButton)gv_finance.FindTitleTemplateControl("btnclear_finance_recon");

		if (quote_level == "3")
		{
			ASPxPageControl1.TabPages[1].ClientEnabled = qs.schedule_produced_date_completed != null;
			ASPxPageControl1.TabPages[2].ClientEnabled = qs.schedule_produced_date_completed != null;
			ASPxPageControl1.TabPages[3].ClientEnabled = qs.schedule_produced_date_completed != null;
			ASPxPageControl1.TabPages[4].ClientEnabled = qs.schedule_produced_date_completed != null;
		}
		else
		{
			ASPxPageControl1.TabPages[1].ClientEnabled = false;
			ASPxPageControl1.TabPages[2].ClientEnabled = false;
			ASPxPageControl1.TabPages[3].ClientEnabled = false;
			ASPxPageControl1.TabPages[4].ClientEnabled = false;
		}

			ASPxPageControl1.TabPages[5].ClientEnabled = qs.schedule_produced_date_completed != null;
			ASPxPageControl1.TabPages[6].ClientEnabled = qs.schedule_produced_date_completed != null;
			ASPxPageControl1.TabPages[7].ClientEnabled = qs.schedule_produced_date_completed != null;
	
		var is_quotedbys_bm		= new NeMember((int) quote.quoted_by).business_unit.branch_manager.id == current_user.id;

		if ((qs.id!=0)&&(quote.allowed_to_quote))
		{
			dte_openquote.Date = quote.opendate;
			ddl_openquote.DataBind();
			if (ddl_openquote.Items.FindByValue(Convert.ToInt32(quote.quoted_by)) == null)
			{
				ddl_openquote.Items.Add(new ListEditItem(new NeMember(Convert.ToInt32(quote.quoted_by)).FullName, quote.quoted_by));
			}
			ddl_openquote.Value = Convert.ToInt32(quote.quoted_by);

			if ((qs.recon_report_created_date_completed != null)||(quote_level=="2"))
			{
				if (((qs.rt1_s4_approved == null) || (qs.rt2_s4_approved == null) || (qs.rt3_s4_approved == null) || (qs.rt4_s4_approved == null)) && (quote.status == 11) && (qs.recon_report_created_date_completed != null))
				{
					chk_rt1_s4.ClientEnabled = current_user.id == qs.rt1 || reports_to_list.Contains(qs.rt1);
					chk_rt2_s4.ClientEnabled = current_user.id == qs.rt2 || reports_to_list.Contains(qs.rt2);
					chk_rt3_s4.ClientEnabled = current_user.id == qs.rt3 || reports_to_list.Contains(qs.rt3);
					chk_rt4_s4.ClientEnabled = current_user.id == qs.rt4 || reports_to_list.Contains(qs.rt4);
				}
			}

			if (qs.worksheet_review_date_completed != null)
			{
				if ((qs.rt1_f_approved == null) || (qs.rt2_f_approved == null) || (qs.rt3_f_approved == null) || (qs.rt4_f_approved == null))
				{
					chk_rt1_f.ClientEnabled = current_user.id == qs.rt1 || reports_to_list.Contains(qs.rt1);
					chk_rt2_f.ClientEnabled = current_user.id == qs.rt2 || reports_to_list.Contains(qs.rt2);
					chk_rt3_f.ClientEnabled = current_user.id == qs.rt3 || reports_to_list.Contains(qs.rt3);
					chk_rt4_f.ClientEnabled = current_user.id == qs.rt4 || reports_to_list.Contains(qs.rt4);
				}
				
			}

			if ((current_user.id == qs.schedule_produced_mid) || NeMember.is_supervisor((int) qs.schedule_produced_mid, current_user.id) || (qs.schedule_produced_date_completed != null))
			{
				btn_emailschedule.ClientEnabled = true;
				if ((current_user.id == qs.schedule_produced_mid) || NeMember.is_supervisor((int) qs.schedule_produced_mid, current_user.id))
				{
					dte_manpower.ClientEnabled = ddl_manpower.ClientEnabled = true;
					dte_customerinfo.ClientEnabled = ddl_customerinfo.ClientEnabled = true;
					dte_marketinfo.ClientEnabled = ddl_marketinfo.ClientEnabled = true;
					dte_financeinfo.ClientEnabled = ddl_financeinfo.ClientEnabled = true;
					dte_reconinfo.ClientEnabled = ddl_reconinfo.ClientEnabled = true;
					dte_gonogo.ClientEnabled = true;
					dte_sellstrategy.ClientEnabled = ddl_sellstrategy.ClientEnabled = true;
					dte_projectestimated.ClientEnabled = ddl_projectestimated.ClientEnabled = true;
					dte_worksheetreview.ClientEnabled = ddl_worksheetreview.ClientEnabled = true;
					dte_finalreview.ClientEnabled = true;
					dte_quotedelivered.ClientEnabled = ddl_quotedelivered.ClientEnabled = true;
					dte_followup1.ClientEnabled = ddl_followup1.ClientEnabled = true;
					dte_followup2.ClientEnabled = ddl_followup2.ClientEnabled = true;
					dte_convertkill.ClientEnabled = ddl_convertkill.ClientEnabled = true;
					dte_postmortem.ClientEnabled = ddl_postmortem.ClientEnabled = true;
				}
			}
			if ((current_user.id == qs.recon_report_created_mid) || NeMember.is_supervisor((int) qs.recon_report_created_mid, current_user.id) && (qs.recon_report_created_date_completed == null))
				{
					if ((qs.customer_info_collected_date_completed != null) && (qs.finance_info_collected_date_completed != null) && (qs.market_info_collected_date_completed != null) && (qs.manpower_information_collected_date_completed != null))
					{
						btn_printrecon.ClientEnabled = true;
					}
				}
			if ((quote.allowed_to_quote_2)||(quote_level=="2"))
			{

				btn_print_final_quote0.ClientEnabled = true;
				if ((current_user.id == qs.quote_delivery_strategy_mid) || NeMember.is_supervisor((int) qs.quote_delivery_strategy_mid, current_user.id))
				{
					btnsell_done.ClientEnabled = true;
				}

				if ((current_user.id == qs.project_estimated_mid) || NeMember.is_supervisor((int) qs.project_estimated_mid, current_user.id))
				{
					btn_est_review.ClientEnabled = true;

				} 

				if ((current_user.id == qs.worksheet_review_mid) || NeMember.is_supervisor((int) qs.worksheet_review_mid, current_user.id))
				{
					btn_ws_review.ClientEnabled = true;
				}

				if ((current_user.id == qs.quote_delivered_mid) || NeMember.is_supervisor((int) qs.quote_delivered_mid, current_user.id))
				{
					if (((quote_level == "2") && (quote.PrintedDate != null))|| ((qs.rt1_f_approved != null) && (qs.rt2_f_approved != null) && (qs.rt3_f_approved != null) && (qs.rt4_f_approved != null) && (quote.PrintedDate != null)))
					{
						btn_delivered.ClientEnabled = true;
					}
				}
			}
			if ((current_user.id == qs.convert_or_kill_mid) || NeMember.is_supervisor((int) qs.convert_or_kill_mid, current_user.id) || is_quotedbys_bm)
			{
				btn_kill_quote.ClientEnabled = true;
				btn_kill_quote.Visible		= true;
				if (((quote_level == "2") && (quote.PrintedDate != null)) || ((qs.rt1_f_approved != null) && (qs.rt2_f_approved != null) && (qs.rt3_f_approved != null) && (qs.rt4_f_approved != null) && (quote.PrintedDate != null)))
				{
					btn_convert.ClientEnabled = true;
				}
			}

			if ((current_user.id == qs.customer_info_collected_mid))
			{
				if (qs.customer_info_collected_date_completed == null)
					btn_custrecon_done.ClientEnabled = true;
				else
					clear_customer.ClientEnabled = true;
			}

			if ((current_user.id == qs.market_info_collected_mid))
			{
				if (qs.market_info_collected_date_completed == null)
					btnmarket_done.ClientEnabled = true;
				else
					clear_market.ClientEnabled = true;
			}
			if ((current_user.id == qs.finance_info_collected_mid))
			{
				if (qs.finance_info_collected_date_completed == null)
					btnfinance_done.ClientEnabled = true;
				else
					clear_finance.ClientEnabled = true;
			}
			if ((current_user.id == qs.manpower_information_collected_mid))
			{
				if (qs.manpower_information_collected_date_completed == null)
					btnmanpower_done.ClientEnabled = true;
				else
					clear_manpower.ClientEnabled = true;
			}
			

		}
		else if(is_quotedbys_bm || NeMember.is_supervisor((int) quote.quoted_by, current_user.id))
			{
			btn_kill_quote.ClientEnabled = true;
			}

		if ((quote.status == 6) || (quote.status == 8) || (quote.status == 9))
		{
			btn_kill_quote.ClientEnabled = false;
			ddl_postmortem.ClientEnabled = false;
			dte_postmortem.ClientEnabled = false;
		}
		#endregion

		
	}
	protected void mem_PreRender(object sender, EventArgs e)
	{
		if (((ASPxMemo)sender).Text.Length > 50)
		{
			((ASPxMemo)sender).Height = Unit.Pixel(50);
		}
		else
		{
			((ASPxMemo)sender).Height = Unit.Pixel(15);
		}
	}
	#region recon stuff
	protected void populate_recon_scores()
	{
		hdn_quote_id.Value = quote_id.ToString();
		qs = new NeQuoteSchedule(hdn_quote_id.Value);

		var cb2 = (ASPxCallbackPanel)gv_manpower.FindTitleTemplateControl("cb2");
		((ASPxLabel)cb2.FindControl("lblmanpower_recon_score")).Text = get_score("Manpower").ToString();

		var cb1 = (ASPxCallbackPanel)gv_market.FindTitleTemplateControl("cb1");
		((ASPxLabel)cb1.FindControl("lblmarket_recon_score")).Text = get_score("Market").ToString();
		
		var cb = (ASPxCallbackPanel)gv_cr.FindTitleTemplateControl("cb");
		((ASPxLabel)cb.FindControl("lblcust_recon_score")).Text = get_score("Customer").ToString();

		var cb3 = (ASPxCallbackPanel)gv_finance.FindTitleTemplateControl("cb3");
		((ASPxLabel)cb3.FindControl("lblfinance_recon_score")).Text = get_score("Finance").ToString();
	}
	#region customer grid inline editing
	protected void ASPxCheckBox1_Init(object sender, EventArgs e)
	{

		
		var ddl = sender as ASPxCheckBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.CheckedChanged = string.Format("function (s, e) {{ cb.PerformCallback('c|{0}|' + s.GetValue()); }}", container.KeyValue);
		ddl.ClientEnabled = ((current_user.id == qs.customer_info_collected_mid || reports_to_list.Contains(qs.customer_info_collected_mid)) && qs.customer_info_collected_date_completed == null) ? true : false;
	}
	protected void ASPxMemo1_Init(object sender, EventArgs e)
	{
		
		var ddl = sender as ASPxMemo;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb.PerformCallback('n|{0}|' + s.GetText()); }}", container.KeyValue);
	}
	protected void cb_Callback(object sender, CallbackEventArgsBase e)
	{
		hdn_quote_id.Value = quote_id.ToString();
		qs = new NeQuoteSchedule(hdn_quote_id.Value);
		var cb = (ASPxCallbackPanel)sender;
		if (e.Parameter.Length > 1)
		{
			var id = e.Parameter.Split('|').GetValue(1).ToString();
			var value = e.Parameter.Split('|').GetValue(2).ToString();
			if (e.Parameter[0].ToString() == "c")
			{
				_tools.getSQL_void(@"update quote_process_question_history set checked =@v0 where id =@v1 ", new object[] { value , id});
				((ASPxLabel)cb.FindControl("lblcust_recon_score")).Text = get_score("Customer").ToString();
			}
			else if (e.Parameter[0].ToString() == "n")
			{
				_tools.getSQL_void(@"update quote_process_question_history set notes =@v0 where id = @v1", new object[] { value, id});
			}
		}
	}
	#endregion
	#region market grid inline editing

	protected void ASPxCheckBox2_Init(object sender, EventArgs e)
	{
		
		var ddl = sender as ASPxCheckBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.CheckedChanged = string.Format("function (s, e) {{ cb1.PerformCallback('c|{0}|' + s.GetValue()); }}", container.KeyValue);
		ddl.ClientEnabled = ((current_user.id == qs.market_info_collected_mid || reports_to_list.Contains(qs.market_info_collected_mid)) && qs.market_info_collected_date_completed == null) ? true : false;
	}
	protected void ASPxMemo2_Init(object sender, EventArgs e)
	{
		
		var ddl = sender as ASPxMemo;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb1.PerformCallback('n|{0}|' + s.GetText()); }}", container.KeyValue);
	}
	protected void cb1_Callback(object sender, CallbackEventArgsBase e)
	{
		hdn_quote_id.Value = quote_id.ToString();
		qs = new NeQuoteSchedule(hdn_quote_id.Value);
		var cb = (ASPxCallbackPanel)sender;
		if (e.Parameter.Length > 1)
		{
			var id = e.Parameter.Split('|').GetValue(1).ToString();
			var value = e.Parameter.Split('|').GetValue(2).ToString();
			if (e.Parameter[0].ToString() == "c")
			{
				_tools.getSQL_void(@"update quote_process_question_history set checked = " + value + " where id = " + id);
				((ASPxLabel)cb.FindControl("lblmarket_recon_score")).Text = get_score("Market").ToString();
			}
			else if (e.Parameter[0].ToString() == "n")
			{
				_tools.getSQL_void(@"update quote_process_question_history set notes =@v0  where id = @v1", new object[] { value, id } );
			}
		}
	}

	#endregion
	#region manpower grid inline editing

	protected void ASPxCheckBox3_Init(object sender, EventArgs e)
	{
		hdn_quote_id.Value = quote_id.ToString();
		qs = new NeQuoteSchedule(hdn_quote_id.Value);
		var ddl = sender as ASPxCheckBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.CheckedChanged = string.Format("function (s, e) {{ cb2.PerformCallback('c|{0}|' + s.GetValue()); }}", container.KeyValue);
		ddl.ClientEnabled = ((current_user.id == qs.manpower_information_collected_mid || reports_to_list.Contains(qs.manpower_information_collected_mid)) && qs.manpower_information_collected_date_completed == null) ? true : false;
	}
	protected void ASPxMemo3_Init(object sender, EventArgs e)
	{
		
		var ddl = sender as ASPxMemo;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb2.PerformCallback('n|{0}|' + s.GetText()); }}", container.KeyValue);
	}
	protected void cb2_Callback(object sender, CallbackEventArgsBase e)
	{
		hdn_quote_id.Value = quote_id.ToString();
		qs = new NeQuoteSchedule(hdn_quote_id.Value);
		var cb = (ASPxCallbackPanel)sender;
		if (e.Parameter.Length > 1)
		{
			var id = e.Parameter.Split('|').GetValue(1).ToString();
			var value = e.Parameter.Split('|').GetValue(2).ToString();
			if (e.Parameter[0].ToString() == "c")
			{
				_tools.getSQL_void(@"update quote_process_question_history set checked =@v0  where id =@v1 ", new object[] { value, id });
				((ASPxLabel)cb.FindControl("lblmanpower_recon_score")).Text = get_score("Manpower").ToString();
			}
			else if (e.Parameter[0].ToString() == "n")
			{
				_tools.getSQL_void(@"update quote_process_question_history set notes =@v0  where id =@v1 " , new object[] { value, id});
			}
		}
	}

	#endregion
	#region Finance grid inline editing

	protected void ASPxCheckBox4_Init(object sender, EventArgs e)
	{
	
		var ddl = sender as ASPxCheckBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.CheckedChanged = string.Format("function (s, e) {{ cb3.PerformCallback('c|{0}|' + s.GetValue()); }}", container.KeyValue);
		ddl.ClientEnabled = ((current_user.id == qs.finance_info_collected_mid || reports_to_list.Contains(qs.finance_info_collected_mid)) && qs.finance_info_collected_date_completed == null) ? true : false;
	}
	protected void ASPxMemo4_Init(object sender, EventArgs e)
	{
		
		var ddl = sender as ASPxMemo;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb3.PerformCallback('n|{0}|' + s.GetText()); }}", container.KeyValue);
	}
	protected void cb3_Callback(object sender, CallbackEventArgsBase e)
	{
		hdn_quote_id.Value = quote_id.ToString();
		qs = new NeQuoteSchedule(hdn_quote_id.Value);
		var cb = (ASPxCallbackPanel)sender;
		if (e.Parameter.Length > 1)
		{
			var id = e.Parameter.Split('|').GetValue(1).ToString();
			var value = e.Parameter.Split('|').GetValue(2).ToString();
			if (e.Parameter[0].ToString() == "c")
			{
				_tools.getSQL_void("update quote_process_question_history set checked = " + value + " where id = " + id);
				((ASPxLabel)cb.FindControl("lblfinance_recon_score")).Text = get_score("Finance").ToString();
			}
			else if (e.Parameter[0].ToString() == "n")
			{
				_tools.getSQL_void("update quote_process_question_history  set notes =@v0 where id = @v1", new object[] { value, id });
			}
		}
	}

	#endregion
	protected void btnclear_cust_recon_Click(object sender, EventArgs e)
	{
		
		_tools.getSQL_void(@"Delete s.* from quote_process_question_history s INNER JOIN quote_process_questions n ON s.question_id = n.id
where n.type = 'Customer' and s.quoteid=@v0", new object[] { hdn_quote_id.Value });
		qs.customer_info_collected_date_completed = null;
		qs.save();
		populate_cust_recon_onetime();
	}
	protected void populate_cust_recon_onetime()
	{
		var dt = _tools.getSQL_datatable(@"Select * from quote_process_questions  where status = 'Active' and type = 'Customer'" , null);
		foreach (DataRow dr in dt.Rows)
		{
			_tools.getSQL_void("insert into quote_process_question_history (quoteid,question_id) values (@v0,@v1)", new object[] { hdn_quote_id.Value, dr["id"]}); 
		}
		gv_cr.DataBind();
		populate_recon_scores();
	}
	protected void btnclear_market_recon_Click(object sender, EventArgs e)
	{
		
		_tools.getSQL_void(@"Delete s.* from quote_process_question_history s INNER JOIN quote_process_questions n 
ON s.question_id = n.id where n.type = 'Market' and s.quoteid=@v0", new object[] {hdn_quote_id.Value});
		qs.market_info_collected_date_completed = null;
		qs.save();
		populate_market_recon_onetime();
	}
	protected void populate_market_recon_onetime()
	{
		var dt = _tools.getSQL_datatable(@"Select * from quote_process_questions  where status = 'Active' and type = 'Market'" , null);
		foreach (DataRow dr in dt.Rows)
		{
			_tools.getSQL_void("insert into quote_process_question_history (quoteid,question_id) values (@v0,@v1)", new object[] { hdn_quote_id.Value,  dr["id"]});
		}
		gv_market.DataBind();
		populate_recon_scores();
	}
	protected void btnclear_manpower_recon_Click(object sender, EventArgs e)
	{
		
		_tools.getSQL_void(@"Delete s.* from quote_process_question_history s INNER JOIN quote_process_questions n ON s.question_id = n.id 
where n.type = 'Manpower' and s.quoteid=@v0", new object[] { hdn_quote_id.Value });
		qs.manpower_information_collected_date_completed = null;
		qs.save();

		populate_manpower_recon_onetime();
	}
	protected void populate_manpower_recon_onetime()
	{
		var dt = _tools.getSQL_datatable(@"Select * from quote_process_questions  where status = 'Active' and type = 'Manpower'" , null);
		foreach (DataRow dr in dt.Rows)
		{
			_tools.getSQL_void(@"insert into quote_process_question_history (quoteid,question_id) values (@v0,@v1)", new object[] { hdn_quote_id.Value, dr["id"] });
		}
		gv_manpower.DataBind();
		populate_recon_scores();
	}
	protected void btnclear_finance_recon_Click(object sender, EventArgs e)
	{
		
		_tools.getSQL_void(@"Delete s.* from quote_process_question_history s INNER JOIN quote_process_questions n ON s.question_id = n.id " +
		                "where n.type = 'Finance' and s.quoteid=@v0", new object[] { hdn_quote_id.Value });

		qs.finance_info_collected_date_completed = null;
		qs.save();

		populate_finance_recon_onetime();
	}
	protected void populate_finance_recon_onetime()
	{
		var dt = _tools.getSQL_datatable(@"Select * from quote_process_questions  where status = 'Active' and type = 'Finance'" , null);
		foreach (DataRow dr in dt.Rows)
		{
			_tools.getSQL_void(@"insert into quote_process_question_history (quoteid,question_id) values (@v0,@v1)", new object[] { hdn_quote_id.Value, dr["id"] });
		}
		gv_finance.DataBind();
		populate_recon_scores();
	}
	protected double get_score(string type)
	{
		return (_tools.getSQL_double(@"SELECT round(if ((sum(quote_process_questions.ifyes)<>0),sum(if (a.checked=1,quote_process_questions.ifyes,quote_process_questions.ifno))/sum(quote_process_questions.ifyes),0)*100,1) _percent_score FROM quote_process_question_history AS a INNER JOIN quote_process_questions ON a.question_id = quote_process_questions.id  WHERE a.quoteid =@v0 AND quote_process_questions.type =@v1 ", new object[] { hdn_quote_id.Value,type }));
	}
	#endregion
	protected void dte0_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxDateEdit;
		ddl.ClientSideEvents.DateChanged = string.Format("function (s, e) {{ cb10.PerformCallback('d|{0}|' + s.GetText()); }}", ddl.ID);
	}
	protected void ddl0_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb10.PerformCallback('a|{0}|' + s.GetValue()); }}", ddl.ID);
	}
	protected void mem0_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxMemo;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb10.PerformCallback('m|{0}|' + s.GetText()); }}", ddl.ID);
	}
	protected void cb_s4_review_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter.Length > 0)
		{
			switch (e.Parameter[0])
				{
					case '1': qs.rt1_s4_approved = qs.rt1_s4_approved == null ? qs.rt1_s4_approved = DateTime.Now : null;
							break;
					case '2': qs.rt2_s4_approved = qs.rt2_s4_approved == null ? qs.rt2_s4_approved = DateTime.Now : null;
							break;
					case '3':qs.rt3_s4_approved = qs.rt3_s4_approved == null ? qs.rt3_s4_approved = DateTime.Now : null;
							break;
					case '4': qs.rt4_s4_approved = qs.rt4_s4_approved == null ? qs.rt4_s4_approved = DateTime.Now : null;
							break;
				}
			qs.save();
			if ((qs.rt1_s4_approved!=null) && (qs.rt2_s4_approved!=null) && (qs.rt3_s4_approved!=null) && (qs.rt4_s4_approved!=null))
			{
				_tools.getSQL_void(@"update quote_master set allowed_to_quote=1,status_id = 1,allowed_to_quote_2=1
where quote_id = @v0 AND active_revision = 1" ,new object[] { qs.quoteid});
				var email = new NeEMail();
				var estimator				= new NeMember(Convert.ToInt32(qs.project_estimated_mid));
				var delivery_strategist	= new NeMember(Convert.ToInt32(qs.quote_delivery_strategy_mid));
				email.To = estimator.NEEmail;
				email.CC = delivery_strategist.NEEmail;
				email.Subject = string.Format("Quote {0} for {1} has just been approved for estimating & sell strategy development", qs.quoteid, quote.txtCustomerName);
				email.Body = string.Format("{0}: You have been allotted {1} hr(s) to estimate this project.<br/>{2}: Please develop the sell strategy for this quote.", estimator.FullName,qs.allowedquotetime, delivery_strategist.FullName);
				email.From = "nomail@" + Toolbox.app_setting("DomainForEmail");
				email.Send();

				var x = qs.member_list();
				foreach (var mid in x)
				{
					try
					{
						if (mid != qs.project_estimated_mid)
						{
							if (mid != 0)
							{
								email_notification(mid, "Quote " + qs.quoteid + " has been approved for quoting, please find your involvement in this email");
							}
						}
					}
					catch { }

				}

			}
			fill_schedule();
		}
	}
	protected void cb_f_review_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter.Length > 0)
		{
			switch (e.Parameter[0])
			{
				case '1':
					if (qs.rt1_f_approved == null)
					{
						qs.rt1_f_approved = DateTime.Now;
					}
					else
					{
						qs.rt1_f_approved = null;
					}
					break;
				case '2':
					if (qs.rt2_f_approved == null)
					{
						qs.rt2_f_approved = DateTime.Now;
					}
					else
					{
						qs.rt2_f_approved = null;
					}
					break;
				case '3':
					if (qs.rt3_f_approved == null)
					{
						qs.rt3_f_approved = DateTime.Now;
					}
					else
					{
						qs.rt3_f_approved = null;
					}
					break;
				case '4':
					if (qs.rt4_f_approved == null)
					{
						qs.rt4_f_approved = DateTime.Now;
					}
					else
					{
						qs.rt4_f_approved = null;
					}
					break;
			}
			qs.save();
			if (
				((quote_level == "2") && ((qs.rt1_f_approved != null) || (qs.rt2_f_approved != null) || (qs.rt3_f_approved != null) || (qs.rt4_f_approved != null)))||
				(qs.rt1_f_approved != null) && (qs.rt2_f_approved != null) && (qs.rt3_f_approved != null) && (qs.rt4_f_approved != null)
				
				)
			{
				_tools.getSQL_void(@"update quote_master set prev_status_id = status_id,allowed_to_quote=1,allowed_to_quote_2=1,status_id = 4
where quote_id =@v0  AND active_revision = 1", new object[] { qs.quoteid});
				var email = new NeEMail();
				email.To = new NeMember(Convert.ToInt32(qs.quote_delivered_mid)).NEEmail;
				email.Subject = "Quote " + qs.quoteid + " for " + quote.txtCustomerName + " has just been approved for final delivery";
				email.From = "nomail@" + Toolbox.app_setting("DomainForEmail");
				email.Body = "When you have delivered the quote, please click the 'DELIVERED' button on the quote strategy page";
				email.Send();
				var x = qs.member_list();
				foreach (var mid in x)
				{
					try
					{
						if (mid != qs.quote_delivered_mid)
						{
							if (mid != 0)
							{
								email_notification(mid, "Quote " + qs.quoteid + " has been approved for delivery.  It is now of status 'Waiting for Customer Approval'");
							}
						}
					}
					catch { }

				}
			}

			fill_schedule();
		}
	}
		protected void cb10_Callback(object sender, CallbackEventArgsBase e)
			{
			if (e.Parameter.Length > 0 && e.Parameter.Contains("|"))
				{
				var qs1 = new NeQuoteSchedule(hdn_quote_id.Value);
				var control = e.Parameter.Split('|').GetValue(1).ToString();
				var value = e.Parameter.Split('|').GetValue(2).ToString();
				#region Dates
				if (e.Parameter[0] == 'd')  // date change callback
					{
					var d = Convert.ToDateTime(value);
					switch (control)
						{
						case "dte_openquote":
							qs1.opendate = d;
						break;
						case "dte_screeningcompleted":
							qs1.stage1screening_date = d;
						break;
						case "dte_scheduleproduced":
							qs1.schedule_produced_date = d;
						break;
						case "dte_manpower":
							qs1.manpower_information_collected_date = d;
						break;
						case "dte_customerinfo":
							qs1.customer_info_collected_date = d;
						break;
						case "dte_marketinfo":
							qs1.market_info_collected_date = d;
						break;
						case "dte_financeinfo":
							qs1.finance_info_collected_date = d;
						break;
						case "dte_reconinfo":
							qs1.recon_report_created_date = d;
						break;
						case "dte_gonogo":
							qs1.stage4_gono_date = d;
						break;
						case "dte_sellstrategy":
							qs1.quote_delivery_strategy_date = d;
						break;
						case "dte_projectestimated":
							qs1.project_estimated_date = d;
						break;
						case "dte_worksheetreview":
							qs1.worksheet_review_date = d;
						break;
						case "dte_finalreview":
							qs1.stage6_final_review_date = d;
						break;
						case "dte_quotedelivered":
							qs1.quote_delivered_date = d;
						break;
						case "dte_followup1":
							qs1.followup1_date = d;
						break;
						case "dte_followup2":
							qs1.followup2_date = d;
						break;
						case "dte_convertkill":
							qs1.convert_or_kill_date = d;
						break;
						case "dte_postmortem":
							qs1.post_mortem_complete_date = d;
						break;
						}
					qs1.schedule_produced_date_completed = null;
					}
				#endregion Dates
				#region IDs
				else if (e.Parameter[0] == 'a')  // assignment change callback
					{
					var id = Convert.ToInt32(value);
					switch (control)
						{
						case "ddl_screeningcompleted":
							qs1.stage1screening_mid = id;
						break;
						case "ddl_scheduleproduced":
							qs1.schedule_produced_mid = id;
							qs1.pointperson = id;
						break;
						case "ddl_manpower":
							qs1.manpower_information_collected_mid = id;
						break;
						case "ddl_customerinfo":
							qs1.customer_info_collected_mid = id;
						break;
						case "ddl_marketinfo":
							qs1.market_info_collected_mid = id;
						break;
						case "ddl_financeinfo":
							qs1.finance_info_collected_mid = id;
						break;
						case "ddl_reconinfo":
							qs1.recon_report_created_mid = id;
						break;
						case "ddl_sellstrategy":
							qs1.quote_delivery_strategy_mid = id;
						break;
						case "ddl_projectestimated":
							qs1.project_estimated_mid = id;
						break;
						case "ddl_worksheetreview":
							qs1.worksheet_review_mid = id;
						break;
						case "ddl_quotedelivered":
							qs1.quote_delivered_mid = id;
						break;
						case "ddl_followup1":
							qs1.followup1_mid = id;
						break;
						case "ddl_followup2":
							qs1.followup2_mid = id;
						break;
						case "ddl_convertkill":
							qs1.convert_or_kill_mid = id;
						break;
						case "ddl_postmortem":
							qs1.post_mortem_complete_mid = id;
						break;
						}
					qs1.schedule_produced_date_completed = null;
					}
				#endregion IDs
				#region Memos
				else if (e.Parameter[0] == 'm')  // memo change callback
					{
					switch (control)
						{
						case "mem":
							qs1.opennotes = value;
						break;
						case "mem0":
							qs1.stage1screening_notes = value;
						break;
						case "mem1":
							qs1.schedule_produced_notes = value;
						break;
						case "mem2":
							qs1.manpower_information_collected_notes = value;
						break;
						case "mem3":
							qs1.customer_info_collected_notes = value;
						break;
						case "mem4":
							qs1.market_info_collected_notes = value;
						break;
						case "mem5":
							qs1.finance_info_collected_notes = value;
						break;
						case "mem6":
							qs1.recon_report_created_notes = value;
						break;
						case "mem7":
							qs1.stage4_gono_notes = value;
						break;
						case "mem8":
							qs1.quote_delivery_strategy_notes = value;
						break;
						case "mem10":
							qs1.project_estimated_notes = value;
						break;
						case "mem11":
							qs1.worksheet_review_notes = value;
						break;
						case "mem12":
							qs1.stage6_final_review_notes = value;
						break;
						case "mem13":
							qs1.quote_delivered_notes = value;
						break;
						case "mem14":
							qs1.followup1_notes = value;
						break;
						case "mem15":
							qs1.followup2_notes = value;
						break;
						case "mem16":
							qs1.convert_or_kill_notes = value;
						break;
						case "mem17":
							qs1.post_mortem_complete_notes = value;
						break;
						}
					}
				#endregion Memos
				qs1.save();
				}
			}
		protected void gv_finance_DataBound(object sender, EventArgs e)
		{
			populate_recon_scores();
		}
		protected void gv_manpower_DataBound(object sender, EventArgs e)
		{
			populate_recon_scores();
		}
		protected void ASPxButton1_Click(object sender, EventArgs e)
		{
			
			qs.estimating_concerns = html_estimating.Html;
			qs.save();
		}
		protected void ASPxButton2_Click(object sender, EventArgs e)
		{
			qs.sell_strategy = html_sell.Html;
			qs.save();
		}
		protected void btnmanpower_done_Click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var x = qs.member_list();
			foreach (var mid in x)
			{
				try
				{
					if (mid != 0)
					{
						email_notification(mid, current_user.FullName + " completed the Manpower Recon for Quote " + qs.quoteid);
					}
				}
				catch { }

			}
			qs.manpower_information_collected_date_completed = DateTime.Now;
			qs.save();
			gv_manpower.DataBind();
			fill_schedule();
		}
		protected void btnmarket_done_Click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var x = qs.member_list();
			foreach (var mid in x)
			{
				try
				{
					if (mid != 0)
					{
						email_notification(mid, current_user.FullName + " completed the Market Recon for Quote " + qs.quoteid);
					}
				}
				catch { }

			}
			qs.market_info_collected_date_completed = DateTime.Now;
			qs.save();
			fill_schedule();
		}
		protected void btn_custrecon_done_Click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var x = qs.member_list();
			foreach (var mid in x)
			{
				try
				{
					if (mid != 0)
					{
						email_notification(mid, current_user.FullName + " completed the Customer Recon for Quote " + qs.quoteid);
					}
				}
				catch { }

			}
			qs.customer_info_collected_date_completed = DateTime.Now;
			qs.save();
			fill_schedule();


		}
		protected void btnfinance_done_Click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var x = qs.member_list();
			foreach (var mid in x)
			{
				try
				{
					if (mid != 0)
					{
						email_notification(mid, current_user.FullName + " completed the Finance Recon for Quote " + qs.quoteid);
					}
				}
				catch { }

			}
			qs.finance_info_collected_date_completed = DateTime.Now;
			qs.save();
			fill_schedule();
		}
		protected void btnsell_done_Click(object sender, EventArgs e)
		{

			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);

			qs.sell_strategy = html_sell.Html;
			qs.save();

			var x = qs.member_list();
			foreach (var mid in x)
			{
				try
				{
					if (mid != 0)
					{
						email_notification(mid, current_user.FullName + " completed the Quote Delivery Strategy for Quote " + qs.quoteid);
					}
				}
				catch { }

			}
			qs.quote_delivery_strategy_date_completed = DateTime.Now;
			qs.save();
			fill_schedule();
		}
		protected void btn_emailschedule_Click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var x = qs.member_list();
			foreach (var mid in x)
			{
				if (mid != 0)
				{
					try
					{
						email_notification(mid, "Quote " + qs.quoteid + " Schedule.  You are Involved");
					}
					catch { }
				}
			}
			qs.schedule_produced_date_completed = DateTime.Now;
			qs.save();
			fill_schedule();
			
		}
		protected void btn_printrecon_Click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var x = qs.member_list();
			foreach (var mid in x)
			{
				try
				{
					if (mid != 0)
					{
						email_notification(mid, current_user.FullName + " Has the recon report ready for final review for Quote: " + qs.quoteid);
					}
				}
				catch { }

			}
			qs.recon_report_created_date_completed = DateTime.Now;
			qs.save();
			fill_schedule();
		}
		protected void btn_est_review_Click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
			qs.project_estimated_date_completed = DateTime.Now;
			qs.save();
			fill_schedule();
			var email = new NeEMail();
			email.To = new NeMember(Convert.ToInt32(qs.worksheet_review_mid)).NEEmail;
			email.Subject = "Quote " + qs.quoteid + " is waiting for your review of the worksheet";
			email.Send();
		}
		protected void btn_ws_review_Click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var x = qs.member_list();
			foreach (var mid in x)
			{
				try
				{
					if (mid != 0)
					{
						email_notification(mid, current_user.FullName + " completed the WorkSheet Review for Quote " + qs.quoteid);
					}
				}
				catch (Exception ee) { _tools.catch_error(ee); }

			}
			qs.worksheet_review_date_completed = DateTime.Now;
			qs.save();
			_tools.getSQL_void(@"UPDATE quote_master SET allowed_to_quote=1, prev_status_id=status_id, status_id= 12,allowed_to_quote_2=1
WHERE quote_id =@v0 AND active_revision = 1", new object[] {
				quote_id
								   });
			fill_schedule();
		}
		protected void btn_print_final_quote_Click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var revision_n				= Toolbox.doSQL_string(@"SELECT IFNULL(MAX(revision), 1) FROM quote_master 
WHERE quote_id = @v0 AND active_revision = true", new object[] {
				hdn_quote_id.Value});
			if ((quote_level=="3")&&(qs.rt1_f_approved != null) && (qs.rt2_f_approved != null) && (qs.rt3_f_approved != null) && (qs.rt4_f_approved != null))
			{
				ScriptManager.RegisterStartupScript(this, typeof(string), "key1", "javascript:boing('/sections/reports/print_quote/index.aspx?quoteid=" + hdn_quote_id.Value + revision_n+ "&from_process=1&type=w','Print Quote', 1035, 800);", true);
				_tools.getSQL_void(@"Update quote_master set status_id=2, last_print_date = curdate() 
where quote_id = @v0  and active_revision = 1", new object[] {
					hdn_quote_id.Value});
			}
			else if ((quote_level=="2")&&((qs.rt1_f_approved != null)||(qs.rt2_f_approved != null)||(qs.rt3_f_approved != null)||(qs.rt4_f_approved != null)))
			{
				ScriptManager.RegisterStartupScript(this, typeof(string), "key1", "javascript:boing('/sections/reports/print_quote/index.aspx?quoteid=" + hdn_quote_id.Value + revision_n+ "&from_process=1&type=w','Print Quote', 1035, 800);", true);
				_tools.getSQL_void(@"Update quote_master set status_id=2, last_print_date = curdate() 
where quote_id =@v0  and active_revision = 1", new object[] {
					hdn_quote_id.Value});
		}
			else
			{
				ScriptManager.RegisterStartupScript(this, typeof(string), "key1", "javascript:boing('/sections/reports/print_quote/index.aspx?quoteid=" + hdn_quote_id.Value + revision_n+ "&from_process=1&type=wo','Print Quote', 1035, 800);", true);
			}
			
		}
		protected void btn_delivered_click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var x = qs.member_list();
			foreach (var mid in x)
			{
				try
				{
					if (mid != 0)
					{
						email_notification(mid, current_user.FullName + " Delivered the Final Print Off for Quote " + qs.quoteid);
					}
				}
				catch { }

			}
			_tools.getSQL_void(@"Update quote_master set status_id=4, last_fax_date = curdate(), verified_date = curdate() 
where quote_id =@v0  and active_revision = 1", new object[] {
				hdn_quote_id.Value});
		qs.quote_delivered_date_completed = DateTime.Now;
			qs.save();
			fill_schedule();
		}
		protected void btn_convert_Click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var x = qs.member_list();
			foreach (var mid in x)
			{
				try
				{
					if (mid != 0)
					{
						email_notification(mid, current_user.FullName + " Converted the Quote: " + qs.quoteid + ".  In other words, we won the job!");
					}
				}
				catch { }

			}
			qs.convert_or_kill_date_completed = DateTime.Now;
			qs.save();
			fill_schedule();


			ScriptManager.RegisterStartupScript(this, typeof(string), "wo", "javascript:boing('/sections/workorder/index.aspx?woprog_id=0&business_unit_id=" + hdn_cid.Value + "&fromquoteid="+qs.quoteid+"','Work Order', 1035, 900);", true);

			
			//window.opener.window.document.location = "/sections/workorder/index.aspx?woprog_id=0&business_unit_id=" + business_unit_id;
		}
		protected void btn_kill_quote_Click(object sender, EventArgs e)
		{
			hdn_quote_id.Value = quote_id.ToString();
			qs = new NeQuoteSchedule(hdn_quote_id.Value);
//			int[] x = qs.member_list();
//			foreach (int mid in x)
//			{
//				try
//				{
	//				email_notification(mid, current_user.FullName + " Killed the Quote: " + qs.quoteid );
	//			}
	//			catch { }
//
//			}
//			
//			qs.convert_or_kill_date_completed = System.DateTime.Now;
//			qs.save();

			

			ScriptManager.RegisterStartupScript(this, typeof(string), "key", "window.parent.pop_close.Show();", true);

			
		}
		protected void btncancelstage1_pop_Click(object sender, EventArgs e)
		{
			//	pop_close.ShowOnPageLoad = false;
		}
		protected void pop_close_WindowCallback(object source, PopupWindowCallbackArgs e)
	{
		var mem = (ASPxMemo)pop_close.FindControl("mem_close_notes");
		var qs = new NeQuoteSchedule(quote.QuoteID.ToString());
		var comp = new NeBusinessUnit(quote.business_unit_id);
		var email = new NeEMail();

		email.Subject = "Quote " + quote.QuoteID + " for " + quote.txtCustomerName + " Has Been Closed ";
		email.To = new NeMember(Convert.ToInt32(quote.quoted_by)).NEEmail + ";";
		email.CC = new NeMember(Convert.ToInt32(current_user.reports_to)).NEEmail;
		email.isHTML = true;
		email.Body = mem.Text + "</br> The quote is now waiting for a POST mortem </br>";


        //string killed_at_stage = "0";
        //if (quote.status == 11)
        //{
        //    killed_at_stage = "1";
        //}
        //if (quote.status == 12)
        //{
        //    killed_at_stage = "1";
        //}
        //if (quote.status == 4)
        //{
        //    killed_at_stage = "2";
        //}


			_tools.getSQL_void(@"update quote_master set allowed_to_quote=0,status_id = 6,allowed_to_quote_2=0, killed_at_stage='0',
killed_by=@v0,killed_date=curdate(),why_killed=@v1 where quote_id = @v2 and active_revision = 1", new object[] {
				current_user.id,mem.Text,quote_id
								   });
		#region set up post mortem questions
		var dt = _tools.getSQL_datatable(@"Select * from quote_post_mortem_questions  where status='Active' and stage ='Stage 1'" , null);
		foreach (DataRow dr in dt.Rows)
		{
			if (_tools.getSQL_int(@"Select count(quote_post_mortem.id) from quote_post_mortem  where quote_id =@v0 and question_id=@v1 ", new object[] { quote.QuoteID,dr["id"] }) == 0)
			{
				_tools.getSQL_void(@"Insert into quote_post_mortem (question_id,date,memberid,quote_id) 
values(@v0,curdate(),@v1,@v2)", new object[] { dr["id"] , current_user.id , quote.QuoteID });
			}
		}

		#endregion
		if (qs.id > 0)
		{
			email.To += new NeMember(Convert.ToInt32(qs.rt1)).NEEmail + ";";
			email.To += new NeMember(Convert.ToInt32(qs.rt2)).NEEmail + ";";
			email.To += new NeMember(Convert.ToInt32(qs.rt3)).NEEmail + ";";
			email.To += new NeMember(Convert.ToInt32(qs.rt4)).NEEmail + ";";
		}
		email.Send();
		if (qs.id != 0)
		{
			qs.convert_or_kill_date_completed = DateTime.Now;
			qs.save();
		}
		pop_close.JSProperties["cp_close"] = "close";
//				ScriptManager.RegisterStartupScript(this, typeof(string), "key1", "window.parent.popstage1.Hide();window.parent.window.location.reload();", true);
		fill_schedule();
	}
		protected void email_notification(int to_mid, string message)
		{
			var qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var bg_pointperson = qs.pointperson == to_mid ? "Yellow" : "";
			var bg_estimator = qs.estimator == to_mid ? "Yellow" : "";
			var bg_rt1 = qs.rt1 == to_mid ? "Yellow" : "";
			var bg_rt2 = qs.rt2 == to_mid ? "Yellow" : "";
			var bg_rt3 = qs.rt3 == to_mid ? "Yellow" : "";
			var bg_rt4 = qs.rt4 == to_mid ? "Yellow" : "";
			var comp = new NeBusinessUnit(quote.business_unit_id);
			var email = new NeEMail();
			var m = new NeMember(Convert.ToInt32(to_mid));
			email.Subject = message;
			email.isHTML = true;
			email.Body = string.Format(@"<table style='width: 100%; font-family: Arial; font-size:11px; border-collapse: collapse;'>
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Quote:</b></td>
					<td><a href='{2}/sections/member/quote/index.aspx?a=g&quote_id={0}&revision={1}'>{0}</a></td><td width='70%'></td>
                </tr>
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Status:</b></td>
					<td>" + _tools.getSQL_string("Select status from quote_status where id =@v0 ",new object[] {  quote.status}) + @"</td><td width='70%'></td>
				</tr>
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Customer:</b></td>
					<td>" + quote.txtCustomerName + @"</td><td width='70%'></td>
				</tr>
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Contact:</b></td>
					<td>" + quote.txtContact + @"</td><td width='70%'></td>
				</tr>
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Due Date:</b></td>
					<td>" + Convert.ToDateTime(quote.date_due).ToString("F") + @"</td><td width='70%'></td>
				</tr>
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Expected Value:</b></td>
					<td>" + quote.expected_value.ToString("C0") + @"</td><td width='70%'></td>
				</tr>
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Business Unit:</b></td>
					<td>" + new NeBusinessUnit(quote.business_unit_id).name + @"</td><td width='70%'></td>
				</tr>
							<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Point Person:</b></td>
					<td BgColor='" + bg_pointperson + @"'>" + new NeMember(Convert.ToInt32(qs.pointperson)).FullName + @"</td><td width='70%'></td>
				</tr>
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Estimator:</b></td>
					<td BgColor='" + bg_estimator + @"'>" + new NeMember(Convert.ToInt32(qs.estimator)).FullName + @"</td><td width='70%'></td>
				</tr>", quote.QuoteID, quote.Revision, Toolbox.app_setting("Domain"));

			if (quote_level == "3")
			{
				email.Body += @"<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Quote Review Team:</b></td>
					<td><table style='font-size:11px; font-family: Arial; border-collapse: collapse;'><tr><td nowrap='nowrap' BgColor='" + bg_rt1 + @"'>" + new NeMember(Convert.ToInt32(qs.rt1)).FullName + ",</td><td nowrap='nowrap' BgColor='" + bg_rt2 + @"'>" + new NeMember(Convert.ToInt32(qs.rt2)).FullName + ",</td><td nowrap='nowrap' BgColor='" + bg_rt3 + @"'>" + new NeMember(Convert.ToInt32(qs.rt3)).FullName + ",</td><td nowrap='nowrap' BgColor='" + bg_rt4 + @"'>" + new NeMember(Convert.ToInt32(qs.rt4)).FullName + @"</td></tr></table></td><td width='70%'></td>
				</tr>";
			}

			email.Body += @"</table></br>
			<table style='width: 500px; font-size:11px; font-family: Arial; border-collapse: collapse;' cellpadding='5'>
				<tr>
					<td bgcolor='DarkBlue' color='White' nowrap='nowrap' style='padding: 2px 15px 2px 2px; color: #FFFFFF; font-size: 11px;'><b>Schedule</b></td>
					<td bgcolor='DarkBlue' color='White' nowrap='nowrap' style='padding: 2px 15px 2px 2px; color: #FFFFFF; font-size: 11px;'><b>Assigned To</b></td>
					<td bgcolor='DarkBlue' color='White' nowrap='nowrap' style='padding: 2px 15px 2px 2px; color: #FFFFFF; font-size: 11px;'><b>Due Date</b></td>
					<td bgcolor='DarkBlue' color='White' nowrap='nowrap' style='padding: 2px 15px 2px 2px; color: #FFFFFF; font-size: 11px;'><b>Notes</b></td>
				</tr>";

			if (quote.expected_value >= comp.quote_level_3_start)
			{
				email.Body += do_email_row(false,	"Stage 1 Screening Completed", qs.stage1screening_mid, m.id, qs.stage1screening_date, qs.stage1screening_notes);
				email.Body += do_email_row(true,	"Schedule Produced", qs.schedule_produced_mid, m.id, qs.schedule_produced_date, qs.schedule_produced_notes);
				email.Body += do_email_row(false,	"Manpower Information Collected", qs.manpower_information_collected_mid, m.id, qs.manpower_information_collected_date, qs.manpower_information_collected_notes);
				email.Body += do_email_row(true,	"Customer Information Collected", qs.customer_info_collected_mid, m.id, qs.customer_info_collected_date, qs.customer_info_collected_notes);
				email.Body += do_email_row(false,	"Market Information Collected", qs.market_info_collected_mid, m.id, qs.market_info_collected_date, qs.market_info_collected_notes);
				email.Body += do_email_row(true,	"Finance Information Collected", qs.finance_info_collected_mid, m.id, qs.finance_info_collected_date, qs.finance_info_collected_notes);
				email.Body += do_email_row(false,	"Recon Report Created", qs.recon_report_created_mid, m.id, qs.recon_report_created_date, qs.recon_report_created_notes);
				email.Body += do_email_row(true,	"Stage 4 Go-No Go Review Meeting", -1, m.id, qs.stage4_gono_date, qs.stage4_gono_notes);
				email.Body += do_email_row(true,	"Quote Delivery / Sell Strategy Developed", qs.quote_delivery_strategy_mid, m.id, qs.quote_delivery_strategy_date, qs.quote_delivery_strategy_notes);
				email.Body += do_email_row(false,	"Project Estimated", qs.project_estimated_mid, m.id, qs.project_estimated_date, qs.project_estimated_notes);
				email.Body += do_email_row(true,	"Estimate Worksheet Review", qs.worksheet_review_mid, m.id, qs.worksheet_review_date, qs.worksheet_review_notes);
				email.Body += do_email_row(false,	"Stage 6 Final Review Meeting", -1, m.id, qs.stage6_final_review_date, qs.stage6_final_review_notes);
				email.Body += do_email_row(true,	"Quote Delivered (Due Date)", qs.quote_delivered_mid, m.id, qs.quote_delivered_date, qs.quote_delivered_notes);
				email.Body += do_email_row(false,	"Convert or Kill", qs.convert_or_kill_mid, m.id, qs.convert_or_kill_date, qs.convert_or_kill_notes);
				//email.Body += do_email_row(true,	"Post Mortem Completed", qs.post_mortem_complete_mid, m.id, qs.post_mortem_complete_date, qs.post_mortem_complete_notes);
				email.Body += do_email_row(false,	"Follow Up 1", qs.followup1_mid, m.id, qs.followup1_date, qs.followup1_notes);
				email.Body += do_email_row(true,	"Follow Up 2", qs.followup2_mid, m.id, qs.followup2_date, qs.followup2_notes);
			}
			else if (quote.expected_value >= comp.quote_level_2_start)
			{
				email.Body += do_email_row(false, "Stage 1 Screening Completed", qs.stage1screening_mid, m.id, qs.stage1screening_date, qs.stage1screening_notes);
				email.Body += do_email_row(true,  "Quote Delivery / Sell Strategy Developed", qs.quote_delivery_strategy_mid, m.id, qs.quote_delivery_strategy_date, qs.quote_delivery_strategy_notes);
				email.Body += do_email_row(false, "Project Estimated", qs.project_estimated_mid, m.id, qs.project_estimated_date, qs.project_estimated_notes);
				email.Body += do_email_row(true,  "Estimate Worksheet Review", qs.worksheet_review_mid, m.id, qs.worksheet_review_date, qs.worksheet_review_notes);
				email.Body += do_email_row(false, "Quote Delivered (Due Date)", qs.quote_delivered_mid, m.id, qs.quote_delivered_date, qs.quote_delivered_notes);
				email.Body += do_email_row(true,  "Convert or Kill", qs.convert_or_kill_mid, m.id, qs.convert_or_kill_date, qs.convert_or_kill_notes);
			//	email.Body += do_email_row(false, "Post Mortem Completed", qs.post_mortem_complete_mid, m.id, qs.post_mortem_complete_date, qs.post_mortem_complete_notes);
				email.Body += do_email_row(true,  "Follow Up 1", qs.followup1_mid, m.id, qs.followup1_date, qs.followup1_notes);
				email.Body += do_email_row(false, "Follow Up 2", qs.followup2_mid, m.id, qs.followup2_date, qs.followup2_notes);
			}
			email.Body += @"</table>";
			email.To = m.NEEmail;
			email.From = "nomail@" + Toolbox.app_setting("DomainForEmail");
			email.Send();

		}
		private string do_email_row(bool is_even, string row_name, int should_be_id, int is_id, DateTime used_date, string notes)
			{
			var row_bgcolor		= !is_even ? "bgcolor='lavender'" : "";
			var cell_bgcolor		= should_be_id == -1 ? "transparent" : should_be_id == is_id ? "yellow" : "transparent";
			var member_name		= should_be_id == -1 
										? "Quote Review Team" 
										: should_be_id == 0 
											? "--" 
											: Toolbox.doSQL_string(@"SELECT member_fullname FROM member WHERE member_id = @v0 LIMIT 1", should_be_id);

            var date_due = "Unknown, contact quoter";
            if (used_date!=null && used_date.ToString()!="" && used_date.ToString().Trim()!="" && used_date.ToString().Length>0)
            {
                date_due = Toolbox.doSQL_string(@"SELECT FUN_TIME(@v0)", Toolbox.MySQL_longdt(used_date));
            }
            
		
            return string.Format(@"
		<tr {0}>
			<td nowrap='nowrap'>{1}:</td>
			<td nowrap='nowrap' bgcolor='{2}'>{3}</td>
			<td nowrap='nowrap'>{4}</td>
			<td>{5}</td>
		</tr>", 
				row_bgcolor, 		// {0}
				row_name, 			// {1}
				cell_bgcolor, 		// {2}
				member_name, 		// {3}
				date_due, 			// {4}
				notes				// {5}
				);
			}
		}