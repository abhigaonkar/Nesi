using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using DevExpress.Web.Data;
using System.Web.Script.Serialization;
using System.Data;
using nesi.core;

public partial class sections_member_inventory_inv_rfq : System.Web.UI.UserControl
	{
	public NeMember myMember;
	Toolbox _tools;
	JavaScriptSerializer jSON				= new JavaScriptSerializer();
    public const string _page_name			= "BranchInventoryRFQ";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	NeBusinessUnit this_company;
	ASPxGridView gv_templates;
	NeGridLayouts gl;
	ASPxCallback cb_save_template;
	public string BusinessUnitList { get; set; }
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(43);
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		cb_save_template					= (ASPxCallback) layout.FindControl("cb_save_template");
		this_company						= new NeBusinessUnit(Session["working_business_unit_id"]);
		layout.used_gv						= gv_rfq;
		gv_templates						= (ASPxGridView) dde_filter.FindControl("gv_templates");
		}
	protected void Page_Load(object sender, EventArgs e)
		{
			layout.__page_name					= _page_name;
			panel_export.Visible				= true;
			if(!gv_rfq.IsCallback && !cb_save_template.IsCallback && !gv_templates.IsCallback)
				{
				gl									= new NeGridLayouts(myMember.id, _page_name);
				ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
				ds_templates.SelectParameters["@member_id"].DefaultValue	= myMember.id.ToString();
				if(gl.GridLayout_Layout != "")
					{
					gv_rfq.LoadClientLayout(gl.GridLayout_Layout);
					}
				else
					{
					gl							= new NeGridLayouts();
					gl.GridLayout_Layout		= gv_rfq.SaveClientLayout();
					gl.member_id	= myMember.id;
					gl.GridLayout_Name			= "Default";
					gl.GridLayout_Gridid		= _page_name;
					gl.SaveGridLayout();
					}
			//	hidexp.Value					= gv_rfq.SaveClientLayout();
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				dde_filter.Text					= gl.GridLayout_Name;
				}
		}
	protected void cbp_save_rfq_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		}
	protected void gv_rfq_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		gv.CancelEdit();
		gv.DataBind();
		if(e.Parameters != "" && !e.Parameters.Contains('|') && e.Parameters != "new")
			{
			gv_rfq.FilterExpression		= "vendors like '%"+e.Parameters+"%'";
			}
		else if(e.Parameters == "new")
			{
			gv_rfq.FilterExpression		= "";
			}
		else if(e.Parameters != "")
			{
			gv_rfq.FilterExpression		= e.Parameters;
			}
		}
	protected void gv_rfq_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
		{
		if(e.Expanded)
			{
			var gv				= (ASPxGridView) sender;
			var id					= gv.GetRowValues(e.VisibleIndex, "id").ToString();
			Session["working_rfq_id"]	= id;
			var i		= (HtmlContainerControl) gv.FindDetailRowTemplateControl(e.VisibleIndex, "i_rfq_detail");
			i.Attributes["src"]			= "/sections/member/picklist/pikclist.aspx?origin=rfq&id="+id;
			}
		}
	protected class rfq_merge_item
		{
		public int master_id { get; set;}
		public string vendor_code { get; set; }
		public double cost {  get; set; }
		public double qty_per {get; set;}
		}
	protected void cb_merge_Callback(object source, CallbackEventArgs e)
		{
	
	
		
		var json_text			= e.Parameter;
		//_tools.catch_error(new Exception(json_text));
		var rm	= jSON.Deserialize<IList<rfq_merge_item>>(json_text);
		var returned				= "";
		var i						= 1;
		var rfq_id					= 0;
		var business_unit_id				= 0;
		var vrfq				= new VendorRFQ();
		var rfq				= new rfq_vendor();
		if(Session["working_rfq_id"] != null)
			{
			rfq_id					= Convert.ToInt32(Session["working_rfq_id"]);
			vrfq					= new VendorRFQ(rfq_id);
			business_unit_id				= vrfq.business_unit_id;
			}
		var vendor_id				= 0; 
		if(Session["rfq_vendor_id"] != null)
			{
			vendor_id				= Convert.ToInt32(Session["rfq_vendor_id"]);
			}
		//int vendor_number_int		= _tools.getSQL_int(@"SELECT vendor_number_int FROM vendor WHERE vendor_id = @v0 ", new object[] {  vendor_id } );
		// Level 1 - Check for syntaxual errors
		foreach(var r in rm)
			{
			if(r.vendor_code.Trim() == "")
				{
				returned							+= "• The vendor code on the "+AddOrdinalSuffix(i)+" selected line is blank\n";
				}
			if(r.cost == 0)
				{
				returned							+= "• The cost on the "+AddOrdinalSuffix(i)+" selected line is zero\n";
				}
			if(r.cost < 0)
				{
				returned							+= "• The cost on the "+AddOrdinalSuffix(i)+" selected line is less than zero\n";
				}
			if(r.qty_per == 0)
				{
				returned							+= "• The qty per on the "+AddOrdinalSuffix(i)+" selected line is zero\n";
				}
			if(r.qty_per < 0)
				{
				returned							+= "• The qty per on the "+AddOrdinalSuffix(i)+" selected line is less than zero\n";
				}
			i++;
			}

		// Level 2 - Passed Level 1, now check for possible conflicts in existing data
		if(returned == "" && i > 0)
			{
			foreach(var r in rm)
				{
				// Check for existing parts with the same vendor code in the current branch / vendor
				var dr						= _tools.getSQL_datatable(@"SELECT COUNT(*) c, IFNULL(GROUP_CONCAT(DISTINCT master_id), '') m FROM inventory_price WHERE master_id != @v0  AND vendor_code = @v1  AND vendor_id = @v2 ", new object[] {  r.master_id, Toolbox.do_value_from(r.vendor_code, false).Trim(), vendor_id } ).Rows[0];
				var existing_rows				= Convert.ToInt32(dr["c"]);
				var master_ids				= dr["m"].ToString();
				if(existing_rows > 0)
					{
					returned					+= "• The vendor part # for "+r.master_id+" on the "+AddOrdinalSuffix(i)+" selected line already exists on id(s): "+master_ids+"\n";
					}
				}
			if(returned == "")
				{
				// Level 3 - Merge & Update
				var vpr		= new vendor_price_row();
				//	_tools.page_author		= new NeMember(711);
				foreach(var r in rm)
					{
					var line	= new rfq_lineitem(rfq_id, r.master_id, vendor_id);
					vpr					= new vendor_price_row(r.master_id, business_unit_id, vendor_id, Toolbox.do_value_from(r.vendor_code, false).Trim());
					//_tools.catch_error(new Exception(Toolbox.dict_dump(Toolbox.dict_create(vpr))));
					if(!vpr.exists)
						{
						vpr.master_id		= r.master_id;
						vpr.vendor_id		= vendor_id;//vendor_number_int;
						vpr.is_benchmark	= false;
						vpr.is_preferred	= false;
						vpr.business_unit_id		= business_unit_id;
						}
					vpr.vendor_code		= Toolbox.do_value_from(r.vendor_code, false).Trim();
					vpr.lead_time		= line.lead_time;
					vpr.member_id		= myMember.id;
					vpr.qty				= r.qty_per;
					vpr.cost			= r.cost / r.qty_per;
					vpr.total			= r.cost;
					vpr.origin			= "RFQ Merge - "+Toolbox.MySQLNow_short();
					vpr.save(true);

					line.qty			= r.qty_per;
					line.vendor_code	= r.vendor_code;
					line.vendor_price	= r.cost;
					line.save();
					line.set_merged();
					}
				returned = "SUCCESS";
				}
			}
		else if(i == 0)
			{
			returned = "No lines were sent...";
			}
		e.Result	= returned;
		}
	private string AddOrdinalSuffix(int num)
		{
		var last2Digits = Math.Abs(num % 100);
		var lastDigit = last2Digits % 10;
		return num + "thstndrd".Substring((last2Digits > 10 && last2Digits < 14) || lastDigit > 3 ? 0 : lastDigit * 2, 2);
		}
	protected void gv_rfq_detail_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv						= (ASPxGridView) sender;
		if (e.Parameters.Equals("merge"))
		{
			var fieldValues = gv.GetSelectedFieldValues(new string[] { "master_id", "vendor_code", "cost", "qty_per", });
			foreach (object[] item in fieldValues)
			{
				var master_id = Convert.ToInt32(item[0]);
				var vendor_code = item[1].ToString();
				var cost = Convert.ToDouble(item[2]);
				var qty_per = Convert.ToDouble(item[3]);

				var returned = "";

				var rfq_id = 0;
				var business_unit_id = 0;
				var vrfq = new VendorRFQ();
				var rfq = new rfq_vendor();
				if (Session["working_rfq_id"] != null)
				{
					rfq_id = Convert.ToInt32(Session["working_rfq_id"]);
					vrfq = new VendorRFQ(rfq_id);
					business_unit_id = vrfq.business_unit_id;
				}
				var vendor_id = 0;
				if (Session["rfq_vendor_id"] != null)
				{
					vendor_id = Convert.ToInt32(Session["rfq_vendor_id"]);
				}
				//int vendor_number_int		= _tools.getSQL_int(@"SELECT vendor_number_int FROM vendor WHERE vendor_id = @v0 ", new object[] {  vendor_id } );
				// Level 1 - Check for syntaxual errors

				if (vendor_code == "")
				{
					returned += "• The vendor code on the " + master_id + " selected line is blank\n";
				}
				if (cost == 0)
				{
					returned += "• The cost on the " + master_id + " selected line is zero\n";
				}
				if (cost < 0)
				{
					returned += "• The cost on the " + master_id + " selected line is less than zero\n";
				}
				if (qty_per == 0)
				{
					returned += "• The qty per on the " + master_id + " selected line is zero\n";
				}
				if (qty_per < 0)
				{
					returned += "• The qty per on the " + master_id + " selected line is less than zero\n";
				}

				if (returned != "")
				{
					throw new Exception(returned);
				}
				// Level 2 - Passed Level 1, now check for possible conflicts in existing data

				// Check for existing parts with the same vendor code in the current branch / vendor
				var dr = _tools.getSQL_datatable(@"SELECT COUNT(*) c, IFNULL(GROUP_CONCAT(DISTINCT master_id), '') m FROM inventory_price WHERE master_id != @v0  AND vendor_code = @v1  AND vendor_id = @v2 ", new object[] {  master_id, Toolbox.do_value_from(vendor_code, false).Trim(), vendor_id } ).Rows[0];
				var existing_rows = Convert.ToInt32(dr["c"]);
				var master_ids = dr["m"].ToString();
				if (existing_rows > 0)
				{
					returned += "• The vendor part # for " + master_id + " selected line already exists on another master id(s): " + master_ids + "\n";
				}
				if (returned != "")
				{
					throw new Exception(returned);
				}

				// Level 3 - Merge & Update
				var vpr = new vendor_price_row();
				//	_tools.page_author		= new NeMember(711);

				var line = new rfq_lineitem(rfq_id, master_id, vendor_id);
				vpr = new vendor_price_row(master_id, business_unit_id, vendor_id, Toolbox.do_value_from(vendor_code, false).Trim());
				//_tools.catch_error(new Exception(Toolbox.dict_dump(Toolbox.dict_create(vpr))));
				if (!vpr.exists)
				{
					vpr.master_id = master_id;
					vpr.vendor_id = vendor_id;//vendor_number_int;
					vpr.is_benchmark = false;
					vpr.is_preferred = false;
					vpr.business_unit_id = business_unit_id;
				}
				vpr.vendor_code = Toolbox.do_value_from(vendor_code, false).Trim();
				vpr.lead_time = line.lead_time;
				vpr.member_id = myMember.id;
				vpr.qty = qty_per;
				vpr.cost = cost / qty_per;
				vpr.total = cost;
				vpr.origin = "RFQ Merge - " + Toolbox.MySQLNow_short();
				vpr.save(true);

				line.qty = qty_per;
				line.vendor_code = vendor_code;
				line.vendor_price = cost;
				line.save();
				line.set_merged();

				gv.JSProperties["cp_alert"] = "Selected line itmes have been successfully merged";
			}
		}
		else
		{

			var rfq_id = 0;
			var rfq = new rfq_vendor();
			if (Session["working_rfq_id"] != null)
			{
				rfq_id = Convert.ToInt32(Session["working_rfq_id"]);
			}
			var vendor_id = 0;
			if (Session["rfq_vendor_id"] != null)
			{
				vendor_id = Convert.ToInt32(Session["rfq_vendor_id"]);
			}
			if (vendor_id > 0 && rfq_id > 0)
			{
				rfq = new rfq_vendor(vendor_id, rfq_id);
			}
	//		gv.Columns["Merge"].Visible = (rfq_id != 0 && rfq.date_verified != null);
	//		gv.Columns["Merge Date"].Visible = (rfq_id != 0 && rfq.date_verified != null);
			gv.DataBind();
		}
		}
	protected void gv_rfq_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		var gv					= (ASPxGridView) sender;
		var p						= (ASPxPanel)  gv.FindEditFormTemplateControl("rfq_panel");
		var name				= (ASPxTextBox) p.FindControl("t_rfq_name");
		var vendor			= (HtmlInputText) p.FindControl("t_vendor");
		var contact				= (HtmlSelect) p.FindControl("s_contact");		
		var save_button			= (ASPxButton) p.FindControl("b_save_vendor");
		var lb_vendor					= (Label) p.FindControl("lb_vendor");
		var lb_contact				= (Label) p.FindControl("lb_contact");
		var open_date			= (ASPxDateEdit) p.FindControl("date_open");
		var close_date			= (ASPxDateEdit) p.FindControl("date_close");
		open_date.MinDate				= DateTime.Now.Date;
		close_date.MinDate				= DateTime.Now.AddDays(1);

		if(gv_rfq.IsNewRowEditing)
			{
			var gv_v				= (ASPxGridView) p.FindControl("gv_vendors");
			gv_v.Visible					= 
			vendor.Visible					= 
			contact.Visible					= 
			lb_vendor.Visible				= 
			lb_contact.Visible				= 
			save_button.Visible				= false;
			name.Focus();
			}
		else
			{
			vendor.Attributes.Add("onfocus", "attach_ac(this, 'vendor')");
			//contact.Attributes.Add("onfocus", "attach_ac(this, 'vendor_contact', {control:'"+vendor.ClientID+"'})");
			}
		}
	protected void gv_vendors_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		var rv				= new rfq_vendor(Convert.ToInt32(e.Keys[0]));
		rv.delete();
		gv.DataBind();
		e.Cancel					= true;
		}
	protected void cbox_available_vendors_DataBound(object sender, EventArgs e)
		{
		var cb					= (ASPxComboBox) sender;
		var item				= new ListEditItem("Pick a Vendor");
		item.Value						= 0;
		item.Selected					= true;
		cb.Items.Insert(0, item);
		}
	protected void cbox_available_contacts_DataBound(object sender, EventArgs e)
		{
		var cb					= (DropDownList) sender;
		var item					= new ListItem("Pick a Contact");
		item.Value						= "0";
		item.Selected					= true;
		cb.Items.Insert(0, item);
		}
	protected void cbox_available_vendors_SelectedIndexChanged(object sender, EventArgs e)
		{
		var cb					= (ASPxComboBox) sender;
		var p						= (ASPxPanel)  gv_rfq.FindEditFormTemplateControl("rfq_panel");
		var selected_contact	= (DropDownList) p.FindControl("cbox_available_contacts");
		selected_contact.DataBind();
		}
	protected void b_start_Click(object sender, EventArgs e)
		{
		var b						= (ASPxButton) sender;
		var p							= (ASPxPanel) gv_rfq.FindEditFormTemplateControl("rfq_panel");
		if(ASPxEdit.ValidateEditorsInContainer(p, "new_rfq"))
			{
			var id					= (HiddenField) p.FindControl("hid_rfq_id");
			var notes					= (ASPxMemo) p.FindControl("t_rfq_notes");
			var name				= (ASPxTextBox) p.FindControl("t_rfq_name");
			var open_date			= (ASPxDateEdit) p.FindControl("date_open");
			var close_date			= (ASPxDateEdit) p.FindControl("date_close");
			var rfq					= id.Value == "" ? new VendorRFQ() : new VendorRFQ(Convert.ToInt32(id.Value));
			rfq.name						= name.Text;
			rfq.notes						= notes.Text;
			rfq.date_open					= (DateTime) open_date.Value;
			rfq.date_close					= (DateTime) close_date.Value;
			rfq.business_unit_id					= Convert.ToInt32(Session["working_business_unit_id"]);
			rfq.member_id					= myMember.id;
			rfq.save();
			gv_rfq.DataBind();
			gv_rfq.StartEdit(gv_rfq.FindVisibleIndexByKeyValue(rfq.id));
			}
		}
	protected void b_cancel_Click(object sender, EventArgs e)
		{
		gv_rfq.CancelEdit();
		gv_rfq.DetailRows.CollapseAllRows();
		gv_rfq.DataBind();
		}
	protected void b_save_vendor_Click(object sender, EventArgs e)
		{
		var b					= (ASPxButton) sender;
		}
	protected void gv_rfq_StartRowEditing1(object sender, ASPxStartRowEditingEventArgs e)
		{
		Session["working_rfq_id"]	= e.EditingKeyValue.ToString();;
		}
	protected void preview_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
		{
		var bt								= (ASPxButton) sender;
		var container	= bt.NamingContainer as GridViewDataItemTemplateContainer;
		var p									= (ASPxPanel) gv_rfq.FindEditFormTemplateControl("rfq_panel");
		var gv								= (ASPxGridView) p.FindControl("gv_vendors");
		e.Properties["cp_vendor_id"]				= gv.GetDataRow(container.VisibleIndex)["vendor_id"].ToString();
		}
	protected void callback_set_vendor_Callback(object source, CallbackEventArgs e)
		{
		Session["rfq_vendor_id"]			= e.Parameter;
		}
	protected void bt_send_rfq_Click(object sender, EventArgs e)
		{
		}
	protected void gv_rfq_detail_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var vendor_id							= 0; 
		if(Session["rfq_vendor_id"] != null)
			{
			vendor_id							= Convert.ToInt32(Session["rfq_vendor_id"]);
			}
		
		var rfq_id								= 0;
		if(Session["working_rfq_id"] != null)
			{
			rfq_id								= Convert.ToInt32(Session["working_rfq_id"]);
			}
		if(rfq_id != 0 && vendor_id != 0)
			{
			var ven							= new NEVendor(vendor_id);
			try
				{
				var rfq							= new VendorRFQ(rfq_id);
				var rfv							= new rfq_vendor(vendor_id, rfq_id);
				var c								= new NEContact(rfv.contact_id);
				e.Properties["cp_vendor_name"]			= Toolbox.do_value_from(ven.Vendor_Name, false);
				e.Properties["cp_vendor_note"]			= rfv.note == null ? "" : Toolbox.do_value_from(rfv.note, false);
				e.Properties["cp_contact_name"]			= Toolbox.do_value_from(c.Contact_Name, false);
				e.Properties["cp_contact_email"]		= c.Contact_Email;
				e.Properties["cp_rfq_status"]			= rfq.status;
				}
			catch (Exception ee)
				{
				_tools.catch_error(ee);
				}
			}
		}
	protected void cbp_email_preview_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var rfq_id								= 0;
		if(Session["working_rfq_id"] != null)
			{
			rfq_id								= Convert.ToInt32(Session["working_rfq_id"]);
			}
		var vendor_id							= 0; 
		if(Session["rfq_vendor_id"] != null)
			{
			vendor_id							= Convert.ToInt32(Session["rfq_vendor_id"]);
			}
		var ven							= new NEVendor();
		var rfq							= new VendorRFQ();
		var con							= new NEContact();
		var rfv							= new rfq_vendor();
		if(rfq_id != 0 && vendor_id != 0)
			{
			ven											= new NEVendor(vendor_id);
			try
				{
				rfq										= new VendorRFQ(rfq_id);
				rfv										= new rfq_vendor(vendor_id, rfq_id);
				con										= new NEContact(rfv.contact_id);
				}
			catch
				{
				}
			}
		var member_type				= Toolbox.doSQL_string(@"SELECT membertype_name from membertype where membertype_id =@v0 ",myMember.MemberTypeID);
		var panel_text		= (LiteralControl) email_preview.Controls[0];
		panel_text.Text					= string.Format(@"
<div style='font-family:Arial;padding:10px;white-space:pre;'>{0},
This is {1} from {2}.
We have put together an RFQ and would like it if you could take a look.

{3}
<hr/>This RFQ is available through our <a href='{9}/sections/vendor_rfq/index.aspx'>company's website</a>.
It will give you the ability to update/verify our price information we have stored for you.<br/>Your login will be valid until: <b>{7:dddd - MMMM dd, yyyy}</b>.
Your temporary passcode is: <b>{8}</b><hr />
Thank you,
{4} Ext: {5}
{6}
<font style='font-size:9px;'>Please consider financial and environmental costs before you print off this email.</font>
</div>",
					con.Contact_Name,
					myMember.FirstName,
					new NeBusinessUnit(rfq.business_unit_id).name,
					rfv.note,
					myMember.FullName,
					myMember.PhoneExtension,
					member_type,
					rfq.date_close,
					rfv.keycode,
                    Toolbox.app_setting("Domain")
                    );
		}
	protected void cb_savenote_Callback(object source, CallbackEventArgs e)
		{
		var rfq_id								= 0;
		if(Session["working_rfq_id"] != null)
			{
			rfq_id								= Convert.ToInt32(Session["working_rfq_id"]);
			}
		var vendor_id							= 0; 
		if(Session["rfq_vendor_id"] != null)
			{
			vendor_id							= Convert.ToInt32(Session["rfq_vendor_id"]);
			}

		var rfv							= new rfq_vendor();
		if(rfq_id != 0 && vendor_id != 0)
			{
			try
				{
				rfv									= new rfq_vendor(vendor_id, rfq_id);
				rfv.save_note(e.Parameter);
				e.Result		= "SUCCESS";
				}
			catch (Exception ee)
				{
				e.Result		= ee.ToString();
				}
			}
		else
			{
			e.Result			= "Vendor or RFQ not set";
			}
		}
	protected void cb_sendemail_Callback(object source, CallbackEventArgs e)
		{
			if (_tools.getSQL_int(@"SELECT COUNT(*) FROM rfq_part_list WHERE rfq_header_id = @v0 ", new object[] {  Session["working_rfq_id"]} ) > 0)
			{
				try
				{
					//Check if they already exist...
					var c = _tools.getSQL_int(@"SELECT COUNT(*) FROM rfq_lineitem WHERE rfq_header_id = @v0  AND vendor_id = @v1 ", new object[] {  Session["working_rfq_id"], Session["rfq_vendor_id"] } );
					int rfq_id, vendor_id, master_id;
					if (c == 0)
					{
						#region Move items to rfq_lineitem
						var dt = _tools.getSQL_datatable(@" SELECT a.id, c.master_id, b.vendor_id, IFNULL(URLDECODE(e.vendor_code), '') vendor_code, IFNULL(e.cost, 0) cost, IFNULL(e.qty, 1) qty_per, IFNULL(e.lead_time, 7) lead_time, c.required_date FROM rfq_header a LEFT JOIN rfq_vendor_list b on a.id = b.rfq_header_id LEFT JOIN rfq_part_list c on a.id = c.rfq_header_id LEFT JOIN vendor d on b.vendor_id = d.vendor_id LEFT JOIN inventory_price e ON a.business_unit_id = e.business_unit_id AND d.vendor_id = e.vendor_id AND c.masteR_id = e.master_id LEFT join business_unit f ON a.business_unit_id = f.id WHERE a.id = @v0  AND b.vendor_id = @v1 ", new object[] {  Session["working_rfq_id"], Session["rfq_vendor_id"] } );
						foreach (DataRow dr in dt.Rows)
						{
							vendor_id = Convert.ToInt32(dr["vendor_id"]);
							rfq_id = Convert.ToInt32(dr["id"]);
							master_id = Convert.ToInt32(dr["master_id"]);
							rfq_lineitem li;
							try
							{
								li = new rfq_lineitem(rfq_id, master_id, vendor_id);
							}
							catch
							{
								li = new rfq_lineitem();
								li.rfq_header_id = rfq_id;
								li.master_id = master_id;
							}
							if (li.id == null)
							{
								li.required_date = dr["required_date"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["required_date"]);
								li.vendor_id = vendor_id;
								li.vendor_code = dr["vendor_code"].ToString();
								li.vendor_price = Convert.ToDouble(dr["cost"]);
								li.vendor_note = "";
								li.lead_time = Convert.ToInt32(dr["lead_time"]);
								li.note = "";
								li.save();
							}
						}
						#endregion Move items to rfq_lineitem
					}
					#region send rfq
					rfq_id = Convert.ToInt32(Session["working_rfq_id"]);
					vendor_id = Convert.ToInt32(Session["rfq_vendor_id"]);
					var rv = new rfq_vendor(vendor_id, rfq_id);
					var con = new NEContact(rv.contact_id);
					var rfq = new VendorRFQ(rfq_id);
					var mem = new NeMember(rfq.member_id);
					var em = new NeEMail();
					em.To = con.Contact_Email;
					em.CC = mem.NEEmail;
					em.From = mem.NEEmail;
					em.Subject = "Request for Quote from: " + mem.business_unit.name + ", RFQ#" + rfq.id;
					em.isHTML = true;

					var memo_text = string.Format(@"<div style='font-size:12pt;font-family:Arial;'>
	{0},
	This is {1} from {2}.
	We have put together an RFQ and would like it if you could take a look.

	{3}</div>",
								Toolbox.do_value_from(con.Contact_Name, false),
								mem.FirstName,
								mem.business_unit.name,
								rv.note
								);

					memo_text = memo_text.Replace("\n", "<br/>");
					em.Body = "<div style='font-size:12pt;font-family:Arial;'>" + memo_text + @"<br/><br/><hr />This RFQ is available through our <a href='" + Toolbox.app_setting("Domain") + "/sections/vendor_rfq/index.aspx?passcode=" + rv.keycode + "'>company's website</a>.<br/>It will give you the ability to update/verify our price information we have stored for you.<br/>Your login will be valid until: <b>" + rfq.date_close.ToString("dddd - MMMM dd, yyyy") + "</b>.<br/>Your temporary passcode is: <b>" + rv.keycode + "</b><hr /></div>";
					em.Send();
					rv.set_vendor_sent();
					rfq.status = "Sent";
					rfq.save();
					e.Result = "SUCCESS";
					#endregion send rfq
				}
				catch (Exception ex)
				{
					e.Result = ex.ToString();
				}
			}
			else
			{
				throw new Exception("No parts in this RFQ!");
			}
		}
	protected void gv_rfq_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	class rfq_header_item
		{
		public int id { get; set; }
		public string name { get; set; }
		public string note { get; set; }
		public DateTime date_open { get; set; }
		public DateTime date_close { get; set; }
		}
	protected void cb_save_Callback(object source, CallbackEventArgs e)
		{
		var json_text			= e.Parameter;
		var rf			= jSON.Deserialize<rfq_header_item>(json_text);
		var is_new					= rf.id == 0;
		var rfq				= is_new ? new VendorRFQ() : new VendorRFQ(rf.id);
		rfq.name					= rf.name;
		rfq.notes					= rf.note;
		rfq.date_open				= rf.date_open;
		rfq.date_close				= rf.date_close;
		rfq.business_unit_id				= Convert.ToInt32(Session["working_business_unit_id"]);
		rfq.member_id				= myMember.id;
		rfq.save();
		e.Result					= "SUCCESS";
		
		}
	protected void gv_rfq_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
	{

	

		_tools.getSQL_void(@"Delete from rfq_vendor_list where rfq_header_id = @v0" , new object[] { e.Keys[0]});
		_tools.getSQL_void(@"Delete from rfq_part_list where rfq_header_id =  @v0", new object[] { e.Keys[0] });
		_tools.getSQL_void(@"Delete from rfq_lineitem where rfq_header_id =  @v0", new object[] { e.Keys[0] });
		_tools.getSQL_void(@"Delete from rfq_header where id =  @v0", new object[] { e.Keys[0] });


		e.Cancel = true;
		gv_rfq.CancelEdit();
		gv_rfq.DataBind();

	}
}