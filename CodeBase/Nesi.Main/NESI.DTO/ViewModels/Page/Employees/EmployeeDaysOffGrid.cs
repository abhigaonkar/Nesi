using System;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Employees
{
	[ModelDefination("EmployeeDaysOffGrid")]
	public class EmployeeDaysOffGrid : ModelBase<EmployeeDaysOffGrid>
	{
		public int id { get; set; }
		public string business_unit { get; set; }
		public string employee { get; set; }
		public int member_id { get; set; }
		public string member_status { get; set; }
		public int type_id { get; set; }
		public int requesttype_id { get; set; }
		public DateTime? date_requested { get; set; }
		public DateTime date_start { get; set; }
		public DateTime date_end { get; set; }
		public string comments { get; set; }
		public string status { get; set; }
		public string leavetype { get; set; }
		public string requesttype { get; set; }
		public int payperiod_id { get; set; }
		public int business_unit_id { get; set; }
	}
}