using System;
using System.Data.Entity.Migrations;
using System.Linq;
using NESI.BLL.Base;
using NESI.Data.Entities;

// ReSharper disable InconsistentNaming

namespace NESI.BLL.Core
{

	/// <summary>
	/// translate from App_Code\NeEMail.cs
	/// </summary>
	public class Ne2Passport : BLLBase
	{
		public class array_object
		{
			public string url_yes { get; set; }
			public string url_no { get; set; }
			public string text_yes { get; set; }
			public string text_no { get; set; }
			public string reqhash { get; set; }
			public bool process_all { get; set; }
		}

		public DTO.Models.Core.Passport Entity;
		private readonly string _reqhash;


		public Ne2Passport()
		{
		}

		public Ne2Passport(string _hash) : base()
		{
			_reqhash = _hash;
			Entity = Get(_hash);
		}

		private DTO.Models.Core.Passport Get(string hash)
		{
			return AutoMapper.Mapper.Map<DTO.Models.Core.Passport>(_db.passport.FirstOrDefault(x => x.reqhash == hash));
		}

		public bool Check(string hash)
		{
			return _db.passport.Count(x => x.reqhash == hash && x.active == true) > 0;
		}

		public void Save(Data.Entities.passport entity)
		{
			var p_expiry = entity.expiry_date == null || (entity.expiry_date != null && entity.expiry_date.Trim() == "")
				? "NULL"
				: entity.expiry_date;
			_db.Database.ExecuteSqlCommand(@"
INSERT INTO passport
	(
	`dt`,
	`to`, 
	`from`, 
	`active`, 
	`reqhash`,
	`valid`,
	`url_yes`, 
	`url_no`,
	`to_member_id`,
	`expiry_date`,
    `yes_text`,
    `no_text`
	) 
VALUES 
	(
	NOW(),
	@p0, 
	@p1, 
	true, 
	@p2,
	-1,
	@p3, 
	@p4,
	@p5,
	@p6,
    @p7,
    @p8
	)",

				entity.to, // {0}
				entity.from, // {1}
				entity.reqhash, // {2} 
				entity.url_yes, // {3}
				entity.url_no, // {4}
				entity.to_member_id, // {5}
					p_expiry, // {6}
				entity.yes_text,
				entity.no_text
				);
		}

		public string Process(bool which)
		{
			{
				if (string.IsNullOrEmpty(_reqhash)) { throw new Exception("Invalid Request"); }
				var column = which ? "url_yes" : "url_no";

				if ((Entity.expiry_date == null) || (Convert.ToDateTime(Entity.expiry_date) < DateTime.Today))
				{
					_db.Database.ExecuteSqlCommand(@"UPDATE passport SET valid = @p0, active = false WHERE reqhash = @p1 LIMIT 1", new object[] { which, _reqhash });
				}

				return _db.Database.SqlQuery<string>($@"SELECT {column} FROM passport WHERE reqhash = @p0  LIMIT 1", Entity.reqhash).FirstOrDefault();
			}
		}
	}
}
