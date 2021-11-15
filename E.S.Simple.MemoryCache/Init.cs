using E.S.Simple.MemoryCache.Core;
using E.S.Simple.MemoryCache.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace E.S.Simple.MemoryCache
{
    public static class Init
    {
        public static void AddSimpleMemoryCache(this IServiceCollection services)
        {
            services.AddSingleton<IMemoryCacheManager, MemoryCacheManager>();
        }
    }
}
