namespace NESI.BLL.Common.Cache
{
	public class OriginalUser : OnlineUser
	{
		private const string CacheName = "OriginalUser";

		public OriginalUser() : base(CacheName)
		{

		}
	}
}