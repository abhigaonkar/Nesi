using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class InsertTimeSheetTelem : InsertTimeSheetBase
	{
		[Required]
		public int CustomerBusinessUnitId { get; set; }
		[Required]
		public string CustomerBusinessUnitname { get; set; }

		public override string CustNo => CustomerBusinessUnitId.ToString();
		public override int CustId => 0;
		public override string CustName => CustomerBusinessUnitname;
		public override string WoProg_Bvwo => CustomerBusinessUnitId.ToString();
		public TelemRecord Record;
	}
}