namespace NESI.DTO.ViewModels.Page.MessagesPage
{
	public class SendMessage
	{
		public int[] to { get; set; }
		public string subject { get; set; }
		public string body { get; set; }
		public string file_name { get; set; }
	}
}