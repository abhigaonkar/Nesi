using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Data;
using System.IO;
using System.Text;
using NESI.Common.Models;
using nesi.core;

public partial class index : Page
	{
	protected void Page_Init(object sender, EventArgs e)
		{
		var currentUser = Toolbox.do_handle_authentication(OpsPage.Home);
		}

    

	protected void Page_Load(object sender, EventArgs e)
		{
		var _q					= Request.QueryString;
		var action				= string.IsNullOrEmpty(_q["a"]) ? "" : _q["a"];
		var from_q				= string.IsNullOrEmpty(_q["from"]) ? "" : _q["from"];
		
		if(from_q == "APP")
			{
			Session["mobile_from"]	= "APP";
            action = "part_management";
            Response.Redirect("/#/home/1/185/default");
			}
		var isNullNesiSession	= Session["session"] == null;
       
		if (action != "" && isNullNesiSession)
			{
			Response.Redirect("/default.aspx");
			}

		if (action != "" && !isNullNesiSession)
			{
			switch(action)
				{
				case "workorder":
				    Page.Title		= "Work Orders";
				    Toolbox.load_ctrl(Page, "modules/workorder.ascx", ctrlbase);
				    break;
				case "tickets":
				    Page.Title = "Tickets";
				    Toolbox.load_ctrl(Page, "modules/tickets.ascx", ctrlbase);
				    break;
				case "new_ticket":
				    Page.Title = "New Ticket";
				    Toolbox.load_ctrl(Page, "modules/new_ticket.ascx", ctrlbase);
				    break;
				case "scheduler":
				    Page.Title = "Scheduler";
				    Toolbox.load_ctrl(Page, "modules/schedule.ascx", ctrlbase);
				    break;
				case "timesheet":
				    Page.Title		= "Timesheet";
				    //Toolbox.load_ctrl(Page, "modules/timesheet.ascx", ctrlbase);
				    Response.Write("<script> top.location.href = '/#/home/28/timesheet';</script>");
				    break;
				case "inventory":
				    Page.Title = "Inventory";
				    if (_q["id"] != null)
				    {
				        Session["inventory_master_id"] = _q["id"];
				    }
				    Toolbox.load_ctrl(Page, "modules/Inventory.ascx", ctrlbase);
				    break;
				case "part_management":
				    Page.Title		= "Part Management";
				    Toolbox.load_ctrl(Page, "modules/part_management.ascx", ctrlbase);
				    break;
				case "part_management2":
				{
				
				    Page.Title = "Part Management";
				    Toolbox.load_ctrl(Page, "modules/part_management2.ascx", ctrlbase);
				}
				    break;
				case "invoice_preview":
				    Page.Title		= "Approval";
				    if(!string.IsNullOrEmpty(_q["woprog_id"]))
				    {
				        var ctrl	= (shared.mobile_subpage) Page.LoadControl("modules/invoice_preview.ascx");
				        ctrl.this_woprog_id				= Convert.ToInt32(_q["woprog_id"]);
				        ctrlbase.Controls.Clear();
				        ctrlbase.Controls.Add(ctrl);
				    }
				    break;
				case "signoff":
				    Page.Title		= "Customer Signoff";
				    if(!string.IsNullOrEmpty(_q["woprog_id"]))
				    {
				        var ctrl	= (shared.mobile_subpage) Page.LoadControl("modules/signoff.ascx");
				        ctrl.this_woprog_id				= Convert.ToInt32(_q["woprog_id"]);
				        ctrl.@from						= string.IsNullOrEmpty(_q["from"]) ? "" : _q["from"];
				        ctrlbase.Controls.Clear();
				        ctrlbase.Controls.Add(ctrl);
				    }
				    else
				    {
				        Toolbox.load_ctrl(Page, "modules/signoff.ascx", ctrlbase);
				    }
				    break;
				case "releases":
				    Page.Title		= "Releases";
				    Toolbox.load_ctrl(Page, "modules/releases.ascx", ctrlbase);
				    break;
				case "contact_detail":
				    Page.Title		= "Contact Detail";
				    if(string.IsNullOrEmpty(_q["id"]) || _q["id"] == "0")
				    {
				        Toolbox.FriendlyException(Response, "Contact ID not defined", "/index.html");
				    }
				    else
				    {
				        Toolbox.load_ctrl(Page, "modules/contact_detail.ascx", ctrlbase);
				    }
				    break;
				case "fvr":
				    Page.Title = "FVR";
				    Toolbox.load_ctrl(Page, "modules/fvr.ascx", ctrlbase);
				    break;
				case "credit_card":
				    Page.Title = "Company Credit Card Purchase";
				    Toolbox.load_ctrl(Page, "modules/company_credit_card.ascx", ctrlbase);
				    break;
				case "download":
				{
				    var path			= _q["path"];
				    var fi			= new FileInfo(path);
				    Response.ContentType = NeFiles.GetMimeType(fi.Extension);
				    Response.AddHeader("content-disposition", "attachment; filename=" + fi.Name);
				    Response.WriteFile(path);
				    Response.End();
				}
				    break;
				case "persist":
				    if(_q["generate"] != null && _q["generate"] == "true")
				    {
				        Toolbox.QuickReponse(Response, generate_inventory());
				    }
				    else
				    {
				        Page.Title = "Persistance Check";
				        Toolbox.load_ctrl(Page, "modules/persistenttest.ascx", ctrlbase);
				    }
				    break;
				case "shopping_cart":
				    Page.Title = "Shopping Cart";
				    Toolbox.load_ctrl(Page, "modules/shopping_cart.ascx", ctrlbase);
				    break;
				}
			handle_loggedout();
			}
		else if(Session["mobile_from"] != null)
			{
			if(Session["mobile_from"].ToString() == "APP")
				{
				if(!isNullNesiSession)
					{
					var sess						= Session["session"].ToString();
					var current_user				= new NeMember(sess);
					var page						= new NEUserPage(current_user.id, 185, 1);
					if(page.active)
						{
						Page.Title = "Part Management";
						Toolbox.load_ctrl(Page, "modules/part_management2.ascx", ctrlbase);
						}
					else
						{
						Page.Title = "Home";
						Toolbox.load_ctrl(Page, "modules/dashboard.ascx", ctrlbase);
						}
                    }
               
                }
            
			}
		else if(!isNullNesiSession)
			{
            var sess							= Session["session"].ToString();
			var sess_c							= Toolbox.doSQL_int(@"SELECT COUNT(id) FROM ne_session WHERE id = @v0 AND is_active = TRUE", sess);
			if(sess_c == 0)
				{
				Response.Redirect("./index.aspx?a=logoff");
				}
			var current_user				= new NeMember(sess);
			if(current_user.business_unit.fvr_lockout)
				{
				var frms						= member_fvr_hdr.chk_member(current_user.id);
				var n_fvrs						= frms.Rows.Count;
				if(n_fvrs > 0)
					{
					var oldest_fvr	= Toolbox.doSQL_datetime(string.Format(@"
SELECT 
	MIN(a.expire_date) dt
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
	IFNULL(c.confirmed, 0) = 0", current_user.id),null);
					var days						= oldest_fvr.Subtract(DateTime.Now).Days;
					Session["shown_warning"]		= true;
					errorbase.InnerHtml				= string.Format(@"
<div class='attention'>
	You currently have {0} FVR(s) which needs to be addressed within the next {1} days.
	<div class='subtext'>
		If these requests are not addressed within the alotted timeframe, all NESI functionality will be disabled, including the ability to enter time.<br/>
	<br/>
		<button type='button' class='whitetext aligncenter' onclick=""location.href='./index.aspx?a=fvr';"">Load FVR(s)</button>
	</div>
</div>", n_fvrs, days);
					}
				}

			    Page.Title = "Home";
			    Toolbox.load_ctrl(Page, "modules/dashboard.ascx", ctrlbase);
			}
        // This is needed if the "session" session variable is null, meaning it no longer can communicate with the normal nesi login procedure.
        // The !IsNewSession is for the built-in login system used just in the mobile section
        // The !IsPostBack is needed so that it doesn't automatically log someone out when they are logging in (NewSession = false, Session = null).
        else if (!Session.IsNewSession && !IsPostBack && string.IsNullOrEmpty(_q["origin"]))
			{
			// While this method does contain everything in the previous "if" statement, the nested logic is needed because the handle_loggedout logic
			// is used in other places.
              
            handle_loggedout();
            
			}
		else if(Session.IsNewSession)
			{
			// This block is hit when the sessionId cookie doesn't match the AUTH cookie.
			// But this is also hit when it is a brand new session.
			// Nothing goes in here... strictly for debugging.
			}
		}
	private void handle_loggedout()
		{
		if(Session["session"] == null && !Session.IsNewSession && !IsPostBack)
			{
			Response.Redirect("default.aspx");
			}
		}
	private string generate_inventory()
		{
		var sb		= new StringBuilder();
		sb.Append("[");
		var parts			= Toolbox.doSQL_dt(@"SELECT a.master_id id, b.description text FROM inventory_item_master a LEFT JOIN inventory_description b ON a.master_id = b.master_id  WHERE a.active = 1" , null);
		var i					= 0;
		foreach(DataRow dr in parts.Rows)
			{
			i++;
			var id				= Convert.ToInt32(dr["id"]);
			sb.AppendFormat(@"{{""id"":{0},""d"":""{1}""}}", id, HttpUtility.HtmlEncode(dr["text"]));//""l"":["
			if(parts.Rows.Count > i)
				{
				sb.Append(",\n");
				}
			}
		sb.Append("]");
		return sb.ToString();
		}
	}