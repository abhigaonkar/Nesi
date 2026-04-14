using System;
using System.Data;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Quotes;

namespace NESI.BLL.Pages.Quotes
{
	public class QuoteStrategyUpdate : QuoteStrategBase
	{

		public QuoteStrategyUpdate(Employee user, int quoteId) : base(user, quoteId)
		{

		}

		protected void sendNotifications(string message)
		{
			var x = qs.member_list();
			foreach (var mid in x)
			{
				try
				{
					if (mid != 0)
					{
						email_notification(mid, message);
					}
				}
				catch { }
			}
		}

		protected void email_notification(int to_mid, string message)
		{
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
					<td>" + bllToolbox.doSQL_string("Select status from quote_status where id =@v0 ", quote.status) + @"</td><td width='70%'></td>
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
				email.Body += do_email_row(false, "Stage 1 Screening Completed", qs.stage1screening_mid, m.id, qs.stage1screening_date, qs.stage1screening_notes);
				email.Body += do_email_row(true, "Schedule Produced", qs.schedule_produced_mid, m.id, qs.schedule_produced_date, qs.schedule_produced_notes);
				email.Body += do_email_row(false, "Manpower Information Collected", qs.manpower_information_collected_mid, m.id, qs.manpower_information_collected_date, qs.manpower_information_collected_notes);
				email.Body += do_email_row(true, "Customer Information Collected", qs.customer_info_collected_mid, m.id, qs.customer_info_collected_date, qs.customer_info_collected_notes);
				email.Body += do_email_row(false, "Market Information Collected", qs.market_info_collected_mid, m.id, qs.market_info_collected_date, qs.market_info_collected_notes);
				email.Body += do_email_row(true, "Finance Information Collected", qs.finance_info_collected_mid, m.id, qs.finance_info_collected_date, qs.finance_info_collected_notes);
				email.Body += do_email_row(false, "Recon Report Created", qs.recon_report_created_mid, m.id, qs.recon_report_created_date, qs.recon_report_created_notes);
				email.Body += do_email_row(true, "Stage 4 Go-No Go Review Meeting", -1, m.id, qs.stage4_gono_date, qs.stage4_gono_notes);
				email.Body += do_email_row(true, "Quote Delivery / Sell Strategy Developed", qs.quote_delivery_strategy_mid, m.id, qs.quote_delivery_strategy_date, qs.quote_delivery_strategy_notes);
				email.Body += do_email_row(false, "Project Estimated", qs.project_estimated_mid, m.id, qs.project_estimated_date, qs.project_estimated_notes);
				email.Body += do_email_row(true, "Estimate Worksheet Review", qs.worksheet_review_mid, m.id, qs.worksheet_review_date, qs.worksheet_review_notes);
				email.Body += do_email_row(false, "Stage 6 Final Review Meeting", -1, m.id, qs.stage6_final_review_date, qs.stage6_final_review_notes);
				email.Body += do_email_row(true, "Quote Delivered (Due Date)", qs.quote_delivered_mid, m.id, qs.quote_delivered_date, qs.quote_delivered_notes);
				email.Body += do_email_row(false, "Convert or Kill", qs.convert_or_kill_mid, m.id, qs.convert_or_kill_date, qs.convert_or_kill_notes);
				//email.Body += do_email_row(true,	"Post Mortem Completed", qs.post_mortem_complete_mid, m.id, qs.post_mortem_complete_date, qs.post_mortem_complete_notes);
				email.Body += do_email_row(false, "Follow Up 1", qs.followup1_mid, m.id, qs.followup1_date, qs.followup1_notes);
				email.Body += do_email_row(true, "Follow Up 2", qs.followup2_mid, m.id, qs.followup2_date, qs.followup2_notes);
			}
			else if (quote.expected_value >= comp.quote_level_2_start)
			{
				email.Body += do_email_row(false, "Stage 1 Screening Completed", qs.stage1screening_mid, m.id, qs.stage1screening_date, qs.stage1screening_notes);
				email.Body += do_email_row(true, "Quote Delivery / Sell Strategy Developed", qs.quote_delivery_strategy_mid, m.id, qs.quote_delivery_strategy_date, qs.quote_delivery_strategy_notes);
				email.Body += do_email_row(false, "Project Estimated", qs.project_estimated_mid, m.id, qs.project_estimated_date, qs.project_estimated_notes);
				email.Body += do_email_row(true, "Estimate Worksheet Review", qs.worksheet_review_mid, m.id, qs.worksheet_review_date, qs.worksheet_review_notes);
				email.Body += do_email_row(false, "Quote Delivered (Due Date)", qs.quote_delivered_mid, m.id, qs.quote_delivered_date, qs.quote_delivered_notes);
				email.Body += do_email_row(true, "Convert or Kill", qs.convert_or_kill_mid, m.id, qs.convert_or_kill_date, qs.convert_or_kill_notes);
				//	email.Body += do_email_row(false, "Post Mortem Completed", qs.post_mortem_complete_mid, m.id, qs.post_mortem_complete_date, qs.post_mortem_complete_notes);
				email.Body += do_email_row(true, "Follow Up 1", qs.followup1_mid, m.id, qs.followup1_date, qs.followup1_notes);
				email.Body += do_email_row(false, "Follow Up 2", qs.followup2_mid, m.id, qs.followup2_date, qs.followup2_notes);
			}
			email.Body += @"</table>";
			email.To = m.NEEmail;
			email.From = "nomail@" + Toolbox.app_setting("DomainForEmail");
			email.Send();

		}

