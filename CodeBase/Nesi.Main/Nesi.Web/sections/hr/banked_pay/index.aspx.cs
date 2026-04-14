using System;
using System.Data;
using System.Collections.Specialized;
using nesi.core;

public partial class BankedPay : System.Web.UI.Page
{
	NeMember myMember;
	private const int _page_id			= 28; // from Page table in DB
	private const string _page_description	= "Banked Pay";
	
    protected void Page_Load(object sender, EventArgs e)
		{
		#region Variable Declaration
		var _tools			= new Toolbox();
		myMember				= Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();
		var _q	= Request.QueryString;
		var UserBank			= new payroll.banked_pay();
		DataTable _dt;
		var action			= "";
		var title			= "";
		var subbody			= "";
		
		if(_q["action"] != null)
			{
			action				= _q["action"];
			}
			
		var this_memberid		= myMember.id;
		UserBank.member_id		= (int)this_memberid;
		UserBank.admin_id		= (int)this_memberid;
		var this_payrate		= UserBank.user_pay_rate();
		if(_q["payperiod"] != null)
			{
			UserBank.payperiod_id	= Convert.ToInt32(_q["payperiod"]);
			}
		else
			{
			UserBank.payperiod_id	= CurrentPayPeriod();
			var payp		= new NePayPeriod(UserBank.payperiod_id);
			UserBank.date_start		= payp.StartDate;
			UserBank.date_end		= payp.Enddate;
			}
		#endregion
		#region Handlers
		if(_q["hours"] != null && _q["action"] != null)
			{
			var hours	= Convert.ToDouble(_q["hours"]);
			switch(action)
				{
				case "D":	UserBank.bank_hours(hours);
				break;
				case "W":	
							UserBank.withdraw_hours(hours);
				break;
				}
			Response.Redirect("./");
			}

		if(_q["action"] == "retract" && _q["id"] != null)
			{
			var id				= Convert.ToInt32(_q["id"]);
			UserBank.retract_hours(id);
			Response.Redirect("./?action=ledger");
			}
		#endregion Handlers
		#region Pages
		switch(action)
			{
			#region withdraw
			case "withdraw":	withdraw.Disabled				= true;
								title							= "Withdraw from your Account";
								subbody							+= string.Format(@"
			<div class='withdraw' align='center'>
				<div class='available'>You have <b>{0}</b>  hour(s) available to withdraw.<br />You may withdraw hours in quarter hour increments.</div>
				<div class='bankable'>
					<input type='hidden' id='available_hours' value='{0}'><input type='hidden' id='payrate' value='{1}'>How many <b>regular time</b> hours would you like to withdraw? <br/>
					<input id='requested_hours' onclick='this.select();'  onkeyup='check_hours();' type='text' value='' size='1'> <button onclick='check_hours(""W"");'>Withdraw Hours</button>
				</div>
			</div>", UserBank.withdrawable_hours().ToString("N2"), this_payrate);
			break;
			#endregion withdraw
			#region deposit
			case "deposit":		deposit.Disabled				= true;
								title							= "Deposit to your Account";
								subbody							+= string.Format(@"
			<div class='deposit' align='center'>
				<div class='available'>You have <b>{0}</b>  hour(s) available to bank this payroll.<br />You may deposit hours in quarter hour increments.</div>
				<div class='bankable'>
					<input type='hidden' id='available_hours' value='{0}'>How many <b>regular time</b> hours would you like to bank? <br/>
					<input id='requested_hours' onclick='this.select();'  onkeyup='check_hours();' type='text' value='' size='1'> <button onclick='check_hours(""D"");'>Bank Hours</button></div>
			</div>", UserBank.available_hours().ToString("N2"));
			break;
			#endregion deposit
			#region ledger
			case "ledger":		ledger.Disabled						= true;
								title								= "Your Account Ledger";
								#region Transactions
								subbody								= @"
										<table cellpadding='3' cellspacing='0' class='ledger'>";
								_dt									= _tools.getSQL_datatable(@" SELECT A.id, A.added_by, CONCAT(B.member_firstname, ' ', B.member_lastname) added_by_name, DATE_FORMAT(A.date, '%M %d, %Y @ %l:%i:%s%p') date, A.type, A.hours, A.payrate, (A.hours * A.payrate) total, A.note FROM bankedpay_ledger A LEFT JOIN member B ON A.added_by = B.member_id  WHERE A.member_id =@v0 and A.payperiod_id =@v1  and A.type in ('W', 'D', 'R', 'E', 'P')", new object[] { UserBank.member_id,UserBank.payperiod_id });
						
								if(_dt.Rows.Count > 0)
									{
									subbody							+= @"
											<tr>
												<td class='head' title='ACTIONS AVAILABLE FOR ACCOUNT'>&nbsp;</td>
												<td class='head' title='DATE OF TRANSACTION'>DATE</td>
												<td class='head' title='TYPE OF TRANSACTION'>TYPE</td>
												<td class='head' title='AMOUNT OF BANKED HOURS'>HOURS</td>
												<td class='head' title='PAYRATE AT THE TIME OF TRANSACTION'>PAYRATE</td>
												<td class='head' title='NUMBER OF HOURS TIME PAYRATE'>LINE ITEM TOTAL</td>
											</tr>";
									foreach(DataRow _dr in _dt.Rows)
										{
										#region Variable Declaration
										var ID					= _dr["id"].ToString();
										var AddedBy				= _dr["added_by"].ToString();
										var AddedByName			= _dr["added_by_name"].ToString();
										var Date					= _dr["date"].ToString();
										var Type					= _dr["type"].ToString();
										var Hours				= Convert.ToDouble(_dr["hours"].ToString());
										var PayRate				= Convert.ToDouble(_dr["payrate"].ToString());
										var Total				= Convert.ToDouble(_dr["total"].ToString());
										var Note					= _dr["note"].ToString();
										var RowType				= "";
										var Disabled				= "";

										if(Note.Length == 0)
											{
											Note					= "Default transaction, no note supplied.";
											}
										else
											{
											Disabled				= "disabled";
											}

										if(UserBank.payperiod_id < CurrentPayPeriod())
											{
											Disabled				= "disabled";
											}

										if(AddedBy != UserBank.member_id.ToString())
											{
											Disabled				= "disabled";
											}

										switch(Type)
											{
											case "W":		Type		= "Withdrawal";
															RowType		= "withdrawal";
											break;
											case "D":		Type		= "Deposit";
															RowType		= "deposit";
											break;
											case "R":		Disabled	= "disabled";
															Type		= "Retracted";
															RowType		= "retracted";
											break;
											case "E":		Disabled	= "disabled";
															Type		= "Deducted";
															RowType		= "deducted";
											break;
											case "P":		Disabled	= "disabled";
															Type		= "Paid Out";
															RowType		= "paidout";
											break;
											}
										#endregion
										subbody						+= string.Format(@"
											<tr class='{0}'>
												<td class='action'><button onclick='retract_entry(""{1}"");' {2}><img src='/images/icon/icon[remove].gif'></button></td>
												<td class='date' title='DATE OF TRANSACTION'>{3}</td>
												<td class='type' title='TYPE OF TRANSACTION'>{4}</td>
												<td class='hours' title='AMOUNT OF BANKED HOURS'>{5}</td>
												<td class='payrate' title='PAYRATE AT THE TIME OF TRANSACTION'>${6} / hr</td>
												<td class='total' title='NUMBER OF HOURS TIME PAYRATE'>${7}</td>
											</tr>
											<tr class='{0}'>
												<td colspan='6' style='padding:3px;'>
													<div class='note'>
														<b>Created By:</b> {9}<br />
														<b>Note:</b> <i>{8}</i>
													</div>
												</td>
											</tr>", RowType, ID, Disabled, Date, Type, Hours.ToString("N2"), PayRate.ToString("N2"), Total.ToString("N2"), Note, AddedByName);
										}
									}
								else
									{
									subbody							+= @"
											<tr>
												<td class='null'>There are no records available for this payperiod.</td>
											</tr>";
									}
								subbody								+= @"
										</table>";
								#endregion Transactions
								#region History
								subbody								+= @"
										<div class='history'>
											<div class='title'>History</div>";
								_dt									= _tools.getSQL_datatable(@"SELECT distinct(a.payperiod_id) ID, DATE_FORMAT(b.startdate, '%M %d, %Y') START, DATE_FORMAT(b.enddate, '%M %d, %Y') END FROM bankedpay_ledger a LEFT JOIN payperiods b ON a.payperiod_id = b.PayPeriodID  WHERE a.payperiod_id <=@v0 AND a.type in ('W', 'D', 'P', 'E') AND a.member_id =@v1  ORDER BY ID DESC", new object[] { UserBank.payperiod_id,UserBank.member_id });
								if(_dt.Rows.Count > 0)
									{
									subbody							+= @"
											<select class='entries' onchange='if(this.value != 0){location.href=""./?action=ledger&payperiod=""+this.value;}'>
												<option value='0'>Please choose a pay period";
									foreach(DataRow _dr in _dt.Rows)
										{
										var ID						= _dr["ID"].ToString();
										var START					= _dr["START"].ToString();
										var END						= _dr["END"].ToString();

										subbody							+= string.Format(@"
												<option value='{0}'>({0}) {1} - {2}", ID, START, END);
										}
									subbody							+= @"
											</select>";
									}
								else
									{
									subbody							+= @"
											<div class='null'><a href='./?action=ledger'>No Further Historic Information Available.</a></div>";
									}
								subbody								+= "</div>";
								#endregion History
			break;
			#endregion ledger
			#region default (history)
			default:			bankhome.Disabled						= true;
								title									= "Account Summary";
								var balance_money					= UserBank.balance();
								var balance_hours					= UserBank.withdrawable_hours();//Math.Round(Math.Round(UserBank.Balance(),2,MidpointRounding.AwayFromZero) / UserBank.UserPayRate(), 2, MidpointRounding.AwayFromZero);

								subbody							= @"
								<div class='home'>
									<div class='balance'><b>Account Balance</b><br /> <i>$"+balance_money.ToString("N2")+@"</i></div>
									<div class='hours'><b>Available Withdrawable Hours</b><br/><i>"+balance_hours.ToString("N2")+@"</i></div>
								</div>";
			break;
			#endregion
			}
		#endregion

		body.InnerHtml					= string.Format(@"
	<div class='subbody'>
		<div class='title'>{0}</div>
		{1}
	</div>", title, subbody);
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

}
