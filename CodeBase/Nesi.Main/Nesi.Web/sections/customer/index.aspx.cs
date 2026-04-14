using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Services;
using System.Text.RegularExpressions;
using nesi.core;

public partial class sections_customer_index : System.Web.UI.Page
	{
	public Toolbox _tools					= new Toolbox();
	private NeMember myMember;
	protected NameValueCollection _q;
	public string callback_control			= "";
	private const int _page_id			= 10;
	private bool _is_new_allowed		= false;
	private bool _is_edit_allowed		= false;
	private bool _isExtContact = false;
	private Parameter _customer_id		= new Parameter("@customer_id");
	ne_page_log pl						= new ne_page_log();
	private string _cust_id = "";
	protected void Page_Init(object sender, EventArgs e)
		{
		    var _q = Request.QueryString;

		    if (!string.IsNullOrEmpty(_q["customer_id"]))
		    {
		        Response.Redirect("/#/opens/10/customers/" + _q["customer_id"]);

        }
		        Response.Redirect("/#/opens/10/customers/");
		    
            

        Toolbox.do_debug("Customer Page Init Start");
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		_q									= Request.QueryString;
		_tools.dont_cache_page();
		pl.ip_address			= Request.UserHostAddress;
		pl.member_id			= myMember.id;
		pl.url					= Request.Url.AbsolutePath;
		pl.query_string			= Request.Url.Query;
		pl.request_start		= DateTime.Now;
		pl.save();
		pl_id.Value				= pl.id.ToString();
		if(!IsPostBack)
			{
			ViewState["address_id"]	= null;
			Session["customer_id"]	= null;
			ViewState["customer_id"]	= null;
			Session["business_unit_id"]	= null;
			Session["cust_emails"] = null;
			}
		if (!string.IsNullOrEmpty(_q["customer_id"]))
			{
			_cust_id = _q["customer_id"];
			Session["customer_id"] = _cust_id;
			}
		if ((Session["customer_id"] != null) && (Session["customer_id"] != ""))
		{
			uc_accounting.customer_id = Convert.ToInt32(Session["customer_id"]);
			uc_accounting.this_customer = new NECustomer(Convert.ToInt32(Session["customer_id"]));
			uc_accounting.init();
			uc_accounting.DataBind();
		}
	//	reset_params();
		Toolbox.do_debug("Customer Page Init End");
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		Toolbox.do_debug("Customer Page Load Start");
		_tools = new Toolbox();
	//	ddl_address.DataBind();
		_is_new_allowed = myMember.AuthenticatedForPrivilege(9); // New Customer
		_is_edit_allowed = myMember.AuthenticatedForPrivilege(10); // Edit Customer
		_q = Request.QueryString;
		if (!string.IsNullOrEmpty(_q["customer_id"]))
			{
			_cust_id = _q["customer_id"];
			Session["customer_id"] = _cust_id;
			
			}
		_isExtContact = myMember.isContact;
		if (_isExtContact)
			{
			_cust_id = new NEContact(Convert.ToInt32(myMember.ContactID)).Contact_Cust_ID.ToString();
			}
		//btn_save.Visible = false;//_is_edit_allowed;
		//btn_new_address.Visible = _is_edit_allowed;
		//btn_phone_save.Visible = _is_edit_allowed;
		//btn_bv_contacts.Visible = true;

		Toolbox.StyleReferenceManager.AddStyleLinksToHead(this);
		if ((_q["from_wo"] == null) && (!_isExtContact))
			{
			btn_new.Visible = _is_new_allowed;
			btn_postalsearch.Visible = _is_new_allowed;
			cust_search_t.Visible = true;
			cust_search_b.Visible = true;
			cust_page_c.TabPages[0].Visible = true;


			}
		else
			{
			btn_new.Visible = false;
			cust_search_t.Visible = false;
			cust_search_b.ClientVisible = false;
			cust_page_c.TabPages[0].ClientVisible = false;
			if (_isExtContact == true)
				{
				right_tabs.TabPages[3].ClientVisible = false;
				right_tabs.TabPages[9].ClientVisible = false;
				right_tabs.TabPages[4].ClientVisible = false;
				//contact_tabber.TabPages[1].ClientVisible = false;
				}
			


			}

		if (!IsCallback && !IsPostBack)
			{
			main_persistence_handler.Clear();
	//		Session["customers_gv_parts"] = null;
			
			}
		if (!string.IsNullOrEmpty(_q["customer_id"]))
			{
			_cust_id = _q["customer_id"];
			}
		cust_search_t.Attributes["type"]			= "search";
		//sales_reps.Visible						= myMember.membertype.id == vp_sales;
		if ((_cust_id != "") && !chk("customer_id"))
			{
			var does_exist = Convert.ToBoolean(_tools.getSQL_int(@"SELECT COUNT(*) FROM customer WHERE customer_id = @v0  LIMIT 1", new object[] {  _tools.value_to(_cust_id) } ));
			if (!does_exist)
				{
				Response.Clear();
				Response.Write("Invalid Customer ID");
				Response.End();
				}
			put("customer_id", _cust_id);
			cust_page_c.TabPages[1].ClientEnabled = true;
			cust_page_c.ActiveTabPage = cust_page_c.TabPages[1];
			right_tabs.ActiveTabPage = right_tabs.TabPages[0];
			put("from_querystring", "true");
			FillCustomerMain(true);
			reset_params();
		
			
			}
		else
			{
			if (!chk("detail_gen_company"))
				{
				put("detail_gen_company", myMember.business_unit_id);
				}
			if (!IsCallback)
				{
				cust_page_c.ActiveTabPage = cust_page_c.TabPages[0];
				cust_search_t.Focus();
			//	fill_popup();
				}
			if (exists("customer_id") &&
				grab("customer_id") != "0" &&
				!bv_data.IsCallback/* &&
				!nc_phone_check.IsCallback &&
				!nc_name_check.IsCallback*/
				//!add_phone_check.IsCallback &&
				//!cb_current_name_check.IsCallback
				)
				{
				FillCustomerMain(false);

				cust_page_c.TabPages[1].ClientEnabled = true;
				var active_tab_index = grab("active_tab_index") != "" ? Convert.ToInt32(grab("active_tab_index")) : 0;
				cust_page_c.ActiveTabPage = cust_page_c.TabPages[1];
				right_tabs.ActiveTabPage = right_tabs.TabPages[active_tab_index];
				}
			}
		//acct_discount_code_text.ClientEnabled = myMember.AuthenticatedForPrivilege(10, 94);
		if(!IsPostBack)
			{
			uc_new.business_unit_id			= myMember.business_unit.id;
			}
		Toolbox.do_debug("Customer Page Load End");
		}
	protected void Page_Unload(object sender, EventArgs e)
		{
		pl.request_end				= DateTime.Now;
		pl.save();
		}
	protected void reset_params()
		{
        
   //     int _index = Convert.ToInt32(grab("customer_id"));
   //     int customer_id = Convert.ToInt32(search_results_gv.GetRowValues(_index, "customer_id"));
		Session["customer_id"]			= exists("customer_id") && grab("customer_id") != "0" ? grab("customer_id") : "0";
		ViewState["customer_id"]			= exists("customer_id") && grab("customer_id") != "0" ? grab("customer_id") : "0";
//        Session["customer_id"] = customer_id.ToString();
		}
	protected void cust_main_cb(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		_q									= Request.QueryString;
		NECustomer _customer;
		callback_control							= e.Parameter;
		switch(callback_control)
			{
			#region case "search":
			case "search":
				cust_page_c.ActiveTabPage				= cust_page_c.TabPages[0];
				search_results_gv.DataBind();
			break;
			#endregion case "search":
			#region case "load_customer":
			case "load_customer":
				cust_page_c.TabPages[1].ClientEnabled	= true;
				search_results_gv.DataSourceID			= "";
				cust_page_c.ActiveTabPage				= cust_page_c.TabPages[1];
				right_tabs.ActiveTabPage				= right_tabs.TabPages[0];
				//(For Now)//btn_save.Enabled					= false;
				var _index								= Convert.ToInt32(grab("customer_id"));
				var customer_id = Convert.ToInt32(_index);
				_customer								= new NECustomer(customer_id);
				main_persistence_handler.Clear();
				main_persistence_handler["customer_id"]	= customer_id.ToString();
				
				

				reset_params();
				Session["customer_id"] = customer_id.ToString();
				ViewState["customer_id"] = customer_id.ToString();
				Session["cust_emails"] = null;
				uc_accounting.customer_id = _customer.id;
				uc_accounting.address_id = _customer.Address.id;
				ddl_address.DataBind();
				ddl_address.Value = null;
			

				FillCustomerMain(true);

			//	ddl_address.Value = Convert.ToInt32(_customer.Address.ID);
			//	gv_workorders.DataBind();
			//	gv_quotes.DataBind();
			
			//	gv_phonecalls.DataBind();

			break;
			#endregion case "load_customer":
			#region case "load_address_contacts":
			case "load_address_contacts":
				//fill_bv_contacts();
				//fill_address();
			break;
			#endregion case "load_address_contacts":
			#region case "save_address_contacts":
			case "save_address_contacts":
				/*
				if(grab("active_address_id") == "")
					{
					throw new Exception("Address ID not set... this is bad. Not proceeding.");
					}
				else
					{
					string address_id			= grab("active_address_id");
					_customer					= new NECustomer(Convert.ToInt32(grab("customer_id")));
					BVContact _contact1			= new BVContact(address_id, 1);
					BVContact _contact2			= new BVContact(address_id, 2);
					BVContact _contact3			= new BVContact(address_id, 3);
					try
						{
						_contact1.Name				= grab("bv_contact1_name");
						_contact1.Phone_Area		= grab("bv_contact1_phone1");
						_contact1.Phone_First		= grab("bv_contact1_phone2");
						_contact1.Phone_Last		= grab("bv_contact1_phone3");
						_contact1.Phone_Ext			= grab("bv_contact1_phoneext");
						_contact1.Fax_Area			= grab("bv_contact1_fax1");
						_contact1.Fax_First			= grab("bv_contact1_fax2");
						_contact1.Fax_Last			= grab("bv_contact1_fax3");
						_contact1.Email				= grab("bv_contact1_email");
						_contact1.Save();
						
						_contact2.Name				= grab("bv_contact2_name");
						_contact2.Phone_Area		= grab("bv_contact2_phone1");
						_contact2.Phone_First		= grab("bv_contact2_phone2");
						_contact2.Phone_Last		= grab("bv_contact2_phone3");
						_contact2.Phone_Ext			= grab("bv_contact2_phoneext");
						_contact2.Fax_Area			= grab("bv_contact2_fax1");
						_contact2.Fax_First			= grab("bv_contact2_fax2");
						_contact2.Fax_Last			= grab("bv_contact2_fax3");
						_contact2.Email				= grab("bv_contact2_email");
						_contact2.Save();
						
						_contact3.Name				= grab("bv_contact3_name");
						_contact3.Phone_Area		= grab("bv_contact3_phone1");
						_contact3.Phone_First		= grab("bv_contact3_phone2");
						_contact3.Phone_Last		= grab("bv_contact3_phone3");
						_contact3.Phone_Ext			= grab("bv_contact3_phoneext");
						_contact3.Fax_Area			= grab("bv_contact3_fax1");
						_contact3.Fax_First			= grab("bv_contact3_fax2");
						_contact3.Fax_Last			= grab("bv_contact3_fax3");
						_contact3.Email				= grab("bv_contact3_email");
						_contact3.Save();
						_customer.sync_bvs(myMember);
						_customer.add_history("BV Contacts Updated", myMember, myMember.business_unit_id);
						}
					catch (Exception)
						{
						throw;
						}
					}
				*/
			break;
			#endregion case "save_address_contacts":
			#region case "get_number":
			case "get_number":
				//_number						= new NePhoneNumbers(grab("selected_phone_item"));
				//phone_type.SelectedItem		= phone_type.Items.FindByValue(_number.phone_numbers_comm_type);
				//phone_number.Text			= _number.phone_numbers_number;
				//fill_address();
				//fill_associated();
				//fill_phones();
			break;
			#endregion case "get_number":
			#region case "switch_address"/"cancel_shipping":
			/*
			case "cancel_shipping":
			case "switch_address":
				put("is_new_address", "false");
				if(e.Parameter == "cancel_shipping")
					{
					put("active_address_id", address_list.Items[0].Value.ToString());
					address_list.SelectedItem			= address_list.Items.FindByValue(grab("active_address_id"));
					phone_address_list.SelectedItem		= phone_address_list.Items.FindByValue(grab("active_address_id"));
					contact_address_list.SelectedItem	= contact_address_list.Items.FindByValue(grab("active_address_id"));
					}
				fill_address();
				fill_bv_contacts();
				fill_phones();
			break;
			 */
			#endregion case "switch_address"/"cancel_shipping":
			#region case "save_customer":
			case "save_customer":
				if(CheckValidity())
					{
					if(exists("customer_id"))
						{
						_customer						= new NECustomer(Convert.ToInt32(grab("customer_id")));
						}
					else
						{
						throw new Exception("Trying to save customer without customer ID set...");
						}
					if(grab("ts") != _customer.ts.Ticks.ToString())
						{
						throw new Exception("This customer's information has changed since it was last loaded. \nPlease refresh the page or reload this customer");
						}
					_customer.business_unit_id					= Convert.ToInt32(grab("detail_gen_company"));
					_customer.Customer_Name						= grab("detail_customer_name");
					_customer.Address.GPS_Coords				= grab("detail_gen_gps");
					//_customer.GL_ID								= grab("acct_gl_account") == "" ? _customer.GL_ID : grab("acct_gl_account");
					//_customer.Credit_Type						= grab("acct_credit_type") == "" ? _customer.Credit_Type : Convert.ToInt32(grab("acct_credit_type"));
					_customer.Account_Manager					= grab("detail_gen_acctmgr") == "" ? 0 : Convert.ToInt32(grab("detail_gen_acctmgr"));
					_customer.ControlsGuy_MemberID				= grab("detail_gen_controlsguy") == "" ? 0 : Convert.ToInt32(grab("detail_gen_controlsguy"));
					//_customer.CreditLimit						= grab("acct_credit_limit") == "" ? _customer.CreditLimit : Convert.ToDouble(grab("acct_credit_limit"));
					//_customer.StatementType						= grab("acct_statements") == "" ? _customer.StatementType : grab("acct_statements");
					//_customer.InvoiceType						= grab("acct_invoices") == "" ? _customer.InvoiceType : grab("acct_invoices");
					//_customer.default_invoicetype				= grab("acct_default_invoicetype") == "" ? _customer.default_invoicetype : Convert.ToInt32(grab("acct_default_invoicetype"));
					//_customer.TaxPrompt							= grab("acct_prompt_tax") == "True" ? "T" : "F";
					//_customer.PORequired						= grab("acct_po_required") == "True" ? "T" : "F";
					//_customer.customer_autostatements			= grab("acct_default_invoicetype") == "" ? _customer.customer_autostatements : Convert.ToInt32(grab("acct_auto_statement"));
					//_customer.customer_auto_invoice				= grab("acct_auto_invoicing") == "" ? _customer.customer_auto_invoice : Convert.ToInt32(grab("acct_auto_invoicing"));
					//_customer.customer_autostatement_address	= grab("txtStatementEmail") == "" ? _customer.customer_autostatement_address : grab("txtStatementEmail");
					//_customer.customer_autostatement_ccaddress	= grab("txtStatementCCEmail") == "" ? _customer.customer_autostatement_ccaddress: grab("txtStatementCCEmail");
					//_customer.customer_invoice_address			= grab("txtInvoiceEmail") == "" ? _customer.customer_invoice_address : grab("txtInvoiceEmail");
					//_customer.customer_invoice_ccaddress		= grab("txtInvoiceCCEmail") == "" ? _customer.customer_invoice_ccaddress : grab("txtInvoiceCCEmail");
					//_customer.customer_creditdays				= grab("acct_dayscredit") == "" ? _customer.customer_creditdays : Convert.ToInt32(grab("acct_dayscredit"));

			//		_customer.customer_arnotes = grab("customer_arnotes_text");
			//		_customer.Memo=grab("customer_memo_text");
			//		_customer.salesnotes=grab("customer_salesnotes_text"); 
			//		_customer.Notes= grab("customer_notes_text");
					/*
					if (grab("on_hold") == "True")
						{
						if (_customer.Hold == "F")
							{
							if (grab("memwhyhold").ToString() == "")
								{
								//status_message.Text = "You must enter a reason for why you are putting this customer on hold.";
								//status_message.Style["color"] = "#FF0000";

								return;
								}
							}
						_customer.customer_whohold = myMember.id;
						_customer.customer_whyhold = grab("memwhyhold").ToString();
						_customer.Hold = "T";
						NECustomer.add_to_customer_history(Convert.ToInt32(_customer.Customer_ID), myMember.id, System.DateTime.Now, _customer.customer_whyhold, 19, 1);
						}
					else
						{
						if (_customer.Hold == "T")
							{
							NECustomer.add_to_customer_history(Convert.ToInt32(_customer.Customer_ID), myMember.id, System.DateTime.Now, _customer.customer_whyhold, 20, 1);
							}
						_customer.customer_whohold = 0;
						//memwhyhold.Text = "";
						_customer.customer_whyhold = "";
						_customer.Hold = "F";
						}
					*/
					//if (grab("acct_discount_code_text") != "" && _customer.Discount < Convert.ToDouble(grab("acct_discount_code_text")))
					//	{
					//	try
					//		{
					//		NeEMail email = new NeEMail();
					//		email.Subject = _customer.Customer_Name + " has had their discount raised to " + Convert.ToDouble(grab("acct_discount_code_text")) + "% by " + myMember.FullName;
					//		DataTable bms = _tools.getSQL_datatable(@"Select member_neemail from member  where member_membertype_id = 5 or member_membertype_id = 29 and member_status = 'Active'" , null);
					//		foreach (DataRow dr in bms.Rows)
					//			{
					//
					//			email.To = dr[0].ToString();
					//
					//			email.From = "CustomerDiscounts@thatsnew.com";
					//			try
					//				{
					//				email.Send();
					//				}
					//			catch
					//			{ }
					//			}
					//		}
					//	catch (Exception ex)
					//	{ }
					//
					//	}
					//
					//double temp_discount = _customer.Discount;
					//_customer.Discount = acct_discount_code_text.Enabled && grab("acct_discount_code_text") != "" ? Convert.ToDouble(grab("acct_discount_code_text")) : _customer.Discount;
					//_customer.PriceCode = grab("acct_discount_code_ddl") != "" ? grab("acct_discount_code_ddl") : _customer.PriceCode;
					//_customer.ApplyFinanceCharges = grab("acct_finance_charges") == "" ? _customer.ApplyFinanceCharges : grab("acct_finance_charges") == "True" ? "T" : "F";
					//try
					//	{
					//	_customer.Discount = grab("acct_discount_code_text") == "" ? _customer.Discount : Convert.ToDouble(grab("acct_discount_code_text"));
					//	if (_customer.Discount > 25)
					//		{
					//		_customer.Discount = 25;
					//		acct_discount_code_text.Text = "25";
					//		}
					//	}
					//
					//
                    //catch
					//{ _customer.Discount = 0;
                    //acct_discount_code_text.Text = "0";
                    //    }
					//if (temp_discount!=_customer.Discount)
					//{
					//NECustomer.add_to_customer_history(Convert.ToInt32(_customer.Customer_ID), myMember.id, System.DateTime.Now, "Customer discount changed to " + _customer.Discount, 6, 1);
					//}
					//int saved							= _customer.Save();
					//save_address(_customer.Address);
					//_customer							= new NECustomer(Convert.ToInt32(grab("customer_id")));
					//put("ts", _customer.ts.Ticks.ToString());
					//_customer.sync_bvs(myMember);
					//if(saved.ToString() == grab("customer_id"))
					//	{
					//	status_message.Text					= "saved";
					//	status_message.Style["color"]		= "#0c0";
					//	fill_associated();
					//	fill_phones();
					//	}
					//else if(saved != 0)
					//	{
					//	status_message.Text					= "saved";
					//	status_message.Style["color"]		= "#0c0";
					//	put("customer_id", saved.ToString());
					//	fill_associated();
					//	fill_phones();
					//	}
					//else
					//	{
					//	status_message.Text					= "There were problems saving this customer";
					//	status_message.Style["color"]		= "#fc0";
					//	}
					

			#region check for qc2 prereqs
			var disabled_reasons						= new List<string>();
			// auto statement email address
			if(string.IsNullOrEmpty(_customer.customer_autostatement_address))
				{
				disabled_reasons.Add("Customer's auto-statement email address is empty");
				}
			// auto invoice email address
			if(string.IsNullOrEmpty(_customer.customer_invoice_address))
				{
				disabled_reasons.Add("Customer's invoice email address is empty");
				}
			// Credit limit other than 0
			if(_customer.CreditLimit == 0)
				{
				disabled_reasons.Add("Customer's credit limit is zero");
				}
			// Credit days other than 0
			if(_customer.customer_creditdays == 0)
				{
				disabled_reasons.Add("Customer's credit days is zero");
				}
			if(disabled_reasons.Count > 0)
				{
				bt_qc2approve.ClientEnabled						= false;
				bt_qc2approve.CssClass							= "ttip";
				var bt_qc2approve_tooltip					= "<div>These issues need to be fixed before this customer can be level 2 qc'ed</div><ul>";
				for(var i = 0; i < disabled_reasons.Count; i++)
					{
					bt_qc2approve_tooltip						+= "<li>"+disabled_reasons[i]+"</li>";
					}
				bt_qc2approve_tooltip							+= "</ul>";
				bt_qc2approve.Attributes.Add("data-title", "Problems found");
				bt_qc2approve.Attributes.Add("data-tooltip", bt_qc2approve_tooltip);
				}
			else
				{
				bt_qc2approve.ClientEnabled						= true;
				bt_qc2approve.CssClass							= "";
				}
			#endregion check for qc2 prereqs
					}
				else
					{
					//status_message.Text					= "couldn't save";
					//status_message.Style["color"]		= "#f00";
					//fill_companies();
					//fill_gls();
					//fill_provs();
					//fill_phones();
					//fill_countries();
					//fill_acctmgrs();
					//fill_controlguys();
					//fill_taxes();
					}

			break;
			#endregion case "save_customer":
			#region case "start_customer":
			case "start_customer":
				ASPxWebControl.RedirectOnCallback(string.Format("./index.aspx?customer_id={0}", _cust_id));
			break;
			#endregion case "start_customer":
			#region case "save_phone":
			case "save_phone":
				/*
				string _phone_number		= grab("phone_number").Replace(" ", "").Replace("-", "").Replace(".", "");
				if(_phone_number.Length != 10)
					{
					throw new Exception("Please format your phone number as ### ### #### or ##########");
					}
				else
					{
					_phone_number			= Convert.ToInt32(_phone_number).ToString("### ### ####");
					}
				if(exists("customer_id"))
					{
					_customer				= new NECustomer(Convert.ToInt32(grab("customer_id")));
					}
				else
					{
					throw new Exception("Trying to save phone number without customer ID set...");
					}
				string _phone_type			= grab("phone_type");
				try
					{
					if(exists("selected_phone_item"))
						{
						//_number								= new NePhoneNumbers(grab("selected_phone_item"));
						main_persistence_handler.Remove("selected_phone_item");
						}
					else
						{
						_number								= new NePhoneNumbers();
						_number.phone_numbers_type			= "Address";
						_number.phone_numbers_table_id		= _tools.getSQL_int(@"SELECT address_id FROM address where address_table_id = @v0  LIMIT 1", new object[] {  grab("customer_id") } );
						_number.phone_numbers_default		= false;
						_number.phone_numbers_active		= true;
						}
					_number.phone_numbers_comm_type			= _phone_type;
					_number.phone_numbers_number			= _phone_number;
					_number.Save();
					_customer.sync_bvs(myMember);
					//phone_type.SelectedIndex				= 0;
					//phone_list.SelectedIndex				= -1;
					//phone_number.Text						= "";
					fill_phones();
					//phone_number.Focus();
					_customer.add_history(String.Format("Phone number saved ({0}) Added", _phone_number), myMember, myMember.business_unit_id);
					}
				catch(Exception ee)
					{
					throw ee;
					}
				 */
			break;
			#endregion case "save_phone":
			#region case "new_customer":
			case "new_customer":
				cust_page_c.TabPages[1].ClientEnabled	= true;
				cust_page_c.ActiveTabIndex				= 1;
				right_tabs.ActiveTabIndex				= 0;
				ClearCustomerMain();
				//fill_companies();
				//fill_gls();
				//fill_provs();
				//fill_countries();
				//fill_acctmgrs();
				//fill_controlguys();
				//fill_taxes();
			break;
			#endregion case "new_customer":
			#region case "switch_branch":
			case "switch_branch":
				//fill_companies();
				//fill_acctmgrs();
				//fill_controlguys();
				//fill_provs();
				//fill_countries();
				//fill_gls();
				//fill_taxes();
			break;
			#endregion case "switch_branch":
			#region case "associated_phone":
			case "associated_phone":
				//fill_associated();
				//fill_phones();
				//fill_address();
			break;
			#endregion case "associated_phone":
			}
		 }
	protected void FillCustomerMain(bool force)
		{
		Toolbox.do_debug("Starting FillCustomerMain");
		var customer_id									= Convert.ToInt32(grab("customer_id"));
		if(customer_id > 0)
			{
		Session["customer_id"]							= customer_id;
		ViewState["customer_id"]							= customer_id;
		var _customer								= new NECustomer(customer_id);
		uc_locations.customer_id							= Convert.ToInt32(customer_id);
		uc_locations.this_customer							= _customer;

		partner_skills1.customer_id = Session["customer_id"] != null ? Convert.ToInt32(Session["customer_id"]) : 0;
		partner_skills1.init();

		if(force)
			{
			uc_locations.init();
			}
		uc_accounting.customer_id							= Convert.ToInt32(customer_id);
		uc_accounting.this_customer							= _customer;
		if(force)
			{
			uc_accounting.DataBind();
			}
		uc_header.customer_id								= Convert.ToInt32(customer_id);
		uc_header.this_customer								= _customer;
		uc_header.DataBind();
		var adr_id = ddl_address.Value;
	//	reset_params();
		if(!IsCallback && !IsPostBack || (ViewState["address_id"] == null))
			{
			ddl_address.DataBind();
			gv_workorders.DataBind();
			gv_quotes.DataBind();
			}
		var _company									= new NeBusinessUnit(_customer.business_unit_id);
		lb_customer_number.Text								= string.IsNullOrEmpty(_customer.Customer_Number) ? "N/A" : _customer.Customer_Number;
		lb_customer_id.Text									= _customer.Customer_ID.ToString();
		lb_added_by.Text									= Toolbox.doSQL_string(string.Format(@"SELECT IFNULL(MAX(member_fullname), 'Not Set') FROM member WHERE member_id = '{0}' LIMIT 1", _customer.InitMember_ID));
		lb_date_added.Text									= _customer.CreatedDateTime.ToString("MM/dd/yyyy");
		var qc_dt1										= Toolbox.MySQL_shortdt(_customer.QC_DateTime);
		var qc_dt2										= Toolbox.MySQL_shortdt(_customer.QC2_DateTime);
		lb_qc_date.Text										= lb_customer_number.Text == "N/A" || qc_dt1  == "0001-01-01" || qc_dt1 == "" ? "N/A" : qc_dt1;
		lb_qc2_date.Text									= lb_customer_number.Text == "N/A" || qc_dt2 == "0001-01-01"  || qc_dt2 == ""  ? "N/A" : qc_dt2;

		bt_qcapprove.ClientVisible							= (lb_qc_date.Text == "N/A" && myMember.AuthenticatedForPage("21"));
		bt_qc2approve.ClientVisible							= (lb_qc2_date.Text == "N/A" && myMember.AuthenticatedForPage("21"));
		hl_qc1.ClientVisible								= !bt_qcapprove.ClientVisible && myMember.AuthenticatedForPage("21");
		hl_qc2.ClientVisible								= !bt_qc2approve.ClientVisible && myMember.AuthenticatedForPage("21");

			    ds_workorders.SelectCommand = @"SELECT 

			    a.woprog_id, 
			    a.WOProg_CutDateTime cut_dt, 
			    c.contact_name, 
			    a.WOProg_CloseDateTime close_dt, 
			    a.woprog_bvwo wo_number,
			        a.woprog_description wo_description,
			        a.woprog_status status,
			        IF(a.WOProg_QuoteID = 0 OR a.woprog_quoteid < 100000, NULL, SUBSTRING(a.WOProg_QuoteID, 1, 6)) quote_id, 
			    IF(a.WOProg_QuoteID = 0 OR a.woprog_quoteid < 100000, NULL, SUBSTRING(a.WOprog_quoteid, 7, 3)) rev,
			    a.business_unit_id,
			    a.woprog_bvwo, 
			    CONCAT(b.Address_Addr1, ',', b.Address_City) addy
			        FROM

			    woprog a
			    left join

			    address b on b.address_id = a.woprog_address_id
			    LEFT JOIN

			    contact c on a.woprog_contact_id = c.contact_id
			    WHERE
a.business_unit_id in (" + new Current_User().visible_business_units + @") and 
			    a.woprog_customer_id = @customer_id AND

			    a.woprog_bvwo != '' and
			        ((@address_id != '' and @address_id != 0 and @address_id is not null and a.woprog_address_id = @address_id)

			    or(@address_id = 0)or(@address_id = '')or(@address_id is null) )
			    ORDER BY a.woprog_ID DESC";


        if (force)
			{
			Session["customers_gv_parts"] = null;
			Session["cust_ar_emails"] = null;
			Session["cust_emails"] = null;
			}

			#region check for qc2 prereqs
			var disabled_reasons						= new List<string>();
			// auto statement email address
			if(string.IsNullOrEmpty(_customer.customer_autostatement_address))
				{
				disabled_reasons.Add("Customer's auto-statement email address is empty");
				}
			// auto invoice email address
			if(string.IsNullOrEmpty(_customer.customer_invoice_address))
				{
				disabled_reasons.Add("Customer's invoice email address is empty");
				}
			// Credit limit other than 0
			if(_customer.CreditLimit == 0)
				{
				disabled_reasons.Add("Customer's credit limit is zero");
				}
			// Credit days other than 0
			if(_customer.customer_creditdays == 0)
				{
				disabled_reasons.Add("Customer's credit days is zero");
				}
			if(_customer.Contacts.Count == 0)
				{
				disabled_reasons.Add("Customer must have at least one contact specified");
				}
			if(string.IsNullOrEmpty(_customer.Address.Addr1))
				{
				disabled_reasons.Add("Customer must have the billing address filled out");
				}
			if(_customer.confirmed_po == 0)
				{
				disabled_reasons.Add("Whether the customer or not requires a PO has not yet been confirmed.");
				}
			if(_customer.confirmed_taxexempt == 0)
				{
				disabled_reasons.Add("Whether the customer is tax exempt or not has not yet been confirmed.");
				}
			if(disabled_reasons.Count > 0)
				{
				bt_qc2approve.ClientEnabled						= false;
				bt_qc2approve.CssClass							= "ttip";
				var bt_qc2approve_tooltip					= "<div>These issues need to be fixed before this customer can be level 2 qc'ed</div><ul>";
				for(var i = 0; i < disabled_reasons.Count; i++)
					{
					bt_qc2approve_tooltip						+= "<li>"+disabled_reasons[i]+"</li>";
					}
				bt_qc2approve_tooltip							+= "</ul>";
				bt_qc2approve.Attributes.Add("data-title", "Problems found");
				bt_qc2approve.Attributes.Add("data-tooltip", bt_qc2approve_tooltip);
				}
			else
				{
				bt_qc2approve.ClientEnabled						= true;
				bt_qc2approve.CssClass							= "";
				}
			#endregion check for qc2 prereqs
		/*
		if (!exists("detail_gen_company"))
			{
			put("detail_gen_company", _customer.business_unit_id);
			}
		if(!exists("detail_gen_acctmgr"))
			{
			put("detail_gen_acctmgr", _customer.Account_Manager);
			}
		if(!exists("detail_gen_controlsguy"))
			{
			put("detail_gen_controlsguy", _customer.ControlsGuy_MemberID);
			}
		 */
		//detail_customer_name.Text							= exists("detail_customer_name") ? grab("detail_customer_name") : _customer.Customer_Name;
		//address_phone_area.Text								= exists("address_phone_area") ? grab("address_phone_area") : _customer.Address.PhoneArea;
		//address_phone_prefix.Text							= exists("address_phone_prefix") ? grab("address_phone_prefix") : _customer.Address.Phonefirst;
		//address_phone_suffix.Text							= exists("address_phone_suffix") ? grab("address_phone_suffix") : _customer.Address.PhoneLast;
		//address_phone_ext.Text								= exists("address_phone_ext") ? grab("address_phone_ext") : _customer.Address.PhoneExt;
		//address_fax_area.Text								= exists("address_fax_area") ? grab("address_fax_area") : _customer.Address.FaxArea;
		//address_fax_prefix.Text								= exists("address_fax_prefix") ? grab("address_fax_prefix") : _customer.Address.FaxFirst;
		//address_fax_suffix.Text								= exists("address_fax_suffix") ? grab("address_fax_suffix") : _customer.Address.FaxLast;
		//detail_gen_website.Text								= exists("detail_gen_website") ? grab("detail_gen_website") : _customer.Address.Web;
		//detail_gen_gps.Text									= exists("detail_gen_gps") ? grab("detail_gen_gps") : _customer.Address.GPS_Coords;
		#region 115 lockdown - Customer Administration
		// Only use client enabled
		var AdminAuthenticated					= myMember.AuthenticatedForPrivilege(115);
		btn_new.Visible								= _is_new_allowed;
		btn_postalsearch.Visible					= _is_new_allowed;
		//detail_customer_name.ClientEnabled		= AdminAuthenticated;
		//detail_gen_addr1.ClientEnabled			= AdminAuthenticated;
		//detail_gen_addr2.ClientEnabled			= AdminAuthenticated;
		//detail_gen_addr3.ClientEnabled			= AdminAuthenticated;
		//detail_gen_addr4.ClientEnabled			= AdminAuthenticated;
		//detail_gen_city.ClientEnabled			= AdminAuthenticated;
		//detail_gen_country.ClientEnabled		= AdminAuthenticated;
		//detail_gen_postal.ClientEnabled			= AdminAuthenticated;
		//detail_gen_provstate.ClientEnabled		= AdminAuthenticated;
		//address_phone_area.ClientEnabled		= AdminAuthenticated;
		//address_phone_prefix.ClientEnabled		= AdminAuthenticated;
		//address_phone_suffix.ClientEnabled		= AdminAuthenticated;
		//address_phone_ext.ClientEnabled			= AdminAuthenticated;
		//acct_tax1.ClientEnabled					= AdminAuthenticated;
		//acct_tax2.ClientEnabled					= AdminAuthenticated;
		//acct_tax3.ClientEnabled					= AdminAuthenticated;
		//acct_tax4.ClientEnabled					= AdminAuthenticated;
		//acct_taxex1.ClientEnabled				= AdminAuthenticated;
		//acct_taxex2.ClientEnabled				= AdminAuthenticated;
		//acct_taxex3.ClientEnabled				= AdminAuthenticated;
		//acct_taxex4.ClientEnabled				= AdminAuthenticated;
		//on_hold.ClientEnabled					= AdminAuthenticated;
		#endregion 115 lockdown - Customer Administration
		right_tabs.TabPages[1].ClientEnabled	= myMember.AuthenticatedForPrivilege(118);
		//on_hold.Checked										= exists("on_hold") && grab("on_hold") != "" ? (grab("on_hold") == "True") : (_customer.Hold == "T");
		//memwhyhold.Text                                     = exists("memwhyhold") ? grab("memwhyhold") : _customer.customer_whyhold;
		//memwhyhold.ToolTip                                  = new NeMember(Convert.ToInt32(_customer.customer_whohold)).FullName2;
		//lbl_whohold.Text                                    = new NeMember(Convert.ToInt32(_customer.customer_whohold)).FullName2;
		#region Populate Address Info
		//main_persistence_handler["active_address_id"]		= exists("active_address_id") ? grab("active_address_id") : _customer.Address.ID.ToString();
		//detail_gen_city.Text								= exists("detail_gen_city") ? grab("detail_gen_city") : _customer.Address.City;
		//detail_gen_postal.Text								= exists("detail_gen_postal") ? grab("detail_gen_postal") : _customer.Address.Postal;
		//detail_gen_addr1.Text								= exists("detail_gen_addr1") ? grab("detail_gen_addr1") : _customer.Address.Addr1;
		//detail_gen_addr1.Text								= detail_gen_addr1.Text.Replace("&amp;", "&").Replace("&quot;", "\"");
		//detail_gen_addr2.Text								= exists("detail_gen_addr2") ? grab("detail_gen_addr2") : _customer.Address.Addr2;
		//detail_gen_addr3.Text								= exists("detail_gen_addr3") ? grab("detail_gen_addr3") : _customer.Address.Addr3;
		//detail_gen_addr4.Text								= exists("detail_gen_addr4") ? grab("detail_gen_addr4") : _customer.Address.Addr4;
		//main_persistence_handler["detail_gen_provstate"]	= exists("detail_gen_provstate") ? grab("detail_gen_provstate") : _customer.Address.Prov;
		//main_persistence_handler["detail_gen_country"]		= exists("detail_gen_country") ? grab("detail_gen_country") : _customer.Address.Country;
		//main_persistence_handler["ts"]						= exists("ts") ? grab("ts") : _customer.ts.Ticks.ToString();
		#endregion Populate Address Info
		#region Populate Accounting Tab
		//acct_credit_limit.Text								= exists("acct_credit_limit") ? grab("acct_credit_limit") : _customer.CreditLimit.ToString();
		//acct_finance_charges.Checked						= exists("acct_finance_charges") && grab("acct_finance_charges") != "" ? (grab("acct_finance_charges") == "True") : (_customer.ApplyFinanceCharges == "T");
		//acct_prompt_tax.Checked								= exists("acct_prompt_tax") && grab("acct_prompt_tax") != "" ? (grab("acct_prompt_tax") == "True") : (_customer.TaxPrompt == "T");
		//acct_sell_level.SelectedItem						= _customer.Address.SellPrice != "" ? acct_sell_level.Items.FindByValue(Convert.ToInt32(_customer.Address.SellPrice).ToString()) : null;
		//acct_discount_code_text.Text						= exists("acct_discount_code_text") ? grab("acct_discount_code_text") : _customer.Discount.ToString();
		//acct_discount_code_ddl.SelectedItem					= exists("acct_discount_code_ddl") ? acct_discount_code_ddl.Items.FindByValue(grab("acct_discount_code_ddl")) : acct_discount_code_ddl.Items.FindByValue(_customer.PriceCode);
		//acct_credit_type.SelectedItem						= exists("acct_credit_type") ? acct_credit_type.Items.FindByValue(grab("acct_credit_type")) : acct_credit_type.Items.FindByValue(_customer.Credit_Type.ToString());
		//acct_statements.SelectedItem						= exists("acct_statements") ? acct_statements.Items.FindByValue(grab("acct_statements")) : acct_statements.Items.FindByValue(_customer.StatementType);
		//acct_invoices.SelectedItem							= exists("acct_invoices") ? acct_invoices.Items.FindByValue(grab("acct_invoices")) : acct_invoices.Items.FindByValue(_customer.InvoiceType);
		//acct_default_invoicetype.SelectedItem				= exists("acct_default_invoicetype") ? acct_default_invoicetype.Items.FindByValue(Convert.ToInt32(grab("acct_default_invoicetype"))) : acct_default_invoicetype.Items.FindByValue(_customer.default_invoicetype);
	
		
		//bool show_grossmargin_info							= myMember.AuthenticatedForPrivilege(12, 81) && !myMember.isContact;
		//lb_overallmargin.Text								= !show_grossmargin_info ? "--" : Toolbox.doSQL_double(@"SELECT customer_margin(@v0 )", new object[] {  customer_id) } ).ToString("P2");
		//main_persistence_handler["acct_gl_account"]			= exists("acct_gl_account") ? grab("acct_gl_account") : _customer.GL_ID;
		//txtInvoiceCCEmail.Text								= exists("txtInvoiceCCEmail") ? grab("txtInvoiceCCEmail") : _customer.customer_invoice_ccaddress;
		//txtInvoiceEmail.Text								= exists("txtInvoiceEmail") ? grab("txtInvoiceEmail") : _customer.customer_invoice_address;
		//txtStatementCCEmail.Text							= exists("txtStatementCCEmail") ? grab("txtStatementCCEmail") : _customer.customer_autostatement_ccaddress;
		//txtStatementEmail.Text								= exists("txtStatementEmail") ? grab("txtStatementEmail") : _customer.customer_autostatement_address;
		//acct_auto_statement.Value							= exists("acct_auto_statement") ? Convert.ToInt32(grab("acct_auto_statement")) : Convert.ToInt32(_customer.customer_autostatements);
		//dayscredit.Value									= exists("acct_dayscredit") ? Convert.ToInt32(grab("acct_dayscredit")) : Convert.ToInt32(_customer.customer_creditdays);
		//acct_auto_invoicing.Checked							=  exists("acct_auto_invoicing") && grab("acct_auto_invoicing") != "" ? (grab("acct_auto_invoicing").ToString() == "1") : (_customer.customer_auto_invoice == 1);
		//if(!exists("acct_tax1"))
		//	{
		//	put("acct_tax1", _customer.Address.Tax1);
		//	}
		//if(!exists("acct_tax2"))
		//	{
		//	put("acct_tax2", _customer.Address.Tax2);
		//	}
		//if(!exists("acct_tax3"))
		//	{
		//	put("acct_tax3", _customer.Address.Tax3);
		//	}
		//if(!exists("acct_tax4"))
		//	{
		//	put("acct_tax4", _customer.Address.Tax4);
		//	}
		//if(!exists("acct_taxex1"))
		//	{
		//	put("acct_taxex1", _customer.Address.Tax1Exempt);
		//	}
		//if(!exists("acct_taxex2"))
		//	{
		//	put("acct_taxex2", _customer.Address.Tax2Exempt);
		//	}
		//if(!exists("acct_taxex3"))
		//	{
		//	put("acct_taxex3", _customer.Address.Tax3Exempt);
		//	}
		//if(!exists("acct_taxex4"))
		//	{
		//	put("acct_taxex4", _customer.Address.Tax4Exempt);
		//	}
		#endregion Populate Accounting Tab
		//fill_companies();
		//fill_acctmgrs();
		//fill_controlguys();
		//fill_contacts();
		//fill_gls();
		//fill_countries();
		//fill_provs();
		//fill_taxes();
		//fill_associated();
		//fill_phones();
		//fill_bv_contacts();
		//fill_popup();
		//fill_parts();
		
		//fill_emails();
		//frame_notes.Attributes.Add("src","customer_notes.aspx?customer_id=" + _customer.Customer_ID.ToString());
		//files_frame.Attributes.Add("src", "/filemanager.aspx?parent_page=customer&id=" + _customer.Customer_ID.ToString());
		//frame_sales.Attributes.Add("src", "customer_sales.aspx?customer_id=" + _customer.Customer_ID.ToString());
		//frame_assets.Attributes.Add("src", "/sections/reports/customer_assets/index.aspx?customer_id=" + _customer.Customer_ID.ToString());
		//files_frame.DataBind();
		//frame_notes.DataBind();
		//frame_sales.DataBind();
		//frame_assets.DataBind();

		//frame_notes.Attributes.Add("onload", "resizeIframe(this);");
		//files_frame.Attributes.Add("onload", "resizeIframe(this);");
		//frame_sales.Attributes.Add("onload", "resizeIframe(this);");
		//frame_assets.Attributes.Add("onload", "resizeIframe(this);");
		var asdf = ds_workorders.SelectParameters[0];
		var fdsa = ds_workorders.SelectParameters[1];
			}
		Toolbox.do_debug("Ending FillCustomerMain");
		}
	protected bool chk(string _key)
		{
		if(exists(_key) && main_persistence_handler[_key] != null && main_persistence_handler[_key].ToString() != "" )
			{
			return true;
			}
		else
			{
			return false;
			}
		}
	protected bool exists(string _key)
		{
		return main_persistence_handler.Contains(_key);
		}
	protected string grab(string _key)
		{
		var _returned		= "";
		if(!exists(_key))
			{
			return "";
			}
		if(main_persistence_handler.Contains(_key))
			{
			var _r				= main_persistence_handler[_key];
			if(_r != null)
				{
				_returned			= _r.ToString();
				}
			}
		return _returned;
		}
	protected void put(string _key, object _value)
		{
		var _val			= _value != null ? _value.ToString() : "";
		if(chk(_key) || main_persistence_handler.Contains(_key))
			{
			main_persistence_handler[_key]		= _val;
			}
		else
			{
			main_persistence_handler.Add(_key, _val);
			}
		}
	protected bool CheckValidity()
		{

		var validity									= true;
		//detail_gen_company.Style["background-color"]	= grab("detail_gen_company") != "" ? good : bad;
		//if(detail_gen_company.Style["background-color"]	== bad){validity = false;}
				
		//detail_gen_addr1.Style["background-color"]		= grab("detail_gen_addr1") != "" ? good : bad;
		//if(detail_gen_addr1.Style["background-color"]	== bad){validity = false;}
		
		//detail_gen_city.Style["background-color"]		= grab("detail_gen_city") != "" ? good : bad;
		//if(detail_gen_city.Style["background-color"]	== bad){validity = false;}
		
		//detail_gen_provstate.Style["background-color"]	= grab("detail_gen_provstate") != "" ? good : bad;
		//if(detail_gen_provstate.Style["background-color"]	== bad){validity = false;}
		
		//detail_gen_postal.Style["background-color"]		= grab("detail_gen_postal") != "" ? good : bad;
		//if(detail_gen_postal.Style["background-color"]	== bad){validity = false;}
		
		//detail_gen_country.Style["background-color"]	= grab("detail_gen_country") != "" ? good : bad;
		//if(detail_gen_country.Style["background-color"]	== bad){validity = false;}
		
		//address_phone_area.Style["background-color"]		= grab("address_phone_area") != "" ? good : bad;
		//if(address_phone_area.Style["background-color"]	== bad){validity = false;}
		
		//address_phone_prefix.Style["background-color"]		= grab("address_phone_prefix") != "" ? good : bad;
		//if(address_phone_prefix.Style["background-color"]	== bad){validity = false;}
		
		//address_phone_suffix.Style["background-color"]		= grab("address_phone_suffix") != "" ? good : bad;
		//if(address_phone_suffix.Style["background-color"]	== bad){validity = false;}
		
		//acct_gl_account.Style["background-color"]		= grab("acct_gl_account") != "" ? good : bad;
		//if(acct_gl_account.Style["background-color"]	== bad){validity = false;}

		//detail_gen_acctmgr.Style["background-color"]	= grab("detail_gen_acctmgr") != "" ? good : bad;
		//if(detail_gen_acctmgr.Style["background-color"]	== bad){validity = false;}

		return validity;
		}
	protected void ClearCustomerMain()
		{
		main_persistence_handler.Clear();
		//put("detail_gen_company", myMember.business_unit_id);
		//detail_customer_name.Text				= "";
		//on_hold.Checked							= false;
		lb_added_by.Text = lb_customer_id.Text = lb_customer_number.Text = lb_date_added.Text = lb_qc_date.Text = "";
		//detail_gen_company.SelectedIndex		= -1;
		//detail_gen_acctmgr.SelectedIndex		= -1;
		//detail_gen_region.SelectedIndex			= -1;
	
		//detail_gen_addr1.Text					= "";
		//detail_gen_addr2.Text					= "";
		//detail_gen_addr3.Text					= "";
		//detail_gen_addr4.Text					= "";
		//detail_gen_city.Text					= "";
		//detail_gen_provstate.SelectedIndex		= 0;
		//detail_gen_postal.Text					= "";
		//detail_gen_country.SelectedIndex		= 0;
		//address_phone_area.Text					= address_phone_prefix.Text = address_phone_suffix.Text = address_phone_ext.Text = "";
		//address_fax_area.Text					= address_fax_prefix.Text = address_fax_suffix.Text = "";
		
		//detail_gen_website.Text					= "";
		//detail_gen_gps.Text						= "";
		//acct_gl_account.SelectedIndex			= -1;
		//acct_sell_level.SelectedIndex			= -1;
		//acct_discount_code_ddl.SelectedIndex	= -1;
		//acct_discount_code_text.Text			= "0";
		//acct_credit_limit.Text					= "";
		//acct_prompt_tax.Checked					= false;
		//acct_credit_type.SelectedIndex			= -1;
		//acct_statements.SelectedIndex			= 1;
		//acct_invoices.SelectedIndex				= 1;
		//acct_default_invoicetype.SelectedIndex	= 0;
		//acct_finance_charges.Checked			= false;
		//acct_auto_invoicing.Checked = false;
		
		

		
		//phone_list.Items.Clear();
		//phone_number.Text						= "";
		//phone_type.SelectedIndex				= 0;
		//acct_auto_statement.Value = 1;
		//txtInvoiceCCEmail.Text = "";
		//txtInvoiceEmail.Text = "";
		//txtStatementCCEmail.Text = "";
		//txtStatementEmail.Text = "";
		//dayscredit.Value = 30;	
		
		var tool_tip							= "This tab will be available once the customer is created";
		right_tabs.TabPages[2].Enabled			= false;
		right_tabs.TabPages[2].ToolTip			= tool_tip;
		right_tabs.TabPages[3].Enabled			= false;
		right_tabs.TabPages[3].ToolTip			= tool_tip;
		right_tabs.TabPages[4].Enabled			= false;
		right_tabs.TabPages[4].ToolTip			= tool_tip;
		//detail_customer_name.Focus();
		}
	protected void fill_emails()
	{
		
		/*

		if(ViewState["customer_id"] != null && ViewState["customer_id"].ToString() != "2361")
			{
		ds_emails.SelectCommand = null;
		if (ViewState["customer_id"] != null)
		{
			int isinternal = _tools.getSQL_int(@"Select count(internal_companyNo_id) from internal_companyno  where internal_companyno_intranet_custid =@v0", new object[] { ViewState["customer_id"].ToString() });

			if (ViewState["customer_id"] != null && ViewState["customer_id"].ToString() != "0" && ViewState["customer_id"].ToString() != "177" && isinternal == 0)
			{
				string sql = "SELECT contact_email FROM contact WHERE contact_type = 'customer' AND contact_cust_id = " + ViewState["customer_id"] + " AND contact_status = 'active' AND contact_email NOT IN ('', 'needed', '@') AND contact_email LIKE '%@%.%'";
				DataTable _emails = _tools.get_datatable(sql);
				if (_emails.Rows.Count > 0)
					{
					List<string> emails		= new List<string>();
					List<string>  appendage	= new List<string>();
					foreach (DataRow _address in _emails.Rows)
						{
						string email		= "'\""+_address["contact_email"]+"\"'";
						if(!emails.Contains(email))
							{
							emails.Add(email);
							}
						}
					if(emails.Count == 0)
						{
						emails.Add("'_#_#_#_#_'");
						}
					foreach(string e in emails)
						{
						//appendage.Add("emaillog_to LIKE '%"+e.Replace("'","").Replace("\"", "")+"%' OR emaillog_from LIKE '%"+e.Replace("'","").Replace("\"", "")+"%'");
						}
					sql = @"
SELECT 
	emaillog_timestamp date,
	emaillog_from from_address,
	emaillog_to to_address,
	emaillog_subject subject,
	emaillog_id
FROM 
	emaillog 
WHERE 
	MATCH(emaillog_to, emaillog_from) AGAINST (" + string.Join(" ", emails.ToArray()) + @" IN BOOLEAN MODE)
ORDER BY Date DESC";

					ds_emails.SelectCommand = sql;
				}
				}
				
			}
		}
		 */
		
	}
	protected void fill_address()
		{
		var _address					= new NEAddress(Convert.ToInt32(grab("active_address_id")));
		var _e								= (_address.Type != "S");

		right_tabs.TabPages[3].Enabled		= _e;
		right_tabs.TabPages[4].Enabled		= _e;
	
		#region 115 lockdown - Customer Administration
		// Only use client enabled
		var AdminAuthenticated					= myMember.AuthenticatedForPrivilege(115);
	
		#endregion 115 lockdown - Customer Administration

	
		}

	protected void fill_parts()
	{
		var cid = Convert.ToInt32(grab("customer_id"));
		if (Session["customers_gv_parts"] == null)
		{
			Session["customers_gv_parts"] = _tools.getSQL_datatable(@"SELECT wo_detail_history.wo_detail_history_master_id masterid, 
URLDECODE(wo_detail_history.wo_detail_history_description) description, Sum(wo_detail_history.wo_detail_history_qty_committed) qty,
Avg(wo_detail_history.wo_detail_history_price_sell) avg_sell FROM wo_detail_history INNER JOIN woprog
ON wo_detail_history.wo_detail_history_woprog_id = woprog.WOProg_ID  where wo_detail_history_master_id<900000
and wo_detail_history_master_id>0 and wo_detail_history_master_id != 2139 and woprog_customer_id =@v0 
GROUP BY wo_detail_history.wo_detail_history_master_id,wo_detail_history.wo_detail_history_description", new object[] { cid});
		}
		gv_parts.DataSource = Session["customers_gv_parts"];
		gv_parts.DataBind();
	}
	//protected void fill_companies()
	//	{
		//string _cid							= chk("detail_gen_company") ? grab("detail_gen_company") : myMember.business_unit_id.ToString();
		//detail_gen_company.DataSource		= new NeBusinessUnit().LoadCompanysWithBV();
		//detail_gen_company.DataBind();
		//detail_gen_company.SelectedItem		= detail_gen_company.Items.FindByValue(_cid);
	//	}
	/*
	protected void fill_bv()
		{
		NECustomer _customer					= new NECustomer(Convert.ToInt32(grab("customer_id")));
		DataTable _test							= new NeBusinessUnit().LoadCompanysWithBV();
		DataTable _company_data					= new DataTable();
		_company_data.Columns.Add(new DataColumn("bv_name", Type.GetType("System.String")));
		_company_data.Columns.Add(new DataColumn("bv_exists", Type.GetType("System.Boolean")));
		_company_data.Columns.Add(new DataColumn("bv_dsn", Type.GetType("System.String")));
		_company_data.Columns.Add(new DataColumn("business_unit_id", Type.GetType("System.String")));
		_company_data.Columns.Add(new DataColumn("bv_addr_exists", Type.GetType("System.String")));
		foreach(DataRow _dr in _test.Rows)
			{
			DataRow _row		= _company_data.NewRow();
			_row[0]				= _dr["name"].ToString();
			string dsn			= _dr["company_dsnbv7"].ToString();
			bool _exists		= Convert.ToBoolean(_tools.getSQL_int(@"SELECT COUNT(*) FROM customer WHERE cus_no = @v0 ",dsn, new object[] {  _customer.Customer_Number)} );
			bool _addr_exists	= Convert.ToBoolean(_tools.getSQL_int(@"SELECT COUNT(*) FROM address WHERE cev_no = @v0 ",dsn, new object[] {  _customer.Customer_Number) } );
			_row[1]				= _exists.ToString();
			_row[2]				= dsn;
			_row[3]				= _dr["business_unit_id"].ToString();
			_row[4]				= _addr_exists.ToString();
			_company_data.Rows.Add(_row);
			}	
		bv_data.DataSource		= _company_data;
		bv_data.DataBind();
		}
	 */
	//protected void fill_acctmgrs()
	//	{
		//string _cid							= chk("detail_gen_company") ? grab("detail_gen_company") : myMember.business_unit_id.ToString();
		//string _mid							= chk("detail_gen_acctmgr") ? grab("detail_gen_acctmgr") : myMember.id.ToString();
		//detail_gen_acctmgr.DataSource		= MemberList(_cid);
		//detail_gen_acctmgr.DataBind();
		//detail_gen_acctmgr.SelectedItem		= detail_gen_acctmgr.Items.FindByValue(_mid);
	//	}
	//protected void fill_controlguys()
		//{
		//string _cid								= chk("detail_gen_company") ? grab("detail_gen_company") : myMember.business_unit_id.ToString();
		//string _mid								= chk("detail_gen_controlsguy") ? grab("detail_gen_controlsguy") : myMember.id.ToString();
		//detail_gen_controlsguy.DataSource		= MemberList(_cid);
		//detail_gen_controlsguy.DataBind();
		//detail_gen_controlsguy.SelectedItem		= detail_gen_controlsguy.Items.FindByValue(_mid);
	//	}
	//protected void fill_taxes()
	//	{
		/*
		DataSet full_taxes					= new NeTax().GetNETaxList();
		#region tax1
		acct_tax1.DataSource				= full_taxes;
		acct_tax1.DataBind();
		acct_tax1.SelectedItem				= acct_tax1.Items.FindByValue(grab("acct_tax1"));
		acct_taxex1.Text					= grab("acct_taxex1");
		//if(ship_tax1.SelectedIndex == -1)
		//	{
		//	ship_tax1.DataSource			= full_taxes;
		//	ship_tax1.DataBind();
		//	}
		#endregion tax1
		#region tax2
		acct_tax2.DataSource				= full_taxes;
		acct_tax2.DataBind();
		acct_tax2.SelectedItem				= acct_tax1.Items.FindByValue(grab("acct_tax2"));
		acct_taxex2.Text					= grab("acct_taxex2");
		//if(ship_tax2.SelectedIndex == -1)
		//	{
		//	ship_tax2.DataSource			= full_taxes;
		//	ship_tax2.DataBind();
		//	}
		#endregion tax2
		#region tax3
		acct_tax3.DataSource				= full_taxes;
		acct_tax3.DataBind();
		acct_tax3.SelectedItem				= acct_tax3.Items.FindByValue(grab("acct_tax3"));
		acct_taxex3.Text					= grab("acct_taxex3");
		//
		//if(ship_tax3.SelectedIndex == -1)
		//	{
		//	ship_tax3.DataSource			= full_taxes;
		//	ship_tax3.DataBind();
		//	}
		// 
		#endregion tax3
		#region tax4
		acct_tax4.DataSource				= full_taxes;
		acct_tax4.DataBind();
		acct_tax4.SelectedItem				= acct_tax4.Items.FindByValue(grab("acct_tax4"));
		acct_taxex4.Text					= grab("acct_taxex4");
		//if(ship_tax4.SelectedIndex == -1)
		//	{
		//	ship_tax4.DataSource			= full_taxes;
		//	ship_tax4.DataBind();
		//	}
		#endregion tax4
		*/
	//	}
	protected void search_ds_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
		{
		e.Command.Parameters["@criteria"].Value = string.Format("{0}", cust_search_t.Value);
		}
	protected void bv_data_CustomCallback(object sender, CallbackEventArgsBase e)
		{
	//	fill_bv();
		}
	protected string BVSynced(object dataItem)
		{
		var row = dataItem as DataRowView;
        if(row != null)
			{
            var exists = (bool)row["bv_exists"];
            return exists ? "background-color:#0f0;color:#000" : "background-color:#f00;color:#fff;";
			}
        return "background-color:#ccc;color:#000";
		}
	protected string phone_check(string pn)
		{
		var _dr			= _tools.getSQL_datatable(@" SELECT COUNT(*) c, b.address_table_id id FROM phone_numbers a LEFT JOIN address b ON a.phone_numbers_table_id = b.address_id WHERE phone_numbers_number = @v0  AND phone_numbers_type = 'address' AND phone_numbers_table_id IN ( SELECT d.address_id FROM customer c LEFT JOIN address d ON d.address_table_id = c.customer_id LEFT JOIN customer_sales_properties e ON c.customer_id = e.customer_id AND d.address_id = e.address_id WHERE c.customer_status != 4 ) LIMIT 1", new object[] {  pn } ).Rows[0];
		var custname		= "";
		if(_dr["id"] != DBNull.Value)
			{
			try
				{
				custname		= _tools.getSQL_string(@"SELECT customer_name FROM customer a WHERE customer_id = @v0  LIMIT 1", new object[] {  _dr["id"] } );
				}
			catch (Exception ee)
				{
				_tools.catch_error(ee);
				}
			}
		return string.Format("{0},{1},{2}", _dr["c"], custname, _dr["id"]);
		}
	protected void add_phone_check_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		var full_phone	= string.Format("{0} {1} {2}", grab("address_phone_area"), grab("address_phone_prefix"), grab("address_phone_suffix"));		
		e.Result			= phone_check(full_phone);
		}
	protected void cb_qc1_callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		_tools.getSQL_void(@"
UPDATE 
	customer 
SET 
	customer_qc_datetime = NULL, 
	customer_qc_member_id = NULL 
WHERE 
	customer_id = @v0 
LIMIT 1", new object[] { grab("customer_id")});

		}
	protected void cb_qc2_callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		_tools.getSQL_void(@"
UPDATE 
	customer 
SET 
	customer_qc2_datetime = NULL, 
	customer_qc2_member_id = NULL
WHERE 
	customer_id = @v0 
LIMIT 1", new object[] { grab("customer_id")});
		}
	protected void cb_qc_customer_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		switch(e.Parameter)
			{
			case "1":
				_tools.getSQL_void(@"
UPDATE 
	customer 
SET 
	customer_qc_datetime = NOW(), 
	customer_qc_member_id = @v0 
WHERE 
	customer_id = @v1 
LIMIT 1", new object[] { myMember.id, grab("customer_id")});
				e.Result			= "1|"+_tools.getSQL_string(@" SELECT DATE_FORMAT(customer_qc_datetime, '%Y-%m-%d') FROM customer WHERE customer_id = @v0  LIMIT 1", new object[] {  grab("customer_id") } );
			break;
			case "2":
				_tools.getSQL_void(@"
UPDATE 
	customer 
SET 
	customer_qc2_datetime = NOW(), 
	customer_qc2_member_id = @v0 
WHERE 
	customer_id = @v1 
LIMIT 1", new object[] { myMember.id, grab("customer_id")});
				e.Result			= "2|"+_tools.getSQL_string(@" SELECT DATE_FORMAT(customer_qc2_datetime, '%Y-%m-%d') FROM customer WHERE customer_id = @v0  LIMIT 1", new object[] {  grab("customer_id") } );
			break;
			}
		}
	protected void detail_gen_acctmgr_Callback(object sender, CallbackEventArgsBase e)
		{
		//fill_acctmgrs();
		}
	protected void hl_quote_Init(object sender, EventArgs e)
		{
		var container		= ((ASPxHyperLink)sender).NamingContainer as GridViewDataItemTemplateContainer;
		var gv_r									= gv_workorders.GetDataRow(container.VisibleIndex);
		if(gv_r != null)
			{
			var this_quote_id							= gv_r["quote_id"].ToString();
			((ASPxHyperLink)sender).NavigateUrl				= string.IsNullOrEmpty(this_quote_id) ? "" : "javascript:void(0);";
			var this_rev									= gv_r["rev"].ToString();
			((ASPxHyperLink)sender).ClientSideEvents.Click	= string.IsNullOrEmpty(this_quote_id) ? "" : string.Format("function(s,e){{boing('/#/opens/65/quotes/{0}/{1}', 'quote', 1035, 800);}}", this_quote_id, this_rev);
			}
		}
	protected void hl_wo_Init(object sender, EventArgs e)
		{
		var container		= ((ASPxHyperLink)sender).NamingContainer as GridViewDataItemTemplateContainer;
		var gv_r									= gv_workorders.GetDataRow(container.VisibleIndex);
		if(gv_r != null)
			{
			var this_bvwo								= gv_r["woprog_bvwo"].ToString();
			var this_woprog_id							= gv_r["woprog_id"].ToString();
			((ASPxHyperLink)sender).Text					= this_woprog_id == "" ? string.Empty : this_bvwo;
			var this_business_unit_id							= gv_r["business_unit_id"].ToString();
			((ASPxHyperLink)sender).ClientSideEvents.Click	= string.Format("function(s,e){{boing('/sections/workorder/index.aspx?woprog_id={0}&business_unit_id={1}&fromwo=', 'quote', 1035, 800);}}", this_woprog_id, this_business_unit_id);
			}
		}
	protected void search_results_gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if ((e.DataColumn.FieldName == "address_")||(e.DataColumn.FieldName=="customer_name"))
		{
		//	e.Cell.Text = _tools.value_from(e.CellValue);
		}

	}
	protected void search_results_gv_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex > -1)
		{
			var ishold = search_results_gv.GetRowValues(e.VisibleIndex, "hold").ToString();
			var added = Convert.ToDateTime(search_results_gv.GetRowValues(e.VisibleIndex, "dateadded"));
			var qc1 = (search_results_gv.GetRowValues(e.VisibleIndex, "qc1"));
			var qc2 = (search_results_gv.GetRowValues(e.VisibleIndex, "qc2"));
			if (ishold == "T")
			{
				e.Row.BackColor = System.Drawing.Color.Red;
				e.Row.ForeColor = System.Drawing.Color.White;
				e.Row.Font.Bold = true;
			}
			if (added > System.DateTime.Today.AddDays(-30))
			{
				e.Row.BackColor = System.Drawing.Color.Blue;
				e.Row.ForeColor = System.Drawing.Color.White;
				e.Row.ToolTip = "This customer was added within the last month!";
			}
			if (qc1 == DBNull.Value)
			{
				e.Row.BackColor = System.Drawing.Color.DarkGreen;
				e.Row.ForeColor = System.Drawing.Color.White;
				e.Row.ToolTip = "This customer hasn't been QC1'd yet, just an FYI";
			}
		}
	}


	protected void ASPxGridView2_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.DataColumn.FieldName == "description")
		{
			e.Cell.Text = _tools.value_from(e.CellValue);
		}
	}
	protected void bt_parts_excel_Click(object sender, EventArgs e)
		{
		var customer_id					= Convert.ToInt32(grab("customer_id"));
		var cust						= new NECustomer(customer_id);
		ex_gv_parts.FileName				= cust.Customer_Number+"_partssold";
		ex_gv_parts.WriteXlsxToResponse();
		}
	[WebMethod]
	public static string chk_territory(string postal, string country)
		{
		postal						= postal.ToUpper();
		var cdn_postal_space		= new Regex(@"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ (\d[ABCEGHJKLMNPRSTVWXYZ]\d)$");
		var cdn_postal_nospace	= new Regex(@"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$");
		var usa_postal			= new Regex(@"^\d{5}(?:[-\s]\d{4})?$");
		var business_unit_id				= 0; 
		var c						= 0;
		var space					= false;
		var nospace				= false;
		switch(country)
			{
			case "CAN":
			case "CDN":
				space				= cdn_postal_space.Match(postal).Success;
				nospace				= cdn_postal_nospace.Match(postal).Success;
				if(space || nospace)
					{
					var part	= space ? postal.Split(' ')[0] : postal.Substring(0,3);
					postal		= nospace ? postal.Substring(0,3)+" "+postal.Substring(3,3) : postal;
					c			= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM territory_mapping WHERE postal_definer = @v0", part);
					business_unit_id	= c != 1 ? 1 :Toolbox.doSQL_int(@"SELECT business_unit_id FROM territory_mapping WHERE postal_definer = @v0", part);
					}
			break;
			case "USA":
				if(usa_postal.Match(postal).Success)
					{
					}
			break;
			}
		return @"{""postal"":"""+postal+@""",""business_unit_id"":"""+business_unit_id+@"""}";
		}
	[WebMethod]
	public static void do_apply_template(int template_id, int editing_id)
		{
		var _tools				= new Toolbox();
		var current_user		= Toolbox.do_handle_authentication(10);
		var user_id					= editing_id;
		var contact			= new NEContact(user_id);
		var customer			= new NECustomer((int) contact.Contact_Cust_ID);
		var user				= new NeMember(contact.nesi_member_id);
		// This is in we haven't set up login privileges yet
		user.id						= (int) user_id;
		user.business_unit_id				= customer.business_unit_id;
		//
		var dt				= Toolbox.doSQL_dt(@"SELECT * FROM pageprivilege_template_item WHERE template_id = @v0  ORDER BY type", new object[] {  template_id } );
		NEUserPrivilege.clear(current_user, user, 2);
		NEUserPage.clear(current_user, user, 2);
		foreach(DataRow dr in dt.Rows)
			{
			var id					= Convert.ToInt32(dr["table_id"]);
			var type				= dr["type"].ToString();
			if(type == "Page")
				{
				var up		= new NEUserPage();
				up.admin			= current_user;
				up.user				= user;
				up.user_id			= (int) user_id;
				up.page_id			= id;
				up.type_id			= 2;
				up.save();
				}
			else if(type == "Privilege")
				{
				var p		= new NePrivilege(id);
				var up	= new NEUserPrivilege();
				up.admin			= current_user;
				up.user				= user;
				up.user_id			= user_id;
				up.privilege_id		= id;
				up.type_id			= 2;
				var upage	= new NEUserPage((int) user_id, p.page_id, 2);
				up.typepage_id		= Convert.ToInt32(upage.id);
				up.save();
				}
			}
		}
	protected void ddl_address_SelectedIndexChanged(object sender, EventArgs e)
		{
		ds_workorders.DataBind();
		gv_workorders.DataBind();
		gv_quotes.DataBind();
		}
	protected void right_tabs_ActiveTabChanged(object source, TabControlEventArgs e)
	{
		if (e.Tab.Index == 1)
		{
			uc_accounting.address_id = Convert.ToInt32(ViewState["address_id"]);
			uc_accounting.customer_id = Convert.ToInt32(Session["customer_id"]);
			uc_accounting.init();


		}
		else if(e.Tab.Index == 2) // Grids
			{
			var address_id		= ddl_address.Value;
			if(address_id != null)
				{
				var customer_addresses			= Toolbox.doSQL_string(@"SELECT GROUP_CONCAT(address_id) FROM address WHERE address_table = 'Customer' AND address_table_id = @v0", new object[] {
				Session["customer_id"]});
				if(!Toolbox.Contains(address_id.ToString(), customer_addresses.Split(',')))
					{
					ddl_address.Value			= null;
					}
				}
			}
	}
	protected void cust_page_c_ActiveTabChanged(object source, TabControlEventArgs e)
	{
		if (e.Tab.Index == 1)
		{
			uc_accounting.address_id = Convert.ToInt32(ViewState["address_id"]);
			uc_accounting.customer_id = Convert.ToInt32(Session["customer_id"]);
			uc_accounting.init();
		

		}
	}
}