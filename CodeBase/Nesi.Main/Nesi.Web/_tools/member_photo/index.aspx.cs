using System;
using System.Web;
using System.IO;
using System.Drawing;
using NESI.Common.Models;
using nesi.core;

public partial class _tools_member_photo_index : System.Web.UI.Page
	{
	NeMember current_user;
	private const int _page_id								= 1; // from Page table in DB
	private NeMember user										= new NeMember();
	protected void Page_Load(object sender, EventArgs e)
		{
		current_user						= Toolbox.do_handle_authentication(OpsPage.Home);
		var _q				= Request.QueryString;
		int.TryParse(_q["member_id"], out var this_user_id);
		var this_user					= new NeMember();
		if(this_user_id > 0)
			{
			this_user					= new NeMember(this_user_id);
			user						= this_user;
			}
		if(string.IsNullOrEmpty(this_user.LDAP_user) && !string.IsNullOrEmpty(_q["upload_form"]))
			{
			Toolbox.QuickReponse(Response, "This user has not been hooked up to the active directory via the IT tab.");
			}
		if(!current_user.AuthenticatedForPrivilege(124) && !string.IsNullOrEmpty(_q["upload_form"]))
			{
			Toolbox.QuickReponse(Response, "You do not have permission to administer photos.");
			}
		if(string.IsNullOrEmpty(_q["upload_form"]))
			{
			if(this_user_id > 0)
				{
				Response.Clear();
				Response.ContentType			= "Image/jpeg";
				Response.BinaryWrite(GetUserPicture(this_user.LDAP_user));
				Response.End();
				}
			else if(this_user_id == 0)
				{
				Toolbox.FriendlyException(Response, "User not supplied", "/index.html");
				}
			}
		}
	private byte[] GetUserPicture(string userName)
		{
		var bytes = new byte[16];
		var _path						= HttpContext.Current.Server.MapPath("/images/member/");
		var img							= Image.FromFile(_path+"nophoto.png");
		using(var ms = new MemoryStream())
			{
			img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
			bytes	= ms.ToArray();
			}
		return bytes;
		} 
	private void InsertPicture(string userName, byte[] data)
		{
		}
	protected void bt_image_Click(object sender, EventArgs e)
		{
		}
	protected void uc_image_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
		{
		}
}