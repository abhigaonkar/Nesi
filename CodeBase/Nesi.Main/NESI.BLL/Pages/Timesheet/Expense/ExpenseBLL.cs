using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using nesi.core;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;
using NESI.Common.Models;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.TimeSheet.Expense;
using static nesi.core.payroll;

namespace NESI.BLL.Pages.Timesheet.Expense
{
	public class ExpenseBLL : ExpenseBase
	{
		public bool IsAdmin { get; set; }
		public LabelValueInt[] VisibleUserList { get; set; }
		public string SubUsers { get; set; }
		public bool CanSelectUsers { get; set; }
		public DateTime MaxDate { get; set; }
		public string UploadFullPath { get; set; }

        public int Currency { get; set; }


		public ExpenseBLL()
		{

		}
		public ExpenseBLL(Employee user) : base(user)
		{
			IsAdmin = CurrentUser.AuthenticatedForPrivilege(175);
			UploadFullPath = new BLL.Core.FileManager.ExpenseFile(user).BasePath;

			SubUsers = _db.Database.SqlQuery<string>(@"SELECT IFNULL(REPORTS_TO(@p0),'')", CurrentUser.Id).First();
			if (IsAdmin)
			{
				VisibleUserList = _db.Database.SqlQuery<LabelValueInt>(
					@"SELECT a.member_id value, CONCAT(b.ddl_name, ' - ', a.member_fullname) label
					FROM member a inner join business_unit b ON a.business_unit_id = b.id where Find_in_set(b.id, @p0) 
					and a.member_termdate > curdate()-interval 1 month
					ORDER BY b.ddl_name, a.member_fullname",
					CurrentUser.VisibleBusinessUnits).ToArray();
				CanSelectUsers = true;
			}
			else if (!string.IsNullOrEmpty(SubUsers))
			{
				VisibleUserList = _db.Database.SqlQuery<LabelValueInt>(
					@"SELECT a.member_id value, CONCAT(b.ddl_name, ' - ', a.member_fullname) label 
					FROM member a inner join business_unit b ON a.business_unit_id = b.id 
					WHERE (Find_in_set(a.member_id, @p0) OR a.member_id = @p1) and  Find_in_set(b.id,@p2)
					and a.member_termdate > curdate()-interval 1 month
					ORDER BY b.ddl_name, a.member_fullname",
					SubUsers, CurrentUser.Id, CurrentUser.VisibleBusinessUnits).ToArray();
				CanSelectUsers = true;
			}
			else
			{
				VisibleUserList = new[]
				{
					new LabelValueInt
					{
						Label = CurrentUser.FullName,
						Value = CurrentUser.Id
					}
				};
				CanSelectUsers = false;
			}

			MaxDate = DateTime.Parse(DateTime.Now.ToShortDateString());

            Currency = GetCurrencyBUid(CurrentUser.BusinessUnitId);

        }

        public int GetCurrencyBUid(int bu_id)
        {
            return bllToolbox.doSQL_int(@"SELECT default_currency FROM neintranet.business_unit where ID=@v0", bu_id);
        }

