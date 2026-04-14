using System;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class sections_member_upload_signback : System.Web.UI.Page
{
	private NeMember current_user;
	private NameValueCollection _q;
	private int isapplicant;
	private int applicantid;
	private int memberid;
	private int pageid;
	private int offerid;

	protected void Page_Load(object sender, EventArgs e)
	{
		var _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(1);
		_q = Request.QueryString;

		isapplicant = (Convert.ToInt32(_q["isapplicant"]));
		applicantid = (Convert.ToInt32(_q["applicantid"]));
		memberid = (Convert.ToInt32(_q["memberid"]));
		pageid = (Convert.ToInt32(_q["pageid"]));
		offerid = (Convert.ToInt32(_q["offerid"]));
		populate_signback_page();
	}
	protected void upload_handler(object sender, FileUploadCompleteEventArgs e)
	{

		var s = (ASPxUploadControl)sender;

		//Getting the length of the fill in bytes
		var fileLength = (int)e.UploadedFile.FileContent.Length;
		//creating an array to store the image as bytes
		var rawdata = new byte[fileLength];
		//using the filestream and converting the image to bits and storing it in //an array
		e.UploadedFile.FileContent.Read(rawdata, 0, (int)fileLength);
		var f = new file_store.fileObj();
		f.page_id = Convert.ToInt32(pageid);

		
			f.folder_id = 3;
			f.sub_folder_id = offerid;

		if (Toolbox.doSQL_int(@"Select count(id) from filestore.files where (folder_id=3 and sub_folder_id=@v0)", offerid) > 0)
		{
			Toolbox.doSQL_void(@"delete from filestore.files where (folder_id=3 and sub_folder_id=@v0)", offerid);
		}

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
			case "f4v":
			case "flv":
				f.mime = "video/x-flv";
				break;
			default:
				f.mime = e.UploadedFile.ContentType;
				break;
		}
	
		f.content = rawdata;
		f.save();

		populate_signback_page();
	}

	protected void populate_signback_page()
	{

		var dt = Toolbox.doSQL_dt(@"Select filestore.files.id from filestore.files  where filestore.files.folder_id = 3 and filestore.files.sub_folder_id =@v0", new object[] { offerid });
		pnl_showfile_html.InnerHtml = "";
		
		
		foreach (DataRow dr in dt.Rows)
		{
		
			var f = new file_store.fileObj(Convert.ToInt32(dr[0]));
//			pnl_showfile.Visible = true;
//			pnl_showfile_html.InnerHtml += string.Format(@"View Uploaded Signback: <button onclick=""boing('/_tools/get_file/index.aspx?file_id={0}', 'uploadedfile', 640,480);"" type='button'>{1}.{2}</button> from {3}", f.id, f.name, f.ext, f.dt.ToString("yyyy-MM-dd")) + "</br>";
			txt_file.ClientVisible = true;
			txt_file.Text = f.name + "." + f.ext + " from " + f.dt.ToString("yyyy-MM-dd");
			txt_file.JSProperties["cp_fid"] = f.id.ToString();
		}
	}

	protected void ASPxButtonEdit1_ButtonClick(object source, ButtonEditClickEventArgs e)
	{
		var fid = txt_file.JSProperties["cp_fid"].ToString();

		if (e.ButtonIndex == 0)
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "AKey", "boing('/_tools/get_file/index.aspx?file_id=" + fid + "','uploadedfile',640,480);", true);
		}
		else if (e.ButtonIndex == 1)
		{
			Toolbox.doSQL_void(@"Delete from filestore.files where id = @v0 limit 1", fid);
			txt_file.Text = "";
			txt_file.ClientVisible = false;
		}
	}
}