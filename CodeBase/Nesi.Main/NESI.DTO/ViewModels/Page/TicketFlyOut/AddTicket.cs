using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.TicketFlyOut
{
	public class AddTicket
	{
		[Required]
		public int Group { get; set; }
		[Required]
		public int Type { get; set; }
		[Required]
		public int Pert { get; set; }
		[Required]
		public string Subject { get; set; }
		[Required]
		public string body { get; set; }
		public bool Is_private { get; set; }
		public string File { get; set; }
		public bool Has_file { get; set; }
	}
}