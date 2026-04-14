using System;
using System.IO;
using System.Web.UI;
using DevExpress.Web;
using System.Net.Mail;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Data;
using nesi.core;


public partial class sections_member_expense_if_newexpense : Page
	{
	NeMember current_user;
	int id = 0;
	NameValueCollection _q;
	payroll.expense this_expense;
	bool _isAdmin;
	bool _isEdit;
	string _visibleBusinessUnits;
	protected void Page_Init(object sender, EventArgs e)
		{
		current_user = Toolbox.do_handle_authentication(1);
		_visibleBusinessUnits = new Current_User().visible_business_units;
		_q = Request.QueryString;
		var isMobile = !string.IsNullOrEmpty(_q["is_mobile"]) && _q["is_mobile"] == "1";
		_isAdmin = current_user.AuthenticatedForPrivilege(175);
		var dt_users = Toolbox.doSQL_dt(@"SELECT a.member_id id, CONCAT(b.ddl_name, ' - ', a.member_fullname) name
FROM member a inner join business_unit b ON a.business_unit_id = b.id where b.id in(" + _visibleBusinessUnits + @") and a.member_termdate > curdate()-interval 1 month
ORDER BY b.ddl_name, a.member_fullname", null);
		cb_user.DataSource = dt_users;
		cb_user.DataBind();
		var sub_users = Toolbox.doSQL_string(@"SELECT IFNULL(REPORTS_TO(@v0),'')", current_user.id);
		if (!_isAdmin && sub_users != "")
			{
			dt_users = Toolbox.doSQL_dt(string.Format(@"SELECT a.member_id id, CONCAT(b.ddl_name, ' - ', a.member_fullname) name 
FROM member a inner join business_unit b ON a.business_unit_id = b.id WHERE (a.member_id IN ({0}) OR a.member_id = @v0 ) and b.id in(" + _visibleBusinessUnits + @") and a.member_termdate > curdate()-interval 1 month
ORDER BY b.ddl_name, a.member_fullname", sub_users), new object[] { current_user.id });
			cb_user.DataSource = dt_users;
			cb_user.DataBind();
			}
		if (!IsCallback && !IsPostBack)
			{
			cb_user.Value = current_user.id;
			}
		var _user = new NeMember(Convert.ToInt32(cb_user.Value));
		cb_user.Enabled = _isAdmin || sub_users != "";
		int.TryParse(_q["id_expense"], out id);
		_isEdit = id > 0;
		if (!IsPostBack && !string.IsNullOrEmpty(_q["id_expense"]) && (_isAdmin || sub_users != ""))
			{
			if (id == 0)
				{
				Toolbox.FriendlyException(Response, "Invalid Expense", "");
				}
			this_expense = new payroll.expense(id);

			cb_user.Value = Convert.ToInt32(this_expense.id_member);
			cb_user.Enabled = false;
			_user = new NeMember(Convert.ToInt32(cb_user.Value));
			t_amount.Value = this_expense.amount.ToString();
			c_category.Value = this_expense.master_id.ToString();
			t_receipt.Value = this_expense.receipt_number;
			c_seller.Value = Toolbox.doSQL_int(@"SELECT MIN(id_seller) FROM expense_seller WHERE name_seller = (SELECT IFNULL(name_seller, '') FROM expense_seller WHERE id_seller = @v0)", this_expense.id_seller);
			memo_description.Text = this_expense.item_text;
			c_currency.Value = this_expense.currency;
			cb_shop.Checked = this_expense.woprog_id == 0;
			se_distance.Value = this_expense.distance;
			de_purchase.Date = this_expense.date_purchased;
			row_receiptlink.Visible = this_expense.has_file;
			tb_attendees.Text = this_expense.attendees;

			if (this_expense.has_file)
				{
				hl_receipturl.NavigateUrl = "javascript:void(0);";

				var fileServer = NeTaxEntity.BaseFolder(_user.business_unit_id, true);
				var strPath = fileServer + @"\expense_receipts\" + this_expense.id_expense + "." + this_expense.file_ext;

				hl_receipturl.ClientSideEvents.Click = "function(s,e){handle_link('" + strPath.Replace(@"\", "/") + "','" + this_expense.file_ext + "');}";
				hl_receipturl.Text = "Link";
				}
			if (Toolbox.doSQL_int(@"SELECT count(id) from gl_te where tax_entity_id = @v0 and is_mileage=1 and account_no =@v1", new object[] { _user.business_unit.tax_entity_id, this_expense.master_id }) > 0)
				{
				// Mileage
				row_distance.Visible = true;
				row_attendees.Visible = false;
				rbl_distance_unit.SelectedIndex = _user.business_unit.country == "CDN" ? 0 : 1;
				}
			else if (this_expense.master_id == 73105)
				{
				// Meals/Entertainment
				row_distance.Visible = false;
				row_attendees.Visible = true;
				}
			else
				{
				row_attendees.Visible = false;
				row_distance.Visible = false;
				}
			if (cb_shop.Checked)
				{
				c_workorder.ClientEnabled = false;
				}
			else
				{
				c_workorder.Value = this_expense.woprog_id;
				}
			ds_workorder.SelectCommand = string.Format(@"
SELECT
	a.woprog_id,
	b.customer_name customer_name,
	a.woprog_bvwo,
	a.woprog_description `description`
FROM
	woprog a
LEFT JOIN
customer b ON a.woprog_customer_id = b.customer_id
WHERE
	a.business_unit_id = '{0}' AND
	a.labor_only=false and 
	a.WOProg_Hold = 0 AND
	a.woprog_bvwo != 'Not Entered'
ORDER BY 
	a.woprog_bvwo DESC
",
				_user.business_unit_id);
			}
		if (isMobile)
			{
			Toolbox.do_add_css(Page, "./mobile.css");
			var height = Unit.Pixel(35);
			de_purchase.Height =
				c_workorder.Height =
				t_amount.Height =
				c_currency.Height =
				c_category.Height =
				t_receipt.Height =
				uc_receipt.Height =
				c_seller.Height =
				tb_attendees.Height = height;
			}
		else
			{
			Toolbox.do_add_css(Page, "./desktop.css");
			}
		b_save.ClientSideEvents.Click = @"
function(s, e) 
	{
	if(btn_clicked) return;
	btn_clicked = true;
	if(s.CauseValidation())
		{
		var amount = t_amount.GetValue();
		if(isNaN(amount))
			{
			e.processOnServer = false;
			alert('Please enter a valid dollar amount');
			btn_clicked = false;
			return;
			}
		else if(amount / 1 > 15000)
			{
			e.processOnServer = false;
			alert('The amount entered is higher than the maximum of $15,000');
			t_amount.Focus();
			btn_clicked = false;
			return;
			}
		else if(" + (!_isEdit).ToString().ToLower() + @" && uc_receipt.GetText() == '' && '" + Toolbox.doSQL_string(@"Select ifnull((SELECT group_concat(account_no) from gl_te where tax_entity_id = @v0 and is_mileage=1),'')", new object[] { _user.business_unit.tax_entity_id }) + @"'.indexOf(c_category.GetValue())==false )
			{
			e.processOnServer = false;
			alert('You must include a physical receipt with this request.');
			btn_clicked = false;
			return;
			}


		}
	}";
		if (!IsPostBack && !IsCallback && string.IsNullOrEmpty(_q["id_expense"]))
			{
			var pp_id = NePayPeriod.CurrentOpenPayPeriodId();
			var pp = new NePayPeriod(pp_id);
			de_purchase.MaxDate = DateTime.Now;
			de_purchase.MinDate = pp.start_date;
			de_purchase.Date = DateTime.Now;
			row_attendees.Visible = false;
			row_distance.Visible = false;
			c_currency.SelectedIndex = current_user.Country == "CAN" ? 1 : 0;
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		fill_workorders();
		}
	private void clear_form()
		{
		de_purchase.Date = DateTime.Now;
		t_receipt.Text = "";
		c_category.SelectedIndex = -1;
		c_seller.SelectedIndex = -1;
		c_workorder.SelectedIndex = -1;
		memo_description.Text = "";
		t_amount.Text = "";
		lb_error.Text = "";
		cb_shop.Checked = false;
		row_distance.Visible = false;
		row_attendees.Visible = false;
		c_currency.SelectedIndex = current_user.Country == "CAN" ? 1 : 0;
		tb_attendees.Text = "";
		se_distance.Value = 0;
		c_seller.DataBind();
		c_workorder.DataBind();
		}
	private void do_error(string e, bool show_up_error)
		{
		var up_e = uc_receipt.UploadedFiles.Length != 0 && show_up_error ? " <b class='upload'>• You may need to re-attach your receipt to upload.</b>" : "";
		lb_error.Text = "- " + e + up_e;
		}
	protected void b_save_Click(object sender, EventArgs e)
		{
		using (var conn = Toolbox.connect())
			{
			double amount = 0;
			double adjusted_amount = 0;
			var is_shop = false;
			uc_receipt.Visible = uc_receipt.Visible;
			var manager = new NeMember();
			var user = new NeMember(Convert.ToInt32(cb_user.Value));
			if (!double.TryParse(t_amount.Text, out amount))
				{
				c_workorder.DataBind();
				do_error("Supplied amount is not a useable number", true);
				return;
				}
			if ((uc_receipt.UploadedFiles.Length == 0 || uc_receipt.UploadedFiles[0].ContentLength == 0)
				&& !_isEdit
				&& Toolbox.doSQL_int(@"SELECT count(id) from gl_te where tax_entity_id = @v0 and is_mileage=1 and account_no =@v1", new object[] { user.business_unit.tax_entity_id, c_category.Value }) == 0)
				{
				c_workorder.DataBind();
				do_error("You must supply a physical receipt", true);
				return;
				}
			is_shop = cb_shop.Checked;
			// Check if expense exists.
			var exp = _isEdit
											? new payroll.expense(id)
											: new payroll.expense();
			var prev_exp = exp;

			var is_new_seller = c_seller.SelectedIndex == -1;
			exp.id_seller = is_new_seller
								? payroll.expense.new_seller(c_seller.Text, current_user.id)
								: Convert.ToInt32(c_seller.Value);
			exp.receipt_number = t_receipt.Text;
			exp.item_text = memo_description.Text;
			exp.amount = amount;
			exp.master_id = Convert.ToInt32(c_category.Value);
			exp.date_purchased = de_purchase.Date;
			exp.date_start = exp.date_purchased;
			exp.date_end = exp.date_purchased;
			exp.attendees = tb_attendees.Text.Trim();
			exp.distance = Convert.ToDouble(se_distance.Value);

			var is_mileage =
				Toolbox.doSQL_int(
					@"SELECT count(id) from gl_te where tax_entity_id = @v0 and is_mileage=1 and account_no =@v1",
					new object[] { user.business_unit.tax_entity_id, exp.master_id }) > 0;

			exp.unit_distance = is_mileage ? rbl_distance_unit.Value.ToString()
											: "";
			exp.id_member = user.id;
			exp.id_payperiod = NePayPeriod.get_payperiod_id(de_purchase.Date);
			// Check if user's payroll is already approved
			var payroll_c		= Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM payroll_hours WHERE member_id = @v0 AND payperiod_id = @v1", new object[] {user.id, exp.id_payperiod});
			if(payroll_c > 0)
				{
				throw new Exception("This user's payroll has already been approved, in order to add this expense, their payroll will first need to be unapproved.");
				}
			exp.currency = c_currency.Value.ToString();
			var _woprog_id = 0;


			if (c_workorder.Value != null)
				{
				int.TryParse(c_workorder.Value.ToString(), out _woprog_id);
				}
			exp.woprog_id = _woprog_id;
			var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM expense_reimbursement WHERE id_payperiod = @v0  AND woprog_id = @v1  AND id_member = @v2  AND amount = @v3  AND date_purchased = @v4  AND receipt_number = @v5  AND approved NOT IN (-1, 1)", new object[] { exp.id_payperiod, exp.woprog_id, exp.id_member, amount, Toolbox.MySQL_shortdt(exp.date_purchased), t_receipt.Text });
			if (c > 0 && !_isEdit)
				{
				c_workorder.DataBind();
				do_error("You have already submitted this expense for the current pay period. Please click the history tab above to review your past expenses.", false);
				return;
				}
			if (exp.woprog_id == 0 && !is_shop)
				{
				c_workorder.DataBind();
				do_error("Please either choose a work order, or check the shop expense checkbox.", true);
				return;
				}
			var wo_dc = new NeWODetailCurrent();
			var do_edit_wo = false;
			#region Work orders don't match... surgery time.
			if (prev_exp.woprog_id != exp.woprog_id)
				{
				if (prev_exp.woprog_id == 0) // Shop Expense moving to a real work order
					{
					// Don't need to do anything, the below code will create the work order line
					do_edit_wo = true;
					}
				else
					{
					// Previous was a work order, and it is moving to a shop expense
					var prev_wo = new NeWOProg(prev_exp.woprog_id);
					if (!payroll.expense.workorder_available(prev_exp, prev_wo))
						{
						c_workorder.DataBind();
						do_error("Work order is no longer open to expenses.", true);
						return;
						}
					var bs = payroll.expense.detach(conn, prev_wo, prev_exp, ref wo_dc, current_user);
					if (!bs.success)
						{
						c_workorder.DataBind();
						do_error(bs.message, true);
						return;
						}
					}
				}
			#endregion

			var did_workorder = 0;
			var did_passport = 0;
			var did_uploadedfile = 0;
			var wo = new NeWOProg();
			try
				{
				if (!is_shop || _isEdit && do_edit_wo)
					{
					wo = new NeWOProg(exp.woprog_id);
					if (!payroll.expense.workorder_available(exp, wo))
						{
						c_workorder.DataBind();
						do_error("Work order is no longer open to expenses.", true);
						return;
						}
					if (exp.id_expense > 0)
						{
						var wo_lineid = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(id), 0) FROM wo_detail 
WHERE memberid = @v0 AND consignment_id = @v1", new object[] { exp.id_member, exp.id_expense });
						wo_dc = new NeWODetailCurrent(wo_lineid);
						}
					var bs = payroll.expense.attach(conn, wo, exp, ref wo_dc, current_user);
					if (bs.success)
						{
						did_workorder = 1;
						}
					else
						{
						c_workorder.DataBind();
						do_error(bs.message, true);
						return;
						}
					manager = wo.intProjectManager == current_user.id32 
									? new NeMember(current_user.reports_to) 
									: new NeMember(wo.intProjectManager);
					}
				else
					{
					exp.woprog_id = 0;
					did_workorder = -1;
					}
				}
			catch (Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				do_error("There was an error saving to the work order, an email has been created to our support team to help troubleshoot the issue... Please do not submit the request again.", true);
				return;
				}


			if (manager.id == 0)
				{
				manager = user.business_unit.branch_manager;
				}
			if (manager.id == 0)
				{
				manager = new NeMember(user.business_unit.branch_manager.reports_to);
				}
			if(manager.id == user.id)
				{
				manager = new NeMember(user.reports_to);
				}
			var attachmentPath = "";
			try
				{
				var posted_file = uc_receipt.UploadedFiles[0];
				if (!string.IsNullOrEmpty(posted_file.FileName))
					{
					exp.has_file = true;
					exp.file_ext = Path.GetExtension(posted_file.FileName).Replace(".", "").ToLower();
					exp.file_mime = posted_file.ContentType;
					}
				else if (_isEdit && prev_exp.has_file)
					{
					exp.has_file = prev_exp.has_file;
					exp.file_ext = prev_exp.file_ext;
					exp.file_mime = prev_exp.file_mime;
					}
				exp.save();
				if (did_workorder == 1)
					{
					wo_dc.qty_committed = 0;
					wo_dc.qty_invoiced = 0;
					wo_dc.qty_ordered = 0;
					wo_dc.consignment_id = exp.id_expense;
					wo_dc.save(current_user, "Expense module - save routine", false);
					}
				#region Save File
				if (uc_receipt.UploadedFiles[0].FileName != "")
					{
					try
						{
						var this_data = new byte[posted_file.ContentLength];
						var fileServer = NeTaxEntity.BaseFolder(new NeMember(Convert.ToInt32(cb_user.Value)).business_unit_id, false);
						attachmentPath = fileServer + @"\expense_receipts\" + exp.id_expense + "." + exp.file_ext;
						posted_file.FileContent.Read(this_data, 0, Convert.ToInt32(posted_file.ContentLength));
						var this_stream = new FileStream(attachmentPath, FileMode.Create);
						this_stream.Write(this_data, 0, this_data.Length);
						this_stream.Close();
						this_stream.Dispose();
						did_uploadedfile = 1;
						}
					catch (Exception ee)
						{
						Toolbox.do_errorLog_errorStack(ee);
						do_error("There was an error saving the uploaded file, an email has been sent to our support team to help troubleshoot this issue... Please do not submit the request again.", true);
						exp.delete();
						// Rewind the work order.
						if (did_workorder == 1)
							{
							var bs = payroll.expense.detach(conn, wo, exp, ref wo_dc, current_user);
							if (bs.success) return;
							c_workorder.DataBind();
							do_error(bs.message, true);
							return;
							}
						return;
						}
					}
				#endregion Save File

				var woshop = is_shop ? "Shop" : "WO";
				var wo_num = !is_shop ? new NeWOProg(exp.woprog_id).OrderNumber : "N/A";
				var customer = "N/A";
				try
					{
					customer = !is_shop ? new NECustomer((int)new NeWOProg(exp.woprog_id).WOProg_Customer_ID).Customer_Name : "N/A";
					}
				catch (Exception ee)
					{
					// Shouldn't happen, but this will not hurt the process.
					Toolbox.do_errorLog_errorStack(ee);
					}
				var m = new NeEMail();
				m.To = manager.NEEmail;
				var attendees_appendage = exp.master_id != 73105 ? "" : string.Format(@"
	<tr>
		<td><b>Attendees:</b></td>
		<td>{0}</td>
	</tr>", exp.attendees);
				var mileage_appendage = is_mileage ? string.Format(@"
	<tr>
		<td><b>Distance Traveled:</b></td>
		<td>{0} {1}</td>
	</tr>", exp.distance, exp.unit_distance) : "";
				var wo_appendage = is_shop ? "" : string.Format(@"
	<tr>
		<td><b>WO#:</b></td>
		<td>{0}</td>
	</tr>
	<tr>
		<td><b>Customer:</b></td>
		<td>{1}</td>
	</tr>", wo_num, customer);
				m.From = user.NEEmail;
				m.Body = string.Format(@"
{0},<br/>
{1} has requested approval for their expense:<br/><br/>
<b><u>Expense Information</u><b><br/>
<table cellspacing='0' cellpadding='2'>
	<tr>
		<td><b>Type of Purchase (GL Account):</b></td>
		<td>{2} ({11})</td>
	</tr>
{9}
{10}
        <tr>
             <td><b>Business Unit Name:</b></td>
             <td>{12}</td>
        </tr>
	<tr>
		<td><b>WO or Shop?:</b></td>
		<td>{7}</td>
	</tr>
{8}
	<tr>
		<td><b>Purchased From:</b></td>
		<td>{3}</td>
	</tr>
	<tr>
		<td><b>Amount:</b></td>
		<td>{4:c2}</td>
	</tr>
	<tr>
		<td><b>Purchased Date:</b></td>
		<td>{5}</td>
	</tr>
	<tr>
		<td><b>Description:</b></td>
		<td>{6}</td>
	</tr>
</table>
<br/>
<br/>
",
manager.FirstName,                          // {0}
user.FullName,                              // {1}
c_category.Text,                            // {2}
Toolbox.do_value_from(exp.name_seller, false),  // {3}
exp.amount,                                 // {4}
exp.date_purchased.ToShortDateString(),     // {5}
Toolbox.do_value_from(exp.item_text, false),    // {6}
woshop,                                     // {7}
wo_appendage,                               // {8}
attendees_appendage,                        // {9}
mileage_appendage,                           // {10}
c_category.Value,							// {11}
user.business_unit.name                     //{12}
);
				m.URLyes = string.Format("/sections/member/expense/index.aspx?a=manager_approval&id={0}&type=approve", exp.id_expense);
				m.URLno = string.Format("/sections/member/expense/index.aspx?a=manager_approval&id={0}&type=deny", exp.id_expense);
				m.Subject = "Expense needs your Approval";
				m.to_member_id = manager.id;
				m.isHTML = true;
				if (exp.has_file)
					{
					var fileServer = NeTaxEntity.BaseFolder(new NeMember(Convert.ToInt32(cb_user.Value)).business_unit_id, false);
					m.Attachment = new Attachment(string.Format(@"{0}\expense_receipts\{1}.{2}", fileServer, exp.id_expense, exp.file_ext), exp.file_mime);
					m.Attachment.Name = "receipt." + exp.file_ext;
					}
				if(!_isEdit)
					{
					m.file_passport();
					}
				did_passport = 1;
				c_workorder.DataBind();
				clear_form();
				if (_isEdit)
					{
					Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "window.opener.gv_expensereport.Refresh();alert('Successfully saved expense - Reloading..');please_wait('Start');window.close();", true);
					}
				else
					{
					Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "alert('Successfully saved expense - Reloading..');location.href = location.href;", true);
					}
				}
			catch (Exception ee)
				{
				do_error(ee.ToString(), true);
				Toolbox.do_errorLog_errorStack(ee);
				if (did_passport == 0)
					{
					do_error("There was an error sending this request to your manager, an email has been sent to our support team to help troubleshoot this issue... Please do not submit the request again.", true);
					exp.delete();
					// Rewind the work order.
					if (did_workorder == 1)
						{
						payroll.expense.detach(conn, wo, exp, ref wo_dc, current_user);
						}
					// Delete uploaded file.
					if (did_uploadedfile == 1)
						{
						File.Delete(attachmentPath);
						}
					}
				}
			}
		}
	public int CurrentPayPeriod()
		{
		var payperiodid = Toolbox.doSQL_int("CALL _payperiod()");
		var pp = new NePayPeriod(payperiodid);
		if (Convert.ToDateTime(pp.Enddate) < DateTime.Now)
			{
			pp.payperiodID = Toolbox.doSQL_int(@"SELECT payperiodid FROM payperiods WHERE startdate <= NOW() AND enddate >= NOW()");
			}
		return Convert.ToInt32(pp.payperiodID);
		}
	protected void c_category_SelectedIndexChanged(object sender, EventArgs e)
		{
		var cat_id = c_category.Value.ToString();
		if (Toolbox.doSQL_int(
				@"SELECT count(id) from gl_te where tax_entity_id = @v0 and is_mileage=1 and account_no =@v1",
				new object[] { new NeMember(Convert.ToInt32(cb_user.Value)).business_unit.tax_entity_id, c_category.Value }) > 0)
			{
			// Mileage
			row_distance.Visible = true;
			row_attendees.Visible = false;
			t_amount.ReadOnly = true;
			t_amount.ValidationSettings.RequiredField.IsRequired = false;
			se_distance.ValidationSettings.RequiredField.IsRequired = true;
			rbl_distance_unit.SelectedIndex = new NeMember(Convert.ToInt32(cb_user.Value)).business_unit.country == "CDN" ? 0 : 1;
			se_distance.Focus();
			}
		else if (cat_id == "73105")
			{
			// Meals/Entertainment
			row_distance.Visible = false;
			row_attendees.Visible = true;
			row_attendees.Focus();
			}
		else
			{
			t_amount.ValidationSettings.RequiredField.IsRequired = true;
			se_distance.ValidationSettings.RequiredField.IsRequired = false;
			}
		c_workorder.DataBind();
		c_category.DataBind();
		c_seller.DataBind();
		}
	private void fill_workorders()
		{
		var user = new NeMember(Convert.ToInt32(cb_user.Value));
		var categories = Toolbox.doSQL_dt(@"
SELECT
   a.account_no account,
    a.gl_chart_name name
FROM
    gl_te a,
    gl_group_te b
WHERE a.gl_group_id = b.id
    AND b.type = 'X'
	    AND a.tax_entity_id = (SELECT tax_entity_id FROM business_unit WHERE id = @v0)
	    AND a.is_active order by a.gl_chart_name", new object[] { user.business_unit_id });
		c_category.DataSource = categories;
		c_category.DataBind();

		ds_workorder.SelectCommand = string.Format(@"
SELECT
	a.woprog_id,
	b.customer_name customer_name,
	a.woprog_bvwo,
	a.woprog_description `description`
FROM
	woprog a
LEFT JOIN
customer b ON a.woprog_customer_id = b.customer_id
WHERE
	a.woprog_status = 'Open' AND
a.labor_only=false and a.WOProg_Hold = 0 AND
	a.business_unit_id = {0} AND
a.woprog_bvwo != 'Not Entered'
ORDER BY 
	a.woprog_bvwo DESC
",
 user.business_unit_id);
		c_workorder.DataBind();
		}
	protected void cb_user_SelectedIndexChanged(object sender, EventArgs e)
		{
		fill_workorders();
		}
	protected void c_currency_SelectedIndexChanged(object sender, EventArgs e)
		{
	//	lbl_converted_currency.Text = "( " + Convert.ToDouble(Convert.ToDouble(t_amount.Text) * NECurrency.get_exchange_rate_at_date(Convert.ToInt32(cb_user.Value), de_purchase.Date, c_currency.Value.ToString())).ToString("N2") + " in users base currency)";
		}

	protected void c_workorder_SelectedIndexChanged1(object sender, EventArgs e)
		{
		var wo = new NeWOProg(Convert.ToInt32(c_workorder.Value));
		if (c_category.SelectedIndex < 0)
			{
			c_category.Value = "50110";
			}


		}





	protected void se_distance_Load(object sender, EventArgs e)
		{
		var sp = (ASPxSpinEdit)sender;
		sp.ClientSideEvents.NumberChanged = @"function(s,e){{   t_amount.SetText(s.GetValue() * " + Toolbox.doSQL_double("Select default_mileage_rate from tax_entity where id = " + new NeMember(Convert.ToInt32(cb_user.Value)).business_unit.tax_entity_id, null) + @");   }}";
		}

	protected void rbl_distance_unit_Load(object sender, EventArgs e)
		{
		var sp = (ASPxRadioButtonList)sender;
		sp.ClientSideEvents.ValueChanged = @"function(s,e){{   t_amount.SetText(s.GetValue() * " + Toolbox.doSQL_double("Select default_mileage_rate from tax_entity where id = " + new NeMember(Convert.ToInt32(cb_user.Value)).business_unit.tax_entity_id, null) + @");   }}";

		}
	}