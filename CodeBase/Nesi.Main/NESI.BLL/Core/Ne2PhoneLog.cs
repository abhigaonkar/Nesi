using System;
using System.Data.Entity.Migrations;
using System.Linq;
using NESI.BLL.Base;
using NESI.DTO.Models.Core;

namespace NESI.BLL.Core
{
	public class Ne2PhoneLog : BLLBase
	{
		public DTO.Models.Core.PhoneLog[] GetPhoneLogByExt(string ext, int days = 1)
		{
			var list = _db.phone_log.Where(x => (x.phone_log_from_number == ext ||
												 x.phone_log_to_number == ext) && (x.phone_log_date ?? DateTime.Now) >= DateTime.Now.AddDays(-days) &&
			                                    !(x.is_internal ?? false)).ToArray();
			return AutoMapper.Mapper.Map<DTO.Models.Core.PhoneLog[]>(list);
		}

		public DTO.Models.Core.PhoneLog[] GetPhoneLogByExt(string ext, DateTime date)
		{
			var list = _db.phone_log.Where(x => (x.phone_log_from_number == ext ||
												x.phone_log_to_number == ext)
												&& (x.phone_log_date ?? DateTime.Now).Year == date.Year
												&& (x.phone_log_date ?? DateTime.Now).Month == date.Month
												&& (x.phone_log_date ?? DateTime.Now).Day == date.Day
												&& !(x.is_internal?? false)).ToArray();
			return AutoMapper.Mapper.Map<DTO.Models.Core.PhoneLog[]>(list);
		}

		public string UpdatePhoneLog(string ext, int id, string notes)
		{
			var entity = _db.phone_log
						.FirstOrDefault(x => x.phone_log_id == id
							&& (x.phone_log_from_number == ext || x.phone_log_to_number == ext)
						);
			if (entity == null) return $"Phone comment (ID:{id}) is not found.";
			entity.notes = notes;
			_db.phone_log.AddOrUpdate(entity);
			try
			{
				_db.SaveChanges();
			}
			catch (Exception)
			{
				return $"Phone comment (ID:{id}) updated failure.";
			}
			return "Phone comment updated successfully.";
		}

	}


}