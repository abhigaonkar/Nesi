using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using nesi.core;
using NESI.BLL.Base;
using NESI.Common.Models;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Quotes;
using Label = System.Reflection.Emit.Label;

namespace NESI.BLL.Pages.Quotes
{
	public class NeQuote : BLLBase
	{

		#region Variable Assignment

		public new string VisibleBusinessUnit { get; set; }

		readonly Toolbox _tools = new Toolbox();
		public MySqlConnection conn { get; set; }
		private string sql;
		public readonly Dictionary<string, string> field_titles = new Dictionary<string, string>();
		public string this_string;
		public bool this_bool;

		public NeMember this_branch_manager;
		public NEAddress _address = new NEAddress();
		public NEContact _contact = new NEContact();
		public NECustomer _customer = new NECustomer();
		public NeMember this_member;
		public NeBusinessUnit this_company;

		public LabelValueInt[] contact_select;
		public LabelValueInt[] pricetype_select;
        public LabelValueInt[] revenueLines;
        public bool show_help = false;
		public bool follow_up = false;
        public bool is_tm = false;
		public string show_help_checked = "";
		public string col_type = "";
		public string col_name = "";
		public string new_value = "";
		public string current_quoter = "";
		public string business_unit_id = "";
        public string RevenueLine_Id = "";
        public string quote_id = "";
		public string revision = "1";
		public string customer_id = "NULL";
		public string customer_name = "";
		public string competitor_id = "";
		public string status = "";
		public string competitor_name = "";
		public string contact_id = "NULL";
		public string open_date = "NULL";
		public string completion_date = "NULL";
		public string date_due = "NULL";
		public string date_expected_start = "NULL";
		public string exp_podate = "NULL";

		public string job_description = "";
		public string cust_spec_doc = "";
		public string po = "";
		public string wo = "";
		public string pct_chance_reason = "0";
		public string pct_chance_note = "";
		public string include_title = "";
		public string wo_link = "";
		public string po_link = "";
		public string quoted_by = "NULL";
		public bool quoter_locked_bool = false;
		public string active_revision = "";
		public string quoter_locked_str = "";
		public string quoted_business_unit_id = "";
		public string quoted_business_unit_id_locked = "";
		public string last_print_date = "NULL";
		public string last_print_date_printable = "NULL";
		public string address_id = "";
		public string last_fax_date = "NULL";
		public string verified_date = "NULL";
		public string hours_spent = "";
		public string dollars_spent = "";
		public string quoted_price = "NULL";
		public string price_to = "NULL";
		public string status_id = "NULL";
		public string status_health = "good_health";
		public string status_title = "The connection to the server is good";
		public string prev_status_id = "NULL";
		public string pricetype_id = "NULL";
		public string takeoff_price = "";
		public string tm_pricing = "0";
		public string percent_down = "NULL";
		public string net_due = "4";
		public string custom_term = "";
		public int pct_chance = 0;
		public string us_currency = "";
		public string inflation_term = "";
		public string quote_status_button = "";
		public string why_lose = "";
		public string who_competitor = "";
		public string what_price = "";
		public string post_mortem_button = "";
		public double expected_value = 0;
		public bool strategy_tab = false;
		public bool stage1_approved = false;
		public bool stage4_approved = false;
		public TextInfo textinfo;
		public string print_buttons = "";
		public DataTable all_reports;
		public string reports_list = "";
		public string[] details;
		public string[] notes;
		public string[] reasons;
		public string quick_pick_link = "";
		public string checkmemberid = "";
		public long ts_ticks = 0;
		public int currency = 1;
		public string period_valid = "NULL";
        public string netsuite_estimate_internal_id = "NULL";
		public int opportunity_internal_id = 0;
		public bool parallel_bid = false;
		public int bdm = 0;
		public int acting_bdm= 0;

		#endregion Variable Assignment
		#region [VOID] Methods (12)
		#region PRIVATE (3)
		private void get_customer_contactinfo()
		{
			if (customer_id != "")
			{
				_customer = new NECustomer(Convert.ToInt32(customer_id));
				_address = address_id != "" ? new NEAddress(Convert.ToInt32(address_id)) : _customer.Address;
				if (contact_id != "" && contact_id != "NULL")
				{
					_contact = new NEContact(Convert.ToInt32(contact_id));
				}
			}
		}
		private string get_customer_name(string id)
		{
			return Toolbox.doSQL_string(conn, @"SELECT IFNULL(MAX(customer_name), '') customer_name FROM customer  WHERE customer_id =@v0", new object[] { id });
		}
		private void set_show_help()
		{
			show_help = Convert.ToBoolean(Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(show_quote_help), '0') FROM member WHERE member_id = @v0 ", new object[] { this_member.id }));
			if (show_help)
			{
				show_help_checked = "checked";
			}
			else
			{
				show_help_checked = "";
			}
		}
		#endregion PRIVATE
		#region PUBLIC (9)
		public void get_contact_select()
		{
			if (customer_id != "" && customer_id != "NULL")
			{
				contact_select = get_Contacts(customer_id, address_id);
			}
		}

        public void get_revenueLines()
        {
            revenueLines=bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT Distinct b.revenue_line_id as value, b.Name as label FROM revenue_line_business_unit a LEFT JOIN revenue_line b ON a.revenue_line_id = b.revenue_line_id WHERE business_unit_id =@v0", this.business_unit_id );         
        }

