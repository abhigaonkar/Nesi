using System;

namespace NESI.DTO.ViewModels.CurrentUser.Layout
{
	public class DashMessage
	{
		public int Id { get; set; }
		public DateTime? Date { get; set; }
		public string Name { get; set; }
		public string Text { get; set; }
		public int? BusinessUnitId { get; set; }
		public int MemberId { get; set; }
	}
}