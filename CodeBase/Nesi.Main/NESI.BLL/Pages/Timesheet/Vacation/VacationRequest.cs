using System;
using System.Collections.Generic;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.Timesheet.Vacation
{
	public class VacationRequest : BLLBase
	{
		public int vacation_id { get; set; }

		public DateTime date_insert { get; set; }

		public DateTime date_start { get; set; }

		public DateTime date_end { get; set; }

		public DateTime date_return { get; set; }

		public int payperiod_id { get; set; }

		public double hours_requested { get; set; }

		public string note { get; set; }

		public int member_id { get; set; }

		public int status_id { get; set; }

		public int payment_method_id { get; set; }

		public double payment_amount { get; set; }

		public bool separate_check { get; set; }

		public int create_member_id { get; set; }
		public int business_unit_id { get; set; }

		public VacationRequest()
		{
		}
		public VacationRequest(object value)
		{
			vacation_id = Convert.ToInt32(value);
			get_record();
		}
		private void get_record()
		{
			var _record = bllToolbox.doSQL_dt(@"SELECT * FROM vacation_master WHERE vacation_id = @v0 ", vacation_id).Rows[0];
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
		}

		public int save_record()
		{
			var query = "";
			var report = check_record();
			if (report.Count > 0)
			{
				throw new Exception(string.Join("\r\n", report.ToArray()));
			}
			var is_new = (vacation_id == 0);
			if (!is_new && date_start.ToString("MM/dd/yyyy") != "01/01/0001")
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
	business_unit_id	= @v12
WHERE
	vacation_id		= @v10
LIMIT 1";
				var paramObjects = new object[] {
					date_start.ToString("yyyy-MM-dd HH:mm:ss"), 					   // {0}
					date_end.ToString("yyyy-MM-dd HH:mm:ss"), 						   // {1}
					NextBusinessDay(date_return, CurrentUser.BusinessUnit.Country == "CDN").ToString("yyyy-MM-dd HH:mm:ss"), 					   // {2}
					payperiod_id, 					   // {3}
					hours_requested, 				   // {4}
					member_id, 					   // {5}
					status_id, 					   // {6}
					payment_method_id, 			   // {7}
					payment_amount, 				   // {8}
					(separate_check ? "1" : "0"),	   // {9}
					vacation_id,					   // {10}
					create_member_id,				   // {11}
					business_unit_id						   // {12}
				};
				bllToolbox.doSQL_void(query, paramObjects);
				#endregion update scheduled vacation
			}
			else if (is_new && date_start.ToString("MM/dd/yyyy") != "01/01/0001")
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
	business_unit_id
	)
VALUES
	(
	NOW(),
	@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11
	)";
				var paramObjects = new object[] {
					date_start.ToString("yyyy-MM-dd HH:mm:ss"), 					   // {0}
					date_end.ToString("yyyy-MM-dd HH:mm:ss"), 						   // {1}
					date_return.ToString("yyyy-MM-dd HH:mm:ss"), 					   // {2}
					payperiod_id, 					   // {3}
					hours_requested, 				   // {4}
					member_id, 					   // {5}
					status_id, 					   // {6}
					payment_method_id, 			   // {7}
					payment_amount, 				   // {8}
					(separate_check ? "1" : "0"),	   // {9}
					create_member_id,				   // {10}
					business_unit_id						   // {11}
				};
				vacation_id = Convert.ToInt32(bllToolbox.doSQL_return_id(query, paramObjects));
				#endregion new scheduled vacation
			}
			else if (is_new && date_start.ToString("MM/dd/yyyy") == "01/01/0001")
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
	business_unit_id
	)
