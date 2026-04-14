using System;
using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Employees
{
	public class EmployeeStartTerminate
	{
		[Required]
		public string roe { get; set; }
		[Required]
		public string act { get; set; }
		[Required]
		public DateTime ldw { get; set; }
	}
}