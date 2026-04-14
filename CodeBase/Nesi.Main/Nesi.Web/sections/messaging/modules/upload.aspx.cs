using System;
using DevExpress.Web;
using System.IO;
using nesi.core;

public partial class sections_messaging_upload : System.Web.UI.Page
	{
        private NeMember current_user;
	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(1);
		}
	protected void upload_handler(object sender, FileUploadCompleteEventArgs e)
		{
		if(combo_category.Value == null)
			{
			throw new Exception("Please select a category");
			}
		var s		= (ASPxUploadControl) sender;
		#region save file
		var fileLength			= (int)e.UploadedFile.FileContent.Length;
		var rawdata			= new byte[fileLength];
		e.UploadedFile.FileContent.Read(rawdata, 0, (int)fileLength);
		var f		= new file_store.fileObj();
		f.page_id				= 153;
		f.folder_id				= 5;
		f.sub_folder_id			= Convert.ToInt32(combo_category.Value);
		f.mime					= e.UploadedFile.ContentType;
		f.name					= System.IO.Path.GetFileNameWithoutExtension(e.UploadedFile.FileName);
		f.ext					= System.IO.Path.GetExtension(e.UploadedFile.FileName).Replace(".", "");
		switch (f.ext)
			{
			case "mp4":
				f.mime = "video/mp4";
			break;
			case "f4v":
				throw new Exception("File must be an Flv file.");
				
			case "flv":
				f.mime = "video/x-flv";
			break;
			}
		f.content = null;
		f.save();
		#endregion save file
		#region save db entry for header
		var v				= new NEVideo();
		v.name					= f.name;
		v.description			= memo_description.Text;
		v.file_id				= f.id;
		v.category_id			= f.sub_folder_id;
		var fileServer = Toolbox.app_setting("UNC_global_attachments");
		var file_path		= fileServer + "\\videos\\"+f.id+"."+f.ext;
		try
			{
			e.UploadedFile.SaveAs(file_path);
			v.save();
			combo_category.SelectedIndex	= -1;
			memo_description.Text			= "";
			}
		catch (Exception ee)	
			{
			// rollback
			Toolbox.doSQL_void(@"DELETE FROM filestore.files WHERE id = @v0 LIMIT 1", f.id);
			if(File.Exists(file_path))
				{
				File.Delete(file_path);
				}
			e.ErrorText					= ee.Message;
			e.IsValid					= false;
			}
		#endregion save db entry for header
		}
	}