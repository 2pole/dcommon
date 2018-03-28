using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Security
{
    public static class EncryptionServiceExtensions
    {
        public static string Decode(this IEncryptionService encryptionService, string encodingString)
        {
            var encodingData = Convert.FromBase64String(encodingString);
            var decodedData = encryptionService.Decode(encodingData);
            var decodedString = Encoding.UTF8.GetString(decodedData);
            return decodedString;
        }

        public static string Encode(this IEncryptionService encryptionService, string decodingString)
        {
            var decodingData = Encoding.UTF8.GetBytes(decodingString); 
            var encodedData = encryptionService.Encode(decodingData);
            var encodedString = Convert.ToBase64String(encodedData); 
            return encodedString;
        }

        public static string Hash(this IEncryptionService encryptionService, string value)
        {
            var data = Encoding.UTF8.GetBytes(value);
            var hashData = encryptionService.Hash(data);
            var hashString = Convert.ToBase64String(hashData);
            return hashString;
        }
    }
}
