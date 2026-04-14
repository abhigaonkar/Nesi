using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Web.Services;
//using nesi.bv;
using nesi.core;
public partial class sections_vendor_index : Page
	{
	/*
	int current_vendor_id;
	private NeMember current_user;
	NEVendor vendobj;
	bool process_save;
	private const int _page_id = 11;
	DataTable dt_vendors = new DataTable();
	NameValueCollection _q;
	private bool _can_save_vendor;
	private bool _can_edit_vendor_contacts;
	protected void Page_Init(object sender, EventArgs e)
		{
		current_user = Toolbox.do_handle_authentication(_page_id);
		_can_save_vendor = current_user.AuthenticatedForPrivilege(12);
		_can_edit_vendor_contacts = current_user.AuthenticatedForPrivilege(108);
		if (!IsCallback && !IsPostBack)
			{
			Session["current_vendor_id"] = null;
			}
		}
	protected void Page_Load(object sender, EventArgs e)
	{
		_q = Request.QueryString;
            if (!string.IsNullOrEmpty(_q["vendor_id"]))
            {
                Toolbox.FriendlyException(Response, "Please update your bookmarks. This Vendor page is no longer to be used.", "/#/opens/11/vendors/" + _q["vendor_id"]); 
           }
        t_name.ReadOnly = true;
        b_new.Visible = false;
		if (!IsPostBack)
			{
			Session["vendor_emails"] = null;
			}
		if (
		!string.IsNullOrEmpty(_q["add_to_branch"]) &&
		!string.IsNullOrEmpty(_q["frombusiness_unit_id"]) &&
		!string.IsNullOrEmpty(_q["tobusiness_unit_id"]) &&
		!string.IsNullOrEmpty(_q["vendor_id"])
		)
			{

           // Toolbox.FriendlyException(Response, "Please update your bookmarks. This Vendor page is no longer to be used.", "/#/home/11/vendors/eidt/" + _q["vendor_id"]);
            var strError = "";
			var strFormDSN = "";
			var strToDSN = "";
			var strFromCoID = _q["frombusiness_unit_id"];
			var strToCoID = _q["tobusiness_unit_id"];
			var strVendorNo = Toolbox.doSQL_string(@"SELECT vendor_number FROM vendor  WHERE vendor_id =@v0 limit 1 ", new object[] { _q["vendor_id"] });
			if (strFromCoID != strToCoID)
				{
				strFormDSN = Toolbox.doSQL_string(@"Select tax_entity.dsn from business_unit inner join tax_entity on tax_entity.id = company.tax_entity_id  where business_unit_id =@v0", new object[] { strFromCoID });
				strToDSN = Toolbox.doSQL_string(@"Select tax_entity.dsn from business_unit inner join tax_entity on tax_entity.id = company.tax_entity_id  where business_unit_id =@v0", new object[] { strToCoID });
				var fromVendor = new Vendor(strFormDSN, strVendorNo);
				try
					{
					fromVendor.VendorName = fromVendor.VendorName.Replace("'", "''");
					fromVendor.VendorAddress.Name = fromVendor.VendorAddress.Name.Replace("'", "''");

					fromVendor.Save(strToDSN, true, current_user.Initials);

					}
				catch (Exception ex)
					{
					strError = ex.Message;
					}
				if (strError == "")
					{
					strError = "SUCCESS";
					}
				}
			else
				{
				strError = "Already existing in the destination branch";
				}
			Response.Write(strError);
			Response.End();
			}
		if (!string.IsNullOrEmpty(_q["vendor_id"]) && !persist.Contains("current_vendor_id") && !persist.Contains("row_index"))
			{
			b_new.Visible = !string.IsNullOrEmpty(_q["first_tab"]);
			Session["current_vendor_id"] = _q["vendor_id"];
			hid_vendor_id.Value = _q["vendor_id"];
			}
		if (string.IsNullOrEmpty(_q["vendor_id"]) && !persist.Contains("current_vendor_id") && persist.Contains("row_index"))
			{
			current_vendor_id = persist.Contains("row_index") ? Convert.ToInt32(gv_vendor.GetDataRow(Convert.ToInt32(persist["row_index"]))["vendor_id"]) : current_vendor_id;
			persist.Remove("row_index");
			}
		else if (persist.Contains("row_index"))
			{
			current_vendor_id = persist.Contains("row_index") ? Convert.ToInt32(gv_vendor.GetDataRow(Convert.ToInt32(persist["row_index"]))["vendor_id"]) : current_vendor_id;
			Session["current_vendor_id"] = current_vendor_id;
			Session["vendor_emails"] = null;
			hid_vendor_id.Value = current_vendor_id.ToString();
			fill_gvpos();
			fill_emails();
			partner_skills1.vendor_id = hid_vendor_id.Value != null ? Convert.ToInt32(hid_vendor_id.Value) : 0;
			ViewState["vendor_id"] = partner_skills1.vendor_id;
			persist.Remove("row_index");
			}
		else if (persist.Contains("current_vendor_id"))
			{
			current_vendor_id = Convert.ToInt32(persist["current_vendor_id"]);
			}
		else if (!string.IsNullOrEmpty(_q["vendor_id"]))
			{
			current_vendor_id = Convert.ToInt32(_q["vendor_id"]);
			Session["current_vendor_id"] = _q["vendor_id"];
			hid_vendor_id.Value = _q["vendor_id"];
			}
		if (!string.IsNullOrEmpty(_q["parent"]))
			{
			hidParent["root"] = _q["Parent"];
			}
		else
			{
			hidParent["root"] = "";
			}
		persist["current_vendor_id"] = current_vendor_id;
		vendobj = current_vendor_id == 0 ? new NEVendor() : new NEVendor(current_vendor_id);
		pc_main.TabPages[1].ClientEnabled = current_vendor_id > 0;
		b_save_vendor.Visible = b_save_contact.Visible = _can_save_vendor;
		partner_skills1.init();
		gv_phonecalls.DataBind();
		if (Session["current_vendor_id"] != null)
			{
			vendobj = new NEVendor(Convert.ToInt32(Session["current_vendor_id"]));
			hdnaddressid.Value = vendobj.Address.id.ToString();
			ds_phones.DataBind();
			partner_skills1.vendor_id = hid_vendor_id.Value != null ? Convert.ToInt32(hid_vendor_id.Value) : 0;
			ViewState["vendor_id"] = partner_skills1.vendor_id;
			}
		if (!cbp_main.IsCallback && !cbp_contacts.IsCallback)
			{
			pc_main.ActiveTabPage = pc_main.TabPages[0];
			pc_detail.ActiveTabPage = pc_detail.TabPages[0];
			bind("provstate");
			bind("country");
			bind("mainbranch");
			bind("terms");
			if (!string.IsNullOrEmpty(_q["vendor_id"]))
				{
				pc_main.TabPages[0].ClientEnabled = !string.IsNullOrEmpty(_q["first_tab"]) && _q["first_tab"] == "true";
				pc_main.TabPages[1].ClientEnabled = true;
				pc_main.ActiveTabPage = pc_main.TabPages[1];
				pc_detail.ActiveTabPage = pc_detail.TabPages[0];
				fill_main();
				}
			}
		else
			{
			pc_main.TabPages[1].ClientEnabled = true;
			pc_main.ActiveTabPage = pc_main.TabPages[1];
			pc_detail.ActiveTabPage = pc_detail.TabPages[0];
			}
		if (Session["current_vendor_id"] != null && Session["current_vendor_id"].ToString() != "0")
			{
			fill_gvpos();
			}
		fill_emails();
		}
	protected void hl_name_Init(object sender, EventArgs e)
		{
		var container = ((ASPxHyperLink)sender).NamingContainer as GridViewDataItemTemplateContainer;
		((ASPxHyperLink)sender).ClientSideEvents.Click = string.Format("function(s,e){{row_click(s,e,{0}); }}", container.VisibleIndex);
		}
	// protected void l_addbranch_Init(object sender, EventArgs e)
	// 	{
	// 	ASPxHyperLink link								= (ASPxHyperLink) sender;
	// 	GridViewDataItemTemplateContainer container		= link.NamingContainer as GridViewDataItemTemplateContainer;
	// 	int this_business_unit_id							= Convert.ToInt32(gv_vendor.GetDataRow(container.VisibleIndex)[4]);
	// 	string this_vendor_id							= gv_vendor.GetDataRow(container.VisibleIndex)[0].ToString();
	// 	string this_vendor_number						= gv_vendor.GetDataRow(container.VisibleIndex)[5].ToString();
	// 	string text										= "Yes";
	// 	string action									= "";
	// 	bool exists										= this_vendor_number == "" ? false : vendor_string.Contains(this_vendor_number);
	// 	if(this_business_unit_id != current_user.business_unit_id && !exists)
	// 		{
	// 		text										= "Add to Branch";
	// 		action										= String.Format("function(s,e){{add_to_branch({0}, {1}, {2}); }}", this_business_unit_id, current_user.business_unit_id, this_vendor_id);
	// 		}
	// 	link.Visible					= (text != "");
	// 	link.Text						= text;
	// 	link.NavigateUrl				= action != "" ? "javascript:void();" : "";
	// 	link.ClientSideEvents.Click		= action;
	// 	}
	// 
	protected void bind(string type)
		{
		switch (type)
			{
			case "provstate":
				cb_provstate.DataSource = new NeProvince().Load();
				cb_provstate.DataBind();
				cb_new_provstate.DataSource = new NeProvince().Load();
				cb_new_provstate.DataBind();
				var li1 = new ListEditItem("Unknown", 0);
				cb_new_provstate.Items.Add(li1);
				cb_new_provstate.SelectedItem = cb_new_provstate.SelectedIndex > -1 ? cb_new_provstate.SelectedItem : cb_new_provstate.Items.FindByValue(current_user.Prov);
				break;
			case "country":
				cb_country.DataSource = NeCountry.get_list();
				cb_country.DataBind();
				cb_new_country.DataSource = NeCountry.get_list();
				cb_new_country.DataBind();
				cb_new_country.SelectedItem = cb_new_country.SelectedIndex > -1 ? cb_new_country.SelectedItem : cb_new_country.Items.FindByValue(current_user.Country);
				break;
			case "mainbranch":
				cb_main_branch.DataSource = NeBusinessUnit.units_with_BV(current_user);
				cb_main_branch.DataBind();
				break;
			case "terms":
				cb_terms.DataSource = Toolbox.doSQL_dt(@"SELECT term_id value, CAST(CONCAT(term_code,' - ', term_desc) AS CHAR) text FROM term"  , null);
				cb_terms.DataBind();
				cb_new_terms.DataSource = Toolbox.doSQL_dt(@"SELECT term_id value, CAST(CONCAT(term_code,' - ', term_desc) AS CHAR) text FROM term"  , null);
				cb_new_terms.DataBind();
				cb_new_terms.SelectedIndex = cb_new_terms.SelectedIndex == -1 ? 0 : cb_new_terms.SelectedIndex;
				break;
			//case "gl":
			//ddl_payable_gl.DataSource = Toolbox.doSQL_dt(@"SELECT id value, CAST(CONCAT(account_no,' - ',gl_chart_name) AS CHAR) text FROM gl_te  where is_active=1 and gl_group_id = 12 order by account_no " , null);
			//ddl_payable_gl.DataBind();
			//ddl_payable_gl_consol.DataSource = Toolbox.doSQL_dt(@"SELECT id value, CAST(CONCAT(account_no,' - ',gl_chart_name) AS CHAR) text FROM gl_te  where is_active=1 and gl_group_id = 12 order by account_no" , null);
			//ddl_payable_gl_consol.DataBind();
			//ddl_expense_gl.DataSource = Toolbox.doSQL_dt(@"SELECT account_no value, CAST(CONCAT(account_no,' - ',gl_chart_name) AS CHAR) text FROM gl_te inner join gl_group_te on gl_group_te.id = gl_te.gl_group_id  where gl_te.is_active=1 and type ='X' order by account_no " , null);
			//ddl_expense_gl.DataBind();
			//ddl_expense_gl_consol.DataSource = Toolbox.doSQL_dt(@"SELECT account_no value, CAST(CONCAT(account_no,' - ',gl_chart_name) AS CHAR) text FROM gl_te inner join gl_group_te on gl_group_te.id = gl_te.gl_group_id  where gl_te.is_active=1 and type ='X' order by account_no" , null);
			//ddl_expense_gl_consol.DataBind();
			//break;
			}
		}
	protected void clear()
		{
		t_name.Text = "";
		cb_main_branch.SelectedIndex = -1;
		t_address1.Text = "";
		t_address2.Text = "";
		t_address3.Text = "";
		t_address4.Text = "";
		t_city.Text = "";
		t_postal.Text = "";
		cb_provstate.SelectedIndex = -1;
		cb_country.SelectedIndex = -1;
		cb_terms.SelectedIndex = -1;
		cb_id_type.SelectedIndex = -1;
		ck_cprs.Checked = false;
		ck_onhold.Checked = false;
		ck_partner.Checked = false;
		ck_poexempt.Checked = false;
		t_notes.Text = "";
		t_id_number.Text = "";
		t_account_number.Text = "";
		t_buyer_name.Text = "";
		t_credit_limit.Text = "";
		t_phone_area.Text = "";
		t_phone_prefix.Text = "";
		t_phone_suffix.Text = "";
		t_phone_ext.Text = "";
		t_fax_area.Text = "";
		t_fax_prefix.Text = "";
		t_fax_suffix.Text = "";
		t_email.Text = "";
		t_web.Text = "";
		t_memo.Text = "";
		t_past_memo.InnerHtml = "";
		vend_search_t.Text = "";
		}
	protected void fill_main()
		{
		var main_branch = cb_main_branch.Value == null && vendobj != null ? new NeBusinessUnit(vendobj.business_unit_id) : new NeBusinessUnit(cb_main_branch.Value);
		using (var conn = Toolbox.connect())
			{
			using (var bv_conn = BVDB.connect(main_branch.DSN))
				{
				if (vendobj == null)
					{
					return;
					}
				if (!IsPostBack)
					{
					clear();
					}
				#region Header
				t_name.Text = vendobj.Name;
				ck_onhold.Checked = vendobj.Vendor_Hold == "T";
				ck_partner.Checked = vendobj.is_partner;
				cb_main_branch.SelectedItem = cb_main_branch.Items.FindByValue(Convert.ToInt32(vendobj.business_unit_id));
				l_vendor_id.Text = vendobj.ID.ToString();
				l_vendor_number.Text = vendobj.Number;
				hidvendno.Value = vendobj.Number;
				l_qc_date.Text = vendobj.QC_DateTime.ToString("MM/dd/yyyy hh:mm:tt");
				l_date_added.Text = vendobj.Vendor_CreatedDateTime.ToString("MM/dd/yyyy hh:mm:tt");
				l_added_by.Text = new NeMember(Convert.ToInt32(vendobj.InitMember_ID)).FullName;
				#endregion Header
				#region Address Tab
				t_address1.Text = vendobj.Address.Addr1 != null ? vendobj.Address.Addr1 : "";
				t_address2.Text = vendobj.Address.Addr2 != null ? vendobj.Address.Addr2 : "";
				t_address3.Text = vendobj.Address.Addr3 != null ? vendobj.Address.Addr3 : "";
				t_address4.Text = vendobj.Address.Addr4 != null ? vendobj.Address.Addr4 : "";
				t_city.Text = vendobj.Address.City;
				t_postal.Text = vendobj.Address.Postal;
				cb_provstate.SelectedItem = cb_provstate.Items.FindByValue(vendobj.Address.Prov);
				cb_country.SelectedItem = cb_country.Items.FindByValue(vendobj.Address.Country);
				t_phone_area.Text = vendobj.Address.PhoneArea;
				t_phone_prefix.Text = vendobj.Address.Phonefirst;
				t_phone_suffix.Text = vendobj.Address.PhoneLast;
				t_phone_ext.Text = vendobj.Address.PhoneExt;
				t_fax_area.Text = vendobj.Address.FaxArea;
				t_fax_prefix.Text = vendobj.Address.FaxFirst;
				t_fax_suffix.Text = vendobj.Address.FaxLast;
				t_email.Text = vendobj.Address.Email;
				t_web.Text = vendobj.Address.Web;
				#endregion Address Tab
				#region Accounting Tab
				cb_terms.SelectedItem = vendobj.Vendor_Term_ID != 0 ? cb_terms.Items.FindByValue(vendobj.Vendor_Term_ID) : cb_terms.Items[0];
				ck_poexempt.Checked = vendobj.Vendor_PO_Exempt == 1;
				
				ck_cprs.Checked = vendobj.Vendor_CPRS == "T";
				cb_id_type.SelectedItem = cb_id_type.Items.FindByValue(vendobj.Vendor_IDType);
				t_id_number.Text = vendobj.Vendor_IDNumber;
				cb_credit_type.SelectedItem = cb_credit_type.Items.FindByValue(vendobj.Vendor_Credit_Type);
				t_credit_limit.Text = vendobj.Vendor_Credit_Limit.ToString();
				t_account_number.Text = vendobj.Vendor_Account;
				t_buyer_name.Text = vendobj.Vendor_Buyer;
                ddl_tax1.Value = vendobj.Address.Tax1;
                ddl_tax2.Value = vendobj.Address.Tax2;
                ddl_tax3.Value = vendobj.Address.Tax3;
                ddl_tax4.Value = vendobj.Address.Tax4;

                #endregion Accounting Tab
                #region Contacts Tab
                lb_contacts.Items[0].Selected = true;
				t_contact_name.Text = vendobj.Address.BVContact1.Name;
				t_contact_phone_area.Text = vendobj.Address.BVContact1.Phone_Area;
				t_contact_phone_prefix.Text = vendobj.Address.BVContact1.Phone_First;
				t_contact_phone_suffix.Text = vendobj.Address.BVContact1.Phone_Last;
				t_contact_phone_ext.Text = vendobj.Address.BVContact1.Phone_Ext;
				t_contact_fax_area.Text = vendobj.Address.BVContact1.Fax_Area;
				t_contact_fax_prefix.Text = vendobj.Address.BVContact1.Fax_First;
				t_contact_fax_suffix.Text = vendobj.Address.BVContact1.Fax_Last;
				t_contact_email.Text = vendobj.Address.BVContact1.Email;
				hid_ts.Value = vendobj.ts.Ticks.ToString();
				//NESI Contact List
				#endregion Contacts Tab
				t_notes.Text = vendobj.Vendor_Notes;
				#region NotePage
				#region files
				files_frame.Attributes.Add("src", "/filemanager.aspx?parent_page=vendor&id=" + vendobj.ID);
				#endregion
				#region Linecard
				div_linecard.InnerHtml = "";
				try
					{
					var dt = Toolbox.doSQL_dt(@"SELECT DISTINCT
urldecode(inventory_attribute_value.`value`)
FROM
inventory_attribute_value
INNER JOIN inventory_item_detail ON inventory_item_detail.attribute_value_id = inventory_attribute_value.attribute_value_id
LEFT JOIN inventory_price ON inventory_item_detail.master_id = inventory_price.master_id
INNER JOIN vendor ON inventory_price.vendor_id = vendor.Vendor_ID AND vendor.Vendor_ID = @v0 
where inventory_attribute_value.attribute_id = 16", new object[] { vendobj.ID.ToString()});
					foreach (DataRow dr in dt.Rows)
						{
						div_linecard.InnerHtml += dr[0] + "</br>";
						}
					}
				catch { }
				#endregion
				#region competitors
				div_competitors.InnerHtml = "";
				try
					{
					var dt = Toolbox.doSQL_dt(@"Select distinct vendor.Vendor_Name,vendor.Vendor_ID from inventory_price, vendor  where inventory_price.master_id in (Select distinct inventory_price.master_id from inventory_price,inventory_item_master,inventory_tag where inventory_price.vendor_id =@v0 and inventory_item_master.master_id=inventory_price.master_id and inventory_item_master.tag_id =inventory_tag.tag_id and inventory_tag.is_exclude=FALSE) and inventory_price.business_unit_id=@v1  and inventory_price.vendor_id = vendor.vendor_id and inventory_price.vendor_id!=@v2 ", new object[] { vendobj.ID.ToString(),current_user.business_unit_id.ToString(),vendobj.ID.ToString() });
					foreach (DataRow dr in dt.Rows)
						{
						div_competitors.InnerHtml += string.Format("<a href='./index.aspx?vendor_id={0}&first_tab=true'>{1}</a><br/>", dr[1], dr[0]);
						}
					}
				catch { }
				#endregion
				var notepadflag = "";
				try
					{
					notepadflag = BVDB.getSQL_string(bv_conn,@"Select ifnull((SELECT NOTEPAD FROM VENDOR WHERE VEN_NO = ? ),'X')", new object[] {  vendobj.Number } );
					}
				catch { }
				if (notepadflag == "X")
					{
					t_past_memo.InnerHtml = "";
					var NotePad = BVDB.getSQL_dt(bv_conn,@"SELECT N_DATE, DETAIL, N_TIME FROM NOTES WHERE PROG='SUPP' AND ITEM=?  ORDER BY N_DATE DESC, N_TIME DESC", new object[] {  vendobj.Number } );
					foreach (DataRow NoteRow in NotePad.Rows)
						{
						var strDate = NoteRow["N_DATE"].ToString();
						strDate = strDate.Insert(4, "-");
						strDate = strDate.Insert(7, "-");
						var strTime = NoteRow["N_TIME"].ToString();
						var timecount = strTime.Length;
						if (timecount == 8)
							{
							strTime = strTime.Substring(0, 4);
							strTime = strTime.Insert(2, ":");
							}
						else
							{
							strTime = strTime.Substring(0, 3);
							strTime = strTime.Insert(1, ":");
							}
						var n = NoteRow["DETAIL"].ToString().Replace("\n", "<br/>");
						t_past_memo.InnerHtml += string.Format(@"
<div style='border:solid 1px #ccc;margin:5px;width:350px;padding:10px;border-radius:5px;background-color:#ff9;min-height:100px;'>
<b>DATE: </b>{0}<br/>
<b>TIME: </b>{2}<br/><br/>
<div style='width:340px;'>{1}</div>
</div>
<hr/>
", strDate,
							n,
							strTime
							);
						}
					}
				fill_contacts();
				#endregion
				#region specialities
				ViewState["vendor_id"] = vendobj.ID;
				partner_skills1.init();
				#endregion
				}
			}
		}
	protected void cbp_contacts_Callback(object sender, CallbackEventArgsBase e)
		{
		var contact_n = Convert.ToInt32(lb_contacts.SelectedItem.Value);
		BVContact c;
		if (e.Parameter == "save")
			{
			c = new BVContact(vendobj.Address.id, contact_n);
			c.Address_ID = vendobj.Address.id;
			c.Name = t_contact_name.Text;
			c.Phone_Area = t_contact_phone_area.Text;
			c.Phone_First = t_contact_phone_prefix.Text;
			c.Phone_Last = t_contact_phone_suffix.Text;
			c.Phone_Ext = t_contact_phone_ext.Text;
			c.Fax_Area = t_contact_fax_area.Text;
			c.Fax_First = t_contact_fax_prefix.Text;
			c.Fax_Last = t_contact_fax_suffix.Text;
			c.Email = t_contact_email.Text;
			c.Save();
			}
		else if (e.Parameter == "bv_con_change")
			{
			c = new BVContact(vendobj.Address.id, contact_n, true);
			t_contact_name.Text = c.Name;
			t_contact_phone_area.Text = c.Phone_Area;
			t_contact_phone_prefix.Text = c.Phone_First;
			t_contact_phone_suffix.Text = c.Phone_Last;
			t_contact_phone_ext.Text = c.Phone_Ext;
			t_contact_fax_area.Text = c.Fax_Area;
			t_contact_fax_prefix.Text = c.Fax_First;
			t_contact_fax_suffix.Text = c.Fax_Last;
			t_contact_email.Text = c.Email;
			}
		}
	[WebMethod]
	public static bool chk_name(string n, string _id)
		{
		return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM vendor  WHERE vendor_name = @v0 AND vendor_id !=@v1 ", new object[] { n.Trim(),_id }) > 0;
		}
	protected void cbp_main_Callback(object sender, CallbackEventArgsBase e)
		{
		process_save = e.Parameter == "save";
		var phone_check = e.Parameter == "phone_check";
		var phone_refresh = e.Parameter == "phone_refresh";
		var name_check = e.Parameter == "name_check";
		var search = e.Parameter == "search";
		if (search)
			{
			if (vend_search_t.Text != "")
				{
				gv_vendor.FilterExpression = string.Format(@"
[vendor_name] like '%{0}%' or 
[vendor_address] like '%{0}%' or 
[vendor_number] like '%{0}%' or 
[vendor_id] like '%{0}%' or 
[phone] like '%{0}%'
", Toolbox.AddSlashes(vend_search_t.Text));
				pc_main.ActiveTabPage = pc_main.TabPages[0];
				}
			}
		else if (process_save)
			{
			var v_before = new NEVendor(vendobj.ID);
			if (vendobj.ts.Year > 2000 && vendobj.ts.Ticks.ToString() != hid_ts.Value)
				{
				//throw new Exception("This vendor's information has changed since it was last loaded. Please refresh the page or reload this vendor");
				}
			vendobj.Name = t_name.Text;
			vendobj.Hold = ck_onhold.Checked;
			vendobj.is_partner = ck_partner.Checked;
			vendobj.business_unit_id = Convert.ToInt32(cb_main_branch.Value);
			vendobj.Term_ID = cb_terms.SelectedIndex > -1 ? Convert.ToInt32(cb_terms.Value) : 0;
			vendobj.PO_Exempt = ck_poexempt.Checked;
			//vendobj.GL_ID = ddl_payable_gl.Value == null ? 31 : Convert.ToInt32(ddl_payable_gl.Value);
			//vendobj.GL_ID_Consol = ddl_payable_gl_consol.Value == null ? 773 : Convert.ToInt32(ddl_payable_gl_consol.Value);
			vendobj.CPRS = ck_cprs.Checked;
			vendobj.IDType = cb_id_type.SelectedIndex == -1 ? "B" : cb_id_type.Value.ToString();
			vendobj.IDNumber = t_id_number.Text;
			vendobj.Credit_Type = Convert.ToInt32(cb_credit_type.Value);
			vendobj.Credit_Limit = Convert.ToDouble(t_credit_limit.Text);
			vendobj.Account = t_account_number.Text;
			vendobj.Buyer = t_buyer_name.Text;
			vendobj.Notes = t_notes.Text;
			try
				{
				if (vendobj.id != 0)
					{
					vendobj.Save();
					vendobj = new NEVendor(vendobj.Vendor_ID);
					hid_ts.Value = vendobj.ts.Ticks.ToString();
					}
				else
					{
					vendobj = new NEVendor(vendobj.Vendor_ID);
					hid_ts.Value = vendobj.ts.Ticks.ToString();
					throw new Exception("Possible duplicate");
					}
				}
			catch (Exception ee)
				{
				vendobj = new NEVendor(vendobj.Vendor_ID);
				hid_ts.Value = vendobj.ts.Ticks.ToString();
				throw;
				}
			vendobj.Address.Type = "B";
			vendobj.Address.Desc = "";
			vendobj.Address.Table = "Vendor";
			vendobj.Address.Table_ID = vendobj.id;
			vendobj.Address.Addr1 = t_address1.Text.Replace("'", "-");
			vendobj.Address.Addr2 = t_address2.Text.Replace("'", "-");
			vendobj.Address.Addr3 = t_address3.Text.Replace("'", "-");
			vendobj.Address.Addr4 = t_address4.Text.Replace("'", "-");
			vendobj.Address.City = t_city.Text.Replace("'", "-");
			vendobj.Address.Postal = t_postal.Text;
			vendobj.Address.Prov = Toolbox.ReturnBlankIfNull_string(cb_provstate.Value);
			vendobj.Address.Country = Toolbox.ReturnBlankIfNull_string(cb_country.Value);
			vendobj.Address.PhoneArea = t_phone_area.Text == "NA" ? "000" : t_phone_area.Text;
			vendobj.Address.Phonefirst = t_phone_prefix.Text == "" ? "000" : t_phone_prefix.Text;
			vendobj.Address.PhoneLast = t_phone_suffix.Text == "" ? "0000" : t_phone_suffix.Text;
			vendobj.Address.PhoneExt = t_phone_area.Text;
			vendobj.Address.FaxArea = t_fax_area.Text == "NA" ? "000" : t_fax_area.Text;
			vendobj.Address.FaxFirst = t_fax_prefix.Text == "" ? "000" : t_fax_prefix.Text;
			vendobj.Address.FaxLast = t_fax_suffix.Text == "" ? "0000" : t_fax_suffix.Text;
			vendobj.Address.Email = t_email.Text;
			vendobj.Address.Web = t_web.Text;
            vendobj.Address.Tax1 = Toolbox.ReturnZeroIfNull_int(ddl_tax1.Value);
            vendobj.Address.Tax2 = Toolbox.ReturnZeroIfNull_int(ddl_tax2.Value);
            vendobj.Address.Tax3 = Toolbox.ReturnZeroIfNull_int(ddl_tax3.Value);
            vendobj.Address.Tax4 = Toolbox.ReturnZeroIfNull_int(ddl_tax4.Value);

            try
				{
				vendobj.Address.Save();
				var before = Toolbox.dict_create(v_before);
				var after = Toolbox.dict_create(vendobj);
				var diff_from = before.Except(after).ToDictionary(k => k.Key, v => v.Value);
				var diff_to = after.Except(before).ToDictionary(k => k.Key, v => v.Value);
				var changes = "Vendor Info<br/>" + Toolbox.dict_dump(diff_from) + "<b>Changed to</b> <br/>" + Toolbox.dict_dump(diff_to);
				var before_addy = Toolbox.dict_create(v_before.Address);
				var after_addy = Toolbox.dict_create(vendobj.Address);
				var diff_addy_from = before_addy.Except(after_addy).ToDictionary(k => k.Key, v => v.Value);
				var diff_addy_to = after_addy.Except(before_addy).ToDictionary(k => k.Key, v => v.Value);
				changes += "<hr/>Address Info<br/>" + Toolbox.dict_dump(diff_addy_from) + "<b>Changed to</b> <br/>" + Toolbox.dict_dump(diff_addy_to);
				var mail = new NeEMail
								{
								To = "mjrodriguez@newelectric.com",
								Subject = "Vendor information updated",
								From = current_user.NEEmail,
								isHTML = true,
								Body = string.Format("{0} has updated vendor {1}'s information, here is a list of the changes<br/><hr/>{2}", current_user.FullName, v_before.vendor_number, changes)
								};
				mail.Send();
				}
			catch (Exception ee)
				{
				vendobj = new NEVendor(vendobj.Vendor_ID);
				hid_ts.Value = vendobj.ts.Ticks.ToString();
				throw;
				}
			vendobj = new NEVendor(vendobj.Vendor_ID);
			hid_ts.Value = vendobj.ts.Ticks.ToString();
			try
				{
				// Doing these update statements only until we have NESI-US worked out.
				//Toolbox.doSQL_void(@"UPDATE company SET active = 'F'  WHERE business_unit_id = 48" );
				vendobj.sync_bvs(current_user);
				//Toolbox.doSQL_void(@"UPDATE company SET active = 'T'  WHERE business_unit_id = 48" );
			}
			catch (Exception ee)
				{
				vendobj = new NEVendor(vendobj.Vendor_ID);
				hid_ts.Value = vendobj.ts.Ticks.ToString();
				//Toolbox.doSQL_void(@"UPDATE company SET active = 'T'  WHERE business_unit_id = 48" );
				throw ee;
				}
			fill_main();
			b_save_vendor.ClientEnabled = true;
			ScriptManager.RegisterClientScriptBlock(sender as Control, this.GetType(), "remove", "alert('Vendor Info Saved')", true);
			}
		else if (phone_check)
			{
			t_error.Text = "";
			var error_text = "";
			if (t_phone_area.Text != "" && t_phone_prefix.Text != "" && t_phone_suffix.Text != "")
				{
				var numbers = Toolbox.doSQL_dt(@"SELECT b.vendor_id, b.vendor_name, b.vendor_number FROM address a LEFT JOIN vendor b ON b.vendor_id = a.address_table_id  WHERE address_table = 'Vendor' AND a.address_phonearea =@v0 AND a.address_phonefirst =@v1  AND a.address_phonelast =@v2  AND b.vendor_id !=@v3  ORDER BY CAST(b.vendor_number AS UNSIGNED)", new object[] { t_phone_area.Text,t_phone_prefix.Text,t_phone_suffix.Text,current_vendor_id });
				if (numbers.Rows.Count > 0)
					{
					error_text += "<br/>* Phone Number Exists";
					foreach (DataRow ve in numbers.Rows)
						{
						var _id = ve["vendor_id"].ToString();
						var _number = ve["vendor_number"].ToString();
						var _name = ve["vendor_name"].ToString();
						error_text += string.Format("<br/><a href='./index.aspx?vendor_id={0}&first_tab=true'>({1}) - {2}</a>", _id, _number, _name);
						}
					}
				}
			t_error.Text += error_text;
			l_vendor_id.Text = vendobj.id.ToString();
			l_vendor_number.Text = vendobj.Number;
			l_qc_date.Text = vendobj.QC_DateTime.ToString("MM/dd/yyyy hh:mm:tt");
			l_date_added.Text = vendobj.Vendor_CreatedDateTime.ToString("MM/dd/yyyy hh:mm:tt");
			l_added_by.Text = new NeMember(Convert.ToInt32(vendobj.InitMember_ID)).FullName;
			}
		else if (name_check)
			{
			t_error.Text = "";
			var error_text = "";
			if (t_name.Text.Trim() != "" && t_name.Text.Trim().Length > 3)
				{
				var names = Toolbox.doSQL_dt(@"SELECT * FROM vendor WHERE vendor_name LIKE  CONCAT('%',@v0,'%')  AND vendor_id != @v1  ORDER BY CAST(vendor_number AS UNSIGNED)", new object[] {  t_name.Text.Trim(), current_vendor_id } );
				if (names.Rows.Count > 0)
					{
					error_text = "* Possible Duplicate Name Exists";
					foreach (DataRow ve in names.Rows)
						{
						var _id = ve["vendor_id"].ToString();
						var _number = ve["vendor_number"].ToString();
						var _name = ve["vendor_name"].ToString();
						var exact = t_name.Text.Trim() == _name.Trim() ? " <b style='color:red'>[EXACT]</b>" : "";
						if (b_save_vendor.Enabled)
							{
							b_save_vendor.Enabled = t_name.Text.Trim() != _name.Trim();
							}
						error_text += string.Format("<br/><a href='./index.aspx?vendor_id={0}&first_tab=true'>({1}) - {2}</a>{3}", _id, _number, _name, exact);
						}
					}
				}
			t_error.Text += error_text;
			l_vendor_id.Text = vendobj.id.ToString();
			l_vendor_number.Text = vendobj.Number;
			l_qc_date.Text = vendobj.QC_DateTime.ToString("MM/dd/yyyy hh:mm:tt");
			l_date_added.Text = vendobj.Vendor_CreatedDateTime.ToString("MM/dd/yyyy hh:mm:tt");
			l_added_by.Text = new NeMember(Convert.ToInt32(vendobj.InitMember_ID)).FullName;
			}
		else if (phone_refresh)
			{
			t_phone_area.Text = vendobj.Address.PhoneArea;
			t_phone_prefix.Text = vendobj.Address.Phonefirst;
			t_phone_suffix.Text = vendobj.Address.PhoneLast;
			t_phone_ext.Text = vendobj.Address.PhoneExt;
			t_fax_area.Text = vendobj.Address.FaxArea;
			t_fax_prefix.Text = vendobj.Address.FaxFirst;
			t_fax_suffix.Text = vendobj.Address.FaxLast;
			}
		else
			{
			pc_main.TabPages[1].ClientEnabled = true;
			pc_main.ActiveTabPage = pc_main.TabPages[1];
			pc_detail.ActiveTabPage = pc_detail.TabPages[0];
			fill_main();
			}
		}
	protected void cbp_new_Callback(object sender, CallbackEventArgsBase e)
		{
		var start_vendor = e.Parameter == "start_vendor";
		var phone_check = e.Parameter == "phone_check";
		var name_check = e.Parameter == "name_check";
		#region Dupe Check
		var dupe_name = Toolbox.doSQL_int(@"SELECT COUNT(vendor_id) FROM vendor WHERE vendor_name = @v0 ", new object[] {  t_new_name.Text } ) > 0;
		var dupe_addr = Toolbox.doSQL_int(@"SELECT COUNT(address_id) FROM address WHERE address_addr1 =@v0", new object[] { t_new_address_1.Text.Replace("'", "-") } ) > 0;
		if (dupe_name && dupe_addr)
			{
			var v_number = Toolbox.doSQL_string(@"SELECT vendor_number FROM vendor WHERE vendor_name = @v0  LIMIT 1", new object[] {  t_new_name.Text } );
			throw new Exception("This vendor already exists on vendor #" + v_number);
			}
		#endregion Dupe Check
		if (start_vendor)
			{
			vendobj = new NEVendor();
			vendobj.Name = t_new_name.Text;
			vendobj.Hold = false;
			vendobj.business_unit_id = current_user.business_unit_id;
			vendobj.Term_ID = Convert.ToInt32(cb_new_terms.SelectedItem.Value);
			vendobj.PO_Exempt = false;
		
			vendobj.CPRS = false;
			vendobj.InitMember_ID = current_user.id32;
			vendobj.IDType = "B";
			vendobj.IDNumber = "";
			vendobj.Credit_Type = 1;
			vendobj.Credit_Limit = 0;
			vendobj.Account = "";
			vendobj.Buyer = "";
			vendobj.Notes = "";
			try
				{
				var returned_id = vendobj.Save();
				vendobj.Number = returned_id.ToString();
				vendobj.Number_Int = Convert.ToInt32(returned_id);
				vendobj.id = returned_id;
				Toolbox.doSQL_void(@"UPDATE vendor SET vendor_number = vendor_id, vendor_number_int = vendor_id  WHERE vendor_id =@v0", new object[] { returned_id });
				vendobj.Address.Table_ID = returned_id;
				vendobj.Address.Table = "Vendor";
				vendobj.Address.Type = "B";
				vendobj.Address.Desc = "";
				vendobj.Address.Addr1 = t_new_address_1.Text.Replace("'", "-");
				vendobj.Address.Addr2 = t_new_address_2.Text.Replace("'", "-");
				vendobj.Address.Addr3 = t_new_address_3.Text.Replace("'", "-");
				vendobj.Address.Addr4 = t_new_address_4.Text.Replace("'", "-");
				vendobj.Address.City = t_new_city.Text.Replace("'", "-");
				vendobj.Address.Postal = t_new_postal.Text;
				vendobj.Address.Prov = cb_new_provstate.SelectedItem.Value.ToString();
				vendobj.Address.Country = cb_new_country.SelectedItem.Value.ToString();
				vendobj.Address.PhoneArea = t_new_phone_area.Text;
				vendobj.Address.Phonefirst = t_new_phone_prefix.Text;
				vendobj.Address.PhoneLast = t_new_phone_suffix.Text;
				vendobj.Address.PhoneExt = t_new_phone_ext.Text;
				vendobj.Address.FaxArea = t_new_fax_area.Text;
				vendobj.Address.FaxFirst = t_new_fax_prefix.Text;
				vendobj.Address.FaxLast = t_new_fax_suffix.Text;
				vendobj.Address.Email = t_new_email.Text;
				vendobj.Address.Web = t_new_website.Text;
				vendobj.Address.RVAccount = "50110";
				vendobj.Address.RVAccount_Consol = "51000";
				vendobj.Address.Save();

              // var NSI = new NetSuite_Integration.Vendor();
              //  NSI.SyncNetSuite(vendobj, current_user.business_unit.tax_entity_id);
                vendobj.sync_bvs(current_user);

                ASPxWebControl.RedirectOnCallback("./index.aspx?vendor_id=" + returned_id + "&first_tab=true");
				}
			catch (Exception ee)
				{
				vendobj = new NEVendor(vendobj.id);
				hid_ts.Value = vendobj.ts.Ticks.ToString();
			
				throw ee;
				}
			}
		else
			{
			var error_text = "";
			t_new_error.Text = "";
			if (t_new_name.Text.Trim() != "" && t_new_name.Text.Trim().Length > 3)
				{
				var names = Toolbox.doSQL_dt(@"SELECT * FROM vendor  WHERE vendor_name LIKE  CONCAT('%',@v0,'%')  ORDER BY CAST(vendor_number AS UNSIGNED)", new object[] { t_new_name.Text.Trim() });
				if (names.Rows.Count > 0)
					{
					error_text = "* Possible Duplicate Name Exists";
					foreach (DataRow v in names.Rows)
						{
						var _id = v["vendor_id"].ToString();
						var _number = v["vendor_number"].ToString();
						var _name = v["vendor_name"].ToString().Trim();
						var exact = t_new_name.Text.Trim() == _name.Trim() ? " <b style='color:red'>[EXACT]</b>" : "";
						if (b_new_save.Enabled)
							{
							b_new_save.Enabled = t_new_name.Text.Trim() == _name.Trim();
							}
						error_text += string.Format("<br/><a href='./index.aspx?vendor_id={0}&first_tab=true'>({1}) - {2}</a> {3}", _id, _number, _name, exact);
						}
					}
				}
			if (t_new_phone_area.Text != "" && t_new_phone_prefix.Text != "" && t_new_phone_suffix.Text != "")
				{
				var numbers = Toolbox.doSQL_dt(@"SELECT b.vendor_id, b.vendor_name, b.vendor_number FROM address a LEFT JOIN vendor b ON b.vendor_id = a.address_table_id  WHERE address_table = 'Vendor' AND a.address_phonearea =@v0 AND a.address_phonefirst =@v1  AND a.address_phonelast =@v2  ORDER BY CAST(b.vendor_number AS UNSIGNED)", new object[] { t_new_phone_area.Text,t_new_phone_prefix.Text,t_new_phone_suffix.Text });
				if (numbers.Rows.Count > 0)
					{
					error_text += "<br/>* Phone Number Exists";
					foreach (DataRow v in numbers.Rows)
						{
						var _id = v["vendor_id"].ToString();
						var _number = v["vendor_number"].ToString();
						var _name = v["vendor_name"].ToString();
						error_text += string.Format("<br/><a href='./index.aspx?vendor_id={0}&first_tab=true'>({1}) - {2}</a>", _id, _number, _name);
						}
					}
				}
			t_new_error.Text = error_text;
			if (name_check)
				{
				t_new_address_1.Focus();
				}
			if (phone_check)
				{
				t_new_phone_ext.Focus();
				}
			}
		}
	protected void savenote(object sender, EventArgs e)
		{
		var notesadd = new bv_notes();
		var nowdate = new DateTime();
		nowdate = DateTime.Now;
		var bvdate = nowdate.ToString("yyyyMMdd");
		var bvtime = nowdate.ToString("HHmmssFF");
		notesadd.prog = "SUPP";
		notesadd.item = hidvendno.Value;
		notesadd.subject = hidvendno.Value;
		notesadd.n_date = bvdate;
		notesadd.n_time = Convert.ToInt32(bvtime);
		notesadd.n_user = current_user.Initials;
		notesadd.detail = t_memo.Text.Replace("\"", "").Replace("'", "");
		var error = "";
		if (notesadd.detail.Trim() != "")
			{
			var DSNS = Toolbox.doSQL_dt(@"SELECT distinct t.dsn FROM tax_entity t inner join business_unit b on t.id=b.tax_entity_id  WHERE t.is_active = 1 AND t.is_test = 0", null);
			foreach (DataRow dsnrow in DSNS.Rows)
				{
				try
					{
					var dsn = dsnrow[0].ToString();
					notesadd.Save(dsn);
					}
				catch (Exception ee)
					{
					error += ee.Message + "<br/>";
					}
				//notesadd.Save("BV7DEBUG");
				}
			}
		else
			{
			error = "Note not defined";
			}
		if (error != "")
			{
			memo_error.Text = error;
			}
		else
			{
			memo_error.Text = "";
			}
		t_memo.Text = "";
		btn_memo.Enabled = true;
		fill_main();
		}
	protected void fill_emails()
		{
		var gv = gv_emails;
		if (Session["vendor_emails"] == null)
			{
			if (Session["current_vendor_id"] != null && Session["current_vendor_id"].ToString() != "0")
				{
				// build supervisor email list
				var sup_emails = Toolbox.doSQL_dt(@"SELECT member.member_neemail FROM member  WHERE is_us_boardmember=1 or is_can_boardmember=1" , null);
				var sql = "SELECT contact_email FROM contact WHERE contact_type = 'vendor' AND contact_cust_id = '" + Session["current_vendor_id"] + "' AND contact_status = 'active' AND contact_email NOT IN ('', 'needed', '@') AND contact_email LIKE '%@%.%'";
				var _emails = Toolbox.doSQL_dt(@"SELECT contact_email FROM contact  WHERE contact_type = 'vendor' AND contact_cust_id =@v0 AND contact_status = 'active' AND contact_email NOT IN ('', 'needed', '@') AND contact_email LIKE '%@%.%'", new object[] { Session["current_vendor_id"] });
				if (_emails.Rows.Count > 0)
					{
					var emails = new List<string>();
					var appendage = new List<string>();
					foreach (DataRow _address in _emails.Rows)
						{
						var email = "'\"" + _address["contact_email"] + "\"'";
						if (!emails.Contains(email))
							{
							emails.Add(email);
							}
						}
					if (emails.Count == 0)
						{
						emails.Add("'_#_#_#_#_'");
						}
					    sql = @"
SELECT 
	emaillog_timestamp `date`,
	emaillog_from `from_address`,
	emaillog_to `to_address`,
	emaillog_subject subject,
	emaillog_id
FROM 
	emaillog 
WHERE 
	MATCH(emaillog_to, emaillog_from) AGAINST (" + string.Join(" ", emails.ToArray()) + @" IN BOOLEAN MODE)
ORDER BY Date DESC limit 1000";
					    var dt = Toolbox.doSQL_dt(sql, null);
                    foreach (DataRow dr in dt.Rows)
						{
						foreach (DataRow dr_supers in sup_emails.Rows)
							{
							if (dr["from_address"].ToString().Contains(dr_supers[0].ToString().TrimEnd(';').TrimEnd(',')) ||
								dr["to_address"].ToString().Contains(dr_supers[0].ToString().TrimEnd(';').TrimEnd(',')))
								{
								dr.BeginEdit();
								dr["subject"] = "Private";
								dr.AcceptChanges();
								dr.EndEdit();
								}
							}
						}
					Session["vendor_emails"] = dt;
					}
				}
			}
		gv.DataSourceID = "";
		gv.DataSource = Session["vendor_emails"];
		gv.DataBind();
		}
	protected void gv_phonelog_Init(object sender, EventArgs e)
		{
		var gv = (ASPxGridView)sender;
		if (Session["current_vendor_id"] != null && Session["current_vendor_id"].ToString() != "0")
			{
			gv.DataSourceID = "";
			var _dt = Toolbox.doSQL_dt(@"CALL vendor_phone_log_copy(@v0)",new object[] { Session["current_vendor_id"] } );
			if (_dt.Rows.Count == 0)
				{
				}
			gv.DataSource = _dt;
			gv.DataBind();
			}
		}
	protected void hl_wo_Init(object sender, EventArgs e)
		{
		var container = ((ASPxHyperLink)sender).NamingContainer as GridViewDataItemTemplateContainer;
		var gv_r = gv_pos.GetDataRow(container.VisibleIndex);
		if (gv_r != null)
			{
			var this_bvpo = gv_r["po_number"].ToString();
			var this_poprog_id = gv_r["poprog_id"].ToString();
			((ASPxHyperLink)sender).Text = this_poprog_id == "" ? string.Empty : this_bvpo;
			((ASPxHyperLink)sender).ClientSideEvents.Click = string.Format("function(s,e){{boing('/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}', 'po', 1035, 800);}}", this_poprog_id);
			}
		}
	protected void fill_gvpos()
		{
		if (Session["current_vendor_id"] != null && Session["current_vendor_id"].ToString() != "0")
			{
			gv_pos.DataSourceID = "";
			var _dt = Toolbox.doSQL_dt(@" SELECT a.poprog_id, DATE_FORMAT(a.poprog_cutdate, '%Y-%m-%d') cut_dt, DATE_FORMAT(a.poprog_closedDate, '%Y-%m-%d') close_dt, a.poprog_bvpo po_number, urldecode(a.poprog_order_description) po_description, b.status_type status, a.business_unit_id, c.name branch FROM poprog_header a LEFT JOIN poprog_status b ON a.poprog_status = b.poprog_status_id LEFT join business_unit c ON a.business_unit_id = c.id  WHERE a.poprog_vendor_id =@v0 AND a.poprog_bvpo != '' ORDER BY a.poprog_bvpo DESC", new object[] { Session["current_vendor_id"] });
			if (_dt.Rows.Count != 0 && gv_pos.FilterExpression == "")
				{
				gv_pos.FilterExpression = "[branch] = '" + current_user.business_unit.name + "'";
				}
			gv_pos.DataSource = _dt;
			gv_pos.DataBind();
			}
		}
	protected void fill_contacts()
		{
		gv_contacts.DataSource = Toolbox.doSQL_dt(@"SELECT contact_name, contact_id,contact_Cellphone,contact_directline, contact_extension, contact_email, contact_password, contact_status as contact_status, contact_login_enabled as login_enable, customer_or_contact_status_id as status FROM contact,customer_or_contact_status WHERE contact_cust_id = @v0  AND contact_type='Vendor' and customer_or_contact_status_id=contact_status_id", new object[] {  persist["current_vendor_id"] } );
		gv_contacts.DataBind();
		}
	protected void gv_contacts_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var contact = new NEContact(Convert.ToInt32(e.Keys["contact_id"]));
		if (e.NewValues["contact_name"] == null)
			{
			throw new Exception("Contact Name Cannont Be Blank");
			}
		else if (e.NewValues["contact_name"].ToString() == "")
			{
			throw new Exception("Contact Name Cannont Be Blank");
			}
		if (e.NewValues["login_enable"].ToString() == "1")
			{
			if (e.NewValues["contact_password"] == null)
				{
				throw new Exception("If NESI login is Active, the password can't be BLANK");
				}
			else if (e.NewValues["contact_password"].ToString() == "")
				{
				throw new Exception("If NESI login is Active, the password can't be BLANK");
				}
			if (e.NewValues["contact_email"] == null)
				{
				throw new Exception("If NESI login is Active, the email can't be BLANK");
				}
			else if (e.NewValues["contact_email"].ToString() == "")
				{
				throw new Exception("If NESI login is Active, the email can't be BLANK");
				}
			contact.Contact_Password = e.NewValues["contact_password"].ToString();
			}
		string c = null;
		if (e.NewValues["contact_Cellphone"] != null)  // cell phone format validation
			{
			c = e.NewValues["contact_Cellphone"].ToString();
			c = c.Replace("(", "").Replace(")", "").Replace("-", "");
			if (c.Substring(0, 1) == "1")
				{
				c = c.Remove(0, 1);
				}
			if (c.Length != 10)
				{
				throw new Exception("Cell number must be in US standard format 999-999-9999.");
				}
			try
				{
				var x = Convert.ToInt64(c.Replace("(", "").Replace(")", "").Replace("-", ""));
				}
			catch
				{
				throw new Exception("Cell number must contain only numbers, no letters or characters");
				}
			c = c.Substring(0, 3) + "-" + c.Substring(3, 3) + "-" + c.Substring(6, 4);
			}
		string d = null;
		if (e.NewValues["contact_directline"] != null)  // cell phone format validation
			{
			d = e.NewValues["contact_directline"].ToString();
			d = d.Replace("(", "").Replace(")", "").Replace("-", "");
			if (d.Substring(0, 1) == "1")
				{
				d = d.Remove(0, 1);
				}
			if (d.Length != 10)
				{
				throw new Exception("Direct number must be in US standard format 999-999-9999.");
				}
			try
				{
				var x = Convert.ToInt64(d.Replace("(", "").Replace(")", "").Replace("-", ""));
				}
			catch
				{
				throw new Exception("Direct number must contain only numbers, no letters or characters");
				}
			d = d.Substring(0, 3) + "-" + d.Substring(3, 3) + "-" + d.Substring(6, 4);
			}
		contact.Contact_Name = e.NewValues["contact_name"].ToString();
		contact.Contact_Status_ID = Convert.ToInt32(e.NewValues["status"]);
		contact.Contact_Status = e.NewValues["contact_status"].ToString();
		contact.Contact_Login_enabled = Convert.ToInt16(e.NewValues["login_enable"]);
		contact.Contact_Extension = e.NewValues["contact_extension"] == null ? "" : e.NewValues["contact_extension"].ToString();
		contact.Contact_CellPhone = c == null ? "" : c;
		contact.Contact_DirectLine = d == null ? "" : d;
		contact.Contact_Email = e.NewValues["contact_email"] == null ? "" : e.NewValues["contact_email"].ToString();
		contact.Save(contact);
		// var NSI = new NetSuite_Integration.Vendor();
		// NSI.SyncNetSuite(vendobj, current_user.business_unit.tax_entity_id);
		e.Cancel = true;
		gv_contacts.CancelEdit();
		fill_contacts();
		}
	protected void gv_contacts_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var x = 0;
		x = Toolbox.doSQL_int(@"Select count(woprog_id) from woprog  where woprog_contact_id =@v0", new object[] { e.Values["contact_id"] });
		if (x > 0)
			{
			throw new Exception("You can't delete this contact, it's found on " + x + " work order(s). You need to merge it.");
			}
		x = 0;
		x = Toolbox.doSQL_int(@"Select count(quote_id) from quote_master  where contact_id =@v0", new object[] { e.Values["contact_id"] });
		if (x > 0)
			{
			throw new Exception("You can't delete this contact, it's found on " + x + " quote(s).  you need to merge it.");
			}
		try
			{
			Toolbox.doSQL_void(@"Delete from contact  where contact_id =@v0 limit 1 ", new object[] { e.Values["contact_id"] });
			}
		catch
			{
			throw new Exception("This thing crashed during the deletion of this contact!!");
			}
		e.Cancel = true;
		gv_contacts.CancelEdit();
		fill_contacts();
		}
	protected void gv_contacts_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
		var contact = new NEContact();
		if (e.NewValues["contact_name"] == null)
			{
			throw new Exception("Contact Name Cannont Be Blank");
			}
		else if (e.NewValues["contact_name"].ToString() == "")
			{
			throw new Exception("Contact Name Cannont Be Blank");
			}
		if (e.NewValues["login_enable"].ToString() == "1")
			{
			if (e.NewValues["contact_password"] == null)
				{
				throw new Exception("If NESI login is Active, the password can't be BLANK");
				}
			else if (e.NewValues["contact_password"].ToString() == "")
				{
				throw new Exception("If NESI login is Active, the password can't be BLANK");
				}
			if (e.NewValues["contact_email"] == null)
				{
				throw new Exception("If NESI login is Active, the email can't be BLANK");
				}
			else if (e.NewValues["contact_email"].ToString() == "")
				{
				throw new Exception("If NESI login is Active, the email can't be BLANK");
				}
			contact.Contact_Password = e.NewValues["contact_password"].ToString();
			}
		var x = 0;
		x = Toolbox.doSQL_int(@"Select count(contact_id) from contact  where contact_name like  CONCAT('%',@v0,'%')  and contact_cust_id=@v1 ", new object[] { e.NewValues["contact_name"],persist["current_vendor_id"] });
		if (x > 0)
			{
			throw new Exception("Sorry a contact with this name already exists for this vendor.");
			}
		if (e.NewValues["contact_email"] != null)
			{
			x = Toolbox.doSQL_int(@"Select count(contact_id) from contact  where contact_email like  CONCAT('%',@v0,'%')  and contact_cust_id=@v1 ", new object[] { e.NewValues["contact_email"],persist["current_vendor_id"] });
			if (x > 0)
				{
				throw new Exception("Sorry a contact with this email already exists for this vendor.");
				}
			}
		if (e.NewValues["contact_Cellphone"] != null)
			{
			x = Toolbox.doSQL_int(@"Select count(contact_id) from contact  where replace(replace(replace(REPLACE(contact_cellphone,'-',''),'(',''),')',''),' ','') like  CONCAT('%',@v0,'%')  and contact_cust_id=@v1 ", new object[] { e.NewValues["contact_Cellphone"].ToString().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim(),persist["current_vendor_id"] });
			if (x > 0)
				{
				throw new Exception("Sorry a contact with this cell phone already exists for this vendor.");
				}
			}
		if (e.NewValues["contact_directline"] != null)
			{
			x = Toolbox.doSQL_int(@"Select count(contact_id) from contact  where replace(replace(replace(REPLACE(contact_directline,'-',''),'(',''),')',''),' ','') like  CONCAT('%',@v0,'%')  and contact_cust_id=@v1 ", new object[] { e.NewValues["contact_directline"].ToString().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim(),persist["current_vendor_id"] });
			if (x > 0)
				{
				throw new Exception("Sorry a contact with this direct line already exists for this vendor.");
				}
			}
		    string c = null;
        if (e.NewValues["contact_Cellphone"] != null)  // cell phone format validation
		        {
		        c = e.NewValues["contact_Cellphone"].ToString();
		        c = c.Replace("(", "").Replace(")", "").Replace("-", "");
		        if (c.Substring(0, 1) == "1")
		            {
		            c = c.Remove(0, 1);
		            }
		        if (c.Length != 10)
		            {
		            throw new Exception("Cell number must be in US standard format 999-999-9999.");
		            }
		        try
		            {
		            var xx = Convert.ToInt64(c.Replace("(", "").Replace(")", "").Replace("-", ""));
		            }
		        catch
		            {
		            throw new Exception("Cell number must contain only numbers, no letters or characters");
		            }
		        c = c.Substring(0, 3) + "-" + c.Substring(3, 3) + "-" + c.Substring(6, 4);
		        }
		    string d = null;
		    if (e.NewValues["contact_directline"] != null)  // cell phone format validation
		        {
		        d = e.NewValues["contact_directline"].ToString();
		        d = d.Replace("(", "").Replace(")", "").Replace("-", "");
		        if (d.Substring(0, 1) == "1")
		            {
		            d = d.Remove(0, 1);
		            }
		        if (d.Length != 10)
		            {
		            throw new Exception("Direct number must be in US standard format 999-999-9999.");
		            }
		        try
		            {
		            var xx = Convert.ToInt64(d.Replace("(", "").Replace(")", "").Replace("-", ""));
		            }
		        catch
		            {
		            throw new Exception("Direct number must contain only numbers, no letters or characters");
		            }
		        d = d.Substring(0, 3) + "-" + d.Substring(3, 3) + "-" + d.Substring(6, 4);
		        }

        contact.Contact_Name = e.NewValues["contact_name"].ToString();
		contact.Contact_Status = e.NewValues["contact_status"].ToString();
		contact.Contact_Login_enabled = Convert.ToInt16(e.NewValues["login_enable"]);
		contact.Contact_Status_ID = Convert.ToInt32(e.NewValues["status"]);
		contact.Contact_Extension = e.NewValues["contact_extension"] == null ? "" : e.NewValues["contact_extension"].ToString();
		    contact.Contact_CellPhone = c == null ? "" : c;
		    contact.Contact_DirectLine = d == null ? "" : d;
        contact.Contact_Email = e.NewValues["contact_email"] == null ? "" : e.NewValues["contact_email"].ToString();
		contact.Contact_Type = "Vendor";
		contact.Contact_Cust_ID = Convert.ToInt32(persist["current_vendor_id"]);
		contact.AddNEContact(contact);
	//	var NSI = new NetSuite_Integration.Vendor();
	//	NSI.SyncNetSuite(vendobj, current_user.business_unit.tax_entity_id);
		e.Cancel = true;
		gv_contacts.CancelEdit();
		fill_contacts();
		}
	protected void gv_contacts_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
		{
		e.NewValues["login_enable"] = 0;
		e.NewValues["status"] = 8;
		e.NewValues["contact_name"] = "";
		e.NewValues["contact_status"] = "Active";
		e.NewValues["contact_extension"] = "";
		e.NewValues["contact_Cellphone"] = "";
		e.NewValues["contact_email"] = "";
		e.NewValues["contact_directline"] = "";
		}
	protected void gv_contacts_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
		{
		if (e.ButtonID == "Merge")
			{
			var contact_id = Convert.ToInt32(gv_contacts.GetRowValues(e.VisibleIndex, "contact_id"));
			//	pop_merge.ShowOnPageLoad = true;
			//	combo_merge_from.SelectedItem = combo_merge_from.Items.FindByValue(contact_id);
			}
		}
	protected void gv_contacts_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		var IDs = new object[gv.VisibleRowCount];
		for (var i = 0; i < gv.VisibleRowCount; i++)
			{
			IDs[i] = gv.GetRowValues(i, "contact_id");
			}
		e.Properties["cpid"] = IDs;
		}
	protected void gv_contacts_ClientLayout(object sender, ASPxClientLayoutArgs e)
		{
		if (!_can_edit_vendor_contacts)
			{
			var gv = (ASPxGridView)sender;
			gv.Columns["contact_password"].Visible = false;
			gv.Columns[0].Visible = false;
			}
		}
	protected void cb_provstate_Callback(object sender, CallbackEventArgsBase e)
		{
		cb_provstate.DataSource = new NeProvince().Load();
		cb_provstate.DataBind();
		cb_new_provstate.DataSource = new NeProvince().Load();
		cb_new_provstate.DataBind();
		var li1 = new ListEditItem("Unknown", 0);
		cb_new_provstate.Items.Add(li1);
		}
	protected void hl_web_Init(object sender, EventArgs e)
		{
		try
			{
			var container = ((ASPxHyperLink)sender).NamingContainer as GridViewDataItemTemplateContainer;
			if (container.Text != "&nbsp;")
				{
				((ASPxHyperLink)sender).ClientSideEvents.Click = "function(s,e){ window.open('" + container.Text.Replace("www.", "http://") + "','','scrollbars=no,menubar=no,height=600,width=800,resizable=yes,toolbar=no,location=no,status=no');}";
				}
			}
		catch { }
		}
	protected void pop_new_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
		{
		}
	protected void cb_save_memo_Callback(object source, CallbackEventArgs e)
		{
		}
	protected void pc_detail_Callback(object sender, CallbackEventArgsBase e)
		{
		if (t_memo.Text != "" && hid_vendor_id.Value != "")
			{
			var v = new NEVendor(Convert.ToInt32(hid_vendor_id.Value));
			var notesadd = new bv_notes();
			var nowdate = DateTime.Now;
			var bvdate = nowdate.ToString("yyyyMMdd");
			var bvtime = nowdate.ToString("HHmmssFF");
			notesadd.prog = "SUPP";
			notesadd.item = v.vendor_number;
			notesadd.subject = v.vendor_number;
			notesadd.n_date = bvdate;
			notesadd.n_time = Convert.ToInt32(bvtime);
			notesadd.n_user = current_user.Initials;
			notesadd.detail = t_memo.Text.Replace("\"", "").Replace("'", "");
			var error = "";
			if (notesadd.detail.Trim() != "")
				{
				var DSNS = Toolbox.doSQL_dt(@"SELECT distinct t.dsn FROM tax_entity t inner join business_unit b on t.id=b.tax_entity_id  WHERE t.is_active = 1 AND t.is_test = 0", null);
				foreach (DataRow dsnrow in DSNS.Rows)
					{
					try
						{
						var dsn = dsnrow[0].ToString();
						notesadd.Save(dsn);
						}
					catch (Exception ee)
						{
						error += ee.Message + "<br/>";
						}
					//notesadd.Save("BV7DEBUG");
					}
				}
			else
				{
				error = "Note not defined";
				}
			if (error != "")
				{
				memo_error.Text = error;
				}
			else
				{
				memo_error.Text = "";
				}
			t_memo.Text = "";
			btn_memo.Enabled = true;
			fill_main();
			}
		}
	protected void btn_cancel_Click(object sender, EventArgs e)
		{
		gv_phones.CancelEdit();
		}
	protected void gv_phones_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
		{
		e.Visible = _can_edit_vendor_contacts;
		//	e.Visible = true;
		}
	protected void btn_save_Click(object sender, EventArgs e)
		{
		if (!_can_edit_vendor_contacts)
			{
			throw new Exception("Sorry you do not have permission to Edit Vendor Contacts");
			}
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
		if (!_can_edit_vendor_contacts)
			{
			throw new Exception("Sorry you do not have permission to Edit Vendor Contacts");
			}
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
		if (!_can_edit_vendor_contacts)
			{
			throw new Exception("Sorry you do not have permission to Edit Vendor Contacts");
			}
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
		var d = Convert.ToInt32(e.Keys[0]);
		p = new NePhoneNumbers(d);
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
		if (!_can_save_vendor)
			{
			throw new Exception("Sorry you do not have permission to Edit Vendors");
			}
		NePhoneNumbers p;
		p = new NePhoneNumbers(Convert.ToInt32(e.Keys[0]));
		p.delete();
		gv_phones.DataBind();
		e.Cancel = true;
		gv_phones.CancelEdit();
		}
	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		if (!_can_save_vendor)
			{
			throw new Exception("Sorry you do not have permission to Edit Vendors");
			}
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
	*/
	}
