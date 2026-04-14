using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Linq;
using System.Text;
using nesi.core;

public partial class Vacation : System.Web.UI.Page
	{
	public NeMember myMember;
	private const int _page_id			= 28; // from Page table in DB
	private const string _page_description	= "Vacation";

	Toolbox _tools;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(_page_id);
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		string Output						= null;
		var this_vacation				= new vacation();
		this_vacation._tools				= _tools;
		this_vacation.member_id				= myMember.id.ToString();
		this_vacation.Initialize_Employee();
		this_vacation.business_unit_id			= myMember.business_unit_id.ToString();
		var myCompany					= new NeBusinessUnit(myMember.business_unit_id);
		var lastanniversary_year			= this_vacation.Last_Anniversary();
		var this_year						= DateTime.Now.Year;
		//bool updated_vacation				= false;
		//if(this_vacation.Is_Anniversary() && this_year > lastanniversary_year)
		//	{
		//	updated_vacation				= this_vacation.Add_Vacation();
		//	}
			
		string vacation_id;
		string note;
		var _q				= Request.QueryString;
		
		//double available_hours				= this_vacation.Available_Hours();
		//double available_money				= this_vacation.Available_Money();


		_tools.dont_cache_page();

			if(_q["a"] != null)
				{
				var action					= (string) _q["a"];
				var Payment_Methods_Select	= "";
				double available_hours		= 0;
				var Payment_Methods	= this_vacation.Payment_Methods();
				if(Payment_Methods.Rows.Count > 0)
					{
					Payment_Methods_Select		= @"
	<select id='payment_method' onchange='payment_amount_toggle();check_request();'>";
					foreach(DataRow dr in Payment_Methods.Rows)
						{	
						var method_id			= dr["method_id"].ToString();
						var method				= (string) dr["method"];
						Payment_Methods_Select	+= string.Format(@"
		<option value='{0}'>{1}", method_id, method);
						}
					Payment_Methods_Select		+= @"
	</select> <input type='text' id='amount' size='1' onfocus='this.select();check_request();' onblur='check_request();' onkeyup='check_amount();check_request();/*separate_check()*/'> <b id='amount_type'><i class='money'></i><!-- <div style='display:none;' class='separate_check'><input type='checkbox' />Separate Check/Deposit</div> -->";
					}
				else
					{
					Payment_Methods_Select		= "Could Not Get Payment Methods.";
					}
				switch(action)
					{
					case "schedule":
						#region schedule
						var today					= string.Format("{0:MM/dd/yyyy}", DateTime.Now);
						this_vacation.payperiod_id		= this_vacation.CurrentPayPeriod();
						this_vacation.member_id			= myMember.id.ToString();
						var alloutstanding_available	= this_vacation.All_Outstanding_Available();
						var current_payperiod		= this_vacation.CurrentPayPeriod();
						var shown_header				= "For an exact amount of your available vacation, please refer to your most recent paystub.";
						var alloutstanding_vacation		= myMember.paytype_id == 3 
															? "<td rowspan='5' align='center'><input type='checkbox' id='cb_alloutstanding_1' onclick='payment_type_toggle(this);' style='display:none;' />&nbsp;&nbsp;Unpaid Timeoff<br/><input type='checkbox' id='cb_unpaid_1' onclick='payment_type_toggle(this);' /></td>" 
															: "<td align='center'>All Outstanding<br/><input type='checkbox' id='cb_alloutstanding_1' onclick='payment_type_toggle(this);' /></td><td rowspan='3'>or</td><td align='center'>Unpaid Timeoff<br/><input type='checkbox' id='cb_unpaid_1' onclick='payment_type_toggle(this);' /></td>";
						var outstanding_withdrawal	= alloutstanding_available 
															? "<tr><td align='center'><input type='text' id='hours_2' class='amount'  onkeyup='calculate_money(this,2)' placeholder='Hours' /></td><td align='center'><input type='text' id='money_2' class='amount' onkeyup='calculate_hours(this,2)' placeholder='Dollars' /></td><td align='center'><input type='checkbox' id='cb_alloutstanding_2' placeholder='Dollars'  onclick='payment_type_toggle(this);'/><label for='cb_alloutstanding_2'>All Outstanding</label></td></tr>"
															: "<tr><td align='center'><b style='color:#c00;'>An all outstanding request already exists for this pay period, no further withdrawals may be requested.</b><input type='hidden' id='hours_2' value='0' /><input type='hidden' id='money_2' value='0' /></td></tr>";
						Output								+= string.Format(@"
					 <table width='100%' cellpadding='0' cellspacing='0' class='schedule_vacation'>
						<tr>
							<td colspan='3' align='left'><button class='nav_button' onclick=""location.href='./index.aspx'"">Back</button></td>
						</tr>
						<tr>
							<td colspan='3' style='padding:5px;' align='center'>
											{4}
											<input type='hidden' id='pay_rate' value='{2}'>
											<input type='hidden' id='available_vacation' value='{0}'>
							</td>
						</tr>
						<tr>
							<td style='padding-top:10px;font-size:1.5em;' align='center' colspan='3'>Please choose one of the options below<div style='font-size:0.75em;'>* denotes a required field</div></td>
						</tr>
						<tr>
							<td valign='top' class='option' onclick='toggle_option(1,this);'>
								<div class='option_head_wrapper'>
									<input type='hidden' value='false'/>
									<div class='option_head' align='center'>Schedule Vacation</div>
									<div class='option_head_sub'>This will only allow you to schedule whole days off <br/> Paid out in 8 hour increments per day.</div>
								</div>
								<table width='100%' cellpadding='5' cellspacing='0' id='option_1'>
									<tr>
										<td class='c' align='right' width='50%'>*Start Date:</td>
										<td class='v' align='left'><input	type	= 'text'
																class	= 'date'
																id		= 'start_date'
																placeholder = 'YYYY-MM-DD' 
																onkeyup	= 'check_request();' 
																onblur	= 'check_request();'></td>
									</tr>
									<tr>
										<td class='c' align='right' width='40%'>*End Date:</td>
										<td class='v' align='left'><input	type	= 'text'
																class	= 'date'
																id		= 'end_date'
																placeholder = 'YYYY-MM-DD' 
																onkeyup	= 'check_request();'
																onblur	= 'check_request();'
																disabled = 'disabled'></td>
									</tr>
									<tr>
										<td class='c' align='right' width='40%'>*Return Date:</td>
										<td class='v' align='left'><input	type	= 'text'
																			class	= 'date'
																			id		= 'return_date'
																placeholder = 'YYYY-MM-DD' 
																			onkeyup	= 'check_request();' 
																			onblur	= 'check_request();'
																			disabled = 'disabled'></td>
									</tr>
									<tr>
										<td colspan='2' align='center'>
											<table cellpadding='5' cellspacing='0' width='100%' class='pay_box'>
												<tr>
													<td colspan='3' class='pay_head'>Payment Options - Optional</td>
												</tr>
												<tr>
													{6}
												</tr>
												<tr>
													<td colspan='3' style='font-size:10px;'><div id='available_note'>If at the time of payroll it is found that the amount of money you have requested is greater than what you have available, only the available money will be granted</div></td>
												</tr>
											</table>
											<input type='hidden' id='current_payperiod' value='{3}' />
										</td>
									</tr>
								</table>
							</td>
							<td width='50%' valign='top' class='option' onclick='toggle_option(2,this);'>
								<div class='option_head_wrapper'>
									<input type='hidden' value='false'/>
									<div class='option_head' align='center'>Vacation Withdrawal</div>
									<div class='option_head_sub' align='center'>Withdrawals are applied toward the current payroll.<br/><br/></div>
								</div>
								<table width='100%' cellpadding='5' cellspacing='0' id='option_2'>
									<tr>
										<td>
											<table cellpadding='5' cellspacing='0'  class='pay_box' width='100%'>
												<tr>
													<td colspan='3' class='pay_head'>Payment Options - Required</td>
												</tr>
												<tr>
													<td colspan='3' align='center'>Choose withdrawal type</td>
												</tr>
												{5}
											</table>
											<div style='font-size:11px;text-align:center;'><br/>Unless cancelled, this request will be automatically approved and will appear on this payroll's check. <br/><br/> After submital, you are allowed to cancel this request up until your branch's payroll is completed.</div>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td colspan='3' align='center'>
								<div class='add_note' style='margin-top:5px;' onclick=""$('#note').slideToggle('fast');"">Attach Note?</div>
								<textarea id='note' class='note'></textarea>
							</td>
						</tr>
						<tr>
							<td colspan='3' align='center'>
								<br/>
								<button id='submit' type='button' class='nav_button' onclick='next_step(this);' disabled>next &gt;&gt;</button>
							</td>
						</tr>
					 </table>
					 <table cellpadding='0' cellspacing='0' width='100%' class='review'>
						<tr>
							<td class='head'>Please review the following information that is about to be sent for approval.</td>
						</tr>
						<tr>
							<td class='points'>
								<table cellpadding='3' cellspacing='0' width='100%'></table>
							</td>
						</tr>
						<tr>
							<td class='buttons'><button onclick='prevstep()'>&lt;&lt; back</button>&nbsp;<button id='review_submit' onclick=""$(this).attr('disabled', true);submit_request();"">finish &gt;&gt;</button></td>
						</tr>
					</table>
					<script>
						$('document').ready(function()
							{{
							$('#start_date').keydown(function(event) {{event.preventDefault();}});
							$('#end_date').keydown(function(event) {{event.preventDefault();}});
							$('#return_date').keydown(function(event) {{event.preventDefault();}});
							}});
					</script>
					 ", 
					 this_vacation.Available_Hours(),					// 0
					 this_vacation.Available_Money(),					// 1
					 this_vacation.UserPayRate().ToString("N2"),		// 2
					 current_payperiod,									// 3
					 shown_header,										// 4
					 outstanding_withdrawal,								// 5
					 alloutstanding_vacation							// {6}
					 );
					#endregion schedule
					break;
					case "note-add":
						#region note-add
						Response.Clear();
						this_vacation.member_id				= myMember.id.ToString();
						vacation_id							= _q["vacation_id"];
						this_vacation.vacation_id			= vacation_id;
						note								= _q["note"];
						if(this_vacation.Request_Note_Add(note))
							{
							Response.Write("SUCCESS");
							}
						else
							{
							Response.Write("FAILED");
							}
						Response.End();
						#endregion note-add
					break;
					case "submit":
						#region submit
						Response.Clear();
						available_hours		= Toolbox.doSQL_double(@"CALL VACATION_GATEKEEPER(@v0 , @v1 )", new object[] {  myMember.id, (_q["start_date"] == null ? Toolbox.MySQLNow_short() : _q["start_date"]) } );
						try
							{
							this_vacation.o					= _q["o"];
							if(_q["start_date"] != null)
								{
								this_vacation.date_start		= _q["start_date"];
								}
							if(_q["end_date"] != null)
								{
								this_vacation.date_end		= _q["end_date"];
								}
							if(_q["return_date"] != null)
								{
								this_vacation.date_return		= _q["return_date"];
								}
							//this_vacation.separate_check	= _q["separate_check"];
							
							var _this_hours				= Convert.ToDouble(_q["hours"]);
							var _this_money				= Convert.ToDouble(_q["money"]);
							if(_q["hours"] == "0" && _q["money"] != "0")
								{
								this_vacation.hours			= Math.Round(_this_money / this_vacation.UserPayRate(), 5); 
								}
							else
								{
								this_vacation.hours			= _this_hours;
								}
							this_vacation.note				= _q["note"];
							this_vacation.all_outstanding	= Convert.ToBoolean(_q["alloutstanding"]);
							this_vacation.unpaid			= Convert.ToBoolean(_q["unpaid"]);
							this_vacation.save();
							Response.Write("SUCCESS");
							}
						catch (Exception ee)
							{
							_tools.catch_error(ee);
							Response.Write(ee.ToString());
							}
						Response.End();
						#endregion submit
					break;
					case "cancel":
						#region cancel
						Response.Clear();
						try
							{
							vacation_id			= (string) _q["vacation_id"];
							if(this_vacation.Request_Cancel(vacation_id))
								{
								Response.Write("SUCCESS");
								}
							else
								{
								Response.Write("FAILED");
								}
							}
						catch (Exception ee)
							{
                                _tools.catch_error(ee);
							Response.Write("FAILED");
							}
						Response.End();
						#endregion cancel
					break;
					case "get_payperiods":
						#region get_payperiods
						Response.Clear();
						try
							{
							if(_q["start_date"] != null && _q["end_date"] != null)
								{
								var start		= _q["start_date"];
								var end			= _q["end_date"];
								var _return		= _tools.getSQL_string(@"select GROUP_CONCAT(Date_FORMAT(startdate, '%M %d %Y'),' - ',DATE_FORMAT(enddate, '%M %d %Y')) from payperiods where startdate between @v0  AND @v1  or enddate between @v0  AND @v1 ", new object[] {  start, end } );
								if(_return == "")
									{
									_return			= _tools.getSQL_string(@"SELECT CONCAT(Date_FORMAT(startdate, '%M %d %Y'),' - ', DATE_FORMAT(enddate, '%M %d %Y')) FROM payperiods WHERE startdate <= @v0  AND enddate >= @v1 ", new object[] {  start, end } );
									}
								Response.Write(_return);
								}
							else
								{
								Response.Write("FAILED");
								}
							}
						catch (Exception ee)
							{
							Response.Write("FAILED");
                            _tools.catch_error(ee);
							}
						Response.End();
						#endregion get_payperiods
					break;
					case "get_payperiod":
						#region get_payperiod
						Response.Clear();
						try
							{
							Response.Write(_tools.getSQL_string(@"SELECT CAST(CONCAT(DATE_FORMAT(startdate, '%m/%d/%Y'), ' - ', DATE_FORMAT(enddate, '%m/%d/%Y')) AS CHAR) FROM payperiods  WHERE payperiodid =@v0", new object[] { this_vacation.CurrentPayPeriod()}));
							}
						catch (Exception ee)
							{
							Response.Write("FAILED");
                            _tools.catch_error(ee);
							}
						Response.End();
						#endregion get_payperiod
					break;
					case "all_outstanding_available":
						#region all_outstanding_available
						Response.Clear();
						if(_q["start_date"] != null)
							{
							try
								{
								var _sql							= "SELECT payperiodid FROM payperiods WHERE startdate = str_to_date('"+HttpUtility.UrlDecode(_q["start_date"])+"', '%M %d %Y')";
								this_vacation.payperiod_id			= _tools.getSQL_string(@"SELECT payperiodid FROM payperiods  WHERE startdate = str_to_date(@v0, '%M %d %Y')", new object[] { HttpUtility.UrlDecode(_q["start_date"]) });
								Response.Write(this_vacation.All_Outstanding_Available().ToString());								
								}
							catch 
								{
								Response.Write("FAILED");
								}
							}
						else
							{
							Response.Write("FAILED");
							}
						Response.End();
						#endregion all_outstanding_available_for
					break;
					case "intersecting_holidays":
						#region intersecting_holidays
						Response.Clear();
						try
							{
							if(_q["start_date"] != null && _q["end_date"] != null)
								{
								var start		= _q["start_date"];
								var end			= _q["end_date"];
								var country_appendage		= "";
								if(myMember.business_unit.country == "CDN")
									{
									country_appendage	= " holidays_canada = 1 AND ";
									}
								else if(myMember.business_unit.country == "USA")
									{
									country_appendage	= " holidays_america = 1 AND ";
									}
								var _return		= _tools.getSQL_string(@" SELECT GROUP_CONCAT(CONCAT(holidays_name, ' - ', DATE_FORMAT(holidays_date, '%Y-%m-%d'))) h FROM holidays WHERE " +  country_appendage + " holidays_date BETWEEN @v0  AND @v1 ", new object[] {  start, end} );
								Response.Write(_return);
								}
							else
								{
								Response.Write("FAILED");
								}
							}
						catch (Exception ee)
							{
							Response.Write("FAILED");
                            _tools.catch_error(ee);
							}
						Response.End();
						#endregion intersecting_holidays
					break;
					case "check_available":
						available_hours		= Toolbox.doSQL_double(@"CALL VACATION_GATEKEEPER(@v0 , @v1 )", new object[] {  myMember.id, (_q["date"] == "today" ? Toolbox.MySQLNow_short() : _q["date"]) } );
						Toolbox.QuickReponse(Response, available_hours.ToString("N2"));
					break;
					}
				}
			else
				{
				var sb				= new StringBuilder();
				this_vacation.payperiod_id	= this_vacation.CurrentPayPeriod();
				//available_hours				= Math.Round(available_hours, 2, MidpointRounding.AwayFromZero);
				//available_money				= Math.Round(this_vacation.UserPayRate() * available_hours, 2, MidpointRounding.AwayFromZero);
				sb.Append(@"
				<table cellpadding='0' cellspacing='0' class='past_requests'>
					<tr>
						<td colspan='8' align='left'>
							<button class='nav_button' type='button' onclick=""location.href='./index.aspx?a=schedule';"">Schedule / Withdraw Vacation</button>
						</td>
					</tr>
					<tr>
						<td class='header' colspan='8'>Past Requests</td>
					</tr>");

				var Past_Activity				= this_vacation.View_Past();
				if(Past_Activity.Rows.Count > 0)
					{
					sb.Append(@"
					<tr>
						<td class='row_head'>Status</td>
						<td class='row_head'>Request Date</td>
						<td class='row_head'>Start Date</td>
						<td class='row_head'>End Date</td>
						<td class='row_head'>Pay Type</td>
						<td class='row_head'>Hrs Req</td>
						<td class='row_head' colspan='2'>&nbsp;</td>
					</tr>");


					foreach(DataRow dr in Past_Activity.Rows)
						{
						var Hours_Req				= dr["hours_requested"].ToString();
						var Vacation_ID				= dr["vacation_id"].ToString();
						var Status					= dr["status"].ToString();
						var payperiod_id				= (int) dr["payperiod_id"];
						var current_payperiod			= _tools.getSQL_int(@"CALL _payperiod"  , null);
						var CancelButton_action		= "";
						switch(Status)
							{
							case "APPROVED":
							case "DENIED":
							case "PENDING":				CancelButton_action		= "onclick='cancel_request("+Vacation_ID+");'";
							break;
							case "CANCELLED":
							case "PAIDOUT":				CancelButton_action		= "";
							break;
							}
						var Date_Insert				= dr["date_insert"].ToString();
						if(Date_Insert == "")
							{
							Date_Insert					= "--";
							}
						var Date_Start				= dr["date_start"].ToString();
						if(Date_Start == "" || Date_Start == "0001-01-01")
							{
							Date_Start					= "--";
							}
						var Date_End					= dr["date_end"].ToString();
						if(Date_End == "" || Date_End == "0001-01-01")
							{
							Date_End					= "--";
							}
						var Payment_Amount			= dr["amount"].ToString();
						var Payment_Type				= Payment_Amount == "-1" ? "ALL OUTSTANDING" : dr["payment_method"].ToString();
						if((Payment_Amount == "0" && Payment_Type == "ALL OUTSTANDING") || Payment_Amount == "-1")
							{
							Payment_Amount				= "<i title='To Be Determined'>TBD</i>";
							}
						else if(Payment_Amount == "0" && Payment_Type == "WITHOUT PAY")
							{
							Payment_Amount				= "--";
							}
							
						var notes_button				= "";
						var delete_button			= "";
						this_vacation.vacation_id		= Vacation_ID;
						var Past_Notes			= this_vacation.Past_Notes();
						if(Past_Notes.Rows.Count > 0)
							{
							if(Status != "DENIED" && Status != "PAIDOUT")
								{
								notes_button				= "<button style='background-color:transparent;border:0;' id='view_notes_"+Vacation_ID+"' title='view/post notes' onclick='toggle_notes("+Vacation_ID+");'><img src='/images/icon/icon[note].gif'></button>";
								}
							}
						else
							{
							if(Status != "DENIED" && Status != "PAIDOUT")
								{
								notes_button				= "<button style='background-color:transparent;border:0;' id='view_notes_"+Vacation_ID+"' title='post note' onclick='toggle_notes("+Vacation_ID+");'><img src='/images/icon/icon[note].gif'></button>";
								}
							}
						if(Status != "DENIED" && Status != "PAIDOUT")
							{
							delete_button				= string.Format("<button style='background-color:transparent;border:0;' title='Cancel Request' onclick='cancel_request({0});'><img src='/images/icon/icon[delete].gif'></button>", Vacation_ID);
							}
						sb.AppendFormat(@"
					<tr id='request_{6}' class='row'>
						<td class='_cell'><div class='{0}'><img src='/images/pixel.gif' width='1' height='20' align='absmiddle' />{0}</div></td>
						<td class='_cell'>{1}</td>
						<td class='_cell'>{2}</td>
						<td class='_cell'>{3}</td>
						<td class='_cell'>{4}</td>
						<td class='_cell'>{8}</td>
						<td class='_cell' width='35'>{5}</td>
						<td class='_cell' width='35'>{7}</td>
					</tr>
					<tr>
						<td colspan='8' class='notes'>
							<div id='notes_{6}' style='display:none;'>", 
							Status,						// 0
							Date_Insert,				// 1
							Date_Start,					// 2
							Date_End,					// 3
							Payment_Type,				// 4
							delete_button,				// 5
							Vacation_ID,				// 6
							notes_button,				// 7
							Payment_Amount				// 8
							);	

						foreach(DataRow note_dr in Past_Notes.Rows)
							{
							var member_name			= note_dr["name_full"].ToString();
							var date					= note_dr["date"].ToString();
							note						= note_dr["note"].ToString();
							sb.AppendFormat(@"
							<div class='past_note'>
								<div class='name_plate'>{0} - <i>{1}</i></div>
								<div class='note_text'>{2}</div>
							</div>", member_name, date, note);
							}
						sb.AppendFormat(@"
							</div>
							<div id='make_note_{0}' class='make_note' align='center' style='display:none;'>
								<textarea id='note_{0}' onkeyup='check_note({0})'></textarea><br/>
								<button onclick='add_note({0});' id='add_note_{0}' disabled>add note<br><i>branch manager will see this also</i></button>
							</div>
						</td>
					</tr>", Vacation_ID);
						Output				= sb.ToString();
						}
					}
				else
					{
					Output					+= @"
					<tr>
						<td colspan='8' class='no_records' valign='middle' align='center'>Sorry "+myMember.Nickname+@", there are no records currently on file for you. <br/><a style='color:#c60;' href='./index.aspx?a=schedule'>Do you want to make a new request?</a></td>
					</tr>";
					}


				Output						+= @"
				</table>
				";
				}
				source.InnerHtml					= Output;
		}
	}

public class vacation
	{
	NeMember employee;
	public Toolbox _tools			{get; set;}
	string sql;
	public string o						= "";
	public string member_id				= "";
	public string business_unit_id			= "";
	public string member_name			= "";
	public string vacation_id			= "";
	public string payperiod_id			= "";
	public string date_start			= "";
	public string date_end				= "";
	public string date_return			= "";
	public string note					= "";
	public string payment_method		= "";
	//public string separate_check		= "";
	
	public double hours					= 0;
	
	public bool all_outstanding			= false;
	public bool unpaid					= false;
	public void Initialize_Employee()
		{
		employee					= new NeMember(Convert.ToInt32(member_id));
		}
	public double Available_Hours()
		{
		double result		= 0;
		var payrate		= UserPayRate();
		if(payrate > 0 && All_Outstanding_Available())
			{
			result			= Math.Round((Deposited_Money() - Withdrawn_Money()) / payrate, 2, MidpointRounding.AwayFromZero);
			}
		return result;
		}
	public double Available_Money()
		{
		double result		= 0;
		var payrate		= UserPayRate();
		if(payrate > 0 && All_Outstanding_Available())
			{
			result			= Math.Round(Deposited_Money() - Withdrawn_Money(), 2, MidpointRounding.AwayFromZero);
			}
		return result;
		}
	public double Deposited_Money()
		{
		return Math.Round(Toolbox.doSQL_double(@"SELECT CAST(IFNULL((SUM(hours*@v0 )), 0) AS DECIMAL(8,2)) HOURS FROM bankedpay_ledger WHERE type = 'V' AND member_id = @v1 ", new object[] {  UserPayRate(), member_id } ), 2, MidpointRounding.AwayFromZero);
		}
	public double Withdrawn_Money()
		{
		return Math.Round(Toolbox.doSQL_double(@"SELECT CAST(IFNULL((SUM(hours*" + UserPayRate() + ")), 0) AS DECIMAL(8,2)) HOURS FROM bankedpay_ledger WHERE type in ('A') AND member_id = @v0 ", new object[] {  member_id, CurrentPayPeriod() } ), 2, MidpointRounding.AwayFromZero);;
		}
	public double UserPayRate()
		{
		return Math.Round(Toolbox.doSQL_double(@"SELECT FORMAT(wage,2) AS WAGE FROM currentwage  WHERE member_id =@v0", new object[] { member_id }), 2, MidpointRounding.AwayFromZero);
		}
	public int Last_Anniversary()
		{
		return Toolbox.doSQL_int(@"SELECT last_anniversary FROM member WHERE member_id =@v0" ,new object[] { member_id });
		}
	public string CurrentPayPeriod()
		{
		var payperiodid 			= Toolbox.doSQL_int("CALL _payperiod()");
		var pp				= new NePayPeriod(payperiodid);
		if(Convert.ToDateTime(pp.Enddate) < DateTime.Now)
			{
			pp.payperiodID		= Toolbox.doSQL_int("SELECT payperiodid FROM payperiods WHERE startdate <= NOW() AND enddate >= NOW()");
			}
		return pp.payperiodID.ToString();
		}
	public DataTable Payment_Methods()
		{
		return Toolbox.doSQL_dt("SELECT * FROM vacation_payment_method",null);
		}
	public DataTable View_Past()
		{
		return Toolbox.doSQL_dt(@" SELECT vacation_id, hours_requested, status, payperiod_id, IF((date_insert IS NULL OR YEAR(date_insert) < 2000), '--', date_insert) date_insert, IF((date_start IS NULL OR YEAR(date_start) < 2000), '--', DATE_FORMAT(date_start, '%Y-%m-%d')) date_start, IF((date_end IS NULL OR YEAR(date_end) < 2000), '--', DATE_FORMAT(date_end, '%Y-%m-%d')) date_end, amount, payment_method FROM vacation  WHERE status_id != 4 AND type_id = 4 AND status IS NOT NULL AND member_id =@v0 ORDER BY date_start DESC", new object[] { member_id });
		}
	public DataTable Past_Requests()
		{
		return Toolbox.doSQL_dt(@"SELECT * FROM vacation WHERE member_id = @v0  AND type_id = 4 AND status not in ('pending', 'cancelled')", new object[] {  member_id } );
		}
	public DataTable Pending_Requests()
		{
		return Toolbox.doSQL_dt(@"SELECT * FROM vacation WHERE member_id = @v0  AND type_id = 4 AND status ='PENDING'", new object[] {  member_id } );
		}
	public DataTable Past_Notes()
		{
		return Toolbox.doSQL_dt(@" SELECT CONCAT(b.member_firstname, ' ', b.member_lastname) name_full, DATE_FORMAT(a.date_insert, '%m/%d/%Y @ %I:%i%p') date, URLDECODE(a.note) note FROM vacation_note a LEFT JOIN member b ON a.member_id = b.member_id  WHERE a.vacation_id=@v0 ORDER BY a.date_insert", new object[] { vacation_id });
		}
	private bool Update_Anniversary()
		{
		var _last		= Toolbox.doSQL_int(@"SELECT ifnull(last_anniversary, 0) last FROM member WHERE member_id =@v0", member_id);
		if (_last == 0)
			{
			_last		= Toolbox.doSQL_int("SELECT YEAR(member_startdate) FROM member WHERE member_id =@v0", member_id);
		}
		else
			{
			_last		= Toolbox.doSQL_int("SELECT YEAR(NOW())");
			}
		return _tools.getSQL_bool(@"UPDATE member SET last_anniversary = @v0  WHERE member_id = @v1  LIMIT 1", new object[] {  _last, member_id } );
		}
	public bool Add_Vacation()
		{
		decimal vacation_amount		= 0;
		var months_with_company		= employee.months_with_company;
		var vacation_interval_1		= employee.vacation_interval_1;
		var vacation_interval_2		= employee.vacation_interval_2;
		var vacation_interval_3		= employee.vacation_interval_3;
		var vacation_amount_1	= employee.vacation_amount_1;
		var vacation_amount_2	= employee.vacation_amount_2;
		var vacation_amount_3	= employee.vacation_amount_3;
		

		if(months_with_company >= vacation_interval_1 && months_with_company < vacation_interval_2)
			{
			vacation_amount		= vacation_amount_1;
			}
		else if(months_with_company >= vacation_interval_2 && months_with_company < vacation_interval_3)
			{
			vacation_amount		= vacation_amount_2;
			}
		else if(months_with_company >= vacation_interval_3)
			{
			vacation_amount		= vacation_amount_3;
			}

		if (_tools.getSQL_bool(@"INSERT INTO bankedpay_ledger (date,type, member_id, added_by, hours, payrate, note) VALUES (now(), 'V', @v2 , @v2 , @v0 , @v1 , 'Allotment per anniversary of statutory vacation time')", new object[] {  vacation_amount, UserPayRate(), member_id } ))
			{
			Update_Anniversary();
			}
		else
			{
			return false;
			}
		return true;
		}
	public bool Request_Note_Add(string note)
		{
		if(note.Length == 0)
			{
			return true;
			}
		try
			{
			_tools.getSQL_bool(@" INSERT INTO vacation_note (vacation_id,member_id,date_insert, note) VALUES ( @v0 , @v1 , now(), @v2  ) ", new object[] {  vacation_id, member_id, note } );
			}
		catch
			{
			return false;
			}
		return true;
		}
	public bool All_Outstanding_Available()
		{
		if(payperiod_id == "")
			{
			payperiod_id			= CurrentPayPeriod();
			}
		return Toolbox.doSQL_int(@"SELECT IFNULL(COUNT(*),0) c 
FROM vacation_master WHERE payperiod_id = @v0 AND member_id = @v1 AND status NOT IN (4,2) AND (payment_method = 3 OR payment_amount = -1)",
			new object[] { payperiod_id, member_id}) == 0;
		}
	public bool Is_Anniversary()
		{
		var country					= employee.Country;
		if(country == "USA")
			{			
			var startdate			= System.DateTime.Parse(Toolbox.doSQL_string(@"SELECT member_startdate FROM member WHERE member_id=@v0", member_id));
			var DoY_Start				= Toolbox.doSQL_int("SELECT DAYOFYEAR(member_startdate) FROM member WHERE member_id =@v0", member_id);
			var DoY_Now					= Toolbox.doSQL_int("SELECT DAYOFYEAR(now())");
			var initial_vac_inc			= Toolbox.doSQL_int("SELECT vacation_interval_1 FROM member WHERE member_id =@v0", member_id);
			var second_vac_inc			= Toolbox.doSQL_int("SELECT vacation_interval_2 FROM member WHERE member_id =@v0", member_id);
			var Year_Last_Anniversary	= Last_Anniversary();
			var Days_Differential		= Toolbox.doSQL_int("SELECT DATEDIFF(NOW(),member_startdate) diff FROM member WHERE member_id=@v0", member_id);
			var Month_Start				= startdate.Month;
			var Month_Now				= DateTime.Now.Month;
			var Day_Start				= startdate.Day;
			var Day_Now					= DateTime.Now.Day;
			var Year_Start				= startdate.Year;
			var Year_Now				= DateTime.Now.Year;
			var Leapyear_Start			= System.DateTime.IsLeapYear(Year_Start);
			var Leapyear_Now			= System.DateTime.IsLeapYear(Year_Now);
			if(Year_Now == Year_Start && initial_vac_inc == 0 && Year_Last_Anniversary < Year_Now)
				{
				return true;
				}
			else if(Year_Now == Year_Start && initial_vac_inc <= employee.months_with_company && Year_Last_Anniversary < Year_Now)
				{
				return true;
				}
			else if(Year_Now == Year_Start && second_vac_inc <= employee.months_with_company && Year_Last_Anniversary == Year_Now)
				{
				return true;
				}
			else if(Year_Now > Year_Start && initial_vac_inc == 0 && Year_Last_Anniversary < Year_Now && Days_Differential < 365)
				{
				return true;
				}
			else if(Year_Now > Year_Start && Year_Last_Anniversary < Year_Now)
				{
				if(Leapyear_Start == true && Month_Start >= 3)
					{
					DoY_Start--;
					}

				if(Leapyear_Now == true && Month_Now >= 3)
					{
					DoY_Now--;
					}

				if(DoY_Start <= DoY_Now)
					{
					return true;
					}
				else
					{
					return false;
					}
				}
			else
				{
				return false;
				}
			}
		else
			{
			return false;
			}
		}
	public bool Request_Note_Add(string vacation_id, string note)
		{
		if(note.Length == 0)
			{
			return true;
			}
		try
			{
			_tools.getSQL_bool(@" INSERT INTO vacation_note (vacation_id,member_id,date_insert, note) VALUES ( @v0 , @v1 , now(), @v2  ) ", new object[] {  vacation_id, member_id, note } );
			}
		catch
			{
			return false;
			}
		return true;
		}
	public bool Deduct_Vacation(string amount, string vacation_id, string type)
		{
		var this_payperiod_id	= Toolbox.doSQL_string(@"SELECT payperiod_id FROM vacation_master WHERE vacation_id =@v0 ",vacation_id);
			sql = @"
INSERT INTO bankedpay_ledger 
(
	date,
	type,
	member_id,
	added_by,
	ref_id,
	hours,
	payrate,
	payperiod_id,
	note
)
VALUES
(
	now(),
	@v6,
	@v2,
	@v2,
	@v3,
	@v0,
	@v4,
	@v5,
	CONCAT('User requested vacation time for vacation_req id ',@v1)
)";
		var paramObjects= new object[] {  amount, vacation_id, member_id, vacation_id, UserPayRate(), this_payperiod_id, type};
		try
			{
			Toolbox.doSQL_void(sql, paramObjects);
			}
		catch (Exception ee)
			{
            Toolbox.do_catch_error(ee, 711);
			return(false);
			}
		return true;
		}
	public bool Request_Cancel(string vacation_id)
		{
		try
			{
			Toolbox.doSQL_void(@"UPDATE vacation_master SET status = 4 WHERE vacation_id = @v0",vacation_id);
			Toolbox.doSQL_void(@"UPDATE bankedpay_ledger SET type = 'C' WHERE ref_id= @v0", vacation_id);
		}
		catch (Exception ee)
			{
            Toolbox.do_catch_error(ee, 711);
			return false;
			}
		return true;
		}
	public void save()
		{
		var _request	= new VacationRequest();
		if(o == "1" && !unpaid && !all_outstanding)
			{
			payment_method			= "2";
			}
		else if(unpaid)
			{
			payment_method			= "1";
			hours					= 0;
			}
		else if(all_outstanding)
			{
			payment_method			= "3";
			hours					= 0;
			}
		var last_id					= 0;
		var vacations_toapprove	= new ArrayList();
		double used_hours			= 0;
		var available_hours		= Toolbox.doSQL_double(@"CALL VACATION_GATEKEEPER(@v0 , @v1 )", new object[] {  employee.id, (_request.date_start.Year < 2000 ? Toolbox.MySQLNow_short() : Toolbox.MySQL_shortdt(_request.date_start)) } );
		if(o == "2") // Vacation pay out
			{
			_request					= new VacationRequest();
			_request.hours_requested	= 0;
			_request.payment_amount		= all_outstanding ? -1 : hours;
			_request.payment_method_id	= 2;
			_request.payperiod_id		= Convert.ToInt32(CurrentPayPeriod());
			_request.status_id			= 3;
			_request.create_member_id	= employee.id32;
			_request.business_unit_id			= Convert.ToInt32(employee.business_unit_id);
			_request.member_id			= employee.id;
			//_request.separate_check		= false;
			_request.note				= note;
			last_id						= _request.save_record();
			used_hours					= hours;
			}
		else
			{
			var requested_date_start	= Convert.ToDateTime(date_start);
			var requested_date_end		= Convert.ToDateTime(date_end);
			var requested_date_return	= Convert.ToDateTime(date_return);

			var payperiod_info		= Toolbox.doSQL_dt(@"SELECT payperiodid, startdate, enddate FROM payperiods"  , null);
			for(var d = requested_date_start; d <= requested_date_end; d = d.AddDays(1))
				{
				if(d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday)
					{
					continue;
					}
				_request					= new VacationRequest();
				_request.date_start			= d.AddHours(8);
				_request.date_end			= d.AddHours(17);
				_request.date_return		= requested_date_return.AddHours(8);
				_request.hours_requested	= 0;
				_request.payment_amount		= 8;
				_request.create_member_id	= employee.id32;
				_request.business_unit_id			= Convert.ToInt32(employee.business_unit_id);
				_request.payment_method_id	= Convert.ToInt32(payment_method);
				_request.payperiod_id		= Convert.ToInt32(payperiod_info.Select(string.Format("startdate <= '{0:yyyy-MM-dd}' AND  enddate >= '{0:yyyy-MM-dd}'", d))[0]["payperiodid"]);
				_request.status_id			= 1;
				_request.member_id			= employee.id;
				//_request.separate_check		= false;
				_request.note				= note;
				last_id						= _request.save_record();
				vacations_toapprove.Add(_request);
				used_hours					+= 8;
				}
			EmailBranchManager(vacations_toapprove);
			}
		if(available_hours < used_hours)
			{
			var em					= new NeEMail();
			em.From						= "noreply@" + Toolbox.app_setting("DomainForEmail");
			em.To						= new NeMember((int) employee.reports_to).NEEmail;
			em.CC						= "mhyde@" + Toolbox.app_setting("DomainForEmail");
			em.Subject					= "Alert: Vacation request above available hours";
			var body_appendature		= date_start == "" ? "" : " on the requested date ("+date_start+")";
			em.Body						= string.Format("{0} just requested a vacation payout in the amount of {1:N2} hours, when they only have {2:N2} hours available{4}.<br/> Please follow up with them on this.", employee.FullName, used_hours, available_hours, date_start, body_appendature);
			em.isHTML					= true;
			//em.Send();
			}
		}
	public bool EmailBranchManager(ArrayList vacations_toapprove)
		{
		var m							= new NeEMail();
		m.From								= "payroll@" + Toolbox.app_setting("DomainForEmail");
		var manager					= employee.reports_to != 0 ? new NeMember((int)employee.reports_to) : employee;
		m.To								= manager.NEEmail;
		m.Subject							= "Vacation Request";
		m.isHTML							= true;
		m.to_member_id						= manager.id;
		var body					= new StringBuilder();
		m.passport_array					= new ArrayList();
		body.AppendFormat(@"<div style='font-family:arial; font-size:12px;'>{0} has requested vacation from {1} to {2}.<br/><br/>Below are a list of days requested off.<br/>", employee.FullName, date_start, date_return);
		var ids						= new List<int>();
		foreach(VacationRequest v in vacations_toapprove)
			{
			ids.Add(v.vacation_id);
			}
		var id_list						= string.Join(",", ids.ToArray<int>());
		// const string _host					= "www.nesi.ca";//"localhost:61114";
		if(vacations_toapprove.Count > 1)
			{
			var pao		= new passport.array_object();
			pao.url_yes						= string.Format("/sections/hr/vacation_admin/index.aspx?a=email-process&vacation_id={0}&type_of=3", id_list);
			pao.url_no						= string.Format("/sections/hr/vacation_admin/index.aspx?a=email-process&vacation_id={0}&type_of=2", id_list);
			pao.text_yes					= "APPROVE ALL";
			pao.text_no						= "DENY ALL";
			m.passport_array.Add(pao);
			}
		foreach(VacationRequest v in vacations_toapprove)
			{
			var pao		= new passport.array_object();
			pao.url_yes						= string.Format("/sections/hr/vacation_admin/index.aspx?a=email-process&vacation_id={0}&type_of=3", v.vacation_id);
			pao.url_no						= string.Format("/sections/hr/vacation_admin/index.aspx?a=email-process&vacation_id={0}&type_of=2", v.vacation_id);

            // Reduced text size as it is having issue
            /*
            pao.text_yes					= string.Format("<div style='font-size:11px;'>APPROVE<br/>{0:yyyy-MM-dd htt} - {1:htt}</div>", v.date_start, v.date_end);
			pao.text_no						= string.Format("<div style='font-size:11px;'>DENY<br/>{0:yyyy-MM-dd htt} - {1:htt}</div> ", v.date_start, v.date_end);
            */
			    pao.text_yes = string.Format("APPROVE {0:yyyy-MM-dd htt} - {1:htt}", v.date_start, v.date_end);
			    pao.text_no = string.Format("DENY {0:yyyy-MM-dd htt} - {1:htt}", v.date_start, v.date_end);

            m.passport_array.Add(pao);
			}

		#region Other people taking vacation during the requested dates
		var _others					= Toolbox.doSQL_dt(@" SELECT b.member_fullname name, CAST(CONCAT(DATE_FORMAT(a.date_start, '%Y-%m-%d'),' - ', DATE_FORMAT(a.date_return, '%Y-%m-%d')) AS CHAR) timespan FROM vacation_master a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.member_id != @v0  AND a.date_start >= @v1  AND a.date_return <= @v2  AND b.business_unit_id = @v3  AND a.status = 3 ", new object[] {  employee.id, date_start, date_return, business_unit_id } );
		if(_others.Rows.Count > 0)
			{
			body.Append(@"<br/><div><u>Other employees that have time off during this timespan</u><br/>");
			foreach(DataRow _other in _others.Rows)
				{
				body.AppendFormat(@"{0} - ({1})<br/>", _other["name"], _other["timespan"]);
				}
			body.Append(@"</div><br/>");
			}
		else
			{
			body.Append(@"<br/><div>No other employees have vacations during the requested time period(s).</div><br/>");
			}			
		#endregion Other people taking vacation during the requested dates
		#region Other days off this year by this employee
		var _days = Toolbox.doSQL_dt(@"SELECT CAST(CONCAT(DATE_FORMAT(a.date_start, '%Y-%m-%d'),' - ', DATE_FORMAT(a.date_return, '%Y-%m-%d')) AS CHAR) timespan FROM vacation_master a WHERE a.member_id = @v0  AND status IN (3,5) AND YEAR(date_start) = YEAR(NOW())", new object[] {  member_id } );
		if(_days.Rows.Count > 0)
			{
			body.AppendFormat(@"<div><u>{0} has taken these days off this year:</u><br/>", employee.Nickname);
			foreach(DataRow _day in _days.Rows)
				{
				var _timespan			= _day["timespan"].ToString();
				body.AppendFormat(@"{0}<br/>", _timespan);
				}
			body.Append(@"</div><br/>");
			}
		else
			{
			body.AppendFormat(@"<div>{0} hasn't taken any other days off this year.</div><br/>", employee.Nickname);
			}
		#endregion Other days off this year by this employee
		body.AppendFormat(@"<div>{1}'s Hire Date: {0}</div><br/></div>", Toolbox.MySQL_shortdt(Convert.ToDateTime(employee.StartDate)), employee.Nickname);
		m.Body		= body.ToString();
		try
			{
			m.file_passport();
			foreach(VacationRequest vr in vacations_toapprove)
				{
				Toolbox.doSQL_void(@"INSERT INTO vacation_log (dt, vacation_id, member_id, action) 
VALUES (NOW(), @v0,@v1, CONCAT('Vacation Request emailed to ',@v2))", new object[] { vr.vacation_id, employee.id, m.To });
				}
			return true;
			}
		catch (Exception ee)
			{
            Toolbox.do_catch_error(ee, 711);
			foreach(VacationRequest vr in vacations_toapprove)
				{
				Toolbox.doSQL_void(@"INSERT INTO vacation_log (dt, vacation_id, member_id, action) 
VALUES (NOW(), @v0, @v1, CONCAT('Vacation Request not emailed because: ',@v2))", new object[] { vr.vacation_id, employee.id, ee.ToString()});
				}
			return true;
			}
		}
	}