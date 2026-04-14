using System;
using System.Collections.Generic;
using NESI.Common.Models;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeVacation
	/// </summary>
	public class VacationInterface
		{
		public static bool is_on_vacation(int mid, DateTime dt)
			{
			return Toolbox.doSQL_int(@"Select count(vacation_id) from vacation_master a  where a.member_id =@v0 and a.date_start <=@v1  and a.date_end >=@v2 and a.status = 3", new object[] { mid, dt.ToString("yyyy-MM-dd HH:mm:ss"), dt.ToString("yyyy-MM-dd HH:mm:ss") }) > 0;
			}
		}
	public class VacationRequest
		{
		public int vacation_id { get; set; }
		public DateTime date_insert { get; set; }
		public DateTime date_start { get; set; }
		public DateTime date_end { get; set; }
		public DateTime date_return { get; set; }
		public int payperiod_id { get; set; }
		/// <summary>
		/// The type of vacation, default is 4 or "Scheduled Vacation"
		/// </summary>
		public int VacationType { get; set; } = OpsPayroll.Vacation.Type.ScheduledVacation;
		public double hours_requested { get; set; }
		public string note { get; set; }
		public int member_id { get; set; }
		public int status_id { get; set; }
		public int payment_method_id { get; set; }
		public string Comments { get; set; }
		public double payment_amount { get; set; }
		public bool separate_check { get; set; } = false;
		public int create_member_id { get; set; }
		public int business_unit_id { get; set; }
		public VacationRequest() : base()
			{
			}
		public VacationRequest(object value)
			{
			vacation_id = Convert.ToInt32(value);
			get_record();
			}
		private void get_record()
			{
			var _record = Toolbox.doSQL_dt(@"SELECT * FROM vacation_master WHERE vacation_id = @v0 ", new object[] { vacation_id }).Rows[0];
			date_insert = (DateTime)_record["date_insert"];
			date_start = (DateTime)_record["date_start"];
			date_end = (DateTime)_record["date_end"];
			date_return = (DateTime)_record["date_return"];
			payperiod_id = (int)_record["payperiod_id"];
			hours_requested = Convert.ToDouble(_record["hours_requested"]);
			member_id = Convert.ToInt32(_record["member_id"]);
			status_id = (int)_record["status"];
			payment_method_id = (int)_record["payment_method"];
			payment_amount = (double)_record["payment_amount"];
			separate_check = Convert.ToBoolean(_record["separate_check"]);
			business_unit_id = Convert.ToInt32(_record["business_unit_id"]);
			create_member_id = Convert.ToInt32(_record["create_member_id"]);
			VacationType = Convert.ToInt32(_record["type_id"]);
			}
		public int save_record()
			{
			var isNew = vacation_id == 0;
			string query;
			var payPeriodObj = new NePayPeriod(payperiod_id);
			var isWithdrawal = date_start.Year < 2000; // Vacation withdrawals will have a year of 1
			if (!isNew && !isWithdrawal)
				{
				#region update scheduled vacation
				query = @"
UPDATE 
	vacation_master 
SET 
	date_start			= @v0,
	date_end			= @v1,
	date_return			= @v2,
	payperiod_id		= @v3,
	hours_requested		= @v4,
	member_id			= @v5,
	status				= @v6,
	payment_method		= @v7,
	payment_amount		= @v8,
	separate_check		= @v9,
	create_member_id	= @v11,
	business_unit_id	= @v12,
	comments			= @v13,
	type_id				= @v14
WHERE
	vacation_id	= @v10
LIMIT 1";
				var paramObjects = new object[] 
										{
										date_start, 			// 0
										date_end, 				// 1
										date_return, 			// 2
										payperiod_id, 			// 3
										hours_requested, 		// 4
										member_id, 				// 5
										status_id, 				// 6
										payment_method_id, 		// 7
										payment_amount, 		// 8
										separate_check,			// 9
										vacation_id,			// 10
										create_member_id,		// 11
										business_unit_id,		// 12
										Comments,				// 13
										VacationType			// 14
										};
				Toolbox.doSQL_void(query, paramObjects);
				#endregion update scheduled vacation
				}
			if (isNew && !isWithdrawal)
				{
				#region new scheduled vacation
				query = @"
INSERT INTO vacation_master
	(
	date_insert,
	date_start,
	date_end,
	date_return,
	payperiod_id,
	hours_requested,
	member_id,
	status,
	payment_method,
	payment_amount,
	separate_check,
	create_member_id,
	business_unit_id,
	comments,
	type_id
	)
VALUES
	(
	NOW(),
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v5,
	@v6,
	@v7,
	@v8,
	@v9,
	@v10,
	@v11,
	@v12,
	@v13
	)";
				var paramObjects = new object[] 
										{
										date_start, 			// 0
										date_end, 				// 1
										date_return, 			// 2
										payperiod_id, 			// 3
										hours_requested, 		// 4
										member_id, 				// 5
										status_id, 				// 6
										payment_method_id, 		// 7
										payment_amount, 		// 8
										separate_check,			// 9
										create_member_id,		// 10
										business_unit_id,		// 11
										Comments,				// 12
										VacationType			// 13
										};
				vacation_id = Toolbox.doSQL_return_id(query, paramObjects);
				#endregion new scheduled vacation
				}
			if (isNew && isWithdrawal)
				{
				#region new vacation withdrawal
				query = @"
INSERT INTO vacation_master
	(
	date_insert,
	date_start,
	date_end,
	date_return,
	payperiod_id,
	hours_requested,
	member_id,
	status,
	payment_method,
	payment_amount,
	separate_check,
	create_member_id,
	business_unit_id,
	comments,
	type_id
	)
VALUES
	(
	@v10,
	'0001-01-01',
	'0001-01-01',
	'0001-01-01',
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v5,
	@v6,
	@v7,
	@v8,
	@v9,
	@v11
	)";
				var paramObjects = new object[]
					{
					payperiod_id, 	    // 0
					hours_requested,    // 1
					member_id, 		    // 2
					status_id, 		    // 3
					payment_method_id,  // 4
					payment_amount,     // 5
					separate_check,	    // 6
					create_member_id,   // 7
					business_unit_id,   // 8
					Comments,		    // 9
					// MH 2020-11-04 - Commenting this out until it's completed.
					//Comments == payroll.vacation.PayoutComments 
					//	? payPeriodObj.end_date 
					//	: 
						DateTime.Now,
					VacationType		// 11
					};
				vacation_id = Toolbox.doSQL_return_id(query, paramObjects);
				#endregion new vacation withdrawal
				}
			if (!isNew && isWithdrawal)
				{
				#region update vacation withdrawal
				query = @"
UPDATE 
	vacation_master 
SET 
	date_start			= '0001-01-01',
	date_end			= '0001-01-01',
	date_return			= '0001-01-01',
	payperiod_id		= @v0,
	hours_requested		= @v1,
	member_id			= @v2,
	status				= @v3,
	payment_method		= @v4,
	payment_amount		= @v5,
	separate_check		= @v6,
	create_member_id	= @v8,
	business_unit_id	= @v9,
	comments			= @v10,
	type_id				= @v11
WHERE
	vacation_id		= @v7
LIMIT 1";
				var paramObjects = new object[] 
										{
										payperiod_id, 			 // 0
										hours_requested, 		 // 1
										member_id, 				 // 2
										status_id, 				 // 3
										payment_method_id, 		 // 4
										payment_amount, 		 // 5
										separate_check,	         // 6
										vacation_id,			 // 7
										create_member_id,		 // 8
										business_unit_id,		 // 9
										Comments,				 // 10
										VacationType			 // 11
										};
				Toolbox.doSQL_void(query, paramObjects);
				#endregion update vacation withdrawal
				}
			if (!string.IsNullOrEmpty(note) && vacation_id > 0)
				{
				Toolbox.doSQL_void(@"INSERT INTO vacation_note (vacation_id, member_id, date_insert, note) VALUES (@v0,@v1, NOW(), @v2)",
					new object[] {
						vacation_id,
						member_id,
						note
						});
				}
			return vacation_id;
			}
		public void email_requester(NeMember _user, string vacation_id)
			{
			this.vacation_id = Convert.ToInt32(vacation_id);
			get_record();
			try
				{
				var mem = new NeMember(Convert.ToInt32(member_id));
				if (mem.Status == "Active")
					{
					var email = new NeEMail
						{
						Subject = "Vacation request has been updated",
						To = !mem.NEEmail.Contains("nomail")
										? mem.NEEmail
										: mem.Email,
						Body = "<div style='font-family:Arial'>",
						From = _user.NEEmail,
						isHTML = true
						};
					if (vacation_id.Contains(","))
						{
						foreach (var _id in vacation_id.Split(','))
							{
							var vr = new VacationRequest(_id);
							email.Body += "Vacation Request Start: " + vr.date_start.ToString("yyyy-MM-dd") + " to " +
										  vr.date_end.ToString("yyyy-MM-dd") + " has been set to the status of -  " +
										  Toolbox.doSQL_string("Select vacation_status.status  from vacation_status  where status_id = @v0",
											  new object[] { vr.status_id }) + "</br>";
							if (vr.date_end < DateTime.Today)
								{
								return;
								}
							}
						}
					else
						{
						var vr = new VacationRequest(vacation_id);
						email.Body += "Start: " + vr.date_start.ToString("yyyy-MM-dd") + " to " +
									  vr.date_end.ToString("yyyy-MM-dd") + " has been set to the status of -  " +
									  Toolbox.doSQL_string("Select vacation_status.status from vacation_status where status_id = @v0",
										  new object[] { vr.status_id }) + "</br>";
						if (vr.date_end < DateTime.Today)
							{
							return;
							}
						}
					if (email.Body.Length > 0)
						{
						email.Body += "</div>";
						email.Send();
						}
					}
				}
			catch
				{
				}
			}
		}
	}