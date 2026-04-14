using System;
using System.Data;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeMemberDaysOff
	/// </summary>
	public class NeMemberDaysOff
		{
		public int id {get;set;}
		public bool active {get;set;}
		public int business_unit_id {get;set;}
		public int member_id {get;set;}
		public int leavetype_id {get;set;}
		public int requesttype_id {get;set;}
		public int requested_by {get;set;}
		public string comments {get;set;}
		public string status {get;set;}
		public string date_requested_start {get;set;}
		public string date_requested_end {get;set;}
		public int member_id_audit {get;set;}
		public string date_created {get;set;}
		public string date_requested {get;set;}
		public string date_modified {get;set;}
		public double days {get;set;}
		public double hours {get;set;}

/*
MemberDaysOff_ID
membersdayoff_member_id
MembersDaysOff_LeaveType_ID
MembersDayOff_RequestType_ID
StartDateRequested
EndDateRequested
RequestedBy
Comments
Status
Member_ID_Audit
Created_Date
Modified_Date
Active
DateRequested
Hours
Days
CompanyID
 */
		public NeMemberDaysOff(){}
		public NeMemberDaysOff(object _id)
			{
			Load(_id);
			}
		private void Load(object _id)
			{
			var vars		= Toolbox.doSQL_dt(@" SELECT vacation_id id, member_id, type_id, requesttype_id, date_start, date_end, comments, date_insert, ts, create_member_id, business_unit_id FROM vacation_master WHERE vacation_id = @v0  LIMIT 1", new object[] {  _id } );
			if(vars.Rows.Count > 0)
				{
				foreach(DataRow v in vars.Rows)
					{
					id						= Convert.ToInt32(v["id"]);
					business_unit_id				= (int) v["business_unit_id"];
					member_id				= Convert.ToInt32(v["member_id"]);
					leavetype_id			= Convert.ToInt32(v["type_id"]);
					requesttype_id			= Convert.ToInt32(v["requesttype_id"]);
					comments				= v["comments"].ToString();
					date_requested_start	= v["date_start"].ToString();
					date_created			= v["date_insert"].ToString();
					date_modified			= v["ts"].ToString();
					member_id_audit			= Convert.ToInt32(v["create_member_id"]);

					//date_requested		= v["date_requested"].ToString();
					//days					= Convert.ToDouble(v["days"]);
					//active					= v["active"].ToString() == "true";
					//date_requested_end		= v["date_end"].ToString();
					//requested_by			= Convert.ToInt32(v["requested_by"]);
					}
				}
			}
		public void delete()
			{
			if(id > 0)
				{
				Toolbox.doSQL_void(@"DELETE FROM vacation_master WHERE vacation_id = @v0 LIMIT 1", id);
				//var mt		= new NeMemberTime("DayOff", id);
				//if(mt.ID > 0)
				//	{
				//	mt.DeleteMemberTimeDet();
				//	}
				}
			}
		private void AddMembertime(DateTime date, int _id)
			{
			var timeentry = new NeMemberTime();
			timeentry.CreatedDate = Toolbox.MySQLNow_short();
			timeentry.Date = Toolbox.MySQL_shortdt(date);
			timeentry.MemberIDAudit = member_id_audit;
			timeentry.MemberIDCreate = member_id_audit;
			timeentry.MemberTime_Cust_No = "0";
			timeentry.membertime_memberid = member_id;
			timeentry.MemberTimeCustomerName = "Human Resources";
			timeentry.MemberTimePayTypeHoursID = 1;
			timeentry.MemberTimeWoComment = comments.Replace("'", "''");
			timeentry.MemberTimeWoCommentID = 0;
			timeentry.MemberTimeWorkOrderID = _id.ToString();
			timeentry.Memo = comments.Replace("'", "''");
			timeentry.Miles = 0;
			timeentry.NumberOfHours = 0;
			timeentry.MemberTime_SRED = "0";
			timeentry.MemberTime_Warranty = "0";
			timeentry.MemberTime_Premium = "0";
			timeentry.MemberTime_Mileage = "false";
			timeentry.ProductCode = "HR";
			timeentry.WOType = "DayOff";
			timeentry.business_unit_id = business_unit_id;
			timeentry.AddMemberTime(member_id);
			}
		private void UpdateMemberTime(DateTime date, int _id)
			{
			//var timeentry = new NeMemberTime("DayOff", _id);
			//timeentry.Date = Toolbox.MySQL_shortdt(date);
			//timeentry.MemberIDAudit = member_id_audit;
			//timeentry.MemberIDCreate = member_id_audit;
			//timeentry.MemberTime_Cust_No = "0";
			//timeentry.membertime_memberid = member_id;
			//timeentry.MemberTimeCustomerName = "Human Resources";
			//timeentry.MemberTimePayTypeHoursID = 1;
			//timeentry.MemberTimeWoComment = comments.Replace("'", "''");
			//timeentry.MemberTimeWoCommentID = 0;
			//timeentry.MemberTimeWorkOrderID = _id.ToString();
			//timeentry.Memo = comments.Replace("'", "''");
			//timeentry.Miles = 0;
			//timeentry.NumberOfHours = 0;
			//timeentry.MemberTime_SRED = "0";
			//timeentry.MemberTime_Warranty = "0";
			//timeentry.MemberTime_Premium = "0";
			//timeentry.MemberTime_Mileage = "false";
			//timeentry.ProductCode = "HR";
			//timeentry.WOType = "DayOff";
			//timeentry.business_unit_id = business_unit_id;
			//timeentry.UpdateMemberTime();
			}
		public void save()
			{
			if(id == 0)
				{
				var date_start			= new DateTime();
				var date_end			= new DateTime();
				DateTime.TryParse(date_requested_start, out date_start);
				DateTime.TryParse(date_requested_end, out date_end);
				var diff					= date_end.Subtract(date_start).Days;
				if(diff == 0)
					{
					if(date_start.Date == date_end.Date && date_start.Hour == date_end.Hour)
						{
						// They are taking the full day off if they didn't take the time to change the times...
						date_start			= new DateTime(date_start.Year, date_start.Month, date_start.Day, 7,30,0);
						date_end			= new DateTime(date_end.Year, date_end.Month, date_end.Day, 16,0,0);
						}
					id = Toolbox.doSQL_return_id(@"
INSERT INTO vacation_master 
	(
	member_id,
	type_id,
	requesttype_id,
	date_start,
	date_end,
	date_return,
	comments,
	date_insert,
	create_member_id,
	business_unit_id,
	payperiod_id
	) 
VALUES (@v0,@v1,@v2,@v3,@v4,@v4,@v5,NOW(),@v6,@v7,@v8)", 
	new object[] {					member_id, 					    // {0}
						leavetype_id, 				    // {1}
						requesttype_id, 			    // {2}
						Toolbox.MySQL_longdt(date_start),		    // {3}
						Toolbox.MySQL_longdt(date_end),				// {4}
						comments,   // {5}
						member_id_audit, 			    // {6}
						business_unit_id,					    // {7}
						NePayPeriod.get_payperiod_id(date_start) // {8}
					});
					//AddMembertime(date_start, id);
					}
				else if(diff > 0)
					{
					// Insert Multiple Vacation Entries Per One Day(s) off entry.
					var i			= 0;
					var now	= DateTime.Now;
					foreach(var day in Toolbox.EachDay(date_start, date_end))
						{
						if(day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
							{
							i++;
							continue;
							}
						var this_day_start			= new DateTime(day.Year, day.Month, day.Day, 7,30,0);
						var this_day_end			= new DateTime(day.Year, day.Month, day.Day, 16,0,0);
						if(i == 0)
							{
							// Because the user has already defined the start time
							this_day_start				= date_start;
							// They may be entering time before the day has started, or after the work day is over... default time to applicable start time
							if(DateTime.Now.Date == this_day_start.Date && (this_day_start.Hour > 16 || this_day_start.Hour < 7))
								{
								this_day_start			= new DateTime(now.Year, now.Month, now.Day, 7,30,0);
								}
							}
						else if(i == diff)
							{
							// Because the user has already defined the end time
							this_day_end				= date_end;
							// They may be entering time before the day has started, or after the work day is over... default time to applicable end time
							if(this_day_end.Hour > 16 || this_day_end.Hour < 7)
								{
								this_day_end			= new DateTime(this_day_end.Year, this_day_end.Month, this_day_end.Day, 16,0,0);
								}
							}
						id = Toolbox.doSQL_return_id(@"
INSERT INTO vacation_master 
	(
	member_id,
	type_id,
	requesttype_id,
	date_start,
	date_end,
	date_return,
	comments,
	date_insert,
	create_member_id,
	business_unit_id,
	payperiod_id
	) 
VALUES 
	(
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v4,
	@v5,
	NOW(),
	@v6,
	@v7,
	@v8
	)", new object[] {

							member_id, 										// {0}
							leavetype_id, 									// {1}
							requesttype_id, 								// {2}
							Toolbox.MySQL_longdt(this_day_start),		    // {3}
							Toolbox.MySQL_longdt(this_day_end),				// {4}
							comments,					// {5}
							member_id_audit, 								// {6}
							business_unit_id,										// {7}
							NePayPeriod.get_payperiod_id(this_day_start)	// {8}
						});
						//AddMembertime(day, id);
						i++;
						}
					}
				}
			else
				{
				// UPDATE
				var date_start			= new DateTime();
				var date_end			= new DateTime();
				DateTime.TryParse(date_requested_start, out date_start);
				DateTime.TryParse(date_requested_end, out date_end);

				if(date_start.Hour < 7 || date_start.Hour > 16)
					{
					date_start			= new DateTime(date_start.Year, date_start.Month, date_start.Day, 7,30,0);
					}
				if(date_end.Hour > 16 || date_end.Hour < 7)
					{
					date_end			= new DateTime(date_end.Year, date_end.Month, date_end.Day, 16,0,0);
					}
				Toolbox.doSQL_void(@"
UPDATE 
	vacation_master 
SET 
	date_start = @v0,
	date_end = @v1,
	comments = @v2,
	payperiod_id = @v3,
	type_id = @v4,
	requesttype_id = @v5
WHERE 
	vacation_id = @v6
LIMIT 1", new object[] {
					Toolbox.MySQL_longdt(date_start),	// {0}
					Toolbox.MySQL_longdt(date_end),		// {1}
					comments,		// {2}
					NePayPeriod.get_payperiod_id(Convert.ToDateTime(date_requested_start)),	// {3}
					leavetype_id,						// {4}
					requesttype_id,						// {5}
					id									// {6}
				});
				//UpdateMemberTime(date_start, id);
				}
			}

		}
	}