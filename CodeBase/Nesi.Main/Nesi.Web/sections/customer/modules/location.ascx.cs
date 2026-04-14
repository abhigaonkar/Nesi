using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using nesi.core;

public partial class sections_customer_modules_location : System.Web.UI.UserControl
	{
	NeMember current_user;
	public int business_unit_id	{ get; set; }
	public int address_id  
		{ 
		get {var _address_id = 0; if(ViewState["address_id"] == null){return _address_id;}else{int.TryParse(ViewState["address_id"].ToString(), out _address_id); return _address_id; }} 
		set {ViewState["address_id"] = value.ToString();}
		}
	public int customer_id  
		{ 
		get {var _customer_id = 0; if(ViewState["customer_id"] == null){return _customer_id;}else{int.TryParse(ViewState["customer_id"].ToString(), out _customer_id); return _customer_id; }} 
		set {ViewState["customer_id"] = value.ToString();}
		}
	public NECustomer this_customer  { get; set; }
	public NEAddress this_address  { get; set; }
    protected bool is_initialized { get; set; }
	public void init()
		{
		var AdminAuthenticated									= current_user.AuthenticatedForPrivilege(115);
		var is_billing					= false;
		if (customer_id!=0 && this_customer == null)
		{
			this_customer = new NECustomer((int)customer_id);
		}
		
		if(!is_initialized && address_id != 0 && customer_id != 0)
			{
			Toolbox.do_debug("Location UC Init Start");
			//NECustomer c				= this_customer == null ? new NECustomer((int) customer_id) : this_customer;
			var a					= this_address == null ? new NEAddress(address_id) : this_address;
			a_desc.Text					= a.Desc;
			a_addr1.Text				= a.Addr1;
			a_addr2.Text				= a.Addr2;
			a_addr3.Text				= a.Addr3;
			a_addr4.Text				= a.Addr4;
			a_city.Text					= a.City;
			set_ddl(a_provstate, a.Prov);
			set_ddl(a_country, a.Country);
			p_area.Text					= a.PhoneArea;
			p_prefix.Text				= a.Phonefirst;
			p_suffix.Text				= a.PhoneLast;
			p_ext.Text					= a.PhoneExt;
			f_area.Text					= a.FaxArea;
			f_prefix.Text				= a.FaxFirst;
			f_suffix.Text				= a.FaxLast;
			a_postal.Text				= a.Postal;
			a_website.Text				= a.Web;
			a_facebook.Text				= a.facebook;
			a_twitter.Text				= a.twitter;
			a_linkedin.Text				= a.linkedin;
			a_gpscoordinates.Text		= a.GPS_Coords;
		
			a_table.Text = a.Table;
			a_lblid.Text = a.id.ToString();
            /**
             * TODO : This needs to change
             */ 
			// cb_gl_account.DataSource = Toolbox.doSQL_dt(@"SELECT b.id,CONCAT(a.account_no,' - ',a.gl_chart_name) TEXT FROM GL_te a,GL_Group_te b  WHERE a.gl_group_id = b.id AND b.number ='120'" , null);
			cb_gl_account.DataSource = Toolbox.doSQL_dt(@"select '' id, '' TEXT  union ( SELECT a.account_no id, CONCAT(a.account_no,' - ',a.gl_chart_name) TEXT FROM GL_te a  WHERE a.account_no = '41001' limit 1 )", null);
            cb_gl_account.DataBind();
			// cb_gl_account.Value = Convert.ToInt32(a.RVAccount);

            set_ddl(a_tax1, a.Tax1);
			set_ddl(a_tax2, a.Tax2);
			set_ddl(a_tax3, a.Tax3);
			set_ddl(a_tax4, a.Tax4);

			if(a.Type == "B")
				{
				is_billing							= true;
				pc_location.TabPages.FindByName("bv_contacts").Visible = true;
				bv_contact1_name.Text				= a.BVContact1.Name;
				bv_contact1_phone1.Text				= a.BVContact1.Phone_Area;
				bv_contact1_phone2.Text				= a.BVContact1.Phone_First;
				bv_contact1_phone3.Text				= a.BVContact1.Phone_Last;
				bv_contact1_phoneext.Text			= a.BVContact1.Phone_Ext;
				bv_contact1_fax1.Text				= a.BVContact1.Fax_Area;
				bv_contact1_fax2.Text				= a.BVContact1.Fax_First;
				bv_contact1_fax3.Text				= a.BVContact1.Fax_Last;
				bv_contact1_email.Text				= a.BVContact1.Email;
		
				bv_contact2_name.Text				= a.BVContact2.Name;
				bv_contact2_phone1.Text				= a.BVContact2.Phone_Area;
				bv_contact2_phone2.Text				= a.BVContact2.Phone_First;
				bv_contact2_phone3.Text				= a.BVContact2.Phone_Last;
				bv_contact2_phoneext.Text			= a.BVContact2.Phone_Ext;
				bv_contact2_fax1.Text				= a.BVContact2.Fax_Area;
				bv_contact2_fax2.Text				= a.BVContact2.Fax_First;
				bv_contact2_fax3.Text				= a.BVContact2.Fax_Last;
				bv_contact2_email.Text				= a.BVContact2.Email;
		
				bv_contact3_name.Text				= a.BVContact3.Name;
				bv_contact3_phone1.Text				= a.BVContact3.Phone_Area;
				bv_contact3_phone2.Text				= a.BVContact3.Phone_First;
				bv_contact3_phone3.Text				= a.BVContact3.Phone_Last;
				bv_contact3_phoneext.Text			= a.BVContact3.Phone_Ext;
				bv_contact3_fax1.Text				= a.BVContact3.Fax_Area;
				bv_contact3_fax2.Text				= a.BVContact3.Fax_First;
				bv_contact3_fax3.Text				= a.BVContact3.Fax_Last;
				bv_contact3_email.Text				= a.BVContact3.Email;
				}
			else
				{
				pc_location.TabPages.FindByName("bv_contacts").Visible = false;
				}


		

			uc_contacts.customer_id		= customer_id;
			uc_contacts.address_id		= address_id;
			uc_contacts.this_customer	= this_customer;            
			hdnaddressid.Value = uc_contacts.address_id.ToString();
			ds_phones.DataBind();

			uc_contacts.fill_contacts();
			uc_contacts.fill_merge_contacts();
			//uc_phone.customer_id									= customer_id;
	
			var ds_locations								= (SqlDataSource) uc_contacts.FindControl("ds_ddl_location");
			ds_locations.SelectParameters[0].DefaultValue			= customer_id.ToString();
			var ds_contacts								= (SqlDataSource) uc_contacts.FindControl("ds_contacts");
			ds_contacts.SelectParameters[1].DefaultValue			= address_id.ToString();
			var ds_sales_contacts							= (SqlDataSource) uc_sales.FindControl("ds_contacts");
			ds_sales_contacts.SelectParameters[0].DefaultValue		= customer_id.ToString();
			ds_sales_contacts.SelectParameters[1].DefaultValue		= address_id.ToString();

			uc_sales.address_id										= (int) address_id;
			uc_sales.customer_id									= customer_id;
			uc_sales.this_customer									= this_customer;
			uc_sales.this_address									= this_address;
			uc_sales.DataBind(); // Controls initial population of sales tab... needed.

			uc_notes.address_id										= address_id;
			uc_notes.customer_id									= customer_id;
			uc_notes.this_customer									= this_customer;
			uc_notes.this_address									= this_address;
			uc_notes.init();

			uc_files.address_id										= address_id;
			uc_files.customer_id									= customer_id;
			uc_files.this_customer									= this_customer;
			uc_files.this_address									= this_address;
			uc_files.init();

			uc_assets.address_id									= address_id;
			uc_assets.customer_id									= customer_id;
			uc_assets.this_customer									= this_customer;
			uc_assets.this_address									= this_address;
			uc_assets.init();

			is_initialized				= true;
			}
		else if(address_id == 0)  // if its a new address
			{
			var c					= new NeBusinessUnit(business_unit_id);
			set_ddl(a_provstate, c.provstate);
			set_ddl(a_country, c.country);
			for(var i = 0; i < pc_location.TabPages.Count; i++)
				{
				if(i > 0)
					{
					pc_location.TabPages[i].Visible		= false;
					}
				}
			if (customer_id != 0)
			{
				// TODO: Move this to the consolidated function

				//cb_gl_account.Value = Convert.ToInt32(this_customer.GL_ID);
				//cb_gl_account0.Value = Convert.ToInt32(this_customer.GL_ID_consol);
			}
			}
		//Lockdown... uses priv 115
		if(address_id > 0)
			{
			AdminAuthenticated = !is_billing || AdminAuthenticated;
			}
		a_desc.Text												= is_billing ? "" : a_desc.Text;
		a_desc.ReadOnly											= is_billing && AdminAuthenticated;
		a_addr1.Enabled											= AdminAuthenticated;
		a_addr2.Enabled											= AdminAuthenticated;
		a_addr3.Enabled											= AdminAuthenticated;
		a_addr4.Enabled											= AdminAuthenticated;
		a_city.Enabled											= AdminAuthenticated;
		a_country.Enabled										= AdminAuthenticated;
		a_postal.Enabled										= AdminAuthenticated;
		a_provstate.Enabled										= AdminAuthenticated;
		p_area.Enabled											= AdminAuthenticated;
		p_prefix.Enabled										= AdminAuthenticated;
		p_suffix.Enabled										= AdminAuthenticated;
		p_ext.Enabled											= AdminAuthenticated;
		f_area.Enabled											= AdminAuthenticated;
		f_prefix.Enabled										= AdminAuthenticated;
		f_suffix.Enabled										= AdminAuthenticated;
		a_website.Enabled										= AdminAuthenticated;
		a_gpscoordinates.Enabled								= AdminAuthenticated;
		a_tax1.Enabled											= AdminAuthenticated;
		a_tax1_ex.Enabled										= AdminAuthenticated;
		a_tax2.Enabled											= AdminAuthenticated;
		a_tax2_ex.Enabled										= AdminAuthenticated;
		a_tax3.Enabled											= AdminAuthenticated;
		a_tax3_ex.Enabled										= AdminAuthenticated;
		a_tax4.Enabled											= AdminAuthenticated;
		a_tax4_ex.Enabled										= AdminAuthenticated;
		
		cb_gl_account.ClientEnabled =  AdminAuthenticated;
		b_save_address.Enabled									= AdminAuthenticated;
		Toolbox.do_debug("Location UC Init End");
		}
	public void Page_Init(object sender, EventArgs e)
		{
		Page.RegisterRequiresControlState(this);
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(1);
		var addr1			= a_addr1.Text;
		}
	public void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (address_id > 0)
				{
					
					
					
				}

			}
		}
	private void set_ddl(DropDownList ddl, object _value)
		{
		var value		= _value == null ? "" : _value.ToString().ToUpper();
		 ddl.SelectedValue	= value;
		}
	/// <summary>
	/// For checking the validity of required strings.
	/// </summary>
	/// <param name="s"></param>
	/// <returns></returns>
	private bool str_check(string s)
		{
		return !string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s);
		}
	/// <summary>
	/// For checking the validity of required ddls
	/// </summary>
	/// <param name="ddl"></param>
	/// <returns></returns>
	private bool ddl_check(DropDownList ddl)
		{
		return ddl.SelectedIndex >= 0;
		}
	protected void b_cancel_edit_Click(object sender, EventArgs e)
		{
		if(Parent is GridViewEditFormTemplateContainer)
			{
			var template		= (GridViewEditFormTemplateContainer) Parent;
			if(template != null)
				{
				template.Grid.DataBind();
				template.Grid.CancelEdit();
				}
			}
		}
	protected void b_save_address_Click(object sender, EventArgs e)
		{
		// Validate required elements
		var errors			= new List<string>();
		var is_error				= false;
		if(!str_check(a_addr1.Text))
			{
			is_error				= true;
			errors.Add("Blank Address - Line 1");
			a_addr1.Style["border"]	= "solid 1px #f00";
			}
		else
			{
			a_addr1.Style["border"]	= "solid 1px #ccc";
			}
		if(!str_check(a_city.Text))
			{
			is_error				= true;
			errors.Add("Blank City");
			a_city.Style["border"]	= "solid 1px #f00";
			}
		else
			{
			a_city.Style["border"]	= "solid 1px #ccc";
			}
		if(!ddl_check(a_provstate))
			{
			is_error				= true;
			errors.Add("Blank Prov / State");
			a_provstate.Style["border"]	= "solid 1px #f00";
			}
		else
			{
			a_provstate.Style["border"]	= "solid 1px #ccc";
			}
		if(!str_check(a_postal.Text))
			{
			is_error				= true;
			errors.Add("Blank Postal");
			a_postal.Style["border"]	= "solid 1px #f00";
			}
		else
			{
			a_postal.Style["border"]	= "solid 1px #ccc";
			}
		if(!ddl_check(a_country))
			{
			is_error				= true;
			errors.Add("Blank Country");
			a_country.Style["border"]	= "solid 1px #f00";
			}
		else
			{
			a_country.Style["border"]	= "solid 1px #ccc";
			}
		if(!str_check(p_area.Text))
			{
			is_error				= true;
			errors.Add("Blank Phone Area Code");
			p_area.Style["border"]	= "solid 1px #f00";
			}
		else
			{
			p_area.Style["border"]	= "solid 1px #ccc";
			}
		if(!str_check(p_prefix.Text))
			{
			is_error				= true;
			errors.Add("Blank Phone Prefix");
			p_prefix.Style["border"]	= "solid 1px #f00";
			}
		else
			{
			p_prefix.Style["border"]	= "solid 1px #ccc";
			}
		if (a_desc.Text.Length > 59)
		{
			is_error = true;
			errors.Add("Description is too int");
			a_desc.Style["border"] = 	"solid 1px #f00";

		}
		else
		{
				a_desc.Style["border"] = 	 "solid 1px #ccc";
		}
		if(!str_check(p_suffix.Text))
			{
			is_error				= true;
			errors.Add("Blank Phone Suffix");
			p_suffix.Style["border"]	= "solid 1px #f00";
			}
		else
			{
			p_suffix.Style["border"]	= "solid 1px #ccc";
			}
		if(is_error)
			{
			// Required elements have not been met.. not proceeding, deliver the error report.
			var sb			= new StringBuilder();
			sb.Append("<div class='title'>There are errors with your submission</div>");
			foreach(var err in errors)
				{
				sb.AppendFormat("<div class='err'>&bullet; {0}</div>",err);
				}
			error_report.InnerHtml		= sb.ToString();
			}
		else
			{
			var a			= new NEAddress();
			
			a.Table = a_table.SelectedValue;
			a.Table_ID			= customer_id;
			var is_new			= true;
			var prev_addr1	= "";
			var gl= "00000";
			var gl_consol ="";
			var cust = new NECustomer(Convert.ToInt32(customer_id));
			if(address_id != 0)
				{
				a				= new NEAddress(address_id);
				is_new			= false;
				prev_addr1		= a.Addr1;
				if (a.Type == "B")
				{
				}
				else
				{
                    /*
                     * This will be 00000, Before it was 41005 wrong account #
                     * a.RVAccount = cb_gl_account.Value == null ? "" : cb_gl_account.Value.ToString();
                     */
				    a.RVAccount = "00000";

                }
            }
			else
			{
				a.RVAccount = gl;
			}



			a.Desc = a_desc.Text == "" ? a_addr1.Text : a_desc.Text;
			a.Addr1				= a_addr1.Text;
			a.Addr2				= a_addr2.Text;
			a.Addr3				= a_addr3.Text;
			a.Addr4				= a_addr4.Text;
			a.City				= a_city.Text;
			a.Prov				= a_provstate.SelectedValue;
			a.Country			= a_country.SelectedValue;
			a.Postal			= a_postal.Text;
			a.Type				= is_new ? "S" : a.Type;
			a.PhoneArea			= p_area.Text;
			a.Phonefirst		= p_prefix.Text;
			a.PhoneLast			= p_suffix.Text;
			a.PhoneExt			= p_ext.Text;
			a.FaxArea			= f_area.Text;
			a.FaxFirst			= f_prefix.Text;
			a.FaxLast			= f_suffix.Text;
			a.Web				= a_website.Text;
			a.GPS_Coords		= a_gpscoordinates.Text;
			
			a.Tax1Exempt		= a_tax1_ex.Text;
			a.Tax2Exempt		= a_tax2_ex.Text;
			a.Tax3Exempt		= a_tax3_ex.Text;
			a.Tax4Exempt		= a_tax4_ex.Text;
			a.SellPrice			= a.SellPrice == "" ? "01" : a.SellPrice;
			a.facebook			= a_facebook.Text;
			a.twitter			= a_twitter.Text;
			a.linkedin			= a_linkedin.Text;
			a.Table = a_table.Text;
			
			

			try
			{
                
				address_id = a.Save();
                this_address = a;
               // NEAddress.sync_bvs(a, current_user);
            }
			catch (Exception ee)
			{
				lb_error.Text = ee.Message;
				return;
			}
			var c = new NECustomer((int)customer_id);

			if(!is_new)
				{
				// Folder checking
			
				var fileServer				= NeTaxEntity.BaseFolder(current_user.business_unit_id, false);
				var base_path				= fileServer + @"\customer_files";
				var pattern					= @"[\~#%*{}/:<>?|""]";
				base_path						= Path.Combine(base_path, string.Format("C{0}-{1}-{2}", c.Customer_ID, c.Customer_Number, Regex.Replace(c.Customer_Name, pattern, "").Replace("&", " AND ").Replace("+", "-")));
				var prev_foldername			= Regex.Replace(prev_addr1, pattern, "");
				var new_foldername			= Regex.Replace(a.Addr1, pattern, "");
				var new_path					= base_path+"\\["+a.Type+"] - "+new_foldername;
				var prev_path				= base_path+"\\["+a.Type+"] - "+prev_foldername;
				if(Directory.Exists(prev_path) && new_path.ToLower() != prev_path.ToLower())
					{
					new_path					= base_path+"\\["+a.Type+"] - "+new_foldername;
					Directory.Move(prev_path, new_path);
					}
				}
			#region Save Phone # to phone_numbers table.
	//		NePhoneNumbers b_num			= new NePhoneNumbers();
	//		b_num.phone_numbers_active		= true;
	//		b_num.phone_numbers_comm_type	= "LandLine";
	//		b_num.phone_numbers_default		= true;
	//		b_num.phone_numbers_type		= "Address";
	//		b_num.phone_numbers_table_id	= address_id;
	//		b_num.phone_numbers_number		= String.Format("{0} {1} {2}", a.PhoneArea, a.Phonefirst, a.PhoneLast);
	//		b_num.Save();
			#endregion Save Phone # to phone_numbers table.

			if(a.FaxArea != "" && a.FaxFirst != "" && a.FaxLast != "")
				{
	//			NePhoneNumbers f_num			= new NePhoneNumbers();
	//			f_num.phone_numbers_active		= true;
	//			f_num.phone_numbers_comm_type	= "Fax";
	//			f_num.phone_numbers_default		= true;
	//			f_num.phone_numbers_type		= "Address";
	//			f_num.phone_numbers_table_id	= address_id;
	//			f_num.phone_numbers_number		= String.Format("{0} {1} {2}", a.FaxArea, a.FaxFirst, a.FaxLast);
	//			f_num.Save();
				}
			grid_update(address_id, is_new);
	 
				hdnaddressid.Value = address_id.ToString();
	  			grid_update(address_id, true);
				
				is_initialized = false;
				
			if(this_customer.id == 0)
				{
				this_customer = new NECustomer(customer_id);
				}
		//	var NSI = new NetSuite_Integration.Customer();
		//	NSI.SyncNetSuite(this_customer, current_user.business_unit.tax_entity_id);
			init();
			//Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Some text here - maybe ex.Message');", true);
		//	ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "remove", "alert('Address Saved')", true);


			}
		}
	private void grid_update(int address_id, bool is_new)
		{
		if(Parent is GridViewEditFormTemplateContainer)
			{
			var template		= (GridViewEditFormTemplateContainer) Parent;
			if(template != null)
				{
				template.Grid.DataBind();
				if(is_new)
					{
					template.Grid.CancelEdit();
					template.Grid.StartEdit(template.Grid.FindVisibleIndexByKeyValue(Convert.ToInt32(address_id)));
					}
				else
				{
					template.Grid.CancelEdit();
				}
				}
			
			}
		}
	protected void save_bvcontacts_Click(object sender, EventArgs e)
		{
		var this_customer	= new NECustomer((int) customer_id);
		var _contact1			= new BVContact(address_id, 1);
		var _contact2			= new BVContact(address_id, 2);
		var _contact3			= new BVContact(address_id, 3);
		var this_error			= "";
		var is_error				= false;
		try
			{
			_contact1.Name				= bv_contact1_name.Text;
			_contact1.Phone_Area		= bv_contact1_phone1.Text;
			_contact1.Phone_First		= bv_contact1_phone2.Text;
			_contact1.Phone_Last		= bv_contact1_phone3.Text;
			_contact1.Phone_Ext			= bv_contact1_phoneext.Text;
			_contact1.Fax_Area			= bv_contact1_fax1.Text;
			_contact1.Fax_First			= bv_contact1_fax2.Text;
			_contact1.Fax_Last			= bv_contact1_fax3.Text;
			_contact1.Email				= bv_contact1_email.Text;
			_contact1.Save();
						
			_contact2.Name				= bv_contact2_name.Text;
			_contact2.Phone_Area		= bv_contact2_phone1.Text;
			_contact2.Phone_First		= bv_contact2_phone2.Text;
			_contact2.Phone_Last		= bv_contact2_phone3.Text;
			_contact2.Phone_Ext			= bv_contact2_phoneext.Text;
			_contact2.Fax_Area			= bv_contact2_fax1.Text;
			_contact2.Fax_First			= bv_contact2_fax2.Text;
			_contact2.Fax_Last			= bv_contact2_fax3.Text;
			_contact2.Email				= bv_contact2_email.Text;
			_contact2.Save();
	
			_contact3.Name				= bv_contact3_name.Text;
			_contact3.Phone_Area		= bv_contact3_phone1.Text;
			_contact3.Phone_First		= bv_contact3_phone2.Text;
			_contact3.Phone_Last		= bv_contact3_phone3.Text;
			_contact3.Phone_Ext			= bv_contact3_phoneext.Text;
			_contact3.Fax_Area			= bv_contact3_fax1.Text;
			_contact3.Fax_First			= bv_contact3_fax2.Text;
			_contact3.Fax_Last			= bv_contact3_fax3.Text;
			_contact3.Email				= bv_contact3_email.Text;
			_contact3.Save();

			//this_customer.sync_bvs(current_user);
			this_customer.add_history("Contacts Updated", current_user, current_user.business_unit_id);
			this_error					= "<div id='lb_notify'><b style='color:#090;'>Saved</b></div>";
			}
		catch (Exception ee)
			{
			this_error					= ee.Message;
			}

		lb_error.Text					= this_error;
		if(is_error)
			{
			lb_error.ForeColor		= System.Drawing.Color.Red;
			}
		else
			{
			lb_error.ForeColor		= System.Drawing.Color.Green;
			ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "remove", "alert('BV Contacts Updated')", true);
			}
		}


	protected void gv_phones_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		e.Visible = current_user.AuthenticatedForPrivilege(115);
		//	e.Visible = true;
	}

	protected void btn_save_Click(object sender, EventArgs e)
	{

		var ddl_type = (DropDownList)gv_phones.FindEditFormTemplateControl("ddl_type");
		var tb_number = (TextBox)gv_phones.FindEditFormTemplateControl("tb_number");
		var error_report = (HtmlTableCell)gv_phones.FindEditFormTemplateControl("error_report");
		var phone_numbers_id = 0;
		var is_new = gv_phones.IsNewRowEditing;
		if (!is_new)
		{
			phone_numbers_id = Convert.ToInt32(gv_phones.GetRowValues(gv_phones.EditingRowVisibleIndex, "id"));

		}
		error_report.InnerHtml = "";
		var errors = new List<string>();
		var is_error = false;
		#region Number formatting / Checking
		var phone_number = Toolbox.regex_only_numbers().Replace(tb_number.Text, "");
		#region Basic check
		if (phone_number.Length != 10)
		{
			is_error = true;
			errors.Add("Not a valid phone number, it needs to have 10 numbers");
			tb_number.Style["border"] = "solid 1px #f00";
		}
		else
		{
			phone_number = phone_number.Substring(0, 3) + " " + phone_number.Substring(3, 3) + " " + phone_number.Substring(6, 4);
			tb_number.Text = phone_number;
			tb_number.Style["border"] = "solid 1px #ccc";
		}
		#endregion Basic check
		#region Communications Type Check
		if (ddl_type.SelectedValue == "")
		{
			is_error = true;
			errors.Add("Please select a communications type");
			ddl_type.Style["border"] = "solid 1px #f00";
		}
		else
		{
			ddl_type.Style["border"] = "solid 1px #ccc";
		}
		#endregion Communications Type Check
		#endregion Number formatting / Checking
		if (is_error)
		{
			// Required elements have not been met.. not proceeding, deliver the error report.
			throw new Exception("Invalid data entry");
		}
		else
		{
			NePhoneNumbers p;
			if (is_new)
			{
				try
				{

					p = new NePhoneNumbers();
					var j = Convert.ToInt32(hdnaddressid.Value);

					p.table_id = j;

					p.type = "Address";
					p.comm_type = ddl_type.SelectedValue;
					p.number = phone_number;
					p.is_default = false;
					p.is_active = true;
					p.Save();
				}
				catch (Exception ee)
				{
					Toolbox.do_errorLog(ee);
					throw new Exception("Invalid data entry");
				}
			}
			else
			{
				p = new NePhoneNumbers(phone_numbers_id);
				p.number = phone_number;
				p.comm_type = ddl_type.SelectedValue;
				p.Save();
			}
		}
		gv_phones.CancelEdit();
		gv_phones.DataBind();

	}


	protected void gv_phones_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		var phone_number = Toolbox.regex_only_numbers().Replace(e.NewValues["number"].ToString(), "");

		if (phone_number.Length != 10)
		{

			throw new Exception("Not a valid phone number, it needs to have 10 numbers");

		}
		else
		{
			phone_number = phone_number.Substring(0, 3) + " " + phone_number.Substring(3, 3) + " " + phone_number.Substring(6, 4);

		}

		NePhoneNumbers p;
		p = new NePhoneNumbers(0);
		p.phone_numbers_table_id = Convert.ToInt32(hdnaddressid.Value);
		p.number = phone_number;
		p.is_default = Convert.ToBoolean(e.NewValues["_default"]);
		p.is_active = Convert.ToBoolean(e.NewValues["active"]);
		p.comm_type = e.NewValues["type"].ToString();
		p.type = "Address";
		p.Save();

		gv_phones.DataBind();
		e.Cancel = true;
		gv_phones.CancelEdit();
	}
	protected void gv_phones_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		var phone_number = Toolbox.regex_only_numbers().Replace(e.NewValues["number"].ToString(), "");

		if (phone_number.Length != 10)
		{

			throw new Exception("Not a valid phone number, it needs to have 10 numbers");

		}
		else
		{
			phone_number = phone_number.Substring(0, 3) + " " + phone_number.Substring(3, 3) + " " + phone_number.Substring(6, 4);

		}
		NePhoneNumbers p;
		p = new NePhoneNumbers(Convert.ToInt32(e.Keys[0]));
		p.number = phone_number;
		p.is_default = Convert.ToBoolean(e.NewValues["_default"]);
		p.is_active = Convert.ToBoolean(e.NewValues["active"]);
		p.comm_type = e.NewValues["type"].ToString();
		p.type = "Address";
		p.Save();

		gv_phones.DataBind();
		e.Cancel = true;
		gv_phones.CancelEdit();
	}
	protected void gv_phones_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		NePhoneNumbers p;
		p = new NePhoneNumbers(Convert.ToInt32(e.Keys[0]));
		p.delete();

		gv_phones.DataBind();
		e.Cancel = true;
		gv_phones.CancelEdit();
	}

	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = sender as ASPxGridView;
		if (e.Parameters != null)
		{
			var parameters = e.Parameters.Split('|');

			if (parameters[0] == "DELETE")
			{
				var key = gv.GetRowValues(Convert.ToInt32(parameters[1]), "id").ToString();
				NePhoneNumbers p;
				p = new NePhoneNumbers(Convert.ToInt32(key));
				p.delete();
				gv_phones.CancelEdit();
			}
			gv.DataBind();
		}
	}
	protected void gv_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
	{

		var key = gv_phones.GetRowValues(e.VisibleIndex, "id").ToString();
		NePhoneNumbers p;
		p = new NePhoneNumbers(Convert.ToInt32(key));
		p.delete();

		gv_phones.DataBind();
	}

	protected void b_save_address_accounting_Click(object sender, EventArgs e)
	{

		var a = new NEAddress();

		a.Table = a_table.SelectedValue;
		a.Table_ID = customer_id;
		
		var prev_addr1 = "";
		if (address_id != 0)
		{
			a = new NEAddress(address_id);

			
			
			a.Tax1 = Convert.ToInt32(a_tax1.SelectedValue);
			a.Tax2 = Convert.ToInt32(a_tax2.SelectedValue);
			a.Tax3 = Convert.ToInt32(a_tax3.SelectedValue);
			a.Tax4 = Convert.ToInt32(a_tax4.SelectedValue);
			a.Tax1Exempt = a_tax1_ex.Text;
			a.Tax2Exempt = a_tax2_ex.Text;
			a.Tax3Exempt = a_tax3_ex.Text;
			a.Tax4Exempt = a_tax4_ex.Text;

            // 00000 will be the new RVAccount # for customer addresses
			 a.RVAccount = "00000";
			try
			{
				address_id = a.Save();
			  //  NEAddress.sync_bvs(a, current_user);

            }
			catch (Exception ee)
			{
				lb_error.Text = ee.Message;
				return;
			}
		}
	}
	protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
	{

	}
}