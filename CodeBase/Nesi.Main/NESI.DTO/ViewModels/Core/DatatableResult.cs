using System.Data;

namespace NESI.DTO.ViewModels.Core
{
	public class DatatableResult
	{
		public int Page { get; set; }
		public int PageSize { get; set; }
		public int TotalRecorders { get; set; }
		public DataTable Data { get; set; }
	}
}