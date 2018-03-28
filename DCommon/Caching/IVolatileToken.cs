using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Caching
{
    public interface IVolatileToken
    {
        bool IsCurrent { get; }
    }
}
