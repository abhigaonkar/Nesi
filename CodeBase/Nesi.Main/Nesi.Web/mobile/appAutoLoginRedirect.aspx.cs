using System;
using System.Web;
using nesi.core;

namespace mobile
	{
	public partial class index : System.Web.UI.Page
		{

		protected void Page_Load(object sender, EventArgs e)
			{
            
			var user = HttpContext.Current.Request.QueryString["user"];
			var pass = HttpContext.Current.Request.QueryString["pass"];

			if (user != null && pass != null)
				{
                
				//decrypt pass

				var passphrase = "$!18$uv58SjET9QaIH5"; //Should get thisfrom ToolBox
				var username = Toolbox.DecryptString(user, passphrase);
				var password = Toolbox.DecryptString(pass, passphrase);

				credentials_Authenticate(username, password);
				}
			else
				{
				Response.Redirect("index.aspx?from=APP");
				}
			}
		protected void credentials_Authenticate(string username, string password)
			{
			var current_user = new NeMember(username, password);



			// Do they have any open FVR's?
			var frms = member_fvr_hdr.chk_member(current_user.id);
			var n_fvrs = frms.Rows.Count;
			if (n_fvrs > 0 && current_user.business_unit.fvr_lockout)
				{
				// Yes
				var temp_start = current_user.business_unit.country == "CDN" ? new DateTime(2013, 9, 9) : new DateTime(2013, 11, 13);
				// How old is the oldest?
				var oldest_fvr = Toolbox.doSQL_datetime(string.Format(@"
SELECT 
	IFNULL(MIN(b.dt_insert), DATE_SUB('{1}', INTERVAL 1 DAY)) dt
FROM 
	member_fvr_hdr a 
LEFT JOIN 
	member_fvr_dtl b ON a.id = b.member_fvr_hdr_id 
LEFT JOIN 
	member_fvr_history c ON b.id = c.member_fvr_dtl_id 
LEFT JOIN 
	member_fvr_tab d ON b.tab_index = d.tab_index AND a.type = d.type 
WHERE 
	b.member_id = {0} AND 
	a.active = 1 AND 
	IFNULL(c.confirmed, 0) = 0", current_user.id, Toolbox.MySQL_shortdt(temp_start)),null);
				// >= 15 days?
				oldest_fvr = oldest_fvr <= temp_start ? temp_start : oldest_fvr;
				if (DateTime.Now.Subtract(oldest_fvr).Days >= 30)
					{
					// Lockout.
					Response.Redirect("index.aspx?from=APP");
					return;
					}
				// < 15 days?
				else
					{
					if (current_user.Authenticated)
						{
						var session = new ne_session
										{
										ip = Request.UserHostAddress,
										member_id = current_user.id,
										is_active = true,
										is_contact = current_user.isContact,
										res_x = 0,
										res_y = 0
										};
						session.save();

						Session.Add("session", session.id);
						Session.Add("profile", current_user);
						Response.Redirect("index.aspx?from=APP");
						return;
						}
                    
					}
				}
			else
				{
				if (current_user.Authenticated)
					{
					var session = new ne_session
									{
									ip = Request.UserHostAddress,
									member_id = current_user.id,
									is_active = true,
									is_contact = current_user.isContact,
									res_x = 0,
									res_y = 0
									};
					session.save();

					Session.Add("session", session.id);
					Session.Add("profile", current_user);
					}
				Response.Redirect("index.aspx?from=APP");
				return;
				}
			}


		}
	}