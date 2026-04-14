using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class InsertTimeSheetShop : InsertTimeSheetBase
	{
		[Required]
		public int SelectedShopTimeTypeId { get; set; }
		[Required]
		public string SelectedShopTimeTypeName { get; set; }
        public int? internal_project_id { get; set; }
        public override string CustNo => SelectedShopTimeTypeId.ToString();
		public override int CustId => 0;
		public override string WoProg_Bvwo => SelectedShopTimeTypeId.ToString();
		public override string CustName => SelectedShopTimeTypeName;

	}
}