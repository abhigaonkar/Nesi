using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class sections_customer_modules_contact : System.Web.UI.UserControl
	{
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
	private const int _page_id			= 10;
	public NECustomer this_customer  { get; set; }
	NeMember current_user;
#pragma warning disable CS0618 // Type or member is obsolete
	Toolbox _tools			= new Toolbox();
#pragma warning restore CS0618 // Type or member is obsolete
	protected void Page_Init(object sender, EventArgs e)
		{
#pragma warning disable CS0618 // Type or member is obsolete
		_tools = new Toolbox();
#pragma warning restore CS0618 // Type or member is obsolete
		current_user			= Toolbox.do_handle_authentication(Convert.ToInt32(_page_id));
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		hdn_customer_id.Value	= customer_id.ToString();
		fill_contacts();
		}
	public void fill_contacts()
		{
		gv_contacts.DataBind();
		}
	protected void gv_contacts_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var gv					= (ASPxGridView) sender;
		var orig_contact			= new NEContact(Convert.ToInt32(e.Keys["contact_id"]));
		var contact				= new NEContact(Convert.ToInt32(e.Keys["contact_id"]));
		// Validation
		var t_name					= (TextBox) gv.FindEditFormTemplateControl("t_name");
		var t_firstname				= (TextBox) gv.FindEditFormTemplateControl("t_firstname");
		var t_lastname				= (TextBox) gv.FindEditFormTemplateControl("t_lastname");
		var t_title					= (TextBox) gv.FindEditFormTemplateControl("t_title");
		var t_email					= (TextBox) gv.FindEditFormTemplateControl("t_email");
		var t_cell					= (TextBox) gv.FindEditFormTemplateControl("t_cell");
		var t_direct				= (TextBox) gv.FindEditFormTemplateControl("t_direct");
		var t_extension				= (TextBox) gv.FindEditFormTemplateControl("t_extension");
		var t_password				= (TextBox) gv.FindEditFormTemplateControl("t_password");
		var t_facebook				= (TextBox) gv.FindEditFormTemplateControl("t_facebook");
		var t_twitter				= (TextBox) gv.FindEditFormTemplateControl("t_twitter");
		var t_linkedin				= (TextBox) gv.FindEditFormTemplateControl("t_linkedin");
		var chk_loginenabled = (ASPxCheckBox)gv.FindEditFormTemplateControl("chk_loginenabled");
		var ddl_status			= (DropDownList) gv.FindEditFormTemplateControl("ddl_status");
		
		var ddl_active			= (ASPxComboBox) gv.FindEditFormTemplateControl("ddl_active");
		var ddl_location		= (ASPxComboBox) gv.FindEditFormTemplateControl("ddl_location");
		var stopsurveys = (ASPxCheckBox)gv.FindEditFormTemplateControl("chk_stop_surveys");

		var randompass_set = false;
		var name						= t_name.Text.Trim();
		var name_first				= t_firstname.Text.Trim();
		var name_last				= t_lastname.Text.Trim();
		var active					= "";
		try
			{
			active						= ddl_active.Value.ToString();
			}
		catch
			{
			throw new Exception("Please select a value for 'Active'");
			}
		var cell						= Toolbox.regex_only_numbers().Replace(t_cell.Text, "");
		var direct					= Toolbox.regex_only_numbers().Replace(t_direct.Text, "");
		var facebook					= t_facebook.Text.Trim();
		var twitter					= t_twitter.Text.Trim();
		var linkedin					= t_linkedin.Text.Trim();
		var login_enabled				= 0;
		login_enabled				= Convert.ToInt32(chk_loginenabled.Value);
		var email					= t_email.Text.Trim();
		if(email != "" && !Toolbox.CheckEmail(email))
			{
			if(email.Contains(";"))
				{
				var emails		= email.Split(';');
				foreach(var _e in emails)
					{
					if(!Toolbox.CheckEmail(_e))
						{
						throw new Exception("The supplied email address is not in a valid user@domain.com format.");
						}
					}
				}
			else
				{
				throw new Exception("The supplied email address is not in a valid user@domain.com format.");
				}
			}
		var title					= t_title.Text.Trim();
		var password					= t_password.Text.Trim();
		var extension				= t_extension.Text.Trim();
		var status_id					= 0;
		try
			{
			status_id					= Convert.ToInt32(ddl_status.SelectedValue);
			}
		catch
			{
			throw new Exception("Please select a value for 'Status'");
			} 

		if (name == "")
			{
			throw new Exception("Contact name can't be blank");
			}
		if (login_enabled == 1)
			{
			if(email == "")
				{
				throw new Exception("If NESI login is Active, the email can't be BLANK");
				}
			randompass_set = password == "";
			contact.password = password == "" ? Toolbox.do_RandomString(8) : password;
			}
		if(cell != "" && cell.Length != 10)
			{
			throw new Exception("Not a valid cell phone number, it needs to have 10 numbers");
			}
		else if(cell.Length == 10)
			{
			cell			= cell.Substring(0,3)+"-"+cell.Substring(3,3)+"-"+cell.Substring(6,4);
			}
		if(direct != "" && direct.Length != 10)
			{
			throw new Exception("Not a valid direct phone number, it needs to have 10 numbers");
			}
		else if(direct.Length == 10)
			{
			direct			= direct.Substring(0,3)+"-"+direct.Substring(3,3)+"-"+direct.Substring(6,4);
			}
		contact.name			= name;
		contact.status_id		= status_id;
		contact.status			= active;
		contact.login_enabled	= login_enabled;
		contact.extension		= extension;
		contact.cellphone		= cell;
		contact.direct_line		= direct;
		contact.email			= email;
		contact.title			= title;
		contact.stopsurveys = stopsurveys.Checked;
		if (!contact.stopsurveys)
		{
			if (_tools.getSQL_int(@"Select count(contactpage_id) from contactpage  where contactpage_page_id = 161 and contactpage_contact_id =@v0", new object[] { contact.Contact_ID}) == 0)
			{ _tools.getSQL_void("insert into contactpage (contactpage_contact_id,contactpage_page_id,contactpage_member_id) values(@v0,@v1,@v2)", 
				new object[] { contact.Contact_ID ,161, (Convert.ToInt32(contact.Contact_ID)+100000) } );

			}
		}
		try
			{
			contact.address_id	= Convert.ToInt32(ddl_location.Value);
			}
		catch
			{
			throw new Exception("Please select a value for 'Location'");
			}
		
		contact.name_first		= name_first;
 		contact.name_last		= name_last;
		contact.facebook		= facebook;
		contact.twitter			= twitter;
		contact.linkedin		= linkedin;

		if (randompass_set)
			{
			contact.contact_password_set = false;
			}
		else if (orig_contact.Contact_Password != contact.Contact_Password && orig_contact.contact_password_set)
			{
			contact.contact_password_set = false;
			}
		contact.save();
		if (contact.login_enabled==1)
		{
			check_home_page_access(contact.Contact_ID);
		}
		e.Cancel = true;
		gv_contacts.CancelEdit();
		fill_contacts();
		}
	protected void check_home_page_access(int contactid)
	{
		if (contactid != 0)
		{
			var co = new NEContact(contactid);
			var user = new NeMember(co.nesi_member_id);
			if (!user.AuthenticatedForPage(1))
			{
				if (!NEUserPage.exists((int) contactid, 1, 2))
				{
					var NE_up = new NEUserPage();
					NE_up.admin = current_user;
					NE_up.user = user;
					NE_up.page_id = 1;
					NE_up.user_id = (int) contactid;
					NE_up.type_id = 2;  // type 2  = contact
					NE_up.save();
				}
			}

		}
	}
	protected void gv_contacts_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var gv		= (ASPxGridView) sender;
		NEContact.delete(Convert.ToInt32(e.Values["contact_id"]));
		fill_contacts();
		e.Cancel		= true;
		}
	protected void gv_contacts_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
		var gv					= (ASPxGridView) sender;
		var contact				= new NEContact();
		var t_name					= (TextBox) gv.FindEditFormTemplateControl("t_name");
		var t_firstname				= (TextBox) gv.FindEditFormTemplateControl("t_firstname");
		var t_lastname				= (TextBox) gv.FindEditFormTemplateControl("t_lastname");
		var t_title					= (TextBox) gv.FindEditFormTemplateControl("t_title");
		var t_email					= (TextBox) gv.FindEditFormTemplateControl("t_email");
		var t_cell					= (TextBox) gv.FindEditFormTemplateControl("t_cell");
		var t_direct				= (TextBox) gv.FindEditFormTemplateControl("t_direct");
		var t_extension				= (TextBox) gv.FindEditFormTemplateControl("t_extension");
		var t_password				= (TextBox) gv.FindEditFormTemplateControl("t_password");
		var t_facebook				= (TextBox) gv.FindEditFormTemplateControl("t_facebook");
		var t_twitter				= (TextBox) gv.FindEditFormTemplateControl("t_twitter");
		var t_linkedin				= (TextBox) gv.FindEditFormTemplateControl("t_linkedin");
		var ddl_status			= (DropDownList) gv.FindEditFormTemplateControl("ddl_status");
		var chk_loginenabled = (ASPxCheckBox)gv.FindEditFormTemplateControl("chk_loginenabled");
		var ddl_active			= (ASPxComboBox) gv.FindEditFormTemplateControl("ddl_active");
		var ddl_location		= (ASPxComboBox) gv.FindEditFormTemplateControl("ddl_location");
		var stopsurveys = (ASPxCheckBox)gv.FindEditFormTemplateControl("chk_stop_surveys");
		var randompass_set				= false;
		var name						= t_name.Text.Trim();
		var name_first				= t_firstname.Text.Trim();
		var name_last				= t_lastname.Text.Trim();
		var active					= "Active";
		try
			{
			active						= ddl_active.Value.ToString();
			}
		catch (Exception)
			{
			throw new Exception("Please select a value for 'Active'");
			}
		var cell						= Toolbox.regex_only_numbers().Replace(t_cell.Text, "");
		var direct					= Toolbox.regex_only_numbers().Replace(t_direct.Text, "");
	
		var	login_enabled				= Convert.ToInt32(chk_loginenabled.Value);
		var email					= t_email.Text.Trim();
		var title					= t_title.Text.Trim();
		var password					= t_password.Text.Trim();
		var extension				= t_extension.Text.Trim();
		var status_id					= 1;
		var facebook					= t_facebook.Text.Trim();
		var twitter					= t_twitter.Text.Trim();
		var linkedin					= t_linkedin.Text.Trim();

		
		if (name_first == "")
			{
			throw new Exception("First Name Cannot Be Blank");
			}
		if (name_last == "")
			{
			throw new Exception("Last Name Cannot Be Blank");
			}
		if (name == "")
			{
			throw new Exception("Contact Name Cannot Be Blank");
			}
		if (login_enabled == 1)
			{
			if(email == "")
				{
				throw new Exception("If NESI login is Active, the email can't be BLANK");
				}
			randompass_set = password == "";
			contact.password = password == "" ? Toolbox.do_RandomString(8) : password;
			}

		#region Duplicate Name Check
		var x = Toolbox.doSQL_int(@"SELECT COUNT(contact_id) FROM contact
WHERE contact_name LIKE CONCAT('%',@v0,'%') AND contact_cust_id = @v1", new object[] { name, customer_id});
		if (x > 0)
			{
			throw new Exception("Sorry a contact with this name already exists for this customer.");
			}
		#endregion Duplicate Name Check
		#region Duplicate Email Check
		if (email != "")
			{
			x = Toolbox.doSQL_int(@"SELECT COUNT(contact_id) FROM contact WHERE contact_email LIKE CONCAT('%',@v0,'%') AND contact_cust_id = @v1",
				new object[] { email, customer_id});
			if (x > 0)
				{
				throw new Exception("Sorry a contact with this email already exists for this customer.");
				}
			}
		#endregion Duplicate Email Check
		#region Duplicate Cellphone Check
		if (cell != "")
			{
			x = Toolbox.doSQL_int(@"SELECT COUNT(contact_id) FROM contact WHERE contact_cellphone LIKE  CONCAT('%',@v0,'%') AND contact_cust_id = @v1",
				new object[] { cell, customer_id});
			if (x > 0)
				{
				throw new Exception("Sorry a contact with this cell phone already exists for this customer.");
				}
			}
		#endregion Duplicate Cellphone Check
		#region Duplicate Direct Line Check
		if (direct != "")
			{
			x = Toolbox.doSQL_int(@"SELECT COUNT(contact_id) FROM contact WHERE contact_directline like CONCAT('%',@v0,'%') AND contact_cust_id = @v1",
				new object[] { cell, customer_id });
			if (x > 0)
				{
				throw new Exception("Sorry a contact with this direct line already exists for this customer.");
				}
			}
		#endregion Duplicate Direct Line Check
		#region Duplicate Direct Line AS Cellphone Check
		if (direct != "")
			{
			x = Toolbox.doSQL_int(@"SELECT COUNT(contact_id) FROM contact WHERE contact_cellphone like CONCAT('%',@v0,'%') AND contact_cust_id = @v1",
				new object[] { cell, customer_id });
			if (x > 0)
				{
				throw new Exception("Sorry a contact with this cell phone already exists for this customer.");
				}
			}
		#endregion Duplicate Direct Line AS Cellphone Check
		contact.name					= name;
		contact.name_first				= name_first;
		contact.name_last				= name_last;
		contact.status					= active;
		contact.login_enabled			= login_enabled;
		contact.status_id				= status_id;
		contact.extension				= extension;
		contact.cellphone				= cell;
		contact.direct_line				= direct;
		contact.title					= title;
		contact.email					= email;
		contact.type					= "Customer";
		contact.customer_id				= customer_id;
		contact.address_id				= (int) address_id;
		contact.facebook				= facebook;
		contact.twitter					= twitter;
		contact.linkedin				= linkedin;
		contact.stopsurveys = Convert.ToBoolean(stopsurveys.Checked);
		contact.save();
		if (!contact.stopsurveys)
		{
			if (_tools.getSQL_int(@"Select count(contactpage_id) from contactpage  where contactpage_page_id = 161 and contactpage_contact_id =@v0", new object[] { contact.Contact_ID}) == 0)
			{ _tools.getSQL_void("insert into contactpage (contactpage_contact_id,contactpage_page_id,contactpage_member_id) values(@v0,@v1,@v2)",
				new object[] { contact.Contact_ID ,161, (Convert.ToInt32(contact.Contact_ID)+100000) } );

			}
		}

		if (contact.login_enabled == 1)
		{
			check_home_page_access(contact.Contact_ID);
		}
		if(this_customer.id == 0)
			{
			this_customer = new NECustomer(customer_id);
			}
	//	var NSI = new NetSuite_Integration.Customer();
	//	NSI.SyncNetSuite(this_customer, current_user.business_unit.tax_entity_id);
		e.Cancel = true;
		gv_contacts.CancelEdit();
		fill_contacts();
		}
	public void fill_merge_contacts()
		{
		var merge_from_contacts		= Toolbox.doSQL_dt(@"SELECT contact_id,CONCAT(contact_name, ' - Email:', contact_email) contact_name FROM contact,customer_or_contact_status WHERE contact_cust_id = @v0  and contact_status = 'Active' AND contact_type = 'Customer' and customer_or_contact_status_id = contact_status_id", new object[] {  customer_id } );
		var merge_to_contacts			= Toolbox.doSQL_dt(@"SELECT contact_id,CONCAT(contact_name, ' - Email:', contact_email) contact_name FROM contact,customer_or_contact_status WHERE contact_cust_id = @v0  and contact_status = 'Active' AND contact_type = 'Customer' and customer_or_contact_status_id = contact_status_id", new object[] {  customer_id } );
		combo_merge_from.DataSource			= merge_from_contacts;
		combo_merge_from.DataBind();
		combo_merge_to.DataSource			= merge_to_contacts;
		combo_merge_to.DataBind();
		}
	protected void gv_contacts_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
		{
		var gv		              = (ASPxGridView) sender;
		e.NewValues["status"]             = 8;
		e.NewValues["login_enabled"]      = 0;
		e.NewValues["contact_status"]     = "Active";
		e.NewValues["address_id"]         = address_id;
		e.NewValues["contact_name"]       = "";
		e.NewValues["contact_extension"]  = "";
		e.NewValues["contact_cellphone"]  = "";
		e.NewValues["contact_email"]      = "";
		e.NewValues["contact_directline"] = "";
		e.NewValues["contact_title"]      = "";
		e.NewValues["contact_password"]   = Toolbox.do_RandomString(8);
		}
	protected void cb_merge_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		var ids				= e.Parameter.Split('|');
		try
			{
			new NEContact().merge_contacts(ids[0], ids[1], current_user);
			e.Result			= "SUCCESS";
			}
		catch (Exception ee)
			{
			_tools.debug_note("Failed Merging Contacts ("+e.Parameter+").\nError Given:"+ee);
			e.Result			= "Failed trying to merge these contacts.";
			}
		}
	protected void gv_contacts_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
		{
		if (e.ButtonID == "Merge")
			{
			var contact_id					= Convert.ToInt32(gv_contacts.GetRowValues(e.VisibleIndex, "contact_id"));
			pop_merge.ShowOnPageLoad		= true;
			combo_merge_from.SelectedItem	= combo_merge_from.Items.FindByValue(contact_id);
			}
		}
	protected void gv_contact_custom_js(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
        var IDs				= new object[gv.VisibleRowCount];
        for(var i = 0; i < gv.VisibleRowCount; i++)
        	{
			IDs[i]					= gv.GetRowValues(i, "contact_id");
        	}
		e.Properties["cpid"]		= IDs;
		}
	protected void gv_contacts_ClientLayout(object sender, ASPxClientLayoutArgs e)
		{
		if (!current_user.AuthenticatedForPrivilege(99))
			{
			var gv = (ASPxGridView)sender;
			gv.Columns["contact_password"].Visible = false;
			gv.Columns[0].Visible = false;
			}
		}
	protected void gv_contacts_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		var gv					= (ASPxGridView) sender;
		var priv_templates		= (HtmlContainerControl) gv.FindEditFormTemplateControl("template_div");
		var ddl_status			= (DropDownList) gv.FindEditFormTemplateControl("ddl_status");
		var chk_loginenabled = (ASPxCheckBox)gv.FindEditFormTemplateControl("chk_loginenabled");

		if(gv.IsNewRowEditing)
			{
			priv_templates.Visible	= false;
			if(ddl_status.Items.Count > 0)
				{
				ddl_status.SelectedValue = "1";
				}
			ddl_status.Enabled		= false;
			chk_loginenabled.Value	= 0;
			}
		}
	protected void gv_contacts_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
		{
		var gv			= (ASPxGridView) sender;
		if(e.Column.FieldName == "contact_password")
			{
			var password		= e.Value.ToString();
			if(string.IsNullOrEmpty(password))
				{
				var tb_password	= (TextBox) gv.FindEditFormTemplateControl("tb_password");
				tb_password.Text	= Toolbox.do_RandomString(6);
				}
			}
		}
	protected void ds_ddl_location_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
		{
		e.Command.Parameters[0].Value		= customer_id;
		}
	protected void ddl_loginenabled_Init(object sender, EventArgs e)
		{
		var ddl		= (DropDownList) sender;
		
		}
	protected void gv_contacts_DataBound(object sender, EventArgs e)
		{
		}
	protected void gv_contacts_DataBinding(object sender, EventArgs e)
		{
		var gv			= (ASPxGridView) sender;

		}
}