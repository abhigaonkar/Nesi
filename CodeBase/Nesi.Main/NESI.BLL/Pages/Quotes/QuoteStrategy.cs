using System;
using System.Collections.Generic;
using System.Linq;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Quotes;

namespace NESI.BLL.Pages.Quotes
{
	public class QuoteStrategy : QuoteStrategBase
	{
		public string[] reports_to_list { get; set; }
		public string Manpower_score { get; set; }
		public string Market_score { get; set; }
		public string Customer_score { get; set; }
		public string Finance_score { get; set; }
		public string html_estimating { get; set; }
		public string html_sell { get; set; }
		public string lbl_lvl_text { get; set; }


		public QuoteStrategy(Employee user, int quoteId) : base(user, quoteId)
		{
			reports_to_list = Toolbox.doSQL_string(@"SELECT IFNULL(REPORTS_TO(@v0), '')", current_user.id).Split(',');
			if (quote_level == "3")
			{
				lbl_lvl_text = "This is a Level 3 Quote, so ALL steps are required";
				fill_schedule();
				//	populate_recon_scores();
			}
			else if (quote_level == "2")
			{
				lbl_lvl_text = "This is a Level 2 Quote, so some steps are disabled";
				fill_schedule();
			}
		}

		private LabelValueInt[] checkNameInList(List<LabelValueInt> list, int mid)
		{
			if (list.FirstOrDefault(x => x.Value == mid) == null)
			{
				var sub_m = new NeMember(mid);
				var item_text = sub_m.business_unit.ddl_name + " - " + sub_m.FullName;
				list.Add(new LabelValueInt()
				{
					Label = item_text,
					Value = mid
				});
			}
			return list.ToArray();
		}


	

	

