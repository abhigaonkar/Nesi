using System;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace nesi.core
{
	/// <summary>
	/// Logging interface
	/// </summary>
	public class NELog
	{
		public object section_id { get; set; }
		public object action_id { get; set; }
		public object value_old { get; set; }
		public object value_new { get; set; }
		public object table { get; set; }
		public object table_id { get; set; }
		public object alt_table_id { get; set; }
		public object member_id { get; set; }
		public object business_unit_id { get; set; }
		public bool is_manual { get; set; }

		public void save()
		{
			using (var conn = Toolbox.connect())
			{
				save(conn);
			}
		}
		public void save(MySqlConnection _conn)
		{
			//Toolbox.do_debug("start - NELog.save("+table+","+table_id+","+alt_table_id+")");
			if (alt_table_id == null)
			{
				alt_table_id = 0;
			}

			var my_conn = _conn;
			var my_comm = new MySqlCommand();
			my_comm.Connection = my_conn;
			my_comm.CommandText = @"
INSERT INTO log
	(
	section_id,
	action_id,
	member_id,
	business_unit_id,
	associated_table,
	associated_table_id,
	associated_alt_table_id,
	dt,
	value_old,
	value_new,
	is_manual
	)
VALUES
	(
	@section_id,
	@action_id,
	@member_id,
	@business_unit_id,
	@table,
	@table_id,
	@alt_table_id,
	NOW(),
	@value_old,
	@value_new,
	@is_manual
	)
";
			my_comm.Parameters.AddWithValue("@section_id", section_id);
			my_comm.Parameters.AddWithValue("@action_id", action_id);
			my_comm.Parameters.AddWithValue("@member_id", member_id);
			my_comm.Parameters.AddWithValue("@business_unit_id", business_unit_id);
			my_comm.Parameters.AddWithValue("@table", table);
			my_comm.Parameters.AddWithValue("@table_id", table_id);
			my_comm.Parameters.AddWithValue("@alt_table_id", alt_table_id);
			my_comm.Parameters.AddWithValue("@value_old", value_old);
			my_comm.Parameters.AddWithValue("@value_new", value_new);
			my_comm.Parameters.AddWithValue("@is_manual", is_manual);

			try
			{
				my_comm.ExecuteNonQuery();
			}
			catch (Exception ee)
			{
			}
		}
		/// <summary>
		/// Writes an event to the windows event log.
		/// </summary>
		/// <param name="e_text">The text of the event</param>
		/// <param name="e_type">The type of event System.Diagnostics.EventLogEntryType</param>
		/// <param name="e_id">Number ID designator</param>
		public static void write_event(string e_text, EventLogEntryType e_type, int e_id)
		{
			if (EventLog.SourceExists("NESI"))
			{
				EventLog.WriteEntry("NESI", e_text, e_type, e_id);
			}
		}
	}
	public class ne_page_log
	{
		public int id { get; set; }
		public int member_id { get; set; }
		public string url { get; set; }
		public string host { get; set; }
		public string query_string { get; set; }
		public string ip_address { get; set; }
		public DateTime request_start { get; set; }
		public DateTime request_end { get; set; }
		public DateTime render_end { get; set; }

		public void save()
		{
			if (member_id != 0)
			{
				if (id == 0)
				{
					id = Toolbox.doSQL_return_id(@"
INSERT INTO log_page 
	(
	dt, 
	member_id, 
	ip_address,
	url,  
	query_string,
	request_start,
	host
	) 
VALUES 
	(NOW(), @v0,@v1,@v2,@v3,@v4,@v5)",
	new object[] { member_id, ip_address, url, query_string, Toolbox.MySQL_longdt(request_start), host });
				}
				else
				{
					Toolbox.doSQL_void(@"UPDATE  
	log_page
SET
	request_end = @v0
WHERE
	id = @v1
LIMIT 1", new object[] { Toolbox.MySQL_longdt(request_end), id });
				}
			}
			//Toolbox.do_debug("end - page log save");
		}
		public void save_asax()
		{
			if (member_id != 0)
			{
				if (id == 0)
				{
					id = Toolbox.doSQL_return_id(@"INSERT INTO log_page_asax 
	(
	dt, 
	member_id, 
	ip_address,
	url, 
	query_string,
	request_start
	) 
VALUES 	(NOW(),@v0,@v1,@v2,@v3,@v4)", new object[] { member_id, ip_address, url, query_string, Toolbox.MySQL_longdt(request_start)});
				}
				else
				{
					Toolbox.doSQL_void(@"UPDATE  
	log_page_asax 
SET
	request_end = @v0
WHERE
	id = @v1
LIMIT 1", new object[] { Toolbox.MySQL_longdt(request_end), id });
				}
			}
			//Toolbox.do_debug("end - page log_asax save");
		}
	}

}
