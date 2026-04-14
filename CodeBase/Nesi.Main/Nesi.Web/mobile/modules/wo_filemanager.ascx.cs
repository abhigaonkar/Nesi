using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class mobile_modules_wo_filemanager : System.Web.UI.UserControl
	{
	public int woprog_id { get; set; }
	public NeMember current_user {get; set;}
    protected override void OnLoad(EventArgs e)
		{
		DataBinding += populate;
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		DataBind();
		}
	protected void populate(object sender, EventArgs e)
		{
		build_folders("");
		}
	private void build_files(string folder)
		{
		var f				= new NeFiles();
		var sb				= new StringBuilder();
		var contents		= Directory.GetFiles(folder);
		sb.Append("<div class='title'>Files in Folder</div>");
		foreach(var file in contents)
			{
			var fi			= new FileInfo(file);
			sb.AppendFormat(@"
			<div class='file'>
				<button class='whitetext alignleft' onclick=""fm.download('{1}');"" type='button'><img src='/images/ext/{2}.png' width='16' height='16' align='absmiddle' />&nbsp;{0}</button>
			</div>", fi.Name, fi.FullName.Replace("\\", "/"), fi.Extension.TrimStart('.'));
			}
		if(contents.Any())
			{
			files.InnerHtml	= sb.ToString();
			}
		}
	private void build_folders(string _base_folder)
		{
		var f					= new NeFiles();
		var sb					= new StringBuilder();
		var is_base				= false;
		var wo_base_folder		= f.GetProjectFolder(woprog_id, "workorder", null, current_user.business_unit_id).Replace("\\", "/");
		_base_folder			= _base_folder.Replace("\\", "/");
		if(_base_folder == "")
			{
			is_base			= true;
			_base_folder		= wo_base_folder;
			}
		else
			{
			is_base			= _base_folder == wo_base_folder;
			}
		var di_base			= new DirectoryInfo(_base_folder);
		if(!is_base)
			{
			sb.AppendFormat("<div class='working_folder title alignleft' data-path='{1}'>{0}</div>", di_base.Name, _base_folder);
			}
		if(_base_folder != "" && !is_base && di_base.Parent.FullName.Contains(@"\WO"))
			{
			sb.AppendFormat(@"<div class='sub_folder'><button class='whitetext alignleft' onclick=""fm.navigate('{0}');"" type='button'><img src='/images/icon/icon[folder].png' width='16' height='16' align='absmiddle' style='margin-right:7px;' />.. (Up a Folder)</button></div>", di_base.Parent.FullName.Replace("\\", "/"));
			}
		var directories		= Directory.GetDirectories(_base_folder, "*", SearchOption.TopDirectoryOnly);
		foreach(var sf in directories)
			{
			var di			= new DirectoryInfo(sf);
			sb.AppendFormat(@"
			<div class='sub_folder'>
				<button class='whitetext alignleft' onclick=""fm.navigate('{1}');"" type='button'><img src='/images/icon/icon[folder].png' width='16' height='16' align='absmiddle' style='margin-right:7px;' />{0}</button>
			</div>", di.Name, sf.Replace("\\", "/"));
			}
		folders.InnerHtml	= sb.ToString();
		build_files(_base_folder);
		Session["working_folder"]	= _base_folder;
		}

	protected void cbp_fm_OnCallback(object sender, CallbackEventArgsBase e)
		{
		if(e.Parameter.Contains("|"))
			{
			var paras		= e.Parameter.Split('|');
			var action		= paras[0];
			var item		= paras[1];
			switch(action)
				{
				case "navigate":
				case "refresh":
					build_folders(item);
				break;
				}
			}
		}
	protected void up_file_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
		{
		e.UploadedFile.SaveAs(Session["working_folder"]+"/"+e.UploadedFile.FileName);
		}
}