using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DCommon
{
    public static class StreamExtentions
    {
        public static byte[] ToBytes(this Stream inputStream)
        {
            if (null == inputStream)
            {
                return new byte[0];
            }
            byte[] bytes = new byte[inputStream.Length];
            inputStream.Read(bytes, 0, bytes.Length);
            inputStream.Seek(0, SeekOrigin.Begin);

            return bytes;
        }

        public static Stream ToStream(this byte[] bytes)
        {
            if (null == bytes)
            {
                return null;
            }
            return new MemoryStream(bytes);
        }
    }
}
