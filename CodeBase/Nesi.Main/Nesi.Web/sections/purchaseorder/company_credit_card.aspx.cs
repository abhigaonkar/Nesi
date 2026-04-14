using System;
using System.IO;
using System.Web.UI;
using DevExpress.Web;
using System.Net.Mail;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Data;
using nesi.core;
using NESI.Common.Models;



public partial class sections_member_expense_company_credit_card : Page
{
	Toolbox _tools;
	NeMember current_user;
	int exp_type = 1;
	int id = 0;
	NameValueCollection _q;
	NECredit_card_purchase this_expense;
	bool is_admin = false;
	bool can_purchase_other_branches = false;
	bool is_purchaser = false;
	bool is_edit;
    int Select_User;
    DataTable dt_receipts;
    double original_amount = 0;
    protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(1);
		_tools.page_author = new NeMember(711);
		Session["business_unit_id"] = current_user.business_unit_id.ToString();
		_q = Request.QueryString;
		//		int.TryParse(_q["type"], out exp_type);
		row_pp.Visible = exp_type == 0;
		row_receiptnumber.Visible = exp_type == 0;
		lb_ccwarning.Visible = exp_type == 1;
		var is_mobile = !string.IsNullOrEmpty(_q["is_mobile"]) && _q["is_mobile"] == "1";
		is_admin = current_user.AuthenticatedForPrivilege(175);
		can_purchase_other_branches = current_user.AuthenticatedForPrivilege(187);
		is_purchaser = current_user.AuthenticatedForPrivilege(71);

        fill_cc_bu_user();
        
		
        //var dt_users = Toolbox.doSQL_dt(string.Format(@"SELECT a.member_id id, CONCAT(b.ddl_name, ' - ', a.member_fullname) name 
        //FROM member a inner join business_unit b ON a.business_unit_id = b.id
        //and a.member_status='Active' and b.id in (" + new Current_User().visible_business_units + ") ORDER BY b.ddl_name, a.member_fullname"), null);
		


