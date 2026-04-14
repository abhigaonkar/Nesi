using System;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_hr_fvr_modules_upload : System.Web.UI.Page
	{
	private NeMember current_user;
	private NameValueCollection _q;
	private member_fvr_dtl dtl;
	private member_fvr_hdr hdr;
    private member_fvr_history hist;
	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(1);
		_q					= Request.QueryString;
		if(string.IsNullOrEmpty(_q["dtl_id"]))
			{
			Toolbox.FriendlyException(Response, "FVR not defined", "");
			}
		btn_upload_instructions.Attributes["onclick"]	= Request.Browser.IsMobileDevice ? "fvr.client.toggle_upload_instructions(true);" : "fvr.client.toggle_upload_instructions(false);";
		}
	protected void upload_handler(object sender, FileUploadCompleteEventArgs e)
		{
		_q					= Request.QueryString;
		var s = (ASPxUploadControl)sender;
		dtl					= new member_fvr_dtl(Convert.ToInt32(_q["dtl_id"]));
		hdr					= new member_fvr_hdr(dtl.member_fvr_hdr_id);
		hist				= new member_fvr_history(dtl.id);
		//Getting the length of the fill in bytes
		var fileLength = (int)e.UploadedFile.FileContent.Length;
		//creating an array to store the image as bytes
		var rawdata = new byte[fileLength];
		//using the filestream and converting the image to bits and storing it in //an array
		e.UploadedFile.FileContent.Read(rawdata, 0, (int)fileLength);
		var f = new file_store.fileObj();
		f.page_id = 123;
		f.folder_id = member_fvr_hdr.types.ids.IndexOf(hdr.type);
		f.sub_folder_id = Convert.ToInt32("123" + dtl.member_id);
		f.mime = e.UploadedFile.ContentType;
		f.name = System.IO.Path.GetFileNameWithoutExtension(e.UploadedFile.FileName);
		f.ext = System.IO.Path.GetExtension(e.UploadedFile.FileName).Replace(".", "");
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
			case "flv":
				f.mime = "video/x-flv";
			break;
			default:
				f.mime = e.UploadedFile.ContentType;
			break;
			}
		f.content = f.ext == "mp4" || f.ext == "flv" || f.ext == "f4v" ? null : rawdata;
		f.save();
		if(f.ext == "mp4" || f.ext == "flv" || f.ext == "f4v")
			{
			var fileServer = Toolbox.app_setting("UNC_global_attachments");
			var file_path		= fileServer + "\\videos\\"+f.id+"."+f.ext;
			e.UploadedFile.SaveAs(file_path);
			}
		dtl.uploaded_file_id = f.id;
		dtl.save();
		hist.file_uploaded			= 1;
		hist.save();
		}
	}