		private string do_email_row(bool is_even, string row_name, int should_be_id, int is_id, DateTime used_date, string notes)
		{
			var row_bgcolor = !is_even ? "bgcolor='lavender'" : "";
			var cell_bgcolor = should_be_id == -1 ? "transparent" : should_be_id == is_id ? "yellow" : "transparent";
			var member_name = should_be_id == -1
				? "Quote Review Team"
				: should_be_id == 0
					? "--"
					: Toolbox.doSQL_string(@"SELECT member_fullname FROM member WHERE member_id = @v0 LIMIT 1", should_be_id);

			var date_due = "Unknown, contact quoter";
			if (used_date != null && used_date.ToString() != "" && used_date.ToString().Trim() != "" && used_date.ToString().Length > 0)
			{
				date_due = Toolbox.doSQL_string(@"SELECT FUN_TIME(@v0)", Toolbox.MySQL_longdt(used_date));
			}


			return $@"
		<tr {row_bgcolor}>
			<td nowrap='nowrap'>{row_name}:</td>
			<td nowrap='nowrap' bgcolor='{cell_bgcolor}'>{member_name}</td>
			<td nowrap='nowrap'>{date_due}</td>
			<td>{notes}</td>
		</tr>";
		}



		public DataExtra UpdateScheduleItem(NameFieldValue model)
		{
			var ret = "";
			switch (model.field)
			{
				case "date":
					updateDate(model);
					break;
				case "assign_to":
					updateAssignTo(model);
					break;
				case "notes":
					updateNotes(model);
					break;
				case "button_label":
					ret = buttonClick(model);
					break;
				case "check_value":
					ret = updateCheckValue(model);
					break;
				case "sell_strategy":
					if (model.name == "sell_strategy_save")
					{
						qs.sell_strategy = model.value;
						qs.save();
					}
					else
					{
						qs.sell_strategy = model.value;
						qs.quote_delivery_strategy_date_completed = DateTime.Now;
						qs.save();
						sendNotifications(current_user.FullName + " completed the " + "Quote Delivery Strategy" + " for Quote " + qs.quoteid);
					}
					break;
				case "project_estimated":
					if (model.name == "project_estimated_save")
					{
						qs.estimating_concerns = model.value;
						qs.save();
					}
					else
					{
						qs.project_estimated_date_completed = DateTime.Now;
						qs.save();
						var email = new NeEMail
						{
							To = new NeMember(Convert.ToInt32(qs.worksheet_review_mid)).NEEmail,
							Subject = "Quote " + qs.quoteid + " is waiting for your review of the worksheet"
						};
						email.Send();
					}
					break;
				default:
					break;
			}
			return new DataExtra()
			{
				Data = "Saved successfully.",
				Extra = ret
			};
		}

