using nesi.core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NESI.BLL.Common.Cache
{
	public class BusinessUnit : MemoryCacher<DTO.Models.Core.BusinessUnit>
	{
		private const string CacheName = "BusinessUnit";

		public BusinessUnit() : base(CacheName)
		{
			GetAll();
		}

		public void GetAll()
			{
			try
				{
				var db = new NESI.Data.Entities.NESIMySQL();
				foreach (var x in AutoMapper.Mapper.Map<List<DTO.Models.Core.BusinessUnit>>(db.business_unit.ToList()))
					{
					// x.tax_entity_name = Global.TaxEntity.GetName(x.tax_entity_id);
					var te = Global.TaxEntity.GetValue(x.tax_entity_id.ToString());
					x.tax_entity_name = te?.public_name;
					x.tax_no = te?.tax_id_no;
					if (Contains(x.ID.ToString()))
						{
						Set(x.ID.ToString(), x);
						}
					else
						{
						Add(x.ID.ToString(), x, DateTime.Now.AddYears(15));
						}
					}
				}
			catch (Exception ex)
				{
				Toolbox.do_errorLog(ex, "Error caching business units");
				throw;
				}
			}
		/// <summary>
		/// get all active business_unit
		/// </summary>
		/// <returns>activ business unit list</returns>
		public List<DTO.Models.Core.BusinessUnit> GetActiveList()
		{
			return GetList().FindAll(m => m.Active == "T");
		}

		public override DTO.Models.Core.BusinessUnit GetValue(string id)
		{
			var result = base.GetValue(id);
			if (result == null && int.TryParse(id, out int intId))
			{
				var db = new NESI.Data.Entities.NESIMySQL();
				var item = db.business_unit.FirstOrDefault(bu => bu.ID == intId);
				if (item != null)
				{
					result = AutoMapper.Mapper.Map<DTO.Models.Core.BusinessUnit>(item);

					var te = Global.TaxEntity.GetValue(result.tax_entity_id.ToString());
					result.tax_entity_name = te?.public_name;
					result.tax_no = te?.tax_id_no;

					Add(result.ID.ToString(), result, DateTime.Now.AddYears(15));
				}
			}
			return result;
		}

		public DTO.Models.Core.BusinessUnit GetValue(int id) => GetValue(id.ToString());

		/// <summary>
		/// get business unit name by business unit id
		/// </summary>
		/// <param name="id">business unit id</param>
		/// <returns></returns>
		public string GetName(int id)
		{
			return GetValue(id.ToString()).Name;
		}
	}
}