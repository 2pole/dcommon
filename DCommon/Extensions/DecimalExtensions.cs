using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon
{
    public static class DecimalExtensions
    {
        public static decimal Less(this decimal v, decimal max)
        {
            return v > max ? max : v;
        }

        public static decimal Less(this decimal v, decimal? max)
        {
            return max.HasValue ? v.Less(max.Value) : v;
        }
    }
}
