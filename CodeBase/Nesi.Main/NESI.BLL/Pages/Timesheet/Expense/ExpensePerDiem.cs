using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using nesi.core;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.Employee;
using NESI.Common.Models;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.TimeSheet.Expense;

#pragma warning disable 618

namespace NESI.BLL.Pages.Timesheet.Expense
{
	public class ExpensePerDiem : ExpenseBase
	{
		public LabelValueInt[] PerDiemBusinessUnitList { get; set; }


		public ExpensePerDiem(Employee user) : base(user)
		{
			PerDiemBusinessUnitList = CurrentUser.AuthenticatedForPrivilege(175) ? CurrentUser.VisibleBusinessUnitLabelValueList : GetPerDiemBusinessUnitlist();
		}

		public LabelValueInt[] GetPerDiemBusinessUnitlist()
		{
			return GetLabelValueIntListFromSQL(
				@"Select distinct b.id value,ddl_name label from business_unit b 
				inner join member on member.business_unit_id = b.id 
				where (is_supervisor(member.member_id,@p0) 
				or member.member_id = @p0)  
				and  find_in_set(b.id, @p1) order by ddl_name",
				CurrentUser.Id, CurrentUser.VisibleBusinessUnits
			);
		}

		public ExpensePerDiemProfile GetPerDiemProfile(int buId)
		{
			var bu = Global.BusinessUnit.GetValue(buId);
			return new ExpensePerDiemProfile
			{
				PerdiemRate = bu.perdiem_rate.GetValueOrDefault(),
				BusinessUnitCountry = bu.Country,
				ForEmployees = GetForEmployees(buId, CurrentUser.Id),
				WorkOrders = GetWorkOrders(buId),
				allow_unlinked_timesheet = allow_unlinked_timesheet
			};
		}

		public LabelValueInt[] GetForEmployees(int buId, int userId)
		{
			if (CurrentUser.AuthenticatedForPrivilege(175))
			{
				return GetLabelValueIntListFromSQL(
					@"SELECT a.member_id value, a.member_fullname label
					FROM member a inner join business_unit b ON a.business_unit_id = b.id where b.id=@p0 
					and  a.member_status='Active'
					ORDER BY a.member_fullname",
					buId);
			}
			else
			{
				return GetLabelValueIntListFromSQL(
					@"Select a.member_id value, b.ddl_name, a.member_fullname, CONCAT('(',b.ddl_name,') ', a.member_fullname) label from member a 
							LEFT JOIN business_unit b ON a.business_unit_id = b.id 
							LEFT JOIN member c ON c.member_id = @p0
							LEFT JOIN business_unit d ON c.business_unit_id = d.id
							where a.member_status='Active' and b.id=@p1 
							and ((is_supervisor(a.member_id,@p0) or a.member_id = @p0 ) 
							OR (d.country = b.country AND d.is_corporate = 1)) order by ddl_name, member_fullname",
					userId, buId
				);
			}
		}