		protected void fill_schedule()
		{
			if (qs.id != 0)
			{
				var selectcommand =
					@"Select a.member_id value,CONCAT(b.ddl_name, ' - ', a.member_fullname) label 
						from member a LEFT join business_unit b on a.business_unit_id = b.id 
						inner join memberpage p on a.member_id= p.memberpage_member_id 
						where a.member_status = 'Active' 
						and p.memberpage_page_id=65
						and p.active=1
						and a.Member_MemberType_ID in (2,4,5,8,9,11,12,17,21,24,25,27,29,33,34,35,37,38,39,41,46,49,52,54,55,56,59,60)
						and find_in_set(@p0,get_visible_business_units_group_concat(a.member_id))
						UNION select 0 memberid,'Not Assigned' member_fullname order by label";
				var all_members = bllToolbox.doSQL_List<LabelValueInt>(selectcommand, buId);


				selectcommand = @"Select a.member_id value,CONCAT(b.ddl_name, ' - ', a.member_fullname) label 
						from member a LEFT join business_unit b on a.business_unit_id = b.id 
						inner join memberpage p on a.member_id= p.memberpage_member_id 
						where a.member_status = 'Active' 
						and p.memberpage_page_id=65
						and p.active=1
						and a.Member_MemberType_ID in (4,5,11,12,17,23,24,25,29,33,34,39,38,49,53,52,55)
						and find_in_set(@p0,get_visible_business_units_group_concat(a.member_id))
						and a.business_unit_id = @p0
						UNION select 0 memberid,'Not Assigned' member_fullname order by label";

				var fields_members = bllToolbox.doSQL_List<LabelValueInt>(selectcommand, buId);

				selectcommand = @"Select a.member_id value,CONCAT(b.ddl_name, ' - ', a.member_fullname) label
						from member a LEFT join business_unit b on a.business_unit_id = b.id 
						inner join memberpage p on a.member_id= p.memberpage_member_id 
						where a.member_status = 'Active' 
						and p.memberpage_page_id=65
						and p.active=1
						and a.Member_MemberType_ID in (8,5,4,27,37,38,41,49,52,54,55,56,59,60) 
						and find_in_set(@p0,get_visible_business_units_group_concat(a.member_id))
						UNION select 0 memberid,'Not Assigned' member_fullname order by label";
				var all_sales_members = bllToolbox.doSQL_List<LabelValueInt>(selectcommand, buId);

				selectcommand = @"Select a.member_id value,CONCAT(b.ddl_name, ' - ', a.member_fullname) label
						from member a LEFT join business_unit b on a.business_unit_id = b.id 
						inner join memberpage p on a.member_id= p.memberpage_member_id 
						where a.member_status = 'Active' 
						and p.memberpage_page_id=65
						and p.active=1
						and a.Member_MemberType_ID in (5,11,39,29,52,36) 
						and find_in_set(@p0,get_visible_business_units_group_concat(a.member_id))
						UNION select 0 memberid,'Not Assigned' member_fullname order by label";
				var branchManagers = bllToolbox.doSQL_List<LabelValueInt>(selectcommand, buId);

				selectcommand = @"SELECT
				a.member_id value,
					CONCAT(b.ddl_name, ' - ', a.member_fullname) label,
				a.member_membertype_id
					from member a LEFT join business_unit b on a.business_unit_id = b.id
						inner join memberpage p on a.member_id= p.memberpage_member_id 
						where a.member_status = 'Active' 
						and p.memberpage_page_id=65
						and p.active=1
				AND a.Member_MemberType_ID IN(58, 56, 51, 50, 47, 45, 36, 30,44,48,7,67)
				and find_in_set(@p0,get_visible_business_units_group_concat(a.member_id))
				UNION
					SELECT
				0 memberid,
				'Not Assigned' member_fullname,
				0 m
					ORDER BY
					label";
				var finance_members = bllToolbox.doSQL_List<LabelValueInt>(selectcommand, buId);


				openquote = new QuoteStrategyCheckItem
				{
					name = "open_quote",
					milestone = "Open Quote",
					date = qs.opendate.ToString("s"),
					options = checkNameInList(all_members, qs.openmid),
					assign_to = qs.openmid,
					notes = qs.opennotes,
					is_checked = true,
					date_enabled = false
				};

				screeningcompleted = new QuoteStrategyCheckItem
				{
					name = "screening_completed",
					milestone = "Stage 1 Screening Completed",
					date = qs.stage1screening_date.ToString("s"),
					options = checkNameInList(all_members, qs.stage1screening_mid),
					assign_to = qs.stage1screening_mid,
					notes = qs.stage1screening_notes,
					is_checked = true,
					date_enabled = false
				};


				scheduleproduced = new QuoteStrategyCheckItem
				{
					name = "schedule_produced",
					milestone = "Schedule Produced and Distributed (This is the Point Person)",
					date = qs.schedule_produced_date.ToString("s"),
					options = checkNameInList(all_members, qs.schedule_produced_mid),
					assign_to = qs.schedule_produced_mid,
					notes = qs.schedule_produced_notes,
					is_checked = qs.schedule_produced_date_completed != null,
					button_label = "Email",
					button_icon = "fa-envelope",
					completed_type = "button"
				};

				manpower = new QuoteStrategyCheckItem
				{
					name = "manpower",
					milestone = "Manpower Information Collected",
					date = qs.manpower_information_collected_date.ToString("s"),
					options = checkNameInList(fields_members, qs.manpower_information_collected_mid),
					assign_to = qs.manpower_information_collected_mid,
					notes = qs.manpower_information_collected_notes,
					is_checked = qs.manpower_information_collected_date_completed != null,
					button_label = "Manpower Recon",
					button_icon = qs.manpower_information_collected_date_completed != null ? "fa-eye" : "fa-pencil",
					completed_type = "dialog",
					dialog_type = "Manpower"
				};


				customerinfo = new QuoteStrategyCheckItem
				{
					name = "customerinfo",
					milestone = "Customer Information Collected",
					date = qs.customer_info_collected_date.ToString("s"),
					options = checkNameInList(all_sales_members, qs.customer_info_collected_mid),
					assign_to = qs.customer_info_collected_mid,
					notes = qs.customer_info_collected_notes,
					is_checked = qs.customer_info_collected_date_completed != null,
					button_label = "Customer Recon",
					button_icon = qs.customer_info_collected_date_completed != null ? "fa-eye" : "fa-pencil",
					completed_type = "dialog",
					dialog_type = "Customer"
				};


				marketinfo = new QuoteStrategyCheckItem
				{
					name = "marketinfo",
					milestone = "Market Information Collected",
					date = qs.market_info_collected_date.ToString("s"),
					options = checkNameInList(all_sales_members, qs.market_info_collected_mid),
					assign_to = qs.market_info_collected_mid,
					notes = qs.market_info_collected_notes,
					is_checked = qs.market_info_collected_date_completed != null,
					button_label = "Market Recon",
					button_icon = qs.market_info_collected_date_completed != null ? "fa-eye" : "fa-pencil",
					completed_type = "dialog",
					dialog_type = "Market"
				};

				financeinfo = new QuoteStrategyCheckItem
				{
					name = "financeinfo",
					milestone = "Finance Information Collected",
					date = qs.finance_info_collected_date.ToString("s"),
					options = checkNameInList(finance_members, qs.finance_info_collected_mid),
					assign_to = qs.finance_info_collected_mid,
					notes = qs.finance_info_collected_notes,
					is_checked = qs.finance_info_collected_date_completed != null,
					button_label = "Finance Recon",
					button_icon = qs.finance_info_collected_date_completed != null ? "fa-eye" : "fa-pencil",
					completed_type = "dialog",
					dialog_type = "Finance"
				};


				reconinfo = new QuoteStrategyCheckItem
				{
					name = "reconinfo",
					milestone = "Recon Information Inspected",
					date = qs.recon_report_created_date.ToString("s"),
					options = checkNameInList(finance_members, qs.recon_report_created_mid),
					assign_to = qs.recon_report_created_mid,
					notes = qs.recon_report_created_notes,
					is_checked = qs.recon_report_created_date_completed != null,
					button_label = "Verified",
					button_icon = "fa-check-circle",
					completed_type = "button",
				};



				stage4 = new QuoteStrategyStageMeetingItem();
				stage6 = new QuoteStrategyStageMeetingItem();

				if (quote.status == 11 && qs.recon_report_created_date_completed != null)
				{
					stage4.milestone = "Stage 4 Go-No Go Review Meeting";
					stage4.button_enabled = true;
				}

				stage4.date = qs.stage4_gono_date.ToString("s");
				stage4.notes = qs.stage4_gono_notes;
				if (qs.rt1_s4_approved == null || qs.rt2_s4_approved == null || qs.rt3_s4_approved == null || qs.rt4_s4_approved == null)
				{
					stage4.is_checked = false;
				}
				else
				{
					stage4.is_checked = true;
				}
				sellstrategy = new QuoteStrategyCheckItem
				{
					name = "sell_strategy",
					milestone = "Sell Strategy Developed",
					date = qs.quote_delivery_strategy_date.ToString("s"),
					options = checkNameInList(all_sales_members, qs.quote_delivery_strategy_mid),
					assign_to = qs.quote_delivery_strategy_mid,
					notes = qs.quote_delivery_strategy_notes,
					is_checked = qs.quote_delivery_strategy_date_completed != null,
					button_label = "Sell Strategy",
					button_icon = qs.quote_delivery_strategy_date_completed != null ? "fa-eye" : "fa-pencil",
					completed_type = "review",
					html = qs.sell_strategy
				};

				projectestimated = new QuoteStrategyCheckItem
				{
					name = "project_estimated",
					milestone = "Project Estimated",
					date = qs.project_estimated_date.ToString("s"),
					options = checkNameInList(fields_members, qs.project_estimated_mid),
					assign_to = qs.project_estimated_mid,
					notes = qs.project_estimated_notes,
					is_checked = qs.project_estimated_date_completed != null,
					button_label = "Estimated Concerns",
					button_icon = qs.project_estimated_date_completed != null ? "fa-eye" : "fa-pencil",
					completed_type = "review",
					html = qs.estimating_concerns
				};

				worksheetreview = new QuoteStrategyCheckItem
				{
					name = "worksheet_review",
					milestone = "Estimate Worksheet Review",
					date = qs.worksheet_review_date.ToString("s"),
					options = checkNameInList(all_sales_members, qs.worksheet_review_mid),
					assign_to = qs.worksheet_review_mid,
					notes = qs.worksheet_review_notes,
					is_checked = qs.worksheet_review_date_completed != null,
					button_label = "Done",
					button_icon = "fa-check-circle",
					completed_type = "button"
				};


				stage6.date = qs.stage6_final_review_date.ToString("s");
				stage6.notes = qs.stage6_final_review_notes;


				if (quote.status == 12 && qs.worksheet_review_date_completed != null)
				{
					stage6.button_enabled = true;
					stage6.milestone = "Stage 6 Final Review Meeting";
				}
				if (quote_level == "3" && (qs.rt1_f_approved == null || qs.rt2_f_approved == null || qs.rt3_f_approved == null || qs.rt4_f_approved == null))
				{
					stage6.is_checked = false;
				}
				else if (quote_level == "2" && qs.rt1_f_approved == null && qs.rt2_f_approved == null && qs.rt3_f_approved == null && qs.rt4_f_approved == null)
				{
					stage6.is_checked = false;
				}
				else
				{
					stage6.is_checked = true;
				}
				stage6.button_enabled = true;
				stage6.button_icon = "fa-print";
				stage6.button_label = "Print Quote";
				stage6.completed_type = "print";

				quotedelivered = new QuoteStrategyCheckItem
				{
					name = "quote_delivered",
					background_color = "red",
					milestone = "Quote Delivered (Due Date)",
					date = qs.quote_delivered_date.ToString("s"),
					options = checkNameInList(all_members, qs.quote_delivered_mid),
					assign_to = qs.quote_delivered_mid,
					notes = qs.quote_delivered_notes,
					is_checked = qs.quote_delivered_date_completed != null,
					button_label = "Deliveried",
					button_icon = "fa-truck",
					completed_type = "button"
				};

				followup1 = new QuoteStrategyCheckItem
				{
					name = "follow_up_1",
					milestone = "Follow up 1",
					date = qs.followup1_date.ToString("s"),
					options = checkNameInList(all_members, qs.followup1_mid),
					assign_to = qs.followup1_mid,
					notes = qs.followup1_notes,
					//					is_checked = qs.followup1_date != null,
					//					button_label = "Done",
					//					button_icon = "fa-check-circle",
					//					completed_type = "button",
					//					button_enabled = qs.followup1_mid == current_user.id,
				};


				followup2 = new QuoteStrategyCheckItem
				{
					name = "follow_up_2",
					milestone = "Follow up 2",
					date = qs.followup2_date.ToString("s"),
					options = checkNameInList(all_members, qs.followup2_mid),
					assign_to = qs.followup2_mid,
					notes = qs.followup2_notes,
					//					is_checked = false,
					//					button_label = "Done",
					//					button_icon = "fa-check-circle",
					//					completed_type = "button",
					//					button_enabled = qs.followup2_mid == current_user.id,
				};

				convertkill = new QuoteStrategyCheckItem
				{
					name = "convert_kill",
					milestone = "Convert or Kill",
					date = qs.convert_or_kill_date.ToString("s"),
					options = checkNameInList(all_members, qs.convert_or_kill_mid),
					assign_to = qs.convert_or_kill_mid,
					notes = qs.convert_or_kill_notes,
					is_checked = qs.convert_or_kill_date_completed != null,
					button_label = "Convert",
					button_icon = "fa-file-alt",
					completed_type = "two_buttons"
				};
				postmortem = new QuoteStrategyCheckItem
				{
					name = "postmortem",
					milestone = "Post Mortem Completed",
					date = qs.post_mortem_complete_date.ToString("s"),
					options = checkNameInList(all_members, qs.post_mortem_complete_mid),
					assign_to = qs.post_mortem_complete_mid,
					notes = qs.post_mortem_complete_notes,
					is_checked = qs.post_mortem_complete_date_completed != null
				};

				var hours = bllToolbox.doSQL_double(
					@"select ifnull((Select sum(numberofhours) from membertime  where wotype = 'Quote' and left(membertime_workorder_id,6) =@v0),0) ", quote_id);
				projectestimated.milestone = hours + " of " + qs.allowedquotetime + " hours spent so far on ESTIMATING";

				stage4.check_list = new[]
				{
					new LabelValueInt()
					{
						Label = new NeMember(qs.rt1).Nickname,
						Value=qs.rt1
					},
					new LabelValueInt()
					{
						Label = new NeMember(qs.rt2).Nickname,
						Value=qs.rt2
					},
					new LabelValueInt()
					{
						Label = new NeMember(qs.rt3).Nickname,
						Value=qs.rt3
					},
					new LabelValueInt()
					{
						Label = new NeMember(qs.rt4).Nickname,
						Value=qs.rt4
					}
				};
				stage6.check_list = stage4.check_list;

				stage4.check_value = new[]
				{
					qs.rt1_s4_approved != null,
					qs.rt2_s4_approved != null,
					qs.rt3_s4_approved != null,
					qs.rt4_s4_approved != null,

				};
				stage4.check_visible = new[]
				{
					true, true, true, true
				};

				stage4.check_enabled = new[]
				{
					true, true, true, true
				};

				stage4.check_text = new[]
				{
					getStageText(qs.rt1_s4_approved),
					getStageText(qs.rt2_s4_approved),
					getStageText(qs.rt3_s4_approved),
					getStageText(qs.rt4_s4_approved)
				};

				stage6.check_value = new[]
				{
					qs.rt1_f_approved != null,
					qs.rt2_f_approved != null,
					qs.rt3_f_approved != null,
					qs.rt4_f_approved != null,

				};

				stage6.check_text = new[]
				{
					getStageText(qs.rt1_f_approved),
					getStageText(qs.rt2_f_approved),
					getStageText(qs.rt3_f_approved),
					getStageText(qs.rt4_f_approved)
				};

				stage6.check_visible = new[]
				{
					quote_level != "2" || qs.rt1 == Branch_manager_id,
					quote_level != "2" || qs.rt2 == Branch_manager_id,
					quote_level != "2" || qs.rt3 == Branch_manager_id,
					quote_level != "2" || qs.rt4 == Branch_manager_id
				};
				stage6.check_enabled = new[]
				{
					true, true, true, true
				};


				html_estimating = qs.estimating_concerns;
				html_sell = qs.sell_strategy;
				//#endregion


				#region permissions

				tabenable_1_4 = quote_level == "3" && qs.schedule_produced_date_completed != null;
				tabenable_5_7 = qs.schedule_produced_date_completed != null;


				is_quotedbys_bm = new NeMember(quote.quoted_by).business_unit.branch_manager.id == current_user.id;

				if (qs.id != 0 && quote.allowed_to_quote)
				{
					openquote.date = quote.opendate.ToString("s");
					openquote.options = checkNameInList(all_members, quote.quoted_by);
					openquote.assign_to = quote.quoted_by;


					if (qs.recon_report_created_date_completed != null || quote_level == "2")
					{
						if ((qs.rt1_s4_approved == null || qs.rt2_s4_approved == null || qs.rt3_s4_approved == null || qs.rt4_s4_approved == null) && quote.status == 11 && qs.recon_report_created_date_completed != null)
						{
							stage4.check_enabled = new[]
							{
								current_user.id == qs.rt1 || reports_to_list.FirstOrDefault(x => x == qs.rt1.ToString()) != null,
								current_user.id == qs.rt2 || reports_to_list.FirstOrDefault(x => x == qs.rt2.ToString()) != null,
								current_user.id == qs.rt3 || reports_to_list.FirstOrDefault(x => x == qs.rt3.ToString()) != null,
								current_user.id == qs.rt4 || reports_to_list.FirstOrDefault(x => x == qs.rt4.ToString()) != null,
							};
						}
					}

					if (qs.worksheet_review_date_completed != null)
					{
						if (qs.rt1_f_approved == null || qs.rt2_f_approved == null || qs.rt3_f_approved == null ||
							qs.rt4_f_approved == null)
						{
							stage6.check_enabled = new[]
							{
								current_user.id == qs.rt1 || reports_to_list.FirstOrDefault(x => x == qs.rt1.ToString()) != null,
								current_user.id == qs.rt2 || reports_to_list.FirstOrDefault(x => x == qs.rt2.ToString()) != null,
								current_user.id == qs.rt3 || reports_to_list.FirstOrDefault(x => x == qs.rt3.ToString()) != null,
								current_user.id == qs.rt4 || reports_to_list.FirstOrDefault(x => x == qs.rt4.ToString()) != null,
							};
						}

					}
					stage6.date_enabled = false;
					stage4.date_enabled = false;
					if (current_user.id == qs.schedule_produced_mid ||
						NeMember.is_supervisor(qs.schedule_produced_mid, current_user.id) ||
						qs.schedule_produced_date_completed != null)
					{
						scheduleproduced.button_enabled = true;

						if (current_user.id == qs.schedule_produced_mid || NeMember.is_supervisor(qs.schedule_produced_mid, current_user.id))
						{
							stage6.date_enabled = true;
							stage4.date_enabled = true;
							manpower.assign_to_enabled = true;
							manpower.date_enabled = true;
							customerinfo.assign_to_enabled = true;
							customerinfo.date_enabled = true;
							marketinfo.assign_to_enabled = true;
							marketinfo.date_enabled = true;
							financeinfo.assign_to_enabled = true;
							financeinfo.date_enabled = true;
							reconinfo.assign_to_enabled = true;
							reconinfo.date_enabled = true;
							sellstrategy.assign_to_enabled = true;
							sellstrategy.date_enabled = true;
							projectestimated.assign_to_enabled = true;
							projectestimated.date_enabled = true;
							worksheetreview.assign_to_enabled = true;
							worksheetreview.date_enabled = true;
							followup1.assign_to_enabled = true;
							followup1.date_enabled = true;
							followup2.assign_to_enabled = true;
							followup2.date_enabled = true;
							convertkill.assign_to_enabled = true;
							convertkill.date_enabled = true;
							postmortem.assign_to_enabled = true;
							postmortem.date_enabled = true;
						}
					}
					if (current_user.id == qs.recon_report_created_mid ||
						NeMember.is_supervisor(qs.recon_report_created_mid, current_user.id) &&
						qs.recon_report_created_date_completed == null)
					{
						if (qs.customer_info_collected_date_completed != null && qs.finance_info_collected_date_completed != null &&
							qs.market_info_collected_date_completed != null &&
							qs.manpower_information_collected_date_completed != null)
						{
							reconinfo.button_enabled = true;
						}
					}
					if (quote.allowed_to_quote_2 || quote_level == "2")
					{

						btn_print_final_quote_enabled = true;
						if (current_user.id == qs.quote_delivery_strategy_mid || NeMember.is_supervisor(qs.quote_delivery_strategy_mid, current_user.id))
						{
							btnsell_done_enabled = true;
						}

						if (current_user.id == qs.project_estimated_mid || NeMember.is_supervisor(qs.project_estimated_mid, current_user.id))
						{
							projectestimated.button_enabled = true;

						}

						if (current_user.id == qs.worksheet_review_mid || NeMember.is_supervisor(qs.worksheet_review_mid, current_user.id))
						{
							worksheetreview.button_enabled = true;
						}

						if (current_user.id == qs.quote_delivered_mid || NeMember.is_supervisor(qs.quote_delivered_mid, current_user.id))
						{
							if (quote_level == "2" && quote.PrintedDate != null || qs.rt1_f_approved != null && qs.rt2_f_approved != null && qs.rt3_f_approved != null && qs.rt4_f_approved != null && quote.PrintedDate != null)
							{
								quotedelivered.button_enabled = true;
							}
						}
					}
					if (current_user.id == qs.convert_or_kill_mid ||
						NeMember.is_supervisor(qs.convert_or_kill_mid, current_user.id) || is_quotedbys_bm)
					{
						kill_button_enabled = true;
						kill_button_visible = true;
						if (quote_level == "2" && quote.PrintedDate != null || qs.rt1_f_approved != null && qs.rt2_f_approved != null && qs.rt3_f_approved != null && qs.rt4_f_approved != null && quote.PrintedDate != null)
						{
							convert_button_enabled = true;
							convertkill.button_enabled = true;
						}
					}

					if (current_user.id == qs.customer_info_collected_mid)
					{
						customerinfo.button_enabled = true;
						if (qs.customer_info_collected_date_completed == null)
						{
							btn_custrecon_done_enabled = true;
						}
						else
						{
							clear_customer_enabled = true;
						}
					}

					if (current_user.id == qs.quote_delivery_strategy_mid)
					{
						sellstrategy.button_enabled = true;
					}

					if (current_user.id == qs.market_info_collected_mid)
					{
						marketinfo.button_enabled = true;
						if (qs.market_info_collected_date_completed == null)
						{
							btnmarket_done_enabeld = true;
						}
						else
						{
							clear_market_enabled = true;
						}
					}
					if (current_user.id == qs.finance_info_collected_mid)
					{
						financeinfo.button_enabled = true;
						if (qs.finance_info_collected_date_completed == null)
						{
							btnfinance_done_enabled = true;
						}
						else
						{
							clear_finance_enabled = true;
						}
					}
					if (current_user.id == qs.manpower_information_collected_mid)
					{
						manpower.button_enabled = true;
						if (qs.manpower_information_collected_date_completed == null)
						{
							btnmanpower_done_enabled = true;
						}
						else
						{
							clear_manpower_enabeld = true;
						}
					}


				}
				else if (is_quotedbys_bm || NeMember.is_supervisor(quote.quoted_by, current_user.id))
				{
					kill_button_enabled = true;
				}

				if (quote.status == 6 || quote.status == 8 || quote.status == 9)
				{
					kill_button_enabled = false;
					postmortem.assign_to_enabled = false;
					postmortem.date_enabled = false;
				}

				#endregion

				stage4.name = "stage4";
				stage6.name = "stage6";
				stage4.milestone = "Stage 4 Go-No Go Review Meeting";
				stage6.milestone = "Stage 6 Final Review Meeting";
				stage4.isStageMeeting = true;
				stage6.isStageMeeting = true;
				if (quote_level == "3")
				{
					rows = new[]
					{
						openquote,
						screeningcompleted,
						scheduleproduced,
						manpower,
						customerinfo,
						marketinfo,
						financeinfo,
						reconinfo,
						stage4,
						sellstrategy,
						projectestimated,
						worksheetreview,
						stage6,
						quotedelivered,
						followup1,
						followup2,
						convertkill
					};
				}
				else
				{
					rows = new[]
					{
						openquote,
						screeningcompleted,
						scheduleproduced,
						sellstrategy,
						projectestimated,
						worksheetreview,
						stage6,
						quotedelivered,
						followup1,
						convertkill
					};
				}
			}
		}

