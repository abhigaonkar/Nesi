using System;
using System.Data;
using DevExpress.Web;

public partial class sections_hr_fvr_modules_template_page : System.Web.UI.UserControl
	{
	public string PageName	 { get; set; }
	public int thisID  { get; set; }
	public int PageID  { get; set; }
	public int DefaultFileID  { get; set; }
	public int TypeID  { get; set; }
	public int PageIndex  { get; set; }
	public bool UploadRequired  { get; set; }
	public string bgColor  { get; set; }
	public void Page_Init(object sender, EventArgs e)
		{
		load_files();
		//chk_isrequired.Checked					= UploadRequired;
		hid_pageid.Value						= PageID.ToString();
		hid_id.Value							= thisID.ToString();
		}
	public void Page_Load(object sender, EventArgs e)
		{
		if(TypeID == 0 && PageName.StartsWith("New Hire Acknowledgement"))
			{
			chk_enablepage.Attributes["onclick"]	= "fvr_template.toggle_page(this, true, true);fvr_template.newhire_control(this, true);";
			chk_enablepage.Attributes["class"]		= "newhireack";
			lnk_pagename.Attributes["onclick"]		= "fvr_template.toggle_page(this, false, true);fvr_template.newhire_control(this, false);";
			}
		else if(PageName == "Employee Information")
			{
			chk_enablepage.Attributes["onclick"]	= "fvr_template.toggle_page(this, true, false)";
			lnk_pagename.Attributes["onclick"]		= "fvr_template.toggle_page(this, false, false)";
			}
		else
			{
			chk_enablepage.Attributes["onclick"]	= "fvr_template.toggle_page(this, true, true)";
			lnk_pagename.Attributes["onclick"]		= "fvr_template.toggle_page(this, false, true)";
			}
		}
	private void load_files()
		{
		var fs							= new file_store();
		var _files						= fs.get_header_fileinfo(123, TypeID, PageIndex);
		foreach(DataRow _file in _files.Rows)
			{
			var li						= new ListEditItem();
			var date						= Convert.ToDateTime(_file["dt"]);
			li.Value							= Convert.ToInt32(_file["id"]);
			var is_default					= (int) li.Value == DefaultFileID ? "(default) " : "";
			li.Text								= (is_default+_file["name"]+"."+_file["ext"]+" --- Date: "+date.ToString("G")).Trim();
			list_files.Items.Add(li);
			}
		}
	}