using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.BLL.Pages.Shared.PickList.Quote;
using NESI.Common.Models;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Quotes;

namespace NESI.BLL.Pages.Quotes
{
	public class QuoteEdit : QuoteBLLBase
	{
		public QuoteEdit(Employee user, int quoteId, int revision) : base(user)
		{
			_quote.quote_id = quoteId.ToString();
			_quote.revision = revision.ToString();
			_quote.InitializeQuote();
		}

		public QuoteEdit(Employee user) : base(user)
		{

		}

		public string AddFollowUp(QuoteFollowUp model)
		{
			var set = _quote.schedule_followup(model.schedule_date.ToString("yyyy-MM-dd"), model.follow_up_note);
			return set ? "Success" : "Failed";
		}

		public string DoneFollowUp(int follw_up_id)
		{
			bllToolbox.doSQL_void(@"update quote_follow_up set is_done=1, done_datetime=NOW(), done_member_id=@p1 where id=@p0", follw_up_id, UserId);
			return "Success";
		}

		public DataTable GetFollowHistory()
		{
			return _quote.get_followup_history();
		}

		public string status_update(QuoteStatusUpdate model)
		{
			if (model.status_id == "6")
			{
				_quote.who_competitor = model.who_competitor;
				_quote.why_lose = model.why_lose;
				_quote.what_price = model.what_price;
			}
			_quote.status_id = model.status_id;
			if (model.is_revive)
			{
               _quote.status_id =  bllToolbox.doSQL_string("SELECT prev_status_id from quote_master where quote_id=@v0 and revision=@v1", model.quote_id, model.revision);

				if (string.IsNullOrEmpty(_quote.status_id) || _quote.status_id == "0" || _quote.status_id == "8" || _quote.status_id == "3" || _quote.status_id == "4")
				{
					_quote.status_id = "1";
				}
				return _quote.status_update() ? _quote.set_active_revision() : "Failed.";
			}
			return _quote.status_update() ? "Success." : "Failed.";
		}

