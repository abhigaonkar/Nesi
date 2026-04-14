using System;
using System.Data;

namespace nesi.core
	{
	public class NeApplicant
		{
		#region Variables
		private int _id=0;
		private string _firstname="";
		private string _lastname="";
		private int _addedbymemberid=0;
		private DateTime _dateentered;
		private string _notes="";
		private string _status="";
		private int _membertypeid = 0;
	
		private string _address = "";
		private string _city = "";
		private string _province = "";
		private string _postal = "";
		private string _cellphone = "";
		private string _country = "";
		private string _email = "";
		private string _homephone = "";
		private string _apt = "";
		private int _becomes_memberid = 0;

		public int id { get { return _id; } set { _id = value; } }
		public int membertypeid { get { return _membertypeid; } set { _membertypeid = value; } }
		public int business_unit_id { get; set; }
		
		public int addedbymemberid { get { return _addedbymemberid; } set { _addedbymemberid = value; } }
		public string lastname { get { return _lastname; } set { _lastname = value; } }
		public string firstname { get { return _firstname; } set { _firstname = value; } }
		public DateTime dateentered { get { return _dateentered; } set { _dateentered = value; } }
		public string notes { get { return _notes; } set { _notes = value; } }
		public string address { get { return _address; } set { _address = value; } }
		public string city { get { return _city; } set { _city = value; } }
		public string province { get { return _province; } set { _province = value; } }
		public string postal { get { return _postal; } set { _postal = value; } }
		public string cellphone { get { return _cellphone; } set { _cellphone = value; } }
		public string homephone { get { return _homephone; } set { _homephone = value; } }
		public string country { get { return _country; } set { _country = value; } }
		public string status { get { return _status; } set { _status = value; } }
		public string email { get { return _email; } set { _email = value; } }
		public string apt { get { return _apt; } set { _apt = value; } }
		public int becomes_memberid { get { return _becomes_memberid; } set { _becomes_memberid = value; } }
		    public NeBusinessUnit business_unit
		        {
		        get { return new NeBusinessUnit(business_unit_id); }
		        }
        #endregion
        public NeApplicant()
			{

			}
		public NeApplicant(int id)
			{
			var dt  = Toolbox.doSQL_dt(@"Select * from applicants  where id =@v0", new object[] { id });
			foreach (DataRow dr in dt.Rows)
				{
				_addedbymemberid = Convert.ToInt32(dr["addedbymemberid"]);
				_firstname = dr["firstname"].ToString();
				_lastname = dr["lastname"].ToString();
				_notes = dr["notes"].ToString();
				_status = dr["status"].ToString();
				_dateentered = dr["dateentered"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(dr["dateentered"]);
				business_unit_id = Convert.ToInt32(dr["business_unit_id"]);
				    
                _membertypeid = Convert.ToInt32(dr["membertypeid"]);
				
				_id = id;
				_address = dr["address"].ToString();
				_city = dr["city"].ToString();
				_province = dr["province"].ToString();
				_postal = dr["postal"].ToString();
				_cellphone = dr["cellphone"].ToString();
				_homephone = dr["homephone"].ToString();
				_country = dr["country"].ToString();
				_email = dr["email"].ToString();
				_apt = dr["apt"].ToString();
				_becomes_memberid = dr["becomes_memberid"] == DBNull.Value ? 0 : Convert.ToInt32(dr["becomes_memberid"]);
				}
			}
		public void save()
			{
			if (id == 0)
				{
				Toolbox.doSQL_void(@"Insert into applicants (firstname,lastname,addedbymemberid,notes,status,dateentered,membertypeid,business_unit_id,address,city,province,country,postal,cellphone,email,homephone,apt,becomes_memberid)  values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17)",new object[] { _firstname,_lastname,_addedbymemberid,_notes,_status,_dateentered.ToString("yyyy-MM-dd"),_membertypeid,business_unit_id,_address,_city,_province,_country,_postal,_cellphone,_email,_homephone,_apt,_becomes_memberid } );
				_id = Toolbox.doSQL_int(@"Select id from applicants order by id desc limit 1"  , new object[] { });
				}
			else
				{
				Toolbox.doSQL_void(@"Update applicants  Set firstname=@v0,lastname=@v1 ,addedbymemberid=@v2 ,notes=@v3 ,status=@v4 ,dateentered=@v5 , business_unit_id =@v6 , membertypeid =@v7 ,address =@v8 ,city =@v9 ,province =@v10 ,country =@v11 ,postal =@v12 ,cellphone =@v13 ,email =@v14 , homephone=@v15 , apt =@v16 ,becomes_memberid=@v17   where id =@v18", new object[] { _firstname,_lastname,_addedbymemberid,_notes,_status,_dateentered.ToString("yyyy-MM-dd"),business_unit_id,_membertypeid,_address,_city,_province,_country,_postal,_cellphone,_email,_homephone,_apt,_becomes_memberid,_id });

				}
			}
	
  
	
		}
	}