		public bool MustProvideReceipt(Employee user, int categoryId)
		{
			return bllToolbox.doSQL_int(@"
					SELECT count(id) 
					from gl_te where tax_entity_id = @v0 
					and is_mileage=1 
					and account_no =@v1", user.TaxEntityId, categoryId) == 0;
		}

		public Profile GetProfile(Employee user)
		{


            return new Profile
            {
                UserId = user.Id,
                Currency = user.Currency,
                Categories = GetCategories(user),
                creditCardCategories = GetCreditCardCategories(user),
				WorkOrders = GetWorkOrders(user.BusinessUnitId)
			};
		}

		public LabelValueString[] GetCategories(Employee user)
		{
            /*
			return _db.Database.SqlQuery<LabelValueString>(
				@"
				SELECT
				   a.account_no value,
					a.gl_chart_name label
				FROM
					gl_te a,
					gl_group_te b
				WHERE a.gl_group_id = b.id
					AND b.type = 'X'
						AND a.tax_entity_id = (SELECT tax_entity_id FROM business_unit WHERE id = @p0)
						  and (a.see_on_po_gl_list=1 or a.is_mileage=1)
						AND a.is_active order by a.gl_chart_name", user.BusinessUnitId
			).ToArray();
            */

            var newQuery = @"
SELECT  e.expense_category_id value, e.Name label
FROM expense_category_business_unit ec
INNER JOIN expense_category e ON e.expense_category_id = ec.expense_category_id
WHERE ec.business_unit_id = @p0 and e.Active = 1
";

		    return _db.Database.SqlQuery<LabelValueString>(newQuery, user.BusinessUnitId).ToArray();
        }

        public int GetWorkorderCurrency(int woNumber)
        {
            return bllToolbox.doSQL_int(@"select IFNULL(b.default_currency,2) from business_unit b right join woprog w on b.id=w.business_unit_id where w.woprog_bvwo=@v0", woNumber);
        }



        public LabelValueString[] GetCreditCardCategories(Employee user)
        {


            return _db.Database.SqlQuery<LabelValueString>(
                @"
				SELECT
    a.account_no value,
    a.gl_chart_name label
FROM
    gl_te a,
    gl_group_te b
WHERE a.gl_group_id = b.id
    AND b.type = 'X'
	    AND a.tax_entity_id = (SELECT tax_entity_id FROM business_unit WHERE id = @p0)
	    AND a.is_active order by a.gl_chart_name", user.BusinessUnitId
            ).ToArray();
        }


        public string[] GetExpenseSeller(string like = "")
		{
			var sql =
				@"SELECT name_seller label FROM expense_seller WHERE id_seller != 309 ";
			string[] list;
			if (!string.IsNullOrWhiteSpace(like))
			{
				sql += " and name_seller like CONCAT('%', @p0, '%') GROUP BY name_seller ORDER BY name_seller";
				list = _db.Database.SqlQuery<string>(sql, like).ToArray();
			}
			else
			{
				sql += " GROUP BY name_seller ORDER BY name_seller limit 20";
				list = _db.Database.SqlQuery<string>(sql).ToArray();

			}
			return list;
		}

		public int GetExpenseSellerId(string name)
		{
			const string sql = @"SELECT id_seller FROM expense_seller WHERE id_seller != 309 and name_seller =@p0 GROUP BY name_seller ORDER BY name_seller";

			return bllToolbox.doSQL_int(sql, name);
		}

		public int new_seller(string _name, int _member_id)
		{
			return bllToolbox.doSQL_return_id(@"INSERT INTO expense_seller (name_seller, added_by, added_dt) VALUES (@v0, @v1, NOW())", _name, _member_id);
		}

		public string DeleteExpense(int Id)
		{
            var current_user = new NeMember(CurrentUser.Id);
			var dr_exp = Toolbox.doSQL_dt(@"SELECT * FROM expense_reimbursement WHERE id_expense = @v0 ", new object[] { Id }).Rows[0];
			var woprog_id = 0;
			int.TryParse(dr_exp["woprog_id"].ToString(), out woprog_id);
			var description = dr_exp["item_text"].ToString();
			var seller_id = Convert.ToInt32(dr_exp["id_seller"]);
			var exp_mem = dr_exp["id_member"].ToString();
            var exp_id = dr_exp["id_expense"].ToString();
            var bs = new Toolbox.boolstr();
            var ex = new payroll.expense(Id);
            var conn = Toolbox.connect();
           
            // check delete prevlilege.
            if (exp_mem != CurrentUser.Id.ToString())
			{
				return "You can not deny this expense.";
			}
			var affected_workorder_result = "";
			if (woprog_id > 0)
			{
				var wo = new NeWOProg(woprog_id);
				if (wo.Status != OpsWOStatus.Open)
				{
					return ("This work order is no longer open, it must be manually denied");
				}

                //
                // ExpenseReimbursement     = 55556;
			    // PerDiemExpense           = 55558;
                //
                var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current 
							WHERE wo_detail_current_woprog_id = @v0 and wo_detail_current_master_id IN (55556, 55558) and wo_detail_current_description = @v1", new object[] { woprog_id, description });
				if (c == 1)
				{
                    var wo_lineid = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(id), 0) FROM wo_detail WHERE memberid = @v0 AND consignment_id = @v1 ",
                                new object[] { exp_mem, exp_id });
                    var wo_line = new NeWODetailCurrent(wo_lineid);

                    bs = expense.detach(conn, wo, ex, ref wo_line, current_user);
         
                    NeWOProg.update_header_totals(woprog_id.ToString(), wo.business_unit_id, wo.OrderNumber);
					affected_workorder_result = $"Work order #{wo.OrderNumber} has been updated.";
				}
				else if (c > 1)
				{
					return ("There are more than one entries on that work order matching this expense, it might need to be manually denied.");
				}
			}
			else
			{
				affected_workorder_result = "This was a shop expense, no work order needed to be updated.";
			}
            
