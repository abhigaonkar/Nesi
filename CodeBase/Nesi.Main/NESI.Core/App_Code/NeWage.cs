using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NeWage
	/// </summary>
	public class NeWage
	{
		/*
		MemberWage_ID
		memberwage_memberid
		Date
		CurrentWage
		CurrentWageType
		NextRaise
		Comment
		Member_ID_Audit
		Created_Date
		Modified_Date
		Active
		CompanyID
		MemberWage_Added_By
		 */
		public int id { get; set; }
		public int member_id { get; set; }
		public string date { get; set; }
		public double current_wage { get; set; }
		public string current_wage_type { get; set; }
		public string date_next_raise { get; set; }
		public string comment { get; set; }
		public int member_id_audit { get; set; }
		public string date_created { get; set; }
		public string date_modified { get; set; }
		public bool active { get; set; }
		public int business_unit_id { get; set; }
		public int member_id_added_by { get; set; }
		public int membertype_id { get; set; }
		public int bonus_type { get; set; }
		public string bonus_name { get; set; }
		public double bonus_amount { get; set; }


		// TODO: Change table structure to match proper table structure.
		public NeWage() { }

		public NeWage(string __id)
		{
			if (__id != "0")
			{
				get(Convert.ToInt32(__id));
			}
		}

		public NeWage(int _member_id)
		{
			var _id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(memberwage_id),0) FROM memberwage WHERE memberwage_memberid = @v0 AND active = 'true' LIMIT 1", _member_id);
			if (_id != 0)
			{
				get(_id);
			}
			else
			{
				member_id = _member_id;
			}
		}
		private bool exists(int _id)
		{
			var c = Toolbox.doSQL_int(@"SELECT COUNT(memberwage_id) FROM memberwage WHERE memberwage_id =@v0", _id);
			return c != 0;
		}
		public static double GetEmployeeWage(MySqlConnection _conn, int _member_id)
			{
			return Toolbox.doSQL_double(_conn, @"SELECT IFNULL(GET_WAGE(@v0), 0)", new object[]{_member_id});
			}
		private void get(int _id)
		{
			var dr = Toolbox.doSQL_dt(@"SELECT * FROM memberwage  WHERE memberwage_id =@v0", new object[] { _id }).Rows[0];
			id = _id;
			member_id = Convert.ToInt32(dr["memberwage_memberid"]);
			member_id_audit = Convert.ToInt32(dr["member_id_audit"]);
			date = Toolbox.MySQL_shortdt(Convert.ToDateTime(dr["date"]));
			current_wage = (double)dr["currentwage"];
			comment = dr["comment"].ToString();
			date_next_raise = Toolbox.MySQL_shortdt(Convert.ToDateTime(dr["nextraise"]));
			date_created = Toolbox.MySQL_shortdt(Convert.ToDateTime(dr["created_date"]));
			date_modified = dr["modified_date"] == DBNull.Value ? "" : Toolbox.MySQL_longdt(Convert.ToDateTime(dr["modified_date"]));
			member_id_added_by = Convert.ToInt32(dr["memberwage_added_by"]);
			business_unit_id = Convert.ToInt32(dr["business_unit_id"]);
			active = dr["active"].ToString() == "true";
			bonus_type = Convert.ToInt32(dr["bonus_type_id"]);
			bonus_amount = Convert.ToDouble(dr["bonus_amount"]);
			//	bonus_name = Toolbox.doSQL_string(@"Select ifnull(bonus_type,'Not Set') from bonus_type  where id =@v0", new object[] { bonus_type });


		}

		public void save() /* Update Wage Info in Employee Tab */
		{
			if (string.IsNullOrEmpty(date_next_raise))
			{
				date_next_raise = Toolbox.MySQL_longdt(DateTime.Now.AddYears(1));
			}
			if (id > 0)
			{
                //put(id);
                if (active)
                {
                    Toolbox.doSQL_void(@"update memberwage set memberwage.active = 'false' where  memberwage_id !=@v0 and memberwage_memberid =@v1 ", new object[] { id, member_id });
                }

                Toolbox.doSQL_void(@"
UPDATE 
	memberwage 
SET 
	date					= @v0,
	currentwage				= @v1, 
	comment					= @v2, 
	nextraise				= @v3, 
	memberwage_added_by		= @v4,
	modified_date			= NOW(),
	active					= @v6,
	bonus_type_id			= @v7,
	bonus_amount			= @v8,
    business_unit_id = @v9
WHERE 
	memberwage_id = @v5
LIMIT 1", new object[] {
					date,						  // {0}
					current_wage,				  // {1}
					comment,  // {2}
					date_next_raise,			  // {3}
					member_id_added_by,			  // {4}
					id,							  // {5}
					active? "true": "false",						  // {6}
					bonus_type,					  // {7}
					bonus_amount,				  // {8}
                    business_unit_id
				});
			}
			else
			{
				id = Toolbox.doSQL_return_id(@"
INSERT INTO memberwage 
	(
	memberwage_memberid, 
	date, 
	currentwage, 
	comment, 
	nextraise, 
	member_id_audit, 
	created_date,
	memberwage_added_by, 
	active,
	modified_date,
	bonus_type_id,
	bonus_amount,
	membertype_id,
business_unit_id
	)
VALUES 
	(
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v5,
	NOW(),
	@v6,
	'True',
	NOW(),
	@v8,
	@v9,
	@v10,
@v11
	)", new object[] {
					member_id,						// {0}
					date,							// {1}
					current_wage,					// {2}
					comment,	// {3}
					date_next_raise,				// {4}
					member_id_audit,				// {5}
					member_id_added_by,				// {6}
					"",						// {7}
					bonus_type,						// {8}
					bonus_amount,					// {9}
					membertype_id,					// {10}
                    business_unit_id
				});
				Toolbox.doSQL_void(@"update memberwage set memberwage.active = 'false' where  memberwage_id !=@v0 and memberwage_memberid =@v1 ", new object[] { id, member_id });
			}
		}
		public void delete() /* Delete Wage Info in Employee Tab */
		{
			if (exists(id))
			{
				Toolbox.doSQL_void(@"DELETE FROM memberwage WHERE memberwage_id = @v0 LIMIT 1", id);
			}
		}
	}
}