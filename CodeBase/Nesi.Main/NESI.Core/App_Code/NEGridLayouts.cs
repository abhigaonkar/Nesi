using System;
using System.Data;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NEContact
	/// </summary>
	public class NeGridLayouts
	{
		private int _id = 0;
		private string _layout = "";
		private string _grid_id = "";
		private string _name = "not named";
        private bool _is_default;

		public int GridLayoutID { get { return _id; } set { _id = value; } }
		public string GridLayout_Layout { get { return _layout; } set { _layout = value; } }
		public string GridLayout_Gridid { get { return _grid_id; } set { _grid_id = value; } }
        public bool is_default { get { return _is_default; } set { _is_default = value; } }
		public int member_id { get; set; }
		public string GridLayout_Name { get { return _name; } set { _name = value; } }

		public NeGridLayouts()
		{
		}

		public NeGridLayouts(int memberid, string gridid)
		{
			load(memberid, gridid);

		}
		private void load(int memberid, string gridid)
		{
			var c = Toolbox.doSQL_int(@"SELECT COUNT(gridviewlayouts_id) FROM gridviewlayouts WHERE gridviewlayouts_member_id = @v0  AND gridviewlayouts_gridid = @v1 ", new object[] {  memberid, gridid } );
			if (c > 0)
			{
				var dr = Toolbox.doSQL_dt(@" SELECT gridviewlayouts_id id, gridviewlayouts_member_id member_id, gridviewlayouts_gridid grid, gridviewlayout_layout layout, gridviewlayouts_name name, is_default FROM gridviewlayouts WHERE gridviewlayouts_member_id = @v0  AND gridviewlayouts_gridid = @v1  ORDER BY is_default DESC,gridviewlayouts_ts DESC LIMIT 1", new object[] {  memberid, gridid } ).Rows[0];
				_id = Convert.ToInt32(dr["id"]);
				_layout = dr["layout"].ToString().Replace("~", "'");
				_grid_id = dr["grid"].ToString();
				member_id = Convert.ToInt32(dr["member_id"]);
                _is_default = Convert.ToBoolean(dr["is_default"]);
				_name = dr["name"].ToString();
			}
		}
		public NeGridLayouts(int id)
		{
			var c = Toolbox.doSQL_int(@"SELECT COUNT(gridviewlayouts_id) FROM gridviewlayouts WHERE gridviewlayouts_id = @v0 ", new object[] {  id } );
			if (c > 0)
			{
				var dr = Toolbox.doSQL_dt(@" SELECT gridviewlayouts_member_id member_id, gridviewlayouts_gridid grid, gridviewlayout_layout layout, gridviewlayouts_name name, is_default FROM gridviewlayouts WHERE gridviewlayouts_id = @v0 ", new object[] {  id } ).Rows[0];
				_id = id;
				_layout = dr["layout"].ToString().Replace("~", "'");
				_grid_id = dr["grid"].ToString();
				member_id = Convert.ToInt32(dr["member_id"]);
                _is_default = Convert.ToBoolean(dr["is_default"]);
                _name = dr["name"].ToString();
			}
		}
		public static void HandleDefaultLayout(int layoutId, int memberId, string gridId)
			{
			Toolbox.doSQL_void(@"UPDATE gridviewlayouts SET is_default = gridviewlayouts_id = @v0 WHERE gridviewlayouts_member_id = @v1 AND GridviewLayouts_GridID = @v2", new object[]{layoutId, memberId, gridId });
			}
		public static string ExistingIncorrectLayoutIds(string badId)
			{
			return Toolbox.doSQL_string(@"SELECT IFNULL(GROUP_CONCAT(gridviewlayouts_id), '') FROM gridviewlayouts WHERE gridviewlayouts_gridid = @v0", new [] {badId});
			}
		public static void CorrectExistingLayoutGridIds(string layoutIds, string correctGridId)
			{
			Toolbox.doSQL_void(@"	UPDATE gridviewlayouts 
									SET 
										gridviewlayouts_gridid = @v0, 
										gridviewlayouts_ts = gridviewlayouts_ts 
									WHERE 
										FIND_IN_SET(gridviewlayouts_id, @v1)", new object[] { correctGridId, layoutIds });
			}
		public void SaveGridLayout()
		{
			if (_id == 0)
			{
				#region INSERT
				var temp_id = Toolbox.doSQL_return_id(@"
INSERT INTO gridviewlayouts
	(
	gridviewlayouts_member_id, 
	gridviewlayouts_gridid, 
	gridviewlayout_layout, 
	gridviewlayouts_name,
is_default
	) 
VALUES (@v0,@v1,@v2,@v3,@v4)",
	new object[] {              member_id,
					_grid_id,
					_layout,
					_name,
                    _is_default?1:0
    });
				_id = Convert.ToInt32(temp_id)
                
                ;
				#endregion INSERT
			}
			else
			{
				#region UPDATE
				Toolbox.doSQL_void(@"
UPDATE 
	gridviewlayouts 
SET 
	gridviewlayouts_member_id = @v0, 
	gridviewlayouts_gridid = @v1, 
	gridviewlayout_layout = @v2, 
	gridviewlayouts_name = @v3,
is_default = @v5
WHERE 
	gridviewlayouts_id = @v4
LIMIT 1", new object[] {
					member_id,
					_grid_id,
					_layout,
					_name,
					_id,
                    _is_default?1:0
                });
				#endregion UPDATE
			}
		}
	}
}