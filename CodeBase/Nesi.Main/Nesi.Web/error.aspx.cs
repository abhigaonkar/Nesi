using System;
using System.Web.UI;
using DevExpress.Xpo;
using nesi.core;
using ne_xpo.cs;

public partial class error : Page
	{
	protected void Page_Load(object sender, EventArgs e)
		{
		var _q = Request.QueryString;
		var reply = "You have performed an invalid request";
		using (var uow = new UnitOfWork())
			{
			int priv, page;
			if (!string.IsNullOrEmpty(_q["priv_id"]) && int.TryParse(_q["priv_id"], out priv))
				{
				var p = uow.GetObjectByKey<privilege>(priv);
				if (p != null)
					{
					reply = "Sorry you don't have access to privilege: <br/><b>" + p.privilege_name + "</b></br>" + p.privilege_desc;
					}
				}
			else if (!string.IsNullOrEmpty(_q["page_id"]) && int.TryParse(_q["page_id"], out page))
				{
				var p = uow.GetObjectByKey<page>(page);
				if (p != null)
					{
					reply = "Sorry you don't have access to the page: <br/><b>" + p.page_name + "</b>";
					}
				}
			else
				{
				reply = "No valid page or privilege specified";
				}
			}
		Toolbox.FriendlyException(Response, reply, "/index.html");
		}
	}