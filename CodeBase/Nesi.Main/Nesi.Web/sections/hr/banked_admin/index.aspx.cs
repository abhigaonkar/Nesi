using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using DevExpress.Web;
using System.Collections.Generic;
using System.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using nesi.core;

public partial class BankedAdmin : System.Web.UI.Page
	{
///////////////////////////////////////////////////////////////////////////////////////
// Canned header in all pages
///////////////////////////////////////////////////////////////////////////////////////
	public NeMember myMember;
	private const int _page_id			= 48; // from Page table in DB
	private const string _page_description	= "Banked Pay Admin";
	Toolbox _tools;
	payroll.banked_pay AdminBank;
	NameValueCollection _q;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(_page_id);
		_q									= Request.QueryString;
		if(!IsCallback && !IsPostBack)
			{
			cb_branch.Value					= myMember.business_unit_id;
			}
		AdminBank							= new payroll.banked_pay();
		if(myMember.business_unit_id != 11)
			{
			cb_branch.ClientEnabled		= false;
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var menu							= new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml					= menu.MenuHTML;
		divSide.InnerHtml					= shared.PrintSidePanelHTML(myMember);
		var lbltemp						= (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text						= _page_description;
		var can_see_wage					= myMember.AuthenticatedForPrivilege(101);
		var body							= "";
		var body_sb				= new StringBuilder();

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Query string action catches
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
		if(_q["member_id"] != null && Request.QueryString["action"] != null)
			{
			#region action handlers
			var html				= "";
			var action			= _q["action"];
			var member_id			= Convert.ToInt32(_q["member_id"]);
			Response.Clear();
			AdminBank.member_id		= member_id;
			AdminBank.admin_id		= (int)myMember.id;
			AdminBank.payperiod_id	= CurrentPayPeriod();
			var ThisMember		= new NeMember(member_id);
			var name				= ThisMember.FullName;
			var title			= "";
			var hours			= "";
			var payperiod_id		= "";
			var payrate			= "";
			var notes			= "";
			double deductablehours	= 0;
			var dollar_default	= can_see_wage ? "$0.00" : "--";
;
			switch(action)
				{
				#region ledger

				case "ledger":
						title = "Ledger for " + name;
						body = @"
										<table cellpadding='3' cellspacing='0' class='ledger'>";
						var sql_get_ledger = @"
						SELECT 
						A.payperiod_id,
						A.id, 
						A.added_by, 
						CONCAT(B.member_firstname, ' ', B.member_lastname) added_by_name,
						DATE_FORMAT(A.date, '%M %d, %Y @ %l:%i:%s%p') date, 
						A.type, 
						A.hours, 
						A.payrate, 
						(A.hours * A.payrate) total, 
						A.note 
						FROM 
						bankedpay_ledger A
						LEFT JOIN member B ON A.added_by = B.member_id
						WHERE 
						A.type in ('D', 'W', 'P', 'E') AND
						A.member_id = @v0";
						var dt_read_get_ledger = Toolbox.doSQL_dt(sql_get_ledger, new object[] { AdminBank.member_id });
						if (dt_read_get_ledger.Rows.Count >0)
							{
							body += @"
											<tr>
												<td class='head' title='DATE OF TRANSACTION'>DATE</td>
												<td class='head' title='PAYPERIOD OF TRANSACTION'>PAYPERIOD</td>
												<td class='head' title='TYPE OF TRANSACTION'>TYPE</td>
												<td class='head' title='AMOUNT OF BANKED HOURS'>HOURS</td>
												<td class='head' title='PAYRATE AT THE TIME OF TRANSACTION'>PAYRATE</td>
												<td class='head' title='NUMBER OF HOURS TIME PAYRATE'>LINE ITEM TOTAL</td>
											</tr>";
							foreach (DataRow read_get_ledger in dt_read_get_ledger.Rows)
								{
								var PAYPERIOD_ID = read_get_ledger["payperiod_id"].ToString();
								var ID = read_get_ledger["id"].ToString();
								var AddedBy = read_get_ledger["added_by"].ToString();
								var AddedByName = read_get_ledger["added_by_name"].ToString();
								var Date = read_get_ledger["date"].ToString();
								var Type = read_get_ledger["type"].ToString();
								var Hours = Convert.ToDouble(read_get_ledger["hours"].ToString());
								var PayRate = can_see_wage ? Convert.ToDouble(read_get_ledger["payrate"].ToString()) : 0;
								var Total = can_see_wage ? Convert.ToDouble(read_get_ledger["total"].ToString()) : 0;
								var Note = read_get_ledger["note"].ToString();
								if (Note.Length == 0)
									{
									Note = "Default transaction, no note supplied.";
									}
								var RowType = "";
								var Disabled = "";

								if (AdminBank.payperiod_id < CurrentPayPeriod())
									{
									Disabled = "disabled";
									}

								switch (Type)
									{
										case "W":
											Type = "Withdrawal";
											RowType = "withdrawal";
											break;
										case "D":
											Type = "Deposit";
											RowType = "deposit";
											break;
										case "R":
											Disabled = "disabled";
											Type = "Retracted";
											RowType = "retracted";
											break;
										case "E":
											Disabled = "disabled";
											Type = "Deducted";
											RowType = "deducted";
											break;
										case "P":
											Disabled = "disabled";
											Type = "Paid Out";
											RowType = "paidout";
											break;
									}

								body += string.Format(@"
											<tr class='{0}'>
												<td class='date' title='DATE OF TRANSACTION'>{3}</td>
												<td class='payperiod' title='PAYPERIOD OF TRANSACTION'>{10}</td>
												<td class='type' title='TYPE OF TRANSACTION'>{4}</td>
												<td class='hours' title='AMOUNT OF BANKED HOURS'>{5}</td>
												<td class='payrate' title='PAYRATE AT THE TIME OF TRANSACTION'>{6}</td>
												<td class='total' title='NUMBER OF HOURS TIME PAYRATE'>{7}</td>
											</tr>
											<tr class='{0}'>
												<td colspan='6' style='padding:3px;'>
													<div class='note'>
														<b>Created By:</b> {9}<br />
														<b>Note:</b> <i>{8}</i>
													</div>
												</td>
											</tr>", RowType, ID, Disabled, Date, Type, Hours.ToString("N2"),
								                      (can_see_wage ? PayRate.ToString("C2") + " / hr" : "--"),
								                      (can_see_wage ? Total.ToString("C2") : "--"), Note, AddedByName, PAYPERIOD_ID);
								}
							}
						else
							{
							body += @"
											<tr>
												<td class='null'>There are no records available for this member.</td>
											</tr>";
							}
						body += @"
										</table>
										<button onclick=""window.close();"" class='action_button'>close window</button><button onclick=""window.print();"" class='action_button'>print ledger</button>";
						break;

				#endregion
				#region withdraw
				case "withdraw":
					title = "Withdraw from " + name + @"'s account";
					body = string.Format(@"
							<table cellspacing='0' cellpadding='0' height='280' class='add'>
								<tr>
									<td width='50%' class='title'>How many hours?</td>
									<td width='50%' class='title'>Total pay</td>
								</tr>
								<tr>
									<td width='50%' align='center' class='cell'><input id='howmany' size='1' type='number' step='0.25' onkeyup='hours_test();'></td>
									<td width='50%' align='center' class='cell'><div id='total'>{4}</div></td>
								</tr>
								<tr>
									<td colspan='2' class='title'>Which Pay Period?</td>
								</tr>		
								<tr>
									<td colspan='2' align='center' style='padding:5px;border:solid 1px #070;'>{1}</td>
								</tr>
								<tr>
									<td colspan='2' class='title'>Note</td>
								</tr>		
								<tr>
									<td colspan='2' align='center' class='cell'>
										<textarea cols='30' rows='10' class='note' id='notes' style='resize:none;' onkeyup='hours_test();' placeholder='Minimum of 10 character note'></textarea>
									</td>
								</tr>	
								<tr>
									<td colspan='2'><button id='submit' class='submit' onclick='hours_submit(""withdraw"");' disabled>Withdraw Funds</button></td>
								</tr>
							</table>
							<input type='hidden' id='memberid' value='{2}'>
							<input type='hidden' id='payrate' value='{3}'>", name, SelectPayPeriod(),
						AdminBank.member_id, AdminBank.user_pay_rate(), dollar_default);
					break;
				#endregion withdraw
					#region withdrawhours
					case "withdrawhours":	
						member_id						= Convert.ToInt32(_q["member_id"]);
						hours							= _q["hours"];
						payperiod_id					= _q["payperiod"];
						payrate							= AdminBank.user_pay_rate().ToString();
						notes							= HttpUtility.HtmlDecode(Request.QueryString["notes"]);
					
						Toolbox.doSQL_void(@"INSERT INTO bankedpay_ledger (date, type, member_id, added_by, hours, payrate, payperiod_id, note)  VALUES (now(), 'W',@v0,@v1,@v2,@v3,@v4,@v5)",new object[] { member_id,AdminBank.admin_id,hours,payrate,payperiod_id,notes } );
						Response.Write("<script>opener.location.href = opener.location.href;window.close();</script>");
						Response.End();
						break;
					#endregion withdrawhours
				#region deposit
				case "deposit":
					title = "Deposit to " + name + @"'s account";
					body = string.Format(@"
								<table cellspacing='0' cellpadding='0' height='280' class='add'>
									<tr>
										<td width='50%' class='title'>How many hours?</td>
										<td width='50%' class='title'>Total pay</td>
									</tr>
									<tr>
										<td width='50%' align='center' class='cell'><input type='text' id='howmany' size='1' onkeyup='hours_test();'></td>
										<td width='50%' align='center' class='cell'><div id='total'>{4}</div></td>
									</tr>
									<tr>
										<td colspan='2' class='title'>Which Pay Period?</td>
									</tr>		
									<tr>
										<td colspan='2' align='center' style='padding:5px;border:solid 1px #070;'>{1}</td>
									</tr>
									<tr>
										<td colspan='2' class='title'>Note</td>
									</tr>		
									<tr>
										<td colspan='2' align='center' class='cell'>
											<textarea cols='30' rows='10' class='note' id='notes' onkeyup='hours_test();' style='resize:none;' placeholder='Minimum of 10 character note'></textarea>
										</td>
									</tr>	
									<tr>
										<td colspan='2'><button id='submit' class='submit' onclick='hours_submit(""add"");' disabled>Deposit Funds</button></td>
									</tr>
								</table>
								<input type='hidden' id='memberid' value='{2}'>
								<input type='hidden' id='payrate' value='{3}'>", name, SelectPayPeriod(),
											AdminBank.member_id, AdminBank.user_pay_rate(), dollar_default);
				break;
				#endregion deposit
				#region addhours
				case "addhours":	
					member_id						= Convert.ToInt32(_q["member_id"]);
					hours							= _q["hours"];
					payrate							= AdminBank.user_pay_rate().ToString();
					payperiod_id					= _q["payperiod"];
					notes							= HttpUtility.HtmlDecode(_q["notes"]);

					Toolbox.doSQL_void(@"INSERT INTO bankedpay_ledger (date, type, member_id, added_by, hours, payrate, payperiod_id, note) VALUES (now(), 'D', @v0 , @v1 , @v2 , @v3 , @v4 , @v5)", new object[] {  member_id, AdminBank.admin_id, hours, payrate, payperiod_id, notes } );

					Response.Write("<script>opener.location.href = opener.location.href;window.close();</script>");
					Response.End();
				break;
				#endregion
				#region deduct
				case "deduct":		title					= "Deduct from "+name+@"'s account";
									deductablehours			= AdminBank.balance()/AdminBank.user_pay_rate();
									body					= string.Format(@"
									<table cellspacing='0' cellpadding='0' height='280' class='deduct'>
										<tr>
											<td width='50%' class='title'>How many hours?</td>
											<td width='50%' class='title'>Total pay</td>
										</tr>
										<tr>
											<td width='50%' align='center' class='cell'><input type='text' id='howmany' size='1' onkeyup='hours_test( {4} );'></td>
											<td width='50%' align='center' class='cell'><div id='total'>{5}</div></td>
										</tr>
										<tr>
											<td colspan='2' class='title'>Which Pay Period?</td>
										</tr>		
										<tr>
											<td colspan='2' align='center' style='padding:5px;border:solid 1px #070;'>{1}</td>
										</tr>
										<tr>
											<td colspan='2' class='title'>Note</td>
										</tr>		
										<tr>
											<td colspan='2' align='center' class='cell'>
												<textarea cols='30' rows='10' class='note' id='notes' onkeyup='hours_test( {4} );'></textarea>
											</td>
										</tr>	
										<tr>
											<td colspan='2'><button id='submit' class='submit' onclick='hours_submit(""deduct"");' disabled>DEDUCT FUNDS</button></td>
										</tr>
									</table>
									<input type='hidden' id='memberid' value='{2}'>
									<input type='hidden' id='payrate' value='{3}'>", name, SelectPayPeriod(), AdminBank.member_id, AdminBank.user_pay_rate(), deductablehours.ToString("N2"), dollar_default);
				break;
				#endregion deduct
				#region deducthours
				case "deducthours":	
					member_id						= Convert.ToInt32(_q["member_id"]);
					hours							= _q["hours"];
					payperiod_id					= _q["payperiod"];
					payrate							= AdminBank.user_pay_rate().ToString();
					notes							= HttpUtility.HtmlDecode(Request.QueryString["notes"]);
					
					Toolbox.doSQL_void(@"INSERT INTO bankedpay_ledger (date, type, member_id, added_by, hours, payrate, payperiod_id, note)  VALUES (now(), 'E',@v0,@v1,@v2,@v3,@v4,@v5)",new object[] { member_id,AdminBank.admin_id,hours,payrate,payperiod_id,notes } );
					Response.Write("<script>opener.location.href = opener.location.href;window.close();</script>");
					Response.End();
				break;
				#endregion deducthours
				#region payout
				case "payout":
					title					= "Create payout for "+name+@"'s account";
					deductablehours			= Math.Round(AdminBank.balance(),2,MidpointRounding.AwayFromZero) / AdminBank.user_pay_rate();
					var payoutamount		= AdminBank.balance();// AdminBank.UserPayRate() * Math.Round(deductablehours, 2,MidpointRounding.AwayFromZero);
					var payoutamount_str	= can_see_wage ? Math.Round(payoutamount,2,MidpointRounding.AwayFromZero).ToString("C2") : "--";
					body					= string.Format(@"
					<table cellspacing='0' cellpadding='0' height='280' class='payout'>
						<tr>
							<td width='50%' class='title'>How many hours?</td>
							<td width='50%' class='title'>Total pay</td>
						</tr>
						<tr>
							<td width='50%' align='center' class='cell'><input type='text' id='howmany' size='1' onkeyup='hours_test( {4} );' value='{4}' disabled></td>
							<td width='50%' align='center' class='cell'><div id='total'>{5}</div></td>
						</tr>
						<tr>
							<td colspan='2' class='title'>Which Pay Period?</td>
						</tr>		
						<tr>
							<td colspan='2' align='center' style='padding:5px;border:solid 1px #070;'>{1}</td>
						</tr>
						<tr>
							<td colspan='2' class='title'>Note</td>
						</tr>		
						<tr>
							<td colspan='2' align='center' class='cell'>
								<textarea cols='30' rows='10' class='note' id='notes' onkeyup='hours_test( {4} );'></textarea>
							</td>
						</tr>	
						<tr>
							<td colspan='2'><button id='submit' class='submit' onclick='hours_submit(""payout"");' disabled>PAYOUT FUNDS</button></td>
						</tr>
					</table>
					<input type='hidden' id='memberid' value='{2}'>
					<input type='hidden' id='payrate' value='{3}'>", name, SelectPayPeriod(), AdminBank.member_id, AdminBank.user_pay_rate(), decimal.Round((decimal)deductablehours,2,MidpointRounding.AwayFromZero), payoutamount_str);
				break;
				#endregion payout
				#region payouthours
				case "payouthours":	
					member_id							= Convert.ToInt32(_q["member_id"]);
					hours								= _q["hours"];
					payrate								= AdminBank.user_pay_rate().ToString();
					payperiod_id						= _q["payperiod"];
					notes								= HttpUtility.HtmlDecode(_q["notes"]);
					
					Toolbox.doSQL_void(@"INSERT INTO bankedpay_ledger (date, type, member_id, added_by, hours, payrate, payperiod_id, note) VALUES (now(), 'P', @v0 , @v1 , @v2 , @v3 , @v4 , @v5 )", new object[] {  member_id, AdminBank.admin_id, hours, payrate, payperiod_id, notes } );
					Response.Write("<script>opener.location.href = opener.location.href;window.close();</script>");
					Response.End();
				break;
				#endregion payouthours
				}
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////
			// Create body for popup
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////
			#region html
			html					= string.Format(@"
					<html>
						<head>
							<link href='/App_Themes/BlueStyle/common.css' type='text/css' rel='stylesheet' />
							<link type='text/css' href='/css/base/ui.all.css' rel='Stylesheet' />	
							<script type='text/javascript' src='/js/functions.js'></script>
							<script type='text/javascript' src='/js/jquery-1.3.2.min.js'></script>
							<script type='text/javascript' src='/js/jquery-ui-1.7.1.custom.min.js'></script>
							<script type='text/javascript' src='/js/hr/banked.js'></script>
							<title>{0}</title>
						</head>
						<body>
							<div id='banked_admin_popup' align='center'>
{1}
							</div>
						</body>
					</html>", title, body);
			#endregion html
			Response.Write(html);
			Response.End();
			#endregion action handlers
			}
		//detail.InnerHtml					= body_sb.ToString();
		}
	Dictionary<int, bank_info> di_bank_info;
	class bank_info : IDisposable
		{
		public double balance  { get; set; }
		public double hours  { get; set; }
		public bool has_history  { get; set; }
		public void Dispose()
			{
			this.Dispose();
			}
		}
		
	public string SelectPayPeriod()
		{
		var _select						= @"
			<select class='payperiods' id='payperiod'>";
		var _payperiods				= Toolbox.doSQL_dt(@"SELECT PayPeriodID AS ID, date_format(startdate, '%b %d,%Y') AS START, date_format(enddate, '%b %d, %Y') AS END FROM payperiods  WHERE enddate >= from_days(to_days(curdate()) - 365) ORDER BY enddate ASC" , null);

		foreach(DataRow _payperiod in _payperiods.Rows)
			{
			var ID						= _payperiod["ID"].ToString();
			var START					= _payperiod["START"].ToString();;
			var END						= _payperiod["END"].ToString();;
			var SELECTED				= ID == AdminBank.payperiod_id.ToString() ? " SELECTED" : "";
			_select						+= string.Format(@"
				<option value='{2}' {3}>{0} - {1}</option>", START, END, ID, SELECTED);
			}
		_select						+= @"
			</select>";
		return _select;
		}
	public int CurrentPayPeriod()
		{
		var payperiodid 			= Toolbox.doSQL_int("CALL _payperiod()");
		var pp				= new NePayPeriod(payperiodid);
		if(Convert.ToDateTime(pp.Enddate) < DateTime.Now)
			{
			pp.payperiodID		= Toolbox.doSQL_int(@"SELECT payperiodid FROM payperiods WHERE startdate <= NOW() AND enddate >= NOW()");
			}
		return Convert.ToInt32(pp.payperiodID);
		}
	protected void bt_init(object sender, EventArgs e)
		{
		var bt								= (ASPxButton) sender;
		var tc		= (GridViewDataItemTemplateContainer) bt.NamingContainer;
		var dr									= tc.Grid.GetDataRow(tc.VisibleIndex);
		if(dr != null)
			{
			var id										= Convert.ToInt32(dr["id"]);
			var status								= dr["status"].ToString();
			double wage									= 0;
			var this_wage								= dr["current_wage"];
			double.TryParse(this_wage.ToString(), out wage);
			bt.JSProperties.Add("cpid", id);
			if(di_bank_info == null)
				{
				di_bank_info				= new Dictionary<int,bank_info>();
				}
			var bi					= new bank_info();
			if(di_bank_info.ContainsKey(id))
				{
				bi							= di_bank_info[id];
				}
			else
				{
				bi							= init_di_row(id);
				}
			switch(tc.Column.Name)
				{
				case "add":
					bt.Enabled							= status != "Not Active" && wage > 0;
				break;
				case "deduct":
					bt.Enabled							= status != "Not Active" && bi.has_history;
				break;
				case "payout":
					bt.Enabled							= bi.has_history;
				break;
				}
			}
		}
	private bank_info init_di_row(int member_id)
		{
		var bi					= new bank_info();
		AdminBank.member_id				= member_id;
		var this_balance				= AdminBank.balance();
		var this_payrate				= AdminBank.user_pay_rate();
		var has_history				= AdminBank.has_history();
		var BALANCE					= this_balance < 1 ? 0 : this_balance;
		var HOURS					= Math.Round(Math.Round(this_balance,2,MidpointRounding.AwayFromZero) / this_payrate, 2, MidpointRounding.AwayFromZero);
		bi.balance						= BALANCE;
		bi.hours						= HOURS;
		bi.has_history					= has_history;
		di_bank_info[member_id]			= bi;
		return bi;
		}
	protected void gv_bankedadmin_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if(di_bank_info == null)
			{
			di_bank_info				= new Dictionary<int,bank_info>();
			}
		var id							= Convert.ToInt32(gv_bankedadmin.GetDataRow(e.VisibleIndex)["id"]);
		var bi					= new bank_info();
		if(di_bank_info.ContainsKey(id))
			{
			bi							= di_bank_info[id];
			}
		else
			{
			bi							= init_di_row(id);
			}
		switch(e.DataColumn.FieldName)
			{
			case "balance":
				e.Cell.Text					= bi.balance.ToString("C2");
				if(bi.balance == 0)
					{
					e.Cell.ForeColor		= Color.LightGray;
					}
			break;
			case "hours":
				e.Cell.Text					= bi.hours.ToString("N2");
				if(bi.hours == 0)
					{
					e.Cell.ForeColor		= Color.LightGray;
					}
			break;
			}
		}
	protected void bt_export_Click(object sender, EventArgs e)
		{
		gv_bankedadmin.Columns["add"].Visible			= false;
		gv_bankedadmin.Columns["deduct"].Visible		= false;
		gv_bankedadmin.Columns["ledger"].Visible		= false;
		gv_bankedadmin.Columns["payout"].Visible		= false;
		gve_bankedadmin.WriteXlsxToResponse("banked_pay"+cb_branch.Text, true, new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
		}
	protected void gve_bankedadmin_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
		{
		if(e.RowType == GridViewRowType.Data)
			{
			if(di_bank_info == null)
				{
				di_bank_info				= new Dictionary<int,bank_info>();
				}
			var bi					= new bank_info();
			var id							= Convert.ToInt32(gv_bankedadmin.GetDataRow(e.VisibleIndex)["id"]);
			if(di_bank_info.ContainsKey(id))
				{
				bi							= di_bank_info[id];
				}
			else
				{
				bi							= init_di_row(id);
				}
			switch(e.Column.Name)
				{
				case "balance":
					e.TextValue			= bi.balance.ToString("C2");
					e.Text				= (string) e.TextValue;
				break;
				case "hours":
					e.TextValue			= bi.hours.ToString("N2");
					e.Text				= (string) e.TextValue;
				break;
				}
			}
		}
	protected void cbp_Callback(object sender, CallbackEventArgsBase e)
		{
		gv_bankedadmin.DataBind();
		}
}
