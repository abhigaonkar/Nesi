using System;
using System.Web.UI;
using DevExpress.Web;
using System.Collections.Specialized;

using System.Data;
using nesi.core;

public partial class quote_approval_frame : Page
	{
	public int quote_id;
	NameValueCollection _q;

	

		private const int _page_id = 65;
		NeMember current_user;

		Toolbox _tools = new Toolbox();
		quote quote;
		NeBusinessUnit comp;
		string quote_level = "1";
		int current_revision = 0;
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		}
	protected void Page_Load(object sender, EventArgs e)
		{
			_q = Request.QueryString;
			quote_id = _q["qid"] != null ? Convert.ToInt32(_q["qid"]) : 0;
			if(quote_id == 0)
				{
				Toolbox.FriendlyException(Response, "Quote not supplied", "");
				}
			hdn_quote_id.Value = quote_id.ToString();
			quote = new quote(Convert.ToInt32(hdn_quote_id.Value));
			comp = new NeBusinessUnit(quote.business_unit_id);
			hdn_cid.Value = quote.business_unit_id.ToString();
	    SqlDataSource6.SelectCommand = string.Format(@"

SELECT
    a.member_id member_id,
    CONCAT(b.ddl_Name, ' - ', a.member_fullname) AS member_name
FROM
    member a
    JOIN business_unit b ON a.business_unit_id = b.id
WHERE a.member_status = 'Active'
    AND a.Member_MemberType_ID IN (4,5,11,12,17,23,24,25,29,33,34,39,38,49,53,52,55)
    AND a.business_unit_id = {0}
UNION
SELECT
    0 memberid,
    'Not Assigned' member_fullname
ORDER BY member_name", quote.business_unit_id);


	    SqlDataSource5.SelectCommand = string.Format(@"
SELECT
    a.member_id member_id,
    CONCAT(b.ddl_Name, ' - ', a.member_fullname) AS member_name
FROM
    member a
    JOIN business_unit b ON a.business_unit_id = b.id
WHERE a.member_status = 'Active' AND
b.id IN ({0})
ORDER BY member_name", new Current_User().visible_business_units);

            current_revision			= Toolbox.doSQL_int(string.Format(@"SELECT revision FROM quote_master WHERE quote_id = '{0}' AND active_revision = true", hdn_quote_id.Value));
		if (current_revision > 1)
				{
				// Bring in the existing data
				var qs		= new NeQuoteSchedule(hdn_quote_id.Value);
				txt_hours.Text			= qs.allowedquotetime.ToString();
				ddl_rt5.Value			= qs.pointperson;
				ddl_rt6.Value			= qs.estimator;
				ddl_rt1.Value			= qs.rt1;
				ddl_rt2.Value			= qs.rt2;
				ddl_rt3.Value			= qs.rt3;
				ddl_rt4.Value			= qs.rt4;
				}
			if (quote.expected_value>=comp.quote_level_3_start)
			{
				quote_level = "3";
			}
			else if (quote.expected_value>=comp.quote_level_2_start)
			{
				quote_level = "2";
			}

			if (!IsPostBack)
			{
				if (hdn_quote_id.Value != "")
				{
					fill_page();
				}
			}
		
		}

	protected void btnapprovestage1_Click(object sender, EventArgs e)
	{
		try
		{
			validate();
		}
		catch (Exception ex)
		{
			_tools.catch_error(ex);
			lblerror.InnerText = ex.Message;
			lblerror.Visible = true;
			return;
		}

		try
		{
			var bm = comp.branch_manager;
			if(bm.id == 0)
				{
				bm = new NeMember(37);
				}
			var addr = new NEAddress(Convert.ToInt32(quote.address_id));
			var qs = new NeQuoteSchedule(hdn_quote_id.Value);
			var qway = 0;
			if(current_revision == 1)
				{
				var ts = Convert.ToDateTime(quote.date_due).Subtract(quote.opendate);
				qway = ts.Days / 4;
				}

			qs.opendate = quote.opendate;
			qs.openmid = quote.quoted_by;

			qs.quote_delivered_date = current_revision > 1 ? qs.quote_delivered_date : new DateTime(Convert.ToDateTime(quote.date_due).Year, Convert.ToDateTime(quote.date_due).Month, Convert.ToDateTime(quote.date_due).Day, 16, 0, 0);
			qs.quote_delivered_mid = addr.csp.am_member_id;

			qs.stage1screening_date = current_revision > 1 ? qs.stage1screening_date : DateTime.Now;
			qs.stage1screening_mid = current_user.id32;

			#region Schedule Produced

			qs.schedule_produced_date = quote.opendate.AddDays(1);
			qs.schedule_produced_mid = Convert.ToInt32(ddl_rt5.Value);

			#endregion

			#region Field Info
			if (quote_level == "3")
			{
				qs.manpower_information_collected_date = current_revision > 1 ? qs.manpower_information_collected_date : qs.opendate.AddDays(qway);
				qs.manpower_information_collected_mid = bm.id32;
				qs.finance_info_collected_date = current_revision > 1 ? qs.finance_info_collected_date :  qs.opendate.AddDays(qway);
				qs.finance_info_collected_mid = addr.csp.isr_member_id;
				qs.market_info_collected_date = current_revision > 1 ? qs.market_info_collected_date :  qs.opendate.AddDays(qway);
				qs.market_info_collected_mid = addr.csp.isr_member_id;
				qs.customer_info_collected_date = current_revision > 1 ? qs.customer_info_collected_date :  qs.opendate.AddDays(qway);
				qs.customer_info_collected_mid = addr.csp.am_member_id;
			}
			#endregion

			#region recon report and review
			if (quote_level == "3")
			{
				qs.recon_report_created_date = current_revision > 1 ? qs.recon_report_created_date :  qs.opendate.AddDays(qway + 1);
				qs.recon_report_created_mid = qs.schedule_produced_mid;
				qs.stage4_gono_date = current_revision > 1 ? qs.stage4_gono_date : qs.opendate.AddDays(qway + 1);
			}
			#endregion

			#region quote strategy and estimate

			qs.quote_delivery_strategy_date = qs.quote_delivered_date.AddDays(-3);
			qs.quote_delivery_strategy_mid = addr.csp.am_member_id;
			qs.project_estimated_date = qs.quote_delivered_date.AddDays(-2);
			qs.project_estimated_mid = Convert.ToInt32(ddl_rt6.Value);
			qs.worksheet_review_date = qs.quote_delivered_date.AddDays(-1);
			qs.worksheet_review_mid = bm.id32;

			qs.stage6_final_review_date = qs.quote_delivered_date.AddDays(-1);

			#endregion

			#region delivery and afterwards
			if (quote_level == "3")
			{
				qs.followup1_date = qs.quote_delivered_date.AddDays(1);
				qs.followup1_mid = addr.csp.am_member_id;
			}
			qs.convert_or_kill_date = current_revision > 1 ? qs.convert_or_kill_date : new DateTime(Convert.ToDateTime(quote.completion_date).Year, Convert.ToDateTime(quote.completion_date).Month, Convert.ToDateTime(quote.completion_date).Day);
			qs.convert_or_kill_mid = addr.csp.am_member_id;

			qs.post_mortem_complete_date = current_revision > 1 ? qs.convert_or_kill_date :  qs.convert_or_kill_date.AddDays(1);
			qs.post_mortem_complete_mid = addr.csp.am_member_id;

			if (quote_level == "3")
			{
				qs.followup2_date = current_revision > 1 ? qs.convert_or_kill_date : qs.convert_or_kill_date.AddDays(-2);
				qs.followup2_mid = addr.csp.am_member_id;
			}

			#endregion

			qs.memberid = current_user.id32;
			qs.quoteid = Convert.ToInt32(hdn_quote_id.Value);

			try
			{
				qs.allowedquotetime = txt_hours.Text == "" ? 0 : Convert.ToDouble(txt_hours.Text);
			}
			catch (Exception ex) {
			_tools.catch_error(ex); }

			qs.rt1 = Convert.ToInt32(ddl_rt1.Value);
			qs.rt2 = Convert.ToInt32(ddl_rt2.Value);
			qs.rt3 = Convert.ToInt32(ddl_rt3.Value);
			qs.rt4 = Convert.ToInt32(ddl_rt4.Value);

			qs.pointperson = Convert.ToInt32(ddl_rt5.Value);
			qs.allowedquotetime = Convert.ToDouble(txt_hours.Text);
			qs.estimator = Convert.ToInt32(ddl_rt6.Value);
			qs.save();

			email_notification(qs.pointperson, "You've just been assigned as the point person for quote " + quote.QuoteID);
			email_notification(qs.estimator, "You've just been assigned as the estimator for quote " + quote.QuoteID);
			if (quote_level == "3")
			{
				email_notification(qs.rt1, "You've just been assigned as a review team member for quote " + quote.QuoteID);
				email_notification(qs.rt2, "You've just been assigned as a review team member for quote " + quote.QuoteID);
				email_notification(qs.rt3, "You've just been assigned as a review team member for quote " + quote.QuoteID);
				email_notification(qs.rt4, "You've just been assigned as a review team member for quote " + quote.QuoteID);
			}

		
			

			DataTable dt;
			if (quote.expected_value >= comp.quote_level_3_start)
				{
				if(current_revision == 1)
					{
					dt = _tools.getSQL_datatable(@"Select * from quote_process_questions"  , null);
					foreach (DataRow dr in dt.Rows)
						{
						if (_tools.getSQL_int(@"Select count(id) from quote_process_question_history  where quoteid =@v0 and question_id =@v1 ", new object[] { quote.QuoteID,dr["id"] }) == 0)
							{
							_tools.getSQL_void(@"Insert into quote_process_question_history (question_id,dt,memberid,quoteid) 
values(@v0,curdate(),@v1,@v2)", new object[] {  dr["id"] , current_user.id , quote.QuoteID });
							}
						}
					}
				_tools.getSQL_void(@"UPDATE quote_master SET allowed_to_quote=1,status_id = 11, allowed_to_quote_2=0 
WHERE quote_id = @v0 AND revision = @v1 LIMIT 1", new object[] { quote_id, current_revision});
				}
			else
				{
				_tools.getSQL_void(@"UPDATE quote_master SET allowed_to_quote=1, status_id = 12 WHERE quote_id = @v0 AND revision = @v1 LIMIT 1",
					new object[] { quote_id, current_revision});
				}
			ScriptManager.RegisterStartupScript(this, typeof(string), "key", "window.parent.popstage1.Hide();window.parent.window.location.href = window.parent.window.location.href;", true);
		}
		catch (Exception ee)
		{
			_tools.catch_error(ee);
			lblerror.InnerText = "Could not create or save quote schedule";
			lblerror.Visible = true;
			return;
		}
	}

	protected void email_notification(int to_mid, string message)
	{
		var qs = new NeQuoteSchedule(hdn_quote_id.Value);
		var bg_pointperson = qs.pointperson==to_mid? "Yellow" : "";
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
		var contact	= new NEContact(Convert.ToInt32(quote.txtContact));
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
						<b>Customer:</b></td>
					<td>" + quote.txtCustomerName + @"</td><td width='70%'></td>
				</tr>
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Contact:</b></td>
					<td>" + contact.name + @"</td><td width='70%'></td>
				</tr>
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Due Date:</b></td>
					<td>" + (current_revision > 1 ? "Not yet set<br/> <i>(Revised Quote)</i>" : Convert.ToDateTime(quote.date_due).ToString("F")) + @"</td><td width='70%'></td>
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
				</tr>		
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Quote Review Team:</b></td>
					<td><table style='font-size:11px; font-family: Arial; border-collapse: collapse;'><tr><td nowrap='nowrap' BgColor='" + bg_rt1 + @"'>" + new NeMember(Convert.ToInt32(qs.rt1)).FullName + ",</td><td nowrap='nowrap' BgColor='" + bg_rt2 + @"'>" + new NeMember(Convert.ToInt32(qs.rt2)).FullName + ",</td><td nowrap='nowrap' BgColor='" + bg_rt3 + @"'>" + new NeMember(Convert.ToInt32(qs.rt3)).FullName + ",</td><td nowrap='nowrap' BgColor='" + bg_rt4 + @"'>" + new NeMember(Convert.ToInt32(qs.rt4)).FullName + @"</td></tr></table></td><td width='70%'></td>
				</tr></table></br> ", quote.QuoteID, quote.Revision,Toolbox.app_setting("Domain"));





                email.To = m.NEEmail;
//		email.To = "aketelaars@newelectric.com";
		email.From = "noreply@" + Toolbox.app_setting("DomainForEmail");
		email.Send();

	}

	protected void validate()
	{
		try
		{
			var hours = Convert.ToDouble(txt_hours.Text);
		}
		catch
		{
			throw new Exception("The allowed quote time must set as a regular number");
		}

		if (quote_level == "3")
		{
			if ((ddl_rt1.Text == "") || (ddl_rt2.Text == "") || (ddl_rt3.Text == "") || (ddl_rt4.Text == ""))
			{
				throw new Exception("You must set all the review team members");
			}
			if ((ddl_rt5.Text == "") && (quote.expected_value > comp.quote_level_3_start))
			{
				throw new Exception("Because this is a level 3 quote, you must select a point person");
			}
			if ((ddl_rt1.Text == ddl_rt2.Text) || (ddl_rt2.Text == ddl_rt3.Text) || (ddl_rt3.Text == ddl_rt4.Text) || (ddl_rt1.Text == ddl_rt3.Text) || (ddl_rt1.Text == ddl_rt4.Text) || (ddl_rt2.Text == ddl_rt4.Text))
			{
				throw new Exception("You must have 4 different members for the quote review team");
			}
		}

	}

	protected void fill_page()
	{
	var bm = comp.branch_manager;
	var rm = bm;
	if (bm.reports_to!=0)
	{
		rm = new NeMember(Convert.ToInt32(bm.reports_to));
	}

		var dt = _tools.getSQL_datatable(@"SELECT quote_filter.quote_id, quote_filter_questions.filter_question, quote_filter.result, quote_filter.note FROM quote_filter INNER JOIN quote_filter_questions ON quote_filter.question_id = quote_filter_questions.id  where quote_filter.quote_id =@v0", new object[] { quote_id });
		filter_results.InnerHtml = "<table width=100%; style='border-collapse:collapse'>";
		
		var x = 0;
		foreach (DataRow dr in dt.Rows)
		{
			if (x == 0)
			{
				filter_results.InnerHtml += "<tr bgcolor='whitesmoke'><td>" + dr["filter_question"] + "</td><td align='center'>" + (Convert.ToBoolean(dr["result"]) == true ? "Yes" : "No") + "</td></tr>";
				x++;
			}
			else
			{
				filter_results.InnerHtml += "<tr bgcolor='white'><td>" + dr["filter_question"] + "</td><td align='center'>" + (Convert.ToBoolean(dr["result"]) == true ? "Yes" : "No") + "</td></tr>";
				x = 0;
			}

		}
		filter_results.InnerHtml += "</table>";



		if (quote_level == "2")
		{
			ddl_rt1.Value = bm.id;
			ddl_rt2.Value = 0;
			ddl_rt3.Value = 0;
			ddl_rt4.Value = 0;
			ddl_rt5.Value = quote.quoted_by;
		}
		else if (quote_level == "3")
		{
			ddl_rt1.Value = bm.reports_to;
			ddl_rt2.Value = bm.id;
			ddl_rt3.Value = rm.id;
			ddl_rt4.Value = quote.quoted_by;
			ddl_rt5.Value = bm.id;
		}

		
		ddl_rt6.Value = quote.quoted_by;
		lbl_pop1_completion.Text = quote.completion_date != "" ? Convert.ToDateTime(quote.completion_date).ToLongDateString() : quote.Revision > 1 ? "(Revised Quote - To be provided)" : "Not Provided";
		lbl_pop1_value.Text = quote.expected_value.ToString("C2");
		lbl_pop1_due.Text = quote.date_due != "" ? Convert.ToDateTime(quote.date_due).ToLongDateString() : quote.Revision > 1 ? "(Revised Quote - To be provided)" : "Not Provided";
		lbl_pop1_started_by.Text = new NeMember(Convert.ToInt32(quote.quoted_by)).FullName;
	//	lbl_hit_rate_of_estimator = _tools.get_double("Select (Select count(quoteid) from quote_master where quoted_by = " + ddl_rt6.Value + " and status_id = 5 and 
		pop_close_list.InnerHtml = "&nbsp;&nbsp;&nbsp;&nbsp;" + new NeMember(Convert.ToInt32(ddl_rt1.Value)).FullName + "</br>";
		pop_close_list.InnerHtml += "&nbsp;&nbsp;&nbsp;&nbsp;" + new NeMember(Convert.ToInt32(ddl_rt2.Value)).FullName + "</br>";
		pop_close_list.InnerHtml += "&nbsp;&nbsp;&nbsp;&nbsp;" + new NeMember(Convert.ToInt32(ddl_rt3.Value)).FullName + "</br>";
		pop_close_list.InnerHtml += "&nbsp;&nbsp;&nbsp;&nbsp;" + new NeMember(Convert.ToInt32(ddl_rt4.Value)).FullName + "</br>";

		if (quote_level != "3")
		{
			tr_rt1.Visible = false;
			tr_rt2.Visible = false;
			tr_rt3.Visible = false;
			tr_rt4.Visible = false;
		}
	}
	protected void btncancelstage1_pop_Click(object sender, EventArgs e)
	{
	//	pop_close.ShowOnPageLoad = false;
	}

	protected void pop_close_WindowCallback(object source, PopupWindowCallbackArgs e)
	{
		var mem = (ASPxMemo)pop_close.FindControl("mem_close_notes");
		var qs = new NeQuoteSchedule(hdn_quote_id.Value);
		var comp = new NeBusinessUnit(quote.business_unit_id);
		var email = new NeEMail();
		
		

		//email.Subject = "Quote " + quote.QuoteID + " for " + quote.txtCustomerName + " Has Been Closed at Stage 1!";
		//email.To = new NeMember(Convert.ToInt32(quote.quoted_by)).NEEmail + ";";
		//email.CC = new NeMember(Convert.ToInt32(current_user.reports_to)).NEEmail;
		//email.isHTML = true;
		//email.Body = mem.Text + "</br> The quote is now waiting for a POST mortem </br>";
		_tools.getSQL_void(@"update quote_master set allowed_to_quote=0,status_id = 6,allowed_to_quote_2=0, killed_at_stage='0',
killed_by=@v0,  
killed_date=curdate(),
why_killed=@v1 
where quote_id =@v2  
and active_revision = 1",
			new object[] {current_user.id,
			mem.Text,
			quote_id 
			});
		//#region set up post mortem questions
		//DataTable dt = _tools.getSQL_datatable(@"Select * from quote_post_mortem_questions  where status='Active' and stage ='Stage 1'" , null);
		//foreach (DataRow dr in dt.Rows)
		//{
		//	if (_tools.getSQL_int(@"Select count(quote_post_mortem.id) from quote_post_mortem  where quote_id =@v0 and question_id=@v1 ", new object[] { quote.QuoteID,dr["id"])==0) });
		//	}
		//}
		//
		//#endregion
		//try
		//{
		//	if (qs.id > 0)
		//	{
		//		email.To += new NeMember(Convert.ToInt32(qs.rt1)).NEEmail + ";";
		//		email.To += new NeMember(Convert.ToInt32(qs.rt2)).NEEmail + ";";
		//		email.To += new NeMember(Convert.ToInt32(qs.rt3)).NEEmail + ";";
		//		email.To += new NeMember(Convert.ToInt32(qs.rt4)).NEEmail + ";";
		//	}
		//	email.Send();
		//}
		//catch { }
	
		pop_close.JSProperties["cp_close"] = "close";
//		ScriptManager.RegisterStartupScript(this, typeof(string), "key1", "window.parent.popstage1.Hide();window.parent.window.location.reload();", true);

	}
}