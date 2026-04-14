using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using System.Text;
using nesi.core;

public partial class sections_hr_fvr_modules_pop_manage_templates : System.Web.UI.UserControl
	{
	Toolbox _tools;
	NeMember current_user		= new NeMember();
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools					= new Toolbox();
		current_user			= Toolbox.do_handle_authentication(1);
		if(Page.IsCallback)
			{
			var dtl										= LoadControl("./template_detail.ascx");
			dtl.ID											= "new_template_dtl";
			cbp_detail.Controls.Add(dtl);
			}
		load_current_templates();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		if(Page.IsCallback)
			{
			#region Needed to regenerate dynamic user controls
			var detail	= (sections_hr_fvr_modules_template_detail) cbp_detail.FindControl("new_template_dtl");
			var cb_type							= (ASPxComboBox) detail.FindControl("combo_type");
			if(cb_type != null && cb_type.Value != null)
				{
				var cb_type_value							= (int) cb_type.Value;
				var pc							= (ASPxPageControl) detail.FindControl("pc_type");
				for(var i = 1; i < pc.TabPages.Count; i++)
					{
					foreach(Control p in pc.TabPages[i].Controls)
						{
						if(p is sections_hr_fvr_modules_template_page)
							{
							var tp		= (sections_hr_fvr_modules_template_page) p;
							var cb_enable						= (HtmlInputCheckBox) tp.FindControl("chk_enablepage");
							if(cb_enable.Checked && tp.PageName != "Employee Information")
								{
								var div_list_files			= (HtmlGenericControl) tp.FindControl("div_list_files");
								div_list_files.Style.Clear();
								var lb_files						= (ASPxListBox) tp.FindControl("list_files");
								var val										= lb_files.Value;
								var div_url					= (HtmlGenericControl) tp.FindControl("div_url"); 
								div_url.Style.Clear();
								div_url.Style["margin-top"]					= "5px";
								var div_file_buttons			= (HtmlGenericControl) tp.FindControl("div_file_buttons"); 
								div_file_buttons.Style.Clear();
								var div_isrequired			= (HtmlGenericControl) tp.FindControl("div_isrequired");
								div_isrequired.Style.Clear();
								}
							}
						}
					}
				}
			#endregion Needed to regenerate dynamic user controls
			}
		}
	private void load_current_templates()
		{
		list_available_templates.DataSource				= Toolbox.doSQL_dt(@"SELECT id, name FROM member_fvr_template_header ORDER BY name"  , null);
		list_available_templates.ValueField				= "id";
		list_available_templates.TextField				= "name";
		list_available_templates.DataBind();
		}
	protected void cbp_detail_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		sections_hr_fvr_modules_template_detail dtl;
		switch(e.Parameter.Split('|')[0])
			{
			case "new":
				cbp_detail.Controls.Clear();
				dtl												= (sections_hr_fvr_modules_template_detail) LoadControl("./template_detail.ascx");
				dtl.ID											= "new_template_dtl";
				dtl.load_children();
				dtl.ComboType.SelectedIndex						= 0;
				dtl.PCType.ActiveTabIndex						= 0;
				cbp_detail.Controls.Add(dtl);
				cbp_detail.JSProperties["cpResult"]					= "";
			break;
			case "existing":
				cbp_detail.Controls.Clear();
				dtl												= (sections_hr_fvr_modules_template_detail) LoadControl("./template_detail.ascx");
				dtl.ID											= "new_template_dtl";
				dtl.load_children();
				dtl.load(Convert.ToInt32(e.Parameter.Split('|')[1]));
				var active_tab_index							= dtl.PCType.ActiveTabIndex;
				cbp_detail.Controls.Add(dtl);
				// This is a hack method to get this out the door... basically the active tab index is sticking even through implicit setting of it... very odd.
				dtl												= (sections_hr_fvr_modules_template_detail) cbp_detail.FindControl("new_template_dtl");
				var pc_t							= (ASPxPageControl) dtl.FindControl("pc_type");
				pc_t.ActiveTabIndex								= active_tab_index;
				cbp_detail.JSProperties["cpResult"]				= "";
			break;
			case "cancel":
				cbp_detail.Controls.Clear();
				cbp_detail.JSProperties["cpResult"]					= "";
			break;
			case "save":
				var detail	= (sections_hr_fvr_modules_template_detail) cbp_detail.FindControl("new_template_dtl");
				var cb_type							= (ASPxComboBox) detail.FindControl("combo_type");
				var lb_error								= (ASPxLabel) detail.FindControl("lb_error");
				var tb_name								= (ASPxTextBox) detail.FindControl("tb_name");
				var spin_till_expire					= (ASPxSpinEdit) detail.FindControl("spin_till_expire");
				var hid_id							= (HtmlInputHidden) detail.FindControl("hid_id");
				var is_valid						= validate((int) cb_type.Value);
				if(!is_valid.success)
					{
					lb_error.Text								= is_valid.message;
					}
				else // is valid, save the template
					{
					if(hid_id.Value == "" || hid_id.Value == "0")
						{
						var this_hdr			= new member_fvr_template.header();
						this_hdr.name								= tb_name.Text;
						this_hdr.type_id							= (int) cb_type.Value;
						this_hdr.created_member_id					= current_user.id;
						this_hdr.days_till_expire					= Convert.ToInt32(spin_till_expire.Value);
						this_hdr.save();

						var pc								= (ASPxPageControl) detail.FindControl("pc_type");
						foreach(Control p in pc.TabPages[this_hdr.type_id].Controls)
							{
							if(p is sections_hr_fvr_modules_template_page)
								{
								var tp		= (sections_hr_fvr_modules_template_page) p;
								var cb_enable						= (HtmlInputCheckBox) tp.FindControl("chk_enablepage");
								var lb_files							= (ASPxListBox) tp.FindControl("list_files");
								var tb_url								= (ASPxTextBox) tp.FindControl("tb_url");
								var hid_pageid						= (HtmlInputHidden) tp.FindControl("hid_pageid");
								var chk_isrequired							= (CheckBox) tp.FindControl("chk_isrequired");
								var is_selected								= cb_enable.Checked;
								var this_dtl = new member_fvr_template.detail
									               {
									               hdr_id  = this_hdr.id,
									               page_id = Convert.ToInt32(hid_pageid.Value),
									               file_id = !cb_enable.Checked || tb_url.Text.Trim() != "" || lb_files.Value == null
										               ? 0
										               : (int) lb_files.Value,
									               is_selected = cb_enable.Checked,
									               url         = !cb_enable.Checked
										               ? ""
										               : tb_url.Text.Trim(),
									               upload_required = cb_enable.Checked && chk_isrequired.Checked &&
									                                 tp.PageName != "Employee Information"
									               };
								this_dtl.save();
								}
							}
						cbp_detail.Controls.Clear();
						cbp_detail.JSProperties["cpResult"]					= "refresh_available";
						}
					else
						{
						var this_hdr			= new member_fvr_template.header(Convert.ToInt32(hid_id.Value));
						this_hdr.name								= tb_name.Text;
						this_hdr.days_till_expire					= Convert.ToInt32(spin_till_expire.Value);
						this_hdr.save();

						var pc								= (ASPxPageControl) detail.FindControl("pc_type");
						foreach(Control p in pc.TabPages[this_hdr.type_id].Controls)
							{
							if(p is sections_hr_fvr_modules_template_page)
								{
								var tp		= (sections_hr_fvr_modules_template_page) p;
								var cb_enable						= (HtmlInputCheckBox) tp.FindControl("chk_enablepage");
								var lb_files							= (ASPxListBox) tp.FindControl("list_files");
								var tb_url								= (ASPxTextBox) tp.FindControl("tb_url");
								var hid_pageid						= (HtmlInputHidden) tp.FindControl("hid_pageid");
								var hid_dtlid						= (HtmlInputHidden) tp.FindControl("hid_id");
								var chk_isrequired							= (CheckBox) tp.FindControl("chk_isrequired");
								var is_selected								= cb_enable.Checked;
								var this_dtl = new member_fvr_template.detail(Convert.ToInt32(hid_dtlid.Value))
									               {
									               hdr_id  = this_hdr.id,
									               page_id = Convert.ToInt32(
										               hid_pageid.Value),
									               file_id = !cb_enable.Checked || tb_url.Text.Trim() != "" || lb_files.Value == null
										               ? 0
										               : (int) lb_files.Value,
									               is_selected = is_selected,
									               url         = !cb_enable.Checked
										               ? ""
										               : tb_url.Text.Trim(),
									               upload_required = cb_enable.Checked && chk_isrequired.Checked &&
									                                 tp.PageName != "Employee Information"
									               };
								this_dtl.save();
								}
							}
						cbp_detail.Controls.Clear();
						cbp_detail.JSProperties["cpResult"]					= "refresh_available";
						}
					}
			break;
			}
		}
	private Toolbox.boolstr validate(int type_index)
		{
		var detail	= (sections_hr_fvr_modules_template_detail) cbp_detail.FindControl("new_template_dtl");
		var cb_type							= (ASPxComboBox) detail.FindControl("combo_type");
		var pc								= (ASPxPageControl) detail.FindControl("pc_type");
		var tb_name								= (ASPxTextBox) detail.FindControl("tb_name");
		var sb								= new StringBuilder();
		var selected_pages								= 0;
		var bs								= new Toolbox.boolstr();
		bs.success										= true;
		if(tb_name.Text.Trim() == "")
			{
			bs.success			= false;
			sb_formatter("A valid name was not provided", "", ref sb); 
			}
		if((int) cb_type.Value == 0)
			{
			bs.success			= false;
			sb_formatter("Type of template not provided", "", ref sb); 
			}
		foreach(Control p in pc.TabPages[type_index].Controls)
			{
			if(p is sections_hr_fvr_modules_template_page)
				{
				var tp		= (sections_hr_fvr_modules_template_page) p;
				var cb_enable						= (HtmlInputCheckBox) tp.FindControl("chk_enablepage");
				var lb_files							= (ASPxListBox) tp.FindControl("list_files");
				var tb_url								= (ASPxTextBox) tp.FindControl("tb_url");
				tb_url.Text										= tb_url.Text.Replace(",", "");
				if(cb_enable.Checked)
					{
					selected_pages++;
					var is_employee_info						= tp.PageName == "Employee Information";
					Uri try_uri;
					if(!is_employee_info && lb_files.Value == null && tb_url.Text.Trim() == "")
						{
						bs.success			= false;
						sb_formatter("File not selected", tp.PageName, ref sb); 
						}
					else if(!is_employee_info && lb_files.Value == null && tb_url.Text.Trim() != "" && !Uri.TryCreate(tb_url.Text, UriKind.Absolute, out try_uri))
						{
						bs.success			= false;
						sb_formatter("Not a valid URL provided", tp.PageName, ref sb); 
						}
					}
				}
			}
		if(selected_pages == 0)
			{
			bs.success		= false;
			sb_formatter("No pages have been selected", "", ref sb); 
			}

		if(!bs.success && sb.Length > 0)
			{
			bs.message	= "<ul>"+sb+"</ul>";
			}
		return bs;
		}
	private void sb_formatter(string st, string page_name, ref StringBuilder sb)
		{
		if(page_name != "")
			{
			sb.AppendFormat("<li style='color:#f00;'>{0} for the page: <b>{1}</b></li>", st, page_name); 
			}
		else
			{
			sb.AppendFormat("<li style='color:#f00;'>{0}</li>", st); 
			}
		}
	protected void list_available_templates_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		load_current_templates();
		}
}
