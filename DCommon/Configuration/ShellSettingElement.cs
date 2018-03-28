using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DCommon.Configuration
{
    public class ShellSettingElement : ConfigurationElement
    {
        /// <summary>
        /// The name the shell
        /// </summary>
        [ConfigurationProperty("shellName", IsRequired = false)]
        public string ShellName
        {
            get
            {
                return (string)this["shellName"];
            }
        }

        /// <summary>
        /// The encryption algorithm used for encryption services
        /// </summary>
        [ConfigurationProperty("encryptionAlgorithm", IsRequired = false)]
        public string EncryptionAlgorithm
        {
            get
            {
                return (string)this["encryptionAlgorithm"];
            }
        }

        /// <summary>
        /// The encryption key used for encryption services
        /// </summary>
        [ConfigurationProperty("encryptionKey", IsRequired = false)]
        public string EncryptionKey
        {
            get
            {
                return (string)this["encryptionKey"];
            }
        }

        /// <summary>
        /// The hash algorithm used for encryption services
        /// </summary>
        [ConfigurationProperty("hashAlgorithm", IsRequired = false)]
        public string HashAlgorithm
        {
            get
            {
                return (string)this["hashAlgorithm"];
            }
        }

        /// <summary>
        /// The hash key used for encryption services
        /// </summary>
        [ConfigurationProperty("hashKey", IsRequired = false)]
        public string HashKey
        {
            get
            {
                return (string)this["hashKey"];
            }
        }
    }
}
