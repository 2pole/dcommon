using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Logging;
using NHibernate.Cfg;
using DCommon.Environment.Configuration;
using DCommon.FileSystems;
using DCommon.Utility;
using Cfg = NHibernate.Cfg.Configuration;

namespace DCommon.NHibernate
{
    public class SessionConfigurationCache : ISessionConfigurationCache
    {
        private readonly ShellSetting _shellSettings;
        private readonly IAppDataFolder _appDataFolder;
        private ConfigurationCache _currentConfig;

        public SessionConfigurationCache(ISettingsManager settingsManager, IAppDataFolder appDataFolder)
        {
            _shellSettings = settingsManager.LoadSetting();
            _appDataFolder = appDataFolder;
            _currentConfig = null;

            Logger = LogManager.GetLogger(typeof (SessionConfigurationCache));
        }

        public ILog Logger { get; set; }

        public Cfg GetConfiguration(Func<Cfg> builder)
        {
            var hash = ComputeHash().Value;

            // if the current configuration is unchanged, return it
            if (_currentConfig != null && _currentConfig.Hash == hash)
            {
                return _currentConfig.Configuration;
            }

            // Return previous configuration if it exists and has the same hash as
            // the current blueprint.
            var previousConfig = ReadConfiguration(hash);
            if (previousConfig != null)
            {
                _currentConfig = previousConfig;
                return previousConfig.Configuration;
            }

            // Create cache and persist it
            _currentConfig = new ConfigurationCache
            {
                Hash = hash,
                Configuration = builder()
            };

            StoreConfiguration(_currentConfig);
            return _currentConfig.Configuration;
        }

        private class ConfigurationCache
        {
            public string Hash { get; set; }
            public Cfg Configuration { get; set; }
        }

        private void StoreConfiguration(ConfigurationCache cache)
        {
            var pathName = GetPathName(_shellSettings.Name);

            try
            {
                var formatter = new BinaryFormatter();
                using (var stream = _appDataFolder.CreateFile(pathName))
                {
                    formatter.Serialize(stream, cache.Hash);
                    formatter.Serialize(stream, cache.Configuration);
                }
            }
            catch (SerializationException e)
            {
                //Note: This can happen when multiple processes/AppDomains try to save
                //      the cached configuration at the same time. Only one concurrent
                //      writer will win, and it's harmless for the other ones to fail.
                for (Exception scan = e; scan != null; scan = scan.InnerException)
                    Logger.WarnFormat("Error storing new NHibernate cache configuration: {0}", scan.Message);
            }
        }

        private ConfigurationCache ReadConfiguration(string hash)
        {
            var pathName = GetPathName(_shellSettings.Name);

            if (!_appDataFolder.FileExists(pathName))
                return null;

            try
            {
                var formatter = new BinaryFormatter();
                using (var stream = _appDataFolder.OpenFile(pathName))
                {

                    // if the stream is empty, stop here
                    if (stream.Length == 0)
                    {
                        return null;
                    }

                    var oldHash = (string)formatter.Deserialize(stream);
                    if (hash != oldHash)
                    {
                        Logger.InfoFormat("The cached NHibernate configuration is out of date. A new one will be re-generated.");
                        return null;
                    }

                    var oldConfig = (Cfg)formatter.Deserialize(stream);

                    return new ConfigurationCache
                    {
                        Hash = oldHash,
                        Configuration = oldConfig
                    };
                }
            }
            catch (Exception e)
            {
                for (var scan = e; scan != null; scan = scan.InnerException)
                    Logger.WarnFormat("Error reading the cached NHibernate configuration: {0}", scan.Message);
                Logger.Info("A new one will be re-generated.");
                return null;
            }
        }

        private Hash ComputeHash()
        {
            var hash = new Hash();

            // Shell settings physical location
            //   The nhibernate configuration stores the physical path to the SqlCe database
            //   so we need to include the physical location as part of the hash key, so that
            //   xcopy migrations work as expected.
            var pathName = GetPathName(_shellSettings.Name);
            hash.AddString(_appDataFolder.MapPath(pathName).ToLowerInvariant());

            // Shell settings data
            hash.AddString(_shellSettings.DataProvider);
            hash.AddString(_shellSettings.DataConnectionString);
            hash.AddString(_shellSettings.Name);

            //// Assembly names, record names and property names
            //foreach (var tableName in _shellBlueprint.Records.Select(x => x.TableName))
            //{
            //    hash.AddString(tableName);
            //}

            //foreach (var recordType in _shellBlueprint.Records.Select(x => x.Type))
            //{
            //    hash.AddTypeReference(recordType);

            //    if (recordType.BaseType != null)
            //        hash.AddTypeReference(recordType.BaseType);

            //    foreach (var property in recordType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public))
            //    {
            //        hash.AddString(property.Name);
            //        hash.AddTypeReference(property.PropertyType);

            //        foreach (var attr in property.GetCustomAttributesData())
            //        {
            //            hash.AddTypeReference(attr.Constructor.DeclaringType);
            //        }
            //    }
            //}

            return hash;
        }

        private string GetPathName(string shellName)
        {
            return _appDataFolder.Combine(shellName, "mappings.bin");
        }
    }
}
