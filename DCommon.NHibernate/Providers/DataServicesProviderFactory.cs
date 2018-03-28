using System;
using System.Collections.Generic;
using Common.Logging;
using DCommon.Environment.Configuration;

namespace DCommon.NHibernate.Providers
{
    public class DataServicesProviderFactory : IDataServicesProviderFactory
    {
        public ILog Logger { get; set; }

        public DataServicesProviderFactory()
        {
            this.Logger = LogManager.GetLogger<DataServicesProviderFactory>();
        }

        private IDataServicesProvider CreateProviderFromProviderTable(string providerName)
        {
            try
            {
                var providerCreator = DataProviderTable.Providers[providerName];
                if (providerCreator != null)
                {
                    return providerCreator();
                }
            }
            catch (Exception exp)
            {
                Logger.InfoFormat("Create data provider from DataProviderTable occur error.", exp);
            }
            return null;
        }

        private IDataServicesProvider CreateProviderFromConfig()
        {
            try
            {
                var section = DCommon.Configuration.DCommonSection.GetDCommonSection();
                var providerType = section.DataProvider.DataProvider;
                if (providerType != null && typeof(IDataServicesProvider).IsAssignableFrom(providerType))
                {
                    return (IDataServicesProvider)Activator.CreateInstance(providerType);
                }
            }
            catch (Exception exp)
            {
                Logger.InfoFormat("Create data provider from configuration occur error.", exp);
            }
            return null;
        }

        public IDataServicesProvider CreateProvider(ShellSetting setting)
        {
            Logger.DebugFormat("Create data provider, provider name: {0}.", setting.DataProvider);

            IDataServicesProvider provider = null;
            if (!string.IsNullOrEmpty(setting.DataProvider))
            {
                provider = CreateProviderFromProviderTable(setting.DataProvider);
            }

            if (provider == null)
            {
                provider = CreateProviderFromConfig();
            }

            if (provider == null)
            {
                provider = new SQLiteDataServicesProvider();
            }
            Logger.DebugFormat("Create data provider completed. Provider Type: {0}", provider.GetType());

            return provider;
        }
    }
}
