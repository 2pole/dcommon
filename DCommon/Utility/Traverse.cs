﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Utility
{
    public static class Traverse
    {
        public static IEnumerable<T> Across<T>(T first, Func<T, T> next)
            where T : class
        {
            var item = first;
            while (item != null)
            {
                yield return item;
                item = next(item);
            }
        }
    }
}
