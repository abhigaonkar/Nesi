using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Base
{
	public class ProfileBase : BLLBase
	{
		public ProfileBase(Employee user) : base(user)
		{

		}

		public int[] GetHomeLayout_selected_business_units(string prefix)
		{
			return SplitStringToIntArray(GetValueByPropertyName($"{prefix}HomeLayout_selected_business_units"));
		}

		public int[] GetHomeLayout_openned_tax_entities(string prefix)
		{
			return SplitStringToIntArray(GetValueByPropertyName($"{prefix}HomeLayout_openned_tax_entities"));
		}

		public string GetValueByPropertyName(string name)
		{
			var ret = (from u in _db.n2_user_profile
					   where u.n2_profile_property.property_name == name && u.user_id == UserId
					   select u.property_value).FirstOrDefault();
			if (string.IsNullOrEmpty(ret))
			{
				ret = (from p in _db.n2_profile_property
					   where p.property_name == name
					   select p.property_defaullt).FirstOrDefault();
			}
			return ret;
		}

		public List<LabelValueInt> GetPropertyAllMemberValue(string name)
		{

			var pid = (from p in _db.n2_profile_property
				where p.property_name == name
				select p.id).FirstOrDefault();

			return (from u in _db.n2_user_profile
				where u.property_id == pid
				select new LabelValueInt()
				{
					Label = u.property_value,
					Value = u.user_id
				}).ToList();
		}

		public string SetPropertyValue(string name, string value)
		{
			var pid = (from p in _db.n2_profile_property
					   where p.property_name == name
					   select p.id).FirstOrDefault();
			var entity = (from u in _db.n2_user_profile
						  where u.property_id == pid && u.user_id == UserId
						  select u).FirstOrDefault() ?? new n2_user_profile()
						  {
							  user_id = UserId,
							  property_id = pid,
						  };
			entity.property_value = value;
			_db.n2_user_profile.AddOrUpdate(entity);
			_db.SaveChanges();
			return "Saved successfully.";
		}

		public int[] SplitStringToIntArray(string str)
		{
			return string.IsNullOrEmpty(str) ? null : new List<int>(Array.ConvertAll(str.Split(','), int.Parse)).ToArray();
			
		}

		public string JoinIntArrayToString(int[] ary)
		{
			return string.Join(",", ary);
		}
	}
}