		var sub_users = Toolbox.doSQL_string(@"SELECT IFNULL(REPORTS_TO(" + current_user.id + "), '')");
		/*if (!is_admin && !is_purchaser && sub_users != "" && !can_purchase_other_branches)// Branch Manager
		{
			dt_users = Toolbox.doSQL_dt(string.Format(@"SELECT a.member_id id, CONCAT(b.ddl_name, ' - ', a.member_fullname) name 
FROM member a inner join business_unit b ON a.business_unit_id = b.id 
WHERE (a.member_id IN ({0}) OR a.member_id = @v0 ) and b.id in (" + new Current_User().visible_business_units + @") 
ORDER BY  b.ddl_name, a.member_fullname", sub_users), new object[] { current_user.id });
			cb_user.DataSource = dt_users;
			cb_user.DataBind();
			ddl_credit_card.DataSource = Toolbox.doSQL_dt(@"Select credit_cards.id, concat(member_fullname,' ', credit_cards.type,' - ',right(number,4)) _name from credit_cards 
inner join member on member.member_id = credit_cards.member_id 
inner join business_unit b ON member.business_unit_id = b.id
where  b.id in (" + new Current_User().visible_business_units + ") and credit_cards.member_id in(" + sub_users + "," + current_user.id + ") and credit_cards.status='Active' and member.member_status='Active' ORDER BY b.ddl_name, member.member_fullname", null);
			ddl_credit_card.DataBind();
		}

		else if (is_admin)// Back office /Accounts Payable 
		{
          
                ddl_credit_card.DataSource = Toolbox.doSQL_dt(@"Select credit_cards.id, concat(member_fullname,' ', credit_cards.type,' - ',right(number,4)) _name 
from credit_cards 
inner join member on member.member_id = credit_cards.member_id 
inner join business_unit b on b.id = member.business_unit_id 
where member.member_status='Active' and credit_cards.status='Active' and b.id in (" + current_user.business_unit_id + ") ORDER BY b.ddl_name, member.member_fullname ", null);
            
			ddl_credit_card.DataBind();
		}
		else if (is_purchaser)
		{
			dt_users = Toolbox.doSQL_dt(@"SELECT a.member_id id, CONCAT(b.name, ' - ', a.member_fullname) name 
FROM member a 
inner join business_unit b ON a.business_unit_id = b.id 
WHERE a.business_unit_id = @v0  ORDER BY b.ddl_name, a.member_fullname", new object[] { current_user.business_unit_id });
			ddl_credit_card.DataSource = Toolbox.doSQL_dt(@"Select credit_cards.id, concat(member_fullname,' ', credit_cards.type,' - ',right(number,4)) _name 
from credit_cards 
inner join member on member.member_id = credit_cards.member_id  
where member.member_status='Active' and credit_cards.status='Active' and find_in_set(member.business_unit_id, @v0) ORDER BY  member.member_fullname", new object[] { current_user.business_unit_id });
			ddl_credit_card.DataBind();

		}
		else //was cannot purchase from other branches
		{
			ddl_credit_card.DataSource = Toolbox.doSQL_dt(@"Select credit_cards.id, concat(member_fullname,' ', credit_cards.type,' - ',right(number,4)) _name 
from credit_cards 
inner join member on member.member_id = credit_cards.member_id  
where find_in_set(credit_cards.member_id,@v0 ) and credit_cards.status='Active' and member.member_status='Active' ORDER BY  member.member_fullname", new object[] { current_user.id });
			ddl_credit_card.DataBind();
		}
        */
		if (!IsCallback && !IsPostBack)
		{
            if (cb_user.Items.FindByValue(current_user.id) != null)
            {
                cb_user.Value = current_user.id;
            }
            else
            {
                cb_user.SelectedIndex = 0;
            }
            
		}
        NeMember _user = cb_user.Value==null ? new NeMember(): new NeMember(Convert.ToInt32(cb_user.Value));
        ddl_div.Value = _user.business_unit_id;
        cb_user.Enabled = can_purchase_other_branches || is_admin || is_purchaser || sub_users != "";
		if (!string.IsNullOrEmpty(_q["id"]) && (is_admin || sub_users != ""))
		{
			int.TryParse(_q["id"], out id);
			is_edit = id > 0;
			if (id == 0)
			{
				Toolbox.FriendlyException(Response, "Unable to find credit card purchase record", "");
			}
			this_expense = new NECredit_card_purchase(id);

			row_pp.Visible = false;
			cb_user.Value = Convert.ToInt32(this_expense.member_id);
			cb_user.Enabled = false;
		    _user = new NeMember(Convert.ToInt32(cb_user.Value));
            t_amount.Value = this_expense.amount.ToString();
            total.Value = this_expense.total.ToString();
            double.TryParse(this_expense.amount.ToString(), out original_amount);
            c_category.Value = this_expense.expense_category_id;
			t_receipt.Value = this_expense.receipt_number;
			c_seller.Value = Toolbox.doSQL_int(@"SELECT MIN(id_seller) FROM expense_seller WHERE name_seller = (SELECT IFNULL(name_seller, '') FROM expense_seller WHERE id_seller = @v0)", new object[] { this_expense.seller_id });
			memo_description.Text = this_expense.item_text;
			c_currency.Value = this_expense.currency;
			cb_shop.Checked = this_expense.woprog_id == 0;           

            ddl_credit_card.Value = this_expense.credit_card_id != 0 ? this_expense.credit_card_id : 0;
			de_purchase.Date = this_expense.date_purchased;
			row_receiptlink.Visible = this_expense.has_file;

            if (is_edit)
            {
				if (this_expense.woprog_id > 0)
				{ 
					var woProgStatus = Toolbox.doSQL_string(@"SELECT woprog_status from woprog WHERE woprog_id = @v0", new object[] { this_expense.woprog_id });
					if (woProgStatus != OpsWOStatus.Open && woProgStatus != OpsWOStatus.Rework) 
					{
						Toolbox.FriendlyException(Response,"The associated work order is in an inoperable status, credit card purchase cannot be edited","");
					}								
				}
				ddl_receipts.Enabled = false;              
                ddl_credit_card.Enabled = false;

            }

			if (this_expense.has_file)
			{


				UploadWarning.Visible = true;
				row_receiptlink.Visible = true;
				hl_receipturl.NavigateUrl = "javascript:void(0);";
				hl_receipturl.ClientSideEvents.Click = "function(s,e){handle_link('" + NeTaxEntity.BaseFolder(this_expense.business_unit_id, true) + "/credit_card_receipts/" + this_expense.id + "." + this_expense.file_ext + "');}";
				hl_receipturl.Text = "See Scanned Receipt Here";
			}
			else
			{
				row_receiptlink.Visible = false;
			}


			if (cb_shop.Checked)
			{
				c_workorder.ClientEnabled = false;
			}
			else
			{
				c_workorder.Value = this_expense.woprog_id;
			}
			if (is_admin || can_purchase_other_branches)
			{
				ds_workorder.SelectCommand = string.Format(@"
SELECT
	a.woprog_id,
	Concat(c.name,'-',b.customer_name) customer_name,
	a.woprog_bvwo,
	a.woprog_description `description`,
    c.ddl_name
FROM
	woprog a
LEFT JOIN
customer b ON a.woprog_customer_id = b.customer_id
inner join business_unit c on c.id = a.business_unit_id
WHERE 
c.id in(" + new Current_User().visible_business_units + @") and
a.woprog_status !='Invoiced' and a.woprog_status != 'Waiting For PO' and a.woprog_status!='Deleted' and a.woprog_status!='waiting To Be Invoiced' and 
a.woprog_bvwo != 'Not Entered' and
a.labor_only=false and a.WOProg_Hold = 0 
and c.istest='F' 
ORDER BY 
c.ddl_name, a.woprog_bvwo  DESC
",
		_user.business_unit_id);
			}
			else
			{
				ds_workorder.SelectCommand = string.Format(@"
SELECT
	a.woprog_id,
	b.customer_name customer_name,
	a.woprog_bvwo,
	a.woprog_description `description`,
    c.ddl_name
FROM
	woprog a
LEFT JOIN
customer b ON a.woprog_customer_id = b.customer_id
inner join business_unit c on c.id = a.business_unit_id
WHERE
a.woprog_status !='Invoiced' and a.woprog_status != 'Waiting For PO' and a.woprog_status!='Deleted' and a.woprog_status!='waiting To Be Invoiced' and 
	a.business_unit_id  in(" + new Current_User().visible_business_units + @") AND
a.labor_only=false and a.WOProg_Hold = 0 AND
a.woprog_bvwo != 'Not Entered' 
ORDER BY 
	a.woprog_bvwo DESC
",
		new NeMember(this_expense.member_id).business_unit_id);
			}
		}
		if (is_mobile)
		{
			_tools.add_css("./mobile.css");
			var height = Unit.Pixel(35);
			de_purchase.Height =
				c_workorder.Height =
				t_amount.Height =
				c_currency.Height =
				c_category.Height =
				t_receipt.Height =
				uc_receipt.Height =
				c_seller.Height =
				 height;
		}
		else
		{
			_tools.add_css("./desktop.css");
		}
	// btn_clicked is a measure to prevent multiple submissions
		b_save.ClientSideEvents.Click = @"
function(s, e) 
	{
    e.processOnServer = !btn_clicked;
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
        please_wait('start');
		}
	else if(" + (!is_edit).ToString().ToLower() + @" && uc_receipt.GetText() == '' && (amount > -1 || amount == '') )
		{
		e.processOnServer = false;
		alert('You must include a physical receipt with this request.');
		btn_clicked = false;
		return;
		}
	else
		{
		btn_clicked = false;
		}
      
	}";
		if (!IsPostBack && !IsCallback && string.IsNullOrEmpty(_q["id"]))
		{
			var pp_id = new NePayPeriod().CurrentPayPeriod();
			var pp = new NePayPeriod(pp_id);
			var pp_start = Convert.ToDateTime(pp.StartDate);
			de_purchase.MaxDate = DateTime.Now;
			if (exp_type == 0)
			{
				de_purchase.MinDate = DateTime.Now.AddDays(-14);
			}
			var pp_end = Convert.ToDateTime(pp.Enddate);
			lb_payperiod.Text = string.Format("{0:MMMM dd, yyyy} - {1:MMMM dd, yyyy}", pp_start, pp_end);
			de_purchase.Date = DateTime.Now;

			c_currency.SelectedIndex = current_user.business_unit.country == "CDN" ? 1 : 0;
		}

        
    }
	protected void Page_Load(object sender, EventArgs e)
	{
        Double amountValue;

        Double.TryParse(t_amount.Text, out amountValue);
        dt_receipts = Toolbox.doSQL_dt(string.Format(@"
SELECT
   id,
   a.member_id,
    a.date_purchased,
   b.`start_date`,b.`end_date`,
   CONCAT( amount, ' - ', DATE_FORMAT(date_purchased, '%M %d'), ' - ', LEFT(item_text, 100) ) item_text
FROM
   credit_card_purchase a
   INNER JOIN
      accounting_period b ON a.date_purchased BETWEEN b.start_date AND b.end_date
WHERE
   a.member_id = @v0 AND  
    CURDATE() BETWEEN b.start_date AND b.end_date  AND
    a.amount > 0 AND
   (
      a.origin_id IS NULL
      OR a.origin_id = 0
   ) AND
   b.is_quarter = 0 AND b.is_year = 0
     AND
   a.approved = 0
    AND (SELECT COUNT(*) FROM credit_card_purchase WHERE origin_id = a.id) = 0
ORDER BY
     a.date_purchased DESC"), new[] { cb_user.Value });

        ddl_receipts.DataSource = dt_receipts;
        ddl_receipts.DataBind();
        if (IsPostBack && amountValue < 0)
        {
            receipts_row.Style["display"] = "block";
            receipts_title.Style["display"] = "block";
            cb_shop.ClientEnabled = false;
            cb_user.ClientEnabled = false;
            de_purchase.ClientEnabled = false;
            c_seller.ClientEnabled = false;
            c_workorder.ClientEnabled = false;
            ddl_credit_card.ClientEnabled = false;
            c_category.ClientEnabled = false;

        }
        else if(amountValue > 0)
        {
            receipts_row.Style["display"] = "none";
            receipts_title.Style["display"] = "none";
            cb_shop.ClientEnabled = true;
            cb_user.ClientEnabled = true;
            de_purchase.ClientEnabled = true;
            c_seller.ClientEnabled = true;
            c_workorder.ClientEnabled = true;
            ddl_credit_card.ClientEnabled = true;
            c_category.ClientEnabled = true;
        }

        if (is_edit)
        {
            if(this_expense.origin_id != 0)
            {
                ddl_receipts.Value = this_expense.origin_id;
            }
        }

        

            fill_cc_bu_user();
            
        
        fill_workorders();
		
	}
    public void fill_cc_bu_user()
    {
        cb_user.DataSource = Toolbox.doSQL_dt("call ds_creditcard_purchase(@v0,'EMP')", new object[] { current_user.id });
        ddl_div.DataSource = Toolbox.doSQL_dt("call ds_creditcard_purchase(@v0,'BU')", new object[] { current_user.id });
        ddl_credit_card.DataSource = Toolbox.doSQL_dt("call ds_creditcard_purchase(@v0,'CC')", new object[] { cb_user.Value});

       
        cb_user.DataBind();
        ddl_div.DataBind();
        ddl_credit_card.DataBind();
    }
	protected void clear_form()
	{
		de_purchase.Date = DateTime.Now;
		t_receipt.Text = "";
		c_category.SelectedIndex = -1;
		c_seller.SelectedIndex = -1;
		c_workorder.SelectedIndex = -1;
		memo_description.Text = "";
		t_amount.Text = "";
        total.Text = "";
		lb_error.Text = "";
		cb_shop.Checked = false;
		ddl_credit_card.SelectedIndex = -1;
		c_currency.SelectedIndex = current_user.business_unit.country == "CDN" ? 1 : 0;

		c_seller.DataBind();
		c_workorder.DataBind();
	}
	private void do_error(string e, bool show_up_error)
	{
		
		var up_e = uc_receipt.UploadedFiles.Length != 0 && show_up_error ? " <b class='upload'>• You may need to re-attach your receipt to upload, so do not delete it from your mobile.</b>" : "";
		lb_error.Text = "- " + e + up_e;
	}

	protected void b_save_Click(object sender, EventArgs e)
	    {

	    //b_save.ClientVisible = false;
		double amount = 0;
		double.TryParse(t_amount.Text, out amount);
        double total_amount = 0;
		double.TryParse(total.Text, out total_amount);
		var is_shop = false;
	   
        uc_receipt.Visible = uc_receipt.Visible;
		if (amount == 0)
		{
			 
			c_workorder.DataBind();
			do_error("Entered amount is not a valid number", true);
		    
            return;
		}
        if (total_amount ==0 )
        {
            c_workorder.DataBind();
            do_error("Entered total amount is not a valid number", true);

            return;
        }
		if(!uc_receipt.UploadedFiles[0].IsValid)
		{
			do_error("Please attach a receipt of supported format", true);
            return;
		}
        if (uc_receipt.UploadedFiles.Length != 0 && uc_receipt.UploadedFiles[0].FileBytes.Length > 8096000)
		{
			c_workorder.DataBind();
			do_error("The supplied receipt file is larger than 4MB.  Please change the resolution to reduce the file size.", true);
		    
            return;
		}
		if ((uc_receipt.UploadedFiles.Length == 0 || uc_receipt.UploadedFiles[0].ContentLength == 0) && exp_type == 1 && !is_edit && amount > 0)
		{
			c_workorder.DataBind();
			do_error("You must supply a physical receipt when submitting a 'Credit Card Receipt'", true);
		    
            return;
		}
		if (ddl_credit_card.SelectedIndex < 0)
		{
			c_workorder.DataBind();
			do_error("You must choose a valid company credit card.  If you do not see your card in the list, please ask your purchaser to enter it, or email ap@newelectric.com.", true);
		    
            return;
		}
		is_shop = cb_shop.Checked;
		// Check if expense exists.
		var exp = is_edit
										? new NECredit_card_purchase(id)
										: new NECredit_card_purchase();
		var prev_exp = exp;
		var user = new NeMember(Convert.ToInt32(cb_user.Value));
		var is_new_seller = (c_seller.SelectedIndex == -1);
		exp.seller_id = is_new_seller
										? exp.new_seller(c_seller.Text, current_user.id)
										: Convert.ToInt32(c_seller.Value);
		exp.receipt_number = t_receipt.Text;
		exp.item_text = memo_description.Text;
		exp.amount = amount;
        exp.total = total_amount;
		exp.expense_category_id = (int)c_category.Value;
		exp.date_purchased = Convert.ToDateTime(de_purchase.Date);
		exp.date_requested = DateTime.Now;
		exp.member_id = user.id32;
		exp.credit_card_id = Convert.ToInt32(ddl_credit_card.Value);
		exp.currency = c_currency.Value.ToString();
		exp.business_unit_id = Convert.ToInt32(ddl_div.Value);

        if (receipts_row.Style["display"] == "block")
        {
            exp.origin_id = Convert.ToInt32(ddl_receipts.Value);
        }
        else
        {
            exp.origin_id = 0;
        }

        if(amount < 0 && ddl_receipts.Value == null && !is_edit)
        {
            do_error("Since you are creating a negative purchase, please choose a past receipt.", true);
            return;
        }
		var _woprog_id = 0;
		if (c_workorder.Value != null)
		{
			var valid = int.TryParse(c_workorder.Value.ToString(), out _woprog_id);

            if (!valid && !is_shop)
            {
                // not valid work number
                c_workorder.DataBind();
               
                do_error("The work order " + c_workorder.Value.ToString() + " is not valid, please select a valid one.", true);
                return;
            }

		}
		exp.woprog_id = _woprog_id;

		if (exp.woprog_id == 0 && !is_shop)
		{
			c_workorder.DataBind();
            do_error("Please either choose a work order, or check the shop expense checkbox.", true);

            return;
		}

        if (!is_shop)
        {
            //
            // Bug 1961: Credit card allows for direct entering of woprog id
            //
            // Make sure the work order must be from the list.
            //

            // Get the query string for datasource control.
            var sqlToRun = ds_workorder.SelectCommand;
            var dt = Toolbox.doSQL_dt(sqlToRun, new object[] { });
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
            {
                do_error("Please either choose a work order, or check the shop expense checkbox.", true);
                return;
            }

            var found = false;
            foreach (DataRow dr in dt.Rows)
            {
                var workorder = Convert.ToInt32(dr["woprog_id"].ToString());
                if (workorder == exp.woprog_id)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                do_error("The work order " + exp.woprog_id.ToString() + " does not exist in the list, please select a valid one.", true);
                return;
            }
        }
        else
        {
            //
            // Is shopping.
            //
            exp.woprog_id = 0;
        }
		// Pre tax and Total Due validation
		// Pre tax value is exp.amount
		// Total due (Post tax value) is exp.total

		// if pre tax amount is positive and total due is negative
		if(exp.amount > 0 && exp.total < 0)
		{
			 do_error("Your amount before taxes cannot be greater than your total amount. Please enter a valid total amount. ", true);
            return;
		}
		//if pre tax amount is negative and total due is positive
		if(exp.amount < 0 && exp.total > 0)
		{
			 do_error("Your amount before taxes is negative, total amount can not be greater than zero. Please enter a valid total amount. ", true);
            return;
		}
		// if pre tax amount is greater than total due, making sure that both amounts are positive or both are negative to user Math.Abs
		if ((exp.total < 0 && exp.amount < 0) || (exp.total > 0 && exp.amount > 0))
		{
			if (Math.Abs(exp.total) < Math.Abs(exp.amount))
			{
				do_error("Your amount before taxes cannot be greater than your total amount. Please enter a valid total amount and pre-tax amount. ", true);
				return;
			}
		}

        if (is_edit)
        {
            if(original_amount > 0 && exp.amount < 0)
            {
                do_error("You cannot edit a negative amount against a positive credit card purchase. Please enter a positive amount.", true);
                return;
            }
            else if (original_amount < 0 && exp.amount > 0)
            {
                do_error("You cannot edit a positive amount against a negative credit card purchase. Please enter a negative amount.", true);
                return;
            }
        }

		var wo_dc = new NeWODetailCurrent();
		var do_edit_wo = false;
		// Work orders don't match... surgery time.
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
				// Check status of work order
				if ((exp_type == 0 && prev_wo.Status != "Open") ||
							(exp_type == 1 &&
								(
								prev_wo.Status == "Deleted" ||
								prev_wo.Status == "Waiting Cust PO" ||
								prev_wo.Status == OpsWOStatus.WaitingToBeInvoiced ||
								prev_wo.Status == "Invoiced" ||
								prev_wo.Status == "Waiting for PO" ||
								prev_wo.Status == "Waiting PM Approval" ||
								prev_wo.Status == "Waiting BM Approval"
								)
							))
				{
					c_workorder.DataBind();
					do_error("Work order is no longer open to expenses.", true);
				    
                    return;
				}
				else
				{
                    int master_id = prev_exp.master_id == 0 ? prev_exp.expense_category_id : prev_exp.master_id;
                    // Get line from work order
                    var c_wo_lines = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0 AND wo_detail_current_master_id = @v1 AND wo_detail_current_description = @v2", new object[] { prev_exp.woprog_id, master_id ,prev_exp.item_text });
					if (c_wo_lines == 1)
					{
						var wo_line_id = Toolbox.doSQL_string(@"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0 AND wo_detail_current_master_id = @v1 AND wo_detail_current_description = @v2", new object[] { prev_exp.woprog_id, master_id, prev_exp.item_text });
						wo_dc.GetLineDetails(wo_line_id);
						wo_dc.qty_ordered = -1;
						wo_dc.qty_committed = -1;
						wo_dc.qty_invoiced = -1;
						try
						{
							wo_dc.save(current_user, "/sections/purchaseorder/company_credit_card.aspx.cs - b_save_Click #1", false);
						}
						catch (Exception ee)
						{
							_tools.catch_error(ee);
							c_workorder.DataBind();
							do_error("Couldn't uncommit quantity from work order - (this error should never happen)", true);
						    
                            return;
						}
						try
						{
							if (current_user.AuthenticatedForPrivilege(65))
							{
								NeWODetailCurrent.delete_workorder_line(Convert.ToInt32(wo_line_id), master_id, current_user);
							}
						}
						catch (Exception ee)
						{
							_tools.catch_error(ee);
							c_workorder.DataBind();
							do_error("Work order line doesn't want to delete, though we were able to uncommit the quantity - (this error should never happen).", true);
						    
                            return;
						}
					}
					else
					{
						c_workorder.DataBind();
						do_error("Work order line doesn't match expense, cannot edit.", true);
					    
                        return;
					}
				}
			}
		}
		
		var did_workorder = 0;
		var did_passport = 0;
		var did_uploadedfile = 0;
		NeWOProg wo = new NeWOProg();
		try
		{
			if (!is_shop || is_edit && do_edit_wo)
			{
				wo = new NeWOProg(exp.woprog_id);
				int editLine = NECredit_card_purchase.indexCCpurchaseinWODC(exp.id);
				wo_dc = new NeWODetailCurrent(editLine);
					if ((exp_type == 0 && wo.Status != "Open") ||
							(exp_type == 1 &&
								(
								wo.Status == "Deleted" ||
								wo.Status == OpsWOStatus.WaitingToBeInvoiced ||
								wo.Status == "Invoiced" ||
								wo.Status == "Waiting for PO" ||
								wo.Status == "Waiting PM Approval" ||
								wo.Status == "Waiting BM Approval"
								)
							))
				{
					c_workorder.DataBind();
					do_error("Work order is no longer open to expenses.", true);
				    
                    return;
				}
				
				var used_amount = exp.amount < 0 ? Math.Abs(exp.amount) : exp.amount;
				double used_qty = is_edit ? 0 : exp.amount < 0 ? -1 : 1;
				wo_dc.added_by = (int)exp.member_id;
				wo_dc.date_added = Toolbox.MySQLNow_long();
				wo_dc.date_modified = Toolbox.MySQLNow_long();
				wo_dc.description = exp.item_text;
                wo_dc.master_id = exp.expense_category_id == 20
                                    ? OpsSpecialPart.MiscMaterial
                                        : exp.expense_category_id == 21
                                            ? OpsSpecialPart.SubContractor
                                                : OpsSpecialPart.CompanyCreditCardExpense;
                wo_dc.cost = Convert.ToDouble(used_amount);
				wo_dc.sell = wo.use_fixed_material_markup? wo.fixed_material_markup* used_amount: shared.GetSellPrice(Convert.ToDouble(used_amount), 0, false, 1, wo_dc.business_unit_id);
				wo_dc.unit = wo.use_fixed_material_markup ? wo.fixed_material_markup * used_amount : shared.GetSellPrice(Convert.ToDouble(used_amount), 0, false, 1, wo_dc.business_unit_id);
				wo_dc.qty_committed = used_qty;
				wo_dc.qty_invoiced = used_qty;
				wo_dc.billtypeid = wo.QuoteID == "0" ? 0 : 1;
				wo_dc.tax1 = 0;
				wo_dc.tax2 = 0;
				wo_dc.tax3 = 0;
				wo_dc.tax4 = 0;
				wo_dc.woprog_id = wo.woprog_id;
				wo_dc.bvwo = Convert.ToInt32(wo.OrderNumber);
				wo_dc.business_unit_id = wo.business_unit_id;
				wo_dc.type = "M";
				//wo_dc.code = exp.master_id.ToString();
                wo_dc.code = exp.expense_category_id.ToString();
                wo_dc.origin = OpsWOLineOrigin.CompanyCreditCardExpense;
				wo_dc.issues = "";
				wo_dc.memberid = (int)exp.member_id;
				wo_dc.paytypeid = 0;
				wo_dc.save(current_user, "/sections/purchaseorder/company_credit_card.aspx.cs - b_save_Click #2",false);
				did_workorder = 1;
			



			    }
			else
			{
				exp.woprog_id = 0;
				did_workorder = -1;
			}

		}
		catch (Exception ee)
		{
			Toolbox.do_errorLog(ee);
			do_error("There was an error saving to the work order, an email has been created to our support team to help troubleshoot the issue... Please do not submit the request again.", true);
		    
            return;
		}

	   
		var strPath = "";
		try
		{
			var posted_file = uc_receipt.UploadedFiles[0];
			if (posted_file.FileName != "")
			{
				exp.has_file = true;
				exp.file_ext = Path.GetExtension(posted_file.FileName).Replace(".", "").ToLower();
				exp.file_mime = posted_file.ContentType;
			}
			else if (is_edit && prev_exp.has_file)
			{
				exp.has_file = prev_exp.has_file;
				exp.file_ext = prev_exp.file_ext;
				exp.file_mime = prev_exp.file_mime;
			}
			exp.save();

			if (wo_dc.id > 0)
			{
				Toolbox.doSQL_void(@"
									UPDATE
											wo_detail_current
									SET
											wo_detail_current_consignment_id = @v0 
									WHERE 
											wo_detail_current_id = @v1 AND 
											wo_detail_current_origin = @v2 LIMIT 1", 
											new object[] { exp.id, wo_dc.id, wo_dc.origin });
			}
			#region Save File
			if (uc_receipt.UploadedFiles[0].FileName != "")
			{
				try
				{
					var this_data = new byte[posted_file.ContentLength];

					var fileServer = NeTaxEntity.BaseFolder(user.business_unit_id, false);
					var idFileName = exp.id + "." + exp.file_ext;
					strPath = $@"{fileServer}\credit_card_receipts\{idFileName}";
					posted_file.FileContent.Read(this_data, 0, Convert.ToInt32(posted_file.ContentLength));
					var this_stream = new FileStream(strPath, FileMode.Create);
					this_stream.Write(this_data, 0, this_data.Length);
					this_stream.Close();
					this_stream.Dispose();
					if(wo_dc.id > 0)
						{
						var detailedFileName = $"{Toolbox.MySQL_shortdt(exp.date_purchased)}-{user.FullName}-{exp.id}.{exp.file_ext}";
						var woProjPath = new NeFiles().GetProjectFolder(wo_dc.woprog_id);
						var woDestination	= Path.Combine(woProjPath, NeFiles.WoFolder.ExpenseReceipts, detailedFileName);
						File.Copy(strPath, woDestination); // Copy receipts to project folder
						//File.Move(Path.Combine(woDestination, idFileName), Path.Combine(woDestination, detailedFileName));
						}
					did_uploadedfile = 1;
				}
				catch (Exception ee)
				{
					Toolbox.do_errorLog(ee);
					do_error("There was an error saving the uploaded file, an email has been sent to our support team to help troubleshoot this issue... Please do not submit the request again.", true);
					//		exp.delete();
					// Rewind the work order.
					if (did_workorder == 1)
					{
						wo_dc.qty_committed = -1;
						wo_dc.qty_ordered = -1;
						wo_dc.qty_invoiced = -1;
						wo_dc.billtypeid = 5;
						wo_dc.save(current_user, "/sections/purchaseorder/company_credit_card.aspx.cs - b_save_Click #3", false);
						// Can't delete if the user doesn't have the privilege to.
						if (current_user.AuthenticatedForPrivilege(65))
						{
							//	NeWODetailCurrent.delete_workorder_line(wo_dc.id, wo_dc.master_id, current_user);
						}
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
				customer = !is_shop ? _tools.value_from(new NECustomer((int)new NeWOProg(exp.woprog_id).WOProg_Customer_ID).Customer_Name, false) : "N/A";
			}
			catch (Exception ee)
			{
				// Shouldn't happen, but this will not hurt the process.
				_tools.catch_error(ee);
			}
			var m = new NeEMail();
			// m.To						= "ap@newelectric.com"; 
			// Removed as per ticket 9765
			m.To = new NeMember(Convert.ToInt32(user.reports_to)).NEEmail + ";" + user.business_unit.branch_manager.NEEmail;
			var businessUnitId = wo.woprog_id > 0 ? wo.business_unit_id : exp.business_unit_id;
			var businessUnit = new NeBusinessUnit(businessUnitId);
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

{0} has made a company credit card purchase:<br/><br/>
<b><u>Purchase Information</u><b><br/>
<table font-face = 'Arial' cellspacing='0' cellpadding='2'>
	<tr>
		<td><b>Type of Purchase (GL Account):</b></td>
		<td>{1}</td>
	</tr>
	<tr>
		<td><b>WO or Shop?:</b></td>
		<td>{6}</td>
	</tr>
{7}
	<tr>
		<td><b>Purchased From:</b></td>
		<td>{2}</td>
	</tr>
	<tr>
		<td><b>Amount:</b></td>
		<td>{3:c2}</td>
	</tr>
	<tr>
		<td><b>Purchased Date:</b></td>
		<td>{4}</td>
	</tr>
	<tr>
		<td><b>Description:</b></td>
		<td>{5}</td>
	</tr>
<tr>
		<td><b>Credit Card Used:</b></td>
		<td>{8}</td>
	</tr>
<tr>
		<td><b>Business Unit:</b></td>
		<td>{9}</td>
	</tr>
</table>
<br/>
<br/>
",
                         
user.FullName,                              // {0}
c_category.Text + " (" + c_category.Value + ")",                            // {1}
_tools.value_from(c_seller.Text, false),    // {2}
exp.amount,                                 // {3}
exp.date_purchased.ToShortDateString(),     // {4}
_tools.value_from(exp.item_text, false),    // {5}
woshop,                                     // {6}
wo_appendage,                               // {7}
ddl_credit_card.Text, //{8}
ddl_div.Text//9
);
			m.URLyes = exp_type == 0 ? string.Format("/sections/purchaseorder/company_credit_card.aspx?a=manager_approval&id={0}&type=approve", exp.id) : "";
			m.URLno = exp_type == 0 ? string.Format("/sections/purchaseorder/company_credit_card.aspx?a=manager_approval&id={0}&type=deny", exp.id) : "";
			m.Subject = "Credit Card Purchase Made";
			m.to_member_id = 0;

			m.isHTML = true;
			if (exp.has_file)
			{
				var fileServer = NeTaxEntity.BaseFolder(user.business_unit_id, false);
			 	m.Attachment = new Attachment(fileServer + @"\credit_card_receipts\" + exp.id + "." + exp.file_ext, exp.file_mime);
                m.Attachment.Name = "receipt." + exp.file_ext;
			}

			m.Send();

			did_passport = 1;
			c_workorder.DataBind();
		    
            clear_form();
			if (is_edit)
			{
				Page.ClientScript.RegisterStartupScript(GetType(), "myScript", "please_wait('start', 'Reloading');window.opener.gv.Refresh();alert('Successfully saved purchase record - Reloading..');location.href = location.href;", true);
			}
			else
			{
				ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "remove", "alert('Successfully saved purchase record - Reloading..');window.parent.location.href = window.parent.location.href;", true);

			}

		}
		catch (Exception ee)
		{
			do_error(ee.ToString(), true);
			Toolbox.do_errorLog(ee);
		    
            if (did_passport == 0)
			{
				do_error("There was an error sending this request to your manager, an email has been sent to our support team to help troubleshoot this issue... Please do not submit the request again.", true);
				//		exp.delete();
				// Rewind the work order.
				if (did_workorder == 1)
				{
					wo_dc.qty_committed = -1;
					wo_dc.qty_ordered = -1;
					wo_dc.qty_invoiced = -1;
					wo_dc.billtypeid = 5;
					//wo_dc.UpdateWODetailCurrentPart(wo_dc.id.ToString(), false);
					if (current_user.AuthenticatedForPrivilege(65))
					{
						//		NeWODetailCurrent.delete_workorder_line(wo_dc.id, wo_dc.master_id, current_user);
					}
				}
				// Delete uploaded file.
				if (did_uploadedfile == 1)
				{
					//	File.Delete(strPath);
				}
			}
		}
	    
    }
	protected void gv_workorders_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		var IDs = new object[gv.VisibleRowCount];
		var WOs = new object[gv.VisibleRowCount];
		var NAMEs = new object[gv.VisibleRowCount];
		for (var i = 0; i < gv.VisibleRowCount; i++)
		{
			try
			{
				IDs[i] = gv.GetRowValues(i, "woprog_id");
				WOs[i] = gv.GetRowValues(i, "woprog_bvwo");
				NAMEs[i] = gv.GetRowValues(i, "customer_name");
			}
			catch (Exception ee)
			{
				var extra = string.Format("Current iterator: {0}\nGV VisibleRowCount:{1}\nIDs Length:{2}", i, gv.VisibleRowCount, IDs.Length);
				_tools.catch_error(new Exception(ee + "\n" + extra));
			}
		}
		e.Properties["cpIDs"] = IDs;
		e.Properties["cpWOs"] = WOs;
		e.Properties["cpNAMEs"] = NAMEs;
	}
	protected void c_category_SelectedIndexChanged(object sender, EventArgs e)
	{
		var cat_id = (int)c_category.Value;

		c_workorder.DataBind();
		c_category.DataBind();
		c_seller.DataBind();
	}
	private void fill_workorders()
	{
		var user = new NeMember(Convert.ToInt32(cb_user.Value));
		 
			  ddl_div.Value = user.business_unit_id;
			
if(cb_user.Value!= null) { 
        c_currency.Value  = Toolbox.doSQL_string(@"select MAX(currency) from currency c inner join business_unit b on c.id=b.default_currency where b.ID=@v0", new object[] { user.business_unit_id });
        }
        //        c_category.DataSource = Toolbox.doSQL_dt(@"SELECT
        //    a.account_no account,
        //    a.gl_chart_name name
        //FROM
        //    gl_te a,
        //    gl_group_te b
        //WHERE a.gl_group_id = b.id
        //    AND b.type = 'X'
        //	    AND a.tax_entity_id = (SELECT tax_entity_id FROM business_unit WHERE id = @v0)
        //	    AND a.is_active order by a.gl_chart_name", new object[] { user.business_unit_id });

        c_category.DataSource = Toolbox.doSQL_dt(@"Select a.expense_category_id account ,a.Name name From expense_category as a
inner join expense_category_business_unit as b
on a.expense_category_id = b.expense_category_id
where a.Active=1 and b.business_unit_id = @v0", new object[] { user.business_unit_id });

        c_category.DataBind();

        if (is_admin || can_purchase_other_branches)
		{
			ds_workorder.SelectCommand = string.Format(@"
SELECT
	a.woprog_id,
	b.customer_name customer_name,
	a.woprog_bvwo,
	a.woprog_description `description`,
c.ddl_name
FROM
	woprog a
LEFT JOIN
customer b ON a.woprog_customer_id = b.customer_id
inner join business_unit c on c.id = a.business_unit_id
WHERE c.id ={1} and
	{0}  AND
a.labor_only=false and a.WOProg_Hold = 0 AND
a.woprog_bvwo != 'Not Entered'

ORDER BY 
c.ddl_name,	a.woprog_bvwo DESC
",
	 (exp_type == 0 ? "a.woprog_status = 'Open'" : "a.woprog_status NOT IN ('Deleted', 'Waiting To Be Invoiced', 'Invoiced', 'Waiting for PO', 'Waiting PM Approval', 'Waiting BM Approval')"),
	 user.business_unit_id);

		}
		else
		{
//		    c_category.DataSource = Toolbox.doSQL_dt(@"SELECT
//    a.account_no account,
//    a.gl_chart_name name
//FROM
//    gl_te a,
//    gl_group_te b
//WHERE a.gl_group_id = b.id
//    AND b.type = 'X'
//	    AND a.tax_entity_id = (SELECT tax_entity_id FROM business_unit WHERE id = @v0)
//	    AND a.is_active order by a.gl_chart_name", new object[] { user.business_unit_id });
//		    c_category.DataBind();

            ds_workorder.SelectCommand = string.Format(@"
SELECT
	a.woprog_id,
	b.customer_name customer_name,
	a.woprog_bvwo,
	a.woprog_description `description`,
c.ddl_name
FROM
	woprog a
LEFT JOIN
customer b ON a.woprog_customer_id = b.customer_id
inner join business_unit c on c.id = a.business_unit_id
WHERE
	{0} AND
	a.business_unit_id = {1} AND
a.labor_only=false and a.WOProg_Hold = 0 AND
a.woprog_bvwo != 'Not Entered'
ORDER BY 
c.ddl_name,	a.woprog_bvwo DESC
",
	 (exp_type == 0 ? "a.woprog_status = 'Open'" : "a.woprog_status NOT IN ('Deleted', 'Waiting To Be Invoiced', 'Invoiced', 'Waiting for PO', 'Waiting PM Approval', 'Waiting BM Approval')"),
	 user.business_unit_id);
		}
		c_workorder.DataBind();

//        ddl_credit_card.DataSource = Toolbox.doSQL_dt(@"Select credit_cards.id, concat(member_fullname,' ', credit_cards.type,' - ',right(number,4)) _name from credit_cards 
//inner join member on member.member_id = credit_cards.member_id 
//inner join business_unit b ON member.business_unit_id = b.id
//where  b.id in (" + new Current_User().visible_business_units + ") and credit_cards.member_id in(" + current_user.id + ") and credit_cards.status='Active' and member.member_status='Active' ORDER BY b.ddl_name, member.member_fullname", null);
//        ddl_credit_card.DataBind();

    }

	protected void cb_user_SelectedIndexChanged(object sender, EventArgs e)
	{
        ASPxComboBox comboBox = (ASPxComboBox)sender;
        Select_User = (int)comboBox.Value;
        fill_workorders();
        if (Select_User>0)
        {
            fill_cc_bu_user();
            /*NeMember _user = new NeMember(Select_User);
            ddl_credit_card.DataSource = Toolbox.doSQL_dt(@"Select credit_cards.id, concat(member_fullname,' ', credit_cards.type,' - ',right(number,4)) name 
from credit_cards 
inner join member on member.member_id = credit_cards.member_id 
inner join business_unit b on b.id = member.business_unit_id 
where member.member_status='Active' and credit_cards.status='Active' and b.id in (" + _user.business_unit_id + ") ORDER BY b.ddl_name, member.member_fullname ", null);
            ddl_credit_card.DataBind();
            */
        }

        ddl_receipts.Value = null;
        de_purchase.Date = DateTime.Today;
        c_seller.Value = null;
        c_workorder.Value = null;
        cb_shop.Checked = false;
        ddl_credit_card.Value = null;
        t_amount.Value = null;
        total.Value = null;
        c_category.Value = null;
        memo_description.Value = null;

    }

    protected void ddl_receipts_SelectedIndexChanged(object sender, EventArgs e)
    {

        var row = ddl_receipts.SelectedItem;
        var rowId = Convert.ToInt32(ddl_receipts.Value);
        var ccItem = new NECredit_card_purchase(rowId);
        cb_user.Value = ccItem.member_id;
        de_purchase.Date = DateTime.Now;
        c_seller.Value = ccItem.seller_id;
        c_workorder.Value = ccItem.woprog_id;
        //bool isShop = Convert.ToBoolean(row.GetFieldValue("date_purchased"));
        //t_amount.Value = Convert.ToInt32(row.GetFieldValue("amount"));
        ddl_credit_card.Value = ccItem.credit_card_id;
        c_category.Value = ccItem.expense_category_id;
        //receipt = Convert.ToString(row.GetFieldValue("date_purchased"));
        memo_description.Value = ccItem.item_text;

        if (c_workorder.Value.ToString() == "0")
        {
            cb_shop.Checked = true;
            c_workorder.Value = null;
        }

    }
    protected void c_workorder_SelectedIndexChanged1(object sender, EventArgs e)
    {
		if(c_workorder.Value == null) return;

        int workorder = 0;
        if (!int.TryParse(c_workorder.Value.ToString(), out workorder))
        {
            return;
        }

        var user = new NeMember(Convert.ToInt32(cb_user.Value));
        NeWOProg wo = new NeWOProg(Convert.ToInt32(c_workorder.Value));
        ddl_div.Value = wo.business_unit_id;
        if (user.business_unit_id == wo.business_unit_id)
        {
            //if (c_category.SelectedIndex < 0)
            //{
            //   c_category.Value =-1;
            //}
            // NeMember _user = new NeMember((int)cb_user.Value);
            fill_cc_bu_user();
            /*
            ddl_credit_card.DataSource = Toolbox.doSQL_dt(@"Select credit_cards.id, concat(member_fullname,' ', credit_cards.type,' - ',right(number,4)) name 
from credit_cards 
inner join member on member.member_id = credit_cards.member_id 
inner join business_unit b on b.id = member.business_unit_id 
where member.member_status='Active' and credit_cards.status='Active' and b.id in (" + wo.business_unit_id + ") ORDER BY b.ddl_name, member.member_fullname ", null);
            ddl_credit_card.DataBind();*/
         //   ddl_credit_card.SelectedIndex = -1;
        }
        else
        {
            return;
        }

    }
}