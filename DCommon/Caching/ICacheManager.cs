using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Caching
{
    public interface ICacheManager
    {
        TResult Get<TKey, TResult>(TKey key, Func<AcquireContext<TKey>, TResult> acquire);
        ICache<TKey, TResult> GetCache<TKey, TResult>();
    }
}
