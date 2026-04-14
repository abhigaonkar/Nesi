using System;
using System.ComponentModel.DataAnnotations;
using NESI.Common;

namespace NESI.DTO.ViewModels.Page.Employees
{
	[ModelDefination("EmployeeDiscplinaryGrid")]
	public class EmployeeDiscplinaryGrid : ModelBase<EmployeeDiscplinaryGrid>
	{
		public int membernote_id { get; set; }
		[Required]
		public int membernote_member_id { get; set; }
		[Required]
		public DateTime date { get; set; }
		public string note_type { get; set; }
		[Required]
		public string comments { get; set; }
		public int member_id_audit { get; set;}
		public string active { get; set; }
		public DateTime? Created_Date { get; set; }
		public int membernote_addedby_member_id { get; set; }
		public int business_unit { get; set; }
		public string usernote { get; set; }
		public string addedby { get; set; }
		public string file_id { get; set; }
		public string file_name { get; set; }
	}
}