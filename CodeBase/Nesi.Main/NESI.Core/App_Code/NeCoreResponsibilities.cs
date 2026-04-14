using System;
using System.Data;

namespace nesi.core
{
	/// <summary>
	/// Summary description for Member
	/// </summary>
	public class NeCoreResponsibilities
	{
		#region Variables
		private Toolbox _tools = new Toolbox();
		private int _id;
		private string _core_responsibility;
		private int _member_id;
		private DateTime _date_added;
		private string _status;
		private string _default_target;
		private string _description;
		private string _annually;
		private string _quarterly;
		private string _monthly;
		private string _weekly;
		private string _as_required;
		private int _cr_group_id;
		private string _cr_group_name;
		private string _daily;


		public int id { get { return _id; } set { _id = value; } }
		public int member_id { get { return _member_id; } set { _member_id = value; } }
		public string core_responsibility { get { return _core_responsibility; } set { _core_responsibility = value; } }
		public string status { get { return _status; } set { _status = value; } }
		public string default_target { get { return _default_target; } set { _default_target = value; } }
		public string description { get { return _description; } set { _description = value; } }
		public string annually { get { return _annually; } set { _annually = value; } }
		public string quarterly { get { return _quarterly; } set { _quarterly = value; } }
		public string monthly { get { return _monthly; } set { _monthly = value; } }
		public string weekly { get { return _weekly; } set { _weekly = value; } }
		public string as_required { get { return _as_required; } set { _as_required = value; } }
		public string cr_group_name { get { return _cr_group_name; } set { _cr_group_name = value; } }
		public string daily { get { return _daily; } set { _daily = value; } }
		public int cr_group_id { get { return _cr_group_id; } set { _cr_group_id = value; } }
		public DateTime dateadded { get { return _date_added; } set { _date_added = value; } }

		#endregion
		public NeCoreResponsibilities()
		{

		}
		public NeCoreResponsibilities(int id)
		{
			var dt = Toolbox.doSQL_dt(@"Select * from core_responsibilities  where id = @v0", new object[] { id });
			foreach (DataRow dr in dt.Rows)
			{
				_core_responsibility = dr["core_responsibility"].ToString();
				_member_id = Convert.ToInt32(dr["member_id"]);
				_date_added = Convert.ToDateTime(dr["date_added"]);
				_status = dr["status"].ToString();
				_default_target = dr["default_target"].ToString();
				_description = dr["description"].ToString();
				_annually = dr["annually"].ToString();
				_quarterly = dr["quarterly"].ToString();
				_monthly = dr["monthly"].ToString();
				_weekly = dr["weekly"].ToString();
				_as_required = dr["as_required"].ToString();
				_daily = dr["daily"].ToString();
				_cr_group_id = Convert.ToInt32(dr["cr_group_id"]);
			}
		}
		public void save()
		{
			if (id == 0)
			{
				_tools.getSQL_void(@"Insert into core_responsibilities
(core_responsibility,member_id,date_added,status,default_target,description,annually,quarterly,monthly,weekly,as_required,daily,cr_group_id) 
values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12)",
new object[] {
_core_responsibility, //0
_member_id, //1
_date_added.ToString("yyyy-MM-dd"), //2
_status, //3
_default_target, //4
_description, //5
_annually, //6
_quarterly, //7
_monthly, //8
_weekly, //9
_as_required, //10
_daily, //11
_cr_group_id //12
});
				_id = _tools.getSQL_int(@"Select id from applicants order by id desc limit 1" ,null);
			}
			else
			{
				_tools.getSQL_void(@"Update core_responsibilities
Set core_responsibilities=@v0  ,
member_id=@v1 ,
date_added=@v2, 
status=@v3 ,
default_target=@v4 ,
description=@v5  ,
annually =@v6 ,
quarterly = @v7 ,
monthly=@v8  ,
weekly = @v9 ,
daily = @v10 ,
as_required =@v11 ,
cr_group_id = @v12  
where id =@v13",
new object[] {
		_core_responsibility, //0
		_member_id, //1
		_date_added.ToString("yyyy-MM-dd"), //2
	_status,
	_default_target,
	_description,
	_annually,
	_quarterly,
	_monthly,
	_weekly,
	_daily,
	_as_required,
	_cr_group_id,
		_id});
			}
		}



	}
}