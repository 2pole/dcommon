using System;
using DCommon.Data;
using NHibernate;
using DCommon.Utility;

namespace DCommon.NHibernate
{
    public class NHUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly INHSessionResolver _sessionResolver = new NHSessionResolver();

        /// <summary>
        /// Registers a <see cref="Func{T}"/> of type <see cref="ISessionFactory"/> provider with the unit of work factory.
        /// </summary>
        /// <param name="factoryProvider">A <see cref="Func{T}"/> of type <see cref="ISessionFactory"/> instance.</param>
        public void RegisterSessionFactoryProvider(Func<ISessionFactory> factoryProvider)
        {
            Guard.Against<ArgumentNullException>(factoryProvider == null,
                                                 "Invalid session factory provider registration. " +
                                                 "Expected a non-null Func<ISessionFactory> instance.");
            _sessionResolver.RegisterSessionFactoryProvider(factoryProvider);
        }
        
        public IUnitOfWork Create()
        {
            Guard.Against<InvalidOperationException>(
                 _sessionResolver.SessionFactoriesRegistered == 0,
                 "No session factory providers have been registered. You must register ISessionFactory providers using " +
                 "the RegisterSessionFactoryProvider method or use NCommon.Configure class to configure NCommon.NHibernate " +
                 "using the NHConfiguration class and register ISessionFactory instances using the WithSessionFactory method.");

            return new NHUnitOfWork(_sessionResolver);
        }
    }
}