		public DataExtra re_order(DTO.ViewModels.Core.ReOrderItem[] models, int t)
		{
			if (models == null)
			{
				models = bllToolbox.doSQL_Array<ReOrderItem>(@"SELECT id, if(order_number<0,line_number,order_number) `order`, is_checked from quote_extratext where quote_id=@p0 and revision=@p1 and type=@p2 order by `order`",
					_quote.quote_id, _quote.revision, t);
			}
			var step = 0;
			foreach (var model in models)
			{

				if (model.is_checked)
				{
					bllToolbox.doSQL_void(@"UPDATE quote_extratext SET order_number=@v1, line_number = @v2 WHERE id = @v0 LIMIT 1",
						model.id, model.order, step);
					var detail_row = bllToolbox.doSQL_dt(@"SELECT * FROM quote_extratext WHERE id = @v0 ", model.id).Rows[0];
					var detail_name = detail_row["linetext"].ToString().Trim();
					var type = Convert.ToInt32(detail_row["type"]);
					//				if (type == 1)
					//				{
					// Need to update the associated section also.
					var section_id =
						bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(id),0) FROM quote_section WHERE detail_id = @v0", model.id);
					if (section_id != 0)
					{
						var picklist_controlled = Convert.ToBoolean(bllToolbox.doSQL_int(
							@"SELECT IFNULL(MAX(picklist_controlled),0) FROM quote_section WHERE id = @v0",
							section_id));
						if (!picklist_controlled)
						{
							//var step = Convert.ToInt32(detail_row["line_number"]);
							detail_name = $"{step + 1}. {detail_name.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ")}";
							if (type == 2)
							{
								detail_name = $"N{detail_name}";
							}
							var len = detail_name.Length > 60 ? 60 : detail_name.Length;
							bllToolbox.doSQL_void(@"UPDATE quote_section SET section = @v0 WHERE id = @v1 LIMIT 1",
								detail_name.Substring(0, len).Trim(), section_id);
						}
					}
					step++;
				}
				else
				{
					bllToolbox.doSQL_void(@"UPDATE quote_extratext SET order_number=@v1, line_number = @v1 WHERE id = @v0 LIMIT 1",
						model.id, model.order);
				}
			}
			return
				new DataExtra()
				{
					Data = "Saved successfully.",
					Extra =
					new
					{
						sections = new PickListQuote(CurrentUser).GetSectionList(Convert.ToInt32(_quote.quote_id), Convert.ToInt32(_quote.revision)),
						div = _quote.divs(t)
					}
				};
		}

		public QuoteEditInit InitilizeQuote(int quoteId, int revision)
		{
			_quote.quote_id = quoteId.ToString();
			_quote.revision = revision.ToString();
			_quote.InitializeQuote();
			var o = new QuoteEditInit();
			if (quoteId < 100000)
			{
				o.HasError = true;
				o.ErrorMessage =
					"This quote was cut using the ACCESS based quote program, not the NESI quote program... all NESI quotes have a quote number greater than 100000.";
				o.ErrorTitle = "";
				return o;
			}

			var popstage = false;
			var post_mortemt = false;
			//			var errormessage = "";
			//			var errortitle = "";

			if (_quote.wo != "" && _quote.status_id == "8")
			{
				// Check if this quote is used on a work order
				var c_wo = bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE woprog_quoteid =@v0 ",
					_quote.quote_id + _quote.revision);

                //if there is no quote used on the work order AND if the quote does not have its T&M flag set
				if (c_wo == 0 && _quote.is_tm != true)
				{
					// Unreceive it.
					bllToolbox.doSQL_void(
						@"UPDATE quote_master SET status_id = 4, wo = null WHERE quote_id = @v0  AND revision = @v1 ", _quote.quote_id,
						_quote.revision);
					_quote.InitializeQuote();
				}
			}
			if (!CurrentUser.VisibleBusinessUnitIdList.ToArray().Contains(_quote.business_unit_id))
			{
				o.HasError = true;
				o.ErrorMessage =
					"This quote is from a different business unit than your's - A privilege is required for access to other business units.";
				o.ErrorTitle = "";
				return o;
			}

			var comp = new NeBusinessUnit(_quote.business_unit_id);
			var bm = comp.branch_manager;

			if (bm.id == 0)
			{
				bm = new NeMember(37);
			}

			if (_quote.status_id == "10") // if we are waiting for stage 1
			{
				if (_quote.expected_value >= comp.quote_level_3_start) // if its a level 3 quote
				{
					if (NeMember.is_supervisor(Convert.ToInt32(_quote.quoted_by), current_user.id) ||
						comp.branch_manager.id == current_user.id || NeMember.is_supervisor(comp.branch_manager.id, current_user.id))
					{
						// iframe_approval.Attributes["src"] = "approval_frame.aspx?qid=" + _quote.quote_id;
						popstage = true;
					}
					else
					{
						var name = comp.branch_manager.FullName;
						if (name == "")
						{
							bm = comp.regional_manager;
							if (bm.FullName.Trim() == "")
							{
								bm = new NeMember(37);
							}
						}
						o.HasError = true;
						o.ErrorMessage = "This quote has not been approved to be worked on yet!  Stage 1 approval by " +
									   comp.branch_manager.FullName + " is required before you can start to work on this.";
						o.ErrorTitle = "Waiting for Quote LEVEL 3 Authorization";
						return o;
					}
				}
				else if (_quote.expected_value >= comp.quote_level_2_start) //else if its a level 2 quote
				{
					if (current_user.id == bm.id || NeMember.is_supervisor(bm.id, current_user.id))
					{
						// iframe_approval.Attributes["src"] = "approval_frame.aspx?qid=" + _quote.quote_id;
						popstage = true;
					}
					else
					{
						var name = bm.FullName.Trim();
						if (name == "")
						{
							bm = comp.regional_manager;
							if (bm.FullName.Trim() == "")
							{
								bm = new NeMember(37);
							}
						}
						o.HasError = true;
						o.ErrorMessage = "This quote has not been approved to be worked on yet!  Stage 1 approval by " + bm.FullName +
									   " is required before you can start to work on this.";
						o.ErrorTitle = "Waiting for Quote LEVEL 2 Authorization";
						return o;
					}
				}
			}
			else if (_quote.status_id == "13")
			{

			}
			else if (_quote.status_id == "6")
			{
				post_mortemt = true;
			}

			var received_button_disabled = _quote.status_id != "4" && _quote.status_id != "8";
			var hide_price_to = _quote.price_to == "NULL" || _quote.price_to == "" || _quote.pricetype_id != "6";

			var wo_link = "";
			var wo = _quote.wo;
			if (wo != "N/A")
			{
				var woprog_id = bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(woprog_id), 0) FROM woprog  WHERE woprog_bvwo =@v0",
					wo.PadLeft(10, '0'));
				if (woprog_id != 0)
				{
					var woprog = new NeWOProg(woprog_id);
					//TODO: change this url to angular router link after finish work order page
					wo_link = wo == "N/A"
						? wo
						: string.Format(
							@"<a href=""javascript:boing('/sections/workorder/index.aspx?woprog_id={2}&business_unit_id={1}', 'wo');"">{0}</a>",
							woprog.OrderNumber, woprog.business_unit_id, woprog.woprog_id);
				}
				else
				{
					wo_link = "N/A";
				}
			}
			else
			{
				wo_link = wo;
			}
			var show_workseet_tab = _quote.this_member.AuthenticatedForPrivilege(163);
			var why_revised = Convert.ToInt32(revision) == 1
				? ""
				: bllToolbox.doSQL_string(@"SELECT why_revised FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ",
					quoteId, revision);

			var is_qced = IsQced(_quote.customer_id);
			var is_actived = IsActived(_quote.customer_id);
			var chance_winning_why = bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT quote_chance_id value, quote_chance_name label FROM quote_chance ORDER BY quote_chance_id");
			foreach (var item in chance_winning_why)
			{
				if (item.Label.Trim() == "")
				{
					item.Label = "　";
				}
			}

			var disabled_status = new[]
			{
				2,3, 4, 6, 8,9
			};
			if (_quote.status_id != "NULL")
			{
				o.edit_disabled = (Array.IndexOf(disabled_status, Convert.ToInt32(_quote.status_id)) > -1);
			}


			o.privilege61 = Privilege61;
			o.HasError = false; // !string.IsNullOrWhiteSpace(errormessage);
								//			o.ErrorTitle = errortitle;
								//			o.ErrorMessage = errormessage;
			o.Popstage = popstage;
			o.PostMortemt = post_mortemt;
			o.Received_button_disabled = received_button_disabled;
			o.Hide_price_to = hide_price_to;
			o.wo = _quote.wo;
			o.po = _quote.po;
			o.wo_link = _quote.wo_link;
			o.Po_link = _quote.po_link;
			o.show_workseet_tab = show_workseet_tab;
			o.why_revised = why_revised;
			o.business_unit_id = _quote.business_unit_id;
		    o.revenue_line_id = "0"; //_quote.RevenueLine_Id;

            o.quote_id = _quote.quote_id;
			o.Revisions = _quote.revisions();
			o.customer_id = _quote.customer_id;
			o.expected_value = _quote.expected_value;
			o.contact_id = _quote.contact_id;
			o.open_date = _quote.open_date;
			o.date_due = _quote.date_due;
			o.date_expected_start = _quote.date_expected_start;
			o.is_Qced = is_qced;
			o.is_actived = is_actived;
			o.job_description = _quote.job_description;
			o.cust_spec_doc = _quote.cust_spec_doc;
			o.quoted_by = _quote.quoted_by;
			o.quoted_by_name = new nesi.core.NeMember(Convert.ToInt32(o.quoted_by)).FullName;
			int.TryParse(o.quoted_by, out var QuotedBy);
			var quoted_by_list = GetUserListByBusinessUnit(Convert.ToInt32(_quote.quoted_business_unit_id),false);
			if (quoted_by_list.Select(x => x.Value.ToString() == o.quoted_by).ToArray().Length <= 0)
			{
				quoted_by_list.Add(new LabelValueInt() { Label = o.quoted_by_name, Value = QuotedBy});
			}

			o.quoter_locked_bool = _quote.quoter_locked_bool;
			o.last_print_date = _quote.last_print_date;
			o.chance_winning = _quote.pct_chance;
			o.chance_winning_reason = _quote.pct_chance_reason;
			o.chance_winning_note = _quote.pct_chance_note;
			o.chance_winning_why = chance_winning_why;
			o.last_fax_date = _quote.last_fax_date;                                          // 11
			o.verified_date = _quote.verified_date;
			o.hours_spent = _quote.hours_spent;
			o.dollars_spent = _quote.dollars_spent;
			o.quoted_price = _quote.quoted_price;
			o.price_to = _quote.price_to;
			o.pricetype_id = _quote.pricetype_id;
			o.pricetype_select = _quote.pricetype_select;
			o.takeoff_price = _quote.takeoff_price;                                          // 17
			o.tm_pricing = _quote.tm_pricing;                                             // 18
			o.percent_down = _quote.percent_down;                                           // 19
			o.net_due = _quote.net_due;                                                // 20
			o.customer_term = _quote.custom_term;                                            // 21
			o.customer_name = _quote._customer.Customer_Name;                                // 22
			o.current_quoter = _quote.current_quoter;                                         // 23
			o.contact_select = _quote.contact_select;                                         // 25
			o.us_currency = _quote.us_currency == "checked";                                            // 26
			o.inflation_term = _quote.inflation_term == "checked";                                         // 27
			o.address_addr1 = _quote._address.Addr1;                                         // 28
			o.address_phoneNumber = _quote._address.PhoneNumber;                                   // 29
			o.address_faxNumber = _quote._address.FaxNumber;                                     // 30
			o.contact_email = _quote._contact.Contact_Email;                                 // 31
			o.divs1 = _quote.divs(1);                                                // 32
			o.divs2 = _quote.divs(2);                                                // 33
			o.quote_status_button = _quote.quote_status_button;                                    // 34
			o.status_id = _quote.status_id;                                              // 35
			o.prev_status_id = _quote.prev_status_id;                                         // 36
			o.revision = _quote.revision;                                               // 37
			o.not_used = "NOT USED";                                             // 38
			o.po = _quote.po;                                  // 39
			o.wo = _quote.wo;                                  // 40
			o.tm_pricing_total = _quote.tm_pricing_total();                                     // 41
			o.title_options = _quote.get_title_options();                                    // 44
			o.company_list = GetBusinessUnitList();                                 // 45
			o.kill_reason__map = _quote.kill_reason_map();                                      // 46
			o.competitor_id = _quote.competitor_id;                                          // 47
			o.quoted_business_unit_id = _quote.quoted_business_unit_id;                                        // 48
			o.address_id = _quote._address.id;
			o.address_addr2 = _quote._address.Addr2;                                  // 49
			o.address_addr3 = _quote._address.Addr3;                                  // 50
			o.address_addr4 = _quote._address.Addr4;                                  // 51
			o.address_city = _quote._address.City;                                    // 52
			o.address_prov = _quote._address.Prov;                                    // 53
			o.address_postal = _quote._address.Postal;                                // 54
			o.address_country = _quote._address.Country;                              // 55
			o.contact_cellPhone = _quote._contact.Contact_CellPhone;                  // 56
			o.contact_extension = _quote._contact.Contact_Extension;                  // 57
			o.search_pan = _quote.search_pane();                                      // 58
			o.term_options = _quote.term_options();                                   // 59
			o.completion_date = _quote.completion_date;                               // 60
			o.status_health = _quote.status_health;                                   // 63
			o.status_title = _quote.status_title;                                     // 64
			o.quoter_locked_str = _quote.quoter_locked_str;                           // 65
			o.quoted_business_unit_id_locked = _quote.quoted_business_unit_id_locked; // 66
			o.worksheet_total = _quote.worksheet_total();                             // 67
			o.show_help_checked = _quote.show_help_checked;                           // 68
			o.active_revision = _quote.active_revision;                               // 69
			o.service_report = _quote.service_report();                               // 70
			o.include_title = _quote.include_title == "1";                            // 71
			o.status = _quote.status;                                                 // 73
			o.address_options = address_options(Convert.ToInt32(_quote.customer_id)); // 74                           
			o.build_canned_descriptions = _quote.build_canned_descriptions();         // 75
			o.build_percent_spinner = _quote.build_percent_spinner();                 // 76
			o.strategy_tab = _quote.strategy_tab;                                     // 78
			o.post_mortem_button = _quote.post_mortem_button;                         // 79
			o.print_buttons = _quote.print_buttons;                                   // 80
			o.initial_quoted_by_list = quoted_by_list.ToArray();                      // {81}
			o.build_percent_down = _quote.build_percent_down();                       // 82
			o.follow_up = _quote.follow_up;                                           // 84
			o.ts_ticks = _quote.ts_ticks;                                             // 85
			o.currency = _quote.currency;                                             // 86
			o.exp_podate = _quote.exp_podate;                                         //89
			o.exist_quotes = new QuoteNew(CurrentUser).GetQuotesByCustomerId(Convert.ToInt32(_quote.customer_id));
			o.print_draft_show = !(Convert.ToInt32(_quote.status_id) >= 3 &&
								   !Toolbox.Contains(_quote.status_id, new[] { "6", "10", "11", "12" }));
			o.uses_quote_process = _quote.this_company.uses_quote_process != 1;
			o.service_report_hider = !_quote.this_company.is_er;
			o.service_report_disabled = wo == "" || wo == "N/A";
			o.followup_history = _quote.get_followup_history();
            o.revenueLines = _quote.revenueLines;
            o.is_tm = _quote.is_tm;
			o.period_valid_value = _quote.period_valid_value();
			o.period_valid = _quote.period_valid;
            o.netsuite_estimate_internal_id = _quote.netsuite_estimate_internal_id;
			o.opportunity_internal_id = _quote.opportunity_internal_id;
			o.parallel_bid = _quote.parallel_bid;
			o.bdm = _quote.bdm;
			o.bdm_options = bdm_options();
			o.acting_bdm = _quote.acting_bdm;
			o.acting_bdm_options = acting_bdm_options();
			var canAdjustSalesReps = this.CurrentUser.AuthenticatedForPrivilege(OpsPrivilege.ManageSalesReps);
			var reportLine = NeMember.GetReportingLineList(CurrentUser.Id); 
			var canPMAdjustSalesReps = CurrentUser.AuthenticatedForPrivilege(OpsPrivilege.CanPMAdjustBDM) && (CurrentUser.Id == QuotedBy || reportLine.Contains(QuotedBy));
			o.isBdmReadOnly = !canAdjustSalesReps && !canPMAdjustSalesReps; 
			return o;

		}


		public LabelValueInt[] address_options(int customerId)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				@"SELECT address_id value, CONCAT(address_type, '-', address_addr1) label 
				FROM address  WHERE address_table = 'customer' AND address_table_id =@v0 AND Active=true", customerId);
		}

        public LabelValueInt[] bdm_options()
        {
            return bllToolbox.doSQL_Array<LabelValueInt>(
                @"SELECT 
                    netsuite_employee_internal_id value,
                    netsuite_employee_name label
                  FROM netsuite_sales_rep
                  WHERE netsuite_isinactive = 0
                  AND netsuite_issales_rep = 1"
            );
        }

		public LabelValueInt[] acting_bdm_options()
        {
			return bllToolbox.doSQL_Array<LabelValueInt>(
                @"SELECT 
                    a.member_id value,
                    a.member_fullname label
                  FROM member a
					INNER JOIN membertype b ON a.member_membertype_id = b.membertype_id 
					WHERE b.considered_ram = 1
                  and a.member_status ='Active' union select 0 value, 'No One' label  order by label");

		}

		public QuoteEditCustomerAddressInfo GetCustomerAddressInfo(int customerId, int addressId = 0)
		{
			var options = address_options(customerId);
			if (options == null || options.Length == 0) return null;
			if (addressId == 0)
			{
				addressId = options[0].Value;
			}
			var address = new NEAddress(addressId);
			return new QuoteEditCustomerAddressInfo()
			{
				address_options = options,
				address_addr1 = address.Addr1,
				address_addr2 = address.Addr2,
				address_addr3 = address.Addr3,
				address_addr4 = address.Addr4,
				address_city = address.City,
				address_country = address.Country,
				address_faxNumber = address.FaxNumber,
				address_phoneNumber = address.PhoneNumber,
				address_postal = address.Postal,
				address_prov = address.Prov,
                address_is_active = address.Active
			};
		}


		public bool IsQced(string customerid)
		{
			return bllToolbox.doSQL_int(@"SELECT COUNT(customer_id) 
					FROM customer WHERE customer_id = @v0  AND customer_qc_member_id 
					IS NOT NULL AND customer_qc_datetime IS NOT NULL", customerid) > 0;
		}

		public bool IsActived(string customerId)
		{
			if (!string.IsNullOrEmpty(customerId) && customerId != "NULL")
			{
				var customer = new NECustomer(Convert.ToInt32(customerId));
				return customer.Hold != "T";
			}
			return false;
		}

		public string set_active_revision()
		{
			return _quote.set_active_revision();
		}

		public DataExtra duplicate_quote()
		{
			var result = _quote.duplicate_quote();
		
			
			if (int.TryParse(result, out var quoteid))
			{
				return new DataExtra()
				{
					Data = "Success",
					Extra = quoteid,
				};
			}
			else
			{
				return new DataExtra()
				{
					Data = "Failed",
					Extra = result,
				};
			}

		}

		public DataExtra Update_last_printed()
		{
			return new DataExtra
			{
				Data = "Success",
				Extra = _quote.update_last_printed()
			};
		}
		public string Get_last_sent_printed()
		{
			return _quote.get_last_sent();
		}

		public DataExtra UpdateAll(QuoteEditUpdateAll model)
		{
			// ReSharper disable once RedundantAssignment
			var quote_id = model.quote_id;
			var revision = model.revision;
			var resp = "";
			var r_ticks = model.ts_ticks;
			var extraData = "";
			if (quote_id == 0 || revision == 0)
			{
				resp = "Invalid Request";
				extraData = "close";
			}
			else
			{

				var q_ticks = bllToolbox.doSQL_datetime(@"SELECT last_saved last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", quote_id, revision);
				if (r_ticks < q_ticks.Ticks)
				{
					resp = "InvalidState: this quote has been already modifed by another user.";
					extraData = "refresh";
				}
				else
				{
					var status_id = bllToolbox.doSQL_int(@"SELECT status_id FROM quote_master WHERE quote_id = @v0 AND revision =@v1 LIMIT 1", quote_id, revision);
                 
					var obj = _db.quote_master.FirstOrDefault(x => x.quote_id == quote_id && x.revision == revision);
					if (obj == null)
					{
						resp = "Invalid Request: Quote does not exist.";
						extraData = "close";
					}
					else if(obj.business_unit_id != model.quoted_business_unit_id && IsQuoteIntegratedToNs(quote_id, revision))
                    {
                        resp = "The quote is integrated with NetSuite. Business unit can not be changed!";
                        extraData = "refresh";
                    }
                    else
					{
                      

                        obj.customer_id = model.customer_id;
						obj.Contact_ID = model.customer_contact;
						obj.date_due = model.date_due;
						obj.date_expected_start = model.date_expected_start;
						obj.exp_podate = model.exp_podate;
						obj.pct_chance = model.chance_winning;
						obj.pct_chance_reason = model.chance_winning_reason;
						var wpn = new NeWoProgNotes(quote_id, "Q")
						{
							woprog_project_notes_memberid = bllToolbox.doSQL_int(
								@"SELECT quoted_by FROM quote_master WHERE quote_id = @v0 AND revision = @v1", quote_id, revision),
							woprog_project_notes_notes = model.chance_winning_note,
							woprog_project_notes_woprogid = quote_id,
							woprog_project_notes_Type = "Q"
						};
						wpn.SaveWOProgProjectNote();
						obj.completion_date = model.completion_date;
						obj.job_description = model.job_description;
						obj.cust_spec_doc = model.cust_spec_doc;
                        obj.business_unit_id = model.quoted_business_unit_id;
					    obj.revenue_line_id = 0; //model.revenue_line_id;
                        obj.quoted_by = model.quoted_by;
						//obj. = model.quoted_by;
						obj.quoted_by = model.quoted_by;
						obj.follow_up = model.follow_up;
                        obj.is_tm = model.is_tm;
						obj.quoter_locked = model.quoter_locked_bool ? 1 : 0;
						
						obj.expected_value = model.expected_value;
						obj.us_currency = model.us_currency ? 1 : 0;
						obj.currency = model.us_currency ? 1 : 2;
						obj.inflation_term = model.inflation_term ? 1 : 0;
						obj.percentage_down = model.percent_down;
						obj.net_due = model.net_due;
						obj.custom_term = model.customer_term;
						obj.address_id = model.address_id;
						obj.period_valid = model.period_valid;
						obj.opportunity_internal_id = model.opportunity_internal_id ?? 0;
						obj.parallel_bid = model.parallel_bid;
						obj.bdm = model.bdm ?? 0;
						obj.acting_bdm = model.acting_bdm ?? 0;
						if (status_id > 1)
                        {
                            if (obj.quoted_price != model.quoted_price || obj.price_to != model.price_to || obj.pricetype_id != model.pricetype_id)
                            {
                                resp = "Invalid Request: Quote Price can't be changed.";
                                extraData = "close";
                            }
                            else
                            {
                                obj.quoted_price = model.quoted_price;
                                obj.price_to = model.price_to;
                                obj.pricetype_id = model.pricetype_id;
                                _db.quote_master.AddOrUpdate(obj);
                                _db.SaveChanges();
                                bllToolbox.doSQL_void(@"UPDATE quote_master SET last_saved = NOW() WHERE quote_id = @v0  AND revision = @v1 ", quote_id, revision);
                                q_ticks = bllToolbox.doSQL_datetime(@"SELECT last_saved as last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", quote_id, revision);

                                extraData = q_ticks.Ticks.ToString();
                                resp = "Quote has been updated successfully.";
                            }
                        }
                        else
                        {
                            obj.quoted_price = model.quoted_price;
                            obj.price_to = model.price_to;
                            obj.pricetype_id = model.pricetype_id;
                            _db.quote_master.AddOrUpdate(obj);
                            _db.SaveChanges();
                            bllToolbox.doSQL_void(@"UPDATE quote_master SET last_saved = NOW() WHERE quote_id = @v0  AND revision = @v1 ", quote_id, revision);
                            q_ticks = bllToolbox.doSQL_datetime(@"SELECT last_saved as last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", quote_id, revision);

                            extraData = q_ticks.Ticks.ToString();
                            resp = "Quote has been updated successfully.";
                        }
                     

					
					}
				}
			}


			return new DataExtra()
			{
				Data = resp,
				Extra = extraData
			};
		}


		public DataExtra UpdateField(DTO.ViewModels.Page.Quotes.QuoteEditFieldUpdate model)
		{
			// ReSharper disable once RedundantAssignment
			var temp_dt = new DateTime(1969, 6, 1);
			var do_save = true;
			var quote_id = model.QuoteId;
			var revision = model.Revision;
			var resp = "";
			var r_ticks = model.Ticks;
			var line_id = model.Line_id;
			var extraData = new object();
            
			if (quote_id == 0 || revision == 0)
			{
				resp = "Invalid Request";
			}
			else
			{
				var q_ticks = bllToolbox.doSQL_datetime(@"SELECT last_saved last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", quote_id, revision);
				if (r_ticks < q_ticks.Ticks)
				{
					//resp				= "InvalidState";
				}
				DateTime last_modified;
				var status_id = bllToolbox.doSQL_int(@"SELECT status_id FROM quote_master WHERE quote_id = @v0 AND revision =@v1 LIMIT 1", quote_id, revision);
				var fields = "";
				var v = model.FieldValue;
				long ticks = 0;
             
				if (resp == "")
				{
					var extra = "";
					switch (model.FieldName)
					{

						case "customer_id":
							fields = $@"customer_id = '{v}', contact_id = null, address_id = null";
							break;
						case "customer_contact":
							fields = $@"contact_id = '{v}'";
							break;
						case "date_due":
							// Valid Date?
							DateTime.TryParse(v.ToString(), out temp_dt);
							if (temp_dt.Year == 1969)
							{
								resp = "Invalid Date";
							}
							else
							{
								fields = $@"date_due = '{Toolbox.MySQL_shortdt(temp_dt)}'";
							}
							break;
						case "date_expected_start":
							// Valid Date?
							DateTime.TryParse(v.ToString(), out temp_dt);
							if (temp_dt.Year == 1969)
							{
								resp = "Invalid Date";
							}
							else
							{
								fields = $@"date_expected_start = '{Toolbox.MySQL_shortdt(temp_dt)}'";
							}
							break;
						case "exp_podate":
							// Valid Date?
							DateTime.TryParse(v.ToString(), out temp_dt);
							if (temp_dt.Year == 1969)
							{
								resp = "Invalid Date";
							}
							else
							{
								fields = $@"exp_podate = '{Toolbox.MySQL_shortdt(temp_dt)}'";
							}
							break;
						case "chance_winning":
							fields = $@"pct_chance = '{v}'";
							break;
						case "chance_winning_reason":
							fields = $@"pct_chance_reason = '{v}'";
							break;
						case "chance_winning_note":
							try
							{
								var wpn = new NeWoProgNotes(quote_id, "Q")
								{
									woprog_project_notes_memberid = bllToolbox.doSQL_int(
										@"SELECT quoted_by FROM quote_master WHERE quote_id = @v0 AND revision = @v1", quote_id, revision),
									woprog_project_notes_notes = v,
									woprog_project_notes_woprogid = quote_id
								};
								wpn.SaveWOProgProjectNote();
								// resp = "SUCCESS";
							}
							catch (Exception ee)
							{
								resp = ee.ToString();
							}
							break;
						case "completion_date":
							// Valid Date?
							do_save = DateTime.TryParse(v, out temp_dt);
							if (temp_dt.Year == 1969)
							{
								resp = "Invalid Date";
							}
							else
							{
								fields = $@"completion_date = '{Toolbox.MySQL_shortdt(temp_dt)}'";
							}
							break;
						case "job_description":
							fields = $@"job_description = '{Toolbox.AddSlashes(v)}'";
							break;
						case "cust_spec_doc":
							fields = $@"cust_spec_doc = '{Toolbox.AddSlashes(v)}'";
							break;
						case "quoted_business_unit_id":
                            var isQuoteIntegratedToNs = IsQuoteIntegratedToNs(quote_id, revision);
                            if(isQuoteIntegratedToNs)
                            {
                                resp = "The quote is integrated with NetSuite. Business unit can not be changed!";
                                extraData = "refresh";
							}
                            else
                            {
								fields = $@"business_unit_id = '{v}'";
							}
                            break;
                        case "revenue_line_id":
                            // fields = $@"revenue_line_id = '{v}'";
                            break;
                        case "s_department":
							fields = $@"division_id = '{v}'";
							break;
						case "quoted_by":
							fields = $@"quoted_by = '{v}'";
							break;
						case "follow_up":
							fields = $@"follow_up = '{v}' ";
							break;
                        case "is_tm":
                            fields = $@"is_tm = '{v}' ";
                            break;
                        case "quoter_locked_bool":
							fields = $@"quoter_locked = '{v}' ";
							break;
						case "last_print_date":
                            extra = status_id == 1 ? "prev_status_id = 1, status_id = 2" : "";
                            fields = $@"last_print_date = '{v}', {extra}";
                            break;
						case "last_fax_date":
							extra = status_id == 2 ? "prev_status_id = 2, status_id = 3" : "";
							fields = $@"last_fax_date = '{v}', {extra}";
							break;
						case "verified_date":
							extra = status_id == 3 ? "prev_status_id = 3, status_id = 4" : "";
							fields = $@"verified_date = '{v}', {extra}";
							break;
						case "quoted_price":
							double temp_double = 0;
							do_save = double.TryParse(v, out temp_double);
							fields = $@"quoted_price = '{temp_double}'";
							break;
						case "s_quotedpriceto":
							fields = $@"price_to = '{v}'";
							break;
						case "pricetype_id":
							fields = $@"pricetype_id = '{v}'";
							break;
						case "expected_value":
							fields = $@"expected_value = '{v}'";
							break;
						case "us_currency":
							fields = $@"us_currency = {v},currency={(v == "1" ? 1 : 2)}";
							break;
						case "inflation_term":
							fields = $@"inflation_term = {v}";
							break;
						case "percent_down":
							fields = $@"percentage_down = '{v}'";
							break;
						case "net_due":
							fields = $@"net_due = '{v}'";
							break;
						case "customer_term":
							fields = $@"custom_term = '{Toolbox.AddSlashes(v)}'";
							break;
						case "s_addressid":
							fields = $@"address_id = '{v}'";
							break;
						case "include_title":
							fields = $@"include_title = " + (v.ToLower() == "true" ? "1" : "0");
							break;
						case "period_valid":
							fields = $@"period_valid = '{v}'";
							break;
						case "detailrow":
						case "notesrow":
							try
							{
								var trimmedLineText = v.Trim();
								bllToolbox.doSQL_void(@"UPDATE quote_extratext SET linetext = @v0 WHERE id = @v1 LIMIT 1", trimmedLineText, line_id);
								//								if (model.FieldName == "detailrow")
								//								{
								var detail_row = bllToolbox.doSQL_dt(@"SELECT * FROM quote_extratext WHERE id = @v0 ", line_id).Rows[0];
								// Need to update the associated section also.
								var section_id = bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(id),0) FROM quote_section WHERE detail_id = @v0", line_id);
								if (section_id != 0)
								{
									var picklist_controlled = Convert.ToBoolean(bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(picklist_controlled),0) 
FROM quote_section WHERE id = @v0", section_id));
									if (!picklist_controlled)
									{
										var step = Convert.ToInt32(detail_row["line_number"]);
										var detail_name = $"{step + 1}. {trimmedLineText.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ")}";
										if (model.FieldName == "notesrow")
										{
											detail_name = $"N{detail_name}";
										}
										var len = detail_name.Length > 60 ? 60 : detail_name.Length;
										bllToolbox.doSQL_void(@"UPDATE quote_section SET section = @v0 WHERE id = @v1 LIMIT 1", detail_name.Substring(0, len).Trim(), section_id);
									}
								}
								//								}
								last_modified = bllToolbox.doSQL_datetime(@"SELECT last_saved last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", quote_id, revision);
								ticks = last_modified.Ticks;
								extraData = ticks.ToString();
							}
							catch (Exception ee)
							{
								resp = ee.ToString();
							}
							do_save = false;
							break;
						case "detail_row_is_checked":
						case "notes_row_is_checked":
							bllToolbox.doSQL_void(@"UPDATE quote_extratext SET is_checked = @v0 WHERE id = @v1 LIMIT 1", v, line_id);
							var s_id = bllToolbox.doSQL_int(@"SELECT ifnull(section_id,0) from quote_extratext where id=@v0", line_id);
							var s_id2 = bllToolbox.doSQL_int(@"SELECT id FROM quote_section WHERE detail_id = @v0", line_id);
							if (s_id == 0 && s_id2 > 0)
							{
								s_id = s_id2;
								bllToolbox.doSQL_void(@"UPDATE quote_extratext set section_id=@p0 where id=@p1", s_id, line_id);
							}
							if (s_id > 0)
							{
								bllToolbox.doSQL_void(@"UPDATE quote_section SET is_checked = @v0 WHERE id = @v1 LIMIT 1", v, s_id);
							}
							var t = model.FieldName == "detail_row_is_checked" ? 1 : 2;
							this._quote.quote_id = quote_id.ToString();
							this._quote.revision = revision.ToString();
							extraData = this.re_order(null, t).Extra;
							do_save = false;
							break;
						case "update_order":
							if (v != "")
							{
								var asdf = v.Split(',');
								foreach (var f in asdf)
								{
									var bo = f.Split('|');
									var id = bo[0];
									var oo = bo[1];
									bllToolbox.doSQL_void(@"UPDATE quote_extratext SET line_number = @v1 WHERE id = @v0 LIMIT 1", id, oo);
									var detail_row = bllToolbox.doSQL_dt(@"SELECT * FROM quote_extratext WHERE id = @v0 ", id).Rows[0];
									var detail_name = detail_row["linetext"].ToString().Trim();
									var type = Convert.ToInt32(detail_row["type"]);
									if (type == 1)
									{
										// Need to update the associated section also.
										var section_id = bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(id),0) FROM quote_section WHERE detail_id = @v0", id);
										if (section_id != 0)
										{
											var picklist_controlled = Convert.ToBoolean(bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(picklist_controlled),0) FROM quote_section WHERE id = @v0",
												section_id));
											if (!picklist_controlled)
											{
												var step = Convert.ToInt32(detail_row["line_number"]);
												detail_name = $"{step + 1}. {detail_name.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ")}";
												var len = detail_name.Length > 60 ? 60 : detail_name.Length;
												bllToolbox.doSQL_void(@"UPDATE quote_section SET section = @v0 WHERE id = @v1 LIMIT 1", detail_name.Substring(0, len).Trim(), section_id);
											}
										}
									}
								}
							}
							last_modified = bllToolbox.doSQL_datetime(@"SELECT last_saved last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", quote_id, revision);
							ticks = last_modified.Ticks;
							extraData = ticks.ToString();
							break;
						case "do_revision":
							var this_rev = bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(revision), 0) FROM quote_master WHERE quote_id = @v0 AND status_id != 9", quote_id);
							if (this_rev != 0)
							{
								var wo_count = bllToolbox.doSQL_int(@"SELECT COUNT(*)   FROM   woprog WHERE woprog_quoteid = CONCAT(@p0,@p1)", quote_id, revision);
								if (wo_count > 0)
								{
									return new DataExtra("This quote is attached with a work order, it can not be revised.", null);
								}
								to_history(quote_id, revision, "New Revision Made");
								var result = bllToolbox.doSQL_string(@"CALL RevisionQuote(@v0, @v1)", quote_id, revision);
								if(int.TryParse(result, out this_rev))
								{
								try
								{
									nesi.core.quote.copy_worksheet_file_to_new_rev(quote_id.ToString(), revision.ToString(), this_rev.ToString());
								}
								catch
								{
									// ignored
								}
								bllToolbox.doSQL_void(@"UPDATE quote_master SET why_revised = @v2 WHERE quote_id = @v0 AND revision = @v1 ", quote_id, this_rev, v);
								extraData = this_rev.ToString();
								}
								else
								{
									extraData = result;
								}

							}
							else
							{
								resp = "There isn't an active version to revise.";
							}
							break;
						case "opportunity_internal_id":
							fields = $@"opportunity_internal_id = '{(string.IsNullOrWhiteSpace(v) ? "0" :v)}'";
							break;
						case "parallel_bid":
							fields = $@"parallel_bid = '{v}'";
							break;
						case "bdm":
							fields = $@"bdm = '{v}'";
							break;
						case "acting_bdm":
							fields = $@"acting_bdm = '{v}'";
							break;
					}
				}
				if (do_save && fields != "")
				{
					try
					{
						bllToolbox.doSQL_void(
							$@"UPDATE quote_master SET {fields} WHERE quote_id = '{quote_id}' AND revision = '{revision}' LIMIT 1");
						last_modified = bllToolbox.doSQL_datetime(@"SELECT last_saved last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", quote_id, revision);
						ticks = last_modified.Ticks;
						extraData = ticks.ToString();
					}
					catch (Exception ee)
					{
						resp = ee.ToString();
					}

				}
			}
			return new DataExtra()
			{
				Data = resp == "" ? "Successful" : resp,
				Extra = extraData
			};
		}

        private bool IsQuoteIntegratedToNs(int quote_id, int revision)
        {
            var netSuitId =
                bllToolbox.doSQL_string(
                    @"SELECT netsuite_estimate_internal_id FROM quote_master WHERE quote_id = @v0 AND revision =@v1 LIMIT 1",
                    quote_id, revision);
            var isQuoteIntegratedToNs = !string.IsNullOrWhiteSpace(netSuitId) && netSuitId != "NULL";
            return isQuoteIntegratedToNs;
        }

        public void to_history(int quote_id, int revision, string _event)
		{
			bllToolbox.doSQL_void(@"INSERT INTO quote_history (create_datetime, created_by, quote_id, revision, event) 
VALUES (now(), @v2, @v0, @v1, @v3)", quote_id, revision, current_user.id, _event);
		}

		public DTO.ViewModels.Page.Quotes.QuoteEditCustomerContactInfo GetContactInfo(int contact_id)
		{
			var _contact = new NEContact(Convert.ToInt32(contact_id));
			return new QuoteEditCustomerContactInfo()
			{
				contact_cellPhone = _contact.cellphone,
				contact_email = _contact.Contact_Email,
				contact_extension = _contact.Contact_Extension
			};
		}


		public DTO.ViewModels.Page.Quotes.QuoteDetail[] GetQuoteDiv(string quoteid, string revision, int which)
		{
			_quote.quote_id = quoteid;
			_quote.revision = revision;
			return _quote.divs(which);
		}


		public DataExtra SaveNewDetails(int typeId, LabelValueInt[] models)
		{
			var list = new List<DTO.ViewModels.Page.Quotes.QuoteDetail>();
			foreach (var model in models)
			{
				list.Add(_quote.new_extratext(typeId, model.Label));
			}
			;
			return
				new DataExtra()
				{
					Data = "Saved successfully.",
					Extra = re_order(null, typeId).Extra
				};
		}


        public LabelValueInt[] GetOpportunities(int customerId)
        {
            string sql = @"SELECT DISTINCT

            op.opportunity_internal_id VALUE,
                CONCAT('#',
                op.opportunity_number,
                ' ',
                op.title,
                ' | PT: $',
                op.projected_total) Label
                FROM
                neintranet.ns_opportunity op
            JOIN
                customer c ON c.netsuite_internal_id = op.customer_internal_id
            WHERE
            c.customer_id = @p0 
            ORDER BY Label";

           return _db.Database.SqlQuery<LabelValueInt>(sql, customerId).ToArray();
        }
    }
}