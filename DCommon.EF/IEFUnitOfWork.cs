using DCommon.Data;

namespace DCommon.EF
{
    public interface IEFUnitOfWork : IUnitOfWork
    {
        IEFSessionLocator GetSession<T>();
    }
}
