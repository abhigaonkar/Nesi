using System;
using System.Collections.Generic;
using System.Linq;
using NESI.BLL.Mebmer;

namespace NESI.Cache
{
	public class BusinessUnit : MemoryCacher<NESI.DTO.Models.BusinessUnit>
	{
		private const string CacheName = "BusinessUnit";

		public BusinessUnit() :base(CacheName)
		{
			var db=new NESI.Data.Entities.NESIMySQL();
			foreach (var x in AutoMapper.Mapper.Map<List<NESI.DTO.Models.BusinessUnit>>(db.business_unit.ToList()))
			{
				this.Add(x.ID.ToString(), x, DateTime.UtcNow.AddYears(10));
			}
		}

		/// <summary>
		/// get all active business_unit
		/// </summary>
		/// <returns>activ business unit list</returns>
		public List<NESI.DTO.Models.BusinessUnit> GetActiveList()
		{
			return base.GetList().FindAll(m => m.Active == "T");
		}

		/// <summary>
		/// get business unit name by business unit id
		/// </summary>
		/// <param name="id">business unit id</param>
		/// <returns></returns>
		public string GetName(int id)
		{
			return this.GetValue(id).Name;
		}
	}
}