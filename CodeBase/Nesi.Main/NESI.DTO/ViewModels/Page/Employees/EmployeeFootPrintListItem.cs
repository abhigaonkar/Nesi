using NESI.DTO.ViewModels.Core;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeFootPrintListItem
	{
		public EmployeeFootPrintListItem(string btn, LabelValueStringLink[] o)
		{
			button = btn;
			options = o;
		}

		public string button { get; set; }
		public LabelValueStringLink[] options { get; set; }
	}
}