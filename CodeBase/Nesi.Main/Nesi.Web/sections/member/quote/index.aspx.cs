using System;
using System.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Text.RegularExpressions;
using DevExpress.Web;
using System.IO;
using System.Web.UI.WebControls;
using DevExpress.DataProcessing;
using MySql.Data.MySqlClient;
using nesi.core;

public partial class Quoting : Page
{
	NeMember current_user;
	Toolbox _tools;
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(65);
		_tools.page_author = new NeMember(711);
		_tools.dont_cache_page();
		_tools.current_user = current_user;
	}
	protected void Page_Load(object sender, EventArgs e)
	{

		var _q = Request.QueryString;
		if (!Request.Url.Host.Contains("luke"))
		{
			Response.Redirect("/#/opens/65/quotes/" + _q["quote_id"] + "/" + _q["revision"]);
		}
		//  Toolbox.RedirectToN2(Response, Request, 65, "Quote Open", "opens/65/quotes/" + _q["quote_id"] + "/" + _q["revision"]);

		using (var conn = Toolbox.connect())
		{
			if (!IsCallback)
			{
				#region VARIABLES
				var cultureInfo = new CultureInfo("en-US");
				var textInfo = cultureInfo.TextInfo;

				// I shortened these for ease of typing
				//var _q = Request.QueryString;
				var _f = Request.Form;

				var quote_inventory = new inventory();
				var _quote = new NeQuote();
				_quote.conn = conn;
				_quote.textinfo = textInfo;
				_quote.quoted_by = current_user.id.ToString();
				_quote.current_quoter = current_user.id.ToString();
				_quote.business_unit_id = current_user.business_unit_id.ToString();
				_quote.this_member = current_user;


				var _company_obj = new NeBusinessUnit(current_user.business_unit_id);

				DataTable _dt;
				var check_numeric = new Regex("[^0-9]");
				var q = "";
				var Output = "";
				var action = "";
				var row_id = "";
				var returned_string = "";
				#endregion VARIABLES
				switch (Request.RequestType)
				{
					#region POST
					case "POST":
						if (_f["a"] != null)
						{
							action = _f["a"];
						}
						switch (action)
						{
							#region SAVE
							case "save":
								if (_f["customer_id"] != "")
								{
									_quote.quote_id = _f["quote_id"];
									_quote.revision = _f["revision"];
									try
									{
										#region customer_id check
										if (_f["customer_id"] != "")
										{
											_quote.customer_id = _f["customer_id"];
										}
										#endregion customer_id check
										#region include title check
										if (_f["include_title"] != "")
										{
											_quote.include_title = _f["include_title"];
										}
										#endregion include title check
										#region contact_id check
										if (_f["contact_id"] != "")
										{
											_quote.contact_id = _f["contact_id"];
										}
										#endregion contact_id check
										#region date_due check
										if (_f["date_due"] != "")
										{
											_quote.date_due = "'" + _f["date_due"] + "'";
										}
										else
										{
											_quote.date_due = "NULL";
										}
										#endregion date_due check
										#region completion_date check
										if (_f["completion_date"] != "")
										{
											_quote.completion_date = "'" + _f["completion_date"] + "'";
										}
										else
										{
											_quote.completion_date = "NULL";
										}
										#endregion completion_date check
										#region job_description check
										if (_f["job_description"] != "")
										{
											_quote.job_description = Toolbox.do_value_from(_f["job_description"], false);
										}
										#endregion job_description check
										#region cust_spec_doc check
										if (_f["cust_spec_doc"] != "")
										{
											_quote.cust_spec_doc = _f["cust_spec_doc"];
										}
										#endregion cust_spec_doc check
										#region quoted_by check
										if (_f["quoted_by"] != "" && _f["quoted_by"] != "0")
										{
											_quote.quoted_by = _f["quoted_by"];
										}
										else
										{
											_quote.quoted_by = current_user.id.ToString();
										}
										#endregion quoted_by check
										#region last_print_date check
										if (_f["last_print_date"] != "")
										{
											_quote.last_print_date = "'" + _f["last_print_date"] + "'";
										}
										else
										{
											_quote.last_print_date = "NULL";
										}
										#endregion last_print_date check
										#region last_fax_date check
										if (_f["last_fax_date"] != "")
										{
											_quote.last_fax_date = "'" + _f["last_fax_date"] + "'";
										}
										else
										{
											_quote.last_fax_date = "NULL";
										}
										#endregion last_fax_date check
										#region verified_date check
										if (_f["verified_date"] != "")
										{
											_quote.verified_date = "'" + _f["verified_date"] + "'";
										}
										else
										{
											_quote.verified_date = "NULL";
										}
										#endregion verified_date check
										#region quoted_price check
										if (_f["quoted_price"] != "")
										{
											_quote.quoted_price = _f["quoted_price"];
										}
										#endregion quoted_price check
										#region price_to check
										if (_f["price_to"] != "")
										{
											_quote.price_to = _f["price_to"];
										}
										#endregion price_to check
										#region quoted_pricetype check
										if (_f["quoted_pricetype"] != "")
										{
											_quote.pricetype_id = _f["quoted_pricetype"];
										}
										#endregion quoted_pricetype check
										#region us_currency check
										if (_f["us_currency"] != "")
										{
											_quote.us_currency = _f["us_currency"];
											_quote.currency = _f["us_currency"] == "1" || _f["us_currency"] == "checked" ? 1 : 2;
										}
										#endregion us_currency check
										#region status_id check
										if (_f["status_id"] != "")
										{
											_quote.status_id = _f["status_id"];
										}
										#endregion status_id check
										#region inflation_term check
										if (_f["inflation_term"] != "")
										{
											_quote.inflation_term = _f["inflation_term"];
										}
										#endregion inflation_term check
										#region percent_down check
										if (_f["percent_down"] != "")
										{
											_quote.percent_down = _f["percent_down"];
										}
										#endregion percent_down check
										#region percent_down check
										if (_f["net_due"] != "")
										{
											_quote.net_due = _f["net_due"];
										}
										else
										{
											_quote.net_due = "4";
										}
										#endregion percent_down check
										#region custom_term check
										if (_f["custom_term"] != "")
										{
											_quote.custom_term = _f["custom_term"];
										}
										#endregion custom_term check
										#region details[] check
										if (_f["details[]"] != "")
										{
											_quote.details = _f["details[]"].Split(',');
										}
										#endregion details[] check
										#region notes[] check
										if (_f["notes[]"] != "")
										{
											_quote.notes = _f["notes[]"].Split(',');
										}
										#endregion notes[] check
										#region quoted_company check
										if (_f["quoted_company"] != "")
										{
											_quote.quoted_business_unit_id = _f["quoted_company"];
										}
										#endregion quoted_company check
										#region address id check
										if (_f["address_id"] != "")
										{
											_quote.address_id = _f["address_id"];
										}
										#endregion
										#region percent chance check
										if (_f["pct_chance"] != "")
										{
											_quote.pct_chance = Convert.ToInt32(_f["pct_chance"]);
										}
										#endregion
										#region percent chance reason check
										if (_f["pct_chance_reason"] != "")
										{
											_quote.pct_chance_reason = _f["pct_chance_reason"];
										}
										#endregion
										#region percent chance reason check
										if (_f["pct_chance_note"] != "")
										{
											_quote.pct_chance_note = _f["pct_chance_note"];
										}
										#endregion
										_quote.save();
										Response.Write("SUCCESS");
									}
									catch (Exception ee)
									{
										Response.Write("FAILED" + ee.Message);
									}
								}
								else
								{
									Response.Write("Customer Is Blank");
								}
								Response.End();
								break;
							#endregion SAVE
							#region DEFAULT
							/* 
					 Threw this in here for anything that might try posting a NON-actioned POST.
					 */
							default:
								Response.Clear();
								Response.Write("FAILED");
								Response.End();
								break;
								#endregion DEFAULT
						}
						break;
					#endregion POST
					#region GET
					case "GET":
						#region action assignment
						if (_q["a"] != null)
						{
							action = _q["a"];
						}
						else
						{
							action = "new";
						}
						if (action.Contains("xml"))
						{
							_tools.set_XML_header();
						}
						#endregion action assignment
						if (_q["a"] != null)
						{
							switch (action)
							{
								#region PAGES
								#region g: Get Quote
								case "g":
									if (_q["quote_id"] != null && _q["revision"] != null)
									{
										if (Convert.ToInt32(_q["quote_id"]) == 0)
										{
											Toolbox.FriendlyException(Response, "Quote # not supplied - If you are cutting a quote please remember each step you just did and cut a ticket with each step detailed.", "");
										}
										else if (Convert.ToInt32(_q["quote_id"]) < 100000)
										{
											Toolbox.FriendlyException(Response, "This quote was cut using the ACCESS based quote program, not the NESI quote program... all NESI quotes have a quote number greater than 100000.", "");
										}
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];


										//Response.Redirect("/#/opens/65/quotes/" + _quote.quote_id + "/" + _quote.revision);


										_quote.InitializeQuote();
										if (_quote.wo != "" && _quote.status_id == "8")
										{
											// Check if this quote is used on a work order
											var c_wo = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM woprog WHERE woprog_quoteid =@v0 ", new object[] { _quote.quote_id + _quote.revision });
											if (c_wo == 0)
											{
												// Unreceive it.
												Toolbox.doSQL_void(conn, @"UPDATE quote_master SET status_id = 4, wo = null WHERE quote_id = @v0  AND revision = @v1 ", new object[] { _quote.quote_id, _quote.revision });
												_quote.InitializeQuote();
											}
										}
										if ((new Current_User().visible_business_units).Split(',').IndexOf(_quote.business_unit_id) < 0)
										{
											Toolbox.FriendlyException(Response, "This quote is from a different business unit than your's - A privilege is required for access to other business units.", "");
										}

										var comp = new NeBusinessUnit(_quote.business_unit_id);
										#region quote process stage 1 approval
										var bm = comp.branch_manager;

										if (bm.id == 0)
										{
											bm = new NeMember(37);
										}

										if (_quote.status_id == "10") // if we are waiting for stage 1
										{
											if (_quote.expected_value >= comp.quote_level_3_start)  // if its a level 3 quote
											{
												if (NeMember.is_supervisor(Convert.ToInt32(_quote.quoted_by), current_user.id) || comp.branch_manager.id == current_user.id || NeMember.is_supervisor(comp.branch_manager.id, current_user.id))
												{
													iframe_approval.Attributes["src"] = "approval_frame.aspx?qid=" + _quote.quote_id;
													popstage1.ShowOnPageLoad = true;
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
													Toolbox.FriendlyPopup(HttpContext.Current.Response, "This quote has not been approved to be worked on yet!  Stage 1 approval by " + comp.branch_manager.FullName + " is required before you can start to work on this.", "", "Waiting for Quote LEVEL 3 Authorization");
												}
											}
											else if (_quote.expected_value >= comp.quote_level_2_start)  //else if its a level 2 quote
											{
												if (current_user.id == bm.id || NeMember.is_supervisor(bm.id, current_user.id))
												{
													iframe_approval.Attributes["src"] = "approval_frame.aspx?qid=" + _quote.quote_id;
													popstage1.ShowOnPageLoad = true;
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
													Toolbox.FriendlyPopup(HttpContext.Current.Response, "This quote has not been approved to be worked on yet!  Stage 1 approval by " + bm.FullName + " is required before you can start to work on this.", "", "Waiting for Quote LEVEL 2 Authorization");
												}
											}
										}
										else if (_quote.status_id == "13")
										{
											//post_mortem1.quote_id = Convert.ToInt32(_quote.quote_id);
											//post_mortem1.DataBind();
											//NeQuoteSchedule qs = new NeQuoteSchedule(_quote.quote_id.ToString());
											//if (current_user.id == qs.post_mortem_complete_mid || (qs.post_mortem_complete_mid == 0 && NeMember.is_supervisor(Convert.ToInt32(_quote.quoted_by), current_user.id)))
											//{
											//	poppost_mortem.ShowOnPageLoad = true;
											//}
										}
										else if (_quote.status_id == "6")
										{
											post_mortem1.quote_id = Convert.ToInt32(_quote.quote_id);
											post_mortem1.DataBind();
										}


										#endregion

										_tools.dont_cache_page();
										Output = _quote.Container();
									}
									else
									{
										Response.Redirect("./frame.aspx");
									}
									break;
								#endregion
								#endregion PAGES
								#region AUTOCOMPLETERS
								#region ac_customers
								case "ac_customers":
									if (_q["q"] != null)
									{
										q = _q["q"];
										_dt = _quote.get_Customers(q);
										_tools.dont_cache_page();
										if (_dt.Rows.Count > 0)
										{
											Response.Clear();
											Response.ContentType = "text/plain";
											foreach (DataRow _dr in _dt.Rows)
											{
												var c_id = _dr["c_id"].ToString();
												var c_number = _dr["c_number"].ToString();
												var c_name = _dr["c_name"].ToString();
												var c_hold = _dr["c_hold"].ToString() == "T" ? "On Hold" : "Active";
												Response.Write("(" + c_number + ") " + c_name + " - " + c_hold + "|" + c_id + "\n");
											}
											Response.End();
										}
										else
										{
											Response.Clear();
											Response.ContentType = "text/plain";
											Response.End();
										}
									}
									else
									{
										Response.Redirect("./frame.aspx");
									}
									break;
								#endregion
								#region ac_competitors
								case "ac_competitors":
									if (_q["q"] != null)
									{
										Response.Clear();
										Response.ContentType = "text/plain";
										q = _q["q"];
										var competitors = Toolbox.doSQL_dt(conn, @"SELECT * FROM quote_competitor  WHERE name LIKE CONCAT('%',@v0,'%')", new object[] { q });
										if (competitors.Rows.Count > 0)
										{
											foreach (DataRow competitor in competitors.Rows)
											{
												var ccmpetitor_id = competitor["id"].ToString();
												var competitor_name = HttpUtility.UrlDecode(competitor["name"].ToString());
												Response.Write(competitor_name + "|" + ccmpetitor_id + "\n");
											}
										}
										else
										{
											Response.Write("FAILED");
										}
										Response.End();
									}
									else
									{
										Response.Redirect("./frame.aspx");
									}
									break;
								#endregion
								#endregion AUTOCOMPLETERS
								#region XML HANDLERS
								#region xml_openquotes
								case "xml_openquotes":
									object customer_id = _q["customer_id"];
									object quote_id = "";
									if (_q["quote_id"] != null)
									{
										quote_id = _q["quote_id"];
										_dt = Toolbox.doSQL_dt(conn, @"SELECT quote_id, revision, business_unit_id, business_unit_name(business_unit_id) name, quoted_by member_id, get_name(quoted_by) member_name, date_format(last_print_date, '%m/%d/%Y') last_print_date, IF(job_description = '', 'No Description', urldecode(job_description)) job_description FROM quote_master WHERE customer_id = @v0  AND status_id IN (1, 2, 3, 4) AND quoted_by != 0 AND quote_id != @v1  ORDER BY open_date DESC", new object[] { customer_id, quote_id });
									}
									else
									{
										_dt = Toolbox.doSQL_dt(conn, @"SELECT quote_id, revision, business_unit_id, business_unit_name(business_unit_id) name, quoted_by member_id, get_name(quoted_by) member_name, date_format(last_print_date, '%m/%d/%Y') last_print_date, IF(job_description = '', 'No Description', urldecode(job_description)) job_description FROM quote_master WHERE customer_id = @v0  AND status_id IN (1, 2, 3, 4) AND quoted_by != 0 ORDER BY open_date DESC", new object[] { customer_id });
									}
									_dt.TableName = "quote";
									Response.Clear();
									_dt.WriteXml(Response.OutputStream);
									Response.End();
									break;
								#endregion xml_openquotes
								#region xml_contacts
								case "xml_contacts":
									if (_q["cust_id"] != null)
									{
										var cust_id = _q["cust_id"];
										_dt = _quote.get_Contacts(cust_id);
										if (_dt.Rows.Count > 0)
										{
											Response.Write(@"
<contacts>");
											foreach (DataRow _dr in _dt.Rows)
											{
												var contact_id = _dr["contact_id"].ToString();
												var contact_name = Toolbox.do_value_from(_dr["contact_name"]);
												Response.Write(string.Format(@"
	<contact id=""{0}"" name=""{1}""/>", contact_id, contact_name));
											}
											Response.Write(@"
</contacts>");
											Response.End();
										}
										else
										{
											Response.Redirect("./frame.aspx");
										}
									}
									else
									{
										Response.Redirect("./frame.aspx");
									}
									break;
								#endregion
								#region xml_addresses
								case "xml_addresses":
									if (_q["customer_id"] != null)
									{
										var cust_id = _q["customer_id"];
										var _addresses = Toolbox.doSQL_dt(conn, @"SELECT * FROM address  WHERE address_table = 'customer' AND address_table_id =@v0", new object[] { cust_id });
										var _xmldata = @"
<addresses>";
										foreach (DataRow op in _addresses.Rows)
										{
											var this_address_id = op["address_id"];
											var this_address_type = op["address_type"];
											object this_address_name = Toolbox.do_value_from(op["address_desc"], false);
											object this_address_addr1 = Toolbox.do_value_from(op["address_addr1"], false);
											_xmldata += string.Format(@"
	<address>
		<id>{0}</id>
		<type>{1}</type>
		<addr>{2}</addr>
	</address>
", this_address_id, this_address_type, this_address_addr1);
										}
										_xmldata += @"
</addresses>";
										Response.Write(_xmldata);
										Response.End();
									}
									break;
								#endregion
								#region xml_customerinfo
								case "xml_customerinfo":
									if (_q["cust_id"] != null)
									{
										var cust_id = _q["cust_id"];
										var this_address_id = !string.IsNullOrEmpty(_q["address_id"]) ? _q["address_id"] : "";
										var this_address = !string.IsNullOrEmpty(this_address_id) ? new NEAddress(Convert.ToInt32(this_address_id)) : new NEAddress(Convert.ToInt32(cust_id), "customer");
										Response.Write(@"
<customer>");
										var address_1 = this_address.Addr1;
										var address_2 = this_address.Addr2;
										var address_3 = this_address.Addr3;
										var address_4 = this_address.Addr4;
										var city = this_address.City;
										var state = this_address.Prov;
										var postal = this_address.Postal;
										var country = this_address.Country;
										var phone = this_address.PhoneArea + "-" + this_address.Phonefirst + "-" + this_address.PhoneLast;
										var fax = this_address.FaxArea + "-" + this_address.FaxFirst + "-" + this_address.FaxLast;
										Response.Write(string.Format(@"
	<info address_1=""{0}"" address_2=""{1}"" address_3=""{2}"" address_4=""{3}"" city=""{4}"" state=""{5}"" postal=""{6}"" country=""{7}"" phone=""{8}"" fax=""{9}"" addres_id=""10""/>", address_1, address_2, address_3, address_4, city, state, postal, country, phone, fax, this_address.id));

										Response.Write(@"
</customer>");
										Response.End();
									}
									break;
								#endregion
								#region xml_contactinfo
								case "xml_contactinfo":
									if (_q["contact_id"] != null)
									{
										var contact_id = _q["contact_id"];
										var this_contact = new NEContact(Convert.ToInt32(contact_id));
										Response.Write(@"
<contact>");
										var mobile = this_contact.Contact_CellPhone;
										var extension = this_contact.Contact_Extension;
										var email = this_contact.Contact_Email;
										Response.Write(string.Format(@"
	<info mobile=""{0}"" extension=""{1}"" email=""{2}"" />", mobile, extension, email));

										Response.Write(@"
</contact>");
										Response.End();
									}
									break;
								#endregion
								#region xml_quoters
								case "xml_quoters":
									var sb_xml_quoters = new StringBuilder();
									if (_q["business_unit_id"] != null)
									{
										_quote.business_unit_id = _q["business_unit_id"];
									}
									else
									{
										_quote.business_unit_id = current_user.business_unit_id.ToString();
									}
									var _quoters = _quote.get_Quoters(_quote.business_unit_id);
									if (_quoters.Rows.Count > 0)
									{
										sb_xml_quoters.Append("\n<members>");
										foreach (DataRow _quoter in _quoters.Rows)
										{
											var _id = _quoter["id"].ToString();
											var _name = _quoter["name"].ToString();
											sb_xml_quoters.AppendFormat(@"
	<member>
		<id>{0}</id>
		<name>{1}</name>
	</member>", _id, _name);
										}
										sb_xml_quoters.Append("\n</members>");
										Response.Write(sb_xml_quoters.ToString());
										Response.End();
									}
									else
									{
										Response.Write(@"
<members>
	<member>
		<id>0</id>
		<name>No Quoters Available</name>
	</member>
</members>");
										Response.End();
									}
									break;
								#endregion
								#region xml_companies
								case "xml_companies":
									var _companies = _quote.get_Companies();
									if (_companies.Rows.Count > 0)
									{
										Response.Write("\n<companies>");
										foreach (DataRow _company in _companies.Rows)
										{
											var _id = _company["id"].ToString();
											var _name = _company["name"].ToString();
											Response.Write(string.Format("\n\t<company id=\"{0}\" name=\"{1}\"/>", _id, _name));
										}
										Response.Write("\n</companies>");
										Response.End();
									}
									else
									{
										Response.Write("FAILED");
									}
									break;
								#endregion
								#region xml_divisions
								case "xml_divisions":
									_quote.quoted_business_unit_id = _q["business_unit_id"];
									var _divisions = _quote.divisions_dt();
									if (_divisions.Rows.Count > 0)
									{
										Response.Write("\n<divisions>");
										foreach (DataRow _division in _divisions.Rows)
										{
											var _id = _division["id"].ToString();
											var _name = _division["name"].ToString();
											Response.Write(string.Format("\n\t<division id=\"{0}\" name=\"{1}\"/>", _id, _name));
										}
										Response.Write("\n</divisions>");
										Response.End();
									}
									else
									{
										Response.Write("FAILED");
									}
									break;
								#endregion xml_divisions
								#region xml_quote_search
								case "xml_quotesearch":
									var this_quote_id = _q["quote_id"];
									var this_job_description = _q["job_description"];
									var this_quoted_by = _q["quoted_by"];
									var this_business_unit_id = _q["business_unit_id"];
									var this_customer_name = _q["customer_name"];
									var this_quoted_before = _q["quoted_before"];
									var this_quoted_after = _q["quoted_after"];
									var this_status_id = _q["status_id"];
									_dt = _quote.search_Quotes(this_job_description, this_quoted_by, this_customer_name, this_quoted_before, this_quoted_after, this_status_id, this_quote_id, this_business_unit_id, current_user.id.ToString());
									_dt.TableName = "quote";
									_dt.WriteXml(Response.OutputStream);
									Response.End();
									break;
								#endregion
								#region xml_worksheet
								case "xml_worksheet":
									if (_q["quote_id"] != null && _q["revision"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										if (_q["section_id"] == null)
										{
											_dt = _quote.get_Worksheet();
										}
										else
										{
											_dt = _quote.get_Worksheet(_q["section_id"]);
										}

										if (_dt.Rows.Count > 0)
										{
											Response.Write(@"
<worksheet>");
											foreach (DataRow _dr in _dt.Rows)
											{
												var this_id = _dr["id"].ToString();
												var this_section_id = _dr["section_id"].ToString();
												var this_part_no = _dr["part_no"].ToString();
												var this_is_QTY = false;
												if (Regex.Match(this_part_no, "^[0-9]+$").Success)
												{
													quote_inventory.Load(this_part_no, current_user.business_unit_id);
													this_is_QTY = quote_inventory.is_qty;
												}

												var this_original_sell = _dr["original_sell"].ToString();
												var this_code = _dr["code"].ToString();
												var this_cost = _dr["cost"].ToString();
												var this_description = _dr["description"].ToString();
												var this_sell = _dr["sell"].ToString();
												var this_extended_per = _dr["extended_per"].ToString();
												var this_qty = _dr["qty"].ToString();
												Response.Write(string.Format(@"
	<item id=""{0}"" section_id=""{1}"" part_no=""{2}"" description=""{3}"" sell=""{4}"" extended_per=""{9}"" is_QTY=""{8}"" qty=""{5}"" code=""{6}"" original_sell=""{7}"" cost=""{10}""/>",
												this_id,                // {0}
												this_section_id,
												this_part_no,           // {2}
												this_description,
												this_sell,              // {4}
												this_qty,
												this_code,              // {6}
												this_original_sell,
												this_is_QTY,            // {8}
												this_extended_per,      // {9}
												this_cost               // {10}
												));
											}
											Response.Write(@"
</worksheet>");
											Response.End();
										}
									}
									else
									{
										Response.Redirect("./frame.aspx");
									}
									break;
								#endregion
								#region xml_sections
								case "xml_sections":
									if (_q["quote_id"] != null && _q["rev"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["rev"];
										_dt = _quote.get_Sections();
										Response.Write(@"
<sections>");
										if (_dt.Rows.Count > 0)
										{
											foreach (DataRow _dr in _dt.Rows)
											{
												var section_id = _dr["id"].ToString();
												var section = _dr["section"].ToString();
												Response.Write(string.Format(@"
	<section id=""{0}"" name=""{1}""/>", section_id, section));
											}
										}
										else
										{
											Response.Write(@"
	<error_in_xmit/>");
										}
										Response.Write(@"
</sections>");
										Response.End();
									}
									else
									{
										Response.Redirect("./frame.aspx");
									}
									break;
								#endregion
								#region xml_followup_history
								case "xml_followup_history":
									if (_q["quote_id"] != null && _q["revision"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										_dt = _quote.get_followup_history();
										Response.Write(@"
<history count='" + _dt.Rows.Count + "'>");
										if (_dt.Rows.Count > 0)
										{
											foreach (DataRow _dr in _dt.Rows)
											{
												var _id = _dr["id"].ToString();
												var _name = _dr["name"].ToString();
												var _date = _dr["date"].ToString();
												var _note = _dr["note"].ToString().Replace("\n", "<br /><br />");
												var _type = _dr["type"].ToString();
												Response.Write(string.Format(@"
	<item>
		<id>{0}</id>
		<name>{1}</name>
		<type>{4}</type>
		<date>{2}</date>
		<note>{3}</note>
	</item>", _id, _name, _date, _note, _type));
											}
										}
										else
										{
											Response.Write(@"
	<error_in_xmit/>");
										}
										Response.Write(@"
</history>");
										Response.End();
									}

									else
									{
										Response.Redirect("./frame.aspx");
									}
									break;
								#endregion
								#endregion XML HANDLERS
								#region GET/SET HANDLERS
								#region adjusted_qty_sell
								case "adjusted_qty_sell":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["master_id"] != null)
									{
										double adjusted_sell = 0;
										double used_sell = 0;
										try
										{
											quote_inventory.Load(_q["master_id"], Convert.ToInt32(_quote.quoted_business_unit_id));
											double quantity = 0;
											if (_q["quantity"] == null || _q["quantity"] == "" || Regex.Match(_q["quantity"], "^[^0-9.]$").Success)
											{
												quantity = 1;
											}
											else
											{
												quantity = Convert.ToDouble(_q["quantity"]);
											}
											if (quantity < 1)
											{
												quantity = 1;
											}
											used_sell = quote_inventory.sell_price;
											adjusted_sell = Convert.ToDouble(shared.GetSellPrice(quote_inventory.cost_price_branch_lowest, used_sell, quote_inventory.is_qty, quantity, Convert.ToInt32(_quote.quoted_business_unit_id)).ToString("N2"));
										}
										catch
										{
											adjusted_sell = 0;
										}

										Response.Write(adjusted_sell);
									}
									Response.End();
									break;
								#endregion adjusted_qty_sell
								#region del_section
								case "del_section":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["section_id"] != null)
									{
										if (_quote.del_section(_q["section_id"]))
										{
											Response.Write("SUCCESS");
										}
										else
										{
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region del_worksheet_row
								case "del_worksheet_row":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["row_id"] != null)
									{
										if (_quote.del_worksheet_row(_q["row_id"]))
										{
											Response.Write("SUCCESS");
										}
										else
										{
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region duplicate_quote
								case "duplicate_quote":
									_tools.dont_cache_page();
									if (_q["quote_id"] != null && _q["revision"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										_quote.to_history("Quote Duplicated");

										if (_quote.duplicate_quote())
										{
											Response.Redirect("./index.aspx?a=g&quote_id=" + _quote.quote_id + "&revision=" + _quote.revision);
										}
										else
										{
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Redirect("./index.aspx");
									}
									break;
								#endregion
								#region get_membername
								case "get_membername":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["member_id"] != null)
									{
										try
										{
											var member_id = _q["member_id"];
											var temp_member = new NeMember(Convert.ToInt32(member_id));
											Response.Write(temp_member.FullName);
										}
										catch
										{
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion get_membername
								#region get_sellprice
								case "get_sellprice":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["master_id"] != null)
									{
										try
										{
											quote_inventory.Load(_q["master_id"], _quote.business_unit_id);
											returned_string = quote_inventory.sell_price.ToString();
										}
										catch
										{
											returned_string = "FAILED";
										}
									}
									else
									{
										returned_string = "FAILED";
									}
									Response.Write(returned_string);
									Response.End();
									break;
								#endregion
								#region get_status
								case "get_status":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null &&
										_q["revision"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										try
										{
											Response.Write(Toolbox.doSQL_string(conn, @"SELECT status_id FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { _q["quote_id"], _q["revision"] }));
										}
										catch (Exception ee)
										{
											Response.Write(ee.Message);
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region get_quote
								case "get_quote":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _quote.get_active_revision();
										Response.Redirect("./index.aspx?a=g&quote_id=" + _quote.quote_id + "&revision=" + _quote.revision);
										//ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('./index.aspx?a=g&quote_id=" + _quote.quote_id + "&revision=" + _quote.revision + "','quote',950,800)", true);


									}
									else
									{
										Response.Write("FAILED");
										Response.End();
									}
									break;
								#endregion
								#region is_active
								case "is_active":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["cust_id"] != string.Empty && _q["cust_id"] != null && _q["cust_id"] != "")
									{
										var _customer = new NECustomer(Convert.ToInt32(_q["cust_id"]));
										if (_customer.Hold == "T")
										{
											Response.Write("True");
										}
										else
										{
											Response.Write("False");
										}
									}
									else
									{
										Response.Write("False");
									}
									Response.End();
									break;
								#endregion
								#region is_QCed
								case "is_QCed":
									Response.Clear();
									_tools.dont_cache_page();
									if (!string.IsNullOrEmpty(_q["cust_id"]) && !check_numeric.IsMatch(_q["cust_id"]))
									{
										var c_is_qced = Toolbox.doSQL_int(conn, @"SELECT COUNT(customer_id) FROM customer WHERE customer_id = @v0  AND customer_qc_member_id IS NOT NULL AND customer_qc_datetime IS NOT NULL", new object[] { _q["cust_id"] });
										var str_is_qced = c_is_qced > 0 ? "True" : "False";
										Response.Write(str_is_qced);
									}
									else
									{
										Response.Write("False");
									}
									Response.End();
									break;
								#endregion
								#region last_print_date
								case "last_print_date":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null && _q["revision"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										var this_last_printdate = _quote.update_last_printed();
										Response.Write(this_last_printdate);
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region last_print_date
								case "last_sent_date":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null && _q["revision"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										var this_last_sentdate = _quote.get_last_sent();
										Response.Write(this_last_sentdate);
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region lock_quoter
								case "lock_quoter":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null &&
										_q["revision"] != null &&
										_q["lock_quoter"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										_quote.this_member = current_user;
										Response.Write(_quote.lock_quoter(_q["lock_quoter"]));
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion lock_quoter
								#region new_revision
								case "new_revision":
									_tools.dont_cache_page();
									if (_q["quote_id"] != null && _q["revision"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										_quote.to_history("New Revision Made");

										var new_rev = _quote.new_revision();
										Response.Redirect("./index.aspx?a=g&quote_id=" + _quote.quote_id + "&revision=" + new_rev);
									}
									else
									{
										Response.Redirect("./index.aspx");
									}
									break;
								#endregion
								#region new_detail
								case "new_detail":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null && _q["revision"] != null && _q["type"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										var new_id = "";
										try
										{
											new_id = _quote.new_extratext(Convert.ToInt32(_q["type"]));
										}
										catch (Exception ee)
										{
											new_id = ee.Message;
										}
										Response.Write(new_id);
									}
									else
									{
										Response.Redirect("./index.aspx");
									}
									Response.End();
									break;
								#endregion new_detail
								#region del_extratext
								case "del_extratext":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["id"] != null)
									{
										try
										{
											Toolbox.doSQL_void(conn, @"DELETE FROM quote_extratext WHERE id = @v0  LIMIT 1", new object[] { _q["id"] });
											var c = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM quote_section WHERE detail_id = @v0 ", new object[] { _q["id"] });
											var temp_section_id = 0;
											var del_assoc = Convert.ToBoolean(_q["del_assoc"]);
											if (c == 1)
											{
												temp_section_id = Toolbox.doSQL_int(conn, @"SELECT id FROM quote_section WHERE detail_id = @v0 ", new object[] { _q["id"] });
												var _c = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM quote_worksheet WHERE section_id = @v0 ", new object[] { temp_section_id });
												if (_c > 0 && del_assoc)
												{
													Toolbox.doSQL_void(conn, @"DELETE FROM quote_section WHERE detail_id = @v0  LIMIT 1", new object[] { _q["id"] });
													Toolbox.doSQL_void(conn, @"DELETE FROM quote_worksheet WHERE section_id = @v0 ", new object[] { temp_section_id });
												}
												else if (_c == 0 && del_assoc)
												{
													// Delete section if user wanted to delete associate and there is no lines attached to it.
													Toolbox.doSQL_void(conn, @"DELETE FROM quote_section WHERE detail_id = @v0  LIMIT 1", new object[] { _q["id"] });
												}
												else if (!del_assoc)
												{
													// Changed section to same text just without id
													Toolbox.doSQL_void(conn, @"UPDATE quote_section SET detail_id = NULL, section = case when substring(section, 1, locate('.', section, 1)-1)>0 then substring(section, locate('.', section, 1)+1, length(section)) else id end, picklist_controlled = TRUE WHERE detail_id = @v0  LIMIT 1", new object[] { _q["id"] });
												}
											}
											else if (c > 1)
											{
												throw new Exception("There were multiple sections with this section ID assigned to it... this shouldn't happen.");
											}
											returned_string = "SUCCESS";
										}
										catch (Exception ee)
										{
											returned_string = ee.Message;
										}
										Response.Write(returned_string);
									}
									else
									{
										Response.Redirect("./index.aspx");
									}
									Response.End();
									break;
								#endregion del_extratext
								#region push_contact
								case "push_contact":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["customer_id"] != null &&
										_q["name"] != null
										)
									{
										try
										{
											var this_contact = new NEContact();
											this_contact.Contact_Cust_ID = Convert.ToInt32(_q["customer_id"]);
											this_contact.Contact_Type = "Customer";
											this_contact.Contact_Status = "Active";
											this_contact.Contact_Name = _q["name"].Trim();
											var this_address_id = 0;
											int.TryParse(_q["address_id"], out this_address_id);
											if (this_address_id == 0)
											{
												throw new Exception("Please select an address before adding a contact");
											}
											this_contact.address_id = this_address_id;
											var c = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM contact WHERE contact_name = @v0  AND contact_cust_id = @v1  AND contact_type = 'Customer'", new object[] { _q["name"].Trim(), _q["customer_id"] });
											if (c > 0)
											{
												throw new Exception("Duplicate Contact Detected");
											}
											if (_q["title_id"] != null)
											{
												this_contact.Contact_Title = _q["title_id"];
											}
											if (_q["email"] != null)
											{
												this_contact.Contact_Email = _q["email"];
											}
											if (_q["cell"] != null)
											{
												this_contact.Contact_CellPhone = _q["cell"];
											}
											if (_q["ext"] != null)
											{
												this_contact.Contact_Extension = _q["ext"];
											}
											this_contact.AddNEContact(this_contact);
											Response.Write(this_contact.Contact_ID.ToString());
										}
										catch (Exception ee)
										{
											Response.Write(ee.Message);
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region uh / update header
								case "uh": // uh = update header
									Response.Clear();
									_tools.set_plain_header();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null &&
										_q["revision"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										Output = string.Format(@"
{{
	'status'			: 'success',
	'tm_pricing'		: '{0:c2}',
	'worksheet_total'	: '{1:c2}',
	'ticks'				: '{2}'
}}",
										_quote.tm_pricing_total(),      // {0}
										_quote.worksheet_total(),       // {1}
										_quote.get_last_ticks()         // {2}
										).Replace('\'', '\"');
									}
									else
									{
										Output = @"{""status"":""error""}";
									}
									Response.Write(Output);
									Response.End();
									break;
								#endregion uh / update header
								#region ug / update groups
								case "ug": // ug = update groups
									Response.Clear();
									_tools.set_plain_header();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null &&
										_q["revision"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										_dt = _quote.group_totals();
										if (_dt.Rows.Count > 0)
										{
											Output += "[";
											foreach (DataRow _dr in _dt.Rows)
											{
												var id = _dr["id"].ToString();
												var total = Convert.ToDouble(_dr["total"]).ToString("c2");
												Output += string.Format(@"
	{{
	 'id':'{0}',
	 'total':'{1}'
	}},",
												id,
												total
												).Replace('\'', '\"');
											}
											Output = Output.TrimEnd(',') + "\n]";
										}
										else
										{
											Output += @"{""status"":""success"",""rows"":""0""}";
										}
									}
									else
									{
										Output = @"{""status"":""error""}";
									}
									Response.Write(Output);
									Response.End();
									break;
								#endregion ug / update groups
								#region reason_update
								case "reason_update":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null &&
										_q["revision"] != null &&
										_q["reason_id"] != null &&
										_q["checked"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										var reason_id = _q["reason_id"];
										var checked_state = _q["checked"];
										if (_quote.update_reason(reason_id, checked_state))
										{
											Response.Write("SUCCESS");
										}
										else
										{
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region return_sellprice
								case "return_sellprice":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["cost"] != null)
									{
										double cost = 0;
										double sent_sell = 0;
										try
										{
											cost = Convert.ToDouble(_q["cost"]);
											sent_sell = Convert.ToDouble(shared.GetSellPrice(cost, 0, false, 1, Convert.ToInt32(_quote.quoted_business_unit_id)).ToString("N2"));
										}
										catch
										{
											sent_sell = 0;
										}
										Response.Write(sent_sell);
									}
									Response.End();
									break;
								#endregion return_sellprice
								#region save_appointment
								case "save_appointment":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null &&
										_q["revision"] != null &&
										_q["note"] != null &&
										_q["date"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										var _date = _q["date"];
										var _note = _q["note"];
										var _set = false;
										if (_q["_id"] != "")
										{
											var _id = _q["_id"];
											_set = _quote.schedule_followup(_date, _note, _id);
										}
										else
										{
											_set = _quote.schedule_followup(_date, _note);
										}
										if (_set)
										{
											Response.Write("SUCCESS");
										}
										else
										{
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region set_active_revision
								case "set_active_revision":
									Response.Clear();
									_tools.dont_cache_page();
									_quote.quote_id = _q["quote_id"];
									_quote.revision = _q["revision"];
									try
									{
										_quote.set_active_revision();
										Response.Write("SUCCESS");
									}
									catch (Exception ee)
									{
										Response.Write(ee.Message);
									}
									Response.End();
									break;
								#endregion set_active_revision
								#region show_help
								case "show_help":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["show"] != null)
									{
										try
										{
											Toolbox.doSQL_void(conn, @"UPDATE member SET show_quote_help = @v1  WHERE member_id = @v0  LIMIT 1", new object[] { current_user.id, _q["show"] });
											Response.Write("SUCCESS");
										}
										catch (Exception ee)
										{
											_tools.catch_error(ee);
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion show_help
								#region start_quote
								case "start_quote":
									Response.Clear();
									_quote.customer_id = _q["customer_id"];
									_quote.contact_id = _q["contact_id"];
									_quote.date_due = _q["date_due"];
									_quote.job_description = _q["job_description"];
									_quote.business_unit_id = _q["business_unit_id"];
									_quote.quoted_by = _q["quoted_by"];
									_quote.quoter_locked_bool = _q["lock_quoted_by"].ToLower() == "true";
									_quote.quoted_price = _q["estimated_value"];
									_quote.completion_date = _q["date_completion"];
									try
									{
										_quote.StartQuote();
										Toolbox.doSQL_void(conn, @"INSERT INTO quote_history (create_datetime, created_by, quote_id, revision, event) VALUES (NOW(), @v0 , @v1 , 1, 'Quote Created')", new object[] { current_user.id, _quote.quote_id });
										Response.Redirect("./index.aspx?a=g&quote_id=" + _quote.quote_id + "&revision=1");
									}
									catch (Exception ee)
									{
										throw ee;
									}
									break;
								#endregion start_quote
								#region status_update
								case "status_update":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null && _q["status_id"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										_quote.InitializeQuote();
										_quote.who_competitor = _q["who_competitor"];
										_quote.why_lose = _q["why_lose"];
										_quote.what_price = _q["what_price"];
										_quote.status_id = _q["status_id"];
										if (_quote.status_update())
										{
											Response.Write("SUCCESS");
										}
										else
										{
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region stop_redir
								case "stop_redir":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["this_quote_id"] != null &&
										_q["this_revision"] != null &&
										_q["redir_quote_id"] != null &&
										_q["redir_revision"] != null)
									{
										object _this_quote_id = _q["this_quote_id"];
										object _this_revision = _q["this_revision"];
										object _redir_quote_id = _q["redir_quote_id"];
										object _redir_revision = _q["redir_revision"];
										try
										{
											var quoted_by = Toolbox.doSQL_string(conn, @"SELECT quoted_by FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { _this_quote_id, _this_revision });
											if (quoted_by == current_user.id.ToString())
											{
												Toolbox.doSQL_void(conn, @"UPDATE quote_master SET status_id = 1, updated_by_page = 'quote - stop_redir()', customer_id = null, quoted_by = @v2  WHERE quote_id = @v0  AND revision = @v1  LIMIT 1", new object[] { _this_quote_id, _this_revision, current_user.id });
											}
											Response.Redirect(string.Format("./index.aspx?a=g&quote_id={0}&revision={1}", _redir_quote_id, _redir_revision));
										}
										catch (Exception ee)
										{
											_tools.catch_error(ee);
											Response.Write("<script>alert('There was an error closing this quote. \\n An email has been sent to tech support.');location.href = './frame.aspx';</script>");
											Response.End();
										}
									}
									else
									{
										Response.Write("<script>alert('This was an incomplete request.');location.href = './frame.aspx';</script>");
										Response.End();
									}
									break;
								#endregion
								#region switch_section_for_worksheet_row
								case "switch_section_for_worksheet_row":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["section_id"] != null && _q["row_id"] != null)
									{
										if (_quote.worksheet_change_row_section(_q["row_id"], _q["section_id"]) == true)
										{
											Response.Write("SUCCESS");
										}
										else
										{
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region tm_price
								case "tm_price":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null &&
										_q["revision"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										Response.Write(_quote.tm_pricing_total().ToString("c2"));
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region up_exp_val
								case "up_exp_val":
									_quote.quote_id = _q["quote_id"];
									_quote.revision = _q["revision"];
									_quote.InitializeQuote();
									_quote.expected_value = Convert.ToDouble(_q["exp_val"]);
									returned_string = !_quote.update_expected_value() ? "SUCCESS" : "FAILED";
									Toolbox.QuickReponse(Response, returned_string);
									break;
								#endregion up_exp_val
								#region update_po_info
								case "update_po_info":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null &&
										_q["revision"] != null &&
										_q["type"] != null &&
										_q["value"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										var this_type = _q["type"];
										var this_value = _q["value"];
										if (_quote.update_po_info(this_type, this_value))
										{
											Response.Write("SUCCESS");
										}
										else
										{
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region update_section
								case "update_section":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["name"] != null &&
										_q["quote_id"] != null &&
										_q["revision"] != null &&
										_q["section_id"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];

										if (_quote.update_section(_q["name"], _q["section_id"]))
										{
											Response.Write("SUCCESS");
										}
										else
										{
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion
								#region worksheet_total
								case "worksheet_total":
									Response.Clear();
									_tools.dont_cache_page();
									if (_q["quote_id"] != null &&
										_q["revision"] != null)
									{
										_quote.quote_id = _q["quote_id"];
										_quote.revision = _q["revision"];
										Response.Write(_quote.worksheet_total().ToString("c2"));
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
								#endregion worksheet_total
								#region worksheet_update
								case "worksheet_update":
									Response.Clear();
									_tools.dont_cache_page();
									if (
										_q["row_id"] != null &&
										_q["section_id"] != null &&
										(_q["part_no"] != null && _q["code"] != null || _q["description"] != null) &&
										_q["per_sell"] != null &&
										_q["qty"] != null)
									{
										row_id = _q["row_id"];
										var this_section_id = _q["section_id"];
										var this_part_no = _q["part_no"];
										var this_extended_per = "";
										if (_q["per_extended"] != null)
										{
											this_extended_per = _q["per_extended"];
										}
										else
										{
											this_extended_per = "0";
										}
										inventory inventory_interface = null;
										if (this_part_no != "")
										{
											inventory_interface = new inventory();
											inventory_interface.Load(this_part_no, current_user.business_unit_id);
										}
										var this_cost = "";
										if (_q["cost"] != null)
										{
											this_cost = _q["cost"];
										}
										else
										{
											this_cost = "0";
										}
										var this_code = _q["code"];
										var this_description = _q["description"];
										var this_sell = _q["per_sell"];
										var this_qty = _q["qty"];
										var this_original_sell = "";
										if (_q["original_sell"] != null)
										{
											this_original_sell = _q["original_sell"];
										}
										else
										{
											this_original_sell = "0";
										}
										if ((this_original_sell == "" || Convert.ToDouble(this_original_sell) == 0) && inventory_interface != null)
										{
											this_original_sell = inventory_interface.sell_price.ToString();
										}
										//Toolbox.doSQL_void(conn,@"INSERT INTO matt (jumble)  values (@v0)",new object[] { Request.RawUrl } );
										if (_quote.worksheet_update(row_id, this_part_no, this_description, this_sell, this_original_sell, this_qty, this_section_id, this_code, this_extended_per, this_cost))
										{
											Response.Write("SUCCESS");
										}
										else
										{
											Response.Write("FAILED");
										}
									}
									else
									{
										Response.Write("FAILED");
									}
									Response.End();
									break;
									#endregion
									#endregion GET/SET HANDLERS
							}
						}
						else
						{
							#region DEFAULT PAGE
							_quote.quoted_by = current_user.id.ToString();
							_quote.quoted_business_unit_id = current_user.business_unit_id.ToString();
							var quoter_locked = "";
							var stage1header = "";
							var stage1column = "";
							var stage4header = "";
							var stage4column = "";
							var reviewheader = "";
							var reviewcolumn = "";
							var stage1dollar = "";
							var stage4dollar = "";
							var reviewcolumndollar = "";
							var now_list = "";
							var this_todlist = _quote.todonow_list();


							if (new NeBusinessUnit(current_user.business_unit_id).uses_quote_process == 1 || this_todlist.Length > 50)
							{

								stage1header = "<td class='h'>Stage 1 Go/No Go<br/>(" + _quote.quote_count("10") + @")</td>";
								stage1column = "<td valign='top' class='quotes'>" + _quote.quote_list("10") + @"</td>";
								stage4header = "<td class='h'>Stage 4 Go/No Go<br/>(" + _quote.quote_count("11") + @")</td>";
								stage4column = "<td valign='top' class='quotes'>" + _quote.quote_list("11") + @"</td>";
								reviewheader = "<td class='h'>Final Review<br/>(" + _quote.quote_count("12") + @")</td>";
								reviewcolumn = "<td valign='top' class='quotes'>" + _quote.quote_list("12") + @"</td>";
								stage1dollar = "<td>" + _quote.quote_dollar_total("10") + @"</td>";
								stage4dollar = "<td>" + _quote.quote_dollar_total("11") + @"</td>";
								reviewcolumndollar = "<td>" + _quote.quote_dollar_total("12") + @"</td>";

								now_list = @"</br>
<div id='tobedealtwith' style='color:#fff' align='left'><b>Needs Your Attention Now:</b>			
</br><table cellpadding='2' cellspacing='1' id='now_list' width='100%'>
				<thead>
					<tr>
						<td class='h'>Quote ID</td>
						<td class='h'>Quote Level</td>
						<td class='h'>Status</td>
						<td class='h'>Customer</td>
						<td class='h'>Description</td>
						<td class='h'>Quoted By</td>
					</tr>
				</thead>
				<tbody>" + this_todlist;

								now_list += @"</tbody>
			</table>
					
			</div>";

							}
							if (current_user.AuthenticatedForPrivilege(61))
							{
								quoter_locked = "<input type='checkbox' data-is_new_quote='true' height='15px' onclick='toggle_lock_quoted_by(this)' id='lock_quoter'/><img hspace='0' src='/images/icon/icon[lock].gif' width='16' height='16' />";
							}




							Output += string.Format(@"
			
			<script>
				$('document').ready(function()
					{{
					$('#quote_list').find('.quote').each(function(){{$(this).tip()}});
					//fill_companies({0});
					//fill_quoters({2},{0},true);
					$('#search_pane').dialog(
						{{
						autoOpen:	false,
						resizable:	true,
						draggable:	true,
						modal:		true,
						title:		'Quote Search',
						width:		1200,
						height:		700
						}});
						
							$('#quote_addcontact').dialog(
								{{
								autoOpen:	false,
								title:		'ADD CONTACT',
								resizable:	true,
								modal:		true,
								draggable:	true,
								width:		450
								}});
					$('#quickpick button').click(function()
						{{
						var quote_id	= $(this).prev('input').val();
						quote_obj.load_quote(quote_id);
						}});
					$('#quickpick input').keypress(function(e)
						{{
						var key			= e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
						var quote_id	= $(this).val();
						if(key == 13)
							{{
							quote_obj.load_quote(quote_id);
							$(this).val('');
							e.preventDefault();
							}}
						}});
					$('#new_quote').dialog(
						{{
						autoOpen:		false,
						width:			850,
						position:		[50,35],
						title:			'New Quote',
						modal:			true,
						draggable:		false,
						open:			function(event, ui)
											{{
											$('html').css({{'overflow-y':'hidden'}});
											$('#new_quote').find('input, select, textarea').bind('change', function()
																									{{
																									gatekeeper_startquote();
																									}});
											$('#newquote_customer').focus();
											}},
						close:			function(event, ui)
											{{
											$('html').css({{'overflow-y':'visible'}});
											}},
						resizable:		false
						}});
					//fill_companies({0}, $('#newquote_branch'));
					//fill_quoters({2}, {0}, false, true);
					//$('#newquote_datedue').datepicker({{dateFormat: 'yy-mm-dd'}})
					//$('#newquote_completedate').datepicker({{dateFormat: 'yy-mm-dd'}})
					}});
			</script>
			<div id='new_quote' style='display:none;'>
				<table cellpadding='3' cellspacing='0' width='100%' style='font-family:arial;font-size:11px;'>
					<tr>
						<td width='150'><b>Customer:</b></td>
						<td><input id='newquote_customer' type='text' data-id='' onfocus=""attach_ac(this, 'customer');"" style='width:100%'/></td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td style='font-size:11px;'>If a customer does not appear in this list, it is possible that they have not yet been QC'ed yet.</td>
					</tr>
					<tr>
						<td><b>Contact:</b></td>
						<td><select style='width:90%' id='newquote_contact' disabled><option value='0'>Select Customer First</option></select><button type='button' title='Add contact to this customer' onclick=""$('#quote_addcontact').dialog('open');""><img src='/images/icon/icon[add].gif' width='16' height='16'></button></td>
					</tr>
					<tr>
						<td><b>Expected Quote Value:</b></td>
						<td><input style='width:150px' onkeydown='only_numeric(event)' id='newquote_value' type='text' value='' /></td>
					</tr>
					<tr>
						<td><b>Date Due:</b></td>
						<td><input style='width:100%' id='newquote_datedue' type='text' value='' /></td>
					</tr>
<tr>
						<td><b>Expected PO Date:</b></td>
						<td><input style='width:100%' id='newquote_exp_podate' type='text' value='' /></td>
					</tr>
					<tr>
						<td><b>Expected Complete Date:</b></td>
						<td><input style='width:100%' id='newquote_completedate' type='text' value='' /></td>
					</tr>
					<tr>
						<td valign='top'><b>Job Description:</b></td>
						<td><textarea style='width:100%;height:50px;' id='newquote_description' type='text'></textarea></td>
					</tr>
					<tr>
						<td><b>Business Unit:</b></td>
						<td><select style='width:75%' onchange=""if(this.value != '0'){{fill_divisions({0}, true, 'newquote_division', this, '');fill_quoters(this);}}"" id='newquote_branch'></select></td>
					</tr>
					<tr>
						<td><b>Quoted By:</b></td>
						<td><select style='width:75%' id='newquote_quotedby'></select>{5}</td>
					</tr>
					<tr>
						<td colspan='2' align='center'><button type='button' id='newquote_submit' onclick='gatekeeper_startquote(this);' disabled>Start Quote</button></td>
					</tr>
				</table>
				<hr>
				<div id='newquote_openquotes'></div>
			</div>
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
							<option value='0'>Choose Title</option>{4}
						</select>
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
			<button type='button' id='button_new' onclick=""start_quote();"">Start New</button>
			<button type='button' id='button_search_onbucket' onclick=""$('#search_pane').dialog('open');"">Search</button>
			
			<div id='quickpick'><b>Quote ID: </b><input type='text' style='font-size:11px; height:16px;width:60px;border:none;background-color:white;' title='To quickly get to the quote number you want, just type it in here and press go.' id='quote_id' size='7' /><button style='font-size:12px; padding:3px; height:17px;width:25px;border:none;background-color:whitesmoke;cursor:pointer;' type='button'>GO</button></div>
			<center>
{16}
			<table cellpadding='4' cellspacing='0' id='quote_list'>
				<thead>
					<tr>
						{7}
						{9}
						<td class='h'>To be Quoted <br/>(" + _quote.quote_count("1") + @")</td>
						{11}
						<td class='h'>To be Delivered <br/>(" + _quote.quote_count("2") + @")</td>
						<td class='h'>To be Verified <br/>(" + _quote.quote_count("3") + @")</td>
						<td class='h'>Waiting Customer Go Ahead<br/>(" + _quote.quote_count("4") + @")</td>
						<td class='h'>Follow Up Today <br/>(" + _quote.quote_count("5") + @")</td>
					</tr>
				<thead>
				<tbody>
					<tr>
						{8}
						{10}
						<td valign='top' class='quotes'>" + _quote.quote_list("1") + @"</td>
						{12}
						<td valign='top' class='quotes'>" + _quote.quote_list("2") + @"</td>
						<td valign='top' class='quotes'>" + _quote.quote_list("3") + @"</td>
						<td valign='top' class='quotes'>" + _quote.quote_list("4") + @"</td>
						<td valign='top' class='quotes'>" + _quote.quote_list("5") + @"</td>
					</tr>
				</tbody>
				<tfoot>
					<tr class='tfoot'>
						{13}
						{14}
						<td>" + _quote.quote_dollar_total("1") + @"</td>
						{15}
						<td>" + _quote.quote_dollar_total("2") + @"</td>
						<td>" + _quote.quote_dollar_total("3") + @"</td>
						<td>" + _quote.quote_dollar_total("4") + @"</td>
						<td>" + _quote.quote_dollar_total("5") + @"</td>
					</tr>
				</tfoot>
			</table>
			</center>
			{1}
			",
					current_user.business_unit_id,           // {0}
					_quote.search_pane(),                   // {1}
					current_user.id,                        // {2}
					DateTime.Now.ToString("yyyy-MM-dd"),    // {3}
					_quote.get_title_options(),             // {4}
					quoter_locked,                          // {5}
					null,                       // {6}
					stage1header,//{7}
					stage1column, //{8}
					stage4header, // {9}
					stage4column, // {10}
					reviewheader, // {11}
					reviewcolumn,// {12}
					stage1dollar, //{13}
					stage4dollar, //{14}
					reviewcolumndollar,//{15}
					now_list //16
					);
							#endregion DEFAULT PAGE
						}
						break;
						#endregion GET
				}
				quoting.InnerHtml = Output;
			}
		}
	}





}

public class NeQuote
{

	#region Variable Assignment
	Toolbox _tools = new Toolbox();
	public MySqlConnection conn { get; set; }
	private string sql;
	private Dictionary<string, string> field_titles = new Dictionary<string, string>();
	private string this_string;
	private bool this_bool;

	private NeMember this_branch_manager;
	private NEAddress _address = new NEAddress();
	private NEContact _contact = new NEContact();
	private NECustomer _customer = new NECustomer();
	public NeMember this_member;
	private NeBusinessUnit this_company;

	private StringBuilder contact_select = new StringBuilder();
	private string pricetype_select = "";
	private bool show_help = false;
	public bool follow_up = false;
	private string show_help_checked = "";
	public string col_type = "";
	public string col_name = "";
	public string new_value = "";
	public string current_quoter = "";
	public string business_unit_id = "";
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
	public string exp_podate = "NULL";

	public string job_description = "";
	public string cust_spec_doc = "";
	public string po = "";
	public string wo = "";
	public string pct_chance_reason = "0";
	public string pct_chance_note = "";
	public string include_title = "";
	public string wo_link = "";
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
	public string strategy_tab = "";
	public bool stage1_approved = false;
	public bool stage4_approved = false;
	public TextInfo textinfo;
	string print_buttons = "";
	public DataTable all_reports;
	string reports_list = "";
	public string[] details;
	public string[] notes;
	public string[] reasons;
	public string quick_pick_link = "";
	public string checkmemberid = "";
	public long ts_ticks = 0;
	public int currency = 1;

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
	private void get_customer_name()
	{
		customer_name = Toolbox.doSQL_string(conn, @"SELECT IFNULL(MAX(customer_name), '') customer_name FROM customer  WHERE customer_id =@v0", new object[] { customer_id });
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
			var _dt = get_Contacts(customer_id);
			if (_dt.Rows.Count > 0)
			{
				contact_select.AppendFormat(@"
				<select class='contact' tabindex='5' data-ov='{0}' onchange=""quote_obj.s('s_contact', this);"" id='contact_id' data-unlock='{1}'>
					<option value='0'>Please Select Contact</option>", contact_id, contact_id == "");
				foreach (DataRow _dr in _dt.Rows)
				{
					var this_id = _dr["contact_id"].ToString();
					var this_name = _dr["contact_name"].ToString();
					var this_selected = contact_id != "" && this_id == contact_id ? "selected" : "";
					contact_select.AppendFormat(@"
					<option value='{0}' {2}>{1}</option>", this_id, this_name, this_selected);
				}
				contact_select.Append(@"
				</select>");
			}
			else
			{
				contact_select.Clear();
				contact_select.Append(@"<select class='contact' onchange=""quote_obj.s('s_contact', this);"" id='contact_id' disabled><option value='0' selected>No Contacts Set - Please Add > </option></select>");
			}
		}
		else
		{
			contact_select.Clear();
			contact_select.Append(@"<select class='contact' onchange=""quote_obj.s('s_contact', this);"" id='contact_id' disabled><option value='0' selected>Please Select Contact</option></select>");
		}
	}
	public void get_BranchManager()
	{
		this_branch_manager = this_company.branch_manager; ;
	}
	public void to_history(string _event)
	{
		_tools.getSQL_bool(@"INSERT INTO quote_history (create_datetime, created_by, quote_id, revision, event) VALUES (now(), @v2 , @v0 , @v1 , @v3 )", new object[] { quote_id, revision, current_quoter, _event });
	}
	public void set_active_revision()
	{
		// Check if linked to WO...
		var current_rev = Toolbox.doSQL_string(conn, @"SELECT revision FROM quote_master WHERE quote_id = @v0  AND active_revision = true", new object[] { quote_id });
		var c = Toolbox.doSQL_int(conn, @"SELECT COUNT(woprog_id) FROM woprog WHERE woprog_quoteid = @v0", new object[] { quote_id + current_rev });
		if (c == 0)
		{
			Toolbox.doSQL_void(conn, @"UPDATE quote_master SET active_revision = false WHERE quote_id = @v0  AND revision != @v1 ", new object[] { quote_id, revision });
			Toolbox.doSQL_void(conn, @"UPDATE quote_master SET active_revision = true  WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
		}
		else if (c == 1)
		{
			var woprog_bvwo = Toolbox.doSQL_string(conn, @"SELECT woprog_bvwo FROM woprog WHERE woprog_quoteid = @v0 ", new object[] { quote_id + current_rev });
			throw new Exception(string.Format("This quote/revision is currently linked to work order '{0}', you must first unlink the quote before you can change the active revision.", woprog_bvwo));
		}
	}
	public void get_pricetype_select()
	{
		var _dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM quote_pricetype  WHERE active = true", null);
		var this_select = new StringBuilder();
		if (_dt.Rows.Count > 0)
		{
			if (pricetype_id == "5")
			{
				pricetype_select = string.Format(@"Not to Exceed Price <input type='hidden' id='quoted_pricetype' value='{0}' />", pricetype_id);
				return;
			}
			else
			{
				this_select.Append(@"
			<select class='pricetype' id='quoted_pricetype' tabindex='19' onchange=""quote_obj.s('s_pricetype', this);"">
				<option value='0' selected></option>");
			}
			foreach (DataRow _dr in _dt.Rows)
			{
				var this_id = _dr["id"].ToString();
				var this_type = _dr["type"].ToString();
				var this_selected = pricetype_id != "" && this_id == pricetype_id ? "selected" : "";
				if (this_type.Contains("FROM"))
				{
					this_type = "FROM - TO";
				}
				this_select.AppendFormat(@"
				<option value='{0}' {2}>{1}</option>", this_id, this_type, this_selected);
			}
			this_select.Append(@"
			</select>");
			pricetype_select = this_select.ToString();
		}
	}
	public void InitializeQuote()
	{
		sql = string.Format(@"
SELECT
	a.customer_id,
	a.open_date,
	date_format(a.completion_date, '%Y-%m-%d') completion_date,
	date_format(a.date_due, '%Y-%m-%d') date_due,
date_format(a.exp_podate, '%Y-%m-%d') exp_podate,
	a.active_revision,
	a.job_description,
	a.cust_spec_doc,
	a.quoted_by,
	a.quoter_locked,
	a.business_unit_id,
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
	a.last_modified,
a.currency
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
					get_customer_name();
				}
				if (_dr["competitor_id"].ToString() != "0")
				{
					competitor_id = _dr["competitor_id"].ToString();
					competitor_name = set_competitor_name();
				}
				open_date = _dr["open_date"].ToString();
				date_due = _dr["date_due"].ToString();
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
				date_due = _dr["date_due"].ToString();
				stage1_approved = _dr["allowed_to_quote"] != DBNull.Value && Convert.ToBoolean(_dr["allowed_to_quote"]);
				stage4_approved = _dr["allowed_to_quote_2"] != DBNull.Value && Convert.ToBoolean(_dr["allowed_to_quote_2"]);
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
					Toolbox.doSQL_void(conn, @"UPDATE quote_master SET last_modified = NOW() WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
					var last_modified = Toolbox.doSQL_datetime(@"SELECT last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
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
				get_customer_contactinfo();
				this_company = new NeBusinessUnit(quoted_business_unit_id);
				if (this_member.AuthenticatedForPrivilege(61))
				{
					var locked_str = quoter_locked_bool ? "checked" : "";
					quoter_locked_str = string.Format("<input type='checkbox' data-is_new_quote='false' tabindex='14' height='15px' onchange='toggle_lock_quoted_by(this)' id='lock_quoter' {0}/><img hspace='5' src='/images/icon/icon[lock].gif' width='16' height='16'/>", locked_str);
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

				quoted_price = _dr["quoted_price"].ToString();
				price_to = _dr["price_to"].ToString();
				custom_term = Toolbox.do_value_from(_dr["custom_term"]);
				net_due = _dr["net_due"].ToString();
				percent_down = _dr["percentage_down"].ToString();
				pricetype_id = _dr["pricetype_id"].ToString();
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
					us_currency = "" + _dr["us_currency"];
					currency = 2;
				}
				active_revision = _dr["active_revision"].ToString() == "0" ? "" : "disabled checked";

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
					strategy_tab = @"<td class='h' data-title='Strategy Tab' style='width:35px'>
						<button type='button' class='normal' onclick=""tab_control(this, 'strategy');"" id='tab_strategy'>Strategy</button>
						<div class='tip'>" + field_title("strategy") + @"</div>";
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
		quote_id = Toolbox.doSQL_string(conn, @"CALL StartQuote(@v0 , @v1 , @v2 , @v3 , @v4 , @v5 )", new object[] { quoted_by, customer_id, contact_id, date_due, job_description, business_unit_id });
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
		var business_unit_id = _business_unit_id.ToString();
		var this_select = new StringBuilder();
		if (search_pane)
		{
			this_select.Append(@"<select onchange=""if(this.value != '0'){fill_quoters(this)}"" class='subquotedby_company'>");
		}
		else
		{
			this_select.AppendFormat(@"<select id='quoted_company' tabindex='11' onchange=""quote_obj.s('s_company', this);"">");
		}
		var _companys = Toolbox.doSQL_dt(conn, @"Select 0 id, 'All' name UNION SELECT id, ddl_name name from business_unit 
WHERE enable_timesheet = 1 and id in(" + new Current_User().visible_business_units + ") order by name", null);
		if (_companys.Rows.Count > 0)
		{
			foreach (DataRow row in _companys.Rows)
			{
				var this_id = row["id"].ToString();
				var this_name = row["name"].ToString();
				var this_selected = business_unit_id == this_id ? " selected" : "";
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
	public string competitor_id_from_name(string competitor_name)
	{
		this_string = Toolbox.doSQL_string(conn, @"SELECT id FROM quote_competitor  WHERE name = @v0", new object[] { competitor_name });
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

			price_to_box = string.Format("<input type='text' onchange='priceto(this);' id='price_to' value='{0}' {1} class='price'>", price_to, hide_me);
		}
		var po_link = po == "N/A" ? po : string.Format(@"{0}", po);
		var wo_link = "";
		if (wo != "N/A")
		{
			var woprog_id = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(woprog_id), 0) FROM woprog  WHERE woprog_bvwo =@v0", new object[] { wo.PadLeft(10, '0') });
			if (woprog_id != 0)
			{
				var woprog = new NeWOProg(woprog_id);
				wo_link = wo == "N/A" ? wo : string.Format(@"<a href=""javascript:boing('/sections/workorder/index.aspx?woprog_id={2}&business_unit_id={1}', 'wo');"">{0}</a>", woprog.OrderNumber, woprog.business_unit_id, woprog.woprog_id);
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
		var expected_value_update = expected_value == 0 ? "<input type='text' id='new_expected_value' value=''  onkeydown='only_numeric(event)'/><div style='font-size:10px;'>Please update the expected value of this quote</div>" : expected_value.ToString("C2");
		var worksheet_tab = this_member.AuthenticatedForPrivilege(163)
												? @"
					<td class='h' data-title='Worksheet Tab'>
						<button type='button' class='normal' onclick='javascript:boing(""/sections/member/picklist/pikclist.aspx?id=" + quote_id + "&rev=" + revision + @"&origin=quote"", ""WorkSheetPickList" + quote_id + @""", 1024, 800);' id='tab_worksheet'>Worksheet</button>
						<div class='tip'>" + field_title("worksheet") + @"</div>
					</td>"
												: "";
		var why_revised = Convert.ToInt32(revision) == 1 ? "" : Toolbox.doSQL_string(conn, @"SELECT why_revised FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
		var why_revised_html = why_revised != "" ? string.Format("<img src='/images/icon/icon[note].gif' width='14' height='18' align='absmiddle' style='margin:0px 3px 0px 3px;' title='{0}' />", HttpUtility.HtmlEncode(why_revised)) : "";
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
						<button type='button' onclick=""this.disabled = true; add_detail('detail', this)"" class='detail_add_new'>add item</button>
						<button data-keepenabled='1' type='button' onclick=""quote_obj.resize_textboxes(0);"" class='detail_resize'>resize all textboxes</button>
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
						<button type='button' onclick=""this.disabled = true; add_detail('notes', this)"" class='detail_add_new'>add item</button>
						<button type='button' onclick=""quote_obj.resize_textboxes(1);"" data-keepenabled='1' class='detail_resize'>resize all textboxes</button>
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
	address_options(),                                      // 74
	build_canned_descriptions(),                            // 75
	build_percent_spinner(),                                // 76
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
	public string build_percent_down()
	{
		var temp_select = new StringBuilder();
		temp_select.Append(@"<select class='percentdown' id='percent_down' tabindex='22' onchange=""quote_obj.s('s_pctdown', this)"">");
		for (var i = 0; i <= 100; i += 5)
		{
			var selected = i.ToString() == percent_down ? "selected" : "";
			temp_select.AppendFormat(@"<option value='{0}' {1}>{0}%</option>", i, selected);
		}
		temp_select.Append(@"</select>");
		return temp_select.ToString();
	}
	public string initial_quoted_by_list(int member_id, bool quoter_locked, bool search_pane)
	{
		var this_select = new StringBuilder();
		var disabled = quoter_locked ? " disabled='disabled'" : "";
		var c_id = search_pane ? this_member.business_unit_id.ToString() : quoted_business_unit_id;
		if (search_pane)
		{
			this_select.Append(@"<select class='subquotedby_name'><option value='0' selected>All</option>");
		}
		else
		{
			this_select.AppendFormat(@"<select class='quotedby' tabindex='13' id='quoted_by' onchange=""quote_obj.s('s_quotedby', this);"" {0}><option value='0'>Choose Quoter</option>", disabled);
		}
		foreach (DataRow dr in get_Quoters(c_id).Rows)
		{
			var id = Convert.ToInt32(dr["id"]);
			var name = dr["name"].ToString();
			var selected = id == member_id ? "selected" : "";
			this_select.AppendFormat(@"<option value='{0}' {2}>{1}</option>", id, name, selected);
		}
		this_select.Append("</select>");
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
		var hider = !this_company.is_er ? "style='display:none;'" : "";
		var disabler = wo == "" || wo == "N/A" ? " title='A work order has not been attached to this quote.' disabled" : "";
		return "<button type='button' onclick=\"print_quote('service_report', this);$('#print_pane').dialog('open');\" " + hider + " data-wo='" + wo + "' " + disabler + "><img src='/images/icon/icon[print].gif' width='16' height='16' /><br/>Service Report</button>";
	}
	public string build_percent_spinner()
	{
		var stringWriter = new StringWriter();
		using (var writer = new HtmlTextWriter(stringWriter))
		{
			var spinner = new ASPxSpinEdit();
			spinner.Theme = "NETheme01";
			spinner.CssClass = "spinner";
			spinner.ID = "pct_chance";
			spinner.MinValue = 30;
			spinner.Increment = 30;
			spinner.DecimalPlaces = 0;
			spinner.AllowMouseWheel = true;
			spinner.Text = pct_chance.ToString();
			spinner.MaxValue = 90;
			spinner.AllowUserInput = false;
			spinner.ShowOutOfRangeWarning = false;
			spinner.Width = Unit.Pixel(50);
			spinner.ClientInstanceName = "pct_chance";
			spinner.ClientSideEvents.NumberChanged = "function(s,e){quote_obj.s('s_pctchance', s.mainElement);}";
			spinner.RenderControl(writer);
		}
		var body = "<table cellpadding='0' cellspacing='0' width='100%'><tr><td class='v'>" + stringWriter + "</td>";

		stringWriter = new StringWriter();
		using (var writer = new HtmlTextWriter(stringWriter))
		{
			var reasons = new DropDownList();
			reasons.ID = "pct_chance_reason";
			reasons.Width = Unit.Percentage(75);
			var _reasons = Toolbox.doSQL_dt(conn, @"SELECT * FROM quote_chance ORDER BY quote_chance_id", null);
			foreach (DataRow _reason in _reasons.Rows)
			{
				var id = _reason["quote_chance_id"].ToString();
				var name = _reason["quote_chance_name"].ToString();
				reasons.Items.Add(new ListItem(name, id));
			}
			reasons.Items.FindByValue(pct_chance_reason).Selected = true;
			reasons.Attributes["onchange"] = "quote_obj.s('s_pctchancereason', this);";
			reasons.RenderControl(writer);
		}
		var show_note = pct_chance_reason == "6" ? "" : "display:none;";
		body += "<td nowrap='nowrap' class='v' width='100%' align='center'>Why? " + stringWriter + @"</td></tr><tr><td colspan='2'  class='v' style='padding:4px;' align='center'><textarea onchange=""quote_obj.s('s_pctchancenote', this);"" id='pct_chance_note' style='" + show_note + "width:98%;'>" + pct_chance_note + "</textarea></td></tr></table>";
		return body;
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
	private string address_options()
	{
		var temp_select = new StringBuilder();
		temp_select.Append(@"<select onchange=""quote_obj.s('s_addressid', this)"" tabindex='25' id='address_id'>");
		var _addresses = Toolbox.doSQL_dt(conn, @"SELECT * FROM address  WHERE address_table = 'customer' AND address_table_id =@v0", new object[] { customer_id });
		foreach (DataRow op in _addresses.Rows)
		{
			var this_address_id = op["address_id"];
			var this_address_type = op["address_type"];
			object this_address_name = Toolbox.do_value_from(op["address_desc"], false);
			object this_address_addr1 = Toolbox.do_value_from(op["address_addr1"], false);
			var this_selected = address_id == this_address_id.ToString() ? " SELECTED" : "";
			temp_select.AppendFormat("<option value='{0}'{3}>{1} - {2}</option>", this_address_id, this_address_type, this_address_addr1, this_selected);
		}
		temp_select.Append("</select>");
		return temp_select.ToString();
	}
	public string term_options()
	{
		var temp_select = new StringBuilder();
		temp_select.Append(@"<select class='netdue' id='net_due' tabindex='23' onchange=""quote_obj.s('s_netdue', this)"">");
		var options = Toolbox.doSQL_dt(conn, @"SELECT * FROM quote_term ORDER BY order_n", null);
		if (options.Rows.Count > 0)
		{
			foreach (DataRow _dr in options.Rows)
			{
				var id = _dr["id"].ToString();
				var selected = id == net_due ? " selected" : "";
				temp_select.AppendFormat(@"<option value='{0}' {2}>{1}</option>", id, _dr["description"], selected);
			}
		}
		/*
		<center>
			<input type='hidden' id='print_type' />
			<div>Email Quote to: <input type='text' value=""{31}"" /> <button style='margin-left:5px;border:solid 1px #000;' title='The email function has been disabled temporarily, please save the quote and email using Outlook.' onclick=""print_quote($('#print_type').val(), $(this).prev('input:text').val(), this);"" disabled>SEND</button></div>
			<div style='font-size:10px;'>This will send the quote to the contact address that you specify, aint with your email address for your records.</div>
		</center>
		 */
		temp_select.Append(@"</select>");
		return temp_select.ToString();
	}
	public string search_pane()
	{
		return string.Format(@"
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
							<td class='v'>{0}</td>
						</tr>
						<tr>
							<td class='c'>Quoted By (Name):</td>
							<td class='v'>{1}</td>
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
		",
		 company_list(this_member.business_unit_id, true),
		 initial_quoted_by_list(0, false, true)
		 );
	}
	public string divs(int which)
	{
		var div_name = which == 1 ? "detail" : which == 2 ? "notes" : "";

		var _dt = Toolbox.doSQL_dt(conn, @"SELECT * FROM quote_extratext WHERE quote_id = @v0  AND revision = @v1  AND type = @v2  ORDER BY line_number", new object[] { quote_id, revision, which });
		var sb = new StringBuilder();
		if (_dt.Rows.Count > 0)
		{
			var count = 1;
			foreach (DataRow _dr in _dt.Rows)
			{
				var line_text = Toolbox.do_value_from(_dr["linetext"]);
				var row_id = _dr["id"].ToString();
				var isDisabled = "";
				var picklist_button = "";
				if (div_name == "detail")
				{
					var current_section_total = Toolbox.doSQL_double(conn, @" SELECT IFNULL(SUM(a.extended_per),0) FROM quote_worksheet a LEFT JOIN quote_section b ON a.section_id = b.id WHERE b.detail_id = @v0 ", new object[] { row_id });
					var isReferenced = Toolbox.doSQL_int(conn, @"SELECT IF(COUNT(*) > 0, 1, 0) FROM quote_section WHERE detail_id = @v0 ", new object[] { row_id }) == 1;
					if (isReferenced)
					{
						var section_id = Toolbox.doSQL_int(conn, @"SELECT id FROM quote_section WHERE detail_id = @v0 ", new object[] { row_id });
						var _c = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM quote_worksheet WHERE section_id = @v0 ", new object[] { section_id });
						picklist_button = string.Format("<button type='button' onclick='get_section(this,{1})' id='s_{1}'>Section Total(~Ext'd): {0:C2}</button>", current_section_total, section_id);
					}
					else
					{
						picklist_button = "";
					}
				}
				isDisabled = string.Format(@"onclick=""this.disabled=true;del_extratext(this, '{0}');""", div_name);
				sb.AppendFormat(@"
					<li>
						<div data-row_id='{3}'>
							<table cellpadding=0 cellspacing=0 width='100%'>
								<tr>
								<td class='c' align='center'><span id='row_n'>{1}</span></td>
									<td class='t'>
<textarea onload='_resize(this);' onfocus='expand(this);' onchange=""quote_obj.s('s_{0}row', this)"">
{2}
</textarea>
									</td>
									<td class='action'><button type='button' {5}>X</button>{4}</td>
								</tr>
							</table>
						</div>
					</li>", div_name, count, line_text, row_id, picklist_button, isDisabled);
				count++;
			}
		}
		return sb.ToString();
	}
	/// <summary>
	/// Create a new container for the selected container type
	/// Types: 1=Details, 2=Notes &amp; Adders
	/// </summary>
	/// <param name="_type"></param>
	/// <returns></returns>
	public string new_extratext(int _type)
	{
		var max_line_number = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(line_number), -1) FROM quote_extratext WHERE quote_id = @v0  AND revision = @v1  AND type = @v2 ", new object[] { quote_id, revision, _type });
		max_line_number = max_line_number == -1 ? 0 : max_line_number + 1;
		var section_id = 0;
		if (quote_id == string.Empty)
		{
			throw new Exception("Blank Quote ID");
		}
		var extratext_id = Toolbox.doSQL_return_id(@" INSERT INTO quote_extratext ( quote_id, revision, type, line_number, linetext ) VALUES ( @v0 , @v1 , @v2 , @v3 , '' )", new object[] { quote_id, revision, _type, max_line_number });
		if (_type == 1)
		{
			section_id = Convert.ToInt32(save_section("", extratext_id));
			Toolbox.doSQL_void(conn, @"UPDATE quote_extratext SET section_id = @v0  WHERE id = @v1  LIMIT 1", new object[] { section_id, extratext_id });
		}
		return string.Format("{0},{1}", extratext_id, section_id);
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
		sql = string.Format("SELECT sellp FROM inventory_item_master WHERE master_id = {0}", master_id);
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
					<td colspan='2'><textarea id='why_lose' style='width:100%;height:150px;margin-bottom:10px;'>" + HttpContext.Current.Server.UrlDecode(why_lose) + @"</textarea></td>
				</tr>
				<tr>
					<td colspan='2'><b>Who was the competitor? (optional)</b></td>
				</tr>
				<tr>
					<td colspan='2'><textarea id='who_competitor' style='width:100%;height:150px;margin-bottom:10px;'>" + HttpContext.Current.Server.UrlDecode(who_competitor) + @"</textarea></td>
				</tr>
				<tr>
					<td colspan='2'><b>What was the price that it went for? (optional)</b></td>
				</tr>
				<tr>
					<td colspan='2'><textarea id='what_price' style='width:100%;height:150px;margin-bottom:10px;'>" + HttpContext.Current.Server.UrlDecode(what_price) + @"</textarea></td>
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
				return Toolbox.doSQL_string(conn, string.Format(@"SELECT COUNT(quote_id) C FROM quote_master WHERE quoted_by in ({0}) AND status_id = @v0 ", reports_list), new object[] { status_id });
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
			this_string = Convert.ToDouble(Toolbox.doSQL_string(conn, string.Format(@"SELECT IFNULL(SUM(if(status_id in(10,11,12),ifnull(expected_value,0),quoted_price)), 0) 
FROM quote_master  WHERE quoted_by in ({0}) AND status_id =@v0 ", reports_list), new object[] { status_id })).ToString("N2");
		}
		else
		{
			this_string = Convert.ToDouble(Toolbox.doSQL_string(conn, @"SELECT IFNULL(SUM(if(status_id=1 and ifnull(quoted_price,0)=0,ifnull(expected_value,0),ifnull(quoted_price,0))), 0) FROM quote_master  WHERE quoted_by =@v0 AND status_id =@v1 ", new object[] { quoted_by, status_id })).ToString("N2");
		}
		if (this_string == "0.00")
		{
			this_string = "<i>0.00</i>";
		}
		else
		{
			this_string = "<b>$" + this_string + "</b>";
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
	/// <param name="status_id"></param>
	/// <returns></returns>
	public string quote_list(string status_id) // Needs to be revisited
	{
		DataTable _dt;

		var columns = @"";
		if (status_id == "5")
		{
			_dt = Toolbox.doSQL_dt(conn, @" SELECT a.quote_id, a.revision, a.pricetype_id, a.price_to, c.type pricetype, CAST( DATE_FORMAT(a.open_date, '%m/%d/%Y') AS CHAR ) open_date, a.quoted_price, a.customer_id, a.job_description, IFNULL(a.allowed_to_quote, 0) allowed_to_quote, IFNULL(a.allowed_to_quote_2, 0) allowed_to_quote_2, IFNULL(a.expected_value, 0) expected_value, IFNULL(d.uses_quote_process, 0) uses_quote_process, IFNULL(d.quote_level_2_start, 0) quote_level_2_start, IFNULL(d.quote_level_3_start, 0) quote_level_3_start, a.quoted_by, e.member_fullname, d.ddl_name FROM quote_master a LEFT JOIN quote_follow_up b ON a.quote_id = b.quote_id AND a.revision = b.revision LEFT JOIN quote_pricetype c ON a.pricetype_id = c.id INNER JOIN business_unit d ON a.business_unit_id = d.id LEFT JOIN member e ON a.quoted_by = e.member_id WHERE a.quoted_by = @v0 AND a.active_revision = 1  AND b.schedule_date <= CURDATE() AND a.status_id NOT IN (6, 7, 8, 9) GROUP BY quote_id, revision", new object[] { quoted_by });
		}
		else if (status_id == "10" || status_id == "11" || status_id == "12")
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

			_dt = Toolbox.doSQL_dt(conn, string.Format(@" SELECT a.quote_id, a.revision, a.pricetype_id, b.type pricetype, a.price_to, CAST( DATE_FORMAT(a.open_date, '%m/%d/%Y') AS CHAR ) open_date, a.customer_id, a.quoted_price, a.job_description, IFNULL(a.allowed_to_quote, 0) allowed_to_quote, IFNULL(a.allowed_to_quote_2, 0) allowed_to_quote_2, IFNULL(a.expected_value, 0) expected_value, IFNULL(c.uses_quote_process, 0) uses_quote_process, IFNULL(c.quote_level_2_start, 0) quote_level_2_start, IFNULL(c.quote_level_3_start, 0) quote_level_3_start, a.quoted_by, d.member_fullname, c.ddl_name 
FROM quote_master a LEFT JOIN quote_pricetype b ON a.pricetype_id = b.id 
INNER JOIN business_unit c ON a.business_unit_id = c.id LEFT JOIN member d ON a.quoted_by = d.member_id 
WHERE ( a.quoted_by IN ({0})   AND a.active_revision = 1 AND a.status_id = @v0  AND c.id IN ({1}) ) ORDER BY a.open_date", reports_list, new Current_User().visible_business_units), new object[] { status_id });

		}
		else
		{
			_dt = Toolbox.doSQL_dt(conn, string.Format(@" SELECT a.quote_id, a.revision, a.pricetype_id, b.type pricetype, a.price_to, CAST( DATE_FORMAT(a.open_date, '%m/%d/%Y') AS CHAR ) open_date, 
a.customer_id, a.quoted_price, a.job_description, IFNULL(a.allowed_to_quote, 0) allowed_to_quote, IFNULL(a.allowed_to_quote_2, 0) allowed_to_quote_2, 
IFNULL(a.expected_value, 0) expected_value, IFNULL(d.uses_quote_process, 0) uses_quote_process, IFNULL(d.quote_level_2_start, 0) quote_level_2_start,
IFNULL(d.quote_level_3_start, 0) quote_level_3_start, a.quoted_by, c.member_fullname, d.ddl_name
FROM quote_master a 
LEFT JOIN quote_pricetype b ON a.pricetype_id = b.id INNER JOIN business_unit d ON a.business_unit_id = d.id
LEFT JOIN member c ON a.quoted_by = c.member_id WHERE a.quoted_by = @v0  AND a.status_id = @v1 AND a.active_revision = 1  AND d.id IN ({0}) AND a.active_revision = TRUE ORDER BY a.open_date", new Current_User().visible_business_units),
new object[] { quoted_by, status_id });
		}
		var this_list = new StringBuilder();
		this_string = "";
		if (_dt.Rows.Count > 0)
		{
			foreach (DataRow _dr in _dt.Rows)
			{
				var this_id = _dr["quote_id"].ToString();
				var this_date_open = _dr["open_date"].ToString();
				var this_quoted_price = _dr["quoted_price"].ToString();
				var this_custname = "";
				var this_price_to = _dr["price_to"].ToString();
				var this_pricetype_id = _dr["pricetype_id"].ToString();
				var this_pricetype = _dr["pricetype"].ToString();
				var this_allowed_to_quote = _dr["allowed_to_quote"].ToString();
				var this_allowed_to_quote_2 = _dr["allowed_to_quote_2"].ToString();
				var this_expected_value = _dr["expected_value"].ToString();
				var this_uses_quote_process = _dr["uses_quote_process"].ToString();
				var this_quote_level_2_start = _dr["quote_level_2_start"].ToString();
				var this_quote_level_3_start = _dr["quote_level_3_start"].ToString();
				var this_quoted_by = _dr["quoted_by"].ToString();
				var this_fullname = _dr["member_fullname"].ToString();
				var allowed_color = "";
				var allowed_color_2 = "";
				var name = _dr["ddl_name"].ToString();

				if (this_pricetype_id == "6")
				{
					this_quoted_price = _tools.Monetize(this_quoted_price) + " to " + _tools.Monetize(this_price_to);
				}
				else
				{
					this_quoted_price = _tools.Monetize(this_quoted_price);
				}
				var this_revision = _dr["revision"].ToString();
				customer_id = _dr["customer_id"].ToString();
				if (customer_id != "" && customer_id != "NULL")
				{
					get_customer_name();
				}
				this_custname = customer_name.Length > 14 ? customer_name.Substring(0, 12) + ".." : customer_name;
				var this_description = Toolbox.do_value_from(_dr["job_description"]);
				var quote_level = "";
				if (this_uses_quote_process == "1" && Convert.ToDouble(this_expected_value) > Convert.ToDouble(this_quote_level_3_start))
				{
					allowed_color = "<font color='red'>";
					quote_level = "<strong> Quote Level: Level 3 </strong><br/>";
				}
				else if (this_uses_quote_process == "1" && Convert.ToDouble(this_expected_value) > Convert.ToDouble(this_quote_level_2_start))
				{
					allowed_color = "<font color='red'>";
					quote_level = "<strong> Quote Level: Level 2 </strong><br/>";
				}
				if (status_id != "7")
				{

					//<div class='quote' data-title=""{7}"" onclick='location.href=""./index.aspx?a=g&quote_id={0}&revision={3}""'>
					//	<button type='button' class='normal' onclick='javascript:boing(""/sections/member/picklist/pikclist.aspx?id=" + quote_id + "&rev=" + revision + @"&origin=quote"", ""WorkSheetPickList"", 1024, 800);' id='tab_worksheet'>Worksheet</button>
					var linkOnclick = string.Format(@"onclick='javascript:boing(""index.aspx?a=g&quote_id={0}&revision={1}"",""quote_count{0}"",1060,920);'", this_id, this_revision);
					var linkClass = "quote";
					var divTitle = "";
					if (status_id == "10" || status_id == "11")
					{
						linkOnclick = "";
						linkClass = "quote disabled";
						divTitle = "Quote cannot be viewed while in " + (status_id == "10" ? "Stage 1" : "Stage 4") + " Go / No go";
					}
					this_list.AppendFormat(@"
							<div class='{13}' data-title=""{7}"" {14} >
								<div class='tip'>
									<div style='font-size:1.2em;font-weight:bold;'>{15}</div>
									<font size='1'>
										<strong>Version:</strong> {3}<br/>
										<strong>Open Date:</strong> {4}<br/>
										<strong>Price Type:</strong> {6}<br/>
										<strong>Quoted Price:</strong> {5}<br/>
										{10}
										<strong>Quoted By:</strong> {11}<br/>
<strong>Business Unit:</strong> {12}<br/>
									</font>
									<hr size='1'>
									{2}
								</div>
								<div class='quote_number'>{8}{0}</font></div>
								<div class='quote_name'>{8}{1}</font></div>
							</div>",
							this_id,            // 0
							GetfirstxCharacters(this_custname, 12),     // 1
							this_description,   // 2
							this_revision,      // 3
							this_date_open,     // 4
							status_id == "10" || status_id == "11" || status_id == "1" && Convert.ToDouble(_dr["quoted_price"] == DBNull.Value ? 0 : _dr["quoted_price"]) == 0 ? _tools.Monetize(this_expected_value) : this_quoted_price,  // 5
							this_pricetype,     // 6
							customer_name,      // 7
							allowed_color,      // 8
							allowed_color_2,        //9
							quote_level,    //10
							this_fullname,  //11
							name, //12
							linkClass,
							linkOnclick,
							divTitle
							);
				}
				else
				{
					this_list.AppendFormat(@"
							<div class='quote' onclick='javascript:boing(""index.aspx?a=g&quote_id={0}&revision={1}"",""quote_count{0}"",1060,920);''>
								<div class='tip'><i>VERSION {1}</i><br>No Description</div>
								<div class='quote_id'>{0}</div>
							</div>", this_id, this_revision);
				}
			}
		}
		else
		{
			this_list.Append("<b class='none'>None</b>");
		}
		return this_list.ToString();
	}
	public string revisions()
	{
		var _dt = Toolbox.doSQL_dt(conn, @"SELECT revision FROM quote_master WHERE quote_id = @v0  ORDER BY revision", new object[] { quote_id, revision });
		this_string = "";
		try
		{
			foreach (DataRow _dr in _dt.Rows)
			{
				var selected = "";
				if (_dr["revision"].ToString() == revision)
				{
					selected = " selected";
				}
				else
				{
					selected = "";
				}
				this_string += string.Format("\n<option value='{0}'{1}>v{0}</option>", _dr["revision"], selected);
			}
		}
		catch
		{
			this_string = "";
		}
		return this_string;
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
		if (s != null && s.Length > 0)
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
		if (field_name != "" && field_name != null && field_titles.ContainsKey(field_name))
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
		if (current_status_id != to_status_id)
		{
			Toolbox.doSQL_void(conn, @"UPDATE quote_master SET status_id = @v2 , last_print_date = now(), updated_by_page = 'quote - update_last_printed()' WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision, to_status_id });
		}
		return Toolbox.doSQL_string(conn, @"SELECT DATE_FORMAT(last_print_date, '%Y-%m-%d %H:%i:%s') last FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
	}
	public string get_last_ticks()
	{
		var last_modified = Toolbox.doSQL_datetime(@"SELECT last_modified FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { quote_id, revision });
		return last_modified.Ticks.ToString();
	}
	#endregion
	#region [DOUBLE] Methods (4)
	public double tm_pricing_total()
	{
		return Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM((IFNULL(A.original_sell, 0) * (1 - (quote_worksheet_discount/100))) * IFNULL(A.qty, 0)),0) AS extTandM FROM quote_worksheet A WHERE A.quote_id = @v0  AND A.revision = @v1 ", new object[] { quote_id, revision });
		//return Toolbox.doSQL_string(conn,@"SELECT CAST(IFNULL(FORMAT(SUM(qty*original_sell), 2), 0) AS CHAR) tm_pricing FROM quote_worksheet WHERE quote_id = @v0  AND revision = @v1 ", new object[] {  quote_id, revision } );
	}
	public double worksheet_total()
	{
		return Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(A.extended_per), 0) AS extended_per FROM quote_worksheet A WHERE A.quote_id = @v0  AND A.revision = @v1 ", new object[] { quote_id, revision });
		//return Toolbox.doSQL_string(conn,@"SELECT SUM(extended_per) FROM quote_worksheet WHERE quote_id = @v0  AND revision = @v1 ", new object[] {  quote_id, revision } );
	}
	public double worksheet_total(string section_id)
	{
		return Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(A.extended_per), 0) AS extended_per FROM quote_worksheet A WHERE A.quote_id = @v0  AND A.revision = @v1 ", new object[] { quote_id, revision });
		//return Toolbox.doSQL_string(conn,@"SELECT FORMAT(IFNULL(SUM(sell*qty), 0),2) total FROM quote_worksheet WHERE quote_id= @v0  AND revision = @v1  AND section_id = @v2 ", new object[] {  quote_id, revision, section_id } );
	}
	public double tm_pricing_total(string section_id)
	{
		return Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(IFNULL(A.original_sell, 0) * IFNULL(A.qty, 0)),0) AS extTandM FROM quote_worksheet A WHERE A.quote_id = @v0  AND A.revision = @v1 ", new object[] { quote_id, revision });
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
						var detail_name = string.Format("{0}. {1}", step, Toolbox.do_value_from(this_detail[1], false).Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "));
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
	public bool duplicate_quote()
	{
		try
		{
			quote_id = Toolbox.doSQL_string(conn, @"CALL DuplicateQuote(@v0 , @v1 )", new object[] { quote_id, revision });
			revision = "1";
		}
		catch
		{
			return false;
		}
		return true;
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
pricetype_id = @v12 , us_currency = @v13 , inflation_term = @v14 , include_title = @v20 , pct_chance = @v22 , pct_chance_reason = @v23 , currency = @v24 , exp_podate = @v25  WHERE quote_id = @v0  AND revision = @v15  ",
new object[] { quote_id, HttpUtility.UrlDecode(date_due),
				job_description, cust_spec_doc, quoted_by, HttpUtility.UrlDecode(last_print_date),
	HttpUtility.UrlDecode(last_fax_date), HttpUtility.UrlDecode(verified_date),
	try_price, custom_term, net_due, percent_down, pricetype_id, us_currency, inflation_term, revision, to_price, status_id, quoted_business_unit_id,
	completion_date, include_title, address_id, pct_chance, pct_chance_reason, currency, HttpUtility.UrlDecode(exp_podate) });

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
			if (this_branch_manager.id == 8)
			{
				this_branch_manager = new NeMember(9);
			}
			msg.To = this_branch_manager.NEEmail;
			msg.Subject = "Update on Quote #" + quote_id;
			if (quoted_business_unit_id == "1")
			{
				msg.CC = "mniesner@thatsnew.com";
			}
			msg.Body = this_string;
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
		sql = string.Format("UPDATE quote_master SET {0} = {1}, updated_by_page = 'quote - update_po_info()' WHERE quote_id = {2} AND revision = {3}", type, value, quote_id, revision);
		try
		{
			_tools.getSQL_bool(string.Format("UPDATE quote_master SET {0} = @v0, updated_by_page = 'quote - update_po_info()' WHERE quote_id =@v1 AND revision =@v2", type), new object[] { value, quote_id, revision });
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
	public DataTable get_Contacts(string cust_id)
	{
		sql = string.Format(@"
SELECT
	b.contact_id contact_id, 
	b.contact_name contact_name
FROM 
	customer a 
LEFT JOIN 
	contact b 
		ON 
		a.customer_id = b.contact_cust_id 
WHERE 
	a.customer_id = {0} AND
	b.contact_status = 'Active' AND
	b.contact_type = 'Customer' ORDER BY contact_name", cust_id);
		return Toolbox.doSQL_dt(conn, @" SELECT b.contact_id contact_id, b.contact_name contact_name FROM customer a LEFT JOIN contact b ON a.customer_id = b.contact_cust_id WHERE a.customer_id = @v0  AND b.contact_status = 'Active' AND b.contact_type = 'Customer' ORDER BY contact_name", new object[] { cust_id });
	}
	public DataTable get_Customers(string q)
	{
		return Toolbox.doSQL_dt(conn, @" SELECT a.customer_id c_id, a.customer_number c_number, a.customer_name c_name, customer_hold c_hold FROM customer a WHERE (a.customer_id like  CONCAT('%',@v0,'%')  OR a.customer_name like  CONCAT('%',@v0,'%') ) AND Customer_Status != 4 ORDER BY c_name", new object[] { q });
	}
	public DataTable get_Quoters(object _id)
	{
		if (quoter_locked_bool)
		{
			return Toolbox.doSQL_dt(conn, @" SELECT a.member_id id, a.member_fullname name FROM member a WHERE a.member_id = @v0  LIMIT 1", new object[] { quoted_by });
		}
		else
		{
			return Toolbox.doSQL_dt(conn, @"
SELECT 
	a.member_id id, 
	CONCAT(member_fullname,' (',bu.ddl_name,')') name 
FROM 
	member a
INNER JOIN 
	business_unit bu ON bu.id = a.business_unit_id
LEFT JOIN
       memberpage b ON a.member_id = b.memberpage_member_id 
WHERE 
	FIND_IN_SET(a.business_unit_id,@v0) AND 
	b.memberpage_page_id = 65 AND 
	a.member_status = 'Active' 
ORDER BY 
	bu.ddl_name,a.member_fullname"
				, new object[] { new Current_User().visible_business_units });
		}

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
		sql = string.Format(@"
SELECT
	*
FROM 
	quote_section
WHERE 
	quote_id = {0} AND 
	revision = {1}
ORDER BY
	section", quote_id, revision);
		return Toolbox.doSQL_dt(conn, @" SELECT * FROM quote_section WHERE quote_id = @v0  AND revision = @v1  ORDER BY section", new object[] { quote_id, revision });
	}
	public DataTable get_Worksheet()
	{
		sql = string.Format(@"
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
	b.quote_id = {0} AND 
	b.revision = {1}
ORDER BY
	section_id", quote_id, revision);
		return Toolbox.doSQL_dt(conn, @" SELECT b.id, b.section_id, b.part_no, ifnull(b.original_sell, 0) original_sell, b.code, b.description, b.sell, b.extended_per, b.qty, b.cost FROM quote_worksheet b WHERE b.quote_id = @v0  AND b.revision = @v1  ORDER BY section_id", new object[] { quote_id, revision });
	}
	public DataTable get_followup_history()
	{
		sql = string.Format(@"
SELECT * FROM
	(
	SELECT
		CAST(CONCAT('f',id) AS CHAR) id,
		CAST(schedule_date AS CHAR) date,
		b.member_fullname name,
		'f' type,
		note
	FROM 
		quote_follow_up a
	LEFT JOIN
		member b ON a.editted_by = b.member_id
	WHERE 
		quote_id = {0} AND 
		revision = {1}
UNION
	SELECT
		CAST(CONCAT('h',id) AS CHAR) id,
		CAST(DATE_FORMAT(create_datetime, '%Y-%m-%d - %l:%i:%s%p') AS char) date,
		b.member_fullname name,
		'h' type,
		event note
	FROM 
		quote_history a 
	LEFT JOIN
		member b ON
			a.created_by = b.member_id
	WHERE 
		quote_id = {0} AND 
		revision = {1}
UNION
	SELECT
		CAST(CONCAT('l',a.id) AS CHAR) id,
		CAST(DATE_FORMAT(a.dt, '%Y-%m-%d - %l:%i:%s%p') AS CHAR) date,
		e.member_fullname name,
		's' type,
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
		a.associated_table_id = '{0}' AND
		a.associated_alt_table_id = '{1}' AND
		a.action_id = 4
UNION
	SELECT 
		CAST(CONCAT('n',a.woprog_project_notes_id) AS CHAR) id,
		CAST(DATE_FORMAT(a.woprog_project_notes_notes, '%Y-%m-%d - %l:%i:%s%p') AS CHAR) date,
		b.member_fullname name,
		'n' type,
		woprog_project_notes_notes note
	FROM
		woprog_project_notes a
	LEFT JOIN
		member b ON a.woprog_project_notes_memberid = b.member_id
	where 
		woprog_project_notes_Type = 'Q' AND 
		woprog_project_notes_woprogid = {0}
	) history ORDER BY date DESC;
	
", quote_id, revision);
		return Toolbox.doSQL_dt(conn, @" SELECT * FROM ( SELECT CAST(CONCAT('f',id) AS CHAR) id, CAST(schedule_date AS CHAR) date, b.member_fullname name, 'f' type, note FROM quote_follow_up a LEFT JOIN member b ON a.editted_by = b.member_id WHERE quote_id = @v0  AND revision = @v1  UNION SELECT CAST(CONCAT('h',id) AS CHAR) id, CAST(DATE_FORMAT(create_datetime, '%Y-%m-%d - %l:%i:%s%p') AS char) date, b.member_fullname name, 'h' type, event note FROM quote_history a LEFT JOIN member b ON a.created_by = b.member_id WHERE quote_id = @v0  AND revision = @v1  UNION SELECT CAST(CONCAT('l',a.id) AS CHAR) id, CAST(DATE_FORMAT(a.dt, '%Y-%m-%d - %l:%i:%s%p') AS CHAR) date, e.member_fullname name, 's' type, CAST(CONCAT(b.name, "" from: '"", c.status,""' to '"", d.status, ""'"") AS CHAR) note FROM log a LEFT JOIN log_action b ON a.action_id = b.id LEFT JOIN quote_status c ON a.value_old = c.id LEFT JOIN quote_status d ON a.value_new = d.id LEFT JOIN member e ON a.member_id = e.member_id WHERE a.associated_table = 'quote_master' AND a.associated_table_id = @v0  AND a.associated_alt_table_id = @v1  AND a.action_id = 4 UNION SELECT CAST(CONCAT('n',a.woprog_project_notes_id) AS CHAR) id, CAST(DATE_FORMAT(a.woprog_project_notes_notes, '%Y-%m-%d - %l:%i:%s%p') AS CHAR) date, b.member_fullname name, 'n' type, woprog_project_notes_notes note FROM woprog_project_notes a LEFT JOIN member b ON a.woprog_project_notes_memberid = b.member_id where woprog_project_notes_Type = 'Q' AND woprog_project_notes_woprogid = @v0  ) history ORDER BY date DESC ", new object[] { quote_id, revision });
	}

	public DataTable get_Worksheet(string this_section_id)
	{
		sql = string.Format(@"
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
	a.quote_id = {0} AND 
	a.revision = {1} AND
	b.section_id = {2}
ORDER BY
	section_id", quote_id, revision, this_section_id);
		return Toolbox.doSQL_dt(conn, @" SELECT b.id, b.section_id, b.part_no, b.code, ifnull(b.original_sell, 0) original_sell, b.description, b.sell, b.extended_per, b.qty, b.cost FROM quote_section a LEFT JOIN quote_worksheet b ON a.id = b.section_id WHERE a.quote_id = @v0  AND a.revision = @v1  AND b.section_id = @v2  ORDER BY section_id", new object[] { quote_id, revision, this_section_id });
	}
	public DataTable search_Quotes(string this_description, string this_quoted_by, string this_customer, string this_before, string this_after, string this_status, string this_quote_id, string this_business_unit_id, string member_id)
	{
		var appendage = "";
		sql = @"
SELECT 
	a.quote_id, 
	(select max(g.revision) FROM quote_master g WHERE quote_id = a.quote_id AND status_id != 9) revision, 
	CAST(DATE_FORMAT(a.open_date, '%m/%d/%Y') AS CHAR) open_date,
	ifnull(REPLACE(a.job_description, CHAR(0), ' '), '--') job_description,
	ifnull(b.customer_name, '--') customer_name,
	'--' phone,
	CAST(ifnull(FORMAT(a.quoted_price, 2), '--') AS CHAR(20)) quoted_price,
	Concat(ifnull(e.member_fullname, '--'),' (',bb.ddl_name,')') quoted_by,
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
left join business_unit bb on a.business_unit_id = bb.id
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
		sql = sql + appendage + @" and find_in_set(a.business_unit_id,get_visible_business_units_group_concat(" + member_id + @"))
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