using System;
using System.IO;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class sections_hr_member_modules_discipline_tab : System.Web.UI.UserControl
	{
	Toolbox	_tools		= new Toolbox();
	public int _id {get;set;}
	public int _addedid { get; set; }
	NeMember user;
	NeMember added_by_user;
	protected void Page_Init(object sender, EventArgs e)
	{



		if (!IsPostBack)
			{
				
			}
		hdnaddedid.Value = _addedid.ToString();
		//	populate();
	}

	protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				hdnid.Value = _id.ToString();
				hdnaddedid.Value = _addedid.ToString();
		
			}

		
		}
	public void populate()
		{
			hdnaddedid.Value = _addedid.ToString();
		
			user = new NeMember(Convert.ToInt32(hdnaddedid.Value));
			var dsDisciplinary = new NeDisciplinary(); /* Populate Disciplinarygrid for selected Member with Disciplinary Details*/
			gv_disc.DataSource = dsDisciplinary.disciplinary_summary(Convert.ToInt32(_id));
			gv_disc.DataBind();
			
		}
	protected void bt_savepanel_Click(object sender, EventArgs e)
		{
			#region Validating
	

			#endregion


	//	ScriptManager.RegisterStartupScript(this, this.GetType(), "remove", @"please_wait('stop');", true);

		}

	protected void gv_disc_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		var date = Convert.ToDateTime(e.NewValues["Date"]).ToString("yyyy-MM-dd");
		var memo = e.NewValues["comments"].ToString();
		var id = e.Keys[0].ToString();
		
		_tools.getSQL_void(@"Update membernote set
date = @v0,
comments =@v1 
where membernote_id = @v2 limit 1",
			new object[] { 
				date,memo,id
				});


		var gvd = (GridViewDataColumn)gv_disc.Columns["file_id"];
		var cb = (ASPxCallbackPanel)gv_disc.FindEditRowCellTemplateControl(gvd, "del_cb");
		var auc1 = (ASPxUploadControl)cb.FindControl("auc1");

		if (auc1.UploadedFiles[0].FileName != "")
		{
			_tools.getSQL_void(@"Delete from filestore.files where sub_folder_id =@v0 and page_id = 127 and folder_id = 6", new object[] { id});
			add_file(auc1, Convert.ToInt32(id));
		}
		
		e.Cancel = true;
		gv_disc.CancelEdit();
		_id = Convert.ToInt32(hdnid.Value);
		_addedid = Convert.ToInt32(hdnaddedid.Value);
		populate();
	}

	protected void auc_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
	{
//			string file_path		= "f:\\files\\videos\\"+f.id+"."+f.ext;
//	
//			e.UploadedFile.SaveAs(file_path);
	}
	protected void ASPxButton1_Click(object sender, EventArgs e)
	{
		
				
				var date = dte.Text;
	
				var note = mem.Html;
				if ((dte.Text != "") && (mem.Html != ""))
				{
					var dis = new NeDisciplinary();
					var mem_ = new NeMember(Convert.ToInt32(hdnid.Value));
					
					dis.active = true;
					dis.comments = note;
					dis.business_unit_id = mem_.business_unit.id;
					dis.date = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
					dis.member_id_audit = Convert.ToInt32(hdnid.Value);
					dis.member_note_added_by_member_id = Convert.ToInt32(hdnaddedid.Value);
					dis.member_note_member_id = Convert.ToInt32(mem_.id);
					dis.note_type = "Disciplinary";
					dis.add(mem_.id);
					
					_id = Convert.ToInt32(hdnid.Value);
					_addedid = Convert.ToInt32(hdnaddedid.Value);
					add_file(auc, Convert.ToInt32(dis.member_note_id));
					
					
					var email = new NeEMail();
					var dt_super = NeMember.get_supervisors(mem_.id);
					foreach (DataRow dr_super in dt_super.Rows)
					{
						if (Convert.ToInt32(hdnaddedid.Value) != Convert.ToInt32(dr_super["memberid"]))
						{
							email.To += dr_super["NEEmail"] + ";";
						}
					}

					
					email.CC = "hr@" + Toolbox.app_setting("DomainForEmail");
					email.Subject = "Discipline Note Created";
					email.From = "admin@" + Toolbox.app_setting("DomainForEmail");
				
					email.isHTML = true;
					email.Body = "<font face='Arial'><a href='" + Toolbox.app_setting("Domain") + "/sections/hr/member/index.aspx?id=" + mem_.id + "'>Go to employee page</a><br>";
					email.Body += new NeMember(Convert.ToInt32(dis.member_note_added_by_member_id)).FullName + " added a disciplinary note for " + new NeMember(Convert.ToInt32(mem_.id)).FullName + " on " + dis.date + "<br>";
					email.Body += "<br>Note: <br>";
					email.Body += note + "<br></font>";
/*					if (auc.UploadedFiles[0].FileName != "")
					{

							System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(auc.UploadedFiles[0].FileContent, "disc file");
							email.Attachment = att;
					}
 */ 
					email.Send();

					populate();
				}
	}

	protected void add_file(ASPxUploadControl auc1, int disc_id)
	{

		

		var posted_file = auc1.UploadedFiles[0];

		var file_ext = auc1.UploadedFiles[0].FileName != "" ? Path.GetExtension(posted_file.FileName).Replace(".", "").ToLower() : "";
		var file_mime = auc1.UploadedFiles[0].FileName != "" ? posted_file.ContentType : "";

		#region Save File
		if (auc1.UploadedFiles[0].FileName != "")
		{
			try
			{
				_tools.getSQL_void(@"Delete from filestore.files where sub_folder_id = @v0 and page_id = 127 and folder_id = 6", new object[] { disc_id});
				//Getting the length of the fill in bytes
				var fileLength = (int)auc1.UploadedFiles[0].FileContent.Length;
				//creating an array to store the image as bytes
				var rawdata = new byte[fileLength];
				//using the filestream and converting the image to bits and storing it in //an array
				auc1.UploadedFiles[0].FileContent.Read(rawdata, 0, (int)fileLength);
				var f = new file_store.fileObj();
				f.page_id = 127;  // this marks it as an emaployee page

				f.folder_id = 6;
				f.sub_folder_id = Convert.ToInt32(disc_id);

				f.mime = auc1.UploadedFiles[0].ContentType;
				f.name = System.IO.Path.GetFileNameWithoutExtension(auc1.UploadedFiles[0].FileName);
				f.ext = System.IO.Path.GetExtension(auc1.UploadedFiles[0].FileName).Replace(".", "");
				switch (f.ext)
				{
					case "xls":
						f.mime = "application/vnd.ms-excel";
						break;
					case "xlsx":
						f.mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
						break;
					case "doc":
						f.mime = "application/vnd.ms-word";
						break;
					case "rtf":
						f.mime = "application/vnd.ms-word";
						break;
					case "txt":
						f.mime = "application/vnd.ms-word";
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
					case "png":
						f.mime = "image/png";
						break;
					case "f4v":
					case "flv":
						f.mime = "video/x-flv";
						break;
					default:
						f.mime = auc1.UploadedFiles[0].ContentType;
						break;
				}
				f.content = rawdata;
				f.save();

			}
			catch (Exception ee)
			{
				//						_tools.catch_error(ee);

				return;
			}
		}
		#endregion Save File


	}

	protected void gv_disc_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		_tools.getSQL_void(@"Delete from membernote where membernote_id =@v0  limit 1", new object[] { e.Keys[0]});
		_tools.getSQL_void(@"Delete from filestore.files  where sub_folder_id =@v0 and page_id = 127 and folder_id = 6", new object[] { e.Keys[0] });
		e.Cancel = true;
		_id = Convert.ToInt32(hdnid.Value);
		_addedid = Convert.ToInt32(hdnaddedid.Value);
		populate();
	}
	
	protected void del_file_Init(object sender, EventArgs e)
	{
		var del = sender as ASPxButton;
		var container = del.NamingContainer as GridViewDataItemTemplateContainer;
		del.ClientSideEvents.Click = string.Format("function (s, e) {{ del_cb.PerformCallback('{0}'); }}", gv_disc.GetRowValues(gv_disc.EditingRowVisibleIndex,"file_id"));
		if (gv_disc.GetRowValues(gv_disc.EditingRowVisibleIndex, "file_id") == null)
		{
			del.Visible = false;
		}
		else if (gv_disc.GetRowValues(gv_disc.EditingRowVisibleIndex, "file_id").ToString() == "")
		{
			del.Visible = false;
		}

	}
	protected void del_cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var del_cb = (ASPxCallbackPanel)sender;
		var link = (ASPxHyperLink)del_cb.FindControl("link");
		var del_file = (ASPxButton)del_cb.FindControl("del_file");
	
		if (e.Parameter.StartsWith("refresh"))
		{
			var dt =  _tools.getSQL_datatable(@"Select * from filestore.files  where sub_folder_id =@v0 and page_id = 127 and folder_id = 6", new object[] { e.Parameter.Split('|').GetValue(1) });
			if (dt.Rows.Count > 0)
			{
				link.Text = dt.Rows[0]["name"].ToString();
				link.NavigateUrl = string.Format("javascript:preview({0})", dt.Rows[0]["id"]);
				del_file.ClientVisible = true;
			}
			else
			{
				link.Text = "";
				del_file.ClientVisible = false;
			}
		}
		else if (e.Parameter != "")
		{
			_tools.getSQL_void(@"Delete from filestore.files where id = @v0", new object[] { e.Parameter});
			link.Text = "";
			del_file.ClientVisible = false;
		}

	}
	protected void gv_disc_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{
		_id = Convert.ToInt32(hdnid.Value);
		_addedid = Convert.ToInt32(hdnaddedid.Value);
		populate();
	}
	protected void auc1_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
	{
		var auc1 = (ASPxUploadControl)sender;
		add_file(auc1, Convert.ToInt32(gv_disc.GetRowValues(gv_disc.EditingRowVisibleIndex, "MemberNote_ID").ToString()));
	
	}

	protected void auc1_Init(object sender, EventArgs e)
	{
		var uac = (ASPxUploadControl)sender;
		var container = uac.NamingContainer as GridViewDataItemTemplateContainer;
		uac.ClientSideEvents.FileUploadComplete = string.Format("function (s, e) {{ del_cb.PerformCallback('refresh|{0}'); }}", gv_disc.GetRowValues(gv_disc.EditingRowVisibleIndex,"MemberNote_ID"));
	}
	protected void gv_disc_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		if (user != null)
		{
			if ((_addedid.ToString() == gv_disc.GetRowValues(e.VisibleIndex, "MemberNote_Addedby_Member_ID").ToString()) || (user.business_unit_id == 11))
			{
				e.Visible = true;
			}
			else
			{
				e.Visible = false;
			}
		}
		else
		{
			e.Visible = false;
		}
		
	}
}