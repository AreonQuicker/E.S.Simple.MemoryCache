using System.Collections.Generic;

namespace E.S.Simple.MemoryCache.Interfaces
{
    public interface IMemoryCacheManager
    {
        T Get<T>(string key);
        IDictionary<string, object> GetMany(string[] keys);
        void Set(string key, object data, int cacheTimeInSeconds);
        bool IsSet(string key);
        void Remove(string key);
        void RemoveByPattern(string pattern); 
        void Clear();
    }
}
