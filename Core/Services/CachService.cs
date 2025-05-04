using Domain.Contracts;
using Services_Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CachService(ICachRepository cachRepository) : ICachService
    {
        public async Task<string?> GetCachValueAsync(string key)
        {
            // Use the ICachRepository to retrieve the cached value
            var cachedValue = await cachRepository.GetAsync(key);

            // Return the cached value, or null if it doesn't exist
            return string.IsNullOrEmpty(cachedValue) ? null : cachedValue;
        }


        public async Task SetCacheValueAsync(string key, object value, TimeSpan duration)
        {
            // Use the ICachRepository to set the cached value
            await cachRepository.SetAsync(key, value, duration);
        }
    }
}
