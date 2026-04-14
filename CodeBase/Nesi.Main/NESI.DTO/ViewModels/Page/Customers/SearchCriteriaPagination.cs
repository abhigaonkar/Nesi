namespace NESI.DTO.ViewModels.Page.Customers
{
	public class SearchCriteriaPagination
	{
		public string criteria { get; set; }
		public int page { get; set; }
		public int pageSize { get; set; }
		public string sortField { get; set; }
		public int sortOrder { get; set; }
	}
}