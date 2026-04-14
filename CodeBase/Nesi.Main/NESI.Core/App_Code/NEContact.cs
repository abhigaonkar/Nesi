using System;
using System.Data;
using System.Web;
using System.Diagnostics;
using System.Linq;
using DevExpress.Xpo;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NEContact
	/// </summary>
	[Serializable()]
	public class NEContact
	{
		private int _id;
		private int _customer_id;
		private string _bvno;
		private int _quotemdb_id;
		private string _name;
		private string _email;
		private string _title;
		private string _cellphone;
		private string _directline;
		private string _status;
		private DateTime _birthday;
		private string _extension;
		private string _login;
		private string _password;
		private string _type;
		private int _status_id;
		private int _nesi_member_id;
		private int _login_enabled = 0;
		private DataSet _phonenumbers;
		private bool _stopsurveys = true;


		public int id { get { return _id; } set { _id = value; } }
		public int Contact_ID { get { return _id; } set { _id = value; } }

		public int customer_id { get { return _customer_id; } set { _customer_id = value; } }
		public int Contact_Cust_ID { get { return _customer_id; } set { _customer_id = value; } }

		public string bvno { get { return _bvno; } set { _bvno = value; } }
		public string Contact_BVNO { get { return _bvno; } set { _bvno = value; } }

		public int quotemdb_id { get { return _quotemdb_id; } set { _quotemdb_id = value; } }
		public int Contact_QuoteMDB_ID { get { return _quotemdb_id; } set { _quotemdb_id = value; } }

		public string Contact_Name { get { return _name; } set { _name = value; } }
		public string name { get { return _name; } set { _name = value; } }

		public string Contact_Email { get { return _email; } set { _email = value; } }
		public string email { get { return _email; } set { _email = value; } }

		public string Contact_Title { get { return _title; } set { _title = value; } }
		public string title { get { return _title; } set { _title = value; } }
		/* 'Purchaser',
		 * 'President',
		 * 'Shipper/Receiver'
		 * ,'Plant Manager',
		 * 'Mechanical Engineer',
		 * 'Electrical Engineer',
		 * 'Maintenance Manager',
		 * 'Accounts Payable' 
		 * 'Sales',
		 * 'Accounts Recievable',
		 */
		public string Contact_CellPhone { get { return _cellphone; } set { _cellphone = value; } }
		public string cellphone { get { return _cellphone; } set { _cellphone = value; } }

		public string Contact_DirectLine { get { return _directline; } set { _directline = value; } }
		public string direct_line { get { return _directline; } set { _directline = value; } }

		public string Contact_Status { get { return _status; } set { _status = value; } }
		public string status { get { return _status; } set { _status = value; } }

		public DateTime Contact_Birthday { get { return _birthday; } set { _birthday = value; } }
		public DateTime birthday { get { return _birthday; } set { _birthday = value; } }

		public string Contact_Extension { get { return _extension; } set { _extension = value; } }
		public string extension { get { return _extension; } set { _extension = value; } }

		public string Contact_Login { get { return _login; } set { _login = value; } }
		public string login { get { return _login; } set { _login = value; } }

		public string Contact_Password { get { return _password; } set { _password = value; } }
		public string password { get { return _password; } set { _password = value; } }

		public int Contact_Login_enabled { get { return _login_enabled; } set { _login_enabled = value; } }
		public int login_enabled { get { return _login_enabled; } set { _login_enabled = value; } }

		public string Contact_Type { get { return _type; } set { _type = value; } }
		public string type { get { return _type; } set { _type = value; } }

		public DataSet phonenumbers { get; set; }

		public int Contact_NesiMemberID { get { return _nesi_member_id; } set { _nesi_member_id = value; } }
		public int nesi_member_id { get { return _nesi_member_id; } set { _nesi_member_id = value; } }

		public int Contact_Status_ID { get { return _status_id; } set { _status_id = value; } }
		public int status_id { get { return _status_id; } set { _status_id = value; } }

		public bool contact_password_set { get; set; }
		public int address_id { get; set; }
		public string name_last { get; set; }
		public string name_first { get; set; }
		public string facebook { get; set; }
		public string twitter { get; set; }
		public string linkedin { get; set; }
		public bool stopsurveys { get; set; }
		public NEContact() { }
		public NEContact(int __id)
		{
			init(__id);
		}
		private bool exists(int _id)
		{
			using (var uow = new UnitOfWork())
			{
				return (from x in new XPQuery<ne_xpo.cs.contact>(uow)
						where x.contact_id == _id
						select x).Any();
			}
		}
		private void init(int _id)
		{
			using (var uow = new UnitOfWork())
			{
				if (!exists(_id)) return;
				var co = uow.GetObjectByKey<ne_xpo.cs.contact>(_id); ;
				id = _id;
				customer_id = co.contact_cust_id;
				address_id = co.address_id;
				bvno = co.contact_bvno;
				quotemdb_id = co.contact_quotemdb_id;
				name_first = co.name_first;
				name_last = co.name_last;
				name = co.contact_name;
				email = co.contact_email;
				title = co.contact_title;
				cellphone = co.contact_cellphone;
				direct_line = co.contact_directline;
				status = co.contact_status;
				nesi_member_id = co.contact_nesimemberid;
				status_id = co.contact_status_id;
				birthday = Toolbox.ReturnBlankDateTimeIfNull(co.contact_birthday);
				extension = co.contact_extension;
				login = co.contact_login;
				password = string.IsNullOrEmpty(co.contact_password)
					? Toolbox.do_RandomString(8)
					: co.contact_password;
				contact_password_set = co.contact_password_set;
				facebook = co.facebook;
				twitter = co.twitter;
				linkedin = co.linkedin;
				login_enabled = co.contact_login_enabled;
				type = co.contact_type;
				stopsurveys = co.stopsurveys == 1;
			}
		}

		public static DataSet LoadContactList(int _cust_id, string _type)
		{
			var _tool = new Toolbox();
			return _tool.getSQL_dataset(@"SELECT SQL_NO_CACHE * FROM contact WHERE contact_type=@v0  AND contact_cust_id = @v1  AND contact_status = 'Active' ORDER BY contact_name", new object[] { _type, _cust_id });
		}
		public void AddNEContact(NEContact c)
		{
			c.id = 0;
			Save(c);
		}
		public void save()
		{
			Save(this);
		}
		public void Save(NEContact c)
		{
			c.password = string.IsNullOrEmpty(c.password)
				? Toolbox.do_RandomString(8)
				: c.password;
			// Adding stop gap... the address_id should never be 0.
			if (c.address_id == 0 && c.type == "Customer")
			{
				// Throw Matt an error
				if (HttpContext.Current != null)
				{
					var s = HttpContext.Current.Session;
					var req = HttpContext.Current.Request;
					if (req != null && s != null)
					{
						var extra = string.Format(@"
Request URL: {0}
Session ID: {1}
IP Address: {2}
Customer ID: {3}
Contact ID: {4}
", req.RawUrl, s["session"], req.UserHostAddress, c.customer_id, c.id);
					}
				}
				// Set it to the customer's billing address
				var temp_address_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(address_id),0) FROM address 
where address_table = 'Customer' AND address_table_id = @v0", c.customer_id);
				if (temp_address_id > 0 && c.address_id == 0)
				{
					c.address_id = temp_address_id;
				}
			}
			if (c.id != 0)
			{
				// Update
				Toolbox.doSQL_void(@"
UPDATE 
	contact 
SET 
	contact_name			= @v0,
	contact_email			= @v1,
	contact_cellphone		= @v2,
	contact_directline		= @v3,
	contact_extension		= @v4,
	contact_bvno			= @v5,
	contact_cust_id			= @v6,
	contact_type			= @v7,
	contact_quotemdb_id		= @v8,
	contact_status			= @v9,
	contact_login			= @v10,
	contact_password		= @v11,
	contact_title			= @v12,
	contact_status_id		= @v13,
	contact_password_set	= @v14,
	contact_login_enabled	= @v15,
	name_first				= @v16,
	name_last				= @v17,
	address_id				= @v18,
	facebook				= @v20,
	twitter					= @v21,
	linkedin				= @v22,
	stopsurveys				= @v23,
contact_nesimemberid = @v24
WHERE 
	contact_id = @v19
LIMIT 1", new object[] {
					c.name,					 // {0}
					c.email,				 // {1}
					c.cellphone,			 // {2}
					c.direct_line,			 // {3}
					c.extension,			 // {4}
					c.bvno,					 // {5}
					c.customer_id,								 // {6}
					c.type,					 // {7}
					c.quotemdb_id,								 // {8}
					c.status,				 // {9}
					c.login,				 // {10}
					c.password,				 // {11}
					c.title,				 // {12}
					c.status_id,								 // {13}
					c.contact_password_set,								 // {14}
					c.login_enabled,							 // {15}
					c.name_first,			 // {16}
					c.name_last,			 // {17}
					c.address_id,								 // {18}
					c.id,										 // {19}
					c.facebook,									// {20}
					c.twitter,									// {21}
					c.linkedin,									// {22}
				    stopsurveys,			//23
				    c.Contact_NesiMemberID			//24
				});
			}
			else
			{
				// Insert
				var a = new NEAddress(c.address_id);
				c.id = Toolbox.doSQL_return_id(@"
INSERT INTO contact 
	(
	contact_name,
	contact_email,
	contact_cellphone,
	contact_directline,
	contact_extension,
	contact_bvno,
	contact_cust_id,
	contact_type,
	contact_quotemdb_id,
	contact_status,
	contact_login,
	contact_password,
	contact_title,
	contact_status_id,
	contact_password_set,
	contact_login_enabled,
	name_first,
	name_last,
	address_id,
	facebook,
	twitter,
	linkedin,
	stopsurveys
	) 
VALUES (
	@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22
	)", new object[] {
					c.name,					 // {0}
					c.email,				 // {1}
					c.cellphone,			 // {2}
					c.direct_line,			 // {3}
					c.extension,			 // {4}
					c.bvno,					 // {5}
					a.Table_ID==0? c.Contact_Cust_ID: a.Table_ID,								 // {6}
					string.IsNullOrEmpty(a.Table)? c.Contact_Type: a.Table,					 // {7}
					c.quotemdb_id,								 // {8}
					c.status,				 // {9}
					c.login,				 // {10}
					c.password,				 // {11}
					c.title,				 // {12}
					c.status_id,								 // {13}
					c.contact_password_set,								 // {14}
					c.login_enabled,							 // {15}
					c.name_first,			 // {16}
					c.name_last,			 // {17}
					c.address_id,								 // {18}
					c.facebook,									  // {19}
					c.twitter,									// {20}
					c.linkedin,									// {21}
					stopsurveys,				//{22}
					 
					
				});
				c = new NEContact(c.id);
				c.nesi_member_id = c.id + 100000;
				c.save();
			}

		}
		public static string delete(int __id)
		{
			var x = 0;
			var contact_id = __id;
			x = Toolbox.doSQL_int(@"Select count(woprog_id) from woprog where woprog_contact_id =@v0", contact_id);
			var _tools = new Toolbox();
			if (x > 0)
			{
                //throw new Exception("You can't delete this contact, it's found on " + x + " work order(s). You need to merge it.");
                 return "You can't delete this contact, it's found on " + x + " work order(s). You need to merge it.";
            }
			x = 0;
			x = Toolbox.doSQL_int(@"Select count(quote_id) from quote_master where contact_id = @v0", contact_id);
			if (x > 0)
			{
				//throw new Exception("You can't delete this contact, it's found on " + x + " quote(s).  you need to merge it.");
                return "You can't delete this contact, it's found on " + x + " quote(s).  you need to merge it.";
            }
			try
			{
				Toolbox.doSQL_void(@"DELETE from contactpage where contactpage_contact_id = @v0", contact_id);
				Toolbox.doSQL_void(@"DELETE from contactpageprivilege where contactpageprivilege_contact_id = @v0", contact_id);
				Toolbox.doSQL_void(@"DELETE from contact where contact_id =@v0 limit 1", contact_id);
			}
			catch (Exception ee)
			{
				_tools.catch_error(ee);
				throw new Exception("This thing crashed during the deletion of this contact!!");
			}
            return "";
		}
		public void merge_contacts(object c_from, object c_to, NeMember current_member)
		{
			/*
		From Ticket 1633
			1. Check what fields are missing from B and populate them with A's... but don't overwrite non blanks. 
			2. Set B login enable and active status to what A used to be 
			3. Change phone number contact ID references to B  -- N/A contact doesn't currently references the phone_numbers table... for the time being I am going to hard code to's phone number to from's phone number.
			4. Change all status of A to disabled and extinct etc in the contact table. 
			5. Create a record in the contact_history table for A. 
			6. Change all contact_page and contactprivy references to B 
			7. Change all contactID references on all "open" records in the quote and wo tables. 
		*/
			var contact_from = new NEContact(Convert.ToInt32(c_from));
			var contact_to = new NEContact(Convert.ToInt32(c_to));
			#region 1. Check what fields are missing from B, and populate them with A's values
			if (contact_to.email == "" && contact_from.email != "")
			{
				contact_to.email = contact_from.email;
			}
			if (contact_to.birthday.Year < 1900 && contact_from.birthday.Year > 1900)
			{
				contact_to.birthday = contact_from.birthday;
			}
			if (contact_to.bvno == "" && contact_from.bvno != "")
			{
				contact_to.bvno = contact_from.bvno;
			}
			if (contact_to.cellphone == "" && contact_from.cellphone != "")
			{
				contact_to.cellphone = contact_from.cellphone;
			}
			if (contact_to.extension == "" && contact_from.extension != "")
			{
				contact_to.extension = contact_from.extension;
			}
			if (contact_to.login == "" && contact_from.login != "")
			{
				contact_to.login = contact_from.login;
			}
			if (contact_to.nesi_member_id == 0 && contact_from.nesi_member_id != 0)
			{
				contact_to.nesi_member_id = contact_from.nesi_member_id;
			}
			if (contact_to.password == "" && contact_from.password != "")
			{
				contact_to.contact_password_set = contact_from.contact_password_set;
				contact_to.contact_password_set = contact_from.contact_password_set;
			}
			if (contact_to.quotemdb_id == 0 && contact_from.quotemdb_id != 0)
			{
				contact_to.quotemdb_id = contact_from.quotemdb_id;
			}
			if (contact_to.status == "" && contact_from.status != "")
			{
				contact_to.status = contact_from.status;
			}
			if (contact_to.status_id == 0 && contact_from.status_id != 0)
			{
				contact_to.status_id = contact_from.status_id;
			}
			if (contact_to.title == "" && contact_from.title != "")
			{
				contact_to.title = contact_from.title;
			}
			if (contact_to.type == "" && contact_from.type != "")
			{
				contact_to.type = contact_from.type;
			}
			contact_to.address_id = contact_from.address_id;
			if (contact_to.name_first.Trim() == "" && contact_to.name_first.Trim() != contact_from.name_first.Trim())
			{
				contact_to.name_first = contact_from.name_first;
			}
			if (contact_to.name_last.Trim() == "" && contact_to.name_last.Trim() != contact_from.name_last.Trim())
			{
				contact_to.name_last = contact_from.name_last;
			}
			#endregion Check what fields are missing from B, and populate them with A's values
			#region 2. Set Login Enabled
			contact_to.Contact_Login_enabled = contact_from.Contact_Login_enabled;
			#endregion 2. Set Login Enabled
			#region 3. Change phone numbers that contact_from references to contact_to's ID
			if ((contact_to.cellphone.Trim() == "" && contact_from.cellphone.Trim() != "") ||
				   (contact_to.cellphone.Trim() != "" && contact_from.cellphone.Trim() != ""))
			{
				contact_to.cellphone = contact_from.cellphone.Trim();
			}
			if ((contact_to.direct_line.Trim() == "" && contact_from.direct_line.Trim() != "") ||
				   (contact_to.direct_line.Trim() != "" && contact_from.direct_line.Trim() != ""))
			{
				contact_to.direct_line = contact_from.direct_line.Trim();
			}
			#endregion 3. Change phone numbers that contact_from references to contact_to's ID
			#region 4. Social Networks
			if ((contact_to.facebook.Trim() == "" && contact_from.facebook.Trim() != "") ||
				   (contact_to.facebook.Trim() != "" && contact_from.facebook.Trim() != ""))
			{
				contact_to.facebook = contact_from.facebook.Trim();
			}
			if ((contact_to.twitter.Trim() == "" && contact_from.twitter.Trim() != "") ||
				   (contact_to.twitter.Trim() != "" && contact_from.twitter.Trim() != ""))
			{
				contact_to.twitter = contact_from.twitter.Trim();
			}
			if ((contact_to.linkedin.Trim() == "" && contact_from.linkedin.Trim() != "") ||
				   (contact_to.linkedin.Trim() != "" && contact_from.linkedin.Trim() != ""))
			{
				contact_to.linkedin = contact_from.linkedin.Trim();
			}
			#endregion 4. Social Networks
			#region 5. Change all status of A to disabled and extinct etc in the contact table.
			contact_from.status_id = 4;
			contact_from.status = "Inactive";
			contact_from.Save(contact_from);
			contact_to.Save(contact_to);
			#endregion 5. Change all status of A to disabled and extinct etc in the contact table.
			#region 6. Create a record in contact_history

			var h = new NEContactHistory
			{
				contact_id = contact_from.id,
				date = DateTime.Now,
				customerwide = true,
				notes = "Merged into " + contact_from.name,
				enteredby = current_member.id
			};
			h.AddNEContact_history(h);
			#endregion 6. Create a record in contact_history
			#region 7. Move everything for contact_from into contact_to in the contactpage, contactpageprivilege link tables
			Toolbox.doSQL_void(@"UPDATE contactpageprivilege SET contactpageprivilege_contact_id = @v0 WHERE contactpageprivilege_contact_id = @v1",
				new object[] {
				contact_to.id, contact_from.id});
			Toolbox.doSQL_void(@"UPDATE contactpage SET contactpage_contact_id = @v0 WHERE contactpage_contact_id = @v1",
				new object[] {
				contact_to.id, contact_from.id});
			#endregion 7. Move everything for contact_from into contact_to in the contactpage, contactpageprivilege tables
			#region 8. Change all contact references on all open records in quote/wo tables
			// Quote
			Toolbox.doSQL_void(@"UPDATE quote_master SET contact_id = @v0 WHERE contact_id = @v1 ", new object[] {
				contact_to.id, contact_from.id});
			// WO
			Toolbox.doSQL_void(@"UPDATE woprog SET woprog_contact_id = @v0 WHERE woprog_contact_id = @v1 ", new object[] {
				contact_to.id, contact_from.id});
			#endregion 8. Change all contact references on all open records in quote/wo tables
		}
	}
}