namespace DCommon.Environment.Configuration
{
    /// <summary>
    /// Represents the minimalistic set of fields stored for each tenant. This 
    /// model is obtained from the IShellSettingsManager, which by default reads this
    /// from the App_Data settings.txt files.
    /// </summary>
    public class ShellSetting
    {
        public ShellSetting()
        { 
        }

        public ShellSetting(ShellSetting setting)
        {
            Name = setting.Name;
            DataProvider = setting.DataProvider;
            DataConnectionString = setting.DataConnectionString;
            EncryptionAlgorithm = setting.EncryptionAlgorithm;
            EncryptionKey = setting.EncryptionKey;
            HashAlgorithm = setting.HashAlgorithm;
            HashKey = setting.HashKey;
            ShellPath = setting.ShellPath;
        }

        /// <summary>
        /// The name pf the tenant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The database provider
        /// </summary>
        public string DataProvider { get; set; }

        /// <summary>
        /// The database connection string
        /// </summary>
        public string DataConnectionString { get; set; }

        /// <summary>
        /// The encryption algorithm used for encryption services
        /// </summary>
        public string EncryptionAlgorithm { get; set; }

        /// <summary>
        /// The encryption key used for encryption services
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// The hash algorithm used for encryption services
        /// </summary>
        public string HashAlgorithm { get; set; }

        /// <summary>
        /// The hash key used for encryption services
        /// </summary>
        public string HashKey { get; set; }

        /// <summary>
        /// The path for the app.
        /// </summary>
        public string ShellPath { get; set; }
    }
}