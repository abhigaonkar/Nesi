namespace NESI.DTO.ViewModels.Core
{
	public class DataExtra
	{
		public DataExtra()
		{
			
		}
		public DataExtra(object data)
		{
			Data = data;
		}

		public DataExtra(object data, object extra)
		{
			Data = data;
			Extra = extra;
		}
		public object Data { get; set; }
		public object Extra { get; set; }
	}
}