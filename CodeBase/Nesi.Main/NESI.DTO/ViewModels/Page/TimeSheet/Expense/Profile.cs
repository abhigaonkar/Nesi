using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Core.Enums;

namespace NESI.DTO.ViewModels.Page.TimeSheet.Expense
{
	public class Profile
	{
		public int UserId { get; set; }
		public Currency Currency { get; set; }
		public WorkOrder[] WorkOrders { get; set; }
		public LabelValueString[] Categories { get; set; }

        public LabelValueString[] creditCardCategories { get; set; }
    }
}