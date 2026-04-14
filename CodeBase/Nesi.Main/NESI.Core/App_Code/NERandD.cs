using MySql.Data.MySqlClient;
using System;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NERandD
	/// </summary>
	public class NERandD
	{
		private int _RD_ID;
		private int _RD_WOProgID;
		private string _RD_ProjectAdvancement = "";
		private string _RD_ProjectUncertainty = "";
		private int _RD_Member_ID;
		private DateTime _RD_Date_Entered = DateTime.Now;
		private int _RD_MemberTime_ID;

		public int RD_ID
		{
			get { return _RD_ID; }
			set { _RD_ID = value; }
		}
		public int RD_WOProgID
		{
			get { return _RD_WOProgID; }
			set
			{
				_RD_WOProgID = value;
			}
		}
		public string RD_ProjectAdvancement
		{
			get
			{
				return _RD_ProjectAdvancement;
			}
			set { _RD_ProjectAdvancement = value; }
		}
		public string RD_ProjectUncertainty
		{
			get { return _RD_ProjectUncertainty; }
			set
			{
				_RD_ProjectUncertainty = value;
			}
		}
		public int RD_Member_ID
		{
			get
			{
				return _RD_Member_ID;
			}
			set
			{
				_RD_Member_ID = value;
			}
		}
		public DateTime RD_Date_Entered
		{
			get
			{
				return _RD_Date_Entered;
			}
			set
			{
				_RD_Date_Entered = value;
			}

		}
		public int RD_MemberTime_ID
		{
			get
			{
				return _RD_MemberTime_ID;
			}
			set
			{
				_RD_MemberTime_ID = value;
			}

		}

		public void InsertNewEntry(MySqlConnection connection = null, MySqlTransaction transaction = null)
		{
			var tools = new Toolbox();
			var InsertEntry = "INSERT INTO rd ";
			InsertEntry += "(RD_WOProgID, RD_ProjectAdvancement, RD_ProjectUncertainty, ";
			InsertEntry += "RD_Member_ID, RD_Date_Entered, RD_MemberTime_ID) ";
			InsertEntry += "VALUES(@v0,@v1,@v2,@v3,NOW(),@v4)";
			var paramObjects = new object[] {
				 _RD_WOProgID ,
				 _RD_ProjectAdvancement,
				 _RD_ProjectUncertainty,
				_RD_Member_ID,
				_RD_MemberTime_ID };
			if (connection == null || transaction == null)
				tools.getSQL_void(InsertEntry, paramObjects);
			else
				tools.getSQL_void(InsertEntry, paramObjects, connection, transaction);
		}

	}
}