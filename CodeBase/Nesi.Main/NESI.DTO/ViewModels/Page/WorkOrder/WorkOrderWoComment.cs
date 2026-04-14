using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.WorkOrder
{
	public class WorkOrderWoComment : WorkOrderBase
	{
		public bool printComments { get; set; }
		public int wocomment_id { get; set; }
		[Required]
		public string comments { get; set; }
	}
}