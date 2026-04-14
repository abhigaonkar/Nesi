using System;
using System.Data;
using nesi.core;

namespace nesi.core
{
	/// <summary>

	/// </summary>
	public class NeTrainingModule
	{
		public int id { get; set; }
		public bool is_loaded { get; set; }
		public string colour { get; set; }
		public string is_mandatory { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public DateTime date_create { get; set; }
		public bool is_active { get; set; }
		public bool is_certificate { get; set; }

		public NeTrainingModule() { }
		public NeTrainingModule(int _id)
		{
			if (exists(_id))
			{
				load(_id);
			}
		}
		private bool exists(int _id)
		{
			return Toolbox.doSQL_int("SELECT COUNT(id) FROM training_module WHERE id =@v0", _id) > 0;
		}
		private void load(int _id)
		{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM training_module  WHERE id =@v0", new object[] { _id });
			foreach (DataRow dr in dt.Rows)
			{
				id = (int)dr["id"];
				name = (string)dr["name"];
				description = (string)dr["description"];
				date_create = dr["date_create"] == DBNull.Value ? DateTime.Now : (DateTime)dr["date_create"];
				is_mandatory = (string)dr["is_mandatory"];
				is_active = (bool)dr["is_active"];
				colour = (string)dr["colour"];
				is_certificate = (bool)dr["is_certificate"];
				is_loaded = true;
			}
		}
		public void save()
		{
			if (id == 0)
			{
				id = Toolbox.doSQL_return_id(@"
INSERT INTO training_module
	(
	name,
	description,
	date_create,
	is_active,
	is_mandatory,
	colour,
	is_certificate	
	)
VALUES
	(
	@v0,
	@v1,
	NOW(),
	@v2,
	@v3,
	@v4,
	@v5
	)
", new object[] {
					name,			// {0}
					description,	// {1}
					is_active, 							// {2}
					is_mandatory, 						// {3}
					colour, 							// {4}
					is_certificate						// {5}
				});
			}
			else
			{
				Toolbox.doSQL_void(@"
UPDATE 
	training_module
SET
	name			= @v1,
	description		= @v2,
	is_active		= @v3,
	is_mandatory	= @v4,
	colour			= @v5,
	is_certificate	= @v6
WHERE
	id = @v0
LIMIT 1
", new object[] {
					id,									// {0}
					name,			// {1}
					description,	// {2}
					is_active, 							// {3}
					is_mandatory, 						// {4}
					colour, 							// {5}
					is_certificate						// {6}
				});
			}
		}
		private bool is_linked(int _id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM training_curriculum WHERE module_id =@v0", _id) > 0;
		}
		public bool delete(int _id)
		{
			if (exists(_id))
			{
				if (is_linked(_id))
				{
					return false;
				}
				else
				{
					Toolbox.doSQL_void(@"DELETE FROM training_module WHERE id = @v0 LIMIT 1", _id);
					return true;
				}
			}
			else
			{
				return false;
			}
		}
	}
}

public class NETrainingHistory
{
	public int id { get; set; }
	public bool is_loaded { get; set; }
	public int module_id { get; set; }
	public int member_id { get; set; }
	public string teacher_name { get; set; }
	public DateTime date { get; set; }
	public DateTime date_expires { get; set; }
	public string comments { get; set; }
	public int business_unit_id { get; set; }

	public NETrainingHistory() { }

	public NETrainingHistory(int _id)
	{
		if (exists(_id))
		{
			load(_id);
		}
	}
	private bool exists(int _id)
	{
		return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM training_history WHERE id = @v1", _id) > 0;
	}
	private void load(int _id)
	{
		var dt = Toolbox.doSQL_dt(@"SELECT * FROM training_history  WHERE id =@v0", new object[] { _id });
		foreach (DataRow dr in dt.Rows)
		{
			id = (int)dr["id"];
			module_id = (int)dr["module_id"];
			member_id = Convert.ToInt32(dr["member_id"]);
			teacher_name = (string)dr["teacher_name"];
			date = dr["date"] == DBNull.Value ? DateTime.Now : (DateTime)dr["date"];
			date_expires = dr["date_expires"] == DBNull.Value ? DateTime.Now : (DateTime)dr["date_expires"];
			comments = (string)dr["comments"];
			business_unit_id = (int)dr["business_unit_id"];
			is_loaded = true;
		}
	}
	public void save()
	{
		if (id == 0)
		{
			id = Toolbox.doSQL_return_id(@"
INSERT INTO training_history
	(
	module_id,
	member_id,	
	teacher_name,
	date,
	date_expires,
	comments,	
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
	@v6
	)
", new object[] {
			module_id,                              // {0}
			member_id,                              // {1}
			teacher_name,       // {2}
			Toolbox.MySQL_shortdt(date),            // {3}
			Toolbox.MySQL_shortdt(date_expires),    // {4}
			comments,           // {5}
			business_unit_id                                // {6}
			});
		}
		else
		{
			Toolbox.doSQL_void(@"
UPDATE 
	training_history
SET
	module_id			= @v1,
	member_id			= @v2,	
	teacher_name		= @v3,
	date				= @v4,
	date_expires		= @v5,
	comments			= @v6,	
	business_unit_id			= @v7
WHERE
	id = @v0
LIMIT 1
", new object[] {
			id,                                     // {0}
			module_id,                              // {1}
			member_id,                              // {2}
			teacher_name,       // {3}
			Toolbox.MySQL_shortdt(date),            // {4}
			Toolbox.MySQL_shortdt(date_expires),    // {5}
			comments,           // {6}
			business_unit_id                                // {7}
			});
		}
	}
	public bool delete(int _id)
	{
		if (exists(_id))
		{
			Toolbox.doSQL_void(@"DELETE FROM training_history WHERE id = @v0 LIMIT 1", _id);
			return true;
		}
		else
		{
			return false;
		}
	}
}


