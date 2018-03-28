using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Logging;
using DCommon.Caching;
using DCommon.Configuration;
using DCommon.FileSystems;

namespace DCommon.Environment.Configuration
{
    public class SettingsManager : ISettingsManager
    {
        private readonly ICacheManager _cacheManager;
        private readonly IAppDataFolder _appDataFolder;
        public ILog Logger { get; set; }
        //private readonly IAppDataFolder _appDataFolder;
        //private readonly IShellSettingsManagerEventHandler _events;

        //public ShellSettingsManager(
        //    IAppDataFolder appDataFolder,
        //    IShellSettingsManagerEventHandler events)
        //{
        //    _appDataFolder = appDataFolder;
        //    _events = events;
        //}

        public SettingsManager(ICacheManager cacheManager, IAppDataFolder appDataFolder)
        {
            _cacheManager = cacheManager;
            _appDataFolder = appDataFolder;
            this.Logger = LogManager.GetLogger<SettingsManager>();
        }

        ShellSetting ISettingsManager.LoadSetting()
        {
            return LoadSetting();
        }

        void ISettingsManager.SaveSettings(ShellSetting settings)
        {
            //if (settings == null)
            //    throw new ArgumentException("There are no settings to save.");
            //if (string.IsNullOrEmpty(settings.Name))
            //    throw new ArgumentException("Settings \"Name\" is not set.");

            //var filePath = Path.Combine(Path.Combine("Sites", settings.Name), "Settings.txt");
            //_appDataFolder.CreateFile(filePath, ShellSettingsSerializer.ComposeSettings(settings));
            //_events.Saved(settings);
        }

        ShellSetting LoadSetting()
        {
            return _cacheManager.Get(typeof (ShellSetting), context => LoadSettingCore());
        }

        private ShellSetting LoadSettingCore()
        {
            Logger.DebugFormat("Loading setting from config.");
            var section = DCommonSection.GetDCommonSection();
            var setting = section.ShellSetting;
            var shell = new ShellSetting();
            shell.DataConnectionString = section.DataProvider.ConnectionString;
            shell.DataProvider = section.DataProvider.DataProviderName;
            shell.EncryptionAlgorithm = setting.EncryptionAlgorithm;
            shell.EncryptionKey = setting.EncryptionKey;
            shell.HashAlgorithm = setting.HashAlgorithm;
            shell.HashKey = setting.HashKey;
            shell.Name = setting.ShellName;
            shell.ShellPath = _appDataFolder.MapPath(setting.ShellName);

            return shell;
        }
    }
}
