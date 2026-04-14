using DevExpress.Xpo;
using System;
using System.Data;
using System.Linq;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NePhoneNumbers
	/// </summary>
	public class NePhoneNumbers
	{
		private int _phone_numbers_id = 0;
		private string _phone_numbers_number;
		private int _phone_numbers_table_id;
		private string _phone_numbers_type;
		private string _phone_numbers_added_date;
		private bool _phone_numbers_active;
		private string _phone_numbers_comm_type;
		private string _phone_numbers_comments;
		private bool _phone_numbers_default;
		private readonly Toolbox _tools = new Toolbox();

		public int phone_numbers_id { get { return _phone_numbers_id; } set { _phone_numbers_id = value; } }
		public int id { get { return _phone_numbers_id; } set { _phone_numbers_id = value; } }

		public int phone_numbers_table_id { get { return _phone_numbers_table_id; } set { _phone_numbers_table_id = value; } }
		public int table_id { get { return _phone_numbers_table_id; } set { _phone_numbers_table_id = value; } }

		public string phone_numbers_type { get { return _phone_numbers_type; } set { _phone_numbers_type = value; } }
		public string type { get { return _phone_numbers_type; } set { _phone_numbers_type = value; } }

		public string phone_numbers_added_date { get { return _phone_numbers_added_date; } set { _phone_numbers_added_date = value; } }
		public string added_date { get { return _phone_numbers_added_date; } set { _phone_numbers_added_date = value; } }

		public bool phone_numbers_active { get { return _phone_numbers_active; } set { _phone_numbers_active = value; } }
		public bool is_active { get { return _phone_numbers_active; } set { _phone_numbers_active = value; } }

		public string phone_numbers_comm_type { get { return _phone_numbers_comm_type; } set { _phone_numbers_comm_type = value; } }
		public string comm_type { get { return _phone_numbers_comm_type; } set { _phone_numbers_comm_type = value; } }

		public string phone_numbers_comments { get { return _phone_numbers_comments; } set { _phone_numbers_comments = value; } }
		public string comments { get { return _phone_numbers_comments; } set { _phone_numbers_comments = value; } }

		public string phone_numbers_number { get { return _phone_numbers_number; } set { _phone_numbers_number = value; } }
		public string number { get { return _phone_numbers_number; } set { _phone_numbers_number = value; } }

		public bool phone_numbers_default { get { return _phone_numbers_default; } set { _phone_numbers_default = value; } }
		public bool is_default { get { return _phone_numbers_default; } set { _phone_numbers_default = value; } }

		public NePhoneNumbers() { }
		public NePhoneNumbers(object _id)
		{
			init(Convert.ToInt32(_id));
		}

		private void init(int _id)
		{
			var count = _tools.getSQL_int(@"SELECT count(*) FROM Phone_Numbers WHERE phone_numbers_id = @v0 ", new object[] { _id });
			if (count > 0)
			{
				var _dt = Toolbox.doSQL_dt(@"SELECT * FROM Phone_Numbers WHERE phone_numbers_id = @v0  LIMIT 1", new object[] { _id });
				foreach (DataRow _dr in _dt.Rows)
				{
					_phone_numbers_id = _id;
					_phone_numbers_table_id = Convert.ToInt32(_dr["phone_numbers_table_id"]);
					_phone_numbers_type = _dr["phone_numbers_type"].ToString();
					_phone_numbers_added_date = _dr["phone_numbers_added_date"].ToString();
					_phone_numbers_active = Convert.ToBoolean(_dr["phone_numbers_active"]);
					_phone_numbers_comm_type = _dr["phone_numbers_comm_type"].ToString();
					_phone_numbers_comments = _dr["phone_numbers_comments"].ToString();
					_phone_numbers_number = _dr["phone_numbers_number"].ToString();
					_phone_numbers_default = Convert.ToBoolean(_dr["phone_numbers_default"]);
				}
			}
		}
		public DataSet GetNePhoneNumbers(int AddressID, string type_, string _status, string _comm_type)
		{
			return _tools.getSQL_dataset(@"SELECT * from Phone_Numbers where Phone_Numbers_active Like @v0  and phone_numbers_type = @v1  and phone_numbers_table_id = @v2  and phone_numbers_comm_type = @v3 ", new object[] { _status, type_, AddressID, _comm_type });
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="AddressID"></param>
		/// <param name="type_"></param>
		/// <param name="_status"></param>
		/// <param name="_comm_type"></param>
		/// <returns></returns>
		public string GetDefaultPhoneNumbers(int AddressID, string type_, string _status, string _comm_type)
		{
			using (var uow = new UnitOfWork())
			{
				var is_active = _status == "1";
				var p = (from x in new XPQuery<ne_xpo.cs.phone_numbers>(uow)
						 where
						 x.phone_numbers_table_id == AddressID &&
						 x.phone_numbers_type == type_ &&
						 x.phone_numbers_active == is_active &&
						 x.phone_numbers_comm_type == _comm_type &&
						 x.phone_numbers_default == true
						 select
						 new
						 {
							 phone = x.phone_numbers_number
						 }).ToList();
				return p.Count == 0 ? "" : p[0].phone.Replace("-", "");
			}
		}
		public DataSet GetAllPhoneNumbers()
		{
			return _tools.getSQL_dataset(@"SELECT phone_numbers.phone_numbers_id, phone_numbers.phone_numbers_number, contact.Contact_Name, phone_numbers.phone_numbers_type FROM phone_numbers Inner Join contact ON phone_numbers.phone_numbers_table_id = contact.Contact_ID  WHERE phone_numbers.phone_numbers_type = 'Contact' AND phone_numbers.phone_numbers_comm_type = 'Cell'", null);
		}
		public DataSet GetAllPhoneNumbers(string _number)
		{
			return _tools.getSQL_dataset(@"SELECT phone_numbers.phone_numbers_id, phone_numbers.phone_numbers_number, contact.Contact_Name, phone_numbers.phone_numbers_type, urldecode(customer.customer_name) customer_name,phone_numbers.phone_numbers_comm_type FROM phone_numbers Left Join contact ON phone_numbers.phone_numbers_table_id = contact.Contact_ID Left Join address ON address.Address_ID = phone_numbers.phone_numbers_table_id Inner Join customer ON address.Address_Table_ID = customer.Customer_ID where replace(phone_numbers_number,' ','') = @v0 ", new object[] { _number });
		}

		public void delete()
		{

			var c = _tools.getSQL_int(@"SELECT COUNT(*) FROM phone_numbers WHERE phone_numbers_table_id = @v0  AND phone_numbers_type = @v1  AND phone_numbers_comm_type = @v2 ", new object[] { _phone_numbers_table_id, _phone_numbers_type, _phone_numbers_comm_type });
			if (c > 1)
			{
				if (!_phone_numbers_default)
				{
					_tools.getSQL_void(@"Delete from phone_numbers where phone_numbers_id = @v0 limit 1", new object[] { _phone_numbers_id });
				}
				else
				{
					throw new Exception("You can not delete the default number for this address");
				}
			}
			else
			{
				throw new Exception("You can not delete the last number for this address");
			}

		}

		public void Save()
		{



			var _rows = _tools.getSQL_int(@"SELECT COUNT(*) FROM phone_numbers WHERE phone_numbers_id = @v0 ", new object[] { _phone_numbers_id });
			var c = _tools.getSQL_int(@"SELECT COUNT(*) FROM phone_numbers WHERE phone_numbers_table_id = @v0  AND phone_numbers_type = @v1  AND phone_numbers_comm_type = @v2 ", new object[] { _phone_numbers_table_id, _phone_numbers_type, _phone_numbers_comm_type });
			if (c > 0 && _rows != 0)
			{
				//_phone_numbers_id	= _tools.getSQL_int(@"SELECT phone_numbers_id FROM phone_numbers WHERE phone_numbers_table_id = @v0  AND phone_numbers_type = @v1  AND phone_numbers_comm_type = @v2 ", new object[] {  _phone_numbers_table_id, _phone_numbers_type, _phone_numbers_comm_type } );
				_rows = 1;
			}
			if (_rows == 1)
			{
				_tools.getSQL_void(@"
UPDATE 
	phone_numbers 
SET 
	phone_numbers_table_id		= @v0,
	phone_numbers_type			= @v1,
	phone_numbers_comm_type		= @v2,
	phone_numbers_comments		= @v3,
	phone_numbers_number		= @v4, 
	phone_numbers_default		= @v5,
	phone_numbers_active		= @v6
WHERE 
	phone_numbers_id = @v7 
LIMIT 1", new object[] {
					_phone_numbers_table_id,
					_phone_numbers_type,
					_phone_numbers_comm_type,
					_phone_numbers_comments,
					_phone_numbers_number,
					_phone_numbers_default?1: 0,
					_phone_numbers_active?1:0,
					_phone_numbers_id
				});
			}
			else
			{
				_tools.getSQL_void(@"
INSERT INTO phone_numbers 
	(
	phone_numbers_table_id, 
	phone_numbers_type,
	phone_numbers_added_date, 
	phone_numbers_comm_type, 
	phone_numbers_number,
	phone_numbers_default,
	phone_numbers_active,
	phone_numbers_comments
	) 
VALUES
	(
	@v0,
	@v1,
	now(),
	@v2,
	@v3,
	@v4,
	@v5,
	@v6
	)", new object[] {
					_phone_numbers_table_id,		// 0
					_phone_numbers_type,			// 1
					_phone_numbers_comm_type,		// 2
					_phone_numbers_number,			// 3
					_phone_numbers_default?1:0,			// 4
					_phone_numbers_active?1:0,			// 5
					_phone_numbers_comments			// 6
				});




			}
			reset_default(_phone_numbers_table_id, _phone_numbers_comm_type, _phone_numbers_type);

			var d_p = get_default_number(_phone_numbers_table_id, _phone_numbers_comm_type, _phone_numbers_type);
			if (d_p.id >= 0)
			{
				if (d_p.type.Equals("Address"))
				{
					var a = new NEAddress(Convert.ToInt32(d_p.table_id));
					var pp = d_p.phone_numbers_number.Replace(" ", "");
					if (d_p.phone_numbers_comm_type.Equals("LandLine"))
					{
						a.PhoneArea = pp.Substring(0, 3);
						a.Phonefirst = pp.Substring(3, 3);
						a.PhoneLast = pp.Substring(6, 4);
						a.Save();
					}
					else
					{
						a.FaxArea = pp.Substring(0, 3);
						a.FaxFirst = pp.Substring(3, 3);
						a.FaxLast = pp.Substring(6, 4);
						a.Save();
					}

				}
			}

		}

		public static NePhoneNumbers get_default_number(int table_id, string comm_type, string phone_number_type)
		{
			var x = Toolbox.doSQL_int(@"Select ifnull((SELECT phone_numbers_id FROM phone_numbers 
WHERE phone_numbers_table_id = @v0 and phone_numbers_type = @v1 and phone_numbers_comm_type = @v2 and phone_numbers_default = 1 ),-1)", new object[] {
			table_id, phone_number_type, comm_type});
			return new NePhoneNumbers(x);

		}

		public static void reset_default(int table_id, string comm_type, string phone_number_type)
		{
			var x = Toolbox.doSQL_int(@"
SELECT COUNT(*) FROM phone_numbers WHERE phone_numbers_table_id = @v0 and phone_numbers_type = @v1 and phone_numbers_comm_type = @v2 and phone_numbers_default = 1 
",
				new object[] {
table_id, phone_number_type, comm_type
});
			if (x > 1)
			{
				Toolbox.doSQL_void(@"
update phone_numbers set phone_numbers_default = 0 
where phone_numbers_table_id = @v0 
and phone_numbers_type = @v1 
and phone_numbers_comm_type = @v2 
and phone_numbers_default = 1 
limit 1 ",
					new object[] {
table_id, phone_number_type, comm_type});
				reset_default(table_id, comm_type, phone_number_type);
			}
			else if (x == 0)
			{
				Toolbox.doSQL_void(@"update phone_numbers 
set phone_numbers_default = 1 
where phone_numbers_table_id = @v0
and phone_numbers_type = @v1
and phone_numbers_comm_type = @v2
limit 1 ",
					new object[] {
table_id, phone_number_type, comm_type});
			}

		}

		public static NePhoneNumbers get_phonenumber_record(object phonenumber)
		{
			var pn = new NePhoneNumbers();
			var p = phonenumber.ToString().Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
			if (p.Length == 10)
			{
				p = p.Substring(0, 3) + " " + p.Substring(3, 3) + " " + p.Substring(6, 4);
				var x = Toolbox.doSQL_int(@"Select ifnull((Select phone_numbers_id from phone_numbers 
where phone_numbers_number = @v0 and phone_numbers_active = 1 limit 1),0)", p);
				if (x != 0)
				{
					return new NePhoneNumbers(x);
				}
			}


			return pn;
		}

		public DataTable get_contact_from_phonenumber(object phonenumber)
		{
			var x = new DataTable();
			var p = phonenumber.ToString().Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
			if (p.Length == 10)
			{
				x = Toolbox.doSQL_dt(@"Select contact_id, if(replace(replace(replace(replace(Contact_CellPhone,'(',''),')',''),' ',''),'-','') = '" + p + "','Cell','Direct Line') _type from contact  where replace(replace(replace(replace(Contact_CellPhone,'(',''),')',''),' ',''),'-','') = @v0 or replace(replace(replace(replace(Contact_Directline,'(',''),')',''),' ',''),'-','') = @v1", new object[] { p, p });
			}
			if (x.Rows.Count == 0)
			{
				p = "1" + p;
				x = Toolbox.doSQL_dt(@"Select contact_id, if(replace(replace(replace(replace(Contact_CellPhone,'(',''),')',''),' ',''),'-','') = '" + p + "','Cell','Direct Line') _type from contact  where replace(replace(replace(replace(Contact_CellPhone,'(',''),')',''),' ',''),'-','') = @v0 or replace(replace(replace(replace(Contact_Directline,'(',''),')',''),' ',''),'-','') = @v1", new object[] { p, p });
			}


			return x;
		}

		public string GetWhoIsfromPhoneNumber(string phonenumber)
		{
			var dsPhonenos = "Unknown";
			var dt = Toolbox.doSQL_dt(@"SELECT * from Phone_Numbers where Phone_Numbers_number = @v0 ", new object[] { phonenumber });
			if (dt.Rows.Count > 0)
			{
				var dr = dt.Rows[0];
				var type_ = dr["phone_numbers_type"].ToString();
				var subdt = Toolbox.doSQL_dt(string.Format("SELECT * from {0} where {1}_id = {2}", dr["phone_numbers_type"], dr["phone_numbers_type"], dr["phone_numbers_table_id"]), null);
				if (subdt.Rows.Count == 1)
				{
					var subdr = subdt.Rows[0];
					if (type_ == "Address")
					{
						var table = subdr["address_table"].ToString();
						if (table == "Customer")
						{
							dsPhonenos = new NECustomer(Convert.ToInt32(subdr["Address_table_id"])).Customer_Name;
						}
						else if (table == "Vendor")
						{
							dsPhonenos = "Supplier";
						}
						else if (table == "Member")
						{
							dsPhonenos = new NeMember(Convert.ToInt32(subdr["Address_table_id"])).FullName;
						}
					}
					else
					{
						dsPhonenos = "Unknown";
					}
					return dsPhonenos;
				}
				else
				{
					return "Unknown";
				}
			}
			else
			{
				return "Unknown";
			}
		}

		public DataTable addressids_fromPhoneNumber(string ten_digit_phonenumber)
		{

			var dt = Toolbox.doSQL_dt(@"SELECT phone_numbers_table_id from Phone_Numbers where Phone_Numbers_number = @v0  and phone_numbers_type = 'Address'", new object[] { ten_digit_phonenumber });
			return dt;
		}


	}
}