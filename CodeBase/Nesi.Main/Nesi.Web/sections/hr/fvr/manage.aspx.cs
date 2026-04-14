using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Services;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.Common.Exceptions;

public partial class sections_hr_i_manage : System.Web.UI.Page
{
	public NeMember current_user;
	private const int _page_id = 123; // from Page table in DB
	private const string _page_name = "FVR";
	private const string _page_description = "FVR - File Verification Request";
	private Control post_sender;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	bool is_manager = false;
	bool is_manager_set = false;
	Toolbox _tools;
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		Session["fvr_req_member_id"] = current_user.id;
		layout.used_gv = gv_details;
		//Thread.Sleep(500);
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		l_error.Text = "";
		fill_branches();
		//if(!IsPostBack)
		//	{
		fill_tabs();
		fill_members();
		//	}

		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		h.Set("gridview_id", "gv_details");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
		if (!IsPostBack)
		{
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				gl.GridLayout_Layout = gv_details.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_details.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;
		}
	}
	protected void lb_company_SelectedIndexChanged(object sender, EventArgs e)
	{
		var c = lb_company.Items.Count;
		var select_all = lb_company.Items.FindByValue(9999).Selected;
		var select_none = lb_company.Items.FindByValue(9998).Selected;
		if (select_all)
		{
			foreach (ListEditItem li in lb_company.Items)
			{
				li.Selected = li.Value.ToString() != "9999" && li.Value.ToString() != "9998";
			}
			lb_company.Items.FindByValue(9999).Selected = false;
		}
		if (select_none)
		{
			foreach (ListEditItem li in lb_company.Items)
			{
				li.Selected = li.Value.ToString() == "9998";
			}
			lb_company.Items.FindByValue(9998).Selected = false;
			l_error.Text = "";
		}

		lb_employees.Items.Clear();
		fill_members();
		fill_tabs();
	}
	protected void lb_employees_SelectedIndexChanged(object sender, EventArgs e)
	{
		var c = lb_employees.Items.Count;
		if (c > 0)
		{
			var select_all = lb_employees.Items.FindByValue(9999).Selected;
			var select_none = lb_employees.Items.FindByValue(9998).Selected;
			if (select_all)
			{
				foreach (ListEditItem li in lb_employees.Items)
				{
					li.Selected = li.Value.ToString() != "9999" && li.Value.ToString() != "9998";
				}
				lb_employees.Items.FindByValue(9999).Selected = false;
			}
			if (select_none)
			{
				foreach (ListEditItem li in lb_employees.Items)
				{
					li.Selected = li.Value.ToString() == "9998";
				}
				lb_employees.Items.FindByValue(9998).Selected = false;
				l_error.Text = "";
			}
			fill_tabs();
		}
	}
	protected void lb_tabs_Init(object sender, EventArgs e)
	{
	}
	protected void combo_type_SelectedIndexChanged(object sender, EventArgs e)
	{
		post_sender = (Control)sender;
		pc_content.TabPages.Clear();
		lb_tabs.Items.Clear();
		fill_tabs();
	}
	protected void fill_branches()
	{
		if (lb_company.Items.Count == 0)
		    {
		    var dt = Toolbox.doSQL_dt(
		        @"SELECT id, ddl_name FROM business_unit a   WHERE a.istest = 'F' and a.Active='T' and a.id in(" + new Current_User().visible_business_units + ") order by ddl_name",
		        null);

            lb_company.Items.Add(new ListEditItem("Select All", 9999));
			lb_company.Items.Add(new ListEditItem("Deselect All", 9998));
			foreach (DataRow dr in dt.Rows)
			{
				var id = Convert.ToInt32(dr["id"]);
				var name = dr["ddl_name"].ToString();
				//if(id != 8)
				//	{
				lb_company.Items.Add(new ListEditItem(name, id));
				//}
			}
		}
	}
	protected void fill_members()
	{
		if (lb_employees.Items.Count == 0)
		{
			var item = new ListEditItem();
			var ids = "";
			for (var i = 0; i < lb_company.SelectedValues.Count; i++)
			{
				var business_unit_id = (int)lb_company.SelectedValues[i];
				if (business_unit_id == 99 && lb_company.SelectedValues.Count == 1)
				{
					foreach (ListEditItem li in lb_company.Items)
					{
						ids += li.Value + ",";
					}
				}
				else
				{
					ids += business_unit_id + ",";
				}
			}
			ids = ids.TrimEnd(',');
			if (ids != "")
			{

				var dt = Toolbox.doSQL_dt(string.Format(@" SELECT a.member_id id, CONCAT(b.ddl_name, ' - ',member_fullname,'(',member_email,')') name, b.ddl_name business_unit
FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id 
WHERE a.member_status = 'Active' AND b.id IN ({0}) ORDER BY b.name, a.member_fullname", ids), null);

				if (dt.Rows.Count > 0)
				{
					lb_employees.Items.Add(new ListEditItem("Select All", 9999));
					lb_employees.Items.Add(new ListEditItem("Deselect All", 9998));
					foreach (DataRow dr in dt.Rows)
					{
						if (dr["id"].ToString() != "1316")
						{
							item = new ListEditItem();
							item.Text = dr["name"].ToString();
							item.Value = Convert.ToInt32(dr["id"]);
							lb_employees.Items.Add(item);
						}
					}
				}
				l_totalusers.Text = "(" + dt.Rows.Count + ") Available Employees";
				lb_employees.Enabled = true;
			}
			else
			{
				l_totalusers.Text = "(0) Available Employees";
				lb_employees.Enabled = false;
			}
		}
	}
	protected void fill_tabs()
	{
		var fvr_type = combo_type.Value.ToString();
		if (lb_tabs.Items.Count > 0)
		{
			var active_tab = pc_content.ActiveTabIndex;
			pc_content.TabPages.Clear();
			var fs = new file_store();
			if (!lb_tabs.Items.FindByValue(9998).Selected)
			{
				foreach (ListEditItem item in lb_tabs.SelectedItems)
				{
					var this_val = Convert.ToInt32(item.Value);
					if (this_val != 9999 && this_val != 9998 &&
						(
						// Don't want a tab created for employee information.
						(this_val != 2 && fvr_type == "NEWHIRE") || (this_val != 18 && fvr_type == "RENEW") || (fvr_type == "EXIT")
						)
						)
					{
						var cat = new member_fvr_category((int)item.Value);
						var p = new TabPage(item.Text, "page" + cat.tab_index);
						p.ToolTip = item.Text;
						var up = new ASPxUploadControl();
						var lab_top = new ASPxLabel();
						var lab_oruseexisting = new ASPxLabel();
						var gen_left = new HtmlGenericControl("div");
						gen_left.Attributes["class"] = "tab_left_col";
						var gen_right = new HtmlGenericControl("div");
						gen_right.Attributes["class"] = "tab_right_col";

						lab_top.ID = "lab_top" + item.Text.ToLower().Replace(" ", "");
						lab_top.EncodeHtml = false;
						lab_top.Text = "Upload a new file";
						lab_top.CssClass = "label_top";
						//gen_right.Controls.Add(lab_top);
						up.ID = "upload" + item.Text.ToLower().Replace(" ", "");
						up.ShowUploadButton = true;
						up.ShowProgressPanel = true;
						up.FileUploadMode = UploadControlFileUploadMode.OnPageLoad;
						up.FileUploadComplete += new EventHandler<FileUploadCompleteEventArgs>(upload_handler);
						up.ClientSideEvents.FileUploadComplete = "function(s, e) {if(e.errorText == ''){b_refresh_new.ClickInternalButton();}";
						//gen_right.Controls.Add(up);


						lab_oruseexisting.ID = "lab_oruseexisting" + item.Text.ToLower().Replace(" ", "");
						lab_oruseexisting.Text = "Choose an existing file for this category";
						lab_oruseexisting.CssClass = "label_oruseexisting";
						gen_left.Controls.Add(lab_oruseexisting);

						var existing_files = new ASPxListBox();
						existing_files.ID = "existing_files" + item.Text.ToLower().Replace(" ", "");
						existing_files.ClientInstanceName = existing_files.ID;
						existing_files.SelectionMode = ListEditSelectionMode.Single;
						existing_files.Width = Unit.Pixel(550);
						existing_files.ValueType = Type.GetType("int");
						existing_files.Height = Unit.Pixel(200);
						existing_files.ClientSideEvents.SelectedIndexChanged = "handle_url";
						var _files = fs.get_files(Convert.ToInt32(_page_id), combo_type.SelectedIndex, cat.tab_index);
						var def_file_id = cat.default_file_id;
						foreach (DataRow _file in _files.Rows)
						{
							var li = new ListEditItem();
							var date = Convert.ToDateTime(_file["dt"]);
							li.Value = Convert.ToInt32(_file["id"]);
							if ((int)li.Value == def_file_id)
							{
								li.Selected = true;
							}
							var is_default = li.Selected ? "(default) " : "";
							li.Text = is_default + _file["name"] + "." + _file["ext"] + " --- Date Saved: " + date.ToString("G");
							existing_files.Items.Add(li);
						}
						gen_left.Controls.Add(existing_files);

						var url = new ASPxTextBox();
						url.ID = "tb_url" + item.Text.ToLower().Replace(" ", "");
						url.AutoPostBack = false;
						url.Width = Unit.Pixel(550);
						url.Attributes["style"] = "margin-top:5px;margin-bottom:5px;";
						url.ClientInstanceName = url.ID;
						url.NullText = "OR enter a url for a page";
						url.ClientSideEvents.KeyPress = "handle_url";
						gen_left.Controls.Add(url);

						var button = new ASPxButton();
						button.ID = "button_preview" + item.Text.ToLower().Replace(" ", "");
						button.AutoPostBack = false;
						button.Text = "Preview file";
						button.Attributes["style"] = "float:left;display:inline-block;margin:2px;";
						button.ClientInstanceName = "button_preview" + item.Text.ToLower().Replace(" ", "");
						button.ClientSideEvents.Click = "function(s,e){preview(" + existing_files.ID + ".GetValue());}";
						gen_left.Controls.Add(button);

						var button_remove = new ASPxButton();
						button_remove.ID = "button_remove" + item.Text.ToLower().Replace(" ", "");
						button_remove.AutoPostBack = false;
						button_remove.Text = "Remove file";
						button_remove.Attributes["style"] = "float:left;display:inline-block;margin:2px;";
						button_remove.ClientInstanceName = "button_remove" + item.Text.ToLower().Replace(" ", "");
						button_remove.ClientSideEvents.Click = "function(s,e){remove_file(" + existing_files.ID + ".GetValue(), false);}";
						gen_left.Controls.Add(button_remove);
						/*
						ASPxButton button_setdefault				= new ASPxButton();
						button_setdefault.ID						= "button_setdefault"+item.Text.ToLower().Replace(" ", "");
						button_setdefault.AutoPostBack				= false;
						button_setdefault.Text						= "Set file as default";
						button_setdefault.Attributes["style"]		= "float:left;display:inline-block;margin:2px;";
						button_setdefault.ClientInstanceName		= "button_setdefault"+item.Text.ToLower().Replace(" ", "");
						button_setdefault.ClientSideEvents.Click	= "function(s,e){setdefault("+existing_files.ID+".GetValue(), "+item.Value+","+combo_type.SelectedIndex+", false);}";
						gen_left.Controls.Add(button_setdefault);
						*/
						var chkbox_needupload = new ASPxCheckBox();
						chkbox_needupload.ID = "chkbox_needupload" + item.Text.ToLower().Replace(" ", "");
						chkbox_needupload.AutoPostBack = false;
						chkbox_needupload.ForeColor = System.Drawing.Color.Red;
						chkbox_needupload.Text = "User needs to upload this file back";
						chkbox_needupload.Attributes["style"] = "float:left;display:inline-block;margin:2px;";
						chkbox_needupload.ClientInstanceName = "chkbox_needupload" + item.Text.ToLower().Replace(" ", "");
						chkbox_needupload.Checked = cat.upload_required;
						//chkbox_needupload.ClientSideEvents.CheckedChanged	= "function(s,e){alert(1);}";
						gen_left.Controls.Add(chkbox_needupload);

						p.Controls.Add(gen_right);
						p.Controls.Add(gen_left);

						if ((combo_type.SelectedItem.Value.ToString() == "RENEW" || combo_type.SelectedItem.Value.ToString() == "NEWHIRE") && item.Value.ToString() == "2")
						{
						}
						else
						{
							pc_content.TabPages.Add(p);
						}
					}
				}
			}
			try
			{
				if (active_tab >= 0 && pc_content.TabPages.Count > 0 && pc_content.TabPages.Count > active_tab && pc_content.TabPages[active_tab] != null)
				{
					pc_content.ActiveTabIndex = active_tab;
				}
			}
			catch (Exception ee)
			{
				_tools.catch_error(ee);
			}
		}
		else
		{
			lb_tabs.Items.Clear();
			lb_tabs.Items.Add(new ListEditItem("Select All", 9999));
			lb_tabs.Items.Add(new ListEditItem("Deselect All", 9998));
			switch (combo_type.Value.ToString())
			{
				case "NEWHIRE":
					var _newhire = Toolbox.doSQL_dt(@"SELECT * FROM member_fvr_tab  WHERE type = 'NEWHIRE' ORDER BY tab_index, name", null);
					foreach (DataRow dr in _newhire.Rows)
					{
						var name = dr["name"].ToString();
						var tab_index = Convert.ToInt32(dr["tab_index"]);
						var id = Convert.ToInt32(dr["id"]);
						if (tab_index == 1)
						{
							name += " (FORM)";
						}
						lb_tabs.Items.Add(new ListEditItem(name, id));
					}
					break;
				case "EXIT":
					var _exit = Toolbox.doSQL_dt(@"SELECT * FROM member_fvr_tab  WHERE type = 'EXIT' ORDER BY tab_index, name", null);
					foreach (DataRow dr in _exit.Rows)
					{
						var name = dr["name"].ToString();
						var tab_index = Convert.ToInt32(dr["tab_index"]);
						var id = Convert.ToInt32(dr["id"]);
						lb_tabs.Items.Add(new ListEditItem(name, id));
					}
					break;
				case "RENEW":
					var _renew = Toolbox.doSQL_dt(@"SELECT * FROM member_fvr_tab  WHERE type = 'RENEW' ORDER BY tab_index, name", null);
					foreach (DataRow dr in _renew.Rows)
					{
						var name = dr["name"].ToString();
						var tab_index = Convert.ToInt32(dr["tab_index"]);
						var id = Convert.ToInt32(dr["id"]);
						if (tab_index == 1)
						{
							name += " (FORM)";
						}
						lb_tabs.Items.Add(new ListEditItem(name, id));
					}
					break;
			}
			fill_tabs();
		}
		lb_tabs.Enabled = lb_employees.SelectedItems.Count > 0;
	}
	protected void lb_tabs_SelectedIndexChanged(object sender, EventArgs e)
	{
		var c = lb_tabs.Items.Count;
		var select_all = lb_tabs.Items.FindByValue(9999).Selected;
		var select_none = lb_tabs.Items.FindByValue(9998).Selected;
		var fvr_type = combo_type.Value.ToString();
		if (select_all)
		{
			foreach (ListEditItem li in lb_tabs.Items)
			{
				li.Selected = li.Value.ToString() != "9999" && li.Value.ToString() != "9998";
			}
			lb_tabs.Items.FindByValue(9999).Selected = false;
			if (fvr_type == "NEWHIRE")
			{
				lb_tabs.Items.FindByValue(26).Selected = false;
				lb_tabs.Items.FindByValue(27).Selected = false;
			}
		}
		if (select_none)
		{
			foreach (ListEditItem li in lb_tabs.Items)
			{
				li.Selected = li.Value.ToString() == "9998";
			}
			lb_tabs.Items.FindByValue(9998).Selected = false;
		}
		if (combo_type.Value.ToString() == "NEWHIRE")
		{
			// Only one new hire ack can be clicked.
			var can_on = lb_tabs.Items.FindByValue(5).Selected;
			var us_mi = lb_tabs.Items.FindByValue(26).Selected;
			var us_nc = lb_tabs.Items.FindByValue(27).Selected;
			if ((can_on && us_mi) || (can_on && us_nc) || (us_mi && us_nc))
			{
				lb_tabs.Items.FindByValue(5).Selected = false;
				lb_tabs.Items.FindByValue(26).Selected = false;
				lb_tabs.Items.FindByValue(27).Selected = false;
				l_error.Text = "&bullet; You can only select one 'New Hire Acknowledgement', clearing all selected";
			}
		}
		fill_tabs();
	}
	protected void upload_handler(object sender, FileUploadCompleteEventArgs e)
	{
		var s = (ASPxUploadControl)sender;
		var gv = gv_categories;
		//Getting the length of the fill in bytes
		var fileLength = (int)e.UploadedFile.FileContent.Length;
		//creating an array to store the image as bytes
		var rawdata = new byte[fileLength];
		//using the filestream and converting the image to bits and storing it in //an array
		e.UploadedFile.FileContent.Read(rawdata, 0, (int)fileLength);
		//gv.GetRowValues(gv.EditingRowVisibleIndex, new string[] { "tab_index" }).ToString();
		var tab_index = Convert.ToInt32(gv.GetRowValues(gv.EditingRowVisibleIndex, new string[] { "tab_index" }));
		var _type = gv.GetRowValues(gv.EditingRowVisibleIndex, new string[] { "type" }).ToString();
		var f = new file_store.fileObj();
		f.page_id = Convert.ToInt32(_page_id);
		f.folder_id = member_fvr_hdr.types.ids.IndexOf(_type);
		f.sub_folder_id = tab_index;
		f.mime = e.UploadedFile.ContentType;
		f.name = Path.GetFileNameWithoutExtension(e.UploadedFile.FileName);
		f.ext = Path.GetExtension(e.UploadedFile.FileName).Replace(".", "");
		switch (f.ext)
		{
			case "xls":
				f.mime = "application/vnd.ms-excel";
				break;
			case "xlsx":
				f.mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				break;
			case "doc":
				f.mime = "application/msword";
				break;
			case "docx":
				f.mime = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
				break;
			case "pdf":
				f.mime = "application/pdf";
				break;
			case "bmp":
				f.mime = "image/bmp";
				break;
			case "gif":
				f.mime = "image/gif";
				break;
			case "jpg":
			case "jpeg":
				f.mime = "image/jpeg";
				break;
			case "f4v":
			case "flv":
				f.mime = "video/x-flv";
				break;
			default:
				f.mime = e.UploadedFile.ContentType;
				break;
		}
		f.content = f.ext == "mp4" || f.ext == "flv" || f.ext == "f4v" ? null : rawdata;
		f.save();
		if (f.ext == "mp4" || f.ext == "flv" || f.ext == "f4v")
			{
			var fileServer = Toolbox.app_setting("UNC_global_attachments");
			var file_path = fileServer + "\\videos\\" + f.id + "." + f.ext;
			e.UploadedFile.SaveAs(file_path);
		}
	}
	protected void b_save_Click(object sender, EventArgs e)
	{
		l_error.Text = "";
		// Check branches
		if (lb_company.SelectedItems.Count == 0)
		{
			l_error.Text += "&bullet; Please choose a branch" + "<br/>";
		}
		// Check employees
		if (lb_employees.SelectedItems.Count == 0)
		{
			l_error.Text += "&bullet; Please choose an employee" + "<br/>";
		}
		// Check tabs
		if (lb_tabs.SelectedItems.Count == 0)
		{
			l_error.Text += "&bullet; Please choose some categories" + "<br/>";
		}
		// Check files
		foreach (ListEditItem tab in lb_tabs.SelectedItems)
		{
			var fvr_type = combo_type.SelectedItem.Value.ToString();
			var tab_id = Convert.ToInt32(tab.Value);
			var lb_name = "existing_files" + tab.Text.ToLower().Replace(" ", "");
			var tb_name = "tb_url" + tab.Text.ToLower().Replace(" ", "");
			var files = (ASPxListBox)pc_content.FindControl(lb_name);
			var tb_url = (ASPxTextBox)pc_content.FindControl(tb_name);
			var file_id = files == null || files.SelectedItem == null ? 0 : Convert.ToInt32(files.SelectedItem.Value);
			if ((file_id == 0 && tb_url != null && tb_url.Text == "") && ((fvr_type == "RENEW" && tab_id != 18) || (fvr_type == "NEWHIRE" && tab_id != 2)))
			{
				l_error.Text += "&bullet; Please choose a file for the category: " + tab.Text + " OR provide a URL<br/>";
			}
			else if (tb_url != null && tb_url.Text != "" && !Uri.IsWellFormedUriString(tb_url.Text, UriKind.Absolute))
			{
				l_error.Text += "&bullet; Please enter a valid URL for the category: " + tab.Text + "<br/>";
			}
		}
		if (l_error.Text == "")
		{
			try
			{
				var business_unit_ids = Toolbox.CommaDelimit(lb_company);
				var tabs_needed = Toolbox.CommaDelimit(lb_tabs);
			var hdr = new member_fvr_hdr
				          {
				          req_member_id     = current_user.id,
				          tabs_needed       = tabs_needed,
				          business_unit_ids = business_unit_ids,
				          expire_date       = DateTime.Now.AddDays(30),
				          active            = 1,
				          type              = (string) combo_type.Value
				          };
			hdr.save();
				var master_dtls = new member_fvr_dtl();
				var dtls = new ArrayList();
				var tempmem = 0;
				foreach (ListEditItem li in lb_employees.SelectedItems)  // loop through every employee
				{
					tempmem = 0;
					foreach (ListEditItem tab in lb_tabs.SelectedItems)   // loop through every tab
					{
						if (Toolbox.doSQL_int(@"Select count(id) 
from member_fvr_dtl where dt_insert>(curdate()-interval 14 day) and member_id = @v0", new object[] { li.Value }) == 0)  // if an fvr as never been sent before... 
						{
							var n_info = new NeMember(Convert.ToInt32(li.Value));
							if (n_info.Email != "" && (n_info.hrstatus_id <= 2) && !n_info.sent_welcome_email)   // if they have an email address and if they are new
							{
							    try
							    {
							        if ((n_info.reports_to != 0) && (tempmem == 0))
							        {
							            var this_business_unit = n_info.business_unit;
							            var taxEntityObj = new NeTaxEntity(this_business_unit.tax_entity_id);
							            var newEmployee = new Employee(n_info.id);
							            var this_reports_to_obj = new NeMember(newEmployee.ReportToManager.Member_ID);
							            NESI.BLL.Pages.Employees.NeMemberOffer.SendGoLiveEmail(newEmployee, taxEntityObj, this_reports_to_obj, n_info);
							        }

							    }
							    catch (Exception ee)
							    {
							        Toolbox.do_errorLog(ee);
							        throw ee;
							    }
							}
						}
						var dtl = new member_fvr_dtl();
						dtl.member_id = Convert.ToInt32(li.Value);
						dtl.tab_index_actual = Convert.ToInt32(tab.Value);
						dtl.member_fvr_hdr_id = hdr.id;
						var files = (ASPxListBox)pc_content.FindControl("existing_files" + tab.Text.ToLower().Replace(" ", ""));
						var tb_name = "tb_url" + tab.Text.ToLower().Replace(" ", "");
						var tb_url = (ASPxTextBox)pc_content.FindControl(tb_name);
						dtl.file_id = files == null || files.SelectedItem == null ? 0 : Convert.ToInt32(files.SelectedItem.Value);
						dtl.url = tb_url != null ? tb_url.Text : "";
						if (!tab.Text.Contains("(FORM)"))
						{
							var chkbox = (ASPxCheckBox)pc_content.FindControl("chkbox_needupload" + tab.Text.ToLower().Replace(" ", ""));
							dtl.upload_required = chkbox.Checked ? 1 : 0;
						}
						dtls.Add(dtl);



					}
				}
				master_dtls.mass_insert(dtls);

				clear_new();
			}
			catch (Exception ee)
			{
				var eee = new NeEMail();
				eee.To = "debug@" + Toolbox.app_setting("DomainForEmail");
				eee.From = "admin@" + Toolbox.app_setting("DomainForEmail");
				eee.Subject = "something is busted with the FVR sending";
				eee.Body = ee.Message;
				eee.Send();

                Toolbox.do_errorLog(ee);

				l_error.Text = ee.ToString();
			}
		}
	}
	protected void b_refresh_Click(object sender, EventArgs e)
	{
		fill_branches();
		fill_members();
		fill_tabs();
	}
	private void clear_new()
	{
		foreach (ListEditItem i in lb_company.Items)
		{
			i.Selected = false;
		}
		lb_employees.Items.Clear();
		l_totalusers.Text = "";
		foreach (ListEditItem i in lb_tabs.Items)
		{
			i.Selected = false;
		}
		fill_tabs();
	}
	protected void gv_details_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.DataColumn.Caption == "Req ID" && gv.GetDataRow(e.VisibleIndex)["req_id"] != DBNull.Value)
		{
			if (!is_manager_set)
			{
				is_manager = current_user.AuthenticatedForPrivilege(138);
				is_manager_set = true;
			}
			if (is_manager)
			{
				var id = Convert.ToInt32(gv.GetDataRow(e.VisibleIndex)["req_id"]);
				var member_id = Convert.ToInt32(gv.GetDataRow(e.VisibleIndex)["member_id"]);
				var confirmed = Convert.ToBoolean(gv.GetDataRow(e.VisibleIndex)["confirmed"].ToString());
				e.Cell.Text = confirmed ? id.ToString() : string.Format(@"<a href=""javascript:boing('./index.aspx?id={0}&member_id={1}', 'Forms', 960, 768) "">{0}</a>", id, member_id);
			}
		}
		if (e.DataColumn.FieldName == "filename")
		{
			var file_id = gv.GetDataRow(e.VisibleIndex)["file_id"];
			var filename = gv.GetDataRow(e.VisibleIndex)["filename"];
			if ((int)file_id != 0)
			{
				e.Cell.Text = string.Format(@"<a href=""javascript:preview({0})"">{1}</a>", file_id, filename);
			}
		}
	}
	protected void gv_details_HtmlEditFormCreated(object sender, DevExpress.Web.ASPxGridViewEditFormEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		var b = (ASPxButton)gv.FindEditFormTemplateControl("bt_save");
		var cb = (ASPxComboBox)gv.FindEditFormTemplateControl("combo_status");
		var de_expire = (ASPxDateEdit) gv.FindEditFormTemplateControl("expire_date");
		//ASPxListBox lb		= (ASPxListBox)	gv.FindEditFormTemplateControl("lb_file");
		var confirmed = Convert.ToBoolean(gv.GetDataRow(gv_details.EditingRowVisibleIndex)["confirmed"].ToString());
		var id = Convert.ToInt32(gv.GetDataRow(gv_details.EditingRowVisibleIndex)["id"]);
		var type = gv_details.GetDataRow(gv.EditingRowVisibleIndex)["type"].ToString();
		var date_expire = Convert.ToDateTime(gv_details.GetDataRow(gv.EditingRowVisibleIndex)["expire_date"]);
		//file_store fs		= new file_store();
		var dtl = new member_fvr_dtl(id);

		//DataTable dt		= fs.get_files(123, member_fvr_hdr.types.ids.IndexOf(type), dtl.tab_index);
		//if(dt.Rows.Count > 0 && lb != null)
		//	{
		//	foreach(DataRow dr in dt.Rows)
		//		{
		//		lb.Items.Add(dr["name"]+"."+dr["ext"]+" --- "+Convert.ToDateTime(dr["dt"]).ToString("d"), dr["id"]);
		//		}
		//	}
		//if(lb != null && lb.Items.FindByValue(dtl.file_id) != null)
		//	{
		//	lb.Items.FindByValue(dtl.file_id).Selected	= true;
		//	lb.Enabled			= !confirmed;
		//	}
		if (cb != null)
		{
			cb.Enabled = !confirmed;
		}
		if (b != null)
		{
			b.Enabled = !confirmed;
		}
		de_expire.Date = date_expire;
	}
	protected void bt_save_Click(object sender, EventArgs e)
	{
		var cb = (ASPxComboBox)gv_details.FindEditFormTemplateControl("combo_status");
		var id = Convert.ToInt32(gv_details.GetDataRow(gv_details.EditingRowVisibleIndex)["id"]);
		//ASPxListBox lb		= (ASPxListBox) gv_details.FindEditFormTemplateControl("lb_file");
		var dtl = new member_fvr_dtl(id);
		dtl.active = cb.Value.ToString() == "True" ? 1 : 0;
		//dtl.file_id			= (int) lb.Value;
		dtl.save();
		gv_details.DataBind();
		gv_details.CancelEdit();
	}
	protected void gv_details_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		var cb = (ASPxComboBox)gv.FindEditFormTemplateControl("combo_status");
		//ASPxListBox lb		= (ASPxListBox) gv.FindEditFormTemplateControl("lb_file");
	}
	protected void gv_details_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		var cb = (ASPxComboBox)gv.FindEditFormTemplateControl("combo_status");
		var id = Convert.ToInt32(gv.GetDataRow(gv_details.EditingRowVisibleIndex)["id"]);
		var de_expire = (ASPxDateEdit) gv.FindEditFormTemplateControl("expire_date");
		//	ASPxListBox lb		= (ASPxListBox) gv.FindEditFormTemplateControl("lb_file");
		var dtl = new member_fvr_dtl(id);
		dtl.active = cb.Value.ToString() == "True" ? 1 : 0;
		//dtl.file_id			= (int) lb.Value;
		dtl.save();
		var hdr = new member_fvr_hdr(dtl.member_fvr_hdr_id);
		hdr.expire_date = de_expire.Date;
		hdr.save();
		gv_details.DataBind();
		gv_details.CancelEdit();
		e.Cancel = true;
	}
	protected void upload_handler_edit(object sender, FileUploadCompleteEventArgs e)
	{
		var id = Convert.ToInt32(gv_details.GetDataRow(gv_details.EditingRowVisibleIndex)["id"]);
		var dtl = new member_fvr_dtl(id);
		var s = (ASPxUploadControl)sender;
		var fileLength = (int)e.UploadedFile.FileContent.Length;
		var rawdata = new byte[fileLength];
		var fileServer = NeTaxEntity.BaseFolder(current_user.business_unit_id, false);
		e.UploadedFile.FileContent.Read(rawdata, 0, (int)fileLength);
		var f = dtl.file_id == 0 ? new file_store.fileObj() : new file_store.fileObj(dtl.file_id);
		var new_f = new file_store.fileObj();
		new_f.page_id = 123;
		var type = gv_details.GetDataRow(gv_details.EditingRowVisibleIndex)["type"].ToString();
		new_f.folder_id = f.folder_id == 0 ? member_fvr_hdr.types.ids.IndexOf(type) : f.folder_id;
		new_f.sub_folder_id = dtl.tab_index_actual;
		new_f.mime = e.UploadedFile.ContentType;
		new_f.name = Path.GetFileNameWithoutExtension(e.UploadedFile.FileName);
		new_f.ext = Path.GetExtension(e.UploadedFile.FileName).Replace(".", "");
		switch (new_f.ext)
		{
			case "xls":
				new_f.mime = "application/vnd.ms-excel";
				break;
			case "xlsx":
				new_f.mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				break;
			case "doc":
				new_f.mime = "application/msword";
				break;
			case "docx":
				new_f.mime = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
				break;
			case "pdf":
				new_f.mime = "application/pdf";
				break;
			case "bmp":
				new_f.mime = "image/bmp";
				break;
			case "gif":
				new_f.mime = "image/gif";
				break;
			case "jpg":
			case "jpeg":
				new_f.mime = "image/jpeg";
				break;
			case "mp4":
			case "f4v":
			case "flv":
				new_f.mime = "video/x-flv";
				break;
			default:
				new_f.mime = e.UploadedFile.ContentType;
				break;
		}
		new_f.content = new_f.ext == "mp4" || new_f.ext == "flv" || new_f.ext == "f4v" ? null : rawdata;
		new_f.save();
		if (new_f.ext == "mp4" || new_f.ext == "flv" || new_f.ext == "f4v")
		{
			var file_path = Toolbox.app_setting("UNC_global_attachments") + "\\videos\\" + new_f.id + "." + new_f.ext;
			e.UploadedFile.SaveAs(file_path);
		}
	}
	protected void gv_details_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		var id = Convert.ToInt32(e.Values["id"]);
		var confirmed = Convert.ToBoolean(e.Values["confirmed"]);
		if (confirmed)
		{
			throw new Exception("Cannot delete a document that is already confirmed");
			// e.Cancel = true; unreachable code
		}
		else
		{
			// delete history
			Toolbox.doSQL_void(@"DELETE FROM member_fvr_history WHERE member_fvr_dtl_id =@v0 LIMIT 1", id);
			// delete details
			Toolbox.doSQL_void(@"DELETE FROM member_fvr_dtl WHERE id =@v0 LIMIT 1", id);
			gv.DataBind();
			e.Cancel = true;
		}
	}
	protected void gv_details_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			var confirmed = false;
			var active = false;
			var id = e.GetValue("id") == DBNull.Value ? 0 : Convert.ToInt32(e.GetValue("id"));
			bool.TryParse(e.GetValue("confirmed").ToString(), out confirmed);
			bool.TryParse(e.GetValue("active").ToString(), out active);
			if (id != 0)
			{
				if (active)
				{
					e.Row.BackColor = confirmed ? System.Drawing.Color.FromArgb(200, 255, 200) : System.Drawing.Color.FromArgb(255, 200, 200);
				}
				else
				{
					e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
				}
			}
		}
	}
	protected void gv_details_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.Parameters != "")
		{
			gv.LoadClientLayout(e.Parameters);
		}
		else
		{
			gv.FilterExpression = "";
			for (var i = 0; i < gv.Columns.Count; i++)
			{
				if (gv.Columns[i] is GridViewDataColumn)
				{
					var col = (GridViewDataColumn)gv.Columns[i];
					if (col.GroupIndex > -1)
					{
						gv.UnGroup(col);
					}
					col.Visible = true;
				}
			}
		}
	}
	protected void gv_details_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		var confirmed = new object[gv.VisibleRowCount];
		var ids = new object[gv.VisibleRowCount];
		for (var i = 0; i < gv.VisibleRowCount; i++)
		{
			confirmed[i] = gv.GetRowValues(i, "confirmed").ToString() == "True" ? "T" : "F";
			ids[i] = gv.GetRowValues(i, "id");
		}
		e.Properties["cpconfirmed"] = confirmed;
		e.Properties["cpids"] = ids;
		e.Properties["cpeditindex"] = gv.EditingRowVisibleIndex;
		e.Properties["cpmaxindex"] = gv.VisibleRowCount - 1;
	}
	protected void callback_remove_Callback(object source, DevExpress.Web.CallbackEventArgs e)
	{
		var file_id = Convert.ToInt32(e.Parameter);
		var f = new file_store.fileObj(file_id);
		var n = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member_fvr_dtl WHERE file_id = @v0", file_id);
		if (n > 0)
		{
			throw new Exception("Cannot delete file that has been linked to other FVR's");
		}
		else
		{
			n = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member_fvr_tab WHERE default_file_id = @v0", file_id);
			if (n > 0)
			{
				Toolbox.doSQL_void(@"UPDATE member_fvr_tab SET default_file_id = NULL WHERE default_file_id = @v0", file_id);
			}
			Toolbox.doSQL_void(@"UPDATE filestore.files SET page_id = 1 WHERE id = @v0 LIMIT 1", file_id);
		}
	}
	protected void callback_toggleactive_Callback(object source, DevExpress.Web.CallbackEventArgs e)
	{
		var file_id = Convert.ToInt32(e.Parameter);
		var f = new file_store.fileObj(file_id);
		var n = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member_fvr_dtl WHERE file_id = @v0 AND active = 1", file_id);
		if (n > 0)
		{
			throw new Exception("Cannot toggle the active status of a file that is been to active FVR's");
		}
		else
		{
			n = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member_fvr_tab WHERE default_file_id = @v0", file_id);
			if (n > 0)
			{
				Toolbox.doSQL_void(@"UPDATE member_fvr_tab SET default_file_id = NULL WHERE default_file_id = @v0", file_id);
			}
			Toolbox.doSQL_void(@"UPDATE filestore.files SET active = !active WHERE id = @v0 LIMIT 1", file_id);
		}
	}
	protected void callback_setdefault_Callback(object source, DevExpress.Web.CallbackEventArgs e)
	{
		var paras = e.Parameter.Split('|');
		var file_id = Convert.ToInt32(paras[0]);
		var cat_id = Convert.ToInt32(paras[1]);
		var fvr_type = Convert.ToInt32(paras[2]);
		var f = new file_store.fileObj(file_id);
		Toolbox.doSQL_void(@"UPDATE member_fvr_tab 
SET default_file_id = @v0 WHERE tab_index = @v1 AND type = @v2 LIMIT 1", new object[] { file_id, cat_id, member_fvr_hdr.types.ids[fvr_type] });
	}
	protected void gv_details_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		var id = 0;
		var gv = (ASPxGridView)sender;
		int.TryParse(gv.GetRowValues(e.VisibleIndex, "req_id").ToString(), out id);
		var confirmed = gv.GetRowValues(e.VisibleIndex, "confirmed").ToString();
		if (confirmed == "True" && (e.ButtonType == ColumnCommandButtonType.SelectCheckbox || e.ButtonType == ColumnCommandButtonType.Delete))
		{
			e.Enabled = false;
			e.Visible = false;
		}
	}
	protected void gv_categories_DataBound(object sender, EventArgs e)
	{
	}
	protected void gv_categories_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		var combo = (ASPxComboBox)gv.FindRowCellTemplateControl(e.VisibleIndex, (GridViewDataColumn)gv.Columns["default_file_id"], "combo_files");
		var s = (SqlDataSource)gv.FindRowCellTemplateControl(e.VisibleIndex, (GridViewDataColumn)gv.Columns["default_file_id"], "sds_files");
		var this_type = gv.GetRowValues(e.VisibleIndex, new string[] { "type" }).ToString();
		var this_category = gv.GetRowValues(e.VisibleIndex, new string[] { "tab_index" }).ToString();
		var this_default_file_id = gv.GetRowValues(e.VisibleIndex, new string[] { "default_file_id" }).ToString();
		s.SelectParameters["@type"].DefaultValue = this_type;
		s.SelectParameters["@category"].DefaultValue = this_category;
		combo.DataBind();
		if (this_default_file_id != "" && combo.Items.FindByValue(this_default_file_id) != null)
		{
			combo.Items.FindByValue(this_default_file_id).Selected = true;
		}
		else
		{
			combo.SelectedIndex = -1;
		}
	}
	protected void gv_categories_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		var id = Convert.ToInt32(e.Keys["id"]);
		var new_name = e.NewValues["name"].ToString();
		var new_file_id = e.NewValues.Contains("default_file_id")
									? Convert.ToInt32(e.NewValues["default_file_id"])
									: 0;
		var new_upload_req = Convert.ToBoolean(e.NewValues["upload_required"]);
		var cat = new member_fvr_category(id);
		cat.name = new_name;
		cat.default_file_id = new_file_id;
		cat.upload_required = new_upload_req;
		cat.save();
		gv_categories.DataBind();
		gv_categories.CancelEdit();
		e.Cancel = true;
	}
	protected void gv_categories_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		var combo = (ASPxComboBox)gv.FindEditFormTemplateControl("combo_ed_default_file");
		var s = (SqlDataSource)gv.FindEditFormTemplateControl("sds_ed_default_file");
		var s_all = (SqlDataSource)gv.FindEditFormTemplateControl("sds_ed_all_file");
		var this_type = gv.GetRowValues(gv.EditingRowVisibleIndex, new string[] { "type" }).ToString();
		var this_category = gv.GetRowValues(gv.EditingRowVisibleIndex, new string[] { "tab_index" }).ToString();
		var this_default_file_id = gv.GetRowValues(gv.EditingRowVisibleIndex, new string[] { "default_file_id" }).ToString();
		s.SelectParameters["@type"].DefaultValue = this_type;
		s.SelectParameters["@category"].DefaultValue = this_category;
		s_all.SelectParameters["@type"].DefaultValue = this_type;
		s_all.SelectParameters["@category"].DefaultValue = this_category;
		combo.DataBind();
		if (this_default_file_id != "" && combo.Items.FindByValue(this_default_file_id) != null)
		{
			combo.Items.FindByValue(this_default_file_id).Selected = true;
		}
		else
		{
			combo.SelectedIndex = -1;
			combo.Text = "";
		}

	}
	protected void combo_ed_default_file_DataBound(object sender, EventArgs e)
	{
		var combo = (ASPxComboBox)sender;
		combo.Items.Insert(0, new ListEditItem("Remove Default", "0"));
	}
	[WebMethod]
	public static void change_status(int dtl_id, int status_id)
	{
		Toolbox.doSQL_void(@"UPDATE member_fvr_dtl SET status_id = @v1 WHERE id = @v0 LIMIT 1", new object[] { dtl_id, status_id });
	}

	[WebMethod]
	public static void delete_rows(string csv)
	{
		if (!string.IsNullOrEmpty(csv))
		{
			// delete history
			Toolbox.doSQL_void(string.Format(@"DELETE FROM member_fvr_history WHERE member_fvr_dtl_id IN ({0})", csv));
			// delete details
			Toolbox.doSQL_void(string.Format("DELETE FROM member_fvr_dtl WHERE id IN ({0})", csv));
		}
	}
	protected void pop_newbytemplate_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{
		fvr_by_template.DataBind();

	}
	protected void pop_new_WindowCallback(object source, PopupWindowCallbackArgs e)
	{
		if (e.Parameter == "clear")
		{
			lb_company.SelectedIndex = -1;
			lb_employees.Items.Clear();
			lb_tabs.Items.Clear();
			pc_content.TabPages.Clear();
		}
	}

	protected void pop_tesettings_OnWindowCallback(object _source, PopupWindowCallbackArgs _e)
		{
		}
	}