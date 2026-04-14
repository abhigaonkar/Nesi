using System;
using System.Collections.Generic;
using System.Linq;
using NESI.BLL.Mebmer;

namespace NESI.Cache
{
	public class TaxEntity : MemoryCacher<NESI.DTO.Models.TaxEntity>
	{
		private const string CacheName = "TaxEntity";

		public TaxEntity() :base(CacheName)
		{
			var db=new NESI.Data.Entities.NESIMySQL();
			foreach (var x in AutoMapper.Mapper.Map<List<NESI.DTO.Models.TaxEntity>>(db.tax_entity.ToList()))
			{
				this.Add(x.id.ToString(), x, DateTime.UtcNow.AddYears(10));
			}
		}

		/// <summary>
		/// get all business unit in this tax entity
		/// </summary>
		/// <param name="id">tax entity id</param>
		/// <returns>active business unit list in the tax entity</returns>
		public List<NESI.DTO.Models.BusinessUnit> GetBusinessUnits(int id)
		{
			return NESI.Cache.Global.BusinessUnit.GetActiveList().FindAll(m => m.tax_entity_id == id);
		}
		/// <summary>
		/// get all active tax entities
		/// </summary>
		/// <returns>list of tax entity</returns>
		public List<NESI.DTO.Models.TaxEntity> GetActiveList()
		{
			return NESI.Cache.Global.TaxEntity.GetActiveList().FindAll(m => m.is_active == true);
		}

		public string GetName(int id)
		{
			return this.GetValue(id).public_name;
		}
	}
}