		private string updateCheckValue(NameFieldValue model)
		{
			var value = Convert.ToBoolean(model.value);
			var now = DateTime.Now;
			var now_txt = base.getStageText(now);
			var done = false;
			switch (model.name)
			{
				case "stage4":
					switch (model.index)
					{
						case 0:
							qs.rt1_s4_approved = value ? qs.rt1_s4_approved = now : null;
							break;
						case 1:
							qs.rt2_s4_approved = value ? qs.rt2_s4_approved = now : null;
							break;
						case 2:
							qs.rt3_s4_approved = value ? qs.rt3_s4_approved = now : null;
							break;
						case 3:
							qs.rt4_s4_approved = value ? qs.rt4_s4_approved = now : null;
							break;
					}
					qs.save();
					if ((qs.rt1_s4_approved != null) && (qs.rt2_s4_approved != null) && (qs.rt3_s4_approved != null) &&
						(qs.rt4_s4_approved != null))
					{
						bllToolbox.doSQL_void(@"update quote_master set allowed_to_quote=1,status_id = 1,allowed_to_quote_2=1
									where quote_id = @v0 AND active_revision = 1", quote_id);
						var email = new NeEMail();
						var estimator = new NeMember(Convert.ToInt32(qs.project_estimated_mid));
						var delivery_strategist = new NeMember(Convert.ToInt32(qs.quote_delivery_strategy_mid));
						email.To = estimator.NEEmail;
						email.CC = delivery_strategist.NEEmail;
						email.Subject =
							$"Quote {qs.quoteid} for {quote.txtCustomerName} has just been approved for estimating & sell strategy development";
						email.Body =
							$"{estimator.FullName}: You have been allotted {qs.allowedquotetime} hr(s) to estimate this project.<br/>{delivery_strategist.FullName}: Please develop the sell strategy for this quote.";
						email.From = "nomail@" + Toolbox.app_setting("DomainForEmail");
						email.Send();
						sendNotifications("Quote " + qs.quoteid + " has been approved for quoting, please find your involvement in this email");
						done = true;
					}
					break;
				case "stage6":
					switch (model.index)
					{
						case 0:
							if (value)
							{
								qs.rt1_f_approved = now;
							}
							else
							{
								qs.rt1_f_approved = null;
							}
							break;
						case 1:
							if (value)
							{
								qs.rt2_f_approved = now;
							}
							else
							{
								qs.rt2_f_approved = null;
							}
							break;
						case 2:
							if (value)
							{
								qs.rt3_f_approved = now;
							}
							else
							{
								qs.rt3_f_approved = null;
							}
							break;
						case 3:
							if (value)
							{
								qs.rt4_f_approved = now;
							}
							else
							{
								qs.rt4_f_approved = null;
							}
							break;
					}
					qs.save();
					if (
						((quote_level == "2") && ((qs.rt1_f_approved != null) || (qs.rt2_f_approved != null) ||
												  (qs.rt3_f_approved != null) || (qs.rt4_f_approved != null))) ||
						((qs.rt1_f_approved != null) && (qs.rt2_f_approved != null) && (qs.rt3_f_approved != null) &&
						 (qs.rt4_f_approved != null))

					)
					{
						bllToolbox.doSQL_void(
							@"update quote_master set prev_status_id = status_id,allowed_to_quote=1,allowed_to_quote_2=1,status_id = 4
						where quote_id =@v0  AND active_revision = 1", quote_id);
						var email = new NeEMail
						{
							To = new NeMember(Convert.ToInt32(qs.quote_delivered_mid)).NEEmail,
							Subject = "Quote " + qs.quoteid + " for " + quote.txtCustomerName +
									  " has just been approved for final delivery",
							From = "nomail@" + Toolbox.app_setting("DomainForEmail"),
							Body = "When you have delivered the quote, please click the 'DELIVERED' button on the quote strategy page"
						};
						email.Send();
						sendNotifications("Quote " + qs.quoteid + " has been approved for delivery.  It is now of status 'Waiting for Customer Approval'");
						done = true;
					}  
					break;
				default:
					break;
			}

			return done ? "A|" + now_txt : model.index.ToString() + "|" + now_txt;
		}

		public void updateDate(NameFieldValue model)
		{
			var date = Convert.ToDateTime(model.value);
			switch (model.name)
			{
				case "Open Quote":
					qs.opendate = date;
					break;
				case "screening_completed":
					qs.stage1screening_date = date;
					break;
				case "schedule_produced":
					qs.schedule_produced_date = date;
					break;
				case "manpower":
					qs.manpower_information_collected_date = date;
					break;
				case "customerinfo":
					qs.customer_info_collected_date = date;
					break;
				case "marketinfo":
					qs.market_info_collected_date = date;
					break;
				case "financeinfo":
					qs.finance_info_collected_date = date;
					break;
				case "reconinfo":
					qs.recon_report_created_date = date;
					break;
				case "stage4":
					qs.stage4_gono_date = date;
					break;
				case "sell_strategy":
					qs.quote_delivery_strategy_date = date;
					break;
				case "project_estimated":
					qs.project_estimated_date = date;
					break;
				case "worksheet_review":
					qs.worksheet_review_date = date;
					break;
				case "stage6":
					qs.stage6_final_review_date = date;
					break;
				case "quote_delivered":
					qs.quote_delivered_date = date;
					break;
				case "follow_up_1":
					qs.followup1_date = date;
					break;
				case "follow_up_2":
					qs.followup2_date = date;
					break;
				case "convert_kill":
					qs.convert_or_kill_date = date;
					break;
				default:
					break;

			}
			qs.save();
		}
		public void updateAssignTo(NameFieldValue model)
		{
			var mid = Convert.ToInt32(model.value);
			switch (model.name)
			{
				case "Open Quote":
					qs.openmid = mid;
					break;
				case "screening_completed":
					qs.stage1screening_mid = mid;
					break;
				case "schedule_produced":
					qs.schedule_produced_mid = mid;
					break;
				case "manpower":
					qs.manpower_information_collected_mid = mid;
					break;
				case "customerinfo":
					qs.customer_info_collected_mid = mid;
					break;
				case "marketinfo":
					qs.market_info_collected_mid = mid;
					break;
				case "financeinfo":
					qs.finance_info_collected_mid = mid;
					break;
				case "reconinfo":
					qs.recon_report_created_mid = mid;
					break;
				case "stage4":

					break;
				case "sell_strategy":
					qs.quote_delivery_strategy_mid = mid;
					break;
				case "project_estimated":
					qs.project_estimated_mid = mid;
					break;
				case "worksheet_review":
					qs.worksheet_review_mid = mid;
					break;
				case "stage6":

					break;
				case "quote_delivered":
					qs.quote_delivered_mid = mid;
					break;
				case "follow_up_1":
					qs.followup1_mid = mid;
					break;
				case "follow_up_2":
					qs.followup2_mid = mid;
					break;
				case "convert_kill":
					qs.convert_or_kill_mid = mid;
					break;
				default:
					break;

			}
			qs.save();
		}
		public void updateNotes(NameFieldValue model)
		{
			var note = model.value;
			switch (model.name)
			{
				case "Open Quote":
					qs.opennotes = note;
					break;
				case "screening_completed":
					qs.stage1screening_notes = note;
					break;
				case "schedule_produced":
					qs.schedule_produced_notes = note;
					break;
				case "manpower":
					qs.manpower_information_collected_notes = note;
					break;
				case "customerinfo":
					qs.customer_info_collected_notes = note;
					break;
				case "marketinfo":
					qs.market_info_collected_notes = note;
					break;
				case "financeinfo":
					qs.finance_info_collected_notes = note;
					break;
				case "reconinfo":
					qs.recon_report_created_notes = note;
					break;
				case "stage4":
					qs.stage4_gono_notes = note;
					break;
				case "sell_strategy":
					qs.quote_delivery_strategy_notes = note;
					break;
				case "project_estimated":
					qs.project_estimated_notes = note;
					break;
				case "worksheet_review":
					qs.worksheet_review_notes = note;
					break;
				case "stage6":
					qs.stage6_final_review_notes = note;
					break;
				case "quote_delivered":
					qs.quote_delivered_notes = note;
					break;
				case "follow_up_1":
					qs.followup1_notes = note;
					break;
				case "follow_up_2":
					qs.followup2_notes = note;
					break;
				case "convert_kill":
					qs.convert_or_kill_notes = note;
					break;
				default:
					break;

			}
			qs.save();
		}
		public string buttonClick(NameFieldValue model)
		{
			var label = model.value.ToLower();
			var note = model.value;

			switch (model.name)
			{
				case "Open Quote":
					break;
				case "screening_completed":
					break;
				case "schedule_produced":
					qs.schedule_produced_date_completed = DateTime.Now;
					qs.save();
					sendNotifications("Quote " + qs.quoteid + " Schedule.  You are Involved");
					break;
				case "manpower":
					break;
				case "customerinfo":
					break;
				case "marketinfo":
					break;
				case "financeinfo":
					break;
				case "reconinfo":
					qs.recon_report_created_date_completed = DateTime.Now;
					qs.save();
					sendNotifications(current_user.FullName + " has the recon report ready for final review for Quote: " + qs.quoteid);
					break;
				case "stage4":
					break;
				case "sell_strategy":
					//qs.sell_strategy = note;
					//qs.quote_delivery_strategy_date_completed = DateTime.Now;
					break;
				case "project_estimated":
				//					qs.project_estimated_date_completed = DateTime.Now;
				//					var email = new NeEMail();
				//					email.To = new NeMember(Convert.ToInt32(qs.worksheet_review_mid)).NEEmail;
				//					email.Subject = "Quote " + qs.quoteid + " is waiting for your review of the worksheet";
				//					email.Send();
				//					break;
				case "worksheet_review":
					qs.worksheet_review_date_completed = DateTime.Now;
					bllToolbox.doSQL_void(@"UPDATE quote_master SET allowed_to_quote=1, prev_status_id=status_id, status_id= 12,allowed_to_quote_2=1
							WHERE quote_id =@v0 AND active_revision = 1", quote_id);
					qs.save();
					sendNotifications(current_user.FullName + " completed the WorkSheet Review for Quote " + qs.quoteid);
					break;
				case "stage6":
					var revision_n = bllToolbox.doSQL_string(@"SELECT IFNULL(MAX(revision), 1) FROM quote_master 
WHERE quote_id = @v0 AND active_revision = true", quote_id);
					if ((quote_level == "3") && (qs.rt1_f_approved != null) && (qs.rt2_f_approved != null) && (qs.rt3_f_approved != null) && (qs.rt4_f_approved != null))
					{
						// ScriptManager.RegisterStartupScript(this, typeof(string), "key1", "javascript:boing('/sections/reports/print_quote/index.aspx?quoteid=" +quote_id + revision_n + "&from_process=1&type=w','Print Quote', 1035, 800);", true);
						bllToolbox.doSQL_void(@"Update quote_master set status_id=2, last_print_date = curdate() 
where quote_id = @v0  and active_revision = 1", quote_id);
					}
					else if ((quote_level == "2") && ((qs.rt1_f_approved != null) || (qs.rt2_f_approved != null) || (qs.rt3_f_approved != null) || (qs.rt4_f_approved != null)))
					{
						// ScriptManager.RegisterStartupScript(this, typeof(string), "key1", "javascript:boing('/sections/reports/print_quote/index.aspx?quoteid=" +quote_id + revision_n + "&from_process=1&type=w','Print Quote', 1035, 800);", true);
						bllToolbox.doSQL_void(@"Update quote_master set status_id=2, last_print_date = curdate() 
where quote_id =@v0  and active_revision = 1", quote_id);
					}
					return revision_n;
				// break;
				case "quote_delivered":
					qs.quote_delivered_date_completed = DateTime.Now;
					bllToolbox.doSQL_void(@"Update quote_master set status_id=4, last_fax_date = curdate(), verified_date = curdate() 
							where quote_id =@v0  and active_revision = 1", quote_id);
					qs.save();
					sendNotifications(current_user.FullName + " Delivered the Final Print Off for Quote " + qs.quoteid);
					break;
				case "follow_up_1":
					qs.followup1_date = DateTime.Now;
					qs.save();
					break;
				case "follow_up_2":
					qs.followup2_date = DateTime.Now;
					qs.save();
					break;
				case "convert_kill":
					qs.convert_or_kill_date_completed = DateTime.Now;
					qs.save();
					sendNotifications(current_user.FullName + " Converted the Quote: " + qs.quoteid + ".  In other words, we won the job!");
					break;
				default:
					break;

			}
			return "";

		}

		public QuoteStrategyQuestion[] GetQuoteProcessQuestionHistory(string type)
		{
			return bllToolbox.doSQL_Array<QuoteStrategyQuestion>(@"Select
			  h.id,
			  q.`question`,
			  ifnull(h.checked,0) is_checked,
			  h.notes,
			  @v1 `type`,
			  q.ifyes,
			  q.ifno
			from
			  quote_process_question_history h
			  inner join quote_process_questions q
				on h.question_id = q.id
			where h.quoteid = @v0
			  and q.type = @v1", quote_id, type);
		}

		public string ClearQuoteQuestionHistory(string type)
		{
			bllToolbox.doSQL_void(@"update quote_process_question_history s INNER JOIN quote_process_questions n ON s.question_id = n.id 
						set s.notes='', s.checked=0
						where n.type = @p1 and s.quoteid=@p0", quote_id, type);

			switch (type.ToLower())
			{
				case "manpower":
					qs.manpower_information_collected_date_completed = null;
					break;
				case "customer":
					qs.customer_info_collected_date_completed = null;
					break;
				case "market":
					qs.market_info_collected_date_completed = null;
					break;
				case "finance":
					qs.finance_info_collected_date_completed = null;
					break;
				default:
					break;
			}
			qs.save();

			return "Saved successfully.";
		}

		public DataExtra ProcessQuestionHistoryUpdate(NameFieldValueId model)
		{
			switch (model.field)
			{
				case "is_checked":
					bllToolbox.doSQL_void(@"update quote_process_question_history set checked =@v0  where id =@v1 ", Convert.ToBoolean(model.value) ? 1 : 0, model.Id);
					break;
				case "notes":
					bllToolbox.doSQL_void(@"update quote_process_question_history set notes =@v0  where id =@v1 ", model.value, model.Id);
					break;
				default:
					break;
			}

			return new DataExtra()
			{
				Data = "Success.",
				Extra = get_score(model.name)
			};

		}

		protected double get_score(string type)
		{
			return (bllToolbox.doSQL_double(@"SELECT
  ROUND(
    IF (
      (
        SUM(quote_process_questions.ifyes) <> 0
      ),
      SUM(
        IF (
          a.checked = 1,
          quote_process_questions.ifyes,
          quote_process_questions.ifno
        )
      ) / SUM(quote_process_questions.ifyes),
      0
    ) * 100,
    1
  ) score
FROM
  quote_process_question_history AS a
  INNER JOIN quote_process_questions
    ON a.question_id = quote_process_questions.id
WHERE a.quoteid =@v0 and quote_process_questions.TYPE=@v1
", quote_id, type));
		}

		public string SaveQuoteProcessQuestionHistory(string type, QuoteStrategyQuestion[] models)
		{
			foreach (var model in models)
			{
				bllToolbox.doSQL_void(@"update quote_process_question_history set checked =@v0, notes=@v1  where id =@v2 ", Convert.ToBoolean(model.is_checked) ? 1 : 0, model.notes, model.id);

			}

			var x = qs.member_list();
			foreach (var mid in x)
			{
				try
				{
					if (mid != 0)
					{
						email_notification(mid, current_user.FullName + " completed the " + type + " Recon for Quote " + qs.quoteid);
					}
				}
				catch { }

			}
			switch (type.ToLower())
			{
				case "manpower":
					qs.manpower_information_collected_date_completed = DateTime.Now;
					break;
				case "customer":
					qs.customer_info_collected_date_completed = DateTime.Now;
					break;
				case "market":
					qs.market_info_collected_date_completed = DateTime.Now;
					break;
				case "finance":
					qs.finance_info_collected_date_completed = DateTime.Now;
					break;
				default:
					break;

			}
			qs.save();

			return "Saved successfully.";
		}


		public string KillQuote(string mem)
		{
			var comp = new NeBusinessUnit(quote.business_unit_id);
			var email = new NeEMail
			{
				Subject = "Quote " + quote.QuoteID + " for " + quote.txtCustomerName + " Has Been Closed ",
				To = new NeMember(Convert.ToInt32(quote.quoted_by)).NEEmail + ";",
				CC = new NeMember(Convert.ToInt32(current_user.reports_to)).NEEmail,
				isHTML = true,
				Body = mem + "</br> The quote is now waiting for a POST mortem </br>"
			};



			bllToolbox.doSQL_void(@"update quote_master set allowed_to_quote=0,status_id = 6,allowed_to_quote_2=0, killed_at_stage='0',
killed_by=@v0,killed_date=curdate(),why_killed=@v1 where quote_id = @v2 and active_revision = 1", current_user.id, mem, quote_id);
			#region set up post mortem questions
			var dt = bllToolbox.doSQL_dt(@"Select * from quote_post_mortem_questions  where status='Active' and stage ='Stage 1'");
			foreach (DataRow dr in dt.Rows)
			{
				if (bllToolbox.doSQL_int(@"Select count(quote_post_mortem.id) from quote_post_mortem  where quote_id =@v0 and question_id=@v1 ", quote.QuoteID, dr["id"]) == 0)
				{
					bllToolbox.doSQL_void(@"Insert into quote_post_mortem (question_id,date,memberid,quote_id) 
values(@v0,curdate(),@v1,@v2)", dr["id"], current_user.id, quote.QuoteID);
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
			return "Quote has been killed successfully.";
		}
	}
}