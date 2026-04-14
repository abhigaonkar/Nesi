namespace NESI.BLL.Common.Cache
{
	public static class Global
	{
		private static TaxEntity _taxEntity;
		private static BusinessUnit _businessUnit;
		private static OnlineUser _onlineUser;
		private static OriginalUser _originalUser;
		private static DatatableCacher _datatable;
		private static IReportCacher _ireport;
		private static ListCacher<object> _listObject;

		public static OnlineUser OriginalUser => _originalUser = _originalUser ?? new OriginalUser();
		public static OnlineUser OnlineUser => _onlineUser = _onlineUser ?? new OnlineUser();
		public static TaxEntity TaxEntity => _taxEntity = _taxEntity ?? new TaxEntity();
		public static BusinessUnit BusinessUnit => _businessUnit = _businessUnit ?? new BusinessUnit();
		public static DatatableCacher Datatable => _datatable = _datatable ?? new DatatableCacher();
		public static IReportCacher IReport => _ireport = _ireport ?? new IReportCacher();
		public static ListCacher<object> ListObject => _listObject = _listObject ?? new ListCacher<object>();

		//		static Global()
		//		{
		//			// newcache();
		//		}
		//
		//		public static void newcache()
		//		{
		//			_taxEntity = new TaxEntity();
		//			_businessUnit = new BusinessUnit();
		//		}

		public static void Refresh()
		{
			BusinessUnit.GetAll();
			TaxEntity.GetAll();
		}
	}
}
