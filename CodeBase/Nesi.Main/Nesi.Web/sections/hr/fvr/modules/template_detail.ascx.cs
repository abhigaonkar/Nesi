using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data;
using System.Web.UI.HtmlControls;
using nesi.core;

public partial class sections_hr_fvr_modules_template_detail : System.Web.UI.UserControl
	{
	public ASPxComboBox ComboType	{ get { return combo_type;} }
	public ASPxPageControl PCType { get { return pc_type; } }
	protected void Page_Init(object sender, EventArgs e)
		{
		load_children(); // Needed for saving... needs the children in order to re-bind from the pagestate.
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		pc_type.ShowTabs		= false;
		}
	protected void cbp_pages_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var cbp							= (ASPxCallbackPanel) sender;

		}
	public void reset()
		{
		pc_type.TabPages[1].Controls.Clear();
		pc_type.TabPages[2].Controls.Clear();
		pc_type.TabPages[3].Controls.Clear();
		pc_type.ActiveTabIndex	= 0;
		tb_name.Text			= "";
		combo_type.Value		= 0;

		}
	public void load_children()
		{
		var dt_pages		= Toolbox.doSQL_dt(@"SELECT id, default_file_id, type, tab_index, name, upload_required FROM member_fvr_tab ORDER BY type,tab_index"  , null);
		// Set up New
		if(pc_type.TabPages[1].Controls.Count == 0)
			{
			var color			= 255;
			foreach(DataRow dr in dt_pages.Select("type = 'NEWHIRE'").CopyToDataTable().Rows)
				{
				var color_hex	= System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(color, color, color));
				load_ctrl(pc_type, 0, "NEWHIRE", dr, color_hex);
				color				= color - 2;
				}
			}
		// Set up Exit
		if(pc_type.TabPages[2].Controls.Count == 0)
			{
			var color			= 255;
			foreach(DataRow dr in dt_pages.Select("type = 'EXIT'").CopyToDataTable().Rows)
				{
				var color_hex	= System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(color, color, color));
				load_ctrl(pc_type, 1, "EXIT", dr, color_hex);
				color				= color - 2;
				}
			}
		// Set up Renew
		if(pc_type.TabPages[3].Controls.Count == 0)
			{
			var color			= 255;
			foreach(DataRow dr in dt_pages.Select("type = 'RENEW'").CopyToDataTable().Rows)
				{
				var color_hex	= System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(color, color, color));
				load_ctrl(pc_type, 2, "RENEW", dr, color_hex);
				color				= color - 2;
				}
			}
		}
	public void load(int id)
		{
		var hdr					= new member_fvr_template.header(id);
		var dtls									= Toolbox.doSQL_dt(@"SELECT id FROM member_fvr_template_detail WHERE hdr_id = @v0  ", new object[] {  id } );
		tb_name.Text									= hdr.name;
		combo_type.Value								= hdr.type_id;
		combo_type.ClientEnabled						= false;
		spin_till_expire.Value							= hdr.days_till_expire;
		hid_id.Value									= id.ToString();
		pc_type.ActiveTabIndex							= hdr.type_id;
		var li_str								= new List<string>(){"", "NEWHIRE", "EXIT", "RENEW"};
		foreach(DataRow dr in dtls.Rows)
			{
			var dtl_id										= Convert.ToInt32(dr["id"]);
			var dtl					= new member_fvr_template.detail(dtl_id);
			var tp		= (sections_hr_fvr_modules_template_page) pc_type.TabPages[hdr.type_id].FindControl(li_str[hdr.type_id]+dtl.page_id);
			var cb_enable						= (HtmlInputCheckBox) tp.FindControl("chk_enablepage");
			var lb_files							= (ASPxListBox) tp.FindControl("list_files");
			var tb_url								= (ASPxTextBox) tp.FindControl("tb_url");
			tb_url.Text										= dtl.url;
			var hid_dtlid						= (HtmlInputHidden) tp.FindControl("hid_id");
			tp.thisID										= dtl_id;
			var hid_pageid						= (HtmlInputHidden) tp.FindControl("hid_pageid");
			var chk_isrequired							= (CheckBox) tp.FindControl("chk_isrequired");
			cb_enable.Checked								= dtl.is_selected;
			chk_isrequired.Checked						= dtl.upload_required;
			if(dtl.file_id > 0)
				{
				lb_files.Value							= dtl.file_id;
				}
			if(cb_enable.Checked && tp.PageName != "Employee Information")
				{
				var div_list_files			= (HtmlGenericControl) tp.FindControl("div_list_files");
				div_list_files.Style.Clear();
				var div_url					= (HtmlGenericControl) tp.FindControl("div_url"); 
				div_url.Style.Clear();
				div_url.Style["margin-top"]					= "5px";
				var div_file_buttons			= (HtmlGenericControl) tp.FindControl("div_file_buttons"); 
				div_file_buttons.Style.Clear();
				var div_isrequired			= (HtmlGenericControl) tp.FindControl("div_isrequired");
				div_isrequired.Style.Clear();
				}
			else
				{
				chk_isrequired.Checked = false;
				}
			}
		}
	protected void pc_type_Init(object sender, EventArgs e)
		{
		}
	private void load_ctrl(ASPxPageControl _pc, int _page_index, string type_name, DataRow dr, string bgColor)
		{
		var ctrl_page		= (sections_hr_fvr_modules_template_page) LoadControl(@"./template_page.ascx");
		ctrl_page.ID										= type_name+dr["id"];
		ctrl_page.PageID									= Convert.ToInt32(dr["id"]);
		ctrl_page.PageName									= dr["name"].ToString();
		ctrl_page.TypeID									= _page_index;
		ctrl_page.PageIndex									= Convert.ToInt32(dr["tab_index"]);
		ctrl_page.bgColor									= bgColor;
		ctrl_page.DefaultFileID								= dr["default_file_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["default_file_id"]);
		ctrl_page.UploadRequired							= Convert.ToInt32(dr["upload_required"]) == 1;
		_pc.TabPages[_page_index+1].Controls.Add(ctrl_page);
		}
}