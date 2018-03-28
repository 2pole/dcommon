using DCommon.Data;
using NHibernate;

namespace DCommon.NHibernate
{
    public interface INHUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Gets a <see cref="ISession"/> instance that can be used for querying and updating
        /// instances of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type for which a <see cref="ISession"/> is retrieved.</typeparam>
        /// <returns>An instance of <see cref="ISession"/> that can be used for querying and updating
        /// instances of <typeparamref name="T"/></returns>
        ISession GetSession<T>();
    }
}
