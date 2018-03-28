using System;
using NHibernate.Cfg;
using Cfg = NHibernate.Cfg.Configuration;

namespace DCommon.NHibernate
{
    public interface ISessionConfigurationCache
    {
        Cfg GetConfiguration(Func<Cfg> builder);
    }
}