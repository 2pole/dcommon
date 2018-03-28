using DCommon.Environment.Configuration;

namespace DCommon.NHibernate.Providers
{
    public interface IDataServicesProviderFactory
    {
        IDataServicesProvider CreateProvider(ShellSetting sessionFactoryParameters);
    }
}
