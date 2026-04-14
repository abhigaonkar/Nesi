using System;
using System.Collections.Generic;
using System.Web;
using DevExpress.Web;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using nesi.core;
// ReSharper disable CompareOfFloatsByEqualityOperator

public partial class Expense : System.Web.UI.Page
	{
	NeMember current_user;
	int current_payperiod_id;
	int id_perdiem;
	NameValueCollection _q;
	bool is_edit;
	payroll.expense originalPerdiem;
	protected void Page_Init(object sender, EventArgs e)
		{
		_q								= Request.QueryString;
		current_user					= Toolbox.do_handle_authentication(1);
		Session["business_unit_id"]		= current_user.business_unit_id.ToString();
		hdn_member.Value				= current_user.id.ToString();
		current_payperiod_id			= new NePayPeriod().CurrentPayPeriod();
		if(!string.IsNullOrEmpty(_q["id_perdiem"]))
			{
			int.TryParse(_q["id_perdiem"], out id_perdiem);
			is_edit = true;
			}
        ddl_bu.DataSource = Toolbox.doSQL_dt(@"Select distinct b.id,ddl_name from business_unit b inner join member on member.business_unit_id = b.id where (is_supervisor(member.member_id," + current_user.id32 + @") or member.member_id = " + current_user.id32 + @")  and b.id in(" + new Current_User().visible_business_units + ") order by ddl_name",null);
        ddl_bu.DataBind();
     
		ds_expense.SelectParameters.Add("@member_id", current_user.id.ToString());
		if(!string.IsNullOrEmpty(_q["is_mobile"]) && _q["is_mobile"] == "1")
			{
			// Reformat to mobile friendly
			var height								= Unit.Pixel(35);
			pc_main.Width							= Unit.Percentage(100);
			pc_main.TabStyle.Width					= Unit.Percentage(25);
			pc_main.TabStyle.Paddings.PaddingLeft	= Unit.Pixel(7);
			pc_main.TabStyle.Paddings.PaddingRight	= Unit.Pixel(7);
			if_newexpense.Attributes["src"]			= "if_newexpense.aspx?type=0&is_mobile=1";
			if_newexpense.Style["width"]			= "100%";
			if_newexpense.Style["height"]			= "750px";
			perdiem_c_workorder.Height				= perdiem_t_rate.Height = perdiem_date_start.Height = perdiem_date_end.Height = height;
			Toolbox.do_add_css(Page, "./mobile.css");
			}
		else
			{
			Toolbox.do_add_css(Page, "./desktop.css");
			}
		if(!string.IsNullOrEmpty(_q["id_expense"]))
			{
			if_newexpense.Attributes["src"]		+= "&id_expense="+_q["id_expense"];
			}
		sm.RegisterPostBackControl(btn_export);
		if(is_edit)
			{
			originalPerdiem = new payroll.expense(id_perdiem);
			pc_main.ShowTabs = false;
			pc_main.ActiveTabIndex = 1;
			pc_main.ContentStyle.Paddings.Padding = Unit.Pixel(0);
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		using(var conn = Toolbox.connect())
			{
			var _q		= Request.QueryString;
			if(!string.IsNullOrEmpty(_q["a"]) && !string.IsNullOrEmpty(_q["id"]) && !string.IsNullOrEmpty(_q["type"]))
				{
				Response.Clear();
				var action			= _q["a"];
				var id				= Convert.ToInt32(_q["id"]);
				var type			= _q["type"];
				var ex				= new payroll.expense(id);
				if(action != "manager_approval" || type != "approve" && type != "deny")
					{
					Response.Write("Invalid Attempt");
					Response.End();
					}
				var bs			= ex.review(conn, type == "approve", current_user, false);
				Response.Write(
				    $@"{bs.message}.<br/><button onclick=""top.document.location.href ='/#/signin'; sessionStorage.clear();"">Home</button>");
				Response.End();
              
				}
			    if (!IsPostBack && !IsCallback)
			        {
			        ddl_bu.Value = current_user.business_unit_id;
			        }
			    if (IsPostBack || IsCallback) return;
            var pp_id                   = NePayPeriod.CurrentOpenPayPeriodId();
			var pp                      = new NePayPeriod(pp_id);
			perdiem_date_start.MinDate  = pp.start_date;
			perdiem_date_end.MinDate    = pp.start_date;
			perdiem_t_rate.Text         = new NeBusinessUnit(ddl_bu.Value).perdiem_rate.ToString();
			//perdiem_lb_payperiod.Text   = string.Format("{0:MMMM dd, yyyy} - {1:MMMM dd, yyyy}", pp_start, pp_end);
			  
            

            hid_empname.Value           = current_user.FullName;
			hid_country.Value           = new NeBusinessUnit(ddl_bu.Value).country;
			cl_employees.DataBind();
			foreach (ListEditItem li in cl_employees.Items)
				{
				if (li.Value.ToString() == current_user.id.ToString())
					{
					li.Selected         = true;
					}
				}
			}
		if(is_edit && !IsPostBack)
			{
			ddl_bu.ClientEnabled = false;
			perdiem_date_start.Date = originalPerdiem.date_start;
			perdiem_date_end.Date = originalPerdiem.date_end;
			cl_employees.Value = originalPerdiem.id_member;
			cl_employees.ClientEnabled = false;
			perdiem_lb_desc.Text = string.Format("{{Employee Name}} - Per Diem ({0}) to ({1})", Toolbox.MySQL_shortdt(originalPerdiem.date_start), Toolbox.MySQL_shortdt(originalPerdiem.date_end));
			perdiem_lb_calculated.Text = originalPerdiem.amount.ToString("C0");
			if(originalPerdiem.woprog_id == 0)
				{
				perdiem_cb_shop.Checked = true;
				}
			else
				{
				perdiem_c_workorder.Value = originalPerdiem.woprog_id;
				}
			}
		}

	protected void cb_new_Callback(object sender, CallbackEventArgsBase e)
		{
		using (var conn = Toolbox.connect())
			{
			double thisRate;
			double.TryParse(perdiem_t_rate.Text, out thisRate);
			if (cl_employees.SelectedItems.Count <= 0)
				{
				throw new Exception("You must select at least one employee.");
				}
			if (thisRate == 0)
				{
				throw new Exception("The supplied rate (" + perdiem_t_rate.Text + ") is not valid. Please try again, and make sure it is numeric only.");
				}
			var fromId = NePayPeriod.get_payperiod_id(perdiem_date_start.Date);
			var toId = NePayPeriod.get_payperiod_id(perdiem_date_end.Date);
			if (fromId != toId)
				{
				throw new Exception("A request cannot span multiple pay periods. Please adjust your dates.");
				}
			var sbError = new StringBuilder();
			var errors = new List<int>();
			foreach (ListEditItem li in cl_employees.Items)
				{
				if (!li.Selected) continue;
				var mid = Convert.ToInt32(li.Value);
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
				throw new Exception(sbError.ToString());
				}



			// Check date range for duplicates
			foreach (ListEditItem li in cl_employees.Items)
				{
				if (!li.Selected) continue;
				var mid = li.Value.ToString();
				var memberCurrent = new NeMember(Convert.ToInt32(mid));
				var manager = new NeMember();
				var managerList = NeMember.supervisor_list(memberCurrent.id);

				var c_dupes = 0;
				if(is_edit)
					{
					c_dupes = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM expense_reimbursement
WHERE id_expense != @v3 AND approved IN (1, -1) AND item_text LIKE '% - Per Diem (%' AND id_member = @v0 AND ((date_start BETWEEN @v1 AND @v2) OR (date_end BETWEEN @v1 AND @v2) OR 
(@v1 BETWEEN date_start AND date_end))", new object[] { mid, Toolbox.MySQL_shortdt(perdiem_date_start.Date), Toolbox.MySQL_shortdt(perdiem_date_end.Date), originalPerdiem.id_expense });
					}
				else
					{
					c_dupes = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM expense_reimbursement
WHERE approved IN (1, -1) AND item_text LIKE '% - Per Diem (%' AND id_member = @v0 AND ((date_start BETWEEN @v1 AND @v2) OR (date_end BETWEEN @v1 AND @v2) OR 
(@v1 BETWEEN date_start AND date_end))", new object[] { mid, Toolbox.MySQL_shortdt(perdiem_date_start.Date), Toolbox.MySQL_shortdt(perdiem_date_end.Date) });
					}
				//if (c_dupes > 0)
				//	{
				//	throw new Exception(string.Format(li.Text + " already has per diems saved within that date range ({0} to {1})... cannot save request.", Toolbox.MySQL_shortdt(perdiem_date_start.Date), Toolbox.MySQL_shortdt(perdiem_date_end.Date)));
				//	}
				//	return;
				var amount = perdiem_amount();
				var is_shop = perdiem_cb_shop.Checked;
				// Check if expense exists.
				var exp = new payroll.expense();
				if(is_edit)
					{
					exp = originalPerdiem;
					exp.date_start = perdiem_date_start.Date;
					exp.date_end = perdiem_date_end.Date;
					exp.amount = amount;
					}
				else
					{
					exp = new payroll.expense
						{
						id_seller = 309,
						receipt_number = "",
						item_text = string.Format("{0} - Per Diem ({1:yyyy-MM-dd}) to ({2:yyyy-MM-dd})", memberCurrent.FullName, perdiem_date_start.Date, perdiem_date_end.Date),
						amount = amount,
						master_id = 360,
						date_purchased = DateTime.Now,
						date_start = perdiem_date_start.Date,
						date_end = perdiem_date_end.Date,
						id_member = memberCurrent.id,
						currency = memberCurrent.Country,
						id_payperiod = NePayPeriod.get_payperiod_id(perdiem_date_start.Date)
						};
					}
				var wo_dc = new NeWODetailCurrent();
				if (!is_shop)
					{
					if(is_edit && originalPerdiem.woprog_id > 0)
						{
						var woRemove = new NeWOProg(originalPerdiem.woprog_id);
						var bsRemove = payroll.expense.detach(conn, woRemove, originalPerdiem, ref wo_dc, current_user);
						if(!bsRemove.success)
							{
							throw new Exception(bsRemove.message);
							}
						}
					exp.woprog_id = Convert.ToInt32(perdiem_c_workorder.Value);
					var wo = new NeWOProg(exp.woprog_id);
					if (wo.Status != "Open")
						{
						throw new Exception("Work order is no longer open to expenses.");
						}
					if (wo.business_unit_id != memberCurrent.business_unit_id)
						{
						throw new Exception("Work order is from a different branch, please select another.");
						}
					var bs = payroll.expense.attach(conn, wo, exp, ref wo_dc, current_user);
					if (!bs.success)
						{
						cbp_new.JSProperties["cpResult"] = "ERROR - " + bs.message;
						return;
						}
					else
						{
						manager = new NeMember(wo.intProjectManager);
						}
					}
				else
					{
					if(is_edit && exp.woprog_id > 0)
						{
						var woRemove = new NeWOProg(originalPerdiem.woprog_id);
						var bsRemove = payroll.expense.detach(conn, woRemove, originalPerdiem, ref wo_dc, current_user);
						if(!bsRemove.success)
							{
							throw new Exception(bsRemove.message);
							}
						}
					exp.woprog_id = 0;
					}
				if (manager.id == 0)
					{
					manager = new NeMember(Convert.ToInt32(memberCurrent.reports_to));
					}
				if (manager.id == 0)
					{
					manager = memberCurrent.business_unit.branch_manager;
					}
				if(manager.id == memberCurrent.id)
					{
					manager = new NeMember(memberCurrent.reports_to);
					}
				try
					{
					exp.save();
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
					var wo_num = !is_shop ? new NeWOProg(exp.woprog_id).OrderNumber : "N/A";
					var customer = "N/A";
					try
						{
						customer = !is_shop ? new NECustomer((int)new NeWOProg(exp.woprog_id).WOProg_Customer_ID).Customer_Name : "N/A";
						}
					catch (Exception ee)
						{
						Toolbox.do_errorLog_errorStack(ee);
						}
					if (!managerList.Contains(current_user.id) && !is_edit)
						{
						var m = new NeEMail
							{
							To = manager.NEEmail,
							From = current_user.NEEmail
							};
						var dt_past_perdiems = Toolbox.doSQL_dt(conn, @"SELECT item_text,amount FROM expense_reimbursement WHERE item_text LIKE '% - Per Diem (%' AND id_member = @v0  ORDER BY date_end DESC", new object[] { memberCurrent.id });
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
								Server.HtmlEncode(exp.item_text),			// {5}
								woshop,                                     // {6}
								wo_num,                                     // {7}	
								customer,                                   // {8}
								sb_past_perdiems,                            // {9}
                                memberCurrent.business_unit.name            //{10}
								);
						m.URLyes = string.Format("/sections/member/expense/index.aspx?a=manager_approval&id={0}&type=approve", exp.id_expense);
						m.URLno = string.Format("/sections/member/expense/index.aspx?a=manager_approval&id={0}&type=deny", exp.id_expense);
						m.Subject = "Per Diem needs your Approval";
						m.to_member_id = manager.id;
						m.file_passport();

						cbp_new.JSProperties["cpResult"] = "SUCCESS";
						}
					else
						{
						cbp_new.JSProperties["cpResult"] = !is_edit ? "SUCCESS" : "SUCCESSCLOSE";

						}
					}
				catch (Exception ee)
					{
					cbp_new.JSProperties["cpResult"] = "ERROR - " + ee;
					Toolbox.do_errorLog_errorStack(ee);
					throw;
					}
				}
						gv_expense.DataBind();
						clear_form();
			}
		}
	private double perdiem_amount()
		{
		var ts					= perdiem_date_end.Date - perdiem_date_start.Date;
		double rate;
		double.TryParse(perdiem_t_rate.Text, out rate);
		var amount				= (ts.Days+1)*rate;
		return amount;
		}
	protected void clear_form()
		{
		perdiem_c_workorder.DataBind();
		perdiem_cb_shop.Checked		       = false;
		perdiem_lb_calculated.Text	       = "";
		perdiem_lb_desc.Text		       = "";
		perdiem_c_workorder.Value	       = "";
		perdiem_c_workorder.Text		   = "";
		perdiem_c_workorder.ClientEnabled  = true;
		perdiem_date_start.Value		   = null;
		perdiem_date_end.Value			   = null;
		perdiem_date_start.Text			   = "";
		perdiem_date_end.Text			   = "";
		perdiem_t_rate.Text  = new NeBusinessUnit(ddl_bu.Value).perdiem_rate.ToString();
        gv_expense.DataBind();
		}
	protected void c_workorder_Callback(object sender, CallbackEventArgsBase e)
		{
		var cb					= (ASPxComboBox) sender;
		cb.DataBind();
		cb.SelectedIndex		= -1;
		}
	protected void gv_expense_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var gv			= (ASPxGridView) sender;
		var dr_exp			= Toolbox.doSQL_dt(@"SELECT * FROM expense_reimbursement WHERE id_expense = @v0 ", new object[] {  e.Keys[0] } ).Rows[0];
		var woprog_id			= 0;
		int.TryParse(dr_exp["woprog_id"].ToString(), out woprog_id);
		var description		= dr_exp["item_text"].ToString();
		var seller_id			= Convert.ToInt32(dr_exp["id_seller"]);
		    var exp_mem = dr_exp["id_member"].ToString();

        var affected_workorder_result		= "";
		if(woprog_id > 0)
			{
			var wo			= new NeWOProg(woprog_id);
			if(wo.Status != "Open")
				{
				throw new Exception("This work order is no longer open, you cannot delete an expense through this interface, it must be manually denied");
				}
			var c				= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current 
WHERE wo_detail_current_woprog_id = @v0 and wo_detail_current_master_id = 9595 and wo_detail_current_description = @v1", new object[] { woprog_id, description});
			if(c == 1)
				{
				var dr_wo	= Toolbox.doSQL_dt(@"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  and wo_detail_current_master_id = 9595 and wo_detail_current_description = @v1 ", new object[] {  woprog_id, description } ).Rows[0];
				NeWODetailCurrent.delete_workorder_line(Convert.ToInt32(dr_wo["wo_detail_current_id"]), 9595, current_user);
				NeWOProg.update_header_totals(woprog_id.ToString(), wo.business_unit_id, wo.OrderNumber);
				affected_workorder_result		= string.Format("Work order #{0} has been updated.", wo.OrderNumber);
				}
			else if(c > 1)
				{
				throw new Exception("There are more than one entries on that work order matching this expense, it might need to be manually denied.");
				}
			}
		else
			{
			affected_workorder_result		= "This was a shop expense, no work order needed to be updated.";
			}
		Toolbox.doSQL_void(@"DELETE FROM expense_reimbursement WHERE id_expense = @v0 LIMIT 1", new object[] { e.Keys[0]});
		#region Send email to reports_to

		var em = new NeEMail
					{
						To = new NeMember(new NeMember(Convert.ToInt32(exp_mem)).reports_to).NEEmail,
						From = "noreply@" + Toolbox.app_setting("DomainForEmail"),
						Subject = seller_id == 309 ? "Per diem has been deleted" : "Expense has been deleted",
						isHTML = true,
						Body = string.Format(@"
<b>{0}</b> just deleted an unapproved {1} for <b>{2}</b> - <b>Description</b> {3}.<br/>
<i>{4}</i>
",
							current_user.FullName,
							seller_id == 309 ? "per diem" : "expense",
							Convert.ToDouble(dr_exp["amount"]).ToString("c2"),
							description,
							affected_workorder_result
							)
					};
		em.Send();
		#endregion Send email to reports_to
		gv.DataBind();
		e.Cancel = true;
		}
	protected void gv_expense_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
		{
		var gv			= (ASPxGridView) sender;
		var pp			= new NePayPeriod();
		var request_pp			= Convert.ToInt32(gv.GetDataRow(e.VisibleIndex)["id_payperiod"]);
		e.Visible				= request_pp >= current_payperiod_id;
		}

    protected void ddl_bu_SelectedIndexChanged(object sender, EventArgs e)
        {
        ds_workorder.DataBind();
        cl_employees.DataBind();
    perdiem_t_rate.Text = new NeBusinessUnit(ddl_bu.Value).perdiem_rate.ToString();
        Session["business_unit_id"] = ddl_bu.Value.ToString();
        }
	    protected void btn_export_Click(object sender, EventArgs e)
	        {
	        ASPxGridViewExporter1.FileName = "Expense Entries";
	        ASPxGridViewExporter1.WriteXlsxToResponse();
	        }
}