        public void get_BranchManager()
		{
			this_branch_manager = this_company.branch_manager; ;
		}
		public void to_history(string _event)
		{
			_tools.getSQL_bool(@"INSERT INTO quote_history (create_datetime, created_by, quote_id, revision, event) VALUES (now(), @v2 , @v0 , @v1 , @v3 )", new object[] { quote_id, revision, current_quoter, _event });
		}
		public string set_active_revision()
		{
			// Check if linked to WO...
			var current_rev = Toolbox.doSQL_string(conn, @"SELECT revision FROM quote_master WHERE quote_id = @v0  AND active_revision = true", new object[] { quote_id });
			var c = Toolbox.doSQL_int(conn, @"SELECT COUNT(woprog_id) FROM woprog WHERE woprog_quoteid = @v0", new object[] { quote_id + current_rev });
			if (c == 0)
			{
				Toolbox.doSQL_void(conn, @"UPDATE quote_master SET active_revision = false, prev_status_id = status_id, status_id = 9 WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, current_rev });

				Toolbox.doSQL_void(conn, @"UPDATE quote_master SET active_revision = true, status_id = prev_status_id  WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
				//Toolbox.doSQL_void(conn, @"UPDATE quote_master SET active_revision = false WHERE quote_id = @v0  AND revision != @v1 ", new object[] { quote_id, revision });
				//Toolbox.doSQL_void(conn, @"UPDATE quote_master SET active_revision = true  WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
			}
			else if (c == 1)
			{

				var woprog_bvwo = Toolbox.doSQL_string(conn, @"SELECT woprog_bvwo FROM woprog WHERE woprog_quoteid = @v0 ", new object[] { quote_id + current_rev });
				return (
					$"This quote/revision is currently linked to work order '{woprog_bvwo}', you must first unlink the quote before you can change the active revision.");
			}
			return "Success.";
		}
		public void get_pricetype_select()
		{
			pricetype_select = bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT id value, type label FROM quote_pricetype where active=true");
		}

		public void InitializeQuote()
		{
			sql = string.Format(@"
SELECT
	a.customer_id,
	a.open_date,
	date_format(a.completion_date, '%Y-%m-%d') completion_date,
	date_format(a.date_due, '%Y-%m-%d') date_due,
	date_format(a.date_expected_start, '%Y-%m-%d') date_expected_start,
date_format(a.exp_podate, '%Y-%m-%d') exp_podate,
	a.active_revision,
	a.job_description,
	a.cust_spec_doc,
	a.quoted_by,
	a.quoter_locked,
	a.business_unit_id,
    a.revenue_line_id,
	a.status_id,
	b.status,
	CAST(IFNULL(a.competitor_id, 0) AS CHAR) competitor_id,
	a.prev_status_id,
	a.contact_id,
	DATE_FORMAT(a.last_print_date, '%Y-%m-%d %H:%i:%s') last_print_date,
	DATE_FORMAT(a.last_print_date, '%W, %M %D %Y') last_print_date_printable,
	DATE_FORMAT(a.last_fax_date, '%Y-%m-%d %H:%i:%s')last_fax_date,
	DATE_FORMAT(a.verified_date, '%Y-%m-%d %H:%i:%s')verified_date,
	a.quoted_price,
	a.price_to,
	a.revision,
	a.custom_term,
	a.net_due,
	IFNULL(a.address_id, 0) address_id,
	a.percentage_down,
	CAST(IFNULL(a.us_currency, 'NULL') AS CHAR) us_currency,
	a.inflation_term,
	a.pricetype_id,
	a.po,
	a.wo,
	IFNULL(a.pct_chance, 0) pct_chance,
	IFNULL(a.pct_chance_reason,0) pct_chance_reason,
	IFNULL(c.woprog_project_notes_notes,'') pct_chance_note,
	a.what_price,
	a.who_competitor,
	a.why_lose,
	a.include_title,
	a.expected_value,
	a.allowed_to_quote,
	a.allowed_to_quote_2,
	IFNULL(a.follow_up, 0) follow_up,
    IFNULL(a.is_tm, 0) is_tm,
	a.last_saved last_modified,
	a.currency,
	a.period_valid,
    a.netsuite_estimate_internal_id,
	a.opportunity_internal_id,
	IFNULL(a.parallel_bid, 0) parallel_bid,
    IFNULL(a.bdm, 0) bdm,
    IFNULL(a.acting_bdm, 0) acting_bdm
FROM
	quote_master a 
LEFT JOIN
	quote_status b ON a.status_id = b.id
LEFT JOIN
	woprog_project_notes c ON c.woprog_project_notes_woprogid = a.quote_id AND c.woprog_project_notes_Type = 'Q'
WHERE 
	a.quote_id = @v0 AND a.revision =@v1 ");
			var _dt = Toolbox.doSQL_dt(conn, sql, new object[] { quote_id, revision });
			if (_dt.Rows.Count > 0)
			{
				get_field_titles();
				foreach (DataRow _dr in _dt.Rows)
				{
					customer_id = _dr["customer_id"].ToString();
					contact_id = _dr["contact_id"].ToString();
					if (customer_id != "" && customer_id != "NULL")
					{
						customer_name = get_customer_name(customer_id);
					}
					if (_dr["competitor_id"].ToString() != "0")
					{
						competitor_id = _dr["competitor_id"].ToString();
						competitor_name = set_competitor_name();
					}
					open_date = _dr["open_date"].ToString();
					date_due = _dr["date_due"].ToString();
					date_expected_start = _dr["date_expected_start"].ToString();
					exp_podate = _dr["exp_podate"].ToString();
					if (date_due == "" || date_due == "NULL")
					{
						date_due = DateTime.Now.ToString("yyyy-MM-dd");
					}
					completion_date = _dr["completion_date"].ToString();
					job_description = _dr["job_description"].ToString();
					cust_spec_doc = _dr["cust_spec_doc"].ToString();
					quoted_by = _dr["quoted_by"].ToString();
					if (quoted_by != "")
					{
						all_reports = NeMember.get_allreports(Convert.ToInt32(quoted_by));
					}
					pct_chance = Convert.ToInt32(_dr["pct_chance"]);
					pct_chance_reason = _dr["pct_chance_reason"].ToString();
					pct_chance_note = _dr["pct_chance_note"].ToString();
					why_lose = _dr["why_lose"].ToString();
					what_price = _dr["what_price"].ToString();
					who_competitor = _dr["who_competitor"].ToString();
					expected_value = (double)_dr["expected_value"];
					quoter_locked_bool = Convert.ToBoolean(_dr["quoter_locked"]);
					follow_up = Convert.ToBoolean(_dr["follow_up"]);
                    is_tm = Convert.ToBoolean(_dr["is_tm"]);
					date_due = _dr["date_due"].ToString();
					date_expected_start = _dr["date_expected_start"].ToString();
					stage1_approved = _dr["allowed_to_quote"] != DBNull.Value && Convert.ToBoolean(_dr["allowed_to_quote"]);
					stage4_approved = _dr["allowed_to_quote_2"] != DBNull.Value && Convert.ToBoolean(_dr["allowed_to_quote_2"]);
                    RevenueLine_Id = _dr["revenue_line_id"].ToString();

                    if (_dr["business_unit_id"].ToString() != "" && _dr["business_unit_id"].ToString() != "NULL")
					{
						quoted_business_unit_id = _dr["business_unit_id"].ToString();
                       
                        business_unit_id = quoted_business_unit_id;
						if (quoted_by != "" && quoter_locked_bool)
						{
							quoted_business_unit_id_locked = "0";
						}
						else
						{
							quoted_business_unit_id_locked = quoted_business_unit_id;
						}
					}
					else
					{
						quoted_business_unit_id = business_unit_id;
						quoted_business_unit_id_locked = "0";
					}
					po = _dr["po"].ToString().Trim();
					wo = _dr["wo"].ToString().Trim();
					if (po == "" || po == "0")
					{
						po = "N/A";
					}
					if (wo == "" || wo == "0")
					{
						wo = "N/A";
					}
					if (_dr["last_modified"] == DBNull.Value)
					{
						Toolbox.doSQL_void(conn, @"UPDATE quote_master SET last_saved = NOW() WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
						var last_modified = Toolbox.doSQL_datetime(@"SELECT last_saved as last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
						ts_ticks = last_modified.Ticks;
					}
					else
					{
						ts_ticks = Convert.ToDateTime(_dr["last_modified"]).Ticks;
					}
					this_member = new NeMember(Convert.ToInt32(current_quoter));
					checkmemberid = this_member.id.ToString();
					address_id = _dr["address_id"].ToString();
					if (address_id == "0")
					{
						address_id = Toolbox.doSQL_string(conn, @"SELECT IFNULL(MAX(address_id),0) FROM address  WHERE address_table = 'Customer' AND address_type = 'B' AND address_table_id =@v0", new object[] { customer_id });
						Toolbox.doSQL_void(conn, @"UPDATE quote_master SET address_id = @v0  WHERE quote_id = @v1  AND revision = @v2 ", new object[] { address_id, quote_id, revision });
					}
					get_contact_select();
                    get_revenueLines();
                    get_customer_contactinfo();
					this_company = new NeBusinessUnit(quoted_business_unit_id);
					if (this_member.AuthenticatedForPrivilege(61))
					{
						var locked_str = quoter_locked_bool ? "checked" : "";
						quoter_locked_str =
							$"<input type='checkbox' data-is_new_quote='false' tabindex='14' height='15px' onchange='toggle_lock_quoted_by(this)' id='lock_quoter' {locked_str}/><img hspace='5' src='/images/icon/icon[lock].gif' width='16' height='16'/>";
					}
					get_BranchManager();
					currency = Toolbox.ReturnZeroIfNull_int(_dr["currency"]);
					revision = _dr["revision"].ToString();
					contact_id = _dr["contact_id"].ToString();
					last_print_date = _dr["last_print_date"].ToString();
					last_print_date_printable = _dr["last_print_date_printable"].ToString();
					if (last_print_date == "0000-00-00 00:00:00")
					{
						last_print_date = "";
					}
					last_fax_date = _dr["last_fax_date"].ToString();
					if (last_fax_date == "0000-00-00 00:00:00")
					{
						last_fax_date = "";
					}
					verified_date = _dr["verified_date"].ToString();
					if (verified_date == "0000-00-00 00:00:00")
					{
						verified_date = "";
					}
					status_id = _dr["status_id"].ToString();
					status = _dr["status"].ToString();
					if (status_id == "9")
					{
						status_health = "locked";
						status_title = "Quote is dead. Read Only access allowed.";
					}
					else if (status_id == "8")
					{
						status_health = "locked";
						status_title = "Quote has been referenced on a work order and is now read only.";
					}
					prev_status_id = _dr["prev_status_id"].ToString();
					if (status_id == "6")
					{
						quote_status_button = this_company.uses_quote_process != 1 ? "<button type='button' onclick='status_toggle(this);'><img src='/images/icon/icon[alive].gif' width='16' height='16' id='toggleimg' align='absmiddle' /><div id='toggletext'>It's Alive</div></button>" : "";
					}
					else
					{
						//if (this_company.uses_quote_process != 1)
						//{
						quote_status_button = "<button type='button' onclick='status_toggle(this);'><img src='/images/icon/icon[dead].gif' width='16' height='16'  id='toggleimg' align='absmiddle' /><div id='toggletext'>Kill Quote</div></button>";
						//}
						//else
						//{
						//	if (expected_value >= this_company.quote_level_2_start)
						//	{
						//		quote_status_button = "<button style='font: bold 9px Arial' type='button'><img src='/images/icon/icon[help].gif' width='16' height='16'  id='toggleimg' align='absmiddle' /><div id='toggletext2'>Go to Strategy Page to Kill the Quote</div></button>";
						//	}
						//	else
						//	{
						//		quote_status_button = "<button type='button' onclick='status_toggle(this);'><img src='/images/icon/icon[dead].gif' width='16' height='16'  id='toggleimg' align='absmiddle' /><div id='toggletext'>Kill Quote</div></button>";
						//	}
						//}
					}
					var print_button_client_validation_base = @" onclick=""
				if( 
				$('#completion_date').val() != '' && 
				$('#date_due').val() != '' && 
				$('#status').text() != 'Dead Quote'
					)
					{{
					save_quote();
					print_quote('$$');
					$('#print_pane').dialog('open');

					}}
				else
					{{
					if($('#completion_date').val() == '' || $('#date_due').val() == '')
						{{
						alert('You must fill out the completion & due dates before printing.');
						}}
					else
						{{
						alert('You cannot print a dead quote');
						}}
					}}"" ".Replace("\n", "").Replace("\t", "");
					// Matt: As every instance of the if statement that used to be here produced the same buttons, I simplified it.
					var print_button_w = @"<td id='option_print_w'><button type='button' " + print_button_client_validation_base.Replace("$$", "w") + @" ><img width='16' height='16' src='/images/icon/icon[print].gif'/> <br/>Print w/ Price</button></td>";
					var print_button_wo = @"<td id='option_print_wo'><button type='button'  " + print_button_client_validation_base.Replace("$$", "wo") + @" id='opt_print_wo'><img width='16' height='16' src='/images/icon/icon[print].gif'/> <br/>Print w/o Price</button></td>";
					var print_button_draft = Convert.ToInt32(status_id) >= 3 && !Toolbox.Contains(status_id, new[] { "6", "10", "11", "12" }) ? "<td>&nbsp;</td>" : @"<td id='option_print_draft'><button type='button' onclick=""print_quote('draft');$('#print_pane').dialog('open');""><img width='16' height='16' src='/images/icon/icon[print].gif'/> <br/>Print Draft</button></td>";
					print_buttons = print_button_w + print_button_wo + print_button_draft;

					if (wo != "N/A")
					{
						var woprog_id = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(woprog_id), 0) FROM woprog  WHERE woprog_bvwo =@v0", new object[] { wo.PadLeft(10, '0') });
						if (woprog_id != 0)
						{
							var woprog = new NeWOProg(woprog_id);
							wo_link = $@"/sections/workorder/index.aspx?woprog_id={woprog.woprog_id}&business_unit_id={woprog.business_unit_id}";
						}
					}
					quoted_price = _dr["quoted_price"].ToString();
					price_to = _dr["price_to"].ToString();
					custom_term = Toolbox.do_value_from(_dr["custom_term"]);
					net_due = _dr["net_due"].ToString();
					percent_down = _dr["percentage_down"].ToString();
					pricetype_id = _dr["pricetype_id"].ToString();
					period_valid = _dr["period_valid"].ToString();
					opportunity_internal_id = Convert.ToInt32(_dr["opportunity_internal_id"]);
				
					parallel_bid = Convert.ToBoolean(_dr["parallel_bid"]);
					bdm = Convert.ToInt32(_dr["bdm"]);
					acting_bdm = Convert.ToInt32(_dr["acting_bdm"]);
					netsuite_estimate_internal_id = _dr["netsuite_estimate_internal_id"].ToString();

					if (pricetype_id == "0")
					{
						pricetype_id = "";
					}
					if (_dr["us_currency"].ToString() == "1")
					{
						us_currency = "checked";
						currency = 1;
					}
					else
					{
						us_currency = "" + _dr["us_currency"].ToString();
						currency = 2;
					}
					active_revision = _dr["active_revision"].ToString() == "0" ? "" : "1";

					if (_dr["inflation_term"].ToString() == "1")
					{
						inflation_term = "checked";
					}
					else if (_dr["inflation_term"].ToString() == "0")
					{
						inflation_term = "";
					}
					include_title = _dr["include_title"].ToString();
					get_pricetype_select();
					set_tm_pricing();
					set_show_help();

					var comp = new NeBusinessUnit(this_company.id);
					if (comp.uses_quote_process == 1 && expected_value >= comp.quote_level_2_start)
					{
						strategy_tab = true;
					}

				}
			}
		}
		public void get_field_titles()
		{
			try
			{
				var _temp_dt = Toolbox.doSQL_dt(conn, @"SELECT field, title FROM quote_field_title", null);
				foreach (DataRow _temp_dr in _temp_dt.Rows)
				{
					field_titles.Add(_temp_dr["field"].ToString(), _temp_dr["title"].ToString());
				}
			}
			catch (Exception ee)
			{
				_tools.catch_error(ee);
			}
		}
		public void set_tm_pricing()
		{
			try
			{
				tm_pricing = tm_pricing_total().ToString("c2");
			}
			catch
			{
				tm_pricing = "$0.00";
			}
		}
		public void StartQuote()
		{
			quote_id = Toolbox.doSQL_string(conn, @"CALL StartQuote_beta(@v0 , @v1 , @v2 , @v3 , @v4 , @v5, @v6 )", new object[] { quoted_by, customer_id, contact_id, date_due, date_expected_start, job_description, business_unit_id });
			if (quoter_locked_bool)
			{
				Toolbox.doSQL_void(conn, @"UPDATE quote_master SET quoter_locked = true WHERE quote_id = @v0  AND revision = '1' LIMIT 1", new object[] { quote_id });
			}
			Toolbox.doSQL_void(conn, @"UPDATE quote_master SET expected_value = @v0 , completion_date = @v1  WHERE quote_id = @v2  AND active_revision = TRUE LIMIT 1", new object[] { quoted_price, completion_date, quote_id });
		}
		#endregion PUBLIC
		#endregion
		#region [STRING] Methods (25)
		public string company_list(object _business_unit_id, bool search_pane)
		{
			var buid = _business_unit_id.ToString();
			var this_select = new StringBuilder();
			if (search_pane)
			{
				this_select.Append(@"<select onchange=""if(this.value != '0'){fill_quoters(this)}"" class='subquotedby_company'>");
			}
			else
			{
				this_select.AppendFormat(@"<select id='quoted_company' tabindex='11' onchange=""quote_obj.s('s_company', this);"">");
			}
			var _companys = Toolbox.doSQL_dt(conn, @"SELECT id, ddl_name name from business_unit  WHERE enable_timesheet = 1", null);
			if (_companys.Rows.Count > 0)
			{
				foreach (DataRow row in _companys.Rows)
				{
					var this_id = row["id"].ToString();
					var this_name = row["name"].ToString();
					var this_selected = buid == this_id ? " selected" : "";
					this_select.AppendFormat("<option value='{0}'{2}>{1}</option>", this_id, this_name, this_selected);
				}
			}
			else
			{
				this_select.Append("<option value='0'>NO COMPANIES TO PICK FROM</option>");
			}
			this_select.Append("</select>");
			return this_select.ToString();
		}

		public DataTable divisions_dt()
		{
			return Toolbox.doSQL_dt(conn, @"SELECT
    a.id,
    a.ddl_name NAME
FROM
    business_unit a
WHERE a.tax_entity_id = (SELECT tax_entity_id FROM business_unit WHERE id = @v0)", new object[] { quoted_business_unit_id });
		}
		public string competitor_id_from_name(string competitorname)
		{
			this_string = Toolbox.doSQL_string(conn, @"SELECT id FROM quote_competitor  WHERE name = @v0", new object[] { competitorname });
			return this_string;
		}
		public string Container()
		{
			var price_to_box = "";
			var hide_me = "";
			var received_button_disabled = "";



			if (status_id != "4" && status_id != "8")
			{
				received_button_disabled = "disabled";
			}
			if (price_to == "NULL" || price_to == "")
			{
				price_to_box = "<input type='text' id='price_to' onchange='priceto(this);' value='' style='display:none;' class='price'>";
			}
			else
			{
				if (pricetype_id != "6")
				{
					hide_me = "style='display:none;'";
				}

				price_to_box =
					$"<input type='text' onchange='priceto(this);' id='price_to' value='{price_to}' {hide_me} class='price'>";
			}

			po_link = po == "N/A" ? "" : $@"{po}";
			if (wo != "N/A")
			{
				var woprog_id = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(woprog_id), 0) FROM woprog  WHERE woprog_bvwo =@v0", new object[] { wo.PadLeft(10, '0') });
				if (woprog_id != 0)
				{
					var woprog = new NeWOProg(woprog_id);
					wo_link = $@"/sections/workorder/index.aspx?woprog_id={woprog.woprog_id}&business_unit_id={woprog.business_unit_id}";
				}
			}

			var expected_value_update = expected_value == 0 ? "<input type='text' id='new_expected_value' value=''  onkeydown='only_numeric(event)'/><div style='font-size:10px;'>Please update the expected value of this quote</div>" : expected_value.ToString("C2");
			var worksheet_tab = this_member.AuthenticatedForPrivilege(163)
				? @"
					<td class='h' data-title='Worksheet Tab'>
						<button type='button' class='normal' onclick='javascript:boing(""/sections/member/picklist/pikclist.aspx?id=" + quote_id + "&rev=" + revision + @"&origin=quote"", ""WorkSheetPickList" + quote_id + @""", 1024, 800);' id='tab_worksheet'>Worksheet</button>
						<div class='tip'>" + field_title("worksheet") + @"</div>
					</td>"
				: "";
			var why_revised = Convert.ToInt32(revision) == 1 ? "" : Toolbox.doSQL_string(conn, @"SELECT why_revised FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
			var why_revised_html = why_revised != "" ? $"<img src='/images/icon/icon[note].gif' width='14' height='18' align='absmiddle' style='margin:0px 3px 0px 3px;' title='{HttpUtility.HtmlEncode(why_revised)}' />"
				: "";
			this_string = string.Format(@"
	<div id='quote_information'>
		<input id='this_quote_id' type='hidden' value='{1}'>
		<input id='status_id' type='hidden' value='{35}'>
		<input id='prev_status_id' type='hidden' value='{36}'>
		<div id='quote_id' class='c' data-title='Quote Number Information'> QUOTE #:<input type='text' value='{1}' tabindex='1' data-title='Quote'/>
			<select onchange=""location.href='./index.aspx?a=g&quote_id={1}&revision='+this.value""  tabindex='2'>
				{2}
			</select>{87}
			<input type='checkbox' style='width:auto;' onclick='set_active_revision(this);' title='Make this version the active version' {69}/>
			<button id='revision_quote' tabindex='3' onclick=""quote_obj.revision.show({1}, {37})"" type='button'>New Version</button>
			<div class='tip'>" + field_title("quote") + @"</div>
		</div>
		<!-- <span id='quote_help' onclick='show_help(this)'><input type='checkbox' id='show_help' {68}  disabled/> <u>show help?</u></span> -->
		<div id='saved_message'>Saved</div>
		<!-- <img align='absmiddle' id='button_save' onclick=""save_quote(true, false)"" src='/images/quote/button/button[save].png' /> -->
		<img align='absmiddle' id='button_saveclose' style='display:none;' onclick=""save_quote(true, true)""  src='/images/quote/button/button[saveclose].png'>
		<button type='button' id='button_search' onclick=""$('#search_pane').dialog('open');"">Search</button>
		<img src='/images/quote/button/button[back].png' style='display:none;' id='back_button' title='Clicking this will not save your quote.' tabindex='99' onmouseup=""location.href='./frame.aspx'"" />
		<span id='quote_connection'>Connection Status<img src='/images/quote/decal/decal[{63}].gif' title='{64}' align='absmiddle' width='16' height='16' />
		</span>
		<div id='quote_tabs' align='left'>
			<table cellspacing='0' cellpadding='0'>
				<tr>
					<td class='h' data-title='General Tab'>
						<button type='button' class='active' onclick=""tab_control(this, 'general');"" id='tab_general'  disabled>General</button>
						<div class='tip'>" + field_title("general") + @"</div>
					</td>
					<td class='h' data-title='Details Tab'>
						<button type='button' class='normal' onclick=""tab_control(this, 'detail');"" id='tab_detail'>Scope of Work</button>
						<div class='tip'>" + field_title("details") + @"</div>
					</td>
					<td class='h' data-title='Notes / Adders Tab'>
						<button type='button' class='normal' onclick=""tab_control(this, 'notes');"" id='tab_notes'>Notes/Adders</button>
						<div class='tip'>" + field_title("notes_adders") + @"</div>
					</td>
					{83}
					<td class='h' data-title='Maintenance Tab'>
						<button type='button' class='normal' onclick=""tab_control(this, 'maintenance');load_followup_history();"" id='tab_maintenance'>Maintenance</button>
						<div class='tip'>" + field_title("maintenance") + @"</div>
					</td>
					<td class='h' data-title='Folder Tab' style='width:35px'>
						<button type='button' class='normal' onclick=""tab_control(this, 'folder');"" id='tab_folder'>Storage</button>
						<div class='tip'>" + field_title("folder") + @"</div>
					</td>
					{78}
					</td>
					<td class='h' data-title='ER Product Tab'>
						<button type='button' class='normal' onclick=""tab_control(this, 'er');"" style='display:none;' id='tab_er'>ER Product</button>
						<div class='tip'>" + field_title("er") + @"</div>
					</td>
					<td class='c' align='center' style='width:150px;' data-title='Total Benchmark Sell(Ext`d)'>
						<div style='font-size:9px;'>T&M Price</div>
						<div style=""width:150px;height:19px;display:block;text-align:center;"" id='tm_price'>{41:c2}</div>
					</td>
					<td class='c' align='center' data-title='Total Quoted (~Ext`d)'>
						<div style='font-size:9px;'>Worksheet Total</div>
						<div style=""width:120px;height:19px;display:block;font-size:11px;text-align:center;"" id='worksheet_price'>{67:c2}</div>
					</td>
				</tr>
			</table>
		</div>
		<div id='quote_general'>
						<table cellpadding='0' cellspacing='0' width='100%'>
							<tr>
								<td class='c' data-title='Customer'>
									Customer:
									<div class='tip'>" + field_title("customer") + @"</div>
								</td>
								<td class='v'><input type='text' id='customer' data-isac='false'  onfocus=""attach_ac(this, 'customer');"" data-id='{3}' tabindex='4' onblur=""quote_obj.s('s_customer', this);"" data-ov=""{3}"" class='customer' value=""{22}""/></td>
<td class='v' >
<table width='100%' cellspacing='0' cellpadding='0'>
<tr>								
<td><img align='absmiddle' src='/images/pixel.gif' title='' height='16' width='16' id='is_QCed' /></td>
								<td class='v'><img align='absmiddle' src='/images/pixel.gif' title='' id='is_active' height='16' width='34' /></td>
								</tr>
</table>
</td>
								<td rowspan='28' align='center' valign='top' class='r'>
									<div class='address_info'>
										<div class='header'>
											<button type='button' title='Click me to edit this information' onclick='customer_edit(this);'> <img src='/images/icon/icon[edit].gif' width='16' height='16' align='absmiddle' /> Edit Contact / Customer Info</button>
										</div>
										<div class='address_selector'>
											{74}
										</div>
									</div>
									<div class='customer_info'>
										<div class='pane'>
											<div class='_head_cu'><img src='/images/icon/icon[company].gif' align='absmiddle' width='16' height='16' /> CUSTOMER ADDRESS</div>
											<div id='_address'>
												<div id='_address_1'>{28}</div>
												<div id='_address_2'>{49}</div>
												<div id='_address_3'>{50}</div>
												<div id='_address_4'>{51}</div>
												<div id='region_info'>
													<span id='_city'>{52}</span> <span id='_state'>{53}</span>, <span id='_postal'>{54}</span>
												</div>
												<div id='_country'>{55}</div>
											</div>
										</div>
										<div class='pane'>
											<div class='_head_cu'><img src='/images/icon/icon[handset].gif' align='absmiddle' width='16' height='16' /> CUSTOMER PHONE</div>
											<div class='_phone'><span id='_phone'>{29}</span> x<span id='_extension'>{57}</span></div>
										</div>
										<div class='pane'>
											<div class='_head_cu'><img src='/images/icon/icon[fax].gif' align='absmiddle' width='16' height='16' /> CUSTOMER FAX</div>
											<div class='_phone' id='_fax'>{30}</div>
										</div>
									</div>
									<div class='contact_info'>
										<div class='pane'>
											<div class='_head_co'><img src='/images/icon/icon[mobile].gif' align='absmiddle' width='16' height='16' /> CONTACT MOBILE #</div>
											<div class='_phone' id='_mobile'>{56}</div>
										</div>
										<div class='pane'>
											<div class='_head_co'><img src='/images/icon/icon[email].gif' align='absmiddle' width='16' height='16' /> CONTACT EMAIL</div>
											<div class='_email' id='_email'><a href='mailto:{31}'>{31}</a></div>
										</div>
										<div class='pane'>
											<div class='_head_co'><img src='/images/icon/icon[member].gif' align='absmiddle' width='16' height='16' /> INCLUDE CONTACT TITLE</div>
											<div class='_email' id='_inc_title'><input type='checkbox' id='include_title'  onchange=""quote_obj.s('s_includetitle', this)""  {71}/> Includes Title on Printout</div>
										</div>
									</div>
									<table cellspacing='0' cellpadding='2' class='options' id='quote_options'>
										<tr>
											<td colspan='3' class='header'><img src='/images/icon/icon[browse].gif' align='absmiddle' width='16' height='16'/> OPTIONS</td>
										</tr>
										<tr>{80}
                                        </tr>
										<tr>
											<td id='option_status'>{34}</td>
											<td id='option_received'><button type='button' onclick=""new_wo();"" id='received_button' {43}><img width='16' height='16' src='/images/icon/icon[ok].gif' width='16' height='16'/> <br/>Work Order</button></td>
											<td id='option_post_mortem'>{79}</td>
										</tr>
										<tr>
											<td id='option_service_report'>{70}</td>
											<td id='option_duplicate'><button type='button'  onclick='duplicate_quote({1}, {37}, this);'><img width='16' height='16' src='/images/icon/icon[copy].gif' width='16' height='16'/> <br/>Duplicate</button></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td class='c' data-title='Contact'>
									Contact:
									<div class='tip'>" + field_title("contact") + @"</div>
								</td>
								<td class='v'>{25}</td>
<td class='v'><button type='button' class='now' title='Add contact to this customer' onclick=""$('#quote_addcontact').dialog('open');""><img src='/images/icon/icon[add].gif'  width='16'  height='16' /></button></td>
							</tr>
							<tr>
								<td class='c' data-title='Open Date'>
									Open Date:
									<div class='tip'>" + field_title("open_date") + @"</div>
								</td>
								<td class='v'><b class='date'>{24}</b></td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='Date Due'>
									Date Due:
									<div class='tip'>" + field_title("date_due") + @"</div>
								</td>
								<td class='v'><input type='text' tabindex='6' class='date' data-ov=""{6}""  onchange=""quote_obj.s('s_datedue', this);"" readonly='readonly' value='{6}' id='date_due'/></td>
<td class='v'></td>
							</tr>
<tr>
								<td class='c' data-title='Expected PO date'>
									Expected PO Date:
									<div class='tip'>" + field_title("exp_podate") + @"</div>
								</td>
								<td class='v'><input type='text' tabindex='7' class='date' data-ov=""{89}""  onchange=""quote_obj.s('s_exp_podate', this);"" readonly='readonly' value='{89}' id='exp_podate'/></td>
<td class='v'></td>
							</tr>
							<tr id='tr_pc'>
								<td class='c' data-title='Percent Chance'>
									% Chance of Getting Job:<div style='font-size:9px;color:#555;'>30% - Budget quote/ Bid Tender<br/>60% - NEW is 1/3 contractors Pricing<br/>90% - NEW is the only contractor pricing</div>
									<div class='tip'>" + field_title("percent_chance") + @"</div>
								</td>
								<td class='v'>{76}</td>
<td class='v'></td>
							</tr>
							<tr id='tr_ec'>
								<td class='c' data-title='Expected Complete Date'>
									Expected Complete Date:
									<div class='tip'>" + field_title("job_complete_date") + @"</div>
								</td>
								<td class='v'><input type='text' tabindex='8' class='date' data-ov=""{60}""  onchange=""quote_obj.s('s_expcompletiondate', this);"" readonly='readonly' value='{60}' id='completion_date'/></td>
<td class='v'></td>							
</tr>

							<tr>
								<td class='c' data-title='Job Description'>
									Job Description:
									<div class='tip'>" + field_title("job_description") + @"</div>
								</td>
								<td class='v'>
									<textarea class='jobdescription' tabindex='9' onchange=""quote_obj.s('s_jobdescription', this);"" data-ov=""{7}"" id='job_description' >{7}</textarea>{75}
								</td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='PM To Follow-up'>
									PM To Follow-Up:
									<div class='tip'>" + field_title("pm_followup") + @"</div>
								</td>
								<td class='v'>
									<input type='checkbox' id='pm_followup' height='15px' value='1' tabindex='9' onchange=""quote_obj.s('s_follow_up', this)"" {84}/>
								</td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='Cust Spec Doc'>
									Cust Spec Doc:
									<div class='tip'>" + field_title("cust_spec_doc") + @"</div>
								</td>
								<td class='v'><input type='text' class='custspecdoc' tabindex='10' onchange=""quote_obj.s('s_custspecdoc', this);"" value='{8}' id='cust_spec_doc'></td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='Company'>
									Branch:
									<div class='tip'>" + field_title("company") + @"</div>
								</td>
								<td class='v'>{45}</td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='Quoted By'>
									Quoted By:
									<div class='tip'>" + field_title("quoted_by") + @"</div>
								</td>
								<td class='v'>{81}</td><td class='v'>{65}</td>
							</tr>
							<tr>
								<td class='c' data-title='Last Print Date'>
									Last Print Date:
									<div class='tip'>" + field_title("last_print_date") + @"</div>
								</td>
<td class='v' id='last_print_date'><b class='date'>{10}</b></td>
<td class='v'><button id='last_print_button' type='button' tabindex='15' data-field='print' class='now' onclick='update_date(this,0);' disabled='disabled'>update</button></td>
</tr>
							<tr>
								<td class='c' data-title='Last Sent Date'>
									Last Sent Date:
									<div class='tip'>" + field_title("last_sent_date") + @"</div>
								</td>

								<td class='v' id='last_fax_date'><b class='date'>{11}</b></td>
<td class='v'><button id='last_fax_button' type='button' tabindex='16' data-field='sent' class='now' onclick='update_date(this,1);' disabled='disabled'>update</button></td>
</tr>
							<tr>
								<td class='c' data-title='Verified Date'>
									Verified Date:
									<div class='tip'>" + field_title("verified_date") + @"</div>
								</td>
								<td class='v' id='verified_date'><b class='date'>{12}</b></td>
<td class='v'><button id='last_verified_button' type='button' tabindex='17' data-field='verified' class='now' onclick='update_date(this,2);' disabled='disabled'>update</button></td>
							
</tr>
							<tr>
								<td class='c' data-title='Status'>
									Status:
									<div class='tip'>" + field_title("status") + @"</div>
								</td>
								<td class='v'><b class='date' id='status'>{73}</b></td>	
<td class='v'></td>
							</tr>
							<tr style='display:none;'>
								<td class='c'><img src='/images/icon/icon[construction].gif' align='absmiddle' width='16' height='16' title='This field has not been completed. Pay no attention to this.'/> Hours Spent:</td>
								<td class='v'><b class='hoursspent' id='hours_spent'>000</b> or: $000.00<b id='dollars_spent' class='moneyspent'></b></td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='Quoted Price'>
									Quoted Price:
									<div class='tip'>" + field_title("quoted_price") + @"</div>
								</td>
								<td class='v'>
										<input type='text' tabindex='18' onchange=""quote_obj.s('s_quotedprice', this);"" onkeydown=""only_numeric(event);"" id='quoted_price' value='{15}' class='price'>
										{16}</td>
										<td class='v'>{42}
								</td>
							</tr>
							<tr>
                                <td class='c'>Expected Value:</td>
								<td class='v'>{77}</td>
<td class='v'><button type='button' onclick='update_exp_val(this)' class='now' style='visibility:{88};'>Update</button></td>
							</tr>
							<tr>
                                <td class='c'>Total Quoted(~Ext`d):</td>
								<td class='v'><b id='worksheet_total_label'>{67:c2}</b></td>
<td class='v'></td>
							</tr>
							<tr>
                                <td class='c'>Total Benchmark Sell(Ext`d):</td>
								<td class='v'><b id='tm_total_label'>{41:c2}</b></td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='Attached to WO #'>
									Attached to WO #:
									<div class='tip'>" + field_title("attached_wo") + @"</div>
								</td>
								<td class='v'>{61}</td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='Attached to PO #'>
									Attached to PO #:
									<div class='tip'>" + field_title("attached_po") + @"</div>
								</td>
								<td class='v'>{62}</td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='US Currency'>
									US Currency:
									<div class='tip'>" + field_title("us_currency") + @"</div>
								</td>
								<td class='v'><input type='checkbox' id='us_currency' value='1' tabindex='20' onchange=""quote_obj.s('s_uscurrency', this)"" {26}/></td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='Inflation Term'>
									Inflation Term:
									<div class='tip'></div>
								</td>
								<td class='v'><input type='checkbox' id='inflation_term' value='1' tabindex='21' onchange=""quote_obj.s('s_inflationterm',this)"" {27}/></td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='Percent Down'>
									Percent Down:
									<div class='tip'>" + field_title("percent_down") + @"</div>
								</td>
								<td class='v'>
									{82}
								</td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='Net Due'>
									Payment Terms:
									<div class='tip'>" + field_title("net_due") + @"</div>
								</td>
								<td class='v'>
									{59}
								</td>
<td class='v'></td>
							</tr>
							<tr>
								<td class='c' data-title='Custom Term'>
									Billing Schedule:
									<div class='tip'>" + field_title("custom_term") + @"</div>
								</td>
								<td class='v'><textarea class='customterm' tabindex='24' id='custom_term' onchange=""quote_obj.s('s_customterm', this)"">{21}</textarea></td>
<td class='v'></td>
							</tr>
						</table>
		</div>
		<div id='open_quotes'>&nbsp;</div>
		<div id='quote_addcontact' style='display:none'>
			<table cellpadding='1' cellspacing='0' width='100%'>
				<tr>
					<td width='150'><b>NAME</b></td>
					<td><input type='text' class='contact_name' style='width:95%'></td>
				</tr>
				<tr>
					<td><b>TITLE:</b></td>
					<td>
						<select class='contact_title' style='width:95%'>
							<option value='0'>Choose Title</option>{44}</select>
					</td>
				</tr>
				<tr>
					<td><b>EMAIL ADDRESS:</b></td>
					<td><input type='text' class='contact_email' style='width:95%'/></td>
				</tr>
				<tr>
					<td><b>EXT #:</b></td>
					<td><input type='text' class='contact_ext' style='width:50px' /></td>
				</tr>
				<tr>
					<td><b>CELL #:</b></td>
					<td><input type='text' class='contact_cell' style='width:150px' /></td>
				</tr>
				<tr>
					<td colspan='2' height='35' valign='middle' align='center'><button type='button' onclick=""if($('.contact_name').val() != ''){{push_contact(this);}}else{{alert('You need to at least fill out a contact name.')}}"" style='background-color:#cfc;border:solid 1px #000;width:100%'>Save Contact</button></td>
				</tr>
			</table>
		</div>
		<div id='quote_detail'>
			<table cellpadding='0' cellspacing='0' width='100%'>
				<tr>
					<td class='buttonbar'>
						<button onclick=""this.disabled = true; add_detail('detail', this)"" class='detail_add_new'>add item</button>
						<input type='hidden' value='' class='active_row' />
					</td>
				</tr>
				<tr>
					<td class='entries' >
						<ul>
							{32}
						</ul>
					</td>
				</tr>
			</table>
		</div>
		<div id='quote_folder'>
			<iframe src='about:blank' frameborder='0' id='if_folder'></iframe>
		</div>
		<div id='quote_strategy' style='vertical-align:top;'>
			<iframe src='about:blank' frameborder='0' id='if_strategy'></iframe>
		</div>
		<div id='quote_er' style='display:none;'>
			<iframe src='about:blank'  frameborder='0' id='if_er'></iframe>
		</div>
		<div id='quote_notes'>
			<table cellpadding='0' cellspacing='0' width='99%'>
				<tr>
					<td class='buttonbar'>
						<button onclick=""this.disabled = true; add_detail('notes', this)"" class='detail_add_new'>add item</button>
					<input type='hidden' value='' class='active_row' />
					</td>
				</tr>
				<tr>
					<td class='entries'>
						<ul>
							{33}
						</ul>
					</td>
				</tr>
			</table>
		</div>
		<div id='quote_maintenance'>
			<table cellpadding='0' cellspacing='0' class='pane'>
				<tr>
					<td class='body' valign='top' align='left'>
						<div class='head'>Schedule Next</div>
						<table cellpadding='5' cellspacing='0'>
							<tr>
								<td align='left'><b>Date:</b></td>
								<td align='left'><input type='text' id='schedule_date' size='10'></td>
							</tr>
							<tr>
								<td align='left'><b>Note:</b></td>
								<td align='left'><textarea id='follow_up_note'  ></textarea></td>
							</tr>
							<tr>
								<td colspan='2' align='center'><button type='button' onclick='save_appointment(this);'><img src='/images/icon/icon[calendar].gif' align='absmiddle' width='16' height='16'/> save</button><input type='hidden' id='schedule_id' value=''></td>
							</tr>
						</table>
						<div class='head'>History</div>
						<div class='history'>
							<table cellpadding='0' cellspacing='0'>
								<thead>
									<th width='40%'>Date</th>
									<th width='30%'>Action</th>
									<th width='30%'>User</th>
								</thead>
								<tbody id='quote_history'>
								</tbody>
							</table>
						</div>
					</td>
				</tr>
			</table>
		</div>
		{58}
		<div style='display:none;' id='print_pane'>
			<iframe width='100%' height='740' name='print_pane' src='about:blank' frameborder='0' scrolling='yes' ></iframe>
		</div>
		<div style='display:none;' id='new_price_pane'>
			<div>Please enter the cost price for this item, it will then be ran through the markup formula.<br/><br/>This will be calculated using a quantity of 1, you may adjust the quantity afterward.</div>
			<div align='center'>
			$<input type='text' class='new_price' onkeydown=""if(event.keyCode == 13){{return_sellprice(this)}}"" size='10' data-isQTY='false'/><br/>
			<button type='button' onclick='return_sellprice(this)'>calculate</button>
			</div>
		</div>
		<div style='display:none;' id='kill_pane'>
			<input type='hidden' value='{47}' id='competitor_id' />
			{46}
		</div>
	</div>
	<input type='hidden' id='quote_ts' value='{85}'>
	<script>
		$(document).ready(function()
							{{
							//fill_quoters({9}, {66}, false);
							//fill_companies({0});
							do_static_functions();
							}});
	</script>
",
				business_unit_id,                                               // 0
				quote_id,                                               // 1
				revisions(),                                            // 2
				customer_id,                                            // 3
				contact_id,                                             // 4
				open_date,                                              // 5
				date_due,                                               // 6
				job_description,                                        // 7
				cust_spec_doc,                                          // 8
				quoted_by,                                              // 9
				last_print_date,                                        // 10
				last_fax_date,                                          // 11
				verified_date,                                          // 12
				hours_spent,                                            // 13
				dollars_spent,                                          // 14
				quoted_price,                                           // 15
				pricetype_select,                                       // 16
				takeoff_price,                                          // 17
				tm_pricing,                                             // 18
				percent_down,                                           // 19
				net_due,                                                // 20
				custom_term,                                            // 21
				_customer.Customer_Name,                                // 22
				current_quoter,                                         // 23
				open_date,                                              // 24
				contact_select,                                         // 25
				us_currency,                                            // 26
				inflation_term,                                         // 27
				_address.Addr1,                                         // 28
				_address.PhoneNumber,                                   // 29
				_address.FaxNumber,                                     // 30
				_contact.Contact_Email,                                 // 31
				divs(1),                                                // 32
				divs(2),                                                // 33
				quote_status_button,                                    // 34
				status_id,                                              // 35
				prev_status_id,                                         // 36
				revision,                                               // 37
				"NOT USED",                                             // 38
				Toolbox.do_value_from(po),                                  // 39
				Toolbox.do_value_from(wo),                                  // 40
				tm_pricing_total(),                                     // 41
				price_to_box,                                           // 42
				received_button_disabled,                               // 43
				get_title_options(),                                    // 44
				company_list(quoted_business_unit_id, false),                   // 45
				kill_reason_map(),                                      // 46
				competitor_id,                                          // 47
				quoted_business_unit_id,                                        // 48
				_address.Addr2,                                         // 49
				_address.Addr3,                                         // 50
				_address.Addr4,                                         // 51
				_address.City,                                          // 52
				_address.Prov,                                          // 53
				_address.Postal,                                        // 54
				_address.Country,                                       // 55
				_contact.Contact_CellPhone,                             // 56
				_contact.Contact_Extension,                             // 57
				search_pane(),                                          // 58
				term_options(),                                         // 59
				completion_date,                                        // 60
				wo_link,                                                // 61
				po_link,                                                // 62
				status_health,                                          // 63
				status_title,                                           // 64
				quoter_locked_str,                                      // 65
				quoted_business_unit_id_locked,                             // 66
				worksheet_total(),                                      // 67
				show_help_checked,                                      // 68
				active_revision,                                        // 69
				service_report(),                                       // 70
				include_title == "1" ? "checked" : "",              // 71
				"",                                     // 72
				status,                                                 // 73
																		//address_options(),                                      // 74
				build_canned_descriptions(),                            // 75
				"",  //build_percent_spinner(),                                // 76
				expected_value_update,                                  // 77
				strategy_tab,                                           // 78
				post_mortem_button,                                     // 79
				print_buttons,                                          // 80
				initial_quoted_by_list(Convert.ToInt32(quoted_by), quoter_locked_bool, false), // {81}
				build_percent_down(),                                   // 82
				worksheet_tab,                                          // 83
				follow_up ? "checked" : "",                         // 84
				ts_ticks,                                               // 85
				currency, // 86
				why_revised_html, // 87
				"hidden", //88
				exp_podate //89
			);
			return this_string;
		}

		public LabelValueInt[] build_percent_down()
		{
			var list = new List<LabelValueInt>();
			for (var i = 0; i <= 100; i += 5)
			{
				list.Add(new LabelValueInt()
				{
					Label = i.ToString() + "%",
					Value = i
				});
			}
			return list.ToArray();
		}
		public LabelValueInt[] period_valid_value()
		{
			var list = new List<LabelValueInt>();

			foreach (var i in new int[] { 3, 7, 14, 30, 60, 90 })
			{
				list.Add(new LabelValueInt()
				{
					Label = i.ToString(),
					Value = i
				});
			}
			return list.ToArray();
		}
		public string initial_quoted_by_list(int member_id, bool quoter_locked, bool search_pane)
		{
			var this_select = new StringBuilder();
			//			var disabled = quoter_locked ? " disabled='disabled'" : "";
			//			var c_id = search_pane ? this_member.business_unit_id.ToString() : quoted_business_unit_id;
			//			if (search_pane)
			//			{
			//				this_select.Append(@"<select class='subquotedby_name'><option value='0' selected>All</option>");
			//			}
			//			else
			//			{
			//				this_select.AppendFormat(@"<select class='quotedby' tabindex='13' id='quoted_by' onchange=""quote_obj.s('s_quotedby', this);"" {0}><option value='0'>Choose Quoter</option>", disabled);
			//			}
			//			foreach (DataRow dr in get_Quoters(c_id).Rows)
			//			{
			//				var id = Convert.ToInt32(dr["id"]);
			//				var name = dr["name"].ToString();
			//				var selected = id == member_id ? "selected" : "";
			//				this_select.AppendFormat(@"<option value='{0}' {2}>{1}</option>", id, name, selected);
			//			}
			//			this_select.Append("</select>");
			return this_select.ToString();
		}
		public string build_canned_descriptions()
		{
			var can_edit = this_member.AuthenticatedForPrivilege(102);
			//	string edit_button			= can_edit ? "<button type='button' style='vertical-align:base;'><img src='/images/icon/icon[edit].gif'/></button>" : "";
			//	string canned_descriptions	= string.Format(@"
			//<br/><select id='job_description_canned'></select>{0}", edit_button);
			//	return canned_descriptions;
			return "";
		}
		public string service_report()
		{
			var hider = this_company == null || (this_company.is_er || this_company.is_panelshop) ? "style='display:none;'" : "";
			var disabler = wo == "" || wo == "N/A" ? " title='A work order has not been attached to this quote.' disabled" : "";
			return "<button type='button' onclick=\"print_quote('service_report', this);$('#print_pane').dialog('open');\" " + hider + " data-wo='" + wo + "' " + disabler + "><img src='/images/icon/icon[print].gif' width='16' height='16' /><br/>Service Report</button>";
		}
		public string build_percent_spinner()
		{
			//	var stringWriter = new StringWriter();
			//	using (var writer = new HtmlTextWriter(stringWriter))
			//	{
			//		var spinner = new ASPxSpinEdit();
			//		spinner.Theme = "NETheme01";
			//		spinner.CssClass = "spinner";
			//		spinner.ID = "pct_chance";
			//		spinner.MinValue = 30;
			//		spinner.Increment = 30;
			//		spinner.DecimalPlaces = 0;
			//		spinner.AllowMouseWheel = true;
			//		spinner.Text = pct_chance.ToString();
			//		spinner.MaxValue = 90;
			//		spinner.AllowUserInput = false;
			//		spinner.ShowOutOfRangeWarning = false;
			//		spinner.Width = Unit.Pixel(50);
			//		spinner.ClientInstanceName = "pct_chance";
			//		spinner.ClientSideEvents.NumberChanged = "function(s,e){quote_obj.s('s_pctchance', s.mainElement);}";
			//		spinner.RenderControl(writer);
			//	}
			//	var body = "<table cellpadding='0' cellspacing='0' width='100%'><tr><td class='v'>" + stringWriter + "</td>";

			//	stringWriter = new StringWriter();
			//	using (var writer = new HtmlTextWriter(stringWriter))
			//	{
			//		var reasons = new DropDownList();
			//		reasons.ID = "pct_chance_reason";
			//		reasons.Width = Unit.Percentage(75);
			//		var _reasons = Toolbox.doSQL_dt(conn, @"SELECT * FROM quote_chance ORDER BY quote_chance_id", null);
			//		foreach (DataRow _reason in _reasons.Rows)
			//		{
			//			var id = _reason["quote_chance_id"].ToString();
			//			var name = _reason["quote_chance_name"].ToString();
			//			reasons.Items.Add(new ListItem(name, id));
			//		}
			//		reasons.Items.FindByValue(pct_chance_reason).Selected = true;
			//		reasons.Attributes["onchange"] = "quote_obj.s('s_pctchancereason', this);";
			//		reasons.RenderControl(writer);
			//	}
			//	var show_note = pct_chance_reason == "6" ? "" : "display:none;";
			//	body += "<td nowrap='nowrap' class='v' width='100%' align='center'>Why? " + stringWriter + @"</td></tr><tr><td colspan='2'  class='v' style='padding:4px;' align='center'><textarea onchange=""quote_obj.s('s_pctchancenote', this);"" id='pct_chance_note' style='" + show_note + "width:98%;'>" + pct_chance_note + "</textarea></td></tr></table>";
			//	return body;
			return "";
		}
		public string get_active_revision()
		{
			if (quote_id != "")
			{
				active_revision = Toolbox.doSQL_string(conn, @"SELECT MAX(revision) FROM quote_master WHERE quote_id = @v0  AND status_id != 9", new object[] { quote_id });
				active_revision = active_revision.Length > 0 ? active_revision : "1";
				return active_revision;
			}
			else
			{
				throw new Exception("Quote ID not set");
			}
		}


		public LabelValueInt[] term_options()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT id value, description label FROM quote_term ORDER BY order_n");
		}

		public string search_pane()
		{
			return $@"
		<div style='display:none;' id='search_pane' align='center'>
			<table width='100%' height='95' cellpadding='1' cellspacing='0'>
				<tr>
					<td valign='top' align='left'>
					<table class='criteria' cellpadding='0' cellspacing='1'>
						<tr>
							<td class='c'>Quote Number:</td>
							<td class='v'><input class='quote_number' type='text' /></td>
						</tr>
						<tr>
							<td class='c'>Job Description:</td>
							<td class='v'><input class='jobdescription' type='text' /></td>
						</tr>
						<tr>
							<td class='c'>Quoted By (Business Unit):</td>
							<td class='v'>{company_list(this_member.business_unit_id, true)}</td>
						</tr>
						<tr>
							<td class='c'>Quoted By (Name):</td>
							<td class='v'>{initial_quoted_by_list(0, false, true)}</td>
						</tr>
						<tr>
							<td class='c'>Customer Name:</td>
							<td class='v'><input class='customer_name' type='text' /></td>
						</tr>
						<tr>
							<td class='c'>Quoted Before:</td>
							<td class='v'><input class='quoted_before' type='text' /><script type='text/javascript'>$('.quoted_before').datepicker({{dateFormat:'yy-mm-dd'}});</script></td>
						</tr>
						<tr>
							<td class='c'>Quoted After:</td>
							<td class='v'><input class='quoted_after' type='text' /><script type='text/javascript'>$('.quoted_after').datepicker({{dateFormat:'yy-mm-dd'}});</script></td>
						</tr>
						<tr>
							<td class='c'>Status:</td>
							<td class='v'>
								<select class='status'>
									<option value='0'>Choose Status</option>
									<option value='OPEN'>All Open Quotes</option>
									<option value='8'>PO Received</option>
									<option value='6_8'>Quote is Closed</option>
									<option value='1'>Waiting to be Quoted</option>
									<option value='4'>Waiting Approval</option>
<option value='10'>Waiting for Stage 1 Go</option>
<option value='11'>Waiting for Stage 4 Go</option>
<option value='12'>Waiting for Final Review</option>
<option value='13'>Waiting for Post Mortem</option>
								</select>
							</td>
						</tr>
					</table>
					<div class='center'>This will show the top 500 quotes matching your defined criteria.</div>
					<div class='center'><button type='button' onclick='search_quotes(this);'>search</button><button type='button' onclick=""$('#quote_search_results .results').select();"">select</button></div>
				</td>
			</tr>
			<tr>
				<td valign='top' align='center'><div id='quote_search_results'>&nbsp;</div>
				</td>
			</tr>
			</table>
		</div>
		";
		}
		public DTO.ViewModels.Page.Quotes.QuoteDetail[] divs(int which)
		{
			var div_name = which == 1 ? "detail" : which == 2 ? "notes" : "";

			var _dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM quote_extratext WHERE quote_id = @v0  AND revision = @v1  AND type = @v2 ORDER BY if(order_number<0,line_number,order_number)", new object[] { quote_id, revision, which });
			var sbList = new List<DTO.ViewModels.Page.Quotes.QuoteDetail>();
			if (_dt.Rows.Count > 0)
			{
				foreach (DataRow _dr in _dt.Rows)
				{
					var isChecked = Convert.ToBoolean(_dr["is_checked"]);
					var order_number = Convert.ToInt32(_dr["order_number"]);
					var line_number = Convert.ToInt32(_dr["line_number"]);
					order_number = (order_number < 0 ? line_number : order_number);
					var sb = new DTO.ViewModels.Page.Quotes.QuoteDetail
					{
						count = 1,
						line_text = _dr["linetext"].ToString(),
						row_id = _dr["id"].ToString(),
						is_checked = isChecked,
						order_number = order_number,
						line_number = line_number
					};

					sb.has_name = sb.line_text != "";
					//var isDisabled = "";
					//var picklist_button = "";
					//					if (div_name == "detail")
					//					{
					sb.current_section_total = bllToolbox.doSQL_double(@" SELECT IFNULL(SUM(a.extended_per),0) FROM quote_worksheet a LEFT JOIN quote_section b ON a.section_id = b.id WHERE b.detail_id = @v0 and a.is_checked=1", sb.row_id);
					sb.is_referenced = bllToolbox.doSQL_int(@"SELECT IF(COUNT(*) > 0, 1, 0) FROM quote_section WHERE detail_id = @v0 ", sb.row_id) == 1;
					if (sb.is_referenced)
					{
						sb.section_id = bllToolbox.doSQL_int(@"SELECT id FROM quote_section WHERE detail_id = @v0 ", sb.row_id);
						//var _c = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM quote_worksheet WHERE section_id = @v0 ", new object[] { sb.section_id });
						//picklist_button = string.Format("<button type='button' onclick='get_section(this,{1})' id='s_{1}'>Section Total(~Ext'd): {0:C2}</button>", current_section_total, section_id);
					}
					//					}
					//					isDisabled = $@"onclick=""this.disabled=true;del_extratext(this, '{div_name}');""";
					//					sb.AppendFormat(@"
					//					<li>
					//						<div data-row_id='{3}'>
					//							<table cellpadding=0 cellspacing=0 width='100%'>
					//								<tr>
					//								<td class='c' align='center'><span id='row_n'>{1}</span></td>
					//									<td class='t'>
					//<textarea onload='_resize(this);' onfocus='expand(this);' onchange=""quote_obj.s('s_{0}row', this)"">
					//{2}
					//</textarea>
					//									</td>
					//									<td class='action'><button type='button' {5}>X</button>{4}</td>
					//								</tr>
					//							</table>
					//						</div>
					//					</li>", div_name, count, line_text, row_id, picklist_button, isDisabled);
					sbList.Add(sb);
					sb.count++;

				}
			}
			return sbList.ToArray();
		}

		public DTO.ViewModels.Page.Quotes.QuoteDetail createSectionForNoteAdder(int line_id)
		{
			var detail_row = bllToolbox.doSQL_dt(@"SELECT * FROM quote_extratext WHERE id = @v0 ", line_id).Rows[0];
			var v = detail_row["linetext"].ToString().Trim();
			var step = Convert.ToInt32(detail_row["line_number"]);
			var order_number = Convert.ToInt32(detail_row["order_number"]) + 1;
			var detail_name = $"N{step + 1}. {v.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ")}";
			var len = detail_name.Length > 60 ? 60 : detail_name.Length;
			if (!string.IsNullOrEmpty(detail_row["section_id"].ToString()))
				return new DTO.ViewModels.Page.Quotes.QuoteDetail()
				{
					section_id = Convert.ToInt32(detail_row["section_id"]),
					row_id = line_id.ToString(),
					count = step,
					line_text = v,
					line_number = step,
					order_number = order_number
				};
			// var section_id = bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(id),0) FROM quote_section WHERE detail_id = @v0", line_id);
			detail_name = detail_name.Substring(0, len).Trim();
			var section_id = Convert.ToInt32(save_section(detail_name, line_id));
			bllToolbox.doSQL_void(@"UPDATE quote_extratext set section_id=@p0 where id=@p1", section_id, line_id);
			return new DTO.ViewModels.Page.Quotes.QuoteDetail()
			{
				section_id = section_id,
				row_id = line_id.ToString(),
				count = step,
				line_text = v,
				line_number = step,
				order_number = order_number
			};
		}

		public DTO.ViewModels.Page.Quotes.QuoteDetail new_extratext(int _type, string _text = "")
		{
			_text = _text.Trim();
			var max_order_number = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(if(order_number<0,line_number,order_number)),0) FROM quote_extratext WHERE quote_id = @v0  AND revision = @v1  AND type = @v2 ", new object[] { quote_id, revision, _type });
			var max_line_number = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(line_number), -1) FROM quote_extratext WHERE quote_id = @v0  AND revision = @v1  AND type = @v2 ", new object[] { quote_id, revision, _type });
			max_line_number = max_line_number == -1 ? 0 : max_line_number + 1;
			var section_id = 0;
			if (quote_id == string.Empty)
			{
				throw new Exception("Blank Quote ID");
			}
			var extratext_id = Toolbox.doSQL_return_id(@" INSERT INTO quote_extratext ( quote_id, revision, type, line_number, linetext ) VALUES ( @v0 , @v1 , @v2 , @v3 , @v4 )", new object[] { quote_id, revision, _type, max_line_number, _text });
			//			if (_type == 1)
			//			{
			section_id = Convert.ToInt32(save_section("", extratext_id));
			Toolbox.doSQL_void(conn, @"UPDATE quote_extratext SET section_id = @v0  WHERE id = @v1  LIMIT 1", new object[] { section_id, extratext_id });
			//			}
			return new DTO.ViewModels.Page.Quotes.QuoteDetail()
			{
				section_id = section_id,
				row_id = extratext_id.ToString(),
				count = max_line_number,
				line_text = _text,
				is_checked = true,
				line_number = max_line_number,
				order_number = max_order_number
			};
		}
		public string lock_quoter(string _lock)
		{
			try
			{
				Toolbox.doSQL_void(conn, @"UPDATE quote_master SET quoter_locked = @v2 , updated_by_page = 'quote - lock_quoter()' WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision, _lock });
				return "SUCCESS";
			}
			catch (Exception ee)
			{
				return ee.ToString();
			}
		}
		public string get_pricetype()
		{
			if (pricetype_id != "")
			{
				this_string = Toolbox.doSQL_string(conn, @"SELECT type FROM quote_pricetype WHERE id = @v0 ", new object[] { pricetype_id });
			}
			else
			{
				this_string = "Quoted Price";
			}
			return this_string;
		}
		public string get_questionnaire()
		{
			var questions = "";
			var _dt = Toolbox.doSQL_dt(conn, @" SELECT a.id reason_id, c.name tier, a.reason FROM quote_kill_reason a LEFT JOIN quote_kill_reason_map b ON a.id = b.reason_id AND b.quote_id = @v0  AND b.revision = @v1  LEFT JOIN quote_kill_tier c ON a.tier_id = c.id WHERE a.id in (SELECT reason_id FROM quote_kill_reason WHERE quote_id = @v0  and revision = @v1 )", new object[] { quote_id, revision });
			if (_dt.Rows.Count > 0)
			{
				foreach (DataRow row in _dt.Rows)
				{
					var tier = row["tier"].ToString();
					var reason = row["reason"].ToString();
					questions += string.Format(@"
{1} {0}
", reason, tier);
				}
			}
			return questions;
		}
		public string get_sellprice(string master_id)
		{
			sql = $"SELECT sellp FROM inventory_item_master WHERE master_id = {master_id}";
			try
			{
				this_string = Toolbox.doSQL_double(conn, @"SELECT sellp FROM inventory_item_master WHERE master_id = @v0 ", new object[] { master_id }).ToString("N2");
			}
			catch
			{
				this_string = "0.00";
			}
			return this_string;
		}
		public string get_title_options()
		{
			this_string = "";
			var _dt = Toolbox.doSQL_dt(conn, @"SELECT title_id id,title_name name FROM titles", null);
			if (_dt.Rows.Count > 0)
			{
				foreach (DataRow row in _dt.Rows)
				{
					var this_id = row["id"].ToString();
					var this_name = row["name"].ToString().Replace("\"", "");
					this_string += string.Format(@"
	<option value=""{1}"">{1}</option>", this_id, this_name);
				}
			}
			else
			{
				this_string = "<option value='0'>No Titles Available</option>";
			}
			return this_string;
		}
		public string kill_reason_map()
		{
			if (this_company != null)
			{
				this_string = @"
			<table cellpadding='0' cellspacing='0' width='100%'>
				<tr>
					<td colspan='2'><b>Why did we lose the job? (required)</b></td>
				</tr>
				<tr>
					<td colspan='2'><textarea id='why_lose' style='width:100%;height:150px;margin-bottom:10px;'>" + why_lose + @"</textarea></td>
				</tr>
				<tr>
					<td colspan='2'><b>Who was the competitor? (optional)</b></td>
				</tr>
				<tr>
					<td colspan='2'><textarea id='who_competitor' style='width:100%;height:150px;margin-bottom:10px;'>" + who_competitor + @"</textarea></td>
				</tr>
				<tr>
					<td colspan='2'><b>What was the price that it went for? (optional)</b></td>
				</tr>
				<tr>
					<td colspan='2'><textarea id='what_price' style='width:100%;height:150px;margin-bottom:10px;'>" + what_price + @"</textarea></td>
				</tr>
				<tr>
					<td colspan='2' align='center' style='padding-top:15px;'>
						<button type='button' id='kill_quote' onclick='kill_resurrect(this);' style='font-size:8px;font-family:verdana;width:100px;font-weight:bold;margin-right:10px;'><img src='/images/icon/icon[dead].gif' align='absmiddle'  width='16' height='16'/><br/>KILL QUOTE</button>
						<button type='button' id='resurrect_quote' onclick='kill_resurrect(this);' style='font-size:8px;font-family:verdana;width:100px;font-weight:bold;'><img src='/images/icon/icon[alive].gif' align='absmiddle' width='16' height='16' /><br/>IT'S ALIVE!</button>
					</td>
				</tr>
			</table>";
			}
			else
			{
				this_string = "";
			}
			/*
			try 
				{
				DataTable tiers			= Toolbox.doSQL_dt(conn,@"SELECT * FROM quote_kill_tier"  , null);
				if(tiers.Rows.Count > 0)
					{
	";
					foreach(DataRow tier in tiers.Rows)
						{
						string tier_id		= tier["id"].ToString();
						string tier_name	= tier["name"].ToString();
						this_string			+= string.Format(@"
					<tr>
						<td colspan='2' style='padding:5px;'><b>{0}</b></td>
					</tr>
						", tier_name);
						DataTable reasons	= Toolbox.doSQL_dt(conn,@" SELECT a.id reason_id, a.reason, IF(IFNULL(b.reason_id, 0) = 0, 0, 1) checked FROM quote_kill_reason a LEFT JOIN quote_kill_reason_map b ON a.id = b.reason_id AND b.quote_id = @v0  AND b.revision = @v1  WHERE a.tier_id = @v2 ", new object[] {  quote_id, revision, tier_id } );
						foreach(DataRow reason in reasons.Rows)
							{
							string reason_id			= reason["reason_id"].ToString();
							string reason_name			= reason["reason"].ToString();
							this_bool					= Convert.ToBoolean(Convert.ToInt32(reason["checked"].ToString()));
							string this_checked			= "";
							if(this_bool)
								{
								this_checked			= " checked";
								}
							this_string					+= string.Format(@"
					<tr>
						<td align='right' width='50'><input type='checkbox' name='reasons[]' value='{0}' {2}></td>
						<td>{1}</td>
					</tr>", reason_id, reason_name, this_checked);
							}
						}

					this_string						+= @"
					<tr>
						<td colspan='2' style='padding:5px;'><u><b>Competitor?</b></u></td>
					</tr>
					<tr>
						<td width='50'>&nbsp;</td>
						<td><input type='text' value="""+competitor_name+@""" onkeydown=""if(event.keyCode != 13 && event.keyCode != 9 ){$('#competitor_id').val('');}"" size='50'/><div style='font-size:10px;'>Start typing name, they will appear if they exist in the database.<br/>If there isn't one, just leave this field blank.</div></td>
					</tr>
					<tr>
						<td colspan='2' align='center' style='padding-top:15px;'>
							<button type='button' id='kill_quote' onclick='kill_resurrect(this);' style='font-size:8px;font-family:verdana;width:100px;font-weight:bold;margin-right:10px;'><img src='/images/icon/icon[dead].gif' align='absmiddle' /><br/>KILL QUOTE</button>
							<button type='button' id='resurrect_quote' onclick='kill_resurrect(this);' style='font-size:8px;font-family:verdana;width:100px;font-weight:bold;'><img src='/images/icon/icon[alive].gif' align='absmiddle' /><br/>IT'S ALIVE!</button>
						</td>
					</tr>
				</table>";
					}
				else
					{
					this_string			= "Kill reasons do not exist.";
					}
				}
			catch(Exception ee)
				{
				this_string				= "Cannot pull kill reasons";
				}
			 */
			return this_string;
		}
		public string new_revision()
		{
			revision = Toolbox.doSQL_string(conn, @"SELECT MAX(revision) FROM quote_master WHERE quote_id = @v0  AND status_id != 9", new object[] { quote_id });
			if (revision != "")
			{
				var newrev = Toolbox.doSQL_string(conn, @"CALL RevisionQuote(@v0, @v1, null)", new object[] { quote_id, revision });
				try
				{
					quote.copy_worksheet_file_to_new_rev(quote_id, revision, newrev);
				}
				catch { }

				return newrev;
			}
			else
			{
				throw new Exception("There isn't an active version to revise.");
			}
		}



		public string quote_count(string status_id)
		{
			if (status_id == "10" || status_id == "11" || status_id == "12")
			{


				if (all_reports == null)
				{
					all_reports = NeMember.get_allreports(Convert.ToInt32(quoted_by));


					foreach (DataRow dr in all_reports.Rows)
					{
						reports_list += dr["member_id"] + ",";
					}
				}
				reports_list = reports_list.TrimEnd(',');
				if (reports_list != "")
				{
					return Toolbox.doSQL_string(conn,
						$@"SELECT COUNT(quote_id) C FROM quote_master WHERE quoted_by in ({reports_list}) AND status_id = @v0 ", new object[] { status_id });
				}
				else
				{
					return "0";
				}
			}
			else
			{
				return Toolbox.doSQL_string(conn, @"SELECT COUNT(quote_id) C FROM quote_master WHERE quoted_by = @v0  AND status_id = @v1 ", new object[] { quoted_by, status_id });
			}
		}
		public string quote_dollar_total(string status_id)
		{
			if (status_id == "10" || status_id == "11" || status_id == "12")
			{


				if (all_reports == null)
				{
					all_reports = NeMember.get_allreports(Convert.ToInt32(quoted_by));


					foreach (DataRow dr in all_reports.Rows)
					{
						reports_list += dr["member_id"] + ",";
					}
				}
				reports_list = reports_list.TrimEnd(',');
				this_string = Convert.ToDouble(Toolbox.doSQL_string(conn,
					$@"SELECT IFNULL(SUM(if(status_id in(10,11,12),ifnull(expected_value,0),quoted_price)), 0) 
FROM quote_master  WHERE quoted_by in ({reports_list}) AND status_id =@v0 ", new object[] { status_id })).ToString("N2");
			}
			else
			{
				this_string = Convert.ToDouble(Toolbox.doSQL_string(conn, @"SELECT IFNULL(SUM(if(status_id=1 and ifnull(quoted_price,0)=0,ifnull(expected_value,0),ifnull(quoted_price,0))), 0) FROM quote_master  WHERE quoted_by =@v0 AND status_id =@v1 ", new object[] { quoted_by, status_id })).ToString("N2");
			}
			if (this_string == "0.00")
			{
				this_string = "0.00";
			}
			else
			{
				this_string = "$" + this_string + "";
			}
			return this_string;
		}

		/// <summary>
		/// Returns a string list of the quotes requiring immedaite attention.
		/// </summary>
		/// <returns></returns>

		public string todonow_list()
		{

			//var reports_list = this_member.id + ","; 
			var sb = new StringBuilder();
			#region old way
			//	DataTable dt = NeMember.get_allreports(Convert.ToInt32(quoted_by));

			//	foreach (DataRow dr in dt.Rows)
			//	{
			//		reports_list += dr["member_id"] + ",";
			//	}
			//	reports_list = reports_list.TrimEnd(',');
			/*
			DataTable dt = Toolbox.do_dt(conn, string.Format(@"
	SELECT 
		a.quote_id,
		a.revision,
		a.pricetype_id,
		b.type pricetype,
		a.price_to,
		CAST(DATE_FORMAT(a.open_date, '%m/%d/%Y') AS CHAR) open_date,
		a.customer_id,
		a.quoted_price,
		a.job_description,
		ifnull(a.allowed_to_quote,0) allowed_to_quote,
		ifnull(a.allowed_to_quote_2,0) allowed_to_quote_2,
		ifnull(a.expected_value,0) expected_value,
		ifnull(c.uses_quote_process,0) uses_quote_process,
		ifnull(c.quote_level_2_start,0) quote_level_2_start,
		ifnull(c.quote_level_3_start,0) quote_level_3_start,
		d.status,
		c.business_unit_id,
		a.quoted_by,
		a.status_id,
		e.member_fullname,
		GET_BM(a.business_unit_id) bm_id
	FROM 
		quote_master a
	LEFT JOIN
		quote_pricetype b ON a.pricetype_id = b.id
		inner join business_unit c on a.business_unit_id = c.id  
		inner join quote_status d on a.status_id = d.id
		left join quote_schedule q on a.quote_id = q.quoteid
		LEFT JOIN member e ON a.quoted_by = e.member_id
	WHERE 
		  ((a.status_id = 10) or (a.status_id in(11,12,13,4) and (q.id!=0))) and
		(
	q.estimator={1} or
	q.pointperson={1} or
	q.stage1screening_mid={1} or
	q.schedule_produced_mid={1} or
	q.manpower_information_collected_mid={1} or
	q.finance_info_collected_mid={1} or
	q.customer_info_collected_mid={1} or
	q.market_info_collected_mid={1} or
	q.recon_report_created_mid={1} or
	q.quote_delivery_strategy_mid={1} or
	q.project_estimated_mid={1} or
	q.worksheet_review_mid={1} or
	q.stage6_final_review_mid={1} or
	q.worksheet_review_mid={1} or
	q.quote_delivered_mid={1} or
	q.followup1_mid={1} or
	q.followup2_mid={1} or
	q.convert_or_kill_mid={1} or
	q.post_mortem_complete_mid={1} or
	q.rt1 = {1} or
	q.rt2 = {1} or
	q.rt3 = {1} or
	q.rt4 = {1})
	ORDER BY a.open_date", 0,this_member.id));

			StringBuilder sb = new StringBuilder();
			if (dt.Rows.Count > 0)
			{
				foreach (DataRow _dr in dt.Rows)
				{
					string this_id = _dr["quote_id"].ToString();
					string this_date_open = _dr["open_date"].ToString();
					string this_quoted_price = _dr["quoted_price"].ToString();
					string this_custname = "";
					string this_price_to = _dr["price_to"].ToString();
					string this_pricetype_id = _dr["pricetype_id"].ToString();
					string this_pricetype = _dr["pricetype"].ToString();
					string this_allowed_to_quote = _dr["allowed_to_quote"].ToString();
					string this_allowed_to_quote_2 = _dr["allowed_to_quote_2"].ToString();
					string this_expected_value = _dr["expected_value"].ToString();
					string this_uses_quote_process = _dr["uses_quote_process"].ToString();
					string this_quote_level_2_start = _dr["quote_level_2_start"].ToString();
					string this_quote_level_3_start = _dr["quote_level_3_start"].ToString();
					string allowed_color = "";
					string allowed_color_2 = "";
					string this_company = _dr["business_unit_id"].ToString();
					string status_id = _dr["status_id"].ToString();
					string status = _dr["status"].ToString();
					string this_quotedby = _dr["quoted_by"].ToString();
					string this_revision = _dr["revision"].ToString();
					string this_fullname = _dr["member_fullname"].ToString();
					customer_id = _dr["customer_id"].ToString();
					string row_message = "";
					string this_description = Toolbox.do_value_from(_dr["job_description"]);
					int bm_id		= Convert.ToInt32(_dr["bm_id"]);

					this_quoted_price = this_pricetype_id == "6" ? _tools.Monetize(this_quoted_price) + " to " + _tools.Monetize(this_price_to) : _tools.Monetize(this_quoted_price);

					if (customer_id != "" && customer_id != "NULL")
					{
						get_customer_name();
					}
					this_custname = customer_name.Length > 14 ? customer_name.Substring(0, 12) + ".." : customer_name;
					NeQuoteSchedule qs = new NeQuoteSchedule(this_id);
					bool is_level_2 = ((Convert.ToDouble(this_expected_value) >= Convert.ToDouble(this_quote_level_2_start)) && (Convert.ToDouble(this_expected_value) < Convert.ToDouble(this_quote_level_3_start)));
					bool is_level_3 = (Convert.ToDouble(this_expected_value) >= Convert.ToDouble(this_quote_level_3_start));
					if (qs.id != 0)
					{

						if ((is_level_2 && (status_id == "10") && ((this_member.id == bm_id))) ||  // if its a level 2 and its a branch manager
						   (is_level_3 && (status_id == "10") && (NeMember.is_supervisor(bm_id, this_member.id))))
						{
							row_message = "Waiting for stage 1 approval from you.";
						}
						else if (status_id == "11")
						{
							if ((this_member.id == qs.schedule_produced_mid) && (qs.schedule_produced_date_completed == null))
							{
								row_message = "Waiting for you to produce and distribute the schedule.";
							}
							else if ((this_member.id == qs.manpower_information_collected_mid) && (qs.manpower_information_collected_date_completed == null))
							{
								row_message = "Waiting for you to complete the manpower info recon";
							}
							else if ((this_member.id == qs.market_info_collected_mid) && (qs.market_info_collected_date_completed == null))
							{
								row_message = "Waiting for you to complete the market info recon";
							}
							else if ((this_member.id == qs.customer_info_collected_mid) && (qs.customer_info_collected_date_completed == null))
							{
								row_message = "Waiting for you to complete the customer recon";
							}
							else if ((this_member.id == qs.finance_info_collected_mid) && (qs.finance_info_collected_date_completed == null))
							{
								row_message = "Waiting for you to complete the finance recon";
							}
							else if ((this_member.id == qs.recon_report_created_mid) && (qs.recon_report_created_date_completed == null))
							{
								row_message = "Waiting for you to review and approve all the recon stuff";
							}
							else if ((qs.recon_report_created_date_completed != null))
							{
								if (((this_member.id == qs.rt1) && (qs.rt1_s4_approved == null)) || ((this_member.id == qs.rt2) && (qs.rt2_s4_approved == null)) || ((this_member.id == qs.rt3) && (qs.rt3_s4_approved == null)) || ((this_member.id == qs.rt4) && (qs.rt4_s4_approved == null)))
								{
									row_message = "Waiting for you to review the recon info and approve it through stage 4";
								}
							}
						}
						else if (status_id == "12")
						{
							if ((this_member.id == qs.quote_delivery_strategy_mid) && (qs.quote_delivery_strategy_date_completed == null))
							{
								row_message = "Waiting for you to finish the sell strategy";
							}
							else if ((this_member.id == qs.project_estimated_mid) && (qs.project_estimated_date_completed == null))
							{
								row_message = "Waiting for you to estimate the project";
							}
							else if ((this_member.id == qs.worksheet_review_mid) && (qs.project_estimated_date_completed != null) && (qs.worksheet_review_date_completed == null))
							{
								row_message = "Waiting for you to complete the worksheet review";
							}
							else if (qs.worksheet_review_date_completed != null)
							{
								if (is_level_3)
								{
									if (((this_member.id == qs.rt1) && (qs.rt1_f_approved == null)) || ((this_member.id == qs.rt2) && (qs.rt2_f_approved == null)) || ((this_member.id == qs.rt3) && (qs.rt3_f_approved == null)) || ((this_member.id == qs.rt4) && (qs.rt4_f_approved == null)))
									{
										row_message = "Waiting for you to give the final review";
									}
								}
								else if (is_level_2)
								{
									if ((this_member.id == bm_id))
									{
										if (((this_member.id == qs.rt1) && (qs.rt1_f_approved == null)) || ((this_member.id == qs.rt2) && (qs.rt2_f_approved == null)) || ((this_member.id == qs.rt3) && (qs.rt3_f_approved == null)) || ((this_member.id == qs.rt4) && (qs.rt4_f_approved == null)))
										{
											row_message = "Waiting for you to give the final review";
										}
									}
								}

							}

						}
						else if (status_id == "4")
						{
							if ((this_member.id == qs.quote_delivered_mid) && (qs.quote_delivered_date_completed == null))
							{
								row_message = "Waiting for you to deliver the quote";
							}
							else if ((this_member.id == qs.convert_or_kill_mid) && (qs.convert_or_kill_date_completed == null))
							{
								row_message = "Waiting for you to convert or kill the quote";
							}
						}
						else if (status == "13")
						{
							if ((this_member.id == qs.post_mortem_complete_mid) && (qs.post_mortem_complete_date_completed == null))
							{
								row_message = "Waiting for you to complete the post mortem";
							}
						}

					}
					else
					{
						if ((is_level_2 && (status_id == "10") && ((this_member.id == bm_id))) ||  // if its a level 2 and its a branch manager
						   (is_level_3 && (status_id == "10") && (NeMember.is_supervisor(bm_id, this_member.id))))
						{
							row_message = "Waiting for your initial approval to allow the quote to begin";
						}
					}
					*/
			#endregion
			var dt_quotes = new quote().get_quote_todo_list(this_member.id);
			foreach (DataRow dr_quotes in dt_quotes.Rows)
			{

				sb.AppendFormat(@"<tr>
						<td valign='top' class='quotes' width='10%'><a href='javascript:boing(""index.aspx?a=g&quote_id={0}&revision={6}"",""quote_count" + quote_id + @""",1050,920);'>{0}</a></td>
						<td valign='top' class='quotes' width='15%'>{1}</td>
						<td valign='top' class='quotes' width='10%'>{2}</td>
						<td valign='top' class='quotes' width='10%'>{3}</td>
						<td valign='top' class='quotes' width='60%'>{4}</td>
						<td valign='top' class='quotes' width='10%'>{5}</td>
							</tr>",
					dr_quotes["quote_id"],            // 0
					Convert.ToBoolean(dr_quotes["is_level_2"]) ? "Level 2" : Convert.ToBoolean(dr_quotes["is_level_3"]) ? "Level 3" : "Level 1",  //1
					dr_quotes["row_message"],   // 2
					dr_quotes["customer_name"],       // 3
					dr_quotes["this_description"], //4
					dr_quotes["this_fullname"],  //5
					dr_quotes["revision"]  //6
				);

			}
			return sb.ToString();
		}


		/// <summary>
		/// Returns a string list of the available quotes.
		/// </summary>
		/// <param name="statusid"></param>
		/// <returns></returns>
		public QuoteListItem[] quote_list(string statusid) // Needs to be revisited
		{
			QuoteListItem[] _dt;

			// var columns = @"";
			if (statusid == "5")
			{
				_dt = base.bllToolbox.doSQL_Array<QuoteListItem>(@"
						SELECT a.quote_id, a.revision, a.pricetype_id, a.price_to, 
						c.type pricetype, CAST( DATE_FORMAT(a.open_date, '%m/%d/%Y') AS CHAR ) open_date, 
						a.quoted_price, a.customer_id, a.job_description, IFNULL(a.allowed_to_quote, 0) allowed_to_quote,
						IFNULL(a.allowed_to_quote_2, 0) allowed_to_quote_2, 
						IFNULL(a.expected_value, 0) expected_value, 
						IFNULL(d.uses_quote_process, 0) uses_quote_process, 
						IFNULL(d.quote_level_2_start, 0) quote_level_2_start, 
						IFNULL(d.quote_level_3_start, 0) quote_level_3_start, 
						a.quoted_by, e.member_fullname, d.ddl_name 
						FROM quote_master a LEFT JOIN quote_follow_up b ON a.quote_id = b.quote_id AND a.revision = b.revision and b.is_done=0
						LEFT JOIN quote_pricetype c ON a.pricetype_id = c.id
						INNER JOIN business_unit d ON a.business_unit_id = d.id 
						LEFT JOIN member e ON a.quoted_by = e.member_id WHERE a.quoted_by = @v0 AND a.active_revision = 1  
						AND b.schedule_date <= CURDATE() AND a.status_id NOT IN (6, 7, 8, 9) 
						GROUP BY quote_id, revision", quoted_by);
			}
			else if (statusid == "10" || statusid == "11" || statusid == "12")
			{
				reports_list = this_member.id + ",";
				if (all_reports == null)
				{
					all_reports = NeMember.get_allreports(Convert.ToInt32(quoted_by));


				}
				foreach (DataRow dr in all_reports.Rows)
				{
					reports_list += dr["member_id"] + ",";
				}
				reports_list = reports_list.TrimEnd(',');

				_dt = base.bllToolbox.doSQL_Array<QuoteListItem>(
					$@" SELECT a.quote_id, a.revision, a.pricetype_id, b.type pricetype, a.price_to, CAST( DATE_FORMAT(a.open_date, '%m/%d/%Y') AS CHAR ) open_date, a.customer_id, a.quoted_price, a.job_description, IFNULL(a.allowed_to_quote, 0) allowed_to_quote, IFNULL(a.allowed_to_quote_2, 0) allowed_to_quote_2, IFNULL(a.expected_value, 0) expected_value, IFNULL(c.uses_quote_process, 0) uses_quote_process, IFNULL(c.quote_level_2_start, 0) quote_level_2_start, IFNULL(c.quote_level_3_start, 0) quote_level_3_start, a.quoted_by, d.member_fullname, c.ddl_name 
FROM quote_master a LEFT JOIN quote_pricetype b ON a.pricetype_id = b.id 
INNER JOIN business_unit c ON a.business_unit_id = c.id LEFT JOIN member d ON a.quoted_by = d.member_id 
WHERE ( a.quoted_by IN ({reports_list})   AND a.active_revision = 1 AND a.status_id = @v0  AND c.id IN ({
						VisibleBusinessUnit
					}) ) ORDER BY a.open_date", statusid);

			}
			else
			{
				_dt = base.bllToolbox.doSQL_Array<QuoteListItem>(
					$@" SELECT a.quote_id, a.revision, a.pricetype_id, b.type pricetype, a.price_to, CAST( DATE_FORMAT(a.open_date, '%m/%d/%Y') AS CHAR ) open_date, 
a.customer_id, a.quoted_price, a.job_description, IFNULL(a.allowed_to_quote, 0) allowed_to_quote, IFNULL(a.allowed_to_quote_2, 0) allowed_to_quote_2, 
IFNULL(a.expected_value, 0) expected_value, IFNULL(d.uses_quote_process, 0) uses_quote_process, IFNULL(d.quote_level_2_start, 0) quote_level_2_start,
IFNULL(d.quote_level_3_start, 0) quote_level_3_start, a.quoted_by, c.member_fullname, d.ddl_name
FROM quote_master a 
LEFT JOIN quote_pricetype b ON a.pricetype_id = b.id INNER JOIN business_unit d ON a.business_unit_id = d.id
LEFT JOIN member c ON a.quoted_by = c.member_id WHERE a.quoted_by = @v0  AND a.status_id = @v1 AND a.active_revision = 1  AND d.id IN ({
						VisibleBusinessUnit
					}) AND a.active_revision = TRUE ORDER BY a.open_date", quoted_by, statusid);
			}

			foreach (var _dr in _dt)
			{
				if (statusid == "10" || statusid == "11" || statusid == "1" && (string.IsNullOrEmpty(_dr.quoted_price) || Math.Abs(Convert.ToDouble(_dr.quoted_price) - 0.0) < 0.01))
				{
					_dr.quoted_price = _dr.expected_value;
				}

				if (_dr.pricetype_id == "6")
				{
					_dr.quoted_price = _tools.Monetize(_dr.quoted_price) + " to " + _tools.Monetize(_dr.price_to);
				}
				else
				{
					_dr.quoted_price = _tools.Monetize(_dr.quoted_price);
				}
				if (_dr.customer_id != "" && _dr.customer_id != "NULL")
				{
					_dr.customer_name = get_customer_name(_dr.customer_id);
					// _dr.customer_name = _dr.customer_name.Length > 14 ? _dr.customer_name.Substring(0, 12) + ".." : _dr.customer_name;
				}
				if (_dr.uses_quote_process == "1" && Convert.ToDouble(_dr.expected_value) > Convert.ToDouble(_dr.quote_level_3_start))
				{
					_dr.allowed_color = "red";
					_dr.quote_level = "Level 3";
				}
				else if (_dr.uses_quote_process == "1" && Convert.ToDouble(_dr.expected_value) > Convert.ToDouble(_dr.quote_level_2_start))
				{
					_dr.allowed_color = "red";
					_dr.quote_level = "Level 2";
				}
				_dr.status_id = statusid;
				if (statusid == "10" || statusid == "11")
				{
					_dr.divTitle = "Quote cannot be viewed while in " + (status_id == "10" ? "Stage 1" : "Stage 4") + " Go / No go";
				}

			}

			return _dt;
		}
		public LabelValueInt[] revisions()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT revision value, CONCAT('v', revision) label FROM quote_master WHERE quote_id = @v0  ORDER BY revision", quote_id);
			//			this_string = "";
			//			try
			//			{
			//				foreach (DataRow _dr in _dt.Rows)
			//				{
			//					var selected = "";
			//					if (_dr["revision"].ToString() == revision)
			//					{
			//						selected = " selected";
			//					}
			//					else
			//					{
			//						selected = "";
			//					}
			//					this_string += string.Format("\n<option value='{0}'{1}>v{0}</option>", _dr["revision"], selected);
			//				}
			//			}
			//			catch
			//			{
			//				this_string = "";
			//			}
			//			return this_string;
		}

		public string GetfirstxCharacters(string s, int length)
		{
			// This says "If string s is less than 10 characters, return s.
			// Otherwise, return the first 10 characters of s."
			return s.Length < length ? s : s.Substring(0, length);
		}

		public string set_competitor_name()
		{
			this_string = Toolbox.doSQL_string(conn, @"SELECT name FROM quote_competitor  WHERE id =@v0", new object[] { competitor_id });
			return Toolbox.do_value_from(this_string);
		}
		public string CleanString(string s)
		{
			if (!string.IsNullOrEmpty(s))
			{
				var sb = new StringBuilder(s.Length);
				foreach (var c in s)
				{
					sb.Append(char.IsControl(c) ? ' ' : c);
				}
				s = sb.ToString();
			}
			return s;
		}
		public string field_title(string field_name)
		{
			var _title = "";
			if (!string.IsNullOrEmpty(field_name) && field_titles.ContainsKey(field_name))
			{
				try
				{
					_title = field_titles[field_name];
				}
				catch (Exception ee)
				{
					_title = field_name + " doesn't exist";
					_tools.catch_error(ee);
				}
			}
			else
			{
				_title = "Not defined";
			}
			return _title;
		}
		public string get_last_sent()
		{
			return Toolbox.doSQL_string(conn, @"SELECT DATE_FORMAT(last_fax_date, '%Y-%m-%d %H:%i:%s') last FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
		}
		public string update_last_printed()
		{
			var current_status_id = Toolbox.doSQL_int(conn, @"SELECT status_id FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
			var to_status_id = current_status_id;
			if (current_status_id == 1)
			{
				to_status_id = 2;
			}
			if (current_status_id == 1 && current_status_id != to_status_id)
			{
				Toolbox.doSQL_void(conn, @"UPDATE quote_master SET status_id = @v2 , last_print_date = now(), updated_by_page = 'quote - update_last_printed()' WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision, to_status_id });
			}
            else
            {
                Toolbox.doSQL_void(conn, @"UPDATE quote_master SET last_print_date = now(), updated_by_page = 'quote - update_last_printed()' WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
            }
			return Toolbox.doSQL_string(conn, @"SELECT DATE_FORMAT(last_print_date, '%Y-%m-%d %H:%i:%s') last FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
		}
		public string get_last_ticks()
		{
			var last_modified = Toolbox.doSQL_datetime(@"SELECT last_saved last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
			return last_modified.Ticks.ToString();
		}

		#endregion
		#region [DOUBLE] Methods (4)
		public double tm_pricing_total()
		{
			return Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM((IFNULL(A.original_sell, 0) * (1 - (quote_worksheet_discount/100))) * IFNULL(A.qty, 0)),0) AS extTandM FROM quote_worksheet A WHERE A.quote_id = @v0  AND A.revision = @v1 and ifnull(a.is_checked,1)=1 ", new object[] { quote_id, revision });
			//return Toolbox.doSQL_string(conn,@"SELECT CAST(IFNULL(FORMAT(SUM(qty*original_sell), 2), 0) AS CHAR) tm_pricing FROM quote_worksheet WHERE quote_id = @v0  AND revision = @v1 ", new object[] {  quote_id, revision } );
		}
		public double worksheet_total()
		{
			return WorksheetTotalSell(conn, quote_id, revision, TotalType.All);
			//return Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(A.extended_per), 0) AS extended_per FROM quote_worksheet A WHERE A.quote_id = @v0  AND A.revision = @v1  and ifnull(a.is_checked,1)=1 ", new object[] { quote_id, revision });
			//return Toolbox.doSQL_string(conn,@"SELECT SUM(extended_per) FROM quote_worksheet WHERE quote_id = @v0  AND revision = @v1 ", new object[] {  quote_id, revision } );
		}
		public enum TotalType
		{
			All,
			Material,
			Labor
		}
		public static double WorksheetTotalCost(MySqlConnection _conn, object _quote_id, object _revision, TotalType _type)
		{
			var cost = 0D;
			switch (_type)
			{
				case TotalType.All:
					cost = Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(cost*qty),0)  FROM quote_worksheet a LEFT JOIN quote_section b ON a.section_id = b.id WHERE a.quote_id = @v0 AND a.revision = @v1 AND IFNULL(a.is_checked,1)=1 AND IFNULL(b.is_checked, 1) = 1 ", new object[] { _quote_id, _revision });
					break;
				case TotalType.Labor:
					cost = Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(cost*qty),0)  FROM quote_worksheet a LEFT JOIN quote_section b ON a.section_id = b.id WHERE a.quote_id = @v0  AND a.revision = @v1  AND (a.part_no BETWEEN 990000 AND 2000000) AND IFNULL(a.is_checked,1)=1 AND IFNULL(b.is_checked, 1) = 1 ", new object[] { _quote_id, _revision });
					break;
				case TotalType.Material:
					cost = Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(cost*qty),0)  FROM quote_worksheet a LEFT JOIN quote_section b ON a.section_id = b.id WHERE a.quote_id = @v0  AND a.revision = @v1  AND (a.part_no < 990000 OR a.part_no >= 2000000) AND IFNULL(a.is_checked,1)=1 AND IFNULL(b.is_checked, 1) = 1 ", new object[] { _quote_id, _revision });
					break;
			}
			return cost;
		}
		public static double WorksheetTotalSell(MySqlConnection _conn, object _quote_id, object _revision, TotalType _type)
		{
			var sell = 0D;
			switch (_type)
			{
				case TotalType.All:
					sell = Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(extended_per),0) FROM quote_worksheet a LEFT JOIN quote_section b ON a.section_id = b.id WHERE a.quote_id = @v0 AND a.revision = @v1 AND IFNULL(a.is_checked,1)=1 AND IFNULL(b.is_checked, 1) = 1 ", new object[] { _quote_id, _revision });
					break;
				case TotalType.Labor:
					sell = Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(extended_per),0) FROM quote_worksheet a LEFT JOIN quote_section b ON a.section_id = b.id WHERE a.quote_id = @v0  AND a.revision = @v1  AND (a.part_no BETWEEN 990000 AND 2000000) AND IFNULL(a.is_checked,1)=1 AND IFNULL(b.is_checked, 1) = 1 ", new object[] { _quote_id, _revision });
					break;
				case TotalType.Material:
					sell = Toolbox.doSQL_double(_conn, @"SELECT IFNULL(SUM(extended_per),0) FROM quote_worksheet a LEFT JOIN quote_section b ON a.section_id = b.id WHERE a.quote_id = @v0  AND a.revision = @v1  AND (a.part_no < 990000 OR part_no >= 2000000) AND IFNULL(a.is_checked,1)=1 AND IFNULL(b.is_checked, 1) = 1 ", new object[] { _quote_id, _revision });
					break;
			}
			return sell;
		}
		public double tm_pricing_total(string section_id)
		{
			return Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(IFNULL(A.original_sell, 0) * IFNULL(A.qty, 0)),0) AS extTandM FROM quote_worksheet A WHERE A.quote_id = @v0  AND A.revision = @v1  and ifnull(a.is_checked,1)=1 ", new object[] { quote_id, revision });
			//return Toolbox.doSQL_string(conn,@"SELECT FORMAT(IFNULL(SUM(original_sell*qty), 0),2) total FROM quote_worksheet WHERE quote_id= @v0  AND revision = @v1  AND section_id = @v2 ", new object[] {  quote_id, revision, section_id } );
		}
		#endregion
		#region [BOOLEAN] Methods (21)
		public bool add_details()
		{
			if (details != null)
			{
				for (var d = 0; d < details.Length; d++)
				{
					var this_detail = Regex.Split(CleanString(details[d].Trim()), @"\]:\[");
					//_tools.debug_note("Quote - ID:"+this_detail[0]+" TEXT:"+this_detail[1]);
					try
					{
						Toolbox.doSQL_void(conn, @"UPDATE quote_extratext SET line_number = @v0 , linetext=@v1  WHERE id = @v2  LIMIT 1", new object[] { d, this_detail[1], this_detail[0] });
					}
					catch (Exception ee)
					{
						_tools.catch_error(ee);
					}
					if (this_detail[1] != "")
					{
						//_tools.debug_note(this_detail.Length);
						var temp_section_id = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(section_id), 0) FROM quote_extratext WHERE id = @v0 ", new object[] { this_detail[0] });
						if (temp_section_id > 0)
						{
							var step = d + 1;
							var detail_name =
								$"{step}. {Toolbox.do_value_from(this_detail[1], false).Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ")}";
							var len = detail_name.Length > 60 ? 60 : detail_name.Length;
							var temp_section_name = _tools.value_to(detail_name.Substring(0, len));
							var is_saved = save_section(temp_section_id, temp_section_name);
						}
					}
					/*
					try
						{
						_tools.getSQL_bool(@"INSERT INTO quote_extratext (quote_id, revision, type, line_number, linetext) VALUES (@v0 , @v1 , 1, @v2 , @v3 )", new object[] {  quote_id, revision, d, CleanString(details[d].Trim()) } );
						this_bool						= true;
						}
					catch
						{
						this_bool						= false;
						}
					 */
				}
				this_bool = true;
			}
			else
			{
				this_bool = true;
			}
			return this_bool;
		}
		public bool add_notes()
		{
			if (notes != null)
			{
				for (var n = 0; n < notes.Length; n++)
				{
					var this_note = Regex.Split(CleanString(notes[n].Trim()), @"\]:\[");
					Toolbox.doSQL_void(conn, @"UPDATE quote_extratext SET line_number = @v0 , linetext=@v1  WHERE id = @v2  LIMIT 1", new object[] { n, this_note[1], this_note[0] });
					/*
					try
						{
						_tools.getSQL_bool(@"INSERT INTO quote_extratext (quote_id, revision, type, line_number, linetext) VALUES (@v0 , @v1 , 2, @v2 , @v3 )", new object[] {  quote_id, revision, n, CleanString(notes[n].Trim()) } );
						this_bool						= true;
						}
					catch
						{
						this_bool						= false;
						}
					 */
					this_bool = true;
				}
			}
			else
			{
				this_bool = true;
			}
			return this_bool;
		}
		public bool clear_extras()
		{
			try
			{
				_tools.getSQL_bool(@"DELETE FROM quote_extratext WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
				this_bool = true;
			}
			catch
			{
				this_bool = false;
			}
			return this_bool;
		}
		public bool clear_kill_reasons()
		{
			return _tools.getSQL_bool(@"DELETE FROM quote_kill_reason_map WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
		}
		public bool del_section(string section_id)
		{
			try
			{
				_tools.getSQL_bool(@"DELETE FROM quote_worksheet WHERE section_id = @v0 ", new object[] { section_id });
				try
				{
					_tools.getSQL_bool(@"DELETE FROM quote_section WHERE id = @v0 ", new object[] { section_id });
					this_bool = true;
				}
				catch
				{
					this_bool = false;
				}
			}
			catch
			{
				this_bool = false;
			}
			return this_bool;
		}
		public bool del_worksheet_row(string row_id)
		{
			try
			{
				_tools.getSQL_bool(@"DELETE FROM quote_worksheet WHERE id = @v0 ", new object[] { row_id });
				this_bool = true;
			}
			catch
			{
				this_bool = false;
			}
			return this_bool;
		}
		public string duplicate_quote()
		{
			try
			{
				quote_id = Toolbox.doSQL_string(conn, @"CALL DuplicateQuote(@v0 , @v1 )", new object[] { quote_id, revision });
				revision = "1";
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return "";
			}
			return quote_id;
		}
		public bool insert_kill_reasons()
		{
			try
			{
				for (var i = 0; i <= reasons.Length; i++)
				{
					this_bool = _tools.getSQL_bool(@"INSERT INTO quote_kill_reason_map (quote_id, revision, reason_id) VALUES (@v0 , @v1 , @v2 )", new object[] { quote_id, revision, reasons[i] });
				}
			}
			catch
			{
				this_bool = false;
			}
			return this_bool;
		}
		public bool save()
		{
			if (Toolbox.doSQL_int(conn, @"SELECT status_id FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision }) == 8)
			{
				throw new Exception("Quote tried to save information after it was referenced on a workorder. Aborting");
			}
			if (status_id != "6" && status_id != "8" && status_id != "9" && status_id != "10" && status_id != "11" && status_id != "12" && status_id != "13")
			{
				if (last_print_date == "NULL" && last_fax_date == "NULL" && verified_date == "NULL")
				{
					status_id = "1";
				}
				else if (last_print_date != "NULL" && last_fax_date == "NULL" && verified_date == "NULL")
				{
					status_id = "2";
				}
				else if (last_fax_date != "NULL" && verified_date == "NULL")
				{
					status_id = "3";
				}
				else if (verified_date != "NULL")
				{
					status_id = "4";
				}
				else
				{
					status_id = "7";
				}

				if (status_id == "4")
				{
					Toolbox.doSQL_void(conn, @"UPDATE customer SET Customer_LastDateTime = NOW() WHERE customer_id = @v0  LIMIT 1", new object[] { customer_id });
					Toolbox.doSQL_void(conn, @" INSERT INTO customer_history ( customer_history_memberid, customer_history_date, customer_history_action, customer_history_notes, customer_history_custid ) VALUES ( @v0 , NOW(), 12, 'Quote: @v2 , @v1  )", new object[] { this_member.id, customer_id, quote_id });
				}
			}
			quoted_price = quoted_price.Replace(",", "").Replace("$", "");
			double try_price = 0;
			double to_price = 0;
			double.TryParse(quoted_price, out try_price);
			double.TryParse(price_to, out to_price);

			try
			{
				Toolbox.doSQL_void(conn, @" UPDATE quote_master SET date_due = @v1 , completion_date = @v19 , job_description = @v2 , cust_spec_doc = @v3 , quoted_by = @v4 , business_unit_id = @v18 , last_print_date = @v5 ,
updated_by_page = 'quote - save()', status_id = @v17 , last_fax_date = @v6 , verified_date = @v7 , address_id = @v21 , quoted_price = @v8 , price_to = @v16 , custom_term = @v9 , net_due = @v10 , percentage_down = @v11 , 
pricetype_id = @v12 , us_currency = @v13 , inflation_term = @v14 , include_title = @v20 , pct_chance = @v22 , pct_chance_reason = @v23 , currency = @v24 , exp_podate = @v25 ,period_valid = @v26  WHERE quote_id = @v0  AND revision = @v15  ",
					new object[] { quote_id, HttpUtility.UrlDecode(date_due),
						job_description, cust_spec_doc, quoted_by, HttpUtility.UrlDecode(last_print_date),
						HttpUtility.UrlDecode(last_fax_date), HttpUtility.UrlDecode(verified_date),
						try_price, custom_term, net_due, percent_down, pricetype_id, us_currency, inflation_term, revision, to_price, status_id, quoted_business_unit_id,
						completion_date, include_title, address_id, pct_chance, pct_chance_reason, currency, HttpUtility.UrlDecode(exp_podate),period_valid });

				add_details();
				add_notes();
				if (pct_chance_note.Trim() != "")
				{
					var wpn = new NeWoProgNotes(Convert.ToInt32(quote_id), "Q");
					wpn.woprog_project_notes_memberid = Convert.ToInt32(quoted_by);
					wpn.woprog_project_notes_notes = pct_chance_note.Replace("\"", "'");
					wpn.woprog_project_notes_woprogid = Convert.ToInt32(quote_id);
					wpn.SaveWOProgProjectNote();
				}
				this_bool = true;
			}
			catch (Exception ee)
			{
				_tools.catch_error(ee);
				this_bool = false;
			}
			return this_bool;
		}
		public bool save_competitor(string competitor)
		{
			this_bool = _tools.getSQL_bool(@"INSERT INTO quote_competitor (insert_date,name)  VALUES (now(),@v0)", new object[] { competitor });
			return this_bool;
		}
		public bool schedule_followup(string scheduled_date, string note)
		{
			this_bool = _tools.getSQL_bool(@" INSERT INTO quote_follow_up ( create_datetime, schedule_date, created_by, quote_id, revision, note, editted_by ) VALUES ( now(), @v2 , @v4 , @v0 , @v1 , @v3 , @v4  )", new object[] { quote_id, revision, scheduled_date, note, current_quoter });
			return this_bool;
		}
		public bool schedule_followup(string scheduled_date, string note, string _id)
		{
			this_bool = _tools.getSQL_bool(@" UPDATE quote_follow_up SET schedule_date = @v0 , note = @v1 , editted_by = @v2  WHERE id = @v3  ", new object[] { scheduled_date, note, current_quoter, _id });
			return this_bool;
		}
		/// <summary>
		/// For saving new sections
		/// </summary>
		/// <param name="section_name"></param>
		/// <param name="detail_id"></param>
		/// <returns></returns>
		public string save_section(string section_name, int detail_id) { return save_section(0, section_name, detail_id); }
		/// <summary>
		/// For saving existing sections
		/// </summary>
		/// <param name="section_id"></param>
		/// <param name="section_name"></param>
		/// <returns></returns>
		public string save_section(int section_id, string section_name) { return save_section(section_id, section_name, 0); }
		private string save_section(int section_id, string section_name, int detail_id)
		{
			var row_id = "";
			if (section_id == 0)
			{
				row_id = Toolbox.doSQL_return_id(@"INSERT INTO quote_section (quote_id, revision, section, detail_id) VALUES (@v0 , @v1 , @v2 , @v3 )", new object[] { quote_id, revision, section_name, detail_id }).ToString();
			}
			else
			{
				var picklist_controlled = Convert.ToBoolean(Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(picklist_controlled),0) FROM quote_section WHERE id = @v0 ", new object[] { section_id }));
				if (!picklist_controlled)
				{
					Toolbox.doSQL_void(conn, @"UPDATE quote_section SET section = @v0  WHERE id = @v1  LIMIT 1", new object[] { section_name, section_id });
				}
				row_id = section_id.ToString();
			}
			return row_id;
		}
		public bool status_update()
		{
			var msg = new NeEMail();
			_tools.current_user = this_member;
			_tools.page_author = new NeMember(711);
			//Toolbox.doSQL_void(conn,@"UPDATE quote_master SET competitor_id = @v0 , updated_by_page = 'quote - status_update()' WHERE quote_id = @v1  AND active_revision = true", new object[] {  competitor_id, quote_id } );
			prev_status_id = Toolbox.doSQL_string(conn, @"SELECT status_id FROM quote_master WHERE quote_id = @v0  AND active_revision = true", new object[] { quote_id });
			if (status_id == "6")
			{
				to_history("Quote was killed by " + this_member.FullName);
				this_string = string.Format("Quote #{0} (Quoted Price: {3}) for {2} was killed by {1}\n\nJob Description\n---------\n{4}", quote_id, this_member.FirstName + " " + this_member.LastName, customer_name, quoted_price, job_description);
				this_string += "\n\nWhy did we lose the job\n---------------------------------------------------------\n";
				if (why_lose.Length > 0)
				{
					this_string += why_lose + "\n";
				}
				this_string += "\n\nWho was our competitor\n---------------------------------------------------------\n";
				if (who_competitor.Length > 0)
				{
					this_string += who_competitor + "\n";
				}
				else
				{
					this_string += "Not Entered\n";
				}
				this_string += "\n\nWhat was the price that it went for\n---------------------------------------------------------\n";
				if (what_price.Length > 0)
				{
					this_string += what_price + "\n";
				}
				else
				{
					this_string += "Not Entered\n";
				}
				sql = string.Format(@"
UPDATE 
	quote_master 
SET 
	killed_by = {1}, 
	prev_status_id = status_id, 
	status_id = 6, 
	updated_by_page = 'quote - status_update()', 
	who_competitor = '{2}', 
	what_price = '{3}', 
	why_lose = '{4}'  
WHERE 
	quote_id = {0} AND 
	active_revision = true", quote_id, current_quoter, Toolbox.do_value_to(who_competitor), Toolbox.do_value_to(what_price), Toolbox.do_value_to(why_lose));

			}
			else
			{
				this_string = "";
				if (status_id == "")
				{
					status_id = "2";
				}
				sql = string.Format(@"
UPDATE 
	quote_master 
SET 
	prev_status_id = status_id, 
	status_id = {0}, 
	updated_by_page = 'quote - status_update()', 
	who_competitor ='{2}', 
	what_price = '{3}', 
	why_lose = '{4}' 
WHERE 
	quote_id = {1} AND 
	active_revision = true", status_id, quote_id, Toolbox.do_value_to(who_competitor), Toolbox.do_value_to(what_price), Toolbox.do_value_to(why_lose));
			}

			if (this_string != "")
			{
				msg.From = this_member.NEEmail;
				msg.To = this_branch_manager.NEEmail;
				msg.Subject = "Update on Quote #" + quote_id;				
				msg.Body = this_string.Replace("\n", "<br/>");
			}
			try
			{
				Toolbox.doSQL_void(conn, sql, null);
				if (this_string != "" && this_member.id != 711)
				{
					msg.Send();
				}
				else if (this_string != "" && this_member.id == 711)
				{
					msg.From = "mhyde@thatsnew.com";
					msg.To = "mhyde@thatsnew.com";
					msg.CC = "";
					msg.Send();
				}
				this_bool = true;
			}
			catch (Exception ee)
			{
				this_bool = false;
				_tools.catch_error(ee);
			}
		var l = new NELog
			        {
			        section_id = OpsLog.Section.QuoteGeneral,
			        action_id = OpsLog.Action.StatusOfQuoteChanged,
			        business_unit_id = quoted_business_unit_id,
			        member_id = this_member.id,
			        table = OpsLog.Table.QuoteMaster,
			        table_id = quote_id,
			        alt_table_id = revision,
			        value_old = prev_status_id,
			        value_new = status_id
			        };
		l.save();
			return this_bool;
		}
		public bool update_po_info(string type, string value)
		{
			if (type == "po")
			{
				value = "\"" + value + "\"";
			}
			else if (type == "wo" && value.Trim().Length == 0)
			{
				value = "NULL";
			}
			sql =
				$"UPDATE quote_master SET {type} = {value}, updated_by_page = 'quote - update_po_info()' WHERE quote_id = {quote_id} AND revision = {revision}";
			try
			{
				_tools.getSQL_bool(
					$"UPDATE quote_master SET {type} = @v0, updated_by_page = 'quote - update_po_info()' WHERE quote_id =@v1 AND revision =@v2", new object[] { value, quote_id, revision });
				this_bool = true;
			}
			catch (Exception ee)
			{
				_tools.debug_note(ee);
				this_bool = false;
			}
			return this_bool;
		}
		public bool update_reason(string reason_id, string checked_state)
		{
			if (checked_state == "0")
			{
				sql = string.Format("DELETE FROM quote_received_reason_map WHERE reason_id = {0} AND quote_id = {2} AND revision = {3}", reason_id, checked_state, quote_id, revision);
				try
				{
					_tools.getSQL_bool(@"DELETE FROM quote_received_reason_map WHERE reason_id = @v0  AND quote_id = @v2  AND revision = @v3 ", new object[] { reason_id, checked_state, quote_id, revision });
					this_bool = true;
				}
				catch
				{
					this_bool = false;
				}
			}
			else
			{
				sql = string.Format("INSERT INTO quote_received_reason_map (quote_id, revision, reason_id, checked) VALUES ({2}, {3}, {0}, {1})", reason_id, checked_state, quote_id, revision);
				try
				{
					_tools.getSQL_bool(@"INSERT INTO quote_received_reason_map (quote_id, revision, reason_id, checked) VALUES (@v2 , @v3 , @v0 , @v1 )", new object[] { reason_id, checked_state, quote_id, revision });
					this_bool = true;
				}
				catch
				{
					this_bool = false;
				}
			}

			return this_bool;
		}
		public bool update_section(string section_name, string section_id)
		{
			try
			{
				_tools.getSQL_bool(@"UPDATE quote_section SET section = @v0  WHERE id = @v1 ", new object[] { section_name, section_id });
				this_bool = true;
			}
			catch
			{
				this_bool = false;
			}
			return this_bool;
		}
		public bool worksheet_change_row_section(string row_id, string section_id)
		{
			return _tools.getSQL_bool(@"UPDATE quote_worksheet SET section_id = @v0  WHERE id = @v1 ", new object[] { section_id, row_id });
		}
		public bool worksheet_update(string this_row_id, string this_part_no, string this_description, string this_sell, string this_original_sell, string this_qty, string this_section_id, string this_code, string this_extended_per, string this_cost)
		{
			if (this_sell == "")
			{
				this_sell = "NULL";
			}
			if (this_qty == "")
			{
				this_qty = "NULL";
			}

			try
			{

				Toolbox.doSQL_void(conn, @" UPDATE quote_worksheet SET section_id = @v0 , 
part_no = @v1 , description = @v2 , sell = @v3 , 
original_sell = @v7 , qty = @v4 , code = @v6 , extended_per = @v8 , cost = @v9  WHERE id = @v5 ",
					new object[] { this_section_id, this_part_no, this_description,
						this_sell, this_qty, this_row_id, this_code, this_original_sell,
						this_extended_per, this_cost });
				this_bool = true;
			}
			catch (Exception ee)
			{
				throw ee;
				//this_bool			= false;  unreachable code
			}
			return this_bool;
		}
		#endregion
		#region [DATATABLE] Methods (9)
		public DataTable get_Companies()
		{
			return Toolbox.doSQL_dt(conn, @"SELECT id, name name from business_unit  WHERE enable_timesheet = 1 ORDER BY name", null);
		}
		public LabelValueInt[] get_Contacts(string cust_id)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@" SELECT b.contact_id value, b.contact_name label FROM customer a LEFT JOIN contact b ON a.customer_id = b.contact_cust_id WHERE a.customer_id = @v0  AND b.contact_status = 'Active' AND b.contact_type = 'Customer' ORDER BY contact_name", cust_id);
		}

		public LabelValueInt[] get_Contacts(string cust_id, string add_id)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@" SELECT b.contact_id value, b.contact_name label FROM customer 
a LEFT JOIN contact b ON a.customer_id = b.contact_cust_id WHERE a.customer_id = @v0  AND b.contact_status = 'Active' AND b.contact_type = 'Customer'
ORDER BY contact_name", cust_id);
		}

		public DataTable get_Customers(string q)
		{
			return Toolbox.doSQL_dt(conn, @" SELECT a.customer_id c_id, a.customer_number c_number, a.customer_name c_name, customer_hold c_hold FROM customer a WHERE (a.customer_id like  CONCAT('%',@v0,'%')  OR a.customer_name like  CONCAT('%',@v0,'%') ) AND Customer_Status != 4 ORDER BY c_name", new object[] { q });
		}
		public List<LabelValueInt> get_Quoters(object _id)
		{
			return quoter_locked_bool ?
				bllToolbox.doSQL_List<LabelValueInt>(@" SELECT a.member_id value, a.member_fullname label FROM member a WHERE a.member_id = @v0  LIMIT 1", quoted_by)
				:
				bllToolbox.doSQL_List<LabelValueInt>(@" SELECT a.member_id value, a.member_fullname label FROM member a LEFT JOIN memberpage b ON a.member_id = b.memberpage_member_id WHERE a.business_unit_id = @v0  AND b.memberpage_page_id = 65 AND a.member_status = 'Active' ORDER BY a.member_fullname", _id);
		}

        public  List<LabelValueInt> Get_RevenueLine()
        {
            var o = bllToolbox.doSQL_List<LabelValueInt>(@"SELECT Distinct b.revenue_line_id value, b.Name label FROM revenue_line_business_unit a LEFT JOIN revenue_line b ON a.revenue_line_id = b.revenue_line_id");
            return o;
        }

        public List<LabelValueInt> Get_RevenueLine(object bu_id)
        {
            var o = bllToolbox.doSQL_List<LabelValueInt>(@"SELECT Distinct b.revenue_line_id value, b.Name label FROM revenue_line_business_unit a LEFT JOIN revenue_line b ON a.revenue_line_id = b.revenue_line_id  WHERE business_unit_id=@v0 ", bu_id);
            return o;
        }

        public DataTable get_Section(string section_id, bool include_price)
		{
			if (include_price)
			{
				return Toolbox.doSQL_dt(conn, @" SELECT IFNULL(TRIM(part_no), '--') part_n, IFNULL(TRIM(code), '--') code, IFNULL(TRIM(description), '--') description, qty, FORMAT(sell, 2) sell, FORMAT(original_sell, 2) tm_sell, FORMAT(original_sell * qty, 2) tm_extd, FORMAT(extended_per, 2) total FROM quote_worksheet WHERE quote_id = @v0  AND revision = @v1  AND section_id = @v2  ORDER BY CAST(part_no AS UNSIGNED), total ASC", new object[] { quote_id, revision, section_id });
			}
			else
			{
				return Toolbox.doSQL_dt(conn, @" SELECT IFNULL(TRIM(part_no), '--') part_n, IFNULL(TRIM(code), '--') code, IFNULL(TRIM(description), '--') description, qty, FORMAT(sell, 2) sell, FORMAT(original_sell, 2) tm_sell, FORMAT(original_sell * qty, 2) tm_extd, FORMAT(extended_per, 2) total FROM quote_worksheet WHERE quote_id = @v0  AND revision = @v1  AND section_id = @v2  AND code NOT LIKE 'LB%' AND part_no NOT IN (SELECT master_id FROM inventory_item_master WHERE tag_id = 571) ORDER BY CAST(part_no AS UNSIGNED), total ASC", new object[] { quote_id, revision, section_id });
			}

		}
		public DataTable get_Sections()
		{
			sql = $@"
SELECT
	*
FROM 
	quote_section
WHERE 
	quote_id = {quote_id} AND 
	revision = {revision}
ORDER BY
	section";
			return Toolbox.doSQL_dt(conn, @" SELECT * FROM quote_section WHERE quote_id = @v0  AND revision = @v1  ORDER BY section", new object[] { quote_id, revision });
		}
		public DataTable get_Worksheet()
		{
			sql = $@"
SELECT
	b.id,
	b.section_id,
	b.part_no,
	ifnull(b.original_sell, 0) original_sell,
	b.code,
	b.description,
	b.sell,
	b.extended_per,
	b.qty,
	b.cost
FROM 
	quote_worksheet b 
WHERE 
	b.quote_id = {quote_id} AND 
	b.revision = {revision}
ORDER BY
	section_id";
			return Toolbox.doSQL_dt(conn, @" SELECT b.id, b.section_id, b.part_no, ifnull(b.original_sell, 0) original_sell, b.code, b.description, b.sell, b.extended_per, b.qty, b.cost FROM quote_worksheet b WHERE b.quote_id = @v0  AND b.revision = @v1  ORDER BY section_id", new object[] { quote_id, revision });
		}
		public DataTable get_followup_history()
		{
			sql = $@"
SELECT * FROM
	(
	SELECT
		CAST(CONCAT('f',id) AS CHAR) id,
		CAST(schedule_date AS CHAR) date,
		b.member_fullname name,
		'f' type,
		a.is_done,
		a.done_datetime,
		note
	FROM 
		quote_follow_up a
	LEFT JOIN
		member b ON a.editted_by = b.member_id
	WHERE 
		quote_id = {quote_id} AND 
		revision = {revision}
UNION
	SELECT
		CAST(CONCAT('h',id) AS CHAR) id,
		CAST(DATE_FORMAT(create_datetime, '%Y-%m-%d - %l:%i:%s%p') AS char) date,
		b.member_fullname name,
		'h' type,
		0 is_done,
		NOW() done_datetime,
		event note
	FROM 
		quote_history a 
	LEFT JOIN
		member b ON
			a.created_by = b.member_id
	WHERE 
		quote_id = {quote_id} AND 
		revision = {revision}
UNION
	SELECT
		CAST(CONCAT('l',a.id) AS CHAR) id,
		CAST(DATE_FORMAT(a.dt, '%Y-%m-%d - %l:%i:%s%p') AS CHAR) date,
		e.member_fullname name,
		's' type,
		0 is_done,
		NOW() done_datetime,
		CAST(CONCAT(b.name, "" from: '"", c.status,""' to '"", d.status, ""'"") AS CHAR) note
	FROM 
		log a
	LEFT JOIN
		log_action b 
			ON a.action_id = b.id
	LEFT JOIN
		quote_status c
			ON a.value_old = c.id
	LEFT JOIN
		quote_status d
			ON a.value_new = d.id
	LEFT JOIN
		member e ON a.member_id = e.member_id
	WHERE 
		a.associated_table = 'quote_master' AND
		a.associated_table_id = '{quote_id}' AND
		a.associated_alt_table_id = '{revision}' AND
		a.action_id = 4
UNION
	SELECT 
		CAST(CONCAT('n',a.woprog_project_notes_id) AS CHAR) id,
		CAST(DATE_FORMAT(a.woprog_project_notes_notes, '%Y-%m-%d - %l:%i:%s%p') AS CHAR) date,
		b.member_fullname name,
		'n' type,
		0 is_done,
		NOW() done_datetime,
		woprog_project_notes_notes note
	FROM
		woprog_project_notes a
	LEFT JOIN
		member b ON a.woprog_project_notes_memberid = b.member_id
	where 
		woprog_project_notes_Type = 'Q' AND 
		woprog_project_notes_woprogid = {quote_id}
	) history ORDER BY date DESC;
	
";
			return Toolbox.doSQL_dt(conn, @" SELECT * FROM ( SELECT CAST(CONCAT('f',id) AS CHAR) id,
CAST(schedule_date AS CHAR) date, 
b.member_fullname name, 'f' type, 
		a.is_done,
		a.done_datetime,
note 
FROM quote_follow_up a LEFT JOIN member b ON a.editted_by = b.member_id
WHERE quote_id = @v0  AND revision = @v1  
UNION 
SELECT CAST(CONCAT('h',id) AS CHAR) id, 
CAST(DATE_FORMAT(create_datetime, '%Y-%m-%d - %l:%i:%s%p') AS char) date,
b.member_fullname name, 'h' type, 
		0 is_done,
		NOW() done_datetime,
event note 
FROM quote_history a LEFT JOIN member b ON a.created_by = b.member_id 
WHERE quote_id = @v0  AND revision = @v1 
UNION 
SELECT CAST(CONCAT('l',a.id) AS CHAR) id, 
CAST(DATE_FORMAT(a.dt, '%Y-%m-%d - %l:%i:%s%p') AS CHAR) date, 
e.member_fullname name, 's' type, 
		0 is_done,
		NOW() done_datetime,
CAST(CONCAT(b.name, "" from: '"", c.status,""' to '"", d.status, ""'"") AS CHAR) note 
FROM log a LEFT JOIN log_action b ON a.action_id = b.id 
LEFT JOIN quote_status c ON a.value_old = c.id 
LEFT JOIN quote_status d ON a.value_new = d.id 
LEFT JOIN member e ON a.member_id = e.member_id
WHERE a.associated_table = 'quote_master' AND a.associated_table_id = @v0 
AND a.associated_alt_table_id = @v1  AND a.action_id = 4 
UNION 
SELECT CAST(CONCAT('n',a.woprog_project_notes_id) AS CHAR) id,
CAST(DATE_FORMAT(a.woprog_project_notes_notes, '%Y-%m-%d - %l:%i:%s%p') AS CHAR) date, 
b.member_fullname name,
'n' type, 
		0 is_done,
		NOW() done_datetime,
woprog_project_notes_notes note 
FROM woprog_project_notes a LEFT JOIN member b ON a.woprog_project_notes_memberid = b.member_id 
where woprog_project_notes_Type = 'Q' AND woprog_project_notes_woprogid = @v0  ) history
ORDER BY date DESC ", new object[] { quote_id, revision });
		}

		public DataTable get_Worksheet(string this_section_id)
		{
			sql = $@"
SELECT
	b.id,
	b.section_id,
	b.part_no,
	b.code,
	ifnull(b.original_sell, 0) original_sell,
	b.description,
	b.sell,
	b.extended_per,
	b.qty,
	b.cost
FROM 
	quote_section a
LEFT JOIN
	quote_worksheet b ON a.id = b.section_id
WHERE 
	a.quote_id = {quote_id} AND 
	a.revision = {revision} AND
	b.section_id = {this_section_id}
ORDER BY
	section_id";
			return Toolbox.doSQL_dt(conn, @" SELECT b.id, b.section_id, b.part_no, b.code, ifnull(b.original_sell, 0) original_sell, b.description, b.sell, b.extended_per, b.qty, b.cost FROM quote_section a LEFT JOIN quote_worksheet b ON a.id = b.section_id WHERE a.quote_id = @v0  AND a.revision = @v1  AND b.section_id = @v2  ORDER BY section_id", new object[] { quote_id, revision, this_section_id });
		}
		public DataTable search_Quotes(string this_description, string this_quoted_by, string this_customer, string this_before, string this_after, string this_status, string this_quote_id, string this_business_unit_id)
		{
			var appendage = "";
			sql = @"
SELECT 
	a.quote_id, 
	(select max(g.revision) FROM quote_master g WHERE quote_id = a.quote_id ) revision, 
	a.open_date,
	ifnull(REPLACE(a.job_description, CHAR(0), ' '), '--') job_description,
	ifnull(b.customer_name, '--') customer_name,
	'--' phone,
	a.quoted_price,
	ifnull(e.member_fullname, '--') quoted_by,
	ifnull(c.contact_name, '--') contact_name,
	f.status
FROM 
	quote_master a
LEFT JOIN
	customer b ON a.customer_id = b.customer_id
LEFT JOIN
	contact c ON a.contact_id = c.contact_id
LEFT JOIN
	member e ON a.quoted_by = e.member_id
LEFT JOIN
	quote_status f ON a.status_id = f.id";
			//---------------------------------
			if (this_quote_id.Trim() != "")
			{
				if (appendage != "")
				{
					appendage += @"
AND";
				}
				appendage += @"
a.quote_id LIKE ""%" + this_quote_id + @"%""";
			}
			//---------------------------------
			if (this_description.Trim() != "")
			{
				if (appendage != "")
				{
					appendage += @"
AND";
				}
				appendage += @"
a.job_description LIKE ""%" + this_description + @"%""";
			}
			//---------------------------------
			if (this_customer.Trim() != "")
			{
				if (appendage != "")
				{
					appendage += @"
AND";
				}
				appendage += @"
b.customer_name LIKE ""%" + this_customer + @"%""";
			}
			//---------------------------------
			if (this_quoted_by != "0")
			{
				if (appendage != "")
				{
					appendage += @"
AND";
				}
				appendage += @"
a.quoted_by = " + this_quoted_by;
			}
			//---------------------------------
			if (this_business_unit_id != "0")
			{
				if (appendage != "")
				{
					appendage += @"
AND";
				}
				appendage += @"
a.business_unit_id = " + this_business_unit_id;
			}
			//---------------------------------
			if (this_before.Trim() != "")
			{
				if (appendage != "")
				{
					appendage += @"
AND";
				}
				appendage += @"
a.open_date < '" + this_before + @"'";
			}
			//---------------------------------
			if (this_after.Trim() != "")
			{
				if (appendage != "")
				{
					appendage += @"
AND";
				}
				appendage += @"
a.open_date > '" + this_after + @"'";
			}
			//---------------------------------
			if (this_status != "0")
			{
				if (appendage != "")
				{
					appendage += @"
AND";
				}
				if (this_status == "6_8")
				{
					appendage += @"
(a.status_id = 6 OR a.status_id = 8)";
				}
				else if (this_status == "OPEN")
				{
					appendage += @"
(a.status_id = 2 OR a.status_id = 3 OR a.status_id = 4)";
				}
				else
				{
					appendage += @"
a.status_id = " + this_status;
				}
			}
			//---------------------------------
			if (appendage != "")
			{
				appendage = @"
			WHERE active_revision = true AND " + appendage;
			}
			sql = sql + appendage + @"
ORDER BY a.quote_id DESC,a.revision DESC LIMIT 1000";
			return Toolbox.doSQL_dt(conn, sql, null);
		}
		public DataTable group_totals()
		{
			return Toolbox.doSQL_dt(conn, @"SELECT a.id, IFNULL(SUM(b.extended_per),0) total FROM quote_section a LEFT JOIN quote_worksheet b ON a.id = b.section_id WHERE a.quote_id = @v0  AND a.revision = @v1  GROUP by id", new object[] { quote_id, revision });
		}
		#endregion
		public bool update_expected_value()
		{
			var temp_status = 99;

			var comp = new NeBusinessUnit(business_unit_id);
			if (comp.uses_quote_process == 1)
			{
				var old_exp_value = Toolbox.doSQL_double(conn, @"Select ifnull(expected_value,0) from quote_master  WHERE quote_id =@v0 AND revision =@v1  limit 1 ", new object[] { quote_id, revision });

				if (old_exp_value < comp.quote_level_2_start)
				{
					if (expected_value >= comp.quote_level_2_start)
					{
						temp_status = 10;
					}
				}

				if (old_exp_value > comp.quote_level_2_start && expected_value < comp.quote_level_2_start)
				{
					temp_status = 1;
					Toolbox.doSQL_void(conn, @"Delete from quote_schedule  where quoteid =@v0 and revision =@v1  limit 1 ", new object[] { quote_id, revision });

				}

				if (old_exp_value >= comp.quote_level_2_start && expected_value > comp.quote_level_3_start)
				{
					temp_status = 10;
				}

			}
			if (temp_status == 99)
			{
				Toolbox.doSQL_void(conn, @"UPDATE quote_master SET expected_value = @v2  WHERE quote_id = @v0  AND revision = @v1  LIMIT 1", new object[] { quote_id, revision, expected_value });
				return false;
			}
			else
			{
				Toolbox.doSQL_void(conn, @"UPDATE quote_master SET expected_value = @v2 , status_id = @v3  WHERE quote_id = @v0  AND revision = @v1  LIMIT 1", new object[] { quote_id, revision, expected_value, temp_status });
				return true;
			}


			//		System.Web.HttpResponse.Redirect("./index.aspx?a=g&quote_id=" + quote_id + "&revision=" + revision);


		}
	}
}