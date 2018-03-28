using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon
{
    public static class Int32Extensions
    {
        public static string ToFileSize(this int size)
        {
            return ((double) size).ToFileSize();
        }

        public static string ToFileSize(this Double size)
        {
            var scale = 1024;
            var kb = 1 * scale;
            var mb = kb * scale;
            var gb = mb * scale;
            var tb = gb * scale;

            if (size < kb)
                return size + " Bytes";
            else if (size < mb)
                return ((Double)size / kb).ToString("0.## KB");
            else if (size < gb)
                return ((Double)size / mb).ToString("0.## MB");
            else if (size < tb)
                return ((Double)size / gb).ToString("0.## GB");
            else
                return ((Double)size / tb).ToString("0.## TB");
        }
    }
}
