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
        public Task<string?> GetCachValueAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task SetCacheValueAsync(string key, object value, TimeSpan duration)
        {
            throw new NotImplementedException();
        }
    }
}
