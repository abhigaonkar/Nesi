using System;
using System.Web.UI;
using DevExpress.Web;
using System.Collections.Specialized;
using nesi.core;

public partial class quote_startframe : Page
	{
	NeMember current_user;
	private const int _page_id = 65; // from Page table in DB
	private const string _page_name = "Quotes";
	NameValueCollection _q;

	private Toolbox _tools = new Toolbox();

	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		chk_lockquoter.ClientEnabled = current_user.AuthenticatedForPrivilege(61);
		}
	protected void Page_Load(object sender, EventArgs e)
		{
			_q = Request.QueryString;
		if (!IsPostBack)
			{
		fillgrid();

		lblerror.Visible = false;
		lblerror.InnerHtml = "";
		ddlbranch.Value = current_user.business_unit_id;
		//ddldept.Value = Convert.ToInt32(current_user.Division);
		ddlpm.Value = current_user.id;

            ASPxDateEdit1.MinDate = DateTime.Now.AddDays(-7);
            ASPxDateEdit2.MinDate = DateTime.Now.AddDays(-7);
            SqlDataSource3.SelectCommand = "Select id, ddl_name from business_unit where find_in_set(id,'" + new Current_User().visible_business_units + "') order by ddl_name";
            SqlDataSource5.SelectCommand = @"Select a.member_id , concat(member_fullname,' (',bu.ddl_name,')') _name 
from member a
inner join business_unit bu on bu.id = a.business_unit_id
LEFT JOIN
	memberpage b
	ON a.member_id = b.memberpage_member_id 
where find_in_set(a.business_unit_id,'" + new Current_User().visible_business_units + "') and b.memberpage_page_id = 65 AND a.member_status = 'Active' order by bu.ddl_name,a.member_fullname"; 
        }

    }
	protected void fillgrid()
	{
		cbl.DataSource = _tools.getSQL_datatable(@"Select id,filter_question question from quote_filter_questions  where status = 'Active'" , null);
		cbl.DataBind();

	}
	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{

	}

	protected void ASPxComboBox1_ItemsRequestedByFilterCondition(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
	{
		try
		{
			var comboBox = (ASPxComboBox)source;
			SqlDataSource2.SelectCommand = @"SELECT
customer.Customer_ID id,
Concat('(',customer.Customer_Number,') ',customer.Customer_Name,(if(customer.Customer_Hold='T','- On Hold','- Active'))) _name
FROM
customer WHERE Concat('(',customer.Customer_Number,')',customer.Customer_Name,(if(customer.Customer_Hold='T','- On Hold','- Active'))) like @filter order by customer.Customer_Name ";
			//	   @"SELECT [ID], [Phone], [FirstName], [LastName] FROM (select [ID], [Phone], [FirstName], [LastName], row_number()over(order by t.[LastName]) as [rn] from [Persons] as t where (([FirstName] + ' ' + [LastName] + ' ' + [Phone]) LIKE @filter)) as st where st.[rn] between @startIndex and @endIndex";

			SqlDataSource2.SelectParameters.Clear();
			SqlDataSource2.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
			//	SqlDataSource2.SelectParameters.Add("startIndex", TypeCode.int, (e.BeginIndex + 1).ToString());
			//	SqlDataSource2.SelectParameters.Add("endIndex", TypeCode.int, (e.EndIndex + 1).ToString());
			comboBox.DataSource = SqlDataSource2;
			comboBox.DataBind();
		}
		catch { }

	}
	protected void ASPxComboBox2_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		if (e.Parameter.Contains("Add"))
		{
			
		
		}
		else
		{
			var comboBox = (ASPxComboBox)sender;
			comboBox.DataSource = new Toolbox().getSQL_datatable(@"Select contact_id id, contact_name _name from contact  where contact_cust_id =@v0 and contact_status = 'Active' order by contact_name", new object[] { e.Parameter });
			comboBox.DataBind();
		}
	}

	protected void ddlpm_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var comboBox = (ASPxComboBox)sender;

		comboBox.DataBind();
		if (comboBox.Items.FindByValue(current_user.id) != null)
		{
			comboBox.JSProperties["cpSelectedIndex"] = comboBox.Items.FindByValue(current_user.id).Index;
		}
		else
		{
			comboBox.JSProperties["cpSelectedIndex"] = null;
		}
		
	}
	protected void btnsave_Click(object sender, EventArgs e)
	{
		double quote_value = 0;
		var quote_id="0";
		if (validate())
		{
			try
			{
			quote_value = Convert.ToDouble(ASPxTextBox1.Text);
			var q = new quote();
			var comp = new NeBusinessUnit(ddlbranch.Value);
			var cust = new NECustomer(Convert.ToInt32(ddlcustomer.Value));
			

			q.business_unit_id = Convert.ToInt32(ddlbranch.Value);
			q.contact_id = Convert.ToInt32(ddlcontact.Value);
			q.cust_id = Convert.ToInt32(ddlcustomer.Value);
			q.date_due = ASPxDateEdit1.Date.ToString("yyyy-MM-dd");	
			q.quoted_by = Convert.ToInt32(ddlpm.Value);
			q.completion_date = ASPxDateEdit2.Date.ToString("yyyy-MM-dd");
			q.txtJobDescription = ASPxMemo1.Text;
			q.address_id = Convert.ToInt32(ddladdress1.Value);
			q.pct_chance = Convert.ToInt32(spn_chances.Value);
			var addr = new NEAddress(q.address_id);
			var expected_value = Convert.ToDouble(ASPxTextBox1.Text);
			if (comp.uses_quote_process == 1)
			{
				quote_id = _tools.getSQL_string(@"CALL StartQuote_Process(@v0 , @v1 , @v2 , @v3 , @v4 , @v5 , @v6 )", new object[] {  q.quoted_by, q.cust_id, q.contact_id, q.date_due, q.txtJobDescription, q.business_unit_id, Convert.ToDouble(ASPxTextBox1.Text) } );
			}
			else
			{
				quote_id = _tools.getSQL_string(@"CALL StartQuote(@v0 , @v1 , @v2 , @v3 , @v4 , @v5 )", new object[] {  q.quoted_by, q.cust_id, q.contact_id, q.date_due, q.txtJobDescription, q.business_unit_id } );
			}
				var status_id		= comp.uses_quote_process == 1 && expected_value >= comp.quote_level_2_start ? 10 : 1;
				Toolbox.doSQL_void(@"UPDATE quote_master SET expected_value = @v0 , completion_date = @v1 , address_id = @v3 , status_id = @v4 , pct_chance=@v5  WHERE quote_id = @v2  AND active_revision = 1 LIMIT 1", new object[] {  expected_value, q.completion_date, quote_id, q.address_id, status_id, q.pct_chance } );

			//	quote_id = "0";
				var body = @"<style type='text/css'>
table.stat td { font-family: Arial, Helvetica, sans-serif; font-size:9pt; }
table.sstat td { font-family: Arial, Helvetica, sans-serif; font-size:9pt; }

</style><table class='stat'><tr><td><b>Opportunity Details:</b></td><td></td></tr>";
				body += "<tr><td><b>Quote ID:</b></td><td><a href='" + Toolbox.app_setting("Domain") + "/sections/member/quote/index.aspx?a=g&quote_id=" + quote_id + "&revision=1'>" + quote_id + "</a></td></tr>";
				body += "<tr><td><b>Business Unit:</b></td><td>" + ddlbranch.Text + "</td></tr>";
				body += "<tr><td><b>Customer:</b></td><td> " + ddlcustomer.Text + "</td></tr>";
				body += "<tr><td><b>Contact:</b></td><td> " + ddlcontact.Text + "</td></tr>";
				body += "<tr><td><b>Raised By:</b></td><td> " + ddlpm.Text + "</td></tr>";
				body += "<tr><td><b>Due Date:</b></td><td> " + q.date_due + "</td></tr>";
				body += "<tr><td><b>Exp Comp Date:</b></td><td> " + q.completion_date + "</td></tr>";
				body += "<tr><td><b>Exp Value:</b></td><td> " + Convert.ToDouble(ASPxTextBox1.Text).ToString("C2") + "</td></tr>";
				body += "<tr><td><b>Chance of Winning:</b></td><td> " + q.pct_chance + "%</td></tr>";
				body += "<tr><td><b>Description:</b></td><td> " + ASPxMemo1.Text + "</td></tr>";
				body += "</td></tr></table><table class='sstat'><tr>";
				var emaillist = "";
				foreach (ListEditItem l in cbl.Items)
				{
					var qq = Convert.ToInt32(l.Value);
					var qu = l.Text;
					var i = l.Selected;

					_tools.getSQL_void(@"Insert into quote_filter (quote_id,date,memberid,question_id,result)  values(@v0,@v1,@v2,@v3,@v4)",new object[] { quote_id,System.DateTime.Today.ToString("yyyy-MM-dd"),current_user.id,qq,Convert.ToInt16(i) } );
					if (_tools.getSQL_int(@"Select kills_quote from quote_filter_questions  where id =@v0", new object[] { qq }) > 0)
					{
						if (!i)
						{
							body += "<td>" + l.Text + "</td><td>" + (i?"Yes":"No") + "</td></tr>";
						}
					}
					else
					{
						body += "<td>" + l.Text + "</td><td>" + (i ? "Yes" : "No") + "</td></tr>";
					}

				}
				body += "</table>";
				var ee = new NeEMail();
				ee.Subject = "Opportunity Alert";
				#region companies not on quote process
				if (comp.uses_quote_process != 1)
				{
					if (current_user.id != comp.branch_manager.id)
					{
						emaillist += comp.branch_manager.NEEmail + ";";
					}
					try
					{
						if (quote_value >= 149000)  // if it's big enough, add the bm's boss and hector and andy to the list
						{
							// send this email with a passport link back into the first approval screen
							if (comp.branch_manager.reports_to != 0)
							{
								emaillist += new NeMember((int) comp.branch_manager.reports_to).NEEmail+";";
							}
							ee.CC = "Mhyde@newelectric.com;";
						}
					}
					catch (Exception eee) { _tools.catch_error(eee);}
				}
				#endregion

				// We should always be notifying the reps when a quote is cut, per ticket 5472
				ee.CC += addr.csp.am_member_id!=0?new NeMember((int) addr.csp.am_member_id).NEEmail + ";":"";
				ee.CC += addr.csp.osr_member_id!=0?new NeMember((int) addr.csp.osr_member_id).NEEmail + ";":"";
				ee.CC += addr.csp.ram_member_id!=0?new NeMember((int) addr.csp.ram_member_id).NEEmail + ";":"";
				ee.CC += addr.csp.isr_member_id!=0?new NeMember((int) addr.csp.isr_member_id).NEEmail + ";":"";

				#region companies on the quote process
				if (comp.uses_quote_process == 1)
				{
					if (quote_value > comp.quote_level_3_start)	
					{
						emaillist += new NeMember((int) comp.branch_manager.reports_to).NEEmail+";";
						ee.Subject = "Level 3 quote requires " + new NeMember((int) comp.branch_manager.reports_to).FullName + "'s approval prior to starting.";
						ee.CC = comp.branch_manager.NEEmail + ";Mhyde@newelectric.com;";
					}
					else if (quote_value > comp.quote_level_2_start)
					{
						emaillist += comp.branch_manager.NEEmail;
						ee.Subject = "Level 2 quote requires " + comp.branch_manager.FullName + "'s approval prior to starting.";
					}
					else
					{
						if (comp.branch_manager != current_user)
						{
							emaillist += comp.branch_manager.NEEmail+";";
						}
					}

				}
				#endregion
						
						
				ee.To = emaillist;
			//	ee.To = "aketelaars@newelectric.com";
			//	ee.CC = "aketelaars@newelectric.com";
				
				ee.isHTML = true;
				ee.Body = body;
				//_tools.catch_error(new Exception(Toolbox.dict_dump(Toolbox.dict_create(ee))));
				if (emaillist != "")
				{
					ee.Send();
				}
				lblerror.InnerHtml = "";


			}
			catch (Exception ex)
			{
				lblerror.InnerHtml = ex.Message;
				lblerror.Visible = true;
				btnsave.ClientEnabled = true;
			}
			if (_q["p"] == "q")
			{
		//		ClientScript.RegisterStartupScript(GetType(), "closepop", "window.parent.popnew.Hide();");
				ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('./index.aspx?a=g&quote_id=" + quote_id + "&revision=1','quote',1020,850)", true);

		
				ScriptManager.RegisterStartupScript(this, typeof(string), "key", "window.parent.popnew.Hide();window.parent.window.location.href = window.parent.window.location.href;", true);
				
			//	Response.Redirect("./index.aspx?a=g&quote_id=" + quote_id + "&revision=" + 1);

			}

		}
		
	}

	
	

	protected bool validate()
	{
		var fail = "";
		if (ddlcustomer.Text == "")
		{
			fail = "You must select a valid customer.";
		}
		if (ddlcontact.Text == "" || ddlcontact.Value == null || (int) ddlcontact.Value == 0)
		{
			fail += "</br>You must select a valid contact.";
		}
		if (ASPxTextBox1.Text == "")
		{
				fail += "</br>You must enter a valid expected sales value.";
		}
		else
			{
			double x		= 0;
			double.TryParse(ASPxTextBox1.Text, out x);
			if(x == 0)
			{
				fail += "</br>You must enter an expected sales value greater than 0";
			}
			}
		if (ASPxDateEdit1.Text=="")
		{
			fail += "</br>You must select a valid expected due date.";
		}
		if (ASPxDateEdit2.Text == "")
		{
			fail += "</br>You must select a valid expected end date.";
		}
		if (ASPxMemo1.Text == "")
		{
			fail += "</br>You must enter a valid description.";
		}
		if (ddlbranch.Text == "")
		{
			fail += "</br>You must select a valid business unit.";
		}
		if (ddlpm.Text == "")
		{
			fail += "</br>You must select a valid project manager.";
		}
		if (fail!="")
		{
			lblerror.Visible = true;
			lblerror.InnerHtml = fail;
			return false;
		}
		return true;

	}
	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters != "")
		{
			gv.DataSource = _tools.getSQL_datatable(@"SELECT quote_master.quote_id AS quoteid, quote_master.active_revision AS rev, quote_status.`status`, get_name(quote_master.quoted_by) AS quotedby, quote_master.job_description AS description, quote_master.customer_id FROM quote_master INNER JOIN quote_status ON quote_master.status_id = quote_status.id  where status_id <6 and quote_master.customer_id =@v0", new object[] { e.Parameters });
			gv.DataBind();
			if (gv.DetailRows.VisibleCount > 0)
			{
				gv.ClientVisible = true;
			}
		}
	}
	
	protected void cb_contact_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
	if (tbnewcontact.Text == "")
		{
			throw new Exception("You must enter a valid contact name");
		}
		if (ddltitle.SelectedIndex <0)
		{
			throw new Exception("You must select a valid title");
		}
		if ((txtcontactemail.Text=="")&&(txtcellphone.Text==""))
		{
			throw new Exception("You must enter a valid cell phone or email address");
		}
		var this_contact = new NEContact();
		this_contact.Contact_Cust_ID = Convert.ToInt32(e.Parameter);
		this_contact.Contact_Type = "Customer";
		this_contact.Contact_Status = "Active";
		this_contact.Contact_Name = tbnewcontact.Text.Trim();
		var c = _tools.getSQL_int(@"SELECT COUNT(*) FROM contact WHERE contact_name = @v0  AND contact_cust_id = @v1  AND contact_type = 'Customer'", new object[] {  tbnewcontact.Text.Trim(), e.Parameter } );
		if (c > 0)
		{
			throw new Exception("Duplicate Contact Detected");
		}
		if (ddltitle.Text != null)
		{
			this_contact.Contact_Title = ddltitle.Text;
		}
		if (txtcontactemail.Text != null)
		{
			this_contact.Contact_Email = txtcontactemail.Text;
		}
		if (txtcellphone.Text != null)
		{
			this_contact.Contact_CellPhone = txtcellphone.Text;
		}
		try
		{
			var temp_address_id			= Toolbox.doSQL_int(@"SELECT IFNULL(MAX(address_id),0) FROM address where address_table = 'Customer' AND address_table_id = @v0 ", new object[] {  e.Parameter } );
			this_contact.address_id		= temp_address_id;

			this_contact.AddNEContact(this_contact);
			popc.ShowOnPageLoad = false;
			hid_newcontact_id.Value		= this_contact.id.ToString();
		
		}
		catch { throw new Exception("There was an error saving this contact");}
	
	}

	protected void chk_lockquoter_CheckedChanged(object sender, EventArgs e)
		{
		var chked = chk_lockquoter.Checked;
		ddlpm.ClientEnabled = !chked;
		var business_unit_id = Convert.ToInt32(ddlbranch.Value);
		if(!chked)
			{
			ddlpm.Value = null;
			ddlpm.DataBind();
			if(business_unit_id == current_user.business_unit_id)
				{
				ddlpm.Value = current_user.id;
				}
			else
				{
				ddlpm.SelectedIndex	= 0;
				}
			}
		}

	protected void ddladdress_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter!=null && e.Parameter != "")
		{
			var comboBox = (ASPxComboBox)sender;
			comboBox.DataSource = new Toolbox().getSQL_datatable(@"Select address_id id, concat(Address_Addr1,' ',Address_City) _addr from address  where address_table = 'Customer' and address_table_id =@v0", new object[] { e.Parameter });
			comboBox.DataBind();
			comboBox.SelectedIndex = 0;
		}
	}
}