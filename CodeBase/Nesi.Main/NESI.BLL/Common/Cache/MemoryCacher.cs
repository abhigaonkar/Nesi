using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace NESI.BLL.Common.Cache
{
    public class MemoryCacher<T> : IDisposable where T : class
    {
        private MemoryCache _cache;
        internal string _name;
        private bool _disposed = false;

        public MemoryCacher()
        {
            _cache = MemoryCache.Default;
        }

        public MemoryCacher(string name)
        {
            _name = name;
            _cache = new MemoryCache(name);
        }

        public virtual T GetValue(string key)
        {
            return _cache.Contains(key) ? (T)_cache.Get(key) : null;
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

        public void Clear()
        {
            foreach (var item in _cache)
            {
                _cache.Remove(item.Key);
            }
        }
		public void Clear(string keyLike)
			{
			List<string> keysToRemove = _cache.Where(pair => pair.Key.StartsWith(keyLike))
											  .Select(pair => pair.Key)
											  .ToList();

			foreach (string key in keysToRemove)
				{
				_cache.Remove(key);
				}
			}

		public bool Contains(string key)
        {
            return _cache.Contains(key);
        }

        public virtual void Set(string key, T value)
        {
            if (_cache.Contains(key))
            {
                _cache[key] = value;
            }
            else
            {
                Add(key, value, DateTimeOffset.UtcNow.AddMinutes(
                    Convert.ToDouble(NESI.BLL.Common.Shared.Configuration.MemCacheAbsoluteTimeout)));
            }
        }

        public virtual void Set(int key, T value)
        {
            Set(key.ToString(), value);
        }

        public List<T> GetList()
        {
            return _cache.ToArray().Select(item => (T)item.Value).ToList();
        }

        // Implement IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    if (_cache != null)
                    {
                        _cache.Dispose();
                        _cache = null;
                    }
                }

                // Dispose unmanaged resources (if any)

                _disposed = true;
            }
        }

        ~MemoryCacher()
        {
            Dispose(false);
        }
    }
}
