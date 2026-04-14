using System;
using System.Collections.Generic;

namespace NESI.BLL.Common.Cache
{
	public class ListCacher<T> : MemoryCacher<List<T>>
	{
		private const string CacheName = "List";

		public ListCacher() : base(CacheName)
		{
			_name = CacheName + typeof(T);
		}
	}
}