using System.ComponentModel.DataAnnotations;
using NESI.DTO.ViewModels.Core;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeFootPrintsReassign
	{
		[Required]
		public string button { get; set; }
		[Required]
		public string type { get; set; }
		[Required]
		public int from_member_id { get; set; }
		[Required]
		public int to_member_id { get; set; }
		[Required]
		public LabelValueStringLink[] items { get; set; }
	}
}