using System;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NEContact
	/// </summary>
	public class NeWoProgNotes
	{
		private int _woprog_project_notes_id;
		private int _woprog_project_notes_woprogid;
		private int _woprog_project_notes_memberid;
		private string _woprog_project_notes_notes;
		private string _woprog_project_notes_Datetime;
		private string _woprog_project_notes_Type;
		private Toolbox _tools = new Toolbox();

		public int woprog_project_notes_id { get { return _woprog_project_notes_id; } set { _woprog_project_notes_id = value; } }
		public int woprog_project_notes_woprogid { get { return _woprog_project_notes_woprogid; } set { _woprog_project_notes_woprogid = value; } }
		public int woprog_project_notes_memberid { get { return _woprog_project_notes_memberid; } set { _woprog_project_notes_memberid = value; } }
		public string woprog_project_notes_notes { get { return _woprog_project_notes_notes; } set { _woprog_project_notes_notes = value; } }
		public string woprog_project_notes_Datetime { get { return _woprog_project_notes_Datetime; } set { _woprog_project_notes_Datetime = value; } }
		public string woprog_project_notes_Type { get { return _woprog_project_notes_Type; } set { _woprog_project_notes_Type = value; } }

		public int id { get { return _woprog_project_notes_id; } set { _woprog_project_notes_id = value; } }
		public int woprogid { get { return _woprog_project_notes_woprogid; } set { _woprog_project_notes_woprogid = value; } }
		public int memberid { get { return _woprog_project_notes_memberid; } set { _woprog_project_notes_memberid = value; } }
		public string notes { get { return _woprog_project_notes_notes; } set { _woprog_project_notes_notes = value; } }
		public string Datetime { get { return _woprog_project_notes_Datetime; } set { _woprog_project_notes_Datetime = value; } }
		public string Type { get { return _woprog_project_notes_Type; } set { _woprog_project_notes_Type = value; } }


		public NeWoProgNotes(int _woprogid, string _type)
		{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM woprog_project_notes WHERE woprog_project_notes_woprogid = @v0  and woprog_project_notes_type = @v1 ",
				new object[] { _woprogid, _type });
			if (dt.Rows.Count != 1) return;
			var dr = dt.Rows[0];
			_woprog_project_notes_id = Convert.ToInt32(dr["woprog_project_notes_id"]);
			_woprog_project_notes_woprogid = Convert.ToInt32(dr["woprog_project_notes_woprogid"]);
			_woprog_project_notes_memberid = Convert.ToInt32(dr["woprog_project_notes_memberid"]);
			_woprog_project_notes_notes = Convert.ToString(dr["woprog_project_notes_notes"]);
			_woprog_project_notes_Datetime = Convert.ToString(dr["woprog_project_notes_datetime"]);
			_woprog_project_notes_Type = Convert.ToString(dr["woprog_project_notes_type"]);
		}


		public void SaveWOProgProjectNote()
		{
			var rec_id = _tools.getSQL_int(@"SELECT IFNULL(MAX(woprog_project_notes_id), 0) FROM woprog_project_notes
WHERE woprog_project_notes_woprogid = @v0  AND woprog_project_notes_type = @v1 ",
				new object[] { _woprog_project_notes_woprogid, _woprog_project_notes_Type });
			if (rec_id == 0)
			{
				_tools.getSQL_void(@"
INSERT INTO woprog_project_notes 
	(
	woprog_project_notes_woprogid,
	woprog_project_notes_memberid,
	woprog_project_notes_notes,
	woprog_project_notes_type
	) 
VALUES (@v0,@v1,@v2,@v3)",
	new object[] {
					_woprog_project_notes_woprogid,
					_woprog_project_notes_memberid,
					_woprog_project_notes_notes,
					_woprog_project_notes_Type
				});
			}
			else
			{
				_tools.getSQL_void(@"
UPDATE 
	woprog_project_notes 
SET 
	woprog_project_notes_memberid =@v0,
	woprog_project_notes_type = @v1,
	woprog_project_notes_notes = @v2
WHERE 
	woprog_project_notes_id = @v3
LIMIT 1", new object[] {
					_woprog_project_notes_memberid,
					_woprog_project_notes_Type,
					_woprog_project_notes_notes,
					rec_id
				});
			}
		}
	}
}