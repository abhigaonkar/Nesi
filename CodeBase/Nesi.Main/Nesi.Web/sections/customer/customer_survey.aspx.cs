using System;
using System.Data;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using nesi.core;

public partial class customer_survey : Page
	{
	private NeMember myMember;
	private const int _page_id = 1; // from Page table in DB
    private Toolbox _tools;
	string contactid = null;
	string wolist = "";
	protected NameValueCollection _q;

	protected void Page_Load(object sender, EventArgs e)
	{
		_tools = new Toolbox();
        //todo add authentication for page
	    if (Session["session"] == null)
	        {
	       Response.Redirect("~/default.aspx");
	        }
		myMember = new NeMember(Session["session"].ToString());
	    _logo.Src = "/images/Logos/nesi-logo-blue-invert.png";

    //	myMember = Toolbox.do_handle_authentication(1);
        _q = Request.QueryString;
		if (_q["c"] != null)
		{
			contactid = _q["c"];
			var c = new NEContact(Convert.ToInt32(contactid));
            
			if ((c.login_enabled == 0 || c.email == ""))
			{
				lnk_getpassword.Text = "If you would like a customer login to " + Toolbox.app_setting("Domain")  + ", click this link";

			}
		    if (_q["remove"] != null)
		        {
		        c.stopsurveys = true;
                c.save();
		        Session["session"] = null;
                Toolbox.FriendlyPopup(Response,"Your account has been remove from future survey requests", "../../default.aspx","Account Change Verified");
                
            }
        }
	  
		if (_q["cc"] != null)
		{
			wolist = _q["cc"].TrimEnd(',');
		}
		if (!IsPostBack)
		{
			Session["gv_customer_survey"] = null;
		}
		fill_grid();
	}

	protected void fill_grid()
	{
		if ((Session["gv_customer_survey"] == null)&&(contactid!=null))
		{
			var dt = _tools.getSQL_datatable(@"call get_survey_list_for_contact(@v0)",new object[] { contactid } );

			if (dt.Rows.Count > 0)
			{
                

                if (wolist != "")
				{

					var dt1 = dt.Select("woprog_id in (" + wolist + ")").Length > 0 ? dt.Select("woprog_id in (" + wolist + ")").CopyToDataTable() : null;
					dt = dt1;
					if (dt.Rows.Count > 0)
					{
					    NeWOProg wo = new NeWOProg(Convert.ToInt32(dt.Rows[0]["woprog_id"]));
					    _logo.Src = "/images/Logos/" + new NeBusinessUnit(wo.business_unit_id).logo_file;
                        if (dt.Rows[0]["survey_finished"] != DBNull.Value)
						{
				//			gv.Enabled = false;
				//			lblsdate.Text = "*** THIS SURVEY IS CLOSED.  It was completed on: " + Convert.ToDateTime(dt.Rows[0]["survey_finished"]).ToString("yyyy-MM-dd");
				//			ASPxButton1.ClientEnabled = false;
							
						}
						else
						{
				//			ASPxButton1.ClientEnabled = true;
							
						}
					}
				}
				else
				{

				}
			}

			Session["gv_customer_survey"] = dt;
		
		}
		gv.DataSource = Session["gv_customer_survey"];
		gv.DataBind();
		if (contactid != null)
		{
			s_comp.Text = _tools.getSQL_string(@"Select count(id) from wo_survey  where contact_id =@v0", new object[] { contactid });
			lblname.Text = new NEContact(Convert.ToInt32(contactid)).name;
		}

	}

	protected void email_andy(string stuff)
	{
		var e = new NeEMail();
		e.To = "Mhyde@" + Toolbox.app_setting("DomainForEmail");
		e.Subject = "customer survey email failure";
		e.Body = stuff;
		e.Send();
	}

	protected void cb_Callback(object sender, CallbackEventArgsBase e)
	{
		var contact = new NEContact(Convert.ToInt32(contactid));
		var customer = new NECustomer(Convert.ToInt32(contact.customer_id));
		var isr = new NeMember(Convert.ToInt32(new NEAddress(contact.address_id).csp.isr_member_id));
		var osr = new NeMember(Convert.ToInt32(new NEAddress(contact.address_id).csp.osr_member_id));
		var ram = new NeMember(Convert.ToInt32(new NEAddress(contact.address_id).csp.ram_member_id));

		var bm = new NeBusinessUnit(customer.business_unit_id).branch_manager;
		var pm = new NeMember(Convert.ToInt32(new NEAddress(contact.address_id).csp.project_mgr_member_id));

		if (e.Parameter.Length > 1)
		{
			var type = e.Parameter.Split('|').GetValue(0).ToString();
			var woid = e.Parameter.Split('|').GetValue(1).ToString();
			var value = e.Parameter.Split('|').GetValue(2).ToString();
			
			
			var is_update = _tools.getSQL_int(@"Select ifnull((select id from wo_survey  where woprog_id =@v0), 0)", new object[] { woid});
			if (type == "r")
			{
				if (is_update==0)
				{
					_tools.getSQL_void(@"insert into wo_survey (woprog_id,date,contact_id,rating) values (" + woid + ",now()," + contactid + "," + value + ")");
				}
				else
				{
					_tools.getSQL_void("update wo_survey set rating=" + value + " where woprog_id=" + woid);
				}
			}
			else if (type == "n")
			{
				if (is_update == 0)
				{
					_tools.getSQL_void(@"insert into wo_survey (woprog_id,date,contact_id,notes) 
values (@v0,now(),@v1,@v2)", new object[] { woid , contactid , value });
				}
				else
				{
					_tools.getSQL_void(@"update wo_survey set notes=@v0  where woprog_id=@v1", new object[] { value, woid});
				}
			}
			else if (type == "1")
			{
				if (is_update == 0)
				{
					_tools.getSQL_void(@"insert into wo_survey (woprog_id,date,contact_id,clean) values (@v0,now(),@v1,@v2)",new object[] {  woid , contactid, value });
				}
				else
				{
					_tools.getSQL_void("update wo_survey set clean=@v0 where woprog_id=@v1" ,new object[] { value, woid});
				}
			}
			else if (type == "2")
			{
				if (is_update == 0)
				{
					_tools.getSQL_void("insert into wo_survey (woprog_id,date,contact_id,ontime) values (@v0,now(),@v1,@v2)", new object[] { woid, contactid, value });
				}
				else
				{
					_tools.getSQL_void("update wo_survey set ontime=@v0 where woprog_id=@v1", new object[] { value, woid });
				}
			}
			else if (type == "3")
			{
				if (is_update == 0)
				{
					_tools.getSQL_void("insert into wo_survey (woprog_id,date,contact_id,callme) values (@v0,now(),@v1,@v2)", new object[] { woid, contactid, value });
				}
				else
				{
					_tools.getSQL_void("update wo_survey set callme=@v0 where woprog_id=@v1", new object[] { value, woid });
					if (value=="1") { cb.JSProperties["cp_alert"] = "When you are finished this survey list, someone will get back to you...";}
                   
				}
			}
            else if (type == "c")
			    {

	        _tools.getSQL_void(@"update woprog set survey_finished = NOW() where woprog_id=@v0",
	            new object[] {woid});
                fill_grid();
            }
		}


	    if (e.Parameter == "p")
			{
				if ((contactid != null) && (contactid != "0"))
				{
					var c = new NEContact(Convert.ToInt32(contactid));
					var email = new NeEMail();
					email.Subject = Toolbox.app_setting("Domain") + "Login Credentials";
					email.isHTML = true;
					email.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
					email.Body = @"<html>
	<head>
		<title></title>
<style type='text/css'>
</style>
	</head>";
					if (c.Contact_Login_enabled == 1)
					{
						

						
						email.Body += @"
	<body>
		<p>
			<font-face='arial' size='4'><span style='font-size:12px;'><span style='font-family:arial,helvetica,sans-serif;'>Please use the following credentials to log onto " + Toolbox.app_setting("Domain") + @" <br />
			</p>";

						if ((c.password != "") && (c.password != null) && (c.email.Length > 6) && (c.email.Contains("@")))
						{
							email.To = c.email;
						//	email.To = "aketelaars@newelectric.com";
							email.Body += @"Username: " + c.email + @"</br>
											Password: " + c.password + @"</br>";
							cb.JSProperties["cp_alert"] = "Please check your email. Your username and password have been sent to " + c.email;
						}
						else
						{
							email.To = isr.NEEmail != "" ? isr.NEEmail : (bm.NEEmail != "" ? bm.NEEmail : "Mhyde@" + Toolbox.app_setting("DomainForEmail"));

							email.CC = "Mhyde@" + Toolbox.app_setting("DomainForEmail");
							email.Body += @"" + c.name + " from " + new NECustomer(Convert.ToInt32(c.Contact_Cust_ID)).Customer_Name + " has requested their login information.  We don't have a valid password saved for them.  Please create one for them and get back to them with it.</br>";
							cb.JSProperties["cp_alert"] = "Your request has been sent.  Someone will get back to you shortly";
						}
						
					}
					else
					{
						email.To = isr.NEEmail != "" ? isr.NEEmail : (bm.NEEmail != "" ? bm.NEEmail : "Mhyde@" + Toolbox.app_setting("DomainForEmail"));
						email.CC = "Mhyde@" + Toolbox.app_setting("DomainForEmail");
						email.Body += @"" + c.name + " from " + new NECustomer(Convert.ToInt32(c.Contact_Cust_ID)).Customer_Name + " has requested a " + Toolbox.app_setting("Domain") + " login! Please create one for them and get back to them with it.</br>";
						cb.JSProperties["cp_alert"] = "Your request has been sent.  Someone will get back to you shortly";
					}
					email.Body += "</span></span></font-face='arial'></body></html>";
					try
					{
						if (email.To == null)
						{
							email.To = "Mhyde@" + Toolbox.app_setting("DomainForEmail");
						}
						email.Send();
					}
					catch
					{
						email_andy("crash while sending credentials email to/for " + c.Contact_ID + " from the customer_survey page");
					}
					
				}
			}
		else if (e.Parameter == "x")
		{
			if ((contactid != null) && (contactid != "0"))
			{
				var c = new NEContact(Convert.ToInt32(contactid));
				var email = new NeEMail();
				
				
				c.stopsurveys = true;
				c.save();
				if (isr.NEEmail != "") { email.To = isr.NEEmail; }
				else if (osr.NEEmail != "") { email.To = osr.NEEmail; }
				else if (ram.NEEmail != "") { email.CC = ram.NEEmail; }


				if (email.To == null)
				{
					email.To = "Mhyde@" + Toolbox.app_setting("DomainForEmail");
				}
				email.CC = new NeBusinessUnit(new NECustomer(Convert.ToInt32(c.customer_id)).business_unit_id).branch_manager.NEEmail;
				email.Bcc = "Mhyde@" + Toolbox.app_setting("DomainForEmail");
				
				email.Subject = c.Contact_Name + " from " + new NECustomer(Convert.ToInt32(c.Contact_Cust_ID)).Customer_Name + " has been removed from the customer survey program after requesting for this change.";
				try
				{
					email.Send();
				}
				catch
				{
					email_andy("crash while sending survey cancellation email from " + c.Contact_ID + " from the customer_survey page");
				}
				cb.JSProperties["cp_alert"] = "Your have been removed from the customer survey email list.";
			}
		}

		
	}
	protected int GetRatingValue(object value)
	{
		return value != DBNull.Value ? Convert.ToInt32(value) : 0;
	}

	protected void rc_Init(object sender, EventArgs e)
	{
		var due = sender as ASPxRatingControl;
		var container = due.NamingContainer as GridViewDataItemTemplateContainer;
	    due.Enabled = (gv.GetRowValues(container.VisibleIndex,"survey_finished") == DBNull.Value);
		due.ClientSideEvents.ItemClick = string.Format("function (s, e) {{ cb.PerformCallback('r|{0}|' + s.GetValue()); }}", container.KeyValue);
	}

	protected void notes_Init(object sender, EventArgs e)
	{
		var due = sender as ASPxMemo;
		var container = due.NamingContainer as GridViewDataItemTemplateContainer;
		due.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb.PerformCallback('n|{0}|' + s.GetText()); }}", container.KeyValue);
	}
	protected void chk1_Init(object sender, EventArgs e)
	{
		var due = sender as ASPxCheckBox;
		var container = due.NamingContainer as GridViewDataItemTemplateContainer;
		due.ClientSideEvents.ValueChanged = string.Format("function (s, e) {{ cb.PerformCallback('1|{0}|' + s.GetValue()); }}", container.KeyValue);
	}
	protected void chk2_Init(object sender, EventArgs e)
	{
		var due = sender as ASPxCheckBox;
		var container = due.NamingContainer as GridViewDataItemTemplateContainer;
		due.ClientSideEvents.ValueChanged = string.Format("function (s, e) {{ cb.PerformCallback('2|{0}|' + s.GetValue()); }}", container.KeyValue);
	}
	protected void chk3_Init(object sender, EventArgs e)
	{
		var due = sender as ASPxCheckBox;
		var container = due.NamingContainer as GridViewDataItemTemplateContainer;
		due.ClientSideEvents.ValueChanged = string.Format("function (s, e) {{ cb.PerformCallback('3|{0}|' + s.GetValue()); }}", container.KeyValue);
	}
	protected void ASPxButton1_Click(object sender, EventArgs e)
	{
		Session["gv_customer_survey"] = null;
		fill_grid();
		var strbody = @"<html>
	<head>
		<title></title>
<style type='text/css'>
table.gridtable {
	font-family: verdana,arial,sans-serif;
	font-size:11px;
	color:#333333;
	border-width: 1px;
	border-color: #666666;
	border-collapse: collapse;
}
table.gridtable th {
	border-width: 1px;
	padding: 8px;
	border-style: solid;
	border-color: #666666;
	background-color: #dedede;
}
table.gridtable td {
	border-width: 1px;
	padding: 8px;
	border-style: solid;
	border-color: #666666;
	background-color: #ffffff;
}
</style>
	</head>
	<body>
		<p>
			<font-face='arial' size='4'><span style='font-size:11px;'><span style='font-family:arial,helvetica,sans-serif;'>This survey was automatically sent to this contact after one month. This customer gets this survey every month. If you wish for this to not happen, you must turn off the auto survey feature found on the customer page.</span></span></font-face='arial'><br />
			&nbsp;</p>
		<table class='gridtable' cellpadding='1' font-face='Arial' font-size='10pt' border-collapse: collapse;
  border-spacing: 0;>
			<tbody>
				<tr>
					<th>
						<strong><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>Work Order</span></span></strong></th>
					<th>
						<strong><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>Description</span></span></strong></th>
					<th style='text-align: center;'>
						<strong><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>Project Manager</span></span></strong></th>
					<th style='text-align: center;'>
						<strong><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>Close Date</span></span></strong></th>
					<th style='text-align: center;'>
						<strong><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>Service Call</span></span></strong></th>
					<th style='text-align: center;'>
						<strong><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>Customer Rating</span></span></strong></th>
					<th style='text-align: center;'>
						<strong><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>Clean?</span></span></strong></th>
					<th style='text-align: center;'>
						<strong><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>On Time?</span></span></strong></th>
					<th style='text-align: center;'>
						<strong><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>Wants to be Called?</span></span></strong></th>
					<th style='text-align: center;'>
						<strong><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>Notes</span></span></strong></th>
				</tr>
";
		var dt = (DataTable)Session["gv_customer_survey"];

		if (contactid == "0")
		{
			if (dt.Rows.Count > 0)
			{
				contactid = dt.Rows[0]["WOProg_Contact_ID"].ToString();
			}
		}
		var contact = new NEContact(Convert.ToInt32(contactid));
		var customer = new NECustomer(Convert.ToInt32(contact.Contact_Cust_ID));
		foreach (DataRow dr in dt.Rows)
		{
			var woid = dr["woprog_id"].ToString();
				var wo = new NeWOProg(Convert.ToInt32(woid));
				var pm = new NeMember(Convert.ToInt32(wo.intProjectManager));
				var cust = new NECustomer(Convert.ToInt32(wo.WOProg_Customer_ID));
				_tools.getSQL_void("Update woprog set survey_finished = now() where woprog_id = " + woid);
				
			if (dr["callme"].ToString()=="1")
			{
				var email = new NeEMail();
				

						email.To = pm.NEEmail;
						email.CC = pm.business_unit.branch_manager.NEEmail;
				//email.To = "aketelaars@newelectric.com";
				email.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
				email.Subject = contact.Contact_Name + " from " + wo.CustomerName + " would like you to call them regarding WO:" + wo.OrderNumber;
				email.Body = "From the online survey for WO: " + wo.OrderNumber + " - '" + wo.Description + "', " + contact.Contact_Name + " would like you to contact them.  " + System.Environment.NewLine + System.Environment.NewLine;
				email.Body += "Their cell: " + contact.cellphone + System.Environment.NewLine;
				email.Body += "Their email: " + contact.email + System.Environment.NewLine;
				email.Body += "Their direct line: " + contact.direct_line + System.Environment.NewLine;

				email.Send();
			}

			strbody += @"<tr>
						<td valign='top'><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>" + wo.OrderNumber + @"</span></span></td>
						<td valign='top'><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>" + wo.Description + @"</span></span></td>
						<td valign='top' style='text-align: center;'><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>" + wo.strProjectManager + @"</span></span></td>
						<td valign='top' style='text-align: center;' nowrap:'nowrap'><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>" + wo.woprog_CloseDateTime.ToString("yyyy-MM-dd") + @"</span></span></td>
						<td valign='top' style='text-align: center;'><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>" + wo.chkServiceCall + @"</span></span></td>
						<td valign='top' style='text-align: center;'><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>" + dr["rating"] + @"</span></span></td>
						<td valign='top' style='text-align: center;'><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>" +  yesno(Convert.ToBoolean(dr["clean"])) + @"</span></span></td>
						<td valign='top' style='text-align: center;'><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>" +  yesno(Convert.ToBoolean(dr["ontime"])) + @"</span></span></td>
						<td valign='top' style='text-align: center;'><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>" + yesno(Convert.ToBoolean(dr["callme"])) + @"</span></span></td>
						<td><span style='font-size:10px;'><span style='font-family:arial,helvetica,sans-serif;'>" + dr["notes"] + "</span></span></td></tr>";
		}
		strbody += @"</tbody>
		</table></body>
</html>";
		var bm = new NeBusinessUnit(customer.business_unit_id).branch_manager;
		var email2 = new NeEMail();
				email2.To = bm.NEEmail;
				email2.CC = "Mhyde@" + Toolbox.app_setting("DomainForEmail");
	//	email2.To = "aketelaars@newelectric.com";
		email2.Subject = contact.Contact_Name + " from " + new NECustomer(Convert.ToInt32(contact.customer_id)).Customer_Name + " completed a survey!";
		email2.Body = strbody;
		email2.isHTML = true;
		email2.From = "administrator@" + Toolbox.app_setting("DomainForEmail");
		email2.Send();
		Session["gv_customer_survey"] = null;
		var csm = Page.ClientScript;
		csm.RegisterClientScriptBlock(this.GetType(), "", "<script> if(window.opener != null){window.opener.location.href = window.opener.location.href; window.close();}else{window.location.href = 'http://www.newelectric.com'; }</script>");


		
	}
	protected string yesno(bool a)
	{
		if (a)
		{
			return "Yes";
		}
		else
		{
			return "No";
		}
	}
	protected void ASPxButton2_Click(object sender, EventArgs e)
	{
		var csm = Page.ClientScript;
		csm.RegisterClientScriptBlock(this.GetType(), "", "<script> if(window.opener != null){window.opener.location.href = window.opener.location.href; window.close();}else{window.location.href = 'http://www.newelectric.com'; }</script>");

	}

    protected void ch_done_Init(object sender, EventArgs e)
    {
    var due = sender as ASPxCheckBox;
    var container = due.NamingContainer as GridViewDataItemTemplateContainer;
    due.Enabled = (gv.GetRowValues(container.VisibleIndex, "survey_finished") == DBNull.Value);
    due.Checked = !due.Enabled;
    due.ClientSideEvents.CheckedChanged = string.Format("function (s, e) {{ cb.PerformCallback('c|{0}|' + s.GetValue()); }}", container.KeyValue);
    }
}
