using E.S.Simple.MemoryCache.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace E.S.Simple.MemoryCache.Core
{
    public class MemoryCacheManager : IMemoryCacheManager
    {
        #region Private Fields

        private string keyPrefix { get; } = nameof(MemoryCacheManager);

        #endregion

        #region Constructor

        public MemoryCacheManager()
        {
        }

        public MemoryCacheManager(string keyPrefix)
        {
            this.keyPrefix = keyPrefix;
        }

        #endregion

        #region Private Properties

        private string GetKey(string key) => $"{keyPrefix}{key}";

        private ObjectCache Cache => System.Runtime.Caching.MemoryCache.Default;

        #endregion

        #region ICacheManager

        public virtual T Get<T>(string key) => (T) Cache[GetKey(key)];

        public virtual IDictionary<string, object> GetMany(string[] keys)
        {
            var tresult = new Dictionary<string, object>();

            foreach (var k in keys)
            {
                var val = Get<object>(GetKey(k));

                tresult.Add(k, val);
            }

            return tresult;
        }

        public virtual void Set(string key, object data, int cacheTimeInSeconds)
        {
            if (data == null) return;
            if (key == null) return;

            if (IsSet(key))
                Remove(key);

            var policy = new CacheItemPolicy
                {AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds((cacheTimeInSeconds))};

            Cache.Add(new CacheItem(GetKey(key), data), policy);
        }

        public virtual bool IsSet(string key) => Cache.Contains(GetKey(key));

        public virtual void Remove(string key)
        {
            if (!key.StartsWith(keyPrefix)) key = keyPrefix + key;
            Cache.Remove(key);
        }

        public virtual void RemoveByPattern(string pattern)
        {
            var regex = new Regex(GetKey(pattern),
                RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<string>();

            foreach (var item in Cache)
                if (regex.IsMatch(item.Key))
                    keysToRemove.Add(item.Key);

            foreach (var key in keysToRemove)
                Remove(key);
        }

        public virtual void Clear()
        {
            foreach (var item in Cache.Where(c => c.Key.StartsWith(keyPrefix)))
                Remove(item.Key);
        }

        #endregion
    }
}