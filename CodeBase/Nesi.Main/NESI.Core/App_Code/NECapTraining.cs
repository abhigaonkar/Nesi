using System;
using System.Data;

namespace nesi.core
	{
	/// <summary>

	/// </summary>
	public class NeCapTraining
		{
		public int id { get; set; }
		public string name {get; set;}
		public string url { get; set;}
		public double cost { get; set; }
		public DateTime last_modified { get; set; }
		public string phone_number { get; set; }
		public string contact_info { get; set; }
		public int quality {get; set;}
		public bool is_internal { get; set; }
		public string notes { get; set; }

		public NeCapTraining(){}
		public NeCapTraining(int _id)
			{
			if(exists(_id))
				{
				load(_id);
				}
			}
		private bool exists(int _id)
			{
			return Toolbox.doSQL_int("SELECT COUNT(id) FROM training_header WHERE id = @v0", _id) > 0;
			}
		private void load(int _id)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM training_header  WHERE id = @v0", new object[] { _id });
			foreach (DataRow dr in dt.Rows)
				{
				id = (int)dr["id"];
				name = (string)dr["name"];
				url = dr["url"] == DBNull.Value ? "" : (string)dr["url"];
				cost = dr["cost"] == DBNull.Value ? 0 : (double)dr["cost"];
				last_modified = dr["last_modified"] == DBNull.Value ? new DateTime() : (DateTime)dr["last_modified"];
				phone_number = dr["phone_number"] == DBNull.Value ? "" : dr["phone_number"].ToString();
				contact_info = dr["contact_info"] == DBNull.Value ? "" : dr["contact_info"].ToString();
				quality = dr["quality"] == DBNull.Value ? 0 : (int)dr["quality"];
				is_internal = dr["is_internal"] == DBNull.Value ? false : Convert.ToBoolean(dr["is_internal"]);
				notes = dr["notes"] == DBNull.Value ? "" : dr["notes"].ToString();

				}
			}


		public static DataTable get_missing_certs_for_member(int member_id)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT distinct cert1.id cert_id, cert1.certificate_name AS cert_req, cert1.expires, cert1.how_to_acquire acquire, cert1.notes, cert1_history.date, ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0) cert_exp FROM member LEFT JOIN member_offers _mo on member.member_id = _mo.id and _mo.status = 'Accepted' LEFT JOIN memberoffer_cr ON _mo.id = memberoffer_cr.memberoffer_moid inner JOIN core_responsibilities ON memberoffer_cr.memberoffer_crid = core_responsibilities.id and core_responsibilities.status = 'Active' LEFT JOIN cr_certificates ON core_responsibilities.id = cr_certificates.cr_id Inner JOIN certificates AS cert1 ON cr_certificates.certificates_id = cert1.id and cert1.`status` = 'Active' LEFT JOIN certificate_history AS cert1_history ON cert1.id = cert1_history.certificate_id and cert1_history.member_id = member.member_id and cert1_history.id = (Select certificate_history.id from certificate_history  where certificate_history.certificate_id = cert1.id and certificate_history.member_id = member.member_id order by certificate_history.date desc limit 1) WHERE member.member_id = @v0  and ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)<=90 ORDER BY cert_exp DESC", new object[] { member_id });
			return dt;

			}

		public static DataTable get_required_certs_for_member(int member_id)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT distinct cert1.id cert_id, cert1.certificate_name AS cert_req, cert1.expires, cert1.how_to_acquire acquire, cert1.notes FROM member LEFT JOIN member_offers _mo on member.member_id = _mo.id and _mo.status = 'Accepted' LEFT JOIN memberoffer_cr ON _mo.id = memberoffer_cr.memberoffer_moid inner JOIN core_responsibilities ON memberoffer_cr.memberoffer_crid = core_responsibilities.id and core_responsibilities.status = 'Active' LEFT JOIN cr_certificates ON core_responsibilities.id = cr_certificates.cr_id Inner JOIN certificates AS cert1 ON cr_certificates.certificates_id = cert1.id and cert1.`status` = 'Active'  WHERE member.member_id = @v0  ORDER BY cert_req ASC", new object[] { member_id });
			return dt;
			}

		public static DataTable get_certifications_for_training_id(int training_header_id)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT distinct certificates.id, certificates.certificate_name, certificates.notes FROM certificate_training_link INNER JOIN certificates ON certificate_training_link.certificate_id = certificates.id  WHERE certificates.`status` = 'Active' and certificate_training_link.training_header_id = @v0", new object[] { training_header_id });
			return dt;

			}

		public static void issue_certs(int training_header_history_id)
			{
			var th = new NeCapTraining_History(training_header_history_id);
			if (th.certs_issued == false)
				{
				var dt = get_certifications_for_training_id(th.training_header_id);
				foreach (DataRow dr in dt.Rows)
					{
					Toolbox.doSQL_void(@"Insert into certificate_history 
(certificate_id, member_id,business_unit_id, date, external_id, notes, score) 
values (@v0,@v1,@v2,@v3,@v4,@v5,@v6)",
new object[] {
dr["id"], //0
th.member_id, //1
th.business_unit_id, //2
th.date.ToString("yyyy-MM-dd"), //3
th.external_id, //4
th.notes, //5
100 //6
});
					}
				Toolbox.doSQL_void(@"Update training_header_history set certs_issued = 1 where id =@v0 ", training_header_history_id);
				}
			}
	

		}
	public class NeCapTraining_History
		{
		public int id				 { get; set; }
		public int training_header_id		 { get; set; }
		public int member_id		 { get; set; }
		public int business_unit_id	 { get; set; }
		public int membertype_id		 { get; set; }
		public DateTime date { get; set; }
		public double score		 { get; set; }
		public string notes		 { get; set; }
		public string external_id { get; set; }
		public int cap_training_schedule_id { get; set; }
		public TimeSpan start_time { get; set; }
		public TimeSpan end_time { get; set; }
		public bool certs_issued { get; set; }


		public NeCapTraining_History(){}

		public NeCapTraining_History(int _id)
			{
			if(exists(_id))
				{
				load(_id);
				}
			}
		private bool exists(int _id)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM training_header_history WHERE id = @v0", _id) > 0;
			}
		private void load(int _id)
			{
			var dt		= Toolbox.doSQL_dt(@"SELECT * FROM training_header_history  WHERE id = @v0", new object[] { _id });
			foreach(DataRow dr in dt.Rows)
				{
				id				= (int) dr["id"];
				training_header_id = (int)dr["training_header_id"];
				member_id		= (int) dr["member_id"];
				business_unit_id	= (int)dr["business_unit_id"];
				membertype_id = dr["membertype_id"] == DBNull.Value ? 0 : (int)dr["membertype_id"];
				date	= dr["date"] == DBNull.Value ? DateTime.Now : (DateTime) dr["date"];
				score		= dr["score"]==DBNull.Value? 0: (double) dr["score"];
				notes = dr["notes"] == DBNull.Value ? "" : (string)dr["notes"];
				external_id = dr["external_id"] == DBNull.Value ? "" : (string)dr["external_id"];
				cap_training_schedule_id = dr["cap_training_schedule_id"] == DBNull.Value ? 0 : (int)dr["cap_training_schedule_id"];
				start_time = dr["start_time"] == DBNull.Value ? new TimeSpan() : (TimeSpan)dr["start_time"];
				end_time = dr["end_time"] == DBNull.Value ? new TimeSpan() : (TimeSpan)dr["end_time"];
				certs_issued = dr["certs_issued"] == DBNull.Value ? false : Convert.ToBoolean(dr["certs_issued"]);
				}
			}
		public void save()
			{
//		id = Toolbox.doSQL_int(@"Select ifnull((Select id from training_header_history  where cap_training_schedule_id = @v0 and member_id = @v1 limit 1),0)", new object[] { cap_training_schedule_id,member_id });

			if(id == 0)
				{
				id		= Toolbox.doSQL_return_id(@"
INSERT INTO training_header_history
	(
	training_header_id,
	member_id,	
	business_unit_id,
	membertype_id,
	date,
	score,	
	notes,
	external_id,
cap_training_schedule_id,
start_time,
end_time
	)
VALUES
	(@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10
	)
", new object[] {
					training_header_id,								// {0}
					member_id, 								// {1}
					business_unit_id,		// {2}
					membertype_id, 			// {3}
					date.ToString("yyyy-MM-dd"),	// {4}
					score,			// {5}
					notes,								// {6}
					external_id,
					cap_training_schedule_id,
					start_time,
					end_time
				});
				}
			else
				{
				Toolbox.doSQL_void(@"
UPDATE 
	training_header_history
SET
	training_header_id			= @v1,
	member_id			= @v2,	
	business_unit_id		= @v3,
	membertype_id				= @v4,
	date		= @v5,
	score			= @v6,	
	notes			= @v7,
	external_id = @v8,
cap_training_schedule_id = @v9,
start_time = @v10,
end_time = @v11
WHERE
	id = @v0
LIMIT 1
", new object[] {
					id,										// {0}
					training_header_id,								// {1}
					member_id, 								// {2}
					business_unit_id,		// {3}
					membertype_id,			// {4}
					date.ToString("yyyy-MM-dd"),	// {5}
					score,			// {6}
					notes,							// {7}
					external_id,
					cap_training_schedule_id,
					start_time,
					end_time
				});
				}
			}
		public bool delete(int _id)
			{
			if(exists(_id))
				{
				Toolbox.doSQL_void(@"DELETE FROM training_header_history WHERE id = @v0 LIMIT 1", _id);
				return true;
				}
			else
				{
				return false;
				}
			}
		}
	public class NeCapTraining_Schedule
		{
		public int id { get; set; }
		public int training_header_id { get; set; }
		public DateTime date { get; set; }
		public TimeSpan start_time {get;set;}
		public TimeSpan end_time{get;set;}
		public string location{get;set;}
		public int max_fill {get;set;}
		public DateTime date_added { get; set; }
		

		public NeCapTraining_Schedule() { }

		public NeCapTraining_Schedule(int _id)
			{
			if (exists(_id))
				{
				load(_id);
				}
			}
		private bool exists(int _id)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM cap_training_schedule WHERE id = @v0", _id) > 0;
			}
		private void load(int _id)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM cap_training_schedule  WHERE id = @v0", new object[] { _id });
			foreach (DataRow dr in dt.Rows)
				{
				id = (int)dr["id"];
				training_header_id = (int)dr["training_header_id"];
				date = (DateTime)dr["date"];
				start_time = (TimeSpan)dr["start_time"];
				end_time = (TimeSpan)dr["end_time"];
				location = dr["location"] == DBNull.Value ? "" : (string)dr["location"];
				max_fill = dr["max_fill"] == DBNull.Value ? 0 : (int)dr["max_fill"];
				date_added = dr["date_added"] == DBNull.Value ? new DateTime() : (DateTime)dr["date_added"];
				}
			}

		public static void send_calendar_request(int training_history_id)
			{
			var cth = new NeCapTraining_History(Convert.ToInt32(training_history_id));
			var cts = new NeCapTraining_Schedule(Convert.ToInt32(cth.cap_training_schedule_id));
			var members_email = new NeMember(cth.member_id).NEEmail.Contains("nomail") ? new NeMember(cth.member_id).Email : new NeMember(cth.member_id).NEEmail;

			var email = new NeEMail();

	
			var fileServer				= NeTaxEntity.BaseFolder(cth.business_unit_id, false);
			try
				{
				email = new NeEMail();
				email.To = members_email;
				email.Subject = "CAP Training: " + new NeCapTraining(cth.training_header_id).name;
				email.Body = "Training " + new NeCapTraining(cth.training_header_id).name + "<br>";
				email.Body += "Date: " + cts.date.ToString("yyyy-MM-dd") + "<br>";
				email.Body += "Start Time: " + cts.start_time.Hours + ":" + cts.start_time.Minutes.ToString().PadRight(2, '0') + "<br>";
				email.Body += "End Time: " + cts.end_time.Hours + ":" + cts.end_time.Minutes.ToString().PadRight(2, '0') + "<br>";
				email.Body += "Location: " + cts.location;
				email.From = "admin@" + Toolbox.app_setting("DomainForEmail");
				email.isHTML = true;

				var s = new DateTime(Convert.ToInt16(cts.date.ToString("yyyy")), Convert.ToInt16(cts.date.ToString("MM")), Convert.ToInt16(cts.date.ToString("dd")), cts.start_time.Hours, cts.start_time.Minutes, 0);
				var en = new DateTime(Convert.ToInt16(cts.date.ToString("yyyy")), Convert.ToInt16(cts.date.ToString("MM")), Convert.ToInt16(cts.date.ToString("dd")), cts.end_time.Hours, cts.end_time.Minutes, 0);

				string[] contents = { "BEGIN:VCALENDAR",
										"PRODID:-//nesi//nesi//EN",
										"BEGIN:VEVENT",
										"UID:" + cth.member_id.ToString() + cth.id.ToString() + cts.id.ToString(),
										"DTSTART:" + s.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
										"DTEND:" + en.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
										"LOCATION:" + cts.location, 
										"DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + email.Subject,
										"SUMMARY:" + email.Subject, "PRIORITY:3", 
										"END:VEVENT", "END:VCALENDAR" };
				var temp_str = DateTime.Now.ToUniversalTime().ToString().Replace("/", "").Replace(":", "");

				System.IO.File.WriteAllLines((fileServer + @"\training_files\training" + temp_str + ".ics"), contents);
				var mailAttachment = new System.Net.Mail.Attachment((fileServer + @"\training_files\training" + temp_str + ".ics"));
				email.Attachment = mailAttachment;
				System.Threading.Thread.Sleep(1500);
				email.Send();
				mailAttachment.Dispose();
				System.IO.File.Delete((fileServer + @"\training_files\training" + temp_str + ".ics"));
				}
			catch { }

			

			}

		public static void send_cancel_calendar(int training_history_id)
			{
			var cth = new NeCapTraining_History(Convert.ToInt32(training_history_id));
			var cts = new NeCapTraining_Schedule(Convert.ToInt32(cth.cap_training_schedule_id));
			var members_email = new NeMember(cth.member_id).NEEmail.Contains("nomail") ? new NeMember(cth.member_id).Email : new NeMember(cth.member_id).NEEmail;

			var email = new NeEMail();
			var fileServer				= NeTaxEntity.BaseFolder(cth.business_unit_id, false);

			email.To = members_email;
			email.Subject = "CAP Training: " + new NeCapTraining(cth.training_header_id).name + " Cancelled";
			email.Body = "Training " + new NeCapTraining(cth.training_header_id).name + "<br>";
			email.Body += "Date: " + cts.date.ToString("yyyy-MM-dd") + "<br>";
			email.Body += "Start Time: " + cts.start_time.Hours + ":" + cts.start_time.Minutes.ToString().PadRight(2, '0') + "<br>";
			email.Body += "End Time: " + cts.end_time.Hours + ":" + cts.end_time.Minutes.ToString().PadRight(2, '0') + "<br>";
			email.Body += "Location: " + cts.location;
			email.From = "admin@" + Toolbox.app_setting("DomainForEmail");
			email.isHTML = true;

			var s = new DateTime(Convert.ToInt16(cts.date.ToString("yyyy")), Convert.ToInt16(cts.date.ToString("MM")), Convert.ToInt16(cts.date.ToString("dd")), cts.start_time.Hours, cts.start_time.Minutes, 0);
			var en = new DateTime(Convert.ToInt16(cts.date.ToString("yyyy")), Convert.ToInt16(cts.date.ToString("MM")), Convert.ToInt16(cts.date.ToString("dd")), cts.end_time.Hours, cts.end_time.Minutes, 0);

			string[] contents = { "BEGIN:VCALENDAR",
									"PRODID:-//nesi.ca//nesi//EN",
									"METHOD:CANCEL",
									"BEGIN:VEVENT",
									"UID:" + cth.member_id.ToString() + cth.id.ToString() + cts.id.ToString(),
									"DTSTART:" + s.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
									"DTEND:" + en.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
									"LOCATION:" + cts.location, 
									"DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + email.Subject,
									"SUMMARY:" + email.Subject, "PRIORITY:3", 
									"END:VEVENT", "END:VCALENDAR" };
			var temp_str = DateTime.Now.ToUniversalTime().ToString().Replace("/","").Replace(":","");
			System.IO.File.WriteAllLines((fileServer + @"\training_files\training" + temp_str + ".ics"), contents);
			var mailAttachment = new System.Net.Mail.Attachment((fileServer + @"\training_files\training" + temp_str + ".ics"));
			email.Attachment = mailAttachment;
			System.Threading.Thread.Sleep(1500);
			email.Send();
			System.Threading.Thread.Sleep(500);
			mailAttachment.Dispose();
			System.IO.File.Delete((fileServer + @"\training_files\training" + temp_str + ".ics"));


			}

		public static void send_update_calendar(int training_history_id)
			{
			var cth = new NeCapTraining_History(Convert.ToInt32(training_history_id));
			var cts = new NeCapTraining_Schedule(Convert.ToInt32(cth.cap_training_schedule_id));
			var members_email = new NeMember(cth.member_id).NEEmail.Contains("nomail") ? new NeMember(cth.member_id).Email : new NeMember(cth.member_id).NEEmail;

			var email = new NeEMail();
			var fileServer				= NeTaxEntity.BaseFolder(cth.business_unit_id, false);
			email.To = members_email;
			email.Subject = "CAP Training: " + new NeCapTraining(cth.training_header_id).name + " CHANGE";
			email.Body = "Training " + new NeCapTraining(cth.training_header_id).name + "<br>";
			email.Body += "Date: " + cts.date.ToString("yyyy-MM-dd") + "<br>";
			email.Body += "Start Time: " + cts.start_time.Hours + ":" + cts.start_time.Minutes.ToString().PadRight(2, '0') + "<br>";
			email.Body += "End Time: " + cts.end_time.Hours + ":" + cts.end_time.Minutes.ToString().PadRight(2, '0') + "<br>";
			email.Body += "Location: " + cts.location;
			email.From = "admin@" + Toolbox.app_setting("DomainForEmail");
			email.isHTML = true;

			var s = new DateTime(Convert.ToInt16(cts.date.ToString("yyyy")), Convert.ToInt16(cts.date.ToString("MM")), Convert.ToInt16(cts.date.ToString("dd")), cts.start_time.Hours, cts.start_time.Minutes, 0);
			var en = new DateTime(Convert.ToInt16(cts.date.ToString("yyyy")), Convert.ToInt16(cts.date.ToString("MM")), Convert.ToInt16(cts.date.ToString("dd")), cts.end_time.Hours, cts.end_time.Minutes, 0);

			string[] contents = { "BEGIN:VCALENDAR",
									"PRODID:-//nesi.ca//nesi//EN",
									"METHOD:REQUEST",
									"BEGIN:VEVENT",
									"UID:" + cth.member_id.ToString() + cth.id.ToString() + cts.id.ToString(),
									"DTSTART:" + s.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
									"DTEND:" + en.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
									"LOCATION:" + cts.location, 
									"DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + email.Subject,
									"SUMMARY:" + email.Subject, "PRIORITY:3", 
									"ORGANIZER:admin@"+ Toolbox.app_setting("DomainForEmail"),
									"SEQUENCE:1",
									"END:VEVENT", "END:VCALENDAR" };
			var temp_str = DateTime.Now.ToUniversalTime().ToString().Replace("/", "").Replace(":", "");
			System.IO.File.WriteAllLines((fileServer + @"\training_files\training" + temp_str + ".ics"), contents);
			var mailAttachment = new System.Net.Mail.Attachment((fileServer + @"\training_files\training" + temp_str + ".ics"));
			email.Attachment = mailAttachment;
			System.Threading.Thread.Sleep(1500);
			email.Send();
			mailAttachment.Dispose();
			System.IO.File.Delete((fileServer + @"\training_files\training" + temp_str + ".ics"));


			}


		public void save()
			{
			if (id == 0)
				{
				id = Toolbox.doSQL_return_id(@"
INSERT INTO cap_training_schedule
	(
	training_header_id,
	date,	
	start_time,
	end_time,
	location,
	max_fill,	
	date_added
	)
VALUES
	(@v0,@v1,@v2,@v3,@v4,@v5,
	NOW()
	)
", new object[] {
					training_header_id,								// {0}
					date.ToString("yyyy-MM-dd"), 								// {1}
					start_time,		// {2}
					end_time, 			// {3}
					location,	// {4}
					max_fill
				});
				}
			else
				{
				Toolbox.doSQL_void(@"
UPDATE 
	cap_training_schedule
SET
	training_header_id			= @v1,
	date			= @v2,	
	start_time		= @v3,
	end_time				= @v4,
	location		= @v5,
	max_fill			= @v6	
WHERE
	id = @v0
LIMIT 1
", new object[] {
					id,										// {0}
					training_header_id,								// {1}
					date.ToString("yyyy-MM-dd"), 								// {2}
					start_time,		// {3}
					end_time,			// {4}
					location,	// {5}
					max_fill
				});
				}
			}
		public bool delete(int _id)
			{
			if (exists(_id))
				{
				Toolbox.doSQL_void(@"DELETE FROM cap_training_schedule WHERE id = @v0 LIMIT 1", _id);
				return true;
				}
			else
				{
				return false;
				}
			}
		}
	}