            bs = ex.review(conn, false, current_user, true);
            passport.DeactivatePassport("/sections/member/expense", Id);

            #region Send email to reports_to

            var em = new NeEMail
			{
				To = new NeMember(new NeMember(Convert.ToInt32(exp_mem)).reports_to).NEEmail,
				CC = "payroll@" + Toolbox.app_setting("DomainForEmail"),
				From = "noreply@" + Toolbox.app_setting("DomainForEmail"),
				Subject = seller_id == 309 ? "Per diem has been deleted" : "Expense has been deleted",
				isHTML = true,
				Body = $@"
<b>{current_user.FullName}</b> just deleted an unapproved {(seller_id == 309 ? "per diem" : "expense")} 
for <b>{Convert.ToDouble(dr_exp["amount"]):c2}</b> - <b>Description</b> {description}.<br/>
<i>{affected_workorder_result}</i>
"
			};
			em.Send();
			#endregion Send email to reports_to
			return "Expense has successfully been denied.";
		}

		public string SaveExpense(int Id, Employee employee, AddExpense model)
		{

			var user = new NeMember(model.Id_member);
			var payperiodid = nesi.core.NePayPeriod.get_payperiod_id(model.Date_purchased);

			var entriescount =
				bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM payroll_hours WHERE member_id =@v0 AND payperiod_id =@v1", model.Id_member, payperiodid);

			if (entriescount > 0)
			{
				return "Your payroll has already been submitted for this pay period, please talk with your manager.";
			}
			var sellerid = GetExpenseSellerId(model.Id_seller);
			if (sellerid == 0)
			{
				sellerid = new_seller(model.Id_seller, model.Id_member);
			}
			var is_mileage =
				bllToolbox.doSQL_int(
					@"SELECT count(id) from gl_te where tax_entity_id = @v0 and is_mileage=1 and account_no =@v1", user.business_unit.tax_entity_id, model.Master_id) > 0;

			var _isEdit = Id > 0;
			var is_shop = model.IsShop;
			var exp = Id == 0 ? new payroll.expense() : new payroll.expense(Id);
			var prev_exp = Id == 0 ? new payroll.expense() : new payroll.expense(Id);
			var manager = new NeMember();


			exp.receipt_number = model.Receipt_number;
			exp.item_text = model.Item_text;
            exp.pre_tax_amount = model.Pre_Tax_Amount;
			exp.amount = model.Amount;
			exp.master_id = model.Master_id;
			exp.date_purchased = model.Date_purchased;
			exp.id_seller = sellerid;
			exp.id_member = model.Id_member;
			exp.id_payperiod = payperiodid; // nesi.core.NePayPeriod.get_payperiod_id(model.Date_purchased);
			exp.currency = model.Currency.ToString();
			exp.unit_distance = is_mileage ? model.Distance_unit : "";
			exp.attendees = model.Attendees;
			exp.distance = model.Distance;
			exp.woprog_id = int.TryParse(model.Wo_number, out int woId) ? woId : 0;
			exp.date_start = model.Date_start;
			exp.date_end = model.Date_end;
		    exp.expense_category_id = model.Master_id;
			var expenseMember = new NeMember(exp.id_member);
			var posted_file = Path.Combine(model.File_path, model.File_name);
			if (model.Has_file && !System.IO.File.Exists(posted_file))
			{
				return "You must supply a physical receipt";
			}

			var c = bllToolbox.doSQL_int(@"
				SELECT COUNT(*) FROM expense_reimbursement 
				WHERE id_payperiod = @v0  
				AND woprog_id = @v1  
				AND id_member = @v2  
                AND pre_tax_amount = @v3
				AND amount = @v4  
				AND date_purchased = @v5  
				AND receipt_number = @v6
				AND approved NOT IN (-1, 1)",
				exp.id_payperiod, exp.woprog_id, exp.id_member,exp.pre_tax_amount, exp.amount, bllToolbox.MySQL_shortdt(exp.date_purchased), exp.receipt_number);
			if (c > 0 && !_isEdit)
			{
				return
					"You have already submitted this expense for the current pay period. Please click the history tab above to review your past expenses.";
			}
			if (!allow_unlinked_timesheet && exp.woprog_id == 0 && !model.IsShop)
			{
				return "Please either choose a work order, or check the shop expense checkbox.";
			}
			var wo_dc = new NeWODetailCurrent();
			var do_edit_wo = false;
			var current_user = new NeMember(CurrentUser.Id);
			var conn = nesi.core.Toolbox.connect();
            var wo = new NeWOProg();
            #region Work orders don't match... surgery time.
            if (!allow_unlinked_timesheet && _isEdit && prev_exp.woprog_id != exp.woprog_id)
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
					if (!nesi.core.payroll.expense.workorder_available(prev_exp, prev_wo))
					{
						return "Work order is no longer open to expenses.";
					}
					var bs = payroll.expense.detach(conn, prev_wo, prev_exp, ref wo_dc, current_user);
					if (!bs.success)
					{
						return bs.message;
					}
					manager = wo.intProjectManager == current_user.id32 && expenseMember.business_unit.branch_manager.id != wo.intProjectManager
								? new NeMember(current_user.reports_to)
								: new NeMember(wo.intProjectManager);
				}
			}
			#endregion
			var did_workorder = 0;
			var did_passport = 0;
			var did_uploadedfile = 0;
			
			try
			{
				if (!allow_unlinked_timesheet && !is_shop || _isEdit && do_edit_wo)
				{
					wo = new NeWOProg(exp.woprog_id);
					if (!payroll.expense.workorder_available(exp, wo))
					{
						return "Work order is no longer open to expenses.";
					}
					if (exp.id_expense > 0)
					{
						var wo_lineid = bllToolbox.doSQL_int(@"SELECT IFNULL(MAX(id), 0) FROM wo_detail 
WHERE memberid = @v0 AND consignment_id = @v1", exp.id_member, exp.id_expense);
						wo_dc = new NeWODetailCurrent(wo_lineid);
					}
					var bs = payroll.expense.attach(conn, wo, exp, ref wo_dc, current_user);
					if (bs.success)
					{
						did_workorder = 1;
					}
					else
					{
						return bs.message;
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
				// nesi.core.Toolbox.do_errorLog_errorStack(ee);
				return
					"There was an error saving to the work order, an email has been created to our support team to help troubleshoot the issue... Please do not submit the request again.";
			}


			if (manager.id == 0)
			{
				manager = user.business_unit.branch_manager;
			}
			if (manager.id == 0)
			{
				manager = new NeMember(user.business_unit.branch_manager.reports_to);
			}
			if (manager.id == user.id)
			{
				manager = new NeMember(user.reports_to);
			}
			var attachmentPath = "";

			try
			{
				if (model.Has_file && !string.IsNullOrEmpty(posted_file))
				{
					exp.has_file = true;

					exp.file_ext = Path.GetExtension(posted_file).Replace(".", "").ToLower();

					// exp.file_mime = posted_file.ContentType;
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

				if (model.Has_file && !string.IsNullOrEmpty(posted_file))
				{
					try
					{
						attachmentPath = Path.Combine(new BLL.Core.FileManager.ExpenseFile(employee).BasePath, exp.id_expense + "." + exp.file_ext);
						System.IO.File.Move(posted_file, attachmentPath);
						System.IO.File.Delete(posted_file);
						// if directory is empty delete directory.
						if (!Directory.EnumerateFileSystemEntries(model.File_path).Any())
						{
							Directory.Delete(model.File_path);
						}
						did_uploadedfile = 1;
					}
					catch (Exception ee)
					{

						exp.delete();
						// Rewind the work order.
						if (did_workorder == 1)
						{
							var bs = payroll.expense.detach(conn, wo, exp, ref wo_dc, current_user);
						}
						return
							"There was an error saving the uploaded file, an email has been sent to our support team to help troubleshoot this issue... Please do not submit the request again.";
					}
				}
				#endregion Save File

				var woshop = is_shop ? "Shop" : "WO";
				var wo_num = allow_unlinked_timesheet ? model.Wo_number : (!is_shop ? new NeWOProg(exp.woprog_id).OrderNumber : "N/A");
				var customer = "N/A";
				try
				{
					customer = !is_shop ? new NECustomer((int)new NeWOProg(exp.woprog_id).WOProg_Customer_ID).Customer_Name : "N/A";
				}
				catch (Exception ee)
				{
					// Shouldn't happen, but this will not hurt the process.
					// Toolbox.do_errorLog_errorStack(ee);
				}
				var m = new NeEMail { To = manager.NEEmail };
				var attendees_appendage = exp.master_id != 73105 ? "" : $@"
	<tr>
		<td><b>Attendees:</b></td>
		<td>{exp.attendees}</td>
	</tr>";
				var mileage_appendage = is_mileage ? $@"
	<tr>
		<td><b>Distance Traveled:</b></td>
		<td>{exp.distance} {exp.unit_distance}</td>
	</tr>"
					: "";
				var wo_appendage = (is_shop ? "" : $@"
	<tr>
		<td><b>" + (allow_unlinked_timesheet ? "JOB#" : "WO#") + $@":</b></td>
		<td>{wo_num}</td>
	</tr>") +
	(allow_unlinked_timesheet ? "" :
	$@"<tr>
		<td><b>Customer:</b></td>
		<td>{customer}</td>
	</tr>");
				m.From = user.NEEmail;
                if (string.IsNullOrEmpty(m.From))
                {
                    m.From = user.Email;
                }

				m.Body = string.Format(@"
{0},<br/>
{1} has requested approval for their expense:<br/><br/>
<b><u>Expense Information</u><b><br/>
<table cellspacing='0' cellpadding='2'>
	<tr>
		<td><b>Type of Purchase (GL Account):</b></td>
		<td>{2} ({12})</td>
	</tr>
{10}
{11}" + (  //allow_unlinked_timesheet? "" :
	@"<tr>
		<td><b>WO or Shop?:</b></td>
		<td>{8}</td>
	</tr>") + @"
{9}
    <tr>
        <td><b>Business Unit Name:</b></td>
        <td>{13}</td>
    </tr>
	<tr>
		<td><b>Purchased From:</b></td>
		<td>{3}</td>
	</tr>
    <tr>
		<td><b>Pre-Tax Amount:</b></td>
		<td>{4:c2}</td>
	</tr>
	<tr>
		<td><b>Amount:</b></td>
		<td>{5:c2}</td>
	</tr>
	<tr>
		<td><b>Purchased Date:</b></td>
		<td>{6}</td>
	</tr>
	<tr>
		<td><b>Description:</b></td>
		<td>{7}</td>
	</tr>
</table>
<br/>
<br/>
",
					manager.FirstName,                          // {0}
					user.FullName,                              // {1}
					model.Master_name,                            // {2}
					nesi.core.Toolbox.do_value_from(exp.name_seller, false),  // {3}
                    exp.pre_tax_amount,                         //{4}
					exp.amount,                                 // {5}
					exp.date_purchased.ToShortDateString(),     // {6}
					nesi.core.Toolbox.do_value_from(exp.item_text, false),    // {7}
					woshop,                                     // {8}
					wo_appendage,                               // {9}
					attendees_appendage,                        // {10}
					mileage_appendage,                           // {11}
					model.Master_id,                          // {12}
                    user.business_unit.name                     //{13}

				);
				m.URLyes = $"/sections/member/expense/index.aspx?a=manager_approval&id={exp.id_expense}&type=approve";
				m.URLno = $"/sections/member/expense/index.aspx?a=manager_approval&id={exp.id_expense}&type=deny";
				m.Subject = "Expense needs your Approval";
				m.to_member_id = manager.id;
				m.isHTML = true;
				if (exp.has_file)
				{
					var fileServer = NeTaxEntity.BaseFolder(new NeMember(Convert.ToInt32(model.Id_member)).business_unit_id, false);
					m.Attachment = new Attachment(attachmentPath,
						exp.file_mime)
					{ Name = "receipt." + exp.file_ext };
				}
				if (!_isEdit)
				{
					m.To = string.IsNullOrEmpty(manager.NEEmail) ? manager.Email : manager.NEEmail;
					m.file_passport();
				}
				did_passport = 1;

			}
			catch (Exception ee)
			{
				//	do_error(ee.ToString(), true);
				//	Toolbox.do_errorLog_errorStack(ee);
				if (did_passport == 0)
				{
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
					return
						"There was an error sending this request to your manager, an email has been sent to our support team to help troubleshoot this issue... Please do not submit the request again.";

				}
			}
			return "Expense has been updated successfully.";
		}

	}
}