using System.Collections.Generic;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Common.Cache;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Core
{
	public class BusinessUnit : BLLBase
	{

		public BusinessUnit(Employee.Employee employee) : base(employee)
		{

		}

		public string GetVisibleBusinessUnits()
		{
			return _db.Database.SqlQuery<string>("Call get_visible_business_units_group_concat(@p0)", CurrentUser.Id)
				.FirstOrDefault();
		}

		public List<DTO.Models.Core.BusinessUnit> GetVisibleBusinessUnitList()
		{
			return GetVisibleBusinessUnitList(GetVisibleBusinessUnits());
		}


		public List<DTO.ViewModels.Core.BusinessUnitDropDownList> GetVisibileBusinessUnitDropDownLists()
		{
			return GetVisibileBusinessUnitDropDownLists(GetVisibleBusinessUnits());
		}

		public string GetVisibleTaxEntities()
		{
			return _db.Database.SqlQuery<string>("Call get_visible_tax_entities_group_concat(@p0)", CurrentUser.Id)
				.FirstOrDefault();
		}


		public static string GetVisibleBusinessUnits(int user_id)
		{
			var db = new NESIMySQL();
			return db.Database.SqlQuery<string>("Call get_visible_business_units_group_concat(@p0)", user_id)
				.FirstOrDefault();
		}


		public string GetAssociatedBusinessUnits()
		{
			var ids = Global.BusinessUnit.GetActiveList()
				.Where(b => b.tax_entity_id == CurrentUser.TaxEntityId)
				.Select(b => b.ID);
			return string.Join(",", ids.Select(i => i.ToString()).ToArray());
		}

		public List<DTO.Models.Core.BusinessUnit> GetAssociatedBusinessUnitList()
		{
			return Global.BusinessUnit.GetActiveList()
				.Where(b => b.tax_entity_id == CurrentUser.TaxEntityId)
				.ToList();
		}

		public DTO.ViewModels.Core.BusinessUnitDropDownList[] GetActiveBusinessUnitDropDownLists()
		{
			return AutoMapper.Mapper.Map<DTO.ViewModels.Core.BusinessUnitDropDownList[]>(Global.BusinessUnit.GetActiveList());


		}

		public int GetBranchManagerId()
		{
			return _db.Database.SqlQuery<int>(@"select get_bm(@p0 )", CurrentUser.BusinessUnitId).FirstOrDefault();
		}

		public int GetBusinessUnitIdByUserId(int userId)
		{
			var o = _db.member.FirstOrDefault(x => x.Member_ID == userId);
			return o?.business_unit_id ?? 0;
		}


	}
}
