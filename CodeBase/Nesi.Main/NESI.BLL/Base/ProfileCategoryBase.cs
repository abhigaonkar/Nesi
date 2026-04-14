using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using NESI.BLL.Common.Shared;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Base
{
	public abstract class ProfileCategoryBase : BLLBase
	{
		protected string _categoryName;
		protected int _maxCount;

		public int Count { get; set; }

		protected ProfileCategoryBase(Employee user, string name, int max) : base(user)
		{
			_categoryName = name;
			_maxCount = max;

		}

		public LabelValueInt[] GetValues(string query)
		{
			query = query.Trim();
			return (from u in _db.n2_user_profile
					where u.n2_profile_property.n2_profile_category.profile_category_name == _categoryName
						  && u.user_id == UserId && u.n2_profile_property.property_name != _categoryName + "_0"
						  && (string.IsNullOrEmpty(query) || u.property_value.Contains(query))
					orderby u.id descending
					select new LabelValueInt { Label = u.property_value, Value = u.id }
			).ToArray();
		}

		public LabelValueInt[] SearchValues(string query)
		{
			var take = Common.Shared.Configuration.SearchHistoryDisplay;
			query = query.Trim();
			return (from u in _db.n2_user_profile
					where u.n2_profile_property.n2_profile_category.profile_category_name == _categoryName
						  && u.user_id == UserId && u.n2_profile_property.property_name != _categoryName + "_0"
						  && (string.IsNullOrEmpty(query) || u.property_value.Contains(query))
					orderby u.id descending
					select new LabelValueInt { Label = u.property_value, Value = u.id }
			).Take(take).ToArray();
		}

		protected n2_profile_property GetProfileProperty(int index)
		{
			var propertyCategory = _db.n2_profile_property
				.FirstOrDefault(x => x.n2_profile_category.profile_category_name == _categoryName
									 && x.property_name == _categoryName + "_" + index.ToString());
			// property category does not exist.
			if (propertyCategory == null)
			{
				var category = _db.n2_profile_category.FirstOrDefault(x => x.profile_category_name == _categoryName);
				if (category == null)
				{
					throw new Exception("Not found.");
				}
				propertyCategory = new n2_profile_property
				{
					property_category_id = category.id,
					property_name = _categoryName + "_" + index,
					property_description = "",
					property_defaullt = ""
				};
				_db.n2_profile_property.Add(propertyCategory);
				_db.SaveChanges();
			}
			return propertyCategory;
		}

		protected n2_user_profile SetUserProperty(int index, string value)
		{
			var property = GetProfileProperty(index);

			var userProfile = _db.n2_user_profile.FirstOrDefault(x => x.property_id == property.id && x.user_id == UserId);
			if (userProfile == null)
			{
				userProfile = new n2_user_profile
				{
					user_id = UserId,
					property_id = property.id,
					property_value = value
				};
			}
			else
			{
				userProfile.property_value = value;
			}
			_db.n2_user_profile.AddOrUpdate(userProfile);
			_db.SaveChanges();
			return userProfile;
		}


		protected n2_user_profile GetUserProperty(int index)
		{
			var property = GetProfileProperty(index);

			var userProfile = _db.n2_user_profile.FirstOrDefault(x => x.property_id == property.id);
			if (userProfile == null)
			{
				userProfile = new n2_user_profile
				{
					user_id = UserId,
					property_id = property.id,
					property_value = index == 0 ? "0" : ""
				};
				_db.n2_user_profile.AddOrUpdate(userProfile);
				_db.SaveChanges();
			}
			return userProfile;
		}
		public void Add(string value)
		{
			var exist = _db.n2_user_profile
				.FirstOrDefault(x => x.property_value == value && x.n2_profile_property.property_name != _categoryName + "_0"
							&& x.n2_profile_property.n2_profile_category.profile_category_name ==
							_categoryName && x.user_id == UserId);
			if (exist != null) return;
			var index = Convert.ToInt32(GetUserProperty(0).property_value) + 1;
			if (index > _maxCount) index = 1;
			SetUserProperty(0, index.ToString());
			SetUserProperty(index, value);
		}
	}
}