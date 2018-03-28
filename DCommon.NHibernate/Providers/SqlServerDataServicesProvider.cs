using System;
using DCommon.Environment.Configuration;

//using FluentNHibernate.Cfg.Db;

namespace DCommon.NHibernate.Providers
{
    public class SqlServerDataServicesProvider : AbstractDataServicesProvider
    {
        public static string ProviderName
        {
            get { return "SqlServer"; }
        }

        #region Overrides of AbstractDataServicesProvider

        protected override bool DatabaseExists(ShellSetting setting)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}