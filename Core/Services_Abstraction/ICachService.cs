using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Abstraction
{
    public interface ICachService 
    {
        Task SetCacheValueAsync(string key, object value, TimeSpan duration);
        Task<string?> GetCachValueAsync(string key);
    }
}
