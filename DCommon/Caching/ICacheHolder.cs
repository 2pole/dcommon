using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Caching
{
    public interface ICacheHolder
    {
        ICache<TKey, TResult> GetCache<TKey, TResult>();
    }
}