public class NETrainingCurriculum
{
	public int id { get; set; }
	public bool is_loaded { get; set; }
	public int day { get; set; }
	public int type_id { get; set; }
	public int year { get; set; }
	public int module_id { get; set; }
	public string description { get; set; }
	public bool paid { get; set; }

	public NETrainingCurriculum() { }

	public NETrainingCurriculum(int _id)
	{
		if (exists(_id))
		{
			load(_id);
		}
	}
	private bool exists(int _id)
	{
		return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM training_curriculum WHERE id =@v0",_id) > 0;
	}
	private void load(int _id)
	{
		var dt = Toolbox.doSQL_dt(@"SELECT * FROM training_curriculum  WHERE id =@v0", new object[] { _id });
		foreach (DataRow dr in dt.Rows)
		{
			id = (int)dr["id"];
			type_id = (int)dr["type_id"];
			day = (int)dr["day"];
			year = (int)dr["year"];
			module_id = (int)dr["module_id"];
			description = (string)dr["description"];
			paid = (bool)dr["paid"];
			is_loaded = true;
		}
	}
	public void save()
	{
		if (id == 0)
		{
			id = Toolbox.doSQL_return_id(@"
INSERT INTO training_curriculum
	(
	day,
	year,
	module_id,
	description,
	type_id,
	paid
	)
VALUES
	(
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v5
	)
", new object[] {
			day,                                    // {0}
			year,                                   // {1}
			module_id,                              // {2}
			description,        // {3}
			type_id,                                // {4}
			paid                                    // {5}
			});
		}
		else
		{
			Toolbox.doSQL_void(@"
UPDATE 
	training_curriculum
SET
	day				= @v1,
	year			= @v2,
	module_id		= @v3,
	description		= @v4,
	type_id			= @v5,
	paid			= @v6
WHERE
	id = @v0
LIMIT 1
", new object[] {
			id,                                     // {0}
			day,                                    // {1}
			year,                                   // {2}
			module_id,                              // {3}
			description,        // {4}
			type_id,                                // {5}
			paid                                    // {6}
			});
		}
	}
	private bool is_linked(int _id)
	{
		return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM training_schedule WHERE curriculum_id =@v0", _id) > 0;
	}
	public bool delete(int _id)
	{
		if (exists(_id))
		{
			if (is_linked(_id))
			{
				return false;
			}
			else
			{
				Toolbox.doSQL_void(@"DELETE FROM training_curriculum WHERE id = @v0 LIMIT 1", _id);
				return true;
			}
		}
		else
		{
			return false;
		}
	}
}

public class NETrainingSchedule
{
	public int id { get; set; }
	public DateTime date_start { get; set; }
	public DateTime date_end { get; set; }
	public int trainer_id { get; set; }
	public int trainee_id { get; set; }
	public int curriculum_id { get; set; }
	public string notes { get; set; }
	public NETrainingSchedule() { }