		public QuoteStrategyCheckItem[] rows { get; set; }

		private QuoteStrategyStageMeetingItem stage6 { get; set; }

		private QuoteStrategyStageMeetingItem stage4 { get; set; }


		public bool btnsell_done_enabled { get; set; }

		public bool btn_print_final_quote_enabled { get; set; }

		public bool clear_manpower_enabeld { get; set; }

		public bool btnmanpower_done_enabled { get; set; }

		public bool clear_finance_enabled { get; set; }

		public bool btnfinance_done_enabled { get; set; }

		public bool clear_market_enabled { get; set; }

		public bool btnmarket_done_enabeld { get; set; }

		public bool clear_customer_enabled { get; set; }

		public bool btn_custrecon_done_enabled { get; set; }

		public bool convert_button_enabled { get; set; }

		public bool kill_button_visible { get; set; }

		public bool kill_button_enabled { get; set; }



		private QuoteStrategyCheckItem postmortem { get; set; }

		private QuoteStrategyCheckItem convertkill { get; set; }

		private QuoteStrategyCheckItem followup2 { get; set; }

		private QuoteStrategyCheckItem followup1 { get; set; }

		private QuoteStrategyCheckItem quotedelivered { get; set; }

		private QuoteStrategyCheckItem worksheetreview { get; set; }

		private QuoteStrategyCheckItem projectestimated { get; set; }

		private QuoteStrategyCheckItem sellstrategy { get; set; }

		private QuoteStrategyCheckItem reconinfo { get; set; }

		private QuoteStrategyCheckItem financeinfo { get; set; }

		private QuoteStrategyCheckItem marketinfo { get; set; }

		private QuoteStrategyCheckItem customerinfo { get; set; }

		private QuoteStrategyCheckItem manpower { get; set; }

		private QuoteStrategyCheckItem scheduleproduced { get; set; }

		private QuoteStrategyCheckItem screeningcompleted { get; set; }

		private QuoteStrategyCheckItem openquote { get; set; }

		public bool is_quotedbys_bm { get; set; }

		public bool tabenable_5_7 { get; set; }

		public bool tabenable_1_4 { get; set; }


		//		protected void populate_recon_scores()
		//		{
		//
		//			Manpower_score = get_score("Manpower").ToString("N");
		//
		//			Market_score = get_score("Market").ToString("N");
		//
		//			Customer_score = get_score("Customer").ToString("N");
		//
		//			Finance_score = get_score("Finance").ToString("N");
		//		}


	}
}