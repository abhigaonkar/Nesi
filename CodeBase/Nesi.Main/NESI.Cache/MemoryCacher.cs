using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace NESI.Cache
{
	public class MemoryCacher<T> where T : class
	{
		private readonly MemoryCache _cache;
		
		public MemoryCacher()
		{
			_cache = MemoryCache.Default;
		}
		public MemoryCacher(string name)
		{
			_cache = new MemoryCache(name);
		}
		public T GetValue(string key)
		{
			return _cache.Contains(key) ? (T) _cache.Get(key) : null;
		}

		public T GetValue(int key)
		{
			return GetValue(key.ToString());
		}
		public bool Add(string key, T value, DateTimeOffset absExpiration)
		{
			return _cache.Add(key, value, absExpiration);
		}

		public void Delete(string key)
		{
			if (_cache.Contains(key))
			{
				_cache.Remove(key);
			}
		}

		public void Set(string key, T value)
		{
			_cache[key] = value;
		}
		public void Set(int key, T value)
		{
			Set(key.ToString(),value);
		}
		public List<T> GetList()
		{
			return _cache.ToArray().Select(item => (T) item.Value).ToList();
		}
	}
}