using NESI.Common.Interface;

namespace NESI.BLL.Common.Cache
{
	public class IReportCacher : MemoryCacher<IReport>
	{
		private const string CacheName = "IReport";

		public IReportCacher() : base(CacheName)
		{

		}
	}
}