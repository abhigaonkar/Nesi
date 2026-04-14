using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.IO;
using nesi.core;

public partial class FileManager : Page
	{
	NeMember currentUser;
	private const int _page_id = 1;
	private const string Output = "";
	protected NameValueCollection _q;
	int id = 0;
	int address_id = 0;
	string parent_page = "";
	bool show_folders = true;
	string folder = null;

	protected void Page_Load(object sender, EventArgs e)
		{
		_q = Request.QueryString;
		currentUser = Toolbox.do_handle_authentication(1);
		parent_page = string.IsNullOrEmpty(_q["parent_page"]) ? "" : _q["parent_page"];
		int.TryParse(_q["id"], out id);
		int.TryParse(_q["address_id"], out address_id);
		show_folders = string.IsNullOrEmpty(_q["show_folders"]) ? show_folders : _q["show_folders"] == "true";
		folder = string.IsNullOrEmpty(_q["folder"]) ? "" : _q["folder"];
		fm.SettingsFolders.Visible = show_folders;
		Master.FindControl("page_heading").Visible = false;
		var files = new NeFiles();


		if (id == 0)
			{
			Toolbox.FriendlyException(Response, "An ID was not supplied", "");
			}
		if (parent_page == "")
			{
			Toolbox.FriendlyException(Response, "A parent page was not supplied", "");
			}

		string path;

		if (parent_page != "_protected" && parent_page != "Business_Unit_Files")
			{
			if(parent_page == "workorder")
				{
				path = files.GetProjectFolder(id); 
				}
			else
				{ 
				path = address_id == 0
					? files.GetProjectFolder(id, parent_page, folder, currentUser.business_unit_id)
					: files.GetProjectFolder(id, parent_page, folder, address_id, currentUser.business_unit_id);
				}
			}
		else
			{
			path = files.GetProjectFolder(address_id, parent_page, folder, Toolbox.doSQL_int("Select id from business_unit where tax_entity_id = @v0 limit 1", new object[] { id }));
			}
		try
			{
			if (path == "" || !Directory.Exists(path))
				{
				throw new Exception("Folder doesn't exist");
				}
			fm.Settings.RootFolder = path;
			}
		catch
			{
			files.CreateFolder(id, parent_page, currentUser.business_unit_id);
			fm.Settings.RootFolder = path;
			}

		}

	}
