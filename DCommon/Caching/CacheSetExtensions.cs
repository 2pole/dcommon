using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Caching
{
    public static class CacheSetExtensions
    {
        public static T Get<T>(this ICacheSet cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 0, acquire);
        }

        public static T Get<T>(this ICacheSet cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }
            else
            {
                var result = acquire();
                cacheManager.Set(key, result, cacheTime);
                return result;
            }
        }
    }
}
