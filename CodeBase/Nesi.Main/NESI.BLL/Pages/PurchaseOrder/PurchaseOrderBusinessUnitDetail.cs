using System.Data;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;

namespace NESI.BLL.Pages.PurchaseOrder
{
	public class PurchaseOrderBusinessUnitDetail : BLLGridBase<DTO.ViewModels.Page.PurchaseOrder.PurchaseOrderBusinessUnitDetail>
	{
		public PurchaseOrderBusinessUnitDetail()
		{

		}


		public PurchaseOrderBusinessUnitDetail(Employee user, BodyParams param, int buid) : base(user, param, new object[] { buid })
		{
			this.donotcache = true;
			this.query = @"CALL report_po_progress(@p0)";
		}

		public override DataTable ExtraFilterDataTable(DataTable dt)
		{
			return !CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault()
				? (new DataView(dt) { RowFilter = "nesi_cut_po = false" }).ToTable()
				: dt;
		}
	}
}