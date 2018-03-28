using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using DCommon.Environment.Configuration;
using Cfg = NHibernate.Cfg.Configuration;

namespace DCommon.NHibernate.Providers
{
    public interface IDataServicesProvider
    {
        Cfg BuildConfiguration(ShellSetting setting);
    }
}
