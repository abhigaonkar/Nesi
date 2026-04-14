using System;
using System.Collections.Specialized;
using nesi.core;

public partial class flash_photo_index : System.Web.UI.Page
	{
	NeMember current_user;
	private const int _page_id								= 1; // from Page table in DB
	private NeMember user										= new NeMember();
	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools						= new Toolbox();
		current_user						= Toolbox.do_handle_authentication(_page_id);
		var _q				= Request.QueryString;
		if(Request.RequestType == "POST")
			{
			// Form processing
			Response.Clear();
			var photo = Request.Form["imageData"];
			Toolbox.doSQL_void(@"INSERT INTO filestore.photos (photo) VALUES (COMPRESS(@v0 ))", new object[] {  photo } );
			Response.End();
			}
		else if(_q["id"] != null)
			{
			Response.Clear();
			// Check if exists
			var id = 0;
			int.TryParse(_q["id"], out id);
			if(id == 0)
				{
				Response.Write("Invalid Photo");
				}
			else
				{
				var c							= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM filestore.photos WHERE id = @v0 ", new object[] {  id } );
				if(c == 1)
					{
					Response.ContentType			= "Image/jpeg";
					var str_photo				= Toolbox.doSQL_string(@"SELECT CAST(UNCOMPRESS(photo) AS CHAR) FROM filestore.photos WHERE id = @v0  LIMIT 1", new object[] {  id } );
					var photo					= Convert.FromBase64String(str_photo);
					Response.BinaryWrite(photo);
					}
				else
					{
					Response.Write("Invalid Photo");
					}
				}
			Response.End();
			}
		}
	}
