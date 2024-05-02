using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services.Caching
{
    internal static class keys
    {
        public static List<string> cachedKeys = new List<string>();
    }
    public class CacheService<T> : ICacheService<T>
    {
        private readonly IMemoryCache _cache;
        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        //-------------------------------------------------------
        public async Task<T> GetAsync(string cacheKey)
        {

            //TODO - Log
            //T result;
            return await Task.Run(() =>
            {
                _cache.TryGetValue(cacheKey, out T entry);
                return entry;
            });
            //return Task.FromResult(result);
        }

        public async Task SetAsync(string cacheKey, T value, TimeSpan duration)
        {
            keys.cachedKeys.Add(cacheKey);
            //TODO - Log
            await Task.Run(() =>
             _cache.Set(
                 cacheKey,
                 value,
                 new MemoryCacheEntryOptions().SetAbsoluteExpiration(duration)
                 )
             );

        }

        public void Delete(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }

        public async Task<bool> RemoveLookUpKeys()
        {
            try
            {
                foreach (var cacheKey in keys.cachedKeys)
                {
                    await Task.Run(() => _cache.Remove(cacheKey));
                }
                keys.cachedKeys.Clear();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }



    }
}
