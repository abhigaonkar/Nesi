using System;
using System.Data;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for stock_transfer_history
	/// </summary>
	public class NeTask	
		{
		Toolbox _tools = new Toolbox();
		#region privates
		private int _id;
		private string _name;
		private int _owner_int;
		private string _owner;
		private DateTime _date_started;
		private int _percent_complete;
		private int _status_int;
		private string _status;
		private int _cutby_int;
		private string _cutby;
		private string _notes="";
		private int _parent_task;
		private DateTime _date_due;
		private bool _recurring;
		private int _recurring_frequency;
		private bool _isprivate;
		private int _notify_warning_days;
		private bool _emails;
		private bool _messageboard;
		private bool _text;
		private string _recurring_resolution="";
		private int _priority;
		private int _dept;
		private int _expectedhours;

		#endregion privates
		#region publics

	
		public int id {get {return _id;} set {_id = value;}	}
		public string name { get { return _name; } set { _name = value; } }
		public int owner_int { get { return _owner_int; } set { _owner_int = value; } }
		public string owner { get { return _owner; } set { _owner = value; } }
		public DateTime date_started { get { return _date_started; } set { _date_started = value; } }
		public int percent_complete { get { return _percent_complete; } set { _percent_complete = value; } }
		public int status_int { get { return _status_int; } set { _status_int = value; } }
		public string status { get { return _status; } set { _status = value; } }
		public int cutby_int { get { return _cutby_int; } set { _cutby_int = value; } }
		public string cutby { get { return _cutby; } set { _cutby = value; } }
		public string notes { get { return _notes; } set { _notes = value; } }
		public int parent_task { get { return _parent_task; } set { _parent_task = value; } }
		public DateTime date_due { get { return _date_due; } set { _date_due = value; } }
		public bool recurring { get { return _recurring; } set { _recurring = value; } }
		public int recurring_frequency { get { return _recurring_frequency; } set { _recurring_frequency = value; } }
		public bool isprivate { get { return _isprivate; } set { _isprivate = value; } }
		public int notify_warning_days { get { return _notify_warning_days; } set { _notify_warning_days = value; } }
		public bool emails { get { return _emails; } set { _emails = value; } }
		public bool messageboard { get { return _messageboard; } set { _messageboard = value; } }
		public bool text { get { return _text; } set { _text = value; } }
		public string recurring_resolution { get { return _recurring_resolution; } set { _recurring_resolution = value; } }
		public int priority {get { return _priority;} set { _priority=value;}}
		public int dept {get { return _dept;} set { _dept=value;}}
		public int expectedhours { get {return _expectedhours;} set { _expectedhours=value;}}

		#endregion publics

		#region functions and stuff
		public NeTask(int id)
			{
			DataTable dt;
			try { dt= Toolbox.doSQL_dt(@"Select *,get_name(owner) as o, get_name(cutby) as c from task  where id =@v0", new object[] { id }); }
			catch { throw new Exception("Task Not Found"); }

			_id = Convert.ToInt32(dt.Rows[0]["id"]);
			_name = _tools.value_from(dt.Rows[0]["name"].ToString());
			_owner_int = Convert.ToInt32(dt.Rows[0]["owner"]);
			_owner = dt.Rows[0]["o"].ToString();
			_date_started = Convert.ToDateTime(dt.Rows[0]["date_started"]);
			_percent_complete = Convert.ToInt32(dt.Rows[0]["percent_complete"]);
			_status_int =  Convert.ToInt32(dt.Rows[0]["status"]);
			_status = dt.Rows[0]["status"].ToString();
			_cutby_int = Convert.ToInt32(dt.Rows[0]["cutby"]);
			_cutby = dt.Rows[0]["c"].ToString();
			_notes = _tools.value_from(dt.Rows[0]["notes"].ToString());
			_parent_task = Convert.ToInt32(dt.Rows[0]["parent_task"]);
			_date_due=Convert.ToDateTime(dt.Rows[0]["date_due"]);
			_recurring = Convert.ToBoolean(dt.Rows[0]["recurring"]);
			_recurring_frequency = Convert.ToInt32(dt.Rows[0]["recurring_frequency"]);
			_isprivate = Convert.ToBoolean(dt.Rows[0]["private"]);
			_notify_warning_days = Convert.ToInt32(dt.Rows[0]["notify_warning_days"]);
			_emails = Convert.ToBoolean(dt.Rows[0]["emails"]);
			_messageboard =Convert.ToBoolean(dt.Rows[0]["messageboard"]);
			_text = Convert.ToBoolean(dt.Rows[0]["text"]);
			_recurring_resolution = (dt.Rows[0]["recurring_resolution"].ToString());
			_priority = Convert.ToInt32(dt.Rows[0]["priority"]);
			_dept = Convert.ToInt32(dt.Rows[0]["dept"]);
			_expectedhours = Convert.ToInt32(dt.Rows[0]["expectedhours"]);

			}
		public void save()
			{
			if (id == 0)
				{
				try
				{
					var sql = @"Insert into task (
				name,
				owner,
				date_started,
				percent_complete,
				status,
				cutby,
				parent_task,
				private,
				date_due,
				recurring,
				recurring_frequency,
				notify_warning_days,
				emails,
				messageboard,
				text,
				recurring_resolution,
				priority,
				dept,
				expectedhours,
				notes)
				values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19)";
					var paramObjects =
						new object[]
						{
							_tools.value_to(_name), //0
							_owner_int, //1
							date_started.ToString("yyyy-MM-dd"), //2
							percent_complete, //3
							status_int, //4
							cutby_int, //5
							parent_task, //6
							Convert.ToInt16(_isprivate), //7
							_date_due.ToString("yyyy-MM-dd"), //8
							Convert.ToInt16(_recurring), //9
							Convert.ToInt32(_recurring_frequency), //10
							Convert.ToInt32(_notify_warning_days), //11
							Convert.ToInt16(_emails), //12
							Convert.ToInt16(_messageboard), //13
							Convert.ToInt16(_text), //14
							_recurring_resolution.ToString(), //15
							Convert.ToInt16(_priority), //16
							Convert.ToInt16(_dept), //17
							Convert.ToInt32(_expectedhours), //18
							_tools.value_to(_notes) //19
						};
				_id=_tools.getSQL_return_id(sql, paramObjects);
				//_id= _tools.getSQL_int(@"Select id from task order by id desc limit 1"  , null);
					}
				catch
					{
					throw new Exception("Could not Add task");
					}
				}
			else
				{
				try
				{
					var sql = @"Update task set 
			name=@v0,
			owner=@v1,
			percent_complete=@v2,
			status=@v3,
			parent_task=@v4,
			private=@v5,
			date_due=@v6,
			recurring=@v7,
			recurring_frequency=@v8,
			notify_warning_days=@v9,
			emails=@v10,
			messageboard=@v11,
			text=@v12,
			recurring_resolution=@v13,
			priority=@v14,
			dept=@v15,
			expectedhours=@v16,
            notes=@v17
			where id=@v18";

						var paramObjects = new object[]
						{
							_tools.value_to(_name), //0
							_owner_int,//1
							percent_complete,//2
							status_int, //3
							parent_task,//4
							Convert.ToInt16(_isprivate),//5
							_date_due.ToString("yyyy-MM-dd"),//6
							Convert.ToInt16(_recurring),//7
							Convert.ToInt32(_recurring_frequency),//8
							Convert.ToInt32(_notify_warning_days),//9
							Convert.ToInt16(_emails),//10
							Convert.ToInt16(_messageboard),//11
							Convert.ToInt16(_text),//12
							_recurring_resolution.ToString(),//13
							Convert.ToInt16(_priority),//14
							Convert.ToInt16(_dept),//15
							Convert.ToInt32(_expectedhours),//16
							_tools.value_to(_notes),//17
							_id//18
						};
					_tools.getSQL_void(sql, paramObjects);
					}
				catch
					{
					throw new Exception("Couldn't Update Task");
					}


				}

			}


		public NeTask()
			{
			//
			//  
			//
			}
		#endregion
		}
	}