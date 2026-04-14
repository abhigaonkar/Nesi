using System;
using System.Collections.Generic;
using System.Data;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Quotes
{
	public class QuoteStrategyStage1 : QuoteStrategBase
	{
		public DataTable filters { get; set; }
		public string started_by { get; set; }
		public string completion { get; set; }
		public string value { get; set; }
		public string due { get; set; }
		public LabelValueInt[] ddl5 { get; set; }
		public LabelValueInt[] ddl6 { get; set; }
		public int rt1 { get; set; }
		public int rt2 { get; set; }
		public int rt3 { get; set; }
		public int rt4 { get; set; }
		public int rt5 { get; set; }
		public int rt6 { get; set; }

		public QuoteStrategyStage1(Employee user, int quoteId) : base(user, quoteId)
		{
			this.filters = GetStage1QuoteFilters();
			this.started_by = new NeMember(Convert.ToInt32(quote.quoted_by)).FullName;
			value = quote.expected_value.ToString("C2");
			due = quote.date_due != ""
				? Convert.ToDateTime(quote.date_due).ToLongDateString()
				: quote.Revision > 1
					? "(Revised Quote - To be provided)"
					: "Not Provided";
			completion = quote.completion_date != ""
				? Convert.ToDateTime(quote.completion_date).ToLongDateString()
				: quote.Revision > 1
					? "(Revised Quote - To be provided)"
					: "Not Provided";
			var listrt = new List<LabelValueInt>();
			var rm = bm;
			if (bm.reports_to != 0)
			{
				rm = new NeMember(Convert.ToInt32(bm.reports_to));
			}
			ddl6 = bllToolbox.doSQL_Array<LabelValueInt>(
				@"
				SELECT
					a.member_id value,
					CONCAT(b.ddl_Name, ' - ', a.member_fullname) AS label
				FROM
					member a
					JOIN business_unit b ON a.business_unit_id = b.id
						inner join memberpage p on a.member_id= p.memberpage_member_id 
						where a.member_status = 'Active' 
						and p.memberpage_page_id=65
						and p.active=1
					AND a.Member_MemberType_ID IN (4,5,11,12,17,23,24,25,29,33,34,39,38,49,53,52,55)
					AND a.business_unit_id = @p0
				UNION
				SELECT
					0 value,
					'Not Assigned' label
				ORDER BY label", quote.business_unit_id);

			ddl5 = bllToolbox.doSQL_Array<LabelValueInt>(
				@"
				SELECT
					a.member_id value,
					CONCAT(b.ddl_Name, ' - ', a.member_fullname) AS label
				FROM
					member a
					JOIN business_unit b ON a.business_unit_id = b.id
						inner join memberpage p on a.member_id= p.memberpage_member_id 
						where a.member_status = 'Active' 
						and p.memberpage_page_id=65
						and p.active=1 and
				FIND_IN_SET(b.id, @p0)
						and find_in_set(@p1,get_visible_business_units_group_concat(a.member_id))
				ORDER BY label", CurrentUser.VisibleBusinessUnits, quote.business_unit_id);

			if (quote_level == "2")
			{
				rt1 = bm.id;
				rt2 = 0;
				rt3 = 0;
				rt4 = 0;
				rt5 = quote.quoted_by;

			}
			else if (quote_level == "3")
			{
				rt1 = bm.reports_to;
				rt2 = bm.id;
				rt3 = rm.id;
				rt4 = quote.quoted_by;
				rt5 = bm.id;
			}
			rt6 = quote.quoted_by;
		}

		public DataTable GetStage1QuoteFilters()
		{
			return bllToolbox.doSQL_dt(
				@"SELECT quote_filter.quote_id, 
quote_filter_questions.filter_question question, 
quote_filter.result result, 
quote_filter.note 
FROM quote_filter 
INNER JOIN quote_filter_questions 
ON quote_filter.question_id = quote_filter_questions.id  
where quote_filter.quote_id =@v0",
				quote_id);
		}

		public string CloseStage1(string mem)
		{
			//			var qs = new NeQuoteSchedule(quote_id);
			//			var comp = new NeBusinessUnit(quote.business_unit_id);
			//			var email = new NeEMail();

			bllToolbox.doSQL_void(
				@"update quote_master set allowed_to_quote=0,status_id = 6,allowed_to_quote_2=0, killed_at_stage='0',
			killed_by=@v0,  
			killed_date=curdate(),
			why_killed=@v1 
			where quote_id =@v2  
			and active_revision = 1", current_user.id, mem, quote_id);
			return "Quote has been closed successfully.";
		}

		protected void email_notification(int to_mid, string message)
		{
			var current_revision = Convert.ToInt32(Revision);
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
			var contact = new NEContact(Convert.ToInt32(quote.txtContact));
			email.Body = string.Format(
                @"<table style='width: 100%; font-family: Arial; font-size:11px; border-collapse: collapse;'>
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
					<td>" + (current_revision > 1
					? "Not yet set<br/> <i>(Revised Quote)</i>"
					: Convert.ToDateTime(quote.date_due).ToString("F")) + @"</td><td width='70%'></td>
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
					<td BgColor='" + bg_pointperson + @"'>" + new NeMember(Convert.ToInt32(qs.pointperson)).FullName +
				@"</td><td width='70%'></td>
				</tr>
				<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Estimator:</b></td>
					<td BgColor='" + bg_estimator + @"'>" + new NeMember(Convert.ToInt32(qs.estimator)).FullName +
				@"</td><td width='70%'></td>
				</tr>" +
				(quote_level == "2"
					? ""
					: @"<tr>
					<td nowrap='nowrap' 
						style='padding: 2px 15px 2px 2px; font-size: 11px;'>
						<b>Quote Review Team:</b></td>
					<td><table style='font-size:11px; font-family: Arial; border-collapse: collapse;'><tr><td nowrap='nowrap' BgColor='" +
					  bg_rt1 + @"'>"
					  + new NeMember(Convert.ToInt32(qs.rt1)).FullName + ",</td><td nowrap='nowrap' BgColor='" + bg_rt2 + @"'>"
					  + new NeMember(Convert.ToInt32(qs.rt2)).FullName + ",</td><td nowrap='nowrap' BgColor='" + bg_rt3 + @"'>"
					  + new NeMember(Convert.ToInt32(qs.rt3)).FullName + ",</td><td nowrap='nowrap' BgColor='" + bg_rt4 + @"'>"
					  + new NeMember(Convert.ToInt32(qs.rt4)).FullName + @"</td></tr>") +
				@"</table>
</td><td width='70%'></td>
				</tr></table></br>", quote.QuoteID, quote.Revision,Toolbox.app_setting("Domain"));




			email.To = m.NEEmail;
			//		email.To = "aketelaars@newelectric.com";
			email.From = "noreply@" + Toolbox.app_setting("DomainForEmail");
			email.Send();

		}


		public string ApproveStage1(DTO.ViewModels.Page.Quotes.QuoteStrategyStage1 model)
		{
			var addr = new NEAddress(Convert.ToInt32(quote.address_id));
			var qway = 0;
			var current_revision = Convert.ToInt32(Revision);
			if (current_revision == 1)
			{
				var ts = Convert.ToDateTime(quote.date_due).Subtract(quote.opendate);
				qway = ts.Days / 4;
			}

			qs.opendate = quote.opendate;
			qs.openmid = quote.quoted_by;

			qs.quote_delivered_date = current_revision > 1
				? qs.quote_delivered_date
				: new DateTime(Convert.ToDateTime(quote.date_due).Year, Convert.ToDateTime(quote.date_due).Month,
					Convert.ToDateTime(quote.date_due).Day, 16, 0, 0);
			qs.quote_delivered_mid = addr.csp.am_member_id;

			qs.stage1screening_date = current_revision > 1 ? qs.stage1screening_date : DateTime.Now;
			qs.stage1screening_mid = current_user.id32;

			#region Schedule Produced

			qs.schedule_produced_date = quote.opendate.AddDays(1);
			qs.schedule_produced_mid = Convert.ToInt32(rt5);

			#endregion

			#region Field Info

			if (quote_level == "3")
			{
				qs.manpower_information_collected_date =
					current_revision > 1 ? qs.manpower_information_collected_date : qs.opendate.AddDays(qway);
				qs.manpower_information_collected_mid = bm.id32;
				qs.finance_info_collected_date = current_revision > 1 ? qs.finance_info_collected_date : qs.opendate.AddDays(qway);
				qs.finance_info_collected_mid = addr.csp.isr_member_id;
				qs.market_info_collected_date = current_revision > 1 ? qs.market_info_collected_date : qs.opendate.AddDays(qway);
				qs.market_info_collected_mid = addr.csp.isr_member_id;
				qs.customer_info_collected_date =
					current_revision > 1 ? qs.customer_info_collected_date : qs.opendate.AddDays(qway);
				qs.customer_info_collected_mid = addr.csp.am_member_id;
			}

			#endregion

			#region recon report and review

			if (quote_level == "3")
			{
				qs.recon_report_created_date = current_revision > 1 ? qs.recon_report_created_date : qs.opendate.AddDays(qway + 1);
				qs.recon_report_created_mid = qs.schedule_produced_mid;
				qs.stage4_gono_date = current_revision > 1 ? qs.stage4_gono_date : qs.opendate.AddDays(qway + 1);
			}

			#endregion

			#region quote strategy and estimate

			qs.quote_delivery_strategy_date = qs.quote_delivered_date.AddDays(-3);
			qs.quote_delivery_strategy_mid = addr.csp.am_member_id;
			qs.project_estimated_date = qs.quote_delivered_date.AddDays(-2);
			qs.project_estimated_mid = Convert.ToInt32(rt6);
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
			qs.convert_or_kill_date = current_revision > 1
				? qs.convert_or_kill_date
				: new DateTime(Convert.ToDateTime(quote.completion_date).Year, Convert.ToDateTime(quote.completion_date).Month,
					Convert.ToDateTime(quote.completion_date).Day);
			qs.convert_or_kill_mid = addr.csp.am_member_id;

			qs.post_mortem_complete_date = current_revision > 1 ? qs.convert_or_kill_date : qs.convert_or_kill_date.AddDays(1);
			qs.post_mortem_complete_mid = addr.csp.am_member_id;

			if (quote_level == "3")
			{
				qs.followup2_date = current_revision > 1 ? qs.convert_or_kill_date : qs.convert_or_kill_date.AddDays(-2);
				qs.followup2_mid = addr.csp.am_member_id;
			}

			#endregion

			qs.memberid = current_user.id32;
			qs.quoteid = quote_id;
			qs.allowedquotetime = model.hours;


			qs.rt1 = model.rt1;
			qs.rt2 = model.rt2;
			qs.rt3 = model.rt3;
			qs.rt4 = model.rt4;

			qs.pointperson = model.rt5;
			qs.allowedquotetime = model.hours;
			qs.estimator = model.rt6;
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


			if (quote.expected_value >= bu.quote_level_3_start)
			{
				if (current_revision == 1)
				{
					var dt = bllToolbox.doSQL_dt(@"Select * from quote_process_questions");
					foreach (DataRow dr in dt.Rows)
					{
						if (bllToolbox.doSQL_int(
								@"Select count(id) from quote_process_question_history  where quoteid =@v0 and question_id =@v1 ", quote.QuoteID, dr["id"]) == 0)
						{
							bllToolbox.doSQL_void(@"Insert into quote_process_question_history (question_id,dt,memberid,quoteid) values(@v0,curdate(),@v1,@v2)", dr["id"],
								current_user.id, quote.QuoteID);
						}
					}
				}
				bllToolbox.doSQL_void(@"UPDATE quote_master SET allowed_to_quote=1,status_id = 11, allowed_to_quote_2=0 
					WHERE quote_id = @v0 AND revision = @v1 LIMIT 1", quote_id, current_revision);
			}
			else
			{
				bllToolbox.doSQL_void(
					@"UPDATE quote_master SET allowed_to_quote=1, status_id = 12 WHERE quote_id = @v0 AND revision = @v1 LIMIT 1", quote_id, current_revision);
			}
			return "Stage 1 has been approved successfully.";
		}

	}
}
