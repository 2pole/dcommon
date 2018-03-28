using System;
using Common.Logging;
using NHibernate;
using NHibernate.Cfg;
using DCommon.Configuration;
using DCommon.NHibernate.Providers;
using DCommon.Environment.Configuration;
using Cfg = NHibernate.Cfg.Configuration;

namespace DCommon.NHibernate
{
    public interface ISessionFactoryHolder
    {
        ISessionFactory GetSessionFactory();
        Cfg GetConfiguration();
    }

    public class SessionFactoryHolder : ISessionFactoryHolder, IDisposable
    {
        private readonly IDataServicesProviderFactory _dataServicesProviderFactory;
        private readonly ISessionConfigurationCache _sessionConfigurationCache;
        private readonly ISettingsManager _shellSettingsManager;

        private ISessionFactory _sessionFactory;
        private Cfg _configuration;

        public SessionFactoryHolder(
            IDataServicesProviderFactory dataServicesProviderFactory,
            ISessionConfigurationCache sessionConfigurationCache,
            ISettingsManager settingsManager)
        {
            _dataServicesProviderFactory = dataServicesProviderFactory;
            _sessionConfigurationCache = sessionConfigurationCache;
            _shellSettingsManager = settingsManager;

            Logger = LogManager.GetLogger(typeof (SessionFactoryHolder));
        }

        public ILog Logger { get; set; }

        public void Dispose()
        {
            if (_sessionFactory != null)
            {
                _sessionFactory.Dispose();
                _sessionFactory = null;
            }
        }

        public ISessionFactory GetSessionFactory()
        {
            lock (this)
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = BuildSessionFactory();
                }
            }
            return _sessionFactory;
        }

        public Cfg GetConfiguration()
        {
            lock (this)
            {
                if (_configuration == null)
                {
                    _configuration = BuildConfiguration();
                }
            }
            return _configuration;
        }

        private ISessionFactory BuildSessionFactory()
        {
            Logger.Debug("Building session factory");

            //if (!_hostEnvironment.IsFullTrust)
            //    NHibernate.Cfg.Environment.UseReflectionOptimizer = false;

            Cfg config = GetConfiguration();
            var result = config.BuildSessionFactory();
            Logger.Debug("Done building session factory");
            return result;
        }

        private Cfg BuildConfiguration()
        {
            Logger.Debug("Building configuration");
            var setting = _shellSettingsManager.LoadSetting();
            var config = _sessionConfigurationCache.GetConfiguration(() =>
                _dataServicesProviderFactory
                    .CreateProvider(setting)
                    .BuildConfiguration(setting));

            Logger.Debug("Done Building configuration");
            return config;
        }
    }
}
