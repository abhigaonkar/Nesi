
using System;
using System.Data;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NEContact
	/// </summary>
	public class NECellphone
	{
	public int business_unit_id { get; set; }

		public int id { get; set; }
		public string os { get; set; }
		public string imei { get; set; }
		public int status_id { get; set; }
		public DateTime activedate { get; set; }
		public int last_update_member_id { get; set; }
		public DateTime last_modified { get; set; }
		public int carrier_id { get; set; }
		cellphone_status status { get; set; }
		cellphone_carrier carrier { get; set; }

		public NECellphone() { }
		public NECellphone(int _id)
		{
			if (exists(_id))
			{
				load(_id);
			}
		}
		public bool exists(int _id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM cellphone WHERE id = @v0", _id) > 0;
		}
		private void load(int _id)
		{
			var dr = Toolbox.doSQL_dt(@" SELECT id, cellphone_status_id, cellphone_carrier_id, activedate, imei, OS,
simcard, number, last_modified,business_unit_id FROM cellphone WHERE id = @v0 ", new object[] {  _id } ).Rows[0];
			id = _id;
			os = dr["OS"].ToString();
			imei = dr["imei"].ToString();
			status_id = Convert.ToInt16(dr["cellphone_status_id"]);
			carrier_id = Convert.ToInt16(dr["cellphone_carrier_id"]);
			activedate = Convert.ToDateTime(dr["activedate"]);
		if (dr["business_unit_id"] != DBNull.Value)
			business_unit_id = Convert.ToInt16(dr["business_unit_id"]);
		if (dr["last_modified"] != DBNull.Value)
			{
			last_modified = Convert.ToDateTime(dr["last_modified"]);
			}
			status = new cellphone_status(status_id);
			carrier = new cellphone_carrier(status_id);
		
		}

		public void save()
		{
			if (id > 0)  // update
			{
				var old_cell = new NECellphone(id);
				if (old_cell.activedate.ToString("yyyy-MM-dd") != activedate.ToString("yyyy-MM-dd"))
				{
					add_cellphone_history("Active Date changed from " + old_cell.activedate.ToString("yyyy-MM-dd") + " to " + activedate.ToString("yyyy-MM-dd") + ".");
				}

				if (old_cell.status_id != status_id)
				{
					status = new cellphone_status(status_id);
					add_cellphone_history("Status changed from " + old_cell.status.name + " to " + status.name + ".");
				}
				if (old_cell.imei != imei)
				{
					add_cellphone_history("IMEI changed from " + old_cell.imei + " to " + imei + ".");
				}
				if (old_cell.os != os)
				{
					add_cellphone_history("OS changed from " + old_cell.os + " to " + os + ".");
				}
				if (old_cell.carrier_id != carrier_id)
				{
					carrier = new cellphone_carrier(carrier_id);
					add_cellphone_history("Carrier changed from " + old_cell.carrier.name + " to " + carrier.name + ".");
				}

				Toolbox.doSQL_void(@"
UPDATE
	cellphone
SET
	os = @v0,
	cellphone_carrier_id = @v1,
	imei = @v2,
	activedate = @v3,
	cellphone_status_id = @v4,
	last_modified=NOW()
WHERE
	id = @v5
LIMIT 1", new object[] {
					os,
					carrier_id,
					imei,
					Toolbox.MySQL_shortdt(activedate),
					status_id,
					id
				});
			}
			else // insert
			{
				id = Toolbox.doSQL_return_id(@"
INSERT INTO cellphone
	(
	os,
	imei,
	cellphone_status_id,
	activedate,
	cellphone_carrier_id,
	business_unit_id,
	last_modified
	)
VALUES (@v0,@v1,@v2,@v3,@v4,@v5,NOW())",
new object[] {
					os,
					imei,
					status_id,
					Toolbox.MySQL_shortdt(activedate),
					carrier_id,
					business_unit_id
				});
			}
		}

		public void add_cellphone_history(string note)
		{
			var _tools = new Toolbox();
			_tools.getSQL_void(@"INSERT INTO cellphone_history
(dt,member_id,cellphone_id,notes) 
VALUES (NOW(),@v0,@v1,@v2)",
new object[] {
last_update_member_id,id, note});
		}

		public static void delete(int _id)
		{
			var cell = new NECellphone(_id);
			Toolbox.doSQL_void(@"UPDATE member SET cellphone_id = NULL WHERE cellphone_number_id = @v0 LIMIT 1", _id);
			cell.add_cellphone_history("Cellphone removed from all members.");
			Toolbox.doSQL_void(@"DELETE FROM cellphone WHERE id = @v0 LIMIT 1", _id);
		}

		private class cellphone_status : NECellphone
		{
			public int sc_id { get; set; }
			public string name { get; set; }

			public cellphone_status(int _id)
			{
				if (status_exists(_id))
				{
					load_status(_id);
				}
			}
			private void load_status(int _id)
			{
				sc_id = _id;
				var dr = Toolbox.doSQL_dt(@"SELECT name FROM cellphone_status WHERE id = @v0  LIMIT 1", new object[] {  _id } ).Rows[0];
				name = (string)dr["name"];
			}
			private bool status_exists(int _id)
			{
				return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM cellphone_status WHERE id = @v0", _id) > 0;
			}
		}

		public class cellphone_carrier : NECellphone
		{
			public int cc_id { get; set; }
			public string name { get; set; }
			public cellphone_carrier() { }
			public cellphone_carrier(int _id)
			{
				if (carrier_exists(_id))
				{
					load_carrier(_id);
				}
			}
			private void load_carrier(int _id)
			{
				cc_id = _id;
				var dr = Toolbox.doSQL_dt(@"SELECT name FROM cellphone_carrier WHERE id = @v0  LIMIT 1", new object[] {  _id } ).Rows[0];
				name = (string)dr["name"];
			}
			private bool carrier_exists(int _id)
			{
				return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM cellphone_carrier WHERE id = @v0", _id) > 0;
			}
		}
	}
	public class NECellphone_Number
	{
		public int business_unit_id { get; set; }

		public int id { get; set; }
		public string number { get; set; }
		public DateTime active_date { get; set; }
		public string simcard { get; set; }
		public int last_update_member_id { get; set; }
		public DateTime last_modified { get; set; }
		public string status { get; set; }
		public int carrier_id { get; set; }
		public cellphone_carrier carrier { get; set; }

		public NECellphone_Number() { }
		public NECellphone_Number(int _id)
		{
			if (exists(_id))
			{
				load(_id);
			}
		}
		private void load(int _id)
		{
			var dr = Toolbox.doSQL_dt(@" SELECT id, number, cellphone_carrier_id, 
simcard, activedate, status, last_modified,business_unit_id FROM cellphone_number WHERE id = @v0  LIMIT 1", new object[] {  _id } ).Rows[0];

			id = _id;
			number = dr["number"].ToString();
			status = dr["status"].ToString();
			if(dr["business_unit_id"]!=DBNull.Value)
				business_unit_id = Convert.ToInt16(dr["business_unit_id"]);
			if (dr["last_modified"] != DBNull.Value)
			{
				last_modified = Convert.ToDateTime(dr["last_modified"]);
			}
			if (dr["activedate"] != DBNull.Value)
			{
				active_date = Convert.ToDateTime(dr["activedate"]);
			}
			simcard = dr["simcard"].ToString();
			carrier_id = Convert.ToInt16(dr["cellphone_carrier_id"]);
			carrier = new cellphone_carrier(carrier_id);
		}

		private bool exists(int _id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM cellphone_number WHERE id = @v0", _id) > 0;
		}
		public static void delete(int _id)
		{
			var number = new NECellphone_Number(_id);
			Toolbox.doSQL_void(@"UPDATE member SET cellphone_number_id = NULL WHERE cellphone_number_id = @v0 LIMIT 1", _id);
			number.add_cellphone_number_history("Number removed from all members.");
			Toolbox.doSQL_void("DELETE FROM cellphone_number WHERE id = @v0 LIMIT 1", _id);
		}
		public void save()
		{
			if (id > 0)  // update
			{
				var old_cell = new NECellphone_Number(id);
				if (old_cell.simcard != simcard)
				{
					add_cellphone_number_history("SIM Card changed from " + old_cell.simcard + " to " + simcard + ".");
				}

				if (old_cell.number != number)
				{
					add_cellphone_number_history("Number changed from " + old_cell.number + " to " + number + ".");
				}
				if (old_cell.status != status)
				{
					add_cellphone_number_history("Status changed from " + old_cell.status + " to " + status + ".");
				}
				if (old_cell.carrier_id != carrier_id)
				{
					add_cellphone_number_history("Carrier changed from " + old_cell.carrier.name + " to " + carrier.name + ".");
				}

				Toolbox.doSQL_void(@"
UPDATE
	cellphone_number
SET
	number = @v0,
	activedate =@v1,
	simcard =@v2,
	status = @v3,
	cellphone_carrier_id = @v4
WHERE
	id = @v5
LIMIT 1", new object[] {
					number,
					Toolbox.MySQL_shortdt(active_date),
					simcard,
					status,
					carrier_id,
					id
				});
			}
			else // Insert
			{
				id = Toolbox.doSQL_return_id(@"
INSERT INTO cellphone_number 
	(
	number,
	simcard,
	status,
	cellphone_carrier_id,
	activedate,
	business_unit_id
	) 
VALUES 	(@v0,@v1,@v2,@v3,@v4,@v5)",
new object[] {
number,
					simcard,
					status,
					carrier_id,
					Toolbox.MySQL_shortdt(active_date),
					business_unit_id
				});
			}
		}

		public void add_cellphone_number_history(string note)
		{
			Toolbox.doSQL_void(@"INSERT INTO cellphone_number_history
(dt,member_id,cellphone_number_id,notes) 
VALUES (now(),@v0,@v1,@v2)",
new object[] {
last_update_member_id ,id,note});
		}

		public sealed class cellphone_carrier : NECellphone_Number
		{
			public int cc_id { get; set; }
			public string name { get; set; }

			public cellphone_carrier(int _id)
			{
				if (carrier_exists(_id))
				{
					load_carrier(_id);
				}
			}
			private void load_carrier(int _id)
			{
				cc_id = _id;
				var dr = Toolbox.doSQL_dt(@"SELECT name FROM cellphone_carrier WHERE id = @v0  LIMIT 1", new object[] {  _id } ).Rows[0];
				name = (string)dr["name"];
			}
			private bool carrier_exists(int _id)
			{
				return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM cellphone_carrier WHERE id = @v0", _id) > 0;
			}
		}
	}
}