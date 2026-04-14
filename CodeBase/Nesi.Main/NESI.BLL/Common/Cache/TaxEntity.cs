using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using nesi.core;

namespace NESI.BLL.Common.Cache
{
	public class TaxEntity : MemoryCacher<DTO.Models.Core.TaxEntity>
	{
		private const string CacheName = "TaxEntity";

		public TaxEntity() :base(CacheName)
		{
			GetAll();
		}


		public void GetAll()
		{
			try
				{
				var db = new NESI.Data.Entities.NESIMySQL();
				foreach (var x in AutoMapper.Mapper.Map<List<DTO.Models.Core.TaxEntity>>(db.tax_entity.ToList()))
					{
					if (Contains(x.id.ToString()))
						{
						Set(x.id.ToString(), x);
						}
					else
						{
						Add(x.id.ToString(), x, DateTime.Now.AddYears(15));
						}

					// Add(x.id.ToString(), x, DateTime.Now.AddYears(15));
					}
				}
			catch (Exception ex)
				{
				Toolbox.do_errorLog(ex, "Error caching tax entities");
				throw;
				}
			}
		/// <summary>
		/// get all business unit in this tax entity
		/// </summary>
		/// <param name="id">tax entity id</param>
		/// <returns>active business unit list in the tax entity</returns>
		public List<DTO.Models.Core.BusinessUnit> GetBusinessUnits(int id)
		{
			return Global.BusinessUnit.GetActiveList().FindAll(m => m.tax_entity_id == id);
		}
		/// <summary>
		/// get all active tax entities
		/// </summary>
		/// <returns>list of tax entity</returns>
		public List<DTO.Models.Core.TaxEntity> GetActiveList()
		{
			return GetList().FindAll(m => m.is_active == true);
		}

		public DTO.Models.Core.TaxEntity GetValue(int id)
		{
			return base.GetValue(id.ToString());
		}
		public string GetName(int id)
		{
			return this.GetValue(id.ToString())?.public_name;
		}
	}
}