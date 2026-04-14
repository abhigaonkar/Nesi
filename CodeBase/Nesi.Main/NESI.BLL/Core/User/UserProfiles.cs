using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.CurrentUser.Layout;

namespace NESI.BLL.Core.User
{
	public partial class User
	{

		/// <summary>
		/// get layout profiles
		/// </summary>
		/// <returns></returns>
		public Profile[] GetLayoutProfiles()
		{
			var db = new NESIMySQL();
			var profiles = new List<Profile>();
			var list = (from l in db.n2_user_profile
					where l.user_id == this.Id
					      && l.n2_profile_property.n2_profile_category.profile_category_name.ToLower() == "layout"
					select l
				).ToList<n2_user_profile>();
			var properties = db.n2_profile_property
				.Where(m => m.n2_profile_category.profile_category_name.ToLower() == "layout").ToList();
			foreach (var prop in properties)
			{
				var userProf = list.FirstOrDefault(m => m.property_id == prop.id);
				var profile = new Profile()
				{
					Id = 0,
					UserId = this.Id,
					ProfileId = prop.id,
					Name = prop.property_name,
					Value = prop.property_defaullt,
					Descrtiption = prop.property_description
				};
				if (userProf != null)
				{
					profile.Id = userProf.id;
					profile.Value = userProf.property_value;

				}
				profiles.Add(profile);
			}
			return profiles.ToArray();
		}

		/// <summary>
		/// Save profiles to database
		/// </summary>
		/// <param name="profiles"></param>
		/// <returns></returns>
		public bool SaveProfiles(Profile[] profiles)
		{
			if (profiles == null) return false;
			var db = new NESIMySQL();

			foreach (var prop in profiles)
			{
				if (prop.UserId != Id) continue;
				var up = db.n2_user_profile.FirstOrDefault(m => m.property_id == prop.ProfileId && m.user_id == Id);

				var userProfile = new Data.Entities.n2_user_profile()
				{
					id = up?.id ?? prop.Id,
					user_id = Id,
					property_id = prop.ProfileId,
					property_value = prop.Value
				};

				db.n2_user_profile.AddOrUpdate(userProfile);
			}
			try
			{
				db.SaveChanges();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

	}
}