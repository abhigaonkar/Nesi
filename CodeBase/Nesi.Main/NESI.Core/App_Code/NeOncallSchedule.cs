using System;
using System.Data;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NEContact
	/// </summary>
	public class NeOncallSchedule
		{
		public int id { get; set; }
		public int member_id { get; set; }
		public int business_unit_id { get; set; }
		public DateTime date { get; set; }
		public DateTime last_modified { get; set; }
		public int added_by { get; set; }
		public string notes { get; set; }
		public bool is_backup { get; set; }

		public NeOncallSchedule(){}

		public NeOncallSchedule(int _id)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM oncall_schedule WHERE id = @v0 ", new object[] {  _id } );
			foreach (DataRow dr in dt.Rows)
				{
				id				= _id;
				member_id		= Convert.ToInt32(dr["member_id"]);
				business_unit_id		= Convert.ToInt32(dr["business_unit_id"]);
				notes			= dr["notes"].ToString();
				added_by		= Convert.ToInt32(dr["added_by"]);
				date			= Convert.ToDateTime(dr["date"]);
				last_modified	= Convert.ToDateTime(dr["last_modified"]);
				is_backup		= Convert.ToBoolean(dr["is_backup"]);
				}
			}

		public void save()
			{
			if (id == 0)
				{
				if (member_id == 0)
					{
					throw new Exception("You must select a person");
					}
				if (business_unit_id == 0)
					{
					throw new Exception("You must select a branch");
					}
				if (date.Year < 2000)
					{
					throw new Exception("You must select a date");
					}
				if (Toolbox.doSQL_int(@"Select count(id) from oncall_schedule
where business_unit_id = @v0 and date =@v1 and is_backup =@v2", new object[] { business_unit_id, date.ToString("yyyy-MM-dd"),(is_backup?1:0)}) > 0)
					{
					Toolbox.doSQL_void(@"Delete from oncall_schedule where business_unit_id = @v0 and date =@v1 and is_backup =@v2",new object[]
						{ business_unit_id, date.ToString("yyyy-MM-dd"), (is_backup ? 1 : 0)});
					}
				id=Toolbox.doSQL_return_id(@"Insert into oncall_schedule 
(member_id,date,notes, business_unit_id, added_by, is_backup) 
values(@v0,@v1,@v2,@v3,@v4,@v5)",new object[] { member_id, date.ToString("yyyy-MM-dd"), notes, business_unit_id, added_by, is_backup});

				}
			else
				{

				}


			}


		}
	}