VALUES
	(
	NOW(),
	'0001-01-01 00:00:00',
	'0001-01-01 00:00:00',
	'0001-01-01 00:00:00',
	@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8
	)";
				var paramObjects = new object[]
				{

					payperiod_id, 					   // {0}
					hours_requested, 				   // {1}
					member_id, 					   // {2}
					status_id, 					   // {3}
					payment_method_id, 			   // {4}
					payment_amount, 				   // {5}
					(separate_check ? "1" : "0"),	   // {6}
					create_member_id,				   // {7}
					business_unit_id                           // {8}
				};
				vacation_id = Convert.ToInt32(bllToolbox.doSQL_return_id(query, paramObjects));
				#endregion new vacation withdrawal
			}
			else if (!is_new && date_start.ToString("MM/dd/yyyy") == "01/01/0001")
			{
				#region update vacation withdrawal

				query = @"
UPDATE 
	vacation_master 
SET 
	date_start			= null,
	date_end			= null,
	date_return			= null,
	payperiod_id		= @v0,
	hours_requested		= @v1,
	member_id			= @v2,
	status				= @v3,
	payment_method		= @v4,
	payment_amount		= @v5,
	separate_check		= @v6,
	create_member_id	= @v8,
	business_unit_id			= @v9
WHERE
	vacation_id		= @v7
LIMIT 1";
				var paramObjects = new object[] {
					payperiod_id, 					 // {0}
					hours_requested, 				 // {1}
					member_id, 					 // {2}
					status_id, 					 // {3}
					payment_method_id, 			 // {4}
					payment_amount, 				 // {5}
					(separate_check ? "1" : "0"),	 // {6}
					vacation_id,					 // {7}
					create_member_id,				 // {8}
					business_unit_id						 // {9}
				};
				bllToolbox.doSQL_void(query, paramObjects);
				#endregion update vacation withdrawal
			}
			if (note != string.Empty && vacation_id > 0)
			{
				bllToolbox.doSQL_void(@"INSERT INTO vacation_note 
(vacation_id, member_id, date_insert, note) VALUES (@v0,@v1, NOW(), @v2)", vacation_id, member_id, note);
			}

			return vacation_id;
		}

	private DateTime NextBusinessDay(DateTime _inDate, bool _isCanadian)
		{
		var returnedDate = _inDate;
		if (_inDate.DayOfWeek == DayOfWeek.Saturday) returnedDate = _inDate.Date.AddDays(2);
		if (_inDate.DayOfWeek == DayOfWeek.Sunday) returnedDate = _inDate.Date.AddDays(1);
		while (nesi.core.NeHolidays.DateIsHoliday(returnedDate, _isCanadian))
			{
			returnedDate = returnedDate.AddDays(1);
			}

		if (returnedDate.DayOfWeek == DayOfWeek.Saturday || returnedDate.DayOfWeek == DayOfWeek.Sunday)
			{
			return NextBusinessDay(returnedDate, _isCanadian);
			}

		return returnedDate;
		}
		private List<string> check_record()
		{
			var report = new List<string>();
			if (payperiod_id == 0) { report.Add("Pay Period not set"); }
			if (member_id == 0) { report.Add("Employee not set"); }
			return report;
		}

		public void email_requester(Employee _user, string avacation_id)
		{
			this.vacation_id = Convert.ToInt32(avacation_id);
			get_record();

			try
			{
				var mem = new Employee(Convert.ToInt32(member_id));
				if (mem.Status == "Active")
				{
					NeEMail email = new NeEMail
					{
						Subject = "Vacation request has been updated",
						To = !mem.EmployeeProfile.member_neemail.Contains("nomail") ? mem.EmployeeProfile.member_neemail : mem.Email,
						Body = "<div style='font-family:Arial'>",
						From = _user.EmployeeProfile.member_neemail,
						isHTML = true
					};
					if (avacation_id.Contains(","))
					{
						foreach (var _id in avacation_id.Split(','))
						{
							var vr = new VacationRequest(_id);
							email.Body += "Vacation Request Start: " + vr.date_start.ToString("yyyy-MM-dd") + " to " +
							              vr.date_end.ToString("yyyy-MM-dd") + " has been set to the status of -  " +
							              bllToolbox.doSQL_string("Select vacation_status.status  from vacation_status  where status_id = @v0", vr.status_id) + "</br>";
							if (vr.date_end < DateTime.Today)
							{
								return;
							}
						}
					}
					else
					{
						var vr = new VacationRequest(avacation_id);
						email.Body += "Start: " + vr.date_start.ToString("yyyy-MM-dd") + " to " +
						              vr.date_end.ToString("yyyy-MM-dd") + " has been set to the status of -  " +
						              bllToolbox.doSQL_string("Select vacation_status.status from vacation_status where status_id = @v0", vr.status_id) + "</br>";
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
				// ignored
			}
		}
	}
}