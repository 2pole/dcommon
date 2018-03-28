using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using DCommon.Data;

namespace DCommon.NHibernate
{
    public interface INHSessionResolver : ISessionResolver
    {
        /// <summary>
        /// Gets the <see cref="ISessionFactory"/> that can be used to create instances of <see cref="ISession"/>
        /// to query and update the specified type..
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="ISessionFactory"/> is returned.</typeparam>
        /// <returns>An <see cref="ISessionFactory"/> that can be used to create instances of <see cref="ISession"/>
        /// to query and update the specified type.</returns>
        ISessionFactory GetFactoryFor<T>();

        /// <summary>
        /// Opens a <see cref="ISession"/> instance for a given type.
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="ISession"/> is returned.</typeparam>
        /// <returns>An instance of <see cref="ISession"/>.</returns>
        ISession OpenSessionFor<T>();

        /// <summary>
        /// Registers an <see cref="ISessionFactory"/> provider with the resolver.
        /// </summary>
        /// <param name="factoryProvider">A <see cref="Func{T}"/> of type <see cref="ISessionFactory"/>.</param>
        void RegisterSessionFactoryProvider(Func<ISessionFactory> factoryProvider);

        /// <summary>
        /// Gets the count of <see cref="ISessionFactory"/> providers registered with the resolver.
        /// </summary>
        int SessionFactoriesRegistered { get; }
    }
}
