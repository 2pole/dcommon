    using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DCommon.Environment.Configuration;

namespace DCommon.Security.Providers
{
    public class DefaultEncryptionService : IEncryptionService
    {
        //private readonly ISettingsManager _settingManager;
        private readonly ShellSetting _shellSetting;
        protected ShellSetting ShellSetting { get { return _shellSetting; } }

        public DefaultEncryptionService(ISettingsManager settingManager)
        {
            _shellSetting = settingManager.LoadSetting();
        }

        public virtual byte[] Decode(byte[] encodedData)
        {
            // extract parts of the encoded data
            using (var symmetricAlgorithm = CreateSymmetricAlgorithm())
            {
                using (var hashAlgorithm = CreateHashAlgorithm())
                {
                    var iv = new byte[symmetricAlgorithm.BlockSize / 8];
                    var signature = new byte[hashAlgorithm.HashSize / 8];
                    var data = new byte[encodedData.Length - iv.Length - signature.Length];

                    Array.Copy(encodedData, 0, iv, 0, iv.Length);
                    Array.Copy(encodedData, iv.Length, data, 0, data.Length);
                    Array.Copy(encodedData, iv.Length + data.Length, signature, 0, signature.Length);

                    // validate the signature
                    var mac = hashAlgorithm.ComputeHash(iv.Concat(data).ToArray());

                    if (!mac.SequenceEqual(signature))
                    {
                        // message has been tampered
                        throw new ArgumentException();
                    }

                    symmetricAlgorithm.IV = iv;

                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                            cs.FlushFinalBlock();
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

        public virtual byte[] Encode(byte[] data)
        {
            // cipherText ::= IV || ENC(EncryptionKey, IV, plainText) || HMAC(SigningKey, IV || ENC(EncryptionKey, IV, plainText))

            byte[] encryptedData;
            byte[] iv;

            using (var ms = new MemoryStream())
            {
                using (var symmetricAlgorithm = CreateSymmetricAlgorithm())
                {
                    // generate a new IV each time the Encode is called
                    symmetricAlgorithm.GenerateIV();
                    iv = symmetricAlgorithm.IV;

                    using (var cs = new CryptoStream(ms, symmetricAlgorithm.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                    }

                    encryptedData = ms.ToArray();
                }
            }

            byte[] signedData;

            // signing IV || encrypted data
            using (var hashAlgorithm = CreateHashAlgorithm())
            {
                signedData = hashAlgorithm.ComputeHash(iv.Concat(encryptedData).ToArray());
            }

            return iv.Concat(encryptedData).Concat(signedData).ToArray();
        }

        /// <summary>
        /// Hash data.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <returns>The hashed value.</returns>
        public virtual byte[] Hash(byte[] data)
        {
            byte[] hashBytes = null;
            using (var hashAlgorithm = CreateHashAlgorithm())
            {
                hashBytes = hashAlgorithm.ComputeHash(data);
            }
            return hashBytes;
        }

        protected virtual SymmetricAlgorithm CreateSymmetricAlgorithm()
        {
            var algorithm = SymmetricAlgorithm.Create(_shellSetting.EncryptionAlgorithm);
            algorithm.Key = HexToByteArray(_shellSetting.EncryptionKey);
            return algorithm;
        }

        protected virtual HMAC CreateHashAlgorithm()
        {
            var algorithm = HMAC.Create(_shellSetting.HashAlgorithm);
            algorithm.Key = HexToByteArray(_shellSetting.HashKey);
            return algorithm;
        }

        protected static byte[] HexToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).
                Where(x => 0 == x % 2).
                Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).
                ToArray();
        }

        //private static string ToHexString(byte[] bytes)
        //{
        //    StringBuilder strB = new StringBuilder();
        //}
    }
}