		public string Save(int Id, AddExpensePerDiem model)
		{
			var is_edit = Id > 0;
			var originalPerdiem = Id > 0 ? new payroll.expense(Id) : new payroll.expense();
			var current_user = new NeMember(CurrentUser.Id);
			var _tools = new Toolbox();

			using (var conn = Toolbox.connect())
			{
				if (model.Id_member == null || model.Id_member.Length <= 0)
				{
					return "You must select at least one employee.";
				}

				var fromId = NePayPeriod.get_payperiod_id(model.Date_start);
				var toId = NePayPeriod.get_payperiod_id(model.Date_end);
				if (fromId != toId)
				{
					return "A request cannot span multiple pay periods. Please adjust your dates.";
				}
				var sbError = new StringBuilder();
				var errors = new List<int>();
				foreach (var mid in model.Id_member)
				{
					var checkOpenPayroll = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM payroll_hours WHERE member_id = @v0 AND payperiod_id = @v1", new object[] { mid, fromId });
					if (checkOpenPayroll <= 0) continue;
					errors.Add(mid);
				}
				if (errors.Count > 0)
				{
					sbError.Append("The following user(s) already have their payroll approved and are stopping this request from processing:\n");
					foreach (var id in errors)
					{
						var m = new NeMember(id);
						sbError.AppendFormat("- {0}\n", m.FullName);
					}
					return (sbError.ToString());
				}



				// Check date range for duplicates
				foreach (var mid in model.Id_member)
				{
					var memberCurrent = new NeMember(Convert.ToInt32(mid));
					var manager = new NeMember();
					var managerList = NeMember.supervisor_list(memberCurrent.id);

					var c_dupes = 0;
					if (is_edit)
					{
						c_dupes = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM expense_reimbursement
					WHERE id_expense != @v3 AND approved IN (1, -1) AND item_text LIKE '% - Per Diem (%' AND id_member = @v0 AND ((date_start BETWEEN @v1 AND @v2) OR (date_end BETWEEN @v1 AND @v2) OR 
					(@v1 BETWEEN date_start AND date_end))", new object[] { mid, bllToolbox.MySQL_shortdt(model.Date_start), bllToolbox.MySQL_shortdt(model.Date_end), originalPerdiem.id_expense });
					}
					else
					{
						c_dupes = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM expense_reimbursement
					WHERE approved IN (1, -1) AND item_text LIKE '% - Per Diem (%' AND id_member = @v0 AND ((date_start BETWEEN @v1 AND @v2) OR (date_end BETWEEN @v1 AND @v2) OR 
					(@v1 BETWEEN date_start AND date_end))", new object[] { mid, bllToolbox.MySQL_shortdt(model.Date_start), bllToolbox.MySQL_shortdt(model.Date_end) });
					}
					if (c_dupes > 0)
					{
						return string.Format(memberCurrent.FullName + " already has per diems saved within that date range ({0} to {1})... cannot save request.", Toolbox.MySQL_shortdt(model.Date_start), Toolbox.MySQL_shortdt(model.Date_end));
					}

					var amount = model.Amount;
					var is_shop = model.IsShop;
					// Check if expense exists.
					payroll.expense exp;
                    
                    if (is_edit)
					{
						exp = originalPerdiem;
						exp.date_start = model.Date_start;
						exp.date_end = model.Date_end;
						exp.amount = amount;
					}
					else
					{
                       
                        exp = new payroll.expense
                        {

                            id_seller = 309,
                            receipt_number = "",
                            item_text =
                                $"{memberCurrent.FullName} - Per Diem ({model.Date_start:yyyy-MM-dd}) to ({model.Date_end:yyyy-MM-dd})",
                            amount = amount,
                            master_id = 360,
                            date_purchased = DateTime.Now,
                            date_start = model.Date_start,
                            date_end = model.Date_end,
                            id_member = memberCurrent.id,
                            currency = (memberCurrent.business_unit.country=="CAN"||memberCurrent.business_unit.country == "CDN")? "CAN": memberCurrent.business_unit.country,
							id_payperiod = NePayPeriod.get_payperiod_id(model.Date_start)
						};
					}
					var wo_dc = new NeWODetailCurrent();
					if (!is_shop && !allow_unlinked_timesheet)
					{
						if (is_edit && originalPerdiem.woprog_id > 0)
						{
							var woRemove = new NeWOProg(originalPerdiem.woprog_id);
							var bsRemove = payroll.expense.detach(conn, woRemove, originalPerdiem, ref wo_dc, current_user);
							if (!bsRemove.success)
							{
								return (bsRemove.message);
							}
						}

						exp.woprog_id = Convert.ToInt32(model.Wo_number);
						var wo = new NeWOProg(exp.woprog_id);
						if (wo.Status != OpsWOStatus.Open)
						{
							return ("Work order is no longer open to expenses.");
						}
						//if (wo.business_unit_id != memberCurrent.business_unit_id)
						//{
						//	return ("Work order is from a different branch, please select another.");
						//}
						var bs = payroll.expense.attach(conn, wo, exp, ref wo_dc, current_user);
						if (!bs.success)
						{
							return "ERROR - " + bs.message;
						}
						else
						{
							manager = new NeMember(wo.intProjectManager);
						}
					}
					else
					{
						if (!allow_unlinked_timesheet && is_edit && exp.woprog_id > 0)
						{
							var woRemove = new NeWOProg(originalPerdiem.woprog_id);
							var bsRemove = payroll.expense.detach(conn, woRemove, originalPerdiem, ref wo_dc, current_user);
							if (!bsRemove.success)
							{
								throw new Exception(bsRemove.message);
							}
						}
						exp.woprog_id = 0;
					}
					
					if (manager.id == 0)
					{
						manager = memberCurrent.business_unit.branch_manager;
					}
					if (manager.id == memberCurrent.id)
					{
						manager = new NeMember(memberCurrent.reports_to);
					}
					try
					{
						var wo_num = allow_unlinked_timesheet ? model.Wo_number : !is_shop ? new NeWOProg(exp.woprog_id).OrderNumber : "N/A";
						exp.save();
						if (allow_unlinked_timesheet)
						{
							bllToolbox.doSQL_void(@"update expense_reimbursement set job_no=@p0 where id_expense=@p1", wo_num, exp.id_expense);
						}
						if (exp.woprog_id > 0)
						{
							// Because we don't know the id of the expense at the time of creating the row, you need to save it after the fact
							wo_dc.consignment_id = exp.id_expense;
							wo_dc.qty_committed = 0;
							wo_dc.qty_invoiced = 0;
							wo_dc.qty_ordered = 0;
							wo_dc.save(current_user, "/sections/expense/index.aspx.cs - cb_new_Callback #2", false);
						}
						var woshop = is_shop ? "Shop" : "WO";
						var customer = "N/A";
						try
						{
							customer = !is_shop ? new NECustomer((int)new NeWOProg(exp.woprog_id).WOProg_Customer_ID).Customer_Name : "N/A";
						}
						catch (Exception ee)
						{
							_tools.catch_error(ee);
						}
						if (!managerList.Contains(current_user.id) && !is_edit)
						{
							var m = new NeEMail
							{
								To = manager.NEEmail,
								// CC = "payroll@newelectric.com",
								From = current_user.NEEmail
							};
							var dt_past_perdiems = Toolbox.doSQL_dt(conn, @"SELECT item_text,amount FROM expense_reimbursement WHERE item_text LIKE '% - Per Diem (%' AND id_member = @v0 AND date_requested > DATE_SUB(CURDATE(), INTERVAL 1 YEAR) ORDER BY date_end DESC", new object[] { memberCurrent.id });
							var sb_past_perdiems = new StringBuilder();
							sb_past_perdiems.Append("<div style='font-size:13px;font-weight:bold'>Past Per Diems</div>");
							if (dt_past_perdiems.Rows.Count > 0)
							{
								sb_past_perdiems.Append(@"	
	<table cellpadding='2' cellspacing='0' style='font-family:arial;font-size:12px;'>
		<thead>
			<tr>
				<th>Description</th>
				<th>Requested Amount</th>
			</tr>
		</thead>
		<tbody>
	");
								foreach (DataRow dr in dt_past_perdiems.Rows)
								{
									sb_past_perdiems.AppendFormat("<tr><td align='center'>{0}</td><td align='center'>${1}</td></tr>", dr["item_text"], dr["amount"]);
								}
								sb_past_perdiems.Append(@"
		</tbody>
	</table>");
							}
							else
							{
								sb_past_perdiems.Append("<div>-- None --</div>");
							}
							m.Body = string.Format(@"
	{0},<br/>
	{1} has requested approval for their expense:<br/><br/>
	<b><u>Expense Information</u><b><br/>
	<table cellspacing='0' cellpadding='2' style='font-family:arial;font-size:12px;'>
		<tr>
			<td><b>Type of Purchase:</b></td>
			<td>Per Diem</td>
		</tr>
        <tr>
             <td><b>Business Unit Name:</b></td>
            <td>{10}</td>
        </tr>
		<tr>
			<td><b>WO or Shop?:</b></td>
			<td>{6}</td>
		</tr>
		<tr>
			<td><b>WO # (if applicable):</b></td>
			<td>{7}</td>
		</tr>
		<tr>
			<td><b>Customer (if applicable):</b></td>
			<td>{8}</td>
		</tr>
		<tr>
			<td><b>Amount:</b></td>
			<td>{3:c2}</td>
		</tr>
		<tr>
			<td><b>Date Start:</b></td>
			<td>{2}</td>
		</tr>
		<tr>
			<td><b>Date End:</b></td>
			<td>{4}</td>
		</tr>
		<tr>
			<td><b>Description of Per Diem:</b></td>
			<td>{5}</td>
		</tr>
		<tr>
			<td>Past Perdiems:</td>
			<td>{9}</td>
		</td>
	</table>
	<br/>
	<br/>
	",
								manager.FirstName,                          // {0}
								memberCurrent.FullName,                        // {1}
								exp.date_start.ToShortDateString(),         // {2}
								exp.amount,                                 // {3}
								exp.date_end.ToShortDateString(),           // {4}
								_tools.value_from(exp.item_text, false),    // {5}
								woshop,                                     // {6}
								wo_num,                                     // {7}	
								customer,                                   // {8}
								sb_past_perdiems,                            // {9}
                                memberCurrent.business_unit.name                     //{10}
                            );
							m.URLyes = $"/sections/member/expense/index.aspx?a=manager_approval&id={exp.id_expense}&type=approve";
							m.URLno = $"/sections/member/expense/index.aspx?a=manager_approval&id={exp.id_expense}&type=deny";
							m.Subject = "Per Diem needs your Approval";
							m.to_member_id = manager.id;
							m.file_passport();

							// cbp_new.JSProperties["cpResult"] = "SUCCESS";
						}
						else
						{
							// cbp_new.JSProperties["cpResult"] = !is_edit ? "SUCCESS" : "SUCCESSCLOSE";

						}
					}
					catch (Exception ee)
					{
						return "ERROR - " + ee;

					}
				}
			}

			return "Expense has been saved successfully.";
		}
	}
}