	public NETrainingSchedule(int _id)
	{
		if (exists(_id))
		{
			load(_id);
		}
	}
	private bool exists(int _id)
	{
		return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM training_schedule WHERE id =@v0", _id) > 0;
	}
	private void load(int _id)
	{
		var dt = Toolbox.doSQL_dt(@"SELECT * FROM training_schedule  WHERE id =@v0", new object[] { _id });
		foreach (DataRow dr in dt.Rows)
		{
			id = (int)dr["id"];
			date_start = dr["date_start"] == DBNull.Value ? DateTime.Now : (DateTime)dr["date_start"];
			date_end = dr["date_end"] == DBNull.Value ? DateTime.Now : (DateTime)dr["date_end"];
			trainer_id = (int)dr["trainer_id"];
			trainee_id = (int)dr["trainee_id"];
			curriculum_id = (int)dr["curriculum_id"];
			notes = (string)dr["notes"];
		}
	}
	public void save()
	{
		if (id == 0)
		{
			id = Toolbox.doSQL_return_id(@"
INSERT INTO training_schedule
	(
	date_start,
	date_end,
	trainer_id,
	trainee_id,
	curriculum_id,
	notes
	)
VALUES
	(
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v5
	)
", new object[] {
			Toolbox.MySQL_shortdt(date_start),      // {0}
			Toolbox.MySQL_shortdt(date_end),        // {1}
			trainer_id,                             // {2}
			trainee_id,                             // {3}
			curriculum_id,                          // {4}
			notes               // {5}
			});
		}
		else
		{
			Toolbox.doSQL_void(@"
UPDATE 
	training_schedule
SET
	date_start		= @v1,
	date_end		= @v2,
	trainer_id		= @v3,
	trainee_id		= @v4,
	curriculum_id	= @v5,
	notes			= @v6	
WHERE
	id = @v0
LIMIT 1
", new object[] {
			id,                                     // {0}
			Toolbox.MySQL_shortdt(date_start),      // {1}
			Toolbox.MySQL_shortdt(date_end),        // {2}
			trainer_id,                             // {3}
			trainee_id,                             // {4}
			curriculum_id,                          // {5}
			notes               // {6}
			});
		}
	}
	public bool delete(int _id)
	{
		if (exists(_id))
		{
			Toolbox.doSQL_void(@"DELETE FROM training_schedule WHERE id = @v0 LIMIT 1", _id);
			return true;
		}
		else
		{
			return false;
		}
	}
}
public class faq
{
	public int id { get; set; }
	public int section_id { get; set; }
	public string body { get; set; }
	public string subject { get; set; }
	public bool customer_visible { get; set; }
	public faq() { }

	public faq(int _id)
	{
		if (exists(_id))
		{
			load(_id);
		}
	}
	private bool exists(int _id)
	{
		return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM faq WHERE id = @v0" , _id ) > 0;
	}
	private void load(int _id)
	{
		var dt = Toolbox.doSQL_dt(@"SELECT * FROM faq  WHERE id =@v0", new object[] { _id });
		foreach (DataRow dr in dt.Rows)
		{
			id = (int)dr["id"];
			section_id = (int)dr["section_id"];
			subject = (string)dr["subject"];
			body = (string)dr["body"];
		}
	}
	public void save()
	{
		if (id == 0)
		{
			id = Toolbox.doSQL_return_id(@"
INSERT INTO faq
	(
	section_id,
	subject,
	body,
	customer_visible
	)
VALUES
	(
	@v0,
	@v1,
	@v2,
	@v3
	)
", new object[] {
			section_id,                     // {0}
			subject,    // {1}
			body,       // {2}
			customer_visible                // {3}
			});
		}
		else
		{
			Toolbox.doSQL_void(@"
UPDATE 
	faq
SET
	section_id			= @v1,
	subject				= @v2,
	body				= @v3,
	customer_visible	= @v4
WHERE
	id = @v0
LIMIT 1
", new object[] {
			id,                                     // {0}
			section_id,                             // {1}
			subject,            // {2}
			body,               // {3}
			customer_visible                        // {4}
			});
		}
	}
	public bool delete(int _id)
	{
		if (exists(_id))
		{
			Toolbox.doSQL_void(@"DELETE FROM faq WHERE id = @v0 LIMIT 1", _id);
			return true;
		}
		else
		{
			return false;
		}
	}
}

public class faq_section
{
	public int id { get; set; }
	public string name { get; set; }
	public faq_section() { }

	public faq_section(int _id)
	{
		if (exists(_id))
		{
			load(_id);
		}
	}
	private bool exists(int _id)
	{
		return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM faq_section WHERE id =@v0",_id) > 0;
	}
	private void load(int _id)
	{
		var dt = Toolbox.doSQL_dt(@"SELECT * FROM faq_section  WHERE id =@v0", new object[] { _id });
		foreach (DataRow dr in dt.Rows)
		{
			id = (int)dr["id"];
			name = (string)dr["name"];
		}
	}
	public void save()
	{
		if (id == 0)
		{
			id = Toolbox.doSQL_return_id(@"
INSERT INTO faq_section
	(
	name
	)
VALUES
	(
	@v0
	)
", new object[] {
			name        // {0}
			});
		}
		else
		{
			Toolbox.doSQL_void(@"
UPDATE 
	faq_section
SET
	name = @v1
WHERE
	id = @v0
LIMIT 1
", new object[] {
			id,                                     // {0}
			name                // {1}
			});
		}
	}
	public bool delete(int _id)
	{
		if (exists(_id))
		{
			var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM faq WHERE section_id = @v0", _id);
			if (c > 0)
			{
				throw new Exception("You cannot delete a section that has articles attached to it.");
			}
			Toolbox.doSQL_void(@"DELETE FROM faq_section WHERE id = @v0 LIMIT 1", _id);
			return true;
		}
		else
		{
			return false;
		}
	}
}