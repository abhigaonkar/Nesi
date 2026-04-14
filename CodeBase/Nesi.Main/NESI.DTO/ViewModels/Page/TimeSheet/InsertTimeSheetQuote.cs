using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class InsertTimeSheetQuote : InsertTimeSheetBase
	{
		[Required]
		public int SelectedCustomerId { get; set; }
		[Required]
		public string SelectedCustomerName { get; set; }
		[Required]
		public int SelectedQuoteId { get; set; }
		[Required]
		public string SelectedQuoteName { get; set; }

		public override int CustId => 0;
		public override string CustNo => SelectedCustomerId.ToString();
		public override string CustName => SelectedCustomerName;
		public override string WoProg_Bvwo => SelectedQuoteId.ToString();

	}
}