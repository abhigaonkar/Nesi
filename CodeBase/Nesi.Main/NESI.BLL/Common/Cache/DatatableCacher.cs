using System;
using System.Data;
using nesi.core;

namespace NESI.BLL.Common.Cache
{
	public class DatatableCacher : MemoryCacher<DataTable>
	{
		private const string CacheName = "BusinessUnit";

		public DatatableCacher() :base(CacheName)
		{
			
		}
	}
}