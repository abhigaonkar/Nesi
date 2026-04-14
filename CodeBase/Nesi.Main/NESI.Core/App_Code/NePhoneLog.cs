using System;
using System.Data;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NePhoneLog
	/// </summary>
	public class NePhoneLog
	{
		public int phone_log_id { get; set; }
		public int phone_log_alitgen_session_id { get; set; }
		public DateTime phone_log_date { get; set; }
		public string phone_log_from_number { get; set; }
		public string phone_log_to_number { get; set; }
		public string phone_log_from_name { get; set; }
		public string phone_log_to_name { get; set; }
		public int phone_log_duration { get; set; }
		public int phone_log_time { get; set; }
		public int phone_log_direction { get; set; }
		public string external_name { get; set; }
		public string internal_name { get; set; }
		public string strdirection { get; set; }
		public string imei_ds { get; set; }
		public int calltime_nb { get; set; }
		public string notes { get; set; }
		public int numbertype_fg { get; set; }
		public int calltype_fg { get; set; }
		public bool updated_to_customer_history { get; set; }

		public NePhoneLog()
		{
			calltype_fg = 0;
			numbertype_fg = 0;
		}

		public void save()
		{
			if ((phone_log_alitgen_session_id != null) && (phone_log_alitgen_session_id != 0))  // if its an altigen call
			{
				if (Toolbox.doSQL_int(@"Select count(phone_log_id) from phone_log where phone_log_altigen_session_id = @v0 limit 1", phone_log_alitgen_session_id) == 0)
				{
					Toolbox.doSQL_void(@"Insert into phone_log (phone_log_altigen_session_id,phone_log_date,phone_log_from_number,phone_log_to_number,phone_log_from_name,
phone_log_to_name,phone_log_duration,phone_log_time,phone_log_direction,numbertype_fg,calltype_fg)
									Values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10)",
									new object[] {
										phone_log_alitgen_session_id, //0
									phone_log_date.ToString("yyyy-MM-dd"), //1
									phone_log_from_number,//2
										phone_log_to_number,//3
										phone_log_from_name, //4
										phone_log_to_name, //5
										phone_log_duration,//6
										phone_log_time,//7
										phone_log_direction, //8
										numbertype_fg, //9
										calltype_fg //10
									});

					var dt_from = new NePhoneNumbers().get_contact_from_phonenumber(phone_log_from_number);
					if (dt_from.Rows.Count > 0)
					{
						var c = new NEContact(Convert.ToInt32(dt_from.Rows[0][0]));
					}

					var dt_to = new NePhoneNumbers().get_contact_from_phonenumber(phone_log_to_number);
					if (dt_to.Rows.Count > 0)
					{
						var c = new NEContact(Convert.ToInt32(dt_to.Rows[0][0]));
					}

				}

			}
			else
			{
				if (Toolbox.doSQL_int(@"Select count(phone_log_id) from phone_log where phone_log_id = @v0 limit 1", phone_log_id) == 0)
				{
					Toolbox.doSQL_void(@"Insert into phone_log 
(phone_log_altigen_session_id,phone_log_date,phone_log_from_number,phone_log_to_number,
phone_log_from_name,phone_log_to_name,phone_log_duration,phone_log_time,
phone_log_direction,imei_ds,calltime_nb,notes,numbertype_fg,calltype_fg)
Values(@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13)",
new object[] {
phone_log_alitgen_session_id, //0
phone_log_date.ToString("yyyy-MM-dd"), //1
phone_log_from_number, //2
phone_log_to_number, //3
phone_log_from_name, //4
phone_log_to_name, //5
phone_log_duration, //6
phone_log_time,//7
phone_log_direction, //8
imei_ds,//9
calltime_nb, //10
notes, //11
numbertype_fg, //12
calltype_fg //13
});

					var dt_from = new NePhoneNumbers().get_contact_from_phonenumber(phone_log_from_number);
					if (dt_from.Rows.Count > 0)
					{
						var c = new NEContact(Convert.ToInt32(dt_from.Rows[0][0]));
					}

					var dt_to = new NePhoneNumbers().get_contact_from_phonenumber(phone_log_to_number);
					if (dt_to.Rows.Count > 0)
					{
						var c = new NEContact(Convert.ToInt32(dt_to.Rows[0][0]));
					}

				}
				else
				{
					Toolbox.doSQL_void(@"Update phone_log set notes = @v0 where phone_log_id = @v1", new object[] { notes, phone_log_id });
				}

			}

		}

		public void update_after_customer_history()
		{
			if (Toolbox.doSQL_int(@"Select count(phone_log_id) from phone_log where phone_log_id = @v0 limit 1", phone_log_id) != 0)
			{
				Toolbox.doSQL_void(@"Update phone_log 
set phone_log_from_name=@v0,
phone_log_to_name=@v1,
updated_to_customer_history=1 
where phone_log_id=@v2  
limit 1",
new object[]
	{
		phone_log_from_name,phone_log_to_name,phone_log_id
	}
);
			}
		}

		public NePhoneLog get_NePhonglog(object imei_ds, object calltime_nb)
		{

			var x = Toolbox.doSQL_int(@"Select ifnull((Select phone_log_id from phone_log 
where imei_ds =@v0 
and calltime_nb=@v1  
limit 1),0)",
new object[]
{
	imei_ds,calltime_nb
}
);
			if (x != 0)
			{
				return new NePhoneLog(x);
			}
			return new NePhoneLog(0);
		}


		public NePhoneLog(int id)
		{
			calltype_fg = 0;
			numbertype_fg = 0;
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM phone_log WHERE phone_log_ID =@v0 ", new object[] {  id } );
			phone_log_id = 0;
			if (dt.Rows.Count == 1)
			{
				var dr = dt.Rows[0];
				phone_log_id = id;
				phone_log_alitgen_session_id = dr["phone_log_altigen_session_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["phone_log_altigen_session_id"]);
				phone_log_date = Convert.ToDateTime(dr["phone_log_date"]);
				phone_log_from_number = Convert.ToString(dr["phone_log_from_number"]);
				phone_log_to_number = Convert.ToString(dr["phone_log_to_number"]);
				phone_log_from_name = Convert.ToString(dr["phone_log_from_name"]);
				phone_log_to_name = Convert.ToString(dr["phone_log_to_name"]);
				phone_log_duration = Convert.ToInt32(dr["phone_log_duration"]);
				phone_log_time = Convert.ToInt32(dr["phone_log_time"]);
				phone_log_direction = Convert.ToChar(dr["phone_log_direction"]);
				numbertype_fg = Convert.ToInt16(dr["numbertype_fg"]);
				calltype_fg = Convert.ToInt16(dr["calltype_fg"]);
				imei_ds = dr["imei_ds"] == DBNull.Value ? "" : dr["imei_ds"].ToString();
				calltime_nb = dr["calltime_nb"] == DBNull.Value ? 0 : Convert.ToInt32(dr["calltime_nb"]);
				notes = dr["notes"] == DBNull.Value ? "" : dr["notes"].ToString();
				updated_to_customer_history = Convert.ToBoolean(dr["updated_to_customer_history"]);
				var p = "";

				if (phone_log_direction == 1)
				{
					strdirection = "In";
					if (phone_log_from_number.Length == 10)
					{
						p = phone_log_from_number.Substring(0, 3) + " " + phone_log_from_number.Substring(3, 3) + " " + phone_log_from_number.Substring(6, 4);
					}
					else if (phone_log_from_number.Length == 11)
					{
						p = phone_log_from_number.Substring(1, 3) + " " + phone_log_from_number.Substring(4, 3) + " " + phone_log_from_number.Substring(7, 4);
					}
					external_name = new NePhoneNumbers().GetWhoIsfromPhoneNumber(p);
					internal_name = phone_log_to_name;
				}
				else
				{
					strdirection = "Out";
					if (phone_log_to_number.Length == 10)
					{
						p = phone_log_to_number.Substring(0, 3) + " " + phone_log_to_number.Substring(3, 3) + " " + phone_log_to_number.Substring(6, 4);
					}
					else if (phone_log_to_number.Length == 11)
					{
						p = phone_log_to_number.Substring(1, 3) + " " + phone_log_to_number.Substring(4, 3) + " " + phone_log_to_number.Substring(7, 4);
					}
					external_name = new NePhoneNumbers().GetWhoIsfromPhoneNumber(p);
					internal_name = phone_log_from_name;
				}

			}

		}


	}
}