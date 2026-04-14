using System;
using System.Data;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NEContact
	/// </summary>
	public class NEContactHistory
	{
		public int id { get; set; }
		public int contact_id { get; set; }
		public DateTime date { get; set; }
		public int enteredby { get; set; }
		public bool customerwide { get; set; }
		public string customer_name { get; set; }
		public string contact_name { get; set; }
		public string enteredby_name { get; set; }
		public string notes { get; set; }


		public NEContactHistory(int _contact_history_id)
		{
			var dt = Toolbox.doSQL_dt(@" SELECT a.contact_history_id id, a.contact_history_enteredby enteredby, IFNULL(b.member_fullname, '') enteredby_name, a.contact_history_contact_id contact_id, IFNULL(d.contact_name, '') contact_name, a.contact_history_date date, a.contact_history_notes notes, a.contact_history_customerwide customer_wide, IFNULL(c.customer_name, '') customer_name FROM contact_history a LEFT JOIN member b ON a.contact_history_enteredby = b.member_id LEFT JOIN customer c on a.contact_history_cust_id = c.customer_id LEFT JOIN contact d ON a.contact_history_contact_id = d.contact_id WHERE contact_history_contact_id = @v0 ", new object[] {  _contact_history_id } );

			foreach (DataRow dr in dt.Rows)
			{
				id = Convert.ToInt32(dr["id"]);
				enteredby = (int)dr["enteredby"];
				contact_id = (int)dr["contact_id"];
				date = (DateTime)dr["date"];
				notes = (string)dr["notes"];
				customerwide = (bool)dr["customerwide"];
				enteredby_name = (string)dr["enteredby_name"];
				contact_name = (string)dr["contact_name"];
				customer_name = (string)dr["customer_name"];
			}
		}



		public void AddNEContact_history(NEContactHistory _contact_history)
		{
			var str_sql = "INSERT INTO Contact_history (";
			str_sql += "Contact_history_enteredby,";
			str_sql += "Contact_history_contact_ID,";
			str_sql += "Contact_history_date,";
			str_sql += "Contact_history_notes,";
			str_sql += "Contact_history_customerwide) ";
			str_sql += "VALUES (@v0,@v1,@v2,@v3,@v4)";
			var paramObjects = new object[]
			{
					_contact_history.enteredby,
					 _contact_history.contact_id,
					_contact_history.date.ToString("yyyy-MM-dd"),
					_contact_history.notes,
					_contact_history.customerwide
			};
			_contact_history.id = Toolbox.doSQL_return_id(str_sql, paramObjects);
		}




		public NEContactHistory()
		{
			//
			//  
			//
		}
	}
}