using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Caching
{
    public interface ICache<TKey, TResult>
    {
        TResult Get(TKey key, Func<AcquireContext<TKey>, TResult> acquire);
    }
}
