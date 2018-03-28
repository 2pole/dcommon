using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DCommon.NHibernate.Providers;

namespace DCommon.NHibernate
{   
    public static class DataProviderTable
    {
        private static readonly DataProviderCollection _instance = new DataProviderCollection();

        public static DataProviderCollection Providers
        {
            get
            {
                return DataProviderTable._instance;
            }
        }

        static DataProviderTable()
        {
            RegisterDefaultProviders();
        }

        private static void RegisterDefaultProviders()
        {
            _instance.Add(SqlServerDataServicesProvider.ProviderName, () => new SqlServerDataServicesProvider());
            _instance.Add(SQLiteDataServicesProvider.ProviderName, () => new SQLiteDataServicesProvider());
            _instance.Add(SqlCeDataServicesProvider.ProviderName, () => new SqlCeDataServicesProvider());
        }
    }

    public class DataProviderCollection
    {
        private readonly Dictionary<string, Func<IDataServicesProvider>> _providerCreators = new Dictionary<string, Func<IDataServicesProvider>>();

        public Func<IDataServicesProvider> this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    return null;
                Func<IDataServicesProvider> providerCreator;
                if (this._providerCreators.TryGetValue(name, out providerCreator))
                    return providerCreator;
                else
                    return null;
            }
        }

        public void Add(string name, Func<IDataServicesProvider> creator)
        {
            if (_providerCreators.ContainsKey(name))
                throw new ArgumentException(string.Format("The data provider:{0} is exists.", name));

            _providerCreators[name] = creator;
        }
    }
}
