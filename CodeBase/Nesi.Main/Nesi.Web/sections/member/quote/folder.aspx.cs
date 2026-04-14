using System;
using System.IO;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using nesi.core;

public partial class sections_member_quote_folder : Page
	{
	protected void Page_Load(object sender, EventArgs e)
		{
		var _q			= Request.QueryString;
		var _tools					= new Toolbox();
		var current_user			= Toolbox.do_handle_authentication(1);
		Toolbox.StyleReferenceManager.AddStyleLinksToHead(this);
		if(!string.IsNullOrEmpty(_q["a"]) && _q["a"] == "dl")
			{
            Response.Clear();
            var file				= new FileInfo(MapPath(_q["file"]));
            if(!file.Exists)
            	{
            	throw new Exception("File Doesn't Exist");
            	}
			Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(file.FullName);
            Response.End();
			}
		if(string.IsNullOrEmpty(_q["quote_id"]))
			{
			Response.Clear();
			Response.Write("Quote # not supplied");
			Response.End();
			}
		var quoteID					= 0;
		int.TryParse(_q["quote_id"], out quoteID);
		var quoteObj				= new quote(quoteID);
		var root_path				= string.Format(@"{1}\quote_store\{0}", _q["quote_id"], NeTaxEntity.BaseFolder(quoteObj.business_unit_id, false));
		var quote = new quote(Convert.ToInt32(_q["quote_id"]));
		var comp = new NeBusinessUnit(quote.business_unit_id);

		if(!Directory.Exists(root_path))
			{
			Directory.CreateDirectory(root_path);
			}
		if (quote.expected_value >= comp.quote_level_3_start)
		{
			if (!Directory.Exists(root_path + "/Finance Recon"))
			{
				Directory.CreateDirectory(root_path + "/Finance Recon");
			}
			if (!Directory.Exists(root_path + "/Customer Recon"))
			{
				Directory.CreateDirectory(root_path + "/Customer Recon");
			}
			if (!Directory.Exists(root_path + "/Field Recon"))
			{
				Directory.CreateDirectory(root_path + "/Field Recon");
			}
			if (!Directory.Exists(root_path + "/Market Recon"))
			{
				Directory.CreateDirectory(root_path + "/Market Recon");
			}
			if (!Directory.Exists(root_path + "/Post Mortem"))
			{
				Directory.CreateDirectory(root_path + "/Post Mortem");
			}
		}

		fm_quote.Settings.RootFolder	= root_path;
		}
	}
