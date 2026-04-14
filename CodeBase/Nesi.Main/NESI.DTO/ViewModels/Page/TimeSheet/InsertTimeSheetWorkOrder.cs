using System;
using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{
	public class InsertTimeSheetWorkOrder : InsertTimeSheetBase
	{
		[Required]
		public int SelectedCustomerId { get; set; }
		[Required]
		public string SelectedCustomerName { get; set; }
		[Required]
		public int SelectedWorkOrderId { get; set; }
		[Required]
		public string SelectedWorkOrderName { get; set; }
		public int SelectedTsLiteType { get; set; }
        public bool is_prevailing_wage { get; set; }

        public override int CustId => SelectedCustomerId;
		public override string CustName => SelectedCustomerName;
	}
}